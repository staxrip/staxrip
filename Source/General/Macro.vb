
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic
Imports StaxRip.UI
Imports StaxRip.VideoEncoderCommandLine

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

    Shared Function GetTips(includeInteractive As Boolean, includeParam As Boolean, includeWhileProcessing As Boolean) As StringPairList
        Dim ret As New StringPairList

        For Each macro In GetMacros(includeInteractive, includeParam, includeWhileProcessing)
            ret.Add(macro.Name, macro.Description)
        Next

        Return ret
    End Function

    Shared Function GetTipsFriendly(convertHTMLChars As Boolean) As StringPairList
        Dim ret As New StringPairList

        For Each mac As Macro In GetMacros(False, False, True)
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

    Shared Function GetMacros(includeInteractive As Boolean, includeParam As Boolean, includeWhileProcessing As Boolean) As List(Of Macro)
        Dim ret As New List(Of Macro)

        If includeInteractive Then
            ret.Add(New Macro("$browse_file$", "Browse For File", GetType(String), "Filepath returned from a file browser."))
            ret.Add(New Macro("$enter_text$", "Enter Text", GetType(String), "Text entered in an input box."))
            ret.Add(New Macro("$enter_text:prompt$", "Enter Text (Params)", GetType(String), "Text entered in an input box."))
            ret.Add(New Macro("$select:param1;param2;...$", "Select", GetType(String), "String selected from dropdown, to show an optional message the first parameter has to start with msg: and to give the items optional captions use caption|value. Example: $select:msg:hello;caption1|value1;caption2|value2$"))
        End If

        If includeParam Then
            ret.Add(New Macro("app:name", "Application File Path", GetType(String), "Returns the path of a given tool, it can be any type of tool found in the Apps dialog. Example: %app:x265%"))
            ret.Add(New Macro("app_dir:name", "Application Directory", GetType(String), "Returns the directory of a given tool, it can be any type of tool found in the Apps dialog. Example: %app_dir:x265%"))
            ret.Add(New Macro("app_path:name", "Application File Path", GetType(String), "Returns the path of a given tool, it can be any type of tool found in the Apps dialog. Example: %app:x265%"))
            ret.Add(New Macro("app_version:name", "Application Version", GetType(String), "Returns the version of a given tool, it can be any type of tool found in the Apps dialog. Example: %version:x265%"))
            ret.Add(New Macro("eval:expression", "Eval Math Expression", GetType(String), "Evaluates a PowerShell expression which may contain macros."))
            ret.Add(New Macro("filter:name", "Filter", GetType(String), "Returns the script code of a filter of the active project that matches the specified name."))
            ret.Add(New Macro("random:digits", "Random Number", GetType(Integer), "Returns a 'digits' long random number, whereas 'digits' is clamped between 1 and 10."))
            ret.Add(New Macro("source_mi_g:property", "MediaInfo General Property", GetType(String), "Returns the given MediaInfo property from the General section for the source file."))
            ret.Add(New Macro("source_mi_v:property", "MediaInfo Video Property", GetType(String), "Returns the given MediaInfo property from the Video section for the source file. Before ':' you can add a zero-based index for the track number of that section."))
            ret.Add(New Macro("source_mi_a:property", "MediaInfo Audio Property", GetType(String), "Returns the given MediaInfo property from the Audio section for the source file. Before ':' you can add a zero-based index for the track number of that section."))
            ret.Add(New Macro("source_mi_t:property", "MediaInfo Text Property", GetType(String), "Returns the given MediaInfo property from the Text section for the source file. Before ':' you can add a zero-based index for the track number of that section."))
        End If

        If includeWhileProcessing Then
            ret.Add(New Macro("commandline", "Command Line", GetType(String), "Returns the command line used for the running app."))
            ret.Add(New Macro("progress", "Progress", GetType(Integer), "Returns the current progress as Integer value."))
            ret.Add(New Macro("progressline", "Progress Line", GetType(String), "Returns the progress line received from the running app."))
        End If

        ret.Add(New Macro("audio_bitrate", "Audio Bitrate", GetType(Integer), "Overall audio bitrate."))
        ret.Add(New Macro("audio_bitrateX", "Audio Bitrate X", GetType(Integer), "Audio bitrate of the X'th audio track."))
        ret.Add(New Macro("audio_channelsX", "Audio Channels X", GetType(Integer), "Audio channels of the X'th audio track."))
        ret.Add(New Macro("audio_codecX", "Audio Codec X", GetType(String), "Audio codec of the X'th audio track."))
        ret.Add(New Macro("audio_delayX", "Audio Delay X", GetType(Integer), "Audio delay of the X'th audio track."))
        ret.Add(New Macro("audio_fileX", "X'th Audio File", GetType(String), "File path of the X'th audio file."))
        ret.Add(New Macro("compressibility", "Compressibility", GetType(Integer), "Compressibility value."))
        ret.Add(New Macro("crop_bottom", "Crop Bottom", GetType(Integer), "Bottom crop value."))
        ret.Add(New Macro("crop_height", "Crop Height", GetType(Integer), "Crop height."))
        ret.Add(New Macro("crop_left", "Crop Left", GetType(Integer), "Left crop value."))
        ret.Add(New Macro("crop_right", "Crop Right", GetType(Integer), "Right crop value."))
        ret.Add(New Macro("crop_top", "Crop Top", GetType(Integer), "Top crop value."))
        ret.Add(New Macro("crop_width", "Crop Width", GetType(Integer), "Crop width."))
        ret.Add(New Macro("current_date", "Current Date", GetType(String), "Returns the current date."))
        ret.Add(New Macro("current_time", "Current Time (12h)", GetType(String), "Returns the current time (12h)."))
        ret.Add(New Macro("current_time24", "Current Time (24h)", GetType(String), "Returns the current time (24h)."))
        ret.Add(New Macro("dpi", "Main Dialog DPI", GetType(Integer), "DPI value of the main dialog."))
        ret.Add(New Macro("empty", "Empty/Nothing", GetType(String), "Empty character for special use like removing transferred values."))
        ret.Add(New Macro("encoder", "Encoder", GetType(String), "Name of the active video encoder."))
        ret.Add(New Macro("encoder_codec", "Encoder Codec", GetType(String), "Codec that is used by the active video encoder."))
        ret.Add(New Macro("encoder_ext", "Encoder File Extension", GetType(String), "File extension of the format the encoder of the active project outputs."))
        ret.Add(New Macro("encoder_out_file", "Encoder Output File", GetType(String), "Output file of the video encoder."))
        ret.Add(New Macro("encoder_profile", "Encoder Profile", GetType(String), "Name of the selected video encoder profile name."))
        ret.Add(New Macro("encoder_settings", "Encoder Settings", GetType(String), "Settings of the active video encoder."))
        ret.Add(New Macro("muxer_ext", "Muxer Extension", GetType(String), "Output extension of the active muxer."))
        ret.Add(New Macro("jobs", "Jobs", GetType(Integer), "Number of all jobs in Jobs List."))
        ret.Add(New Macro("jobs_active", "Jobs Active", GetType(Integer), "Number of active jobs in Jobs List."))
        ret.Add(New Macro("player", "Player", GetType(String), "Path of the media player."))
        ret.Add(New Macro("plugin_dir", "Plugin Directory", GetType(String), "AviSynth/VapourSynth plugin auto load directory."))
        ret.Add(New Macro("pos_frame", "Position In Frames", GetType(Integer), "Current preview position in frames."))
        ret.Add(New Macro("pos_ms", "Position In Milliseconds", GetType(Integer), "Current preview position in milliseconds."))
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
        ret.Add(New Macro("source_framerate", "Source Framerate", GetType(Integer), "Frame rate returned by the Source filter section with up to 6 decimal places, depending on digits, with no trailing zeros."))
        ret.Add(New Macro("source_framerate6", "Source Framerate (6 fixed decimal places)", GetType(Integer), "Frame rate returned by the Source filter section with 6 fixed decimal places."))
        ret.Add(New Macro("source_frames", "Source Frames", GetType(Integer), "Length in frames of the source video."))
        ret.Add(New Macro("source_height", "Source Image Height", GetType(Integer), "Image height of the source video."))
        ret.Add(New Macro("source_name", "Source Filename Without Extension", GetType(String), "The name of the source file without file extension."))
        ret.Add(New Macro("source_par_x", "Source Pixel Aspect Ratio X", GetType(String), "Source pixel/sample aspect ratio."))
        ret.Add(New Macro("source_par_y", "Source Pixel Aspect Ratio Y", GetType(String), "Source pixel/sample aspect ratio."))
        ret.Add(New Macro("source_seconds", "Source Seconds", GetType(Integer), "Length in seconds of the source video."))
        ret.Add(New Macro("source_temp_file", "Source Temp File", GetType(String), "File located in the temp directory using the same name as the source file."))
        ret.Add(New Macro("source_width", "Source Image Width", GetType(Integer), "Image width of the source video."))
        ret.Add(New Macro("source_mi_v:Format", "Source Video Codec", GetType(String), "Video codec of the source file."))
        ret.Add(New Macro("source_mi_vc", "Source Video Track Count", GetType(Integer), "Video track count of the source video."))
        ret.Add(New Macro("source_mi_ac", "Source Audio Track Count", GetType(Integer), "Audio track count of the source video."))
        ret.Add(New Macro("source_mi_tc", "Source Text Track Count", GetType(Integer), "Text track count of the source video."))
        ret.Add(New Macro("startup_dir", "Startup Directory", GetType(String), "Directory of the application."))
        ret.Add(New Macro("system_dir", "System Directory", GetType(String), "System directory."))
        ret.Add(New Macro("target_dar", "Target Display Aspect Ratio", GetType(String), "Target display aspect ratio."))
        ret.Add(New Macro("target_dir", "Target Directory", GetType(String), "Directory of the target file."))
        ret.Add(New Macro("target_file", "Target File Path", GetType(String), "File path of the target file."))
        ret.Add(New Macro("target_framerate", "Target Framerate", GetType(Integer), "Frame rate of the target video with up to 6 decimal places, depending on digits, with no trailing zeros."))
        ret.Add(New Macro("target_framerate6", "Target Framerate (6 fixed decimal places)", GetType(Integer), "Frame rate of the target video with 6 fixed decimal places."))
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
        ret.Add(New Macro("video_bitrate", "Video Bitrate", GetType(Integer), "Video bitrate in Kbps."))
        ret.Add(New Macro("working_dir", "Working Directory", GetType(String), "Directory of the source file or the temp directory if enabled."))

        ret.Sort()

        Return ret
    End Function

    Shared Function ExpandGUI(value As String, Optional throwIfCancel As Boolean = False) As (Value As String, Caption As String, Cancel As Boolean)
        Dim ret As (Value As String, Caption As String, Cancel As Boolean) = (value, "", False)
        If ret.Value = "" Then Return ret

        If ret.Value.Contains("$browse_file$") Then
            Using dialog As New OpenFileDialog
                ret.Cancel = dialog.ShowDialog <> DialogResult.OK

                If ret.Cancel Then
                    If throwIfCancel Then Throw New AbortException
                    Return ret
                Else
                    ret.Value = ret.Value.Replace("$browse_file$", dialog.FileName)
                End If
            End Using
        End If

        If ret.Value.Contains("$enter_text$") Then
            Dim inputText = InputBox.Show("Please enter text/value")

            If inputText = "" Then
                ret.Cancel = True
                Return ret
            Else
                ret.Value = ret.Value.Replace("$enter_text$", inputText)
            End If
        End If

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

        If ret.Value.Contains("$select:") Then
            Dim matches = Regex.Matches(ret.Value, "\$select:(.+?)\$")

            For Each iMatch As Match In matches
                Dim items = iMatch.Groups(1).Value.SplitNoEmpty(";").ToList

                If items.Count > 0 Then
                    Using td As New TaskDialog(Of String)
                        If items?(0)?.StartsWith("msg:") Then
                            td.Title = items(0).Substring(4)
                            items.RemoveAt(0)
                        Else
                            td.Title = "Please select one of the options."
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
        Return Expand(value, p)
    End Function

    Shared Function Expand(value As String, proj As Project) As String
        If value = "" Then Return ""
        If proj Is Nothing Then Return ""

        Dim matches As MatchCollection = Nothing

        If value.Contains("%empty%") Then value = value.Replace("%empty%", "")
        If Not value.Contains("%") Then Return value

        If value.Contains("%current_date%") Then value = value.Replace("%current_date%", Date.Now.ToString("yyyy-MM-dd"))
        If value.Contains("%current_time%") Then value = value.Replace("%current_time%", Date.Now.ToString("hh-mm-ss"))
        If value.Contains("%current_time24%") Then value = value.Replace("%current_time24%", Date.Now.ToString("HH-mm-ss"))
        If value.Contains("%source_file%") Then value = value.Replace("%source_file%", proj.SourceFile)
        If value.Contains("%working_dir%") Then value = value.Replace("%working_dir%", proj.TempDir)
        If value.Contains("%temp_dir%") Then value = value.Replace("%temp_dir%", proj.TempDir)
        If value.Contains("%temp_file%") Then value = value.Replace("%temp_file%", Path.Combine(proj.TempDir, proj.SourceFile.Base))
        If value.Contains("%source_temp_file%") Then value = value.Replace("%source_temp_file%", Path.Combine(proj.TempDir, g.GetSourceBase))
        If value.Contains("%target_temp_file%") Then value = value.Replace("%target_temp_file%", Path.Combine(proj.TempDir, proj.TargetFile.Base))
        If value.Contains("%source_name%") Then value = value.Replace("%source_name%", proj.SourceFile.Base)
        If value.Contains("%source_ext%") Then value = value.Replace("%source_ext%", proj.FirstOriginalSourceFile.Ext)
        If value.Contains("%version%") Then value = value.Replace("%version%", Application.ProductVersion)
        If value.Contains("%source_width%") Then value = value.Replace("%source_width%", proj.SourceWidth.ToString)
        If value.Contains("%source_height%") Then value = value.Replace("%source_height%", proj.SourceHeight.ToString)
        If value.Contains("%source_seconds%") Then value = value.Replace("%source_seconds%", proj.SourceSeconds.ToString)
        If value.Contains("%source_frames%") Then value = value.Replace("%source_frames%", proj.SourceFrames.ToString)
        If value.Contains("%source_framerate%") Then value = value.Replace("%source_framerate%", proj.SourceFrameRate.ToString("0.######", CultureInfo.InvariantCulture))
        If value.Contains("%source_framerate6%") Then value = value.Replace("%source_framerate6%", proj.SourceFrameRate.ToString("f6", CultureInfo.InvariantCulture))
        If value.Contains("%source_dir%") Then value = value.Replace("%source_dir%", proj.SourceFile.Dir)
        If value.Contains("%source_dir_parent%") Then value = value.Replace("%source_dir_parent%", proj.SourceFile.Dir.Parent)
        If value.Contains("%source_dir_name%") Then value = value.Replace("%source_dir_name%", proj.SourceFile.Dir.DirName)

        If value.Contains("%source_mi_") Then
            Dim miFile = If(proj.FirstOriginalSourceFile.FileExists(), proj.FirstOriginalSourceFile, proj.SourceFile)

            If value.Contains("%source_mi_v:Format%") Then value = value.Replace("%source_mi_v:Format%", MediaInfo.GetVideo(miFile, "Format"))

            If value.Contains("%source_mi_vc%") Then value = value.Replace("%source_mi_vc%", If(miFile.FileExists(), MediaInfo.GetVideoCount(miFile).ToInvariantString(), ""))
            If value.Contains("%source_mi_ac%") Then value = value.Replace("%source_mi_ac%", If(miFile.FileExists(), MediaInfo.GetAudioCount(miFile).ToInvariantString(), ""))
            If value.Contains("%source_mi_tc%") Then value = value.Replace("%source_mi_tc%", If(miFile.FileExists(), MediaInfo.GetSubtitleCount(miFile).ToInvariantString(), ""))

            If value.Contains("%source_mi_") Then
                For Each match As Match In Regex.Matches(value, "%source_mi_g:(.+?)%")
                    value = value.Replace(match.Value, MediaInfo.GetGeneral(miFile, match.Groups(1).Value))
                Next
                For Each match As Match In Regex.Matches(value, "%source_mi_v(\d*):(.+?)%")
                    value = value.Replace(match.Value, MediaInfo.GetVideo(miFile, If(match.Groups(1).Success, match.Groups(1).Value.ToInt(), 0), match.Groups(2).Value))
                Next
                For Each match As Match In Regex.Matches(value, "%source_mi_a(\d*):(.+?)%")
                    value = value.Replace(match.Value, MediaInfo.GetAudio(miFile, If(match.Groups(1).Success, match.Groups(1).Value.ToInt(), 0), match.Groups(2).Value))
                Next
                For Each match As Match In Regex.Matches(value, "%source_mi_t(\d*):(.+?)%")
                    value = value.Replace(match.Value, MediaInfo.GetText(miFile, If(match.Groups(1).Success, match.Groups(1).Value.ToInt(), 0), match.Groups(2).Value))
                Next
            End If
        End If

        If value.Contains("%target_width%") Then value = value.Replace("%target_width%", proj.TargetWidth.ToString)
        If value.Contains("%target_height%") Then value = value.Replace("%target_height%", proj.TargetHeight.ToString)
        If value.Contains("%target_seconds%") Then value = value.Replace("%target_seconds%", proj.TargetSeconds.ToString)
        If value.Contains("%target_frames%") Then value = value.Replace("%target_frames%", proj.Script.Info.FrameCount.ToString)
        If value.Contains("%target_framerate%") Then value = value.Replace("%target_framerate%", proj.Script.GetCachedFrameRate.ToString("0.######", CultureInfo.InvariantCulture))
        If value.Contains("%target_framerate6%") Then value = value.Replace("%target_framerate6%", proj.Script.GetCachedFrameRate.ToString("f6", CultureInfo.InvariantCulture))
        If value.Contains("%target_size%") Then value = value.Replace("%target_size%", (proj.TargetSize * SizePrefix.Base).ToString)
        If value.Contains("%target_file%") Then value = value.Replace("%target_file%", proj.TargetFile)
        If value.Contains("%target_dir%") Then value = value.Replace("%target_dir%", proj.TargetFile.Dir)
        If value.Contains("%target_name%") Then value = value.Replace("%target_name%", proj.TargetFile.Base)
        If value.Contains("%crop_width%") Then value = value.Replace("%crop_width%", (proj.SourceWidth - proj.CropLeft - proj.CropRight).ToString)
        If value.Contains("%crop_height%") Then value = value.Replace("%crop_height%", (proj.SourceHeight - proj.CropTop - proj.CropBottom).ToString)
        If value.Contains("%crop_left%") Then value = value.Replace("%crop_left%", proj.CropLeft.ToString)
        If value.Contains("%crop_top%") Then value = value.Replace("%crop_top%", proj.CropTop.ToString)
        If value.Contains("%crop_right%") Then value = value.Replace("%crop_right%", proj.CropRight.ToString)
        If value.Contains("%crop_bottom%") Then value = value.Replace("%crop_bottom%", proj.CropBottom.ToString)
        If value.Contains("%video_bitrate%") Then value = value.Replace("%video_bitrate%", proj.VideoBitrate.ToString)
        If value.Contains("%audio_bitrate%") Then value = value.Replace("%audio_bitrate%", proj.AudioTracks.Sum(Function(x) x.AudioProfile.Bitrate).ToString)

        matches = Regex.Matches(value, "%audio_bitrate(\d+)?%")
        For Each match As Match In matches
            Select Case match.Groups.Count
                Case 1
                    value = value.Replace(match.Value, proj.AudioTracks.Sum(Function(x) x.AudioProfile.Bitrate).ToString)
                Case 2
                    Dim track = match.Groups(1).Value.ToInt() - 1
                    If track < proj.AudioTracks.Count Then
                        value = value.Replace(match.Value, proj.AudioTracks(track).AudioProfile.Bitrate.ToString)
                    End If
                Case Else
                    Throw New NotImplementedException("Macro %audio_bitrate%")
            End Select
        Next

        matches = Regex.Matches(value, "%audio_channels(\d+)%")
        For Each match As Match In matches
            Dim track = match.Groups(1).Value.ToInt() - 1
            If track < proj.AudioTracks.Count Then
                value = value.Replace(match.Value, proj.AudioTracks(track).AudioProfile.Channels.ToString)
            End If
        Next

        matches = Regex.Matches(value, "%audio_codec(\d+)%")
        For Each match As Match In matches
            Dim track = match.Groups(1).Value.ToInt() - 1
            If track < proj.AudioTracks.Count Then
                value = value.Replace(match.Value, proj.AudioTracks(track).AudioProfile.AudioCodec.ToString)
            End If
        Next

        matches = Regex.Matches(value, "%audio_delay(\d+)%")
        For Each match As Match In matches
            Dim track = match.Groups(1).Value.ToInt() - 1
            If track < proj.AudioTracks.Count Then
                value = value.Replace(match.Value, proj.AudioTracks(track).AudioProfile.Delay.ToString)
            End If
        Next

        matches = Regex.Matches(value, "%audio_file(\d+)%")
        For Each match As Match In matches
            Dim track = match.Groups(1).Value.ToInt() - 1
            If track < proj.AudioTracks.Count Then
                value = value.Replace(match.Value, proj.AudioTracks(track).AudioProfile.File)
            End If
        Next

        If value.Contains("%startup_dir%") Then value = value.Replace("%startup_dir%", Folder.Startup)
        If value.Contains("%system_dir%") Then value = value.Replace("%system_dir%", Folder.System)
        If value.Contains("%script_dir%") Then value = value.Replace("%script_dir%", Folder.Scripts)
        If value.Contains("%programs_dir%") Then value = value.Replace("%programs_dir%", Folder.Programs)
        If value.Contains("%plugin_dir%") Then value = value.Replace("%plugin_dir%", Folder.Plugins)
        If value.Contains("%source_files_comma%") Then value = value.Replace("%source_files_comma%", """" + String.Join(""",""", proj.SourceFiles.ToArray) + """")
        If value.Contains("%source_files%") Then value = value.Replace("%source_files%", """" + String.Join(""" """, proj.SourceFiles.ToArray) + """")
        If value.Contains("%compressibility%") Then value = value.Replace("%compressibility%", Math.Round(proj.Compressibility, 3).ToString.Replace(",", "."))
        If value.Contains("%encoder%") Then value = value.Replace("%encoder%", TryCast(proj.VideoEncoder, BasicVideoEncoder)?.CommandLineParams.GetPackage.Name)
        If value.Contains("%encoder_profile%") Then value = value.Replace("%encoder_profile%", proj.VideoEncoder.Name)
        If value.Contains("%encoder_settings%") Then value = value.Replace("%encoder_settings%", TryCast(proj.VideoEncoder, BasicVideoEncoder)?.GetCommandLine(False, True).Replace("--", ""))
        If value.Contains("%encoder_out_file%") Then value = value.Replace("%encoder_out_file%", proj.VideoEncoder.OutputPath)
        If value.Contains("%encoder_ext%") Then value = value.Replace("%encoder_ext%", proj.VideoEncoder.OutputExt)
        If value.Contains("%encoder_codec%") Then value = value.Replace("%encoder_codec%", proj.VideoEncoder.Codec)
        If value.Contains("%muxer_ext%") Then value = value.Replace("%muxer_ext%", proj.VideoEncoder.Muxer.OutputExt)
        If value.Contains("%script_ext%") Then value = value.Replace("%script_ext%", proj.Script.FileType)
        If value.Contains("%pos_frame%") Then value = value.Replace("%pos_frame%", s.LastPosition.ToString)
        If value.Contains("%template_name%") Then value = value.Replace("%template_name%", proj.TemplateName)
        If value.Contains("%settings_dir%") Then value = value.Replace("%settings_dir%", Folder.Settings)
        If value.Contains("%player%") Then value = value.Replace("%player%", Package.mpvnet.Path)
        If value.Contains("%text_editor%") Then value = value.Replace("%text_editor%", g.GetTextEditorPath)
        If value.Contains("%processing%") Then value = value.Replace("%processing%", g.IsJobProcessing.ToString)
        If value.Contains("%dpi%") Then value = value.Replace("%dpi%", g.DPI.ToString())
        If value.Contains("%script_file%") Then value = value.Replace("%script_file%", proj.Script.Path)
        If value.Contains("%pos_ms%") Then value = value.Replace("%pos_ms%", g.GetPreviewPosMS.ToString)
        If value.Contains("%hdr10plus_path%") Then value = value.Replace("%hdr10plus_path%", p.Hdr10PlusMetadataFile)
        If value.Contains("%hdrdv_path%") Then value = value.Replace("%hdrdv_path%", p.HdrDolbyVisionMetadataFile?.Path)

        Dim jobs As List(Of Job)
        Dim getJobs = Function() As List(Of Job)
                            If jobs Is Nothing Then jobs = JobManager.GetJobs()
                            Return jobs
                        End Function
        If value.Contains("%jobs%") Then value = value.Replace("%jobs%", getJobs().Count.ToString())
        If value.Contains("%jobs_active%") Then value = value.Replace("%jobs_active%", getJobs().AsEnumerable().Count(Function(x) x.Active).ToString())

        Dim sourcePar As Point
        Dim getSourcePar = Function() As Point
            If sourcePar = Point.Empty Then sourcePar = Calc.GetSourcePAR()
            Return sourcePar
        End Function
        If value.Contains("%source_par_x%") Then value = value.Replace("%source_par_x%", getSourcePar().X.ToString)
        If value.Contains("%source_par_y%") Then value = value.Replace("%source_par_y%", getSourcePar().Y.ToString)

        Dim targetPar As Point
        Dim getTargetPar = Function() As Point
            If targetPar = Point.Empty Then targetPar = Calc.GetTargetPAR()
            Return targetPar
        End Function
        If value.Contains("%target_par_x%") Then value = value.Replace("%target_par_x%", getTargetPar().X.ToString)
        If value.Contains("%target_par_y%") Then value = value.Replace("%target_par_y%", getTargetPar().Y.ToString)

        If value.Contains("%source_dar%") Then value = value.Replace("%source_dar%", Calc.GetSourceDAR.ToString("f9", CultureInfo.InvariantCulture))
        If value.Contains("%target_dar%") Then value = value.Replace("%target_dar%", Calc.GetTargetDAR.ToString("f9", CultureInfo.InvariantCulture))

        If value.Contains("%sel_") Then
            If proj.Ranges.Count > 0 Then
                If value.Contains("%sel_start%") Then
                    value = value.Replace("%sel_start%", proj.Ranges(0).Start.ToString)
                End If

                If value.Contains("%sel_end%") Then
                    value = value.Replace("%sel_end%", proj.Ranges(0).End.ToString)
                End If
            Else
                If value.Contains("%sel_start%") Then
                    value = value.Replace("%sel_start%", "0")
                End If

                If value.Contains("%sel_end%") Then
                    value = value.Replace("%sel_end%", "0")
                End If
            End If
        End If
        If value.Contains("%app:") Then
            Dim mc = Regex.Matches(value, "%app:(.+?)%")

            For Each match As Match In mc
                Dim pack = Package.Items.Values.FirstOrDefault(
                    Function(pack2) pack2.Name.ToLowerInvariant = match.Groups(1).Value.ToLowerInvariant)

                Dim path = pack?.Path

                If path <> "" Then
                    value = value.Replace(match.Value, path)

                    If Not value.Contains("%") Then
                        Return value
                    End If
                End If
            Next
        End If

        If value.Contains("%app_path:") Then
            Dim mc = Regex.Matches(value, "%app_path:(.+?)%")

            For Each match As Match In mc
                Dim pack = Package.Items.Values.FirstOrDefault(
                    Function(pack2) pack2.Name.ToLowerInvariant = match.Groups(1).Value.ToLowerInvariant)

                Dim path = pack?.Path

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
                Dim pack = Package.Items.Values.FirstOrDefault(
                    Function(pack2) pack2.Name.ToLowerInvariant = match.Groups(1).Value.ToLowerInvariant)

                Dim path = pack?.Path

                If path <> "" Then
                    value = value.Replace(match.Value, path.Dir)

                    If Not value.Contains("%") Then
                        Return value
                    End If
                End If
            Next
        End If

        If value.Contains("%app_version:") Then
            For Each match As Match In Regex.Matches(value, "%app_version:(.+?)%")
                Dim pack = Package.Items.Values.FirstOrDefault(
                    Function(pack2) pack2.Name.ToLowerInvariant = match.Groups(1).Value.ToLowerInvariant)

                Dim version = pack?.Version

                If version <> "" Then
                    value = value.Replace(match.Value, version)

                    If Not value.Contains("%") Then
                        Return value
                    End If
                End If
            Next
        End If

        If value.Contains("%random:") Then
            For Each i As Match In Regex.Matches(value, "%random(?::(\d+))?%")
                Dim digits = 6
                If Integer.TryParse(i.Groups(1).Value, digits) Then
                    digits = If(digits < 1, 1, digits)
                    digits = If(digits > 10, 10, digits)
                End If
                Dim random = New Random()
                Dim randomInt = random.Next(0, Enumerable.Repeat(10, digits).Aggregate(1, Function(a, b) a * b))
                value = value.Replace(i.Value, randomInt.ToString().PadLeft(digits, "0"c))

                If Not value.Contains("%") Then
                    Return value
                End If
            Next
        End If

        If value.Contains("%filter:") Then
            Dim mc = Regex.Matches(value, "%filter:(.+?)%")

            For Each i As Match In mc
                For Each i2 In proj.Script.Filters
                    If i2.Active AndAlso i2.Path.ToUpperInvariant = i.Groups(1).Value.ToUpperInvariant Then
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
                matches = Regex.Matches(value, "%eval:(.+?)%")

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

    Shared Function ExpandWhileProcessing(value As String, proj As Project, commandline As String, progress As Single, progressline As String) As String
        If value = "" Then Return ""
        If proj Is Nothing Then Return ""

        value = Expand(value, proj)

        If Not value.Contains("%") Then Return value
        If value.Contains("%commandline%") Then value = value.Replace("%commandline%", commandline.Trim().Escape())
        If value.Contains("%progress%") Then value = value.Replace("%progress%", progress.ToInvariantString("0.0"))
        If value.Contains("%progressline%") Then value = value.Replace("%progressline%", progressline.Trim())

        Return value
    End Function

    Shared Function ExpandParamValues(value As String, params As IEnumerable(Of CommandLineParam), Optional proj As Project = Nothing) As String
        If value = "" Then Return ""
        If Not value.ContainsAny("%") Then Return value
        If params Is Nothing Then Return value
        If Not params.Any() Then Return value
        If proj Is Nothing Then proj = p

        For Each param In params.Where(Function(x) x.Path <> "" AndAlso x.Visible)
            Dim switches = param.GetSwitches().Select(Function(x) $"{x.ToLowerEx()}")

            For Each sw In switches
                If value.Contains(sw) Then
                    If TypeOf param Is BoolParam Then
                        Dim castedParam = DirectCast(param, BoolParam)
                        Dim v = ""
                        If castedParam.ArgsFunc Is Nothing Then
                            v = castedParam.Value.ToInvariantString()
                        Else
                            v = castedParam.ArgsFunc.Invoke()
                            If v = "" Then
                                v = castedParam.Value.ToString()
                            End If
                            v = v.ReplaceAll(switches, "").Replace(" ", "")
                        End If
                        If value.Contains($"%{sw}%") Then value = value.Replace($"%{sw}%", v)
                    ElseIf TypeOf param Is NumParam Then
                        Dim castedParam = DirectCast(param, NumParam)
                        Dim v = ""
                        If castedParam.ArgsFunc Is Nothing Then
                            v = castedParam.Value.ToInvariantString()
                        Else
                            v = castedParam.ArgsFunc.Invoke()
                            If v = "" Then
                                v = castedParam.Value.ToString()
                            End If
                            v = v.ReplaceAll(switches, "").Replace(" ", "")
                        End If
                        If value.Contains($"%{sw}%") Then value = value.Replace($"%{sw}%", v)
                    ElseIf TypeOf param Is OptionParam Then
                        Dim castedParam = DirectCast(param, OptionParam)
                        Dim v = ""
                        If castedParam.ArgsFunc Is Nothing Then
                            If castedParam.Values.NothingOrEmpty() Then
                                If castedParam.IntegerValue Then
                                    v = castedParam.Value.ToInvariantString()
                                Else 
                                    v = castedParam.OptionText.Replace(" ", "")
                                End If
                            Else
                                v = castedParam.ValueText
                            End If
                        Else
                            v = castedParam.ArgsFunc.Invoke()
                            If v = "" Then
                                v = castedParam.Value.ToString()
                            End If
                            v = v.ReplaceAll(switches, "").Replace(" ", "")
                        End If
                        If value.Contains($"%{sw}%") Then value = value.Replace($"%{sw}%", v.ToInvariantString())
                    ElseIf TypeOf param Is StringParam Then
                        Dim castedParam = DirectCast(param, StringParam)
                        Dim v = ""
                        If castedParam.ArgsFunc Is Nothing Then
                            v = castedParam.Value.ToInvariantString()
                        Else
                            v = castedParam.ArgsFunc.Invoke()
                            If v = "" Then
                                v = castedParam.Value.ToInvariantString()
                            End If
                            v = v.ReplaceAll(switches, "").Replace(" ", "")
                        End If
                        If value.Contains($"%{sw}%") Then value = value.Replace($"%{sw}%", v)
                    End If
                    Exit For
                End If
            Next

            value = value.ReplaceInvalidFileSystemName("-"c)
            If Not value.ContainsAny("%") Then Return value
        Next

        value = Expand(value, proj)
        Return value
    End Function
End Class
