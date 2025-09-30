﻿
Imports StaxRip.VideoEncoderCommandLine
Imports StaxRip.UI
Imports DirectN
Imports System.Text
Imports System.Threading.Tasks
Imports System.Threading
Imports MS.Internal.IO

Public Class AudioForm
    Inherits DialogBase

#Region " Designer "
    Friend WithEvents CommandLink1 As StaxRip.UI.CommandLink
    Friend WithEvents gbBasic As GroupBoxEx
    Friend WithEvents numQuality As NumEdit
    Friend WithEvents numBitrate As NumEdit
    Friend WithEvents lQualiy As LabelEx
    Friend WithEvents lCodec As LabelEx
    Friend WithEvents mbCodec As StaxRip.UI.MenuButton
    Friend WithEvents mbLanguage As StaxRip.UI.MenuButton
    Friend WithEvents lLanguage As LabelEx
    Friend WithEvents numDelay As NumEdit
    Friend WithEvents lDelay As LabelEx
    Friend WithEvents mbChannels As StaxRip.UI.MenuButton
    Friend WithEvents lChannels As LabelEx
    Friend WithEvents gbAdvanced As GroupBoxEx
    Friend WithEvents laProfileName As LabelEx
    Friend WithEvents tbProfileName As TextEdit
    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents mbSamplingRate As StaxRip.UI.MenuButton
    Friend WithEvents Label1 As LabelEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents mbEncoder As StaxRip.UI.MenuButton
    Friend WithEvents Label2 As LabelEx
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents SimpleUI As StaxRip.SimpleUI
    Friend WithEvents Label3 As LabelEx
    Friend WithEvents numGain As StaxRip.UI.NumEdit
    Friend WithEvents Label4 As LabelEx
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents flpButtons As FlowLayoutPanel
    Friend WithEvents tlpBasic As TableLayoutPanel
    Friend WithEvents bnMenu As ButtonEx
    Friend WithEvents tlpRTB As TableLayoutPanel
    Friend WithEvents rtbCommandLine As CommandLineRichTextBox
    Friend WithEvents laStreamName As LabelEx
    Friend WithEvents tbStreamName As TextEdit
    Friend WithEvents laCustom As LabelEx
    Friend WithEvents tbCustom As TextEdit
    Friend WithEvents cbForcedTrack As CheckBoxEx
    Friend WithEvents cbDefaultTrack As CheckBoxEx
    Friend WithEvents laDecoder As LabelEx
    Friend WithEvents mbDecoder As MenuButton
    Friend WithEvents tlpAdvanced As TableLayoutPanel
    Friend WithEvents bnAdvanced As ButtonEx
    Friend WithEvents cbNormalize As CheckBoxEx
    Friend WithEvents cbCenterOptimizedStereo As CheckBoxEx
    Friend WithEvents cbCommentaryTrack As CheckBoxEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.gbBasic = New GroupBoxEx()
        Me.tlpBasic = New System.Windows.Forms.TableLayoutPanel()
        Me.lCodec = New LabelEx()
        Me.tbProfileName = New TextEdit
        Me.laProfileName = New LabelEx()
        Me.mbCodec = New StaxRip.UI.MenuButton()
        Me.mbLanguage = New StaxRip.UI.MenuButton()
        Me.mbSamplingRate = New StaxRip.UI.MenuButton()
        Me.lLanguage = New LabelEx()
        Me.Label1 = New LabelEx()
        Me.Label3 = New LabelEx()
        Me.numBitrate = New StaxRip.UI.NumEdit()
        Me.Label2 = New LabelEx()
        Me.mbEncoder = New StaxRip.UI.MenuButton()
        Me.lChannels = New LabelEx()
        Me.mbChannels = New StaxRip.UI.MenuButton()
        Me.laStreamName = New LabelEx()
        Me.tbStreamName = New TextEdit
        Me.laCustom = New LabelEx()
        Me.tbCustom = New TextEdit
        Me.cbDefaultTrack = New StaxRip.UI.CheckBoxEx()
        Me.cbForcedTrack = New StaxRip.UI.CheckBoxEx()
        Me.laDecoder = New LabelEx()
        Me.mbDecoder = New StaxRip.UI.MenuButton()
        Me.lQualiy = New LabelEx()
        Me.numQuality = New StaxRip.UI.NumEdit()
        Me.Label4 = New LabelEx()
        Me.numGain = New StaxRip.UI.NumEdit()
        Me.cbNormalize = New StaxRip.UI.CheckBoxEx()
        Me.cbCenterOptimizedStereo = New StaxRip.UI.CheckBoxEx()
        Me.cbCommentaryTrack = New StaxRip.UI.CheckBoxEx()
        Me.numDelay = New StaxRip.UI.NumEdit()
        Me.lDelay = New LabelEx()
        Me.gbAdvanced = New GroupBoxEx()
        Me.tlpAdvanced = New System.Windows.Forms.TableLayoutPanel()
        Me.SimpleUI = New StaxRip.SimpleUI()
        Me.bnAdvanced = New StaxRip.UI.ButtonEx()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.flpButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
        Me.tlpRTB = New System.Windows.Forms.TableLayoutPanel()
        Me.rtbCommandLine = New StaxRip.UI.CommandLineRichTextBox()
        Me.gbBasic.SuspendLayout()
        Me.tlpBasic.SuspendLayout()
        Me.gbAdvanced.SuspendLayout()
        Me.tlpAdvanced.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.flpButtons.SuspendLayout()
        Me.tlpRTB.SuspendLayout()
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
        Me.gbBasic.Size = New System.Drawing.Size(1050, 1150)
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
        Me.tlpBasic.Controls.Add(Me.tbProfileName, 1, 7)
        Me.tlpBasic.Controls.Add(Me.laProfileName, 0, 7)
        Me.tlpBasic.Controls.Add(Me.mbCodec, 1, 0)
        Me.tlpBasic.Controls.Add(Me.mbLanguage, 1, 5)
        Me.tlpBasic.Controls.Add(Me.mbSamplingRate, 1, 4)
        Me.tlpBasic.Controls.Add(Me.lLanguage, 0, 5)
        Me.tlpBasic.Controls.Add(Me.Label1, 0, 4)
        Me.tlpBasic.Controls.Add(Me.Label3, 2, 0)
        Me.tlpBasic.Controls.Add(Me.numBitrate, 3, 0)
        Me.tlpBasic.Controls.Add(Me.Label2, 0, 2)
        Me.tlpBasic.Controls.Add(Me.mbEncoder, 1, 2)
        Me.tlpBasic.Controls.Add(Me.lChannels, 0, 3)
        Me.tlpBasic.Controls.Add(Me.mbChannels, 1, 3)
        Me.tlpBasic.Controls.Add(Me.laStreamName, 0, 8)
        Me.tlpBasic.Controls.Add(Me.tbStreamName, 1, 8)
        Me.tlpBasic.Controls.Add(Me.laCustom, 0, 9)
        Me.tlpBasic.Controls.Add(Me.tbCustom, 1, 9)
        Me.tlpBasic.Controls.Add(Me.cbNormalize, 0, 10)
        Me.tlpBasic.Controls.Add(Me.cbCenterOptimizedStereo, 0, 11)
        Me.tlpBasic.Controls.Add(Me.cbDefaultTrack, 0, 12)
        Me.tlpBasic.Controls.Add(Me.cbForcedTrack, 0, 13)
        Me.tlpBasic.Controls.Add(Me.cbCommentaryTrack, 0, 14)
        Me.tlpBasic.Controls.Add(Me.laDecoder, 0, 1)
        Me.tlpBasic.Controls.Add(Me.mbDecoder, 1, 1)
        Me.tlpBasic.Controls.Add(Me.lQualiy, 2, 1)
        Me.tlpBasic.Controls.Add(Me.numQuality, 3, 1)
        Me.tlpBasic.Controls.Add(Me.Label4, 2, 2)
        Me.tlpBasic.Controls.Add(Me.numGain, 3, 2)
        Me.tlpBasic.Controls.Add(Me.numDelay, 3, 3)
        Me.tlpBasic.Controls.Add(Me.lDelay, 2, 3)
        Me.tlpBasic.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpBasic.Location = New System.Drawing.Point(5, 53)
        Me.tlpBasic.Margin = New System.Windows.Forms.Padding(5)
        Me.tlpBasic.Name = "tlpBasic"
        Me.tlpBasic.RowCount = 15
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBasic.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8.0!))
        Me.tlpBasic.Size = New System.Drawing.Size(901, 960)
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
        'tbProfileName
        '
        Me.tbProfileName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpBasic.SetColumnSpan(Me.tbProfileName, 3)
        Me.tbProfileName.Location = New System.Drawing.Point(250, 485)
        Me.tbProfileName.Margin = New System.Windows.Forms.Padding(5)
        Me.tbProfileName.Name = "tbProfileName"
        Me.tbProfileName.Size = New System.Drawing.Size(646, 55)
        Me.tbProfileName.TabIndex = 16
        '
        'laProfileName
        '
        Me.laProfileName.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.laProfileName.AutoSize = True
        Me.laProfileName.Location = New System.Drawing.Point(5, 488)
        Me.laProfileName.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.laProfileName.Name = "laProfileName"
        Me.laProfileName.Size = New System.Drawing.Size(235, 48)
        Me.laProfileName.TabIndex = 15
        Me.laProfileName.Text = "Profile Name:"
        '
        'mbCodec
        '
        Me.mbCodec.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbCodec.Location = New System.Drawing.Point(250, 5)
        Me.mbCodec.Margin = New System.Windows.Forms.Padding(5)
        Me.mbCodec.ShowMenuSymbol = True
        Me.mbCodec.Size = New System.Drawing.Size(293, 70)
        Me.mbCodec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'mbLanguage
        '
        Me.mbLanguage.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbLanguage.Location = New System.Drawing.Point(250, 405)
        Me.mbLanguage.Margin = New System.Windows.Forms.Padding(5)
        Me.mbLanguage.ShowMenuSymbol = True
        Me.mbLanguage.Size = New System.Drawing.Size(293, 70)
        Me.mbLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'mbSamplingRate
        '
        Me.mbSamplingRate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbSamplingRate.Location = New System.Drawing.Point(250, 325)
        Me.mbSamplingRate.Margin = New System.Windows.Forms.Padding(5)
        Me.mbSamplingRate.ShowMenuSymbol = True
        Me.mbSamplingRate.Size = New System.Drawing.Size(293, 70)
        Me.mbSamplingRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lLanguage
        '
        Me.lLanguage.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lLanguage.AutoSize = True
        Me.lLanguage.Location = New System.Drawing.Point(5, 416)
        Me.lLanguage.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lLanguage.Name = "lLanguage"
        Me.lLanguage.Size = New System.Drawing.Size(182, 48)
        Me.lLanguage.TabIndex = 10
        Me.lLanguage.Text = "Language:"
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 336)
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
        Me.Label3.Location = New System.Drawing.Point(553, 16)
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
        Me.numBitrate.Location = New System.Drawing.Point(703, 5)
        Me.numBitrate.Margin = New System.Windows.Forms.Padding(5)
        Me.numBitrate.Name = "numBitrate"
        Me.numBitrate.Size = New System.Drawing.Size(193, 70)
        Me.numBitrate.TabIndex = 17
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(5, 176)
        Me.Label2.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(157, 48)
        Me.Label2.TabIndex = 24
        Me.Label2.Text = "Encoder:"
        '
        'mbEncoder
        '
        Me.mbEncoder.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbEncoder.Location = New System.Drawing.Point(250, 165)
        Me.mbEncoder.Margin = New System.Windows.Forms.Padding(5)
        Me.mbEncoder.ShowMenuSymbol = True
        Me.mbEncoder.Size = New System.Drawing.Size(293, 70)
        Me.mbEncoder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lChannels
        '
        Me.lChannels.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lChannels.AutoSize = True
        Me.lChannels.Location = New System.Drawing.Point(5, 256)
        Me.lChannels.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lChannels.Name = "lChannels"
        Me.lChannels.Size = New System.Drawing.Size(171, 48)
        Me.lChannels.TabIndex = 5
        Me.lChannels.Text = "Channels:"
        '
        'mbChannels
        '
        Me.mbChannels.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbChannels.Location = New System.Drawing.Point(250, 245)
        Me.mbChannels.Margin = New System.Windows.Forms.Padding(5)
        Me.mbChannels.ShowMenuSymbol = True
        Me.mbChannels.Size = New System.Drawing.Size(293, 70)
        Me.mbChannels.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'laStreamName
        '
        Me.laStreamName.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.laStreamName.AutoSize = True
        Me.laStreamName.Location = New System.Drawing.Point(5, 553)
        Me.laStreamName.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.laStreamName.Name = "laStreamName"
        Me.laStreamName.Size = New System.Drawing.Size(215, 48)
        Me.laStreamName.TabIndex = 44
        Me.laStreamName.Text = "Track Name:"
        '
        'tbStreamName
        '
        Me.tbStreamName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpBasic.SetColumnSpan(Me.tbStreamName, 3)
        Me.tbStreamName.Location = New System.Drawing.Point(250, 550)
        Me.tbStreamName.Margin = New System.Windows.Forms.Padding(5)
        Me.tbStreamName.Name = "tbStreamName"
        Me.tbStreamName.Size = New System.Drawing.Size(646, 55)
        Me.tbStreamName.TabIndex = 45
        '
        'laCustom
        '
        Me.laCustom.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.laCustom.AutoSize = True
        Me.laCustom.Location = New System.Drawing.Point(5, 618)
        Me.laCustom.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.laCustom.Name = "laCustom"
        Me.laCustom.Size = New System.Drawing.Size(141, 48)
        Me.laCustom.TabIndex = 46
        Me.laCustom.Text = "Custom:"
        '
        'tbCustom
        '
        Me.tbCustom.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpBasic.SetColumnSpan(Me.tbCustom, 3)
        Me.tbCustom.Location = New System.Drawing.Point(250, 615)
        Me.tbCustom.Margin = New System.Windows.Forms.Padding(5)
        Me.tbCustom.Name = "tbCustom"
        Me.tbCustom.Size = New System.Drawing.Size(646, 55)
        Me.tbCustom.TabIndex = 47
        '
        'cbDefaultTrack
        '
        Me.cbDefaultTrack.AutoSize = True
        Me.tlpBasic.SetColumnSpan(Me.cbDefaultTrack, 4)
        Me.cbDefaultTrack.Location = New System.Drawing.Point(15, 796)
        Me.cbDefaultTrack.Margin = New System.Windows.Forms.Padding(15, 1, 3, 1)
        Me.cbDefaultTrack.Size = New System.Drawing.Size(272, 52)
        Me.cbDefaultTrack.Text = "Default Track"
        '
        'cbForcedTrack
        '
        Me.cbForcedTrack.AutoSize = True
        Me.tlpBasic.SetColumnSpan(Me.cbForcedTrack, 4)
        Me.cbForcedTrack.Location = New System.Drawing.Point(15, 852)
        Me.cbForcedTrack.Margin = New System.Windows.Forms.Padding(15, 1, 3, 1)
        Me.cbForcedTrack.Size = New System.Drawing.Size(267, 52)
        Me.cbForcedTrack.Text = "Forced Track"
        '
        'cbCommentaryTrack
        '
        Me.cbCommentaryTrack.AutoSize = True
        Me.tlpBasic.SetColumnSpan(Me.cbCommentaryTrack, 4)
        Me.cbCommentaryTrack.Location = New System.Drawing.Point(15, 908)
        Me.cbCommentaryTrack.Margin = New System.Windows.Forms.Padding(15, 1, 3, 1)
        Me.cbCommentaryTrack.Size = New System.Drawing.Size(290, 52)
        Me.cbCommentaryTrack.Text = "Commentary Track"
        Me.cbCommentaryTrack.UseVisualStyleBackColor = False
        '
        'laDecoder
        '
        Me.laDecoder.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.laDecoder.AutoSize = True
        Me.laDecoder.Location = New System.Drawing.Point(5, 96)
        Me.laDecoder.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.laDecoder.Name = "laDecoder"
        Me.laDecoder.Size = New System.Drawing.Size(163, 48)
        Me.laDecoder.TabIndex = 50
        Me.laDecoder.Text = "Decoder:"
        '
        'mbDecoder
        '
        Me.mbDecoder.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbDecoder.Location = New System.Drawing.Point(250, 85)
        Me.mbDecoder.Margin = New System.Windows.Forms.Padding(5)
        Me.mbDecoder.ShowMenuSymbol = True
        Me.mbDecoder.Size = New System.Drawing.Size(293, 70)
        Me.mbDecoder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lQualiy
        '
        Me.lQualiy.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lQualiy.AutoSize = True
        Me.lQualiy.Location = New System.Drawing.Point(553, 96)
        Me.lQualiy.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lQualiy.Name = "lQualiy"
        Me.lQualiy.Size = New System.Drawing.Size(140, 48)
        Me.lQualiy.TabIndex = 12
        Me.lQualiy.TabStop = True
        Me.lQualiy.Text = "Quality:"
        '
        'numQuality
        '
        Me.numQuality.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numQuality.Location = New System.Drawing.Point(703, 85)
        Me.numQuality.Margin = New System.Windows.Forms.Padding(5)
        Me.numQuality.Name = "numQuality"
        Me.numQuality.Size = New System.Drawing.Size(193, 70)
        Me.numQuality.TabIndex = 18
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(553, 176)
        Me.Label4.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 48)
        Me.Label4.TabIndex = 36
        Me.Label4.TabStop = True
        Me.Label4.Text = "Gain:"
        '
        'numGain
        '
        Me.numGain.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numGain.Location = New System.Drawing.Point(703, 165)
        Me.numGain.Margin = New System.Windows.Forms.Padding(5)
        Me.numGain.Name = "numGain"
        Me.numGain.Size = New System.Drawing.Size(193, 70)
        Me.numGain.TabIndex = 37
        '
        'cbNormalize
        '
        Me.cbNormalize.AutoSize = True
        Me.tlpBasic.SetColumnSpan(Me.cbNormalize, 4)
        Me.cbNormalize.Location = New System.Drawing.Point(15, 678)
        Me.cbNormalize.Margin = New System.Windows.Forms.Padding(15, 3, 3, 1)
        Me.cbNormalize.Size = New System.Drawing.Size(229, 52)
        Me.cbNormalize.Text = "Normalize"
        '
        'cbCenterOptimizedStereo
        '
        Me.cbCenterOptimizedStereo.AutoSize = True
        Me.tlpBasic.SetColumnSpan(Me.cbCenterOptimizedStereo, 4)
        Me.cbCenterOptimizedStereo.Location = New System.Drawing.Point(15, 708)
        Me.cbCenterOptimizedStereo.Margin = New System.Windows.Forms.Padding(15, 3, 3, 1)
        Me.cbCenterOptimizedStereo.Size = New System.Drawing.Size(229, 52)
        Me.cbCenterOptimizedStereo.Text = "Center/Speech optimized Stereo"
        '
        'numDelay
        '
        Me.numDelay.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.numDelay.Location = New System.Drawing.Point(703, 245)
        Me.numDelay.Margin = New System.Windows.Forms.Padding(5)
        Me.numDelay.Name = "numDelay"
        Me.numDelay.Size = New System.Drawing.Size(193, 70)
        Me.numDelay.TabIndex = 19
        '
        'lDelay
        '
        Me.lDelay.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lDelay.AutoSize = True
        Me.lDelay.Location = New System.Drawing.Point(553, 256)
        Me.lDelay.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lDelay.Name = "lDelay"
        Me.lDelay.Size = New System.Drawing.Size(116, 48)
        Me.lDelay.TabIndex = 14
        Me.lDelay.TabStop = True
        Me.lDelay.Text = "Delay:"
        '
        'gbAdvanced
        '
        Me.gbAdvanced.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbAdvanced.Controls.Add(Me.tlpAdvanced)
        Me.gbAdvanced.Location = New System.Drawing.Point(940, 14)
        Me.gbAdvanced.Margin = New System.Windows.Forms.Padding(7, 14, 15, 14)
        Me.gbAdvanced.Name = "gbAdvanced"
        Me.gbAdvanced.Padding = New System.Windows.Forms.Padding(5)
        Me.gbAdvanced.Size = New System.Drawing.Size(1000, 1150)
        Me.gbAdvanced.TabIndex = 3
        Me.gbAdvanced.TabStop = False
        Me.gbAdvanced.Text = "Advanced"
        '
        'tlpAdvanced
        '
        Me.tlpAdvanced.ColumnCount = 1
        Me.tlpAdvanced.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAdvanced.Controls.Add(Me.SimpleUI, 0, 0)
        Me.tlpAdvanced.Controls.Add(Me.bnAdvanced, 0, 1)
        Me.tlpAdvanced.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAdvanced.Location = New System.Drawing.Point(5, 53)
        Me.tlpAdvanced.Name = "tlpAdvanced"
        Me.tlpAdvanced.RowCount = 2
        Me.tlpAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAdvanced.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpAdvanced.Size = New System.Drawing.Size(901, 960)
        Me.tlpAdvanced.TabIndex = 1
        '
        'SimpleUI
        '
        Me.SimpleUI.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleUI.FormSizeScaleFactor = New System.Drawing.SizeF(0!, 0!)
        Me.SimpleUI.Location = New System.Drawing.Point(4, 3)
        Me.SimpleUI.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.SimpleUI.Name = "SimpleUI"
        Me.SimpleUI.Size = New System.Drawing.Size(893, 878)
        Me.SimpleUI.TabIndex = 0
        Me.SimpleUI.Text = "SimpleUI1"
        '
        'bnAdvanced
        '
        Me.bnAdvanced.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAdvanced.Location = New System.Drawing.Point(3, 887)
        Me.bnAdvanced.Size = New System.Drawing.Size(895, 70)
        Me.bnAdvanced.Text = "More..."
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(115, 15)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(15)
        Me.bnOK.Size = New System.Drawing.Size(250, 70)
        Me.bnOK.Text = "OK"
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(380, 15)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(0, 15, 15, 15)
        Me.bnCancel.Size = New System.Drawing.Size(250, 70)
        Me.bnCancel.Text = "Cancel"
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(22, 1268)
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
        Me.tlpMain.Controls.Add(Me.gbBasic, 0, 0)
        Me.tlpMain.Controls.Add(Me.flpButtons, 1, 2)
        Me.tlpMain.Controls.Add(Me.gbAdvanced, 1, 0)
        Me.tlpMain.Controls.Add(Me.tlpRTB, 0, 1)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1866, 1206)
        Me.tlpMain.TabIndex = 11
        '
        'flpButtons
        '
        Me.flpButtons.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.flpButtons.AutoSize = True
        Me.flpButtons.Controls.Add(Me.bnMenu)
        Me.flpButtons.Controls.Add(Me.bnOK)
        Me.flpButtons.Controls.Add(Me.bnCancel)
        Me.flpButtons.Location = New System.Drawing.Point(1221, 1106)
        Me.flpButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flpButtons.Name = "flpButtons"
        Me.flpButtons.Size = New System.Drawing.Size(645, 100)
        Me.flpButtons.TabIndex = 11
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnMenu.Location = New System.Drawing.Point(0, 15)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(0)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(100, 70)
        '
        'tlpRTB
        '
        Me.tlpRTB.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpRTB.AutoSize = True
        Me.tlpRTB.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpRTB.ColumnCount = 1
        Me.tlpMain.SetColumnSpan(Me.tlpRTB, 2)
        Me.tlpRTB.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpRTB.Controls.Add(Me.rtbCommandLine, 0, 0)
        Me.tlpRTB.Location = New System.Drawing.Point(15, 1046)
        Me.tlpRTB.Margin = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.tlpRTB.Name = "tlpRTB"
        Me.tlpRTB.RowCount = 1
        Me.tlpRTB.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpRTB.Size = New System.Drawing.Size(1836, 60)
        Me.tlpRTB.TabIndex = 12
        '
        'rtbCommandLine
        '
        Me.rtbCommandLine.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbCommandLine.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbCommandLine.LastCommandLine = Nothing
        Me.rtbCommandLine.Location = New System.Drawing.Point(0, 0)
        Me.rtbCommandLine.Margin = New System.Windows.Forms.Padding(0)
        Me.rtbCommandLine.Name = "rtbCommandLine"
        Me.rtbCommandLine.ReadOnly = True
        Me.rtbCommandLine.Size = New System.Drawing.Size(1836, 60)
        Me.rtbCommandLine.TabIndex = 45
        Me.rtbCommandLine.Text = ""
        '
        'AudioForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = False
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1866, 1406)
        Me.Controls.Add(Me.tlpMain)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(7)
        Me.Name = "AudioForm"
        Me.Text = $"Audio Settings - {g.DefaultCommands.GetApplicationDetails()}"
        Me.gbBasic.ResumeLayout(False)
        Me.tlpBasic.ResumeLayout(False)
        Me.tlpBasic.PerformLayout()
        Me.gbAdvanced.ResumeLayout(False)
        Me.tlpAdvanced.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.flpButtons.ResumeLayout(False)
        Me.tlpRTB.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Profile, TempProfile As GUIAudioProfile
    Private CommandLineHighlightingMenuItem As MenuItemEx
    Private BlockUpdate As Boolean = False

    Sub New()
        MyBase.New()
        InitializeComponent()
        RestoreClientSize(55, 35)

        rtbCommandLine.ReadOnly = True

        numBitrate.Minimum = 1
        numBitrate.Maximum = 16000
        numGain.DecimalPlaces = 1

        'cbDefaultTrack.Visible = TypeOf p.VideoEncoder.Muxer Is MkvMuxer
        cbForcedTrack.Visible = TypeOf p.VideoEncoder.Muxer Is MkvMuxer
        cbCommentaryTrack.Visible = TypeOf p.VideoEncoder.Muxer Is MkvMuxer

        If components Is Nothing Then
            components = New System.ComponentModel.Container
        End If

        rtbCommandLine.ScrollBars = RichTextBoxScrollBars.None

        Dim cms As New ContextMenuStripEx(components) With {
            .Form = Me
        }
        bnMenu.ContextMenuStrip = cms

        cms.Add("Execute Command Line", AddressOf Execute).SetImage(Symbol.fa_terminal)
        cms.Add("Copy Command Line", Sub() Clipboard.SetText(TempProfile.GetCommandLine(True))).SetImage(Symbol.Copy)
        cms.Add("Show Command Line...", Sub() g.ShowCommandLinePreview("Command Line", TempProfile.GetCommandLine(True), False))
        cms.Add("-")

        Dim a = Sub()
                    CommandLineHighlightingMenuItem.Checked = Not CommandLineHighlightingMenuItem.Checked
                    s.CommandLineHighlighting = CommandLineHighlightingMenuItem.Checked
                    rtbCommandLine.Format(rtbCommandLine.Text.ToString)
                End Sub

        CommandLineHighlightingMenuItem = cms.Add("Command Line Highlighting", a, Keys.Control Or Keys.H)
        CommandLineHighlightingMenuItem.Checked = s.CommandLineHighlighting

        cms.Add("-")
        cms.Add("Save Profile...", AddressOf SaveProfile, "Saves the current settings as profile").SetImage(Symbol.Save)
        cms.Add("-")
        cms.Add("Help", AddressOf ShowHelp).SetImage(Symbol.Help)
        cms.Add("eac3to Help", Sub() g.ShellExecute("https://en.wikibooks.org/wiki/Eac3to"))
        cms.Add("ffmpeg Help", Sub() Package.ffmpeg.ShowHelp())

        TipProvider.SetTip("Defines which decoder to use and forces decoding even if not necessary.", laDecoder, mbDecoder)
        TipProvider.SetTip("Profile name that is auto generated when undefined.", laProfileName)
        TipProvider.SetTip("Language used by the muxer. Saved in projects/templates but not in profiles.", mbLanguage, lLanguage)
        TipProvider.SetTip("Delay in milliseconds. eac3to handles delay, ffmpeg don't but it is handled by the muxer. Saved in projects/templates but not in profiles.", numDelay, lDelay)
        TipProvider.SetTip("Track name used by the muxer.", tbStreamName, laStreamName)
        TipProvider.SetTip("Custom command line arguments.", tbCustom, laCustom)
        TipProvider.SetTip("Default MKV Track.", cbDefaultTrack)
        TipProvider.SetTip("Forced MKV Track.", cbForcedTrack)
        TipProvider.SetTip("Commentary MKV Track.", cbCommentaryTrack)

        ApplyTheme()

        AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        RemoveHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
        components?.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    Sub OnThemeChanged(theme As Theme)
        ApplyTheme(theme)
    End Sub

    Sub ApplyTheme()
        ApplyTheme(ThemeManager.CurrentTheme)
    End Sub

    Sub ApplyTheme(theme As Theme)
        If DesignHelp.IsDesignMode Then Exit Sub

        BackColor = theme.General.BackColor
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        If DialogResult = DialogResult.OK Then
            SetValues(Profile)
        End If
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        g.PopulateLanguagesAsync(mbLanguage)
        UpdateControls()
        Refresh()
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
        gap.Commentary = TempProfile.Commentary
        gap.Params = TempProfile.Params
        gap.Decoder = TempProfile.Decoder
        gap.DecodingMode = TempProfile.DecodingMode
        gap.ExtractDTSCore = TempProfile.ExtractDTSCore
    End Sub

    Sub UpdateBitrate()
        nudQuality_ValueChanged(numQuality)
    End Sub

    Sub nudQuality_ValueChanged(numEdit As NumEdit) Handles numQuality.ValueChanged
        If TempProfile IsNot Nothing Then
            TempProfile.Params.Quality = CSng(numQuality.Value)
            numBitrate.Value = TempProfile.GetBitrate
            If Not BlockUpdate Then UpdateControls()
        End If
    End Sub

    Sub nudBitrate_ValueChanged(numEdit As NumEdit) Handles numBitrate.ValueChanged
        If TempProfile IsNot Nothing Then
            TempProfile.Bitrate = CSng(numBitrate.Value)
            If Not BlockUpdate Then UpdateControls()
        End If
    End Sub

    Sub nudDelay_ValueChanged(numEdit As NumEdit) Handles numDelay.ValueChanged
        TempProfile.Delay = CInt(numDelay.Value)
        If Not BlockUpdate Then UpdateControls()
    End Sub

    Sub numGain_ValueChanged(numEdit As NumEdit) Handles numGain.ValueChanged
        TempProfile.Gain = CSng(numGain.Value)
        If Not BlockUpdate Then UpdateControls()
    End Sub

    Sub SimpleUIValueChanged()
        SimpleUI.Save()
        UpdateControls()
    End Sub

    Sub UpdateEncoderMenu()
        Dim list As New List(Of GuiAudioEncoder) From {
            GuiAudioEncoder.Automatic
        }

        Select Case TempProfile.Params.Codec
            Case AudioCodec.None
            Case AudioCodec.AAC
                list.Add(GuiAudioEncoder.eac3to)
                list.Add(GuiAudioEncoder.fdkaac)
                list.Add(GuiAudioEncoder.ffmpeg)
                list.Add(GuiAudioEncoder.qaac)
            Case AudioCodec.AC3
                list.Add(GuiAudioEncoder.deezy)
                list.Add(GuiAudioEncoder.eac3to)
                list.Add(GuiAudioEncoder.ffmpeg)
            Case AudioCodec.DTS
                list.Add(GuiAudioEncoder.eac3to)
            Case AudioCodec.EAC3
                list.Add(GuiAudioEncoder.deezy)
                list.Add(GuiAudioEncoder.ffmpeg)
            Case AudioCodec.FLAC
                list.Add(GuiAudioEncoder.eac3to)
                list.Add(GuiAudioEncoder.ffmpeg)
            Case AudioCodec.MP3
                list.Add(GuiAudioEncoder.ffmpeg)
            Case AudioCodec.Opus
                list.Add(GuiAudioEncoder.ffmpeg)
                list.Add(GuiAudioEncoder.opusenc)
            Case AudioCodec.Vorbis
                list.Add(GuiAudioEncoder.ffmpeg)
            Case AudioCodec.W64, AudioCodec.WAV
                list.Add(GuiAudioEncoder.eac3to)
                list.Add(GuiAudioEncoder.ffmpeg)
            Case Else
                Throw New NotImplementedException("UpdateEncoderMenu")
        End Select

        mbEncoder.Clear()

        For Each item As GuiAudioEncoder In list
            mbEncoder.Add([Enum].GetName(GetType(GuiAudioEncoder), item), item)
        Next

        mbEncoder.Value = If(list.Contains(TempProfile.Params.Encoder), TempProfile.Params.Encoder, GuiAudioEncoder.Automatic)
    End Sub

    Sub UpdateControls()
        If TempProfile.ExtractCore Then
            numQuality.Enabled = False
            numBitrate.Enabled = False
        Else
            Select Case TempProfile.Params.Codec
                Case AudioCodec.Opus, AudioCodec.FLAC, AudioCodec.W64, AudioCodec.WAV, AudioCodec.DTS
                    numQuality.Enabled = False
                Case Else
                    numQuality.Enabled = TempProfile.Params.RateMode = AudioRateMode.VBR
            End Select

            numBitrate.Enabled = Not {AudioCodec.FLAC, AudioCodec.W64, AudioCodec.WAV}.ContainsEx(TempProfile.Params.Codec) AndAlso Not numQuality.Enabled
        End If

        UpdateEncoderMenu()

#Region "SamplingRate"
        If TempProfile.AudioCodec = AudioCodec.Opus Then
            mbSamplingRate.Clear()
            mbSamplingRate.Add("Original", 0)
            mbSamplingRate.Add("8000 Hz", 8000)
            mbSamplingRate.Add("12000 Hz", 12000)
            mbSamplingRate.Add("16000 Hz", 16000)
            mbSamplingRate.Add("24000 Hz", 24000)
            mbSamplingRate.Add("48000 Hz", 48000)

            If Not mbSamplingRate.Items.Cast(Of Integer).Contains(CInt(mbSamplingRate.Value)) Then
                mbSamplingRate.Value = 0
                TempProfile.Params.SamplingRate = 0
            End If
        Else
            mbSamplingRate.Clear()
            mbSamplingRate.Add("Original", 0)
            mbSamplingRate.Add("11025 Hz", 11025)
            mbSamplingRate.Add("22050 Hz", 22050)
            mbSamplingRate.Add("44100 Hz", 44100)
            mbSamplingRate.Add("48000 Hz", 48000)
            mbSamplingRate.Add("88200 Hz", 88200)
            mbSamplingRate.Add("96000 Hz", 96000)

            If Not mbSamplingRate.Items.Cast(Of Integer).Contains(CInt(mbSamplingRate.Value)) Then
                mbSamplingRate.Value = 0
                TempProfile.Params.SamplingRate = 0
            End If
        End If
#End Region

        Dim enc = TempProfile.GetEncoder()

        mbDecoder.Enabled = Not TempProfile.ExtractCore
        mbChannels.Enabled = Not TempProfile.ExtractCore AndAlso enc <> GuiAudioEncoder.opusenc AndAlso enc <> GuiAudioEncoder.deezy
        mbSamplingRate.Enabled = Not TempProfile.ExtractCore AndAlso enc <> GuiAudioEncoder.opusenc
        cbNormalize.Enabled = Not TempProfile.ExtractCore AndAlso enc <> GuiAudioEncoder.deezy
        cbCenterOptimizedStereo.Enabled = Not TempProfile.ExtractCore AndAlso {AudioDecoderMode.ffmpeg, AudioDecoderMode.Automatic}.Contains(TempProfile.Decoder) AndAlso enc <> GuiAudioEncoder.deezy AndAlso ((TempProfile.Params.Codec <> AudioCodec.Opus AndAlso TempProfile.Params.ChannelsMode = ChannelsMode._2) OrElse (TempProfile.Params.Codec = AudioCodec.Opus AndAlso TempProfile.Params.OpusencDownmix = OpusDownmix.Stereo))
        numGain.Enabled = Not TempProfile.ExtractCore

#Region "Bitrate"
        Dim setIncrement = Sub(bitrate As Double, increment As Double)
                               If CInt(numBitrate.Value) >= bitrate Then numBitrate.UpIncrement = increment
                               If CInt(numBitrate.Value) > bitrate Then numBitrate.DownIncrement = increment
                           End Sub

        If TempProfile.Params.Codec = AudioCodec.AC3 Then
            If enc = GuiAudioEncoder.deezy Then
                numBitrate.Increment = 8
                setIncrement(128, 32)
                setIncrement(256, 64)
                numBitrate.Maximum = 640
                numBitrate.Minimum = 96
                If {6}.Contains(TempProfile.SourceChannels) OrElse TempProfile.Params.DeezyChannelsDd = DeezyChannelsDd._6 Then numBitrate.Minimum = 224
            Else
                numBitrate.Increment = 32
                setIncrement(320, 64)
                numBitrate.Maximum = 1664
                numBitrate.Minimum = 32
            End If
        ElseIf TempProfile.Params.Codec = AudioCodec.EAC3 Then
            If enc = GuiAudioEncoder.deezy Then
                If TempProfile.Params.DeezyDdpMode = DeezyDdpMode.Ddp Then
                    numBitrate.Increment = 8
                    numBitrate.Maximum = 1024
                    If {1}.Contains(TempProfile.SourceChannels) OrElse {DeezyChannelsDdp._1}.Contains(TempProfile.Params.DeezyChannelsDdp) Then
                        setIncrement(128, 16)
                        setIncrement(400, 48)
                        setIncrement(448, 64)
                        setIncrement(960, 48)
                        setIncrement(1008, 16)
                        numBitrate.Minimum = 32
                    ElseIf {2}.Contains(TempProfile.SourceChannels) OrElse {DeezyChannelsDdp._2}.Contains(TempProfile.Params.DeezyChannelsDdp) Then
                        setIncrement(128, 16)
                        setIncrement(400, 48)
                        setIncrement(448, 64)
                        setIncrement(960, 48)
                        setIncrement(1008, 16)
                        numBitrate.Minimum = 96
                    ElseIf {6}.Contains(TempProfile.SourceChannels) OrElse {DeezyChannelsDdp._6}.Contains(TempProfile.Params.DeezyChannelsDdp) Then
                        setIncrement(256, 16)
                        setIncrement(400, 48)
                        setIncrement(448, 64)
                        setIncrement(960, 48)
                        setIncrement(1008, 16)
                        numBitrate.Minimum = 192
                    Else
                        numBitrate.Increment = 64
                        setIncrement(960, 48)
                        setIncrement(1008, 16)
                        numBitrate.Minimum = 384
                    End If
                ElseIf TempProfile.Params.DeezyDdpMode = DeezyDdpMode.DdpBluray Then
                    numBitrate.Increment = 256
                    setIncrement(1536, 128)
                    numBitrate.Maximum = 1664
                    numBitrate.Minimum = 768
                ElseIf TempProfile.Params.DeezyDdpMode = DeezyDdpMode.Atmos Then
                    If TempProfile.Params.DeezyChannelsAtmos = DeezyChannelsAtmos._6 Then
                        numBitrate.Increment = 64
                        setIncrement(640, 128)
                        setIncrement(768, 256)
                        numBitrate.Maximum = 1024
                        numBitrate.Minimum = 384
                    ElseIf TempProfile.Params.DeezyChannelsAtmos = DeezyChannelsAtmos._8 Then
                        numBitrate.Increment = 128
                        setIncrement(1408, 104)
                        setIncrement(1512, 24)
                        setIncrement(1536, 128)
                        numBitrate.Maximum = 1664
                        numBitrate.Minimum = 1152
                    Else
                        Throw New NotImplementedException("AudioForm.UpdateControls()")
                    End If
                Else
                    Throw New NotImplementedException("AudioForm.UpdateControls()")
                End If
            Else
                numBitrate.UpIncrement = If(CInt(numBitrate.Value) >= 320, 64, 32)
                numBitrate.DownIncrement = If(CInt(numBitrate.Value) > 320, 64, 32)
                numBitrate.Maximum = 1664
                numBitrate.Minimum = 32
            End If
        Else
            numBitrate.Increment = 1
            numBitrate.Maximum = 10_000
            numBitrate.Minimum = 1
        End If
        numBitrate.UpdateValue()
#End Region

        tbProfileName.SendMessageCue(TempProfile.Name, False)
        rtbCommandLine.SetText(TempProfile.GetCommandLine(False))
        rtbCommandLine.UpdateHeight()
    End Sub

    Sub mbCodec_ValueChangedUser() Handles mbCodec.ValueChangedUser
        TempProfile.Params.Codec = mbCodec.GetValue(Of AudioCodec)()

        Select Case TempProfile.Params.Codec
            Case AudioCodec.AAC
                Select Case TempProfile.Params.Encoder
                    Case GuiAudioEncoder.qaac, GuiAudioEncoder.Automatic
                        SetQuality(50)
                    Case GuiAudioEncoder.eac3to
                        SetQuality(0.5)
                    Case Else
                        SetQuality(3)
                End Select

                TempProfile.Params.RateMode = AudioRateMode.VBR
            Case AudioCodec.AC3, AudioCodec.EAC3
                numBitrate.Value = If(TempProfile.Channels = 6, 448, 224)
                TempProfile.Params.RateMode = AudioRateMode.CBR
            Case AudioCodec.FLAC
                numBitrate.Value = TempProfile.GetBitrate
                TempProfile.Params.RateMode = AudioRateMode.CBR
            Case AudioCodec.DTS
                numBitrate.Value = If(TempProfile.Channels = 6, 1536, 768)
                TempProfile.Params.RateMode = AudioRateMode.CBR
            Case AudioCodec.MP3
                SetQuality(4)
                TempProfile.Params.RateMode = AudioRateMode.VBR
            Case AudioCodec.Vorbis
                SetQuality(1)
                TempProfile.Params.RateMode = AudioRateMode.VBR
            Case AudioCodec.Opus
                numBitrate.Value = 80
        End Select

        UpdateBitrate()
        TempProfile.GetCommandLine(False) 'set encoder
        LoadAdvanced()
        If Not BlockUpdate Then UpdateControls()
    End Sub

    Sub SetQuality(value As Single)
        If TempProfile.Params.Codec = AudioCodec.AAC AndAlso
            TempProfile.Params.Encoder = GuiAudioEncoder.qaac OrElse
            TempProfile.Params.Encoder = GuiAudioEncoder.Automatic Then

            numQuality.Minimum = 0
            numQuality.Maximum = 127
            numQuality.Increment = 1
            numQuality.DecimalPlaces = 0
        ElseIf TempProfile.Params.Codec = AudioCodec.AAC Then
            Select Case TempProfile.Params.Encoder
                Case GuiAudioEncoder.eac3to
                    numQuality.Minimum = 0
                    numQuality.Maximum = 1
                    numQuality.Increment = 0.01
                    numQuality.DecimalPlaces = 2
                Case GuiAudioEncoder.fdkaac, GuiAudioEncoder.ffmpeg
                    numQuality.Minimum = 1
                    numQuality.Maximum = 5
                    numQuality.Increment = 1
                    numQuality.DecimalPlaces = 0
                Case Else
            End Select
        ElseIf TempProfile.Params.Codec = AudioCodec.MP3 Then
            numQuality.Minimum = 0
            numQuality.Maximum = 9
            numQuality.Increment = 1
            numQuality.DecimalPlaces = 0
        ElseIf TempProfile.Params.Codec = AudioCodec.Vorbis Then
            numQuality.Minimum = 0
            numQuality.Maximum = 10
            numQuality.Increment = 1
            numQuality.DecimalPlaces = 0
        Else
            numQuality.Minimum = 0
            numQuality.Maximum = Integer.MaxValue
            numQuality.Increment = 0.01
            numQuality.DecimalPlaces = 2
        End If

        numQuality.Value = value
    End Sub

    Sub mbSamplingRate_ValueChanged() Handles mbSamplingRate.ValueChangedUser
        TempProfile.Params.SamplingRate = mbSamplingRate.GetValue(Of Integer)()
        UpdateBitrate()
        If Not BlockUpdate Then UpdateControls()
    End Sub

    Sub mbLanguage_ValueChanged() Handles mbLanguage.ValueChangedUser
        TempProfile.Language = mbLanguage.GetValue(Of Language)()
        mbLanguage.Text = TempProfile.Language.Name
        If Not BlockUpdate Then UpdateControls()
    End Sub

    Sub tbName_TextChanged(sender As Object, e As EventArgs) Handles tbProfileName.TextChanged
        TempProfile.Name = tbProfileName.Text
        If Not BlockUpdate Then UpdateControls()
    End Sub

    Sub SaveProfile()
        Dim gap = ObjectHelp.GetCopy(TempProfile)
        Dim name = InputBox.Show("Enter the profile name", gap.Name)

        If name <> "" Then
            gap.Name = name
            gap.Clean()
            s.AudioProfiles.Add(gap)
            g.SaveAudioProfiles()
            MsgInfo("The profile was saved.")
        End If
    End Sub

    Sub LoadProfile(gap As GUIAudioProfile)
        Profile = gap
        TempProfile = ObjectHelp.GetCopy(gap)
        LoadProfile()
    End Sub

    Sub LoadProfile()
        BlockUpdate = True

        If TempProfile.Name <> TempProfile.DefaultName Then
            tbProfileName.Text = TempProfile.Name
        End If

        tbProfileName.SendMessageCue(TempProfile.Name, False)

        tbCustom.Text = TempProfile.Params.CustomSwitches
        tbStreamName.Text = TempProfile.StreamName

        cbDefaultTrack.Checked = TempProfile.Default
        cbForcedTrack.Checked = TempProfile.Forced
        cbCommentaryTrack.Checked = TempProfile.Commentary
        cbNormalize.Checked = TempProfile.Params.Normalize
        cbCenterOptimizedStereo.Checked = TempProfile.Params.CenterOptimizedStereo

        mbCodec.Value = TempProfile.Params.Codec
        mbChannels.Value = TempProfile.Params.ChannelsMode
        mbSamplingRate.Value = TempProfile.Params.SamplingRate
        mbEncoder.Value = TempProfile.Params.Encoder
        mbDecoder.Value = TempProfile.Decoder

        mbLanguage.Value = TempProfile.Language
        mbLanguage.Text = TempProfile.Language.Name

        SetQuality(TempProfile.Params.Quality)

        numBitrate.Value = TempProfile.Bitrate
        numDelay.Value = TempProfile.Delay
        numGain.Value = TempProfile.Gain

        BlockUpdate = False

        LoadAdvanced()
        UpdateControls()
    End Sub

    Sub SetBitrate(v As Integer)
        numBitrate.Value = v
    End Sub

    Sub LoadAdvanced()
        RemoveHandler SimpleUI.ValueChanged, AddressOf SimpleUIValueChanged

        Dim ui = SimpleUI
        ui.Store = TempProfile.Params
        ui.Host.Controls.Clear()

        If ui.ActivePage IsNot Nothing Then
            DirectCast(ui.ActivePage, Control).Dispose()
        End If

        Dim page = ui.CreateFlowPage()
        page.SuspendLayout()

        Dim cb As SimpleUI.SimpleUICheckBox

        Select Case TempProfile.GetEncoder
            Case GuiAudioEncoder.deezy
                Dim mbDdpMode As SimpleUI.MenuBlock(Of DeezyDdpMode)
                Dim mbChannelsDdp As SimpleUI.MenuBlock(Of DeezyChannelsDdp)
                Dim mbChannelsDdpBluray As SimpleUI.MenuBlock(Of DeezyChannelsDdpBluray)
                Dim mbChannelsAtmos As SimpleUI.MenuBlock(Of DeezyChannelsAtmos)
                Dim mbStereodownmix As SimpleUI.MenuBlock(Of DeezyStereodownmix)
                Select Case TempProfile.Params.Codec
                    Case AudioCodec.AC3
                        Dim mbChannelsDd = ui.AddMenu(Of DeezyChannelsDd)(page)
                        mbChannelsDd.Label.Text = "Channels:"
                        mbChannelsDd.Button.Expand = True
                        mbChannelsDd.Button.Value = TempProfile.Params.DeezyChannelsDd
                        mbChannelsDd.Button.SaveAction = Sub(value)
                                                             TempProfile.Params.DeezyChannelsDd = value
                                                             mbStereodownmix.Enabled = value = DeezyChannelsDd._2
                                                             'TempProfile.Params.ChannelsMode = If(TempProfile.Params.DeezyChannelsDd = DeezyChannelsDd._1, ChannelsMode._1, If(TempProfile.Params.DeezyChannelsDd = DeezyChannelsDd._2, ChannelsMode._2, If(TempProfile.Params.DeezyChannelsDd = DeezyChannelsDd._6, ChannelsMode._6, ChannelsMode.Original)))
                                                         End Sub
                    Case AudioCodec.EAC3
                        mbDdpMode = ui.AddMenu(Of DeezyDdpMode)(page)
                        mbDdpMode.Label.Text = "Mode:"
                        mbDdpMode.Button.Expand = True
                        mbDdpMode.Button.Value = TempProfile.Params.DeezyDdpMode
                        mbDdpMode.Button.SaveAction = Sub(value)
                                                          TempProfile.Params.DeezyDdpMode = value
                                                          mbChannelsDdp.Visible = value = DeezyDdpMode.Ddp
                                                          mbChannelsDdpBluray.Visible = value = DeezyDdpMode.DdpBluray
                                                          mbChannelsAtmos.Visible = value = DeezyDdpMode.Atmos
                                                          mbStereodownmix.Enabled = value = DeezyDdpMode.Ddp AndAlso TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._2
                                                          'TempProfile.Params.ChannelsMode = If(TempProfile.Params.Codec = AudioCodec.AC3 AndAlso TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._1, ChannelsMode._1, If(TempProfile.Params.Codec = AudioCodec.EAC3 AndAlso TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._2, ChannelsMode._2, If(TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._6, ChannelsMode._6, If(TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._8, ChannelsMode._8, ChannelsMode.Original))))
                                                      End Sub
                        mbChannelsDdp = ui.AddMenu(Of DeezyChannelsDdp)(page)
                        mbChannelsDdp.Label.Text = "Channels:"
                        mbChannelsDdp.Visible = TempProfile.Params.DeezyDdpMode = DeezyDdpMode.Ddp
                        mbChannelsDdp.Button.Expand = True
                        mbChannelsDdp.Button.Value = TempProfile.Params.DeezyChannelsDdp
                        mbChannelsDdp.Button.SaveAction = Sub(value)
                                                              TempProfile.Params.DeezyChannelsDdp = value
                                                              mbStereodownmix.Enabled = value = DeezyChannelsDdp._2
                                                              'TempProfile.Params.ChannelsMode = If(TempProfile.Params.Codec = AudioCodec.AC3 AndAlso TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._1, ChannelsMode._1, If(TempProfile.Params.Codec = AudioCodec.EAC3 AndAlso TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._2, ChannelsMode._2, If(TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._6, ChannelsMode._6, If(TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._8, ChannelsMode._8, ChannelsMode.Original))))
                                                          End Sub
                        mbChannelsDdpBluray = ui.AddMenu(Of DeezyChannelsDdpBluray)(page)
                        mbChannelsDdpBluray.Label.Text = "Channels:"
                        mbChannelsDdpBluray.Visible = TempProfile.Params.DeezyDdpMode = DeezyDdpMode.DdpBluray
                        mbChannelsDdpBluray.Button.Expand = True
                        mbChannelsDdpBluray.Button.Value = TempProfile.Params.DeezyChannelsDdpBluray
                        mbChannelsDdpBluray.Button.SaveAction = Sub(value)
                                                                    TempProfile.Params.DeezyChannelsDdpBluray = value
                                                                    mbStereodownmix.Enabled = TempProfile.Params.DeezyDdpMode = DeezyDdpMode.Ddp AndAlso TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._2
                                                                    'TempProfile.Params.ChannelsMode = If(TempProfile.Params.Codec = AudioCodec.AC3 AndAlso TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._1, ChannelsMode._1, If(TempProfile.Params.Codec = AudioCodec.EAC3 AndAlso TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._2, ChannelsMode._2, If(TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._6, ChannelsMode._6, If(TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._8, ChannelsMode._8, ChannelsMode.Original))))
                                                                End Sub
                        mbChannelsAtmos = ui.AddMenu(Of DeezyChannelsAtmos)(page)
                        mbChannelsAtmos.Label.Text = "Atmos Mode:"
                        mbChannelsAtmos.Visible = TempProfile.Params.DeezyDdpMode = DeezyDdpMode.Atmos
                        mbChannelsAtmos.Button.Expand = True
                        mbChannelsAtmos.Button.Value = TempProfile.Params.DeezyChannelsAtmos
                        mbChannelsAtmos.Button.SaveAction = Sub(value)
                                                                TempProfile.Params.DeezyChannelsAtmos = value
                                                                mbStereodownmix.Enabled = TempProfile.Params.DeezyDdpMode = DeezyDdpMode.Ddp AndAlso TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._2
                                                                'TempProfile.Params.ChannelsMode = If(TempProfile.Params.Codec = AudioCodec.AC3 AndAlso TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._1, ChannelsMode._1, If(TempProfile.Params.Codec = AudioCodec.EAC3 AndAlso TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._2, ChannelsMode._2, If(TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._6, ChannelsMode._6, If(TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._8, ChannelsMode._8, ChannelsMode.Original))))
                                                            End Sub
                    Case Else
                        Throw New NotImplementedException("LoadAdvanced")
                End Select

                mbStereodownmix = ui.AddMenu(Of DeezyStereodownmix)(page)
                mbStereodownmix.Enabled = TempProfile.Params.DeezyChannelsDd = DeezyChannelsDd._2 OrElse (TempProfile.Params.DeezyDdpMode = DeezyDdpMode.Ddp AndAlso TempProfile.Params.DeezyChannelsDdp = DeezyChannelsDdp._2)
                mbStereodownmix.Label.Text = "Stereo Downmix:"
                mbStereodownmix.Button.Expand = True
                mbStereodownmix.Button.Value = TempProfile.Params.DeezyStereodownmix
                mbStereodownmix.Button.SaveAction = Sub(value) TempProfile.Params.DeezyStereodownmix = value

                Dim mbDrc = ui.AddMenu(Of DeezyDrcLineMode)(page)
                mbDrc.Label.Text = "Dynamic Range Comp.:"
                mbDrc.Button.Expand = True
                mbDrc.Button.Value = TempProfile.Params.DeezyDynamicrangecompression
                mbDrc.Button.SaveAction = Sub(value) TempProfile.Params.DeezyDynamicrangecompression = value

                Dim bKeeptemp = ui.AddBool(page)
                bKeeptemp.Text = "Keep Temp Files"
                bKeeptemp.Checked = TempProfile.Params.DeezyKeeptemp
                bKeeptemp.SaveAction = Sub(value) TempProfile.Params.DeezyKeeptemp = value
            Case GuiAudioEncoder.eac3to
                Dim mbFrameRateMode = ui.AddMenu(Of AudioFrameRateMode)(page)
                mbFrameRateMode.Label.Text = "Frame Rate:"
                mbFrameRateMode.Button.Expand = True
                mbFrameRateMode.Button.Value = TempProfile.Params.FrameRateMode
                mbFrameRateMode.Button.SaveAction = Sub(value) TempProfile.Params.FrameRateMode = value

                Dim mbStereoDownmix = ui.AddMenu(Of Integer)(page)
                mbStereoDownmix.Label.Text = "Stereo Downmix:"
                mbStereoDownmix.Button.Expand = True
                mbStereoDownmix.Button.Add("Simple", 0)
                mbStereoDownmix.Button.Add("DPL II", 1)
                mbStereoDownmix.Button.Value = TempProfile.Params.eac3toStereoDownmixMode
                mbStereoDownmix.Button.SaveAction = Sub(value) TempProfile.Params.eac3toStereoDownmixMode = value

                cb = ui.AddBool(page)
                cb.Text = "Convert to 16-bit"
                cb.Checked = TempProfile.Params.eac3toDown16
                cb.SaveAction = Sub(value) TempProfile.Params.eac3toDown16 = value

                If (TempProfile.File = "" OrElse TempProfile.File.ToLowerInvariant.Contains("dts") OrElse
                    (TempProfile.Stream IsNot Nothing AndAlso TempProfile.Stream.Name.Contains("DTS"))) AndAlso
                    TempProfile.Params.Codec = AudioCodec.DTS Then

                    cb = ui.AddBool(page)
                    cb.Text = "Extract DTS core"
                    cb.Checked = TempProfile.ExtractDTSCore
                    cb.SaveAction = Sub(value)
                                        TempProfile.ExtractDTSCore = value
                                        UpdateControls()
                                    End Sub
                End If
            Case GuiAudioEncoder.ffmpeg
                Select Case TempProfile.Params.Codec
                    Case AudioCodec.DTS, AudioCodec.AC3, AudioCodec.EAC3
                    Case AudioCodec.AAC
                        Dim mbRateMode = ui.AddMenu(Of SimpleAudioRateMode)
                        mbRateMode.Text = "Rate Mode:"
                        mbRateMode.Expanded = True
                        mbRateMode.Button.Value = TempProfile.Params.SimpleRateMode
                        mbRateMode.Button.SaveAction = Sub(value) TempProfile.Params.SimpleRateMode = value

                        cb = ui.AddBool
                        cb.Text = "Use fdk-aac"
                        cb.Property = NameOf(TempProfile.Params.ffmpegLibFdkAAC)
                    Case AudioCodec.FLAC
                        Dim mbBitDepth = ui.AddMenu(Of FlacBitDepth)
                        mbBitDepth.Text = "Bit Depth:"
                        mbBitDepth.Expanded = True
                        mbBitDepth.Button.Value = TempProfile.Params.ffmpegFlacBitDepth
                        mbBitDepth.Button.SaveAction = Sub(value)
                                                           TempProfile.Params.ffmpegFlacBitDepth = value
                                                           UpdateBitrate()
                                                           UpdateControls()
                                                       End Sub
                    Case AudioCodec.Opus
                        Dim mbRateMode = ui.AddMenu(Of OpusRateMode)
                        mbRateMode.Text = "Rate Mode:"
                        mbRateMode.Expanded = True
                        mbRateMode.Button.Value = TempProfile.Params.ffmpegOpusRateMode
                        mbRateMode.Button.SaveAction = Sub(value) TempProfile.Params.ffmpegOpusRateMode = value

                        Dim mbOpusApp = ui.AddMenu(Of OpusApp)
                        mbOpusApp.Text = "Application Type:"
                        mbOpusApp.Expanded = True
                        mbOpusApp.Button.Value = TempProfile.Params.ffmpegOpusApp
                        mbOpusApp.Button.SaveAction = Sub(value) TempProfile.Params.ffmpegOpusApp = value

                        Dim frame = ui.AddMenu(Of Double)
                        frame.Text = "Frame Duration:"
                        frame.Expanded = True
                        frame.Add("2.5", 2.5)
                        frame.Add("5", 5)
                        frame.Add("10", 10)
                        frame.Add("20", 20)
                        frame.Add("40", 40)
                        frame.Add("60", 60)
                        frame.Add("80", 80)
                        frame.Add("100", 100)
                        frame.Add("120", 120)
                        frame.Property = NameOf(TempProfile.Params.ffmpegOpusFrame)

                        Dim map = ui.AddMenu(Of Integer)
                        map.Text = "Mapping Family:"
                        map.Expanded = True
                        map.Add("-1", -1)
                        map.Add("0", 0)
                        map.Add("1", 1)
                        map.Add("255", 255)
                        map.Property = NameOf(TempProfile.Params.ffmpegOpusMap)

                        Dim comp = ui.AddNum(page)
                        comp.Text = "Compression Level:"
                        comp.Config = {0, 10, 1}
                        comp.NumEdit.Value = TempProfile.Params.ffmpegOpusCompress
                        comp.NumEdit.SaveAction = Sub(value) TempProfile.Params.ffmpegOpusCompress = CInt(value)

                        Dim packet = ui.AddNum(page)
                        packet.Text = "Packet Loss:"
                        packet.Config = {0, 100, 1}
                        packet.NumEdit.Value = TempProfile.Params.ffmpegOpusPacket
                        packet.NumEdit.SaveAction = Sub(value) TempProfile.Params.ffmpegOpusPacket = CInt(value)
                    Case AudioCodec.W64, AudioCodec.WAV
                        Dim mDepth = ui.AddMenu(Of WaveBitDepth)
                        mDepth.Text = "Bit Depth:"
                        mDepth.Expanded = True
                        mDepth.Button.Value = TempProfile.Params.ffmpegWaveBitDepth
                        mDepth.Button.SaveAction = Sub(val)
                                                       TempProfile.Params.ffmpegWaveBitDepth = val
                                                       UpdateBitrate()
                                                       UpdateControls()
                                                   End Sub
                    Case Else
                        If Not {AudioCodec.WAV, AudioCodec.W64, AudioCodec.FLAC}.Contains(TempProfile.Params.Codec) Then
                            Dim mbRateMode = ui.AddMenu(Of AudioRateMode)
                            mbRateMode.Text = "Rate Mode:"
                            mbRateMode.Expanded = True
                            mbRateMode.Button.Value = TempProfile.Params.RateMode
                            mbRateMode.Button.SaveAction = Sub(value) TempProfile.Params.RateMode = value
                        End If
                End Select

                If (TempProfile.File = "" OrElse TempProfile.File.ToLowerInvariant.Contains("dts") OrElse
                    (TempProfile.Stream IsNot Nothing AndAlso TempProfile.Stream.Name.Contains("DTS"))) AndAlso
                    TempProfile.Params.Codec = AudioCodec.DTS Then

                    cb = ui.AddBool(page)
                    cb.Text = "Extract DTS core"
                    cb.Checked = TempProfile.ExtractDTSCore
                    cb.SaveAction = Sub(value)
                                        TempProfile.ExtractDTSCore = value
                                        UpdateControls()
                                    End Sub
                End If
            Case GuiAudioEncoder.fdkaac
                Dim getHelpAction = Function(switch As String) Sub() CommandLineParams.ShowConsoleHelp(Package.fdkaac, {switch})

                Dim modeMenu = ui.AddMenu(Of SimpleAudioRateMode)
                modeMenu.Text = "Rate Mode:"
                modeMenu.Expanded = True
                modeMenu.HelpAction = getHelpAction("--bitrate-mode")
                modeMenu.Button.Value = TempProfile.Params.SimpleRateMode
                modeMenu.Button.SaveAction = Sub(value)
                                                 TempProfile.Params.SimpleRateMode = value
                                                 UpdateBitrate()
                                             End Sub

                Dim profileMenu = ui.AddMenu(Of Integer)
                profileMenu.Text = "Profile:"
                profileMenu.Expanded = True
                profileMenu.HelpAction = getHelpAction("--profile")
                profileMenu.Add("AAC LC", 2)
                profileMenu.Add("HE-AAC SBR", 5)
                profileMenu.Add("HE-AAC SBR+PS", 29)
                profileMenu.Add("AAC LD", 23)
                profileMenu.Add("AAC ELD", 39)
                profileMenu.Property = NameOf(TempProfile.Params.fdkaacProfile)

                Dim lowDelaySBR = ui.AddMenu(Of Integer)
                lowDelaySBR.Text = "Lowdelay SBR:"
                lowDelaySBR.Expanded = True
                lowDelaySBR.HelpAction = getHelpAction("--lowdelay-sbr")
                lowDelaySBR.Add("ELD SBR auto configuration", -1)
                lowDelaySBR.Add("Disable SBR on ELD", 0)
                lowDelaySBR.Add("Enable SBR on ELD", 1)
                lowDelaySBR.Property = NameOf(TempProfile.Params.fdkaacLowDelaySBR)

                Dim sbrRatio = ui.AddMenu(Of Integer)
                sbrRatio.Text = "SBR Ratio:"
                sbrRatio.Expanded = True
                sbrRatio.HelpAction = getHelpAction("--sbr-ratio")
                sbrRatio.Add("Library Default", 0)
                sbrRatio.Add("Downsampled SBR (ELD+SBR default)", 1)
                sbrRatio.Add("Dual-rate SBR (HE-AAC default)", 2)
                sbrRatio.Property = NameOf(TempProfile.Params.fdkaacSbrRatio)

                Dim gaplessMode = ui.AddMenu(Of Integer)
                gaplessMode.Text = "Gapless Mode:"
                gaplessMode.Expanded = True
                gaplessMode.HelpAction = getHelpAction("--gapless-mode")
                gaplessMode.Add("iTunSMPB", 0)
                gaplessMode.Add("ISO Standard (EDTS And SGPD)", 1)
                gaplessMode.Add("Both", 2)
                gaplessMode.Property = NameOf(TempProfile.Params.fdkaacGaplessMode)

                Dim transportFormat = ui.AddMenu(Of Integer)
                transportFormat.Text = "Transport Format:"
                transportFormat.Expanded = True
                transportFormat.HelpAction = getHelpAction("--transport-format")
                transportFormat.Add("M4A", 0)
                transportFormat.Add("ADIF", 1)
                transportFormat.Add("ADTS", 2)
                transportFormat.Add("LATM MCP=1", 6)
                transportFormat.Add("LATM MCP=0", 7)
                transportFormat.Add("LOAS/LATM (LATM within LOAS)", 10)
                transportFormat.Property = NameOf(TempProfile.Params.fdkaacTransportFormat)

                Dim n = ui.AddNum
                n.Text = "Bandwidth:"
                n.HelpAction = getHelpAction("--bandwidth")
                n.Property = NameOf(TempProfile.Params.fdkaacBandwidth)

                cb = ui.AddBool
                cb.Text = "Afterburner"
                cb.HelpAction = getHelpAction("--afterburner")
                cb.Property = NameOf(TempProfile.Params.fdkaacAfterburner)

                cb = ui.AddBool
                cb.Text = "Add CRC Check on ADTS header"
                cb.HelpAction = getHelpAction("--adts-crc-check")
                cb.Property = NameOf(TempProfile.Params.fdkaacAdtsCrcCheck)

                cb = ui.AddBool
                cb.Text = "Header Period"
                cb.HelpAction = getHelpAction("--header-period")
                cb.Property = NameOf(TempProfile.Params.fdkaacHeaderPeriod)

                cb = ui.AddBool
                cb.Text = "Include SBR Delay"
                cb.HelpAction = getHelpAction("--include-sbr-delay")
                cb.Property = NameOf(TempProfile.Params.fdkaacIncludeSbrDelay)

                cb = ui.AddBool
                cb.Text = "Place moov box before mdat box"
                cb.HelpAction = getHelpAction("--moov-before-mdat")
                cb.Property = NameOf(TempProfile.Params.fdkaacMoovBeforeMdat)
            Case GuiAudioEncoder.qaac
                Dim mbMode = ui.AddMenu(Of Integer)
                mbMode.Text = "Mode:"
                mbMode.Expanded = True
                mbMode.Add("True VBR", 0)
                mbMode.Add("Constrained VBR", 1)
                mbMode.Add("ABR", 2)
                mbMode.Add("CBR", 3)
                mbMode.Button.Value = TempProfile.Params.qaacRateMode
                mbMode.Button.SaveAction = Sub(value)
                                               TempProfile.Params.qaacRateMode = value
                                               TempProfile.Params.RateMode = If(TempProfile.Params.qaacRateMode = 0, AudioRateMode.VBR, AudioRateMode.CBR)
                                               UpdateBitrate()
                                           End Sub

                Dim mbQuality = ui.AddMenu(Of Integer)(page)
                mbQuality.Label.Text = "Quality:"
                mbQuality.Button.Expand = True
                mbQuality.Button.Add("Low", 0)
                mbQuality.Button.Add("Medium", 1)
                mbQuality.Button.Add("High", 2)
                mbQuality.Button.Value = TempProfile.Params.qaacQuality
                mbQuality.Button.SaveAction = Sub(value) TempProfile.Params.qaacQuality = value

                Dim num = ui.AddNum(page)
                num.Text = "Lowpass:"
                num.Config = {0, Integer.MaxValue}
                num.NumEdit.Value = TempProfile.Params.qaacLowpass
                num.NumEdit.SaveAction = Sub(value) TempProfile.Params.qaacLowpass = CInt(value)

                Dim he = ui.AddBool(page)
                he.Text = "High Efficiency"
                he.Checked = TempProfile.Params.qaacHE
                he.SaveAction = Sub(value) TempProfile.Params.qaacHE = value

                AddHandler he.CheckedChanged, Sub() If he.Checked AndAlso mbMode.Button.Value = 0 Then mbMode.Button.Value = 1
                AddHandler mbMode.Button.ValueChangedUser, Sub() If mbMode.Button.Value = 0 Then he.Checked = False

                cb = ui.AddBool(page)
                cb.Text = "No delay"
                cb.Checked = TempProfile.Params.qaacNoDelay
                cb.SaveAction = Sub(value) TempProfile.Params.qaacNoDelay = value

                cb = ui.AddBool(page)
                cb.Text = "No dither when quantizing to lower bit depth"
                cb.Checked = TempProfile.Params.qaacNoDither
                cb.SaveAction = Sub(value) TempProfile.Params.qaacNoDither = value
            Case GuiAudioEncoder.opusenc
                Dim mbMode = ui.AddMenu(Of OpusRateMode)
                mbMode.Text = "Mode:"
                mbMode.Expanded = True
                mbMode.Button.Value = TempProfile.Params.OpusencOpusRateMode
                mbMode.Button.SaveAction = Sub(value)
                                               TempProfile.Params.OpusencOpusRateMode = value
                                               TempProfile.Params.RateMode = If(TempProfile.Params.OpusencOpusRateMode = (OpusRateMode.VBR Or OpusRateMode.CVBR), AudioRateMode.VBR, AudioRateMode.CBR)
                                               UpdateBitrate()
                                           End Sub

                Dim mbTune = ui.AddMenu(Of OpusTune)(page)
                mbTune.Label.Text = "Tune:"
                mbTune.Button.Expand = True
                mbTune.Button.Value = TempProfile.Params.OpusencTune
                mbTune.Button.SaveAction = Sub(value) TempProfile.Params.OpusencTune = value

                Dim mbDownmix = ui.AddMenu(Of OpusDownmix)(page)
                mbDownmix.Label.Text = "Downmix:"
                mbDownmix.Button.Expand = True
                mbDownmix.Button.Value = TempProfile.Params.OpusencDownmix
                mbDownmix.Button.SaveAction = Sub(value)
                                                  TempProfile.Params.OpusencDownmix = value
                                                  'TempProfile.Params.ChannelsMode = If(TempProfile.Params.OpusencDownmix = OpusDownmix.Mono, ChannelsMode._1, If(TempProfile.Params.OpusencDownmix = OpusDownmix.Stereo, ChannelsMode._2, ChannelsMode.Original))
                                              End Sub

                Dim mbFramesize = ui.AddMenu(Of OpusFramesize)(page)
                mbFramesize.Label.Text = "Framesize in ms:"
                mbFramesize.Button.Expand = True
                mbFramesize.Button.Value = TempProfile.Params.OpusencFramesize
                mbFramesize.Button.SaveAction = Sub(value) TempProfile.Params.OpusencFramesize = value

                Dim numExpectloss = ui.AddNum(page)
                numExpectloss.Text = "Expect Packet Loss in %:"
                numExpectloss.Config = {0, 100, 1, 0}
                numExpectloss.NumEdit.Value = TempProfile.Params.OpusencExpectloss
                numExpectloss.NumEdit.SaveAction = Sub(value) TempProfile.Params.OpusencExpectloss = CInt(value)

                Dim numComplexity = ui.AddNum(page)
                numComplexity.Text = "Complexity (10: best):"
                numComplexity.Config = {0, 10, 1, 0}
                numComplexity.NumEdit.Value = TempProfile.Params.OpusencComp
                numComplexity.NumEdit.SaveAction = Sub(value) TempProfile.Params.OpusencComp = CInt(value)

                Dim numMaxdelay = ui.AddNum(page)
                numMaxdelay.Text = "Max Delay in ms:"
                numMaxdelay.Config = {0, 1000, 1, 0}
                numMaxdelay.NumEdit.Value = TempProfile.Params.OpusencMaxdelay
                numMaxdelay.NumEdit.SaveAction = Sub(value) TempProfile.Params.OpusencMaxdelay = CInt(value)

                Dim bPhaseInv = ui.AddBool(page)
                bPhaseInv.Text = "Phase Inversion"
                bPhaseInv.Checked = TempProfile.Params.OpusencPhaseinversion
                bPhaseInv.SaveAction = Sub(value) TempProfile.Params.OpusencPhaseinversion = value
        End Select

        page.ResumeLayout(True)
        AddHandler SimpleUI.ValueChanged, AddressOf SimpleUIValueChanged
    End Sub

    Sub nudBitrate_KeyUp(sender As Object, e As KeyEventArgs) Handles numBitrate.KeyUp
        Try
            Dim v = CInt(numBitrate.Text)

            If v Mod 16 = 0 Then
                numBitrate.Value = v
            End If
        Catch
        End Try
    End Sub

    Sub nudQuality_KeyUp(sender As Object, e As KeyEventArgs) Handles numQuality.KeyUp
        Try
            Dim v = CInt(numQuality.Text)
            numQuality.Value = v
        Catch
        End Try
    End Sub

    Sub Execute()
        If TempProfile.File <> "" Then
            If Not TempProfile.IsInputSupported AndAlso Not TempProfile.DecodingMode = AudioDecodingMode.Pipe Then
                MsgWarn("The input format isn't supported by the current encoder," + BR + "convert to WAV or FLAC first or enable piping in the options.")
            Else
                Using pr As New Process
                    pr.StartInfo.FileName = "cmd.exe"
                    pr.StartInfo.Arguments = "/S /K """ + TempProfile.GetCommandLine(True) + """"
                    pr.StartInfo.UseShellExecute = False
                    Proc.SetEnvironmentVariables(pr)
                    pr.Start()
                End Using
            End If
        Else
            MsgWarn("Source file is missing!")
        End If
    End Sub
    Sub tbStreamName_TextChanged(sender As Object, e As EventArgs) Handles tbStreamName.TextChanged
        TempProfile.StreamName = tbStreamName.Text
    End Sub

    Sub tbCustom_TextChanged(sender As Object, e As EventArgs) Handles tbCustom.TextChanged
        TempProfile.Params.CustomSwitches = tbCustom.Text
        If Not BlockUpdate Then UpdateControls()
    End Sub

    Sub cbDefaultTrack_CheckedChanged(sender As Object, e As EventArgs) Handles cbDefaultTrack.CheckedChanged
        TempProfile.Default = cbDefaultTrack.Checked
    End Sub

    Sub cbForcedTrack_CheckedChanged(sender As Object, e As EventArgs) Handles cbForcedTrack.CheckedChanged
        TempProfile.Forced = cbForcedTrack.Checked
    End Sub

    Sub cbCommentaryTrack_CheckedChanged(sender As Object, e As EventArgs) Handles cbCommentaryTrack.CheckedChanged
        TempProfile.Commentary = cbCommentaryTrack.Checked
    End Sub

    Sub mbEncoder_ValueChangedUser(value As Object) Handles mbEncoder.ValueChangedUser
        Dim enc = mbEncoder.GetValue(Of GuiAudioEncoder)()
        TempProfile.Params.Encoder = enc

        If enc = GuiAudioEncoder.deezy Then
            cbNormalize.Checked = False
            cbCenterOptimizedStereo.Checked = False
        End If
        mbCodec_ValueChangedUser()
    End Sub

    Sub ShowHelp()
        Dim form As New HelpForm()
        form.Doc.WriteStart(Text)
        form.Doc.WriteTips(TipProvider.GetTips, SimpleUI.ActivePage.TipProvider.GetTips)
        form.Show()
    End Sub

    Sub mbChannels_ValueChanged(value As Object) Handles mbChannels.ValueChangedUser
        TempProfile.Params.ChannelsMode = mbChannels.GetValue(Of ChannelsMode)()
        UpdateBitrate()
        If Not BlockUpdate Then UpdateControls()
    End Sub

    Sub mbConvertMode_ValueChangedUser(value As Object) Handles mbDecoder.ValueChangedUser
        TempProfile.Decoder = mbDecoder.GetValue(Of AudioDecoderMode)()
        If Not BlockUpdate Then UpdateControls()
    End Sub

    Sub bnAdvanced_Click(sender As Object, e As EventArgs) Handles bnAdvanced.Click
        Using form As New SimpleSettingsForm("Advanced Audio Options")
            form.ScaleClientSize(30, 21)
            Dim ui = form.SimpleUI
            ui.Store = TempProfile.Params

            ui.CreateFlowPage("General", True)

            Dim convFormat = ui.AddMenu(Of AudioDecodingMode)
            convFormat.Text = "Decoding Method:"
            convFormat.Button.Value = TempProfile.DecodingMode
            convFormat.Button.SaveAction = Sub(val) TempProfile.DecodingMode = val

            ui.CreateFlowPage("ffmpeg", True)

            Dim ffmpegNormalize = ui.AddMenu(Of ffmpegNormalizeMode)
            ffmpegNormalize.Text = "Normalize Method:"
            ffmpegNormalize.Property = NameOf(TempProfile.Params.ffmpegNormalizeMode)

            Dim n = ui.AddNum()
            n.Text = "Probe Size"
            n.Config = {1, 999, 5}
            n.Property = NameOf(TempProfile.Params.ProbeSize)

            n = ui.AddNum()
            n.Text = "Analyze Duration"
            n.Config = {1, 999, 5}
            n.Property = NameOf(TempProfile.Params.AnalyzeDuration)

            ui.CreateFlowPage("ffmpeg | loudnorm", True)

            ui.AddLabel("EBU R128 Loudness Normalization")

            Dim helpUrl = "https://www.ffmpeg.org/ffmpeg-filters.html#loudnorm"

            n = ui.AddNum()
            n.Text = "Integrated"
            n.Help = helpUrl
            n.Config = {0, 0, 0.1, 1}
            n.Property = NameOf(TempProfile.Params.ffmpegLoudnormIntegrated)

            n = ui.AddNum()
            n.Text = "True Peak"
            n.Help = helpUrl
            n.Config = {0, 0, 0.1, 1}
            n.Property = NameOf(TempProfile.Params.ffmpegLoudnormTruePeak)

            n = ui.AddNum()
            n.Text = "LRA"
            n.Help = helpUrl
            n.Config = {0, 0, 0.1, 1}
            n.Property = NameOf(TempProfile.Params.ffmpegLoudnormLRA)

            ui.CreateFlowPage("ffmpeg | dynaudnorm", True)

            ui.AddLabel("Dynamic Audio Normalizer")

            helpUrl = "https://www.ffmpeg.org/ffmpeg-filters.html#dynaudnorm"

            n = ui.AddNum()
            n.Text = "Frame Length"
            n.Help = helpUrl
            n.Config = {10, 8000}
            n.Property = NameOf(TempProfile.Params.ffmpegDynaudnormF)

            n = ui.AddNum()
            n.Text = "Gaus filter win size"
            n.Help = helpUrl
            n.Config = {3, 301, 2, 0}
            n.Property = NameOf(TempProfile.Params.ffmpegDynaudnormG)

            n = ui.AddNum()
            n.Text = "Target Peak"
            n.Help = helpUrl
            n.Config = {0, 0, 0.05, 2}
            n.Property = NameOf(TempProfile.Params.ffmpegDynaudnormP)

            n = ui.AddNum()
            n.Text = "Max gain factor"
            n.Help = helpUrl
            n.Config = {1, 100, 1, 1}
            n.Property = NameOf(TempProfile.Params.ffmpegDynaudnormM)

            n = ui.AddNum()
            n.Text = "Target RMS"
            n.Help = helpUrl
            n.Config = {0, 1, 0.1, 1}
            n.Property = NameOf(TempProfile.Params.ffmpegDynaudnormR)

            n = ui.AddNum()
            n.Text = "Compress factor"
            n.Help = helpUrl
            n.Config = {0, 30, 1, 1}
            n.Property = NameOf(TempProfile.Params.ffmpegDynaudnormS)

            Dim b = ui.AddBool
            b.Text = "Enable channels coupling"
            b.Help = helpUrl
            b.Property = NameOf(TempProfile.Params.ffmpegDynaudnormN)

            b = ui.AddBool
            b.Text = "Enable DC bias correction"
            b.Help = helpUrl
            b.Property = NameOf(TempProfile.Params.ffmpegDynaudnormC)

            b = ui.AddBool
            b.Text = "Enable alternative boundary mode"
            b.Help = helpUrl
            b.Property = NameOf(TempProfile.Params.ffmpegDynaudnormB)

            ui.SelectLast("last advanced audio options page")

            If form.ShowDialog() = DialogResult.OK Then
                ui.Save()
            End If

            UpdateControls()
            ui.SaveLast("last advanced audio options page")
        End Using
    End Sub

    Sub cbNormalize_CheckedChanged(sender As Object, e As EventArgs) Handles cbNormalize.CheckedChanged
        TempProfile.Params.Normalize = cbNormalize.Checked
    End Sub

    Sub cbCenterOptimizedStereo_CheckedChanged(sender As Object, e As EventArgs) Handles cbCenterOptimizedStereo.CheckedChanged
        TempProfile.Params.CenterOptimizedStereo = cbCenterOptimizedStereo.Checked
    End Sub

    Sub AudioForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        ShowHelp()
    End Sub
End Class
