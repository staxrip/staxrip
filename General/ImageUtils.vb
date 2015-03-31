Imports System.Drawing.Drawing2D
Imports System.Globalization

Public Class Thumbnails
    Shared Sub SaveThumbnails(fp As String)
        If Not File.Exists(fp) Then Exit Sub
        If Not Packs.AviSynth.VerifyOK Then Exit Sub

        Log.WriteHeader("Saving Thumnails")
        Log.WriteLine(fp)
        Log.Save()

        Dim w = s.ThumbnailWidth
        Dim columns = s.ThumbnailColumns
        Dim rows = s.ThumbnailRows
        Dim dar = MediaInfo.GetVideo(fp, "DisplayAspectRatio")
        Dim h = CInt(w / Convert.ToSingle(dar, CultureInfo.InvariantCulture))
        Dim shadowDistance = 5
        Dim backgroundColor = Color.Gainsboro
        Dim gap = 5

        w = w - w Mod 4
        h = h - h Mod 4

        Dim d As New AviSynthDocument
        d.Path = Paths.SettingsDir + "Thumbnails.avs"
        d.Filters.Add(New AviSynthFilter("DirectShowSource(""" + fp + """, audio=false, convertfps=true).LanczosResize(" & w & "," & h & ")"))
        d.Filters.Add(New AviSynthFilter("ConvertToRGB()"))

        Dim errorMsg = ""

        Try
            d.Synchronize()
            errorMsg = p.SourceAviSynthDocument.GetErrorMessage
        Catch ex As Exception
            errorMsg = ex.Message
        End Try

        If OK(errorMsg) Then
            MsgError("Failed to open file, are you missing a codec?" + CrLf2 + fp, errorMsg)
            Exit Sub
        End If

        Dim frames = d.GetFrames
        Dim count = columns * rows

        Dim bitmaps As New List(Of Bitmap)

        Using avi As New AVIFile(d.Path)
            For x = 1 To count
                avi.Position = CInt((frames / count) * x) - CInt((frames / count) / 2)
                Dim b = New Bitmap(avi.GetBitmap())
                DropShadow(b, Color.Black, backgroundColor, shadowDistance)
                bitmaps.Add(b)
                ProcessForm.UpdateStatusThreadsafe("Extracting thumbnails: " & CInt(100 / count * x) & "%")
            Next

            w = w + shadowDistance + gap
            h = h + shadowDistance + gap
        End Using

        FileHelp.Delete(d.Path)

        Dim infoWidth = MediaInfo.GetVideo(fp, "Width")
        Dim infoHeight = MediaInfo.GetVideo(fp, "Height")
        Dim infoSize = CInt(New FileInfo(fp).Length / 1024 ^ 2).ToString
        Dim infoDuration = MediaInfo.GetInfo(fp, MediaInfoStreamKind.General, "Duration")

        Dim ts As New TimeSpan(CLng(infoDuration) * 10000L)
        
        Dim caption = Filepath.GetName(fp) + CrLf &
             infoSize & " MB, " & CInt(Math.Floor(ts.TotalMinutes)).ToString("D2") +
             ":" + ts.Seconds.ToString("D2") + " min, " & infoWidth & "x" & infoHeight

        Dim offsetCaption = 50

        Dim imgWidth = w * columns + shadowDistance + gap
        Dim imgHeight = h * rows + offsetCaption

        Using b As New Bitmap(imgWidth, imgHeight)
            Using g = Graphics.FromImage(b)
                g.Clear(backgroundColor)
                Dim r = New RectangleF(shadowDistance + gap, 0, imgWidth - (shadowDistance + gap) * 2, offsetCaption)
                Dim sf As New StringFormat
                sf.LineAlignment = StringAlignment.Center
                g.DrawString(caption, New Font("Tahoma", 12), Brushes.Black, r, sf)
                sf.Alignment = StringAlignment.Far
                g.DrawString("StaxRip", New Font("Tahoma", 30), Brushes.White, r, sf)

                For x = 0 To bitmaps.Count - 1
                    Dim rowPos = x \ columns
                    Dim columnPos = x Mod columns
                    g.DrawImage(bitmaps(x), columnPos * w + shadowDistance + gap, rowPos * h + offsetCaption)
                Next
            End Using

            b.Save(Filepath.GetChangeExt(fp, "jpg"), Imaging.ImageFormat.Jpeg)
        End Using
    End Sub

    Private Shared Sub DropShadow(ByRef sourceImage As Bitmap,
                          shadowColor As Color,
                          backgroundColor As Color,
                          Optional shadowDistance As Integer = 5,
                          Optional shadowOpacity As Integer = 150,
                          Optional shadowSoftness As Integer = 4,
                          Optional shadowRoundedEdges As Boolean = True)

        Dim imgTarget As Bitmap = Nothing
        Dim imgShadow As Bitmap = Nothing
        Dim g As Graphics = Nothing

        Try
            If sourceImage IsNot Nothing Then
                shadowOpacity = shadowOpacity.EnsureRange(0, 255)
                shadowSoftness = shadowSoftness.EnsureRange(1, 30)
                shadowDistance = shadowDistance.EnsureRange(1, 50)

                If shadowColor = Color.Transparent Then
                    shadowColor = Color.Black
                End If

                If backgroundColor = Color.Transparent Then
                    backgroundColor = Color.White
                End If

                'get shadow
                Dim shWidth = CInt(sourceImage.Width / shadowSoftness)
                Dim shHeight = CInt(sourceImage.Height / shadowSoftness)
                imgShadow = New Bitmap(shWidth, shHeight)
                g = Graphics.FromImage(imgShadow)
                g.Clear(Color.Transparent)
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.SmoothingMode = SmoothingMode.AntiAlias
                Dim sre = 0

                If shadowRoundedEdges Then sre = 1

                Using b = New SolidBrush(Color.FromArgb(shadowOpacity, shadowColor))
                    g.FillRectangle(b, sre, sre, shWidth, shHeight)
                End Using

                g.Dispose()

                'draw shadow
                Dim d_shWidth = sourceImage.Width + shadowDistance
                Dim d_shHeight = sourceImage.Height + shadowDistance
                imgTarget = New Bitmap(d_shWidth, d_shHeight)
                g = Graphics.FromImage(imgTarget)
                g.Clear(backgroundColor)
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.DrawImage(imgShadow, New Rectangle(0, 0, d_shWidth, d_shHeight), 0, 0, imgShadow.Width, imgShadow.Height, GraphicsUnit.Pixel)
                g.DrawImage(sourceImage, New Rectangle(0, 0, sourceImage.Width, sourceImage.Height), 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel)
                g.Dispose()
                imgShadow.Dispose()
                sourceImage = New Bitmap(imgTarget)
                imgTarget.Dispose()
            End If
        Catch
            If g IsNot Nothing Then g.Dispose()
            If imgShadow IsNot Nothing Then imgShadow.Dispose()
            If imgTarget IsNot Nothing Then imgTarget.Dispose()
        End Try
    End Sub
End Class