
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Drawing.Drawing2D
Imports System.Globalization
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports Microsoft.VisualBasic
Imports StaxRip.UI
Imports VB6 = Microsoft.VisualBasic

Public Class MainForm
    Inherits FormBase

#Region " Designer "
    Private components As IContainer
    Private Const _filterHeight As Integer = 640

    Public WithEvents bnNext As ButtonEx
    Public WithEvents tbSourceFile As TextEdit
    Public WithEvents tbTargetFile As TextEdit
    Public WithEvents gbAssistant As GroupBoxEx
    Public WithEvents lgbFilters As LinkGroupBox
    Public WithEvents tbTargetSize As TextEdit
    Public WithEvents laBitrate As LabelEx
    Public WithEvents tbBitrate As TextEdit
    Public WithEvents lTarget1 As LabelEx
    Public WithEvents lSource1 As LabelEx
    Public WithEvents lgbResize As LinkGroupBox
    Public WithEvents lPixel As LabelEx
    Public WithEvents lPixelText As LabelEx
    Public WithEvents tbResize As TrackBar
    Public WithEvents lZoom As LabelEx
    Public WithEvents lZoomText As LabelEx
    Public WithEvents tbTargetHeight As TextEdit
    Public WithEvents tbTargetWidth As TextEdit
    Public WithEvents lTargetHeight As LabelEx
    Public WithEvents lTargetWidth As LabelEx
    Public WithEvents lDAR As LabelEx
    Public WithEvents blTargetDarText As ButtonLabel
    Public WithEvents lCrop As LabelEx
    Public WithEvents lCropText As LabelEx
    Public WithEvents lgbSource As LinkGroupBox
    Public WithEvents lSource2 As LabelEx
    Public WithEvents lgbTarget As LinkGroupBox
    Public WithEvents GroupBox1 As GroupBoxEx
    Public WithEvents laTip As LabelEx
    Public WithEvents lgbEncoder As LinkGroupBox
    Public WithEvents laTarget2 As LabelEx
    Public WithEvents MenuStrip As MenuStrip
    Public WithEvents pnEncoder As Panel
    Public WithEvents FiltersListView As StaxRip.FiltersListView
    Public WithEvents blFilesize As ButtonLabel
    Public WithEvents llMuxer As ButtonLabel
    Public WithEvents lPAR As StaxRip.UI.LabelEx
    Public WithEvents blTargetParText As ButtonLabel
    Public WithEvents lAspectRatioError As LabelEx
    Public WithEvents lAspectRatioErrorText As LabelEx
    Public WithEvents blCropText As ButtonLabel
    Public WithEvents lCrop2 As LabelEx
    Public WithEvents gbAudio As GroupBoxEx
    Public WithEvents blSourceParText As ButtonLabel
    Public WithEvents lSourcePAR As LabelEx
    Public WithEvents lSourceDar As LabelEx
    Public WithEvents lSAR As LabelEx
    Public WithEvents lSarText As LabelEx
    Friend WithEvents tlpTarget As TableLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents tlpSource As TableLayoutPanel
    Friend WithEvents tlpResizeValues As TableLayoutPanel
    Friend WithEvents tlpResize As TableLayoutPanel
    Friend WithEvents tlpAudio As TableLayoutPanel
    Friend WithEvents tlpAssistant As TableLayoutPanel
    Friend WithEvents blSourceDarText As ButtonLabel
    Friend WithEvents tlpSourceValues As TableLayoutPanel
    Public WithEvents TipProvider As StaxRip.UI.TipProvider

    '<System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.bnNext = New StaxRip.UI.ButtonEx()
        Me.gbAssistant = New StaxRip.UI.GroupBoxEx()
        Me.tlpAssistant = New TableLayoutPanel()
        Me.laTip = New StaxRip.UI.LabelEx()
        Me.gbAudio = New StaxRip.UI.GroupBoxEx()
        Me.tlpAudio = New TableLayoutPanel()
        Me.MenuStrip = New MenuStrip()
        Me.lgbTarget = New StaxRip.UI.LinkGroupBox()
        Me.tlpTarget = New TableLayoutPanel()
        Me.tbTargetFile = New StaxRip.UI.TextEdit()
        Me.laTarget2 = New StaxRip.UI.LabelEx()
        Me.tbTargetSize = New StaxRip.UI.TextEdit()
        Me.lTarget1 = New StaxRip.UI.LabelEx()
        Me.tbBitrate = New StaxRip.UI.TextEdit()
        Me.laBitrate = New StaxRip.UI.LabelEx()
        Me.blFilesize = New StaxRip.UI.ButtonLabel()
        Me.lgbSource = New StaxRip.UI.LinkGroupBox()
        Me.tlpSource = New TableLayoutPanel()
        Me.tlpSourceValues = New TableLayoutPanel()
        Me.blSourceDarText = New StaxRip.UI.ButtonLabel()
        Me.lSourcePAR = New StaxRip.UI.LabelEx()
        Me.blSourceParText = New StaxRip.UI.ButtonLabel()
        Me.lCrop = New StaxRip.UI.LabelEx()
        Me.lCropText = New StaxRip.UI.LabelEx()
        Me.lSourceDar = New StaxRip.UI.LabelEx()
        Me.tbSourceFile = New StaxRip.UI.TextEdit()
        Me.lSource1 = New StaxRip.UI.LabelEx()
        Me.lSource2 = New StaxRip.UI.LabelEx()
        Me.lgbResize = New StaxRip.UI.LinkGroupBox()
        Me.tlpResize = New TableLayoutPanel()
        Me.tlpResizeValues = New TableLayoutPanel()
        Me.blTargetDarText = New StaxRip.UI.ButtonLabel()
        Me.lAspectRatioError = New StaxRip.UI.LabelEx()
        Me.lPAR = New StaxRip.UI.LabelEx()
        Me.lZoom = New StaxRip.UI.LabelEx()
        Me.lSarText = New StaxRip.UI.LabelEx()
        Me.lPixel = New StaxRip.UI.LabelEx()
        Me.blTargetParText = New StaxRip.UI.ButtonLabel()
        Me.lAspectRatioErrorText = New StaxRip.UI.LabelEx()
        Me.lCrop2 = New StaxRip.UI.LabelEx()
        Me.blCropText = New StaxRip.UI.ButtonLabel()
        Me.lDAR = New StaxRip.UI.LabelEx()
        Me.lZoomText = New StaxRip.UI.LabelEx()
        Me.lPixelText = New StaxRip.UI.LabelEx()
        Me.lSAR = New StaxRip.UI.LabelEx()
        Me.tbResize = New TrackBar()
        Me.lTargetWidth = New StaxRip.UI.LabelEx()
        Me.tbTargetWidth = New StaxRip.UI.TextEdit()
        Me.lTargetHeight = New StaxRip.UI.LabelEx()
        Me.tbTargetHeight = New StaxRip.UI.TextEdit()
        Me.lgbFilters = New StaxRip.UI.LinkGroupBox()
        Me.FiltersListView = New StaxRip.FiltersListView()
        Me.lgbEncoder = New StaxRip.UI.LinkGroupBox()
        Me.llMuxer = New StaxRip.UI.ButtonLabel()
        Me.pnEncoder = New Panel()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.tlpMain = New TableLayoutPanel()
        Me.gbAssistant.SuspendLayout()
        Me.tlpAssistant.SuspendLayout()
        Me.gbAudio.SuspendLayout()
        Me.tlpAudio.SuspendLayout()
        Me.lgbTarget.SuspendLayout()
        Me.tlpTarget.SuspendLayout()
        Me.lgbSource.SuspendLayout()
        Me.tlpSource.SuspendLayout()
        Me.tlpSourceValues.SuspendLayout()
        Me.lgbResize.SuspendLayout()
        Me.tlpResize.SuspendLayout()
        Me.tlpResizeValues.SuspendLayout()
        CType(Me.tbResize, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.lgbFilters.SuspendLayout()
        Me.lgbEncoder.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()

        '
        'bnNext
        '
        Me.bnNext.Anchor = AnchorStyles.None
        Me.bnNext.AutoSize = False
        Me.bnNext.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.bnNext.Cursor = Cursors.Default
        Me.bnNext.FlatStyle = FlatStyle.Flat
        Me.bnNext.Location = New System.Drawing.Point(1880, 33)
        Me.bnNext.Margin = New Padding(0, 0, 20, 5)
        'Me.bnNext.MinimumSize = New System.Drawing.Size(265, 85)
        Me.bnNext.Size = New System.Drawing.Size(CInt(285 * s.UIScaleFactor), CInt(105 * s.UIScaleFactor))
        Me.bnNext.UseCompatibleTextRendering = True
        '
        'gbAssistant
        '
        Me.gbAssistant.Anchor = CType((((AnchorStyles.Top) Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.gbAssistant, 4)
        Me.gbAssistant.Controls.Add(Me.tlpAssistant)
        Me.gbAssistant.Location = New System.Drawing.Point(9, 1534)
        Me.gbAssistant.Margin = New Padding(9, 0, 9, 9)
        Me.gbAssistant.Name = "gbAssistant"
        Me.gbAssistant.Padding = New Padding(6, 0, 6, 6)
        Me.gbAssistant.Size = New System.Drawing.Size(2031, 191)
        Me.gbAssistant.TabIndex = 44
        Me.gbAssistant.TabStop = False
        Me.gbAssistant.Text = "Assistant"
        '
        'tlpAssistant
        '
        Me.tlpAssistant.Anchor = CType((((AnchorStyles.Top) Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles)
        Me.tlpAssistant.ColumnCount = 2
        Me.tlpAssistant.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0!))
        Me.tlpAssistant.ColumnStyles.Add(New ColumnStyle())
        Me.tlpAssistant.Controls.Add(Me.laTip, 0, 0)
        Me.tlpAssistant.Controls.Add(Me.bnNext, 1, 0)
        Me.tlpAssistant.Dock = DockStyle.Fill
        Me.tlpAssistant.Location = New System.Drawing.Point(6, 48)
        Me.tlpAssistant.Margin = New Padding(0)
        Me.tlpAssistant.Name = "tlpAssistant"
        Me.tlpAssistant.RowCount = 1
        Me.tlpAssistant.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0!))
        Me.tlpAssistant.Size = New System.Drawing.Size(2019, 137)
        Me.tlpAssistant.TabIndex = 62
        '
        'laTip
        '
        Me.laTip.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.laTip.Location = New System.Drawing.Point(0, 0)
        Me.laTip.Margin = New Padding(0)
        Me.laTip.Size = New System.Drawing.Size(1880, 137)
        Me.laTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbAudio
        '
        Me.gbAudio.Anchor = CType(((AnchorStyles.Top Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.gbAudio, 4)
        Me.gbAudio.Controls.Add(Me.tlpAudio)
        Me.gbAudio.Location = New System.Drawing.Point(9, 724)
        Me.gbAudio.Margin = New Padding(9, 0, 9, 0)
        Me.gbAudio.Name = "gbAudio"
        Me.gbAudio.Padding = New Padding(6, 0, 6, 6)
        Me.gbAudio.Size = New System.Drawing.Size(2031, 510)
        Me.gbAudio.AutoSize = True
        Me.gbAudio.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.gbAudio.TabIndex = 59
        Me.gbAudio.TabStop = False
        Me.gbAudio.Text = "Audio"
        '
        'tlpAudio
        '
        Me.tlpAudio.ColumnCount = 4
        Me.tlpAudio.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))
        Me.tlpAudio.ColumnStyles.Add(New ColumnStyle())
        Me.tlpAudio.ColumnStyles.Add(New ColumnStyle())
        Me.tlpAudio.ColumnStyles.Add(New ColumnStyle())
        Me.tlpAudio.Dock = DockStyle.Fill
        Me.tlpAudio.Location = New System.Drawing.Point(6, 48)
        Me.tlpAudio.Margin = New Padding(0)
        Me.tlpAudio.Name = "tlpAudio"
        Me.tlpAudio.RowCount = 0
        Me.tlpAudio.Size = New System.Drawing.Size(2019, 456)
        Me.tlpAudio.TabIndex = 62
        Me.tlpAudio.AutoSize = True
        Me.tlpAudio.AutoSizeMode = AutoSizeMode.GrowAndShrink
        '
        'MenuStrip
        '
        Me.MenuStrip.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.MenuStrip.AutoSize = False
        Me.tlpMain.SetColumnSpan(Me.MenuStrip, 4)
        Me.MenuStrip.Dock = DockStyle.None
        Me.MenuStrip.GripMargin = New Padding(2, 2, 0, 2)
        Me.MenuStrip.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Padding = New Padding(6, 6, 0, 6)
        Me.MenuStrip.Size = New System.Drawing.Size(2049, 81)
        Me.MenuStrip.TabIndex = 60
        Me.MenuStrip.Text = "MenuStrip"
        '
        'lgbTarget
        '
        Me.lgbTarget.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.lgbTarget, 2)
        Me.lgbTarget.Controls.Add(Me.tlpTarget)
        Me.lgbTarget.Location = New System.Drawing.Point(1029, 81)
        Me.lgbTarget.Margin = New Padding(6, 0, 9, 0)
        Me.lgbTarget.Name = "lgbTarget"
        Me.lgbTarget.Padding = New Padding(6, 0, 6, 6)
        Me.lgbTarget.Size = New System.Drawing.Size(1011, 357)
        Me.lgbTarget.TabIndex = 58
        Me.lgbTarget.TabStop = False
        Me.lgbTarget.Text = "Target"
        '
        'tlpTarget
        '
        Me.tlpTarget.ColumnCount = 4
        Me.tlpTarget.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0!))
        Me.tlpTarget.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0!))
        Me.tlpTarget.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0!))
        Me.tlpTarget.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0!))
        Me.tlpTarget.Controls.Add(Me.tbTargetFile, 0, 0)
        Me.tlpTarget.Controls.Add(Me.laTarget2, 0, 3)
        Me.tlpTarget.Controls.Add(Me.tbTargetSize, 1, 1)
        Me.tlpTarget.Controls.Add(Me.lTarget1, 0, 2)
        Me.tlpTarget.Controls.Add(Me.tbBitrate, 3, 1)
        Me.tlpTarget.Controls.Add(Me.laBitrate, 2, 1)
        Me.tlpTarget.Controls.Add(Me.blFilesize, 0, 1)
        Me.tlpTarget.Dock = DockStyle.Fill
        Me.tlpTarget.Location = New System.Drawing.Point(6, 48)
        Me.tlpTarget.Margin = New Padding(0)
        Me.tlpTarget.Name = "tlpTarget"
        Me.tlpTarget.RowCount = 4
        Me.tlpTarget.RowStyles.Add(New RowStyle(SizeType.Percent, 25.0!))
        Me.tlpTarget.RowStyles.Add(New RowStyle(SizeType.Percent, 25.0!))
        Me.tlpTarget.RowStyles.Add(New RowStyle(SizeType.Percent, 25.0!))
        Me.tlpTarget.RowStyles.Add(New RowStyle(SizeType.Percent, 25.0!))
        Me.tlpTarget.Size = New System.Drawing.Size(999, 303)
        Me.tlpTarget.TabIndex = 62
        '
        'tbTargetFile
        '
        Me.tbTargetFile.Anchor = CType((AnchorStyles.Left Or AnchorStyles.Right), AnchorStyles)
        Me.tlpTarget.SetColumnSpan(Me.tbTargetFile, 4)
        Me.tbTargetFile.Location = New System.Drawing.Point(6, 10)
        Me.tbTargetFile.Margin = New Padding(6, FontHeight \ 4, 6, FontHeight \ 4)
        Me.tbTargetFile.Name = "tbTargetFile"
        Me.tbTargetFile.ReadOnly = False
        Me.tbTargetFile.Size = New System.Drawing.Size(987, 55)
        Me.tbTargetFile.TabIndex = 0
        '
        'laTarget2
        '
        Me.laTarget2.Anchor = CType((AnchorStyles.Left Or AnchorStyles.Right), AnchorStyles)
        Me.tlpTarget.SetColumnSpan(Me.laTarget2, 4)
        Me.laTarget2.ImeMode = ImeMode.NoControl
        Me.laTarget2.Location = New System.Drawing.Point(6, 234)
        Me.laTarget2.Margin = New Padding(6, 0, 6, 0)
        Me.laTarget2.Size = New System.Drawing.Size(987, 60)
        Me.laTarget2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTargetSize
        '
        Me.tbTargetSize.Anchor = AnchorStyles.Left
        Me.tbTargetSize.Location = New System.Drawing.Point(249, 85)
        Me.tbTargetSize.Margin = New Padding(0)
        Me.tbTargetSize.Name = "tbTargetSize"
        Me.tbTargetSize.ReadOnly = False
        Me.tbTargetSize.Size = New System.Drawing.Size(136, 55)
        Me.tbTargetSize.TabIndex = 55
        Me.tbTargetSize.TextBox.TextAlign = HorizontalAlignment.Center
        '
        'lTarget1
        '
        Me.lTarget1.Anchor = CType((AnchorStyles.Left Or AnchorStyles.Right), AnchorStyles)
        Me.tlpTarget.SetColumnSpan(Me.lTarget1, 4)
        Me.lTarget1.Location = New System.Drawing.Point(6, 157)
        Me.lTarget1.Margin = New Padding(6, 0, 6, 0)
        Me.lTarget1.Size = New System.Drawing.Size(987, 60)
        Me.lTarget1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbBitrate
        '
        Me.tbBitrate.Anchor = AnchorStyles.Left
        Me.tbBitrate.Location = New System.Drawing.Point(747, 85)
        Me.tbBitrate.Margin = New Padding(0)
        Me.tbBitrate.Name = "tbBitrate"
        Me.tbBitrate.ReadOnly = False
        Me.tbBitrate.Size = New System.Drawing.Size(139, 55)
        Me.tbBitrate.TabIndex = 41
        Me.tbBitrate.TextBox.TextAlign = HorizontalAlignment.Center
        '
        'laBitrate
        '
        Me.laBitrate.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.laBitrate.Location = New System.Drawing.Point(498, 75)
        Me.laBitrate.Margin = New Padding(0)
        Me.laBitrate.Size = New System.Drawing.Size(249, 75)
        Me.laBitrate.Text = "Video Bitrate:"
        Me.laBitrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'blFilesize
        '
        Me.blFilesize.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.blFilesize.Location = New System.Drawing.Point(6, 75)
        Me.blFilesize.Margin = New Padding(6, 0, 0, 0)
        Me.blFilesize.Name = "blFilesize"
        Me.blFilesize.Size = New System.Drawing.Size(243, 75)
        Me.blFilesize.TabIndex = 59
        Me.blFilesize.TabStop = True
        Me.blFilesize.Text = "Size:"
        Me.blFilesize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lgbSource
        '
        Me.lgbSource.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.lgbSource, 2)
        Me.lgbSource.Controls.Add(Me.tlpSource)
        Me.lgbSource.Location = New System.Drawing.Point(9, 81)
        Me.lgbSource.Margin = New Padding(9, 0, 6, 0)
        Me.lgbSource.Name = "lgbSource"
        Me.lgbSource.Padding = New Padding(6, 0, 6, 6)
        Me.lgbSource.Size = New System.Drawing.Size(1008, 357)
        Me.lgbSource.TabIndex = 57
        Me.lgbSource.TabStop = False
        Me.lgbSource.Text = "Source"
        '
        'tlpSource
        '
        Me.tlpSource.ColumnCount = 1
        Me.tlpSource.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0!))
        Me.tlpSource.Controls.Add(Me.tlpSourceValues, 0, 3)
        Me.tlpSource.Controls.Add(Me.tbSourceFile, 0, 0)
        Me.tlpSource.Controls.Add(Me.lSource1, 0, 1)
        Me.tlpSource.Controls.Add(Me.lSource2, 0, 2)
        Me.tlpSource.Dock = DockStyle.Fill
        Me.tlpSource.Location = New System.Drawing.Point(6, 48)
        Me.tlpSource.Margin = New Padding(0)
        Me.tlpSource.Name = "tlpSource"
        Me.tlpSource.RowCount = 4
        Me.tlpSource.RowStyles.Add(New RowStyle(SizeType.Percent, 25.0!))
        Me.tlpSource.RowStyles.Add(New RowStyle(SizeType.Percent, 25.0!))
        Me.tlpSource.RowStyles.Add(New RowStyle(SizeType.Percent, 25.0!))
        Me.tlpSource.RowStyles.Add(New RowStyle(SizeType.Percent, 25.0!))
        Me.tlpSource.Size = New System.Drawing.Size(996, 303)
        Me.tlpSource.TabIndex = 62
        '
        'tlpSourceValues
        '
        Me.tlpSourceValues.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.tlpSourceValues.ColumnCount = 7
        Me.tlpSourceValues.ColumnStyles.Add(New ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))
        Me.tlpSourceValues.Controls.Add(Me.blSourceDarText, 4, 0)
        Me.tlpSourceValues.Controls.Add(Me.lSourcePAR, 3, 0)
        Me.tlpSourceValues.Controls.Add(Me.blSourceParText, 2, 0)
        Me.tlpSourceValues.Controls.Add(Me.lCrop, 1, 0)
        Me.tlpSourceValues.Controls.Add(Me.lCropText, 0, 0)
        Me.tlpSourceValues.Controls.Add(Me.lSourceDar, 5, 0)
        Me.tlpSourceValues.Location = New System.Drawing.Point(0, 225)
        Me.tlpSourceValues.Margin = New Padding(0)
        Me.tlpSourceValues.Name = "tlpSourceValues"
        Me.tlpSourceValues.RowCount = 1
        Me.tlpSourceValues.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0!))
        Me.tlpSourceValues.Size = New System.Drawing.Size(996, 78)
        Me.tlpSourceValues.TabIndex = 1
        '
        'blSourceDarText
        '
        Me.blSourceDarText.Anchor = AnchorStyles.None
        Me.blSourceDarText.AutoSize = True
        Me.blSourceDarText.Location = New System.Drawing.Point(446, 15)
        Me.blSourceDarText.Name = "blSourceDarText"
        Me.blSourceDarText.Size = New System.Drawing.Size(97, 48)
        Me.blSourceDarText.TabIndex = 51
        Me.blSourceDarText.Text = "DAR:"
        Me.blSourceDarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourcePAR
        '
        Me.lSourcePAR.Anchor = AnchorStyles.None
        Me.lSourcePAR.AutoSize = True
        Me.lSourcePAR.Location = New System.Drawing.Point(400, 15)
        Me.lSourcePAR.Margin = New Padding(9, 0, 9, 0)
        Me.lSourcePAR.Size = New System.Drawing.Size(34, 48)
        Me.lSourcePAR.Text = "-"
        Me.lSourcePAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'blSourceParText
        '
        Me.blSourceParText.Anchor = AnchorStyles.None
        Me.blSourceParText.AutoSize = True
        Me.blSourceParText.Location = New System.Drawing.Point(291, 15)
        Me.blSourceParText.Margin = New Padding(9, 0, 9, 0)
        Me.blSourceParText.Name = "blSourceParText"
        Me.blSourceParText.Size = New System.Drawing.Size(91, 48)
        Me.blSourceParText.TabIndex = 45
        Me.blSourceParText.TabStop = True
        Me.blSourceParText.Text = "PAR:"
        Me.blSourceParText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lCrop
        '
        Me.lCrop.Anchor = AnchorStyles.None
        Me.lCrop.AutoSize = True
        Me.lCrop.Location = New System.Drawing.Point(120, 15)
        Me.lCrop.Margin = New Padding(9, 0, 9, 0)
        Me.lCrop.Size = New System.Drawing.Size(153, 48)
        Me.lCrop.Text = "disabled"
        Me.lCrop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lCropText
        '
        Me.lCropText.Anchor = AnchorStyles.None
        Me.lCropText.AutoSize = True
        Me.lCropText.Location = New System.Drawing.Point(3, 15)
        Me.lCropText.Size = New System.Drawing.Size(105, 48)
        Me.lCropText.TabStop = True
        Me.lCropText.Text = "Crop:"
        Me.lCropText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourceDar
        '
        Me.lSourceDar.Anchor = AnchorStyles.None
        Me.lSourceDar.AutoSize = True
        Me.lSourceDar.Location = New System.Drawing.Point(549, 15)
        Me.lSourceDar.Size = New System.Drawing.Size(34, 48)
        Me.lSourceDar.Text = "-"
        Me.lSourceDar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSourceFile
        '
        Me.tbSourceFile.Anchor = CType((AnchorStyles.Left Or AnchorStyles.Right), AnchorStyles)
        Me.tbSourceFile.Location = New System.Drawing.Point(6, 10)
        Me.tbSourceFile.Margin = New Padding(6, FontHeight \ 4, 6, FontHeight \ 4)
        Me.tbSourceFile.Name = "tbSourceFile"
        Me.tbSourceFile.ReadOnly = False
        Me.tbSourceFile.Size = New System.Drawing.Size(984, 55)
        Me.tbSourceFile.TabIndex = 2
        '
        'lSource1
        '
        Me.lSource1.Anchor = CType((AnchorStyles.Left Or AnchorStyles.Right), AnchorStyles)
        Me.lSource1.Location = New System.Drawing.Point(6, 84)
        Me.lSource1.Margin = New Padding(6, 0, 6, 0)
        Me.lSource1.Size = New System.Drawing.Size(984, 57)
        Me.lSource1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSource2
        '
        Me.lSource2.Anchor = CType((AnchorStyles.Left Or AnchorStyles.Right), AnchorStyles)
        Me.lSource2.Location = New System.Drawing.Point(6, 159)
        Me.lSource2.Margin = New Padding(6, 0, 6, 0)
        Me.lSource2.Size = New System.Drawing.Size(984, 57)
        Me.lSource2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lgbResize
        '
        Me.lgbResize.Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
        Me.tlpMain.SetColumnSpan(Me.lgbResize, 2)
        Me.lgbResize.Controls.Add(Me.tlpResize)
        Me.lgbResize.Location = New System.Drawing.Point(688, 438)
        Me.lgbResize.Margin = New Padding(6, 0, 6, 0)
        Me.lgbResize.Name = "lgbResize"
        Me.lgbResize.Padding = New Padding(6, 0, 6, 6)
        Me.lgbResize.Size = New System.Drawing.Size(670, _filterHeight)
        Me.lgbResize.TabIndex = 55
        Me.lgbResize.TabStop = False
        Me.lgbResize.Text = "Resize"
        '
        'tlpResize
        '
        Me.tlpResize.ColumnCount = 4
        Me.tlpResize.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0!))
        Me.tlpResize.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0!))
        Me.tlpResize.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0!))
        Me.tlpResize.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 25.0!))
        Me.tlpResize.Controls.Add(Me.tlpResizeValues, 0, 2)
        Me.tlpResize.Controls.Add(Me.tbResize, 0, 1)
        Me.tlpResize.Controls.Add(Me.lTargetWidth, 0, 0)
        Me.tlpResize.Controls.Add(Me.tbTargetWidth, 1, 0)
        Me.tlpResize.Controls.Add(Me.lTargetHeight, 2, 0)
        Me.tlpResize.Controls.Add(Me.tbTargetHeight, 3, 0)
        Me.tlpResize.Dock = DockStyle.Fill
        Me.tlpResize.Location = New System.Drawing.Point(6, 48)
        Me.tlpResize.Name = "tlpResize"
        Me.tlpResize.RowCount = 3
        Me.tlpResize.RowStyles.Add(New RowStyle(SizeType.Absolute, 81.0!))
        Me.tlpResize.RowStyles.Add(New RowStyle(SizeType.Absolute, 81.0!))
        Me.tlpResize.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0!))
        Me.tlpResize.Size = New System.Drawing.Size(658, 232)
        Me.tlpResize.TabIndex = 63
        '
        'tlpResizeValues
        '
        Me.tlpResizeValues.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.tlpResizeValues.ColumnCount = 4
        Me.tlpResize.SetColumnSpan(Me.tlpResizeValues, 4)
        Me.tlpResizeValues.ColumnStyles.Add(New ColumnStyle())
        Me.tlpResizeValues.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.0!))
        Me.tlpResizeValues.ColumnStyles.Add(New ColumnStyle())
        Me.tlpResizeValues.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40.0!))
        Me.tlpResizeValues.Controls.Add(Me.blTargetDarText, 0, 1)
        Me.tlpResizeValues.Controls.Add(Me.lDAR, 1, 1)
        Me.tlpResizeValues.Controls.Add(Me.lPixelText, 2, 1)
        Me.tlpResizeValues.Controls.Add(Me.lPixel, 3, 1)
        Me.tlpResizeValues.Controls.Add(Me.lSarText, 0, 2)
        Me.tlpResizeValues.Controls.Add(Me.lSAR, 1, 2)
        Me.tlpResizeValues.Controls.Add(Me.lZoomText, 2, 2)
        Me.tlpResizeValues.Controls.Add(Me.lZoom, 3, 2)
        Me.tlpResizeValues.Controls.Add(Me.blTargetParText, 0, 3)
        Me.tlpResizeValues.Controls.Add(Me.lPAR, 1, 3)
        Me.tlpResizeValues.Controls.Add(Me.lAspectRatioErrorText, 2, 3)
        Me.tlpResizeValues.Controls.Add(Me.lAspectRatioError, 3, 3)
        Me.tlpResizeValues.Controls.Add(Me.blCropText, 0, 4)
        Me.tlpResizeValues.Controls.Add(Me.lCrop2, 1, 4)
        Me.tlpResize.SetColumnSpan(Me.lCrop2, 3)
        Me.tlpResizeValues.Location = New System.Drawing.Point(0, 162)
        Me.tlpResizeValues.Margin = New Padding(0)
        Me.tlpResizeValues.Name = "tlpResizeValues"
        Me.tlpResizeValues.RowCount = 11
        Me.tlpResizeValues.RowStyles.Add(New RowStyle(SizeType.Percent, 5.0!))
        Me.tlpResizeValues.RowStyles.Add(New RowStyle(SizeType.Percent, 15.0!))
        Me.tlpResizeValues.RowStyles.Add(New RowStyle(SizeType.Percent, 15.0!))
        Me.tlpResizeValues.RowStyles.Add(New RowStyle(SizeType.Percent, 15.0!))
        Me.tlpResizeValues.RowStyles.Add(New RowStyle(SizeType.Percent, 15.0!))
        Me.tlpResizeValues.RowStyles.Add(New RowStyle(SizeType.Percent, 15.0!))
        Me.tlpResizeValues.RowStyles.Add(New RowStyle(SizeType.Percent, 15.0!))
        Me.tlpResizeValues.RowStyles.Add(New RowStyle(SizeType.Percent, 5.0!))
        Me.tlpResizeValues.Size = New System.Drawing.Size(658, 70)
        Me.tlpResizeValues.TabIndex = 62
        '
        'blTargetDarText
        '
        Me.blTargetDarText.Anchor = AnchorStyles.Left
        Me.blTargetDarText.AutoSize = True
        Me.blTargetDarText.ImeMode = ImeMode.NoControl
        Me.blTargetDarText.Location = New System.Drawing.Point(0, 0)
        Me.blTargetDarText.Margin = New Padding(0)
        Me.blTargetDarText.Name = "blTargetDarText"
        Me.blTargetDarText.Size = New System.Drawing.Size(97, 23)
        Me.blTargetDarText.TabIndex = 23
        Me.blTargetDarText.Text = "DAR:"
        Me.blTargetDarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPAR
        '
        Me.lPAR.Anchor = AnchorStyles.Left
        Me.lPAR.AutoSize = True
        Me.lPAR.Location = New System.Drawing.Point(97, 46)
        Me.lPAR.Margin = New Padding(0)
        Me.lPAR.Size = New System.Drawing.Size(34, 24)
        Me.lPAR.Text = "-"
        Me.lPAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lZoom
        '
        Me.lZoom.Anchor = AnchorStyles.Left
        Me.lZoom.AutoSize = True
        Me.lZoom.Location = New System.Drawing.Point(253, 23)
        Me.lZoom.Margin = New Padding(0)
        Me.lZoom.Size = New System.Drawing.Size(34, 23)
        Me.lZoom.Text = "-"
        Me.lZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSarText
        '
        Me.lSarText.Anchor = AnchorStyles.Left
        Me.lSarText.AutoSize = True
        Me.lSarText.Location = New System.Drawing.Point(0, 23)
        Me.lSarText.Margin = New Padding(0)
        Me.lSarText.Size = New System.Drawing.Size(92, 23)
        Me.lSarText.Text = "SAR:"
        Me.lSarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPixel
        '
        Me.lPixel.Anchor = AnchorStyles.Left
        Me.lPixel.AutoSize = True
        Me.lPixel.Location = New System.Drawing.Point(253, 0)
        Me.lPixel.Margin = New Padding(0)
        Me.lPixel.Size = New System.Drawing.Size(34, 23)
        Me.lPixel.Text = "-"
        Me.lPixel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'blTargetParText
        '
        Me.blTargetParText.Anchor = AnchorStyles.Left
        Me.blTargetParText.AutoSize = True
        Me.blTargetParText.Location = New System.Drawing.Point(0, 46)
        Me.blTargetParText.Margin = New Padding(0)
        Me.blTargetParText.Name = "blTargetParText"
        Me.blTargetParText.Size = New System.Drawing.Size(91, 24)
        Me.blTargetParText.TabIndex = 51
        Me.blTargetParText.Text = "PAR:"
        Me.blTargetParText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lAspectRatioError
        '
        Me.lAspectRatioError.Anchor = AnchorStyles.Left
        Me.lAspectRatioError.AutoSize = True
        Me.lAspectRatioError.Location = New System.Drawing.Point(253, 46)
        Me.lAspectRatioError.Margin = New Padding(0)
        Me.lAspectRatioError.Size = New System.Drawing.Size(34, 24)
        Me.lAspectRatioError.Text = "-"
        Me.lAspectRatioError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lAspectRatioErrorText
        '
        Me.lAspectRatioErrorText.Anchor = AnchorStyles.Left
        Me.lAspectRatioErrorText.AutoSize = True
        Me.lAspectRatioErrorText.Location = New System.Drawing.Point(131, 46)
        Me.lAspectRatioErrorText.Margin = New Padding(0)
        Me.lAspectRatioErrorText.Size = New System.Drawing.Size(106, 24)
        Me.lAspectRatioErrorText.Text = "Error:"
        Me.lAspectRatioErrorText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lCrop2
        '
        Me.lCrop2.Anchor = AnchorStyles.Left
        Me.lCrop2.AutoSize = True
        Me.lCrop2.Location = New System.Drawing.Point(253, 46)
        Me.lCrop2.Margin = New Padding(0)
        Me.lCrop2.Size = New System.Drawing.Size(34, 24)
        Me.lCrop2.Text = "-"
        Me.lCrop2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'blCropText
        '
        Me.blCropText.Anchor = AnchorStyles.Left
        Me.blCropText.AutoSize = True
        Me.blCropText.Location = New System.Drawing.Point(131, 46)
        Me.blCropText.Margin = New Padding(0)
        Me.blCropText.Size = New System.Drawing.Size(106, 24)
        Me.blCropText.Text = "Crop:"
        Me.blCropText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.blCropText.ClickAction = AddressOf ShowCropDialog
        '
        'lDAR
        '
        Me.lDAR.Anchor = AnchorStyles.Left
        Me.lDAR.AutoSize = True
        Me.lDAR.ImeMode = ImeMode.NoControl
        Me.lDAR.Location = New System.Drawing.Point(97, 0)
        Me.lDAR.Margin = New Padding(0)
        Me.lDAR.Size = New System.Drawing.Size(34, 23)
        Me.lDAR.Text = "-"
        Me.lDAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lZoomText
        '
        Me.lZoomText.Anchor = AnchorStyles.Left
        Me.lZoomText.AutoSize = True
        Me.lZoomText.Location = New System.Drawing.Point(131, 23)
        Me.lZoomText.Margin = New Padding(0)
        Me.lZoomText.Size = New System.Drawing.Size(122, 23)
        Me.lZoomText.Text = "Zoom:"
        Me.lZoomText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPixelText
        '
        Me.lPixelText.Anchor = AnchorStyles.Left
        Me.lPixelText.AutoSize = True
        Me.lPixelText.Location = New System.Drawing.Point(131, 0)
        Me.lPixelText.Margin = New Padding(0)
        Me.lPixelText.Size = New System.Drawing.Size(102, 23)
        Me.lPixelText.Text = "Pixel:"
        Me.lPixelText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSAR
        '
        Me.lSAR.Anchor = AnchorStyles.Left
        Me.lSAR.AutoSize = True
        Me.lSAR.Location = New System.Drawing.Point(97, 23)
        Me.lSAR.Margin = New Padding(0)
        Me.lSAR.Size = New System.Drawing.Size(34, 23)
        Me.lSAR.Text = "-"
        Me.lSAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbResize
        '
        Me.tbResize.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.tbResize.AutoSize = False
        Me.tlpResize.SetColumnSpan(Me.tbResize, 4)
        Me.tbResize.Location = New System.Drawing.Point(0, 81)
        Me.tbResize.Margin = New Padding(0)
        Me.tbResize.Minimum = 0
        Me.tbResize.TickFrequency = TrackBarTicks
        Me.tbResize.Name = "tbResize"
        Me.tbResize.Size = New System.Drawing.Size(658, 81)
        Me.tbResize.TabIndex = 46
        '
        'lTargetWidth
        '
        Me.lTargetWidth.Anchor = AnchorStyles.Left
        Me.lTargetWidth.AutoSize = True
        Me.lTargetWidth.Location = New System.Drawing.Point(3, 16)
        Me.lTargetWidth.Size = New System.Drawing.Size(124, 48)
        Me.lTargetWidth.Text = "Width:"
        Me.lTargetWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTargetWidth
        '
        Me.tbTargetWidth.Anchor = AnchorStyles.Left
        Me.tbTargetWidth.BorderStyle = BorderStyle.FixedSingle
        Me.tbTargetWidth.Location = New System.Drawing.Point(167, 13)
        Me.tbTargetWidth.Name = "tbTargetWidth"
        Me.tbTargetWidth.ReadOnly = False
        Me.tbTargetWidth.Size = New System.Drawing.Size(130, 55)
        Me.tbTargetWidth.TabIndex = 39
        Me.tbTargetWidth.TextBox.TextAlign = HorizontalAlignment.Center
        '
        'lTargetHeight
        '
        Me.lTargetHeight.Anchor = AnchorStyles.Left
        Me.lTargetHeight.AutoSize = True
        Me.lTargetHeight.Location = New System.Drawing.Point(331, 16)
        Me.lTargetHeight.Size = New System.Drawing.Size(135, 48)
        Me.lTargetHeight.Text = "Height:"
        Me.lTargetHeight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTargetHeight
        '
        Me.tbTargetHeight.Anchor = AnchorStyles.Left
        Me.tbTargetHeight.BorderStyle = BorderStyle.FixedSingle
        Me.tbTargetHeight.Location = New System.Drawing.Point(495, 13)
        Me.tbTargetHeight.Name = "tbTargetHeight"
        Me.tbTargetHeight.ReadOnly = False
        Me.tbTargetHeight.Size = New System.Drawing.Size(130, 55)
        Me.tbTargetHeight.TabIndex = 40
        Me.tbTargetHeight.TextBox.TextAlign = HorizontalAlignment.Center
        '
        'lgbFilters
        '
        Me.lgbFilters.Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
        Me.lgbFilters.Controls.Add(Me.FiltersListView)
        Me.lgbFilters.Location = New System.Drawing.Point(9, 438)
        Me.lgbFilters.Margin = New Padding(9, 0, 6, 0)
        Me.lgbFilters.Name = "lgbFilters"
        Me.lgbFilters.Padding = New Padding(9, 3, 9, 9)
        Me.lgbFilters.Size = New System.Drawing.Size(667, _filterHeight)
        Me.lgbFilters.TabIndex = 53
        Me.lgbFilters.TabStop = False
        '
        'FiltersListView
        '
        Me.FiltersListView.AllowDrop = True
        Me.FiltersListView.AutoCheckMode = StaxRip.UI.AutoCheckMode.None
        Me.FiltersListView.CheckBoxes = True
        Me.FiltersListView.Dock = DockStyle.Fill
        Me.FiltersListView.FullRowSelect = True
        Me.FiltersListView.HeaderStyle = ColumnHeaderStyle.None
        Me.FiltersListView.HideSelection = False
        Me.FiltersListView.IsLoading = False
        Me.FiltersListView.Location = New System.Drawing.Point(9, 51)
        Me.FiltersListView.MultiSelect = False
        Me.FiltersListView.Name = "FiltersListView"
        Me.FiltersListView.OwnerDraw = True
        Me.FiltersListView.Size = New System.Drawing.Size(649, 226)
        Me.FiltersListView.TabIndex = 0
        Me.FiltersListView.UseCompatibleStateImageBehavior = False
        Me.FiltersListView.View = View.Details
        '
        'lgbEncoder
        '
        Me.lgbEncoder.Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
        Me.lgbEncoder.Controls.Add(Me.llMuxer)
        Me.lgbEncoder.Controls.Add(Me.pnEncoder)
        Me.lgbEncoder.Location = New System.Drawing.Point(1370, 438)
        Me.lgbEncoder.Margin = New Padding(6, 0, 9, 0)
        Me.lgbEncoder.Name = "lgbEncoder"
        Me.lgbEncoder.Padding = New Padding(9, 3, 9, 9)
        Me.lgbEncoder.Size = New System.Drawing.Size(670, _filterHeight)
        Me.lgbEncoder.TabIndex = 51
        Me.lgbEncoder.TabStop = False
        '
        'llMuxer
        '
        Me.llMuxer.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Me.llMuxer.AutoSize = True
        Me.llMuxer.Location = New System.Drawing.Point(536, 0)
        Me.llMuxer.Name = "llMuxer"
        Me.llMuxer.Size = New System.Drawing.Size(201, 48)
        Me.llMuxer.TabIndex = 1
        Me.llMuxer.TabStop = True
        Me.llMuxer.Text = "Muxer"
        Me.llMuxer.TextAlign = ContentAlignment.MiddleRight
        '
        'pnEncoder
        '
        Me.pnEncoder.Dock = DockStyle.Fill
        Me.pnEncoder.Location = New System.Drawing.Point(9, 51)
        Me.pnEncoder.Name = "pnEncoder"
        Me.pnEncoder.Size = New System.Drawing.Size(652, 226)
        Me.pnEncoder.TabIndex = 0
        '
        'tlpMain
        '
        Me.tlpMain.AutoSize = False
        Me.tlpMain.ColumnCount = 4
        Me.tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33!))
        Me.tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 375.0!))
        Me.tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 375.0!))
        Me.tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33!))
        Me.tlpMain.Controls.Add(Me.MenuStrip, 0, 0)
        Me.tlpMain.Controls.Add(Me.lgbSource, 0, 1)
        Me.tlpMain.Controls.Add(Me.lgbTarget, 2, 1)
        Me.tlpMain.Controls.Add(Me.lgbFilters, 0, 2)
        Me.tlpMain.Controls.Add(Me.lgbResize, 1, 2)
        Me.tlpMain.Controls.Add(Me.lgbEncoder, 3, 2)
        Me.tlpMain.Controls.Add(Me.gbAudio, 0, 3)
        Me.tlpMain.Controls.Add(Me.gbAssistant, 0, 4)
        Me.tlpMain.Dock = DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New Padding(0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 5
        Me.tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 81.0!))
        Me.tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 357.0!))
        Me.tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, _filterHeight))
        Me.tlpMain.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        Me.tlpMain.RowStyles.Add(New RowStyle(SizeType.Absolute, 200.0!))
        Me.tlpMain.Size = New System.Drawing.Size(2049, 2334)
        Me.tlpMain.TabIndex = 61
        '
        'MainForm
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = AutoScaleMode.Dpi
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.HelpButton = True
        Me.MainMenuStrip = Me.MenuStrip
        Me.Margin = New Padding(9, 12, 9, 12)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = $"{g.DefaultCommands.GetApplicationDetails()}"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.gbAssistant.ResumeLayout(False)
        Me.tlpAssistant.ResumeLayout(False)
        Me.tlpAssistant.PerformLayout()
        Me.gbAudio.ResumeLayout(False)
        Me.tlpAudio.ResumeLayout(False)
        Me.tlpAudio.PerformLayout()
        Me.lgbTarget.ResumeLayout(False)
        Me.lgbTarget.PerformLayout()
        Me.tlpTarget.ResumeLayout(False)
        Me.lgbSource.ResumeLayout(False)
        Me.lgbSource.PerformLayout()
        Me.tlpSource.ResumeLayout(False)
        Me.tlpSourceValues.ResumeLayout(False)
        Me.tlpSourceValues.PerformLayout()
        Me.lgbResize.ResumeLayout(False)
        Me.lgbResize.PerformLayout()
        Me.tlpResize.ResumeLayout(False)
        Me.tlpResize.PerformLayout()
        Me.tlpResizeValues.ResumeLayout(False)
        Me.tlpResizeValues.PerformLayout()
        CType(Me.tbResize, System.ComponentModel.ISupportInitialize).EndInit()
        Me.lgbFilters.ResumeLayout(False)
        Me.lgbFilters.PerformLayout()
        Me.lgbEncoder.ResumeLayout(False)
        Me.lgbEncoder.PerformLayout()
        Me.tlpMain.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.ScaleSize(44, 44)
    End Sub

#End Region

    Public WithEvents CustomMainMenu As CustomMenu
    Public WithEvents CustomSizeMenu As CustomMenu
    Public CurrentAssistantTipKey As String
    Public BlockSubtitlesItemCheck As Boolean
    Public AssistantPassed As Boolean
    Public CommandManager As New CommandManager

    Property PreviewScript As VideoScript

    Private TargetAspectRatioMenu As ContextMenuStripEx
    Private SizeContextMenuStrip As ContextMenuStripEx
    Private EncoderMenu As ContextMenuStripEx
    Private ContainerMenu As ContextMenuStripEx
    Private SourceAspectRatioMenu As ContextMenuStripEx
    Private TargetFileMenu As ContextMenuStripEx
    Private SourceFileMenu As ContextMenuStripEx
    Private NextContextMenuStrip As ContextMenuStripEx
    Private CanIgnoreTip As Boolean = True
    Private IsLoading As Boolean = True
    Private BlockBitrate, BlockSize As Boolean
    Private SkipAssistant As Boolean
    Private BlockSourceTextBoxTextChanged As Boolean
    Private AssistantClickAction As Action
    Private ThemeRefresh As Boolean
    Private Const TrackBarInterval As Integer = 16
    Private Const TrackBarTicks As Integer = 4

    Sub New(loadSettings As Boolean)
        AddHandler Application.ThreadException, AddressOf g.OnUnhandledException
        g.MainForm = Me
        If loadSettings Then g.LoadSettings()

        PowerShell.InitCode =
            "Using namespace StaxRip;" + BR +
            "Using namespace StaxRip.UI;" + BR +
            "[Reflection.Assembly]::LoadWithPartialName(""StaxRip"")"

        If s.WriteDebugLog Then
            Dim filePath = Path.Combine(Folder.Startup, "Debug.log")

            If File.Exists(filePath) Then
                File.Delete(filePath)
            End If

            Dim listener = New TextWriterTraceListener(filePath) With {
                .TraceOutputOptions = TraceOptions.ThreadId Or TraceOptions.DateTime
            }
            Trace.Listeners.Add(listener)
            Trace.AutoFlush = True
        End If

        MenuItemEx.UseTooltips = s.EnableTooltips
        Icon = g.Icon
        g.DPI = DeviceDpi

        InitializeComponent()
        RestoreClientSize(55, 36)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)

        If components Is Nothing Then
            components = New Container()
        End If

        SetTip()

        TargetAspectRatioMenu = New ContextMenuStripEx(components)
        SizeContextMenuStrip = New ContextMenuStripEx(components)
        EncoderMenu = New ContextMenuStripEx(components)
        ContainerMenu = New ContextMenuStripEx(components)
        SourceAspectRatioMenu = New ContextMenuStripEx(components)
        TargetFileMenu = New ContextMenuStripEx(components)
        SourceFileMenu = New ContextMenuStripEx(components)
        NextContextMenuStrip = New ContextMenuStripEx(components)

        tbTargetFile.TextBox.ContextMenuStrip = TargetFileMenu
        tbSourceFile.TextBox.ContextMenuStrip = SourceFileMenu

        Dim rc = "right-click"
        tbSourceFile.TextBox.SendMessageCue(rc, False)
        tbTargetFile.TextBox.SendMessageCue(rc, False)

        MenuStrip.SuspendLayout()
        MenuStrip.Font = FontManager.GetDefaultFont()

        CommandManager.AddCommandsFromObject(Me)
        CommandManager.AddCommandsFromObject(g.DefaultCommands)

        CustomMainMenu = New CustomMenu(AddressOf GetDefaultMainMenu, s.CustomMenuMainForm, CommandManager, MenuStrip)


        OpenProject(g.StartupTemplatePath)
        CustomMainMenu.AddKeyDownHandler(Me)
        CustomMainMenu.BuildMenu()
        UpdateAudioMenus()
        UpdateTargetSizeLabel()
        MenuStrip.ResumeLayout()
        SizeContextMenuStrip.SuspendLayout()

        CustomSizeMenu = New CustomMenu(AddressOf GetDefaultMenuSize,
                s.CustomMenuSize, CommandManager, SizeContextMenuStrip)

        CustomSizeMenu.AddKeyDownHandler(Me)
        CustomSizeMenu.BuildMenu()
        SizeContextMenuStrip.ResumeLayout()

        NextContextMenuStrip.Add("Add to top and open Jobs", Sub() AddJob(True, 0))
        NextContextMenuStrip.Add("Add to bottom and open Jobs", Sub() AddJob(True, -1))
        NextContextMenuStrip.Add("-")
        NextContextMenuStrip.Add("Add to top w/o opening Jobs", Sub() AddJob(False, 0))
        NextContextMenuStrip.Add("Add to bottom w/o opening Jobs", Sub() AddJob(False, -1))

        g.SetRenderer(MenuStrip)

        ThemeManager.SetCurrentTheme(s.ThemeName)
        ApplyTheme()

        AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
        AddHandler FiltersListView.Changed, AddressOf ApplyFilters
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        RemoveHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
        components?.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    Sub OnThemeChanged(theme As Theme)
        ApplyTheme()
        Assistant(False)
    End Sub

    Sub ApplyTheme()
        ApplyTheme(ThemeManager.CurrentTheme)
    End Sub

    Sub ApplyTheme(theme As Theme)
        ApplyTheme(GetAllControls(), theme)
    End Sub

    Sub ApplyTheme(controls As IEnumerable(Of Control))
        ApplyTheme(controls, ThemeManager.CurrentTheme)
    End Sub

    Sub ApplyTheme(controls As IEnumerable(Of Control), theme As Theme)
        If DesignHelp.IsDesignMode Then
            Exit Sub
        End If

        BackColor = theme.General.BackColor

        For Each control In controls.OfType(Of Label)
            If control Is laTip Then
                If CanIgnoreTip Then
                    control.BackColor = theme.MainForm.laTipBackColor
                    control.ForeColor = theme.MainForm.laTipForeColor
                Else
                    control.BackColor = theme.MainForm.laTipBackHighlightColor
                    control.ForeColor = theme.MainForm.laTipForeHighlightColor
                End If
            Else
                control.BackColor = theme.General.Controls.Label.BackColor
                control.ForeColor = theme.General.Controls.Label.ForeColor
            End If
        Next

        For Each control In controls.OfType(Of ButtonLabel)
            control.BackColor = If(TypeOf control.Parent Is UserControl, theme.General.Controls.ListView.BackColor, theme.General.Controls.ButtonLabel.BackColor)
            control.ForeColor = theme.General.Controls.ButtonLabel.ForeColor
            control.LinkColor = theme.General.Controls.ButtonLabel.LinkForeColor
            control.LinkHoverColor = theme.General.Controls.ButtonLabel.LinkForeHoverColor
        Next

        For Each control In controls.OfType(Of ButtonEx)
            control.BackColor = theme.General.Controls.Button.BackColor
            control.ForeColor = theme.General.Controls.Button.ForeColor
            control.BackDisabledColor = theme.General.Controls.Button.BackDisabledColor
            control.ForeDisabledColor = theme.General.Controls.Button.ForeDisabledColor
        Next

        For Each control In controls.OfType(Of GroupBox)
            control.BackColor = theme.General.Controls.GroupBox.BackColor
            control.ForeColor = theme.General.Controls.GroupBox.ForeColor
        Next

        For Each control In controls.OfType(Of Panel)
            control.BackColor = theme.General.Controls.Panel.BackColor
            control.ForeColor = theme.General.Controls.Panel.ForeColor
        Next

        For Each control In controls.OfType(Of TableLayoutPanel)
            control.BackColor = theme.General.Controls.TableLayoutPanel.BackColor
            control.ForeColor = theme.General.Controls.TableLayoutPanel.ForeColor
        Next

        For Each control In controls.OfType(Of TextBox)
            control.BackColor = theme.General.Controls.TextBox.BackColor
            control.ForeColor = theme.General.Controls.TextBox.ForeColor
        Next

        For Each control In controls.OfType(Of TextBoxEx)
            control.BorderColor = theme.General.Controls.TextBox.BorderColor
            control.BorderFocusedColor = theme.General.Controls.TextBox.BorderFocusedColor
        Next

        For Each control In controls.OfType(Of TextEdit)
            control.BackColor = theme.General.Controls.TextEdit.BackColor
            control.ForeColor = theme.General.Controls.TextEdit.ForeColor
            control.BorderColor = theme.General.Controls.TextEdit.BorderColor
            control.BorderFocusedColor = theme.General.Controls.TextEdit.BorderFocusedColor
        Next

        For Each control In controls.OfType(Of TrackBar)
            control.BackColor = theme.General.Controls.TrackBar.BackColor
            control.ForeColor = theme.General.Controls.TrackBar.ForeColor
        Next

        pnEncoder.BackColor = theme.General.Controls.ListView.BackColor
    End Sub

    Function GetIfoFile() As String
        Dim ret = p.SourceFile.Dir

        If New DirectoryInfo(ret).Name.EndsWithEx("_temp") Then
            ret = ret.Parent
        End If

        ret = Path.Combine(ret, p.SourceFile.FileName.DeleteRight(5) + "0.ifo")

        If File.Exists(ret) Then
            Return ret
        End If
    End Function

    Sub RenameDVDTracks()
        Dim ifoPath = GetIfoFile()

        If Not File.Exists(ifoPath) Then
            Exit Sub
        End If

        For Each i In Directory.GetFiles(p.TempDir)
            If g.IsSourceSame(i) AndAlso i.Contains("VTS") Then
                Dim regex As New Regex(" T(\d\d) ")
                Dim match = regex.Match(i)

                If match.Success Then
                    Dim dgID = CInt(match.Groups(1).Value)

                    Using mi As New MediaInfo(ifoPath)
                        For x = 0 To mi.AudioStreams.Count - 1
                            If mi.GetAudio(x, "ID/String").Contains(" (0x" & dgID & ")") Then
                                Dim stream = mi.AudioStreams(x)
                                Dim base = i.Base
                                base = regex.Replace(base, " ID" & x & " ")

                                If base.Contains("2_0ch") Then
                                    base = base.Replace("2_0ch", "2ch")
                                End If

                                If base.Contains("3_2ch") Then
                                    base = base.Replace("3_2ch", "6ch")
                                End If

                                If base.Contains(" DELAY") Then
                                    base = base.Replace(" DELAY", "")
                                End If

                                If base.Contains(" 0ms") Then
                                    base = base.Replace(" 0ms", "")
                                End If

                                If stream.Language.TwoLetterCode <> "iv" Then
                                    base += " " + stream.Language.EnglishName
                                End If

                                If stream.Title <> "" Then
                                    base += " " + stream.Title
                                End If

                                FileHelp.Move(i, Path.Combine(i.Dir, base + i.ExtFull))
                            End If
                        Next
                    End Using
                End If
            End If
        Next
    End Sub

    Function IsSaveCanceled(Optional saveProject As Boolean = True) As Boolean
        SetLastModifiedTemplate()

        If s.ApplicationExitMode <> ApplicationExitMode.Regular Then
            Select Case s.ApplicationExitMode
                Case ApplicationExitMode.BypassProjectSaving
                    Return False
                Case ApplicationExitMode.ForceProjectSaving
                    If g.ProjectPath IsNot Nothing Then
                        SaveProjectPath(g.ProjectPath)
                    End If
                    Return False
                Case Else
                    Throw New NotImplementedException()
            End Select

            If g.ProjectPath Is Nothing Then
                If Not OpenSaveProjectDialog() Then
                    Return True
                End If
            Else
                SaveProjectPath(g.ProjectPath)
            End If
        End If

        If ObjectHelp.GetCompareString(g.SavedProject) <> ObjectHelp.GetCompareString(p) Then
            'If String.IsNullOrWhiteSpace(p.SourceFile) AndAlso Log.Length > 0 Then Return False

            If s.AutoSaveProject AndAlso p.SourceFile <> "" Then
                If g.ProjectPath Is Nothing Then
                    g.ProjectPath = Path.Combine(p.TempDir, p.TargetFile.Base + ".srip")
                End If

                SaveProjectPath(g.ProjectPath)
                Return False
            End If

            Using td As New TaskDialog(Of DialogResult)
                td.Title = "Save changed project?"
                td.AddButton("Save", DialogResult.Yes)
                td.AddButton("Don't Save", DialogResult.No)
                td.AddButton("Cancel", DialogResult.Cancel)
                td.Show()
                Refresh()

                If td.SelectedValue = DialogResult.Yes Then
                    If g.ProjectPath Is Nothing Then
                        If Not OpenSaveProjectDialog() Then
                            Return True
                        End If
                    Else
                        SaveProjectPath(g.ProjectPath)
                    End If
                ElseIf td.SelectedValue = DialogResult.Cancel Then
                    Return True
                Else
                    Return False
                End If
            End Using
        End If
    End Function

    Sub UpdateRecentProjectsMenu()
        If Disposing OrElse IsDisposed Then Exit Sub

        For Each mi In CustomMainMenu.MenuItems
            If mi.CustomMenuItem?.MethodName = "DynamicMenuItem" AndAlso
                mi.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.RecentProjects) Then

                mi.DropDownItems.ClearAndDisplose

                SyncLock s.RecentProjects
                    For Each recentProj In s.RecentProjects
                        If File.Exists(recentProj) AndAlso Not recentProj.Base = "recover" Then
                            Dim name = recentProj

                            If recentProj.Length > 70 Then
                                name = "..." + name.Remove(0, name.Length - 70)
                            End If

                            mi.DropDownItems.Add(New MenuItemEx(name, Sub() LoadProject(recentProj)))
                        End If
                    Next
                End SyncLock

                Exit For
            End If
        Next
    End Sub

    <DebuggerNonUserCode>
    Sub UpdateDynamicMenuAsync()
        Task.Run(Sub()
                     Thread.Sleep(500)

                     Try
                         Invoke(Sub() UpdateDynamicMenu())
                     Catch
                     End Try
                 End Sub)
    End Sub

    Sub UpdateDynamicMenu()
        PopulateProfileMenu(DynamicMenuItemID.EncoderProfiles)
        PopulateProfileMenu(DynamicMenuItemID.MuxerProfiles)
        PopulateProfileMenu(DynamicMenuItemID.AudioProfiles)
        PopulateProfileMenu(DynamicMenuItemID.FilterSetupProfiles)

        For Each iMenuItem In CustomMainMenu.MenuItems
            If iMenuItem.CustomMenuItem.MethodName = "DynamicMenuItem" Then
                If iMenuItem.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.HelpApplications) Then
                    iMenuItem.DropDownItems.ClearAndDisplose

                    For Each pack In Package.Items.Values
                        MenuItemEx.Add(iMenuItem.DropDownItems, pack.Name.Substring(0, 1).ToUpperInvariant + " | " + pack.ID, Sub() pack.ShowHelp())
                    Next
                End If
            End If
        Next

        Application.DoEvents()
    End Sub

    Async Sub UpdateScriptsMenuAsync()
        Dim files As String() = {}
        Dim events As String() = System.Enum.GetNames(GetType(ApplicationEvent))

        Await Task.Run(Sub()
                           If Folder.Scripts.DirExists Then
                               files = Directory.GetFiles(Folder.Scripts)
                           End If
                       End Sub)

        If IsDisposed OrElse Native.GetForegroundWindow() <> Handle OrElse files.Count = 0 Then
            Exit Sub
        End If

        For Each menuItem In CustomMainMenu.MenuItems
            If menuItem.CustomMenuItem.MethodName = "DynamicMenuItem" Then
                If menuItem.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.Scripts) Then
                    menuItem.DropDownItems.ClearAndDisplose

                    For Each path In files
                        If Not events.Contains(path.FileName.Left(".")) AndAlso Not path.FileName.StartsWith("_") Then
                            MenuItemEx.Add(menuItem.DropDownItems, path.FileName.Base,
                                Sub() g.DefaultCommands.ExecutePowerShellFile(path))
                        End If
                    Next

                    menuItem.DropDownItems.Add(New ToolStripSeparator)
                    MenuItemEx.Add(menuItem.DropDownItems, "Open Script Folder", Sub() g.ShellExecute(Folder.Scripts))
                End If
            End If
        Next
    End Sub

    Async Sub UpdateTemplatesMenuAsync()
        If IsDisposed Then Exit Sub

        Dim templates As IEnumerable(Of String) = Nothing

        Await Task.Run(Sub()
                           Thread.Sleep(500)

                           For Each iFile In Directory.GetFiles(Folder.Template, "*.srip.backup")
                               FileHelp.Move(iFile, Path.Combine(iFile.Dir, "Backup", iFile.Base))
                           Next

                           templates = Directory.GetFiles(Folder.Template, "*.srip", SearchOption.AllDirectories).OrderBy(Function(x) x.Count(Function(c) c = Path.DirectorySeparatorChar)).ThenBy(Function(x) x)
                       End Sub)


        For i = 0 To CustomMainMenu.MenuItems.Count - 1
            Dim menuItem = CustomMainMenu.MenuItems(i)

            If IsDisposed Then Exit Sub
            If menuItem.CustomMenuItem.MethodName <> "DynamicMenuItem" OrElse Not menuItem.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.TemplateProjects) Then Continue For

            menuItem.DropDownItems.ClearAndDisplose()
            Dim items As New List(Of MenuItemEx)

            For Each template In templates
                Dim text = template.Replace(Folder.Template, "").Trim(Path.DirectorySeparatorChar).Replace(Path.DirectorySeparatorChar, " | ")
                text = text.Substring(0, text.LastIndexOf(".srip"))
                text += If(template = g.StartupTemplatePath, " (Startup)", "")

                MenuItemEx.Add(menuItem.DropDownItems, text, AddressOf LoadProject, template, Nothing)
            Next

            menuItem.DropDownItems.Add("-")
            MenuItemEx.Add(menuItem.DropDownItems, "Explore", Sub() g.ShellExecute(Folder.Template), "Opens the directory containing the templates.")
            MenuItemEx.Add(menuItem.DropDownItems, "Restore", AddressOf ResetTemplates, "Restores the default templates.")

            Exit For
        Next

        If IsDisposed Then Exit Sub

        Application.DoEvents()
    End Sub

    Sub ResetTemplates()
        If MsgQuestion("Restore the default templates?") = DialogResult.OK Then
            Try
                FolderHelp.Delete(Folder.Template)
                Folder.Template.ToString()
                UpdateTemplatesMenuAsync()
            Catch ex As Exception
                g.ShowException(ex)
            End Try
        End If
    End Sub

    <Command("Loads a template.")>
    Sub LoadTemplate(name As String)
        Dim templates = Directory.GetFiles(Folder.Template, "*.srip", SearchOption.AllDirectories)

        For Each template In templates
            Dim temp = template.Replace(Folder.Template, "").Trim(Path.DirectorySeparatorChar)
            Dim text1 = temp.Substring(0, temp.LastIndexOf(".srip"))
            Dim text2 = text1.Replace(Path.DirectorySeparatorChar, " | ")

            If {text1.ToLowerEx(), text2.ToLowerEx()}.Contains(name.ToLowerEx()) Then
                LoadProject(template)
                Exit Sub
            End If
        Next
    End Sub

    Function LoadTemplateWithSelectionDialog(sources As IEnumerable(Of String), timeout As Integer, Optional templateFolder As String = "") As Boolean
        Return LoadTemplateWithSelectionDialog(sources?.Join("; "), timeout, templateFolder)
    End Function

    Function LoadTemplateWithSelectionDialog(sources As String(), timeout As Integer, Optional templateFolder As String = "") As Boolean
        Return LoadTemplateWithSelectionDialog(sources?.Join("; "), timeout, templateFolder)
    End Function

    <Command("Loads a template that you can choose from via dialog.")>
    Function LoadTemplateWithSelectionDialog(source As String, timeout As Integer, Optional templateFolder As String = "") As Boolean
        If Not templateFolder.DirExists Then templateFolder = Folder.Template
        If Not templateFolder.DirExists Then Return False
        Dim directories = Directory.GetDirectories(templateFolder, "*", SearchOption.TopDirectoryOnly)
        Dim templates = Directory.GetFiles(templateFolder, "*.srip", SearchOption.TopDirectoryOnly)?.Where(Function(x) x <> g.StartupTemplatePath)?.OrderBy(Function(x) x)
        If Not templates?.Any() Then Return True

        Using td As New TaskDialog(Of String)
            td.Timeout = If(s.ShowTemplateSelectionDefault = ShowTemplateSelectionDefaultMode.None, 0, timeout)
            td.Title = "Select a template"
            td.Content = If(String.IsNullOrWhiteSpace(source),
                "Please select a template you want to use:",
                $"Please select a template you want to use for:{BR}{source}")
            td.Icon = TaskIcon.Question

            If p.SourceFile = "" Then
                td.AddCommand("Current Template", "CURRENT")
            Else
                td.AddCommand("Last set Template", "LAST")
            End If

            If ObjectHelp.GetCompareString(g.SavedProject) <> ObjectHelp.GetCompareString(p) Then
                td.AddCommand(g.StartupTemplatePath.Base() + " (Startup)", g.StartupTemplatePath)
            End If

            If templateFolder.Contains(Folder.Template + Path.DirectorySeparatorChar) Then
                td.AddCommand($". . {Path.DirectorySeparatorChar}", "UP")
            End If

            For i = 0 To templates.Count() - 1
                Dim text = templates(i).Replace(Folder.Template, "").Trim(Path.DirectorySeparatorChar).Replace(Path.DirectorySeparatorChar, $" {Path.DirectorySeparatorChar} ")
                text = text.Substring(0, text.LastIndexOf(".srip"))
                td.AddCommand(text, templates(i))
            Next

            For i = 0 To directories.Count() - 1
                Dim text = directories(i).Replace(Folder.Template, "").Trim(Path.DirectorySeparatorChar).Replace(Path.DirectorySeparatorChar, $" {Path.DirectorySeparatorChar} ")
                text += $" {Path.DirectorySeparatorChar} . ."
                td.AddCommand(text, directories(i))
            Next

            If s.ShowTemplateSelectionDefault = ShowTemplateSelectionDefaultMode.None Then
                td.AddButton("Abort", "", False)
            ElseIf s.ShowTemplateSelectionDefault = ShowTemplateSelectionDefaultMode.Abort Then
                td.AddButton("Abort", "", True)
            ElseIf s.ShowTemplateSelectionDefault = ShowTemplateSelectionDefaultMode.CurrentLast Then
                If p.SourceFile = "" Then
                    td.AddButton("Current Template", "CURRENT", True)
                Else
                    td.AddButton("Last set Template", "LAST", True)
                End If
                td.AddButton("Abort", "", False)
            ElseIf s.ShowTemplateSelectionDefault = ShowTemplateSelectionDefaultMode.Startup Then
                td.AddButton("Startup Template", g.StartupTemplatePath, True)
                td.AddButton("Abort", "", False)
            End If

            Dim selection = td.Show()

            If selection = "" Then
                Return False
            ElseIf selection = "CURRENT" Then
                Return True
            ElseIf selection = "LAST" Then
                Return OpenProject(g.LastModifiedTemplate)
            ElseIf selection = "UP" Then
                Return LoadTemplateWithSelectionDialog(source, timeout, templateFolder.Dir())
            ElseIf selection.DirExists() Then
                Return LoadTemplateWithSelectionDialog(source, timeout, selection)
            Else
                Return OpenProject(td.SelectedValue, True)
            End If
        End Using
    End Function

    <Command("Adds batch jobs for multiple files.")>
    Sub AddBatchJobs(sourcefiles As String())
        If sourcefiles Is Nothing Then Return

        For Each sourcefile In sourcefiles
            AddBatchJob(sourcefile)
        Next
    End Sub

    <Command("Adds a batch job for a single file.")>
    Sub AddBatchJob(sourcefile As String)
        Dim batchFolder = Path.Combine(Folder.Settings, "Batch Projects")

        If Not Directory.Exists(batchFolder) Then
            Directory.CreateDirectory(batchFolder)
        End If

        g.RaiseAppEvent(ApplicationEvent.BeforeJobAdding)

        sourcefile = sourcefile.TrimQuotes()
        Dim batchProject = ObjectHelp.GetCopy(Of Project)(p)
        batchProject.BatchMode = True
        batchProject.SourceFiles = {sourcefile}.ToList
        Dim splits = sourcefile.Dir.Split(Path.DirectorySeparatorChar)
        Dim joins = splits.Select(Function(x) x.Substring(0, Math.Min(x.Length, 128 \ splits.Length))).Join("-").Replace(":", "-")
        Dim jobPath = Path.Combine(batchFolder, joins + " " + p.TemplateName + " - " + sourcefile.FileName + ".srip")
        SafeSerialization.Serialize(batchProject, jobPath)
        JobManager.AddJob(sourcefile.Base, jobPath)
    End Sub

    Function LoadProject(path As String) As Boolean
        SetLastModifiedTemplate()
        Refresh()

        If Not File.Exists(path) Then
            MsgWarn("Project file not found.", $"{path}{BR}could not be found.")
            s.UpdateRecentProjects(path)
            UpdateRecentProjectsMenu()
            UpdateTemplatesMenuAsync()
            Return False
        Else
            Return OpenProject(path)
        End If
    End Function

    Function OpenSaveProjectDialog() As Boolean
        Using dialog As New SaveFileDialog
            dialog.SetInitDir(p.TempDir)

            dialog.FileName = If(p.SourceFile <> "", p.TargetFile.Base, "Untitled")

            dialog.Filter = "StaxRip Project Files (*.srip)|*.srip"

            If dialog.ShowDialog() = DialogResult.OK Then
                If Not dialog.FileName.ToLowerInvariant.EndsWith(".srip") Then
                    dialog.FileName += ".srip"
                End If

                SaveProjectPath(dialog.FileName)
                Return True
            End If
        End Using
    End Function

    Sub SetBindings(proj As Project, add As Boolean)
        SetTextBoxBinding(tbTargetFile, proj, NameOf(Project.TargetFile), add)

        RemoveHandler proj.PropertyChanged, AddressOf ProjectPropertyChanged
        If add Then
            AddHandler proj.PropertyChanged, AddressOf ProjectPropertyChanged
        End If
    End Sub

    Sub ProjectPropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        Assistant()
    End Sub

    Sub SetTextBoxBinding(tb As TextBox, obj As Object, prop As String, add As Boolean)
        tb.DataBindings.Clear()

        If add Then
            tb.DataBindings.Add(New Binding(NameOf(TextBox.Text), obj, prop, False, DataSourceUpdateMode.OnPropertyChanged))
        End If
    End Sub

    Sub SetTextBoxBinding(te As TextEdit, obj As Object, prop As String, add As Boolean)
        te.TextBox.DataBindings.Clear()

        If add Then
            te.TextBox.DataBindings.Add(New Binding(NameOf(TextBox.Text), obj, prop, False, DataSourceUpdateMode.OnPropertyChanged))
        End If
    End Sub

    Function OpenProject(Optional path As String = Nothing) As Boolean
        Return OpenProject(path, True)
    End Function

    Function OpenProject(path As String, saveCurrent As Boolean) As Boolean
        Try
            SetLastModifiedTemplate()

            If Not IsLoading AndAlso saveCurrent AndAlso IsSaveCanceled() Then Return False
            If String.IsNullOrWhiteSpace(path) OrElse Not File.Exists(path) Then path = g.StartupTemplatePath

            Try
                p = SafeSerialization.Deserialize(New Project, path)
            Catch ex As Exception
                g.ShowException(ex, "Project file failed to load", "It will be reset to defaults." + BR2 + path)
                p = New Project()
                p.Init()
            End Try

            Return OpenProject(p, path)
        Catch ex As Exception
            OpenProject(g.StartupTemplatePath)
        End Try
    End Function

    Function OpenProject(proj As Project, Optional path As String = "") As Boolean
        Try
            SetLastModifiedTemplate()

            If proj IsNot Nothing AndAlso g.LastModifiedTemplate IsNot Nothing AndAlso ObjectHelp.GetCompareString(proj).Equals(ObjectHelp.GetCompareString(g.LastModifiedTemplate)) Then
                proj = ObjectHelp.GetCopy(g.LastModifiedTemplate)
                If String.IsNullOrWhiteSpace(path) Then path = proj.TemplatePath

                If path = "" Then
                    Dim templates = Directory.GetFiles(Folder.Template, "*.srip", SearchOption.AllDirectories).Where(Function(x) x.Base() = proj.TemplateName).ToList()
                    If templates.Count = 0 Then
                        g.ShowException(New FileNotFoundException(), "Template not found", $"'{proj.TemplateName}' was not found")
                    ElseIf templates.Count = 1 Then
                        path = templates.First()
                    Else
                        g.ShowException(New Exception("Too many templates with the same name found and could not choose."))
                    End If
                End If
            ElseIf String.IsNullOrWhiteSpace(path) OrElse Not File.Exists(path) Then
                path = g.StartupTemplatePath
            End If

            p = If(proj IsNot Nothing, proj, SafeSerialization.Deserialize(New Project(), path))
            Log = p.Log

            FileHelp.Delete(IO.Path.Combine(Folder.Temp, "staxrip.log"))
            SetBindings(p, True)

            Text = $"{path.Base} - {g.DefaultCommands.GetApplicationDetails()}"

            If Not Environment.Is64BitProcess Then
                Text += " (32 bit)"
            End If

            PreviewScript = Nothing
            SkipAssistant = True

            If path.StartsWith(Folder.Template) Then
                g.ProjectPath = Nothing
                p.TemplateName = path.Base
                p.TemplatePath = path
                p.BatchMode = False
            Else
                g.ProjectPath = path
            End If

            g.SetTempDir()

            Dim width = p.TargetWidth
            Dim height = p.TargetHeight
            Dim size = p.TargetSize
            Dim bitrate = p.VideoBitrate

            FiltersListView.Load()
            FiltersListView.Items(0).Selected = True
            p.VideoEncoder.OnStateChange()

            Dim targetPath = p.TargetFile

            BlockSourceTextBoxTextChanged = True
            tbSourceFile.Text = p.SourceFile
            BlockSourceTextBoxTextChanged = False

            If p.SourceFile <> "" Then
                s.LastSourceDir = p.SourceFile.Dir
            End If

            SetAudioTracks(p)

            If p.BitrateIsFixed Then
                tbBitrate.Text = ""
                tbBitrate.Text = bitrate.ToString
            Else
                tbTargetSize.Text = ""
                tbTargetSize.Text = size.ToString()
            End If

            SetSlider()

            tbTargetWidth.Text = width.ToString
            tbTargetHeight.Text = height.ToString

            SetSavedProject()

            SkipAssistant = False

            Assistant(False)
            s.UpdateRecentProjects(path)
            UpdateRecentProjectsMenu()
            g.RaiseAppEvent(ApplicationEvent.AfterProjectLoaded)
            g.RaiseAppEvent(ApplicationEvent.AfterProjectOrSourceLoaded)
            FiltersListView.RebuildMenu()

            If p.SourceFile <> "" AndAlso Not g.VerifyRequirements Then
                Throw New AbortException
            End If

            Return True
        Catch ex As Exception
            OpenProject(g.StartupTemplatePath)
        Finally
            SkipAssistant = False
        End Try
    End Function

    Sub SetSlider()
        tbResize.Maximum = ((Calc.FixMod(p.SourceWidth, TrackBarInterval) \ 2) * 3) \ Math.Max(TrackBarInterval, p.ForcedOutputMod)
        tbResize.Maximum += If(tbResize.Maximum Mod 3 = 0, 0, 1)
        tbResize.Maximum += If(tbResize.Maximum Mod 3 = 0, 0, 1)
    End Sub

    Sub SetSavedProject()
        g.SavedProject = ObjectHelp.GetCopy(Of Project)(p)
    End Sub

    Sub SetLastModifiedTemplate()
        If p.SourceFile <> "" OrElse Not p.Log.IsEmpty Then Exit Sub

        g.LastModifiedTemplate = ObjectHelp.GetCopy(Of Project)(p)
    End Sub

    Sub SetAudioTracks(Optional proj As Project = Nothing)
        proj = If(proj, p)
        proj.AudioTracks = If(proj.AudioTracks, New List(Of AudioTrack))
        Dim availableAudioTracks = Mathf.Clamp(proj.AudioTracksAvailable, 1, 25)
        Dim sourceFile = p.LastOriginalSourceFile
        Dim streams = If(sourceFile.FileExists(), MediaInfo.GetAudioStreams(sourceFile), New List(Of AudioStream))
        Dim ateHeight = -1

        'tlpAudio.SuspendLayout()
        Try
            tlpAudio.Controls.Clear()
            tlpAudio.RowStyles.Clear()

            If proj.AudioTracks.Count > availableAudioTracks Then
                For index = proj.AudioTracks.Count - 1 To availableAudioTracks - 1 Step -1
                    proj.AudioTracks(index).TextEdit.Dispose()
                    proj.AudioTracks(index).LanguageLabel.Dispose()
                    proj.AudioTracks(index).NameLabel.Dispose()
                    proj.AudioTracks(index).EditLabel.Dispose()
                    proj.AudioTracks.RemoveAt(index)
                Next
            End If

            For i = 0 To availableAudioTracks - 1
                Dim index = i

                Dim textEditContextMenuStripEx = New ContextMenuStripEx(components)
                Dim audioTrack As AudioTrack
                Dim audioProfile As AudioProfile = If(index < proj.AudioTracks.Count AndAlso proj.AudioTracks(index)?.AudioProfile IsNot Nothing, proj.AudioTracks(index)?.AudioProfile, New MuxAudioProfile())
                If audioProfile.Streams Is Nothing OrElse Not audioProfile.Streams.Any() Then audioProfile.Streams = streams
                Dim textEdit = New AudioTextEdit(index)
                Dim languageLabel = New AudioLanguageLabel(index) With {
                    .Text = audioProfile.Language.Name
                }
                Dim nameLabel = New AudioNameButtonLabel(index) With {
                    .ContextMenuStripEx = New ContextMenuStripEx(components),
                    .ClickAction = Sub()
                                       .ContextMenuStripEx.Show(audioTrack.NameLabel, 0, 16)
                                   End Sub
                }
                Dim editLabel = New AudioEditButtonLabel(index) With {
                    .ClickAction = Sub()
                                       audioTrack.AudioProfile.EditProject()
                                       UpdateAudioMenus()
                                       UpdateSizeOrBitrate()
                                       nameLabel.Text = g.ConvertPath(audioTrack.AudioProfile.Name)
                                       languageLabel.Text = audioTrack.AudioProfile.Language.Name
                                       ShowAudioTip(audioTrack.AudioProfile)
                                   End Sub
                }

                textEdit.TextBox.ContextMenuStrip = textEditContextMenuStripEx

                TipProvider.SetTip("Opens audio settings for the current project/template", editLabel)
                TipProvider.SetTip("Shows audio profiles", nameLabel)

                Dim textEditTextChanged = Sub(sender As Object, e As EventArgs)
                                              AudioTextEditChanged(audioTrack)
                                          End Sub
                Dim textEditDoubleClick = Sub(sender As Object, e As EventArgs)
                                              AudioTextEditDoubleClick(audioTrack)
                                          End Sub
                Dim textEditMouseDown = Sub(sender As Object, e As MouseEventArgs)
                                            If e.Button = MouseButtons.Right Then
                                                UpdateAudioFileMenu(audioTrack, Sub() textEditDoubleClick(Nothing, Nothing))
                                            End If
                                        End Sub

                AddHandler textEdit.TextChanged, textEditTextChanged
                AddHandler textEdit.DoubleClick, textEditDoubleClick
                AddHandler textEdit.MouseDown, textEditMouseDown

                If proj.AudioTracks.ElementAtOrDefault(index) Is Nothing Then
                    audioTrack = New AudioTrack() With {.AudioProfile = audioProfile, .EditLabel = editLabel, .LanguageLabel = languageLabel, .NameLabel = nameLabel, .TextEdit = textEdit}
                    proj.AudioTracks.Add(audioTrack)
                Else
                    audioTrack = New AudioTrack() With {.AudioProfile = proj.AudioTracks(index).AudioProfile, .EditLabel = editLabel, .LanguageLabel = languageLabel, .NameLabel = nameLabel, .TextEdit = textEdit}
                    proj.AudioTracks(index) = audioTrack
                End If
            Next

            For index = 0 To proj.AudioTracks.Count - 1
                tlpAudio.Controls.Add(proj.AudioTracks(index).TextEdit, 0, index)
                tlpAudio.Controls.Add(proj.AudioTracks(index).LanguageLabel, 1, index)
                tlpAudio.Controls.Add(proj.AudioTracks(index).NameLabel, 2, index)
                tlpAudio.Controls.Add(proj.AudioTracks(index).EditLabel, 3, index)
            Next

        Catch ex As Exception
        Finally
            tlpAudio.RowCount = availableAudioTracks

            For Each ate In tlpAudio.Controls.OfType(Of AudioTextEdit)
                ateHeight = ate.Height + ate.Margin.Vertical
                ate.TextBox.SendMessageCue("right-click", False)
                tlpAudio.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            Next

            'tlpAudio.ResumeLayout()
            SetFormBoundaries(ateHeight)
            UpdateAudioMenus()
            PopulateProfileMenu(DynamicMenuItemID.AudioProfiles)
        End Try
    End Sub

    Sub AddAudioTracks()
        If p.AudioTracks.Count = 0 Then Exit Sub

        Dim files = g.GetFilesInTempDirAndParent().AsEnumerable()
        files = files.Where(Function(x) FileTypes.Audio.Contains(x.Ext.ToLowerInvariant()))
        files = files.Where(Function(x) g.IsSourceSame(x))
        files = files.Where(Function(x) x.Ext.ToLowerInvariant() <> "avs")
        files = files.Where(Function(x) Not x.ToLowerInvariant().Contains("_cut_"))
        files = files.Where(Function(x) Not x.ToLowerInvariant().Contains("_out"))
        files = files.Where(Function(x) Not x.ToLowerInvariant().Matches("_chunk\d+$"))
        files = files.OrderBy(Function(x) x, New StringLogicalComparer())

        Dim hqAudioFiles = files.Where(Function(x) FileTypes.AudioHQ.Contains(x.Ext()))
        Dim normalAudioFiles = files.Where(Function(x) (FileTypes.Audio.Except(FileTypes.AudioHQ)).Contains(x.Ext()))
        Dim orderedAudioFiles = hqAudioFiles.Concat(normalAudioFiles).Distinct()
        Dim groupedAudioFiles = orderedAudioFiles.GroupBy(Function(x) g.ExtractLanguageFromPath(x)).ToList()

        Dim setLanguages = p.AudioTracks.Select(Function(x) x.AudioProfile.Language).Where(Function(x) x.IsDetermined).Select(Function(x) x.ThreeLetterCode)
        Dim preferredAudios = p.PreferredAudio.ToLowerInvariant.SplitNoEmptyAndWhiteSpace(",", ";", " ").Union(setLanguages).Distinct()

        Dim audioTracks As New List(Of (FilePath As String, Language As Language, Title As String, Stream As AudioStream))

        Dim addAudioTrack = Sub(groupIndex As Integer, pathIndex As Integer)
                                If groupedAudioFiles.Count <= groupIndex Then Exit Sub
                                If groupedAudioFiles(groupIndex) Is Nothing Then Exit Sub
                                If groupedAudioFiles(groupIndex).Count() <= pathIndex Then Exit Sub
                                If groupedAudioFiles(groupIndex)(pathIndex) = "" Then Exit Sub

                                Dim filePath = groupedAudioFiles(groupIndex)(pathIndex)
                                Dim baseName = UnescapeIllegalFileSysChars(filePath.Base)
                                Dim titleMatch = Regex.Match(baseName, "\{(.+)\}", RegexOptions.IgnoreCase)
                                Dim title = If(titleMatch.Success, titleMatch.Groups(1).Value, "")

                                If Not audioTracks?.Where(Function(x) x.FilePath = filePath)?.Any() Then
                                    audioTracks.Add((filePath, groupedAudioFiles(groupIndex).Key, title, Nothing))
                                End If
                            End Sub

        Dim audioStreams = New MediaInfo(p.SourceFile)?.AudioStreams
        Dim addAudioStream = Sub(index As Integer)
                                 If index >= audioStreams.Count Then Exit Sub
                                 If audioStreams(index) Is Nothing Then Exit Sub

                                 If Not audioTracks?.Where(Function(x) x.Stream?.Name = audioStreams(index).Name)?.Any() Then
                                     audioTracks.Add((audioStreams(index).Name, audioStreams(index).Language, audioStreams(index).Title, audioStreams(index)))
                                 End If
                             End Sub


        For i = 0 To preferredAudios.Count() - 1
            Dim preferredAudio = preferredAudios(i)

            Dim idMatch = Regex.Match(preferredAudio, "^(\d+)([^,; ]*)$", RegexOptions.IgnoreCase)
            Dim languageMatch = Regex.Match(preferredAudio, "^([a-z]{2,}|[a-z]{2,4}(?:-[a-z]{2,})-[a-z]{2,5})([^,; ]*)$", RegexOptions.IgnoreCase)

            If idMatch.Success Then
                Dim id = idMatch.Groups(1).Value
                Dim fileAdded = False

                For j = 0 To groupedAudioFiles.Count() - 1
                    Dim item = groupedAudioFiles(j)

                    If item Is Nothing Then Continue For

                    For k = 0 To item.Count() - 1
                        If Not Regex.IsMatch(item(k).Base(), $"ID{id}\D") Then Continue For

                        addAudioTrack(j, k)
                    Next

                    fileAdded = True
                Next

                If Not fileAdded Then
                    addAudioStream(id.ToInt() - 1)
                End If
            ElseIf languageMatch.Success Then
                Dim prefLangString = languageMatch.Groups(1).Value
                Dim prefLang = New Language(prefLangString)

                For j = 0 To groupedAudioFiles.Count() - 1
                    Dim item = groupedAudioFiles(j)

                    If item IsNot Nothing Then
                        If prefLangString.ToLowerInvariant() <> "all" AndAlso prefLang.ThreeLetterCode <> item.Key.ThreeLetterCode AndAlso prefLang.Name <> item.Key.Name Then Continue For

                        For k = 0 To item.Count()
                            addAudioTrack(j, k)
                        Next
                    End If
                Next

                For j = 0 To audioStreams.Count() - 1
                    Dim item = audioStreams(j)

                    If item IsNot Nothing Then
                        If prefLangString.ToLowerInvariant() <> "all" AndAlso prefLang.ThreeLetterCode <> item.Language.ThreeLetterCode AndAlso prefLang.Name <> item.Language.Name Then Continue For
                        If audioTracks?.Where(Function(x) x.FilePath.ContainsEx("ID" + (item.Index + 1).ToString()) AndAlso x.Language.ThreeLetterCode = item.Language.ThreeLetterCode)?.Any() Then Continue For

                        addAudioStream(j)
                    End If
                Next
            End If
        Next

        Dim addTrack = Sub(source As (FilePath As String, Language As Language, Title As String, Stream As AudioStream), dest As AudioTrack)
                           If source.FilePath = "" Then Return
                           If dest Is Nothing Then Return

                           dest.AudioProfile.Reset()

                           If source.Stream Is Nothing Then
                               dest.TextEdit.Text = source.FilePath
                               dest.AudioProfile.File = source.FilePath
                           Else
                               dest.AudioProfile.File = p.SourceFile
                               dest.AudioProfile.Stream = source.Stream
                               dest.TextEdit.Text = $"{source.Stream.Name} ({p.SourceFile?.Ext()})"
                           End If

                           dest.AudioProfile.Language = source.Language
                           dest.LanguageLabel.Refresh()
                       End Sub

        For index = 0 To audioTracks.Count - 1
            Dim i = index
            If String.IsNullOrWhiteSpace(audioTracks(i).FilePath) Then Continue For

            Dim sameLanguages = p.AudioTracks.Where(Function(x) x.TextEdit.Text = "" AndAlso
                                                        TypeOf x.AudioProfile IsNot NullAudioProfile AndAlso
                                                        x.AudioProfile.Language.ThreeLetterCode = audioTracks(i).Language.ThreeLetterCode)

            If sameLanguages.Any() Then
                addTrack(audioTracks(i), sameLanguages.First())
            Else
                For index2 = 0 To p.AudioTracks.Count - 1
                    Dim j = index2

                    If TypeOf p.AudioTracks(j).AudioProfile Is NullAudioProfile Then Continue For
                    If p.AudioTracks(j).TextEdit.Text <> "" Then Continue For
                    If p.AudioTracks(j).AudioProfile.Language.IsDetermined AndAlso p.AudioTracks(j).AudioProfile.Language.ThreeLetterCode <> audioTracks(i).Language.ThreeLetterCode Then Continue For

                    addTrack(audioTracks(i), p.AudioTracks(j))
                    Exit For
                Next
            End If
        Next
    End Sub

    Function GetAudioTextBox(ap As AudioProfile) As TextEdit
        Return p.AudioTracks.Where(Function(x) x.AudioProfile Is ap)?.FirstOrDefault()?.TextEdit
    End Function

    Dim BlockAudioTextChanged As Boolean

    Sub AudioTextEditChanged(audioTrack As AudioTrack)
        If audioTrack Is Nothing Then Exit Sub
        If BlockAudioTextChanged Then Exit Sub

        Dim te = audioTrack.TextEdit
        Dim ap = audioTrack.AudioProfile

        If te.Text.Contains(":\") OrElse te.Text.StartsWith("\\") Then
            If te.Text <> ap.File Then
                ap.Reset()
                ap.File = te.Text

                If FileTypes.Audio.Contains(ap.File.Ext) Then
                    If Not p.Script.GetFilter("Source").Script.Contains("DirectShowSource") Then
                        ap.Delay += g.ExtractDelay(ap.File)
                    End If

                    If ap.StreamName = "" AndAlso ap.File.Contains("{") Then
                        Dim title = ap.File.Right("{")
                        ap.StreamName = title.Left("}").UnescapeIllegalFileSysChars
                    End If
                End If

                ap.SetStreamOrLanguage()
            End If

            UpdateSizeOrBitrate()

            BlockAudioTextChanged = True
            te.Text = ap.DisplayName
            BlockAudioTextChanged = False
        ElseIf te.Text = "" Then
            audioTrack.Remove()
            UpdateSizeOrBitrate()
        End If

        audioTrack.LanguageLabel?.Refresh()
    End Sub

    Sub AudioTextEditDoubleClick(audioTrack As AudioTrack)
        Using dialog As New OpenFileDialog
            Dim filter = FileTypes.Audio.ToList()
            filter.Insert(0, "avs")
            dialog.SetFilter(filter)
            dialog.SetInitDir(p.TempDir, s.LastSourceDir)

            If dialog.ShowDialog() = DialogResult.OK Then
                audioTrack.TextEdit.Text = dialog.FileName
            End If
        End Using
    End Sub

    Function GetPathFromIndexFile(sourcePath As String) As String
        For Each i In File.ReadAllLines(sourcePath)
            If i.Contains(":\") OrElse i.StartsWith("\\") Then
                If Regex.IsMatch(i, "^.+ \d+$") Then
                    i = i.LeftLast(" ")
                End If

                If File.Exists(i) AndAlso FileTypes.Video.Contains(i.Ext) Then
                    Return i
                End If
            End If
        Next
    End Function

    Function ShowSourceFilterSelectionDialog(inputFile As String) As VideoFilter
        Dim filters As New List(Of VideoFilter)

        If inputFile.Ext.EqualsAny("mp4", "m4v", "mov") Then
            AddSourceFilters({"LSMASHVideoSource", "LibavSMASHSource"}, filters)
        End If

        If FileTypes.Video.Contains(inputFile.Ext) AndAlso Not FileTypes.VideoText.Contains(inputFile.Ext) Then
            AddSourceFilters({"BestSource", "FFVideoSource", "LWLibavVideoSource", "ffms2", "LWLibavSource"}, filters)
        End If

        If g.IsCOMObjectRegistered(GUIDS.LAVSplitter) AndAlso g.IsCOMObjectRegistered(GUIDS.LAVVideoDecoder) Then
            AddSourceFilters({"DSS2"}, filters)
        End If

        If {"avi", "avs", "vdr"}.Contains(inputFile.Ext) Then
            AddSourceFilters({"AVISource"}, filters)
        End If

        If inputFile.Ext = "d2v" Then
            AddSourceFilters({"d2vsource"}, filters)
        End If

        If p.Script.IsAviSynth Then
            filters = filters.Where(Function(filter) Not filter.Script.Replace(" ", "").Contains("clip=core.")).ToList
            filters.Insert(0, New VideoFilter("Source", "Automatic", "#avs"))
        Else
            filters = filters.Where(Function(filter) filter.Script.Replace(" ", "").Contains("clip=core.")).ToList
            filters.Insert(0, New VideoFilter("Source", "Automatic", "#vs"))
        End If

        Dim td As New TaskDialog(Of VideoFilter) With {
            .Title = If(p.Script.IsAviSynth, "Select an AviSynth source filter:", "Select a VapourSynth source filter:")
        }

        For Each f In filters
            td.AddCommand(f.Name, f)
        Next

        Dim ret = td.Show
        td.Dispose()

        If ret Is Nothing Then
            Throw New AbortException
        End If

        ret.Script = Macro.ExpandGUI(ret.Script, True).Value
        Return ret
    End Function

    Sub AddSourceFilters(filterNames As String(), filters As List(Of VideoFilter))
        Dim avsProfiles = s.AviSynthProfiles.Where(Function(cat) cat.Name = "Source").First.Filters
        Dim vsProfiles = s.VapourSynthProfiles.Where(Function(cat) cat.Name = "Source").First.Filters
        Dim allFilters = avsProfiles.Concat(vsProfiles)

        For Each f In allFilters
            For Each filterName In filterNames
                If f.Script.ToLowerInvariant.Contains(filterName.ToLowerInvariant + "(") OrElse
                    f.Script.ToLowerInvariant.Contains(filterName.ToLowerInvariant + ".") Then

                    If filters.Where(Function(val) val.Name = f.Name).Count = 0 Then
                        filters.Add(f.GetCopy)
                    End If
                End If
            Next
        Next
    End Sub

    Sub OpenAnyFile(files As IEnumerable(Of String), showTemplateSelection As Boolean, Optional errorTimeout As Integer = 0)
        If files Is Nothing Then Exit Sub
        If Not files.Any() Then Exit Sub

        SetLastModifiedTemplate()

        Dim showTemplateSelectionTimeout = If(s.ShowTemplateSelection <> ShowTemplateSelectionMode.Never, s.ShowTemplateSelectionTimeout, 0)
        files = files.Select(Function(filePath) New FileInfo(filePath.TrimQuotes()).FullName)

        If files(0).Ext = "srip" Then
            OpenProject(files(0))
        ElseIf FileTypes.Video.Contains(files(0).Ext.ToLowerInvariant) Then
            files = files.OrderBy(Function(x) FileTypes.Video.Contains(x.Ext)).ThenBy(Function(x) FileTypes.Audio.Contains(x.Ext))
            If showTemplateSelection Then
                If LoadTemplateWithSelectionDialog(files, showTemplateSelectionTimeout) Then
                    OpenVideoSourceFiles(files, errorTimeout)
                Else
                    Exit Sub
                End If
            Else
                OpenVideoSourceFiles(files, errorTimeout)
            End If
        ElseIf FileTypes.Audio.ContainsAny(files.Select(Function(s) s.Ext.ToLowerInvariant)) Then
            Dim audioFiles = files.Where(Function(x) FileTypes.Audio.Contains(x.Ext().ToLowerInvariant())).OrderBy(Function(x) x, New StringLogicalComparer())
            Dim fileIndex = 0
            Dim freeAudioTracks = p.AudioTracks.Where(Function(x) x.TextEdit.Text = "")

            If audioFiles?.Any() AndAlso freeAudioTracks?.Any() Then
                For Each track In freeAudioTracks
                    If fileIndex >= audioFiles.Count() Then Exit For

                    track.TextEdit.Text = audioFiles(fileIndex)
                    fileIndex += 1
                Next
            End If
        Else
            files = files.OrderBy(Function(x) FileTypes.Video.Contains(x.Ext)).ThenBy(Function(x) FileTypes.Audio.Contains(x.Ext))

            Dim action = Sub()
                             If files(0).DirExists() Then
                                 OpenBlurayFolder(files(0))
                             Else
                                 OpenVideoSourceFiles(files, errorTimeout)
                             End If
                         End Sub

            If showTemplateSelection Then
                If LoadTemplateWithSelectionDialog(files, showTemplateSelectionTimeout) Then
                    action()
                Else
                    Exit Sub
                End If
            Else
                action()
            End If
        End If
    End Sub

    Function OpenBlurayFolder(srcPath As String) As Boolean
        If String.IsNullOrWhiteSpace(srcPath) Then Return False
        If Not srcPath.DirExists() Then Return False

        If Directory.Exists(Path.Combine(srcPath, "BDMV")) Then
            srcPath = Path.Combine(srcPath, "BDMV")
        End If
        If Directory.Exists(Path.Combine(srcPath, "PLAYLIST")) Then
            srcPath = Path.Combine(srcPath, "PLAYLIST")
        End If
        If New DirectoryInfo(srcPath).Name <> "PLAYLIST" Then
            MsgWarn("No playlist directory found.")
            Return False
        End If

        Log.WriteEnvironment()
        Log.Write("Process Blu-Ray folder using eac3to", """" + Package.eac3to.Path + """ """ + srcPath + """" + BR2)
        Log.WriteLine("Source Drive Type: " + New DriveInfo(srcPath).DriveType.ToString + BR)

        Dim output = ProcessHelp.GetConsoleOutput(Package.eac3to.Path, srcPath.Escape).Replace(VB6.vbBack, "")
        Log.WriteLine(output)

        Dim a = Regex.Split(output, "^\d+\)", RegexOptions.Multiline).ToList
        If a(0) = "" Then a.RemoveAt(0)

        Using td As New TaskDialog(Of Integer)
            td.Title = "Please select a playlist."

            For Each i In a
                If Not i.Contains(BR) Then Continue For
                Dim match = Regex.Match(i, "(\+\d+)\1{8,}", RegexOptions.Multiline)
                If match.Success AndAlso match.Groups.Count > 1 Then Continue For

                Dim value = a.IndexOf(i) + 1
                Dim text = value & ")  " & i.Left(BR).Trim()
                Dim description = i.Right(BR).TrimEnd()

                td.AddCommand(text, description, value)
            Next

            If td.Show() <> 0 Then
                OpenEac3toDemuxForm(srcPath, td.SelectedValue)
            End If
        End Using

        Return True
    End Function

    Sub OpenVideoSourceFile(fp As String, Optional errorTimeout As Integer = 0)
        OpenVideoSourceFiles({fp}, errorTimeout)
    End Sub

    Sub OpenVideoSourceFiles(files As IEnumerable(Of String), Optional errorTimeout As Integer = 0)
        OpenVideoSourceFiles(files, False, errorTimeout)
    End Sub

    Sub OpenVideoSourceFiles(files As IEnumerable(Of String), isEncoding As Boolean, Optional errorTimeout As Integer = 0)
        If p.SourceFile = "" Then SetLastModifiedTemplate()

        Dim recoverPath = g.ProjectPath
        Dim recoverProjectPath = Path.Combine(Folder.Temp, Guid.NewGuid.ToString + ".bin")
        Dim recoverText = Text
        Dim saveCurrent = p.SourceFile <> ""

        If saveCurrent AndAlso Not IsLoading Then
            If IsSaveCanceled() Then
                Return
            Else
                saveCurrent = False
            End If
        End If

        SafeSerialization.Serialize(p, recoverProjectPath)
        AddHandler Disposed, Sub() FileHelp.Delete(recoverProjectPath)

        SkipAssistant = True

        Try
            files = files.Select(Function(filePath) New FileInfo(filePath.TrimQuotes()).FullName).OrderBy(Function(filePath) filePath, StringComparer.InvariantCultureIgnoreCase)

            If Not g.VerifySource(files) Then
                Throw New AbortException
            End If

            For Each i In files
                Dim name = i.FileName

                If name.ToUpperInvariant Like "VTS_0#_0.VOB" Then
                    If MsgQuestion("Are you sure you want to open the file " + name + "," + BR + "the first VOB file usually contains a menu.") = DialogResult.Cancel Then
                        Throw New AbortException
                    End If
                End If

                If name.ToUpperInvariant = "VIDEO_TS.VOB" Then
                    MsgWarn("The file VIDEO_TS.VOB can't be opened.")
                    Throw New AbortException
                End If
            Next

            Debug.WriteLine(isEncoding)
            Debug.WriteLine(p.SourceFile)

            p.SourceFiles = files.ToList()
            p.SourceFile = files(0)

            If p.SourceFile.Ext.EqualsAny(FileTypes.Image) Then
                If p.SourceFile.Base(p.SourceFile.Base.Length - 1).IsDigit Then
                    If p.Script.IsAviSynth Then
                        Dim digitCount = 0

                        For i = p.SourceFile.Base.Length - 1 To 0 Step -1
                            If p.SourceFile.Base(i).IsDigit Then
                                digitCount += 1
                            End If
                        Next

                        Dim startText = p.SourceFile.Base.Substring(0, p.SourceFile.Base.Length - digitCount)
                        Dim images As New List(Of String)
                        Dim allFiles = Directory.GetFiles(p.SourceFile.Dir)

                        For Each file In Directory.GetFiles(p.SourceFile.Dir)
                            If file.Base.Length = p.SourceFile.Base.Length AndAlso
                                file.Ext = p.SourceFile.Ext AndAlso file.Base.StartsWith(startText) Then

                                images.Add(file)
                            End If
                        Next

                        images.Sort()

                        Dim filter = p.Script.GetFilter("Source")
                        filter.Path = "Image"
                        filter.Script = "ImageSource(""" + Path.Combine(images(0).Dir, p.SourceFile.Base.Substring(0,
                            p.SourceFile.Base.Length - digitCount) + "%0" & digitCount & "d." + p.SourceFile.Ext) +
                            """, " & images(0).Base.Substring(p.SourceFile.Base.Length - digitCount).ToInt &
                            ", " & images(images.Count - 1).Base.Substring(p.SourceFile.Base.Length - digitCount).ToInt & ", 25)"
                    End If
                Else
                    Dim filter = p.Script.GetFilter("Source")
                    filter.Path = "Image"
                    filter.Script = "ImageSource(""%source_file%"", 0, 1000, 25)"
                End If
            End If

            FiltersListView.IsLoading = True
            PreviewScript = Nothing

            Dim preferredSourceFilter As VideoFilter = Nothing

            If p.SourceFiles.Count = 1 AndAlso
                p.Script.Filters(0).Name = "Manual" AndAlso
                Not p.NoDialogs AndAlso Not p.BatchMode AndAlso
                Not p.SourceFile.Ext.EqualsAny("avs", "vpy") Then

                preferredSourceFilter = ShowSourceFilterSelectionDialog(files(0))
            End If

            If preferredSourceFilter IsNot Nothing Then
                Dim isVapourSynth = preferredSourceFilter.Script.Replace(" ", "").Contains("clip=core.") OrElse preferredSourceFilter.Script = "#vs"

                If isVapourSynth Then
                    If Not Package.Python.VerifyOK(True) OrElse
                        Not Package.VapourSynth.VerifyOK(True) OrElse
                        Not Package.vspipe.VerifyOK(True) Then

                        Throw New AbortException
                    End If

                    If p.Script.IsAviSynth Then
                        p.Script = VideoScript.GetDefaults()(1)
                    End If
                Else
                    If Not Package.AviSynth.VerifyOK(True) Then Throw New AbortException

                    If p.Script.Engine = ScriptEngine.VapourSynth Then
                        p.Script = VideoScript.GetDefaults()(0)
                    End If
                End If

                p.Script.SetFilter(preferredSourceFilter.Category, preferredSourceFilter.Name, preferredSourceFilter.Script)
            End If

            If Not g.VerifyRequirements() Then
                Throw New AbortException
            End If

            p.LastOriginalSourceFile = p.SourceFile
            p.FirstOriginalSourceFile = p.SourceFile

            If FileTypes.VideoIndex.Contains(p.SourceFile.Ext) Then
                Dim path = GetPathFromIndexFile(p.SourceFile)

                If path <> "" Then
                    p.LastOriginalSourceFile = path
                    p.FirstOriginalSourceFile = path
                End If
            ElseIf p.SourceFile.Ext.EqualsAny({"avs", "vpy"}) Then
                Dim code = p.SourceFile.ReadAllText
                Dim matches = Regex.Matches(code, "('|"")(.+?)\1", RegexOptions.IgnoreCase)

                For Each match As Match In matches
                    If match.Success Then
                        Dim path = match.Groups(2).Value

                        If File.Exists(path) AndAlso FileTypes.Video.Contains(path.Ext) Then
                            If FileTypes.VideoIndex.Contains(path.Ext) Then
                                path = GetPathFromIndexFile(path)

                                If path <> "" Then
                                    p.LastOriginalSourceFile = path
                                    p.FirstOriginalSourceFile = path
                                    Exit For
                                End If
                            Else
                                p.LastOriginalSourceFile = path
                                p.FirstOriginalSourceFile = path
                                Exit For
                            End If
                        End If
                    End If
                Next
            End If

            g.SetTempDir()

            Dim sourcePAR = MediaInfo.GetVideo(p.LastOriginalSourceFile, "PixelAspectRatio")

            If sourcePAR <> "" Then
                p.SourcePAR.X = CInt(Convert.ToSingle(sourcePAR, CultureInfo.InvariantCulture) * 1000)
                p.SourcePAR.Y = 1000
            End If

            p.SourceVideoHdrFormat = MediaInfo.GetVideo(p.LastOriginalSourceFile, "HDR_Format_Commercial")
            p.SourceVideoHdrFormat = If(String.IsNullOrWhiteSpace(p.SourceVideoHdrFormat), "SDR", p.SourceVideoHdrFormat)
            p.SourceVideoHdrFormat = If(p.SourceVideoHdrFormat = "Dolby Vision", "DV", p.SourceVideoHdrFormat)

            If p.SourceVideoHdrFormat.ContainsAny("SDR", "HDR10", "HDR10+", "Dolby Vision") Then
                Dim hdrFormat = MediaInfo.GetVideo(p.LastOriginalSourceFile, "HDR_Format/String")

                If hdrFormat.ContainsEx("Dolby Vision") Then
                    p.SourceVideoHdrFormat = "DV"

                    Dim profileMatch = Regex.Match(hdrFormat, "Profile (\d\.\d),")
                    If profileMatch.Success Then
                        p.SourceVideoHdrFormat += " " + profileMatch.Groups(1).Value
                    End If

                    If Regex.IsMatch(hdrFormat, "HDR10 compatible") Then
                        p.SourceVideoHdrFormat += " / HDR10"
                    ElseIf Regex.IsMatch(hdrFormat, "HDR10\+ Profile [AB] compatible") Then
                        p.SourceVideoHdrFormat += " / HDR10+"
                    End If
                End If
            End If

            p.SourceVideoFormat = MediaInfo.GetVideoFormat(p.LastOriginalSourceFile)
            p.SourceVideoFormatProfile = MediaInfo.GetVideo(p.LastOriginalSourceFile, "Format_Profile")
            p.SourceVideoBitDepth = MediaInfo.GetVideo(p.LastOriginalSourceFile, "BitDepth").ToInt
            p.SourceColorSpace = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ColorSpace")
            p.SourceChromaSubsampling = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ChromaSubsampling")
            p.SourceSize = If(p.LastOriginalSourceFile.FileExists(), p.LastOriginalSourceFile.FileSize, If(p.LastOriginalSourceFile.DirExists(), p.LastOriginalSourceFile.DirSize(), 0L))
            p.SourceVideoSize = MediaInfo.GetVideo(p.LastOriginalSourceFile, "StreamSize").ToLong()
            p.SourceBitrate = CInt(MediaInfo.GetVideo(p.LastOriginalSourceFile, "BitRate").ToInt / 1000)
            p.SourceFrameRateMode = MediaInfo.GetVideo(p.LastOriginalSourceFile, "FrameRate_Mode")
            p.SourceScanType = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ScanType")
            p.SourceScanOrder = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ScanOrder")

            Dim mkvMuxer = TryCast(p.VideoEncoder.Muxer, MkvMuxer)

            If mkvMuxer IsNot Nothing Then
                If p.TakeOverVideoLanguage AndAlso Not mkvMuxer.VideoTrackLanguage?.IsDetermined Then
                    mkvMuxer.VideoTrackLanguage = New Language(MediaInfo.GetVideo(p.LastOriginalSourceFile, "Language"))
                End If
                If p.TakeOverTitle Then 
                    If mkvMuxer.Title = "" Then mkvMuxer.Title = MediaInfo.GetGeneral(p.LastOriginalSourceFile, "Title")
                    If mkvMuxer.Title = "" Then mkvMuxer.Title = MediaInfo.GetGeneral(p.LastOriginalSourceFile, "Movie")
                End If
            End If

            If Not isEncoding AndAlso p.BatchMode Then
                Assistant()
                Exit Sub
            End If

            Log.WriteEnvironment()

            Dim targetDir As String = Nothing

            If p.DefaultTargetFolder <> "" Then
                targetDir = Macro.Expand(p.DefaultTargetFolder).FixDir

                If Not Directory.Exists(targetDir) Then
                    Try
                        Directory.CreateDirectory(targetDir)
                    Catch ex As Exception
                    End Try
                End If
            End If

            If Not Directory.Exists(targetDir) Then
                targetDir = p.SourceFile.Dir
            End If

            Dim targetName = Macro.Expand(p.DefaultTargetName)

            If Not targetName.IsValidFileSystemName Then
                targetName = p.SourceFile.Base
            End If

            tbTargetFile.Text = Path.Combine(targetDir, targetName + p.VideoEncoder.Muxer.OutputExtFull)

            If p.SourceFile = p.TargetFile OrElse
                (FileTypes.VideoIndex.Contains(p.SourceFile.Ext) AndAlso
                p.SourceFile.ReadAllText.Contains(p.TargetFile)) Then

                tbTargetFile.Text = p.TargetFile.DirAndBase + "_new" + p.TargetFile.ExtFull
            End If

            Log.WriteHeader("Media Info Source File")

            For Each i In p.SourceFiles
                Log.WriteLine(i)
            Next

            Log.WriteLine(BR + MediaInfo.GetSummary(p.SourceFile))

            For Each i In DriveInfo.GetDrives()
                If i.DriveType = DriveType.CDRom AndAlso
                    p.TempDir.ToUpperInvariant.StartsWith(i.RootDirectory.ToString.ToUpperInvariant) Then

                    MsgWarn("Opening files from an optical drive requires to set a temp files folder in the options.")
                    Throw New AbortException
                End If
            Next

            Demux()

            If String.IsNullOrWhiteSpace(p.Hdr10PlusMetadataFile) OrElse String.IsNullOrWhiteSpace(p.HdrDolbyVisionMetadataFile?.Path) Then
                Dim metadatas = Task.Run(Async Function() Await FindHdrMetadataAsync(p)).Result
                p.Hdr10PlusMetadataFile = If(String.IsNullOrWhiteSpace(metadatas.jsonFile), Nothing, metadatas.jsonFile)
                p.HdrDolbyVisionMetadataFile = If(String.IsNullOrWhiteSpace(metadatas.rpuFile), Nothing, New DolbyVisionMetadataFile(metadatas.rpuFile))
            End If

            If p.ExtractHdrmetadata <> HdrmetadataMode.None AndAlso
                Not p.SourceVideoHdrFormat?.EqualsAny("", "SDR") AndAlso
                (String.IsNullOrWhiteSpace(p.Hdr10PlusMetadataFile) OrElse String.IsNullOrWhiteSpace(p.HdrDolbyVisionMetadataFile?.Path)) Then

                Select Case p.ExtractHdrmetadata
                    Case HdrmetadataMode.DolbyVision
                        ExtractDolbyVisionMetadata(p)
                    Case HdrmetadataMode.HDR10Plus
                        ExtractHdr10PlusMetadata(p)
                    Case Else
                        ExtractHdrMetadata(p)
                End Select
            End If

            p.VideoEncoder.SetMetaData(p.LastOriginalSourceFile)

            If p.LastOriginalSourceFile <> p.SourceFile AndAlso Not FileTypes.VideoText.Contains(p.SourceFile.Ext) Then
                p.LastOriginalSourceFile = p.SourceFile
            End If

            s.LastSourceDir = p.SourceFile.Dir

            If p.SourceFile.Ext = "avs" AndAlso p.Script.IsAviSynth Then
                p.Script.Filters.Clear()
                p.Script.Filters.Add(New VideoFilter("Source", "AVS Script Import", "Import(""" + p.SourceFile + """)"))
            ElseIf p.SourceFile.Ext = "vpy" Then
                p.Script.Engine = ScriptEngine.VapourSynth
                p.Script.Filters.Clear()
                Dim code = "import vapoursynth as vs" + BR +
                           "core = vs.core" + BR +
                           "from importlib.machinery import SourceFileLoader" + BR +
                           $"SourceFileLoader('clip', r""{p.SourceFile}"").load_module()" + BR +
                           "clip = vs.get_output()"
                p.Script.Filters.Add(New VideoFilter("Source", "VS Script Import", code))
            End If

            ModifyFilters(errorTimeout)
            FiltersListView.IsLoading = False
            FiltersListView.Load()

            RenameDVDTracks()

            If FileTypes.VideoAudio.Contains(p.LastOriginalSourceFile.Ext) Then
                Dim streams = MediaInfo.GetAudioStreams(p.LastOriginalSourceFile)
                For Each track In p.AudioTracks
                    track.AudioProfile.Streams = streams
                Next
            End If

            If p.SourceFile.Ext = "d2v" Then
                Dim content = p.SourceFile.ReadAllText

                If content.Contains("Aspect_Ratio=16:9") Then
                    p.SourceAnamorphic = True
                Else
                    Dim ifoFile = GetIfoFile()

                    If ifoFile <> "" Then
                        Dim dar2 = MediaInfo.GetVideo(ifoFile, "DisplayAspectRatio")

                        If dar2 = "1.778" Then
                            p.SourceAnamorphic = True
                        End If
                    End If
                End If

                If content.Contains("Frame_Rate=29970") Then
                    Dim m = Regex.Match(content, "FINISHED\s+(\d{1,2}(?:\.\d{1,2}))\s?%\s+FILM")

                    If m.Success Then
                        Dim film = m.Groups(1).Value.ToSingle()
                        If film >= p.D2VAutoForceFilmThreshold Then
                            content = content.Replace($"Field_Operation=0{BR}Frame_Rate=29970 (30000/1001)", $"Field_Operation=1{BR}Frame_Rate=23976 (24000/1001)")
                            content.WriteFileSystemEncoding(p.SourceFile)
                        End If
                    End If
                End If
            End If

            UpdateSourceParameters()
            SetSlider()
            AddAudioTracks()

            If p.UseScriptAsAudioSource Then
                If p.AudioTracks.Any() Then
                    p.AudioTracks(0).TextEdit.Text = p.Script.Path
                End If
            End If

            BlockSourceTextBoxTextChanged = True
            tbSourceFile.Text = p.SourceFile
            BlockSourceTextBoxTextChanged = False

            s.LastPosition = 0

            UpdateTargetParameters(p)
            DemuxVobSubSubtitles()
            ConvertBluRaySubtitles()
            ExtractForcedVobSubSubtitles()
            g.RaiseAppEvent(ApplicationEvent.BeforeMuxingWhenSourceOpening)
            p.VideoEncoder.Muxer.Init()

            If p.HardcodedSubtitle Then
                g.AddHardcodedSubtitle()
            End If

            AutoCrop()
            AutoResize()

            If p.Script.IsFilterActive("Crop") AndAlso (p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 Then
                g.OvercropWidth()

                If p.AutoSmartCrop Then
                    g.SmartCrop()
                End If
            End If

            Dim nonDvdFiles = files.Where(Function(x) Not Regex.IsMatch(x, ".*VTS_\d\d_\d\.vob", RegexOptions.IgnoreCase))

            If nonDvdFiles.Count() > 1 Then
                Dim psi = New ProcessStartInfo(Application.ExecutablePath, $"-LoadTemplate:""{p.TemplateName}"" -LoadSourceFiles:""{nonDvdFiles.Skip(1).Join(""";""", False)}""") With {
                    .UseShellExecute = False,
                    .CreateNoWindow = True
                }

                Process.Start(psi)?.Dispose()
            End If

            If p.AutoCompCheck AndAlso p.VideoEncoder.IsCompCheckEnabled Then
                p.VideoEncoder.RunCompCheck()
            End If

            g.RaiseAppEvent(ApplicationEvent.AfterSourceOpened)
            SkipAssistant = False
            Assistant()
            g.RaiseAppEvent(ApplicationEvent.AfterSourceLoaded)
            g.RaiseAppEvent(ApplicationEvent.AfterProjectOrSourceLoaded)
            Log.Save()
        Catch ex As AbortException
            Log.Save()
            SetSavedProject()
            OpenProject(recoverProjectPath)
            Text = recoverText
            g.ProjectPath = recoverPath

            If isEncoding Then
                Throw New AbortException
            End If
        Catch ex As Exception
            g.OnException(ex)
            OpenProject("", False)
        Finally
            SkipAssistant = False
            If Not isEncoding Then
                ProcController.Finished()
            End If
        End Try
    End Sub

    Sub ModifyFilters(Optional timeout As Integer = 0)
        If p.SourceFile = "" Then Exit Sub

        Dim profiles = If(p.Script.IsAviSynth, s.AviSynthProfiles, s.VapourSynthProfiles)
        Dim preferences = If(p.Script.IsAviSynth, s.AviSynthFilterPreferences, s.VapourSynthFilterPreferences)
        Dim editAVS = p.Script.IsAviSynth AndAlso p.SourceFile.Ext <> "avs"
        Dim editVS = p.Script.IsVapourSynth AndAlso p.SourceFile.Ext <> "vpy"

        If p.AutoRotation AndAlso (editAVS OrElse editVS) Then
            Dim rot = MediaInfo.GetVideo(p.SourceFile, "Rotation").ToDouble

            If rot <> 0 Then
                Dim name = ""

                Select Case rot
                    Case 90
                        name = "Right"
                    Case 180
                        name = "Upside Down"
                    Case 270
                        name = "Left"
                    Case Else
                End Select

                If name <> "" Then
                    Dim filter = VideoFilter.GetDefault("Rotation", name, p.Script.Engine)

                    If filter IsNot Nothing Then
                        p.Script.SetFilter(filter.Category, filter.Name, filter.Script)
                    End If
                End If
            End If
        End If

        Dim sourceFilter = p.Script.GetFilter("Source")

        SetSourceFilter(sourceFilter, preferences, profiles, True, True, False, False)
        SetSourceFilter(sourceFilter, preferences, profiles, False, True, True, False)
        SetSourceFilter(sourceFilter, preferences, profiles, True, False, False, True)
        SetSourceFilter(sourceFilter, preferences, profiles, False, False, False, False)

        If editAVS Then
            If Not sourceFilter.Script.Contains("(") Then
                Dim filter = VideoFilter.GetDefault("Source", "FFVideoSource")
                p.Script.SetFilter(filter.Category, filter.Name, filter.Script)
            End If
        ElseIf editVS Then
            If Not sourceFilter.Script.Contains("(") Then
                Dim filter = VideoFilter.GetDefault("Source", "ffms2", ScriptEngine.VapourSynth)
                p.Script.SetFilter(filter.Category, filter.Name, filter.Script)
            End If
        End If

        For Each iFilter In p.Script.Filters
            If iFilter.Script.Contains("$") Then
                iFilter.Script = Macro.ExpandGUI(iFilter.Script, True).Value
            End If
        Next

        Dim errorMsg As String

        Try
            errorMsg = p.SourceScript.GetError
        Catch ex As AbortException
            errorMsg = ex.Message
            timeout = -1
        Catch ex As Exception
            errorMsg = ex.Message
        End Try

        If errorMsg <> "" Then
            Log.WriteHeader("Error opening source")
            Log.WriteLine(errorMsg + BR2)
            Log.WriteLine(p.SourceScript.GetFullScript)
            Log.Save()

            MsgError("Script Error", errorMsg, Handle, timeout)
            Throw New AbortException
        End If

        If Not editAVS AndAlso Not editVS Then
            Exit Sub
        End If

        Dim sourceInfo = p.SourceScript.Info

        If p.FixFrameRate Then
            Dim fixedFrameRate = FixFrameRate(sourceInfo.FrameRateNum, sourceInfo.FrameRateDen)

            If fixedFrameRate.num <> sourceInfo.FrameRateNum OrElse fixedFrameRate.den <> sourceInfo.FrameRateDen Then
                If editAVS Then
                    p.Script.GetFilter("Source").Script += BR + "AssumeFPS(" & fixedFrameRate.num & ", " & fixedFrameRate.den & ")"
                Else
                    p.Script.GetFilter("Source").Script += BR + "clip = core.std.AssumeFPS(clip, None, " & fixedFrameRate.num & ", " & fixedFrameRate.den & ")"
                End If

                p.SourceScript.Synchronize()
            End If
        End If

        If editAVS Then
            Dim miFPS = MediaInfo.GetFrameRate(p.FirstOriginalSourceFile, 25)
            Dim avsFPS = sourceInfo.FrameRate

            If (CInt(miFPS) * 2) = CInt(avsFPS) Then
                p.Script.GetFilter("Source").Script += BR + "SelectEven().AssumeFPS(" & miFPS.ToInvariantString + ")"
                p.SourceScript.Synchronize()
            End If
        End If

        Dim bitDepthAdjusted = False

        If p.SourceChromaSubsampling <> "4:2:0" AndAlso p.ConvertChromaSubsampling Then
            Dim interlaced = p.SourceScanType.EqualsAny("Interlaced", "MBAFF")

            If editVS Then
                Dim sourceHeight = MediaInfo.GetVideo(p.LastOriginalSourceFile, "Height").ToInt
                Dim matrix = If(sourceHeight = 0 OrElse sourceHeight > 576, "709", "470bg")
                Dim format = If(p.SourceVideoBitDepth = 10, "YUV420P10", "YUV420P8")
                If p.ConvertToBits <> ConvertTo420BitDepth.None AndAlso p.ConvertToBits <> p.SourceVideoBitDepth Then
                    format = "YUV420P" & (p.ConvertToBits + 0)
                    bitDepthAdjusted = True
                End If
                Dim category = "Color"
                Dim name = $"Convert To {format}"
                Dim script = $"clip = clip.resize.Bicubic(format = vs.{format})"

                If interlaced Then
                    p.Script.Filters.Add(New VideoFilter(category, name, script, True))
                Else
                    p.Script.Filters.Insert(1, New VideoFilter(category, name, script, True))
                End If
            ElseIf editAVS Then
                Dim category = "Color"
                Dim name = $"ConvertToYUV420()"
                Dim script = $"ConvertToYUV420()"

                If interlaced Then
                    p.Script.Filters.Add(New VideoFilter(category, name, script, True))
                Else
                    p.Script.Filters.Insert(1, New VideoFilter(category, name, script, True))
                End If
            End If
        End If

        If p.ConvertToBits <> ConvertTo420BitDepth.None AndAlso p.ConvertToBits <> p.SourceVideoBitDepth AndAlso Not bitDepthAdjusted Then
            Dim bits = (p.ConvertToBits + 0)
            Dim category = "BitDepth"
            Dim name = $"Convert To {bits}-bit"

            If editVS Then
                Dim script = $"clip = core.fmtc.bitdepth(clip, bits={bits})"
                p.Script.Filters.Add(New VideoFilter(category, name, script, True))
            ElseIf editAVS Then
                Dim script = $"ConvertBits({bits})"
                p.Script.Filters.Add(New VideoFilter(category, name, script, True))
            End If
        End If
    End Sub

    Function FixFrameRate(num As Integer, den As Integer) As (num As Integer, den As Integer)
        Dim rate = num / den
        Return If(rate < 50 AndAlso rate > 49, (50, 1), (num, den))
    End Function

    Sub SetSourceFilter(
        sourceFilter As VideoFilter,
        preferences As StringPairList,
        profiles As List(Of FilterCategory),
        skipAnyExtension As Boolean,
        skipAnyFormat As Boolean,
        skipNonAnyExtension As Boolean,
        skipNonAnyFormat As Boolean)

        If Not sourceFilter.Script.Contains("(") Then
            For Each pref In preferences
                Dim extensions = pref.Name.SplitNoEmptyAndWhiteSpace({",", " ", ";"})

                For Each extension In extensions
                    extension = extension.ToLowerInvariant
                    Dim format = "*"

                    If extension.Contains(":") Then
                        format = extension.Right(":").ToLowerInvariant
                        extension = extension.Left(":").ToLowerInvariant
                    End If

                    If skipAnyExtension AndAlso extension = "*" Then Continue For
                    If skipAnyFormat AndAlso format = "*" Then Continue For
                    If skipNonAnyExtension AndAlso extension <> "*" Then Continue For
                    If skipNonAnyFormat AndAlso format <> "*" Then Continue For

                    If (extension = p.SourceFile.Ext OrElse extension = "*") AndAlso
                        (format = "*" OrElse format = MediaInfo.GetVideo(p.SourceFile, "Format").ToLowerInvariant) Then

                        Dim filters = profiles.Where(
                            Function(cat) cat.Name = "Source").First.Filters.Where(
                            Function(cat) cat.Name = pref.Value)

                        If filters.Count > 0 Then
                            p.Script.SetFilter("Source", filters(0).Name, filters(0).Script)
                            Exit Sub
                        End If
                    End If
                Next
            Next
        End If
    End Sub

    Sub AutoCrop()
        g.CheckForModifiedDolbyVisionLevel5Data()

        If p.AutoCropMode = AutoCropMode.DolbyVisionOnly OrElse p.AutoCropMode = AutoCropMode.Always Then
            p.SourceScript.Synchronize(True, True, True)

            If p.HdrDolbyVisionMetadataFile IsNot Nothing Then
                Dim c = p.HdrDolbyVisionMetadataFile.Crop
                g.SetCrop(c.Left, c.Top, c.Right, c.Bottom, ForceOutputModDirection.Decrease, True)
            ElseIf p.AutoCropMode = AutoCropMode.Always Then
                Dim info = p.SourceScript.Info
                Dim selectionMode = Convert.ToInt32(p.AutoCropFrameSelectionMode)
                Dim selectionValue = If(p.AutoCropFrameSelectionMode = AutoCropFrameSelectionMode.FrameInterval, p.AutoCropFrameIntervalFrameSelection, p.AutoCropFixedFramesFrameSelection)
                selectionValue = If(p.AutoCropFrameSelectionMode = AutoCropFrameSelectionMode.TimeInterval, p.AutoCropTimeIntervalFrameSelection, selectionValue)
                Dim considerationMode = Convert.ToInt32(p.AutoCropFrameRangeMode)
                Dim considerationThresholdBegin = 0
                Dim considerationThresholdEnd = 0
                Dim luminanceThreshold = CInt(p.AutoCropLuminanceThreshold * 100)
                Dim vfw = If(FrameServerHelp.IsVfwUsed, 1, 0)

                If p.AutoCropFrameRangeMode = AutoCropFrameRangeMode.Automatic Then
                    Dim threshold = CInt(Conversion.Fix(info.FrameCount * 0.05))
                    considerationThresholdBegin = threshold
                    considerationThresholdEnd = threshold
                ElseIf p.AutoCropFrameRangeMode = AutoCropFrameRangeMode.ManualThreshold Then
                    considerationThresholdBegin = p.AutoCropFrameRangeThresholdBegin
                    considerationThresholdEnd = p.AutoCropFrameRangeThresholdEnd
                End If

                Try
                    Using proc As New Proc
                        proc.Header = "Auto Crop"
                        proc.SkipString = "%"
                        proc.Package = Package.AutoCrop
                        proc.Arguments = $"{p.SourceScript.Path.Escape} {selectionMode} {selectionValue} {considerationThresholdBegin} {considerationThresholdEnd} {luminanceThreshold.ToInvariantString()} {vfw}"
                        proc.Start()

                        Dim match = Regex.Match(proc.Log.ToString, "(\d+),(\d+),(\d+),(\d+)")

                        If match.Success Then
                            g.SetCrop(match.Groups(1).Value.ToInt, match.Groups(2).Value.ToInt, match.Groups(3).Value.ToInt, match.Groups(4).Value.ToInt, p.ForcedOutputModDirection, False)
                        End If
                    End Using
                Catch ex As SkipException
                    Log.WriteLine("AutoCrop skipped...")
                    Log.Save()
                    ProcController.Aborted = False
                Catch ex As AbortException
                    Log.WriteLine("AutoCrop aborted...")
                    Log.Save()
                    ProcController.Aborted = False
                Catch ex As ErrorAbortException
                    Log.WriteLine("AutoCrop aborted...")
                    Log.Save()
                    ProcController.Aborted = False
                Catch ex As Exception
                    Throw ex
                End Try
            End If

            SetCropFilter()
            DisableCropFilter()
        End If
    End Sub

    Sub AutoResize()
        If p.Script.IsFilterActive("Resize") Then
            If p.AutoResizeImage <> 0 Then
                SetTargetImageSizeByPixel(p.AutoResizeImage)
            Else
                If p.AdjustHeight Then
                    Dim h = Calc.FixMod(CInt(p.TargetWidth / Calc.GetTargetDAR()), p.ForcedOutputMod)
                    tbTargetHeight.Text = h.ToString()
                End If
            End If
        Else
            Dim cropw = p.SourceWidth - p.CropLeft - p.CropRight
            tbTargetWidth.Text = cropw.ToString
            tbTargetHeight.Text = (p.SourceHeight - p.CropTop - p.CropBottom).ToString
        End If
    End Sub

    Sub ConvertBluRaySubtitles()
        If Not p.ConvertSup2Sub Then Exit Sub

        For Each i In g.GetFilesInTempDirAndParent
            If i.Ext = "sup" AndAlso g.IsSourceSameOrSimilar(i) AndAlso Not File.Exists(i.DirAndBase + ".idx") Then
                Using proc As New Proc
                    proc.Header = "Convert sup to sub"
                    proc.SkipStrings = {"#>", "#<", "Decoding frame"}
                    proc.File = Package.BDSup2SubPP.Path
                    proc.Arguments = "-o """ + i.DirAndBase + ".idx"" " + i.Escape
                    proc.Start()
                End Using
            End If
        Next
    End Sub

    Sub ExtractForcedVobSubSubtitles()
        If Not p.ExtractForcedSubSubtitles Then Exit Sub

        For Each path In g.GetFilesInTempDirAndParent
            If path.ExtFull = ".idx" AndAlso g.IsSourceSameOrSimilar(path) AndAlso
                    Not path.Contains("_forced") AndAlso
                    Not File.Exists(path.DirAndBase + "_forced.idx") Then

                Dim idxContent = path.ReadAllTextDefault

                If idxContent.Contains(VB6.ChrW(&HA) + VB6.ChrW(&H0) + VB6.ChrW(&HD) + VB6.ChrW(&HA)) Then
                    idxContent = idxContent.FixBreak
                    idxContent = idxContent.Replace(BR + VB6.ChrW(&H0) + BR, BR + "langidx: 0" + BR)
                    idxContent.WriteFileSystemEncoding(path)
                End If

                Using proc As New Proc
                    proc.Header = "Extract forced subtitles if existing"
                    proc.SkipString = "# "
                    proc.WriteLog(path.FileName + BR2)
                    proc.File = Package.BDSup2SubPP.Path
                    proc.Arguments = "--forced-only -o " + (path.DirAndBase + "_forced.idx").Escape + " " + path.Escape
                    proc.AllowedExitCodes = {0, 1, 4}
                    proc.Start()
                End Using
            End If
        Next
    End Sub

    Sub DemuxVobSubSubtitles()
        If p.SubtitleMode = SubtitleMode.Disabled Then Exit Sub
        If Not {"vob", "m2v"}.Contains(p.LastOriginalSourceFile.Ext) Then Exit Sub

        Dim ifoPath = GetIfoFile()

        If ifoPath = "" Then Exit Sub
        If File.Exists(Path.Combine(p.TempDir, p.SourceFile.Base + ".idx")) Then Exit Sub

        Dim subtitleCount = MediaInfo.GetSubtitleCount(ifoPath)

        If subtitleCount > 0 Then
            Dim pgcCount As Byte
            Dim sectorPointer As Integer
            Dim buffer = File.ReadAllBytes(ifoPath)
            sectorPointer = (((buffer(&HCC) * &H1000000) + (buffer(&HCD) * &H10000)) + (buffer(&HCE) * &H100)) + (buffer(&HCF) * 1)
            pgcCount = CByte((buffer(sectorPointer * &H800) * &H100) + buffer((sectorPointer * &H800) + 1))

            If pgcCount <> 0 Then
                For i = 0 To pgcCount - 1
                    Dim a = ((sectorPointer * &H800) + (i * 8)) + 12
                    Dim b = (((buffer(a + 0) * &H1000000) + (buffer(a + 1) * &H10000)) + (buffer(a + 2) * &H100)) + (buffer(a + 3) * 1) + sectorPointer * &H800
                    Dim hour, min, sec As Integer

                    hour = CByte((((buffer(b + 4) And 240) >> 4) * 10) + (buffer(b + 4) And 15))
                    min = CByte((((buffer(b + 5) And 240) >> 4) * 10) + (buffer(b + 5) And 15))
                    sec = CByte((((buffer(b + 6) And 240) >> 4) * 10) + (buffer(b + 6) And 15))

                    If Math.Abs(p.SourceSeconds - ((hour * 60 ^ 2) + (min * 60) + sec)) < 30 Then
                        Dim args =
                            ifoPath + BR +
                            Path.Combine(p.TempDir, p.SourceFile.Base) + BR &
                            (i + 1) & BR +
                            "1" + BR +
                            "ALL" + BR +
                            "CLOSE"

                        Dim fileContent = Path.Combine(p.TempDir, p.TargetFile.Base + "_vsrip.txt")
                        args.WriteFileSystemEncoding(fileContent)

                        Using proc As New Proc
                            proc.Header = "Demux subtitles using VSRip"
                            proc.WriteLog(args + BR2)
                            proc.File = Package.VSRip.Path
                            proc.Arguments = """" + fileContent + """"
                            proc.AllowedExitCodes = {0, 1, 2}
                            proc.Start()
                        End Using

                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Function ProcessTip(message As String) As Boolean
        If message.Contains(BR2) Then
            message = message.Replace(BR2, BR)
        End If

        If message.Contains(VB6.vbLf + VB6.vbLf) Then
            message = message.FixBreak.Replace(BR2, BR)
        End If

        CurrentAssistantTipKey = message.MD5Hash

        If Not p.SkippedAssistantTips.Contains(CurrentAssistantTipKey) Then
            If message <> "" Then
                If message.Length > 130 Then
                    laTip.SetFontSize(8)
                Else
                    laTip.SetFontSize(9)
                End If
            End If

            laTip.Text = message
            Return True
        End If
    End Function

    Function RemoveTip(message As String) As Boolean
        If message.Contains(BR2) Then
            message = message.Replace(BR2, BR)
        End If

        If message.Contains(VB6.vbLf + VB6.vbLf) Then
            message = message.FixBreak.Replace(BR2, BR)
        End If

        Return p.SkippedAssistantTips.Remove(message.MD5Hash)
    End Function

    Function Assistant(Optional refreshScript As Boolean = True) As Boolean
        If SkipAssistant Then Return False

        If ThemeRefresh Then
            ApplyTheme()
            ThemeRefresh = False
        End If

        If refreshScript Then
            p.Script.Synchronize(False, False)
        ElseIf p.Script.Info.FrameCount = 0 Then
            Using server = FrameServerFactory.Create(p.Script.Path)
                p.Script.Info = server.Info
                p.script.Error = server.Error
            End Using
        End If

        g.CheckForModifiedDolbyVisionLevel5Data()

        Dim isCropped = p.Script.IsFilterActive("Crop")
        Dim isResized = p.Script.IsFilterActive("Resize")

        tbTargetWidth.ReadOnly = Not isResized
        tbTargetHeight.ReadOnly = Not isResized

        Dim cropw = p.SourceWidth
        Dim croph = p.SourceHeight

        If isCropped Then
            cropw = p.SourceWidth - p.CropLeft - p.CropRight
            croph = p.SourceHeight - p.CropTop - p.CropBottom
        End If

        Dim isValidAnamorphicSize = (p.TargetWidth = 1440 AndAlso p.TargetHeight = 1080) OrElse
            (p.TargetWidth = 960 AndAlso p.TargetHeight = 720)

        If Not isResized Then
            If p.TargetWidth <> cropw Then
                tbTargetWidth.Text = cropw.ToString
            End If

            If p.TargetHeight <> croph Then
                tbTargetHeight.Text = croph.ToString
            End If
        End If

        lAspectRatioError.Text = Calc.GetAspectRatioError.ToInvariantString("f2") + "%"
        lCrop.Text = If(isCropped, $"{cropw}/{croph}", "disabled")
        lCrop2.Text = If(isCropped, $"{p.CropLeft}, {p.CropRight} / {p.CropTop}, {p.CropBottom}", "-")

        Dim widthZoom = p.TargetWidth / cropw * 100
        Dim heightZoom = p.TargetHeight / croph * 100

        lZoom.Text = widthZoom.ToInvariantString("f1") + "/" + heightZoom.ToInvariantString("f1")
        lPixel.Text = CInt(p.TargetWidth * p.TargetHeight).ToString

        SetSlider()
        Dim trackBarValue = CInt(p.TargetWidth \ Math.Max(TrackBarInterval, p.ForcedOutputMod) - (tbResize.Maximum \ 3))
        trackBarValue = Math.Min(trackBarValue, tbResize.Maximum)
        trackBarValue = Math.Max(trackBarValue, tbResize.Minimum)
        tbResize.Value = trackBarValue


        Dim par = Calc.GetTargetPAR

        lPAR.Text = If(Calc.IsARSignalingRequired OrElse (par.X = 1 AndAlso par.Y = 1), par.X & ":" & par.Y, "n/a")
        lDAR.Text = Calc.GetTargetDAR.ToInvariantString.Shorten(8)
        lSAR.Text = (p.TargetWidth / p.TargetHeight).ToInvariantString.Shorten(8)
        lSourceDar.Text = Calc.GetSourceDAR.ToInvariantString.Shorten(8)
        par = Calc.GetSimpleSourcePAR
        lSourcePAR.Text = par.X & ":" & par.Y

        If p.SourceSeconds > 0 Then
            Dim size = If(p.SourceVideoSize > 0, p.SourceVideoSize, p.SourceSize)
            Dim sizeText = If(size / PrefixedSize(2).Factor < SizePrefix.Base, CInt(size / PrefixedSize(2).Factor).ToString + PrefixedSize(2).Unit, (size / PrefixedSize(3).Factor).ToString("f1") + PrefixedSize(3).Unit)

            If size <> p.SourceVideoSize Then
                sizeText = $"[{sizeText}]"
            End If

            lSource1.Text = lSource1.GetMaxTextSpace(
                g.GetTimeString(p.SourceSeconds),
                sizeText,
                If(p.SourceBitrate > 0, (p.SourceBitrate / 1000).ToInvariantString("f1") + "Mb/s", ""),
                p.SourceFrameRate.ToInvariantString.Shorten(9) + "fps",
                p.SourceFrameRateMode)

            lSource2.Text = lSource1.GetMaxTextSpace(
                p.SourceWidth.ToString + "x" + p.SourceHeight.ToString, p.SourceColorSpace,
                p.SourceChromaSubsampling, If(p.SourceVideoBitDepth <> 0, p.SourceVideoBitDepth & "Bits", ""),
                p.SourceVideoHdrFormat,
                p.SourceScanType, If(p.SourceScanType.EqualsAny("Interlaced", "MBAFF"), p.SourceScanOrder, ""),
                p.SourceVideoFormat, p.SourceVideoFormatProfile)

            lTarget1.Text = lSource1.GetMaxTextSpace(g.GetTimeString(p.TargetSeconds),
                p.TargetFrameRate.ToInvariantString.Shorten(9) + "fps", p.Script.Info.Width & "x" & p.Script.Info.Height,
                "Audio Bitrate: " & CInt(Calc.GetAudioBitrate))

            If p.VideoEncoder.IsCompCheckEnabled Then
                laTarget2.Text = lSource1.GetMaxTextSpace(
                    "Quality: " & CInt(Calc.GetPercent).ToInvariantString() + " %",
                    "Compressibility: " + p.Compressibility.ToInvariantString("f2"))
            Else
                Dim subtitles = p.VideoEncoder.Muxer.Subtitles.Where(Function(i) i.Enabled)
                laTarget2.Text = "Subtitles: " & subtitles.Count & " " + subtitles.Select(Function(i) i.TypeName).Distinct.Join("/")
            End If
        Else
            lTarget1.Text = ""
            lSource1.Text = ""
            laTarget2.Text = ""
            lSource2.Text = ""
        End If

        AssistantClickAction = Nothing
        CanIgnoreTip = True
        AssistantPassed = False

        If p.VideoEncoder.Muxer.TagFile <> "" AndAlso File.Exists(p.VideoEncoder.Muxer.TagFile) AndAlso
            p.VideoEncoder.Muxer.Tags.Count > 0 Then

            If ProcessTip("In the container options there is both a tag file and tags in the Tags tab defined. Only one can be used, the file will be ignored.") Then
                Return Warn("Tags are defined twice")
            End If
        End If

        If p.VideoEncoder.Muxer.CoverFile <> "" AndAlso TypeOf p.VideoEncoder.Muxer Is MkvMuxer Then
            If Not p.VideoEncoder.Muxer.CoverFile.Base.EqualsAny("cover", "small_cover", "cover_land", "small_cover_land") OrElse
                Not p.VideoEncoder.Muxer.CoverFile.Ext.EqualsAny("jpg", "png") Then

                If ProcessTip("The cover file name bust be cover, small_cover, cover_land or small_cover_land, the file type must be jpg or png.") Then
                    Return Block("Invalid Cover File Name")
                End If
            End If
        End If

        If TypeOf p.VideoEncoder Is BasicVideoEncoder Then
            Dim enc = DirectCast(p.VideoEncoder, BasicVideoEncoder)
            Dim param = enc.CommandLineParams.GetOptionParam("--vpp-resize")

            If param IsNot Nothing AndAlso param.Value > 0 AndAlso
                Not p.Script.IsFilterActive("Resize", "Hardware Encoder") Then

                If ProcessTip("In order to use a resize filter of the hardware encoder select 'Hardware Encoder' as resize filter from the filters menu.") Then
                    Return Block("Invalid Filter Setting")
                End If
            End If
        End If

        If Not p.BatchMode Then
            If p.SourceFile <> "" AndAlso p.Script.IsAviSynth AndAlso Not TextEncoding.ArePathsSupportedByProcessEncoding AndAlso
                ProcessTip("AviSynth Unicode support requires at least Windows 10 1903.") Then
                Return Block("Text Encoding Limitation", lgbEncoder.Label)
            End If

            If p.Script.Filters.Count = 0 OrElse Not p.Script.Filters(0).Active OrElse
                p.Script.Filters(0).Category <> "Source" Then

                If ProcessTip("The first filter must have the category Source.") Then
                    Return Block("Invalid Filter Setup")
                End If
            End If

            If p.SourceSeconds = 0 AndAlso ProcessTip("Click here to open a source file.") Then
                CanIgnoreTip = False
                Return Warn("Assistant", AddressOf ShowOpenSourceDialog)
            End If

            If p.SourceFile = p.TargetFile AndAlso ProcessTip("The source and target filepath is identical.") Then
                Return Block("Invalid Target Path", tbSourceFile, tbTargetFile)
            End If

            If p.RemindToCrop AndAlso TypeOf p.VideoEncoder IsNot NullEncoder AndAlso
                ProcessTip("Click here to open the crop dialog. When done continue with Next.") Then

                Return Warn("Crop", AddressOf ShowCropDialog)
            End If

            If p.WarnNoAudio Then
                If Not p.AudioTracks.Where(Function(track) TypeOf track.AudioProfile IsNot NullAudioProfile AndAlso track.AudioProfile.File <> "")?.Any() Then
                    If ProcessTip("There will be no audio in the output file.") Then
                        Return Warn("No audio", p.AudioTracks.Select(Function(x) x.TextEdit).ToArray())
                    End If
                End If
            End If

            If p.WarnIdenticalAudio Then
                Dim fileGroups = p.AudioTracks.Where(Function(x) x.TextEdit.Text <> "" AndAlso x.AudioProfile.Stream Is Nothing).GroupBy(Function(g) g.AudioProfile.File).Where(Function(x) x.Count() > 1)
                If fileGroups.Any() Then
                    If ProcessTip($"Some audio source files are identical.") Then
                        Return Warn("Suspicious Audio Settings", fileGroups.SelectMany(Function(x) x.AsEnumerable().Select(Function(s) s.TextEdit)).ToArray())
                    End If
                End If

                Dim streamGroups = p.AudioTracks.Where(Function(x) x.AudioProfile.Stream IsNot Nothing).GroupBy(Function(g) g.AudioProfile.Stream.Index).Where(Function(x) x.Count() > 1)
                If streamGroups.Any() Then
                    If ProcessTip($"Some audio source streams are identical.") Then
                        Return Warn("Suspicious Audio Settings", streamGroups.SelectMany(Function(x) x.AsEnumerable().Select(Function(s) s.TextEdit)).ToArray())
                    End If
                End If
            End If

            If Not p.VideoEncoder.Muxer.IsSupported(p.VideoEncoder.OutputExt) Then
                If ProcessTip("The encoder outputs '" + p.VideoEncoder.OutputExt + "' but the container '" + p.VideoEncoder.Muxer.Name + "' supports only " + p.VideoEncoder.Muxer.SupportedInputTypes.Join(", ") + ".") Then
                    Return Block("Encoder conflicts with container", lgbEncoder.Label, llMuxer)
                End If
            End If

            For Each ap In AudioProfile.GetProfiles
                If ap.File = "" Then Continue For

                If TypeOf ap Is GUIAudioProfile Then
                    Dim gap = DirectCast(ap, GUIAudioProfile)

                    If (gap.AudioCodec = AudioCodec.AC3 OrElse gap.AudioCodec = AudioCodec.EAC3) AndAlso
                        (gap.Channels = 7 OrElse gap.Channels = 8) AndAlso gap.GetEncoder = GuiAudioEncoder.ffmpeg Then

                        If ProcessTip("AC3/EAC3 6.1/7.1 is not supported by ffmpeg.") Then
                            Return Block("Invalid Audio Channel Count", GetAudioTextBox(ap))
                        End If
                    End If
                End If

                If ap.AudioCodec = AudioCodec.AC3 AndAlso CInt(ap.Bitrate) Mod If(CInt(ap.Bitrate) > 256, 64, 32) <> 0 Then
                    If ProcessTip($"The AC3 bitrate {CInt(ap.Bitrate)} is not specification compliant.") Then
                        Return Warn("Invalid Audio Bitrate", GetAudioTextBox(ap))
                    End If
                End If

                If ap.File = p.TargetFile AndAlso ProcessTip("The audio source and target filepath is identical.") Then
                    Return Block("Invalid Targetpath", GetAudioTextBox(ap), tbTargetFile)
                End If

                If Math.Abs(ap.Delay) > 2000 AndAlso ProcessTip("The audio delay is unusual high indicating a sync problem.") Then
                    Return Warn("High Audio Delay", GetAudioTextBox(ap))
                End If

                If Not p.VideoEncoder.Muxer.IsSupported(ap.OutputFileType) AndAlso Not ap.OutputFileType = "ignore" Then
                    If ProcessTip("The audio format is '" + ap.OutputFileType + "' but the container '" +
                        p.VideoEncoder.Muxer.Name + "' supports only " +
                        p.VideoEncoder.Muxer.SupportedInputTypes.Join(", ") +
                        ". Select another audio profile or another container.") Then

                        Return Block("Audio format not compatible with container", GetAudioTextBox(ap), llMuxer)
                    End If
                End If
            Next

            If p.VideoEncoder.Muxer.OutputExtFull <> p.TargetFile.ExtFull Then
                If ProcessTip("The container requires " + p.VideoEncoder.Muxer.OutputExt.ToUpperInvariant + " as target file type.") Then
                    Return Block("Invalid File Type", tbTargetFile)
                End If
            End If

            If p.VideoEncoder.GetError IsNot Nothing Then
                If ProcessTip(p.VideoEncoder.GetError) Then
                    Return Block("Encoder Error")
                End If
            End If

            Dim ae = Calc.GetAspectRatioError()

            If Not isValidAnamorphicSize AndAlso (ae > p.MaxAspectRatioError OrElse ae < -p.MaxAspectRatioError) AndAlso
                isResized AndAlso p.WarnArError AndAlso p.CustomTargetPAR <> "1:1" Then

                If ProcessTip("Use the resize slider to correct the aspect ratio error or click Next to encode anamorphic.") Then
                    Return Warn("Aspect Ratio Error", lAspectRatioError)
                End If
            End If

            If p.RemindToSetFilters AndAlso ProcessTip("Verify the filter setup, when done continue with Next.") Then
                Return Warn("Filter Setup")
            End If

            If p.Ranges.Count = 0 Then
                If p.RemindToCut AndAlso TypeOf p.VideoEncoder IsNot NullEncoder AndAlso
                    ProcessTip("Click here to open the preview for cutting if necessary. When done continue with Next.") Then

                    Return Warn("Cutting", AddressOf ShowPreview)
                End If
            Else
                If p.CutFrameRate <> p.Script.Info.FrameRate Then
                    If ProcessTip("The frame rate was changed after cutting was performed, please ensure that this change is happening after the Cutting filter section in the AviSynth script.") Then
                        Return Warn("Frame Rate Change")
                    End If
                End If

                If Not p.Script.IsFilterActive("Cutting") AndAlso Form.ActiveForm Is Me Then
                    If ProcessTip("The cutting filter settings don't match with the cutting settings used in the preview." + BR +
                                  "This can usually be fixed by opening and closing the preview.") Then
                        Return Block("Invalid Cutting Settings")
                    End If
                End If
            End If

            If p.RemindToDoCompCheck AndAlso p.VideoEncoder.IsCompCheckEnabled AndAlso p.Compressibility = 0 Then
                If ProcessTip("Click here to start the compressibility check. The compressibility check helps to finds the ideal bitrate or image size.") Then
                    Return Warn("Compressibility Check", AddressOf p.VideoEncoder.RunCompCheck, laTarget2)
                End If
            End If

            If Not p.TargetFile.IsValidPath() Then
                If ProcessTip("The target file path is invalid." + BR + p.TargetFile) Then
                    Return Warn("Invalid Target File", tbTargetFile)
                End If
            End If

            If File.Exists(p.TargetFile) Then
                If FileTypes.VideoText.Contains(p.SourceFile.Ext) AndAlso p.SourceFile.ReadAllText.Contains(p.TargetFile) Then
                    If ProcessTip("Source and target name are identical, please select another target name.") Then
                        Return Block("Invalid Target File", tbTargetFile)
                    End If
                Else
                    If ProcessTip("The target file already exists." + BR + p.TargetFile) Then
                        Return Warn("Target File", tbTargetFile)
                    End If
                End If
            End If

            If TypeOf p.VideoEncoder.Muxer Is MkvMuxer AndAlso Not String.IsNullOrWhiteSpace(p.VideoEncoder.Muxer.TimestampsFile) Then
                Dim sfc = p.SourceScript.GetFrameCount()
                Dim tfc = p.Script.GetFrameCount()
                If sfc <> tfc Then
                    If ProcessTip("The duration changed and doesn't fit the original timestamps, which can cause unwanted results. You can delete/change the timestamps file selection under Container Options > Options.") Then
                        Return Warn("Changed Length", lSource1, lTarget1)
                    End If
                End If

                Dim sfr = p.SourceScript.GetFramerate()
                Dim tfr = p.Script.GetFramerate()
                If sfr <> tfr Then
                    If ProcessTip("The frame rate changed and could not fit the original timestamps anymore, which can cause unwanted results. You can delete/change the timestamps file selection under Container Options > Options.") Then
                        Return Warn("Changed Frame Rate", lSource1, lTarget1)
                    End If
                End If

            End If

            If p.Script.IsFilterActive("Crop") AndAlso Not p.VideoEncoder?.IsOvercroppingAllowed AndAlso p.HdrDolbyVisionMetadataFile IsNot Nothing Then
                Dim side = ""
                Dim by = 0
                Dim c = p.HdrDolbyVisionMetadataFile.Crop
                Dim leftOvercropping = p.CropLeft - c.Left
                Dim topOvercropping = p.CropTop - c.Top
                Dim rightOvercropping = p.CropRight - c.Right
                Dim bottomOvercropping = p.CropBottom - c.Bottom

                If leftOvercropping > 0 Then
                    side = "left"
                    by = leftOvercropping
                ElseIf p.CropTop > c.Top Then
                    side = "top"
                    by = topOvercropping
                ElseIf p.CropRight > c.Right Then
                    side = "right"
                    by = rightOvercropping
                ElseIf p.CropBottom > c.Bottom Then
                    side = "bottom"
                    by = bottomOvercropping
                End If

                If by > 0 Then
                    If ProcessTip($"You have cropped the {side} side by {by}px too much.{BR}Decrease the crop to continue and ensure a valid result.") Then
                        CanIgnoreTip = False
                        Return Warn("Overcropping", AddressOf ShowCropDialog)
                    End If
                End If
            End If

            If p.Script.IsFilterActive("Cutting") AndAlso p.Ranges?.Any() AndAlso p.VideoEncoder.DolbyVisionMetadataPath IsNot Nothing Then
                If ProcessTip($"Cutting is currently not supported for Dolby Vision encodes.{BR}Please remove the cut(s) or the RPU file from encoder options to get a valid Dolby Vision result.") Then
                    CanIgnoreTip = False
                    Return Warn("Cutting not allowed")
                End If
            End If

            If p.Script.IsFilterActive("Resize") AndAlso widthZoom <> heightZoom AndAlso p.VideoEncoder?.IsResizingAllowed AndAlso Not p.VideoEncoder?.IsUnequalResizingAllowed Then
                If ProcessTip("Resizing of that kind will interfere with the Dolby Vision metadata. Keep the original aspect ratio, disable the 'Resize' filter or remove the Dolby Vision RPU file.") Then
                    CanIgnoreTip = False
                    Return Warn("Wrong resizing", tbTargetWidth, tbTargetHeight, lSAR, lDAR, lZoom, lAspectRatioError)
                End If
            End If

            If TypeOf p.VideoEncoder Is x265Enc Then
                Dim x265 = DirectCast(p.VideoEncoder, x265Enc)
                Dim rpuParam = x265.CommandLineParams.GetStringParam(x265.Params.DolbyVisionRpu.Switch)
                Dim bufsizeParam = x265.CommandLineParams.GetNumParam(x265.Params.VbvBufSize.Switch)
                Dim maxrateParam = x265.CommandLineParams.GetNumParam(x265.Params.VbvMaxRate.Switch)

                Dim optionsLabel = DirectCast(pnEncoder.Controls(0), x265Control).blConfigCodec
                If Not String.IsNullOrWhiteSpace(rpuParam?.Value) Then
                    If bufsizeParam?.Value < 1 Then
                        If ProcessTip("Dolby Vision requires VBV settings to enable HRD.") Then
                            CanIgnoreTip = False
                            Return Warn("Missing VBV settings", Sub() p.VideoEncoder.ShowConfigDialog(x265.Params.VbvBufSize.Path), optionsLabel)
                        End If
                    ElseIf maxrateParam?.Value < 1 Then
                        If ProcessTip("Dolby Vision requires VBV settings to enable HRD.") Then
                            CanIgnoreTip = False
                            Return Warn("Missing VBV settings", Sub() p.VideoEncoder.ShowConfigDialog(x265.Params.VbvMaxRate.Path), optionsLabel)
                        End If
                    End If
                End If
            End If

            If p.Script.IsAviSynth AndAlso TypeOf p.VideoEncoder Is x264Enc AndAlso
                Not Package.x264.Version.ToLowerInvariant.ContainsAny("amod", "djatom", "patman") AndAlso
                p.Script.Info.ColorSpace <> ColorSpace.YUV420P8 AndAlso
                p.Script.Info.ColorSpace <> ColorSpace.YUV420P8_ AndAlso
                p.Script.Info.ColorSpace <> ColorSpace.YUV422P8 AndAlso
                p.Script.Info.ColorSpace <> ColorSpace.YUV444P8 AndAlso
                p.Script.Info.ColorSpace <> ColorSpace.BGR32 AndAlso
                Not g.ContainsPipeTool(p.VideoEncoder.GetCommandLine(True, True)) Then

                If ProcessTip("x264 AviSynth input supports only YUV420P8, YUV422P8, YUV444P8 and BGR32 " +
                             $"as input colorspace.{BR}Consider to use a pipe tool: " +
                              "x264 Options > Input/Output > Pipe > avs2pipemod y4m") Then
                    Return Block("Incompatible colorspace")
                End If
            End If

            If p.Script.Info.Width Mod p.ForcedOutputMod <> 0 AndAlso (Not p.ForcedOutputModOnlyIfCropped OrElse p.Script.Info.Width <> p.SourceScript.Info.Width) Then
                Dim tip = "Change output width to be divisible by " & p.ForcedOutputMod & " or customize:" + BR + "Options > Image > Output Mod"
                If Not p.ForcedOutputModIgnorable Then RemoveTip(tip)
                If ProcessTip(tip) Then
                    CanIgnoreTip = p.ForcedOutputModIgnorable
                    Return Warn("Invalid Target Width", tbTargetWidth, lSAR)
                End If
            End If

            If p.Script.Info.Height Mod p.ForcedOutputMod <> 0 AndAlso (Not p.ForcedOutputModOnlyIfCropped OrElse p.Script.Info.Height <> p.SourceScript.Info.Height) Then
                Dim tip = "Change output height to be divisible by " & p.ForcedOutputMod & " or customize:" + BR + "Options > Image > Output Mod"
                If Not p.ForcedOutputModIgnorable Then RemoveTip(tip)
                If ProcessTip(tip) Then
                    CanIgnoreTip = p.ForcedOutputModIgnorable
                    Return Warn("Invalid Target Height", tbTargetHeight, lSAR)
                End If
            End If

            If p.VideoEncoder.IsCompCheckEnabled AndAlso p.Compressibility > 0 Then
                Dim value = Calc.GetPercent

                If value < (p.VideoEncoder.AutoCompCheckValue - 20) OrElse
                    value > (p.VideoEncoder.AutoCompCheckValue + 20) Then

                    If ProcessTip("Aimed quality value is more than 20% off, change the image or file size to get something between 50% and 70% quality.") Then
                        Return Warn("Quality", tbTargetSize, tbBitrate, tbTargetWidth, tbTargetHeight, laTarget2)
                    End If
                End If
            End If

            If TypeOf p.VideoEncoder.Muxer Is MP4Muxer Then
                For Each i In p.VideoEncoder.Muxer.Subtitles
                    If Not i.Path.Ext.EqualsAny("idx", "srt", "sub") Then
                        If ProcessTip("MP4 supports only SUB, SRT and IDX subtitles.") Then
                            Return Block("Invalid subtitle format")
                        End If
                    End If
                Next
            End If

            If refreshScript AndAlso Not (MouseButtons = MouseButtons.Left AndAlso ActiveControl Is tbResize) Then
                Dim err = p.Script.Error

                If err <> "" AndAlso ProcessTip(err) Then
                    Return Block("Click on the error message", Sub() MsgError("Script Error", err))
                End If
            End If
        Else
            If p.SourceFiles.Count = 0 Then
                If ProcessTip("Click here to open a source file.") Then
                    CanIgnoreTip = False
                    Return Warn("Assistant", AddressOf ShowOpenSourceDialog)
                End If
            End If
        End If

        'flicker issue
        If gbAssistant.Text <> "Add Job" Then
            gbAssistant.Text = "Add Job"
        End If

        If laTip.Font.Size <> 9 Then
            laTip.SetFontSize(9)
        End If

        laTip.Text = "Click on the button to the right to add a job to the job list."
        AssistantPassed = True
        bnNext.Enabled = True
        laTip.BackColor = ThemeManager.CurrentTheme.MainForm.laTipBackColor
        laTip.ForeColor = ThemeManager.CurrentTheme.MainForm.laTipForeColor
        UpdateNextButton()
    End Function

    Function Warn(msg As String, ParamArray controls As Control()) As Boolean
        Warn(msg, Nothing, controls)
    End Function

    Function Warn(msg As String, clickAction As Action, ParamArray controls As Control()) As Boolean
        AssistantClickAction = clickAction
        bnNext.Enabled = True

        If msg <> gbAssistant.Text Then
            gbAssistant.Text = msg
        End If

        Highlight(True, controls)
        UpdateNextButton()
    End Function

    Function Block(msg As String, ParamArray controls As Control()) As Boolean
        Block(msg, Nothing, controls)
    End Function

    Function Block(msg As String, clickAction As Action, ParamArray controls As Control()) As Boolean
        AssistantClickAction = clickAction
        gbAssistant.Text = msg
        bnNext.Enabled = False
        CanIgnoreTip = False
        ThemeRefresh = True
        Highlight(True, controls)
        UpdateNextButton()
    End Function

    Sub Highlight(ParamArray controls As Control())
        Highlight(False, controls)
    End Sub

    Sub Highlight(highlight As Boolean, ParamArray controls As Control())
        Dim theme = ThemeManager.CurrentTheme

        If highlight Then
            laTip.BackColor = theme.MainForm.laTipBackHighlightColor
            laTip.ForeColor = theme.MainForm.laTipForeHighlightColor
        Else
            laTip.BackColor = theme.MainForm.laTipBackColor
            laTip.ForeColor = theme.MainForm.laTipForeColor
        End If

        If controls Is Nothing Then Return
        If Not controls.NothingOrEmpty Then ThemeRefresh = True

        For Each control In controls.OfType(Of Label)
            control.BackColor = theme.General.Controls.Label.BackHighlightColor
            control.ForeColor = theme.General.Controls.Label.ForeHighlightColor
        Next

        For Each control In controls.OfType(Of ButtonLabel)
            control.BackColor = If(TypeOf control.Parent Is UserControl, theme.General.Controls.ListView.BackHighlightColor, theme.General.Controls.ButtonLabel.BackHighlightColor)
            control.ForeColor = theme.General.Controls.ButtonLabel.ForeHighlightColor
            control.LinkColor = theme.General.Controls.ButtonLabel.ForeHighlightColor
            control.LinkHoverColor = theme.General.Controls.ButtonLabel.ForeHighlightColor
        Next

        For Each control In controls.OfType(Of Button)
            control.BackColor = theme.General.Controls.Button.BackHighlightColor
            control.ForeColor = theme.General.Controls.Button.ForeHighlightColor
        Next

        For Each control In controls.OfType(Of GroupBox)
            control.BackColor = theme.General.Controls.GroupBox.BackHighlightColor
            control.ForeColor = theme.General.Controls.GroupBox.ForeHighlightColor
        Next

        For Each control In controls.OfType(Of Panel)
            control.BackColor = theme.General.Controls.Panel.BackHighlightColor
            control.ForeColor = theme.General.Controls.Panel.ForeHighlightColor
        Next

        For Each control In controls.OfType(Of TableLayoutPanel)
            control.BackColor = theme.General.Controls.TableLayoutPanel.BackHighlightColor
            control.ForeColor = theme.General.Controls.TableLayoutPanel.ForeHighlightColor
        Next

        For Each control In controls.OfType(Of TextBox)
            control.BackColor = theme.General.Controls.TextBox.BackHighlightColor
            control.ForeColor = theme.General.Controls.TextBox.ForeHighlightColor
        Next

        For Each control In controls.OfType(Of TextBoxEx)
            control.BorderColor = theme.General.Controls.TextBox.BackHighlightColor
            control.BorderFocusedColor = theme.General.Controls.TextBox.ForeHighlightColor
        Next

        For Each control In controls.OfType(Of TextEdit)
            control.TextBox.BackColor = theme.General.Controls.TextEdit.BackHighlightColor
            control.TextBox.ForeColor = theme.General.Controls.TextEdit.ForeHighlightColor
        Next

        For Each control In controls.OfType(Of TrackBar)
            control.BackColor = theme.General.Controls.TrackBar.BackHighlightColor
            control.ForeColor = theme.General.Controls.TrackBar.ForeHighlightColor
        Next
    End Sub

    Sub OpenTargetFolder()
        g.ShellExecute(p.TargetFile.Dir)
    End Sub

    Function AbortDueToLowDiskSpace() As Boolean
        Try 'crashes with network shares
            If p.TargetFile = "" OrElse p.TargetFile.StartsWith("\\") Then
                Exit Function
            End If

            Dim di As New DriveInfo(p.TargetFile.Dir)

            If di.AvailableFreeSpace / PrefixedSize(3).Factor < s.MinimumDiskSpace Then
                Using td As New TaskDialog(Of String)
                    td.Title = "Low Disk Space"
                    td.Content = $"The target drive {Path.GetPathRoot(p.TargetFile)} has only " +
                                 $"{(di.AvailableFreeSpace / PrefixedSize(3).Factor):f2} {PrefixedSize(3).Unit} free disk space."
                    td.Icon = TaskIcon.Warning
                    td.AddButton("Continue")
                    td.AddButton("Abort")

                    If td.Show <> "Continue" Then
                        Return True
                    End If
                End Using
            End If
        Catch
        End Try
    End Function

    <Command("Creates a job and runs the job list.")>
    Sub StartEncoding()
        AssistantPassed = True
        AddJob(False, "")
        g.ProcessJobs()
    End Sub

    <Command("Extract dynamic HDR metadata from a source file.")>
    Sub ExtractHdrMetadata(sourcePath As String)
        Dim proj = New Project() With {
            .SourceFile = sourcePath,
            .FirstOriginalSourceFile = sourcePath
        }
        ExtractHdrMetadata(proj)
    End Sub

    Sub ExtractHdrMetadata(proj As Project)
        ExtractHdr10PlusMetadata(proj)
        ExtractDolbyVisionMetadata(proj)
    End Sub

    Sub ExtractHdr10PlusMetadata(proj As Project)
        If proj Is Nothing Then Return
        If String.IsNullOrWhiteSpace(proj.SourceFile) Then Return
        If Not File.Exists(proj.SourceFile) Then Return

        Dim sourcePath = proj.SourceFile

        Dim fileHdrFormat = MediaInfo.GetVideo(sourcePath, "HDR_Format_Commercial")
        If fileHdrFormat?.ContainsAny("HDR10+") Then
            Dim jsonPath = sourcePath.ChangeExt("json")
            If Not String.IsNullOrWhiteSpace(proj.TempDir) Then
                jsonPath = If(sourcePath.Contains(proj.TempDir), jsonPath, If(proj.TempDir.DirExists(), $"{Path.Combine({proj.TempDir, "HDR10PlusMetadata.json"})}", jsonPath))
            End If

            If Not jsonPath.FileExists() Then
                Dim commandLine = $"{Package.ffmpeg.Path.Escape} -hide_banner -probesize 50M -i ""{sourcePath}"" -an -sn -dn -c:v copy -bsf:v hevc_mp4toannexb -f hevc - | {Package.HDR10PlusTool.Path.Escape} extract -o ""{jsonPath}"" -"

                Try
                    Using proc As New Proc
                        proc.Priority = ProcessPriorityClass.Normal
                        proc.Package = Package.HDR10PlusTool
                        proc.Project = If(proj, p)
                        proc.Header = "Extract HDR10+ metadata"
                        proc.Encoding = Encoding.UTF8
                        proc.File = "cmd.exe"
                        proc.Arguments = "/S /C """ + commandLine + """"
                        proc.SkipStrings = Proc.GetSkipStrings(commandLine)
                        proc.AllowedExitCodes = {0}
                        proc.OutputFiles = {jsonPath}
                        proc.Start()
                    End Using

                    If jsonPath?.FileExists() Then
                        Try
                            Dim fi = New FileInfo(jsonPath)
                            If fi.Length < 100 Then
                                File.Delete(jsonPath)
                            End If
                        Catch ex As Exception
                            jsonPath = ""
                        End Try
                    End If
                Catch ex As AbortException
                    Throw ex
                Catch ex As Exception
                    g.ShowException(ex)
                    Throw New AbortException
                Finally
                    Log.Save()
                End Try
            End If

            proj.Hdr10PlusMetadataFile = jsonPath
        End If
    End Sub

    Sub ExtractDolbyVisionMetadata(proj As Project)
        If proj Is Nothing Then Return
        If String.IsNullOrWhiteSpace(proj.SourceFile) Then Return
        If Not File.Exists(proj.SourceFile) Then Return

        Dim sourcePath = proj.SourceFile
        Dim isEL = False

        Dim files = Directory.GetFiles(sourcePath.Dir, sourcePath.Base + "*_EL*.*", SearchOption.TopDirectoryOnly).AsEnumerable()
        files = files?.Where(Function(x) {"h265", "hevc"}.Contains(x.Ext))
        If files?.Any() Then
            sourcePath = files.First()
            isEL = True
        End If

        Dim format = MediaInfo.GetVideoFormat(sourcePath)
        If format <> "HEVC" Then Return

        Dim fileHdrFormat = MediaInfo.GetVideo(sourcePath, "HDR_Format/String")
        If isEL OrElse (Regex.IsMatch(fileHdrFormat, "Dolby Vision.*Profile|HDR10+ Profile B")) Then
            Dim rpuPath = sourcePath.ChangeExt("rpu")
            Dim doviFile = proj.HdrDolbyVisionMetadataFile

            If Not String.IsNullOrWhiteSpace(proj.TempDir) Then
                rpuPath = If(sourcePath.Contains(proj.TempDir), rpuPath, If(proj.TempDir.DirExists(), $"{Path.Combine({proj.TempDir, "HDRDVmetadata.rpu"})}", rpuPath))
            End If

            Try
                If Not rpuPath.FileExists() Then
                    Dim commandLine = $"{Package.ffmpeg.Path.Escape} -hide_banner -probesize 50M -i ""{sourcePath}"" -an -sn -dn -c:v copy -bsf:v hevc_mp4toannexb -f hevc - | {Package.DoViTool.Path.Escape} extract-rpu - -o ""{rpuPath}"""
                    Dim throwError As (Title As String, Content As String)

                    Using proc As New Proc
                        Dim throwIf = Sub(value As String)
                                          If value?.Contains("Unexpected RPU NALU") Then
                                              throwError = ("Unexpected RPU NALU", "Unexpected RPU NALU found, extraction aborts...")
                                              proc.Kill()
                                          End If
                                          If value?.Contains("Discarding") Then
                                              throwError = ("Discarding", "Dovi_tool is discarding data, extraction aborts...")
                                              proc.Kill()
                                          End If
                                      End Sub
                        proc.Priority = ProcessPriorityClass.Normal
                        proc.Package = Package.DoViTool
                        proc.Project = If(proj, p)
                        proc.Header = "Extract Dolby Vision metadata"
                        proc.Encoding = Encoding.UTF8
                        proc.File = "cmd.exe"
                        proc.Arguments = "/S /C """ + commandLine + """"
                        proc.SkipStrings = Proc.GetSkipStrings(commandLine)
                        proc.AllowedExitCodes = {0}
                        proc.OutputFiles = {rpuPath}
                        proc.ReadOutput = True
                        AddHandler proc.OutputDataReceived, throwIf
                        AddHandler proc.ErrorDataReceived, throwIf
                        proc.Start()
                    End Using

                    If throwError.Title IsNot Nothing AndAlso throwError.Content IsNot Nothing Then Throw New ErrorAbortException(throwError.Title, throwError.Content)

                    If rpuPath?.FileExists() Then
                        Try
                            Dim fi = New FileInfo(rpuPath)
                            If fi.Length > 100 Then
                                doviFile = New DolbyVisionMetadataFile(rpuPath)
                            Else
                                File.Delete(rpuPath)
                            End If
                        Catch ex As Exception
                            doviFile = New DolbyVisionMetadataFile(rpuPath)
                        End Try
                    End If
                End If
            Catch ex As ErrorAbortException
                g.ShowException(ex, Nothing, Nothing, s.ErrorMessageTimeout)
                doviFile = Nothing
            Catch ex As AbortException
                g.ShowException(ex)
                doviFile = Nothing
                Throw ex
            Catch ex As Exception
                g.ShowException(ex)
                doviFile = Nothing
                Throw New AbortException
            Finally
                Log.Save()
            End Try

            proj.HdrDolbyVisionMetadataFile = doviFile
        End If
    End Sub

    Async Function FindHdrMetadataAsync(proj As Project) As Task(Of (jsonFile As String, rpuFile As String))
        If proj Is Nothing Then Return Nothing
        If String.IsNullOrWhiteSpace(proj.SourceFile) Then Return Nothing

        Dim sourcePath = proj.SourceFile
        Dim jsonFile As String = ""
        Dim rpuFile As String = ""
        Dim files As IEnumerable(Of String)

        Dim searchTask = Task.Run(Sub()
                                      Try
                                          files = Directory.GetFiles(proj.SourceFile.Dir(), $"{proj.SourceFile.Base}*.*", SearchOption.TopDirectoryOnly)
                                          jsonFile = files.Where(Function(x) {"json"}.Contains(x.Ext) AndAlso Not x.Base.EndsWithAny("_L5", "_Config"))?.FirstOrDefault()
                                          rpuFile = files.Where(Function(x) {"bin", "rpu"}.Contains(x.Ext) AndAlso Not x.Base.EndsWith("_Cropped"))?.FirstOrDefault()

                                          If Not String.IsNullOrWhiteSpace(proj.TempDir) AndAlso String.IsNullOrWhiteSpace(jsonFile) AndAlso String.IsNullOrWhiteSpace(rpuFile) Then
                                              files = Directory.GetFiles(proj.TempDir, "*.*", SearchOption.TopDirectoryOnly)
                                              jsonFile = If(String.IsNullOrWhiteSpace(jsonFile), files?.Where(Function(x) {"json"}.Contains(x.Ext) AndAlso Not x.Base.EndsWithAny("_L5", "_Config"))?.FirstOrDefault(), jsonFile)
                                              rpuFile = If(String.IsNullOrWhiteSpace(rpuFile), files?.Where(Function(x) {"bin", "rpu"}.Contains(x.Ext) AndAlso Not x.Base.EndsWith("_Cropped"))?.FirstOrDefault(), rpuFile)
                                          End If
                                      Catch ex As Exception
                                          Log.WriteLine(ex.Message)
                                          Log.Save()
                                      End Try
                                  End Sub)

        Await searchTask
        Return (jsonFile, rpuFile)
    End Function

    Sub Demux()
        Dim getFormat = Function() As String
                            Dim ret = MediaInfo.GetVideo(p.SourceFile, "Format")

                            Select Case ret
                                Case "MPEG Video"
                                    ret = "mpeg2"
                                Case "VC-1"
                                    ret = "vc1"
                            End Select

                            Return ret.ToLowerInvariant
                        End Function

        Dim srcScript = p.Script.GetFilter("Source").Script.ToLowerInvariant

        For Each i In s.Demuxers
            If Not i.Active AndAlso (i.SourceFilters.NothingOrEmpty OrElse Not srcScript.ContainsAny(i.SourceFilters.Select(Function(val) val.ToLowerInvariant + "(").ToArray)) Then
                Continue For
            End If

            If i.InputExtensions?.Length = 0 OrElse i.InputExtensions.Contains(p.SourceFile.Ext) Then
                If Not srcScript?.Contains("(") OrElse i.SourceFilters.NothingOrEmpty OrElse
                    srcScript.ContainsAny(i.SourceFilters.Select(Function(val) val.ToLowerInvariant + "(").ToArray) Then

                    Dim inputFormats = i.InputFormats.NothingOrEmpty OrElse
                        i.InputFormats.Contains(getFormat())

                    If inputFormats Then
                        i.Run(p)
                        Refresh()

                        For Each outExt In i.OutputExtensions
                            Dim exitFor = False

                            For Each iFile In Directory.GetFiles(p.TempDir, "*." + outExt)
                                If g.IsSourceSame(iFile) AndAlso
                                    p.SourceFile <> iFile AndAlso
                                    Not iFile.Base.EndsWith("_CompCheck") AndAlso
                                    Not iFile.Base.EndsWith("_out") AndAlso
                                    Not iFile.Base.Matches("_chunk\d+$") AndAlso
                                    Not iFile.Base.ContainsEx("_cut_") Then

                                    p.SourceFile = iFile
                                    p.SourceFiles.Clear()
                                    p.SourceFiles.Add(p.SourceFile)

                                    BlockSourceTextBoxTextChanged = True
                                    tbSourceFile.Text = p.SourceFile
                                    BlockSourceTextBoxTextChanged = False

                                    exitFor = True
                                    Exit For
                                End If
                            Next

                            If exitFor Then
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If
        Next

        p.Script.Synchronize()
    End Sub

    Sub Indexing()
        If p.SourceFile.Ext.EqualsAny("avs", "vpy") Then Exit Sub
        If Not p.Script.Path.FileExists Then p.Script.Synchronize(False, False)

        Dim codeLower = p.Script.GetFilter("Source").Script.ToLowerInvariant

        If codeLower.Contains("ffvideosource(") OrElse codeLower.Contains("ffms2(") OrElse codeLower.Contains("ffms2.source") Then
            If FileTypes.VideoIndex.Contains(p.SourceFile.Ext) Then
                p.SourceFile = p.LastOriginalSourceFile
                BlockSourceTextBoxTextChanged = True
                tbSourceFile.Text = p.SourceFile
                BlockSourceTextBoxTextChanged = False
            End If

            Dim cachePath = If(codeLower.Contains("cachefile") AndAlso p.TempDir <> "", Path.Combine(p.TempDir, g.GetSourceBase + ".ffindex"), p.SourceFile + ".ffindex")

            If Not cachePath.FileExists() Then
                g.ffmsindex(p.SourceFile, cachePath)
            End If
        ElseIf codeLower.Contains("lwlibavvideosource(") OrElse codeLower.Contains("lwlibavsource(") Then
            If FileTypes.VideoIndex.Contains(p.SourceFile.Ext) Then
                p.SourceFile = p.LastOriginalSourceFile
                BlockSourceTextBoxTextChanged = True
                tbSourceFile.Text = p.SourceFile
                BlockSourceTextBoxTextChanged = False
            End If

            If Not File.Exists(p.SourceFile + ".lwi") AndAlso
                Not File.Exists(Path.Combine(p.TempDir, g.GetSourceBase + ".lwi")) AndAlso
                File.Exists(p.Script.Path) AndAlso Not FileTypes.VideoText.Contains(p.SourceFile.Ext) Then

                Using proc As New Proc
                    proc.Header = "Index LWLibav"
                    proc.Encoding = Encoding.UTF8
                    proc.SkipString = "Creating lwi"

                    If p.Script.IsAviSynth Then
                        proc.File = Package.ffmpeg.Path
                        proc.Arguments = "-i " + p.Script.Path.LongPathPrefix.Escape + " -hide_banner"
                    Else
                        proc.File = Package.vspipe.Path
                        proc.Arguments = p.Script.Path.Escape + " NUL -i"
                    End If

                    proc.AllowedExitCodes = {0, 1}
                    proc.Start()
                End Using
            End If
        ElseIf codeLower.Contains("lsmashvideosource(") OrElse
            codeLower.Contains("libavsmashsource(") OrElse
            codeLower.Contains("directshowsource(") OrElse
            codeLower.Contains("dss2(") Then

            If FileTypes.VideoIndex.Contains(p.SourceFile.Ext) Then
                p.SourceFile = p.LastOriginalSourceFile
                BlockSourceTextBoxTextChanged = True
                tbSourceFile.Text = p.SourceFile
                BlockSourceTextBoxTextChanged = False
            End If
        End If
    End Sub

    <Command("Shows a file browser to open a project file.")>
    Sub ShowFileBrowserToOpenProject()
        Using dialog As New OpenFileDialog
            dialog.Filter = "Project Files|*.srip"

            If dialog.ShowDialog() = DialogResult.OK Then
                Refresh()
                OpenProject(dialog.FileName)
            End If
        End Using
    End Sub

    <Command("Shows the Event Command dialog.")>
    Sub ShowEventCommandsDialog()
        Using form As New EventCommandsEditor(s.EventCommands)
            If form.ShowDialog() = DialogResult.OK Then
                s.EventCommands.Clear()

                For Each i As ListViewItem In form.lv.Items
                    s.EventCommands.Add(DirectCast(i.Tag, EventCommand))
                Next
            End If
        End Using

        g.SaveSettings()
    End Sub

    Function GetRestartID() As String
        Return s.AviSynthMode & s.VapourSynthMode
    End Function

    <Command("Shows the settings dialog.")>
    Sub ShowSettingsDialog()
        Dim restartID = GetRestartID()
        Dim oldDefaultFont = FontManager.GetDefaultFont()

        Using form As New SimpleSettingsForm("Settings")
            Dim ui = form.SimpleUI
            ui.Store = s


            '################# General
            ui.CreateFlowPage("General", True)

            Dim b = ui.AddBool
            b.Text = "Save projects automatically"
            b.Field = NameOf(s.AutoSaveProject)

            b = ui.AddBool()
            b.Text = "Reverse mouse wheel video seek direction"
            b.Field = NameOf(s.ReverseVideoScrollDirection)

            Dim n = ui.AddNum()
            n.Text = "Number of most recently used projects to keep"
            n.Help = "MRU list shown in the main menu under: File > Recent Projects"
            n.Config = {0, 15}
            n.Field = NameOf(s.ProjectsMruNum)

            n = ui.AddNum()
            n.Text = "Maximum number of parallel processes"
            n.Help = "Maximum number of parallel processes used for audio and video processing. Chunk encoding can be enabled in the encoder options."
            n.Config = {1, 16}
            n.Field = NameOf(s.ParallelProcsNum)

            n = ui.AddNum()
            n.Text = "Timeout error messages on job processing"
            n.Help = "Duration of error messages being shown if an error occures and before the next job shall start."
            n.Config = {-1, 3600, 5, 0}
            n.Field = NameOf(s.ErrorMessageTimeout)


            '############### Generation
            Dim generationPage = ui.CreateFlowPage("Generation", True)

            ui.AddLabel("Position of frame number in file name:")

            Dim previewFrameNumberPosition = ui.AddMenu(Of ImageFrameNumberPosition)()
            previewFrameNumberPosition.Text = "Preview:"
            previewFrameNumberPosition.Field = NameOf(s.SaveImagePreviewFrameNumberPosition)

            Dim videoComparisonFrameNumberPosition = ui.AddMenu(Of ImageFrameNumberPosition)()
            videoComparisonFrameNumberPosition.Text = "Video Comparison:"
            videoComparisonFrameNumberPosition.Field = NameOf(s.SaveImageVideoComparisonFrameNumberPosition)

            ui.AddLine(generationPage)

            b = ui.AddBool()
            b.Text = "Add line numbers to generated code"
            b.Help = ""
            b.Field = NameOf(s.CommandLinePreviewWithLineNumbers)


            '################# Logs
            ui.CreateFlowPage("Logs", True)

            n = ui.AddNum()
            n.Text = "Number of log files to keep"
            n.Help = "Log files can be found at: Tools > Folders > Log Files"
            n.Config = {1, Integer.MaxValue}
            n.Field = NameOf(s.LogFileNum)

            b = ui.AddBool()
            b.Text = "Write Event Commands to log file"
            b.Field = NameOf(s.LogEventCommand)

            b = ui.AddBool()
            b.Text = "Enable debug logging"
            b.Field = NameOf(s.WriteDebugLog)



            '################# Startup
            ui.CreateFlowPage("Startup", True)

            Dim mb = ui.AddMenu(Of String)()
            mb.Text = "Startup Template:"
            mb.Help = "Template loaded when StaxRip starts."
            mb.Field = NameOf(s.StartupTemplate)
            mb.Expanded = True
            Dim templates = Directory.GetFiles(Folder.Template, "*.srip", SearchOption.AllDirectories).OrderBy(Function(x) x.Count(Function(c) c = Path.DirectorySeparatorChar)).ThenBy(Function(x) x)
            For Each template In templates
                Dim text = template.Replace(Folder.Template, "").Trim(Path.DirectorySeparatorChar).Replace(Path.DirectorySeparatorChar, " | ")
                text = text.Substring(0, text.LastIndexOf(".srip"))
                mb.Add(text)
            Next
            mb.Button.SaveAction = Sub(value)
                                       If value <> s.StartupTemplate AndAlso p.SourceFile = "" Then
                                           Dim templatePath = Path.Combine(Folder.Template, value.Replace(" | ", Path.DirectorySeparatorChar) + ".srip")
                                           If templatePath.FileExists() Then LoadProject(templatePath)
                                       End If
                                       UpdateTemplatesMenuAsync()
                                   End Sub
            mb.Button.ShowPath = True

            b = ui.AddBool
            b.Text = "Check for Long Path Support"
            b.Field = NameOf(s.CheckForLongPathSupport)

            b = ui.AddBool()
            b.Text = "Check for updates approx. once per day"
            b.Field = NameOf(s.CheckForUpdates)


            '################# Source Opening
            ui.CreateFlowPage("Source Opening", True)

            ui.AddLabel("Template Selection:")

            Dim stsm = ui.AddMenu(Of ShowTemplateSelectionMode)()
            Dim stsd = ui.AddMenu(Of ShowTemplateSelectionDefaultMode)()
            Dim stst = ui.AddNum()

            stsm.Text = "Show when loading via"
            stsm.Field = NameOf(s.ShowTemplateSelection)
            stsm.Expanded = True
            stsm.Button.ValueChangedAction = Sub(value)
                                                 Dim enabled = value <> ShowTemplateSelectionMode.Never
                                                 stsd.Enabled = enabled
                                                 stst.Enabled = enabled
                                             End Sub
            stsm.Button.ValueChangedAction.Invoke(s.ShowTemplateSelection)

            stsd.Text = "Default timeout action"
            stsd.Field = NameOf(s.ShowTemplateSelectionDefault)
            stsd.Expanded = True
            stsd.Button.ValueChangedAction = Sub(value)
                                                 stst.Enabled = value <> ShowTemplateSelectionDefaultMode.None
                                             End Sub
            stsd.Button.ValueChangedAction.Invoke(s.ShowTemplateSelectionDefault)

            stst.Text = "Timeout"
            stst.Help = "Timeout in seconds the Template Selection is shown befor the current template is used"
            stst.Config = {1, Integer.MaxValue}
            stst.Field = NameOf(s.ShowTemplateSelectionTimeout)


            '############# Quality Definitions
            Dim qualityDefinitionsPage = ui.CreateFlowPage("Quality Definitions", True)

            Dim t = ui.AddText()
            t.Text = "x264 quality definitions"
            t.Help = "Create custom quality definitions for x264." + BR2 +
                         "Use this format to create your custom values with optional description:" + BR +
                         "number""text""" + BR +
                         "number: can be used with optional decimal separator (, or .)" + BR +
                         "text: description, optionally empty" + BR2 +
                         "Example:" + BR +
                         "8""Crazy""_19.5""Personal Default"" 21,5""Why not?!"" 22.0 44,3"
            t.Edit.Expand = True
            t.Edit.Text = s.X264QualityDefinitions.ToSeparatedString()
            t.Edit.SaveAction = Sub(value) s.X264QualityDefinitions = value.ToX264QualityItems()?.ToList()

            t = ui.AddText()
            t.Text = "x265 quality definitions"
            t.Help = "Create custom quality definitions for x265." + BR2 +
                         "Use this format to create your custom values with optional description:" + BR +
                         "number""text""" + BR +
                         "number: can be used with optional decimal separator (, or .)" + BR +
                         "text: description, optionally empty" + BR2 +
                         "Example:" + BR +
                         "8""Crazy""_19.5""Personal Default"" 21,5""Why not?!"" 22.0 44,3"
            t.Edit.Expand = True
            t.Edit.Text = s.X265QualityDefinitions.ToSeparatedString()
            t.Edit.SaveAction = Sub(value) s.X265QualityDefinitions = value.ToX265QualityItems()?.ToList()

            t = ui.AddText()
            t.Text = "SvtAv1EncApp quality definitions"
            t.Help = "Create custom quality definitions for SvtAv1EncApp." + BR2 +
                         "Use this format to create your custom values with optional description:" + BR +
                         "number""text""" + BR +
                         "number: can be used with optional decimal separator (, or .)" + BR +
                         "text: description, optionally empty" + BR2 +
                         "Example:" + BR +
                         "8""Crazy""_19.5""Personal Default"" 21,5""Why not?!"" 22.0 44,3"
            t.Edit.Expand = True
            t.Edit.Text = s.SvtAv1EncAppQualityDefinitions.ToSeparatedString()
            t.Edit.SaveAction = Sub(value) s.SvtAv1EncAppQualityDefinitions = value.ToSvtAv1EncAppQualityItems()?.ToList()

            t = ui.AddText()
            t.Text = "vccencFFapp quality definitions"
            t.Help = "Create custom quality definitions for vccencFFapp." + BR2 +
                         "Use this format to create your custom values with optional description:" + BR +
                         "number""text""" + BR +
                         "number: can be used with optional decimal separator (, or .)" + BR +
                         "text: description, optionally empty" + BR2 +
                         "Example:" + BR +
                         "8""Crazy""_19.5""Personal Default"" 21,5""Why not?!"" 22.0 44,3"
            t.Edit.Expand = True
            t.Edit.Text = s.VvencffappQualityDefinitions.ToSeparatedString()
            t.Edit.SaveAction = Sub(value) s.VvencffappQualityDefinitions = value.ToVvencffappQualityItems()?.ToList()


            '############# System
            Dim systemPage = ui.CreateFlowPage("System", True)

            Dim procPriority = ui.AddMenu(Of ProcessPriorityClass)
            procPriority.Text = "Process Priority"
            procPriority.Help = "Process priority of the applications StaxRip launches."
            procPriority.Expanded = True
            procPriority.Field = NameOf(s.ProcessPriority)

            Dim cmdline = ui.AddMenu(Of CommandLinePreview)
            cmdline.Text = "Command Line Preview"
            cmdline.Expanded = True
            cmdline.Field = NameOf(s.CommandLinePreview)

            n = ui.AddNum
            n.Text = "Minimum Disk Space"
            n.Help = "Minimum allowed disk space in GB before StaxRip shows an error message."
            n.Config = {0, 10000}
            n.Field = NameOf(s.MinimumDiskSpace)

            n = ui.AddNum
            n.Text = "Shutdown Timeout"
            n.Help = "Timeout in seconds before the shutdown is executed."
            n.Config = {0, 10000}
            n.Field = NameOf(s.ShutdownTimeout)

            n = ui.AddNum
            n.Text = "Focus Steal prevention until"
            n.Help = "StaxRip Main window will not steal focus from other active programs within the given time (in seconds) after a job in StaxRip (in the same instance) has started."
            n.Config = {-1, 1000000}
            n.Field = NameOf(s.PreventFocusStealUntil)

            n = ui.AddNum
            n.Text = "Focus Steal prevention after"
            n.Help = "StaxRip Main window will not steal focus from other active programs, if a job in StaxRip (in the same instance) takes longer than the given time (in seconds)."
            n.Config = {-1, 1000000}
            n.Field = NameOf(s.PreventFocusStealAfter)

            b = ui.AddBool
            b.Text = "Minimize processing dialog to tray"
            b.Field = NameOf(s.MinimizeToTray)

            b = ui.AddBool
            b.Text = "Extend error messages with the help of 'Err'"
            b.Field = NameOf(s.ErrorMessageExtendedByErr)

            b = ui.AddBool
            b.Text = "Force close running apps at shutdown or in hybrid mode"
            b.Field = NameOf(s.ShutdownForce)

            b = ui.AddBool
            b.Text = "Prevent system from entering standby mode while encoding"
            b.Field = NameOf(s.PreventStandby)

            b = ui.AddBool
            b.Text = "Prefer Windows Terminal over Powershell if present"
            b.Field = NameOf(s.PreferWindowsTerminal)


            '################# User Interface
            Dim uiPage = ui.CreateFlowPage("User Interface", True)

            Dim theme = ui.AddMenu(Of String)
            theme.Text = "Theme"
            theme.Expanded = True
            theme.Field = NameOf(s.ThemeName)
            theme.Add(ThemeManager.Themes.Select(Function(x) x.Name))
            theme.Button.ShowPath = True
            theme.Button.SaveAction = Sub(value) ThemeManager.SetCurrentTheme(value)
            theme.Button.ValueChangedAction = Sub(value) ThemeManager.SetCurrentTheme(value)

            Dim uiFallback = ui.AddMenu(Of Boolean)
            uiFallback.Text = "UI Fallback"
            uiFallback.Expanded = True
            uiFallback.Field = NameOf(s.UIFallback)
            uiFallback.Add(New String() {"False", "True"})
            uiFallback.Button.ShowPath = True
            uiFallback.Button.SaveAction = Sub(value) s.UIFallback = value
            uiFallback.Button.ValueChangedAction = Sub(value) s.UIFallback = value

            Dim codeFont = ui.AddTextButton()
            codeFont.Text = "Code Font"
            codeFont.Expanded = True
            codeFont.Edit.Text = s.Fonts(FontCategory.Code)
            codeFont.ClickAction = Sub()
                                       Using td As New TaskDialog(Of FontFamily)
                                           td.Title = "Choose a monospaced font"
                                           td.Symbol = Symbol.Font

                                           For Each ff In FontManager.GetFontFamilies(FontCategory.Code, True)
                                               td.AddCommand(ff.Name, ff)
                                           Next

                                           If td.Show IsNot Nothing Then
                                               codeFont.Edit.Text = td.SelectedText
                                           End If
                                       End Using
                                   End Sub
            codeFont.Edit.SaveAction = Sub(value As String)
                                           s.Fonts(FontCategory.Code) = value
                                           FontManager.Reset()
                                       End Sub

            Dim defaultFontfamilies = FontManager.GetFontFamilies(FontCategory.Default, True)
            Dim defaultFont = ui.AddTextButton()
            defaultFont.Text = "Default Font"
            defaultFont.Expanded = True
            defaultFont.Edit.Text = s.Fonts(FontCategory.Default)
            defaultFont.ClickAction = Sub()
                                          Using td As New TaskDialog(Of FontFamily)
                                              td.Title = "Choose a default font"
                                              td.Symbol = Symbol.Font

                                              For Each ff In defaultFontfamilies
                                                  td.AddCommand(ff.Name, ff)
                                              Next

                                              If td.Show IsNot Nothing Then
                                                  defaultFont.Edit.Text = td.SelectedText
                                                  Dim family = defaultFontfamilies.FirstOrDefault(Function(x) x.Name = td.SelectedText)
                                                  If family IsNot Nothing Then
                                                      For Each control As Control In form.GetAllControls()
                                                          control.ReplaceFontFamily(family)
                                                      Next
                                                  End If
                                              End If
                                          End Using
                                      End Sub
            defaultFont.Edit.TextChangedAction = Sub(value As String)
                                                     Dim family = defaultFontfamilies.FirstOrDefault(Function(x) x.Name = value)
                                                     If family IsNot Nothing Then
                                                         For Each control As Control In form.GetAllControls()
                                                             control.ReplaceFontFamily(family)
                                                         Next
                                                     End If
                                                 End Sub
            defaultFont.Edit.SaveAction = Sub(value As String)
                                              s.Fonts(FontCategory.Default) = value
                                              FontManager.Reset()
                                          End Sub

            n = ui.AddNum()
            n.Text = "Scale Factor"
            n.Help = "Requires to restart StaxRip."
            n.Config = {0.3, 3, 0.05, 2}
            n.Field = NameOf(s.UIScaleFactor)

            Dim l = ui.AddLabel("Icon File:")
            l.Help = "The Windows Startmenu uses Windows Links which allow to use custom icon files."

            Dim tb = ui.AddTextButton
            tb.Label.Visible = False
            tb.BrowseFile("ico|*.ico", Folder.Icons)
            tb.Edit.Expand = True
            tb.Edit.Text = s.IconFile
            tb.Edit.SaveAction = Sub(value) s.IconFile = value

            l = ui.AddLabel("Remember Window Positions:")
            l.Help = "Title or beginning of the title of windows of which the location should be remembered. For all windows enter '''all'''."

            t = ui.AddText()
            t.Help = "Title or beginning of the title of windows of which the location should be remembered. For all windows enter '''all'''."
            t.Label.Visible = False
            t.Edit.Expand = True
            t.Edit.Text = s.WindowPositionsRemembered.Join(", ")
            t.Edit.SaveAction = Sub(value) s.WindowPositionsRemembered = value.SplitNoEmptyAndWhiteSpace(",")

            n = ui.AddNum()
            n.Text = "Preview size compared to screen size (percent)"
            n.Label.Offset = 1
            n.Config = {10, 90, 5}
            n.Field = NameOf(s.PreviewSize)

            b = ui.AddBool()
            b.Text = "Expand Preview window automatically depending on its size"
            b.Help = ""
            b.Field = NameOf(s.ExpandPreviewWindow)

            b = ui.AddBool()
            b.Text = "Use binary prefix (MiB) instead of decimal prefix (MB) for sizes"
            b.Help = "Binary: 1 MiB = 1024 KiB" + BR + "Decimal: 1 MB = 1000 KB" + BR2 +
                            "When selected, Staxrip will use binary prefix instead of decimal in the display and calculation of sizes." + BR +
                            "This will not affect external tools behavior nor their displayed information."
            b.Checked = s.BinaryPrefix
            b.SaveAction = Sub(value)
                               s.BinaryPrefix = value
                               UpdateTargetSizeLabel()
                               UpdateSizeOrBitrate()
                           End Sub

            b = ui.AddBool()
            b.Text = "Invert CTRL key behavior on 'Add Job' button"
            b.Help = ""
            b.Field = NameOf(s.InvertCtrlKeyOnNextButton)

            b = ui.AddBool()
            b.Text = "Invert SHIFT key behavior on 'Add Job' button"
            b.Help = ""
            b.Field = NameOf(s.InvertShiftKeyOnNextButton)

            b = ui.AddBool()
            b.Text = "Enable tooltips in menus (restart required)"
            b.Help = "Tooltips can always be shown by right-clicking menu items."
            b.Field = NameOf(s.EnableTooltips)


            '################# Frameserver
            ui.CreateFlowPage("Frameserver", True)

            Dim avsMode = ui.AddMenu(Of FrameServerMode)()
            avsMode.Text = "AviSynth Mode"
            avsMode.Field = NameOf(s.AviSynthMode)

            Dim vsMode = ui.AddMenu(Of FrameServerMode)()
            vsMode.Text = "VapourSynth Mode"
            vsMode.Field = NameOf(s.VapourSynthMode)

            b = ui.AddBool()
            b.Text = "Load AviSynth plugins"
            b.Help = "Detects and adds necessary LoadPlugin calls."
            b.Field = NameOf(s.LoadAviSynthPlugins)

            b = ui.AddBool()
            b.Text = "Load VapourSynth plugins"
            b.Help = "Detects and adds necessary LoadPlugin calls."
            b.Field = NameOf(s.LoadVapourSynthPlugins)


            '############# Preprocessing
            ui.AddControlPage(New PreprocessingControl, "Preprocessing")


            '############# Source Filters
            Dim bsAVS = AddFilterPreferences(ui, "Source Filters | AviSynth", s.AviSynthFilterPreferences, s.AviSynthProfiles)
            Dim bsVS = AddFilterPreferences(ui, "Source Filters | VapourSynth", s.VapourSynthFilterPreferences, s.VapourSynthProfiles)


            '############### Danger Zone
            Dim dangerZonePage = ui.CreateFlowPage("Danger Zone", True)

            l = ui.AddLabel("")

            l = ui.AddLabel("Don't change Danger Zone settings unless you are" + BR +
                                "a power user with debugging experience." + BR)

            l.BackColor = ThemeManager.CurrentTheme.General.DangerBackColor
            l.ForeColor = ThemeManager.CurrentTheme.General.DangerForeColor

            l = ui.AddLabel("")

            b = ui.AddBool
            b.Text = "Allow using tools with unknown version"
            b.Field = NameOf(s.AllowToolsWithWrongVersion)

            b = ui.AddBool
            b.Text = "Allow using custom paths in startup folder"
            b.Field = NameOf(s.AllowCustomPathsInStartupFolder)

            b = ui.AddBool
            b.Text = "Verify tool status"
            b.Field = NameOf(s.VerifyToolStatus)

            ui.SelectLast("last settings page")

            If form.ShowDialog() = DialogResult.OK Then
                s.AviSynthFilterPreferences = DirectCast(bsAVS.DataSource, StringPairList)
                s.AviSynthFilterPreferences.Sort()
                s.VapourSynthFilterPreferences = DirectCast(bsVS.DataSource, StringPairList)
                s.VapourSynthFilterPreferences.Sort()
                ui.Save()
                g.SetRenderer(MenuStrip)
                s.UpdateRecentProjects(Nothing)
                UpdateRecentProjectsMenu()
                UpdateNextButton()

                If Icon IsNot g.Icon Then
                    Icon = g.Icon
                End If

                Dim newDefaultFont = FontManager.GetDefaultFont()
                If Not oldDefaultFont.FontFamily.Equals(newDefaultFont.FontFamily) Then
                    For Each control As Control In GetAllControls()
                        control.ReplaceFontFamily(newDefaultFont.FontFamily)
                    Next
                    SetAudioTracks()
                End If

                FrameServerHelp.AviSynthToolPath()
                g.SaveSettings()
            Else
                If Not ThemeManager.CurrentTheme.Name.Equals(s.ThemeName, StringComparison.OrdinalIgnoreCase) Then
                    ThemeManager.SetCurrentTheme(s.ThemeName)
                End If
            End If

            If restartID <> GetRestartID() Then
                MsgInfo("Please restart StaxRip.")
            End If

            ui.SaveLast("last settings page")
        End Using
    End Sub

    Function AddFilterPreferences(
        ui As SimpleUI,
        pagePath As String,
        preferences As StringPairList,
        profiles As List(Of FilterCategory)) As BindingSource

        Dim filterPage = ui.CreateDataPage(pagePath)

        Dim fn = Function() As StringPairList
                     Dim ret As New StringPairList From {{" Filters Menu", "StaxRip allows to assign a source filter profile to a particular source file type or format. The source filter profiles can be customized by right-clicking the filters menu in the main dialog."}}

                     For Each i In profiles.Where(Function(v) v.Name = "Source").First.Filters
                         If i.Script <> "" Then
                             ret.Add(i.Name, i.Script)
                         End If
                     Next

                     Return ret
                 End Function

        filterPage.TipProvider.TipsFunc = fn

        Dim c1 = filterPage.AddTextBoxColumn()
        c1.DataPropertyName = "Name"
        c1.HeaderText = "Extension[:Format]"

        Dim c2 = filterPage.AddComboBoxColumn
        c2.DataPropertyName = "Value"
        c2.HeaderText = "Source Filter"

        Dim filterNames = profiles.Where(
            Function(v) v.Name = "Source").First.Filters.Where(
            Function(v) v.Name <> "Automatic" AndAlso
            v.Name <> "Manual" AndAlso Not v.Name.EndsWith("...")).Select(
            Function(v) v.Name).Sort.ToArray

        c2.Items.AddRange(filterNames)

        filterPage.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        Dim ret2 As New BindingSource With {
            .DataSource = ObjectHelp.GetCopy(
            New StringPairList(preferences.Where(
                               Function(a) filterNames.Contains(a.Value) AndAlso a.Name <> "")))
        }

        filterPage.DataSource = ret2
        Return ret2
    End Function

    <Command("Saves the current project.")>
    Sub SaveProject()
        If g.ProjectPath Is Nothing Then
            OpenSaveProjectDialog()
        Else
            SaveProjectPath(g.ProjectPath)
        End If
    End Sub

    <Command("Saves the current project at the specified path.")>
    Sub SaveProjectPath(<Description("The path may contain macros."),
        Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))> path As String)

        path = Macro.Expand(path)

        If path.StartsWith(Folder.Template) Then
            If p.SourceFile <> "" Then
                MsgWarn("A template cannot be created after a source file was opened.")
                Exit Sub
            End If
            p.Log.Clear()
        Else
            g.ProjectPath = path
        End If

        Try
            SafeSerialization.Serialize(p, path)
            SetSavedProject()
            Text = $"{path.Base} - {g.DefaultCommands.GetApplicationDetails()}"
            s.UpdateRecentProjects(path)
            UpdateRecentProjectsMenu()
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    <Command("Saves the current project.")>
    Sub SaveProjectAs()
        OpenSaveProjectDialog()
    End Sub

    <Command("Saves the current project as template.")>
    Sub SaveProjectAsTemplate()
        If p.SourceFile = "" Then
            Dim box As New InputBox With {
                .Text = "Enter the name of the template.",
                .Title = "Save Template",
                .Value = p.TemplateName,
                .CheckBoxText = "Load template on startup"
            }

            If box.Show = DialogResult.OK Then
                p.TemplateName = box.Value.RemoveChars(Path.GetInvalidFileNameChars)
                p.TemplatePath = Path.Combine(Folder.Template, p.TemplateName + ".srip")
                SaveProjectPath(p.TemplatePath)
                UpdateTemplatesMenuAsync()

                If box.Checked Then
                    s.StartupTemplate = box.Value
                    g.SaveSettings()
                End If
            End If
        Else
            MsgWarn("A template cannot be created after a source file was opened.")
        End If
    End Sub

    <Command("Closes the current project.")>
    Sub CloseProject()
        OpenProject(g.StartupTemplatePath)
    End Sub

    <Command("Starts the compressibility check.")>
    Sub StartCompCheck()
        p.VideoEncoder.RunCompCheck()
    End Sub

    <Command("Launches a new instance of StaxRip")>
    Sub StartNewInstance()
        Using p As New Process()
            p.StartInfo.FileName = Application.ExecutablePath
            p.Start()
        End Using
    End Sub

    <Command("Exits StaxRip")>
    Sub [Exit]()
        Close()
    End Sub

    <Command("Exits StaxRip without saving an unsaved project.")>
    Sub ExitWithoutSaving()
        s.ApplicationExitMode = ApplicationExitMode.BypassProjectSaving
        Close()
    End Sub

    <Command("Dialog to manage external tools.")>
    Sub ShowAppsDialog()
        Using form As New AppsForm
            Dim found As Boolean

            If s.StringDictionary.ContainsKey("RecentExternalApplicationControl") Then
                For Each pack In Package.Items.Values
                    If pack.Name + pack.Version = s.StringDictionary("RecentExternalApplicationControl") Then
                        form.ShowPackage(pack)
                        found = True
                        Exit For
                    End If
                Next
            End If

            If Not found Then
                form.ShowPackage(Package.x265)
            End If

            form.ShowDialog()
            g.SaveSettings()
        End Using
    End Sub

    <Command("Shows a dialog to manage video encoder profiles.")>
    Sub ShowEncoderProfilesDialog()
        Using form As New ProfilesForm(
            "Encoder Profiles", s.VideoEncoderProfiles,
            AddressOf g.LoadVideoEncoder,
            AddressOf GetNewVideoEncoderProfile,
            AddressOf VideoEncoder.GetDefaults)

            If form.ShowDialog() = DialogResult.OK Then
                PopulateProfileMenu(DynamicMenuItemID.EncoderProfiles)
            End If
        End Using
    End Sub

    <Command("Dialog to manage Muxer profiles.")>
    Sub ShowMuxerProfilesDialog()
        If p.VideoEncoder.OpenMuxerProfilesDialog() = DialogResult.OK Then
            PopulateProfileMenu(DynamicMenuItemID.MuxerProfiles)
        End If

        Assistant()
    End Sub

    Sub RecalcBitrate()
        UpdateSizeOrBitrate()
    End Sub

    Function GetNewVideoEncoderProfile() As Profile
        Dim sb As New SelectionBox(Of VideoEncoder) With {
            .Title = "Add New Profile",
            .Text = "Please select a profile from the defaults."
        }
        sb.AddItem("Current Project", p.VideoEncoder)

        For Each i In VideoEncoder.GetDefaults()
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK Then
            Return sb.SelectedValue
        End If
    End Function

    Sub PopulateProfileMenu(id As DynamicMenuItemID)
        For Each i In CustomMainMenu.MenuItems
            If i.CustomMenuItem.MethodName = "DynamicMenuItem" AndAlso i.CustomMenuItem.Parameters(0).Equals(id) Then
                i.DropDownItems.ClearAndDisplose

                Select Case id
                    Case DynamicMenuItemID.EncoderProfiles
                        g.PopulateProfileMenu(i.DropDownItems, s.VideoEncoderProfiles, AddressOf ShowEncoderProfilesDialog, AddressOf g.LoadVideoEncoder)
                    Case DynamicMenuItemID.MuxerProfiles
                        g.PopulateProfileMenu(i.DropDownItems, s.MuxerProfiles, AddressOf ShowMuxerProfilesDialog, AddressOf p.VideoEncoder.LoadMuxer)
                    Case DynamicMenuItemID.AudioProfiles
                        For j = 0 To p.AudioTracks.Count - 1
                            Dim index = j
                            g.PopulateProfileMenu(i.DropDownItems, s.AudioProfiles, Sub() ShowAudioProfilesDialog(index), Sub(profile) g.LoadAudioProfile(profile, index))
                        Next
                    Case DynamicMenuItemID.FilterSetupProfiles
                        g.PopulateProfileMenu(i.DropDownItems, s.FilterSetupProfiles, AddressOf ShowFilterSetupProfilesDialog, AddressOf LoadFilterSetup)
                End Select

                Exit For
            End If
        Next
    End Sub

    <Command("Shows the crop dialog to crop borders.")>
    Sub ShowCropDialog()
        If p.SourceFile = "" Then
            ShowOpenSourceDialog()
        Else
            If Not g.VerifyRequirements OrElse Not g.IsValidSource Then
                Exit Sub
            End If

            Using form As New CropForm
                form.ShowDialog()
            End Using

            SetCropFilter()
            DisableCropFilter()
            Assistant()
        End If
    End Sub

    '<Command("Applies the current filter state.")>
    Sub ApplyFilters(Optional filters As List(Of VideoFilter) = Nothing)
        If PreviewScript Is Nothing Then Exit Sub

        PreviewScript.Filters = If(filters, p.Script.Filters)
    End Sub

    <Command("Dialog to preview or cut the video.")>
    Sub ShowPreviewDialog()
        ShowPreview(True)
    End Sub

    <Command("Window to preview or cut the video.")>
    Sub ShowPreview()
        ShowPreview(False)
    End Sub

    Sub ShowPreview(modal As Boolean)
        If p.SourceFile = "" Then
            ShowOpenSourceDialog()
        Else
            If Not g.VerifyRequirements OrElse Not g.IsValidSource Then Exit Sub

            If PreviewScript Is Nothing Then
                PreviewScript = p.Script.GetNewScript()
                If PreviewScript Is Nothing Then
                    Exit Sub
                End If
                PreviewScript.Path = Path.Combine(p.TempDir, p.TargetFile.Base + "_view." + PreviewScript.FileType)
                PreviewScript.RemoveFilter("Cutting")
            Else
                ApplyFilters()
            End If

            PreviewScript.RemoveFilter("Cutting")

            Dim err = PreviewScript.GetError()
            If err <> "" Then
                MsgError("Script Error", err)
                Exit Sub
            End If

            If modal Then
                Using form As New PreviewForm(PreviewScript)
                    form.ShowDialog()
                End Using
            Else
                Dim form As New PreviewForm(PreviewScript)
                form.Show()
            End If
        End If
    End Sub

    <Command("Dialog to configure the main menu.")>
    Sub ShowMainMenuEditor()
        s.CustomMenuMainForm = CustomMainMenu.Edit()
        UpdateTemplatesMenuAsync()
        UpdateScriptsMenuAsync()
        UpdateRecentProjectsMenu()
        UpdateDynamicMenuAsync()
        g.SetRenderer(MenuStrip)
        Refresh()
        g.SaveSettings()
    End Sub

    <Command("Dialog to manage batch jobs.")>
    Sub ShowJobsDialog()
        JobsForm.ShowForm()
    End Sub

    <Command("Clears the job list.")>
    Sub ClearJobs()
        FileHelp.Delete(Path.Combine(Folder.Settings, "Jobs.dat"))
    End Sub

    <Command("Loads an audio or video profile.")>
    Sub LoadProfile(<DispName("Video")> videoProfile As String,
                    <DispName("Audio 1")> audioProfile1 As String,
                    <DispName("Audio 2")> audioProfile2 As String)

        If videoProfile <> "" Then
            For Each i In s.VideoEncoderProfiles
                If i.Name = videoProfile Then
                    g.LoadVideoEncoder(i)
                End If
            Next
        End If

        If audioProfile1 <> "" Then
            For Each i In s.AudioProfiles
                If i.Name = audioProfile1 Then
                    g.LoadAudioProfile(i, 0)
                End If
            Next
        End If

        If audioProfile2 <> "" Then
            For Each i In s.AudioProfiles
                If i.Name = audioProfile2 Then
                    g.LoadAudioProfile(i, 1)
                End If
            Next
        End If
    End Sub

    Sub AddJob(Optional showJobsDialog As Boolean = True, Optional position As Integer = -1)
        If Not CanIgnoreTip Then
            MsgWarn("Assistant warning cannot be skipped.")
            Exit Sub
        End If

        If Not p.SkippedAssistantTips.Contains(CurrentAssistantTipKey) Then
            p.SkippedAssistantTips.Add(CurrentAssistantTipKey)
        End If

        If Not g.VerifyRequirements() Then
            Exit Sub
        End If

        For Each form In PreviewForm.Instances.ToArray
            form.Close()
        Next

        If AssistantPassed Then
            If AbortDueToLowDiskSpace() Then Exit Sub

            g.RaiseAppEvent(ApplicationEvent.BeforeJobAdding)

            Dim jobPath = JobManager.JobPath
            Dim jobs = JobManager.GetJobs()?.Where(Function(x) x.Active AndAlso x.Path = jobPath)

            If jobs.Any() Then
                If MsgQuestion($"An active job for this project already exists.{BR}If you continue, it will be overwritten.", TaskButton.OkCancel) = DialogResult.Cancel Then
                    Exit Sub
                End If
            End If

            If TypeOf p.VideoEncoder IsNot NullEncoder Then
                Dim op = If(p.VideoEncoder.CanChunkEncode(), p.VideoEncoder.OutputPath.DirAndBase() + "_chunk1" + p.VideoEncoder.OutputPath.ExtFull(), p.VideoEncoder.OutputPath)

                If op.FileExists() Then
                    Select Case p.FileExistVideo
                        Case FileExistMode.Ask
                            Using td As New TaskDialog(Of String)
                                td.Title = "A video encoding output file already exists"
                                td.Content = "Would you like to skip video encoding and reuse the existing video encoder output file or would you like to re-encode and overwrite it?"
                                td.AddCommand("Reuse")
                                td.AddCommand("Re-encode")

                                Select Case td.Show
                                    Case "Reuse"
                                        p.SkipVideoEncoding = True
                                    Case "Re-encode"
                                        p.SkipVideoEncoding = False
                                    Case Else
                                        Exit Sub
                                End Select
                            End Using
                        Case FileExistMode.Overwrite
                            p.SkipVideoEncoding = False
                        Case FileExistMode.Skip
                            p.SkipVideoEncoding = True
                    End Select
                End If
            End If

            If p.AudioTracks.Where(Function(track) track.AudioProfile.File <> "" AndAlso
                                       (TypeOf track.AudioProfile Is GUIAudioProfile OrElse TypeOf track.AudioProfile Is BatchAudioProfile) AndAlso
                                       File.Exists(track.AudioProfile.GetOutputFile()))?.Any() Then
                Select Case p.FileExistAudio
                    Case FileExistMode.Ask
                        Using td As New TaskDialog(Of String)
                            td.Title = "An audio encoding output file already exists"
                            td.Content = "Would you like to skip audio encoding and reuse existing audio encoding output files or would you like to re-encode and overwrite?"
                            td.AddCommand("Reuse")
                            td.AddCommand("Re-encode")

                            Select Case td.Show
                                Case "Reuse"
                                    p.SkipAudioEncoding = True
                                Case "Re-encode"
                                    p.SkipAudioEncoding = False
                                Case Else
                                    Exit Sub
                            End Select
                        End Using
                    Case FileExistMode.Overwrite
                        p.SkipAudioEncoding = False
                    Case FileExistMode.Skip
                        p.SkipAudioEncoding = True
                End Select
            End If

            AddJob(False, Nothing, position)

            If showJobsDialog Then
                Me.ShowJobsDialog()
            Else
                bnNext.ShowBold()
            End If
        Else
            Assistant()
        End If
    End Sub

    <Command("Adds a job to the job list.")>
    Sub AddJob(
        <DispName("Show Confirmation")>
        showConfirmation As Boolean,
        <DispName("Template Name"),
        Description("Name of the template to be loaded after the job was added. Empty to load no template.")>
        templateName As String,
        <DispName("Position to insert new job")>
        Optional position As Integer = -1)

        AddJob(showConfirmation, templateName, True, position)
    End Sub

    Sub AddJob(
        showConfirmation As Boolean,
        templateName As String,
        showAssistant As Boolean,
        Optional position As Integer = -1)

        If Not g.VerifyRequirements() Then
            Exit Sub
        End If

        If showAssistant AndAlso Not IsLoading AndAlso Not AssistantPassed Then
            MsgWarn("Please follow the assistant.")
            Exit Sub
        End If

        Dim jobPath = JobManager.JobPath
        SaveProjectPath(jobPath)
        JobManager.AddJob(jobPath, jobPath, position)

        If showConfirmation Then
            MsgInfo("Job added")
        End If

        If templateName <> "" Then
            LoadProject(Path.Combine(Folder.Template, templateName + ".srip"))
        End If
    End Sub

    <Command("Shows a dialog to compare different videos.")>
    Sub ShowVideoComparison()
        Dim form As New VideoComparisonForm
        form.Show()
    End Sub

    <Command("Dialog to edit filters.")>
    Sub ShowFiltersEditor()
        FiltersListView.ShowCodeEditor()
    End Sub

    <Command("Dialog to preview script code.")>
    Sub ShowCodePreview()
        g.ShowCodePreview(p.Script.GetFullScript)
    End Sub

    <Command("Dialog to configure project options.")>
    Sub ShowOptionsDialog()
        ShowOptionsDialog(Nothing)
    End Sub

    Sub ShowOptionsDialog(pagePath As String)
        Using form As New SimpleSettingsForm(
            "Project Options",
            "In order to save project options go to:",
            "File > Save Project As Template",
            "In order to select a template to be loaded on program startup go to:",
            "Tools > Settings > General > Templates > Default Template")

            form.ScaleClientSize(38, 30)

            Dim ui = form.SimpleUI
            ui.Store = p


            '   ----------------------------------------------------------------
            ui.CreateFlowPage("Image", True)

            Dim n = ui.AddNum()
            n.Text = "Auto resize image size"
            n.Help = "Resizes to a given pixel size after loading a source file."
            n.Config = {0, Integer.MaxValue, 10000}
            n.Field = NameOf(p.AutoResizeImage)

            ui.AddLabel(n, "(0 = disabled)")

            Dim m = ui.AddMenu(Of Integer)()
            m.Text = "Output Mod"
            m.Add(2, 4, 8, 16)
            m.Field = NameOf(p.ForcedOutputMod)

            Dim dirMenu = ui.AddMenu(Of ForceOutputModDirection)()
            dirMenu.Text = "Output Mod Direction"
            dirMenu.Field = NameOf(p.ForcedOutputModDirection)

            Dim b = ui.AddBool()
            b.Text = "Make Output Mod warning ignorable"
            b.Field = NameOf(p.ForcedOutputModIgnorable)

            b = ui.AddBool()
            b.Text = "Warn on invalid Output Mod only if video is cropped"
            b.Field = NameOf(p.ForcedOutputModOnlyIfCropped)


            '   ----------------------------------------------------------------
            ui.CreateFlowPage("Image | Aspect Ratio", True)

            b = ui.AddBool()
            b.Text = "Use ITU-R BT.601 compliant aspect ratio"
            b.Help = "Calculates the aspect ratio according to ITU-R BT.601 standard."
            b.Field = NameOf(p.ITU)

            b = ui.AddBool()
            b.Text = "Adjust height according to target display aspect ratio"
            b.Help = "Adjusts the height to match the target display aspect ratio in case the auto resize option is disabled."
            b.Field = NameOf(p.AdjustHeight)

            n = ui.AddNum()
            n.Text = "Max AR Error"
            n.Help = "Maximum aspect ratio error. In case of a higher value the AR signaled to the encoder or muxer."
            n.Config = {1, 10, 0.1, 1}
            n.Field = NameOf(p.MaxAspectRatioError)

            Dim t = ui.AddText()
            t.Text = "Source DAR"
            t.Help = "Custom source display aspect ratio."
            t.Field = NameOf(p.CustomSourceDAR)

            t = ui.AddText()
            t.Text = "Source PAR"
            t.Help = "Custom source pixel aspect ratio."
            t.Field = NameOf(p.CustomSourcePAR)

            t = ui.AddText()
            t.Text = "Target DAR"
            t.Help = "Custom target display aspect ratio."
            t.Field = NameOf(p.CustomTargetDAR)

            t = ui.AddText()
            t.Text = "Target PAR"
            t.Help = "Custom target pixel aspect ratio."
            t.Field = NameOf(p.CustomTargetPAR)


            '   ----------------------------------------------------------------
            Dim cropPage = ui.CreateFlowPage("Image | Crop", True)

            b = ui.AddBool()
            b.Text = "Auto correct crop values"
            b.Help = "Force crop values compatible with YUV/YV12 colorspace and with the forced output mod value. "
            b.Field = NameOf(p.AutoCorrectCropValues)

            b = ui.AddBool()
            b.Text = "Auto crop borders until proper aspect ratio is found"
            b.Help = "Automatically crops borders until the proper aspect ratio is found."
            b.Field = NameOf(p.AutoSmartCrop)

            n = ui.AddNum()
            n.Text = "Auto overcrop width to limit aspect ratio to"
            n.Help = "On small devices it can help to restrict the aspect ratio and overcrop the width instead."
            n.Config = {0, 2, 0.1, 3}
            n.Field = NameOf(p.AutoSmartOvercrop)

            b = ui.AddBool()
            b.Text = "Tonemapping for HDR videos"
            b.Help = "Tonemap sources with a higher Bit Depth than 8bit."
            b.Enabled = Vulkan.IsSupported
            b.Field = NameOf(p.CropWithTonemapping)
            b.Checked = b.Checked AndAlso Vulkan.IsSupported

            b = ui.AddBool()
            b.Text = "High contrast for easier cropping"
            b.Help = ""
            b.Field = NameOf(p.CropWithHighContrast)

            ui.AddLine(cropPage, "Custom crop values")

            Dim eb = ui.AddEmptyBlock(cropPage)
            ui.AddLabel(eb, "Left:", 2)
            Dim leftCrop = ui.AddNumeric(eb)
            ui.AddLabel(eb, "Right:", 4)
            Dim rightCrop = ui.AddNumeric(eb)

            eb = ui.AddEmptyBlock(cropPage)
            ui.AddLabel(eb, "Top:", 2)
            Dim topCrop = ui.AddNumeric(eb)
            ui.AddLabel(eb, "Bottom:", 4)
            Dim bottomCrop = ui.AddNumeric(eb)

            leftCrop.Value = p.CropLeft
            leftCrop.Config = {0, 9999, 2, 0}

            rightCrop.Value = p.CropRight
            rightCrop.Config = leftCrop.Config

            topCrop.Value = p.CropTop
            topCrop.Config = {0, 9999, 2, 0}

            bottomCrop.Value = p.CropBottom
            bottomCrop.Config = topCrop.Config



            '   ----------------------------------------------------------------
            Dim autoCropPage = ui.CreateFlowPage("Image | Crop | Auto Crop", True)

            Dim autoCropMode = ui.AddMenu(Of AutoCropMode)

            ui.AddLine(autoCropPage, "General")
            'dim l = ui.AddLabel("Regular AutoCrop settings:", 0, FontStyle.Bold)
            'l.Margin = New Padding(0, 10, 0, 0)
            Dim autoCropFrameRangeMode = ui.AddMenu(Of AutoCropFrameRangeMode)

            Dim thresholdEb = ui.AddEmptyBlock(autoCropPage)
            thresholdEb.Margin = New Padding(0, 6, 0, 3)
            Dim l = ui.AddLabel(thresholdEb, "Threshold at:", 7)
            ui.AddLabel(thresholdEb, "Beginning:", 2)
            Dim thresholdBegin = ui.AddNumeric(thresholdEb)
            ui.AddLabel(thresholdEb, " ", 1)
            ui.AddLabel(thresholdEb, "Ending:", 2)
            Dim thresholdEnd = ui.AddNumeric(thresholdEb)

            thresholdBegin.Help = "Number of frames at the beginning of the video, that are ignored when setting the crop values."
            thresholdBegin.Config = {0, 999999, 25, 0}
            thresholdBegin.Field = NameOf(p.AutoCropFrameRangeThresholdBegin)

            thresholdEnd.Help = "Number of frames at the ending of the video, that are ignored when setting the crop values."
            thresholdEnd.Config = thresholdBegin.Config
            thresholdEnd.Field = NameOf(p.AutoCropFrameRangeThresholdEnd)

            autoCropFrameRangeMode.Text = "Frame Range Mode"
            autoCropFrameRangeMode.Help = "Defines the range frames are considered to be taken into account for processing:" + BR2 +
                                                        "Automatic: Depending on video length a small portion of the beginning and end will be ignored." + BR +
                                                        "Complete: The whole video length will be used." + BR +
                                                        "Manual Threshold: Define your own number of frames at beginning and end that will be ignored."
            autoCropFrameRangeMode.Expanded = True
            autoCropFrameRangeMode.Field = NameOf(p.AutoCropFrameRangeMode)
            autoCropFrameRangeMode.Button.ValueChangedAction = Sub(value)
                                                                   Dim active = autoCropFrameRangeMode.Enabled
                                                                   Dim activeAutomaticRange = active AndAlso value = StaxRip.AutoCropFrameRangeMode.Automatic
                                                                   Dim activeCompleteRange = active AndAlso value = StaxRip.AutoCropFrameRangeMode.Complete
                                                                   Dim activeManualThresholdRange = active AndAlso value = StaxRip.AutoCropFrameRangeMode.ManualThreshold
                                                                   thresholdEb.Visible = activeManualThresholdRange
                                                               End Sub
            autoCropFrameRangeMode.Button.ValueChangedAction.Invoke(p.AutoCropFrameRangeMode)



            Dim autoCropFrameSelectionMode = ui.AddMenu(Of AutoCropFrameSelectionMode)

            Dim fsFixedFrames = ui.AddNum()
            fsFixedFrames.Text = "Number of frames:"
            fsFixedFrames.Help = "Fixed number of frames being analyzed over the whole video."
            fsFixedFrames.Config = {1, 7200, 5, 0}
            fsFixedFrames.Field = NameOf(p.AutoCropFixedFramesFrameSelection)
            fsFixedFrames.Margin = New Padding(0, 6, 0, 3)

            Dim fsFrameInterval = ui.AddNum()
            fsFrameInterval.Text = "Frame interval:"
            fsFrameInterval.Help = "Frame interval betwen analyzed frames."
            fsFrameInterval.Config = {1, 3600, 5, 0}
            fsFrameInterval.Field = NameOf(p.AutoCropFrameIntervalFrameSelection)
            fsFrameInterval.Margin = New Padding(0, 6, 0, 3)

            Dim fsTimeInterval = ui.AddNum()
            fsTimeInterval.Text = "Time interval in seconds:"
            fsTimeInterval.Help = "Time interval in seconds betwen analyzed frames."
            fsTimeInterval.Config = {1, 3600, 5, 0}
            fsTimeInterval.Field = NameOf(p.AutoCropTimeIntervalFrameSelection)
            fsTimeInterval.Margin = New Padding(0, 6, 0, 3)


            Dim luminanceThreshold = ui.AddNum()
            luminanceThreshold.Text = "Luminance Threshold in percent:"
            luminanceThreshold.Help = "Max brightness in peercent of those lines, that are considered to be cropped."
            luminanceThreshold.Config = {0, 99, 0.1, 1}
            luminanceThreshold.Field = NameOf(p.AutoCropLuminanceThreshold)
            luminanceThreshold.Margin = New Padding(0, 6, 0, 3)



            ui.AddLine(autoCropPage, "Dolby Vision")

            Dim autoCropDVMode = ui.AddMenu(Of AutoCropDolbyVisionMode)()

            Dim doviThresholdEb = ui.AddEmptyBlock(autoCropPage)
            doviThresholdEb.Margin = New Padding(0, 6, 0, 3)
            l = ui.AddLabel(doviThresholdEb, "Threshold at:", 7)
            ui.AddLabel(doviThresholdEb, "Beginning:", 2)
            Dim doviThresholdBegin = ui.AddNumeric(doviThresholdEb)
            ui.AddLabel(doviThresholdEb, " ", 1)
            ui.AddLabel(doviThresholdEb, "Ending:", 2)
            Dim doviThresholdEnd = ui.AddNumeric(doviThresholdEb)


            autoCropDVMode.Text = "Mode"
            autoCropDVMode.Help = "Decide between an automatic mode and a manual threshold to ignore a number of frames at the beginning and/or end."
            autoCropDVMode.Expanded = True
            autoCropDVMode.Field = NameOf(p.AutoCropDolbyVisionMode)
            autoCropDVMode.Button.ValueChangedAction = Sub(value)
                                                           Dim active = value = AutoCropDolbyVisionMode.ManualThreshold
                                                           doviThresholdEb.Visible = active
                                                       End Sub
            autoCropDVMode.Button.ValueChangedAction.Invoke(p.AutoCropDolbyVisionMode)

            autoCropFrameSelectionMode.Text = "Frame Selection Mode"
            autoCropFrameSelectionMode.Help = ""
            autoCropFrameSelectionMode.Expanded = True
            autoCropFrameSelectionMode.Field = NameOf(p.AutoCropFrameSelectionMode)
            autoCropFrameSelectionMode.Button.ValueChangedAction = Sub(value)
                                                                       Dim active = autoCropFrameSelectionMode.Enabled
                                                                       Dim activeFixedFrames = active AndAlso value = StaxRip.AutoCropFrameSelectionMode.FixedFrames
                                                                       Dim activeFrameInterval = active AndAlso value = StaxRip.AutoCropFrameSelectionMode.FrameInterval
                                                                       Dim activeTimeInterval = active AndAlso value = StaxRip.AutoCropFrameSelectionMode.TimeInterval
                                                                       fsFixedFrames.Visible = activeFixedFrames
                                                                       fsFrameInterval.Visible = activeFrameInterval
                                                                       fsTimeInterval.Visible = activeTimeInterval
                                                                   End Sub
            autoCropFrameSelectionMode.Button.ValueChangedAction.Invoke(p.AutoCropFrameSelectionMode)

            autoCropMode.Text = "Auto Crop after opening"
            autoCropMode.Help = "Use Auto Crop when a file is opened to crop it directly."
            autoCropMode.Expanded = True
            autoCropMode.Field = NameOf(p.AutoCropMode)
            autoCropMode.Button.ValueChangedAction = Sub(value)
                                                         autoCropDVMode.Button.ValueChangedAction.Invoke(autoCropDVMode.Button.Value)
                                                         autoCropFrameSelectionMode.Button.ValueChangedAction.Invoke(autoCropFrameSelectionMode.Button.Value)
                                                     End Sub
            autoCropMode.Button.ValueChangedAction.Invoke(p.AutoCropMode)

            doviThresholdBegin.Help = "Number of frames at the beginning of the video, that are ignored when setting the crop values."
            doviThresholdBegin.Config = {0, 999999, 25, 0}
            doviThresholdBegin.Field = NameOf(p.AutoCropDolbyVisionThresholdBegin)

            doviThresholdEnd.Help = "Number of frames at the ending of the video, that are ignored when setting the crop values."
            doviThresholdEnd.Config = doviThresholdBegin.Config
            doviThresholdEnd.Field = NameOf(p.AutoCropDolbyVisionThresholdEnd)


            '   ----------------------------------------------------------------
            Dim generalPage = ui.CreateFlowPage("General", True)

            Dim takeOverTitle = ui.AddBool()

            takeOverTitle.Text = "Take over title"
            takeOverTitle.Help = "Take over title from source file - Title Name in container options must be empty!"
            takeOverTitle.Checked = p.TakeOverTitle
            takeOverTitle.SaveAction = Sub(value) p.TakeOverTitle = value


            '   ----------------------------------------------------------------
            Dim videoPage = ui.CreateFlowPage("Video", True)

            Dim videoExist = ui.AddMenu(Of FileExistMode)
            Dim demuxVideo = ui.AddBool()
            Dim fixFramerate = ui.AddBool()
            Dim takeOverVideoLanguage = ui.AddBool()
            Dim extractHdrmetadata = ui.AddMenu(Of HdrmetadataMode)

            videoExist.Text = "Existing Video Output"
            videoExist.Help = "What to do in case the video encoding output file already exists from a previous job run, skip and reuse or re-encode and overwrite. The 'Copy/Mux' video encoder profile is also capable of reusing existing video encoder output.'"
            videoExist.Expanded = True
            videoExist.Field = NameOf(p.FileExistVideo)

            demuxVideo.Text = "Demux Video"
            demuxVideo.Field = NameOf(p.DemuxVideo)

            takeOverVideoLanguage.Text = "Take over language"
            takeOverVideoLanguage.Field = NameOf(p.TakeOverVideoLanguage)

            fixFramerate.Text = "Add filter to automatically correct the frame rate."
            fixFramerate.Field = NameOf(p.FixFrameRate)

            extractHdrmetadata.Text = "Extract HDR metadata"
            extractHdrmetadata.Help = "Extract dynamic HDR10+ and DolbyVision metadata if available"
            extractHdrmetadata.Expanded = True
            extractHdrmetadata.Field = NameOf(p.ExtractHdrmetadata)
            extractHdrmetadata.Button.ValueChangedAction = Sub(value)
                                                               Dim visible = value = HdrmetadataMode.All OrElse value = HdrmetadataMode.DolbyVision
                                                           End Sub

            b = ui.AddBool
            b.Text = "Import VUI metadata"
            b.Help = "Imports VUI metadata such as HDR from the source file to the video encoder."
            b.Field = NameOf(p.ImportVUIMetadata)

            b = ui.AddBool
            b.Text = "Add filter to convert chroma subsampling to 4:2:0"
            b.Help = "After a source is loaded, automatically add a filter to convert chroma subsampling to 4:2:0"
            b.Field = NameOf(p.ConvertChromaSubsampling)

            Dim cb = ui.AddMenu(Of ConvertTo420BitDepth)
            cb.Text = "Add filter to convert bit depth to"
            cb.Help = "After a source is loaded, automatically add a filter to convert bit-depth to x-bit"
            cb.Expanded = True
            cb.Field = NameOf(p.ConvertToBits)

            b = ui.AddBool
            b.Text = "Auto-rotate video after loading when possible"
            b.Help = "Auto-rotate video after loading when the source file/container supports it."
            b.Field = NameOf(p.AutoRotation)

            n = ui.AddNum()
            n.Text = "Film Threshold for D2V files:"
            n.Help = ""
            n.Config = {0, 100, 0.1, 2}
            n.Field = NameOf(p.D2VAutoForceFilmThreshold)


            '   ----------------------------------------------------------------
            Dim audioPage = ui.CreateFlowPage("Audio", True)

            Dim prefAudio = ui.AddTextMenu
            prefAudio.Text = "Preferred Languages"
            prefAudio.Help = "List of audio tracks to demux."
            prefAudio.Field = NameOf(p.PreferredAudio)

            For x = 1 To 9
                Dim temp = x
                prefAudio.AddMenu("Choose ID | " & x, Sub() prefAudio.Edit.Text += " " & temp)
            Next

            prefAudio.AddMenu("Choose All", "all")
            prefAudio.AddMenu("-", "")

            For Each lng In Language.Languages
                If lng.IsCommon Then
                    prefAudio.AddMenu(lng.ToString + " (" + lng.TwoLetterCode + ", " + lng.ThreeLetterCode + ")",
                        Sub() prefAudio.Edit.Text += " " + lng.ThreeLetterCode)
                End If
            Next

            Dim moreAudio = prefAudio.AddMenu("More", DirectCast(Nothing, Action))
            Dim moreAudioFirst = prefAudio.AddMenu("More | temp", DirectCast(Nothing, Action))

            Dim moreAudioAction = Sub()
                                      For Each lng In Language.Languages
                                          prefAudio.AddMenu("More | " + lng.ToString.Substring(0, 1).ToUpperInvariant + " | " + lng.ToString +
                                         " (" + lng.TwoLetterCode + ", " + lng.ThreeLetterCode + ")",
                                         Sub() prefAudio.Edit.Text += " " + lng.ThreeLetterCode)
                                      Next
                                  End Sub

            AddHandler moreAudio.DropDownOpened, Sub()
                                                     If moreAudio.DropDownItems.Count > 1 Then
                                                         Exit Sub
                                                     End If

                                                     moreAudioFirst.Visible = False
                                                     moreAudioAction()
                                                 End Sub

            Dim cut = ui.AddMenu(Of CuttingMode)
            cut.Text = "Cutting Method"
            cut.Help = "Defines which method to use for cutting."
            cut.Field = NameOf(p.CuttingMode)

            Dim demuxAudio = ui.AddMenu(Of DemuxMode)
            demuxAudio.Text = "Demux Audio"
            demuxAudio.Field = NameOf(p.DemuxAudio)

            Dim audioExist = ui.AddMenu(Of FileExistMode)
            audioExist.Text = "Existing Output"
            audioExist.Help = "What to do in case an audio encoding output file already exists from a previous job run, skip and reuse or re-encode and overwrite."
            audioExist.Field = NameOf(p.FileExistAudio)

            n = ui.AddNum
            n.Text = "Audio Tracks"
            n.Config = {2, 20, 1}
            n.Field = NameOf(p.AudioTracksAvailable)
            'n.NumEdit.SaveAction = Sub(x) SetAudioTracks(CType(x, Integer))

            b = ui.AddBool
            b.Text = "On load use AviSynth script as audio source"
            b.Help = "Sets the AviSynth script (*.avs) as audio source file when loading a source file."
            b.Field = NameOf(p.UseScriptAsAudioSource)


            '   ----------------------------------------------------------------
            Dim subPage = ui.CreateFlowPage("Subtitles", True)

            Dim subMode = ui.AddMenu(Of SubtitleMode)
            subMode.Expanded = True
            subMode.Text = "Subtitles"
            subMode.Field = NameOf(p.SubtitleMode)

            Dim prefSub = ui.AddTextMenu(subPage)
            prefSub.Text = "Languages"
            prefSub.Help = "List of used subtitle languages."
            prefSub.Field = NameOf(p.PreferredSubtitles)

            For x = 1 To 9
                Dim temp = x
                prefSub.AddMenu("Choose ID | " & x, Sub() prefSub.Edit.Text += " " & temp)
            Next

            prefSub.AddMenu("Choose All", "all")
            prefSub.AddMenu("-", "")

            For Each lng In Language.Languages
                If lng.IsCommon Then
                    prefSub.AddMenu(lng.ToString + " (" + lng.TwoLetterCode + ", " + lng.ThreeLetterCode + ")",
                        Sub() prefSub.Edit.Text += " " + lng.ThreeLetterCode)
                End If
            Next

            Dim moreSub = prefSub.AddMenu("More", DirectCast(Nothing, Action))
            Dim moreSubFirst = prefSub.AddMenu("More | temp", DirectCast(Nothing, Action))

            Dim moreSubAction = Sub()
                                    For Each lng In Language.Languages
                                        prefSub.AddMenu("More | " + lng.ToString.Substring(0, 1).ToUpperInvariant + " | " + lng.ToString +
                                         " (" + lng.TwoLetterCode + ", " + lng.ThreeLetterCode + ")",
                                         Sub() prefSub.Edit.Text += " " + lng.ThreeLetterCode)
                                    Next
                                End Sub

            AddHandler moreSub.DropDownOpened, Sub()
                                                   If moreSub.DropDownItems.Count > 1 Then
                                                       Exit Sub
                                                   End If

                                                   moreSubFirst.Visible = False
                                                   moreSubAction()
                                               End Sub

            Dim tbm = ui.AddTextMenu(subPage)
            tbm.Text = "Track Name"
            tbm.Help = "Track name used for muxing, may contain macros."
            tbm.Field = NameOf(p.SubtitleName)
            tbm.AddMenu("Language English", "%language_english%")
            tbm.AddMenu("Language Native", "%language_native%")

            Dim mb = ui.AddMenu(Of DefaultSubtitleMode)(subPage)
            mb.Text = "Default Subtitle"
            mb.Field = NameOf(p.DefaultSubtitle)

            b = ui.AddBool(subPage)
            b.Text = "Convert Sup (PGS/Blu-ray) to IDX (Sub/VobSub/DVD)"
            b.Help = "Works only with demuxed subtitles."
            b.Field = NameOf(p.ConvertSup2Sub)

            b = ui.AddBool(subPage)
            b.Text = "Extract forced subtitles from IDX files (Sub/VobSub/DVD)"
            b.Help = "Works only with demuxed subtitles."
            b.Field = NameOf(p.ExtractForcedSubSubtitles)

            b = ui.AddBool(subPage)
            b.Text = "Add hardcoded subtitle"
            b.Help = "Automatically hardcodes a subtitle." + BR2 + "Supported formats are SRT, ASS and VobSub."
            b.Field = NameOf(p.HardcodedSubtitle)


            '   ----------------------------------------------------------------
            Dim chaptersPage = ui.CreateFlowPage("Chapters", True)

            b = ui.AddBool(chaptersPage)
            b.Text = "Demux Chapters"
            b.Checked = p.DemuxChapters
            b.SaveAction = Sub(val) p.DemuxChapters = val


            '   ----------------------------------------------------------------
            Dim timestampsPage = ui.CreateFlowPage("Timestamps", True)

            Dim timestamps = ui.AddMenu(Of TimestampsMode)
            timestamps.Expanded = False
            timestamps.Text = "Extract timestamps from MKV files (if existing)"
            timestamps.Field = NameOf(p.ExtractTimestamps)


            '   ----------------------------------------------------------------
            Dim attachmentsPage = ui.CreateFlowPage("Attachments", True)

            b = ui.AddBool(attachmentsPage)
            b.Text = "Demux Attachments"
            b.Checked = p.DemuxAttachments
            b.SaveAction = Sub(val) p.DemuxAttachments = val

            b = ui.AddBool(attachmentsPage)
            b.Text = "Add Attachments to Muxing"
            b.Checked = p.AddAttachmentsToMuxer
            b.SaveAction = Sub(val) p.AddAttachmentsToMuxer = val


            '   ----------------------------------------------------------------
            Dim tagsPage = ui.CreateFlowPage("Tags", True)

            b = ui.AddBool(tagsPage)
            b.Text = "Demux Tags"
            b.Checked = p.DemuxTags
            b.SaveAction = Sub(val) p.DemuxTags = val

            b = ui.AddBool(tagsPage)
            b.Text = "Add Tags to Muxing"
            b.Checked = p.AddTagsToMuxer
            b.SaveAction = Sub(val) p.AddTagsToMuxer = val


            '   ----------------------------------------------------------------
            ui.CreateFlowPage("Thumbnails", True)

            b = ui.AddBool()
            b.Text = "Create a thumbnail sheet automatically after encoding"
            b.Field = NameOf(p.Thumbnailer)

            Dim thumbsImageFormat = ui.AddMenu(Of String)
            Dim thumbsQuality = ui.AddNum()

            thumbsImageFormat.Text = "Image Format"
            thumbsImageFormat.Expanded = True
            thumbsImageFormat.Add("BITMAP", "bmp")
            thumbsImageFormat.Add("GIF", "gif")
            thumbsImageFormat.Add("JPEG", "jpg")
            thumbsImageFormat.Add("PNG", "png")
            thumbsImageFormat.Add("TIFF", "tif")
            thumbsImageFormat.Button.Value = p.ThumbnailerSettings.GetString("ImageFileFormat", "jpg")
            thumbsImageFormat.Button.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("ImageFileFormat", value)
            thumbsImageFormat.Button.ValueChangedAction = Sub(value)
                                                              thumbsQuality.Visible = value = "jpg"
                                                          End Sub

            thumbsQuality.Text = "Quality (%):"
            thumbsQuality.Config = {1, 100, 1}
            thumbsQuality.NumEdit.Value = p.ThumbnailerSettings.GetInt("ImageQuality", 70)
            thumbsQuality.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("ImageQuality", CInt(value))

            Dim thumbsHeaderBackColor As SimpleUI.ColorPickerBlock = Nothing

            Dim thumbsImageBackColor = ui.AddColorPicker()
            thumbsImageBackColor.Text = "Background Color:"
            thumbsImageBackColor.Expanded = True
            thumbsImageBackColor.Color = p.ThumbnailerSettings.GetString("ImageBackColor", New ColorHSL(0, 0, 0.05, 1).ToHTML()).ToColor()
            thumbsImageBackColor.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("ImageBackColor", ColorTranslator.ToHtml(value))
            thumbsImageBackColor.ValueChangedAction = Sub(value) If thumbsHeaderBackColor IsNot Nothing Then thumbsHeaderBackColor.Color = value

            n = ui.AddNum()
            n.Text = "Spacer (%):"
            n.Config = {0, 1000, 1}
            n.NumEdit.Value = p.ThumbnailerSettings.GetInt("SpacerPercent", 20)
            n.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("SpacerPercent", CInt(value))

            ui.AddLabel(n, " between thumbs, header and sides")


            '   ----------------------------------------------------------------
            ui.CreateFlowPage("Thumbnails | Header", True)

            b = ui.AddBool()
            b.Text = "Draw Header"
            b.Checked = p.ThumbnailerSettings.GetBool("Header", True)
            b.SaveAction = Sub(value) p.ThumbnailerSettings.SetBool("Header", value)

            Dim headerFont = ui.AddTextButton()
            headerFont.Text = "Font Name:"
            headerFont.Expanded = True
            headerFont.Edit.Text = p.ThumbnailerSettings.GetString("HeaderFontName", "Consolas")
            headerFont.Edit.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("HeaderFontName", value)
            headerFont.ClickAction = Sub()
                                         Using td As New TaskDialog(Of FontFamily)
                                             td.Title = "Choose a font for the header"
                                             td.Symbol = Symbol.Font

                                             For Each ff In FontFamily.Families.Where(Function(x) Not x.Name.ToLowerEx().ContainsAny(" mdl2", " assets", "marlett", "ms outlook", "mt extra", "wingdings 2") AndAlso x.IsStyleAvailable(FontStyle.Regular) AndAlso x.IsMonospace())
                                                 td.AddCommand(ff.Name, ff)
                                             Next

                                             If td.Show IsNot Nothing Then
                                                 headerFont.Edit.Text = td.SelectedText
                                             End If
                                         End Using
                                     End Sub

            Dim cp = ui.AddColorPicker()
            cp.Text = "Font Color:"
            cp.Expanded = True
            cp.Color = p.ThumbnailerSettings.GetString("HeaderFontColor", New ColorHSL(0, 0, 0.8, 1).ToHTML()).ToColor()
            cp.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("HeaderFontColor", ColorTranslator.ToHtml(value))

            n = ui.AddNum()
            n.Text = "Font Size (%):"
            n.Config = {10, 1000, 1}
            n.NumEdit.Value = p.ThumbnailerSettings.GetInt("HeaderFontSizePercent", 100)
            n.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("HeaderFontSizePercent", CInt(value))

            thumbsHeaderBackColor = ui.AddColorPicker()
            thumbsHeaderBackColor.Text = "Background Color:"
            thumbsHeaderBackColor.Expanded = True
            thumbsHeaderBackColor.Color = p.ThumbnailerSettings.GetString("HeaderBackColor", ColorTranslator.ToHtml(thumbsImageBackColor.Color)).ToColor()
            thumbsHeaderBackColor.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("HeaderBackColor", ColorTranslator.ToHtml(value))

            n = ui.AddNum()
            n.Text = "Separator Size (%):"
            n.Config = {0, 1000, 2}
            n.NumEdit.Value = p.ThumbnailerSettings.GetInt("HeaderSeparatorHeightPercent", 0)
            n.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("HeaderSeparatorHeightPercent", CInt(value))


            '   ----------------------------------------------------------------
            ui.CreateFlowPage("Thumbnails | Thumbs", True)

            Dim thumbstimestampAlign = ui.AddMenu(Of ContentAlignment)
            thumbstimestampAlign.Text = "Timestamp Alignment:"
            thumbstimestampAlign.Expanded = True
            thumbstimestampAlign.Button.Value = DirectCast([Enum].Parse(GetType(ContentAlignment), p.ThumbnailerSettings.GetString("TimestampAlignment", ContentAlignment.BottomLeft.ToString()).ToString()), ContentAlignment)
            thumbstimestampAlign.Button.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("TimestampAlignment", value.ToString())

            Dim timestampFont = ui.AddTextButton()
            timestampFont.Text = "Timestamp Font Name:"
            timestampFont.Expanded = True
            timestampFont.Edit.Text = p.ThumbnailerSettings.GetString("TimestampFontName", "Consolas")
            timestampFont.Edit.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("TimestampFontName", value)
            timestampFont.ClickAction = Sub()
                                            Using td As New TaskDialog(Of FontFamily)
                                                td.Title = "Choose a font for the timestamps"
                                                td.Symbol = Symbol.Font

                                                For Each ff In FontFamily.Families.Where(Function(x) Not x.Name.ToLowerEx().ContainsAny(" mdl2", " assets", "marlett", "ms outlook", "mt extra", "wingdings 2") AndAlso x.IsStyleAvailable(FontStyle.Regular) AndAlso x.IsMonospace())
                                                    td.AddCommand(ff.Name, ff)
                                                Next

                                                If td.Show IsNot Nothing Then
                                                    timestampFont.Edit.Text = td.SelectedText
                                                End If
                                            End Using
                                        End Sub

            cp = ui.AddColorPicker()
            cp.Text = "Timestamp Font Color:"
            cp.Expanded = True
            cp.Color = p.ThumbnailerSettings.GetString("TimestampFontColor", New ColorHSL(0, 0, 0.9, 1).ToHTML()).ToColor()
            cp.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("TimestampFontColor", ColorTranslator.ToHtml(value))

            n = ui.AddNum()
            n.Text = "Timestamp Font Size (%):"
            n.Config = {10, 1000, 1}
            n.NumEdit.Value = p.ThumbnailerSettings.GetInt("TimestampFontSizePercent", 100)
            n.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("TimestampFontSizePercent", CInt(value))

            cp = ui.AddColorPicker()
            cp.Text = "Timestamp Outline Color:"
            cp.Expanded = True
            cp.Color = p.ThumbnailerSettings.GetString("TimestampOutlineColor", New ColorHSL(0, 0, 0.1, 1).ToHTML()).ToColor()
            cp.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("TimestampOutlineColor", ColorTranslator.ToHtml(value))

            n = ui.AddNum()
            n.Text = "Timestamp Outline Strength (%):"
            n.Config = {0, 1000, 2}
            n.NumEdit.Value = p.ThumbnailerSettings.GetInt("TimestampOutlineStrengthPercent", 100)
            n.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("TimestampOutlineStrengthPercent", CInt(value))


            Dim thumbsMode = ui.AddMenu(Of ThumbnailerRowMode)
            Dim thumbsColumns = ui.AddNum()
            Dim thumbsRows = ui.AddNum()
            Dim thumbsInterval = ui.AddNum()

            thumbsMode.Text = "Mode to set number of rows:"
            thumbsMode.Expanded = True
            thumbsMode.Button.Value = DirectCast([Enum].Parse(GetType(ThumbnailerRowMode), p.ThumbnailerSettings.GetInt("RowMode", ThumbnailerRowMode.Fixed).ToString()), ThumbnailerRowMode)
            thumbsMode.Button.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("RowMode", value)
            thumbsMode.Button.ValueChangedAction = Sub(value)
                                                       thumbsRows.Visible = value = ThumbnailerRowMode.Fixed
                                                       thumbsInterval.Visible = value = ThumbnailerRowMode.TimeInterval
                                                   End Sub

            thumbsColumns.Text = "Columns:"
            thumbsColumns.Config = {1, 50, 1}
            thumbsColumns.NumEdit.Value = p.ThumbnailerSettings.GetInt("Columns", 4)
            thumbsColumns.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("Columns", CInt(value))

            thumbsRows.Text = "Rows:"
            thumbsRows.Visible = thumbsMode.Button.Value = ThumbnailerRowMode.Fixed
            thumbsRows.Config = {1, 80, 1}
            thumbsRows.NumEdit.Value = p.ThumbnailerSettings.GetInt("Rows", 6)
            thumbsRows.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("Rows", CInt(value))

            thumbsInterval.Text = "Interval (s):"
            thumbsInterval.Visible = thumbsMode.Button.Value = ThumbnailerRowMode.TimeInterval
            thumbsInterval.Config = {1, 1800, 1}
            thumbsInterval.NumEdit.Value = p.ThumbnailerSettings.GetInt("Interval", 60)
            thumbsInterval.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("Interval", CInt(value))

            n = ui.AddNum()
            n.Text = "Width of each thumb (px):"
            n.Config = {0, 1920, 10}
            n.NumEdit.Value = p.ThumbnailerSettings.GetInt("ThumbWidth", 600)
            n.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("ThumbWidth", CInt(value))



            '   ----------------------------------------------------------------
            Dim pathPage = ui.CreateFlowPage("Paths")

            l = ui.AddLabel(pathPage, "Default Target Folder:")
            l.Help = "Leave empty to use the source file folder."

            Dim macroAction = Function() As Object
                                  MacrosForm.ShowDialogForm()
                              End Function

            Dim tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.DefaultTargetFolder
            tm.Edit.SaveAction = Sub(value) p.DefaultTargetFolder = value
            tm.AddMenu("Browse Folder...", Function() g.BrowseFolder(p.DefaultTargetFolder))
            tm.AddMenu("Directory of source file", "%source_dir%")
            tm.AddMenu("Parent directory of source file directory", "%source_dir_parent%")
            tm.AddMenu("Macros...", macroAction)

            l = ui.AddLabel(pathPage, "Default Target Name:")
            l.Help = "Leave empty to use the source filename"
            l.MarginTop = Font.Height

            tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.DefaultTargetName
            tm.Edit.SaveAction = Sub(value) p.DefaultTargetName = value
            tm.AddMenu("Name of source file without extension", "%source_name%")
            tm.AddMenu("Name of source file directory", "%source_dir_name%")
            tm.AddMenu("Macros...", macroAction)

            l = ui.AddLabel(pathPage, If(p.DeleteTempFilesMode = DeleteMode.Disabled, "Temp Files Folder:", $"Temp Files Folder: (MUST end with '_temp{Path.DirectorySeparatorChar}' for Auto-Deletion!)"))
            l.Help = "Leave empty to use the source file folder."
            l.MarginTop = Font.Height

            Dim tempDirFunc = Function()
                                  Dim tempDir = g.BrowseFolder(p.TempDir)

                                  If tempDir <> "" Then
                                      Return Path.Combine(tempDir, "%source_name%_temp")
                                  End If
                              End Function

            tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.TempDir
            tm.Edit.SaveAction = Sub(value) p.TempDir = value
            tm.AddMenu("Browse Folder...", tempDirFunc)
            tm.AddMenu("Source File Directory", $"%source_dir%{Path.DirectorySeparatorChar}%source_name%_temp")
            tm.AddMenu("Macros...", macroAction)

            l = ui.AddLabel(pathPage, "Default Thumbnails Path without extension:")
            l.Help = "Leave empty to save it next to the video file"
            l.MarginTop = Font.Height

            tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.ThumbnailerSettings.GetString("ImageFilePathWithoutExtension", "")
            tm.Edit.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("ImageFilePathWithoutExtension", value)
            tm.AddMenu("Path of target file without extension + Postfix", $"%target_dir%{Path.DirectorySeparatorChar}%target_name%_Thumbnail")
            tm.AddMenu("Path of target file without extension", $"%target_dir%{Path.DirectorySeparatorChar}%target_name%")
            tm.AddMenu("Macros...", macroAction)



            '   ----------------------------------------------------------------
            Dim systemTempFilesPage = ui.CreateFlowPage("Paths | Temp Files", True)

            Dim deleteModeMenu = ui.AddMenu(Of DeleteMode)
            Dim deleteSelectionMenu = ui.AddMenu(Of DeleteSelection)
            Dim deleteSelectionModeMenu = ui.AddMenu(Of SelectionMode)

            eb = ui.AddEmptyBlock(systemTempFilesPage)
            eb.Margin = New Padding(0, 6, 0, 3)
            eb.Visible = p.DeleteTempFilesMode <> DeleteMode.Disabled
            ui.AddLabel(eb, "Allowed on Frame Mismatch:", 7)
            ui.AddLabel(eb, "Too less:", 2)
            Dim deleteOnFrameMismatchNegative = ui.AddNumeric(eb)
            ui.AddLabel(eb, " ", 1)
            ui.AddLabel(eb, "Too much:", 2)
            Dim deleteOnFrameMismatchPositive = ui.AddNumeric(eb)

            Dim deleteCustomLabel = ui.AddLabel("Custom file extensions (space separated):")
            Dim deleteCustom = ui.AddTextMenu()
            Dim deleteSelectiveLabelExcludeText = "Select what shall be excluded:"
            Dim deleteSelectiveLabelIncludeText = "Select what shall be included:"
            Dim deleteSelectiveLabel = ui.AddLabel(If(p.DeleteTempFilesSelectionMode = SelectionMode.Exclude, deleteSelectiveLabelExcludeText, deleteSelectiveLabelIncludeText))
            Dim deleteSelectiveProjects = ui.AddBool()
            Dim deleteSelectiveLogs = ui.AddBool()
            Dim deleteSelectiveScripts = ui.AddBool()
            Dim deleteSelectiveIndexes = ui.AddBool()
            Dim deleteSelectiveVideos = ui.AddBool()
            Dim deleteSelectiveAudios = ui.AddBool()
            Dim deleteSelectiveSubtitles = ui.AddBool()


            deleteModeMenu.Text = "Deletion after successful processing:"
            deleteModeMenu.Expanded = True
            deleteModeMenu.Field = NameOf(p.DeleteTempFilesMode)
            deleteModeMenu.Button.ValueChangedAction = Sub(value)
                                                           Dim deleteModeActive = value <> DeleteMode.Disabled
                                                           deleteSelectionMenu.Visible = deleteModeActive
                                                           deleteSelectionMenu.Button.ValueChangedAction.Invoke(deleteSelectionMenu.Button.Value)
                                                           eb.Visible = deleteModeActive
                                                       End Sub

            deleteSelectionMenu.Text = "Selection:"
            deleteSelectionMenu.Expanded = True
            deleteSelectionMenu.Field = NameOf(p.DeleteTempFilesSelection)
            deleteSelectionMenu.Visible = p.DeleteTempFilesMode <> DeleteMode.Disabled
            deleteSelectionMenu.Button.ValueChangedAction = Sub(value)
                                                                Dim activeCustom = deleteSelectionMenu.Visible AndAlso value = DeleteSelection.Custom
                                                                Dim activeSelective = deleteSelectionMenu.Visible AndAlso value = DeleteSelection.Selective
                                                                deleteSelectionModeMenu.Visible = activeCustom OrElse activeSelective
                                                                deleteCustomLabel.Visible = activeCustom
                                                                deleteCustom.Visible = activeCustom
                                                                deleteSelectiveLabel.Visible = activeSelective
                                                                deleteSelectiveProjects.Visible = activeSelective
                                                                deleteSelectiveLogs.Visible = activeSelective
                                                                deleteSelectiveScripts.Visible = activeSelective
                                                                deleteSelectiveIndexes.Visible = activeSelective
                                                                deleteSelectiveVideos.Visible = activeSelective
                                                                deleteSelectiveAudios.Visible = activeSelective
                                                                deleteSelectiveSubtitles.Visible = activeSelective
                                                            End Sub

            deleteSelectionModeMenu.Text = "Selection Mode:"
            deleteSelectionModeMenu.Expanded = True
            deleteSelectionModeMenu.Visible = p.DeleteTempFilesMode <> DeleteMode.Disabled AndAlso p.DeleteTempFilesSelection <> DeleteSelection.Everything
            deleteSelectionModeMenu.Field = NameOf(p.DeleteTempFilesSelectionMode)
            deleteSelectionModeMenu.Button.ValueChangedAction = Sub(value)
                                                                    deleteSelectiveLabel.Text = If(value = SelectionMode.Exclude, deleteSelectiveLabelExcludeText, deleteSelectiveLabelIncludeText)
                                                                    If value = SelectionMode.Exclude Then
                                                                        MsgWarn("Be aware!", "Every not selected, listed or identified file type will be deleted!")
                                                                    End If
                                                                End Sub

            deleteOnFrameMismatchNegative.Help = "Number of frames that the target file may have too less in order to delete the temp files. (-1 disables the check)"
            deleteOnFrameMismatchNegative.Config = {-1, 999, 1, 0}
            deleteOnFrameMismatchNegative.Field = NameOf(p.DeleteTempFilesOnFrameMismatchNegative)

            deleteOnFrameMismatchPositive.Help = "Number of frames that the target file may have too much in order to delete the temp files. (-1 disables the check)"
            deleteOnFrameMismatchPositive.Config = {-1, 999, 1, 0}
            deleteOnFrameMismatchPositive.Field = NameOf(p.DeleteTempFilesOnFrameMismatchPositive)

            deleteCustomLabel.MarginTop = Font.Height \ 2
            deleteCustomLabel.Visible = p.DeleteTempFilesMode <> DeleteMode.Disabled AndAlso p.DeleteTempFilesSelection = DeleteSelection.Custom

            Dim customAddFunc = Function(extensions As String()) As String
                                    Dim exts = deleteCustom.Edit.Text.ToLower().SplitNoEmpty(BR, " ")
                                    Dim allExts = exts.Union(extensions)
                                    Return allExts.Distinct().Sort().Join(" ")
                                End Function

            deleteCustom.Visible = p.DeleteTempFilesMode <> DeleteMode.Disabled AndAlso p.DeleteTempFilesSelection = DeleteSelection.Custom
            deleteCustom.Label.Visible = False
            deleteCustom.Edit.MultilineHeightFactor = 8
            deleteCustom.Edit.Expand = True
            deleteCustom.Edit.Text = p.DeleteTempFilesCustomSelection.Join(" ")
            deleteCustom.Edit.SaveAction = Sub(value) p.DeleteTempFilesCustomSelection = value.ToLower().SplitNoEmpty(BR, " ")
            deleteCustom.AddMenu("Add project file types", Function() customAddFunc(FileTypes.Projects))
            deleteCustom.AddMenu("Add log file types", Function() customAddFunc(FileTypes.Logs))
            deleteCustom.AddMenu("Add script file types", Function() customAddFunc(FileTypes.Scripts))
            deleteCustom.AddMenu("Add index file types", Function() customAddFunc(FileTypes.Indexes))
            deleteCustom.AddMenu("Add video file types", Function() customAddFunc(FileTypes.Video))
            deleteCustom.AddMenu("Add audio file types", Function() customAddFunc(FileTypes.Audio))
            deleteCustom.AddMenu("Add subtitle file types", Function() customAddFunc(FileTypes.SubtitleExludingContainers))

            Dim selectiveVisible = p.DeleteTempFilesMode <> DeleteMode.Disabled AndAlso p.DeleteTempFilesSelection = DeleteSelection.Selective

            deleteSelectiveLabel.MarginTop = Font.Height \ 2
            deleteSelectiveLabel.Visible = selectiveVisible

            deleteSelectiveProjects.Text = "Project files"
            deleteSelectiveProjects.Checked = p.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Projects)
            deleteSelectiveProjects.Visible = selectiveVisible
            deleteSelectiveProjects.SaveAction = Sub(value) p.DeleteTempFilesSelectiveSelection = If(value, p.DeleteTempFilesSelectiveSelection Or DeleteSelectiveSelection.Projects, p.DeleteTempFilesSelectiveSelection And Not DeleteSelectiveSelection.Projects)

            deleteSelectiveLogs.Text = "Log files"
            deleteSelectiveLogs.Checked = p.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Logs)
            deleteSelectiveLogs.Visible = selectiveVisible
            deleteSelectiveLogs.SaveAction = Sub(value) p.DeleteTempFilesSelectiveSelection = If(value, p.DeleteTempFilesSelectiveSelection Or DeleteSelectiveSelection.Logs, p.DeleteTempFilesSelectiveSelection And Not DeleteSelectiveSelection.Logs)

            deleteSelectiveScripts.Text = "Script files"
            deleteSelectiveScripts.Checked = p.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Scripts)
            deleteSelectiveScripts.Visible = selectiveVisible
            deleteSelectiveScripts.SaveAction = Sub(value) p.DeleteTempFilesSelectiveSelection = If(value, p.DeleteTempFilesSelectiveSelection Or DeleteSelectiveSelection.Scripts, p.DeleteTempFilesSelectiveSelection And Not DeleteSelectiveSelection.Scripts)

            deleteSelectiveIndexes.Text = "Index files"
            deleteSelectiveIndexes.Checked = p.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Indexes)
            deleteSelectiveIndexes.Visible = selectiveVisible
            deleteSelectiveIndexes.SaveAction = Sub(value) p.DeleteTempFilesSelectiveSelection = If(value, p.DeleteTempFilesSelectiveSelection Or DeleteSelectiveSelection.Indexes, p.DeleteTempFilesSelectiveSelection And Not DeleteSelectiveSelection.Indexes)

            deleteSelectiveVideos.Text = "Video files"
            deleteSelectiveVideos.Checked = p.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Videos)
            deleteSelectiveVideos.Visible = selectiveVisible
            deleteSelectiveVideos.SaveAction = Sub(value) p.DeleteTempFilesSelectiveSelection = If(value, p.DeleteTempFilesSelectiveSelection Or DeleteSelectiveSelection.Videos, p.DeleteTempFilesSelectiveSelection And Not DeleteSelectiveSelection.Videos)

            deleteSelectiveAudios.Text = "Audio files"
            deleteSelectiveAudios.Checked = p.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Audios)
            deleteSelectiveAudios.Visible = selectiveVisible
            deleteSelectiveAudios.SaveAction = Sub(value) p.DeleteTempFilesSelectiveSelection = If(value, p.DeleteTempFilesSelectiveSelection Or DeleteSelectiveSelection.Audios, p.DeleteTempFilesSelectiveSelection And Not DeleteSelectiveSelection.Audios)

            deleteSelectiveSubtitles.Text = "Subtitles"
            deleteSelectiveSubtitles.Checked = p.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Subtitles)
            deleteSelectiveSubtitles.Visible = selectiveVisible
            deleteSelectiveSubtitles.SaveAction = Sub(value) p.DeleteTempFilesSelectiveSelection = If(value, p.DeleteTempFilesSelectiveSelection Or DeleteSelectiveSelection.Subtitles, p.DeleteTempFilesSelectiveSelection And Not DeleteSelectiveSelection.Subtitles)


            '   ----------------------------------------------------------------
            ui.CreateFlowPage("Assistant")

            b = ui.AddBool()
            b.Text = "Remind to crop"
            b.Field = NameOf(p.RemindToCrop)

            b = ui.AddBool()
            b.Text = "Remind to cut"
            b.Field = NameOf(p.RemindToCut)

            b = ui.AddBool()
            b.Text = "Remind to do compressibility check"
            b.Field = NameOf(p.RemindToDoCompCheck)

            b = ui.AddBool()
            b.Text = "Remind to set filters"
            b.Field = NameOf(p.RemindToSetFilters)

            b = ui.AddBool()
            b.Text = "Warn on aspect ratio error"
            b.Field = NameOf(p.WarnArError)

            b = ui.AddBool()
            b.Text = "Warn if no audio in output"
            b.Field = NameOf(p.WarnNoAudio)

            b = ui.AddBool()
            b.Text = "Warn if identical audio is used multiple times"
            b.Field = NameOf(p.WarnIdenticalAudio)


            '   ----------------------------------------------------------------
            Dim filtersPage = ui.CreateFlowPage("Filters")

            l = ui.AddLabel(filtersPage, "Code appended to trim functions:")
            l.Help = "Code appended to trim functions StaxRip generates using the cut feature."
            l.MarginTop = Font.Height \ 2

            t = ui.AddText(filtersPage)
            t.Label.Visible = False
            t.Edit.Expand = True
            t.Edit.TextBox.Multiline = True
            t.Edit.UseMacroEditor = True
            t.Edit.Text = p.TrimCode
            t.Edit.SaveAction = Sub(value) p.TrimCode = value

            l = ui.AddLabel(filtersPage, "Code inserted at top of scripts:")
            l.Help = "Code inserted at the top of every script StaxRip generates."
            l.MarginTop = Font.Height \ 2

            t = ui.AddText(filtersPage)
            t.Label.Visible = False
            t.Edit.Expand = True
            t.Edit.TextBox.Multiline = True
            t.Edit.UseMacroEditor = True
            t.Edit.Text = p.CodeAtTop
            t.Edit.SaveAction = Sub(value) p.CodeAtTop = value

            l = ui.AddLabel(filtersPage, "Code inserted at bottom of scripts:")
            l.Help = "Code inserted at the bottom of every script StaxRip generates."
            l.MarginTop = Font.Height \ 2

            t = ui.AddText(filtersPage)
            t.Label.Visible = False
            t.Edit.Expand = True
            t.Edit.TextBox.Multiline = True
            t.Edit.UseMacroEditor = True
            t.Edit.Text = p.CodeAtBottom
            t.Edit.SaveAction = Sub(value) p.CodeAtBottom = value


            '   ----------------------------------------------------------------
            Dim miscPage = ui.CreateFlowPage("Misc")
            miscPage.SuspendLayout()

            b = ui.AddBool(miscPage)
            b.Text = "Hide dialogs asking to demux, source filter etc."
            b.Checked = p.NoDialogs
            b.SaveAction = Sub(value) p.NoDialogs = value

            b = ui.AddBool(miscPage)
            b.Text = "Use source file folder for temp files"
            b.Checked = p.NoTempDir
            b.SaveAction = Sub(value) p.NoTempDir = value

            b = ui.AddBool(miscPage)
            b.Text = "Abort on Frame Mismatch"
            b.Field = NameOf(p.AbortOnFrameMismatch)

            ui.AddLine(miscPage, "Compressibility Check")

            b = ui.AddBool(miscPage)
            b.Text = "Auto run compressibility check"
            b.Help = "Performs a compressibility check after loading a source file."
            b.Checked = p.AutoCompCheck
            b.SaveAction = Sub(value) p.AutoCompCheck = value

            n = ui.AddNum(miscPage)
            n.Label.Text = "Percentage of length to check"
            n.NumEdit.Config = {1, 25}
            n.NumEdit.Value = p.CompCheckPercentage
            n.NumEdit.SaveAction = Sub(value) p.CompCheckPercentage = value

            n = ui.AddNum(miscPage)
            n.Label.Text = "Seconds per test block"
            n.NumEdit.Config = {0.5, 10.0, 0.1, 2}
            n.NumEdit.Value = p.CompCheckTestblockSeconds
            n.NumEdit.SaveAction = Sub(value) p.CompCheckTestblockSeconds = value

            Dim compCheckButton = ui.AddMenu(Of CompCheckAction)(miscPage)
            compCheckButton.Label.Text = "After comp. check adjust"
            compCheckButton.Button.Value = p.CompCheckAction
            compCheckButton.Button.SaveAction = Sub(value) p.CompCheckAction = value

            miscPage.ResumeLayout()

            '   ----------------------------------------------------------------


            If pagePath <> "" Then
                ui.ShowPage(pagePath)
            Else
                ui.SelectLast("last options page")
            End If

            If form.ShowDialog() = DialogResult.OK Then
                Dim autoCropModeOn = autoCropMode.Button.Value <> StaxRip.AutoCropMode.Disabled
                Dim autoCropModeChanged = autoCropMode.Button.Value <> p.AutoCropMode
                Dim dvDataAvailable = p.HdrDolbyVisionMetadataFile IsNot Nothing
                Dim cropFilterActive = p.Script.GetFilter("Crop")?.Active
                Dim cropChanged = leftCrop.Value <> p.CropLeft OrElse topCrop.Value <> p.CropTop OrElse rightCrop.Value <> p.CropRight OrElse bottomCrop.Value <> p.CropBottom
                Dim dvThresholdChanged = doviThresholdBegin.Value <> p.AutoCropDolbyVisionThresholdBegin OrElse doviThresholdEnd.Value <> p.AutoCropDolbyVisionThresholdEnd

                ui.Save()

                If p.CompCheckPercentage < 1 OrElse p.CompCheckPercentage > 25 Then p.CompCheckPercentage = 5
                If p.CompCheckTestblockSeconds < 0.5 OrElse p.CompCheckTestblockSeconds > 10.0 Then p.CompCheckTestblockSeconds = 2.0
                If autoCropModeOn AndAlso autoCropModeChanged AndAlso dvDataAvailable AndAlso cropFilterActive Then StartAutoCrop()

                If cropChanged Then
                    p.CropLeft = CInt(leftCrop.Value)
                    p.CropTop = CInt(topCrop.Value)
                    p.CropRight = CInt(rightCrop.Value)
                    p.CropBottom = CInt(bottomCrop.Value)
                End If

                UpdateSizeOrBitrate()
                tbBitrate_TextChanged()
                SetSlider()
                SetAudioTracks(p)
                Assistant()
            End If

            ui.SaveLast("last options page")
        End Using
    End Sub

    Sub SetCropFilter()
        If CInt(p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 Then
            If Not g.EnableFilter("Crop") Then
                If p.Script.IsAviSynth Then
                    p.Script.InsertAfter("Source", New VideoFilter("Crop", "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)"))
                Else
                    p.Script.InsertAfter("Source", New VideoFilter("Crop", "Crop", "clip = core.std.Crop(clip, %crop_left%, %crop_right%, %crop_top%, %crop_bottom%)"))
                End If
            End If

            FiltersListView.Load()
        End If
    End Sub

    Sub DisableCropFilter()
        Dim f = p.Script.GetFilter("Crop")

        If f IsNot Nothing AndAlso CInt(p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) = 0 Then
            f.Active = False
            FiltersListView.Load()
        End If
    End Sub

    <Command("Dialog to configure filter setup profiles.")>
    Sub ShowFilterSetupProfilesDialog()
        Using form As New ProfilesForm(
            "Filter Setup Profiles",
            s.FilterSetupProfiles,
            AddressOf LoadFilterSetup,
            AddressOf GetScriptAsProfile,
            AddressOf VideoScript.GetDefaults)

            If form.ShowDialog() = DialogResult.OK Then
                PopulateProfileMenu(DynamicMenuItemID.FilterSetupProfiles)
                FiltersListView.RebuildMenu()
            End If
        End Using
    End Sub

    <Command("Dialog to configure filter profiles.")>
    Sub ShowFilterProfilesDialog()
        Dim filterProfiles = If(p.Script.IsAviSynth, s.AviSynthProfiles, s.VapourSynthProfiles)
        Dim getDefaults = If(p.Script.IsAviSynth,
            Function() FilterCategory.GetAviSynthDefaults,
            Function() FilterCategory.GetVapourSynthDefaults)

        Using dialog As New MacroEditorDialog
            dialog.SetScriptDefaults()
            dialog.Text = $"Filter Profiles - {g.DefaultCommands.GetApplicationDetails()}"
            dialog.MacroEditorControl.Value = g.GetFilterProfilesText(filterProfiles)
            dialog.bnContext.Text = " Restore Defaults... "
            dialog.bnContext.Visible = True
            dialog.MacroEditorControl.rtbDefaults.Text = g.GetFilterProfilesText(getDefaults())
            dialog.bnContext.ClickAction = Sub()
                                               If Msg("Restore defaults?", Nothing, TaskIcon.Warning, TaskButton.OkCancel) = DialogResult.OK Then
                                                   dialog.MacroEditorControl.Value = g.GetFilterProfilesText(getDefaults())
                                                   MsgInfo("Defaults were restored.")
                                               End If
                                           End Sub

            If dialog.ShowDialog() = DialogResult.OK Then
                Try
                    filterProfiles = FilterCategory.ParseFilterProfilesIniContent(dialog.MacroEditorControl.Value)

                    For Each i In getDefaults()
                        Dim found As Boolean

                        For Each i2 In filterProfiles
                            If i.Name = i2.Name Then
                                found = True
                            End If
                        Next

                        If Not found AndAlso {"Source", "Crop", "Resize"}.Contains(i.Name) Then
                            MsgWarn($"The category '{i.Name}' was recreated. A Source, Crop and Resize category is mandatory.")
                            filterProfiles.Add(i)
                        End If
                    Next

                    If p.Script.IsAviSynth Then
                        s.AviSynthProfiles = filterProfiles
                    Else
                        s.VapourSynthProfiles = filterProfiles
                    End If

                    g.SaveSettings()
                    g.MainForm.FiltersListView.RebuildMenu()
                Catch ex As Exception
                    g.ShowException(ex)
                End Try
            End If
        End Using
    End Sub

    Shared Function GetDefaultMainMenu() As CustomMenuItem
        Dim ret As New CustomMenuItem("Root")

        ret.Add("File|Open Video Source File(s)...", NameOf(ShowOpenSourceDialog), Keys.O Or Keys.Control, Symbol.Preview)
        ret.Add("File|Demux...", NameOf(g.DefaultCommands.ShowDemuxTool))
        ret.Add("File|-")
        ret.Add("File|Video Comparison...", NameOf(ShowVideoComparison), Keys.F5, Symbol.VideoLegacy)
        ret.Add("File|-")
        ret.Add("File|Open Project...", NameOf(ShowFileBrowserToOpenProject))
        ret.Add("File|Save Project", NameOf(SaveProject), Keys.S Or Keys.Control, Symbol.Save)
        ret.Add("File|Save Project As...", NameOf(SaveProjectAs), Keys.S Or Keys.Control Or Keys.Shift, Symbol.SaveAs)
        ret.Add("File|Save Project As Template...", NameOf(SaveProjectAsTemplate))
        ret.Add("File|Close Project", NameOf(CloseProject), Keys.W Or Keys.Control)
        ret.Add("File|-")
        ret.Add("File|Project Templates", NameOf(g.DefaultCommands.DynamicMenuItem), {DynamicMenuItemID.TemplateProjects})
        ret.Add("File|Recent Projects", NameOf(g.DefaultCommands.DynamicMenuItem), {DynamicMenuItemID.RecentProjects})
        ret.Add("File|-")
        ret.Add("File|Launch New Instance", NameOf(StartNewInstance), Keys.N Or Keys.Control)
        ret.Add("File|-")
        ret.Add("File|Exit", NameOf([Exit]), Keys.X Or Keys.Alt)

        ret.Add("Crop", NameOf(ShowCropDialog), Keys.F3, Symbol.fa_scissors)
        ret.Add("Preview", NameOf(ShowPreview), Keys.F4, Symbol.fa_eye)

        ret.Add("Project|Add Hardcoded Subtitle...", NameOf(ShowHardcodedSubtitleDialogFromTempDir), Keys.Control Or Keys.H, Symbol.Subtitles)
        ret.Add("Project|Script Info...", NameOf(ShowScriptInfo), Keys.F2)
        ret.Add("Project|Advanced Script Info...", NameOf(ShowAdvancedScriptInfo), Keys.Control Or Keys.F2)
        ret.Add("Project|-")
        ret.Add("Project|Log File", NameOf(g.DefaultCommands.ShowLogFile), Keys.F8, Symbol.Page)
        ret.Add("Project|-")
        ret.Add("Project|Folders|Source", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%source_dir%"""})
        ret.Add("Project|Folders|Target", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%target_dir%"""})
        ret.Add("Project|Folders|Temp", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%temp_dir%"""})
        ret.Add("Project|Folders|Working", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%working_dir%"""})
        ret.Add("Project|-")
        ret.Add("Project|Options", NameOf(ShowOptionsDialog), Keys.F9, Symbol.Project)

        ret.Add("Tools|Jobs...", NameOf(ShowJobsDialog), Keys.F6, Symbol.MultiSelectLegacy)
        ret.Add("Tools|Folders", Symbol.Folder)
        ret.Add("Tools|Folders|Log Files", NameOf(g.DefaultCommands.ExecuteCommandLine), {$"""%settings_dir%{IO.Path.DirectorySeparatorChar}Log Files"""})
        ret.Add("Tools|Folders|Plugins", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%plugin_dir%"""})
        ret.Add("Tools|Folders|Programs", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%programs_dir%"""})
        ret.Add("Tools|Folders|Scripts", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%script_dir%"""})
        ret.Add("Tools|Folders|Settings", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%settings_dir%"""})
        ret.Add("Tools|Folders|Startup", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%startup_dir%"""})
        ret.Add("Tools|Folders|System", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%system_dir%"""})
        ret.Add("Tools|Folders|Templates", NameOf(g.DefaultCommands.ExecuteCommandLine), {$"""%settings_dir%{IO.Path.DirectorySeparatorChar}Templates"""})

        ret.Add("Tools|Scripts", NameOf(g.DefaultCommands.DynamicMenuItem), Symbol.Code, {DynamicMenuItemID.Scripts})

        ret.Add("Tools|Advanced", Symbol.More)
        ret.Add("Tools|Advanced|-")
        ret.Add("Tools|Advanced|Event Commands...", NameOf(ShowEventCommandsDialog), Keys.Control Or Keys.E, Symbol.LightningBolt)
        ret.Add("Tools|Advanced|Reset Settings...", NameOf(g.DefaultCommands.ResetSettings))
        ret.Add("Tools|Advanced|-")
        ret.Add("Tools|Advanced|Command Prompt", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.fa_terminal, {"cmd.exe", False, False, False, "%working_dir%"})
        ret.Add("Tools|Advanced|PowerShell Terminal", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.fa_terminal, {"powershell.exe -nologo -executionpolicy bypass", False, False, False, "%working_dir%"})

        If g.IsWindowsTerminalAvailable Then
            ret.Add("Tools|Advanced|Windows Terminal", NameOf(g.DefaultCommands.ExecuteCommandLine), Keys.Control Or Keys.T, Symbol.fa_terminal, {"wt.exe", False, False, False, "%working_dir%"})
        End If

        ret.Add("Tools|Advanced|-")
        ret.Add("Tools|Advanced|Generate Wiki Content", NameOf(g.DefaultCommands.GenerateWikiContent))
        ret.Add("Tools|Advanced|Ingest HDR", NameOf(g.DefaultCommands.SaveMKVHDR))

        ret.Add("Tools|-")
        ret.Add("Tools|Edit Menu...", NameOf(ShowMainMenuEditor))
        ret.Add("Tools|Settings...", NameOf(ShowSettingsDialog), Keys.F10, Symbol.Settings, {""})

        ret.Add("Apps|Subtitles|Subtitle Edit", NameOf(g.DefaultCommands.StartTool), {"Subtitle Edit"})
        ret.Add("Apps|Subtitles|BDSup2Sub++", NameOf(g.DefaultCommands.StartTool), {"BDSup2Sub++"})
        ret.Add("Apps|Subtitles|VSRip", NameOf(g.DefaultCommands.StartTool), {"VSRip"})
        ret.Add("Apps|Media Info|mkvinfo", NameOf(g.DefaultCommands.ShowMkvInfo))
        ret.Add("Apps|Media Info|MediaInfo File", NameOf(g.DefaultCommands.ShowMediaInfo))
        ret.Add("Apps|Media Info|MediaInfo Folder", NameOf(g.DefaultCommands.ShowMediaInfoFolderViewDialog))
        ret.Add("Apps|Players|mpv.net", NameOf(g.DefaultCommands.StartTool), {"mpv.net"})
        ret.Add("Apps|Players|MPC", NameOf(g.DefaultCommands.StartTool), {"MPC"})
        ret.Add("Apps|Indexing|D2V Witch", NameOf(g.DefaultCommands.StartTool), {"D2V Witch"})
        ret.Add("Apps|Indexing|DGIndex", NameOf(g.DefaultCommands.StartTool), {"DGIndex"})
        ret.Add("Apps|Thumbnails|StaxRip Thumbnailer (using project options)", NameOf(g.DefaultCommands.ShowThumbnailerDialogAsync))
        ret.Add("Apps|Animation|Animated GIF", NameOf(g.DefaultCommands.SaveGIF))
        ret.Add("Apps|Animation|Animated PNG", NameOf(g.DefaultCommands.SavePNG))
        ret.Add("Apps|Other|MKVToolnix GUI", NameOf(g.DefaultCommands.StartTool), {"MKVToolnix GUI"})
        ret.Add("Apps|Other|chapterEditor", NameOf(g.DefaultCommands.StartTool), {"chapterEditor"})
        ret.Add("Apps|-")
        ret.Add("Apps|Manage...", NameOf(ShowAppsDialog), Keys.F11)

        If g.IsDevelopmentPC Then
            ret.Add("Apps|Test...", NameOf(g.DefaultCommands.Test), Keys.F12)
        End If

        ret.Add("Help|Apps", NameOf(g.DefaultCommands.DynamicMenuItem), {DynamicMenuItemID.HelpApplications})
        ret.Add("Help|-")
        ret.Add("Help|Website", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.Globe, {"https://github.com/staxrip/staxrip"})
        ret.Add("Help|Documentation", NameOf(g.DefaultCommands.ExecuteCommandLine), Keys.F1, Symbol.Help, {"https://github.com/staxrip/staxrip/wiki"})
        ret.Add("Help|Changelog", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.Bookmarks, {"https://github.com/staxrip/staxrip/blob/master/Changelog.md"})
        ret.Add("Help|Support", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.Heart, {"https://github.com/staxrip/staxrip?tab=readme-ov-file#contribution--support"})
        ret.Add("Help|-")
        ret.Add("Help|Report an issue", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.fa_bug, {"https://github.com/staxrip/staxrip/issues/new/choose"})
        ret.Add("Help|-")
        ret.Add("Help|Discord Server", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.People, {"https://discord.gg/uz8pVR79Bd"})
        ret.Add("Help|-")
        ret.Add("Help|Check for Updates", NameOf(g.DefaultCommands.CheckForUpdate), Symbol.fa_recycle)
        ret.Add("Help|-")
        ret.Add("Help|What's new...", NameOf(ShowChangelog), Symbol.Shield, {True})
        ret.Add("Help|Info...", NameOf(g.DefaultCommands.OpenHelpTopic), Symbol.Info, {"info"})

        Return ret
    End Function

    <Command("Shows a dialog, opening from the last source directory, to add a hardcoded subtitle.")>
    Sub ShowHardcodedSubtitleDialogFromLastSourceDir()
        ShowHardcodedSubtitleDialog(s.LastSourceDir)
    End Sub

    <Command("Shows a dialog, opening from the temp folder, to add a hardcoded subtitle.")>
    Sub ShowHardcodedSubtitleDialogFromTempDir()
        ShowHardcodedSubtitleDialog(p.TempDir)
    End Sub

    Sub ShowHardcodedSubtitleDialog(initDir As String)
        Using dialog As New OpenFileDialog
            dialog.SetFilter(FileTypes.SubtitleExludingContainers)
            dialog.SetInitDir(initDir)

            If dialog.ShowDialog = DialogResult.OK Then
                If dialog.FileName.Ext = "idx" Then
                    Dim subs = Subtitle.Create(dialog.FileName)

                    If subs.Count = 0 Then
                        MsgInfo("No subtitles found.")
                        Exit Sub
                    End If

                    Dim sb As New SelectionBox(Of Subtitle) With {
                        .Title = "Language",
                        .Text = "Please select a subtitle."
                    }

                    For Each i In subs
                        sb.AddItem(i.Language.EnglishName, i)
                    Next

                    If sb.Show = DialogResult.Cancel Then
                        Exit Sub
                    End If

                    Regex.Replace(dialog.FileName.ReadAllText, "langidx: \d+", "langidx: " +
                                  sb.SelectedValue.IndexIDX.ToString).WriteFileSystemEncoding(dialog.FileName)
                End If

                p.AddHardcodedSubtitleFilter(dialog.FileName, True)
            End If
        End Using
    End Sub

    Sub tbResize_MouseUp(sender As Object, e As MouseEventArgs) Handles tbResize.MouseUp
        Assistant()
    End Sub

    Sub tbResize_Scroll() Handles tbResize.Scroll
        SkipAssistant = True
        g.AddResizeFilter()
        tbTargetWidth.Text = CInt(p.SourceWidth \ 2 + tbResize.Value * Math.Max(TrackBarInterval, p.ForcedOutputMod)).ToString
        SetImageHeight()
        SkipAssistant = False
        Assistant(False)
    End Sub

    Sub SetImageHeight()
        Try
            Dim modval = p.ForcedOutputMod
            Dim ar As Double

            If ModifierKeys = Keys.Control Then
                Dim cropw = p.SourceWidth - p.CropLeft - p.CropRight
                Dim croph = p.SourceHeight - p.CropTop - p.CropBottom

                ar = cropw / croph
            Else
                ar = Calc.GetTargetDAR
            End If

            tbTargetHeight.Text = (CInt(p.TargetWidth / ar / modval) * modval).ToString()

            If p.TargetWidth = 1920 AndAlso p.TargetHeight = 1088 Then
                tbTargetHeight.Text = CStr(1080)
            End If
        Catch
        End Try
    End Sub

    Sub SetImageWidth()
        Try
            Dim modval = p.ForcedOutputMod
            tbTargetWidth.Text = (CInt(p.TargetHeight * Calc.GetTargetDAR / modval) * modval).ToString()
        Catch
        End Try
    End Sub

    Sub tbBitrate_KeyDown(sender As Object, e As KeyEventArgs) Handles tbBitrate.KeyDown
        If tbBitrate.ReadOnly Then
            Exit Sub
        End If

        If e.KeyData = Keys.Up Then
            e.Handled = True
            tbBitrate.Text = Math.Max(0, Calc.GetPreviousMod(tbBitrate.Text.ToInt, 50)).ToString
        ElseIf e.KeyData = Keys.Down Then
            e.Handled = True
            tbBitrate.Text = Math.Max(0, Calc.GetNextMod(tbBitrate.Text.ToInt, 50)).ToString
        End If
    End Sub

    Sub tbTargetSize_KeyDown(sender As Object, e As KeyEventArgs) Handles tbTargetSize.KeyDown
        If tbTargetSize.ReadOnly Then
            Exit Sub
        End If

        Dim modValue As Integer

        Select Case p.TargetSeconds
            Case Is > 50 * 60
                modValue = 50
            Case Is > 10 * 60
                modValue = 10
            Case Is > 60
                modValue = 5
            Case Else
                modValue = 1
        End Select

        If e.KeyData = Keys.Up Then
            e.Handled = True
            tbTargetSize.Text = Math.Max(0, Calc.GetPreviousMod(tbTargetSize.Text.ToInt, modValue)).ToString
        ElseIf e.KeyData = Keys.Down Then
            e.Handled = True
            tbTargetSize.Text = Math.Max(0, Calc.GetNextMod(tbTargetSize.Text.ToInt, modValue)).ToString
        End If
    End Sub

    Sub UpdateSizeOrBitrate()
        If p.BitrateIsFixed Then
            tbBitrate_TextChanged()
        Else
            tbSize_TextChanged()
        End If
    End Sub

    Sub tbSize_TextChanged() Handles tbTargetSize.TextChanged
        Try
            If tbTargetSize.TextBox.Focused Then
                p.BitrateIsFixed = False
            End If

            If Integer.TryParse(tbTargetSize.Text, Nothing) Then
                p.TargetSize = Math.Max(0, CInt(tbTargetSize.Text))
                BlockSize = True

                If Not BlockBitrate Then
                    tbBitrate.Text = CInt(Calc.GetVideoBitrate).ToString
                End If

                BlockSize = False
                Assistant()
            End If
        Catch
        End Try
    End Sub

    Sub tbBitrate_TextChanged() Handles tbBitrate.TextChanged
        Try
            If tbBitrate.TextBox.Focused Then
                p.BitrateIsFixed = True
            End If

            If Integer.TryParse(tbBitrate.Text, Nothing) Then
                p.VideoBitrate = Math.Max(0, CInt(tbBitrate.Text))
                p.VideoEncoder.Bitrate = p.VideoBitrate
                BlockBitrate = True

                If Not BlockSize Then
                    tbTargetSize.Text = $"{CInt(Calc.GetSizeInBytes \ PrefixedSize(2).Factor)}"
                End If

                BlockBitrate = False
                Assistant()
            End If
        Catch
        End Try
    End Sub

    Sub tbTargetWidth_TextChanged() Handles tbTargetWidth.TextChanged
        Try
            If Integer.TryParse(tbTargetWidth.Text, Nothing) Then
                p.TargetWidth = CInt(tbTargetWidth.Text)
                Assistant()
            End If
        Catch
        End Try
    End Sub

    Sub tbTargetHeight_TextChanged() Handles tbTargetHeight.TextChanged
        Try
            If Integer.TryParse(tbTargetHeight.Text, Nothing) Then
                p.TargetHeight = CInt(tbTargetHeight.Text)
                Assistant()
            End If
        Catch
        End Try
    End Sub

    Sub tbTargetWidth_KeyDown(sender As Object, e As KeyEventArgs) Handles tbTargetWidth.KeyDown
        If e.KeyData = Keys.Up Then
            e.Handled = True
            Dim modVal = p.ForcedOutputMod
            tbTargetWidth.Text = CInt(((p.TargetWidth + modVal) / modVal) * modVal).ToString()
        ElseIf e.KeyData = Keys.Down Then
            e.Handled = True
            Dim modVal = p.ForcedOutputMod
            tbTargetWidth.Text = CInt(((p.TargetWidth - modVal) / modVal) * modVal).ToString()
        End If
    End Sub

    Sub tbTargetHeight_KeyDown(sender As Object, e As KeyEventArgs) Handles tbTargetHeight.KeyDown
        If e.KeyData = Keys.Up Then
            e.Handled = True
            Dim modVal = p.ForcedOutputMod
            tbTargetHeight.Text = CInt(((p.TargetHeight + modVal) / modVal) * modVal).ToString()
        ElseIf e.KeyData = Keys.Down Then
            e.Handled = True
            Dim modVal = p.ForcedOutputMod
            tbTargetHeight.Text = CInt(((p.TargetHeight - modVal) / modVal) * modVal).ToString()
        End If
    End Sub

    Sub tbTargetWidth_MouseWheel(sender As Object, e As MouseEventArgs) Handles tbTargetWidth.MouseWheel
        If e.Delta < 0 Then
            Dim modVal = p.ForcedOutputMod
            tbTargetWidth.Text = CInt(((p.TargetWidth - modVal) / modVal) * modVal).ToString()
        Else
            Dim modVal = p.ForcedOutputMod
            tbTargetWidth.Text = CInt(((p.TargetWidth + modVal) / modVal) * modVal).ToString()
        End If
    End Sub

    Sub tbTargetHeight_MouseWheel(sender As Object, e As MouseEventArgs) Handles tbTargetHeight.MouseWheel
        If e.Delta < 0 Then
            Dim modVal = p.ForcedOutputMod
            tbTargetHeight.Text = CInt(((p.TargetHeight - modVal) / modVal) * modVal).ToString()
        Else
            Dim modVal = p.ForcedOutputMod
            tbTargetHeight.Text = CInt(((p.TargetHeight + modVal) / modVal) * modVal).ToString()
        End If
    End Sub

    Shared Function GetDefaultMenuSize() As CustomMenuItem
        Dim ret = New CustomMenuItem("Root")
        ret.Add("DVD/BD-5 (4482 MiB / 4700 MB)", NameOf(SetSize), {CInt(4700_000_000 \ PrefixedSize(2).Factor)})
        ret.Add("DVD-DL/BD-9 (8145 MiB / 8540 MB)", NameOf(SetSize), {CInt(8540_000_000 \ PrefixedSize(2).Factor)})
        ret.Add("-")
        ret.Add("BD (23842 MiB / 25000 MB)", NameOf(SetSize), {CInt(25000_000_000 \ PrefixedSize(2).Factor)})
        ret.Add("BD-DL (47684 MiB / 50000 MB)", NameOf(SetSize), {CInt(50000_000_000 \ PrefixedSize(2).Factor)})
        ret.Add("-")
        ret.Add("50%", NameOf(SetPercent), {50})
        ret.Add("60%", NameOf(SetPercent), {60})
        ret.Add("-")
        ret.Add("Edit Menu...", NameOf(ShowSizeMenuEditor))
        Return ret
    End Function

    <Command("Sets the target image size.")>
    Sub SetTargetImageSize(width As Integer, height As Integer)
        g.AddResizeFilter()

        If height = 0 AndAlso width > 0 Then
            tbTargetWidth.Text = width.ToString
            SetImageHeight()
        ElseIf width = 0 AndAlso height > 0 Then
            tbTargetHeight.Text = height.ToString
            SetImageWidth()
        Else
            tbTargetWidth.Text = width.ToString
            tbTargetHeight.Text = height.ToString
        End If

        Assistant()
    End Sub

    <Command("Sets the target image size by pixels (width x height).")>
    Sub SetTargetImageSizeByPixel(pixel As Integer)
        g.AddResizeFilter()

        Dim cropw = p.SourceWidth - p.CropLeft - p.CropRight
        Dim ar = Calc.GetTargetDAR()
        Dim w = Calc.FixMod16(CInt(Math.Sqrt(pixel * ar)))

        If w > cropw Then
            w = (cropw \ 16) * 16
        End If

        Dim h = Calc.FixMod16(CInt(w / ar))

        tbTargetWidth.Text = w.ToString()
        tbTargetHeight.Text = h.ToString()

        Assistant()
    End Sub

    <Command("Sets the target file size in MB.")>
    Sub SetSize(<DispName("Target File Size")> targetSize As Integer)
        tbTargetSize.Text = targetSize.ToString
        p.BitrateIsFixed = False
    End Sub

    <Command("Shows info about the output AviSynth/VapourSynth script.")>
    Sub ShowScriptInfo()
        g.ShowScriptInfo(p.Script)
    End Sub

    <Command("Shows advanced AviSynth/VapourSynth output script info using various tools.")>
    Sub ShowAdvancedScriptInfo()
        g.ShowAdvancedScriptInfo(p.Script)
    End Sub

    <Command("Sets the bitrate according to the compressibility.")>
    Sub SetPercent(<DispName("Percent Value")> value As Integer)
        tbTargetSize.Text = g.GetAutoSize(value).ToString
        p.BitrateIsFixed = False
    End Sub

    <Command("Sets the target video bitrate in Kbps.")>
    Sub SetBitrate(<DispName("Target Video Bitrate")> bitrate As Integer)
        tbBitrate.Text = bitrate.ToString
    End Sub

    <Command("Crops borders automatically.")>
    Sub StartAutoCrop()
        g.RunAutoCrop(Nothing)
        SetCropFilter()
        DisableCropFilter()
        Assistant()
    End Sub

    <Command("Crops borders automatically until the proper aspect ratio is found.")>
    Sub StartSmartCrop()
        g.SmartCrop()
        SetCropFilter()
        DisableCropFilter()
        Assistant()
    End Sub

    <Command("Menu editor for the size menu.")>
    Sub ShowSizeMenuEditor()
        s.CustomMenuSize = CustomSizeMenu.Edit()
    End Sub

    Sub UpdateSizeMenu()
        For Each i As MenuItemEx In CustomSizeMenu.MenuItems
            If i.CustomMenuItem.MethodName = "SetPercent" Then
                i.Enabled = p.Compressibility > 0
            End If
        Next
    End Sub

    Function GetScriptAsProfile() As Profile
        Dim sb As New SelectionBox(Of TargetVideoScript) With {
            .Title = "New Profile",
            .Text = "Please select a profile."
        }

        sb.AddItem("Current Project", p.Script)

        For Each i In VideoScript.GetDefaults
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK Then
            Return sb.SelectedValue
        End If
    End Function

    Sub LoadFilterSetup(profileInterface As Profile)
        Dim profile = DirectCast(ObjectHelp.GetCopy(profileInterface), TargetVideoScript)

        If profile.Engine = ScriptEngine.AviSynth OrElse (Package.Python.VerifyOK(True) AndAlso Package.VapourSynth.VerifyOK(True) AndAlso Package.vspipe.VerifyOK(True)) Then
            Dim currentSetup = p.Script

            Try
                p.Script = profile
                SetCropFilter()
                PreviewScript = Nothing
                ModifyFilters()
                FiltersListView.OnChanged()
                Assistant()
            Catch ex As Exception
                p.Script = currentSetup
                PreviewScript = Nothing
                ModifyFilters()
                FiltersListView.OnChanged()
                Assistant()
            End Try
        End If

        FiltersListView.RebuildMenu()
    End Sub

    Function ParseCommandLine(commandLine As String, Optional delimiter As Char = " "c) As String()
        If String.IsNullOrWhiteSpace(commandLine) Then Return New List(Of String)().ToArray()

        Dim args = New List(Of String)
        Dim sb = New StringBuilder()
        Dim insideQuote = False

        For i = 0 To commandLine.Length - 1
            If commandLine.Chars(i) = """" Then
                sb.Append(commandLine.Chars(i))
                insideQuote = Not insideQuote
            ElseIf commandLine.Chars(i) = delimiter Then
                If insideQuote Then
                    sb.Append(commandLine.Chars(i))
                Else
                    Dim str = sb.ToString()
                    If Not String.IsNullOrWhiteSpace(str) Then
                        If str.First() = """"c AndAlso str.Last() = """"c Then str = str.Trim(""""c)
                        args.Add(str)
                    End If
                    sb = New StringBuilder()
                End If
            Else
                sb.Append(commandLine.Chars(i))
            End If
        Next

        If sb.Length > 0 Then args.Add(sb.ToString())

        Return args.ToArray()
    End Function

    Sub ProcessCommandLine(commandLine As String)
        If String.IsNullOrWhiteSpace(commandLine) Then Exit Sub

        Dim args = ParseCommandLine(commandLine)
        If args.Any() Then
            Package.LoadConfAll()
        Else
            Exit Sub
        End If

        Dim files As New List(Of String)
        Dim showTemplateSelection = (s.ShowTemplateSelection And (ShowTemplateSelectionMode.Always Or ShowTemplateSelectionMode.CommandLine)) <> 0
        Dim forcedTemplateLoading = args.Where(Function(s) s.ToLowerInvariant().Unescape().StartsWith("-" & NameOf(LoadTemplate).ToLowerInvariant())).Any()

        For Each arg In args.Skip(1)
            Try
                If Not arg.FileExists() AndAlso files.Count > 0 Then
                    Dim files2 As New List(Of String)(files)
                    Refresh()
                    If Not showTemplateSelection AndAlso Not forcedTemplateLoading Then OpenProject(g.LastModifiedTemplate)
                    OpenAnyFile(files2, showTemplateSelection AndAlso Not forcedTemplateLoading)
                    files.Clear()
                End If

                If arg.FileExists() Then
                    files.Add(arg)
                Else
                    If Not CommandManager.ProcessCommandLineArgument(arg) Then
                        Throw New Exception
                    End If
                End If
            Catch ex As Exception
                MsgWarn("Error parsing argument:" + BR2 + arg + BR2 + ex.Message)
            End Try
        Next

        If files.Count > 0 Then
            Refresh()
            If Not showTemplateSelection AndAlso Not forcedTemplateLoading Then OpenProject(g.LastModifiedTemplate)
            OpenAnyFile(files, showTemplateSelection AndAlso Not forcedTemplateLoading)
        End If
    End Sub

    <Command("Sets the project option 'Hide dialogs asking to demux, source filter etc.'")>
    Sub SetHideDialogsOption(hide As Boolean)
        p.NoDialogs = hide
    End Sub

    <Command("Standby PC.")>
    Sub Standby()
        g.ShutdownPC(ShutdownMode.Standby)
    End Sub

    <Command("Shut down PC.")>
    Sub Shutdown()
        g.ShutdownPC(ShutdownMode.Shutdown)
    End Sub

    Sub SetEncoderControl(c As Control)
        pnEncoder.Controls.Add(c)

        If pnEncoder.Controls.Count > 1 Then
            Dim old = pnEncoder.Controls(0)
            pnEncoder.Controls.Remove(old)
            old.Dispose()
        End If
    End Sub

    Sub UpdateEncoderStateRelatedControls()
        blFilesize.Visible = Not p.VideoEncoder.QualityMode
        tbTargetSize.Visible = Not p.VideoEncoder.QualityMode
        laBitrate.Visible = Not p.VideoEncoder.QualityMode
        tbBitrate.Visible = Not p.VideoEncoder.QualityMode
    End Sub

    <Command("Dialog to open a single file source.")>
    Sub ShowOpenSourceSingleFileDialog()
        SetLastModifiedTemplate()
        If p.SourceFile <> "" AndAlso IsSaveCanceled() Then Exit Sub

        Using dialog As New OpenFileDialog
            dialog.SetFilter(FileTypes.Video.Concat(FileTypes.Image))
            dialog.SetInitDir(s.LastSourceDir)

            If dialog.ShowDialog() = DialogResult.OK Then
                Dim showTemplateSelection = (s.ShowTemplateSelection And (ShowTemplateSelectionMode.Always Or ShowTemplateSelectionMode.OpeningMenu)) <> 0
                Dim showTemplateSelectionTimeout = If(s.ShowTemplateSelection <> ShowTemplateSelectionMode.Never, s.ShowTemplateSelectionTimeout, 0)

                If showTemplateSelection Then
                    If LoadTemplateWithSelectionDialog(dialog.FileNames, showTemplateSelectionTimeout) Then
                        OpenVideoSourceFiles(dialog.FileNames)
                    Else
                        Exit Sub
                    End If
                Else
                    OpenProject(g.LastModifiedTemplate)
                    OpenVideoSourceFiles(dialog.FileNames)
                End If
            End If
        End Using
    End Sub

    <Command("Dialog to open multiple file sources.")>
    Sub ShowOpenSourceMultipleFilesDialog()
        SetLastModifiedTemplate()
        If p.SourceFile <> "" AndAlso IsSaveCanceled() Then Exit Sub

        Using dialog As New OpenFileDialog
            dialog.SetFilter(FileTypes.Video)
            dialog.SetInitDir(s.LastSourceDir)
            dialog.Multiselect = True

            If dialog.ShowDialog() = DialogResult.OK Then
                Dim showTemplateSelection = (s.ShowTemplateSelection And (ShowTemplateSelectionMode.Always Or ShowTemplateSelectionMode.OpeningMenu)) <> 0
                Dim showTemplateSelectionTimeout = If(s.ShowTemplateSelection <> ShowTemplateSelectionMode.Never, s.ShowTemplateSelectionTimeout, 0)

                If showTemplateSelection Then
                    If LoadTemplateWithSelectionDialog(dialog.FileNames, showTemplateSelectionTimeout) Then
                        OpenVideoSourceFiles(dialog.FileNames)
                    Else
                        Exit Sub
                    End If
                Else
                    OpenProject(g.LastModifiedTemplate)
                    OpenVideoSourceFiles(dialog.FileNames)
                End If
            End If
        End Using
    End Sub

    <Command("Dialog to open a Blu-ray folder source.")>
    Sub ShowOpenSourceBlurayFolderDialog()
        SetLastModifiedTemplate()
        If p.SourceFile <> "" AndAlso IsSaveCanceled() Then Exit Sub

        Using dialog As New FolderBrowserDialog
            dialog.Description = "Please select a Blu-ray source folder."
            dialog.SetSelectedPath(s.Storage.GetString("last blu-ray source folder"))
            dialog.ShowNewFolderButton = False

            If dialog.ShowDialog = DialogResult.OK Then
                s.Storage.SetString("last blu-ray source folder", dialog.SelectedPath)

                Dim srcPath = dialog.SelectedPath.FixDir
                Dim showTemplateSelection = (s.ShowTemplateSelection And (ShowTemplateSelectionMode.Always Or ShowTemplateSelectionMode.OpeningMenu)) <> 0
                Dim showTemplateSelectionTimeout = If(s.ShowTemplateSelection <> ShowTemplateSelectionMode.Never, s.ShowTemplateSelectionTimeout, 0)

                If showTemplateSelection Then
                    If LoadTemplateWithSelectionDialog(srcPath, showTemplateSelectionTimeout) Then
                        OpenBlurayFolder(srcPath)
                    Else
                        Exit Sub
                    End If
                Else
                    OpenProject(g.LastModifiedTemplate)
                    OpenBlurayFolder(srcPath)
                End If
            End If
        End Using
    End Sub

    <Command("Dialog to open a merged files source.")>
    Sub ShowOpenSourceMergeFilesDialog()
        SetLastModifiedTemplate()
        If p.SourceFile <> "" AndAlso IsSaveCanceled() Then Exit Sub

        Using form As New SourceFilesForm()
            form.Text = $"Merge - {g.DefaultCommands.GetApplicationDetails()}"
            form.IsMerge = True

            If form.ShowDialog() = DialogResult.OK AndAlso form.lb.Items.Count > 0 Then
                Dim files = form.GetFiles

                Select Case files(0).Ext
                    Case "mpg", "vob"
                        OpenVideoSourceFiles(files)
                    Case Else
                        Using proc As New Proc
                            proc.Header = "Merge source files"

                            For Each i In files
                                Log.WriteLine(MediaInfo.GetSummary(i) + "---------------------------------------------------------" + BR2)
                            Next

                            proc.Encoding = Encoding.UTF8
                            proc.Package = Package.mkvmerge
                            Dim outFile = files(0).DirAndBase + "_merged.mkv"
                            proc.Arguments = "-o " + outFile.Escape + " """ + files.Join(""" + """) + """"

                            Try
                                proc.Start()
                            Catch ex As Exception
                                g.ShowException(ex)
                                MsgInfo("Manual Merging", "Please merge the files manually with an appropriate tool or visit the support forum].")
                                Throw New AbortException
                            End Try

                            If Not g.FileExists(outFile) Then
                                Log.Write("Error merged output file is missing", outFile)
                                Exit Sub
                            Else
                                Dim showTemplateSelection = (s.ShowTemplateSelection And (ShowTemplateSelectionMode.Always Or ShowTemplateSelectionMode.OpeningMenu)) <> 0
                                Dim showTemplateSelectionTimeout = If(s.ShowTemplateSelection <> ShowTemplateSelectionMode.Never, s.ShowTemplateSelectionTimeout, 0)

                                If showTemplateSelection Then
                                    If LoadTemplateWithSelectionDialog(outFile, showTemplateSelectionTimeout) Then
                                        OpenVideoSourceFile(outFile)
                                    Else
                                        Exit Sub
                                    End If
                                Else
                                    OpenProject(g.LastModifiedTemplate)
                                    OpenVideoSourceFile(outFile)
                                End If
                            End If
                        End Using
                End Select
            End If
        End Using
    End Sub

    <Command("Dialog to open a file batch source.")>
    Sub ShowOpenSourceBatchFilesDialog()
        SetLastModifiedTemplate()
        If p.SourceFile <> "" AndAlso IsSaveCanceled() Then Exit Sub
        If AbortDueToLowDiskSpace() Then Exit Sub

        Using form As New SourceFilesForm()
            form.Text = $"File Batch - {g.DefaultCommands.GetApplicationDetails()}"

            If p.DefaultTargetName = "%source_dir_name%" Then
                p.DefaultTargetName = "%source_name%"
            End If

            If form.ShowDialog() = DialogResult.OK AndAlso form.lb.Items.Count > 0 Then
                Dim showTemplateSelection = (s.ShowTemplateSelection And (ShowTemplateSelectionMode.Always Or ShowTemplateSelectionMode.OpeningMenu)) <> 0
                Dim showTemplateSelectionTimeout = If(s.ShowTemplateSelection <> ShowTemplateSelectionMode.Never, s.ShowTemplateSelectionTimeout, 0)

                If showTemplateSelection Then
                    If LoadTemplateWithSelectionDialog(form.GetFiles(), showTemplateSelectionTimeout) Then
                        For Each filepath In form.GetFiles()
                            AddBatchJob(filepath)
                        Next
                    Else
                        Exit Sub
                    End If
                Else
                    OpenProject(g.LastModifiedTemplate)

                    For Each filepath In form.GetFiles
                        AddBatchJob(filepath)
                    Next
                End If

                ShowJobsDialog()
            End If
        End Using
    End Sub

    <Command("Dialog to open source files.")>
    Sub ShowOpenSourceDialog()
        Using td As New TaskDialog(Of String)
            td.Title = "Select a method for opening a source:"
            td.AddCommand("Single File")
            td.AddCommand("Multiple Files")
            td.AddCommand("Blu-ray Folder")
            td.AddCommand("Merge Files")
            td.AddCommand("File Batch")

            Select Case td.Show
                Case "Single File"
                    ShowOpenSourceSingleFileDialog()
                Case "Multiple Files"
                    ShowOpenSourceMultipleFilesDialog()
                Case "Merge Files"
                    ShowOpenSourceMergeFilesDialog()
                Case "File Batch"
                    ShowOpenSourceBatchFilesDialog()
                Case "Blu-ray Folder"
                    ShowOpenSourceBlurayFolderDialog()
            End Select
        End Using
    End Sub

    Sub OpenEac3toDemuxForm(playlistFolder As String, playlistID As Integer)
        Using form As New eac3toForm(p)
            form.PlaylistFolder = playlistFolder
            form.PlaylistID = playlistID

            Dim workDir = playlistFolder.Parent.Parent
            Dim title = InputBox.Show("Enter a short title used as filename", playlistFolder.Parent.Parent.DirName)
            If title = "" Then Exit Sub

            If p.TempDir <> "" Then
                workDir = p.TempDir.Replace("%source_name%", title).FixDir
            Else
                workDir = Path.Combine(workDir, title)
            End If

            If Not g.IsFixedDrive(workDir) Then
                Using dialog As New FolderBrowserDialog
                    dialog.Description = "Please select a folder for temporary files on a fixed local drive."
                    dialog.SetSelectedPath(s.Storage.GetString("last blu-ray target folder").Parent)

                    If dialog.ShowDialog = DialogResult.OK Then
                        workDir = dialog.SelectedPath
                    Else
                        Exit Sub
                    End If
                End Using
            End If

            form.teTempDir.Text = workDir

            If form.ShowDialog() = DialogResult.OK Then
                If Not Directory.Exists(form.OutputFolder) Then
                    Try
                        Directory.CreateDirectory(form.OutputFolder)
                    Catch ex As Exception
                        MsgError("The temp folder could not be created." + BR2 + form.OutputFolder)
                        Exit Sub
                    End Try
                End If

                If Not g.IsFixedDrive(form.OutputFolder) Then
                    MsgError("Only fixed local drives are supported as temp dir.")
                    Exit Sub
                End If

                Try
                    Dim di As New DriveInfo(form.OutputFolder)

                    If di.AvailableFreeSpace / PrefixedSize(3).Factor < s.MinimumDiskSpace Then
                        Using td As New TaskDialog(Of String)
                            td.Title = "Low Disk Space"
                            td.Content = $"The target drive {Path.GetPathRoot(p.TargetFile)} has only " +
                                         $"{(di.AvailableFreeSpace / PrefixedSize(3).Factor):f2} {PrefixedSize(3).Unit} free disk space."
                            td.Icon = TaskIcon.Warning
                            td.AddButton("Continue")
                            td.AddButton("Abort")

                            If td.Show <> "Continue" Then
                                Exit Sub
                            End If
                        End Using
                    End If

                    Using pr As New Proc
                        pr.Header = "Demux M2TS"
                        pr.TrimChars = {"-"c, " "c}
                        pr.SkipStrings = {"analyze: ", "process: "}
                        pr.Package = Package.eac3to
                        Dim outFiles As New List(Of String)
                        pr.Process.StartInfo.Arguments = form.GetArgs(playlistFolder.Escape + " " & playlistID & ")", title, outFiles)
                        pr.OutputFiles = outFiles

                        Try
                            pr.Start()
                        Catch ex As AbortException
                            Exit Sub
                        Catch ex As Exception
                            g.ShowException(ex)
                            Exit Sub
                        End Try
                    End Using

                    s.Storage.SetString("last blu-ray target folder", form.OutputFolder)
                    Dim fs = Path.Combine(form.OutputFolder, title + "." + form.cbVideoOutput.Text.ToLowerInvariant)

                    If File.Exists(fs) Then
                        p.TempDir = form.OutputFolder
                        OpenVideoSourceFile(fs)
                    End If
                Catch
                End Try
            End If
        End Using
    End Sub

    Function GetSourceFilesFromDir(path As String) As String()
        For Each i In FileTypes.Video
            Dim files = Directory.GetFiles(path, "*." + i)
            Array.Sort(files)

            If files.Length > 0 Then
                Return files
            End If
        Next
    End Function

    Protected Overrides Sub WndProc(ByRef m As Message)
        MyBase.WndProc(m)
    End Sub

    Sub tbSource_DoubleClick() Handles tbSourceFile.DoubleClick
        SetLastModifiedTemplate()

        Using dialog As New OpenFileDialog
            dialog.SetFilter(FileTypes.Video)
            dialog.Multiselect = True
            dialog.SetInitDir(s.LastSourceDir)

            If dialog.ShowDialog() = DialogResult.OK Then
                Refresh()

                Dim files = dialog.FileNames
                files.Sort()

                If Not files.NothingOrEmpty Then
                    Dim showTemplateSelection = (s.ShowTemplateSelection And (ShowTemplateSelectionMode.Always Or ShowTemplateSelectionMode.OpeningMenu)) <> 0

                    If p.SourceFile <> "" AndAlso IsSaveCanceled() Then Exit Sub
                    If Not showTemplateSelection Then OpenProject(g.LastModifiedTemplate)
                    BeginInvoke(Sub() OpenAnyFile(files.ToList, showTemplateSelection))
                End If
            End If
        End Using
    End Sub

    Function GetNewAudioProfile(currentProfile As AudioProfile) As AudioProfile
        Dim sb As New SelectionBox(Of AudioProfile) With {
            .Title = "New Profile",
            .Text = "Please select a profile."
        }

        If currentProfile IsNot Nothing Then
            sb.AddItem("Current Project", currentProfile)
        End If

        For Each i In AudioProfile.GetDefaults()
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK Then
            Return sb.SelectedValue
        End If
    End Function

    Sub lTip_Click() Handles laTip.Click
        If AssistantClickAction IsNot Nothing Then
            AssistantClickAction.Invoke()
            Assistant()
        End If
    End Sub

    Sub UpdateTargetParameters(proj As Project)
        If proj Is Nothing Then Exit Sub
        If proj.Script Is Nothing Then Exit Sub

        Dim info = proj.Script.GetInfo()
        Dim frameCount = 0
        Dim frameRate = 0D
        Dim seconds = 0

        If info.FrameRate > 0 Then
            frameCount = info.FrameCount
            frameRate = info.FrameRate
            seconds = CInt(frameCount / frameRate)
        Else
        End If

        UpdateTargetParameters(seconds, frameCount, frameRate)
    End Sub

    Sub UpdateTargetParameters(seconds As Integer, frames As Integer, frameRate As Double)
        p.TargetSeconds = seconds
        p.TargetFrames = frames
        p.TargetFrameRate = frameRate

        UpdateSizeOrBitrate()
    End Sub

    Sub pEncoder_MouseLeave() Handles pnEncoder.MouseLeave
        Assistant()
    End Sub

    Sub ShowAudioTip(ap As AudioProfile)
        If TypeOf ap Is GUIAudioProfile Then
            Dim gap = DirectCast(ap, GUIAudioProfile)

            If ap.Decoder = AudioDecoderMode.ffmpeg AndAlso gap.GetEncoder = GuiAudioEncoder.ffmpeg Then
                Documentation.ShowTip(
                    "Please note that defining an audio decoder will always result in a separate " +
                    "decoding step, which is usually not necessary if the decoder and encoder are identical, " +
                    "it's a waste of time and disk space.")
            End If
        End If
    End Sub

    <Command("Dialog to manage audio profiles.")>
    Sub ShowAudioProfilesDialog(<DispName("Track Number")> number As Integer)
        Dim form = New ProfilesForm("Audio Profiles", s.AudioProfiles, Sub(profile) g.LoadAudioProfile(profile, number), Function() GetNewAudioProfile(p.AudioTracks(number).AudioProfile), AddressOf AudioProfile.GetDefaults)

        form.ShowDialog()
        form.Dispose()

        UpdateAudioMenus()
        PopulateProfileMenu(DynamicMenuItemID.AudioProfiles)
    End Sub

    Sub UpdateAudioMenus()
        If p.AudioTracksAvailable < 1 Then Exit Sub
        Dim nameControls = tlpAudio?.Controls?.OfType(Of AudioNameButtonLabel)?.Where(Function(x) x.GetType() Is GetType(AudioNameButtonLabel))
        If nameControls Is Nothing Then Exit Sub
        If nameControls.Count < 1 Then Exit Sub

        For i = 0 To nameControls.Count - 1
            Dim index = i
            Dim control = nameControls(index)

            control.ContextMenuStripEx = If(control.ContextMenuStripEx, New ContextMenuStripEx(components))

            control.ContextMenuStripEx.Items.ClearAndDisplose()

            g.PopulateProfileMenu(control.ContextMenuStripEx.Items, s.AudioProfiles, Sub() ShowAudioProfilesDialog(index), Sub(profile) g.LoadAudioProfile(profile, index))
        Next
    End Sub

    Sub AviSynthListView_ScriptChanged() Handles FiltersListView.Changed
        If Not IsLoading AndAlso Not FiltersListView.IsLoading Then
            If g.IsValidSource(False) Then
                UpdateSourceParameters()
                UpdateTargetParameters(p)
            Else
                Assistant()
            End If
        End If
    End Sub

    Sub UpdateFilters()
        FiltersListView.Load()

        If g.IsValidSource(False) Then
            UpdateTargetParameters(p)
        End If
    End Sub

    Sub AviSynthListView_DoubleClick() Handles FiltersListView.DoubleClick
        FiltersListView.ShowCodeEditor()
    End Sub

    Sub gbFilters_MenuClick() Handles lgbFilters.LinkClick
        FiltersListView.ContextMenuStrip.Show(lgbFilters, 0, 16)
    End Sub

    Sub gbEncoder_LinkClick() Handles lgbEncoder.LinkClick
        EncoderMenu.Items.ClearAndDisplose
        g.PopulateProfileMenu(EncoderMenu.Items, s.VideoEncoderProfiles, AddressOf ShowEncoderProfilesDialog, AddressOf g.LoadVideoEncoder)
        EncoderMenu.Show(lgbEncoder, 0, 16)
    End Sub

    Sub llSize_Click() Handles blFilesize.Click
        UpdateSizeMenu()
        SizeContextMenuStrip.Show(blFilesize, 0, 16)
    End Sub

    Sub llContainer_Click() Handles llMuxer.Click
        ContainerMenu.Items.ClearAndDisplose
        g.PopulateProfileMenu(ContainerMenu.Items, s.MuxerProfiles, AddressOf ShowMuxerProfilesDialog, AddressOf p.VideoEncoder.LoadMuxer)
        ContainerMenu.Show(llMuxer, 0, 16)
    End Sub

    Sub gbResize_LinkClick() Handles lgbResize.LinkClick
        Dim cms = TextCustomMenu.GetMenu(s.TargetImageSizeMenu, lgbResize.Label, components, AddressOf TargetImageMenuClick)
        cms.Add("-")
        cms.Add("Image Options...", Sub() ShowOptionsDialog("Image"))
        cms.Add("Edit Menu...", Sub() s.TargetImageSizeMenu = TextCustomMenu.EditMenu(s.TargetImageSizeMenu, ApplicationSettings.GetDefaultTargetImageSizeMenu, Me))
        cms.Show(lgbResize, 0, lgbResize.Label.Height)
    End Sub

    Sub blSourceParText_Click(sender As Object, e As EventArgs) Handles blSourceParText.Click
        Dim menu = TextCustomMenu.GetMenu(s.ParMenu, blSourceParText, components, AddressOf SourceParMenuClick)
        menu.Add("-")
        menu.Items.Add(New MenuItemEx("Edit Menu...", Sub() s.ParMenu = TextCustomMenu.EditMenu(s.ParMenu, ApplicationSettings.GetParMenu, Me)))
        menu.Show(blSourceParText, 0, blSourceParText.Height)
    End Sub

    Sub blSourceDarText_Click(sender As Object, e As EventArgs) Handles blSourceDarText.Click
        Dim menu = TextCustomMenu.GetMenu(s.DarMenu, blSourceDarText, components, AddressOf SourceDarMenuClick)
        menu.Add("-")
        menu.Items.Add(New MenuItemEx("Edit Menu...", Sub() s.DarMenu = TextCustomMenu.EditMenu(s.DarMenu, ApplicationSettings.GetDarMenu, Me)))
        menu.Show(blSourceDarText, 0, blSourceDarText.Height)
    End Sub

    Sub blTargetDarText_Click(sender As Object, e As EventArgs) Handles blTargetDarText.Click
        Dim menu = TextCustomMenu.GetMenu(s.DarMenu, blTargetDarText, components, AddressOf TargetDarMenuClick)
        menu.Add("-")
        menu.Items.Add(New MenuItemEx("Edit Menu...", Sub() s.DarMenu = TextCustomMenu.EditMenu(s.DarMenu, ApplicationSettings.GetDarMenu, Me)))
        menu.Show(blTargetDarText, 0, blTargetDarText.Height)
    End Sub

    Sub blTargetParText_Click(sender As Object, e As EventArgs) Handles blTargetParText.Click
        Dim menu = TextCustomMenu.GetMenu(s.ParMenu, blTargetParText, components, AddressOf TargetParMenuClick)
        menu.Add("-")
        menu.Items.Add(New MenuItemEx("Edit Menu...", Sub() s.ParMenu = TextCustomMenu.EditMenu(s.ParMenu, ApplicationSettings.GetParMenu, Me)))
        menu.Show(blTargetParText, 0, blTargetParText.Height)
    End Sub

    Sub SourceDarMenuClick(value As String)
        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)

        If tup.Cancel Then
            Exit Sub
        End If

        p.CustomSourcePAR = ""
        p.CustomSourceDAR = tup.Value

        If Calc.ParseCustomAR(tup.Value, 0, 0).X = 0 Then
            p.CustomSourceDAR = ""
        End If

        Assistant()
    End Sub

    Sub SourceParMenuClick(value As String)
        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)

        If tup.Cancel Then
            Exit Sub
        End If

        p.CustomSourcePAR = tup.Value
        p.CustomSourceDAR = ""

        If value <> "force" AndAlso Calc.ParseCustomAR(tup.Value, 0, 0).X = 0 Then
            p.CustomSourcePAR = ""
        End If

        Assistant()
    End Sub

    Sub TargetDarMenuClick(value As String)
        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)

        If tup.Cancel Then
            Exit Sub
        End If

        p.CustomTargetPAR = ""
        p.CustomTargetDAR = tup.Value

        If Calc.ParseCustomAR(tup.Value, 0, 0).X = 0 Then
            p.CustomTargetDAR = ""
        End If

        Assistant()
    End Sub

    Sub TargetParMenuClick(value As String)
        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)

        If tup.Cancel Then
            Exit Sub
        End If

        p.CustomTargetPAR = tup.Value
        p.CustomTargetDAR = ""

        If value <> "force" AndAlso Calc.ParseCustomAR(tup.Value, 0, 0).X = 0 Then
            p.CustomTargetPAR = ""
        End If

        Assistant()
    End Sub

    Sub TargetImageMenuClick(value As String)
        g.EnableFilter("Resize")

        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)

        If tup.Cancel Then
            Exit Sub
        End If

        value = tup.Value

        If value.IsInt Then
            SetTargetImageSizeByPixel(CInt(value))
            Exit Sub
        End If

        If value?.Contains("x") Then
            Dim a = value.SplitNoEmptyAndWhiteSpace("x")

            If a.Length = 2 AndAlso a(0).IsInt AndAlso a(1).IsInt Then
                SetTargetImageSize(a(0).ToInt, a(1).ToInt)
                Exit Sub
            End If
        End If

        MsgWarn("Invalid format")
    End Sub

    Sub SetAutoAspectRatio(isAnamorphic As Boolean)
        p.SourceAnamorphic = isAnamorphic
        p.CustomSourcePAR = ""
        p.CustomSourceDAR = ""

        If p.Script.IsFilterActive("Resize)") Then
            SetTargetImageSize(p.TargetWidth, 0)
        End If

        Assistant()
    End Sub

    Sub SetTip()
        With TipProvider
            .SetTip("Target Display Aspect Ratio", lDAR, blTargetDarText)
            .SetTip("Target Pixel Aspect Ratio", lPAR, blTargetParText)
            .SetTip("Target Storage Aspect Ratio", lSAR, lSarText)
            .SetTip("Imagesize in pixel (width x height)", lPixel, lPixelText)
            .SetTip("Zoom factor between source and target image size", lZoom, lZoomText)
            .SetTip("Press Ctrl while using the slider to resize anamorphic.", tbResize)
            .SetTip("Aspect Ratio Error", lAspectRatioError, lAspectRatioErrorText)
            .SetTip("Cropped image size", lCrop, lCropText)
            .SetTip("Cropped sides in format: Left, Right / Top, Bottom", blCropText, lCrop2)
            .SetTip("Source Display Aspect Ratio", lSourceDar, blSourceDarText)
            .SetTip("Source Pixel Aspect Ratio", lSourcePAR, blSourceParText)
            .SetTip("Target Video Bitrate in Kbps (Up/Down key)", tbBitrate, laBitrate)
            .SetTip("Target Image Width in pixel (Up/Down key)", tbTargetWidth, lTargetWidth)
            .SetTip("Target Image Height in pixel (Up/Down key)", tbTargetHeight, lTargetHeight)
            .SetTip("Target File Size (Up/Down key)", tbTargetSize)
            .SetTip("Source file", tbSourceFile)
            .SetTip("Target file", tbTargetFile)
            .SetTip("Shows a menu with Container/Muxer profiles", llMuxer)
            .SetTip("Shows a menu with video encoder profiles", lgbEncoder.Label)
            .SetTip("Shows a menu with AviSynth filter options", lgbFilters.Label)
        End With
    End Sub

    Sub UpdateSourceParameters()
        If p.SourceScript Is Nothing Then Exit Sub

        Try
            Dim info = p.SourceScript.GetInfo

            p.SourceWidth = info.Width
            p.SourceHeight = info.Height
            p.SourceSeconds = CInt(info.FrameCount / info.FrameRate)
            p.SourceFrameRate = info.FrameRate
            p.SourceFrames = info.FrameCount
        Catch ex As Exception
            MsgError("Source filter returned invalid parameters", p.SourceScript.GetFullScript)
            Throw New AbortException()
        End Try
    End Sub

    Sub tbSource_TextChanged(sender As Object, e As EventArgs) Handles tbSourceFile.TextChanged
        If Not BlockSourceTextBoxTextChanged Then
            If File.Exists(tbSourceFile.Text) Then
                OpenVideoSourceFile(tbSourceFile.Text)
            End If
        End If
    End Sub

    Sub gbTarget_LinkClick() Handles lgbTarget.LinkClick
        UpdateTargetFileMenu()
        TargetFileMenu.Show(lgbTarget, 0, lgbTarget.Label.Height)
    End Sub

    Sub gbSource_MenuClick() Handles lgbSource.LinkClick
        UpdateSourceFileMenu()

        If File.Exists(p.SourceFile) Then
            SourceFileMenu.Show(lgbSource, 0, lgbSource.Label.Height)
        Else
            ShowOpenSourceDialog()
        End If
    End Sub

    Sub tbTargetFile_DoubleClick() Handles tbTargetFile.DoubleClick
        Using dialog As New SaveFileDialog
            dialog.FileName = p.TargetFile.Base
            dialog.SetInitDir(p.TargetFile.Dir)

            If dialog.ShowDialog() = DialogResult.OK Then
                Dim ext = p.VideoEncoder.Muxer.OutputExtFull
                p.TargetFile = If(dialog.FileName.Ext = ext, dialog.FileName, dialog.FileName + ext)
            End If
        End Using
    End Sub

    Sub tbTargetFile_MouseDown(sender As Object, e As MouseEventArgs) Handles tbTargetFile.MouseDown
        If e.Button = MouseButtons.Right Then
            UpdateTargetFileMenu()
        End If
    End Sub

    Sub tbSourceFile_MouseDown(sender As Object, e As MouseEventArgs) Handles tbSourceFile.MouseDown
        If e.Button = MouseButtons.Right Then
            UpdateSourceFileMenu()
        End If
    End Sub

    Sub UpdateAudioFileMenu(audioTrack As AudioTrack, a As Action)
        Dim cms = DirectCast(audioTrack.TextEdit.TextBox.ContextMenuStrip, ContextMenuStripEx)
        Dim ap = audioTrack.AudioProfile
        Dim te = audioTrack.TextEdit
        Dim exist = File.Exists(ap.File)
        Dim convertName = Function(name As String)
                              Return name.Replace(" | ", " - ")
                          End Function

        cms.Items.ClearAndDisplose

        If ap.Streams IsNot Nothing AndAlso ap.Streams.Count > 0 Then
            For Each i In ap.Streams
                Dim temp = i

                Dim menuAction = Sub()
                                     If ap.File <> p.LastOriginalSourceFile Then
                                         te.Text = p.LastOriginalSourceFile
                                     End If

                                     te.Text = temp.Name + " (" + ap.File.Ext + ")"
                                     ap.Stream = temp

                                     audioTrack.LanguageLabel.Refresh()
                                     UpdateSizeOrBitrate()
                                 End Sub

                If ap.Streams.Count > 10 Then
                    cms.Add("Streams | " + convertName(i.Name), menuAction)
                Else
                    cms.Add(convertName(i.Name), menuAction)
                End If
            Next

            cms.Items.Add("-")
        End If

        If p.SourceFile.Ext = "avs" OrElse p.Script.GetScript.ToLowerInvariant.Contains("audiodub(") Then
            cms.Add(p.Script.Path.FileName, Sub() te.Text = p.Script.Path)
        End If

        If p.TempDir <> "" AndAlso Directory.Exists(p.TempDir) Then
            Dim audioFiles = Directory.GetFiles(p.TempDir).Where(
                Function(audioPath) FileTypes.Audio.Contains(audioPath.Ext)).ToList

            audioFiles.Sort(New StringLogicalComparer)

            If audioFiles.Count > 0 Then
                For Each i In audioFiles
                    Dim temp = i

                    If audioFiles.Count > 10 Then
                        cms.Add("Files | " + i.FileName, Sub() te.Text = temp)
                    Else
                        cms.Add(i.FileName, Sub() te.Text = temp)
                    End If
                Next

                cms.Items.Add("-")
            End If
        End If

        Dim moreFilesAction = Sub()
                                  s.Storage.SetInt("last selected muxer tab", 1)
                                  p.VideoEncoder.OpenMuxerConfigDialog()
                              End Sub

        cms.Add("Open File", a, "Change the audio source file.").SetImage(Symbol.OpenFile)
        cms.Add("Add more files...", moreFilesAction, exist)
        cms.Add("Play", Sub() g.PlayAudio(ap), exist, "Plays the audio source file with a media player.").SetImage(Symbol.Play)
        cms.Add("Media Info...", Sub() g.DefaultCommands.ShowMediaInfo(ap.File), exist, "Show MediaInfo for the audio source file.").SetImage(Symbol.Info)
        cms.Add("Explore", Sub() g.SelectFileWithExplorer(ap.File), exist, "Open the audio source file directory with File Explorer.").SetImage(Symbol.FileExplorer)
        cms.Add("-")
        cms.Add("Execute", Sub() ExecuteAudio(ap), exist, "Processes the audio profile.").SetImage(Symbol.LightningBolt)
        cms.Add("Execute All", Sub() ExecuteAllAudio(), Function() If(p.AudioTracks?.Where(Function(x) x.TextEdit.Text <> "" AndAlso TypeOf x.AudioProfile IsNot NullAudioProfile)?.Any(), True, False), "Processes all audio profiles.")
        cms.Add("-")
        cms.Add("Copy Path", Sub() Clipboard.SetText(ap.File), te.Text <> "")
        cms.Add("Copy Selection", Sub() Clipboard.SetText(te.TextBox.SelectedText), te.Text <> "").SetImage(Symbol.Copy)
        cms.Add("Paste", Sub() te.TextBox.Paste(), Clipboard.GetText.Trim <> "").SetImage(Symbol.Paste)
        cms.Add("-")
        cms.Add("Remove", Sub() audioTrack.Remove(), te.Text <> "", "Remove audio file").SetImage(Symbol.Remove)
    End Sub

    Sub ExecuteAudio(ap As AudioProfile)
        If MsgQuestion("Confirm to process the track.") = DialogResult.OK Then
            Dim cleanTemp = False

            Try
                If p.TempDir = "" Then
                    p.TempDir = ap.File.Dir
                    cleanTemp = True
                End If

                ap = ObjectHelp.GetCopy(Of AudioProfile)(ap)
                Audio.Process(ap)
                ap.Encode()
            Catch
            Finally
                If cleanTemp Then p.TempDir = ""
            End Try
        End If
    End Sub

    Sub ExecuteAllAudio()
        If MsgQuestion("Confirm to process ALL audio tracks.") = DialogResult.OK Then
            For Each track In p.AudioTracks
                Dim cleanTemp = False

                Try
                    If track.TextEdit.Text = "" Then Continue For
                    If TypeOf track.AudioProfile Is NullAudioProfile Then Continue For
                    If p.TempDir = "" Then
                        p.TempDir = track.AudioProfile.File.Dir()
                        cleanTemp = True
                    End If

                    Dim ap = ObjectHelp.GetCopy(Of AudioProfile)(track.AudioProfile)
                    Audio.Process(ap)
                    ap.Encode()
                Catch
                Finally
                    If cleanTemp Then p.TempDir = ""
                End Try
            Next
        End If
    End Sub

    Sub UpdateTargetFileMenu()
        TargetFileMenu.Items.ClearAndDisplose
        TargetFileMenu.Add("Browse File...", AddressOf tbTargetFile_DoubleClick, File.Exists(p.SourceFile), "Change the path of the target file.")
        TargetFileMenu.Add("Play", Sub() g.Play(p.TargetFile), File.Exists(p.TargetFile), "Play the target file.").SetImage(Symbol.Play)
        TargetFileMenu.Add("Media Info...", Sub() g.DefaultCommands.ShowMediaInfo(p.TargetFile), File.Exists(p.TargetFile), "Show MediaInfo for the target file.").SetImage(Symbol.Info)
        TargetFileMenu.Add("Explore...", Sub() g.SelectFileWithExplorer(p.TargetFile), Directory.Exists(p.TargetFile.Dir), "Open the target file directory with File Explorer.").SetImage(Symbol.FileExplorer)
        TargetFileMenu.Add("-")
        TargetFileMenu.Add("Copy", Sub() tbTargetFile.TextBox.Copy(), tbTargetFile.Text <> "").SetImage(Symbol.Copy)
        TargetFileMenu.Add("Paste", Sub() tbTargetFile.TextBox.Paste(), Clipboard.GetText.Trim <> "" AndAlso File.Exists(p.SourceFile)).SetImage(Symbol.Paste)
    End Sub

    Sub UpdateSourceFileMenu()
        SourceFileMenu.Items.ClearAndDisplose
        Dim isIndex = FileTypes.VideoIndex.Contains(p.LastOriginalSourceFile.Ext)

        SourceFileMenu.Add("Open...", AddressOf ShowOpenSourceDialog, "Open source files").SetImage(Symbol.OpenFile)
        SourceFileMenu.Add("Play", Sub() g.Play(p.LastOriginalSourceFile), File.Exists(p.LastOriginalSourceFile) AndAlso Not isIndex, "Play the source file.").SetImage(Symbol.Play)
        SourceFileMenu.Add("Media Info...", Sub() g.DefaultCommands.ShowMediaInfo(p.LastOriginalSourceFile), File.Exists(p.LastOriginalSourceFile) AndAlso Not isIndex, "Show MediaInfo for the source file.").SetImage(Symbol.Info)
        SourceFileMenu.Add("Explore...", Sub() g.SelectFileWithExplorer(p.SourceFile), File.Exists(p.SourceFile), "Open the source file directory with File Explorer.").SetImage(Symbol.FileExplorer)
        SourceFileMenu.Items.Add("-")
        SourceFileMenu.Add("Copy", Sub() tbSourceFile.TextBox.Copy(), tbSourceFile.Text <> "", "Copies the selected text to the clipboard.").SetImage(Symbol.Copy)
        SourceFileMenu.Add("Paste", Sub() tbSourceFile.TextBox.Paste(), Clipboard.GetText.Trim <> "", "Copies the full source file path to the clipboard.").SetImage(Symbol.Paste)
    End Sub

    Protected Overrides Sub OnDragEnter(e As DragEventArgs)
        MyBase.OnDragEnter(e)

        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Protected Overrides Sub OnDragDrop(e As DragEventArgs)
        MyBase.OnDragDrop(e)
        SetLastModifiedTemplate()

        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            Dim showTemplateSelection = (s.ShowTemplateSelection And (ShowTemplateSelectionMode.Always Or ShowTemplateSelectionMode.DragDrop)) <> 0

            If p.SourceFile <> "" AndAlso IsSaveCanceled() Then Exit Sub
            If Not showTemplateSelection Then OpenProject(g.LastModifiedTemplate)
            BeginInvoke(Sub() OpenAnyFile(files.ToList, showTemplateSelection))
        End If
    End Sub

    Protected Overrides Sub OnActivated(e As EventArgs)
        MyBase.OnActivated(e)
        UpdateNextButton()

        If Not FrameServerHelp.IsAviSynthPortable AndAlso FrameServerHelp.GetAviSynthInstallPath = "" Then
            MsgError($"AviSynth installation not found,{BR}using portable mode instead.")
            s.AviSynthMode = FrameServerMode.Portable
        End If

        If Not FrameServerHelp.IsVapourSynthPortable AndAlso FrameServerHelp.GetVapourSynthInstallPath = "" Then
            MsgError($"VapourSynth installation not found,{BR}using portable mode instead.")
            s.VapourSynthMode = FrameServerMode.Portable
        End If

        ProcController.SetLastActivation()

        BeginInvoke(New Action(Sub()
                                   Application.DoEvents()
                                   Assistant()
                                   UpdateScriptsMenuAsync()
                               End Sub))
    End Sub

    Protected Overrides Sub OnLoad(args As EventArgs)
        MyBase.OnLoad(args)
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        UpdateDynamicMenuAsync()
        UpdateRecentProjectsMenu()
        UpdateTemplatesMenuAsync()
        IsLoading = False
        Refresh()
        CheckForWindows7()
        g.CheckForLongPathSupport()
        g.PreloadValuesAsync()
        ShowChangelog(False)
        ProcessCommandLine(Environment.CommandLine)
        StaxRipUpdate.SetFirstRunOnCurrentVersion()
        StaxRipUpdate.ShowUpdateQuestion()
        StaxRipUpdate.CheckForUpdateAsync(False, Environment.Is64BitProcess)
        g.RunTask(AddressOf g.LoadPowerShellScripts)
        'Text = $"{Size.Width}x{Size.Height}"
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        'Text = $"{Size.Width}x{Size.Height}"
    End Sub

    Protected Overrides Sub OnFormClosing(args As FormClosingEventArgs)
        MyBase.OnFormClosing(args)

        If IsSaveCanceled() Then
            args.Cancel = True
        End If
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)

        If g.ProcForm IsNot Nothing Then
            If g.ProcForm.InvokeRequired Then
                g.ProcForm.Invoke(Sub() g.ProcForm.Close())
            Else
                g.ProcForm.Close()
            End If
        End If
        g.SaveSettings()
        g.RaiseAppEvent(ApplicationEvent.ApplicationExit)
    End Sub

    Protected Overrides Sub OnDeactivate(e As EventArgs)
        MyBase.OnDeactivate(e)
        UpdateNextButton()
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)
        UpdateNextButton()
    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        MyBase.OnKeyUp(e)
        UpdateNextButton()
    End Sub

    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            If ProcController.BlockActivation Then
                ProcController.BlockActivation = False

                If s.PreventFocusStealUntil >= 0 AndAlso ProcController.SecondsSinceLastActivation <= s.PreventFocusStealUntil Then
                    Return True
                ElseIf s.PreventFocusStealAfter >= 0 AndAlso ProcController.SecondsSinceLastActivation >= s.PreventFocusStealAfter Then
                    Return True
                End If
            End If

            Return MyBase.ShowWithoutActivation
        End Get
    End Property

    Sub UpdateTargetSizeLabel()
        Me.blFilesize.Text = $"Size in {PrefixedSize(2).Unit}:"
    End Sub

    Sub UpdateNextButton()
        If AssistantPassed AndAlso CanIgnoreTip Then
            Dim toTop = ModifierKeys.HasFlag(Keys.Shift)
            toTop = If(s.InvertShiftKeyOnNextButton, Not toTop, toTop)
            Dim hideList = ModifierKeys.HasFlag(Keys.Control)
            hideList = If(s.InvertCtrlKeyOnNextButton, Not hideList, hideList)

            Dim txt = "Add job"
            txt += If(toTop, " to top", "")
            txt += If(hideList, $"{BR}w/o showing list", "")

            bnNext.Text = txt
        Else
            bnNext.Text = "Accept / Next"
        End If
    End Sub

    Sub bnNext_Click(sender As Object, e As EventArgs) Handles bnNext.Click
        Dim toTop = ModifierKeys.HasFlag(Keys.Shift)
        toTop = If(s.InvertShiftKeyOnNextButton, Not toTop, toTop)
        Dim hideList = ModifierKeys.HasFlag(Keys.Control)
        hideList = If(s.InvertCtrlKeyOnNextButton, Not hideList, hideList)

        AddJob(Not hideList, If(toTop, 0, -1))
    End Sub

    Sub bnNext_MouseDown(sender As Object, e As MouseEventArgs) Handles bnNext.MouseDown
        If e.Button = MouseButtons.Right AndAlso AssistantPassed AndAlso CanIgnoreTip Then
            NextContextMenuStrip.Show(bnNext, New Point(bnNext.Width, 0), ToolStripDropDownDirection.AboveLeft)
        End If
    End Sub

    Sub SetFormBoundaries(ateHeight As Integer)
        Dim h = MainMenuStrip.Height + lgbSource.Height + lgbFilters.Height + ateHeight * p.AudioTracksAvailable + (gbAssistant.Height << 1) - Font.Height \ 3
        MinimumSize = New Size(Font.Height * 48, CInt(h))
        MaximumSize = New Size(Font.Height * 75, CInt(h))
    End Sub

    Sub CheckForWindows7()
        If s.ShowWindows7Warning AndAlso OSVersion.VersionInfo.dwMajorVersion = 7 Then
            MsgWarn("Compatibility problem!", "Whereas Windows 7 is supported by StaxRip itself, some tools don't do it anymore. This can cause tools denying to work correctly or at whole. Currently those tools are 'MKVToolNix (mkvmerge)' and 'Python', which you have to downgrade or try to avoid. For further help join our Discord server.")
        End If
        s.ShowWindows7Warning = False
    End Sub

    <Command("Shows the latest changes.")>
    Sub ShowChangelog(force As Boolean)
        Dim appDetails = g.DefaultCommands.GetApplicationDetails(True, True, False)
        If Not force AndAlso s.ShowChangelog = appDetails Then Exit Sub

        Dim currentVersion = Assembly.GetExecutingAssembly.GetName.Version
        Dim lastChangelogVersionMatch = Regex.Match(s.ShowChangelog, "\d+\.\d+(?:\.\d+)*")
        Dim lastChangelogVersion = If(lastChangelogVersionMatch.Success, New Version(lastChangelogVersionMatch.Value), Nothing)
        Dim readoutVersion As Version = Nothing

        If Not force AndAlso (currentVersion.Minor Mod 2) = 1 Then Exit Sub

        Using stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("StaxRip.Changelog.md")
            Using reader As New StreamReader(stream)
                Dim sb As New StringBuilder()
                Dim sbNotEmpty = False
                Dim relevant = False
                Dim abortAfter = False
                Dim versions = 0

                Do While Not reader.EndOfStream
                    Dim line = reader.ReadLine()
                    If line Like "========*" Then Continue Do

                    Dim lines = Regex.Matches(sb.ToString(), Environment.NewLine).Count
                    Dim readoutVersionMatch = Regex.Match(line, "^(v(\d+\.\d+(?:\.\d+)*(?:-(RC\d+))?))\W+\(.*\)")
                    readoutVersion = If(readoutVersionMatch.Success, New Version(readoutVersionMatch.Groups(2).Value), readoutVersion)

                    If readoutVersionMatch.Success Then
                        If readoutVersion.Minor < currentVersion.Minor Then Exit Do
                        If readoutVersion.Minor > currentVersion.Minor Then Continue Do
                        If readoutVersion.Build > currentVersion.Build Then Continue Do

                        If relevant Then
                            If (lastChangelogVersionMatch.Success AndAlso readoutVersion.Build > Math.Max(lastChangelogVersion.Build, 0)) OrElse lines < 40 Then
                                versions += 1

                                sb.AppendLine()
                                sb.AppendLine(line)
                                sb.AppendLine("-------------------------")

                                If currentVersion.Build > 0 AndAlso readoutVersion.Build = 0 AndAlso lines > 25 Then
                                    sb.AppendLine("---- Hidden because of the length of this report ----")
                                    relevant = False
                                End If

                                Continue Do
                            Else
                                Exit Do
                            End If
                        End If

                        relevant = True
                        Continue Do
                    End If

                    If abortAfter AndAlso String.IsNullOrWhiteSpace(line) Then Exit Do
                    If Not relevant Then Continue Do
                    If String.IsNullOrWhiteSpace(line) Then
                        If lines > 0 AndAlso readoutVersion.Build = currentVersion.Build AndAlso NOT sb.ToString().EndsWith(BR2) Then sb.AppendLine()
                        Continue Do
                    End If
                    If versions > 1 AndAlso currentVersion.Build > 0 AndAlso line.StartsWithEx("- Update ") Then
                        sb.AppendLine("---- Tool and Plugin updates are not shown! ----")
                        abortAfter = True
                        relevant = False
                    ElseIf lines > 35 AndAlso line.StartsWithEx("- Update ") Then
                        sb.AppendLine("---- Tool and Plugin updates are not shown! ----")
                        abortAfter = True
                        relevant = False
                    End If
                    If Not relevant Then Continue Do

                    line = Regex.Replace(line, "(?<=\W\(\[#\d+\])(\(/\.\./\.\./\w+/\d+\))(?=\)(?>,|\)|$))", "", RegexOptions.CultureInvariant)
                    line = Regex.Replace(line, "(?<=^| ) (?= |-)", "  ", RegexOptions.CultureInvariant)
                    sb.AppendLine(line)
                    sbNotEmpty = True
                Loop

                If sbNotEmpty Then
                    sb.AppendLine(BR)

                    Using td As New TaskDialog(Of String)()
                        td.Title = $"What's new in {appDetails}:"
                        td.Icon = TaskIcon.Shield
                        td.Content = sb.ToString()

                        td.AddCommand("OK")

                        Dim answer = td.Show

                        s.ShowChangelog = appDetails
                    End Using
                End If
            End Using
        End Using
    End Sub
End Class
