
Imports Microsoft.VisualBasic
Imports StaxRip.UI

Partial Public Class MainForm
    Inherits FormBase

    Function Assistant(Optional refreshScript As Boolean = True) As Boolean
        If SkipAssistant Then Return False

        If ThemeRefresh Then
            ApplyTheme()
            ThemeRefresh = False
        End If

        If refreshScript Then
            p.Script.Synchronize(False, False)
        ElseIf p.Script.Info.FrameCount = 0 Then
            Using server = FrameServerFactory.Create(p.Script.Path)
                p.Script.Info = server.Info
                p.Script.Error = server.Error
            End Using
        End If

        g.CheckForModifiedDolbyVisionLevel5Data()

        Dim isCropped = p.Script.IsFilterActive("Crop")
        Dim isResized = p.Script.IsFilterActive("Resize")

        tbTargetWidth.ReadOnly = Not isResized
        tbTargetHeight.ReadOnly = Not isResized

        Dim cropw = p.SourceWidth
        Dim croph = p.SourceHeight

        If isCropped Then
            cropw = p.SourceWidth - p.CropLeft - p.CropRight
            croph = p.SourceHeight - p.CropTop - p.CropBottom
        End If

        Dim isValidAnamorphicSize = (p.TargetWidth = 1440 AndAlso p.TargetHeight = 1080) OrElse
            (p.TargetWidth = 960 AndAlso p.TargetHeight = 720)

        If Not isResized Then
            If p.TargetWidth <> cropw Then
                tbTargetWidth.Text = cropw.ToString
            End If

            If p.TargetHeight <> croph Then
                tbTargetHeight.Text = croph.ToString
            End If
        End If

        lAspectRatioError.Text = Calc.GetAspectRatioError.ToInvariantString("f2") + "%"
        lCrop.Text = If(isCropped, $"{cropw}/{croph}", "disabled")
        lCrop2.Text = If(isCropped, $"{p.CropLeft}, {p.CropRight} / {p.CropTop}, {p.CropBottom}", "-")

        Dim widthZoom = p.TargetWidth / cropw * 100
        Dim heightZoom = p.TargetHeight / croph * 100

        lZoom.Text = widthZoom.ToInvariantString("f1") + "/" + heightZoom.ToInvariantString("f1")
        lPixel.Text = CInt(p.TargetWidth * p.TargetHeight).ToString

        SetSlider()
        Dim trackBarValue = CInt(p.TargetWidth \ Math.Max(TrackBarInterval, p.ForcedOutputMod) - (tbResize.Maximum \ 3))
        trackBarValue = Math.Min(trackBarValue, tbResize.Maximum)
        trackBarValue = Math.Max(trackBarValue, tbResize.Minimum)
        tbResize.Value = trackBarValue


        Dim par = Calc.GetTargetPAR

        lPAR.Text = If(Calc.IsARSignalingRequired OrElse (par.X = 1 AndAlso par.Y = 1), par.X & ":" & par.Y, "n/a")
        lDAR.Text = Calc.GetTargetDAR.ToInvariantString.Shorten(8)
        lSAR.Text = (p.TargetWidth / p.TargetHeight).ToInvariantString.Shorten(8)
        lSourceDar.Text = Calc.GetSourceDAR.ToInvariantString.Shorten(8)
        par = Calc.GetSimpleSourcePAR
        lSourcePAR.Text = par.X & ":" & par.Y

        If p.SourceSeconds > 0 Then
            Dim size = If(p.SourceVideoSize > 0, p.SourceVideoSize, p.SourceSize)
            Dim sizeText = If(size / PrefixedSize(2).Factor < SizePrefix.Base, CInt(size / PrefixedSize(2).Factor).ToString + PrefixedSize(2).Unit, (size / PrefixedSize(3).Factor).ToString("f1") + PrefixedSize(3).Unit)

            If size <> p.SourceVideoSize Then
                sizeText = $"[{sizeText}]"
            End If

            lSource1.Text = lSource1.GetMaxTextSpace(
                g.GetTimeString(p.SourceSeconds),
                sizeText,
                If(p.SourceBitrate > 0, (p.SourceBitrate / 1000).ToInvariantString("f1") + "Mb/s", ""),
                p.SourceFrameRate.ToInvariantString.Shorten(9) + "fps",
                p.SourceFrameRateMode)

            lSource2.Text = lSource1.GetMaxTextSpace(
                p.SourceWidth.ToString + "x" + p.SourceHeight.ToString, p.SourceColorSpace,
                p.SourceChromaSubsampling, If(p.SourceVideoBitDepth <> 0, p.SourceVideoBitDepth & "Bits", ""),
                p.SourceVideoHdrFormat,
                p.SourceScanType, If(p.SourceScanType.EqualsAny("Interlaced", "MBAFF"), p.SourceScanOrder, ""),
                p.SourceVideoFormat, p.SourceVideoFormatProfile)

            lTarget1.Text = lSource1.GetMaxTextSpace(g.GetTimeString(p.TargetSeconds),
                p.TargetFrameRate.ToInvariantString.Shorten(9) + "fps", p.Script.Info.Width & "x" & p.Script.Info.Height,
                "Audio Bitrate: " & CInt(Calc.GetAudioBitrate))

            If p.VideoEncoder.IsCompCheckEnabled Then
                laTarget2.Text = lSource1.GetMaxTextSpace(
                    "Quality: " & CInt(Calc.GetPercent).ToInvariantString() + " %",
                    "Compressibility: " + p.Compressibility.ToInvariantString("f2"))
            Else
                Dim subtitles = p.VideoEncoder.Muxer.Subtitles.Where(Function(i) i.Enabled)
                laTarget2.Text = "Subtitles: " & subtitles.Count & " " + subtitles.Select(Function(i) i.TypeName).Distinct.Join("/")
            End If
        Else
            lTarget1.Text = ""
            lSource1.Text = ""
            laTarget2.Text = ""
            lSource2.Text = ""
        End If

        AssistantClickAction = Nothing
        CanIgnoreTip = True
        AssistantPassed = False

        If p.VideoEncoder.Muxer.TagFile <> "" AndAlso File.Exists(p.VideoEncoder.Muxer.TagFile) AndAlso
            p.VideoEncoder.Muxer.Tags.Count > 0 Then

            If ProcessTip("In the container options there is both a tag file and tags in the Tags tab defined. Only one can be used, the file will be ignored.") Then
                Return Warn("Tags are defined twice")
            End If
        End If

        If p.VideoEncoder.Muxer.CoverFile <> "" AndAlso TypeOf p.VideoEncoder.Muxer Is MkvMuxer Then
            If Not p.VideoEncoder.Muxer.CoverFile.Base.EqualsAny("cover", "small_cover", "cover_land", "small_cover_land") OrElse
                Not p.VideoEncoder.Muxer.CoverFile.Ext.EqualsAny("jpg", "png") Then

                If ProcessTip("The cover file name bust be cover, small_cover, cover_land or small_cover_land, the file type must be jpg or png.") Then
                    Return Block("Invalid Cover File Name")
                End If
            End If
        End If

        Dim enc = TryCast(p.VideoEncoder, BasicVideoEncoder)
        If enc IsNot Nothing Then
            Dim param = enc.CommandLineParams.GetOptionParam("--vpp-resize")

            If param IsNot Nothing AndAlso param.Value > 0 AndAlso Not p.Script.IsFilterActive("Resize", "Hardware Encoder") Then
                If ProcessTip("In order to use a resize filter of the hardware encoder select 'Hardware Encoder' as resize filter from the filters menu.") Then
                    Return Block("Invalid Filter Setting")
                End If
            End If
        End If

        If Not p.BatchMode Then
            If p.SourceFile <> "" AndAlso p.Script.IsAviSynth AndAlso Not TextEncoding.ArePathsSupportedByProcessEncoding AndAlso
                ProcessTip("AviSynth Unicode support requires at least Windows 10 1903.") Then
                Return Block("Text Encoding Limitation", lgbEncoder.Label)
            End If

            If p.Script.Filters.Count = 0 OrElse Not p.Script.Filters(0).Active OrElse
                p.Script.Filters(0).Category <> "Source" Then

                If ProcessTip("The first filter must have the category Source.") Then
                    Return Block("Invalid Filter Setup")
                End If
            End If

            If p.SourceSeconds = 0 AndAlso ProcessTip("Click here to open a source file.") Then
                CanIgnoreTip = False
                Return Warn("Assistant", AddressOf ShowOpenSourceDialog)
            End If

            If p.SourceFile = p.TargetFile AndAlso ProcessTip("The source and target filepath is identical.") Then
                Return Block("Invalid Target Path", tbSourceFile, tbTargetFile)
            End If

            If p.RemindToCrop AndAlso TypeOf p.VideoEncoder IsNot NullEncoder AndAlso
                ProcessTip("Click here to open the crop dialog. When done continue with Next.") Then

                Return Warn("Crop", AddressOf ShowCropDialog)
            End If

            If p.WarnNoAudio Then
                If Not p.AudioTracks.Any(Function(track) track.IsRelevant) Then
                    If ProcessTip("There will be no audio in the output file.") Then
                        Return Warn("No audio", p.AudioTracks.Select(Function(x) x.TextEdit).ToArray())
                    End If
                End If
            End If

            If p.WarnIdenticalAudio Then
                Dim fileGroups = p.AudioTracks.Where(Function(x) x.TextEdit.Text <> "" AndAlso x.AudioProfile.Stream Is Nothing).GroupBy(Function(g) g.AudioProfile.File).Where(Function(x) x.Count() > 1).ToList()
                If fileGroups.Any() Then
                    If ProcessTip($"Some audio source files are identical.") Then
                        Return Warn("Suspicious Audio Settings", fileGroups.SelectMany(Function(x) x.AsEnumerable().Select(Function(s) s.TextEdit)).ToArray())
                    End If
                End If

                Dim streamGroups = p.AudioTracks.Where(Function(x) x.AudioProfile.Stream IsNot Nothing).GroupBy(Function(g) g.AudioProfile.Stream.Index).Where(Function(x) x.Count() > 1).ToList()
                If streamGroups.Any() Then
                    If ProcessTip($"Some audio source streams are identical.") Then
                        Return Warn("Suspicious Audio Settings", streamGroups.SelectMany(Function(x) x.AsEnumerable().Select(Function(s) s.TextEdit)).ToArray())
                    End If
                End If
            End If

            If Not p.VideoEncoder.Muxer.IsSupported(p.VideoEncoder.OutputExt) Then
                If ProcessTip("The encoder outputs '" + p.VideoEncoder.OutputExt + "' but the container '" + p.VideoEncoder.Muxer.Name + "' supports only " + p.VideoEncoder.Muxer.SupportedInputTypes.Join(", ") + ".") Then
                    Return Block("Encoder conflicts with container", lgbEncoder.Label, llMuxer)
                End If
            End If

            For Each ap In AudioProfile.GetProfiles
                If ap.File = "" Then Continue For

                If TypeOf ap Is GUIAudioProfile Then
                    Dim gap = DirectCast(ap, GUIAudioProfile)

                    If (gap.AudioCodec = AudioCodec.AC3 OrElse gap.AudioCodec = AudioCodec.EAC3) AndAlso
                        (gap.Channels = 7 OrElse gap.Channels = 8) AndAlso gap.GetEncoder = GuiAudioEncoder.ffmpeg Then

                        If ProcessTip("AC3/EAC3 6.1/7.1 is not supported by ffmpeg.") Then
                            Return Block("Invalid Audio Channel Count", GetAudioTextBox(ap))
                        End If
                    End If
                End If

                If ap.AudioCodec = AudioCodec.AC3 AndAlso CInt(ap.Bitrate) Mod If(CInt(ap.Bitrate) > 256, 64, 32) <> 0 Then
                    If ProcessTip($"The AC3 bitrate {CInt(ap.Bitrate)} is not specification compliant.") Then
                        Return Warn("Invalid Audio Bitrate", GetAudioTextBox(ap))
                    End If
                End If

                If ap.File = p.TargetFile AndAlso ProcessTip("The audio source and target filepath is identical.") Then
                    Return Block("Invalid Targetpath", GetAudioTextBox(ap), tbTargetFile)
                End If

                If Math.Abs(ap.Delay) > 2000 AndAlso ProcessTip("The audio delay is unusual high indicating a sync problem.") Then
                    Return Warn("High Audio Delay", GetAudioTextBox(ap))
                End If

                If Not p.VideoEncoder.Muxer.IsSupported(ap.OutputFileType) AndAlso Not ap.OutputFileType = "ignore" Then
                    If ProcessTip("The audio format is '" + ap.OutputFileType + "' but the container '" +
                        p.VideoEncoder.Muxer.Name + "' supports only " +
                        p.VideoEncoder.Muxer.SupportedInputTypes.Join(", ") +
                        ". Select another audio profile or another container.") Then

                        Return Block("Audio format not compatible with container", GetAudioTextBox(ap), llMuxer)
                    End If
                End If
            Next

            If p.VideoEncoder.Muxer.OutputExtFull <> p.TargetFile.ExtFull Then
                If ProcessTip("The container requires " + p.VideoEncoder.Muxer.OutputExt.ToUpperInvariant + " as target file type.") Then
                    Return Block("Invalid File Type", tbTargetFile)
                End If
            End If

            If p.VideoEncoder.GetError IsNot Nothing Then
                If ProcessTip(p.VideoEncoder.GetError) Then
                    Return Block("Encoder Error")
                End If
            End If

            Dim ae = Calc.GetAspectRatioError()

            If Not isValidAnamorphicSize AndAlso (ae > p.MaxAspectRatioError OrElse ae < -p.MaxAspectRatioError) AndAlso
                isResized AndAlso p.WarnArError AndAlso p.CustomTargetPAR <> "1:1" Then

                If ProcessTip("Use the resize slider to correct the aspect ratio error or click Next to encode anamorphic.") Then
                    Return Warn("Aspect Ratio Error", lAspectRatioError)
                End If
            End If

            If p.RemindToSetFilters AndAlso ProcessTip("Verify the filter setup, when done continue with Next.") Then
                Return Warn("Filter Setup")
            End If

            If p.Ranges.Count = 0 Then
                If p.RemindToCut AndAlso TypeOf p.VideoEncoder IsNot NullEncoder AndAlso
                    ProcessTip("Click here to open the preview for cutting if necessary. When done continue with Next.") Then

                    Return Warn("Cutting", AddressOf ShowPreview)
                End If
            Else
                If p.CutFrameRate <> p.Script.Info.FrameRate Then
                    If ProcessTip("The frame rate was changed after cutting was performed, please ensure that this change is happening after the Cutting filter section in the AviSynth script.") Then
                        Return Warn("Frame Rate Change")
                    End If
                End If

                If Not p.Script.IsFilterActive("Cutting") AndAlso Form.ActiveForm Is Me Then
                    If ProcessTip("The cutting filter settings don't match with the cutting settings used in the preview." + BR +
                                  "This can usually be fixed by opening and closing the preview.") Then
                        Return Block("Invalid Cutting Settings")
                    End If
                End If
            End If

            If p.RemindToDoCompCheck AndAlso p.VideoEncoder.IsCompCheckEnabled AndAlso p.Compressibility = 0 Then
                If ProcessTip("Click here to start the compressibility check. The compressibility check helps to finds the ideal bitrate or image size.") Then
                    Return Warn("Compressibility Check", AddressOf p.VideoEncoder.RunCompCheck, laTarget2)
                End If
            End If

            If Not p.TargetFile.IsValidPath() Then
                If ProcessTip("The target file path is invalid." + BR + p.TargetFile) Then
                    Return Warn("Invalid Target File", tbTargetFile)
                End If
            End If

            If File.Exists(p.TargetFile) Then
                If FileTypes.VideoText.Contains(p.SourceFile.Ext) AndAlso p.SourceFile.ReadAllText.Contains(p.TargetFile) Then
                    If ProcessTip("Source and target name are identical, please select another target name.") Then
                        Return Block("Invalid Target File", tbTargetFile)
                    End If
                Else
                    If ProcessTip("The target file already exists." + BR + p.TargetFile) Then
                        Return Warn("Target File", tbTargetFile)
                    End If
                End If
            End If

            If TypeOf p.VideoEncoder.Muxer Is MkvMuxer AndAlso Not String.IsNullOrWhiteSpace(p.VideoEncoder.Muxer.TimestampsFile) Then
                Dim sfc = p.SourceScript.GetFrameCount()
                Dim tfc = p.Script.GetFrameCount()
                If sfc <> tfc Then
                    If ProcessTip("The duration changed and doesn't fit the original timestamps, which can cause unwanted results. You can delete/change the timestamps file selection under Container Options > Options.") Then
                        Return Warn("Changed Length", lSource1, lTarget1)
                    End If
                End If

                Dim sfr = p.SourceScript.GetFramerate()
                Dim tfr = p.Script.GetFramerate()
                If sfr <> tfr Then
                    If ProcessTip("The frame rate changed and could not fit the original timestamps anymore, which can cause unwanted results. You can delete/change the timestamps file selection under Container Options > Options.") Then
                        Return Warn("Changed Frame Rate", lSource1, lTarget1)
                    End If
                End If
            End If

            If p.Script.IsFilterActive("Crop") AndAlso p.VideoEncoder?.IsDolbyVisionSet AndAlso Not p.VideoEncoder?.IsOvercroppingAllowed Then
                If p.HdrDolbyVisionMetadataFile Is Nothing Then
                    If ProcessTip($"You have set a Dolby Vision metadata file, that is unknown to StaxRip, while cropping the video. Please make sure it is extracted with the source file.") Then
                        Return Block("Unknown RPU File", AddressOf p.VideoEncoder.ShowConfigDialog)
                    End If
                End If

                Dim side = ""
                Dim by = 0
                Dim c = p.HdrDolbyVisionMetadataFile.Crop
                Dim leftOvercropping = p.CropLeft - c.Left
                Dim topOvercropping = p.CropTop - c.Top
                Dim rightOvercropping = p.CropRight - c.Right
                Dim bottomOvercropping = p.CropBottom - c.Bottom

                If leftOvercropping > 0 Then
                    side = "left"
                    by = leftOvercropping
                ElseIf p.CropTop > c.Top Then
                    side = "top"
                    by = topOvercropping
                ElseIf p.CropRight > c.Right Then
                    side = "right"
                    by = rightOvercropping
                ElseIf p.CropBottom > c.Bottom Then
                    side = "bottom"
                    by = bottomOvercropping
                End If

                If by > 0 Then
                    If ProcessTip($"You have cropped the {side} side by {by}px too much.{BR}Decrease the crop to continue and ensure a valid result.") Then
                        Return Block("Overcropping", AddressOf ShowCropDialog)
                    End If
                End If
            End If

            If p.Script.IsFilterActive("Resize") AndAlso widthZoom <> heightZoom AndAlso p.VideoEncoder?.IsResizingAllowed AndAlso Not p.VideoEncoder?.IsUnequalResizingAllowed Then
                Dim arw = p.TargetWidth / cropw
                Dim h = croph * arw
                Dim res = CInt(If(CInt(Fix(h)) Mod 2 = 0, Math.Floor(h), Math.Floor(h + 1)))

                If p.TargetHeight <> res Then
                    If ProcessTip("Resizing of that kind will interfere with the Dolby Vision metadata. Keep the original aspect ratio, disable the 'Resize' filter or remove the Dolby Vision RPU file.") Then
                        Return Block("Wrong resizing", tbTargetWidth, tbTargetHeight, lSAR, lDAR, lZoom, lAspectRatioError)
                    End If
                End If
            End If

            If TypeOf p.VideoEncoder Is x265Enc Then
                Dim x265 = DirectCast(p.VideoEncoder, x265Enc)
                Dim rpuParam = x265.Params.DolbyVisionRpu
                Dim bufsizeParam = x265.Params.VbvBufSize
                Dim maxrateParam = x265.Params.VbvMaxRate
                Dim optionsLabel = DirectCast(pnEncoder.Controls(0), x265Control).blConfigCodec

                If Not String.IsNullOrWhiteSpace(rpuParam.Value) Then
                    If bufsizeParam?.Value < 1 Then
                        If ProcessTip("Dolby Vision requires VBV settings to enable HRD.") Then
                            Return Block("Missing VBV settings", Sub() x265.ShowConfigDialog(bufsizeParam), optionsLabel)
                        End If
                    ElseIf maxrateParam?.Value < 1 Then
                        If ProcessTip("Dolby Vision requires VBV settings to enable HRD.") Then
                            Return Block("Missing VBV settings", Sub() x265.ShowConfigDialog(maxrateParam), optionsLabel)
                        End If
                    End If
                End If
            End If

            If TypeOf p.VideoEncoder Is SvtAv1EssentialEnc Then
                If p.Script.Info.BitDepth <> 10 Then
                    If ProcessTip("This encoder requires 10-bit sources. Add a filter that converts it to 10-bit.") Then
                        Return Block("Invalid bit-depth")
                    End If
                End If
            End If

            If p.Script.IsAviSynth AndAlso TypeOf p.VideoEncoder Is x264Enc AndAlso
                Not Package.x264.Version.ToLowerInvariant.ContainsAny("amod", "djatom", "patman") AndAlso
                p.Script.Info.ColorSpace <> ColorSpace.YUV420P8 AndAlso
                p.Script.Info.ColorSpace <> ColorSpace.YUV420P8_ AndAlso
                p.Script.Info.ColorSpace <> ColorSpace.YUV422P8 AndAlso
                p.Script.Info.ColorSpace <> ColorSpace.YUV444P8 AndAlso
                p.Script.Info.ColorSpace <> ColorSpace.BGR32 AndAlso
                Not g.ContainsPipeTool(p.VideoEncoder.GetCommandLine(True, True)) Then

                If ProcessTip("x264 AviSynth input supports only YUV420P8, YUV422P8, YUV444P8 and BGR32 " +
                             $"as input colorspace.{BR}Consider to use a pipe tool: " +
                              "x264 Options > Input/Output > Pipe > avs2pipemod y4m") Then
                    Return Block("Incompatible colorspace")
                End If
            End If

            If p.Script.Info.Width Mod p.ForcedOutputMod <> 0 AndAlso (Not p.ForcedOutputModOnlyIfCropped OrElse p.Script.Info.Width <> p.SourceScript.Info.Width) Then
                Dim tip = "Change output width to be divisible by " & p.ForcedOutputMod & " or customize:" + BR + "Options > Image > Output Mod"
                If Not p.ForcedOutputModIgnorable Then RemoveTip(tip)
                If ProcessTip(tip) Then
                    CanIgnoreTip = p.ForcedOutputModIgnorable
                    Return Warn("Invalid Target Width", tbTargetWidth, lSAR)
                End If
            End If

            If p.Script.Info.Height Mod p.ForcedOutputMod <> 0 AndAlso (Not p.ForcedOutputModOnlyIfCropped OrElse p.Script.Info.Height <> p.SourceScript.Info.Height) Then
                Dim tip = "Change output height to be divisible by " & p.ForcedOutputMod & " or customize:" + BR + "Options > Image > Output Mod"
                If Not p.ForcedOutputModIgnorable Then RemoveTip(tip)
                If ProcessTip(tip) Then
                    CanIgnoreTip = p.ForcedOutputModIgnorable
                    Return Warn("Invalid Target Height", tbTargetHeight, lSAR)
                End If
            End If

            If p.VideoEncoder.IsCompCheckEnabled AndAlso p.Compressibility > 0 Then
                Dim value = Calc.GetPercent

                If value < (p.VideoEncoder.AutoCompCheckValue - 20) OrElse
                    value > (p.VideoEncoder.AutoCompCheckValue + 20) Then

                    If ProcessTip("Aimed quality value is more than 20% off, change the image or file size to get something between 50% and 70% quality.") Then
                        Return Warn("Quality", tbTargetSize, tbBitrate, tbTargetWidth, tbTargetHeight, laTarget2)
                    End If
                End If
            End If

            If TypeOf p.VideoEncoder.Muxer Is MP4Muxer Then
                For Each i In p.VideoEncoder.Muxer.Subtitles
                    If Not i.Path.Ext.EqualsAny("idx", "srt", "sub") Then
                        If ProcessTip("MP4 supports only SUB, SRT and IDX subtitles.") Then
                            Return Block("Invalid subtitle format")
                        End If
                    End If
                Next
            End If

            If refreshScript AndAlso Not (MouseButtons = MouseButtons.Left AndAlso ActiveControl Is tbResize) Then
                Dim err = p.Script.Error

                If err <> "" AndAlso ProcessTip(err) Then
                    Return Block("Click on the error message", Sub() MsgError("Script Error", err))
                End If
            End If
        Else
            If p.SourceFiles.Count = 0 Then
                If ProcessTip("Click here to open a source file.") Then
                    CanIgnoreTip = False
                    Return Warn("Assistant", AddressOf ShowOpenSourceDialog)
                End If
            End If
        End If

        'flicker issue
        If gbAssistant.Text <> "Add Job" Then
            gbAssistant.Text = "Add Job"
        End If

        If laTip.Font.Size <> 9 Then
            laTip.SetFontSize(9)
        End If

        laTip.Text = "Click on the button to the right to add a job to the job list."
        AssistantPassed = True
        bnNext.Enabled = True
        laTip.BackColor = ThemeManager.CurrentTheme.MainForm.laTipBackColor
        laTip.ForeColor = ThemeManager.CurrentTheme.MainForm.laTipForeColor
        UpdateNextButton()
    End Function
End Class
