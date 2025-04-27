﻿
Imports System.Collections.ObjectModel
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.Management.Automation
Imports System.Management.Automation.Language
Imports System.Runtime.ExceptionServices
Imports System.Security.Principal
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms.VisualStyles
Imports System.Xml.Linq
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
    Shared Property MAX_PATH As Integer = 260
    Property MinimizedWindows As Boolean
    Property ProcForm As ProcessingForm
    Property LastModifiedTemplate As New Project
    Property ProjectPath As String
    Property SavedProject As New Project
    Property StopAfterCurrentJob As Boolean
    Property ActiveForm As Form

    Event AfterJobAdded()
    Event AfterJobFailed()
    Event AfterJobMuxed()
    Event AfterJobProcessed()
    Event AfterJobsProcessed()
    Event AfterProjectLoaded()
    Event AfterProjectOrSourceLoaded()
    Event AfterSourceOpened()
    Event AfterSourceLoaded()
    Event AfterVideoEncoded()
    Event ApplicationExit()
    Event BeforeJobAdding()
    Event BeforeJobProcessed()
    Event BeforeMuxing()
    Event BeforeProcessing()
    Event WhileProcessing(commandLine As String, percentage As Single, lastProgress As String)

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
        For Each dirPath In {Path.Combine(Folder.Apps, "Scripts"), Folder.Scripts + "Auto Load"}
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
        Return File.Exists(Path.Combine(Folder.AppDataLocal, "Microsoft", "WindowsApps", "wt.exe"))
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
        Dim info As New ProcessStartInfo With {
            .UseShellExecute = False,
            .FileName = fileName
        }

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
            Dim psi = New ProcessStartInfo(fileName.Escape) With {
                .UseShellExecute = True,
                .Arguments = arguments
            }

            Process.Start(psi)?.Dispose()
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

                g.MainForm.OpenVideoSourceFiles(p.SourceFiles, True, s.ErrorMessageTimeout)
                g.ProjectPath = Path.Combine(p.TempDir, p.TargetFile.Base + ".srip")
                p.BatchMode = False
                g.MainForm.SaveProjectPath(g.ProjectPath)
            Else
                If Not File.Exists(p.SourceFile) Then
                    g.RunTask(Sub() MsgError("Source file not found!", $"'{p.SourceFile}'{BR}could not be found and got skipped!"))
                    Exit Sub
                End If
            End If

            g.RaiseAppEvent(ApplicationEvent.BeforeProcessing)

            Log.WriteEnvironment()
            Log.WriteConfiguration()

            Dim summary = MediaInfo.GetSummary(p.SourceFile)
            If Not Log.ToString().Contains(summary) Then
                Log.WriteHeader("Media Info Source File")
                For Each i In p.SourceFiles
                    Log.WriteLine(i)
                Next
                Log.WriteLine(BR + MediaInfo.GetSummary(p.SourceFile))
            End If

            Log.WriteHeader($"{p.Script.Engine} Script")
            Log.WriteLine(p.Script.GetFullScript)

            p.Script.Synchronize(False, False)
            If p.Script.Error <> "" Then
                Throw New ErrorAbortException($"{p.Script.Engine} Script Error", p.Script.Error)
            End If

            Log.WriteHeader("Source Script Info")
            Log.WriteLine(p.SourceScript.GetInfo().GetInfoText(-1))
            Log.WriteHeader("Target Script Info")
            Log.WriteLine(p.Script.Info.GetInfoText(-1))

            g.MainForm.Hide()

            Dim actions As New List(Of Action)

            For Each track In p.AudioTracks
                Dim temp = track.AudioProfile

                If p.SkipAudioEncoding AndAlso File.Exists(track.AudioProfile.GetOutputFile()) Then
                    track.AudioProfile.File = track.AudioProfile.GetOutputFile()
                Else
                    actions.Add(Sub()
                                    Audio.Process(temp)
                                    temp.Encode()
                                End Sub)
                End If
            Next

            For Each track In p.AudioFiles
                Dim temp = track

                If p.SkipAudioEncoding AndAlso File.Exists(track.GetOutputFile()) Then
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

                    If p.VideoEncoder.BeforeEncoding() Then
                        For Each i In p.VideoEncoder.GetChunkEncodeActions()
                            actions.Add(i)
                        Next
                    End If
                Else
                    actions.Add(Sub()
                                    If p.VideoEncoder.BeforeEncoding() Then
                                        Log.Save()
                                        p.VideoEncoder.Encode()
                                    End If
                                End Sub)
                End If
            End If

            Try
                Parallel.Invoke(New ParallelOptions With {.MaxDegreeOfParallelism = s.ParallelProcsNum}, actions.ToArray)
            Catch ex As AggregateException
                ExceptionDispatchInfo.Capture(ex.InnerExceptions(0)).Throw()
            End Try

            Log.Save()
            p.VideoEncoder.AfterEncoding()

            g.RaiseAppEvent(ApplicationEvent.AfterVideoEncoded)

            p.VideoEncoder.Muxer.Mux()

            If p.Thumbnailer Then
                Dim cts = New CancellationTokenSource()
                Dim closingTD As TaskDialog(Of DialogResult) = Nothing
                Dim mainFormClosingHandler As FormClosingEventHandler = Nothing

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

            Dim deleteTempFiles = True

            If FileTypes.Video.Contains(p.TargetFile.Ext()) Then
                Dim isCuttedRemux = TypeOf p.VideoEncoder Is NullEncoder AndAlso p.Ranges?.Any()
                Dim hasFrames = MediaInfo.GetVideo(p.TargetFile, "FrameCount").ToInt(-1)
                Dim shouldFrames = p.TargetFrames

                If hasFrames <> shouldFrames Then
                    g.MainForm.UpdateTargetParameters(p)
                    shouldFrames = p.TargetFrames
                End If

                If hasFrames <> shouldFrames Then
                    Dim difference = shouldFrames - hasFrames
                    If deleteTempFiles AndAlso p.DeleteTempFilesOnFrameMismatchNegative >= 0 AndAlso difference > 0 Then deleteTempFiles = difference <= p.DeleteTempFilesOnFrameMismatchNegative
                    If deleteTempFiles AndAlso p.DeleteTempFilesOnFrameMismatchPositive >= 0 AndAlso difference < 0 Then deleteTempFiles = -difference <= p.DeleteTempFilesOnFrameMismatchPositive

                    Log.WriteHeader("Frame Mismatch")
                    Log.WriteLine($"WARNING: Target file has {hasFrames} frames, but should have {shouldFrames} frames!")
                    If isCuttedRemux Then
                        Log.WriteLine($"There is a mismatch of {difference} frames!")
                    Else
                        Log.WriteLine($"Encoding was probably terminated at {hasFrames / shouldFrames * 100:0.0}% with a mismatch of {difference} frames!")
                    End If

                    If hasFrames > -1 AndAlso p.AbortOnFrameMismatch AndAlso Not isCuttedRemux Then
                        Throw New ErrorAbortException("Frame Mismatch", $"Target file has {hasFrames} frames, but should have {shouldFrames} frames!", p)
                    End If
                End If
            End If

            Log.Save()

            g.ArchiveLogFile(Log.GetPath())
            g.RaiseAppEvent(ApplicationEvent.AfterJobProcessed)
            JobManager.RemoveJob(jobPath)
            If deleteTempFiles Then g.DeleteTempFiles()
            If jobPath.StartsWith(Path.Combine(Folder.Settings, "Batch Projects")) Then File.Delete(jobPath)
        Catch ex As SkipException
            Log.Save()
            ProcController.Aborted = False
        Catch ex As ErrorAbortException
            Log.Save()
            g.RaiseAppEvent(ApplicationEvent.AfterJobFailed)
            g.ShowException(ex, Nothing, Nothing, s.ErrorMessageTimeout)
            g.ShellExecute(g.GetTextEditorPath(), """" + p.Log.GetPath() + """")
            ProcController.Aborted = False
        End Try
    End Sub

    Function CanEncodeVideo() As Boolean
        Return Not (p.SkipVideoEncoding AndAlso TypeOf p.VideoEncoder IsNot NullEncoder AndAlso File.Exists(p.VideoEncoder.OutputPath))
    End Function

    Sub DeleteTempFiles(Optional proj As Project = Nothing)
        If proj Is Nothing Then proj = p

        If proj.DeleteTempFilesMode = DeleteMode.Disabled Then Return
        If String.IsNullOrWhiteSpace(proj.TempDir) OrElse Not proj.TempDir.DirExists() OrElse Not New DirectoryInfo(proj.TempDir).Name.EndsWith("_temp") Then Return
        If JobManager.GetJobs()?.Where(Function(a) a.Path.Contains(proj.TempDir))?.Any() Then Return

        Dim excludeSourcefile = True
        Dim extensions As IEnumerable(Of String) = {}
        Dim howToDelete = If(proj.DeleteTempFilesMode = DeleteMode.RecycleBin, RecycleOption.SendToRecycleBin, RecycleOption.DeletePermanently)

        Select Case proj.DeleteTempFilesSelection
            Case DeleteSelection.Everything
                FolderHelp.Delete(proj.TempDir, howToDelete)
                Exit Sub
            Case DeleteSelection.Custom
                excludeSourcefile = False
                extensions = proj.DeleteTempFilesCustomSelection
            Case DeleteSelection.Selective
                If proj.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Projects) Then extensions = extensions.Union(FileTypes.Projects)
                If proj.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Logs) Then extensions = extensions.Union(FileTypes.Logs)
                If proj.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Scripts) Then extensions = extensions.Union(FileTypes.Scripts)
                If proj.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Indexes) Then extensions = extensions.Union(FileTypes.Indexes)
                If proj.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Videos) Then extensions = extensions.Union(FileTypes.Video.Except(FileTypes.Scripts))
                If proj.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Audios) Then extensions = extensions.Union(FileTypes.Audio)
                If proj.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Subtitles) Then extensions = extensions.Union(FileTypes.SubtitleExludingContainers)
        End Select

        Dim filesInTemp = Directory.GetFiles(proj.TempDir).AsEnumerable()
        If excludeSourcefile Then filesInTemp = filesInTemp.Where(Function(x) x <> proj.SourceFile)
        Dim filesToDelete As IEnumerable(Of String)

        Select Case proj.DeleteTempFilesSelectionMode
            Case SelectionMode.Exclude
                filesToDelete = filesInTemp.Where(Function(x) Not extensions.Contains(x.Ext(), StringComparer.InvariantCultureIgnoreCase))
            Case SelectionMode.Include
                filesToDelete = filesInTemp.Where(Function(x) extensions.Contains(x.Ext(), StringComparer.InvariantCultureIgnoreCase))
            Case Else
        End Select

        FileHelp.Delete(filesToDelete, howToDelete)
    End Sub

    ReadOnly Property StartupTemplatePath As String
        Get
            Dim ret = Path.Combine(Folder.Template, s.StartupTemplate.Replace(" | ", Path.DirectorySeparatorChar) + ".srip")

            If Not File.Exists(ret) Then
                ret = Path.Combine(Folder.Template, "Automatic Workflow.srip")
                s.StartupTemplate = "Automatic Workflow"
            End If

            Return ret
        End Get
    End Property

    ReadOnly Property SettingsFile As String
        Get
            Return Path.Combine(Folder.Settings, "Settings.dat")
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
        For Each track In p.AudioTracks
            If File.Exists(track.AudioProfile.File) AndAlso FileTypes.Audio.Contains(track.AudioProfile.File.Ext()) Then
                Return track.AudioProfile
            End If
        Next
    End Function

    Sub PlayScriptWithMPC(script As VideoScript, Optional cliArgs As String = Nothing)
        If script Is Nothing Then Exit Sub

        script.Synchronize()
        Dim playerPath = Package.MPC.Path

        If Not playerPath.FileExists AndAlso Package.MPC.VerifyOK(True) Then
            playerPath = Package.MPC.Path
        End If

        If Not playerPath.FileExists Then Exit Sub

        Dim args = ""

        If cliArgs <> "" Then
            args += " " + cliArgs
        End If

        Dim ap = GetAudioProfileForScriptPlayback()

        If ap IsNot Nothing AndAlso FileTypes.Audio.Contains(ap.File.Ext) AndAlso
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
        Dim args = ""

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

        If ap IsNot Nothing AndAlso FileTypes.Audio.Contains(ap.File.Ext) AndAlso p.Ranges.Count = 0 Then
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

    Function ExtractTrackNameFromFilename(filename As String) As String
        Return If(filename.Base().Contains("{"), filename.Base().Right("{").LeftLast("}").UnescapeIllegalFileSysChars, Nothing)
    End Function

    Function ExtractLanguageFromPath(path As String) As Language
        If String.IsNullOrWhiteSpace(path) Then Return Nothing

        Dim ret As New Language(CultureInfo.InvariantCulture)
        Dim filename = path.Base

        Dim segments = {Regex.Replace(filename, "{.*}", ""),
                        filename.Right("{").LeftLast("}")} _
                        .Concat(filename.SplitNoEmpty(" ")) _
                        .Concat(filename.SplitNoEmpty(".")) _
                        .Concat(filename.SplitNoEmpty(",")) _
                        .Concat(filename.SplitNoEmpty("_")) _
                        .Concat(Regex.Matches(filename, "(?<=\().+?(?=\))", RegexOptions.IgnoreCase).Cast(Of Match).Select(Function(x) x.Value)) _
                        .Concat(Regex.Matches(filename, "(?<=\[).+?(?=\])", RegexOptions.IgnoreCase).Cast(Of Match).Select(Function(x) x.Value)) _
                        .Concat(Regex.Matches(filename, "(?<=[,\._]\().+?(?=\))", RegexOptions.IgnoreCase).Cast(Of Match).Select(Function(x) x.Value)) _
                        .Concat(Regex.Matches(filename, "(?<=[,\._]\[).+?(?=\])", RegexOptions.IgnoreCase).Cast(Of Match).Select(Function(x) x.Value)) _
                        .Concat({
                            filename.RightLast("_")
                        }).Reverse().Distinct().Reverse()

        For Each extracted In segments
            If String.IsNullOrWhiteSpace(extracted) Then Continue For
            extracted = extracted.Trim()

            For Each lng In Language.Languages.OrderBy(Function(x) x.Name.Length)
                If lng.Name.Equals(extracted, StringComparison.InvariantCultureIgnoreCase) Then
                    ret = lng
                    Exit For
                ElseIf lng.EnglishName.Equals(extracted, StringComparison.InvariantCultureIgnoreCase) Then
                    ret = lng
                    Exit For
                ElseIf lng.ThreeLetterCode.Equals(extracted, StringComparison.InvariantCultureIgnoreCase) Then
                    ret = lng
                    Exit For
                ElseIf lng.TwoLetterCode.Equals(extracted, StringComparison.InvariantCultureIgnoreCase) Then
                    ret = lng
                    Exit For
                End If
            Next
        Next

        If ret Is Nothing OrElse Not ret.IsDetermined Then
            For Each lng In Language.Languages.OrderByDescending(Function(x) x.Name.Length)
                If lng.EnglishName.Contains(" (") AndAlso path.ToUpperInvariant().Contains(lng.EnglishName.Left(" (").ToUpperInvariant()) Then
                    ret = lng
                End If

                If path.ToUpperInvariant().Contains(lng.LocalName.ToUpperInvariant()) Then
                    ret = lng
                End If

                If path.ToUpperInvariant().Contains(lng.EnglishName.ToUpperInvariant()) Then
                    ret = lng

                    If filename.ToUpperInvariant().Contains(lng.EnglishName.ToUpperInvariant()) Then
                        ret = lng
                        Exit For
                    End If
                End If
            Next
        End If

        Return ret
    End Function

    Function GetSourceBase() As String
        Return If(p.TempDir <> "" AndAlso New DirectoryInfo(p.TempDir).Name.EndsWithEx("_temp"), "temp", p.SourceFile.Base)
    End Function

    Sub ShowCode(title As String, content As String, Optional find As String = Nothing, Optional wordwrap As Boolean = False)
        Dim form As New CodeForm(content, find, wordwrap) With {
            .Text = $"{title} - {g.DefaultCommands.GetApplicationDetails()}"
        }
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

        If dialogAction IsNot Nothing Then
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

        Return If(ret <> "", ret, "notepad.exe")
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

    Function ObfuscateLogFile(text As String, Optional proj As Project = Nothing, Optional folderReplacement As String = "x", Optional fileReplacement As String = "Source") As String
        If String.IsNullOrWhiteSpace(text) Then Return ""
        If proj Is Nothing Then proj = p
        If String.IsNullOrWhiteSpace(p.SourceFile) Then Return text

        Dim ret = text
        Dim sourcePath = proj.SourceFile
        Dim toReplace = sourcePath.DirAndBase()
        Dim replaceBy = Regex.Replace(toReplace, $"[^{Path.VolumeSeparatorChar}\{Path.DirectorySeparatorChar}]+(?=\{Path.DirectorySeparatorChar})", folderReplacement)
        replaceBy = Regex.Replace(replaceBy, $"(?<=\{Path.DirectorySeparatorChar})[^\{Path.DirectorySeparatorChar}]+$", fileReplacement)
        ret = ret.Replace(toReplace, replaceBy)

        toReplace = sourcePath.Base()
        replaceBy = fileReplacement
        ret = ret.Replace(toReplace, replaceBy)

        Return ret
    End Function

    Sub SaveSettings()
        Try
            Using mutex As New Mutex(False, "staxrip settings file")
                mutex.WaitOne()
                SafeSerialization.Serialize(s, g.SettingsFile)
                mutex.ReleaseMutex()
            End Using

            Dim backupPath = Path.Combine(Folder.Settings, "Backup")

            If Not Directory.Exists(backupPath) Then
                Directory.CreateDirectory(backupPath)
            End If

            FileHelp.Copy(g.SettingsFile, Path.Combine(backupPath, "Settings(v" + Application.ProductVersion + ").dat"))
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

    Sub LoadAudioProfile(profile As Profile, index As Integer)
        If profile Is Nothing Then Exit Sub
        If index < 0 Then Exit Sub
        If index > p.AudioTracksAvailable - 1 Then Exit Sub
        If p.AudioTracks Is Nothing Then Exit Sub
        If p.AudioTracks.Count < 1 Then Exit Sub
        If p.AudioTracks(index).AudioProfile Is Nothing Then Exit Sub

        Dim audioTrack = p.AudioTracks(index)

        Dim commentary = audioTrack.AudioProfile.Commentary
        Dim [default] = audioTrack.AudioProfile.Default
        'Dim delay = audioTrack.AudioProfile.Delay
        Dim file = audioTrack.AudioProfile.File
        Dim forced = audioTrack.AudioProfile.Forced
        Dim language = audioTrack.AudioProfile.Language
        Dim stream = audioTrack.AudioProfile.Stream
        Dim streamName = audioTrack.AudioProfile.StreamName
        Dim streams = audioTrack.AudioProfile.Streams
        audioTrack.AudioProfile = DirectCast(ObjectHelp.GetCopy(profile), AudioProfile)
        audioTrack.AudioProfile.Commentary = commentary
        audioTrack.AudioProfile.Default = [default]
        audioTrack.AudioProfile.File = file
        audioTrack.AudioProfile.Forced = forced
        audioTrack.AudioProfile.Language = language
        audioTrack.AudioProfile.Stream = stream
        audioTrack.AudioProfile.StreamName = If(audioTrack.AudioProfile.StreamName = "", streamName, audioTrack.AudioProfile.StreamName)
        audioTrack.AudioProfile.Streams = streams
        audioTrack.AudioProfile.Delay += g.ExtractDelay(file)

        audioTrack.NameLabel.Refresh()
        g.MainForm.UpdateSizeOrBitrate()
        g.MainForm.Assistant()
    End Sub

    Sub RaiseAppEvent(ae As ApplicationEvent, Optional commandline As String = Nothing, Optional progress As Single = -1.0F, Optional progressline As String = Nothing)
        Select Case ae
            Case ApplicationEvent.AfterJobAdded
                RaiseEvent AfterJobAdded()
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
            Case ApplicationEvent.AfterSourceOpened
                RaiseEvent AfterSourceOpened()
            Case ApplicationEvent.AfterSourceLoaded
                RaiseEvent AfterSourceLoaded()
            Case ApplicationEvent.AfterVideoEncoded
                RaiseEvent AfterVideoEncoded()
            Case ApplicationEvent.ApplicationExit
                RaiseEvent ApplicationExit()
            Case ApplicationEvent.BeforeJobAdding
                RaiseEvent BeforeJobAdding()
            Case ApplicationEvent.BeforeJobProcessed
                RaiseEvent BeforeJobProcessed()
            Case ApplicationEvent.BeforeMuxingWhenSourceOpening
                RaiseEvent BeforeMuxing()
            Case ApplicationEvent.BeforeProcessing
                RaiseEvent BeforeProcessing()
            Case ApplicationEvent.WhileProcessing
                RaiseEvent WhileProcessing(commandline, progress, progressline)
        End Select

        Dim scriptPath = Path.Combine(Folder.Scripts, ae.ToString + ".ps1")

        If File.Exists(scriptPath) Then
            g.DefaultCommands.ExecutePowerShellFile(scriptPath)
        End If

        For Each ec In s.EventCommands
            If ec.Enabled AndAlso ec.Event = ae Then
                Dim matches = 0

                For Each criteria In ec.CriteriaList
                    criteria.PropertyString = Macro.ExpandWhileProcessing(criteria.Macro, p, commandline, progress, progressline)

                    If criteria.Eval Then
                        matches += 1
                    End If
                Next

                If (ec.CriteriaList.Count = 0 OrElse (ec.OrOnly AndAlso matches > 0) OrElse
                    (Not ec.OrOnly AndAlso matches = ec.CriteriaList.Count)) AndAlso
                    ec.CommandParameters IsNot Nothing Then

                    Dim command = g.MainForm.CustomMainMenu.CommandManager.GetCommand(ec.CommandParameters.MethodName)
                    Dim params = command.FixParameters(ec.CommandParameters.Parameters)

                    If s.LogEventCommand AndAlso p.SourceFile <> "" Then
                        Log.WriteHeader("Event Command: " + ec.Name)
                        Log.WriteLine("Event: " + DispNameAttribute.GetValueForEnum(ec.Event))
                        Log.WriteLine("Command: " + command.MethodInfo.Name)
                        Log.WriteLine(command.GetParameterHelp(ec.CommandParameters.Parameters))
                    End If

                    For i = 0 To params.Count - 1
                        If TypeOf params(i) Is String Then
                            params(i) = Macro.ExpandWhileProcessing(DirectCast(params(i), String), p, commandline, progress, progressline)
                        End If
                    Next

                    g.MainForm.CustomMainMenu.CommandManager.ProcessPlusFixParams(command, params)
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
                    p.TempDir = If(New DirectoryInfo(p.SourceFile.Dir).Name.EndsWithEx("_temp"), p.SourceFile.Dir, Path.Combine(p.SourceFile.Dir, p.SourceFile.Base + p.SourceFile.ExtFull + "_temp"))
                End If

                p.TempDir = p.TempDir.FixDir

                If Not Directory.Exists(p.TempDir) Then
                    Try
                        Directory.CreateDirectory(p.TempDir)
                    Catch
                        Try
                            p.TempDir = p.SourceFile.DirAndBase + p.SourceFile.ExtFull + "_temp"

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

    Sub ShowCommandLinePreview(title As String, value As String, lineNumbers As Boolean)
        If lineNumbers Then
            Dim sb = New StringBuilder()
            Dim lines = Regex.Split(value, "\r\n|\r|\n")

            For i = 0 To lines.Length - 1
                sb.AppendLine($"{i + 1,3}: {lines(i)}")
            Next

            value = sb.ToString()
        End If

        Select Case s.CommandLinePreview
            Case CommandLinePreview.CodePreview
                ShowCodePreview(value, Nothing, True)
            Case CommandLinePreview.Powershell
                Environment.SetEnvironmentVariable("CommandLineToShow", BR + value + BR)
                g.Execute("powershell.exe", "-NoLogo -NoExit -NoProfile -Command $env:CommandLineToShow")
            Case CommandLinePreview.WindowsTerminal
                Environment.SetEnvironmentVariable("CommandLineToShow", BR + value + BR)

                If g.IsWindowsTerminalAvailable Then
                    g.Execute("wt.exe", "powershell.exe -NoLogo -NoExit -NoProfile -Command $env:CommandLineToShow")
                Else
                    MsgWarn("Windows Terminal not found!", "Windows Terminal could not be found and thus Powershell will be used to show the command line preview!")
                    g.Execute("powershell.exe", "-NoLogo -NoExit -NoProfile -Command $env:CommandLineToShow")
                End If
        End Select
    End Sub

    Sub ShowCodePreview(code As String, Optional find As String = Nothing, Optional wordwrap As Boolean = False)
        ShowCode("Code Preview", code, find, wordwrap)
    End Sub

    Sub ffmsindex(sourcePath As String, cachePath As String, Optional indexAudio As Boolean = False, Optional proj As Project = Nothing)
        If File.Exists(sourcePath) AndAlso Not File.Exists(cachePath) AndAlso Not FileTypes.VideoText.Contains(sourcePath.Ext) Then
            Using proc As New Proc
                proc.Header = "Indexing using ffmsindex"
                proc.SkipString = "Indexing, please wait..."
                proc.Project = proj
                proc.Priority = ProcessPriorityClass.Normal
                proc.File = Path.Combine(Package.ffms2.Directory, "ffmsindex.exe")
                proc.Arguments = If(indexAudio, "-t -1 ", "") + sourcePath.LongPathPrefix.Escape + " " + cachePath.LongPathPrefix.Escape
                proc.Start()
            End Using
        End If
    End Sub

    Function IsValidSource(Optional warn As Boolean = True) As Boolean
        p.SourceScript.Synchronize(False, False)

        If p.SourceScript.Info.FrameCount = 0 Then
            If warn Then
                MsgWarn("Failed to load source.")
            End If

            Return False
        End If

        If p.SourceScript.Error <> "" Then
            MsgError("Script Error", p.SourceScript.Error)
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

        If Not String.IsNullOrWhiteSpace(p.TempDir) AndAlso New DirectoryInfo(p.TempDir).Name.EndsWithEx("_temp") Then
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
        If ExceptionHandled Then Exit Sub

        ExceptionHandled = True

        If TypeOf ex IsNot AbortException Then
            Try
                If File.Exists(p.SourceFile) Then
                    Dim name = p.TargetFile.Base

                    If name = "" Then
                        name = p.SourceFile.Base
                    End If

                    g.MainForm.SaveProjectPath(Path.Combine(p.SourceFile.Dir, "recovery.srip"))
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

    Sub ShowException(ex As Exception, Optional title As String = Nothing, Optional content As String = Nothing, Optional timeout As Integer = 0)
        If timeout < 0 Then Exit Sub

        Try
            Using td As New TaskDialog(Of String)
                td.Title = If(title = "",
                    If(TypeOf ex Is ErrorAbortException,
                        DirectCast(ex, ErrorAbortException).Title + $" ({g.DefaultCommands.GetApplicationDetails(False, True, True)})",
                        ex.GetType.Name + $" ({g.DefaultCommands.GetApplicationDetails(False, True, True)})"),
                    title)

                td.Timeout = timeout
                td.Content = (ex.Message + BR2 + content).Trim()
                td.Icon = TaskIcon.Error
                td.ExpandedContent = ex.ToString().Trim()
                td.ShowCopyButton = True
                td.AddButton("OK")
                td.Show()
            End Using
        Catch
            Dim msg = If(TypeOf ex Is ErrorAbortException, DirectCast(ex, ErrorAbortException).Title, ex.GetType.Name)

            VB6.MsgBox(msg + BR2 + ex.Message + BR2 + ex.ToString, VB6.MsgBoxStyle.Critical)
        End Try
    End Sub

    Sub ArchiveLogFile(path As String)
        Try
            Dim logFolder = IO.Path.Combine(Folder.Settings, "Log Files")
            Dim di = If(Directory.Exists(logFolder), Nothing, Directory.CreateDirectory(logFolder))

            FileHelp.Copy(path, IO.Path.Combine(logFolder, Date.Now.ToString("yyyy-MM-dd - HH.mm.ss") + " - " + path.FileName))

            If di Is Nothing Then
                di = New DirectoryInfo(logFolder)
                Dim files = di.GetFiles("*.log").OrderByDescending(Function(x) x.LastWriteTime).Skip(s.LogFileNum).Select(Function(x) x.FullName)
                FileHelp.Delete(files, RecycleOption.DeletePermanently)
            End If
        Catch ex As Exception
            ShowException(ex, "Failed to archive log file")
        End Try
    End Sub

    Sub SetRenderer(ms As ToolStrip)
        ms.Renderer = If(VisualStyleInformation.IsEnabledByUser, New ToolStripRendererEx(), DirectCast(New ToolStripSystemRenderer(), ToolStripRenderer))
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

        If e IsNot Nothing Then
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
        For Each Filter In p.Script.Filters
            If Filter.Category = search Then
                If Not Filter.Active Then
                    Filter.Active = True
                    g.MainForm.FiltersListView.Load()
                End If

                Return True
            End If
        Next
    End Function

    Function AddResizeFilter() As Boolean
        If Not g.EnableFilter("Resize") Then
            If p.Script.IsAviSynth Then
                p.Script.AddFilter(New VideoFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)"))
            Else
                p.Script.AddFilter(New VideoFilter("Resize", "Bicubic", "clip = core.resize.Bicubic(clip, %target_width%, %target_height%)"))
            End If
        End If
        Return True
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
            form.Text = $"{script.Path} - {g.DefaultCommands.GetApplicationDetails()}"
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

    Sub CheckForModifiedDolbyVisionLevel5Data()
        If p.HdrDolbyVisionMetadataFile IsNot Nothing Then
            If p.HdrDolbyVisionMetadataFile.HasLevel5Changed Then
                p.HdrDolbyVisionMetadataFile.RefreshLevel5Data()
                'MainForm.AutoCrop()
                'Dim newCrop = p.HdrDolbyVisionMetadataFile.Crop
                'g.SetCrop(newCrop.Left, newCrop.Top, newCrop.Right, newCrop.Bottom, True, False)
            End If
        End If
    End Sub

    Sub CheckForLongPathSupport()
        If Not s.CheckForLongPathSupport Then Return

        Try
            Dim root = "HKLM"
            Dim key = "SYSTEM\CurrentControlSet\Control\FileSystem"
            Dim name = "LongPathsEnabled"
            Dim enabled = Registry.LocalMachine.GetBoolean(key, name)
            If Not enabled Then
                Using td = New TaskDialog(Of DialogResult)
                    td.Icon = TaskIcon.Question
                    td.Timeout = s.ErrorMessageTimeout
                    td.Title = "Enable Long Path Support?"
                    td.Content = $"Long Path Support is disabled in your Registry under:{BR}{root}\{key}\{name}{BR}This can cause issues with long paths as well as Unicode characters within the paths.{BR2}Do you want to enable it?"
                    td.AddButton("Yes", DialogResult.Yes)
                    td.AddButton("No", DialogResult.No, True)
                    td.AddButton("No and disable the check", DialogResult.Ignore)
                    td.Show()

                    If td.SelectedValue = DialogResult.Yes Then
                        Try
                            Dim psi As New ProcessStartInfo With {
                                .FileName = "reg.exe",
                                .Arguments = $"add {root}\{key} /v {name} /t REG_DWORD /d 1 /f",
                                .Verb = "runas",
                                .UseShellExecute = True
                            }

                            Using regProcess = Process.Start(psi)
                                regProcess.WaitForExit()

                                If regProcess.ExitCode <> 0 Then
                                    MsgError("Something went wrong, the Long Path Support was not enabled!")
                                End If
                            End Using
                        Catch ex As Exception
                            MsgError("Something went wrong, the Long Path Support was not enabled!" + BR2 + ex.Message)
                        End Try
                    ElseIf td.SelectedValue = DialogResult.Ignore Then
                        s.CheckForLongPathSupport = False
                    End If
                End Using
            End If
        Catch ex As Exception
        End Try
    End Sub

    Async Sub PreloadValuesAsync()
        Await Task.Run(Sub()
                           Dim a = OS.Hardware.Cores
                       End Sub)
    End Sub

    Sub SetCrop(left As Integer, top As Integer, right As Integer, bottom As Integer, direction As ForceOutputModDirection, Optional force As Boolean = False)
        p.CropLeft = Math.Max(0, left)
        p.CropTop = Math.Max(0, top)
        p.CropRight = Math.Max(0, right)
        p.CropBottom = Math.Max(0, bottom)

        CorrectCropMod(direction, force)
        MainForm.SetCropFilter()
        MainForm.DisableCropFilter()
        MainForm.Assistant()
    End Sub

    Sub RunAutoCrop(progressAction As Action(Of Double))
        If p.SourceScript Is Nothing Then Return
        If String.IsNullOrWhiteSpace(p.SourceScript.Path) Then Return
        If Not p.SourceScript.Path.FileExists() Then Return

        p.SourceScript.Synchronize(True, True, True)

        If p.HdrDolbyVisionMetadataFile IsNot Nothing Then
            Dim c = p.HdrDolbyVisionMetadataFile.Crop
            SetCrop(c.Left, c.Top, c.Right, c.Bottom, ForceOutputModDirection.Decrease, True)
        Else
            Using server = FrameServerFactory.Create(p.SourceScript.Path)
                Dim info = server.Info
                Dim frameCount = info.FrameCount
                Dim frameRate = info.FrameRate
                Dim startFrame = 0
                Dim endFrame = frameCount - 1
                If p.AutoCropFrameRangeMode = AutoCropFrameRangeMode.Automatic Then
                    Dim threshold = CInt(VB6.Conversion.Fix(frameCount * 0.05))
                    startFrame += threshold
                    endFrame -= threshold
                ElseIf p.AutoCropFrameRangeMode = AutoCropFrameRangeMode.ManualThreshold Then
                    startFrame += p.AutoCropFrameRangeThresholdBegin
                    endFrame -= p.AutoCropFrameRangeThresholdEnd
                End If
                Dim consideredFrames = endFrame - startFrame
                Dim minFrames = 5
                Dim interval = CInt(VB6.Conversion.Fix(frameRate * 60))
                If p.AutoCropFrameSelectionMode = AutoCropFrameSelectionMode.FixedFrames Then
                    interval = consideredFrames \ (p.AutoCropFixedFramesFrameSelection - 1)
                ElseIf p.AutoCropFrameSelectionMode = AutoCropFrameSelectionMode.FrameInterval Then
                    interval = p.AutoCropFrameIntervalFrameSelection
                    If interval * minFrames > consideredFrames Then
                        interval = CInt(VB6.Conversion.Fix(consideredFrames / minFrames))
                    End If
                ElseIf p.AutoCropFrameSelectionMode = AutoCropFrameSelectionMode.TimeInterval Then
                    interval = CInt(VB6.Conversion.Fix(frameRate * p.AutoCropTimeIntervalFrameSelection))
                    If interval * minFrames > consideredFrames Then
                        interval = CInt(VB6.Conversion.Fix(consideredFrames / minFrames))
                    End If
                End If
                Dim analyzeCount = (consideredFrames \ interval) + 1
                Dim analyzeFrames(analyzeCount - 1) As Integer
                Dim crops(analyzeCount - 1) As AutoCrop
                Dim offset = (consideredFrames - ((analyzeCount - 1) * interval)) \ 2
                startFrame += offset
                Dim luminanceThreshold = p.AutoCropLuminanceThreshold / 100

                For i = 0 To analyzeCount - 1
                    analyzeFrames(i) = Math.Min(startFrame + i * interval, endFrame)
                Next

                For i = 0 To analyzeCount - 1
                    progressAction?.Invoke(i / analyzeCount * 100)

                    Dim frame = analyzeFrames(i)
                    Using bmp = BitmapUtil.CreateBitmap(server, frame)
                        crops(i) = AutoCrop.Start(bmp.Clone(New Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format32bppRgb), luminanceThreshold)
                    End Using

                    If {crops(i).Left.Min, crops(i).Top.Min, crops(i).Right.Min, crops(i).Bottom.Min}.Max() = 0 Then
                        progressAction?.Invoke(100.0)
                        Exit For
                    End If
                Next

                Dim left = crops.Where(Function(x) x IsNot Nothing).SelectMany(Function(arg) arg.Left).Min()
                Dim top = crops.Where(Function(x) x IsNot Nothing).SelectMany(Function(arg) arg.Top).Min()
                Dim right = crops.Where(Function(x) x IsNot Nothing).SelectMany(Function(arg) arg.Right).Min()
                Dim bottom = crops.Where(Function(x) x IsNot Nothing).SelectMany(Function(arg) arg.Bottom).Min()

                SetCrop(left, top, right, bottom, p.ForcedOutputModDirection, False)
            End Using
        End If
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

            CorrectCropMod(p.ForcedOutputModDirection, False)
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

    Sub ForceCropMod()
        If Not g.EnableFilter("Crop") Then
            p.Script.InsertAfter("Source", New VideoFilter("Crop", "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)"))
        End If

        CorrectCropMod(p.ForcedOutputModDirection, True)
    End Sub

    Sub CorrectCropMod(direction As ForceOutputModDirection, force As Boolean)
        If p.AutoCorrectCropValues OrElse force Then
            If direction = ForceOutputModDirection.Increase Then
                p.CropLeft += p.CropLeft Mod 2
                p.CropRight += p.CropRight Mod 2
                p.CropTop += p.CropTop Mod 2
                p.CropBottom += p.CropBottom Mod 2
            Else
                p.CropLeft -= p.CropLeft Mod 2
                p.CropRight -= p.CropRight Mod 2
                p.CropTop -= p.CropTop Mod 2
                p.CropBottom -= p.CropBottom Mod 2
            End If

            Dim modValue = 4

            If Not p.Script.IsFilterActive("Resize") Then
                modValue = p.ForcedOutputMod
            End If

            Dim whalf = ((p.SourceWidth - p.CropLeft - p.CropRight) Mod modValue) \ 2
            Dim hhalf = ((p.SourceHeight - p.CropTop - p.CropBottom) Mod modValue) \ 2

            If direction = ForceOutputModDirection.Increase Then
                If p.CropLeft > p.CropRight Then
                    p.CropLeft += whalf - whalf Mod 2
                    p.CropRight += whalf + whalf Mod 2
                Else
                    p.CropRight += whalf - whalf Mod 2
                    p.CropLeft += whalf + whalf Mod 2
                End If

                If p.CropTop > p.CropBottom Then
                    p.CropTop += hhalf - hhalf Mod 2
                    p.CropBottom += hhalf + hhalf Mod 2
                Else
                    p.CropBottom += hhalf - hhalf Mod 2
                    p.CropTop += hhalf + hhalf Mod 2
                End If
            Else
                If p.CropLeft > p.CropRight Then
                    p.CropLeft -= whalf - whalf Mod 2
                    p.CropRight -= whalf + whalf Mod 2
                Else
                    p.CropRight -= whalf - whalf Mod 2
                    p.CropLeft -= whalf + whalf Mod 2
                End If

                If p.CropTop > p.CropBottom Then
                    p.CropTop -= hhalf - hhalf Mod 2
                    p.CropBottom -= hhalf + hhalf Mod 2
                Else
                    p.CropBottom -= hhalf - hhalf Mod 2
                    p.CropTop -= hhalf + hhalf Mod 2
                End If
            End If

            g.MainForm.FiltersListView.Load()
        End If
    End Sub

    Function GetTimeString(sec As Double) As String
        Dim ts = TimeSpan.FromSeconds(sec)

        Return If(ts.TotalHours > 0,
            $"{Math.Floor(ts.TotalHours):0}:{Math.Floor(ts.Minutes):00}:{Math.Floor(ts.Seconds):00}",
            $"{Math.Floor(ts.TotalMinutes):00}:{Math.Floor(ts.Seconds):00}")
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
                        infoScript.Path = Path.Combine(p.TempDir, p.TargetFile.Base + $"_info." + script.FileType)

                        Dim errorMsg = infoScript.GetError()
                        If errorMsg <> "" Then
                            MsgError("Script Error", errorMsg)
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
                        infoScript.Path = Path.Combine(p.TempDir, p.TargetFile.Base + "_info." + script.FileType)
                        infoScript.AddFilter(New VideoFilter("clip = clip.resize.Bicubic(720, (720 / clip.width * clip.height) // 8 * 8)"))
                        infoScript.AddFilter(New VideoFilter("clip = core.text.ClipInfo(clip)"))

                        Dim errorMsg = infoScript.GetError()
                        If errorMsg <> "" Then
                            MsgError("Script Error", errorMsg)
                            Exit Sub
                        End If

                        g.PlayScriptWithMPV(infoScript, "--pause=yes --osc=no --osd-level=0")
                End Select
            End Using
        End If
    End Sub

    Function IsDevelopmentPC() As Boolean
        Return New DirectoryInfo(Application.StartupPath).Name.EndsWithEx("bin") OrElse New DirectoryInfo(Application.StartupPath).Name.EndsWithEx("bin-x86")
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
        Dim ret = ""

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

        Return If(script.Engine = ScriptEngine.AviSynth, ret, "clip = " + ret)
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

    Function GetCodeFont(Optional size As Single = 10.0) As Font
        Return New Font(If(s.CodeFont = "", "Consolas", s.CodeFont), size * s.UIScaleFactor)
    End Function
End Class
