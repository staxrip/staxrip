
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Globalization
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks

Imports StaxRip.UI

Imports VB6 = Microsoft.VisualBasic

Public Class MainForm
    Inherits FormBase

#Region " Designer "

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            components?.Dispose()
        End If

        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    Public WithEvents tbAudioFile0 As TextEdit
    Public WithEvents tbAudioFile1 As TextEdit
    Public WithEvents llEditAudio1 As ButtonLabel
    Public WithEvents llEditAudio0 As ButtonLabel
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
    Public WithEvents tbResize As System.Windows.Forms.TrackBar
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
    Public WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Public WithEvents llAudioProfile0 As ButtonLabel
    Public WithEvents llAudioProfile1 As ButtonLabel
    Public WithEvents pnEncoder As System.Windows.Forms.Panel
    Public WithEvents FiltersListView As StaxRip.FiltersListView
    Public WithEvents blFilesize As ButtonLabel
    Public WithEvents llMuxer As ButtonLabel
    Public WithEvents lPAR As StaxRip.UI.LabelEx
    Public WithEvents blTargetParText As ButtonLabel
    Public WithEvents lAspectRatioError As LabelEx
    Public WithEvents lAspectRatioErrorText As LabelEx
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
        Me.llEditAudio0 = New StaxRip.UI.ButtonLabel()
        Me.gbAssistant = New StaxRip.UI.GroupBoxEx()
        Me.tlpAssistant = New System.Windows.Forms.TableLayoutPanel()
        Me.laTip = New StaxRip.UI.LabelEx()
        Me.llEditAudio1 = New StaxRip.UI.ButtonLabel()
        Me.gbAudio = New StaxRip.UI.GroupBoxEx()
        Me.tlpAudio = New System.Windows.Forms.TableLayoutPanel()
        Me.tbAudioFile0 = New StaxRip.UI.TextEdit()
        Me.tbAudioFile1 = New StaxRip.UI.TextEdit()
        Me.llAudioProfile1 = New StaxRip.UI.ButtonLabel()
        Me.llAudioProfile0 = New StaxRip.UI.ButtonLabel()
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.lgbTarget = New StaxRip.UI.LinkGroupBox()
        Me.tlpTarget = New System.Windows.Forms.TableLayoutPanel()
        Me.tbTargetFile = New StaxRip.UI.TextEdit()
        Me.laTarget2 = New StaxRip.UI.LabelEx()
        Me.tbTargetSize = New StaxRip.UI.TextEdit()
        Me.lTarget1 = New StaxRip.UI.LabelEx()
        Me.tbBitrate = New StaxRip.UI.TextEdit()
        Me.laBitrate = New StaxRip.UI.LabelEx()
        Me.blFilesize = New StaxRip.UI.ButtonLabel()
        Me.lgbSource = New StaxRip.UI.LinkGroupBox()
        Me.tlpSource = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpSourceValues = New System.Windows.Forms.TableLayoutPanel()
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
        Me.tlpResize = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpResizeValues = New System.Windows.Forms.TableLayoutPanel()
        Me.blTargetDarText = New StaxRip.UI.ButtonLabel()
        Me.lAspectRatioError = New StaxRip.UI.LabelEx()
        Me.lPAR = New StaxRip.UI.LabelEx()
        Me.lZoom = New StaxRip.UI.LabelEx()
        Me.lSarText = New StaxRip.UI.LabelEx()
        Me.lPixel = New StaxRip.UI.LabelEx()
        Me.blTargetParText = New StaxRip.UI.ButtonLabel()
        Me.lAspectRatioErrorText = New StaxRip.UI.LabelEx()
        Me.lDAR = New StaxRip.UI.LabelEx()
        Me.lZoomText = New StaxRip.UI.LabelEx()
        Me.lPixelText = New StaxRip.UI.LabelEx()
        Me.lSAR = New StaxRip.UI.LabelEx()
        Me.tbResize = New System.Windows.Forms.TrackBar()
        Me.lTargetWidth = New StaxRip.UI.LabelEx()
        Me.tbTargetWidth = New StaxRip.UI.TextEdit()
        Me.lTargetHeight = New StaxRip.UI.LabelEx()
        Me.tbTargetHeight = New StaxRip.UI.TextEdit()
        Me.lgbFilters = New StaxRip.UI.LinkGroupBox()
        Me.FiltersListView = New StaxRip.FiltersListView()
        Me.lgbEncoder = New StaxRip.UI.LinkGroupBox()
        Me.llMuxer = New StaxRip.UI.ButtonLabel()
        Me.pnEncoder = New System.Windows.Forms.Panel()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
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
        Me.bnNext.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnNext.AutoSize = False
        Me.bnNext.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.bnNext.Cursor = System.Windows.Forms.Cursors.Default
        Me.bnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bnNext.Location = New System.Drawing.Point(1880, 33)
        Me.bnNext.Margin = New System.Windows.Forms.Padding(0, 0, 20, 0)
        'Me.bnNext.MinimumSize = New System.Drawing.Size(265, 85)
        Me.bnNext.Size = New System.Drawing.Size(CInt(275 * s.UIScaleFactor), CInt(105 * s.UIScaleFactor))
        Me.bnNext.UseCompatibleTextRendering = True
        '
        'llEditAudio0
        '
        Me.llEditAudio0.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.llEditAudio0.AutoSize = True
        Me.llEditAudio0.Location = New System.Drawing.Point(1933, 11)
        Me.llEditAudio0.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.llEditAudio0.Name = "llEditAudio0"
        Me.llEditAudio0.Size = New System.Drawing.Size(80, 48)
        Me.llEditAudio0.TabIndex = 20
        Me.llEditAudio0.TabStop = True
        Me.llEditAudio0.Text = "Edit"
        Me.llEditAudio0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbAssistant
        '
        Me.gbAssistant.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.gbAssistant, 4)
        Me.gbAssistant.Controls.Add(Me.tlpAssistant)
        Me.gbAssistant.Location = New System.Drawing.Point(9, 934)
        Me.gbAssistant.Margin = New System.Windows.Forms.Padding(9, 0, 9, 9)
        Me.gbAssistant.Name = "gbAssistant"
        Me.gbAssistant.Padding = New System.Windows.Forms.Padding(6, 0, 6, 6)
        Me.gbAssistant.Size = New System.Drawing.Size(2031, 191)
        Me.gbAssistant.TabIndex = 44
        Me.gbAssistant.TabStop = False
        Me.gbAssistant.Text = "Assistant"
        '
        'tlpAssistant
        '
        Me.tlpAssistant.ColumnCount = 2
        Me.tlpAssistant.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAssistant.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAssistant.Controls.Add(Me.laTip, 0, 0)
        Me.tlpAssistant.Controls.Add(Me.bnNext, 1, 0)
        Me.tlpAssistant.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAssistant.Location = New System.Drawing.Point(6, 48)
        Me.tlpAssistant.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpAssistant.Name = "tlpAssistant"
        Me.tlpAssistant.RowCount = 1
        Me.tlpAssistant.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAssistant.Size = New System.Drawing.Size(2019, 137)
        Me.tlpAssistant.TabIndex = 62
        '
        'laTip
        '
        Me.laTip.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.laTip.Location = New System.Drawing.Point(0, 0)
        Me.laTip.Margin = New System.Windows.Forms.Padding(0)
        Me.laTip.Size = New System.Drawing.Size(1880, 137)
        Me.laTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'llEditAudio1
        '
        Me.llEditAudio1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.llEditAudio1.AutoSize = True
        Me.llEditAudio1.Location = New System.Drawing.Point(1933, 89)
        Me.llEditAudio1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.llEditAudio1.Name = "llEditAudio1"
        Me.llEditAudio1.Size = New System.Drawing.Size(80, 48)
        Me.llEditAudio1.TabIndex = 21
        Me.llEditAudio1.TabStop = True
        Me.llEditAudio1.Text = "Edit"
        Me.llEditAudio1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbAudio
        '
        Me.gbAudio.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.gbAudio, 4)
        Me.gbAudio.Controls.Add(Me.tlpAudio)
        Me.gbAudio.Location = New System.Drawing.Point(9, 724)
        Me.gbAudio.Margin = New System.Windows.Forms.Padding(9, 0, 9, 0)
        Me.gbAudio.Name = "gbAudio"
        Me.gbAudio.Padding = New System.Windows.Forms.Padding(6, 0, 6, 6)
        Me.gbAudio.Size = New System.Drawing.Size(2031, 210)
        Me.gbAudio.TabIndex = 59
        Me.gbAudio.TabStop = False
        Me.gbAudio.Text = "Audio"
        '
        'tlpAudio
        '
        Me.tlpAudio.ColumnCount = 3
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudio.Controls.Add(Me.llEditAudio1, 2, 1)
        Me.tlpAudio.Controls.Add(Me.tbAudioFile0, 0, 0)
        Me.tlpAudio.Controls.Add(Me.llEditAudio0, 2, 0)
        Me.tlpAudio.Controls.Add(Me.tbAudioFile1, 0, 1)
        Me.tlpAudio.Controls.Add(Me.llAudioProfile1, 1, 1)
        Me.tlpAudio.Controls.Add(Me.llAudioProfile0, 1, 0)
        Me.tlpAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAudio.Location = New System.Drawing.Point(6, 48)
        Me.tlpAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpAudio.Name = "tlpAudio"
        Me.tlpAudio.RowCount = 2
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.0!))
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.0!))
        Me.tlpAudio.Size = New System.Drawing.Size(2019, 156)
        Me.tlpAudio.TabIndex = 62
        '
        'tbAudioFile0
        '
        Me.tbAudioFile0.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAudioFile0.Location = New System.Drawing.Point(6, 7)
        Me.tbAudioFile0.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.tbAudioFile0.Name = "tbAudioFile0"
        Me.tbAudioFile0.ReadOnly = False
        Me.tbAudioFile0.Size = New System.Drawing.Size(1869, 55)
        Me.tbAudioFile0.TabIndex = 22
        '
        'tbAudioFile1
        '
        Me.tbAudioFile1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAudioFile1.Location = New System.Drawing.Point(6, 85)
        Me.tbAudioFile1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.tbAudioFile1.Name = "tbAudioFile1"
        Me.tbAudioFile1.ReadOnly = False
        Me.tbAudioFile1.Size = New System.Drawing.Size(1869, 55)
        Me.tbAudioFile1.TabIndex = 23
        '
        'llAudioProfile1
        '
        Me.llAudioProfile1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.llAudioProfile1.AutoSize = True
        Me.llAudioProfile1.Location = New System.Drawing.Point(1887, 89)
        Me.llAudioProfile1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.llAudioProfile1.Name = "llAudioProfile1"
        Me.llAudioProfile1.Size = New System.Drawing.Size(34, 48)
        Me.llAudioProfile1.TabIndex = 26
        Me.llAudioProfile1.Text = "-"
        Me.llAudioProfile1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'llAudioProfile0
        '
        Me.llAudioProfile0.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.llAudioProfile0.AutoSize = True
        Me.llAudioProfile0.Location = New System.Drawing.Point(1887, 11)
        Me.llAudioProfile0.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.llAudioProfile0.Name = "llAudioProfile0"
        Me.llAudioProfile0.Size = New System.Drawing.Size(34, 48)
        Me.llAudioProfile0.TabIndex = 25
        Me.llAudioProfile0.Text = "-"
        Me.llAudioProfile0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MenuStrip
        '
        Me.MenuStrip.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MenuStrip.AutoSize = False
        Me.tlpMain.SetColumnSpan(Me.MenuStrip, 4)
        Me.MenuStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.MenuStrip.GripMargin = New System.Windows.Forms.Padding(2, 2, 0, 2)
        Me.MenuStrip.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Padding = New System.Windows.Forms.Padding(6, 6, 0, 6)
        Me.MenuStrip.Size = New System.Drawing.Size(2049, 81)
        Me.MenuStrip.TabIndex = 60
        Me.MenuStrip.Text = "MenuStrip"
        '
        'lgbTarget
        '
        Me.lgbTarget.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.lgbTarget, 2)
        Me.lgbTarget.Controls.Add(Me.tlpTarget)
        Me.lgbTarget.Location = New System.Drawing.Point(1029, 81)
        Me.lgbTarget.Margin = New System.Windows.Forms.Padding(6, 0, 9, 0)
        Me.lgbTarget.Name = "lgbTarget"
        Me.lgbTarget.Padding = New System.Windows.Forms.Padding(6, 0, 6, 6)
        Me.lgbTarget.Size = New System.Drawing.Size(1011, 357)
        Me.lgbTarget.TabIndex = 58
        Me.lgbTarget.TabStop = False
        Me.lgbTarget.Text = "Target"
        '
        'tlpTarget
        '
        Me.tlpTarget.ColumnCount = 4
        Me.tlpTarget.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.Controls.Add(Me.tbTargetFile, 0, 0)
        Me.tlpTarget.Controls.Add(Me.laTarget2, 0, 3)
        Me.tlpTarget.Controls.Add(Me.tbTargetSize, 1, 1)
        Me.tlpTarget.Controls.Add(Me.lTarget1, 0, 2)
        Me.tlpTarget.Controls.Add(Me.tbBitrate, 3, 1)
        Me.tlpTarget.Controls.Add(Me.laBitrate, 2, 1)
        Me.tlpTarget.Controls.Add(Me.blFilesize, 0, 1)
        Me.tlpTarget.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpTarget.Location = New System.Drawing.Point(6, 48)
        Me.tlpTarget.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpTarget.Name = "tlpTarget"
        Me.tlpTarget.RowCount = 4
        Me.tlpTarget.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpTarget.Size = New System.Drawing.Size(999, 303)
        Me.tlpTarget.TabIndex = 62
        '
        'tbTargetFile
        '
        Me.tbTargetFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpTarget.SetColumnSpan(Me.tbTargetFile, 4)
        Me.tbTargetFile.Location = New System.Drawing.Point(6, 10)
        Me.tbTargetFile.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.tbTargetFile.Name = "tbTargetFile"
        Me.tbTargetFile.ReadOnly = False
        Me.tbTargetFile.Size = New System.Drawing.Size(987, 55)
        Me.tbTargetFile.TabIndex = 0
        '
        'laTarget2
        '
        Me.laTarget2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpTarget.SetColumnSpan(Me.laTarget2, 4)
        Me.laTarget2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.laTarget2.Location = New System.Drawing.Point(6, 234)
        Me.laTarget2.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.laTarget2.Size = New System.Drawing.Size(987, 60)
        Me.laTarget2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTargetSize
        '
        Me.tbTargetSize.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tbTargetSize.Location = New System.Drawing.Point(249, 85)
        Me.tbTargetSize.Margin = New System.Windows.Forms.Padding(0)
        Me.tbTargetSize.Name = "tbTargetSize"
        Me.tbTargetSize.ReadOnly = False
        Me.tbTargetSize.Size = New System.Drawing.Size(136, 55)
        Me.tbTargetSize.TabIndex = 55
        Me.tbTargetSize.TextBox.TextAlign = HorizontalAlignment.Center
        '
        'lTarget1
        '
        Me.lTarget1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpTarget.SetColumnSpan(Me.lTarget1, 4)
        Me.lTarget1.Location = New System.Drawing.Point(6, 157)
        Me.lTarget1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lTarget1.Size = New System.Drawing.Size(987, 60)
        Me.lTarget1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbBitrate
        '
        Me.tbBitrate.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tbBitrate.Location = New System.Drawing.Point(747, 85)
        Me.tbBitrate.Margin = New System.Windows.Forms.Padding(0)
        Me.tbBitrate.Name = "tbBitrate"
        Me.tbBitrate.ReadOnly = False
        Me.tbBitrate.Size = New System.Drawing.Size(139, 55)
        Me.tbBitrate.TabIndex = 41
        Me.tbBitrate.TextBox.TextAlign = HorizontalAlignment.Center
        '
        'laBitrate
        '
        Me.laBitrate.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.laBitrate.Location = New System.Drawing.Point(498, 75)
        Me.laBitrate.Margin = New System.Windows.Forms.Padding(0)
        Me.laBitrate.Size = New System.Drawing.Size(249, 75)
        Me.laBitrate.Text = "Video Bitrate:"
        Me.laBitrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'blFilesize
        '
        Me.blFilesize.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.blFilesize.Location = New System.Drawing.Point(6, 75)
        Me.blFilesize.Margin = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.blFilesize.Name = "blFilesize"
        Me.blFilesize.Size = New System.Drawing.Size(243, 75)
        Me.blFilesize.TabIndex = 59
        Me.blFilesize.TabStop = True
        Me.blFilesize.Text = "Size:"
        Me.blFilesize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lgbSource
        '
        Me.lgbSource.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.lgbSource, 2)
        Me.lgbSource.Controls.Add(Me.tlpSource)
        Me.lgbSource.Location = New System.Drawing.Point(9, 81)
        Me.lgbSource.Margin = New System.Windows.Forms.Padding(9, 0, 6, 0)
        Me.lgbSource.Name = "lgbSource"
        Me.lgbSource.Padding = New System.Windows.Forms.Padding(6, 0, 6, 6)
        Me.lgbSource.Size = New System.Drawing.Size(1008, 357)
        Me.lgbSource.TabIndex = 57
        Me.lgbSource.TabStop = False
        Me.lgbSource.Text = "Source"
        '
        'tlpSource
        '
        Me.tlpSource.ColumnCount = 1
        Me.tlpSource.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpSource.Controls.Add(Me.tlpSourceValues, 0, 3)
        Me.tlpSource.Controls.Add(Me.tbSourceFile, 0, 0)
        Me.tlpSource.Controls.Add(Me.lSource1, 0, 1)
        Me.tlpSource.Controls.Add(Me.lSource2, 0, 2)
        Me.tlpSource.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpSource.Location = New System.Drawing.Point(6, 48)
        Me.tlpSource.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpSource.Name = "tlpSource"
        Me.tlpSource.RowCount = 4
        Me.tlpSource.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpSource.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpSource.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpSource.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpSource.Size = New System.Drawing.Size(996, 303)
        Me.tlpSource.TabIndex = 62
        '
        'tlpSourceValues
        '
        Me.tlpSourceValues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpSourceValues.ColumnCount = 7
        Me.tlpSourceValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpSourceValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpSourceValues.Controls.Add(Me.blSourceDarText, 4, 0)
        Me.tlpSourceValues.Controls.Add(Me.lSourcePAR, 3, 0)
        Me.tlpSourceValues.Controls.Add(Me.blSourceParText, 2, 0)
        Me.tlpSourceValues.Controls.Add(Me.lCrop, 1, 0)
        Me.tlpSourceValues.Controls.Add(Me.lCropText, 0, 0)
        Me.tlpSourceValues.Controls.Add(Me.lSourceDar, 5, 0)
        Me.tlpSourceValues.Location = New System.Drawing.Point(0, 225)
        Me.tlpSourceValues.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpSourceValues.Name = "tlpSourceValues"
        Me.tlpSourceValues.RowCount = 1
        Me.tlpSourceValues.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpSourceValues.Size = New System.Drawing.Size(996, 78)
        Me.tlpSourceValues.TabIndex = 1
        '
        'blSourceDarText
        '
        Me.blSourceDarText.Anchor = System.Windows.Forms.AnchorStyles.None
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
        Me.lSourcePAR.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lSourcePAR.AutoSize = True
        Me.lSourcePAR.Location = New System.Drawing.Point(400, 15)
        Me.lSourcePAR.Margin = New System.Windows.Forms.Padding(9, 0, 9, 0)
        Me.lSourcePAR.Size = New System.Drawing.Size(34, 48)
        Me.lSourcePAR.Text = "-"
        Me.lSourcePAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'blSourceParText
        '
        Me.blSourceParText.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.blSourceParText.AutoSize = True
        Me.blSourceParText.Location = New System.Drawing.Point(291, 15)
        Me.blSourceParText.Margin = New System.Windows.Forms.Padding(9, 0, 9, 0)
        Me.blSourceParText.Name = "blSourceParText"
        Me.blSourceParText.Size = New System.Drawing.Size(91, 48)
        Me.blSourceParText.TabIndex = 45
        Me.blSourceParText.TabStop = True
        Me.blSourceParText.Text = "PAR:"
        Me.blSourceParText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lCrop
        '
        Me.lCrop.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lCrop.AutoSize = True
        Me.lCrop.Location = New System.Drawing.Point(120, 15)
        Me.lCrop.Margin = New System.Windows.Forms.Padding(9, 0, 9, 0)
        Me.lCrop.Size = New System.Drawing.Size(153, 48)
        Me.lCrop.Text = "disabled"
        Me.lCrop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lCropText
        '
        Me.lCropText.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lCropText.AutoSize = True
        Me.lCropText.Location = New System.Drawing.Point(3, 15)
        Me.lCropText.Size = New System.Drawing.Size(105, 48)
        Me.lCropText.TabStop = True
        Me.lCropText.Text = "Crop:"
        Me.lCropText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourceDar
        '
        Me.lSourceDar.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lSourceDar.AutoSize = True
        Me.lSourceDar.Location = New System.Drawing.Point(549, 15)
        Me.lSourceDar.Size = New System.Drawing.Size(34, 48)
        Me.lSourceDar.Text = "-"
        Me.lSourceDar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSourceFile
        '
        Me.tbSourceFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSourceFile.Location = New System.Drawing.Point(6, 10)
        Me.tbSourceFile.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.tbSourceFile.Name = "tbSourceFile"
        Me.tbSourceFile.ReadOnly = False
        Me.tbSourceFile.Size = New System.Drawing.Size(984, 55)
        Me.tbSourceFile.TabIndex = 2
        '
        'lSource1
        '
        Me.lSource1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lSource1.Location = New System.Drawing.Point(6, 84)
        Me.lSource1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lSource1.Size = New System.Drawing.Size(984, 57)
        Me.lSource1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSource2
        '
        Me.lSource2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lSource2.Location = New System.Drawing.Point(6, 159)
        Me.lSource2.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lSource2.Size = New System.Drawing.Size(984, 57)
        Me.lSource2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lgbResize
        '
        Me.lgbResize.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.lgbResize, 2)
        Me.lgbResize.Controls.Add(Me.tlpResize)
        Me.lgbResize.Location = New System.Drawing.Point(688, 438)
        Me.lgbResize.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lgbResize.Name = "lgbResize"
        Me.lgbResize.Padding = New System.Windows.Forms.Padding(6, 0, 6, 6)
        Me.lgbResize.Size = New System.Drawing.Size(670, 286)
        Me.lgbResize.TabIndex = 55
        Me.lgbResize.TabStop = False
        Me.lgbResize.Text = "Resize"
        '
        'tlpResize
        '
        Me.tlpResize.ColumnCount = 4
        Me.tlpResize.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpResize.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpResize.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpResize.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpResize.Controls.Add(Me.tlpResizeValues, 0, 2)
        Me.tlpResize.Controls.Add(Me.tbResize, 0, 1)
        Me.tlpResize.Controls.Add(Me.lTargetWidth, 0, 0)
        Me.tlpResize.Controls.Add(Me.tbTargetWidth, 1, 0)
        Me.tlpResize.Controls.Add(Me.lTargetHeight, 2, 0)
        Me.tlpResize.Controls.Add(Me.tbTargetHeight, 3, 0)
        Me.tlpResize.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpResize.Location = New System.Drawing.Point(6, 48)
        Me.tlpResize.Name = "tlpResize"
        Me.tlpResize.RowCount = 3
        Me.tlpResize.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 81.0!))
        Me.tlpResize.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 81.0!))
        Me.tlpResize.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpResize.Size = New System.Drawing.Size(658, 232)
        Me.tlpResize.TabIndex = 63
        '
        'tlpResizeValues
        '
        Me.tlpResizeValues.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpResizeValues.ColumnCount = 4
        Me.tlpResize.SetColumnSpan(Me.tlpResizeValues, 4)
        Me.tlpResizeValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpResizeValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpResizeValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpResizeValues.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpResizeValues.Controls.Add(Me.blTargetDarText, 0, 0)
        Me.tlpResizeValues.Controls.Add(Me.lAspectRatioError, 3, 2)
        Me.tlpResizeValues.Controls.Add(Me.lPAR, 1, 2)
        Me.tlpResizeValues.Controls.Add(Me.lZoom, 3, 1)
        Me.tlpResizeValues.Controls.Add(Me.lSarText, 0, 1)
        Me.tlpResizeValues.Controls.Add(Me.lPixel, 3, 0)
        Me.tlpResizeValues.Controls.Add(Me.blTargetParText, 0, 2)
        Me.tlpResizeValues.Controls.Add(Me.lAspectRatioErrorText, 2, 2)
        Me.tlpResizeValues.Controls.Add(Me.lDAR, 1, 0)
        Me.tlpResizeValues.Controls.Add(Me.lZoomText, 2, 1)
        Me.tlpResizeValues.Controls.Add(Me.lPixelText, 2, 0)
        Me.tlpResizeValues.Controls.Add(Me.lSAR, 1, 1)
        Me.tlpResizeValues.Location = New System.Drawing.Point(0, 162)
        Me.tlpResizeValues.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpResizeValues.Name = "tlpResizeValues"
        Me.tlpResizeValues.RowCount = 3
        Me.tlpResizeValues.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpResizeValues.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpResizeValues.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpResizeValues.Size = New System.Drawing.Size(658, 70)
        Me.tlpResizeValues.TabIndex = 62
        '
        'blTargetDarText
        '
        Me.blTargetDarText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.blTargetDarText.AutoSize = True
        Me.blTargetDarText.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.blTargetDarText.Location = New System.Drawing.Point(0, 0)
        Me.blTargetDarText.Margin = New System.Windows.Forms.Padding(0)
        Me.blTargetDarText.Name = "blTargetDarText"
        Me.blTargetDarText.Size = New System.Drawing.Size(97, 23)
        Me.blTargetDarText.TabIndex = 23
        Me.blTargetDarText.Text = "DAR:"
        Me.blTargetDarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lAspectRatioError
        '
        Me.lAspectRatioError.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lAspectRatioError.AutoSize = True
        Me.lAspectRatioError.Location = New System.Drawing.Point(253, 46)
        Me.lAspectRatioError.Margin = New System.Windows.Forms.Padding(0)
        Me.lAspectRatioError.Size = New System.Drawing.Size(34, 24)
        Me.lAspectRatioError.Text = "-"
        Me.lAspectRatioError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPAR
        '
        Me.lPAR.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lPAR.AutoSize = True
        Me.lPAR.Location = New System.Drawing.Point(97, 46)
        Me.lPAR.Margin = New System.Windows.Forms.Padding(0)
        Me.lPAR.Size = New System.Drawing.Size(34, 24)
        Me.lPAR.Text = "-"
        Me.lPAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lZoom
        '
        Me.lZoom.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lZoom.AutoSize = True
        Me.lZoom.Location = New System.Drawing.Point(253, 23)
        Me.lZoom.Margin = New System.Windows.Forms.Padding(0)
        Me.lZoom.Size = New System.Drawing.Size(34, 23)
        Me.lZoom.Text = "-"
        Me.lZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSarText
        '
        Me.lSarText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lSarText.AutoSize = True
        Me.lSarText.Location = New System.Drawing.Point(0, 23)
        Me.lSarText.Margin = New System.Windows.Forms.Padding(0)
        Me.lSarText.Size = New System.Drawing.Size(92, 23)
        Me.lSarText.Text = "SAR:"
        Me.lSarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPixel
        '
        Me.lPixel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lPixel.AutoSize = True
        Me.lPixel.Location = New System.Drawing.Point(253, 0)
        Me.lPixel.Margin = New System.Windows.Forms.Padding(0)
        Me.lPixel.Size = New System.Drawing.Size(34, 23)
        Me.lPixel.Text = "-"
        Me.lPixel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'blTargetParText
        '
        Me.blTargetParText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.blTargetParText.AutoSize = True
        Me.blTargetParText.Location = New System.Drawing.Point(0, 46)
        Me.blTargetParText.Margin = New System.Windows.Forms.Padding(0)
        Me.blTargetParText.Name = "blTargetParText"
        Me.blTargetParText.Size = New System.Drawing.Size(91, 24)
        Me.blTargetParText.TabIndex = 51
        Me.blTargetParText.Text = "PAR:"
        Me.blTargetParText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lAspectRatioErrorText
        '
        Me.lAspectRatioErrorText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lAspectRatioErrorText.AutoSize = True
        Me.lAspectRatioErrorText.Location = New System.Drawing.Point(131, 46)
        Me.lAspectRatioErrorText.Margin = New System.Windows.Forms.Padding(0)
        Me.lAspectRatioErrorText.Size = New System.Drawing.Size(106, 24)
        Me.lAspectRatioErrorText.Text = "Error:"
        Me.lAspectRatioErrorText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lDAR
        '
        Me.lDAR.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lDAR.AutoSize = True
        Me.lDAR.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lDAR.Location = New System.Drawing.Point(97, 0)
        Me.lDAR.Margin = New System.Windows.Forms.Padding(0)
        Me.lDAR.Size = New System.Drawing.Size(34, 23)
        Me.lDAR.Text = "-"
        Me.lDAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lZoomText
        '
        Me.lZoomText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lZoomText.AutoSize = True
        Me.lZoomText.Location = New System.Drawing.Point(131, 23)
        Me.lZoomText.Margin = New System.Windows.Forms.Padding(0)
        Me.lZoomText.Size = New System.Drawing.Size(122, 23)
        Me.lZoomText.Text = "Zoom:"
        Me.lZoomText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPixelText
        '
        Me.lPixelText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lPixelText.AutoSize = True
        Me.lPixelText.Location = New System.Drawing.Point(131, 0)
        Me.lPixelText.Margin = New System.Windows.Forms.Padding(0)
        Me.lPixelText.Size = New System.Drawing.Size(102, 23)
        Me.lPixelText.Text = "Pixel:"
        Me.lPixelText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSAR
        '
        Me.lSAR.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lSAR.AutoSize = True
        Me.lSAR.Location = New System.Drawing.Point(97, 23)
        Me.lSAR.Margin = New System.Windows.Forms.Padding(0)
        Me.lSAR.Size = New System.Drawing.Size(34, 23)
        Me.lSAR.Text = "-"
        Me.lSAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbResize
        '
        Me.tbResize.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbResize.AutoSize = False
        Me.tlpResize.SetColumnSpan(Me.tbResize, 4)
        Me.tbResize.Location = New System.Drawing.Point(0, 81)
        Me.tbResize.Margin = New System.Windows.Forms.Padding(0)
        Me.tbResize.Name = "tbResize"
        Me.tbResize.Size = New System.Drawing.Size(658, 81)
        Me.tbResize.TabIndex = 46
        '
        'lTargetWidth
        '
        Me.lTargetWidth.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lTargetWidth.AutoSize = True
        Me.lTargetWidth.Location = New System.Drawing.Point(3, 16)
        Me.lTargetWidth.Size = New System.Drawing.Size(124, 48)
        Me.lTargetWidth.Text = "Width:"
        Me.lTargetWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTargetWidth
        '
        Me.tbTargetWidth.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tbTargetWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbTargetWidth.Location = New System.Drawing.Point(167, 13)
        Me.tbTargetWidth.Name = "tbTargetWidth"
        Me.tbTargetWidth.ReadOnly = False
        Me.tbTargetWidth.Size = New System.Drawing.Size(130, 55)
        Me.tbTargetWidth.TabIndex = 39
        Me.tbTargetWidth.TextBox.TextAlign = HorizontalAlignment.Center
        '
        'lTargetHeight
        '
        Me.lTargetHeight.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lTargetHeight.AutoSize = True
        Me.lTargetHeight.Location = New System.Drawing.Point(331, 16)
        Me.lTargetHeight.Size = New System.Drawing.Size(135, 48)
        Me.lTargetHeight.Text = "Height:"
        Me.lTargetHeight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTargetHeight
        '
        Me.tbTargetHeight.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tbTargetHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbTargetHeight.Location = New System.Drawing.Point(495, 13)
        Me.tbTargetHeight.Name = "tbTargetHeight"
        Me.tbTargetHeight.ReadOnly = False
        Me.tbTargetHeight.Size = New System.Drawing.Size(130, 55)
        Me.tbTargetHeight.TabIndex = 40
        Me.tbTargetHeight.TextBox.TextAlign = HorizontalAlignment.Center
        '
        'lgbFilters
        '
        Me.lgbFilters.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lgbFilters.Controls.Add(Me.FiltersListView)
        Me.lgbFilters.Location = New System.Drawing.Point(9, 438)
        Me.lgbFilters.Margin = New System.Windows.Forms.Padding(9, 0, 6, 0)
        Me.lgbFilters.Name = "lgbFilters"
        Me.lgbFilters.Padding = New System.Windows.Forms.Padding(9, 3, 9, 9)
        Me.lgbFilters.Size = New System.Drawing.Size(667, 286)
        Me.lgbFilters.TabIndex = 53
        Me.lgbFilters.TabStop = False
        '
        'FiltersListView
        '
        Me.FiltersListView.AllowDrop = True
        Me.FiltersListView.AutoCheckMode = StaxRip.UI.AutoCheckMode.None
        Me.FiltersListView.CheckBoxes = True
        Me.FiltersListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FiltersListView.FullRowSelect = True
        Me.FiltersListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.FiltersListView.HideSelection = False
        Me.FiltersListView.IsLoading = False
        Me.FiltersListView.Location = New System.Drawing.Point(9, 51)
        Me.FiltersListView.MultiSelect = False
        Me.FiltersListView.Name = "FiltersListView"
        Me.FiltersListView.OwnerDraw = True
        Me.FiltersListView.Size = New System.Drawing.Size(649, 226)
        Me.FiltersListView.TabIndex = 0
        Me.FiltersListView.UseCompatibleStateImageBehavior = False
        Me.FiltersListView.View = System.Windows.Forms.View.Details
        '
        'lgbEncoder
        '
        Me.lgbEncoder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lgbEncoder.Controls.Add(Me.llMuxer)
        Me.lgbEncoder.Controls.Add(Me.pnEncoder)
        Me.lgbEncoder.Location = New System.Drawing.Point(1370, 438)
        Me.lgbEncoder.Margin = New System.Windows.Forms.Padding(6, 0, 9, 0)
        Me.lgbEncoder.Name = "lgbEncoder"
        Me.lgbEncoder.Padding = New System.Windows.Forms.Padding(9, 3, 9, 9)
        Me.lgbEncoder.Size = New System.Drawing.Size(670, 286)
        Me.lgbEncoder.TabIndex = 51
        Me.lgbEncoder.TabStop = False
        '
        'llMuxer
        '
        Me.llMuxer.AutoSize = True
        Me.llMuxer.Location = New System.Drawing.Point(516, 0)
        Me.llMuxer.Name = "llMuxer"
        Me.llMuxer.Size = New System.Drawing.Size(121, 48)
        Me.llMuxer.TabIndex = 1
        Me.llMuxer.TabStop = True
        Me.llMuxer.Text = "Muxer"
        '
        'pnEncoder
        '
        Me.pnEncoder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnEncoder.Location = New System.Drawing.Point(9, 51)
        Me.pnEncoder.Name = "pnEncoder"
        Me.pnEncoder.Size = New System.Drawing.Size(652, 226)
        Me.pnEncoder.TabIndex = 0
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 4
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.67!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.67!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33!))
        Me.tlpMain.Controls.Add(Me.lgbTarget, 2, 1)
        Me.tlpMain.Controls.Add(Me.gbAssistant, 0, 4)
        Me.tlpMain.Controls.Add(Me.gbAudio, 0, 3)
        Me.tlpMain.Controls.Add(Me.lgbSource, 0, 1)
        Me.tlpMain.Controls.Add(Me.lgbResize, 1, 2)
        Me.tlpMain.Controls.Add(Me.MenuStrip, 0, 0)
        Me.tlpMain.Controls.Add(Me.lgbFilters, 0, 2)
        Me.tlpMain.Controls.Add(Me.lgbEncoder, 3, 2)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 5
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 81.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 357.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 210.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.tlpMain.Size = New System.Drawing.Size(2049, 1134)
        Me.tlpMain.TabIndex = 61
        '
        'MainForm
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(2049, 1134)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.MainMenuStrip = Me.MenuStrip
        Me.Margin = New System.Windows.Forms.Padding(9, 12, 9, 12)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "StaxRip"
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

    End Sub

#End Region

    Public WithEvents CustomMainMenu As CustomMenu
    Public WithEvents CustomSizeMenu As CustomMenu
    Public CurrentAssistantTipKey As String
    Public BlockSubtitlesItemCheck As Boolean
    Public AssistantPassed As Boolean
    Public CommandManager As New CommandManager

    Private AudioMenu0 As ContextMenuStripEx
    Private AudioMenu1 As ContextMenuStripEx
    Private TargetAspectRatioMenu As ContextMenuStripEx
    Private SizeContextMenuStrip As ContextMenuStripEx
    Private EncoderMenu As ContextMenuStripEx
    Private ContainerMenu As ContextMenuStripEx
    Private SourceAspectRatioMenu As ContextMenuStripEx
    Private TargetFileMenu As ContextMenuStripEx
    Private SourceFileMenu As ContextMenuStripEx
    Private Audio0FileMenu As ContextMenuStripEx
    Private Audio1FileMenu As ContextMenuStripEx
    Private NextContextMenuStrip As ContextMenuStripEx
    Private BlockAviSynthItemCheck As Boolean
    Private CanChangeSize As Boolean = True
    Private CanChangeBitrate As Boolean = True
    Private CanIgnoreTip As Boolean = True
    Private IsLoading As Boolean = True
    Private BlockBitrate, BlockSize As Boolean
    Private SkipAssistant As Boolean
    Private BlockSourceTextBoxTextChanged As Boolean
    Private AssistantClickAction As Action
    Private ThemeRefresh As Boolean

    Sub New()
        AddHandler Application.ThreadException, AddressOf g.OnUnhandledException
        g.MainForm = Me
        LoadSettings()

        PowerShell.InitCode =
            "Using namespace StaxRip;" + BR +
            "Using namespace StaxRip.UI;" + BR +
            "[Reflection.Assembly]::LoadWithPartialName(""StaxRip"")"

        If s.WriteDebugLog Then
            Dim filePath = Folder.Startup + "Debug.log"

            If File.Exists(filePath) Then
                File.Delete(filePath)
            End If

            Dim listener = New TextWriterTraceListener(filePath)
            listener.TraceOutputOptions = TraceOptions.ThreadId Or TraceOptions.DateTime
            Trace.Listeners.Add(listener)
            Trace.AutoFlush = True
        End If

        MenuItemEx.UseTooltips = s.EnableTooltips
        Icon = g.Icon

        InitializeComponent()
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        ScaleClientSize(41, 26.5)
        g.DPI = DeviceDpi

        If components Is Nothing Then
            components = New System.ComponentModel.Container
        End If

        SetTip()

        AudioMenu0 = New ContextMenuStripEx(components)
        AudioMenu1 = New ContextMenuStripEx(components)
        TargetAspectRatioMenu = New ContextMenuStripEx(components)
        SizeContextMenuStrip = New ContextMenuStripEx(components)
        EncoderMenu = New ContextMenuStripEx(components)
        ContainerMenu = New ContextMenuStripEx(components)
        SourceAspectRatioMenu = New ContextMenuStripEx(components)
        TargetFileMenu = New ContextMenuStripEx(components)
        SourceFileMenu = New ContextMenuStripEx(components)
        Audio0FileMenu = New ContextMenuStripEx(components)
        Audio1FileMenu = New ContextMenuStripEx(components)
        Audio1FileMenu = New ContextMenuStripEx(components)
        NextContextMenuStrip = New ContextMenuStripEx(components)

        tbTargetFile.TextBox.ContextMenuStrip = TargetFileMenu
        tbSourceFile.TextBox.ContextMenuStrip = SourceFileMenu
        tbAudioFile0.TextBox.ContextMenuStrip = Audio0FileMenu
        tbAudioFile1.TextBox.ContextMenuStrip = Audio1FileMenu

        Dim rc = "right-click"
        tbAudioFile0.TextBox.SendMessageCue(rc, False)
        tbAudioFile1.TextBox.SendMessageCue(rc, False)
        tbSourceFile.TextBox.SendMessageCue(rc, False)
        tbTargetFile.TextBox.SendMessageCue(rc, False)

        llEditAudio0.ClickAction = AddressOf AudioEdit0ToolStripMenuItemClick
        llEditAudio1.ClickAction = AddressOf AudioEdit1ToolStripMenuItemClick

        MenuStrip.SuspendLayout()
        MenuStrip.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)

        CommandManager.AddCommandsFromObject(Me)
        CommandManager.AddCommandsFromObject(g.DefaultCommands)

        CustomMainMenu = New CustomMenu(AddressOf GetDefaultMainMenu, s.CustomMenuMainForm, CommandManager, MenuStrip)


        OpenProject(g.StartupTemplatePath)
        CustomMainMenu.AddKeyDownHandler(Me)
        CustomMainMenu.BuildMenu()
        UpdateAudioMenu()
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
        ApplyTheme()

        AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
    End Sub

    Sub OnThemeChanged(theme As Theme)
        ApplyTheme()
        Assistant()
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
            If TypeOf control.Parent Is UserControl Then
                control.BackColor = theme.General.Controls.ListView.BackColor
            Else
                control.BackColor = theme.General.Controls.ButtonLabel.BackColor
            End If

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
    End Sub

    Sub LoadSettings()
        Try
            Using mutex As New Mutex(False, "staxrip settings file")
                mutex.WaitOne()
                s = SafeSerialization.Deserialize(New ApplicationSettings, g.SettingsFile)
                mutex.ReleaseMutex()
            End Using
        Catch ex As Exception
            Using td As New TaskDialog(Of String)
                td.Title = "The settings failed to load!"
                td.Content = ex.Message
                td.Icon = TaskIcon.Error
                td.AddButton("Retry")
                td.AddButton("Reset")
                td.AddButton("Exit")

                Select Case td.Show
                    Case "Retry"
                        LoadSettings()
                    Case "Reset"
                        If MsgQuestion("Are you sure you want to reset your settings? Your current settings will be lost!") = DialogResult.OK Then
                            s = New ApplicationSettings
                            s.Init()
                        Else
                            LoadSettings()
                        End If
                    Case Else
                        Process.GetCurrentProcess.Kill()
                End Select
            End Using
        End Try
    End Sub

    Function GetIfoFile() As String
        Dim ret = p.SourceFile.Dir

        If ret.EndsWith("_temp\") Then
            ret = ret.Parent
        End If

        ret = ret + p.SourceFile.FileName.DeleteRight(5) + "0.ifo"

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
                                    base += " " + stream.Language.Name
                                End If

                                If stream.Title <> "" Then
                                    base += " " + stream.Title
                                End If

                                FileHelp.Move(i, i.Dir + base + i.ExtFull)
                            End If
                        Next
                    End Using
                End If
            End If
        Next
    End Sub

    Sub DetectAudioFiles(track As Integer, lang As Boolean, same As Boolean, hq As Boolean)
        Dim tb, tbOther As TextEdit
        Dim profile As AudioProfile

        If track = 0 Then
            tb = tbAudioFile0
            tbOther = tbAudioFile1
            profile = p.Audio0
        Else
            tb = tbAudioFile1
            tbOther = tbAudioFile0
            profile = p.Audio1
        End If

        If tb.Text <> "" OrElse TypeOf profile Is NullAudioProfile Then
            Exit Sub
        End If

        Dim files = g.GetFilesInTempDirAndParent
        files.Sort(New StringLogicalComparer)

        For Each iExt In FileTypes.Audio
            If iExt = "avs" Then
                Continue For
            End If

            For Each iPath In files
                If track = 1 AndAlso iPath.Base = p.Audio0.File.Base Then Continue For
                If track = 0 AndAlso iPath.Base = p.Audio1.File.Base Then Continue For
                If tbOther.Text = iPath Then Continue For
                If Not iPath.Ext = iExt Then Continue For
                If iPath.Contains("_cut_") Then Continue For
                If iPath.Contains("_out") Then Continue For
                If Not g.IsSourceSame(iPath) Then Continue For
                If hq AndAlso Not iPath.Ext.EqualsAny(FileTypes.AudioHQ) Then Continue For

                If same AndAlso tbOther.Text <> "" AndAlso tbOther.Text.ExtFull <> iPath.ExtFull Then
                    Continue For
                End If

                If lang Then
                    Dim lng = profile.Language

                    If profile.Language.ThreeLetterCode = "und" Then
                        lng = If(track = 0, New Language(), New Language("en"))
                    End If

                    If Not iPath.Contains(lng.Name) Then
                        Continue For
                    End If
                End If

                If iPath.Ext = "mp4" AndAlso p.SourceFile.IsSameBase(iPath) Then
                    Continue For
                End If

                If Not (track = 1 AndAlso p.Audio0.File = iPath) AndAlso Not (track = 0 AndAlso p.Audio1.File = iPath) Then
                    tb.Text = iPath
                    Exit Sub
                End If
            Next
        Next
    End Sub

    Function IsSaveCanceled() As Boolean
        'ObjectHelp.GetCompareString(g.SavedProject).WriteFile(Folder.Desktop + "\test1.txt", Encoding.ASCII)
        'ObjectHelp.GetCompareString(p).WriteFile(Folder.Desktop + "\test2.txt", Encoding.ASCII)

        If ObjectHelp.GetCompareString(g.SavedProject) <> ObjectHelp.GetCompareString(p) Then
            If s.AutoSaveProject AndAlso p.SourceFile <> "" Then
                If g.ProjectPath Is Nothing Then
                    g.ProjectPath = p.TempDir + p.TargetFile.Base + ".srip"
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
                End If
            End Using
        End If
    End Function

    Sub UpdateRecentProjectsMenu()
        If Disposing OrElse IsDisposed Then
            Exit Sub
        End If

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
        PopulateProfileMenu(DynamicMenuItemID.Audio1Profiles)
        PopulateProfileMenu(DynamicMenuItemID.Audio2Profiles)
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
        Dim files As String()

        Await Task.Run(Sub()
                           Thread.Sleep(500)

                           For Each iFile In Directory.GetFiles(Folder.Template, "*.srip.backup")
                               FileHelp.Move(iFile, iFile.Dir + "Backup\" + iFile.Base)
                           Next

                           files = Directory.GetFiles(Folder.Template, "*.srip", SearchOption.AllDirectories)
                       End Sub)

        If IsDisposed Then
            Exit Sub
        End If

        For Each i In CustomMainMenu.MenuItems
            If i.CustomMenuItem.MethodName = "DynamicMenuItem" AndAlso
                i.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.TemplateProjects) Then

                i.DropDownItems.ClearAndDisplose
                Dim items As New List(Of MenuItemEx)

                For Each i2 In files
                    Dim base = i2.Base

                    If i2 = g.StartupTemplatePath Then
                        base += " (Startup)"
                    End If

                    If i2.Contains("Backup\") Then
                        base = "Backup | " + base
                    End If

                    MenuItemEx.Add(i.DropDownItems, base, AddressOf LoadProject, i2, Nothing)
                Next

                i.DropDownItems.Add("-")
                MenuItemEx.Add(i.DropDownItems, "Explore", Sub() g.ShellExecute(Folder.Template), "Opens the directory containing the templates.")
                MenuItemEx.Add(i.DropDownItems, "Restore", AddressOf ResetTemplates, "Restores the default templates.")

                Exit For
            End If
        Next

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
        LoadProject(Folder.Template + name + ".srip")
    End Sub

    <Command("Adds a batch job for multiple files.")>
    Sub AddBatchJobs(sourcefiles As String())
        If sourcefiles Is Nothing Then Return

        For Each sourcefile In sourcefiles
            AddBatchJob(sourcefile)
        Next
    End Sub

    <Command("Adds a batch job for a single file.")>
    Sub AddBatchJob(sourcefile As String)
        Dim batchFolder = Folder.Settings + "Batch Projects\"

        If Not Directory.Exists(batchFolder) Then
            Directory.CreateDirectory(batchFolder)
        End If

        Dim batchProject = ObjectHelp.GetCopy(Of Project)(p)
        batchProject.BatchMode = True
        batchProject.SourceFiles = {sourcefile}.ToList
        Dim jobPath = batchFolder + sourcefile.Dir.Replace("\", "-").Replace(":", "-") + " " + p.TemplateName + " - " + sourcefile.FileName + ".srip"
        SafeSerialization.Serialize(batchProject, jobPath)
        JobManager.AddJob(sourcefile.Base, jobPath)
    End Sub

    Sub LoadProject(path As String)
        Refresh()

        If Not File.Exists(path) Then
            MsgWarn("Project file not found.")
            s.UpdateRecentProjects(path)
            UpdateRecentProjectsMenu()
            UpdateTemplatesMenuAsync()
        Else
            OpenProject(path)
        End If
    End Sub

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
            If Not IsLoading AndAlso saveCurrent AndAlso IsSaveCanceled() Then
                Return False
            End If

            If path = "" OrElse Not File.Exists(path) Then
                path = g.StartupTemplatePath
            End If

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
            If String.IsNullOrWhiteSpace(path) OrElse Not File.Exists(path) Then
                path = g.StartupTemplatePath
            End If

            p = If(proj, SafeSerialization.Deserialize(New Project(), path))
            Log = p.Log

            If File.Exists(Folder.Temp + "staxrip.log") Then
                FileHelp.Delete(Folder.Temp + "staxrip.log")
            End If

            SetBindings(p, True)

            Text = path.Base + " - " + Application.ProductName + " v" + Application.ProductVersion

            If Not Environment.Is64BitProcess Then
                Text += " (32 bit)"
            End If

            SkipAssistant = True

            If path.StartsWith(Folder.Template) Then
                g.ProjectPath = Nothing
                p.TemplateName = path.Base
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

            Dim audio0 = p.Audio0.File
            Dim audio1 = p.Audio1.File

            Dim delay0 = p.Audio0.Delay
            Dim delay1 = p.Audio1.Delay

            BlockSourceTextBoxTextChanged = True
            tbSourceFile.Text = p.SourceFile
            BlockSourceTextBoxTextChanged = False

            If p.SourceFile <> "" Then
                s.LastSourceDir = p.SourceFile.Dir
            End If

            tbAudioFile0.Text = audio0
            tbAudioFile1.Text = audio1

            llAudioProfile0.Text = g.ConvertPath(p.Audio0.Name)
            llAudioProfile1.Text = g.ConvertPath(p.Audio1.Name)

            p.Audio0.Delay = delay0
            p.Audio1.Delay = delay1

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

            Assistant()
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
        End Try
    End Function

    Sub SetSlider()
        Dim w = If(p.ResizeSliderMaxWidth = 0, p.SourceWidth, p.ResizeSliderMaxWidth)
        tbResize.Maximum = CInt((Calc.FixMod16(w) - 320) / p.ForcedOutputMod)
    End Sub

    Sub SetSavedProject()
        g.SavedProject = ObjectHelp.GetCopy(Of Project)(p)
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
            AddSourceFilters({"FFVideoSource", "LWLibavVideoSource", "ffms2", "LWLibavSource"}, filters)
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

        Dim td As New TaskDialog(Of VideoFilter)

        td.Title = If(p.Script.IsAviSynth, "Select an AviSynth source filter:", "Select a VapourSynth source filter:")

        For Each filter In filters
            td.AddCommand(filter.Name, filter)
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

        For Each filter In allFilters
            For Each filterName In filterNames
                If filter.Script.ToLowerInvariant.Contains(filterName.ToLowerInvariant + "(") OrElse
                    filter.Script.ToLowerInvariant.Contains(filterName.ToLowerInvariant + ".") Then

                    If filters.Where(Function(val) val.Name = filter.Name).Count = 0 Then
                        filters.Add(filter.GetCopy)
                    End If
                End If
            Next
        Next
    End Sub

    Sub OpenAnyFile(files As IEnumerable(Of String))
        files = files.Select(Function(filePath) New FileInfo(filePath).FullName)

        If files(0).Ext = "srip" Then
            OpenProject(files(0))
        ElseIf FileTypes.Video.Contains(files(0).Ext.ToLowerInvariant) Then
            files.Sort()
            OpenVideoSourceFiles(files)
        ElseIf FileTypes.Audio.Contains(files(0).Ext.ToLowerInvariant) Then
            tbAudioFile0.Text = files(0)
        Else
            files.Sort()
            OpenVideoSourceFiles(files)
        End If
    End Sub

    Sub OpenVideoSourceFile(fp As String)
        OpenVideoSourceFiles({fp})
    End Sub

    Sub OpenVideoSourceFiles(files As IEnumerable(Of String))
        OpenVideoSourceFiles(files, False)
    End Sub

    Sub OpenVideoSourceFiles(files As IEnumerable(Of String), isEncoding As Boolean)
        Dim recoverPath = g.ProjectPath
        Dim recoverProjectPath = Folder.Temp + Guid.NewGuid.ToString + ".bin"
        Dim recoverText = Text

        SafeSerialization.Serialize(p, recoverProjectPath)
        AddHandler Disposed, Sub() FileHelp.Delete(recoverProjectPath)

        Try
            files = files.Select(Function(filePath) New FileInfo(filePath).FullName).OrderBy(Function(filePath) filePath, StringComparer.InvariantCultureIgnoreCase)

            If Not g.VerifySource(files) Then
                Throw New AbortException
            End If

            For Each i In files
                Dim name = i.FileName

                If name.ToUpperInvariant Like "VTS_0#_0.VOB" Then
                    If MsgQuestion("Are you sure you want to open the file " + name + "," + BR +
                           "the first VOB file usually contains a menu.") = DialogResult.Cancel Then

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

            If p.SourceFile <> "" AndAlso Not isEncoding Then
                Dim templates = Directory.GetFiles(Folder.Template, "*.srip")

                Debug.WriteLine(templates.Length)

                If templates.Length = 1 Then
                    If Not OpenProject(templates(0), True) Then
                        Throw New AbortException
                    End If
                Else
                    If s.ShowTemplateSelection Then
                        If Not LoadTemplateWithSelectionDialog() Then
                            Throw New AbortException
                        End If
                    Else
                        If Not OpenProject() Then
                            Throw New AbortException
                        End If
                    End If
                End If
            End If

            p.SourceFiles = files.ToList
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
                        filter.Script = "ImageSource(""" + images(0).Dir + p.SourceFile.Base.Substring(0,
                            p.SourceFile.Base.Length - digitCount) + "%0" & digitCount & "d." + p.SourceFile.Ext +
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

            Dim preferredSourceFilter As VideoFilter

            If p.SourceFiles.Count = 1 AndAlso
                p.Script.Filters(0).Name = "Manual" AndAlso
                Not p.NoDialogs AndAlso Not p.BatchMode AndAlso
                Not p.SourceFile.Ext.EqualsAny("avs", "vpy") Then

                preferredSourceFilter = ShowSourceFilterSelectionDialog(files(0))
            End If

            If Not preferredSourceFilter Is Nothing Then
                Dim isVapourSynth = preferredSourceFilter.Script.Replace(" ", "").Contains("clip=core.") OrElse
                    preferredSourceFilter.Script = "#vs"

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
                    If Not Package.AviSynth.VerifyOK(True) Then
                        Throw New AbortException
                    End If

                    If p.Script.Engine = ScriptEngine.VapourSynth Then
                        p.Script = VideoScript.GetDefaults()(0)
                    End If
                End If

                p.Script.SetFilter(preferredSourceFilter.Category,
                                   preferredSourceFilter.Name,
                                   preferredSourceFilter.Script)
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

            If p.SourceVideoHdrFormat = "" Then
                p.SourceVideoHdrFormat = "SDR"
            ElseIf p.SourceVideoHdrFormat.Contains("Blu-ray / HDR10") OrElse p.SourceVideoHdrFormat.Contains("Dolby") Then
                p.SourceVideoHdrFormat = "DV"
            End If

            p.SourceVideoFormat = MediaInfo.GetVideoFormat(p.LastOriginalSourceFile)
            p.SourceVideoFormatProfile = MediaInfo.GetVideo(p.LastOriginalSourceFile, "Format_Profile")
            p.SourceVideoBitDepth = MediaInfo.GetVideo(p.LastOriginalSourceFile, "BitDepth").ToInt
            p.SourceColorSpace = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ColorSpace")
            p.SourceChromaSubsampling = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ChromaSubsampling")
            p.SourceSize = New FileInfo(p.LastOriginalSourceFile).Length
            p.SourceVideoSize = MediaInfo.GetVideo(p.LastOriginalSourceFile, "StreamSize").ToLong()
            p.SourceBitrate = CInt(MediaInfo.GetVideo(p.LastOriginalSourceFile, "BitRate").ToInt / 1000)
            p.SourceScanType = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ScanType")
            p.SourceScanOrder = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ScanOrder")

            p.VideoEncoder.SetMetaData(p.LastOriginalSourceFile)

            Dim mkvMuxer = TryCast(p.VideoEncoder.Muxer, MkvMuxer)

            If Not mkvMuxer Is Nothing AndAlso mkvMuxer.Title = "" Then
                mkvMuxer.Title = MediaInfo.GetGeneral(p.LastOriginalSourceFile, "Movie")
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

            tbTargetFile.Text = targetDir + targetName + p.VideoEncoder.Muxer.OutputExtFull

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

            If p.LastOriginalSourceFile <> p.SourceFile AndAlso
                Not FileTypes.VideoText.Contains(p.SourceFile.Ext) Then

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
                           "core = vs.get_core()" + BR +
                           "from importlib.machinery import SourceFileLoader" + BR +
                           $"SourceFileLoader('clip', r""{p.SourceFile}"").load_module()" + BR +
                           "clip = vs.get_output()"
                p.Script.Filters.Add(New VideoFilter("Source", "VS Script Import", code))
            End If

            ModifyFilters()
            FiltersListView.IsLoading = False
            FiltersListView.Load()

            RenameDVDTracks()

            If FileTypes.VideoAudio.Contains(p.LastOriginalSourceFile.Ext) Then
                p.Audio0.Streams = MediaInfo.GetAudioStreams(p.LastOriginalSourceFile)
                p.Audio1.Streams = p.Audio0.Streams
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

            DetectAudioFiles(0, True, True, True)
            DetectAudioFiles(1, True, True, True)
            DetectAudioFiles(0, True, False, True)
            DetectAudioFiles(1, True, False, True)

            DetectAudioFiles(0, True, True, False)
            DetectAudioFiles(1, True, True, False)
            DetectAudioFiles(0, True, False, False)
            DetectAudioFiles(1, True, False, False)

            DetectAudioFiles(0, False, True, False)
            DetectAudioFiles(1, False, True, False)

            DetectAudioFiles(0, False, False, False)
            DetectAudioFiles(1, False, False, False)

            If p.UseScriptAsAudioSource Then
                tbAudioFile0.Text = p.Script.Path
            Else
                If p.Audio0.File = "" AndAlso p.Audio1.File = "" AndAlso
                    FileTypes.VideoExtensionSupportsAudio(p.LastOriginalSourceFile.Ext) Then

                    Dim audioCount = MediaInfo.GetAudioCount(p.LastOriginalSourceFile)

                    If audioCount > 0 Then
                        For Each iAP In {p.Audio0, p.Audio1}
                            If TypeOf iAP Is NullAudioProfile Then
                                Continue For
                            End If

                            Dim tb = GetAudioTextBox(iAP)
                            tb.Text = p.LastOriginalSourceFile

                            If audioCount = 1 Then
                                Exit For
                            End If
                        Next

                        If Not p.Audio0.Stream Is Nothing AndAlso Not p.Audio1.Stream Is Nothing AndAlso
                            p.Audio0.Stream.ID = p.Audio1.Stream.ID Then

                            tbAudioFile1.Text = ""
                        End If
                    End If
                End If
            End If

            BlockSourceTextBoxTextChanged = True
            tbSourceFile.Text = p.SourceFile
            BlockSourceTextBoxTextChanged = False

            s.LastPosition = 0

            UpdateTargetParameters(p.Script.GetSeconds, p.Script.GetFramerate)
            DemuxVobSubSubtitles()
            ConvertBluRaySubtitles()
            ExtractForcedVobSubSubtitles()
            p.VideoEncoder.Muxer.Init()

            If p.HardcodedSubtitle Then
                g.AddHardcodedSubtitle()
            End If

            Dim isCropActive = p.Script.IsFilterActive("Crop")

            If isCropActive AndAlso (p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) = 0 Then
                p.SourceScript.Synchronize(True, True, True)

                Using proc As New Proc
                    proc.Header = "Auto Crop"
                    proc.SkipString = "%"
                    proc.Package = Package.AutoCrop
                    proc.Arguments = p.SourceScript.Path.Escape + " " & s.CropFrameCount & " " &
                        If(FrameServerHelp.IsVfwUsed, 1, 0)
                    proc.Start()

                    Dim output = proc.Log.ToString
                    Dim match = Regex.Match(proc.Log.ToString, "(\d+),(\d+),(\d+),(\d+)")

                    If match.Success Then
                        p.CropLeft = match.Groups(1).Value.ToInt
                        p.CropTop = match.Groups(2).Value.ToInt
                        p.CropRight = match.Groups(3).Value.ToInt
                        p.CropBottom = match.Groups(4).Value.ToInt
                        g.CorrectCropMod()
                        DisableCropFilter()
                    End If
                End Using
            End If

            AutoResize()

            If isCropActive Then
                g.OvercropWidth()

                If p.AutoSmartCrop Then
                    g.SmartCrop()
                End If
            End If

            If p.AutoCompCheck AndAlso p.VideoEncoder.IsCompCheckEnabled Then
                p.VideoEncoder.RunCompCheck()
            End If

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
            If Not isEncoding Then
                ProcController.Finished()
            End If
        End Try
    End Sub

    Sub ModifyFilters()
        If p.SourceFile = "" Then
            Exit Sub
        End If

        Dim profiles = If(p.Script.IsAviSynth, s.AviSynthProfiles, s.VapourSynthProfiles)

        Dim preferences = If(p.Script.IsAviSynth,
            s.AviSynthFilterPreferences, s.VapourSynthFilterPreferences)

        Dim editAVS = p.Script.IsAviSynth AndAlso p.SourceFile.Ext <> "avs"
        Dim editVS = p.Script.IsVapourSynth AndAlso p.SourceFile.Ext <> "vpy"

        If p.AutoRotation AndAlso (editAVS OrElse editVS) Then
            Dim rot = MediaInfo.GetVideo(p.SourceFile, "Rotation").ToDouble

            If rot <> 0 Then
                Dim name As String

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

                    If Not filter Is Nothing Then
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
        Catch ex As Exception
            errorMsg = ex.Message
        End Try

        If errorMsg <> "" Then
            Log.WriteHeader("Error opening source")
            Log.WriteLine(errorMsg + BR2)
            Log.WriteLine(p.SourceScript.GetFullScript)
            Log.Save()

            MsgError("Script Error", errorMsg, Handle)
            p.Script.Synchronize()
            Throw New AbortException
        End If

        If Not editAVS AndAlso Not editVS Then
            Exit Sub
        End If

        Dim sourceInfo = p.SourceScript.Info

        If s.FixFrameRate Then
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

        If p.SourceChromaSubsampling <> "4:2:0" AndAlso s.ConvertChromaSubsampling Then
            If editVS Then
                Dim sourceHeight = MediaInfo.GetVideo(p.LastOriginalSourceFile, "Height").ToInt
                Dim matrix = If(sourceHeight = 0 OrElse sourceHeight > 576, "709", "470bg")
                Dim format = If(p.SourceVideoBitDepth = 10, "YUV420P10", "YUV420P8")
                p.Script.GetFilter("Source").Script += BR + "clip = clip.resize.Bicubic(matrix_s = '" +
                    matrix + $"', format = vs.{format})"
            ElseIf editAVS AndAlso Not sourceFilter.Script.ContainsAny("ConvertToYV12", "ConvertToYUV420") AndAlso
                Not sourceFilter.Script.Contains("ConvertToYUV420") Then

                p.Script.GetFilter("Source").Script += BR + "ConvertToYUV420()"
            End If
        End If

        If Not sourceFilter.Script.Contains("Crop(") Then
            Dim sourceWidth = MediaInfo.GetVideo(p.LastOriginalSourceFile, "Width").ToInt
            Dim sourceHeight = MediaInfo.GetVideo(p.LastOriginalSourceFile, "Height").ToInt

            If sourceWidth Mod 4 <> 0 OrElse sourceHeight Mod 4 <> 0 Then
                p.CropRight = sourceWidth Mod 4
                p.CropBottom = sourceHeight Mod 4
                p.Script.ActivateFilter("Crop")
            End If
        End If
    End Sub

    Function FixFrameRate(num As Integer, den As Integer) As (num As Integer, den As Integer)
        Dim rate = num / den

        If rate < 50 AndAlso rate > 49 Then
            Return (50, 1)
        End If

        Return (num, den)
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
        If Not p.ConvertSup2Sub Then
            Exit Sub
        End If

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
        If Not p.ExtractForcedSubSubtitles Then
            Exit Sub
        End If

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
        If Not {"vob", "m2v"}.Contains(p.LastOriginalSourceFile.Ext) Then
            Exit Sub
        End If

        Dim ifoPath = GetIfoFile()

        If ifoPath = "" Then
            Exit Sub
        End If

        If File.Exists(p.TempDir + p.SourceFile.Base + ".idx") Then
            Exit Sub
        End If

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
                            p.TempDir + p.SourceFile.Base + BR &
                            (i + 1) & BR +
                            "1" + BR +
                            "ALL" + BR +
                            "CLOSE"

                        Dim fileContent = p.TempDir + p.TargetFile.Base + "_vsrip.txt"
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
                    laTip.SetFontSize(8 * s.UIScaleFactor)
                Else
                    laTip.SetFontSize(9 * s.UIScaleFactor)
                End If
            End If

            laTip.Text = message
            Return True
        End If
    End Function

    Function Assistant(Optional refreshScript As Boolean = True) As Boolean
        If SkipAssistant Then
            Return False
        End If

        If ThemeRefresh Then
            ApplyTheme()
            ThemeRefresh = False
        End If

        If refreshScript Then
            p.Script.Synchronize(False, False)
        End If

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

        lAspectRatioError.Text = Calc.GetAspectRatioError.ToString("f2") + "%"

        If isCropped Then
            lCrop.Text = cropw.ToString() + "/" + croph.ToString()
        Else
            lCrop.Text = "disabled"
        End If

        Dim widthZoom = p.TargetWidth / cropw * 100
        Dim heightZoom = p.TargetHeight / croph * 100

        lZoom.Text = widthZoom.ToString("f1") + "/" + heightZoom.ToString("f1")
        lPixel.Text = CInt(p.TargetWidth * p.TargetHeight).ToString

        Dim trackBarValue = CInt((p.TargetWidth - 320) / p.ForcedOutputMod)

        If trackBarValue < tbResize.Minimum Then
            trackBarValue = tbResize.Minimum
        End If

        If trackBarValue > tbResize.Maximum Then
            trackBarValue = tbResize.Maximum
        End If

        tbResize.Value = trackBarValue

        Dim par = Calc.GetTargetPAR

        If Calc.IsARSignalingRequired OrElse (par.X = 1 AndAlso par.Y = 1) Then
            lPAR.Text = par.X & ":" & par.Y
        Else
            lPAR.Text = "n/a"
        End If

        lDAR.Text = Calc.GetTargetDAR.ToString.Shorten(8)
        lSAR.Text = (p.TargetWidth / p.TargetHeight).ToString.Shorten(8)
        lSourceDar.Text = Calc.GetSourceDAR.ToString.Shorten(8)
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
                If(p.SourceBitrate > 0, (p.SourceBitrate / 1000).ToString("f1") + "Mb/s", ""),
                p.SourceFrameRate.ToString.Shorten(9) + "fps",
                p.SourceVideoFormat, p.SourceVideoFormatProfile)

            lSource2.Text = lSource1.GetMaxTextSpace(
                p.SourceWidth.ToString + "x" + p.SourceHeight.ToString, p.SourceColorSpace,
                p.SourceChromaSubsampling, If(p.SourceVideoBitDepth <> 0, p.SourceVideoBitDepth & "Bits", ""),
                p.SourceVideoHdrFormat,
                p.SourceScanType, If(p.SourceScanType = "Interlaced", p.SourceScanOrder, ""))

            lTarget1.Text = lSource1.GetMaxTextSpace(g.GetTimeString(p.TargetSeconds),
                p.TargetFrameRate.ToString.Shorten(9) + "fps", p.Script.Info.Width & "x" & p.Script.Info.Height,
                "Audio Bitrate: " & CInt(Calc.GetAudioBitrate))

            If p.VideoEncoder.IsCompCheckEnabled Then
                laTarget2.Text = lSource1.GetMaxTextSpace(
                    "Quality: " & CInt(Calc.GetPercent).ToString() + " %",
                    "Compressibility: " + p.Compressibility.ToString("f2"))
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

            If Not param Is Nothing AndAlso param.Value > 0 AndAlso
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
                If (TypeOf p.Audio0 Is NullAudioProfile OrElse p.Audio0.File = "") AndAlso (TypeOf p.Audio1 Is NullAudioProfile OrElse p.Audio1.File = "") Then
                    If ProcessTip("There will be no audio in the output file.") Then
                        Return Warn("No audio", tbAudioFile0, tbAudioFile1)
                    End If
                End If
            End If

            If (p.Audio0.File <> "" AndAlso p.Audio0.File = p.Audio1.File AndAlso p.Audio0.Stream Is Nothing) OrElse
                (Not p.Audio0.Stream Is Nothing AndAlso Not p.Audio1.Stream Is Nothing AndAlso
                p.Audio0.Stream.StreamOrder = p.Audio1.Stream.StreamOrder) Then

                If ProcessTip("The first and second audio source files or streams are identical.") Then
                    Return Warn("Invalid Audio Settings", tbAudioFile0, tbAudioFile1)
                End If
            End If

            If Not p.VideoEncoder.Muxer.IsSupported(p.VideoEncoder.OutputExt) Then
                If ProcessTip("The encoder outputs '" + p.VideoEncoder.OutputExt + "' but the container '" + p.VideoEncoder.Muxer.Name + "' supports only " + p.VideoEncoder.Muxer.SupportedInputTypes.Join(", ") + ".") Then
                    Return Block("Encoder conflicts with container", lgbEncoder.Label, llMuxer)
                End If
            End If

            For Each ap In AudioProfile.GetProfiles
                If ap.File = "" Then
                    Continue For
                End If

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

            If Not p.VideoEncoder.GetError Is Nothing Then
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
                If p.RemindToCut AndAlso Not TypeOf p.VideoEncoder Is NullEncoder AndAlso
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

            If p.Script.Info.Width Mod p.ForcedOutputMod <> 0 Then
                If ProcessTip("Change output width to be divisible by " & p.ForcedOutputMod &
                              " or customize:" + BR + "Options > Image > Output Mod") Then
                    CanIgnoreTip = Not p.AutoCorrectCropValues
                    Return Warn("Invalid Target Width", tbTargetWidth, lSAR)
                End If
            End If

            If p.Script.Info.Height Mod p.ForcedOutputMod <> 0 Then
                If ProcessTip("Change output height to be divisible by " & p.ForcedOutputMod &
                              " or customize:" + BR + "Options > Image > Output Mod") Then
                    CanIgnoreTip = Not p.AutoCorrectCropValues
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
                    If Not i.Path.Ext.EqualsAny("idx", "srt") Then
                        If ProcessTip("MP4 supports only SRT and IDX subtitles.") Then
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

        If laTip.Font.Size <> (9 * s.UIScaleFactor) Then
            laTip.SetFontSize(9 * s.UIScaleFactor)
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

        Highlight(controls)
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
        If controls Is Nothing Then
            Return
        End If

        If Not controls.NothingOrEmpty Then
            ThemeRefresh = True
        End If

        Dim theme = ThemeManager.CurrentTheme

        If highlight Then
            laTip.BackColor = theme.MainForm.laTipBackHighlightColor
            laTip.ForeColor = theme.MainForm.laTipForeHighlightColor
        Else
            laTip.BackColor = theme.MainForm.laTipBackColor
            laTip.ForeColor = theme.MainForm.laTipForeColor
        End If

        For Each control In controls.OfType(Of Label)
            control.BackColor = theme.General.Controls.Label.BackHighlightColor
            control.ForeColor = theme.General.Controls.Label.ForeHighlightColor
        Next

        For Each control In controls.OfType(Of ButtonLabel)
            If TypeOf control.Parent Is UserControl Then
                control.BackColor = theme.General.Controls.ListView.BackHighlightColor
            Else
                control.BackColor = theme.General.Controls.ButtonLabel.BackHighlightColor
            End If

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

    Function GetAudioTextBox(ap As AudioProfile) As TextEdit
        If ap Is p.Audio0 Then Return tbAudioFile0
        If ap Is p.Audio1 Then Return tbAudioFile1
    End Function

    Dim BlockAudioTextChanged As Boolean

    Sub AudioTextChanged(tb As TextEdit, ap As AudioProfile)
        If BlockAudioTextChanged Then
            Exit Sub
        End If

        If tb.Text.Contains(":\") OrElse tb.Text.StartsWith("\\") Then
            If tb.Text <> ap.File Then
                ap.File = tb.Text

                If FileTypes.Audio.Contains(ap.File.Ext) Then
                    If Not p.Script.GetFilter("Source").Script.Contains("DirectShowSource") Then
                        ap.Delay = g.ExtractDelay(ap.File)
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
            tb.Text = ap.DisplayName

            If tb Is tbAudioFile0 Then
                llAudioProfile0.Text = g.ConvertPath(ap.Name)
            Else
                llAudioProfile1.Text = g.ConvertPath(ap.Name)
            End If

            BlockAudioTextChanged = False
        ElseIf tb.Text = "" Then
            ap.File = ""
            UpdateSizeOrBitrate()
        End If
    End Sub

    Sub tbAudioFile0_TextChanged() Handles tbAudioFile0.TextChanged
        AudioTextChanged(tbAudioFile0, p.Audio0)
    End Sub

    Sub tbAudioFile1_TextChanged() Handles tbAudioFile1.TextChanged
        AudioTextChanged(tbAudioFile1, p.Audio1)
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
            If Not i.Active AndAlso (i.SourceFilters.NothingOrEmpty OrElse
                Not srcScript.ContainsAny(i.SourceFilters.Select(Function(val) val.ToLowerInvariant + "(").ToArray)) Then

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
                                    Not iFile.Base.EndsWith("_out") AndAlso
                                    Not iFile.Base.Contains("_cut_") Then

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

    Private BlockIndexingRecursion As Boolean = False

    Sub Indexing()
        If p.SourceFile.Ext.EqualsAny("avs", "vpy") Then
            Exit Sub
        End If

        Dim codeLower = p.Script.GetFilter("Source").Script.ToLowerInvariant

        If codeLower.Contains("ffvideosource(") OrElse codeLower.Contains("ffms2.source") Then
            If FileTypes.VideoIndex.Contains(p.SourceFile.Ext) Then
                p.SourceFile = p.LastOriginalSourceFile
                BlockSourceTextBoxTextChanged = True
                tbSourceFile.Text = p.SourceFile
                BlockSourceTextBoxTextChanged = False
            End If

            If codeLower.Contains("cachefile") AndAlso p.TempDir.EndsWithEx("_temp\") Then
                g.ffmsindex(p.SourceFile, p.TempDir + g.GetSourceBase + ".ffindex")
            Else
                g.ffmsindex(p.SourceFile, p.SourceFile + ".ffindex")
            End If
        ElseIf codeLower.Contains("lwlibavvideosource(") OrElse codeLower.Contains("lwlibavsource(") Then
            If FileTypes.VideoIndex.Contains(p.SourceFile.Ext) Then
                p.SourceFile = p.LastOriginalSourceFile
                BlockSourceTextBoxTextChanged = True
                tbSourceFile.Text = p.SourceFile
                BlockSourceTextBoxTextChanged = False
            End If

            If Not File.Exists(p.SourceFile + ".lwi") AndAlso
                Not File.Exists(p.TempDir + g.GetSourceBase + ".lwi") AndAlso
                File.Exists(p.Script.Path) AndAlso Not FileTypes.VideoText.Contains(p.SourceFile.Ext) Then

                Using proc As New Proc
                    proc.Header = "Index LWLibav"
                    proc.Encoding = Encoding.UTF8
                    proc.SkipString = "Creating lwi"

                    If p.Script.IsAviSynth Then
                        proc.File = Package.ffmpeg.Path
                        proc.Arguments = "-i " + p.Script.Path.Escape + " -hide_banner"
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

    Function LoadTemplateWithSelectionDialog() As Boolean
        Using td As New TaskDialog(Of String)
            td.Title = "Please select a template"

            For Each fp In Directory.GetFiles(Folder.Template, "*.srip")
                td.AddCommand(fp.Base, fp)
            Next

            If td.Show <> "" Then
                Return OpenProject(td.SelectedValue, True)
            End If
        End Using
    End Function

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

        Using form As New SimpleSettingsForm("Settings")
            Dim ui = form.SimpleUI
            ui.Store = s

            '################# General
            ui.CreateFlowPage("General")

            Dim b = ui.AddBool()
            b.Text = "Check for updates once per day"
            b.Field = NameOf(s.CheckForUpdates)

            b = ui.AddBool
            b.Text = "Include Dev builds for update check"
            b.Field = NameOf(s.CheckForUpdatesDev)

            b = ui.AddBool
            b.Text = "Save projects automatically"
            b.Field = NameOf(s.AutoSaveProject)

            b = ui.AddBool()
            b.Text = "Show template selection when loading new files"
            b.Field = NameOf(s.ShowTemplateSelection)

            b = ui.AddBool()
            b.Text = "Reverse mouse wheel video seek direction"
            b.Field = NameOf(s.ReverseVideoScrollDirection)

            b = ui.AddBool()
            b.Text = "Write Event Commands to log file"
            b.Field = NameOf(s.LogEventCommand)

            b = ui.AddBool()
            b.Text = "Enable debug logging"
            b.Field = NameOf(s.WriteDebugLog)

            Dim mb = ui.AddMenu(Of String)()
            mb.Text = "Startup Template"
            mb.Help = "Template loaded when StaxRip starts."
            mb.Field = NameOf(s.StartupTemplate)
            mb.Expanded = True
            mb.Add(From i In Directory.GetFiles(Folder.Template) Select i.Base)

            Dim n = ui.AddNum()
            n.Text = "Number of log files to keep"
            n.Help = "Log files can be found at: Tools > Folders > Log Files"
            n.Config = {0, Integer.MaxValue}
            n.Field = NameOf(s.LogFileNum)

            n = ui.AddNum()
            n.Text = "Number of most recently used projects to keep"
            n.Help = "MRU list shown in the main menu under: File > Recent Projects"
            n.Config = {0, 15}
            n.Field = NameOf(s.ProjectsMruNum)

            n = ui.AddNum()
            n.Text = "Maximum number of parallel processes"
            n.Help = "Maximum number of parallel processes used for audio and video processing. Chunk encoding can be enabled in the x265 dialog."
            n.Config = {1, 16}
            n.Field = NameOf(s.ParallelProcsNum)

            n = ui.AddNum()
            n.Text = "Preview size compared to screen size (percent)"
            n.Config = {10, 90, 5}
            n.Field = NameOf(s.PreviewSize)

            '############# System
            Dim systemPage = ui.CreateFlowPage("System", True)

            Dim procPriority = ui.AddMenu(Of ProcessPriorityClass)
            procPriority.Text = "Process Priority"
            procPriority.Help = "Process priority of the applications StaxRip launches."
            procPriority.Field = NameOf(s.ProcessPriority)

            Dim tempDelete = ui.AddMenu(Of DeleteMode)
            tempDelete.Text = "Delete temp files"
            tempDelete.Field = NameOf(s.DeleteTempFilesMode)

            n = ui.AddNum
            n.Text = "Minimum Disk Space"
            n.Help = "Minimum allowed disk space in GB before StaxRip shows an error message."
            n.Config = {0, 10000}
            n.Field = NameOf(s.MinimumDiskSpace)

            n = ui.AddNum
            n.Text = "Shutdown Timeout"
            n.Help = "Timeout in seconds before the shutdown is executed."
            n.Field = NameOf(s.ShutdownTimeout)

            b = ui.AddBool
            b.Text = "Extend error messages with the help of 'Err'"
            b.Field = NameOf(s.ErrorMessageExtendedByErr)

            b = ui.AddBool
            b.Text = "Force closing running apps when shutdown and for hybrid mode"
            b.Field = NameOf(s.ShutdownForce)

            b = ui.AddBool
            b.Text = "Prevent system entering standby mode while encoding"
            b.Field = NameOf(s.PreventStandby)

            b = ui.AddBool
            b.Text = "Minimize processing dialog to tray"
            b.Field = NameOf(s.MinimizeToTray)

            '############# Video
            Dim videoPage = ui.CreateFlowPage("Video")

            b = ui.AddBool
            b.Text = "Add filter to convert chroma subsampling to 4:2:0"
            b.Help = "After a source is loaded, automatically add a filter to convert chroma subsampling to 4:2:0"
            b.Field = NameOf(s.ConvertChromaSubsampling)

            b = ui.AddBool
            b.Text = "Add filter to automatically correct the frame rate."
            b.Field = NameOf(s.FixFrameRate)

            n = ui.AddNum
            n.Text = "Number of frames used for auto crop"
            n.Config = {5, 200}
            n.Field = NameOf(s.CropFrameCount)

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

            '################# User Interface
            ui.CreateFlowPage("User Interface", True)

            Dim theme = ui.AddMenu(Of String)
            theme.Text = "Theme"
            theme.Expanded = True
            theme.Field = NameOf(s.ThemeName)
            theme.Add(ThemeManager.Themes.Select(Function(x) x.Name))
            theme.Button.ShowPath = True
            theme.Button.SaveAction = Sub(value) ThemeManager.SetCurrentTheme(value)
            theme.Button.ValueChangedAction = Sub(value) ThemeManager.SetCurrentTheme(value)

            Dim codeFont = ui.AddTextButton()
            codeFont.Text = "Console Font"
            codeFont.Expanded = True
            codeFont.Field = NameOf(s.CodeFont)
            codeFont.ClickAction = Sub()
                                       Using td As New TaskDialog(Of FontFamily)
                                           td.Title = "Choose a monospaced font"
                                           td.Symbol = Symbol.Font

                                           For Each ff In FontFamily.Families.Where(Function(x) Not x.Name.ToLowerEx().ContainsAny(" mdl2", " assets", "marlett", "ms outlook", "mt extra", "wingdings 2") AndAlso x.IsStyleAvailable(FontStyle.Regular) AndAlso x.IsMonospace())
                                               td.AddCommand(ff.Name, ff)
                                           Next

                                           If td.Show IsNot Nothing Then
                                               codeFont.Edit.Text = td.SelectedText
                                           End If
                                       End Using
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
            tb.BrowseFile("ico|*.ico", Folder.Startup + "Apps\Icons")
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
            ui.AddControlPage(New PreprocessingControl, "Preprocessing").FormSizeScaleFactor = New Size(40, 22)
            ui.FormSizeScaleFactor = New Size(33, 22)

            '############# Source Filters
            Dim bsAVS = AddFilterPreferences(ui, "Source Filters | AviSynth",
                    s.AviSynthFilterPreferences, s.AviSynthProfiles)

            Dim bsVS = AddFilterPreferences(ui, "Source Filters | VapourSynth",
                    s.VapourSynthFilterPreferences, s.VapourSynthProfiles)

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

                If Not Icon Is g.Icon Then
                    Icon = g.Icon
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

        Dim ret2 As New BindingSource

        ret2.DataSource = ObjectHelp.GetCopy(
            New StringPairList(preferences.Where(
                               Function(a) filterNames.Contains(a.Value) AndAlso a.Name <> "")))

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
        Else
            g.ProjectPath = path
        End If

        Try
            SafeSerialization.Serialize(p, path)
            SetSavedProject()
            Text = path.Base + " - " + Application.ProductName + " v" + Application.ProductVersion
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
            Dim box As New InputBox
            box.Text = "Enter the name of the template."
            box.Title = "Save Template"
            box.Value = p.TemplateName
            box.CheckBoxText = "Load template on startup"

            If box.Show = DialogResult.OK Then
                p.TemplateName = box.Value.RemoveChars(Path.GetInvalidFileNameChars)
                SaveProjectPath(Folder.Template + p.TemplateName + ".srip")
                UpdateTemplatesMenuAsync()

                If box.Checked Then
                    s.StartupTemplate = box.Value
                End If
            End If
        Else
            MsgWarn("A template cannot be created after a source file was opened.")
        End If
    End Sub

    <Command("Starts the compressibility check.")>
    Sub StartCompCheck()
        p.VideoEncoder.RunCompCheck()
    End Sub

    <Command("Exits StaxRip")>
    Sub [Exit]()
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
        Dim sb As New SelectionBox(Of VideoEncoder)
        sb.Title = "Add New Profile"
        sb.Text = "Please select a profile from the defaults."
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
            If i.CustomMenuItem.MethodName = "DynamicMenuItem" AndAlso
                i.CustomMenuItem.Parameters(0).Equals(id) Then

                i.DropDownItems.ClearAndDisplose

                Select Case id
                    Case DynamicMenuItemID.EncoderProfiles
                        g.PopulateProfileMenu(i.DropDownItems, s.VideoEncoderProfiles, AddressOf ShowEncoderProfilesDialog, AddressOf g.LoadVideoEncoder)
                    Case DynamicMenuItemID.MuxerProfiles
                        g.PopulateProfileMenu(i.DropDownItems, s.MuxerProfiles, AddressOf ShowMuxerProfilesDialog, AddressOf p.VideoEncoder.LoadMuxer)
                    Case DynamicMenuItemID.Audio1Profiles
                        g.PopulateProfileMenu(i.DropDownItems, s.AudioProfiles, Sub() ShowAudioProfilesDialog(0), AddressOf g.LoadAudioProfile0)
                    Case DynamicMenuItemID.Audio2Profiles
                        g.PopulateProfileMenu(i.DropDownItems, s.AudioProfiles, Sub() ShowAudioProfilesDialog(1), AddressOf g.LoadAudioProfile1)
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

            If Not g.EnableFilter("Crop") Then
                If p.Script.IsAviSynth Then
                    p.Script.InsertAfter("Source", New VideoFilter("Crop", "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)"))
                Else
                    p.Script.InsertAfter("Source", New VideoFilter("Crop", "Crop", "clip = core.std.Crop(clip, %crop_left%, %crop_right%, %crop_top%, %crop_bottom%)"))
                End If
            End If

            Using form As New CropForm
                form.ShowDialog()
            End Using

            DisableCropFilter()
            Assistant()
        End If
    End Sub

    <Command("Dialog to preview or cut the video.")>
    Sub ShowPreview()
        If p.SourceFile = "" Then
            ShowOpenSourceDialog()
        Else
            If Not g.VerifyRequirements OrElse Not g.IsValidSource Then
                Exit Sub
            End If

            Dim script = p.Script.GetNewScript
            script.Path = p.TempDir + p.TargetFile.Base + "_view." + script.FileType
            script.RemoveFilter("Cutting")

            If script.GetError <> "" Then
                MsgError("Script Error", script.GetError)
                Exit Sub
            End If

            Dim form As New PreviewForm(script)
            form.Show()
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
        FileHelp.Delete(Folder.Settings + "Jobs.dat")
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
                    g.LoadAudioProfile0(i)
                End If
            Next
        End If

        If audioProfile2 <> "" Then
            For Each i In s.AudioProfiles
                If i.Name = audioProfile2 Then
                    g.LoadAudioProfile1(i)
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
            If AbortDueToLowDiskSpace() Then
                Exit Sub
            End If

            If Not TypeOf p.VideoEncoder Is NullEncoder AndAlso File.Exists(p.VideoEncoder.OutputPath) Then
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

            If (p.Audio0.File <> "" AndAlso (TypeOf p.Audio0 Is GUIAudioProfile OrElse
                TypeOf p.Audio0 Is BatchAudioProfile) AndAlso File.Exists(p.Audio0.GetOutputFile)) OrElse
                (p.Audio1.File <> "" AndAlso (TypeOf p.Audio1 Is GUIAudioProfile OrElse
                TypeOf p.Audio1 Is BatchAudioProfile) AndAlso File.Exists(p.Audio1.GetOutputFile)) Then

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

        Dim jobPath = JobManager.GetJobPath()
        SaveProjectPath(jobPath)
        JobManager.AddJob(jobPath, jobPath, position)

        If showConfirmation Then
            MsgInfo("Job added")
        End If

        If templateName <> "" Then
            LoadProject(Folder.Template + templateName + ".srip")
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

            form.ScaleClientSize(33, 22)

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

            n = ui.AddNum()
            n.Text = "Resize slider width"
            n.Config = {0, Integer.MaxValue, 64}
            n.Field = NameOf(p.ResizeSliderMaxWidth)

            ui.AddLabel(n, "(0 = auto)")

            Dim m = ui.AddMenu(Of Integer)()
            m.Text = "Output Mod"
            m.Add(2, 4, 8, 16)
            m.Field = NameOf(p.ForcedOutputMod)


            '   ----------------------------------------------------------------
            ui.CreateFlowPage("Image | Aspect Ratio", True)

            Dim b = ui.AddBool()
            b.Text = "Use ITU-R BT.601 compliant aspect ratio"
            b.Help = "Calculates the aspect ratio according to ITU-R BT.601 standard. "
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
            Dim cropPage = ui.CreateFlowPage("Image | Crop")

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

            ui.AddLine(cropPage, "Crop Values")

            Dim eb = ui.AddEmptyBlock(cropPage)

            ui.AddLabel(eb, "Left:", 2)

            Dim te = ui.AddEdit(eb)
            te.Text = p.CropLeft.ToString
            te.WidthFactor = 3
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value)
                                If value.IsInt Then
                                    p.CropLeft = CInt(value)
                                End If
                            End Sub

            Dim l = ui.AddLabel(eb, "Right:", 4)

            te = ui.AddEdit(eb)
            te.Text = p.CropRight.ToString
            te.WidthFactor = 3
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value)
                                If value.IsInt Then
                                    p.CropRight = CInt(value)
                                End If
                            End Sub

            eb = ui.AddEmptyBlock(cropPage)

            ui.AddLabel(eb, "Top:", 2)

            te = ui.AddEdit(eb)
            te.Text = p.CropTop.ToString
            te.WidthFactor = 3
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value)
                                If value.IsInt Then
                                    p.CropTop = CInt(value)
                                End If
                            End Sub

            l = ui.AddLabel(eb, "Bottom:", 4)

            te = ui.AddEdit(eb)
            te.Text = p.CropBottom.ToString
            te.WidthFactor = 3
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value)
                                If value.IsInt Then
                                    p.CropBottom = CInt(value)
                                End If
                            End Sub


            '   ----------------------------------------------------------------
            Dim videoPage = ui.CreateFlowPage("Video", True)

            Dim videoExist = ui.AddMenu(Of FileExistMode)
            Dim demuxVideo = ui.AddBool()

            videoExist.Text = "Existing Video Output"
            videoExist.Help = "What to do in case the video encoding output file already exists from a previous job run, skip and reuse or re-encode and overwrite. The 'Copy/Mux' video encoder profile is also capable of reusing existing video encoder output.'"
            videoExist.Field = NameOf(p.FileExistVideo)

            demuxVideo.Text = "Demux Video"
            demuxVideo.Checked = p.DemuxVideo
            demuxVideo.SaveAction = Sub(val) p.DemuxVideo = val

            b = ui.AddBool
            b.Text = "Import VUI metadata"
            b.Help = "Imports VUI metadata such as HDR from the source file to the video encoder."
            b.Field = NameOf(p.ImportVUIMetadata)

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
            Dim chaptersPage = ui.CreateFlowPage("Chapters")

            b = ui.AddBool(chaptersPage)
            b.Text = "Demux Chapters"
            b.Checked = p.DemuxChapters
            b.SaveAction = Sub(val) p.DemuxChapters = val


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

            Dim thumbsHeaderBackColor As SimpleUI.ColorPickerBlock

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

            l = ui.AddLabel(pathPage, If(s.DeleteTempFilesMode = DeleteMode.Disabled, "Temp Files Folder:", "Temp Files Folder: (MUST end with '_temp\' for Auto-Deletion!)"))
            l.Help = "Leave empty to use the source file folder."
            l.MarginTop = Font.Height

            Dim tempDirFunc = Function()
                                  Dim tempDir = g.BrowseFolder(p.TempDir)

                                  If tempDir <> "" Then
                                      Return tempDir.FixDir + "%source_name%_temp"
                                  End If
                              End Function

            tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.TempDir
            tm.Edit.SaveAction = Sub(value) p.TempDir = value
            tm.AddMenu("Browse Folder...", tempDirFunc)
            tm.AddMenu("Source File Directory", "%source_dir%%source_name%_temp")
            tm.AddMenu("Macros...", macroAction)

            l = ui.AddLabel(pathPage, "Default Thumbnails Path without extension:")
            l.Help = "Leave empty to save it next to the video file"
            l.MarginTop = Font.Height

            tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.ThumbnailerSettings.GetString("ImageFilePathWithoutExtension", "")
            tm.Edit.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("ImageFilePathWithoutExtension", value)
            tm.AddMenu("Path of target file without extension + Postfix", "%target_dir%%target_name%_Thumbnail")
            tm.AddMenu("Path of target file without extension", "%target_dir%%target_name%")
            tm.AddMenu("Macros...", macroAction)



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


            '   ----------------------------------------------------------------
            Dim miscPage = ui.CreateFlowPage("Misc")
            miscPage.SuspendLayout()

            b = ui.AddBool(miscPage)
            b.Text = "Hide dialogs asking to demux, source filter etc."
            b.Checked = p.NoDialogs
            b.SaveAction = Sub(value) p.NoDialogs = value

            b = ui.AddBool(miscPage)
            b.Text = "Extract timestamps from VFR MKV files"
            b.Checked = p.ExtractTimestamps
            b.SaveAction = Sub(value) p.ExtractTimestamps = value

            b = ui.AddBool(miscPage)
            b.Text = "Use source file folder for temp files"
            b.Checked = p.NoTempDir
            b.SaveAction = Sub(value) p.NoTempDir = value

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

            If pagePath <> "" Then
                ui.ShowPage(pagePath)
            Else
                ui.SelectLast("last options page")
            End If

            If form.ShowDialog() = DialogResult.OK Then
                ui.Save()

                If p.CompCheckPercentage < 1 OrElse p.CompCheckPercentage > 25 Then
                    p.CompCheckPercentage = 5
                End If

                If p.CompCheckTestblockSeconds < 0.5 OrElse p.CompCheckTestblockSeconds > 10.0 Then
                    p.CompCheckTestblockSeconds = 2.0
                End If

                If p.TempDir <> "" Then
                    p.TempDir = p.TempDir.FixDir

                    'If s.DeleteTempFilesMode <> DeleteMode.Disabled AndAlso p.TempDir.EndsWith("_temp\") Then
                    '    MsgWarn("Temp Files Folder", "The folder description, you have chosen, does not end with '_temp\'! " + BR2 +
                    '            "The folder description must end with '_temp\' in order to automatically delete the folder, otherwise no files are going to be deleted. " +
                    '            "This is a safety feature to prevent you from unintentionally data loss. ")
                    'End If
                End If

                UpdateSizeOrBitrate()
                tbBitrate_TextChanged()
                SetSlider()
                Assistant()
            End If

            ui.SaveLast("last options page")
        End Using
    End Sub

    Sub DisableCropFilter()
        Dim f = p.Script.GetFilter("Crop")

        If Not f Is Nothing AndAlso CInt(p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) = 0 Then
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
            dialog.Text = "Filter Profiles"
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

        ret.Add("File|Open Video Source File...", NameOf(ShowOpenSourceDialog), Keys.O Or Keys.Control)
        ret.Add("File|-")
        ret.Add("File|Open Project...", NameOf(ShowFileBrowserToOpenProject))
        ret.Add("File|Save Project", NameOf(SaveProject), Keys.S Or Keys.Control, Symbol.Save)
        ret.Add("File|Save Project As...", NameOf(SaveProjectAs))
        ret.Add("File|Save Project As Template...", NameOf(SaveProjectAsTemplate))
        ret.Add("File|-")
        ret.Add("File|Project Templates", NameOf(g.DefaultCommands.DynamicMenuItem), {DynamicMenuItemID.TemplateProjects})
        ret.Add("File|Recent Projects", NameOf(g.DefaultCommands.DynamicMenuItem), {DynamicMenuItemID.RecentProjects})

        ret.Add("Crop", NameOf(ShowCropDialog), Keys.F4)
        ret.Add("Preview", NameOf(ShowPreview), Keys.F5)

        ret.Add("Options", NameOf(ShowOptionsDialog), Keys.F8)

        ret.Add("Tools|Jobs...", NameOf(ShowJobsDialog), Keys.F6, Symbol.MultiSelectLegacy)
        ret.Add("Tools|Log File", NameOf(g.DefaultCommands.ShowLogFile), Keys.F7, Symbol.Page)
        ret.Add("Tools|Folders", Symbol.Folder)
        ret.Add("Tools|Folders|Log Files", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%settings_dir%Log Files"""})
        ret.Add("Tools|Folders|Plugins", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%plugin_dir%"""})
        ret.Add("Tools|Folders|Programs", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%programs_dir%"""})
        ret.Add("Tools|Folders|Scripts", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%script_dir%"""})
        ret.Add("Tools|Folders|Settings", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%settings_dir%"""})
        ret.Add("Tools|Folders|Source", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%source_dir%"""})
        ret.Add("Tools|Folders|Startup", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%startup_dir%"""})
        ret.Add("Tools|Folders|System", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%system_dir%"""})
        ret.Add("Tools|Folders|Target", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%target_dir%"""})
        ret.Add("Tools|Folders|Temp", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%temp_dir%"""})
        ret.Add("Tools|Folders|Templates", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%settings_dir%Templates"""})
        ret.Add("Tools|Folders|Working", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%working_dir%"""})

        ret.Add("Tools|Scripts", NameOf(g.DefaultCommands.DynamicMenuItem), Symbol.Code, {DynamicMenuItemID.Scripts})

        ret.Add("Tools|Advanced", Symbol.More)
        ret.Add("Tools|Advanced|Add Hardcoded Subtitle...", NameOf(ShowHardcodedSubtitleDialog), Keys.Control Or Keys.H)
        ret.Add("Tools|Advanced|Script Info...", NameOf(ShowScriptInfo), Keys.F2)
        ret.Add("Tools|Advanced|Advanced Script Info...", NameOf(ShowAdvancedScriptInfo), Keys.Control Or Keys.F2)
        ret.Add("Tools|Advanced|Demux...", NameOf(g.DefaultCommands.ShowDemuxTool))
        ret.Add("Tools|Advanced|Video Comparison...", NameOf(ShowVideoComparison))
        ret.Add("Tools|Advanced|-")
        ret.Add("Tools|Advanced|Event Command...", NameOf(ShowEventCommandsDialog), Symbol.LightningBolt)
        ret.Add("Tools|Advanced|Reset Settings...", NameOf(g.DefaultCommands.ResetSettings))
        ret.Add("Tools|Advanced|-")
        ret.Add("Tools|Advanced|Command Prompt", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.fa_terminal, {"cmd.exe", False, False, False, "%working_dir%"})
        ret.Add("Tools|Advanced|PowerShell Terminal", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.fa_terminal, {"powershell.exe -nologo -executionpolicy bypass", False, False, False, "%working_dir%"})

        If g.IsWindowsTerminalAvailable Then
            ret.Add("Tools|Advanced|Windows Terminal", NameOf(g.DefaultCommands.ExecuteCommandLine), Keys.Control Or Keys.T, Symbol.fa_terminal, {"wt.exe", False, False, False, "%working_dir%"})
        End If

        ret.Add("Tools|Advanced|-")
        ret.Add("Tools|Advanced|Generate Wiki Content", NameOf(g.DefaultCommands.GenerateWikiContent), Keys.Control Or Keys.F12)
        ret.Add("Tools|Advanced|Ingest HDR", NameOf(g.DefaultCommands.SaveMKVHDR))

        If g.IsDevelopmentPC Then
            ret.Add("Tools|Advanced|Test...", NameOf(g.DefaultCommands.Test), Keys.F12)
        End If

        ret.Add("Tools|-")
        ret.Add("Tools|Edit Menu...", NameOf(ShowMainMenuEditor))
        ret.Add("Tools|Settings...", NameOf(ShowSettingsDialog), Keys.F3, Symbol.Settings, {""})

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
        ret.Add("Apps|Manage...", NameOf(ShowAppsDialog), Keys.F9)

        ret.Add("Help|Documentation", NameOf(g.DefaultCommands.ExecuteCommandLine), Keys.F1, Symbol.Help, {"https://github.com/staxrip/staxrip/wiki"})
        ret.Add("Help|Website", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.Globe, {"https://github.com/staxrip/staxrip"})
        ret.Add("Help|Apps", NameOf(g.DefaultCommands.DynamicMenuItem), {DynamicMenuItemID.HelpApplications})
        ret.Add("Help|Check for Updates", NameOf(g.DefaultCommands.CheckForUpdate))
        ret.Add("Help|-")
        ret.Add("Help|Info...", NameOf(g.DefaultCommands.OpenHelpTopic), Symbol.Info, {"info"})

        Return ret
    End Function

    <Command("Shows a dialog to add a hardcoded subtitle.")>
    Sub ShowHardcodedSubtitleDialog()
        Using dialog As New OpenFileDialog
            dialog.SetFilter(FileTypes.SubtitleExludingContainers)
            dialog.SetInitDir(p.TempDir)

            If dialog.ShowDialog = DialogResult.OK Then
                If dialog.FileName.Ext = "idx" Then
                    Dim subs = Subtitle.Create(dialog.FileName)

                    If subs.Count = 0 Then
                        MsgInfo("No subtitles found.")
                        Exit Sub
                    End If

                    Dim sb As New SelectionBox(Of Subtitle)
                    sb.Title = "Language"
                    sb.Text = "Please select a subtitle."

                    For Each i In subs
                        sb.AddItem(i.Language.Name, i)
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

        If Not g.EnableFilter("Resize") Then
            If p.Script.IsAviSynth Then
                p.Script.AddFilter(New VideoFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)"))
            Else
                p.Script.AddFilter(New VideoFilter("Resize", "Bicubic", "clip = core.resize.Bicubic(clip, %target_width%, %target_height%)"))
            End If
        End If

        tbTargetWidth.Text = CInt(320 + tbResize.Value * p.ForcedOutputMod).ToString
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
        If Not g.EnableFilter("Resize") Then
            p.Script.AddFilter(New VideoFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)"))
        End If

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
        If Not g.EnableFilter("Resize") Then
            p.Script.AddFilter(New VideoFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)"))
        End If

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
        Assistant()
    End Sub

    <Command("Crops borders automatically until the proper aspect ratio is found.")>
    Sub StartSmartCrop()
        g.SmartCrop()
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
        Dim sb As New SelectionBox(Of TargetVideoScript)

        sb.Title = "New Profile"
        sb.Text = "Please select a profile."

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

        If profile.Engine = ScriptEngine.AviSynth OrElse
            (Package.Python.VerifyOK(True) AndAlso
            Package.VapourSynth.VerifyOK(True) AndAlso
            Package.vspipe.VerifyOK(True)) Then

            Dim currentSetup = p.Script

            Try
                p.Script = profile
                ModifyFilters()
                FiltersListView.OnChanged()
                Assistant()
            Catch ex As Exception
                p.Script = currentSetup
                ModifyFilters()
                FiltersListView.OnChanged()
                Assistant()
            End Try
        End If

        FiltersListView.RebuildMenu()
    End Sub

    Sub ProcessCommandLine(args As String())
        If args.Length > 1 Then
            Package.LoadConfAll()
        Else
            Exit Sub
        End If

        Dim files As New List(Of String)

        For Each arg In CliArg.GetArgs(args)
            Try
                If Not arg.IsFile AndAlso files.Count > 0 Then
                    Dim files2 As New List(Of String)(files)
                    Refresh()
                    OpenAnyFile(files2)
                    files.Clear()
                End If

                If arg.IsFile Then
                    files.Add(arg.Value)
                Else
                    If Not CommandManager.ProcessCommandLineArgument(arg.Value) Then
                        Throw New Exception
                    End If
                End If
            Catch ex As Exception
                MsgWarn("Error parsing argument:" + BR2 + arg.Value + BR2 + ex.Message)
            End Try
        Next

        If files.Count > 0 Then
            Refresh()
            OpenAnyFile(files)
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
        Using dialog As New OpenFileDialog
            dialog.SetFilter(FileTypes.Video.Concat(FileTypes.Image))
            dialog.SetInitDir(s.LastSourceDir)

            If dialog.ShowDialog() = DialogResult.OK Then
                OpenVideoSourceFiles(dialog.FileNames)
            End If
        End Using
    End Sub

    <Command("Dialog to open a Blu-ray folder source.")>
    Sub ShowOpenSourceBlurayFolderDialog()
        If p.SourceFile <> "" Then
            If Not OpenProject() Then
                Exit Sub
            End If
        End If

        Using dialog As New FolderBrowserDialog
            dialog.Description = "Please select a Blu-ray source folder."
            dialog.SetSelectedPath(s.Storage.GetString("last blu-ray source folder"))
            dialog.ShowNewFolderButton = False

            If dialog.ShowDialog = DialogResult.OK Then
                s.Storage.SetString("last blu-ray source folder", dialog.SelectedPath.FixDir)
                Dim srcPath = dialog.SelectedPath.FixDir

                If Directory.Exists(srcPath + "BDMV") Then
                    srcPath = srcPath + "BDMV\"
                End If

                If Directory.Exists(srcPath + "PLAYLIST") Then
                    srcPath = srcPath + "PLAYLIST\"
                End If

                If Not srcPath.ToUpperInvariant.EndsWith("PLAYLIST\") Then
                    MsgWarn("No playlist directory found.")
                    Exit Sub
                End If

                Log.WriteEnvironment()
                Log.Write("Process Blu-Ray folder using eac3to", """" + Package.eac3to.Path + """ """ + srcPath + """" + BR2)
                Log.WriteLine("Source Drive Type: " + New DriveInfo(dialog.SelectedPath).DriveType.ToString + BR)

                Dim output = ProcessHelp.GetConsoleOutput(Package.eac3to.Path, srcPath.Escape).Replace(VB6.vbBack, "")
                Log.WriteLine(output)

                Dim a = Regex.Split(output, "^\d+\)", RegexOptions.Multiline).ToList

                If a(0) = "" Then
                    a.RemoveAt(0)
                End If

                Using td As New TaskDialog(Of Integer)
                    td.Title = "Please select a playlist."

                    For Each i In a
                        If i.Contains(BR) Then
                            td.AddCommand(i.Left(BR).Trim, i.Right(BR).TrimEnd, a.IndexOf(i) + 1)
                        End If
                    Next

                    If td.Show() = 0 Then
                        Exit Sub
                    End If

                    OpenEac3toDemuxForm(srcPath, td.SelectedValue)
                End Using
            End If
        End Using
    End Sub

    <Command("Dialog to open a merged files source.")>
    Sub ShowOpenSourceMergeFilesDialog()
        Using form As New SourceFilesForm()
            form.Text = "Merge"
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
                                OpenVideoSourceFile(outFile)
                            End If
                        End Using
                End Select
            End If
        End Using
    End Sub

    <Command("Dialog to open a file batch source.")>
    Sub ShowOpenSourceBatchFilesDialog()
        If AbortDueToLowDiskSpace() Then
            Exit Sub
        End If

        Using form As New SourceFilesForm()
            form.Text = "File Batch"

            If p.DefaultTargetName = "%source_dir_name%" Then
                p.DefaultTargetName = "%source_name%"
            End If

            If form.ShowDialog() = DialogResult.OK AndAlso form.lb.Items.Count > 0 Then
                If p.SourceFiles.Count > 0 AndAlso Not LoadTemplateWithSelectionDialog() Then
                    Exit Sub
                End If

                For Each filepath In form.GetFiles
                    AddBatchJob(filepath)
                Next

                ShowJobsDialog()
            End If
        End Using
    End Sub

    <Command("Dialog to open source files.")>
    Sub ShowOpenSourceDialog()
        Using td As New TaskDialog(Of String)
            td.Title = "Select a method for opening a source:"
            td.AddCommand("Single File")
            td.AddCommand("Blu-ray Folder")
            td.AddCommand("Merge Files")
            td.AddCommand("File Batch")

            Select Case td.Show
                Case "Single File"
                    ShowOpenSourceSingleFileDialog()
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

            Dim title = InputBox.Show("Enter a short title used as filename",
                                      playlistFolder.Parent.Parent.DirName)
            If title = "" Then
                Exit Sub
            End If

            If p.TempDir <> "" Then
                workDir = p.TempDir.Replace("%source_name%", title)
            Else
                workDir += title
            End If

            If Not g.IsFixedDrive(workDir) Then
                Using dialog As New FolderBrowserDialog
                    dialog.Description = "Please select a folder for temporary files on a fixed local drive."
                    dialog.SetSelectedPath(s.Storage.GetString("last blu-ray target folder").Parent)

                    If dialog.ShowDialog = DialogResult.OK Then
                        workDir = dialog.SelectedPath.FixDir
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

                    If di.AvailableFreeSpace / PrefixedSize(3).Factor < 50 Then
                        MsgError("The target drive has not enough free disk space.")
                        Exit Sub
                    End If

                    Using pr As New Proc
                        pr.Header = "Demux M2TS"
                        pr.TrimChars = {"-"c, " "c}
                        pr.SkipStrings = {"analyze: ", "process: "}
                        pr.Package = Package.eac3to
                        Dim outFiles As New List(Of String)
                        pr.Process.StartInfo.Arguments = form.GetArgs(
                            playlistFolder.Escape + " " & playlistID & ")", title, outFiles)
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
                    Dim fs = form.OutputFolder + title + "." + form.cbVideoOutput.Text.ToLowerInvariant

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
        Using dialog As New OpenFileDialog
            dialog.SetFilter(FileTypes.Video)
            dialog.Multiselect = True
            dialog.SetInitDir(s.LastSourceDir)

            If dialog.ShowDialog() = DialogResult.OK Then
                Refresh()

                Dim l As New List(Of String)(dialog.FileNames)
                l.Sort()
                OpenVideoSourceFiles(l)
            End If
        End Using
    End Sub

    Function GetAudioProfile0() As Profile
        Return GetNewAudioProfile(p.Audio0)
    End Function

    Function GetAudioProfile1() As Profile
        Return GetNewAudioProfile(p.Audio1)
    End Function

    Function GetNewAudioProfile(currentProfile As AudioProfile) As AudioProfile
        Dim sb As New SelectionBox(Of AudioProfile)
        sb.Title = "New Profile"
        sb.Text = "Please select a profile."

        If Not currentProfile Is Nothing Then
            sb.AddItem("Current Project", currentProfile)
        End If

        For Each i In AudioProfile.GetDefaults()
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK Then
            Return sb.SelectedValue
        End If
    End Function

    Sub tbAudioFile0_DoubleClick() Handles tbAudioFile0.DoubleClick
        Using dialog As New OpenFileDialog
            Dim filter = FileTypes.Audio.ToList
            filter.Insert(0, "avs")
            dialog.SetFilter(filter)
            dialog.SetInitDir(p.TempDir, s.LastSourceDir)

            If dialog.ShowDialog() = DialogResult.OK Then
                tbAudioFile0.Text = dialog.FileName
            End If
        End Using
    End Sub

    Sub tbAudioFile1_DoubleClick() Handles tbAudioFile1.DoubleClick
        Using dialog As New OpenFileDialog
            Dim filter = FileTypes.Audio.ToList
            filter.Insert(0, "avs")
            dialog.SetFilter(filter)
            dialog.SetInitDir(p.TempDir, s.LastSourceDir)

            If dialog.ShowDialog() = DialogResult.OK Then
                tbAudioFile1.Text = dialog.FileName
            End If
        End Using
    End Sub

    Sub lTip_Click() Handles laTip.Click
        If Not AssistantClickAction Is Nothing Then
            AssistantClickAction.Invoke()
            Assistant()
        End If
    End Sub

    Sub UpdateTargetParameters(seconds As Integer, frameRate As Double)
        p.TargetSeconds = seconds
        p.TargetFrameRate = frameRate
        UpdateSizeOrBitrate()
    End Sub

    Sub pEncoder_MouseLeave() Handles pnEncoder.MouseLeave
        Assistant()
    End Sub

    Sub AudioEdit0ToolStripMenuItemClick()
        p.Audio0.EditProject()
        UpdateAudioMenu()
        UpdateSizeOrBitrate()
        llAudioProfile0.Text = g.ConvertPath(p.Audio0.Name)
        ShowAudioTip(p.Audio0)
    End Sub

    Sub AudioEdit1ToolStripMenuItemClick()
        p.Audio1.EditProject()
        UpdateAudioMenu()
        UpdateSizeOrBitrate()
        llAudioProfile1.Text = g.ConvertPath(p.Audio1.Name)
        ShowAudioTip(p.Audio1)
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

    Sub AudioSource0ToolStripMenuItemClick()
        tbAudioFile0_DoubleClick()
    End Sub

    Sub AudioSource1ToolStripMenuItemClick()
        tbAudioFile1_DoubleClick()
    End Sub

    <Command("Dialog to manage audio profiles.")>
    Sub ShowAudioProfilesDialog(<DispName("Track Number (0 or 1)")> number As Integer)
        Dim form As ProfilesForm

        If number = 0 Then
            form = New ProfilesForm("Audio Profiles", s.AudioProfiles, AddressOf g.LoadAudioProfile0, AddressOf GetAudioProfile0, AddressOf AudioProfile.GetDefaults)
        Else
            form = New ProfilesForm("Audio Profiles", s.AudioProfiles, AddressOf g.LoadAudioProfile1, AddressOf GetAudioProfile1, AddressOf AudioProfile.GetDefaults)
        End If

        form.ShowDialog()
        form.Dispose()

        UpdateAudioMenu()
        PopulateProfileMenu(DynamicMenuItemID.Audio1Profiles)
        PopulateProfileMenu(DynamicMenuItemID.Audio2Profiles)
    End Sub

    Sub UpdateAudioMenu()
        AudioMenu0.Items.ClearAndDisplose
        AudioMenu1.Items.ClearAndDisplose
        g.PopulateProfileMenu(AudioMenu0.Items, s.AudioProfiles, Sub() ShowAudioProfilesDialog(0), AddressOf g.LoadAudioProfile0)
        g.PopulateProfileMenu(AudioMenu1.Items, s.AudioProfiles, Sub() ShowAudioProfilesDialog(1), AddressOf g.LoadAudioProfile1)
    End Sub

    Sub AviSynthListView_ScriptChanged() Handles FiltersListView.Changed
        If Not IsLoading AndAlso Not FiltersListView.IsLoading Then
            If g.IsValidSource(False) Then
                UpdateSourceParameters()
                UpdateTargetParameters(p.Script.GetSeconds, p.Script.GetFramerate)
            End If

            Assistant()
        End If
    End Sub

    Sub UpdateFilters()
        FiltersListView.Load()

        If g.IsValidSource(False) Then
            UpdateTargetParameters(p.Script.GetSeconds, p.Script.GetFramerate)
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

    Sub lAudioProfile0_Click() Handles llAudioProfile0.Click
        AudioMenu0.Show(llAudioProfile0, 0, 16)
    End Sub

    Sub lAudioProfile1_Click() Handles llAudioProfile1.Click
        AudioMenu1.Show(llAudioProfile1, 0, 16)
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
            .SetTip("Source Display Aspect Ratio", lSourceDar, blSourceDarText)
            .SetTip("Source Pixel Aspect Ratio", lSourcePAR, blSourceParText)
            .SetTip("Target Video Bitrate in Kbps (Up/Down key)", tbBitrate, laBitrate)
            .SetTip("Target Image Width in pixel (Up/Down key)", tbTargetWidth, lTargetWidth)
            .SetTip("Target Image Height in pixel (Up/Down key)", tbTargetHeight, lTargetHeight)
            .SetTip("Target File Size (Up/Down key)", tbTargetSize)
            .SetTip("Source file", tbSourceFile)
            .SetTip("Target file", tbTargetFile)
            .SetTip("Opens audio settings for the current project/template", llEditAudio0, llEditAudio1)
            .SetTip("Shows audio profiles", llAudioProfile0)
            .SetTip("Shows audio profiles", llAudioProfile1)
            .SetTip("Shows a menu with Container/Muxer profiles", llMuxer)
            .SetTip("Shows a menu with video encoder profiles", lgbEncoder.Label)
            .SetTip("Shows a menu with AviSynth filter options", lgbFilters.Label)
        End With
    End Sub

    Sub UpdateSourceParameters()
        If Not p.SourceScript Is Nothing Then
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
        End If
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

    Sub tbAudioFile0_MouseDown(sender As Object, e As MouseEventArgs) Handles tbAudioFile0.MouseDown
        If e.Button = MouseButtons.Right Then
            UpdateAudioFileMenu(Audio0FileMenu, AddressOf tbAudioFile0_DoubleClick, p.Audio0, tbAudioFile0)
        End If
    End Sub

    Sub tbAudioFile1_MouseDown(sender As Object, e As MouseEventArgs) Handles tbAudioFile1.MouseDown
        If e.Button = MouseButtons.Right Then
            UpdateAudioFileMenu(Audio1FileMenu, AddressOf tbAudioFile1_DoubleClick, p.Audio1, tbAudioFile1)
        End If
    End Sub

    Sub UpdateAudioFileMenu(
        m As ContextMenuStripEx, a As Action, ap As AudioProfile, tb As TextEdit)

        m.Items.ClearAndDisplose
        Dim exist = File.Exists(ap.File)

        If ap.Streams.Count > 0 Then
            For Each i In ap.Streams
                Dim temp = i

                Dim menuAction = Sub()
                                     If ap.File <> p.LastOriginalSourceFile Then
                                         tb.Text = p.LastOriginalSourceFile
                                     End If

                                     tb.Text = temp.Name + " (" + ap.File.Ext + ")"
                                     ap.Stream = temp

                                     If tb Is tbAudioFile0 Then
                                         llAudioProfile0.Text = g.ConvertPath(ap.Name)
                                     Else
                                         llAudioProfile1.Text = g.ConvertPath(ap.Name)
                                     End If

                                     UpdateSizeOrBitrate()
                                 End Sub

                If ap.Streams.Count > 10 Then
                    m.Add("Streams | " + i.Name, menuAction)
                Else
                    m.Add(i.Name, menuAction)
                End If
            Next

            m.Items.Add("-")
        End If

        If p.SourceFile.Ext = "avs" OrElse p.Script.GetScript.ToLowerInvariant.Contains("audiodub(") Then
            m.Add(p.Script.Path.FileName, Sub() tb.Text = p.Script.Path)
        End If

        If p.TempDir <> "" AndAlso Directory.Exists(p.TempDir) Then
            Dim audioFiles = Directory.GetFiles(p.TempDir).Where(
                Function(audioPath) FileTypes.Audio.Contains(audioPath.Ext)).ToList

            audioFiles.Sort(New StringLogicalComparer)

            If audioFiles.Count > 0 Then
                For Each i In audioFiles
                    Dim temp = i

                    If audioFiles.Count > 10 Then
                        m.Add("Files | " + i.FileName, Sub() tb.Text = temp)
                    Else
                        m.Add(i.FileName, Sub() tb.Text = temp)
                    End If
                Next

                m.Items.Add("-")
            End If
        End If

        Dim moreFilesAction = Sub()
                                  s.Storage.SetInt("last selected muxer tab", 1)
                                  p.VideoEncoder.OpenMuxerConfigDialog()
                              End Sub

        m.Add("Open File", a, "Change the audio source file.").SetImage(Symbol.OpenFile)
        m.Add("Add more files...", moreFilesAction, exist)
        m.Add("Play", Sub() g.PlayAudio(ap), exist, "Plays the audio source file with a media player.").SetImage(Symbol.Play)
        m.Add("Media Info...", Sub() g.DefaultCommands.ShowMediaInfo(ap.File), exist, "Show MediaInfo for the audio source file.").SetImage(Symbol.Info)
        m.Add("Explore", Sub() g.SelectFileWithExplorer(ap.File), exist, "Open the audio source file directory with File Explorer.").SetImage(Symbol.FileExplorer)
        m.Add("Execute", Sub() ExecuteAudio(ap), exist, "Processes the audio profile.")
        m.Add("-")
        m.Add("Copy Path", Sub() Clipboard.SetText(ap.File), tb.Text <> "")
        m.Add("Copy Selection", Sub() Clipboard.SetText(tb.TextBox.SelectedText), tb.Text <> "").SetImage(Symbol.Copy)
        m.Add("Paste", Sub() tb.TextBox.Paste(), Clipboard.GetText.Trim <> "").SetImage(Symbol.Paste)
        m.Add("-")
        m.Add("Remove", Sub() tb.Text = "", tb.Text <> "", "Remove audio file").SetImage(Symbol.Remove)
    End Sub

    Sub ExecuteAudio(ap As AudioProfile)
        If MsgQuestion("Confirm to process the track.") = DialogResult.OK Then
            Try
                If p.TempDir = "" Then
                    p.TempDir = ap.File.Dir
                End If

                ap = ObjectHelp.GetCopy(Of AudioProfile)(ap)
                Audio.Process(ap)
                ap.Encode()
            Catch
            End Try
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

        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            BeginInvoke(Sub() OpenAnyFile(files.ToList))
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

        ProcController.LastActivation = Environment.TickCount

        BeginInvoke(New Action(Sub()
                                   Application.DoEvents()
                                   Assistant()
                                   UpdateScriptsMenuAsync()
                               End Sub))
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        UpdateDynamicMenuAsync()
        UpdateRecentProjectsMenu()
        UpdateTemplatesMenuAsync()
        IsLoading = False
        Refresh()
        ProcessCommandLine(Environment.GetCommandLineArgs)
        StaxRipUpdate.ShowUpdateQuestion()
        StaxRipUpdate.CheckForUpdate(False, s.CheckForUpdatesDev, Environment.Is64BitProcess)
        g.RunTask(AddressOf g.LoadPowerShellScripts)

        If TypeOf p.VideoEncoder Is x265Enc Then
            g.RunTask(Sub() Equals(Package.x265Type))
        End If
    End Sub

    Protected Overrides Sub OnFormClosing(args As FormClosingEventArgs)
        MyBase.OnFormClosing(args)

        If IsSaveCanceled() Then
            args.Cancel = True
        End If
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)

        If Not g.ProcForm Is Nothing Then
            g.ProcForm.Invoke(Sub() g.ProcForm.Close())
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

                If ProcController.IsLastActivationLessThan(60) Then
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
End Class
