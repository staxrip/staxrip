Imports StaxRip.UI
Imports System.Globalization

Class x264Form
    Inherits DialogBase

#Region " Designer "

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then If Not components Is Nothing Then components.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    Friend WithEvents cbWeightB As System.Windows.Forms.CheckBox
    Friend WithEvents nudBFramesBias As NumEdit
    Friend WithEvents lBias As System.Windows.Forms.Label
    Friend WithEvents lMax As System.Windows.Forms.Label
    Friend WithEvents lMin As System.Windows.Forms.Label
    Friend WithEvents nudGOPSizeMax As NumEdit
    Friend WithEvents nudGOPSizeMin As NumEdit
    Friend WithEvents cbCABAC As System.Windows.Forms.CheckBox
    Friend WithEvents lMEAlgorithm As System.Windows.Forms.Label
    Friend WithEvents cbMEMethod As System.Windows.Forms.ComboBox
    Friend WithEvents cbTrellis As System.Windows.Forms.ComboBox
    Friend WithEvents lTrellis As System.Windows.Forms.Label
    Friend WithEvents cbMixedReferences As System.Windows.Forms.CheckBox
    Friend WithEvents lSubpixelRefinement As System.Windows.Forms.Label
    Friend WithEvents cbSubME As System.Windows.Forms.ComboBox
    Friend WithEvents lAimedQuality As System.Windows.Forms.Label
    Friend WithEvents nudPercent As NumEdit
    Friend WithEvents nudQP As NumEdit
    Friend WithEvents lQP As System.Windows.Forms.Label
    Friend WithEvents lMode2 As System.Windows.Forms.Label
    Friend WithEvents cbFastPSkip As System.Windows.Forms.CheckBox
    Friend WithEvents lDirectMode As System.Windows.Forms.Label
    Friend WithEvents cbDirectMode As System.Windows.Forms.ComboBox
    Friend WithEvents AddCmdlControl As StaxRip.CommandLineControl
    Friend WithEvents AddTurboCmdlControl As StaxRip.CommandLineControl
    Friend WithEvents RemoveTurboCmdlControl As StaxRip.CommandLineControl
    Friend WithEvents nudQPCompCheck As NumEdit
    Friend WithEvents lCRFValueDefining100Quality As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lAQStrengthHint As System.Windows.Forms.Label
    Friend WithEvents cbAQMode As System.Windows.Forms.ComboBox
    Friend WithEvents cbChromaInME As System.Windows.Forms.CheckBox
    Friend WithEvents nudMeRange As NumEdit
    Friend WithEvents lMERange As System.Windows.Forms.Label
    Friend WithEvents cbDCTDecimate As StaxRip.UI.CheckBoxEx
    Friend WithEvents lBufferSize As LabelEx
    Friend WithEvents nudVBVBufSize As NumEdit
    Friend WithEvents nudVBVMaxRate As NumEdit
    Friend WithEvents lMaxBitrate As LabelEx
    Friend WithEvents nudVBVInit As NumEdit
    Friend WithEvents lInitialBuffer As StaxRip.UI.LabelEx
    Friend WithEvents nudIPRatio As NumEdit
    Friend WithEvents lIPRatio As StaxRip.UI.LabelEx
    Friend WithEvents nudQPMin As NumEdit
    Friend WithEvents lQPMinimum As StaxRip.UI.LabelEx
    Friend WithEvents nudQPComp As NumEdit
    Friend WithEvents lQPComp As StaxRip.UI.LabelEx
    Friend WithEvents nudPBRatio As NumEdit
    Friend WithEvents lPBRatio As StaxRip.UI.LabelEx
    Friend WithEvents rtbCommandLine As CommandLineRichTextBox
    Friend WithEvents tcMain As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage8 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage9 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents gbBFrames As System.Windows.Forms.GroupBox
    Friend WithEvents gbFrameOptions As System.Windows.Forms.GroupBox
    Friend WithEvents gbCompressibilityCheck As System.Windows.Forms.GroupBox
    Friend WithEvents cb8x8DCT As System.Windows.Forms.CheckBox
    Friend WithEvents cbi4x4 As System.Windows.Forms.CheckBox
    Friend WithEvents cbP8x8 As System.Windows.Forms.CheckBox
    Friend WithEvents cbP4x4 As System.Windows.Forms.CheckBox
    Friend WithEvents cbI8x8 As System.Windows.Forms.CheckBox
    Friend WithEvents cbb8x8 As System.Windows.Forms.CheckBox
    Friend WithEvents gbPartitions As System.Windows.Forms.GroupBox
    Friend WithEvents gbMotionEstimation As System.Windows.Forms.GroupBox
    Friend WithEvents gbQuantOptions As System.Windows.Forms.GroupBox
    Friend WithEvents gbAnalysisMisc As System.Windows.Forms.GroupBox
    Friend WithEvents gbRC1 As System.Windows.Forms.GroupBox
    Friend WithEvents gbRC2 As System.Windows.Forms.GroupBox
    Friend WithEvents lAimedQualityHint As System.Windows.Forms.Label
    Friend WithEvents lPsyRD As System.Windows.Forms.Label
    Friend WithEvents lPsyTrellis As System.Windows.Forms.Label
    Friend WithEvents nudPsyTrellis As NumEdit
    Friend WithEvents nudPsyRD As NumEdit
    Friend WithEvents lMode As System.Windows.Forms.Label
    Friend WithEvents tpBasic As System.Windows.Forms.TabPage
    Friend WithEvents tpAnalysis As System.Windows.Forms.TabPage
    Friend WithEvents tpFrameOptions As System.Windows.Forms.TabPage
    Friend WithEvents tpRateControl As System.Windows.Forms.TabPage
    Friend WithEvents tpCommandLine As System.Windows.Forms.TabPage
    Friend WithEvents gbAddToToPrecedingPasses As System.Windows.Forms.GroupBox
    Friend WithEvents gbRemoveFromPrecedingPasses As System.Windows.Forms.GroupBox
    Friend WithEvents gbAddToAll As System.Windows.Forms.GroupBox
    Friend WithEvents tpStaxrip As System.Windows.Forms.TabPage
    Friend WithEvents cbProfile As System.Windows.Forms.ComboBox
    Friend WithEvents lProfile As System.Windows.Forms.Label
    Friend WithEvents lPreset As System.Windows.Forms.Label
    Friend WithEvents cbPreset As System.Windows.Forms.ComboBox
    Friend WithEvents lTune As System.Windows.Forms.Label
    Friend WithEvents cbTune As System.Windows.Forms.ComboBox
    Friend WithEvents cbSlowFirstpass As System.Windows.Forms.CheckBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents cbMBTree As System.Windows.Forms.CheckBox
    Friend WithEvents lRcLookahead As StaxRip.UI.LabelEx
    Friend WithEvents nudRcLookahead As NumEdit
    Friend WithEvents nudNoiseReduction As NumEdit
    Friend WithEvents lNoiseReduction As System.Windows.Forms.Label
    Friend WithEvents cbGoTo As System.Windows.Forms.ComboBox
    Friend WithEvents bnProfiles As ButtonEx
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents lSlices As System.Windows.Forms.Label
    Friend WithEvents nudSlices As NumEdit
    Friend WithEvents lSceneCut As System.Windows.Forms.Label
    Friend WithEvents nudSceneCut As NumEdit
    Friend WithEvents lBAdapt As System.Windows.Forms.Label
    Friend WithEvents lBFrames As System.Windows.Forms.Label
    Friend WithEvents nudBFrames As NumEdit
    Friend WithEvents cbBAdapt As System.Windows.Forms.ComboBox
    Friend WithEvents lReferenceFrames As System.Windows.Forms.Label
    Friend WithEvents nudReferenceFrames As NumEdit
    Friend WithEvents cbPsy As System.Windows.Forms.CheckBox
    Friend WithEvents cbDeblock As System.Windows.Forms.CheckBox
    Friend WithEvents lDeblockThresholdHint As System.Windows.Forms.Label
    Friend WithEvents lDeblockStrengthHint As System.Windows.Forms.Label
    Friend WithEvents lStrength As System.Windows.Forms.Label
    Friend WithEvents nudDeblockAlpha As NumEdit
    Friend WithEvents lThreshold As System.Windows.Forms.Label
    Friend WithEvents nudDeblockBeta As NumEdit
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents nudAQStrength As NumEdit
    Friend WithEvents lBPyramidMode As System.Windows.Forms.Label
    Friend WithEvents cbBPyramidMode As System.Windows.Forms.ComboBox
    Friend WithEvents cbDevice As System.Windows.Forms.ComboBox
    Friend WithEvents lDevice As System.Windows.Forms.Label
    Friend WithEvents lWeightP As System.Windows.Forms.Label
    Friend WithEvents cbWeightP As System.Windows.Forms.ComboBox
    Friend WithEvents cbMode As System.Windows.Forms.ComboBox
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents paDeblocking As System.Windows.Forms.Panel
    Friend WithEvents LineControl1 As StaxRip.UI.LineControl
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents LineControl2 As StaxRip.UI.LineControl
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tpMisc As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel5 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents lNalHrd As System.Windows.Forms.Label
    Friend WithEvents cbNalHrd As System.Windows.Forms.ComboBox
    Friend WithEvents cbAud As System.Windows.Forms.CheckBox
    Friend WithEvents l0Auto As StaxRip.UI.LabelEx
    Friend WithEvents cbProgress As System.Windows.Forms.CheckBox
    Friend WithEvents lLevel As System.Windows.Forms.Label
    Friend WithEvents cbPSNR As System.Windows.Forms.CheckBox
    Friend WithEvents cbLevel As System.Windows.Forms.ComboBox
    Friend WithEvents lThreads As System.Windows.Forms.Label
    Friend WithEvents cbSSIM As System.Windows.Forms.CheckBox
    Friend WithEvents nudThreads As NumEdit
    Friend WithEvents cbThreadInput As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents lOverscan As System.Windows.Forms.Label
    Friend WithEvents cbOverscan As System.Windows.Forms.ComboBox
    Friend WithEvents lChromaloc As System.Windows.Forms.Label
    Friend WithEvents nudChromaloc As NumEdit
    Friend WithEvents cbPicStruct As StaxRip.UI.CheckBoxEx
    Friend WithEvents lColormatrix As System.Windows.Forms.Label
    Friend WithEvents cbColormatrix As System.Windows.Forms.ComboBox
    Friend WithEvents lTransfer As System.Windows.Forms.Label
    Friend WithEvents cbTransfer As System.Windows.Forms.ComboBox
    Friend WithEvents lColorprim As System.Windows.Forms.Label
    Friend WithEvents cbColorprim As System.Windows.Forms.ComboBox
    Friend WithEvents lFullrange As System.Windows.Forms.Label
    Friend WithEvents cbFullrange As System.Windows.Forms.ComboBox
    Friend WithEvents lVideoformat As System.Windows.Forms.Label
    Friend WithEvents cbVideoformat As System.Windows.Forms.ComboBox
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents buImport As StaxRip.UI.ButtonEx
    Friend WithEvents cbBlurayCompat As System.Windows.Forms.CheckBox
    Friend WithEvents cbOpenGOP As System.Windows.Forms.CheckBox
    Friend WithEvents cbDepth As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents TableLayoutPanel6 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel7 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel8 As TableLayoutPanel
    Private components As System.ComponentModel.IContainer

    '<System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.nudPBRatio = New StaxRip.UI.NumEdit()
        Me.lPBRatio = New StaxRip.UI.LabelEx()
        Me.nudQPComp = New StaxRip.UI.NumEdit()
        Me.lQPComp = New StaxRip.UI.LabelEx()
        Me.nudQPMin = New StaxRip.UI.NumEdit()
        Me.lQPMinimum = New StaxRip.UI.LabelEx()
        Me.nudIPRatio = New StaxRip.UI.NumEdit()
        Me.lIPRatio = New StaxRip.UI.LabelEx()
        Me.nudVBVInit = New StaxRip.UI.NumEdit()
        Me.lInitialBuffer = New StaxRip.UI.LabelEx()
        Me.nudVBVMaxRate = New StaxRip.UI.NumEdit()
        Me.lMaxBitrate = New StaxRip.UI.LabelEx()
        Me.nudVBVBufSize = New StaxRip.UI.NumEdit()
        Me.lBufferSize = New StaxRip.UI.LabelEx()
        Me.cbAQMode = New System.Windows.Forms.ComboBox()
        Me.lAQStrengthHint = New System.Windows.Forms.Label()
        Me.lMode = New System.Windows.Forms.Label()
        Me.lMax = New System.Windows.Forms.Label()
        Me.lMin = New System.Windows.Forms.Label()
        Me.nudGOPSizeMax = New StaxRip.UI.NumEdit()
        Me.nudGOPSizeMin = New StaxRip.UI.NumEdit()
        Me.cbCABAC = New System.Windows.Forms.CheckBox()
        Me.lMode2 = New System.Windows.Forms.Label()
        Me.nudQP = New StaxRip.UI.NumEdit()
        Me.lQP = New System.Windows.Forms.Label()
        Me.cbDCTDecimate = New StaxRip.UI.CheckBoxEx()
        Me.nudMeRange = New StaxRip.UI.NumEdit()
        Me.lMERange = New System.Windows.Forms.Label()
        Me.cbChromaInME = New System.Windows.Forms.CheckBox()
        Me.cbFastPSkip = New System.Windows.Forms.CheckBox()
        Me.lMEAlgorithm = New System.Windows.Forms.Label()
        Me.cbMEMethod = New System.Windows.Forms.ComboBox()
        Me.cbTrellis = New System.Windows.Forms.ComboBox()
        Me.lTrellis = New System.Windows.Forms.Label()
        Me.cbMixedReferences = New System.Windows.Forms.CheckBox()
        Me.lSubpixelRefinement = New System.Windows.Forms.Label()
        Me.cbSubME = New System.Windows.Forms.ComboBox()
        Me.lDirectMode = New System.Windows.Forms.Label()
        Me.cbDirectMode = New System.Windows.Forms.ComboBox()
        Me.cbWeightB = New System.Windows.Forms.CheckBox()
        Me.nudBFramesBias = New StaxRip.UI.NumEdit()
        Me.lBias = New System.Windows.Forms.Label()
        Me.nudQPCompCheck = New StaxRip.UI.NumEdit()
        Me.lCRFValueDefining100Quality = New System.Windows.Forms.Label()
        Me.lAimedQuality = New System.Windows.Forms.Label()
        Me.nudPercent = New StaxRip.UI.NumEdit()
        Me.rtbCommandLine = New StaxRip.UI.CommandLineRichTextBox()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tcMain = New System.Windows.Forms.TabControl()
        Me.tpBasic = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel7 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel6 = New System.Windows.Forms.TableLayoutPanel()
        Me.cbSlowFirstpass = New System.Windows.Forms.CheckBox()
        Me.cbMode = New System.Windows.Forms.ComboBox()
        Me.cbDepth = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lPreset = New System.Windows.Forms.Label()
        Me.cbPreset = New System.Windows.Forms.ComboBox()
        Me.cbDevice = New System.Windows.Forms.ComboBox()
        Me.cbProfile = New System.Windows.Forms.ComboBox()
        Me.lProfile = New System.Windows.Forms.Label()
        Me.lTune = New System.Windows.Forms.Label()
        Me.lDevice = New System.Windows.Forms.Label()
        Me.cbTune = New System.Windows.Forms.ComboBox()
        Me.tpAnalysis = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.gbQuantOptions = New System.Windows.Forms.GroupBox()
        Me.lPsyRD = New System.Windows.Forms.Label()
        Me.nudPsyTrellis = New StaxRip.UI.NumEdit()
        Me.nudPsyRD = New StaxRip.UI.NumEdit()
        Me.lPsyTrellis = New System.Windows.Forms.Label()
        Me.cbPsy = New System.Windows.Forms.CheckBox()
        Me.gbPartitions = New System.Windows.Forms.GroupBox()
        Me.cbP4x4 = New System.Windows.Forms.CheckBox()
        Me.cbi4x4 = New System.Windows.Forms.CheckBox()
        Me.cb8x8DCT = New System.Windows.Forms.CheckBox()
        Me.cbP8x8 = New System.Windows.Forms.CheckBox()
        Me.cbI8x8 = New System.Windows.Forms.CheckBox()
        Me.cbb8x8 = New System.Windows.Forms.CheckBox()
        Me.gbMotionEstimation = New System.Windows.Forms.GroupBox()
        Me.gbAnalysisMisc = New System.Windows.Forms.GroupBox()
        Me.cbWeightP = New System.Windows.Forms.ComboBox()
        Me.lWeightP = New System.Windows.Forms.Label()
        Me.nudNoiseReduction = New StaxRip.UI.NumEdit()
        Me.lNoiseReduction = New System.Windows.Forms.Label()
        Me.tpFrameOptions = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.gbBFrames = New System.Windows.Forms.GroupBox()
        Me.lBPyramidMode = New System.Windows.Forms.Label()
        Me.cbBPyramidMode = New System.Windows.Forms.ComboBox()
        Me.lBAdapt = New System.Windows.Forms.Label()
        Me.lBFrames = New System.Windows.Forms.Label()
        Me.nudBFrames = New StaxRip.UI.NumEdit()
        Me.cbBAdapt = New System.Windows.Forms.ComboBox()
        Me.gbFrameOptions = New System.Windows.Forms.GroupBox()
        Me.cbOpenGOP = New System.Windows.Forms.CheckBox()
        Me.nudReferenceFrames = New StaxRip.UI.NumEdit()
        Me.nudSlices = New StaxRip.UI.NumEdit()
        Me.nudSceneCut = New StaxRip.UI.NumEdit()
        Me.paDeblocking = New System.Windows.Forms.Panel()
        Me.nudDeblockBeta = New StaxRip.UI.NumEdit()
        Me.nudDeblockAlpha = New StaxRip.UI.NumEdit()
        Me.lStrength = New System.Windows.Forms.Label()
        Me.lDeblockThresholdHint = New System.Windows.Forms.Label()
        Me.lThreshold = New System.Windows.Forms.Label()
        Me.lDeblockStrengthHint = New System.Windows.Forms.Label()
        Me.LineControl1 = New StaxRip.UI.LineControl()
        Me.cbDeblock = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lReferenceFrames = New System.Windows.Forms.Label()
        Me.lSlices = New System.Windows.Forms.Label()
        Me.lSceneCut = New System.Windows.Forms.Label()
        Me.tpRateControl = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.gbRC2 = New System.Windows.Forms.GroupBox()
        Me.lRcLookahead = New StaxRip.UI.LabelEx()
        Me.nudRcLookahead = New StaxRip.UI.NumEdit()
        Me.cbMBTree = New System.Windows.Forms.CheckBox()
        Me.gbRC1 = New System.Windows.Forms.GroupBox()
        Me.LineControl2 = New StaxRip.UI.LineControl()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.nudAQStrength = New StaxRip.UI.NumEdit()
        Me.tpMisc = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.cbBlurayCompat = New System.Windows.Forms.CheckBox()
        Me.cbAud = New System.Windows.Forms.CheckBox()
        Me.l0Auto = New StaxRip.UI.LabelEx()
        Me.cbProgress = New System.Windows.Forms.CheckBox()
        Me.lLevel = New System.Windows.Forms.Label()
        Me.cbPSNR = New System.Windows.Forms.CheckBox()
        Me.cbLevel = New System.Windows.Forms.ComboBox()
        Me.lThreads = New System.Windows.Forms.Label()
        Me.cbSSIM = New System.Windows.Forms.CheckBox()
        Me.nudThreads = New StaxRip.UI.NumEdit()
        Me.cbThreadInput = New System.Windows.Forms.CheckBox()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.lChromaloc = New System.Windows.Forms.Label()
        Me.nudChromaloc = New StaxRip.UI.NumEdit()
        Me.cbPicStruct = New StaxRip.UI.CheckBoxEx()
        Me.lColormatrix = New System.Windows.Forms.Label()
        Me.cbColormatrix = New System.Windows.Forms.ComboBox()
        Me.lTransfer = New System.Windows.Forms.Label()
        Me.cbTransfer = New System.Windows.Forms.ComboBox()
        Me.lColorprim = New System.Windows.Forms.Label()
        Me.cbColorprim = New System.Windows.Forms.ComboBox()
        Me.lFullrange = New System.Windows.Forms.Label()
        Me.cbFullrange = New System.Windows.Forms.ComboBox()
        Me.lVideoformat = New System.Windows.Forms.Label()
        Me.cbVideoformat = New System.Windows.Forms.ComboBox()
        Me.lNalHrd = New System.Windows.Forms.Label()
        Me.lOverscan = New System.Windows.Forms.Label()
        Me.cbNalHrd = New System.Windows.Forms.ComboBox()
        Me.cbOverscan = New System.Windows.Forms.ComboBox()
        Me.tpCommandLine = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.gbAddToAll = New System.Windows.Forms.GroupBox()
        Me.AddCmdlControl = New StaxRip.CommandLineControl()
        Me.gbAddToToPrecedingPasses = New System.Windows.Forms.GroupBox()
        Me.AddTurboCmdlControl = New StaxRip.CommandLineControl()
        Me.gbRemoveFromPrecedingPasses = New System.Windows.Forms.GroupBox()
        Me.RemoveTurboCmdlControl = New StaxRip.CommandLineControl()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.buImport = New StaxRip.UI.ButtonEx()
        Me.tpStaxrip = New System.Windows.Forms.TabPage()
        Me.gbCompressibilityCheck = New System.Windows.Forms.GroupBox()
        Me.lAimedQualityHint = New System.Windows.Forms.Label()
        Me.cbGoTo = New System.Windows.Forms.ComboBox()
        Me.bnProfiles = New StaxRip.UI.ButtonEx()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.TableLayoutPanel8 = New System.Windows.Forms.TableLayoutPanel()
        Me.tcMain.SuspendLayout()
        Me.tpBasic.SuspendLayout()
        Me.TableLayoutPanel7.SuspendLayout()
        Me.TableLayoutPanel6.SuspendLayout()
        Me.tpAnalysis.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.gbQuantOptions.SuspendLayout()
        Me.gbPartitions.SuspendLayout()
        Me.gbMotionEstimation.SuspendLayout()
        Me.gbAnalysisMisc.SuspendLayout()
        Me.tpFrameOptions.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.gbBFrames.SuspendLayout()
        Me.gbFrameOptions.SuspendLayout()
        Me.paDeblocking.SuspendLayout()
        Me.tpRateControl.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.gbRC2.SuspendLayout()
        Me.gbRC1.SuspendLayout()
        Me.tpMisc.SuspendLayout()
        Me.TableLayoutPanel5.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.tpCommandLine.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.gbAddToAll.SuspendLayout()
        Me.gbAddToToPrecedingPasses.SuspendLayout()
        Me.gbRemoveFromPrecedingPasses.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.tpStaxrip.SuspendLayout()
        Me.gbCompressibilityCheck.SuspendLayout()
        Me.TableLayoutPanel8.SuspendLayout()
        Me.SuspendLayout()
        '
        'nudPBRatio
        '
        Me.nudPBRatio.DecimalPlaces = 1
        Me.nudPBRatio.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudPBRatio.Location = New System.Drawing.Point(168, 172)
        Me.nudPBRatio.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudPBRatio.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudPBRatio.Name = "nudPBRatio"
        Me.nudPBRatio.Size = New System.Drawing.Size(90, 37)
        Me.nudPBRatio.TabIndex = 8
        '
        'lPBRatio
        '
        Me.lPBRatio.AutoSize = True
        Me.lPBRatio.Location = New System.Drawing.Point(13, 177)
        Me.lPBRatio.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lPBRatio.Size = New System.Drawing.Size(82, 25)
        Me.lPBRatio.Text = "PB Ratio:"
        Me.lPBRatio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudQPComp
        '
        Me.nudQPComp.DecimalPlaces = 2
        Me.nudQPComp.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudQPComp.Location = New System.Drawing.Point(168, 89)
        Me.nudQPComp.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudQPComp.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudQPComp.Name = "nudQPComp"
        Me.nudQPComp.Size = New System.Drawing.Size(90, 37)
        Me.nudQPComp.TabIndex = 5
        '
        'lQPComp
        '
        Me.lQPComp.AutoSize = True
        Me.lQPComp.Location = New System.Drawing.Point(13, 95)
        Me.lQPComp.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lQPComp.Size = New System.Drawing.Size(150, 25)
        Me.lQPComp.Text = "QP Compression:"
        Me.lQPComp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudQPMin
        '
        Me.nudQPMin.Location = New System.Drawing.Point(168, 47)
        Me.nudQPMin.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudQPMin.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudQPMin.Name = "nudQPMin"
        Me.nudQPMin.Size = New System.Drawing.Size(90, 37)
        Me.nudQPMin.TabIndex = 4
        '
        'lQPMinimum
        '
        Me.lQPMinimum.AutoSize = True
        Me.lQPMinimum.Location = New System.Drawing.Point(13, 50)
        Me.lQPMinimum.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lQPMinimum.Size = New System.Drawing.Size(121, 25)
        Me.lQPMinimum.Text = "QP Minimum:"
        Me.lQPMinimum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudIPRatio
        '
        Me.nudIPRatio.DecimalPlaces = 1
        Me.nudIPRatio.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudIPRatio.Location = New System.Drawing.Point(168, 130)
        Me.nudIPRatio.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudIPRatio.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudIPRatio.Name = "nudIPRatio"
        Me.nudIPRatio.Size = New System.Drawing.Size(90, 37)
        Me.nudIPRatio.TabIndex = 7
        '
        'lIPRatio
        '
        Me.lIPRatio.AutoSize = True
        Me.lIPRatio.Location = New System.Drawing.Point(13, 136)
        Me.lIPRatio.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lIPRatio.Size = New System.Drawing.Size(77, 25)
        Me.lIPRatio.Text = "IP Ratio:"
        Me.lIPRatio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudVBVInit
        '
        Me.nudVBVInit.DecimalPlaces = 1
        Me.nudVBVInit.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudVBVInit.Location = New System.Drawing.Point(127, 300)
        Me.nudVBVInit.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudVBVInit.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudVBVInit.Name = "nudVBVInit"
        Me.nudVBVInit.Size = New System.Drawing.Size(90, 37)
        Me.nudVBVInit.TabIndex = 11
        '
        'lInitialBuffer
        '
        Me.lInitialBuffer.AutoSize = True
        Me.lInitialBuffer.Location = New System.Drawing.Point(8, 306)
        Me.lInitialBuffer.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lInitialBuffer.Size = New System.Drawing.Size(110, 25)
        Me.lInitialBuffer.Text = "Initial Buffer:"
        Me.lInitialBuffer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudVBVMaxRate
        '
        Me.nudVBVMaxRate.Location = New System.Drawing.Point(127, 257)
        Me.nudVBVMaxRate.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudVBVMaxRate.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudVBVMaxRate.Name = "nudVBVMaxRate"
        Me.nudVBVMaxRate.Size = New System.Drawing.Size(90, 37)
        Me.nudVBVMaxRate.TabIndex = 9
        '
        'lMaxBitrate
        '
        Me.lMaxBitrate.AutoSize = True
        Me.lMaxBitrate.Location = New System.Drawing.Point(8, 263)
        Me.lMaxBitrate.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lMaxBitrate.Size = New System.Drawing.Size(104, 25)
        Me.lMaxBitrate.Text = "Max Bitrate:"
        Me.lMaxBitrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudVBVBufSize
        '
        Me.nudVBVBufSize.Location = New System.Drawing.Point(127, 215)
        Me.nudVBVBufSize.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudVBVBufSize.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudVBVBufSize.Name = "nudVBVBufSize"
        Me.nudVBVBufSize.Size = New System.Drawing.Size(90, 37)
        Me.nudVBVBufSize.TabIndex = 6
        '
        'lBufferSize
        '
        Me.lBufferSize.AutoSize = True
        Me.lBufferSize.Location = New System.Drawing.Point(8, 221)
        Me.lBufferSize.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lBufferSize.Size = New System.Drawing.Size(99, 25)
        Me.lBufferSize.Text = "Buffer Size:"
        Me.lBufferSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbAQMode
        '
        Me.cbAQMode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbAQMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAQMode.FormattingEnabled = True
        Me.cbAQMode.Location = New System.Drawing.Point(8, 66)
        Me.cbAQMode.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbAQMode.Name = "cbAQMode"
        Me.cbAQMode.Size = New System.Drawing.Size(423, 33)
        Me.cbAQMode.TabIndex = 2
        '
        'lAQStrengthHint
        '
        Me.lAQStrengthHint.AutoSize = True
        Me.lAQStrengthHint.Location = New System.Drawing.Point(231, 121)
        Me.lAQStrengthHint.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lAQStrengthHint.Name = "lAQStrengthHint"
        Me.lAQStrengthHint.Size = New System.Drawing.Size(42, 25)
        Me.lAQStrengthHint.TabIndex = 8
        Me.lAQStrengthHint.Text = "hint"
        '
        'lMode
        '
        Me.lMode.AutoSize = True
        Me.lMode.Location = New System.Drawing.Point(8, 29)
        Me.lMode.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lMode.Name = "lMode"
        Me.lMode.Size = New System.Drawing.Size(94, 25)
        Me.lMode.TabIndex = 0
        Me.lMode.Text = "AQ Mode:"
        '
        'lMax
        '
        Me.lMax.AutoSize = True
        Me.lMax.Location = New System.Drawing.Point(266, 151)
        Me.lMax.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lMax.Name = "lMax"
        Me.lMax.Size = New System.Drawing.Size(49, 25)
        Me.lMax.TabIndex = 12
        Me.lMax.Text = "Max:"
        '
        'lMin
        '
        Me.lMin.AutoSize = True
        Me.lMin.Location = New System.Drawing.Point(122, 149)
        Me.lMin.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lMin.Name = "lMin"
        Me.lMin.Size = New System.Drawing.Size(46, 25)
        Me.lMin.TabIndex = 7
        Me.lMin.Text = "Min:"
        '
        'nudGOPSizeMax
        '
        Me.nudGOPSizeMax.Increment = New Decimal(New Integer() {50, 0, 0, 0})
        Me.nudGOPSizeMax.Location = New System.Drawing.Point(319, 146)
        Me.nudGOPSizeMax.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudGOPSizeMax.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudGOPSizeMax.Name = "nudGOPSizeMax"
        Me.nudGOPSizeMax.Size = New System.Drawing.Size(90, 37)
        Me.nudGOPSizeMax.TabIndex = 14
        '
        'nudGOPSizeMin
        '
        Me.nudGOPSizeMin.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudGOPSizeMin.Location = New System.Drawing.Point(174, 146)
        Me.nudGOPSizeMin.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudGOPSizeMin.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.nudGOPSizeMin.Name = "nudGOPSizeMin"
        Me.nudGOPSizeMin.Size = New System.Drawing.Size(90, 37)
        Me.nudGOPSizeMin.TabIndex = 9
        '
        'cbCABAC
        '
        Me.cbCABAC.AutoSize = True
        Me.cbCABAC.Location = New System.Drawing.Point(301, 26)
        Me.cbCABAC.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbCABAC.Name = "cbCABAC"
        Me.cbCABAC.Size = New System.Drawing.Size(94, 29)
        Me.cbCABAC.TabIndex = 13
        Me.cbCABAC.Text = "CABAC"
        '
        'lMode2
        '
        Me.lMode2.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lMode2.AutoSize = True
        Me.lMode2.Location = New System.Drawing.Point(4, 255)
        Me.lMode2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lMode2.Name = "lMode2"
        Me.lMode2.Size = New System.Drawing.Size(63, 25)
        Me.lMode2.TabIndex = 7
        Me.lMode2.Text = "Mode:"
        '
        'nudQP
        '
        Me.nudQP.DecimalPlaces = 1
        Me.nudQP.Enabled = False
        Me.nudQP.Location = New System.Drawing.Point(89, 4)
        Me.nudQP.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudQP.Maximum = New Decimal(New Integer() {51, 0, 0, 0})
        Me.nudQP.Name = "nudQP"
        Me.nudQP.Size = New System.Drawing.Size(117, 34)
        Me.nudQP.TabIndex = 5
        '
        'lQP
        '
        Me.lQP.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lQP.AutoSize = True
        Me.lQP.Location = New System.Drawing.Point(4, 8)
        Me.lQP.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lQP.Name = "lQP"
        Me.lQP.Size = New System.Drawing.Size(77, 25)
        Me.lQP.TabIndex = 0
        Me.lQP.Text = "Quality: "
        Me.lQP.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'cbDCTDecimate
        '
        Me.cbDCTDecimate.AutoSize = True
        Me.cbDCTDecimate.Location = New System.Drawing.Point(10, 62)
        Me.cbDCTDecimate.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbDCTDecimate.Size = New System.Drawing.Size(150, 29)
        Me.cbDCTDecimate.Text = "DCT Decimate"
        '
        'nudMeRange
        '
        Me.nudMeRange.Increment = New Decimal(New Integer() {8, 0, 0, 0})
        Me.nudMeRange.Location = New System.Drawing.Point(147, 133)
        Me.nudMeRange.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudMeRange.Maximum = New Decimal(New Integer() {64, 0, 0, 0})
        Me.nudMeRange.Name = "nudMeRange"
        Me.nudMeRange.Size = New System.Drawing.Size(90, 37)
        Me.nudMeRange.TabIndex = 5
        '
        'lMERange
        '
        Me.lMERange.AutoSize = True
        Me.lMERange.Location = New System.Drawing.Point(8, 139)
        Me.lMERange.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lMERange.Name = "lMERange"
        Me.lMERange.Size = New System.Drawing.Size(104, 25)
        Me.lMERange.TabIndex = 3
        Me.lMERange.Text = "M.E. Range:"
        '
        'cbChromaInME
        '
        Me.cbChromaInME.AutoSize = True
        Me.cbChromaInME.Location = New System.Drawing.Point(248, 137)
        Me.cbChromaInME.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbChromaInME.Name = "cbChromaInME"
        Me.cbChromaInME.Size = New System.Drawing.Size(158, 29)
        Me.cbChromaInME.TabIndex = 6
        Me.cbChromaInME.Text = "Chroma in M.E."
        Me.cbChromaInME.UseVisualStyleBackColor = True
        '
        'cbFastPSkip
        '
        Me.cbFastPSkip.AutoSize = True
        Me.cbFastPSkip.Location = New System.Drawing.Point(10, 95)
        Me.cbFastPSkip.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbFastPSkip.Name = "cbFastPSkip"
        Me.cbFastPSkip.Size = New System.Drawing.Size(123, 29)
        Me.cbFastPSkip.TabIndex = 2
        Me.cbFastPSkip.Text = "Fast P Skip"
        Me.cbFastPSkip.UseVisualStyleBackColor = True
        '
        'lMEAlgorithm
        '
        Me.lMEAlgorithm.AutoSize = True
        Me.lMEAlgorithm.Location = New System.Drawing.Point(8, 99)
        Me.lMEAlgorithm.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lMEAlgorithm.Name = "lMEAlgorithm"
        Me.lMEAlgorithm.Size = New System.Drawing.Size(134, 25)
        Me.lMEAlgorithm.TabIndex = 2
        Me.lMEAlgorithm.Text = "M.E. Algorithm:"
        '
        'cbMEMethod
        '
        Me.cbMEMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMEMethod.FormattingEnabled = True
        Me.cbMEMethod.Location = New System.Drawing.Point(147, 95)
        Me.cbMEMethod.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbMEMethod.Name = "cbMEMethod"
        Me.cbMEMethod.Size = New System.Drawing.Size(169, 33)
        Me.cbMEMethod.TabIndex = 4
        '
        'cbTrellis
        '
        Me.cbTrellis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTrellis.FormattingEnabled = True
        Me.cbTrellis.Location = New System.Drawing.Point(281, 31)
        Me.cbTrellis.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbTrellis.Name = "cbTrellis"
        Me.cbTrellis.Size = New System.Drawing.Size(116, 33)
        Me.cbTrellis.TabIndex = 8
        '
        'lTrellis
        '
        Me.lTrellis.AutoSize = True
        Me.lTrellis.Location = New System.Drawing.Point(215, 33)
        Me.lTrellis.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lTrellis.Name = "lTrellis"
        Me.lTrellis.Size = New System.Drawing.Size(58, 25)
        Me.lTrellis.TabIndex = 6
        Me.lTrellis.Text = "Trellis:"
        '
        'cbMixedReferences
        '
        Me.cbMixedReferences.AutoSize = True
        Me.cbMixedReferences.Location = New System.Drawing.Point(10, 31)
        Me.cbMixedReferences.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbMixedReferences.Name = "cbMixedReferences"
        Me.cbMixedReferences.Size = New System.Drawing.Size(175, 29)
        Me.cbMixedReferences.TabIndex = 0
        Me.cbMixedReferences.Text = "Mixed References"
        Me.cbMixedReferences.UseVisualStyleBackColor = True
        '
        'lSubpixelRefinement
        '
        Me.lSubpixelRefinement.AutoSize = True
        Me.lSubpixelRefinement.Location = New System.Drawing.Point(8, 26)
        Me.lSubpixelRefinement.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSubpixelRefinement.Name = "lSubpixelRefinement"
        Me.lSubpixelRefinement.Size = New System.Drawing.Size(177, 25)
        Me.lSubpixelRefinement.TabIndex = 0
        Me.lSubpixelRefinement.Text = "Subpixel Refinement:"
        '
        'cbSubME
        '
        Me.cbSubME.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbSubME.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSubME.FormattingEnabled = True
        Me.cbSubME.Location = New System.Drawing.Point(12, 56)
        Me.cbSubME.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbSubME.MaxDropDownItems = 30
        Me.cbSubME.Name = "cbSubME"
        Me.cbSubME.Size = New System.Drawing.Size(419, 33)
        Me.cbSubME.TabIndex = 1
        '
        'lDirectMode
        '
        Me.lDirectMode.AutoSize = True
        Me.lDirectMode.Location = New System.Drawing.Point(10, 26)
        Me.lDirectMode.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lDirectMode.Name = "lDirectMode"
        Me.lDirectMode.Size = New System.Drawing.Size(114, 25)
        Me.lDirectMode.TabIndex = 0
        Me.lDirectMode.Text = "Direct Mode:"
        '
        'cbDirectMode
        '
        Me.cbDirectMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDirectMode.FormattingEnabled = True
        Me.cbDirectMode.Location = New System.Drawing.Point(10, 58)
        Me.cbDirectMode.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbDirectMode.Name = "cbDirectMode"
        Me.cbDirectMode.Size = New System.Drawing.Size(114, 33)
        Me.cbDirectMode.TabIndex = 1
        '
        'cbWeightB
        '
        Me.cbWeightB.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbWeightB.AutoSize = True
        Me.cbWeightB.Location = New System.Drawing.Point(199, 59)
        Me.cbWeightB.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbWeightB.Name = "cbWeightB"
        Me.cbWeightB.Size = New System.Drawing.Size(227, 29)
        Me.cbWeightB.TabIndex = 4
        Me.cbWeightB.Text = "Weighted Pred. B-frame"
        '
        'nudBFramesBias
        '
        Me.nudBFramesBias.Location = New System.Drawing.Point(112, 171)
        Me.nudBFramesBias.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudBFramesBias.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudBFramesBias.Name = "nudBFramesBias"
        Me.nudBFramesBias.Size = New System.Drawing.Size(90, 37)
        Me.nudBFramesBias.TabIndex = 7
        '
        'lBias
        '
        Me.lBias.AutoSize = True
        Me.lBias.Location = New System.Drawing.Point(14, 176)
        Me.lBias.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lBias.Name = "lBias"
        Me.lBias.Size = New System.Drawing.Size(47, 25)
        Me.lBias.TabIndex = 6
        Me.lBias.Text = "Bias:"
        '
        'nudQPCompCheck
        '
        Me.nudQPCompCheck.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudQPCompCheck.Location = New System.Drawing.Point(447, 197)
        Me.nudQPCompCheck.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudQPCompCheck.Maximum = New Decimal(New Integer() {51, 0, 0, 0})
        Me.nudQPCompCheck.Name = "nudQPCompCheck"
        Me.nudQPCompCheck.Size = New System.Drawing.Size(90, 37)
        Me.nudQPCompCheck.TabIndex = 3
        '
        'lCRFValueDefining100Quality
        '
        Me.lCRFValueDefining100Quality.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lCRFValueDefining100Quality.AutoSize = True
        Me.lCRFValueDefining100Quality.Location = New System.Drawing.Point(212, 203)
        Me.lCRFValueDefining100Quality.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lCRFValueDefining100Quality.Name = "lCRFValueDefining100Quality"
        Me.lCRFValueDefining100Quality.Size = New System.Drawing.Size(229, 25)
        Me.lCRFValueDefining100Quality.TabIndex = 0
        Me.lCRFValueDefining100Quality.Text = "CRF value for 100% quality:"
        '
        'lAimedQuality
        '
        Me.lAimedQuality.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lAimedQuality.AutoSize = True
        Me.lAimedQuality.Location = New System.Drawing.Point(212, 158)
        Me.lAimedQuality.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lAimedQuality.Name = "lAimedQuality"
        Me.lAimedQuality.Size = New System.Drawing.Size(159, 25)
        Me.lAimedQuality.TabIndex = 1
        Me.lAimedQuality.Text = "Aimed Quality (%):"
        '
        'nudPercent
        '
        Me.nudPercent.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudPercent.Increment = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nudPercent.Location = New System.Drawing.Point(447, 151)
        Me.nudPercent.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudPercent.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me.nudPercent.Name = "nudPercent"
        Me.nudPercent.Size = New System.Drawing.Size(90, 37)
        Me.nudPercent.TabIndex = 2
        '
        'rtbCommandLine
        '
        Me.rtbCommandLine.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbCommandLine.BlockPaint = False
        Me.rtbCommandLine.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbCommandLine.LastCommandLine = Nothing
        Me.rtbCommandLine.Location = New System.Drawing.Point(8, 453)
        Me.rtbCommandLine.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.rtbCommandLine.Name = "rtbCommandLine"
        Me.rtbCommandLine.ReadOnly = True
        Me.rtbCommandLine.Size = New System.Drawing.Size(911, 38)
        Me.rtbCommandLine.TabIndex = 1
        Me.rtbCommandLine.Text = ""
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(192, 6)
        '
        'tcMain
        '
        Me.tcMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcMain.Controls.Add(Me.tpBasic)
        Me.tcMain.Controls.Add(Me.tpAnalysis)
        Me.tcMain.Controls.Add(Me.tpFrameOptions)
        Me.tcMain.Controls.Add(Me.tpRateControl)
        Me.tcMain.Controls.Add(Me.tpMisc)
        Me.tcMain.Controls.Add(Me.tpCommandLine)
        Me.tcMain.Controls.Add(Me.tpStaxrip)
        Me.tcMain.Location = New System.Drawing.Point(8, 9)
        Me.tcMain.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(911, 436)
        Me.tcMain.TabIndex = 0
        '
        'tpBasic
        '
        Me.tpBasic.Controls.Add(Me.TableLayoutPanel7)
        Me.tpBasic.Location = New System.Drawing.Point(4, 34)
        Me.tpBasic.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tpBasic.Name = "tpBasic"
        Me.tpBasic.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tpBasic.Size = New System.Drawing.Size(903, 398)
        Me.tpBasic.TabIndex = 0
        Me.tpBasic.Text = "Basic"
        Me.tpBasic.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel7
        '
        Me.TableLayoutPanel7.ColumnCount = 1
        Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel7.Controls.Add(Me.TableLayoutPanel6, 0, 0)
        Me.TableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel7.Location = New System.Drawing.Point(4, 4)
        Me.TableLayoutPanel7.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TableLayoutPanel7.Name = "TableLayoutPanel7"
        Me.TableLayoutPanel7.RowCount = 1
        Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel7.Size = New System.Drawing.Size(895, 390)
        Me.TableLayoutPanel7.TabIndex = 23
        '
        'TableLayoutPanel6
        '
        Me.TableLayoutPanel6.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TableLayoutPanel6.AutoSize = True
        Me.TableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel6.ColumnCount = 3
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.TableLayoutPanel6.Controls.Add(Me.lQP, 0, 0)
        Me.TableLayoutPanel6.Controls.Add(Me.cbSlowFirstpass, 0, 7)
        Me.TableLayoutPanel6.Controls.Add(Me.cbMode, 1, 6)
        Me.TableLayoutPanel6.Controls.Add(Me.cbDepth, 1, 5)
        Me.TableLayoutPanel6.Controls.Add(Me.nudQP, 1, 0)
        Me.TableLayoutPanel6.Controls.Add(Me.lMode2, 0, 6)
        Me.TableLayoutPanel6.Controls.Add(Me.Label4, 0, 5)
        Me.TableLayoutPanel6.Controls.Add(Me.lPreset, 0, 1)
        Me.TableLayoutPanel6.Controls.Add(Me.cbPreset, 1, 1)
        Me.TableLayoutPanel6.Controls.Add(Me.cbDevice, 1, 3)
        Me.TableLayoutPanel6.Controls.Add(Me.cbProfile, 1, 4)
        Me.TableLayoutPanel6.Controls.Add(Me.lProfile, 0, 4)
        Me.TableLayoutPanel6.Controls.Add(Me.lTune, 0, 2)
        Me.TableLayoutPanel6.Controls.Add(Me.lDevice, 0, 3)
        Me.TableLayoutPanel6.Controls.Add(Me.cbTune, 1, 2)
        Me.TableLayoutPanel6.Location = New System.Drawing.Point(296, 32)
        Me.TableLayoutPanel6.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TableLayoutPanel6.Name = "TableLayoutPanel6"
        Me.TableLayoutPanel6.RowCount = 8
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel6.Size = New System.Drawing.Size(302, 325)
        Me.TableLayoutPanel6.TabIndex = 22
        '
        'cbSlowFirstpass
        '
        Me.cbSlowFirstpass.AutoSize = True
        Me.TableLayoutPanel6.SetColumnSpan(Me.cbSlowFirstpass, 2)
        Me.cbSlowFirstpass.Location = New System.Drawing.Point(4, 292)
        Me.cbSlowFirstpass.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbSlowFirstpass.Name = "cbSlowFirstpass"
        Me.cbSlowFirstpass.Size = New System.Drawing.Size(150, 29)
        Me.cbSlowFirstpass.TabIndex = 12
        Me.cbSlowFirstpass.Text = "Slow Firstpass"
        Me.cbSlowFirstpass.UseVisualStyleBackColor = True
        '
        'cbMode
        '
        Me.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMode.FormattingEnabled = True
        Me.cbMode.Location = New System.Drawing.Point(89, 251)
        Me.cbMode.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbMode.MaxDropDownItems = 20
        Me.cbMode.Name = "cbMode"
        Me.cbMode.Size = New System.Drawing.Size(119, 33)
        Me.cbMode.TabIndex = 11
        '
        'cbDepth
        '
        Me.cbDepth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDepth.FormattingEnabled = True
        Me.cbDepth.Location = New System.Drawing.Point(89, 210)
        Me.cbDepth.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbDepth.MaxDropDownItems = 20
        Me.cbDepth.Name = "cbDepth"
        Me.cbDepth.Size = New System.Drawing.Size(119, 33)
        Me.cbDepth.TabIndex = 21
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(4, 214)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 25)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Depth:"
        '
        'lPreset
        '
        Me.lPreset.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lPreset.AutoSize = True
        Me.lPreset.Location = New System.Drawing.Point(4, 50)
        Me.lPreset.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lPreset.Name = "lPreset"
        Me.lPreset.Size = New System.Drawing.Size(64, 25)
        Me.lPreset.TabIndex = 1
        Me.lPreset.Text = "Preset:"
        '
        'cbPreset
        '
        Me.cbPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPreset.FormattingEnabled = True
        Me.cbPreset.Location = New System.Drawing.Point(89, 46)
        Me.cbPreset.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbPreset.MaxDropDownItems = 20
        Me.cbPreset.Name = "cbPreset"
        Me.cbPreset.Size = New System.Drawing.Size(119, 33)
        Me.cbPreset.TabIndex = 6
        '
        'cbDevice
        '
        Me.cbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDevice.FormattingEnabled = True
        Me.cbDevice.Location = New System.Drawing.Point(89, 128)
        Me.cbDevice.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbDevice.MaxDropDownItems = 20
        Me.cbDevice.Name = "cbDevice"
        Me.cbDevice.Size = New System.Drawing.Size(119, 33)
        Me.cbDevice.TabIndex = 9
        '
        'cbProfile
        '
        Me.cbProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbProfile.FormattingEnabled = True
        Me.cbProfile.Location = New System.Drawing.Point(89, 169)
        Me.cbProfile.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbProfile.MaxDropDownItems = 20
        Me.cbProfile.Name = "cbProfile"
        Me.cbProfile.Size = New System.Drawing.Size(119, 33)
        Me.cbProfile.TabIndex = 10
        '
        'lProfile
        '
        Me.lProfile.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lProfile.AutoSize = True
        Me.lProfile.Location = New System.Drawing.Point(4, 173)
        Me.lProfile.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lProfile.Name = "lProfile"
        Me.lProfile.Size = New System.Drawing.Size(66, 25)
        Me.lProfile.TabIndex = 4
        Me.lProfile.Text = "Profile:"
        '
        'lTune
        '
        Me.lTune.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lTune.AutoSize = True
        Me.lTune.Location = New System.Drawing.Point(4, 91)
        Me.lTune.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lTune.Name = "lTune"
        Me.lTune.Size = New System.Drawing.Size(54, 25)
        Me.lTune.TabIndex = 2
        Me.lTune.Text = "Tune:"
        '
        'lDevice
        '
        Me.lDevice.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lDevice.AutoSize = True
        Me.lDevice.Location = New System.Drawing.Point(4, 132)
        Me.lDevice.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lDevice.Name = "lDevice"
        Me.lDevice.Size = New System.Drawing.Size(73, 25)
        Me.lDevice.TabIndex = 3
        Me.lDevice.Text = "Device: "
        '
        'cbTune
        '
        Me.cbTune.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTune.FormattingEnabled = True
        Me.cbTune.Location = New System.Drawing.Point(89, 87)
        Me.cbTune.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbTune.MaxDropDownItems = 20
        Me.cbTune.Name = "cbTune"
        Me.cbTune.Size = New System.Drawing.Size(119, 33)
        Me.cbTune.TabIndex = 8
        '
        'tpAnalysis
        '
        Me.tpAnalysis.Controls.Add(Me.TableLayoutPanel1)
        Me.tpAnalysis.Location = New System.Drawing.Point(4, 34)
        Me.tpAnalysis.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tpAnalysis.Name = "tpAnalysis"
        Me.tpAnalysis.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tpAnalysis.Size = New System.Drawing.Size(903, 398)
        Me.tpAnalysis.TabIndex = 1
        Me.tpAnalysis.Text = "Analysis"
        Me.tpAnalysis.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.gbQuantOptions, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.gbPartitions, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.gbMotionEstimation, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.gbAnalysisMisc, 1, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(4, 4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(895, 390)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'gbQuantOptions
        '
        Me.gbQuantOptions.Controls.Add(Me.lPsyRD)
        Me.gbQuantOptions.Controls.Add(Me.nudPsyTrellis)
        Me.gbQuantOptions.Controls.Add(Me.nudPsyRD)
        Me.gbQuantOptions.Controls.Add(Me.lPsyTrellis)
        Me.gbQuantOptions.Controls.Add(Me.cbPsy)
        Me.gbQuantOptions.Controls.Add(Me.lTrellis)
        Me.gbQuantOptions.Controls.Add(Me.cbTrellis)
        Me.gbQuantOptions.Controls.Add(Me.cbMixedReferences)
        Me.gbQuantOptions.Controls.Add(Me.cbDCTDecimate)
        Me.gbQuantOptions.Controls.Add(Me.cbFastPSkip)
        Me.gbQuantOptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbQuantOptions.Location = New System.Drawing.Point(451, 4)
        Me.gbQuantOptions.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbQuantOptions.Name = "gbQuantOptions"
        Me.gbQuantOptions.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbQuantOptions.Size = New System.Drawing.Size(440, 187)
        Me.gbQuantOptions.TabIndex = 2
        Me.gbQuantOptions.TabStop = False
        Me.gbQuantOptions.Text = "Quant Options"
        '
        'lPsyRD
        '
        Me.lPsyRD.AutoSize = True
        Me.lPsyRD.Location = New System.Drawing.Point(100, 130)
        Me.lPsyRD.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lPsyRD.Name = "lPsyRD"
        Me.lPsyRD.Size = New System.Drawing.Size(40, 25)
        Me.lPsyRD.TabIndex = 4
        Me.lPsyRD.Text = "RD:"
        '
        'nudPsyTrellis
        '
        Me.nudPsyTrellis.DecimalPlaces = 2
        Me.nudPsyTrellis.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudPsyTrellis.Location = New System.Drawing.Point(307, 124)
        Me.nudPsyTrellis.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudPsyTrellis.Name = "nudPsyTrellis"
        Me.nudPsyTrellis.Size = New System.Drawing.Size(90, 37)
        Me.nudPsyTrellis.TabIndex = 9
        '
        'nudPsyRD
        '
        Me.nudPsyRD.DecimalPlaces = 2
        Me.nudPsyRD.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudPsyRD.Location = New System.Drawing.Point(144, 124)
        Me.nudPsyRD.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudPsyRD.Maximum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.nudPsyRD.Name = "nudPsyRD"
        Me.nudPsyRD.Size = New System.Drawing.Size(90, 37)
        Me.nudPsyRD.TabIndex = 5
        '
        'lPsyTrellis
        '
        Me.lPsyTrellis.AutoSize = True
        Me.lPsyTrellis.Location = New System.Drawing.Point(243, 130)
        Me.lPsyTrellis.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lPsyTrellis.Name = "lPsyTrellis"
        Me.lPsyTrellis.Size = New System.Drawing.Size(58, 25)
        Me.lPsyTrellis.TabIndex = 7
        Me.lPsyTrellis.Text = "Trellis:"
        '
        'cbPsy
        '
        Me.cbPsy.AutoSize = True
        Me.cbPsy.Location = New System.Drawing.Point(10, 128)
        Me.cbPsy.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbPsy.Name = "cbPsy"
        Me.cbPsy.Size = New System.Drawing.Size(65, 29)
        Me.cbPsy.TabIndex = 3
        Me.cbPsy.Text = "Psy"
        Me.cbPsy.UseVisualStyleBackColor = True
        '
        'gbPartitions
        '
        Me.gbPartitions.Controls.Add(Me.cbP4x4)
        Me.gbPartitions.Controls.Add(Me.cbi4x4)
        Me.gbPartitions.Controls.Add(Me.cb8x8DCT)
        Me.gbPartitions.Controls.Add(Me.cbP8x8)
        Me.gbPartitions.Controls.Add(Me.cbI8x8)
        Me.gbPartitions.Controls.Add(Me.cbb8x8)
        Me.gbPartitions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbPartitions.Location = New System.Drawing.Point(4, 199)
        Me.gbPartitions.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbPartitions.Name = "gbPartitions"
        Me.gbPartitions.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbPartitions.Size = New System.Drawing.Size(439, 187)
        Me.gbPartitions.TabIndex = 1
        Me.gbPartitions.TabStop = False
        Me.gbPartitions.Text = "Partitions"
        '
        'cbP4x4
        '
        Me.cbP4x4.AutoSize = True
        Me.cbP4x4.Location = New System.Drawing.Point(14, 64)
        Me.cbP4x4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbP4x4.Name = "cbP4x4"
        Me.cbP4x4.Size = New System.Drawing.Size(76, 29)
        Me.cbP4x4.TabIndex = 0
        Me.cbP4x4.Text = "P4x4"
        '
        'cbi4x4
        '
        Me.cbi4x4.AutoSize = True
        Me.cbi4x4.Location = New System.Drawing.Point(14, 97)
        Me.cbi4x4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbi4x4.Name = "cbi4x4"
        Me.cbi4x4.Size = New System.Drawing.Size(71, 29)
        Me.cbi4x4.TabIndex = 1
        Me.cbi4x4.Text = "I4x4"
        '
        'cb8x8DCT
        '
        Me.cb8x8DCT.AutoSize = True
        Me.cb8x8DCT.Location = New System.Drawing.Point(196, 97)
        Me.cb8x8DCT.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cb8x8DCT.Name = "cb8x8DCT"
        Me.cb8x8DCT.Size = New System.Drawing.Size(104, 29)
        Me.cb8x8DCT.TabIndex = 5
        Me.cb8x8DCT.Text = "8x8 DCT"
        '
        'cbP8x8
        '
        Me.cbP8x8.AutoSize = True
        Me.cbP8x8.Location = New System.Drawing.Point(103, 64)
        Me.cbP8x8.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbP8x8.Name = "cbP8x8"
        Me.cbP8x8.Size = New System.Drawing.Size(76, 29)
        Me.cbP8x8.TabIndex = 2
        Me.cbP8x8.Text = "P8x8"
        '
        'cbI8x8
        '
        Me.cbI8x8.AutoSize = True
        Me.cbI8x8.Location = New System.Drawing.Point(196, 64)
        Me.cbI8x8.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbI8x8.Name = "cbI8x8"
        Me.cbI8x8.Size = New System.Drawing.Size(71, 29)
        Me.cbI8x8.TabIndex = 4
        Me.cbI8x8.Text = "I8x8"
        '
        'cbb8x8
        '
        Me.cbb8x8.AutoSize = True
        Me.cbb8x8.Location = New System.Drawing.Point(103, 97)
        Me.cbb8x8.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbb8x8.Name = "cbb8x8"
        Me.cbb8x8.Size = New System.Drawing.Size(76, 29)
        Me.cbb8x8.TabIndex = 3
        Me.cbb8x8.Text = "B8x8"
        '
        'gbMotionEstimation
        '
        Me.gbMotionEstimation.Controls.Add(Me.cbMEMethod)
        Me.gbMotionEstimation.Controls.Add(Me.lSubpixelRefinement)
        Me.gbMotionEstimation.Controls.Add(Me.cbSubME)
        Me.gbMotionEstimation.Controls.Add(Me.nudMeRange)
        Me.gbMotionEstimation.Controls.Add(Me.lMERange)
        Me.gbMotionEstimation.Controls.Add(Me.cbChromaInME)
        Me.gbMotionEstimation.Controls.Add(Me.lMEAlgorithm)
        Me.gbMotionEstimation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbMotionEstimation.Location = New System.Drawing.Point(4, 4)
        Me.gbMotionEstimation.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbMotionEstimation.Name = "gbMotionEstimation"
        Me.gbMotionEstimation.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbMotionEstimation.Size = New System.Drawing.Size(439, 187)
        Me.gbMotionEstimation.TabIndex = 0
        Me.gbMotionEstimation.TabStop = False
        Me.gbMotionEstimation.Text = "Motion Estimation"
        '
        'gbAnalysisMisc
        '
        Me.gbAnalysisMisc.Controls.Add(Me.cbDirectMode)
        Me.gbAnalysisMisc.Controls.Add(Me.cbWeightP)
        Me.gbAnalysisMisc.Controls.Add(Me.lWeightP)
        Me.gbAnalysisMisc.Controls.Add(Me.nudNoiseReduction)
        Me.gbAnalysisMisc.Controls.Add(Me.lNoiseReduction)
        Me.gbAnalysisMisc.Controls.Add(Me.cbWeightB)
        Me.gbAnalysisMisc.Controls.Add(Me.lDirectMode)
        Me.gbAnalysisMisc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbAnalysisMisc.Location = New System.Drawing.Point(451, 199)
        Me.gbAnalysisMisc.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbAnalysisMisc.Name = "gbAnalysisMisc"
        Me.gbAnalysisMisc.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbAnalysisMisc.Size = New System.Drawing.Size(440, 187)
        Me.gbAnalysisMisc.TabIndex = 3
        Me.gbAnalysisMisc.TabStop = False
        Me.gbAnalysisMisc.Text = "Misc"
        '
        'cbWeightP
        '
        Me.cbWeightP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbWeightP.FormattingEnabled = True
        Me.cbWeightP.Location = New System.Drawing.Point(10, 133)
        Me.cbWeightP.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbWeightP.Name = "cbWeightP"
        Me.cbWeightP.Size = New System.Drawing.Size(114, 33)
        Me.cbWeightP.TabIndex = 3
        '
        'lWeightP
        '
        Me.lWeightP.AutoSize = True
        Me.lWeightP.Location = New System.Drawing.Point(10, 101)
        Me.lWeightP.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lWeightP.Name = "lWeightP"
        Me.lWeightP.Size = New System.Drawing.Size(201, 25)
        Me.lWeightP.TabIndex = 2
        Me.lWeightP.Text = "Weighted Pred. P-frame"
        '
        'nudNoiseReduction
        '
        Me.nudNoiseReduction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudNoiseReduction.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.nudNoiseReduction.Location = New System.Drawing.Point(267, 132)
        Me.nudNoiseReduction.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudNoiseReduction.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudNoiseReduction.Name = "nudNoiseReduction"
        Me.nudNoiseReduction.Size = New System.Drawing.Size(90, 37)
        Me.nudNoiseReduction.TabIndex = 6
        '
        'lNoiseReduction
        '
        Me.lNoiseReduction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lNoiseReduction.AutoSize = True
        Me.lNoiseReduction.Location = New System.Drawing.Point(263, 101)
        Me.lNoiseReduction.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lNoiseReduction.Name = "lNoiseReduction"
        Me.lNoiseReduction.Size = New System.Drawing.Size(145, 25)
        Me.lNoiseReduction.TabIndex = 5
        Me.lNoiseReduction.Text = "Noise Reduction:"
        '
        'tpFrameOptions
        '
        Me.tpFrameOptions.Controls.Add(Me.TableLayoutPanel2)
        Me.tpFrameOptions.Location = New System.Drawing.Point(4, 34)
        Me.tpFrameOptions.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tpFrameOptions.Name = "tpFrameOptions"
        Me.tpFrameOptions.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tpFrameOptions.Size = New System.Drawing.Size(903, 398)
        Me.tpFrameOptions.TabIndex = 2
        Me.tpFrameOptions.Text = "Frame Options"
        Me.tpFrameOptions.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.gbBFrames, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.gbFrameOptions, 1, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(4, 4)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(895, 390)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'gbBFrames
        '
        Me.gbBFrames.Controls.Add(Me.lBPyramidMode)
        Me.gbBFrames.Controls.Add(Me.cbBPyramidMode)
        Me.gbBFrames.Controls.Add(Me.lBAdapt)
        Me.gbBFrames.Controls.Add(Me.lBFrames)
        Me.gbBFrames.Controls.Add(Me.nudBFrames)
        Me.gbBFrames.Controls.Add(Me.cbBAdapt)
        Me.gbBFrames.Controls.Add(Me.lBias)
        Me.gbBFrames.Controls.Add(Me.nudBFramesBias)
        Me.gbBFrames.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbBFrames.Location = New System.Drawing.Point(4, 4)
        Me.gbBFrames.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbBFrames.Name = "gbBFrames"
        Me.gbBFrames.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbBFrames.Size = New System.Drawing.Size(439, 382)
        Me.gbBFrames.TabIndex = 0
        Me.gbBFrames.TabStop = False
        Me.gbBFrames.Text = "B-frames"
        '
        'lBPyramidMode
        '
        Me.lBPyramidMode.AutoSize = True
        Me.lBPyramidMode.Location = New System.Drawing.Point(14, 87)
        Me.lBPyramidMode.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lBPyramidMode.Name = "lBPyramidMode"
        Me.lBPyramidMode.Size = New System.Drawing.Size(81, 25)
        Me.lBPyramidMode.TabIndex = 1
        Me.lBPyramidMode.Text = "Pyramid:"
        '
        'cbBPyramidMode
        '
        Me.cbBPyramidMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbBPyramidMode.FormattingEnabled = True
        Me.cbBPyramidMode.Location = New System.Drawing.Point(112, 83)
        Me.cbBPyramidMode.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbBPyramidMode.Name = "cbBPyramidMode"
        Me.cbBPyramidMode.Size = New System.Drawing.Size(105, 33)
        Me.cbBPyramidMode.TabIndex = 4
        '
        'lBAdapt
        '
        Me.lBAdapt.AutoSize = True
        Me.lBAdapt.Location = New System.Drawing.Point(14, 44)
        Me.lBAdapt.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lBAdapt.Name = "lBAdapt"
        Me.lBAdapt.Size = New System.Drawing.Size(87, 25)
        Me.lBAdapt.TabIndex = 0
        Me.lBAdapt.Text = "Adaptive:"
        '
        'lBFrames
        '
        Me.lBFrames.AutoSize = True
        Me.lBFrames.Location = New System.Drawing.Point(14, 130)
        Me.lBFrames.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lBFrames.Name = "lBFrames"
        Me.lBFrames.Size = New System.Drawing.Size(90, 25)
        Me.lBFrames.TabIndex = 3
        Me.lBFrames.Text = "B-Frames:"
        '
        'nudBFrames
        '
        Me.nudBFrames.Location = New System.Drawing.Point(112, 125)
        Me.nudBFrames.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudBFrames.Maximum = New Decimal(New Integer() {16, 0, 0, 0})
        Me.nudBFrames.Name = "nudBFrames"
        Me.nudBFrames.Size = New System.Drawing.Size(90, 37)
        Me.nudBFrames.TabIndex = 5
        '
        'cbBAdapt
        '
        Me.cbBAdapt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbBAdapt.FormattingEnabled = True
        Me.cbBAdapt.Location = New System.Drawing.Point(112, 40)
        Me.cbBAdapt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbBAdapt.Name = "cbBAdapt"
        Me.cbBAdapt.Size = New System.Drawing.Size(105, 33)
        Me.cbBAdapt.TabIndex = 2
        '
        'gbFrameOptions
        '
        Me.gbFrameOptions.Controls.Add(Me.cbOpenGOP)
        Me.gbFrameOptions.Controls.Add(Me.nudGOPSizeMax)
        Me.gbFrameOptions.Controls.Add(Me.nudReferenceFrames)
        Me.gbFrameOptions.Controls.Add(Me.nudGOPSizeMin)
        Me.gbFrameOptions.Controls.Add(Me.nudSlices)
        Me.gbFrameOptions.Controls.Add(Me.nudSceneCut)
        Me.gbFrameOptions.Controls.Add(Me.paDeblocking)
        Me.gbFrameOptions.Controls.Add(Me.LineControl1)
        Me.gbFrameOptions.Controls.Add(Me.cbDeblock)
        Me.gbFrameOptions.Controls.Add(Me.Label1)
        Me.gbFrameOptions.Controls.Add(Me.lMin)
        Me.gbFrameOptions.Controls.Add(Me.lReferenceFrames)
        Me.gbFrameOptions.Controls.Add(Me.lMax)
        Me.gbFrameOptions.Controls.Add(Me.lSlices)
        Me.gbFrameOptions.Controls.Add(Me.cbCABAC)
        Me.gbFrameOptions.Controls.Add(Me.lSceneCut)
        Me.gbFrameOptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbFrameOptions.Location = New System.Drawing.Point(451, 4)
        Me.gbFrameOptions.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbFrameOptions.Name = "gbFrameOptions"
        Me.gbFrameOptions.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbFrameOptions.Size = New System.Drawing.Size(440, 382)
        Me.gbFrameOptions.TabIndex = 1
        Me.gbFrameOptions.TabStop = False
        '
        'cbOpenGOP
        '
        Me.cbOpenGOP.AutoSize = True
        Me.cbOpenGOP.Location = New System.Drawing.Point(20, 190)
        Me.cbOpenGOP.Name = "cbOpenGOP"
        Me.cbOpenGOP.Size = New System.Drawing.Size(123, 29)
        Me.cbOpenGOP.TabIndex = 16
        Me.cbOpenGOP.Text = "Open GOP"
        Me.cbOpenGOP.UseVisualStyleBackColor = True
        '
        'nudReferenceFrames
        '
        Me.nudReferenceFrames.Location = New System.Drawing.Point(174, 23)
        Me.nudReferenceFrames.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudReferenceFrames.Maximum = New Decimal(New Integer() {16, 0, 0, 0})
        Me.nudReferenceFrames.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudReferenceFrames.Name = "nudReferenceFrames"
        Me.nudReferenceFrames.Size = New System.Drawing.Size(90, 37)
        Me.nudReferenceFrames.TabIndex = 4
        Me.nudReferenceFrames.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'nudSlices
        '
        Me.nudSlices.Location = New System.Drawing.Point(174, 104)
        Me.nudSlices.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudSlices.Name = "nudSlices"
        Me.nudSlices.Size = New System.Drawing.Size(90, 37)
        Me.nudSlices.TabIndex = 8
        '
        'nudSceneCut
        '
        Me.nudSceneCut.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudSceneCut.Location = New System.Drawing.Point(174, 64)
        Me.nudSceneCut.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudSceneCut.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudSceneCut.Name = "nudSceneCut"
        Me.nudSceneCut.Size = New System.Drawing.Size(90, 37)
        Me.nudSceneCut.TabIndex = 5
        '
        'paDeblocking
        '
        Me.paDeblocking.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.paDeblocking.Controls.Add(Me.nudDeblockBeta)
        Me.paDeblocking.Controls.Add(Me.nudDeblockAlpha)
        Me.paDeblocking.Controls.Add(Me.lStrength)
        Me.paDeblocking.Controls.Add(Me.lDeblockThresholdHint)
        Me.paDeblocking.Controls.Add(Me.lThreshold)
        Me.paDeblocking.Controls.Add(Me.lDeblockStrengthHint)
        Me.paDeblocking.Location = New System.Drawing.Point(79, 274)
        Me.paDeblocking.Name = "paDeblocking"
        Me.paDeblocking.Size = New System.Drawing.Size(324, 95)
        Me.paDeblocking.TabIndex = 10
        '
        'nudDeblockBeta
        '
        Me.nudDeblockBeta.Location = New System.Drawing.Point(107, 49)
        Me.nudDeblockBeta.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudDeblockBeta.Maximum = New Decimal(New Integer() {6, 0, 0, 0})
        Me.nudDeblockBeta.Minimum = New Decimal(New Integer() {6, 0, 0, -2147483648})
        Me.nudDeblockBeta.Name = "nudDeblockBeta"
        Me.nudDeblockBeta.Size = New System.Drawing.Size(90, 37)
        Me.nudDeblockBeta.TabIndex = 3
        '
        'nudDeblockAlpha
        '
        Me.nudDeblockAlpha.Location = New System.Drawing.Point(107, 7)
        Me.nudDeblockAlpha.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudDeblockAlpha.Maximum = New Decimal(New Integer() {6, 0, 0, 0})
        Me.nudDeblockAlpha.Minimum = New Decimal(New Integer() {6, 0, 0, -2147483648})
        Me.nudDeblockAlpha.Name = "nudDeblockAlpha"
        Me.nudDeblockAlpha.Size = New System.Drawing.Size(90, 37)
        Me.nudDeblockAlpha.TabIndex = 2
        '
        'lStrength
        '
        Me.lStrength.AutoSize = True
        Me.lStrength.Location = New System.Drawing.Point(11, 10)
        Me.lStrength.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lStrength.Name = "lStrength"
        Me.lStrength.Size = New System.Drawing.Size(83, 25)
        Me.lStrength.TabIndex = 0
        Me.lStrength.Text = "Strength:"
        '
        'lDeblockThresholdHint
        '
        Me.lDeblockThresholdHint.AutoSize = True
        Me.lDeblockThresholdHint.Location = New System.Drawing.Point(213, 52)
        Me.lDeblockThresholdHint.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lDeblockThresholdHint.Name = "lDeblockThresholdHint"
        Me.lDeblockThresholdHint.Size = New System.Drawing.Size(52, 25)
        Me.lDeblockThresholdHint.TabIndex = 5
        Me.lDeblockThresholdHint.Text = "(hint)"
        '
        'lThreshold
        '
        Me.lThreshold.AutoSize = True
        Me.lThreshold.Location = New System.Drawing.Point(9, 52)
        Me.lThreshold.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lThreshold.Name = "lThreshold"
        Me.lThreshold.Size = New System.Drawing.Size(94, 25)
        Me.lThreshold.TabIndex = 1
        Me.lThreshold.Text = "Threshold:"
        '
        'lDeblockStrengthHint
        '
        Me.lDeblockStrengthHint.AutoSize = True
        Me.lDeblockStrengthHint.Location = New System.Drawing.Point(213, 10)
        Me.lDeblockStrengthHint.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lDeblockStrengthHint.Name = "lDeblockStrengthHint"
        Me.lDeblockStrengthHint.Size = New System.Drawing.Size(52, 25)
        Me.lDeblockStrengthHint.TabIndex = 4
        Me.lDeblockStrengthHint.Text = "(hint)"
        '
        'LineControl1
        '
        Me.LineControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineControl1.Location = New System.Drawing.Point(147, 244)
        Me.LineControl1.Margin = New System.Windows.Forms.Padding(4, 2, 5, 2)
        Me.LineControl1.Name = "LineControl1"
        Me.LineControl1.Size = New System.Drawing.Size(274, 14)
        Me.LineControl1.TabIndex = 11
        '
        'cbDeblock
        '
        Me.cbDeblock.AutoSize = True
        Me.cbDeblock.Location = New System.Drawing.Point(20, 235)
        Me.cbDeblock.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbDeblock.Name = "cbDeblock"
        Me.cbDeblock.Size = New System.Drawing.Size(128, 29)
        Me.cbDeblock.TabIndex = 6
        Me.cbDeblock.Text = "Deblocking"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 149)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 25)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "GOP Size:"
        '
        'lReferenceFrames
        '
        Me.lReferenceFrames.AutoSize = True
        Me.lReferenceFrames.Location = New System.Drawing.Point(15, 26)
        Me.lReferenceFrames.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lReferenceFrames.Name = "lReferenceFrames"
        Me.lReferenceFrames.Size = New System.Drawing.Size(154, 25)
        Me.lReferenceFrames.TabIndex = 0
        Me.lReferenceFrames.Text = "Reference Frames:"
        '
        'lSlices
        '
        Me.lSlices.AutoSize = True
        Me.lSlices.Location = New System.Drawing.Point(15, 106)
        Me.lSlices.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSlices.Name = "lSlices"
        Me.lSlices.Size = New System.Drawing.Size(59, 25)
        Me.lSlices.TabIndex = 2
        Me.lSlices.Text = "Slices:"
        '
        'lSceneCut
        '
        Me.lSceneCut.AutoSize = True
        Me.lSceneCut.Location = New System.Drawing.Point(15, 68)
        Me.lSceneCut.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lSceneCut.Name = "lSceneCut"
        Me.lSceneCut.Size = New System.Drawing.Size(94, 25)
        Me.lSceneCut.TabIndex = 1
        Me.lSceneCut.Text = "Scene Cut:"
        '
        'tpRateControl
        '
        Me.tpRateControl.Controls.Add(Me.TableLayoutPanel3)
        Me.tpRateControl.Location = New System.Drawing.Point(4, 34)
        Me.tpRateControl.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tpRateControl.Name = "tpRateControl"
        Me.tpRateControl.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tpRateControl.Size = New System.Drawing.Size(903, 398)
        Me.tpRateControl.TabIndex = 3
        Me.tpRateControl.Text = "Rate Control"
        Me.tpRateControl.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel3.Controls.Add(Me.gbRC2, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.gbRC1, 0, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(4, 4)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(895, 390)
        Me.TableLayoutPanel3.TabIndex = 0
        '
        'gbRC2
        '
        Me.gbRC2.Controls.Add(Me.lRcLookahead)
        Me.gbRC2.Controls.Add(Me.nudRcLookahead)
        Me.gbRC2.Controls.Add(Me.cbMBTree)
        Me.gbRC2.Controls.Add(Me.lQPMinimum)
        Me.gbRC2.Controls.Add(Me.nudIPRatio)
        Me.gbRC2.Controls.Add(Me.nudPBRatio)
        Me.gbRC2.Controls.Add(Me.lIPRatio)
        Me.gbRC2.Controls.Add(Me.nudQPMin)
        Me.gbRC2.Controls.Add(Me.nudQPComp)
        Me.gbRC2.Controls.Add(Me.lQPComp)
        Me.gbRC2.Controls.Add(Me.lPBRatio)
        Me.gbRC2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbRC2.Location = New System.Drawing.Point(451, 4)
        Me.gbRC2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbRC2.Name = "gbRC2"
        Me.gbRC2.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbRC2.Size = New System.Drawing.Size(440, 382)
        Me.gbRC2.TabIndex = 1
        Me.gbRC2.TabStop = False
        '
        'lRcLookahead
        '
        Me.lRcLookahead.AutoSize = True
        Me.lRcLookahead.Location = New System.Drawing.Point(13, 220)
        Me.lRcLookahead.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lRcLookahead.Size = New System.Drawing.Size(130, 25)
        Me.lRcLookahead.Text = "RC Lookahead:"
        Me.lRcLookahead.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudRcLookahead
        '
        Me.nudRcLookahead.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudRcLookahead.Location = New System.Drawing.Point(168, 214)
        Me.nudRcLookahead.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudRcLookahead.Maximum = New Decimal(New Integer() {250, 0, 0, 0})
        Me.nudRcLookahead.Name = "nudRcLookahead"
        Me.nudRcLookahead.Size = New System.Drawing.Size(90, 37)
        Me.nudRcLookahead.TabIndex = 9
        '
        'cbMBTree
        '
        Me.cbMBTree.AutoSize = True
        Me.cbMBTree.Location = New System.Drawing.Point(18, 285)
        Me.cbMBTree.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbMBTree.Name = "cbMBTree"
        Me.cbMBTree.Size = New System.Drawing.Size(100, 29)
        Me.cbMBTree.TabIndex = 10
        Me.cbMBTree.Text = "MB Tree"
        Me.cbMBTree.UseVisualStyleBackColor = True
        '
        'gbRC1
        '
        Me.gbRC1.Controls.Add(Me.LineControl2)
        Me.gbRC1.Controls.Add(Me.Label2)
        Me.gbRC1.Controls.Add(Me.nudAQStrength)
        Me.gbRC1.Controls.Add(Me.lBufferSize)
        Me.gbRC1.Controls.Add(Me.lAQStrengthHint)
        Me.gbRC1.Controls.Add(Me.lInitialBuffer)
        Me.gbRC1.Controls.Add(Me.lMode)
        Me.gbRC1.Controls.Add(Me.nudVBVInit)
        Me.gbRC1.Controls.Add(Me.cbAQMode)
        Me.gbRC1.Controls.Add(Me.nudVBVMaxRate)
        Me.gbRC1.Controls.Add(Me.lMaxBitrate)
        Me.gbRC1.Controls.Add(Me.nudVBVBufSize)
        Me.gbRC1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbRC1.Location = New System.Drawing.Point(4, 4)
        Me.gbRC1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbRC1.Name = "gbRC1"
        Me.gbRC1.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbRC1.Size = New System.Drawing.Size(439, 382)
        Me.gbRC1.TabIndex = 0
        Me.gbRC1.TabStop = False
        '
        'LineControl2
        '
        Me.LineControl2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineControl2.Location = New System.Drawing.Point(13, 172)
        Me.LineControl2.Margin = New System.Windows.Forms.Padding(4, 2, 5, 2)
        Me.LineControl2.Name = "LineControl2"
        Me.LineControl2.Size = New System.Drawing.Size(417, 24)
        Me.LineControl2.TabIndex = 3
        Me.LineControl2.Text = "VBV"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 121)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(114, 25)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "AQ Strength:"
        '
        'nudAQStrength
        '
        Me.nudAQStrength.DecimalPlaces = 1
        Me.nudAQStrength.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudAQStrength.Location = New System.Drawing.Point(128, 117)
        Me.nudAQStrength.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudAQStrength.Maximum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.nudAQStrength.Name = "nudAQStrength"
        Me.nudAQStrength.Size = New System.Drawing.Size(90, 37)
        Me.nudAQStrength.TabIndex = 4
        '
        'tpMisc
        '
        Me.tpMisc.Controls.Add(Me.TableLayoutPanel5)
        Me.tpMisc.Location = New System.Drawing.Point(4, 34)
        Me.tpMisc.Name = "tpMisc"
        Me.tpMisc.Padding = New System.Windows.Forms.Padding(3, 3, 3, 3)
        Me.tpMisc.Size = New System.Drawing.Size(903, 398)
        Me.tpMisc.TabIndex = 7
        Me.tpMisc.Text = "Misc"
        Me.tpMisc.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel5
        '
        Me.TableLayoutPanel5.ColumnCount = 2
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel5.Controls.Add(Me.GroupBox5, 0, 0)
        Me.TableLayoutPanel5.Controls.Add(Me.GroupBox6, 1, 0)
        Me.TableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel5.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
        Me.TableLayoutPanel5.RowCount = 1
        Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel5.Size = New System.Drawing.Size(897, 392)
        Me.TableLayoutPanel5.TabIndex = 0
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.cbBlurayCompat)
        Me.GroupBox5.Controls.Add(Me.cbAud)
        Me.GroupBox5.Controls.Add(Me.l0Auto)
        Me.GroupBox5.Controls.Add(Me.cbProgress)
        Me.GroupBox5.Controls.Add(Me.lLevel)
        Me.GroupBox5.Controls.Add(Me.cbPSNR)
        Me.GroupBox5.Controls.Add(Me.cbLevel)
        Me.GroupBox5.Controls.Add(Me.lThreads)
        Me.GroupBox5.Controls.Add(Me.cbSSIM)
        Me.GroupBox5.Controls.Add(Me.nudThreads)
        Me.GroupBox5.Controls.Add(Me.cbThreadInput)
        Me.GroupBox5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox5.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(442, 386)
        Me.GroupBox5.TabIndex = 0
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Input/Output"
        '
        'cbBlurayCompat
        '
        Me.cbBlurayCompat.AutoSize = True
        Me.cbBlurayCompat.Location = New System.Drawing.Point(16, 230)
        Me.cbBlurayCompat.Name = "cbBlurayCompat"
        Me.cbBlurayCompat.Size = New System.Drawing.Size(204, 29)
        Me.cbBlurayCompat.TabIndex = 22
        Me.cbBlurayCompat.Text = "Blu-ray Compatibility"
        Me.cbBlurayCompat.UseVisualStyleBackColor = True
        '
        'cbAud
        '
        Me.cbAud.AutoSize = True
        Me.cbAud.Location = New System.Drawing.Point(16, 193)
        Me.cbAud.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbAud.Name = "cbAud"
        Me.cbAud.Size = New System.Drawing.Size(239, 29)
        Me.cbAud.TabIndex = 16
        Me.cbAud.Text = "Use access unit delimiters"
        '
        'l0Auto
        '
        Me.l0Auto.AutoSize = True
        Me.l0Auto.Location = New System.Drawing.Point(217, 279)
        Me.l0Auto.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.l0Auto.Size = New System.Drawing.Size(85, 25)
        Me.l0Auto.Text = " (0=auto)"
        Me.l0Auto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbProgress
        '
        Me.cbProgress.AutoSize = True
        Me.cbProgress.Location = New System.Drawing.Point(16, 39)
        Me.cbProgress.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbProgress.Name = "cbProgress"
        Me.cbProgress.Size = New System.Drawing.Size(156, 29)
        Me.cbProgress.TabIndex = 12
        Me.cbProgress.Text = "Show Progress"
        '
        'lLevel
        '
        Me.lLevel.AutoSize = True
        Me.lLevel.Location = New System.Drawing.Point(12, 322)
        Me.lLevel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lLevel.Name = "lLevel"
        Me.lLevel.Size = New System.Drawing.Size(55, 25)
        Me.lLevel.TabIndex = 18
        Me.lLevel.Text = "Level:"
        '
        'cbPSNR
        '
        Me.cbPSNR.AutoSize = True
        Me.cbPSNR.Location = New System.Drawing.Point(16, 154)
        Me.cbPSNR.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbPSNR.Name = "cbPSNR"
        Me.cbPSNR.Size = New System.Drawing.Size(192, 29)
        Me.cbPSNR.TabIndex = 15
        Me.cbPSNR.Text = "PSNR Computation"
        '
        'cbLevel
        '
        Me.cbLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLevel.Location = New System.Drawing.Point(110, 319)
        Me.cbLevel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbLevel.MaxDropDownItems = 50
        Me.cbLevel.Name = "cbLevel"
        Me.cbLevel.Size = New System.Drawing.Size(128, 33)
        Me.cbLevel.TabIndex = 21
        '
        'lThreads
        '
        Me.lThreads.AutoSize = True
        Me.lThreads.Location = New System.Drawing.Point(11, 277)
        Me.lThreads.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lThreads.Name = "lThreads"
        Me.lThreads.Size = New System.Drawing.Size(78, 25)
        Me.lThreads.TabIndex = 17
        Me.lThreads.Text = "Threads:"
        '
        'cbSSIM
        '
        Me.cbSSIM.AutoSize = True
        Me.cbSSIM.Location = New System.Drawing.Point(16, 116)
        Me.cbSSIM.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbSSIM.Name = "cbSSIM"
        Me.cbSSIM.Size = New System.Drawing.Size(189, 29)
        Me.cbSSIM.TabIndex = 14
        Me.cbSSIM.Text = "SSIM Computation"
        '
        'nudThreads
        '
        Me.nudThreads.Location = New System.Drawing.Point(110, 273)
        Me.nudThreads.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudThreads.Maximum = New Decimal(New Integer() {64, 0, 0, 0})
        Me.nudThreads.Name = "nudThreads"
        Me.nudThreads.Size = New System.Drawing.Size(90, 37)
        Me.nudThreads.TabIndex = 20
        '
        'cbThreadInput
        '
        Me.cbThreadInput.AutoSize = True
        Me.cbThreadInput.Location = New System.Drawing.Point(16, 77)
        Me.cbThreadInput.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbThreadInput.Name = "cbThreadInput"
        Me.cbThreadInput.Size = New System.Drawing.Size(139, 29)
        Me.cbThreadInput.TabIndex = 13
        Me.cbThreadInput.Text = "Thread Input"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.lChromaloc)
        Me.GroupBox6.Controls.Add(Me.nudChromaloc)
        Me.GroupBox6.Controls.Add(Me.cbPicStruct)
        Me.GroupBox6.Controls.Add(Me.lColormatrix)
        Me.GroupBox6.Controls.Add(Me.cbColormatrix)
        Me.GroupBox6.Controls.Add(Me.lTransfer)
        Me.GroupBox6.Controls.Add(Me.cbTransfer)
        Me.GroupBox6.Controls.Add(Me.lColorprim)
        Me.GroupBox6.Controls.Add(Me.cbColorprim)
        Me.GroupBox6.Controls.Add(Me.lFullrange)
        Me.GroupBox6.Controls.Add(Me.cbFullrange)
        Me.GroupBox6.Controls.Add(Me.lVideoformat)
        Me.GroupBox6.Controls.Add(Me.cbVideoformat)
        Me.GroupBox6.Controls.Add(Me.lNalHrd)
        Me.GroupBox6.Controls.Add(Me.lOverscan)
        Me.GroupBox6.Controls.Add(Me.cbNalHrd)
        Me.GroupBox6.Controls.Add(Me.cbOverscan)
        Me.GroupBox6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox6.Location = New System.Drawing.Point(451, 3)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(443, 386)
        Me.GroupBox6.TabIndex = 1
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Video Usability Info"
        '
        'lChromaloc
        '
        Me.lChromaloc.AutoSize = True
        Me.lChromaloc.Location = New System.Drawing.Point(288, 95)
        Me.lChromaloc.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lChromaloc.Name = "lChromaloc"
        Me.lChromaloc.Size = New System.Drawing.Size(102, 25)
        Me.lChromaloc.TabIndex = 35
        Me.lChromaloc.Text = "Chromaloc:"
        '
        'nudChromaloc
        '
        Me.nudChromaloc.Location = New System.Drawing.Point(294, 125)
        Me.nudChromaloc.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.nudChromaloc.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nudChromaloc.Name = "nudChromaloc"
        Me.nudChromaloc.Size = New System.Drawing.Size(90, 37)
        Me.nudChromaloc.TabIndex = 36
        '
        'cbPicStruct
        '
        Me.cbPicStruct.AutoSize = True
        Me.cbPicStruct.Location = New System.Drawing.Point(292, 51)
        Me.cbPicStruct.Size = New System.Drawing.Size(110, 29)
        Me.cbPicStruct.Text = "Pic Struct"
        Me.cbPicStruct.UseVisualStyleBackColor = True
        '
        'lColormatrix
        '
        Me.lColormatrix.AutoSize = True
        Me.lColormatrix.Location = New System.Drawing.Point(13, 137)
        Me.lColormatrix.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lColormatrix.Name = "lColormatrix"
        Me.lColormatrix.Size = New System.Drawing.Size(108, 25)
        Me.lColormatrix.TabIndex = 32
        Me.lColormatrix.Text = "Colormatrix:"
        '
        'cbColormatrix
        '
        Me.cbColormatrix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbColormatrix.Location = New System.Drawing.Point(137, 134)
        Me.cbColormatrix.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbColormatrix.MaxDropDownItems = 50
        Me.cbColormatrix.Name = "cbColormatrix"
        Me.cbColormatrix.Size = New System.Drawing.Size(128, 33)
        Me.cbColormatrix.TabIndex = 33
        '
        'lTransfer
        '
        Me.lTransfer.AutoSize = True
        Me.lTransfer.Location = New System.Drawing.Point(13, 180)
        Me.lTransfer.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lTransfer.Name = "lTransfer"
        Me.lTransfer.Size = New System.Drawing.Size(77, 25)
        Me.lTransfer.TabIndex = 30
        Me.lTransfer.Text = "Transfer:"
        '
        'cbTransfer
        '
        Me.cbTransfer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTransfer.Location = New System.Drawing.Point(137, 177)
        Me.cbTransfer.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbTransfer.MaxDropDownItems = 50
        Me.cbTransfer.Name = "cbTransfer"
        Me.cbTransfer.Size = New System.Drawing.Size(128, 33)
        Me.cbTransfer.TabIndex = 31
        '
        'lColorprim
        '
        Me.lColorprim.AutoSize = True
        Me.lColorprim.Location = New System.Drawing.Point(13, 95)
        Me.lColorprim.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lColorprim.Name = "lColorprim"
        Me.lColorprim.Size = New System.Drawing.Size(96, 25)
        Me.lColorprim.TabIndex = 28
        Me.lColorprim.Text = "Colorprim:"
        '
        'cbColorprim
        '
        Me.cbColorprim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbColorprim.Location = New System.Drawing.Point(137, 92)
        Me.cbColorprim.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbColorprim.MaxDropDownItems = 50
        Me.cbColorprim.Name = "cbColorprim"
        Me.cbColorprim.Size = New System.Drawing.Size(128, 33)
        Me.cbColorprim.TabIndex = 29
        '
        'lFullrange
        '
        Me.lFullrange.AutoSize = True
        Me.lFullrange.Location = New System.Drawing.Point(13, 308)
        Me.lFullrange.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lFullrange.Name = "lFullrange"
        Me.lFullrange.Size = New System.Drawing.Size(88, 25)
        Me.lFullrange.TabIndex = 26
        Me.lFullrange.Text = "Fullrange:"
        '
        'cbFullrange
        '
        Me.cbFullrange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbFullrange.Location = New System.Drawing.Point(137, 305)
        Me.cbFullrange.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbFullrange.MaxDropDownItems = 50
        Me.cbFullrange.Name = "cbFullrange"
        Me.cbFullrange.Size = New System.Drawing.Size(128, 33)
        Me.cbFullrange.TabIndex = 27
        '
        'lVideoformat
        '
        Me.lVideoformat.AutoSize = True
        Me.lVideoformat.Location = New System.Drawing.Point(13, 223)
        Me.lVideoformat.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lVideoformat.Name = "lVideoformat"
        Me.lVideoformat.Size = New System.Drawing.Size(116, 25)
        Me.lVideoformat.TabIndex = 24
        Me.lVideoformat.Text = "Videoformat:"
        '
        'cbVideoformat
        '
        Me.cbVideoformat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbVideoformat.Location = New System.Drawing.Point(137, 220)
        Me.cbVideoformat.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbVideoformat.MaxDropDownItems = 50
        Me.cbVideoformat.Name = "cbVideoformat"
        Me.cbVideoformat.Size = New System.Drawing.Size(128, 33)
        Me.cbVideoformat.TabIndex = 25
        '
        'lNalHrd
        '
        Me.lNalHrd.AutoSize = True
        Me.lNalHrd.Location = New System.Drawing.Point(13, 53)
        Me.lNalHrd.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lNalHrd.Name = "lNalHrd"
        Me.lNalHrd.Size = New System.Drawing.Size(90, 25)
        Me.lNalHrd.TabIndex = 19
        Me.lNalHrd.Text = "HRD Info:"
        '
        'lOverscan
        '
        Me.lOverscan.AutoSize = True
        Me.lOverscan.Location = New System.Drawing.Point(13, 266)
        Me.lOverscan.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lOverscan.Name = "lOverscan"
        Me.lOverscan.Size = New System.Drawing.Size(89, 25)
        Me.lOverscan.TabIndex = 22
        Me.lOverscan.Text = "Overscan:"
        '
        'cbNalHrd
        '
        Me.cbNalHrd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbNalHrd.Location = New System.Drawing.Point(137, 49)
        Me.cbNalHrd.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbNalHrd.MaxDropDownItems = 50
        Me.cbNalHrd.Name = "cbNalHrd"
        Me.cbNalHrd.Size = New System.Drawing.Size(128, 33)
        Me.cbNalHrd.TabIndex = 22
        '
        'cbOverscan
        '
        Me.cbOverscan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbOverscan.Location = New System.Drawing.Point(137, 262)
        Me.cbOverscan.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbOverscan.MaxDropDownItems = 50
        Me.cbOverscan.Name = "cbOverscan"
        Me.cbOverscan.Size = New System.Drawing.Size(128, 33)
        Me.cbOverscan.TabIndex = 23
        '
        'tpCommandLine
        '
        Me.tpCommandLine.Controls.Add(Me.TableLayoutPanel4)
        Me.tpCommandLine.Location = New System.Drawing.Point(4, 34)
        Me.tpCommandLine.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tpCommandLine.Name = "tpCommandLine"
        Me.tpCommandLine.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tpCommandLine.Size = New System.Drawing.Size(903, 398)
        Me.tpCommandLine.TabIndex = 5
        Me.tpCommandLine.Text = "Command Line"
        Me.tpCommandLine.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 1
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.gbAddToAll, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.gbAddToToPrecedingPasses, 0, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.gbRemoveFromPrecedingPasses, 0, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.FlowLayoutPanel1, 0, 3)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(4, 4)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 4
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(895, 390)
        Me.TableLayoutPanel4.TabIndex = 0
        '
        'gbAddToAll
        '
        Me.gbAddToAll.Controls.Add(Me.AddCmdlControl)
        Me.gbAddToAll.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbAddToAll.Location = New System.Drawing.Point(4, 4)
        Me.gbAddToAll.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbAddToAll.Name = "gbAddToAll"
        Me.gbAddToAll.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbAddToAll.Size = New System.Drawing.Size(887, 106)
        Me.gbAddToAll.TabIndex = 0
        Me.gbAddToAll.TabStop = False
        Me.gbAddToAll.Text = "Custom Switches (used in all modes and passes)"
        '
        'AddCmdlControl
        '
        Me.AddCmdlControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AddCmdlControl.Location = New System.Drawing.Point(4, 28)
        Me.AddCmdlControl.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.AddCmdlControl.Name = "AddCmdlControl"
        Me.AddCmdlControl.Size = New System.Drawing.Size(879, 74)
        Me.AddCmdlControl.TabIndex = 0
        '
        'gbAddToToPrecedingPasses
        '
        Me.gbAddToToPrecedingPasses.Controls.Add(Me.AddTurboCmdlControl)
        Me.gbAddToToPrecedingPasses.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbAddToToPrecedingPasses.Location = New System.Drawing.Point(4, 232)
        Me.gbAddToToPrecedingPasses.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbAddToToPrecedingPasses.Name = "gbAddToToPrecedingPasses"
        Me.gbAddToToPrecedingPasses.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbAddToToPrecedingPasses.Size = New System.Drawing.Size(887, 106)
        Me.gbAddToToPrecedingPasses.TabIndex = 2
        Me.gbAddToToPrecedingPasses.TabStop = False
        Me.gbAddToToPrecedingPasses.Text = "Add to to preceding passes:"
        '
        'AddTurboCmdlControl
        '
        Me.AddTurboCmdlControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AddTurboCmdlControl.Location = New System.Drawing.Point(4, 28)
        Me.AddTurboCmdlControl.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.AddTurboCmdlControl.Name = "AddTurboCmdlControl"
        Me.AddTurboCmdlControl.Size = New System.Drawing.Size(879, 74)
        Me.AddTurboCmdlControl.TabIndex = 0
        '
        'gbRemoveFromPrecedingPasses
        '
        Me.gbRemoveFromPrecedingPasses.Controls.Add(Me.RemoveTurboCmdlControl)
        Me.gbRemoveFromPrecedingPasses.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbRemoveFromPrecedingPasses.Location = New System.Drawing.Point(4, 118)
        Me.gbRemoveFromPrecedingPasses.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbRemoveFromPrecedingPasses.Name = "gbRemoveFromPrecedingPasses"
        Me.gbRemoveFromPrecedingPasses.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbRemoveFromPrecedingPasses.Size = New System.Drawing.Size(887, 106)
        Me.gbRemoveFromPrecedingPasses.TabIndex = 1
        Me.gbRemoveFromPrecedingPasses.TabStop = False
        Me.gbRemoveFromPrecedingPasses.Text = "Remove from preceding passes:"
        '
        'RemoveTurboCmdlControl
        '
        Me.RemoveTurboCmdlControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RemoveTurboCmdlControl.Location = New System.Drawing.Point(4, 28)
        Me.RemoveTurboCmdlControl.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RemoveTurboCmdlControl.Name = "RemoveTurboCmdlControl"
        Me.RemoveTurboCmdlControl.Size = New System.Drawing.Size(879, 74)
        Me.RemoveTurboCmdlControl.TabIndex = 0
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Controls.Add(Me.buImport)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 345)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(411, 41)
        Me.FlowLayoutPanel1.TabIndex = 3
        '
        'buImport
        '
        Me.buImport.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.buImport.AutoSize = True
        Me.buImport.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.buImport.Location = New System.Drawing.Point(3, 3)
        Me.buImport.Size = New System.Drawing.Size(405, 35)
        Me.buImport.Text = "Import settings from command line in clipboard"
        '
        'tpStaxrip
        '
        Me.tpStaxrip.Controls.Add(Me.gbCompressibilityCheck)
        Me.tpStaxrip.Location = New System.Drawing.Point(4, 34)
        Me.tpStaxrip.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tpStaxrip.Name = "tpStaxrip"
        Me.tpStaxrip.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.tpStaxrip.Size = New System.Drawing.Size(903, 398)
        Me.tpStaxrip.TabIndex = 6
        Me.tpStaxrip.Text = "Other"
        Me.tpStaxrip.UseVisualStyleBackColor = True
        '
        'gbCompressibilityCheck
        '
        Me.gbCompressibilityCheck.Controls.Add(Me.nudQPCompCheck)
        Me.gbCompressibilityCheck.Controls.Add(Me.lCRFValueDefining100Quality)
        Me.gbCompressibilityCheck.Controls.Add(Me.lAimedQualityHint)
        Me.gbCompressibilityCheck.Controls.Add(Me.nudPercent)
        Me.gbCompressibilityCheck.Controls.Add(Me.lAimedQuality)
        Me.gbCompressibilityCheck.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbCompressibilityCheck.Location = New System.Drawing.Point(4, 4)
        Me.gbCompressibilityCheck.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbCompressibilityCheck.Name = "gbCompressibilityCheck"
        Me.gbCompressibilityCheck.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.gbCompressibilityCheck.Size = New System.Drawing.Size(895, 390)
        Me.gbCompressibilityCheck.TabIndex = 0
        Me.gbCompressibilityCheck.TabStop = False
        Me.gbCompressibilityCheck.Text = "Compressibility Check"
        '
        'lAimedQualityHint
        '
        Me.lAimedQualityHint.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lAimedQualityHint.AutoSize = True
        Me.lAimedQualityHint.Location = New System.Drawing.Point(553, 158)
        Me.lAimedQualityHint.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lAimedQualityHint.Name = "lAimedQualityHint"
        Me.lAimedQualityHint.Size = New System.Drawing.Size(52, 25)
        Me.lAimedQualityHint.TabIndex = 4
        Me.lAimedQualityHint.Text = "(hint)"
        '
        'cbGoTo
        '
        Me.cbGoTo.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cbGoTo.FormattingEnabled = True
        Me.cbGoTo.Location = New System.Drawing.Point(0, 4)
        Me.cbGoTo.Margin = New System.Windows.Forms.Padding(0)
        Me.cbGoTo.Name = "cbGoTo"
        Me.cbGoTo.Size = New System.Drawing.Size(172, 33)
        Me.cbGoTo.TabIndex = 3
        '
        'bnProfiles
        '
        Me.bnProfiles.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnProfiles.Location = New System.Drawing.Point(180, 2)
        Me.bnProfiles.Margin = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.bnProfiles.ShowMenuSymbol = True
        Me.bnProfiles.Size = New System.Drawing.Size(120, 37)
        Me.bnProfiles.Text = "Profiles"
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(811, 2)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.bnCancel.Size = New System.Drawing.Size(100, 37)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(703, 2)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(0)
        Me.bnOK.Size = New System.Drawing.Size(100, 37)
        Me.bnOK.Text = "OK"
        '
        'TableLayoutPanel8
        '
        Me.TableLayoutPanel8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel8.ColumnCount = 4
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel8.Controls.Add(Me.cbGoTo, 0, 0)
        Me.TableLayoutPanel8.Controls.Add(Me.bnCancel, 3, 0)
        Me.TableLayoutPanel8.Controls.Add(Me.bnProfiles, 1, 0)
        Me.TableLayoutPanel8.Controls.Add(Me.bnOK, 2, 0)
        Me.TableLayoutPanel8.Location = New System.Drawing.Point(8, 497)
        Me.TableLayoutPanel8.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TableLayoutPanel8.Name = "TableLayoutPanel8"
        Me.TableLayoutPanel8.RowCount = 1
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel8.Size = New System.Drawing.Size(911, 42)
        Me.TableLayoutPanel8.TabIndex = 4
        '
        'x264Form
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(928, 547)
        Me.Controls.Add(Me.TableLayoutPanel8)
        Me.Controls.Add(Me.tcMain)
        Me.Controls.Add(Me.rtbCommandLine)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(1, 1, 1, 1)
        Me.Name = "x264Form"
        Me.Text = "x264"
        Me.tcMain.ResumeLayout(False)
        Me.tpBasic.ResumeLayout(False)
        Me.TableLayoutPanel7.ResumeLayout(False)
        Me.TableLayoutPanel7.PerformLayout()
        Me.TableLayoutPanel6.ResumeLayout(False)
        Me.TableLayoutPanel6.PerformLayout()
        Me.tpAnalysis.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.gbQuantOptions.ResumeLayout(False)
        Me.gbQuantOptions.PerformLayout()
        Me.gbPartitions.ResumeLayout(False)
        Me.gbPartitions.PerformLayout()
        Me.gbMotionEstimation.ResumeLayout(False)
        Me.gbMotionEstimation.PerformLayout()
        Me.gbAnalysisMisc.ResumeLayout(False)
        Me.gbAnalysisMisc.PerformLayout()
        Me.tpFrameOptions.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.gbBFrames.ResumeLayout(False)
        Me.gbBFrames.PerformLayout()
        Me.gbFrameOptions.ResumeLayout(False)
        Me.gbFrameOptions.PerformLayout()
        Me.paDeblocking.ResumeLayout(False)
        Me.paDeblocking.PerformLayout()
        Me.tpRateControl.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.gbRC2.ResumeLayout(False)
        Me.gbRC2.PerformLayout()
        Me.gbRC1.ResumeLayout(False)
        Me.gbRC1.PerformLayout()
        Me.tpMisc.ResumeLayout(False)
        Me.TableLayoutPanel5.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.tpCommandLine.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.gbAddToAll.ResumeLayout(False)
        Me.gbAddToToPrecedingPasses.ResumeLayout(False)
        Me.gbRemoveFromPrecedingPasses.ResumeLayout(False)
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.tpStaxrip.ResumeLayout(False)
        Me.gbCompressibilityCheck.ResumeLayout(False)
        Me.gbCompressibilityCheck.PerformLayout()
        Me.TableLayoutPanel8.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Encoder As x264Encoder

    Private Params As x264Params
    Private IntBags As New Dictionary(Of Control, SettingBag(Of Integer))
    Private SingleBags As New Dictionary(Of Control, SettingBag(Of Single))
    Private StringBags As New Dictionary(Of Control, SettingBag(Of String))
    Private BoolBags As New Dictionary(Of Control, SettingBag(Of Boolean))
    Private IsLoading As Boolean = True
    Private LastCmdl As String
    Private SwitchControlDic As New Dictionary(Of String, Control)
    Private ControlTipDic As New Dictionary(Of Control, String)
    Private ToolTip As ToolTip
    Private ImportedSwitchesCount As Integer
    Private NameOfLastProfile As String

    Sub New(enc As x264Encoder)
        MyBase.New()
        InitializeComponent()

        Encoder = enc
        NameOfLastProfile = enc.Name

        components = New System.ComponentModel.Container

        ToolTip = New ToolTip(components)
        ToolTip.IsBalloon = True
        ToolTip.AutomaticDelay = 1000

        Populate(Of x264AQMode)(cbAQMode.Items)
        Populate(Of x264SubMEMode)(cbSubME.Items)
        Populate(Of x264TrellisMode)(cbTrellis.Items)
        Populate(Of x264NalHrdMode)(cbNalHrd.Items)
        Populate(Of x264MeMethodMode)(cbMEMethod.Items)
        Populate(Of x264DirectMode)(cbDirectMode.Items)
        Populate(Of x264LevelMode)(cbLevel.Items)
        Populate(Of x264BAdaptMode)(cbBAdapt.Items)
        Populate(Of x264ProfileMode)(cbProfile.Items)
        Populate(Of x264PresetMode)(cbPreset.Items)
        Populate(Of x264TuneMode)(cbTune.Items)
        Populate(Of x264BPyramidMode)(cbBPyramidMode.Items)
        Populate(Of x264DeviceMode)(cbDevice.Items)
        Populate(Of x264WeightpMode)(cbWeightP.Items)
        Populate(Of x264Mode)(cbMode.Items)
        Populate(Of x264OverscanMode)(cbOverscan.Items)
        Populate(Of x264VideoformatMode)(cbVideoformat.Items)
        Populate(Of x264FullrangeMode)(cbFullrange.Items)
        Populate(Of x264ColorprimMode)(cbColorprim.Items)
        Populate(Of x264TransferMode)(cbTransfer.Items)
        Populate(Of x264ColormatrixMode)(cbColormatrix.Items)

        Populate(cbDepth.Items, "8-Bit", "10-Bit")

        AddCmdlControl.Presets = s.CmdlPresetsX264
        RemoveTurboCmdlControl.Presets = s.CmdlPresetsX264
        AddTurboCmdlControl.Presets = s.CmdlPresetsX264

        Dim lastTabIndex = s.Storage.GetInt("x264 tab")
        If lastTabIndex < tcMain.TabPages.Count Then tcMain.SelectedIndex = lastTabIndex

        cbGoTo.Sorted = True
        cbGoTo.SendMessageCue("Search")

        ToolTip.SetToolTip(lAimedQuality, "A well balanced value is 50%. The assistant warns" & BR & "if it's more then 10% off.")
        ToolTip.SetToolTip(rtbCommandLine, "Right-click to show a context menu")

        rtbCommandLine.ScrollBars = RichTextBoxScrollBars.None
        AcceptButton = Nothing
    End Sub

    Sub LoadParams(params As x264Params)
        IntBags.Clear()
        SingleBags.Clear()
        StringBags.Clear()
        BoolBags.Clear()

        Me.Params = DirectCast(ObjectHelp.GetCopy(params), x264Params)
        nudPercent.Value = Encoder.AutoCompCheckValue
        LoadControls()
    End Sub

    Sub LoadControls()
        IsLoading = True

        LoadCheckBox(cb8x8DCT, Params.AdaptiveDCT)
        LoadCheckBox(cbCABAC, Params.CABAC)
        LoadCheckBox(cbb8x8, Params.PartitionB8x8)
        LoadCheckBox(cbi4x4, Params.PartitionI4x4)
        LoadCheckBox(cbI8x8, Params.PartitionI8x8)
        LoadCheckBox(cbP4x4, Params.PartitionP4x4)
        LoadCheckBox(cbP8x8, Params.PartitionP8x8)
        LoadCheckBox(cbDeblock, Params.Deblock)
        LoadCheckBox(cbMixedReferences, Params.MixedRefs)
        LoadCheckBox(cbFastPSkip, Params.FastPSkip)
        LoadCheckBox(cbPSNR, Params.PSNR)
        LoadCheckBox(cbSSIM, Params.SSIM)
        LoadCheckBox(cbProgress, Params.Progress)
        LoadCheckBox(cbWeightB, Params.WeightB)
        LoadCheckBox(cbThreadInput, Params.ThreadInput)
        LoadCheckBox(cbChromaInME, Params.ChromaMe)
        LoadCheckBox(cbDCTDecimate, Params.DctDecimate)
        LoadCheckBox(cbSlowFirstpass, Params.SlowFirstpass)
        LoadCheckBox(cbMBTree, Params.MbTree)
        LoadCheckBox(cbAud, Params.Aud)
        LoadCheckBox(cbPsy, Params.Psy)
        LoadCheckBox(cbPicStruct, Params.PicStruct)
        LoadCheckBox(cbBlurayCompat, Params.BlurayCompat)
        LoadCheckBox(cbOpenGOP, Params.OpenGopV2)

        LoadComboBox(cbAQMode, Params.AQMode)
        LoadComboBox(cbBAdapt, Params.BAdapt)
        LoadComboBox(cbBPyramidMode, Params.BPyramidMode)
        LoadComboBox(cbColormatrix, Params.Colormatrix)
        LoadComboBox(cbColorprim, Params.Colorprim)
        LoadComboBox(cbDevice, Params.Device)
        LoadComboBox(cbDirectMode, Params.DirectMode)
        LoadComboBox(cbFullrange, Params.Fullrange)
        LoadComboBox(cbLevel, Params.Level)
        LoadComboBox(cbMEMethod, Params.MEMethod)
        LoadComboBox(cbMode, Params.Mode)
        LoadComboBox(cbNalHrd, Params.NalHrdMode)
        LoadComboBox(cbOverscan, Params.Overscan)
        LoadComboBox(cbPreset, Params.Preset)
        LoadComboBox(cbProfile, Params.Profile)
        LoadComboBox(cbSubME, Params.SubME)
        LoadComboBox(cbTransfer, Params.Transfer)
        LoadComboBox(cbTrellis, Params.Trellis)
        LoadComboBox(cbTune, Params.Tune)
        LoadComboBox(cbVideoformat, Params.Videoformat)
        LoadComboBox(cbWeightP, Params.WeightP)
        LoadComboBox(cbDepth, Params.Depth)

        LoadNumericUpDown(nudBFrames, Params.BFrames)
        LoadNumericUpDown(nudBFramesBias, Params.BFramesBias)
        LoadNumericUpDown(nudDeblockAlpha, Params.DeblockAlpha)
        LoadNumericUpDown(nudDeblockBeta, Params.DeblockBeta)
        LoadNumericUpDown(nudGOPSizeMax, Params.GOPSizeMax)
        LoadNumericUpDown(nudGOPSizeMin, Params.GOPSizeMin)
        LoadNumericUpDown(nudIPRatio, Params.IPRatio)
        LoadNumericUpDown(nudPBRatio, Params.PBRatio)
        LoadNumericUpDown(nudMeRange, Params.MeRange)
        LoadNumericUpDown(nudPsyRD, Params.PsyRD)
        LoadNumericUpDown(nudPsyTrellis, Params.PsyTrellis)
        LoadNumericUpDown(nudQPComp, Params.QComp)
        LoadNumericUpDown(nudQP, Params.Quant)
        LoadNumericUpDown(nudQPCompCheck, Params.QuantCompCheck)
        LoadNumericUpDown(nudQPMin, Params.QPMin)
        LoadNumericUpDown(nudReferenceFrames, Params.RefFrames)
        LoadNumericUpDown(nudSceneCut, Params.SceneCut)
        LoadNumericUpDown(nudThreads, Params.Threads)
        LoadNumericUpDown(nudVBVBufSize, Params.VBVBufSize)
        LoadNumericUpDown(nudVBVInit, Params.VBVInit)
        LoadNumericUpDown(nudVBVMaxRate, Params.VBVMaxRate)
        LoadNumericUpDown(nudRcLookahead, Params.RcLookahead)
        LoadNumericUpDown(nudNoiseReduction, Params.NoiseReduction)
        LoadNumericUpDown(nudSlices, Params.Slices)
        LoadNumericUpDown(nudAQStrength, Params.AQStrengthV2)
        LoadNumericUpDown(nudChromaloc, Params.Chromaloc)

        LoadTextBox(AddCmdlControl.tb, Params.AddAll)
        LoadTextBox(AddTurboCmdlControl.tb, Params.TurboAdd)
        LoadTextBox(RemoveTurboCmdlControl.tb, Params.TurboRemove)

        IsLoading = False

        Init()
        UpdateControls()
    End Sub

    Sub Init()
        Init(cb8x8DCT, "--no-8x8dct", "Disable adaptive spatial transform size")
        Init(cbCABAC, "--no-cabac")
        Init(cbb8x8, "--partitions b8x8")
        Init(cbP4x4, "--partitions p4x4")
        Init(cbP8x8, "--partitions p8x8")
        Init(cbi4x4, "--partitions i4x4")
        Init(cbI8x8, "--partitions i8x8")
        Init(cbDeblock, "--no-deblock")
        Init(cbMixedReferences, "--no-mixed-refs", "Don't decide references on a per partition basis.")
        Init(cbFastPSkip, "--no-fast-pskip", "Disables early SKIP detection on P-frames.")
        Init(cbPSNR, "--psnr")
        Init(cbSSIM, "--ssim")
        Init(cbAud, "--aud")
        Init(nudSlices, lSlices, "--slices", "Number of slices per frame. Forces rectangular" + BR + "slices and is overridden by other slicing options")
        Init(cbBPyramidMode, lBPyramidMode, "--b-pyramid", "Keep some B-frames as references." + BR2 + "None: Disabled" + BR + "Strict: Strictly hierarchical pyramid" + BR + "Normal: Non-strict (not Blu-ray compatible)")
        Init(cbProgress, "--no-progress", "Don't show the progress indicator while encoding.")
        Init(cbPsy, "--no-psy", "Disable all visual optimizations that worsen both PSNR and SSIM.")
        Init(cbWeightB, "--no-weightb")
        Init(cbThreadInput, "--thread-input")
        Init(cbChromaInME, "--no-chroma-me", "Ignore chroma in motion estimation.")
        Init(cbDCTDecimate, "--no-dct-decimate", "Disables coefficient thresholding on P-frames.")
        Init(cbSlowFirstpass, "--slow-firstpass", "Don't use faster settings with --pass 1.")
        Init(cbMBTree, "--no-mbtree")
        Init(cbAQMode, lMode, "--aq-mode")
        Init(cbDirectMode, lDirectMode, "--direct")
        Init(cbMEMethod, lMEAlgorithm, "--me")
        Init(cbSubME, lSubpixelRefinement, "--subme", "Subpixel motion estimation and mode decision.")
        Init(cbTrellis, lTrellis, "--trellis", "Trellis RD quantization. Requires CABAC.")
        Init(cbLevel, lLevel, "--level")
        Init(cbBAdapt, lBAdapt, "--b-adapt", "Adaptive B-frame decision method." + BR + "Higher values may lower threading efficiency.")
        Init(cbProfile, lProfile, "--profile", "Force H.264 profile, overrides all settings.")
        Init(cbPreset, lPreset, "--preset", "Overriden by user settings.")
        Init(cbTune, lTune, "--tune", "Tune for particular source type." + BR2 + "Overridden by user settings.")
        Init(nudAQStrength, lAQStrengthHint, "--aq-strength", "Reduces blocking and blurring in flat and textured areas.")
        Init(nudBFrames, lBFrames, "--bframes", "Number of B-frames between I and P.")
        Init(nudBFramesBias, lBias, "--b-bias", "Influences how often B-frames are used.")
        Init(nudDeblockAlpha, lStrength, "--deblock")
        Init(nudDeblockBeta, lThreshold, "--deblock")
        Init(nudGOPSizeMax, lMax, "--keyint", "Maximum GOP size")
        Init(nudGOPSizeMin, lMin, "--min-keyint", "Minimum GOP size")
        Init(nudIPRatio, lIPRatio, "--ipratio")
        Init(nudMeRange, lMERange, "--merange")
        Init(nudPBRatio, lPBRatio, "--pbratio")
        Init(nudPsyRD, lPsyRD, "--psy-rd", "Strength of psychovisual optimization.")
        Init(nudPsyTrellis, lPsyTrellis, "--psy-rd")
        Init(nudQPComp, lQPComp, "--qcomp")
        Init(nudQP, lQP, "--crf, --qp")
        Init(nudQPCompCheck, lCRFValueDefining100Quality, "n/a")
        Init(nudQPMin, lQPMinimum, "--qpmin")
        Init(nudReferenceFrames, lReferenceFrames, "--ref")
        Init(nudSceneCut, lSceneCut, "--scenecut", "How aggressively to insert extra I-frames.")
        Init(nudThreads, lThreads, "--threads")
        Init(nudVBVBufSize, lBufferSize, "--vbv-bufsize")
        Init(nudVBVInit, lInitialBuffer, "--vbv-init")
        Init(nudVBVMaxRate, lMaxBitrate, "--vbv-maxrate")
        Init(nudRcLookahead, lRcLookahead, "--rc-lookahead", "Number of frames for frametype lookahead.")
        Init(nudNoiseReduction, lNoiseReduction, "--nr")
        Init(cbWeightP, lWeightP, "--weightp", "Weighted prediction for P-frames" + BR2 + "0: Disabled" + BR + "1: Blind offset" + BR + "2: Smart analysis")

        Init(cbNalHrd, lNalHrd, "--nal-hrd", "Signal HRD information (requires vbv-bufsize, cbr not allowed in .mp4)")
        Init(cbOverscan, lOverscan, "--overscan", "Specify crop overscan setting.")
        Init(cbVideoformat, lVideoformat, "--videoformat", "Videoformat suggested to playback equipment.")
        Init(cbFullrange, lFullrange, "--fullrange", "Specify full range samples setting.")
        Init(cbColorprim, lColorprim, "--colorprim", "Color primaries suggested to playback equipment.")
        Init(cbTransfer, lTransfer, "--transfer", "Transfer characteristics suggested to playback equipment.")
        Init(cbColormatrix, lColormatrix, "--colormatrix", "Colormatrix suggested to playback equipment.")

        Init(cbPicStruct, "--pic-struct", "Force pic_struct in Picture Timing SEI")
        Init(nudChromaloc, lChromaloc, "--chromaloc", "Specify chroma sample location")

        Init(cbOpenGOP, "--open-gop", "Use recovery points to close GOPs(only available with b-frames).")

        Init(cbBlurayCompat, "--bluray-compat", "Enable compatibility hacks for Blu-ray support")
    End Sub

    Function GetWarning() As String
        Dim l As New List(Of String)
        Dim x = Params

        If x.SubME.Value = 10 AndAlso Not x.Trellis.Value = 2 Then
            l.Add("subme 10 requires trellis 2, either enable trellis 2 or disable subme 10")
        End If

        If x.SubME.Value = 10 AndAlso x.AQMode.Value = 0 Then
            l.Add("subme 10 requires aq-mode > 0, either change aq-mode or disable subme 10")
        End If

        If x.Trellis.Value > 0 AndAlso Not x.CABAC.Value Then
            l.Add("trellis requires cabac, either enable cabac or disable trellis")
        End If

        If x.Profile.Value = x264ProfileMode.Baseline AndAlso x.CABAC.Value Then
            l.Add("cabac requires a higher profile than baseline, either choose a higher profile or disable cabac")
        End If

        Return l.Join(", ")
    End Function

    Sub UpdateControls()
        lPreset.Font = If(Params.Preset.Value = x264PresetMode.Medium, New Font(Font, FontStyle.Regular), New Font(Font, FontStyle.Bold))
        lTune.Font = If(Params.Tune.Value = x264TuneMode.Disabled, New Font(Font, FontStyle.Regular), New Font(Font, FontStyle.Bold))
        lDevice.Font = If(Params.Device.Value = x264DeviceMode.Disabled, New Font(Font, FontStyle.Regular), New Font(Font, FontStyle.Bold))
        lProfile.Font = If(Params.Profile.Value = x264ProfileMode.High, New Font(Font, FontStyle.Regular), New Font(Font, FontStyle.Bold))

        Dim subme = DirectCast(Params.SubME.Value, x264SubMEMode)
        Dim trellisMode = DirectCast(Params.Trellis.Value, x264TrellisMode)

        lPsyRD.Enabled = Params.Psy.Value
        nudPsyRD.Enabled = Params.Psy.Value
        lPsyTrellis.Enabled = Params.Psy.Value
        nudPsyTrellis.Enabled = Params.Psy.Value

        Dim aqmode = DirectCast(Params.AQMode.Value, x264AQMode)

        nudAQStrength.Enabled = Not aqmode = x264AQMode.Disabled
        lAQStrengthHint.Enabled = nudAQStrength.Enabled

        If cb8x8DCT.Checked Then
            cbI8x8.Enabled = True
        Else
            cbI8x8.Enabled = False
            cbI8x8.Checked = False
        End If

        If cbP8x8.Checked Then
            cbP4x4.Enabled = True
        Else
            cbP4x4.Enabled = False
            cbP4x4.Checked = False
        End If

        If nudThreads.Value = 0 Then
            cbThreadInput.Checked = True
            cbThreadInput.Enabled = False
        Else
            cbThreadInput.Enabled = True
        End If

        If Params.Mode.Value = x264Mode.SingleQuant Then
            nudQP.Enabled = True
            nudQP.DecimalPlaces = 0
            nudQP.Value = CInt(nudQP.Value)
            lQP.Enabled = True
        ElseIf Params.Mode.Value = x264Mode.SingleCRF Then
            nudQP.Enabled = True
            nudQP.DecimalPlaces = 1
            lQP.Enabled = True
        Else
            nudQP.Enabled = False
            lQP.Enabled = False
        End If

        nudDeblockAlpha.Enabled = cbDeblock.Checked
        nudDeblockBeta.Enabled = cbDeblock.Checked
        lStrength.Enabled = cbDeblock.Checked
        lThreshold.Enabled = cbDeblock.Checked

        paDeblocking.Enabled = cbDeblock.Checked

        Select Case CSng(nudAQStrength.Value)
            Case Is > 1.7F
                lAQStrengthHint.Text = "(very strong)"
            Case Is > 1.2F
                lAQStrengthHint.Text = "(strong)"
            Case Is > 0.7F
                lAQStrengthHint.Text = "(medium)"
            Case Is > 0.0F
                lAQStrengthHint.Text = "(weak)"
            Case Else
                lAQStrengthHint.Text = "(none)"
        End Select

        Select Case CInt(nudDeblockBeta.Value)
            Case Is > 3
                lDeblockThresholdHint.Text = "(very smooth)"
            Case Is > 0
                lDeblockThresholdHint.Text = "(smooth)"
            Case 0
                lDeblockThresholdHint.Text = "(balanced)"
            Case Is < -3
                lDeblockThresholdHint.Text = "(very sharp)"
            Case Is < 0
                lDeblockThresholdHint.Text = "(sharp)"
        End Select

        Select Case CInt(nudDeblockAlpha.Value)
            Case Is > 3
                lDeblockStrengthHint.Text = "(very smooth)"
            Case Is > 0
                lDeblockStrengthHint.Text = "(smooth)"
            Case 0
                lDeblockStrengthHint.Text = "(balanced)"
            Case Is < -3
                lDeblockStrengthHint.Text = "(very sharp)"
            Case Is < 0
                lDeblockStrengthHint.Text = "(sharp)"
        End Select

        nudPercent_ValueChanged()

        Dim warning = GetWarning()

        Dim cmdl = ""

        If GetWarning() <> "" Then
            rtbCommandLine.Text = warning
        Else
            Static tempEnc As x264Encoder = DirectCast(ObjectHelp.GetCopy(Encoder), x264Encoder)
            tempEnc.Params = Params

            If Params.Mode.Value = x264Mode.TwoPass Then
                cmdl = tempEnc.GetArgs(2, s.ShowPathsInCommandLine)
            ElseIf Params.Mode.Value = x264Mode.ThreePass Then
                cmdl = tempEnc.GetArgs(3, s.ShowPathsInCommandLine)
            Else
                cmdl = tempEnc.GetArgs(1, s.ShowPathsInCommandLine)
            End If

            If s.ShowPathsInCommandLine Then cmdl = """" + Package.x264.Path + """ " + cmdl
            rtbCommandLine.SetText(cmdl)
            rtbCommandLine.SelectionLength = 0
        End If

        UpdateHeight()
    End Sub

    Sub UpdateHeight()
        Dim s = TextRenderer.MeasureText(rtbCommandLine.Text, rtbCommandLine.Font,
                                         New Size(rtbCommandLine.ClientSize.Width, Integer.MaxValue),
                                         TextFormatFlags.WordBreak)
        Height += CInt(s.Height * 1.2) - rtbCommandLine.Height
        rtbCommandLine.Refresh()
    End Sub

    Function GetEnum(Of T)(o As Object) As T
        Dim ret As Object = DirectCast(o, ListBag(Of Integer)).Value
        Return CType(ret, T)
    End Function

    Sub x264Form_FormClosed() Handles Me.FormClosed
        ToolTip.Dispose()

        s.CmdlPresetsX264 = AddCmdlControl.Presets.ReplaceUnicode

        If DialogResult = DialogResult.OK Then
            Encoder.Params = Params
            Encoder.AutoCompCheckValue = CInt(nudPercent.Value)
            Encoder.Name = NameOfLastProfile
        End If

        s.Storage.SetInt("x264 tab", tcMain.SelectedIndex)
    End Sub

    Sub x264Form_HelpRequested() Handles Me.HelpRequested
        Dim f As New HelpForm
        f.Doc.WriteStart("x264 Configuration")

        f.Doc.WriteH2("Guides")

        If g.IsCulture("de") Then
            f.Doc.WriteH2("Deutsche Anleitungen")
            f.Doc.WriteList("[http://encodingwissen.de/x264 Anleitung von Brother John]",
                            "[http://www.flaskmpeg.info/board/thread.php?threadid=5571 Anleitung von selur]",
                            "[http://www.mplayerhq.hu/DOCS/HTML/de/menc-feat-x264.html Anleitung für mencoder]")
        ElseIf g.IsCulture("fr") Then
            f.Doc.WriteH2("French Guides")
            f.Doc.WriteList("[http://www.mplayerhq.hu/DOCS/HTML/fr/menc-feat-x264.html guide for mencoder]")
        ElseIf g.IsCulture("ru") Then
            f.Doc.WriteH2("Russian Guides")
            f.Doc.WriteList("[http://www.mplayerhq.hu/DOCS/HTML/ru/menc-feat-x264.html guide for mencoder]")
        End If

        f.Doc.WriteH2("English Guides")
        f.Doc.WriteList("[http://www.avidemux.org/admWiki/index.php?title=H264 guide for AviDemux]",
                        "[http://www.digital-digest.com/articles/x264_options_page1.html guide by DVDGuy]",
                        "[http://www.mplayerhq.hu/DOCS/HTML/en/menc-feat-x264.html guide for mencoder]",
                        "[http://mewiki.project357.com/wiki/X264_Settings switches at MeGUI wiki]")
        f.Show()
    End Sub

    Sub LoadComboBox(cb As ComboBox, bag As SettingBag(Of Integer))
        If Not IntBags.ContainsKey(cb) Then
            AddHandler cb.SelectedIndexChanged, AddressOf ComboBoxSelectedIndexChanged
        End If

        IntBags(cb) = bag

        For i = 0 To cb.Items.Count - 1
            If DirectCast(cb.Items(i), ListBag(Of Integer)).Value = bag.Value Then
                cb.SelectedIndex = i
                Exit For
            End If
        Next
    End Sub

    Sub Init(c As Control, switch As String)
        Init(c, Nothing, switch, Nothing)
    End Sub

    Sub Init(c As Control, switch As String, tip As String)
        Init(c, Nothing, switch, tip)
    End Sub

    Sub Init(c As Control, c2 As Control, switch As String)
        Init(c, c2, switch, Nothing)
    End Sub

    Sub Init(c As Control, c2 As Control, switch As String, tip As String)
        Dim caption As String = Nothing

        If Not c2 Is Nothing Then
            caption = c2.Text.Trim(":"c) + BR2
        Else
            caption = c.Text + BR2
        End If

        If tip Is Nothing Then
            tip = caption + switch
        Else
            tip = caption + switch + BR2 + tip
        End If

        If Not c2 Is Nothing Then
            ToolTip.SetToolTip(c2, tip)
        End If

        ToolTip.SetToolTip(c, tip)
        ControlTipDic(c) = tip

        If switch.StartsWith("--") AndAlso Not SwitchControlDic.ContainsKey(switch) Then
            SwitchControlDic(switch) = c
        End If
    End Sub

    Sub LoadCheckBox(cb As CheckBox, bag As SettingBag(Of Boolean))
        If Not BoolBags.ContainsKey(cb) Then
            AddHandler cb.CheckStateChanged, AddressOf CheckBoxCheckStateChanged
        End If

        BoolBags(cb) = bag
        cb.Checked = bag.Value
    End Sub

    Sub LoadNumericUpDown(nud As NumEdit, bag As SettingBag(Of Integer))
        If Not IntBags.ContainsKey(nud) Then
            AddHandler nud.ValueChanged, AddressOf NumericUpDownValueChanged
        End If

        IntBags(nud) = bag

        Try
            nud.Value = bag.Value
        Catch
        End Try
    End Sub

    Sub LoadNumericUpDown(nud As NumEdit, bag As SettingBag(Of Single))
        If Not SingleBags.ContainsKey(nud) Then
            AddHandler nud.ValueChanged, AddressOf NumericUpDownValueChanged
        End If

        SingleBags(nud) = bag

        Try
            nud.Value = CDec(bag.Value)
        Catch
        End Try
    End Sub

    Sub LoadTextBox(tb As TextBox, bag As SettingBag(Of String))
        If Not StringBags.ContainsKey(tb) Then
            AddHandler tb.TextChanged, AddressOf TextBoxTextChanged
        End If

        StringBags(tb) = bag
        tb.Text = bag.Value
    End Sub

    Sub NumericUpDownValueChanged(numEdit As NumEdit)
        If Not IsLoading Then
            If IntBags.ContainsKey(numEdit) Then
                IntBags(numEdit).Value = CInt(numEdit.Value)
            ElseIf SingleBags.ContainsKey(numEdit) Then
                SingleBags(numEdit).Value = CSng(numEdit.Value)
            End If

            UpdateControls()
        End If
    End Sub

    Sub TextBoxTextChanged(sender As Object, e As EventArgs)
        If Not IsLoading Then
            Dim tb = DirectCast(sender, TextBox)
            StringBags(tb).Value = tb.Text
            UpdateControls()
        End If
    End Sub

    Sub CheckBoxCheckStateChanged(sender As Object, e As EventArgs)
        If Not IsLoading Then
            Dim cb = DirectCast(sender, CheckBox)
            BoolBags(cb).Value = cb.Checked
            UpdateControls()
        End If
    End Sub

    Private Sub ComboBoxSelectedIndexChanged(sender As Object, e As EventArgs)
        If Not IsLoading Then
            Dim cb = DirectCast(sender, ComboBox)
            IntBags(cb).Value = DirectCast(cb.SelectedItem, ListBag(Of Integer)).Value
            UpdateControls()
        End If
    End Sub

    Sub Populate(Of T)(l As IList)
        For Each i As T In System.Enum.GetValues(GetType(T))
            Dim o As Object = i
            l.Add(New ListBag(Of Integer)(DispNameAttribute.GetValueForEnum(i), CInt(o)))
        Next
    End Sub

    Sub Populate(l As IList, ParamArray items As String())
        For index = 0 To items.Length - 1
            l.Add(New ListBag(Of Integer)(items(index), index))
        Next
    End Sub

    Private Sub AddCmdlControl_PresetsChanged(presets As String) Handles AddCmdlControl.PresetsChanged, AddTurboCmdlControl.PresetsChanged, RemoveTurboCmdlControl.PresetsChanged
        AddCmdlControl.Presets = presets
        AddTurboCmdlControl.Presets = presets
        RemoveTurboCmdlControl.Presets = presets
    End Sub

    Private Sub nudPercent_ValueChanged() Handles nudPercent.ValueChanged
        Dim value = CInt(nudPercent.Value)

        Dim s = ""

        Select Case value
            Case Is >= 100
                s = "best quality, extremely huge filesize"
            Case Is >= 80
                s = "extremely good quality, huge filesize"
            Case Is >= 60
                s = "very good quality, very large filesize"
            Case Is >= 50
                s = "balanced"
            Case Is >= 40
                s = "OK quality, small filesize"
            Case Is < 40
                s = "bad quality, very small filesize"
        End Select

        lAimedQualityHint.Text = "(" + s + ")"
    End Sub

    Dim SearchIndex As Integer

    Private Sub cbGoTo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbGoTo.KeyDown
        If e.KeyData = Keys.Enter Then
            SearchIndex += 1
            cbGoTo_TextChanged(Nothing, Nothing)
        Else
            SearchIndex = 0
        End If
    End Sub

    Private Sub cbGoTo_KeyUp(sender As Object, e As KeyEventArgs) Handles cbGoTo.KeyUp
        UpdateSearchComboBox()
        cbGoTo.SelectionStart = cbGoTo.Text.Length
    End Sub

    Private Sub cbGoTo_TextChanged(sender As Object, e As EventArgs) Handles cbGoTo.TextChanged
        Dim q = cbGoTo.Text.ToLower
        ToolTip.Active = False
        ToolTip.Active = True

        Dim matchedControls As New List(Of Control)

        If q.Length > 1 Then
            For Each i In SwitchControlDic
                If i.Key.ToLower.Contains(q) Then
                    matchedControls.Add(i.Value)
                End If
            Next

            For Each i In ControlTipDic
                If i.Value.ToLower.Contains(q) AndAlso
                    Not matchedControls.Contains(i.Key) Then

                    matchedControls.Add(i.Key)
                End If
            Next

            If matchedControls.Count > 0 Then
                If SearchIndex >= matchedControls.Count Then
                    SearchIndex = 0
                End If

                Dim c = matchedControls(SearchIndex)
                tcMain.SelectedTab = GetTab(Me, c)
                ToolTip.Show(ControlTipDic(c), c, 5000)
                cbGoTo.Focus()
            End If
        End If
    End Sub

    Sub UpdateSearchComboBox()
        cbGoTo.Items.Clear()

        For Each i In SwitchControlDic
            If cbGoTo.Text = "" OrElse SwitchControlDic.ContainsKey(cbGoTo.Text) OrElse
                i.Key.ToLower.Contains(cbGoTo.Text.ToLower) Then

                cbGoTo.Items.Add(i.Key)
            End If
        Next
    End Sub

    Function GetTab(walk As Control, search As Control) As TabPage
        Static last As TabPage = Nothing

        If TypeOf walk Is TabPage Then
            last = DirectCast(walk, TabPage)
        End If

        If walk Is search Then
            Return last
        End If

        For Each i As Control In walk.Controls
            Dim r = GetTab(i, search)

            If Not r Is Nothing Then
                Return r
            End If
        Next

        Return Nothing
    End Function

    Function SetParam(switch As String, command As String, bag As SettingBag(Of Integer), enumType As Type) As Boolean
        Try
            command = "--" + command

            If command.StartsWith(switch + " ") Then
                Dim s = command.Substring(switch.Length + 1)

                For Each i In System.Enum.GetValues(enumType)
                    If s.ToLower = i.ToString.ToLower Then
                        bag.Value = CInt(i)
                        ImportedSwitchesCount += 1
                        Return True
                    End If
                Next

                For Each i In System.Enum.GetValues(enumType)
                    If s.ToLower = DispNameAttribute.GetValueForEnum(i).ToLower Then
                        bag.Value = CInt(i)
                        ImportedSwitchesCount += 1
                        Return True
                    End If
                Next
            End If
        Catch
        End Try
    End Function

    Function SetParam(switch As String, command As String, ParamArray bags As SettingBag(Of Integer)()) As Boolean
        Try
            command = "--" + command

            If command.StartsWith(switch + " ") Then
                Dim a = command.Substring(switch.Length + 1).Split(",:;".ToCharArray)

                For x = 0 To bags.Length - 1
                    bags(x).Value = CInt(a(x))
                Next

                ImportedSwitchesCount += 1
                Return True
            End If
        Catch
        End Try
    End Function

    Function SetParam(switch As String, command As String, ParamArray bags As SettingBag(Of Single)()) As Boolean
        Try
            command = "--" + command

            If command.StartsWith(switch + " ") Then
                Dim a = command.Substring(switch.Length + 1).Split(",:;".ToCharArray)

                For x = 0 To bags.Length - 1
                    bags(x).Value = Convert.ToSingle(a(x), CultureInfo.InvariantCulture)
                Next

                ImportedSwitchesCount += 1
                Return True
            End If
        Catch
        End Try
    End Function

    Function SetParam(switch As String, command As String, bag As SettingBag(Of Single)) As Boolean
        Try
            command = "--" + command

            If command.StartsWith(switch + " ") Then
                bag.Value = Convert.ToSingle(command.Substring(switch.Length + 1), CultureInfo.InvariantCulture)
                ImportedSwitchesCount += 1
                Return True
            End If
        Catch
        End Try
    End Function

    Function SetParam(switch As String, command As String, bag As SettingBag(Of Boolean), Optional reverse As Boolean = False) As Boolean
        Try
            command = "--" + command

            If command = switch Then
                bag.Value = If(reverse, False, True)
                ImportedSwitchesCount += 1
                Return True
            End If
        Catch
        End Try
    End Function

    Function SetParam(switch As String, command As String, params As x264Params) As Boolean
        Try
            command = "--" + command

            If command.StartsWith(switch) AndAlso switch = "--partitions" Then
                Dim a = command.Substring(switch.Length + 1).Split(",:;".ToCharArray).ToList

                params.PartitionB8x8.Value = False
                params.PartitionI4x4.Value = False
                params.PartitionI8x8.Value = False
                params.PartitionP4x4.Value = False
                params.PartitionP8x8.Value = False

                For Each i In a
                    Select Case i
                        Case "b8x8"
                            params.PartitionB8x8.Value = True
                        Case "i4x4"
                            params.PartitionI4x4.Value = True
                        Case "i8x8"
                            params.PartitionI8x8.Value = True
                        Case "p4x4"
                            params.PartitionP4x4.Value = True
                        Case "p8x8"
                            params.PartitionP8x8.Value = True
                        Case "all"
                            params.PartitionB8x8.Value = True
                            params.PartitionI4x4.Value = True
                            params.PartitionI8x8.Value = True
                            params.PartitionP4x4.Value = True
                            params.PartitionP8x8.Value = True
                    End Select
                Next

                ImportedSwitchesCount += 1
                Return True
            End If
        Catch
        End Try
    End Function

    Private Sub cbPreset_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbPreset.SelectedIndexChanged
        UpdateBasics()
    End Sub

    Private Sub cbTune_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbTune.SelectedIndexChanged
        UpdateBasics()
    End Sub

    Private Sub cbProfile_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbProfile.SelectedIndexChanged
        UpdateBasics()
    End Sub

    Private Sub cbDevice_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbDevice.SelectedIndexChanged
        UpdateBasics()
    End Sub

    Sub UpdateBasics()
        If Not IsLoading Then
            Params.Preset.Value = ListBag(Of Integer).GetValue(cbPreset)
            Params.Tune.Value = ListBag(Of Integer).GetValue(cbTune)
            Params.Profile.Value = ListBag(Of Integer).GetValue(cbProfile)
            Params.Device.Value = ListBag(Of Integer).GetValue(cbDevice)

            Params.ApplyDeviceSettings()
            Params.ApplyDefaults(Params)
            Params.ApplyDeviceSettings()

            LoadControls()
        End If
    End Sub

    Sub SaveProfile()
        Dim name = InputBox.Show("Please enter a profile name.", "Profile Name", NameOfLastProfile)

        If name <> "" Then
            For Each i In s.VideoEncoderProfiles.OfType(Of x264Encoder)()
                If i.Name = name Then
                    i.Params = ObjectHelp.GetCopy(Of x264Params)(Params)
                    Exit Sub
                End If
            Next

            Dim enc = ObjectHelp.GetCopy(Of x264Encoder)(Encoder)
            enc.Name = name
            enc.Params = ObjectHelp.GetCopy(Of x264Params)(Params)
            enc.Clean()

            NameOfLastProfile = name
            s.VideoEncoderProfiles.Add(enc)
        End If
    End Sub

    Sub EditProfiles()
        Using f As New ProfilesForm("Encoder Profiles", s.VideoEncoderProfiles, Nothing,
                                    AddressOf g.MainForm.GetNewVideoEncoderProfile,
                                    AddressOf VideoEncoder.GetDefaults)
            f.ShowDialog()
        End Using
    End Sub

    Sub CopyCmdl()
        If rtbCommandLine.SelectionLength = 0 Then
            rtbCommandLine.Text.ToClipboard()
        Else
            rtbCommandLine.SelectedText.ToClipboard()
        End If
    End Sub

    Sub ImportCommandLine()
        Dim clip = Clipboard.GetText()
        Dim commandLine = clip
        Dim failed As New List(Of String)

        ImportedSwitchesCount = 0

        If commandLine <> "" AndAlso commandLine.Contains("--") Then
            Dim params As New x264Params
            commandLine = commandLine.Substring(commandLine.IndexOf("--"))

            While commandLine.Contains("  ")
                commandLine = commandLine.Replace("  ", " ")
            End While

            commandLine = commandLine.Trim.Trim("-"c)
            Dim a = commandLine.SplitNoEmpty(" --")

            For x = 0 To a.Length - 1
                If SetParam("--aq-mode", a(x), params.AQMode) Then
                ElseIf SetParam("--weightp", a(x), params.WeightP) Then
                ElseIf SetParam("--aq-strength", a(x), params.AQStrengthV2) Then
                ElseIf SetParam("--aud", a(x), params.Aud) Then
                ElseIf SetParam("--pic-struct", a(x), params.PicStruct) Then
                ElseIf SetParam("--b-adapt", a(x), params.BAdapt) Then
                ElseIf SetParam("--b-bias", a(x), params.BFramesBias) Then
                ElseIf SetParam("--chromaloc", a(x), params.Chromaloc) Then
                ElseIf SetParam("--b-pyramid", a(x), params.BPyramidMode, GetType(x264BPyramidMode)) Then
                ElseIf SetParam("--bframes", a(x), params.BFrames) Then
                ElseIf SetParam("--bluray-compat", a(x), params.BlurayCompat) Then
                ElseIf SetParam("--crf", a(x), params.Quant) Then : params.Mode.Value = x264Mode.SingleCRF
                ElseIf SetParam("--deblock", a(x), params.DeblockAlpha, params.DeblockBeta) Then
                ElseIf SetParam("--direct", a(x), params.DirectMode, GetType(x264DirectMode)) Then
                ElseIf SetParam("--ipratio", a(x), params.IPRatio) Then
                ElseIf SetParam("--keyint", a(x), params.GOPSizeMax) Then
                ElseIf SetParam("--level", a(x), params.Level, GetType(x264LevelMode)) Then
                ElseIf SetParam("--me", a(x), params.MEMethod, GetType(x264MeMethodMode)) Then
                ElseIf SetParam("--nal-hrd", a(x), params.NalHrdMode, GetType(x264NalHrdMode)) Then
                ElseIf SetParam("--overscan", a(x), params.Overscan, GetType(x264OverscanMode)) Then
                ElseIf SetParam("--videoformat", a(x), params.Videoformat, GetType(x264VideoformatMode)) Then
                ElseIf SetParam("--fullrange", a(x), params.Fullrange, GetType(x264FullrangeMode)) Then
                ElseIf SetParam("--colorprim", a(x), params.Colorprim, GetType(x264ColorprimMode)) Then
                ElseIf SetParam("--open-gop", a(x), params.OpenGopV2) Then
                ElseIf SetParam("--transfer", a(x), params.Transfer, GetType(x264TransferMode)) Then
                ElseIf SetParam("--Colormatrix", a(x), params.Colormatrix, GetType(x264ColormatrixMode)) Then
                ElseIf SetParam("--merange", a(x), params.MeRange) Then
                ElseIf SetParam("--min-keyint", a(x), params.GOPSizeMin) Then
                ElseIf SetParam("--no-deblock", a(x), params.Deblock, True) Then
                ElseIf SetParam("--no-8x8dct", a(x), params.AdaptiveDCT, True) Then
                ElseIf SetParam("--no-cabac", a(x), params.CABAC, True) Then
                ElseIf SetParam("--no-chroma-me", a(x), params.ChromaMe, True) Then
                ElseIf SetParam("--no-dct-decimate", a(x), params.DctDecimate, True) Then
                ElseIf SetParam("--no-fast-pskip", a(x), params.FastPSkip, True) Then
                ElseIf SetParam("--no-mbtree", a(x), params.MbTree, True) Then
                ElseIf SetParam("--no-mixed-refs", a(x), params.MixedRefs, True) Then
                ElseIf SetParam("--no-progress", a(x), params.Progress, True) Then
                ElseIf SetParam("--no-psy", a(x), params.Psy, True) Then
                ElseIf SetParam("--no-weightb", a(x), params.WeightB, True) Then
                ElseIf SetParam("--nr", a(x), params.NoiseReduction) Then
                ElseIf SetParam("--partitions", a(x), params) Then
                ElseIf SetParam("--pbratio", a(x), params.PBRatio) Then
                ElseIf SetParam("--preset", a(x), params.Preset, GetType(x264PresetMode)) Then : params.ApplyDefaults(params)
                ElseIf SetParam("--profile", a(x), params.Profile, GetType(x264ProfileMode)) Then : params.ApplyDefaults(params)
                ElseIf SetParam("--psnr", a(x), params.PSNR) Then
                ElseIf SetParam("--psy-rd", a(x), params.PsyRD, params.PsyTrellis) Then
                ElseIf SetParam("--qcomp", a(x), params.QComp) Then
                ElseIf SetParam("--qp", a(x), params.Quant) Then : params.Mode.Value = x264Mode.SingleQuant
                ElseIf SetParam("--qpmin", a(x), params.QPMin) Then
                ElseIf SetParam("--rc-lookahead", a(x), params.RcLookahead) Then
                ElseIf SetParam("--ref", a(x), params.RefFrames) Then
                ElseIf SetParam("--scenecut", a(x), params.SceneCut) Then
                ElseIf SetParam("--slices", a(x), params.Slices) Then
                ElseIf SetParam("--slow-firstpass", a(x), params.SlowFirstpass) Then
                ElseIf SetParam("--ssim", a(x), params.SSIM) Then
                ElseIf SetParam("--subme", a(x), params.SubME) Then
                ElseIf SetParam("--thread-input", a(x), params.ThreadInput) Then
                ElseIf SetParam("--threads", a(x), params.Threads) Then
                ElseIf SetParam("--trellis", a(x), params.Trellis) Then
                ElseIf SetParam("--tune", a(x), params.Tune, GetType(x264TuneMode)) Then : params.ApplyDefaults(params)
                ElseIf SetParam("--vbv-bufsize", a(x), params.VBVBufSize) Then
                ElseIf SetParam("--vbv-init", a(x), params.VBVInit) Then
                ElseIf SetParam("--vbv-maxrate", a(x), params.VBVMaxRate) Then
                ElseIf a(x) = "pass 1" OrElse a(x) = "pass 2" Then
                    params.Mode.Value = x264Mode.TwoPass
                    ImportedSwitchesCount += 1
                ElseIf a(x) = "pass 3" Then
                    params.Mode.Value = x264Mode.ThreePass
                    ImportedSwitchesCount += 1
                ElseIf a(x) Like "bitrate #*" Then
                    If Not clip.Contains("--pass ") Then
                        params.Mode.Value = x264Mode.SingleBitrate
                    End If

                    ImportedSwitchesCount += 1
                ElseIf _
                    Not a(x) Like "output *" AndAlso
                    Not a(x) Like "sar *" AndAlso
                    Not a(x) Like "stats *" Then

                    failed.Add("--" + a(x))
                End If
            Next

            LoadParams(params)
        End If

        Dim m = "Count of imported switches: " & ImportedSwitchesCount

        If failed.Count = 0 Then
            m += BR2 + "No errors"
        Else
            m += BR2 + "Count of switches failed to import: " & failed.Count &
                BR2 & "Failed Switches:" & BR2 & failed.Join(BR)
        End If

        Using td As New TaskDialog(Of String)
            If failed.Count = 0 Then
                td.MainInstruction = m
                td.MainIcon = TaskDialogIcon.Info
            Else
                td.Content = m
                td.MainIcon = TaskDialogIcon.Warning
            End If

            td.Footer = Strings.TaskDialogFooter
            If td.Show() = "copy" Then m.ToClipboard()
        End Using
    End Sub

    Sub ToogleShowPaths()
        s.ShowPathsInCommandLine = Not s.ShowPathsInCommandLine
        UpdateControls()
    End Sub

    Dim ProfilesMenu As ContextMenuStripEx

    Private Sub bProfiles_Click() Handles bnProfiles.Click
        If ProfilesMenu Is Nothing Then
            ProfilesMenu = New ContextMenuStripEx(components)
        End If

        ProfilesMenu.Items.Clear()

        Dim profiles = New MenuItemEx("Load")
        ProfilesMenu.Items.Add(profiles)

        For Each i In s.VideoEncoderProfiles.OfType(Of x264Encoder)()
            ActionMenuItem.Add(profiles.DropDownItems, i.Name, AddressOf LoadProfile, i, Nothing)
        Next

        ProfilesMenu.Items.Add(New ActionMenuItem("Save...", Sub() SaveProfile(), "Saves the current x264 settings to a encoder profile"))
        ProfilesMenu.Items.Add(New ActionMenuItem("Edit...", Sub() EditProfiles(), "Shows the profiles dialog to edit the encoder profiles"))

        ProfilesMenu.Show(bnProfiles, New Point(0, bnProfiles.Height))
    End Sub

    Sub LoadProfile(value As x264Encoder)
        LoadParams(ObjectHelp.GetCopy(Of x264Params)(value.Params))
        NameOfLastProfile = value.Name
    End Sub

    Function GetCommandLineMenu() As ContextMenuStripEx
        Dim ret As New ContextMenuStripEx(components)
        ret.Items.Add(New ActionMenuItem("Copy", Sub() CopyCmdl()))
        ret.Items.Add(New ActionMenuItem("Import", Sub() ImportCommandLine(), "Imports x264 settings from a x264 command line in the clipboard") With {.Enabled = Clipboard.GetText.Contains("--")})
        ret.Items.Add(New ActionMenuItem("Show Paths", Sub() ToogleShowPaths()) With {.Checked = s.ShowPathsInCommandLine, .CheckOnClick = True})
        Return ret
    End Function

    Private Sub rtbCmdl_MouseDown(sender As Object, e As MouseEventArgs) Handles rtbCommandLine.MouseDown
        If Not rtbCommandLine.ContextMenuStrip Is Nothing Then
            rtbCommandLine.ContextMenuStrip.Dispose()
        End If

        rtbCommandLine.ContextMenuStrip = GetCommandLineMenu()
    End Sub

    Private Sub x264Form_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadParams(Encoder.Params)
        UpdateHeight()
        UpdateSearchComboBox()
    End Sub

    Private Sub buImport_Click() Handles buImport.Click
        ImportCommandLine()
    End Sub

    Private Sub x264Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        cbGoTo.Focus()
    End Sub
End Class