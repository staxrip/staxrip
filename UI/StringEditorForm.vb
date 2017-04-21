Namespace UI
    Class StringEditorForm
        Inherits FormBase

#Region " Designer "

        Protected Overloads Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private components As System.ComponentModel.IContainer

        Public WithEvents tb As System.Windows.Forms.TextBox
        Public WithEvents bnOK As System.Windows.Forms.Button
        Public WithEvents cbWrap As System.Windows.Forms.CheckBox
        Public WithEvents bnCancel As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.bnOK = New System.Windows.Forms.Button()
            Me.bnCancel = New System.Windows.Forms.Button()
            Me.tb = New System.Windows.Forms.TextBox()
            Me.cbWrap = New System.Windows.Forms.CheckBox()
            Me.SuspendLayout()
            '
            'bnOK
            '
            Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.bnOK.Location = New System.Drawing.Point(1440, 813)
            Me.bnOK.Margin = New System.Windows.Forms.Padding(5)
            Me.bnOK.Name = "bnOK"
            Me.bnOK.Size = New System.Drawing.Size(200, 70)
            Me.bnOK.TabIndex = 0
            Me.bnOK.Text = "OK"
            '
            'bnCancel
            '
            Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bnCancel.Location = New System.Drawing.Point(1650, 813)
            Me.bnCancel.Margin = New System.Windows.Forms.Padding(5)
            Me.bnCancel.Name = "bnCancel"
            Me.bnCancel.Size = New System.Drawing.Size(200, 70)
            Me.bnCancel.TabIndex = 1
            Me.bnCancel.Text = "Cancel"
            '
            'tb
            '
            Me.tb.AcceptsReturn = True
            Me.tb.AcceptsTab = True
            Me.tb.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tb.Location = New System.Drawing.Point(14, 14)
            Me.tb.Margin = New System.Windows.Forms.Padding(5)
            Me.tb.Multiline = True
            Me.tb.Name = "tb"
            Me.tb.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.tb.Size = New System.Drawing.Size(1836, 784)
            Me.tb.TabIndex = 2
            Me.tb.WordWrap = False
            '
            'cbWrap
            '
            Me.cbWrap.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cbWrap.AutoSize = True
            Me.cbWrap.Location = New System.Drawing.Point(14, 823)
            Me.cbWrap.Margin = New System.Windows.Forms.Padding(5)
            Me.cbWrap.Name = "cbWrap"
            Me.cbWrap.Size = New System.Drawing.Size(152, 52)
            Me.cbWrap.TabIndex = 3
            Me.cbWrap.Text = "Wrap"
            '
            'StringEditorForm
            '
            Me.AcceptButton = Me.bnOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
            Me.CancelButton = Me.bnCancel
            Me.ClientSize = New System.Drawing.Size(1864, 897)
            Me.Controls.Add(Me.cbWrap)
            Me.Controls.Add(Me.tb)
            Me.Controls.Add(Me.bnCancel)
            Me.Controls.Add(Me.bnOK)
            Me.KeyPreview = True
            Me.Margin = New System.Windows.Forms.Padding(5)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(425, 269)
            Me.Name = "StringEditorForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "String Editor"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Sub New()
            MyBase.New()
            InitializeComponent()
            cbWrap.Checked = True
        End Sub

        Private Sub tb_KeyDown(sender As Object, e As KeyEventArgs) Handles tb.KeyDown
            If e.KeyData = (Keys.Enter Or Keys.Control) Then
                e.Handled = True
                bnOK.PerformClick()
            End If
        End Sub

        Private Sub cbWrap_CheckedChanged() Handles cbWrap.CheckedChanged
            tb.WordWrap = cbWrap.Checked
        End Sub
    End Class
End Namespace