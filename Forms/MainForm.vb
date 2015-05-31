Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Globalization
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms.VisualStyles

Imports Microsoft.Win32
Imports StaxRip.UI
Imports SWF = System.Windows.Forms
Imports VB6 = Microsoft.VisualBasic
Imports System.Threading.Tasks
Imports System.Runtime.InteropServices

Public Class MainForm
    Inherits FormBase

#Region " Designer "
    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    Public WithEvents tbAudioFile0 As StaxRip.UI.TextBoxEx
    Public WithEvents tbAudioFile1 As StaxRip.UI.TextBoxEx
    Friend WithEvents llEditAudio1 As System.Windows.Forms.LinkLabel
    Friend WithEvents llEditAudio0 As System.Windows.Forms.LinkLabel
    Public WithEvents bNext As System.Windows.Forms.Button
    Public WithEvents tbSourceFile As StaxRip.UI.TextBoxEx
    Public WithEvents tbTargetFile As StaxRip.UI.TextBoxEx
    Public WithEvents gbAssistant As System.Windows.Forms.GroupBox
    Friend WithEvents lgbFilters As LinkGroupBox
    Friend WithEvents lTargetLength As System.Windows.Forms.Label
    Friend WithEvents tbSize As System.Windows.Forms.TextBox
    Public WithEvents lAudioBitrate As System.Windows.Forms.Label
    Public WithEvents lAudioBitrateText As System.Windows.Forms.Label
    Friend WithEvents lBitrate As System.Windows.Forms.Label
    Friend WithEvents tbBitrate As System.Windows.Forms.TextBox
    Friend WithEvents lTargetLengthText As System.Windows.Forms.Label
    Friend WithEvents lbSourceLength As System.Windows.Forms.Label
    Friend WithEvents lbSourceLengthText As System.Windows.Forms.Label
    Friend WithEvents lSourceFramerate As System.Windows.Forms.Label
    Friend WithEvents lSourceFramerateText As System.Windows.Forms.Label
    Friend WithEvents lgbResize As LinkGroupBox
    Friend WithEvents lPixel As System.Windows.Forms.Label
    Friend WithEvents lPixelText As System.Windows.Forms.Label
    Friend WithEvents tbResize As System.Windows.Forms.TrackBar
    Friend WithEvents lZoom As System.Windows.Forms.Label
    Friend WithEvents lZoomText As System.Windows.Forms.Label
    Public WithEvents tbTargetHeight As System.Windows.Forms.TextBox
    Public WithEvents tbTargetWidth As System.Windows.Forms.TextBox
    Friend WithEvents lTargetHeight As System.Windows.Forms.Label
    Friend WithEvents lTargetWidth As System.Windows.Forms.Label
    Friend WithEvents lDAR As System.Windows.Forms.Label
    Friend WithEvents lDarText As System.Windows.Forms.Label
    Public WithEvents lCrop As System.Windows.Forms.Label
    Friend WithEvents lCropText As System.Windows.Forms.Label
    Friend WithEvents lgbSource As LinkGroupBox
    Friend WithEvents lSourceImageSize As System.Windows.Forms.Label
    Friend WithEvents lSourceImageSizeText As System.Windows.Forms.Label
    Friend WithEvents lgbTarget As LinkGroupBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lTip As System.Windows.Forms.Label
    Friend WithEvents lgbEncoder As LinkGroupBox
    Public WithEvents lQuality As System.Windows.Forms.Label
    Public WithEvents lQualityText As System.Windows.Forms.Label
    Public WithEvents lCompCheck As System.Windows.Forms.Label
    Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents llAudioProfile0 As LinkLabel
    Friend WithEvents llAudioProfile1 As LinkLabel
    Friend WithEvents pEncoder As System.Windows.Forms.Panel
    Friend WithEvents AviSynthListView As StaxRip.AviSynthListView
    Friend WithEvents llFilesize As System.Windows.Forms.LinkLabel
    Friend WithEvents llMuxer As System.Windows.Forms.LinkLabel
    Friend WithEvents lPAR As StaxRip.UI.LabelEx
    Friend WithEvents lParText As StaxRip.UI.LabelEx
    Friend WithEvents lAspectRatioError As StaxRip.UI.LabelEx
    Friend WithEvents lAspectRatioErrorText As StaxRip.UI.LabelEx
    Friend WithEvents gbAudio As System.Windows.Forms.GroupBox
    Friend WithEvents llSourceParText As System.Windows.Forms.LinkLabel
    Friend WithEvents lSourcePAR As System.Windows.Forms.Label
    Friend WithEvents lSourceDar As System.Windows.Forms.Label
    Friend WithEvents lSourceDarText As System.Windows.Forms.Label
    Friend WithEvents lSAR As StaxRip.UI.LabelEx
    Friend WithEvents lSarText As StaxRip.UI.LabelEx
    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents lCodec As System.Windows.Forms.Label
    Friend WithEvents lCodecProfile As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents lCompCheckText As System.Windows.Forms.Label

    '<System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.bNext = New System.Windows.Forms.Button()
        Me.llEditAudio0 = New System.Windows.Forms.LinkLabel()
        Me.gbAssistant = New System.Windows.Forms.GroupBox()
        Me.lTip = New System.Windows.Forms.Label()
        Me.llEditAudio1 = New System.Windows.Forms.LinkLabel()
        Me.gbAudio = New System.Windows.Forms.GroupBox()
        Me.llAudioProfile1 = New System.Windows.Forms.LinkLabel()
        Me.llAudioProfile0 = New System.Windows.Forms.LinkLabel()
        Me.tbAudioFile0 = New StaxRip.UI.TextBoxEx()
        Me.tbAudioFile1 = New StaxRip.UI.TextBoxEx()
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.lgbTarget = New StaxRip.UI.LinkGroupBox()
        Me.lCompCheck = New System.Windows.Forms.Label()
        Me.lAudioBitrate = New System.Windows.Forms.Label()
        Me.llFilesize = New System.Windows.Forms.LinkLabel()
        Me.lTargetLength = New System.Windows.Forms.Label()
        Me.tbSize = New System.Windows.Forms.TextBox()
        Me.lAudioBitrateText = New System.Windows.Forms.Label()
        Me.lQuality = New System.Windows.Forms.Label()
        Me.lQualityText = New System.Windows.Forms.Label()
        Me.lCompCheckText = New System.Windows.Forms.Label()
        Me.lBitrate = New System.Windows.Forms.Label()
        Me.tbBitrate = New System.Windows.Forms.TextBox()
        Me.lTargetLengthText = New System.Windows.Forms.Label()
        Me.tbTargetFile = New StaxRip.UI.TextBoxEx()
        Me.lgbSource = New StaxRip.UI.LinkGroupBox()
        Me.lCodec = New System.Windows.Forms.Label()
        Me.lCodecProfile = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lCrop = New System.Windows.Forms.Label()
        Me.lSourceImageSize = New System.Windows.Forms.Label()
        Me.lbSourceLength = New System.Windows.Forms.Label()
        Me.lSourceFramerate = New System.Windows.Forms.Label()
        Me.lSourceDar = New System.Windows.Forms.Label()
        Me.lSourceDarText = New System.Windows.Forms.Label()
        Me.lSourcePAR = New System.Windows.Forms.Label()
        Me.llSourceParText = New System.Windows.Forms.LinkLabel()
        Me.lCropText = New System.Windows.Forms.Label()
        Me.lSourceImageSizeText = New System.Windows.Forms.Label()
        Me.lbSourceLengthText = New System.Windows.Forms.Label()
        Me.lSourceFramerateText = New System.Windows.Forms.Label()
        Me.tbSourceFile = New StaxRip.UI.TextBoxEx()
        Me.lgbResize = New StaxRip.UI.LinkGroupBox()
        Me.lPAR = New StaxRip.UI.LabelEx()
        Me.lAspectRatioError = New StaxRip.UI.LabelEx()
        Me.lZoom = New System.Windows.Forms.Label()
        Me.lPixel = New System.Windows.Forms.Label()
        Me.lSarText = New StaxRip.UI.LabelEx()
        Me.lAspectRatioErrorText = New StaxRip.UI.LabelEx()
        Me.lParText = New StaxRip.UI.LabelEx()
        Me.lPixelText = New System.Windows.Forms.Label()
        Me.lZoomText = New System.Windows.Forms.Label()
        Me.lDarText = New System.Windows.Forms.Label()
        Me.lSAR = New StaxRip.UI.LabelEx()
        Me.lDAR = New System.Windows.Forms.Label()
        Me.tbResize = New System.Windows.Forms.TrackBar()
        Me.lTargetWidth = New System.Windows.Forms.Label()
        Me.tbTargetWidth = New System.Windows.Forms.TextBox()
        Me.tbTargetHeight = New System.Windows.Forms.TextBox()
        Me.lTargetHeight = New System.Windows.Forms.Label()
        Me.lgbFilters = New StaxRip.UI.LinkGroupBox()
        Me.AviSynthListView = New StaxRip.AviSynthListView()
        Me.lgbEncoder = New StaxRip.UI.LinkGroupBox()
        Me.llMuxer = New System.Windows.Forms.LinkLabel()
        Me.pEncoder = New System.Windows.Forms.Panel()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.gbAssistant.SuspendLayout()
        Me.gbAudio.SuspendLayout()
        Me.lgbTarget.SuspendLayout()
        Me.lgbSource.SuspendLayout()
        Me.lgbResize.SuspendLayout()
        CType(Me.tbResize, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.lgbFilters.SuspendLayout()
        Me.lgbEncoder.SuspendLayout()
        Me.SuspendLayout()
        '
        'bNext
        '
        Me.bNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bNext.Cursor = System.Windows.Forms.Cursors.Default
        Me.bNext.Location = New System.Drawing.Point(906, 36)
        Me.bNext.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.bNext.Name = "bNext"
        Me.bNext.Size = New System.Drawing.Size(65, 34)
        Me.bNext.TabIndex = 39
        Me.bNext.Text = "Next"
        '
        'llEditAudio0
        '
        Me.llEditAudio0.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llEditAudio0.Location = New System.Drawing.Point(928, 23)
        Me.llEditAudio0.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llEditAudio0.Name = "llEditAudio0"
        Me.llEditAudio0.Size = New System.Drawing.Size(48, 30)
        Me.llEditAudio0.TabIndex = 20
        Me.llEditAudio0.TabStop = True
        Me.llEditAudio0.Text = "Edit"
        Me.llEditAudio0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbAssistant
        '
        Me.gbAssistant.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbAssistant.Controls.Add(Me.lTip)
        Me.gbAssistant.Controls.Add(Me.bNext)
        Me.gbAssistant.Location = New System.Drawing.Point(10, 547)
        Me.gbAssistant.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbAssistant.Name = "gbAssistant"
        Me.gbAssistant.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbAssistant.Size = New System.Drawing.Size(980, 92)
        Me.gbAssistant.TabIndex = 44
        Me.gbAssistant.TabStop = False
        Me.gbAssistant.Text = "Assistant"
        '
        'lTip
        '
        Me.lTip.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lTip.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lTip.ForeColor = System.Drawing.Color.Blue
        Me.lTip.Location = New System.Drawing.Point(6, 21)
        Me.lTip.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lTip.Name = "lTip"
        Me.lTip.Size = New System.Drawing.Size(896, 68)
        Me.lTip.TabIndex = 40
        Me.lTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'llEditAudio1
        '
        Me.llEditAudio1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llEditAudio1.Location = New System.Drawing.Point(928, 60)
        Me.llEditAudio1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llEditAudio1.Name = "llEditAudio1"
        Me.llEditAudio1.Size = New System.Drawing.Size(48, 30)
        Me.llEditAudio1.TabIndex = 21
        Me.llEditAudio1.TabStop = True
        Me.llEditAudio1.Text = "Edit"
        Me.llEditAudio1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbAudio
        '
        Me.gbAudio.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbAudio.Controls.Add(Me.llEditAudio1)
        Me.gbAudio.Controls.Add(Me.llEditAudio0)
        Me.gbAudio.Controls.Add(Me.llAudioProfile1)
        Me.gbAudio.Controls.Add(Me.llAudioProfile0)
        Me.gbAudio.Controls.Add(Me.tbAudioFile0)
        Me.gbAudio.Controls.Add(Me.tbAudioFile1)
        Me.gbAudio.Location = New System.Drawing.Point(10, 442)
        Me.gbAudio.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbAudio.Name = "gbAudio"
        Me.gbAudio.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbAudio.Size = New System.Drawing.Size(980, 100)
        Me.gbAudio.TabIndex = 59
        Me.gbAudio.TabStop = False
        Me.gbAudio.Text = "Audio"
        '
        'llAudioProfile1
        '
        Me.llAudioProfile1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llAudioProfile1.Location = New System.Drawing.Point(658, 60)
        Me.llAudioProfile1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llAudioProfile1.Name = "llAudioProfile1"
        Me.llAudioProfile1.Size = New System.Drawing.Size(269, 30)
        Me.llAudioProfile1.TabIndex = 26
        Me.llAudioProfile1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'llAudioProfile0
        '
        Me.llAudioProfile0.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llAudioProfile0.Location = New System.Drawing.Point(658, 23)
        Me.llAudioProfile0.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llAudioProfile0.Name = "llAudioProfile0"
        Me.llAudioProfile0.Size = New System.Drawing.Size(269, 30)
        Me.llAudioProfile0.TabIndex = 25
        Me.llAudioProfile0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbAudioFile0
        '
        Me.tbAudioFile0.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAudioFile0.Location = New System.Drawing.Point(8, 24)
        Me.tbAudioFile0.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbAudioFile0.Size = New System.Drawing.Size(644, 31)
        '
        'tbAudioFile1
        '
        Me.tbAudioFile1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAudioFile1.Location = New System.Drawing.Point(8, 60)
        Me.tbAudioFile1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbAudioFile1.Size = New System.Drawing.Size(644, 31)
        '
        'MenuStrip
        '
        Me.MenuStrip.AutoSize = False
        Me.MenuStrip.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Padding = New System.Windows.Forms.Padding(3, 3, 0, 3)
        Me.MenuStrip.Size = New System.Drawing.Size(1004, 44)
        Me.MenuStrip.TabIndex = 60
        Me.MenuStrip.Text = "MenuStrip"
        '
        'lgbTarget
        '
        Me.lgbTarget.Controls.Add(Me.lCompCheck)
        Me.lgbTarget.Controls.Add(Me.lAudioBitrate)
        Me.lgbTarget.Controls.Add(Me.llFilesize)
        Me.lgbTarget.Controls.Add(Me.lTargetLength)
        Me.lgbTarget.Controls.Add(Me.tbSize)
        Me.lgbTarget.Controls.Add(Me.lAudioBitrateText)
        Me.lgbTarget.Controls.Add(Me.lQuality)
        Me.lgbTarget.Controls.Add(Me.lQualityText)
        Me.lgbTarget.Controls.Add(Me.lCompCheckText)
        Me.lgbTarget.Controls.Add(Me.lBitrate)
        Me.lgbTarget.Controls.Add(Me.tbBitrate)
        Me.lgbTarget.Controls.Add(Me.lTargetLengthText)
        Me.lgbTarget.Controls.Add(Me.tbTargetFile)
        Me.lgbTarget.Location = New System.Drawing.Point(508, 47)
        Me.lgbTarget.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbTarget.Name = "lgbTarget"
        Me.lgbTarget.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbTarget.Size = New System.Drawing.Size(483, 168)
        Me.lgbTarget.TabIndex = 58
        Me.lgbTarget.TabStop = False
        Me.lgbTarget.Text = "Target"
        '
        'lCompCheck
        '
        Me.lCompCheck.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lCompCheck.Location = New System.Drawing.Point(354, 129)
        Me.lCompCheck.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lCompCheck.Name = "lCompCheck"
        Me.lCompCheck.Size = New System.Drawing.Size(122, 30)
        Me.lCompCheck.TabIndex = 46
        Me.lCompCheck.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lAudioBitrate
        '
        Me.lAudioBitrate.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lAudioBitrate.Location = New System.Drawing.Point(330, 98)
        Me.lAudioBitrate.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lAudioBitrate.Name = "lAudioBitrate"
        Me.lAudioBitrate.Size = New System.Drawing.Size(146, 30)
        Me.lAudioBitrate.TabIndex = 52
        Me.lAudioBitrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'llFilesize
        '
        Me.llFilesize.Location = New System.Drawing.Point(7, 67)
        Me.llFilesize.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llFilesize.Name = "llFilesize"
        Me.llFilesize.Size = New System.Drawing.Size(78, 30)
        Me.llFilesize.TabIndex = 59
        Me.llFilesize.TabStop = True
        Me.llFilesize.Text = "Size:"
        Me.llFilesize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lTargetLength
        '
        Me.lTargetLength.Location = New System.Drawing.Point(86, 98)
        Me.lTargetLength.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lTargetLength.Name = "lTargetLength"
        Me.lTargetLength.Size = New System.Drawing.Size(118, 30)
        Me.lTargetLength.TabIndex = 56
        Me.lTargetLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSize
        '
        Me.tbSize.Location = New System.Drawing.Point(86, 66)
        Me.tbSize.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbSize.Name = "tbSize"
        Me.tbSize.Size = New System.Drawing.Size(80, 31)
        Me.tbSize.TabIndex = 55
        Me.tbSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lAudioBitrateText
        '
        Me.lAudioBitrateText.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lAudioBitrateText.Location = New System.Drawing.Point(205, 98)
        Me.lAudioBitrateText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lAudioBitrateText.Name = "lAudioBitrateText"
        Me.lAudioBitrateText.Size = New System.Drawing.Size(124, 30)
        Me.lAudioBitrateText.TabIndex = 51
        Me.lAudioBitrateText.Text = "Audio Bitrate:"
        Me.lAudioBitrateText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lQuality
        '
        Me.lQuality.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lQuality.Location = New System.Drawing.Point(86, 129)
        Me.lQuality.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lQuality.Name = "lQuality"
        Me.lQuality.Size = New System.Drawing.Size(118, 30)
        Me.lQuality.TabIndex = 48
        Me.lQuality.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lQualityText
        '
        Me.lQualityText.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lQualityText.Location = New System.Drawing.Point(7, 129)
        Me.lQualityText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lQualityText.Name = "lQualityText"
        Me.lQualityText.Size = New System.Drawing.Size(78, 30)
        Me.lQualityText.TabIndex = 47
        Me.lQualityText.Text = "Quality:"
        Me.lQualityText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lCompCheckText
        '
        Me.lCompCheckText.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lCompCheckText.Location = New System.Drawing.Point(205, 129)
        Me.lCompCheckText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lCompCheckText.Name = "lCompCheckText"
        Me.lCompCheckText.Size = New System.Drawing.Size(148, 30)
        Me.lCompCheckText.TabIndex = 45
        Me.lCompCheckText.Text = "Compressibility:"
        Me.lCompCheckText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lBitrate
        '
        Me.lBitrate.Location = New System.Drawing.Point(205, 67)
        Me.lBitrate.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lBitrate.Name = "lBitrate"
        Me.lBitrate.Size = New System.Drawing.Size(124, 30)
        Me.lBitrate.TabIndex = 42
        Me.lBitrate.Text = "Video Bitrate:"
        Me.lBitrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbBitrate
        '
        Me.tbBitrate.Location = New System.Drawing.Point(330, 66)
        Me.tbBitrate.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbBitrate.Name = "tbBitrate"
        Me.tbBitrate.Size = New System.Drawing.Size(80, 31)
        Me.tbBitrate.TabIndex = 41
        Me.tbBitrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lTargetLengthText
        '
        Me.lTargetLengthText.Location = New System.Drawing.Point(7, 98)
        Me.lTargetLengthText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lTargetLengthText.Name = "lTargetLengthText"
        Me.lTargetLengthText.Size = New System.Drawing.Size(78, 30)
        Me.lTargetLengthText.TabIndex = 39
        Me.lTargetLengthText.Text = "Length:"
        Me.lTargetLengthText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTargetFile
        '
        Me.tbTargetFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbTargetFile.Location = New System.Drawing.Point(8, 26)
        Me.tbTargetFile.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbTargetFile.Size = New System.Drawing.Size(467, 31)
        '
        'lgbSource
        '
        Me.lgbSource.Controls.Add(Me.lCodec)
        Me.lgbSource.Controls.Add(Me.lCodecProfile)
        Me.lgbSource.Controls.Add(Me.Label1)
        Me.lgbSource.Controls.Add(Me.Label2)
        Me.lgbSource.Controls.Add(Me.Label3)
        Me.lgbSource.Controls.Add(Me.lCrop)
        Me.lgbSource.Controls.Add(Me.lSourceImageSize)
        Me.lgbSource.Controls.Add(Me.lbSourceLength)
        Me.lgbSource.Controls.Add(Me.lSourceFramerate)
        Me.lgbSource.Controls.Add(Me.lSourceDar)
        Me.lgbSource.Controls.Add(Me.lSourceDarText)
        Me.lgbSource.Controls.Add(Me.lSourcePAR)
        Me.lgbSource.Controls.Add(Me.llSourceParText)
        Me.lgbSource.Controls.Add(Me.lCropText)
        Me.lgbSource.Controls.Add(Me.lSourceImageSizeText)
        Me.lgbSource.Controls.Add(Me.lbSourceLengthText)
        Me.lgbSource.Controls.Add(Me.lSourceFramerateText)
        Me.lgbSource.Controls.Add(Me.tbSourceFile)
        Me.lgbSource.Location = New System.Drawing.Point(13, 47)
        Me.lgbSource.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbSource.Name = "lgbSource"
        Me.lgbSource.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbSource.Size = New System.Drawing.Size(487, 168)
        Me.lgbSource.TabIndex = 57
        Me.lgbSource.TabStop = False
        Me.lgbSource.Text = "Source"
        '
        'lCodec
        '
        Me.lCodec.Location = New System.Drawing.Point(373, 68)
        Me.lCodec.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lCodec.Name = "lCodec"
        Me.lCodec.Size = New System.Drawing.Size(105, 30)
        Me.lCodec.TabIndex = 55
        Me.lCodec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lCodecProfile
        '
        Me.lCodecProfile.Location = New System.Drawing.Point(373, 102)
        Me.lCodecProfile.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lCodecProfile.Name = "lCodecProfile"
        Me.lCodecProfile.Size = New System.Drawing.Size(111, 60)
        Me.lCodecProfile.TabIndex = 57
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(307, 67)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 30)
        Me.Label1.TabIndex = 52
        Me.Label1.Text = "Codec:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(307, 98)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 30)
        Me.Label2.TabIndex = 54
        Me.Label2.Text = "Profile:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(307, 129)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 30)
        Me.Label3.TabIndex = 53
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lCrop
        '
        Me.lCrop.Location = New System.Drawing.Point(72, 129)
        Me.lCrop.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lCrop.Name = "lCrop"
        Me.lCrop.Size = New System.Drawing.Size(100, 30)
        Me.lCrop.TabIndex = 16
        Me.lCrop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourceImageSize
        '
        Me.lSourceImageSize.Location = New System.Drawing.Point(72, 98)
        Me.lSourceImageSize.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSourceImageSize.Name = "lSourceImageSize"
        Me.lSourceImageSize.Size = New System.Drawing.Size(100, 30)
        Me.lSourceImageSize.TabIndex = 35
        Me.lSourceImageSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbSourceLength
        '
        Me.lbSourceLength.Location = New System.Drawing.Point(72, 67)
        Me.lbSourceLength.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbSourceLength.Name = "lbSourceLength"
        Me.lbSourceLength.Size = New System.Drawing.Size(100, 30)
        Me.lbSourceLength.TabIndex = 42
        Me.lbSourceLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourceFramerate
        '
        Me.lSourceFramerate.Location = New System.Drawing.Point(229, 67)
        Me.lSourceFramerate.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSourceFramerate.Name = "lSourceFramerate"
        Me.lSourceFramerate.Size = New System.Drawing.Size(77, 30)
        Me.lSourceFramerate.TabIndex = 40
        Me.lSourceFramerate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourceDar
        '
        Me.lSourceDar.Location = New System.Drawing.Point(229, 98)
        Me.lSourceDar.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSourceDar.Name = "lSourceDar"
        Me.lSourceDar.Size = New System.Drawing.Size(77, 30)
        Me.lSourceDar.TabIndex = 50
        Me.lSourceDar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourceDarText
        '
        Me.lSourceDarText.Location = New System.Drawing.Point(173, 98)
        Me.lSourceDarText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSourceDarText.Name = "lSourceDarText"
        Me.lSourceDarText.Size = New System.Drawing.Size(55, 30)
        Me.lSourceDarText.TabIndex = 49
        Me.lSourceDarText.TabStop = True
        Me.lSourceDarText.Text = "DAR:"
        Me.lSourceDarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourcePAR
        '
        Me.lSourcePAR.Location = New System.Drawing.Point(229, 129)
        Me.lSourcePAR.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSourcePAR.Name = "lSourcePAR"
        Me.lSourcePAR.Size = New System.Drawing.Size(77, 30)
        Me.lSourcePAR.TabIndex = 47
        Me.lSourcePAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'llSourceParText
        '
        Me.llSourceParText.Location = New System.Drawing.Point(173, 129)
        Me.llSourceParText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llSourceParText.Name = "llSourceParText"
        Me.llSourceParText.Size = New System.Drawing.Size(55, 30)
        Me.llSourceParText.TabIndex = 45
        Me.llSourceParText.TabStop = True
        Me.llSourceParText.Text = "PAR:"
        Me.llSourceParText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lCropText
        '
        Me.lCropText.Location = New System.Drawing.Point(1, 129)
        Me.lCropText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lCropText.Name = "lCropText"
        Me.lCropText.Size = New System.Drawing.Size(70, 30)
        Me.lCropText.TabIndex = 14
        Me.lCropText.TabStop = True
        Me.lCropText.Text = "Crop:"
        Me.lCropText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourceImageSizeText
        '
        Me.lSourceImageSizeText.Location = New System.Drawing.Point(1, 98)
        Me.lSourceImageSizeText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSourceImageSizeText.Name = "lSourceImageSizeText"
        Me.lSourceImageSizeText.Size = New System.Drawing.Size(70, 30)
        Me.lSourceImageSizeText.TabIndex = 33
        Me.lSourceImageSizeText.Text = "Size:"
        Me.lSourceImageSizeText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lbSourceLengthText
        '
        Me.lbSourceLengthText.Location = New System.Drawing.Point(1, 67)
        Me.lbSourceLengthText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbSourceLengthText.Name = "lbSourceLengthText"
        Me.lbSourceLengthText.Size = New System.Drawing.Size(70, 30)
        Me.lbSourceLengthText.TabIndex = 41
        Me.lbSourceLengthText.Text = "Length:"
        Me.lbSourceLengthText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourceFramerateText
        '
        Me.lSourceFramerateText.Location = New System.Drawing.Point(173, 67)
        Me.lSourceFramerateText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSourceFramerateText.Name = "lSourceFramerateText"
        Me.lSourceFramerateText.Size = New System.Drawing.Size(55, 30)
        Me.lSourceFramerateText.TabIndex = 39
        Me.lSourceFramerateText.Text = "FPS:"
        Me.lSourceFramerateText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSourceFile
        '
        Me.tbSourceFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSourceFile.Location = New System.Drawing.Point(8, 26)
        Me.tbSourceFile.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbSourceFile.Size = New System.Drawing.Size(471, 31)
        '
        'lgbResize
        '
        Me.lgbResize.Controls.Add(Me.lPAR)
        Me.lgbResize.Controls.Add(Me.lAspectRatioError)
        Me.lgbResize.Controls.Add(Me.lZoom)
        Me.lgbResize.Controls.Add(Me.lPixel)
        Me.lgbResize.Controls.Add(Me.lSarText)
        Me.lgbResize.Controls.Add(Me.lAspectRatioErrorText)
        Me.lgbResize.Controls.Add(Me.lParText)
        Me.lgbResize.Controls.Add(Me.lPixelText)
        Me.lgbResize.Controls.Add(Me.lZoomText)
        Me.lgbResize.Controls.Add(Me.lDarText)
        Me.lgbResize.Controls.Add(Me.lSAR)
        Me.lgbResize.Controls.Add(Me.lDAR)
        Me.lgbResize.Controls.Add(Me.tbResize)
        Me.lgbResize.Controls.Add(Me.lTargetWidth)
        Me.lgbResize.Controls.Add(Me.tbTargetWidth)
        Me.lgbResize.Controls.Add(Me.tbTargetHeight)
        Me.lgbResize.Controls.Add(Me.lTargetHeight)
        Me.lgbResize.Location = New System.Drawing.Point(342, 220)
        Me.lgbResize.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbResize.Name = "lgbResize"
        Me.lgbResize.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbResize.Size = New System.Drawing.Size(320, 216)
        Me.lgbResize.TabIndex = 55
        Me.lgbResize.TabStop = False
        Me.lgbResize.Text = "Resize"
        '
        'lPAR
        '
        Me.lPAR.Location = New System.Drawing.Point(57, 175)
        Me.lPAR.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lPAR.Size = New System.Drawing.Size(81, 30)
        Me.lPAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lAspectRatioError
        '
        Me.lAspectRatioError.Location = New System.Drawing.Point(206, 175)
        Me.lAspectRatioError.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lAspectRatioError.Size = New System.Drawing.Size(110, 30)
        Me.lAspectRatioError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lZoom
        '
        Me.lZoom.Location = New System.Drawing.Point(206, 144)
        Me.lZoom.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lZoom.Name = "lZoom"
        Me.lZoom.Size = New System.Drawing.Size(110, 30)
        Me.lZoom.TabIndex = 44
        Me.lZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPixel
        '
        Me.lPixel.Location = New System.Drawing.Point(206, 113)
        Me.lPixel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lPixel.Name = "lPixel"
        Me.lPixel.Size = New System.Drawing.Size(110, 30)
        Me.lPixel.TabIndex = 50
        Me.lPixel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSarText
        '
        Me.lSarText.Location = New System.Drawing.Point(4, 144)
        Me.lSarText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSarText.Size = New System.Drawing.Size(52, 30)
        Me.lSarText.Text = "SAR:"
        Me.lSarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lAspectRatioErrorText
        '
        Me.lAspectRatioErrorText.Location = New System.Drawing.Point(139, 175)
        Me.lAspectRatioErrorText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lAspectRatioErrorText.Size = New System.Drawing.Size(66, 30)
        Me.lAspectRatioErrorText.Text = "Error:"
        Me.lAspectRatioErrorText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lParText
        '
        Me.lParText.Location = New System.Drawing.Point(4, 175)
        Me.lParText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lParText.Size = New System.Drawing.Size(52, 30)
        Me.lParText.Text = "PAR:"
        Me.lParText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPixelText
        '
        Me.lPixelText.Location = New System.Drawing.Point(139, 113)
        Me.lPixelText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lPixelText.Name = "lPixelText"
        Me.lPixelText.Size = New System.Drawing.Size(66, 30)
        Me.lPixelText.TabIndex = 49
        Me.lPixelText.Text = "Pixel:"
        Me.lPixelText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lZoomText
        '
        Me.lZoomText.Location = New System.Drawing.Point(139, 144)
        Me.lZoomText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lZoomText.Name = "lZoomText"
        Me.lZoomText.Size = New System.Drawing.Size(66, 30)
        Me.lZoomText.TabIndex = 42
        Me.lZoomText.Text = "Zoom:"
        Me.lZoomText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lDarText
        '
        Me.lDarText.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lDarText.Location = New System.Drawing.Point(4, 113)
        Me.lDarText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lDarText.Name = "lDarText"
        Me.lDarText.Size = New System.Drawing.Size(52, 30)
        Me.lDarText.TabIndex = 23
        Me.lDarText.Text = "DAR:"
        Me.lDarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSAR
        '
        Me.lSAR.Location = New System.Drawing.Point(57, 144)
        Me.lSAR.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSAR.Size = New System.Drawing.Size(81, 30)
        Me.lSAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lDAR
        '
        Me.lDAR.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lDAR.Location = New System.Drawing.Point(57, 113)
        Me.lDAR.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lDAR.Name = "lDAR"
        Me.lDAR.Size = New System.Drawing.Size(81, 30)
        Me.lDAR.TabIndex = 24
        Me.lDAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbResize
        '
        Me.tbResize.AutoSize = False
        Me.tbResize.BackColor = System.Drawing.SystemColors.Control
        Me.tbResize.Location = New System.Drawing.Point(8, 70)
        Me.tbResize.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbResize.Name = "tbResize"
        Me.tbResize.Size = New System.Drawing.Size(304, 34)
        Me.tbResize.TabIndex = 46
        '
        'lTargetWidth
        '
        Me.lTargetWidth.AutoSize = True
        Me.lTargetWidth.Location = New System.Drawing.Point(12, 36)
        Me.lTargetWidth.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lTargetWidth.Name = "lTargetWidth"
        Me.lTargetWidth.Size = New System.Drawing.Size(64, 25)
        Me.lTargetWidth.TabIndex = 37
        Me.lTargetWidth.Text = "Width:"
        '
        'tbTargetWidth
        '
        Me.tbTargetWidth.Location = New System.Drawing.Point(82, 33)
        Me.tbTargetWidth.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbTargetWidth.Name = "tbTargetWidth"
        Me.tbTargetWidth.Size = New System.Drawing.Size(70, 31)
        Me.tbTargetWidth.TabIndex = 39
        Me.tbTargetWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbTargetHeight
        '
        Me.tbTargetHeight.Location = New System.Drawing.Point(234, 33)
        Me.tbTargetHeight.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbTargetHeight.Name = "tbTargetHeight"
        Me.tbTargetHeight.Size = New System.Drawing.Size(70, 31)
        Me.tbTargetHeight.TabIndex = 40
        Me.tbTargetHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lTargetHeight
        '
        Me.lTargetHeight.AutoSize = True
        Me.lTargetHeight.Location = New System.Drawing.Point(159, 36)
        Me.lTargetHeight.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lTargetHeight.Name = "lTargetHeight"
        Me.lTargetHeight.Size = New System.Drawing.Size(69, 25)
        Me.lTargetHeight.TabIndex = 38
        Me.lTargetHeight.Text = "Height:"
        '
        'lgbFilters
        '
        Me.lgbFilters.Controls.Add(Me.AviSynthListView)
        Me.lgbFilters.Location = New System.Drawing.Point(13, 220)
        Me.lgbFilters.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbFilters.Name = "lgbFilters"
        Me.lgbFilters.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbFilters.Size = New System.Drawing.Size(320, 216)
        Me.lgbFilters.TabIndex = 53
        Me.lgbFilters.TabStop = False
        Me.lgbFilters.Text = "Filters"
        '
        'AviSynthListView
        '
        Me.AviSynthListView.AllowDrop = True
        Me.AviSynthListView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AviSynthListView.CheckBoxes = True
        Me.AviSynthListView.DoubleClickDoesCheck = False
        Me.AviSynthListView.FullRowSelect = True
        Me.AviSynthListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.AviSynthListView.HideSelection = False
        Me.AviSynthListView.Location = New System.Drawing.Point(8, 26)
        Me.AviSynthListView.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.AviSynthListView.MultiSelect = False
        Me.AviSynthListView.Name = "AviSynthListView"
        Me.AviSynthListView.Size = New System.Drawing.Size(304, 180)
        Me.AviSynthListView.TabIndex = 0
        Me.AviSynthListView.UseCompatibleStateImageBehavior = False
        Me.AviSynthListView.View = System.Windows.Forms.View.Details
        '
        'lgbEncoder
        '
        Me.lgbEncoder.Controls.Add(Me.llMuxer)
        Me.lgbEncoder.Controls.Add(Me.pEncoder)
        Me.lgbEncoder.Location = New System.Drawing.Point(671, 220)
        Me.lgbEncoder.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbEncoder.Name = "lgbEncoder"
        Me.lgbEncoder.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbEncoder.Size = New System.Drawing.Size(320, 216)
        Me.lgbEncoder.TabIndex = 51
        Me.lgbEncoder.TabStop = False
        Me.lgbEncoder.Text = "Encoder"
        '
        'llMuxer
        '
        Me.llMuxer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llMuxer.AutoSize = True
        Me.llMuxer.Location = New System.Drawing.Point(259, 0)
        Me.llMuxer.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llMuxer.Name = "llMuxer"
        Me.llMuxer.Size = New System.Drawing.Size(46, 25)
        Me.llMuxer.TabIndex = 1
        Me.llMuxer.TabStop = True
        Me.llMuxer.Text = "Mux"
        '
        'pEncoder
        '
        Me.pEncoder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pEncoder.Location = New System.Drawing.Point(8, 27)
        Me.pEncoder.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pEncoder.Name = "pEncoder"
        Me.pEncoder.Size = New System.Drawing.Size(305, 179)
        Me.pEncoder.TabIndex = 0
        '
        'MainForm
        '
        Me.AllowDrop = True
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1004, 654)
        Me.Controls.Add(Me.gbAudio)
        Me.Controls.Add(Me.lgbTarget)
        Me.Controls.Add(Me.lgbSource)
        Me.Controls.Add(Me.lgbResize)
        Me.Controls.Add(Me.lgbFilters)
        Me.Controls.Add(Me.lgbEncoder)
        Me.Controls.Add(Me.gbAssistant)
        Me.Controls.Add(Me.MenuStrip)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.MainMenuStrip = Me.MenuStrip
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "StaxRip"
        Me.gbAssistant.ResumeLayout(False)
        Me.gbAudio.ResumeLayout(False)
        Me.gbAudio.PerformLayout()
        Me.lgbTarget.ResumeLayout(False)
        Me.lgbTarget.PerformLayout()
        Me.lgbSource.ResumeLayout(False)
        Me.lgbSource.PerformLayout()
        Me.lgbResize.ResumeLayout(False)
        Me.lgbResize.PerformLayout()
        CType(Me.tbResize, System.ComponentModel.ISupportInitialize).EndInit()
        Me.lgbFilters.ResumeLayout(False)
        Me.lgbFilters.PerformLayout()
        Me.lgbEncoder.ResumeLayout(False)
        Me.lgbEncoder.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public BlockSubtitlesItemCheck As Boolean
    Public WithEvents CustomMainMenu As CustomMenu
    Public WithEvents CustomSizeMenu As CustomMenu
    Public CurrentAssistantTipKey As String
    Public AssistantPassed As Boolean

    Private AudioMenu0 As ContextMenuStrip
    Private AudioMenu1 As ContextMenuStrip
    Private TargetAspectRatioMenu As ContextMenuStrip
    Private SizeContextMenuStrip As ContextMenuStrip
    Private EncoderMenu As ContextMenuStrip
    Private ContainerMenu As ContextMenuStrip
    Private SourceAspectRatioMenu As ContextMenuStrip
    Private TargetFileMenu As ContextMenuStrip
    Private SourceFileMenu As ContextMenuStrip
    Private Audio0FileMenu As ContextMenuStrip
    Private Audio1FileMenu As ContextMenuStrip
    Private ResetAssistantFont As Boolean

    Private BlockAviSynthItemCheck As Boolean
    Private CanChangeSize As Boolean = True
    Private CanChangeBitrate As Boolean = True
    Private CanIgnoreTip As Boolean = True
    Private IsLoading As Boolean = True
    Private CommandManager As New CommandManager
    Private BlockBitrate, BlockSize As Boolean
    Private IsManualCheckForUpdates As Boolean
    Private SkipAssistant As Boolean
    Private BlockSourceTextBoxTextChanged As Boolean

    Sub New()
        MyBase.New()

        Try
            g.MainForm = Me

            AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf g.OnUnhandledException
            AddHandler Application.ThreadException, AddressOf g.OnUnhandledException

            Paths.CheckIfSettingsDirIsWriteable()

            s = DirectCast(SafeSerialization.Deserialize(New ApplicationSettings, Paths.SettingsFile, New LegacySerializationBinder), ApplicationSettings)

            MenuItemEx.UseTooltips = s.EnableTooltips

            InitializeComponent()

            If components Is Nothing Then
                components = New System.ComponentModel.Container
            End If

            SetTip()

            TargetAspectRatioMenu = New ContextMenuStrip(components)
            SizeContextMenuStrip = New ContextMenuStrip(components)
            EncoderMenu = New ContextMenuStrip(components)
            ContainerMenu = New ContextMenuStrip(components)
            SourceAspectRatioMenu = New ContextMenuStrip(components)
            TargetFileMenu = New ContextMenuStrip(components)
            SourceFileMenu = New ContextMenuStrip(components)
            Audio0FileMenu = New ContextMenuStrip(components)
            Audio1FileMenu = New ContextMenuStrip(components)

            tbTargetFile.ContextMenuStrip = TargetFileMenu
            tbSourceFile.ContextMenuStrip = SourceFileMenu
            tbAudioFile0.ContextMenuStrip = Audio0FileMenu
            tbAudioFile1.ContextMenuStrip = Audio1FileMenu

            AudioMenu0 = New ContextMenuStrip
            AudioMenu1 = New ContextMenuStrip

            Dim rc = "right-click"
            tbAudioFile0.SendMessageCue(rc, False)
            tbAudioFile1.SendMessageCue(rc, False)
            tbSourceFile.SendMessageCue(rc, False)
            tbTargetFile.SendMessageCue(rc, False)

            llEditAudio0.AddClickAction(AddressOf AudioEdit0ToolStripMenuItemClick)
            llEditAudio1.AddClickAction(AddressOf AudioEdit1ToolStripMenuItemClick)

            Icon = My.Resources.MainIcon

            AviSynthListView.ProfileFunc = AddressOf GetTargetAviSynthDocument

            MenuStrip.SuspendLayout()

            CommandManager.AddCommandsFromObject(Me)
            CommandManager.AddCommandsFromObject(g.DefaultCommands)

            CustomMainMenu = New CustomMenu(AddressOf GetDefaultMainMenu,
                s.CustomMenuMainForm, CommandManager, MenuStrip)

            CustomMainMenu.AddKeyDownHandler(Me)
            CustomMainMenu.BuildMenu()

            LoadMainMenuDynamic()
            UpdateAudioMenu()
            MenuStrip.ResumeLayout()
            OpenProject(Paths.StartupTemplatePath)
            SizeContextMenuStrip.SuspendLayout()

            CustomSizeMenu = New CustomMenu(AddressOf GetDefaultMenuSize,
                s.CustomMenuSize, CommandManager, SizeContextMenuStrip)

            CustomSizeMenu.AddKeyDownHandler(Me)
            CustomSizeMenu.BuildMenu()

            SizeContextMenuStrip.ResumeLayout()

            g.SetRenderer(MenuStrip)
            SetMenuStyle()
        Catch ex As Exception
            VB6.MsgBox(ex.ToString, Microsoft.VisualBasic.MsgBoxStyle.Critical)
        End Try
    End Sub

    Sub SetMenuStyle()
        If ToolStripRendererEx.IsAutoRenderMode Then
            Dim c = ControlPaint.Dark(ToolStripRendererEx.ColorBorder, 0)

            llAudioProfile0.LinkColor = c
            llAudioProfile1.LinkColor = c
            llEditAudio0.LinkColor = c
            llEditAudio1.LinkColor = c
            llFilesize.LinkColor = c
            llMuxer.LinkColor = c
            llSourceParText.LinkColor = c

            lgbEncoder.ll.LinkColor = c
            lgbFilters.ll.LinkColor = c
            lgbResize.ll.LinkColor = c
            lgbSource.ll.LinkColor = c
            lgbTarget.ll.LinkColor = c
        Else
            llAudioProfile0.LinkColor = Color.Blue
            llAudioProfile1.LinkColor = Color.Blue
            llEditAudio0.LinkColor = Color.Blue
            llEditAudio1.LinkColor = Color.Blue
            llFilesize.LinkColor = Color.Blue
            llMuxer.LinkColor = Color.Blue
            llSourceParText.LinkColor = Color.Blue

            lgbEncoder.ll.LinkColor = Color.Blue
            lgbFilters.ll.LinkColor = Color.Blue
            lgbResize.ll.LinkColor = Color.Blue
            lgbSource.ll.LinkColor = Color.Blue
            lgbTarget.ll.LinkColor = Color.Blue
        End If
    End Sub

    Function GetIfoFile() As String
        Dim ret = Filepath.GetDir(p.SourceFile)
        If ret.EndsWith(" temp files\") Then ret = DirPath.GetParent(ret)
        ret = ret + Filepath.GetName(p.SourceFile).DeleteRight(5) + "0.ifo"
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
                                Dim base = Filepath.GetBase(i)
                                base = regex.Replace(base, " ID" & x & " ")

                                If base.Contains("2_0ch") Then base = base.Replace("2_0ch", "2ch")
                                If base.Contains("3_2ch") Then base = base.Replace("3_2ch", "6ch")
                                If base.Contains(" DELAY") Then base = base.Replace(" DELAY", "")
                                If base.Contains(" 0ms") Then base = base.Replace(" 0ms", "")

                                If stream.Language.TwoLetterCode <> "iv" Then
                                    base += " " + stream.Language.Name
                                End If
                                If stream.Title <> "" Then base += " " + stream.Title
                                FileHelp.Move(i, Filepath.GetDir(i) + base + Filepath.GetExtFull(i))
                            End If
                        Next
                    End Using
                End If
            End If
        Next
    End Sub

    Private Sub DetectAudioFiles(track As Integer,
                                 lang As Boolean,
                                 same As Boolean)

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
        files.Sort()

        For Each iExt In FileTypes.Audio
            If iExt = "avs" Then
                Continue For
            End If

            For Each iPath In files
                If tbOther.Text = iPath Then
                    Continue For
                End If

                If Not Filepath.GetExt(iPath) = iExt Then
                    Continue For
                End If

                If iPath.Contains("_cut_") Then
                    Continue For
                End If

                If Not g.IsSourceSame(iPath) Then
                    Continue For
                End If

                If same AndAlso tbOther.Text <> "" AndAlso Filepath.GetExtFull(tbOther.Text) <> Filepath.GetExtFull(iPath) Then
                    Continue For
                End If

                If lang Then
                    Dim lng = profile.Language

                    If profile.Language.ThreeLetterCode = "und" Then
                        lng = If(track = 0, New Language(), New Language("en"))
                    End If

                    If Not iPath.Contains(lng.Name) Then Continue For
                End If

                If Filepath.GetExtFull(iPath) = ".mp4" AndAlso Filepath.IsSameBase(p.SourceFile, iPath) Then
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
        'ObjectHelp.GetCompareString(g.SavedProject).WriteFile(CommonDirs.UserDesktop + "test1.txt")
        'ObjectHelp.GetCompareString(p).WriteFile(CommonDirs.UserDesktop + "test2.txt")

        If ObjectHelp.GetCompareString(g.SavedProject) <> ObjectHelp.GetCompareString(p) Then
            If Environment.OSVersion.Version.Major >= 6 Then
                Using td As New TaskDialog(Of DialogResult)
                    td.MainInstruction = "Save changed project?"
                    td.AddButton(DialogResult.Yes, "Save")
                    td.AddButton(DialogResult.No, "Don't Save")
                    td.AddButton(DialogResult.Cancel, "Cancel")
                    td.Show()
                    Refresh()

                    If td.SelectedValue = DialogResult.Yes Then
                        If g.ProjectPath Is Nothing Then
                            If Not ShowSaveDialog() Then
                                Return True
                            End If
                        Else
                            SaveProjectByPath(g.ProjectPath)
                        End If
                    ElseIf td.SelectedValue = DialogResult.Cancel Then
                        Return True
                    End If
                End Using
            Else
                Dim result = MsgQuestion("Save changed project?", MessageBoxButtons.YesNoCancel)
                Refresh()

                Select Case result
                    Case DialogResult.Yes
                        If g.ProjectPath Is Nothing Then
                            If Not ShowSaveDialog() Then
                                Return True
                            End If
                        Else
                            SaveProjectByPath(g.ProjectPath)
                        End If
                    Case DialogResult.Cancel
                        Return True
                End Select
            End If
        End If
    End Function

    Sub UpdateRecentProjectsMenuItems()
        UpdateRecentProjectsMenuItems(Nothing)
    End Sub

    Async Sub UpdateRecentProjectsMenuItems(path As String)
        Await Task.Run(Sub()
                           Dim list As New List(Of String)

                           If path <> "" AndAlso Not path.Contains(Paths.TemplateDir) AndAlso
                               Not path.Contains("crash.srip") Then

                               list.Add(path)
                           End If

                           For Each i In s.RecentProjects
                               If i <> path AndAlso File.Exists(i) Then
                                   list.Add(i)
                               End If
                           Next

                           While list.Count > 9
                               list.RemoveAt(list.Count - 1)
                           End While

                           SyncLock s.RecentProjects
                               s.RecentProjects = list
                           End SyncLock
                       End Sub)

        For Each i In CustomMainMenu.MenuItems
            If i.CustomMenuItem.MethodName = "DynamicMenuItem" AndAlso
                i.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.RecentProjects) Then

                i.DropDownItems.Clear()

                SyncLock s.RecentProjects
                    For Each i2 In s.RecentProjects
                        If File.Exists(i2) AndAlso Not Filepath.GetBase(i2) = "recover" Then
                            Dim name = i2

                            If i2.Length > 70 Then
                                name = "..." + name.Remove(0, name.Length - 70)
                            End If

                            i.DropDownItems.Add(New ActionMenuItem(name, Sub() LoadProject(i2)))
                        End If
                    Next
                End SyncLock

                Exit For
            End If
        Next
    End Sub

    Sub UpdateDynamicMenu()
        For Each i In CustomMainMenu.MenuItems
            If i.CustomMenuItem.MethodName = "DynamicMenuItem" Then
                If i.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.LaunchApplications) Then
                    i.DropDownItems.Clear()

                    Dim sd As New SortedDictionary(Of String, Action)

                    For Each i2 In Packs.Packages.Values
                        If Not i2.LaunchAction Is Nothing AndAlso Not i2.IsStatusCritical Then
                            sd(i2.LaunchTitle) = i2.LaunchAction
                        End If
                    Next

                    sd("mkvinfo GUI") = Sub() g.ShellExecute(Packs.Mkvmerge.GetDir + "mkvinfo.exe", "-g")

                    For Each i2 In sd
                        i.DropDownItems.Add(New ActionMenuItem(i2.Key, i2.Value))
                    Next
                End If

                If i.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.HelpApplications) Then
                    i.DropDownItems.Clear()

                    For Each i2 In Packs.Packages.Values
                        Dim helpPath = i2.GetHelpPath

                        If helpPath <> "" Then
                            ActionMenuItem.Add(i.DropDownItems, i2.TreePath + " | " + i2.Name,
                                               Sub() g.ShellExecute(helpPath))
                        End If
                    Next
                End If
            End If
        Next
    End Sub

    Sub UpdateTemplateProjectsMenuItems()
        For Each i In CustomMainMenu.MenuItems
            If i.CustomMenuItem.MethodName = "DynamicMenuItem" AndAlso
                i.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.TemplateProjects) Then

                i.DropDownItems.Clear()

                For Each i2 In Directory.GetFiles(Paths.TemplateDir, "*.srip.backup")
                    FileHelp.Move(i2, Filepath.GetDir(i2) + "Backup\" + Filepath.GetBase(i2))
                Next

                For Each i2 In Directory.GetFiles(Paths.TemplateDir, "*.srip", SearchOption.AllDirectories)
                    Dim base = Filepath.GetBase(i2)

                    If i2 = Paths.StartupTemplatePath Then
                        base += " (Startup)"
                    End If

                    If i2.Contains("Backup\") Then
                        base = "Backup | " + base
                    End If

                    ActionMenuItem.Add(i.DropDownItems, base, AddressOf LoadProject, i2, Nothing)
                Next

                i.DropDownItems.Add("-")
                ActionMenuItem.Add(i.DropDownItems, "Explore", Sub() g.ShellExecute(Paths.TemplateDir), "Opens the directory containing the templates.")
                ActionMenuItem.Add(i.DropDownItems, "Restore", AddressOf ResetTemplates, "Restores the default templates.")

                Exit For
            End If
        Next
    End Sub

    Sub ResetTemplates()
        If MsgQuestion("Restore the default templates?") = DialogResult.OK Then
            Try
                DirectoryHelp.Delete(Paths.TemplateDir)
                Paths.TemplateDir.ToString()
                UpdateTemplateProjectsMenuItems()
            Catch ex As Exception
                g.ShowException(ex)
            End Try
        End If
    End Sub

    <Command("Perform | Load Template", "Loads a template.", Switch:="template|t")>
    Sub LoadTemplate(name As String)
        LoadProject(Paths.TemplateDir + name + ".srip")
    End Sub

    Sub LoadProject(path As String)
        Refresh()

        If Not File.Exists(path) Then
            MsgWarn("Project file not found.")
            UpdateTemplateProjectsMenuItems()
            UpdateRecentProjectsMenuItems()
        Else
            OpenProject(path)
        End If
    End Sub

    Function ShowSaveDialog() As Boolean
        Using d As New SWF.SaveFileDialog
            d.SetInitDir(p.TempDir)

            If p.SourceFile <> "" Then
                d.FileName = p.Name
            Else
                d.FileName = "Untitled"
            End If

            d.Filter = "StaxRip Project Files (*.srip)|*.srip"

            If d.ShowDialog() = DialogResult.OK Then
                If Not d.FileName.ToLower.EndsWith(".srip") Then
                    d.FileName += ".srip"
                End If

                SaveProjectByPath(d.FileName)
                Return True
            End If
        End Using
    End Function

    Function OpenProject(path As String) As Boolean
        Return OpenProject(path, True)
    End Function

    Function OpenProject(path As String, saveCurrent As Boolean) As Boolean
        If Not IsLoading AndAlso saveCurrent AndAlso IsSaveCanceled() Then
            Return False
        End If

        If Not File.Exists(path) Then path = Paths.StartupTemplatePath

        Dim safeInstance = New Project
        safeInstance.Init()

        p = DirectCast(SafeSerialization.Deserialize(safeInstance, path, New LegacySerializationBinder), Project)

        Text = Application.ProductName + " x64 - " + Filepath.GetBase(path)
        SkipAssistant = True

        If path.StartsWith(Paths.TemplateDir) Then
            g.ProjectPath = Nothing
            p.TemplateName = Filepath.GetBase(path)
        Else
            g.ProjectPath = path
        End If

        g.SetTempDir()

        Dim width = p.TargetWidth
        Dim height = p.TargetHeight
        Dim size = p.Size
        Dim bitrate = p.VideoBitrate

        SetTargetLength(p.TargetSeconds)

        AviSynthListView.Load()
        AviSynthListView.Items(0).Selected = True
        p.VideoEncoder.OnStateChange()

        Dim targetPath = p.TargetFile

        Dim audio0 = p.Audio0.File
        Dim audio1 = p.Audio1.File

        Dim delay0 = p.Audio0.Delay
        Dim delay1 = p.Audio1.Delay

        BlockSourceTextBoxTextChanged = True
        tbSourceFile.Text = p.SourceFile
        BlockSourceTextBoxTextChanged = False

        s.LastSourceDir = Filepath.GetDir(p.SourceFile)

        tbAudioFile0.Text = audio0
        tbAudioFile1.Text = audio1

        llAudioProfile0.Text = g.ConvertPath(p.Audio0.Name)
        llAudioProfile1.Text = g.ConvertPath(p.Audio1.Name)

        p.Audio0.Delay = delay0
        p.Audio1.Delay = delay1

        tbSize.Text = size.ToString()
        SetSlider()

        tbTargetWidth.Text = width.ToString
        tbTargetHeight.Text = height.ToString
        tbTargetFile.Text = targetPath

        SetSavedProject()

        SkipAssistant = False

        Assistant()
        UpdateRecentProjectsMenuItems(path)
        g.RaiseApplicationEvent(ApplicationEvent.ProjectLoaded)
        g.RaiseApplicationEvent(ApplicationEvent.ProjectOrSourceLoaded)

        Return True
    End Function

    Sub SetSlider()
        Dim w = If(p.ResizeSliderMaxWidth = 0, p.SourceWidth, p.ResizeSliderMaxWidth)
        tbResize.Maximum = CInt((Calc.FixMod16(w) - 320) / p.ForcedOutputMod)
    End Sub

    Sub SetSavedProject()
        g.SavedProject = StaxRip.ObjectHelp.GetCopy(Of Project)(p)
    End Sub

    Sub FormMain_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        Dim a = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If OK(a) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Sub FormMain_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If OK(files) Then
            Activate()
            BeginInvoke(Sub() OpenAnyFile(files.ToList, False))
        End If
    End Sub

    Sub OpenSingleFile(files As IEnumerable(Of String))
        Dim filter As AviSynthFilter

        If files.Count = 1 Then
            Dim td As New TaskDialog(Of String)
            td.MainInstruction = "Choose a preferred source filter"
            td.Content = "A description of the available source filters can be found in the [http://github.com/stax76/staxrip/wiki/Source-Filters wiki]."
            td.AddCommandLink("Automatic", "Automatic")

            If FileTypes.DGDecNVInput.Contains(files(0).Ext) Then
                If Packs.DGDecodeNV.GetPath <> "" Then td.AddCommandLink("DGSource", "DGSource")
                If Packs.DGDecodeIM.GetPath <> "" Then td.AddCommandLink("DGSourceIM", "DGSourceIM")
            End If

            td.AddCommandLink("FFVideoSource", "FFVideoSource")
            td.AddCommandLink("LWLibavVideoSource", "LWLibavVideoSource")

            If {"mp4", "m4v", "mov"}.Contains(files(0).Ext) Then
                td.AddCommandLink("LSMASHVideoSource", "LSMASHVideoSource")
            End If

            If g.IsCOMObjectRegistered(GUIDS.LAVSplitter) AndAlso
                g.IsCOMObjectRegistered(GUIDS.LAVVideoDecoder) Then

                td.AddCommandLink("DSS2", "DSS2")
            End If

            If files(0).Ext = "avi" Then td.AddCommandLink("AVISource", "AVISource")

            Select Case td.Show
                Case "Automatic"
                    filter = New AviSynthFilter("Source", "Automatic", "", True)
                Case "DGSource"
                    filter = New AviSynthFilter("Source", "DGSource", "DGSource(""%source_file%"")")
                Case "DGSourceIM"
                    filter = New AviSynthFilter("Source", "DGSourceIM", "DGSourceIM(""%source_file%"")")
                Case "FFVideoSource"
                    filter = New AviSynthFilter("Source", "FFVideoSource", "FFVideoSource(""%source_file%"", cachefile = ""%temp_file%.ffindex"")")
                Case "LWLibavVideoSource"
                    filter = New AviSynthFilter("Source", "LWLibavVideoSource", "LWLibavVideoSource(""%source_file%"")")
                Case "LSMASHVideoSource"
                    filter = New AviSynthFilter("Source", "LSMASHVideoSource", "LSMASHVideoSource(""%source_file%"")")
                Case "DSS2"
                    filter = New AviSynthFilter("Source", "DSS2", "DSS2(""%source_file%"")")
                Case "AVISource"
                    filter = New AviSynthFilter("Source", "AVISource", "AviSource(""%source_file%"", audio = false)")
            End Select
        End If

        OpenVideoSourceFiles(files, filter)
    End Sub

    Sub OpenAnyFile(files As IEnumerable(Of String), Optional silent As Boolean = True)
        If Filepath.GetExtFull(files(0)) = ".srip" Then
            OpenProject(files(0))
        ElseIf FileTypes.Video.Contains(Filepath.GetExt(files(0)).ToLower) Then
            files.Sort()

            If silent Then
                OpenVideoSourceFiles(files)
            Else
                OpenSingleFile(files)
            End If
        ElseIf FileTypes.Audio.Contains(Filepath.GetExt(files(0)).ToLower) Then
            tbAudioFile0.Text = files(0)
        Else
            files.Sort()
            OpenVideoSourceFiles(files)
        End If
    End Sub

    Sub OpenVideoSourceFile(fp As String)
        OpenVideoSourceFiles({fp})
    End Sub

    Sub OpenVideoSourceFiles(files As IEnumerable(Of String),
                             Optional preferredSourceFilter As AviSynthFilter = Nothing)

        OpenVideoSourceFiles(files, True, preferredSourceFilter)
    End Sub

    Sub OpenVideoSourceFiles(files As IEnumerable(Of String),
                             autoMode As Boolean,
                             Optional preferredSourceFilter As AviSynthFilter = Nothing)

        Dim recoverPath = g.ProjectPath
        Dim recoverProjectPath = CommonDirs.Temp + Guid.NewGuid.ToString + ".bin"
        Dim recoverText = Text

        SafeSerialization.Serialize(p, recoverProjectPath)
        AddHandler g.MainForm.Disposed, Sub() FileHelp.Delete(recoverProjectPath)

        Try
            If g.ShowVideoSourceWarnings(files) Then Throw New AbortException

            For Each i In files
                Dim name = Filepath.GetName(i)

                If name.ToUpper Like "VTS_0#_0.VOB" Then
                    If Msg("Are you sure you want to open the file " + name + "," + CrLf +
                           "the first VOB file usually contains a menu!", Nothing,
                           MessageBoxIcon.Question, MessageBoxButtons.YesNo,
                           DialogResult.No) <> DialogResult.Yes Then

                        Throw New AbortException
                    End If
                End If

                If name.ToUpper = "VIDEO_TS.VOB" Then
                    MsgWarn("The file VIDEO_TS.VOB can't be opened.")
                    Throw New AbortException
                End If
            Next

            If Not Paths.VerifyRequirements() Then Throw New AbortException

            If p.SourceFile <> "" AndAlso autoMode Then
                Dim templates = Directory.GetFiles(Paths.TemplateDir, "*.srip")

                If templates.Length = 1 Then
                    If Not OpenProject(templates(0), True) Then
                        Throw New AbortException
                    End If
                Else
                    If Not LoadTemplateWithSelectionDialog() Then
                        Throw New AbortException
                    End If
                End If
            End If

            If p.SourceFile <> "" AndAlso autoMode AndAlso
                Not LoadTemplateWithSelectionDialog() Then

                Throw New AbortException
            End If

            p.SourceFiles = files.ToList
            p.SourceFile = files(0)

            g.SetTempDir()

            p.NativeSourceFile = p.SourceFile
            p.OriginalSourceFile = p.SourceFile

            If FileTypes.VideoIndex.Contains(Filepath.GetExt(p.SourceFile)) Then
                For Each i In File.ReadAllLines(p.SourceFile)
                    If i.Contains(":\") Then
                        If Regex.IsMatch(i, "^.+ \d+$") Then
                            i = i.LeftLast(" ")
                        End If

                        If File.Exists(i) AndAlso FileTypes.Video.Contains(Filepath.GetExt(i)) Then
                            p.NativeSourceFile = i
                            p.OriginalSourceFile = i
                            Exit For
                        End If
                    End If
                Next
            ElseIf Filepath.GetExtFull(p.SourceFile) = ".avs" Then
                Dim code = File.ReadAllText(p.SourceFile)

                Dim match = Regex.Match(code, "source\(""(.+?)""", RegexOptions.IgnoreCase)

                If match.Success Then
                    Dim fp = match.Groups(1).Value

                    If File.Exists(fp) AndAlso FileTypes.Video.Contains(Filepath.GetExt(fp)) Then
                        p.NativeSourceFile = fp
                        p.OriginalSourceFile = fp
                    End If
                End If
            End If

            Dim sourcePAR = MediaInfo.GetVideo(p.NativeSourceFile, "PixelAspectRatio")

            If sourcePAR <> "" Then
                p.SourcePAR.X = CInt(Convert.ToSingle(sourcePAR, CultureInfo.InvariantCulture) * 1000)
                p.SourcePAR.Y = 1000
            End If

            p.Codec = MediaInfo.GetVideoCodec(p.NativeSourceFile)

            p.CodecProfile = MediaInfo.GetVideo(p.NativeSourceFile, "Format_Profile")

            If autoMode AndAlso p.BatchMode Then
                Assistant()
                Exit Sub
            End If

            Log.WriteEnvironment()

            Dim targetDir As String = Nothing

            If p.DefaultTargetFolder <> "" Then
                targetDir = DirPath.AppendSeparator(Macro.Solve(p.DefaultTargetFolder)).Replace("\\", "\")

                If Not Directory.Exists(targetDir) Then
                    Try
                        Directory.CreateDirectory(targetDir)
                    Catch ex As Exception
                    End Try
                End If
            End If

            If Not Directory.Exists(targetDir) Then
                targetDir = Filepath.GetDir(p.SourceFile)
            End If

            Dim targetName = Macro.Solve(p.DefaultTargetName)

            If Not Filepath.IsValidFileSystemName(targetName) Then
                targetName = Filepath.GetBase(p.SourceFile)
            End If

            tbTargetFile.Text = targetDir + targetName + p.VideoEncoder.Muxer.GetExtension

            If p.SourceFile = p.TargetFile OrElse
                (FileTypes.VideoIndex.Contains(Filepath.GetExtFull(p.SourceFile)) AndAlso
                File.ReadAllText(p.SourceFile).Contains(p.TargetFile)) Then

                tbTargetFile.Text = Filepath.GetDirAndBase(p.TargetFile) + "_new" + Filepath.GetExtFull(p.TargetFile)
            End If

            Log.WriteHeader("Source file MediaInfo")

            For Each i In p.SourceFiles
                Log.WriteLine(i)
            Next

            Log.WriteLine(CrLf + MediaInfo.GetSummary(p.SourceFile))

            For Each i In DriveInfo.GetDrives()
                If i.DriveType = DriveType.CDRom AndAlso
                    p.TempDir.ToUpper.StartsWith(i.RootDirectory.ToString.ToUpper) Then

                    MsgWarn("Opening files from a CD/DVD drive requires to set a temp files directory in the option.")
                    Throw New AbortException
                End If
            Next

            If Not preferredSourceFilter Is Nothing Then
                p.AvsDoc.SetFilter(preferredSourceFilter.Category,
                                   preferredSourceFilter.Name,
                                   preferredSourceFilter.Script)
            End If

            Demux()

            If Filepath.GetExt(p.SourceFile) = "dgi" AndAlso Not Packs.DGDecodeNV.VerifyOK(True) Then
                Throw New AbortException
            End If

            If p.NativeSourceFile <> p.SourceFile AndAlso
                Not FileTypes.VideoText.Contains(Filepath.GetExt(p.SourceFile)) Then

                p.NativeSourceFile = p.SourceFile
            End If

            s.LastSourceDir = Filepath.GetDir(p.SourceFile)

            Dim sourceFilter = p.AvsDoc.GetFilter("Source")

            If Not sourceFilter.Script.Contains("(") Then
                If Filepath.GetExt(p.SourceFile) = "avs" Then
                    p.AvsDoc.Filters.Clear()
                    p.AvsDoc.Filters.Add(New AviSynthFilter("Source", "AVS Import", File.ReadAllText(p.SourceFile), True))
                Else
                    For Each i In s.FilterPreferences
                        Dim name = i.Name.SplitNoEmptyAndWhiteSpace({",", " "})

                        If name.Contains(Filepath.GetExt(p.SourceFile)) Then
                            Dim filters = s.AviSynthCategories.Where(
                                Function(v) v.Name = "Source").First.Filters.Where(
                                Function(v) v.Name = i.Value)

                            If filters.Count > 0 Then
                                p.AvsDoc.SetFilter("Source", filters(0).Name, filters(0).Script)
                                Exit For
                            End If
                        End If
                    Next

                    If Not sourceFilter.Script.Contains("(") Then
                        Dim def = s.FilterPreferences.Where(Function(v) v.Name = "default")

                        If def.Count > 0 Then
                            Dim filters = s.AviSynthCategories.Where(
                                Function(v) v.Name = "Source").First.Filters.Where(
                                Function(v) v.Name = def(0).Value)

                            If filters.Count > 0 Then
                                p.AvsDoc.SetFilter("Source", filters(0).Name, filters(0).Script)
                            End If
                        End If
                    End If

                    If Not sourceFilter.Script.Contains("(") Then
                        Dim filter = AviSynthCategory.GetDefaults.Where(Function(v) v.Name = "Source").First.Filters.Where(Function(v) v.Name = "FFVideoSource").First
                        p.AvsDoc.SetFilter(filter.Category, filter.Name, filter.Script)
                        avsIndexing()
                    End If

                    If Not sourceFilter.Script.Contains("Crop(") Then
                        Dim sourceWidth = MediaInfo.GetVideo(p.NativeSourceFile, "Width").ToInt
                        Dim sourceHeight = MediaInfo.GetVideo(p.NativeSourceFile, "Height").ToInt

                        If sourceWidth Mod 4 <> 0 OrElse sourceHeight Mod 4 <> 0 Then
                            p.AvsDoc.GetFilter("Source").Script += CrLf + "Crop(0, 0, -" & sourceWidth Mod 4 & ", -" & sourceHeight Mod 4 & ")"
                        End If
                    End If

                    If Not sourceFilter.Script.Contains("ConvertToYV12") Then
                        Dim ChromaSubsampling = MediaInfo.GetVideo(p.NativeSourceFile, "ChromaSubsampling")

                        If ChromaSubsampling <> "4:2:0" Then
                            Dim format = MediaInfo.GetVideo(p.NativeSourceFile, "Format")
                            Dim matrix As String

                            If format = "RGB" Then
                                Dim sourceHeight = MediaInfo.GetVideo(p.NativeSourceFile, "Height").ToInt

                                If sourceHeight > 576 Then
                                    matrix = "matrix=""Rec709"""
                                Else
                                    matrix = "matrix=""Rec601"""
                                End If
                            End If

                            p.AvsDoc.GetFilter("Source").Script += CrLf + "ConvertToYV12(" + matrix + ")"
                        End If
                    End If
                End If
            End If

            AviSynthListView.Load()
            RenameDVDTracks()

            If FileTypes.AudioVideo.Contains(Filepath.GetExt(p.NativeSourceFile)) Then
                p.Audio0.Streams = MediaInfo.GetAudioStreams(p.NativeSourceFile)
                p.Audio1.Streams = p.Audio0.Streams
            End If

            DetectAudioFiles(0, True, True)
            DetectAudioFiles(1, True, True)

            DetectAudioFiles(0, False, True)
            DetectAudioFiles(1, False, True)

            DetectAudioFiles(0, False, False)
            DetectAudioFiles(1, False, False)

            If p.UseAvsAsAudioSource Then
                tbAudioFile0.Text = p.AvsDoc.Path
            Else
                If p.Audio0.File = "" AndAlso p.Audio1.File = "" Then
                    If Not TypeOf p.Audio0 Is NullAudioProfile AndAlso
                        Not FileTypes.VideoText.Contains(Filepath.GetExt(p.NativeSourceFile)) Then

                        tbAudioFile0.Text = p.NativeSourceFile

                        If p.Audio0.Streams.Count = 0 Then
                            tbAudioFile0.Text = ""
                        End If

                        If Not TypeOf p.Audio1 Is NullAudioProfile AndAlso
                            p.Audio0.Streams.Count > 1 Then

                            tbAudioFile1.Text = p.NativeSourceFile

                            For Each i In p.Audio1.Streams
                                If Not p.Audio0.Stream Is Nothing AndAlso
                                    Not p.Audio1.Stream Is Nothing Then

                                    If p.Audio0.Stream.StreamOrder = p.Audio1.Stream.StreamOrder Then
                                        For Each i2 In p.Audio1.Streams
                                            If i2.StreamOrder <> p.Audio1.Stream.StreamOrder Then
                                                tbAudioFile1.Text = i2.Name + " (" + Filepath.GetExt(p.Audio1.File) + ")"
                                                p.Audio1.Stream = i2
                                                UpdateAudio(p.Audio1)
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

            If Filepath.GetExtFull(p.SourceFile) = ".d2v" Then
                Dim content = File.ReadAllText(p.SourceFile)

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
                    Dim m = Regex.Match(content, "FINISHED +(\d+).+FILM")

                    If m.Success Then
                        Dim film = CInt(m.Groups(1).Value)

                        If film >= p.AutoForceFilmThreshold Then
                            content = content.Replace("Field_Operation=0" + CrLf + "Frame_Rate=29970 (30000/1001)", "Field_Operation=1" + CrLf + "Frame_Rate=23976 (24000/1001)")
                            content.WriteFile(p.SourceFile)
                        End If
                    End If
                End If
            End If

            Dim errorMsg = ""

            Try
                p.SourceAviSynthDocument.Synchronize()
                errorMsg = p.SourceAviSynthDocument.GetErrorMessage
            Catch ex As Exception
                errorMsg = ex.Message
            End Try

            If errorMsg <> "" Then
                Log.WriteHeader("Error opening source")
                Log.WriteLine(errorMsg + CrLf2)
                Log.WriteLine(Macro.Solve(p.SourceAviSynthDocument.GetScript))
                Log.Save()

                ProcessForm.CloseProcessForm()

                g.ShowDirectShowWarning()

                Using td As New TaskDialog(Of DialogResult)
                    td.Timeout = 60
                    td.MainInstruction = "Failed to open source, try another method?"
                    td.Content = errorMsg
                    td.CommonButtons = TaskDialogButtons.OkCancel

                    If td.Show = DialogResult.OK Then
                        TryAnotherFilter()
                    Else
                        p.AvsDoc.Synchronize()
                        Throw New AbortException
                    End If
                End Using

                errorMsg = ""

                Try
                    p.SourceAviSynthDocument.Synchronize()
                    errorMsg = p.SourceAviSynthDocument.GetErrorMessage
                Catch ex As Exception
                    errorMsg = ex.Message
                End Try

                If errorMsg <> "" Then
                    MsgError("Failed to open source", errorMsg)
                    p.AvsDoc.Synchronize()
                    Throw New AbortException
                End If
            End If

            Dim miFPS = MediaInfo.GetFrameRate(p.OriginalSourceFile)
            Dim avsFPS = p.SourceAviSynthDocument.GetFramerate

            If (CInt(miFPS) * 2) = CInt(avsFPS) Then
                Dim src = p.AvsDoc.GetFilter("Source")
                src.Script = src.Script + CrLf + "SelectEven().AssumeFPS(" & miFPS.ToString(CultureInfo.InvariantCulture) + ")"
                p.SourceAviSynthDocument.Synchronize()
            ElseIf miFPS <> avsFPS
                Dim src = p.AvsDoc.GetFilter("Source")
                src.Script = src.Script + CrLf + "# AssumeFPS(" + miFPS.ToString(CultureInfo.InvariantCulture) + ")"
                p.SourceAviSynthDocument.Synchronize()
            End If

            UpdateSourceParameters()
            SetSlider()

            BlockSourceTextBoxTextChanged = True
            tbSourceFile.Text = p.SourceFile
            BlockSourceTextBoxTextChanged = False

            s.LastPosition = 0

            SetTargetLength(p.SourceSeconds)
            lbSourceLength.Text = lTargetLength.Text

            DemuxVobSubSubtitles()
            ConvertBluRaySubtitles()
            ExtractForcedVobSubSubtitles()
            p.VideoEncoder.Muxer.Init()

            Dim crop = p.AvsDoc.IsFilterActive("Crop")

            If crop Then
                g.AutoCrop()
                DisableCropFilter()
                AviSynthListView.Load()
            End If

            AutoResize()

            If crop Then
                g.OvercropWidth()

                If p.AutoSmartCrop Then
                    g.SmartCrop()
                End If
            End If

            If p.AutoCompCheck AndAlso p.VideoEncoder.IsCompCheckEnabled Then
                p.VideoEncoder.RunCompCheck()
            End If

            Assistant()

            If Not p.BatchMode Then ProcessForm.CloseProcessForm()

            g.RaiseApplicationEvent(ApplicationEvent.AfterSourceLoaded)
            g.RaiseApplicationEvent(ApplicationEvent.ProjectOrSourceLoaded)
            Log.Save()
        Catch ex As AbortException
            Log.Save()
            ProcessForm.CloseProcessForm()
            SetSavedProject()
            OpenProject(recoverProjectPath)
            Text = recoverText
            g.ProjectPath = recoverPath
            If Not autoMode Then Throw New AbortException
        Catch ex As Exception
            g.OnException(ex)
        End Try
    End Sub

    Sub TryAnotherFilter(Optional recursive As Boolean = True)
        If p.AvsDoc.GetFilter("Source").Script.ToLower.Contains("directshowsource") Then
            Dim src = "FFVideoSource(""%source_file%"", cachefile = ""%temp_file%.ffindex"")" + CrLf +
                          "Crop(0, 0, -Width % 4, -Height % 4)" + CrLf + "ConvertToYV12()"

            p.AvsDoc.SetFilter("Source", "FFVideoSource", src)
            avsIndexing()
        Else
            Dim fpsParam = ""
            Dim fps = MediaInfo.GetFrameRate(p.SourceFile)

            If fps <> 0.0! Then
                fpsParam = ", convertfps=true, fps=" + fps.ToString(CultureInfo.InvariantCulture)
            ElseIf MediaInfo.GetVideo(p.SourceFile, "Format") = "Screen Video" Then
                fpsParam = ", convertfps=true, fps=10"
            End If

            Dim script = "DirectShowSource(""%source_file%"", audio=false" + fpsParam + ")" + CrLf + "ConvertToYV12()"
            p.AvsDoc.SetFilter("Source", "DirectShow", script)
        End If

        AviSynthListView.Load()

        Dim errorMsg = ""

        Try
            p.SourceAviSynthDocument.Synchronize()
            errorMsg = p.SourceAviSynthDocument.GetErrorMessage
        Catch ex As Exception
            errorMsg = ex.Message
        End Try

        If errorMsg <> "" Then
            g.ShowDirectShowWarning()

            If recursive Then
                TryAnotherFilter(False)
            Else
                MsgError(errorMsg)
                Throw New AbortException
            End If
        End If
    End Sub

    Sub AutoResize()
        If p.AvsDoc.IsFilterActive("Resize") Then
            If p.AutoResizeImage <> 0 Then
                SetTargetImageSizeByPixel(p.AutoResizeImage)
            Else
                If p.AdjustHeight Then
                    Dim h = Calc.FixMod16(CInt(p.TargetWidth / Calc.GetTargetDAR()))
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
            If Filepath.GetExtFull(i) = ".sup" AndAlso g.IsSourceSameOrSimilar(i) AndAlso
                Not File.Exists(Filepath.GetDirAndBase(i) + ".idx") Then

                Using proc As New Proc
                    proc.Init("Convert sup to sub: " + Filepath.GetName(i), "#>", "#<", "Decoding frame")
                    proc.File = Packs.BDSup2SubPP.GetPath
                    proc.Arguments = "-o """ + Filepath.GetDirAndBase(i) + ".idx"" """ + i + """"
                    proc.AllowedExitCodes = {}
                    proc.Start()
                End Using
            End If
        Next
    End Sub

    Sub ExtractForcedVobSubSubtitles()
        For Each i In g.GetFilesInTempDirAndParent
            If Filepath.GetExtFull(i) = ".idx" AndAlso g.IsSourceSameOrSimilar(i) AndAlso
                Not File.Exists(Filepath.GetDirAndBase(i) + "_Forced.idx") Then

                Dim idxContent = File.ReadAllText(i, Encoding.Default)

                If idxContent.Contains(VB6.ChrW(&HA) + VB6.ChrW(&H0) + VB6.ChrW(&HD) + VB6.ChrW(&HA)) Then
                    idxContent = idxContent.FixBreak
                    idxContent = idxContent.Replace(CrLf + VB6.ChrW(&H0) + CrLf, CrLf + "langidx: 0" + CrLf)
                    File.WriteAllText(i, idxContent, Encoding.Default)
                End If

                Using proc As New Proc
                    proc.Init("Extract forced subtitles if existing", "# ")
                    proc.WriteLine(Filepath.GetName(i) + CrLf2)
                    proc.File = Packs.BDSup2SubPP.GetPath
                    proc.Arguments = "--forced-only -o """ + Filepath.GetDirAndBase(i) + "_Forced.idx"" """ + i + """"
                    proc.AllowedExitCodes = {}
                    proc.Start()
                End Using
            End If
        Next
    End Sub

    Sub DemuxVobSubSubtitles()
        If Not {"vob", "m2v"}.Contains(Filepath.GetExt(p.NativeSourceFile)) Then Exit Sub
        Dim ifoPath = GetIfoFile()
        If ifoPath = "" Then Exit Sub
        If File.Exists(p.TempDir + Filepath.GetBase(p.SourceFile) + ".idx") Then Exit Sub
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
                            ifoPath + CrLf +
                            p.TempDir + Filepath.GetBase(p.SourceFile) + CrLf &
                            (i + 1) & CrLf +
                            "1" + CrLf +
                            "ALL" + CrLf +
                            "CLOSE"

                        Dim fileContent = p.TempDir + Filepath.GetBase(p.TargetFile) + "_vsrip.txt"
                        args.WriteFile(fileContent)

                        Using proc As New Proc
                            proc.Init("Demux subtitles using VSRip")
                            proc.WriteLine(args + CrLf2)
                            proc.File = Packs.VSRip.GetPath
                            proc.Arguments = """" + fileContent + """"
                            proc.Directory = Packs.VSRip.GetDir
                            proc.AllowedExitCodes = {0, 1, 2}
                            proc.Start()
                        End Using

                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Sub Encode()
        Try
            g.IsProcessing = True
            g.RaiseApplicationEvent(ApplicationEvent.BeforeEncoding)
            Dim startTime = DateTime.Now

            If p.BatchMode Then
                If g.ProjectPath Is Nothing Then
                    g.ProjectPath = p.TempDir + Filepath.GetBase(p.SourceFiles(0)) + ".srip"
                End If

                SaveProjectByPath(g.ProjectPath)
                OpenVideoSourceFiles(p.SourceFiles, False)
                p.BatchMode = False
                SaveProjectByPath(g.ProjectPath)
            End If

            ProcessForm.ShowForm()

            Log.WriteHeader("AviSynth Filters")
            Log.WriteLine(Macro.Solve(p.AvsDoc.GetScript))

            Log.WriteHeader("AviSynth Properties")

            Dim props = "source frame count: " & p.SourceAviSynthDocument.GetFrames & CrLf +
                "source frame rate: " & p.SourceAviSynthDocument.GetFramerate.ToString("f6", CultureInfo.InvariantCulture) + CrLf +
                "source duration: " + TimeSpan.FromSeconds(p.SourceAviSynthDocument.GetFrames / p.SourceAviSynthDocument.GetFramerate).ToString + CrLf +
                "target frame count: " & p.AvsDoc.GetFrames & CrLf +
                "target frame rate: " & p.AvsDoc.GetFramerate.ToString("f6", CultureInfo.InvariantCulture) + CrLf +
                "target duration: " + TimeSpan.FromSeconds(p.AvsDoc.GetFrames / p.AvsDoc.GetFramerate).ToString

            Log.WriteLine(props.FormatColumn(":"))

            Audio.Process(p.Audio0)
            p.Audio0.Encode()

            Audio.Process(p.Audio1)
            p.Audio1.Encode()

            p.VideoEncoder.Encode()
            Log.Save()
            p.VideoEncoder.Muxer.Mux()

            If p.SaveThumbnails Then Thumbnails.SaveThumbnails(p.TargetFile)

            Log.WriteHeader("Job Complete")
            Log.WriteStats(startTime)

            g.RaiseApplicationEvent(ApplicationEvent.JobEncoded)
        Finally
            g.IsProcessing = False
        End Try
    End Sub

    Function ProcessTip(message As String) As Boolean
        CurrentAssistantTipKey = message.MD5Hash

        If Not p.SkippedAssistantTips.Contains(CurrentAssistantTipKey) Then
            If message <> "" Then
                If message.Length > 130 Then
                    lTip.Font = New Font(lTip.Font.FontFamily, 8)
                Else
                    lTip.Font = New Font(lTip.Font.FontFamily, 9)
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

            If Value Is Nothing Then
                lTip.Cursor = Cursors.Default
            Else
                lTip.Cursor = Cursors.Hand
            End If
        End Set
    End Property

    Function Assistant() As Boolean
        If SkipAssistant Then
            Return False
        End If

        Dim isCropped = p.AvsDoc.IsFilterActive("Crop")
        Dim isResized = p.AvsDoc.IsFilterActive("Resize")

        tbTargetWidth.ReadOnly = Not isResized
        tbTargetHeight.ReadOnly = Not isResized

        g.Highlight(False, lSAR)
        g.Highlight(False, llAudioProfile0)
        g.Highlight(False, llAudioProfile1)
        g.Highlight(False, lCompCheck)
        g.Highlight(False, lAspectRatioError)
        g.Highlight(False, lQuality)
        g.Highlight(False, lZoom)
        g.Highlight(False, tbAudioFile0)
        g.Highlight(False, tbAudioFile1)
        g.Highlight(False, tbBitrate)
        g.Highlight(False, tbSize)
        g.Highlight(False, tbSourceFile)
        g.Highlight(False, tbTargetHeight)
        g.Highlight(False, tbTargetFile)
        g.Highlight(False, tbTargetWidth)
        g.Highlight(False, llMuxer)
        g.Highlight(False, lgbEncoder.ll)

        If ResetAssistantFont Then
            lTip.Font = New Font(Font.FontFamily, 9)
            ResetAssistantFont = False
        End If

        Dim cropw = p.SourceWidth
        Dim croph = p.SourceHeight

        If isCropped Then
            cropw = p.SourceWidth - p.CropLeft - p.CropRight
            croph = p.SourceHeight - p.CropTop - p.CropBottom
        End If

        Dim isValidAnamorphicSize = (p.TargetWidth = 1440 AndAlso p.TargetHeight = 1080) OrElse
            (p.TargetWidth = 960 AndAlso p.TargetHeight = 720)

        If isResized Then
            If isValidAnamorphicSize Then
                lAspectRatioError.Text = "n/a"
            Else
                lAspectRatioError.Text = Calc.GetAspectRatioError.ToString("f2") + "%"
            End If
        Else
            lAspectRatioError.Text = "n/a"

            If p.TargetWidth <> cropw Then
                tbTargetWidth.Text = cropw.ToString
            End If

            If p.TargetHeight <> croph Then
                tbTargetHeight.Text = croph.ToString
            End If
        End If

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
        lCompCheck.Text = p.Compressibility.ToString("f2")
        lQuality.Text = CInt(Calc.GetPercent).ToString() + " %"
        lAudioBitrate.Text = CInt(Calc.GetAudioBitrate).ToString

        Dim par = Calc.GetTargetPAR

        If Calc.IsARSignalingRequired OrElse (par.X = 1 AndAlso par.Y = 1) Then
            lPAR.Text = par.X & ":" & par.Y
        Else
            lPAR.Text = "n/a"
        End If

        lDAR.Text = Calc.GetTargetDAR.ToString.Shorten(5)

        lSAR.Text = (p.TargetWidth / p.TargetHeight).ToString.Shorten(5) +
            " " + Calc.GetMod(p.TargetWidth, p.TargetHeight)

        lSourceDar.Text = Calc.GetSourceDAR.ToString.Shorten(5)

        par = Calc.GetSimpleSourcePAR

        lSourcePAR.Text = par.X & ":" & par.Y

        lbSourceLength.Text = GetLengthString(p.SourceSeconds)
        lSourceImageSize.Text = p.SourceWidth.ToString + "/" + p.SourceHeight.ToString
        lSourceFramerate.Text = p.SourceFramerate.ToString("f3")

        lCodec.Text = p.Codec
        lCodecProfile.Text = p.CodecProfile

        lTip.Text = ""
        gbAssistant.Text = ""
        AssistantMethod = Nothing
        CanIgnoreTip = True
        AssistantPassed = False

        If Not p.BatchMode Then
            If Filepath.GetExtFull(p.TargetFile) = ".mp4" AndAlso p.TargetFile.Contains("#") Then
                If ProcessTip("Character # can't be processed by MP4Box, please rename target file.") Then
                    gbAssistant.Text = "Invalid target file name"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If p.AvsDoc.Filters.Count = 0 OrElse
                Not p.AvsDoc.Filters(0).Active OrElse
                p.AvsDoc.Filters(0).Category <> "Source" Then

                If ProcessTip("The first filter must have the category Source.") Then
                    gbAssistant.Text = "Invalid filter setup"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If p.SourceSeconds = 0 Then
                If ProcessTip("Click here to open a source file.") Then
                    AssistantMethod = AddressOf OpenSourceFiles
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

            If p.Audio0.File = p.TargetFile OrElse p.Audio1.File = p.TargetFile Then
                If ProcessTip("The audio source and target filepath is identical.") Then
                    g.Highlight(True, tbTargetFile)
                    gbAssistant.Text = "Invalid Targetpath"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If p.RemindToCrop AndAlso Not TypeOf p.VideoEncoder Is NullEncoder AndAlso
                p.AvsDoc.IsFilterActive("Crop") AndAlso
                ProcessTip("Click here to open the crop dialog. When done continue with Next.") Then

                AssistantMethod = AddressOf OpenCropDialog
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

            If Not p.VideoEncoder.Muxer.IsSupported(p.VideoEncoder.OutputFileType) Then
                If ProcessTip("The encoder outputs '" + p.VideoEncoder.OutputFileType + "' but the container '" + p.VideoEncoder.Muxer.Name + "' supports only " + p.VideoEncoder.Muxer.SupportedInputTypes.Join(", ") + ".") Then
                    gbAssistant.Text = "Encoder conflicts with container"
                    g.Highlight(True, llMuxer)
                    g.Highlight(True, lgbEncoder.ll)
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If Math.Abs(p.Audio0.Delay) > 2000 Then
                If ProcessTip("The audio delay is unusual high, tools that can prevent such delays are MakeMKV, ProjectX, dsmux, gdsmux or TS-Doctor." + CrLf +
                              "Visit the support forum for help on individual cases.") Then
                    lTip.Font = New Font(Font.FontFamily, 8)
                    ResetAssistantFont = True
                    g.Highlight(True, tbAudioFile0)
                    gbAssistant.Text = "Unusual high audio delay"
                    Return False
                End If
            End If

            If p.Audio0.File <> "" AndAlso Not p.VideoEncoder.Muxer.IsSupported(p.Audio0.OutputFileType) AndAlso Not p.Audio0.OutputFileType = "ignore" Then
                If ProcessTip("The audio format is '" + p.Audio0.OutputFileType + "' but the container '" + p.VideoEncoder.Muxer.Name + "' supports only " + p.VideoEncoder.Muxer.SupportedInputTypes.Join(", ") + ". Choose another audio profile or another container.") Then
                    g.Highlight(True, llAudioProfile0)
                    g.Highlight(True, llMuxer)
                    gbAssistant.Text = "Audio format conflicts with container"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If p.Audio1.File <> "" AndAlso Not p.VideoEncoder.Muxer.IsSupported(p.Audio1.OutputFileType) AndAlso Not p.Audio1.OutputFileType = "ignore" Then
                If ProcessTip("The audio format is '" + p.Audio1.OutputFileType + "' but the container '" + p.VideoEncoder.Muxer.Name + "' supports only " + p.VideoEncoder.Muxer.SupportedInputTypes.Join(", ") + ". Choose another audio profile or another container.") Then
                    g.Highlight(True, llAudioProfile1)
                    g.Highlight(True, llMuxer)
                    gbAssistant.Text = "Audio format conflicts with container"
                    CanIgnoreTip = False
                    Return False
                End If
            End If

            If p.VideoEncoder.Muxer.GetExtension <> Filepath.GetExtFull(p.TargetFile) Then
                If ProcessTip("The container requires " + p.VideoEncoder.Muxer.GetExtension.Trim("."c).ToUpper + " as target file type.") Then
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

            If p.TargetWidth > cropw Then
                If ProcessTip("The target width is larger then the source width, usually it's better to disable the resize filter.") Then
                    g.Highlight(True, lZoom)
                    g.Highlight(True, tbTargetWidth)
                    gbAssistant.Text = "Bad Image Size"
                    Return False
                End If
            End If

            If p.TargetHeight > croph Then
                If ProcessTip("The target height is larger then the source height, usually it's better to disable the resize filter.") Then
                    g.Highlight(True, lZoom)
                    g.Highlight(True, tbTargetHeight)
                    gbAssistant.Text = "Bad Image Size"
                    Return False
                End If
            End If

            Dim ae = Calc.GetAspectRatioError()

            If Not isValidAnamorphicSize AndAlso
                (ae > p.MaxAspectRatioError OrElse
                ae < -p.MaxAspectRatioError) _
                AndAlso p.AvsDoc.IsFilterActive("Resize") Then

                If ProcessTip("Use the resize slider to correct the aspect ratio error.") Then
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

                    AssistantMethod = AddressOf OpenPreview
                    gbAssistant.Text = "Cutting"
                    Return False
                End If
            Else
                If Not p.AvsDoc.IsFilterActive("Cutting") AndAlso Form.ActiveForm Is Me Then
                    If ProcessTip("The cutting filter settings don't match with the cutting settings used in the preview.") Then
                        gbAssistant.Text = "Invalid Cutting Settings"
                        CanIgnoreTip = False
                        Return False
                    End If
                End If

                If p.VideoEncoder.Muxer.Subtitles.Count > 0 Then
                    If ProcessTip("Subtitle cutting is not supported, please remove the subtitles from the container options.") Then
                        gbAssistant.Text = "Subtitle cutting unsupported"
                        CanIgnoreTip = False
                        Return False
                    End If
                End If
            End If

            If p.RemindToDoCompCheck AndAlso p.VideoEncoder.IsCompCheckEnabled AndAlso p.Compressibility = 0 Then
                If ProcessTip("Click here to start the compressibility check. The compressibility check helps to finds the ideal bitrate or image size.") Then
                    AssistantMethod = AddressOf p.VideoEncoder.RunCompCheck
                    g.Highlight(True, lCompCheck)
                    gbAssistant.Text = "Compressibility Check"
                    Return False
                End If
            End If

            If File.Exists(p.TargetFile) Then
                If FileTypes.VideoText.Contains(Filepath.GetExt(p.SourceFile)) AndAlso
                    File.ReadAllText(p.SourceFile).Contains(p.TargetFile) Then

                    If ProcessTip("Source and target name are identical, please choose another target name.") Then
                        CanIgnoreTip = False
                        tbTargetFile.BackColor = Color.Yellow
                        gbAssistant.Text = "Target File"
                        Return False
                    End If
                Else
                    AssistantMethod = Sub() g.OpenDirAndSelectFile(p.TargetFile, Handle)

                    If ProcessTip("The target file already exist, usually this means it was encoded successfully." + CrLf + "Click here to to open the containing directory.") Then
                        tbTargetFile.BackColor = Color.Yellow
                        gbAssistant.Text = "Target File"
                        Return False
                    End If
                End If
            End If

            If p.AvsDoc.IsFilterActive("Resize") Then
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
                        g.Highlight(True, tbSize)
                        g.Highlight(True, tbBitrate)
                        g.Highlight(True, tbTargetWidth)
                        g.Highlight(True, tbTargetHeight)
                        g.Highlight(True, lQuality)
                        lQuality.BackColor = Color.Red
                        gbAssistant.Text = "Quality"
                        Return False
                    End If
                End If
            End If

            If TypeOf p.VideoEncoder.Muxer Is MP4Muxer Then
                For Each i In p.VideoEncoder.Muxer.Subtitles
                    If Not IsOneOf(Filepath.GetExtFull(i.Path), ".idx", ".srt") Then
                        If ProcessTip("MP4 supports only SRT and IDX subtitles.") Then
                            CanIgnoreTip = False
                            gbAssistant.Text = "Invalid subtitle format"
                            Return False
                        End If
                    End If
                Next
            End If

            If Not (MouseButtons = MouseButtons.Left AndAlso ActiveControl Is tbResize) Then
                If Not p.AvsDoc.GetErrorMessage Is Nothing AndAlso Not Paths.VerifyRequirements Then
                    If ProcessTip(p.AvsDoc.GetErrorMessage) Then
                        CanIgnoreTip = False
                        gbAssistant.Text = "AviSynth Error"
                        Return False
                    End If
                End If

                If Not p.AvsDoc.GetErrorMessage Is Nothing Then
                    If ProcessTip(p.AvsDoc.GetErrorMessage) Then
                        MsgError(p.AvsDoc.GetErrorMessage)
                        CanIgnoreTip = False
                        gbAssistant.Text = "AviSynth Error"
                        Return False
                    End If
                End If
            End If
        Else
            If p.SourceFiles.Count = 0 Then
                If ProcessTip("Click here to open a source file.") Then
                    AssistantMethod = AddressOf OpenSourceFiles
                    gbAssistant.Text = "Assistant"
                    CanIgnoreTip = False
                    Return False
                End If
            End If
        End If

        gbAssistant.Text = "Add Job"

        If lTip.Font.Size <> 9 Then
            lTip.Font = New Font(lTip.Font.FontFamily, 9)
        End If

        lTip.Text = "Click on the next button to add a job."

        AssistantPassed = True
    End Function

    Private Sub OpenTargetFolder()
        g.ShellExecute(Filepath.GetDir(p.TargetFile))
    End Sub

    Dim BlockAudioTextChanged As Boolean

    Sub AudioTextChanged(tb As TextBox, ap As AudioProfile)
        If BlockAudioTextChanged Then
            Exit Sub
        End If

        If tb.Text.ContainsUnicode Then
            MsgWarn(Strings.NoUnicode)
            tb.Text = ""
            Exit Sub
        End If

        If tb.Text.Contains(":\") Then
            If tb.Text <> ap.File Then
                ap.File = tb.Text

                If Not p.AvsDoc.GetFilter("Source").Script.Contains("DirectShowSource") Then
                    ap.Delay = g.ExtractDelay(ap.File)
                End If

                ap.SetStreamOrLanguage()
            End If

            UpdateAudio(ap)

            BlockAudioTextChanged = True

            If ap.Stream Is Nothing Then
                Dim streams = MediaInfo.GetAudioStreams(tb.Text)
                If streams.Count > 0 Then tb.Text = GetAudioText(ap, streams(0), tb.Text)
            Else
                tb.Text = ap.Stream.Name + " (" + Filepath.GetExt(ap.File) + ")"
            End If

            BlockAudioTextChanged = False
        ElseIf tb.Text = "" Then
            ap.File = ""
            UpdateAudio(ap)
        End If
    End Sub

    Function GetAudioText(ap As AudioProfile, stream As AudioStream, path As String) As String
        For Each i In Language.Languages
            If path.Contains(i.CultureInfo.EnglishName) Then
                stream.Language = i
                Exit For
            End If
        Next

        Dim matchDelay = Regex.Match(path, " (-?\d+)ms")
        If matchDelay.Success Then stream.Delay = matchDelay.Groups(1).Value.ToInt

        Dim matchID = Regex.Match(path, " ID(\d+)")
        Dim name As String

        If matchID.Success Then
            stream.StreamOrder = matchID.Groups(1).Value.ToInt - 1
            name = stream.Name
        Else
            name = stream.Name.Substring(4)
        End If

        If Filepath.GetBase(ap.File) = Filepath.GetBase(p.SourceFile) Then
            Return name + " (" + Filepath.GetExt(ap.File) + ")"
        Else
            Return name + " (" + Filepath.GetName(ap.File) + ")"
        End If
    End Function

    Sub UpdateAudio(ap As AudioProfile)
        SetBitrateMuxAudioProfile(ap)
        lAudioBitrate.Text = CInt(Calc.GetAudioBitrate).ToString
        tbSize_TextChanged()
    End Sub

    Sub tbAudioFile0_TextChanged() Handles tbAudioFile0.TextChanged
        AudioTextChanged(tbAudioFile0, p.Audio0)
    End Sub

    Sub tbAudioFile1_TextChanged() Handles tbAudioFile1.TextChanged
        AudioTextChanged(tbAudioFile1, p.Audio1)
    End Sub

    Sub SetBitrateMuxAudioProfile(ap As AudioProfile)
        If TypeOf ap Is MuxAudioProfile Then
            If ap.Stream Is Nothing Then
                ap.Bitrate = Calc.GetBitrateFromFile(ap.File, p.SourceSeconds)
            Else
                ap.Bitrate = ap.Stream.Bitrate + ap.Stream.BitrateCore
            End If
        End If
    End Sub

    Sub bSkip_Click() Handles bNext.Click
        If Not CanIgnoreTip Then
            MsgWarn("The current assistant instruction or warning cannot be skipped.")
            Exit Sub
        End If

        If Not p.SkippedAssistantTips.Contains(CurrentAssistantTipKey) Then
            p.SkippedAssistantTips.Add(CurrentAssistantTipKey)
        End If

        If Not Paths.VerifyRequirements() Then
            Exit Sub
        End If

        If AssistantPassed Then
            AddJob(False, Nothing)
            OpenJobsDialog()
        Else
            Assistant()
        End If
    End Sub

    <Command("Perform | Encode", "Creates a job and runs the job list.", Switch:="encode|e")>
    Sub StartEncoding()
        AssistantPassed = True
        AddJob(False, Nothing)
        RunJobs()
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

        Dim srcScript = p.AvsDoc.GetFilter("Source").Script.ToLower

        For Each i In s.Demuxers
            If i.Name = "dsmux" Then
                If MediaInfo.GetAudioCount(p.SourceFile) = 0 Then
                    Continue For
                End If

                If CommandLineDemuxer.IsActive("DGIndexNV") OrElse
                    CommandLineDemuxer.IsActive("DGIndexIM") Then

                    Continue For
                End If

                If p.AvsDoc.Contains("Source", "DGSource(") OrElse
                    p.AvsDoc.Contains("Source", "DGSourceIM(") Then

                    Continue For
                End If
            End If

            If Not i.Active AndAlso Not srcScript.Contains(i.SourceFilter.ToLower + "(") Then Continue For

            If i.InputExtensions?.Length = 0 OrElse i.InputExtensions.Contains(Filepath.GetExt(p.SourceFile)) Then
                If srcScript = "" OrElse i.SourceFilter = "" OrElse
                    srcScript.Contains(i.SourceFilter.ToLower + "(") Then

                    Dim inputFormats = Not OK(i.InputFormats) OrElse i.InputFormats.Contains(getFormat())

                    If inputFormats Then
                        i.Run()
                        Refresh()

                        For Each iExt In i.OutputExtensions
                            Dim exitFor = False

                            For Each iFile In Directory.GetFiles(p.TempDir, "*." + iExt)
                                If g.IsSourceSame(iFile) AndAlso
                                    p.SourceFile <> iFile AndAlso
                                    Not Filepath.GetBase(iFile).EndsWith("_out") AndAlso
                                    Not Filepath.GetBase(iFile).Contains("_cut_") Then

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

        avsIndexing()
    End Sub

    Sub avsIndexing()
        Dim srcScript = p.AvsDoc.GetFilter("Source").Script.ToLower

        If srcScript.Contains("directshowsource") Then Exit Sub

        If srcScript.Contains("ffvideosource") Then
            If srcScript.Contains("cachefile") Then
                ffmsindex(p.SourceFile, p.TempDir + Filepath.GetBase(p.SourceFile) + ".ffindex")
            Else
                ffmsindex(p.SourceFile, p.SourceFile + ".ffindex")
            End If
        ElseIf srcScript.Contains("lwlibavvideosource") Then
            If Not File.Exists(p.SourceFile + ".lwi") AndAlso File.Exists(p.AvsDoc.Path) AndAlso
                Not FileTypes.VideoText.Contains(Filepath.GetExt(p.SourceFile)) Then

                Using proc As New Proc
                    proc.Init("Index LWLibavVideoSource")
                    proc.Encoding = Encoding.UTF8
                    proc.File = Packs.ffmpeg.GetPath
                    proc.Arguments = "-i """ + p.AvsDoc.Path + """"
                    proc.AllowedExitCodes = {0, 1}
                    proc.Start()
                End Using
            End If
        End If
    End Sub

    Sub ffmsindex(sourcePath As String, cachePath As String, Optional indexAudio As Boolean = False)
        If File.Exists(sourcePath) AndAlso Not File.Exists(cachePath) AndAlso
            Not FileTypes.VideoText.Contains(Filepath.GetExt(sourcePath)) Then

            Using o As New Proc
                o.Init("Index with ffmsindex", "Indexing, please wait...")
                o.File = Packs.ffms2.GetDir + "ffmsindex.exe"
                o.Arguments = If(indexAudio, "-t -1 ", "") + """" + sourcePath + """ """ + cachePath + """"
                o.Start()
            End Using
        End If
    End Sub

    <Command("Project | Open", "Opens a project.")>
    Sub OpenProjectFile()
        Using d As New SWF.OpenFileDialog
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

        For Each i In Directory.GetFiles(Paths.TemplateDir, "*.srip")
            td.AddCommandLink(Filepath.GetBase(i), i)
        Next

        If td.Show <> "" Then Return OpenProject(td.SelectedValue, True)
    End Function

    <Command("Dialog | Event Commands", Strings.EventCommands)>
    Sub OpenEventCommandsDialog()
        Using f As New EventCommandsEditor(s.EventCommands)
            If f.ShowDialog() = DialogResult.OK Then
                s.EventCommands = f.lb.Items.OfType(Of EventCommand).ToList
            End If
        End Using

        SaveSettings()
    End Sub

    <Command("Dialog | Settings", "Dialog to edit settings.")>
    Sub OpenSettingsDialog()
        Using form As New SimpleSettingsForm("Settings")
            Dim ui = form.SimpleUI

            Dim generalPage = ui.CreateFlowPage("General")
            generalPage.SuspendLayout()

            Dim cb = ui.AddCheckBox(generalPage)
            cb.Text = "Enable tooltips in menus (restart required)"
            cb.Tooltip = "If you disable this you can still right-click menu items to show the tooltip."
            cb.Checked = s.EnableTooltips
            cb.SaveAction = Sub(value) s.EnableTooltips = value

            Dim mb = ui.AddMenuButtonBlock(Of String)(generalPage)
            mb.Label.Text = "Startup Template:"
            mb.Label.Tooltip = "Template loaded when StaxRip starts."
            mb.MenuButton.Value = s.StartupTemplate
            mb.MenuButton.SaveAction = Sub(value) s.StartupTemplate = value
            mb.MenuButton.Add(From i In Directory.GetFiles(Paths.TemplateDir) Select Filepath.GetBase(i))

            Dim tb = ui.AddTextBlock(generalPage)
            tb.Label.Text = "Remember Window Positions:"
            tb.Label.Tooltip = "Title or beginning of the title of windows of which the location should be remembered. For all windows enter '''all'''."
            tb.Label.Offset = 12
            tb.Expand(tb.Edit)
            tb.Edit.Text = s.WindowPositionsRemembered.Join(", ")
            tb.Edit.SaveAction = Sub(value) s.WindowPositionsRemembered = value.SplitNoEmptyAndWhiteSpace(",")

            tb = ui.AddTextBlock(generalPage)
            tb.Label.Text = "Center Screen Window Positions:"
            tb.Label.Tooltip = "Title or beginning of the title of windows to be centered on the screen. For all windows enter '''all'''."
            tb.Label.Offset = 12
            tb.Expand(tb.Edit)
            tb.Edit.Text = s.WindowPositionsCenterScreen.Join(", ")
            tb.Edit.SaveAction = Sub(value) s.WindowPositionsCenterScreen = value.SplitNoEmptyAndWhiteSpace(",")

            ui.AddLine(generalPage, "Thumbnails")

            Dim num = ui.AddNumericBlock(generalPage)
            num.Label.Text = "Width:"
            num.NumEdit.Init(0, 10000, 16)
            num.NumEdit.Value = s.ThumbnailWidth
            num.NumEdit.SaveAction = Sub(value) s.ThumbnailWidth = CInt(value)

            num = ui.AddNumericBlock(generalPage)
            num.Label.Text = "Rows:"
            num.NumEdit.Init(0, 100, 1)
            num.NumEdit.Value = s.ThumbnailRows
            num.NumEdit.SaveAction = Sub(value) s.ThumbnailRows = CInt(value)

            num = ui.AddNumericBlock(generalPage)
            num.Label.Text = "Columns:"
            num.NumEdit.Init(0, 100, 1)
            num.NumEdit.Value = s.ThumbnailColumns
            num.NumEdit.SaveAction = Sub(value) s.ThumbnailColumns = CInt(value)

            generalPage.ResumeLayout()

            ui.CreateControlPage(New DemuxingControl, "Demuxing")

            Dim systemPage = ui.CreateFlowPage("System")
            systemPage.SuspendLayout()

            Dim mb2 = ui.AddMenuButtonBlock(Of ProcessPriorityClass)(systemPage)
            mb2.Label.Text = "Process Priority:"
            mb2.Label.Tooltip = "Process priority of the applications StaxRip launches."
            mb2.MenuButton.Value = s.ProcessPriority
            mb2.MenuButton.SaveAction = Sub(value) s.ProcessPriority = value

            Dim mb3 = ui.AddMenuButtonBlock(Of ToolStripRenderMode)(systemPage)
            mb3.Label.Text = "Menu Style:"
            mb3.Label.Tooltip = "Defines the style used to render main menus, context menus and toolbars."
            mb3.MenuButton.Value = s.ToolStripRenderMode
            mb3.MenuButton.SaveAction = Sub(value) s.ToolStripRenderMode = value

            cb = ui.AddCheckBox(systemPage)
            cb.Text = "Prevent system entering standby mode while encoding"
            cb.Checked = s.PreventStandby
            cb.SaveAction = Sub(value) s.PreventStandby = value

            cb = ui.AddCheckBox(systemPage)
            cb.Text = "Use recycle bin when temp files are deleted"
            cb.Checked = s.DeleteTempFilesToRecycleBin
            cb.SaveAction = Sub(value) s.DeleteTempFilesToRecycleBin = value

            systemPage.ResumeLayout()

            Dim filterPage = ui.CreateDataPage("Source Filters")

            Dim tipsFunc = Function() As StringPairList
                               Dim ret As New StringPairList
                               ret.Add(" Filters Menu", "StaxRip allows to assign a source filter profile to a particular source file type. The source filter profiles can be customized by right-clicking the filters menu in the main dialog.")

                               For Each i In s.AviSynthCategories.Where(
                                   Function(v) v.Name = "Source").First.Filters

                                   If i.Script <> "" Then
                                       ret.Add(i.Name, i.Script)
                                   End If
                               Next

                               Return ret
                           End Function

            filterPage.TipProvider.TipsFunc = tipsFunc

            Dim c1 = filterPage.AddTextColumn()
            c1.DataPropertyName = "Name"
            c1.HeaderText = "File Type"

            Dim c2 = filterPage.AddComboBoxColumn
            c2.DataPropertyName = "Value"
            c2.HeaderText = "Prefered Source Filter"
            c2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            Dim filterNames = s.AviSynthCategories.Where(
                Function(v) v.Name = "Source").First.Filters.Where(
                Function(v) v.Name <> "Automatic").Select(Function(v) v.Name).Sort.ToArray

            c2.Items.AddRange(filterNames)
            filterPage.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

            Dim bs As New BindingSource

            bs.DataSource = ObjectHelp.GetCopy(
                New StringPairList(s.FilterPreferences.Where(
                                   Function(a) filterNames.Contains(a.Value) AndAlso a.Name <> "")))

            filterPage.DataSource = bs

            ui.SelectLast("last settings page")

            If form.ShowDialog() = DialogResult.OK Then
                s.FilterPreferences = DirectCast(bs.DataSource, StringPairList)
                s.FilterPreferences.Sort()
                ui.Save()
                g.SetRenderer(MenuStrip)
                SetMenuStyle()
                SaveSettings()
            End If

            ui.SaveLast("last settings page")
        End Using
    End Sub

    <Command("Project | Save", "Saves the current project.")>
    Sub SaveProject()
        If g.ProjectPath Is Nothing Then
            ShowSaveDialog()
        Else
            SaveProjectByPath(g.ProjectPath)
        End If
    End Sub

    <Command("Project | Save Path", "Saves the current project at the specified path.")>
    Sub SaveProjectByPath(<Description("The path may contain macros."),
        Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))> path As String)

        path = Macro.Solve(path)

        If path.StartsWith(Paths.TemplateDir) Then
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
            Text = Application.ProductName + " x64 - " + Filepath.GetBase(path)
            UpdateRecentProjectsMenuItems(path)
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    <Command("Project | Save As", "Saves the current project.")>
    Sub SaveProjectAs()
        ShowSaveDialog()
    End Sub

    <Command("Project | Save As Template", "Saves the current project as template.")>
    Sub SaveProjectAsTemplate()
        If p.SourceFile = "" Then
            Dim b As New InputBox
            b.Text = "Enter the name of the template."
            b.Title = "Save Template"
            b.Value = p.TemplateName
            b.VerificationText = "Load template on startup"

            If b.Show = DialogResult.OK Then
                p.TemplateName = b.Value.RemoveChars(Path.GetInvalidFileNameChars)
                SaveProjectByPath(Paths.TemplateDir + p.TemplateName + ".srip")
                UpdateTemplateProjectsMenuItems()

                If b.Checked Then
                    s.StartupTemplate = b.Value
                End If
            End If
        Else
            MsgWarn("A template cannot be created after a source file was opened.")
        End If
    End Sub

    <Command("Perform | Start Compressibility Check", "Starts the compressibility check.")>
    Sub RunCompCheck()
        p.VideoEncoder.RunCompCheck()
    End Sub

    <Command("Perform | Exit StaxRip", "Exits StaxRip", Switch:="exit")>
    Sub ExitStaxRip()
        Close()
    End Sub

    <Command("Dialog | Applications", "Dialog to manage external applications.")>
    Sub OpenApplicationsDialog()
        Using f As New ApplicationsForm
            Dim found As Boolean

            If s.StringDictionary.ContainsKey("RecentExternalApplicationControl") Then
                For Each i As Package In Packs.Packages.Values
                    If i.Name = s.StringDictionary("RecentExternalApplicationControl") Then
                        f.ShowPackage(i)
                        found = True
                        Exit For
                    End If
                Next
            End If

            If Not found Then
                f.ShowPackage(Packs.x264)
            End If

            f.ShowDialog()
        End Using
    End Sub

    <Command("Dialog | Encoder Profiles", "Dialog to manage encoder profiles.")>
    Sub OpenEncoderProfilesDialog()
        Using f As New ProfilesForm("Encoder Profiles", s.VideoEncoderProfiles,
                                    AddressOf LoadVideoEncoder,
                                    AddressOf GetNewVideoEncoderProfile,
                                    AddressOf VideoEncoder.GetDefaults)

            If f.ShowDialog() = DialogResult.OK Then
                PopulateProfileMenu(DynamicMenuItemID.EncoderProfiles)
            End If
        End Using
    End Sub

    <Command("Dialog | Muxer Profiles", "Dialog to manage Muxer profiles.")>
    Sub OpenMuxerProfilesDialog()
        If p.VideoEncoder.OpenMuxerProfilesDialog() = DialogResult.OK Then
            PopulateProfileMenu(DynamicMenuItemID.MuxerProfiles)
        End If

        Assistant()
    End Sub

    Sub LoadVideoEncoder(profile As Profile)
        Dim currentMuxer = p.VideoEncoder.Muxer
        p.VideoEncoder = DirectCast(ObjectHelp.GetCopy(profile), VideoEncoder)

        If currentMuxer.IsSupported(p.VideoEncoder.OutputFileType) Then
            p.VideoEncoder.Muxer = currentMuxer
        Else
            p.VideoEncoder.Muxer.Init()
        End If

        tbTargetFile.Text = Filepath.GetChangeExt(p.TargetFile, p.VideoEncoder.Muxer.GetExtension)
        p.VideoEncoder.OnStateChange()
        RecalcBitrate()
        Assistant()
    End Sub

    Sub RecalcBitrate()
        tbSize_TextChanged()
    End Sub

    Function GetNewVideoEncoderProfile() As Profile
        Dim sb As New SelectionBox(Of VideoEncoder)
        sb.Title = "Add New Profile"
        sb.Text = "Please choose a profile from the defaults."

        sb.AddItem("Current Project", p.VideoEncoder)

        For Each i In VideoEncoder.GetDefaults()
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK Then
            Return sb.SelectedItem
        End If

        Return Nothing
    End Function

    <Command("Dynamic", "Placeholder for dynamically updated menu items.")>
    Sub DynamicMenuItem(<DispName("ID")> id As DynamicMenuItemID)
    End Sub

    Sub PopulateProfileMenu(id As DynamicMenuItemID)
        For Each i In CustomMainMenu.MenuItems.OfType(Of MenuItemEx)()
            If i.CustomMenuItem.MethodName = "DynamicMenuItem" AndAlso
                i.CustomMenuItem.Parameters(0).Equals(id) Then

                i.DropDownItems.Clear()

                Select Case id
                    Case DynamicMenuItemID.EncoderProfiles
                        g.PopulateProfileMenu(i.DropDownItems, s.VideoEncoderProfiles, AddressOf OpenEncoderProfilesDialog, AddressOf LoadVideoEncoder)
                    Case DynamicMenuItemID.MuxerProfiles
                        g.PopulateProfileMenu(i.DropDownItems, s.MuxerProfiles, AddressOf OpenMuxerProfilesDialog, AddressOf LoadMuxer)
                    Case DynamicMenuItemID.Audio1Profiles
                        g.PopulateProfileMenu(i.DropDownItems, s.AudioProfiles, Sub() OpenAudioProfilesDialog(0), AddressOf LoadAudioProfile0)
                    Case DynamicMenuItemID.Audio2Profiles
                        g.PopulateProfileMenu(i.DropDownItems, s.AudioProfiles, Sub() OpenAudioProfilesDialog(1), AddressOf LoadAudioProfile1)
                    Case DynamicMenuItemID.FilterSetupProfiles
                        g.PopulateProfileMenu(i.DropDownItems, s.AviSynthProfiles, AddressOf OpenAviSynthProfilesDialog, AddressOf LoadAviSynthProfile)
                End Select

                Exit For
            End If
        Next
    End Sub

    <Command("Dialog | Crop", "Dialog to crop borders.")>
    Sub OpenCropDialog()
        If Not OK(p.SourceFile) Then
            OpenSourceFiles()
        Else
            If Not Paths.VerifyRequirements Then Exit Sub
            If Not g.IsValidSource Then Exit Sub

            If Not g.EnableFilter("Crop") Then
                p.AvsDoc.InsertAfter("Source", New AviSynthFilter("Crop", "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)", True))
            End If

            Using f As New CropForm
                f.ShowDialog()
            End Using

            DisableCropFilter()
            AviSynthListView.Load()
            Assistant()
        End If
    End Sub

    <Command("Dialog | Preview", "Dialog to preview or cut the video.")>
    Sub OpenPreview()
        If Not OK(p.SourceFile) Then
            OpenSourceFiles()
        Else
            If Not Paths.VerifyRequirements OrElse Not g.IsValidSource Then
                Exit Sub
            End If

            Dim m = p.AvsDoc.GetErrorMessage

            If Not m Is Nothing Then
                MsgError(m)
                Exit Sub
            End If

            Dim cutting = p.AvsDoc.GetFilter("Cutting")

            If Not cutting Is Nothing Then
                p.AvsDoc.Filters.Remove(cutting)
                g.MainForm.AviSynthListView.Load()
                g.MainForm.SetTargetLength(p.AvsDoc.GetSeconds)
            End If

            Dim doc As New AviSynthDocument
            doc = New AviSynthDocument()
            doc.Filters = p.AvsDoc.GetFiltersCopy

            If p.SourceHeight > 576 Then
                doc.Filters.Add(New AviSynthFilter("ConvertToRGB(matrix=""Rec709"")"))
            Else
                doc.Filters.Add(New AviSynthFilter("ConvertToRGB(matrix=""Rec601"")"))
            End If

            doc.Path = p.TempDir + p.Name + "_Preview.avs"
            doc.Synchronize()

            Dim f As New PreviewForm(doc)
            f.Owner = g.MainForm
            f.Show()
        End If
    End Sub

    <Command("Dialog | Main Menu Editor", "Dialog to configure the main menu.")>
    Sub OpenMainMenuEditor()
        s.CustomMenuMainForm = CustomMainMenu.Edit()
        g.SetRenderer(MenuStrip)
        Refresh()
        LoadMainMenuDynamic()
        UpdateRecentProjectsMenuItems()
        UpdateTemplateProjectsMenuItems()
        UpdateDynamicMenu()
        SaveSettings()
    End Sub

    <Command("Dialog | Jobs", "Dialog to manage batch jobs.")>
    Sub OpenJobsDialog()
        Using f As New JobsForm()
            If f.ShowDialog() = DialogResult.OK Then
                Refresh()
                RunJobs()
            End If
        End Using
    End Sub

    <Command("Perform | Clear Jobs", "Clears the job list.")>
    Sub ClearJobs()
        FileHelp.Delete(Paths.SettingsDir + "Jobs.dat")
    End Sub

    <Command("Perform | Run Jobs", "Runs all active jobs of the job list.")>
    Sub RunJobs()
        If Not Paths.VerifyRequirements() Then Exit Sub
        SaveSettings()
        RunJobRecursive()
        OpenProject(g.ProjectPath, False)
        ProcessForm.CloseProcessForm(False)
        g.RaiseApplicationEvent(ApplicationEvent.JobsEncoded)
        g.ShutdownPC()
    End Sub

    Sub RunJobRecursive()
        Dim jobs = From i In JobsForm.GetJobs() Where i.Value = True
        If jobs.Count = 0 Then Exit Sub
        Dim jobPath = jobs(0).Key
        JobsForm.ActivateJob(jobPath, False)
        g.MainForm.OpenProject(jobPath, False)

        Try
            If s.PreventStandby Then PowerRequest.SuppressStandby()
            StaxRip.ProcessForm.ShutdownVisible = True
            Encode()
            Log.Save()
            JobsForm.RemoveJob(jobPath)

            If p.DeleteTempFilesDir AndAlso p.TempDir.Contains("temp files") Then
                Try
                    FileHelp.Delete(p.TempDir + p.Name + "_StaxRip.log", VB6.FileIO.RecycleOption.SendToRecycleBin)

                    Dim moreJobsToProcessInTempDir = JobsForm.GetJobs.Where(Function(a) a.Value AndAlso a.Key.Contains(p.TempDir))

                    If moreJobsToProcessInTempDir.Count = 0 Then
                        Dim tempDir = p.TempDir
                        OpenProject(Paths.StartupTemplatePath, False)
                        MediaInfo.ClearCache()

                        If s.DeleteTempFilesToRecycleBin Then
                            DirectoryHelp.Delete(tempDir, VB6.FileIO.RecycleOption.SendToRecycleBin)
                        Else
                            DirectoryHelp.Delete(tempDir)
                        End If
                    End If
                Catch
                End Try
            End If
        Catch ex As AbortException
            Log.Save()
            OpenProject(g.ProjectPath, False)
            ProcessForm.CloseProcessForm()
            Exit Sub
        Catch ex As ErrorAbortException
            Log.Save()
            OpenProject(g.ProjectPath, False)
            ProcessForm.CloseProcessForm()
            g.ShowException(ex, Nothing, 120)
            g.ShellExecute(p.TempDir + p.Name + "_StaxRip.log")
        Catch ex As Exception
            Log.Save()
            g.OnException(ex)
            Exit Sub
        Finally
            If s.PreventStandby Then PowerRequest.EnableStandby()
            StaxRip.ProcessForm.ShutdownVisible = False
        End Try

        RunJobRecursive()
    End Sub

    Function GetJobPath() As String
        Dim name = Filepath.GetBase(p.TargetFile)

        If Not OK(name) Then
            name = Filepath.GetBase(p.SourceFile)
        End If

        Return p.TempDir + name + ".srip"
    End Function

    <Command("Perform | Load Profile", "Loads a audio or video profile.")>
    Sub LoadProfile(<DispName("Video")> videoProfile As String,
                    <DispName("Audio 1")> audioProfile1 As String,
                    <DispName("Audio 2")> audioProfile2 As String)

        If videoProfile <> "" Then
            For Each i In s.VideoEncoderProfiles
                If i.Name = videoProfile Then
                    LoadVideoEncoder(i)
                End If
            Next
        End If

        If audioProfile1 <> "" Then
            For Each i In s.AudioProfiles
                If i.Name = audioProfile1 Then
                    LoadAudioProfile0(i)
                End If
            Next
        End If

        If audioProfile2 <> "" Then
            For Each i In s.AudioProfiles
                If i.Name = audioProfile2 Then
                    LoadAudioProfile1(i)
                End If
            Next
        End If
    End Sub

    <Command("Perform | Add Job", "Adds a job to the job list.")>
    Sub AddJob(
        <DispName("Show Confirmation"), DefaultValue(True)> showConfirmation As Boolean,
        <DispName("Template Name"), Description("Name of the template to be loaded after the job was added. Empty to load no template.")>
        templateName As String)

        If Not Paths.VerifyRequirements() Then
            Exit Sub
        End If

        If Not IsLoading AndAlso Not g.MainForm.AssistantPassed Then
            MsgWarn("Please follow the assistant.")
            Exit Sub
        End If

        Dim path = GetJobPath()
        g.MainForm.SaveProjectByPath(path)

        JobsForm.AddJob(path)

        If showConfirmation Then
            MsgInfo("Job added")
        End If

        If templateName <> "" Then
            LoadProject(Paths.TemplateDir + templateName + ".srip")
        End If
    End Sub

    <Command("Dialog | Compare and extract images", "Compare and extract images for codec comparisons.")>
    Sub ImageGrabber()
        Dim f As New CodecComparisonForm
        f.Show()
        f.Add()
    End Sub

    <Command("Dialog | Filters", "Dialog to edit filters.")>
    Sub OpenFiltersEditor()
        AviSynthListView.ShowEditor()
    End Sub

    <Command("Dialog | Tasks", "Shows a dialog to perform a large collection of tasks.")>
    Sub OpenTaskDialog()
        Using f As New MacrosForm
            f.ShowDialog()
        End Using
    End Sub

    <Command("Dialog | Options", "Dialog to configure project options.")>
    Sub OpenOptionsDialog()
        OpenOptionsDialog(Nothing)
    End Sub

    Sub OpenOptionsDialog(pagePath As String)
        Using form As New SimpleSettingsForm(
            "Project Options",
            "In order to save project options go to:",
            "Main Menu > Project > Save As Template",
            "In order to choose a template to be loaded on program startup go to:",
            "Main Menu > Tools > Settings > General > Templates > Default Template")

            Dim ui = form.SimpleUI

            Dim imagePage = ui.CreateFlowPage("Image")

            Dim cb = ui.AddCheckBox(imagePage)
            cb.Text = "Save Thumbnails"
            cb.Tooltip = "Saves thumbnails in the target folder. Customizations can be made in the settings under:" + CrLf2 + "General > Advanced > Thumbnails"
            cb.Checked = p.SaveThumbnails
            cb.SaveAction = Sub(value) p.SaveThumbnails = value

            Dim nb = ui.AddNumericBlock(imagePage)
            nb.Label.Text = "Auto resize image size:"
            nb.Label.Tooltip = "Resizes to a given pixel size after loading a source file."
            nb.Label.Offset = 9
            nb.NumEdit.Init(0, Integer.MaxValue, 10000)
            nb.NumEdit.Value = p.AutoResizeImage
            nb.NumEdit.SaveAction = Sub(value) p.AutoResizeImage = CInt(value)

            ui.AddLabel(nb, "(0 = disabled)")

            nb = ui.AddNumericBlock(imagePage)
            nb.Label.Text = "Resize slider width:"
            nb.Label.Offset = 9
            nb.NumEdit.Init(0, Integer.MaxValue, 64)
            nb.NumEdit.Value = p.ResizeSliderMaxWidth
            nb.NumEdit.SaveAction = Sub(value) p.ResizeSliderMaxWidth = CInt(value)

            ui.AddLabel(nb, "(0 = auto)")

            Dim mbi = ui.AddMenuButtonBlock(Of Integer)(imagePage)
            mbi.Label.Text = "Output Mod:"
            mbi.Label.Offset = 9
            mbi.MenuButton.Add({2, 4, 8, 16})
            mbi.MenuButton.Value = p.ForcedOutputMod
            mbi.MenuButton.SaveAction = Sub(value) p.ForcedOutputMod = value

            Dim aspectRatioPage = ui.CreateFlowPage("Image|Aspect Ratio")

            cb = ui.AddCheckBox(aspectRatioPage)
            cb.Text = "Auto Aspect Ratio Signaling"
            cb.Tooltip = "In case no resize filter is used or the aspect ratio error exceeds the defined maximal value StaxRip signals the encoder to write the aspect ratio to the bitstream so players that support aspect ratio signaling playback still the proper aspect ratio." + CrLf2
            cb.Checked = p.AutoARSignaling
            cb.SaveAction = Sub(value) p.AutoARSignaling = value

            cb = ui.AddCheckBox(aspectRatioPage)
            cb.Text = "Use ITU-R BT.601 compliant aspect ratio"
            cb.Tooltip = "Calculates the aspect ratio according to ITU-R BT.601 standard. "
            cb.Checked = p.ITU
            cb.SaveAction = Sub(value) p.ITU = value

            cb = ui.AddCheckBox(aspectRatioPage)
            cb.Text = "Adjust height according to target display aspect ratio"
            cb.Tooltip = "Adjusts the height to match the target display aspect ratio in case the auto resize option is disabled."
            cb.Checked = p.AdjustHeight
            cb.SaveAction = Sub(value) p.AdjustHeight = value

            nb = ui.AddNumericBlock(aspectRatioPage)
            nb.Label.Text = "Maximum Aspect Ratio Error:"
            nb.NumEdit.Init(1, 10, 1)
            nb.NumEdit.Value = p.MaxAspectRatioError
            nb.NumEdit.SaveAction = Sub(value) p.MaxAspectRatioError = CInt(value)

            Dim tb = ui.AddTextBlock(aspectRatioPage)
            tb.Label.Text = "Custom Source DAR:"
            tb.Label.Tooltip = "Custom source display aspect ratio which overrides the automatic detection and uses the format 4/3 or 4:3 or " & 1.333333 & "."
            tb.Label.Offset = 8
            tb.Edit.Text = p.CustomDAR
            tb.Edit.TextBox.ValidationFunc = AddressOf IsCorrectAspectRatioFormat
            tb.Edit.SaveAction = Sub(value) p.CustomDAR = value

            tb = ui.AddTextBlock(aspectRatioPage)
            tb.Label.Text = "Custom Source PAR: "
            tb.Label.Tooltip = "Custom source pixel aspect ratio which overrides the automatic detection and uses the format 4/3 or 4:3 or " & 1.333333 & "."
            tb.Label.Offset = 8
            tb.Edit.Text = p.CustomPAR
            tb.Edit.TextBox.ValidationFunc = AddressOf IsCorrectAspectRatioFormat
            tb.Edit.SaveAction = Sub(value) p.CustomPAR = value

            Dim cropPage = ui.CreateFlowPage("Image|Crop")

            cb = ui.AddCheckBox(cropPage)
            cb.Text = "Auto correct crop values"
            cb.Tooltip = "Force crop values compatible with YUV/YV12 colorspace and with the forced output mod value. "
            cb.Checked = p.AutoCorrectCropValues
            cb.SaveAction = Sub(value) p.AutoCorrectCropValues = value

            cb = ui.AddCheckBox(cropPage)
            cb.Text = "Auto crop borders until proper aspect ratio is found"
            cb.Tooltip = "Automatically crops borders until the proper aspect ratio is found."
            cb.Checked = p.AutoSmartCrop
            cb.SaveAction = Sub(value) p.AutoSmartCrop = value

            nb = ui.AddNumericBlock(cropPage)
            nb.Label.Text = "Auto overcrop width to limit aspect ratio to:"
            nb.Label.Tooltip = "On small devices it can help to restrict the aspect ratio and overcrop the width instead."
            nb.NumEdit.Init(0, 2, 0.1D, 3)
            nb.NumEdit.Value = CDec(p.AutoSmartOvercrop)
            nb.NumEdit.SaveAction = Sub(value) p.AutoSmartOvercrop = value

            ui.AddLine(cropPage, "Crop Values")

            Dim eb = ui.AddEmptyBlock(cropPage)

            ui.AddLabel(eb, "Left:", 2)

            Dim te = ui.AddEdit(eb)
            te.Width = 60
            te.Text = p.CropLeft.ToString
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value) If value.IsInt Then p.CropLeft = CInt(value)

            Dim l = ui.AddLabel(eb, "Right:", 4)

            te = ui.AddEdit(eb)
            te.Width = 60
            te.Text = p.CropRight.ToString
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value) If value.IsInt Then p.CropRight = CInt(value)

            eb = ui.AddEmptyBlock(cropPage)

            ui.AddLabel(eb, "Top:", 2)

            te = ui.AddEdit(eb)
            te.Width = 60
            te.Text = p.CropTop.ToString
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value) If value.IsInt Then p.CropTop = CInt(value)

            l = ui.AddLabel(eb, "Bottom:", 4)

            te = ui.AddEdit(eb)
            te.Width = 60
            te.Text = p.CropBottom.ToString
            te.TextBox.TextAlign = HorizontalAlignment.Center
            te.SaveAction = Sub(value) If value.IsInt Then p.CropBottom = CInt(value)

            Dim audioPage = ui.CreateFlowPage("Audio")
            audioPage.SuspendLayout()

            cb = ui.AddCheckBox(audioPage)
            cb.Text = "Use AviSynth script as audio source"
            cb.Tooltip = "Sets the AviSynth script (*.avs) as audio source file when loading a source file."
            cb.Checked = p.UseAvsAsAudioSource
            cb.SaveAction = Sub(value) p.UseAvsAsAudioSource = value

            Dim dec = ui.AddMenuButtonBlock(Of DecodingMode)(audioPage)
            dec.Label.Text = "Force decoding using:"
            dec.Tooltip = "Defines if audio should be processed without decoding if possible or if decoding should be forced with a certain decoder."
            dec.MenuButton.Value = p.DecodingMode
            dec.MenuButton.SaveAction = Sub(value) p.DecodingMode = value

            Dim cut = ui.AddMenuButtonBlock(Of CuttingMode)(audioPage)
            cut.Label.Text = "Preferred cutting method:"
            cut.Tooltip = "Defines which method to use for cutting."
            cut.MenuButton.Value = p.CuttingMode
            cut.MenuButton.SaveAction = Sub(value) p.CuttingMode = value

            audioPage.ResumeLayout()

            Dim subPage = ui.CreateFlowPage("Subtitles")

            cb = ui.AddCheckBox(subPage)
            cb.Text = "Convert Sup (PGS/Blu-ray) to Sub (IDX/DVD)"
            cb.Tooltip = "Works only with demuxed subtitles."
            cb.Checked = p.ConvertSup2Sub
            cb.SaveAction = Sub(value) p.ConvertSup2Sub = value

            tb = ui.AddTextBlock(subPage)
            tb.Label.Text = "Auto load subtitles:"
            tb.Label.Tooltip = "Subtitles loaded automatically using [http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes two letter language] code separated by comma or the magic word 'all'." + CrLf2 + String.Join(CrLf, From i In Language.Languages Where i.IsCommon Select i.ToString + ": " + i.TwoLetterCode)
            tb.Edit.Text = p.AutoSubtitles
            tb.Edit.SaveAction = Sub(value) p.AutoSubtitles = value

            Dim pathPage = ui.CreateFlowPage("Paths")

            l = ui.AddLabel(pathPage, "Default Target Directory:")
            l.Tooltip = "Leave empty to use the source file directory"

            Dim tm = ui.AddTextMenuBlock(pathPage)
            tm.Label.Visible = False
            tm.Expand(tm.Edit)
            tm.Edit.Text = p.DefaultTargetFolder
            tm.Edit.SaveAction = Sub(value) p.DefaultTargetFolder = value
            tm.AddMenu("Edit...", Function() Paths.BrowseFolder(p.DefaultTargetFolder))
            tm.AddMenu("Directory of source file", "%source_dir%")
            tm.AddMenu("Parent directory of source file directory", "%source_dir_parent%")

            l = ui.AddLabel(pathPage, "Default Target Name:")
            l.Tooltip = "Leave empty to use the source file name"
            l.MarginTop = Font.Height

            tm = ui.AddTextMenuBlock(pathPage)
            tm.Label.Visible = False
            tm.Expand(tm.Edit)
            tm.Edit.Text = p.DefaultTargetName
            tm.Edit.SaveAction = Sub(value) p.DefaultTargetName = value
            tm.AddMenu("Name of source file", "%source_name%")
            tm.AddMenu("Name of source file directory", "%source_dir_name%")

            l = ui.AddLabel(pathPage, "Temp Files Directory:")
            l.Tooltip = "Leave empty to use the source file directory"
            l.MarginTop = Font.Height

            Dim tempDirFunc = Function()
                                  Dim tempDir = Paths.BrowseFolder(p.TempDir)
                                  If tempDir <> "" Then Return DirPath.AppendSeparator(tempDir) + "%source_name% temp files"
                              End Function

            tm = ui.AddTextMenuBlock(pathPage)
            tm.Label.Visible = False
            tm.Expand(tm.Edit)
            tm.Edit.Text = p.TempDir
            tm.Edit.SaveAction = Sub(value) p.TempDir = value
            tm.AddMenu("Edit...", tempDirFunc)
            tm.AddMenu("Source File Directory", "%source_dir%%source_name% temp files")

            Dim assistantPage = ui.CreateFlowPage("Assistant")

            cb = ui.AddCheckBox(assistantPage)
            cb.Text = "Remind To Crop"
            cb.Checked = p.RemindToCrop
            cb.SaveAction = Sub(value) p.RemindToCrop = value

            cb = ui.AddCheckBox(assistantPage)
            cb.Text = "Remind To Cut"
            cb.Checked = p.RemindToCut
            cb.SaveAction = Sub(value) p.RemindToCut = value

            cb = ui.AddCheckBox(assistantPage)
            cb.Text = "Remind To Do Compressibility Check"
            cb.Checked = p.RemindToDoCompCheck
            cb.SaveAction = Sub(value) p.RemindToDoCompCheck = value

            cb = ui.AddCheckBox(assistantPage)
            cb.Text = "Remind To Set Filters"
            cb.Checked = p.RemindToSetFilters
            cb.SaveAction = Sub(value) p.RemindToSetFilters = value

            cb = ui.AddCheckBox(assistantPage)
            cb.Text = "Remind Not To Oversize"
            cb.Checked = p.RemindOversize
            cb.SaveAction = Sub(value) p.RemindOversize = value

            Dim filtersPage = ui.CreateFlowPage("Filters")

            l = ui.AddLabel(filtersPage, "Code appended to trim functions:")
            l.Tooltip = "Code appended to trim functions StaxRip generates using the cut feature."
            l.MarginTop = Font.Height \ 2

            tb = ui.AddTextBlock(filtersPage)
            tb.Label.Visible = False
            tb.Expand(tb.Edit)
            tb.Edit.Height = CInt(Font.Height * 3)
            tb.Edit.TextBox.Multiline = True
            tb.Edit.UseMacroEditor = True
            tb.Edit.Text = p.TrimAvsCode
            tb.Edit.SaveAction = Sub(value) p.TrimAvsCode = value

            l = ui.AddLabel(filtersPage, "Code inserted at top of scripts:")
            l.Tooltip = "Code inserted at the top of every script StaxRip generates."
            l.MarginTop = Font.Height \ 2

            tb = ui.AddTextBlock(filtersPage)
            tb.Label.Visible = False
            tb.Expand(tb.Edit)
            tb.Edit.Height = CInt(Font.Height * 3)
            tb.Edit.TextBox.Multiline = True
            tb.Edit.UseMacroEditor = True
            tb.Edit.Text = p.AvsCodeAtTop
            tb.Edit.SaveAction = Sub(value) p.AvsCodeAtTop = value

            Dim miscPage = ui.CreateFlowPage("Misc")

            cb = ui.AddCheckBox(miscPage)
            cb.Text = "Delete temp files directory"
            cb.Checked = p.DeleteTempFilesDir
            cb.SaveAction = Sub(value) p.DeleteTempFilesDir = value

            nb = ui.AddNumericBlock(miscPage)
            nb.Label.Text = "Auto Force Film Threshold (Percent):"
            nb.Label.Tooltip = "Threshold at which force film is applied automatically."
            nb.NumEdit.Init(0, 100, 1)
            nb.NumEdit.Value = p.AutoForceFilmThreshold
            nb.NumEdit.SaveAction = Sub(value) p.AutoForceFilmThreshold = CInt(value)

            ui.AddLine(miscPage, "Compressibility Check")

            cb = ui.AddCheckBox(miscPage)
            cb.Text = "Auto run compressibility check"
            cb.Tooltip = "Performs a compressibility check after loading a source file."
            cb.Checked = p.AutoCompCheck
            cb.SaveAction = Sub(value) p.AutoCompCheck = value

            nb = ui.AddNumericBlock(miscPage)
            nb.Label.Text = "Percentage to use for compressibility check:"
            nb.Label.Offset = 15
            nb.NumEdit.Init(2, 20, 1)
            nb.NumEdit.Value = p.CompCheckRange
            nb.NumEdit.SaveAction = Sub(value) p.CompCheckRange = CInt(value)

            Dim compCheckButton = ui.AddMenuButtonBlock(Of CompCheckAction)(miscPage)
            compCheckButton.Label.Text = "After the compressibility check adjust:"
            compCheckButton.Label.Offset = 15
            compCheckButton.MenuButton.Value = p.CompCheckAction
            compCheckButton.MenuButton.SaveAction = Sub(value) p.CompCheckAction = value

            If pagePath <> "" Then
                ui.ShowPage(pagePath)
            Else
                ui.SelectLast("last options page")
            End If

            If form.ShowDialog() = DialogResult.OK Then
                ui.Save()

                If p.CompCheckRange < 2 OrElse p.CompCheckRange > 20 Then
                    p.CompCheckRange = 5
                End If

                If OK(p.TempDir) Then
                    p.TempDir = DirPath.AppendSeparator(p.TempDir)
                End If

                If OK(p.DefaultTargetFolder) Then
                    p.DefaultTargetFolder = DirPath.AppendSeparator(p.DefaultTargetFolder)
                End If

                SetSlider()
                Assistant()
            End If

            ui.SaveLast("last options page")
        End Using
    End Sub

    Function IsCorrectAspectRatioFormat(inputString As String) As Boolean
        If inputString = "" OrElse Regex.IsMatch(inputString, Strings.ParRegexPattern) Then
            Return True
        End If

        Dim r = 0.0F

        If Single.TryParse(inputString, r) Then
            If r > 0 AndAlso r < 3 Then
                Return True
            End If
        End If

        MsgWarn("Invalid format, please use:" + CrLf2 + "4/3 or 4:3 or " & 1.333333)
    End Function

    Sub DisableCropFilter()
        Dim f = p.AvsDoc.GetFilter("Crop")

        If Not f Is Nothing AndAlso CInt(p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) = 0 Then
            f.Active = False
        End If
    End Sub

    <Command("Dialog | Filter Setup Profiles", "Dialog to configure filter setup profiles.")>
    Sub OpenAviSynthProfilesDialog()
        Using f As New ProfilesForm("AviSynth Profiles", s.AviSynthProfiles,
                                    AddressOf LoadAviSynthProfile,
                                    AddressOf GetAVSDocAsProfile,
                                    AddressOf AviSynthDocument.GetDefaults)

            If f.ShowDialog() = DialogResult.OK Then
                PopulateProfileMenu(DynamicMenuItemID.FilterSetupProfiles)
            End If
        End Using
    End Sub

    Function GetFilterProfilesText(categories As List(Of AviSynthCategory)) As String
        Dim ret = ""

        For Each i In categories
            ret += "[" + i.Name + "]" + CrLf

            For Each i2 In i.Filters
                If i2.Category = "Source" AndAlso i2.Name = "Automatic" Then
                    Continue For
                End If

                If i2.Script.Contains(CrLf) Then
                    Dim lines = i2.Script.SplitLinesNoEmpty

                    For x = 0 To lines.Length - 1
                        lines(x) = VB6.vbTab + lines(x)
                    Next

                    ret += If(ret.EndsWith(CrLf2), "", CrLf) + i2.Path + " =" + CrLf + lines.Join(CrLf) + CrLf2
                Else
                    ret += i2.Path + " = " + i2.Script + CrLf
                End If
            Next

            ret += CrLf
        Next

        Return ret
    End Function

    <Command("Dialog | AviSynth Filter Profiles", "Dialog to configure AviSynth filter profiles.")>
    Sub OpenAviSynthFilterProfilesDialog()
        Using f As New AviSynthScriptEditor(GetFilterProfilesText(s.AviSynthCategories))
            f.Text = "Filter Profiles"

            f.bnContext.Text = " Restore Defaults... "
            f.bnContext.Visible = True

            Dim t = f

            Dim resetAction = Sub()
                                  If MsgOK("Restore defaults?") Then
                                      t.MacroEditorControl.Value = GetFilterProfilesText(AviSynthCategory.GetDefaults)
                                  End If
                              End Sub

            f.bnContext.AddClickAction(resetAction)

            If f.ShowDialog(Me) = DialogResult.OK Then
                s.AviSynthCategories.Clear()
                Dim cat As AviSynthCategory
                Dim filter As AviSynthFilter

                For Each i In f.MacroEditorControl.Value.SplitLinesNoEmpty
                    Dim multiline = i.StartsWith(VB6.vbTab) OrElse i.StartsWith(" "c)
                    i = i.Trim()

                    If i.StartsWith("[") AndAlso i.EndsWith("]") Then
                        cat = New AviSynthCategory(i.Substring(1, i.Length - 2).Trim)
                        s.AviSynthCategories.Add(cat)
                    End If

                    If multiline Then
                        If Not filter Is Nothing Then
                            filter.Script += CrLf + i.Trim
                            filter.Script = filter.Script.Trim
                        End If
                    Else
                        Dim filterName = i.Left("=").Trim

                        If filterName <> "" Then
                            filter = New AviSynthFilter(cat.Name, filterName, i.Right("=").Trim, True)
                            cat.Filters.Add(filter)
                        End If
                    End If
                Next

                For Each i In AviSynthCategory.GetDefaults
                    Dim found = False

                    For Each i2 In s.AviSynthCategories
                        If i.Name = i2.Name Then
                            found = True
                        End If
                    Next

                    If Not found AndAlso IsOneOf(i.Name, "Source", "Crop", "Resize") Then
                        MsgWarn("The category '" + i.Name + "' was recreated. A Source, Crop and Resize category is mandatory.")
                        s.AviSynthCategories.Add(i)
                    End If
                Next

                Dim src = s.AviSynthCategories.Where(Function(v) v.Name = "Source").First
                src.Filters.Insert(0, New AviSynthFilter("Source", "Automatic", "", True))

                g.MainForm.SaveSettings()
            End If
        End Using
    End Sub

    <Command("Help | Command Line", "Opens the command line help.", Switch:="help|h|?")>
    Private Sub OpenCmdlHelp()
        Dim f As New HelpForm()
        f.Owner = Me

        f.Doc.WriteStart("Command Line Help")
        f.Doc.WriteP("StaxRip uses a similar command line syntax as the .NET framework tools as it simplifies development and usage. The arguments are processed sequentially in the order they appear in the command line so it would be fatal to put the shutdown switch before the encode switch or to put the source files after the encode switch. Don't forget to enclose strings containing blanks with quotes and ensure to study the examples at the bottom of this page.")
        f.Doc.WriteP("Many of the available switches might not be useful from the command line, the reason why these switches are available is they are provided by StaxRip's command engine that is also used for customizable menu's and event commands.")

        Dim commands As New List(Of Command)(CommandManager.Commands.Values)
        commands.Sort()

        Dim basic, advanced As New StringPairList

        For Each iCommand In commands
            Dim params = iCommand.MethodInfo.GetParameters
            Dim switches As String()
            Dim switch = iCommand.Attribute.Switch

            If switch Is Nothing Then
                switches = {iCommand.Attribute.Name}
            Else
                If switch.Contains("|") Then
                    switches = switch.SplitNoEmpty("|")
                Else
                    switches = {switch}
                End If
            End If

            For iSwitch = 0 To switches.Length - 1
                switches(iSwitch) = "-" + switches(iSwitch).Replace(" ", "").Replace("|", "/")
            Next

            Dim desc = iCommand.Attribute.Description

            If desc Is Nothing Then
                desc = "n/a"
            End If

            If params.Length = 0 Then
                If switch Is Nothing Then
                    advanced.Add(String.Join("<br>", switches), desc)
                Else
                    basic.Add(String.Join(", ", switches), desc)
                End If
            Else
                Dim paramList As New List(Of String)
                Dim enumList As New List(Of String)

                For Each iParam As ParameterInfo In params
                    If iParam.ParameterType.IsEnum Then
                        Dim l As New List(Of String)

                        For Each i As System.Enum In System.Enum.GetValues(iParam.ParameterType)
                            l.Add(i.ToString)
                        Next

                        enumList.Add(iParam.ParameterType.Name + ": " + String.Join(", ", l.ToArray))
                    End If

                    paramList.Add("&lt;" + DispNameAttribute.GetValue(iParam.GetCustomAttributes(False)) + " As " + iParam.ParameterType.Name.Replace("Int32", "Integer") + "&gt;")
                Next

                For iSwitch As Integer = 0 To switches.Length - 1
                    switches(iSwitch) += ":<i>" + String.Join(",", paramList.ToArray) + "</i>"
                Next

                Dim switchcell As String = String.Join("<br>", switches)

                If enumList.Count > 0 Then
                    switchcell += "<br><br>" + String.Join("<br>", enumList.ToArray)
                End If

                If switch Is Nothing Then
                    advanced.Add(switchcell, desc)
                Else
                    basic.Add(switchcell, desc)
                End If
            End If
        Next

        f.Doc.WriteTable("Basic Switches", basic)
        f.Doc.WriteTable("Advanced Switches", advanced)

        f.Doc.WriteElement("h2", "Examples")
        f.Doc.WriteP("StaxRip C:\Movie\project.srip")
        f.Doc.WriteP("StaxRip ""C:\Movie 2\project.srip""")
        f.Doc.WriteP("StaxRip ""C:\Movie 2\VTS_01_1.VOB""")
        f.Doc.WriteP("StaxRip ""C:\Movie 2\VTS_01_1.VOB"" ""C:\Movie 2\VTS_01_2.VOB""")
        f.Doc.WriteP("StaxRip -template:DVB ""C:\Movie 2\capture.mpg"" -encode -standby")
        f.Doc.WriteP("StaxRip -template:""beware of blanks"" ""C:\Movie 2\VTS_01_1.VOB"" ""C:\Movie 2\VTS_01_2.VOB"" -encode -shutdown")
        f.Doc.WriteP("StaxRip -Perform/ShowMessageBox:""beware of blanks"",title,info")
        f.Doc.WriteP("StaxRip -Perform/ShowMessageBox:no_blanks_no_quotes,""wasted_quotes"",info")
        f.Doc.WriteP("StaxRip -Perform/ShowMessageBox:no_blanks_no_quotes,nothingToWaste,info")
        f.Doc.WriteP("StaxRip -perform/showmessagebox:""Windows and VB don't care about case, Unix and C do"",title,info")
        f.Doc.WriteP("StaxRip /perform/showmessagebox:""I hope StaxRip CLI don't suck since I've seen too many ugly CLI's"",title,info")

        f.Show()
    End Sub

    <Command("Dialog | Help File", "Opens a given URL or local file in the help browser.")>
    Sub OpenHelpURL(
        <DispName("URL"),
        Description("URL or local file to be shown in the internet explorer powered help browser."),
        Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))>
        url As String)

        Dim f As New HelpForm(Macro.Solve(url))
        f.Show()
    End Sub

    <Command("Help | Make Report Bug", "Makes a bug report.")>
    Sub MakeBugReport()
        g.MakeBugReport(Nothing)
    End Sub

    Shared Function GetDefaultMainMenu() As CustomMenuItem
        Dim ret As New CustomMenuItem("Root")

        ret.Add("Project|Open...", "OpenProjectFile", Keys.O Or Keys.Control)
        ret.Add("Project|Save", "SaveProject", Keys.S Or Keys.Control)
        ret.Add("Project|Save As...", "SaveProjectAs")
        ret.Add("Project|Save As Template...", "SaveProjectAsTemplate")
        ret.Add("Project|-")
        ret.Add("Project|Templates", "DynamicMenuItem", DynamicMenuItemID.TemplateProjects)
        ret.Add("Project|Recent", "DynamicMenuItem", DynamicMenuItemID.RecentProjects)

        ret.Add("Crop", "OpenCropDialog", Keys.F4)
        ret.Add("Preview", "OpenPreview", Keys.F5)

        ret.Add("Options", "OpenOptionsDialog", Keys.F8)

        ret.Add("Tools|Jobs...", "OpenJobsDialog", Keys.F6)
        ret.Add("Tools|Apps...", "OpenApplicationsDialog")

        ret.Add("Tools|Files|Log File", "ExecuteCmdl", """%text_editor%"" ""%working_dir%%target_name%_StaxRip.log""")
        ret.Add("Tools|Files|AviSynth Script", "ExecuteCmdl", """%text_editor%"" ""%avs_file%""")

        ret.Add("Tools|Directories|Source", "ExecuteCmdl", """%source_dir%""")
        ret.Add("Tools|Directories|Working", "ExecuteCmdl", """%working_dir%""")
        ret.Add("Tools|Directories|Target", "ExecuteCmdl", """%target_dir%""")
        ret.Add("Tools|Directories|Settings", "ExecuteCmdl", """%settings_dir%""")
        ret.Add("Tools|Directories|Templates", "ExecuteCmdl", """%settings_dir%Templates""")
        ret.Add("Tools|Directories|Plugins", "ExecuteCmdl", """%plugin_dir%""")
        ret.Add("Tools|Directories|Startup", "ExecuteCmdl", """%startup_dir%""")
        ret.Add("Tools|Directories|Programs", "ExecuteCmdl", """%programs_dir%""")
        ret.Add("Tools|Directories|System", "ExecuteCmdl", """%system_dir%""")
        ret.Add("Tools|Launch", "DynamicMenuItem", DynamicMenuItemID.LaunchApplications)

        ret.Add("Tools|Advanced|AVSMeter...", "ExecuteCmdl", """%app:AVSMeter%"" ""%avs_file%""" + CrLf + "pause", False, False, True)
        ret.Add("Tools|Advanced|Codec Comparison...", "ImageGrabber")
        ret.Add("Tools|Advanced|Command Prompt...", "ShowCommandPrompt")
        ret.Add("Tools|Advanced|Event Commands...", "OpenEventCommandsDialog")
        ret.Add("Tools|Advanced|Hardcoded Subtitle...", "AddHardcodedSubtitle")
        ret.Add("Tools|Advanced|LAV Filters video decoder configuration...", "OpenLAVFiltersConfiguration")
        ret.Add("Tools|Advanced|MediaInfo Folder View...", "OpenMediaInfoFolderView")
        ret.Add("Tools|Advanced|Reset Setting...", "ResetSettings")
        ret.Add("Tools|Advanced|Thumbnails Generator...", "BatchGenerateThumbnails")

        ret.Add("Tools|Edit Menu...", "OpenMainMenuEditor")
        ret.Add("Tools|Settings...", "OpenSettingsDialog", "")

        If g.IsCulture("de") Then
            ret.Add("Help|Support Forum", "ExecuteCmdl", "http://forum.gleitz.info/showthread.php?26177-StaxRip-Encoding-Frontend-(Diskussion)")
        Else
            ret.Add("Help|Support Forum", "ExecuteCmdl", "http://forum.videohelp.com/threads/369913-StaxRip-support-%28encoder-GUI-for-x265-h265-hevc-mkv-4K%29")
        End If

        ret.Add("Help|Guides", "ExecuteCmdl", "http://sourceforge.net/p/staxmedia/wiki/Guides")
        ret.Add("Help|Bug Report", "MakeBugReport")
        ret.Add("Help|Mail", "ExecuteCmdl", "mailto:frank_skare@yahoo.de?subject=StaxRip%20feedback")
        ret.Add("Help|Website", "ExecuteCmdl", "http://staxmedia.sourceforge.net")
        ret.Add("Help|Donate", "ExecuteCmdl", Strings.DonationsURL)
        ret.Add("Help|Changelog", "OpenHelpTopic", "changelog")
        ret.Add("Help|Command Line", "OpenCmdlHelp")
        ret.Add("Help|Apps", "DynamicMenuItem", DynamicMenuItemID.HelpApplications)
        ret.Add("Help|-")
        ret.Add("Help|Info...", "OpenHelpTopic", "info")

        Return ret
    End Function

    <Command("Perform | Add Hardcoded Subtitle", "Adds a hardcoded subtitle.")>
    Sub AddHardcodedSubtitle()
        Using d As New SWF.OpenFileDialog
            d.SetFilter(FileTypes.SubtitleIncludingContainers)
            d.SetInitDir(s.LastSourceDir)

            If d.ShowDialog = DialogResult.OK Then
                If Filepath.GetExtFull(d.FileName) = ".idx" Then
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

                    If sb.Show = DialogResult.Cancel Then
                        Exit Sub
                    End If

                    Regex.Replace(File.ReadAllText(d.FileName), "langidx: \d+", "langidx: " +
                                  sb.SelectedItem.IndexIDX.ToString).WriteFile(d.FileName)
                End If

                Dim filter As New AviSynthFilter

                filter.Category = "Subtitle"
                filter.Path = Filepath.GetName(d.FileName)
                filter.Active = True

                If Filepath.GetExtFull(d.FileName) = ".idx" Then
                    filter.Script = "VobSub(""" + d.FileName + """)"
                Else
                    filter.Script = "TextSub(""" + d.FileName + """)"
                End If

                Dim insertCat = If(p.AvsDoc.IsFilterActive("Crop"), "Crop", "Source")
                p.AvsDoc.InsertAfter(insertCat, filter)
                AviSynthListView.Load()
            End If
        End Using
    End Sub

    Private Sub tbResize_MouseUp(sender As Object, e As MouseEventArgs) Handles tbResize.MouseUp
        Assistant()
    End Sub

    Private Sub tbResize_Scroll() Handles tbResize.Scroll
        SkipAssistant = True

        If Not g.EnableFilter("Resize") Then
            p.AvsDoc.Filters.Add(New AviSynthFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)", True))
            AviSynthListView.Load()
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

    Private Sub tbBitrate_KeyDown(sender As Object, e As KeyEventArgs) Handles tbBitrate.KeyDown
        If e.KeyData = Keys.Up Then
            e.Handled = True
            tbBitrate.Text = Calc.GetPreviousMod(tbBitrate.Text.ToInt(1), 50).ToString
        ElseIf e.KeyData = Keys.Down Then
            e.Handled = True
            tbBitrate.Text = Calc.GetNextMod(tbBitrate.Text.ToInt(1), 50).ToString
        End If
    End Sub

    Private Sub tbSize_KeyDown(sender As Object, e As KeyEventArgs) Handles tbSize.KeyDown
        Dim l As Integer

        Select Case p.TargetSeconds
            Case Is > 80 * 60
                l = 50
            Case Is > 40 * 60
                l = 20
            Case Is > 20 * 60
                l = 10
            Case Is > 10 * 60
                l = 5
            Case Else
                l = 1
        End Select

        If e.KeyData = Keys.Up Then
            e.Handled = True
            tbSize.Text = Calc.GetPreviousMod(tbSize.Text.ToInt(1), l).ToString
        ElseIf e.KeyData = Keys.Down Then
            e.Handled = True
            tbSize.Text = Calc.GetNextMod(tbSize.Text.ToInt(1), l).ToString
        End If
    End Sub

    Sub tbSize_TextChanged() Handles tbSize.TextChanged
        Try
            If Integer.TryParse(tbSize.Text, Nothing) Then
                p.Size = CInt(tbSize.Text)
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
            If Integer.TryParse(tbBitrate.Text, Nothing) Then
                p.VideoBitrate = CInt(tbBitrate.Text)
                BlockBitrate = True

                If Not BlockSize Then
                    tbSize.Text = CInt((Calc.GetVideoKBytes() + Calc.GetAudioKBytes() + Calc.GetOverheadAndSubtitlesKBytes()) / 1024).ToString
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
        Dim ret As CustomMenuItem = New CustomMenuItem("Root")

        ret.Add("700 MB", "SetSize", 700)
        ret.Add("800 MB", "SetSize", 800)
        ret.Add("1/3 DVD", "SetSize", Keys.None, 1493)
        ret.Add("1 DVD", "SetSize", 4480)
        ret.Add("-")
        ret.Add("50%", "SetPercent", 50)
        ret.Add("60%", "SetPercent", 60)
        ret.Add("-")
        ret.Add("Edit Menu...", "OpenSizeMenuEditor")

        Return ret
    End Function

    <Command("Parameter | Target Image Size", "Sets the target image size.")>
    Sub SetTargetImageSize(width As Integer, height As Integer)
        If Not g.EnableFilter("Resize") Then
            p.AvsDoc.Filters.Add(New AviSynthFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)", True))
            AviSynthListView.Load()
        End If

        tbTargetWidth.Text = width.ToString

        If height = 0 Then
            SetImageHeight()
        Else
            tbTargetHeight.Text = height.ToString
        End If
    End Sub

    <Command("Parameter | Target Image Size By Pixel", "Sets the target image size by pixels (width x height).")>
    Sub SetTargetImageSizeByPixel(pixel As Integer)
        If Not g.EnableFilter("Resize") Then
            p.AvsDoc.Filters.Add(New AviSynthFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)", True))
            AviSynthListView.Load()
        End If

        Dim cropw = p.SourceWidth - p.CropLeft - p.CropRight
        Dim ar = Calc.GetTargetDAR()
        Dim w = Calc.FixMod16(CInt(Math.Sqrt(pixel * ar)))

        If w > cropw Then w = (cropw \ 16) * 16

        Dim h = Calc.FixMod16(CInt(w / ar))

        tbTargetWidth.Text = w.ToString()
        tbTargetHeight.Text = h.ToString()
    End Sub

    <Command("Parameter | Target File Size", "Sets the target file size.")>
    Sub SetSize(<DispName("Target File Size")> targetSize As Integer)
        tbSize.Text = targetSize.ToString
    End Sub

    <Command("Parameter | Target Video Bitrate", "Sets the target video bitrate.")>
    Sub SetBitrate(<DispName("Target Video Bitrate")> bitrate As Integer)
        tbBitrate.Text = bitrate.ToString
    End Sub

    <Command("Perform | Auto Crop", "Crops borders automatically.")>
    Sub PerformAutoCrop()
        g.AutoCrop()
        Assistant()
    End Sub

    <Command("Perform | Smart Crop", "Crops borders automatically until the proper aspect ratio is found.")>
    Sub PerformSmartCrop()
        g.SmartCrop()
        Assistant()
    End Sub

    <Command("Parameter | Bitrate By Percent", "Sets the bitrate according to the compressibility.")>
    Sub SetPercent(<DispName("Percent Value")> value As Integer)
        tbSize.Text = g.GetAutoSize(value).ToString
    End Sub

    <Command("Dialog | Size Menu Editor", "Menu editor for the size menu.")>
    Sub OpenSizeMenuEditor()
        s.CustomMenuSize = CustomSizeMenu.Edit()
    End Sub

    Private Sub UpdateSizeMenu()
        For Each i As MenuItemEx In CustomSizeMenu.MenuItems
            If i.CustomMenuItem.MethodName = "SetPercent" Then
                i.Enabled = p.Compressibility > 0
            End If
        Next
    End Sub

    Private Function GetAVSDocAsProfile() As Profile
        Dim sb As New SelectionBox(Of TargetAviSynthDocument)

        sb.Title = "New Profile"
        sb.Text = "Please choose a profile."

        sb.AddItem("Current Project", p.AvsDoc)

        For Each i In AviSynthDocument.GetDefaults
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK Then
            Return sb.SelectedItem
        End If

        Return Nothing
    End Function

    Function GetTargetAviSynthDocument() As AviSynthDocument
        Return p.AvsDoc
    End Function

    Sub LoadAviSynthProfile(profileInterface As Profile)
        p.AvsDoc = DirectCast(ObjectHelp.GetCopy(profileInterface), TargetAviSynthDocument)
        AviSynthListView.Load()
        Assistant()
    End Sub

    Private Sub MainForm_Shown() Handles Me.Shown
        Activate() 'needed for custom settings dir option
        Refresh()

        Packs.Init()

        If Not File.Exists(Packs.NeroAACEnc.GetPath) Then
            MsgError("Files included with StaxRip are missing, maybe the 7-Zip archive wasn't properly unpacked. You can find a packer at [http://www.7-zip.org www.7-zip.org].")
            Close()
            Exit Sub
        End If

        If Application.StartupPath.Contains(":\Program Files") Then
            Try
                Dim fp = Application.StartupPath + "\write privileges check"
                File.WriteAllText(fp, "aaa")
                File.Delete(fp)
            Catch ex As Exception
                MsgWarn("Please move StaxRip out of the Program Files directory.")
                Close()
                Exit Sub
            End Try
        End If

        UpdateRecentProjectsMenuItems()
        UpdateTemplateProjectsMenuItems()
        UpdateDynamicMenu()
        ProcessCmdl(Environment.GetCommandLineArgs)

        IsLoading = False

        'Using f As New TestForm
        '    f.StartPosition = FormStartPosition.CenterScreen
        '    f.ShowDialog()
        'End Using

        'AddHandler tbAudioFile0.TextChanged, Sub()
        '                                         MsgInfo(Environment.StackTrace)
        '                                     End Sub

        'OpenVideoSourceFiles({"D:\Video\Samples\M2TS\1080p - VC-1 - AC3 - AC3 TrueHD - PCM.m2ts"}.ToList)

        'For Each i In Directory.GetFiles("E:\Video\Samples\misc", "*.*", SearchOption.AllDirectories)
        '    If i.EndsWith(".wmv") Then Continue For

        '    Dim fpsMediaInfo = MediaInfo.GetFrameRate(i).ToSingle
        '    Dim fps As Double

        '    Dim doc As New AviSynthDocument
        '    doc.Path = "D:\Temp\test.avs"
        '    Dim cachefile = "D:\Temp\test.ffindex"
        '    doc.Filters.Add(New AviSynthFilter("FFVideoSource(""" + i + """, cachefile = """ + cachefile + """)"))
        '    doc.Synchronize()

        '    Using avs As New AVIFile(doc.Path)
        '        fps = avs.FrameRate
        '    End Using

        '    File.Delete(doc.Path)
        '    File.Delete(cachefile)

        '    Debug.WriteLine(fpsMediaInfo.ToString.PadRight(7) + "; " & fps.ToString.PadRight(15) + "; " + i)
        'Next

        'Using f As New MediaInfoForm("C:\Daten\Temp\Filme\The Rewrite.mkv")
        '    f.ShowDialog()
        'End Using

        'MsgInfo(ProcessHelp.GetStandardOutput("C:\Daten\Projekte\VS\CPP\cpp_console\Debug\cpp_console.exe", ""))
        'MsgInfo(ProcessHelp.GetErrorOutput("C:\Daten\Projekte\VS\CPP\cpp_console\Debug\cpp_console.exe", ""))

        'Using pp As New Process
        '    'pp.StartInfo.FileName = Packs.eac3to.GetPath
        '    pp.StartInfo.FileName = "C:\Daten\Projekte\VS\CPP\cpp_console\Debug\cpp_console.exe"
        '    'pp.StartInfo.Arguments = "aaa"
        '    pp.StartInfo.CreateNoWindow = True
        '    pp.StartInfo.RedirectStandardError = True
        '    pp.StartInfo.RedirectStandardOutput = True
        '    pp.StartInfo.UseShellExecute = False
        '    pp.Start()
        '    Dim aaa = pp.StandardError.ReadToEnd()
        '    Dim bbb = pp.StandardOutput.ReadToEnd()
        '    pp.WaitForExit()
        '    MsgInfo(aaa)
        '    MsgInfo(bbb)
        'End Using

        ''Exit Sub

        'Using pr As New Proc
        '    pr.BeginReadBoth("aaa")
        '    'pr.File = Packs.eac3to.GetPath
        '    'pr.Arguments = "aaa"
        '    pr.File = "C:\Daten\Projekte\VS\CPP\cpp_console\Debug\cpp_console.exe"
        '    pr.AllowedExitCodes = {0, 1}
        '    pr.Start()
        '    MsgInfo(ProcessForm.CmdlLog.ToString, "")
        'End Using

        'ProcessForm.CloseProcessForm()

        'Dim fp2 = "C:\Daten\Temp\test.mkv"
        'MediaInfo.GetVideo(fp2, "Format")

        'Try
        '    File.Delete(fp2)
        'Catch ex As Exception
        '    g.ShowException(ex)
        'End Try

        'Using avi As New AVIFile("C:\Daten\Temp\test temp files\test.avs")
        '    avi.GetBitmap()
        'End Using

        'FileHelp.Delete("C:\Daten\Temp\test.mp4", VB6.FileIO.RecycleOption.SendToRecycleBin)

        'Dim sw As New Stopwatch
        'sw.Start()
        'Dim output = ProcessHelp.GetStandardOutput(Packs.QSVEncC.GetPath, "--check-lib")
        'sw.Stop()
        'MsgInfo(sw.ElapsedMilliseconds.ToString)

        'MsgInfo(Registry.LocalMachine.GetString("SOFTWARE\Intel\GFX", "Version"))

        'Using f As New AviSynthEditor
        '    f.StartPosition = FormStartPosition.CenterParent
        '    f.ShowDialog()
        'End Using

        'LoadProject("D:\Temp\test temp files\test.srip")
    End Sub

    <Command("Dialog | LAV Filters video decoder configuration", "Shows LAV Filters video decoder configuration")>
    Private Sub OpenLAVFiltersConfiguration()
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

    <Command("Perform | Execute Multiple StaxRip Commands", "Executes multiple StaxRip commands using command line switches.")>
    Private Sub EcecuteStaxRipCmdlArgs(<Description("Requires a single command line switch on each line."),
        Editor(GetType(MacroStringTypeEditor), GetType(UITypeEditor))> switches As String)

        Dim errorArg As String = Nothing

        Try
            For Each i In switches.SplitLinesNoEmpty
                errorArg = i

                If Not CommandManager.ProcessCmdlArgument(i) Then
                    Throw New Exception
                End If
            Next
        Catch
            MsgWarn("Error parsing argument: " + errorArg)
        End Try
    End Sub

    Private Sub ProcessCmdl(a As String())
        Dim files As New List(Of String)

        For Each i In CLIArg.GetArgs(a)
            Try
                If Not i.IsFile AndAlso files.Count > 0 Then
                    Dim l As New List(Of String)(files)
                    OpenAnyFile(l)
                    files.Clear()
                End If

                If i.IsFile Then
                    files.Add(i.Value)
                Else
                    If Not CommandManager.ProcessCmdlArgument(i.Value) Then
                        Throw New Exception
                    End If
                End If
            Catch ex As Exception
                MsgWarn("Error parsing argument: " + i.Value)
                OpenCmdlHelp()
            End Try
        Next

        If files.Count > 0 Then
            OpenAnyFile(files)
        End If
    End Sub

    <Command("Perform | Standby", "Puts PC in standby mode.", Switch:="standby")>
    Private Sub Standby()
        g.ShutdownPC(ShutdownMode.Standby)
    End Sub

    <Command("Perform | Shutdown", "Shuts PC down.", Switch:="shutdown")>
    Private Sub Shutdown()
        g.ShutdownPC(ShutdownMode.Shutdown)
    End Sub

    Sub SetEncoderControl(c As Control)
        pEncoder.Controls.Add(c)

        If pEncoder.Controls.Count > 1 Then
            Dim old = pEncoder.Controls(0)
            pEncoder.Controls.Remove(old)
            old.Dispose()
        End If
    End Sub

    Sub UpdateEncoderStateRelatedControls()
        llFilesize.Visible = Not p.VideoEncoder.QualityMode
        tbSize.Visible = Not p.VideoEncoder.QualityMode
        lBitrate.Visible = Not p.VideoEncoder.QualityMode
        tbBitrate.Visible = Not p.VideoEncoder.QualityMode
        lQualityText.Visible = p.VideoEncoder.IsCompCheckEnabled
        lQuality.Visible = p.VideoEncoder.IsCompCheckEnabled
        lCompCheckText.Visible = p.VideoEncoder.IsCompCheckEnabled
        lCompCheck.Visible = p.VideoEncoder.IsCompCheckEnabled
    End Sub

    <Command("Dialog | Open Source Files", "Dialog to open source files.")>
    Private Sub OpenSourceFiles()
        Dim td As New TaskDialog(Of String)
        td.MainInstruction = "Choose a method for opening a source."
        td.AddCommandLink("Single File", "DVD and Blu-ray discs can be ripped very easily using MakeMKV.", "single")
        td.AddCommandLink("Blu-ray folder", "Blu-ray folder directly on the Blu-ray drive or on the hard drive.", "blu")
        td.AddCommandLink("Merge Files", "Merge multiple source files.", "merge")
        td.AddCommandLink("File Batch", "Fully automated batch processing where every file is a separate encoding.", "batch")
        td.AddCommandLink("Directory Batch", "Directories are processed as separate encoding, containing files are merged.", "batch2")

        Select Case td.Show
            Case "single"
                Using d As New OpenFileDialog
                    d.SetFilter(FileTypes.Video)
                    d.SetInitDir(s.LastSourceDir)

                    If d.ShowDialog() = DialogResult.OK Then
                        OpenSingleFile(d.FileNames)
                    End If
                End Using
            Case "merge"
                Using f As New SourceFilesForm()
                    f.Mode = SourceInputMode.Combine
                    f.UpdateControls()

                    If f.ShowDialog() = DialogResult.OK Then
                        Refresh()

                        Select Case Filepath.GetExt(f.Files(0))
                            Case "mpg", "mpeg", "vob", "mpv", "m2v", "m2t", "ts", "pva", "trp"
                                OpenVideoSourceFiles(f.Files)
                            Case Else
                                Using proc As New Proc
                                    proc.Init("Merge source files")
                                    proc.Encoding = Encoding.UTF8
                                    proc.File = Packs.Mkvmerge.GetPath
                                    Dim outFile = Filepath.GetDirAndBase(f.Files(0)) + "_merged.mkv"
                                    proc.Arguments = "-o """ + outFile + """ """ + f.Files.Join(""" + """) + """"
                                    proc.AllowedExitCodes = {0, 1}
                                    proc.Start()

                                    If Not g.WasFileJustWritten(outFile) Then
                                        Log.Write("Error merged output file is missing", outFile)
                                        Exit Sub
                                    Else
                                        OpenVideoSourceFile(outFile)
                                    End If
                                End Using
                        End Select
                    End If
                End Using
            Case "batch"
                Using f As New SourceFilesForm()
                    If p.DefaultTargetName = "%source_dir_name%" Then
                        p.DefaultTargetName = "%source_name%"
                    End If

                    f.Mode = SourceInputMode.FileBatch
                    f.UpdateControls()

                    If f.ShowDialog() = DialogResult.OK Then
                        Refresh()

                        If p.SourceFiles.Count > 0 AndAlso
                            Not LoadTemplateWithSelectionDialog() Then

                            Exit Sub
                        End If

                        Dim tempPath = Paths.TemplateDir + "temp.srip"

                        If f.cbCreateJobs.Checked Then p.BatchMode = True

                        SaveProjectByPath(tempPath)

                        For Each i In f.Files
                            OpenProject(tempPath, False)
                            OpenVideoSourceFile(i)

                            If f.cbCreateJobs.Checked Then
                                Try
                                    g.SetTempDir()
                                Catch ex As Exception
                                    FileHelp.Delete(tempPath)
                                    Exit Sub
                                End Try

                                AddJob(False, Nothing)
                            End If
                        Next

                        OpenProject(tempPath, False)
                        FileHelp.Delete(tempPath)
                        UpdateRecentProjectsMenuItems()

                        If f.cbCreateJobs.Checked Then
                            OpenJobsDialog()
                        End If
                    End If
                End Using
            Case "batch2"
                Using f As New SourceFilesForm()
                    f.Mode = SourceInputMode.DirectoryBatch
                    f.UpdateControls()

                    If f.ShowDialog() = DialogResult.OK Then
                        Refresh()

                        If p.SourceFiles.Count > 0 AndAlso Not LoadTemplateWithSelectionDialog() Then
                            Exit Sub
                        End If

                        Dim tempPath = Paths.TemplateDir + "temp.srip"

                        If f.cbCreateJobs.Checked Then p.BatchMode = True

                        SaveProjectByPath(tempPath)

                        For Each i In f.DirTree.Paths
                            OpenProject(tempPath, False)
                            OpenVideoSourceFiles(GetSourceFilesFromDir(i))

                            If f.cbCreateJobs.Checked Then
                                Try
                                    g.SetTempDir()
                                Catch ex As Exception
                                    FileHelp.Delete(tempPath)
                                    Exit Sub
                                End Try

                                AddJob(False, Nothing)
                            End If
                        Next

                        OpenProject(tempPath, False)
                        FileHelp.Delete(tempPath)
                        UpdateRecentProjectsMenuItems()

                        If f.cbCreateJobs.Checked Then
                            OpenJobsDialog()
                        End If
                    End If
                End Using
            Case "blu"
                Using d As New FolderBrowserDialog
                    d.Description = "Please choose a Blu-ray source folder."
                    d.SetSelectedPath(s.Storage.GetString("last blu-ray source folder"))
                    d.ShowNewFolderButton = False

                    If d.ShowDialog = DialogResult.OK Then
                        s.Storage.SetString("last blu-ray source folder", d.SelectedPath)

                        Dim path = DirPath.AppendSeparator(d.SelectedPath)

                        If Directory.Exists(path + "BDMV") Then
                            path = path + "BDMV\"
                        End If

                        If Directory.Exists(path + "PLAYLIST") Then
                            path = path + "PLAYLIST\"
                        End If

                        If Not path.ToUpper.EndsWith("PLAYLIST\") Then
                            MsgWarn("No playlist directory found.")
                            Exit Sub
                        End If

                        Dim output = ProcessHelp.GetStandardOutput(
                            Packs.eac3to.GetPath, """" + path + """")

                        Log.Write("Process Blu-Ray folder using eac3to", """" + Packs.eac3to.GetPath + """ """ + path + """" + CrLf2)

                        output = output.Replace(VB6.vbBack, "")

                        Log.WriteLine(output)

                        Dim a = Regex.Split(output, "^\d+\)", RegexOptions.Multiline).ToList

                        If a(0) = "" Then a.RemoveAt(0)

                        Dim td2 As New TaskDialog(Of Integer)
                        td2.MainInstruction = "Please choose a playlist."

                        For Each i In a
                            If i.Contains(CrLf) Then
                                td2.AddCommandLink(i.Left(CrLf).Trim, i.Right(CrLf).TrimEnd, a.IndexOf(i) + 1)
                            End If
                        Next

                        If td2.Show() = 0 Then Exit Sub

                        Showeac3toDemuxForm(path, td2.SelectedValue)
                    End If
                End Using
        End Select
    End Sub

    Sub Showeac3toDemuxForm(playlistFolder As String, playlistID As Integer)
        Using f As New eac3toForm
            f.PlaylistFolder = playlistFolder
            f.PlaylistID = playlistID

            Dim workDir = DirPath.GetParent(DirPath.GetParent(playlistFolder))
            Dim movieName = DirPath.GetName(workDir)

            If movieName = "" Then movieName = "Untitled"

            movieName = InputBox.Show("Please enter the name of this movie.", "Movie Name", movieName)

            If Not Filepath.IsValidFileSystemName(movieName) Then
                movieName = Filepath.RemoveIllegalCharsFromName(movieName)
            End If

            If movieName = "" Then Exit Sub

            If Directory.Exists(p.TempDir) Then
                f.tbTempDir.Text = p.TempDir + movieName
            Else
                If workDir.StartsWith("\\") Then
                    Using d As New FolderBrowserDialog
                        d.Description = "Please choose a local directory for temporary files."

                        If d.ShowDialog = DialogResult.OK Then
                            workDir = d.SelectedPath
                        Else
                            Exit Sub
                        End If
                    End Using
                End If

                If workDir.StartsWith("\\") Then
                    Exit Sub
                End If

                Dim di As New DriveInfo(Path.GetPathRoot(workDir))

                If di.DriveType <> DriveType.Fixed Then
                    Using d As New FolderBrowserDialog
                        d.Description = "Please choose a directory on a local hard drive to save demuxed files."
                        d.SetSelectedPath(s.Storage.GetString("last Blu-ray target dir"))

                        If d.ShowDialog = DialogResult.OK Then
                            s.Storage.SetString("last Blu-ray target dir", DirPath.GetParent(d.SelectedPath))
                            f.tbTempDir.Text = DirPath.AppendSeparator(d.SelectedPath)
                        Else
                            Exit Sub
                        End If
                    End Using
                Else
                    f.tbTempDir.Text = workDir + "Temp"
                End If
            End If

            If f.ShowDialog() = DialogResult.OK Then
                Try
                    Dim di2 As New DriveInfo(Path.GetPathRoot(f.OutputFolder))

                    If di2.AvailableFreeSpace / 1024 ^ 3 < 50 Then
                        If MsgQuestion("The drive has only " & CInt(di2.AvailableFreeSpace / 1024 ^ 3) &
                                       " GB free space available. Depending on Blu-ray disc and processing options this might be insufficient, continue anyway?") = DialogResult.Cancel Then
                            Exit Sub
                        End If
                    End If

                    If Not Directory.Exists(f.OutputFolder) Then
                        Try
                            Directory.CreateDirectory(f.OutputFolder)
                        Catch ex As Exception
                            MsgError("Failed to create directory for temporary files.", f.OutputFolder)
                            Exit Sub
                        End Try
                    End If

                    Using proc As New Proc
                        proc.TrimChars = {"-"c, " "c}
                        proc.RemoveChars = {CChar(VB6.vbBack)}
                        proc.Init("Demux M2TS using eac3to", "analyze: ", "process: ")
                        proc.File = Packs.eac3to.GetPath
                        proc.Process.StartInfo.Arguments = f.GetArgs(
                            """" + playlistFolder + """ " & playlistID & ")", movieName)

                        Try
                            proc.Start()
                        Catch ex As Exception
                            ProcessForm.CloseProcessForm()
                            g.ShowException(ex)
                            Exit Sub
                        End Try
                    End Using

                    Dim fs = f.OutputFolder + movieName + "." + f.cbVideoOutput.Text.ToLower

                    If File.Exists(fs) Then
                        p.TempDir = f.OutputFolder
                        OpenVideoSourceFile(fs)
                    End If
                Finally
                    ProcessForm.CloseProcessForm()
                End Try
            End If
        End Using
    End Sub

    Private Function GetSourceFilesFromDir(path As String) As String()
        For Each i In FileTypes.Video
            Dim files = Directory.GetFiles(path, "*." + i)
            Array.Sort(files)

            If files.Length > 0 Then
                Return files
            End If
        Next

        Return Nothing
    End Function

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case 800 'WM_DWMCOLORIZATIONCOLORCHANGED
                If ToolStripRendererEx.IsAutoRenderMode Then
                    ToolStripRendererEx.InitColors(s.ToolStripRenderMode)
                    SetMenuStyle()
                    MenuStrip.Refresh()
                End If
        End Select

        MyBase.WndProc(m)
    End Sub

    Sub tbSource_DoubleClick() Handles tbSourceFile.DoubleClick
        Using d As New SWF.OpenFileDialog
            d.SetFilter(FileTypes.Video)
            d.Multiselect = True
            d.SetInitDir(s.LastSourceDir)

            If d.ShowDialog() = DialogResult.OK Then
                Refresh()

                Dim l As New List(Of String)(d.FileNames)
                l.Sort()
                OpenSingleFile(l)
            End If
        End Using
    End Sub

    Private Sub LoadAudioProfile0(profile As Profile)
        Dim file = p.Audio0.File
        Dim delay = p.Audio0.Delay
        Dim language = p.Audio0.Language
        Dim stream = p.Audio0.Stream
        Dim streams = p.Audio0.Streams

        p.Audio0 = DirectCast(ObjectHelp.GetCopy(profile), AudioProfile)

        p.Audio0.File = file
        p.Audio0.Language = language
        p.Audio0.Stream = stream
        p.Audio0.Streams = streams
        p.Audio0.Delay = delay

        llAudioProfile0.Text = g.ConvertPath(p.Audio0.Name)
        SetBitrateMuxAudioProfile(p.Audio0)
        lAudioBitrate.Text = CInt(Calc.GetAudioBitrate).ToString

        If Not p.VideoEncoder.QualityMode Then
            tbSize_TextChanged()
        End If

        Assistant()
    End Sub

    Private Sub LoadAudioProfile1(profile As Profile)
        Dim file = p.Audio1.File
        Dim delay = p.Audio1.Delay
        Dim language = p.Audio1.Language
        Dim stream = p.Audio1.Stream
        Dim streams = p.Audio1.Streams

        p.Audio1 = DirectCast(ObjectHelp.GetCopy(profile), AudioProfile)

        p.Audio1.File = file
        p.Audio1.Language = language
        p.Audio1.Stream = stream
        p.Audio1.Streams = streams
        p.Audio1.Delay = delay

        llAudioProfile1.Text = g.ConvertPath(p.Audio1.Name)
        SetBitrateMuxAudioProfile(p.Audio1)
        lAudioBitrate.Text = CInt(Calc.GetAudioBitrate).ToString
        tbSize_TextChanged()
        Assistant()
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
        sb.Text = "Please choose a profile."

        If Not currentProfile Is Nothing Then
            sb.AddItem("Current Project", currentProfile)
        End If

        For Each i In AudioProfile.GetDefaults()
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK Then
            Return sb.SelectedItem
        End If

        Return Nothing
    End Function

    Sub tbAudioFile0_DoubleClick() Handles tbAudioFile0.DoubleClick
        Using d As New SWF.OpenFileDialog
            d.SetFilter(FileTypes.Audio)
            d.SetInitDir(p.TempDir, s.LastSourceDir)

            If d.ShowDialog() = DialogResult.OK Then
                tbAudioFile0.Text = d.FileName
            End If
        End Using
    End Sub

    Sub tbAudioFile1_DoubleClick() Handles tbAudioFile1.DoubleClick
        Using d As New SWF.OpenFileDialog
            d.SetFilter(FileTypes.Audio)
            d.SetInitDir(p.TempDir, s.LastSourceDir)

            If d.ShowDialog() = DialogResult.OK Then
                tbAudioFile1.Text = d.FileName
            End If
        End Using
    End Sub

    Private Sub lTip_Click() Handles lTip.Click
        If Not AssistantMethod Is Nothing Then
            AssistantMethod.Invoke()
            Assistant()
        End If
    End Sub

    Sub SetTargetLength(seconds As Integer)
        p.TargetSeconds = seconds
        lTargetLength.Text = GetLengthString(seconds)
        SetBitrateMuxAudioProfile(p.Audio0)
        SetBitrateMuxAudioProfile(p.Audio1)
        tbSize_TextChanged()
    End Sub

    Function GetLengthString(seconds As Integer) As String
        Dim sec As Integer = seconds Mod 60

        If sec <= 9 Then
            Return (seconds \ 60).ToString + ":0" + sec.ToString
        Else
            Return (seconds \ 60).ToString + ":" + sec.ToString
        End If
    End Function

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If IsSaveCanceled() Then
            e.Cancel = True
            Exit Sub
        End If

        Hide()
        SaveSettings()
        g.RaiseApplicationEvent(ApplicationEvent.ApplicationExit)
    End Sub

    Sub SaveSettings()
        SafeSerialization.Serialize(s, Paths.SettingsFile)
    End Sub

    Private Sub pEncoder_MouseLeave() Handles pEncoder.MouseLeave
        Assistant()
    End Sub

    Private Sub AudioEdit0ToolStripMenuItemClick()
        p.Audio0.EditProject()
        UpdateAudioMenu()
        SetBitrateMuxAudioProfile(p.Audio1)
        tbSize_TextChanged()
        llAudioProfile0.Text = g.ConvertPath(p.Audio0.Name)
    End Sub

    Private Sub AudioEdit1ToolStripMenuItemClick()
        p.Audio1.EditProject()
        UpdateAudioMenu()
        SetBitrateMuxAudioProfile(p.Audio1)
        tbSize_TextChanged()
        llAudioProfile1.Text = g.ConvertPath(p.Audio1.Name)
    End Sub

    Private Sub AudioSource0ToolStripMenuItemClick()
        tbAudioFile0_DoubleClick()
    End Sub

    Private Sub AudioSource1ToolStripMenuItemClick()
        tbAudioFile1_DoubleClick()
    End Sub

    <Command("Perform | Reset a specific setting", "Shows a dialog allowing to reset various settings.")>
    Sub ResetSettings()
        Dim sb As New SelectionBox(Of String)

        sb.Title = "Reset Settings"
        sb.Text = "Please choose a setting to reset."

        Dim appSettings As New ApplicationSettings
        appSettings.Init()

        For Each i In appSettings.Versions.Keys
            sb.AddItem(i)
        Next

        sb.Items.Sort()

        If sb.Show = DialogResult.OK Then
            s.Versions(sb.SelectedItem) = 0
            MsgInfo("Will be reseted on next startup.")
        End If
    End Sub

    <Command("Dialog | Audio Profiles", "Dialog to manage audio profiles.")>
    Sub OpenAudioProfilesDialog(<DispName("Track Number")> number As Integer)
        Dim f As ProfilesForm

        If number = 0 Then
            f = New ProfilesForm("Audio Profiles", s.AudioProfiles, AddressOf LoadAudioProfile0, AddressOf GetAudioProfile0, AddressOf AudioProfile.GetDefaults)
        Else
            f = New ProfilesForm("Audio Profiles", s.AudioProfiles, AddressOf LoadAudioProfile1, AddressOf GetAudioProfile1, AddressOf AudioProfile.GetDefaults)
        End If

        f.ShowDialog()
        f.Dispose()

        UpdateAudioMenu()

        PopulateProfileMenu(DynamicMenuItemID.Audio1Profiles)
        PopulateProfileMenu(DynamicMenuItemID.Audio2Profiles)
    End Sub

    Private Sub LoadMainMenuDynamic()
        PopulateProfileMenu(DynamicMenuItemID.EncoderProfiles)
        PopulateProfileMenu(DynamicMenuItemID.MuxerProfiles)
        PopulateProfileMenu(DynamicMenuItemID.Audio1Profiles)
        PopulateProfileMenu(DynamicMenuItemID.Audio2Profiles)
        PopulateProfileMenu(DynamicMenuItemID.FilterSetupProfiles)
    End Sub

    Private Sub UpdateAudioMenu()
        AudioMenu0.Items.Clear()
        AudioMenu1.Items.Clear()

        g.PopulateProfileMenu(AudioMenu0.Items, s.AudioProfiles, Sub() OpenAudioProfilesDialog(0), AddressOf LoadAudioProfile0)
        g.PopulateProfileMenu(AudioMenu1.Items, s.AudioProfiles, Sub() OpenAudioProfilesDialog(1), AddressOf LoadAudioProfile1)
    End Sub

    Sub LoadMuxer(profile As Profile)
        p.VideoEncoder.LoadMuxer(profile)
    End Sub

    Private Sub AviSynthListView_ScriptChanged() Handles AviSynthListView.ScriptChanged
        If Not IsLoading Then
            If g.IsValidSource(False) Then
                UpdateSourceParameters()
                SetTargetLength(p.AvsDoc.GetSeconds)
            End If

            Assistant()
        End If
    End Sub

    Sub UpdateFilters()
        AviSynthListView.Load()

        If g.IsValidSource(False) Then
            SetTargetLength(p.AvsDoc.GetSeconds)
        End If
    End Sub

    Private Sub tbTargetFile_Validating(sender As Object, e As CancelEventArgs) Handles tbTargetFile.Validating
        Assistant() 'script path is based on target path
    End Sub

    Sub tbTargetPath_TextChanged() Handles tbTargetFile.TextChanged
        If tbTargetFile.Text <> "" AndAlso Not Filepath.IsValidFileSystemName(
            Filepath.GetName(tbTargetFile.Text)) Then

            MsgWarn("Filename contains invalid characters.")
            tbTargetFile.Text = p.TargetFile
        End If

        If tbTargetFile.Text.ContainsUnicode Then
            MsgWarn(Strings.NoUnicode)
            tbTargetFile.Text = p.TargetFile
        End If

        p.Name = Filepath.GetBase(tbTargetFile.Text)
        p.TargetFile = tbTargetFile.Text
    End Sub

    Sub tbTargetPath_DoubleClick() Handles tbTargetFile.DoubleClick
        Using d As New SWF.SaveFileDialog
            d.FileName = Filepath.GetBase(p.TargetFile)
            d.SetInitDir(Filepath.GetDir(p.TargetFile))

            If d.ShowDialog() = DialogResult.OK Then
                Dim ext = p.VideoEncoder.Muxer.GetExtension

                If d.FileName.ToLower.EndsWith(ext) Then
                    tbTargetFile.Text = d.FileName
                Else
                    tbTargetFile.Text = d.FileName + ext
                End If

                Assistant()
            End If
        End Using
    End Sub

    Private Sub AviSynthListView_DoubleClick() Handles AviSynthListView.DoubleClick
        AviSynthListView.ShowEditor()
    End Sub

    Private Sub gbFilters_MenuClick() Handles lgbFilters.LinkClick
        AviSynthListView.UpdateMenu()
        AviSynthListView.ContextMenuStrip.Show(lgbFilters, 0, 16)
    End Sub

    Private Sub gbEncoder_LinkClick() Handles lgbEncoder.LinkClick
        EncoderMenu.Items.Clear()
        g.PopulateProfileMenu(EncoderMenu.Items, s.VideoEncoderProfiles, AddressOf OpenEncoderProfilesDialog, AddressOf LoadVideoEncoder)
        EncoderMenu.Show(lgbEncoder, 0, 16)
    End Sub

    Private Sub llSize_LinkClicked() Handles llFilesize.LinkClicked
        UpdateSizeMenu()
        SizeContextMenuStrip.Show(llFilesize, 0, 16)
    End Sub

    Private Sub lAudioProfile0_LinkClicked() Handles llAudioProfile0.LinkClicked
        AudioMenu0.Show(llAudioProfile0, 0, 16)
    End Sub

    Private Sub lAudioProfile1_LinkClicked() Handles llAudioProfile1.LinkClicked
        AudioMenu1.Show(llAudioProfile1, 0, 16)
    End Sub

    Private Sub llContainer_LinkClicked() Handles llMuxer.LinkClicked
        ContainerMenu.Items.Clear()
        g.PopulateProfileMenu(ContainerMenu.Items, s.MuxerProfiles, AddressOf OpenMuxerProfilesDialog, AddressOf LoadMuxer)
        ContainerMenu.Show(llMuxer, 0, 16)
    End Sub

    Private Sub gbResize_LinkClick() Handles lgbResize.LinkClick
        Dim cms = TextCustomMenu.GetMenu(s.TargetImageSizeMenu, lgbResize.ll, components, AddressOf TargetImageMenuClick)

        Dim helpUrl = If(g.IsCulture("de"),
                 "http://encodingwissen.de/videobild/zielaufloesung",
                 "http://www.doom9.org/index.html?/aspectratios.htm")

        Dim helpAction = Sub() g.ShellExecute(helpUrl)

        cms.Items.Add(New ToolStripSeparator)

        cms.Items.Add(New ActionMenuItem("Image Options...", Sub() OpenOptionsDialog("Image")))
        cms.Items.Add(New ActionMenuItem("Edit Menu...", Sub() s.TargetImageSizeMenu = TextCustomMenu.EditMenu(s.TargetImageSizeMenu, "Target Image Help...", ApplicationSettings.GetDefaultTargetImageSizeMenu, Me)))
        cms.Items.Add(New ActionMenuItem("Help...", helpAction))

        cms.Show(lgbResize, 0, lgbResize.ll.Height)
    End Sub

    Private Sub llSourceParText_Click(sender As Object, e As EventArgs) Handles llSourceParText.Click
        Dim cms = TextCustomMenu.GetMenu(s.SourceAspectRatioMenu, llSourceParText, components, AddressOf SourceAspectRatioMenuClick)

        Dim helpUrl = If(g.IsCulture("de"),
                         "http://encodingwissen.de/video/anamorph-quelle.html",
                         "http://www.doom9.org/index.html?/capture/par.html")

        Dim helpAction = Sub() g.ShellExecute(helpUrl)

        cms.Items.Insert(0, New ToolStripSeparator)
        cms.Items.Insert(0, New ActionMenuItem("Automatic 4:3", Sub() SetAutoAspectRatio(False)))
        cms.Items.Insert(0, New ActionMenuItem("Automatic 16:9", Sub() SetAutoAspectRatio(True)))

        cms.Items.Add(New ToolStripSeparator)

        cms.Items.Add(New ActionMenuItem("Image Options...", Sub() OpenOptionsDialog("Image")))
        cms.Items.Add(New ActionMenuItem("Edit Menu...", Sub() s.SourceAspectRatioMenu = TextCustomMenu.EditMenu(s.SourceAspectRatioMenu, "Source Image Help...", ApplicationSettings.GetDefaultSourceAspectRatioMenu, Me)))
        cms.Items.Add(New ActionMenuItem("Help...", helpAction))

        cms.Font = New Font("Consolas", 9)
        cms.Show(llSourceParText, 0, llSourceParText.Height)
    End Sub

    Sub SourceAspectRatioMenuClick(value As String)
        value = Macro.Solve(value)

        If value.IsSingle Then
            Dim dar = CSng(value)

            If dar > 1 AndAlso dar < 3 Then
                p.CustomDAR = value
                p.CustomPAR = ""
                Assistant()
                Exit Sub
            End If
        End If

        Dim m = Regex.Match(value, Strings.ParRegexPattern)

        If m.Success Then
            p.CustomDAR = ""
            p.CustomPAR = value
            Assistant()
            Exit Sub
        End If

        MsgWarn("Invalid format, please use:" + CrLf2 + "4/3 or 4:3 or " & 1.333333)
        s.SourceAspectRatioMenu = TextCustomMenu.EditMenu(s.SourceAspectRatioMenu, Me)
    End Sub

    Sub TargetImageMenuClick(value As String)
        g.EnableFilter("Resize")

        value = Macro.Solve(value)

        If value.IsInt Then
            SetTargetImageSizeByPixel(CInt(value))
            Exit Sub
        End If

        Dim a = value.SplitNoEmptyAndWhiteSpace("x")

        If a.Length = 2 AndAlso a(0).IsInt AndAlso a(1).IsInt Then
            SetTargetImageSize(a(0).ToInt, a(1).ToInt)
            Exit Sub
        End If

        MsgWarn("Invalid format")
        s.TargetImageSizeMenu = TextCustomMenu.EditMenu(s.TargetImageSizeMenu, Me)
    End Sub

    Sub SetAutoAspectRatio(isAnamorphic As Boolean)
        p.SourceAnamorphic = isAnamorphic
        p.CustomPAR = ""
        p.CustomDAR = ""

        If p.AvsDoc.IsFilterActive("Resize)") Then
            SetTargetImageSize(p.TargetWidth, 0)
        End If

        Assistant()
    End Sub

    Sub SetTip()
        With TipProvider
            .SetTip("Target Display Aspect Ratio", lDAR, lDarText)
            .SetTip("Target Pixel Aspect Ratio", lPAR, lParText)
            .SetTip("Target Storage Aspect Ratio", lSAR, lSarText)
            .SetTip("Imagesize in pixel (width x height)", lPixel, lPixelText)
            .SetTip("Zoom factor between source and target image size", lZoom, lZoomText)
            .SetTip("Press Ctrl while using the slider to resize anamorphic.", tbResize)
            .SetTip("Aspect Ratio Error", lAspectRatioError, lAspectRatioErrorText)
            .SetTip("Source Framerate (frames/seconds)", lSourceFramerate, lSourceFramerateText)
            .SetTip("Source Length (minutes:seconds)", lbSourceLength, lbSourceLengthText)
            .SetTip("Storage source image size in pixel (width/height)", lSourceImageSize, lSourceImageSizeText)
            .SetTip("Cropped image size", lCrop, lCropText)
            .SetTip("Source Display Aspect Ratio", lSourceDar, lSourceDarText)
            .SetTip("Source Pixel Aspect Ratio", lSourcePAR, llSourceParText)
            .SetTip("Target Video Bitrate in Kbps (Up/Down key)", tbBitrate, lBitrate)
            .SetTip("Target Length (minutes:seconds)", lTargetLength, lTargetLengthText)
            .SetTip("Compressibility value", lCompCheck, lCompCheckText)
            .SetTip("Audio Bitrate of both tracks in Kbps", lAudioBitrate, lAudioBitrateText)
            .SetTip("Target Image Width in pixel (Up/Down key)", tbTargetWidth, lTargetWidth)
            .SetTip("Target Image Height in pixel (Up/Down key)", tbTargetHeight, lTargetHeight)
            .SetTip("Target File Size (Up/Down key)", tbSize)
            .SetTip("Source file", tbSourceFile)
            .SetTip("Target file", tbTargetFile)
            .SetTip("Opens audio settings for the current project/template", llEditAudio0, llEditAudio1)
            .SetTip("Shows audio profiles", llAudioProfile0)
            .SetTip("Shows audio profiles", llAudioProfile1)
            .SetTip("Shows a menu with Container/Muxer profiles", llMuxer)
            .SetTip("Shows a menu with video encoder profiles", lgbEncoder.ll)
            .SetTip("Shows a menu with AviSynth filter options", lgbFilters.ll)
        End With
    End Sub

    Sub UpdateSourceParameters()
        If Not p.SourceAviSynthDocument Is Nothing Then
            Try
                p.SourceWidth = p.SourceAviSynthDocument.GetSize.Width
                p.SourceHeight = p.SourceAviSynthDocument.GetSize.Height
                p.SourceSeconds = CInt(p.SourceAviSynthDocument.GetFrames / p.SourceAviSynthDocument.GetFramerate)
                p.SourceFramerate = p.SourceAviSynthDocument.GetFramerate
                p.SourceFrames = p.SourceAviSynthDocument.GetFrames
            Catch ex As Exception
                MsgError("Source filter returned invalid parameters", Macro.Solve(p.SourceAviSynthDocument.GetScript))
                Throw New AbortException()
            End Try
        End If
    End Sub

    Private Sub tbSource_TextChanged(sender As Object, e As EventArgs) Handles tbSourceFile.TextChanged
        If Not BlockSourceTextBoxTextChanged Then
            If File.Exists(tbSourceFile.Text) Then
                OpenVideoSourceFile(tbSourceFile.Text)
            End If
        End If
    End Sub

    Private Sub gbTarget_LinkClick() Handles lgbTarget.LinkClick
        UpdateTargetFileMenu()
        TargetFileMenu.Show(lgbTarget, 0, lgbTarget.ll.Height)
    End Sub

    Private Sub gbSource_MenuClick() Handles lgbSource.LinkClick
        UpdateSourceFileMenu()

        If File.Exists(p.SourceFile) Then
            SourceFileMenu.Show(lgbSource, 0, lgbSource.ll.Height)
        Else
            OpenSourceFiles()
        End If
    End Sub

    Private Sub tbTargetFile_MouseDown(sender As Object, e As MouseEventArgs) Handles tbTargetFile.MouseDown
        If e.Button = MouseButtons.Right Then
            UpdateTargetFileMenu()
        End If
    End Sub

    Private Sub tbSourceFile_MouseDown(sender As Object, e As MouseEventArgs) Handles tbSourceFile.MouseDown
        If e.Button = MouseButtons.Right Then
            UpdateSourceFileMenu()
        End If
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

    Sub UpdateAudioFileMenu(m As ContextMenuStrip,
                            a As Action,
                            ap As AudioProfile,
                            tb As TextBox)
        m.Items.Clear()
        Dim exist = File.Exists(ap.File)

        If ap.Streams.Count > 0 Then
            For Each i In ap.Streams
                Dim temp = i

                Dim menuAction = Sub()
                                     If ap.File <> p.NativeSourceFile Then
                                         tb.Text = p.NativeSourceFile
                                     End If

                                     tb.Text = temp.Name + " (" + Filepath.GetExt(ap.File) + ")"
                                     ap.Stream = temp
                                     UpdateAudio(ap)
                                 End Sub

                m.Items.Add(New ActionMenuItem(i.Name, menuAction, Nothing))
            Next

            m.Items.Add("-")
        End If

        If p.TempDir <> "" AndAlso Directory.Exists(p.TempDir) Then
            Dim audioFiles = Directory.GetFiles(p.TempDir).Where(
                Function(audioPath) FileTypes.Audio.Contains(Filepath.GetExt(audioPath)))

            If audioFiles.Count > 0 Then
                For Each i In audioFiles
                    Dim temp = i
                    Dim menuAction = Sub() tb.Text = temp
                    m.Items.Add(New ActionMenuItem(Filepath.GetName(i), menuAction))
                Next

                m.Items.Add("-")
            End If
        End If

        m.Items.Add(New ActionMenuItem("Open", a, "Change the audio source file."))
        m.Items.Add(New ActionMenuItem("Play audio", Sub() PlayAudio(ap), "Plays the audio source file with MPC.", exist))
        m.Items.Add(New ActionMenuItem("Play audio and video", Sub() g.PlayScript(ap), "Plays the audio source file together with the AviSynth script.", exist))
        m.Items.Add(New ActionMenuItem("MediaInfo", Sub() g.DefaultCommands.ShowMediaInfo(ap.File), "Show MediaInfo for the audio source file.", exist))
        m.Items.Add(New ActionMenuItem("Explore", Sub() g.OpenDirAndSelectFile(ap.File, Handle), "Open the audio source file directory with Windows Explorer.", exist))
        m.Items.Add("-")
        m.Items.Add(New ActionMenuItem("Copy Path", Sub() Clipboard.SetText(ap.File), Nothing, tb.Text <> ""))
        m.Items.Add(New ActionMenuItem("Copy Selection", Sub() tb.Copy(), Nothing, tb.Text <> ""))
        m.Items.Add(New ActionMenuItem("Paste", Sub() tb.Paste(), Nothing, Clipboard.GetText.Trim <> ""))
    End Sub

    Sub PlayAudio(ap As AudioProfile)
        g.Play(ap.File)
    End Sub

    Sub UpdateTargetFileMenu()
        TargetFileMenu.Items.Clear()
        TargetFileMenu.Items.Add(New ActionMenuItem("Edit...", AddressOf tbTargetPath_DoubleClick, "Change the path of the target file."))
        TargetFileMenu.Items.Add(New ActionMenuItem("Play...", Sub() g.Play(p.TargetFile), "Play the target file.", File.Exists(p.TargetFile)))
        TargetFileMenu.Items.Add(New ActionMenuItem("MediaInfo...", Sub() g.DefaultCommands.ShowMediaInfo(p.TargetFile), "Show MediaInfo for the target file.", File.Exists(p.TargetFile)))
        TargetFileMenu.Items.Add(New ActionMenuItem("Explore...", Sub() g.OpenDirAndSelectFile(p.TargetFile, Handle), "Open the target file directory with Windows Explorer.", Directory.Exists(Filepath.GetDir(p.TargetFile))))
        TargetFileMenu.Items.Add("-")
        TargetFileMenu.Items.Add(New ActionMenuItem("Copy", Sub() tbTargetFile.Copy(), "", tbTargetFile.Text <> ""))
        TargetFileMenu.Items.Add(New ActionMenuItem("Paste", Sub() tbTargetFile.Paste(), "", Clipboard.GetText.Trim <> ""))
    End Sub

    Sub UpdateSourceFileMenu()
        SourceFileMenu.Items.Clear()
        Dim isIndex = FileTypes.VideoIndex.Contains(Filepath.GetExt(p.SourceFile))

        SourceFileMenu.Items.Add(New ActionMenuItem("Open...", AddressOf OpenSourceFiles, "Open source files"))
        SourceFileMenu.Items.Add(New ActionMenuItem("Play...", Sub() g.Play(p.SourceFile), "Play the source file.", File.Exists(p.SourceFile) AndAlso Not isIndex))
        SourceFileMenu.Items.Add(New ActionMenuItem("MediaInfo...", Sub() g.DefaultCommands.ShowMediaInfo(p.SourceFile), "Show MediaInfo for the source file.", File.Exists(p.SourceFile) AndAlso Not isIndex))
        SourceFileMenu.Items.Add(New ActionMenuItem("Explore...", Sub() g.OpenDirAndSelectFile(p.SourceFile, Handle), "Open the source file directory with Windows Explorer.", File.Exists(p.SourceFile)))
        SourceFileMenu.Items.Add("-")
        SourceFileMenu.Items.Add(New ActionMenuItem("Copy", Sub() tbSourceFile.Copy(), "Copies the selected text to the clipboard.", tbSourceFile.Text <> ""))
        SourceFileMenu.Items.Add(New ActionMenuItem("Paste", Sub() tbSourceFile.Paste(), "Copies the full source file path to the clipboard.", Clipboard.GetText.Trim <> ""))
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        'on small screensizes like netbooks dialogs get cut at screensize
        If lgbEncoder.Right > ClientSize.Width Then
            ClientSize = New Size(lgbEncoder.Right + lgbFilters.Left, gbAssistant.Bottom + lgbFilters.Left)
        End If
    End Sub

    <Command("Perform | Show Command Prompt", "Shows a command prompt with the temp directory of the current project.")>
    Sub ShowCommandPrompt()
        Dim batchCode = "@echo off" + CrLf

        For Each i In Packs.Packages.Values
            Dim dir = i.GetDir

            If Directory.Exists(dir) AndAlso
                Not dir.ToLower.Contains("system32") AndAlso
                Not batchCode.Contains(dir) Then

                batchCode += "@set PATH=" + dir + ";%PATH%" + CrLf
            End If
        Next

        Dim batchPath = CommonDirs.Temp + Guid.NewGuid.ToString + ".bat"
        File.WriteAllText(batchPath, batchCode, Encoding.GetEncoding(850))
        AddHandler g.MainForm.Disposed, Sub() FileHelp.Delete(batchPath)

        Dim batchProcess As New Process
        batchProcess.StartInfo.FileName = "cmd.exe"
        batchProcess.StartInfo.Arguments = "/k """ + batchPath + """"
        batchProcess.StartInfo.WorkingDirectory = p.TempDir
        batchProcess.Start()
    End Sub

    <Command("Perform | Batch Generate Thumbnails", "Generates a thumbnails image.")>
    Sub BatchGenerateThumbnails()
        Using fd As New OpenFileDialog
            fd.Title = "Select files"
            fd.SetFilter(FileTypes.Video)
            fd.Multiselect = True

            If fd.ShowDialog = DialogResult.OK Then
                Using f As New SimpleSettingsForm("Thumbnails Options")
                    f.Size = New Size(500, 300)

                    Dim ui = f.SimpleUI

                    Dim page = ui.CreateFlowPage("main page")

                    Dim nb = ui.AddNumericBlock(page)
                    nb.Label.Text = "Thumbnail Width:"
                    nb.Label.Offset = 7
                    nb.NumEdit.Init(260, 4000, 10)
                    nb.NumEdit.Value = s.ThumbnailWidth
                    nb.NumEdit.SaveAction = Sub(value) s.ThumbnailWidth = CInt(value)

                    nb = ui.AddNumericBlock(page)
                    nb.Label.Text = "Rows:"
                    nb.Label.Offset = 7
                    nb.NumEdit.Init(1, 1000, 1)
                    nb.NumEdit.Value = s.ThumbnailRows
                    nb.NumEdit.SaveAction = Sub(value) s.ThumbnailRows = CInt(value)

                    nb = ui.AddNumericBlock(page)
                    nb.Label.Text = "Columns:"
                    nb.Label.Offset = 7
                    nb.NumEdit.Init(1, 1000, 1)
                    nb.NumEdit.Value = s.ThumbnailColumns
                    nb.NumEdit.SaveAction = Sub(value) s.ThumbnailColumns = CInt(value)

                    If f.ShowDialog() = DialogResult.OK Then
                        ui.Save()

                        Dim tmp = ""

                        SyncLock p.Log
                            tmp = p.Log.ToString
                        End SyncLock

                        ProcessForm.ShowForm(False)

                        For Each i In fd.FileNames
                            Try
                                Thumbnails.SaveThumbnails(i)
                            Catch ex As Exception
                                g.ShowException(ex)
                            End Try
                        Next

                        SyncLock p.Log
                            p.Log.Length = 0
                            p.Log.Append(tmp)
                        End SyncLock

                        ProcessForm.CloseProcessForm()
                        g.ShellExecute(Filepath.GetDir(fd.FileName))
                    End If
                End Using
            End If
        End Using
    End Sub

    <Command("Perform | Show MediaInfo of all files in a folder", "Presents MediaInfo of all files in a folder in a list view.")>
    Sub OpenMediaInfoFolderView()
        Using d As New FolderBrowserDialog
            d.Description = "Please choose a folder to be viewed."
            d.ShowNewFolderButton = False
            d.SetSelectedPath(s.Storage.GetString("MediaInfo Folder View folder"))

            If d.ShowDialog = DialogResult.OK Then
                s.Storage.SetString("MediaInfo Folder View folder", d.SelectedPath)
                Dim f As New MediaInfoFolderViewForm(DirPath.AppendSeparator(d.SelectedPath))
                f.Show()
            End If
        End Using
    End Sub

    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            Return True
        End Get
    End Property
End Class