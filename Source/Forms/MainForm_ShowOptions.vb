Imports StaxRip.UI

Partial Public Class MainForm
    Inherits FormBase

    <Command("Dialog to configure project options.")>
    Sub ShowOptionsDialog()
        ShowOptionsDialog(Nothing)
    End Sub

    Sub ShowOptionsDialog(pagePath As String)
        Using form As New SimpleSettingsForm(
            "Project Options",
            "In order to save project options go to:",
            "File > Save Project As Template",
            "In order to select a template to be loaded on program startup go to:",
            "Tools > Settings > General > Templates > Default Template")

            form.ScaleClientSize(38, 30)

            Dim ui = form.SimpleUI
            ui.Store = p


            '   ----------------------------------------------------------------
            ui.CreateFlowPage("Image", True)

            Dim n = ui.AddNum()
            n.Text = "Auto resize image size"
            n.Help = "Resizes to a given pixel size after loading a source file."
            n.Config = {0, Integer.MaxValue, 10000}
            n.Field = NameOf(p.AutoResizeImage)

            ui.AddLabel(n, "(0 = disabled)")

            Dim m = ui.AddMenu(Of Integer)()
            m.Text = "Output Mod"
            m.Add(2, 4, 8, 16)
            m.Field = NameOf(p.ForcedOutputMod)

            Dim dirMenu = ui.AddMenu(Of ForceOutputModDirection)()
            dirMenu.Text = "Output Mod Direction"
            dirMenu.Field = NameOf(p.ForcedOutputModDirection)

            Dim b = ui.AddBool()
            b.Text = "Make Output Mod warning ignorable"
            b.Field = NameOf(p.ForcedOutputModIgnorable)

            b = ui.AddBool()
            b.Text = "Warn on invalid Output Mod only if video is cropped"
            b.Field = NameOf(p.ForcedOutputModOnlyIfCropped)


            '   ----------------------------------------------------------------
            ui.CreateFlowPage("Image | Aspect Ratio", True)

            b = ui.AddBool()
            b.Text = "Use ITU-R BT.601 compliant aspect ratio"
            b.Help = "Calculates the aspect ratio according to ITU-R BT.601 standard."
            b.Field = NameOf(p.ITU)

            b = ui.AddBool()
            b.Text = "Adjust height according to target display aspect ratio"
            b.Help = "Adjusts the height to match the target display aspect ratio in case the auto resize option is disabled."
            b.Field = NameOf(p.AdjustHeight)

            n = ui.AddNum()
            n.Text = "Max AR Error"
            n.Help = "Maximum aspect ratio error. In case of a higher value the AR signaled to the encoder or muxer."
            n.Config = {1, 10, 0.1, 1}
            n.Field = NameOf(p.MaxAspectRatioError)

            Dim t = ui.AddText()
            t.Text = "Source DAR"
            t.Help = "Custom source display aspect ratio."
            t.Field = NameOf(p.CustomSourceDAR)

            t = ui.AddText()
            t.Text = "Source PAR"
            t.Help = "Custom source pixel aspect ratio."
            t.Field = NameOf(p.CustomSourcePAR)

            t = ui.AddText()
            t.Text = "Target DAR"
            t.Help = "Custom target display aspect ratio."
            t.Field = NameOf(p.CustomTargetDAR)

            t = ui.AddText()
            t.Text = "Target PAR"
            t.Help = "Custom target pixel aspect ratio."
            t.Field = NameOf(p.CustomTargetPAR)


            '   ----------------------------------------------------------------
            Dim cropPage = ui.CreateFlowPage("Image | Crop", True)

            b = ui.AddBool()
            b.Text = "Auto correct crop values"
            b.Help = "Force crop values compatible with YUV/YV12 colorspace and with the forced output mod value. "
            b.Field = NameOf(p.AutoCorrectCropValues)

            b = ui.AddBool()
            b.Text = "Auto crop borders until proper aspect ratio is found"
            b.Help = "Automatically crops borders until the proper aspect ratio is found."
            b.Field = NameOf(p.AutoSmartCrop)

            n = ui.AddNum()
            n.Text = "Auto overcrop width to limit aspect ratio to"
            n.Help = "On small devices it can help to restrict the aspect ratio and overcrop the width instead."
            n.Config = {0, 2, 0.1, 3}
            n.Field = NameOf(p.AutoSmartOvercrop)

            b = ui.AddBool()
            b.Text = "Tonemapping for HDR videos"
            b.Help = "Tonemap sources with a higher Bit Depth than 8bit."
            b.Enabled = Vulkan.IsSupported
            b.Field = NameOf(p.CropWithTonemapping)
            b.Checked = b.Checked AndAlso Vulkan.IsSupported

            b = ui.AddBool()
            b.Text = "High contrast for easier cropping"
            b.Help = ""
            b.Field = NameOf(p.CropWithHighContrast)

            ui.AddLine(cropPage, "Custom crop values")

            Dim eb = ui.AddEmptyBlock(cropPage)
            ui.AddLabel(eb, "Left:", 2)
            Dim leftCrop = ui.AddNumeric(eb)
            ui.AddLabel(eb, "Right:", 4)
            Dim rightCrop = ui.AddNumeric(eb)

            eb = ui.AddEmptyBlock(cropPage)
            ui.AddLabel(eb, "Top:", 2)
            Dim topCrop = ui.AddNumeric(eb)
            ui.AddLabel(eb, "Bottom:", 4)
            Dim bottomCrop = ui.AddNumeric(eb)

            leftCrop.Value = p.CropLeft
            leftCrop.Config = {0, 9999, 2, 0}

            rightCrop.Value = p.CropRight
            rightCrop.Config = leftCrop.Config

            topCrop.Value = p.CropTop
            topCrop.Config = {0, 9999, 2, 0}

            bottomCrop.Value = p.CropBottom
            bottomCrop.Config = topCrop.Config



            '   ----------------------------------------------------------------
            Dim autoCropPage = ui.CreateFlowPage("Image | Crop | Auto Crop", True)

            Dim autoCropMode = ui.AddMenu(Of AutoCropMode)

            ui.AddLine(autoCropPage, "General")
            'dim l = ui.AddLabel("Regular AutoCrop settings:", 0, FontStyle.Bold)
            'l.Margin = New Padding(0, 10, 0, 0)
            Dim autoCropSideMode = ui.AddMenu(Of AutoCropSideMode)
            Dim autoCropFrameRangeMode = ui.AddMenu(Of AutoCropFrameRangeMode)

            Dim thresholdEb = ui.AddEmptyBlock(autoCropPage)
            thresholdEb.Margin = New Padding(0, 6, 0, 3)
            Dim l = ui.AddLabel(thresholdEb, "Threshold at:", 7)
            ui.AddLabel(thresholdEb, "Beginning:", 2)
            Dim thresholdBegin = ui.AddNumeric(thresholdEb)
            ui.AddLabel(thresholdEb, " ", 1)
            ui.AddLabel(thresholdEb, "Ending:", 2)
            Dim thresholdEnd = ui.AddNumeric(thresholdEb)

            thresholdBegin.Help = "Number of frames at the beginning of the video, that are ignored when setting the crop values."
            thresholdBegin.Config = {0, 999999, 25, 0}
            thresholdBegin.Field = NameOf(p.AutoCropFrameRangeThresholdBegin)

            thresholdEnd.Help = "Number of frames at the ending of the video, that are ignored when setting the crop values."
            thresholdEnd.Config = thresholdBegin.Config
            thresholdEnd.Field = NameOf(p.AutoCropFrameRangeThresholdEnd)

            autoCropSideMode.Text = "Side Selection Mode"
            autoCropSideMode.Help = "Decide which sides shall be cropped."
            autoCropSideMode.Expanded = True
            autoCropSideMode.Field = NameOf(p.AutoCropSideMode)

            autoCropFrameRangeMode.Text = "Frame Range Mode"
            autoCropFrameRangeMode.Help = "Defines the range frames are considered to be taken into account for processing:" + BR2 +
                                                        "Automatic: Depending on video length a small portion of the beginning and end will be ignored." + BR +
                                                        "Complete: The whole video length will be used." + BR +
                                                        "Manual Threshold: Define your own number of frames at beginning and end that will be ignored."
            autoCropFrameRangeMode.Expanded = True
            autoCropFrameRangeMode.Field = NameOf(p.AutoCropFrameRangeMode)
            autoCropFrameRangeMode.Button.ValueChangedAction = Sub(value)
                                                                   Dim active = autoCropFrameRangeMode.Enabled
                                                                   Dim activeAutomaticRange = active AndAlso value = StaxRip.AutoCropFrameRangeMode.Automatic
                                                                   Dim activeCompleteRange = active AndAlso value = StaxRip.AutoCropFrameRangeMode.Complete
                                                                   Dim activeManualThresholdRange = active AndAlso value = StaxRip.AutoCropFrameRangeMode.ManualThreshold
                                                                   thresholdEb.Visible = activeManualThresholdRange
                                                               End Sub
            autoCropFrameRangeMode.Button.ValueChangedAction.Invoke(p.AutoCropFrameRangeMode)



            Dim autoCropFrameSelectionMode = ui.AddMenu(Of AutoCropFrameSelectionMode)

            Dim fsFixedFrames = ui.AddNum()
            fsFixedFrames.Text = "Number of frames:"
            fsFixedFrames.Help = "Fixed number of frames being analyzed over the whole video."
            fsFixedFrames.Config = {1, 7200, 5, 0}
            fsFixedFrames.Field = NameOf(p.AutoCropFixedFramesFrameSelection)
            fsFixedFrames.Margin = New Padding(0, 6, 0, 3)

            Dim fsFrameInterval = ui.AddNum()
            fsFrameInterval.Text = "Frame interval:"
            fsFrameInterval.Help = "Frame interval betwen analyzed frames."
            fsFrameInterval.Config = {1, 3600, 5, 0}
            fsFrameInterval.Field = NameOf(p.AutoCropFrameIntervalFrameSelection)
            fsFrameInterval.Margin = New Padding(0, 6, 0, 3)

            Dim fsTimeInterval = ui.AddNum()
            fsTimeInterval.Text = "Time interval in seconds:"
            fsTimeInterval.Help = "Time interval in seconds betwen analyzed frames."
            fsTimeInterval.Config = {1, 3600, 5, 0}
            fsTimeInterval.Field = NameOf(p.AutoCropTimeIntervalFrameSelection)
            fsTimeInterval.Margin = New Padding(0, 6, 0, 3)


            Dim luminanceThreshold = ui.AddNum()
            luminanceThreshold.Text = "Luminance Threshold in percent:"
            luminanceThreshold.Help = "Max brightness in peercent of those lines, that are considered to be cropped."
            luminanceThreshold.Config = {0, 99, 0.1, 1}
            luminanceThreshold.Field = NameOf(p.AutoCropLuminanceThreshold)
            luminanceThreshold.Margin = New Padding(0, 6, 0, 3)



            ui.AddLine(autoCropPage, "Dolby Vision")

            Dim autoCropDVMode = ui.AddMenu(Of AutoCropDolbyVisionMode)()

            Dim doviThresholdEb = ui.AddEmptyBlock(autoCropPage)
            doviThresholdEb.Margin = New Padding(0, 6, 0, 3)
            l = ui.AddLabel(doviThresholdEb, "Threshold at:", 7)
            ui.AddLabel(doviThresholdEb, "Beginning:", 2)
            Dim doviThresholdBegin = ui.AddNumeric(doviThresholdEb)
            ui.AddLabel(doviThresholdEb, " ", 1)
            ui.AddLabel(doviThresholdEb, "Ending:", 2)
            Dim doviThresholdEnd = ui.AddNumeric(doviThresholdEb)

            Dim autoCropDVSideMode = ui.AddMenu(Of AutoCropDolbyVisionSideMode)()


            autoCropDVMode.Text = "Threshold Mode"
            autoCropDVMode.Help = "Decide between an automatic mode and a manual threshold to ignore a number of frames at the beginning and/or end."
            autoCropDVMode.Expanded = True
            autoCropDVMode.Field = NameOf(p.AutoCropDolbyVisionMode)
            autoCropDVMode.Button.ValueChangedAction = Sub(value)
                                                           Dim active = value = AutoCropDolbyVisionMode.ManualThreshold
                                                           doviThresholdEb.Visible = active
                                                       End Sub
            autoCropDVMode.Button.ValueChangedAction.Invoke(p.AutoCropDolbyVisionMode)

            autoCropDVSideMode.Text = "Side Selection Mode"
            autoCropDVSideMode.Help = "Decide which sides shall be cropped."
            autoCropDVSideMode.Expanded = True
            autoCropDVSideMode.Field = NameOf(p.AutoCropDolbyVisionSideMode)

            autoCropFrameSelectionMode.Text = "Frame Selection Mode"
            autoCropFrameSelectionMode.Help = ""
            autoCropFrameSelectionMode.Expanded = True
            autoCropFrameSelectionMode.Field = NameOf(p.AutoCropFrameSelectionMode)
            autoCropFrameSelectionMode.Button.ValueChangedAction = Sub(value)
                                                                       Dim active = autoCropFrameSelectionMode.Enabled
                                                                       Dim activeFixedFrames = active AndAlso value = StaxRip.AutoCropFrameSelectionMode.FixedFrames
                                                                       Dim activeFrameInterval = active AndAlso value = StaxRip.AutoCropFrameSelectionMode.FrameInterval
                                                                       Dim activeTimeInterval = active AndAlso value = StaxRip.AutoCropFrameSelectionMode.TimeInterval
                                                                       fsFixedFrames.Visible = activeFixedFrames
                                                                       fsFrameInterval.Visible = activeFrameInterval
                                                                       fsTimeInterval.Visible = activeTimeInterval
                                                                   End Sub
            autoCropFrameSelectionMode.Button.ValueChangedAction.Invoke(p.AutoCropFrameSelectionMode)

            autoCropMode.Text = "Auto Crop after opening"
            autoCropMode.Help = "Use Auto Crop when a file is opened to crop it directly."
            autoCropMode.Expanded = True
            autoCropMode.Field = NameOf(p.AutoCropMode)
            autoCropMode.Button.ValueChangedAction = Sub(value)
                                                         autoCropDVMode.Button.ValueChangedAction.Invoke(autoCropDVMode.Button.Value)
                                                         autoCropFrameSelectionMode.Button.ValueChangedAction.Invoke(autoCropFrameSelectionMode.Button.Value)
                                                     End Sub
            autoCropMode.Button.ValueChangedAction.Invoke(p.AutoCropMode)

            doviThresholdBegin.Help = "Number of frames at the beginning of the video, that are ignored when setting the crop values."
            doviThresholdBegin.Config = {0, 999999, 25, 0}
            doviThresholdBegin.Field = NameOf(p.AutoCropDolbyVisionThresholdBegin)

            doviThresholdEnd.Help = "Number of frames at the ending of the video, that are ignored when setting the crop values."
            doviThresholdEnd.Config = doviThresholdBegin.Config
            doviThresholdEnd.Field = NameOf(p.AutoCropDolbyVisionThresholdEnd)


            '   ----------------------------------------------------------------
            Dim generalPage = ui.CreateFlowPage("General", True)

            Dim takeOverTitle = ui.AddBool()

            takeOverTitle.Text = "Take over title"
            takeOverTitle.Help = "Take over title from source file - Title Name in container options must be empty!"
            takeOverTitle.Checked = p.TakeOverTitle
            takeOverTitle.SaveAction = Sub(value) p.TakeOverTitle = value


            '   ----------------------------------------------------------------
            Dim videoPage = ui.CreateFlowPage("Video", True)

            Dim videoExist = ui.AddMenu(Of FileExistMode)
            Dim demuxVideo = ui.AddBool()
            Dim fixFramerate = ui.AddBool()
            Dim takeOverVideoLanguage = ui.AddBool()
            Dim extractHdrmetadata = ui.AddMenu(Of HdrmetadataMode)

            videoExist.Text = "Existing Video Output"
            videoExist.Help = "What to do in case the video encoding output file already exists from a previous job run, skip and reuse or re-encode and overwrite. The 'Copy/Mux' video encoder profile is also capable of reusing existing video encoder output.'"
            videoExist.Expanded = True
            videoExist.Field = NameOf(p.FileExistVideo)

            demuxVideo.Text = "Demux Video"
            demuxVideo.Field = NameOf(p.DemuxVideo)

            takeOverVideoLanguage.Text = "Take over language"
            takeOverVideoLanguage.Field = NameOf(p.TakeOverVideoLanguage)

            fixFramerate.Text = "Add filter to automatically correct the frame rate."
            fixFramerate.Field = NameOf(p.FixFrameRate)

            extractHdrmetadata.Text = "Extract HDR metadata"
            extractHdrmetadata.Help = "Extract dynamic HDR10+ and DolbyVision metadata if available"
            extractHdrmetadata.Expanded = True
            extractHdrmetadata.Field = NameOf(p.ExtractHdrmetadata)
            extractHdrmetadata.Button.ValueChangedAction = Sub(value)
                                                               Dim visible = value = HdrmetadataMode.All OrElse value = HdrmetadataMode.DolbyVision
                                                           End Sub

            b = ui.AddBool
            b.Text = "Import VUI metadata"
            b.Help = "Imports VUI metadata such as HDR from the source file to the video encoder."
            b.Field = NameOf(p.ImportVUIMetadata)

            b = ui.AddBool
            b.Text = "Add filter to convert chroma subsampling to 4:2:0"
            b.Help = "After a source is loaded, automatically add a filter to convert chroma subsampling to 4:2:0"
            b.Field = NameOf(p.ConvertChromaSubsampling)

            Dim cb = ui.AddMenu(Of ConvertTo420BitDepth)
            cb.Text = "Add filter to convert bit depth to"
            cb.Help = "After a source is loaded, automatically add a filter to convert bit-depth to x-bit"
            cb.Expanded = True
            cb.Field = NameOf(p.ConvertToBits)

            b = ui.AddBool
            b.Text = "Auto-rotate video after loading when possible"
            b.Help = "Auto-rotate video after loading when the source file/container supports it."
            b.Field = NameOf(p.AutoRotation)

            ui.AddLine()

            n = ui.AddNum()
            n.Text = "Min. duration for BDMV playlists:"
            n.Help = "Duration in seconds. A value of '-1' will bypass the custom value and take eac3to's default value instead."
            n.Config = {-1, 3600, 1, 0}
            n.Field = NameOf(p.MinBdmvPlaylistDuration)

            n = ui.AddNum()
            n.Text = "Film Threshold for D2V files:"
            n.Help = ""
            n.Config = {0, 100, 0.1, 2}
            n.Field = NameOf(p.D2VAutoForceFilmThreshold)


            '   ----------------------------------------------------------------
            Dim audioPage = ui.CreateFlowPage("Audio", True)

            Dim prefAudio = ui.AddTextMenu
            prefAudio.Text = "Preferred Languages"
            prefAudio.Help = "List of audio tracks to demux."
            prefAudio.Field = NameOf(p.PreferredAudio)

            For x = 1 To 9
                Dim temp = x
                prefAudio.AddMenu("Choose ID | " & x, Sub() prefAudio.Edit.Text += " " & temp)
            Next

            prefAudio.AddMenu("Choose All", "all")
            prefAudio.AddMenu("-", "")

            For Each lng In Language.Languages
                If lng.IsCommon Then
                    prefAudio.AddMenu(lng.ToString + " (" + lng.TwoLetterCode + ", " + lng.ThreeLetterCode + ")",
                        Sub() prefAudio.Edit.Text += " " + lng.ThreeLetterCode)
                End If
            Next

            Dim moreAudio = prefAudio.AddMenu("More", DirectCast(Nothing, Action))
            Dim moreAudioFirst = prefAudio.AddMenu("More | temp", DirectCast(Nothing, Action))

            Dim moreAudioAction = Sub()
                                      For Each lng In Language.Languages
                                          prefAudio.AddMenu("More | " + lng.ToString.Substring(0, 1).ToUpperInvariant + " | " + lng.ToString +
                                         " (" + lng.TwoLetterCode + ", " + lng.ThreeLetterCode + ")",
                                         Sub() prefAudio.Edit.Text += " " + lng.ThreeLetterCode)
                                      Next
                                  End Sub

            AddHandler moreAudio.DropDownOpened, Sub()
                                                     If moreAudio.DropDownItems.Count > 1 Then
                                                         Exit Sub
                                                     End If

                                                     moreAudioFirst.Visible = False
                                                     moreAudioAction()
                                                 End Sub

            Dim cut = ui.AddMenu(Of CuttingMode)
            cut.Text = "Cutting Method"
            cut.Help = "Defines which method to use for cutting."
            cut.Field = NameOf(p.CuttingMode)

            Dim demuxAudio = ui.AddMenu(Of DemuxMode)
            demuxAudio.Text = "Demux Audio"
            demuxAudio.Field = NameOf(p.DemuxAudio)

            Dim audioExist = ui.AddMenu(Of FileExistMode)
            audioExist.Text = "Existing Output"
            audioExist.Help = "What to do in case an audio encoding output file already exists from a previous job run, skip and reuse or re-encode and overwrite."
            audioExist.Field = NameOf(p.FileExistAudio)

            Dim audioIntermediateWavBitDepth = ui.AddMenu(Of IntermediateWaveBitDepth)
            audioIntermediateWavBitDepth.Text = "Intermediate Wave Bit Depth"
            audioIntermediateWavBitDepth.Help = "In case you choose WAV as intermediate format for audio encoding, you can choose the bit depth. The higher the better, but also needs more disk space and probably time."
            audioIntermediateWavBitDepth.Field = NameOf(p.AudioIntermediateWaveBitDepth)

            n = ui.AddNum
            n.Text = "Audio Tracks"
            n.Config = {2, 20, 1}
            n.Field = NameOf(p.AudioTracksAvailable)
            'n.NumEdit.SaveAction = Sub(x) SetAudioTracks(CType(x, Integer))

            b = ui.AddBool
            b.Text = "On load use AviSynth script as audio source"
            b.Help = "Sets the AviSynth script (*.avs) as audio source file when loading a source file."
            b.Field = NameOf(p.UseScriptAsAudioSource)


            '   ----------------------------------------------------------------
            Dim subPage = ui.CreateFlowPage("Subtitles", True)

            Dim subMode = ui.AddMenu(Of SubtitleMode)
            subMode.Expanded = True
            subMode.Text = "Subtitles"
            subMode.Field = NameOf(p.SubtitleMode)

            Dim prefSub = ui.AddTextMenu(subPage)
            prefSub.Text = "Languages"
            prefSub.Help = "List of used subtitle languages."
            prefSub.Field = NameOf(p.PreferredSubtitles)

            For x = 1 To 9
                Dim temp = x
                prefSub.AddMenu("Choose ID | " & x, Sub() prefSub.Edit.Text += " " & temp)
            Next

            prefSub.AddMenu("Choose All", "all")
            prefSub.AddMenu("-", "")

            For Each lng In Language.Languages
                If lng.IsCommon Then
                    prefSub.AddMenu(lng.ToString + " (" + lng.TwoLetterCode + ", " + lng.ThreeLetterCode + ")",
                        Sub() prefSub.Edit.Text += " " + lng.ThreeLetterCode)
                End If
            Next

            Dim moreSub = prefSub.AddMenu("More", DirectCast(Nothing, Action))
            Dim moreSubFirst = prefSub.AddMenu("More | temp", DirectCast(Nothing, Action))

            Dim moreSubAction = Sub()
                                    For Each lng In Language.Languages
                                        prefSub.AddMenu("More | " + lng.ToString.Substring(0, 1).ToUpperInvariant + " | " + lng.ToString +
                                         " (" + lng.TwoLetterCode + ", " + lng.ThreeLetterCode + ")",
                                         Sub() prefSub.Edit.Text += " " + lng.ThreeLetterCode)
                                    Next
                                End Sub

            AddHandler moreSub.DropDownOpened, Sub()
                                                   If moreSub.DropDownItems.Count > 1 Then
                                                       Exit Sub
                                                   End If

                                                   moreSubFirst.Visible = False
                                                   moreSubAction()
                                               End Sub

            Dim tbm = ui.AddTextMenu(subPage)
            tbm.Text = "Track Name"
            tbm.Help = "Track name used for muxing, may contain macros."
            tbm.Field = NameOf(p.SubtitleName)
            tbm.AddMenu("Language English", "%language_english%")
            tbm.AddMenu("Language Native", "%language_native%")

            Dim mb = ui.AddMenu(Of DefaultSubtitleMode)(subPage)
            mb.Text = "Default Subtitle"
            mb.Field = NameOf(p.DefaultSubtitle)

            b = ui.AddBool(subPage)
            b.Text = "Convert Sup (PGS/Blu-ray) to IDX (Sub/VobSub/DVD)"
            b.Help = "Works only with demuxed subtitles."
            b.Field = NameOf(p.ConvertSup2Sub)

            b = ui.AddBool(subPage)
            b.Text = "Extract forced subtitles from IDX files (Sub/VobSub/DVD)"
            b.Help = "Works only with demuxed subtitles."
            b.Field = NameOf(p.ExtractForcedSubSubtitles)

            b = ui.AddBool(subPage)
            b.Text = "Add hardcoded subtitle"
            b.Help = "Automatically hardcodes a subtitle." + BR2 + "Supported formats are SRT, ASS and VobSub."
            b.Field = NameOf(p.HardcodedSubtitle)


            '   ----------------------------------------------------------------
            Dim chaptersPage = ui.CreateFlowPage("Chapters", True)

            b = ui.AddBool(chaptersPage)
            b.Text = "Demux Chapters"
            b.Checked = p.DemuxChapters
            b.SaveAction = Sub(val) p.DemuxChapters = val


            '   ----------------------------------------------------------------
            Dim timestampsPage = ui.CreateFlowPage("Timestamps", True)

            Dim timestamps = ui.AddMenu(Of TimestampsMode)
            timestamps.Expanded = False
            timestamps.Text = "Extract timestamps from MKV files (if existing)"
            timestamps.Field = NameOf(p.ExtractTimestamps)


            '   ----------------------------------------------------------------
            Dim attachmentsPage = ui.CreateFlowPage("Attachments", True)

            b = ui.AddBool(attachmentsPage)
            b.Text = "Demux Attachments"
            b.Checked = p.DemuxAttachments
            b.SaveAction = Sub(val) p.DemuxAttachments = val

            b = ui.AddBool(attachmentsPage)
            b.Text = "Add Attachments to Muxing"
            b.Checked = p.AddAttachmentsToMuxer
            b.SaveAction = Sub(val) p.AddAttachmentsToMuxer = val


            '   ----------------------------------------------------------------
            Dim tagsPage = ui.CreateFlowPage("Tags", True)

            b = ui.AddBool(tagsPage)
            b.Text = "Demux Tags"
            b.Checked = p.DemuxTags
            b.SaveAction = Sub(val) p.DemuxTags = val

            b = ui.AddBool(tagsPage)
            b.Text = "Add Tags to Muxing"
            b.Checked = p.AddTagsToMuxer
            b.SaveAction = Sub(val) p.AddTagsToMuxer = val


            '   ----------------------------------------------------------------
            ui.CreateFlowPage("Thumbnails", True)

            b = ui.AddBool()
            b.Text = "Create a thumbnail sheet automatically after encoding"
            b.Field = NameOf(p.Thumbnailer)

            Dim thumbsImageFormat = ui.AddMenu(Of String)
            Dim thumbsQuality = ui.AddNum()

            thumbsImageFormat.Text = "Image Format"
            thumbsImageFormat.Expanded = True
            thumbsImageFormat.Add("BITMAP", "bmp")
            thumbsImageFormat.Add("GIF", "gif")
            thumbsImageFormat.Add("JPEG", "jpg")
            thumbsImageFormat.Add("PNG", "png")
            thumbsImageFormat.Add("TIFF", "tif")
            thumbsImageFormat.Button.Value = p.ThumbnailerSettings.GetString("ImageFileFormat", "jpg")
            thumbsImageFormat.Button.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("ImageFileFormat", value)
            thumbsImageFormat.Button.ValueChangedAction = Sub(value)
                                                              thumbsQuality.Visible = value = "jpg"
                                                          End Sub

            thumbsQuality.Text = "Quality (%):"
            thumbsQuality.Config = {1, 100, 1}
            thumbsQuality.NumEdit.Value = p.ThumbnailerSettings.GetInt("ImageQuality", 70)
            thumbsQuality.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("ImageQuality", CInt(value))

            Dim thumbsHeaderBackColor As SimpleUI.ColorPickerBlock = Nothing

            Dim thumbsImageBackColor = ui.AddColorPicker()
            thumbsImageBackColor.Text = "Background Color:"
            thumbsImageBackColor.Expanded = True
            thumbsImageBackColor.Color = p.ThumbnailerSettings.GetString("ImageBackColor", New ColorHSL(0, 0, 0.05, 1).ToHTML()).ToColor()
            thumbsImageBackColor.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("ImageBackColor", ColorTranslator.ToHtml(value))
            thumbsImageBackColor.ValueChangedAction = Sub(value) If thumbsHeaderBackColor IsNot Nothing Then thumbsHeaderBackColor.Color = value

            n = ui.AddNum()
            n.Text = "Spacer (%):"
            n.Config = {0, 1000, 1}
            n.NumEdit.Value = p.ThumbnailerSettings.GetInt("SpacerPercent", 20)
            n.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("SpacerPercent", CInt(value))

            ui.AddLabel(n, " between thumbs, header and sides")


            '   ----------------------------------------------------------------
            ui.CreateFlowPage("Thumbnails | Header", True)

            b = ui.AddBool()
            b.Text = "Draw Header"
            b.Checked = p.ThumbnailerSettings.GetBool("Header", True)
            b.SaveAction = Sub(value) p.ThumbnailerSettings.SetBool("Header", value)

            Dim headerFont = ui.AddTextButton()
            headerFont.Text = "Font Name:"
            headerFont.Expanded = True
            headerFont.Edit.Text = p.ThumbnailerSettings.GetString("HeaderFontName", FontManager.GetThumbnailFont().Name)
            headerFont.Edit.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("HeaderFontName", value)
            headerFont.ClickAction = Sub()
                                         Using td As New TaskDialog(Of FontFamily)
                                             td.Title = "Choose a font for the header"
                                             td.Symbol = Symbol.Font

                                             For Each ff In FontManager.GetFontFamilies(FontCategory.Thumbnail, True)
                                                 td.AddCommand(ff.Name, ff)
                                             Next

                                             If td.Show IsNot Nothing Then
                                                 headerFont.Edit.Text = td.SelectedText
                                             End If
                                         End Using
                                     End Sub

            Dim cp = ui.AddColorPicker()
            cp.Text = "Font Color:"
            cp.Expanded = True
            cp.Color = p.ThumbnailerSettings.GetString("HeaderFontColor", New ColorHSL(0, 0, 0.8, 1).ToHTML()).ToColor()
            cp.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("HeaderFontColor", ColorTranslator.ToHtml(value))

            n = ui.AddNum()
            n.Text = "Font Size (%):"
            n.Config = {10, 1000, 1}
            n.NumEdit.Value = p.ThumbnailerSettings.GetInt("HeaderFontSizePercent", 100)
            n.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("HeaderFontSizePercent", CInt(value))

            thumbsHeaderBackColor = ui.AddColorPicker()
            thumbsHeaderBackColor.Text = "Background Color:"
            thumbsHeaderBackColor.Expanded = True
            thumbsHeaderBackColor.Color = p.ThumbnailerSettings.GetString("HeaderBackColor", ColorTranslator.ToHtml(thumbsImageBackColor.Color)).ToColor()
            thumbsHeaderBackColor.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("HeaderBackColor", ColorTranslator.ToHtml(value))

            n = ui.AddNum()
            n.Text = "Separator Size (%):"
            n.Config = {0, 1000, 2}
            n.NumEdit.Value = p.ThumbnailerSettings.GetInt("HeaderSeparatorHeightPercent", 0)
            n.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("HeaderSeparatorHeightPercent", CInt(value))


            '   ----------------------------------------------------------------
            ui.CreateFlowPage("Thumbnails | Thumbs", True)

            Dim thumbstimestampAlign = ui.AddMenu(Of ContentAlignment)
            thumbstimestampAlign.Text = "Timestamp Alignment:"
            thumbstimestampAlign.Expanded = True
            thumbstimestampAlign.Button.Value = DirectCast([Enum].Parse(GetType(ContentAlignment), p.ThumbnailerSettings.GetString("TimestampAlignment", ContentAlignment.BottomLeft.ToString()).ToString()), ContentAlignment)
            thumbstimestampAlign.Button.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("TimestampAlignment", value.ToString())

            Dim timestampFont = ui.AddTextButton()
            timestampFont.Text = "Timestamp Font Name:"
            timestampFont.Expanded = True
            timestampFont.Edit.Text = p.ThumbnailerSettings.GetString("TimestampFontName", FontManager.GetThumbnailFont().Name)
            timestampFont.Edit.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("TimestampFontName", value)
            timestampFont.ClickAction = Sub()
                                            Using td As New TaskDialog(Of FontFamily)
                                                td.Title = "Choose a font for the timestamps"
                                                td.Symbol = Symbol.Font

                                                For Each ff In FontManager.GetFontFamilies(FontCategory.Thumbnail, True)
                                                    td.AddCommand(ff.Name, ff)
                                                Next

                                                If td.Show IsNot Nothing Then
                                                    timestampFont.Edit.Text = td.SelectedText
                                                End If
                                            End Using
                                        End Sub

            cp = ui.AddColorPicker()
            cp.Text = "Timestamp Font Color:"
            cp.Expanded = True
            cp.Color = p.ThumbnailerSettings.GetString("TimestampFontColor", New ColorHSL(0, 0, 0.9, 1).ToHTML()).ToColor()
            cp.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("TimestampFontColor", ColorTranslator.ToHtml(value))

            n = ui.AddNum()
            n.Text = "Timestamp Font Size (%):"
            n.Config = {10, 1000, 1}
            n.NumEdit.Value = p.ThumbnailerSettings.GetInt("TimestampFontSizePercent", 100)
            n.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("TimestampFontSizePercent", CInt(value))

            cp = ui.AddColorPicker()
            cp.Text = "Timestamp Outline Color:"
            cp.Expanded = True
            cp.Color = p.ThumbnailerSettings.GetString("TimestampOutlineColor", New ColorHSL(0, 0, 0.1, 1).ToHTML()).ToColor()
            cp.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("TimestampOutlineColor", ColorTranslator.ToHtml(value))

            n = ui.AddNum()
            n.Text = "Timestamp Outline Strength (%):"
            n.Config = {0, 1000, 2}
            n.NumEdit.Value = p.ThumbnailerSettings.GetInt("TimestampOutlineStrengthPercent", 100)
            n.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("TimestampOutlineStrengthPercent", CInt(value))


            Dim thumbsMode = ui.AddMenu(Of ThumbnailerRowMode)
            Dim thumbsColumns = ui.AddNum()
            Dim thumbsRows = ui.AddNum()
            Dim thumbsInterval = ui.AddNum()

            thumbsMode.Text = "Mode to set number of rows:"
            thumbsMode.Expanded = True
            thumbsMode.Button.Value = DirectCast([Enum].Parse(GetType(ThumbnailerRowMode), p.ThumbnailerSettings.GetInt("RowMode", ThumbnailerRowMode.Fixed).ToString()), ThumbnailerRowMode)
            thumbsMode.Button.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("RowMode", value)
            thumbsMode.Button.ValueChangedAction = Sub(value)
                                                       thumbsRows.Visible = value = ThumbnailerRowMode.Fixed
                                                       thumbsInterval.Visible = value = ThumbnailerRowMode.TimeInterval
                                                   End Sub

            thumbsColumns.Text = "Columns:"
            thumbsColumns.Config = {1, 50, 1}
            thumbsColumns.NumEdit.Value = p.ThumbnailerSettings.GetInt("Columns", 4)
            thumbsColumns.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("Columns", CInt(value))

            thumbsRows.Text = "Rows:"
            thumbsRows.Visible = thumbsMode.Button.Value = ThumbnailerRowMode.Fixed
            thumbsRows.Config = {1, 80, 1}
            thumbsRows.NumEdit.Value = p.ThumbnailerSettings.GetInt("Rows", 6)
            thumbsRows.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("Rows", CInt(value))

            thumbsInterval.Text = "Interval (s):"
            thumbsInterval.Visible = thumbsMode.Button.Value = ThumbnailerRowMode.TimeInterval
            thumbsInterval.Config = {1, 1800, 1}
            thumbsInterval.NumEdit.Value = p.ThumbnailerSettings.GetInt("Interval", 60)
            thumbsInterval.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("Interval", CInt(value))

            n = ui.AddNum()
            n.Text = "Width of each thumb (px):"
            n.Config = {0, 1920, 10}
            n.NumEdit.Value = p.ThumbnailerSettings.GetInt("ThumbWidth", 600)
            n.NumEdit.SaveAction = Sub(value) p.ThumbnailerSettings.SetInt("ThumbWidth", CInt(value))



            '   ----------------------------------------------------------------
            Dim pathPage = ui.CreateFlowPage("Paths")

            l = ui.AddLabel(pathPage, "Default Target Folder:")
            l.Help = "Leave empty to use the source file folder."

            Dim macroAction = Sub()
                                  MacrosForm.ShowDialogForm()
                              End Sub

            Dim tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.DefaultTargetFolder
            tm.Edit.SaveAction = Sub(value) p.DefaultTargetFolder = value
            tm.AddMenu("Browse Folder...", Function() g.BrowseFolder(p.DefaultTargetFolder))
            tm.AddMenu("Directory of source file", "%source_dir%")
            tm.AddMenu("Parent directory of source file directory", "%source_dir_parent%")
            tm.AddMenu("Macros...", macroAction)

            l = ui.AddLabel(pathPage, "Default Target Name:")
            l.Help = "Leave empty to use the source filename"
            l.MarginTop = Font.Height

            tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.DefaultTargetName
            tm.Edit.SaveAction = Sub(value) p.DefaultTargetName = value
            tm.AddMenu("Name of source file without extension", "%source_name%")
            tm.AddMenu("Name of source file directory", "%source_dir_name%")
            tm.AddMenu("Macros...", macroAction)

            l = ui.AddLabel(pathPage, If(p.DeleteTempFilesMode = DeleteMode.Disabled, "Temp Files Folder:", $"Temp Files Folder: (MUST end with '_temp{Path.DirectorySeparatorChar}' for Auto-Deletion!)"))
            l.Help = "Leave empty to use the source file folder."
            l.MarginTop = Font.Height

            Dim tempDirFunc = Function()
                                  Dim tempDir = g.BrowseFolder(p.TempDir)

                                  If tempDir <> "" Then
                                      Return Path.Combine(tempDir, "%source_name%_temp")
                                  End If
                              End Function

            tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.TempDir
            tm.Edit.SaveAction = Sub(value) p.TempDir = value
            tm.AddMenu("Browse Folder...", tempDirFunc)
            tm.AddMenu("Source File Directory", $"%source_dir%{Path.DirectorySeparatorChar}%source_name%_temp")
            tm.AddMenu("Macros...", macroAction)

            l = ui.AddLabel(pathPage, "Default Thumbnails Path without extension:")
            l.Help = "Leave empty to save it next to the video file"
            l.MarginTop = Font.Height

            tm = ui.AddTextMenu(pathPage)
            tm.Label.Visible = False
            tm.Edit.Expand = True
            tm.Edit.Text = p.ThumbnailerSettings.GetString("ImageFilePathWithoutExtension", "")
            tm.Edit.SaveAction = Sub(value) p.ThumbnailerSettings.SetString("ImageFilePathWithoutExtension", value)
            tm.AddMenu("Path of target file without extension + Postfix", $"%target_dir%{Path.DirectorySeparatorChar}%target_name%_Thumbnail")
            tm.AddMenu("Path of target file without extension", $"%target_dir%{Path.DirectorySeparatorChar}%target_name%")
            tm.AddMenu("Macros...", macroAction)



            '   ----------------------------------------------------------------
            Dim systemTempFilesPage = ui.CreateFlowPage("Paths | Temp Files", True)

            Dim deleteModeMenu = ui.AddMenu(Of DeleteMode)
            Dim deleteSelectionMenu = ui.AddMenu(Of DeleteSelection)
            Dim deleteSelectionModeMenu = ui.AddMenu(Of SelectionMode)

            eb = ui.AddEmptyBlock(systemTempFilesPage)
            eb.Margin = New Padding(0, 6, 0, 3)
            eb.Visible = p.DeleteTempFilesMode <> DeleteMode.Disabled
            ui.AddLabel(eb, "Allowed on Frame Mismatch:", 7)
            ui.AddLabel(eb, "Too less:", 2)
            Dim deleteOnFrameMismatchNegative = ui.AddNumeric(eb)
            ui.AddLabel(eb, " ", 1)
            ui.AddLabel(eb, "Too much:", 2)
            Dim deleteOnFrameMismatchPositive = ui.AddNumeric(eb)

            Dim deleteCustomLabel = ui.AddLabel("Custom file extensions (space separated):")
            Dim deleteCustom = ui.AddTextMenu()
            Dim deleteSelectiveLabelExcludeText = "Select what shall be excluded:"
            Dim deleteSelectiveLabelIncludeText = "Select what shall be included:"
            Dim deleteSelectiveLabel = ui.AddLabel(If(p.DeleteTempFilesSelectionMode = SelectionMode.Exclude, deleteSelectiveLabelExcludeText, deleteSelectiveLabelIncludeText))
            Dim deleteSelectiveProjects = ui.AddBool()
            Dim deleteSelectiveLogs = ui.AddBool()
            Dim deleteSelectiveScripts = ui.AddBool()
            Dim deleteSelectiveIndexes = ui.AddBool()
            Dim deleteSelectiveVideos = ui.AddBool()
            Dim deleteSelectiveAudios = ui.AddBool()
            Dim deleteSelectiveSubtitles = ui.AddBool()


            deleteModeMenu.Text = "Deletion after successful processing:"
            deleteModeMenu.Expanded = True
            deleteModeMenu.Field = NameOf(p.DeleteTempFilesMode)
            deleteModeMenu.Button.ValueChangedAction = Sub(value)
                                                           Dim deleteModeActive = value <> DeleteMode.Disabled
                                                           deleteSelectionMenu.Visible = deleteModeActive
                                                           deleteSelectionMenu.Button.ValueChangedAction.Invoke(deleteSelectionMenu.Button.Value)
                                                           eb.Visible = deleteModeActive
                                                       End Sub

            deleteSelectionMenu.Text = "Selection:"
            deleteSelectionMenu.Expanded = True
            deleteSelectionMenu.Field = NameOf(p.DeleteTempFilesSelection)
            deleteSelectionMenu.Visible = p.DeleteTempFilesMode <> DeleteMode.Disabled
            deleteSelectionMenu.Button.ValueChangedAction = Sub(value)
                                                                Dim activeCustom = deleteSelectionMenu.Visible AndAlso value = DeleteSelection.Custom
                                                                Dim activeSelective = deleteSelectionMenu.Visible AndAlso value = DeleteSelection.Selective
                                                                deleteSelectionModeMenu.Visible = activeCustom OrElse activeSelective
                                                                deleteCustomLabel.Visible = activeCustom
                                                                deleteCustom.Visible = activeCustom
                                                                deleteSelectiveLabel.Visible = activeSelective
                                                                deleteSelectiveProjects.Visible = activeSelective
                                                                deleteSelectiveLogs.Visible = activeSelective
                                                                deleteSelectiveScripts.Visible = activeSelective
                                                                deleteSelectiveIndexes.Visible = activeSelective
                                                                deleteSelectiveVideos.Visible = activeSelective
                                                                deleteSelectiveAudios.Visible = activeSelective
                                                                deleteSelectiveSubtitles.Visible = activeSelective
                                                            End Sub

            deleteSelectionModeMenu.Text = "Selection Mode:"
            deleteSelectionModeMenu.Expanded = True
            deleteSelectionModeMenu.Visible = p.DeleteTempFilesMode <> DeleteMode.Disabled AndAlso p.DeleteTempFilesSelection <> DeleteSelection.Everything
            deleteSelectionModeMenu.Field = NameOf(p.DeleteTempFilesSelectionMode)
            deleteSelectionModeMenu.Button.ValueChangedAction = Sub(value)
                                                                    deleteSelectiveLabel.Text = If(value = SelectionMode.Exclude, deleteSelectiveLabelExcludeText, deleteSelectiveLabelIncludeText)
                                                                    If value = SelectionMode.Exclude Then
                                                                        MsgWarn("Be aware!", "Every not selected, listed or identified file type will be deleted!")
                                                                    End If
                                                                End Sub

            deleteOnFrameMismatchNegative.Help = "Number of frames that the target file may have too less in order to delete the temp files. (-1 disables the check)"
            deleteOnFrameMismatchNegative.Config = {-1, 999, 1, 0}
            deleteOnFrameMismatchNegative.Field = NameOf(p.DeleteTempFilesOnFrameMismatchNegative)

            deleteOnFrameMismatchPositive.Help = "Number of frames that the target file may have too much in order to delete the temp files. (-1 disables the check)"
            deleteOnFrameMismatchPositive.Config = {-1, 999, 1, 0}
            deleteOnFrameMismatchPositive.Field = NameOf(p.DeleteTempFilesOnFrameMismatchPositive)

            deleteCustomLabel.MarginTop = Font.Height \ 2
            deleteCustomLabel.Visible = p.DeleteTempFilesMode <> DeleteMode.Disabled AndAlso p.DeleteTempFilesSelection = DeleteSelection.Custom

            Dim customAddFunc = Function(extensions As String()) As String
                                    Dim exts = deleteCustom.Edit.Text.ToLower().SplitNoEmpty(BR, " ")
                                    Dim allExts = exts.Union(extensions)
                                    Return allExts.Distinct().Sort().Join(" ")
                                End Function

            deleteCustom.Visible = p.DeleteTempFilesMode <> DeleteMode.Disabled AndAlso p.DeleteTempFilesSelection = DeleteSelection.Custom
            deleteCustom.Label.Visible = False
            deleteCustom.Edit.MultilineHeightFactor = 8
            deleteCustom.Edit.Expand = True
            deleteCustom.Edit.Text = p.DeleteTempFilesCustomSelection.Join(" ")
            deleteCustom.Edit.SaveAction = Sub(value) p.DeleteTempFilesCustomSelection = value.ToLower().SplitNoEmpty(BR, " ")
            deleteCustom.AddMenu("Add project file types", Function() customAddFunc(FileTypes.Projects))
            deleteCustom.AddMenu("Add log file types", Function() customAddFunc(FileTypes.Logs))
            deleteCustom.AddMenu("Add script file types", Function() customAddFunc(FileTypes.Scripts))
            deleteCustom.AddMenu("Add index file types", Function() customAddFunc(FileTypes.Indexes))
            deleteCustom.AddMenu("Add video file types", Function() customAddFunc(FileTypes.Video))
            deleteCustom.AddMenu("Add audio file types", Function() customAddFunc(FileTypes.Audio))
            deleteCustom.AddMenu("Add subtitle file types", Function() customAddFunc(FileTypes.SubtitleExludingContainers))

            Dim selectiveVisible = p.DeleteTempFilesMode <> DeleteMode.Disabled AndAlso p.DeleteTempFilesSelection = DeleteSelection.Selective

            deleteSelectiveLabel.MarginTop = Font.Height \ 2
            deleteSelectiveLabel.Visible = selectiveVisible

            deleteSelectiveProjects.Text = "Project files"
            deleteSelectiveProjects.Checked = p.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Projects)
            deleteSelectiveProjects.Visible = selectiveVisible
            deleteSelectiveProjects.SaveAction = Sub(value) p.DeleteTempFilesSelectiveSelection = If(value, p.DeleteTempFilesSelectiveSelection Or DeleteSelectiveSelection.Projects, p.DeleteTempFilesSelectiveSelection And Not DeleteSelectiveSelection.Projects)

            deleteSelectiveLogs.Text = "Log files"
            deleteSelectiveLogs.Checked = p.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Logs)
            deleteSelectiveLogs.Visible = selectiveVisible
            deleteSelectiveLogs.SaveAction = Sub(value) p.DeleteTempFilesSelectiveSelection = If(value, p.DeleteTempFilesSelectiveSelection Or DeleteSelectiveSelection.Logs, p.DeleteTempFilesSelectiveSelection And Not DeleteSelectiveSelection.Logs)

            deleteSelectiveScripts.Text = "Script files"
            deleteSelectiveScripts.Checked = p.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Scripts)
            deleteSelectiveScripts.Visible = selectiveVisible
            deleteSelectiveScripts.SaveAction = Sub(value) p.DeleteTempFilesSelectiveSelection = If(value, p.DeleteTempFilesSelectiveSelection Or DeleteSelectiveSelection.Scripts, p.DeleteTempFilesSelectiveSelection And Not DeleteSelectiveSelection.Scripts)

            deleteSelectiveIndexes.Text = "Index files"
            deleteSelectiveIndexes.Checked = p.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Indexes)
            deleteSelectiveIndexes.Visible = selectiveVisible
            deleteSelectiveIndexes.SaveAction = Sub(value) p.DeleteTempFilesSelectiveSelection = If(value, p.DeleteTempFilesSelectiveSelection Or DeleteSelectiveSelection.Indexes, p.DeleteTempFilesSelectiveSelection And Not DeleteSelectiveSelection.Indexes)

            deleteSelectiveVideos.Text = "Video files"
            deleteSelectiveVideos.Checked = p.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Videos)
            deleteSelectiveVideos.Visible = selectiveVisible
            deleteSelectiveVideos.SaveAction = Sub(value) p.DeleteTempFilesSelectiveSelection = If(value, p.DeleteTempFilesSelectiveSelection Or DeleteSelectiveSelection.Videos, p.DeleteTempFilesSelectiveSelection And Not DeleteSelectiveSelection.Videos)

            deleteSelectiveAudios.Text = "Audio files"
            deleteSelectiveAudios.Checked = p.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Audios)
            deleteSelectiveAudios.Visible = selectiveVisible
            deleteSelectiveAudios.SaveAction = Sub(value) p.DeleteTempFilesSelectiveSelection = If(value, p.DeleteTempFilesSelectiveSelection Or DeleteSelectiveSelection.Audios, p.DeleteTempFilesSelectiveSelection And Not DeleteSelectiveSelection.Audios)

            deleteSelectiveSubtitles.Text = "Subtitles"
            deleteSelectiveSubtitles.Checked = p.DeleteTempFilesSelectiveSelection.HasFlag(DeleteSelectiveSelection.Subtitles)
            deleteSelectiveSubtitles.Visible = selectiveVisible
            deleteSelectiveSubtitles.SaveAction = Sub(value) p.DeleteTempFilesSelectiveSelection = If(value, p.DeleteTempFilesSelectiveSelection Or DeleteSelectiveSelection.Subtitles, p.DeleteTempFilesSelectiveSelection And Not DeleteSelectiveSelection.Subtitles)


            '   ----------------------------------------------------------------
            ui.CreateFlowPage("Assistant")

            b = ui.AddBool()
            b.Text = "Remind to crop"
            b.Field = NameOf(p.RemindToCrop)

            b = ui.AddBool()
            b.Text = "Remind to cut"
            b.Field = NameOf(p.RemindToCut)

            b = ui.AddBool()
            b.Text = "Remind to do compressibility check"
            b.Field = NameOf(p.RemindToDoCompCheck)

            b = ui.AddBool()
            b.Text = "Remind to set filters"
            b.Field = NameOf(p.RemindToSetFilters)

            b = ui.AddBool()
            b.Text = "Warn on aspect ratio error"
            b.Field = NameOf(p.WarnArError)

            b = ui.AddBool()
            b.Text = "Warn if no audio in output"
            b.Field = NameOf(p.WarnNoAudio)

            b = ui.AddBool()
            b.Text = "Warn if identical audio is used multiple times"
            b.Field = NameOf(p.WarnIdenticalAudio)


            '   ----------------------------------------------------------------
            Dim filtersPage = ui.CreateFlowPage("Filters")

            l = ui.AddLabel(filtersPage, "Code appended to trim functions:")
            l.Help = "Code appended to trim functions StaxRip generates using the cut feature."
            l.MarginTop = Font.Height \ 2

            t = ui.AddText(filtersPage)
            t.Label.Visible = False
            t.Edit.Expand = True
            t.Edit.TextBox.Multiline = True
            t.Edit.UseMacroEditor = True
            t.Edit.Text = p.TrimCode
            t.Edit.SaveAction = Sub(value) p.TrimCode = value

            l = ui.AddLabel(filtersPage, "Code inserted at top of scripts:")
            l.Help = "Code inserted at the top of every script StaxRip generates."
            l.MarginTop = Font.Height \ 2

            t = ui.AddText(filtersPage)
            t.Label.Visible = False
            t.Edit.Expand = True
            t.Edit.TextBox.Multiline = True
            t.Edit.UseMacroEditor = True
            t.Edit.Text = p.CodeAtTop
            t.Edit.SaveAction = Sub(value) p.CodeAtTop = value

            l = ui.AddLabel(filtersPage, "Code inserted at bottom of scripts:")
            l.Help = "Code inserted at the bottom of every script StaxRip generates."
            l.MarginTop = Font.Height \ 2

            t = ui.AddText(filtersPage)
            t.Label.Visible = False
            t.Edit.Expand = True
            t.Edit.TextBox.Multiline = True
            t.Edit.UseMacroEditor = True
            t.Edit.Text = p.CodeAtBottom
            t.Edit.SaveAction = Sub(value) p.CodeAtBottom = value


            '   ----------------------------------------------------------------
            Dim miscPage = ui.CreateFlowPage("Misc")
            miscPage.SuspendLayout()

            b = ui.AddBool(miscPage)
            b.Text = "Hide dialogs asking to demux, source filter etc."
            b.Checked = p.NoDialogs
            b.SaveAction = Sub(value) p.NoDialogs = value

            b = ui.AddBool(miscPage)
            b.Text = "Use source file folder for temp files"
            b.Checked = p.NoTempDir
            b.SaveAction = Sub(value) p.NoTempDir = value

            b = ui.AddBool(miscPage)
            b.Text = "Abort on Frame Mismatch"
            b.Field = NameOf(p.AbortOnFrameMismatch)

            ui.AddLine(miscPage, "Compressibility Check")

            b = ui.AddBool(miscPage)
            b.Text = "Auto run compressibility check"
            b.Help = "Performs a compressibility check after loading a source file."
            b.Checked = p.AutoCompCheck
            b.SaveAction = Sub(value) p.AutoCompCheck = value

            n = ui.AddNum(miscPage)
            n.Label.Text = "Percentage of length to check"
            n.NumEdit.Config = {1, 25}
            n.NumEdit.Value = p.CompCheckPercentage
            n.NumEdit.SaveAction = Sub(value) p.CompCheckPercentage = value

            n = ui.AddNum(miscPage)
            n.Label.Text = "Seconds per test block"
            n.NumEdit.Config = {0.5, 10.0, 0.1, 2}
            n.NumEdit.Value = p.CompCheckTestblockSeconds
            n.NumEdit.SaveAction = Sub(value) p.CompCheckTestblockSeconds = value

            Dim compCheckButton = ui.AddMenu(Of CompCheckAction)(miscPage)
            compCheckButton.Label.Text = "After comp. check adjust"
            compCheckButton.Button.Value = p.CompCheckAction
            compCheckButton.Button.SaveAction = Sub(value) p.CompCheckAction = value

            miscPage.ResumeLayout()

            '   ----------------------------------------------------------------


            If pagePath <> "" Then
                ui.ShowPage(pagePath)
            Else
                ui.SelectLast("last options page")
            End If

            If form.ShowDialog() = DialogResult.OK Then
                Dim autoCropModeOn = autoCropMode.Button.Value <> StaxRip.AutoCropMode.Disabled
                Dim autoCropModeChanged = autoCropMode.Button.Value <> p.AutoCropMode
                Dim dvDataAvailable = p.HdrDolbyVisionMetadataFile IsNot Nothing
                Dim cropFilterActive = p.Script.GetFilter("Crop")?.Active
                Dim cropChanged = leftCrop.Value <> p.CropLeft OrElse topCrop.Value <> p.CropTop OrElse rightCrop.Value <> p.CropRight OrElse bottomCrop.Value <> p.CropBottom
                Dim dvThresholdChanged = doviThresholdBegin.Value <> p.AutoCropDolbyVisionThresholdBegin OrElse doviThresholdEnd.Value <> p.AutoCropDolbyVisionThresholdEnd

                ui.Save()

                If p.CompCheckPercentage < 1 OrElse p.CompCheckPercentage > 25 Then p.CompCheckPercentage = 5
                If p.CompCheckTestblockSeconds < 0.5 OrElse p.CompCheckTestblockSeconds > 10.0 Then p.CompCheckTestblockSeconds = 2.0
                If autoCropModeOn AndAlso autoCropModeChanged AndAlso dvDataAvailable AndAlso cropFilterActive Then StartAutoCrop()

                If cropChanged Then
                    p.CropLeft = CInt(leftCrop.Value)
                    p.CropTop = CInt(topCrop.Value)
                    p.CropRight = CInt(rightCrop.Value)
                    p.CropBottom = CInt(bottomCrop.Value)
                End If

                UpdateSizeOrBitrate()
                tbBitrate_TextChanged()
                SetSlider()
                SetAudioTracks(p)
                Assistant()
            End If

            ui.SaveLast("last options page")
        End Using
    End Sub
End Class
