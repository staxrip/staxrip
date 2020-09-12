
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports StaxRip.UI

<Serializable()>
Public Class Macro
    Implements IComparable(Of Macro)

    Sub New()
        MyClass.New("", "", GetType(String), "")
    End Sub

    Sub New(name As String,
            friendlyName As String,
            type As Type,
            description As String)

        If name.StartsWith("$") Then
            Me.Name = name
        Else
            Me.Name = "%" + name + "%"
        End If

        Me.FriendlyName = friendlyName
        Me.Type = type
        Me.Description = description
    End Sub

    Private NameValue As String

    Property Name() As String
        Get
            If NameValue Is Nothing Then
                NameValue = ""
            End If

            Return NameValue
        End Get
        Set(Value As String)
            NameValue = Value
        End Set
    End Property

    Private FriendlyNameValue As String

    Property FriendlyName() As String
        Get
            If FriendlyNameValue = "" AndAlso NameValue <> "" Then
                FriendlyNameValue = NameValue.Replace("_", " ").Replace("%", " ").Trim(" "c).ToTitleCase
            End If

            If FriendlyNameValue Is Nothing Then FriendlyNameValue = ""

            Return FriendlyNameValue
        End Get
        Set(Value As String)
            FriendlyNameValue = Value
        End Set
    End Property

    Private TypeValue As Type

    Property Type As Type
        Get
            If TypeValue Is Nothing Then TypeValue = GetType(String)

            Return TypeValue
        End Get
        Set(Value As Type)
            TypeValue = Value
        End Set
    End Property

    Private DescriptionValue As String

    Property Description() As String
        Get
            If DescriptionValue Is Nothing Then
                DescriptionValue = ""
            End If

            Return DescriptionValue
        End Get
        Set(Value As String)
            DescriptionValue = Value
        End Set
    End Property

    Shared Function GetTips(includeInteractive As Boolean,
                            includeParam As Boolean,
                            includeApps As Boolean) As StringPairList

        Dim ret As New StringPairList

        For Each macro In GetMacros(includeInteractive, includeParam, includeApps)
            ret.Add(macro.Name, macro.Description)
        Next

        Return ret
    End Function

    Shared Function GetTipsFriendly(convertHTMLChars As Boolean) As StringPairList
        Dim ret As New StringPairList

        For Each mac As Macro In GetMacros(False, False, False)
            If convertHTMLChars Then
                ret.Add(HelpDocument.ConvertChars(mac.FriendlyName), mac.Description)
            Else
                ret.Add(mac.FriendlyName, mac.Description)
            End If
        Next

        Return ret
    End Function

    Overrides Function ToString() As String
        Return Name
    End Function

    Function CompareTo(other As Macro) As Integer Implements System.IComparable(Of Macro).CompareTo
        Return Name.CompareTo(other.Name)
    End Function

    Shared Function GetMacros(
        includeInteractive As Boolean, includeParam As Boolean, includeApps As Boolean) As List(Of Macro)

        Dim ret As New List(Of Macro)

        If includeInteractive Then
            ret.Add(New Macro("$browse_file$", "Browse For File", GetType(String), "Filepath returned from a file browser."))
            ret.Add(New Macro("$enter_text$", "Enter Text", GetType(String), "Text entered in a input box."))
            ret.Add(New Macro("$enter_text:prompt$", "Enter Text (Params)", GetType(String), "Text entered in a input box."))
            ret.Add(New Macro("$select:param1;param2;...$", "Select", GetType(String), "String selected from dropdown, to show a optional message the first parameter has to start with msg: and to give the items optional captions use caption|value. Example: $select:msg:hello;caption1|value1;caption2|value2$"))
        End If

        If includeParam Then
            ret.Add(New Macro("app:name", "Application File Path", GetType(String), "Returns the path of a tool, it can be any type of tool found in the Apps dialog. Example: %app:qtgmc%"))
            ret.Add(New Macro("app_dir:name", "Application Directory", GetType(String), "Returns the directory of a tool, it can be any type of tool found in the Apps dialog. Example: %app_dir:x265%"))
            ret.Add(New Macro("eval:expression", "Eval Math Expression", GetType(String), "Evaluates a PowerShell expression which may contain macros."))
            ret.Add(New Macro("eval_ps:expression", "Eval PowerShell Expression", GetType(String), "This macro is obsolete since 2020."))
            ret.Add(New Macro("filter:name", "Filter", GetType(String), "Returns the script code of a filter of the active project that matches the specified name."))
            ret.Add(New Macro("media_info_audio:property", "MediaInfo Audio Property", GetType(String), "Returns a MediaInfo audio property for the video source file."))
            ret.Add(New Macro("media_info_video:property", "MediaInfo Video Property", GetType(String), "Returns a MediaInfo video property for the source file."))
        End If

        ret.Add(New Macro("audio_bitrate", "Audio Bitrate", GetType(Integer), "Overall audio bitrate."))
        ret.Add(New Macro("audio_file1", "First Audio File", GetType(String), "File path of the first audio file."))
        ret.Add(New Macro("audio_file2", "Second Audio File", GetType(String), "File path of the second audio file."))
        ret.Add(New Macro("compressibility", "Compressibility", GetType(Integer), "Compressibility value."))
        ret.Add(New Macro("crop_bottom", "Crop Bottom", GetType(Integer), "Bottom crop value."))
        ret.Add(New Macro("crop_height", "Crop Height", GetType(Integer), "Crop height."))
        ret.Add(New Macro("crop_left", "Crop Left", GetType(Integer), "Left crop value."))
        ret.Add(New Macro("crop_right", "Crop Right", GetType(Integer), "Right crop value."))
        ret.Add(New Macro("crop_top", "Crop Top", GetType(Integer), "Top crop value."))
        ret.Add(New Macro("crop_width", "Crop Width", GetType(Integer), "Crop width."))
        ret.Add(New Macro("delay", "Audio Delay 1", GetType(Integer), "Audio delay of the first audio track."))
        ret.Add(New Macro("delay2", "Audio Delay 2", GetType(Integer), "Audio delay of the second audio track."))
        ret.Add(New Macro("dpi", "Main Dialog DPI", GetType(Integer), "DPI value of the main dialog."))
        ret.Add(New Macro("encoder_ext", "Encoder File Extension", GetType(String), "File extension of the format the encoder of the active project outputs."))
        ret.Add(New Macro("encoder_out_file", "Encoder Output File", GetType(String), "Output file of the video encoder."))
        ret.Add(New Macro("muxer_ext", "Muxer Extension", GetType(String), "Output extension of the active muxer."))
        ret.Add(New Macro("player", "Player", GetType(Integer), "Path of the media player."))
        ret.Add(New Macro("plugin_dir", "Plugin Directory", GetType(String), "AviSynth/VapourSynth plugin auto load directory."))
        ret.Add(New Macro("pos_frame", "Position In Frames", GetType(Integer), "Current preview position in frames."))
        ret.Add(New Macro("pos_ms", "Position In Millisecons", GetType(Integer), "Current preview position in milliseconds."))
        ret.Add(New Macro("processing", "Processing", GetType(String), "Returns 'True' if a job is currently processing otherwise 'False'."))
        ret.Add(New Macro("programs_dir", "Programs Directory", GetType(String), "Programs system directory."))
        ret.Add(New Macro("script_dir", "Script Directory", GetType(String), "Users PowerShell scripts directory."))
        ret.Add(New Macro("script_ext", "Script File Extension", GetType(String), "File extension of the AviSynth/VapourSynth script so either avs or vpy."))
        ret.Add(New Macro("script_file", "Script Path", GetType(String), "Path of the AviSynth/VapourSynth script."))
        ret.Add(New Macro("sel_end", "Selection End", GetType(Integer), "End position of the first selecion in the preview."))
        ret.Add(New Macro("sel_start", "Selection Start", GetType(Integer), "Start position of the first selecion in the preview."))
        ret.Add(New Macro("settings_dir", "Settings Directory", GetType(String), "Path of the settings direcory."))
        ret.Add(New Macro("source_dar", "Source Display Aspect Ratio", GetType(String), "Source display aspect ratio."))
        ret.Add(New Macro("source_dir", "Source Directory", GetType(String), "Directory of the source file."))
        ret.Add(New Macro("source_dir_name", "Source Directory Name", GetType(String), "Name of the source file directory."))
        ret.Add(New Macro("source_dir_parent", "Source Directory Parent", GetType(String), "Parent directory of the source file directory."))
        ret.Add(New Macro("source_ext", "Source File Extension", GetType(String), "File extension of the source file."))
        ret.Add(New Macro("source_file", "Source File Path", GetType(String), "File path of the source video."))
        ret.Add(New Macro("source_files", "Source Files Blank", GetType(String), "Source files in quotes separated by a blank."))
        ret.Add(New Macro("source_files_comma", "Source Files Comma", GetType(String), "Source files in quotes separated by comma."))
        ret.Add(New Macro("source_framerate", "Source Framerate", GetType(Integer), "Frame rate returned by the source filter AviSynth section."))
        ret.Add(New Macro("source_frames", "Source Frames", GetType(Integer), "Length in frames of the source video."))
        ret.Add(New Macro("source_height", "Source Image Height", GetType(Integer), "Image height of the source video."))
        ret.Add(New Macro("source_name", "Source Filename Without Extension", GetType(String), "The name of the source file without file extension."))
        ret.Add(New Macro("source_par_x", "Source Pixel Aspect Ratio X", GetType(String), "Source pixel/sample aspect ratio."))
        ret.Add(New Macro("source_par_y", "Source Pixel Aspect Ratio Y", GetType(String), "Source pixel/sample aspect ratio."))
        ret.Add(New Macro("source_seconds", "Source Seconds", GetType(Integer), "Length in seconds of the source video."))
        ret.Add(New Macro("source_temp_file", "Source Temp File", GetType(String), "File located in the temp directory using the same name as the source file."))
        ret.Add(New Macro("source_video_format", "Source Video Codec", GetType(String), "Video codec of the source file."))
        ret.Add(New Macro("source_width", "Source Image Width", GetType(Integer), "Image width of the source video."))
        ret.Add(New Macro("startup_dir", "Startup Directory", GetType(String), "Directory of the application."))
        ret.Add(New Macro("system_dir", "System Directory", GetType(String), "System directory."))
        ret.Add(New Macro("target_dar", "Target Display Aspect Ratio", GetType(String), "Target display aspect ratio."))
        ret.Add(New Macro("target_dir", "Target Directory", GetType(String), "Directory of the target file."))
        ret.Add(New Macro("target_file", "Target File Path", GetType(String), "File path of the target file."))
        ret.Add(New Macro("target_framerate", "Target Framerate", GetType(Integer), "Frame rate of the target video."))
        ret.Add(New Macro("target_frames", "Target Frames", GetType(Integer), "Length in frames of the target video."))
        ret.Add(New Macro("target_height", "Target Image Height", GetType(Integer), "Image height of the target video."))
        ret.Add(New Macro("target_name", "Target Filename Without Extension", GetType(String), "Name of the target file without file extension."))
        ret.Add(New Macro("target_par_x", "Target Pixel Aspect Ratio X", GetType(String), "Target pixel/sample aspect ratio."))
        ret.Add(New Macro("target_par_y", "Target Pixel Aspect Ratio Y", GetType(String), "Target pixel/sample aspect ratio."))
        ret.Add(New Macro("target_seconds", "Target Seconds", GetType(Integer), "Length in seconds of the target video."))
        ret.Add(New Macro("target_size", "Target Size", GetType(Integer), "Size of the target video in kilo bytes."))
        ret.Add(New Macro("target_temp_file", "Target Temp File", GetType(String), "File located in the temp directory using the same name as the target file."))
        ret.Add(New Macro("target_width", "Target Image Width", GetType(Integer), "Image width of the target video."))
        ret.Add(New Macro("temp_dir", "Temp Directory", GetType(String), "Directory of the source file or the temp directory if enabled."))
        ret.Add(New Macro("temp_file", "Temp File", GetType(String), "File located in the temp directory using the same name as the source file."))
        ret.Add(New Macro("template_name", "Template Name", GetType(String), "Name of the template the active project is based on."))
        ret.Add(New Macro("text_editor", "Text Editor", GetType(String), "Path of the application currently associated with TXT files."))
        ret.Add(New Macro("version", "Version", GetType(String), "StaxRip version."))
        ret.Add(New Macro("video_bitrate", "Video Bitrate", GetType(Integer), "Video bitrate in Kbps"))
        ret.Add(New Macro("video_encoder", "Video Encoder", GetType(String), "Depending on which video encoder is active returns x264, x265, nvenc, qsvenc, vceenc, aomenc, ffmpeg or xvid_encraw."))
        ret.Add(New Macro("working_dir", "Working Directory", GetType(String), "Directory of the source file or the temp directory if enabled."))

        ret.Sort()

        If includeApps Then
            For Each i In Package.Items.Values
                ret.Add(New Macro("app:" + i.Name, "File path to " + i.Name, GetType(String), "File path to " + i.Name))
            Next

            For Each i In Package.Items.Values
                ret.Add(New Macro("app_dir:" + i.Name, "Folder path to " + i.Name, GetType(String), "Folder path to " + i.Name))
            Next
        End If

        Return ret
    End Function

    Shared Function ExpandGUI(
        value As String,
        Optional throwIfCancel As Boolean = False) As (Value As String, Caption As String, Cancel As Boolean)

        Dim ret As (Value As String, Caption As String, Cancel As Boolean) = (value, "", False)

        If ret.Value = "" Then
            Return ret
        End If

        If ret.Value.Contains("$browse_file$") Then
            Using dialog As New OpenFileDialog
                ret.Cancel = dialog.ShowDialog <> DialogResult.OK

                If ret.Cancel Then
                    If throwIfCancel Then
                        Throw New AbortException
                    End If

                    Return ret
                Else
                    ret.Value = ret.Value.Replace("$browse_file$", dialog.FileName)
                End If
            End Using
        End If

        If Not ret.Value.Contains("$") Then Return ret

        If ret.Value.Contains("$enter_text$") Then
            Dim inputText = InputBox.Show("Please enter text/value.")

            If inputText = "" Then
                ret.Cancel = True
                Return ret
            Else
                ret.Value = ret.Value.Replace("$enter_text$", inputText)
            End If
        End If

        If Not ret.Value.Contains("$") Then Return ret

        If ret.Value.Contains("$enter_text:") Then
            Dim matches = Regex.Matches(ret.Value, "\$enter_text:(.+?)\$")

            For Each iMatch As Match In matches
                Dim inputText = InputBox.Show(iMatch.Groups(1).Value)

                If inputText = "" Then
                    ret.Cancel = True
                    Return ret
                Else
                    ret.Value = ret.Value.Replace(iMatch.Value, inputText)
                End If
            Next
        End If

        If Not ret.Value.Contains("$") Then Return ret

        If ret.Value.Contains("$select:") Then
            Dim matches = Regex.Matches(ret.Value, "\$select:(.+?)\$")

            For Each iMatch As Match In matches
                Dim items = iMatch.Groups(1).Value.SplitNoEmpty(";").ToList

                If items.Count > 0 Then
                    Using td As New TaskDialog(Of String)
                        If items?(0)?.StartsWith("msg:") Then
                            td.MainInstruction = items(0).Substring(4)
                            items.RemoveAt(0)
                        Else
                            td.MainInstruction = "Please select one of the options."
                        End If

                        For Each iItem As String In items
                            If iItem.Contains("|") Then
                                td.AddCommand(iItem.Left("|"), iItem.Right("|"))
                            Else
                                td.AddCommand(iItem, iItem)
                            End If
                        Next

                        ret.Cancel = td.Show = ""

                        If ret.Cancel Then
                            Return ret
                        Else
                            ret.Caption = td.SelectedText
                            ret.Value = ret.Value.Replace(iMatch.Value, td.SelectedValue)
                        End If
                    End Using
                End If
            Next
        End If

        Return ret
    End Function

    Shared Function Expand(value As String) As String
        If value = "" Then Return ""
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_file%") Then value = value.Replace("%source_file%", p.SourceFile.LongPathPrefix)
        If Not value.Contains("%") Then Return value

        If value.Contains("%working_dir%") Then value = value.Replace("%working_dir%", p.TempDir)
        If Not value.Contains("%") Then Return value

        If value.Contains("%temp_dir%") Then value = value.Replace("%temp_dir%", p.TempDir)
        If Not value.Contains("%") Then Return value

        If value.Contains("%temp_file%") Then value = value.Replace("%temp_file%", p.TempDir + p.SourceFile.Base)
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_temp_file%") Then value = value.Replace("%source_temp_file%", (p.TempDir + g.GetSourceBase).ToShortFilePath)
        If Not value.Contains("%") Then Return value

        If value.Contains("%target_temp_file%") Then value = value.Replace("%target_temp_file%", p.TempDir + p.TargetFile.Base)
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_name%") Then value = value.Replace("%source_name%", p.SourceFile.Base)
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_ext%") Then value = value.Replace("%source_ext%", p.FirstOriginalSourceFile.Ext)
        If Not value.Contains("%") Then Return value

        If value.Contains("%version%") Then value = value.Replace("%version%", Application.ProductVersion)
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_width%") Then value = value.Replace("%source_width%", p.SourceWidth.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_height%") Then value = value.Replace("%source_height%", p.SourceHeight.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_seconds%") Then value = value.Replace("%source_seconds%", p.SourceSeconds.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_frames%") Then value = value.Replace("%source_frames%", p.SourceFrames.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_framerate%") Then value = value.Replace("%source_framerate%", p.SourceFrameRate.ToString("f6", CultureInfo.InvariantCulture))
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_dir%") Then value = value.Replace("%source_dir%", p.SourceFile.Dir)
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_dir_parent%") Then value = value.Replace("%source_dir_parent%", p.SourceFile.Dir.Parent)
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_dir_name%") Then value = value.Replace("%source_dir_name%", p.SourceFile.Dir.DirName)
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_video_format%") Then value = value.Replace("%source_video_format%", MediaInfo.GetVideo(p.FirstOriginalSourceFile, "Format"))
        If Not value.Contains("%") Then Return value

        If value.Contains("%target_width%") Then value = value.Replace("%target_width%", p.TargetWidth.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%target_height%") Then value = value.Replace("%target_height%", p.TargetHeight.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%target_seconds%") Then value = value.Replace("%target_seconds%", p.TargetSeconds.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%target_frames%") Then value = value.Replace("%target_frames%", p.Script.Info.FrameCount.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%target_framerate%") Then value = value.Replace("%target_framerate%", p.Script.GetCachedFrameRate.ToString("f6", CultureInfo.InvariantCulture))
        If Not value.Contains("%") Then Return value

        If value.Contains("%target_size%") Then value = value.Replace("%target_size%", (p.TargetSize * 1024).ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%target_file%") Then value = value.Replace("%target_file%", p.TargetFile)
        If Not value.Contains("%") Then Return value

        If value.Contains("%target_dir%") Then value = value.Replace("%target_dir%", p.TargetFile.Dir)
        If Not value.Contains("%") Then Return value

        If value.Contains("%target_name%") Then value = value.Replace("%target_name%", p.TargetFile.Base)
        If Not value.Contains("%") Then Return value

        If value.Contains("%crop_width%") Then value = value.Replace("%crop_width%", (p.SourceWidth - p.CropLeft - p.CropRight).ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%crop_height%") Then value = value.Replace("%crop_height%", (p.SourceHeight - p.CropTop - p.CropBottom).ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%crop_left%") Then value = value.Replace("%crop_left%", p.CropLeft.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%crop_top%") Then value = value.Replace("%crop_top%", p.CropTop.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%crop_right%") Then value = value.Replace("%crop_right%", p.CropRight.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%crop_bottom%") Then value = value.Replace("%crop_bottom%", p.CropBottom.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%video_bitrate%") Then value = value.Replace("%video_bitrate%", p.VideoBitrate.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%audio_bitrate%") Then value = value.Replace("%audio_bitrate%", (p.Audio0.Bitrate + p.Audio1.Bitrate).ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%audio_file1%") Then value = value.Replace("%audio_file1%", p.Audio0.File)
        If Not value.Contains("%") Then Return value

        If value.Contains("%audio_file2%") Then value = value.Replace("%audio_file2%", p.Audio1.File)
        If Not value.Contains("%") Then Return value

        If value.Contains("%delay%") Then value = value.Replace("%delay%", p.Audio0.Delay.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%delay2%") Then value = value.Replace("%delay2%", p.Audio1.Delay.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%startup_dir%") Then value = value.Replace("%startup_dir%", Folder.Startup)
        If Not value.Contains("%") Then Return value

        If value.Contains("%system_dir%") Then value = value.Replace("%system_dir%", Folder.System)
        If Not value.Contains("%") Then Return value

        If value.Contains("%script_dir%") Then value = value.Replace("%script_dir%", Folder.Scripts)
        If Not value.Contains("%") Then Return value

        If value.Contains("%programs_dir%") Then value = value.Replace("%programs_dir%", Folder.Programs)
        If Not value.Contains("%") Then Return value

        If value.Contains("%plugin_dir%") Then value = value.Replace("%plugin_dir%", Folder.Plugins)
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_files_comma%") Then value = value.Replace("%source_files_comma%", """" + String.Join(""",""", p.SourceFiles.ToArray) + """")
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_files%") Then value = value.Replace("%source_files%", """" + String.Join(""" """, p.SourceFiles.ToArray) + """")
        If Not value.Contains("%") Then Return value

        If value.Contains("%compressibility%") Then value = value.Replace("%compressibility%", Math.Round(p.Compressibility, 3).ToString.Replace(",", "."))
        If Not value.Contains("%") Then Return value

        If value.Contains("%encoder_out_file%") Then value = value.Replace("%encoder_out_file%", p.VideoEncoder.OutputPath)
        If Not value.Contains("%") Then Return value

        If value.Contains("%encoder_ext%") Then value = value.Replace("%encoder_ext%", p.VideoEncoder.OutputExt)
        If Not value.Contains("%") Then Return value

        If value.Contains("%muxer_ext%") Then value = value.Replace("%muxer_ext%", p.VideoEncoder.Muxer.OutputExt)
        If Not value.Contains("%") Then Return value

        If value.Contains("%script_ext%") Then value = value.Replace("%script_ext%", p.Script.FileType)
        If Not value.Contains("%") Then Return value

        If value.Contains("%pos_frame%") Then value = value.Replace("%pos_frame%", s.LastPosition.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%template_name%") Then value = value.Replace("%template_name%", p.TemplateName)
        If Not value.Contains("%") Then Return value

        If value.Contains("%settings_dir%") Then value = value.Replace("%settings_dir%", Folder.Settings)
        If Not value.Contains("%") Then Return value

        If value.Contains("%player%") Then value = value.Replace("%player%", Package.mpvnet.Path)
        If Not value.Contains("%") Then Return value

        If value.Contains("%text_editor%") Then value = value.Replace("%text_editor%", g.GetTextEditorPath)
        If Not value.Contains("%") Then Return value

        If value.Contains("%processing%") Then value = value.Replace("%processing%", g.IsJobProcessing.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%video_encoder%") Then value = value.Replace("%video_encoder%", TryCast(p.VideoEncoder, BasicVideoEncoder)?.CommandLineParams.GetPackage.Name)
        If Not value.Contains("%") Then Return value

        If value.Contains("%dpi%") Then value = value.Replace("%dpi%", g.DPI.ToString())
        If Not value.Contains("%") Then Return value

        If value.Contains("%script_file%") Then value = value.Replace("%script_file%", p.Script.Path)
        If Not value.Contains("%") Then Return value

        If value.Contains("%pos_ms%") Then value = value.Replace("%pos_ms%", g.GetPreviewPosMS.ToString)
        If Not value.Contains("%") Then Return value

        If value.Contains("%source_par_x%") Then
            Dim par = Calc.GetSourcePAR
            value = value.Replace("%source_par_x%", par.X.ToString)

            If Not value.Contains("%") Then
                Return value
            End If
        End If

        If value.Contains("%source_par_y%") Then
            Dim par = Calc.GetSourcePAR
            value = value.Replace("%source_par_y%", par.Y.ToString)

            If Not value.Contains("%") Then
                Return value
            End If
        End If

        If value.Contains("%target_par_x%") Then
            Dim par = Calc.GetTargetPAR
            value = value.Replace("%target_par_x%", par.X.ToString)

            If Not value.Contains("%") Then
                Return value
            End If
        End If

        If value.Contains("%target_par_y%") Then
            Dim par = Calc.GetTargetPAR
            value = value.Replace("%target_par_y%", par.Y.ToString)

            If Not value.Contains("%") Then
                Return value
            End If
        End If

        If value.Contains("%source_dar%") Then
            Dim dar = Calc.GetSourceDAR
            value = value.Replace("%source_dar%", dar.ToString("f9", CultureInfo.InvariantCulture))

            If Not value.Contains("%") Then
                Return value
            End If
        End If

        If value.Contains("%target_dar%") Then
            Dim dar = Calc.GetTargetDAR
            value = value.Replace("%target_dar%", dar.ToString("f9", CultureInfo.InvariantCulture))

            If Not value.Contains("%") Then
                Return value
            End If
        End If

        If p.Ranges.Count > 0 Then
            If value.Contains("%sel_start%") Then value = value.Replace("%sel_start%", p.Ranges(0).Start.ToString)
            If Not value.Contains("%") Then Return value

            If value.Contains("%sel_end%") Then value = value.Replace("%sel_end%", p.Ranges(0).End.ToString)
            If Not value.Contains("%") Then Return value
        Else
            If value.Contains("%sel_start%") Then value = value.Replace("%sel_start%", 0.ToString)
            If Not value.Contains("%") Then Return value

            If value.Contains("%sel_end%") Then value = value.Replace("%sel_end%", 0.ToString)
            If Not value.Contains("%") Then Return value
        End If

        If value.Contains("%app:") Then
            Dim mc = Regex.Matches(value, "%app:(.+?)%")

            For Each match As Match In mc
                Dim package = StaxRip.Package.Items.Values.FirstOrDefault(
                    Function(pack) pack.Name.ToLower = match.Groups(1).Value.ToLower)

                Dim path = package?.Path

                If path <> "" Then
                    value = value.Replace(match.Value, path)

                    If Not value.Contains("%") Then
                        Return value
                    End If
                End If
            Next
        End If

        If value.Contains("%app_dir:") Then
            For Each match As Match In Regex.Matches(value, "%app_dir:(.+?)%")
                Dim package = StaxRip.Package.Items.Values.FirstOrDefault(
                    Function(pack) pack.Name.ToLower = match.Groups(1).Value.ToLower)

                Dim path = package?.Path

                If path <> "" Then
                    value = value.Replace(match.Value, path.Dir)
                    If Not value.Contains("%") Then
                        Return value
                    End If
                End If
            Next
        End If

        If value.Contains("%media_info_video:") Then
            For Each i As Match In Regex.Matches(value, "%media_info_video:(.+?)%")
                value = value.Replace(i.Value, MediaInfo.GetVideo(p.LastOriginalSourceFile, i.Groups(1).Value))

                If Not value.Contains("%") Then
                    Return value
                End If
            Next
        End If

        If value.Contains("%media_info_audio:") Then
            For Each i As Match In Regex.Matches(value, "%media_info_audio:(.+?)%")
                value = value.Replace(i.Value, MediaInfo.GetAudio(p.LastOriginalSourceFile, i.Groups(1).Value))

                If Not value.Contains("%") Then
                    Return value
                End If
            Next
        End If

        If value.Contains("%filter:") Then
            Dim mc = Regex.Matches(value, "%filter:(.+?)%")

            For Each i As Match In mc
                For Each i2 In p.Script.Filters
                    If i2.Active AndAlso i2.Path.ToUpper = i.Groups(1).Value.ToUpper Then
                        value = value.Replace(i.Value, i2.Script)

                        If Not value.Contains("%") Then
                            Return value
                        End If

                        Exit For
                    End If
                Next

                value = value.Replace(i.Value, "")

                If Not value.Contains("%") Then
                    Return value
                End If
            Next
        End If

        If value.Contains("%eval:") Then
            If Not value.Contains("%eval:<expression>%") AndAlso Not value.Contains("%eval:expression%") Then
                Dim matches = Regex.Matches(value, "%eval:(.+?)%")

                For Each ma As Match In matches
                    Try
                        value = value.Replace(ma.Value, PowerShell.InvokeAndConvert(ma.Groups(1).Value))
                    Catch ex As Exception
                        value = value.Replace(ma.Value, ex.ToString)
                    End Try

                    If Not value.Contains("%") Then
                        Return value
                    End If
                Next
            End If
        End If

        If value.Contains("%eval_ps:") Then
            If Not value.Contains("%eval_ps:<expression>%") AndAlso Not value.Contains("%eval_ps:expression%") Then
                Dim matches = Regex.Matches(value, "%eval_ps:(.+?)%")

                For Each ma As Match In matches
                    Try
                        value = value.Replace(ma.Value, PowerShell.InvokeAndConvert(ma.Groups(1).Value))
                    Catch ex As Exception
                        value = value.Replace(ma.Value, ex.ToString)
                    End Try

                    If Not value.Contains("%") Then
                        Return value
                    End If
                Next
            End If
        End If

        For Each var In OS.EnvVars
            If value = "" OrElse var = "" Then
                Continue For
            End If

            If value.ToLowerInvariant.Contains("%" + var.ToLowerInvariant + "%") Then
                value = Environment.ExpandEnvironmentVariables(value)

                If Not value.Contains("%") Then
                    Return value
                End If

                Exit For
            End If
        Next

        Return value
    End Function
End Class
