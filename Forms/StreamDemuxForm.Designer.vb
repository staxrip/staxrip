Imports StaxRip.UI

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class StreamDemuxForm
    Inherits DialogBase

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.gbAudio = New System.Windows.Forms.GroupBox()
        Me.AudioTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.lvAudio = New StaxRip.UI.ListViewEx()
        Me.flpAudioButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnAudioAll = New StaxRip.UI.ButtonEx()
        Me.bnAudioNone = New StaxRip.UI.ButtonEx()
        Me.bnAudioEnglish = New StaxRip.UI.ButtonEx()
        Me.bnAudioNative = New StaxRip.UI.ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.gbSubtitles = New System.Windows.Forms.GroupBox()
        Me.SubtitleTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.lvSubtitles = New StaxRip.UI.ListViewEx()
        Me.flpSubtitleButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnSubtitleAll = New StaxRip.UI.ButtonEx()
        Me.bnSubtitleNone = New StaxRip.UI.ButtonEx()
        Me.bnSubtitleEnglish = New StaxRip.UI.ButtonEx()
        Me.bnSubtitleNative = New StaxRip.UI.ButtonEx()
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.gbAttachments = New System.Windows.Forms.GroupBox()
        Me.lvAttachments = New StaxRip.UI.ListViewEx()
        Me.gbAudio.SuspendLayout()
        Me.AudioTableLayoutPanel.SuspendLayout()
        Me.flpAudioButtons.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.gbSubtitles.SuspendLayout()
        Me.SubtitleTableLayoutPanel.SuspendLayout()
        Me.flpSubtitleButtons.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.gbAttachments.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbAudio
        '
        Me.gbAudio.Controls.Add(Me.AudioTableLayoutPanel)
        Me.gbAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbAudio.Location = New System.Drawing.Point(16, 0)
        Me.gbAudio.Margin = New System.Windows.Forms.Padding(5, 0, 5, 5)
        Me.gbAudio.Name = "gbAudio"
        Me.gbAudio.Padding = New System.Windows.Forms.Padding(5)
        Me.gbAudio.Size = New System.Drawing.Size(789, 288)
        Me.gbAudio.TabIndex = 24
        Me.gbAudio.TabStop = False
        Me.gbAudio.Text = "Audio"
        '
        'AudioTableLayoutPanel
        '
        Me.AudioTableLayoutPanel.ColumnCount = 1
        Me.AudioTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.AudioTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.AudioTableLayoutPanel.Controls.Add(Me.lvAudio, 0, 0)
        Me.AudioTableLayoutPanel.Controls.Add(Me.flpAudioButtons, 0, 1)
        Me.AudioTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AudioTableLayoutPanel.Location = New System.Drawing.Point(5, 53)
        Me.AudioTableLayoutPanel.Margin = New System.Windows.Forms.Padding(5)
        Me.AudioTableLayoutPanel.Name = "AudioTableLayoutPanel"
        Me.AudioTableLayoutPanel.RowCount = 2
        Me.AudioTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.AudioTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.AudioTableLayoutPanel.Size = New System.Drawing.Size(779, 230)
        Me.AudioTableLayoutPanel.TabIndex = 19
        '
        'lvAudio
        '
        Me.lvAudio.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvAudio.Location = New System.Drawing.Point(0, 0)
        Me.lvAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.lvAudio.Name = "lvAudio"
        Me.lvAudio.Size = New System.Drawing.Size(779, 158)
        Me.lvAudio.TabIndex = 8
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
        Me.flpAudioButtons.Location = New System.Drawing.Point(0, 158)
        Me.flpAudioButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAudioButtons.Name = "flpAudioButtons"
        Me.flpAudioButtons.Size = New System.Drawing.Size(763, 72)
        Me.flpAudioButtons.TabIndex = 18
        '
        'bnAudioAll
        '
        Me.bnAudioAll.Location = New System.Drawing.Point(0, 5)
        Me.bnAudioAll.Margin = New System.Windows.Forms.Padding(0, 5, 5, 5)
        Me.bnAudioAll.Size = New System.Drawing.Size(182, 62)
        Me.bnAudioAll.Text = "All"
        '
        'bnAudioNone
        '
        Me.bnAudioNone.Location = New System.Drawing.Point(192, 5)
        Me.bnAudioNone.Margin = New System.Windows.Forms.Padding(5)
        Me.bnAudioNone.Size = New System.Drawing.Size(182, 62)
        Me.bnAudioNone.Text = "None"
        '
        'bnAudioEnglish
        '
        Me.bnAudioEnglish.Location = New System.Drawing.Point(384, 5)
        Me.bnAudioEnglish.Margin = New System.Windows.Forms.Padding(5)
        Me.bnAudioEnglish.Size = New System.Drawing.Size(182, 62)
        Me.bnAudioEnglish.Text = "English"
        '
        'bnAudioNative
        '
        Me.bnAudioNative.Location = New System.Drawing.Point(576, 5)
        Me.bnAudioNative.Margin = New System.Windows.Forms.Padding(5)
        Me.bnAudioNative.Size = New System.Drawing.Size(182, 62)
        Me.bnAudioNative.Text = "Native"
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.gbSubtitles, 0, 1)
        Me.tlpMain.Controls.Add(Me.gbAudio, 0, 0)
        Me.tlpMain.Controls.Add(Me.FlowLayoutPanel2, 0, 3)
        Me.tlpMain.Controls.Add(Me.gbAttachments, 0, 2)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.Padding = New System.Windows.Forms.Padding(11, 0, 11, 0)
        Me.tlpMain.RowCount = 4
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(821, 677)
        Me.tlpMain.TabIndex = 25
        '
        'gbSubtitles
        '
        Me.gbSubtitles.Controls.Add(Me.SubtitleTableLayoutPanel)
        Me.gbSubtitles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbSubtitles.Location = New System.Drawing.Point(16, 298)
        Me.gbSubtitles.Margin = New System.Windows.Forms.Padding(5)
        Me.gbSubtitles.Name = "gbSubtitles"
        Me.gbSubtitles.Padding = New System.Windows.Forms.Padding(5)
        Me.gbSubtitles.Size = New System.Drawing.Size(789, 165)
        Me.gbSubtitles.TabIndex = 25
        Me.gbSubtitles.TabStop = False
        Me.gbSubtitles.Text = "Subtitles"
        '
        'SubtitleTableLayoutPanel
        '
        Me.SubtitleTableLayoutPanel.ColumnCount = 1
        Me.SubtitleTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.SubtitleTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.SubtitleTableLayoutPanel.Controls.Add(Me.lvSubtitles, 0, 0)
        Me.SubtitleTableLayoutPanel.Controls.Add(Me.flpSubtitleButtons, 0, 1)
        Me.SubtitleTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SubtitleTableLayoutPanel.Location = New System.Drawing.Point(5, 53)
        Me.SubtitleTableLayoutPanel.Margin = New System.Windows.Forms.Padding(5)
        Me.SubtitleTableLayoutPanel.Name = "SubtitleTableLayoutPanel"
        Me.SubtitleTableLayoutPanel.RowCount = 2
        Me.SubtitleTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.SubtitleTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.SubtitleTableLayoutPanel.Size = New System.Drawing.Size(779, 107)
        Me.SubtitleTableLayoutPanel.TabIndex = 20
        '
        'lvSubtitles
        '
        Me.lvSubtitles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvSubtitles.Location = New System.Drawing.Point(0, 0)
        Me.lvSubtitles.Margin = New System.Windows.Forms.Padding(0)
        Me.lvSubtitles.Name = "lvSubtitles"
        Me.lvSubtitles.Size = New System.Drawing.Size(779, 35)
        Me.lvSubtitles.TabIndex = 9
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
        Me.flpSubtitleButtons.Location = New System.Drawing.Point(0, 35)
        Me.flpSubtitleButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flpSubtitleButtons.Name = "flpSubtitleButtons"
        Me.flpSubtitleButtons.Size = New System.Drawing.Size(763, 72)
        Me.flpSubtitleButtons.TabIndex = 19
        '
        'bnSubtitleAll
        '
        Me.bnSubtitleAll.Location = New System.Drawing.Point(0, 5)
        Me.bnSubtitleAll.Margin = New System.Windows.Forms.Padding(0, 5, 5, 5)
        Me.bnSubtitleAll.Size = New System.Drawing.Size(182, 62)
        Me.bnSubtitleAll.Text = "All"
        '
        'bnSubtitleNone
        '
        Me.bnSubtitleNone.Location = New System.Drawing.Point(192, 5)
        Me.bnSubtitleNone.Margin = New System.Windows.Forms.Padding(5)
        Me.bnSubtitleNone.Size = New System.Drawing.Size(182, 62)
        Me.bnSubtitleNone.Text = "None"
        '
        'bnSubtitleEnglish
        '
        Me.bnSubtitleEnglish.Location = New System.Drawing.Point(384, 5)
        Me.bnSubtitleEnglish.Margin = New System.Windows.Forms.Padding(5)
        Me.bnSubtitleEnglish.Size = New System.Drawing.Size(182, 62)
        Me.bnSubtitleEnglish.Text = "English"
        '
        'bnSubtitleNative
        '
        Me.bnSubtitleNative.Location = New System.Drawing.Point(576, 5)
        Me.bnSubtitleNative.Margin = New System.Windows.Forms.Padding(5)
        Me.bnSubtitleNative.Size = New System.Drawing.Size(182, 62)
        Me.bnSubtitleNative.Text = "Native"
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.FlowLayoutPanel2.AutoSize = True
        Me.FlowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel2.Controls.Add(Me.bnOK)
        Me.FlowLayoutPanel2.Controls.Add(Me.bnCancel)
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(426, 590)
        Me.FlowLayoutPanel2.Margin = New System.Windows.Forms.Padding(5, 5, 5, 14)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(379, 72)
        Me.FlowLayoutPanel2.TabIndex = 26
        '
        'bnOK
        '
        Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(5, 5)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(5)
        Me.bnOK.Size = New System.Drawing.Size(182, 62)
        Me.bnOK.Text = "OK"
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(197, 5)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(5, 5, 0, 5)
        Me.bnCancel.Size = New System.Drawing.Size(182, 62)
        Me.bnCancel.Text = "Cancel"
        '
        'gbAttachments
        '
        Me.gbAttachments.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbAttachments.Controls.Add(Me.lvAttachments)
        Me.gbAttachments.Location = New System.Drawing.Point(15, 471)
        Me.gbAttachments.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.gbAttachments.Name = "gbAttachments"
        Me.gbAttachments.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.gbAttachments.Size = New System.Drawing.Size(791, 111)
        Me.gbAttachments.TabIndex = 27
        Me.gbAttachments.TabStop = False
        Me.gbAttachments.Text = "Attachments"
        '
        'lvAttachments
        '
        Me.lvAttachments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvAttachments.Location = New System.Drawing.Point(4, 51)
        Me.lvAttachments.Margin = New System.Windows.Forms.Padding(0)
        Me.lvAttachments.Name = "lvAttachments"
        Me.lvAttachments.Size = New System.Drawing.Size(783, 57)
        Me.lvAttachments.TabIndex = 0
        Me.lvAttachments.UseCompatibleStateImageBehavior = False
        '
        'StreamDemuxForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(821, 677)
        Me.Controls.Add(Me.tlpMain)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(11, 10, 11, 10)
        Me.Name = "StreamDemuxForm"
        Me.Text = "Size"
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
        Me.gbAttachments.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents gbAudio As GroupBox
    Friend WithEvents flpAudioButtons As FlowLayoutPanel
    Friend WithEvents bnAudioAll As ButtonEx
    Friend WithEvents bnAudioNone As ButtonEx
    Friend WithEvents bnAudioEnglish As ButtonEx
    Friend WithEvents bnAudioNative As ButtonEx
    Friend WithEvents lvAudio As ListViewEx
    Friend WithEvents AudioTableLayoutPanel As TableLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents gbSubtitles As GroupBox
    Friend WithEvents flpSubtitleButtons As FlowLayoutPanel
    Friend WithEvents bnSubtitleAll As ButtonEx
    Friend WithEvents bnSubtitleNone As ButtonEx
    Friend WithEvents bnSubtitleEnglish As ButtonEx
    Friend WithEvents bnSubtitleNative As ButtonEx
    Friend WithEvents lvSubtitles As ListViewEx
    Friend WithEvents FlowLayoutPanel2 As FlowLayoutPanel
    Friend WithEvents bnOK As ButtonEx
    Friend WithEvents bnCancel As ButtonEx
    Friend WithEvents SubtitleTableLayoutPanel As TableLayoutPanel
    Friend WithEvents gbAttachments As GroupBox
    Friend WithEvents lvAttachments As ListViewEx
End Class
