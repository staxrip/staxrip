Namespace UI
    Class InputBoxForm
        Inherits FormBase

#Region " Designer "

        Sub New()
            MyBase.New()
            InitializeComponent()
        End Sub

        Friend WithEvents bnCancel As System.Windows.Forms.Button
        Public WithEvents tbInput As System.Windows.Forms.TextBox
        Public WithEvents laPrompt As System.Windows.Forms.Label
        Friend WithEvents cb As System.Windows.Forms.CheckBox
        Friend WithEvents flp As System.Windows.Forms.FlowLayoutPanel
        Public WithEvents bnOK As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.laPrompt = New System.Windows.Forms.Label()
            Me.tbInput = New System.Windows.Forms.TextBox()
            Me.bnCancel = New System.Windows.Forms.Button()
            Me.bnOK = New System.Windows.Forms.Button()
            Me.cb = New System.Windows.Forms.CheckBox()
            Me.flp = New System.Windows.Forms.FlowLayoutPanel()
            Me.flp.SuspendLayout()
            Me.SuspendLayout()
            '
            'laPrompt
            '
            Me.laPrompt.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.laPrompt.Location = New System.Drawing.Point(12, 3)
            Me.laPrompt.Name = "laPrompt"
            Me.laPrompt.Size = New System.Drawing.Size(415, 51)
            Me.laPrompt.TabIndex = 0
            Me.laPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'tbInput
            '
            Me.tbInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tbInput.Location = New System.Drawing.Point(12, 57)
            Me.tbInput.Name = "tbInput"
            Me.tbInput.Size = New System.Drawing.Size(415, 31)
            Me.tbInput.TabIndex = 1
            '
            'bnCancel
            '
            Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.bnCancel.Location = New System.Drawing.Point(229, 3)
            Me.bnCancel.Name = "bnCancel"
            Me.bnCancel.Size = New System.Drawing.Size(100, 34)
            Me.bnCancel.TabIndex = 2
            Me.bnCancel.Text = "Cancel"
            '
            'bnOK
            '
            Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.bnOK.Enabled = False
            Me.bnOK.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.bnOK.Location = New System.Drawing.Point(123, 3)
            Me.bnOK.Name = "bnOK"
            Me.bnOK.Size = New System.Drawing.Size(100, 34)
            Me.bnOK.TabIndex = 3
            Me.bnOK.Text = "OK"
            '
            'cb
            '
            Me.cb.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.cb.AutoSize = True
            Me.cb.Location = New System.Drawing.Point(3, 5)
            Me.cb.Name = "cb"
            Me.cb.Size = New System.Drawing.Size(114, 29)
            Me.cb.TabIndex = 4
            Me.cb.Text = "CheckBox"
            Me.cb.UseVisualStyleBackColor = True
            '
            'flp
            '
            Me.flp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.flp.AutoSize = True
            Me.flp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.flp.Controls.Add(Me.cb)
            Me.flp.Controls.Add(Me.bnOK)
            Me.flp.Controls.Add(Me.bnCancel)
            Me.flp.Location = New System.Drawing.Point(98, 96)
            Me.flp.Name = "flp"
            Me.flp.Size = New System.Drawing.Size(332, 40)
            Me.flp.TabIndex = 5
            '
            'InputBoxForm
            '
            Me.AcceptButton = Me.bnOK
            Me.CancelButton = Me.bnCancel
            Me.ClientSize = New System.Drawing.Size(439, 145)
            Me.Controls.Add(Me.flp)
            Me.Controls.Add(Me.tbInput)
            Me.Controls.Add(Me.laPrompt)
            Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "InputBoxForm"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.flp.ResumeLayout(False)
            Me.flp.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private Sub tbInput_TextChanged() Handles tbInput.TextChanged
            bnOK.Enabled = tbInput.Text <> ""
        End Sub

        Private Sub bCancel_Click() Handles bnCancel.Click
            tbInput.Text = ""
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Private Sub InputBoxForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            If flp.Left < laPrompt.Left Then
                Width += laPrompt.Left - flp.Left
            End If
        End Sub

        Private Sub InputBoxForm_Shown() Handles Me.Shown
            Native.SetForegroundWindow(Handle)
        End Sub
    End Class
End Namespace