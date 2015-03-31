Namespace UI
    Public Class StringEditorForm
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
        Public WithEvents bOK As System.Windows.Forms.Button
        Public WithEvents cbWrap As System.Windows.Forms.CheckBox
        Public WithEvents bCancel As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.bOK = New System.Windows.Forms.Button()
            Me.bCancel = New System.Windows.Forms.Button()
            Me.tb = New System.Windows.Forms.TextBox()
            Me.cbWrap = New System.Windows.Forms.CheckBox()
            Me.SuspendLayout()
            '
            'bOK
            '
            Me.bOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.bOK.Location = New System.Drawing.Point(382, 329)
            Me.bOK.Name = "bOK"
            Me.bOK.Size = New System.Drawing.Size(100, 34)
            Me.bOK.TabIndex = 0
            Me.bOK.Text = "OK"
            '
            'bCancel
            '
            Me.bCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bCancel.Location = New System.Drawing.Point(488, 329)
            Me.bCancel.Name = "bCancel"
            Me.bCancel.Size = New System.Drawing.Size(100, 34)
            Me.bCancel.TabIndex = 1
            Me.bCancel.Text = "Cancel"
            '
            'tb
            '
            Me.tb.AcceptsReturn = True
            Me.tb.AcceptsTab = True
            Me.tb.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tb.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tb.Location = New System.Drawing.Point(12, 12)
            Me.tb.Multiline = True
            Me.tb.Name = "tb"
            Me.tb.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.tb.Size = New System.Drawing.Size(576, 311)
            Me.tb.TabIndex = 2
            Me.tb.WordWrap = False
            '
            'cbWrap
            '
            Me.cbWrap.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cbWrap.AutoSize = True
            Me.cbWrap.Location = New System.Drawing.Point(13, 333)
            Me.cbWrap.Name = "cbWrap"
            Me.cbWrap.Size = New System.Drawing.Size(81, 29)
            Me.cbWrap.TabIndex = 3
            Me.cbWrap.Text = "Wrap"
            '
            'StringEditorForm
            '
            Me.AcceptButton = Me.bOK
            Me.CancelButton = Me.bCancel
            Me.ClientSize = New System.Drawing.Size(600, 375)
            Me.Controls.Add(Me.cbWrap)
            Me.Controls.Add(Me.tb)
            Me.Controls.Add(Me.bCancel)
            Me.Controls.Add(Me.bOK)
            Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(250, 200)
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
                bOK.PerformClick()
            End If
        End Sub

        Private Sub cbWrap_CheckedChanged() Handles cbWrap.CheckedChanged
            tb.WordWrap = cbWrap.Checked
        End Sub
    End Class
End Namespace