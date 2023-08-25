
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
    Friend WithEvents tbStreamName As TextEdit
    Friend WithEvents lStreamName As LabelEx
    Friend WithEvents lInput As LabelEx
    Friend WithEvents tbChannels As TextEdit
    Friend WithEvents lChannels As LabelEx
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents tbType As TextEdit
    Friend WithEvents mbLanguage As StaxRip.UI.MenuButton
    Friend WithEvents tbProfileName As TextEdit
    Friend WithEvents laProfileName As LabelEx
    Friend WithEvents bnMenu As ButtonEx
    Friend WithEvents cms As ContextMenuStripEx
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents tlpBitrateEtcValues As TableLayoutPanel
    Friend WithEvents tlpBitrateEtcLabels As TableLayoutPanel
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanelEx
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents cbDefault As CheckBoxEx
    Friend WithEvents cbForced As CheckBoxEx
    Friend WithEvents cbCommentary As CheckBoxEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.tbInput = New StaxRip.UI.TextEdit()
        Me.lInput = New StaxRip.UI.LabelEx()
        Me.lType = New StaxRip.UI.LabelEx()
        Me.lBitrate = New StaxRip.UI.LabelEx()
        Me.tbBitrate = New StaxRip.UI.TextEdit()
        Me.lLanguage = New StaxRip.UI.LabelEx()
        Me.lDelay = New StaxRip.UI.LabelEx()
        Me.tbDelay = New StaxRip.UI.TextEdit()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.tbStreamName = New StaxRip.UI.TextEdit()
        Me.tbChannels = New StaxRip.UI.TextEdit()
        Me.ValidationProvider = New StaxRip.UI.ValidationProvider()
        Me.EditControl = New StaxRip.MacroEditorControl()
        Me.lStreamName = New StaxRip.UI.LabelEx()
        Me.lChannels = New StaxRip.UI.LabelEx()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tbType = New StaxRip.UI.TextEdit()
        Me.mbLanguage = New StaxRip.UI.MenuButton()
        Me.tbProfileName = New StaxRip.UI.TextEdit()
        Me.laProfileName = New StaxRip.UI.LabelEx()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
        Me.cms = New StaxRip.UI.ContextMenuStripEx(Me.components)
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpBitrateEtcValues = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpBitrateEtcLabels = New System.Windows.Forms.TableLayoutPanel()
        Me.FlowLayoutPanel1 = New StaxRip.UI.FlowLayoutPanelEx()
        Me.cbDefault = New StaxRip.UI.CheckBoxEx()
        Me.cbForced = New StaxRip.UI.CheckBoxEx()
        Me.cbCommentary = New StaxRip.UI.CheckBoxEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel1.SuspendLayout
        Me.tlpBitrateEtcValues.SuspendLayout
        Me.tlpBitrateEtcLabels.SuspendLayout
        Me.FlowLayoutPanel1.SuspendLayout
        Me.tlpMain.SuspendLayout
        Me.SuspendLayout
        '
        'tbInput
        '
        Me.tbInput.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tbInput.Location = New System.Drawing.Point(5, 44)
        Me.tbInput.Margin = New System.Windows.Forms.Padding(5, 2, 2, 2)
        Me.tbInput.Name = "tbInput"
        Me.tbInput.Size = New System.Drawing.Size(285, 28)
        '
        'lInput
        '
        Me.lInput.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lInput.AutoSize = true
        Me.lInput.Location = New System.Drawing.Point(2, 7)
        Me.lInput.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lInput.Size = New System.Drawing.Size(229, 25)
        Me.lInput.Text = "Supported Input File Types:"
        Me.lInput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lType
        '
        Me.lType.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lType.AutoSize = true
        Me.lType.Location = New System.Drawing.Point(294, 7)
        Me.lType.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lType.Size = New System.Drawing.Size(146, 25)
        Me.lType.Text = "Output File Type:"
        Me.lType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lBitrate
        '
        Me.lBitrate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.lBitrate.AutoSize = true
        Me.lBitrate.Location = New System.Drawing.Point(2, 7)
        Me.lBitrate.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lBitrate.Size = New System.Drawing.Size(93, 25)
        Me.lBitrate.Text = "Bitrate:"
        Me.lBitrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbBitrate
        '
        Me.tbBitrate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tbBitrate.Location = New System.Drawing.Point(5, 5)
        Me.tbBitrate.Margin = New System.Windows.Forms.Padding(5, 2, 2, 2)
        Me.tbBitrate.Name = "tbBitrate"
        Me.ValidationProvider.SetPattern(Me.tbBitrate, "^[1-9]+\d*$")
        Me.tbBitrate.Size = New System.Drawing.Size(90, 28)
        '
        'lLanguage
        '
        Me.lLanguage.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lLanguage.AutoSize = true
        Me.lLanguage.Location = New System.Drawing.Point(586, 7)
        Me.lLanguage.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lLanguage.Size = New System.Drawing.Size(93, 25)
        Me.lLanguage.Text = "Language:"
        Me.lLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lDelay
        '
        Me.lDelay.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.lDelay.AutoSize = true
        Me.lDelay.Location = New System.Drawing.Point(196, 7)
        Me.lDelay.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lDelay.Size = New System.Drawing.Size(94, 25)
        Me.lDelay.Text = "Delay:"
        Me.lDelay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbDelay
        '
        Me.tbDelay.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tbDelay.Location = New System.Drawing.Point(196, 5)
        Me.tbDelay.Margin = New System.Windows.Forms.Padding(2)
        Me.tbDelay.Name = "tbDelay"
        Me.ValidationProvider.SetPattern(Me.tbDelay, "^(-?[1-9]+\d*|-?0)$")
        Me.tbDelay.Size = New System.Drawing.Size(94, 28)
        '
        'tbStreamName
        '
        Me.tbStreamName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tbStreamName.Location = New System.Drawing.Point(294, 122)
        Me.tbStreamName.Margin = New System.Windows.Forms.Padding(2)
        Me.tbStreamName.Name = "tbStreamName"
        Me.tbStreamName.Size = New System.Drawing.Size(288, 28)
        '
        'tbChannels
        '
        Me.tbChannels.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tbChannels.Location = New System.Drawing.Point(99, 5)
        Me.tbChannels.Margin = New System.Windows.Forms.Padding(2)
        Me.tbChannels.Name = "tbChannels"
        Me.ValidationProvider.SetPattern(Me.tbChannels, "^[1-9]{1}$")
        Me.tbChannels.Size = New System.Drawing.Size(93, 28)
        '
        'EditControl
        '
        Me.EditControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.EditControl.Location = New System.Drawing.Point(8, 160)
        Me.EditControl.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.EditControl.Name = "EditControl"
        Me.EditControl.Size = New System.Drawing.Size(866, 187)
        Me.EditControl.TabIndex = 5
        Me.EditControl.Text = "Command Lines"
        '
        'lStreamName
        '
        Me.lStreamName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.lStreamName.AutoSize = true
        Me.lStreamName.Location = New System.Drawing.Point(294, 85)
        Me.lStreamName.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lStreamName.Size = New System.Drawing.Size(288, 25)
        Me.lStreamName.Text = "Track Name:"
        Me.lStreamName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lChannels
        '
        Me.lChannels.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.lChannels.AutoSize = true
        Me.lChannels.Location = New System.Drawing.Point(99, 7)
        Me.lChannels.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lChannels.Size = New System.Drawing.Size(93, 25)
        Me.lChannels.Text = "Channels:"
        Me.lChannels.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(531, 8)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(8)
        Me.bnCancel.Size = New System.Drawing.Size(125, 35)
        Me.bnCancel.Symbol = StaxRip.Symbol.None
        Me.bnCancel.Text2 = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(406, 8)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(0, 8, 0, 8)
        Me.bnOK.Size = New System.Drawing.Size(125, 35)
        Me.bnOK.Symbol = StaxRip.Symbol.None
        Me.bnOK.Text2 = "OK"
        '
        'tbType
        '
        Me.tbType.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tbType.Location = New System.Drawing.Point(294, 44)
        Me.tbType.Margin = New System.Windows.Forms.Padding(2)
        Me.tbType.Name = "tbType"
        Me.tbType.Size = New System.Drawing.Size(90, 28)
        '
        'mbLanguage
        '
        Me.mbLanguage.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.mbLanguage.Location = New System.Drawing.Point(586, 41)
        Me.mbLanguage.Margin = New System.Windows.Forms.Padding(2)
        Me.mbLanguage.Padding = New System.Windows.Forms.Padding(2, 0, 0, 0)
        Me.mbLanguage.ShowMenuSymbol = true
        Me.mbLanguage.ShowPath = false
        Me.mbLanguage.Size = New System.Drawing.Size(168, 34)
        Me.mbLanguage.Symbol = StaxRip.Symbol.None
        Me.mbLanguage.Text2 = ""
        '
        'tbProfileName
        '
        Me.tbProfileName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tbProfileName.Location = New System.Drawing.Point(586, 122)
        Me.tbProfileName.Margin = New System.Windows.Forms.Padding(2, 2, 5, 2)
        Me.tbProfileName.Name = "tbProfileName"
        Me.tbProfileName.Size = New System.Drawing.Size(287, 28)
        '
        'laProfileName
        '
        Me.laProfileName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.laProfileName.AutoSize = true
        Me.laProfileName.Location = New System.Drawing.Point(586, 85)
        Me.laProfileName.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.laProfileName.Size = New System.Drawing.Size(290, 25)
        Me.laProfileName.Text = "Name:"
        Me.laProfileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.bnMenu.ContextMenuStrip = Me.cms
        Me.bnMenu.Location = New System.Drawing.Point(340, 8)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(8)
        Me.bnMenu.ShowMenuSymbol = true
        Me.bnMenu.Size = New System.Drawing.Size(50, 35)
        Me.bnMenu.Symbol = StaxRip.Symbol.None
        Me.bnMenu.Text2 = ""
        '
        'cms
        '
        Me.cms.Font = New System.Drawing.Font("Segoe UI", 9!)
        Me.cms.ImageScalingSize = New System.Drawing.Size(48, 48)
        Me.cms.Name = "cms"
        Me.cms.Size = New System.Drawing.Size(61, 4)
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(2, 2)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(2)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(878, 156)
        Me.TableLayoutPanel1.TabIndex = 18
        '
        'tlpBitrateEtcValues
        '
        Me.tlpBitrateEtcValues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tlpBitrateEtcValues.ColumnCount = 3
        Me.tlpBitrateEtcValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpBitrateEtcValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpBitrateEtcValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpBitrateEtcValues.Controls.Add(Me.tbBitrate, 0, 0)
        Me.tlpBitrateEtcValues.Controls.Add(Me.tbChannels, 1, 0)
        Me.tlpBitrateEtcValues.Controls.Add(Me.tbDelay, 2, 0)
        Me.tlpBitrateEtcValues.Location = New System.Drawing.Point(0, 117)
        Me.tlpBitrateEtcValues.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpBitrateEtcValues.Name = "tlpBitrateEtcValues"
        Me.tlpBitrateEtcValues.RowCount = 1
        Me.tlpBitrateEtcValues.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.tlpBitrateEtcValues.Size = New System.Drawing.Size(292, 39)
        Me.tlpBitrateEtcValues.TabIndex = 20
        '
        'tlpBitrateEtcLabels
        '
        Me.tlpBitrateEtcLabels.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom)  _
            Or System.Windows.Forms.AnchorStyles.Left)  _
            Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
        Me.tlpBitrateEtcLabels.ColumnCount = 3
        Me.tlpBitrateEtcLabels.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpBitrateEtcLabels.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpBitrateEtcLabels.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpBitrateEtcLabels.Controls.Add(Me.lBitrate, 0, 0)
        Me.tlpBitrateEtcLabels.Controls.Add(Me.lChannels, 1, 0)
        Me.tlpBitrateEtcLabels.Controls.Add(Me.lDelay, 2, 0)
        Me.tlpBitrateEtcLabels.Location = New System.Drawing.Point(0, 78)
        Me.tlpBitrateEtcLabels.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpBitrateEtcLabels.Name = "tlpBitrateEtcLabels"
        Me.tlpBitrateEtcLabels.RowCount = 1
        Me.tlpBitrateEtcLabels.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.tlpBitrateEtcLabels.Size = New System.Drawing.Size(292, 39)
        Me.tlpBitrateEtcLabels.TabIndex = 19
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.FlowLayoutPanel1.AutomaticOffset = false
        Me.FlowLayoutPanel1.AutoSize = true
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Controls.Add(Me.cbDefault)
        Me.FlowLayoutPanel1.Controls.Add(Me.cbForced)
        Me.FlowLayoutPanel1.Controls.Add(Me.cbCommentary)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnMenu)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnOK)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnCancel)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(210, 347)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(672, 51)
        Me.FlowLayoutPanel1.TabIndex = 19
        Me.FlowLayoutPanel1.WrapContents = false
        '
        'cbDefault
        '
        Me.cbDefault.AutoSize = true
        Me.cbDefault.Location = New System.Drawing.Point(2, 12)
        Me.cbDefault.Margin = New System.Windows.Forms.Padding(2)
        Me.cbDefault.Size = New System.Drawing.Size(95, 29)
        Me.cbDefault.Text = "Default"
        Me.cbDefault.UseVisualStyleBackColor = false
        '
        'cbForced
        '
        Me.cbForced.AutoSize = true
        Me.cbForced.Location = New System.Drawing.Point(99, 12)
        Me.cbForced.Margin = New System.Windows.Forms.Padding(2)
        Me.cbForced.Size = New System.Drawing.Size(92, 29)
        Me.cbForced.Text = "Forced"
        Me.cbForced.UseVisualStyleBackColor = false
        '
        'cbCommentary
        '
        Me.cbCommentary.AutoSize = true
        Me.cbCommentary.Location = New System.Drawing.Point(195, 12)
        Me.cbCommentary.Margin = New System.Windows.Forms.Padding(2)
        Me.cbCommentary.Size = New System.Drawing.Size(141, 29)
        Me.cbCommentary.Text = "Commentary"
        Me.cbCommentary.UseVisualStyleBackColor = false
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.tlpMain.Controls.Add(Me.FlowLayoutPanel1, 0, 2)
        Me.tlpMain.Controls.Add(Me.TableLayoutPanel1, 0, 0)
        Me.tlpMain.Controls.Add(Me.EditControl, 0, 1)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(2)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(882, 398)
        Me.tlpMain.TabIndex = 20
        '
        'CommandLineAudioEncoderForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144!, 144!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(882, 398)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = true
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "CommandLineAudioEncoderForm"
        Me.Text = "Audio Command Lines"
        Me.TableLayoutPanel1.ResumeLayout(false)
        Me.TableLayoutPanel1.PerformLayout
        Me.tlpBitrateEtcValues.ResumeLayout(false)
        Me.tlpBitrateEtcLabels.ResumeLayout(false)
        Me.tlpBitrateEtcLabels.PerformLayout
        Me.FlowLayoutPanel1.ResumeLayout(false)
        Me.FlowLayoutPanel1.PerformLayout
        Me.tlpMain.ResumeLayout(false)
        Me.tlpMain.PerformLayout
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents tbInput As TextEdit
    Friend WithEvents Label4 As LabelEx
    Friend WithEvents lType As LabelEx
    Friend WithEvents lBitrate As LabelEx
    Friend WithEvents tbBitrate As TextEdit
    Friend WithEvents lLanguage As LabelEx
    Friend WithEvents lDelay As LabelEx
    Friend WithEvents tbDelay As TextEdit
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
        cbCommentary.Checked = TempProfile.Commentary

        cbDefault.Margin = New Padding(CInt(FontHeight / 2))
        cbForced.Margin = New Padding(CInt(FontHeight / 2))
        cbCommentary.Margin = New Padding(CInt(FontHeight / 2))

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
        cms.Add("Show Command Line...", Sub() g.ShowCommandLinePreview("Command Lines", TempProfile.GetCode, False))
        cms.Add("Save Profile...", AddressOf SaveProfile, "Saves the current settings as profile").SetImage(Symbol.Save)
        cms.Add("Help", AddressOf ShowHelp).SetImage(Symbol.Help)

        ActiveControl = bnOK
        ApplyTheme()

        AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
    End Sub

    Sub OnThemeChanged(theme As Theme)
        ApplyTheme(theme)
    End Sub

    Sub ApplyTheme()
        ApplyTheme(ThemeManager.CurrentTheme)
    End Sub

    Sub ApplyTheme(theme As Theme)
        If DesignHelp.IsDesignMode Then
            Exit Sub
        End If

        BackColor = theme.General.BackColor
    End Sub

    Sub SaveProfile()
        Dim gap = ObjectHelp.GetCopy(TempProfile)
        Dim name = InputBox.Show("Enter the profile name", gap.Name)

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
            Profile.Commentary = TempProfile.Commentary
        End If
    End Sub

    Sub tbStreamName_TextChanged() Handles tbStreamName.TextChanged
        TempProfile.StreamName = tbStreamName.Text
        EditControl.UpdatePreview()
    End Sub

    Sub tbInput_TextChanged() Handles tbInput.TextChanged
        TempProfile.SupportedInput = tbInput.Text.ToLowerInvariant.SplitNoEmptyAndWhiteSpace(",", ";", " ")
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

        Dim macroList As New StringPairList From {
            {"%input%", "Audio source file"},
            {"%output%", "Audio target File"},
            {"%bitrate%", "Audio bitrate"},
            {"%delay%", "Audio delay"},
            {"%channels%", "Audio channels count"},
            {"%language_native%", "Native language name"},
            {"%language_english%", "English language name"},
            {"%streamid0%", "ID of the stream (starts with 0)"},
            {"%streamid1%", "ID of the stream (starts with 1)"}
        }

        form.Doc.WriteTable("Command Line Audio Macros",
                            "The following macros are available in the command line audio dialog and override global macros with the same name.",
                            macroList)

        form.Doc.WriteTable("Global Macros", "Global macros are passed to the process as environment variables.",
                            Macro.GetTips(False, True))
        form.Show()
    End Sub

    Sub CommandLineAudioEncoderForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Refresh()

        For Each i In Language.Languages
            If i.IsCommon Then
                mbLanguage.Add(i.ToString, i)
            Else
                mbLanguage.Add("More | " + i.ToString.Substring(0, 1).ToUpperInvariant + " | " + i.ToString, i)
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

    Sub cbCommentary_CheckedChanged(sender As Object, e As EventArgs) Handles cbCommentary.CheckedChanged
        TempProfile.Commentary = cbCommentary.Checked
        EditControl.UpdatePreview()
    End Sub

    Sub CommandLineAudioEncoderForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        ShowHelp()
    End Sub
End Class
