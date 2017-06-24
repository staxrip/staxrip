Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports StaxRip.UI

Public Class GlobalCommands
    <Command("Allows to use StaxRip's demuxing GUIs independently.")>
    Sub ShowDemuxTool()
        Using form As New SimpleSettingsForm("Demux")
            form.ScaleClientSize(27, 10)
            Dim ui = form.SimpleUI
            Dim page = ui.CreateFlowPage("main page")
            page.SuspendLayout()

            Dim sourceFile = ui.AddTextButtonBlock(page)
            sourceFile.Label.Text = "Source File:"
            sourceFile.Edit.Expandet = True
            sourceFile.BrowseFile(FileTypes.VideoAudio)

            Dim outputFolder = ui.AddTextButtonBlock(page)
            outputFolder.Label.Text = "Output Folder:"
            outputFolder.Edit.Expandet = True
            outputFolder.BrowseFolder()

            page.ResumeLayout()

            AddHandler sourceFile.Edit.TextChanged, Sub()
                                                        If outputFolder.Edit.Text = "" AndAlso
                                                            File.Exists(sourceFile.Edit.Text) Then

                                                            outputFolder.Edit.Text = sourceFile.Edit.Text.Dir
                                                        End If
                                                    End Sub
            form.FileDrop = True
            AddHandler form.FilesDropped, Sub(files) sourceFile.Edit.Text = files(0)

            If form.ShowDialog() = DialogResult.OK AndAlso
                    File.Exists(sourceFile.Edit.Text) AndAlso
                    Directory.Exists(outputFolder.Edit.Text) Then

                Using td As New TaskDialog(Of Demuxer)
                    td.MainInstruction = "Select a demuxer."
                    If sourceFile.Edit.Text.Ext = "mkv" Then td.AddCommandLink("mkvextract", New mkvDemuxer)
                    If sourceFile.Edit.Text.Ext.EqualsAny("mp4", "flv") Then td.AddCommandLink("MP4Box", New MP4BoxDemuxer)
                    td.AddCommandLink("ffmpeg", New ffmpegDemuxer)
                    td.AddCommandLink("eac3to", New eac3toDemuxer)

                    Dim proj As New Project
                    proj.Init()
                    proj.SourceFile = sourceFile.Edit.Text
                    proj.TempDir = outputFolder.Edit.Text.AppendSeparator

                    If Not td.Show Is Nothing Then
                        td.SelectedValue.Run(proj)
                        ProcessForm.CloseProcessForm()
                        s.LastSourceDir = proj.SourceFile.Dir
                    End If
                End Using
            End If
        End Using
    End Sub

    <Command("Runs all active jobs of the job list.")>
    Sub StartJobs()
        'g.RunJobRecursive()
        g.ShellExecute(Application.ExecutablePath, "-RunJobsMaximized")
    End Sub

    <Command("Shows a command prompt with the temp directory of the current project.")>
    Sub ShowCommandPrompt()
        Dim batchCode = ""

        For Each pack In Package.Items.Values
            If TypeOf pack Is PluginPackage Then Continue For
            Dim dir = pack.GetDir
            If Not Directory.Exists(dir) Then Continue For
            If Not dir.Contains(Folder.Startup) Then Continue For

            batchCode += "@set PATH=" + dir + ";%PATH%" + BR
        Next

        Dim batchPath = Folder.Temp + Guid.NewGuid.ToString + ".bat"
        Proc.WriteBatchFile(batchPath, batchCode)

        AddHandler g.MainForm.Disposed, Sub() FileHelp.Delete(batchPath)

        Dim batchProcess As New Process
        batchProcess.StartInfo.FileName = "cmd.exe"
        batchProcess.StartInfo.Arguments = "/k """ + batchPath + """"
        batchProcess.StartInfo.WorkingDirectory = p.TempDir
        batchProcess.Start()
    End Sub

    <Command("Executes command lines separated by a line break line by line. Macros are solved as well as passed in as environment variables.")>
    Sub ExecuteCommandLine(
        <DispName("Command Line"),
        Description("One or more command lines to be executed or if batch mode is used content of the batch file. Macros are solved as well as passed in as environment variables."),
        Editor(GetType(CommandLineTypeEditor), GetType(UITypeEditor))>
        commandLines As String,
        <DispName("Wait For Exit"),
        Description("This will halt the main thread until the command line returns."),
        DefaultValue(False)>
        waitForExit As Boolean,
        <DispName("Show Process Window"),
        Description("Redirects the output of command line apps to the process window."),
        DefaultValue(False)>
        showProcessWindow As Boolean,
        <DispName("Batch Mode"),
        Description("Alternative mode that creats a BAT file to execute."),
        DefaultValue(False)>
        asBatch As Boolean)

        Dim closeNeeded As Boolean

        If showProcessWindow AndAlso Not ProcessForm.IsActive Then
            closeNeeded = True
        End If

        If asBatch Then
            Dim batchPath = Folder.Temp + Guid.NewGuid.ToString + ".bat"
            Dim batchCode = Macro.Expand(commandLines)
            File.WriteAllText(batchPath, batchCode, Encoding.Default)
            AddHandler g.MainForm.Disposed, Sub() FileHelp.Delete(batchPath)

            Using proc As New Proc
                If showProcessWindow Then proc.Init("Execute Command Line")
                proc.WriteLine(batchCode + BR2)
                proc.File = "cmd.exe"
                proc.Arguments = "/C call """ + batchPath + """"
                proc.Wait = waitForExit
                proc.Process.StartInfo.UseShellExecute = False

                For Each i In Macro.GetMacros
                    proc.Process.StartInfo.EnvironmentVariables(i.Name.Trim("%"c)) = Macro.Expand(i.Name)
                Next

                Try
                    proc.Start()
                Catch ex As Exception
                    g.ShowException(ex)
                    Log.WriteLine(ex.Message)
                End Try
            End Using
        Else
            For Each i In Macro.Expand(commandLines).SplitLinesNoEmpty
                Using proc As New Proc
                    If showProcessWindow Then proc.Init("Execute Command Line")
                    proc.CommandLine = i
                    proc.Wait = waitForExit

                    If i.Ext = "exe" Then
                        proc.Process.StartInfo.UseShellExecute = False

                        For Each i2 In Macro.GetMacros
                            proc.Process.StartInfo.EnvironmentVariables(i2.Name.Trim("%"c)) = Macro.Expand(i2.Name)
                        Next
                    End If

                    Try
                        proc.Start()
                    Catch ex As Exception
                        g.ShowException(ex)
                        Log.WriteLine(ex.Message)
                    End Try
                End Using
            Next
        End If

        If closeNeeded Then ProcessForm.CloseProcessForm()
    End Sub

    <Command("Saves a batch script as bat file and executes it. Macros are solved as well as passed in as environment variables.")>
    Sub ExecuteBatchScript(
        <DispName("Batch Script Code"),
        Description("Batch script code to be executed. Macros are solved as well as passed in as environment variables."),
        Editor(GetType(CommandLineTypeEditor), GetType(UITypeEditor))>
        batchScript As String,
        <DispName("Interpret Output"),
        Description("Interprets each output line as StaxRip command."),
        DefaultValue(False)>
        Optional interpretOutput As Boolean = False)

        Dim closeNeeded As Boolean

        If Not ProcessForm.IsActive Then closeNeeded = True

        Dim batchPath = Folder.Temp + Guid.NewGuid.ToString + ".bat"
        Dim batchCode = Macro.Expand(batchScript)
        File.WriteAllText(batchPath, batchCode, Encoding.Default)
        AddHandler g.MainForm.Disposed, Sub() FileHelp.Delete(batchPath)

        Using proc As New Proc
            proc.Init("Execute Batch Script")
            proc.WriteLine(batchCode + BR2)
            proc.File = "cmd.exe"
            proc.Arguments = "/C call """ + batchPath + """"
            proc.Wait = True
            proc.Process.StartInfo.UseShellExecute = False

            For Each i In Macro.GetMacros
                proc.Process.StartInfo.EnvironmentVariables(i.Name.Trim("%"c)) = Macro.Expand(i.Name)
            Next

            Try
                proc.Start()

                For Each i In ProcessForm.CommandLineLog.ToString.SplitLinesNoEmpty
                    If Not g.MainForm.CommandManager.ProcessCommandLineArgument(i) Then
                        Log.WriteLine("Failed to interpret output:" + BR2 + i)
                    End If
                Next
            Catch ex As Exception
                g.ShowException(ex)
                Log.WriteLine(ex.Message)
            End Try
        End Using

        If closeNeeded Then ProcessForm.CloseProcessForm()
    End Sub

    <Command("Executes a PowerShell (*.ps1) script.")>
    Sub ExecuteScriptFile(<DispName("File Path")>
                          <Description("Filepath to a PowerShell (*.ps1) script, the path may contain macros.")>
                          <Editor(GetType(OpenFileDialogEditor), GetType(UITypeEditor))>
                          filepath As String)

        If File.Exists(filepath) Then
            If filepath.Ext = "ps1" Then
                ExecutePowerShellScript(File.ReadAllText(filepath))
            Else
                MsgError("Only PowerShell (*.ps1) is supported.")
            End If
        Else
            MsgError("File is missing:" + BR2 + filepath)
        End If
    End Sub

    'TODO: legacy
    <Command("Executes C# script code.")>
    Sub ExecuteCSharpScript(scriptCode As String)
        Scripting.RunCSharp(scriptCode)
    End Sub

    <Command("Starts a tool by name as shown in the app manage dialog.")>
    Sub StartTool(<DispName("Tool Name")>
                  <Description("Tool name as shown in the app manage dialog.")>
                  name As String)

        Try
            If Package.Items(name).VerifyOK Then Package.Items(name).StartAction?.Invoke
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    <Command("Executes PowerShell script code.")>
    Sub ExecutePowerShellScript(<DispName("Script Code")>
                                <Description("PowerShell script code to be executed.")>
                                <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
                                scriptCode As String)

        Scripting.RunPowershell(scriptCode)
    End Sub

    <Command("Test")>
    Sub Test()
        Dim msg = ""

        Dim nvExcept = "--help --version --check-device --avsw --input-analyze
        --input-format --output-format --video-streamid --video-track --vpp-delogo
        --vpp-delogo-cb --vpp-delogo-cr --vpp-delogo-depth --vpp-delogo-pos
        --vpp-delogo-select --vpp-delogo-y --check-avversion --check-codecs
        --check-encoders --check-decoders --check-formats --check-protocols
        --check-filters --input --output --raw --avs --vpy --vpy-mt
        --avcuvid-analyze --audio-source --audio-file --seek --format --audio-copy
        --audio-copy --audio-codec --vpp-perf-monitor --avi
        --audio-bitrate --audio-ignore --audio-ignore --audio-samplerate --audio-resampler --audio-stream
        --audio-stream --audio-stream --audio-stream --audio-filter --chapter-copy --chapter --sub-copy
        --avsync --mux-option --input-res --fps --dar --audio-ignore-decode-error --audio-ignore-notrack-error
        --log --log-framelist".Split((" " + BR).ToCharArray())
        File.WriteAllText(Package.NVEnc.GetDir + "help.txt", ProcessHelp.GetStdOut(Package.NVEnc.Path, "-h"))
        Dim nvHelp = File.ReadAllText(Package.NVEnc.GetDir + "help.txt").Replace("(no-)", "").Replace("--no-", "--")
        Dim nvHelpSwitches = Regex.Matches(nvHelp, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
        Dim nvCode = File.ReadAllText(Folder.Startup.Parent + "Encoding\nvenc.vb").Replace("--no-", "--")
        Dim nvPresent = Regex.Matches(nvCode, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
        Dim nvMissing = nvPresent.Where(Function(arg) Not nvHelpSwitches.Contains(arg))
        Dim nvUnknown = nvHelpSwitches.Where(Function(x) Not nvPresent.Contains(x) AndAlso Not nvExcept.Contains(x)).ToList()
        nvUnknown.Sort()
        Dim nvNoNeedToExcept = nvExcept.Where(Function(arg) nvPresent.Contains(arg))
        If nvNoNeedToExcept.Count > 0 Then msg += BR2 + "# Unnecessary NVEncC Exception:" + BR2 + nvNoNeedToExcept.Join(" ")
        If nvMissing.Count > 0 Then msg += BR2 + "# Removed from NVEncC" + BR2 + nvMissing.Join(" ")
        If nvUnknown.Count > 0 Then msg += BR2 + "# NVEncC Todo" + BR2 + nvUnknown.Join(" ")

        Dim amdExcept = "--audio-bitrate --audio-codec --audio-copy --audio-file
        --audio-filter --avsw --device --input-analyze
        --audio-ignore-decode-error --audio-ignore-notrack-error --audio-resampler
        --audio-samplerate --audio-source --audio-stream --avs --avvce-analyze
        --check-avversion --check-codecs --check-decoders --check-encoders --check-filters
        --check-formats --check-protocols --dar --format --fps --help --input-file
        --input-res --log-framelist --mux-option --output-file --raw --seek --skip-frame
        --sub-copy --version --video-streamid --video-track --vpy --vpy-mt".Split((" " + BR).ToCharArray())
        Dim amdHelp = File.ReadAllText(".\Apps\VCEEnc\help.txt").Replace("(no-)", "").Replace("--no-", "--")
        Dim amdHelpSwitches = Regex.Matches(amdHelp, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
        Dim amdCode = File.ReadAllText(Folder.Startup.Parent + "Encoding\vceenc.vb").Replace("--no-", "--")
        Dim amdPresent = Regex.Matches(amdCode, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
        Dim amdMissing = amdPresent.Where(Function(arg) Not amdHelpSwitches.Contains(arg))
        Dim amdUnknown = amdHelpSwitches.Where(Function(x) Not amdPresent.Contains(x) AndAlso Not amdExcept.Contains(x)).ToList()
        amdUnknown.Sort()
        Dim amdNoNeedToExcept = amdExcept.Where(Function(arg) amdPresent.Contains(arg))
        If amdNoNeedToExcept.Count > 0 Then msg += BR2 + "# Unnecessary VCEEncC Exception" + BR2 + amdNoNeedToExcept.Join(" ")
        If amdMissing.Count > 0 Then msg += BR2 + "# Removed from VCEEncC" + BR2 + amdMissing.Join(" ")
        If amdUnknown.Count > 0 Then msg += BR2 + "# VCEEncC Todo" + BR2 + amdUnknown.Join(" ")

        Dim qsExcept = "--help --version --check-device --video-streamid --video-track
        --check-avversion --check-codecs --check-encoders --check-decoders --check-formats
        --check-protocols --chapter-no-trim --check-filters --device --input --output
        --raw --avs --vpy --vpy-mt --audio-source --audio-file --seek --format
        --audio-copy --audio-copy --audio-codec --audio-bitrate --audio-ignore
        --audio-ignore --audio-samplerate --audio-resampler --audio-stream --audio-stream
        --audio-stream --audio-stream --audio-filter --chapter-copy --chapter --sub-copy
        --avsync --mux-option --input-res --fps --dar --avqsv-analyze --benchmark
        --bench-quality --log --log-framelist --audio-thread --avi --avqsv --input-file
        --audio-ignore-decode-error --audio-ignore-notrack-error --nv12 --output-file
        --check-features-html --perf-monitor --perf-monitor-plot --perf-monitor-interval
        --python --qvbr-quality --sharpness --vpp-delogo --vpp-delogo-select
        --vpp-delogo-pos --vpp-delogo-depth --vpp-delogo-y --vpp-delogo-cb --vpp-delogo-cr
        --vpp-delogo-add --vpp-half-turn --input-analyze --input-format --output-format
        ".Split((" " + BR).ToCharArray())

        File.WriteAllText(Package.QSVEnc.GetDir + "help.txt", ProcessHelp.GetStdOut(Package.QSVEnc.Path, "-h"))
        Dim qsHelp = File.ReadAllText(Package.QSVEnc.GetDir + "help.txt").Replace("(no-)", "").Replace("--no-", "--")
        Dim qsHelpSwitches = Regex.Matches(qsHelp, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
        Dim qsCode = File.ReadAllText(Folder.Startup.Parent + "Encoding\qsvenc.vb").Replace("--no-", "--")
        Dim qsPresent = Regex.Matches(qsCode, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
        Dim qsMissing = qsPresent.Where(Function(arg) Not qsHelpSwitches.Contains(arg))
        Dim qsUnknown = qsHelpSwitches.Where(Function(x) Not qsPresent.Contains(x) AndAlso Not qsExcept.Contains(x)).ToList()
        qsUnknown.Sort()
        Dim qsNoNeedToExcept = qsExcept.Where(Function(arg) qsPresent.Contains(arg))
        If qsNoNeedToExcept.Count > 0 Then msg += BR2 + "# Unnecessary QSVEncC Exception:" + BR2 + qsNoNeedToExcept.Join(" ")
        If qsMissing.Count > 0 Then msg += BR2 + "# Removed from QSVEncC" + BR2 + qsMissing.Join(" ")
        If qsUnknown.Count > 0 Then msg += BR2 + "# QSVEncC Todo" + BR2 + qsUnknown.Join(" ")

        Dim x265Except = "--crop-rect --display-window --fast-cbf --frame-skip --help
        --input --input-res --lft --ratetol --recon-y4m-exec --total-frames --version
        --opt-qp-pps --opt-ref-list-length-pps --dhdr10-info --no-scenecut
        --no-progress --pbration".Split((" " + BR).ToCharArray())
        Dim x265RemoveExcept = "--numa-pools --rdoq --cip --qblur --cplxblur --cu-stats
--dhdr10-opt --crop --pb-factor --ip-factor --level --log".Split((" " + BR).ToCharArray())

        Dim x265Help = ProcessHelp.GetStdOut(Package.x265.Path, "--log-level full --help").Replace("--[no-]", "--")
        File.WriteAllText(Folder.Desktop + "x265.txt", x265Help)
        Dim x265HelpSwitches = Regex.Matches(x265Help, "--[\w-]+").OfType(Of Match).Select(Function(val) val.Value)
        Dim x265Code = File.ReadAllText(Folder.Startup.Parent + "Encoding\x265Enc.vb").Replace("--no-", "--")
        Dim x265Present As New HashSet(Of String)

        For Each switch In Regex.Matches(x265Code, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
            x265Present.Add(switch)
        Next

        Dim x265Missing = x265Present.Where(Function(arg) Not x265HelpSwitches.Contains(arg) AndAlso Not x265RemoveExcept.Contains(arg))
        Dim x265Unknown = x265HelpSwitches.Where(Function(x) Not x265Present.Contains(x) AndAlso Not x265Except.Contains(x)).ToList()
        x265Unknown.Sort()
        Dim x265NoNeedToExcept = x265Except.Where(Function(arg) x265Present.Contains(arg))
        If x265NoNeedToExcept.Count > 0 Then MsgInfo("Unnecessary x265 Exception:", x265NoNeedToExcept.Join(" "))
        If x265Missing.Count > 0 Then msg += BR2 + "# Removed from x265" + BR2 + x265Missing.Join(" ")
        If x265Unknown.Count > 0 Then msg += BR2 + "# x265 Todo" + BR2 + x265Unknown.Join(" ")

        For Each pack In Package.Items.Values
            If pack.HelpFile.Ext = "md" Then
                msg += BR2 + "# local MD file for " + pack.Name
            End If

            If pack.Path = "" Then msg += BR2 + "# path missing for " + pack.Name

            If pack.Version = "" Then
                msg += BR2 + "# version missing for " + pack.Name
            Else
                If Not pack.IsCorrectVersion Then msg += BR2 + "# wrong version for " + pack.Name

                If Not pack.Version.ContainsAny({"x86", "x64"}) AndAlso
                    Not pack.Filename.ContainsAny({".jar", ".py", ".avsi"}) Then

                    msg += BR2 + "# x86/x64 missing for " + pack.Name
                End If
            End If

            'does help file exist?
            If pack.Path <> "" AndAlso pack.HelpFile <> "" Then
                If Not File.Exists(pack.GetDir + pack.HelpFile) Then
                    msg += BR2 + $"# Help file of {pack.Name} don't exist!"
                End If
            End If

            'does setup file exist?
            If pack.SetupFilename <> "" AndAlso Not File.Exists(Folder.Apps + pack.SetupFilename) Then
                msg += BR2 + $"Setup file of {pack.Name} don't exist!"
            End If
        Next

        If msg <> "" Then
            Dim fs = Folder.Desktop + "staxrip todo.txt"
            File.WriteAllText(fs, msg)
            g.ShellExecute(fs)
        End If
    End Sub

    Sub ShowPackageError(pack As Package, msg As String)
        MsgError(msg)

        Using form As New AppsForm
            form.ShowPackage(pack)
            form.ShowDialog()
        End Using
    End Sub

    <Command("Plays a mp3, wav or wmv sound file.")>
    Sub PlaySound(<Editor(GetType(OpenFileDialogEditor), GetType(UITypeEditor)),
        Description("Filepath to a mp3, wav or wmv sound file.")> Filepath As String,
        <DispName("Volume (%)"), DefaultValue(20)> Volume As Integer)

        Misc.PlayAudioFile(Filepath, Volume)
    End Sub

    Function GetReleaseType() As String
        Dim version = Assembly.GetExecutingAssembly.GetName.Version
        If version.MinorRevision <> 0 Then Return "unstable test build"
        Return "stable release"
    End Function

    <Command("Opens a given help topic in the help browser.")>
    Sub OpenHelpTopic(
        <DispName("Help Topic"),
        Description("Name of the help topic to be opened.")> topic As String)

        Dim f As New HelpForm()

        Select Case topic
            Case "macros"
                f.Doc.WriteStart("Macros")
                f.Doc.WriteTable("Macros", Strings.MacrosHelp, Macro.GetTips())
            Case "info"
                f.Doc.WriteStart("StaxRip x64 " + Application.ProductVersion + " " + GetReleaseType())
                Dim licensePath = Folder.Startup + "License.txt"
                If File.Exists(licensePath) Then f.Doc.WriteP(File.ReadAllText(licensePath), True)
            Case "CRF Value"
                f.Doc.WriteStart("CRF Value")
                f.Doc.WriteP("Low values produce high quality, large file size, large value produces small file size and poor quality. A balanced value is 23 which is the defalt in x264. Common values are 18-26 where 18 produces near transparent quality at the cost of a huge file size. The quality 26 produces is rather poor so such a high value should only be used when a small file size is the only criterium.")
            Case "x264 Mode"
                f.Doc.WriteStart("x264 Mode")
                f.Doc.WriteP("Generally there are two popular encoding modes, quality based and 2pass. 2pass mode allows to specify a bit rate and file size, quality mode doesn't, it works with a rate factor and requires only a single pass. Other terms for quality mode are constant quality or CRF mode in x264.")
                f.Doc.WriteP("Slow and dark sources compress better then colorful sources with a lot action so a short, slow and dark movie requires a smaller file size then a long, colorful source with a lot action and movement.")
                f.Doc.WriteP("Quality mode works with a rate factor that gives comparable quality regardless of how well a movie compresses so it's not using a constant bit rate but adjusts the bit rate dynamically. So while the same rate factor can be applied to every movie to achieve a constant quality this is not possible with 2pass mode because every movie requires a different bit rate. Quality mode is much easier to use then 2pass mode which requires a longer encoding time due to 2 passes and a compressibility check to be performed to determine a reasonable image and file size which also requires more expertise.")
                f.Doc.WriteP("It's a common misconception that 2pass mode is more efficient than quality mode. The only benefit of 2pass mode is hitting a exact file size. Encoding in quality mode using a single pass will result in equal quality compared to a 2pass encode assuming the file size is identical of course.")
                f.Doc.WriteP("Quality mode is ideal for hard drive storage and 2pass mode is ideal for size restricted mediums like CD's and DVD's. If you are still not sure which mode to use then it's probably better to use quality mode.")
            Case Else
                f.Doc.WriteStart("unknown topic")
                f.Doc.WriteP("The requested help topic '''" + topic + "''' is unknown.")
        End Select

        f.Show()
    End Sub

    <Command("Shows a message box.")>
    Sub ShowMessageBox(
        <DispName("Main Instruction")>
        <Description("Main instruction may contain macros.")>
        <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        mainInstruction As String,
        <DispName("Content")>
        <Description("Content may contain macros.")>
        <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        Optional content As String = Nothing,
        <DispName("Icon")>
        <DefaultValue(GetType(MsgIcon), "Info")>
        Optional icon As MsgIcon = MsgIcon.Info)

        Msg(Macro.Expand(mainInstruction), Macro.Expand(content), icon, TaskDialogButtons.Ok)
    End Sub

    <Command("Shows media info on a given file.")>
    Sub ShowMediaInfo(
        <DispName("Filepath")>
        <Description("The filepath may contain macros.")>
        <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        filepath As String)

        filepath = Macro.Expand(filepath)

        If File.Exists(filepath) Then
            g.ShellExecute(Application.ExecutablePath, "-mediainfo " + filepath.Escape)
        Else
            MsgWarn("No file found.")
        End If
    End Sub

    <Command("Copies a string to the clipboard.")>
    Sub CopyToClipboard(
        <DispName("Value")>
        <Description("Copies the text to the clipboard. The text may contain macros.")>
        <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        value As String)

        Macro.Expand(value).ToClipboard()
    End Sub

    <Command("Writes a log message to the process window.")>
    Sub WriteLog(
        <DispName("Header"), Description("Header is optional.")>
        header As String,
        <DispName("Message"), Description("Message is optional and may contain macros."),
        Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        message As String)

        Log.WriteHeader(header)
        Log.WriteLine(Macro.Expand(message))
    End Sub

    <Command("Deletes files in a given directory.")>
    Sub DeleteFiles(
        <DispName("Directory"),
        Description("Directory in which to delete files."),
        Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        dir As String,
        <DispName("Filter"),
        Description("Example: '.txt .log'")>
        filter As String)

        For Each i In ",;*"
            If filter.Contains(i) Then
                filter = filter.Replace(i, " ")
            End If
        Next

        Try
            For Each i In Directory.GetFiles(Macro.Expand(dir))
                For Each i2 In filter.SplitNoEmpty(" ")
                    If i.ToUpper.EndsWith(i2.ToUpper) Then
                        FileHelp.Delete(i)
                    End If
                Next
            Next
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    <Command("Changes the video encoders settings.")>
    Sub ImportVideoEncoderCommandLine(
        <DispName("Command Line")>
        commandLine As String)

        p.VideoEncoder.ImportCommandLine(commandLine)
    End Sub

    <Command("Adds a filter at the end of the script.")>
    Sub AddFilter(<DefaultValue(True)> active As Boolean,
                  name As String,
                  category As String,
                  <Editor(GetType(MacroStringTypeEditor),
                  GetType(UITypeEditor))> script As String)

        p.Script.AddFilter(New VideoFilter(category, name, script, active))
        g.MainForm.Assistant()
    End Sub

    <Command("Sets a filter replacing a existing filter of same category.")>
    Sub SetFilter(name As String,
                  category As String,
                  <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))> script As String)

        p.Script.SetFilter(category, name, script)
    End Sub

    <Command("Sets the file path of the target file.")>
    Sub SetTargetFile(<DispName("Target File Path")>
                      <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
                      path As String)

        p.TargetFile = Macro.Expand(path)
    End Sub

    <Command("Loads the source file.")>
    Sub LoadSourceFile(<DispName("Source File Path")> path As String)
        g.MainForm.OpenVideoSourceFile(path)
    End Sub
End Class