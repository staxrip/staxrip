Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.Runtime.ExceptionServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms.VisualStyles

Imports Microsoft.Win32
Imports StaxRip.UI
Imports VB6 = Microsoft.VisualBasic

Public Class GlobalClass
    Property ProjectPath As String
    Property MainForm As MainForm
    Property ProcForm As ProcForm
    Property MinimizedWindows As Boolean
    Property SavedProject As New Project
    Property DefaultCommands As New GlobalCommands
    Property IsProcessing As Boolean

    Sub WriteDebugLog(value As String)
        If s?.WriteDebugLog Then Trace.TraceInformation(value)
    End Sub

    Sub ProcessJobs()
        Dim jobs = Job.ActiveJobs
        If jobs.Count = 0 Then Exit Sub
        g.IsProcessing = True
        Dim jobPath = jobs(0).Key

        Try
            Job.ActivateJob(jobPath, False)
            g.MainForm.OpenProject(jobPath, False)
            If s.PreventStandby Then PowerRequest.SuppressStandby()
            ProcessJob(jobPath)
            jobs = Job.GetJobs

            If jobs.Count = 0 Then
                g.RaiseAppEvent(ApplicationEvent.JobsEncoded)
                g.ShutdownPC()
            ElseIf Job.ActiveJobs.Count = 0 Then
                If Process.GetProcessesByName("StaxRip").Count = 1 Then
                    g.RaiseAppEvent(ApplicationEvent.JobsEncoded)
                    g.ShutdownPC()
                End If
            Else
                If Process.GetCurrentProcess.PrivateMemorySize64 / 1024 ^ 2 > 1500 Then
                    g.StartProcess(Application.ExecutablePath, "-StartJobs")
                    g.MainForm.SetSavedProject()
                    g.MainForm.Close()
                Else
                    ProcessJobs()
                End If
            End If
        Catch ex As AbortException
            Log.Save()
            g.MainForm.OpenProject(g.ProjectPath, False)
        Catch ex As Exception
            Log.Save()
            g.OnException(ex)
        Finally
            If s.PreventStandby Then PowerRequest.EnableStandby()
            g.IsProcessing = False
            g.MainForm.OpenProject(jobPath, False)
            ProcController.Finished()
        End Try
    End Sub

    Sub ProcessJob(jobPath As String)
        Try
            g.RaiseAppEvent(ApplicationEvent.BeforeJobProcessed)
            Dim startTime = DateTime.Now

            If p.BatchMode Then
                g.MainForm.OpenVideoSourceFiles(p.SourceFiles, True)
                g.ProjectPath = p.TempDir + p.SourceFile.Base + ".srip"
                g.MainForm.SaveProjectPath(g.ProjectPath)
                p.BatchMode = False
            End If

            Log.WriteHeader(If(p.Script.Engine = ScriptEngine.AviSynth, "AviSynth Script", "VapourSynth Script"))
            Log.WriteLine(p.Script.GetFullScript)
            Log.WriteHeader("Script Properties")

            Dim props =
                "Source Frame Count: " & p.SourceScript.GetFrames & BR +
                "Source Frame Rate: " & p.SourceScript.GetFramerate.ToString("f6", CultureInfo.InvariantCulture) + BR +
                "Source Duration: " + TimeSpan.FromSeconds(g.Get0ForInfinityOrNaN(p.SourceScript.GetFrames / p.SourceScript.GetFramerate)).ToString + BR +
                "Target Frame Count: " & p.Script.GetFrames & BR +
                "Target Frame Rate: " & p.Script.GetFramerate.ToString("f6", CultureInfo.InvariantCulture) + BR +
                "Target Duration: " + TimeSpan.FromSeconds(g.Get0ForInfinityOrNaN(p.Script.GetFrames / p.Script.GetFramerate)).ToString

            Log.WriteLine(props.FormatColumn(":"))

            g.MainForm.Hide()

            Dim actions As New List(Of Action)

            If p.SkipAudioEncoding AndAlso File.Exists(p.Audio0.GetOutputFile) Then
                p.Audio0.File = p.Audio0.GetOutputFile()
            Else
                actions.Add(Sub()
                                Audio.Process(p.Audio0)
                                p.Audio0.Encode()
                            End Sub)
            End If

            If p.SkipAudioEncoding AndAlso File.Exists(p.Audio1.GetOutputFile) Then
                p.Audio1.File = p.Audio1.GetOutputFile()
            Else
                actions.Add(Sub()
                                Audio.Process(p.Audio1)
                                p.Audio1.Encode()
                            End Sub)
            End If

            For Each i In p.AudioTracks
                Dim temp = i

                If p.SkipAudioEncoding AndAlso File.Exists(i.GetOutputFile) Then
                    i.File = i.GetOutputFile()
                Else
                    actions.Add(Sub()
                                    Audio.Process(temp)
                                    temp.Encode()
                                End Sub)
                End If
            Next

            actions.Add(Sub() Subtitle.Cut(p.VideoEncoder.Muxer.Subtitles))
            actions.Add(AddressOf ProcessVideo)

            Try
                Parallel.Invoke(New ParallelOptions With {.MaxDegreeOfParallelism = s.ParallelProcsNum}, actions.ToArray)
            Catch ex As AggregateException
                ExceptionDispatchInfo.Capture(ex.InnerExceptions(0)).Throw()
            End Try

            Log.Save()
            p.VideoEncoder.Muxer.Mux()

            If p.SaveThumbnails Then Thumbnails.SaveThumbnails(p.TargetFile, p)
            If p.MTN Then MTN.Thumbnails(p.TargetFile, p)
            If p.MKVHDR Then MKVInfo.MetadataHDR(p.TargetFile, p)

            Log.WriteHeader("Job Complete")
            Log.WriteStats(startTime)
            Log.Save()

            g.ArchiveLogFile(Log.GetPath)
            g.DeleteTempFiles()
            g.RaiseAppEvent(ApplicationEvent.JobProcessed)
            Job.RemoveJob(jobPath)
            If jobPath.StartsWith(Folder.Settings + "Batch Projects\") Then File.Delete(jobPath)
        Catch ex As ErrorAbortException
            Log.Save()
            g.ShowException(ex, Nothing, 50)
            g.StartProcess(g.GetTextEditor(), """" + p.TempDir + p.TargetFile.Base + "_staxrip.log" + """")
            ProcController.Aborted = False
        End Try
    End Sub

    Sub ProcessVideo()
        If Not (p.SkipVideoEncoding AndAlso Not TypeOf p.VideoEncoder Is NullEncoder AndAlso File.Exists(p.VideoEncoder.OutputPath)) Then
            Dim originalFilters As List(Of VideoFilter)
            Dim originalSource As String

            If p.PreRenderIntoLossless AndAlso Not TypeOf p.VideoEncoder Is NullEncoder Then
                Dim outPath = p.TempDir + p.TargetFile.Base + "_lossless.avi"

                If p.Script.Engine = ScriptEngine.AviSynth Then
                    Using proc As New Proc
                        proc.Header = "Pre-render into lossless AVI"
                        proc.SkipStrings = {"frame=", "size="}
                        proc.Encoding = Encoding.UTF8
                        proc.Package = Package.ffmpeg
                        proc.Arguments = "-i " + p.Script.Path.Escape + " -c:v utvideo -pred median -sn -an -y -hide_banner " + outPath.Escape
                        proc.Start()
                    End Using
                Else
                    Dim commandLine = Package.vspipe.Path.Escape + " " + p.Script.Path.Escape + " - --y4m | " + Package.ffmpeg.Path.Escape + " -i - -c:v utvideo -pred median -sn -an -y -hide_banner " + outPath.Escape

                    Using proc As New Proc
                        proc.Header = "Pre-render into lossless AVI"
                        proc.SkipStrings = {"frame=", "size=", "Multiple"}
                        proc.Encoding = Encoding.UTF8
                        proc.Package = Package.ffmpeg
                        proc.File = "cmd.exe"
                        proc.Arguments = "/S /C call """ + commandLine + """"
                        proc.Start()
                    End Using
                End If

                If File.Exists(outPath) Then
                    Log.WriteHeader("Lossless AVI MediaInfo")
                    Log.WriteLine(MediaInfo.GetSummary(outPath))
                Else
                    Throw New ErrorAbortException("Pre-render failed", "Output file is missing")
                End If

                originalSource = p.SourceFile
                p.SourceFile = p.TempDir + p.TargetFile.Base + "_lossless.avi"
                originalFilters = p.Script.GetFiltersCopy()

                For Each i In p.Script.Filters
                    If i.Category = "Source" Then
                        If p.Script.Engine = ScriptEngine.AviSynth Then
                            i.Script = "FFVideoSource(""" + outPath + """)"
                        Else
                            i.Script = "clip = core.ffms2.Source(r""" + outPath + """)"
                        End If
                    Else
                        i.Active = False
                    End If
                Next

                p.Script.Synchronize()
            End If

            If Not originalFilters Is Nothing Then
                p.SourceFile = originalSource
                p.Script.Filters = originalFilters
                p.Script.Synchronize()
            End If

            p.VideoEncoder.Encode()
            g.RaiseAppEvent(ApplicationEvent.VideoEncoded)
        End If
    End Sub

    Sub DeleteTempFiles()
        If s.DeleteTempFilesMode <> DeleteMode.Disabled AndAlso p.TempDir.EndsWith("_temp\") Then
            Try
                Dim moreJobsToProcessInTempDir = Job.GetJobs.Where(Function(a) a.Value AndAlso a.Key.Contains(p.TempDir))

                If moreJobsToProcessInTempDir.Count = 0 Then
                    If s.DeleteTempFilesMode = DeleteMode.RecycleBin Then
                        DirectoryHelp.Delete(p.TempDir, VB6.FileIO.RecycleOption.SendToRecycleBin)
                    Else
                        DirectoryHelp.Delete(p.TempDir)
                    End If
                End If
            Catch
            End Try
        End If
    End Sub

    ReadOnly Property StartupTemplatePath() As String
        Get
            Dim ret = Folder.Template + s.StartupTemplate + ".srip"

            If Not File.Exists(ret) Then
                ret = Folder.Template + "Automatic Workflow.srip"
                s.StartupTemplate = "Automatic Workflow"
            End If

            Return ret
        End Get
    End Property

    ReadOnly Property SettingsFile() As String
        Get
            Return Folder.Settings + "Settings.dat"
        End Get
    End Property

    Function BrowseFolder(defaultFolder As String) As String
        Using d As New FolderBrowserDialog
            d.SetSelectedPath(defaultFolder)
            If d.ShowDialog = DialogResult.OK Then Return d.SelectedPath
        End Using
    End Function

    Function VerifyRequirements() As Boolean
        For Each pack In Package.Items.Values
            If Not pack.VerifyOK Then Return False
        Next

        If Not p.Script.IsFilterActive("Source") Then
            MsgWarn("No active filter of category 'Source' found.")
            Return False
        End If

        Return True
    End Function

    Function ShowVideoSourceWarnings(files As IEnumerable(Of String)) As Boolean
        For Each i In files
            If Not i.IsANSICompatible AndAlso p.Script.Engine = ScriptEngine.AviSynth Then
                MsgError(Strings.NoUnicode)
                Return True
            End If

            If i.Contains("#") Then
                If i.Ext = "mp4" OrElse MediaInfo.GetGeneral(i, "Audio_Codec_List").Contains("AAC") Then
                    MsgError("Character # can't be processed by MP4Box, please rename." + BR2 + i)
                    Return True
                End If
            End If

            If i.Ext = "dga" Then
                MsgError("There is no properly working x64 source filters available for DGA. There are several newer and faster x64 source filters available.")
                Return True
            End If

            If i.Ext = "dgi" AndAlso File.ReadAllText(i).Contains("DGIndexIM") Then
                MsgError("Please rename the file extension from dgi to dgim.")
                Return True
            End If
        Next
    End Function

    Function Get0ForInfinityOrNaN(arg As Double) As Double
        If Double.IsNaN(arg) OrElse Double.IsInfinity(arg) Then
            Return 0
        Else
            Return arg
        End If
    End Function

    Sub PlayAudio(ap As AudioProfile)
        If FileTypes.AudioRaw.Contains(ap.File.Ext) Then
            g.StartProcess(Package.mpvnet.Path, ap.File.Escape)
        ElseIf ap.File = p.FirstOriginalSourceFile AndAlso ap.Streams.Count > 0 Then
            g.StartProcess(Package.mpvnet.Path, "--audio=" & (ap.Stream.Index + 1) & " " + p.FirstOriginalSourceFile.Escape)
        ElseIf FileTypes.Audio.Contains(ap.File.Ext) Then
            g.StartProcess(Package.mpvnet.Path, "--audio-delay=" + (g.ExtractDelay(ap.File) / 1000).ToInvariantString.Shorten(9) + " --audio-files=" + ap.File.Escape + " " + p.FirstOriginalSourceFile.Escape)
        Else
            MsgError("Unable to play audio.")
        End If
    End Sub

    Sub PlayScript(doc As VideoScript)
        If File.Exists(p.Audio0.File) AndAlso FileTypes.Audio.Contains(p.Audio0.File.Ext) Then
            PlayScript(doc, p.Audio0)
        ElseIf File.Exists(p.Audio1.File) AndAlso FileTypes.Audio.Contains(p.Audio1.File.Ext) Then
            PlayScript(doc, p.Audio1)
        Else
            PlayScript(doc, Nothing)
        End If
    End Sub

    Sub PlayScript(doc As VideoScript, ap As AudioProfile)
        If Not Package.mpvnet.VerifyOK(True) Then Exit Sub

        If doc.Engine = ScriptEngine.VapourSynth Then
            MsgError("VapourSynth scripts are not supported by the mpv player.")
            Exit Sub
        End If

        Dim script As New VideoScript
        script.Engine = doc.Engine
        script.Path = p.TempDir + p.TargetFile.Base + "_play." + script.FileType
        script.Filters = doc.GetFiltersCopy

        If script.Engine = ScriptEngine.AviSynth Then
            If Calc.IsARSignalingRequired Then
                Dim targetWidth = CInt((p.TargetHeight * Calc.GetTargetDAR) / 4) * 4
                script.Filters.Add(New VideoFilter("LanczosResize(" & targetWidth & "," & p.TargetHeight & ")"))
            End If
        Else
            If Calc.IsARSignalingRequired Then
                Dim targetWidth = CInt((p.TargetHeight * Calc.GetTargetDAR) / 4) * 4
                script.Filters.Add(New VideoFilter("clip = core.resize.Bicubic(clip, " & targetWidth & "," & p.TargetHeight & ")"))
            End If
        End If

        script.Synchronize(False)
        Dim args = script.Path.Escape
        If Not ap Is Nothing AndAlso FileTypes.Audio.Contains(ap.File.Ext) Then args = "--audio-files=" + ap.File.Escape + " " + args
        g.StartProcess(Package.mpvnet.Path, args)
    End Sub

    Function ExtractDelay(value As String) As Integer
        Dim match = Regex.Match(value, " (-?\d+)ms")
        If match.Success Then Return CInt(match.Groups(1).Value)
    End Function

    Sub ShowCode(title As String, content As String)
        Dim f As New HelpForm()
        f.Doc.WriteStart(title)
        f.Doc.Writer.WriteRaw("<pre><code>" + content + "</pre></code>")
        f.Show()
    End Sub

    Sub ShowHelp(title As String, content As String)
        If title <> "" Then title = title.TrimEnd("."c, ":"c)
        MsgInfo(title, content)
    End Sub

    Sub PopulateProfileMenu(
            ic As ToolStripItemCollection,
            profiles As IList,
            dialogAction As Action,
            loadAction As Action(Of Profile))

        For Each iProfile As Profile In profiles
            Dim a = iProfile.Name.SplitNoEmpty("|")
            Dim l = ic

            For i = 0 To a.Length - 1
                Dim found = False

                For Each iItem As ToolStripItem In l
                    If i < a.Length - 1 Then
                        If iItem.Text = a(i) Then
                            found = True
                            l = DirectCast(iItem, ToolStripMenuItem).DropDownItems
                        End If
                    End If
                Next

                If Not found Then
                    If i = a.Length - 1 Then
                        Dim item As New ActionMenuItem(a(i), Sub() loadAction(iProfile))
                        l.Add(item)
                        l = item.DropDownItems
                    Else
                        Dim item As New MenuItemEx(a(i))
                        l.Add(item)
                        l = item.DropDownItems
                    End If
                End If
            Next
        Next

        If Not dialogAction Is Nothing Then
            ic.Add(New ToolStripSeparator)
            ic.Add(New ActionMenuItem("Edit Profiles...", dialogAction, "Opens the profiles editor"))
        End If
    End Sub

    Function GetAutoSize(percentage As Integer) As Integer
        Dim ret As Integer
        Dim size = p.TargetSize
        Dim bitrate = p.VideoBitrate

        For i = 1 To 100000
            p.TargetSize = i
            p.VideoBitrate = CInt(Calc.GetVideoBitrate)

            If CInt(Calc.GetPercent) >= percentage Then
                ret = i
                Exit For
            End If
        Next

        p.TargetSize = size
        p.VideoBitrate = bitrate

        If ret = 0 Then ret = size

        Return ret
    End Function

    Function GetPreviewPosMS() As Integer
        Dim fr = p.Script.GetFramerate
        If fr = 0 Then fr = 25
        Return CInt((s.LastPosition / fr) * 1000)
    End Function

    Function GetTextEditor() As String
        Dim ret = GetAssociatedApplication(".txt")
        If ret <> "" Then Return ret
        Return "notepad.exe"
    End Function

    Function GetAssociatedApplication(ext As String) As String
        Dim c = 0UI

        'ASSOCF_VERIFY, ASSOCSTR_EXECUTABLE
        If 1 = Native.AssocQueryString(&H40, 2, ext, Nothing, Nothing, c) Then
            If c > 0 Then
                Dim sb As New StringBuilder(CInt(c))

                'ASSOCF_VERIFY, ASSOCSTR_EXECUTABLE
                If 0 = Native.AssocQueryString(&H40, 2, ext, Nothing, sb, c) Then
                    Dim ret = sb.ToString
                    If File.Exists(ret) Then Return ret
                End If
            End If
        End If
    End Function

    Sub SaveSettings()
        Try
            SafeSerialization.Serialize(s, g.SettingsFile)
            Dim backupPath = Folder.Settings + "Backup\"
            If Not Directory.Exists(backupPath) Then Directory.CreateDirectory(backupPath)
            FileHelp.Copy(g.SettingsFile, backupPath + "Settings(" + Application.ProductVersion + ").dat")
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    Public Sub LoadVideoEncoder(profile As Profile)
        Dim currentMuxer = p.VideoEncoder.Muxer
        p.VideoEncoder = DirectCast(ObjectHelp.GetCopy(profile), VideoEncoder)

        If currentMuxer.IsSupported(p.VideoEncoder.OutputExt) Then
            p.VideoEncoder.Muxer = currentMuxer
        Else
            p.VideoEncoder.Muxer.Init()
        End If

        MainForm.tbTargetFile.Text = p.TargetFile.ChangeExt(p.VideoEncoder.Muxer.OutputExt)
        p.VideoEncoder.OnStateChange()
        p.VideoEncoder.SetMetaData(p.LastOriginalSourceFile)
        MainForm.RecalcBitrate()
        MainForm.Assistant()
    End Sub

    Public Sub LoadAudioProfile0(profile As Profile)
        Dim file = p.Audio0.File
        Dim delay = p.Audio0.Delay
        Dim language = p.Audio0.Language
        Dim stream = p.Audio0.Stream
        Dim streams = p.Audio0.Streams
        p.Audio0 = DirectCast(ObjectHelp.GetCopy(profile), AudioProfile)
        p.Audio0.File = file
        p.Audio0.Language = language
        p.Audio0.Stream = stream
        p.Audio0.Streams = streams
        p.Audio0.Delay = delay
        g.MainForm.llAudioProfile0.Text = g.ConvertPath(p.Audio0.Name)
        g.MainForm.UpdateSizeOrBitrate()
        g.MainForm.Assistant()
    End Sub

    Public Sub LoadAudioProfile1(profile As Profile)
        Dim file = p.Audio1.File
        Dim delay = p.Audio1.Delay
        Dim language = p.Audio1.Language
        Dim stream = p.Audio1.Stream
        Dim streams = p.Audio1.Streams
        p.Audio1 = DirectCast(ObjectHelp.GetCopy(profile), AudioProfile)
        p.Audio1.File = file
        p.Audio1.Language = language
        p.Audio1.Stream = stream
        p.Audio1.Streams = streams
        p.Audio1.Delay = delay
        g.MainForm.llAudioProfile1.Text = g.ConvertPath(p.Audio1.Name)
        g.MainForm.UpdateSizeOrBitrate()
        g.MainForm.Assistant()
    End Sub

    Sub RaiseAppEvent(appEvent As ApplicationEvent)
        Dim scriptPath = Folder.Settings + "Scripts\" + appEvent.ToString + ".ps1"
        If File.Exists(scriptPath) Then g.DefaultCommands.ExecuteScriptFile(scriptPath)

        For Each i In s.EventCommands
            If i.Enabled AndAlso i.Event = appEvent Then
                Dim matches = 0

                For Each i2 In i.CriteriaList
                    i2.PropertyString = Macro.Expand(i2.Macro)
                    If i2.Eval Then matches += 1
                Next

                If (i.CriteriaList.Count = 0 OrElse (i.OrOnly AndAlso matches > 0) OrElse
                    (Not i.OrOnly AndAlso matches = i.CriteriaList.Count)) AndAlso
                    Not i.CommandParameters Is Nothing Then

                    Dim command = g.MainForm.CustomMainMenu.CommandManager.GetCommand(i.CommandParameters.MethodName)

                    If p.SourceFile <> "" Then
                        Log.WriteHeader("Event Command " + i.Name)
                        Log.WriteLine("Event: " + DispNameAttribute.GetValueForEnum(i.Event))
                        Log.WriteLine("Command: " + command.MethodInfo.Name)
                        Log.WriteLine(command.GetParameterHelp(i.CommandParameters.Parameters))
                    End If

                    g.MainForm.CustomMainMenu.CommandManager.Process(i.CommandParameters)
                End If
            End If
        Next
    End Sub

    Sub SetTempDir()

        If p.SourceFile <> "" Then
            p.TempDir = Macro.Expand(p.TempDir)
            If p.TempDir = "" Then
                Try
                    If p.SourceFile.Dir.EndsWith("_temp\") Then
                        p.TempDir = p.SourceFile.Dir
                    Else
                        Dim base = p.SourceFile.Base
                        p.TempDir = p.SourceFile.Dir + base + "_temp\"
                    End If
                Catch ex As PathTooLongException
                    If p.SourceFile.Dir.EndsWith("_temp\") Then
                        p.TempDir = p.SourceFile.Dir
                    Else
                        Dim base = p.SourceFile.Base
                        If base.Length > 30 Then base = base.Shorten(15) + "..."
                        p.TempDir = p.SourceFile.Dir + base + "_temp\"
                    End If
                End Try
            End If

            'Source Code Running Windows 7 & 8.1(Saved Just Incase):

            'If p.SourceFile.Dir.EndsWith("_temp\") Then
            '    p.TempDir = p.SourceFile.Dir
            'Else
            '    Dim base = p.SourceFile.Base
            '    If base.Length > 30 Then base = base.Shorten(15) + "..."
            '    p.TempDir = p.SourceFile.Dir + base + "_temp\"
            'End If


            p.TempDir = p.TempDir.FixDir

            If Not Directory.Exists(p.TempDir) Then
                Try
                    Directory.CreateDirectory(p.TempDir)
                Catch
                    Try
                        p.TempDir = p.SourceFile.DirAndBase + "_temp\"
                        If Not Directory.Exists(p.TempDir) Then Directory.CreateDirectory(p.TempDir)
                    Catch
                        MsgWarn("Failed to create a temp directory. By default it's created in the directory of the source file so it's not possible to open files directly from a optical drive unless a temp directory is defined in the options. Usually discs are copied to the hard drive first using a application like MakeMKV, DVDfab or AnyDVD.")
                        Throw New AbortException
                    End Try
                End Try
            End If
        End If
    End Sub

    Sub ShowCommandLinePreview(title As String, value As String)
        Using f As New StringEditorForm
            f.Text = title
            f.rtb.ReadOnly = True
            f.cbWrap.Checked = Not value.Contains(BR)
            f.rtb.Text = value
            f.bnOK.Visible = False
            f.bnCancel.Text = "Close"
            f.ShowDialog()
        End Using
    End Sub

    Sub ffmsindex(sourcePath As String,
                  cachePath As String,
                  Optional indexAudio As Boolean = False,
                  Optional proj As Project = Nothing)

        If File.Exists(sourcePath) AndAlso Not File.Exists(cachePath) AndAlso
            Not FileTypes.VideoText.Contains(sourcePath.Ext) Then

            Using proc As New Proc
                proc.Header = "Indexing using ffmsindex"
                proc.SkipString = "Indexing, please wait..."
                If proj Is Nothing Then proj = p
                proc.Project = proj
                proc.File = Package.ffms2.GetDir + "ffmsindex.exe"
                proc.Arguments = If(indexAudio, "-t -1 ", "") + sourcePath.Escape + " " + cachePath.Escape
                proc.Start()
            End Using
        End If
    End Sub

    Function IsValidSource(Optional warn As Boolean = True) As Boolean
        If p.SourceScript.GetFrames = 0 Then
            If warn Then
                MsgWarn("Failed to load source.")
            End If

            Return False
        End If

        If Not p.SourceScript.GetErrorMessage Is Nothing Then
            MsgError(p.SourceScript.GetErrorMessage)
            Return False
        End If

        Return True
    End Function

    Function IsSourceSameOrSimilar(path As String) As Boolean
        Return IsSourceSame(path) OrElse IsSourceSimilar(path)
    End Function

    Function IsCOMObjectRegistered(guid As String) As Boolean
        Return File.Exists(Registry.ClassesRoot.GetString("CLSID\" + guid + "\InprocServer32", Nothing))
    End Function

    Function IsSourceSame(path As String) As Boolean
        Return FilePath.GetBase(path).StartsWith(FilePath.GetBase(p.SourceFile))
    End Function

    Function GetFilesInTempDirAndParent() As List(Of String)
        Dim ret As New List(Of String)
        Dim dirs As New HashSet(Of String)

        If p.TempDir <> "" Then dirs.Add(p.TempDir)
        If p.TempDir?.EndsWith("_temp\") Then dirs.Add(DirPath.GetParent(p.TempDir))
        dirs.Add(FilePath.GetDir(p.FirstOriginalSourceFile))

        For Each i In dirs
            ret.AddRange(Directory.GetFiles(i))
        Next

        Return ret
    End Function

    Function IsSourceSimilar(path As String) As Boolean
        If p.SourceFile.Contains("_") Then
            Dim src = FilePath.GetBase(p.SourceFile)

            While src.Length > 2 AndAlso src.ToCharArray.Last.IsDigit
                src = src.DeleteRight(1)
            End While

            If src.EndsWith("_") AndAlso FilePath.GetBase(path).StartsWith(src.TrimEnd("_"c)) Then
                Return True
            End If
        End If
    End Function

    Function IsCulture(twoLetterCode As String) As Boolean
        Return CultureInfo.CurrentCulture.TwoLetterISOLanguageName = twoLetterCode
    End Function

    Private ExceptionHandled As Boolean

    Sub OnException(ex As Exception)
        If ExceptionHandled Then
            Exit Sub
        Else
            ExceptionHandled = True
        End If

        If Not TypeOf ex Is AbortException Then
            Try
                If File.Exists(p.SourceFile) Then
                    Dim name = p.TargetFile.Base
                    If name = "" Then name = p.SourceFile.Base
                    Dim path = FilePath.GetDir(p.SourceFile) + "recovery.srip"
                    g.MainForm.SaveProjectPath(path)
                End If

                g.SaveSettings()
            Catch
            End Try

            Try
                ShowException(ex)
                MakeBugReport(ex)
            Catch
            End Try

            Process.GetCurrentProcess.Kill()
        End If
    End Sub

    Sub ShowException(e As Exception,
                      Optional msg As String = Nothing,
                      Optional timeout As Integer = 0)
        Try
            Using td As New TaskDialog(Of String)
                If msg = "" Then
                    If TypeOf e Is ErrorAbortException Then
                        td.MainInstruction = DirectCast(e, ErrorAbortException).Title + $" ({Application.ProductVersion})"
                    Else
                        td.MainInstruction = e.GetType.Name + $" ({Application.ProductVersion})"
                    End If
                Else
                    td.MainInstruction = msg
                End If

                td.Timeout = timeout
                td.Content = e.Message
                td.MainIcon = TaskDialogIcon.Error
                td.ExpandedInformation = e.ToString
                td.Footer = Strings.TaskDialogFooter
                td.Show()
            End Using
        Catch
            Dim title As String

            If TypeOf e Is ErrorAbortException Then
                title = DirectCast(e, ErrorAbortException).Title
            Else
                title = e.GetType.Name
            End If

            VB6.MsgBox(title + BR2 + e.Message + BR2 + e.ToString, VB6.MsgBoxStyle.Critical)
        End Try
    End Sub

    Sub ArchiveLogFile(path As String)
        Try
            Dim logFolder = Folder.Settings + "Log Files\"
            If Not Directory.Exists(logFolder) Then Directory.CreateDirectory(logFolder)
            FileHelp.Copy(path, logFolder + Date.Now.ToString("yyyy-MM-dd_HH.mm.ss") + " - " + path.FileName)
            Dim di As New DirectoryInfo(logFolder)

            While di.GetFiles("*.log").Length > s.LogFileNum
                FileHelp.Delete(di.GetFiles("*.log").OrderBy(Function(val) val.LastWriteTime).First.FullName)
            End While
        Catch ex As Exception
            ShowException(ex, "Failed to archive log file")
        End Try
    End Sub

    Sub SetRenderer(ms As ToolStrip)
        If VisualStyleInformation.IsEnabledByUser Then
            ms.Renderer = New ToolStripRendererEx(s.ToolStripRenderModeEx)
        Else
            ms.Renderer = New ToolStripSystemRenderer()
        End If
    End Sub

    Sub Play(file As String)
        g.StartProcess(Package.mpvnet.Path.Escape, file.Escape)
    End Sub

    Sub ShowCommandLineHelp(package As Package, switch As String)
        Dim helpPath = package.GetHelpPath
        If Not File.Exists(helpPath) Then Exit Sub
        Dim helpContent = File.ReadAllText(helpPath)
        Dim find As String

        If helpContent.Contains(switch) Then
            find = switch
        ElseIf helpContent.Contains(switch.Replace("--", "--(no-)")) Then
            find = switch.Replace("--", "--(no-)")
        End If

        If find = "" Then Exit Sub

        Dim form As New TextHelpForm(helpContent, find)
        form.Text = package.Name + " Help"
        form.Show()
    End Sub

    Sub StartProcess(cmd As String, Optional args As String = Nothing)
        Try
            Process.Start(cmd, args)
        Catch ex As Exception
            If cmd Like "http*://*" Then
                MsgError("Failed to open URL with browser." + BR2 + cmd, ex.Message)
            ElseIf File.Exists(cmd) Then
                MsgError("Failed to launch file." + BR2 + cmd, ex.Message)
            ElseIf Directory.Exists(cmd) Then
                MsgError("Failed to launch directory." + BR2 + cmd, ex.Message)
            Else
                g.ShowException(ex, "Failed to execute command:" + BR2 + cmd + BR2 + "Arguments:" + BR2 + args)
            End If
        End Try
    End Sub

    Sub OpenDirAndSelectFile(filepath As String, handle As IntPtr)
        If File.Exists(filepath) Then
            g.StartProcess(StaxRip.FilePath.GetDir(filepath))

            Try
                For x = 0 To 9
                    Thread.Sleep(300)
                    Application.DoEvents()

                    If handle <> Native.GetForegroundWindow Then
                        ExplorerHelp.SelectFile(Native.GetForegroundWindow, filepath)
                        Exit For
                    End If
                Next
            Catch
            End Try
        ElseIf Directory.Exists(StaxRip.FilePath.GetDir(filepath)) Then
            g.StartProcess(StaxRip.FilePath.GetDir(filepath))
        End If
    End Sub

    Sub OnUnhandledException(sender As Object, e As ThreadExceptionEventArgs)
        OnException(e.Exception)
    End Sub

    Sub OnUnhandledException(sender As Object, e As UnhandledExceptionEventArgs)
        OnException(DirectCast(e.ExceptionObject, Exception))
    End Sub

    Private IconValue As Icon
    Private LastIconFile As String

    Public Property Icon As Icon
        Get
            If IconValue Is Nothing OrElse s.IconFile <> LastIconFile Then
                If File.Exists(s.IconFile) Then
                    IconValue = New Icon(s.IconFile)
                    LastIconFile = s.IconFile
                Else
                    IconValue = My.Resources.Black
                End If
            End If

            Return IconValue
        End Get
        Set(ByVal value As Icon)
            IconValue = value
        End Set
    End Property

    Sub MakeBugReport(e As Exception)
        If e Is Nothing AndAlso Not g.IsValidSource(False) Then Exit Sub

        If Not e Is Nothing Then
            If Log.IsEmpty Then Log.WriteEnvironment()
            Log.WriteHeader("Exception")
            Log.WriteLine(e.ToString)
        End If

        Log.Save(p)
        Dim fp = Log.GetPath
        g.OpenDirAndSelectFile(fp, g.MainForm.Handle)
        g.StartProcess(g.GetTextEditor(), """" + fp + """")
        g.StartProcess("https://github.com/staxrip/staxrip/issues")
    End Sub

    Function FileExists(path As String) As Boolean
        For x = 1 To 6
            If File.Exists(path) Then Return True
            Thread.Sleep(500)
        Next
    End Function

    Sub ShutdownPC()
        ShutdownPC(CType(Registry.CurrentUser.GetInt("Software\" + Application.ProductName, "ShutdownMode"), ShutdownMode))
    End Sub

    Sub ShutdownPC(mode As ShutdownMode)
        If mode <> ShutdownMode.Nothing Then
            g.SavedProject = p
            g.MainForm.Close()
            Registry.CurrentUser.Write("Software\" + Application.ProductName, "ShutdownMode", 0)
            Shutdown.Commit(mode)
        End If
    End Sub

    Sub Highlight(highlight As Boolean, c As Control)
        If highlight Then
            c.BackColor = Color.Orange
        Else
            If TypeOf c Is Label OrElse TypeOf c Is GroupBox Then
                c.BackColor = SystemColors.Control
            ElseIf TypeOf c Is TextBox AndAlso DirectCast(c, TextBox).ReadOnly Then
                c.BackColor = SystemColors.Control
            Else
                c.BackColor = SystemColors.Window
            End If
        End If
    End Sub

    Function EnableFilter(search As String) As Boolean
        Dim searchLower = search.ToLower

        For Each i In p.Script.Filters
            If i.Script.Contains(search) OrElse i.Script.Contains(searchLower) Then
                If Not i.Active Then
                    i.Active = True
                    g.MainForm.FiltersListView.Load()
                End If

                Return True
            End If
        Next
    End Function

    Function BrowseFile(filter As String) As String
        Using d As New OpenFileDialog
            d.Filter = filter
            d.SetInitDir(p.TempDir)
            If d.ShowDialog = DialogResult.OK Then Return d.FileName
        End Using
    End Function

    Sub CodePreview(code As String)
        Using f As New StringEditorForm
            f.rtb.ReadOnly = True
            f.cbWrap.Checked = False
            f.cbWrap.Visible = False
            f.rtb.Text = code
            f.Text = "Code Preview"
            f.bnOK.Visible = False
            f.bnCancel.Text = "Close"
            f.ShowDialog()
        End Using
    End Sub

    Sub ShowDirectShowWarning()
        If Not p.BatchMode Then
            If Not g.IsCOMObjectRegistered(GUIDS.LAVSplitter) OrElse
                Not g.IsCOMObjectRegistered(GUIDS.LAVVideoDecoder) Then

                MsgError("DirectShow Filter Setup",
                         "An error occurred that could possibly be solved by installing [http://code.google.com/p/lavfilters LAV Filters].")
            End If
        End If
    End Sub

    Sub AddHardcodedSubtitle()
        For Each subtitle In p.VideoEncoder.Muxer.Subtitles
            If subtitle.Path.Ext.EqualsAny("srt", "ass", "idx") Then
                If subtitle.Enabled Then
                    subtitle.Enabled = False
                    p.AddHardcodedSubtitleFilter(subtitle.Path, False)
                    Exit Sub
                End If
            End If
        Next
    End Sub

    Sub RunAutoCrop()
        g.WriteDebugLog("AutoCrop start")
        p.SourceScript.Synchronize(True)

        Using avi As New AVIFile(p.SourceScript.Path)
            Dim segmentCount = 20

            Dim len = avi.FrameCount \ (segmentCount + 1)
            Dim crops(segmentCount - 1) As AutoCrop

            For x = 1 To segmentCount
                avi.Position = len * x

                Using bmp = avi.GetBitmap
                    crops(x - 1) = AutoCrop.Start(bmp.Clone(New Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format32bppRgb), avi.Position)
                End Using
            Next

            Dim leftCrops = crops.SelectMany(Function(arg) arg.Left).OrderBy(Function(arg) arg)
            p.CropLeft = leftCrops(leftCrops.Count \ 10)

            Dim topCrops = crops.SelectMany(Function(arg) arg.Top).OrderBy(Function(arg) arg)
            p.CropTop = topCrops(topCrops.Count \ 10)

            Dim rightCrops = crops.SelectMany(Function(arg) arg.Right).OrderBy(Function(arg) arg)
            p.CropRight = rightCrops(rightCrops.Count \ 10)

            Dim bottomCrops = crops.SelectMany(Function(arg) arg.Bottom).OrderBy(Function(arg) arg)
            p.CropBottom = bottomCrops(bottomCrops.Count \ 10)

            CorrectCropMod()
        End Using

        g.WriteDebugLog("AutoCrop end")
    End Sub

    Sub SmartCrop()
        If Not p.Script.IsFilterActive("Resize") Then
            Exit Sub
        End If

        Dim tempLeft = p.CropLeft
        Dim tempRight = p.CropRight

        Dim ae = Math.Abs(Calc.GetAspectRatioError)

        While (p.SourceWidth - p.CropLeft - p.CropRight) > 64
            p.CropLeft += 2
            p.CropRight += 2

            If Math.Abs(Calc.GetAspectRatioError()) < ae Then
                tempLeft = p.CropLeft
                tempRight = p.CropRight
                ae = Math.Abs(Calc.GetAspectRatioError())
            End If
        End While

        p.CropLeft = tempLeft
        p.CropRight = tempRight

        Dim tempTop = p.CropTop
        Dim tempBottom = p.CropBottom

        While (p.SourceHeight - p.CropTop - p.CropBottom) > 64
            p.CropTop += 2
            p.CropBottom += 2

            If Math.Abs(Calc.GetAspectRatioError()) < ae Then
                tempTop = p.CropTop
                tempBottom = p.CropBottom
                ae = Math.Abs(Calc.GetAspectRatioError())
            End If
        End While

        p.CropTop = tempTop
        p.CropBottom = tempBottom
    End Sub

    Sub OvercropWidth()
        If p.AutoSmartOvercrop > 0 AndAlso
            p.AutoSmartOvercrop < Calc.GetTargetDAR Then

            Dim newar = p.AutoSmartOvercrop
            Dim croph = p.SourceHeight - p.CropTop - p.CropBottom
            g.MainForm.tbTargetHeight.Text = Calc.FixMod16(CInt(p.TargetWidth / newar)).ToString
            newar = CSng(p.TargetWidth / p.TargetHeight)
            Dim cropw = (newar / Calc.GetSourceDAR * (croph / p.SourceHeight)) * p.SourceWidth
            p.CropLeft = CInt((p.SourceWidth - cropw) / 2)
            p.CropRight = p.CropLeft

            If p.CropLeft < 0 Then
                p.CropLeft = 0
                p.CropRight = 0
            End If

            CorrectCropMod()
        End If
    End Sub

    Function ConvertPath(value As String) As String
        If value = "" Then Return ""
        If value.Length > 30 AndAlso value.Contains("|") Then value = value.RightLast("|")
        If value.Contains("Constant Quality") Then value = value.Replace("Constant Quality", "CQ")
        If value.Contains(" | ") Then value = value.Replace(" | ", " - ")
        If value.Contains("  ") Then value = value.Replace("  ", " ")
        Return value.Trim
    End Function

    Sub CorrectCropMod()
        CorrectCropMod(False)
    End Sub

    Sub ForceCropMod()
        If Not g.EnableFilter("Crop") Then
            p.Script.InsertAfter("Source", New VideoFilter("Crop", "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)"))
        End If

        CorrectCropMod(True)
    End Sub

    Private Sub CorrectCropMod(force As Boolean)
        If p.AutoCorrectCropValues OrElse force Then
            p.CropLeft += p.CropLeft Mod 2
            p.CropRight += p.CropRight Mod 2
            p.CropTop += p.CropTop Mod 2
            p.CropBottom += p.CropBottom Mod 2

            Dim modValue = 4

            If Not p.Script.IsFilterActive("Resize") Then
                modValue = p.ForcedOutputMod
            End If

            Dim whalf = ((p.SourceWidth - p.CropLeft - p.CropRight) Mod modValue) \ 2

            If p.CropLeft > p.CropRight Then
                p.CropLeft += whalf - whalf Mod 2
                p.CropRight += whalf + whalf Mod 2
            Else
                p.CropRight += whalf - whalf Mod 2
                p.CropLeft += whalf + whalf Mod 2
            End If

            Dim hhalf = ((p.SourceHeight - p.CropTop - p.CropBottom) Mod modValue) \ 2

            If p.CropTop > p.CropBottom Then
                p.CropTop += hhalf - hhalf Mod 2
                p.CropBottom += hhalf + hhalf Mod 2
            Else
                p.CropBottom += hhalf - hhalf Mod 2
                p.CropTop += hhalf + hhalf Mod 2
            End If

            g.MainForm.FiltersListView.Load()
        End If
    End Sub

    Function GetTimeString(sec As Double) As String
        Dim ts = TimeSpan.FromSeconds(sec)
        Return CInt(Math.Floor(ts.TotalMinutes)).ToString("00") + ":" + CInt(Math.Floor(ts.Seconds)).ToString("00")
    End Function
End Class