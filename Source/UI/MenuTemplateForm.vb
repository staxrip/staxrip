
Namespace UI
    Public Class MenuTemplateForm
        Inherits DialogBase

#Region " Designer "
        Friend WithEvents tv As TreeViewEx
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.tv = New TreeViewEx()
            Me.bnCancel = New StaxRip.UI.ButtonEx()
            Me.bnOK = New StaxRip.UI.ButtonEx()
            Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
            Me.pnTreeView = New PanelEx()
            Me.tlpMain.SuspendLayout()
            Me.pnTreeView.SuspendLayout()
            Me.SuspendLayout()
            '
            'tv
            '
            Me.tv.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tv.Location = New System.Drawing.Point(0, 0)
            Me.tv.Margin = New System.Windows.Forms.Padding(3, 3, 3, 8)
            Me.tv.Name = "tv"
            Me.tv.Size = New System.Drawing.Size(1030, 1093)
            Me.tv.TabIndex = 0
            '
            'bnCancel
            '
            Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bnCancel.Location = New System.Drawing.Point(798, 1129)
            Me.bnCancel.Margin = New System.Windows.Forms.Padding(18, 0, 18, 18)
            Me.bnCancel.Size = New System.Drawing.Size(250, 70)
            Me.bnCancel.Text = "Cancel"
            '
            'bnOK
            '
            Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.bnOK.Location = New System.Drawing.Point(530, 1129)
            Me.bnOK.Margin = New System.Windows.Forms.Padding(0)
            Me.bnOK.Size = New System.Drawing.Size(250, 70)
            Me.bnOK.Text = "OK"
            '
            'tlpMain
            '
            Me.tlpMain.ColumnCount = 2
            Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
            Me.tlpMain.Controls.Add(Me.bnCancel, 1, 1)
            Me.tlpMain.Controls.Add(Me.bnOK, 0, 1)
            Me.tlpMain.Controls.Add(Me.pnTreeView, 0, 0)
            Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tlpMain.Location = New System.Drawing.Point(0, 0)
            Me.tlpMain.Name = "tlpMain"
            Me.tlpMain.RowCount = 2
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.Size = New System.Drawing.Size(1066, 1217)
            Me.tlpMain.TabIndex = 2
            '
            'pnTreeView
            '
            Me.pnTreeView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tlpMain.SetColumnSpan(Me.pnTreeView, 2)
            Me.pnTreeView.Controls.Add(Me.tv)
            Me.pnTreeView.Location = New System.Drawing.Point(18, 18)
            Me.pnTreeView.Margin = New System.Windows.Forms.Padding(18)
            Me.pnTreeView.Name = "pnTreeView"
            Me.pnTreeView.Size = New System.Drawing.Size(1030, 1093)
            Me.pnTreeView.TabIndex = 2
            '
            'MenuTemplateForm
            '
            Me.AcceptButton = Me.bnOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
            Me.CancelButton = Me.bnCancel
            Me.ClientSize = New System.Drawing.Size(1066, 1217)
            Me.Controls.Add(Me.tlpMain)
            Me.KeyPreview = True
            Me.Margin = New System.Windows.Forms.Padding(6)
            Me.Name = "MenuTemplateForm"
            Me.Text = "Default Menu"
            Me.tlpMain.ResumeLayout(False)
            Me.pnTreeView.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
        Friend WithEvents bnOK As StaxRip.UI.ButtonEx
        Friend WithEvents tlpMain As TableLayoutPanel
        Friend WithEvents pnTreeView As PanelEx

#End Region

        Public TreeNode As TreeNode

        Sub New(item As CustomMenuItem)
            InitializeComponent()
            PopulateTreeView(item, Nothing)
            tv.ExpandAll()

            ApplyTheme()
            AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
        End Sub

        Sub OnThemeChanged(theme As Theme)
            ApplyTheme(theme)
        End Sub

        Sub ApplyTheme()
            ApplyTheme(ThemeManager.CurrentTheme)
        End Sub

        Sub ApplyTheme(theme As Theme)
            If DesignHelp.IsDesignMode Then
                Exit Sub
            End If

            BackColor = theme.General.BackColor
        End Sub

        Sub PopulateTreeView(item As CustomMenuItem, node As TreeNode)
            Dim newNode As New TreeNode(item.Text)
            newNode.Tag = item

            If node Is Nothing Then
                tv.Nodes.Add(newNode)
            Else
                node.Nodes.Add(newNode)
            End If

            For Each i As CustomMenuItem In item.SubItems
                PopulateTreeView(i, newNode)
            Next
        End Sub

        Sub tv_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tv.AfterSelect
            If Not tv.SelectedNode Is Nothing Then
                TreeNode = tv.SelectedNode
                bnOK.Enabled = Not TreeNode.Parent Is Nothing
            End If
        End Sub

        Sub MenuTemplateForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
            Dim form As New HelpForm()
            form.Doc.WriteStart(Text)
            form.Doc.WriteParagraph("The new item will be a clone of the selected item.")
            form.Show()
        End Sub
    End Class
End Namespace
