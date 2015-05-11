Public Class VideoDrawer
    Property CropLeft As Integer
    Property CropTop As Integer
    Property CropRight As Integer
    Property CropBottom As Integer
    Property ShowInfos As Boolean

    Private Control As Control
    Private AVI As AVIFile

    Sub New(targetControl As Control, avs As AVIFile)
        Control = targetControl
        AVI = avs
    End Sub

    Sub Draw(Optional g As Graphics = Nothing)
        If g Is Nothing Then g = Control.CreateGraphics()

        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

        Using g
            If Control.Visible Then
                Dim rectDest As RectangleF
                Dim img As Image

                Try
                    img = AVI.GetBitmap

                    If img Is Nothing Then
                        MsgError("AviSynth Error", "AviSynth returned empty image data.")
                        Exit Sub
                    End If
                Catch ex As OutOfMemoryException
                    Static wasShown As Boolean

                    If Not wasShown Then
                        MsgError("Memory Allocation Failure", "Please restart StaxRip in order to free memory.")
                        wasShown = True
                    End If

                    Exit Sub
                End Try

                If CropLeft = 0 AndAlso CropTop = 0 AndAlso CropRight = 0 AndAlso CropBottom = 0 Then
                    rectDest = Control.ClientRectangle
                    g.DrawImage(img, rectDest)
                Else
                    Dim factorX = CSng(Control.Width / img.Width)
                    Dim factorY = CSng(Control.Height / img.Height)

                    Dim left = CropLeft * factorX
                    Dim right = CropRight * factorX
                    Dim top = CropTop * factorY
                    Dim bottom = CropBottom * factorY

                    rectDest.X = left
                    rectDest.Y = top
                    rectDest.Width = Control.Width - left - right
                    rectDest.Height = Control.Height - top - bottom

                    Dim rectSrc As Rectangle

                    rectSrc.X = CropLeft
                    rectSrc.Y = CropTop
                    rectSrc.Width = img.Width - CropLeft - CropRight
                    rectSrc.Height = img.Height - CropTop - CropBottom

                    g.DrawImage(img, rectDest, rectSrc, GraphicsUnit.Pixel)

                    'left magnifier

                    rectSrc.X = CropLeft
                    rectSrc.Y = img.Height \ 2 - 8
                    rectSrc.Width = 8
                    rectSrc.Height = 16

                    rectDest.X = left
                    rectDest.Y = Control.Height \ 2 - 32
                    rectDest.Width = 32
                    rectDest.Height = 64

                    g.DrawImage(img, rectDest, rectSrc, GraphicsUnit.Pixel)

                    'top magnifier

                    rectSrc.X = img.Width \ 2 - 8
                    rectSrc.Y = CropTop
                    rectSrc.Width = 16
                    rectSrc.Height = 8

                    rectDest.X = Control.Width \ 2 - 32
                    rectDest.Y = top
                    rectDest.Width = 64
                    rectDest.Height = 32

                    g.DrawImage(img, rectDest, rectSrc, GraphicsUnit.Pixel)

                    'right magnifier

                    rectSrc.X = img.Width - CropRight - 8
                    rectSrc.Y = img.Height \ 2 - 8
                    rectSrc.Width = 8
                    rectSrc.Height = 16

                    rectDest.X = Control.Width - right - 32
                    rectDest.Y = Control.Height \ 2 - 32
                    rectDest.Width = 32
                    rectDest.Height = 64

                    g.DrawImage(img, rectDest, rectSrc, GraphicsUnit.Pixel)

                    'bottom magnifier

                    rectSrc.X = img.Width \ 2 - 8
                    rectSrc.Y = img.Height - CropBottom - 8
                    rectSrc.Width = 16
                    rectSrc.Height = 8

                    rectDest.X = Control.Width \ 2 - 32
                    rectDest.Y = Control.Height - bottom - 32
                    rectDest.Width = 64
                    rectDest.Height = 32

                    g.DrawImage(img, rectDest, rectSrc, GraphicsUnit.Pixel)

                    Using sb As New SolidBrush(ControlPaint.LightLight(
                                               ControlPaint.LightLight(ToolStripRendererEx.ColorBorder)))

                        g.FillRectangle(sb, 0, 0, left, Control.Height)
                        g.FillRectangle(sb, 0, 0, Control.Width, top)
                        g.FillRectangle(sb, Control.Width - right, 0, right, Control.Height)
                        g.FillRectangle(sb, 0, Control.Height - bottom, Control.Width, bottom)
                    End Using
                End If

                If ShowInfos Then
                    rectDest.Inflate(-10, -10)

                    Dim currentDate As Date
                    currentDate = currentDate.AddSeconds(AVI.Position / AVI.FrameRate)

                    Dim lengthtDate As Date
                    lengthtDate = lengthtDate.AddSeconds(AVI.FrameCount / AVI.FrameRate)
                    Dim frameSize = AVI.FrameSize

                    Dim text = "Frame: " & AVI.Position & " of " & AVI.FrameCount & CrLf &
                               "Time: " & currentDate.ToString("HH:mm:ss.fff") + " of " + lengthtDate.ToString("HH:mm:ss.fff") + CrLf +
                               "Width x Height: " & frameSize.Width & " x " & frameSize.Height & CrLf +
                               "Framerate: " & AVI.FrameRate.ToString("f6")

                    g.DrawString(text, New Font("Segoe UI", 12, FontStyle.Bold), Brushes.White, rectDest)
                End If
            End If
        End Using
    End Sub
End Class