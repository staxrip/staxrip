Imports StaxRip.UI

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class StreamDemuxForm
    Inherits DialogBase

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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.gbAudio = New StaxRip.UI.GroupBoxEx()
        Me.AudioTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.lvAudio = New StaxRip.UI.ListViewEx()
        Me.flpAudioButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnAudioAll = New StaxRip.UI.ButtonEx()
        Me.bnAudioNone = New StaxRip.UI.ButtonEx()
        Me.bnAudioEnglish = New StaxRip.UI.ButtonEx()
        Me.bnAudioNative = New StaxRip.UI.ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.gbSubtitles = New StaxRip.UI.GroupBoxEx()
        Me.SubtitleTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.lvSubtitles = New StaxRip.UI.ListViewEx()
        Me.flpSubtitleButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnSubtitleAll = New StaxRip.UI.ButtonEx()
        Me.bnSubtitleNone = New StaxRip.UI.ButtonEx()
        Me.bnSubtitleEnglish = New StaxRip.UI.ButtonEx()
        Me.bnSubtitleNative = New StaxRip.UI.ButtonEx()
        Me.gbAttachments = New StaxRip.UI.GroupBoxEx()
        Me.AttachmentsTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.AttachmentButtonsFlowLayoutPanel = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnAllAttachments = New StaxRip.UI.ButtonEx()
        Me.bnNoneAttachments = New StaxRip.UI.ButtonEx()
        Me.lvAttachments = New StaxRip.UI.ListViewEx()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.cbDemuxVideo = New StaxRip.UI.CheckBoxEx()
        Me.cbDemuxChapters = New StaxRip.UI.CheckBoxEx()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.gbAudio.SuspendLayout()
        Me.AudioTableLayoutPanel.SuspendLayout()
        Me.flpAudioButtons.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.gbSubtitles.SuspendLayout()
        Me.SubtitleTableLayoutPanel.SuspendLayout()
        Me.flpSubtitleButtons.SuspendLayout()
        Me.gbAttachments.SuspendLayout()
        Me.AttachmentsTableLayoutPanel.SuspendLayout()
        Me.AttachmentButtonsFlowLayoutPanel.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbAudio
        '
        Me.gbAudio.Controls.Add(Me.AudioTableLayoutPanel)
        Me.gbAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbAudio.Location = New System.Drawing.Point(15, 0)
        Me.gbAudio.Margin = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.gbAudio.Name = "gbAudio"
        Me.gbAudio.Padding = New System.Windows.Forms.Padding(10, 0, 10, 10)
        Me.gbAudio.Size = New System.Drawing.Size(1213, 344)
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
        Me.AudioTableLayoutPanel.Location = New System.Drawing.Point(10, 48)
        Me.AudioTableLayoutPanel.Name = "AudioTableLayoutPanel"
        Me.AudioTableLayoutPanel.RowCount = 2
        Me.AudioTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.AudioTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.AudioTableLayoutPanel.Size = New System.Drawing.Size(1193, 286)
        Me.AudioTableLayoutPanel.TabIndex = 19
        '
        'lvAudio
        '
        Me.lvAudio.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvAudio.HideSelection = False
        Me.lvAudio.Location = New System.Drawing.Point(0, 0)
        Me.lvAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.lvAudio.Name = "lvAudio"
        Me.lvAudio.OwnerDraw = True
        Me.lvAudio.Size = New System.Drawing.Size(1193, 216)
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
        Me.flpAudioButtons.Location = New System.Drawing.Point(0, 226)
        Me.flpAudioButtons.Margin = New System.Windows.Forms.Padding(0, 10, 0, 0)
        Me.flpAudioButtons.Name = "flpAudioButtons"
        Me.flpAudioButtons.Size = New System.Drawing.Size(845, 60)
        Me.flpAudioButtons.TabIndex = 18
        '
        'bnAudioAll
        '
        Me.bnAudioAll.Location = New System.Drawing.Point(0, 0)
        Me.bnAudioAll.Margin = New System.Windows.Forms.Padding(0)
        Me.bnAudioAll.Size = New System.Drawing.Size(200, 60)
        Me.bnAudioAll.Text2 = "All"
        '
        'bnAudioNone
        '
        Me.bnAudioNone.Location = New System.Drawing.Point(215, 0)
        Me.bnAudioNone.Margin = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.bnAudioNone.Size = New System.Drawing.Size(200, 60)
        Me.bnAudioNone.Text2 = "None"
        '
        'bnAudioEnglish
        '
        Me.bnAudioEnglish.Location = New System.Drawing.Point(430, 0)
        Me.bnAudioEnglish.Margin = New System.Windows.Forms.Padding(0)
        Me.bnAudioEnglish.Size = New System.Drawing.Size(200, 60)
        Me.bnAudioEnglish.Text2 = "English"
        '
        'bnAudioNative
        '
        Me.bnAudioNative.Location = New System.Drawing.Point(645, 0)
        Me.bnAudioNative.Margin = New System.Windows.Forms.Padding(15, 0, 0, 0)
        Me.bnAudioNative.Size = New System.Drawing.Size(200, 60)
        Me.bnAudioNative.Text2 = "Native"
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.gbSubtitles, 0, 1)
        Me.tlpMain.Controls.Add(Me.gbAudio, 0, 0)
        Me.tlpMain.Controls.Add(Me.gbAttachments, 0, 2)
        Me.tlpMain.Controls.Add(Me.TableLayoutPanel1, 0, 3)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 4
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 108.0!))
        Me.tlpMain.Size = New System.Drawing.Size(1243, 826)
        Me.tlpMain.TabIndex = 25
        '
        'gbSubtitles
        '
        Me.gbSubtitles.Controls.Add(Me.SubtitleTableLayoutPanel)
        Me.gbSubtitles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbSubtitles.Location = New System.Drawing.Point(15, 344)
        Me.gbSubtitles.Margin = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.gbSubtitles.Name = "gbSubtitles"
        Me.gbSubtitles.Padding = New System.Windows.Forms.Padding(10)
        Me.gbSubtitles.Size = New System.Drawing.Size(1213, 215)
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
        Me.SubtitleTableLayoutPanel.Location = New System.Drawing.Point(10, 58)
        Me.SubtitleTableLayoutPanel.Name = "SubtitleTableLayoutPanel"
        Me.SubtitleTableLayoutPanel.RowCount = 2
        Me.SubtitleTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.SubtitleTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.SubtitleTableLayoutPanel.Size = New System.Drawing.Size(1193, 147)
        Me.SubtitleTableLayoutPanel.TabIndex = 20
        '
        'lvSubtitles
        '
        Me.lvSubtitles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvSubtitles.HideSelection = False
        Me.lvSubtitles.Location = New System.Drawing.Point(0, 0)
        Me.lvSubtitles.Margin = New System.Windows.Forms.Padding(0)
        Me.lvSubtitles.Name = "lvSubtitles"
        Me.lvSubtitles.OwnerDraw = True
        Me.lvSubtitles.Size = New System.Drawing.Size(1193, 77)
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
        Me.flpSubtitleButtons.Location = New System.Drawing.Point(0, 87)
        Me.flpSubtitleButtons.Margin = New System.Windows.Forms.Padding(0, 10, 0, 0)
        Me.flpSubtitleButtons.Name = "flpSubtitleButtons"
        Me.flpSubtitleButtons.Size = New System.Drawing.Size(845, 60)
        Me.flpSubtitleButtons.TabIndex = 19
        '
        'bnSubtitleAll
        '
        Me.bnSubtitleAll.Location = New System.Drawing.Point(0, 0)
        Me.bnSubtitleAll.Margin = New System.Windows.Forms.Padding(0)
        Me.bnSubtitleAll.Size = New System.Drawing.Size(200, 60)
        Me.bnSubtitleAll.Text2 = "All"
        '
        'bnSubtitleNone
        '
        Me.bnSubtitleNone.Location = New System.Drawing.Point(215, 0)
        Me.bnSubtitleNone.Margin = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.bnSubtitleNone.Size = New System.Drawing.Size(200, 60)
        Me.bnSubtitleNone.Text2 = "None"
        '
        'bnSubtitleEnglish
        '
        Me.bnSubtitleEnglish.Location = New System.Drawing.Point(430, 0)
        Me.bnSubtitleEnglish.Margin = New System.Windows.Forms.Padding(0)
        Me.bnSubtitleEnglish.Size = New System.Drawing.Size(200, 60)
        Me.bnSubtitleEnglish.Text2 = "English"
        '
        'bnSubtitleNative
        '
        Me.bnSubtitleNative.Location = New System.Drawing.Point(645, 0)
        Me.bnSubtitleNative.Margin = New System.Windows.Forms.Padding(15, 0, 0, 0)
        Me.bnSubtitleNative.Size = New System.Drawing.Size(200, 60)
        Me.bnSubtitleNative.Text2 = "Native"
        '
        'gbAttachments
        '
        Me.gbAttachments.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbAttachments.Controls.Add(Me.AttachmentsTableLayoutPanel)
        Me.gbAttachments.Location = New System.Drawing.Point(15, 559)
        Me.gbAttachments.Margin = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.gbAttachments.Name = "gbAttachments"
        Me.gbAttachments.Padding = New System.Windows.Forms.Padding(10, 0, 10, 10)
        Me.gbAttachments.Size = New System.Drawing.Size(1213, 157)
        Me.gbAttachments.TabIndex = 27
        Me.gbAttachments.TabStop = False
        Me.gbAttachments.Text = "Attachments"
        '
        'AttachmentsTableLayoutPanel
        '
        Me.AttachmentsTableLayoutPanel.ColumnCount = 1
        Me.AttachmentsTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.AttachmentsTableLayoutPanel.Controls.Add(Me.AttachmentButtonsFlowLayoutPanel, 0, 1)
        Me.AttachmentsTableLayoutPanel.Controls.Add(Me.lvAttachments, 0, 0)
        Me.AttachmentsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AttachmentsTableLayoutPanel.Location = New System.Drawing.Point(10, 48)
        Me.AttachmentsTableLayoutPanel.Name = "AttachmentsTableLayoutPanel"
        Me.AttachmentsTableLayoutPanel.RowCount = 2
        Me.AttachmentsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.AttachmentsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.AttachmentsTableLayoutPanel.Size = New System.Drawing.Size(1193, 99)
        Me.AttachmentsTableLayoutPanel.TabIndex = 1
        '
        'AttachmentButtonsFlowLayoutPanel
        '
        Me.AttachmentButtonsFlowLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.AttachmentButtonsFlowLayoutPanel.AutoSize = True
        Me.AttachmentButtonsFlowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.AttachmentButtonsFlowLayoutPanel.Controls.Add(Me.bnAllAttachments)
        Me.AttachmentButtonsFlowLayoutPanel.Controls.Add(Me.bnNoneAttachments)
        Me.AttachmentButtonsFlowLayoutPanel.Location = New System.Drawing.Point(0, 39)
        Me.AttachmentButtonsFlowLayoutPanel.Margin = New System.Windows.Forms.Padding(0, 10, 0, 0)
        Me.AttachmentButtonsFlowLayoutPanel.Name = "AttachmentButtonsFlowLayoutPanel"
        Me.AttachmentButtonsFlowLayoutPanel.Size = New System.Drawing.Size(415, 60)
        Me.AttachmentButtonsFlowLayoutPanel.TabIndex = 20
        '
        'bnAllAttachments
        '
        Me.bnAllAttachments.Location = New System.Drawing.Point(0, 0)
        Me.bnAllAttachments.Margin = New System.Windows.Forms.Padding(0)
        Me.bnAllAttachments.Size = New System.Drawing.Size(200, 60)
        Me.bnAllAttachments.Text2 = "All"
        '
        'bnNoneAttachments
        '
        Me.bnNoneAttachments.Location = New System.Drawing.Point(215, 0)
        Me.bnNoneAttachments.Margin = New System.Windows.Forms.Padding(15, 0, 0, 0)
        Me.bnNoneAttachments.Size = New System.Drawing.Size(200, 60)
        Me.bnNoneAttachments.Text2 = "None"
        '
        'lvAttachments
        '
        Me.lvAttachments.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvAttachments.HideSelection = False
        Me.lvAttachments.Location = New System.Drawing.Point(0, 0)
        Me.lvAttachments.Margin = New System.Windows.Forms.Padding(0)
        Me.lvAttachments.Name = "lvAttachments"
        Me.lvAttachments.OwnerDraw = True
        Me.lvAttachments.Size = New System.Drawing.Size(1193, 29)
        Me.lvAttachments.TabIndex = 0
        Me.lvAttachments.UseCompatibleStateImageBehavior = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel1.ColumnCount = 5
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.Controls.Add(Me.cbDemuxVideo, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.cbDemuxChapters, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.bnCancel, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.bnOK, 3, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 719)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1237, 104)
        Me.TableLayoutPanel1.TabIndex = 28
        '
        'cbDemuxVideo
        '
        Me.cbDemuxVideo.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cbDemuxVideo.AutoSize = True
        Me.cbDemuxVideo.Location = New System.Drawing.Point(15, 26)
        Me.cbDemuxVideo.Margin = New System.Windows.Forms.Padding(15, 0, 50, 0)
        Me.cbDemuxVideo.Size = New System.Drawing.Size(280, 52)
        Me.cbDemuxVideo.Text = "Demux Video"
        Me.cbDemuxVideo.UseVisualStyleBackColor = False
        '
        'cbDemuxChapters
        '
        Me.cbDemuxChapters.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cbDemuxChapters.AutoSize = True
        Me.cbDemuxChapters.Location = New System.Drawing.Point(348, 26)
        Me.cbDemuxChapters.Size = New System.Drawing.Size(328, 52)
        Me.cbDemuxChapters.Text = "Demux Chapters"
        Me.cbDemuxChapters.UseVisualStyleBackColor = False
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(972, 17)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(15)
        Me.bnCancel.Size = New System.Drawing.Size(250, 70)
        Me.bnCancel.Text2 = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(707, 17)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(0)
        Me.bnOK.Size = New System.Drawing.Size(250, 70)
        Me.bnOK.Text2 = "OK"
        '
        'StreamDemuxForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1243, 826)
        Me.Controls.Add(Me.tlpMain)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(11, 10, 11, 10)
        Me.Name = "StreamDemuxForm"
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
        Me.gbAttachments.ResumeLayout(False)
        Me.AttachmentsTableLayoutPanel.ResumeLayout(False)
        Me.AttachmentsTableLayoutPanel.PerformLayout()
        Me.AttachmentButtonsFlowLayoutPanel.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents gbAudio As GroupBoxEx
    Friend WithEvents flpAudioButtons As FlowLayoutPanel
    Friend WithEvents bnAudioAll As ButtonEx
    Friend WithEvents bnAudioNone As ButtonEx
    Friend WithEvents bnAudioEnglish As ButtonEx
    Friend WithEvents bnAudioNative As ButtonEx
    Friend WithEvents lvAudio As ListViewEx
    Friend WithEvents AudioTableLayoutPanel As TableLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents gbSubtitles As GroupBoxEx
    Friend WithEvents flpSubtitleButtons As FlowLayoutPanel
    Friend WithEvents bnSubtitleAll As ButtonEx
    Friend WithEvents bnSubtitleNone As ButtonEx
    Friend WithEvents bnSubtitleEnglish As ButtonEx
    Friend WithEvents bnSubtitleNative As ButtonEx
    Friend WithEvents lvSubtitles As ListViewEx
    Friend WithEvents bnOK As ButtonEx
    Friend WithEvents bnCancel As ButtonEx
    Friend WithEvents SubtitleTableLayoutPanel As TableLayoutPanel
    Friend WithEvents gbAttachments As GroupBoxEx
    Friend WithEvents lvAttachments As ListViewEx
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents cbDemuxVideo As CheckBoxEx
    Friend WithEvents cbDemuxChapters As CheckBoxEx
    Friend WithEvents AttachmentsTableLayoutPanel As TableLayoutPanel
    Friend WithEvents AttachmentButtonsFlowLayoutPanel As FlowLayoutPanel
    Friend WithEvents bnAllAttachments As ButtonEx
    Friend WithEvents bnNoneAttachments As ButtonEx
End Class
