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
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
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
        Me.gbAudio.SuspendLayout()
        Me.AudioTableLayoutPanel.SuspendLayout()
        Me.flpAudioButtons.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.gbSubtitles.SuspendLayout()
        Me.SubtitleTableLayoutPanel.SuspendLayout()
        Me.flpSubtitleButtons.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbAudio
        '
        Me.gbAudio.Controls.Add(Me.AudioTableLayoutPanel)
        Me.gbAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbAudio.Location = New System.Drawing.Point(3, 0)
        Me.gbAudio.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
        Me.gbAudio.Name = "gbAudio"
        Me.gbAudio.Size = New System.Drawing.Size(932, 345)
        Me.gbAudio.TabIndex = 24
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
        Me.AudioTableLayoutPanel.Size = New System.Drawing.Size(926, 315)
        Me.AudioTableLayoutPanel.TabIndex = 19
        '
        'lvAudio
        '
        Me.lvAudio.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvAudio.Location = New System.Drawing.Point(3, 3)
        Me.lvAudio.Name = "lvAudio"
        Me.lvAudio.Size = New System.Drawing.Size(920, 261)
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
        Me.flpAudioButtons.Location = New System.Drawing.Point(3, 270)
        Me.flpAudioButtons.Name = "flpAudioButtons"
        Me.flpAudioButtons.Size = New System.Drawing.Size(424, 42)
        Me.flpAudioButtons.TabIndex = 18
        '
        'bnAudioAll
        '
        Me.bnAudioAll.Location = New System.Drawing.Point(3, 3)
        Me.bnAudioAll.Size = New System.Drawing.Size(100, 36)
        Me.bnAudioAll.Text = "All"
        '
        'bnAudioNone
        '
        Me.bnAudioNone.Location = New System.Drawing.Point(109, 3)
        Me.bnAudioNone.Size = New System.Drawing.Size(100, 36)
        Me.bnAudioNone.Text = "None"
        '
        'bnAudioEnglish
        '
        Me.bnAudioEnglish.Location = New System.Drawing.Point(215, 3)
        Me.bnAudioEnglish.Size = New System.Drawing.Size(100, 36)
        Me.bnAudioEnglish.Text = "English"
        '
        'bnAudioNative
        '
        Me.bnAudioNative.Location = New System.Drawing.Point(321, 3)
        Me.bnAudioNative.Size = New System.Drawing.Size(100, 36)
        Me.bnAudioNative.Text = "Native"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.gbSubtitles, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.gbAudio, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.FlowLayoutPanel2, 0, 2)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 3
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(938, 628)
        Me.TableLayoutPanel2.TabIndex = 25
        '
        'gbSubtitles
        '
        Me.gbSubtitles.Controls.Add(Me.SubtitleTableLayoutPanel)
        Me.gbSubtitles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbSubtitles.Location = New System.Drawing.Point(3, 351)
        Me.gbSubtitles.Name = "gbSubtitles"
        Me.gbSubtitles.Size = New System.Drawing.Size(932, 226)
        Me.gbSubtitles.TabIndex = 25
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
        Me.SubtitleTableLayoutPanel.Size = New System.Drawing.Size(926, 196)
        Me.SubtitleTableLayoutPanel.TabIndex = 20
        '
        'lvSubtitles
        '
        Me.lvSubtitles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvSubtitles.Location = New System.Drawing.Point(3, 3)
        Me.lvSubtitles.Name = "lvSubtitles"
        Me.lvSubtitles.Size = New System.Drawing.Size(920, 142)
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
        Me.flpSubtitleButtons.Location = New System.Drawing.Point(3, 151)
        Me.flpSubtitleButtons.Name = "flpSubtitleButtons"
        Me.flpSubtitleButtons.Size = New System.Drawing.Size(424, 42)
        Me.flpSubtitleButtons.TabIndex = 19
        '
        'bnSubtitleAll
        '
        Me.bnSubtitleAll.Location = New System.Drawing.Point(3, 3)
        Me.bnSubtitleAll.Size = New System.Drawing.Size(100, 36)
        Me.bnSubtitleAll.Text = "All"
        '
        'bnSubtitleNone
        '
        Me.bnSubtitleNone.Location = New System.Drawing.Point(109, 3)
        Me.bnSubtitleNone.Size = New System.Drawing.Size(100, 36)
        Me.bnSubtitleNone.Text = "None"
        '
        'bnSubtitleEnglish
        '
        Me.bnSubtitleEnglish.Location = New System.Drawing.Point(215, 3)
        Me.bnSubtitleEnglish.Size = New System.Drawing.Size(100, 36)
        Me.bnSubtitleEnglish.Text = "English"
        '
        'bnSubtitleNative
        '
        Me.bnSubtitleNative.Location = New System.Drawing.Point(321, 3)
        Me.bnSubtitleNative.Size = New System.Drawing.Size(100, 36)
        Me.bnSubtitleNative.Text = "Native"
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.FlowLayoutPanel2.AutoSize = True
        Me.FlowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel2.Controls.Add(Me.bnOK)
        Me.FlowLayoutPanel2.Controls.Add(Me.bnCancel)
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(723, 583)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(212, 42)
        Me.FlowLayoutPanel2.TabIndex = 26
        '
        'bnOK
        '
        Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(3, 3)
        Me.bnOK.Size = New System.Drawing.Size(100, 36)
        Me.bnOK.Text = "OK"
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(109, 3)
        Me.bnCancel.Size = New System.Drawing.Size(100, 36)
        Me.bnCancel.Text = "Cancel"
        '
        'StreamDemuxForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1004, 654)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Name = "StreamDemuxForm"
        Me.Text = "Demuxing"
        Me.gbAudio.ResumeLayout(False)
        Me.AudioTableLayoutPanel.ResumeLayout(False)
        Me.AudioTableLayoutPanel.PerformLayout()
        Me.flpAudioButtons.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.gbSubtitles.ResumeLayout(False)
        Me.SubtitleTableLayoutPanel.ResumeLayout(False)
        Me.SubtitleTableLayoutPanel.PerformLayout()
        Me.flpSubtitleButtons.ResumeLayout(False)
        Me.FlowLayoutPanel2.ResumeLayout(False)
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
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
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
End Class
