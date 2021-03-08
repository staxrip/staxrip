Imports System.Drawing.Drawing2D
Imports System.Windows.Forms.VisualStyles
Imports StaxRip.UI

Public Class CheckBoxRendererEx

    Public Shared Sub DrawCheckBox(g As Graphics, glyphLocation As Point, state As CheckBoxState)
        DrawCheckBox(g, New Rectangle(glyphLocation, GetGlyphSize(g, state)), state)
    End Sub

    Public Shared Sub DrawCheckBox(g As Graphics, rect As Rectangle, state As CheckBoxState)
        g.SmoothingMode = SmoothingMode.AntiAlias

        If ThemeManager.CurrentTheme.Name = ThemeManager.DefaultThemeName OrElse DesignHelp.IsDesignMode Then
            CheckBoxRenderer.DrawCheckBox(g, rect.Location, state)
            Return
        End If

        Dim checked = state = CheckBoxState.CheckedDisabled OrElse state = CheckBoxState.CheckedHot OrElse state = CheckBoxState.CheckedNormal OrElse state = CheckBoxState.CheckedPressed
        Dim theme = ThemeManager.CurrentTheme.General.Controls.CheckBox
        Dim backColor = If(checked, theme.BoxCheckedColor, theme.BoxColor)
        Dim penColor = theme.CheckmarkColor

        Select Case state
            Case CheckBoxState.CheckedNormal
                Using brush As New SolidBrush(backColor)
                    g.FillRectangle(brush, rect)
                End Using

                Dim penStrength = 4

                Dim startX1 = rect.Left + rect.Width / 4.5F
                Dim startY1 = rect.Top + rect.Height / 2.25F
                Dim endX1 = rect.Left + rect.Width / 3.0F
                Dim endY1 = rect.Top + rect.Height / 1.2F

                Dim startX2 = rect.Left + rect.Width / 3.1F
                Dim startY2 = rect.Top + rect.Height / 1.3F
                Dim endX2 = rect.Left + rect.Width / 1.2F
                Dim endY2 = rect.Top + rect.Height / 5.0F

                Using pen As New Pen(penColor, penStrength)
                    g.DrawLine(pen, startX1, startY1, endX1, endY1)
                    g.DrawLine(pen, startX2, startY2, endX2, endY2)
                End Using
            Case CheckBoxState.UncheckedNormal
                Using brush As New SolidBrush(backColor)
                    g.FillRectangle(brush, rect)
                End Using
        End Select
    End Sub

    Public Shared Function GetGlyphSize(g As Graphics, state As CheckBoxState) As Size
        Return CheckBoxRenderer.GetGlyphSize(g, state)
    End Function

End Class
