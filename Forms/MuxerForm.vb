
Imports StaxRip.UI

Public Class MuxerForm
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

    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents bnCommandLinePreview As System.Windows.Forms.Button
    Friend WithEvents CmdlControl As StaxRip.CommandLineControl
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents tcMain As System.Windows.Forms.TabControl
    Friend WithEvents tpCommandLine As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tpOptions As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents SimpleUI As StaxRip.SimpleUI
    Friend WithEvents tpSubtitles As TabPage
    Friend WithEvents SubtitleControl As SubtitleControl
    Friend WithEvents tpAudio As TabPage
    Friend WithEvents bnPlayAudio As ButtonEx
    Friend WithEvents bnDownAudio As ButtonEx
    Friend WithEvents bnUpAudio As ButtonEx
    Friend WithEvents bnRemoveAudio As ButtonEx
    Friend WithEvents bnAddAudio As ButtonEx
    Friend WithEvents dgvAudio As DataGridViewEx
    Friend WithEvents bnEditAudio As ButtonEx
    Friend WithEvents tlpAudio As TableLayoutPanel
    Friend WithEvents flpAudio As FlowLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents pnTab As Panel
    Friend WithEvents tpAttachments As TabPage
    Friend WithEvents tlpAttachments As TableLayoutPanel
    Friend WithEvents flpAttachments As FlowLayoutPanel
    Friend WithEvents bnAddAttachment As ButtonEx
    Friend WithEvents lbAttachments As ListBoxEx
    Friend WithEvents bnRemoveAttachment As ButtonEx
    Friend WithEvents tpTags As TabPage
    Friend WithEvents dgvTags As DataGridViewEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.CmdlControl = New StaxRip.CommandLineControl()
        Me.bnCommandLinePreview = New System.Windows.Forms.Button()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tcMain = New System.Windows.Forms.TabControl()
        Me.tpSubtitles = New System.Windows.Forms.TabPage()
        Me.SubtitleControl = New StaxRip.SubtitleControl()
        Me.tpAudio = New System.Windows.Forms.TabPage()
        Me.tlpAudio = New System.Windows.Forms.TableLayoutPanel()
        Me.flpAudio = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnAddAudio = New StaxRip.UI.ButtonEx()
        Me.bnRemoveAudio = New StaxRip.UI.ButtonEx()
        Me.bnUpAudio = New StaxRip.UI.ButtonEx()
        Me.bnDownAudio = New StaxRip.UI.ButtonEx()
        Me.bnPlayAudio = New StaxRip.UI.ButtonEx()
        Me.bnEditAudio = New StaxRip.UI.ButtonEx()
        Me.dgvAudio = New StaxRip.UI.DataGridViewEx()
        Me.tpAttachments = New System.Windows.Forms.TabPage()
        Me.tlpAttachments = New System.Windows.Forms.TableLayoutPanel()
        Me.flpAttachments = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnAddAttachment = New StaxRip.UI.ButtonEx()
        Me.bnRemoveAttachment = New StaxRip.UI.ButtonEx()
        Me.lbAttachments = New StaxRip.UI.ListBoxEx()
        Me.tpTags = New System.Windows.Forms.TabPage()
        Me.dgvTags = New StaxRip.UI.DataGridViewEx()
        Me.tpOptions = New System.Windows.Forms.TabPage()
        Me.SimpleUI = New StaxRip.SimpleUI()
        Me.tpCommandLine = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.pnTab = New System.Windows.Forms.Panel()
        Me.tcMain.SuspendLayout()
        Me.tpSubtitles.SuspendLayout()
        Me.tpAudio.SuspendLayout()
        Me.tlpAudio.SuspendLayout()
        Me.flpAudio.SuspendLayout()
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpAttachments.SuspendLayout()
        Me.tlpAttachments.SuspendLayout()
        Me.flpAttachments.SuspendLayout()
        Me.tpTags.SuspendLayout()
        CType(Me.dgvTags, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpOptions.SuspendLayout()
        Me.tpCommandLine.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.pnTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'CmdlControl
        '
        Me.CmdlControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CmdlControl.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdlControl.Location = New System.Drawing.Point(0, 48)
        Me.CmdlControl.Margin = New System.Windows.Forms.Padding(0)
        Me.CmdlControl.Name = "CmdlControl"
        Me.CmdlControl.Size = New System.Drawing.Size(1458, 577)
        Me.CmdlControl.TabIndex = 0
        '
        'bnCommandLinePreview
        '
        Me.bnCommandLinePreview.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnCommandLinePreview.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.bnCommandLinePreview.Location = New System.Drawing.Point(676, 766)
        Me.bnCommandLinePreview.Margin = New System.Windows.Forms.Padding(8)
        Me.bnCommandLinePreview.Name = "bnCommandLinePreview"
        Me.bnCommandLinePreview.Size = New System.Drawing.Size(320, 70)
        Me.bnCommandLinePreview.TabIndex = 4
        Me.bnCommandLinePreview.Text = "Command Line..."
        Me.bnCommandLinePreview.UseVisualStyleBackColor = True
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(1278, 766)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(8)
        Me.bnCancel.Size = New System.Drawing.Size(250, 70)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(1012, 766)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(8)
        Me.bnOK.Size = New System.Drawing.Size(250, 70)
        Me.bnOK.Text = "OK"
        '
        'tcMain
        '
        Me.tcMain.Controls.Add(Me.tpSubtitles)
        Me.tcMain.Controls.Add(Me.tpAudio)
        Me.tcMain.Controls.Add(Me.tpAttachments)
        Me.tcMain.Controls.Add(Me.tpTags)
        Me.tcMain.Controls.Add(Me.tpOptions)
        Me.tcMain.Controls.Add(Me.tpCommandLine)
        Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcMain.Location = New System.Drawing.Point(0, 0)
        Me.tcMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(1512, 734)
        Me.tcMain.TabIndex = 5
        '
        'tpSubtitles
        '
        Me.tpSubtitles.Controls.Add(Me.SubtitleControl)
        Me.tpSubtitles.Location = New System.Drawing.Point(12, 69)
        Me.tpSubtitles.Margin = New System.Windows.Forms.Padding(5)
        Me.tpSubtitles.Name = "tpSubtitles"
        Me.tpSubtitles.Padding = New System.Windows.Forms.Padding(5)
        Me.tpSubtitles.Size = New System.Drawing.Size(1177, 653)
        Me.tpSubtitles.TabIndex = 3
        Me.tpSubtitles.Text = " Subtitles "
        Me.tpSubtitles.UseVisualStyleBackColor = True
        '
        'SubtitleControl
        '
        Me.SubtitleControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SubtitleControl.Location = New System.Drawing.Point(5, 5)
        Me.SubtitleControl.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.SubtitleControl.Name = "SubtitleControl"
        Me.SubtitleControl.Size = New System.Drawing.Size(1167, 643)
        Me.SubtitleControl.TabIndex = 0
        '
        'tpAudio
        '
        Me.tpAudio.Controls.Add(Me.tlpAudio)
        Me.tpAudio.Location = New System.Drawing.Point(12, 69)
        Me.tpAudio.Margin = New System.Windows.Forms.Padding(5)
        Me.tpAudio.Name = "tpAudio"
        Me.tpAudio.Padding = New System.Windows.Forms.Padding(5)
        Me.tpAudio.Size = New System.Drawing.Size(1177, 653)
        Me.tpAudio.TabIndex = 4
        Me.tpAudio.Text = "   Audio   "
        Me.tpAudio.UseVisualStyleBackColor = True
        '
        'tlpAudio
        '
        Me.tlpAudio.ColumnCount = 2
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAudio.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAudio.Controls.Add(Me.flpAudio, 1, 0)
        Me.tlpAudio.Controls.Add(Me.dgvAudio, 0, 0)
        Me.tlpAudio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAudio.Location = New System.Drawing.Point(5, 5)
        Me.tlpAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpAudio.Name = "tlpAudio"
        Me.tlpAudio.RowCount = 1
        Me.tlpAudio.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAudio.Size = New System.Drawing.Size(1167, 643)
        Me.tlpAudio.TabIndex = 7
        '
        'flpAudio
        '
        Me.flpAudio.AutoSize = True
        Me.flpAudio.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAudio.Controls.Add(Me.bnAddAudio)
        Me.flpAudio.Controls.Add(Me.bnRemoveAudio)
        Me.flpAudio.Controls.Add(Me.bnUpAudio)
        Me.flpAudio.Controls.Add(Me.bnDownAudio)
        Me.flpAudio.Controls.Add(Me.bnPlayAudio)
        Me.flpAudio.Controls.Add(Me.bnEditAudio)
        Me.flpAudio.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpAudio.Location = New System.Drawing.Point(876, 0)
        Me.flpAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.flpAudio.Name = "flpAudio"
        Me.flpAudio.Size = New System.Drawing.Size(291, 516)
        Me.flpAudio.TabIndex = 6
        '
        'bnAddAudio
        '
        Me.bnAddAudio.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAddAudio.Location = New System.Drawing.Point(11, 0)
        Me.bnAddAudio.Margin = New System.Windows.Forms.Padding(11, 0, 0, 6)
        Me.bnAddAudio.Size = New System.Drawing.Size(280, 80)
        Me.bnAddAudio.Text = "Add..."
        '
        'bnRemoveAudio
        '
        Me.bnRemoveAudio.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemoveAudio.Location = New System.Drawing.Point(11, 86)
        Me.bnRemoveAudio.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnRemoveAudio.Size = New System.Drawing.Size(280, 80)
        Me.bnRemoveAudio.Text = " Remove"
        '
        'bnUpAudio
        '
        Me.bnUpAudio.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnUpAudio.Location = New System.Drawing.Point(11, 172)
        Me.bnUpAudio.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnUpAudio.Size = New System.Drawing.Size(280, 80)
        Me.bnUpAudio.Text = "Up"
        '
        'bnDownAudio
        '
        Me.bnDownAudio.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnDownAudio.Location = New System.Drawing.Point(11, 258)
        Me.bnDownAudio.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnDownAudio.Size = New System.Drawing.Size(280, 80)
        Me.bnDownAudio.Text = "Down"
        '
        'bnPlayAudio
        '
        Me.bnPlayAudio.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnPlayAudio.Location = New System.Drawing.Point(11, 344)
        Me.bnPlayAudio.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnPlayAudio.Size = New System.Drawing.Size(280, 80)
        Me.bnPlayAudio.Text = "Play"
        '
        'bnEditAudio
        '
        Me.bnEditAudio.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnEditAudio.Location = New System.Drawing.Point(11, 430)
        Me.bnEditAudio.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnEditAudio.Size = New System.Drawing.Size(280, 80)
        Me.bnEditAudio.Text = "Edit..."
        '
        'dgvAudio
        '
        Me.dgvAudio.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvAudio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAudio.Location = New System.Drawing.Point(0, 0)
        Me.dgvAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.dgvAudio.Name = "dgvAudio"
        Me.dgvAudio.RowHeadersWidth = 123
        Me.dgvAudio.RowTemplate.Height = 28
        Me.dgvAudio.Size = New System.Drawing.Size(876, 643)
        Me.dgvAudio.TabIndex = 0
        '
        'tpAttachments
        '
        Me.tpAttachments.Controls.Add(Me.tlpAttachments)
        Me.tpAttachments.Location = New System.Drawing.Point(12, 69)
        Me.tpAttachments.Name = "tpAttachments"
        Me.tpAttachments.Padding = New System.Windows.Forms.Padding(3)
        Me.tpAttachments.Size = New System.Drawing.Size(1177, 653)
        Me.tpAttachments.TabIndex = 5
        Me.tpAttachments.Text = "  Attachments  "
        Me.tpAttachments.UseVisualStyleBackColor = True
        '
        'tlpAttachments
        '
        Me.tlpAttachments.ColumnCount = 2
        Me.tlpAttachments.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAttachments.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpAttachments.Controls.Add(Me.flpAttachments, 1, 0)
        Me.tlpAttachments.Controls.Add(Me.lbAttachments, 0, 0)
        Me.tlpAttachments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpAttachments.Location = New System.Drawing.Point(3, 3)
        Me.tlpAttachments.Name = "tlpAttachments"
        Me.tlpAttachments.RowCount = 1
        Me.tlpAttachments.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpAttachments.Size = New System.Drawing.Size(1171, 647)
        Me.tlpAttachments.TabIndex = 0
        '
        'flpAttachments
        '
        Me.flpAttachments.AutoSize = True
        Me.flpAttachments.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAttachments.Controls.Add(Me.bnAddAttachment)
        Me.flpAttachments.Controls.Add(Me.bnRemoveAttachment)
        Me.flpAttachments.Location = New System.Drawing.Point(877, 3)
        Me.flpAttachments.Name = "flpAttachments"
        Me.flpAttachments.Size = New System.Drawing.Size(291, 172)
        Me.flpAttachments.TabIndex = 0
        '
        'bnAddAttachment
        '
        Me.bnAddAttachment.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAddAttachment.Location = New System.Drawing.Point(11, 0)
        Me.bnAddAttachment.Margin = New System.Windows.Forms.Padding(11, 0, 0, 6)
        Me.bnAddAttachment.Size = New System.Drawing.Size(280, 80)
        Me.bnAddAttachment.Text = "Add..."
        '
        'bnRemoveAttachment
        '
        Me.bnRemoveAttachment.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemoveAttachment.Location = New System.Drawing.Point(11, 86)
        Me.bnRemoveAttachment.Margin = New System.Windows.Forms.Padding(11, 0, 0, 6)
        Me.bnRemoveAttachment.Size = New System.Drawing.Size(280, 80)
        Me.bnRemoveAttachment.Text = "Remove"
        '
        'lbAttachments
        '
        Me.lbAttachments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbAttachments.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.lbAttachments.FormattingEnabled = True
        Me.lbAttachments.ItemHeight = 67
        Me.lbAttachments.Location = New System.Drawing.Point(3, 3)
        Me.lbAttachments.Name = "lbAttachments"
        Me.lbAttachments.Size = New System.Drawing.Size(868, 641)
        Me.lbAttachments.TabIndex = 1
        '
        'tpTags
        '
        Me.tpTags.Controls.Add(Me.dgvTags)
        Me.tpTags.Location = New System.Drawing.Point(12, 69)
        Me.tpTags.Name = "tpTags"
        Me.tpTags.Padding = New System.Windows.Forms.Padding(3)
        Me.tpTags.Size = New System.Drawing.Size(1177, 653)
        Me.tpTags.TabIndex = 6
        Me.tpTags.Text = "  Tags"
        Me.tpTags.UseVisualStyleBackColor = True
        '
        'dgvTags
        '
        Me.dgvTags.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTags.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvTags.Location = New System.Drawing.Point(3, 3)
        Me.dgvTags.Name = "dgvTags"
        Me.dgvTags.RowHeadersWidth = 123
        Me.dgvTags.RowTemplate.Height = 46
        Me.dgvTags.Size = New System.Drawing.Size(1171, 647)
        Me.dgvTags.TabIndex = 0
        '
        'tpOptions
        '
        Me.tpOptions.Controls.Add(Me.SimpleUI)
        Me.tpOptions.Location = New System.Drawing.Point(12, 69)
        Me.tpOptions.Margin = New System.Windows.Forms.Padding(5)
        Me.tpOptions.Name = "tpOptions"
        Me.tpOptions.Padding = New System.Windows.Forms.Padding(5)
        Me.tpOptions.Size = New System.Drawing.Size(1177, 653)
        Me.tpOptions.TabIndex = 2
        Me.tpOptions.Text = "  Options  "
        Me.tpOptions.UseVisualStyleBackColor = True
        '
        'SimpleUI
        '
        Me.SimpleUI.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SimpleUI.FormSizeScaleFactor = New System.Drawing.SizeF(0!, 0!)
        Me.SimpleUI.Location = New System.Drawing.Point(5, 5)
        Me.SimpleUI.Margin = New System.Windows.Forms.Padding(5)
        Me.SimpleUI.Name = "SimpleUI"
        Me.SimpleUI.Size = New System.Drawing.Size(1167, 643)
        Me.SimpleUI.TabIndex = 0
        Me.SimpleUI.Text = "SimpleUI1"
        '
        'tpCommandLine
        '
        Me.tpCommandLine.Controls.Add(Me.TableLayoutPanel1)
        Me.tpCommandLine.Location = New System.Drawing.Point(12, 69)
        Me.tpCommandLine.Margin = New System.Windows.Forms.Padding(5)
        Me.tpCommandLine.Name = "tpCommandLine"
        Me.tpCommandLine.Padding = New System.Windows.Forms.Padding(15, 14, 15, 14)
        Me.tpCommandLine.Size = New System.Drawing.Size(1488, 653)
        Me.tpCommandLine.TabIndex = 1
        Me.tpCommandLine.Text = "  Command Line  "
        Me.tpCommandLine.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.CmdlControl, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(15, 14)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(5)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1458, 625)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 0)
        Me.Label1.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(460, 48)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Additional custom switches:"
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 3
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.Controls.Add(Me.bnCancel, 2, 1)
        Me.tlpMain.Controls.Add(Me.bnOK, 1, 1)
        Me.tlpMain.Controls.Add(Me.bnCommandLinePreview, 0, 1)
        Me.tlpMain.Controls.Add(Me.pnTab, 0, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.Padding = New System.Windows.Forms.Padding(8)
        Me.tlpMain.RowCount = 2
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1544, 852)
        Me.tlpMain.TabIndex = 8
        '
        'pnTab
        '
        Me.tlpMain.SetColumnSpan(Me.pnTab, 3)
        Me.pnTab.Controls.Add(Me.tcMain)
        Me.pnTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnTab.Location = New System.Drawing.Point(16, 16)
        Me.pnTab.Margin = New System.Windows.Forms.Padding(8)
        Me.pnTab.Name = "pnTab"
        Me.pnTab.Size = New System.Drawing.Size(1512, 734)
        Me.pnTab.TabIndex = 8
        '
        'MuxerForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1544, 852)
        Me.Controls.Add(Me.tlpMain)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(11, 10, 11, 10)
        Me.Name = "MuxerForm"
        Me.Text = "Container"
        Me.tcMain.ResumeLayout(False)
        Me.tpSubtitles.ResumeLayout(False)
        Me.tpAudio.ResumeLayout(False)
        Me.tlpAudio.ResumeLayout(False)
        Me.tlpAudio.PerformLayout()
        Me.flpAudio.ResumeLayout(False)
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpAttachments.ResumeLayout(False)
        Me.tlpAttachments.ResumeLayout(False)
        Me.tlpAttachments.PerformLayout()
        Me.flpAttachments.ResumeLayout(False)
        Me.tpTags.ResumeLayout(False)
        CType(Me.dgvTags, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpOptions.ResumeLayout(False)
        Me.tpCommandLine.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.tlpMain.ResumeLayout(False)
        Me.pnTab.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Muxer As Muxer
    Private AudioBindingSource As New BindingSource

    Sub New(muxer As Muxer)
        MyBase.New()
        InitializeComponent()
        ScaleClientSize(40, 20)
        Text += " - " + muxer.Name
        Me.Muxer = muxer
        SubtitleControl.AddSubtitles(muxer.Subtitles)
        CmdlControl.tb.Text = muxer.AdditionalSwitches
        lbAttachments.Items.AddRange(muxer.Attachments.Select(Function(val) New AttachmentContainer With {.Filepath = val}).ToArray)
        lbAttachments.RemoveButton = bnRemoveAttachment

        tcMain.SelectedIndex = s.Storage.GetInt("last selected muxer tab")

        dgvAudio.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvAudio.MultiSelect = False
        dgvAudio.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAudio.AllowUserToResizeRows = False
        dgvAudio.RowHeadersVisible = False
        dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvAudio.AutoGenerateColumns = False
        AudioBindingSource.DataSource = ObjectHelp.GetCopy(p.AudioTracks)
        dgvAudio.DataSource = AudioBindingSource

        dgvTags.DataSource = muxer.Tags
        dgvTags.AllowUserToAddRows = True
        dgvTags.AllowUserToDeleteRows = True
        dgvTags.Columns(0).Width = FontHeight * 10
        dgvTags.Columns(1).Width = FontHeight * 20
        dgvTags.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells

        bnAddAudio.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnRemoveAudio.Image = ImageHelp.GetSymbolImage(Symbol.Remove)
        bnPlayAudio.Image = ImageHelp.GetSymbolImage(Symbol.Play)
        bnUpAudio.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnDownAudio.Image = ImageHelp.GetSymbolImage(Symbol.Down)
        bnEditAudio.Image = ImageHelp.GetSymbolImage(Symbol.Repair)

        bnAddAttachment.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnRemoveAttachment.Image = ImageHelp.GetSymbolImage(Symbol.Remove)

        For Each bn In {bnAddAudio, bnRemoveAudio, bnPlayAudio, bnUpAudio,
                        bnDownAudio, bnEditAudio, bnAddAttachment, bnRemoveAttachment}

            bn.TextImageRelation = TextImageRelation.Overlay
            bn.ImageAlign = ContentAlignment.MiddleLeft
            Dim pad = bn.Padding
            pad.Left = Control.DefaultFont.Height \ 10
            pad.Right = pad.Left
            bn.Padding = pad
        Next

        Dim profileName = dgvAudio.AddTextBoxColumn()
        profileName.DataPropertyName = "Name"
        profileName.HeaderText = "Profile"
        profileName.ReadOnly = True

        Dim pathColumn = dgvAudio.AddTextBoxColumn()
        pathColumn.DataPropertyName = "DisplayName"
        pathColumn.HeaderText = "Track"
        pathColumn.ReadOnly = True

        If dgvAudio.RowCount > 0 Then dgvAudio.Rows(0).Selected = True

        UpdateControls()
        TipProvider.SetTip("Additional command line switches that may contain macros.", tpCommandLine)
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        If TypeOf Muxer Is MkvMuxer Then
            s.CmdlPresetsMKV = CmdlControl.Presets.ReplaceUnicode
        ElseIf TypeOf Muxer Is MP4Muxer Then
            s.CmdlPresetsMP4 = CmdlControl.Presets.ReplaceUnicode
        End If

        s.Storage.SetInt("last selected muxer tab", tcMain.SelectedIndex)
        SetValues()

        If DialogResult = DialogResult.OK Then
            p.AudioTracks = DirectCast(AudioBindingSource.DataSource, List(Of AudioProfile))
            Muxer.Attachments.Clear()
            Muxer.Attachments.AddRange(lbAttachments.Items.OfType(Of AttachmentContainer).Select(Function(val) val.Filepath))
        End If

        MyBase.OnFormClosed(e)
    End Sub

    Private Sub SetValues()
        SubtitleControl.SetValues(Muxer)
        SimpleUI.Save()
        Muxer.AdditionalSwitches = CmdlControl.tb.Text.ReplaceUnicode
    End Sub

    Private Sub bnCmdlPreview_Click() Handles bnCommandLinePreview.Click
        SetValues()
        g.ShowCommandLinePreview("Command Line", Muxer.GetCommandLine)
    End Sub

    Private Sub bnAddAudio_Click(sender As Object, e As EventArgs) Handles bnAddAudio.Click
        Using d As New OpenFileDialog
            d.SetFilter(FileTypes.Audio.Union(FileTypes.VideoAudio))
            d.Multiselect = True
            d.SetInitDir(p.TempDir)

            If d.ShowDialog = DialogResult.OK Then
                Dim sb As New SelectionBox(Of AudioProfile)
                sb.Title = "Audio Profile"
                sb.Text = "Please select a audio profile."

                For Each audioProfile In s.AudioProfiles
                    sb.AddItem(audioProfile)
                Next

                If sb.Show = DialogResult.OK Then
                    For Each path In d.FileNames
                        Dim ap = ObjectHelp.GetCopy(sb.SelectedValue)
                        ap.File = path

                        If Not p.Script.GetFilter("Source").Script.Contains("DirectShowSource") Then
                            ap.Delay = g.ExtractDelay(ap.File)
                        End If

                        If FileTypes.VideoAudio.Contains(ap.File.Ext) Then
                            ap.Streams = MediaInfo.GetAudioStreams(ap.File)
                        End If

                        ap.SetStreamOrLanguage()

                        If Not ap.Stream Is Nothing Then
                            Dim sb2 As New SelectionBox(Of AudioStream)
                            sb2.Title = "Stream Selection"
                            sb2.Text = "Please select a audio stream."

                            For Each i2 In ap.Streams
                                sb2.AddItem(i2)
                            Next

                            If sb2.Show = DialogResult.Cancel Then Return
                            ap.Stream = sb2.SelectedValue
                        End If

                        g.MainForm.UpdateSizeOrBitrate()
                        AudioBindingSource.Add(ap)
                        AudioBindingSource.Position = AudioBindingSource.Count - 1
                        UpdateControls()
                    Next
                End If
            End If
        End Using
    End Sub

    Private Sub bnRemoveAudio_Click(sender As Object, e As EventArgs) Handles bnRemoveAudio.Click
        dgvAudio.RemoveSelection
        UpdateControls()
    End Sub

    Private Sub bnUpAudio_Click(sender As Object, e As EventArgs) Handles bnUpAudio.Click
        dgvAudio.MoveSelectionUp
        UpdateControls()
    End Sub

    Private Sub bnDownAudio_Click(sender As Object, e As EventArgs) Handles bnDownAudio.Click
        dgvAudio.MoveSelectionDown
        UpdateControls()
    End Sub

    Private Sub bnPlayAudio_Click(sender As Object, e As EventArgs) Handles bnPlayAudio.Click
        g.Play(DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile).File)
    End Sub

    Private Sub bnEditAudio_Click(sender As Object, e As EventArgs) Handles bnEditAudio.Click
        Dim ap = DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile)
        ap.EditProject()
        g.MainForm.UpdateAudioMenu()
        g.MainForm.UpdateSizeOrBitrate()
        AudioBindingSource.ResetBindings(False)
    End Sub

    Private Sub BnAddAttachment_Click(sender As Object, e As EventArgs) Handles bnAddAttachment.Click
        Using d As New OpenFileDialog
            d.SetFilter({"ttf", "txt", "jpg", "png", "otf", "jpeg", "xml", "nfo"})
            d.Multiselect = True
            d.SetInitDir(p.TempDir)

            If d.ShowDialog = DialogResult.OK Then
                Dim items2 = lbAttachments.Items.OfType(Of AttachmentContainer).Select(Function(val) val.Filepath).ToList
                items2.AddRange(d.FileNames)
                items2.Sort()
                lbAttachments.Items.Clear()
                lbAttachments.Items.AddRange(items2.Select(Function(val) New AttachmentContainer With {.Filepath = val}).ToArray)
                UpdateControls()
            End If
        End Using
    End Sub

    Sub UpdateControls()
        bnRemoveAudio.Enabled = dgvAudio.SelectedRows.Count > 0
        bnPlayAudio.Enabled = dgvAudio.SelectedRows.Count > 0
        bnEditAudio.Enabled = dgvAudio.SelectedRows.Count > 0
        bnUpAudio.Enabled = dgvAudio.CanMoveUp
        bnDownAudio.Enabled = dgvAudio.CanMoveDown

        lbAttachments.UpdateControls()
    End Sub

    Private Sub dgvAudio_MouseUp(sender As Object, e As MouseEventArgs) Handles dgvAudio.MouseUp
        UpdateControls()
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        Dim lastAction As Action

        Dim UI = SimpleUI
        UI.Store = Muxer
        UI.BackColor = Color.Transparent

        Dim page = UI.CreateFlowPage("main page")
        page.SuspendLayout()

        Dim tb = UI.AddTextButton()
        tb.Text = "Cover"
        tb.Expandet = True
        tb.Property = NameOf(Muxer.CoverFile)
        tb.BrowseFile("jpg, png, bmp|*.jpg;*.png;*.bmp")

        Dim mb = UI.AddTextMenu()
        mb.Text = "Chapters"
        mb.Expandet = True
        mb.Help = "Chapter file to be muxed."
        mb.Property = NameOf(Muxer.ChapterFile)
        mb.AddMenu("Browse File...", Function() g.BrowseFile("txt, xml|*.txt;*.xml"))
        mb.AddMenu("Edit with chapterEditor...", Sub() g.ShellExecute(Package.chapterEditor.Path, Muxer.ChapterFile.Escape))

        If TypeOf Muxer Is MkvMuxer Then
            CmdlControl.Presets = s.CmdlPresetsMKV

            mb = UI.AddTextMenu()
            mb.Text = "Tags"
            mb.Expandet = True
            mb.Help = "Tag file to be muxed."
            mb.Property = NameOf(Muxer.TagFile)
            mb.AddMenu("Browse File...", Function() g.BrowseFile("xml|*.xml"))
            mb.AddMenu("Edit File...", Sub() g.ShellExecute(g.GetAppPathForExtension("xml", "txt"), Muxer.TagFile.Escape))

            tb = UI.AddTextButton()
            tb.Text = "Timestamps"
            tb.Help = "txt or mkv file"
            tb.Expandet = True
            tb.Property = NameOf(Muxer.TimestampsFile)
            tb.BrowseFile("txt, mkv|*.txt;*.mkv")

            tb = UI.AddTextButton()
            tb.Text = "Title"
            tb.Expandet = True
            tb.Property = NameOf(MkvMuxer.Title)
            tb.MacroDialog()

            Dim t = UI.AddText()
            t.Text = "Video Track Name"
            t.Help = "Optional name of the video stream that may contain macros."
            t.Expandet = True
            t.Property = NameOf(Muxer.VideoTrackName)

            Dim tm = UI.AddTextMenu()
            tm.Text = "Display Aspect Ratio"
            tm.Help = "Display Aspect Ratio to be applied by mkvmerge. By default and best practice the aspect ratio should be signalled to the encoder and not to the muxer, use this setting at your own risk."
            tm.Property = NameOf(MkvMuxer.DAR)
            tm.AddMenu(s.DarMenu)

            Dim ml = UI.AddMenu(Of Language)()
            ml.Text = "Video Track Language"
            ml.Help = "Optional language of the video stream."
            ml.Property = NameOf(MkvMuxer.VideoTrackLanguage)

            lastAction = Sub()
                             For Each i In Language.Languages
                                 If i.IsCommon Then
                                     ml.Button.Add(i.ToString + " (" + i.TwoLetterCode + ", " + i.ThreeLetterCode + ")", i)
                                 Else
                                     ml.Button.Add("More | " + i.ToString.Substring(0, 1).ToUpper + " | " + i.ToString + " (" + i.TwoLetterCode + ", " + i.ThreeLetterCode + ")", i)
                                 End If

                                 Application.DoEvents()
                             Next
                         End Sub

        ElseIf TypeOf Muxer Is MP4Muxer Then
            tpAttachments.Enabled = False

            CmdlControl.Presets = s.CmdlPresetsMP4

            Dim t = UI.AddText()
            t.Text = "Video Track Name"
            t.Help = "Optional name of the video stream that may contain macros."
            t.Expandet = True
            t.Property = NameOf(Muxer.VideoTrackName)

            Dim tm = UI.AddTextMenu()
            tm.Text = "Pixel Aspect Ratio:"
            tm.Help = "Display Aspect Ratio to be applied by MP4Box. By default and best practice the aspect ratio should be signalled to the encoder and not to the muxer, use this setting at your own risk."
            tm.Property = NameOf(MP4Muxer.PAR)
            tm.AddMenu(s.ParMenu)
        End If

        page.ResumeLayout()
        lastAction?.Invoke

        MyBase.OnShown(e)
    End Sub

    Private Sub MuxerForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        Dim form As New HelpForm()
        form.Doc.WriteStart(Text)
        form.Doc.WriteP(Strings.Muxer)
        form.Doc.WriteTips(TipProvider.GetTips, SimpleUI.ActivePage.TipProvider.GetTips)
        form.Doc.WriteTable("Macros", Macro.GetTips())
        form.Show()
    End Sub

    Public Class AttachmentContainer
        Property Filepath As String

        Public Overrides Function ToString() As String
            If Filepath.Contains("_attachment_") Then
                Return Filepath.Right("_attachment_")
            End If

            Return Path.GetFileName(Filepath)
        End Function
    End Class
End Class
