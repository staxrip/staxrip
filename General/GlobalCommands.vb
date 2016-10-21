Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports StaxRip.UI

Public Class GlobalCommands
    <Command("Loads the x265 encoder with default values.")>
    Sub Loadx265Defaults()
        Dim ret = New x265Encoder
        ret.Params.ApplyPresetDefaultValues()
        ret.Params.ApplyPresetValues()
        g.LoadVideoEncoder(ret)
    End Sub

    <Command("Loads the x265 encoder if it's not already loaded.")>
    Sub Ensurex265()
        If Not TypeOf p.VideoEncoder Is x265Encoder Then Loadx265Defaults()
    End Sub

    <Command("Loads VapourSynth with default filters.")>
    Sub LoadVapourSynthDefaults()
        g.MainForm.LoadScriptProfile(VideoScript.GetDefaults()(1))
    End Sub

    <Command("Loads VapourSynth if it's not already loaded.")>
    Sub EnsureVapourSynth()
        If Not p.Script.Engine = ScriptEngine.VapourSynth Then
            LoadVapourSynthDefaults()
        End If
    End Sub

    <Command("Executes command lines separated by a line break line by line. Macros are solved as well as passed in as environment variables.")>
    Sub ExecuteCommandLine(
        <DispName("Command Line"),
        Description("One or more command lines to be executed or if batch mode is used content of the batch file. Macros are solved as well as passed in as environment variables."),
        Editor(GetType(CmdlTypeEditor), GetType(UITypeEditor))>
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
            Dim batchCode = Macro.Solve(commandLines)
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
                    proc.Process.StartInfo.EnvironmentVariables(i.Name.Trim("%"c)) = Macro.Solve(i.Name)
                Next

                Try
                    proc.Start()
                Catch ex As Exception
                    g.ShowException(ex)
                    Log.WriteLine(ex.Message)
                End Try
            End Using
        Else
            For Each i In Macro.Solve(commandLines).SplitLinesNoEmpty
                Using proc As New Proc
                    If showProcessWindow Then proc.Init("Execute Command Line")
                    proc.CommandLine = i
                    proc.Wait = waitForExit

                    If i.Ext = "exe" Then
                        proc.Process.StartInfo.UseShellExecute = False

                        For Each i2 In Macro.GetMacros
                            proc.Process.StartInfo.EnvironmentVariables(i2.Name.Trim("%"c)) = Macro.Solve(i2.Name)
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
        Editor(GetType(CmdlTypeEditor), GetType(UITypeEditor))>
        batchScript As String,
        <DispName("Interpret Output"),
        Description("Interprets each output line as StaxRip command."),
        DefaultValue(False)>
        Optional interpretOutput As Boolean = False)

        Dim closeNeeded As Boolean

        If Not ProcessForm.IsActive Then closeNeeded = True

        Dim batchPath = Folder.Temp + Guid.NewGuid.ToString + ".bat"
        Dim batchCode = Macro.Solve(batchScript)
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
                proc.Process.StartInfo.EnvironmentVariables(i.Name.Trim("%"c)) = Macro.Solve(i.Name)
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

    <Command("Executes a csx (C#) or ps1 (PowerShell) script.")>
    Sub ExecuteScriptFile(<DispName("File Path")>
                          <Description("Filepath to a csx (C#) or ps1 (PowerShell) file, the path may contain macros.")>
                          <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
                          filepath As String)

        If File.Exists(filepath) Then
            Select Case filepath.Ext
                Case "csx"
                    Scripting.RunCSharp(File.ReadAllText(filepath))
                Case "ps1"
                    ExecutePowerShellScript(File.ReadAllText(filepath))
                Case Else
                    MsgError("Only csx (C#) and ps1 (PowerShell) are supported at this time.")
            End Select
        Else
            MsgError("File is missing:" + BR2 + filepath)
        End If
    End Sub

    <Command("Executes C# script code.")>
    Sub ExecuteCSharpScript(<DispName("Script Code")>
                            <Description("C# script code to be executed.")>
                            <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
                            scriptCode As String)

        Scripting.RunCSharp(scriptCode)
    End Sub

    <Command("Starts a tool by name as shown in the app manage dialog.")>
    Sub StartTool(<DispName("Tool Name")>
                  <Description("Tool name as shown in the app manage dialog.")>
                  name As String)

        Try
            Package.Items(name).StartAction.Invoke
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    <Command("Executes PowerShell script code.")>
    Sub ExecutePowerShellScript(<DispName("Script Code")>
                                <Description("PowerShell script code to be executed.")>
                                <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
                                scriptCode As String)

        Try
            Scripting.RunPowershell(scriptCode)
        Catch ex As Exception
            MsgError("Failed to execute PowerShell script, on systems prior Windows 10 PowerShell 5 or higher must be installed.", ex.ToString)
        End Try
    End Sub

    <Command("Test")>
    Sub Test()
        Dim nvExcept = "--help --version --check-device
        --check-avversion --check-codecs --check-encoders --check-decoders --check-formats --check-protocols
        --check-filters --device --input --output --raw --avs --vpy --vpy-mt --avcuvid-analyze
        --audio-source --audio-file --seek --format --audio-copy --audio-copy --audio-codec
        --audio-bitrate --audio-ignore --audio-ignore --audio-samplerate --audio-resampler --audio-stream
        --audio-stream --audio-stream --audio-stream --audio-filter --chapter-copy --chapter --sub-copy
        --avsync --mux-option --input-res --fps --dar --audio-ignore-decode-error --audio-ignore-notrack-error
        --log --log-framelist".Split((" " + BR).ToCharArray())
        Dim nvHelp = File.ReadAllText(".\Apps\NVEncC\help.txt").Replace("(no-)", "").Replace("--no-", "--")
        Dim nvHelpSwitches = Regex.Matches(nvHelp, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
        Dim nvCode = File.ReadAllText(Folder.Startup.Parent + "Encoding\NVIDIAEncoder.vb").Replace("--no-", "--")
        Dim nvPresent = Regex.Matches(nvCode, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
        Dim nvMissing = nvPresent.Where(Function(arg) Not nvHelpSwitches.Contains(arg))
        Dim nvUnknown = nvHelpSwitches.Where(Function(x) Not nvPresent.Contains(x) AndAlso Not nvExcept.Contains(x)).ToList()
        nvUnknown.Sort()
        Dim nvNoNeedToExcept = nvExcept.Where(Function(arg) nvPresent.Contains(arg))
        If nvNoNeedToExcept.Count > 0 Then MsgInfo("Unnecessary NVEncC Exception:", nvNoNeedToExcept.Join(" "))
        If nvMissing.Count > 0 Then MsgInfo("Removed from NVEncC:", nvMissing.Join(" "))
        If nvUnknown.Count > 0 Then MsgInfo("NVEncC Todo", nvUnknown.Join(" "))

        Dim amdExcept = "--audio-bitrate --audio-codec --audio-copy --audio-file --audio-filter
        --audio-ignore-decode-error --audio-ignore-notrack-error --audio-resampler
        --audio-samplerate --audio-source --audio-stream --avs --avvce --avvce-analyze
        --check-avversion --check-codecs --check-decoders --check-encoders --check-filters
        --check-formats --check-protocols --dar --format --fps --help --input-file
        --input-res --log-framelist --mux-option --output-file --raw --seek --skip-frame
        --sub-copy --version --video-streamid --video-track --vpy --vpy-mt".Split((" " + BR).ToCharArray())
        Dim amdHelp = File.ReadAllText(".\Apps\VCEEncC\help.txt").Replace("(no-)", "").Replace("--no-", "--")
        Dim amdHelpSwitches = Regex.Matches(amdHelp, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
        Dim amdCode = File.ReadAllText(Folder.Startup.Parent + "Encoding\AMDEncoder.vb").Replace("--no-", "--")
        Dim amdPresent = Regex.Matches(amdCode, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
        Dim amdMissing = amdPresent.Where(Function(arg) Not amdHelpSwitches.Contains(arg))
        Dim amdUnknown = amdHelpSwitches.Where(Function(x) Not amdPresent.Contains(x) AndAlso Not amdExcept.Contains(x)).ToList()
        amdUnknown.Sort()
        Dim amdNoNeedToExcept = amdExcept.Where(Function(arg) amdPresent.Contains(arg))
        If amdNoNeedToExcept.Count > 0 Then MsgInfo("Unnecessary VCEEncC Exception:", amdNoNeedToExcept.Join(" "))
        If amdMissing.Count > 0 Then MsgInfo("Removed from VCEEncC:", amdMissing.Join(" "))

        If amdUnknown.Count > 0 Then
            MsgInfo("VCEEncC Todo", amdUnknown.Join(" "))
            If MsgQuestion("Copy " + amdUnknown(0) + " ?") = DialogResult.OK Then amdUnknown(0).ToClipboard
        End If

        Dim qsExcept = "--help --version --check-device --video-streamid --video-track
        --check-avversion --check-codecs --check-encoders --check-decoders --check-formats --check-protocols
        --check-filters --device --input --output --raw --avs --vpy --vpy-mt
        --audio-source --audio-file --seek --format --audio-copy --audio-copy --audio-codec
        --audio-bitrate --audio-ignore --audio-ignore --audio-samplerate --audio-resampler --audio-stream
        --audio-stream --audio-stream --audio-stream --audio-filter --chapter-copy --chapter --sub-copy
        --avsync --mux-option --input-res --fps --dar --avqsv-analyze --benchmark --bench-quality
        --log --log-framelist --audio-thread --avi --avqsv --input-file --audio-ignore-decode-error
        --audio-ignore-notrack-error --nv12 --output-file --check-features-html --perf-monitor
        --perf-monitor-plot --perf-monitor-interval --python --qvbr-quality --sharpness --vpp-delogo
        --vpp-delogo-select --vpp-delogo-pos --vpp-delogo-depth --vpp-delogo-y --vpp-delogo-cb
        --vpp-delogo-cr --vpp-half-turn --input-analyze --input-format --output-format
        ".Split((" " + BR).ToCharArray())
        Dim qsHelp = File.ReadAllText(".\Apps\QSVEncC\help.txt").Replace("(no-)", "").Replace("--no-", "--")
        Dim qsHelpSwitches = Regex.Matches(qsHelp, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
        Dim qsCode = File.ReadAllText(Folder.Startup.Parent + "Encoding\IntelEncoder.vb").Replace("--no-", "--")
        Dim qsPresent = Regex.Matches(qsCode, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
        Dim qsMissing = qsPresent.Where(Function(arg) Not qsHelpSwitches.Contains(arg))
        Dim qsUnknown = qsHelpSwitches.Where(Function(x) Not qsPresent.Contains(x) AndAlso Not qsExcept.Contains(x)).ToList()
        qsUnknown.Sort()
        Dim qsNoNeedToExcept = qsExcept.Where(Function(arg) qsPresent.Contains(arg))
        If qsNoNeedToExcept.Count > 0 Then MsgInfo("Unnecessary QSVEncC Exception:", qsNoNeedToExcept.Join(" "))
        If qsMissing.Count > 0 Then MsgInfo("Removed from QSVEncC:", qsMissing.Join(" "))
        If qsUnknown.Count > 0 Then
            MsgInfo("QSVEncC Todo", qsUnknown.Join(" "))
            If MsgQuestion("Copy " + qsUnknown(0) + " ?") = DialogResult.OK Then qsUnknown(0).ToClipboard
        End If

        Dim x265Except = "--crop-rect --display-window --fast-cbf --frame-skip --help
--input --input-res --lft --ratetol --recon-y4m-exec --total-frames --version
--opt-qp-pps --opt-ref-list-length-pps".Split((" " + BR).ToCharArray())
        Dim x265RemoveExcept = "--crop --pb-factor --ip-factor --level --log".Split((" " + BR).ToCharArray())

        Dim x265HelpSwitches = Regex.Matches(
            File.ReadAllText(Folder.Startup.Parent + "x265\param.cpp"),
            "OPT2?\(""(.+?)""").
            OfType(Of Match)().
            Select(Function(x) "--" + x.Groups(1).Value).
            Union(Regex.Matches(
            File.ReadAllText(Folder.Startup.Parent + "x265\x265cli.h"),
            "{ *""(.+?)"" *, *\w+argument *,").
            OfType(Of Match)().
            Select(Function(x) "--" + x.Groups(1).Value)).
            Where(Function(arg) Not arg.StartsWith("--no-"))

        Dim x265Code = File.ReadAllText(Folder.Startup.Parent + "Encoding\x265.vb").Replace("--no-", "--")
        Dim x265Present As New HashSet(Of String)

        For Each switch In Regex.Matches(x265Code, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
            x265Present.Add(switch)
        Next

        Dim x265Missing = x265Present.Where(Function(arg) Not x265HelpSwitches.Contains(arg) AndAlso Not x265RemoveExcept.Contains(arg))
        Dim x265Unknown = x265HelpSwitches.Where(Function(x) Not x265Present.Contains(x) AndAlso Not x265Except.Contains(x)).ToList()
        x265Unknown.Sort()
        Dim x265NoNeedToExcept = x265Except.Where(Function(arg) x265Present.Contains(arg))
        If x265NoNeedToExcept.Count > 0 Then MsgInfo("Unnecessary x265 Exception:", x265NoNeedToExcept.Join(" "))
        If x265Missing.Count > 0 Then MsgInfo("Removed from x265:", x265Missing.Join(" "))
        If x265Unknown.Count > 0 Then MsgInfo("x265 Todo", x265Unknown.Join(" "))

        'does the version contain x64 or x86?
        For Each pack In Package.Items.Values
            If pack.Path = "" Then Continue For

            If pack.Version = "" OrElse (Not pack.Version.ContainsAny({"x86", "x64"}) AndAlso
                Not pack.Filename.ContainsAny({".jar", ".py", ".avsi"})) OrElse
                Not pack.IsCorrectVersion Then

                Using f As New AppsForm
                    f.ShowPackage(pack)
                    f.ShowDialog()
                End Using
            End If
        Next

        For Each pack In Package.Items.Values
            'does help file exist?
            If pack.Path <> "" AndAlso pack.HelpFile <> "" Then
                If Not File.Exists(pack.GetDir + pack.HelpFile) Then
                    ShowPackageError(pack, $"Help file of {pack.Name} don't exist!")
                    Exit For
                End If
            End If

            'does setup file exist?
            If pack.SetupFilename <> "" AndAlso Not File.Exists(Folder.Apps + pack.SetupFilename) Then
                ShowPackageError(pack, $"Setup file of {pack.Name} don't exist!")
                Exit For
            End If
        Next

        MsgInfo("end")
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
        If version.MinorRevision <> 0 Then Return "Unstable Test Build"
        Return "Stable Release"
    End Function

    <Command("Opens a given help topic in the help browser.")>
    Sub OpenHelpTopic(
        <DispName("Help Topic"),
        Description("Name of the help topic to be opened.")> topic As String)

        Dim f As New HelpForm()

        Select Case topic
            Case "info"
                f.Doc.WriteStart("StaxRip x64 " + Application.ProductVersion + " " + GetReleaseType())
                f.Doc.WriteP($"Copyright © {Date.Now.Year} Frank Skare frank.skare.de@gmail.com
This work is free. You can redistribute it and/or modify it under the
terms of the Do What The Fuck You Want To Public License, Version 2,
as published by Sam Hocevar. See the COPYING file for more details.", True)
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
    Sub ShowMsg(
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

        Msg(Macro.Solve(mainInstruction), Macro.Solve(content), icon, MessageBoxButtons.OK)
    End Sub

    <Command("Shows media info on a given file.")>
    Sub ShowMediaInfo(
        <DispName("Filepath")>
        <Description("The filepath may contain macros.")>
        <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        filepath As String)

        filepath = Macro.Solve(filepath)

        If File.Exists(filepath) Then
            Using f As New MediaInfoForm(filepath)
                f.ShowDialog()
            End Using
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

        Macro.Solve(value).ToClipboard()
    End Sub

    <Command("Writes a log message to the process window.")>
    Sub WriteLog(
        <DispName("Header"), Description("Header is optional.")>
        header As String,
        <DispName("Message"), Description("Message is optional and may contain macros."),
        Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        message As String)

        Log.WriteHeader(header)
        Log.WriteLine(Macro.Solve(message))
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
            For Each i In Directory.GetFiles(Macro.Solve(dir))
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

    <Command("Adds x264 custom command line switches.")>
    Sub AddX264Zone(<Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))> start As String,
                    <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))> [end] As String,
                    <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))> [option] As String)

        If TypeOf p.VideoEncoder Is x264Encoder Then
            start = Macro.Solve(start)
            [end] = Macro.Solve([end])

            [option] = InputBox.Show("Please enter the option arguments.", "Zone option arguments", [option])

            If [option] = "" Then Exit Sub

            [option] = Macro.Solve([option])

            Dim value = DirectCast(p.VideoEncoder, x264Encoder).Params.AddAll.Value

            If value.Contains("--zones ") Then
                value = value.Replace("--zones ", "--zones " + start + "," + [end] + "," + [option] + "/")
            Else
                value += " --zones " + start + "," + [end] + "," + [option]
            End If

            value = value.Trim

            Using f As New StringEditorForm
                f.Text = "x264 custom command line switches"
                f.cbWrap.Checked = Not value.Contains(BR)
                f.tb.Text = value
                f.tb.Text = value
                f.Width = 800
                f.Height = 200

                If f.ShowDialog() = DialogResult.OK Then
                    DirectCast(p.VideoEncoder, x264Encoder).Params.AddAll.Value = f.tb.Text
                End If
            End Using
        Else
            MsgWarn("This feature is only available for x264.")
        End If
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

    <Command("Command used internally.")>
    Sub SetPreventSaveSettings(preventSaveSettings As Boolean)
        g.PreventSaveSettings = preventSaveSettings
    End Sub

    <Command("Sets the file path of the target file.")>
    Sub SetTargetFile(<DispName("Target File Path")>
                      <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
                      path As String)

        p.TargetFile = Macro.Solve(path)
    End Sub

    <Command("Loads the source file.")>
    Sub LoadSourceFile(<DispName("Source File Path")> path As String)
        g.MainForm.OpenVideoSourceFile(path)
    End Sub
End Class