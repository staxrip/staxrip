
Imports System.Drawing.Drawing2D

Imports StaxRip.UI

Public Class SearchTextBox
    Inherits UserControl

    Property Edit As TextEdit

    Private XButton As ClearButton

    Sub New()
        Edit = New TextEdit()
        XButton = New ClearButton()
        SuspendLayout()
        Edit.Dock = DockStyle.Fill
        Controls.Add(XButton)
        Controls.Add(Edit)
        ResumeLayout(False)
        XButton.Visible = False
        XButton.Text = ""
        Edit.TextBox.SendMessageCue("Search", False)
        ApplyTheme()
        AddHandler Edit.TextChanged, Sub(sender As Object, e As EventArgs) OnTextChanged(e)
        AddHandler XButton.Click, Sub()
                                      XButton.Visible = False
                                      Edit.Text = ""
                                  End Sub
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

        BackColor = theme.General.Controls.TextBox.BackColor
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        XButton.Visible = Edit.Text <> ""
    End Sub

    Protected Overrides Sub OnLayout(e As LayoutEventArgs)
        MyBase.OnLayout(e)

        XButton.Top = 2
        XButton.Height = Height - 4
        XButton.Width = XButton.Height
        XButton.Left = Width - XButton.Width - XButton.Top

        If Height <> Edit.Height Then
            Height = Edit.Height
        End If

        Edit.Width = Width
    End Sub

    Overrides Property Text As String
        Get
            Return Edit.Text
        End Get
        Set(value As String)
            Edit.Text = value
        End Set
    End Property

    Class ClearButton
        Inherits ButtonEx

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality

            Using pen = New Pen(ForeColor, FontHeight / 16.0F)
                Dim offset = CSng(Width / 3.3)
                e.Graphics.DrawLine(pen, offset, offset, Width - offset, Height - offset)
                e.Graphics.DrawLine(pen, Width - offset, offset, offset, Height - offset)
            End Using
        End Sub
    End Class
End Class