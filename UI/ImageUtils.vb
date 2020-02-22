Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Globalization
Imports System.Threading.Tasks

Public Class ImageHelp
    Private Shared Coll As PrivateFontCollection
    Private Shared AwesomePath As String = Folder.Apps + "\Fonts\FontAwesome.ttf"
    Private Shared SegoePath As String = Folder.Apps + "\Fonts\Segoe-MDL2-Assets.ttf"
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
        graphics.TextRenderingHint = TextRenderingHint.AntiAlias
        graphics.DrawString(Convert.ToChar(CInt(symbol)), font, Brushes.Black, -fontHeight * 0.1F, fontHeight * 0.07F)
        graphics.Dispose()
        font.Dispose()

        Return bitmap
    End Function
End Class

Public Class Thumbnails
    Shared Sub SaveThumbnails(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Exit Sub
        End If

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        Dim fontname = "DejaVu Serif"
        Dim fontOptions = "Mikadan"

        Dim width = s.Storage.GetInt("Thumbnail Width", 500)
        Dim columnCount = s.Storage.GetInt("Thumbnail Columns", 4)
        Dim rowCount = s.Storage.GetInt("Thumbnail Rows", 6)
        Dim dar = MediaInfo.GetVideo(inputFile, "DisplayAspectRatio")
        Dim height = CInt(width / Convert.ToSingle(dar, CultureInfo.InvariantCulture))
        Dim gap = CInt((width * columnCount) * (s.Storage.GetInt("Thumbnail Margin", 5) / 1000))
        Dim font = New Font(fontname, (width * columnCount) \ 80, FontStyle.Bold, GraphicsUnit.Pixel)
        Dim foreColor = Color.Black

        width = width - width Mod 16
        height = height - height Mod 16

        Dim script As New VideoScript
        script.Path = Path.Combine(Folder.Temp + "Thumbnails.avs")

        If inputFile.Ext = "mp4" Then
            script.Filters.Add(New VideoFilter($"LSMASHVideoSource(""{inputFile}"")"))
        Else
            script.Filters.Add(New VideoFilter($"FFVideoSource(""{inputFile}"")"))
        End If

        script.Filters.Add(New VideoFilter($"Spline64Resize({width},{height})"))

        Dim mode = s.Storage.GetInt("Thumbnail Mode")
        Dim intervalSec = s.Storage.GetInt("Thumbnail Interval")

        If intervalSec <> 0 AndAlso mode = 1 Then
            rowCount = CInt((script.GetSeconds / intervalSec) / columnCount)
        End If

        Dim errorMsg = script.GetError

        If errorMsg <> "" Then
            MsgError("Failed to open file." + BR2 + inputFile, errorMsg)
            Exit Sub
        End If

        Dim frames = script.GetFrameCount
        Dim count = columnCount * rowCount
        Dim bitmaps As New List(Of Bitmap)

        script.Synchronize(True, True, True)

        Using server As New FrameServer(script.Path)
            Dim serverPos As Integer

            For x = 1 To count
                serverPos = CInt((frames / count) * x) - CInt((frames / count) / 2)
                Dim bitmap = BitmapUtil.CreateBitmap(server, serverPos)

                Using g = Graphics.FromImage(bitmap)
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic
                    g.TextRenderingHint = TextRenderingHint.AntiAlias
                    g.SmoothingMode = SmoothingMode.AntiAlias
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality

                    Dim dur = TimeSpan.FromSeconds(server.Info.FrameCount / server.FrameRate)
                    Dim timestamp = StaxRip.g.GetTimeString(serverPos / server.FrameRate)
                    Dim ft As New Font("Segoe UI", font.Size, FontStyle.Bold, GraphicsUnit.Pixel)

                    Using gp As New GraphicsPath()
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
                End Using

                bitmaps.Add(bitmap)
            Next

            width = width + gap
            height = height + gap
        End Using

        If inputFile.Ext = "mp4" Then
            FileHelp.Delete(inputFile + ".lwi")
        Else
            FileHelp.Delete(inputFile + ".ffindex")
        End If

        Dim infoSize As String
        Dim infoWidth = MediaInfo.GetVideo(inputFile, "Width")
        Dim infoHeight = MediaInfo.GetVideo(inputFile, "Height")
        Dim infoLength = New FileInfo(inputFile).Length
        Dim infoDuration = MediaInfo.GetGeneral(inputFile, "Duration").ToInt
        Dim audioCodecs = MediaInfo.GetAudioCodecs(inputFile)

        If audioCodecs = "" Then
            audioCodecs = ""
        End If

        Dim channels = MediaInfo.GetAudio(inputFile, "Channel(s)").ToInt
        Dim subSampling = MediaInfo.GetVideo(inputFile, "ChromaSubsampling").Replace(":", "")
        If subSampling = "" Then subSampling = ""
        Dim colorSpace = MediaInfo.GetVideo(inputFile, "ColorSpace").ToLower
        If colorSpace = "" Then colorSpace = ""
        Dim profile = MediaInfo.GetVideo(inputFile, "Format_Profile").Shorten(4)
        If profile = "" Then profile = "Main"
        Dim scanType = MediaInfo.GetVideo(inputFile, "ScanType")

        Dim audioSound As String

        If channels = 2 Then audioSound = "Stereo"
        If channels = 1 Then audioSound = "Mono"
        If channels = 6 Then audioSound = "Surround Sound"
        If channels = 8 Then audioSound = "Surround Sound"
        If channels = 0 Then audioSound = ""

        If infoLength / 1024 ^ 3 > 1 Then
            infoSize = (infoLength / 1024 ^ 3).ToInvariantString("f2") + "GB"
        Else
            infoSize = CInt(infoLength / 1024 ^ 2).ToString + "MB"
        End If

        Dim caption = "File: " + FilePath.GetName(inputFile) + BR & "Size: " + MediaInfo.GetGeneral(inputFile, "FileSize") + " bytes" + " (" + infoSize + ")" & ", " + "Duration: " + StaxRip.g.GetTimeString(infoDuration / 1000) + ", avg.bitrate: " + MediaInfo.GetGeneral(inputFile, "OverallBitRate_String") + BR +
            "Audio: " + audioCodecs + ", " + MediaInfo.GetAudio(inputFile, "SamplingRate_String") + ", " + audioSound + ", " + MediaInfo.GetAudio(inputFile, "BitRate_String") + BR +
            "Video: " + MediaInfo.GetVideo(inputFile, "Format") + " (" + profile + ")" + ", " + colorSpace + subSampling + scanType.Shorten(1).ToLower() + ", " + MediaInfo.GetVideo(inputFile, "Width") & "x" & MediaInfo.GetVideo(inputFile, "Height") & ", " + MediaInfo.GetVideo(inputFile, "BitRate_String") + ", " & MediaInfo.GetVideo(inputFile, "FrameRate").ToSingle.ToInvariantString + "fps".Replace(", ", "")

        caption = caption.Replace(" ,", "")

        Dim captionSize = TextRenderer.MeasureText(caption, font)
        Dim captionHeight = captionSize.Height + font.Height \ 3
        Dim waterMark = s.Storage.GetBool("Logo", False)
        Dim imageWidth = width * columnCount + gap
        Dim imageHeight = height * rowCount + captionHeight

        Using bitmap As New Bitmap(imageWidth, imageHeight)
            Using g = Graphics.FromImage(bitmap)
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.TextRenderingHint = TextRenderingHint.AntiAlias
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

                    If waterMark = False Then
                        g.DrawString("StaxRip", New Font(fontOptions, font.Height * 2, FontStyle.Bold, GraphicsUnit.Pixel), brush, rect, format)
                    End If
                End Using

                For x = 0 To bitmaps.Count - 1
                    Dim rowPos = x \ columnCount
                    Dim columnPos = x Mod columnCount
                    g.DrawImage(bitmaps(x), columnPos * width + gap, rowPos * height + captionHeight)
                Next
            End Using

            Dim directoryStatus = s.Storage.GetBool("StaxRipOutput", False)
            Dim directoryLocation = s.Storage.GetString("StaxRipDirectory", p.DefaultTargetFolder)
            Dim export = Path.Combine(directoryLocation, inputFile.Base)
            Dim options = s.Storage.GetString("Picture Format", "png")

            Try
                If options = "jpg" Then
                    Dim params = New EncoderParameters(1)
                    params.Param(0) = New EncoderParameter(Imaging.Encoder.Quality, s.Storage.GetInt("Thumbnail Compression Quality", 95))
                    Dim info = ImageCodecInfo.GetImageEncoders.Where(Function(arg) arg.FormatID = ImageFormat.Jpeg.Guid).First

                    If directoryStatus = True Then
                        bitmap.Save(export.ChangeExt("jpg"), info, params)
                    Else
                        bitmap.Save(inputFile.ChangeExt("jpg"), info, params)
                    End If
                ElseIf options = "png" Then
                    Dim info = ImageCodecInfo.GetImageEncoders.Where(Function(arg) arg.FormatID = ImageFormat.Png.Guid).First

                    If directoryStatus = True Then
                        bitmap.Save(export.ChangeExt("png"), info, Nothing)
                    Else
                        bitmap.Save(inputFile.ChangeExt("png"), info, Nothing)
                    End If
                ElseIf options = "tiff" Then
                    Dim info = ImageCodecInfo.GetImageEncoders.Where(Function(arg) arg.FormatID = ImageFormat.Tiff.Guid).First

                    If directoryStatus = True Then
                        bitmap.Save(export.ChangeExt("tiff"), info, Nothing)
                    Else
                        bitmap.Save(inputFile.ChangeExt("tiff"), info, Nothing)
                    End If
                ElseIf options = "bmp" Then
                    Dim info = ImageCodecInfo.GetImageEncoders.Where(Function(arg) arg.FormatID = ImageFormat.Bmp.Guid).First

                    If directoryStatus = True Then
                        bitmap.Save(export.ChangeExt("bmp"), info, Nothing)
                    Else
                        bitmap.Save(inputFile.ChangeExt("bmp"), info, Nothing)
                    End If
                End If
            Catch ex As Exception
                g.ShowException(ex)
            End Try
        End Using
    End Sub
End Class