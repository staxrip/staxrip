
Imports System.Drawing.Drawing2D
Imports Microsoft.Win32
Imports StaxRip.UI

Public Class ToolStripRendererEx
    Inherits ToolStripSystemRenderer

    Shared Property BackgroundColor As Color
    Shared Property BorderOuterColor As ColorHSL
    Shared Property BorderInnerColor As ColorHSL
    Shared Property BottomColor As Color
    Shared Property BoxColor As ColorHSL
    Shared Property BoxSelectedColor As ColorHSL
    Shared Property CheckmarkColor As ColorHSL
    Shared Property CheckmarkSelectedColor As ColorHSL
    Shared Property CheckedColor As Color
    Shared Property DropdownBackgroundDefaultColor As ColorHSL
    Shared Property DropdownBackgroundSelectedColor As ColorHSL
    Shared Property DropdownTextDefaultColor As ColorHSL
    Shared Property DropdownTextSelectedColor As ColorHSL
    Shared Property MenuStripBackgroundDefaultColor As ColorHSL
    Shared Property MenuStripBackgroundSelectedColor As ColorHSL
    Shared Property MenuStripTextDefaultColor As ColorHSL
    Shared Property MenuStripTextSelectedColor As ColorHSL
    Shared Property SymbolImageColor As ColorHSL
    Shared Property ToolStrip1Color As Color
    Shared Property ToolStrip2Color As Color
    Shared Property ToolStrip3Color As Color
    Shared Property ToolStrip4Color As Color
    Shared Property TopColor As Color

    Private TextOffset As Integer

    Sub New()
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

        BorderInnerColor = theme.General.Controls.ToolStrip.BorderInnerColor
        BorderOuterColor = theme.General.Controls.ToolStrip.BorderOuterColor
        BoxColor = theme.General.Controls.ToolStrip.BoxColor
        BoxSelectedColor = theme.General.Controls.ToolStrip.BoxSelectedColor
        CheckmarkColor = theme.General.Controls.ToolStrip.CheckmarkColor
        CheckmarkSelectedColor = theme.General.Controls.ToolStrip.CheckmarkSelectedColor
        DropdownBackgroundDefaultColor = theme.General.Controls.ToolStrip.DropdownBackgroundDefaultColor
        DropdownBackgroundSelectedColor = theme.General.Controls.ToolStrip.DropdownBackgroundSelectedColor
        DropdownTextDefaultColor = theme.General.Controls.ToolStrip.DropdownTextDefaultColor
        DropdownTextSelectedColor = theme.General.Controls.ToolStrip.DropdownTextSelectedColor
        MenuStripBackgroundDefaultColor = theme.General.Controls.ToolStrip.MenuStripBackgroundDefaultColor
        MenuStripBackgroundSelectedColor = theme.General.Controls.ToolStrip.MenuStripBackgroundSelectedColor
        MenuStripTextDefaultColor = theme.General.Controls.ToolStrip.MenuStripTextDefaultColor
        MenuStripTextSelectedColor = theme.General.Controls.ToolStrip.MenuStripTextSelectedColor
        SymbolImageColor = theme.General.Controls.ToolStrip.SymbolImageColor

        ToolStrip1Color = theme.General.Controls.ToolStrip.MenuStripBackgroundDefaultColor
        ToolStrip2Color = theme.General.Controls.ToolStrip.MenuStripBackgroundDefaultColor
        ToolStrip3Color = theme.General.Controls.ToolStrip.MenuStripBackgroundDefaultColor
        ToolStrip4Color = theme.General.Controls.ToolStrip.MenuStripBackgroundDefaultColor
    End Sub

    Protected Overrides Sub OnRenderToolStripBorder(e As ToolStripRenderEventArgs)
        If TypeOf e.ToolStrip IsNot MenuStrip Then
            Dim r = e.AffectedBounds
            r.Inflate(-1, -1)
            ControlPaint.DrawBorder(e.Graphics, r, BorderInnerColor, ButtonBorderStyle.Solid)
            ControlPaint.DrawBorder(e.Graphics, e.AffectedBounds, BorderOuterColor, ButtonBorderStyle.Solid)
        End If
    End Sub

    Protected Overloads Overrides Sub OnRenderItemText(e As ToolStripItemTextRenderEventArgs)
        If TypeOf e.Item Is ToolStripMenuItem AndAlso Not TypeOf e.Item.Owner Is MenuStrip Then
            Dim rect = e.TextRectangle
            Dim dropDown = TryCast(e.ToolStrip, ToolStripDropDownMenu)

            If dropDown Is Nothing OrElse dropDown.ShowImageMargin OrElse dropDown.ShowCheckMargin Then
                TextOffset = CInt(e.Item.Height * 1.1)
            Else
                TextOffset = CInt(e.Item.Height * 0.2)
            End If

            e.TextRectangle = New Rectangle(TextOffset, CInt((e.Item.Height - rect.Height) / 2), rect.Width, rect.Height)

            If e.Item.Selected AndAlso e.Item.Enabled Then
                e.TextColor = DropdownTextSelectedColor
            Else
                e.TextColor = DropdownTextDefaultColor
            End If
        ElseIf TypeOf e.Item.Owner Is MenuStrip Then
            If e.Item.Selected AndAlso e.Item.Enabled Then
                e.TextColor = MenuStripTextSelectedColor
            Else
                e.TextColor = MenuStripTextDefaultColor
            End If
        Else
            e.TextColor = MenuStripTextDefaultColor
        End If

        MyBase.OnRenderItemText(e)
    End Sub

    Protected Overrides Sub OnRenderToolStripBackground(e As ToolStripRenderEventArgs)
        If TypeOf e.ToolStrip IsNot ToolStripDropDownMenu AndAlso
            Not e.ToolStrip.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow Then

            Dim rect As New Rectangle(0, 0, e.AffectedBounds.Width, e.AffectedBounds.Height)

            Using b As New SolidBrush(MenuStripBackgroundDefaultColor)
                e.Graphics.FillRectangle(b, rect)
            End Using
        End If
    End Sub

    Protected Overrides Sub OnRenderMenuItemBackground(e As ToolStripItemRenderEventArgs)
        e.Item.ForeColor = DropdownTextDefaultColor

        Dim rect = New Rectangle(Point.Empty, e.Item.Size)
        Dim gx = e.Graphics

        If Not TypeOf e.Item.Owner Is MenuStrip Then
            gx.Clear(DropdownBackgroundDefaultColor)
        End If

        If e.Item.Selected AndAlso e.Item.Enabled Then
            If TypeOf e.Item.Owner Is MenuStrip Then
                DrawButton(e)
            Else
                Dim rect2 = New Rectangle(rect.X + 2, rect.Y, rect.Width - 3, rect.Height)

                Using brush As New SolidBrush(DropdownBackgroundSelectedColor)
                    gx.FillRectangle(brush, rect2)
                End Using
            End If
        End If
    End Sub

    Sub DrawButton(e As ToolStripItemRenderEventArgs)
        Dim gx = e.Graphics
        Dim rect = New Rectangle(Point.Empty, e.Item.Size)

        Using brush As New SolidBrush(MenuStripBackgroundSelectedColor)
            gx.FillRectangle(brush, rect)
        End Using
    End Sub

    Protected Overrides Sub OnRenderDropDownButtonBackground(e As ToolStripItemRenderEventArgs)
        If e.Item.Selected Then
            DrawButton(e)
        End If
    End Sub

    Protected Overrides Sub OnRenderButtonBackground(e As ToolStripItemRenderEventArgs)
        Dim button = DirectCast(e.Item, ToolStripButton)

        If e.Item.Selected OrElse button.Checked Then
            DrawButton(e)
        End If
    End Sub

    Protected Overrides Sub OnRenderItemCheck(e As ToolStripItemImageRenderEventArgs)
        Dim item = TryCast(e.Item, ToolStripMenuItem)

        If item Is Nothing OrElse Not item.Checked Then
            Exit Sub
        End If

        Dim gx = e.Graphics
        gx.SmoothingMode = SmoothingMode.AntiAlias
        Dim h = item.Height

        Dim rect = New Rectangle(2, 0, h, h)
        Dim col = If(item.Selected, BoxSelectedColor, BoxColor)

        Using brush As New SolidBrush(col)
            gx.FillRectangle(brush, rect)
        End Using

        Dim x1 = CInt(2 + h * 0.4)
        Dim y1 = CInt(h * 0.7)

        Dim x2 = CInt(x1 - h * 0.2)
        Dim y2 = CInt(y1 - h * 0.2)

        Dim x3 = CInt(x1 + h * 0.37)
        Dim y3 = CInt(y1 - h * 0.37)

        Dim penColor = If(item.Selected, CheckmarkSelectedColor, CheckmarkColor)
        Using pen = New Pen(penColor, e.Item.Font.Height / 16.0F)
            gx.DrawLine(pen, x1, y1, x2, y2)
            gx.DrawLine(pen, x1, y1, x3, y3)
        End Using
    End Sub

    Protected Overloads Overrides Sub OnRenderArrow(e As ToolStripArrowRenderEventArgs)
        Dim gx = e.Graphics
        gx.SmoothingMode = SmoothingMode.HighQuality
        Dim foreColor As Color

        If e.Item.Selected AndAlso e.Item.Enabled Then
            foreColor = DropdownTextSelectedColor
        Else
            foreColor = DropdownTextDefaultColor
        End If

        If e.Direction = ArrowDirection.Down Then
            Dim h = CInt(e.Item.Font.Height * 0.25)
            Dim w = h * 2
            Dim cs = e.Item.Bounds

            Dim x1 = If(e.Item.Text = "", CInt(cs.Width / 2 - w / 2), cs.Width - w - CInt(w * 0.7))
            Dim y1 = CInt(cs.Height / 2 - h / 2)

            Dim x2 = CInt(x1 + w / 2)
            Dim y2 = y1 + h

            Dim x3 = x1 + w
            Dim y3 = y1

            Using pen = New Pen(foreColor, e.Item.Font.Height / 16.0F)
                gx.DrawLine(pen, x1, y1, x2, y2)
                gx.DrawLine(pen, x2, y2, x3, y3)
            End Using
        Else
            Dim x1 = e.Item.Width - e.Item.Height * 0.6F
            Dim y1 = (e.Item.Height * 0.3F) - 1

            Dim x2 = x1 + e.Item.Height * 0.2F
            Dim y2 = (e.Item.Height / 2.0F) - 1

            Dim x3 = x1
            Dim y3 = (e.Item.Height * 0.7F) - 1

            Using pen = New Pen(foreColor, e.Item.Font.Height / 16.0F)
                gx.DrawLine(pen, x1, y1, x2, y2)
                gx.DrawLine(pen, x2, y2, x3, y3)
            End Using
        End If
    End Sub

    Protected Overloads Overrides Sub OnRenderSeparator(e As ToolStripSeparatorRenderEventArgs)
        If e.Item.IsOnDropDown Then
            e.Graphics.Clear(DropdownBackgroundDefaultColor)
            Dim right = e.Item.Width - CInt(TextOffset / 5)
            Dim top = e.Item.Height \ 2
            top -= 1
            Dim bounds = e.Item.Bounds

            Using pen As New Pen(MenuStripTextDefaultColor)
                e.Graphics.DrawLine(pen, New Point(TextOffset, top), New Point(right, top))
            End Using
        ElseIf e.Vertical Then
            Dim bounds = e.Item.Bounds

            Using pen As New Pen(MenuStripTextDefaultColor)
                e.Graphics.DrawLine(pen, CInt(bounds.Width / 2), CInt(bounds.Height * 0.15),
                                         CInt(bounds.Width / 2), CInt(bounds.Height * 0.85))
            End Using
        End If
    End Sub

    Shared Function CreateRoundRectangle(r As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()

        Dim l = r.Left
        Dim t = r.Top
        Dim w = r.Width
        Dim h = r.Height
        Dim d = radius << 1

        path.AddArc(l, t, d, d, 180, 90)
        path.AddLine(l + radius, t, l + w - radius, t)
        path.AddArc(l + w - d, t, d, d, 270, 90)
        path.AddLine(l + w, t + radius, l + w, t + h - radius)
        path.AddArc(l + w - d, t + h - d, d, d, 0, 90)
        path.AddLine(l + w - radius, t + h, l + radius, t + h)
        path.AddArc(l, t + h - d, d, d, 90, 90)
        path.AddLine(l, t + h - radius, l, t + radius)
        path.CloseFigure()

        Return path
    End Function
End Class
