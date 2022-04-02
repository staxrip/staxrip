Imports System.Collections.Concurrent
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Globalization
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks

Public Enum ThumbnailerRowMode
    Fixed
    TimeInterval
End Enum


Public Class Thumbnailer
    Public Shared Function Run(pProject As Project, ParamArray pSourcePaths As String()) As ConcurrentDictionary(Of String, Boolean)
        Dim cts = New CancellationTokenSource()
        Return Task.Run(Async Function() await RunAsync(cts.Token, pProject, pSourcePaths)).Result
    End Function

    Public Shared Async Function RunAsync(pToken As CancellationToken, pProject As Project, ParamArray pSourcePaths As String()) As Task(Of ConcurrentDictionary(Of String, Boolean))
        Dim proceededSources = New ConcurrentDictionary(Of String, Boolean)

        If pSourcePaths Is Nothing OrElse pSourcePaths.Length = 0 Then Return proceededSources
        If Not Package.VapourSynth.VerifyOK(True) Then Return proceededSources

        Using semaSlim = New SemaphoreSlim(3)
            Dim taskList = New List(Of Task)()

            For Each path In pSourcePaths
                taskList.Add(New Task(Function()
                                          Dim succeeded = RunAsync(pProject, path, pToken).Result
                                          proceededSources(path) = succeeded
                                          semaSlim.Release()
                                          Return succeeded
                                      End Function)
                )
            Next

            Dim taskArray = taskList.ToArray()
            For Each t In taskArray
                Await semaSlim.WaitAsync(pToken)
                pToken.ThrowIfCancellationRequested()
                t.Start()
            Next

            Await Task.WhenAll(taskArray)
        End Using

        Return proceededSources
    End Function



    Private Shared Async Function RunAsync(pProject As Project, pSourcePath As String, pToken As CancellationToken) As Task(Of Boolean)
        If Not File.Exists(pSourcePath) Then Return False

        Dim proj As Project = pProject
        If proj Is Nothing Then
            proj = New Project()
            proj.Init()
            proj.SourceFile = pSourcePath
            proj.TargetFile = pSourcePath
        End If

        Dim defaultImageBackColor = New ColorHSL(0, 0, 0.05, 1)
        Dim dar = MediaInfo.GetVideo(pSourcePath, "DisplayAspectRatio").ToSingle()

        Dim HeaderInfoTask = GetHeaderInfosAsync(pSourcePath)

        Dim settings As ObjectStorage = proj.ThumbnailerSettings

        Dim thumbWidth = Mathf.Clamp(settings.GetInt("ThumbWidth", 600), 0, 1920)
        Dim thumbHeight = CInt(thumbWidth / dar)
        thumbWidth -= thumbWidth Mod 4
        thumbHeight -= thumbHeight Mod 4

        Dim scriptTask = PrepareVapoursynthVideoScriptAsync(pSourcePath, New Size(thumbWidth, thumbHeight), True, pToken)

        Dim interval = Mathf.Clamp(settings.GetInt("Interval", 60), 1, 1800)
        Dim columns = Mathf.Clamp(settings.GetInt("Columns", 4), 1, 50)
        Dim rows = Mathf.Clamp(settings.GetInt("Rows", 6), 1, 80)
        Dim rowMode = settings.GetInt("RowMode", ThumbnailerRowMode.Fixed)
        Dim spacerPercent = Mathf.Clamp(settings.GetInt("SpacerPercent", 20), 0, 1000)
        Dim spacer = CInt(thumbWidth / 10.0F * (spacerPercent / 100.0F))

        Dim timestampFontName = settings.GetString("TimestampFontName", "Consolas")
        Dim timestampFontSizePercent = Mathf.Clamp(settings.GetInt("TimestampFontSizePercent", 100), 10, 1000)
        Dim timestampFontSize = (thumbWidth / 20.0F) * (timestampFontSizePercent / 100.0F)
        Dim timestampFont = New Font(timestampFontName, timestampFontSize, FontStyle.Bold, GraphicsUnit.Pixel)
        Dim timestampFontColorValue = settings.GetString("TimestampFontColor", New ColorHSL(0, 0, 0.9, 1).ToHTML())
        Dim timestampFontColor = timestampFontColorValue.ToColor(New ColorHSL(0, 0, 0.9, 1))
        Dim timestampOutlineColorValue = settings.GetString("TimestampOutlineColor", New ColorHSL(0, 0, 0.1, 1).ToHTML())
        Dim timestampOutlineColor = timestampOutlineColorValue.ToColor(New ColorHSL(0, 0, 0.1, 1))
        Dim timestampOutlineStrengthPercent = Mathf.Clamp(settings.GetInt("TimestampOutlineStrengthPercent", 100), 0, 1000)
        Dim timestampOutlineStrength = (timestampFont.Height / 6.0F) * (timestampOutlineStrengthPercent / 100.0F)

        Dim timestampAlignmentValue = settings.GetString("TimestampAlignment")
        Dim timestampAlignment As ContentAlignment = Nothing
        If Not [Enum].TryParse(timestampAlignmentValue, timestampAlignment) Then timestampAlignment = ContentAlignment.BottomLeft
        Dim timestampAlignmentLeftFlags = {ContentAlignment.BottomLeft, ContentAlignment.MiddleLeft, ContentAlignment.TopLeft}
        Dim timestampAlignmentCenterFlags = {ContentAlignment.BottomCenter, ContentAlignment.MiddleCenter, ContentAlignment.TopCenter}
        Dim timestampAlignmentRightFlags = {ContentAlignment.BottomRight, ContentAlignment.MiddleRight, ContentAlignment.TopRight}
        Dim timestampAlignmentTopFlags = {ContentAlignment.TopLeft, ContentAlignment.TopCenter, ContentAlignment.TopRight}
        Dim timestampAlignmentMiddleFlags = {ContentAlignment.MiddleLeft, ContentAlignment.MiddleCenter, ContentAlignment.MiddleRight}
        Dim timestampAlignmentBottomFlags = {ContentAlignment.BottomLeft, ContentAlignment.BottomCenter, ContentAlignment.BottomRight}
        Dim timestampAlignmentMargin = New Size(2, 2)
        timestampAlignmentMargin.Width = CInt(timestampAlignmentMargin.Width * dar)

        Dim imageWidth = (spacer + thumbWidth) * columns + spacer
        Dim imageBackColorValue = settings.GetString("ImageBackColor", defaultImageBackColor.ToHTML())
        Dim imageBackColor = imageBackColorValue.ToColor(defaultImageBackColor)

        Dim header = settings.GetBool("Header", True)
        Dim headerBackColorValue = settings.GetString("HeaderBackColor", defaultImageBackColor.ToHTML())
        Dim headerBackColor = headerBackColorValue.ToColor(defaultImageBackColor)
        Dim headerFontName = settings.GetString("HeaderFontName", "Consolas")
        Dim headerFontSizePercent = Mathf.Clamp(settings.GetInt("HeaderFontSizePercent", 100), 10, 1000)
        Dim headerFontSize = (thumbWidth / 20.0F) * (columns / 4.0F) * (headerFontSizePercent / 100.0F)
        Dim headerFont = New Font(headerFontName, headerFontSize, FontStyle.Regular, GraphicsUnit.Pixel)
        Dim headerFontColorValue = settings.GetString("HeaderFontColor", New ColorHSL(0, 0, 0.8, 1).ToHTML())
        Dim headerFontColor = headerFontColorValue.ToColor(New ColorHSL(0, 0, 0.8, 1))

        Dim headerSeparatorHeightPercent = Mathf.Clamp(settings.GetInt("HeaderSeparatorHeightPercent", 0), 0, 1000)
        Dim headerSeparatorHeight = CInt(thumbWidth / 10.0F * (headerSeparatorHeightPercent / 100.0F))

        Dim imageFileFormat = settings.GetString("ImageFileFormat", "jpg").ToLowerEx()
        Dim imageQuality = Mathf.Clamp(settings.GetInt("ImageQuality", 70), 1, 100)
        Dim imageFormatGuid As Guid = Nothing
        If imageFileFormat = "bmp" Then imageFormatGuid = ImageFormat.Bmp.Guid
        If imageFileFormat = "gif" Then imageFormatGuid = ImageFormat.Gif.Guid
        If imageFileFormat = "jpg" Then imageFormatGuid = ImageFormat.Jpeg.Guid
        If imageFileFormat = "png" Then imageFormatGuid = ImageFormat.Png.Guid
        If imageFileFormat = "tif" Then imageFormatGuid = ImageFormat.Tiff.Guid
        If imageFormatGuid = Nothing Then
            imageFileFormat = "jpg"
            imageFormatGuid = ImageFormat.Jpeg.Guid
        End If

        Dim imageEPs = New EncoderParameters(1)
        imageEPs.Param(0) = New EncoderParameter(Imaging.Encoder.Quality, imageQuality)
        Dim imageCI = ImageCodecInfo.GetImageEncoders.Where(Function(arg) arg.FormatID = imageFormatGuid).FirstOrDefault

        Dim imageFilePathWithoutExtension = settings.GetString("ImageFilePathWithoutExtension", "")
        Dim imageFilePath = Macro.Expand(
                                    If(String.IsNullOrWhiteSpace(imageFilePathWithoutExtension), "%target_dir%%target_name%_Thumbnail", imageFilePathWithoutExtension),
                                    If(String.IsNullOrWhiteSpace(proj?.SourceFile) OrElse String.IsNullOrWhiteSpace(proj.TargetFile), New Project() With {.SourceFile = pSourcePath, .TargetFile = pSourcePath}, proj)
                            ) + "." + imageFileFormat

        Dim headerInfo = Await HeaderInfoTask
        Dim headerNearText = headerInfo.NearSide
        Dim headerFarText = headerInfo.FarSide

        Dim headerNextSize As Size
        Dim headerFarSize As Size
        Using tempBitmap = New Bitmap(1, 1)
            Using tempGraphics = Graphics.FromImage(tempBitmap)
                headerNextSize = tempGraphics.MeasureString(headerNearText, headerFont).ToSize()
                headerFarSize = tempGraphics.MeasureString(headerFarText, headerFont).ToSize()
            End Using
        End Using
        Dim headerHeight = Math.Max(headerNextSize.Height, headerFarSize.Height)

        Dim frames = MediaInfo.GetVideo(pSourcePath, "FrameCount").ToInt()
        Dim calculatedRows = If(rowMode = ThumbnailerRowMode.TimeInterval AndAlso interval > 0, CInt((MediaInfo.GetGeneral(pSourcePath, "Duration").ToInt() / 1000 / interval) / columns), rows)
        Dim thumbCount = columns * calculatedRows
        Dim thumbVerticalOffset = spacer + headerHeight + headerSeparatorHeight + spacer
        If Not header Then thumbVerticalOffset = spacer

        Dim imageHeight = (spacer + thumbHeight) * calculatedRows + thumbVerticalOffset
        Dim headerRect = New RectangleF(spacer, spacer, imageWidth - spacer * 2, headerHeight)

        Dim script = Await scriptTask
        If script Is Nothing Then Return False

        pToken.ThrowIfCancellationRequested()

        Dim frameServerLock As New Object()
        Dim graphicsLock As New Object()
        Using image = New Bitmap(imageWidth, imageHeight, PixelFormat.Format32bppPArgb)
            Using imageGraphics = Graphics.FromImage(image)
                imageGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic
                imageGraphics.TextRenderingHint = TextRenderingHint.AntiAlias
                imageGraphics.SmoothingMode = SmoothingMode.AntiAlias
                imageGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality
                imageGraphics.Clear(imageBackColor)

                Dim drawHeaderTask As Task = Task.CompletedTask
                If header Then
                    drawHeaderTask = Task.Run(
                        Sub()
                            If headerBackColor <> imageBackColor Then
                                Using brush As New SolidBrush(headerBackColor)
                                    SyncLock graphicsLock
                                        Dim rect = New Rectangle()
                                        imageGraphics.FillRectangle(brush, headerRect.X, headerRect.Y, headerRect.Width, headerRect.Height + headerSeparatorHeight)
                                    End SyncLock
                                End Using
                            End If

                            SyncLock graphicsLock
                                TextRenderer.DrawText(imageGraphics, headerNearText, headerFont, New Point(CInt(headerRect.Location.X), CInt(headerRect.Location.Y)), headerFontColor)
                            End SyncLock
                        End Sub,
                        pToken
                    )
                End If

                Using frameServer = FrameServerFactory.Create(script.Path)
                    Parallel.For(0, thumbCount, New ParallelOptions() With {.CancellationToken = pToken, .MaxDegreeOfParallelism = 4},
                    Sub(x)
                        Dim serverPos = CInt((frames / thumbCount) * (x + 1)) - CInt((frames / thumbCount) / 2)
                        Dim bitmap As Bitmap = Nothing

                        SyncLock frameServerLock
                            Dim stride As Integer
                            Dim data As IntPtr
                            If frameServer.GetFrame(serverPos, data, stride) = 0 Then
                                bitmap = New Bitmap(frameServer.Info.Width, frameServer.Info.Height, stride, PixelFormat.Format32bppPArgb, data)
                            End If
                        End SyncLock

                        If bitmap Is Nothing Then Return

                        Using graph = Graphics.FromImage(bitmap)
                            graph.InterpolationMode = InterpolationMode.HighQualityBicubic
                            graph.TextRenderingHint = TextRenderingHint.AntiAlias
                            graph.SmoothingMode = SmoothingMode.AntiAlias
                            graph.PixelOffsetMode = PixelOffsetMode.HighQuality

                            Dim timestamp = g.GetTimeString(serverPos / frameServer.FrameRate)

                            Using gp As New GraphicsPath()
                                Dim timestampSize = TextRenderer.MeasureText(timestamp, timestampFont)
                                Dim timestampPositionX = If(timestampAlignmentLeftFlags.Contains(timestampAlignment), timestampAlignmentMargin.Width, If(timestampAlignmentCenterFlags.Contains(timestampAlignment), (bitmap.Width - timestampSize.Width) \ 2, bitmap.Width - timestampAlignmentMargin.Width - timestampSize.Width))
                                Dim timestampPositionY = If(timestampAlignmentTopFlags.Contains(timestampAlignment), timestampAlignmentMargin.Height, If(timestampAlignmentMiddleFlags.Contains(timestampAlignment), (bitmap.Height - timestampSize.Height) \ 2, bitmap.Height - timestampAlignmentMargin.Height - timestampSize.Height))
                                Dim timestampPosition = New Point(timestampPositionX, timestampPositionY)

                                Using Format = New StringFormat()
                                    gp.AddString(timestamp, timestampFont.FontFamily, timestampFont.Style, timestampFont.Size, timestampPosition, Format)
                                End Using

                                Using pen = New Pen(timestampOutlineColor, timestampOutlineStrength)
                                    graph.DrawPath(pen, gp)
                                End Using

                                Using brush = New SolidBrush(timestampFontColor)
                                    graph.FillPath(brush, gp)
                                End Using
                            End Using
                        End Using

                        Dim rowPos = x \ columns
                        Dim columnPos = x Mod columns

                        SyncLock graphicsLock
                            imageGraphics.DrawImage(bitmap, spacer + columnPos * (thumbWidth + spacer), thumbVerticalOffset + rowPos * (thumbHeight + spacer))
                        End SyncLock

                        bitmap.Dispose()
                    End Sub
                )
                End Using

                If header Then Await drawHeaderTask
            End Using

            pToken.ThrowIfCancellationRequested()
            image.Save(imageFilePath, imageCI, imageEPs)
        End Using

        Return True
    End Function



    Private Shared Async Function PrepareVapoursynthVideoScriptAsync(filePath As String, size As Size, synchronize As Boolean, pToken As CancellationToken) As Task(Of VideoScript)
        Dim script As VideoScript = Nothing

        If Not File.Exists(filePath) Then Return script

        Await Task.Run(
            Sub()
                Dim scriptFilePath = Path.Combine(Folder.Temp, $"{filePath.Base()}_Thumbnails.vpy")
                Dim cacheFilePath = Path.Combine(Folder.Temp, $"{filePath.FileName}_Thumbnails.ffindex")

                script = New VideoScript() With {
                    .Engine = ScriptEngine.VapourSynth,
                    .Path = scriptFilePath
                }

                script.Filters.Add(New VideoFilter($"clip = core.ffms2.Source(r""{filePath}"", cachefile=r""{cacheFilePath}"", cache=False)"))

                If size <> Size.Empty Then
                    script.Filters.Add(New VideoFilter($"clip = core.resize.Spline64(clip, {size.Width},{size.Height})"))
                End If

                pToken.ThrowIfCancellationRequested()

                If synchronize Then script.Synchronize(True, True, True)

                Dim errorMsg = script.Error
                If errorMsg <> "" Then
                    script = Nothing
                End If
            End Sub, pToken
        )

        Return script
    End Function



    Private Shared Async Function GetHeaderInfosAsync(filePath As String) As Task(Of HeaderInfo)
        Dim info As HeaderInfo = Nothing

        If Not File.Exists(filePath) Then Return info

        Await Task.Run(
            Sub()
                Dim mig As Func(Of String, String) = Function(param) MediaInfo.GetGeneral(filePath, param)
                Dim print As Func(Of String, Boolean) = Function(x) Not String.IsNullOrWhiteSpace(x)
                Dim size As Func(Of Long, String) = Function(x) $"{x} Bytes ({If(x < 1000000000,
                                                                                $"{(x / 1000000).ToInvariantString("f2")} MB",
                                                                                $"{(x / 1000000000).ToInvariantString("f2")} GB")} ⇌ {If(x < (1024 ^ 3),
                                                                                                                                        $"{(x / (1024 ^ 2)).ToInvariantString("f2")} MiB",
                                                                                                                                        $"{(x / (1024 ^ 3)).ToInvariantString("f2")} GiB")})"

                Dim sb = New StringBuilder()
                sb.Append($"Filename:  {filePath.FileName}")

                sb.AppendLine()
                sb.Append($"General :  {size(mig("FileSize").ToLong())}")
                Dim duration = mig("Duration/String3")
                sb.Append($" · {If(duration.StartsWith("00:"), duration.Substring(3), duration)}")
                sb.Append($" · {mig("FrameCount")} frames")
                Dim overallBitRate = (mig("OverallBitRate").ToInt() / 1000000).ToInvariantString("f2")
                sb.Append($" · {overallBitRate} Mb/s")

                Dim videoStreamCount = MediaInfo.GetVideoCount(filePath)
                For i = 0 To videoStreamCount - 1
                    Dim streamNumber = i
                    Dim miv As Func(Of String, String) = Function(param) MediaInfo.GetVideo(filePath, streamNumber, param)

                    sb.AppendLine()
                    Dim streamPrint =
                    sb.Append($"Video{If(videoStreamCount > 1, $" #{streamNumber + 1}", "   ")}:")
                    sb.Append($"  {miv("Format").Replace("Video", "").TrimEx()}")
                    sb.Append($" · {miv("Width")}x{miv("Height")} px")
                    sb.Append($" · {miv("FrameRate")}")
                    Dim frameRateNum = miv("FrameRate_Num")
                    Dim frameRateOriginalNum = miv("FrameRate_Original_Num")
                    Dim frameRateDen = miv("FrameRate_Den")
                    Dim frameRateOriginalDen = miv("FrameRate_Original_Den")
                    If print(frameRateNum) Then sb.Append($" ({If(print(frameRateOriginalNum), frameRateOriginalNum, frameRateNum)}/{If(print(frameRateOriginalDen), frameRateOriginalDen, frameRateDen)})")
                    sb.Append($" fps")
                    sb.Append($" · {miv("BitRate").ToInt() \ 1000} kb/s")
                    Dim formatProfile = miv("Format_Profile")
                    If print(formatProfile) Then sb.Append($" · {formatProfile}")
                    Dim hdrFormat = miv("HDR_Format_Commercial")
                    sb.Append($" · {If(print(hdrFormat), hdrFormat, "SDR")}")
                    Dim bitDepth = miv("BitDepth")
                    If print(bitDepth) Then sb.Append($" ({bitDepth} bit)")
                    Dim colorSpace = miv("ColorSpace")
                    If print(colorSpace) Then sb.Append($" · {colorSpace} {miv("ChromaSubsampling/String")}")
                Next

                Dim audioStreamCount = MediaInfo.GetAudioCount(filePath)
                For i = 0 To audioStreamCount - 1
                    Dim streamNumber = i
                    Dim mia As Func(Of String, String) = Function(param) MediaInfo.GetAudio(filePath, streamNumber, param)

                    sb.AppendLine()
                    sb.Append($"Audio{If(audioStreamCount > 1, $" #{streamNumber + 1}", "   ")}:")
                    Dim lang = mia("Language/String3")
                    sb.Append($"  {If(print(lang), lang.ToTitleCase(), "Und")}")
                    sb.Append($" · {mia("Channel(s)")} Channel(s)")
                    Dim bitRate = mia("BitRate")
                    sb.Append($" · {If(print(bitRate), (bitRate.ToInt() / 1000).ToString("f0"), "-"),4} kb/s")
                    sb.Append($" · {mia("SamplingRate")} Hz")
                    sb.Append($" · {mia("Format/String")}")
                    Dim audioTitle = mia("Title")
                    If print(audioTitle) Then sb.Append($" · ""{audioTitle}""")
                Next

                Dim textStreamCount = MediaInfo.GetSubtitleCount(filePath)
                If textStreamCount > 0 Then
                    sb.AppendLine()
                    sb.Append($"Text    : ")

                    For i = 0 To textStreamCount - 1
                        Dim streamNumber = i
                        Dim mit As Func(Of String, String) = Function(param) MediaInfo.GetText(filePath, streamNumber, param)

                        If streamNumber > 0 Then sb.Append($" |")
                        sb.Append($" {mit("Language/String3").ToTitleCase()}")
                        sb.Append($"·{mit("Format").Replace("UTF-8", "SRT")}")
                        If mit("Default") = "Yes" Then sb.Append($"·[DEFAULT]")
                        If mit("Forced") = "Yes" Then sb.Append($"·[FORCED]")
                    Next
                End If

                info = New HeaderInfo With {
                    .NearSide = sb.ToString()
                }
            End Sub
        )

        Return info
    End Function


    Private Class HeaderInfo
        Property NearSide As String
        Property FarSide As String
    End Class
End Class
