Imports System.ComponentModel
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports StaxRip.UI

Class eac3toForm
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
    Friend WithEvents cbVideoOutput As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmdlOptions As StaxRip.CommandLineControl
    Friend WithEvents bnBrowse As StaxRip.UI.ButtonEx
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents lvAudio As ListViewEx
    Friend WithEvents lvSubtitles As StaxRip.UI.ListViewEx
    Friend WithEvents cbVideoStream As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents flpAudioLinks As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents flpSubtitleLinks As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cbChapters As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tlpTarget As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents gbAudio As System.Windows.Forms.GroupBox
    Friend WithEvents gbSubtitles As System.Windows.Forms.GroupBox
    Friend WithEvents cbAudioOutput As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents bnMenu As StaxRip.UI.ButtonEx
    Friend WithEvents cms As ContextMenuStripEx
    Friend WithEvents bnAudioAll As ButtonEx
    Friend WithEvents bnAudioNone As ButtonEx
    Friend WithEvents bnAudioEnglish As ButtonEx
    Friend WithEvents bnAudioNative As ButtonEx
    Friend WithEvents bnSubtitleAll As ButtonEx
    Friend WithEvents bnSubtitleNone As ButtonEx
    Friend WithEvents bnSubtitleEnglish As ButtonEx
    Friend WithEvents bnSubtitleNative As ButtonEx
    Friend WithEvents TableLayoutPanelVideo As TableLayoutPanel
    Friend WithEvents tlpBottom As TableLayoutPanel
    Friend WithEvents tlpAudioOptions As TableLayoutPanel
    Friend WithEvents tlpAudio As TableLayoutPanel
    Friend WithEvents tlpSubtitles As TableLayoutPanel
    Friend WithEvents teTempDir As TextEdit
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.cmdlOptions = New StaxRip.CommandLineControl()
        Me.cbVideoOutput = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.bnBrowse = New StaxRip.UI.ButtonEx()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.lvAudio = New StaxRip.UI.ListViewEx()
        Me.lvSubtitles = New StaxRip.UI.ListViewEx()
        Me.flpSubtitleLinks = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnSubtitleAll = New StaxRip.UI.ButtonEx()
        Me.bnSubtitleNone = New StaxRip.UI.ButtonEx()
        Me.bnSubtitleEnglish = New StaxRip.UI.ButtonEx()
        Me.bnSubtitleNative = New StaxRip.UI.ButtonEx()
        Me.flpAudioLinks = New System.Windows.Forms.FlowLayoutPanel()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnAudioAll = New StaxRip.UI.ButtonEx()
        Me.bnAudioNone = New StaxRip.UI.ButtonEx()
        Me.bnAudioEnglish = New StaxRip.UI.ButtonEx()
        Me.bnAudioNative = New StaxRip.UI.ButtonEx()
        Me.cbVideoStream = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.tlpTarget = New System.Windows.Forms.TableLayoutPanel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.teTempDir = New StaxRip.UI.TextEdit()
        Me.tlpBottom = New System.Windows.Forms.TableLayoutPanel()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
        Me.cms = New ContextMenuStripEx(Me.components)
        Me.cbChapters = New System.Windows.Forms.CheckBox()
        Me.gbAudio = New System.Windows.Forms.GroupBox()
        Me.tlpAudio = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpAudioOptions = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cbAudioOutput = New System.Windows.Forms.ComboBox()
        Me.gbSubtitles = New System.Windows.Forms.GroupBox()
        Me.tlpSubtitles = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanelVideo = New System.Windows.Forms.TableLayoutPanel()
        Me.flpSubtitleLinks.SuspendLayout()
        Me.flpAudioLinks.SuspendLayout()
        Me.tlpTarget.SuspendLayout()
        Me.tlpBottom.SuspendLayout()
        Me.gbAudio.SuspendLayout()
        Me.tlpAudio.SuspendLayout()
        Me.tlpAudioOptions.SuspendLayout()
        Me.gbSubtitles.SuspendLayout()
        Me.tlpSubtitles.SuspendLayout()
        Me.TableLayoutPanelVideo.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdlOptions
        '
        Me.cmdlOptions.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdlOptions.Location = New System.Drawing.Point(268, 2)
        Me.cmdlOptions.Margin = New System.Windows.Forms.Padding(0)
        Me.cmdlOptions.Name = "cmdlOptions"
        Me.cmdlOptions.Size = New System.Drawing.Size(710, 35)
        Me.cmdlOptions.TabIndex = 5
        '
        'cbVideoOutput
        '
        Me.cbVideoOutput.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cbVideoOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbVideoOutput.FormattingEnabled = True
        Me.cbVideoOutput.Location = New System.Drawing.Point(68, 6)
        Me.cbVideoOutput.Margin = New System.Windows.Forms.Padding(0)
        Me.cbVideoOutput.Name = "cbVideoOutput"
        Me.cbVideoOutput.Size = New System.Drawing.Size(115, 33)
        Me.cbVideoOutput.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 7)
        Me.Label5.Margin = New System.Windows.Forms.Padding(6, 6, 0, 6)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(62, 25)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Video:"
        '
        'bnBrowse
        '
        Me.bnBrowse.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnBrowse.Location = New System.Drawing.Point(953, 5)
        Me.bnBrowse.Margin = New System.Windows.Forms.Padding(0)
        Me.bnBrowse.Size = New System.Drawing.Size(35, 35)
        Me.bnBrowse.Text = "..."
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(888, 4)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.bnCancel.Size = New System.Drawing.Size(100, 35)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(782, 4)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.bnOK.Size = New System.Drawing.Size(100, 35)
        Me.bnOK.Text = "OK"
        '
        'lvAudio
        '
        Me.lvAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvAudio.Location = New System.Drawing.Point(6, 6)
        Me.lvAudio.Margin = New System.Windows.Forms.Padding(6)
        Me.lvAudio.Name = "lvAudio"
        Me.lvAudio.Size = New System.Drawing.Size(970, 215)
        Me.lvAudio.TabIndex = 8
        Me.lvAudio.UseCompatibleStateImageBehavior = False
        '
        'lvSubtitles
        '
        Me.lvSubtitles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvSubtitles.Location = New System.Drawing.Point(6, 6)
        Me.lvSubtitles.Margin = New System.Windows.Forms.Padding(6)
        Me.lvSubtitles.Name = "lvSubtitles"
        Me.lvSubtitles.Size = New System.Drawing.Size(970, 134)
        Me.lvSubtitles.TabIndex = 9
        Me.lvSubtitles.UseCompatibleStateImageBehavior = False
        '
        'flpSubtitleLinks
        '
        Me.flpSubtitleLinks.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpSubtitleLinks.AutoSize = True
        Me.flpSubtitleLinks.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpSubtitleLinks.Controls.Add(Me.bnSubtitleAll)
        Me.flpSubtitleLinks.Controls.Add(Me.bnSubtitleNone)
        Me.flpSubtitleLinks.Controls.Add(Me.bnSubtitleEnglish)
        Me.flpSubtitleLinks.Controls.Add(Me.bnSubtitleNative)
        Me.flpSubtitleLinks.Location = New System.Drawing.Point(6, 146)
        Me.flpSubtitleLinks.Margin = New System.Windows.Forms.Padding(6, 0, 0, 6)
        Me.flpSubtitleLinks.Name = "flpSubtitleLinks"
        Me.flpSubtitleLinks.Size = New System.Drawing.Size(418, 37)
        Me.flpSubtitleLinks.TabIndex = 19
        '
        'bnSubtitleAll
        '
        Me.bnSubtitleAll.Location = New System.Drawing.Point(0, 0)
        Me.bnSubtitleAll.Margin = New System.Windows.Forms.Padding(0, 0, 6, 0)
        Me.bnSubtitleAll.Size = New System.Drawing.Size(100, 37)
        Me.bnSubtitleAll.Text = "All"
        '
        'bnSubtitleNone
        '
        Me.bnSubtitleNone.Location = New System.Drawing.Point(106, 0)
        Me.bnSubtitleNone.Margin = New System.Windows.Forms.Padding(0, 0, 6, 0)
        Me.bnSubtitleNone.Size = New System.Drawing.Size(100, 37)
        Me.bnSubtitleNone.Text = "None"
        '
        'bnSubtitleEnglish
        '
        Me.bnSubtitleEnglish.Location = New System.Drawing.Point(212, 0)
        Me.bnSubtitleEnglish.Margin = New System.Windows.Forms.Padding(0, 0, 6, 0)
        Me.bnSubtitleEnglish.Size = New System.Drawing.Size(100, 37)
        Me.bnSubtitleEnglish.Text = "English"
        '
        'bnSubtitleNative
        '
        Me.bnSubtitleNative.Location = New System.Drawing.Point(318, 0)
        Me.bnSubtitleNative.Margin = New System.Windows.Forms.Padding(0)
        Me.bnSubtitleNative.Size = New System.Drawing.Size(100, 37)
        Me.bnSubtitleNative.Text = "Native"
        '
        'flpAudioLinks
        '
        Me.flpAudioLinks.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.flpAudioLinks.AutoSize = True
        Me.flpAudioLinks.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAudioLinks.Controls.Add(Me.FlowLayoutPanel1)
        Me.flpAudioLinks.Controls.Add(Me.bnAudioAll)
        Me.flpAudioLinks.Controls.Add(Me.bnAudioNone)
        Me.flpAudioLinks.Controls.Add(Me.bnAudioEnglish)
        Me.flpAudioLinks.Controls.Add(Me.bnAudioNative)
        Me.flpAudioLinks.Location = New System.Drawing.Point(0, 227)
        Me.flpAudioLinks.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAudioLinks.Name = "flpAudioLinks"
        Me.flpAudioLinks.Size = New System.Drawing.Size(424, 37)
        Me.flpAudioLinks.TabIndex = 18
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 16)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(3, 3, 3, 8)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(0, 0)
        Me.FlowLayoutPanel1.TabIndex = 0
        '
        'bnAudioAll
        '
        Me.bnAudioAll.Location = New System.Drawing.Point(6, 0)
        Me.bnAudioAll.Margin = New System.Windows.Forms.Padding(0, 0, 6, 0)
        Me.bnAudioAll.Size = New System.Drawing.Size(100, 37)
        Me.bnAudioAll.Text = "All"
        '
        'bnAudioNone
        '
        Me.bnAudioNone.Location = New System.Drawing.Point(112, 0)
        Me.bnAudioNone.Margin = New System.Windows.Forms.Padding(0, 0, 6, 0)
        Me.bnAudioNone.Size = New System.Drawing.Size(100, 37)
        Me.bnAudioNone.Text = "None"
        '
        'bnAudioEnglish
        '
        Me.bnAudioEnglish.Location = New System.Drawing.Point(218, 0)
        Me.bnAudioEnglish.Margin = New System.Windows.Forms.Padding(0, 0, 6, 0)
        Me.bnAudioEnglish.Size = New System.Drawing.Size(100, 37)
        Me.bnAudioEnglish.Text = "English"
        '
        'bnAudioNative
        '
        Me.bnAudioNative.Location = New System.Drawing.Point(324, 0)
        Me.bnAudioNative.Margin = New System.Windows.Forms.Padding(0)
        Me.bnAudioNative.Size = New System.Drawing.Size(100, 37)
        Me.bnAudioNative.Text = "Native"
        '
        'cbVideoStream
        '
        Me.cbVideoStream.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbVideoStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbVideoStream.FormattingEnabled = True
        Me.cbVideoStream.Location = New System.Drawing.Point(254, 6)
        Me.cbVideoStream.Margin = New System.Windows.Forms.Padding(0)
        Me.cbVideoStream.Name = "cbVideoStream"
        Me.cbVideoStream.Size = New System.Drawing.Size(734, 33)
        Me.cbVideoStream.TabIndex = 16
        '
        'Label8
        '
        Me.Label8.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(183, 7)
        Me.Label8.Margin = New System.Windows.Forms.Padding(0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(71, 25)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Stream:"
        '
        'tlpTarget
        '
        Me.tlpTarget.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpTarget.ColumnCount = 3
        Me.tlpTarget.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpTarget.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpTarget.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpTarget.Controls.Add(Me.bnBrowse, 2, 0)
        Me.tlpTarget.Controls.Add(Me.Label2, 0, 0)
        Me.tlpTarget.Controls.Add(Me.teTempDir, 1, 0)
        Me.tlpTarget.Location = New System.Drawing.Point(15, 633)
        Me.tlpTarget.Margin = New System.Windows.Forms.Padding(6, 6, 6, 0)
        Me.tlpTarget.Name = "tlpTarget"
        Me.tlpTarget.RowCount = 1
        Me.tlpTarget.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpTarget.Size = New System.Drawing.Size(988, 46)
        Me.tlpTarget.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(0, 10)
        Me.Label2.Margin = New System.Windows.Forms.Padding(0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(141, 25)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Target Directory:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'teTempDir
        '
        Me.teTempDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.teTempDir.Location = New System.Drawing.Point(141, 5)
        Me.teTempDir.Margin = New System.Windows.Forms.Padding(0, 0, 6, 0)
        Me.teTempDir.Name = "teTempDir"
        Me.teTempDir.Size = New System.Drawing.Size(806, 35)
        Me.teTempDir.TabIndex = 3
        '
        'tlpBottom
        '
        Me.tlpBottom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpBottom.ColumnCount = 4
        Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpBottom.Controls.Add(Me.bnCancel, 3, 0)
        Me.tlpBottom.Controls.Add(Me.bnOK, 2, 0)
        Me.tlpBottom.Controls.Add(Me.bnMenu, 1, 0)
        Me.tlpBottom.Controls.Add(Me.cbChapters, 0, 0)
        Me.tlpBottom.Location = New System.Drawing.Point(15, 685)
        Me.tlpBottom.Margin = New System.Windows.Forms.Padding(6)
        Me.tlpBottom.Name = "tlpBottom"
        Me.tlpBottom.RowCount = 1
        Me.tlpBottom.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBottom.Size = New System.Drawing.Size(988, 43)
        Me.tlpBottom.TabIndex = 20
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnMenu.ContextMenuStrip = Me.cms
        Me.bnMenu.Location = New System.Drawing.Point(740, 3)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(0)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(36, 37)
        '
        'cms
        '
        Me.cms.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.cms.Name = "cms"
        Me.cms.Size = New System.Drawing.Size(74, 4)
        '
        'cbChapters
        '
        Me.cbChapters.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cbChapters.AutoSize = True
        Me.cbChapters.Location = New System.Drawing.Point(3, 7)
        Me.cbChapters.Name = "cbChapters"
        Me.cbChapters.Size = New System.Drawing.Size(165, 29)
        Me.cbChapters.TabIndex = 2
        Me.cbChapters.Text = "Extract Chapters"
        Me.cbChapters.UseVisualStyleBackColor = True
        Me.cbChapters.Visible = False
        '
        'gbAudio
        '
        Me.gbAudio.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbAudio.Controls.Add(Me.tlpAudio)
        Me.gbAudio.Location = New System.Drawing.Point(15, 61)
        Me.gbAudio.Margin = New System.Windows.Forms.Padding(6, 0, 6, 6)
        Me.gbAudio.Name = "gbAudio"
        Me.gbAudio.Size = New System.Drawing.Size(988, 338)
        Me.gbAudio.TabIndex = 23
        Me.gbAudio.TabStop = False
        Me.gbAudio.Text = "Audio"
        '
        'tlpAudio
        '
        Me.tlpAudio.ColumnCount = 1
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpAudio.Controls.Add(Me.flpAudioLinks, 0, 1)
        Me.tlpAudio.Controls.Add(Me.lvAudio, 0, 0)
        Me.tlpAudio.Controls.Add(Me.tlpAudioOptions, 0, 2)
        Me.tlpAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAudio.Location = New System.Drawing.Point(3, 27)
        Me.tlpAudio.Margin = New System.Windows.Forms.Padding(2)
        Me.tlpAudio.Name = "tlpAudio"
        Me.tlpAudio.RowCount = 3
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpAudio.Size = New System.Drawing.Size(982, 308)
        Me.tlpAudio.TabIndex = 23
        '
        'tlpAudioOptions
        '
        Me.tlpAudioOptions.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpAudioOptions.ColumnCount = 4
        Me.tlpAudioOptions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudioOptions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudioOptions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudioOptions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpAudioOptions.Controls.Add(Me.Label1, 0, 0)
        Me.tlpAudioOptions.Controls.Add(Me.Label3, 2, 0)
        Me.tlpAudioOptions.Controls.Add(Me.cbAudioOutput, 1, 0)
        Me.tlpAudioOptions.Controls.Add(Me.cmdlOptions, 3, 0)
        Me.tlpAudioOptions.Location = New System.Drawing.Point(2, 266)
        Me.tlpAudioOptions.Margin = New System.Windows.Forms.Padding(2)
        Me.tlpAudioOptions.Name = "tlpAudioOptions"
        Me.tlpAudioOptions.RowCount = 1
        Me.tlpAudioOptions.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpAudioOptions.Size = New System.Drawing.Size(978, 40)
        Me.tlpAudioOptions.TabIndex = 22
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(0, 7)
        Me.Label1.Margin = New System.Windows.Forms.Padding(0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 25)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "Output:"
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(188, 7)
        Me.Label3.Margin = New System.Windows.Forms.Padding(0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 25)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Options:"
        '
        'cbAudioOutput
        '
        Me.cbAudioOutput.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cbAudioOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAudioOutput.FormattingEnabled = True
        Me.cbAudioOutput.Location = New System.Drawing.Point(73, 6)
        Me.cbAudioOutput.Margin = New System.Windows.Forms.Padding(0)
        Me.cbAudioOutput.Name = "cbAudioOutput"
        Me.cbAudioOutput.Size = New System.Drawing.Size(115, 33)
        Me.cbAudioOutput.TabIndex = 21
        '
        'gbSubtitles
        '
        Me.gbSubtitles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbSubtitles.Controls.Add(Me.tlpSubtitles)
        Me.gbSubtitles.Location = New System.Drawing.Point(15, 405)
        Me.gbSubtitles.Margin = New System.Windows.Forms.Padding(6, 0, 6, 6)
        Me.gbSubtitles.Name = "gbSubtitles"
        Me.gbSubtitles.Size = New System.Drawing.Size(988, 219)
        Me.gbSubtitles.TabIndex = 24
        Me.gbSubtitles.TabStop = False
        Me.gbSubtitles.Text = "Subtitles"
        '
        'tlpSubtitles
        '
        Me.tlpSubtitles.ColumnCount = 1
        Me.tlpSubtitles.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpSubtitles.Controls.Add(Me.flpSubtitleLinks, 0, 1)
        Me.tlpSubtitles.Controls.Add(Me.lvSubtitles, 0, 0)
        Me.tlpSubtitles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpSubtitles.Location = New System.Drawing.Point(3, 27)
        Me.tlpSubtitles.Margin = New System.Windows.Forms.Padding(6)
        Me.tlpSubtitles.Name = "tlpSubtitles"
        Me.tlpSubtitles.RowCount = 2
        Me.tlpSubtitles.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpSubtitles.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpSubtitles.Size = New System.Drawing.Size(982, 189)
        Me.tlpSubtitles.TabIndex = 20
        '
        'TableLayoutPanelVideo
        '
        Me.TableLayoutPanelVideo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanelVideo.ColumnCount = 4
        Me.TableLayoutPanelVideo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanelVideo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanelVideo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanelVideo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelVideo.Controls.Add(Me.cbVideoStream, 3, 0)
        Me.TableLayoutPanelVideo.Controls.Add(Me.cbVideoOutput, 1, 0)
        Me.TableLayoutPanelVideo.Controls.Add(Me.Label8, 2, 0)
        Me.TableLayoutPanelVideo.Controls.Add(Me.Label5, 0, 0)
        Me.TableLayoutPanelVideo.Location = New System.Drawing.Point(15, 15)
        Me.TableLayoutPanelVideo.Margin = New System.Windows.Forms.Padding(6)
        Me.TableLayoutPanelVideo.Name = "TableLayoutPanelVideo"
        Me.TableLayoutPanelVideo.RowCount = 1
        Me.TableLayoutPanelVideo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelVideo.Size = New System.Drawing.Size(988, 40)
        Me.TableLayoutPanelVideo.TabIndex = 25
        '
        'eac3toForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1018, 738)
        Me.Controls.Add(Me.tlpBottom)
        Me.Controls.Add(Me.tlpTarget)
        Me.Controls.Add(Me.gbSubtitles)
        Me.Controls.Add(Me.gbAudio)
        Me.Controls.Add(Me.TableLayoutPanelVideo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = False
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(6)
        Me.Name = "eac3toForm"
        Me.Text = "eac3to"
        Me.flpSubtitleLinks.ResumeLayout(False)
        Me.flpAudioLinks.ResumeLayout(False)
        Me.flpAudioLinks.PerformLayout()
        Me.tlpTarget.ResumeLayout(False)
        Me.tlpTarget.PerformLayout()
        Me.tlpBottom.ResumeLayout(False)
        Me.tlpBottom.PerformLayout()
        Me.gbAudio.ResumeLayout(False)
        Me.tlpAudio.ResumeLayout(False)
        Me.tlpAudio.PerformLayout()
        Me.tlpAudioOptions.ResumeLayout(False)
        Me.tlpAudioOptions.PerformLayout()
        Me.gbSubtitles.ResumeLayout(False)
        Me.tlpSubtitles.ResumeLayout(False)
        Me.tlpSubtitles.PerformLayout()
        Me.TableLayoutPanelVideo.ResumeLayout(False)
        Me.TableLayoutPanelVideo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Property M2TSFile As String
    Property PlaylistFolder As String
    Property OutputFolder As String
    Property PlaylistID As Integer

    Private Output As String
    Private Streams As New BindingList(Of M2TSStream)
    Private AudioOutputFormats As String() = {"m4a", "ac3", "dts", "flac", "wav", "dtsma", "dtshr", "eac3", "thd"}

    Sub New()
        MyBase.New()
        InitializeComponent()

        cbAudioOutput.Sorted = True
        cbAudioOutput.Items.AddRange(AudioOutputFormats)

        For Each ctrl As Control In Controls
            ctrl.Enabled = False
        Next

        cbChapters.Checked = s.Storage.GetBool("demux Blu-ray chapters", True)

        lvAudio.View = View.Details
        lvAudio.Columns.Add("")
        lvAudio.CheckBoxes = True
        lvAudio.HeaderStyle = ColumnHeaderStyle.None
        lvAudio.ShowItemToolTips = True
        lvAudio.FullRowSelect = True
        lvAudio.MultiSelect = False
        lvAudio.SendMessageHideFocus()

        lvSubtitles.View = View.SmallIcon
        lvSubtitles.CheckBoxes = True
        lvSubtitles.HeaderStyle = ColumnHeaderStyle.None
        lvSubtitles.AutoCheckMode = AutoCheckMode.SingleClick

        cmdlOptions.Presets = s.CmdlPresetsEac3to
        cmdlOptions.RestoreFunc = Function() ApplicationSettings.GetDefaultEac3toMenu.FormatColumn("=")

        bnAudioNative.Visible = False
        bnAudioEnglish.Visible = False
        bnSubtitleNative.Visible = False
        bnSubtitleEnglish.Visible = False

        If CultureInfo.CurrentCulture.TwoLetterISOLanguageName <> "en" Then
            Try
                bnAudioNative.Text = New CultureInfo(CultureInfo.CurrentCulture.TwoLetterISOLanguageName).EnglishName
                bnSubtitleNative.Text = bnAudioNative.Text
            Catch ex As Exception
                g.ShowException(ex)
                bnAudioNative.Visible = False
                bnSubtitleNative.Visible = False
            End Try
        End If

        cms.Items.Add(New ActionMenuItem("Audio Stream Profiles...", AddressOf ShowAudioStreamProfiles))
        cms.Items.Add(New ActionMenuItem("Show eac3to wikibook", Sub() g.ShellExecute("http://en.wikibooks.org/wiki/Eac3to")))
        cms.Items.Add(New ActionMenuItem("Show eac3to support forum", Sub() g.ShellExecute("http://forum.doom9.org/showthread.php?t=125966")))
        cms.Items.Add(New ActionMenuItem("Execute eac3to.exe -test", Sub() g.ShellExecute("cmd.exe", "/k """ + Package.eac3to.Path + """ -test")))

        ActiveControl = Nothing
    End Sub

    Sub ShowAudioStreamProfilesHelp()
        Dim f As New HelpForm
        f.Doc.WriteStart("Audio Stream Profiles")
        f.Doc.WriteP("Allows to automatically apply default values for audio streams.")
        f.Doc.WriteTable({New StringPair("Match All", "space separated, if all match then the Output Format and Options are applied"),
                          New StringPair("Output Format", "applied to the stream if Match All succeeds"),
                          New StringPair("Options", "applied to the stream if Match All succeeds")})
        f.Show()
    End Sub

    Sub ShowAudioStreamProfiles()
        Using f As New DataForm
            f.Text = "Audio Stream Profiles"
            f.FormBorderStyle = FormBorderStyle.Sizable
            f.dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            f.dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            f.dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            f.dgv.AllowUserToDeleteRows = True
            f.dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            f.HelpAction = AddressOf ShowAudioStreamProfilesHelp

            Dim match = f.dgv.AddTextBoxColumn()
            match.DataPropertyName = "Match"
            match.HeaderText = "Match All"

            Dim out = f.dgv.AddComboBoxColumn()
            out.DataPropertyName = "Output"
            out.HeaderText = "Output Format"
            out.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            out.Items.AddRange(AudioOutputFormats)

            Dim opt = f.dgv.AddTextBoxColumn()
            opt.DataPropertyName = "Options"
            opt.HeaderText = "Options"

            f.dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

            Dim bs As New BindingSource

            bs.DataSource = ObjectHelp.GetCopy(s.eac3toProfiles)
            f.dgv.DataSource = bs

            If f.ShowDialog = DialogResult.OK Then
                s.eac3toProfiles = DirectCast(bs.DataSource, List(Of eac3toProfile))
            End If
        End Using
    End Sub

    Sub StartAnalyze()
        Dim args = ""

        If File.Exists(M2TSFile) Then
            args = """" + M2TSFile + """ -progressnumbers"
            Log.Write("Process M2TS file using eac3to", """" + Package.eac3to.Path + """ " + args + BR2)
        ElseIf Directory.Exists(PlaylistFolder) Then
            args = """" + PlaylistFolder + """ " & PlaylistID & ") -progressnumbers"
            Log.Write("Process playlist file using eac3to", """" + Package.eac3to.Path + """ " + args + BR2)
        End If

        Using o As New Process
            AddHandler o.OutputDataReceived, AddressOf OutputDataReceived
            o.StartInfo.FileName = Package.eac3to.Path
            o.StartInfo.Arguments = args
            o.StartInfo.CreateNoWindow = True
            o.StartInfo.UseShellExecute = False
            o.StartInfo.RedirectStandardOutput = True
            o.Start()
            o.BeginOutputReadLine()
            o.WaitForExit()

            If o.ExitCode <> 0 Then
                Dim exitCode = o.ExitCode

                BeginInvoke(Sub()
                                MsgError("eac3to failed with error code " & exitCode, Output)
                                Cancel()
                            End Sub)
            Else
                BeginInvoke(Sub() Init())
            End If
        End Using
    End Sub

    Sub Cancel()
        For Each ctrl As Control In Controls
            ctrl.Enabled = True 'bnCancel is child of tlp
        Next

        bnCancel.PerformClick()
    End Sub

    Sub OutputDataReceived(sender As Object, e As DataReceivedEventArgs)
        If Not e.Data Is Nothing Then
            BeginInvoke(Sub() Text = e.Data)

            If Not e.Data.StartsWith("analyze: ") Then
                Output += e.Data + BR
            End If
        End If
    End Sub

    Sub Init()
        Text = "eac3to"

        For Each ctrl As Control In Controls
            ctrl.Enabled = True
        Next

        If Output = "" Then
            MsgWarn("eac3to output was empty")
            Cancel()
        ElseIf Output.ContainsAll({"left eye", "right eye"}) Then
            MsgError("3D demuxing isn't supported.")
            Cancel()
        ElseIf Output <> "" Then
            Log.WriteLine(Output)

            If Output.Contains(BR + "   (embedded: ") Then
                Output = Output.Replace(BR + "   (embedded: ", "(embedded: ")
            End If

            While Output.Contains("  (embedded: ")
                Output = Output.Replace("  (embedded: ", " (embedded: ")
            End While

            If Output.Contains(BR + "   (core: ") Then
                Output = Output.Replace(BR + "   (core: ", "(core: ")
            End If

            While Output.Contains("  (core: ")
                Output = Output.Replace("  (core: ", " (core: ")
            End While

            Output = Output.Replace(" channels, ", "ch, ").Replace(" bits, ", "bits, ").Replace("dialnorm", "dn")
            Output = Output.Replace("(core: ", "(").Replace("(embedded: ", "(")

            For Each line In Output.SplitLinesNoEmpty
                If line.Contains("Subtitle (DVB)") Then Continue For

                Dim match = Regex.Match(line, "^(\d+): (.+)$")

                If match.Success Then
                    Dim ms As New M2TSStream
                    ms.Text = line.Trim
                    ms.ID = match.Groups(1).Value.ToInt
                    ms.Codec = match.Groups(2).Value

                    If ms.Codec.Contains(",") Then ms.Codec = ms.Codec.Left(",")

                    ms.IsVideo = ms.Codec.EqualsAny("h264/AVC", "VC-1", "MPEG2")
                    ms.IsAudio = ms.Codec.EqualsAny("DTS Master Audio", "DTS", "DTS-ES", "DTS Hi-Res", "DTS Express", "AC3", "AC3 EX", "AC3 Surround", "E-AC3", "E-AC3 EX", "E-AC3 Surround", "TrueHD/AC3", "TrueHD/AC3 (Atmos)", "RAW/PCM", "MP2")
                    ms.IsSubtitle = ms.Codec.StartsWith("Subtitle")
                    ms.IsChapters = ms.Codec.StartsWith("Chapters")

                    If ms.IsAudio OrElse ms.IsSubtitle Then
                        For Each i2 In Language.Languages
                            If ms.Text.Contains(", " + i2.CultureInfo.EnglishName) Then
                                ms.Language = i2
                                Exit For
                            End If
                        Next

                        Select Case ms.Codec
                            Case "AC3 EX", "AC3 Surround"
                                ms.OutputType = "ac3"
                            Case "E-AC3", "E-AC3 EX"
                                ms.OutputType = "eac3"
                            Case "TrueHD/AC3"
                                ms.OutputType = "thd"
                            Case "TrueHD/AC3 (Atmos)"
                                ms.OutputType = "ac3"
                            Case "DTS-ES", "DTS Express"
                                ms.OutputType = "dts"
                            Case "DTS Master Audio", "DTS Hi-Res"
                                ms.OutputType = "dts"
                                If ms.Text.Contains("(DTS,") Then ms.Options = "-core"
                            Case "RAW/PCM"
                                ms.OutputType = "flac"
                            Case Else
                                ms.OutputType = ms.Codec.ToLower.Replace("-", "")
                        End Select
                    End If

                    For Each iProfile In s.eac3toProfiles
                        Dim searchWords = iProfile.Match.SplitNoEmptyAndWhiteSpace(" ")
                        If searchWords.NothingOrEmpty Then Continue For

                        If ms.Text.ContainsAll(searchWords) Then
                            ms.OutputType = iProfile.Output
                            ms.Options = iProfile.Options
                        End If
                    Next

                    If Not ms.IsVideo AndAlso Not ms.IsAudio AndAlso
                        Not ms.IsSubtitle AndAlso Not ms.IsChapters Then

                        Throw New Exception("Failed to detect stream: " + line)
                    End If

                    Streams.Add(ms)
                End If
            Next

            For Each stream In Streams
                If stream.IsAudio Then
                    stream.ListViewItem = lvAudio.Items.Add(stream.ToString)
                    stream.ListViewItem.Tag = stream

                    If stream.Language.TwoLetterCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName Then
                        bnAudioNative.Visible = True
                        stream.ListViewItem.Checked = True
                    ElseIf stream.Language.TwoLetterCode = "en" Then
                        bnAudioEnglish.Visible = True
                        stream.ListViewItem.Checked = True
                    ElseIf stream.Language.TwoLetterCode = "iv" Then
                        stream.ListViewItem.Checked = True
                    End If
                ElseIf stream.IsVideo Then
                    cbVideoStream.Items.Add(stream)
                ElseIf stream.IsSubtitle Then
                    If stream.Language.CultureInfo.TwoLetterISOLanguageName = CultureInfo.CurrentCulture.TwoLetterISOLanguageName Then
                        bnSubtitleNative.Visible = True
                    ElseIf stream.Language.CultureInfo.TwoLetterISOLanguageName = "en" Then
                        bnSubtitleEnglish.Visible = True
                    End If

                    Dim item = lvSubtitles.Items.Add(stream.Language.ToString)
                    item.Tag = stream

                    Dim autoCode = p.PreferredSubtitles.ToLower.SplitNoEmptyAndWhiteSpace(",", ";", " ")
                    item.Checked = autoCode.ContainsAny("all", stream.Language.TwoLetterCode, stream.Language.ThreeLetterCode)
                ElseIf stream.IsChapters Then
                    cbChapters.Visible = True
                End If
            Next

            If cbVideoStream.Items.Count < 2 Then cbVideoStream.Enabled = False

            If cbVideoStream.Items.Count > 0 Then
                cbVideoStream.SelectedIndex = 0
            Else
                cbVideoOutput.Enabled = False
            End If

            If lvAudio.Items.Count > 0 Then
                lvAudio.Items(0).Selected = True
            Else
                gbAudio.Enabled = False
            End If

            If lvSubtitles.Items.Count = 0 Then
                lvSubtitles.Enabled = False
                flpSubtitleLinks.Enabled = False
            End If

            lvAudio.Columns(0).Width = lvAudio.ClientSize.Width
        End If
    End Sub

    Private Sub eac3toForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim hdCounter As Integer

        For Each i In Streams
            If i.Checked AndAlso i.IsAudio Then
                If {"dtsma", "dtshr"}.Contains(i.OutputType) Then
                    hdCounter += 1
                ElseIf i.OutputType = "dts" AndAlso {"DTS Master Audio", "DTS Hi-Res"}.Contains(i.Codec) Then
                    hdCounter -= 1
                End If
            End If
        Next

        s.CmdlPresetsEac3to = cmdlOptions.Presets

        If Not bnOK.Enabled Then e.Cancel = True

        If DialogResult = DialogResult.OK Then
            If cbVideoOutput.Text = "MKV" AndAlso Not Package.Haali.VerifyOK(True) Then
                e.Cancel = True
            End If

            s.Storage.SetBool("demux Blu-ray chapters", cbChapters.Checked)
        End If

        If Not DirPath.IsFixedDrive(OutputFolder) OrElse Not Directory.Exists(OutputFolder) Then
            MsgWarn("The output folder must be located on a fixed local drive.")
            e.Cancel = True
        End If
    End Sub

    Private Sub AddCmdlControl_PresetsChanged(presets As String) Handles cmdlOptions.PresetsChanged
        cmdlOptions.Presets = presets
    End Sub

    Private Sub eac3toForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Task.Run(AddressOf StartAnalyze)
    End Sub

    Function GetArgs(src As String, baseName As String) As String
        Dim r = src

        Dim videoStream = TryCast(cbVideoStream.SelectedItem, M2TSStream)

        If Not videoStream Is Nothing AndAlso Not cbVideoOutput.Text = "Nothing" Then
            r += " " & videoStream.ID & ": """ + OutputFolder + baseName +
                "." + cbVideoOutput.Text.ToLower + """"
        End If

        For Each i In Streams
            If i.IsAudio AndAlso i.Checked Then
                r += " " & i.ID & ": """ + OutputFolder + baseName + " ID" & i.ID

                If Not i.Language.CultureInfo Is CultureInfo.InvariantCulture Then
                    r += " " + i.Language.CultureInfo.EnglishName
                End If

                r += "." + i.OutputType + """"

                If i.Options <> "" Then r += " " + i.Options.Trim
            End If
        Next

        For Each i In Streams
            If i.IsSubtitle AndAlso i.Checked Then
                r += " " & i.ID & ": """ + OutputFolder + baseName + " ID" & i.ID

                If Not i.Language.CultureInfo Is CultureInfo.InvariantCulture Then
                    r += " " + i.Language.CultureInfo.EnglishName
                End If

                r += ".sup"""
            End If

            If i.IsChapters AndAlso cbChapters.Checked Then
                r += " " & i.ID & ": """ + OutputFolder + baseName + "_chapters.txt"""
            End If
        Next

        Return r + " -progressnumbers"
    End Function

    Private Sub bnBrowse_Click() Handles bnBrowse.Click
        Using d As New FolderBrowserDialog
            d.Description = "Please choose a directory."
            d.SetSelectedPath(teTempDir.Text)

            If d.ShowDialog = DialogResult.OK Then
                teTempDir.Text = d.SelectedPath
            End If
        End Using
    End Sub

    Private Sub lvSubtitles_ItemCheck(sender As Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles lvSubtitles.ItemCheck
        DirectCast(lvSubtitles.Items(e.Index).Tag, M2TSStream).Checked = e.NewValue = CheckState.Checked
    End Sub

    Private Sub cbVideoStream_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbVideoStream.SelectedIndexChanged
        cbVideoOutput.Items.Clear()
        cbVideoOutput.Items.Add("Nothing")
        cbVideoOutput.Enabled = True

        Dim stream = TryCast(cbVideoStream.SelectedItem, M2TSStream)

        Select Case stream.Codec
            Case "h264/AVC"
                cbVideoOutput.Items.Add("H264")
                cbVideoOutput.Items.Add("MKV")
                cbVideoOutput.Text = If(M2TSFile = "", "MKV", "Nothing")
            Case "VC-1"
                cbVideoOutput.Items.Add("MKV")
                cbVideoOutput.Text = If(M2TSFile = "", "MKV", "Nothing")
            Case "MPEG2"
                cbVideoOutput.Items.Add("M2V")
                cbVideoOutput.Items.Add("MKV")
                cbVideoOutput.Text = If(M2TSFile = "", "M2V", "Nothing")
        End Select
    End Sub

    Private Sub cbAudioOutput_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbAudioOutput.SelectedIndexChanged
        If Not cbAudioOutput.SelectedItem Is Nothing AndAlso Not GetSelectedStream() Is Nothing Then
            Dim ms = GetSelectedStream()
            ms.OutputType = cbAudioOutput.SelectedItem.ToString
            ms.ListViewItem.Text = ms.ToString

            If ms.OutputType = "dts" AndAlso {"DTS Master Audio", "DTS Hi-Res"}.Contains(ms.Codec) AndAlso Not ms.Options.Contains("-core") Then
                If ms.Options = "" Then ms.Options = "-core"
                ms.UpdateListViewItem()
                cmdlOptions.tb.Text = ms.Options
            ElseIf {"dtsma", "dtshr"}.Contains(ms.OutputType) AndAlso ms.Options.Contains("-core") Then
                ms.Options = ms.Options.Replace(" -core ", "").Replace(" -core", "").Replace("-core ", "").Replace("-core", "")
                ms.UpdateListViewItem()
                cmdlOptions.tb.Text = ms.Options
            ElseIf ms.OutputType <> "dts" AndAlso ms.Options.Contains("-core") Then
                ms.Options = ms.Options.Replace(" -core ", "").Replace(" -core", "").Replace("-core ", "").Replace("-core", "")
                ms.UpdateListViewItem()
                cmdlOptions.tb.Text = ms.Options
            End If
        End If
    End Sub

    Private Sub cmdlOptions_ValueChanged(value As String) Handles cmdlOptions.ValueChanged
        If cmdlOptions.tb.Focused OrElse cmdlOptions.bu.Focused Then
            Dim ms = GetSelectedStream()

            If Not ms Is Nothing Then
                ms.Options = value

                If ms.Options <> "" Then
                    ms.ListViewItem.Checked = True
                End If

                If ms.Options = "-core" AndAlso ms.Codec.StartsWith("DTS") Then
                    cbAudioOutput.SelectedItem = "dts"
                End If

                If ms.Options.Contains("-quality=") Then
                    cbAudioOutput.SelectedItem = "m4a"
                End If

                lvAudio.SelectedItems(0).Text = ms.ToString
            End If
        End If
    End Sub

    Private Sub lvAudio_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles lvAudio.ItemCheck
        DirectCast(lvAudio.Items(e.Index).Tag, M2TSStream).Checked = e.NewValue = CheckState.Checked
    End Sub

    Function GetSelectedStream() As M2TSStream
        If lvAudio.SelectedItems.Count > 0 Then Return DirectCast(lvAudio.SelectedItems(0).Tag, M2TSStream)
    End Function

    Private Sub lvAudio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvAudio.SelectedIndexChanged
        Dim ms = GetSelectedStream()

        If Not ms Is Nothing Then
            cmdlOptions.tb.Text = ms.Options
            cbAudioOutput.Items.Clear()
            cbAudioOutput.Items.AddRange(GetAudioOutputFormatList(ms.Codec))
            cbAudioOutput.SelectedItem = ms.OutputType
        End If
    End Sub

    Function GetAudioOutputFormatList(codec As String) As String()
        Select Case codec
            Case "TrueHD/AC3", "TrueHD/AC3 (Atmos)"
                Return {"thd", "ac3", "thd+ac3", "wav", "flac", "m4a"}
            Case "DTS", "DTS-ES", "DTS Express"
                Return {"dts", "ac3", "wav", "flac", "m4a"}
            Case "DTS Master Audio"
                Return {"dtsma", "dts", "ac3", "wav", "flac", "m4a"}
            Case "DTS Hi-Res"
                Return {"dtshr", "dts", "ac3", "wav", "flac", "m4a"}
            Case "RAW/PCM"
                Return {"ac3", "wav", "flac", "m4a"}
            Case "E-AC3"
                Return {"eac3", "ac3", "wav", "flac", "m4a"}
            Case Else
                Return {"ac3", "wav", "flac", "m4a"}
        End Select
    End Function

    Private Sub bnAudioAll_Click(sender As Object, e As EventArgs) Handles bnAudioAll.Click
        For Each i As ListViewItem In lvAudio.Items
            i.Checked = True
        Next
    End Sub

    Private Sub bnAudioNone_Click(sender As Object, e As EventArgs) Handles bnAudioNone.Click
        For Each i As ListViewItem In lvAudio.Items
            i.Checked = False
        Next
    End Sub

    Private Sub bnAudioEnglish_Click(sender As Object, e As EventArgs) Handles bnAudioEnglish.Click
        For Each i As ListViewItem In lvAudio.Items
            Dim stream = DirectCast(i.Tag, M2TSStream)

            If stream.Language.TwoLetterCode = "en" Then
                i.Checked = True
            End If
        Next
    End Sub

    Private Sub bnAudioNative_Click(sender As Object, e As EventArgs) Handles bnAudioNative.Click
        For Each i As ListViewItem In lvAudio.Items
            Dim stream = DirectCast(i.Tag, M2TSStream)

            If stream.Language.TwoLetterCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName Then
                i.Checked = True
            End If
        Next
    End Sub

    Private Sub bnSubtitleAll_Click(sender As Object, e As EventArgs) Handles bnSubtitleAll.Click
        For Each i As ListViewItem In lvSubtitles.Items
            i.Checked = True
        Next
    End Sub

    Private Sub bnSubtitleNone_Click(sender As Object, e As EventArgs) Handles bnSubtitleNone.Click
        For Each i As ListViewItem In lvSubtitles.Items
            i.Checked = False
        Next
    End Sub

    Private Sub bnSubtitleEnglish_Click(sender As Object, e As EventArgs) Handles bnSubtitleEnglish.Click
        For Each i As ListViewItem In lvSubtitles.Items
            Dim stream = DirectCast(i.Tag, M2TSStream)

            If stream.Language.TwoLetterCode = "en" Then
                i.Checked = True
            End If
        Next
    End Sub

    Private Sub bnSubtitleNative_Click(sender As Object, e As EventArgs) Handles bnSubtitleNative.Click
        For Each i As ListViewItem In lvSubtitles.Items
            Dim stream = DirectCast(i.Tag, M2TSStream)

            If stream.Language.TwoLetterCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName Then
                i.Checked = True
            End If
        Next
    End Sub

    Private Sub teTempDir_TextChanged() Handles teTempDir.TextChanged
        OutputFolder = teTempDir.Text.AppendSeparator
    End Sub
End Class