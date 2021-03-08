
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

        Public WithEvents rtb As RichTextBoxEx
        Public WithEvents bnOK As ButtonEx
        Public WithEvents cbWrap As CheckBoxEx
        Friend WithEvents Panel1 As PanelEx
        Public WithEvents bnCancel As ButtonEx
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.bnOK = New ButtonEx()
            Me.bnCancel = New ButtonEx()
            Me.rtb = New StaxRip.UI.RichTextBoxEx()
            Me.cbWrap = New CheckBoxEx()
            Me.Panel1 = New PanelEx()
            Me.Panel1.SuspendLayout()
            Me.SuspendLayout()
            '
            'bnOK
            '
            Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.bnOK.Location = New System.Drawing.Point(341, 270)
            Me.bnOK.Name = "bnOK"
            Me.bnOK.Size = New System.Drawing.Size(250, 70)
            Me.bnOK.TabIndex = 0
            Me.bnOK.Text = "OK"
            '
            'bnCancel
            '
            Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.bnCancel.Location = New System.Drawing.Point(602, 270)
            Me.bnCancel.Name = "bnCancel"
            Me.bnCancel.Size = New System.Drawing.Size(250, 70)
            Me.bnCancel.TabIndex = 1
            Me.bnCancel.Text = "Cancel"
            '
            'rtb
            '
            Me.rtb.AcceptsTab = True
            Me.rtb.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.rtb.Dock = System.Windows.Forms.DockStyle.Fill
            Me.rtb.Location = New System.Drawing.Point(0, 0)
            Me.rtb.Margin = New System.Windows.Forms.Padding(5)
            Me.rtb.Name = "rtb"
            Me.rtb.Size = New System.Drawing.Size(840, 241)
            Me.rtb.TabIndex = 2
            Me.rtb.Text = ""
            Me.rtb.WordWrap = False
            '
            'cbWrap
            '
            Me.cbWrap.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.cbWrap.AutoSize = True
            Me.cbWrap.Location = New System.Drawing.Point(14, 278)
            Me.cbWrap.Margin = New System.Windows.Forms.Padding(5)
            Me.cbWrap.Name = "cbWrap"
            Me.cbWrap.Size = New System.Drawing.Size(152, 52)
            Me.cbWrap.TabIndex = 3
            Me.cbWrap.Text = "Wrap"
            '
            'Panel1
            '
            Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.Panel1.Controls.Add(Me.rtb)
            Me.Panel1.Location = New System.Drawing.Point(12, 12)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(840, 241)
            Me.Panel1.TabIndex = 4
            '
            'StringEditorForm
            '
            Me.AcceptButton = Me.bnOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
            Me.CancelButton = Me.bnCancel
            Me.ClientSize = New System.Drawing.Size(864, 352)
            Me.Controls.Add(Me.Panel1)
            Me.Controls.Add(Me.cbWrap)
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
            Me.Panel1.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Sub New()
            MyBase.New()
            InitializeComponent()
            cbWrap.Checked = True
            rtb.Font = New Font("Consolas", 10 * s.UIScaleFactor)
            ScaleClientSize(41, 24)
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

        Sub tb_KeyDown(sender As Object, e As KeyEventArgs) Handles rtb.KeyDown
            If e.KeyData = (Keys.Enter Or Keys.Control) Then
                e.Handled = True
                bnOK.PerformClick()
            End If
        End Sub

        Sub cbWrap_CheckedChanged() Handles cbWrap.CheckedChanged
            rtb.WordWrap = cbWrap.Checked
        End Sub
    End Class
End Namespace
