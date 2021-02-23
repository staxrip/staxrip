
Imports StaxRip.UI

Public Class SelectionBoxForm
    Inherits FormBase

    Private components As System.ComponentModel.IContainer

    Friend WithEvents bnCancel As ButtonEx
    Public WithEvents bnOK As ButtonEx
    Friend WithEvents mb As MenuButton
    Public WithEvents laText As LabelEx

#Region " Designer "
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.laText = New LabelEx()
        Me.bnCancel = New ButtonEx()
        Me.bnOK = New ButtonEx()
        Me.mb = New StaxRip.UI.MenuButton()
        Me.SuspendLayout()
        '
        'laText
        '
        Me.laText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.laText.Location = New System.Drawing.Point(12, 9)
        Me.laText.Name = "laText"
        Me.laText.Size = New System.Drawing.Size(612, 101)
        Me.laText.TabIndex = 0
        Me.laText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(374, 209)
        Me.bnCancel.Name = "bnCancel"
        Me.bnCancel.Size = New System.Drawing.Size(250, 70)
        Me.bnCancel.TabIndex = 3
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(118, 209)
        Me.bnOK.Name = "bnOK"
        Me.bnOK.Size = New System.Drawing.Size(250, 70)
        Me.bnOK.TabIndex = 2
        Me.bnOK.Text = "OK"
        '
        'mb
        '
        Me.mb.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mb.Location = New System.Drawing.Point(12, 121)
        Me.mb.ShowMenuSymbol = True
        Me.mb.Size = New System.Drawing.Size(612, 70)
        '
        'SelectionBoxForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(636, 291)
        Me.Controls.Add(Me.mb)
        Me.Controls.Add(Me.bnOK)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.laText)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectionBoxForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.ResumeLayout(False)

    End Sub

#End Region

    Property ReturnValue As Object

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
        BackColor = theme.General.BackColor
    End Sub

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Sub lText_TextChanged() Handles laText.TextChanged
        Using gx = laText.CreateGraphics
            Dim textSize = gx.MeasureString(laText.Text, laText.Font, laText.Width)

            If textSize.Height > laText.Height Then
                Height += CInt(textSize.Height - laText.Height)
            End If
        End Using
    End Sub
End Class
