Imports StaxRip.UI

Class CommandLineAudioForm
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
        Me.SuspendLayout()
        '
        'tbInput
        '
        Me.tbInput.Location = New System.Drawing.Point(12, 41)
        Me.tbInput.Name = "tbInput"
        Me.tbInput.Size = New System.Drawing.Size(386, 31)
        Me.tbInput.TabIndex = 1
        '
        'lInput
        '
        Me.lInput.AutoSize = True
        Me.lInput.Location = New System.Drawing.Point(12, 11)
        Me.lInput.Name = "lInput"
        Me.lInput.Size = New System.Drawing.Size(198, 25)
        Me.lInput.TabIndex = 0
        Me.lInput.Text = "Supported Input Types:"
        '
        'lType
        '
        Me.lType.AutoSize = True
        Me.lType.Location = New System.Drawing.Point(414, 10)
        Me.lType.Name = "lType"
        Me.lType.Size = New System.Drawing.Size(146, 25)
        Me.lType.TabIndex = 11
        Me.lType.Text = "Output File Type:"
        '
        'lBitrate
        '
        Me.lBitrate.AutoSize = True
        Me.lBitrate.Location = New System.Drawing.Point(12, 79)
        Me.lBitrate.Name = "lBitrate"
        Me.lBitrate.Size = New System.Drawing.Size(66, 25)
        Me.lBitrate.TabIndex = 2
        Me.lBitrate.Text = "Bitrate:"
        '
        'tbBitrate
        '
        Me.tbBitrate.Location = New System.Drawing.Point(12, 109)
        Me.tbBitrate.Name = "tbBitrate"
        Me.ValidationProvider.SetPattern(Me.tbBitrate, "^[1-9]+\d*$")
        Me.tbBitrate.Size = New System.Drawing.Size(70, 31)
        Me.tbBitrate.TabIndex = 3
        Me.tbBitrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lLanguage
        '
        Me.lLanguage.AutoSize = True
        Me.lLanguage.Location = New System.Drawing.Point(611, 10)
        Me.lLanguage.Name = "lLanguage"
        Me.lLanguage.Size = New System.Drawing.Size(93, 25)
        Me.lLanguage.TabIndex = 13
        Me.lLanguage.Text = "Language:"
        '
        'lDelay
        '
        Me.lDelay.AutoSize = True
        Me.lDelay.Location = New System.Drawing.Point(202, 79)
        Me.lDelay.Name = "lDelay"
        Me.lDelay.Size = New System.Drawing.Size(60, 25)
        Me.lDelay.TabIndex = 7
        Me.lDelay.Text = "Delay:"
        '
        'tbDelay
        '
        Me.tbDelay.Location = New System.Drawing.Point(205, 110)
        Me.tbDelay.Name = "tbDelay"
        Me.ValidationProvider.SetPattern(Me.tbDelay, "^(-?[1-9]+\d*|-?0)$")
        Me.tbDelay.Size = New System.Drawing.Size(70, 31)
        Me.tbDelay.TabIndex = 8
        Me.tbDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbStreamName
        '
        Me.tbStreamName.Location = New System.Drawing.Point(300, 109)
        Me.tbStreamName.Name = "tbStreamName"
        Me.tbStreamName.Size = New System.Drawing.Size(200, 31)
        Me.tbStreamName.TabIndex = 10
        '
        'tbChannels
        '
        Me.tbChannels.Location = New System.Drawing.Point(107, 109)
        Me.tbChannels.Name = "tbChannels"
        Me.ValidationProvider.SetPattern(Me.tbChannels, "^[1-9]{1}$")
        Me.tbChannels.Size = New System.Drawing.Size(70, 31)
        Me.tbChannels.TabIndex = 6
        Me.tbChannels.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'EditControl
        '
        Me.EditControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EditControl.Location = New System.Drawing.Point(12, 149)
        Me.EditControl.Name = "EditControl"
        Me.EditControl.Size = New System.Drawing.Size(974, 241)
        Me.EditControl.TabIndex = 5
        Me.EditControl.Text = "Batch Code"
        '
        'lStreamName
        '
        Me.lStreamName.AutoSize = True
        Me.lStreamName.Location = New System.Drawing.Point(294, 79)
        Me.lStreamName.Name = "lStreamName"
        Me.lStreamName.Size = New System.Drawing.Size(123, 25)
        Me.lStreamName.TabIndex = 9
        Me.lStreamName.Text = "Stream Name:"
        '
        'lChannels
        '
        Me.lChannels.AutoSize = True
        Me.lChannels.Location = New System.Drawing.Point(102, 79)
        Me.lChannels.Name = "lChannels"
        Me.lChannels.Size = New System.Drawing.Size(87, 25)
        Me.lChannels.TabIndex = 4
        Me.lChannels.Text = "Channels:"
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(886, 396)
        Me.bnCancel.Size = New System.Drawing.Size(100, 36)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(780, 396)
        Me.bnOK.Size = New System.Drawing.Size(100, 36)
        Me.bnOK.Text = "OK"
        '
        'tbType
        '
        Me.tbType.Location = New System.Drawing.Point(419, 41)
        Me.tbType.Name = "tbType"
        Me.tbType.Size = New System.Drawing.Size(88, 31)
        Me.tbType.TabIndex = 15
        '
        'mbLanguage
        '
        Me.mbLanguage.Location = New System.Drawing.Point(616, 38)
        Me.mbLanguage.ShowMenuSymbol = True
        Me.mbLanguage.Size = New System.Drawing.Size(184, 36)
        '
        'CommandLineAudioForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(998, 442)
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
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Name = "CommandLineAudioForm"
        Me.Text = "Audio Settings"
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

        EditControl.SetCommandLineDefaults()
        EditControl.Value = TempProfile.CommandLines
        EditControl.SpecialMacrosFunction = AddressOf TempProfile.SolveMacros
        EditControl.llExecute.Visible = TempProfile.File <> "" AndAlso TempProfile.SupportedInput.Contains(Filepath.GetExt(TempProfile.File))

        AddHandler EditControl.rtbEdit.TextChanged, AddressOf tbEditTextChanged

        tbStreamName.Text = TempProfile.StreamName
        tbBitrate.Text = TempProfile.Bitrate.ToString
        tbDelay.Text = TempProfile.Delay.ToString
        tbChannels.Text = TempProfile.Channels.ToString

        For Each i In Language.Languages
            If i.IsCommon Then
                mbLanguage.Add(i.ToString, i)
            Else
                mbLanguage.Add("More | " + i.ToString.Substring(0, 1).ToUpper + " | " + i.ToString, i)
            End If
        Next

        mbLanguage.Value = TempProfile.Language

        TipProvider.SetTip("Audio channels count", tbChannels, lChannels)
        TipProvider.SetTip("Display language of the track.", mbLanguage, lLanguage)
        TipProvider.SetTip("The targeted bitrate of the output file.", tbBitrate, lBitrate)
        TipProvider.SetTip("Audio delay to be fixed.", tbDelay, lDelay)
        TipProvider.SetTip("File types accepted as input, leave empty to support any file type.", tbInput, lInput)
        TipProvider.SetTip("Stream name used by the muxer. The stream name may contain macros.", tbStreamName, lStreamName)

        ActiveControl = bnOK
    End Sub

    Private Sub CommandLineAudioSettingsForm_FormClosed() Handles Me.FormClosed
        If DialogResult = DialogResult.OK Then
            Profile.SupportedInput = TempProfile.SupportedInput
            Profile.OutputFileType = TempProfile.OutputFileType
            Profile.CommandLines = TempProfile.CommandLines
            Profile.Bitrate = TempProfile.Bitrate
            Profile.StreamName = TempProfile.StreamName
            Profile.Channels = TempProfile.Channels

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

    Private Sub CommandLineAudioSettingsForm_HelpRequested() Handles Me.HelpRequested
        Dim f As New HelpForm()

        f.Doc.WriteStart(Text)
        f.Doc.WriteP("The command line audio settings define a audio conversion command line. If there is a piping symbol or line break then it's executed as batch file.")
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

        f.Doc.WriteTable("Audio Settings Macros", "The macros below are only available in the audio settings dialog and override global macros with the same name in case they are defined in both scopes.", l)
        f.Doc.WriteTable("Global Macros", "The following macros are available in various parts of StaxRip:", Macro.GetTips())
        f.Show()
    End Sub
End Class