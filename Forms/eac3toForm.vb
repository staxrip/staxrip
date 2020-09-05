
Imports System.ComponentModel
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks

Imports StaxRip.UI

Public Class eac3toForm
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
    Friend WithEvents laVideo As System.Windows.Forms.Label
    Friend WithEvents cmdlOptions As StaxRip.CommandLineControl
    Friend WithEvents bnBrowse As StaxRip.UI.ButtonEx
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents lvAudio As ListViewEx
    Friend WithEvents lvSubtitles As StaxRip.UI.ListViewEx
    Friend WithEvents cbVideoStream As System.Windows.Forms.ComboBox
    Friend WithEvents laStream As System.Windows.Forms.Label
    Friend WithEvents flpAudioLinks As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents flpSubtitleLinks As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents cbChapters As System.Windows.Forms.CheckBox
    Friend WithEvents laTargetDir As System.Windows.Forms.Label
    Friend WithEvents tlpTarget As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents gbAudio As System.Windows.Forms.GroupBox
    Friend WithEvents gbSubtitles As System.Windows.Forms.GroupBox
    Friend WithEvents cbAudioOutput As System.Windows.Forms.ComboBox
    Friend WithEvents laOptions As System.Windows.Forms.Label
    Friend WithEvents laOutput As System.Windows.Forms.Label
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
    Friend WithEvents tlpVideo As TableLayoutPanel
    Friend WithEvents tlpBottom As TableLayoutPanel
    Friend WithEvents tlpAudioOptions As TableLayoutPanel
    Friend WithEvents tlpAudio As TableLayoutPanel
    Friend WithEvents tlpSubtitles As TableLayoutPanel
    Friend WithEvents teTempDir As TextEdit
    Friend WithEvents tlpMain As TableLayoutPanel
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.cmdlOptions = New StaxRip.CommandLineControl()
        Me.cbVideoOutput = New System.Windows.Forms.ComboBox()
        Me.laVideo = New System.Windows.Forms.Label()
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
        Me.bnAudioAll = New StaxRip.UI.ButtonEx()
        Me.bnAudioNone = New StaxRip.UI.ButtonEx()
        Me.bnAudioEnglish = New StaxRip.UI.ButtonEx()
        Me.bnAudioNative = New StaxRip.UI.ButtonEx()
        Me.cbVideoStream = New System.Windows.Forms.ComboBox()
        Me.laStream = New System.Windows.Forms.Label()
        Me.tlpTarget = New System.Windows.Forms.TableLayoutPanel()
        Me.laTargetDir = New System.Windows.Forms.Label()
        Me.teTempDir = New StaxRip.UI.TextEdit()
        Me.tlpBottom = New System.Windows.Forms.TableLayoutPanel()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
        Me.cms = New StaxRip.UI.ContextMenuStripEx(Me.components)
        Me.cbChapters = New System.Windows.Forms.CheckBox()
        Me.gbAudio = New System.Windows.Forms.GroupBox()
        Me.tlpAudio = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpAudioOptions = New System.Windows.Forms.TableLayoutPanel()
        Me.laOutput = New System.Windows.Forms.Label()
        Me.laOptions = New System.Windows.Forms.Label()
        Me.cbAudioOutput = New System.Windows.Forms.ComboBox()
        Me.gbSubtitles = New System.Windows.Forms.GroupBox()
        Me.tlpSubtitles = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpVideo = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.flpSubtitleLinks.SuspendLayout()
        Me.flpAudioLinks.SuspendLayout()
        Me.tlpTarget.SuspendLayout()
        Me.tlpBottom.SuspendLayout()
        Me.gbAudio.SuspendLayout()
        Me.tlpAudio.SuspendLayout()
        Me.tlpAudioOptions.SuspendLayout()
        Me.gbSubtitles.SuspendLayout()
        Me.tlpSubtitles.SuspendLayout()
        Me.tlpVideo.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdlOptions
        '
        Me.cmdlOptions.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdlOptions.Location = New System.Drawing.Point(560, 8)
        Me.cmdlOptions.Margin = New System.Windows.Forms.Padding(3, 8, 3, 8)
        Me.cmdlOptions.Name = "cmdlOptions"
        Me.cmdlOptions.Size = New System.Drawing.Size(702, 70)
        Me.cmdlOptions.TabIndex = 5
        '
        'cbVideoOutput
        '
        Me.cbVideoOutput.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cbVideoOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbVideoOutput.FormattingEnabled = True
        Me.cbVideoOutput.Location = New System.Drawing.Point(169, 3)
        Me.cbVideoOutput.Name = "cbVideoOutput"
        Me.cbVideoOutput.Size = New System.Drawing.Size(206, 56)
        Me.cbVideoOutput.TabIndex = 2
        '
        'laVideo
        '
        Me.laVideo.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.laVideo.AutoSize = True
        Me.laVideo.Location = New System.Drawing.Point(3, 7)
        Me.laVideo.Name = "laVideo"
        Me.laVideo.Size = New System.Drawing.Size(160, 48)
        Me.laVideo.TabIndex = 1
        Me.laVideo.Text = " Output: "
        Me.laVideo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bnBrowse
        '
        Me.bnBrowse.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnBrowse.Location = New System.Drawing.Point(1171, 4)
        Me.bnBrowse.Margin = New System.Windows.Forms.Padding(0)
        Me.bnBrowse.Size = New System.Drawing.Size(100, 70)
        Me.bnBrowse.Text = "..."
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(1021, 3)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(15, 0, 0, 0)
        Me.bnCancel.Size = New System.Drawing.Size(250, 70)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(756, 3)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(15, 0, 0, 0)
        Me.bnOK.Size = New System.Drawing.Size(250, 70)
        Me.bnOK.Text = "OK"
        '
        'lvAudio
        '
        Me.lvAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvAudio.HideSelection = False
        Me.lvAudio.Location = New System.Drawing.Point(10, 0)
        Me.lvAudio.Margin = New System.Windows.Forms.Padding(10, 0, 10, 10)
        Me.lvAudio.Name = "lvAudio"
        Me.lvAudio.Size = New System.Drawing.Size(1251, 219)
        Me.lvAudio.TabIndex = 8
        Me.lvAudio.UseCompatibleStateImageBehavior = False
        '
        'lvSubtitles
        '
        Me.lvSubtitles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvSubtitles.HideSelection = False
        Me.lvSubtitles.Location = New System.Drawing.Point(10, 0)
        Me.lvSubtitles.Margin = New System.Windows.Forms.Padding(10, 0, 10, 10)
        Me.lvSubtitles.Name = "lvSubtitles"
        Me.lvSubtitles.Size = New System.Drawing.Size(1251, 148)
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
        Me.flpSubtitleLinks.Location = New System.Drawing.Point(10, 158)
        Me.flpSubtitleLinks.Margin = New System.Windows.Forms.Padding(10, 0, 0, 10)
        Me.flpSubtitleLinks.Name = "flpSubtitleLinks"
        Me.flpSubtitleLinks.Size = New System.Drawing.Size(925, 70)
        Me.flpSubtitleLinks.TabIndex = 19
        '
        'bnSubtitleAll
        '
        Me.bnSubtitleAll.Location = New System.Drawing.Point(0, 0)
        Me.bnSubtitleAll.Margin = New System.Windows.Forms.Padding(0, 0, 15, 0)
        Me.bnSubtitleAll.Size = New System.Drawing.Size(220, 70)
        Me.bnSubtitleAll.Text = "All"
        '
        'bnSubtitleNone
        '
        Me.bnSubtitleNone.Location = New System.Drawing.Point(235, 0)
        Me.bnSubtitleNone.Margin = New System.Windows.Forms.Padding(0, 0, 15, 0)
        Me.bnSubtitleNone.Size = New System.Drawing.Size(220, 70)
        Me.bnSubtitleNone.Text = "None"
        '
        'bnSubtitleEnglish
        '
        Me.bnSubtitleEnglish.Location = New System.Drawing.Point(470, 0)
        Me.bnSubtitleEnglish.Margin = New System.Windows.Forms.Padding(0, 0, 15, 0)
        Me.bnSubtitleEnglish.Size = New System.Drawing.Size(220, 70)
        Me.bnSubtitleEnglish.Text = "English"
        '
        'bnSubtitleNative
        '
        Me.bnSubtitleNative.Location = New System.Drawing.Point(705, 0)
        Me.bnSubtitleNative.Margin = New System.Windows.Forms.Padding(0)
        Me.bnSubtitleNative.Size = New System.Drawing.Size(220, 70)
        Me.bnSubtitleNative.Text = "Native"
        '
        'flpAudioLinks
        '
        Me.flpAudioLinks.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.flpAudioLinks.AutoSize = True
        Me.flpAudioLinks.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAudioLinks.Controls.Add(Me.bnAudioAll)
        Me.flpAudioLinks.Controls.Add(Me.bnAudioNone)
        Me.flpAudioLinks.Controls.Add(Me.bnAudioEnglish)
        Me.flpAudioLinks.Controls.Add(Me.bnAudioNative)
        Me.flpAudioLinks.Location = New System.Drawing.Point(10, 229)
        Me.flpAudioLinks.Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.flpAudioLinks.Name = "flpAudioLinks"
        Me.flpAudioLinks.Size = New System.Drawing.Size(925, 70)
        Me.flpAudioLinks.TabIndex = 18
        '
        'bnAudioAll
        '
        Me.bnAudioAll.Location = New System.Drawing.Point(0, 0)
        Me.bnAudioAll.Margin = New System.Windows.Forms.Padding(0, 0, 15, 0)
        Me.bnAudioAll.Size = New System.Drawing.Size(220, 70)
        Me.bnAudioAll.Text = "All"
        '
        'bnAudioNone
        '
        Me.bnAudioNone.Location = New System.Drawing.Point(235, 0)
        Me.bnAudioNone.Margin = New System.Windows.Forms.Padding(0, 0, 15, 0)
        Me.bnAudioNone.Size = New System.Drawing.Size(220, 70)
        Me.bnAudioNone.Text = "None"
        '
        'bnAudioEnglish
        '
        Me.bnAudioEnglish.Location = New System.Drawing.Point(470, 0)
        Me.bnAudioEnglish.Margin = New System.Windows.Forms.Padding(0, 0, 15, 0)
        Me.bnAudioEnglish.Size = New System.Drawing.Size(220, 70)
        Me.bnAudioEnglish.Text = "English"
        '
        'bnAudioNative
        '
        Me.bnAudioNative.Location = New System.Drawing.Point(705, 0)
        Me.bnAudioNative.Margin = New System.Windows.Forms.Padding(0)
        Me.bnAudioNative.Size = New System.Drawing.Size(220, 70)
        Me.bnAudioNative.Text = "Native"
        '
        'cbVideoStream
        '
        Me.cbVideoStream.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbVideoStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbVideoStream.FormattingEnabled = True
        Me.cbVideoStream.Location = New System.Drawing.Point(546, 3)
        Me.cbVideoStream.Name = "cbVideoStream"
        Me.cbVideoStream.Size = New System.Drawing.Size(732, 56)
        Me.cbVideoStream.TabIndex = 16
        '
        'laStream
        '
        Me.laStream.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.laStream.AutoSize = True
        Me.laStream.Location = New System.Drawing.Point(381, 7)
        Me.laStream.Name = "laStream"
        Me.laStream.Size = New System.Drawing.Size(159, 48)
        Me.laStream.TabIndex = 15
        Me.laStream.Text = " Stream: "
        Me.laStream.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
        Me.tlpTarget.Controls.Add(Me.laTargetDir, 0, 0)
        Me.tlpTarget.Controls.Add(Me.teTempDir, 1, 0)
        Me.tlpTarget.Location = New System.Drawing.Point(15, 847)
        Me.tlpTarget.Margin = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.tlpTarget.Name = "tlpTarget"
        Me.tlpTarget.RowCount = 1
        Me.tlpTarget.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpTarget.Size = New System.Drawing.Size(1271, 79)
        Me.tlpTarget.TabIndex = 4
        '
        'laTargetDir
        '
        Me.laTargetDir.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.laTargetDir.AutoSize = True
        Me.laTargetDir.Location = New System.Drawing.Point(0, 15)
        Me.laTargetDir.Margin = New System.Windows.Forms.Padding(0)
        Me.laTargetDir.Name = "laTargetDir"
        Me.laTargetDir.Size = New System.Drawing.Size(302, 48)
        Me.laTargetDir.TabIndex = 2
        Me.laTargetDir.Text = " Target Directory: "
        Me.laTargetDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'teTempDir
        '
        Me.teTempDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.teTempDir.BackColor = System.Drawing.Color.White
        Me.teTempDir.Location = New System.Drawing.Point(302, 4)
        Me.teTempDir.Margin = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.teTempDir.Name = "teTempDir"
        Me.teTempDir.Size = New System.Drawing.Size(859, 70)
        Me.teTempDir.TabIndex = 3
        '
        'tlpBottom
        '
        Me.tlpBottom.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
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
        Me.tlpBottom.Location = New System.Drawing.Point(15, 941)
        Me.tlpBottom.Margin = New System.Windows.Forms.Padding(15)
        Me.tlpBottom.Name = "tlpBottom"
        Me.tlpBottom.RowCount = 1
        Me.tlpBottom.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBottom.Size = New System.Drawing.Size(1271, 77)
        Me.tlpBottom.TabIndex = 20
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnMenu.ContextMenuStrip = Me.cms
        Me.bnMenu.Location = New System.Drawing.Point(641, 3)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(0)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(100, 70)
        '
        'cms
        '
        Me.cms.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.cms.Name = "cms"
        Me.cms.Size = New System.Drawing.Size(61, 4)
        '
        'cbChapters
        '
        Me.cbChapters.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cbChapters.AutoSize = True
        Me.cbChapters.Location = New System.Drawing.Point(15, 12)
        Me.cbChapters.Margin = New System.Windows.Forms.Padding(15, 0, 0, 0)
        Me.cbChapters.Name = "cbChapters"
        Me.cbChapters.Size = New System.Drawing.Size(323, 52)
        Me.cbChapters.TabIndex = 2
        Me.cbChapters.Text = "Extract Chapters"
        Me.cbChapters.UseVisualStyleBackColor = True
        Me.cbChapters.Visible = False
        '
        'gbAudio
        '
        Me.gbAudio.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbAudio.Controls.Add(Me.tlpAudio)
        Me.gbAudio.Location = New System.Drawing.Point(10, 82)
        Me.gbAudio.Margin = New System.Windows.Forms.Padding(10, 0, 10, 10)
        Me.gbAudio.Name = "gbAudio"
        Me.gbAudio.Padding = New System.Windows.Forms.Padding(5)
        Me.gbAudio.Size = New System.Drawing.Size(1281, 449)
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
        Me.tlpAudio.Location = New System.Drawing.Point(5, 53)
        Me.tlpAudio.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tlpAudio.Name = "tlpAudio"
        Me.tlpAudio.RowCount = 3
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpAudio.Size = New System.Drawing.Size(1271, 391)
        Me.tlpAudio.TabIndex = 23
        '
        'tlpAudioOptions
        '
        Me.tlpAudioOptions.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpAudioOptions.AutoSize = True
        Me.tlpAudioOptions.ColumnCount = 4
        Me.tlpAudioOptions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudioOptions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudioOptions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudioOptions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpAudioOptions.Controls.Add(Me.laOutput, 0, 0)
        Me.tlpAudioOptions.Controls.Add(Me.laOptions, 2, 0)
        Me.tlpAudioOptions.Controls.Add(Me.cbAudioOutput, 1, 0)
        Me.tlpAudioOptions.Controls.Add(Me.cmdlOptions, 3, 0)
        Me.tlpAudioOptions.Location = New System.Drawing.Point(3, 302)
        Me.tlpAudioOptions.Name = "tlpAudioOptions"
        Me.tlpAudioOptions.RowCount = 1
        Me.tlpAudioOptions.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpAudioOptions.Size = New System.Drawing.Size(1265, 86)
        Me.tlpAudioOptions.TabIndex = 22
        '
        'laOutput
        '
        Me.laOutput.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.laOutput.AutoSize = True
        Me.laOutput.Location = New System.Drawing.Point(3, 19)
        Me.laOutput.Name = "laOutput"
        Me.laOutput.Size = New System.Drawing.Size(160, 48)
        Me.laOutput.TabIndex = 19
        Me.laOutput.Text = " Output: "
        Me.laOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'laOptions
        '
        Me.laOptions.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.laOptions.AutoSize = True
        Me.laOptions.Location = New System.Drawing.Point(381, 19)
        Me.laOptions.Name = "laOptions"
        Me.laOptions.Size = New System.Drawing.Size(173, 48)
        Me.laOptions.TabIndex = 20
        Me.laOptions.Text = " Options: "
        Me.laOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbAudioOutput
        '
        Me.cbAudioOutput.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cbAudioOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAudioOutput.FormattingEnabled = True
        Me.cbAudioOutput.Location = New System.Drawing.Point(169, 20)
        Me.cbAudioOutput.Name = "cbAudioOutput"
        Me.cbAudioOutput.Size = New System.Drawing.Size(206, 56)
        Me.cbAudioOutput.TabIndex = 21
        '
        'gbSubtitles
        '
        Me.gbSubtitles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbSubtitles.Controls.Add(Me.tlpSubtitles)
        Me.gbSubtitles.Location = New System.Drawing.Point(10, 541)
        Me.gbSubtitles.Margin = New System.Windows.Forms.Padding(10, 0, 10, 10)
        Me.gbSubtitles.Name = "gbSubtitles"
        Me.gbSubtitles.Padding = New System.Windows.Forms.Padding(5)
        Me.gbSubtitles.Size = New System.Drawing.Size(1281, 296)
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
        Me.tlpSubtitles.Location = New System.Drawing.Point(5, 53)
        Me.tlpSubtitles.Margin = New System.Windows.Forms.Padding(1, 10, 10, 10)
        Me.tlpSubtitles.Name = "tlpSubtitles"
        Me.tlpSubtitles.RowCount = 2
        Me.tlpSubtitles.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpSubtitles.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpSubtitles.Size = New System.Drawing.Size(1271, 238)
        Me.tlpSubtitles.TabIndex = 20
        '
        'tlpVideo
        '
        Me.tlpVideo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpVideo.AutoSize = True
        Me.tlpVideo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpVideo.ColumnCount = 4
        Me.tlpVideo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpVideo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpVideo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpVideo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpVideo.Controls.Add(Me.cbVideoStream, 3, 0)
        Me.tlpVideo.Controls.Add(Me.cbVideoOutput, 1, 0)
        Me.tlpVideo.Controls.Add(Me.laStream, 2, 0)
        Me.tlpVideo.Controls.Add(Me.laVideo, 0, 0)
        Me.tlpVideo.Location = New System.Drawing.Point(10, 10)
        Me.tlpVideo.Margin = New System.Windows.Forms.Padding(10)
        Me.tlpVideo.Name = "tlpVideo"
        Me.tlpVideo.RowCount = 1
        Me.tlpVideo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpVideo.Size = New System.Drawing.Size(1281, 62)
        Me.tlpVideo.TabIndex = 25
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.tlpVideo, 0, 0)
        Me.tlpMain.Controls.Add(Me.gbAudio, 0, 1)
        Me.tlpMain.Controls.Add(Me.tlpBottom, 0, 4)
        Me.tlpMain.Controls.Add(Me.gbSubtitles, 0, 2)
        Me.tlpMain.Controls.Add(Me.tlpTarget, 0, 3)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 5
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1301, 1033)
        Me.tlpMain.TabIndex = 26
        '
        'eac3toForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1301, 1033)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = False
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(10)
        Me.Name = "eac3toForm"
        Me.Text = "eac3to Demuxing"
        Me.flpSubtitleLinks.ResumeLayout(False)
        Me.flpAudioLinks.ResumeLayout(False)
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
        Me.tlpVideo.ResumeLayout(False)
        Me.tlpVideo.PerformLayout()
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Property M2TSFile As String
    Property PlaylistFolder As String
    Property OutputFolder As String
    Property PlaylistID As Integer

    Private Output As String
    Private Streams As New BindingList(Of M2TSStream)
    Private AudioOutputFormats As String() =
        {"m4a", "ac3", "dts", "flac", "wav", "dtsma", "dtshr", "eac3", "thd", "thd+ac3"}

    Private Project As Project

    Sub New(proj As Project)
        MyBase.New()
        InitializeComponent()
        Project = proj
        ScaleClientSize(40, 30)

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
        lvAudio.HideFocusRectange()

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
        Dim form As New HelpForm
        form.Doc.WriteStart("Audio Stream Profiles")
        form.Doc.WriteParagraph("Allows to automatically apply default values for audio streams.")
        form.Doc.WriteTable({New StringPair("Match All", "space separated, if all match then the Output Format and Options are applied"),
                             New StringPair("Output Format", "applied to the stream if Match All succeeds"),
                             New StringPair("Options", "applied to the stream if Match All succeeds")})
        form.Show()
    End Sub

    Sub ShowAudioStreamProfiles()
        Using form As New DataForm
            form.Text = "Audio Stream Profiles"
            form.FormBorderStyle = FormBorderStyle.Sizable
            form.dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            form.dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            form.dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            form.dgv.AllowUserToDeleteRows = True
            form.dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            form.HelpAction = AddressOf ShowAudioStreamProfilesHelp

            Dim match = form.dgv.AddTextBoxColumn()
            match.DataPropertyName = "Match"
            match.HeaderText = "Match All"

            Dim out = form.dgv.AddComboBoxColumn()
            out.DataPropertyName = "Output"
            out.HeaderText = "Output Format"
            out.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            out.Items.AddRange(AudioOutputFormats)

            Dim opt = form.dgv.AddTextBoxColumn()
            opt.DataPropertyName = "Options"
            opt.HeaderText = "Options"

            form.dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

            Dim bs As New BindingSource

            bs.DataSource = ObjectHelp.GetCopy(s.eac3toProfiles)
            form.dgv.DataSource = bs

            If form.ShowDialog = DialogResult.OK Then
                s.eac3toProfiles = DirectCast(bs.DataSource, List(Of eac3toProfile))
            End If
        End Using
    End Sub

    Sub StartAnalyze()
        Dim args = ""

        If File.Exists(M2TSFile) Then
            args = M2TSFile.Escape + " -progressnumbers"
            Project.Log.Write("Process M2TS file using eac3to", Package.eac3to.Path.Escape + " " + args + BR2)
        ElseIf Directory.Exists(PlaylistFolder) Then
            args = PlaylistFolder.Escape + " " & PlaylistID & ") -progressnumbers"
            Project.Log.Write("Process playlist file using eac3to", Package.eac3to.Path.Escape + " " + args + BR2)
        End If

        Using pr As New Process
            AddHandler pr.OutputDataReceived, AddressOf OutputDataReceived
            pr.StartInfo.FileName = Package.eac3to.Path
            pr.StartInfo.Arguments = args
            pr.StartInfo.CreateNoWindow = True
            pr.StartInfo.UseShellExecute = False
            pr.StartInfo.RedirectStandardOutput = True
            pr.Start()
            pr.BeginOutputReadLine()
            pr.WaitForExit()

            If pr.ExitCode <> 0 Then
                Dim exitCode = pr.ExitCode

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
            ctrl.Enabled = True
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
        ElseIf Output.ContainsAll("left eye", "right eye") Then
            MsgError("3D demuxing isn't supported.")
            Cancel()
        ElseIf Output <> "" Then
            Project.Log.WriteLine(Output)

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
                If line.Contains("Subtitle (DVB)") Then
                    Continue For
                End If

                Dim match = Regex.Match(line, "^(\d+): (.+)$")

                If match.Success Then
                    Dim ms As New M2TSStream
                    ms.Text = line.Trim
                    ms.ID = match.Groups(1).Value.ToInt
                    ms.Codec = match.Groups(2).Value

                    If ms.Codec.Contains(",") Then
                        ms.Codec = ms.Codec.Left(",")
                    End If

                    ms.IsVideo = ms.Codec.EqualsAny("h264/AVC", "h265/HEVC", "VC-1", "MPEG2")

                    ms.IsAudio = ms.Codec.EqualsAny("DTS Master Audio", "DTS", "DTS-ES",
                        "DTS Hi-Res", "DTS Express", "AC3", "AC3 EX", "AC3 Headphone",
                        "AC3 Surround", "EAC3", "E-AC3", "E-AC3 EX", "E-AC3 Surround", "TrueHD/AC3",
                        "TrueHD/AC3 (Atmos)", "TrueHD (Atmos)", "RAW/PCM", "MP2", "AAC")

                    ms.IsSubtitle = ms.Codec.StartsWith("Subtitle")
                    ms.IsChapters = ms.Codec.StartsWith("Chapters")

                    If ms.IsAudio OrElse ms.IsSubtitle Then
                        For Each lng In Language.Languages
                            If ms.Text.Contains(", " + lng.CultureInfo.EnglishName) Then
                                ms.Language = lng
                                Exit For
                            End If
                        Next

                        Select Case ms.Codec
                            Case "AC3", "AC3 EX", "AC3 Surround", "AC3 Headphone"
                                ms.OutputType = "ac3"
                            Case "E-AC3", "E-AC3 EX"
                                ms.OutputType = "eac3"
                            Case "TrueHD/AC3 (Atmos)", "TrueHD/AC3"
                                ms.OutputType = "thd+ac3"
                            Case "TrueHD (Atmos)"
                                ms.OutputType = "thd"
                            Case "DTS-ES", "DTS Express"
                                ms.OutputType = "dts"
                            Case "DTS Master Audio"
                                ms.OutputType = "dtsma"
                            Case "DTS Hi-Res"
                                ms.OutputType = "dtshr"
                            Case "RAW/PCM"
                                ms.OutputType = "flac"
                            Case "AAC"
                                ms.OutputType = "m4a"
                            Case "Subtitle (ASS)"
                                ms.OutputType = "ass"
                            Case Else
                                ms.OutputType = ms.Codec.ToLower.Replace("-", "")
                        End Select
                    End If

                    For Each pro In s.eac3toProfiles
                        Dim searchWords = pro.Match.SplitNoEmptyAndWhiteSpace(" ")

                        If searchWords.NothingOrEmpty Then
                            Continue For
                        End If

                        If ms.Text.ContainsAll(searchWords) Then
                            ms.OutputType = pro.Output
                            ms.Options = pro.Options
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

                    Dim autoCode = Project.PreferredAudio.ToLower.SplitNoEmptyAndWhiteSpace(",", ";", " ")
                    stream.ListViewItem.Checked = autoCode.ContainsAny("all", stream.Language.TwoLetterCode, stream.Language.ThreeLetterCode)
                ElseIf stream.IsVideo Then
                    cbVideoStream.Items.Add(stream)
                ElseIf stream.IsSubtitle Then
                    Dim item = lvSubtitles.Items.Add(stream.Language.ToString)
                    item.Tag = stream

                    Dim autoCode = Project.PreferredSubtitles.ToLower.SplitNoEmptyAndWhiteSpace(",", ";", " ")
                    item.Checked = autoCode.ContainsAny("all", stream.Language.TwoLetterCode, stream.Language.ThreeLetterCode)
                ElseIf stream.IsChapters Then
                    cbChapters.Visible = True
                End If
            Next

            If cbVideoStream.Items.Count < 2 Then
                cbVideoStream.Enabled = False
            End If

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

    Sub AddCmdlControl_PresetsChanged(presets As String) Handles cmdlOptions.PresetsChanged
        cmdlOptions.Presets = presets
    End Sub

    Function GetArgs(src As String, baseName As String) As String
        Dim ret = src

        Dim videoStream = TryCast(cbVideoStream.SelectedItem, M2TSStream)

        If Not videoStream Is Nothing AndAlso Not cbVideoOutput.Text = "Nothing" Then
            ret += " " & videoStream.ID & ": " + (OutputFolder + baseName +
                "." + cbVideoOutput.Text.ToLower).Escape
        End If

        For Each stream In Streams
            If stream.IsAudio AndAlso stream.Checked Then
                ret += " " & stream.ID & ": """ + OutputFolder + baseName + " ID" & stream.ID

                If stream.Language.CultureInfo.TwoLetterISOLanguageName <> "iv" Then
                    ret += " " + stream.Language.CultureInfo.EnglishName
                End If

                ret += "." + stream.OutputType + """"

                If stream.Options <> "" Then
                    ret += " " + stream.Options.Trim
                End If
            End If
        Next

        For Each stream In Streams
            If stream.IsSubtitle AndAlso stream.Checked Then
                ret += " " & stream.ID & ": """ + OutputFolder + baseName + " ID" & stream.ID

                If stream.Language.CultureInfo.TwoLetterISOLanguageName <> "iv" Then
                    ret += " " + stream.Language.CultureInfo.EnglishName
                End If

                ret += ".sup"""
            End If

            If stream.IsChapters AndAlso cbChapters.Checked Then
                ret += " " & stream.ID & ": """ + OutputFolder + baseName + "_chapters.txt"""
            End If
        Next

        Return ret + " -progressnumbers"
    End Function

    Sub bnBrowse_Click() Handles bnBrowse.Click
        Using dialog As New FolderBrowserDialog
            dialog.SetSelectedPath(teTempDir.Text)

            If dialog.ShowDialog = DialogResult.OK Then
                teTempDir.Text = dialog.SelectedPath
            End If
        End Using
    End Sub

    Sub lvSubtitles_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles lvSubtitles.ItemCheck
        DirectCast(lvSubtitles.Items(e.Index).Tag, M2TSStream).Checked = e.NewValue = CheckState.Checked
    End Sub

    Sub cbVideoStream_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbVideoStream.SelectedIndexChanged
        cbVideoOutput.Items.Clear()
        cbVideoOutput.Items.Add("Nothing")
        cbVideoOutput.Enabled = True

        Dim stream = TryCast(cbVideoStream.SelectedItem, M2TSStream)

        Select Case stream.Codec
            Case "h264/AVC"
                cbVideoOutput.Items.Add("H264")
                cbVideoOutput.Items.Add("MKV")
                cbVideoOutput.Text = If(M2TSFile = "", "H264", "Nothing")
            Case "h265/HEVC"
                cbVideoOutput.Items.Add("H265")
                cbVideoOutput.Text = If(M2TSFile = "", "H265", "Nothing")
            Case "VC-1"
                cbVideoOutput.Items.Add("MKV")
                cbVideoOutput.Items.Add("VC1")
                cbVideoOutput.Text = If(M2TSFile = "", "VC1", "Nothing")
            Case "MPEG2"
                cbVideoOutput.Items.Add("M2V")
                cbVideoOutput.Items.Add("MKV")
                cbVideoOutput.Text = If(M2TSFile = "", "M2V", "Nothing")
        End Select
    End Sub

    Sub cbAudioOutput_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbAudioOutput.SelectedIndexChanged
        If Not cbAudioOutput.SelectedItem Is Nothing AndAlso Not GetSelectedStream() Is Nothing Then
            Dim ms = GetSelectedStream()
            ms.OutputType = cbAudioOutput.SelectedItem.ToString
            ms.ListViewItem.Text = ms.ToString

            If ms.OutputType = "dts" AndAlso {"DTS Master Audio", "DTS Hi-Res"}.Contains(ms.Codec) AndAlso Not ms.Options.Contains("-core") Then
                If ms.Options = "" Then
                    ms.Options = "-core"
                End If

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

    Sub cmdlOptions_ValueChanged(value As String) Handles cmdlOptions.ValueChanged
        If cmdlOptions.tb.Focused OrElse cmdlOptions.bn.Focused Then
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

    Sub lvAudio_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles lvAudio.ItemCheck
        DirectCast(lvAudio.Items(e.Index).Tag, M2TSStream).Checked = e.NewValue = CheckState.Checked
    End Sub

    Function GetSelectedStream() As M2TSStream
        If lvAudio.SelectedItems.Count > 0 Then
            Return DirectCast(lvAudio.SelectedItems(0).Tag, M2TSStream)
        End If
    End Function

    Sub lvAudio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvAudio.SelectedIndexChanged
        Dim ms = GetSelectedStream()

        If Not ms Is Nothing Then
            cmdlOptions.tb.Text = ms.Options
            cbAudioOutput.SelectedItem = ms.OutputType
        End If
    End Sub

    Sub bnAudioAll_Click(sender As Object, e As EventArgs) Handles bnAudioAll.Click
        For Each item As ListViewItem In lvAudio.Items
            item.Checked = True
        Next
    End Sub

    Sub bnAudioNone_Click(sender As Object, e As EventArgs) Handles bnAudioNone.Click
        For Each item As ListViewItem In lvAudio.Items
            item.Checked = False
        Next
    End Sub

    Sub bnAudioEnglish_Click(sender As Object, e As EventArgs) Handles bnAudioEnglish.Click
        For Each item As ListViewItem In lvAudio.Items
            Dim stream = DirectCast(item.Tag, M2TSStream)

            If stream.Language.TwoLetterCode = "en" Then
                item.Checked = True
            End If
        Next
    End Sub

    Sub bnAudioNative_Click(sender As Object, e As EventArgs) Handles bnAudioNative.Click
        For Each item As ListViewItem In lvAudio.Items
            Dim stream = DirectCast(item.Tag, M2TSStream)

            If stream.Language.TwoLetterCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName Then
                item.Checked = True
            End If
        Next
    End Sub

    Sub bnSubtitleAll_Click(sender As Object, e As EventArgs) Handles bnSubtitleAll.Click
        For Each item As ListViewItem In lvSubtitles.Items
            item.Checked = True
        Next
    End Sub

    Sub bnSubtitleNone_Click(sender As Object, e As EventArgs) Handles bnSubtitleNone.Click
        For Each item As ListViewItem In lvSubtitles.Items
            item.Checked = False
        Next
    End Sub

    Sub bnSubtitleEnglish_Click(sender As Object, e As EventArgs) Handles bnSubtitleEnglish.Click
        For Each item As ListViewItem In lvSubtitles.Items
            Dim stream = DirectCast(item.Tag, M2TSStream)

            If stream.Language.TwoLetterCode = "en" Then
                item.Checked = True
            End If
        Next
    End Sub

    Sub bnSubtitleNative_Click(sender As Object, e As EventArgs) Handles bnSubtitleNative.Click
        For Each item As ListViewItem In lvSubtitles.Items
            Dim stream = DirectCast(item.Tag, M2TSStream)

            If stream.Language.TwoLetterCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName Then
                item.Checked = True
            End If
        Next
    End Sub

    Sub teTempDir_TextChanged() Handles teTempDir.TextChanged
        OutputFolder = teTempDir.Text.FixDir
    End Sub

    Protected Overrides Sub OnFormClosing(args As FormClosingEventArgs)
        MyBase.OnFormClosing(args)

        Dim hdCounter As Integer

        For Each str In Streams
            If str.Checked AndAlso str.IsAudio Then
                If {"dtsma", "dtshr"}.Contains(str.OutputType) Then
                    hdCounter += 1
                ElseIf str.OutputType = "dts" AndAlso {"DTS Master Audio", "DTS Hi-Res"}.Contains(str.Codec) Then
                    hdCounter -= 1
                End If
            End If
        Next

        s.CmdlPresetsEac3to = cmdlOptions.Presets

        If Not bnOK.Enabled Then
            args.Cancel = True
        End If

        If DialogResult = DialogResult.OK Then
            If cbVideoOutput.Text = "MKV" AndAlso Not Package.Haali.VerifyOK(True) Then
                args.Cancel = True
            End If

            s.Storage.SetBool("demux Blu-ray chapters", cbChapters.Checked)
        End If

        MyBase.OnFormClosing(args)
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        Task.Run(AddressOf StartAnalyze)
    End Sub
End Class
