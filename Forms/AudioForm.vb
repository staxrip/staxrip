Imports StaxRip.UI

Class AudioForm
    Inherits DialogBase

#Region " Designer "

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Friend WithEvents CommandLink1 As StaxRip.UI.CommandLink
    Friend WithEvents gbBasic As System.Windows.Forms.GroupBox
    Friend WithEvents nudQuality As NumEdit
    Friend WithEvents numBitrate As NumEdit
    Friend WithEvents lQualiy As System.Windows.Forms.Label
    Friend WithEvents lCodec As System.Windows.Forms.Label
    Friend WithEvents mbCodec As StaxRip.UI.MenuButton
    Friend WithEvents mbLanguage As StaxRip.UI.MenuButton
    Friend WithEvents lLanguage As System.Windows.Forms.Label
    Friend WithEvents numDelay As NumEdit
    Friend WithEvents lDelay As System.Windows.Forms.Label
    Friend WithEvents rtbCommandLine As CommandLineRichTextBox
    Friend WithEvents mbChannels As StaxRip.UI.MenuButton
    Friend WithEvents lChannels As System.Windows.Forms.Label
    Friend WithEvents gbAdvanced As System.Windows.Forms.GroupBox
    Friend WithEvents lName As System.Windows.Forms.Label
    Friend WithEvents tbName As System.Windows.Forms.TextBox
    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents mbSamplingRate As StaxRip.UI.MenuButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents mbEncoder As StaxRip.UI.MenuButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cbNormalize As StaxRip.UI.CheckBoxEx
    Friend WithEvents SimpleUI As StaxRip.SimpleUI
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents numGain As StaxRip.UI.NumEdit
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents FlowLayoutPanel2 As FlowLayoutPanel
    Friend WithEvents tlpBasic As TableLayoutPanel
    Friend WithEvents bnMenu As ButtonEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.gbBasic = New System.Windows.Forms.GroupBox()
        Me.tlpBasic = New System.Windows.Forms.TableLayoutPanel()
        Me.lCodec = New System.Windows.Forms.Label()
        Me.tbName = New System.Windows.Forms.TextBox()
        Me.lName = New System.Windows.Forms.Label()
        Me.numDelay = New StaxRip.UI.NumEdit()
        Me.numGain = New StaxRip.UI.NumEdit()
        Me.mbCodec = New StaxRip.UI.MenuButton()
        Me.mbLanguage = New StaxRip.UI.MenuButton()
        Me.mbSamplingRate = New StaxRip.UI.MenuButton()
        Me.lLanguage = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lDelay = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.numBitrate = New StaxRip.UI.NumEdit()
        Me.nudQuality = New StaxRip.UI.NumEdit()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.mbEncoder = New StaxRip.UI.MenuButton()
        Me.lQualiy = New System.Windows.Forms.Label()
        Me.lChannels = New System.Windows.Forms.Label()
        Me.mbChannels = New StaxRip.UI.MenuButton()
        Me.cbNormalize = New StaxRip.UI.CheckBoxEx()
        Me.rtbCommandLine = New StaxRip.UI.CommandLineRichTextBox()
        Me.gbAdvanced = New System.Windows.Forms.GroupBox()
        Me.SimpleUI = New StaxRip.SimpleUI()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
        Me.gbBasic.SuspendLayout()
        Me.tlpBasic.SuspendLayout()
        Me.gbAdvanced.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbBasic
        '
        Me.gbBasic.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbBasic.Controls.Add(Me.tlpBasic)
        Me.gbBasic.Location = New System.Drawing.Point(15, 14)
        Me.gbBasic.Margin = New System.Windows.Forms.Padding(15, 14, 7, 14)
        Me.gbBasic.Name = "gbBasic"
        Me.gbBasic.Padding = New System.Windows.Forms.Padding(5)
        Me.gbBasic.Size = New System.Drawing.Size(850, 737)
        Me.gbBasic.TabIndex = 1
        Me.gbBasic.TabStop = False
        Me.gbBasic.Text = "Basic"
        '
        'tlpBasic
        '
        Me.tlpBasic.ColumnCount = 4
        Me.tlpBasic.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpBasic.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.tlpBasic.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpBasic.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.tlpBasic.Controls.Add(Me.lCodec, 0, 0)
        Me.tlpBasic.Controls.Add(Me.tbName, 1, 6)
        Me.tlpBasic.Controls.Add(Me.lName, 0, 6)
        Me.tlpBasic.Controls.Add(Me.numDelay, 3, 3)
        Me.tlpBasic.Controls.Add(Me.numGain, 3, 2)
        Me.tlpBasic.Controls.Add(Me.mbCodec, 1, 0)
        Me.tlpBasic.Controls.Add(Me.mbLanguage, 1, 4)
        Me.tlpBasic.Controls.Add(Me.mbSamplingRate, 1, 3)
        Me.tlpBasic.Controls.Add(Me.lLanguage, 0, 4)
        Me.tlpBasic.Controls.Add(Me.Label4, 2, 2)
        Me.tlpBasic.Controls.Add(Me.lDelay, 2, 3)
        Me.tlpBasic.Controls.Add(Me.Label1, 0, 3)
        Me.tlpBasic.Controls.Add(Me.Label3, 2, 0)
        Me.tlpBasic.Controls.Add(Me.numBitrate, 3, 0)
        Me.tlpBasic.Controls.Add(Me.nudQuality, 3, 1)
        Me.tlpBasic.Controls.Add(Me.Label2, 0, 1)
        Me.tlpBasic.Controls.Add(Me.mbEncoder, 1, 1)
        Me.tlpBasic.Controls.Add(Me.lQualiy, 2, 1)
        Me.tlpBasic.Controls.Add(Me.lChannels, 0, 2)
        Me.tlpBasic.Controls.Add(Me.mbChannels, 1, 2)
        Me.tlpBasic.Controls.Add(Me.cbNormalize, 2, 4)
        Me.tlpBasic.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpBasic.Location = New System.Drawing.Point(5, 53)
        Me.tlpBasic.Margin = New System.Windows.Forms.Padding(5)
        Me.tlpBasic.Name = "tlpBasic"
        Me.tlpBasic.RowCount = 8
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34.0!))
        Me.tlpBasic.Size = New System.Drawing.Size(840, 679)
        Me.tlpBasic.TabIndex = 44
        '
        'lCodec
        '
        Me.lCodec.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lCodec.AutoSize = True
        Me.lCodec.Location = New System.Drawing.Point(5, 16)
        Me.lCodec.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lCodec.Name = "lCodec"
        Me.lCodec.Size = New System.Drawing.Size(128, 48)
        Me.lCodec.TabIndex = 0
        Me.lCodec.Text = "Codec:"
        '
        'tbName
        '
        Me.tbName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpBasic.SetColumnSpan(Me.tbName, 3)
        Me.tbName.Location = New System.Drawing.Point(241, 405)
        Me.tbName.Margin = New System.Windows.Forms.Padding(5)
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(594, 55)
        Me.tbName.TabIndex = 16
        '
        'lName
        '
        Me.lName.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lName.AutoSize = True
        Me.lName.Location = New System.Drawing.Point(5, 408)
        Me.lName.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lName.Name = "lName"
        Me.lName.Size = New System.Drawing.Size(123, 48)
        Me.lName.TabIndex = 15
        Me.lName.Text = "Name:"
        '
        'numDelay
        '
        Me.numDelay.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numDelay.Location = New System.Drawing.Point(663, 245)
        Me.numDelay.Margin = New System.Windows.Forms.Padding(5)
        Me.numDelay.Name = "numDelay"
        Me.numDelay.Size = New System.Drawing.Size(172, 70)
        Me.numDelay.TabIndex = 19
        '
        'numGain
        '
        Me.numGain.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numGain.Location = New System.Drawing.Point(663, 165)
        Me.numGain.Margin = New System.Windows.Forms.Padding(5)
        Me.numGain.Name = "numGain"
        Me.numGain.Size = New System.Drawing.Size(172, 70)
        Me.numGain.TabIndex = 37
        '
        'mbCodec
        '
        Me.mbCodec.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbCodec.Location = New System.Drawing.Point(241, 5)
        Me.mbCodec.Margin = New System.Windows.Forms.Padding(5)
        Me.mbCodec.ShowMenuSymbol = True
        Me.mbCodec.Size = New System.Drawing.Size(262, 70)
        Me.mbCodec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'mbLanguage
        '
        Me.mbLanguage.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbLanguage.Location = New System.Drawing.Point(241, 325)
        Me.mbLanguage.Margin = New System.Windows.Forms.Padding(5)
        Me.mbLanguage.ShowMenuSymbol = True
        Me.mbLanguage.Size = New System.Drawing.Size(262, 70)
        Me.mbLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'mbSamplingRate
        '
        Me.mbSamplingRate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbSamplingRate.Location = New System.Drawing.Point(241, 245)
        Me.mbSamplingRate.Margin = New System.Windows.Forms.Padding(5)
        Me.mbSamplingRate.ShowMenuSymbol = True
        Me.mbSamplingRate.Size = New System.Drawing.Size(262, 70)
        Me.mbSamplingRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lLanguage
        '
        Me.lLanguage.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lLanguage.AutoSize = True
        Me.lLanguage.Location = New System.Drawing.Point(5, 336)
        Me.lLanguage.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lLanguage.Name = "lLanguage"
        Me.lLanguage.Size = New System.Drawing.Size(182, 48)
        Me.lLanguage.TabIndex = 10
        Me.lLanguage.Text = "Language:"
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(513, 176)
        Me.Label4.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 48)
        Me.Label4.TabIndex = 36
        Me.Label4.TabStop = True
        Me.Label4.Text = "Gain:"
        '
        'lDelay
        '
        Me.lDelay.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lDelay.AutoSize = True
        Me.lDelay.Location = New System.Drawing.Point(513, 256)
        Me.lDelay.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lDelay.Name = "lDelay"
        Me.lDelay.Size = New System.Drawing.Size(116, 48)
        Me.lDelay.TabIndex = 14
        Me.lDelay.TabStop = True
        Me.lDelay.Text = "Delay:"
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 256)
        Me.Label1.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(226, 48)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Sample Rate:"
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(513, 16)
        Me.Label3.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(132, 48)
        Me.Label3.TabIndex = 29
        Me.Label3.TabStop = True
        Me.Label3.Text = "Bitrate:"
        '
        'numBitrate
        '
        Me.numBitrate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numBitrate.Location = New System.Drawing.Point(663, 5)
        Me.numBitrate.Margin = New System.Windows.Forms.Padding(5)
        Me.numBitrate.Name = "numBitrate"
        Me.numBitrate.Size = New System.Drawing.Size(172, 70)
        Me.numBitrate.TabIndex = 17
        '
        'nudQuality
        '
        Me.nudQuality.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudQuality.Location = New System.Drawing.Point(663, 85)
        Me.nudQuality.Margin = New System.Windows.Forms.Padding(5)
        Me.nudQuality.Name = "nudQuality"
        Me.nudQuality.Size = New System.Drawing.Size(172, 70)
        Me.nudQuality.TabIndex = 18
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 96)
        Me.Label2.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(157, 48)
        Me.Label2.TabIndex = 24
        Me.Label2.Text = "Encoder:"
        '
        'mbEncoder
        '
        Me.mbEncoder.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbEncoder.Location = New System.Drawing.Point(241, 85)
        Me.mbEncoder.Margin = New System.Windows.Forms.Padding(5)
        Me.mbEncoder.ShowMenuSymbol = True
        Me.mbEncoder.Size = New System.Drawing.Size(262, 70)
        Me.mbEncoder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lQualiy
        '
        Me.lQualiy.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lQualiy.AutoSize = True
        Me.lQualiy.Location = New System.Drawing.Point(513, 96)
        Me.lQualiy.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lQualiy.Name = "lQualiy"
        Me.lQualiy.Size = New System.Drawing.Size(140, 48)
        Me.lQualiy.TabIndex = 12
        Me.lQualiy.TabStop = True
        Me.lQualiy.Text = "Quality:"
        '
        'lChannels
        '
        Me.lChannels.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lChannels.AutoSize = True
        Me.lChannels.Location = New System.Drawing.Point(5, 176)
        Me.lChannels.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lChannels.Name = "lChannels"
        Me.lChannels.Size = New System.Drawing.Size(171, 48)
        Me.lChannels.TabIndex = 5
        Me.lChannels.Text = "Channels:"
        '
        'mbChannels
        '
        Me.mbChannels.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbChannels.Location = New System.Drawing.Point(241, 165)
        Me.mbChannels.Margin = New System.Windows.Forms.Padding(5)
        Me.mbChannels.ShowMenuSymbol = True
        Me.mbChannels.Size = New System.Drawing.Size(262, 70)
        Me.mbChannels.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbNormalize
        '
        Me.cbNormalize.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cbNormalize.AutoSize = True
        Me.tlpBasic.SetColumnSpan(Me.cbNormalize, 2)
        Me.cbNormalize.Location = New System.Drawing.Point(523, 334)
        Me.cbNormalize.Margin = New System.Windows.Forms.Padding(15, 5, 5, 5)
        Me.cbNormalize.Size = New System.Drawing.Size(229, 52)
        Me.cbNormalize.Text = "Normalize"
        Me.cbNormalize.UseVisualStyleBackColor = True
        '
        'rtbCommandLine
        '
        Me.rtbCommandLine.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbCommandLine.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tlpMain.SetColumnSpan(Me.rtbCommandLine, 2)
        Me.rtbCommandLine.LastCommandLine = Nothing
        Me.rtbCommandLine.Location = New System.Drawing.Point(15, 765)
        Me.rtbCommandLine.Margin = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.rtbCommandLine.Name = "rtbCommandLine"
        Me.rtbCommandLine.ReadOnly = True
        Me.rtbCommandLine.Size = New System.Drawing.Size(1715, 62)
        Me.rtbCommandLine.TabIndex = 2
        Me.rtbCommandLine.Text = ""
        '
        'gbAdvanced
        '
        Me.gbAdvanced.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbAdvanced.Controls.Add(Me.SimpleUI)
        Me.gbAdvanced.Location = New System.Drawing.Point(879, 14)
        Me.gbAdvanced.Margin = New System.Windows.Forms.Padding(7, 14, 15, 14)
        Me.gbAdvanced.Name = "gbAdvanced"
        Me.gbAdvanced.Padding = New System.Windows.Forms.Padding(5)
        Me.gbAdvanced.Size = New System.Drawing.Size(851, 737)
        Me.gbAdvanced.TabIndex = 3
        Me.gbAdvanced.TabStop = False
        Me.gbAdvanced.Text = "Advanced"
        '
        'SimpleUI
        '
        Me.SimpleUI.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleUI.Location = New System.Drawing.Point(7, 41)
        Me.SimpleUI.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SimpleUI.Name = "SimpleUI"
        Me.SimpleUI.Size = New System.Drawing.Size(834, 687)
        Me.SimpleUI.TabIndex = 0
        Me.SimpleUI.Text = "SimpleUI1"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(85, 15)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(15)
        Me.bnOK.Size = New System.Drawing.Size(250, 70)
        Me.bnOK.Text = "OK"
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(350, 15)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(0, 15, 15, 15)
        Me.bnCancel.Size = New System.Drawing.Size(250, 70)
        Me.bnCancel.Text = "Cancel"
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(22, 989)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(5)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(0, 0)
        Me.FlowLayoutPanel1.TabIndex = 4
        '
        'tlpMain
        '
        Me.tlpMain.AutoSize = True
        Me.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.Controls.Add(Me.rtbCommandLine, 0, 1)
        Me.tlpMain.Controls.Add(Me.gbBasic, 0, 0)
        Me.tlpMain.Controls.Add(Me.FlowLayoutPanel2, 1, 2)
        Me.tlpMain.Controls.Add(Me.gbAdvanced, 1, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1745, 927)
        Me.tlpMain.TabIndex = 11
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.FlowLayoutPanel2.AutoSize = True
        Me.FlowLayoutPanel2.Controls.Add(Me.bnMenu)
        Me.FlowLayoutPanel2.Controls.Add(Me.bnOK)
        Me.FlowLayoutPanel2.Controls.Add(Me.bnCancel)
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(1130, 827)
        Me.FlowLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(615, 100)
        Me.FlowLayoutPanel2.TabIndex = 11
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnMenu.Location = New System.Drawing.Point(0, 15)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(0)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(70, 70)
        '
        'AudioForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1745, 927)
        Me.Controls.Add(Me.tlpMain)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(7)
        Me.Name = "AudioForm"
        Me.Text = "Audio Settings"
        Me.gbBasic.ResumeLayout(False)
        Me.tlpBasic.ResumeLayout(False)
        Me.tlpBasic.PerformLayout()
        Me.gbAdvanced.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Profile, TempProfile As GUIAudioProfile

    Sub New()
        MyBase.New()
        InitializeComponent()

        rtbCommandLine.ReadOnly = True

        mbSamplingRate.Add("Original", 0)
        mbSamplingRate.Add("11025 Hz", 11025)
        mbSamplingRate.Add("22050 Hz", 22050)
        mbSamplingRate.Add("44100 Hz", 44100)
        mbSamplingRate.Add("48000 Hz", 48000)
        mbSamplingRate.Add("88200 Hz", 88200)
        mbSamplingRate.Add("96000 Hz", 96000)

        numBitrate.Minimum = 16
        numBitrate.Maximum = 16000
        numGain.DecimalPlaces = 1

        If components Is Nothing Then components = New System.ComponentModel.Container

        rtbCommandLine.ScrollBars = RichTextBoxScrollBars.None

        Dim cms As New ContextMenuStripEx(components)
        bnMenu.ContextMenuStrip = cms

        cms.Add("Copy Command Line", Sub() Clipboard.SetText(TempProfile.GetCommandLine(True))).SetImage(Symbol.Copy)
        cms.Add("Execute Command Line", AddressOf Execute).SetImage(Symbol.fa_terminal)
        cms.Add("Show Command Line...", Sub() g.ShowCommandLinePreview("Command Line", TempProfile.GetCommandLine(True)))
        cms.Add("-")
        cms.Add("Save Profile...", AddressOf SaveProfile, "Saves the current settings as profile").SetImage(Symbol.Save)
        cms.Add("-")
        cms.Add("Help", AddressOf ShowHelp).SetImage(Symbol.Help)
        cms.Add("eac3to Help", Sub() g.ShellExecute("http://en.wikibooks.org/wiki/Eac3to"))
        cms.Add("ffmpeg Help", Sub() g.ShellExecute(Package.ffmpeg.GetHelpPath))

        TipProvider.SetTip("Profile name that is auto generated when undefined.", lName)
        TipProvider.SetTip("Language used by the muxer. Saved in projects/templates but not in profiles.", mbLanguage, lLanguage)
        TipProvider.SetTip("Delay in milliseconds. eac3to and BeSweet handle delay, ffmpeg don't but it is handled by the muxer. Saved in projects/templates but not in profiles.", numDelay, lDelay)
    End Sub

    Private Sub AudioForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If DialogResult = DialogResult.OK Then SetValues(Profile)
    End Sub

    Sub SetValues(gap As GUIAudioProfile)
        gap.Bitrate = TempProfile.Bitrate
        gap.Language = TempProfile.Language
        gap.Delay = TempProfile.Delay
        gap.Name = TempProfile.Name
        gap.StreamName = TempProfile.StreamName
        gap.Gain = TempProfile.Gain
        gap.Default = TempProfile.Default
        gap.Forced = TempProfile.Forced
        gap.Params = TempProfile.Params
    End Sub

    Sub UpdateBitrate()
        nudQuality_ValueChanged(nudQuality)
    End Sub

    Private Sub nudQuality_ValueChanged(numEdit As NumEdit) Handles nudQuality.ValueChanged
        If Not TempProfile Is Nothing Then
            TempProfile.Params.Quality = CSng(nudQuality.Value)
            numBitrate.Value = TempProfile.GetBitrate
            UpdateControls()
        End If
    End Sub

    Private Sub nudBitrate_ValueChanged(numEdit As NumEdit) Handles numBitrate.ValueChanged
        If Not TempProfile Is Nothing Then
            TempProfile.Bitrate = CSng(numBitrate.Value)
            UpdateControls()
        End If
    End Sub

    Private Sub nudDelay_ValueChanged(numEdit As NumEdit) Handles numDelay.ValueChanged
        TempProfile.Delay = CInt(numDelay.Value)
        UpdateControls()
    End Sub

    Private Sub numGain_ValueChanged(numEdit As NumEdit) Handles numGain.ValueChanged
        TempProfile.Gain = CSng(numGain.Value)
        UpdateControls()
    End Sub

    Sub SimpleUIValueChanged()
        SimpleUI.Save()
        UpdateControls()
    End Sub

    Sub UpdateControls()
        Select Case TempProfile.Params.Codec
            Case AudioCodec.Opus, AudioCodec.Flac
                nudQuality.Enabled = False
            Case Else
                nudQuality.Enabled = TempProfile.Params.RateMode = AudioRateMode.VBR
        End Select

        If TempProfile.Params.Codec = AudioCodec.Flac Then
            numBitrate.Enabled = False
        Else
            numBitrate.Enabled = Not nudQuality.Enabled
        End If

        numBitrate.Increment = If(TempProfile.Params.Codec = AudioCodec.AC3, 32D, 1D)
        tbName.SendMessageCue(TempProfile.Name, False)
        rtbCommandLine.SetText(TempProfile.GetCommandLine(False))
        rtbCommandLine.UpdateHeight()
    End Sub

    Private Sub mbCodec_ValueChangedUser() Handles mbCodec.ValueChangedUser
        TempProfile.Params.Codec = mbCodec.GetValue(Of AudioCodec)()

        Select Case TempProfile.Params.Codec
            Case AudioCodec.AAC
                SetQuality(If(TempProfile.Params.Encoder = GuiAudioEncoder.qaac, 50.0F, 0.35F))
                TempProfile.Params.RateMode = AudioRateMode.VBR
            Case AudioCodec.AC3
                If TempProfile.Channels = 6 Then
                    numBitrate.Value = 448
                Else
                    numBitrate.Value = 224
                End If

                TempProfile.Params.RateMode = AudioRateMode.CBR
            Case AudioCodec.Flac
                numBitrate.Value = TempProfile.GetBitrate
                TempProfile.Params.RateMode = AudioRateMode.CBR
            Case AudioCodec.DTS
                If TempProfile.Channels = 6 Then
                    numBitrate.Value = 1536
                Else
                    numBitrate.Value = 768
                End If

                TempProfile.Params.RateMode = AudioRateMode.CBR
            Case AudioCodec.MP3
                SetQuality(4)
                TempProfile.Params.RateMode = AudioRateMode.VBR
            Case AudioCodec.Vorbis
                SetQuality(1)
                TempProfile.Params.RateMode = AudioRateMode.VBR
            Case AudioCodec.Opus
                numBitrate.Value = 80
                TempProfile.Params.RateMode = AudioRateMode.VBR
        End Select

        UpdateBitrate()
        TempProfile.GetCommandLine(False) 'set encoder
        LoadAdvanced()
        UpdateControls()
    End Sub

    Sub SetQuality(value As Single)
        If TempProfile.Params.Codec = AudioCodec.AAC AndAlso
            TempProfile.Params.Encoder = GuiAudioEncoder.qaac Then

            nudQuality.Minimum = 0D
            nudQuality.Maximum = 127D
            nudQuality.Increment = 1D
            nudQuality.DecimalPlaces = 0
        ElseIf TempProfile.Params.Codec = AudioCodec.MP3 Then
            nudQuality.Minimum = 0D
            nudQuality.Maximum = 9D
            nudQuality.Increment = 1D
            nudQuality.DecimalPlaces = 0
        ElseIf TempProfile.Params.Codec = AudioCodec.Vorbis Then
            nudQuality.Minimum = 0D
            nudQuality.Maximum = 10D
            nudQuality.Increment = 1D
            nudQuality.DecimalPlaces = 0
        Else
            nudQuality.Minimum = 0.01D
            nudQuality.Maximum = 1D
            nudQuality.Increment = 0.01D
            nudQuality.DecimalPlaces = 2
        End If

        nudQuality.Value = CDec(value)
    End Sub

    Private Sub mbChannels_ValueChanged() Handles mbChannels.ValueChangedUser
        TempProfile.Params.ChannelsMode = mbChannels.GetValue(Of ChannelsMode)()
        UpdateBitrate()
        UpdateControls()
    End Sub

    Private Sub mbSamplingRate_ValueChanged() Handles mbSamplingRate.ValueChangedUser
        TempProfile.Params.SamplingRate = mbSamplingRate.GetValue(Of Integer)()
        UpdateBitrate()
        UpdateControls()
    End Sub

    Private Sub mbLanguage_ValueChanged() Handles mbLanguage.ValueChangedUser
        TempProfile.Language = mbLanguage.GetValue(Of Language)()
        UpdateControls()
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        rtbCommandLine.Margin = New Padding(gbBasic.Margin.Left, 0, gbBasic.Margin.Left, 0)
    End Sub

    Private Sub AudioForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        UpdateControls()
        Refresh()
        ActiveControl = mbCodec

        For Each i In Language.Languages
            If i.IsCommon Then
                mbLanguage.Add(i.ToString + " (" + i.TwoLetterCode + ", " + i.ThreeLetterCode + ")", i)
            Else
                mbLanguage.Add("More | " + i.ToString.Substring(0, 1).ToUpper + " | " + i.ToString + " (" + i.TwoLetterCode + ", " + i.ThreeLetterCode + ")", i)
            End If
        Next
    End Sub

    Private Sub tbName_TextChanged(sender As Object, e As EventArgs) Handles tbName.TextChanged
        TempProfile.Name = tbName.Text
        UpdateControls()
    End Sub

    Sub SaveProfile()
        Dim gap = ObjectHelp.GetCopy(Of GUIAudioProfile)(TempProfile)
        Dim name = InputBox.Show("Enter the profile name.", "Save Profile", gap.Name)

        If name <> "" Then
            gap.Name = name
            s.AudioProfiles.Add(gap)
            MsgInfo("The profile was saved.")
        End If
    End Sub

    Sub LoadProfile(gap As GUIAudioProfile)
        Profile = gap
        TempProfile = ObjectHelp.GetCopy(Of GUIAudioProfile)(gap)
        LoadProfile()
    End Sub

    Sub LoadProfile()
        If TempProfile.Name <> TempProfile.DefaultName Then tbName.Text = TempProfile.Name
        tbName.SendMessageCue(TempProfile.Name, False)

        mbCodec.Value = TempProfile.Params.Codec
        mbChannels.Value = TempProfile.Params.ChannelsMode
        mbLanguage.Value = TempProfile.Language
        mbSamplingRate.Value = TempProfile.Params.SamplingRate
        mbEncoder.Value = TempProfile.Params.Encoder

        SetQuality(TempProfile.Params.Quality)

        numBitrate.Value = CDec(TempProfile.Bitrate)
        numDelay.Value = TempProfile.Delay
        numGain.Value = CDec(TempProfile.Gain)

        cbNormalize.Checked = TempProfile.Params.Normalize

        LoadAdvanced()
        UpdateControls()
    End Sub

    Sub SetBitrate(v As Integer)
        numBitrate.Value = v
    End Sub

    Sub LoadAdvanced()
        RemoveHandler SimpleUI.ValueChanged, AddressOf SimpleUIValueChanged

        Dim ui = SimpleUI
        ui.Host.Controls.Clear()

        If Not ui.ActivePage Is Nothing Then DirectCast(ui.ActivePage, Control).Dispose()

        Dim page = ui.CreateFlowPage("main page")
        page.SuspendLayout()

        Dim tb = ui.AddText(page)
        tb.Text = "Stream Name:"
        tb.Help = "Stream name used by the muxer."
        tb.Edit.Expandet = True
        tb.Edit.Text = TempProfile.StreamName
        tb.Edit.SaveAction = Sub(value) TempProfile.StreamName = value

        tb = ui.AddText(page)
        tb.Text = "Custom Switches:"
        tb.Help = "Custom command line arguments"
        tb.Edit.Expandet = True
        tb.Edit.Text = TempProfile.Params.CustomSwitches
        tb.Edit.SaveAction = Sub(value) TempProfile.Params.CustomSwitches = value

        Dim cb As SimpleUI.SimpleUICheckBox

        Select Case TempProfile.GetEncoder
            'Case GuiAudioEncoder.BeSweet
            '    Dim mb = ui.AddMenuButtonBlock(Of String)(page)
            '    mb.Label.Text = "Dynamic Compr.:"
            '    mb.Label.Tooltip = "Sets the overall dynamic compression in the decoder (applied to every output speaker)."
            '    mb.MenuButton.Expandet = True
            '    mb.MenuButton.Value = TempProfile.Params.BeSweetDynamicCompression
            '    mb.MenuButton.SaveAction = Sub(value) TempProfile.Params.BeSweetDynamicCompression = value
            '    mb.MenuButton.Add({"None", "Normal", "Light", "Heavy", "Inverse"})

            '    Dim mbRateMode = ui.AddMenuButtonBlock(Of AudioRateMode)(page)
            '    mbRateMode.Label.Text = "Rate Mode:"
            '    mbRateMode.MenuButton.Expandet = True
            '    mbRateMode.MenuButton.Value = TempProfile.Params.RateMode
            '    mbRateMode.MenuButton.SaveAction = Sub(value) TempProfile.Params.RateMode = value

            '    tb = ui.AddTextBlock(page)
            '    tb.Label.Text = "Gain/Normalize:"
            '    tb.Label.Tooltip = "Parameters added to ota section when the Normalize checkbox is enabled."
            '    tb.Edit.Expandet = True
            '    tb.Edit.Text = TempProfile.Params.BeSweetGainAndNormalization
            '    tb.Edit.SaveAction = Sub(value) TempProfile.Params.BeSweetGainAndNormalization = value

            '    tb = ui.AddTextBlock(page)
            '    tb.Label.Text = "Azid parameters:"
            '    tb.Label.Tooltip = "Custom parameters used in the Azid section."
            '    tb.Edit.Expandet = True
            '    tb.Edit.Text = TempProfile.Params.BeSweetAzid
            '    tb.Edit.SaveAction = Sub(value) TempProfile.Params.BeSweetAzid = value
            Case GuiAudioEncoder.Eac3to
                Dim mbFrameRateMode = ui.AddMenu(Of AudioFrameRateMode)(page)
                mbFrameRateMode.Label.Text = "Frame rate:"
                mbFrameRateMode.Button.Expandet = True
                mbFrameRateMode.Button.Value = TempProfile.Params.FrameRateMode
                mbFrameRateMode.Button.SaveAction = Sub(value) TempProfile.Params.FrameRateMode = value

                Dim mbStereoDownmix = ui.AddMenu(Of Integer)(page)
                mbStereoDownmix.Label.Text = "Stereo Downmix:"
                mbStereoDownmix.Button.Expandet = True
                mbStereoDownmix.Button.Add("Simple", 0)
                mbStereoDownmix.Button.Add("DPL II", 1)
                mbStereoDownmix.Button.Value = TempProfile.Params.eac3toStereoDownmixMode
                mbStereoDownmix.Button.SaveAction = Sub(value) TempProfile.Params.eac3toStereoDownmixMode = value

                cb = ui.AddBool(page)
                cb.Text = "Downconvert to 16 bit"
                cb.Checked = TempProfile.Params.Down16
                cb.SaveAction = Sub(value)
                                    TempProfile.Params.Down16 = value
                                    UpdateBitrate()
                                    UpdateControls()
                                End Sub

                If (TempProfile.File = "" OrElse TempProfile.File.ToLower.Contains("dts") OrElse
                    (Not TempProfile.Stream Is Nothing AndAlso
                     TempProfile.Stream.Name.Contains("DTS"))) AndAlso
                    TempProfile.Params.Codec = AudioCodec.DTS Then

                    cb = ui.AddBool(page)
                    cb.Text = "Extract DTS core"
                    cb.Checked = TempProfile.Params.eac3toExtractDtsCore
                    cb.SaveAction = Sub(value) TempProfile.Params.eac3toExtractDtsCore = value
                End If
            Case GuiAudioEncoder.ffmpeg
                Select Case TempProfile.Params.Codec
                    Case AudioCodec.AC3, AudioCodec.DTS, AudioCodec.Flac
                    Case Else
                        Dim mbRateMode = ui.AddMenu(Of AudioRateMode)(page)
                        mbRateMode.Label.Text = "Rate Mode:"
                        mbRateMode.Button.Expandet = True
                        mbRateMode.Button.Value = TempProfile.Params.RateMode
                        mbRateMode.Button.SaveAction = Sub(value) TempProfile.Params.RateMode = value
                End Select
            Case GuiAudioEncoder.qaac
                Dim mbMode = ui.AddMenu(Of Integer)(page)
                mbMode.Label.Text = "Mode:"
                mbMode.Button.Expandet = True
                mbMode.Button.Add("True VBR", 0)
                mbMode.Button.Add("Constrained VBR", 1)
                mbMode.Button.Add("ABR", 2)
                mbMode.Button.Add("CBR", 3)
                mbMode.Button.Value = TempProfile.Params.qaacRateMode
                mbMode.Button.SaveAction = Sub(value)
                                               TempProfile.Params.qaacRateMode = value
                                               TempProfile.Params.RateMode = If(TempProfile.Params.qaacRateMode = 0,
                                                                                    AudioRateMode.VBR,
                                                                                    AudioRateMode.CBR)
                                           End Sub

                Dim mbQuality = ui.AddMenu(Of Integer)(page)
                mbQuality.Label.Text = "Quality:"
                mbQuality.Button.Expandet = True
                mbQuality.Button.Add("Low", 0)
                mbQuality.Button.Add("Medium", 1)
                mbQuality.Button.Add("High", 2)
                mbQuality.Button.Value = TempProfile.Params.qaacQuality
                mbQuality.Button.SaveAction = Sub(value) TempProfile.Params.qaacQuality = value

                Dim num = ui.AddNum(page)
                num.Label.Text = "Lowpass:"
                num.NumEdit.Config = {0, Integer.MaxValue, 1}
                num.NumEdit.Value = TempProfile.Params.qaacLowpass
                num.NumEdit.SaveAction = Sub(value) TempProfile.Params.qaacLowpass = CInt(value)

                cb = ui.AddBool(page)
                cb.Text = "High Efficiency"
                cb.Checked = TempProfile.Params.qaacHE
                cb.SaveAction = Sub(value) TempProfile.Params.qaacHE = value
                AddHandler cb.CheckedChanged, Sub() If cb.Checked Then mbMode.Button.Value = 1
                AddHandler mbMode.Button.ValueChangedUser, Sub() If mbMode.Button.Value = 0 Then cb.Checked = False

                cb = ui.AddBool(page)
                cb.Text = "No dither when quantizing to lower bit depth"
                cb.Checked = TempProfile.Params.qaacNoDither
                cb.SaveAction = Sub(value) TempProfile.Params.qaacNoDither = value
        End Select

        cb = ui.AddBool(page)
        cb.Text = "Default"
        cb.Help = "Marked as default in MKV."
        cb.Checked = TempProfile.Default
        cb.SaveAction = Sub(value) TempProfile.Default = value

        cb = ui.AddBool(page)
        cb.Text = "Forced"
        cb.Help = "Marked as forced in MKV."
        cb.Checked = TempProfile.Forced
        cb.SaveAction = Sub(value) TempProfile.Forced = value

        page.ResumeLayout(True)

        AddHandler SimpleUI.ValueChanged, AddressOf SimpleUIValueChanged
    End Sub

    Private Sub nudBitrate_KeyUp(sender As Object, e As KeyEventArgs) Handles numBitrate.KeyUp
        Try
            Dim v = CInt(numBitrate.Text)
            If v Mod 16 = 0 Then numBitrate.Value = v
        Catch
        End Try
    End Sub

    Private Sub nudQuality_KeyUp(sender As Object, e As KeyEventArgs) Handles nudQuality.KeyUp
        Try
            Dim v = CInt(nudQuality.Text)
            nudQuality.Value = v
        Catch
        End Try
    End Sub

    Private Sub mbEncoder_ValueChangedUser() Handles mbEncoder.ValueChangedUser
        TempProfile.Params.Encoder = mbEncoder.GetValue(Of GuiAudioEncoder)()
        mbCodec_ValueChangedUser()
    End Sub

    Private Sub cbNormalize_CheckedChanged(sender As Object, e As EventArgs) Handles cbNormalize.CheckedChanged
        TempProfile.Params.Normalize = cbNormalize.Checked
        UpdateControls()
    End Sub

    Private Sub Execute()
        If TempProfile.File <> "" Then
            If Not TempProfile.SupportedInput.Contains(Filepath.GetExt(TempProfile.File)) Then
                MsgWarn("The input format isn't supported," + BR + "please decode first using:" + BR2 + "Codec: WAV" + BR + "Encoder: ffmpeg")
            Else
                Dim batchPath = p.TempDir + p.TargetFile.Base + "_aexe.bat"
                Dim batchCode = Proc.WriteBatchFile(batchPath, TempProfile.GetCommandLine(True))
                Dim batchProc As New Process
                batchProc.StartInfo.FileName = "cmd.exe"
                batchProc.StartInfo.Arguments = "/k """ + batchPath + """"
                batchProc.StartInfo.WorkingDirectory = p.TempDir
                batchProc.Start()
            End If
        Else
            MsgWarn("Source file is missing!")
        End If
    End Sub

    Private Sub CommandLineAudioSettingsForm_HelpRequested() Handles Me.HelpRequested
        ShowHelp()
    End Sub

    Private Sub ShowHelp()
        Dim f As New HelpForm()
        f.Doc.WriteStart(Text)
        f.Doc.WriteTips(TipProvider.GetTips, SimpleUI.ActivePage.TipProvider.GetTips)
        f.Show()
    End Sub
End Class