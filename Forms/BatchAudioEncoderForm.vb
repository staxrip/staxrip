Imports StaxRip.UI

Class BatchAudioEncoderForm
    Inherits DialogBase

#Region " Designer "
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
    Friend WithEvents EditControl As StaxRip.MacroEditorControl
    Friend WithEvents tbStreamName As System.Windows.Forms.TextBox
    Friend WithEvents lStreamName As System.Windows.Forms.Label
    Friend WithEvents lInput As System.Windows.Forms.Label
    Friend WithEvents tbChannels As System.Windows.Forms.TextBox
    Friend WithEvents lChannels As System.Windows.Forms.Label
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents tbType As System.Windows.Forms.TextBox
    Friend WithEvents mbLanguage As StaxRip.UI.MenuButton
    Friend WithEvents tbProfileName As TextBoxEx
    Friend WithEvents laProfileName As LabelEx
    Friend WithEvents bnMenu As ButtonEx
    Friend WithEvents cms As ContextMenuStripEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.tbInput = New System.Windows.Forms.TextBox()
        Me.lInput = New System.Windows.Forms.Label()
        Me.lType = New System.Windows.Forms.Label()
        Me.lBitrate = New System.Windows.Forms.Label()
        Me.tbBitrate = New System.Windows.Forms.TextBox()
        Me.lLanguage = New System.Windows.Forms.Label()
        Me.lDelay = New System.Windows.Forms.Label()
        Me.tbDelay = New System.Windows.Forms.TextBox()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.tbStreamName = New System.Windows.Forms.TextBox()
        Me.tbChannels = New System.Windows.Forms.TextBox()
        Me.ValidationProvider = New StaxRip.UI.ValidationProvider()
        Me.EditControl = New StaxRip.MacroEditorControl()
        Me.lStreamName = New System.Windows.Forms.Label()
        Me.lChannels = New System.Windows.Forms.Label()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tbType = New System.Windows.Forms.TextBox()
        Me.mbLanguage = New StaxRip.UI.MenuButton()
        Me.tbProfileName = New StaxRip.UI.TextBoxEx()
        Me.laProfileName = New StaxRip.UI.LabelEx()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
        Me.cms = New StaxRip.UI.ContextMenuStripEx(Me.components)
        Me.SuspendLayout()
        '
        'tbInput
        '
        Me.tbInput.Location = New System.Drawing.Point(20, 70)
        Me.tbInput.Margin = New System.Windows.Forms.Padding(5)
        Me.tbInput.Name = "tbInput"
        Me.tbInput.Size = New System.Drawing.Size(501, 55)
        Me.tbInput.TabIndex = 1
        '
        'lInput
        '
        Me.lInput.AutoSize = True
        Me.lInput.Location = New System.Drawing.Point(20, 19)
        Me.lInput.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lInput.Name = "lInput"
        Me.lInput.Size = New System.Drawing.Size(452, 48)
        Me.lInput.TabIndex = 0
        Me.lInput.Text = "Supported Input File Types:"
        '
        'lType
        '
        Me.lType.AutoSize = True
        Me.lType.Location = New System.Drawing.Point(555, 17)
        Me.lType.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lType.Name = "lType"
        Me.lType.Size = New System.Drawing.Size(289, 48)
        Me.lType.TabIndex = 11
        Me.lType.Text = "Output File Type:"
        '
        'lBitrate
        '
        Me.lBitrate.AutoSize = True
        Me.lBitrate.Location = New System.Drawing.Point(14, 135)
        Me.lBitrate.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lBitrate.Name = "lBitrate"
        Me.lBitrate.Size = New System.Drawing.Size(132, 48)
        Me.lBitrate.TabIndex = 2
        Me.lBitrate.Text = "Bitrate:"
        '
        'tbBitrate
        '
        Me.tbBitrate.Location = New System.Drawing.Point(20, 192)
        Me.tbBitrate.Margin = New System.Windows.Forms.Padding(5)
        Me.tbBitrate.Name = "tbBitrate"
        Me.ValidationProvider.SetPattern(Me.tbBitrate, "^[1-9]+\d*$")
        Me.tbBitrate.Size = New System.Drawing.Size(140, 55)
        Me.tbBitrate.TabIndex = 3
        Me.tbBitrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lLanguage
        '
        Me.lLanguage.AutoSize = True
        Me.lLanguage.Location = New System.Drawing.Point(1194, 17)
        Me.lLanguage.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lLanguage.Name = "lLanguage"
        Me.lLanguage.Size = New System.Drawing.Size(182, 48)
        Me.lLanguage.TabIndex = 13
        Me.lLanguage.Text = "Language:"
        '
        'lDelay
        '
        Me.lDelay.AutoSize = True
        Me.lDelay.Location = New System.Drawing.Point(375, 135)
        Me.lDelay.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lDelay.Name = "lDelay"
        Me.lDelay.Size = New System.Drawing.Size(116, 48)
        Me.lDelay.TabIndex = 7
        Me.lDelay.Text = "Delay:"
        '
        'tbDelay
        '
        Me.tbDelay.Location = New System.Drawing.Point(381, 192)
        Me.tbDelay.Margin = New System.Windows.Forms.Padding(5)
        Me.tbDelay.Name = "tbDelay"
        Me.ValidationProvider.SetPattern(Me.tbDelay, "^(-?[1-9]+\d*|-?0)$")
        Me.tbDelay.Size = New System.Drawing.Size(140, 55)
        Me.tbDelay.TabIndex = 8
        Me.tbDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbStreamName
        '
        Me.tbStreamName.Location = New System.Drawing.Point(561, 192)
        Me.tbStreamName.Margin = New System.Windows.Forms.Padding(5)
        Me.tbStreamName.Name = "tbStreamName"
        Me.tbStreamName.Size = New System.Drawing.Size(598, 55)
        Me.tbStreamName.TabIndex = 10
        '
        'tbChannels
        '
        Me.tbChannels.Location = New System.Drawing.Point(201, 192)
        Me.tbChannels.Margin = New System.Windows.Forms.Padding(5)
        Me.tbChannels.Name = "tbChannels"
        Me.ValidationProvider.SetPattern(Me.tbChannels, "^[1-9]{1}$")
        Me.tbChannels.Size = New System.Drawing.Size(140, 55)
        Me.tbChannels.TabIndex = 6
        Me.tbChannels.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'EditControl
        '
        Me.EditControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EditControl.Location = New System.Drawing.Point(14, 255)
        Me.EditControl.Margin = New System.Windows.Forms.Padding(5)
        Me.EditControl.Name = "EditControl"
        Me.EditControl.Size = New System.Drawing.Size(1787, 414)
        Me.EditControl.TabIndex = 5
        Me.EditControl.Text = "Batch Code"
        '
        'lStreamName
        '
        Me.lStreamName.AutoSize = True
        Me.lStreamName.Location = New System.Drawing.Point(555, 135)
        Me.lStreamName.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lStreamName.Name = "lStreamName"
        Me.lStreamName.Size = New System.Drawing.Size(244, 48)
        Me.lStreamName.TabIndex = 9
        Me.lStreamName.Text = "Stream Name:"
        '
        'lChannels
        '
        Me.lChannels.AutoSize = True
        Me.lChannels.Location = New System.Drawing.Point(195, 135)
        Me.lChannels.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lChannels.Name = "lChannels"
        Me.lChannels.Size = New System.Drawing.Size(171, 48)
        Me.lChannels.TabIndex = 4
        Me.lChannels.Text = "Channels:"
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(1601, 679)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(5)
        Me.bnCancel.Size = New System.Drawing.Size(200, 65)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(1391, 679)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(5)
        Me.bnOK.Size = New System.Drawing.Size(200, 65)
        Me.bnOK.Text = "OK"
        '
        'tbType
        '
        Me.tbType.Location = New System.Drawing.Point(561, 70)
        Me.tbType.Margin = New System.Windows.Forms.Padding(5)
        Me.tbType.Name = "tbType"
        Me.tbType.Size = New System.Drawing.Size(180, 55)
        Me.tbType.TabIndex = 15
        '
        'mbLanguage
        '
        Me.mbLanguage.Location = New System.Drawing.Point(1200, 65)
        Me.mbLanguage.Margin = New System.Windows.Forms.Padding(5)
        Me.mbLanguage.ShowMenuSymbol = True
        Me.mbLanguage.Size = New System.Drawing.Size(335, 60)
        '
        'tbProfileName
        '
        Me.tbProfileName.Location = New System.Drawing.Point(1200, 192)
        Me.tbProfileName.Size = New System.Drawing.Size(598, 55)
        '
        'laProfileName
        '
        Me.laProfileName.AutoSize = True
        Me.laProfileName.Location = New System.Drawing.Point(1194, 135)
        Me.laProfileName.Size = New System.Drawing.Size(235, 48)
        Me.laProfileName.Text = "Name:"
        Me.laProfileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnMenu.ContextMenuStrip = Me.cms
        Me.bnMenu.Location = New System.Drawing.Point(1316, 679)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(5)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(65, 65)
        '
        'cms
        '
        Me.cms.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cms.ImageScalingSize = New System.Drawing.Size(48, 48)
        Me.cms.Name = "cms"
        Me.cms.Size = New System.Drawing.Size(61, 4)
        '
        'BatchAudioEncoderForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1815, 758)
        Me.Controls.Add(Me.bnMenu)
        Me.Controls.Add(Me.laProfileName)
        Me.Controls.Add(Me.tbProfileName)
        Me.Controls.Add(Me.mbLanguage)
        Me.Controls.Add(Me.tbType)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.bnOK)
        Me.Controls.Add(Me.tbChannels)
        Me.Controls.Add(Me.lChannels)
        Me.Controls.Add(Me.lStreamName)
        Me.Controls.Add(Me.tbStreamName)
        Me.Controls.Add(Me.EditControl)
        Me.Controls.Add(Me.lLanguage)
        Me.Controls.Add(Me.tbDelay)
        Me.Controls.Add(Me.lDelay)
        Me.Controls.Add(Me.tbBitrate)
        Me.Controls.Add(Me.lBitrate)
        Me.Controls.Add(Me.lType)
        Me.Controls.Add(Me.tbInput)
        Me.Controls.Add(Me.lInput)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(11, 10, 11, 10)
        Me.Name = "BatchAudioEncoderForm"
        Me.Text = "Batch Audio Settings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tbInput As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lType As System.Windows.Forms.Label
    Friend WithEvents lBitrate As System.Windows.Forms.Label
    Friend WithEvents tbBitrate As System.Windows.Forms.TextBox
    Friend WithEvents lLanguage As System.Windows.Forms.Label
    Friend WithEvents lDelay As System.Windows.Forms.Label
    Friend WithEvents tbDelay As System.Windows.Forms.TextBox
    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents ValidationProvider As StaxRip.UI.ValidationProvider
#End Region

    Private Profile, TempProfile As BatchAudioProfile

    Sub New(profile As BatchAudioProfile)
        InitializeComponent()

        Me.Profile = profile
        TempProfile = ObjectHelp.GetCopy(Of BatchAudioProfile)(profile)
        tbType.Text = TempProfile.OutputFileType
        tbInput.Text = TempProfile.SupportedInput.Join(" ")
        If TempProfile.Name <> TempProfile.DefaultName Then tbProfileName.Text = TempProfile.Name
        tbProfileName.SendMessageCue(TempProfile.Name, False)

        EditControl.SetCommandLineDefaults()
        EditControl.Value = TempProfile.CommandLines
        EditControl.SpecialMacrosFunction = AddressOf TempProfile.ExpandMacros

        AddHandler EditControl.rtbEdit.TextChanged, AddressOf tbEditTextChanged

        tbStreamName.Text = TempProfile.StreamName
        tbBitrate.Text = TempProfile.Bitrate.ToString
        tbDelay.Text = TempProfile.Delay.ToString
        tbChannels.Text = TempProfile.Channels.ToString

        TipProvider.SetTip("Audio channels count", tbChannels, lChannels)
        TipProvider.SetTip("Display language of the track.", mbLanguage, lLanguage)
        TipProvider.SetTip("The targeted bitrate of the output file.", tbBitrate, lBitrate)
        TipProvider.SetTip("Audio delay to be fixed.", tbDelay, lDelay)
        TipProvider.SetTip("File types accepted as input, leave empty to support any file type.", tbInput, lInput)
        TipProvider.SetTip("Stream name used by the muxer. The stream name may contain macros.", tbStreamName, lStreamName)
        TipProvider.SetTip("If no name is defined StaxRip auto generate the name.", laProfileName, tbProfileName)

        cms.Add("Execute Batch Code", AddressOf Execute, Nothing, TempProfile.File <> "").SetImage(Symbol.fa_terminal)
        cms.Add("Copy Batch Code", Sub() Clipboard.SetText(TempProfile.GetCode))
        cms.Add("Show Batch Code...", Sub() g.ShowCommandLinePreview("Batch Code", TempProfile.GetCode))
        cms.Add("Save Profile...", AddressOf SaveProfile, "Saves the current settings as profile").SetImage(Symbol.Save)
        cms.Add("Help", AddressOf ShowHelp).SetImage(Symbol.Help)

        ActiveControl = bnOK
    End Sub

    Sub SaveProfile()
        Dim gap = ObjectHelp.GetCopy(Of BatchAudioProfile)(TempProfile)
        Dim name = InputBox.Show("Enter the profile name.", "Save Profile", gap.Name)

        If name <> "" Then
            gap.Name = name
            s.AudioProfiles.Add(gap)
            MsgInfo("The profile was saved.")
        End If
    End Sub

    Sub Execute()
        Dim batchPath = p.TempDir + p.TargetFile.Base + "_aexe.bat"
        Dim batchCode = Proc.WriteBatchFile(batchPath, TempProfile.GetCode)
        Dim batchProc As New Process
        batchProc.StartInfo.FileName = "cmd.exe"
        batchProc.StartInfo.Arguments = "/k """ + batchPath + """"
        batchProc.StartInfo.WorkingDirectory = p.TempDir
        batchProc.Start()
    End Sub

    Private Sub CommandLineAudioSettingsForm_FormClosed() Handles Me.FormClosed
        If DialogResult = DialogResult.OK Then
            Profile.SupportedInput = TempProfile.SupportedInput
            Profile.OutputFileType = TempProfile.OutputFileType
            Profile.CommandLines = TempProfile.CommandLines
            Profile.Bitrate = TempProfile.Bitrate
            Profile.StreamName = TempProfile.StreamName
            Profile.Channels = TempProfile.Channels
            Profile.Name = TempProfile.Name
            Profile.Language = TempProfile.Language
            Profile.Delay = TempProfile.Delay
        End If
    End Sub

    Private Sub tbStreamName_TextChanged() Handles tbStreamName.TextChanged
        TempProfile.StreamName = tbStreamName.Text
        EditControl.UpdatePreview()
    End Sub

    Private Sub tbInput_TextChanged() Handles tbInput.TextChanged
        TempProfile.SupportedInput = tbInput.Text.ToLower.SplitNoEmptyAndWhiteSpace(",", ";", " ")
        EditControl.UpdatePreview()
    End Sub

    Private Sub tbType_TextChanged() Handles tbType.TextChanged
        TempProfile.OutputFileType = tbType.Text
        EditControl.UpdatePreview()
    End Sub

    Private Sub tbBitrate_TextChanged() Handles tbBitrate.TextChanged
        TempProfile.Bitrate = tbBitrate.Text.ToInt
        EditControl.UpdatePreview()
    End Sub

    Private Sub tbChannels_TextChanged() Handles tbChannels.TextChanged
        TempProfile.Channels = tbChannels.Text.ToInt
        EditControl.UpdatePreview()
    End Sub

    Private Sub mbLanguage_ValueChangedUser(value As Object) Handles mbLanguage.ValueChangedUser
        TempProfile.Language = DirectCast(mbLanguage.Value, Language)
        EditControl.UpdatePreview()
    End Sub

    Private Sub tbEditTextChanged(sender As Object, args As EventArgs)
        TempProfile.CommandLines = EditControl.rtbEdit.Text
        EditControl.UpdatePreview()
    End Sub

    Private Sub tbDelay_TextChanged() Handles tbDelay.TextChanged
        TempProfile.Delay = tbDelay.Text.ToInt
        EditControl.UpdatePreview()
    End Sub

    Private Sub tbProfileName_TextChanged(sender As Object, e As EventArgs) Handles tbProfileName.TextChanged
        TempProfile.Name = tbProfileName.Text
    End Sub

    Sub ShowHelp()
        Dim f As New HelpForm()

        f.Doc.WriteStart(Text)
        f.Doc.WriteP("The batch audio settings define audio conversion batch code.")
        f.Doc.WriteTips(TipProvider.GetTips, EditControl.TipProvider.GetTips)
        f.Doc.WriteP("Macros", Strings.MacrosHelp)

        Dim l As New StringPairList

        l.Add("%input%", "Audio source file")
        l.Add("%output%", "Audio target File")
        l.Add("%bitrate%", "Audio bitrate")
        l.Add("%delay%", "Audio delay")
        l.Add("%channels%", "Audio channels count")
        l.Add("%language_native%", "Native language name")
        l.Add("%language_english%", "English language name")

        f.Doc.WriteTable("Batch Audio Settings Macros", "The macros below are only available in the batch audio settings dialog and override global macros with the same name in case they are defined in both scopes.", l)
        f.Doc.WriteTable("Global Macros", Macro.GetTips())
        f.Show()
    End Sub

    Private Sub BatchAudioEncoderForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Refresh()

        For Each i In Language.Languages
            If i.IsCommon Then
                mbLanguage.Add(i.ToString, i)
            Else
                mbLanguage.Add("More | " + i.ToString.Substring(0, 1).ToUpper + " | " + i.ToString, i)
            End If
        Next

        mbLanguage.Value = TempProfile.Language
    End Sub

    Private Sub BatchAudioEncoderForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        ShowHelp()
    End Sub
End Class