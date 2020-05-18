Namespace UI
    Public Class MenuTemplateForm
        Inherits DialogBase

#Region " Designer "
        Friend WithEvents tv As System.Windows.Forms.TreeView
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.tv = New System.Windows.Forms.TreeView()
            Me.bnCancel = New StaxRip.UI.ButtonEx()
            Me.bnOK = New StaxRip.UI.ButtonEx()
            Me.SuspendLayout()
            '
            'tv
            '
            Me.tv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tv.Location = New System.Drawing.Point(12, 13)
            Me.tv.Margin = New System.Windows.Forms.Padding(3, 3, 3, 8)
            Me.tv.Name = "tv"
            Me.tv.Size = New System.Drawing.Size(417, 437)
            Me.tv.TabIndex = 0
            '
            'bnCancel
            '
            Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bnCancel.Location = New System.Drawing.Point(329, 461)
            Me.bnCancel.Size = New System.Drawing.Size(100, 34)
            Me.bnCancel.Text = "Cancel"
            '
            'bnOK
            '
            Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.bnOK.Location = New System.Drawing.Point(223, 461)
            Me.bnOK.Size = New System.Drawing.Size(100, 34)
            Me.bnOK.Text = "OK"
            '
            'MenuTemplateForm
            '
            Me.AcceptButton = Me.bnOK
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
            Me.CancelButton = Me.bnCancel
            Me.ClientSize = New System.Drawing.Size(441, 507)
            Me.Controls.Add(Me.bnCancel)
            Me.Controls.Add(Me.bnOK)
            Me.Controls.Add(Me.tv)
            Me.KeyPreview = True
            Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
            Me.Name = "MenuTemplateForm"
            Me.Text = "Default Menu"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
        Friend WithEvents bnOK As StaxRip.UI.ButtonEx

#End Region

        Public TreeNode As TreeNode

        Sub New(item As CustomMenuItem)
            MyBase.New()
            InitializeComponent()
            PopulateTreeView(item, Nothing)
            tv.ExpandAll()
        End Sub

        Private Sub PopulateTreeView(item As CustomMenuItem, node As TreeNode)
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

        Private Sub tv_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tv.AfterSelect
            If Not tv.SelectedNode Is Nothing Then
                TreeNode = tv.SelectedNode
                bnOK.Enabled = Not TreeNode.Parent Is Nothing
            End If
        End Sub

        Private Sub MenuTemplateForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
            Dim f As New HelpForm()
            f.Doc.WriteStart(Text)
            f.Doc.WriteParagraph("The new item will be a clone of the selected item.")
            f.Show()
        End Sub
    End Class
End Namespace