
Namespace UI
    Public Class InputBoxForm
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
        Friend WithEvents tlpMain As TableLayoutPanel
        Friend WithEvents tlpTextBox As TableLayoutPanel
        Public WithEvents bnOK As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.laPrompt = New System.Windows.Forms.Label()
            Me.tbInput = New System.Windows.Forms.TextBox()
            Me.bnCancel = New System.Windows.Forms.Button()
            Me.bnOK = New System.Windows.Forms.Button()
            Me.cb = New System.Windows.Forms.CheckBox()
            Me.flp = New System.Windows.Forms.FlowLayoutPanel()
            Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
            Me.tlpTextBox = New System.Windows.Forms.TableLayoutPanel()
            Me.flp.SuspendLayout()
            Me.tlpMain.SuspendLayout()
            Me.tlpTextBox.SuspendLayout()
            Me.SuspendLayout()
            '
            'laPrompt
            '
            Me.laPrompt.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.laPrompt.AutoSize = True
            Me.laPrompt.Location = New System.Drawing.Point(12, 12)
            Me.laPrompt.Margin = New System.Windows.Forms.Padding(12, 12, 12, 20)
            Me.laPrompt.Name = "laPrompt"
            Me.laPrompt.Size = New System.Drawing.Size(880, 48)
            Me.laPrompt.TabIndex = 0
            Me.laPrompt.Text = "Please enter your name."
            Me.laPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'tbInput
            '
            Me.tbInput.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tbInput.Location = New System.Drawing.Point(0, 0)
            Me.tbInput.Margin = New System.Windows.Forms.Padding(0)
            Me.tbInput.Name = "tbInput"
            Me.tbInput.Size = New System.Drawing.Size(880, 55)
            Me.tbInput.TabIndex = 1
            '
            'bnCancel
            '
            Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.bnCancel.Location = New System.Drawing.Point(495, 0)
            Me.bnCancel.Margin = New System.Windows.Forms.Padding(0)
            Me.bnCancel.Name = "bnCancel"
            Me.bnCancel.Size = New System.Drawing.Size(250, 70)
            Me.bnCancel.TabIndex = 2
            Me.bnCancel.Text = "Cancel"
            '
            'bnOK
            '
            Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.bnOK.Enabled = False
            Me.bnOK.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.bnOK.Location = New System.Drawing.Point(233, 0)
            Me.bnOK.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
            Me.bnOK.Name = "bnOK"
            Me.bnOK.Size = New System.Drawing.Size(250, 70)
            Me.bnOK.TabIndex = 3
            Me.bnOK.Text = "OK"
            '
            'cb
            '
            Me.cb.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.cb.AutoSize = True
            Me.cb.Location = New System.Drawing.Point(0, 9)
            Me.cb.Margin = New System.Windows.Forms.Padding(0)
            Me.cb.Name = "cb"
            Me.cb.Size = New System.Drawing.Size(221, 52)
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
            Me.flp.Location = New System.Drawing.Point(147, 147)
            Me.flp.Margin = New System.Windows.Forms.Padding(12)
            Me.flp.Name = "flp"
            Me.flp.Size = New System.Drawing.Size(745, 70)
            Me.flp.TabIndex = 5
            Me.flp.WrapContents = False
            '
            'tlpMain
            '
            Me.tlpMain.AutoSize = True
            Me.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.tlpMain.ColumnCount = 1
            Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.tlpMain.Controls.Add(Me.tlpTextBox, 0, 1)
            Me.tlpMain.Controls.Add(Me.laPrompt, 0, 0)
            Me.tlpMain.Controls.Add(Me.flp, 0, 2)
            Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tlpMain.Location = New System.Drawing.Point(0, 0)
            Me.tlpMain.Name = "tlpMain"
            Me.tlpMain.RowCount = 3
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.tlpMain.Size = New System.Drawing.Size(904, 225)
            Me.tlpMain.TabIndex = 6
            '
            'tlpTextBox
            '
            Me.tlpTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tlpTextBox.AutoSize = True
            Me.tlpTextBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.tlpTextBox.ColumnCount = 1
            Me.tlpTextBox.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.tlpTextBox.Controls.Add(Me.tbInput, 0, 0)
            Me.tlpTextBox.Location = New System.Drawing.Point(12, 80)
            Me.tlpTextBox.Margin = New System.Windows.Forms.Padding(12, 0, 12, 0)
            Me.tlpTextBox.Name = "tlpTextBox"
            Me.tlpTextBox.RowCount = 1
            Me.tlpTextBox.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.tlpTextBox.Size = New System.Drawing.Size(880, 55)
            Me.tlpTextBox.TabIndex = 7
            '
            'InputBoxForm
            '
            Me.AcceptButton = Me.bnOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
            Me.AutoSize = True
            Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.CancelButton = Me.bnCancel
            Me.ClientSize = New System.Drawing.Size(904, 225)
            Me.Controls.Add(Me.tlpMain)
            Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.KeyPreview = True
            Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "InputBoxForm"
            Me.ShowInTaskbar = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.flp.ResumeLayout(False)
            Me.flp.PerformLayout()
            Me.tlpMain.ResumeLayout(False)
            Me.tlpMain.PerformLayout()
            Me.tlpTextBox.ResumeLayout(False)
            Me.tlpTextBox.PerformLayout()
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

        Private Sub tbInput_TextChanged(sender As Object, e As EventArgs) Handles tbInput.TextChanged

        End Sub
    End Class
End Namespace