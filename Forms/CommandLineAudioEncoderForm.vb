
Imports StaxRip.UI

Public Class CommandLineAudioEncoderForm
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
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents tlpBitrateEtcValues As TableLayoutPanel
    Friend WithEvents tlpBitrateEtcLabels As TableLayoutPanel
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents cbDefault As CheckBox
    Friend WithEvents cbForced As CheckBox
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpBitrateEtcValues = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpBitrateEtcLabels = New System.Windows.Forms.TableLayoutPanel()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.cbDefault = New System.Windows.Forms.CheckBox()
        Me.cbForced = New System.Windows.Forms.CheckBox()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.tlpBitrateEtcValues.SuspendLayout()
        Me.tlpBitrateEtcLabels.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbInput
        '
        Me.tbInput.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbInput.Location = New System.Drawing.Point(10, 89)
        Me.tbInput.Margin = New System.Windows.Forms.Padding(10, 5, 5, 5)
        Me.tbInput.Name = "tbInput"
        Me.tbInput.Size = New System.Drawing.Size(570, 55)
        Me.tbInput.TabIndex = 1
        '
        'lInput
        '
        Me.lInput.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lInput.AutoSize = True
        Me.lInput.Location = New System.Drawing.Point(5, 15)
        Me.lInput.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lInput.Name = "lInput"
        Me.lInput.Size = New System.Drawing.Size(452, 48)
        Me.lInput.TabIndex = 0
        Me.lInput.Text = "Supported Input File Types:"
        '
        'lType
        '
        Me.lType.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lType.AutoSize = True
        Me.lType.Location = New System.Drawing.Point(590, 15)
        Me.lType.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lType.Name = "lType"
        Me.lType.Size = New System.Drawing.Size(289, 48)
        Me.lType.TabIndex = 11
        Me.lType.Text = "Output File Type:"
        '
        'lBitrate
        '
        Me.lBitrate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lBitrate.AutoSize = True
        Me.lBitrate.Location = New System.Drawing.Point(5, 15)
        Me.lBitrate.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lBitrate.Name = "lBitrate"
        Me.lBitrate.Size = New System.Drawing.Size(185, 48)
        Me.lBitrate.TabIndex = 2
        Me.lBitrate.Text = "Bitrate:"
        '
        'tbBitrate
        '
        Me.tbBitrate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbBitrate.Location = New System.Drawing.Point(10, 12)
        Me.tbBitrate.Margin = New System.Windows.Forms.Padding(10, 5, 5, 5)
        Me.tbBitrate.Name = "tbBitrate"
        Me.ValidationProvider.SetPattern(Me.tbBitrate, "^[1-9]+\d*$")
        Me.tbBitrate.Size = New System.Drawing.Size(180, 55)
        Me.tbBitrate.TabIndex = 3
        Me.tbBitrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lLanguage
        '
        Me.lLanguage.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lLanguage.AutoSize = True
        Me.lLanguage.Location = New System.Drawing.Point(1176, 15)
        Me.lLanguage.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lLanguage.Name = "lLanguage"
        Me.lLanguage.Size = New System.Drawing.Size(182, 48)
        Me.lLanguage.TabIndex = 13
        Me.lLanguage.Text = "Language:"
        '
        'lDelay
        '
        Me.lDelay.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lDelay.AutoSize = True
        Me.lDelay.Location = New System.Drawing.Point(395, 15)
        Me.lDelay.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lDelay.Name = "lDelay"
        Me.lDelay.Size = New System.Drawing.Size(185, 48)
        Me.lDelay.TabIndex = 7
        Me.lDelay.Text = "Delay:"
        '
        'tbDelay
        '
        Me.tbDelay.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDelay.Location = New System.Drawing.Point(395, 12)
        Me.tbDelay.Margin = New System.Windows.Forms.Padding(5)
        Me.tbDelay.Name = "tbDelay"
        Me.ValidationProvider.SetPattern(Me.tbDelay, "^(-?[1-9]+\d*|-?0)$")
        Me.tbDelay.Size = New System.Drawing.Size(185, 55)
        Me.tbDelay.TabIndex = 8
        Me.tbDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbStreamName
        '
        Me.tbStreamName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbStreamName.Location = New System.Drawing.Point(590, 246)
        Me.tbStreamName.Margin = New System.Windows.Forms.Padding(5)
        Me.tbStreamName.Name = "tbStreamName"
        Me.tbStreamName.Size = New System.Drawing.Size(576, 55)
        Me.tbStreamName.TabIndex = 10
        '
        'tbChannels
        '
        Me.tbChannels.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbChannels.Location = New System.Drawing.Point(200, 12)
        Me.tbChannels.Margin = New System.Windows.Forms.Padding(5)
        Me.tbChannels.Name = "tbChannels"
        Me.ValidationProvider.SetPattern(Me.tbChannels, "^[1-9]{1}$")
        Me.tbChannels.Size = New System.Drawing.Size(185, 55)
        Me.tbChannels.TabIndex = 6
        Me.tbChannels.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'EditControl
        '
        Me.EditControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EditControl.Location = New System.Drawing.Point(15, 320)
        Me.EditControl.Margin = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.EditControl.Name = "EditControl"
        Me.EditControl.Size = New System.Drawing.Size(1734, 377)
        Me.EditControl.TabIndex = 5
        Me.EditControl.Text = "Command Lines"
        '
        'lStreamName
        '
        Me.lStreamName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lStreamName.AutoSize = True
        Me.lStreamName.Location = New System.Drawing.Point(590, 171)
        Me.lStreamName.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lStreamName.Name = "lStreamName"
        Me.lStreamName.Size = New System.Drawing.Size(576, 48)
        Me.lStreamName.TabIndex = 9
        Me.lStreamName.Text = "Track Name:"
        '
        'lChannels
        '
        Me.lChannels.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lChannels.AutoSize = True
        Me.lChannels.Location = New System.Drawing.Point(200, 15)
        Me.lChannels.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lChannels.Name = "lChannels"
        Me.lChannels.Size = New System.Drawing.Size(185, 48)
        Me.lChannels.TabIndex = 4
        Me.lChannels.Text = "Channels:"
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(762, 15)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(15)
        Me.bnCancel.Size = New System.Drawing.Size(250, 70)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(497, 15)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(0, 15, 0, 15)
        Me.bnOK.Size = New System.Drawing.Size(250, 70)
        Me.bnOK.Text = "OK"
        '
        'tbType
        '
        Me.tbType.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tbType.Location = New System.Drawing.Point(590, 89)
        Me.tbType.Margin = New System.Windows.Forms.Padding(5)
        Me.tbType.Name = "tbType"
        Me.tbType.Size = New System.Drawing.Size(180, 55)
        Me.tbType.TabIndex = 15
        '
        'mbLanguage
        '
        Me.mbLanguage.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.mbLanguage.Location = New System.Drawing.Point(1176, 83)
        Me.mbLanguage.Margin = New System.Windows.Forms.Padding(5)
        Me.mbLanguage.ShowMenuSymbol = True
        Me.mbLanguage.Size = New System.Drawing.Size(335, 68)
        '
        'tbProfileName
        '
        Me.tbProfileName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbProfileName.Location = New System.Drawing.Point(1176, 246)
        Me.tbProfileName.Margin = New System.Windows.Forms.Padding(5, 5, 10, 5)
        Me.tbProfileName.Size = New System.Drawing.Size(572, 55)
        '
        'laProfileName
        '
        Me.laProfileName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.laProfileName.AutoSize = True
        Me.laProfileName.Location = New System.Drawing.Point(1174, 171)
        Me.laProfileName.Size = New System.Drawing.Size(581, 48)
        Me.laProfileName.Text = "Name:"
        Me.laProfileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnMenu.ContextMenuStrip = Me.cms
        Me.bnMenu.Location = New System.Drawing.Point(382, 15)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(15)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(100, 70)
        '
        'cms
        '
        Me.cms.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cms.ImageScalingSize = New System.Drawing.Size(48, 48)
        Me.cms.Name = "cms"
        Me.cms.Size = New System.Drawing.Size(61, 4)
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
        Me.TableLayoutPanel1.Controls.Add(Me.tlpBitrateEtcValues, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.tlpBitrateEtcLabels, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lInput, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.tbProfileName, 2, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.laProfileName, 2, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lType, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lLanguage, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.tbStreamName, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.lStreamName, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.tbType, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.mbLanguage, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.tbInput, 0, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1758, 314)
        Me.TableLayoutPanel1.TabIndex = 18
        '
        'tlpBitrateEtcValues
        '
        Me.tlpBitrateEtcValues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpBitrateEtcValues.ColumnCount = 3
        Me.tlpBitrateEtcValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpBitrateEtcValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpBitrateEtcValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpBitrateEtcValues.Controls.Add(Me.tbBitrate, 0, 0)
        Me.tlpBitrateEtcValues.Controls.Add(Me.tbChannels, 1, 0)
        Me.tlpBitrateEtcValues.Controls.Add(Me.tbDelay, 2, 0)
        Me.tlpBitrateEtcValues.Location = New System.Drawing.Point(0, 234)
        Me.tlpBitrateEtcValues.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpBitrateEtcValues.Name = "tlpBitrateEtcValues"
        Me.tlpBitrateEtcValues.RowCount = 1
        Me.tlpBitrateEtcValues.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBitrateEtcValues.Size = New System.Drawing.Size(585, 80)
        Me.tlpBitrateEtcValues.TabIndex = 20
        '
        'tlpBitrateEtcLabels
        '
        Me.tlpBitrateEtcLabels.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpBitrateEtcLabels.ColumnCount = 3
        Me.tlpBitrateEtcLabels.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpBitrateEtcLabels.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpBitrateEtcLabels.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpBitrateEtcLabels.Controls.Add(Me.lBitrate, 0, 0)
        Me.tlpBitrateEtcLabels.Controls.Add(Me.lChannels, 1, 0)
        Me.tlpBitrateEtcLabels.Controls.Add(Me.lDelay, 2, 0)
        Me.tlpBitrateEtcLabels.Location = New System.Drawing.Point(0, 156)
        Me.tlpBitrateEtcLabels.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpBitrateEtcLabels.Name = "tlpBitrateEtcLabels"
        Me.tlpBitrateEtcLabels.RowCount = 1
        Me.tlpBitrateEtcLabels.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBitrateEtcLabels.Size = New System.Drawing.Size(585, 78)
        Me.tlpBitrateEtcLabels.TabIndex = 19
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Controls.Add(Me.cbDefault)
        Me.FlowLayoutPanel1.Controls.Add(Me.cbForced)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnMenu)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnOK)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnCancel)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(737, 697)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(1027, 100)
        Me.FlowLayoutPanel1.TabIndex = 19
        '
        'cbDefault
        '
        Me.cbDefault.AutoSize = True
        Me.cbDefault.Location = New System.Drawing.Point(3, 3)
        Me.cbDefault.Name = "cbDefault"
        Me.cbDefault.Size = New System.Drawing.Size(180, 52)
        Me.cbDefault.TabIndex = 3
        Me.cbDefault.Text = "Default"
        Me.cbDefault.UseVisualStyleBackColor = True
        '
        'cbForced
        '
        Me.cbForced.AutoSize = True
        Me.cbForced.Location = New System.Drawing.Point(189, 3)
        Me.cbForced.Name = "cbForced"
        Me.cbForced.Size = New System.Drawing.Size(175, 52)
        Me.cbForced.TabIndex = 4
        Me.cbForced.Text = "Forced"
        Me.cbForced.UseVisualStyleBackColor = True
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.FlowLayoutPanel1, 0, 2)
        Me.tlpMain.Controls.Add(Me.TableLayoutPanel1, 0, 0)
        Me.tlpMain.Controls.Add(Me.EditControl, 0, 1)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 320.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1764, 797)
        Me.tlpMain.TabIndex = 20
        '
        'CommandLineAudioEncoderForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1764, 797)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(11, 10, 11, 10)
        Me.Name = "CommandLineAudioEncoderForm"
        Me.Text = "Audio Command Lines"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.tlpBitrateEtcValues.ResumeLayout(False)
        Me.tlpBitrateEtcValues.PerformLayout()
        Me.tlpBitrateEtcLabels.ResumeLayout(False)
        Me.tlpBitrateEtcLabels.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.ResumeLayout(False)

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

        If TempProfile.Name <> TempProfile.DefaultName Then
            tbProfileName.Text = TempProfile.Name
        End If

        tbProfileName.SendMessageCue(TempProfile.Name, False)

        EditControl.SetCommandLineDefaults()
        EditControl.Value = TempProfile.CommandLines
        EditControl.SpecialMacrosFunction = AddressOf TempProfile.ExpandMacros

        AddHandler EditControl.rtbEdit.TextChanged, AddressOf tbEditTextChanged

        tbStreamName.Text = TempProfile.StreamName
        tbBitrate.Text = TempProfile.Bitrate.ToString
        tbDelay.Text = TempProfile.Delay.ToString
        tbChannels.Text = TempProfile.Channels.ToString
        cbDefault.Checked = TempProfile.Default
        cbForced.Checked = TempProfile.Forced

        cbDefault.Margin = New Padding(CInt(FontHeight / 2))
        cbForced.Margin = New Padding(CInt(FontHeight / 2))

        TipProvider.SetTip("Audio channels count. Macro: %channels%", tbChannels, lChannels)
        TipProvider.SetTip("Display language of the track. Macros: %language_native%, %language_english%", mbLanguage, lLanguage)
        TipProvider.SetTip("The targeted bitrate of the output file. Macro: %bitrate%", tbBitrate, lBitrate)
        TipProvider.SetTip("Audio delay to be fixed. Macro: %delay%", tbDelay, lDelay)
        TipProvider.SetTip("File types accepted as input, leave empty to support any file type.", tbInput, lInput)
        TipProvider.SetTip("Track name used by the muxer. The track name may contain macros.", tbStreamName, lStreamName)
        TipProvider.SetTip("If no name is defined StaxRip auto generate the name.", laProfileName, tbProfileName)
        TipProvider.SetTip("Default MKV Track.", cbDefault)
        TipProvider.SetTip("Forced MKV Track.", cbForced)

        cms.Add("Copy Command Line", Sub() Clipboard.SetText(TempProfile.GetCode))
        cms.Add("Show Command Line...", Sub() g.ShowCommandLinePreview("Command Lines", TempProfile.GetCode))
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

    Sub CommandLineAudioSettingsForm_FormClosed() Handles Me.FormClosed
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
            Profile.Default = TempProfile.Default
            Profile.Forced = TempProfile.Forced
        End If
    End Sub

    Sub tbStreamName_TextChanged() Handles tbStreamName.TextChanged
        TempProfile.StreamName = tbStreamName.Text
        EditControl.UpdatePreview()
    End Sub

    Sub tbInput_TextChanged() Handles tbInput.TextChanged
        TempProfile.SupportedInput = tbInput.Text.ToLower.SplitNoEmptyAndWhiteSpace(",", ";", " ")
        EditControl.UpdatePreview()
    End Sub

    Sub tbType_TextChanged() Handles tbType.TextChanged
        TempProfile.OutputFileType = tbType.Text
        EditControl.UpdatePreview()
    End Sub

    Sub tbBitrate_TextChanged() Handles tbBitrate.TextChanged
        TempProfile.Bitrate = tbBitrate.Text.ToInt
        EditControl.UpdatePreview()
    End Sub

    Sub tbChannels_TextChanged() Handles tbChannels.TextChanged
        TempProfile.Channels = tbChannels.Text.ToInt
        EditControl.UpdatePreview()
    End Sub

    Sub mbLanguage_ValueChangedUser(value As Object) Handles mbLanguage.ValueChangedUser
        TempProfile.Language = DirectCast(mbLanguage.Value, Language)
        EditControl.UpdatePreview()
    End Sub

    Sub tbEditTextChanged(sender As Object, args As EventArgs)
        TempProfile.CommandLines = EditControl.rtbEdit.Text
        EditControl.UpdatePreview()
    End Sub

    Sub tbDelay_TextChanged() Handles tbDelay.TextChanged
        TempProfile.Delay = tbDelay.Text.ToInt
        EditControl.UpdatePreview()
    End Sub

    Sub tbProfileName_TextChanged(sender As Object, e As EventArgs) Handles tbProfileName.TextChanged
        TempProfile.Name = tbProfileName.Text
    End Sub

    Sub ShowHelp()
        Dim form As New HelpForm()

        form.Doc.WriteStart(Text)
        form.Doc.WriteParagraph("Each line is executed separately. Global macros are passed to the process as environment variables.")
        form.Doc.WriteH2("Options")
        form.Doc.WriteTips(TipProvider.GetTips, EditControl.TipProvider.GetTips)

        Dim macroList As New StringPairList

        macroList.Add("%input%", "Audio source file")
        macroList.Add("%output%", "Audio target File")
        macroList.Add("%bitrate%", "Audio bitrate")
        macroList.Add("%delay%", "Audio delay")
        macroList.Add("%channels%", "Audio channels count")
        macroList.Add("%language_native%", "Native language name")
        macroList.Add("%language_english%", "English language name")

        form.Doc.WriteTable("Command Line Audio Macros",
                            "The following macros are available in the command line audio dialog and override global macros with the same name.",
                            macroList)

        form.Doc.WriteTable("Global Macros", "Global macros are passed to the process as environment variables.",
                            Macro.GetTips(False, True, False))
        form.Show()
    End Sub

    Sub CommandLineAudioEncoderForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
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

    Sub cbDefault_CheckedChanged(sender As Object, e As EventArgs) Handles cbDefault.CheckedChanged
        TempProfile.Default = cbDefault.Checked
        EditControl.UpdatePreview()
    End Sub

    Sub cbForced_CheckedChanged(sender As Object, e As EventArgs) Handles cbForced.CheckedChanged
        TempProfile.Forced = cbForced.Checked
        EditControl.UpdatePreview()
    End Sub

    Sub CommandLineAudioEncoderForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        ShowHelp()
    End Sub
End Class
