Imports System.Drawing.Drawing2D
Imports System.Globalization

Class Thumbnails
    Shared Sub SaveThumbnails(inputFile As String)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.AviSynth.VerifyOK(True) Then Exit Sub

        Log.WriteHeader("Saving Thumnails")
        Log.WriteLine(inputFile)
        Log.Save()

        Dim width = s.ThumbnailWidth
        Dim columns = s.ThumbnailColumns
        Dim rows = s.ThumbnailRows
        Dim dar = MediaInfo.GetVideo(inputFile, "DisplayAspectRatio")
        Dim height = CInt(width / Convert.ToSingle(dar, CultureInfo.InvariantCulture))
        Dim shadowDistance = 5
        Dim backgroundColor = Color.Gainsboro
        Dim gap = 5

        width = width - width Mod 4
        height = height - height Mod 4

        Dim avsdoc As New VideoScript
        avsdoc.Path = Paths.SettingsDir + "Thumbnails.avs"
        avsdoc.Filters.Add(New VideoFilter("DirectShowSource(""" + inputFile + """, audio=false, convertfps=true).LanczosResize(" & width & "," & height & ")"))
        avsdoc.Filters.Add(New VideoFilter("ConvertToRGB()"))

        Dim errorMsg = ""

        Try
            avsdoc.Synchronize()
            errorMsg = p.SourceScript.GetErrorMessage
        Catch ex As Exception
            errorMsg = ex.Message
        End Try

        If errorMsg <> "" Then
            MsgError("Failed to open file." + CrLf2 + inputFile, errorMsg)
            Exit Sub
        End If

        Dim frames = avsdoc.GetFrames
        Dim count = columns * rows

        Dim bitmaps As New List(Of Bitmap)

        Using avi As New AVIFile(avsdoc.Path)
            For x = 1 To count
                avi.Position = CInt((frames / count) * x) - CInt((frames / count) / 2)
                Dim bitmap = New Bitmap(avi.GetBitmap())
                DropShadow(bitmap, Color.Black, backgroundColor, shadowDistance)
                bitmaps.Add(bitmap)
                ProcessForm.UpdateStatusThreadsafe("Extracting thumbnails: " & CInt(100 / count * x) & "%")
            Next

            width = width + shadowDistance + gap
            height = height + shadowDistance + gap
        End Using

        FileHelp.Delete(avsdoc.Path)

        Dim infoSize As String

        Dim infoWidth = MediaInfo.GetVideo(inputFile, "Width")
        Dim infoHeight = MediaInfo.GetVideo(inputFile, "Height")
        Dim infoLength = New FileInfo(inputFile).Length

        Dim infoDuration = MediaInfo.GetGeneral(inputFile, "Duration").ToInt

        If infoLength / 1024 ^ 3 > 1 Then
            infoSize = (infoLength / 1024 ^ 3).ToString("f2") + " GB"
        Else
            infoSize = CInt(infoLength / 1024 ^ 2).ToString + " MB"
        End If

        Dim infoDate As Date
        infoDate = infoDate.AddMilliseconds(infoDuration)

        Dim caption = Filepath.GetName(inputFile) + CrLf & "Size: " & infoSize & ", Duration: " +
            infoDate.ToString("HH:mm:ss") + ", Bitrate: " & CInt((infoLength * 8) / 1000 / (infoDuration / 1000)) & " Kbps" + CrLf +
            "Audio: " + MediaInfo.GetAudioCodecs(inputFile) + CrLf +
            "Video: " + MediaInfo.GetVideoCodec(inputFile) + ", " & infoWidth & " x " & infoHeight & ", " & MediaInfo.GetVideo(inputFile, "FrameRate/String")

        Dim captionHeight = 110

        Dim imageWidth = width * columns + shadowDistance + gap
        Dim imageHeight = height * rows + captionHeight

        Using bitmap As New Bitmap(imageWidth, imageHeight)
            Using g = Graphics.FromImage(bitmap)
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.Clear(backgroundColor)
                Dim rect = New RectangleF(shadowDistance + gap, 0, imageWidth - (shadowDistance + gap) * 2, captionHeight)
                Dim format As New StringFormat
                format.LineAlignment = StringAlignment.Center
                g.DrawString(caption, New Font("Tahoma", 9), Brushes.Black, rect, format)
                format.Alignment = StringAlignment.Far
                format.LineAlignment = StringAlignment.Far
                g.DrawString("StaxRip", New Font("Tahoma", 18), Brushes.White, rect, format)

                For x = 0 To bitmaps.Count - 1
                    Dim rowPos = x \ columns
                    Dim columnPos = x Mod columns
                    g.DrawImage(bitmaps(x), columnPos * width + shadowDistance + gap, rowPos * height + captionHeight)
                Next
            End Using

            bitmap.Save(inputFile.ChangeExt("jpg"), Imaging.ImageFormat.Jpeg)
        End Using
    End Sub

    Private Shared Sub DropShadow(sourceImage As Bitmap,
                                  shadowColor As Color,
                                  backgroundColor As Color,
                                  Optional shadowDistance As Integer = 5,
                                  Optional shadowOpacity As Integer = 150,
                                  Optional shadowSoftness As Integer = 4,
                                  Optional shadowRoundedEdges As Boolean = True)

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

            Using imgShadow = New Bitmap(shWidth, shHeight)
                Using g = Graphics.FromImage(imgShadow)
                    g.Clear(Color.Transparent)
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic
                    g.SmoothingMode = SmoothingMode.AntiAlias
                    Dim sre = 0

                    If shadowRoundedEdges Then sre = 1

                    Using b = New SolidBrush(Color.FromArgb(shadowOpacity, shadowColor))
                        g.FillRectangle(b, sre, sre, shWidth, shHeight)
                    End Using
                End Using

                'draw shadow
                Dim d_shWidth = sourceImage.Width + shadowDistance
                Dim d_shHeight = sourceImage.Height + shadowDistance

                Using imgTarget = New Bitmap(d_shWidth, d_shHeight)
                    Using g = Graphics.FromImage(imgTarget)
                        g.Clear(backgroundColor)
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic
                        g.SmoothingMode = SmoothingMode.AntiAlias
                        g.DrawImage(imgShadow, New Rectangle(0, 0, d_shWidth, d_shHeight), 0, 0, imgShadow.Width, imgShadow.Height, GraphicsUnit.Pixel)
                        g.DrawImage(sourceImage, New Rectangle(0, 0, sourceImage.Width, sourceImage.Height), 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel)
                    End Using

                    sourceImage = New Bitmap(imgTarget)
                End Using
            End Using
        End If
    End Sub
End Class