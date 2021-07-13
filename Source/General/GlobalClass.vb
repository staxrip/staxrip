
Imports System.Collections.ObjectModel
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.Management.Automation
Imports System.Runtime.ExceptionServices
Imports System.Security.Principal
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms.VisualStyles
Imports Microsoft.VisualBasic.FileIO
Imports Microsoft.Win32
Imports StaxRip.UI
Imports VB6 = Microsoft.VisualBasic

Public Class GlobalClass
    Property DefaultCommands As New GlobalCommands
    Property DPI As Integer
    Property IsAdmin As Boolean = New WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)
    Property IsJobProcessing As Boolean
    Property MainForm As MainForm
    Property MAX_PATH As Integer = 260
    Property MinimizedWindows As Boolean
    Property ProcForm As ProcessingForm
    Property ProjectPath As String
    Property SavedProject As New Project
    Property StopAfterCurrentJob As Boolean
    Property ActiveForm As Form

    Event AfterJobFailed()
    Event AfterJobMuxed()
    Event AfterJobProcessed()
    Event AfterJobsProcessed()
    Event AfterProjectLoaded()
    Event AfterProjectOrSourceLoaded()
    Event AfterSourceLoaded()
    Event AfterVideoEncoded()
    Event ApplicationExit()
    Event BeforeJobProcessed()
    Event BeforeProcessing()

    Sub WriteDebugLog(value As String)
        If s?.WriteDebugLog Then
            Trace.TraceInformation(value)
        End If
    End Sub

    Function InvokePowerShellCode(code As String, Optional args As String() = Nothing) As Collection(Of PSObject)
        Return InvokePowerShellCode(code, Nothing, Nothing, args)
    End Function

    Function InvokePowerShellCode(
        code As String,
        variableName As String,
        variableValue As Object) As Collection(Of PSObject)

        InvokePowerShellCode(code, variableName, variableValue, Nothing)
    End Function

    Function InvokePowerShellCode(
        code As String,
        variableName As String,
        variableValue As Object,
        args As String()) As Collection(Of PSObject)

        Try
            Return PowerShell.Invoke(code, variableName, variableValue, args)
        Catch ex As RuntimeException
            g.ShowException(ex, "PowerShell Scipt Exception",
                ex.ErrorRecord.ScriptStackTrace.Replace(" <ScriptBlock>, <No file>", ""))
        Catch ex As Exception
            ShowException(ex)
        End Try
    End Function

    Sub LoadPowerShellScripts()
        For Each dirPath In {Folder.Apps + "Scripts", Folder.Scripts + "Auto Load"}
            If dirPath.DirExists Then
                For Each fp In Directory.GetFiles(dirPath, "*.ps1")
                    g.DefaultCommands.ExecutePowerShellFile(fp)
                Next
            End If
        Next
    End Sub

    Function ConvertToCSV(delimiter As String, objects As IEnumerable(Of Object)) As String
        Dim code = $"$inputVar | ConvertTo-Csv -Delimiter '{delimiter}' -NoTypeInformation"
        Return "sep=" + delimiter + BR + PowerShell.InvokeAndConvert(code, "inputVar", objects)
    End Function

    Function IsWindowsTerminalAvailable() As Boolean
        Return File.Exists(Folder.AppDataLocal + "Microsoft\WindowsApps\wt.exe")
    End Function

    Sub RunCodeInTerminal(code As String)
        Dim base64 = Convert.ToBase64String(Encoding.Unicode.GetBytes(code)) 'UTF16LE

        If s.PreferWindowsTerminal AndAlso g.IsWindowsTerminalAvailable Then
            RunCommandInTerminal("wt.exe", $"powershell.exe -NoLogo -NoExit -NoProfile -EncodedCommand ""{base64}""")
        Else
            RunCommandInTerminal("powershell.exe", $"-NoLogo -NoExit -NoProfile -EncodedCommand ""{base64}""")
        End If
    End Sub

    Sub RunCommandInTerminal(fileName As String, Optional arguments As String = Nothing)
        Using pr As New Process
            pr.StartInfo.FileName = fileName
            pr.StartInfo.Arguments = arguments
            pr.StartInfo.UseShellExecute = False
            pr.StartInfo.WorkingDirectory = Folder.Desktop
            Proc.SetEnvironmentVariables(pr)
            pr.Start()
        End Using
    End Sub

    Sub Execute(fileName As String, Optional arguments As String = Nothing)
        Dim info As New ProcessStartInfo
        info.UseShellExecute = False
        info.FileName = fileName

        If arguments <> "" Then
            info.Arguments = arguments
        End If

        Using pr As New Process
            pr.StartInfo = info
            Proc.SetEnvironmentVariables(pr)
            pr.Start()
        End Using
    End Sub

    Sub ShellExecute(fileName As String, Optional arguments As String = Nothing)
        Try
            If Not fileName.StartsWith("http") AndAlso fileName.Ext.EqualsAny("htm", "html") Then
                Dim browser = g.GetAppPathForExtension("htm", "html")

                If Not browser.FileName.EqualsAny("chrome.exe", "firefox.exe") Then
                    browser = "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
                End If

                If Not browser.FileExists Then
                    browser = Package.FindEverywhere({"chrome.exe", "firefox.exe"})
                End If

                If browser.FileExists Then
                    arguments = fileName.Escape
                    fileName = browser
                End If
            End If

            Process.Start(fileName, arguments)?.Dispose()
        Catch ex As Exception
            g.ShowException(ex, "Failed to start process", "Filename:" + BR2 + fileName + BR2 + "Arguments:" + BR2 + arguments)
        End Try
    End Sub

    Sub SelectFileWithExplorer(filepath As String)
        g.ShellExecute("explorer.exe", "/n, /select, " + filepath.Escape)
    End Sub

    Sub AddToPath(ParamArray dirs As String())
        Dim path = Environment.GetEnvironmentVariable("path")
        Dim newPath = path

        For Each d In dirs
            If d.DirExists AndAlso Not d.StartsWith(Folder.System) AndAlso Not path.Contains(d + ";") Then
                newPath = d + ";" + newPath
            End If
        Next

        If newPath <> path Then
            Environment.SetEnvironmentVariable("path", newPath)
        End If
    End Sub

    Function IsFixedDrive(path As String) As Boolean
        Try
            If path <> "" Then
                Return New DriveInfo(path).DriveType = DriveType.Fixed
            End If
        Catch
        End Try
    End Function

    Sub ProcessJobs()
        For Each form In PreviewForm.Instances.ToArray
            form.Close()
        Next

        Dim tempProject = ObjectHelp.GetCopy(p)
        Dim projectPath = g.ProjectPath
        Dim mainFormText = g.MainForm.Text
        Dim targetFile = p.TargetFile

        FrameServerHelp.AviSynthToolPath()
        g.StopAfterCurrentJob = False
        ProcessJobsRecursive()

        Dim restore = String.IsNullOrWhiteSpace(tempProject.SourceFile) OrElse (File.Exists(tempProject.SourceFile) AndAlso Directory.Exists(tempProject.TempDir) AndAlso File.Exists(projectPath))
        If restore Then
            g.MainForm.OpenProject(tempProject, projectPath)
            g.MainForm.tbTargetFile.Text = targetFile
            g.MainForm.Text = mainFormText
            g.ProjectPath = projectPath
            g.UpdateTrim(p.Script)
            g.MainForm.UpdateFilters()
        Else
            g.MainForm.OpenProject("", False)
        End If
    End Sub

    Sub ProcessJobsRecursive()
        Dim jobs = JobManager.ActiveJobs

        If jobs.Count = 0 Then
            Exit Sub
        End If

        g.IsJobProcessing = True
        Dim jobPath = jobs(0).Path

        Try
            JobManager.ActivateJob(jobPath, False)
            g.MainForm.OpenProject(jobPath, False)

            If s.PreventStandby Then
                PowerRequest.SuppressStandby()
            End If

            ProcessJob(jobPath)
            jobs = JobManager.GetJobs

            If jobs.Count = 0 Then
                g.RaiseAppEvent(ApplicationEvent.AfterJobsProcessed)
                g.ShutdownPC()
            ElseIf JobManager.ActiveJobs.Count = 0 OrElse g.StopAfterCurrentJob Then
                g.RaiseAppEvent(ApplicationEvent.AfterJobsProcessed)

                If Process.GetProcessesByName("StaxRip").Count = 1 Then
                    g.ShutdownPC()
                End If
            Else
                If Process.GetCurrentProcess.PrivateMemorySize64 / 1024 ^ 2 > 1500 Then
                    g.ShellExecute(Application.ExecutablePath, "-StartJobs")
                    g.MainForm.SetSavedProject()
                    g.MainForm.Close()
                Else
                    ProcessJobsRecursive()
                End If
            End If
        Catch ex As AbortException
            Log.Save()
            g.MainForm.OpenProject(g.ProjectPath, False)
        Catch ex As Exception
            Log.Save()
            g.OnException(ex)
        Finally
            If s.PreventStandby Then
                PowerRequest.EnableStandby()
            End If

            g.IsJobProcessing = False
            ThemeManager.SetCurrentTheme(s.ThemeName)
            g.MainForm.OpenProject(jobPath, False)
            ProcController.Finished()
        End Try
    End Sub

    Sub ProcessJob(jobPath As String)
        Try
            If Not File.Exists(jobPath) Then
                g.RunTask(Sub() MsgError("Project file not found!", $"'{jobPath}'{BR}could not be found and got skipped!"))
                Exit Sub
            End If

            g.RaiseAppEvent(ApplicationEvent.BeforeJobProcessed)

            Dim startTime = DateTime.Now

            If p.BatchMode Then
                Dim missingFiles = p.SourceFiles.Where(Function(srcFile) Not srcFile.FileExists)

                If missingFiles.Count > 0 Then
                    g.RunTask(Sub() MsgError("Source file not found!", $"{missingFiles.Join(BR)}{BR}could not be found and got skipped!"))
                    Exit Sub
                End If

                g.MainForm.OpenVideoSourceFiles(p.SourceFiles, True)
                g.ProjectPath = p.TempDir + p.TargetFile.Base + ".srip"
                p.BatchMode = False
                g.MainForm.SaveProjectPath(g.ProjectPath)
            Else
                If Not File.Exists(p.SourceFile) Then
                    g.RunTask(Sub() MsgError("Source file not found!", $"'{p.SourceFile}'{BR}could not be found and got skipped!"))
                    Exit Sub
                End If
            End If

            g.RaiseAppEvent(ApplicationEvent.BeforeProcessing)

            Log.WriteConfiguration()

            Log.WriteHeader($"{p.Script.Engine} Script")
            Log.WriteLine(p.Script.GetFullScript)

            Dim err = p.Script.GetError

            If err <> "" Then
                Throw New ErrorAbortException($"{p.Script.Engine} Script Error", err)
            End If

            Log.WriteHeader("Source Script Info")
            Log.WriteLine(p.SourceScript.GetInfo().GetInfoText(-1))
            Log.WriteHeader("Target Script Info")
            Log.WriteLine(p.Script.GetInfo().GetInfoText(-1))

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

            For Each track In p.AudioTracks
                Dim temp = track

                If p.SkipAudioEncoding AndAlso File.Exists(track.GetOutputFile) Then
                    track.File = track.GetOutputFile()
                Else
                    actions.Add(Sub()
                                    Audio.Process(temp)
                                    temp.Encode()
                                End Sub)
                End If
            Next

            actions.Add(Sub() Subtitle.Cut(p.VideoEncoder.Muxer.Subtitles))

            If CanEncodeVideo() Then
                If p.VideoEncoder.CanChunkEncode Then
                    p.Script.Synchronize()

                    For Each i In p.VideoEncoder.GetChunkEncodeActions
                        actions.Add(i)
                    Next
                Else
                    actions.Add(AddressOf p.VideoEncoder.Encode)
                End If
            End If

            Try
                Parallel.Invoke(New ParallelOptions With {.MaxDegreeOfParallelism = s.ParallelProcsNum}, actions.ToArray)
            Catch ex As AggregateException
                ExceptionDispatchInfo.Capture(ex.InnerExceptions(0)).Throw()
            End Try

            Log.Save()

            g.RaiseAppEvent(ApplicationEvent.AfterVideoEncoded)

            p.VideoEncoder.Muxer.Mux()

            Dim cts = New CancellationTokenSource()
            Dim closingTD As TaskDialog(Of DialogResult)
            Dim mainFormClosingHandler As FormClosingEventHandler

            If p.Thumbnailer Then
                mainFormClosingHandler = Sub(sender As Object, e As FormClosingEventArgs)
                                             If Not e.Cancel Then
                                                 Using closingTD
                                                     closingTD = New TaskDialog(Of DialogResult)(DialogResult.Yes)
                                                     closingTD.Icon = TaskIcon.Warning
                                                     closingTD.Title = "Close while Thumbnailer is running?"
                                                     closingTD.Content = "StaxRip shall be closed while Thumbnailer is still running."
                                                     closingTD.Content += $"{BR2}These are your options:"
                                                     closingTD.Content += $"{BR}(1) Leave this dialog open and StaxRip will continue the closing procedure as soon as Thumbnailer finishes."
                                                     closingTD.Content += $" If you have modified the project, you may be asked to save it first."
                                                     closingTD.Content += $"{BR}(2) YES - Forces Thumbnailer to stop immediately and StaxRip will continue closing."
                                                     closingTD.Content += $"{BR}(3) NO  - Cancels the closing procedure and StaxRip keeps running."
                                                     closingTD.AddButton(DialogResult.Yes)
                                                     closingTD.AddButton(DialogResult.No)

                                                     If closingTD.Show() = DialogResult.No Then
                                                         e.Cancel = True
                                                     End If
                                                 End Using
                                             End If
                                         End Sub

                Dim thumbnailerTask = Task.Run(
                    Async Function()
                        AddHandler g.MainForm.FormClosing, mainFormClosingHandler

                        Dim proceededSources = Await Thumbnailer.RunAsync(cts.Token, p, p.TargetFile)

                        If closingTD IsNot Nothing AndAlso Not closingTD.IsDisposingOrDisposed Then
                            closingTD.Close()
                        End If

                        RemoveHandler g.MainForm.FormClosing, mainFormClosingHandler
                        Return proceededSources
                    End Function,
                    cts.Token)
            End If

            g.RaiseAppEvent(ApplicationEvent.AfterJobMuxed)

            Log.WriteHeader("Job Complete")
            Log.WriteStats(startTime)
            Log.Save()

            g.ArchiveLogFile(Log.GetPath)
            g.DeleteTempFiles()
            g.RaiseAppEvent(ApplicationEvent.AfterJobProcessed)
            JobManager.RemoveJob(jobPath)

            If jobPath.StartsWith(Folder.Settings + "Batch Projects\") Then
                File.Delete(jobPath)
            End If
        Catch ex As SkipException
            Log.Save()
            ProcController.Aborted = False
        Catch ex As ErrorAbortException
            Log.Save()
            g.RaiseAppEvent(ApplicationEvent.AfterJobFailed)
            g.ShowException(ex, Nothing, Nothing, 50)
            g.ShellExecute(g.GetTextEditorPath(), """" + p.TempDir + p.TargetFile.Base + "_staxrip.log" + """")
            ProcController.Aborted = False
        End Try
    End Sub

    Function CanEncodeVideo() As Boolean
        Return Not (p.SkipVideoEncoding AndAlso Not TypeOf p.VideoEncoder Is NullEncoder AndAlso
            File.Exists(p.VideoEncoder.OutputPath))
    End Function

    Sub DeleteTempFiles()
        If s.DeleteTempFilesMode <> DeleteMode.Disabled AndAlso p.TempDir.EndsWith("_temp\") Then
            Try
                Dim moreJobsToProcessInTempDir = JobManager.GetJobs.Where(Function(a) a.Active AndAlso a.Path.Contains(p.TempDir))

                If moreJobsToProcessInTempDir.Count = 0 Then
                    If s.DeleteTempFilesMode = DeleteMode.RecycleBin Then
                        FolderHelp.Delete(p.TempDir, RecycleOption.SendToRecycleBin)
                    Else
                        FolderHelp.Delete(p.TempDir)
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
        Using dialog As New FolderBrowserDialog
            dialog.SetSelectedPath(defaultFolder)

            If dialog.ShowDialog = DialogResult.OK Then
                Return dialog.SelectedPath
            End If
        End Using
    End Function

    Function VerifyRequirements() As Boolean
        If s.VerifyToolStatus AndAlso Environment.Is64BitProcess Then
            SyncLock Package.ConfLock
                For Each pack In Package.Items.Values
                    If Not pack.VerifyOK Then
                        Return False
                    End If
                Next
            End SyncLock

            If Not p.Script.IsFilterActive("Source") Then
                MsgWarn("No active filter of category 'Source' found.")
                Return False
            End If
        End If

        Return True
    End Function

    Function VerifySource(files As IEnumerable(Of String)) As Boolean
        For Each file In files
            If Not file.IsProcessEncodingCompatible AndAlso p.Script.IsAviSynth Then
                ShowAviSynthUnicodeError()
                Return False
            End If
        Next

        Return True
    End Function

    Sub ShowAviSynthUnicodeError()
        MsgError($"Unicode filenames are not supported by AviSynth unless Windows 10 is used." + BR2 +
                 $"Rename the file or enable VapourSynth:{BR2}Filters > Filter Setup > VapourSynth")
    End Sub

    Sub PlayAudio(ap As AudioProfile)
        If FileTypes.AudioRaw.Contains(ap.File.Ext) Then
            g.ShellExecute(Package.mpvnet.Path, ap.File.Escape)
        ElseIf ap.File = p.FirstOriginalSourceFile AndAlso ap.Streams.Count > 0 Then
            g.ShellExecute(Package.mpvnet.Path, "--audio=" & (ap.Stream.Index + 1) & " " + p.FirstOriginalSourceFile.Escape)
        ElseIf FileTypes.Audio.Contains(ap.File.Ext) Then
            g.ShellExecute(Package.mpvnet.Path, "--audio-delay=" + (g.ExtractDelay(ap.File) / 1000).ToInvariantString.Shorten(9) + " --audio-files=" + ap.File.Escape + " " + p.FirstOriginalSourceFile.Escape)
        Else
            MsgError("Unable to play audio.")
        End If
    End Sub

    Sub PlayScript(script As VideoScript)
        PlayScriptWithMPV(script)
    End Sub

    Function GetAudioProfileForScriptPlayback() As AudioProfile
        If File.Exists(p.Audio0.File) AndAlso FileTypes.Audio.Contains(p.Audio0.File.Ext) Then
            Return p.Audio0
        ElseIf File.Exists(p.Audio1.File) AndAlso FileTypes.Audio.Contains(p.Audio1.File.Ext) Then
            Return p.Audio1
        End If
    End Function

    Sub PlayScriptWithMPC(script As VideoScript, Optional cliArgs As String = Nothing)
        If script Is Nothing Then
            Exit Sub
        End If

        script.Synchronize()
        Dim playerPath = Package.MPC.Path

        If Not playerPath.FileExists AndAlso Package.MPC.VerifyOK(True) Then
            playerPath = Package.MPC.Path
        End If

        If Not playerPath.FileExists Then
            Exit Sub
        End If

        Dim args As String

        If cliArgs <> "" Then
            args += " " + cliArgs
        End If

        Dim ap = GetAudioProfileForScriptPlayback()

        If Not ap Is Nothing AndAlso FileTypes.Audio.Contains(ap.File.Ext) AndAlso
            p.Ranges.Count = 0 Then

            args += " /dub " + ap.File.Escape
        End If

        args += " " + script.Path.Escape
        g.ShellExecute(playerPath, args.Trim)
    End Sub

    Sub PlayScriptWithMPV(script As VideoScript, Optional cliArgs As String = Nothing)
        If script Is Nothing Then
            Exit Sub
        End If

        script.Synchronize()
        Dim args As String

        If script.Engine = ScriptEngine.VapourSynth Then
            args += " --demuxer-lavf-format=vapoursynth"
        End If

        If cliArgs <> "" Then
            args += " " + cliArgs
        End If

        If Calc.IsARSignalingRequired Then
            args += " --video-aspect=" + Calc.GetTargetDAR.ToInvariantString.Shorten(8)
        End If

        Dim ap = GetAudioProfileForScriptPlayback()

        If Not ap Is Nothing AndAlso FileTypes.Audio.Contains(ap.File.Ext) AndAlso p.Ranges.Count = 0 Then
            args += " --audio-files=" + ap.File.Escape
        End If

        args += " " + script.Path.Escape
        g.Execute(Package.mpvnet.Path, args.Trim)
    End Sub

    Function ExtractDelay(value As String) As Integer
        Dim match = Regex.Match(value, " (-?\d+) ?ms")

        If match.Success Then
            Return CInt(match.Groups(1).Value)
        End If
    End Function

    Function GetSourceBase() As String
        If p.TempDir.EndsWithEx("_temp\") Then
            Return "temp"
        Else
            Return p.SourceFile.Base
        End If
    End Function

    Sub ShowCode(title As String, content As String, Optional find As String = Nothing, Optional wordwrap As Boolean = False)
        Dim form As New CodeForm(content, find, wordwrap)
        form.Text = title
        form.Show()
    End Sub

    Sub ShowHelp(title As String, content As String)
        If title <> "" Then
            title = title.TrimEnd("."c, ":"c)
        End If

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
                        If iItem.Text = a(i) + "     " Then
                            found = True
                            l = DirectCast(iItem, ToolStripMenuItem).DropDownItems
                        End If
                    End If
                Next

                If Not found Then
                    If i = a.Length - 1 Then
                        Dim item As New MenuItemEx(a(i) + "     ", Sub() loadAction(iProfile))
                        l.Add(item)
                        l = item.DropDownItems
                    Else
                        Dim item As New MenuItemEx(a(i) + "     ")
                        l.Add(item)
                        l = item.DropDownItems
                    End If
                End If
            Next
        Next

        If Not dialogAction Is Nothing Then
            ic.Add(New ToolStripSeparator)
            ic.Add(New MenuItemEx("Edit Profiles...", dialogAction, "Opens the profiles editor"))
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
        Return CInt((s.LastPosition / p.Script.GetCachedFrameRate) * 1000)
    End Function

    Function GetTextEditorPath() As String
        Dim ret = GetAppPathForExtension("txt")

        If ret <> "" Then
            Return ret
        End If

        Return "notepad.exe"
    End Function

    Function GetAppPathForExtension(ParamArray extensions As String()) As String
        For Each extension In extensions
            If Not extension.StartsWith(".") Then
                extension = "." + extension
            End If

            Dim c = 0UI

            If Native.AssocQueryString(&H40, 2, extension, Nothing, Nothing, c) = 1 Then
                If c > 0 Then
                    Dim sb As New StringBuilder(CInt(c))

                    If 0 = Native.AssocQueryString(&H40, 2, extension, Nothing, sb, c) Then
                        Dim ret = sb.ToString

                        If File.Exists(ret) Then
                            Return ret
                        End If
                    End If
                End If
            End If
        Next
    End Function

    Sub SaveSettings()
        Try
            Using mutex As New Mutex(False, "staxrip settings file")
                mutex.WaitOne()
                SafeSerialization.Serialize(s, g.SettingsFile)
                mutex.ReleaseMutex()
            End Using

            Dim backupPath = Folder.Settings + "Backup\"

            If Not Directory.Exists(backupPath) Then
                Directory.CreateDirectory(backupPath)
            End If

            FileHelp.Copy(g.SettingsFile, backupPath + "Settings(v" + Application.ProductVersion + ").dat")
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    Sub LoadVideoEncoder(profile As Profile)
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

    Sub LoadAudioProfile0(profile As Profile)
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

    Sub LoadAudioProfile1(profile As Profile)
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

    Sub RaiseAppEvent(ae As ApplicationEvent)
        Select Case ae
            Case ApplicationEvent.AfterJobFailed
                RaiseEvent AfterJobFailed()
            Case ApplicationEvent.AfterJobMuxed
                RaiseEvent AfterJobMuxed()
            Case ApplicationEvent.AfterJobProcessed
                RaiseEvent AfterJobProcessed()
            Case ApplicationEvent.AfterJobsProcessed
                RaiseEvent AfterJobsProcessed()
            Case ApplicationEvent.AfterProjectLoaded
                RaiseEvent AfterProjectLoaded()
            Case ApplicationEvent.AfterProjectOrSourceLoaded
                RaiseEvent AfterProjectOrSourceLoaded()
            Case ApplicationEvent.AfterSourceLoaded
                RaiseEvent AfterSourceLoaded()
            Case ApplicationEvent.AfterVideoEncoded
                RaiseEvent AfterVideoEncoded()
            Case ApplicationEvent.ApplicationExit
                RaiseEvent ApplicationExit()
            Case ApplicationEvent.BeforeJobProcessed
                RaiseEvent BeforeJobProcessed()
            Case ApplicationEvent.BeforeProcessing
                RaiseEvent BeforeProcessing()
        End Select

        Dim scriptPath = Folder.Scripts + ae.ToString + ".ps1"

        If File.Exists(scriptPath) Then
            g.DefaultCommands.ExecutePowerShellFile(scriptPath)
        End If

        For Each ec In s.EventCommands
            If ec.Enabled AndAlso ec.Event = ae Then
                Dim matches = 0

                For Each criteria In ec.CriteriaList
                    criteria.PropertyString = Macro.Expand(criteria.Macro)

                    If criteria.Eval Then
                        matches += 1
                    End If
                Next

                If (ec.CriteriaList.Count = 0 OrElse (ec.OrOnly AndAlso matches > 0) OrElse
                    (Not ec.OrOnly AndAlso matches = ec.CriteriaList.Count)) AndAlso
                    Not ec.CommandParameters Is Nothing Then

                    Dim command = g.MainForm.CustomMainMenu.CommandManager.GetCommand(ec.CommandParameters.MethodName)

                    If s.LogEventCommand AndAlso p.SourceFile <> "" Then
                        Log.WriteHeader("Event Command: " + ec.Name)
                        Log.WriteLine("Event: " + DispNameAttribute.GetValueForEnum(ec.Event))
                        Log.WriteLine("Command: " + command.MethodInfo.Name)
                        Log.WriteLine(command.GetParameterHelp(ec.CommandParameters.Parameters))
                    End If

                    g.MainForm.CustomMainMenu.CommandManager.Process(ec.CommandParameters)
                End If
            End If
        Next
    End Sub

    Sub SetTempDir()
        If p.SourceFile <> "" Then
            If p.NoTempDir Then
                p.TempDir = p.SourceFile.Dir
            Else
                p.TempDir = Macro.Expand(p.TempDir)

                If p.TempDir = "" Then
                    If p.SourceFile.Dir.EndsWith("_temp\") Then
                        p.TempDir = p.SourceFile.Dir
                    Else
                        p.TempDir = p.SourceFile.Dir + p.SourceFile.Base + "_temp\"
                    End If
                End If

                p.TempDir = p.TempDir.FixDir

                If Not Directory.Exists(p.TempDir) Then
                    Try
                        Directory.CreateDirectory(p.TempDir)
                    Catch
                        Try
                            p.TempDir = p.SourceFile.DirAndBase + "_temp\"

                            If Not Directory.Exists(p.TempDir) Then
                                Directory.CreateDirectory(p.TempDir)
                            End If
                        Catch
                            MsgWarn("Failed to create a temp directory. By default it's created " +
                                    "in the directory of the source file so it's not possible " +
                                    "to open files directly from an optical drive unless a temp directory  " +
                                    "is defined in the options. Usually discs are copied to the hard drive " +
                                    "first using an application like MakeMKV, DVDFab or AnyDVD.")
                            Throw New AbortException
                        End Try
                    End Try
                End If
            End If
        End If
    End Sub

    Sub ShowCommandLinePreview(title As String, value As String)
        If s.CommandLinePreviewViaCodeForm Then
            ShowCodePreview(value, Nothing, True)
        Else
            Environment.SetEnvironmentVariable("CommandLineToShow", BR + value + BR)

            If s.PreferWindowsTerminal AndAlso g.IsWindowsTerminalAvailable Then
                g.Execute("wt.exe", "powershell.exe -NoLogo -NoExit -NoProfile -Command $env:CommandLineToShow")
            Else
                g.Execute("powershell.exe", "-NoLogo -NoExit -NoProfile -Command $env:CommandLineToShow")
            End If
        End If
    End Sub

    Sub ShowCodePreview(code As String, Optional find As String = Nothing, Optional wordwrap As Boolean = False)
        ShowCode("Code Preview", code, find, wordwrap)
    End Sub

    Sub ffmsindex(
        sourcePath As String,
        cachePath As String,
        Optional indexAudio As Boolean = False,
        Optional proj As Project = Nothing)

        If File.Exists(sourcePath) AndAlso Not File.Exists(cachePath) AndAlso
            Not FileTypes.VideoText.Contains(sourcePath.Ext) Then

            Using proc As New Proc
                proc.Header = "Indexing using ffmsindex"
                proc.SkipString = "Indexing, please wait..."
                proc.Project = proj
                proc.Priority = ProcessPriorityClass.Normal
                proc.File = Package.ffms2.Directory + "ffmsindex.exe"
                proc.Arguments = If(indexAudio, "-t -1 ", "") + sourcePath.LongPathPrefix.Escape +
                    " " + cachePath.LongPathPrefix.Escape
                proc.Start()
            End Using
        End If
    End Sub

    Function IsValidSource(Optional warn As Boolean = True) As Boolean
        If p.SourceScript.GetFrameCount = 0 Then
            If warn Then
                MsgWarn("Failed to load source.")
            End If

            Return False
        End If

        If p.SourceScript.GetError <> "" Then
            MsgError("Script Error", p.SourceScript.GetError)
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
        Return path.Base.StartsWith(p.SourceFile.Base) OrElse path.StartsWithEx(p.TempDir)
    End Function

    Function GetFilesInTempDirAndParent() As List(Of String)
        Dim ret As New List(Of String)
        Dim dirs As New HashSet(Of String)

        If p.TempDir <> "" Then
            dirs.Add(p.TempDir)
        End If

        If p.TempDir?.EndsWith("_temp\") Then
            dirs.Add(p.TempDir.Parent)
        End If

        dirs.Add(p.FirstOriginalSourceFile.Dir)

        For Each i In dirs
            ret.AddRange(Directory.GetFiles(i))
        Next

        Return ret
    End Function

    Function IsSourceSimilar(path As String) As Boolean
        If p.SourceFile.Contains("_") Then
            Dim sourceBase = p.SourceFile.Base

            While sourceBase.Length > 2 AndAlso sourceBase.ToCharArray.Last.IsDigit
                sourceBase = sourceBase.DeleteRight(1)
            End While

            If sourceBase.EndsWith("_") AndAlso path.Base.StartsWith(sourceBase.TrimEnd("_"c)) Then
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

                    If name = "" Then
                        name = p.SourceFile.Base
                    End If

                    g.MainForm.SaveProjectPath(p.SourceFile.Dir + "recovery.srip")
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

    Sub ShowException(
        ex As Exception,
        Optional title As String = Nothing,
        Optional content As String = Nothing,
        Optional timeout As Integer = 0)

        Try
            Using td As New TaskDialog(Of String)
                If title = "" Then
                    If TypeOf ex Is ErrorAbortException Then
                        td.Title = DirectCast(ex, ErrorAbortException).Title + $" (v{Application.ProductVersion})"
                    Else
                        td.Title = ex.GetType.Name + $" (v{Application.ProductVersion})"
                    End If
                Else
                    td.Title = title
                End If

                td.Timeout = timeout
                td.Content = (ex.Message + BR2 + content).Trim
                td.Icon = TaskIcon.Error
                td.ExpandedContent = ex.ToString
                td.ShowCopyButton = True
                td.AddButton("OK")
                td.Show()
            End Using
        Catch
            Dim msg As String

            If TypeOf ex Is ErrorAbortException Then
                msg = DirectCast(ex, ErrorAbortException).Title
            Else
                msg = ex.GetType.Name
            End If

            VB6.MsgBox(msg + BR2 + ex.Message + BR2 + ex.ToString, VB6.MsgBoxStyle.Critical)
        End Try
    End Sub

    Sub ArchiveLogFile(path As String)
        Try
            Dim logFolder = Folder.Settings + "Log Files\"

            If Not Directory.Exists(logFolder) Then
                Directory.CreateDirectory(logFolder)
            End If

            FileHelp.Copy(path, logFolder + Date.Now.ToString("yyyy-MM-dd - HH.mm.ss") + " - " + path.FileName)
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
            ms.Renderer = New ToolStripRendererEx()
        Else
            ms.Renderer = New ToolStripSystemRenderer()
        End If
    End Sub

    Sub Play(file As String)
        g.ShellExecute(Package.mpvnet.Path.Escape, file.Escape)
    End Sub

    Sub ShowWikiPage(title As String)
        ShellExecute($"https://github.com/staxrip/staxrip/wiki/{title}")
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
        Set(value As Icon)
            IconValue = value
        End Set
    End Property

    Sub MakeBugReport(e As Exception)
        If e Is Nothing AndAlso Not g.IsValidSource(False) Then
            Exit Sub
        End If

        If Not e Is Nothing Then
            If Log.IsEmpty Then Log.WriteEnvironment()
            Log.WriteHeader("Exception")
            Log.WriteLine(e.ToString)
        End If

        Log.Save(p)

        Dim logfileOpened = False
        Dim fp = Log.GetPath

        If MsgQuestion("An error occured", "Do you want to open the log file?",
                       TaskButton.YesNo) = DialogResult.Yes Then

            g.ShellExecute(g.GetTextEditorPath(), fp.Escape)
            logfileOpened = True
        End If

        If MsgQuestion("Bug Report", "Do you want to report an issue or bug?",
                       TaskButton.YesNo) = DialogResult.Yes Then

            If Not logfileOpened Then
                g.ShellExecute(g.GetTextEditorPath(), fp.Escape)
            End If

            g.SelectFileWithExplorer(fp)
            g.ShellExecute("https://github.com/staxrip/staxrip/issues")
        End If
    End Sub

    Function FileExists(path As String) As Boolean
        For x = 1 To 6
            If File.Exists(path) Then
                Return True
            End If

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

    Function EnableFilter(search As String) As Boolean
        For Each filter In p.Script.Filters
            If filter.Category = search Then
                If Not filter.Active Then
                    filter.Active = True
                    g.MainForm.FiltersListView.Load()
                End If

                Return True
            End If
        Next
    End Function

    Function BrowseFile(filter As String) As String
        Using dialog As New OpenFileDialog
            dialog.Filter = filter
            dialog.SetInitDir(p.TempDir)

            If dialog.ShowDialog = DialogResult.OK Then
                Return dialog.FileName
            End If
        End Using
    End Function

    Sub ShowScriptInfo(script As VideoScript)
        script.Synchronize()

        If Not script.Path.FileExists Then
            Exit Sub
        End If

        Dim text = If(script.Error = "", script.Info.GetInfoText(-1), script.Error)
        text = BR + "  " + text.FixBreak.Replace(BR, BR + "  ") + BR

        Using form As New StringEditorForm
            form.ScaleClientSize(25, 15)
            form.MaximizeBox = False
            form.rtb.ReadOnly = True
            form.cbWrap.Checked = False
            form.cbWrap.Visible = False
            form.rtb.Text = text
            form.Text = script.Path
            form.bnOK.Visible = False
            form.bnCancel.Text = "Close"
            form.ShowDialog()
        End Using
    End Sub

    Sub AddHardcodedSubtitle()
        Dim validSubtitles = p.VideoEncoder?.Muxer?.Subtitles?.Where(Function(x) x.Path.Ext().ToLowerEx().EqualsAny("srt", "ass", "idx"))
        Dim enabledValidSubtitles = validSubtitles?.Where(Function(x) x.Enabled)
        Dim selectedSubtitle As Subtitle = If(enabledValidSubtitles?.Any(), enabledValidSubtitles?.FirstOrDefault(), validSubtitles?.FirstOrDefault())

        If selectedSubtitle IsNot Nothing Then
            selectedSubtitle.Enabled = False
            p.AddHardcodedSubtitleFilter(selectedSubtitle.Path, False)
        End If
    End Sub

    Sub RunAutoCrop(progressAction As Action(Of Double))
        p.SourceScript.Synchronize(True, True, True)

        Using server = FrameServerFactory.Create(p.SourceScript.Path)
            Dim len = server.Info.FrameCount \ (s.CropFrameCount + 1)
            Dim crops(s.CropFrameCount - 1) As AutoCrop
            Dim pos As Integer

            For x = 1 To s.CropFrameCount
                progressAction?.Invoke((x - 1) / s.CropFrameCount * 100)
                pos = len * x

                Using bmp = BitmapUtil.CreateBitmap(server, pos)
                    crops(x - 1) = AutoCrop.Start(bmp.Clone(New Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format32bppRgb), pos)
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
        If value = "" Then
            Return ""
        End If

        If value.Length > 30 AndAlso value.Contains("|") Then
            value = value.RightLast("|")
        End If

        If value.Contains("Constant Quality") Then
            value = value.Replace("Constant Quality", "CQ")
        End If

        If value.Contains(" | ") Then
            value = value.Replace(" | ", " - ")
        End If

        If value.Contains("  ") Then
            value = value.Replace("  ", " ")
        End If

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

    Sub CorrectCropMod(force As Boolean)
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

        Return CInt(Math.Floor(ts.TotalMinutes)).ToString("00") + ":" +
            CInt(Math.Floor(ts.Seconds)).ToString("00")
    End Function

    Sub ShowAdvancedScriptInfo(script As VideoScript)
        script.Synchronize()

        If Not script.Path.FileExists Then
            Exit Sub
        End If

        If script.Engine = ScriptEngine.AviSynth Then
            Using td As New TaskDialog(Of String)
                td.Title = "Choose below"
                td.AddCommand("Info()")
                td.AddCommand("avsmeter benchmark")
                td.AddCommand("avsmeter info")
                td.AddCommand("avs2pipemod info")

                Select Case td.Show()
                    Case "avs2pipemod info"
                        g.RunCodeInTerminal($"""`n{Package.avs2pipemod.Name} {Package.avs2pipemod.Version}""; & '{Package.avs2pipemod.Path}' -dll=""{Package.AviSynth.Path.Escape}"" -info '{script.Path}'")
                    Case "avsmeter benchmark"
                        g.RunCodeInTerminal($"& '{Package.AVSMeter.Path}' '{script.Path.ToShortFilePath}'")
                    Case "avsmeter info"
                        g.RunCodeInTerminal($"& '{Package.AVSMeter.Path}' -info '{script.Path.ToShortFilePath}';""""")
                    Case "Info()"
                        Dim infoScript = New VideoScript
                        infoScript.AddFilter(New VideoFilter($"Import(""{script.Path}"")"))
                        Dim infoCode = $"Info(size={(script.GetInfo().Height * 0.05).ToInvariantString()})"
                        infoScript.AddFilter(New VideoFilter(infoCode))
                        infoScript.Path = p.TempDir + p.TargetFile.Base + $"_info." + script.FileType

                        If infoScript.GetError() <> "" Then
                            MsgError("Script Error", infoScript.GetError())
                            Exit Sub
                        End If

                        g.PlayScriptWithMPV(infoScript, "--pause=yes --osc=no --osd-level=0")
                End Select
            End Using
        Else
            Using td As New TaskDialog(Of String)
                td.Title = "Choose below"
                td.AddCommand("ClipInfo()")
                td.AddCommand("vspipe info")

                Select Case td.Show()
                    Case "vspipe info"
                        g.RunCodeInTerminal($"""`n{Package.vspipe.Name} {Package.vspipe.Version}`n""; & '{Package.vspipe.Path}' --info '{script.Path}' -;""""")
                    Case "ClipInfo()"
                        Dim infoScript = script.GetNewScript
                        infoScript.Path = p.TempDir + p.TargetFile.Base + "_info." + script.FileType
                        infoScript.AddFilter(New VideoFilter("clip = clip.resize.Bicubic(720, (720 / clip.width * clip.height) // 8 * 8)"))
                        infoScript.AddFilter(New VideoFilter("clip = core.text.ClipInfo(clip)"))

                        If infoScript.GetError() <> "" Then
                            MsgError("Script Error", infoScript.GetError())
                            Exit Sub
                        End If

                        g.PlayScriptWithMPV(infoScript, "--pause=yes --osc=no --osd-level=0")
                End Select
            End Using
        End If
    End Sub

    Function IsDevelopmentPC() As Boolean
        Return Application.StartupPath.EndsWith("\bin") OrElse Application.StartupPath.EndsWith("\bin-x86")
    End Function

    Sub RunTask(action As Action)
        Task.Run(Sub()
                     Try
                         action.Invoke
                     Catch ex As Exception
                         OnException(ex)
                     End Try
                 End Sub)
    End Sub

    Function RunSTATask(action As Action) As Task
        Dim tcs = New TaskCompletionSource(Of Object)()
        Dim thread = New Thread(Sub()
                                    Try
                                        action?.Invoke()
                                        tcs.SetResult(Nothing)
                                    Catch ex As Exception
                                        tcs.SetException(ex)
                                    End Try
                                End Sub)
        thread.SetApartmentState(ApartmentState.STA)
        thread.Start()
        Return tcs.Task
    End Function

    Sub UpdateTrim(script As VideoScript, Optional moveToTheEnd As Boolean = False)
        If script Is Nothing Then Exit Sub
        If moveToTheEnd OrElse Not p.Ranges.Any() Then script.RemoveFilter("Cutting")

        p.RangesBasedOnFPS = script.OriginalInfo.FrameRate
        Dim filter As VideoFilter = script.GetFilter("Cutting")

        If p.Ranges.Any() Then
            If filter Is Nothing Then
                filter = New VideoFilter()
                script.Filters.Add(filter)
            End If
            filter.Path = "Cutting"
            filter.Category = "Cutting"
            filter.Script = GetTrim(script)
            filter.Active = True
        End If
    End Sub

    Function GetTrim(script As VideoScript) As String
        Dim ret As String

        For Each i In p.Ranges
            If ret <> "" Then
                ret += " + "
            End If

            If script.Engine = ScriptEngine.AviSynth Then
                ret += "Trim(" & i.Start & ", " & i.End & ")"

                If p.TrimCode <> "" Then
                    ret += "." + p.TrimCode.TrimStart("."c)
                End If
            Else
                ret += "clip[" & i.Start & ":" & (i.End + 1) & "]"
            End If
        Next

        If script.Engine = ScriptEngine.AviSynth Then
            Return ret
        Else
            Return "clip = " + ret
        End If
    End Function

    Function ContainsPipeTool(value As String) As Boolean
        If value <> "" Then
            Return value.Contains("ffmpeg") OrElse value.Contains("avs2pipemod") OrElse value.Contains("vspipe")
        End If
    End Function

    Function GetFilterProfilesText(categories As List(Of FilterCategory)) As String
        Dim ret = ""
        Dim wasMultiline As Boolean

        For Each i In categories
            ret += "[" + i.Name + "]" + BR

            For Each filter In i.Filters
                If filter.Script.Contains(BR) Then
                    Dim lines = filter.Script.SplitLinesNoEmpty

                    For x = 0 To lines.Length - 1
                        lines(x) = "    " + lines(x)
                    Next

                    ret += BR + filter.Path + " =" + BR + lines.Join(BR) + BR
                    wasMultiline = True
                Else
                    If wasMultiline Then
                        ret += BR
                    End If

                    ret += filter.Path + " = " + filter.Script + BR
                    wasMultiline = False
                End If
            Next

            If Not ret.EndsWith(BR2) Then
                ret += BR
            End If
        Next

        Return ret
    End Function

    Function GetCodeFont(Optional size As Integer = 10) As Font
        Return New Font(If(s.CodeFont = "", "Consolas", s.CodeFont), size * s.UIScaleFactor)
    End Function
End Class
