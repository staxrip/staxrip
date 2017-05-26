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
    Friend WithEvents gbFrameOptionsRight As System.Windows.Forms.GroupBox
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
    Friend WithEvents tpOther As System.Windows.Forms.TabPage
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
    Friend WithEvents bnMenu As ButtonEx
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
    Friend WithEvents tlpAnalysis As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tlpFrameOptions As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents LineControl1 As StaxRip.UI.LineControl
    Friend WithEvents tlpRateControl As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents LineControl2 As StaxRip.UI.LineControl
    Friend WithEvents tlpCommandLine As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tpMisc As System.Windows.Forms.TabPage
    Friend WithEvents tlpMisc As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents gbIO As System.Windows.Forms.GroupBox
    Friend WithEvents lNalHrd As System.Windows.Forms.Label
    Friend WithEvents cbNalHrd As System.Windows.Forms.ComboBox
    Friend WithEvents cbAud As System.Windows.Forms.CheckBox
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
    Friend WithEvents tlpBasicControls As TableLayoutPanel
    Friend WithEvents tlpBasicHost As TableLayoutPanel
    Friend WithEvents tlpBottom As TableLayoutPanel
    Friend WithEvents tlpPartitions As TableLayoutPanel
    Friend WithEvents tlpBframeControls As TableLayoutPanel
    Friend WithEvents tlpDeblockingControls As TableLayoutPanel
    Friend WithEvents tlpDeblockingHost As TableLayoutPanel
    Friend WithEvents tlpFrameOptionsRightTopControls As TableLayoutPanel
    Friend WithEvents tlpQuantOptionsControls As TableLayoutPanel
    Friend WithEvents tlpFrameOptionRightHost As TableLayoutPanel
    Friend WithEvents tlpMotionEstimationControls As TableLayoutPanel
    Friend WithEvents tlpPsy As TableLayoutPanel
    Friend WithEvents tlpAnalysisMiscControls As TableLayoutPanel
    Friend WithEvents tlpRateControlLeftControls As TableLayoutPanel
    Friend WithEvents tlpRateControlRightControls As TableLayoutPanel
    Friend WithEvents tlpMiscIO As TableLayoutPanel
    Friend WithEvents tlpVUIControls As TableLayoutPanel
    Friend WithEvents tlpOther As TableLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents tlpRtbCommandLineWrapper As TableLayoutPanel
    Friend WithEvents rtbCommandLine As CommandLineRichTextBox
    Friend WithEvents tlpMe As TableLayoutPanel
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
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tcMain = New System.Windows.Forms.TabControl()
        Me.tpBasic = New System.Windows.Forms.TabPage()
        Me.tlpBasicHost = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpBasicControls = New System.Windows.Forms.TableLayoutPanel()
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
        Me.tlpAnalysis = New System.Windows.Forms.TableLayoutPanel()
        Me.gbQuantOptions = New System.Windows.Forms.GroupBox()
        Me.tlpQuantOptionsControls = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpPsy = New System.Windows.Forms.TableLayoutPanel()
        Me.nudPsyTrellis = New StaxRip.UI.NumEdit()
        Me.lPsyTrellis = New System.Windows.Forms.Label()
        Me.nudPsyRD = New StaxRip.UI.NumEdit()
        Me.lPsyRD = New System.Windows.Forms.Label()
        Me.cbPsy = New System.Windows.Forms.CheckBox()
        Me.gbPartitions = New System.Windows.Forms.GroupBox()
        Me.tlpPartitions = New System.Windows.Forms.TableLayoutPanel()
        Me.cb8x8DCT = New System.Windows.Forms.CheckBox()
        Me.cbI8x8 = New System.Windows.Forms.CheckBox()
        Me.cbb8x8 = New System.Windows.Forms.CheckBox()
        Me.cbP8x8 = New System.Windows.Forms.CheckBox()
        Me.cbi4x4 = New System.Windows.Forms.CheckBox()
        Me.cbP4x4 = New System.Windows.Forms.CheckBox()
        Me.gbMotionEstimation = New System.Windows.Forms.GroupBox()
        Me.tlpMotionEstimationControls = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpMe = New System.Windows.Forms.TableLayoutPanel()
        Me.gbAnalysisMisc = New System.Windows.Forms.GroupBox()
        Me.tlpAnalysisMiscControls = New System.Windows.Forms.TableLayoutPanel()
        Me.cbWeightP = New System.Windows.Forms.ComboBox()
        Me.lWeightP = New System.Windows.Forms.Label()
        Me.lNoiseReduction = New System.Windows.Forms.Label()
        Me.nudNoiseReduction = New StaxRip.UI.NumEdit()
        Me.tpFrameOptions = New System.Windows.Forms.TabPage()
        Me.tlpFrameOptions = New System.Windows.Forms.TableLayoutPanel()
        Me.gbBFrames = New System.Windows.Forms.GroupBox()
        Me.tlpBframeControls = New System.Windows.Forms.TableLayoutPanel()
        Me.cbBAdapt = New System.Windows.Forms.ComboBox()
        Me.lBFrames = New System.Windows.Forms.Label()
        Me.lBPyramidMode = New System.Windows.Forms.Label()
        Me.nudBFrames = New StaxRip.UI.NumEdit()
        Me.lBAdapt = New System.Windows.Forms.Label()
        Me.cbBPyramidMode = New System.Windows.Forms.ComboBox()
        Me.gbFrameOptionsRight = New System.Windows.Forms.GroupBox()
        Me.tlpFrameOptionRightHost = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpFrameOptionsRightTopControls = New System.Windows.Forms.TableLayoutPanel()
        Me.nudReferenceFrames = New StaxRip.UI.NumEdit()
        Me.nudSceneCut = New StaxRip.UI.NumEdit()
        Me.cbOpenGOP = New System.Windows.Forms.CheckBox()
        Me.nudSlices = New StaxRip.UI.NumEdit()
        Me.lReferenceFrames = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lSlices = New System.Windows.Forms.Label()
        Me.lSceneCut = New System.Windows.Forms.Label()
        Me.tlpDeblockingHost = New System.Windows.Forms.TableLayoutPanel()
        Me.cbDeblock = New System.Windows.Forms.CheckBox()
        Me.tlpDeblockingControls = New System.Windows.Forms.TableLayoutPanel()
        Me.lDeblockThresholdHint = New System.Windows.Forms.Label()
        Me.nudDeblockBeta = New StaxRip.UI.NumEdit()
        Me.lStrength = New System.Windows.Forms.Label()
        Me.nudDeblockAlpha = New StaxRip.UI.NumEdit()
        Me.lThreshold = New System.Windows.Forms.Label()
        Me.lDeblockStrengthHint = New System.Windows.Forms.Label()
        Me.LineControl1 = New StaxRip.UI.LineControl()
        Me.tpRateControl = New System.Windows.Forms.TabPage()
        Me.tlpRateControl = New System.Windows.Forms.TableLayoutPanel()
        Me.gbRC2 = New System.Windows.Forms.GroupBox()
        Me.tlpRateControlRightControls = New System.Windows.Forms.TableLayoutPanel()
        Me.cbMBTree = New System.Windows.Forms.CheckBox()
        Me.nudRcLookahead = New StaxRip.UI.NumEdit()
        Me.lRcLookahead = New StaxRip.UI.LabelEx()
        Me.gbRC1 = New System.Windows.Forms.GroupBox()
        Me.tlpRateControlLeftControls = New System.Windows.Forms.TableLayoutPanel()
        Me.LineControl2 = New StaxRip.UI.LineControl()
        Me.nudAQStrength = New StaxRip.UI.NumEdit()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tpMisc = New System.Windows.Forms.TabPage()
        Me.tlpMisc = New System.Windows.Forms.TableLayoutPanel()
        Me.gbIO = New System.Windows.Forms.GroupBox()
        Me.tlpMiscIO = New System.Windows.Forms.TableLayoutPanel()
        Me.cbProgress = New System.Windows.Forms.CheckBox()
        Me.cbLevel = New System.Windows.Forms.ComboBox()
        Me.lLevel = New System.Windows.Forms.Label()
        Me.cbBlurayCompat = New System.Windows.Forms.CheckBox()
        Me.cbThreadInput = New System.Windows.Forms.CheckBox()
        Me.cbAud = New System.Windows.Forms.CheckBox()
        Me.nudThreads = New StaxRip.UI.NumEdit()
        Me.lThreads = New System.Windows.Forms.Label()
        Me.cbSSIM = New System.Windows.Forms.CheckBox()
        Me.cbPSNR = New System.Windows.Forms.CheckBox()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.tlpVUIControls = New System.Windows.Forms.TableLayoutPanel()
        Me.lNalHrd = New System.Windows.Forms.Label()
        Me.cbPicStruct = New StaxRip.UI.CheckBoxEx()
        Me.nudChromaloc = New StaxRip.UI.NumEdit()
        Me.lChromaloc = New System.Windows.Forms.Label()
        Me.cbNalHrd = New System.Windows.Forms.ComboBox()
        Me.lColorprim = New System.Windows.Forms.Label()
        Me.cbColorprim = New System.Windows.Forms.ComboBox()
        Me.cbFullrange = New System.Windows.Forms.ComboBox()
        Me.lFullrange = New System.Windows.Forms.Label()
        Me.cbTransfer = New System.Windows.Forms.ComboBox()
        Me.lTransfer = New System.Windows.Forms.Label()
        Me.cbOverscan = New System.Windows.Forms.ComboBox()
        Me.lOverscan = New System.Windows.Forms.Label()
        Me.cbVideoformat = New System.Windows.Forms.ComboBox()
        Me.lVideoformat = New System.Windows.Forms.Label()
        Me.cbColormatrix = New System.Windows.Forms.ComboBox()
        Me.lColormatrix = New System.Windows.Forms.Label()
        Me.tpCommandLine = New System.Windows.Forms.TabPage()
        Me.tlpCommandLine = New System.Windows.Forms.TableLayoutPanel()
        Me.gbAddToAll = New System.Windows.Forms.GroupBox()
        Me.AddCmdlControl = New StaxRip.CommandLineControl()
        Me.gbAddToToPrecedingPasses = New System.Windows.Forms.GroupBox()
        Me.AddTurboCmdlControl = New StaxRip.CommandLineControl()
        Me.gbRemoveFromPrecedingPasses = New System.Windows.Forms.GroupBox()
        Me.RemoveTurboCmdlControl = New StaxRip.CommandLineControl()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.buImport = New StaxRip.UI.ButtonEx()
        Me.tpOther = New System.Windows.Forms.TabPage()
        Me.gbCompressibilityCheck = New System.Windows.Forms.GroupBox()
        Me.tlpOther = New System.Windows.Forms.TableLayoutPanel()
        Me.lAimedQualityHint = New System.Windows.Forms.Label()
        Me.cbGoTo = New System.Windows.Forms.ComboBox()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tlpBottom = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpRtbCommandLineWrapper = New System.Windows.Forms.TableLayoutPanel()
        Me.rtbCommandLine = New StaxRip.UI.CommandLineRichTextBox()
        Me.tcMain.SuspendLayout()
        Me.tpBasic.SuspendLayout()
        Me.tlpBasicHost.SuspendLayout()
        Me.tlpBasicControls.SuspendLayout()
        Me.tpAnalysis.SuspendLayout()
        Me.tlpAnalysis.SuspendLayout()
        Me.gbQuantOptions.SuspendLayout()
        Me.tlpQuantOptionsControls.SuspendLayout()
        Me.tlpPsy.SuspendLayout()
        Me.gbPartitions.SuspendLayout()
        Me.tlpPartitions.SuspendLayout()
        Me.gbMotionEstimation.SuspendLayout()
        Me.tlpMotionEstimationControls.SuspendLayout()
        Me.tlpMe.SuspendLayout()
        Me.gbAnalysisMisc.SuspendLayout()
        Me.tlpAnalysisMiscControls.SuspendLayout()
        Me.tpFrameOptions.SuspendLayout()
        Me.tlpFrameOptions.SuspendLayout()
        Me.gbBFrames.SuspendLayout()
        Me.tlpBframeControls.SuspendLayout()
        Me.gbFrameOptionsRight.SuspendLayout()
        Me.tlpFrameOptionRightHost.SuspendLayout()
        Me.tlpFrameOptionsRightTopControls.SuspendLayout()
        Me.tlpDeblockingHost.SuspendLayout()
        Me.tlpDeblockingControls.SuspendLayout()
        Me.tpRateControl.SuspendLayout()
        Me.tlpRateControl.SuspendLayout()
        Me.gbRC2.SuspendLayout()
        Me.tlpRateControlRightControls.SuspendLayout()
        Me.gbRC1.SuspendLayout()
        Me.tlpRateControlLeftControls.SuspendLayout()
        Me.tpMisc.SuspendLayout()
        Me.tlpMisc.SuspendLayout()
        Me.gbIO.SuspendLayout()
        Me.tlpMiscIO.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.tlpVUIControls.SuspendLayout()
        Me.tpCommandLine.SuspendLayout()
        Me.tlpCommandLine.SuspendLayout()
        Me.gbAddToAll.SuspendLayout()
        Me.gbAddToToPrecedingPasses.SuspendLayout()
        Me.gbRemoveFromPrecedingPasses.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.tpOther.SuspendLayout()
        Me.gbCompressibilityCheck.SuspendLayout()
        Me.tlpOther.SuspendLayout()
        Me.tlpBottom.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.tlpRtbCommandLineWrapper.SuspendLayout()
        Me.SuspendLayout()
        '
        'nudPBRatio
        '
        Me.nudPBRatio.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudPBRatio.DecimalPlaces = 1
        Me.nudPBRatio.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudPBRatio.Location = New System.Drawing.Point(363, 245)
        Me.nudPBRatio.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudPBRatio.Name = "nudPBRatio"
        Me.nudPBRatio.Size = New System.Drawing.Size(200, 70)
        Me.nudPBRatio.TabIndex = 8
        '
        'lPBRatio
        '
        Me.lPBRatio.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lPBRatio.Location = New System.Drawing.Point(13, 245)
        Me.lPBRatio.Size = New System.Drawing.Size(344, 70)
        Me.lPBRatio.Text = "PB Ratio:"
        Me.lPBRatio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudQPComp
        '
        Me.nudQPComp.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudQPComp.DecimalPlaces = 2
        Me.nudQPComp.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudQPComp.Location = New System.Drawing.Point(363, 85)
        Me.nudQPComp.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudQPComp.Name = "nudQPComp"
        Me.nudQPComp.Size = New System.Drawing.Size(200, 70)
        Me.nudQPComp.TabIndex = 5
        '
        'lQPComp
        '
        Me.lQPComp.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lQPComp.Location = New System.Drawing.Point(13, 85)
        Me.lQPComp.Size = New System.Drawing.Size(344, 70)
        Me.lQPComp.Text = "QP Compression:"
        Me.lQPComp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudQPMin
        '
        Me.nudQPMin.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudQPMin.Location = New System.Drawing.Point(363, 5)
        Me.nudQPMin.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudQPMin.Name = "nudQPMin"
        Me.nudQPMin.Size = New System.Drawing.Size(200, 70)
        Me.nudQPMin.TabIndex = 4
        '
        'lQPMinimum
        '
        Me.lQPMinimum.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lQPMinimum.Location = New System.Drawing.Point(13, 5)
        Me.lQPMinimum.Size = New System.Drawing.Size(344, 70)
        Me.lQPMinimum.Text = "QP Minimum:"
        Me.lQPMinimum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudIPRatio
        '
        Me.nudIPRatio.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudIPRatio.DecimalPlaces = 1
        Me.nudIPRatio.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudIPRatio.Location = New System.Drawing.Point(363, 165)
        Me.nudIPRatio.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudIPRatio.Name = "nudIPRatio"
        Me.nudIPRatio.Size = New System.Drawing.Size(200, 70)
        Me.nudIPRatio.TabIndex = 7
        '
        'lIPRatio
        '
        Me.lIPRatio.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lIPRatio.Location = New System.Drawing.Point(13, 165)
        Me.lIPRatio.Size = New System.Drawing.Size(344, 70)
        Me.lIPRatio.Text = "IP Ratio:"
        Me.lIPRatio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudVBVInit
        '
        Me.nudVBVInit.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudVBVInit.DecimalPlaces = 1
        Me.nudVBVInit.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudVBVInit.Location = New System.Drawing.Point(283, 485)
        Me.nudVBVInit.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudVBVInit.Name = "nudVBVInit"
        Me.nudVBVInit.Size = New System.Drawing.Size(194, 70)
        Me.nudVBVInit.TabIndex = 11
        '
        'lInitialBuffer
        '
        Me.lInitialBuffer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lInitialBuffer.Location = New System.Drawing.Point(13, 485)
        Me.lInitialBuffer.Size = New System.Drawing.Size(264, 70)
        Me.lInitialBuffer.Text = "Initial Buffer:"
        Me.lInitialBuffer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudVBVMaxRate
        '
        Me.nudVBVMaxRate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudVBVMaxRate.Location = New System.Drawing.Point(283, 405)
        Me.nudVBVMaxRate.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudVBVMaxRate.Name = "nudVBVMaxRate"
        Me.nudVBVMaxRate.Size = New System.Drawing.Size(194, 70)
        Me.nudVBVMaxRate.TabIndex = 9
        '
        'lMaxBitrate
        '
        Me.lMaxBitrate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lMaxBitrate.Location = New System.Drawing.Point(13, 405)
        Me.lMaxBitrate.Size = New System.Drawing.Size(264, 70)
        Me.lMaxBitrate.Text = "Max Bitrate:"
        Me.lMaxBitrate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudVBVBufSize
        '
        Me.nudVBVBufSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudVBVBufSize.Location = New System.Drawing.Point(283, 325)
        Me.nudVBVBufSize.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudVBVBufSize.Name = "nudVBVBufSize"
        Me.nudVBVBufSize.Size = New System.Drawing.Size(194, 70)
        Me.nudVBVBufSize.TabIndex = 6
        '
        'lBufferSize
        '
        Me.lBufferSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lBufferSize.Location = New System.Drawing.Point(13, 325)
        Me.lBufferSize.Size = New System.Drawing.Size(264, 70)
        Me.lBufferSize.Text = "Buffer Size:"
        Me.lBufferSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbAQMode
        '
        Me.cbAQMode.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpRateControlLeftControls.SetColumnSpan(Me.cbAQMode, 3)
        Me.cbAQMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAQMode.FormattingEnabled = True
        Me.cbAQMode.Location = New System.Drawing.Point(20, 92)
        Me.cbAQMode.Margin = New System.Windows.Forms.Padding(10, 3, 10, 3)
        Me.cbAQMode.Name = "cbAQMode"
        Me.cbAQMode.Size = New System.Drawing.Size(825, 56)
        Me.cbAQMode.TabIndex = 2
        '
        'lAQStrengthHint
        '
        Me.lAQStrengthHint.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lAQStrengthHint.Location = New System.Drawing.Point(483, 165)
        Me.lAQStrengthHint.Name = "lAQStrengthHint"
        Me.lAQStrengthHint.Size = New System.Drawing.Size(369, 70)
        Me.lAQStrengthHint.TabIndex = 8
        Me.lAQStrengthHint.Text = "hint"
        Me.lAQStrengthHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lMode
        '
        Me.lMode.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lMode.Location = New System.Drawing.Point(13, 5)
        Me.lMode.Name = "lMode"
        Me.lMode.Size = New System.Drawing.Size(264, 70)
        Me.lMode.TabIndex = 0
        Me.lMode.Text = "AQ Mode:"
        Me.lMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lMax
        '
        Me.lMax.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lMax.Location = New System.Drawing.Point(513, 245)
        Me.lMax.Name = "lMax"
        Me.lMax.Size = New System.Drawing.Size(104, 70)
        Me.lMax.TabIndex = 12
        Me.lMax.Text = "Max:"
        Me.lMax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lMin
        '
        Me.lMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lMin.Location = New System.Drawing.Point(203, 245)
        Me.lMin.Name = "lMin"
        Me.lMin.Size = New System.Drawing.Size(104, 70)
        Me.lMin.TabIndex = 7
        Me.lMin.Text = "Min:"
        Me.lMin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudGOPSizeMax
        '
        Me.nudGOPSizeMax.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudGOPSizeMax.Increment = New Decimal(New Integer() {50, 0, 0, 0})
        Me.nudGOPSizeMax.Location = New System.Drawing.Point(623, 245)
        Me.nudGOPSizeMax.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudGOPSizeMax.Name = "nudGOPSizeMax"
        Me.nudGOPSizeMax.Size = New System.Drawing.Size(200, 70)
        Me.nudGOPSizeMax.TabIndex = 14
        '
        'nudGOPSizeMin
        '
        Me.nudGOPSizeMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudGOPSizeMin.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudGOPSizeMin.Location = New System.Drawing.Point(313, 245)
        Me.nudGOPSizeMin.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.nudGOPSizeMin.Name = "nudGOPSizeMin"
        Me.nudGOPSizeMin.Size = New System.Drawing.Size(194, 70)
        Me.nudGOPSizeMin.TabIndex = 9
        '
        'cbCABAC
        '
        Me.cbCABAC.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tlpFrameOptionsRightTopControls.SetColumnSpan(Me.cbCABAC, 2)
        Me.cbCABAC.Location = New System.Drawing.Point(525, 85)
        Me.cbCABAC.Margin = New System.Windows.Forms.Padding(15, 3, 3, 3)
        Me.cbCABAC.Name = "cbCABAC"
        Me.cbCABAC.Size = New System.Drawing.Size(260, 70)
        Me.cbCABAC.TabIndex = 13
        Me.cbCABAC.Text = "CABAC"
        '
        'lMode2
        '
        Me.lMode2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lMode2.Location = New System.Drawing.Point(3, 485)
        Me.lMode2.Name = "lMode2"
        Me.lMode2.Size = New System.Drawing.Size(174, 70)
        Me.lMode2.TabIndex = 7
        Me.lMode2.Text = "Mode:"
        Me.lMode2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudQP
        '
        Me.nudQP.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudQP.DecimalPlaces = 1
        Me.nudQP.Enabled = False
        Me.nudQP.Location = New System.Drawing.Point(183, 5)
        Me.nudQP.Maximum = New Decimal(New Integer() {51, 0, 0, 0})
        Me.nudQP.Name = "nudQP"
        Me.nudQP.Size = New System.Drawing.Size(278, 70)
        Me.nudQP.TabIndex = 5
        '
        'lQP
        '
        Me.lQP.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lQP.Location = New System.Drawing.Point(3, 5)
        Me.lQP.Name = "lQP"
        Me.lQP.Size = New System.Drawing.Size(174, 70)
        Me.lQP.TabIndex = 0
        Me.lQP.Text = "Quality: "
        Me.lQP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbDCTDecimate
        '
        Me.cbDCTDecimate.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbDCTDecimate.Location = New System.Drawing.Point(18, 85)
        Me.cbDCTDecimate.Size = New System.Drawing.Size(394, 70)
        Me.cbDCTDecimate.Text = "DCT Decimate"
        '
        'nudMeRange
        '
        Me.nudMeRange.Increment = New Decimal(New Integer() {8, 0, 0, 0})
        Me.nudMeRange.Location = New System.Drawing.Point(236, 0)
        Me.nudMeRange.Margin = New System.Windows.Forms.Padding(0)
        Me.nudMeRange.Maximum = New Decimal(New Integer() {64, 0, 0, 0})
        Me.nudMeRange.Name = "nudMeRange"
        Me.nudMeRange.Size = New System.Drawing.Size(200, 70)
        Me.nudMeRange.TabIndex = 5
        '
        'lMERange
        '
        Me.lMERange.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lMERange.Location = New System.Drawing.Point(3, 0)
        Me.lMERange.Name = "lMERange"
        Me.lMERange.Size = New System.Drawing.Size(230, 70)
        Me.lMERange.TabIndex = 3
        Me.lMERange.Text = "M.E. Range:"
        Me.lMERange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbChromaInME
        '
        Me.cbChromaInME.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbChromaInME.Location = New System.Drawing.Point(451, 0)
        Me.cbChromaInME.Margin = New System.Windows.Forms.Padding(15, 0, 0, 0)
        Me.cbChromaInME.Name = "cbChromaInME"
        Me.cbChromaInME.Size = New System.Drawing.Size(384, 70)
        Me.cbChromaInME.TabIndex = 6
        Me.cbChromaInME.Text = "Chroma in M.E."
        Me.cbChromaInME.UseVisualStyleBackColor = True
        '
        'cbFastPSkip
        '
        Me.cbFastPSkip.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpQuantOptionsControls.SetColumnSpan(Me.cbFastPSkip, 2)
        Me.cbFastPSkip.Location = New System.Drawing.Point(418, 85)
        Me.cbFastPSkip.Name = "cbFastPSkip"
        Me.cbFastPSkip.Size = New System.Drawing.Size(429, 70)
        Me.cbFastPSkip.TabIndex = 2
        Me.cbFastPSkip.Text = "Fast P Skip"
        Me.cbFastPSkip.UseVisualStyleBackColor = True
        '
        'lMEAlgorithm
        '
        Me.lMEAlgorithm.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lMEAlgorithm.Location = New System.Drawing.Point(18, 165)
        Me.lMEAlgorithm.Name = "lMEAlgorithm"
        Me.lMEAlgorithm.Size = New System.Drawing.Size(307, 70)
        Me.lMEAlgorithm.TabIndex = 2
        Me.lMEAlgorithm.Text = "M.E. Algorithm:"
        Me.lMEAlgorithm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbMEMethod
        '
        Me.cbMEMethod.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbMEMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMEMethod.FormattingEnabled = True
        Me.cbMEMethod.Location = New System.Drawing.Point(331, 172)
        Me.cbMEMethod.Name = "cbMEMethod"
        Me.cbMEMethod.Size = New System.Drawing.Size(400, 56)
        Me.cbMEMethod.TabIndex = 4
        '
        'cbTrellis
        '
        Me.cbTrellis.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cbTrellis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTrellis.FormattingEnabled = True
        Me.cbTrellis.Location = New System.Drawing.Point(568, 12)
        Me.cbTrellis.Name = "cbTrellis"
        Me.cbTrellis.Size = New System.Drawing.Size(270, 56)
        Me.cbTrellis.TabIndex = 8
        '
        'lTrellis
        '
        Me.lTrellis.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lTrellis.Location = New System.Drawing.Point(418, 5)
        Me.lTrellis.Name = "lTrellis"
        Me.lTrellis.Size = New System.Drawing.Size(144, 70)
        Me.lTrellis.TabIndex = 6
        Me.lTrellis.Text = "Trellis:"
        Me.lTrellis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbMixedReferences
        '
        Me.cbMixedReferences.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbMixedReferences.Location = New System.Drawing.Point(18, 5)
        Me.cbMixedReferences.Name = "cbMixedReferences"
        Me.cbMixedReferences.Size = New System.Drawing.Size(394, 70)
        Me.cbMixedReferences.TabIndex = 0
        Me.cbMixedReferences.Text = "Mixed Refs"
        Me.cbMixedReferences.UseVisualStyleBackColor = True
        '
        'lSubpixelRefinement
        '
        Me.lSubpixelRefinement.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMotionEstimationControls.SetColumnSpan(Me.lSubpixelRefinement, 3)
        Me.lSubpixelRefinement.Location = New System.Drawing.Point(18, 5)
        Me.lSubpixelRefinement.Name = "lSubpixelRefinement"
        Me.lSubpixelRefinement.Size = New System.Drawing.Size(829, 70)
        Me.lSubpixelRefinement.TabIndex = 0
        Me.lSubpixelRefinement.Text = "Subpixel Refinement:"
        Me.lSubpixelRefinement.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbSubME
        '
        Me.cbSubME.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMotionEstimationControls.SetColumnSpan(Me.cbSubME, 3)
        Me.cbSubME.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSubME.FormattingEnabled = True
        Me.cbSubME.Location = New System.Drawing.Point(25, 97)
        Me.cbSubME.Margin = New System.Windows.Forms.Padding(10, 3, 10, 3)
        Me.cbSubME.MaxDropDownItems = 30
        Me.cbSubME.Name = "cbSubME"
        Me.cbSubME.Size = New System.Drawing.Size(815, 56)
        Me.cbSubME.TabIndex = 1
        '
        'lDirectMode
        '
        Me.lDirectMode.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lDirectMode.Location = New System.Drawing.Point(519, 165)
        Me.lDirectMode.Name = "lDirectMode"
        Me.lDirectMode.Size = New System.Drawing.Size(328, 70)
        Me.lDirectMode.TabIndex = 0
        Me.lDirectMode.Text = "Direct Mode:"
        Me.lDirectMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbDirectMode
        '
        Me.cbDirectMode.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cbDirectMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDirectMode.FormattingEnabled = True
        Me.cbDirectMode.Location = New System.Drawing.Point(519, 252)
        Me.cbDirectMode.Name = "cbDirectMode"
        Me.cbDirectMode.Size = New System.Drawing.Size(300, 56)
        Me.cbDirectMode.TabIndex = 1
        '
        'cbWeightB
        '
        Me.cbWeightB.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbWeightB.Location = New System.Drawing.Point(18, 165)
        Me.cbWeightB.Name = "cbWeightB"
        Me.cbWeightB.Size = New System.Drawing.Size(495, 70)
        Me.cbWeightB.TabIndex = 4
        Me.cbWeightB.Text = "Weighted Pred. B-frame"
        '
        'nudBFramesBias
        '
        Me.nudBFramesBias.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudBFramesBias.Location = New System.Drawing.Point(218, 245)
        Me.nudBFramesBias.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudBFramesBias.Name = "nudBFramesBias"
        Me.nudBFramesBias.Size = New System.Drawing.Size(244, 70)
        Me.nudBFramesBias.TabIndex = 7
        '
        'lBias
        '
        Me.lBias.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lBias.Location = New System.Drawing.Point(18, 245)
        Me.lBias.Name = "lBias"
        Me.lBias.Size = New System.Drawing.Size(194, 70)
        Me.lBias.TabIndex = 6
        Me.lBias.Text = "Bias:"
        Me.lBias.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudQPCompCheck
        '
        Me.nudQPCompCheck.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudQPCompCheck.Location = New System.Drawing.Point(819, 403)
        Me.nudQPCompCheck.Maximum = New Decimal(New Integer() {51, 0, 0, 0})
        Me.nudQPCompCheck.Name = "nudQPCompCheck"
        Me.nudQPCompCheck.Size = New System.Drawing.Size(194, 70)
        Me.nudQPCompCheck.TabIndex = 3
        '
        'lCRFValueDefining100Quality
        '
        Me.lCRFValueDefining100Quality.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lCRFValueDefining100Quality.Location = New System.Drawing.Point(319, 403)
        Me.lCRFValueDefining100Quality.Name = "lCRFValueDefining100Quality"
        Me.lCRFValueDefining100Quality.Size = New System.Drawing.Size(494, 70)
        Me.lCRFValueDefining100Quality.TabIndex = 0
        Me.lCRFValueDefining100Quality.Text = "CRF value for 100% quality:"
        Me.lCRFValueDefining100Quality.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lAimedQuality
        '
        Me.lAimedQuality.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lAimedQuality.Location = New System.Drawing.Point(319, 323)
        Me.lAimedQuality.Name = "lAimedQuality"
        Me.lAimedQuality.Size = New System.Drawing.Size(494, 70)
        Me.lAimedQuality.TabIndex = 1
        Me.lAimedQuality.Text = "Aimed Quality (%):"
        Me.lAimedQuality.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudPercent
        '
        Me.nudPercent.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudPercent.Increment = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nudPercent.Location = New System.Drawing.Point(819, 323)
        Me.nudPercent.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me.nudPercent.Name = "nudPercent"
        Me.nudPercent.Size = New System.Drawing.Size(194, 70)
        Me.nudPercent.TabIndex = 2
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(192, 6)
        '
        'tcMain
        '
        Me.tcMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcMain.Controls.Add(Me.tpBasic)
        Me.tcMain.Controls.Add(Me.tpAnalysis)
        Me.tcMain.Controls.Add(Me.tpFrameOptions)
        Me.tcMain.Controls.Add(Me.tpRateControl)
        Me.tcMain.Controls.Add(Me.tpMisc)
        Me.tcMain.Controls.Add(Me.tpCommandLine)
        Me.tcMain.Controls.Add(Me.tpOther)
        Me.tcMain.Location = New System.Drawing.Point(15, 15)
        Me.tcMain.Margin = New System.Windows.Forms.Padding(15)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(1784, 932)
        Me.tcMain.TabIndex = 0
        '
        'tpBasic
        '
        Me.tpBasic.Controls.Add(Me.tlpBasicHost)
        Me.tpBasic.Location = New System.Drawing.Point(12, 69)
        Me.tpBasic.Name = "tpBasic"
        Me.tpBasic.Size = New System.Drawing.Size(1760, 851)
        Me.tpBasic.TabIndex = 0
        Me.tpBasic.Text = "   Basic   "
        Me.tpBasic.UseVisualStyleBackColor = True
        '
        'tlpBasicHost
        '
        Me.tlpBasicHost.ColumnCount = 1
        Me.tlpBasicHost.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpBasicHost.Controls.Add(Me.tlpBasicControls, 0, 0)
        Me.tlpBasicHost.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpBasicHost.Location = New System.Drawing.Point(0, 0)
        Me.tlpBasicHost.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tlpBasicHost.Name = "tlpBasicHost"
        Me.tlpBasicHost.RowCount = 1
        Me.tlpBasicHost.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpBasicHost.Size = New System.Drawing.Size(1760, 851)
        Me.tlpBasicHost.TabIndex = 23
        '
        'tlpBasicControls
        '
        Me.tlpBasicControls.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.tlpBasicControls.ColumnCount = 3
        Me.tlpBasicControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180.0!))
        Me.tlpBasicControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBasicControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180.0!))
        Me.tlpBasicControls.Controls.Add(Me.lQP, 0, 0)
        Me.tlpBasicControls.Controls.Add(Me.cbSlowFirstpass, 0, 7)
        Me.tlpBasicControls.Controls.Add(Me.cbMode, 1, 6)
        Me.tlpBasicControls.Controls.Add(Me.cbDepth, 1, 5)
        Me.tlpBasicControls.Controls.Add(Me.nudQP, 1, 0)
        Me.tlpBasicControls.Controls.Add(Me.lMode2, 0, 6)
        Me.tlpBasicControls.Controls.Add(Me.Label4, 0, 5)
        Me.tlpBasicControls.Controls.Add(Me.lPreset, 0, 1)
        Me.tlpBasicControls.Controls.Add(Me.cbPreset, 1, 1)
        Me.tlpBasicControls.Controls.Add(Me.cbDevice, 1, 3)
        Me.tlpBasicControls.Controls.Add(Me.cbProfile, 1, 4)
        Me.tlpBasicControls.Controls.Add(Me.lProfile, 0, 4)
        Me.tlpBasicControls.Controls.Add(Me.lTune, 0, 2)
        Me.tlpBasicControls.Controls.Add(Me.lDevice, 0, 3)
        Me.tlpBasicControls.Controls.Add(Me.cbTune, 1, 2)
        Me.tlpBasicControls.Location = New System.Drawing.Point(558, 105)
        Me.tlpBasicControls.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tlpBasicControls.Name = "tlpBasicControls"
        Me.tlpBasicControls.RowCount = 8
        Me.tlpBasicControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.tlpBasicControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.tlpBasicControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.tlpBasicControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.tlpBasicControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.tlpBasicControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.tlpBasicControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.tlpBasicControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.tlpBasicControls.Size = New System.Drawing.Size(644, 640)
        Me.tlpBasicControls.TabIndex = 22
        '
        'cbSlowFirstpass
        '
        Me.cbSlowFirstpass.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.tlpBasicControls.SetColumnSpan(Me.cbSlowFirstpass, 3)
        Me.cbSlowFirstpass.Location = New System.Drawing.Point(162, 565)
        Me.cbSlowFirstpass.Name = "cbSlowFirstpass"
        Me.cbSlowFirstpass.Size = New System.Drawing.Size(320, 70)
        Me.cbSlowFirstpass.TabIndex = 12
        Me.cbSlowFirstpass.Text = "Slow Firstpass"
        Me.cbSlowFirstpass.UseVisualStyleBackColor = True
        '
        'cbMode
        '
        Me.cbMode.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMode.FormattingEnabled = True
        Me.cbMode.Location = New System.Drawing.Point(183, 492)
        Me.cbMode.MaxDropDownItems = 20
        Me.cbMode.Name = "cbMode"
        Me.cbMode.Size = New System.Drawing.Size(278, 56)
        Me.cbMode.TabIndex = 11
        '
        'cbDepth
        '
        Me.cbDepth.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbDepth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDepth.FormattingEnabled = True
        Me.cbDepth.Location = New System.Drawing.Point(183, 412)
        Me.cbDepth.MaxDropDownItems = 20
        Me.cbDepth.Name = "cbDepth"
        Me.cbDepth.Size = New System.Drawing.Size(278, 56)
        Me.cbDepth.TabIndex = 21
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.Location = New System.Drawing.Point(3, 405)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(174, 70)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Depth:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lPreset
        '
        Me.lPreset.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lPreset.Location = New System.Drawing.Point(3, 85)
        Me.lPreset.Name = "lPreset"
        Me.lPreset.Size = New System.Drawing.Size(174, 70)
        Me.lPreset.TabIndex = 1
        Me.lPreset.Text = "Preset:"
        Me.lPreset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbPreset
        '
        Me.cbPreset.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbPreset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPreset.FormattingEnabled = True
        Me.cbPreset.Location = New System.Drawing.Point(183, 92)
        Me.cbPreset.MaxDropDownItems = 20
        Me.cbPreset.Name = "cbPreset"
        Me.cbPreset.Size = New System.Drawing.Size(278, 56)
        Me.cbPreset.TabIndex = 6
        '
        'cbDevice
        '
        Me.cbDevice.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDevice.FormattingEnabled = True
        Me.cbDevice.Location = New System.Drawing.Point(183, 252)
        Me.cbDevice.MaxDropDownItems = 20
        Me.cbDevice.Name = "cbDevice"
        Me.cbDevice.Size = New System.Drawing.Size(278, 56)
        Me.cbDevice.TabIndex = 9
        '
        'cbProfile
        '
        Me.cbProfile.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbProfile.FormattingEnabled = True
        Me.cbProfile.Location = New System.Drawing.Point(183, 332)
        Me.cbProfile.MaxDropDownItems = 20
        Me.cbProfile.Name = "cbProfile"
        Me.cbProfile.Size = New System.Drawing.Size(278, 56)
        Me.cbProfile.TabIndex = 10
        '
        'lProfile
        '
        Me.lProfile.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lProfile.Location = New System.Drawing.Point(3, 325)
        Me.lProfile.Name = "lProfile"
        Me.lProfile.Size = New System.Drawing.Size(174, 70)
        Me.lProfile.TabIndex = 4
        Me.lProfile.Text = "Profile:"
        Me.lProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lTune
        '
        Me.lTune.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lTune.Location = New System.Drawing.Point(3, 165)
        Me.lTune.Name = "lTune"
        Me.lTune.Size = New System.Drawing.Size(174, 70)
        Me.lTune.TabIndex = 2
        Me.lTune.Text = "Tune:"
        Me.lTune.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lDevice
        '
        Me.lDevice.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lDevice.Location = New System.Drawing.Point(3, 245)
        Me.lDevice.Name = "lDevice"
        Me.lDevice.Size = New System.Drawing.Size(174, 70)
        Me.lDevice.TabIndex = 3
        Me.lDevice.Text = "Device: "
        Me.lDevice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbTune
        '
        Me.cbTune.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbTune.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTune.FormattingEnabled = True
        Me.cbTune.Location = New System.Drawing.Point(183, 172)
        Me.cbTune.MaxDropDownItems = 20
        Me.cbTune.Name = "cbTune"
        Me.cbTune.Size = New System.Drawing.Size(278, 56)
        Me.cbTune.TabIndex = 8
        '
        'tpAnalysis
        '
        Me.tpAnalysis.Controls.Add(Me.tlpAnalysis)
        Me.tpAnalysis.Location = New System.Drawing.Point(12, 69)
        Me.tpAnalysis.Name = "tpAnalysis"
        Me.tpAnalysis.Size = New System.Drawing.Size(1760, 851)
        Me.tpAnalysis.TabIndex = 1
        Me.tpAnalysis.Text = "  Analysis  "
        Me.tpAnalysis.UseVisualStyleBackColor = True
        '
        'tlpAnalysis
        '
        Me.tlpAnalysis.ColumnCount = 2
        Me.tlpAnalysis.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAnalysis.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAnalysis.Controls.Add(Me.gbQuantOptions, 1, 0)
        Me.tlpAnalysis.Controls.Add(Me.gbPartitions, 0, 1)
        Me.tlpAnalysis.Controls.Add(Me.gbMotionEstimation, 0, 0)
        Me.tlpAnalysis.Controls.Add(Me.gbAnalysisMisc, 1, 1)
        Me.tlpAnalysis.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAnalysis.Location = New System.Drawing.Point(0, 0)
        Me.tlpAnalysis.Margin = New System.Windows.Forms.Padding(5)
        Me.tlpAnalysis.Name = "tlpAnalysis"
        Me.tlpAnalysis.RowCount = 2
        Me.tlpAnalysis.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAnalysis.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAnalysis.Size = New System.Drawing.Size(1760, 851)
        Me.tlpAnalysis.TabIndex = 0
        '
        'gbQuantOptions
        '
        Me.gbQuantOptions.Controls.Add(Me.tlpQuantOptionsControls)
        Me.gbQuantOptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbQuantOptions.Location = New System.Drawing.Point(886, 3)
        Me.gbQuantOptions.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.gbQuantOptions.Name = "gbQuantOptions"
        Me.gbQuantOptions.Size = New System.Drawing.Size(871, 419)
        Me.gbQuantOptions.TabIndex = 2
        Me.gbQuantOptions.TabStop = False
        Me.gbQuantOptions.Text = "Quant Options"
        '
        'tlpQuantOptionsControls
        '
        Me.tlpQuantOptionsControls.ColumnCount = 3
        Me.tlpQuantOptionsControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400.0!))
        Me.tlpQuantOptionsControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
        Me.tlpQuantOptionsControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpQuantOptionsControls.Controls.Add(Me.tlpPsy, 0, 2)
        Me.tlpQuantOptionsControls.Controls.Add(Me.cbMixedReferences, 0, 0)
        Me.tlpQuantOptionsControls.Controls.Add(Me.cbDCTDecimate, 0, 1)
        Me.tlpQuantOptionsControls.Controls.Add(Me.cbFastPSkip, 1, 1)
        Me.tlpQuantOptionsControls.Controls.Add(Me.cbTrellis, 2, 0)
        Me.tlpQuantOptionsControls.Controls.Add(Me.lTrellis, 1, 0)
        Me.tlpQuantOptionsControls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpQuantOptionsControls.Location = New System.Drawing.Point(3, 51)
        Me.tlpQuantOptionsControls.Name = "tlpQuantOptionsControls"
        Me.tlpQuantOptionsControls.Padding = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.tlpQuantOptionsControls.RowCount = 4
        Me.tlpQuantOptionsControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpQuantOptionsControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpQuantOptionsControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpQuantOptionsControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpQuantOptionsControls.Size = New System.Drawing.Size(865, 365)
        Me.tlpQuantOptionsControls.TabIndex = 13
        '
        'tlpPsy
        '
        Me.tlpPsy.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpPsy.ColumnCount = 6
        Me.tlpQuantOptionsControls.SetColumnSpan(Me.tlpPsy, 3)
        Me.tlpPsy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpPsy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpPsy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpPsy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpPsy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpPsy.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpPsy.Controls.Add(Me.nudPsyTrellis, 4, 0)
        Me.tlpPsy.Controls.Add(Me.lPsyTrellis, 3, 0)
        Me.tlpPsy.Controls.Add(Me.nudPsyRD, 2, 0)
        Me.tlpPsy.Controls.Add(Me.lPsyRD, 1, 0)
        Me.tlpPsy.Controls.Add(Me.cbPsy, 0, 0)
        Me.tlpPsy.Location = New System.Drawing.Point(15, 160)
        Me.tlpPsy.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpPsy.Name = "tlpPsy"
        Me.tlpPsy.RowCount = 1
        Me.tlpPsy.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpPsy.Size = New System.Drawing.Size(835, 80)
        Me.tlpPsy.TabIndex = 6
        '
        'nudPsyTrellis
        '
        Me.nudPsyTrellis.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudPsyTrellis.DecimalPlaces = 2
        Me.nudPsyTrellis.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudPsyTrellis.Location = New System.Drawing.Point(538, 5)
        Me.nudPsyTrellis.Name = "nudPsyTrellis"
        Me.nudPsyTrellis.Size = New System.Drawing.Size(200, 70)
        Me.nudPsyTrellis.TabIndex = 9
        '
        'lPsyTrellis
        '
        Me.lPsyTrellis.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lPsyTrellis.AutoSize = True
        Me.lPsyTrellis.Location = New System.Drawing.Point(414, 16)
        Me.lPsyTrellis.Name = "lPsyTrellis"
        Me.lPsyTrellis.Size = New System.Drawing.Size(118, 48)
        Me.lPsyTrellis.TabIndex = 7
        Me.lPsyTrellis.Text = "Trellis:"
        Me.lPsyTrellis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudPsyRD
        '
        Me.nudPsyRD.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudPsyRD.DecimalPlaces = 2
        Me.nudPsyRD.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudPsyRD.Location = New System.Drawing.Point(208, 5)
        Me.nudPsyRD.Maximum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.nudPsyRD.Name = "nudPsyRD"
        Me.nudPsyRD.Size = New System.Drawing.Size(200, 70)
        Me.nudPsyRD.TabIndex = 5
        '
        'lPsyRD
        '
        Me.lPsyRD.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lPsyRD.AutoSize = True
        Me.lPsyRD.Location = New System.Drawing.Point(127, 16)
        Me.lPsyRD.Name = "lPsyRD"
        Me.lPsyRD.Size = New System.Drawing.Size(75, 48)
        Me.lPsyRD.TabIndex = 4
        Me.lPsyRD.Text = "RD:"
        Me.lPsyRD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbPsy
        '
        Me.cbPsy.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbPsy.AutoSize = True
        Me.cbPsy.Location = New System.Drawing.Point(3, 14)
        Me.cbPsy.Name = "cbPsy"
        Me.cbPsy.Size = New System.Drawing.Size(118, 52)
        Me.cbPsy.TabIndex = 3
        Me.cbPsy.Text = "Psy"
        Me.cbPsy.UseVisualStyleBackColor = True
        '
        'gbPartitions
        '
        Me.gbPartitions.Controls.Add(Me.tlpPartitions)
        Me.gbPartitions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbPartitions.Location = New System.Drawing.Point(3, 428)
        Me.gbPartitions.Margin = New System.Windows.Forms.Padding(3, 3, 6, 3)
        Me.gbPartitions.Name = "gbPartitions"
        Me.gbPartitions.Size = New System.Drawing.Size(871, 420)
        Me.gbPartitions.TabIndex = 1
        Me.gbPartitions.TabStop = False
        Me.gbPartitions.Text = "Partitions"
        '
        'tlpPartitions
        '
        Me.tlpPartitions.ColumnCount = 3
        Me.tlpPartitions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpPartitions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpPartitions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpPartitions.Controls.Add(Me.cb8x8DCT, 2, 1)
        Me.tlpPartitions.Controls.Add(Me.cbI8x8, 2, 0)
        Me.tlpPartitions.Controls.Add(Me.cbb8x8, 1, 1)
        Me.tlpPartitions.Controls.Add(Me.cbP8x8, 1, 0)
        Me.tlpPartitions.Controls.Add(Me.cbi4x4, 0, 1)
        Me.tlpPartitions.Controls.Add(Me.cbP4x4, 0, 0)
        Me.tlpPartitions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpPartitions.Location = New System.Drawing.Point(3, 51)
        Me.tlpPartitions.Name = "tlpPartitions"
        Me.tlpPartitions.RowCount = 2
        Me.tlpPartitions.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpPartitions.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpPartitions.Size = New System.Drawing.Size(865, 366)
        Me.tlpPartitions.TabIndex = 6
        '
        'cb8x8DCT
        '
        Me.cb8x8DCT.Location = New System.Drawing.Point(579, 186)
        Me.cb8x8DCT.Name = "cb8x8DCT"
        Me.cb8x8DCT.Size = New System.Drawing.Size(220, 70)
        Me.cb8x8DCT.TabIndex = 5
        Me.cb8x8DCT.Text = "8x8 DCT"
        '
        'cbI8x8
        '
        Me.cbI8x8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbI8x8.Location = New System.Drawing.Point(579, 110)
        Me.cbI8x8.Name = "cbI8x8"
        Me.cbI8x8.Size = New System.Drawing.Size(220, 70)
        Me.cbI8x8.TabIndex = 4
        Me.cbI8x8.Text = "I8x8"
        '
        'cbb8x8
        '
        Me.cbb8x8.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.cbb8x8.Location = New System.Drawing.Point(322, 186)
        Me.cbb8x8.Name = "cbb8x8"
        Me.cbb8x8.Size = New System.Drawing.Size(220, 70)
        Me.cbb8x8.TabIndex = 3
        Me.cbb8x8.Text = "B8x8"
        '
        'cbP8x8
        '
        Me.cbP8x8.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cbP8x8.Location = New System.Drawing.Point(322, 110)
        Me.cbP8x8.Name = "cbP8x8"
        Me.cbP8x8.Size = New System.Drawing.Size(220, 70)
        Me.cbP8x8.TabIndex = 2
        Me.cbP8x8.Text = "P8x8"
        '
        'cbi4x4
        '
        Me.cbi4x4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbi4x4.Location = New System.Drawing.Point(65, 186)
        Me.cbi4x4.Name = "cbi4x4"
        Me.cbi4x4.Size = New System.Drawing.Size(220, 70)
        Me.cbi4x4.TabIndex = 1
        Me.cbi4x4.Text = "I4x4"
        '
        'cbP4x4
        '
        Me.cbP4x4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbP4x4.Location = New System.Drawing.Point(65, 110)
        Me.cbP4x4.Name = "cbP4x4"
        Me.cbP4x4.Size = New System.Drawing.Size(220, 70)
        Me.cbP4x4.TabIndex = 0
        Me.cbP4x4.Text = "P4x4"
        '
        'gbMotionEstimation
        '
        Me.gbMotionEstimation.Controls.Add(Me.tlpMotionEstimationControls)
        Me.gbMotionEstimation.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbMotionEstimation.Location = New System.Drawing.Point(3, 3)
        Me.gbMotionEstimation.Margin = New System.Windows.Forms.Padding(3, 3, 6, 3)
        Me.gbMotionEstimation.Name = "gbMotionEstimation"
        Me.gbMotionEstimation.Size = New System.Drawing.Size(871, 419)
        Me.gbMotionEstimation.TabIndex = 0
        Me.gbMotionEstimation.TabStop = False
        Me.gbMotionEstimation.Text = "Motion Estimation"
        '
        'tlpMotionEstimationControls
        '
        Me.tlpMotionEstimationControls.ColumnCount = 3
        Me.tlpMotionEstimationControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMotionEstimationControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMotionEstimationControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMotionEstimationControls.Controls.Add(Me.cbMEMethod, 1, 2)
        Me.tlpMotionEstimationControls.Controls.Add(Me.lSubpixelRefinement, 0, 0)
        Me.tlpMotionEstimationControls.Controls.Add(Me.cbSubME, 0, 1)
        Me.tlpMotionEstimationControls.Controls.Add(Me.lMEAlgorithm, 0, 2)
        Me.tlpMotionEstimationControls.Controls.Add(Me.tlpMe, 0, 3)
        Me.tlpMotionEstimationControls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMotionEstimationControls.Location = New System.Drawing.Point(3, 51)
        Me.tlpMotionEstimationControls.Name = "tlpMotionEstimationControls"
        Me.tlpMotionEstimationControls.Padding = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.tlpMotionEstimationControls.RowCount = 5
        Me.tlpMotionEstimationControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMotionEstimationControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMotionEstimationControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMotionEstimationControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMotionEstimationControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMotionEstimationControls.Size = New System.Drawing.Size(865, 365)
        Me.tlpMotionEstimationControls.TabIndex = 5
        '
        'tlpMe
        '
        Me.tlpMe.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMe.ColumnCount = 3
        Me.tlpMotionEstimationControls.SetColumnSpan(Me.tlpMe, 3)
        Me.tlpMe.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMe.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMe.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMe.Controls.Add(Me.cbChromaInME, 2, 0)
        Me.tlpMe.Controls.Add(Me.nudMeRange, 1, 0)
        Me.tlpMe.Controls.Add(Me.lMERange, 0, 0)
        Me.tlpMe.Location = New System.Drawing.Point(15, 245)
        Me.tlpMe.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpMe.Name = "tlpMe"
        Me.tlpMe.RowCount = 1
        Me.tlpMe.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMe.Size = New System.Drawing.Size(835, 70)
        Me.tlpMe.TabIndex = 7
        '
        'gbAnalysisMisc
        '
        Me.gbAnalysisMisc.Controls.Add(Me.tlpAnalysisMiscControls)
        Me.gbAnalysisMisc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbAnalysisMisc.Location = New System.Drawing.Point(886, 428)
        Me.gbAnalysisMisc.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.gbAnalysisMisc.Name = "gbAnalysisMisc"
        Me.gbAnalysisMisc.Size = New System.Drawing.Size(871, 420)
        Me.gbAnalysisMisc.TabIndex = 3
        Me.gbAnalysisMisc.TabStop = False
        Me.gbAnalysisMisc.Text = "Misc"
        '
        'tlpAnalysisMiscControls
        '
        Me.tlpAnalysisMiscControls.ColumnCount = 2
        Me.tlpAnalysisMiscControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.tlpAnalysisMiscControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.tlpAnalysisMiscControls.Controls.Add(Me.cbWeightB, 0, 2)
        Me.tlpAnalysisMiscControls.Controls.Add(Me.cbWeightP, 0, 1)
        Me.tlpAnalysisMiscControls.Controls.Add(Me.cbDirectMode, 1, 3)
        Me.tlpAnalysisMiscControls.Controls.Add(Me.lWeightP, 0, 0)
        Me.tlpAnalysisMiscControls.Controls.Add(Me.lNoiseReduction, 1, 0)
        Me.tlpAnalysisMiscControls.Controls.Add(Me.nudNoiseReduction, 1, 1)
        Me.tlpAnalysisMiscControls.Controls.Add(Me.lDirectMode, 1, 2)
        Me.tlpAnalysisMiscControls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAnalysisMiscControls.Location = New System.Drawing.Point(3, 51)
        Me.tlpAnalysisMiscControls.Name = "tlpAnalysisMiscControls"
        Me.tlpAnalysisMiscControls.Padding = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.tlpAnalysisMiscControls.RowCount = 5
        Me.tlpAnalysisMiscControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpAnalysisMiscControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpAnalysisMiscControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpAnalysisMiscControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpAnalysisMiscControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpAnalysisMiscControls.Size = New System.Drawing.Size(865, 366)
        Me.tlpAnalysisMiscControls.TabIndex = 5
        '
        'cbWeightP
        '
        Me.cbWeightP.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cbWeightP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbWeightP.FormattingEnabled = True
        Me.cbWeightP.Location = New System.Drawing.Point(18, 92)
        Me.cbWeightP.Name = "cbWeightP"
        Me.cbWeightP.Size = New System.Drawing.Size(300, 56)
        Me.cbWeightP.TabIndex = 3
        '
        'lWeightP
        '
        Me.lWeightP.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lWeightP.Location = New System.Drawing.Point(18, 5)
        Me.lWeightP.Name = "lWeightP"
        Me.lWeightP.Size = New System.Drawing.Size(495, 70)
        Me.lWeightP.TabIndex = 2
        Me.lWeightP.Text = "Weighted Pred. P-frame"
        Me.lWeightP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lNoiseReduction
        '
        Me.lNoiseReduction.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lNoiseReduction.Location = New System.Drawing.Point(519, 5)
        Me.lNoiseReduction.Name = "lNoiseReduction"
        Me.lNoiseReduction.Size = New System.Drawing.Size(328, 70)
        Me.lNoiseReduction.TabIndex = 5
        Me.lNoiseReduction.Text = "Noise Reduction:"
        Me.lNoiseReduction.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudNoiseReduction
        '
        Me.nudNoiseReduction.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudNoiseReduction.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.nudNoiseReduction.Location = New System.Drawing.Point(519, 85)
        Me.nudNoiseReduction.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.nudNoiseReduction.Name = "nudNoiseReduction"
        Me.nudNoiseReduction.Size = New System.Drawing.Size(200, 70)
        Me.nudNoiseReduction.TabIndex = 6
        '
        'tpFrameOptions
        '
        Me.tpFrameOptions.Controls.Add(Me.tlpFrameOptions)
        Me.tpFrameOptions.Location = New System.Drawing.Point(12, 69)
        Me.tpFrameOptions.Name = "tpFrameOptions"
        Me.tpFrameOptions.Size = New System.Drawing.Size(1760, 851)
        Me.tpFrameOptions.TabIndex = 2
        Me.tpFrameOptions.Text = " Frame Options "
        Me.tpFrameOptions.UseVisualStyleBackColor = True
        '
        'tlpFrameOptions
        '
        Me.tlpFrameOptions.ColumnCount = 2
        Me.tlpFrameOptions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpFrameOptions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpFrameOptions.Controls.Add(Me.gbBFrames, 0, 0)
        Me.tlpFrameOptions.Controls.Add(Me.gbFrameOptionsRight, 1, 0)
        Me.tlpFrameOptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpFrameOptions.Location = New System.Drawing.Point(0, 0)
        Me.tlpFrameOptions.Margin = New System.Windows.Forms.Padding(5)
        Me.tlpFrameOptions.Name = "tlpFrameOptions"
        Me.tlpFrameOptions.RowCount = 1
        Me.tlpFrameOptions.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpFrameOptions.Size = New System.Drawing.Size(1760, 851)
        Me.tlpFrameOptions.TabIndex = 0
        '
        'gbBFrames
        '
        Me.gbBFrames.Controls.Add(Me.tlpBframeControls)
        Me.gbBFrames.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbBFrames.Location = New System.Drawing.Point(3, 3)
        Me.gbBFrames.Margin = New System.Windows.Forms.Padding(3, 3, 6, 3)
        Me.gbBFrames.Name = "gbBFrames"
        Me.gbBFrames.Size = New System.Drawing.Size(871, 845)
        Me.gbBFrames.TabIndex = 0
        Me.gbBFrames.TabStop = False
        Me.gbBFrames.Text = "B-frames"
        '
        'tlpBframeControls
        '
        Me.tlpBframeControls.ColumnCount = 3
        Me.tlpBframeControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.tlpBframeControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250.0!))
        Me.tlpBframeControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBframeControls.Controls.Add(Me.cbBAdapt, 1, 0)
        Me.tlpBframeControls.Controls.Add(Me.lBias, 0, 3)
        Me.tlpBframeControls.Controls.Add(Me.lBFrames, 0, 2)
        Me.tlpBframeControls.Controls.Add(Me.nudBFramesBias, 1, 3)
        Me.tlpBframeControls.Controls.Add(Me.lBPyramidMode, 0, 1)
        Me.tlpBframeControls.Controls.Add(Me.nudBFrames, 1, 2)
        Me.tlpBframeControls.Controls.Add(Me.lBAdapt, 0, 0)
        Me.tlpBframeControls.Controls.Add(Me.cbBPyramidMode, 1, 1)
        Me.tlpBframeControls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpBframeControls.Location = New System.Drawing.Point(3, 51)
        Me.tlpBframeControls.Name = "tlpBframeControls"
        Me.tlpBframeControls.Padding = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.tlpBframeControls.RowCount = 5
        Me.tlpBframeControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpBframeControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpBframeControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpBframeControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpBframeControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBframeControls.Size = New System.Drawing.Size(865, 791)
        Me.tlpBframeControls.TabIndex = 8
        '
        'cbBAdapt
        '
        Me.cbBAdapt.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbBAdapt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbBAdapt.FormattingEnabled = True
        Me.cbBAdapt.Location = New System.Drawing.Point(218, 17)
        Me.cbBAdapt.Name = "cbBAdapt"
        Me.cbBAdapt.Size = New System.Drawing.Size(244, 56)
        Me.cbBAdapt.TabIndex = 2
        '
        'lBFrames
        '
        Me.lBFrames.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lBFrames.Location = New System.Drawing.Point(18, 165)
        Me.lBFrames.Name = "lBFrames"
        Me.lBFrames.Size = New System.Drawing.Size(194, 70)
        Me.lBFrames.TabIndex = 3
        Me.lBFrames.Text = "B-Frames:"
        Me.lBFrames.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lBPyramidMode
        '
        Me.lBPyramidMode.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lBPyramidMode.Location = New System.Drawing.Point(18, 85)
        Me.lBPyramidMode.Name = "lBPyramidMode"
        Me.lBPyramidMode.Size = New System.Drawing.Size(194, 70)
        Me.lBPyramidMode.TabIndex = 1
        Me.lBPyramidMode.Text = "Pyramid:"
        Me.lBPyramidMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudBFrames
        '
        Me.nudBFrames.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudBFrames.Location = New System.Drawing.Point(218, 165)
        Me.nudBFrames.Maximum = New Decimal(New Integer() {16, 0, 0, 0})
        Me.nudBFrames.Name = "nudBFrames"
        Me.nudBFrames.Size = New System.Drawing.Size(244, 70)
        Me.nudBFrames.TabIndex = 5
        '
        'lBAdapt
        '
        Me.lBAdapt.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lBAdapt.Location = New System.Drawing.Point(18, 5)
        Me.lBAdapt.Name = "lBAdapt"
        Me.lBAdapt.Size = New System.Drawing.Size(194, 70)
        Me.lBAdapt.TabIndex = 0
        Me.lBAdapt.Text = "Adaptive:"
        Me.lBAdapt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbBPyramidMode
        '
        Me.cbBPyramidMode.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbBPyramidMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbBPyramidMode.FormattingEnabled = True
        Me.cbBPyramidMode.Location = New System.Drawing.Point(218, 97)
        Me.cbBPyramidMode.Name = "cbBPyramidMode"
        Me.cbBPyramidMode.Size = New System.Drawing.Size(244, 56)
        Me.cbBPyramidMode.TabIndex = 4
        '
        'gbFrameOptionsRight
        '
        Me.gbFrameOptionsRight.Controls.Add(Me.tlpFrameOptionRightHost)
        Me.gbFrameOptionsRight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbFrameOptionsRight.Location = New System.Drawing.Point(886, 3)
        Me.gbFrameOptionsRight.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.gbFrameOptionsRight.Name = "gbFrameOptionsRight"
        Me.gbFrameOptionsRight.Size = New System.Drawing.Size(871, 845)
        Me.gbFrameOptionsRight.TabIndex = 1
        Me.gbFrameOptionsRight.TabStop = False
        '
        'tlpFrameOptionRightHost
        '
        Me.tlpFrameOptionRightHost.ColumnCount = 1
        Me.tlpFrameOptionRightHost.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpFrameOptionRightHost.Controls.Add(Me.tlpFrameOptionsRightTopControls, 0, 0)
        Me.tlpFrameOptionRightHost.Controls.Add(Me.tlpDeblockingHost, 0, 1)
        Me.tlpFrameOptionRightHost.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpFrameOptionRightHost.Location = New System.Drawing.Point(3, 51)
        Me.tlpFrameOptionRightHost.Name = "tlpFrameOptionRightHost"
        Me.tlpFrameOptionRightHost.Padding = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.tlpFrameOptionRightHost.RowCount = 2
        Me.tlpFrameOptionRightHost.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpFrameOptionRightHost.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpFrameOptionRightHost.Size = New System.Drawing.Size(865, 791)
        Me.tlpFrameOptionRightHost.TabIndex = 0
        '
        'tlpFrameOptionsRightTopControls
        '
        Me.tlpFrameOptionsRightTopControls.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpFrameOptionsRightTopControls.ColumnCount = 5
        Me.tlpFrameOptionsRightTopControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.tlpFrameOptionsRightTopControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110.0!))
        Me.tlpFrameOptionsRightTopControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.tlpFrameOptionsRightTopControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110.0!))
        Me.tlpFrameOptionsRightTopControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpFrameOptionsRightTopControls.Controls.Add(Me.nudReferenceFrames, 2, 0)
        Me.tlpFrameOptionsRightTopControls.Controls.Add(Me.nudSceneCut, 2, 1)
        Me.tlpFrameOptionsRightTopControls.Controls.Add(Me.cbCABAC, 3, 1)
        Me.tlpFrameOptionsRightTopControls.Controls.Add(Me.cbOpenGOP, 3, 2)
        Me.tlpFrameOptionsRightTopControls.Controls.Add(Me.nudSlices, 2, 2)
        Me.tlpFrameOptionsRightTopControls.Controls.Add(Me.lReferenceFrames, 0, 0)
        Me.tlpFrameOptionsRightTopControls.Controls.Add(Me.Label1, 0, 3)
        Me.tlpFrameOptionsRightTopControls.Controls.Add(Me.lSlices, 0, 2)
        Me.tlpFrameOptionsRightTopControls.Controls.Add(Me.lSceneCut, 0, 1)
        Me.tlpFrameOptionsRightTopControls.Controls.Add(Me.nudGOPSizeMax, 4, 3)
        Me.tlpFrameOptionsRightTopControls.Controls.Add(Me.lMin, 1, 3)
        Me.tlpFrameOptionsRightTopControls.Controls.Add(Me.nudGOPSizeMin, 2, 3)
        Me.tlpFrameOptionsRightTopControls.Controls.Add(Me.lMax, 3, 3)
        Me.tlpFrameOptionsRightTopControls.Location = New System.Drawing.Point(15, 0)
        Me.tlpFrameOptionsRightTopControls.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpFrameOptionsRightTopControls.Name = "tlpFrameOptionsRightTopControls"
        Me.tlpFrameOptionsRightTopControls.RowCount = 4
        Me.tlpFrameOptionsRightTopControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpFrameOptionsRightTopControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpFrameOptionsRightTopControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpFrameOptionsRightTopControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpFrameOptionsRightTopControls.Size = New System.Drawing.Size(835, 320)
        Me.tlpFrameOptionsRightTopControls.TabIndex = 19
        '
        'nudReferenceFrames
        '
        Me.nudReferenceFrames.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudReferenceFrames.Location = New System.Drawing.Point(313, 5)
        Me.nudReferenceFrames.Maximum = New Decimal(New Integer() {16, 0, 0, 0})
        Me.nudReferenceFrames.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudReferenceFrames.Name = "nudReferenceFrames"
        Me.nudReferenceFrames.Size = New System.Drawing.Size(194, 70)
        Me.nudReferenceFrames.TabIndex = 4
        Me.nudReferenceFrames.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'nudSceneCut
        '
        Me.nudSceneCut.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudSceneCut.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudSceneCut.Location = New System.Drawing.Point(313, 85)
        Me.nudSceneCut.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudSceneCut.Name = "nudSceneCut"
        Me.nudSceneCut.Size = New System.Drawing.Size(194, 70)
        Me.nudSceneCut.TabIndex = 5
        '
        'cbOpenGOP
        '
        Me.cbOpenGOP.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tlpFrameOptionsRightTopControls.SetColumnSpan(Me.cbOpenGOP, 2)
        Me.cbOpenGOP.Location = New System.Drawing.Point(525, 165)
        Me.cbOpenGOP.Margin = New System.Windows.Forms.Padding(15, 3, 3, 3)
        Me.cbOpenGOP.Name = "cbOpenGOP"
        Me.cbOpenGOP.Size = New System.Drawing.Size(260, 70)
        Me.cbOpenGOP.TabIndex = 16
        Me.cbOpenGOP.Text = "Open GOP"
        Me.cbOpenGOP.UseVisualStyleBackColor = True
        '
        'nudSlices
        '
        Me.nudSlices.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudSlices.Location = New System.Drawing.Point(313, 165)
        Me.nudSlices.Name = "nudSlices"
        Me.nudSlices.Size = New System.Drawing.Size(194, 70)
        Me.nudSlices.TabIndex = 8
        '
        'lReferenceFrames
        '
        Me.lReferenceFrames.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lReferenceFrames.Location = New System.Drawing.Point(3, 5)
        Me.lReferenceFrames.Name = "lReferenceFrames"
        Me.lReferenceFrames.Size = New System.Drawing.Size(194, 70)
        Me.lReferenceFrames.TabIndex = 0
        Me.lReferenceFrames.Text = "Ref Frames:"
        Me.lReferenceFrames.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(3, 245)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(194, 70)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "GOP Size:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSlices
        '
        Me.lSlices.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lSlices.Location = New System.Drawing.Point(3, 165)
        Me.lSlices.Name = "lSlices"
        Me.lSlices.Size = New System.Drawing.Size(194, 70)
        Me.lSlices.TabIndex = 2
        Me.lSlices.Text = "Slices:"
        Me.lSlices.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lSceneCut
        '
        Me.lSceneCut.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lSceneCut.Location = New System.Drawing.Point(3, 85)
        Me.lSceneCut.Name = "lSceneCut"
        Me.lSceneCut.Size = New System.Drawing.Size(194, 70)
        Me.lSceneCut.TabIndex = 1
        Me.lSceneCut.Text = "Scene Cut:"
        Me.lSceneCut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tlpDeblockingHost
        '
        Me.tlpDeblockingHost.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpDeblockingHost.ColumnCount = 2
        Me.tlpDeblockingHost.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpDeblockingHost.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpDeblockingHost.Controls.Add(Me.cbDeblock, 0, 0)
        Me.tlpDeblockingHost.Controls.Add(Me.tlpDeblockingControls, 0, 1)
        Me.tlpDeblockingHost.Controls.Add(Me.LineControl1, 1, 0)
        Me.tlpDeblockingHost.Location = New System.Drawing.Point(15, 320)
        Me.tlpDeblockingHost.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpDeblockingHost.Name = "tlpDeblockingHost"
        Me.tlpDeblockingHost.RowCount = 2
        Me.tlpDeblockingHost.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpDeblockingHost.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160.0!))
        Me.tlpDeblockingHost.Size = New System.Drawing.Size(835, 240)
        Me.tlpDeblockingHost.TabIndex = 18
        '
        'cbDeblock
        '
        Me.cbDeblock.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cbDeblock.Location = New System.Drawing.Point(10, 5)
        Me.cbDeblock.Margin = New System.Windows.Forms.Padding(10, 3, 3, 3)
        Me.cbDeblock.Name = "cbDeblock"
        Me.cbDeblock.Size = New System.Drawing.Size(280, 70)
        Me.cbDeblock.TabIndex = 6
        Me.cbDeblock.Text = "Deblocking"
        '
        'tlpDeblockingControls
        '
        Me.tlpDeblockingControls.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpDeblockingControls.ColumnCount = 3
        Me.tlpDeblockingHost.SetColumnSpan(Me.tlpDeblockingControls, 2)
        Me.tlpDeblockingControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210.0!))
        Me.tlpDeblockingControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.tlpDeblockingControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpDeblockingControls.Controls.Add(Me.lDeblockThresholdHint, 2, 1)
        Me.tlpDeblockingControls.Controls.Add(Me.nudDeblockBeta, 1, 1)
        Me.tlpDeblockingControls.Controls.Add(Me.lStrength, 0, 0)
        Me.tlpDeblockingControls.Controls.Add(Me.nudDeblockAlpha, 1, 0)
        Me.tlpDeblockingControls.Controls.Add(Me.lThreshold, 0, 1)
        Me.tlpDeblockingControls.Controls.Add(Me.lDeblockStrengthHint, 2, 0)
        Me.tlpDeblockingControls.Location = New System.Drawing.Point(0, 80)
        Me.tlpDeblockingControls.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpDeblockingControls.Name = "tlpDeblockingControls"
        Me.tlpDeblockingControls.RowCount = 2
        Me.tlpDeblockingControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpDeblockingControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpDeblockingControls.Size = New System.Drawing.Size(835, 160)
        Me.tlpDeblockingControls.TabIndex = 17
        '
        'lDeblockThresholdHint
        '
        Me.lDeblockThresholdHint.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lDeblockThresholdHint.Location = New System.Drawing.Point(413, 85)
        Me.lDeblockThresholdHint.Name = "lDeblockThresholdHint"
        Me.lDeblockThresholdHint.Size = New System.Drawing.Size(419, 70)
        Me.lDeblockThresholdHint.TabIndex = 5
        Me.lDeblockThresholdHint.Text = "(hint)"
        Me.lDeblockThresholdHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudDeblockBeta
        '
        Me.nudDeblockBeta.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudDeblockBeta.Location = New System.Drawing.Point(213, 85)
        Me.nudDeblockBeta.Maximum = New Decimal(New Integer() {6, 0, 0, 0})
        Me.nudDeblockBeta.Minimum = New Decimal(New Integer() {6, 0, 0, -2147483648})
        Me.nudDeblockBeta.Name = "nudDeblockBeta"
        Me.nudDeblockBeta.Size = New System.Drawing.Size(194, 70)
        Me.nudDeblockBeta.TabIndex = 3
        '
        'lStrength
        '
        Me.lStrength.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lStrength.Location = New System.Drawing.Point(3, 5)
        Me.lStrength.Name = "lStrength"
        Me.lStrength.Size = New System.Drawing.Size(204, 70)
        Me.lStrength.TabIndex = 0
        Me.lStrength.Text = "Strength:"
        Me.lStrength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudDeblockAlpha
        '
        Me.nudDeblockAlpha.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudDeblockAlpha.Location = New System.Drawing.Point(213, 5)
        Me.nudDeblockAlpha.Maximum = New Decimal(New Integer() {6, 0, 0, 0})
        Me.nudDeblockAlpha.Minimum = New Decimal(New Integer() {6, 0, 0, -2147483648})
        Me.nudDeblockAlpha.Name = "nudDeblockAlpha"
        Me.nudDeblockAlpha.Size = New System.Drawing.Size(194, 70)
        Me.nudDeblockAlpha.TabIndex = 2
        '
        'lThreshold
        '
        Me.lThreshold.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lThreshold.Location = New System.Drawing.Point(3, 85)
        Me.lThreshold.Name = "lThreshold"
        Me.lThreshold.Size = New System.Drawing.Size(204, 70)
        Me.lThreshold.TabIndex = 1
        Me.lThreshold.Text = "Threshold:"
        Me.lThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lDeblockStrengthHint
        '
        Me.lDeblockStrengthHint.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lDeblockStrengthHint.Location = New System.Drawing.Point(413, 5)
        Me.lDeblockStrengthHint.Name = "lDeblockStrengthHint"
        Me.lDeblockStrengthHint.Size = New System.Drawing.Size(419, 70)
        Me.lDeblockStrengthHint.TabIndex = 4
        Me.lDeblockStrengthHint.Text = "(hint)"
        Me.lDeblockStrengthHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LineControl1
        '
        Me.LineControl1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineControl1.Location = New System.Drawing.Point(297, 25)
        Me.LineControl1.Margin = New System.Windows.Forms.Padding(4, 2, 5, 2)
        Me.LineControl1.Name = "LineControl1"
        Me.LineControl1.Size = New System.Drawing.Size(533, 30)
        Me.LineControl1.TabIndex = 11
        '
        'tpRateControl
        '
        Me.tpRateControl.Controls.Add(Me.tlpRateControl)
        Me.tpRateControl.Location = New System.Drawing.Point(12, 69)
        Me.tpRateControl.Name = "tpRateControl"
        Me.tpRateControl.Size = New System.Drawing.Size(1760, 851)
        Me.tpRateControl.TabIndex = 3
        Me.tpRateControl.Text = " Rate Control "
        Me.tpRateControl.UseVisualStyleBackColor = True
        '
        'tlpRateControl
        '
        Me.tlpRateControl.ColumnCount = 2
        Me.tlpRateControl.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpRateControl.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpRateControl.Controls.Add(Me.gbRC2, 1, 0)
        Me.tlpRateControl.Controls.Add(Me.gbRC1, 0, 0)
        Me.tlpRateControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpRateControl.Location = New System.Drawing.Point(0, 0)
        Me.tlpRateControl.Margin = New System.Windows.Forms.Padding(5)
        Me.tlpRateControl.Name = "tlpRateControl"
        Me.tlpRateControl.RowCount = 1
        Me.tlpRateControl.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpRateControl.Size = New System.Drawing.Size(1760, 851)
        Me.tlpRateControl.TabIndex = 0
        '
        'gbRC2
        '
        Me.gbRC2.Controls.Add(Me.tlpRateControlRightControls)
        Me.gbRC2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbRC2.Location = New System.Drawing.Point(886, 3)
        Me.gbRC2.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.gbRC2.Name = "gbRC2"
        Me.gbRC2.Size = New System.Drawing.Size(871, 845)
        Me.gbRC2.TabIndex = 1
        Me.gbRC2.TabStop = False
        '
        'tlpRateControlRightControls
        '
        Me.tlpRateControlRightControls.ColumnCount = 2
        Me.tlpRateControlRightControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350.0!))
        Me.tlpRateControlRightControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpRateControlRightControls.Controls.Add(Me.lQPMinimum, 0, 0)
        Me.tlpRateControlRightControls.Controls.Add(Me.cbMBTree, 0, 5)
        Me.tlpRateControlRightControls.Controls.Add(Me.nudRcLookahead, 1, 4)
        Me.tlpRateControlRightControls.Controls.Add(Me.lRcLookahead, 0, 4)
        Me.tlpRateControlRightControls.Controls.Add(Me.lQPComp, 0, 1)
        Me.tlpRateControlRightControls.Controls.Add(Me.nudPBRatio, 1, 3)
        Me.tlpRateControlRightControls.Controls.Add(Me.nudIPRatio, 1, 2)
        Me.tlpRateControlRightControls.Controls.Add(Me.lIPRatio, 0, 2)
        Me.tlpRateControlRightControls.Controls.Add(Me.lPBRatio, 0, 3)
        Me.tlpRateControlRightControls.Controls.Add(Me.nudQPComp, 1, 1)
        Me.tlpRateControlRightControls.Controls.Add(Me.nudQPMin, 1, 0)
        Me.tlpRateControlRightControls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpRateControlRightControls.Location = New System.Drawing.Point(3, 51)
        Me.tlpRateControlRightControls.Name = "tlpRateControlRightControls"
        Me.tlpRateControlRightControls.Padding = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.tlpRateControlRightControls.RowCount = 7
        Me.tlpRateControlRightControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpRateControlRightControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpRateControlRightControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpRateControlRightControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpRateControlRightControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpRateControlRightControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpRateControlRightControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpRateControlRightControls.Size = New System.Drawing.Size(865, 791)
        Me.tlpRateControlRightControls.TabIndex = 15
        '
        'cbMBTree
        '
        Me.cbMBTree.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbMBTree.Location = New System.Drawing.Point(20, 405)
        Me.cbMBTree.Margin = New System.Windows.Forms.Padding(10, 3, 3, 3)
        Me.cbMBTree.Name = "cbMBTree"
        Me.cbMBTree.Size = New System.Drawing.Size(337, 70)
        Me.cbMBTree.TabIndex = 10
        Me.cbMBTree.Text = "MB Tree"
        Me.cbMBTree.UseVisualStyleBackColor = True
        '
        'nudRcLookahead
        '
        Me.nudRcLookahead.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudRcLookahead.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudRcLookahead.Location = New System.Drawing.Point(363, 325)
        Me.nudRcLookahead.Maximum = New Decimal(New Integer() {250, 0, 0, 0})
        Me.nudRcLookahead.Name = "nudRcLookahead"
        Me.nudRcLookahead.Size = New System.Drawing.Size(200, 70)
        Me.nudRcLookahead.TabIndex = 9
        '
        'lRcLookahead
        '
        Me.lRcLookahead.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lRcLookahead.Location = New System.Drawing.Point(13, 325)
        Me.lRcLookahead.Size = New System.Drawing.Size(344, 70)
        Me.lRcLookahead.Text = "RC Lookahead:"
        Me.lRcLookahead.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gbRC1
        '
        Me.gbRC1.Controls.Add(Me.tlpRateControlLeftControls)
        Me.gbRC1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbRC1.Location = New System.Drawing.Point(3, 3)
        Me.gbRC1.Margin = New System.Windows.Forms.Padding(3, 3, 6, 3)
        Me.gbRC1.Name = "gbRC1"
        Me.gbRC1.Size = New System.Drawing.Size(871, 845)
        Me.gbRC1.TabIndex = 0
        Me.gbRC1.TabStop = False
        '
        'tlpRateControlLeftControls
        '
        Me.tlpRateControlLeftControls.ColumnCount = 3
        Me.tlpRateControlLeftControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 270.0!))
        Me.tlpRateControlLeftControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.tlpRateControlLeftControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpRateControlLeftControls.Controls.Add(Me.lMode, 0, 0)
        Me.tlpRateControlLeftControls.Controls.Add(Me.nudVBVInit, 1, 6)
        Me.tlpRateControlLeftControls.Controls.Add(Me.lInitialBuffer, 0, 6)
        Me.tlpRateControlLeftControls.Controls.Add(Me.nudVBVMaxRate, 1, 5)
        Me.tlpRateControlLeftControls.Controls.Add(Me.lBufferSize, 0, 4)
        Me.tlpRateControlLeftControls.Controls.Add(Me.nudVBVBufSize, 1, 4)
        Me.tlpRateControlLeftControls.Controls.Add(Me.LineControl2, 0, 3)
        Me.tlpRateControlLeftControls.Controls.Add(Me.cbAQMode, 0, 1)
        Me.tlpRateControlLeftControls.Controls.Add(Me.lMaxBitrate, 0, 5)
        Me.tlpRateControlLeftControls.Controls.Add(Me.nudAQStrength, 1, 2)
        Me.tlpRateControlLeftControls.Controls.Add(Me.lAQStrengthHint, 2, 2)
        Me.tlpRateControlLeftControls.Controls.Add(Me.Label2, 0, 2)
        Me.tlpRateControlLeftControls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpRateControlLeftControls.Location = New System.Drawing.Point(3, 51)
        Me.tlpRateControlLeftControls.Name = "tlpRateControlLeftControls"
        Me.tlpRateControlLeftControls.Padding = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.tlpRateControlLeftControls.RowCount = 8
        Me.tlpRateControlLeftControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpRateControlLeftControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpRateControlLeftControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpRateControlLeftControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpRateControlLeftControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpRateControlLeftControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpRateControlLeftControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpRateControlLeftControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpRateControlLeftControls.Size = New System.Drawing.Size(865, 791)
        Me.tlpRateControlLeftControls.TabIndex = 13
        '
        'LineControl2
        '
        Me.LineControl2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpRateControlLeftControls.SetColumnSpan(Me.LineControl2, 3)
        Me.LineControl2.Location = New System.Drawing.Point(14, 245)
        Me.LineControl2.Margin = New System.Windows.Forms.Padding(4, 2, 5, 2)
        Me.LineControl2.Name = "LineControl2"
        Me.LineControl2.Size = New System.Drawing.Size(836, 70)
        Me.LineControl2.TabIndex = 3
        Me.LineControl2.Text = "VBV"
        '
        'nudAQStrength
        '
        Me.nudAQStrength.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudAQStrength.DecimalPlaces = 1
        Me.nudAQStrength.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudAQStrength.Location = New System.Drawing.Point(283, 165)
        Me.nudAQStrength.Maximum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.nudAQStrength.Name = "nudAQStrength"
        Me.nudAQStrength.Size = New System.Drawing.Size(194, 70)
        Me.nudAQStrength.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.Location = New System.Drawing.Point(13, 165)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(264, 70)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "AQ Strength:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tpMisc
        '
        Me.tpMisc.Controls.Add(Me.tlpMisc)
        Me.tpMisc.Location = New System.Drawing.Point(12, 69)
        Me.tpMisc.Name = "tpMisc"
        Me.tpMisc.Size = New System.Drawing.Size(1760, 851)
        Me.tpMisc.TabIndex = 7
        Me.tpMisc.Text = "   Misc   "
        Me.tpMisc.UseVisualStyleBackColor = True
        '
        'tlpMisc
        '
        Me.tlpMisc.ColumnCount = 2
        Me.tlpMisc.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMisc.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMisc.Controls.Add(Me.gbIO, 0, 0)
        Me.tlpMisc.Controls.Add(Me.GroupBox6, 1, 0)
        Me.tlpMisc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMisc.Location = New System.Drawing.Point(0, 0)
        Me.tlpMisc.Margin = New System.Windows.Forms.Padding(5)
        Me.tlpMisc.Name = "tlpMisc"
        Me.tlpMisc.RowCount = 1
        Me.tlpMisc.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMisc.Size = New System.Drawing.Size(1760, 851)
        Me.tlpMisc.TabIndex = 0
        '
        'gbIO
        '
        Me.gbIO.Controls.Add(Me.tlpMiscIO)
        Me.gbIO.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbIO.Location = New System.Drawing.Point(3, 3)
        Me.gbIO.Margin = New System.Windows.Forms.Padding(3, 3, 6, 3)
        Me.gbIO.Name = "gbIO"
        Me.gbIO.Size = New System.Drawing.Size(871, 845)
        Me.gbIO.TabIndex = 0
        Me.gbIO.TabStop = False
        Me.gbIO.Text = "Input/Output"
        '
        'tlpMiscIO
        '
        Me.tlpMiscIO.ColumnCount = 3
        Me.tlpMiscIO.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.tlpMiscIO.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMiscIO.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMiscIO.Controls.Add(Me.cbProgress, 0, 0)
        Me.tlpMiscIO.Controls.Add(Me.cbLevel, 1, 7)
        Me.tlpMiscIO.Controls.Add(Me.lLevel, 0, 7)
        Me.tlpMiscIO.Controls.Add(Me.cbBlurayCompat, 0, 5)
        Me.tlpMiscIO.Controls.Add(Me.cbThreadInput, 0, 1)
        Me.tlpMiscIO.Controls.Add(Me.cbAud, 0, 4)
        Me.tlpMiscIO.Controls.Add(Me.nudThreads, 1, 6)
        Me.tlpMiscIO.Controls.Add(Me.lThreads, 0, 6)
        Me.tlpMiscIO.Controls.Add(Me.cbSSIM, 0, 2)
        Me.tlpMiscIO.Controls.Add(Me.cbPSNR, 0, 3)
        Me.tlpMiscIO.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMiscIO.Location = New System.Drawing.Point(3, 51)
        Me.tlpMiscIO.Name = "tlpMiscIO"
        Me.tlpMiscIO.Padding = New System.Windows.Forms.Padding(15, 0, 0, 0)
        Me.tlpMiscIO.RowCount = 9
        Me.tlpMiscIO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMiscIO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMiscIO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMiscIO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMiscIO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMiscIO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMiscIO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMiscIO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMiscIO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65.0!))
        Me.tlpMiscIO.Size = New System.Drawing.Size(865, 791)
        Me.tlpMiscIO.TabIndex = 24
        '
        'cbProgress
        '
        Me.cbProgress.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMiscIO.SetColumnSpan(Me.cbProgress, 3)
        Me.cbProgress.Location = New System.Drawing.Point(18, 5)
        Me.cbProgress.Name = "cbProgress"
        Me.cbProgress.Size = New System.Drawing.Size(844, 70)
        Me.cbProgress.TabIndex = 12
        Me.cbProgress.Text = "Show Progress"
        '
        'cbLevel
        '
        Me.cbLevel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.tlpMiscIO.SetColumnSpan(Me.cbLevel, 2)
        Me.cbLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLevel.Location = New System.Drawing.Point(218, 577)
        Me.cbLevel.MaxDropDownItems = 50
        Me.cbLevel.Name = "cbLevel"
        Me.cbLevel.Size = New System.Drawing.Size(300, 56)
        Me.cbLevel.TabIndex = 21
        '
        'lLevel
        '
        Me.lLevel.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lLevel.Location = New System.Drawing.Point(18, 565)
        Me.lLevel.Name = "lLevel"
        Me.lLevel.Size = New System.Drawing.Size(194, 70)
        Me.lLevel.TabIndex = 18
        Me.lLevel.Text = "Level:"
        Me.lLevel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbBlurayCompat
        '
        Me.cbBlurayCompat.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMiscIO.SetColumnSpan(Me.cbBlurayCompat, 3)
        Me.cbBlurayCompat.Location = New System.Drawing.Point(18, 405)
        Me.cbBlurayCompat.Name = "cbBlurayCompat"
        Me.cbBlurayCompat.Size = New System.Drawing.Size(844, 70)
        Me.cbBlurayCompat.TabIndex = 22
        Me.cbBlurayCompat.Text = "Blu-ray Compatibility"
        Me.cbBlurayCompat.UseVisualStyleBackColor = True
        '
        'cbThreadInput
        '
        Me.cbThreadInput.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMiscIO.SetColumnSpan(Me.cbThreadInput, 3)
        Me.cbThreadInput.Location = New System.Drawing.Point(18, 85)
        Me.cbThreadInput.Name = "cbThreadInput"
        Me.cbThreadInput.Size = New System.Drawing.Size(844, 70)
        Me.cbThreadInput.TabIndex = 13
        Me.cbThreadInput.Text = "Thread Input"
        '
        'cbAud
        '
        Me.cbAud.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMiscIO.SetColumnSpan(Me.cbAud, 3)
        Me.cbAud.Location = New System.Drawing.Point(18, 325)
        Me.cbAud.Name = "cbAud"
        Me.cbAud.Size = New System.Drawing.Size(844, 70)
        Me.cbAud.TabIndex = 16
        Me.cbAud.Text = "Use access unit delimiters"
        '
        'nudThreads
        '
        Me.nudThreads.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.nudThreads.Location = New System.Drawing.Point(218, 485)
        Me.nudThreads.Maximum = New Decimal(New Integer() {64, 0, 0, 0})
        Me.nudThreads.Name = "nudThreads"
        Me.nudThreads.Size = New System.Drawing.Size(200, 70)
        Me.nudThreads.TabIndex = 20
        '
        'lThreads
        '
        Me.lThreads.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lThreads.Location = New System.Drawing.Point(18, 485)
        Me.lThreads.Name = "lThreads"
        Me.lThreads.Size = New System.Drawing.Size(194, 70)
        Me.lThreads.TabIndex = 17
        Me.lThreads.Text = "Threads:"
        Me.lThreads.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbSSIM
        '
        Me.cbSSIM.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMiscIO.SetColumnSpan(Me.cbSSIM, 3)
        Me.cbSSIM.Location = New System.Drawing.Point(18, 165)
        Me.cbSSIM.Name = "cbSSIM"
        Me.cbSSIM.Size = New System.Drawing.Size(844, 70)
        Me.cbSSIM.TabIndex = 14
        Me.cbSSIM.Text = "SSIM Computation"
        '
        'cbPSNR
        '
        Me.cbPSNR.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMiscIO.SetColumnSpan(Me.cbPSNR, 3)
        Me.cbPSNR.Location = New System.Drawing.Point(18, 245)
        Me.cbPSNR.Name = "cbPSNR"
        Me.cbPSNR.Size = New System.Drawing.Size(844, 70)
        Me.cbPSNR.TabIndex = 15
        Me.cbPSNR.Text = "PSNR Computation"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.tlpVUIControls)
        Me.GroupBox6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox6.Location = New System.Drawing.Point(886, 3)
        Me.GroupBox6.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(871, 845)
        Me.GroupBox6.TabIndex = 1
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Video Usability Info"
        '
        'tlpVUIControls
        '
        Me.tlpVUIControls.ColumnCount = 3
        Me.tlpVUIControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280.0!))
        Me.tlpVUIControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280.0!))
        Me.tlpVUIControls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpVUIControls.Controls.Add(Me.lNalHrd, 0, 0)
        Me.tlpVUIControls.Controls.Add(Me.cbPicStruct, 2, 0)
        Me.tlpVUIControls.Controls.Add(Me.nudChromaloc, 1, 7)
        Me.tlpVUIControls.Controls.Add(Me.lChromaloc, 0, 7)
        Me.tlpVUIControls.Controls.Add(Me.cbNalHrd, 1, 0)
        Me.tlpVUIControls.Controls.Add(Me.lColorprim, 0, 1)
        Me.tlpVUIControls.Controls.Add(Me.cbColorprim, 1, 1)
        Me.tlpVUIControls.Controls.Add(Me.cbFullrange, 1, 6)
        Me.tlpVUIControls.Controls.Add(Me.lFullrange, 0, 6)
        Me.tlpVUIControls.Controls.Add(Me.cbTransfer, 1, 3)
        Me.tlpVUIControls.Controls.Add(Me.lTransfer, 0, 3)
        Me.tlpVUIControls.Controls.Add(Me.cbOverscan, 1, 5)
        Me.tlpVUIControls.Controls.Add(Me.lOverscan, 0, 5)
        Me.tlpVUIControls.Controls.Add(Me.cbVideoformat, 1, 4)
        Me.tlpVUIControls.Controls.Add(Me.lVideoformat, 0, 4)
        Me.tlpVUIControls.Controls.Add(Me.cbColormatrix, 1, 2)
        Me.tlpVUIControls.Controls.Add(Me.lColormatrix, 0, 2)
        Me.tlpVUIControls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpVUIControls.Location = New System.Drawing.Point(3, 51)
        Me.tlpVUIControls.Name = "tlpVUIControls"
        Me.tlpVUIControls.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.tlpVUIControls.RowCount = 10
        Me.tlpVUIControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpVUIControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpVUIControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpVUIControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpVUIControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpVUIControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpVUIControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpVUIControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpVUIControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpVUIControls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpVUIControls.Size = New System.Drawing.Size(865, 791)
        Me.tlpVUIControls.TabIndex = 38
        '
        'lNalHrd
        '
        Me.lNalHrd.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lNalHrd.Location = New System.Drawing.Point(13, 5)
        Me.lNalHrd.Name = "lNalHrd"
        Me.lNalHrd.Size = New System.Drawing.Size(274, 70)
        Me.lNalHrd.TabIndex = 19
        Me.lNalHrd.Text = "HRD Info:"
        Me.lNalHrd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbPicStruct
        '
        Me.cbPicStruct.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbPicStruct.Location = New System.Drawing.Point(580, 3)
        Me.cbPicStruct.Margin = New System.Windows.Forms.Padding(10, 3, 3, 3)
        Me.cbPicStruct.Size = New System.Drawing.Size(282, 70)
        Me.cbPicStruct.Text = "Pic Struct"
        Me.cbPicStruct.UseVisualStyleBackColor = True
        '
        'nudChromaloc
        '
        Me.nudChromaloc.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudChromaloc.Location = New System.Drawing.Point(293, 565)
        Me.nudChromaloc.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.nudChromaloc.Name = "nudChromaloc"
        Me.nudChromaloc.Size = New System.Drawing.Size(274, 70)
        Me.nudChromaloc.TabIndex = 36
        '
        'lChromaloc
        '
        Me.lChromaloc.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lChromaloc.Location = New System.Drawing.Point(13, 565)
        Me.lChromaloc.Name = "lChromaloc"
        Me.lChromaloc.Size = New System.Drawing.Size(274, 70)
        Me.lChromaloc.TabIndex = 35
        Me.lChromaloc.Text = "Chromaloc:"
        Me.lChromaloc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbNalHrd
        '
        Me.cbNalHrd.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbNalHrd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbNalHrd.Location = New System.Drawing.Point(293, 12)
        Me.cbNalHrd.MaxDropDownItems = 50
        Me.cbNalHrd.Name = "cbNalHrd"
        Me.cbNalHrd.Size = New System.Drawing.Size(274, 56)
        Me.cbNalHrd.TabIndex = 22
        '
        'lColorprim
        '
        Me.lColorprim.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lColorprim.Location = New System.Drawing.Point(13, 85)
        Me.lColorprim.Name = "lColorprim"
        Me.lColorprim.Size = New System.Drawing.Size(274, 70)
        Me.lColorprim.TabIndex = 28
        Me.lColorprim.Text = "Colorprim:"
        Me.lColorprim.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbColorprim
        '
        Me.cbColorprim.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbColorprim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbColorprim.Location = New System.Drawing.Point(293, 92)
        Me.cbColorprim.MaxDropDownItems = 50
        Me.cbColorprim.Name = "cbColorprim"
        Me.cbColorprim.Size = New System.Drawing.Size(274, 56)
        Me.cbColorprim.TabIndex = 29
        '
        'cbFullrange
        '
        Me.cbFullrange.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbFullrange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbFullrange.Location = New System.Drawing.Point(293, 492)
        Me.cbFullrange.MaxDropDownItems = 50
        Me.cbFullrange.Name = "cbFullrange"
        Me.cbFullrange.Size = New System.Drawing.Size(274, 56)
        Me.cbFullrange.TabIndex = 27
        '
        'lFullrange
        '
        Me.lFullrange.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lFullrange.Location = New System.Drawing.Point(13, 485)
        Me.lFullrange.Name = "lFullrange"
        Me.lFullrange.Size = New System.Drawing.Size(274, 70)
        Me.lFullrange.TabIndex = 26
        Me.lFullrange.Text = "Fullrange:"
        Me.lFullrange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbTransfer
        '
        Me.cbTransfer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbTransfer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTransfer.Location = New System.Drawing.Point(293, 252)
        Me.cbTransfer.MaxDropDownItems = 50
        Me.cbTransfer.Name = "cbTransfer"
        Me.cbTransfer.Size = New System.Drawing.Size(274, 56)
        Me.cbTransfer.TabIndex = 31
        '
        'lTransfer
        '
        Me.lTransfer.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lTransfer.Location = New System.Drawing.Point(13, 245)
        Me.lTransfer.Name = "lTransfer"
        Me.lTransfer.Size = New System.Drawing.Size(274, 70)
        Me.lTransfer.TabIndex = 30
        Me.lTransfer.Text = "Transfer:"
        Me.lTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbOverscan
        '
        Me.cbOverscan.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbOverscan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbOverscan.Location = New System.Drawing.Point(293, 412)
        Me.cbOverscan.MaxDropDownItems = 50
        Me.cbOverscan.Name = "cbOverscan"
        Me.cbOverscan.Size = New System.Drawing.Size(274, 56)
        Me.cbOverscan.TabIndex = 23
        '
        'lOverscan
        '
        Me.lOverscan.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lOverscan.Location = New System.Drawing.Point(13, 405)
        Me.lOverscan.Name = "lOverscan"
        Me.lOverscan.Size = New System.Drawing.Size(274, 70)
        Me.lOverscan.TabIndex = 22
        Me.lOverscan.Text = "Overscan:"
        Me.lOverscan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbVideoformat
        '
        Me.cbVideoformat.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbVideoformat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbVideoformat.Location = New System.Drawing.Point(293, 332)
        Me.cbVideoformat.MaxDropDownItems = 50
        Me.cbVideoformat.Name = "cbVideoformat"
        Me.cbVideoformat.Size = New System.Drawing.Size(274, 56)
        Me.cbVideoformat.TabIndex = 25
        '
        'lVideoformat
        '
        Me.lVideoformat.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lVideoformat.Location = New System.Drawing.Point(13, 325)
        Me.lVideoformat.Name = "lVideoformat"
        Me.lVideoformat.Size = New System.Drawing.Size(274, 70)
        Me.lVideoformat.TabIndex = 24
        Me.lVideoformat.Text = "Videoformat:"
        Me.lVideoformat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbColormatrix
        '
        Me.cbColormatrix.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbColormatrix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbColormatrix.Location = New System.Drawing.Point(293, 172)
        Me.cbColormatrix.MaxDropDownItems = 50
        Me.cbColormatrix.Name = "cbColormatrix"
        Me.cbColormatrix.Size = New System.Drawing.Size(274, 56)
        Me.cbColormatrix.TabIndex = 33
        '
        'lColormatrix
        '
        Me.lColormatrix.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lColormatrix.Location = New System.Drawing.Point(13, 165)
        Me.lColormatrix.Name = "lColormatrix"
        Me.lColormatrix.Size = New System.Drawing.Size(274, 70)
        Me.lColormatrix.TabIndex = 32
        Me.lColormatrix.Text = "Colormatrix:"
        Me.lColormatrix.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tpCommandLine
        '
        Me.tpCommandLine.Controls.Add(Me.tlpCommandLine)
        Me.tpCommandLine.Location = New System.Drawing.Point(12, 69)
        Me.tpCommandLine.Name = "tpCommandLine"
        Me.tpCommandLine.Size = New System.Drawing.Size(1760, 851)
        Me.tpCommandLine.TabIndex = 5
        Me.tpCommandLine.Text = " Command Line "
        Me.tpCommandLine.UseVisualStyleBackColor = True
        '
        'tlpCommandLine
        '
        Me.tlpCommandLine.ColumnCount = 1
        Me.tlpCommandLine.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpCommandLine.Controls.Add(Me.gbAddToAll, 0, 0)
        Me.tlpCommandLine.Controls.Add(Me.gbAddToToPrecedingPasses, 0, 2)
        Me.tlpCommandLine.Controls.Add(Me.gbRemoveFromPrecedingPasses, 0, 1)
        Me.tlpCommandLine.Controls.Add(Me.FlowLayoutPanel1, 0, 3)
        Me.tlpCommandLine.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpCommandLine.Location = New System.Drawing.Point(0, 0)
        Me.tlpCommandLine.Margin = New System.Windows.Forms.Padding(5)
        Me.tlpCommandLine.Name = "tlpCommandLine"
        Me.tlpCommandLine.RowCount = 4
        Me.tlpCommandLine.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpCommandLine.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpCommandLine.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.tlpCommandLine.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpCommandLine.Size = New System.Drawing.Size(1760, 851)
        Me.tlpCommandLine.TabIndex = 0
        '
        'gbAddToAll
        '
        Me.gbAddToAll.Controls.Add(Me.AddCmdlControl)
        Me.gbAddToAll.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbAddToAll.Location = New System.Drawing.Point(10, 0)
        Me.gbAddToAll.Margin = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.gbAddToAll.Name = "gbAddToAll"
        Me.gbAddToAll.Size = New System.Drawing.Size(1740, 257)
        Me.gbAddToAll.TabIndex = 0
        Me.gbAddToAll.TabStop = False
        Me.gbAddToAll.Text = "Custom Switches (used in all modes and passes)"
        '
        'AddCmdlControl
        '
        Me.AddCmdlControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AddCmdlControl.Location = New System.Drawing.Point(3, 51)
        Me.AddCmdlControl.Margin = New System.Windows.Forms.Padding(0)
        Me.AddCmdlControl.Name = "AddCmdlControl"
        Me.AddCmdlControl.Padding = New System.Windows.Forms.Padding(10, 0, 10, 10)
        Me.AddCmdlControl.Size = New System.Drawing.Size(1734, 203)
        Me.AddCmdlControl.TabIndex = 0
        '
        'gbAddToToPrecedingPasses
        '
        Me.gbAddToToPrecedingPasses.Controls.Add(Me.AddTurboCmdlControl)
        Me.gbAddToToPrecedingPasses.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbAddToToPrecedingPasses.Location = New System.Drawing.Point(10, 514)
        Me.gbAddToToPrecedingPasses.Margin = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.gbAddToToPrecedingPasses.Name = "gbAddToToPrecedingPasses"
        Me.gbAddToToPrecedingPasses.Size = New System.Drawing.Size(1740, 257)
        Me.gbAddToToPrecedingPasses.TabIndex = 2
        Me.gbAddToToPrecedingPasses.TabStop = False
        Me.gbAddToToPrecedingPasses.Text = "Add to to preceding passes:"
        '
        'AddTurboCmdlControl
        '
        Me.AddTurboCmdlControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AddTurboCmdlControl.Location = New System.Drawing.Point(3, 51)
        Me.AddTurboCmdlControl.Margin = New System.Windows.Forms.Padding(0)
        Me.AddTurboCmdlControl.Name = "AddTurboCmdlControl"
        Me.AddTurboCmdlControl.Padding = New System.Windows.Forms.Padding(10, 0, 10, 10)
        Me.AddTurboCmdlControl.Size = New System.Drawing.Size(1734, 203)
        Me.AddTurboCmdlControl.TabIndex = 0
        '
        'gbRemoveFromPrecedingPasses
        '
        Me.gbRemoveFromPrecedingPasses.Controls.Add(Me.RemoveTurboCmdlControl)
        Me.gbRemoveFromPrecedingPasses.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbRemoveFromPrecedingPasses.Location = New System.Drawing.Point(10, 257)
        Me.gbRemoveFromPrecedingPasses.Margin = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.gbRemoveFromPrecedingPasses.Name = "gbRemoveFromPrecedingPasses"
        Me.gbRemoveFromPrecedingPasses.Size = New System.Drawing.Size(1740, 257)
        Me.gbRemoveFromPrecedingPasses.TabIndex = 1
        Me.gbRemoveFromPrecedingPasses.TabStop = False
        Me.gbRemoveFromPrecedingPasses.Text = "Remove from preceding passes:"
        '
        'RemoveTurboCmdlControl
        '
        Me.RemoveTurboCmdlControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RemoveTurboCmdlControl.Location = New System.Drawing.Point(3, 51)
        Me.RemoveTurboCmdlControl.Margin = New System.Windows.Forms.Padding(0)
        Me.RemoveTurboCmdlControl.Name = "RemoveTurboCmdlControl"
        Me.RemoveTurboCmdlControl.Padding = New System.Windows.Forms.Padding(10, 0, 10, 10)
        Me.RemoveTurboCmdlControl.Size = New System.Drawing.Size(1734, 203)
        Me.RemoveTurboCmdlControl.TabIndex = 0
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Controls.Add(Me.buImport)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(5, 776)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(5)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(802, 68)
        Me.FlowLayoutPanel1.TabIndex = 3
        '
        'buImport
        '
        Me.buImport.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.buImport.AutoSize = True
        Me.buImport.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.buImport.Location = New System.Drawing.Point(5, 5)
        Me.buImport.Margin = New System.Windows.Forms.Padding(5)
        Me.buImport.Size = New System.Drawing.Size(792, 58)
        Me.buImport.Text = "Import settings from command line in clipboard"
        '
        'tpOther
        '
        Me.tpOther.Controls.Add(Me.gbCompressibilityCheck)
        Me.tpOther.Location = New System.Drawing.Point(12, 69)
        Me.tpOther.Name = "tpOther"
        Me.tpOther.Size = New System.Drawing.Size(1760, 851)
        Me.tpOther.TabIndex = 6
        Me.tpOther.Text = "  Other  "
        Me.tpOther.UseVisualStyleBackColor = True
        '
        'gbCompressibilityCheck
        '
        Me.gbCompressibilityCheck.Controls.Add(Me.tlpOther)
        Me.gbCompressibilityCheck.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbCompressibilityCheck.Location = New System.Drawing.Point(0, 0)
        Me.gbCompressibilityCheck.Name = "gbCompressibilityCheck"
        Me.gbCompressibilityCheck.Size = New System.Drawing.Size(1760, 851)
        Me.gbCompressibilityCheck.TabIndex = 0
        Me.gbCompressibilityCheck.TabStop = False
        Me.gbCompressibilityCheck.Text = "Compressibility Check"
        '
        'tlpOther
        '
        Me.tlpOther.ColumnCount = 4
        Me.tlpOther.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.tlpOther.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 500.0!))
        Me.tlpOther.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.tlpOther.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.0!))
        Me.tlpOther.Controls.Add(Me.lAimedQuality, 1, 1)
        Me.tlpOther.Controls.Add(Me.lAimedQualityHint, 3, 1)
        Me.tlpOther.Controls.Add(Me.nudQPCompCheck, 2, 2)
        Me.tlpOther.Controls.Add(Me.lCRFValueDefining100Quality, 1, 2)
        Me.tlpOther.Controls.Add(Me.nudPercent, 2, 1)
        Me.tlpOther.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpOther.Location = New System.Drawing.Point(3, 51)
        Me.tlpOther.Name = "tlpOther"
        Me.tlpOther.RowCount = 4
        Me.tlpOther.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpOther.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpOther.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpOther.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpOther.Size = New System.Drawing.Size(1754, 797)
        Me.tlpOther.TabIndex = 5
        '
        'lAimedQualityHint
        '
        Me.lAimedQualityHint.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lAimedQualityHint.Location = New System.Drawing.Point(1019, 323)
        Me.lAimedQualityHint.Name = "lAimedQualityHint"
        Me.lAimedQualityHint.Size = New System.Drawing.Size(732, 70)
        Me.lAimedQualityHint.TabIndex = 4
        Me.lAimedQualityHint.Text = "(hint)"
        Me.lAimedQualityHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbGoTo
        '
        Me.cbGoTo.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cbGoTo.FormattingEnabled = True
        Me.cbGoTo.Location = New System.Drawing.Point(0, 13)
        Me.cbGoTo.Margin = New System.Windows.Forms.Padding(0)
        Me.cbGoTo.Name = "cbGoTo"
        Me.cbGoTo.Size = New System.Drawing.Size(400, 56)
        Me.cbGoTo.TabIndex = 3
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnMenu.Location = New System.Drawing.Point(1184, 1)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(0, 0, 15, 0)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(70, 70)
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(1534, 1)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(15, 0, 0, 0)
        Me.bnCancel.Size = New System.Drawing.Size(250, 70)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(1269, 1)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(0)
        Me.bnOK.Size = New System.Drawing.Size(250, 70)
        Me.bnOK.Text = "OK"
        '
        'tlpBottom
        '
        Me.tlpBottom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpBottom.ColumnCount = 4
        Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpBottom.Controls.Add(Me.cbGoTo, 0, 0)
        Me.tlpBottom.Controls.Add(Me.bnCancel, 3, 0)
        Me.tlpBottom.Controls.Add(Me.bnMenu, 1, 0)
        Me.tlpBottom.Controls.Add(Me.bnOK, 2, 0)
        Me.tlpBottom.Location = New System.Drawing.Point(15, 1037)
        Me.tlpBottom.Margin = New System.Windows.Forms.Padding(15)
        Me.tlpBottom.Name = "tlpBottom"
        Me.tlpBottom.RowCount = 1
        Me.tlpBottom.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBottom.Size = New System.Drawing.Size(1784, 72)
        Me.tlpBottom.TabIndex = 4
        '
        'tlpMain
        '
        Me.tlpMain.AutoSize = True
        Me.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.tcMain, 0, 0)
        Me.tlpMain.Controls.Add(Me.tlpBottom, 0, 2)
        Me.tlpMain.Controls.Add(Me.tlpRtbCommandLineWrapper, 0, 1)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1814, 1124)
        Me.tlpMain.TabIndex = 5
        '
        'tlpRtbCommandLineWrapper
        '
        Me.tlpRtbCommandLineWrapper.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpRtbCommandLineWrapper.AutoSize = True
        Me.tlpRtbCommandLineWrapper.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpRtbCommandLineWrapper.ColumnCount = 1
        Me.tlpRtbCommandLineWrapper.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpRtbCommandLineWrapper.Controls.Add(Me.rtbCommandLine, 0, 0)
        Me.tlpRtbCommandLineWrapper.Location = New System.Drawing.Point(15, 962)
        Me.tlpRtbCommandLineWrapper.Margin = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.tlpRtbCommandLineWrapper.Name = "tlpRtbCommandLineWrapper"
        Me.tlpRtbCommandLineWrapper.RowCount = 1
        Me.tlpRtbCommandLineWrapper.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpRtbCommandLineWrapper.Size = New System.Drawing.Size(1784, 60)
        Me.tlpRtbCommandLineWrapper.TabIndex = 5
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
        Me.rtbCommandLine.Size = New System.Drawing.Size(1784, 60)
        Me.rtbCommandLine.TabIndex = 9
        Me.rtbCommandLine.Text = ""
        '
        'x264Form
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1814, 1124)
        Me.Controls.Add(Me.tlpMain)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "x264Form"
        Me.Text = "x264"
        Me.tcMain.ResumeLayout(False)
        Me.tpBasic.ResumeLayout(False)
        Me.tlpBasicHost.ResumeLayout(False)
        Me.tlpBasicControls.ResumeLayout(False)
        Me.tpAnalysis.ResumeLayout(False)
        Me.tlpAnalysis.ResumeLayout(False)
        Me.gbQuantOptions.ResumeLayout(False)
        Me.tlpQuantOptionsControls.ResumeLayout(False)
        Me.tlpPsy.ResumeLayout(False)
        Me.tlpPsy.PerformLayout()
        Me.gbPartitions.ResumeLayout(False)
        Me.tlpPartitions.ResumeLayout(False)
        Me.gbMotionEstimation.ResumeLayout(False)
        Me.tlpMotionEstimationControls.ResumeLayout(False)
        Me.tlpMe.ResumeLayout(False)
        Me.gbAnalysisMisc.ResumeLayout(False)
        Me.tlpAnalysisMiscControls.ResumeLayout(False)
        Me.tpFrameOptions.ResumeLayout(False)
        Me.tlpFrameOptions.ResumeLayout(False)
        Me.gbBFrames.ResumeLayout(False)
        Me.tlpBframeControls.ResumeLayout(False)
        Me.gbFrameOptionsRight.ResumeLayout(False)
        Me.tlpFrameOptionRightHost.ResumeLayout(False)
        Me.tlpFrameOptionsRightTopControls.ResumeLayout(False)
        Me.tlpDeblockingHost.ResumeLayout(False)
        Me.tlpDeblockingControls.ResumeLayout(False)
        Me.tpRateControl.ResumeLayout(False)
        Me.tlpRateControl.ResumeLayout(False)
        Me.gbRC2.ResumeLayout(False)
        Me.tlpRateControlRightControls.ResumeLayout(False)
        Me.gbRC1.ResumeLayout(False)
        Me.tlpRateControlLeftControls.ResumeLayout(False)
        Me.tpMisc.ResumeLayout(False)
        Me.tlpMisc.ResumeLayout(False)
        Me.gbIO.ResumeLayout(False)
        Me.tlpMiscIO.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.tlpVUIControls.ResumeLayout(False)
        Me.tpCommandLine.ResumeLayout(False)
        Me.tlpCommandLine.ResumeLayout(False)
        Me.tlpCommandLine.PerformLayout()
        Me.gbAddToAll.ResumeLayout(False)
        Me.gbAddToToPrecedingPasses.ResumeLayout(False)
        Me.gbRemoveFromPrecedingPasses.ResumeLayout(False)
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.tpOther.ResumeLayout(False)
        Me.gbCompressibilityCheck.ResumeLayout(False)
        Me.tlpOther.ResumeLayout(False)
        Me.tlpBottom.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.tlpRtbCommandLineWrapper.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

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

        Dim cms As New ContextMenuStripEx(components)
        bnMenu.ContextMenuStrip = cms
        cms.Add("Save Profile...", Sub() SaveProfile(), "Saves the current x264 settings to a encoder profile").SetImage(Symbol.Save)
        cms.Add("Help", AddressOf ShowHelp, "Saves the current x264 settings to a encoder profile").SetImage(Symbol.Help)

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
            l.Add("cabac requires a higher profile than baseline, either select a higher profile or disable cabac")
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

        tlpDeblockingControls.Enabled = cbDeblock.Checked

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

            If s.ShowPathsInCommandLine Then cmdl = Package.x264.Path.Quotes + " " + cmdl
            rtbCommandLine.SetText(cmdl)
            rtbCommandLine.SelectionLength = 0
        End If

        rtbCommandLine.UpdateHeight()
    End Sub

    Function GetEnum(Of T)(o As Object) As T
        Dim ret As Object = DirectCast(o, ListBag(Of Integer)).Value
        Return CType(ret, T)
    End Function

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

    Private Sub rtbCommandLine_MouseDown(sender As Object, e As MouseEventArgs) Handles rtbCommandLine.MouseDown
        If Not rtbCommandLine.ContextMenuStrip Is Nothing Then rtbCommandLine.ContextMenuStrip.Dispose()
        rtbCommandLine.ContextMenuStrip = GetCommandLineMenu()
    End Sub

    Private Sub buImport_Click() Handles buImport.Click
        ImportCommandLine()
    End Sub

    Sub ShowHelp()
        Dim f As New HelpForm
        f.Doc.WriteStart("x264 Configuration")
        f.Doc.WriteH2("Guides")
        f.Doc.WriteH2("English Guides")
        f.Doc.WriteList("[http://www.avidemux.org/admWiki/index.php?title=H264 guide for AviDemux]",
                        "[http://www.digital-digest.com/articles/x264_options_page1.html guide by DVDGuy]",
                        "[http://www.mplayerhq.hu/DOCS/HTML/en/menc-feat-x264.html guide for mencoder]",
                        "[http://mewiki.project357.com/wiki/X264_Settings switches at MeGUI wiki]")
        f.Show()
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        LoadParams(Encoder.Params)
        rtbCommandLine.UpdateHeight()
        UpdateSearchComboBox()
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        cbGoTo.Focus()
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)

        ToolTip.Dispose()
        s.CmdlPresetsX264 = AddCmdlControl.Presets.ReplaceUnicode

        If DialogResult = DialogResult.OK Then
            Encoder.Params = Params
            Encoder.AutoCompCheckValue = CInt(nudPercent.Value)
            Encoder.Name = NameOfLastProfile
        End If

        s.Storage.SetInt("x264 tab", tcMain.SelectedIndex)
    End Sub

    Protected Overrides Sub OnHelpRequested(hevent As HelpEventArgs)
        MyBase.OnHelpRequested(hevent)
        ShowHelp()
    End Sub
End Class