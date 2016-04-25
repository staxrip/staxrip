Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Win32

Class MainForm
    Inherits Form

    Property SourceFile As String
    Property TargetDir As String
    Property AudioStreams As List(Of AudioStream)
    Property Subtitles As List(Of Subtitle)

#Region "Designer"

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Friend WithEvents bnDemux As Button
    Friend WithEvents tlpPaths As TableLayoutPanel
    Friend WithEvents tbSourceFile As TextBox
    Friend WithEvents tbTargetDir As TextBox
    Friend WithEvents bnSourceFile As Button
    Friend WithEvents bnTargetDir As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents cbVideo As CheckBox
    Friend WithEvents cbChapters As CheckBox
    Friend WithEvents cbAttachments As CheckBox

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.gbAudio = New System.Windows.Forms.GroupBox()
        Me.AudioTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.lvAudio = New Demux.ListViewEx()
        Me.flpAudioButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnAudioAll = New System.Windows.Forms.Button()
        Me.bnAudioNone = New System.Windows.Forms.Button()
        Me.bnAudioEnglish = New System.Windows.Forms.Button()
        Me.bnAudioNative = New System.Windows.Forms.Button()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.gbSubtitles = New System.Windows.Forms.GroupBox()
        Me.SubtitleTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.lvSubtitles = New Demux.ListViewEx()
        Me.flpSubtitleButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnSubtitleAll = New System.Windows.Forms.Button()
        Me.bnSubtitleNone = New System.Windows.Forms.Button()
        Me.bnSubtitleEnglish = New System.Windows.Forms.Button()
        Me.bnSubtitleNative = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
        Me.cbVideo = New System.Windows.Forms.CheckBox()
        Me.cbChapters = New System.Windows.Forms.CheckBox()
        Me.cbAttachments = New System.Windows.Forms.CheckBox()
        Me.bnDemux = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.tlpPaths = New System.Windows.Forms.TableLayoutPanel()
        Me.tbSourceFile = New System.Windows.Forms.TextBox()
        Me.tbTargetDir = New System.Windows.Forms.TextBox()
        Me.bnSourceFile = New System.Windows.Forms.Button()
        Me.bnTargetDir = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.gbAudio.SuspendLayout()
        Me.AudioTableLayoutPanel.SuspendLayout()
        Me.flpAudioButtons.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.gbSubtitles.SuspendLayout()
        Me.SubtitleTableLayoutPanel.SuspendLayout()
        Me.flpSubtitleButtons.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.tlpPaths.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbAudio
        '
        Me.gbAudio.Controls.Add(Me.AudioTableLayoutPanel)
        Me.gbAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbAudio.Location = New System.Drawing.Point(9, 120)
        Me.gbAudio.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.gbAudio.Name = "gbAudio"
        Me.gbAudio.Size = New System.Drawing.Size(815, 266)
        Me.gbAudio.TabIndex = 0
        Me.gbAudio.TabStop = False
        Me.gbAudio.Text = "Audio"
        '
        'AudioTableLayoutPanel
        '
        Me.AudioTableLayoutPanel.ColumnCount = 1
        Me.AudioTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.AudioTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.AudioTableLayoutPanel.Controls.Add(Me.lvAudio, 0, 0)
        Me.AudioTableLayoutPanel.Controls.Add(Me.flpAudioButtons, 0, 1)
        Me.AudioTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AudioTableLayoutPanel.Location = New System.Drawing.Point(3, 27)
        Me.AudioTableLayoutPanel.Name = "AudioTableLayoutPanel"
        Me.AudioTableLayoutPanel.RowCount = 2
        Me.AudioTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.AudioTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.AudioTableLayoutPanel.Size = New System.Drawing.Size(809, 236)
        Me.AudioTableLayoutPanel.TabIndex = 19
        '
        'lvAudio
        '
        Me.lvAudio.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvAudio.ItemCheckProperty = "Enabled"
        Me.lvAudio.Location = New System.Drawing.Point(3, 3)
        Me.lvAudio.Name = "lvAudio"
        Me.lvAudio.Size = New System.Drawing.Size(803, 182)
        Me.lvAudio.TabIndex = 0
        Me.lvAudio.UseCompatibleStateImageBehavior = False
        '
        'flpAudioButtons
        '
        Me.flpAudioButtons.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpAudioButtons.AutoSize = True
        Me.flpAudioButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAudioButtons.Controls.Add(Me.bnAudioAll)
        Me.flpAudioButtons.Controls.Add(Me.bnAudioNone)
        Me.flpAudioButtons.Controls.Add(Me.bnAudioEnglish)
        Me.flpAudioButtons.Controls.Add(Me.bnAudioNative)
        Me.flpAudioButtons.Location = New System.Drawing.Point(3, 191)
        Me.flpAudioButtons.Name = "flpAudioButtons"
        Me.flpAudioButtons.Size = New System.Drawing.Size(421, 42)
        Me.flpAudioButtons.TabIndex = 1
        '
        'bnAudioAll
        '
        Me.bnAudioAll.Location = New System.Drawing.Point(0, 3)
        Me.bnAudioAll.Margin = New System.Windows.Forms.Padding(0, 3, 3, 3)
        Me.bnAudioAll.Name = "bnAudioAll"
        Me.bnAudioAll.Size = New System.Drawing.Size(100, 36)
        Me.bnAudioAll.TabIndex = 0
        Me.bnAudioAll.Text = "All"
        '
        'bnAudioNone
        '
        Me.bnAudioNone.Location = New System.Drawing.Point(106, 3)
        Me.bnAudioNone.Name = "bnAudioNone"
        Me.bnAudioNone.Size = New System.Drawing.Size(100, 36)
        Me.bnAudioNone.TabIndex = 1
        Me.bnAudioNone.Text = "None"
        '
        'bnAudioEnglish
        '
        Me.bnAudioEnglish.Location = New System.Drawing.Point(212, 3)
        Me.bnAudioEnglish.Name = "bnAudioEnglish"
        Me.bnAudioEnglish.Size = New System.Drawing.Size(100, 36)
        Me.bnAudioEnglish.TabIndex = 2
        Me.bnAudioEnglish.Text = "English"
        '
        'bnAudioNative
        '
        Me.bnAudioNative.Location = New System.Drawing.Point(318, 3)
        Me.bnAudioNative.Name = "bnAudioNative"
        Me.bnAudioNative.Size = New System.Drawing.Size(100, 36)
        Me.bnAudioNative.TabIndex = 3
        Me.bnAudioNative.Text = "Native"
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.gbSubtitles, 0, 2)
        Me.tlpMain.Controls.Add(Me.gbAudio, 0, 1)
        Me.tlpMain.Controls.Add(Me.FlowLayoutPanel2, 0, 4)
        Me.tlpMain.Controls.Add(Me.GroupBox1, 0, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.Padding = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.tlpMain.RowCount = 5
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(833, 664)
        Me.tlpMain.TabIndex = 25
        '
        'gbSubtitles
        '
        Me.gbSubtitles.Controls.Add(Me.SubtitleTableLayoutPanel)
        Me.gbSubtitles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbSubtitles.Location = New System.Drawing.Point(9, 392)
        Me.gbSubtitles.Name = "gbSubtitles"
        Me.gbSubtitles.Size = New System.Drawing.Size(815, 214)
        Me.gbSubtitles.TabIndex = 1
        Me.gbSubtitles.TabStop = False
        Me.gbSubtitles.Text = "Subtitles"
        '
        'SubtitleTableLayoutPanel
        '
        Me.SubtitleTableLayoutPanel.ColumnCount = 1
        Me.SubtitleTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.SubtitleTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.SubtitleTableLayoutPanel.Controls.Add(Me.lvSubtitles, 0, 0)
        Me.SubtitleTableLayoutPanel.Controls.Add(Me.flpSubtitleButtons, 0, 1)
        Me.SubtitleTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SubtitleTableLayoutPanel.Location = New System.Drawing.Point(3, 27)
        Me.SubtitleTableLayoutPanel.Name = "SubtitleTableLayoutPanel"
        Me.SubtitleTableLayoutPanel.RowCount = 2
        Me.SubtitleTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.SubtitleTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.SubtitleTableLayoutPanel.Size = New System.Drawing.Size(809, 184)
        Me.SubtitleTableLayoutPanel.TabIndex = 20
        '
        'lvSubtitles
        '
        Me.lvSubtitles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvSubtitles.ItemCheckProperty = "Enabled"
        Me.lvSubtitles.Location = New System.Drawing.Point(3, 3)
        Me.lvSubtitles.Name = "lvSubtitles"
        Me.lvSubtitles.Size = New System.Drawing.Size(803, 130)
        Me.lvSubtitles.TabIndex = 0
        Me.lvSubtitles.UseCompatibleStateImageBehavior = False
        '
        'flpSubtitleButtons
        '
        Me.flpSubtitleButtons.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpSubtitleButtons.AutoSize = True
        Me.flpSubtitleButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpSubtitleButtons.Controls.Add(Me.bnSubtitleAll)
        Me.flpSubtitleButtons.Controls.Add(Me.bnSubtitleNone)
        Me.flpSubtitleButtons.Controls.Add(Me.bnSubtitleEnglish)
        Me.flpSubtitleButtons.Controls.Add(Me.bnSubtitleNative)
        Me.flpSubtitleButtons.Location = New System.Drawing.Point(3, 139)
        Me.flpSubtitleButtons.Name = "flpSubtitleButtons"
        Me.flpSubtitleButtons.Size = New System.Drawing.Size(421, 42)
        Me.flpSubtitleButtons.TabIndex = 1
        '
        'bnSubtitleAll
        '
        Me.bnSubtitleAll.Location = New System.Drawing.Point(0, 3)
        Me.bnSubtitleAll.Margin = New System.Windows.Forms.Padding(0, 3, 3, 3)
        Me.bnSubtitleAll.Name = "bnSubtitleAll"
        Me.bnSubtitleAll.Size = New System.Drawing.Size(100, 36)
        Me.bnSubtitleAll.TabIndex = 0
        Me.bnSubtitleAll.Text = "All"
        '
        'bnSubtitleNone
        '
        Me.bnSubtitleNone.Location = New System.Drawing.Point(106, 3)
        Me.bnSubtitleNone.Name = "bnSubtitleNone"
        Me.bnSubtitleNone.Size = New System.Drawing.Size(100, 36)
        Me.bnSubtitleNone.TabIndex = 1
        Me.bnSubtitleNone.Text = "None"
        '
        'bnSubtitleEnglish
        '
        Me.bnSubtitleEnglish.Location = New System.Drawing.Point(212, 3)
        Me.bnSubtitleEnglish.Name = "bnSubtitleEnglish"
        Me.bnSubtitleEnglish.Size = New System.Drawing.Size(100, 36)
        Me.bnSubtitleEnglish.TabIndex = 2
        Me.bnSubtitleEnglish.Text = "English"
        '
        'bnSubtitleNative
        '
        Me.bnSubtitleNative.Location = New System.Drawing.Point(318, 3)
        Me.bnSubtitleNative.Name = "bnSubtitleNative"
        Me.bnSubtitleNative.Size = New System.Drawing.Size(100, 36)
        Me.bnSubtitleNative.TabIndex = 3
        Me.bnSubtitleNative.Text = "Native"
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.FlowLayoutPanel2.AutoSize = True
        Me.FlowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel2.Controls.Add(Me.cbVideo)
        Me.FlowLayoutPanel2.Controls.Add(Me.cbChapters)
        Me.FlowLayoutPanel2.Controls.Add(Me.cbAttachments)
        Me.FlowLayoutPanel2.Controls.Add(Me.bnDemux)
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(373, 613)
        Me.FlowLayoutPanel2.Margin = New System.Windows.Forms.Padding(0, 4, 0, 8)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(454, 42)
        Me.FlowLayoutPanel2.TabIndex = 3
        '
        'cbVideo
        '
        Me.cbVideo.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cbVideo.AutoSize = True
        Me.cbVideo.Location = New System.Drawing.Point(3, 6)
        Me.cbVideo.Name = "cbVideo"
        Me.cbVideo.Size = New System.Drawing.Size(84, 29)
        Me.cbVideo.TabIndex = 0
        Me.cbVideo.Text = "Video"
        Me.cbVideo.UseVisualStyleBackColor = True
        '
        'cbChapters
        '
        Me.cbChapters.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cbChapters.AutoSize = True
        Me.cbChapters.Location = New System.Drawing.Point(93, 6)
        Me.cbChapters.Name = "cbChapters"
        Me.cbChapters.Size = New System.Drawing.Size(108, 29)
        Me.cbChapters.TabIndex = 1
        Me.cbChapters.Text = "Chapters"
        Me.cbChapters.UseVisualStyleBackColor = True
        '
        'cbAttachments
        '
        Me.cbAttachments.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cbAttachments.AutoSize = True
        Me.cbAttachments.Location = New System.Drawing.Point(207, 6)
        Me.cbAttachments.Name = "cbAttachments"
        Me.cbAttachments.Size = New System.Drawing.Size(138, 29)
        Me.cbAttachments.TabIndex = 2
        Me.cbAttachments.Text = "Attachments"
        Me.cbAttachments.UseVisualStyleBackColor = True
        '
        'bnDemux
        '
        Me.bnDemux.Location = New System.Drawing.Point(351, 3)
        Me.bnDemux.Name = "bnDemux"
        Me.bnDemux.Size = New System.Drawing.Size(100, 36)
        Me.bnDemux.TabIndex = 3
        Me.bnDemux.Text = "Demux"
        '
        'GroupBox1
        '
        Me.GroupBox1.AutoSize = True
        Me.GroupBox1.Controls.Add(Me.tlpPaths)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(9, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(815, 114)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Paths"
        '
        'tlpPaths
        '
        Me.tlpPaths.AutoSize = True
        Me.tlpPaths.ColumnCount = 3
        Me.tlpPaths.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpPaths.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpPaths.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpPaths.Controls.Add(Me.tbSourceFile, 1, 0)
        Me.tlpPaths.Controls.Add(Me.tbTargetDir, 1, 1)
        Me.tlpPaths.Controls.Add(Me.bnSourceFile, 2, 0)
        Me.tlpPaths.Controls.Add(Me.bnTargetDir, 2, 1)
        Me.tlpPaths.Controls.Add(Me.Label1, 0, 0)
        Me.tlpPaths.Controls.Add(Me.Label2, 0, 1)
        Me.tlpPaths.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpPaths.Location = New System.Drawing.Point(3, 27)
        Me.tlpPaths.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpPaths.Name = "tlpPaths"
        Me.tlpPaths.RowCount = 2
        Me.tlpPaths.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpPaths.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpPaths.Size = New System.Drawing.Size(809, 84)
        Me.tlpPaths.TabIndex = 0
        '
        'tbSourceFile
        '
        Me.tbSourceFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSourceFile.Location = New System.Drawing.Point(128, 5)
        Me.tbSourceFile.Name = "tbSourceFile"
        Me.tbSourceFile.Size = New System.Drawing.Size(636, 31)
        Me.tbSourceFile.TabIndex = 1
        '
        'tbTargetDir
        '
        Me.tbTargetDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbTargetDir.Location = New System.Drawing.Point(128, 47)
        Me.tbTargetDir.Name = "tbTargetDir"
        Me.tbTargetDir.Size = New System.Drawing.Size(636, 31)
        Me.tbTargetDir.TabIndex = 4
        '
        'bnSourceFile
        '
        Me.bnSourceFile.Location = New System.Drawing.Point(770, 3)
        Me.bnSourceFile.Name = "bnSourceFile"
        Me.bnSourceFile.Size = New System.Drawing.Size(36, 36)
        Me.bnSourceFile.TabIndex = 2
        Me.bnSourceFile.Text = "..."
        '
        'bnTargetDir
        '
        Me.bnTargetDir.Location = New System.Drawing.Point(770, 45)
        Me.bnTargetDir.Name = "bnTargetDir"
        Me.bnTargetDir.Size = New System.Drawing.Size(36, 36)
        Me.bnTargetDir.TabIndex = 5
        Me.bnTargetDir.Text = "..."
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(119, 25)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Source File:"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(119, 25)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Target Folder:"
        '
        'MainForm
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144.0!, 144.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(833, 664)
        Me.Controls.Add(Me.tlpMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(600, 600)
        Me.Name = "MainForm"
        Me.ShowIcon = False
        Me.Text = "Demux"
        Me.gbAudio.ResumeLayout(False)
        Me.AudioTableLayoutPanel.ResumeLayout(False)
        Me.AudioTableLayoutPanel.PerformLayout()
        Me.flpAudioButtons.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.gbSubtitles.ResumeLayout(False)
        Me.SubtitleTableLayoutPanel.ResumeLayout(False)
        Me.SubtitleTableLayoutPanel.PerformLayout()
        Me.flpSubtitleButtons.ResumeLayout(False)
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.FlowLayoutPanel2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tlpPaths.ResumeLayout(False)
        Me.tlpPaths.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents gbAudio As GroupBox
    Friend WithEvents flpAudioButtons As FlowLayoutPanel
    Friend WithEvents bnAudioAll As Button
    Friend WithEvents bnAudioNone As Button
    Friend WithEvents bnAudioEnglish As Button
    Friend WithEvents bnAudioNative As Button
    Friend WithEvents lvAudio As ListViewEx
    Friend WithEvents AudioTableLayoutPanel As TableLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents gbSubtitles As GroupBox
    Friend WithEvents flpSubtitleButtons As FlowLayoutPanel
    Friend WithEvents bnSubtitleAll As Button
    Friend WithEvents bnSubtitleNone As Button
    Friend WithEvents bnSubtitleEnglish As Button
    Friend WithEvents bnSubtitleNative As Button
    Friend WithEvents lvSubtitles As ListViewEx
    Friend WithEvents FlowLayoutPanel2 As FlowLayoutPanel
    Friend WithEvents SubtitleTableLayoutPanel As TableLayoutPanel

#End Region

    Sub New()
        InitializeComponent()

        lvAudio.EnableListBoxMode()
        lvAudio.CheckBoxes = True
        lvAudio.ShowItemToolTips = True
        lvAudio.MultiSelect = False
        lvAudio.SendMessageHideFocus()
        lvAudio.AutoCheckMode = AutoCheckMode.SingleClick

        lvSubtitles.View = View.SmallIcon
        lvSubtitles.CheckBoxes = True
        lvSubtitles.HeaderStyle = ColumnHeaderStyle.None
        lvSubtitles.AutoCheckMode = AutoCheckMode.SingleClick

        Text = "Demux " + My.Application.Info.Version.ToString.TrimEnd(".0".ToCharArray)
    End Sub

    <STAThread()>
    Shared Sub Main()
        Native.SetProcessDPIAware()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New MainForm())
    End Sub

    Function ExistsMkvToolnixDir() As Boolean
        Return File.Exists(Settings.MkvToolnixDir + "mkvmerge.exe") AndAlso
            File.Exists(Settings.MkvToolnixDir + "mkvextract.exe")
    End Function

    Function GetMP4BoxPath() As String
        Dim ret = Settings.MP4BoxPath

        If Not File.Exists(ret) Then
            Dim names = Registry.CurrentUser.GetValueNames("SOFTWARE\StaxRip\SettingsLocation")

            If Not names.NothingOrEmpty AndAlso Directory.Exists(names(0)) Then
                ret = names(0).AppendSeparator + "Apps\MP4Box\MP4Box.exe"
            End If
        End If

        If File.Exists(ret) Then Return ret
    End Function

    Function GetMkvToolnixDir() As String
        If Not ExistsMkvToolnixDir() Then
            Settings.MkvToolnixDir = Registry.CurrentUser.GetString(
                "SOFTWARE\mkvmergeGUI\GUI", "mkvmerge_executable").Dir

            If Not ExistsMkvToolnixDir() Then
                Dim names = Registry.CurrentUser.GetValueNames("SOFTWARE\StaxRip\SettingsLocation")

                If Not names.NothingOrEmpty AndAlso Directory.Exists(names(0)) Then
                    Settings.MkvToolnixDir = names(0).AppendSeparator + "Apps\MKVToolNix\"
                End If

                If Not ExistsMkvToolnixDir() Then
                    Using d As New FolderBrowserDialog
                        d.Description = "Select the mkvtoolnix folder containing mkvmerge and mkvextract."
                        If d.ShowDialog = DialogResult.OK Then Settings.MkvToolnixDir = d.SelectedPath.AppendSeparator
                    End Using
                End If
            End If
        End If

        If ExistsMkvToolnixDir() Then Return Settings.MkvToolnixDir
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
            Dim stream = DirectCast(i.Tag, AudioStream)
            If stream.Language.TwoLetter = "en" Then i.Checked = True
        Next
    End Sub

    Private Sub bnAudioNative_Click(sender As Object, e As EventArgs) Handles bnAudioNative.Click
        For Each i As ListViewItem In lvAudio.Items
            Dim stream = DirectCast(i.Tag, AudioStream)

            If stream.Language.TwoLetter = Language.Current.TwoLetter Then
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
            Dim stream = DirectCast(i.Tag, Subtitle)
            If stream.Language.TwoLetter = "en" Then i.Checked = True
        Next
    End Sub

    Private Sub bnSubtitleNative_Click(sender As Object, e As EventArgs) Handles bnSubtitleNative.Click
        For Each i As ListViewItem In lvSubtitles.Items
            Dim stream = DirectCast(i.Tag, Subtitle)

            If stream.Language.TwoLetter = Language.Current.TwoLetter Then
                i.Checked = True
            End If
        Next
    End Sub

    Private Sub bnSourceFile_Click(sender As Object, e As EventArgs) Handles bnSourceFile.Click
        Using d As New OpenFileDialog
            If d.ShowDialog = DialogResult.OK Then tbSourceFile.Text = d.FileName
        End Using
    End Sub

    Private Sub tbSourceFile_TextChanged(sender As Object, e As EventArgs) Handles tbSourceFile.TextChanged
        If Not File.Exists(tbSourceFile.Text) Then
            tbSourceFile.Text = ""
            tbTargetDir.Text = ""
            lvAudio.Items.Clear()
            lvSubtitles.Items.Clear()
            Exit Sub
        End If

        If Not tbSourceFile.Text.Ext.EqualsAny("mkv", "mp4") Then
            tbSourceFile.Text = ""
            tbTargetDir.Text = ""
            lvAudio.Items.Clear()
            lvSubtitles.Items.Clear()
            MsgBox("Only MKV and MP4 is supported right now.", MsgBoxStyle.Critical)
            Exit Sub
        End If

        SourceFile = tbSourceFile.Text
        tbTargetDir.Text = SourceFile.Dir
        lvAudio.Items.Clear()
        lvSubtitles.Items.Clear()

        AudioStreams = MediaInfo.GetAudioStreams(SourceFile)
        Subtitles = MediaInfo.GetSubtitles(SourceFile)

        gbAudio.Enabled = AudioStreams.Count > 0
        gbSubtitles.Enabled = Subtitles.Count > 0

        bnAudioEnglish.Enabled = AudioStreams.Where(Function(stream) stream.Language.TwoLetter = "en").Count > 0
        bnAudioNative.Visible = Language.Current.TwoLetter <> "en"
        bnAudioNative.Text = CultureInfo.CurrentCulture.NeutralCulture.EnglishName
        bnAudioNative.Enabled = AudioStreams.Where(Function(stream) stream.Language.TwoLetter = CultureInfo.CurrentCulture.TwoLetterISOLanguageName).Count > 0

        bnSubtitleEnglish.Enabled = Subtitles.Where(Function(stream) stream.Language.TwoLetter = "en").Count > 0
        bnSubtitleNative.Visible = Language.Current.TwoLetter <> "en"
        bnSubtitleNative.Text = CultureInfo.CurrentCulture.NeutralCulture.EnglishName
        bnSubtitleNative.Enabled = Subtitles.Where(Function(stream) stream.Language.TwoLetter = CultureInfo.CurrentCulture.TwoLetterISOLanguageName).Count > 0

        For Each i In AudioStreams
            i.Enabled = False

            Dim item = lvAudio.Items.Add(i.Name)
            item.Tag = i
        Next

        For Each i In Subtitles
            Dim text = i.Language.ToString
            If Subtitles.Count <= 12 Then text += " (" + i.TypeName + ")"
            Dim item = lvSubtitles.Items.Add(text)
            item.Tag = i
        Next
    End Sub

    Private Sub tbTargetDir_TextChanged(sender As Object, e As EventArgs) Handles tbTargetDir.TextChanged
        TargetDir = tbTargetDir.Text.AppendSeparator
    End Sub

    Private Sub bnTargetDir_Click(sender As Object, e As EventArgs) Handles bnTargetDir.Click
        Using d As New FolderBrowserDialog
            d.SetSelectedPath(TargetDir)
            If d.ShowDialog = DialogResult.OK Then tbTargetDir.Text = d.SelectedPath
        End Using
    End Sub

    Private Sub DemuxForm_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        tbSourceFile.Text = TryCast(e.Data.GetData(DataFormats.FileDrop), String())?(0)
    End Sub

    Private Sub DemuxForm_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
        If Not files.NothingOrEmpty Then e.Effect = DragDropEffects.Copy
    End Sub

    Function TooLong(path As String) As Boolean
        If path.Length >= 260 Then
            MsgBox("Filepath too long, aborting.", MsgBoxStyle.Critical)
            Return True
        End If
    End Function

    Private Sub bnDemux_Click(sender As Object, e As EventArgs) Handles bnDemux.Click
        If Not SourceFile.Ext.EqualsAny("mkv", "mp4") Then
            MsgBox("Open a MKV or MP4 file first.", MsgBoxStyle.Critical)
            Exit Sub
        End If

        Select Case SourceFile.Ext
            Case "mkv"
                If GetMkvToolnixDir() = "" Then Exit Sub
                Dim args = "tracks " + SourceFile.Quotes

                For Each audioStream In AudioStreams
                    If Not audioStream.Enabled Then Continue For
                    Dim outPath = TargetDir + SourceFile.Base + " " + audioStream.Name.RemoveIllegal + audioStream.Extension
                    If TooLong(outPath) Then Exit Sub
                    args += " " & audioStream.StreamOrder & ":" + outPath.Quotes
                Next

                For Each subtitleStream In Subtitles
                    If Not subtitleStream.Enabled Then Continue For
                    Dim outPath = TargetDir + SourceFile.Base + " " + subtitleStream.Filename + subtitleStream.Extension
                    If TooLong(outPath) Then Exit Sub
                    args += " " & subtitleStream.StreamOrder & ":" + outPath.Quotes
                Next

                If cbVideo.Checked Then
                    Dim videoStreams = MediaInfo.GetMediaInfo(SourceFile).VideoStreams

                    If videoStreams.Count > 0 Then
                        For Each videoStream In videoStreams
                            Dim outPath = TargetDir + SourceFile.Base + " ID" & videoStream.StreamOrder + 1 & videoStream.Extension
                            If TooLong(outPath) Then Exit Sub
                            args += " " & videoStream.StreamOrder & ":" + outPath.Quotes
                        Next
                    End If
                End If

                Process.Start(Settings.MkvToolnixDir + "mkvextract.exe", args + " --ui-language en")

                Dim output = GetStdOut(GetMkvToolnixDir() + "mkvmerge.exe", "--identify-verbose --ui-language en " + SourceFile.Quotes)

                If output.Contains("Chapters: ") Then
                    Dim outPath = TargetDir + SourceFile.Base + "_Chapters.xml"
                    If TooLong(outPath) Then Exit Sub
                    Process.Start(GetMkvToolnixDir() + "mkvextract.exe", "chapters " +
                                  SourceFile.Quotes + " --redirect-output " + outPath.Quotes)
                End If

                Dim params As String

                For Each i In output.SplitLinesNoEmpty
                    If i.StartsWith("Attachment ID ") Then
                        Dim match = Regex.Match(i, "Attachment ID (\d+):.+, file name '(.+)'")

                        If match.Success Then
                            Dim outPath = TargetDir + SourceFile.Base + "_attachment_" + match.Groups(2).Value.FileName
                            If TooLong(outPath) Then Exit Sub
                            params += " " + match.Groups(1).Value + ":" + outPath.Quotes
                        End If
                    End If
                Next

                If params <> "" Then
                    Process.Start(GetMkvToolnixDir() + "mkvextract.exe",
                                  "attachments " + SourceFile.Quotes + params + " --ui-language en")
                End If
            Case "mp4"
                If GetMP4BoxPath() = "" Then Exit Sub

                For Each stream In AudioStreams
                    If Not stream.Enabled Then Continue For
                    Dim outPath = TargetDir + SourceFile.Base + " " + stream.Name.RemoveIllegal + stream.Extension
                    If TooLong(outPath) Then Exit Sub
                    If File.Exists(outPath) Then File.Delete(outPath)
                    Dim args As String
                    If stream.Format = "AAC" Then args += "-single" Else args += "-raw"
                    args += " " & stream.ID & " -out " + outPath.Quotes + " " + SourceFile.Quotes
                    Process.Start(GetMP4BoxPath, args)
                Next

                For Each stream In Subtitles
                    If Not stream.Enabled Then Continue For
                    Dim outpath = TargetDir + SourceFile.Base + " " + stream.Filename + stream.Extension
                    If TooLong(outpath) Then Exit Sub
                    If File.Exists(outpath) Then File.Delete(outpath)
                    Dim args As String

                    Select Case stream.Extension
                        Case ""
                            Continue For
                        Case ".srt"
                            args = "-srt "
                        Case Else
                            args = "-raw "
                    End Select

                    args += stream.ID & " -out " + outpath.Quotes + " " + SourceFile.Quotes
                    Process.Start(GetMP4BoxPath, args)
                Next

                If cbVideo.Checked Then MsgBox("Video demuxing is currently not implented for MP4.", MsgBoxStyle.Critical)
                If cbChapters.Checked Then MsgBox("Chapter demuxing is currently not implented for MP4.", MsgBoxStyle.Critical)
                If cbAttachments.Checked Then MsgBox("Attachment demuxing is currently not implented for MP4.", MsgBoxStyle.Critical)
        End Select
    End Sub

    Function GetStdOut(file As String, arguments As String) As String
        Dim ret = ""
        Dim proc As New Process
        proc.StartInfo.UseShellExecute = False
        proc.StartInfo.CreateNoWindow = True
        proc.StartInfo.RedirectStandardOutput = True
        proc.StartInfo.FileName = file
        proc.StartInfo.Arguments = arguments
        proc.Start()
        ret = proc.StandardOutput.ReadToEnd()
        proc.WaitForExit()
        Return ret
    End Function

    Sub FindUnusedMethods()
        Dim funcList As New List(Of String)

        For Each i In Directory.GetFiles("..\", "*.vb", SearchOption.AllDirectories)
            For Each line In File.ReadAllText(i).SplitLinesNoEmpty
                If Not line.Contains(" Handles ") Then
                    Dim match = Regex.Match(line, "(Sub|Function) (\w+)\(")
                    If match.Success Then funcList.Add(match.Groups(2).Value)
                End If
            Next
        Next

        Dim codeSB As New StringBuilder

        For Each i In Directory.GetFiles("..\", "*.vb", SearchOption.AllDirectories)
            For Each line In File.ReadAllText(i).SplitLinesNoEmpty
                If Not line.Contains(" Sub ") AndAlso
                    Not line.Contains(" Function ") AndAlso
                    Not line.Contains(" Handles ") Then

                    codeSB.AppendLine(line)
                End If
            Next
        Next

        Dim code = codeSB.ToString

        For Each func In funcList
            If Not code.Contains(func) Then Debug.WriteLine(func)
        Next
    End Sub
End Class