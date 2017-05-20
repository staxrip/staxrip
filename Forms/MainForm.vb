Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Globalization
Imports System.Reflection
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
    Public WithEvents llEditAudio1 As SimpleLinkLabel
    Public WithEvents llEditAudio0 As SimpleLinkLabel
    Public WithEvents bNext As System.Windows.Forms.Button
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
    Public WithEvents lDarText As System.Windows.Forms.Label
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
    Public WithEvents llAudioProfile0 As SimpleLinkLabel
    Public WithEvents llAudioProfile1 As SimpleLinkLabel
    Public WithEvents pEncoder As System.Windows.Forms.Panel
    Public WithEvents AviSynthListView As StaxRip.AviSynthListView
    Public WithEvents llFilesize As SimpleLinkLabel
    Public WithEvents llMuxer As SimpleLinkLabel
    Public WithEvents lPAR As StaxRip.UI.LabelEx
    Public WithEvents lParText As StaxRip.UI.LabelEx
    Public WithEvents lAspectRatioError As StaxRip.UI.LabelEx
    Public WithEvents lAspectRatioErrorText As StaxRip.UI.LabelEx
    Public WithEvents gbAudio As System.Windows.Forms.GroupBox
    Public WithEvents llSourceParText As SimpleLinkLabel
    Public WithEvents lSourcePAR As System.Windows.Forms.Label
    Public WithEvents lSourceDar As System.Windows.Forms.Label
    Public WithEvents lSourceDarText As System.Windows.Forms.Label
    Public WithEvents lSAR As StaxRip.UI.LabelEx
    Public WithEvents lSarText As StaxRip.UI.LabelEx
    Public WithEvents TipProvider As StaxRip.UI.TipProvider

    '<System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.bNext = New System.Windows.Forms.Button()
        Me.llEditAudio0 = New SimpleLinkLabel()
        Me.gbAssistant = New System.Windows.Forms.GroupBox()
        Me.lTip = New System.Windows.Forms.Label()
        Me.llEditAudio1 = New SimpleLinkLabel()
        Me.gbAudio = New System.Windows.Forms.GroupBox()
        Me.llAudioProfile1 = New SimpleLinkLabel()
        Me.llAudioProfile0 = New SimpleLinkLabel()
        Me.tbAudioFile0 = New StaxRip.UI.TextBoxEx()
        Me.tbAudioFile1 = New StaxRip.UI.TextBoxEx()
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.lgbTarget = New StaxRip.UI.LinkGroupBox()
        Me.llFilesize = New SimpleLinkLabel()
        Me.tbTargetSize = New System.Windows.Forms.TextBox()
        Me.lTarget2 = New System.Windows.Forms.Label()
        Me.lBitrate = New System.Windows.Forms.Label()
        Me.tbBitrate = New System.Windows.Forms.TextBox()
        Me.lTarget1 = New System.Windows.Forms.Label()
        Me.tbTargetFile = New StaxRip.UI.TextBoxEx()
        Me.lgbSource = New StaxRip.UI.LinkGroupBox()
        Me.lCrop = New System.Windows.Forms.Label()
        Me.lSourceDar = New System.Windows.Forms.Label()
        Me.lSourceDarText = New System.Windows.Forms.Label()
        Me.lSourcePAR = New System.Windows.Forms.Label()
        Me.llSourceParText = New SimpleLinkLabel()
        Me.lCropText = New System.Windows.Forms.Label()
        Me.lSource2 = New System.Windows.Forms.Label()
        Me.lSource1 = New System.Windows.Forms.Label()
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
        Me.llMuxer = New SimpleLinkLabel()
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
        Me.bNext.Location = New System.Drawing.Point(903, 37)
        Me.bNext.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.bNext.Name = "bNext"
        Me.bNext.Size = New System.Drawing.Size(70, 35)
        Me.bNext.TabIndex = 39
        Me.bNext.Text = "Next"
        '
        'llEditAudio0
        '
        Me.llEditAudio0.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llEditAudio0.Location = New System.Drawing.Point(929, 24)
        Me.llEditAudio0.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llEditAudio0.Name = "llEditAudio0"
        Me.llEditAudio0.Size = New System.Drawing.Size(48, 31)
        Me.llEditAudio0.TabIndex = 20
        Me.llEditAudio0.TabStop = True
        Me.llEditAudio0.Text = "Edit"
        Me.llEditAudio0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbAssistant
        '
        Me.gbAssistant.Controls.Add(Me.lTip)
        Me.gbAssistant.Controls.Add(Me.bNext)
        Me.gbAssistant.Location = New System.Drawing.Point(10, 570)
        Me.gbAssistant.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbAssistant.Name = "gbAssistant"
        Me.gbAssistant.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbAssistant.Size = New System.Drawing.Size(981, 96)
        Me.gbAssistant.TabIndex = 44
        Me.gbAssistant.TabStop = False
        Me.gbAssistant.Text = "Assistant"
        '
        'lTip
        '
        Me.lTip.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lTip.ForeColor = System.Drawing.Color.Blue
        Me.lTip.Location = New System.Drawing.Point(3, 26)
        Me.lTip.Name = "lTip"
        Me.lTip.Size = New System.Drawing.Size(893, 62)
        Me.lTip.TabIndex = 40
        Me.lTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'llEditAudio1
        '
        Me.llEditAudio1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llEditAudio1.Location = New System.Drawing.Point(929, 62)
        Me.llEditAudio1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llEditAudio1.Name = "llEditAudio1"
        Me.llEditAudio1.Size = New System.Drawing.Size(48, 31)
        Me.llEditAudio1.TabIndex = 21
        Me.llEditAudio1.TabStop = True
        Me.llEditAudio1.Text = "Edit"
        Me.llEditAudio1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbAudio
        '
        Me.gbAudio.Controls.Add(Me.llEditAudio1)
        Me.gbAudio.Controls.Add(Me.llEditAudio0)
        Me.gbAudio.Controls.Add(Me.llAudioProfile1)
        Me.gbAudio.Controls.Add(Me.llAudioProfile0)
        Me.gbAudio.Controls.Add(Me.tbAudioFile0)
        Me.gbAudio.Controls.Add(Me.tbAudioFile1)
        Me.gbAudio.Location = New System.Drawing.Point(10, 460)
        Me.gbAudio.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbAudio.Name = "gbAudio"
        Me.gbAudio.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbAudio.Size = New System.Drawing.Size(981, 104)
        Me.gbAudio.TabIndex = 59
        Me.gbAudio.TabStop = False
        Me.gbAudio.Text = "Audio"
        '
        'llAudioProfile0
        '
        Me.llAudioProfile0.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llAudioProfile0.Location = New System.Drawing.Point(658, 23)
        Me.llAudioProfile0.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llAudioProfile0.Name = "llAudioProfile0"
        Me.llAudioProfile0.Size = New System.Drawing.Size(270, 30)
        Me.llAudioProfile0.TabIndex = 25
        Me.llAudioProfile0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'llAudioProfile1
        '
        Me.llAudioProfile1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llAudioProfile1.Location = New System.Drawing.Point(658, 60)
        Me.llAudioProfile1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llAudioProfile1.Name = "llAudioProfile1"
        Me.llAudioProfile1.Size = New System.Drawing.Size(270, 30)
        Me.llAudioProfile1.TabIndex = 26
        Me.llAudioProfile1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbAudioFile0
        '
        Me.tbAudioFile0.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAudioFile0.Location = New System.Drawing.Point(8, 25)
        Me.tbAudioFile0.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbAudioFile0.Size = New System.Drawing.Size(645, 55)
        '
        'tbAudioFile1
        '
        Me.tbAudioFile1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAudioFile1.Location = New System.Drawing.Point(8, 62)
        Me.tbAudioFile1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbAudioFile1.Size = New System.Drawing.Size(645, 55)
        '
        'MenuStrip
        '
        Me.MenuStrip.AutoSize = False
        Me.MenuStrip.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Padding = New System.Windows.Forms.Padding(3, 3, 0, 3)
        Me.MenuStrip.Size = New System.Drawing.Size(1003, 46)
        Me.MenuStrip.TabIndex = 60
        Me.MenuStrip.Text = "MenuStrip"
        '
        'lgbTarget
        '
        Me.lgbTarget.Controls.Add(Me.llFilesize)
        Me.lgbTarget.Controls.Add(Me.tbTargetSize)
        Me.lgbTarget.Controls.Add(Me.lTarget2)
        Me.lgbTarget.Controls.Add(Me.lBitrate)
        Me.lgbTarget.Controls.Add(Me.tbBitrate)
        Me.lgbTarget.Controls.Add(Me.lTarget1)
        Me.lgbTarget.Controls.Add(Me.tbTargetFile)
        Me.lgbTarget.Location = New System.Drawing.Point(508, 49)
        Me.lgbTarget.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbTarget.Name = "lgbTarget"
        Me.lgbTarget.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbTarget.Size = New System.Drawing.Size(483, 175)
        Me.lgbTarget.TabIndex = 58
        Me.lgbTarget.TabStop = False
        Me.lgbTarget.Text = "Target"
        '
        'llFilesize
        '
        Me.llFilesize.Location = New System.Drawing.Point(7, 66)
        Me.llFilesize.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llFilesize.Name = "llFilesize"
        Me.llFilesize.Size = New System.Drawing.Size(78, 31)
        Me.llFilesize.TabIndex = 59
        Me.llFilesize.TabStop = True
        Me.llFilesize.Text = "Size:"
        Me.llFilesize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTargetSize
        '
        Me.tbTargetSize.Location = New System.Drawing.Point(86, 67)
        Me.tbTargetSize.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbTargetSize.Name = "tbTargetSize"
        Me.tbTargetSize.Size = New System.Drawing.Size(80, 55)
        Me.tbTargetSize.TabIndex = 55
        Me.tbTargetSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lTarget2
        '
        Me.lTarget2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lTarget2.Location = New System.Drawing.Point(7, 134)
        Me.lTarget2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lTarget2.Name = "lTarget2"
        Me.lTarget2.Size = New System.Drawing.Size(471, 31)
        Me.lTarget2.TabIndex = 47
        Me.lTarget2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lBitrate
        '
        Me.lBitrate.Location = New System.Drawing.Point(205, 66)
        Me.lBitrate.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lBitrate.Name = "lBitrate"
        Me.lBitrate.Size = New System.Drawing.Size(124, 31)
        Me.lBitrate.TabIndex = 42
        Me.lBitrate.Text = "Video Bitrate:"
        Me.lBitrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbBitrate
        '
        Me.tbBitrate.Location = New System.Drawing.Point(330, 67)
        Me.tbBitrate.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbBitrate.Name = "tbBitrate"
        Me.tbBitrate.Size = New System.Drawing.Size(80, 55)
        Me.tbBitrate.TabIndex = 41
        Me.tbBitrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lTarget1
        '
        Me.lTarget1.Location = New System.Drawing.Point(7, 102)
        Me.lTarget1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lTarget1.Name = "lTarget1"
        Me.lTarget1.Size = New System.Drawing.Size(471, 31)
        Me.lTarget1.TabIndex = 39
        Me.lTarget1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbTargetFile
        '
        Me.tbTargetFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbTargetFile.Location = New System.Drawing.Point(8, 27)
        Me.tbTargetFile.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbTargetFile.Size = New System.Drawing.Size(467, 55)
        '
        'lgbSource
        '
        Me.lgbSource.Controls.Add(Me.lCrop)
        Me.lgbSource.Controls.Add(Me.lSourceDar)
        Me.lgbSource.Controls.Add(Me.lSourceDarText)
        Me.lgbSource.Controls.Add(Me.lSourcePAR)
        Me.lgbSource.Controls.Add(Me.llSourceParText)
        Me.lgbSource.Controls.Add(Me.lCropText)
        Me.lgbSource.Controls.Add(Me.lSource2)
        Me.lgbSource.Controls.Add(Me.lSource1)
        Me.lgbSource.Controls.Add(Me.tbSourceFile)
        Me.lgbSource.Location = New System.Drawing.Point(13, 49)
        Me.lgbSource.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbSource.Name = "lgbSource"
        Me.lgbSource.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbSource.Size = New System.Drawing.Size(487, 175)
        Me.lgbSource.TabIndex = 57
        Me.lgbSource.TabStop = False
        Me.lgbSource.Text = "Source"
        '
        'lCrop
        '
        Me.lCrop.Location = New System.Drawing.Point(72, 134)
        Me.lCrop.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lCrop.Name = "lCrop"
        Me.lCrop.Size = New System.Drawing.Size(100, 31)
        Me.lCrop.TabIndex = 16
        Me.lCrop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourceDar
        '
        Me.lSourceDar.Location = New System.Drawing.Point(387, 134)
        Me.lSourceDar.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSourceDar.Name = "lSourceDar"
        Me.lSourceDar.Size = New System.Drawing.Size(77, 31)
        Me.lSourceDar.TabIndex = 50
        Me.lSourceDar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourceDarText
        '
        Me.lSourceDarText.Location = New System.Drawing.Point(331, 134)
        Me.lSourceDarText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSourceDarText.Name = "lSourceDarText"
        Me.lSourceDarText.Size = New System.Drawing.Size(55, 31)
        Me.lSourceDarText.TabIndex = 49
        Me.lSourceDarText.TabStop = True
        Me.lSourceDarText.Text = "DAR:"
        Me.lSourceDarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSourcePAR
        '
        Me.lSourcePAR.Location = New System.Drawing.Point(229, 134)
        Me.lSourcePAR.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSourcePAR.Name = "lSourcePAR"
        Me.lSourcePAR.Size = New System.Drawing.Size(77, 31)
        Me.lSourcePAR.TabIndex = 47
        Me.lSourcePAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'llSourceParText
        '
        Me.llSourceParText.Location = New System.Drawing.Point(173, 134)
        Me.llSourceParText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llSourceParText.Name = "llSourceParText"
        Me.llSourceParText.Size = New System.Drawing.Size(55, 31)
        Me.llSourceParText.TabIndex = 45
        Me.llSourceParText.TabStop = True
        Me.llSourceParText.Text = "PAR:"
        Me.llSourceParText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lCropText
        '
        Me.lCropText.Location = New System.Drawing.Point(1, 134)
        Me.lCropText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lCropText.Name = "lCropText"
        Me.lCropText.Size = New System.Drawing.Size(70, 31)
        Me.lCropText.TabIndex = 14
        Me.lCropText.TabStop = True
        Me.lCropText.Text = "Crop:"
        Me.lCropText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSource2
        '
        Me.lSource2.Location = New System.Drawing.Point(1, 100)
        Me.lSource2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSource2.Name = "lSource2"
        Me.lSource2.Size = New System.Drawing.Size(483, 31)
        Me.lSource2.TabIndex = 33
        Me.lSource2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSource1
        '
        Me.lSource1.Location = New System.Drawing.Point(1, 66)
        Me.lSource1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSource1.Name = "lSource1"
        Me.lSource1.Size = New System.Drawing.Size(483, 31)
        Me.lSource1.TabIndex = 41
        Me.lSource1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSourceFile
        '
        Me.tbSourceFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSourceFile.Location = New System.Drawing.Point(8, 27)
        Me.tbSourceFile.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbSourceFile.Size = New System.Drawing.Size(471, 55)
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
        Me.lgbResize.Location = New System.Drawing.Point(342, 229)
        Me.lgbResize.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbResize.Name = "lgbResize"
        Me.lgbResize.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbResize.Size = New System.Drawing.Size(320, 225)
        Me.lgbResize.TabIndex = 55
        Me.lgbResize.TabStop = False
        Me.lgbResize.Text = "Resize"
        '
        'lPAR
        '
        Me.lPAR.Location = New System.Drawing.Point(57, 182)
        Me.lPAR.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lPAR.Size = New System.Drawing.Size(81, 31)
        Me.lPAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lAspectRatioError
        '
        Me.lAspectRatioError.Location = New System.Drawing.Point(206, 182)
        Me.lAspectRatioError.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lAspectRatioError.Size = New System.Drawing.Size(110, 31)
        Me.lAspectRatioError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lZoom
        '
        Me.lZoom.Location = New System.Drawing.Point(206, 150)
        Me.lZoom.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lZoom.Name = "lZoom"
        Me.lZoom.Size = New System.Drawing.Size(110, 31)
        Me.lZoom.TabIndex = 44
        Me.lZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPixel
        '
        Me.lPixel.Location = New System.Drawing.Point(206, 118)
        Me.lPixel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lPixel.Name = "lPixel"
        Me.lPixel.Size = New System.Drawing.Size(110, 31)
        Me.lPixel.TabIndex = 50
        Me.lPixel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSarText
        '
        Me.lSarText.Location = New System.Drawing.Point(4, 150)
        Me.lSarText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSarText.Size = New System.Drawing.Size(52, 31)
        Me.lSarText.Text = "SAR:"
        Me.lSarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lAspectRatioErrorText
        '
        Me.lAspectRatioErrorText.Location = New System.Drawing.Point(139, 182)
        Me.lAspectRatioErrorText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lAspectRatioErrorText.Size = New System.Drawing.Size(66, 31)
        Me.lAspectRatioErrorText.Text = "Error:"
        Me.lAspectRatioErrorText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lParText
        '
        Me.lParText.Location = New System.Drawing.Point(4, 182)
        Me.lParText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lParText.Size = New System.Drawing.Size(52, 31)
        Me.lParText.Text = "PAR:"
        Me.lParText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPixelText
        '
        Me.lPixelText.Location = New System.Drawing.Point(139, 118)
        Me.lPixelText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lPixelText.Name = "lPixelText"
        Me.lPixelText.Size = New System.Drawing.Size(66, 31)
        Me.lPixelText.TabIndex = 49
        Me.lPixelText.Text = "Pixel:"
        Me.lPixelText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lZoomText
        '
        Me.lZoomText.Location = New System.Drawing.Point(139, 150)
        Me.lZoomText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lZoomText.Name = "lZoomText"
        Me.lZoomText.Size = New System.Drawing.Size(66, 31)
        Me.lZoomText.TabIndex = 42
        Me.lZoomText.Text = "Zoom:"
        Me.lZoomText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lDarText
        '
        Me.lDarText.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lDarText.Location = New System.Drawing.Point(4, 118)
        Me.lDarText.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lDarText.Name = "lDarText"
        Me.lDarText.Size = New System.Drawing.Size(52, 31)
        Me.lDarText.TabIndex = 23
        Me.lDarText.Text = "DAR:"
        Me.lDarText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSAR
        '
        Me.lSAR.Location = New System.Drawing.Point(57, 150)
        Me.lSAR.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSAR.Size = New System.Drawing.Size(81, 31)
        Me.lSAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lDAR
        '
        Me.lDAR.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lDAR.Location = New System.Drawing.Point(57, 118)
        Me.lDAR.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lDAR.Name = "lDAR"
        Me.lDAR.Size = New System.Drawing.Size(81, 31)
        Me.lDAR.TabIndex = 24
        Me.lDAR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbResize
        '
        Me.tbResize.AutoSize = False
        Me.tbResize.BackColor = System.Drawing.SystemColors.Control
        Me.tbResize.Location = New System.Drawing.Point(8, 73)
        Me.tbResize.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbResize.Name = "tbResize"
        Me.tbResize.Size = New System.Drawing.Size(304, 35)
        Me.tbResize.TabIndex = 46
        '
        'lTargetWidth
        '
        Me.lTargetWidth.AutoSize = True
        Me.lTargetWidth.Location = New System.Drawing.Point(12, 37)
        Me.lTargetWidth.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lTargetWidth.Name = "lTargetWidth"
        Me.lTargetWidth.Size = New System.Drawing.Size(124, 48)
        Me.lTargetWidth.TabIndex = 37
        Me.lTargetWidth.Text = "Width:"
        '
        'tbTargetWidth
        '
        Me.tbTargetWidth.Location = New System.Drawing.Point(82, 34)
        Me.tbTargetWidth.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbTargetWidth.Name = "tbTargetWidth"
        Me.tbTargetWidth.Size = New System.Drawing.Size(70, 55)
        Me.tbTargetWidth.TabIndex = 39
        Me.tbTargetWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tbTargetHeight
        '
        Me.tbTargetHeight.Location = New System.Drawing.Point(234, 34)
        Me.tbTargetHeight.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tbTargetHeight.Name = "tbTargetHeight"
        Me.tbTargetHeight.Size = New System.Drawing.Size(70, 55)
        Me.tbTargetHeight.TabIndex = 40
        Me.tbTargetHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lTargetHeight
        '
        Me.lTargetHeight.AutoSize = True
        Me.lTargetHeight.Location = New System.Drawing.Point(159, 37)
        Me.lTargetHeight.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lTargetHeight.Name = "lTargetHeight"
        Me.lTargetHeight.Size = New System.Drawing.Size(135, 48)
        Me.lTargetHeight.TabIndex = 38
        Me.lTargetHeight.Text = "Height:"
        '
        'lgbFilters
        '
        Me.lgbFilters.Controls.Add(Me.AviSynthListView)
        Me.lgbFilters.Location = New System.Drawing.Point(13, 229)
        Me.lgbFilters.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbFilters.Name = "lgbFilters"
        Me.lgbFilters.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbFilters.Size = New System.Drawing.Size(320, 225)
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
        Me.AviSynthListView.AutoCheckMode = StaxRip.UI.AutoCheckMode.None
        Me.AviSynthListView.CheckBoxes = True
        Me.AviSynthListView.FullRowSelect = True
        Me.AviSynthListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.AviSynthListView.HideSelection = False
        Me.AviSynthListView.IsLoading = False
        Me.AviSynthListView.Location = New System.Drawing.Point(8, 27)
        Me.AviSynthListView.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.AviSynthListView.MultiSelect = False
        Me.AviSynthListView.Name = "AviSynthListView"
        Me.AviSynthListView.Size = New System.Drawing.Size(304, 187)
        Me.AviSynthListView.TabIndex = 0
        Me.AviSynthListView.UseCompatibleStateImageBehavior = False
        Me.AviSynthListView.View = System.Windows.Forms.View.Details
        '
        'lgbEncoder
        '
        Me.lgbEncoder.Controls.Add(Me.llMuxer)
        Me.lgbEncoder.Controls.Add(Me.pEncoder)
        Me.lgbEncoder.Location = New System.Drawing.Point(671, 229)
        Me.lgbEncoder.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbEncoder.Name = "lgbEncoder"
        Me.lgbEncoder.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lgbEncoder.Size = New System.Drawing.Size(320, 225)
        Me.lgbEncoder.TabIndex = 51
        Me.lgbEncoder.TabStop = False
        Me.lgbEncoder.Text = "Encoder"
        '
        'llMuxer
        '
        Me.llMuxer.AutoSize = True
        Me.llMuxer.Location = New System.Drawing.Point(259, 0)
        Me.llMuxer.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.llMuxer.Name = "llMuxer"
        Me.llMuxer.Size = New System.Drawing.Size(89, 48)
        Me.llMuxer.TabIndex = 1
        Me.llMuxer.TabStop = True
        Me.llMuxer.Text = "Mux"
        '
        'pEncoder
        '
        Me.pEncoder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pEncoder.Location = New System.Drawing.Point(8, 28)
        Me.pEncoder.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pEncoder.Name = "pEncoder"
        Me.pEncoder.Size = New System.Drawing.Size(305, 186)
        Me.pEncoder.TabIndex = 0
        '
        'MainForm
        '
        Me.AllowDrop = True
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1003, 677)
        Me.Controls.Add(Me.gbAudio)
        Me.Controls.Add(Me.lgbTarget)
        Me.Controls.Add(Me.lgbSource)
        Me.Controls.Add(Me.lgbResize)
        Me.Controls.Add(Me.lgbFilters)
        Me.Controls.Add(Me.lgbEncoder)
        Me.Controls.Add(Me.gbAssistant)
        Me.Controls.Add(Me.MenuStrip)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
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

        Try
            s = SafeSerialization.Deserialize(New ApplicationSettings, g.SettingsFile)
        Catch ex As Exception
            g.ShowException(ex, "The settings file failed to load, it will be reset to defaults." + BR2 + g.SettingsFile)
            s = New ApplicationSettings
            s.Init()
        End Try

        MenuItemEx.UseTooltips = s.EnableTooltips

        InitializeComponent()

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

        Icon = My.Resources.RipIcon

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

    <Command("Shows different donation options.")>
    Sub ShowDonateDialog()
        Dim td As New TaskDialog(Of String)
        td.MainInstruction = "If you are a satisfied user of StaxRip, please think about contributing to this project."

        td.AddCommandLink("PayPal", "Donate via PayPal", "PayPal")
        td.AddCommandLink("Bitcoin", "Copy Bitcoin address to clipboard", "Bitcoin")
        If g.IsCulture("de") Then td.AddCommandLink("Amazon", "Email Adresse in Zwischenablage kopieren und Gutschein Seite öffnen", "Amazon")

        Select Case td.Show
            Case "PayPal"
                g.ShellExecute(Strings.DonationsURL)
            Case "Bitcoin"
                Clipboard.SetText("19FjjVNYBUEowkqL3CwrJp6Ks191wYHtph")
                MsgInfo("Address was copied to the clipboard.")
            Case "Amazon"
                Clipboard.SetText("frank.skare.de@gmail.com")
                g.ShellExecute("https://www.amazon.de/Amazon-Gutschein-E-Mail-Verschiedene-Motive/dp/BT00DHI7WY")
        End Select
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

            lgbEncoder.Label.LinkColor = c
            lgbFilters.Label.LinkColor = c
            lgbResize.Label.LinkColor = c
            lgbSource.Label.LinkColor = c
            lgbTarget.Label.LinkColor = c
        Else
            llAudioProfile0.LinkColor = Color.Blue
            llAudioProfile1.LinkColor = Color.Blue
            llEditAudio0.LinkColor = Color.Blue
            llEditAudio1.LinkColor = Color.Blue
            llFilesize.LinkColor = Color.Blue
            llMuxer.LinkColor = Color.Blue
            llSourceParText.LinkColor = Color.Blue

            lgbEncoder.Label.LinkColor = Color.Blue
            lgbFilters.Label.LinkColor = Color.Blue
            lgbResize.Label.LinkColor = Color.Blue
            lgbSource.Label.LinkColor = Color.Blue
            lgbTarget.Label.LinkColor = Color.Blue
        End If
    End Sub

    Function GetIfoFile() As String
        Dim ret = Filepath.GetDir(p.SourceFile)
        If ret.EndsWith("_temp\") Then ret = DirPath.GetParent(ret)
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
                If Not Filepath.GetExt(iPath) = iExt Then Continue For
                If iPath.Contains("_cut_") Then Continue For
                If iPath.Contains("_out") Then Continue For
                If Not g.IsSourceSame(iPath) Then Continue For
                If hq AndAlso Not iPath.Ext.EqualsAny("dtsma", "thd") Then Continue For

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
        'ObjectHelp.GetCompareString(g.SavedProject).WriteFile(Environment.GetFolderPath(SpecialFolder.DesktopDirectory) + "\test1.txt", Encoding.ASCII)
        'ObjectHelp.GetCompareString(p).WriteFile(Environment.GetFolderPath(SpecialFolder.DesktopDirectory) + "\test2.txt", Encoding.ASCII)

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
        For Each i In CustomMainMenu.MenuItems
            If i.CustomMenuItem.MethodName = "DynamicMenuItem" AndAlso
                i.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.RecentProjects) Then

                i.DropDownItems.Clear()

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
                         If Not IsDisposed Then Invoke(Sub() If Not IsDisposed AndAlso Handle = Native.GetForegroundWindow() Then UpdateDynamicMenu())
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
                    i.DropDownItems.Clear()

                    For Each iPackage In Package.Items.Values
                        Dim helpPath = iPackage.GetHelpPath

                        If helpPath <> "" Then
                            Dim plugin = TryCast(iPackage, PluginPackage)

                            If plugin Is Nothing Then
                                ActionMenuItem.Add(i.DropDownItems, "Apps | " + iPackage.Name, Sub() g.ShellExecute(helpPath))
                            Else
                                If plugin.AviSynthFilterNames?.Length > 0 Then
                                    ActionMenuItem.Add(i.DropDownItems, "Plugins | AviSynth | " + iPackage.Name, Sub() g.ShellExecute(iPackage.GetHelpPath(ScriptEngine.AviSynth)))
                                End If

                                If plugin.VapourSynthFilterNames?.Length > 0 Then
                                    ActionMenuItem.Add(i.DropDownItems, "Plugins | VapourSynth | " + iPackage.Name, Sub() g.ShellExecute(iPackage.GetHelpPath(ScriptEngine.VapourSynth)))
                                End If
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

        Await Task.Run(Sub()
                           Thread.Sleep(500)

                           If Directory.Exists(Folder.Startup + "Apps\Scripts") Then
                               For Each script In Directory.GetFiles(Folder.Startup + "Apps\Scripts")
                                   If Not s.Storage.GetBool(script.FileName) AndAlso
                                       Not File.Exists(Folder.Script + script.FileName) Then

                                       FileHelp.Copy(script, Folder.Script + script.FileName)
                                       s.Storage.SetBool(script.FileName, True)
                                   End If
                               Next
                           End If

                           files = Directory.GetFiles(Folder.Script)
                       End Sub)

        If IsDisposed OrElse Native.GetForegroundWindow() <> Handle Then Exit Sub

        For Each menuItem In CustomMainMenu.MenuItems
            If menuItem.CustomMenuItem.MethodName = "DynamicMenuItem" Then
                If menuItem.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.Scripts) Then
                    menuItem.DropDownItems.Clear()
                    For Each path In files
                        ActionMenuItem.Add(menuItem.DropDownItems,
                                           path.FileName.Base,
                                           Sub() g.DefaultCommands.ExecuteScriptFile(path))
                    Next

                    menuItem.DropDownItems.Add(New ToolStripSeparator)
                    ActionMenuItem.Add(menuItem.DropDownItems, "Open Scripts Folder", Sub() g.ShellExecute(Folder.Script))
                End If
            End If
        Next

        Application.DoEvents()
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

        If IsDisposed OrElse Native.GetForegroundWindow() <> Handle Then Exit Sub

        For Each i In CustomMainMenu.MenuItems
            If i.CustomMenuItem.MethodName = "DynamicMenuItem" AndAlso
                i.CustomMenuItem.Parameters(0).Equals(DynamicMenuItemID.TemplateProjects) Then

                i.DropDownItems.Clear()
                Dim items As New List(Of ActionMenuItem)

                For Each i2 In files
                    Dim base = i2.Base
                    If i2 = g.StartupTemplatePath Then base += " (Startup)"
                    If i2.Contains("Backup\") Then base = "Backup | " + base
                    ActionMenuItem.Add(i.DropDownItems, base, AddressOf LoadProject, i2, Nothing)
                Next

                i.DropDownItems.Add("-")
                ActionMenuItem.Add(i.DropDownItems, "Explore", Sub() g.ShellExecute(Folder.Template), "Opens the directory containing the templates.")
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
                d.FileName = p.Name
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

    Function OpenProject(path As String) As Boolean
        Return OpenProject(path, True)
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

    Function OpenProject(path As String, saveCurrent As Boolean) As Boolean
        If Not IsLoading AndAlso saveCurrent AndAlso IsSaveCanceled() Then
            Return False
        End If

        SetBindings(p, False)

        If Not File.Exists(path) Then path = g.StartupTemplatePath

        Try
            p = SafeSerialization.Deserialize(New Project, path)
        Catch ex As Exception
            g.ShowException(ex, "The project file failed to load, it will be reset to defaults." + BR2 + path)
            p = New Project
            p.Init()
        End Try

        SetBindings(p, True)

        Text = Application.ProductName + " x64 - " + Filepath.GetBase(path)
        SkipAssistant = True

        If path.StartsWith(Folder.Template) Then
            g.ProjectPath = Nothing
            p.TemplateName = Filepath.GetBase(path)
        Else
            g.ProjectPath = path
        End If

        g.SetTempDir()

        Dim width = p.TargetWidth
        Dim height = p.TargetHeight
        Dim size = p.TargetSize
        Dim bitrate = p.VideoBitrate

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
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
        If Not files.NothingOrEmpty Then e.Effect = DragDropEffects.Copy
    End Sub

    Sub FormMain_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            Activate()
            BeginInvoke(Sub() OpenAnyFile(files.ToList))
        End If
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
            td.MainInstruction = "Choose a AviSynth source filter"
        Else
            td.MainInstruction = "Choose a VapourSynth source filter"
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
        If Filepath.GetExtFull(files(0)) = ".srip" Then
            OpenProject(files(0))
        ElseIf FileTypes.Video.Contains(Filepath.GetExt(files(0)).ToLower) Then
            files.Sort()
            OpenVideoSourceFiles(files)
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

    Sub OpenVideoSourceFiles(files As IEnumerable(Of String))
        OpenVideoSourceFiles(files, True)
    End Sub

    Sub OpenVideoSourceFiles(files As IEnumerable(Of String), isNotEncoding As Boolean)
        Dim recoverPath = g.ProjectPath
        Dim recoverProjectPath = Folder.Temp + Guid.NewGuid.ToString + ".bin"
        Dim recoverText = Text

        SafeSerialization.Serialize(p, recoverProjectPath)

        AddHandler g.MainForm.Disposed, Sub() FileHelp.Delete(recoverProjectPath)

        Try
            If g.ShowVideoSourceWarnings(files) Then Throw New AbortException

            For Each i In files
                Dim name = Filepath.GetName(i)

                If name.ToUpper Like "VTS_0#_0.VOB" Then
                    If Msg("Are you sure you want to open the file " + name + "," + BR +
                           "the first VOB file usually contains a menu!", Nothing,
                           MsgIcon.Question, MessageBoxButtons.YesNo,
                           DialogResult.No) <> DialogResult.Yes Then

                        Throw New AbortException
                    End If
                End If

                If name.ToUpper = "VIDEO_TS.VOB" Then
                    MsgWarn("The file VIDEO_TS.VOB can't be opened.")
                    Throw New AbortException
                End If
            Next

            If p.SourceFile <> "" AndAlso isNotEncoding Then
                Dim templates = Directory.GetFiles(Folder.Template, "*.srip")

                If templates.Length = 1 Then
                    If Not OpenProject(templates(0), True) Then Throw New AbortException
                Else
                    If s.ShowTemplateSelection Then
                        If Not LoadTemplateWithSelectionDialog() Then Throw New AbortException
                    Else
                        If Not OpenProject(g.StartupTemplatePath, True) Then Throw New AbortException
                    End If
                End If
            End If

            p.SourceFiles = files.ToList
            p.SourceFile = files(0)

            AviSynthListView.IsLoading = True

            Dim preferredSourceFilter As VideoFilter

            If p.SourceFiles.Count = 1 AndAlso
                p.Script.Filters(0).Name = "Manual" AndAlso
                Not p.NoDialogs AndAlso Not p.BatchMode AndAlso
                Not p.SourceFile.Ext = "vpy" Then

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
            g.SetTempDir()

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
                Dim reg = If(p.SourceFile.Ext = "vpy", "source\(.*?('|"")(.+?)\1", "(source)\(""(.+?)""")
                Dim match = Regex.Match(code, reg, RegexOptions.IgnoreCase)

                If match.Success Then
                    Dim path = match.Groups(2).Value

                    If File.Exists(path) AndAlso FileTypes.Video.Contains(path.Ext) Then
                        If FileTypes.VideoIndex.Contains(path.Ext) Then
                            path = GetPathFromIndexFile(path)

                            If path <> "" Then
                                p.LastOriginalSourceFile = path
                                p.FirstOriginalSourceFile = path
                            End If
                        Else
                            p.LastOriginalSourceFile = path
                            p.FirstOriginalSourceFile = path
                        End If
                    End If
                End If
            End If

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

            Dim movieName = MediaInfo.GetGeneral(p.LastOriginalSourceFile, "Movie")

            If movieName <> "" AndAlso TypeOf p.VideoEncoder.Muxer Is MkvMuxer Then
                DirectCast(p.VideoEncoder.Muxer, MkvMuxer).Title = movieName
            End If

            If isNotEncoding AndAlso p.BatchMode Then
                Assistant()
                Exit Sub
            End If

            Log.WriteEnvironment()

            Dim targetDir As String = Nothing

            If p.DefaultTargetFolder <> "" Then
                targetDir = Macro.Expand(p.DefaultTargetFolder).AppendSeparator

                If Not Directory.Exists(targetDir) Then
                    Try
                        Directory.CreateDirectory(targetDir)
                    Catch ex As Exception
                    End Try
                End If
            End If

            If Not Directory.Exists(targetDir) Then targetDir = p.SourceFile.Dir
            Dim targetName = Macro.Expand(p.DefaultTargetName)

            If Not Filepath.IsValidFileSystemName(targetName) Then targetName = p.SourceFile.Base
            tbTargetFile.Text = targetDir + targetName + p.VideoEncoder.Muxer.OutputExtFull

            If p.SourceFile = p.TargetFile OrElse
                (FileTypes.VideoIndex.Contains(Filepath.GetExtFull(p.SourceFile)) AndAlso
                File.ReadAllText(p.SourceFile).Contains(p.TargetFile)) Then

                tbTargetFile.Text = Filepath.GetDirAndBase(p.TargetFile) + "_new" + Filepath.GetExtFull(p.TargetFile)
            End If

            Log.WriteHeader("Source file MediaInfo")

            For Each i In p.SourceFiles
                Log.WriteLine(i)
            Next

            Log.WriteLine(BR + MediaInfo.GetSummary(p.SourceFile))

            For Each i In DriveInfo.GetDrives()
                If i.DriveType = DriveType.CDRom AndAlso
                    p.TempDir.ToUpper.StartsWith(i.RootDirectory.ToString.ToUpper) Then

                    MsgWarn("Opening files from a CD/DVD drive requires to set a temp files directory in the option.")
                    Throw New AbortException
                End If
            Next

            Demux()

            If p.LastOriginalSourceFile <> p.SourceFile AndAlso
                Not FileTypes.VideoText.Contains(Filepath.GetExt(p.SourceFile)) Then

                p.LastOriginalSourceFile = p.SourceFile
            End If

            s.LastSourceDir = Filepath.GetDir(p.SourceFile)

            If p.SourceFile.Ext = "avs" AndAlso p.Script.Engine = ScriptEngine.AviSynth Then
                p.Script.Filters.Clear()
                p.Script.Filters.Add(New VideoFilter("Source", "AviSynth Import", File.ReadAllText(p.SourceFile)))
            ElseIf p.SourceFile.Ext = "vpy" Then
                p.Script.Engine = ScriptEngine.VapourSynth
                p.Script.Filters.Clear()
                p.Script.Filters.Add(New VideoFilter("Source", "VapourSynth Import", File.ReadAllText(p.SourceFile)))
            End If

            ModifyFilters()
            AviSynthListView.IsLoading = False
            AviSynthListView.Load()

            If Not Package.DGDecodeNV.VerifyOK() OrElse Not Package.DGDecodeIM.VerifyOK() Then
                Throw New AbortException
            End If

            RenameDVDTracks()

            If FileTypes.VideoAudio.Contains(Filepath.GetExt(p.LastOriginalSourceFile)) Then
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

                    ProcessForm.CloseProcessForm()

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
                        Not FileTypes.VideoText.Contains(Filepath.GetExt(p.LastOriginalSourceFile)) Then

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
                                                tbAudioFile1.Text = i2.Name + " (" + Filepath.GetExt(p.Audio1.File) + ")"
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

            Dim crop = p.Script.IsFilterActive("Crop")

            If crop Then
                g.RunAutoCrop()
                DisableCropFilter()
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

            g.RaiseAppEvent(ApplicationEvent.AfterSourceLoaded)
            g.RaiseAppEvent(ApplicationEvent.ProjectOrSourceLoaded)
            Log.Save()
        Catch ex As AbortException
            Log.Save()
            ProcessForm.CloseProcessForm()
            SetSavedProject()
            OpenProject(recoverProjectPath)
            Text = recoverText
            g.ProjectPath = recoverPath
            If Not isNotEncoding Then Throw New AbortException
        Catch ex As Exception
            g.OnException(ex)
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

                        If name.Contains(Filepath.GetExt(p.SourceFile)) Then
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
                If editAVS Then
                    p.Script.GetFilter("Source").Script += BR + "Crop(0, 0, -" &
                                sourceWidth Mod 4 & ", -" & sourceHeight Mod 4 & ")"
                ElseIf editVS Then
                    p.Script.GetFilter("Source").Script += BR +
                                "clip = core.std.CropRel(clip, 0, " &
                                sourceWidth Mod 4 & ", 0, " & sourceHeight Mod 4 & ")"
                End If
            End If
        End If
    End Sub

    Sub AutoResize()
        If p.Script.IsFilterActive("Resize") Then
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
                    proc.File = Package.BDSup2SubPP.Path
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
                Not File.Exists(Filepath.GetDirAndBase(i) + "_forced.idx") Then

                Dim idxContent = File.ReadAllText(i, Encoding.Default)

                If idxContent.Contains(VB6.ChrW(&HA) + VB6.ChrW(&H0) + VB6.ChrW(&HD) + VB6.ChrW(&HA)) Then
                    idxContent = idxContent.FixBreak
                    idxContent = idxContent.Replace(BR + VB6.ChrW(&H0) + BR, BR + "langidx: 0" + BR)
                    File.WriteAllText(i, idxContent, Encoding.Default)
                End If

                Using proc As New Proc
                    proc.Init("Extract forced subtitles if existing", "# ")
                    proc.WriteLine(Filepath.GetName(i) + BR2)
                    proc.File = Package.BDSup2SubPP.Path
                    proc.Arguments = "--forced-only -o """ + i.DirAndBase + "_forced.idx"" """ + i + """"
                    proc.AllowedExitCodes = {}
                    proc.Start()
                End Using
            End If
        Next
    End Sub

    Sub DemuxVobSubSubtitles()
        If Not {"vob", "m2v"}.Contains(Filepath.GetExt(p.LastOriginalSourceFile)) Then Exit Sub
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
                            proc.Init("Demux subtitles using VSRip")
                            proc.WriteLine(args + BR2)
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

    Sub Encode()
        Try
            g.IsProcessing = True
            g.RaiseAppEvent(ApplicationEvent.BeforeEncoding)
            Dim startTime = DateTime.Now

            If p.BatchMode Then
                If g.ProjectPath Is Nothing Then
                    g.ProjectPath = p.TempDir + p.SourceFiles(0).Base + ".srip"
                End If

                SaveProjectPath(g.ProjectPath)
                OpenVideoSourceFiles(p.SourceFiles, False)
                p.BatchMode = False
                SaveProjectPath(g.ProjectPath)
            End If

            ProcessForm.ShowForm()

            Log.WriteHeader("Script")
            Log.WriteLine(p.Script.GetFullScript)
            Log.WriteHeader("Script Properties")

            Dim props = "source frame count: " & p.SourceScript.GetFrames & BR +
                "source frame rate: " & p.SourceScript.GetFramerate.ToString("f6", CultureInfo.InvariantCulture) + BR +
                "source duration: " + TimeSpan.FromSeconds(g.Get0ForInfinityOrNaN(p.SourceScript.GetFrames / p.SourceScript.GetFramerate)).ToString + BR +
                "target frame count: " & p.Script.GetFrames & BR +
                "target frame rate: " & p.Script.GetFramerate.ToString("f6", CultureInfo.InvariantCulture) + BR +
                "target duration: " + TimeSpan.FromSeconds(g.Get0ForInfinityOrNaN(p.Script.GetFrames / p.Script.GetFramerate)).ToString

            Log.WriteLine(props.FormatColumn(":"))

            Audio.Process(p.Audio0)
            p.Audio0.Encode()

            Audio.Process(p.Audio1)
            p.Audio1.Encode()

            For Each i In p.AudioTracks
                Audio.Process(i)
                i.Encode()
            Next

            p.VideoEncoder.Encode()
            Log.Save()
            p.VideoEncoder.Muxer.Mux()

            If p.SaveThumbnails Then Thumbnails.SaveThumbnails(p.TargetFile)

            Log.WriteHeader("Job Complete")
            Log.WriteStats(startTime)

            g.RaiseAppEvent(ApplicationEvent.JobEncoded)
        Finally
            g.IsProcessing = False
        End Try
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

        If isResized Then
            If isValidAnamorphicSize Then
                lAspectRatioError.Text = "n/a"
            Else
                lAspectRatioError.Text = Calc.GetAspectRatioError.ToString("f2") + "%"
            End If
        Else
            lAspectRatioError.Text = "n/a"
            If p.TargetWidth <> cropw Then tbTargetWidth.Text = cropw.ToString
            If p.TargetHeight <> croph Then tbTargetHeight.Text = croph.ToString
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
        If trackBarValue < tbResize.Minimum Then trackBarValue = tbResize.Minimum
        If trackBarValue > tbResize.Maximum Then trackBarValue = tbResize.Maximum

        tbResize.Value = trackBarValue

        Dim par = Calc.GetTargetPAR

        If Calc.IsARSignalingRequired OrElse (par.X = 1 AndAlso par.Y = 1) Then
            lPAR.Text = par.X & ":" & par.Y
        Else
            lPAR.Text = "n/a"
        End If

        lDAR.Text = Calc.GetTargetDAR.ToString.Shorten(5)
        lSAR.Text = (p.TargetWidth / p.TargetHeight).ToString.Shorten(5)
        lSourceDar.Text = Calc.GetSourceDAR.ToString.Shorten(5)
        par = Calc.GetSimpleSourcePAR
        lSourcePAR.Text = par.X & ":" & par.Y

        If p.SourceSeconds > 0 Then
            lSource1.Text = lSource1.GetMaxTextSpace(
                p.SourceSeconds \ 60 & "m " + (p.SourceSeconds Mod 60).ToString("00") + "s",
                If(p.SourceSize / 1024 ^ 2 < 1024, CInt(p.SourceSize / 1024 ^ 2).ToString + "MB", (p.SourceSize / 1024 ^ 3).ToString("f1") + "GB"),
                If(p.SourceBitrate > 0, (p.SourceBitrate / 1000).ToString("f1") + "Mbps", ""),
                p.SourceFrameRate.ToString("f3").TrimEnd("0"c).TrimEnd(","c) + "fps",
                p.Codec, p.CodecProfile)

            lSource2.Text = lSource1.GetMaxTextSpace(
                p.SourceWidth.ToString + "x" + p.SourceHeight.ToString, p.ColorSpace,
                p.ChromaSubsampling, If(p.BitDepth <> 0, p.BitDepth & "Bits", ""),
                p.ScanType, If(p.ScanType = "Interlaced", p.ScanOrder, ""))

            lTarget1.Text = lSource1.GetMaxTextSpace(
                p.TargetSeconds \ 60 & "m " + (p.TargetSeconds Mod 60).ToString("00") + "s",
                p.TargetFrameRate.ToString("f3").TrimEnd("0"c).TrimEnd(","c) + "fps",
                "Audio Bitrate: " & CInt(Calc.GetAudioBitrate))

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

        If p.VideoEncoder.Muxer.CoverFile <> "" Then
            If Not p.VideoEncoder.Muxer.CoverFile.Base.EqualsAny("cover", "small_cover", "cover_land", "small_cover_land") OrElse Not p.VideoEncoder.Muxer.CoverFile.Ext.EqualsAny("jpg", "png") Then
                If ProcessTip("The cover file name bust be cover, small_cover, cover_land or small_cover_land, the file type must be jpg or png.") Then
                    gbAssistant.Text = "Invalid cover file name"
                    CanIgnoreTip = False
                    Return False
                End If
            End If
        End If

        If TypeOf p.VideoEncoder Is BasicVideoEncoder Then
            Dim enc = DirectCast(p.VideoEncoder, BasicVideoEncoder)
            Dim param = enc.CommandLineParams.GetOptionParam("--vpp-resize")

            If Not param Is Nothing AndAlso param.Value > 0 AndAlso
                Not p.Script.IsFilterActive("Resize", "Hardware Encoder") Then

                If ProcessTip("In order to use an resize filter of the hardware encoder choose 'Hardware Encoder' as resize filter from the filters menu.") Then
                    gbAssistant.Text = "Invalid filter setting"
                    CanIgnoreTip = False
                    Return False
                End If
            End If
        End If

        If Not p.BatchMode Then
            If Filepath.GetExtFull(p.TargetFile) = ".mp4" AndAlso p.TargetFile.Contains("#") Then
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
                p.Script.IsFilterActive("Crop") AndAlso
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
                    If ProcessTip("The audio delay is unusual high indicating a sync problem, MakeMKV and ProjectX can prevent this problem.") Then
                        g.Highlight(True, tbAudioFile0)
                        gbAssistant.Text = "Unusual high audio delay"
                        Return False
                    End If
                End If
            Next

            For Each i In audioTracks
                If i.File <> "" AndAlso Not p.VideoEncoder.Muxer.IsSupported(i.OutputFileType) AndAlso Not i.OutputFileType = "ignore" Then
                    If ProcessTip("The audio format is '" + i.OutputFileType + "' but the container '" + p.VideoEncoder.Muxer.Name + "' supports only " + p.VideoEncoder.Muxer.SupportedInputTypes.Join(", ") + ". Choose another audio profile or another container.") Then
                        g.Highlight(True, llAudioProfile0)
                        g.Highlight(True, llMuxer)
                        gbAssistant.Text = "Audio format conflicts with container"
                        CanIgnoreTip = False
                        Return False
                    End If
                End If
            Next

            If p.VideoEncoder.Muxer.OutputExtFull <> Filepath.GetExtFull(p.TargetFile) Then
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
                AndAlso p.Script.IsFilterActive("Resize") Then

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

                If p.VideoEncoder.Muxer.Subtitles.Where(Function(subtitle) subtitle.Enabled).Count > 0 AndAlso Not TypeOf p.VideoEncoder Is NullEncoder Then
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
                    g.Highlight(True, lTarget2)
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
                    If ProcessTip("The target file already exist, usually this means it was encoded successfully.") Then
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
        g.ShellExecute(Filepath.GetDir(p.TargetFile))
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

    Sub CloseWithoutSaving()
        g.SavedProject = p
        Close()
    End Sub

    Sub bSkip_Click() Handles bNext.Click
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
            AddJob(False, Nothing)
            ShowJobsDialog()
        Else
            Assistant()
        End If
    End Sub

    Function AbortDueToLowDiskSpace() As Boolean
        Try 'with file batch target file is empty, also crashes with network shares 
            Dim di As New DriveInfo(p.TargetFile.Dir)

            If di.AvailableFreeSpace / 1024 ^ 3 < s.MinimumDiskSpace Then
                ProcessForm.CloseProcessForm()

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
        g.DefaultCommands.StartJobs()
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
            If i.Name = "dsmux" Then
                If MediaInfo.GetAudioCount(p.SourceFile) = 0 Then
                    Continue For
                End If

                If CommandLineDemuxer.IsActive("DGIndexNV") OrElse
                    CommandLineDemuxer.IsActive("DGIndexIM") Then

                    Continue For
                End If

                If p.Script.Contains("Source", "DGSource(") OrElse
                    p.Script.Contains("Source", "DGSourceIM(") Then

                    Continue For
                End If
            End If

            If Not i.Active AndAlso (i.SourceFilter = "" OrElse
                Not srcScript.Contains(i.SourceFilter.ToLower + "(")) Then Continue For

            If i.InputExtensions?.Length = 0 OrElse i.InputExtensions.Contains(p.SourceFile.Ext) Then
                If Not srcScript?.Contains("(") OrElse i.SourceFilter = "" OrElse
                    srcScript.Contains(i.SourceFilter.ToLower + "(") Then

                    Dim inputFormats = i.InputFormats.NothingOrEmpty OrElse
                        i.InputFormats.Contains(getFormat())

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
                Not FileTypes.VideoText.Contains(Filepath.GetExt(p.SourceFile)) Then

                Using proc As New Proc
                    proc.Init("Index LWLibav")
                    proc.Encoding = Encoding.UTF8

                    If p.Script.Engine = ScriptEngine.AviSynth Then
                        proc.File = Package.ffmpeg.Path
                        proc.Arguments = "-i " + p.Script.Path.Quotes + " -hide_banner"
                    Else
                        proc.File = Package.vspipe.Path
                        proc.Arguments = p.Script.Path.Quotes + " NUL -i"
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
            Dim dgIndexNV = Demuxer.GetDefaults.Find(Function(demuxer) demuxer.Name = "DGIndexNV")
            Dim outFile = p.TempDir + p.SourceFile.Base + ".dgi"
            If Not File.Exists(outFile) Then dgIndexNV.Run()

            If File.Exists(outFile) Then
                p.SourceFile = outFile
                BlockSourceTextBoxTextChanged = True
                tbSourceFile.Text = outFile
                BlockSourceTextBoxTextChanged = False
            End If
        ElseIf codeLower.Contains("dgsourceim(") AndAlso Not p.SourceFile.Ext = "dgim" Then
            If FileTypes.VideoIndex.Contains(p.SourceFile.Ext) Then p.SourceFile = p.LastOriginalSourceFile
            Dim dgIndexIM = Demuxer.GetDefaults.Find(Function(demuxer) demuxer.Name = "DGIndexIM")
            Dim outFile = p.TempDir + p.SourceFile.Base + ".dgim"
            If Not File.Exists(outFile) Then dgIndexIM.Run()

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
            td.AddCommandLink(Filepath.GetBase(i), i)
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

            Dim generalPage = ui.CreateFlowPage("General")
            generalPage.SuspendLayout()

            Dim cb = ui.AddCheckBox(generalPage)
            cb.Text = "Enable tooltips in menus (restart required)"
            cb.Tooltip = "If you disable this you can still right-click menu items to show the tooltip."
            cb.Checked = s.EnableTooltips
            cb.SaveAction = Sub(value) s.EnableTooltips = value

            cb = ui.AddCheckBox(generalPage)
            cb.Text = "Snap to desktop edges"
            cb.Checked = s.SnapToDesktopEdges
            cb.SaveAction = Sub(value) s.SnapToDesktopEdges = value

            cb = ui.AddCheckBox(generalPage)
            cb.Text = "Show template selection when loading new files"
            cb.Checked = s.ShowTemplateSelection
            cb.SaveAction = Sub(value) s.ShowTemplateSelection = value

            cb = ui.AddCheckBox(generalPage)
            cb.Text = "Reverse mouse wheel video seek direction"
            cb.Checked = s.ReverseVideoScrollDirection
            cb.SaveAction = Sub(value) s.ReverseVideoScrollDirection = value

            Dim mb = ui.AddMenuButtonBlock(Of String)(generalPage)
            mb.Label.Text = "Startup Template:"
            mb.Label.Tooltip = "Template loaded when StaxRip starts."
            mb.MenuButton.Value = s.StartupTemplate
            mb.MenuButton.SaveAction = Sub(value) s.StartupTemplate = value
            mb.MenuButton.Add(From i In Directory.GetFiles(Folder.Template) Select Filepath.GetBase(i))

            Dim num = ui.AddNumericBlock(generalPage)
            num.Label.Text = "UI Scale Factor:"
            num.Label.Tooltip = "Requires to restart StaxRip."
            num.NumEdit.Init(0.3D, 3, 0.05D, 2)
            num.NumEdit.Value = CDec(s.UIScaleFactor)
            num.NumEdit.SaveAction = Sub(value) s.UIScaleFactor = value

            Dim tb = ui.AddTextBlock(generalPage)
            tb.Label.Text = "Remember Window Positions:"
            tb.Label.Tooltip = "Title or beginning of the title of windows of which the location should be remembered. For all windows enter '''all'''."
            tb.Label.Offset = 12
            tb.Edit.Expandet = True
            tb.Edit.Text = s.WindowPositionsRemembered.Join(", ")
            tb.Edit.SaveAction = Sub(value) s.WindowPositionsRemembered = value.SplitNoEmptyAndWhiteSpace(",")

            tb = ui.AddTextBlock(generalPage)
            tb.Label.Text = "Center Screen Window Positions:"
            tb.Label.Tooltip = "Title or beginning of the title of windows to be centered on the screen. For all windows enter '''all'''."
            tb.Label.Offset = 12
            tb.Edit.Expandet = True
            tb.Edit.Text = s.WindowPositionsCenterScreen.Join(", ")
            tb.Edit.SaveAction = Sub(value) s.WindowPositionsCenterScreen = value.SplitNoEmptyAndWhiteSpace(",")

            generalPage.ResumeLayout()

            ui.CreateControlPage(New DemuxingControl, "Demuxing")

            Dim systemPage = ui.CreateFlowPage("System")
            systemPage.SuspendLayout()

            tb = ui.AddTextBlock(systemPage)
            tb.Label.Text = "Prevent Activation:"
            tb.Label.Tooltip = "Prevents the StaxRip window to become the active foreground window if certain applications are currently in the foreground."
            tb.Edit.Expandet = True
            tb.Edit.Text = s.PreventActivation
            tb.Edit.SaveAction = Sub(value) s.PreventActivation = value.ToLower

            Dim mb2 = ui.AddMenuButtonBlock(Of ProcessPriorityClass)(systemPage)
            mb2.Label.Text = "Process Priority:"
            mb2.Label.Tooltip = "Process priority of the applications StaxRip launches."
            mb2.MenuButton.Value = s.ProcessPriority
            mb2.MenuButton.SaveAction = Sub(value) s.ProcessPriority = value

            Dim mb3 = ui.AddMenuButtonBlock(Of ToolStripRenderModeEx)(systemPage)
            mb3.Label.Text = "Menu Style:"
            mb3.Label.Tooltip = "Defines the style used to render main menus, context menus and toolbars."
            mb3.MenuButton.Value = s.ToolStripRenderModeEx
            mb3.MenuButton.SaveAction = Sub(value) s.ToolStripRenderModeEx = value

            num = ui.AddNumericBlock(systemPage)
            num.Label.Text = "Minimum Disk Space:"
            num.Label.Tooltip = "Minimum allowed disk space in GB."
            num.NumEdit.Init(0, 10000, 1)
            num.NumEdit.Value = s.MinimumDiskSpace
            num.NumEdit.SaveAction = Sub(value) s.MinimumDiskSpace = CInt(value)

            num = ui.AddNumericBlock(systemPage)
            num.Label.Text = "Shutdown Timeout:"
            num.Label.Tooltip = "Timeout in seconds before the shutdown is executed."
            num.NumEdit.Value = s.ShutdownTimeout
            num.NumEdit.SaveAction = Sub(value) s.ShutdownTimeout = CInt(value)

            cb = ui.AddCheckBox(systemPage)
            cb.Text = "Prevent system entering standby mode while encoding"
            cb.Checked = s.PreventStandby
            cb.SaveAction = Sub(value) s.PreventStandby = value

            cb = ui.AddCheckBox(systemPage)
            cb.Text = "Use recycle bin when temp files are deleted"
            cb.Checked = s.DeleteTempFilesToRecycleBin
            cb.SaveAction = Sub(value) s.DeleteTempFilesToRecycleBin = value

            systemPage.ResumeLayout()

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
                    If i.Name = s.StringDictionary("RecentExternalApplicationControl") Then
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
        sb.Text = "Please choose a profile from the defaults."
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

                i.DropDownItems.Clear()

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
                    p.Script.InsertAfter("Source", New VideoFilter("Crop", "CropRel", "clip = core.std.CropRel(clip, %crop_left%, %crop_right%, %crop_top%, %crop_bottom%)"))
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
                g.MainForm.AviSynthListView.Load()
                g.MainForm.UpdateTargetParameters(p.Script.GetSeconds, p.Script.GetFramerate)
            End If

            Dim doc As New VideoScript
            doc.Engine = p.Script.Engine
            doc.Filters = p.Script.GetFiltersCopy
            doc.Path = p.TempDir + p.Name + "_preview." + doc.FileType
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
        Dim name = Filepath.GetBase(p.TargetFile)
        If name = "" Then name = Macro.Expand(p.DefaultTargetName)
        If name = "" Then name = Filepath.GetBase(p.SourceFile)
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
        <DispName("Show Confirmation"), DefaultValue(True)> showConfirmation As Boolean,
        <DispName("Template Name"), Description("Name of the template to be loaded after the job was added. Empty to load no template.")>
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
        JobsForm.AddJob(jobPath)

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
        AviSynthListView.ShowEditor()
    End Sub

    <Command("Dialog to configure project options.")>
    Sub ShowOptionsDialog()
        OpenOptionsDialog(Nothing)
    End Sub

    Sub OpenOptionsDialog(pagePath As String)
        Using form As New SimpleSettingsForm(
            "Project Options",
            "In order to save project options go to:",
            "Project > Save As Template",
            "In order to choose a template to be loaded on program startup go to:",
            "Tools > Settings > General > Templates > Default Template")

            Dim ui = form.SimpleUI

            Dim imagePage = ui.CreateFlowPage("Image")

            Dim cb = ui.AddCheckBox(imagePage)
            cb.Text = "Save Thumbnails"
            cb.Tooltip = "Saves thumbnails in the target folder. Customizations can be made in the settings under:" + BR2 + "General > Advanced > Thumbnails"
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
            cb.Tooltip = "In case no resize filter is used or the aspect ratio error exceeds the defined maximal value StaxRip signals the encoder to write the aspect ratio to the bitstream so players that support aspect ratio signaling playback still the proper aspect ratio." + BR2
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

            Dim audioPage = ui.CreateFlowPage("Audio")
            audioPage.SuspendLayout()

            tb = ui.AddTextBlock(audioPage)
            tb.Label.Text = "Preferred Languages:"
            tb.Label.Tooltip = "Preferred audio languages using [http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes two or three letter language code] separated by space, comma or semicolon. For all languages just enter all." + BR2 + String.Join(BR, From i In Language.Languages Where i.IsCommon Select i.ToString + ": " + i.TwoLetterCode + ", " + i.ThreeLetterCode)
            tb.Edit.Text = p.PreferredAudio
            tb.Edit.SaveAction = Sub(value) p.PreferredAudio = value

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

            Dim audioDemux = ui.AddMenuButtonBlock(Of DemuxMode)(audioPage)
            audioDemux.Label.Text = "Demux Audio:"
            audioDemux.MenuButton.Value = p.DemuxAudio
            audioDemux.MenuButton.SaveAction = Sub(value) p.DemuxAudio = value

            cb = ui.AddCheckBox(audioPage)
            cb.Text = "Use AviSynth script as audio source"
            cb.Tooltip = "Sets the AviSynth script (*.avs) as audio source file when loading a source file."
            cb.Checked = p.UseScriptAsAudioSource
            cb.SaveAction = Sub(value) p.UseScriptAsAudioSource = value

            audioPage.ResumeLayout()

            Dim subPage = ui.CreateFlowPage("Subtitles")
            subPage.SuspendLayout()

            tb = ui.AddTextBlock(subPage)
            tb.Label.Text = "Preferred Languages:"
            tb.Label.Tooltip = "Subtitles demuxed and loaded automatically using [http://en.wikipedia.org/wiki/List_of_ISO_639-1_codes two or three letter language code] separated by space, comma or semicolon. For all subtitles just enter all." + BR2 + String.Join(BR, From i In Language.Languages Where i.IsCommon Select i.ToString + ": " + i.TwoLetterCode + ", " + i.ThreeLetterCode)
            tb.Edit.Text = p.PreferredSubtitles
            tb.Edit.SaveAction = Sub(value) p.PreferredSubtitles = value

            Dim tbm = ui.AddTextMenuBlock(subPage)
            tbm.Label.Text = "Stream Name:"
            tbm.Label.Tooltip = "Stream name used for muxing, may contain macros."
            tbm.Edit.Text = p.SubtitleName
            tbm.Edit.SaveAction = Sub(value) p.SubtitleName = value
            tbm.AddMenu("Language English", "%language_english%")
            tbm.AddMenu("Language Native", "%language_native%")

            Dim subDemux = ui.AddMenuButtonBlock(Of DemuxMode)(subPage)
            subDemux.Label.Text = "Demux Subtitles:"
            subDemux.MenuButton.Value = p.DemuxSubtitles
            subDemux.MenuButton.SaveAction = Sub(value) p.DemuxSubtitles = value

            Dim mb = ui.AddMenuButtonBlock(Of DefaultSubtitleMode)(subPage)
            mb.Label.Text = "Default Subtitle:"
            mb.MenuButton.Value = p.DefaultSubtitle
            mb.MenuButton.SaveAction = Sub(value) p.DefaultSubtitle = value

            cb = ui.AddCheckBox(subPage)
            cb.Text = "Convert Sup (PGS/Blu-ray) to IDX (Sub/VobSub/DVD)"
            cb.Tooltip = "Works only with demuxed subtitles."
            cb.Checked = p.ConvertSup2Sub
            cb.SaveAction = Sub(value) p.ConvertSup2Sub = value

            cb = ui.AddCheckBox(subPage)
            cb.Text = "Add hardcoded subtitle"
            cb.Tooltip = "Automatically hardcodes a subtitle." + BR2 + "Supported formats are SRT, ASS and VobSub."
            cb.Checked = p.HarcodedSubtitle
            cb.SaveAction = Sub(value) p.HarcodedSubtitle = value

            subPage.ResumeLayout()

            Dim pathPage = ui.CreateFlowPage("Paths")

            l = ui.AddLabel(pathPage, "Default Target Directory:")
            l.Tooltip = "Leave empty to use the source file directory"

            Dim tm = ui.AddTextMenuBlock(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expandet = True
            tm.Edit.Text = p.DefaultTargetFolder
            tm.Edit.SaveAction = Sub(value) p.DefaultTargetFolder = value
            tm.AddMenu("Edit...", Function() g.BrowseFolder(p.DefaultTargetFolder))
            tm.AddMenu("Directory of source file", "%source_dir%")
            tm.AddMenu("Parent directory of source file directory", "%source_dir_parent%")

            l = ui.AddLabel(pathPage, "Default Target Name:")
            l.Tooltip = "Leave empty to use the source filename"
            l.MarginTop = Font.Height

            tm = ui.AddTextMenuBlock(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expandet = True
            tm.Edit.Text = p.DefaultTargetName
            tm.Edit.SaveAction = Sub(value) p.DefaultTargetName = value
            tm.AddMenu("Name of source file without extension", "%source_name%")
            tm.AddMenu("Name of source file directory", "%source_dir_name%")

            l = ui.AddLabel(pathPage, "Temp Files Directory:")
            l.Tooltip = "Leave empty to use the source file directory"
            l.MarginTop = Font.Height

            Dim tempDirFunc = Function()
                                  Dim tempDir = g.BrowseFolder(p.TempDir)
                                  If tempDir <> "" Then Return tempDir.AppendSeparator + "%source_name%_temp"
                              End Function

            tm = ui.AddTextMenuBlock(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expandet = True
            tm.Edit.Text = p.TempDir
            tm.Edit.SaveAction = Sub(value) p.TempDir = value
            tm.AddMenu("Edit...", tempDirFunc)
            tm.AddMenu("Source File Directory", "%source_dir%%source_name%_temp")

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
            tb.Edit.Expandet = True
            tb.Edit.TextBox.Multiline = True
            tb.Edit.UseMacroEditor = True
            tb.Edit.Text = p.TrimCode
            tb.Edit.SaveAction = Sub(value) p.TrimCode = value

            l = ui.AddLabel(filtersPage, "Code inserted at top of scripts:")
            l.Tooltip = "Code inserted at the top of every script StaxRip generates."
            l.MarginTop = Font.Height \ 2

            tb = ui.AddTextBlock(filtersPage)
            tb.Label.Visible = False
            tb.Edit.Expandet = True
            tb.Edit.TextBox.Multiline = True
            tb.Edit.UseMacroEditor = True
            tb.Edit.Text = p.CodeAtTop
            tb.Edit.SaveAction = Sub(value) p.CodeAtTop = value

            Dim miscPage = ui.CreateFlowPage("Misc")

            cb = ui.AddCheckBox(miscPage)
            cb.Text = "Show all dialogs when invoked from CLI"
            cb.Checked = p.ShowDialogsCLI
            cb.SaveAction = Sub(value) p.ShowDialogsCLI = value

            cb = ui.AddCheckBox(miscPage)
            cb.Text = "Delete temp files directory"
            cb.Checked = p.DeleteTempFilesDir
            cb.SaveAction = Sub(value) p.DeleteTempFilesDir = value

            cb = ui.AddCheckBox(miscPage)
            cb.Text = "Use fixed bitrate"
            cb.Checked = p.BitrateIsFixed
            cb.SaveAction = Sub(value) p.BitrateIsFixed = value

            cb = ui.AddCheckBox(miscPage)
            cb.Text = "Extract timecodes from VFR MKV files"
            cb.Checked = p.ExtractTimecodes
            cb.SaveAction = Sub(value) p.ExtractTimecodes = value

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

                If p.CompCheckRange < 2 OrElse p.CompCheckRange > 20 Then p.CompCheckRange = 5
                If p.TempDir <> "" Then p.TempDir = p.TempDir.AppendSeparator

                If Not p.TempDir?.EndsWith("_temp\") AndAlso p.DeleteTempFilesDir Then
                    MsgInfo("Temp dir will only be deleted when it ends with _temp")
                End If

                UpdateSizeOrBitrate()
                tbBitrate_TextChanged()
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

        MsgWarn("Invalid format, please use:" + BR2 + "4/3 or 4:3 or " & 1.333333)
    End Function

    Sub DisableCropFilter()
        Dim f = p.Script.GetFilter("Crop")

        If Not f Is Nothing AndAlso CInt(p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) = 0 Then
            f.Active = False
            AviSynthListView.Load()
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
        Dim filterProfiles As List(Of FilterCategory)
        Dim getDefaults As Func(Of List(Of FilterCategory))

        If p.Script.Engine = ScriptEngine.AviSynth Then
            filterProfiles = s.AviSynthProfiles
            getDefaults = Function() FilterCategory.GetAviSynthDefaults
        Else
            filterProfiles = s.VapourSynthProfiles
            getDefaults = Function() FilterCategory.GetVapourSynthDefaults
        End If

        Using f As New MacroEditor
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
            End If
        End Using
    End Sub

    <Command("Shows help for command line automation.")>
    Private Sub ShowCommandLineHelp()
        BeginInvoke(Sub() ShowCommandLineHelpInternal())
    End Sub

    <Command("Shows help for script automation.")>
    Private Sub ShowScriptingHelp()
        BeginInvoke(Sub() ShowScriptingHelpInternal())
    End Sub

    Private Sub ShowCommandLineHelpInternal()
        Dim f As New HelpForm()
        f.Owner = Me

        f.Doc.WriteStart("Command Line Reference")
        f.Doc.WriteP("Switches are processed in the order they appear in the command line.")
        f.Doc.WriteP("The command line interface, main menu and Event Commands feature are built on top of a common command engine which exposes a rich set of commands.")
        f.Doc.WriteElement("h2", "Examples")
        f.Doc.WriteP("StaxRip ""C:\Movie\project.srip""")
        f.Doc.WriteP("StaxRip ""C:\Movie\VTS_01_1.VOB"" ""C:\Movie 2\VTS_01_2.VOB""")
        f.Doc.WriteP("StaxRip -LoadTemplate:DVB ""C:\Movie\capture.mpg"" -StartEncoding -Standby")
        f.Doc.WriteP("StaxRip -ShowMsg:""main text..."",""text..."",info")

        Dim commands As New List(Of Command)(CommandManager.Commands.Values)
        commands.Sort()

        Dim commandList As New StringPairList

        For Each command In commands
            Dim params = command.MethodInfo.GetParameters
            Dim cl = "-" + command.MethodInfo.Name + ":"

            For Each param In params
                cl += param.Name + ","
            Next

            cl = cl.TrimEnd(",:".ToCharArray)
            f.Doc.WriteH2(cl)

            For Each param In params
                Dim d = param.GetCustomAttribute(Of DescriptionAttribute)

                If Not d Is Nothing Then
                    f.Doc.WriteP(param.Name + ": " + param.GetCustomAttribute(Of DescriptionAttribute).Description)
                End If
            Next

            Dim enumList As New List(Of String)

            For Each param In params
                If param.ParameterType.IsEnum Then
                    enumList.Add(param.ParameterType.Name + ": " +
                                 System.Enum.GetNames(param.ParameterType).Join(", "))
                End If
            Next

            For Each en In enumList
                f.Doc.WriteP(en)
            Next

            f.Doc.WriteP(command.Attribute.Description)
        Next

        f.Show()
    End Sub

    Private Sub ShowScriptingHelpInternal()
        Dim f As New HelpForm()

        f.Doc.WriteStart("Scripting")
        f.Doc.WriteP("Below is a list with methods available for PowerShell scripting. For more info visit the support forum and read the automation topic in the [https://stax76.gitbooks.io/staxrip-handbook/content/automation.html handbook].")

        Dim commands As New List(Of Command)(CommandManager.Commands.Values)
        commands.Sort()

        Dim commandList As New StringPairList

        For Each command In commands
            Dim params = command.MethodInfo.GetParameters
            Dim cs = command.MethodInfo.Name + "("

            For Each param In params
                cs += param.ParameterType.Name.Replace("Boolean", "bool").
                    Replace("String", "string").Replace("Int32", "int") + " " + param.Name + ", "
            Next

            cs = cs.TrimEnd(", ".ToCharArray) + ")"
            f.Doc.WriteH2(cs)

            For Each param In params
                Dim d = param.GetCustomAttribute(Of DescriptionAttribute)

                If Not d Is Nothing Then
                    f.Doc.WriteP(param.Name + ": " + param.GetCustomAttribute(Of DescriptionAttribute).Description)
                End If
            Next

            Dim enumList As New List(Of String)

            For Each param In params
                If param.ParameterType.IsEnum Then
                    enumList.Add(param.ParameterType.Name + ": " +
                                 System.Enum.GetNames(param.ParameterType).Join(", "))
                End If
            Next

            For Each en In enumList
                f.Doc.WriteP(en)
            Next

            f.Doc.WriteP(command.Attribute.Description)
        Next

        f.Show()
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
        ret.Add("Tools|Log File", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.Page, {"""%text_editor%"" ""%working_dir%%target_name%_staxrip.log"""})
        ret.Add("Tools|Directories", Symbol.Folder)
        ret.Add("Tools|Directories|Source", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%source_dir%"""})
        ret.Add("Tools|Directories|Working", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%working_dir%"""})
        ret.Add("Tools|Directories|Target", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%target_dir%"""})
        ret.Add("Tools|Directories|Settings", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%settings_dir%"""})
        ret.Add("Tools|Directories|Templates", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%settings_dir%TemplatesV2"""})
        ret.Add("Tools|Directories|Plugins", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%plugin_dir%"""})
        ret.Add("Tools|Directories|Startup", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%startup_dir%"""})
        ret.Add("Tools|Directories|Programs", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%programs_dir%"""})
        ret.Add("Tools|Directories|System", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%system_dir%"""})
        ret.Add("Tools|Directories|Scripts", NameOf(g.DefaultCommands.ExecuteCommandLine), {"""%script_dir%"""})
        ret.Add("Tools|Advanced", Symbol.More)
        If Application.StartupPath = "D:\Projekte\VS\VB\StaxRip\bin" Then ret.Add("Tools|Advanced|Test...", NameOf(g.DefaultCommands.Test), Keys.F12)
        ret.Add("Tools|Advanced|Video Comparison...", NameOf(ShowVideoComparison))
        ret.Add("Tools|Advanced|Command Prompt...", NameOf(ShowCommandPrompt), Symbol.fa_terminal)
        ret.Add("Tools|Advanced|Event Commands...", NameOf(ShowEventCommandsDialog), Symbol.LightningBolt)
        ret.Add("Tools|Advanced|Hardcoded Subtitle...", NameOf(ShowHardcodedSubtitleDialog), Keys.Control Or Keys.H, Symbol.Subtitles)
        ret.Add("Tools|Advanced|LAV Filters video decoder configuration...", NameOf(ShowLAVFiltersConfigDialog), Symbol.Filter)
        ret.Add("Tools|Advanced|MediaInfo Folder View...", NameOf(ShowMediaInfoFolderViewDialog), Symbol.Info)
        ret.Add("Tools|Advanced|Reset Setting...", NameOf(ResetSettings))
        ret.Add("Tools|Advanced|Thumbnails Generator...", NameOf(ShowBatchGenerateThumbnailsDialog))

        ret.Add("Tools|Scripts", NameOf(DynamicMenuItem), Symbol.Code, {DynamicMenuItemID.Scripts})
        ret.Add("Tools|Edit Menu...", NameOf(ShowMainMenuEditor))
        ret.Add("Tools|Settings...", NameOf(ShowSettingsDialog), Symbol.Settings, {""})

        ret.Add("Apps|AVSMeter", NameOf(g.DefaultCommands.StartTool), {"AVSMeter"})
        ret.Add("Apps|BDSup2Sub++", NameOf(g.DefaultCommands.StartTool), {"BDSup2Sub++"})
        ret.Add("Apps|Demux", NameOf(g.DefaultCommands.StartTool), {"Demux"})
        ret.Add("Apps|DGIndexNV", NameOf(g.DefaultCommands.StartTool), {"DGIndexNV"})
        ret.Add("Apps|ProjectX", NameOf(g.DefaultCommands.StartTool), {"ProjectX"})
        ret.Add("Apps|VSRip", NameOf(g.DefaultCommands.StartTool), {"VSRip"})

        ret.Add("Apps|-")
        ret.Add("Apps|Manage...", NameOf(ShowAppsDialog))

        ret.Add("Help|Documentation", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.Lightbulb, {"https://stax76.gitbooks.io/staxrip-handbook/content/"})
        ret.Add("Help|Support Forum", Symbol.People)
        ret.Add("Help|Support Forum|forum.doom9.org", NameOf(g.DefaultCommands.ExecuteCommandLine), {"http://forum.doom9.org/showthread.php?t=172068&page=999999"})
        If g.IsCulture("de") Then ret.Add("Help|Support Forum|forum.gleitz.info", NameOf(g.DefaultCommands.ExecuteCommandLine), {"http://forum.gleitz.info/showthread.php?26177-StaxRip-Encoding-Frontend-%28Diskussion%29/page999999"})
        ret.Add("Help|Support Forum|forum.videohelp.com", NameOf(g.DefaultCommands.ExecuteCommandLine), {"http://forum.videohelp.com/threads/369913-StaxRip-x64-for-AviSynth-VapourSynth-x264-x265-GPU-encoding/page999999"})
        ret.Add("Help|Website", Symbol.Globe)
        ret.Add("Help|Website|Issue Tracker", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.fa_bug, {"https://github.com/stax76/staxrip/issues"})
        ret.Add("Help|Website|Release Build", NameOf(g.DefaultCommands.ExecuteCommandLine), {"https://github.com/stax76/staxrip/releases"})
        ret.Add("Help|Website|Test Build", NameOf(g.DefaultCommands.ExecuteCommandLine), {"https://github.com/stax76/staxrip/blob/master/md/test-build.md"})
        ret.Add("Help|Donate", NameOf(g.DefaultCommands.ExecuteCommandLine), Symbol.Heart, {Strings.DonationsURL})
        ret.Add("Help|Scripting", NameOf(ShowScriptingHelp), Symbol.Code)
        ret.Add("Help|Command Line", NameOf(ShowCommandLineHelp), Symbol.fa_terminal)
        ret.Add("Help|Macros", NameOf(g.DefaultCommands.OpenHelpTopic), Symbol.CalculatorPercentage, {"macros"})
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

        ret.Add("1 DVD", NameOf(SetSize), {4480})
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

    <Command("Sets the bitrate according to the compressibility.")>
    Sub SetPercent(<DispName("Percent Value")> value As Integer)
        tbTargetSize.Text = g.GetAutoSize(value).ToString
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
        sb.Text = "Please choose a profile."

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
            AviSynthListView.OnChanged()
            Assistant()
        End If
    End Sub

    Private Sub MainForm_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        UpdateScriptsMenuAsync()
    End Sub

    Private Sub MainForm_Shown() Handles Me.Shown
        Refresh()
        UpdateDynamicMenuAsync()
        UpdateRecentProjectsMenu()
        UpdateTemplatesMenuAsync()

        If Not File.Exists(Package.x265.Path) Then
            MsgError("Files included with StaxRip are missing, maybe the 7-Zip archive wasn't properly unpacked. You can find a packer at [http://www.7-zip.org www.7-zip.org].")
            Close()
            Exit Sub
        End If

        ProcessCommandLine(Environment.GetCommandLineArgs)
        If Not g.PreventSaveSettings Then Activate()
        IsLoading = False
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
                    If Not p.ShowDialogsCLI Then p.NoDialogs = True
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
                ShowScriptingHelp()
            End Try
        Next

        If files.Count > 0 Then
            If Not p.ShowDialogsCLI Then p.NoDialogs = True
            OpenAnyFile(files)
        End If
    End Sub

    <Command("Sets the project option 'Show all dialogs when invoked from CLI'")>
    Private Sub SetShowDialogsCLIOption(showDialogs As Boolean)
        p.ShowDialogsCLI = showDialogs
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
        pEncoder.Controls.Add(c)

        If pEncoder.Controls.Count > 1 Then
            Dim old = pEncoder.Controls(0)
            pEncoder.Controls.Remove(old)
            old.Dispose()
        End If
    End Sub

    Sub UpdateEncoderStateRelatedControls()
        llFilesize.Visible = Not p.VideoEncoder.QualityMode
        tbTargetSize.Visible = Not p.VideoEncoder.QualityMode
        lBitrate.Visible = Not p.VideoEncoder.QualityMode
        tbBitrate.Visible = Not p.VideoEncoder.QualityMode
        lTarget2.Visible = p.VideoEncoder.IsCompCheckEnabled
    End Sub

    <Command("Dialog to open source files.")>
    Private Sub ShowOpenSourceDialog()
        Dim td As New TaskDialog(Of String)
        td.MainInstruction = "Choose a method for opening a source."
        td.AddCommandLink("Single File", "DVD and Blu-ray discs can be ripped very easily using MakeMKV.", "Single File")
        td.AddCommandLink("Blu-ray Folder", "Blu-ray folder directly on the Blu-ray drive or on the hard drive.", "Blu-ray Folder")
        td.AddCommandLink("Merge Files", "Merge multiple source files.", "Merge Files")
        td.AddCommandLink("File Batch", "Fully automated batch processing where every file is a separate encoding.", "File Batch")
        td.AddCommandLink("Directory Batch", "Directories are processed as separate encoding, containing files are merged.", "Directory Batch")

        Select Case td.Show
            Case "Single File"
                Using d As New OpenFileDialog
                    d.SetFilter(FileTypes.Video)
                    d.SetInitDir(s.LastSourceDir)

                    If d.ShowDialog() = DialogResult.OK Then
                        OpenVideoSourceFiles(d.FileNames)
                    End If
                End Using
            Case "Merge Files"
                Using f As New SourceFilesForm()
                    f.Mode = SourceInputMode.Combine
                    f.UpdateControls()

                    If f.ShowDialog() = DialogResult.OK Then
                        Refresh()

                        Select Case Filepath.GetExt(f.Files(0))
                            Case "mpg", "vob"
                                OpenVideoSourceFiles(f.Files)
                            Case Else
                                Using proc As New Proc
                                    proc.Init("Merge source files using Mkvmerge " + Package.mkvmerge.Version)

                                    For Each i In f.Files
                                        Log.WriteLine(MediaInfo.GetSummary(i) + "---------------------------------------------------------" + BR2)
                                    Next

                                    proc.Encoding = Encoding.UTF8
                                    proc.File = Package.mkvmerge.Path
                                    Dim outFile = Filepath.GetDirAndBase(f.Files(0)) + "_merged.mkv"
                                    proc.Arguments = "-o """ + outFile + """ """ + f.Files.Join(""" + """) + """"

                                    Try
                                        proc.Start()
                                    Catch ex As Exception
                                        ProcessForm.CloseProcessForm()
                                        g.ShowException(ex)
                                        MsgInfo("Manual Merging", "Please merge the files manually with a appropriate tool or visit the support forum].")
                                        Throw New AbortException
                                    End Try

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
            Case "File Batch"
                If AbortDueToLowDiskSpace() Then Exit Sub

                Using f As New SourceFilesForm()
                    If p.DefaultTargetName = "%source_dir_name%" Then p.DefaultTargetName = "%source_name%"

                    f.Mode = SourceInputMode.FileBatch
                    f.UpdateControls()

                    If f.ShowDialog() = DialogResult.OK Then
                        Refresh()

                        If p.SourceFiles.Count > 0 AndAlso Not LoadTemplateWithSelectionDialog() Then
                            Exit Sub
                        End If

                        Dim tempPath = Folder.Template + "temp.srip"
                        p.BatchMode = Not f.cbDemuxAndIndex.Checked
                        p.NoDialogs = f.cbDemuxAndIndex.Checked
                        SaveProjectPath(tempPath)

                        For Each i In f.Files
                            OpenProject(tempPath, False)
                            OpenVideoSourceFile(i)
                            g.SetTempDir()
                            AddJob(False, Nothing, False)
                        Next

                        OpenProject(tempPath, False)
                        FileHelp.Delete(tempPath)
                        UpdateRecentProjectsMenu()
                        ShowJobsDialog()
                    End If
                End Using
            Case "Directory Batch"
                If AbortDueToLowDiskSpace() Then Exit Sub

                Using f As New SourceFilesForm()
                    f.Mode = SourceInputMode.DirectoryBatch
                    f.UpdateControls()

                    If f.ShowDialog() = DialogResult.OK Then
                        Refresh()

                        If p.SourceFiles.Count > 0 AndAlso Not LoadTemplateWithSelectionDialog() Then
                            Exit Sub
                        End If

                        Dim tempPath = Folder.Template + "temp.srip"

                        If f.cbDemuxAndIndex.Checked Then p.BatchMode = True

                        SaveProjectPath(tempPath)

                        For Each i In f.DirTree.Paths
                            OpenProject(tempPath, False)
                            OpenVideoSourceFiles(GetSourceFilesFromDir(i))

                            If f.cbDemuxAndIndex.Checked Then
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
                        UpdateRecentProjectsMenu()
                        If f.cbDemuxAndIndex.Checked Then ShowJobsDialog()
                    End If
                End Using
            Case "Blu-ray Folder"
                Using d As New FolderBrowserDialog
                    d.Description = "Please choose a Blu-ray source folder."
                    d.SetSelectedPath(s.Storage.GetString("last blu-ray source folder"))
                    d.ShowNewFolderButton = False

                    If d.ShowDialog = DialogResult.OK Then
                        s.Storage.SetString("last blu-ray source folder", d.SelectedPath.AppendSeparator)
                        Dim srcPath = d.SelectedPath.AppendSeparator

                        If Directory.Exists(srcPath + "BDMV") Then srcPath = srcPath + "BDMV\"
                        If Directory.Exists(srcPath + "PLAYLIST") Then srcPath = srcPath + "PLAYLIST\"

                        If Not srcPath.ToUpper.EndsWith("PLAYLIST\") Then
                            MsgWarn("No playlist directory found.")
                            Exit Sub
                        End If

                        Log.WriteEnvironment()
                        Log.Write("Process Blu-Ray folder using eac3to", """" + Package.eac3to.Path + """ """ + srcPath + """" + BR2)
                        Log.WriteLine("Source Drive Type: " + New DriveInfo(d.SelectedPath).DriveType.ToString + BR)

                        Dim output = ProcessHelp.GetStdOut(Package.eac3to.Path, """" + srcPath + """").Replace(VB6.vbBack, "")

                        Log.WriteLine(output)

                        Dim a = Regex.Split(output, "^\d+\)", RegexOptions.Multiline).ToList

                        If a(0) = "" Then a.RemoveAt(0)

                        Dim td2 As New TaskDialog(Of Integer)
                        td2.MainInstruction = "Please choose a playlist."

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
        Using f As New eac3toForm
            f.PlaylistFolder = playlistFolder
            f.PlaylistID = playlistID

            Dim workDir = playlistFolder.Parent.Parent
            Dim folderName = DirPath.GetName(playlistFolder.Parent.Parent)

            If folderName <> "" AndAlso folderName.Length < 20 Then
                workDir += folderName
            Else
                workDir += "Temp\"
            End If

            If DirPath.IsFixedDrive(workDir) AndAlso Not Directory.Exists(workDir) Then
                Try
                    Directory.CreateDirectory(workDir)
                Catch ex As Exception
                End Try
            End If

            If Not DirPath.IsFixedDrive(workDir) OrElse Not Directory.Exists(workDir) Then
                Using d As New FolderBrowserDialog
                    d.Description = "Please choose a directory for temporary files on a fixed local drive."
                    d.SetSelectedPath(s.Storage.GetString("last blu-ray target folder").Parent)

                    If d.ShowDialog = DialogResult.OK Then
                        workDir = d.SelectedPath.AppendSeparator
                    Else
                        Exit Sub
                    End If
                End Using

                If Not DirPath.IsFixedDrive(workDir) OrElse Not Directory.Exists(workDir) Then
                    MsgError("Only fixed local drives are supported as temp dir.")
                    Exit Sub
                End If
            End If

            f.teTempDir.Text = workDir

            If f.ShowDialog() = DialogResult.OK Then
                Try
                    Dim di As New DriveInfo(f.OutputFolder)

                    If di.AvailableFreeSpace / 1024 ^ 3 < 50 Then
                        MsgError("The target drive has not enough free disk space.")
                        Exit Sub
                    End If

                    Using proc As New Proc
                        proc.TrimChars = {"-"c, " "c}
                        proc.RemoveChars = {CChar(VB6.vbBack)}
                        proc.Init("Demux M2TS using eac3to " + Package.eac3to.Version, "analyze: ", "process: ")
                        proc.File = Package.eac3to.Path
                        proc.Process.StartInfo.Arguments = f.GetArgs(
                            """" + playlistFolder + """ " & playlistID & ")", DirPath.GetName(workDir))

                        Try
                            proc.Start()
                        Catch ex As AbortException
                            ProcessForm.CloseProcessForm()
                            Exit Sub
                        Catch ex As Exception
                            ProcessForm.CloseProcessForm()
                            g.ShowException(ex)
                            Exit Sub
                        End Try
                    End Using

                    s.Storage.SetString("last blu-ray target folder", workDir)
                    Dim fs = f.OutputFolder + DirPath.GetName(workDir) + "." + f.cbVideoOutput.Text.ToLower

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
        sb.Text = "Please choose a profile."

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

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If IsSaveCanceled() Then
            e.Cancel = True
            Exit Sub
        End If

        Hide()
        g.SaveSettings()
        g.RaiseAppEvent(ApplicationEvent.ApplicationExit)
    End Sub

    Private Sub pEncoder_MouseLeave() Handles pEncoder.MouseLeave
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
        sb.Text = "Please choose a setting to reset."

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
        AudioMenu0.Items.Clear()
        AudioMenu1.Items.Clear()
        g.PopulateProfileMenu(AudioMenu0.Items, s.AudioProfiles, Sub() ShowAudioProfilesDialog(0), AddressOf g.LoadAudioProfile0)
        g.PopulateProfileMenu(AudioMenu1.Items, s.AudioProfiles, Sub() ShowAudioProfilesDialog(1), AddressOf g.LoadAudioProfile1)
    End Sub

    Private Sub AviSynthListView_ScriptChanged() Handles AviSynthListView.Changed
        If Not IsLoading AndAlso Not AviSynthListView.IsLoading Then
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
        AviSynthListView.Load()
        If g.IsValidSource(False) Then UpdateTargetParameters(p.Script.GetSeconds, p.Script.GetFramerate)
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
        g.PopulateProfileMenu(EncoderMenu.Items, s.VideoEncoderProfiles, AddressOf ShowEncoderProfilesDialog, AddressOf g.LoadVideoEncoder)
        EncoderMenu.Show(lgbEncoder, 0, 16)
    End Sub

    Private Sub llSize_Click() Handles llFilesize.Click
        UpdateSizeMenu()
        SizeContextMenuStrip.Show(llFilesize, 0, 16)
    End Sub

    Private Sub lAudioProfile0_Click() Handles llAudioProfile0.Click
        AudioMenu0.Show(llAudioProfile0, 0, 16)
    End Sub

    Private Sub lAudioProfile1_Click() Handles llAudioProfile1.Click
        AudioMenu1.Show(llAudioProfile1, 0, 16)
    End Sub

    Private Sub llContainer_Click() Handles llMuxer.Click
        ContainerMenu.Items.Clear()
        g.PopulateProfileMenu(ContainerMenu.Items, s.MuxerProfiles, AddressOf ShowMuxerProfilesDialog, AddressOf p.VideoEncoder.LoadMuxer)
        ContainerMenu.Show(llMuxer, 0, 16)
    End Sub

    Private Sub gbResize_LinkClick() Handles lgbResize.LinkClick
        Dim cms = TextCustomMenu.GetMenu(s.TargetImageSizeMenu, lgbResize.Label, components, AddressOf TargetImageMenuClick)
        Dim helpUrl = If(g.IsCulture("de"), "http://encodingwissen.de/videobild/zielaufloesung", "http://www.doom9.org/index.html?/aspectratios.htm")
        cms.Add("-")
        cms.Add("Image Options...", Sub() OpenOptionsDialog("Image")).SetImage(Symbol.fa_photo)
        cms.Items.Add(New ActionMenuItem("Edit Menu...", Sub() s.TargetImageSizeMenu = TextCustomMenu.EditMenu(s.TargetImageSizeMenu, "Target Image Help...", ApplicationSettings.GetDefaultTargetImageSizeMenu, Me)))
        cms.Add("Help...", Sub() g.ShellExecute(helpUrl)).SetImage(Symbol.Help)
        cms.Show(lgbResize, 0, lgbResize.Label.Height)
    End Sub

    Private Sub llSourceParText_Click(sender As Object, e As EventArgs) Handles llSourceParText.Click
        Dim cms = TextCustomMenu.GetMenu(s.SourceAspectRatioMenu, llSourceParText, components, AddressOf SourceAspectRatioMenuClick)
        Dim helpUrl = If(g.IsCulture("de"), "http://encodingwissen.de/video/anamorph-quelle.html", "http://www.doom9.org/index.html?/capture/par.html")
        cms.Items.Insert(0, New ToolStripSeparator)
        cms.Items.Insert(0, New ActionMenuItem("Automatic 4:3", Sub() SetAutoAspectRatio(False)))
        cms.Items.Insert(0, New ActionMenuItem("Automatic 16:9", Sub() SetAutoAspectRatio(True)))
        cms.Add("-")
        cms.Add("Image Options...", Sub() OpenOptionsDialog("Image")).SetImage(Symbol.fa_photo)
        cms.Items.Add(New ActionMenuItem("Edit Menu...", Sub() s.SourceAspectRatioMenu = TextCustomMenu.EditMenu(s.SourceAspectRatioMenu, "Source Image Help...", ApplicationSettings.GetDefaultSourceAspectRatioMenu, Me)))
        cms.Add("Help...", Sub() g.ShellExecute(helpUrl)).SetImage(Symbol.Help)
        cms.Show(llSourceParText, 0, llSourceParText.Height)
    End Sub

    Sub SourceAspectRatioMenuClick(value As String)
        value = Macro.Expand(value)

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

        MsgWarn("Invalid format, please use:" + BR2 + "4/3 or 4:3 or " & 1.333333)
        s.SourceAspectRatioMenu = TextCustomMenu.EditMenu(s.SourceAspectRatioMenu, Me)
    End Sub

    Sub TargetImageMenuClick(value As String)
        g.EnableFilter("Resize")

        value = Macro.Expand(value)

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
        s.TargetImageSizeMenu = TextCustomMenu.EditMenu(s.TargetImageSizeMenu, Me)
    End Sub

    Sub SetAutoAspectRatio(isAnamorphic As Boolean)
        p.SourceAnamorphic = isAnamorphic
        p.CustomPAR = ""
        p.CustomDAR = ""

        If p.Script.IsFilterActive("Resize)") Then
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
            .SetTip("Cropped image size", lCrop, lCropText)
            .SetTip("Source Display Aspect Ratio", lSourceDar, lSourceDarText)
            .SetTip("Source Pixel Aspect Ratio", lSourcePAR, llSourceParText)
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
        m.Items.Clear()
        Dim exist = File.Exists(ap.File)

        If ap.Streams.Count > 0 Then
            For Each i In ap.Streams
                Dim temp = i

                Dim menuAction = Sub()
                                     If ap.File <> p.LastOriginalSourceFile Then tb.Text = p.LastOriginalSourceFile
                                     tb.Text = temp.Name + " (" + ap.File.Ext + ")"
                                     ap.Stream = temp
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

        If p.Script.GetScript.ToLower.Contains("audiodub(") Then
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
                        m.Add("Files | " + Filepath.GetName(i), Sub() tb.Text = temp)
                    Else
                        m.Add(Filepath.GetName(i), Sub() tb.Text = temp)
                    End If
                Next

                m.Items.Add("-")
            End If
        End If

        m.Add("Open", a, "Change the audio source file.").SetImage(Symbol.OpenFile)
        m.Add("Play Audio", Sub() PlayAudio(ap), "Plays the audio source file with MPC.", exist).SetImage(Symbol.Play)
        m.Add("Play audio and video", Sub() g.PlayScript(p.Script, ap), "Plays the audio source file together with the AviSynth script.", exist AndAlso p.Script.Engine = ScriptEngine.AviSynth)
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
            If p.TempDir = "" Then p.TempDir = Filepath.GetDir(ap.File)
            ap = ObjectHelp.GetCopy(Of AudioProfile)(ap)
            Audio.Process(ap)
            ap.Encode()
        Finally
            ProcessForm.CloseProcessForm()
        End Try
    End Sub

    Sub PlayAudio(ap As AudioProfile)
        g.Play(ap.File)
    End Sub

    Sub UpdateTargetFileMenu()
        TargetFileMenu.Items.Clear()
        TargetFileMenu.Add("Edit...", AddressOf tbTargetFile_DoubleClick, "Change the path of the target file.")
        TargetFileMenu.Add("Play...", Sub() g.Play(p.TargetFile), "Play the target file.", File.Exists(p.TargetFile)).SetImage(Symbol.Play)
        TargetFileMenu.Add("MediaInfo...", Sub() g.DefaultCommands.ShowMediaInfo(p.TargetFile), "Show MediaInfo for the target file.", File.Exists(p.TargetFile)).SetImage(Symbol.Info)
        TargetFileMenu.Add("Explore...", Sub() g.OpenDirAndSelectFile(p.TargetFile, Handle), "Open the target file directory with File Explorer.", Directory.Exists(p.TargetFile.Dir)).SetImage(Symbol.FileExplorer)
        TargetFileMenu.Add("-")
        TargetFileMenu.Add("Copy", Sub() tbTargetFile.Copy(), "", tbTargetFile.Text <> "").SetImage(Symbol.Copy)
        TargetFileMenu.Add("Paste", Sub() tbTargetFile.Paste(), "", Clipboard.GetText.Trim <> "").SetImage(Symbol.Paste)
    End Sub

    Sub UpdateSourceFileMenu()
        SourceFileMenu.Items.Clear()
        Dim isIndex = FileTypes.VideoIndex.Contains(Filepath.GetExt(p.SourceFile))

        SourceFileMenu.Add("Open...", AddressOf ShowOpenSourceDialog, "Open source files").SetImage(Symbol.OpenFile)
        SourceFileMenu.Add("Play...", Sub() g.Play(p.SourceFile), "Play the source file.", File.Exists(p.SourceFile) AndAlso Not isIndex).SetImage(Symbol.Play)
        SourceFileMenu.Add("MediaInfo...", Sub() g.DefaultCommands.ShowMediaInfo(p.SourceFile), "Show MediaInfo for the source file.", File.Exists(p.SourceFile) AndAlso Not isIndex).SetImage(Symbol.Info)
        SourceFileMenu.Add("Explore...", Sub() g.OpenDirAndSelectFile(p.SourceFile, Handle), "Open the source file directory with File Explorer.", File.Exists(p.SourceFile)).SetImage(Symbol.FileExplorer)
        SourceFileMenu.Items.Add("-")
        SourceFileMenu.Add("Copy", Sub() tbSourceFile.Copy(), "Copies the selected text to the clipboard.", tbSourceFile.Text <> "").SetImage(Symbol.Copy)
        SourceFileMenu.Add("Paste", Sub() tbSourceFile.Paste(), "Copies the full source file path to the clipboard.", Clipboard.GetText.Trim <> "").SetImage(Symbol.Paste)
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        'on small screensizes like netbooks dialogs get cut at screensize
        If lgbEncoder.Right > ClientSize.Width Then
            ClientSize = New Size(lgbEncoder.Right + lgbFilters.Left, gbAssistant.Bottom + lgbFilters.Left)
        End If
    End Sub

    <Command("Shows a command prompt with the temp directory of the current project.")>
    Sub ShowCommandPrompt()
        Dim batchCode = ""

        For Each pack In Package.Items.Values
            If TypeOf pack Is PluginPackage Then Continue For
            Dim dir = pack.GetDir
            If Not Directory.Exists(dir) Then Continue For
            If Not dir.Contains(Folder.Startup) Then Continue For

            batchCode += "@set PATH=" + dir + ";%PATH%" + BR
        Next

        Dim batchPath = Folder.Temp + Guid.NewGuid.ToString + ".bat"
        Proc.WriteBatchFile(batchPath, batchCode)

        AddHandler g.MainForm.Disposed, Sub() FileHelp.Delete(batchPath)

        Dim batchProcess As New Process
        batchProcess.StartInfo.FileName = "cmd.exe"
        batchProcess.StartInfo.Arguments = "/k """ + batchPath + """"
        batchProcess.StartInfo.WorkingDirectory = p.TempDir
        batchProcess.Start()
    End Sub

    <Command("Shows a dialog to generate thumbnails.")>
    Sub ShowBatchGenerateThumbnailsDialog()
        Using fd As New OpenFileDialog
            fd.Title = "Select files"
            fd.SetFilter(FileTypes.Video)
            fd.Multiselect = True

            If fd.ShowDialog = DialogResult.OK Then
                Using f As New SimpleSettingsForm("Thumbnails Options")
                    f.Height = f.Height \ 2
                    f.Width = f.Width \ 2

                    Dim ui = f.SimpleUI

                    Dim page = ui.CreateFlowPage("main page")

                    page.SuspendLayout()

                    Dim nb = ui.AddNumericBlock(page)
                    nb.Label.Text = "Thumbnail Width:"
                    nb.NumEdit.Init(200, 4000, 10)
                    nb.NumEdit.Value = s.Storage.GetInt("Thumbnail Width", 260)
                    nb.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Width", CInt(value))

                    nb = ui.AddNumericBlock(page)
                    nb.Label.Text = "Rows:"
                    nb.NumEdit.Init(1, 1000, 1)
                    nb.NumEdit.Value = s.Storage.GetInt("Thumbnail Rows", 12)
                    nb.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Rows", CInt(value))

                    nb = ui.AddNumericBlock(page)
                    nb.Label.Text = "Columns:"
                    nb.NumEdit.Init(1, 1000, 1)
                    nb.NumEdit.Value = s.Storage.GetInt("Thumbnail Columns", 3)
                    nb.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Columns", CInt(value))

                    nb = ui.AddNumericBlock(page)
                    nb.Label.Text = "Compression Quality:"
                    nb.NumEdit.Init(1, 100, 1)
                    nb.NumEdit.Value = s.Storage.GetInt("Thumbnail Compression Quality", 95)
                    nb.NumEdit.SaveAction = Sub(value) s.Storage.SetInt("Thumbnail Compression Quality", CInt(value))

                    page.ResumeLayout()

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

    <Command("Presents MediaInfo of all files in a folder in a list view.")>
    Sub ShowMediaInfoFolderViewDialog()
        Using d As New FolderBrowserDialog
            d.Description = "Please choose a folder to be viewed."
            d.ShowNewFolderButton = False
            d.SetSelectedPath(s.Storage.GetString("MediaInfo Folder View folder"))

            If d.ShowDialog = DialogResult.OK Then
                s.Storage.SetString("MediaInfo Folder View folder", d.SelectedPath)
                Dim f As New MediaInfoFolderViewForm(d.SelectedPath.AppendSeparator)
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