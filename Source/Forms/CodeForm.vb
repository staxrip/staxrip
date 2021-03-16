
Imports StaxRip.UI

Public Class CodeForm
    Property Find As String
    Property Content As String

    Sub New(text As String, find As String)
        InitializeComponent()

        RestoreClientSize(50, 35)

        rtb.ReadOnly = True
        rtb.WordWrap = False
        rtb.Font = g.GetCodeFont
        rtb.Text = text

        Me.Find = find
        Content = text

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
            FindByIndex(0)
        End If
    End Sub

    Sub FindByIndex(startIndex As Integer)
        If startIndex >= Content.Length Then
            Exit Sub
        End If

        rtb.Find(Find, startIndex, RichTextBoxFinds.None)
        rtb.ScrollToCaret()

        Dim start = rtb.SelectionStart

        If start > startIndex Then
            While True
                start -= 1

                If start = 0 Then
                    Exit While
                End If

                Dim ch = Content(start)

                If ch = BR(1) Then
                    Exit While
                End If
            End While

            If (rtb.SelectionStart - start) > 15 Then
                FindByIndex(rtb.SelectionStart + rtb.SelectionLength)
            End If
        End If
    End Sub

    Sub CodeForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyData = Keys.Escape Then
            Close()
        End If
    End Sub
End Class
