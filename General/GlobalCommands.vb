Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Win32
Imports StaxRip.UI

Public Class GlobalCommands
    <Command("Shows the log file with the built in log file viewer.")>
    Sub ShowLogFile()
        If Not File.Exists(p.Log.GetPath()) Then Exit Sub

        Using form As New LogForm
            form.Path = p.Log.GetPath()
            form.Init()
            form.ShowDialog()
        End Using
    End Sub

    <Command("Allows to use StaxRip's demuxing GUIs independently.")>
    Sub ShowDemuxTool()
        Using form As New SimpleSettingsForm("Demux")
            form.ScaleClientSize(27, 10)
            Dim ui = form.SimpleUI
            Dim page = ui.CreateFlowPage("main page")
            page.SuspendLayout()

            Dim sourceFile = ui.AddTextButton(page)
            sourceFile.Label.Text = "Source File:"
            sourceFile.Edit.Expand = True
            sourceFile.BrowseFile(FileTypes.VideoAudio)

            Dim outputFolder = ui.AddTextButton(page)
            outputFolder.Label.Text = "Output Folder:"
            outputFolder.Edit.Expand = True
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
                    proj.TargetFile = sourceFile.Edit.Text
                    proj.TempDir = outputFolder.Edit.Text.FixDir

                    If Not td.Show Is Nothing Then
                        td.SelectedValue.Run(proj)
                        s.LastSourceDir = proj.SourceFile.Dir
                        proj.Log.Save(proj)
                    End If
                End Using
            End If
        End Using
    End Sub

    <Command("Runs all active jobs of the job list.")>
    Sub StartJobs()
        g.ProcessJobs()
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

    <Command("Shows the powershell with aliases for all tools staxrip includes.")>
    Sub ShowPowerShell()
        Dim val As String

        For Each pack In Package.Items.Values
            If pack.Path <> "" AndAlso pack.Filename.Ext = "exe" AndAlso Not pack.Name.Contains(" ") Then
                Dim name = pack.Name.Replace(" ", "")
                Dim filename = pack.Filename.Replace(" ", "")
                val += "set-alias " + name + " \""" + pack.Path + "\"";"
                If name <> filename Then val += "set-alias " + filename + " \""" + pack.Path + "\"";"
            End If
        Next

        If p.TempDir <> "" Then val += "cd \""" + p.TempDir + """"
        g.StartProcess("powershell.exe", "-noexit -command " + val)
    End Sub

    <Command("Executes command lines separated by a line break line by line. Macros are solved and passed as environment variables.")>
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

        If asBatch Then
            Dim batchPath = Folder.Temp + Guid.NewGuid.ToString + ".bat"
            Dim batchCode = Macro.Expand(commandLines)
            File.WriteAllText(batchPath, batchCode, Encoding.Default)
            AddHandler g.MainForm.Disposed, Sub() FileHelp.Delete(batchPath)

            Using proc As New Proc(showProcessWindow)
                proc.Header = "Execute Command Line"
                proc.WriteLog(batchCode + BR2)
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
                Using proc As New Proc(showProcessWindow)
                    proc.Header = "Execute Command Line"
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

        Dim batchPath = Folder.Temp + Guid.NewGuid.ToString + ".bat"
        Dim batchCode = Macro.Expand(batchScript)
        File.WriteAllText(batchPath, batchCode, Encoding.Default)
        AddHandler g.MainForm.Disposed, Sub() FileHelp.Delete(batchPath)

        Using proc As New Proc
            proc.Header = "Execute Batch Script"
            proc.WriteLog(batchCode + BR2)
            proc.File = "cmd.exe"
            proc.Arguments = "/C call """ + batchPath + """"
            proc.Wait = True
            proc.Process.StartInfo.UseShellExecute = False

            For Each i In Macro.GetMacros
                proc.Process.StartInfo.EnvironmentVariables(i.Name.Trim("%"c)) = Macro.Expand(i.Name)
            Next

            Try
                proc.Start()

                If interpretOutput Then
                    For Each i In proc.Log.ToString.SplitLinesNoEmpty
                        If Not g.MainForm.CommandManager.ProcessCommandLineArgument(i) Then Log.WriteLine("Failed to interpret output:" + BR2 + i)
                    Next
                End If
            Catch ex As Exception
                g.ShowException(ex)
                Log.WriteLine(ex.Message)
            End Try
        End Using
    End Sub

    <Command("Executes a PowerShell script.")>
    Sub ExecuteScriptFile(<DispName("File Path")>
                          <Description("Filepath to a PowerShell script, the path may contain macros.")>
                          <Editor(GetType(OpenFileDialogEditor), GetType(UITypeEditor))>
                          filepath As String)

        filepath = Macro.Expand(filepath)

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

        msg += NVEnc.Test
        msg += QSVEnc.Test
        msg += Rav1e.Test
        msg += VCEEnc.Test
        msg += x264Enc.Test
        msg += x265Enc.Test

        '//////////////////// aom

        'Dim aomCodeExcept = "".Split((" " + BR).ToCharArray())

        'Dim aomExcept = "--output --help".Split((" " + BR).ToCharArray())
        'Dim aomCodeExcept = "--y4m --help".Split((" " + BR).ToCharArray())
        'Dim aomHelp = ProcessHelp.GetStdOut(Package.AOMEnc.Path, "--help")
        'File.WriteAllText(Package.AOMEnc.GetDir + "aomenc.txt", aomHelp)
        'aomHelp = aomHelp.Replace("(no-)", "").Replace("--no-", "--")
        'Dim aomHelpSwitches = Regex.Matches(aomHelp, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
        'Dim aomCode = File.ReadAllText(Folder.Startup.Parent + "Encoding\aomenc.vb").Replace("--no-", "--")
        'Dim aomPresentInCode = Regex.Matches(aomCode, "--[\w-]+").OfType(Of Match)().Select(Function(x) x.Value)
        'Dim aomMissing = aomPresentInCode.Where(Function(arg) Not aomHelpSwitches.Contains(arg) AndAlso Not aomCodeExcept.Contains(arg))
        'Dim aomUnknown = aomHelpSwitches.Where(Function(x) Not aomPresentInCode.Contains(x) AndAlso Not aomExcept.Contains(x)).ToList()
        'aomUnknown.Sort()
        'Dim aomNoNeedToExcept = aomExcept.Where(Function(arg) aomPresentInCode.Contains(arg))
        'If aomNoNeedToExcept.Count > 0 Then msg += BR2 + "# Unnecessary aomenc Exception:" + BR2 + aomNoNeedToExcept.Join(" ")
        'If aomMissing.Count > 0 Then msg += BR2 + "# Removed from aomenc" + BR2 + aomMissing.Join(" ")
        'If aomUnknown.Count > 0 Then msg += BR2 + "# aomenc Todo" + BR2 + aomUnknown.Join(" ")

        Package.fdkaac.CreateHelpfile("-h")
        Package.PNGopt.CreateHelpfile("")

        For Each pack In Package.Items.Values
            If pack.HelpFilename.Ext = "md" Then
                msg += BR2 + "# local MD file for " + pack.Name
            End If

            If pack.IsIncluded Then
                If pack.Path = "" Then
                    msg += BR2 + "# path missing for " + pack.Name
                ElseIf Not pack.IgnoreVersion Then
                    If pack.Version = "" Then
                        msg += BR2 + "# version missing for " + pack.Name
                    ElseIf Not pack.IsCorrectVersion Then
                        msg += BR2 + "# wrong version for " + pack.Name
                    End If
                End If

                'does help file exist?
                If pack.Path <> "" AndAlso pack.HelpFilename <> "" Then
                    If Not File.Exists(pack.GetDir + pack.HelpFilename) Then
                        msg += BR2 + $"# Help file of {pack.Name} don't exist!"
                    End If
                End If

                'does setup file exist?
                If pack.SetupFilename <> "" AndAlso Not File.Exists(Folder.Apps + pack.SetupFilename) Then
                    msg += BR2 + $"Setup file of {pack.Name} don't exist!"
                End If
            End If
        Next

        Dim supportedTools = "Supported Tools" + BR + "===============" + BR2 + "Tools" + BR + "-----" + BR2

        For Each i In Package.Items.Values
            If Not TypeOf i Is PluginPackage Then
                supportedTools += i.Name + BR + "~".Multiply(i.Name.Length) + BR2 + i.Description + BR2
                supportedTools += "Used Version: " + i.Version + BR2 + i.WebURL + BR2 + BR
            End If
        Next

        supportedTools += "AviSynth Plugins" + BR + "----------------" + BR

        For Each i In Package.Items.Values.OfType(Of PluginPackage)
            If Not i.AvsFilterNames.NothingOrEmpty Then
                supportedTools += i.Name + BR + "~".Multiply(i.Name.Length) + BR2 + i.Description + BR2
                supportedTools += "Filters: " + i.AvsFilterNames.Join(", ") + BR2
                supportedTools += "Used Version: " + i.Version + BR2 + i.WebURL + BR2 + BR
            End If
        Next

        supportedTools += "VapourSynth Plugins" + BR + "-------------------" + BR

        For Each i In Package.Items.Values.OfType(Of PluginPackage)
            If Not i.VSFilterNames.NothingOrEmpty Then
                supportedTools += i.Name + BR + "~".Multiply(i.Name.Length) + BR2 + i.Description + BR2
                supportedTools += "Filters: " + i.VSFilterNames.Join(", ") + BR2
                supportedTools += "Used Version: " + i.Version + BR2 + i.WebURL + BR2 + BR
            End If
        Next

        supportedTools.WriteUTF8File(Folder.Startup + "..\docs\tools.rst")

        Dim screenshots = "Screenshots" + BR + "===========" + BR2 + ".. contents::" + BR2
        Dim screenshotFiles = Directory.GetFiles(Folder.Startup + "..\docs\screenshots").ToList
        screenshotFiles.Sort(New StringLogicalComparer)

        For Each i In screenshotFiles
            Dim name = i.Base.Replace("_", " ").Trim
            screenshots += name + BR + "-".Multiply(name.Length) + BR2 + ".. image:: screenshots/" + i.FileName + BR2
        Next

        screenshots.WriteUTF8File(Folder.Startup + "..\docs\screenshots.rst")

        Dim macros = "Macros" + BR + "======" + BR2

        For Each i In Macro.GetTips
            macros += "``" + i.Name + "``" + BR2 + i.Value + BR2
        Next

        macros.WriteUTF8File(Folder.Startup + "..\docs\macros.rst")

        Dim powershell = "PowerShell Scripting
====================

StaxRip can be automated via PowerShell scripting.


Events
------

In order to run scripts on certain events the following events are available:

"

        For Each i As ApplicationEvent In System.Enum.GetValues(GetType(ApplicationEvent))
            powershell += "- ``" + i.ToString + "`` " + DispNameAttribute.GetValueForEnum(i) + BR
        Next

        powershell += BR + "Assign to an event by saving a script file in the scripting folder using the event name as file name." + BR2 + "The scripting folder can be opened with:" + BR2 + "Main Menu > Tools > Scripts > Open script folder" + BR2 + "Use one of the following file names:" + BR2

        For Each i In System.Enum.GetNames(GetType(ApplicationEvent))
            powershell += "- " + i.ToString + ".ps1" + BR
        Next

        powershell += BR + "Support
-------

If you have questions feel free to ask here:

https://github.com/stax76/staxrip/issues/200


Default Scripts
---------------

"
        Dim psdir = Folder.Startup + "..\docs\powershell"
        DirectoryHelp.Delete(psdir)
        Directory.CreateDirectory(psdir)

        For Each i In Directory.GetFiles(Folder.Startup + "Apps\Scripts")
            FileHelp.Copy(i, psdir + "\" + i.FileName)
            Dim filename = i.FileName
            powershell += filename + BR + "~".Multiply(filename.Length) + BR2
            powershell += ".. literalinclude:: " + "powershell/" + i.FileName + BR + "   :language: powershell" + BR2
        Next

        powershell.WriteUTF8File(Folder.Startup + "..\docs\powershell.rst")

        Dim switches = "Command Line Interface
======================

Switches are processed in the order they appear in the command line.

The command line interface, the customizable main menu and Event Commands features are built with a shared command system.

There is a special mode where only the MediaInfo window is shown using -mediainfo , this is useful for Windows File Explorer integration with an app like Open++.


Examples
--------

StaxRip C:\\Movie\\project.srip

StaxRip C:\\Movie\\VTS_01_1.VOB C:\\Movie 2\\VTS_01_2.VOB

StaxRip -LoadTemplate:DVB C:\\Movie\\capture.mpg -StartEncoding -Standby

StaxRip -ShowMessageBox:""main text..."",""text ..."",info


Switches
--------

"

        Dim commands As New List(Of Command)(g.MainForm.CommandManager.Commands.Values)
        commands.Sort()

        Dim commandList As New StringPairList

        For Each command In commands
            Dim params = command.MethodInfo.GetParameters
            Dim switch = "-" + command.MethodInfo.Name + ":"

            For Each param In params
                switch += param.Name + ","
            Next

            switch = switch.TrimEnd(",:".ToCharArray)
            switches += switch + BR + "~".Multiply(switch.Length) + BR2

            For Each param In params
                Dim d = param.GetCustomAttribute(Of DescriptionAttribute)

                If Not d Is Nothing Then
                    switches += param.Name + ": " + param.GetCustomAttribute(Of DescriptionAttribute).Description + BR2
                End If
            Next

            Dim enumList As New List(Of String)

            For Each param In params
                If param.ParameterType.IsEnum Then
                    enumList.Add(param.ParameterType.Name + ": " +
                                 System.Enum.GetNames(param.ParameterType).Join(", "))
                End If
            Next

            For Each en In enumList
                switches += en + BR2
            Next

            switches += command.Attribute.Description + BR2 + BR
        Next

        switches.WriteUTF8File(Folder.Startup + "..\docs\cli.rst")

        If msg <> "" Then
            Dim fs = Folder.Temp + "staxrip test.txt"
            File.WriteAllText(fs, BR + msg.Trim + BR)
            g.StartProcess(fs)
        Else
            MsgInfo("No issues found.")
        End If
    End Sub

    <Command("Release")>
    Sub Release()
        Try
            Dim sourceDir = Application.StartupPath + "\"

            If Not sourceDir.EndsWith("\bin\") Then
                MsgError("Source directory don't end with \bin\" + BR2 + sourceDir)
                Exit Sub
            End If

            Dim version = Assembly.LoadFile(sourceDir + "StaxRip.exe").GetName.Version
            Dim releaseType = "-stable"

            If version.Revision <> 0 Then
                releaseType = "-beta"
            End If

            If Not Directory.Exists(sourceDir) Then
                Throw New Exception("Source directory not found." + BR2 + sourceDir)
            End If

            Dim info = FileVersionInfo.GetVersionInfo(sourceDir + "StaxRip.exe")
            Dim targetDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                         "StaxRip-x64-" + info.FileVersion + releaseType)

            DirectoryHelp.Delete(targetDir)
            DirectoryHelp.Copy(sourceDir, targetDir, Microsoft.VisualBasic.FileIO.UIOption.AllDialogs)

            For Each i In Directory.GetDirectories(targetDir + "\Apps\Plugins\VS")
                Dim cacheDir = i + "\__pycache__"

                If Directory.Exists(cacheDir) Then
                    DirectoryHelp.Delete(cacheDir)
                End If
            Next

            DirectoryHelp.Delete(targetDir + "\.vs")
            DirectoryHelp.Delete(targetDir + "\Apps\Plugins\VS\Scripts\__pycache__")
            DirectoryHelp.Delete(targetDir + "\Apps\Audio\qaac\QTfiles64")

            FileHelp.Delete(targetDir + "\_StaxRip.log")
            FileHelp.Delete(targetDir + "\Apps\Audio\eac3to\log.txt")
            FileHelp.Delete(targetDir + "\Apps\Support\AVSMeter\AVSMeter.ini")
            FileHelp.Delete(targetDir + "\Apps\Support\chapterEditor\chapterEditor.ini")
            FileHelp.Delete(targetDir + "\Apps\Support\DGIndex\DGIndex.ini")
            FileHelp.Delete(targetDir + "\Apps\Support\MKVToolNix\mkvtoolnix.ini")
            FileHelp.Delete(targetDir + "\Apps\Support\MKVToolNix\mkvtoolnix-gui.ini")
            FileHelp.Delete(targetDir + "\Debug.log")
            FileHelp.Delete(targetDir + "\FrameServer.exp")
            FileHelp.Delete(targetDir + "\FrameServer.exp")
            FileHelp.Delete(targetDir + "\FrameServer.exp")
            FileHelp.Delete(targetDir + "\FrameServer.ilk")
            FileHelp.Delete(targetDir + "\FrameServer.lib")
            FileHelp.Delete(targetDir + "\FrameServer.pdb")
            FileHelp.Delete(targetDir + "\StaxRip.vshost.exe")
            FileHelp.Delete(targetDir + "\StaxRip.vshost.exe.config")
            FileHelp.Delete(targetDir + "\StaxRip.vshost.exe.manifest")
            FileHelp.Delete(targetDir + "\StaxRip.vshost.sln")

            For Each i In Directory.GetFiles(targetDir, "*.ini", IO.SearchOption.AllDirectories)
                Throw New Exception("ini file found:" + BR2 + i)
            Next

            Using p As New Process
                p.StartInfo.FileName = "C:\Program Files\7-Zip\7z.exe"
                p.StartInfo.Arguments = $"a -t7z -mx9 ""{targetDir}.7z"" -r ""{targetDir}\*"""
                p.Start()
                p.WaitForExit()

                If p.ExitCode > 0 Then
                    Throw New Exception($"7zip exit code: {p.ExitCode}")
                End If
            End Using

            If releaseType = "-beta" Then
                Dim outputDirectories = {
                    "C:\Users\frank\OneDrive\StaxRip\TestBuilds\",
                    "C:\Users\frank\OneDrive\StaxRip\Builds"}

                For Each i In outputDirectories
                    If Not Directory.Exists(i) Then
                        Continue For
                    End If

                    FileHelp.Copy(targetDir.TrimEnd("\"c) + ".7z", i + DirPath.GetName(targetDir) + ".7z", Microsoft.VisualBasic.FileIO.UIOption.AllDialogs)
                    Process.Start(i)
                Next
            End If
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    <Command("Plays a mp3, wav Or wmv sound file.")>
    Sub PlaySound(<Editor(GetType(OpenFileDialogEditor), GetType(UITypeEditor)),
        Description("Filepath To a mp3, wav Or wmv sound file.")> Filepath As String,
        <DispName("Volume (%)"), DefaultValue(20)> Volume As Integer)

        Misc.PlayAudioFile(Filepath, Volume)
    End Sub

    Function GetReleaseType() As String
        Dim version = Assembly.GetExecutingAssembly.GetName.Version
        If version.MinorRevision <> 0 Then Return "Beta"
        Return "Stable"
    End Function

    <Command("Opens a given help topic In the help browser.")>
    Sub OpenHelpTopic(
        <DispName("Help Topic"),
        Description("Name Of the help topic To be opened.")> topic As String)

        Dim f As New HelpForm()

        Select Case topic
            Case "info"
                f.Doc.WriteStart("StaxRip " + Application.ProductVersion + " " + GetReleaseType())
                f.Doc.WriteP("Thanks for icon artwork: Freepik www.flaticon.com, ilko-k, nulledone, vanontom")
                Dim licensePath = Folder.Startup + "License.txt"
                If File.Exists(licensePath) Then f.Doc.WriteP(File.ReadAllText(licensePath), True)
            Case "CRF Value"
                f.Doc.WriteStart("CRF Value")
                f.Doc.WriteP("Low values produce high quality, large file size, large value produces small file size And poor quality. A balanced value Is 23 which Is the defalt In x264. Common values are 18-26 where 18 produces near transparent quality at the cost Of a huge file size. The quality 26 produces Is rather poor so such a high value should only be used When a small file size Is the only criterium.")
            Case "x264 Mode"
                f.Doc.WriteStart("x264 Mode")
                f.Doc.WriteP("Generally there are two popular encoding modes, quality based And 2pass. 2pass mode allows To specify a bit rate And file size, quality mode doesn't, it works with a rate factor and requires only a single pass. Other terms for quality mode are constant quality or CRF mode in x264.")
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
            Dim path = Registry.CurrentUser.GetString("Software\Microsoft\Windows\CurrentVersion\App Paths\MediaInfoNET.exe", Nothing)

            If File.Exists(path) Then
                g.StartProcess(path, filepath.Escape)
            Else
                g.StartProcess(Application.ExecutablePath, "-mediainfo " + filepath.Escape)
            End If
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