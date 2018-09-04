Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks
Imports Microsoft.Win32
Imports StaxRip.UI
Imports VB6 = Microsoft.VisualBasic

Public Class MainForm
    Inherits FormBase

#Region " Designer "

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then If Not components Is Nothing Then components.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    Public WithEvents tbAudioFile0 As StaxRip.UI.TextBoxEx
    Public WithEvents tbAudioFile1 As StaxRip.UI.TextBoxEx
    Public WithEvents llEditAudio1 As ButtonLabel
    Public WithEvents llEditAudio0 As ButtonLabel
    Public WithEvents bnNext As System.Windows.Forms.Button
    Public WithEvents tbSourceFile As StaxRip.UI.TextBoxEx
    Public WithEvents tbTargetFile As StaxRip.UI.TextBoxEx
    Public WithEvents gbAssistant As System.Windows.Forms.GroupBox
    Public WithEvents lgbFilters As LinkGroupBox
    Public WithEvents tbTargetSize As System.Windows.Forms.TextBox
    Public WithEvents lBitrate As System.Windows.Forms.Label
    Public WithEvents tbBitrate As System.Windows.Forms.TextBox
    Public WithEvents lTarget1 As System.Windows.Forms.Label
    Public WithEvents lSource1 As System.Windows.Forms.Label
    Public WithEvents lgbResize As LinkGroupBox
    Public WithEvents lPixel As System.Windows.Forms.Label
    Public WithEvents lPixelText As System.Windows.Forms.Label
    Public WithEvents tbResize As System.Windows.Forms.TrackBar
    Public WithEvents lZoom As System.Windows.Forms.Label
    Public WithEvents lZoomText As System.Windows.Forms.Label
    Public WithEvents tbTargetHeight As System.Windows.Forms.TextBox
    Public WithEvents tbTargetWidth As System.Windows.Forms.TextBox
    Public WithEvents lTargetHeight As System.Windows.Forms.Label
    Public WithEvents lTargetWidth As System.Windows.Forms.Label
    Public WithEvents lDAR As System.Windows.Forms.Label
    Public WithEvents blTargetDarText As ButtonLabel
    Public WithEvents lCrop As System.Windows.Forms.Label
    Public WithEvents lCropText As System.Windows.Forms.Label
    Public WithEvents lgbSource As LinkGroupBox
    Public WithEvents lSource2 As System.Windows.Forms.Label
    Public WithEvents lgbTarget As LinkGroupBox
    Public WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Public WithEvents lTip As System.Windows.Forms.Label
    Public WithEvents lgbEncoder As LinkGroupBox
    Public WithEvents lTarget2 As System.Windows.Forms.Label
    Public WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Public WithEvents llAudioProfile0 As ButtonLabel
    Public WithEvents llAudioProfile1 As ButtonLabel
    Public WithEvents pnEncoder As System.Windows.Forms.Panel
    Public WithEvents FiltersListView As StaxRip.FiltersListView
    Public WithEvents blFilesize As ButtonLabel
    Public WithEvents llMuxer As ButtonLabel
    Public WithEvents lPAR As StaxRip.UI.LabelEx
    Public WithEvents blTargetParText As ButtonLabel
    Public WithEvents lAspectRatioError As StaxRip.UI.LabelEx
    Public WithEvents lAspectRatioErrorText As StaxRip.UI.LabelEx
    Public WithEvents gbAudio As System.Windows.Forms.GroupBox
    Public WithEvents blSourceParText As ButtonLabel
    Public WithEvents lSourcePAR As System.Windows.Forms.Label
    Public WithEvents lSourceDar As System.Windows.Forms.Label
    Public WithEvents lSAR As StaxRip.UI.LabelEx
    Public WithEvents lSarText As StaxRip.UI.LabelEx
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.bnNext = New System.Windows.Forms.Button()
        Me.llEditAudio0 = New StaxRip.UI.ButtonLabel()
        Me.gbAssistant = New System.Windows.Forms.GroupBox()
        Me.tlpAssistant = New System.Windows.Forms.TableLayoutPanel()
        Me.lTip = New System.Windows.Forms.Label()
        Me.llEditAudio1 = New StaxRip.UI.ButtonLabel()
        Me.gbAudio = New System.Windows.Forms.GroupBox()
        Me.tlpAudio = New System.Windows.Forms.TableLayoutPanel()
        Me.tbAudioFile0 = New StaxRip.UI.TextBoxEx()
        Me.tbAudioFile1 = New StaxRip.UI.TextBoxEx()
        Me.llAudioProfile1 = New StaxRip.UI.ButtonLabel()
        Me.llAudioProfile0 = New StaxRip.UI.ButtonLabel()
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.lgbTarget = New StaxRip.UI.LinkGroupBox()
        Me.tlpTarget = New System.Windows.Forms.TableLayoutPanel()
        Me.tbTargetFile = New StaxRip.UI.TextBoxEx()
        Me.lTarget2 = New System.Windows.Forms.Label()
        Me.tbTargetSize = New System.Windows.Forms.TextBox()
        Me.lTarget1 = New System.Windows.Forms.Label()
        Me.tbBitrate = New System.Windows.Forms.TextBox()
        Me.lBitrate = New System.Windows.Forms.Label()
        Me.blFilesize = New StaxRip.UI.ButtonLabel()
        Me.lgbSource = New StaxRip.UI.LinkGroupBox()
        Me.tlpSource = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpSourceValues = New System.Windows.Forms.TableLayoutPanel()
        Me.blSourceDarText = New StaxRip.UI.ButtonLabel()
        Me.lSourcePAR = New System.Windows.Forms.Label()
        Me.blSourceParText = New StaxRip.UI.ButtonLabel()
        Me.lCrop = New System.Windows.Forms.Label()
        Me.lCropText = New System.Windows.Forms.Label()
        Me.lSourceDar = New System.Windows.Forms.Label()
        Me.tbSourceFile = New StaxRip.UI.TextBoxEx()
        Me.lSource1 = New System.Windows.Forms.Label()
        Me.lSource2 = New System.Windows.Forms.Label()
        Me.lgbResize = New StaxRip.UI.LinkGroupBox()
        Me.tlpResize = New System.Windows.Forms.TableLayoutPanel()
        Me.lTargetWidth = New System.Windows.Forms.Label()
        Me.tlpResizeValues = New System.Windows.Forms.TableLayoutPanel()
        Me.blTargetDarText = New StaxRip.UI.ButtonLabel()
        Me.lAspectRatioError = New StaxRip.UI.LabelEx()
        Me.lPAR = New StaxRip.UI.LabelEx()
        Me.lZoom = New System.Windows.Forms.Label()
        Me.lSarText = New StaxRip.UI.LabelEx()
        Me.lPixel = New System.Windows.Forms.Label()
        Me.blTargetParText = New StaxRip.UI.ButtonLabel()
        Me.lAspectRatioErrorText = New StaxRip.UI.LabelEx()
        Me.lDAR = New System.Windows.Forms.Label()
        Me.lZoomText = New System.Windows.Forms.Label()
        Me.lPixelText = New System.Windows.Forms.Label()
        Me.lSAR = New StaxRip.UI.LabelEx()
        Me.tbResize = New System.Windows.Forms.TrackBar()
        Me.tbTargetWidth = New System.Windows.Forms.TextBox()
        Me.tbTargetHeight = New System.Windows.Forms.TextBox()
        Me.lTargetHeight = New System.Windows.Forms.Label()
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
        Me.bnNext.AutoSize = True
        Me.bnNext.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.bnNext.Cursor = System.Windows.Forms.Cursors.Default
        Me.bnNext.Location = New System.Drawing.Point(1886, 29)
        Me.bnNext.Margin = New System.Windows.Forms.Padding(0, 0, 6, 0)
        Me.bnNext.Name = "bnNext"
        Me.bnNext.Size = New System.Drawing.Size(125, 58)
        Me.bnNext.TabIndex = 39
        Me.bnNext.Text = " Next "
        '
        'llEditAudio0
        '
        Me.llEditAudio0.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.llEditAudio0.AutoSize = True
        Me.llEditAudio0.LinkColor = System.Drawing.Color.Empty
        Me.llEditAudio0.Location = New System.Drawing.Point(1931, 11)
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
        Me.gbAssistant.Location = New System.Drawing.Point(10, 1030)
        Me.gbAssistant.Margin = New System.Windows.Forms.Padding(10, 0, 10, 10)
        Me.gbAssistant.Name = "gbAssistant"
        Me.gbAssistant.Padding = New System.Windows.Forms.Padding(6, 0, 6, 6)
        Me.gbAssistant.Size = New System.Drawing.Size(2029, 170)
        Me.gbAssistant.TabIndex = 44
        Me.gbAssistant.TabStop = False
        Me.gbAssistant.Text = "Assistant"
        '
        'tlpAssistant
        '
        Me.tlpAssistant.ColumnCount = 2
        Me.tlpAssistant.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAssistant.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAssistant.Controls.Add(Me.lTip, 0, 0)
        Me.tlpAssistant.Controls.Add(Me.bnNext, 1, 0)
        Me.tlpAssistant.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAssistant.Location = New System.Drawing.Point(6, 48)
        Me.tlpAssistant.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpAssistant.Name = "tlpAssistant"
        Me.tlpAssistant.RowCount = 1
        Me.tlpAssistant.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAssistant.Size = New System.Drawing.Size(2017, 116)
        Me.tlpAssistant.TabIndex = 62
        '
        'lTip
        '
        Me.lTip.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lTip.ForeColor = System.Drawing.Color.Blue
        Me.lTip.Location = New System.Drawing.Point(0, 0)
        Me.lTip.Margin = New System.Windows.Forms.Padding(0)
        Me.lTip.Name = "lTip"
        Me.lTip.Size = New System.Drawing.Size(1886, 116)
        Me.lTip.TabIndex = 40
        Me.lTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'llEditAudio1
        '
        Me.llEditAudio1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.llEditAudio1.AutoSize = True
        Me.llEditAudio1.LinkColor = System.Drawing.Color.Empty
        Me.llEditAudio1.Location = New System.Drawing.Point(1931, 89)
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
        Me.gbAudio.Location = New System.Drawing.Point(10, 820)
        Me.gbAudio.Margin = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.gbAudio.Name = "gbAudio"
        Me.gbAudio.Padding = New System.Windows.Forms.Padding(6, 0, 6, 6)
        Me.gbAudio.Size = New System.Drawing.Size(2029, 210)
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
        Me.tlpAudio.Size = New System.Drawing.Size(2017, 156)
        Me.tlpAudio.TabIndex = 62
        '
        'tbAudioFile0
        '
        Me.tbAudioFile0.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAudioFile0.Location = New System.Drawing.Point(6, 7)
        Me.tbAudioFile0.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.tbAudioFile0.Size = New System.Drawing.Size(1867, 55)
        '
        'tbAudioFile1
        '
        Me.tbAudioFile1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAudioFile1.Location = New System.Drawing.Point(6, 85)
        Me.tbAudioFile1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.tbAudioFile1.Size = New System.Drawing.Size(1867, 55)
        '
        'llAudioProfile1
        '
        Me.llAudioProfile1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.llAudioProfile1.AutoSize = True
        Me.llAudioProfile1.LinkColor = System.Drawing.Color.Empty
        Me.llAudioProfile1.Location = New System.Drawing.Point(1885, 89)
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
        Me.llAudioProfile0.LinkColor = System.Drawing.Color.Empty
        Me.llAudioProfile0.Location = New System.Drawing.Point(1885, 11)
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
        Me.MenuStrip.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Padding = New System.Windows.Forms.Padding(7, 7, 0, 7)
        Me.MenuStrip.Size = New System.Drawing.Size(2049, 80)
        Me.MenuStrip.TabIndex = 60
        Me.MenuStrip.Text = "MenuStrip"
        '
        'lgbTarget
        '
        Me.lgbTarget.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lgbTarget.Color = System.Drawing.Color.Empty
        Me.tlpMain.SetColumnSpan(Me.lgbTarget, 2)
        Me.lgbTarget.Controls.Add(Me.tlpTarget)
        Me.lgbTarget.Location = New System.Drawing.Point(1029, 80)
        Me.lgbTarget.Margin = New System.Windows.Forms.Padding(6, 0, 10, 0)
        Me.lgbTarget.Name = "lgbTarget"
        Me.lgbTarget.Padding = New System.Windows.Forms.Padding(6, 0, 6, 6)
        Me.lgbTarget.Size = New System.Drawing.Size(1010, 356)
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
        Me.tlpTarget.Controls.Add(Me.lTarget2, 0, 3)
        Me.tlpTarget.Controls.Add(Me.tbTargetSize, 1, 1)
        Me.tlpTarget.Controls.Add(Me.lTarget1, 0, 2)
        Me.tlpTarget.Controls.Add(Me.tbBitrate, 3, 1)
        Me.tlpTarget.Controls.Add(Me.lBitrate, 2, 1)
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
        Me.tlpTarget.Size = New System.Drawing.Size(998, 302)
        Me.tlpTarget.TabIndex = 62
        '
        'tbTargetFile
        '
        Me.tbTargetFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpTarget.SetColumnSpan(Me.tbTargetFile, 4)
        Me.tbTargetFile.Location = New System.Drawing.Point(6, 10)
        Me.tbTargetFile.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.tbTargetFile.Size = New System.Drawing.Size(986, 55)
        '
        'lTarget2
        '
        Me.lTarget2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpTarget.SetColumnSpan(Me.lTarget2, 4)
        Me.lTarget2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lTarget2.Location = New System.Drawing.Point(6, 233)
        Me.lTarget2.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lTarget2.Name = "lTarget2"
        Me.lTarget2.Size = New System.Drawing.Size(986, 60)
        Me.lTarget2.TabIndex = 47
        Me.lTarget2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTargetSize
        '
        Me.tbTargetSize.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tbTargetSize.Location = New System.Drawing.Point(249, 85)
        Me.tbTargetSize.Margin = New System.Windows.Forms.Padding(0)
        Me.tbTargetSize.Name = "tbTargetSize"
        Me.tbTargetSize.Size = New System.Drawing.Size(136, 55)
        Me.tbTargetSize.TabIndex = 55
        Me.tbTargetSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lTarget1
        '
        Me.lTarget1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpTarget.SetColumnSpan(Me.lTarget1, 4)
        Me.lTarget1.Location = New System.Drawing.Point(6, 157)
        Me.lTarget1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lTarget1.Name = "lTarget1"
        Me.lTarget1.Size = New System.Drawing.Size(986, 60)
        Me.lTarget1.TabIndex = 39
        Me.lTarget1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbBitrate
        '
        Me.tbBitrate.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tbBitrate.Location = New System.Drawing.Point(747, 85)
        Me.tbBitrate.Margin = New System.Windows.Forms.Padding(0)
        Me.tbBitrate.Name = "tbBitrate"
        Me.tbBitrate.Size = New System.Drawing.Size(139, 55)
        Me.tbBitrate.TabIndex = 41
        Me.tbBitrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lBitrate
        '
        Me.lBitrate.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lBitrate.Location = New System.Drawing.Point(498, 75)
        Me.lBitrate.Margin = New System.Windows.Forms.Padding(0)
        Me.lBitrate.Name = "lBitrate"
        Me.lBitrate.Size = New System.Drawing.Size(249, 75)
        Me.lBitrate.TabIndex = 42
        Me.lBitrate.Text = "Video Bitrate:"
        Me.lBitrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'blFilesize
        '
        Me.blFilesize.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.blFilesize.LinkColor = System.Drawing.Color.Empty
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
        Me.lgbSource.Color = System.Drawing.Color.Empty
        Me.tlpMain.SetColumnSpan(Me.lgbSource, 2)
        Me.lgbSource.Controls.Add(Me.tlpSource)
        Me.lgbSource.Location = New System.Drawing.Point(10, 80)
        Me.lgbSource.Margin = New System.Windows.Forms.Padding(10, 0, 6, 0)
        Me.lgbSource.Name = "lgbSource"
        Me.lgbSource.Padding = New System.Windows.Forms.Padding(6, 0, 6, 6)
        Me.lgbSource.Size = New System.Drawing.Size(1007, 356)
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
        Me.tlpSource.Size = New System.Drawing.Size(995, 302)
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
        Me.tlpSourceValues.Size = New System.Drawing.Size(995, 77)
        Me.tlpSourceValues.TabIndex = 1
        '
        'blSourceDarText
        '
        Me.blSourceDarText.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.blSourceDarText.AutoSize = True
        Me.blSourceDarText.LinkColor = System.Drawing.Color.Empty
        Me.blSourceDarText.Location = New System.Drawing.Point(446, 14)
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
        Me.lSourcePAR.Location = New System.Drawing.Point(400, 14)
        Me.lSourcePAR.Margin = New System.Windows.Forms.Padding(9, 0, 9, 0)
        Me.lSourcePAR.Name = "lSourcePAR"
        Me.lSourcePAR.Size = New System.Drawing.Size(34, 48)
        Me.lSourcePAR.TabIndex = 47
        Me.lSourcePAR.Text = "-"
        Me.lSourcePAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'blSourceParText
        '
        Me.blSourceParText.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.blSourceParText.AutoSize = True
        Me.blSourceParText.LinkColor = System.Drawing.Color.Empty
        Me.blSourceParText.Location = New System.Drawing.Point(291, 14)
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
        Me.lCrop.Location = New System.Drawing.Point(120, 14)
        Me.lCrop.Margin = New System.Windows.Forms.Padding(9, 0, 9, 0)
        Me.lCrop.Name = "lCrop"
        Me.lCrop.Size = New System.Drawing.Size(153, 48)
        Me.lCrop.TabIndex = 16
        Me.lCrop.Text = "disabled"
        Me.lCrop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lCropText
        '
        Me.lCropText.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lCropText.AutoSize = True
        Me.lCropText.Location = New System.Drawing.Point(3, 14)
        Me.lCropText.Name = "lCropText"
        Me.lCropText.Size = New System.Drawing.Size(105, 48)
        Me.lCropText.TabIndex = 14
        Me.lCropText.TabStop = True
        Me.lCropText.Text = "Crop:"
        Me.lCropText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourceDar
        '
        Me.lSourceDar.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lSourceDar.AutoSize = True
        Me.lSourceDar.Location = New System.Drawing.Point(549, 14)
        Me.lSourceDar.Name = "lSourceDar"
        Me.lSourceDar.Size = New System.Drawing.Size(34, 48)
        Me.lSourceDar.TabIndex = 50
        Me.lSourceDar.Text = "-"
        Me.lSourceDar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSourceFile
        '
        Me.tbSourceFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSourceFile.Location = New System.Drawing.Point(6, 10)
        Me.tbSourceFile.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.tbSourceFile.Size = New System.Drawing.Size(983, 55)
        '
        'lSource1
        '
        Me.lSource1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lSource1.Location = New System.Drawing.Point(6, 84)
        Me.lSource1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lSource1.Name = "lSource1"
        Me.lSource1.Size = New System.Drawing.Size(983, 57)
        Me.lSource1.TabIndex = 41
        Me.lSource1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSource2
        '
        Me.lSource2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lSource2.Location = New System.Drawing.Point(6, 159)
        Me.lSource2.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lSource2.Name = "lSource2"
        Me.lSource2.Size = New System.Drawing.Size(983, 57)
        Me.lSource2.TabIndex = 33
        Me.lSource2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lgbResize
        '
        Me.lgbResize.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lgbResize.Color = System.Drawing.Color.Empty
        Me.tlpMain.SetColumnSpan(Me.lgbResize, 2)
        Me.lgbResize.Controls.Add(Me.tlpResize)
        Me.lgbResize.Location = New System.Drawing.Point(688, 436)
        Me.lgbResize.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lgbResize.Name = "lgbResize"
        Me.lgbResize.Padding = New System.Windows.Forms.Padding(6, 0, 6, 6)
        Me.lgbResize.Size = New System.Drawing.Size(670, 384)
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
        Me.tlpResize.Controls.Add(Me.lTargetWidth, 0, 0)
        Me.tlpResize.Controls.Add(Me.tlpResizeValues, 0, 2)
        Me.tlpResize.Controls.Add(Me.tbResize, 0, 1)
        Me.tlpResize.Controls.Add(Me.tbTargetWidth, 1, 0)
        Me.tlpResize.Controls.Add(Me.tbTargetHeight, 3, 0)
        Me.tlpResize.Controls.Add(Me.lTargetHeight, 2, 0)
        Me.tlpResize.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpResize.Location = New System.Drawing.Point(6, 48)
        Me.tlpResize.Name = "tlpResize"
        Me.tlpResize.RowCount = 3
        Me.tlpResize.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpResize.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpResize.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpResize.Size = New System.Drawing.Size(658, 330)
        Me.tlpResize.TabIndex = 63
        '
        'lTargetWidth
        '
        Me.lTargetWidth.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lTargetWidth.AutoSize = True
        Me.lTargetWidth.Location = New System.Drawing.Point(3, 16)
        Me.lTargetWidth.Name = "lTargetWidth"
        Me.lTargetWidth.Size = New System.Drawing.Size(124, 48)
        Me.lTargetWidth.TabIndex = 37
        Me.lTargetWidth.Text = "Width:"
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
        Me.tlpResizeValues.Location = New System.Drawing.Point(0, 160)
        Me.tlpResizeValues.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpResizeValues.Name = "tlpResizeValues"
        Me.tlpResizeValues.RowCount = 3
        Me.tlpResizeValues.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpResizeValues.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpResizeValues.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpResizeValues.Size = New System.Drawing.Size(658, 170)
        Me.tlpResizeValues.TabIndex = 62
        '
        'blTargetDarText
        '
        Me.blTargetDarText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.blTargetDarText.AutoSize = True
        Me.blTargetDarText.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.blTargetDarText.LinkColor = System.Drawing.Color.Empty
        Me.blTargetDarText.Location = New System.Drawing.Point(0, 4)
        Me.blTargetDarText.Margin = New System.Windows.Forms.Padding(0)
        Me.blTargetDarText.Name = "blTargetDarText"
        Me.blTargetDarText.Size = New System.Drawing.Size(97, 48)
        Me.blTargetDarText.TabIndex = 23
        Me.blTargetDarText.Text = "DAR:"
        Me.blTargetDarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lAspectRatioError
        '
        Me.lAspectRatioError.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lAspectRatioError.AutoSize = True
        Me.lAspectRatioError.Location = New System.Drawing.Point(253, 117)
        Me.lAspectRatioError.Margin = New System.Windows.Forms.Padding(0)
        Me.lAspectRatioError.Size = New System.Drawing.Size(34, 48)
        Me.lAspectRatioError.Text = "-"
        Me.lAspectRatioError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPAR
        '
        Me.lPAR.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lPAR.AutoSize = True
        Me.lPAR.Location = New System.Drawing.Point(97, 117)
        Me.lPAR.Margin = New System.Windows.Forms.Padding(0)
        Me.lPAR.Size = New System.Drawing.Size(34, 48)
        Me.lPAR.Text = "-"
        Me.lPAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lZoom
        '
        Me.lZoom.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lZoom.AutoSize = True
        Me.lZoom.Location = New System.Drawing.Point(253, 60)
        Me.lZoom.Margin = New System.Windows.Forms.Padding(0)
        Me.lZoom.Name = "lZoom"
        Me.lZoom.Size = New System.Drawing.Size(34, 48)
        Me.lZoom.TabIndex = 44
        Me.lZoom.Text = "-"
        Me.lZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSarText
        '
        Me.lSarText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lSarText.AutoSize = True
        Me.lSarText.Location = New System.Drawing.Point(0, 60)
        Me.lSarText.Margin = New System.Windows.Forms.Padding(0)
        Me.lSarText.Size = New System.Drawing.Size(92, 48)
        Me.lSarText.Text = "SAR:"
        Me.lSarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPixel
        '
        Me.lPixel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lPixel.AutoSize = True
        Me.lPixel.Location = New System.Drawing.Point(253, 4)
        Me.lPixel.Margin = New System.Windows.Forms.Padding(0)
        Me.lPixel.Name = "lPixel"
        Me.lPixel.Size = New System.Drawing.Size(34, 48)
        Me.lPixel.TabIndex = 50
        Me.lPixel.Text = "-"
        Me.lPixel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'blTargetParText
        '
        Me.blTargetParText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.blTargetParText.AutoSize = True
        Me.blTargetParText.LinkColor = System.Drawing.Color.Empty
        Me.blTargetParText.Location = New System.Drawing.Point(0, 117)
        Me.blTargetParText.Margin = New System.Windows.Forms.Padding(0)
        Me.blTargetParText.Name = "blTargetParText"
        Me.blTargetParText.Size = New System.Drawing.Size(91, 48)
        Me.blTargetParText.TabIndex = 51
        Me.blTargetParText.Text = "PAR:"
        Me.blTargetParText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lAspectRatioErrorText
        '
        Me.lAspectRatioErrorText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lAspectRatioErrorText.AutoSize = True
        Me.lAspectRatioErrorText.Location = New System.Drawing.Point(131, 117)
        Me.lAspectRatioErrorText.Margin = New System.Windows.Forms.Padding(0)
        Me.lAspectRatioErrorText.Size = New System.Drawing.Size(106, 48)
        Me.lAspectRatioErrorText.Text = "Error:"
        Me.lAspectRatioErrorText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lDAR
        '
        Me.lDAR.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lDAR.AutoSize = True
        Me.lDAR.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lDAR.Location = New System.Drawing.Point(97, 4)
        Me.lDAR.Margin = New System.Windows.Forms.Padding(0)
        Me.lDAR.Name = "lDAR"
        Me.lDAR.Size = New System.Drawing.Size(34, 48)
        Me.lDAR.TabIndex = 24
        Me.lDAR.Text = "-"
        Me.lDAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lZoomText
        '
        Me.lZoomText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lZoomText.AutoSize = True
        Me.lZoomText.Location = New System.Drawing.Point(131, 60)
        Me.lZoomText.Margin = New System.Windows.Forms.Padding(0)
        Me.lZoomText.Name = "lZoomText"
        Me.lZoomText.Size = New System.Drawing.Size(122, 48)
        Me.lZoomText.TabIndex = 42
        Me.lZoomText.Text = "Zoom:"
        Me.lZoomText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPixelText
        '
        Me.lPixelText.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lPixelText.AutoSize = True
        Me.lPixelText.Location = New System.Drawing.Point(131, 4)
        Me.lPixelText.Margin = New System.Windows.Forms.Padding(0)
        Me.lPixelText.Name = "lPixelText"
        Me.lPixelText.Size = New System.Drawing.Size(102, 48)
        Me.lPixelText.TabIndex = 49
        Me.lPixelText.Text = "Pixel:"
        Me.lPixelText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSAR
        '
        Me.lSAR.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lSAR.AutoSize = True
        Me.lSAR.Location = New System.Drawing.Point(97, 60)
        Me.lSAR.Margin = New System.Windows.Forms.Padding(0)
        Me.lSAR.Size = New System.Drawing.Size(34, 48)
        Me.lSAR.Text = "-"
        Me.lSAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbResize
        '
        Me.tbResize.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbResize.AutoSize = False
        Me.tbResize.BackColor = System.Drawing.SystemColors.Control
        Me.tlpResize.SetColumnSpan(Me.tbResize, 4)
        Me.tbResize.Location = New System.Drawing.Point(0, 80)
        Me.tbResize.Margin = New System.Windows.Forms.Padding(0)
        Me.tbResize.Name = "tbResize"
        Me.tbResize.Size = New System.Drawing.Size(658, 80)
        Me.tbResize.TabIndex = 46
        '
        'tbTargetWidth
        '
        Me.tbTargetWidth.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tbTargetWidth.Location = New System.Drawing.Point(167, 12)
        Me.tbTargetWidth.Name = "tbTargetWidth"
        Me.tbTargetWidth.Size = New System.Drawing.Size(144, 55)
        Me.tbTargetWidth.TabIndex = 39
        Me.tbTargetWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbTargetHeight
        '
        Me.tbTargetHeight.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tbTargetHeight.Location = New System.Drawing.Point(495, 12)
        Me.tbTargetHeight.Name = "tbTargetHeight"
        Me.tbTargetHeight.Size = New System.Drawing.Size(146, 55)
        Me.tbTargetHeight.TabIndex = 40
        Me.tbTargetHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lTargetHeight
        '
        Me.lTargetHeight.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lTargetHeight.AutoSize = True
        Me.lTargetHeight.Location = New System.Drawing.Point(331, 16)
        Me.lTargetHeight.Name = "lTargetHeight"
        Me.lTargetHeight.Size = New System.Drawing.Size(135, 48)
        Me.lTargetHeight.TabIndex = 38
        Me.lTargetHeight.Text = "Height:"
        '
        'lgbFilters
        '
        Me.lgbFilters.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lgbFilters.Color = System.Drawing.Color.Empty
        Me.lgbFilters.Controls.Add(Me.FiltersListView)
        Me.lgbFilters.Location = New System.Drawing.Point(10, 436)
        Me.lgbFilters.Margin = New System.Windows.Forms.Padding(10, 0, 6, 0)
        Me.lgbFilters.Name = "lgbFilters"
        Me.lgbFilters.Padding = New System.Windows.Forms.Padding(9, 2, 9, 9)
        Me.lgbFilters.Size = New System.Drawing.Size(666, 384)
        Me.lgbFilters.TabIndex = 53
        Me.lgbFilters.TabStop = False
        Me.lgbFilters.Text = "Filters"
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
        Me.FiltersListView.Location = New System.Drawing.Point(9, 50)
        Me.FiltersListView.MultiSelect = False
        Me.FiltersListView.Name = "FiltersListView"
        Me.FiltersListView.Size = New System.Drawing.Size(648, 325)
        Me.FiltersListView.TabIndex = 0
        Me.FiltersListView.UseCompatibleStateImageBehavior = False
        Me.FiltersListView.View = System.Windows.Forms.View.Details
        '
        'lgbEncoder
        '
        Me.lgbEncoder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lgbEncoder.Color = System.Drawing.Color.Empty
        Me.lgbEncoder.Controls.Add(Me.llMuxer)
        Me.lgbEncoder.Controls.Add(Me.pnEncoder)
        Me.lgbEncoder.Location = New System.Drawing.Point(1370, 436)
        Me.lgbEncoder.Margin = New System.Windows.Forms.Padding(6, 0, 10, 0)
        Me.lgbEncoder.Name = "lgbEncoder"
        Me.lgbEncoder.Padding = New System.Windows.Forms.Padding(9, 2, 9, 9)
        Me.lgbEncoder.Size = New System.Drawing.Size(669, 384)
        Me.lgbEncoder.TabIndex = 51
        Me.lgbEncoder.TabStop = False
        Me.lgbEncoder.Text = "Encoder"
        '
        'llMuxer
        '
        Me.llMuxer.AutoSize = True
        Me.llMuxer.LinkColor = System.Drawing.Color.Empty
        Me.llMuxer.Location = New System.Drawing.Point(517, 1)
        Me.llMuxer.Name = "llMuxer"
        Me.llMuxer.Size = New System.Drawing.Size(121, 48)
        Me.llMuxer.TabIndex = 1
        Me.llMuxer.TabStop = True
        Me.llMuxer.Text = "Muxer"
        '
        'pnEncoder
        '
        Me.pnEncoder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnEncoder.Location = New System.Drawing.Point(9, 50)
        Me.pnEncoder.Name = "pnEncoder"
        Me.pnEncoder.Size = New System.Drawing.Size(651, 325)
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
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 356.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 210.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180.0!))
        Me.tlpMain.Size = New System.Drawing.Size(2049, 1210)
        Me.tlpMain.TabIndex = 61
        '
        'MainForm
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(2049, 1210)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MainMenuStrip = Me.MenuStrip
        Me.Margin = New System.Windows.Forms.Padding(9, 11, 9, 11)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "StaxRip"
        Me.gbAssistant.ResumeLayout(False)
        Me.tlpAssistant.ResumeLayout(False)
        Me.tlpAssistant.PerformLayout()
        Me.gbAudio.ResumeLayout(False)
        Me.tlpAudio.ResumeLayout(False)
        Me.tlpAudio.PerformLayout()
        Me.lgbTarget.ResumeLayout(False)
        Me.lgbTarget.PerformLayout()
        Me.tlpTarget.ResumeLayout(False)
        Me.tlpTarget.PerformLayout()
        Me.lgbSource.ResumeLayout(False)
        Me.lgbSource.PerformLayout()
        Me.tlpSource.ResumeLayout(False)
        Me.tlpSource.PerformLayout()
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

    Public BlockSubtitlesItemCheck As Boolean
    Public WithEvents CustomMainMenu As CustomMenu
    Public WithEvents CustomSizeMenu As CustomMenu
    Public CurrentAssistantTipKey As String
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
    Private BlockAviSynthItemCheck As Boolean
    Private CanChangeSize As Boolean = True
    Private CanChangeBitrate As Boolean = True
    Private CanIgnoreTip As Boolean = True
    Private IsLoading As Boolean = True
    Private BlockBitrate, BlockSize As Boolean
    Private IsManualCheckForUpdates As Boolean
    Private SkipAssistant As Boolean
    Private BlockSourceTextBoxTextChanged As Boolean

    Sub New()
        MyBase.New()

        AddHandler Application.ThreadException, AddressOf g.OnUnhandledException
        g.MainForm = Me
        LoadSettings()
        MenuItemEx.UseTooltips = s.EnableTooltips

        InitializeComponent()

        ScaleClientSize(41, 26.5)

        If components Is Nothing Then components = New System.ComponentModel.Container

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

        tbTargetFile.ContextMenuStrip = TargetFileMenu
        tbSourceFile.ContextMenuStrip = SourceFileMenu
        tbAudioFile0.ContextMenuStrip = Audio0FileMenu
        tbAudioFile1.ContextMenuStrip = Audio1FileMenu

        Dim rc = "right-click"
        tbAudioFile0.SendMessageCue(rc, False)
        tbAudioFile1.SendMessageCue(rc, False)
        tbSourceFile.SendMessageCue(rc, False)
        tbTargetFile.SendMessageCue(rc, False)

        llEditAudio0.AddClickAction(AddressOf AudioEdit0ToolStripMenuItemClick)
        llEditAudio1.AddClickAction(AddressOf AudioEdit1ToolStripMenuItemClick)

        Icon = My.Resources.MainIcon

        MenuStrip.SuspendLayout()
        MenuStrip.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)

        CommandManager.AddCommandsFromObject(Me)
        CommandManager.AddCommandsFromObject(g.DefaultCommands)

        CustomMainMenu = New CustomMenu(AddressOf GetDefaultMenuMain,
                s.CustomMenuMainForm, CommandManager, MenuStrip)

        OpenProject(g.StartupTemplatePath)
        CustomMainMenu.AddKeyDownHandler(Me)
        CustomMainMenu.BuildMenu()
        UpdateAudioMenu()
        MenuStrip.ResumeLayout()
        SizeContextMenuStrip.SuspendLayout()

        CustomSizeMenu = New CustomMenu(AddressOf GetDefaultMenuSize,
                s.CustomMenuSize, CommandManager, SizeContextMenuStrip)

        CustomSizeMenu.AddKeyDownHandler(Me)
        CustomSizeMenu.BuildMenu()
        SizeContextMenuStrip.ResumeLayout()
        g.SetRenderer(MenuStrip)
        SetMenuStyle()
    End Sub

    Private Sub LoadSettings()
        Try
            s = SafeSerialization.Deserialize(New ApplicationSettings, g.SettingsFile)
        Catch ex As Exception
            Using td As New TaskDialog(Of String)
                td.MainInstruction = "The settings failed to load!"
                td.Content = ex.Message
                td.MainIcon = TaskDialogIcon.Error
                td.AddButton("Retry", "Retry")
                td.AddButton("Reset", "Reset")
                td.AddButton("Exit", "Exit")

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

    Sub SetMenuStyle()
        If ToolStripRendererEx.IsAutoRenderMode Then
            Dim col = ControlPaint.Dark(ToolStripRendererEx.ColorBorder, 0)

            llAudioProfile0.LinkColor = col
            llAudioProfile1.LinkColor = col
            llEditAudio0.LinkColor = col
            llEditAudio1.LinkColor = col
            blFilesize.LinkColor = col
            llMuxer.LinkColor = col
            blSourceParText.LinkColor = col
            blSourceDarText.LinkColor = col
            blTargetParText.LinkColor = col
            blTargetDarText.LinkColor = col

            lgbEncoder.Label.LinkColor = col
            lgbFilters.Label.LinkColor = col
            lgbResize.Label.LinkColor = col
            lgbSource.Label.LinkColor = col
            lgbTarget.Label.LinkColor = col
        Else
            llAudioProfile0.LinkColor = Color.Blue
            llAudioProfile1.LinkColor = Color.Blue
            llEditAudio0.LinkColor = Color.Blue
            llEditAudio1.LinkColor = Color.Blue
            blFilesize.LinkColor = Color.Blue
            llMuxer.LinkColor = Color.Blue
            blSourceParText.LinkColor = Color.Blue
            blSourceDarText.LinkColor = Color.Blue
            blTargetParText.LinkColor = Color.Blue
            blTargetDarText.LinkColor = Color.Blue

            lgbEncoder.Label.LinkColor = Color.Blue
            lgbFilters.Label.LinkColor = Color.Blue
            lgbResize.Label.LinkColor = Color.Blue
            lgbSource.Label.LinkColor = Color.Blue
            lgbTarget.Label.LinkColor = Color.Blue
        End If
    End Sub

    Function GetIfoFile() As String
        Dim ret = FilePath.GetDir(p.SourceFile)
        If ret.EndsWith("_temp\") Then ret = DirPath.GetParent(ret)
        ret = ret + FilePath.GetName(p.SourceFile).DeleteRight(5) + "0.ifo"
        If File.Exists(ret) Then Return ret
    End Function

    Sub RenameDVDTracks()
        Dim ifoPath = GetIfoFile()

        If Not File.Exists(ifoPath) Then Exit Sub

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
                                Dim base = FilePath.GetBase(i)
                                base = regex.Replace(base, " ID" & x & " ")

                                If base.Contains("2_0ch") Then base = base.Replace("2_0ch", "2ch")
                                If base.Contains("3_2ch") Then base = base.Replace("3_2ch", "6ch")
                                If base.Contains(" DELAY") Then base = base.Replace(" DELAY", "")
                                If base.Contains(" 0ms") Then base = base.Replace(" 0ms", "")
                                If stream.Language.TwoLetterCode <> "iv" Then base += " " + stream.Language.Name
                                If stream.Title <> "" Then base += " " + stream.Title
                                FileHelp.Move(i, i.Dir + base + i.ExtFull)
                            End If
                        Next
                    End Using
                End If
            End If
        Next
    End Sub

    Private Sub DetectAudioFiles(track As Integer,
                                 lang As Boolean,
                                 same As Boolean,
                                 hq As Boolean)

        Dim tb, tbOther As TextBox
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
            If iExt = "avs" Then Continue For

            For Each iPath In files
                If track = 1 AndAlso iPath.Base = p.Audio0.File.Base Then Continue For
                If track = 0 AndAlso iPath.Base = p.Audio1.File.Base Then Continue For

                If tbOther.Text = iPath Then Continue For
                If Not FilePath.GetExt(iPath) = iExt Then Continue For
                If iPath.Contains("_cut_") Then Continue For
                If iPath.Contains("_out") Then Continue For
                If Not g.IsSourceSame(iPath) Then Continue For
                If hq AndAlso Not iPath.Ext.EqualsAny("dtsma", "thd", "eac3", "thd+ac3", "dtshr") Then Continue For

                If same AndAlso tbOther.Text <> "" AndAlso tbOther.Text.ExtFull <> iPath.ExtFull Then
                    Continue For
                End If

                If lang Then
                    Dim lng = profile.Language

                    If profile.Language.ThreeLetterCode = "und" Then
                        lng = If(track = 0, New Language(), New Language("en"))
                    End If

                    If Not iPath.Contains(lng.Name) Then Continue For
                End If

                If iPath.Ext = "mp4" AndAlso FilePath.IsSameBase(p.SourceFile, iPath) Then
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
            Using td As New TaskDialog(Of DialogResult)
                td.MainInstruction = "Save changed project?"
                td.AddButton("Save", DialogResult.Yes)
                td.AddButton("Don't Save", DialogResult.No)
                td.AddButton("Cancel", DialogResult.Cancel)
                td.Show()
                Refresh()

                If td.SelectedValue = DialogResult.Yes Then
                    If g.ProjectPath Is Nothing Then
                        If Not OpenSaveDialog() Then
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
        If Disposing OrElse IsDisposed Then Exit Sub

        For Each i In CustomMainMenu.MenuItems
            If i.CustomMenuItem?.MethodName = "DynamicMenuItem" AndAlso
                i.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.RecentProjects) Then

                i.DropDownItems.ClearAndDisplose

                SyncLock s.RecentProjects
                    For Each i2 In s.RecentProjects
                        If File.Exists(i2) AndAlso Not i2.Base = "recover" Then
                            Dim name = i2
                            If i2.Length > 70 Then name = "..." + name.Remove(0, name.Length - 70)
                            i.DropDownItems.Add(New ActionMenuItem(name, Sub() LoadProject(i2)))
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

        For Each i In CustomMainMenu.MenuItems
            If i.CustomMenuItem.MethodName = "DynamicMenuItem" Then
                If i.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.HelpApplications) Then
                    i.DropDownItems.ClearAndDisplose

                    For Each iPackage In Package.Items.Values
                        Dim helpPath = iPackage.GetHelpPath

                        If helpPath <> "" Then
                            Dim plugin = TryCast(iPackage, PluginPackage)

                            If plugin Is Nothing Then
                                ActionMenuItem.Add(i.DropDownItems, iPackage.Name.Substring(0, 1).Upper + " | " + iPackage.Name, Sub() g.StartProcess(helpPath))
                            Else
                                If plugin.AvsFilterNames?.Length > 0 Then ActionMenuItem.Add(i.DropDownItems, iPackage.Name.Substring(0, 1).Upper + " | " + iPackage.Name + " (AviSynth)", Sub() g.StartProcess(iPackage.GetHelpPath(ScriptEngine.AviSynth)))
                                If plugin.VSFilterNames?.Length > 0 Then ActionMenuItem.Add(i.DropDownItems, iPackage.Name.Substring(0, 1).Upper + " | " + iPackage.Name + " (VapourSynth)", Sub() g.StartProcess(iPackage.GetHelpPath(ScriptEngine.VapourSynth)))
                            End If
                        End If
                    Next
                End If
            End If
        Next

        Application.DoEvents()
    End Sub

    Async Sub UpdateScriptsMenuAsync()
        Dim files As String()
        Dim events As String()

        Await Task.Run(Sub()
                           Thread.Sleep(500)

                           If Directory.Exists(Folder.Apps + "Scripts") Then
                               For Each script In Directory.GetFiles(Folder.Apps + "Scripts")
                                   If Not s.Storage.GetBool(script.FileName) AndAlso
                                       Not File.Exists(Folder.Script + script.FileName) Then

                                       FileHelp.Copy(script, Folder.Script + script.FileName)
                                       s.Storage.SetBool(script.FileName, True)
                                   End If
                               Next
                           End If

                           events = System.Enum.GetNames(GetType(ApplicationEvent))
                           files = Directory.GetFiles(Folder.Script)
                       End Sub)

        If IsDisposed OrElse Native.GetForegroundWindow() <> Handle Then Exit Sub

        For Each menuItem In CustomMainMenu.MenuItems
            If menuItem.CustomMenuItem.MethodName = "DynamicMenuItem" Then
                If menuItem.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.Scripts) Then
                    menuItem.DropDownItems.ClearAndDisplose

                    For Each path In files
                        If Not events.Contains(path.FileName.Left(".")) AndAlso Not path.FileName.StartsWith("_") Then
                            ActionMenuItem.Add(menuItem.DropDownItems,
                                               path.FileName.Base,
                                               Sub() g.DefaultCommands.ExecuteScriptFile(path))
                        End If
                    Next

                    menuItem.DropDownItems.Add(New ToolStripSeparator)
                    ActionMenuItem.Add(menuItem.DropDownItems, "Open Scripts Folder", Sub() g.StartProcess(Folder.Script))
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

        If IsDisposed Then Exit Sub

        For Each i In CustomMainMenu.MenuItems
            If i.CustomMenuItem.MethodName = "DynamicMenuItem" AndAlso
                i.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.TemplateProjects) Then

                i.DropDownItems.ClearAndDisplose
                Dim items As New List(Of ActionMenuItem)

                For Each i2 In files
                    Dim base = i2.Base
                    If i2 = g.StartupTemplatePath Then base += " (Startup)"
                    If i2.Contains("Backup\") Then base = "Backup | " + base
                    ActionMenuItem.Add(i.DropDownItems, base, AddressOf LoadProject, i2, Nothing)
                Next

                i.DropDownItems.Add("-")
                ActionMenuItem.Add(i.DropDownItems, "Explore", Sub() g.StartProcess(Folder.Template), "Opens the directory containing the templates.")
                ActionMenuItem.Add(i.DropDownItems, "Restore", AddressOf ResetTemplates, "Restores the default templates.")

                Exit For
            End If
        Next

        Application.DoEvents()
    End Sub

    Sub ResetTemplates()
        If MsgQuestion("Restore the default templates?") = DialogResult.OK Then
            Try
                DirectoryHelp.Delete(Folder.Template)
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

    Function OpenSaveDialog() As Boolean
        Using d As New SaveFileDialog
            d.SetInitDir(p.TempDir)

            If p.SourceFile <> "" Then
                d.FileName = p.TargetFile.Base
            Else
                d.FileName = "Untitled"
            End If

            d.Filter = "StaxRip Project Files (*.srip)|*.srip"

            If d.ShowDialog() = DialogResult.OK Then
                If Not d.FileName.ToLower.EndsWith(".srip") Then
                    d.FileName += ".srip"
                End If

                SaveProjectPath(d.FileName)
                Return True
            End If
        End Using
    End Function

    Sub SetBindings(proj As Project, add As Boolean)
        SetTextBoxBinding(tbTargetFile, proj, NameOf(Project.TargetFile), add)

        If add Then
            AddHandler proj.PropertyChanged, AddressOf ProjectPropertyChanged
        Else
            RemoveHandler proj.PropertyChanged, AddressOf ProjectPropertyChanged
        End If
    End Sub

    Sub ProjectPropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        Assistant()
    End Sub

    Sub SetTextBoxBinding(tb As TextBox, obj As Object, prop As String, add As Boolean)
        If add Then
            tb.DataBindings.Add(New Binding(NameOf(TextBox.Text), obj, prop, False, DataSourceUpdateMode.OnPropertyChanged))
        Else
            tb.DataBindings.Clear()
        End If
    End Sub

    Function OpenProject(Optional path As String = Nothing) As Boolean
        Return OpenProject(path, True)
    End Function

    Function OpenProject(path As String, saveCurrent As Boolean) As Boolean
        If Not IsLoading AndAlso saveCurrent AndAlso IsSaveCanceled() Then Return False
        SetBindings(p, False)
        If path = "" OrElse Not File.Exists(path) Then path = g.StartupTemplatePath

        Try
            p = SafeSerialization.Deserialize(New Project, path)
        Catch ex As Exception
            g.ShowException(ex, "The project file failed to load, it will be reset to defaults." + BR2 + path)
            p = New Project
            p.Init()
        End Try

        Log = p.Log

        If File.Exists(Folder.Temp + "staxrip.log") Then FileHelp.Delete(Folder.Temp + "staxrip.log")

        SetBindings(p, True)

        Text = Application.ProductName + " - " + path.Base
        SkipAssistant = True

        If path.StartsWith(Folder.Template) Then
            g.ProjectPath = Nothing
            p.TemplateName = path.Base
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

        If p.SourceFile <> "" Then s.LastSourceDir = p.SourceFile.Dir

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
        g.RaiseAppEvent(ApplicationEvent.ProjectLoaded)
        g.RaiseAppEvent(ApplicationEvent.ProjectOrSourceLoaded)
        FiltersListView.RebuildMenu()

        Return True
    End Function

    Sub SetSlider()
        Dim w = If(p.ResizeSliderMaxWidth = 0, p.SourceWidth, p.ResizeSliderMaxWidth)
        tbResize.Maximum = CInt((Calc.FixMod16(w) - 320) / p.ForcedOutputMod)
    End Sub

    Sub SetSavedProject()
        g.SavedProject = StaxRip.ObjectHelp.GetCopy(Of Project)(p)
    End Sub

    Function GetPathFromIndexFile(sourcePath As String) As String
        For Each i In File.ReadAllLines(sourcePath)
            If i.Contains(":\") OrElse i.StartsWith("\\") Then
                If Regex.IsMatch(i, "^.+ \d+$") Then i = i.LeftLast(" ")

                If File.Exists(i) AndAlso FileTypes.Video.Contains(i.Ext) Then
                    Return i
                End If
            End If
        Next
    End Function

    Function ShowSourceFilterSelectionDialog(inputFile As String) As VideoFilter
        Dim filters As New List(Of VideoFilter)

        If inputFile.Ext = "dgi" OrElse FileTypes.DGDecNVInput.Contains(inputFile.Ext) Then AddSourceFilters({"DGSource"}, filters)
        If inputFile.Ext = "dgim" OrElse FileTypes.DGDecNVInput.Contains(inputFile.Ext) Then AddSourceFilters({"DGSourceIM"}, filters)
        If inputFile.Ext.EqualsAny("mp4", "m4v", "mov") Then AddSourceFilters({"LSMASHVideoSource", "LibavSMASHSource"}, filters)
        If FileTypes.Video.Contains(inputFile.Ext) AndAlso Not FileTypes.VideoText.Contains(inputFile.Ext) Then AddSourceFilters({"FFVideoSource", "LWLibavVideoSource", "ffms2", "LWLibavSource"}, filters)
        If g.IsCOMObjectRegistered(GUIDS.LAVSplitter) AndAlso g.IsCOMObjectRegistered(GUIDS.LAVVideoDecoder) Then AddSourceFilters({"DSS2"}, filters)
        If {"avi", "avs", "vdr"}.Contains(inputFile.Ext) Then AddSourceFilters({"AVISource"}, filters)
        If inputFile.Ext = "d2v" Then AddSourceFilters({"d2vsource"}, filters)

        If p.Script.Engine = ScriptEngine.AviSynth Then
            filters = filters.Where(Function(filter) Not filter.Script.Replace(" ", "").Contains("clip=core.")).ToList
            filters.Insert(0, New VideoFilter("Source", "Automatic", "#avs"))
        Else
            filters = filters.Where(Function(filter) filter.Script.Replace(" ", "").Contains("clip=core.")).ToList
            filters.Insert(0, New VideoFilter("Source", "Automatic", "#vs"))
        End If

        Dim td As New TaskDialog(Of VideoFilter)

        If p.Script.Engine = ScriptEngine.AviSynth Then
            td.MainInstruction = "Select a AviSynth source filter."
        Else
            td.MainInstruction = "Select a VapourSynth source filter."
        End If

        For Each filter In filters
            td.AddCommandLink(filter.Name, filter)
        Next

        Dim ret = td.Show
        If ret Is Nothing Then Throw New AbortException
        ret.Script = Macro.ExpandGUI(ret.Script, True).Value
        Return ret
    End Function

    Sub AddSourceFilters(filterNames As String(), filters As List(Of VideoFilter))
        Dim avsProfiles = s.AviSynthProfiles.Where(Function(cat) cat.Name = "Source").First.Filters
        Dim vsProfiles = s.VapourSynthProfiles.Where(Function(cat) cat.Name = "Source").First.Filters
        Dim allFilters = avsProfiles.Concat(vsProfiles)

        For Each filter In allFilters
            For Each filterName In filterNames
                If filter.Script.ToLower.Contains(filterName.ToLower + "(") OrElse
                    filter.Script.ToLower.Contains(filterName.ToLower + ".") Then

                    If filters.Where(Function(val) val.Name = filter.Name).Count = 0 Then
                        filters.Add(filter.GetCopy)
                    End If
                End If
            Next
        Next
    End Sub

    Sub OpenAnyFile(files As IEnumerable(Of String))
        If files(0).Ext = "srip" Then
            OpenProject(files(0))
        ElseIf FileTypes.Video.Contains(files(0).Ext.Lower) Then
            files.Sort()
            OpenVideoSourceFiles(files)
        ElseIf FileTypes.Audio.Contains(files(0).Ext.Lower) Then
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

        AddHandler g.MainForm.Disposed, Sub() FileHelp.Delete(recoverProjectPath)

        Try
            If g.ShowVideoSourceWarnings(files) Then Throw New AbortException

            For Each i In files
                Dim name = FilePath.GetName(i)

                If name.ToUpper Like "VTS_0#_0.VOB" Then
                    If MsgQuestion("Are you sure you want to open the file " + name + "," + BR +
                           "the first VOB file usually contains a menu!") = DialogResult.Cancel Then

                        Throw New AbortException
                    End If
                End If

                If name.ToUpper = "VIDEO_TS.VOB" Then
                    MsgWarn("The file VIDEO_TS.VOB can't be opened.")
                    Throw New AbortException
                End If
            Next

            If p.SourceFile <> "" AndAlso Not isEncoding Then
                Dim templates = Directory.GetFiles(Folder.Template, "*.srip")

                If templates.Length = 1 Then
                    If Not OpenProject(templates(0), True) Then Throw New AbortException
                Else
                    If s.ShowTemplateSelection Then
                        If Not LoadTemplateWithSelectionDialog() Then Throw New AbortException
                    Else
                        If Not OpenProject() Then Throw New AbortException
                    End If
                End If
            End If

            p.SourceFiles = files.ToList
            p.SourceFile = files(0)

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

                    If p.Script.Engine = ScriptEngine.AviSynth Then
                        p.Script = VideoScript.GetDefaults()(1)
                    End If
                Else
                    If Not Package.AviSynth.VerifyOK(True) Then Throw New AbortException

                    If p.Script.Engine = ScriptEngine.VapourSynth Then
                        p.Script = VideoScript.GetDefaults()(0)
                    End If
                End If

                p.Script.SetFilter(preferredSourceFilter.Category,
                                   preferredSourceFilter.Name,
                                   preferredSourceFilter.Script)
            End If

            If Not g.VerifyRequirements() Then Throw New AbortException

            p.LastOriginalSourceFile = p.SourceFile
            p.FirstOriginalSourceFile = p.SourceFile

            If FileTypes.VideoIndex.Contains(p.SourceFile.Ext) Then
                Dim path = GetPathFromIndexFile(p.SourceFile)

                If path <> "" Then
                    p.LastOriginalSourceFile = path
                    p.FirstOriginalSourceFile = path
                End If
            ElseIf p.SourceFile.Ext.EqualsAny({"avs", "vpy"}) Then
                Dim code = File.ReadAllText(p.SourceFile)
                Dim matches = Regex.Matches(code, "('|"")(.+?)\1", RegexOptions.IgnoreCase)

                For Each match As Match In matches
                    If match.Success Then
                        Dim path = match.Groups(2).Value

                        If Not path.Contains("\") AndAlso File.Exists(p.SourceFile.Dir + path) Then
                            p.TempDir = p.SourceFile.Dir
                            path = p.SourceFile.Dir + path
                        End If

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

            p.Codec = MediaInfo.GetVideoCodec(p.LastOriginalSourceFile)
            p.CodecProfile = MediaInfo.GetVideo(p.LastOriginalSourceFile, "Format_Profile")
            p.BitDepth = MediaInfo.GetVideo(p.LastOriginalSourceFile, "BitDepth").ToInt
            p.ColorSpace = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ColorSpace")
            p.ChromaSubsampling = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ChromaSubsampling")
            p.SourceSize = New FileInfo(p.LastOriginalSourceFile).Length
            p.SourceBitrate = CInt(MediaInfo.GetVideo(p.LastOriginalSourceFile, "BitRate").ToInt / 1000)
            p.ScanType = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ScanType")
            p.ScanOrder = MediaInfo.GetVideo(p.LastOriginalSourceFile, "ScanOrder")

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

            If Not Directory.Exists(targetDir) Then targetDir = p.SourceFile.Dir
            Dim targetName = Macro.Expand(p.DefaultTargetName)

            If Not FilePath.IsValidFileSystemName(targetName) Then targetName = p.SourceFile.Base
            tbTargetFile.Text = targetDir + targetName + p.VideoEncoder.Muxer.OutputExtFull

            If p.SourceFile = p.TargetFile OrElse
                (FileTypes.VideoIndex.Contains(p.SourceFile.Ext) AndAlso
                File.ReadAllText(p.SourceFile).Contains(p.TargetFile)) Then

                tbTargetFile.Text = p.TargetFile.DirAndBase + "_new" + p.TargetFile.ExtFull
            End If

            Log.WriteHeader("MediaInfo Source File")

            For Each i In p.SourceFiles
                Log.WriteLine(i)
            Next

            Log.WriteLine(BR + MediaInfo.GetSummary(p.SourceFile))

            For Each i In DriveInfo.GetDrives()
                If i.DriveType = DriveType.CDRom AndAlso
                    p.TempDir.ToUpper.StartsWith(i.RootDirectory.ToString.ToUpper) Then

                    MsgWarn("Opening files from a optical drive requires to set a temp files folder in the options.")
                    Throw New AbortException
                End If
            Next

            Demux()

            If p.LastOriginalSourceFile <> p.SourceFile AndAlso
                Not FileTypes.VideoText.Contains(FilePath.GetExt(p.SourceFile)) Then

                p.LastOriginalSourceFile = p.SourceFile
            End If

            s.LastSourceDir = p.SourceFile.Dir

            If p.SourceFile.Ext = "avs" AndAlso p.Script.Engine = ScriptEngine.AviSynth Then
                p.Script.Filters.Clear()
                p.Script.Filters.Add(New VideoFilter("Source", "AviSynth Import", File.ReadAllText(p.SourceFile)))
            ElseIf p.SourceFile.Ext = "vpy" Then
                p.Script.Engine = ScriptEngine.VapourSynth
                p.Script.Filters.Clear()
                p.Script.Filters.Add(New VideoFilter("Source", "VapourSynth Import", File.ReadAllText(p.SourceFile)))
            End If

            ModifyFilters()
            FiltersListView.IsLoading = False
            FiltersListView.Load()

            If Not Package.DGDecodeNV.VerifyOK() OrElse Not Package.DGDecodeIM.VerifyOK() Then
                Throw New AbortException
            End If

            RenameDVDTracks()

            If FileTypes.VideoAudio.Contains(FilePath.GetExt(p.LastOriginalSourceFile)) Then
                p.Audio0.Streams = MediaInfo.GetAudioStreams(p.LastOriginalSourceFile)
                p.Audio1.Streams = p.Audio0.Streams
            End If

            If p.SourceFile.Ext = "d2v" Then
                Dim content = File.ReadAllText(p.SourceFile)

                If content.Contains("Aspect_Ratio=16:9") Then
                    p.SourceAnamorphic = True
                Else
                    Dim ifoFile = GetIfoFile()

                    If ifoFile <> "" Then
                        Dim dar2 = MediaInfo.GetVideo(ifoFile, "DisplayAspectRatio")
                        If dar2 = "1.778" Then p.SourceAnamorphic = True
                    End If
                End If

                If content.Contains("Frame_Rate=29970") Then
                    Dim m = Regex.Match(content, "FINISHED +(\d+).+FILM")

                    If m.Success Then
                        Dim film = m.Groups(1).Value.ToInt

                        If film >= 95 Then
                            content = content.Replace("Field_Operation=0" + BR + "Frame_Rate=29970 (30000/1001)", "Field_Operation=1" + BR + "Frame_Rate=23976 (24000/1001)")
                            content.WriteANSIFile(p.SourceFile)
                        End If
                    End If
                End If
            End If

            Dim errorMsg = ""

            Try
                p.SourceScript.Synchronize()
                errorMsg = p.SourceScript.GetErrorMessage
            Catch ex As Exception
                errorMsg = ex.Message
            End Try

            If errorMsg <> "" Then
                If p.SourceFile.Ext = "avs" OrElse p.SourceFile.Ext = "vpy" Then
                    MsgError("Failed to load script.", errorMsg)
                    Throw New AbortException
                Else
                    Log.WriteHeader("Error opening source")
                    Log.WriteLine(errorMsg + BR2)
                    Log.WriteLine(p.SourceScript.GetFullScript)
                    Log.Save()

                    g.ShowDirectShowWarning()

                    Using td As New TaskDialog(Of DialogResult)
                        td.MainInstruction = "Failed to open source, try another source filter?"
                        td.Content = errorMsg
                        td.CommonButtons = TaskDialogButtons.OkCancel

                        If td.Show = DialogResult.OK Then
                            Dim f = ShowSourceFilterSelectionDialog(p.SourceFile)
                            Dim isVapourSynth = f.Script?.Contains("clip = core.")

                            If isVapourSynth Then
                                If p.Script.Engine = ScriptEngine.AviSynth Then
                                    p.Script = VideoScript.GetDefaults()(1)
                                End If
                            Else
                                If p.Script.Engine = ScriptEngine.VapourSynth Then
                                    p.Script = VideoScript.GetDefaults()(0)
                                End If
                            End If

                            If f.Script?.Contains("(") Then p.Script.SetFilter(0, f)
                        Else
                            p.Script.Synchronize()
                            Throw New AbortException
                        End If
                    End Using

                    errorMsg = ""

                    Try
                        p.SourceScript.Synchronize()
                        errorMsg = p.SourceScript.GetErrorMessage
                    Catch ex As Exception
                        errorMsg = ex.Message
                    End Try

                    If errorMsg <> "" Then
                        MsgError("Failed to open source", errorMsg)
                        p.Script.Synchronize()
                        Throw New AbortException
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
                If p.Audio0.File = "" AndAlso p.Audio1.File = "" Then
                    If Not TypeOf p.Audio0 Is NullAudioProfile AndAlso
                        Not FileTypes.VideoText.Contains(FilePath.GetExt(p.LastOriginalSourceFile)) Then

                        tbAudioFile0.Text = p.LastOriginalSourceFile
                        If p.Audio0.Streams.Count = 0 Then tbAudioFile0.Text = ""

                        If Not TypeOf p.Audio1 Is NullAudioProfile AndAlso
                            p.Audio0.Streams.Count > 1 Then

                            tbAudioFile1.Text = p.LastOriginalSourceFile

                            For Each i In p.Audio1.Streams
                                If Not p.Audio0.Stream Is Nothing AndAlso
                                    Not p.Audio1.Stream Is Nothing Then

                                    If p.Audio0.Stream.StreamOrder = p.Audio1.Stream.StreamOrder Then
                                        For Each i2 In p.Audio1.Streams
                                            If i2.StreamOrder <> p.Audio1.Stream.StreamOrder Then
                                                tbAudioFile1.Text = i2.Name + " (" + FilePath.GetExt(p.Audio1.File) + ")"
                                                p.Audio1.Stream = i2
                                                UpdateSizeOrBitrate()
                                                Exit For
                                            End If
                                        Next
                                    End If
                                End If
                            Next
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

            If p.HarcodedSubtitle Then g.AddHardcodedSubtitle()

            Dim isCropActive = p.Script.IsFilterActive("Crop")

            If isCropActive AndAlso (p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) = 0 Then
                g.RunAutoCrop()
                DisableCropFilter()
            End If

            AutoResize()

            If isCropActive Then
                g.OvercropWidth()
                If p.AutoSmartCrop Then g.SmartCrop()
            End If

            If p.AutoCompCheck AndAlso p.VideoEncoder.IsCompCheckEnabled Then p.VideoEncoder.RunCompCheck()
            Assistant()
            g.RaiseAppEvent(ApplicationEvent.AfterSourceLoaded)
            g.RaiseAppEvent(ApplicationEvent.ProjectOrSourceLoaded)
            Log.Save()
        Catch ex As AbortException
            Log.Save()
            SetSavedProject()
            OpenProject(recoverProjectPath)
            Text = recoverText
            g.ProjectPath = recoverPath
            If isEncoding Then Throw New AbortException
        Catch ex As Exception
            g.OnException(ex)
            OpenProject("", False)
        Finally
            If Not isEncoding Then
                g.WriteDebugLog("ProcController.Finished OpenVideoSourceFiles")
                ProcController.Finished()
            Else
                g.WriteDebugLog("ProcController.Finished isEncoding")
            End If
        End Try
    End Sub

    Private Sub ModifyFilters()
        If p.SourceFile = "" Then Exit Sub
        Dim sourceFilter = p.Script.GetFilter("Source")

        If Not sourceFilter.Script.Contains("(") OrElse
                p.Script.Filters(0).Name = "Automatic" OrElse
                p.Script.Filters(0).Name = "Manual" Then

            For Each iPref In {s.AviSynthFilterPreferences, s.VapourSynthFilterPreferences}
                If (iPref Is s.AviSynthFilterPreferences AndAlso
                        p.Script.Engine = ScriptEngine.AviSynth) OrElse
                        (iPref Is s.VapourSynthFilterPreferences AndAlso
                        p.Script.Engine = ScriptEngine.VapourSynth) Then

                    Dim scriptingProfiles = If(p.Script.Engine = ScriptEngine.AviSynth,
                            s.AviSynthProfiles, s.VapourSynthProfiles)

                    For Each i In iPref
                        Dim name = i.Name.SplitNoEmptyAndWhiteSpace({",", " "})

                        If name.Contains(FilePath.GetExt(p.SourceFile)) Then
                            Dim filters = scriptingProfiles.Where(
                                    Function(v) v.Name = "Source").First.Filters.Where(
                                    Function(v) v.Name = i.Value)

                            If filters.Count > 0 Then
                                p.Script.SetFilter("Source", filters(0).Name, filters(0).Script)
                                Exit For
                            End If
                        End If
                    Next

                    If Not sourceFilter.Script.Contains("(") Then
                        Dim def = iPref.Where(Function(v) v.Name = "default")

                        If def.Count > 0 Then
                            Dim filters = scriptingProfiles.Where(
                                    Function(v) v.Name = "Source").First.Filters.Where(
                                    Function(v) v.Name = def(0).Value)

                            If filters.Count > 0 Then p.Script.SetFilter("Source", filters(0).Name, filters(0).Script)
                        End If
                    End If
                End If
            Next
        End If

        Dim editAVS = p.Script.Engine = ScriptEngine.AviSynth AndAlso p.SourceFile.Ext <> "avs"
        Dim editVS = p.Script.Engine = ScriptEngine.VapourSynth AndAlso p.SourceFile.Ext <> "vpy"

        If editAVS Then
            If Not sourceFilter.Script.Contains("(") Then
                Dim filter = FilterCategory.GetAviSynthDefaults.Where(Function(v) v.Name = "Source").First.Filters.Where(Function(v) v.Name = "FFVideoSource").First
                p.Script.SetFilter(filter.Category, filter.Name, filter.Script)
                Indexing()
            End If
        ElseIf editVS Then
            If Not sourceFilter.Script.Contains("(") Then
                Dim filter = FilterCategory.GetVapourSynthDefaults.Where(Function(v) v.Name = "Source").First.Filters.Where(Function(v) v.Name = "ffms2").First
                p.Script.SetFilter(filter.Category, filter.Name, filter.Script)
                Indexing()
            End If
        End If

        For Each iFilter In p.Script.Filters
            If iFilter.Script.Contains("$") Then iFilter.Script = Macro.ExpandGUI(iFilter.Script, True).Value
        Next

        If editAVS Then
            Dim miFPS = MediaInfo.GetFrameRate(p.FirstOriginalSourceFile)
            Dim avsFPS = p.SourceScript.GetFramerate

            If (CInt(miFPS) * 2) = CInt(avsFPS) Then
                Dim src = p.Script.GetFilter("Source")
                src.Script = src.Script + BR + "SelectEven().AssumeFPS(" & miFPS.ToInvariantString + ")"
                p.SourceScript.Synchronize()
            End If
        End If

        'chroma
        If editVS Then
            If p.ChromaSubsampling <> "4:2:0" Then
                Dim sourceHeight = MediaInfo.GetVideo(p.LastOriginalSourceFile, "Height").ToInt
                Dim matrix As String

                If sourceHeight = 0 OrElse sourceHeight > 576 Then
                    matrix = "709"
                Else
                    matrix = "470bg"
                End If

                p.Script.GetFilter("Source").Script += BR + "clip = clip.resize.Bicubic(matrix_s = '" + matrix + "', format = vs.YUV420P8)"
            End If
        ElseIf editAVS Then
            If Not sourceFilter.Script.Contains("ConvertToYV12") Then
                If p.ChromaSubsampling <> "4:2:0" Then
                    Dim format = MediaInfo.GetVideo(p.LastOriginalSourceFile, "Format")
                    Dim matrix As String

                    If format = "RGB" OrElse p.SourceFile.Ext = "vdr" Then
                        Dim sourceHeight = MediaInfo.GetVideo(p.LastOriginalSourceFile, "Height").ToInt

                        If sourceHeight > 576 Then
                            matrix = "matrix = ""Rec709"""
                        Else
                            matrix = "matrix = ""Rec601"""
                        End If
                    End If

                    p.Script.GetFilter("Source").Script += BR + "ConvertToYV12(" + matrix + ")"
                End If
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
                    proc.Arguments = "-o """ + i.DirAndBase + ".idx"" """ + i + """"
                    proc.AllowedExitCodes = {}
                    proc.Start()
                End Using
            End If
        Next
    End Sub

    Sub ExtractForcedVobSubSubtitles()
        For Each i In g.GetFilesInTempDirAndParent
            If i.ExtFull = ".idx" AndAlso g.IsSourceSameOrSimilar(i) AndAlso
                    Not i.Contains("_forced") AndAlso
                    Not File.Exists(i.DirAndBase + "_forced.idx") Then

                Dim idxContent = File.ReadAllText(i, Encoding.Default)

                If idxContent.Contains(VB6.ChrW(&HA) + VB6.ChrW(&H0) + VB6.ChrW(&HD) + VB6.ChrW(&HA)) Then
                    idxContent = idxContent.FixBreak
                    idxContent = idxContent.Replace(BR + VB6.ChrW(&H0) + BR, BR + "langidx: 0" + BR)
                    File.WriteAllText(i, idxContent, Encoding.Default)
                End If

                Using proc As New Proc
                    proc.Header = "Extract forced subtitles if existing"
                    proc.SkipString = "# "
                    proc.WriteLog(FilePath.GetName(i) + BR2)
                    proc.File = Package.BDSup2SubPP.Path
                    proc.Arguments = "--forced-only -o " + (i.DirAndBase + "_forced.idx").Escape + " " + i.Escape
                    proc.AllowedExitCodes = {}
                    proc.Start()
                End Using
            End If
        Next
    End Sub

    Sub DemuxVobSubSubtitles()
        If Not {"vob", "m2v"}.Contains(FilePath.GetExt(p.LastOriginalSourceFile)) Then Exit Sub
        Dim ifoPath = GetIfoFile()
        If ifoPath = "" Then Exit Sub
        If File.Exists(p.TempDir + p.SourceFile.Base + ".idx") Then Exit Sub
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
                        args.WriteANSIFile(fileContent)

                        Using proc As New Proc
                            proc.Header = "Demux subtitles using VSRip"
                            proc.WriteLog(args + BR2)
                            proc.File = Package.VSRip.Path
                            proc.Arguments = """" + fileContent + """"
                            proc.WorkingDirectory = Package.VSRip.GetDir
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
        If message.Contains(BR2) Then message = message.Replace(BR2, BR)
        If message.Contains(VB6.vbLf + VB6.vbLf) Then message = message.FixBreak.Replace(BR2, BR)

        CurrentAssistantTipKey = message.SHA512Hash

        If Not p.SkippedAssistantTips.Contains(CurrentAssistantTipKey) Then
            If message <> "" Then
                If message.Length > 130 Then
                    lTip.Font = New Font(lTip.Font.FontFamily, 7 * s.UIScaleFactor)
                Else
                    lTip.Font = New Font(lTip.Font.FontFamily, 9 * s.UIScaleFactor)
                End If
            End If

            lTip.Text = message
            Return True
        End If
    End Function

    Private AssistantMethodValue As Action

    Property AssistantMethod() As Action
        Get
            Return AssistantMethodValue
        End Get
        Set(Value As Action)
            AssistantMethodValue = Value
        End Set
    End Property

    Function Assistant() As Boolean
        If SkipAssistant Then Return False

        Dim isCropped = p.Script.IsFilterActive("Crop")
        Dim isResized = p.Script.IsFilterActive("Resize")

        tbTargetWidth.ReadOnly = Not isResized
        tbTargetHeight.ReadOnly = Not isResized

        g.Highlight(False, lSAR)
        g.Highlight(False, llAudioProfile0)
        g.Highlight(False, llAudioProfile1)
        g.Highlight(False, lAspectRatioError)
        g.Highlight(False, lZoom)
        g.Highlight(False, tbAudioFile0)
        g.Highlight(False, tbAudioFile1)
        g.Highlight(False, tbBitrate)
        g.Highlight(False, tbTargetSize)
        g.Highlight(False, tbSourceFile)
        g.Highlight(False, tbTargetHeight)
        g.Highlight(False, tbTargetFile)
        g.Highlight(False, tbTargetWidth)
        g.Highlight(False, llMuxer)
        g.Highlight(False, lgbEncoder.Label)
        g.Highlight(False, lTarget2)

        Dim cropw = p.SourceWidth
        Dim croph = p.SourceHeight

        If isCropped Then
            cropw = p.SourceWidth - p.CropLeft - p.CropRight
            croph = p.SourceHeight - p.CropTop - p.CropBottom
        End If

        Dim isValidAnamorphicSize = (p.TargetWidth = 1440 AndAlso p.TargetHeight = 1080) OrElse
            (p.TargetWidth = 960 AndAlso p.TargetHeight = 720)

        If Not isResized Then
            If p.TargetWidth <> cropw Then tbTargetWidth.Text = cropw.ToString
            If p.TargetHeight <> croph Then tbTargetHeight.Text = croph.ToString
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
        If trackBarValue < tbResize.Minimum Then trackBarValue = tbResize.Minimum
        If trackBarValue > tbResize.Maximum Then trackBarValue = tbResize.Maximum

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
            lSource1.Text = lSource1.GetMaxTextSpace(
                g.GetTimeString(p.SourceSeconds),
                If(p.SourceSize / 1024 ^ 2 < 1024, CInt(p.SourceSize / 1024 ^ 2).ToString + "MB",
                (p.SourceSize / 1024 ^ 3).ToString("f1") + "GB"),
                If(p.SourceBitrate > 0, (p.SourceBitrate / 1000).ToString("f1") + "Mb/s", ""),
                p.SourceFrameRate.ToString.Shorten(9) + "fps",
                p.Codec, p.CodecProfile)

            lSource2.Text = lSource1.GetMaxTextSpace(
                p.SourceWidth.ToString + "x" + p.SourceHeight.ToString, p.ColorSpace,
                p.ChromaSubsampling, If(p.BitDepth <> 0, p.BitDepth & "Bits", ""),
                p.ScanType, If(p.ScanType = "Interlaced", p.ScanOrder, ""))

            lTarget1.Text = lSource1.GetMaxTextSpace(g.GetTimeString(p.TargetSeconds),
                p.TargetFrameRate.ToString.Shorten(9) + "fps", "Audio Bitrate: " & CInt(Calc.GetAudioBitrate))

            If p.VideoEncoder.IsCompCheckEnabled Then
                lTarget2.Text = lSource1.GetMaxTextSpace(
                    "Quality: " & CInt(Calc.GetPercent).ToString() + " %",
                    "Compressibility: " + p.Compressibility.ToString("f2"))
            End If
        Else
            lTarget1.Text = ""
            lSource1.Text = ""
            lTarget2.Text = ""
            lSource2.Text = ""
        End If

        lTip.Text = ""
        gbAssistant.Text = ""
        AssistantMethod = Nothing
        CanIgnoreTip = True
        AssistantPassed = False

        If p.VideoEncoder.Muxer.CoverFile <> "" AndAlso TypeOf p.VideoEncoder.Muxer Is MkvMuxer Then
            If Not p.VideoEncoder.Muxer.CoverFile.Base.EqualsAny("cover", "small_cover", "cover_land", "small_cover_land") OrElse Not p.VideoEncoder.Muxer.CoverFile.Ext.EqualsAny("jpg", "png") Then
                If ProcessTip("The cover file name bust be cover, small_cover, cover_land or small_cover_land, the file type must be jpg or png.") Then
                    gbAssistant.Text = "Invalid cover file name"
                    CanIgnoreTip = False
                    Return False
                End If
            End If
        End If

        If p.Script.Engine = ScriptEngine.VapourSynth AndAlso TypeOf p.VideoEncoder Is ffmpegEnc Then
            If ProcessTip("ffmpeg video encoding with VapourSynth input isn't supported.") Then
                gbAssistant.Text = "Incompatible settings"
                CanIgnoreTip = False
                Return False
            End If
        End If

        If TypeOf p.VideoEncoder Is BasicVideoEncoder Then
            Dim enc = DirectCast(p.VideoEncoder, BasicVideoEncoder)
            Dim param = enc.CommandLineParams.GetOptionParam("--vpp-resize")

            If Not param Is Nothing AndAlso param.Value > 0 AndAlso
                Not p.Script.IsFilterActive("Resize", "Hardware Encoder") Then

                If ProcessTip("In order to use an resize filter of the hardware encoder select 'Hardware Encoder' as resize filter from the filters menu.") Then
                    gbAssistant.Text = "Invalid filter setting"
                    CanIgnoreTip = False
                    Return False
                End If
            End If
        End If

        If Not p.BatchMode Then
            If p.TargetFile.Ext = "mp4" AndAlso p.TargetFile.Contains("#") Then
                If ProcessTip("Character # can't be processed by MP4Box, please rename target file.") Then
                    gbAssistant.Text = "Invalid target filename"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If p.Script.Filters.Count = 0 OrElse
                Not p.Script.Filters(0).Active OrElse
                p.Script.Filters(0).Category <> "Source" Then

                If ProcessTip("The first filter must have the category Source.") Then
                    gbAssistant.Text = "Invalid filter setup"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If p.SourceSeconds = 0 Then
                If ProcessTip("Click here to open a source file.") Then
                    AssistantMethod = AddressOf ShowOpenSourceDialog
                    gbAssistant.Text = "Assistant"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If p.SourceFile = p.TargetFile Then
                If ProcessTip("The source and target filepath is identical.") Then
                    g.Highlight(True, tbTargetFile)
                    gbAssistant.Text = "Invalid Targetpath"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            Dim audioTracks = p.GetAudioTracks

            For Each i In audioTracks
                If i.File = p.TargetFile Then
                    If ProcessTip("The audio source and target filepath is identical.") Then
                        g.Highlight(True, tbTargetFile)
                        gbAssistant.Text = "Invalid Targetpath"
                        CanIgnoreTip = False
                        Return False
                    End If
                End If
            Next

            If p.RemindToCrop AndAlso Not TypeOf p.VideoEncoder Is NullEncoder AndAlso
                ProcessTip("Click here to open the crop dialog. When done continue with Next.") Then

                AssistantMethod = AddressOf ShowCropDialog
                gbAssistant.Text = "Crop"
                Return False
            End If

            If (p.Audio0.File <> "" AndAlso p.Audio0.File = p.Audio1.File AndAlso
                p.Audio0.Stream Is Nothing) OrElse
                (Not p.Audio0.Stream Is Nothing AndAlso Not p.Audio1.Stream Is Nothing AndAlso
                p.Audio0.Stream.StreamOrder = p.Audio1.Stream.StreamOrder) Then

                If ProcessTip("The first and second audio source files or streams are identical.") Then
                    g.Highlight(True, tbAudioFile0)
                    g.Highlight(True, tbAudioFile1)
                    gbAssistant.Text = "Invalid Audio Settings"
                    Return False
                End If
            End If

            If TypeOf p.VideoEncoder.Muxer Is MP4Muxer AndAlso p.TargetFile.Contains("#") Then
                If ProcessTip("Filenames with character '#' are not supported by MP4Box.") Then
                    g.Highlight(True, tbTargetFile)
                    gbAssistant.Text = "Invalid Output Filename"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If Not p.VideoEncoder.Muxer.IsSupported(p.VideoEncoder.OutputExt) Then
                If ProcessTip("The encoder outputs '" + p.VideoEncoder.OutputExt + "' but the container '" + p.VideoEncoder.Muxer.Name + "' supports only " + p.VideoEncoder.Muxer.SupportedInputTypes.Join(", ") + ".") Then
                    gbAssistant.Text = "Encoder conflicts with container"
                    g.Highlight(True, llMuxer)
                    g.Highlight(True, lgbEncoder.Label)
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            For Each i In audioTracks
                If Math.Abs(i.Delay) > 2000 Then
                    If ProcessTip("The audio delay is unusual high indicating a sync problem, MakeMKV can prevent this problem.") Then
                        g.Highlight(True, tbAudioFile0)
                        gbAssistant.Text = "Unusual high audio delay"
                        Return False
                    End If
                End If
            Next

            For Each i In audioTracks
                If i.File <> "" AndAlso Not p.VideoEncoder.Muxer.IsSupported(i.OutputFileType) AndAlso Not i.OutputFileType = "ignore" Then
                    If ProcessTip("The audio format is '" + i.OutputFileType + "' but the container '" + p.VideoEncoder.Muxer.Name + "' supports only " + p.VideoEncoder.Muxer.SupportedInputTypes.Join(", ") + ". Select another audio profile or another container.") Then
                        g.Highlight(True, llMuxer)
                        gbAssistant.Text = "Audio format conflicts with container"
                        CanIgnoreTip = False
                        Return False
                    End If
                End If
            Next

            If p.VideoEncoder.Muxer.OutputExtFull <> p.TargetFile.ExtFull Then
                If ProcessTip("The container requires " + p.VideoEncoder.Muxer.OutputExt.ToUpper + " as target file type.") Then
                    g.Highlight(True, tbTargetFile)
                    gbAssistant.Text = "Invalid File Type"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If Not p.VideoEncoder.GetError Is Nothing Then
                If ProcessTip(p.VideoEncoder.GetError) Then
                    gbAssistant.Text = "Encoder Error"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            Dim ae = Calc.GetAspectRatioError()

            If Not isValidAnamorphicSize AndAlso (ae > p.MaxAspectRatioError OrElse
                ae < -p.MaxAspectRatioError) AndAlso p.Script.IsFilterActive("Resize") AndAlso
                p.RemindArError Then

                If ProcessTip("Use the resize slider to correct the aspect ratio error or click next to encode anamorphic.") Then
                    g.Highlight(True, lAspectRatioError)
                    gbAssistant.Text = "Aspect Ratio Error"
                    Return False
                End If
            End If

            If p.RemindToSetFilters AndAlso ProcessTip("Verify the filter setup, when done continue with Next.") Then
                gbAssistant.Text = "Filter Setup"
                Return False
            End If

            If p.Ranges.Count = 0 Then
                If p.RemindToCut AndAlso Not TypeOf p.VideoEncoder Is NullEncoder AndAlso
                    ProcessTip("Click here to open the preview for cutting if necessary. When done continue with Next.") Then

                    AssistantMethod = AddressOf ShowPreview
                    gbAssistant.Text = "Cutting"
                    Return False
                End If
            Else
                If p.CutFrameRate <> p.Script.GetFramerate Then
                    If ProcessTip("The frame rate was changed after cutting was performed, please ensure that this change is happening after the Cutting filter section in the AviSynth script.") Then
                        gbAssistant.Text = "Illegal frame rate change"
                        Return False
                    End If
                End If

                If Not p.Script.IsFilterActive("Cutting") AndAlso Form.ActiveForm Is Me Then
                    If ProcessTip("The cutting filter settings don't match with the cutting settings used in the preview.") Then
                        gbAssistant.Text = "Invalid Cutting Settings"
                        CanIgnoreTip = False
                        Return False
                    End If
                End If
            End If

            If p.RemindToDoCompCheck AndAlso p.VideoEncoder.IsCompCheckEnabled AndAlso p.Compressibility = 0 Then
                If ProcessTip("Click here to start the compressibility check. The compressibility check helps to finds the ideal bitrate or image size.") Then
                    AssistantMethod = AddressOf p.VideoEncoder.RunCompCheck
                    g.Highlight(True, lTarget2)
                    gbAssistant.Text = "Compressibility Check"
                    Return False
                End If
            End If

            If File.Exists(p.TargetFile) Then
                If FileTypes.VideoText.Contains(FilePath.GetExt(p.SourceFile)) AndAlso
                    File.ReadAllText(p.SourceFile).Contains(p.TargetFile) Then

                    If ProcessTip("Source and target name are identical, please select another target name.") Then
                        CanIgnoreTip = False
                        tbTargetFile.BackColor = Color.Yellow
                        gbAssistant.Text = "Target File"
                        Return False
                    End If
                Else
                    If ProcessTip("The target file already exist." + BR + p.TargetFile) Then
                        tbTargetFile.BackColor = Color.Yellow
                        gbAssistant.Text = "Target File"
                        Return False
                    End If
                End If
            End If

            If p.Script.IsFilterActive("Resize") Then
                If p.TargetWidth Mod p.ForcedOutputMod <> 0 Then
                    If ProcessTip("Move the resize slider to adjust the target width to be divisible by " & p.ForcedOutputMod & " or customize Options > Image > Output Mod.") Then
                        CanIgnoreTip = Not p.AutoCorrectCropValues
                        g.Highlight(True, tbTargetWidth)
                        g.Highlight(True, lSAR)
                        gbAssistant.Text = "Invalid Target Width"
                        Return False
                    End If
                End If

                If Not p.TargetHeight = 1080 AndAlso p.TargetHeight Mod p.ForcedOutputMod <> 0 Then
                    If ProcessTip("Move the resize slider to adjust the target height to be divisible by " & p.ForcedOutputMod & " or customize Options > Image > Output Mod.") Then
                        CanIgnoreTip = Not p.AutoCorrectCropValues
                        g.Highlight(True, tbTargetHeight)
                        g.Highlight(True, lSAR)
                        gbAssistant.Text = "Invalid Target Height"
                        Return False
                    End If
                End If
            Else
                If p.TargetWidth Mod p.ForcedOutputMod <> 0 Then
                    If ProcessTip("The image width is not divisible by " & p.ForcedOutputMod & ", click here to correct the crop values or customize Options > Image > Output Mod.") Then
                        CanIgnoreTip = False
                        AssistantMethod = AddressOf g.ForceCropMod
                        g.Highlight(True, tbTargetWidth)
                        g.Highlight(True, lSAR)
                        gbAssistant.Text = "Invalid Target Width"
                        Return False
                    End If
                End If

                If Not p.TargetHeight = 1080 AndAlso p.TargetHeight Mod p.ForcedOutputMod <> 0 Then
                    If ProcessTip("The image height is not divisible by " & p.ForcedOutputMod & ", click here to correct the crop values or customize Options > Image > Output Mod.") Then
                        CanIgnoreTip = False
                        AssistantMethod = AddressOf g.ForceCropMod
                        g.Highlight(True, tbTargetHeight)
                        g.Highlight(True, lSAR)
                        gbAssistant.Text = "Invalid Target Height"
                        Return False
                    End If
                End If
            End If

            If p.VideoEncoder.IsCompCheckEnabled AndAlso p.Compressibility > 0 Then
                Dim value = Calc.GetPercent

                If value < (p.VideoEncoder.AutoCompCheckValue - 20) OrElse
                    value > (p.VideoEncoder.AutoCompCheckValue + 20) Then

                    If ProcessTip("The aimed quality value is more than 20% off, change the image or file size to get something between 50% and 70% quality.") Then
                        g.Highlight(True, tbTargetSize)
                        g.Highlight(True, tbBitrate)
                        g.Highlight(True, tbTargetWidth)
                        g.Highlight(True, tbTargetHeight)
                        g.Highlight(True, lTarget2)
                        lTarget2.BackColor = Color.Red
                        gbAssistant.Text = "Quality"
                        Return False
                    End If
                End If
            End If

            If TypeOf p.VideoEncoder.Muxer Is MP4Muxer Then
                For Each i In p.VideoEncoder.Muxer.Subtitles
                    If Not {"idx", "srt"}.Contains(i.Path.Ext) Then
                        If ProcessTip("MP4 supports only SRT and IDX subtitles.") Then
                            CanIgnoreTip = False
                            gbAssistant.Text = "Invalid subtitle format"
                            Return False
                        End If
                    End If
                Next
            End If

            If Not (MouseButtons = MouseButtons.Left AndAlso ActiveControl Is tbResize) Then
                If Not p.Script.GetErrorMessage Is Nothing AndAlso Not g.VerifyRequirements Then
                    If ProcessTip(p.Script.GetErrorMessage) Then
                        CanIgnoreTip = False
                        gbAssistant.Text = "Script Error"
                        Return False
                    End If
                End If

                If Not p.Script.GetErrorMessage Is Nothing Then
                    If ProcessTip(p.Script.GetErrorMessage) Then
                        CanIgnoreTip = False
                        gbAssistant.Text = "Script Error"
                        Return False
                    End If
                End If
            End If
        Else
            If p.SourceFiles.Count = 0 Then
                If ProcessTip("Click here to open a source file.") Then
                    AssistantMethod = AddressOf ShowOpenSourceDialog
                    gbAssistant.Text = "Assistant"
                    CanIgnoreTip = False
                    Return False
                End If
            End If
        End If

        gbAssistant.Text = "Add Job"
        If lTip.Font.Size <> 9 Then lTip.Font = New Font(lTip.Font.FontFamily, 9 * s.UIScaleFactor)
        lTip.Text = "Click on the next button to add a job."
        AssistantPassed = True
    End Function

    Private Sub OpenTargetFolder()
        g.StartProcess(FilePath.GetDir(p.TargetFile))
    End Sub

    Dim BlockAudioTextChanged As Boolean

    Sub AudioTextChanged(tb As TextBox, ap As AudioProfile)
        If BlockAudioTextChanged Then Exit Sub

        If Not tb.Text.IsANSICompatible AndAlso p.Script.Engine = ScriptEngine.AviSynth Then
            MsgWarn(Strings.NoUnicode)
            tb.Text = ""
            Exit Sub
        End If

        If tb.Text.Contains(":\") OrElse tb.Text.StartsWith("\\") Then
            If tb.Text <> ap.File Then
                ap.File = tb.Text

                If Not p.Script.GetFilter("Source").Script.Contains("DirectShowSource") Then
                    ap.Delay = g.ExtractDelay(ap.File)
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

    Sub bnSkip_Click() Handles bnNext.Click
        If Not CanIgnoreTip Then
            MsgWarn("The current assistant instruction or warning cannot be skipped.")
            Exit Sub
        End If

        If Not p.SkippedAssistantTips.Contains(CurrentAssistantTipKey) Then
            p.SkippedAssistantTips.Add(CurrentAssistantTipKey)
        End If

        If Not g.VerifyRequirements() Then Exit Sub

        If AssistantPassed Then
            If AbortDueToLowDiskSpace() Then Exit Sub

            If Not TypeOf p.VideoEncoder Is NullEncoder AndAlso File.Exists(p.VideoEncoder.OutputPath) Then
                Select Case p.FileExistVideo
                    Case FileExistMode.Ask
                        Using td As New TaskDialog(Of String)
                            td.MainInstruction = "The output file of the video encoder already exists"
                            td.Content = "Would you like to skip video encoding and reuse the existing video encoder output file or would you like to re-encode and overwrite it?"
                            td.AddCommandLink("Reuse", "skip")
                            td.AddCommandLink("Re-encode", "encode")

                            Select Case td.Show
                                Case "skip"
                                    p.SkipVideoEncoding = True
                                Case "encode"
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

            If (p.Audio0.File <> "" AndAlso (TypeOf p.Audio0 Is GUIAudioProfile OrElse TypeOf p.Audio0 Is BatchAudioProfile) AndAlso File.Exists(p.Audio0.GetOutputFile)) OrElse
            (p.Audio1.File <> "" AndAlso (TypeOf p.Audio1 Is GUIAudioProfile OrElse TypeOf p.Audio1 Is BatchAudioProfile) AndAlso File.Exists(p.Audio1.GetOutputFile)) Then
                Select Case p.FileExistAudio
                    Case FileExistMode.Ask
                        Using td As New TaskDialog(Of String)
                            td.MainInstruction = "An audio encoding output file already exists"
                            td.Content = "Would you like to skip audio encoding and reuse existing audio encoding output files or would you like to re-encode and overwrite?"
                            td.AddCommandLink("Reuse", "skip")
                            td.AddCommandLink("Re-encode", "encode")

                            Select Case td.Show
                                Case "skip"
                                    p.SkipAudioEncoding = True
                                Case "encode"
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

            AddJob(False, Nothing)
            ShowJobsDialog()
        Else
            Assistant()
        End If
    End Sub

    Function AbortDueToLowDiskSpace() As Boolean
        Try 'crashes with network shares
            If p.TargetFile = "" OrElse p.TargetFile.StartsWith("\\") Then Exit Function
            Dim di As New DriveInfo(p.TargetFile.Dir)

            If di.AvailableFreeSpace / 1024 ^ 3 < s.MinimumDiskSpace Then
                Using td As New TaskDialog(Of String)
                    td.MainInstruction = "Low Disk Space"
                    td.Content = $"The target drive {Path.GetPathRoot(p.TargetFile)} has only {(di.AvailableFreeSpace / 1024 ^ 3).ToString("f2")} GB free disk space." + BR2 +
                        "This message can be configured at:" + BR2 + "Tools > Settings > System > Minimum Disk Space"
                    td.MainIcon = TaskDialogIcon.Warning
                    td.AddButton("Continue", "Continue")
                    td.AddButton("Abort", "Abort")
                    If td.Show <> "Continue" Then Return True
                End Using
            End If
        Catch
        End Try
    End Function

    <Command("Creates a job and runs the job list.")>
    Sub StartEncoding()
        AssistantPassed = True
        AddJob(False, Nothing)
        g.ProcessJobs()
    End Sub

    Private Sub Demux()
        Dim getFormat = Function() As String
                            Dim ret = MediaInfo.GetVideo(p.SourceFile, "Format")

                            Select Case ret
                                Case "MPEG Video"
                                    ret = "mpeg2"
                                Case "VC-1"
                                    ret = "vc1"
                            End Select

                            Return ret.ToLower
                        End Function

        Dim srcScript = p.Script.GetFilter("Source").Script.ToLower

        For Each i In s.Demuxers
            If Not i.Active AndAlso (i.SourceFilters.NothingOrEmpty OrElse
                Not srcScript.ContainsAny(i.SourceFilters.Select(Function(val) val.ToLower + "("))) Then Continue For

            If i.InputExtensions?.Length = 0 OrElse i.InputExtensions.Contains(p.SourceFile.Ext) Then
                If Not srcScript?.Contains("(") OrElse i.SourceFilters.NothingOrEmpty OrElse
                    srcScript.ContainsAny(i.SourceFilters.Select(Function(val) val.ToLower + "(")) Then

                    Dim inputFormats = i.InputFormats.NothingOrEmpty OrElse
                        i.InputFormats.Contains(getFormat())

                    If inputFormats Then
                        i.Run(p)
                        Refresh()

                        For Each iExt In i.OutputExtensions
                            Dim exitFor = False

                            For Each iFile In Directory.GetFiles(p.TempDir, "*." + iExt)
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

                            If exitFor Then Exit For
                        Next
                    End If
                End If
            End If
        Next

        Indexing()
    End Sub

    Sub Indexing()
        If p.SourceFile.Ext.EqualsAny("avs", "vpy") Then Exit Sub
        Dim codeLower = p.Script.GetFilter("Source").Script.ToLower

        If codeLower.Contains("ffvideosource(") OrElse codeLower.Contains("ffms2.source") Then
            If FileTypes.VideoIndex.Contains(p.SourceFile.Ext) Then
                p.SourceFile = p.LastOriginalSourceFile
                BlockSourceTextBoxTextChanged = True
                tbSourceFile.Text = p.SourceFile
                BlockSourceTextBoxTextChanged = False
            End If

            If codeLower.Contains("cachefile") Then
                g.ffmsindex(p.SourceFile, p.TempDir + p.SourceFile.Base + ".ffindex")
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

            If Not File.Exists(p.SourceFile + ".lwi") AndAlso File.Exists(p.Script.Path) AndAlso
                Not FileTypes.VideoText.Contains(FilePath.GetExt(p.SourceFile)) Then

                Using proc As New Proc
                    proc.Header = "Index LWLibav"
                    proc.Encoding = Encoding.UTF8

                    If p.Script.Engine = ScriptEngine.AviSynth Then
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
        ElseIf codeLower.Contains("dgsource(") AndAlso Not p.SourceFile.Ext = "dgi" Then
            If FileTypes.VideoIndex.Contains(p.SourceFile.Ext) Then p.SourceFile = p.LastOriginalSourceFile
            Dim dgIndexNV = Demuxer.GetDefaults.Find(Function(demuxer) demuxer.Name = "DGIndexNV: Index, No Demux")
            Dim outFile = p.TempDir + p.SourceFile.Base + ".dgi"
            If Not File.Exists(outFile) Then dgIndexNV.Run(p)

            If File.Exists(outFile) Then
                p.SourceFile = outFile
                BlockSourceTextBoxTextChanged = True
                tbSourceFile.Text = outFile
                BlockSourceTextBoxTextChanged = False
            End If
        ElseIf codeLower.Contains("dgsourceim(") AndAlso Not p.SourceFile.Ext = "dgim" Then
            If FileTypes.VideoIndex.Contains(p.SourceFile.Ext) Then p.SourceFile = p.LastOriginalSourceFile
            Dim dgIndexIM = Demuxer.GetDefaults.Find(Function(demuxer) demuxer.Name = "DGIndexIM: Index, No Demux")
            Dim outFile = p.TempDir + p.SourceFile.Base + ".dgim"
            If Not File.Exists(outFile) Then dgIndexIM.Run(p)

            If File.Exists(outFile) Then
                p.SourceFile = outFile
                BlockSourceTextBoxTextChanged = True
                tbSourceFile.Text = outFile
                BlockSourceTextBoxTextChanged = False
            End If
        End If
    End Sub

    <Command("Shows a file browser to open a project file.")>
    Sub ShowFileBrowserToOpenProject()
        Using d As New OpenFileDialog
            d.Filter = "Project Files|*.srip"

            If d.ShowDialog() = DialogResult.OK Then
                Refresh()
                OpenProject(d.FileName)
            End If
        End Using
    End Sub

    Private Function LoadTemplateWithSelectionDialog() As Boolean
        Dim td As New TaskDialog(Of String)
        td.MainInstruction = "Please select a template"

        For Each i In Directory.GetFiles(Folder.Template, "*.srip")
            td.AddCommandLink(FilePath.GetBase(i), i)
        Next

        If td.Show <> "" Then Return OpenProject(td.SelectedValue, True)
    End Function

    <Command(Strings.EventCommands)>
    Sub ShowEventCommandsDialog()
        Using f As New EventCommandsEditor(s.EventCommands)
            If f.ShowDialog() = DialogResult.OK Then
                s.EventCommands.Clear()

                For Each i As ListViewItem In f.lv.Items
                    s.EventCommands.Add(DirectCast(i.Tag, EventCommand))
                Next
            End If
        End Using

        g.SaveSettings()
    End Sub

    <Command("Shows the settings dialog.")>
    Sub ShowSettingsDialog()
        Using form As New SimpleSettingsForm("Settings")
            Dim ui = form.SimpleUI
            ui.Store = s

            ui.CreateFlowPage("General")

            Dim b = ui.AddBool()
            b.Text = "Show template selection when loading new files"
            b.Field = NameOf(s.ShowTemplateSelection)

            b = ui.AddBool()
            b.Text = "Reverse mouse wheel video seek direction"
            b.Field = NameOf(s.ReverseVideoScrollDirection)

            b = ui.AddBool()
            b.Text = "Enable debug logging"
            b.Field = NameOf(s.WriteDebugLog)

            Dim mb = ui.AddMenu(Of String)()
            mb.Text = "Startup Template"
            mb.Help = "Template loaded when StaxRip starts."
            mb.Field = NameOf(s.StartupTemplate)
            mb.Add(From i In Directory.GetFiles(Folder.Template) Select i.Base)

            Dim n = ui.AddNum()
            n.Text = "Number of log files to keep"
            n.Help = "Log files can be found at: Tools > Folders > Log Files"
            n.Config = {0, Integer.MaxValue}
            n.Field = NameOf(s.LogFileNum)

            n = ui.AddNum()
            n.Text = "Number of most recently used projects to keep"
            n.Help = "MRU list shown in the main menu under: Project > Recent"
            n.Config = {0, 15}
            n.Field = NameOf(s.ProjectsMruNum)

            n = ui.AddNum()
            n.Text = "Number of parallel processes"
            n.Config = {1, 4}
            n.Field = NameOf(s.ParallelProcsNum)

            n = ui.AddNum()
            n.Text = "Minimum preview size compared to screen size (percent)"
            n.Config = {10, 90, 10}
            n.Field = NameOf(s.MinPreviewSize)

            ui.CreateFlowPage("User Interface", True)

            Dim t = ui.AddText()
            t.Text = "Remember Window Positions:"
            t.Help = "Title or beginning of the title of windows of which the location should be remembered. For all windows enter '''all'''."
            t.Label.Offset = 12
            t.Edit.Expand = True
            t.Edit.Text = s.WindowPositionsRemembered.Join(", ")
            t.Edit.SaveAction = Sub(value) s.WindowPositionsRemembered = value.SplitNoEmptyAndWhiteSpace(",")

            t = ui.AddText()
            t.Text = "Center Screen Window Positions:"
            t.Help = "Title or beginning of the title of windows to be centered on the screen. For all windows enter '''all'''."
            t.Label.Offset = 12
            t.Edit.Expand = True
            t.Edit.Text = s.WindowPositionsCenterScreen.Join(", ")
            t.Edit.SaveAction = Sub(value) s.WindowPositionsCenterScreen = value.SplitNoEmptyAndWhiteSpace(",")

            n = ui.AddNum()
            n.Text = "UI Scale Factor"
            n.Help = "Requires to restart StaxRip."
            n.Config = {0.3, 3, 0.05, 2}
            n.Field = NameOf(s.UIScaleFactor)

            b = ui.AddBool()
            b.Text = "Enable tooltips in menus (restart required)"
            b.Help = "If you disable this you can still right-click menu items to show the tooltip."
            b.Field = NameOf(s.EnableTooltips)

            ui.AddControlPage(New PreprocessingControl, "Preprocessing").FormSizeScaleFactor = New Size(38, 21)
            ui.FormSizeScaleFactor = New Size(31, 21)

            Dim systemPage = ui.CreateFlowPage("System", True)

            Dim procPriority = ui.AddMenu(Of ProcessPriorityClass)
            procPriority.Text = "Process Priority"
            procPriority.Help = "Process priority of the applications StaxRip launches."
            procPriority.Field = NameOf(s.ProcessPriority)

            Dim renderMode = ui.AddMenu(Of ToolStripRenderModeEx)
            renderMode.Text = "Menu Style"
            renderMode.Help = "Defines the style used to render main menus, context menus and toolbars."
            renderMode.Field = NameOf(s.ToolStripRenderModeEx)

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
            b.Text = "Prevent system entering standby mode while encoding"
            b.Field = NameOf(s.PreventStandby)

            b = ui.AddBool
            b.Text = "Minimize processing dialog to tray"
            b.Field = NameOf(s.MinimizeToTray)

            Dim bsAVS = AddFilterPreferences(ui, "Source Filters | AviSynth",
                                             s.AviSynthFilterPreferences, s.AviSynthProfiles)

            Dim bsVS = AddFilterPreferences(ui, "Source Filters | VapourSynth",
                                            s.VapourSynthFilterPreferences, s.VapourSynthProfiles)

            ui.SelectLast("last settings page")

            If form.ShowDialog() = DialogResult.OK Then
                s.AviSynthFilterPreferences = DirectCast(bsAVS.DataSource, StringPairList)
                s.AviSynthFilterPreferences.Sort()
                s.VapourSynthFilterPreferences = DirectCast(bsVS.DataSource, StringPairList)
                s.VapourSynthFilterPreferences.Sort()
                ui.Save()
                g.SetRenderer(MenuStrip)
                SetMenuStyle()
                s.UpdateRecentProjects(Nothing)
                UpdateRecentProjectsMenu()
                g.SaveSettings()
            End If

            ui.SaveLast("last settings page")
        End Using
    End Sub

    Function AddFilterPreferences(ui As SimpleUI,
                                  pagePath As String,
                                  preferences As StringPairList,
                                  profiles As List(Of FilterCategory)) As BindingSource

        Dim filterPage = ui.CreateDataPage(pagePath)

        Dim tipsFunc = Function() As StringPairList
                           Dim ret As New StringPairList From {
                               {" Filters Menu", "StaxRip allows to assign a source filter profile to a particular source file type. The source filter profiles can be customized by right-clicking the filters menu in the main dialog."}
                           }

                           For Each i In profiles.Where(
                               Function(v) v.Name = "Source").First.Filters

                               If i.Script <> "" Then ret.Add(i.Name, i.Script)
                           Next

                           Return ret
                       End Function

        filterPage.TipProvider.TipsFunc = tipsFunc

        Dim c1 = filterPage.AddTextBoxColumn()
        c1.DataPropertyName = "Name"
        c1.HeaderText = "File Type"

        Dim c2 = filterPage.AddComboBoxColumn
        c2.DataPropertyName = "Value"
        c2.HeaderText = "Prefered Source Filter"

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
            OpenSaveDialog()
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
            Text = Application.ProductName + " x64 - " + path.Base
            s.UpdateRecentProjects(path)
            UpdateRecentProjectsMenu()
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    <Command("Saves the current project.")>
    Sub SaveProjectAs()
        OpenSaveDialog()
    End Sub

    <Command("Saves the current project as template.")>
    Sub SaveProjectAsTemplate()
        If p.SourceFile = "" Then
            Dim b As New InputBox
            b.Text = "Enter the name of the template."
            b.Title = "Save Template"
            b.Value = p.TemplateName
            b.VerificationText = "Load template on startup"

            If b.Show = DialogResult.OK Then
                p.TemplateName = b.Value.RemoveChars(Path.GetInvalidFileNameChars)
                SaveProjectPath(Folder.Template + p.TemplateName + ".srip")
                UpdateTemplatesMenuAsync()

                If b.Checked Then
                    s.StartupTemplate = b.Value
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

    <Command("Dialog to manage external applications.")>
    Sub ShowAppsDialog()
        Using f As New AppsForm
            Dim found As Boolean

            If s.StringDictionary.ContainsKey("RecentExternalApplicationControl") Then
                For Each i In Package.Items.Values
                    If i.Name + i.Version = s.StringDictionary("RecentExternalApplicationControl") Then
                        f.ShowPackage(i)
                        found = True
                        Exit For
                    End If
                Next
            End If

            If Not found Then f.ShowPackage(Package.x265)
            f.ShowDialog()
            g.SaveSettings()
        End Using
    End Sub

    <Command("Dialog to manage encoder profiles.")>
    Sub ShowEncoderProfilesDialog()
        Using f As New ProfilesForm("Encoder Profiles", s.VideoEncoderProfiles,
                                    AddressOf g.LoadVideoEncoder,
                                    AddressOf GetNewVideoEncoderProfile,
                                    AddressOf VideoEncoder.GetDefaults)

            If f.ShowDialog() = DialogResult.OK Then
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

        If sb.Show = DialogResult.OK Then Return sb.SelectedValue
    End Function

    <Command("Placeholder for dynamically updated menu items.")>
    Sub DynamicMenuItem(<DispName("ID")> id As DynamicMenuItemID)
    End Sub

    Sub PopulateProfileMenu(id As DynamicMenuItemID)
        For Each i In CustomMainMenu.MenuItems.OfType(Of MenuItemEx)()
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

    <Command("Dialog to crop borders.")>
    Sub ShowCropDialog()
        If p.SourceFile = "" Then
            ShowOpenSourceDialog()
        Else
            If Not g.VerifyRequirements Then Exit Sub
            If Not g.IsValidSource Then Exit Sub

            If Not g.EnableFilter("Crop") Then
                If p.Script.Engine = ScriptEngine.AviSynth Then
                    p.Script.InsertAfter("Source", New VideoFilter("Crop", "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)"))
                Else
                    p.Script.InsertAfter("Source", New VideoFilter("Crop", "Crop", "clip = core.std.Crop(clip, %crop_left%, %crop_right%, %crop_top%, %crop_bottom%)"))
                End If
            End If

            Using f As New CropForm
                f.ShowDialog()
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

            Dim errMsg = p.Script.GetErrorMessage

            If Not errMsg Is Nothing Then
                MsgError(errMsg)
                Exit Sub
            End If

            Dim cutting = p.Script.GetFilter("Cutting")

            If Not cutting Is Nothing Then
                p.Script.Filters.Remove(cutting)
                g.MainForm.FiltersListView.Load()
                g.MainForm.UpdateTargetParameters(p.Script.GetSeconds, p.Script.GetFramerate)
            End If

            Dim doc As New VideoScript
            doc.Engine = p.Script.Engine
            doc.Filters = p.Script.GetFiltersCopy
            doc.Path = p.TempDir + p.TargetFile.Base + "_preview." + doc.FileType
            doc.Synchronize(True)

            Dim f As New PreviewForm(doc)
            f.Owner = g.MainForm
            f.Show()
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
        Using form As New JobsForm()
            form.ShowDialog()
        End Using
    End Sub

    <Command("Clears the job list.")>
    Sub ClearJobs()
        FileHelp.Delete(Folder.Settings + "Jobs.dat")
    End Sub

    Function GetJobPath() As String
        Dim name = p.TargetFile.Base
        If name = "" Then name = Macro.Expand(p.DefaultTargetName)
        If name = "" Then name = p.SourceFile.Base
        Return p.TempDir + name + ".srip"
    End Function

    <Command("Loads a audio or video profile.")>
    Sub LoadProfile(<DispName("Video")> videoProfile As String,
                    <DispName("Audio 1")> audioProfile1 As String,
                    <DispName("Audio 2")> audioProfile2 As String)

        If videoProfile <> "" Then
            For Each i In s.VideoEncoderProfiles
                If i.Name = videoProfile Then g.LoadVideoEncoder(i)
            Next
        End If

        If audioProfile1 <> "" Then
            For Each i In s.AudioProfiles
                If i.Name = audioProfile1 Then g.LoadAudioProfile0(i)
            Next
        End If

        If audioProfile2 <> "" Then
            For Each i In s.AudioProfiles
                If i.Name = audioProfile2 Then g.LoadAudioProfile1(i)
            Next
        End If
    End Sub

    <Command("Adds a job to the job list.")>
    Sub AddJob(
        <DispName("Show Confirmation")>
        showConfirmation As Boolean,
        <DispName("Template Name"),
        Description("Name of the template to be loaded after the job was added. Empty to load no template.")>
        templateName As String)

        AddJob(showConfirmation, templateName, True)
    End Sub

    Sub AddJob(showConfirmation As Boolean, templateName As String, showAssistant As Boolean)
        If Not g.VerifyRequirements() Then Exit Sub

        If showAssistant AndAlso Not IsLoading AndAlso Not g.MainForm.AssistantPassed Then
            MsgWarn("Please follow the assistant.")
            Exit Sub
        End If

        Dim jobPath = GetJobPath()
        g.MainForm.SaveProjectPath(jobPath)
        Job.AddJob(jobPath)

        If showConfirmation Then MsgInfo("Job added")
        If templateName <> "" Then LoadProject(Folder.Template + templateName + ".srip")
    End Sub

    <Command("Compare and extract images for video comparisons.")>
    Sub ShowVideoComparison()
        Dim f As New VideoComparisonForm
        f.Show()
        f.Add()
    End Sub

    <Command("Dialog to edit filters.")>
    Sub ShowFiltersEditor()
        FiltersListView.ShowEditor()
    End Sub

    <Command("Dialog to configure project options.")>
    Sub ShowOptionsDialog()
        ShowOptionsDialog(Nothing)
    End Sub

    Sub ShowOptionsDialog(pagePath As String)
        Using form As New SimpleSettingsForm(
            "Project Options",
            "In order to save project options go to:",
            "Project > Save As Template",
            "In order to select a template to be loaded on program startup go to:",
            "Tools > Settings > General > Templates > Default Template")

            form.ScaleClientSize(30, 21)

            Dim ui = form.SimpleUI
            ui.Store = p

            ui.CreateFlowPage("Image", True)

            Dim b = ui.AddBool()
            b.Text = "Save Staxrip Thumbnails"
            b.Help = "Saves thumbnails in the target folder. Customizations can be made in the settings under:" + BR2 + "General > Advanced > Thumbnails"
            b.Field = NameOf(p.SaveThumbnails)

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

            ui.CreateFlowPage("Image | Aspect Ratio", True)

            b = ui.AddBool()
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
            te.SaveAction = Sub(value) If value.IsInt Then p.CropLeft = CInt(value)

            Dim l = ui.AddLabel(eb, "Right:", 4)

            te = ui.AddEdit(eb)
            te.Text = p.CropRight.ToString
            te.WidthFactor = 3
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value) If value.IsInt Then p.CropRight = CInt(value)

            eb = ui.AddEmptyBlock(cropPage)

            ui.AddLabel(eb, "Top:", 2)

            te = ui.AddEdit(eb)
            te.Text = p.CropTop.ToString
            te.WidthFactor = 3
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value) If value.IsInt Then p.CropTop = CInt(value)

            l = ui.AddLabel(eb, "Bottom:", 4)

            te = ui.AddEdit(eb)
            te.Text = p.CropBottom.ToString
            te.WidthFactor = 3
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value) If value.IsInt Then p.CropBottom = CInt(value)

            Dim audioPage = ui.CreateFlowPage("Audio", True)

            t = ui.AddText
            t.Text = "Preferred Languages"
            t.Help = "Preferred audio languages using [http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes two or three letter language code] separated by space, comma or semicolon. For all languages just enter all." + BR2 + String.Join(BR, From i In Language.Languages Where i.IsCommon Select i.ToString + ": " + i.TwoLetterCode + ", " + i.ThreeLetterCode)
            t.Field = NameOf(p.PreferredAudio)

            Dim cut = ui.AddMenu(Of CuttingMode)
            cut.Text = "Cutting Method"
            cut.Help = "Defines which method to use for cutting."
            cut.Field = NameOf(p.CuttingMode)

            Dim audioDemux = ui.AddMenu(Of DemuxMode)
            audioDemux.Text = "Demux Audio"
            audioDemux.Field = NameOf(p.DemuxAudio)

            Dim audioExist = ui.AddMenu(Of FileExistMode)
            audioExist.Text = "Existing Output"
            audioExist.Help = "What to do in case a audio encoding output file already exists from a previous job run, skip and reuse or re-encode and overwrite."
            audioExist.Field = NameOf(p.FileExistAudio)

            b = ui.AddBool
            b.Text = "On load use AviSynth script as audio source"
            b.Help = "Sets the AviSynth script (*.avs) as audio source file when loading a source file."
            b.Field = NameOf(p.UseScriptAsAudioSource)

            Dim videoPage = ui.CreateFlowPage("Video", True)

            Dim videoExist = ui.AddMenu(Of FileExistMode)
            videoExist.Text = "Existing Video Output"
            videoExist.Help = "What to do in case the video encoding output file already exists from a previous job run, skip and reuse or re-encode and overwrite. The 'Copy/Mux' video encoder profile is also capable of reusing existing video encoder output.'"
            videoExist.Field = NameOf(p.FileExistVideo)

            b = ui.AddBool
            b.Text = "Pre-render script into lossless AVI file"
            b.Help = "Note that depending on the resolution this can result in very large files."
            b.Field = NameOf(p.PreRenderIntoLossless)

            b = ui.AddBool
            b.Text = "Import VUI metadata"
            b.Help = "Imports VUI metadata such as HDR from the source file to the video encoder."
            b.Field = NameOf(p.ImportVUIMetadata)

            Dim subPage = ui.CreateFlowPage("Subtitles", True)

            t = ui.AddText(subPage)
            t.Text = "Preferred Languages"
            t.Help = "Subtitles demuxed and loaded automatically using [http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes two or three letter language code] separated by space, comma or semicolon. For all subtitles just enter all." + BR2 + String.Join(BR, From i In Language.Languages Where i.IsCommon Select i.ToString + ": " + i.TwoLetterCode + ", " + i.ThreeLetterCode)
            t.Field = NameOf(p.PreferredSubtitles)

            Dim tbm = ui.AddTextMenu(subPage)
            tbm.Text = "Stream Name"
            tbm.Help = "Stream name used for muxing, may contain macros."
            tbm.Field = NameOf(p.SubtitleName)
            tbm.AddMenu("Language English", "%language_english%")
            tbm.AddMenu("Language Native", "%language_native%")

            Dim subDemux = ui.AddMenu(Of DemuxMode)
            subDemux.Text = "Demux Subtitles"
            subDemux.Field = NameOf(p.DemuxSubtitles)

            Dim mb = ui.AddMenu(Of DefaultSubtitleMode)(subPage)
            mb.Text = "Default Subtitle"
            mb.Field = NameOf(p.DefaultSubtitle)

            b = ui.AddBool(subPage)
            b.Text = "Convert Sup (PGS/Blu-ray) to IDX (Sub/VobSub/DVD)"
            b.Help = "Works only with demuxed subtitles."
            b.Field = NameOf(p.ConvertSup2Sub)

            b = ui.AddBool(subPage)
            b.Text = "Add hardcoded subtitle"
            b.Help = "Automatically hardcodes a subtitle." + BR2 + "Supported formats are SRT, ASS and VobSub."
            b.Field = NameOf(p.HarcodedSubtitle)

            Dim pathPage = ui.CreateFlowPage("Paths")

            l = ui.AddLabel(pathPage, "Default Target Folder:")
            l.Help = "Leave empty to use the source file folder."

            Dim tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.DefaultTargetFolder
            tm.Edit.SaveAction = Sub(value) p.DefaultTargetFolder = value
            tm.AddMenu("Edit...", Function() g.BrowseFolder(p.DefaultTargetFolder))
            tm.AddMenu("Directory of source file", "%source_dir%")
            tm.AddMenu("Parent directory of source file directory", "%source_dir_parent%")

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

            l = ui.AddLabel(pathPage, "Temp Files Folder:")
            l.Help = "Leave empty to use the source file folder."
            l.MarginTop = Font.Height

            Dim tempDirFunc = Function()
                                  Dim tempDir = g.BrowseFolder(p.TempDir)
                                  If tempDir <> "" Then Return tempDir.FixDir + "%source_name%_temp"
                              End Function

            tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.TempDir
            tm.Edit.SaveAction = Sub(value) p.TempDir = value
            tm.AddMenu("Edit...", tempDirFunc)
            tm.AddMenu("Source File Directory", "%source_dir%%source_name%_temp")

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
            b.Text = "Remind about aspect ratio error"
            b.Field = NameOf(p.RemindArError)

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

            ui.AddLine(miscPage, "Compressibility Check")

            b = ui.AddBool(miscPage)
            b.Text = "Auto run compressibility check"
            b.Help = "Performs a compressibility check after loading a source file."
            b.Checked = p.AutoCompCheck
            b.SaveAction = Sub(value) p.AutoCompCheck = value

            n = ui.AddNum(miscPage)
            n.Label.Text = "Percentage for comp. check"
            n.NumEdit.Config = {2, 20}
            n.NumEdit.Value = p.CompCheckRange
            n.NumEdit.SaveAction = Sub(value) p.CompCheckRange = CInt(value)

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

                If p.CompCheckRange < 2 OrElse p.CompCheckRange > 20 Then p.CompCheckRange = 5
                If p.TempDir <> "" Then p.TempDir = p.TempDir.FixDir

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
        Using f As New ProfilesForm("AviSynth Profiles", s.FilterSetupProfiles,
                                    AddressOf LoadFilterSetup,
                                    AddressOf GetScriptAsProfile,
                                    AddressOf VideoScript.GetDefaults)

            If f.ShowDialog() = DialogResult.OK Then
                PopulateProfileMenu(DynamicMenuItemID.FilterSetupProfiles)
            End If
        End Using
    End Sub

    Function GetFilterProfilesText(categories As List(Of FilterCategory)) As String
        Dim ret = ""
        Dim wasMultiline As Boolean

        For Each i In categories
            ret += "[" + i.Name + "]" + BR

            For Each filter In i.Filters
                If filter.Script.Contains(BR) Then
                    Dim lines = filter.Script.SplitLinesNoEmpty

                    For x = 0 To lines.Length - 1
                        lines(x) = "    " + lines(x)
                    Next

                    ret += BR + filter.Path + " =" + BR + lines.Join(BR) + BR
                    wasMultiline = True
                Else
                    If wasMultiline Then ret += BR
                    ret += filter.Path + " = " + filter.Script + BR
                    wasMultiline = False
                End If
            Next

            If Not ret.EndsWith(BR2) Then ret += BR
        Next

        Return ret
    End Function

    <Command("Dialog to configure AviSynth filter profiles.")>
    Sub ShowFilterProfilesDialog()
        Dim filterProfiles = If(p.Script.Engine = ScriptEngine.AviSynth, s.AviSynthProfiles, s.VapourSynthProfiles)
        Dim getDefaults = If(p.Script.Engine = ScriptEngine.AviSynth, Function() FilterCategory.GetAviSynthDefaults, Function() FilterCategory.GetVapourSynthDefaults)

        Using f As New MacroEditorDialog
            f.SetScriptDefaults()
            f.Text = "Filter Profiles"
            f.MacroEditorControl.Value = GetFilterProfilesText(filterProfiles)
            f.bnContext.Text = " Restore Defaults... "
            f.bnContext.Visible = True
            f.MacroEditorControl.rtbDefaults.Text = GetFilterProfilesText(getDefaults())
            f.bnContext.AddClickAction(Sub() If MsgOK("Restore defaults?") Then f.MacroEditorControl.Value = GetFilterProfilesText(getDefaults()))

            If f.ShowDialog(Me) = DialogResult.OK Then
                filterProfiles.Clear()
                Dim cat As FilterCategory
                Dim filter As VideoFilter

                For Each line In f.MacroEditorControl.Value.SplitLinesNoEmpty
                    Dim multiline = line.StartsWith("    ") OrElse line.StartsWith(VB6.vbTab)

                    If line.StartsWith("[") AndAlso line.EndsWith("]") Then
                        cat = New FilterCategory(line.Substring(1, line.Length - 2).Trim)
                        filterProfiles.Add(cat)
                    End If

                    If multiline Then
                        If Not filter Is Nothing Then
                            If filter.Script = "" Then
                                If line.StartsWith(VB6.vbTab) Then filter.Script += line.Substring(1)
                                If line.StartsWith("    ") Then filter.Script += line.Substring(4)
                            Else
                                If line.StartsWith(VB6.vbTab) Then filter.Script += BR + line.Substring(1)
                                If line.StartsWith("    ") Then filter.Script += BR + line.Substring(4)
                            End If
                        End If
                    Else
                        Dim filterName = line.Left("=").Trim

                        If filterName <> "" Then
                            filter = New VideoFilter(cat.Name, filterName, line.Right("=").Trim)
                            cat.Filters.Add(filter)
                        End If
                    End If
                Next

                For Each i In getDefaults()
                    Dim found As Boolean

                    For Each i2 In filterProfiles
                        If i.Name = i2.Name Then found = True
                    Next

                    If Not found AndAlso {"Source", "Crop", "Resize"}.Contains(i.Name) Then
                        MsgWarn("The category '" + i.Name + "' was recreated. A Source, Crop and Resize category is mandatory.")
                        filterProfiles.Add(i)
                    End If
                Next

                g.SaveSettings()
                FiltersListView.RebuildMenu()
            End If
        End Using
    End Sub

    <Command("Opens a given URL or local file in the help browser.")>
    Sub ShowHelpURL(
        <DispName("URL"),
        Description("URL or local file to be shown in the internet explorer powered help browser."),
        Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        url As String)

        Dim f As New HelpForm(Macro.Expand(url))
        f.Show()
    End Sub

    Shared Function GetDefaultMenuMain() As CustomMenuItem
        Dim ret As New CustomMenuItem("Root")

        ret.Add("Project|Open...", NameOf(ShowFileBrowserToOpenProject), Keys.O Or Keys.Control, Symbol.OpenFile)
        ret.Add("Project|Save", NameOf(SaveProject), Keys.S Or Keys.Control, Symbol.Save)
        ret.Add("Project|Save As...", NameOf(SaveProjectAs))
        ret.Add("Project|Save As Template...", NameOf(SaveProjectAsTemplate))
        ret.Add("Project|-")
        ret.Add("Project|Templates", NameOf(DynamicMenuItem), {DynamicMenuItemID.TemplateProjects})
        ret.Add("Project|Recent", NameOf(DynamicMenuItem), {DynamicMenuItemID.RecentProjects})

        ret.Add("Crop", NameOf(ShowCropDialog), Keys.F4)
        ret.Add("Preview", NameOf(ShowPreview), Keys.F5)

        ret.Add("Options", NameOf(ShowOptionsDialog), Keys.F8)

        ret.Add("Tools|Jobs...", NameOf(ShowJobsDialog), Keys.F6, Symbol.MultiSelectLegacy)
        ret.Add("Tools|Log File", NameOf(g.DefaultCommands.ShowLogFile), Symbol.Page)
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
        ret.Add("Tools|Folders|Templates", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%settings_dir%TemplatesV2"""})
        ret.Add("Tools|Folders|Working", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%working_dir%"""})

        ret.Add("Tools|Advanced", Symbol.More)
        If Application.StartupPath = "C:\Users\Revan\Desktop\staxrip-1.8.2.0\bin" Then ret.Add("Tools|Advanced|Test...", NameOf(g.DefaultCommands.Test), Keys.F12)
        ret.Add("Tools|Advanced|Video Comparison...", NameOf(ShowVideoComparison))
        ret.Add("Tools|Advanced|Command Prompts", Symbol.fa_terminal)
        'ret.Add("Tools|Advanced|Command Prompts|Bash Prompt", NameOf(g.DefaultCommands.BashCommandPrompt), Symbol.fa_terminal)
        ret.Add("Tools|Advanced|Command Prompts|Command Prompt", NameOf(g.DefaultCommands.ShowCommandPrompt), Symbol.fa_terminal)
        ret.Add("Tools|Advanced|Command Prompts|PowerShell", NameOf(g.DefaultCommands.ShowPowerShell), Keys.Control Or Keys.P, Symbol.fa_terminal)
        ret.Add("Tools|Advanced|Event Commands...", NameOf(ShowEventCommandsDialog), Symbol.LightningBolt)
        ret.Add("Tools|Advanced|Hardcoded Subtitle...", NameOf(ShowHardcodedSubtitleDialog), Keys.Control Or Keys.H, Symbol.Subtitles)
        ret.Add("Tools|Advanced|Demux...", NameOf(g.DefaultCommands.ShowDemuxTool))
        ret.Add("Tools|Advanced|LAV Filters video decoder configuration...", NameOf(ShowLAVFiltersConfigDialog), Symbol.Filter)
        ret.Add("Tools|Advanced|MediaInfo Folder View...", NameOf(ShowMediaInfoFolderViewDialog), Symbol.Info)
        ret.Add("Tools|Advanced|Reset Setting...", NameOf(ResetSettings))
        ret.Add("Tools|Advanced|Thumbnails", Symbol.fa_th)
        ret.Add("Tools|Advanced|Thumbnails|StaxRip Thumbnail Generator", NameOf(ShowBatchGenerateThumbnailsDialog), Symbol.fa_th_large)
        ret.Add("Tools|Advanced|Thumbnails|MTN Thumbnail Generator", NameOf(g.DefaultCommands.StartTool), Symbol.fa_th, {"MTNWindows"})
        ret.Add("Tools|Advanced|Thumbnails|MTN Thumbnail Generator", NameOf(g.DefaultCommands.StartTool), Symbol.fa_th, {"VCSPython"})

        ret.Add("Tools|Scripts", NameOf(DynamicMenuItem), Symbol.Code, {DynamicMenuItemID.Scripts})
        ret.Add("Tools|Edit Menu...", NameOf(ShowMainMenuEditor))
        ret.Add("Tools|Settings...", NameOf(ShowSettingsDialog), Symbol.Settings, {""})

        ret.Add("Apps|AVSTools|AVSMeter", NameOf(g.DefaultCommands.StartTool), {"AVSMeter"})
        ret.Add("Apps|MKVTools|MKVHDR Info", NameOf(g.DefaultCommands.StartTool), {"mkvinfo"})
        ret.Add("Apps|MKVTools|MKVHDR Merge", NameOf(g.DefaultCommands.StartTool), {"mkvmerge"})
        ret.Add("Apps|DGIndex|DGIndex", NameOf(g.DefaultCommands.StartTool), {"DGIndex"})
        ret.Add("Apps|DGIndex|DGIndexNV", NameOf(g.DefaultCommands.StartTool), {"DGIndexNV"})
        ret.Add("Apps|Players|MPV", NameOf(g.DefaultCommands.StartTool), {"mpv"})
        ret.Add("Apps|SubTitles|BDSup2Sub++", NameOf(g.DefaultCommands.StartTool), {"BDSup2Sub++"})
        ret.Add("Apps|SubTitles|SubtitleEdit", NameOf(g.DefaultCommands.StartTool), {"SubtitleEdit"})
        ret.Add("Apps|SubTitles|VSRip", NameOf(g.DefaultCommands.StartTool), {"VSRip"})

        ret.Add("Apps|-")
        ret.Add("Apps|Manage...", NameOf(ShowAppsDialog))

        ret.Add("Help|Docs", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.Lightbulb, {"http://staxrip.readthedocs.io"})
        ret.Add("Help|Apps", NameOf(DynamicMenuItem), {DynamicMenuItemID.HelpApplications})
        ret.Add("Help|-")
        ret.Add("Help|Info...", NameOf(g.DefaultCommands.OpenHelpTopic), Symbol.Info, {"info"})
        Return ret
    End Function

    <Command("Shows a dialog to add a hardcoded subtitle.")>
    Sub ShowHardcodedSubtitleDialog()
        Using d As New OpenFileDialog
            d.SetFilter(FileTypes.SubtitleExludingContainers)
            d.SetInitDir(s.LastSourceDir)

            If d.ShowDialog = DialogResult.OK Then
                If d.FileName.Ext = "idx" Then
                    Dim subs = Subtitle.Create(d.FileName)

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

                    If sb.Show = DialogResult.Cancel Then Exit Sub

                    Regex.Replace(File.ReadAllText(d.FileName), "langidx: \d+", "langidx: " +
                                  sb.SelectedValue.IndexIDX.ToString).WriteANSIFile(d.FileName)
                End If

                p.AddHardcodedSubtitleFilter(d.FileName, True)
            End If
        End Using
    End Sub

    Private Sub tbResize_MouseUp(sender As Object, e As MouseEventArgs) Handles tbResize.MouseUp
        Assistant()
    End Sub

    Private Sub tbResize_Scroll() Handles tbResize.Scroll
        SkipAssistant = True

        If Not g.EnableFilter("Resize") Then
            If p.Script.Engine = ScriptEngine.AviSynth Then
                p.Script.AddFilter(New VideoFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)"))
            Else
                p.Script.AddFilter(New VideoFilter("Resize", "Bicubic", "clip = core.resize.Bicubic(clip, %target_width%, %target_height%)"))
            End If
        End If

        tbTargetWidth.Text = CInt(320 + tbResize.Value * p.ForcedOutputMod).ToString
        SetImageHeight()
        SkipAssistant = False
        Assistant()
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

    Private Sub tbBitrate_KeyDown(sender As Object, e As KeyEventArgs) Handles tbBitrate.KeyDown
        If e.KeyData = Keys.Up Then
            e.Handled = True
            tbBitrate.Text = Math.Max(1, Calc.GetPreviousMod(tbBitrate.Text.ToInt, 50)).ToString
        ElseIf e.KeyData = Keys.Down Then
            e.Handled = True
            tbBitrate.Text = Math.Max(1, Calc.GetNextMod(tbBitrate.Text.ToInt, 50)).ToString
        End If
    End Sub

    Private Sub tbSize_KeyDown(sender As Object, e As KeyEventArgs) Handles tbTargetSize.KeyDown
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
            tbTargetSize.Text = Math.Max(1, Calc.GetPreviousMod(tbTargetSize.Text.ToInt, modValue)).ToString
        ElseIf e.KeyData = Keys.Down Then
            e.Handled = True
            tbTargetSize.Text = Math.Max(1, Calc.GetNextMod(tbTargetSize.Text.ToInt, modValue)).ToString
        End If
    End Sub

    Sub UpdateSizeOrBitrate()
        If p.BitrateIsFixed Then tbBitrate_TextChanged() Else tbSize_TextChanged()
    End Sub

    Sub tbSize_TextChanged() Handles tbTargetSize.TextChanged
        Try
            If tbTargetSize.Focused Then p.BitrateIsFixed = False

            If Integer.TryParse(tbTargetSize.Text, Nothing) Then
                p.TargetSize = Math.Max(1, CInt(tbTargetSize.Text))
                BlockSize = True
                If Not BlockBitrate Then tbBitrate.Text = CInt(Calc.GetVideoBitrate).ToString
                BlockSize = False
                Assistant()
            End If
        Catch
        End Try
    End Sub

    Sub tbBitrate_TextChanged() Handles tbBitrate.TextChanged
        Try
            If tbBitrate.Focused Then p.BitrateIsFixed = True

            If Integer.TryParse(tbBitrate.Text, Nothing) Then
                p.VideoBitrate = Math.Max(1, CInt(tbBitrate.Text))
                BlockBitrate = True
                If Not BlockSize Then tbTargetSize.Text = CInt(Calc.GetSize).ToString
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

    Private Sub tbTargetWidth_KeyDown(sender As Object, e As KeyEventArgs) Handles tbTargetWidth.KeyDown
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

    Private Sub tbTargetHeight_KeyDown(sender As Object, e As KeyEventArgs) Handles tbTargetHeight.KeyDown
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

    Shared Function GetDefaultMenuSize() As CustomMenuItem
        Dim ret = New CustomMenuItem("Root")
        ret.Add("DVD/BD-5 (4480 MB)", NameOf(SetSize), {4480})
        ret.Add("DVD-DL/BD-9 (8145 MB)", NameOf(SetSize), {8145})
        ret.Add("-")
        ret.Add("BD (23450 MB)", NameOf(SetSize), {23450})
        ret.Add("BD-DL (46900 MB)", NameOf(SetSize), {46900})
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
    End Sub

    <Command("Sets the target image size by pixels (width x height).")>
    Sub SetTargetImageSizeByPixel(pixel As Integer)
        If Not g.EnableFilter("Resize") Then
            p.Script.AddFilter(New VideoFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)"))
        End If

        Dim cropw = p.SourceWidth - p.CropLeft - p.CropRight
        Dim ar = Calc.GetTargetDAR()
        Dim w = Calc.FixMod16(CInt(Math.Sqrt(pixel * ar)))

        If w > cropw Then w = (cropw \ 16) * 16

        Dim h = Calc.FixMod16(CInt(w / ar))

        tbTargetWidth.Text = w.ToString()
        tbTargetHeight.Text = h.ToString()
    End Sub

    <Command("Sets the target file size in MB.")>
    Sub SetSize(<DispName("Target File Size")> targetSize As Integer)
        tbTargetSize.Text = targetSize.ToString
        p.BitrateIsFixed = False
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
        g.RunAutoCrop()
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

    Private Sub UpdateSizeMenu()
        For Each i As MenuItemEx In CustomSizeMenu.MenuItems
            If i.CustomMenuItem.MethodName = "SetPercent" Then
                i.Enabled = p.Compressibility > 0
            End If
        Next
    End Sub

    Private Function GetScriptAsProfile() As Profile
        Dim sb As New SelectionBox(Of TargetVideoScript)

        sb.Title = "New Profile"
        sb.Text = "Please select a profile."

        sb.AddItem("Current Project", p.Script)

        For Each i In VideoScript.GetDefaults
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK Then Return sb.SelectedValue
    End Function

    Sub LoadFilterSetup(profileInterface As Profile)
        Dim profile = DirectCast(ObjectHelp.GetCopy(profileInterface), TargetVideoScript)

        If profile.Engine = ScriptEngine.AviSynth OrElse
                (Package.Python.VerifyOK(True) AndAlso
                Package.VapourSynth.VerifyOK(True) AndAlso
                Package.vspipe.VerifyOK(True)) Then

            p.Script = profile
            ModifyFilters()
            FiltersListView.OnChanged()
            Assistant()
        End If

        FiltersListView.RebuildMenu()
    End Sub

    <Command("Shows LAV Filters video decoder configuration")>
    Private Sub ShowLAVFiltersConfigDialog()
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
    Public Shared Sub OpenConfiguration(hwnd As IntPtr, hinst As IntPtr, lpszCmdLine As String, nCmdShow As Integer)
    End Sub

    Private Sub ProcessCommandLine(a As String())
        Dim files As New List(Of String)

        For Each i In CLIArg.GetArgs(a)
            Try
                If Not i.IsFile AndAlso files.Count > 0 Then
                    Dim l As New List(Of String)(files)
                    Refresh()
                    OpenAnyFile(l)
                    files.Clear()
                End If

                If i.IsFile Then
                    files.Add(i.Value)
                Else
                    If Not CommandManager.ProcessCommandLineArgument(i.Value) Then Throw New Exception
                End If
            Catch ex As Exception
                MsgWarn("Error parsing argument:" + BR2 + i.Value + BR2 + ex.Message)
            End Try
        Next

        If files.Count > 0 Then
            Refresh()
            OpenAnyFile(files)
        End If
    End Sub

    <Command("Sets the project option 'Hide dialogs asking to demux, source filter etc.'")>
    Private Sub SetHideDialogsOption(hide As Boolean)
        p.NoDialogs = hide
    End Sub

    <Command("Puts PC in standby mode.")>
    Private Sub Standby()
        g.ShutdownPC(ShutdownMode.Standby)
    End Sub

    <Command("Shuts PC down.")>
    Private Sub Shutdown()
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
        lBitrate.Visible = Not p.VideoEncoder.QualityMode
        tbBitrate.Visible = Not p.VideoEncoder.QualityMode
        lTarget2.Visible = p.VideoEncoder.IsCompCheckEnabled
    End Sub

    <Command("Dialog to open source files.")>
    Private Sub ShowOpenSourceDialog()
        Dim td As New TaskDialog(Of String)
        td.MainInstruction = "Select a method for opening a source."
        td.AddCommandLink("Single File", "Single File")
        td.AddCommandLink("Blu-ray Folder", "Blu-ray Folder")
        td.AddCommandLink("Merge Files", "Merge Files")
        td.AddCommandLink("File Batch", "File Batch")

        Select Case td.Show
            Case "Single File"
                Using d As New OpenFileDialog
                    d.SetFilter(FileTypes.Video)
                    d.SetInitDir(s.LastSourceDir)
                    If d.ShowDialog() = DialogResult.OK Then OpenVideoSourceFiles(d.FileNames)
                End Using
            Case "Merge Files"
                Using form As New SourceFilesForm()
                    form.Text = "Merge"
                    form.IsMerge = True

                    If form.ShowDialog() = DialogResult.OK AndAlso form.lb.Items.Count > 0 Then
                        Dim files = form.GetFiles

                        Select Case FilePath.GetExt(files(0))
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
                                        MsgInfo("Manual Merging", "Please merge the files manually with a appropriate tool or visit the support forum].")
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
            Case "File Batch"
                If AbortDueToLowDiskSpace() Then Exit Sub

                Using form As New SourceFilesForm()
                    form.Text = "File Batch"
                    If p.DefaultTargetName = "%source_dir_name%" Then p.DefaultTargetName = "%source_name%"

                    If form.ShowDialog() = DialogResult.OK AndAlso form.lb.Items.Count > 0 Then
                        If p.SourceFiles.Count > 0 AndAlso Not LoadTemplateWithSelectionDialog() Then Exit Sub

                        Dim batchFolder = Folder.Settings + "Batch Projects\"
                        If Not Directory.Exists(batchFolder) Then Directory.CreateDirectory(batchFolder)

                        For Each i In form.GetFiles
                            Dim batchProject = ObjectHelp.GetCopy(Of Project)(p)
                            batchProject.BatchMode = True
                            batchProject.SourceFiles = {i}.ToList
                            Dim jobPath = batchFolder + i.Replace("\", "-").Replace(":", "-")
                            SafeSerialization.Serialize(batchProject, jobPath)
                            Job.AddJob(jobPath)
                        Next

                        ShowJobsDialog()
                    End If
                End Using
            Case "Blu-ray Folder"
                If p.SourceFile <> "" Then If Not OpenProject() Then Exit Sub

                Using d As New FolderBrowserDialog
                    d.Description = "Please select a Blu-ray source folder."
                    d.SetSelectedPath(s.Storage.GetString("last blu-ray source folder"))
                    d.ShowNewFolderButton = False

                    If d.ShowDialog = DialogResult.OK Then
                        s.Storage.SetString("last blu-ray source folder", d.SelectedPath.FixDir)
                        Dim srcPath = d.SelectedPath.FixDir

                        If Directory.Exists(srcPath + "BDMV") Then srcPath = srcPath + "BDMV\"
                        If Directory.Exists(srcPath + "PLAYLIST") Then srcPath = srcPath + "PLAYLIST\"

                        If Not srcPath.ToUpper.EndsWith("PLAYLIST\") Then
                            MsgWarn("No playlist directory found.")
                            Exit Sub
                        End If

                        Log.WriteEnvironment()
                        Log.Write("Process Blu-Ray folder using eac3to", """" + Package.eac3to.Path + """ """ + srcPath + """" + BR2)
                        Log.WriteLine("Source Drive Type: " + New DriveInfo(d.SelectedPath).DriveType.ToString + BR)

                        Dim output = ProcessHelp.GetStdOut(Package.eac3to.Path, srcPath.Escape).Replace(VB6.vbBack, "")
                        Log.WriteLine(output)

                        Dim a = Regex.Split(output, "^\d+\)", RegexOptions.Multiline).ToList
                        If a(0) = "" Then a.RemoveAt(0)

                        Dim td2 As New TaskDialog(Of Integer)
                        td2.MainInstruction = "Please select a playlist."

                        For Each i In a
                            If i.Contains(BR) Then
                                td2.AddCommandLink(i.Left(BR).Trim, i.Right(BR).TrimEnd, a.IndexOf(i) + 1)
                            End If
                        Next

                        If td2.Show() = 0 Then Exit Sub
                        OpenEac3toDemuxForm(srcPath, td2.SelectedValue)
                    End If
                End Using
        End Select
    End Sub

    Sub OpenEac3toDemuxForm(playlistFolder As String, playlistID As Integer)
        Using form As New eac3toForm(p)
            form.PlaylistFolder = playlistFolder
            form.PlaylistID = playlistID

            Dim workDir = playlistFolder.Parent.Parent

            Dim title = InputBox.Show("Enter a short title used as filename.", "Title",
                                      playlistFolder.Parent.Parent.DirName)

            If title = "" Then Exit Sub

            If p.TempDir <> "" Then
                workDir = p.TempDir.Replace("%source_name%", title)
            Else
                workDir += title
            End If

            If Not DirPath.IsFixedDrive(workDir) Then
                Using d As New FolderBrowserDialog
                    d.Description = "Please select a folder for temporary files on a fixed local drive."
                    d.SetSelectedPath(s.Storage.GetString("last blu-ray target folder").Parent)

                    If d.ShowDialog = DialogResult.OK Then
                        workDir = d.SelectedPath.FixDir
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

                If Not DirPath.IsFixedDrive(form.OutputFolder) Then
                    MsgError("Only fixed local drives are supported as temp dir.")
                    Exit Sub
                End If

                Try
                    Dim di As New DriveInfo(form.OutputFolder)

                    If di.AvailableFreeSpace / 1024 ^ 3 < 50 Then
                        MsgError("The target drive has not enough free disk space.")
                        Exit Sub
                    End If

                    Using proc As New Proc
                        proc.Header = "Demux M2TS"
                        proc.TrimChars = {"-"c, " "c}
                        proc.RemoveChars = {CChar(VB6.vbBack)}
                        proc.SkipStrings = {"analyze: ", "process: "}
                        proc.Package = Package.eac3to
                        proc.Process.StartInfo.Arguments = form.GetArgs(
                            playlistFolder.Escape + " " & playlistID & ")", title)

                        Try
                            proc.Start()
                        Catch ex As AbortException
                            Exit Sub
                        Catch ex As Exception
                            g.ShowException(ex)
                            Exit Sub
                        End Try
                    End Using

                    s.Storage.SetString("last blu-ray target folder", form.OutputFolder)
                    Dim fs = form.OutputFolder + title + "." + form.cbVideoOutput.Text.ToLower

                    If File.Exists(fs) Then
                        p.TempDir = form.OutputFolder
                        OpenVideoSourceFile(fs)
                    End If
                Catch
                End Try
            End If
        End Using
    End Sub

    Private Function GetSourceFilesFromDir(path As String) As String()
        For Each i In FileTypes.Video
            Dim files = Directory.GetFiles(path, "*." + i)
            Array.Sort(files)
            If files.Length > 0 Then Return files
        Next
    End Function

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case 800 'WM_DWMCOLORIZATIONCOLORCHANGED
                If ToolStripRendererEx.IsAutoRenderMode Then
                    ToolStripRendererEx.InitColors(s.ToolStripRenderModeEx)
                    SetMenuStyle()
                    MenuStrip.Refresh()
                End If
        End Select

        MyBase.WndProc(m)
    End Sub

    Sub tbSource_DoubleClick() Handles tbSourceFile.DoubleClick
        Using d As New OpenFileDialog
            d.SetFilter(FileTypes.Video)
            d.Multiselect = True
            d.SetInitDir(s.LastSourceDir)

            If d.ShowDialog() = DialogResult.OK Then
                Refresh()

                Dim l As New List(Of String)(d.FileNames)
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

        If sb.Show = DialogResult.OK Then Return sb.SelectedValue
    End Function

    Sub tbAudioFile0_DoubleClick() Handles tbAudioFile0.DoubleClick
        Using d As New OpenFileDialog
            Dim filter = FileTypes.Audio.ToList
            filter.Insert(0, "avs")
            d.SetFilter(filter)
            d.SetInitDir(p.TempDir, s.LastSourceDir)
            If d.ShowDialog() = DialogResult.OK Then tbAudioFile0.Text = d.FileName
        End Using
    End Sub

    Sub tbAudioFile1_DoubleClick() Handles tbAudioFile1.DoubleClick
        Using d As New OpenFileDialog
            Dim filter = FileTypes.Audio.ToList
            filter.Insert(0, "avs")
            d.SetFilter(filter)
            d.SetInitDir(p.TempDir, s.LastSourceDir)
            If d.ShowDialog() = DialogResult.OK Then tbAudioFile1.Text = d.FileName
        End Using
    End Sub

    Private Sub lTip_Click() Handles lTip.Click
        If Not AssistantMethod Is Nothing Then
            AssistantMethod.Invoke()
            Assistant()
        End If
    End Sub

    Sub UpdateTargetParameters(seconds As Integer, frameRate As Double)
        p.TargetSeconds = seconds
        p.TargetFrameRate = frameRate
        UpdateSizeOrBitrate()
    End Sub

    Private Sub pEncoder_MouseLeave() Handles pnEncoder.MouseLeave
        Assistant()
    End Sub

    Private Sub AudioEdit0ToolStripMenuItemClick()
        p.Audio0.EditProject()
        UpdateAudioMenu()
        UpdateSizeOrBitrate()
        llAudioProfile0.Text = g.ConvertPath(p.Audio0.Name)
    End Sub

    Private Sub AudioEdit1ToolStripMenuItemClick()
        p.Audio1.EditProject()
        UpdateAudioMenu()
        UpdateSizeOrBitrate()
        llAudioProfile1.Text = g.ConvertPath(p.Audio1.Name)
    End Sub

    Private Sub AudioSource0ToolStripMenuItemClick()
        tbAudioFile0_DoubleClick()
    End Sub

    Private Sub AudioSource1ToolStripMenuItemClick()
        tbAudioFile1_DoubleClick()
    End Sub

    <Command("Shows a dialog allowing to reset various settings.")>
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

    <Command("Dialog to manage audio profiles.")>
    Sub ShowAudioProfilesDialog(<DispName("Track Number")> number As Integer)
        Dim f As ProfilesForm

        If number = 0 Then
            f = New ProfilesForm("Audio Profiles", s.AudioProfiles, AddressOf g.LoadAudioProfile0, AddressOf GetAudioProfile0, AddressOf AudioProfile.GetDefaults)
        Else
            f = New ProfilesForm("Audio Profiles", s.AudioProfiles, AddressOf g.LoadAudioProfile1, AddressOf GetAudioProfile1, AddressOf AudioProfile.GetDefaults)
        End If

        f.ShowDialog()
        f.Dispose()

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

    Private Sub AviSynthListView_ScriptChanged() Handles FiltersListView.Changed
        If Not IsLoading AndAlso Not FiltersListView.IsLoading Then
            Package.DGDecodeNV.VerifyOK()
            Package.DGDecodeIM.VerifyOK()

            If g.IsValidSource(False) Then
                UpdateSourceParameters()
                UpdateTargetParameters(p.Script.GetSeconds, p.Script.GetFramerate)
            End If

            Assistant()
        End If
    End Sub

    Sub UpdateFilters()
        FiltersListView.Load()
        If g.IsValidSource(False) Then UpdateTargetParameters(p.Script.GetSeconds, p.Script.GetFramerate)
    End Sub

    Private Sub AviSynthListView_DoubleClick() Handles FiltersListView.DoubleClick
        FiltersListView.ShowEditor()
    End Sub

    Private Sub gbFilters_MenuClick() Handles lgbFilters.LinkClick
        FiltersListView.ContextMenuStrip.Show(lgbFilters, 0, 16)
    End Sub

    Private Sub gbEncoder_LinkClick() Handles lgbEncoder.LinkClick
        EncoderMenu.Items.ClearAndDisplose
        g.PopulateProfileMenu(EncoderMenu.Items, s.VideoEncoderProfiles, AddressOf ShowEncoderProfilesDialog, AddressOf g.LoadVideoEncoder)
        EncoderMenu.Show(lgbEncoder, 0, 16)
    End Sub

    Private Sub llSize_Click() Handles blFilesize.Click
        UpdateSizeMenu()
        SizeContextMenuStrip.Show(blFilesize, 0, 16)
    End Sub

    Private Sub lAudioProfile0_Click() Handles llAudioProfile0.Click
        AudioMenu0.Show(llAudioProfile0, 0, 16)
    End Sub

    Private Sub lAudioProfile1_Click() Handles llAudioProfile1.Click
        AudioMenu1.Show(llAudioProfile1, 0, 16)
    End Sub

    Private Sub llContainer_Click() Handles llMuxer.Click
        ContainerMenu.Items.ClearAndDisplose
        g.PopulateProfileMenu(ContainerMenu.Items, s.MuxerProfiles, AddressOf ShowMuxerProfilesDialog, AddressOf p.VideoEncoder.LoadMuxer)
        ContainerMenu.Show(llMuxer, 0, 16)
    End Sub

    Private Sub gbResize_LinkClick() Handles lgbResize.LinkClick
        Dim cms = TextCustomMenu.GetMenu(s.TargetImageSizeMenu, lgbResize.Label, components, AddressOf TargetImageMenuClick)
        cms.Add("-")
        cms.Add("Image Options...", Sub() ShowOptionsDialog("Image")).SetImage(Symbol.fa_photo)
        cms.Add("Edit Menu...", Sub() s.TargetImageSizeMenu = TextCustomMenu.EditMenu(s.TargetImageSizeMenu, ApplicationSettings.GetDefaultTargetImageSizeMenu, Me))
        cms.Show(lgbResize, 0, lgbResize.Label.Height)
    End Sub

    Private Sub blSourceParText_Click(sender As Object, e As EventArgs) Handles blSourceParText.Click
        Dim menu = TextCustomMenu.GetMenu(s.ParMenu, blSourceParText, components, AddressOf SourceParMenuClick)
        menu.Add("-")
        menu.Items.Add(New ActionMenuItem("Edit Menu...", Sub() s.ParMenu = TextCustomMenu.EditMenu(s.ParMenu, ApplicationSettings.GetParMenu, Me)))
        menu.Show(blSourceParText, 0, blSourceParText.Height)
    End Sub

    Private Sub blSourceDarText_Click(sender As Object, e As EventArgs) Handles blSourceDarText.Click
        Dim menu = TextCustomMenu.GetMenu(s.DarMenu, blSourceDarText, components, AddressOf SourceDarMenuClick)
        menu.Add("-")
        menu.Items.Add(New ActionMenuItem("Edit Menu...", Sub() s.DarMenu = TextCustomMenu.EditMenu(s.DarMenu, ApplicationSettings.GetDarMenu, Me)))
        menu.Show(blSourceDarText, 0, blSourceDarText.Height)
    End Sub

    Private Sub blTargetDarText_Click(sender As Object, e As EventArgs) Handles blTargetDarText.Click
        Dim menu = TextCustomMenu.GetMenu(s.DarMenu, blTargetDarText, components, AddressOf TargetDarMenuClick)
        menu.Add("-")
        menu.Items.Add(New ActionMenuItem("Edit Menu...", Sub() s.DarMenu = TextCustomMenu.EditMenu(s.DarMenu, ApplicationSettings.GetDarMenu, Me)))
        menu.Show(blTargetDarText, 0, blTargetDarText.Height)
    End Sub

    Private Sub blTargetParText_Click(sender As Object, e As EventArgs) Handles blTargetParText.Click
        Dim menu = TextCustomMenu.GetMenu(s.ParMenu, blTargetParText, components, AddressOf TargetParMenuClick)
        menu.Add("-")
        menu.Items.Add(New ActionMenuItem("Edit Menu...", Sub() s.ParMenu = TextCustomMenu.EditMenu(s.ParMenu, ApplicationSettings.GetParMenu, Me)))
        menu.Show(blTargetParText, 0, blTargetParText.Height)
    End Sub

    Sub SourceDarMenuClick(value As String)
        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)
        If tup.Cancel Then Exit Sub
        p.CustomSourcePAR = ""
        p.CustomSourceDAR = tup.Value
        If Calc.ParseCustomAR(tup.Value, 0, 0).X = 0 Then p.CustomSourceDAR = ""
        Assistant()
    End Sub

    Sub SourceParMenuClick(value As String)
        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)
        If tup.Cancel Then Exit Sub
        p.CustomSourcePAR = tup.Value
        p.CustomSourceDAR = ""
        If Calc.ParseCustomAR(tup.Value, 0, 0).X = 0 Then p.CustomSourcePAR = ""
        Assistant()
    End Sub

    Sub TargetDarMenuClick(value As String)
        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)
        If tup.Cancel Then Exit Sub
        p.CustomTargetPAR = ""
        p.CustomTargetDAR = tup.Value
        If Calc.ParseCustomAR(tup.Value, 0, 0).X = 0 Then p.CustomTargetDAR = ""
        Assistant()
    End Sub

    Sub TargetParMenuClick(value As String)
        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)
        If tup.Cancel Then Exit Sub
        p.CustomTargetPAR = tup.Value
        p.CustomTargetDAR = ""
        If Calc.ParseCustomAR(tup.Value, 0, 0).X = 0 Then p.CustomTargetPAR = ""
        Assistant()
    End Sub

    Sub TargetImageMenuClick(value As String)
        g.EnableFilter("Resize")

        value = Macro.Expand(value)
        Dim tup = Macro.ExpandGUI(value)
        If tup.Cancel Then Exit Sub
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
        If p.Script.IsFilterActive("Resize)") Then SetTargetImageSize(p.TargetWidth, 0)
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
            .SetTip("Target Video Bitrate in Kbps (Up/Down key)", tbBitrate, lBitrate)
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
                p.SourceWidth = p.SourceScript.GetSize.Width
                p.SourceHeight = p.SourceScript.GetSize.Height
                p.SourceSeconds = CInt(p.SourceScript.GetFrames / p.SourceScript.GetFramerate)
                p.SourceFrameRate = p.SourceScript.GetFramerate
                p.SourceFrames = p.SourceScript.GetFrames
            Catch ex As Exception
                MsgError("Source filter returned invalid parameters", p.SourceScript.GetFullScript)
                Throw New AbortException()
            End Try
        End If
    End Sub

    Private Sub tbSource_TextChanged(sender As Object, e As EventArgs) Handles tbSourceFile.TextChanged
        If Not BlockSourceTextBoxTextChanged Then
            If File.Exists(tbSourceFile.Text) Then OpenVideoSourceFile(tbSourceFile.Text)
        End If
    End Sub

    Private Sub gbTarget_LinkClick() Handles lgbTarget.LinkClick
        UpdateTargetFileMenu()
        TargetFileMenu.Show(lgbTarget, 0, lgbTarget.Label.Height)
    End Sub

    Private Sub gbSource_MenuClick() Handles lgbSource.LinkClick
        UpdateSourceFileMenu()

        If File.Exists(p.SourceFile) Then
            SourceFileMenu.Show(lgbSource, 0, lgbSource.Label.Height)
        Else
            ShowOpenSourceDialog()
        End If
    End Sub

    Sub tbTargetFile_DoubleClick() Handles tbTargetFile.DoubleClick
        Using d As New SaveFileDialog
            d.FileName = p.TargetFile.Base
            d.SetInitDir(p.TargetFile.Dir)

            If d.ShowDialog() = DialogResult.OK Then
                Dim ext = p.VideoEncoder.Muxer.OutputExtFull
                p.TargetFile = If(d.FileName.Ext = ext, d.FileName, d.FileName + ext)
            End If
        End Using
    End Sub

    Private Sub tbTargetFile_MouseDown(sender As Object, e As MouseEventArgs) Handles tbTargetFile.MouseDown
        If e.Button = MouseButtons.Right Then UpdateTargetFileMenu()
    End Sub

    Private Sub tbSourceFile_MouseDown(sender As Object, e As MouseEventArgs) Handles tbSourceFile.MouseDown
        If e.Button = MouseButtons.Right Then UpdateSourceFileMenu()
    End Sub

    Private Sub tbAudioFile0_MouseDown(sender As Object, e As MouseEventArgs) Handles tbAudioFile0.MouseDown
        If e.Button = MouseButtons.Right Then
            UpdateAudioFileMenu(Audio0FileMenu, AddressOf tbAudioFile0_DoubleClick, p.Audio0, tbAudioFile0)
        End If
    End Sub

    Private Sub tbAudioFile1_MouseDown(sender As Object, e As MouseEventArgs) Handles tbAudioFile1.MouseDown
        If e.Button = MouseButtons.Right Then
            UpdateAudioFileMenu(Audio1FileMenu, AddressOf tbAudioFile1_DoubleClick, p.Audio1, tbAudioFile1)
        End If
    End Sub

    Sub UpdateAudioFileMenu(m As ContextMenuStripEx,
                            a As Action,
                            ap As AudioProfile,
                            tb As TextBox)

        m.Items.ClearAndDisplose
        Dim exist = File.Exists(ap.File)

        If ap.Streams.Count > 0 Then
            For Each i In ap.Streams
                Dim temp = i

                Dim menuAction = Sub()
                                     If ap.File <> p.LastOriginalSourceFile Then tb.Text = p.LastOriginalSourceFile
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
                        m.Add("Files | " + FilePath.GetName(i), Sub() tb.Text = temp)
                    Else
                        m.Add(FilePath.GetName(i), Sub() tb.Text = temp)
                    End If
                Next

                m.Items.Add("-")
            End If
        End If

        m.Add("Open", a, "Change the audio source file.").SetImage(Symbol.OpenFile)
        m.Add("Play", Sub() g.PlayAudio(ap), "Plays the audio source file with a media player.", exist).SetImage(Symbol.Play)
        m.Add("MediaInfo...", Sub() g.DefaultCommands.ShowMediaInfo(ap.File), "Show MediaInfo for the audio source file.", exist).SetImage(Symbol.Info)
        m.Add("Explore", Sub() g.OpenDirAndSelectFile(ap.File, Handle), "Open the audio source file directory with File Explorer.", exist).SetImage(Symbol.FileExplorer)
        m.Add("Execute", Sub() ExecuteAudio(ap), "Processes the audio profile.", exist).SetImage(Symbol.fa_terminal)
        m.Add("-")
        m.Add("Copy Path", Sub() Clipboard.SetText(ap.File), Nothing, tb.Text <> "")
        m.Add("Copy Selection", Sub() Clipboard.SetText(tb.SelectedText), Nothing, tb.Text <> "").SetImage(Symbol.Copy)
        m.Add("Paste", Sub() tb.Paste(), Nothing, Clipboard.GetText.Trim <> "").SetImage(Symbol.Paste)
        m.Add("-")
        m.Add("Remove", Sub() tb.Text = "", "Remove audio file", tb.Text <> "").SetImage(Symbol.Remove)
    End Sub

    Sub ExecuteAudio(ap As AudioProfile)
        Try
            If p.TempDir = "" Then p.TempDir = ap.File.Dir
            ap = ObjectHelp.GetCopy(Of AudioProfile)(ap)
            Audio.Process(ap)
            ap.Encode()
        Catch
        End Try
    End Sub

    Sub UpdateTargetFileMenu()
        TargetFileMenu.Items.ClearAndDisplose
        TargetFileMenu.Add("Edit...", AddressOf tbTargetFile_DoubleClick, "Change the path of the target file.")
        TargetFileMenu.Add("Play...", Sub() g.Play(p.TargetFile), "Play the target file.", File.Exists(p.TargetFile)).SetImage(Symbol.Play)
        TargetFileMenu.Add("MediaInfo...", Sub() g.DefaultCommands.ShowMediaInfo(p.TargetFile), "Show MediaInfo for the target file.", File.Exists(p.TargetFile)).SetImage(Symbol.Info)
        TargetFileMenu.Add("Explore...", Sub() g.OpenDirAndSelectFile(p.TargetFile, Handle), "Open the target file directory with File Explorer.", Directory.Exists(p.TargetFile.Dir)).SetImage(Symbol.FileExplorer)
        TargetFileMenu.Add("-")
        TargetFileMenu.Add("Copy", Sub() tbTargetFile.Copy(), "", tbTargetFile.Text <> "").SetImage(Symbol.Copy)
        TargetFileMenu.Add("Paste", Sub() tbTargetFile.Paste(), "", Clipboard.GetText.Trim <> "").SetImage(Symbol.Paste)
    End Sub

    Sub UpdateSourceFileMenu()
        SourceFileMenu.Items.ClearAndDisplose
        Dim isIndex = FileTypes.VideoIndex.Contains(FilePath.GetExt(p.SourceFile))

        SourceFileMenu.Add("Open...", AddressOf ShowOpenSourceDialog, "Open source files").SetImage(Symbol.OpenFile)
        SourceFileMenu.Add("Play...", Sub() g.Play(p.SourceFile), "Play the source file.", File.Exists(p.SourceFile) AndAlso Not isIndex).SetImage(Symbol.Play)
        SourceFileMenu.Add("MediaInfo...", Sub() g.DefaultCommands.ShowMediaInfo(p.SourceFile), "Show MediaInfo for the source file.", File.Exists(p.SourceFile) AndAlso Not isIndex).SetImage(Symbol.Info)
        SourceFileMenu.Add("Explore...", Sub() g.OpenDirAndSelectFile(p.SourceFile, Handle), "Open the source file directory with File Explorer.", File.Exists(p.SourceFile)).SetImage(Symbol.FileExplorer)
        SourceFileMenu.Items.Add("-")
        SourceFileMenu.Add("Copy", Sub() tbSourceFile.Copy(), "Copies the selected text to the clipboard.", tbSourceFile.Text <> "").SetImage(Symbol.Copy)
        SourceFileMenu.Add("Paste", Sub() tbSourceFile.Paste(), "Copies the full source file path to the clipboard.", Clipboard.GetText.Trim <> "").SetImage(Symbol.Paste)
    End Sub
    <Command("Shows a dialog to generate thumbnails.")>
    Sub ShowBatchGenerateThumbnailsDialog()
        Using fd As New OpenFileDialog
            fd.Title = "Select files"
            fd.SetFilter(FileTypes.Video)
            fd.Multiselect = True

            If fd.ShowDialog = DialogResult.OK Then
                Using f As New SimpleSettingsForm("Thumbnail Options")
                    f.ScaleClientSize(27, 15)

                    Dim ui = f.SimpleUI
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
                    m.Button.Value = s.Storage.GetInt("Thumbnail Position", 1)
                    m.Button.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Position", value)

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
                    nb.Config = {1, 1000}
                    nb.NumEdit.Value = s.Storage.GetInt("Thumbnail Columns", 3)
                    nb.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Columns", CInt(value))

                    row = ui.AddNum()
                    row.Text = "Row Count:"
                    row.Config = {1, 1000}
                    row.NumEdit.Value = s.Storage.GetInt("Thumbnail Rows", 12)
                    row.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Rows", CInt(value))

                    interval = ui.AddNum()
                    interval.Text = "Interval (seconds):"
                    interval.NumEdit.Value = s.Storage.GetInt("Thumbnail Interval")
                    interval.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Interval", CInt(value))

                    row.Visible = mode.Button.Value = 0
                    interval.Visible = mode.Button.Value = 1

                    nb = ui.AddNum()
                    nb.Text = "Compression Quality:"
                    nb.Config = {1, 100}
                    nb.NumEdit.Value = s.Storage.GetInt("Thumbnail Compression Quality", 95)
                    nb.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Compression Quality", CInt(value))

                    page.ResumeLayout()

                    If f.ShowDialog() = DialogResult.OK Then
                        ui.Save()

                        For Each i In fd.FileNames
                            Try
                                Thumbnails.SaveThumbnails(i, Nothing)
                            Catch ex As Exception
                                g.ShowException(ex)
                            End Try
                        Next

                        g.StartProcess(fd.FileName.Dir)
                    End If
                End Using
            End If
        End Using
    End Sub
    <Command("Presents MediaInfo of all files in a folder in a list view.")>
    Sub ShowMediaInfoFolderViewDialog()
        Using d As New FolderBrowserDialog
            d.ShowNewFolderButton = False
            d.SetSelectedPath(s.Storage.GetString("MediaInfo Folder View folder"))

            If d.ShowDialog = DialogResult.OK Then
                s.Storage.SetString("MediaInfo Folder View folder", d.SelectedPath)
                Dim f As New MediaInfoFolderViewForm(d.SelectedPath.FixDir)
                f.Show()
            End If
        End Using
    End Sub

    Protected Overrides Sub OnDragEnter(e As DragEventArgs)
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
        If Not files.NothingOrEmpty Then e.Effect = DragDropEffects.Copy
        MyBase.OnDragEnter(e)
    End Sub

    Protected Overrides Sub OnDragDrop(e As DragEventArgs)
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
        If Not files.NothingOrEmpty Then BeginInvoke(Sub() OpenAnyFile(files.ToList))
        MyBase.OnDragDrop(e)
    End Sub

    Protected Overrides Sub OnActivated(e As EventArgs)
        Assistant()
        UpdateScriptsMenuAsync()
        MyBase.OnActivated(e)
        g.WriteDebugLog("MainForm.Activated")
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        UpdateDynamicMenuAsync()
        UpdateRecentProjectsMenu()
        UpdateTemplatesMenuAsync()
        IsLoading = False
        Refresh()
        ProcessCommandLine(Environment.GetCommandLineArgs)
        MyBase.OnShown(e)
        'TestForm.ShowForm()
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        If IsSaveCanceled() Then e.Cancel = True
        MyBase.OnFormClosing(e)
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        If Not g.ProcForm Is Nothing Then g.ProcForm.Invoke(Sub() g.ProcForm.Close())
        g.SaveSettings()
        g.RaiseAppEvent(ApplicationEvent.ApplicationExit)
        MyBase.OnFormClosed(e)
    End Sub

    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            Dim hwnd = Native.GetForegroundWindow()
            Dim styles = Native.GetWindowLong(hwnd, -16) 'GWL_STYLE
            If (&HC00000L And styles) <> &HC00000L Then Return True 'WS_CAPTION
            If ProcController.BlockActivation Then Return True
            Return MyBase.ShowWithoutActivation
        End Get
    End Property
End Class