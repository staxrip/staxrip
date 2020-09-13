
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Management.Automation
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text
Imports DirectN
Imports Microsoft.Win32
Imports StaxRip.UI

Public Class GlobalCommands
    <Command("Checks if a update is available.")>
    Sub CheckForUpdate()
        Update.CheckForUpdate(True, s.CheckForUpdatesBeta, Environment.Is64BitProcess)
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

            Dim textChanged = Sub()
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
                    td.MainInstruction = "Select a demuxer."

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

        commandLine = Macro.Expand(commandLine)

        Using proc As New Proc(showProcessWindow)
            proc.Header = "Execute Command Line"
            proc.CommandLine = commandLine
            proc.Wait = waitForExit
            proc.WorkingDirectory = workingDirectory

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

    <Command("Executes a PowerShell PS1 script file.")>
    Sub ExecuteScriptFile(
        <DispName("File Path")>
        <Description("Filepath to a PowerShell PS1 script file. May contain macros.")>
        <Editor(GetType(OpenFileDialogEditor), GetType(UITypeEditor))>
        filepath As String)

        filepath = Macro.Expand(filepath)

        ExecutePowerShellScript(filepath.ReadAllText)
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

    <Command("Executes PowerShell script code.")>
    Sub ExecutePowerShellScript(
        <DispName("Script Code")>
        <Description("PowerShell script code to be executed.")>
        <Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        code As String,
        <DispName("Use External Shell")>
        <Description("Execute in StaxRip to automate StaxRip or use external shell.")>
        Optional externalShell As Boolean = False)

        If externalShell Then
            g.RunCodeInTerminal(code)
        Else
            g.InvokePowerShellCode(code)
        End If
    End Sub

    <Command("Development tests and creation of doc files.")>
    Sub TestAndDynamicFileCreation()
        Documentation.GenerateDynamicFiles()

        Dim msg = ""

        msg += NVEnc.Test
        msg += QSVEnc.Test
        'msg += Rav1e.Test
        'msg += VCEEnc.Test
        msg += x264Enc.Test
        msg += x265Enc.Test

        For Each pack In Package.Items.Values
            If pack.HelpFilename.Ext = "md" Then
                msg += BR2 + "# local MD file for " + pack.Name
            End If

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
            MsgInfo("No issues found.")
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

    Function GetReleaseType() As String
        Dim version = Assembly.GetExecutingAssembly.GetName.Version

        If version.MinorRevision <> 0 Then
            Return "Beta"
        End If
    End Function

    <Command("Opens a given help topic In the help browser.")>
    Sub OpenHelpTopic(
        <DispName("Help Topic"),
        Description("Name Of the help topic To be opened.")> topic As String)

        Dim form As New HelpForm()

        Select Case topic
            Case "info"
                form.Doc.WriteStart("StaxRip " + Application.ProductVersion + " " + GetReleaseType())
                form.Doc.Write("Development", "stax76, Revan654, Dendraspis")
                form.Doc.Write("Contributions", "Patman, 44vince44, JKyle, NikosD, qyot27, ernst, Brother John, Freepik, ilko-k, nulledone, vanontom")

                Dim licensePath = Folder.Startup + "License.txt"

                If File.Exists(licensePath) Then
                    form.Doc.WriteParagraph(licensePath.ReadAllText, True)
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
        Optional icon As MsgIcon = MsgIcon.Info)

        Msg(Macro.Expand(mainInstruction), Macro.Expand(content), icon, TaskDialogButtons.Ok)
    End Sub

    <Command("Shows a Open File dialog to show media info.")>
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
            g.ShellExecute(Package.MediaInfoNET.Path, filepath.Escape)
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
                    If i.ToUpper.EndsWith(i2.ToUpper) Then
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

    <Command("Loads a source file.")>
    Sub LoadSourceFile(<DispName("Source File Path")> path As String)
        g.MainForm.OpenVideoSourceFile(path)
    End Sub

    <Command("Shows a Open File dialog to open a file to be shown by the console tool mkvinfo.")>
    Sub ShowMkvInfo()
        Using dialog As New OpenFileDialog
            dialog.Filter = "MKV|*.mkv"

            If dialog.ShowDialog = DialogResult.OK Then
                g.RunCodeInTerminal($"& '{Package.mkvinfo.Path}' '{dialog.FileName}'")
            End If
        End Using
    End Sub

    <Command("Shows a Open File dialog to add the remaining HDR10 Metadata to a MKV file.")>
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

    <Command("Shows a Open File dialog to generate a short GIF.")>
    Sub SaveGIF()
        Using dialog As New OpenFileDialog
            dialog.Multiselect = True

            If dialog.ShowDialog = DialogResult.OK Then
                Using f As New SimpleSettingsForm("Gif Options")
                    f.ScaleClientSize(27, 18)

                    Dim ui = f.SimpleUI
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

                    If f.ShowDialog() = DialogResult.OK Then
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

    <Command("Shows a Open File dialog to generate thumbnails using mtn engine")>
    Sub SaveMTN()
        Using dialog As New OpenFileDialog
            dialog.Multiselect = True

            If dialog.ShowDialog = DialogResult.OK Then
                Using form As New SimpleSettingsForm("Thumbnail Options")
                    form.ScaleClientSize(27, 15)

                    Dim ui = form.SimpleUI
                    Dim page = ui.CreateFlowPage("main page")
                    ui.Store = s
                    page.SuspendLayout()

                    Dim column = ui.AddNum()
                    column.Text = "Columns:"
                    column.Config = {1, 12}
                    column.NumEdit.Value = s.Storage.GetInt("MTNColumn", 4)
                    column.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("MTNColumn", CInt(value))

                    Dim row = ui.AddNum()
                    row.Text = "Rows:"
                    row.Config = {1, 12}
                    row.NumEdit.Value = s.Storage.GetInt("MTNRow", 6)
                    row.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("MTNRow", CInt(value))

                    Dim quality = ui.AddNum()
                    quality.Text = "Quality:"
                    quality.Config = {25, 100}
                    quality.NumEdit.Value = s.Storage.GetInt("MTNQuality", 95)
                    quality.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("MTNQuality", CInt(value))

                    Dim height = ui.AddNum()
                    height.Text = "Height:"
                    height.Config = {150, 500}
                    height.NumEdit.Value = s.Storage.GetInt("MTNHeight", 250)
                    height.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("MTNHeight", CInt(value))

                    Dim width = ui.AddNum()
                    width.Text = "Width:"
                    width.Config = {960, 2000}
                    width.NumEdit.Value = s.Storage.GetInt("MTNWidth", 1920)
                    width.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("MTNWidth", CInt(value))

                    Dim depth = ui.AddNum()
                    depth.Text = "Depth:"
                    depth.Config = {4, 12}
                    depth.NumEdit.Value = s.Storage.GetInt("MTNDepth", 12)
                    depth.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("MTNDepth", CInt(value))

                    Dim output = ui.AddBool()
                    output.Text = "Custom Output"
                    output.Checked = s.Storage.GetBool("MTNOutput", False)
                    output.SaveAction = Sub(value) s.Storage.SetBool("MTNOutput", value)

                    Dim customDir = ui.AddTextMenu() 'Custom Output Directory
                    customDir.Label.Visible = False
                    customDir.Edit.Text = s.Storage.GetString("MTNDirectory", p.DefaultTargetFolder)
                    customDir.Edit.SaveAction = Sub(value) s.Storage.SetString("MTNDirectory", value)
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
                                MTN.Thumbnails(i, Nothing)
                            Catch ex As Exception
                                g.ShowException(ex)
                            End Try
                        Next

                    End If
                End Using
            End If
        End Using
    End Sub

    <Command("Shows a open file dialog to create a high quality PNG animation.")>
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

    <Command("Shows a dialog to generate thumbnails.")>
    Sub ShowBatchGenerateThumbnailsDialog()
        Using dialog As New OpenFileDialog
            dialog.SetFilter(FileTypes.Video)
            dialog.Multiselect = True

            If dialog.ShowDialog = DialogResult.OK Then
                Using form As New SimpleSettingsForm("Thumbnail Options")
                    form.ScaleClientSize(27, 20)

                    Dim ui = form.SimpleUI
                    Dim page = ui.CreateFlowPage("main page")
                    ui.Store = s
                    page.SuspendLayout()

                    Dim row As SimpleUI.NumBlock
                    Dim interval As SimpleUI.NumBlock

                    Dim mode = ui.AddMenu(Of Integer)
                    mode.Text = "Row Count Mode"
                    mode.Expandet = True
                    mode.Add("Manual", 0)
                    mode.Add("Row count is calculated based on time interval", 1)
                    mode.Button.Value = s.Storage.GetInt("Thumbnail Mode")
                    mode.Button.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Mode", value)

                    AddHandler mode.Button.ValueChangedUser, Sub()
                                                                 row.Visible = mode.Button.Value = 0
                                                                 interval.Visible = mode.Button.Value = 1
                                                             End Sub
                    Dim m = ui.AddMenu(Of Integer)
                    m.Text = "Timestamp Position"
                    m.Add("Left Top", 0)
                    m.Add("Right Top", 1)
                    m.Add("Left Bottom", 2)
                    m.Add("Right Bottom", 3)
                    m.Button.Value = s.Storage.GetInt("Thumbnail Position", 3)
                    m.Button.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Position", value)

                    Dim k = ui.AddMenu(Of String)
                    k.Text = "Picture Format:"
                    k.Add("JPG", "jpg")
                    k.Add("PNG", "png")
                    k.Add("TIFF", "tiff")
                    k.Add("BMP", "bmp")
                    k.Button.Value = s.Storage.GetString("Picture Format", "png")
                    k.Button.SaveAction = Sub(value) s.Storage.SetString("Picture Format", CStr(value))

                    Dim cp = ui.AddColorPicker()
                    cp.Text = "Background Color"
                    cp.Field = NameOf(s.ThumbnailBackgroundColor)

                    Dim nb = ui.AddNum()
                    nb.Text = "Thumbnail Width:"
                    nb.Config = {200, 4000, 10}
                    nb.NumEdit.Value = s.Storage.GetInt("Thumbnail Width", 500)
                    nb.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Width", CInt(value))

                    nb = ui.AddNum()
                    nb.Text = "Column Count:"
                    nb.Config = {1, 20}
                    nb.NumEdit.Value = s.Storage.GetInt("Thumbnail Columns", 4)
                    nb.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Columns", CInt(value))

                    row = ui.AddNum()
                    row.Text = "Row Count:"
                    row.Config = {1, 20}
                    row.NumEdit.Value = s.Storage.GetInt("Thumbnail Rows", 6)
                    row.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Rows", CInt(value))

                    interval = ui.AddNum()
                    interval.Text = "Interval (seconds):"
                    interval.NumEdit.Value = s.Storage.GetInt("Thumbnail Interval")
                    interval.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Interval", CInt(value))

                    row.Visible = mode.Button.Value = 0
                    interval.Visible = mode.Button.Value = 1

                    Dim margin = ui.AddNum()
                    margin.Text = "Margin:"
                    margin.Config = {0, 100}
                    margin.NumEdit.Value = s.Storage.GetInt("Thumbnail Margin", 5)
                    margin.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Margin", CInt(value))

                    Dim cq = ui.AddNum()
                    cq.Text = "Compression Quality:"
                    cq.Config = {1, 100}
                    cq.NumEdit.Value = s.Storage.GetInt("Thumbnail Compression Quality", 95)
                    cq.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Compression Quality", CInt(value))
                    AddHandler k.Button.ValueChangedUser, Sub() cq.Visible = k.Button.Value = "jpg"

                    Dim logo = ui.AddBool()
                    logo.Text = "Disable StaxRip Logo"
                    logo.Help = "Enable or disable the StaxRip Watermark"
                    logo.Checked = s.Storage.GetBool("Logo", False)
                    logo.SaveAction = Sub(value) s.Storage.SetBool("Logo", CBool(value))

                    Dim output = ui.AddBool()
                    output.Text = "Output Path"
                    output.Checked = s.Storage.GetBool("StaxRipOutput", False)
                    output.SaveAction = Sub(value) s.Storage.SetBool("StaxRipOutput", value)

                    Dim customDir = ui.AddTextButton()
                    customDir.Visible = output.Checked
                    customDir.Expandet = True
                    customDir.Label.Visible = False
                    customDir.Edit.Text = s.Storage.GetString("StaxRipDirectory", p.DefaultTargetFolder)
                    customDir.Edit.SaveAction = Sub(value) s.Storage.SetString("StaxRipDirectory", value)
                    customDir.BrowseFolder()

                    AddHandler output.CheckStateChanged, Sub() customDir.Visible = output.Checked = True

                    page.ResumeLayout()

                    If form.ShowDialog() = DialogResult.OK Then
                        ui.Save()

                        For Each i In dialog.FileNames
                            Try
                                Thumbnails.SaveThumbnails(i, Nothing)
                            Catch ex As Exception
                                g.ShowException(ex)
                            End Try
                        Next

                        MsgInfo("Thumbnails have been created.")
                    End If
                End Using
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
            s.Versions(sb.SelectedValue) = 0
            MsgInfo("Will be reseted on next startup.")
        End If
    End Sub

    <Command("This command is obsolete since 2020.")>
    Sub ShowLAVFiltersConfigDialog()
        Dim ret = Registry.ClassesRoot.GetString("CLSID\" + GUIDS.LAVVideoDecoder.ToString + "\InprocServer32", Nothing)

        If File.Exists(ret) Then
            Static loaded As Boolean

            If Not loaded Then
                Native.LoadLibrary(ret)
                loaded = True
            End If

            OpenConfiguration(Nothing, Nothing, Nothing, Nothing)
        Else
            MsgError("The LAV Filters video decoder library could not be located.")
        End If
    End Sub

    <DllImport("LAVVideo.ax")>
    Shared Sub OpenConfiguration(hwnd As IntPtr, hinst As IntPtr, lpszCmdLine As String, nCmdShow As Integer)
    End Sub

    <Command("This command is obsolete since 2020.")>
    Sub ShowCommandPrompt()
        g.RunCommandInTerminal("cmd.exe")
    End Sub

    <Command("This command is obsolete since 2020.")>
    Sub ShowPowerShell()
        g.RunCommandInTerminal("powershell.exe")
    End Sub

    <Command("This command is obsolete since 2020.")>
    Sub ExecuteBatchScript(
        <DispName("Batch Script Code"),
        Description("Batch script code to be executed. Macros are solved as well as passed in as environment variables."),
        Editor(GetType(CommandLineTypeEditor), GetType(UITypeEditor))>
        batchScript As String)

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

            Try
                proc.Start()
            Catch ex As Exception
                g.ShowException(ex)
                Log.WriteLine(ex.Message)
            End Try
        End Using
    End Sub

    <Command("This command is obsolete since 2020.")>
    Sub MediainfoMKV()
        ShowMkvInfo()
    End Sub

    <Command("This command is obsolete since 2020.")>
    Sub MediaInfoShowMedia()
        ShowMediaInfoBrowse()
    End Sub
End Class
