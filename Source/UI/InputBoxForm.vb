
Namespace UI
    Public Class InputBoxForm
        Inherits FormBase

#Region " Designer "

        Friend WithEvents bnCancel As ButtonEx
        Public WithEvents tbInput As TextBoxEx
        Public WithEvents laPrompt As LabelEx
        Friend WithEvents cb As CheckBoxEx
        Friend WithEvents flp As FlowLayoutPanel
        Friend WithEvents tlpMain As TableLayoutPanel
        Friend WithEvents tlpTextBox As TableLayoutPanel
        Public WithEvents bnOK As ButtonEx
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.laPrompt = New LabelEx()
            Me.tbInput = New TextBoxEx()
            Me.bnCancel = New ButtonEx()
            Me.bnOK = New ButtonEx()
            Me.cb = New CheckBoxEx()
            Me.flp = New FlowLayoutPanel()
            Me.tlpMain = New TableLayoutPanel()
            Me.tlpTextBox = New TableLayoutPanel()
            Me.flp.SuspendLayout()
            Me.tlpMain.SuspendLayout()
            Me.tlpTextBox.SuspendLayout()
            Me.SuspendLayout()
            '
            'laPrompt
            '
            Me.laPrompt.Anchor = CType((AnchorStyles.Left Or AnchorStyles.Right), AnchorStyles)
            Me.laPrompt.AutoSize = True
            Me.laPrompt.Location = New System.Drawing.Point(12, 12)
            Me.laPrompt.Margin = New Padding(12, 12, 12, 20)
            Me.laPrompt.Name = "laPrompt"
            Me.laPrompt.Size = New System.Drawing.Size(880, 48)
            Me.laPrompt.TabIndex = 0
            Me.laPrompt.Text = "Please enter your name."
            Me.laPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'tbInput
            '
            Me.tbInput.Anchor = CType((AnchorStyles.Left Or AnchorStyles.Right), AnchorStyles)
            Me.tbInput.Location = New System.Drawing.Point(0, 0)
            Me.tbInput.Margin = New Padding(0)
            Me.tbInput.Name = "tbInput"
            Me.tbInput.Size = New System.Drawing.Size(880, 55)
            Me.tbInput.TabIndex = 1
            '
            'bnCancel
            '
            Me.bnCancel.Anchor = AnchorStyles.None
            Me.bnCancel.DialogResult = DialogResult.Cancel
            Me.bnCancel.Location = New System.Drawing.Point(495, 0)
            Me.bnCancel.Margin = New Padding(0)
            Me.bnCancel.Name = "bnCancel"
            Me.bnCancel.Size = New System.Drawing.Size(250, 70)
            Me.bnCancel.TabIndex = 2
            Me.bnCancel.Text = "Cancel"
            '
            'bnOK
            '
            Me.bnOK.Anchor = AnchorStyles.None
            Me.bnOK.DialogResult = DialogResult.OK
            Me.bnOK.Enabled = False
            Me.bnOK.Location = New System.Drawing.Point(233, 0)
            Me.bnOK.Margin = New Padding(12, 0, 12, 0)
            Me.bnOK.Name = "bnOK"
            Me.bnOK.Size = New System.Drawing.Size(250, 70)
            Me.bnOK.TabIndex = 3
            Me.bnOK.Text = "OK"
            '
            'cb
            '
            Me.cb.Anchor = AnchorStyles.None
            Me.cb.AutoSize = True
            Me.cb.Location = New System.Drawing.Point(0, 9)
            Me.cb.Margin = New Padding(0)
            Me.cb.Name = "cb"
            Me.cb.Size = New System.Drawing.Size(221, 52)
            Me.cb.TabIndex = 4
            Me.cb.Text = "CheckBox"
            'Me.cb.UseVisualStyleBackColor = True
            '
            'flp
            '
            Me.flp.Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles)
            Me.flp.AutoSize = True
            Me.flp.AutoSizeMode = AutoSizeMode.GrowAndShrink
            Me.flp.Controls.Add(Me.cb)
            Me.flp.Controls.Add(Me.bnOK)
            Me.flp.Controls.Add(Me.bnCancel)
            Me.flp.Location = New System.Drawing.Point(147, 147)
            Me.flp.Margin = New Padding(12)
            Me.flp.Name = "flp"
            Me.flp.Size = New System.Drawing.Size(745, 70)
            Me.flp.TabIndex = 5
            Me.flp.WrapContents = False
            '
            'tlpMain
            '
            Me.tlpMain.AutoSize = True
            Me.tlpMain.AutoSizeMode = AutoSizeMode.GrowAndShrink
            Me.tlpMain.ColumnCount = 1
            Me.tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))
            Me.tlpMain.Controls.Add(Me.tlpTextBox, 0, 1)
            Me.tlpMain.Controls.Add(Me.laPrompt, 0, 0)
            Me.tlpMain.Controls.Add(Me.flp, 0, 2)
            Me.tlpMain.Dock = DockStyle.Fill
            Me.tlpMain.Location = New System.Drawing.Point(0, 0)
            Me.tlpMain.Name = "tlpMain"
            Me.tlpMain.RowCount = 3
            Me.tlpMain.RowStyles.Add(New RowStyle())
            Me.tlpMain.RowStyles.Add(New RowStyle())
            Me.tlpMain.RowStyles.Add(New RowStyle())
            Me.tlpMain.Size = New System.Drawing.Size(904, 225)
            Me.tlpMain.TabIndex = 6
            '
            'tlpTextBox
            '
            Me.tlpTextBox.Anchor = CType((AnchorStyles.Left Or AnchorStyles.Right), AnchorStyles)
            Me.tlpTextBox.AutoSize = True
            Me.tlpTextBox.AutoSizeMode = AutoSizeMode.GrowAndShrink
            Me.tlpTextBox.ColumnCount = 1
            Me.tlpTextBox.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0!))
            Me.tlpTextBox.Controls.Add(Me.tbInput, 0, 0)
            Me.tlpTextBox.Location = New System.Drawing.Point(12, 80)
            Me.tlpTextBox.Margin = New Padding(12, 0, 12, 0)
            Me.tlpTextBox.Name = "tlpTextBox"
            Me.tlpTextBox.RowCount = 1
            Me.tlpTextBox.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0!))
            Me.tlpTextBox.Size = New System.Drawing.Size(880, 55)
            Me.tlpTextBox.TabIndex = 7
            '
            'InputBoxForm
            '
            Me.AcceptButton = Me.bnOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
            Me.AutoScaleMode = AutoScaleMode.Dpi
            Me.AutoSize = True
            Me.AutoSizeMode = AutoSizeMode.GrowAndShrink
            Me.CancelButton = Me.bnCancel
            Me.ClientSize = New System.Drawing.Size(904, 225)
            Me.Controls.Add(Me.tlpMain)
            Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.KeyPreview = True
            Me.Margin = New Padding(6, 6, 6, 6)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "InputBoxForm"
            Me.ShowInTaskbar = False
            Me.StartPosition = FormStartPosition.CenterParent
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

        Sub New()
            MyBase.New()
            InitializeComponent()
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

        Sub tbInput_TextChanged() Handles tbInput.TextChanged
            bnOK.Enabled = tbInput.Text <> ""
        End Sub

        Sub bnCancel_Click() Handles bnCancel.Click
            tbInput.Text = ""
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Sub InputBoxForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            If flp.Left < laPrompt.Left Then
                Width += laPrompt.Left - flp.Left
            End If
        End Sub

        Sub InputBoxForm_Shown() Handles Me.Shown
            Native.SetForegroundWindow(Handle)
        End Sub
    End Class
End Namespace
