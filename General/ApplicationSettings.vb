
Imports StaxRip.UI

<Serializable()>
Public Class ApplicationSettings
    Implements ISafeSerialization

    Public AllowCustomPathsInStartupFolder As Boolean
    Public AllowToolsWithWrongVersion As Boolean
    Public AudioProfiles As List(Of AudioProfile)
    Public AviSynthFilterPreferences As StringPairList
    Public AviSynthMode As FrameServerMode
    Public AviSynthProfiles As List(Of FilterCategory)
    Public CharacterLimitFolder As Integer = 100
    Public CharacterLimitFilename As Integer = 70
    Public CheckForUpdates As Boolean
    Public CheckForUpdatesBeta As Boolean
    Public CheckForUpdatesDismissed As String
    Public CheckForUpdatesLastRequest As DateTime
    Public CheckForUpdatesQuestion As Boolean
    Public CmdlPresetsEac3to As String
    Public CmdlPresetsMKV As String
    Public CmdlPresetsMP4 As String
    Public CmdlPresetsX264 As String
    Public ConvertChromaSubsampling As Boolean = True
    Public CropFrameCount As Integer
    Public CustomMenuCrop As CustomMenuItem
    Public CustomMenuMainForm As CustomMenuItem
    Public CustomMenuPreview As CustomMenuItem
    Public CustomMenuSize As CustomMenuItem
    Public DarMenu As String
    Public DeleteTempFilesMode As DeleteMode
    Public Demuxers As List(Of Demuxer)
    Public eac3toProfiles As List(Of eac3toProfile)
    Public EnableTooltips As Boolean
    Public EventCommands As List(Of EventCommand)
    Public FilterSetupProfiles As List(Of TargetVideoScript)
    Public HidePreviewButtons As Boolean
    Public IconFile As String
    Public LastPosition As Integer
    Public LastSourceDir As String
    Public LoadAviSynthPlugins As Boolean = True
    Public LoadVapourSynthPlugins As Boolean = True
    Public LogEventCommand As Boolean
    Public LogFileNum As Integer = 50
    Public MinimizeToTray As Boolean
    Public MinimumDiskSpace As Integer = 20
    Public MuxerProfiles As List(Of Muxer)
    Public PackagePaths As Dictionary(Of String, String)
    Public ParallelProcsNum As Integer = 2
    Public ParMenu As String
    Public PreventStandby As Boolean = True
    Public PreviewFormBorderStyle As FormBorderStyle
    Public PreviewSize As Integer = 70
    Public ProcessPriority As ProcessPriorityClass = ProcessPriorityClass.Idle
    Public ProjectsMruNum As Integer = 10
    Public RecentFramePositions As List(Of String)
    Public RecentOptionsPage As String
    Public RecentProjects As List(Of String)
    Public ReverseVideoScrollDirection As Boolean
    Public ShowPathsInCommandLine As Boolean
    Public ShowPreviewInfo As Boolean
    Public ShowTemplateSelection As Boolean
    Public ShutdownTimeout As Integer
    Public StartupTemplate As String
    Public Storage As ObjectStorage
    Public StringDictionary As Dictionary(Of String, String)
    Public StringList As List(Of String)
    Public TargetImageSizeMenu As String
    Public ThumbnailBackgroundColor As Color = Color.AliceBlue
    Public ToolStripRenderModeEx As ToolStripRenderModeEx = ToolStripRenderModeEx.SystemDefault
    Public UIScaleFactor As Single = 1
    Public VapourSynthFilterPreferences As StringPairList
    Public VapourSynthMode As FrameServerMode
    Public VapourSynthProfiles As List(Of FilterCategory)
    Public VerifyToolStatus As Boolean = True
    Public Versions As Dictionary(Of String, Integer)
    Public VideoEncoderProfiles As List(Of VideoEncoder)
    Public WindowPositions As WindowPositions
    Public WindowPositionsRemembered As String()
    Public WriteDebugLog As Boolean

    Property WasUpdated As Boolean Implements ISafeSerialization.WasUpdated

    ReadOnly Property VersionsProperty() As Dictionary(Of String, Integer) Implements ISafeSerialization.Versions
        Get
            Return Versions
        End Get
    End Property

    Function Check(obj As Object, key As String, version As Integer) As Boolean
        Return SafeSerialization.Check(Me, obj, key, version)
    End Function

    Sub Init() Implements ISafeSerialization.Init
        If Versions Is Nothing Then
            Versions = New Dictionary(Of String, Integer)
        End If

        If Check(Storage, "Misc", 2) Then
            Storage = New ObjectStorage
        End If

        If Check(VideoEncoderProfiles, "Video Encoder Profiles", 196) Then
            If VideoEncoderProfiles Is Nothing Then
                VideoEncoderProfiles = VideoEncoder.GetDefaults()
            Else
                Dim profiles As New List(Of VideoEncoder)

                Try
                    For Each i In VideoEncoderProfiles
                        If Not i.Name.Contains("Backup") Then
                            i.Name = "Backup | " + i.Name
                            profiles.Add(i)
                        End If
                    Next
                Catch
                End Try

                VideoEncoderProfiles = VideoEncoder.GetDefaults()
                VideoEncoderProfiles.AddRange(profiles)
            End If
        End If

        If Check(AudioProfiles, "Audio Profiles", 116) Then
            If AudioProfiles Is Nothing Then
                AudioProfiles = AudioProfile.GetDefaults()
            Else
                Dim l As New List(Of AudioProfile)

                For Each i In AudioProfiles
                    If Not i.Name.Contains("Backup") Then
                        i.Name = "Backup | " + i.Name
                        l.Add(i)
                    End If
                Next

                AudioProfiles = AudioProfile.GetDefaults()
                AudioProfiles.AddRange(l)
            End If
        End If

        If Check(Demuxers, "Demuxers", 108) Then
            Demuxers = Demuxer.GetDefaults()
        End If

        If Check(AviSynthFilterPreferences, "AviSynth Source Filter Preferences", 6) Then
            AviSynthFilterPreferences = New StringPairList
            AviSynthFilterPreferences.Add("default", "FFVideoSource")
            AviSynthFilterPreferences.Add("264 h264 avc", "LWLibavVideoSource")
            AviSynthFilterPreferences.Add("265 h265 hevc hvc", "LWLibavVideoSource")
            AviSynthFilterPreferences.Add("d2v", "MPEG2Source")
            AviSynthFilterPreferences.Add("mp4 m4v mov", "LSMASHVideoSource")
            AviSynthFilterPreferences.Add("ts m2ts mts m2t", "LWLibavVideoSource")
            AviSynthFilterPreferences.Add("wmv", "DSS2")
            AviSynthFilterPreferences.Add("vdr", "AviSource")
        End If

        If Check(VapourSynthFilterPreferences, "VapourSynth Source Filter Preference", 6) Then
            VapourSynthFilterPreferences = New StringPairList
            VapourSynthFilterPreferences.Add("default", "ffms2")
            VapourSynthFilterPreferences.Add("264 h264 avc", "LWLibavSource")
            VapourSynthFilterPreferences.Add("265 h265 hevc hvc", "LWLibavSource")
            VapourSynthFilterPreferences.Add("avs vdr", "AVISource")
            VapourSynthFilterPreferences.Add("mp4 m4v mov", "LibavSMASHSource")
            VapourSynthFilterPreferences.Add("ts m2ts mts m2t", "LWLibavSource")
            VapourSynthFilterPreferences.Add("d2v", "d2vsource")
        End If

        If Check(ToolStripRenderModeEx, "menu style", 1) Then
            ToolStripRenderModeEx = ToolStripRenderModeEx.SystemDefault
        End If

        If Check(eac3toProfiles, "eac3to Audio Stream Profiles", 4) Then
            eac3toProfiles = New List(Of eac3toProfile)
        End If

        If Check(EventCommands, "Event Commands", 0) Then
            EventCommands = New List(Of EventCommand)
        End If

        If Check(WindowPositionsRemembered, "Remembered Window Positions", 1) Then
            WindowPositionsRemembered = {"StaxRip", "Crop", "Preview", "Help"}
        End If

        If Check(WindowPositions, "Remembered Window Positions 2", 1) Then
            WindowPositions = New WindowPositions
        End If

        If Check(StartupTemplate, "Startup Template", 2) Then
            StartupTemplate = "Automatic Workflow"
        End If

        If PackagePaths Is Nothing Then
            PackagePaths = New Dictionary(Of String, String)
        End If

        If RecentProjects Is Nothing Then
            RecentProjects = New List(Of String)
        End If

        If Check(MuxerProfiles, "Container Profiles", 40) Then
            MuxerProfiles = New List(Of Muxer)
            MuxerProfiles.AddRange(Muxer.GetDefaults())
        End If

        If StringDictionary Is Nothing Then
            StringDictionary = New Dictionary(Of String, String)
        End If

        If RecentOptionsPage Is Nothing Then
            RecentOptionsPage = ""
        End If

        If Check(CmdlPresetsMKV, "MKV custom command line menu presets", 7) Then
            CmdlPresetsMKV = "File Attachment = --attach-file ""$browse_file$""" + BR +
                             "Attachment Description = --attachment-description ""$enter_text$""" + BR +
                             "Process Priority = --priority $select:lowest;lower;normal;higher;highest$"
        End If

        If Check(CmdlPresetsEac3to, "eac3to custom command line menu presets", 7) Then
            CmdlPresetsEac3to = GetDefaultEac3toMenu()
        End If

        If Check(CmdlPresetsMP4, "MP4 custom command line menu presets", 1) Then
            CmdlPresetsMP4 = "iPod = -ipod"
        End If

        If Check(CmdlPresetsX264, "x264 custom command line menu presets", 6) OrElse CmdlPresetsX264 = "" Then
            CmdlPresetsX264 = "SAR | PAL | 4:3 = --sar 12:11" + BR +
                              "SAR | PAL | 16:9 = --sar 16:11" + BR +
                              "SAR | NTSC | 4:3 = --sar 10:11" + BR +
                              "SAR | NTSC | 16:9 = --sar 40:33" + BR +
                              "Enter SAR = --sar $enter_text:Please enter the SAR.$" + BR +
                              "Stats = --stats ""%target_temp_file%.stats"""
        End If

        If Check(ParMenu, "Source PAR menu", 10) Then
            ParMenu = GetParMenu()
        End If

        If Check(DarMenu, "Source DAR menu", 10) Then
            DarMenu = GetDarMenu()
        End If

        If Check(TargetImageSizeMenu, "Target image size menu", 15) Then
            TargetImageSizeMenu = GetDefaultTargetImageSizeMenu()
        End If

        If StringList Is Nothing Then
            StringList = New List(Of String)
        End If

        If RecentFramePositions Is Nothing Then
            RecentFramePositions = New List(Of String)
        End If

        If CropFrameCount = 0 Then
            CropFrameCount = 10
        End If

        If Check(CustomMenuCrop, "Menu in crop dialog", 17) Then
            CustomMenuCrop = CropForm.GetDefaultMenuCrop
        End If

        If Check(CustomMenuMainForm, "Main menu in main window", 163) Then
            CustomMenuMainForm = MainForm.GetDefaultMainMenu
        End If

        If Check(CustomMenuPreview, "Menu in preview dialog", 54) Then
            CustomMenuPreview = PreviewForm.GetDefaultMenu()
        End If

        If Check(CustomMenuSize, "Target size menu in main dialog", 31) Then
            CustomMenuSize = MainForm.GetDefaultMenuSize
        End If

        If Check(AviSynthProfiles, "AviSynth Filter Profiles", 154) Then
            If AviSynthProfiles Is Nothing Then
                AviSynthProfiles = FilterCategory.GetAviSynthDefaults
            Else
                Dim current = AviSynthProfiles.SelectMany(Function(category) category.Filters)
                AviSynthProfiles = FilterCategory.GetAviSynthDefaults
                Dim defaults = AviSynthProfiles.SelectMany(Function(category) category.Filters)
                Dim defaultScripts = defaults.Select(Function(filter) filter.Script)
                Dim unknown = current.Where(Function(filter) Not defaultScripts.Contains(filter.Script))

                For Each i In unknown
                    If Not i.Path?.StartsWith("Backup | ") Then
                        i.Path = "Backup | " + i.Path
                    End If

                    FilterCategory.AddFilter(i, AviSynthProfiles)
                Next
            End If
        End If

        If Check(VapourSynthProfiles, "VapourSynth Filter Profiles", 35) Then
            If VapourSynthProfiles Is Nothing Then
                VapourSynthProfiles = FilterCategory.GetVapourSynthDefaults
            Else
                Dim current = VapourSynthProfiles.SelectMany(Function(category) category.Filters)
                VapourSynthProfiles = FilterCategory.GetVapourSynthDefaults
                Dim defaults = VapourSynthProfiles.SelectMany(Function(category) category.Filters)
                Dim defaultScripts = defaults.Select(Function(filter) filter.Script)
                Dim unknown = current.Where(Function(filter) Not defaultScripts.Contains(filter.Script))

                For Each i In unknown
                    If Not i.Path?.StartsWith("Backup | ") Then
                        i.Path = "Backup | " + i.Path
                    End If

                    FilterCategory.AddFilter(i, VapourSynthProfiles)
                Next
            End If
        End If

        If Check(FilterSetupProfiles, "Filter Setup Profiles", 101) Then
            FilterSetupProfiles = VideoScript.GetDefaults
        End If

        If LastSourceDir = "" Then
            LastSourceDir = ""
        End If

        Migrate()
    End Sub

    Sub Migrate()
        For Each ap In AudioProfiles
            ap.Migrate()
        Next
    End Sub

    Shared Function GetDarMenu() As String
        Dim ret =
"Automatic = auto

1.333333 = 4:3
1.777777 = 16:9

1.823361 PAL ITU = 1.823361
1.367521 PAL ITU = 1.367521

1.822784 NTSC ITU = 1.822784
1.367088 NTSC ITU = 1.367088

1.85 = 1.85
2.00 = 2.00
2.35 = 2.35
2.40 = 2.40

Custom... = $enter_text:Enter a custom Display Aspect Ratio.$"

        Return ret
    End Function

    Shared Function GetParMenu() As String
        Dim ret =
"Automatic = auto
1:1 = 1:1

PAL 1.33 16:15 = 16:15
PAL 1.36 12:11 = 12:11
PAL 1.77 64:45 = 64:45
PAL 1.82 16:11 = 16:11

NTSC 1.33 8:9   = 8:9
NTSC 1.36 10:11 = 10:11
NTSC 1.77 32:27 = 32:27
NTSC 1.82 40:33 = 40:33

Custom... = $enter_text:Enter a custom Pixel Aspect Ratio.$"

        Return ret
    End Function

    Shared Function GetDefaultTargetImageSizeMenu() As String
        Return "3840 x auto = 3840 x 0
3200 x auto = 3200 x 0
2560 x auto = 2560 x 0

1920 x auto = 1920 x 0
1280 x auto = 1280 x 0
960 x auto = 960 x 0

720 x auto = 720 x 0
640 x auto = 640 x 0
480 x auto = 480 x 0
320 x auto = 320 x 0"
    End Function

    Shared Function GetDefaultEac3toMenu() As String
        Return _
            "AAC 96 Kbps - 2ch - Normalize - 16bit = -down16 -downStereo -normalize -quality=0.3" + BR +
            "AAC 132 Kbps - 2ch - Normalize - 16bit = -down16 -downStereo -normalize -quality=0.4" + BR +
            "AAC 240 Kbps 5.1ch - Normalize - 16bit = -down16 -down6 -normalize -quality=0.3" + BR +
            "Normalize = -normalize" + BR +
            "Convert to 16 bit = -down16" + BR +
            "Extract DTS Core = -core" + BR +
            "Downmix | Multichannel to stereo = -downStereo" + BR +
            "Downmix | Multichannel to stereo (DPL II) = -downDpl" + BR +
            "Downmix | 7 or 8 channels to 6 channels = -down6" + BR +
            "Downmix | Mix LFE in (stereo downmixing) = -mixlfe" + BR +
            "AAC Quality | 0.10 = -quality=0.10" + BR +
            "AAC Quality | 0.15 = -quality=0.15" + BR +
            "AAC Quality | 0.20 = -quality=0.20" + BR +
            "AAC Quality | 0.25 = -quality=0.25" + BR +
            "AAC Quality | 0.30 = -quality=0.30" + BR +
            "AAC Quality | 0.35 = -quality=0.35" + BR +
            "AAC Quality | 0.40 = -quality=0.40" + BR +
            "AAC Quality | 0.45 = -quality=0.45" + BR +
            "AAC Quality | 0.50 = -quality=0.50" + BR +
            "AAC Quality | 0.55 = -quality=0.55" + BR +
            "AAC Quality | 0.60 = -quality=0.60" + BR +
            "AAC Quality | 0.65 = -quality=0.65" + BR +
            "AAC Quality | 0.70 = -quality=0.70" + BR +
            "AAC Quality | 0.75 = -quality=0.75" + BR +
            "AAC Quality | 0.80 = -quality=0.80" + BR +
            "AAC Quality | 0.85 = -quality=0.85" + BR +
            "AAC Quality | 0.90 = -quality=0.90" + BR +
            "AAC Quality | 0.95 = -quality=0.95" + BR +
            "AC3 Encoding | 192 = -192" + BR +
            "AC3 Encoding | 224 = -224" + BR +
            "AC3 Encoding | 384 = -384" + BR +
            "AC3 Encoding | 448 = -448" + BR +
            "AC3 Encoding | 640 = -640" + BR +
            "DTS Encoding | 768 = -768" + BR +
            "DTS Encoding | 1536 = -1536" + BR +
            "Resample | 44100 = -resampleTo44100" + BR +
            "Resample | 48000 = -resampleTo48000" + BR +
            "Resample | 88200 = -resampleTo88200" + BR +
            "Resample | 96000 = -resampleTo96000"
    End Function

    Sub UpdateRecentProjects(path As String)
        Dim skip = Not File.Exists(path) OrElse
            path.StartsWith(Folder.Template) OrElse
            path.EndsWith("recovery.srip") OrElse
            path.Ext = "bin"

        Dim list As New List(Of String)

        If Not skip Then
            list.Add(path)
        End If

        For Each i In s.RecentProjects
            If i <> path AndAlso File.Exists(i) Then
                list.Add(i)
            End If
        Next

        While list.Count > s.ProjectsMruNum
            list.RemoveAt(list.Count - 1)
        End While

        s.RecentProjects = list
    End Sub
End Class
