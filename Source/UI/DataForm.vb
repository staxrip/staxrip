
Imports StaxRip.UI

Public Class DataForm
    Property HelpAction As Action

    Sub New()
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

    Sub DataForm_HelpRequested(sender As Object, args As HelpEventArgs) Handles Me.HelpRequested
        HelpAction?.Invoke
    End Sub
End Class
