Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Globalization
Imports System.Threading.Tasks

Class ImageHelp
    Private Shared Coll As PrivateFontCollection
    Private Shared AwesomePath As String = Folder.Startup + "FontAwesome.ttf"
    Private Shared SegoePath As String = Folder.Startup + "Segoe-MDL2-Assets.ttf"
    Private Shared FontFilesExist As Boolean = File.Exists(AwesomePath) AndAlso File.Exists(SegoePath)

    Shared Async Function GetSymbolImageAsync(symbol As Symbol) As Task(Of Image)
        Return Await Task.Run(Of Image)(Function() GetSymbolImage(symbol))
    End Function

    Shared Function GetSymbolImage(symbol As Symbol) As Image
        If Not FontFilesExist Then Return Nothing
        Dim legacy = OSVersion.Current < OSVersion.Windows10

        If Coll Is Nothing Then
            Coll = New PrivateFontCollection
            Coll.AddFontFile(AwesomePath)
            If legacy Then Coll.AddFontFile(SegoePath)
        End If

        Dim family As FontFamily

        If CInt(symbol) > 61400 Then
            If Coll.Families.Count > 0 Then family = Coll.Families(0)
        Else
            If legacy Then
                If Coll.Families.Count > 1 Then family = Coll.Families(1)
            Else
                family = New FontFamily("Segoe MDL2 Assets")
            End If
        End If

        If family Is Nothing Then Return Nothing
        Dim font As New Font(family, 12)
        Dim fontHeight = font.Height
        Dim bitmap As New Bitmap(CInt(fontHeight * 1.1F), CInt(fontHeight * 1.1F))
        Dim graphics = Drawing.Graphics.FromImage(bitmap)
        'graphics.Clear(Color.Orange)
        graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        graphics.DrawString(Convert.ToChar(CInt(symbol)), font, Brushes.Black, -fontHeight * 0.1F, fontHeight * 0.07F)
        graphics.Dispose()
        font.Dispose()

        Return bitmap
    End Function
End Class

Class Thumbnails
    Shared Sub SaveThumbnails(inputFile As String)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.AviSynth.VerifyOK(True) Then Exit Sub

        Log.WriteHeader("Saving Thumnails")
        Log.WriteLine(inputFile)
        Log.Save()

        Dim width = s.Storage.GetInt("Thumbnail Width", 260)
        Dim columns = s.Storage.GetInt("Thumbnail Columns", 3)
        Dim rows = s.Storage.GetInt("Thumbnail Rows", 12)
        Dim dar = MediaInfo.GetVideo(inputFile, "DisplayAspectRatio")
        Dim height = CInt(width / Convert.ToSingle(dar, CultureInfo.InvariantCulture))
        Dim shadowDistance = 5
        Dim backgroundColor = Color.Gainsboro
        Dim gap = 5

        width = width - width Mod 4
        height = height - height Mod 4

        Dim avsdoc As New VideoScript
        avsdoc.Path = Folder.Settings + "Thumbnails.avs"
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
            MsgError("Failed to open file." + BR2 + inputFile, errorMsg)
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

        Dim caption = Filepath.GetName(inputFile) + BR & "Size: " & infoSize & ", Duration: " +
            infoDate.ToString("HH:mm:ss") + ", Bitrate: " & CInt((infoLength * 8) / 1000 / (infoDuration / 1000)) & " Kbps" + BR +
            "Audio: " + MediaInfo.GetAudioCodecs(inputFile) + BR +
            "Video: " + MediaInfo.GetVideoCodec(inputFile) + ", " & infoWidth & " x " & infoHeight & ", " & MediaInfo.GetVideo(inputFile, "FrameRate").ToSingle.ToString("f3", CultureInfo.InvariantCulture).TrimEnd({"0"c, "."c}) + "fps"

        Dim captionHeight = 110

        Dim imageWidth = width * columns + shadowDistance + gap
        Dim imageHeight = height * rows + captionHeight

        Using bitmap As New Bitmap(imageWidth, imageHeight)
            Using g = Graphics.FromImage(bitmap)
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
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

            Dim params = New EncoderParameters(1)
            params.Param(0) = New EncoderParameter(Encoder.Quality, s.Storage.GetInt("Thumbnail Compression Quality", 95))
            Dim info = ImageCodecInfo.GetImageEncoders.Where(Function(arg) arg.FormatID = ImageFormat.Jpeg.Guid).First
            bitmap.Save(inputFile.ChangeExt("jpg"), info, params)
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

            If shadowColor = Color.Transparent Then shadowColor = Color.Black
            If backgroundColor = Color.Transparent Then backgroundColor = Color.White

            'get shadow
            Dim shWidth = CInt(sourceImage.Width / shadowSoftness)
            Dim shHeight = CInt(sourceImage.Height / shadowSoftness)

            Using imgShadow = New Bitmap(shWidth, shHeight)
                Using g = Graphics.FromImage(imgShadow)
                    g.Clear(Color.Transparent)
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic
                    g.SmoothingMode = SmoothingMode.AntiAlias
                    Dim sre As Integer
                    If shadowRoundedEdges Then sre = 1

                    Using b = New SolidBrush(Color.FromArgb(shadowOpacity, shadowColor))
                        g.FillRectangle(b, sre, sre, shWidth, shHeight)
                    End Using
                End Using

                'draw shadow
                Dim dsWidth = sourceImage.Width + shadowDistance
                Dim dsHeight = sourceImage.Height + shadowDistance

                Using imgTarget = New Bitmap(dsWidth, dsHeight)
                    Using g = Graphics.FromImage(imgTarget)
                        g.Clear(backgroundColor)
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic
                        g.SmoothingMode = SmoothingMode.AntiAlias
                        g.DrawImage(imgShadow, New Rectangle(0, 0, dsWidth, dsHeight), 0, 0, imgShadow.Width, imgShadow.Height, GraphicsUnit.Pixel)
                        g.DrawImage(sourceImage, New Rectangle(0, 0, sourceImage.Width, sourceImage.Height), 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel)
                    End Using

                    sourceImage = New Bitmap(imgTarget)
                End Using
            End Using
        End If
    End Sub
End Class