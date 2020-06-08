
Imports System.Drawing.Drawing2D
Imports Microsoft.Win32

Public Class ToolStripRendererEx
    Inherits ToolStripSystemRenderer

    Shared RenderMode As ToolStripRenderModeEx

    Shared Property ColorChecked As Color
    Shared Property ColorBorder As Color
    Shared Property ColorTop As Color
    Shared Property ColorBottom As Color
    Shared Property ColorBackground As Color

    Shared Property ColorToolStrip1 As Color
    Shared Property ColorToolStrip2 As Color
    Shared Property ColorToolStrip3 As Color
    Shared Property ColorToolStrip4 As Color

    Private TextOffset As Integer

    Sub New(mode As ToolStripRenderModeEx)
        RenderMode = mode
        InitColors(mode)
    End Sub

    Shared Function IsAutoRenderMode() As Boolean
        Return _
            RenderMode = ToolStripRenderModeEx.SystemAuto OrElse
            RenderMode = ToolStripRenderModeEx.Win7Auto OrElse
            RenderMode = ToolStripRenderModeEx.Win10Auto
    End Function

    Shared Sub InitColors(renderMode As ToolStripRenderModeEx)
        If ToolStripRendererEx.IsAutoRenderMode Then
            Dim argb = CInt(Registry.GetValue("HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorizationColor", 0))

            If argb = 0 Then
                argb = Color.LightBlue.ToArgb
            End If

            InitColors(Color.FromArgb(argb))
        Else
            ColorChecked = Color.FromArgb(&HFF91C9F7)
            ColorBorder = Color.FromArgb(&HFF83ABDC)
            ColorTop = Color.FromArgb(&HFFE7F0FB)
            ColorBottom = Color.FromArgb(&HFF91C9F7)
            ColorBackground = SystemColors.Control

            ColorToolStrip1 = Color.FromArgb(&HFFFDFEFF)
            ColorToolStrip2 = Color.FromArgb(&HFFF0F0F0)
            ColorToolStrip3 = Color.FromArgb(&HFFDCE6F4)
            ColorToolStrip4 = Color.FromArgb(&HFFDDE9F7)
        End If
    End Sub

    Shared Sub InitColors(c As Color)
        ColorBorder = HSLColor.Convert(c).ToColorSetLuminosity(100)
        ColorChecked = HSLColor.Convert(c).ToColorSetLuminosity(180)
        ColorBottom = HSLColor.Convert(c).ToColorSetLuminosity(200)
        ColorBackground = HSLColor.Convert(c).ToColorSetLuminosity(230)
        ColorTop = HSLColor.Convert(c).ToColorSetLuminosity(240)

        ColorToolStrip1 = ControlPaint.LightLight(ControlPaint.LightLight(ControlPaint.Light(ColorBorder, 1)))
        ColorToolStrip2 = ControlPaint.LightLight(ControlPaint.LightLight(ControlPaint.Light(ColorBorder, 0.7)))
        ColorToolStrip3 = ControlPaint.LightLight(ControlPaint.LightLight(ControlPaint.Light(ColorBorder, 0.1)))
        ColorToolStrip4 = ControlPaint.LightLight(ControlPaint.LightLight(ControlPaint.Light(ColorBorder, 0.4)))
    End Sub

    Protected Overrides Sub OnRenderToolStripBorder(e As ToolStripRenderEventArgs)
        ControlPaint.DrawBorder(e.Graphics, e.AffectedBounds, Color.FromArgb(160, 175, 195), ButtonBorderStyle.Solid)
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
        End If

        MyBase.OnRenderItemText(e)
    End Sub

    Protected Overrides Sub OnRenderToolStripBackground(e As ToolStripRenderEventArgs)
        If Not TypeOf e.ToolStrip Is ToolStripDropDownMenu AndAlso
            Not e.ToolStrip.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow Then

            Dim rect As New Rectangle(-1, -1, e.AffectedBounds.Width, e.AffectedBounds.Height)

            If IsFlat() Then
                Using b As New SolidBrush(ColorToolStrip2)
                    e.Graphics.FillRectangle(b, rect)
                End Using
            Else
                Dim cb As New ColorBlend()
                cb.Colors = {ColorToolStrip1, ColorToolStrip2, ColorToolStrip3, ColorToolStrip4}
                cb.Positions = {0.0F, 0.5F, 0.5F, 1.0F}

                Using brush As New LinearGradientBrush(rect, ColorToolStrip1, ColorToolStrip4, 90)
                    brush.InterpolationColors = cb
                    e.Graphics.FillRectangle(brush, rect)
                End Using
            End If
        End If
    End Sub

    Protected Overrides Sub OnRenderMenuItemBackground(e As ToolStripItemRenderEventArgs)
        e.Item.ForeColor = Color.Black

        Dim rect = New Rectangle(Point.Empty, e.Item.Size)
        Dim gx = e.Graphics

        If Not TypeOf e.Item.Owner Is MenuStrip Then
            gx.Clear(ColorBackground)
        End If

        If e.Item.Selected AndAlso e.Item.Enabled Then
            If TypeOf e.Item.Owner Is MenuStrip Then
                DrawButton(e)
            Else
                Dim rect2 = New Rectangle(rect.X + 2, rect.Y, rect.Width - 3, rect.Height)

                If IsFlat() Then
                    Using brush As New SolidBrush(ColorBottom)
                        gx.FillRectangle(brush, rect2)
                    End Using
                Else
                    rect2 = New Rectangle(rect2.X, rect2.Y, rect2.Width - 1, rect2.Height - 1)

                    gx.SmoothingMode = SmoothingMode.AntiAlias

                    Using path = CreateRoundRectangle(rect2, 3)
                        Using brush As New LinearGradientBrush(
                            rect2,
                            ControlPaint.LightLight(ControlPaint.LightLight(ColorTop)),
                            ControlPaint.LightLight(ControlPaint.LightLight(ColorBottom)),
                            90.0F)

                            gx.FillPath(brush, path)
                        End Using

                        Using pen As New Pen(ColorBorder)
                            gx.DrawPath(pen, path)
                        End Using
                    End Using

                    rect2.Inflate(-1, -1)

                    Using path = CreateRoundRectangle(rect2, 3)
                        Using brush As New LinearGradientBrush(rect2, ColorTop, ColorBottom, 90.0F)
                            gx.FillPath(brush, path)
                        End Using
                    End Using
                End If
            End If
        End If
    End Sub

    Sub DrawButton(e As ToolStripItemRenderEventArgs)
        Dim gx = e.Graphics
        Dim rect = New Rectangle(Point.Empty, e.Item.Size)

        If IsFlat() Then
            Dim button = TryCast(e.Item, ToolStripButton)

            If Not button Is Nothing AndAlso button.Checked Then
                Using brush As New SolidBrush(ColorChecked)
                    gx.FillRectangle(brush, rect)
                End Using
            Else
                Using brush As New SolidBrush(ColorChecked)
                    gx.FillRectangle(brush, rect)
                End Using
            End If
        Else
            rect = New Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1)

            gx.SmoothingMode = SmoothingMode.AntiAlias

            Dim c1 = HSLColor.Convert(ColorToolStrip1).ToColorAddLuminosity(15)
            Dim c2 = HSLColor.Convert(ColorToolStrip2).ToColorAddLuminosity(15)
            Dim c3 = HSLColor.Convert(ColorToolStrip3).ToColorAddLuminosity(15)
            Dim c4 = HSLColor.Convert(ColorToolStrip4).ToColorAddLuminosity(15)

            Dim cb As New ColorBlend()

            cb.Colors = {c1, c2, c3, c4}
            cb.Positions = {0.0F, 0.5F, 0.5F, 1.0F}

            Using path = CreateRoundRectangle(rect, 3)
                Using brush As New LinearGradientBrush(rect, c1, c4, 90)
                    brush.InterpolationColors = cb
                    gx.FillPath(brush, path)
                End Using

                Using pen As New Pen(ColorBorder)
                    gx.DrawPath(pen, path)
                End Using
            End Using

            rect.Inflate(-1, -1)

            c1 = HSLColor.Convert(ColorToolStrip1).ToColorAddLuminosity(5)
            c2 = HSLColor.Convert(ColorToolStrip2).ToColorAddLuminosity(5)
            c3 = HSLColor.Convert(ColorToolStrip3).ToColorAddLuminosity(-10)
            c4 = HSLColor.Convert(ColorToolStrip4).ToColorAddLuminosity(-10)

            cb.Colors = {c1, c2, c3, c4}
            cb.Positions = {0.0F, 0.5F, 0.5F, 1.0F}

            Using brush As New LinearGradientBrush(rect, c1, c4, 90)
                brush.InterpolationColors = cb

                Using path = CreateRoundRectangle(rect, 3)
                    gx.FillPath(brush, path)
                End Using
            End Using
        End If
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

    Protected Overloads Overrides Sub OnRenderArrow(e As ToolStripArrowRenderEventArgs)
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality

        If e.Direction = ArrowDirection.Down Then
            Dim h = CInt(e.Item.Font.Height * 0.3)
            Dim w = h * 2
            Dim cs = e.Item.Bounds

            Dim x1 = If(e.Item.Text = "", CInt(cs.Width / 2 - w / 2), cs.Width - w - CInt(w * 0.7))
            Dim y1 = CInt(cs.Height / 2 - h / 2)

            Dim x2 = CInt(x1 + w / 2)
            Dim y2 = y1 + h

            Dim x3 = x1 + w
            Dim y3 = y1

            Using brush = New SolidBrush(e.Item.ForeColor)
                Using pen = New Pen(brush, e.Item.Font.Height / 20.0F)
                    e.Graphics.DrawLine(pen, x1, y1, x2, y2)
                    e.Graphics.DrawLine(pen, x2, y2, x3, y3)
                End Using
            End Using
        Else
            Dim x1 = e.Item.Width - e.Item.Height * 0.6F
            Dim y1 = e.Item.Height * 0.25F

            Dim x2 = x1 + e.Item.Height * 0.25F
            Dim y2 = e.Item.Height / 2.0F

            Dim x3 = x1
            Dim y3 = e.Item.Height * 0.75F

            Using brush = New SolidBrush(e.Item.ForeColor)
                Using pen = New Pen(brush, e.Item.Font.Height / 20.0F)
                    e.Graphics.DrawLine(pen, x1, y1, x2, y2)
                    e.Graphics.DrawLine(pen, x2, y2, x3, y3)
                End Using
            End Using
        End If
    End Sub

    Protected Overrides Sub OnRenderItemCheck(e As ToolStripItemImageRenderEventArgs)
        Dim x = CInt(e.ImageRectangle.Height * 0.2)
        e.Graphics.DrawImage(e.Image, New Point(x, x))
    End Sub

    Protected Overloads Overrides Sub OnRenderSeparator(e As ToolStripSeparatorRenderEventArgs)
        If e.Item.IsOnDropDown Then
            e.Graphics.Clear(ColorBackground)
            Dim right = e.Item.Width - CInt(TextOffset / 5)
            Dim top = e.Item.Height \ 2
            top -= 1
            Dim bounds = e.Item.Bounds

            Using pen As New Pen(Color.Gray)
                e.Graphics.DrawLine(pen, New Point(TextOffset, top), New Point(right, top))
            End Using
        ElseIf e.Vertical Then
            Dim bounds = e.Item.Bounds

            Using pen As New Pen(SystemColors.ControlDarkDark)
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

    Shared Function IsFlat() As Boolean
        If RenderMode = ToolStripRenderModeEx.Win10Default Then Return True
        If RenderMode = ToolStripRenderModeEx.Win10Auto Then Return True

        If (RenderMode = ToolStripRenderModeEx.SystemDefault OrElse
            RenderMode = ToolStripRenderModeEx.SystemAuto) AndAlso
            OSVersion.Current >= OSVersion.Windows8 Then Return True
    End Function
End Class
