
Imports System.Collections.Concurrent
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports DirectN
Imports Microsoft.Win32
Imports StaxRip.UI

Public Class GlobalCommands
    <Command("Checks if an update is available.")>
    Sub CheckForUpdate()
        StaxRipUpdate.CheckForUpdate(True, s.CheckForUpdatesDev, Environment.Is64BitProcess)
    End Sub

    <Command("Shows the log file with the built-in log file viewer.")>
    Sub ShowLogFile()
        If File.Exists(p.Log.GetPath()) Then
            Using form As New LogForm()
                form.ShowDialog()
            End Using
        Else
            g.ShellExecute(Folder.Settings + "Log Files")
        End If
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
            sourceFile.Edit.TextBox.ReadOnly = True
            sourceFile.BrowseFile(FileTypes.VideoAudio)

            Dim outputFolder = ui.AddTextButton(page)
            outputFolder.Label.Text = "Output Folder:"
            outputFolder.Edit.Expand = True
            outputFolder.Edit.TextBox.ReadOnly = True
            outputFolder.BrowseFolder()

            page.ResumeLayout()

            Dim textChanged = Sub(sender As Object, e As EventArgs)
                                  If outputFolder.Edit.Text = "" AndAlso
                                      File.Exists(sourceFile.Edit.Text) Then

                                      outputFolder.Edit.Text = sourceFile.Edit.Text.Dir
                                  End If
                              End Sub

            AddHandler sourceFile.Edit.TextChanged, textChanged
            form.FileDrop = True
            AddHandler form.FilesDropped, Sub(files) sourceFile.Edit.Text = files(0)

            If form.ShowDialog() = DialogResult.OK AndAlso
                File.Exists(sourceFile.Edit.Text) AndAlso
                Directory.Exists(outputFolder.Edit.Text) Then

                Using td As New TaskDialog(Of Demuxer)
                    td.Title = "Select a demuxer"

                    If sourceFile.Edit.Text.Ext = "mkv" Then
                        td.AddCommand("mkvextract", New mkvDemuxer)
                    End If

                    If sourceFile.Edit.Text.Ext.EqualsAny("mp4", "flv") Then
                        td.AddCommand("MP4Box", New MP4BoxDemuxer)
                    End If

                    td.AddCommand("ffmpeg", New ffmpegDemuxer)
                    td.AddCommand("eac3to", New eac3toDemuxer)

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

    <Command("Placeholder for dynamically updated menu items.")>
    Sub DynamicMenuItem(<DispName("ID")> id As DynamicMenuItemID)
    End Sub

    <Command("Dialog that shows available macros.")>
    Sub ShowMacrosDialog()
        MacrosForm.ShowDialogForm()
    End Sub

    <Command("Executes a command line. If Shell Execute is disabled then macros are passed in as environment variables.")>
    Sub ExecuteCommandLine(
        <DispName("Command Line"),
        Description("The command line to be executed. Macros are solved."),
        Editor(GetType(CommandLineTypeEditor), GetType(UITypeEditor))>
        commandLine As String,
        <DispName("Wait For Exit"),
        Description("Halt until the command line returns."),
        DefaultValue(False)>
        waitForExit As Boolean,
        <DispName("Show Process Window"),
        Description("Redirects the output of console apps to StaxRips process window. Disables Shell Execute."),
        DefaultValue(False)>
        showProcessWindow As Boolean,
        <DispName("Use Shell Execute"),
        Description("Executes the command line using the shell. Available when the Show Process Window option is disabled."),
        DefaultValue(True)>
        useShellExecute As Boolean,
        <DispName("Working Directory"),
        Description("Working directory the process will use.")>
        workingDirectory As String)

        Using proc As New Proc(showProcessWindow)
            proc.Header = "Execute Command Line"
            proc.CommandLine = Macro.Expand(commandLine)
            proc.Wait = waitForExit
            proc.WorkingDirectory = Macro.Expand(workingDirectory)

            If Not useShellExecute Then
                proc.Process.StartInfo.UseShellExecute = False
            End If

            Try
                proc.Start()
            Catch ex As Exception
                g.ShowException(ex)
                Log.WriteLine(ex.Message)
            End Try
        End Using
    End Sub

    <Command("Starts a tool by name as shown in the app manage dialog.")>
    Sub StartTool(
        <DispName("Tool Name")>
        <Description("Tool name as shown in the app manage dialog.")>
        name As String)

        Try
            If Package.Items(name).VerifyOK Then
                Package.Items(name).LaunchAction?.Invoke
            End If
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    <Command("Executes PowerShell code.")>
    Sub ExecutePowerShellCode(
        <DispName("Script Code")>
        <Description("PowerShell script code to be executed. Macros are expanded.")>
        <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        code As String,
        <DispName("Use External Shell")>
        <Description("Execute in StaxRip to automate StaxRip or use external shell.")>
        Optional externalShell As Boolean = False)

        code = Macro.Expand(code)

        If externalShell Then
            g.RunCodeInTerminal(code)
        Else
            g.InvokePowerShellCode(code)
        End If
    End Sub

    <Command("Executes a PowerShell script file.")>
    Sub ExecutePowerShellFile(
        <DispName("File Path")>
        <Description("Filepath to a PowerShell script file. May contain macros.")>
        <Editor(GetType(OpenFileDialogEditor), GetType(UITypeEditor))>
        filepath As String,
        <DispName("Arguments")>
        <Description("Semicolon separated arguments passed to the script host. May contain macros.")>
        Optional args As String = Nothing)

        g.InvokePowerShellCode(Macro.Expand(filepath).ReadAllText, Macro.Expand(args).SplitNoEmpty(";"c))
    End Sub

    <Command("Generates various wiki content.")>
    Sub GenerateWikiContent()
        Documentation.GenerateWikiContent()
    End Sub

    <Command("Development tests.")>
    Sub Test()
        If Not g.IsDevelopmentPC Then
            Exit Sub
        End If

        Dim msg = ""

        msg += NVEnc.Test
        msg += QSVEnc.Test
        'msg += Rav1e.Test
        'msg += VCEEnc.Test
        msg += x264Enc.Test
        msg += x265Enc.Test

        For Each pack In Package.Items.Values
            If pack.IsIncluded Then
                If pack.Path = "" Then
                    msg += BR2 + "# path missing for " + pack.Name
                ElseIf Not pack.VersionAllowAny Then
                    If pack.Version = "" Then
                        msg += BR2 + "# version missing for " + pack.Name
                    ElseIf Not pack.IsVersionValid Then
                        msg += BR2 + "# wrong version for " + pack.Name
                    End If
                End If

                'does help file exist?
                If pack.Path <> "" AndAlso pack.HelpFilename <> "" Then
                    If Not File.Exists(pack.Directory + pack.HelpFilename) Then
                        msg += BR2 + $"# Help file of {pack.Name} don't exist!"
                    End If
                End If
            End If
        Next

        If msg <> "" Then
            Dim fs = Folder.Temp + "staxrip test.txt"
            File.WriteAllText(fs, BR + msg.Trim + BR)
            g.ShellExecute(fs)
        Else
            MsgInfo("All Good!")
        End If
    End Sub

    <Command("Plays audio file.")>
    Sub PlaySound(
        <Editor(GetType(OpenFileDialogEditor), GetType(UITypeEditor))>
        <Description("Filepath to a mp3, wav or wmv sound file.")>
        FilePath As String,
        <DispName("Volume (%)")>
        <DefaultValue(20)>
        Volume As Integer)

        Try
            Static player As New Reflector("WMPlayer.OCX.7")
            Dim settings = player.Invoke("settings", BindingFlags.GetProperty)
            settings.Invoke("volume", BindingFlags.SetProperty, Volume)
            settings.Invoke("setMode", "loop", False)
            player.Invoke("URL", BindingFlags.SetProperty, FilePath)
            player.Invoke("controls", BindingFlags.GetProperty).Invoke("play")
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    Function GetApplicationDetails(Optional includeName As Boolean = True, Optional includeVersion As Boolean = True, Optional includeReleaseType As Boolean = True) As String
        Dim sb = New StringBuilder()
        Dim version = Assembly.GetExecutingAssembly.GetName.Version

        If includeName Then sb.Append(" StaxRip")
        If includeVersion Then
            sb.Append($" v{version.Major}.{version.Minor}")

            If version.Build > 0 Then
                sb.Append($".{version.Build}")
            End If
        End If
        If includeReleaseType Then
            If version.Build > 0 Then
                sb.Append(" DEV")
            End If
        End If

        Return sb.ToString.Trim()
    End Function

    <Command("Opens a given help topic in the help browser.")>
    Sub OpenHelpTopic(
        <DispName("Help Topic"),
        Description("Name of the help topic to be opened.")> topic As String)

        Dim form As New HelpForm()

        Select Case topic
            Case "info"
                form.Doc.WriteStart(GetApplicationDetails())
                form.Doc.Write("Active Authors", "Dendraspis, JKyle, Patman, DJATOM")
                form.Doc.Write("Retired Authors", "stax76, 44vince44, Revan654, NikosD, jernst, Brother John, Freepik, ilko-k, nulledone, vanontom")
                form.Doc.Writer.WriteRaw("<hr>")

                Dim licensePath = Folder.Startup + "License.txt"

                If licensePath.FileExists Then
                    form.Doc.WriteParagraph(licensePath.ReadAllText.Trim, True)
                End If
            Case Else
                form.Doc.WriteStart("unknown topic")
                form.Doc.WriteParagraph("The requested help topic '''" + topic + "''' is unknown.")
        End Select

        form.Show()
    End Sub

    <Command("Shows a message box.")>
    Sub ShowMessageBox(
        <DispName("Main Instruction")>
        <Description("Main instruction may contain macros.")>
        <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        mainInstruction As String,
        <DispName("Content")>
        <Description("May contain macros.")>
        <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        Optional content As String = Nothing,
        <DispName("Icon")>
        <DefaultValue(GetType(MsgIcon), "Info")>
        Optional icon As TaskIcon = TaskIcon.Info)

        Msg(Macro.Expand(mainInstruction), Macro.Expand(content), icon, TaskButton.OK)
    End Sub

    <Command("Shows an Open File dialog to show media info.")>
    Sub ShowMediaInfoBrowse()
        Using dialog As New OpenFileDialog
            If dialog.ShowDialog = DialogResult.OK Then
                g.DefaultCommands.ShowMediaInfo(dialog.FileName)
            End If
        End Using
    End Sub

    <Command("Shows media info on a given file.")>
    Sub ShowMediaInfo(
        <DispName("Filepath")>
        <Description("May contain macros.")>
        <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        filepath As String)

        filepath = Macro.Expand(filepath)

        If File.Exists(filepath) Then
            Dim theme = ThemeManager.CurrentTheme

            Dim args = " --theme-colors=" +
                "Foreground:" + theme.General.Controls.RichTextBox.ForeColor.ToHTML + "," +
                "Background:" + theme.General.Controls.RichTextBox.BackColor.ToHTML + "," +
                "TextSelection:" + Color.DeepSkyBlue.ToColorHSL.ToHTML + "," +
                "ItemSelection:" + theme.General.Controls.ToolStrip.DropdownBackgroundSelectedColor.ToHTML + "," +
                "ItemHover:" + theme.General.Controls.ToolStrip.DropdownBackgroundSelectedColor.AddLuminance(0.1).ToHTML + "," +
                "Border:" + theme.General.Controls.RichTextBox.BorderColor.ToHTML + "," +
                "Highlight:" + Color.Green.ToColorHSL.ToHTML

            g.ShellExecute(Package.MediaInfoNET.Path, filepath.Escape + args)
        Else
            Using dialog As New OpenFileDialog
                If dialog.ShowDialog = DialogResult.OK Then
                    ShowMediaInfo(dialog.FileName)
                End If
            End Using
        End If
    End Sub

    <Command("Copies a string to the clipboard.")>
    Sub CopyToClipboard(
        <DispName("Value")>
        <Description("Copies text to the clipboard. May contain macros.")>
        <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        value As String)

        Macro.Expand(value).ToClipboard()
    End Sub

    <Command("Writes a log message to the log file.")>
    Sub WriteLog(
        <DispName("Header"), Description("Header is optional and may contain macros.")>
        header As String,
        <DispName("Message"), Description("Message is optional and may contain macros."),
        Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        message As String)

        Log.WriteHeader(Macro.Expand(header))
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
                    If i.ToUpperInvariant.EndsWith(i2.ToUpperInvariant) Then
                        FileHelp.Delete(i)
                    End If
                Next
            Next
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    <Command("Changes video encoder settings.")>
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

    <Command("Sets a filter replacing an existing filter of same category.")>
    Sub SetFilter(name As String,
                  category As String,
                  <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))> script As String)

        p.Script.SetFilter(category, name, script)
    End Sub

    <Command("Sets the file path of the target file.")>
    Sub SetTargetFile(<DispName("Target File Path")>
                      <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
                      path As String)

        Dim oldEncodeFile = p.TempDir + p.TargetFile.Base + "_out." + p.VideoEncoder.OutputExt
        p.TargetFile = Macro.Expand(path)
        Dim newEncodeFile = p.TempDir + p.TargetFile.Base + "_out." + p.VideoEncoder.OutputExt

        If File.Exists(oldEncodeFile) Then
            File.Move(oldEncodeFile, newEncodeFile)
        End If
    End Sub

    <Command("Loads a source file.")>
    Sub LoadSourceFile(<DispName("Source File Path")> path As String)
        g.MainForm.OpenVideoSourceFile(path)
    End Sub

    <Command("Shows an Open File dialog to open a file to be shown by the console tool mkvinfo.")>
    Sub ShowMkvInfo()
        Using dialog As New OpenFileDialog
            dialog.Filter = "MKV|*.mkv"

            If dialog.ShowDialog = DialogResult.OK Then
                g.RunCodeInTerminal($"& '{Package.mkvinfo.Path}' '{dialog.FileName}'")
            End If
        End Using
    End Sub

    <Command("Shows an Open File dialog to add the remaining HDR10 Metadata to a MKV file.")>
    Sub SaveMKVHDR()
        Using dialog As New OpenFileDialog
            dialog.Filter = "mkv|*.mkv"

            If dialog.ShowDialog = DialogResult.OK Then
                Try
                    MKVInfo.MetadataHDR(dialog.FileName, Nothing)
                Catch ex As Exception
                    g.ShowException(ex)
                End Try
            End If
        End Using
    End Sub

    <Command("Shows an Open File dialog to generate a short GIF.")>
    Sub SaveGIF()
        Using dialog As New OpenFileDialog
            dialog.Multiselect = True

            If dialog.ShowDialog = DialogResult.OK Then
                Using form As New SimpleSettingsForm("Gif Options")
                    form.ScaleClientSize(27, 18)

                    Dim ui = form.SimpleUI
                    Dim page = ui.CreateFlowPage("main page")
                    ui.Store = s
                    page.SuspendLayout()

                    Dim paletteGen = ui.AddMenu(Of String)
                    Dim compression = ui.AddMenu(Of String)
                    Dim paletteUse = ui.AddMenu(Of String)

                    Dim time = ui.AddNum()
                    time.Text = "Starting Time:"
                    time.Config = {1.0, 3600.0, 0.2, 1}
                    time.Help = "The Time Position Where the Animation Should start at in Seconds"
                    time.NumEdit.Value = s.Storage.GetDouble("GifTime", 15.0)
                    time.NumEdit.SaveAction = Sub(value) s.Storage.SetDouble("GifTime", value)

                    Dim length = ui.AddNum()
                    length.Text = "Length:"
                    length.Config = {1.0, 9.0, 0.2, 1}
                    length.Help = "The Length of the Animation in Seconds"
                    length.NumEdit.Value = s.Storage.GetDouble("GifLength", 4.2)
                    length.NumEdit.SaveAction = Sub(value) s.Storage.SetDouble("GifLength", value)

                    Dim frameRate = ui.AddNum()
                    frameRate.Text = "Framerate:"
                    frameRate.Config = {15, 60}
                    frameRate.NumEdit.Value = s.Storage.GetInt("GifFrameRate", 15)
                    frameRate.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("GifFrameRate", CInt(value))

                    Dim scale = ui.AddNum()
                    scale.Text = "Scale:"
                    scale.Config = {240, 2160}
                    scale.NumEdit.Value = s.Storage.GetInt("GifScale", 480)
                    scale.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("GifScale", CInt(value))

                    paletteGen.Text = "Statistics Mode:"
                    paletteGen.Add("Full", "full")
                    paletteGen.Add("Difference", "diff")
                    paletteGen.Button.Value = s.Storage.GetString("PaletteGen", "diff")
                    paletteGen.Button.SaveAction = Sub(value) s.Storage.SetString("PaletteGen", value)

                    paletteUse.Text = "Diff Mode:"
                    paletteUse.Add("Rectangle", "rectangle")
                    paletteUse.Add("None", "none")
                    paletteUse.Button.Value = s.Storage.GetString("PaletteUse", "rectangle")
                    paletteUse.Button.SaveAction = Sub(value) s.Storage.SetString("PaletteUse", value)

                    compression.Text = "Dither:"
                    compression.Add("Bayer Scale", "dither=bayer:bayer_scale=5")
                    compression.Add("Heckbert", "dither=heckbert")
                    compression.Add("Floyd Steinberg", "dither=floyd_steinberg")
                    compression.Add("Sierra 2", "dither=sierra2")
                    compression.Add("Sierra 2_4a", "dither=sierra2_4a")
                    compression.Add("None", "dither=none")
                    compression.Button.Value = s.Storage.GetString("GifDither", "dither=floyd_steinberg")
                    compression.Button.SaveAction = Sub(value) s.Storage.SetString("GifDither", value)

                    Dim output = ui.AddBool()
                    output.Text = "Output Path"
                    output.Checked = s.Storage.GetBool("GifOutput", False)
                    output.SaveAction = Sub(value) s.Storage.SetBool("GifOutput", value)

                    Dim customDirectory = ui.AddTextMenu() 'Custom Output Folder
                    customDirectory.Label.Visible = False
                    customDirectory.Edit.Text = s.Storage.GetString("GifDirectory", p.DefaultTargetFolder)
                    customDirectory.Edit.SaveAction = Sub(value) s.Storage.SetString("GifDirectory", value)
                    customDirectory.AddMenu("Browse Folder...", Function() g.BrowseFolder(p.DefaultTargetFolder))

                    AddHandler output.CheckStateChanged, Sub() customDirectory.Visible = output.Checked = True

                    customDirectory.Visible = output.Checked = True

                    page.ResumeLayout()

                    If form.ShowDialog() = DialogResult.OK Then
                        ui.Save()

                        For Each i In dialog.FileNames
                            Try
                                Animation.GIF(i, Nothing)
                            Catch ex As Exception
                                g.ShowException(ex)
                            End Try
                        Next
                    End If
                End Using
            End If
        End Using
    End Sub


    <Command("Shows an Open File dialog to create a high quality PNG animation.")>
    Sub SavePNG()
        Using dialog As New OpenFileDialog
            dialog.Multiselect = True

            If dialog.ShowDialog = DialogResult.OK Then
                Using form As New SimpleSettingsForm("PNG Options")
                    form.ScaleClientSize(27, 15)

                    Dim ui = form.SimpleUI
                    Dim page = ui.CreateFlowPage("main page")
                    ui.Store = s
                    page.SuspendLayout()

                    Dim opt = ui.AddMenu(Of String)

                    Dim time = ui.AddNum()
                    time.Text = "Starting Time:"
                    time.Config = {1.0, 3600.0, 0.2, 1}
                    time.Help = "The Time Position Where the Animation Should start at in Seconds"
                    time.NumEdit.Value = s.Storage.GetDouble("PNGTime", 15.0)
                    time.NumEdit.SaveAction = Sub(value) s.Storage.SetDouble("PNGTime", value)

                    Dim len = ui.AddNum()
                    len.Text = "Length:"
                    len.Config = {1.0, 9.0, 0.2, 1}
                    len.Help = "The Length of the Animation in Seconds"
                    len.NumEdit.Value = s.Storage.GetDouble("PNGLength", 3.8)
                    len.NumEdit.SaveAction = Sub(value) s.Storage.SetDouble("PNGLength", value)

                    Dim frameRate = ui.AddNum()
                    frameRate.Text = "FrameRate:"
                    frameRate.Config = {15, 60}
                    frameRate.NumEdit.Value = s.Storage.GetInt("PNGFrameRate", 15)
                    frameRate.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("PNGFrameRate", CInt(value))

                    Dim scale = ui.AddNum()
                    scale.Text = "Scale:"
                    scale.Config = {240, 2160}
                    scale.Help = "The Size to Scale the Resolution to"
                    scale.NumEdit.Value = s.Storage.GetInt("PNGScale", 480)
                    scale.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("PNGScale", CInt(value))

                    Dim settings = ui.AddBool()
                    settings.Text = "Enable Opt"
                    settings.Help = "Enable or Disable the Usage of PNG Opt"
                    settings.Checked = s.Storage.GetBool("OptSetting", False)
                    settings.SaveAction = Sub(value) s.Storage.SetBool("OptSetting", value)
                    AddHandler settings.CheckStateChanged, Sub()
                                                               opt.Visible = settings.Checked = True
                                                           End Sub

                    opt.Text = "Opt Settings:"
                    opt.Add("Zlib", "-z0")
                    opt.Add("7zip", "-z1")
                    opt.Add("Zopfli", "-z2")
                    opt.Help = "Compression and Optimization Method Used" + BR + "ZLib = Fastest" + BR + "7Zip = Balance" + BR + "Zopfli = Slowest"
                    opt.Button.Value = s.Storage.GetString("PNGopt", "-z1")
                    opt.Button.SaveAction = Sub(value) s.Storage.SetString("PNGopt", value)

                    opt.Visible = settings.Checked = True

                    Dim output = ui.AddBool()
                    output.Text = "Output Path"
                    output.Checked = s.Storage.GetBool("PNGOutput", False)
                    output.SaveAction = Sub(value) s.Storage.SetBool("PNGOutput", value)

                    Dim customDir = ui.AddTextMenu() 'Custom Output Directory
                    customDir.Label.Visible = False
                    customDir.Edit.Text = s.Storage.GetString("PNGDirectory", p.DefaultTargetFolder)
                    customDir.Edit.SaveAction = Sub(value) s.Storage.SetString("PNGDirectory", value)
                    customDir.AddMenu("Browse Folder...", Function() g.BrowseFolder(p.DefaultTargetFolder))

                    AddHandler output.CheckStateChanged, Sub()
                                                             customDir.Visible = output.Checked = True
                                                         End Sub

                    customDir.Visible = output.Checked = True

                    page.ResumeLayout()

                    If form.ShowDialog() = DialogResult.OK Then
                        ui.Save()

                        For Each i In dialog.FileNames
                            Try
                                Animation.aPNG(i, Nothing)
                            Catch ex As Exception
                                g.ShowException(ex)
                            End Try
                        Next
                    End If
                End Using
            End If
        End Using
    End Sub

    <Command("Shows a dialog to select files, for those thumbnail sheets are created.")>
    Async Sub ShowThumbnailerDialogAsync()
        Using dialog As New OpenFileDialog()
            dialog.Title = "Select the video files - for Thumbnailer settings go to Options!"
            dialog.SetFilter(FileTypes.Video)
            dialog.Multiselect = True

            If dialog.ShowDialog() = DialogResult.OK Then
                Dim filePaths = dialog.FileNames
                If filePaths.Length > 0 Then
                    Dim sw = New Stopwatch()
                    sw.Start()

                    Dim reportWhenFinished = True
                    Dim reportTD As TaskDialog(Of DialogResult)
                    Dim summaryTD As TaskDialog(Of DialogResult)
                    Dim closingTD As TaskDialog(Of DialogResult)
                    Dim cts = New CancellationTokenSource()
                    Dim mainFormClosingHandler As FormClosingEventHandler
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

                            Dim proceededSources = Await Thumbnailer.RunAsync(cts.Token, p, filePaths)

                            If reportTD IsNot Nothing AndAlso Not reportTD.IsDisposingOrDisposed Then
                                reportTD.Close()
                            End If
                            If closingTD IsNot Nothing AndAlso Not closingTD.IsDisposingOrDisposed Then
                                closingTD.Close()
                            End If

                            sw.Stop()

                            RemoveHandler g.MainForm.FormClosing, mainFormClosingHandler
                            Return proceededSources
                        End Function,
                        cts.Token)

                    Using reportTD
                        reportTD = New TaskDialog(Of DialogResult)(DialogResult.Yes)
                        reportTD.Icon = TaskIcon.Question
                        reportTD.Title = "This might take a while..."
                        reportTD.Content = If(filePaths.Length = 1,
                            "The thumbnail sheet is generated in the background. Do you want to get informed when the thumbnail sheet is processed?",
                            "The thumbnail sheets are generated in the background. Do you want to get informed when all thumbnail sheets are processed?")
                        reportTD.AddButton(DialogResult.Yes)
                        reportTD.AddButton(DialogResult.No)
                        reportWhenFinished = reportTD.Show() = DialogResult.Yes
                    End Using

                    If reportWhenFinished Then
                        Dim proceededSources = Await thumbnailerTask
                        RemoveHandler g.MainForm.FormClosing, mainFormClosingHandler
                        If proceededSources Is Nothing OrElse Not proceededSources.Any() Then Return

                        Dim proceededFailedSources = proceededSources.Where(Function(x) Not x.Value)
                        Dim proceededSucceededSources = proceededSources.Where(Function(x) x.Value)
                        Dim attention = proceededFailedSources.Any()
                        Using summaryTD
                            summaryTD = New TaskDialog(Of DialogResult)
                            summaryTD.Icon = If(attention, TaskIcon.Warning, TaskIcon.Info)
                            summaryTD.Title = If(proceededSources.Count = 1,
                                                If(attention, "Thumbnail sheet creation failed!", "Thumbnail sheet has been created"),
                                                If(attention, If(proceededSucceededSources.Any(), "Some thumbnail sheet creation failed!", "All thumbnail sheet creation failed!"), "All thumbnail sheets have been created"))

                            For i = 0 To proceededSources.Count - 1
                                Dim key = proceededSources.ElementAtOrDefault(i).Key
                                Dim value = proceededSources.ElementAtOrDefault(i).Value

                                If value Then
                                    summaryTD.ExpandedContent += $"✔ ""{key}""{BR}"
                                Else
                                    summaryTD.Content += $"❌ ""{key}""{BR}"
                                End If
                            Next

                            summaryTD.ExpandedContent += $"Duration: {sw.ElapsedMilliseconds} ms"
                            summaryTD.AddButton(DialogResult.OK)
                            summaryTD.Show()
                        End Using
                    End If
                End If
            End If
        End Using
    End Sub

    <Command("Presents MediaInfo of all files in a folder in a grid view.")>
    Sub ShowMediaInfoFolderViewDialog()
        Using dialog As New FolderBrowserDialog
            dialog.ShowNewFolderButton = False
            dialog.SetSelectedPath(s.Storage.GetString("MediaInfo Folder View folder"))

            If dialog.ShowDialog = DialogResult.OK Then
                s.Storage.SetString("MediaInfo Folder View folder", dialog.SelectedPath)
                g.InvokePowerShellCode($". '{Package.GetMediaInfo.Path}'; Get-ChildItem '{dialog.SelectedPath}' | Get-MediaInfo | Out-GridView")
            End If
        End Using
    End Sub

    <Command("Shows a dialog allowing to reset specific settings.")>
    Sub ResetSettings()
        Dim sb As New SelectionBox(Of String)

        sb.Title = "Reset Settings"
        sb.Text = "Please select a setting to reset."

        Dim appSettings As New ApplicationSettings
        appSettings.Init()

        For Each i In appSettings.Versions.Keys
            sb.AddItem(i)
        Next

        sb.Items.Sort()

        If sb.Show = DialogResult.OK Then
            s.Versions(sb.SelectedValue) = -1
            MsgInfo("Will be reseted on next startup.", sb.SelectedValue)
        End If
    End Sub

    <Command("Shows a message about removed functionality.")>
    Sub ShowRemovedFunctionalityMessage()
        MsgWarn("Functionality is no longer available.")
    End Sub

    <Command("Adds tags to the container (works only with mkvmerge).")>
    Sub AddTags(
        <DispName("Tags"),
        Description("name 1 = value 1; name 2 = value 2; etc.")>
        tags As String)

        For Each i In tags.Split(";"c)
            If i.Contains("=") Then
                Dim left = i.Left("=").Trim
                Dim right = i.Right("=").Trim
                p.VideoEncoder.Muxer.Tags.Add(New StringPair(left, right))
            End If
        Next
    End Sub
End Class
