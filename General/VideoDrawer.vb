
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
        If g Is Nothing Then
            g = Control.CreateGraphics()
        End If

        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        Using g
            If Control.Visible Then
                Dim rectDest As Rectangle

                Try
                    Using bmp = AVI.GetBitmap
                        If bmp Is Nothing Then
                            MsgError("AviSynth Error", "AviSynth returned empty image data.")
                            Exit Sub
                        End If

                        If CropLeft = 0 AndAlso CropTop = 0 AndAlso CropRight = 0 AndAlso CropBottom = 0 Then
                            rectDest = Control.ClientRectangle
                            g.DrawImage(bmp, rectDest)
                        Else
                            Dim factorX = Control.Width / bmp.Width
                            Dim factorY = Control.Height / bmp.Height

                            Dim left = CInt(CropLeft * factorX)
                            Dim right = CInt(CropRight * factorX)
                            Dim top = CInt(CropTop * factorY)
                            Dim bottom = CInt(CropBottom * factorY)

                            rectDest.X = left
                            rectDest.Y = top
                            rectDest.Width = Control.Width - left - right
                            rectDest.Height = Control.Height - top - bottom

                            Dim rectSrc As Rectangle

                            rectSrc.X = CropLeft
                            rectSrc.Y = CropTop
                            rectSrc.Width = bmp.Width - CropLeft - CropRight
                            rectSrc.Height = bmp.Height - CropTop - CropBottom

                            g.DrawImage(bmp, rectDest, rectSrc, GraphicsUnit.Pixel)

                            'left magnifier

                            rectSrc.X = CropLeft
                            rectSrc.Y = bmp.Height \ 2 - 8
                            rectSrc.Width = 8
                            rectSrc.Height = 16

                            rectDest.X = left
                            rectDest.Y = Control.Height \ 2 - 32
                            rectDest.Width = 32
                            rectDest.Height = 64

                            g.DrawImage(bmp, rectDest, rectSrc, GraphicsUnit.Pixel)

                            'top magnifier

                            rectSrc.X = bmp.Width \ 2 - 8
                            rectSrc.Y = CropTop
                            rectSrc.Width = 16
                            rectSrc.Height = 8

                            rectDest.X = Control.Width \ 2 - 32
                            rectDest.Y = top
                            rectDest.Width = 64
                            rectDest.Height = 32

                            g.DrawImage(bmp, rectDest, rectSrc, GraphicsUnit.Pixel)

                            'right magnifier

                            rectSrc.X = bmp.Width - CropRight - 8 - 1
                            rectSrc.Y = bmp.Height \ 2 - 8
                            rectSrc.Width = 8
                            rectSrc.Height = 16

                            rectDest.X = Control.Width - right - 32
                            rectDest.Y = Control.Height \ 2 - 32
                            rectDest.Width = 32
                            rectDest.Height = 64

                            g.DrawImage(bmp, rectDest, rectSrc, GraphicsUnit.Pixel)

                            'bottom magnifier

                            rectSrc.X = bmp.Width \ 2 - 8
                            rectSrc.Y = bmp.Height - CropBottom - 8 - 1
                            rectSrc.Width = 16
                            rectSrc.Height = 8

                            rectDest.X = Control.Width \ 2 - 32
                            rectDest.Y = Control.Height - bottom - 32
                            rectDest.Width = 64
                            rectDest.Height = 32

                            g.DrawImage(bmp, rectDest, rectSrc, GraphicsUnit.Pixel)

                            Using sb As New SolidBrush(ControlPaint.LightLight(
                                                       ControlPaint.LightLight(ToolStripRendererEx.ColorBorder)))

                                g.FillRectangle(sb, 0, 0, left, Control.Height)
                                g.FillRectangle(sb, 0, 0, Control.Width, top)
                                g.FillRectangle(sb, Control.Width - right, 0, right, Control.Height)
                                g.FillRectangle(sb, 0, Control.Height - bottom, Control.Width, bottom)
                            End Using
                        End If
                    End Using

                    If ShowInfos Then
                        rectDest.Inflate(-10, -10)

                        Dim currentDate As Date
                        currentDate = currentDate.AddSeconds(AVI.Position / AVI.FrameRate)

                        Dim lengthtDate As Date
                        lengthtDate = lengthtDate.AddSeconds(AVI.FrameCount / AVI.FrameRate)
                        Dim frameSize = AVI.FrameSize

                        Dim format = If(lengthtDate.Hour = 0, "mm:ss.fff", "HH:mm:ss.fff")

                        Dim text = "Frame: " & AVI.Position & " (" & AVI.FrameCount & ")" + BR &
                                   "Time: " & currentDate.ToString(format) + " (" + lengthtDate.ToString(format) + ")" + BR +
                                   "Size: " & frameSize.Width & " x " & frameSize.Height & BR +
                                   "Rate: " & AVI.FrameRate.ToString.Shorten(9)

                        Dim font = New Font("Segoe UI", 10, FontStyle.Bold)
                        Dim textSize = TextRenderer.MeasureText(text, font)

                        Using brush As New SolidBrush(Color.FromArgb(100, 0, 0, 0))
                            g.FillRectangle(brush, New Rectangle(10, 10, textSize.Width, textSize.Height))
                        End Using

                        g.DrawString(text, font, Brushes.White, rectDest)
                    End If
                Catch ex As OutOfMemoryException
                    Static wasShown As Boolean

                    If Not wasShown Then
                        MsgError("Memory Allocation Failure", "Please restart StaxRip in order to free memory.")
                        wasShown = True
                    End If

                    Exit Sub
                End Try
            End If
        End Using
    End Sub
End Class