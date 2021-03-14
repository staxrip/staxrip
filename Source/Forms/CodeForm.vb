
Imports StaxRip.UI

Public Class CodeForm
    Property Find As String

    Sub New(text As String, find As String)
        InitializeComponent()

        RestoreClientSize(50, 35)

        rtb.Text = text
        rtb.ReadOnly = True

        Me.Find = find

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

        rtb.ForeColor = theme.General.Controls.RichTextBox.ForeColor
        rtb.BackColor = theme.General.Controls.RichTextBox.BackColor
    End Sub

    Sub TextHelpForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If Find <> "" Then
            rtb.Find(Find)
            rtb.ScrollToCaret()
        End If
    End Sub
End Class
