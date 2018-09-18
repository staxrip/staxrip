Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Globalization
Imports System.Threading.Tasks

Public Class ImageHelp
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
        graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        graphics.DrawString(Convert.ToChar(CInt(symbol)), font, Brushes.Black, -fontHeight * 0.1F, fontHeight * 0.07F)
        graphics.Dispose()
        font.Dispose()
        Return bitmap
    End Function
End Class

Public Class Thumbnails
    Shared Sub SaveThumbnails(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.AviSynth.VerifyOK(True) Then Exit Sub

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        proj.Log.WriteHeader("Saving Thumbnails")
        proj.Log.WriteLine(inputFile)
        proj.Log.Save(proj)

        Dim fontname = "Tahoma"
        Dim width = s.Storage.GetInt("Thumbnail Width", 200)
        Dim columnCount = s.Storage.GetInt("Thumbnail Columns", 4)
        Dim rowCount = s.Storage.GetInt("Thumbnail Rows", 6)
        Dim dar = MediaInfo.GetVideo(inputFile, "DisplayAspectRatio")
        Dim height = CInt(width / Convert.ToSingle(dar, CultureInfo.InvariantCulture))
        Dim gap = CInt((width * columnCount) * 0.005)
        Dim font = New Font(fontname, (width * columnCount) \ 80, FontStyle.Regular, GraphicsUnit.Pixel)
        Dim foreColor = Color.Black

        width = width - width Mod 4
        height = height - height Mod 4

        Dim cachePath = Folder.Settings + "Thumbnails.ffindex"
        Dim avsdoc As New VideoScript
        avsdoc.Path = Folder.Settings + "Thumbnails.avs"
        avsdoc.Filters.Add(New VideoFilter("FFVideoSource(""" + inputFile + """, cachefile = """ + cachePath + """, colorspace = ""YV12"").Spline64Resize(" & width & "," & height & ")"))
        avsdoc.Filters.Add(New VideoFilter("ConvertToRGB()"))

        g.ffmsindex(inputFile, cachePath, False, proj)

        Dim errorMsg = ""

        Try
            avsdoc.Synchronize()
            Dim mode = s.Storage.GetInt("Thumbnail Mode")
            Dim intervalSec = s.Storage.GetInt("Thumbnail Interval")
            If intervalSec <> 0 AndAlso mode = 1 Then rowCount = CInt((avsdoc.GetSeconds / intervalSec) / columnCount)
            errorMsg = p.SourceScript.GetErrorMessage
        Catch ex As Exception
            errorMsg = ex.Message
        End Try

        If errorMsg <> "" Then
            MsgError("Failed to open file." + BR2 + inputFile, errorMsg)
            Exit Sub
        End If

        Dim frames = avsdoc.GetFrames
        Dim count = columnCount * rowCount

        Dim bitmaps As New List(Of Bitmap)

        Using avi As New AVIFile(avsdoc.Path)
            For x = 1 To count
                avi.Position = CInt((frames / count) * x) - CInt((frames / count) / 2)
                Dim bitmap = New Bitmap(avi.GetBitmap())

                Using g = Graphics.FromImage(bitmap)
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic
                    g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                    g.SmoothingMode = SmoothingMode.AntiAlias
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality

                    Dim dur = TimeSpan.FromSeconds(avi.FrameCount / avi.FrameRate)
                    Dim timestamp = StaxRip.g.GetTimeString(avi.Position / avi.FrameRate)
                    Dim ft As New Font("Segoe UI", font.Size, FontStyle.Bold, GraphicsUnit.Pixel)

                    Dim gp As New GraphicsPath()
                    Dim sz = g.MeasureString(timestamp, ft)
                    Dim pt As Point
                    Dim pos = s.Storage.GetInt("Thumbnail Position", 1)

                    If pos = 0 OrElse pos = 2 Then
                        pt.X = ft.Height \ 10
                    Else
                        pt.X = CInt(bitmap.Width - sz.Width - ft.Height / 10)
                    End If

                    If pos = 2 OrElse pos = 3 Then
                        pt.Y = CInt(bitmap.Height - sz.Height)
                    Else
                        pt.Y = 0
                    End If

                    gp.AddString(timestamp, ft.FontFamily, CInt(ft.Style), ft.Size, pt, New StringFormat())

                    Using pen As New Pen(Brushes.Black, ft.Height \ 5)
                        g.DrawPath(pen, gp)
                    End Using

                    g.FillPath(Brushes.Gainsboro, gp)
                End Using

                bitmaps.Add(bitmap)
            Next

            width = width + gap
            height = height + gap
        End Using

        FileHelp.Delete(cachePath)
        FileHelp.Delete(avsdoc.Path)

        Dim infoSize As String

        Dim infoWidth = MediaInfo.GetVideo(inputFile, "Width")
        Dim infoHeight = MediaInfo.GetVideo(inputFile, "Height")
        Dim infoLength = New FileInfo(inputFile).Length
        Dim infoDuration = MediaInfo.GetGeneral(inputFile, "Duration").ToInt
        Dim audioCodecs = MediaInfo.GetAudioCodecs(inputFile).Replace(" / ", "  ")

        If audioCodecs.Length > 40 Then audioCodecs = audioCodecs.Shorten(40) + "..."

        If infoLength / 1024 ^ 3 > 1 Then
            infoSize = (infoLength / 1024 ^ 3).ToInvariantString("f2") + "GB"
        Else
            infoSize = CInt(infoLength / 1024 ^ 2).ToString + "MB"
        End If

        Dim caption = FilePath.GetName(inputFile) + BR & infoSize & "  " +
            g.GetTimeString(infoDuration / 1000) + "  " &
            (((infoLength * 8) / 1000 / (infoDuration / 1000)) / 1000).ToInvariantString("f1") & "Mb/s" + BR +
            audioCodecs + BR + MediaInfo.GetVideoCodec(inputFile) + "  " & infoWidth & "x" & infoHeight & "  " &
            MediaInfo.GetVideo(inputFile, "FrameRate").ToSingle.ToInvariantString + "fps"

        Dim captionSize = TextRenderer.MeasureText(caption, font)
        Dim captionHeight = captionSize.Height + font.Height \ 3

        Dim imageWidth = width * columnCount + gap
        Dim imageHeight = height * rowCount + captionHeight

        Using bitmap As New Bitmap(imageWidth, imageHeight)
            Using g = Graphics.FromImage(bitmap)
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.PixelOffsetMode = PixelOffsetMode.HighQuality
                g.Clear(s.ThumbnailBackgroundColor)
                Dim rect = New RectangleF(gap, 0, imageWidth - gap * 2, captionHeight)
                Dim format As New StringFormat
                format.LineAlignment = StringAlignment.Center

                Using brush As New SolidBrush(foreColor)
                    g.DrawString(caption, font, brush, rect, format)
                    format.Alignment = StringAlignment.Far
                    format.LineAlignment = StringAlignment.Center
                    g.DrawString("StaxRip", New Font(fontname, font.Height * 2, FontStyle.Bold, GraphicsUnit.Pixel), brush, rect, format)
                End Using

                For x = 0 To bitmaps.Count - 1
                    Dim rowPos = x \ columnCount
                    Dim columnPos = x Mod columnCount
                    g.DrawImage(bitmaps(x), columnPos * width + gap, rowPos * height + captionHeight)
                Next
            End Using

            Dim params = New EncoderParameters(1)
            params.Param(0) = New EncoderParameter(Encoder.Quality, s.Storage.GetInt("Thumbnail Compression Quality", 95))
            Dim info = ImageCodecInfo.GetImageEncoders.Where(Function(arg) arg.FormatID = ImageFormat.Jpeg.Guid).First
            bitmap.Save(inputFile.ChangeExt("jpg"), info, params)
        End Using
    End Sub
End Class