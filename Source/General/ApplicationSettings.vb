
Imports StaxRip.UI

<Serializable()>
Public Class ApplicationSettings
    Implements ISafeSerialization

    Public AllowCustomPathsInStartupFolder As Boolean
    Public AllowToolsWithWrongVersion As Boolean
    Public AudioProfiles As List(Of AudioProfile)
    Public AutoSaveProject As Boolean
    Public AviSynthFilterPreferences As StringPairList
    Public AviSynthMode As FrameServerMode
    Public AviSynthProfiles As List(Of FilterCategory)
    Public BinaryPrefix As Boolean = False
    Public CheckForUpdates As Boolean
    Public CheckForUpdatesDismissed As String
    Public CheckForUpdatesLastRequest As DateTime
    Public CheckForUpdatesQuestion As Boolean
    Public CmdlPresetsEac3to As String
    Public CmdlPresetsMKV As String
    Public CmdlPresetsMP4 As String
    Public CmdlPresetsX264 As String
    Public CodeFont As String = "Consolas"
    Public CommandLineHighlighting As Boolean = True
    Public CommandLinePreviewMouseUpSearch As Boolean = True
    Public CommandLinePreviewViaCodeForm As Boolean = True    
    Public CommandLinePreview As CommandLinePreview = CommandLinePreview.CodePreview
    Public CommandLinePreviewWithLineNumbers As Boolean = True
    Public CropColor As Color
    Public CropFrameCount As Integer
    Public CustomMenuCodeEditor As CustomMenuItem
    Public CustomMenuCrop As CustomMenuItem
    Public CustomMenuMainForm As CustomMenuItem
    Public CustomMenuPreview As CustomMenuItem
    Public CustomMenuSize As CustomMenuItem
    Public DarMenu As String
    Public DeleteTempFilesMode As DeleteMode
    Public Demuxers As List(Of Demuxer)
    Public eac3toProfiles As List(Of eac3toProfile)
    Public EnableTooltips As Boolean
    Public ErrorMessageExtendedByErr As Boolean = False
    Public EventCommands As List(Of EventCommand)
    Public ExpandPreviewWindow As Boolean = True
    Public ApplicationExitMode As ApplicationExitMode = ApplicationExitMode.Regular
    Public FilterSetupProfiles As List(Of TargetVideoScript)
    Public FixFrameRate As Boolean = True
    Public HidePreviewButtons As Boolean
    Public IconFile As String
    Public InvertCtrlKeyOnNextButton As Boolean = False
    Public InvertShiftKeyOnNextButton As Boolean = False
    Public LastPosition As Integer
    Public LastSourceDir As String
    Public LoadAviSynthPlugins As Boolean = True
    Public LoadVapourSynthPlugins As Boolean = True
    Public LogEventCommand As Boolean
    Public LogFileNum As Integer = 50
    Public MinimizeToTray As Boolean
    Public MinimumDiskSpace As Integer = 20
    Public MuxerProfiles As List(Of Muxer)
    Public OutputHighlighting As Boolean = True
    Public PackagePaths As Dictionary(Of String, String)
    Public ParallelProcsNum As Integer = 3
    Public ParMenu As String
    Public PreferWindowsTerminal As Boolean = False
    Public PreventFocusStealAfter As Integer = 45
    Public PreventFocusStealUntil As Integer = -1
    Public PreventStandby As Boolean = True
    Public PreviewFormBorderStyle As FormBorderStyle
    Public PreviewSize As Integer = 70
    Public ProcessPriority As ProcessPriorityClass = ProcessPriorityClass.Idle
    Public ProgressReformatting As Boolean = True
    Public ProjectsMruNum As Integer = 10
    Public RecentFramePositions As List(Of String)
    Public RecentOptionsPage As String
    Public RecentProjects As List(Of String)
    Public ReverseVideoScrollDirection As Boolean
    Public ShowChangelog As String
    Public ShowPathsInCommandLine As Boolean
    Public ShowPreviewInfo As Boolean
    Public ShowTemplateSelection As Boolean
    Public ShowWindows7Warning As Boolean = True
    Public ShutdownForce As Boolean
    Public ShutdownTimeout As Integer = 90
    Public StartupTemplate As String
    Public Storage As ObjectStorage
    Public StringDictionary As Dictionary(Of String, String)
    Public StringList As List(Of String)
    Public TargetImageSizeMenu As String
    Public ThemeName As String
    Public ThumbnailBackgroundColor As Color = Color.AliceBlue
    Public UIFallback As Boolean = False
    Public UIScaleFactor As Single = 1.0F
    Public VapourSynthFilterPreferences As StringPairList
    Public VapourSynthMode As FrameServerMode
    Public VapourSynthProfiles As List(Of FilterCategory)
    Public VerifyToolStatus As Boolean = True
    Public Versions As Dictionary(Of String, Integer)
    Public VideoEncoderProfiles As List(Of VideoEncoder)
    Public WindowPositions As WindowPositions
    Public WindowPositionsRemembered As String()
    Public WriteDebugLog As Boolean
    Public X264QualityDefinitions As List(Of x264Control.QualityItem)
    Public X265QualityDefinitions As List(Of x265Control.QualityItem)
    Public VvencffappQualityDefinitions As List(Of VvencffappControl.QualityItem)

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
        Versions = If(Versions, New Dictionary(Of String, Integer))

        If Check(Storage, "Misc", 3) Then
            Storage = New ObjectStorage
        End If

        If Check(VideoEncoderProfiles, "Video Encoder Profiles", 202) Then
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

        If Check(AudioProfiles, "Audio Profiles", 117) Then
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

        If Check(Demuxers, "Demuxers", 110) Then
            Demuxers = Demuxer.GetDefaults()
        End If

        If Check(AviSynthFilterPreferences, "AviSynth Source Filter Preferences", 10) Then
            AviSynthFilterPreferences = New StringPairList From {
                {"*", "FFVideoSource"},
                {"*:VP9", "LWLibavVideoSource"},
                {"264 h264 avc", "LWLibavVideoSource"},
                {"265 h265 hevc hvc", "LWLibavVideoSource"},
                {"d2v", "MPEG2Source"},
                {"mp4 m4v mov", "LSMASHVideoSource"},
                {"ts m2ts mts m2t m2v", "LWLibavVideoSource"},
                {"vdr", "AviSource"},
                {"wmv", "DSS2"}
            }
        End If

        If Check(VapourSynthFilterPreferences, "VapourSynth Source Filter Preference", 10) Then
            VapourSynthFilterPreferences = New StringPairList From {
                {"*", "ffms2"},
                {"*:VP9", "LWLibavSource"},
                {"264 h264 avc", "LWLibavSource"},
                {"265 h265 hevc hvc", "LWLibavSource"},
                {"avs vdr", "AVISource"},
                {"d2v", "MPEG2Source"},
                {"mp4 m4v mov", "LibavSMASHSource"},
                {"ts m2ts mts m2t m2v", "LWLibavSource"}
            }
        End If

        If Check(eac3toProfiles, "eac3to Audio Stream Profiles", 5) Then
            eac3toProfiles = New List(Of eac3toProfile)
        End If

        If Check(EventCommands, "Event Commands", 5) Then
            EventCommands = New List(Of EventCommand)
        End If

        If Check(WindowPositionsRemembered, "Remembered Window Positions", 5) Then
            WindowPositionsRemembered = {"StaxRip", "Crop", "Jobs", "Processing", "Preview", "Code Preview", "Help"}
        End If

        If Check(WindowPositions, "Window Positions", 5) Then
            WindowPositions = New WindowPositions
        End If

        If Check(StartupTemplate, "Startup Template", 5) Then
            StartupTemplate = "Automatic Workflow"
        End If

        If PackagePaths Is Nothing Then
            PackagePaths = New Dictionary(Of String, String)
        End If

        If RecentProjects Is Nothing Then
            RecentProjects = New List(Of String)
        End If

        If Check(MuxerProfiles, "Container Profiles", 41) Then
            MuxerProfiles = New List(Of Muxer)
            MuxerProfiles.AddRange(Muxer.GetDefaults())
        End If

        If StringDictionary Is Nothing Then
            StringDictionary = New Dictionary(Of String, String)
        End If

        If RecentOptionsPage Is Nothing Then
            RecentOptionsPage = ""
        End If

        If Check(CmdlPresetsMKV, "MKV custom command line menu presets", 8) Then
            CmdlPresetsMKV = "File Attachment = --attach-file ""$browse_file$""" + BR +
                             "Attachment Description = --attachment-description ""$enter_text$""" + BR +
                             "Process Priority = --priority $select:lowest;lower;normal;higher;highest$"
        End If

        If Check(CmdlPresetsEac3to, "eac3to custom command line menu presets", 8) Then
            CmdlPresetsEac3to = GetDefaultEac3toMenu()
        End If

        If Check(CmdlPresetsMP4, "MP4 custom command line menu presets", 8) Then
            CmdlPresetsMP4 = "iPod = -ipod"
        End If

        If Check(CmdlPresetsX264, "x264 custom command line menu presets", 8) OrElse CmdlPresetsX264 = "" Then
            CmdlPresetsX264 = "SAR | PAL | 4:3 = --sar 12:11" + BR +
                              "SAR | PAL | 16:9 = --sar 16:11" + BR +
                              "SAR | NTSC | 4:3 = --sar 10:11" + BR +
                              "SAR | NTSC | 16:9 = --sar 40:33" + BR +
                              "Enter SAR = --sar $enter_text:Please enter the SAR.$" + BR +
                              "Stats = --stats ""%target_temp_file%.stats"""
        End If

        If Check(ParMenu, "Source PAR menu", 11) Then
            ParMenu = GetParMenu()
        End If

        If Check(DarMenu, "Source DAR menu", 11) Then
            DarMenu = GetDarMenu()
        End If

        If Check(TargetImageSizeMenu, "Target image size menu", 16) Then
            TargetImageSizeMenu = GetDefaultTargetImageSizeMenu()
        End If

        If StringList Is Nothing Then
            StringList = New List(Of String)
        End If

        If RecentFramePositions Is Nothing Then
            RecentFramePositions = New List(Of String)
        End If

        If CropFrameCount = 0 Then
            CropFrameCount = 15
        End If

        If Check(CustomMenuCrop, "Menu in crop dialog", 18) Then
            CustomMenuCrop = CropForm.GetDefaultMenuCrop
        End If

        If Check(CustomMenuMainForm, "Main menu in main window", 165) Then
            CustomMenuMainForm = MainForm.GetDefaultMainMenu
        End If

        If Check(CustomMenuPreview, "Menu in preview dialog", 55) Then
            CustomMenuPreview = PreviewForm.GetDefaultMenu()
        End If

        If Check(CustomMenuCodeEditor, "Menu in code editor", 24) Then
            CustomMenuCodeEditor = CodeEditor.GetDefaultMenu()
        End If

        If Check(CustomMenuSize, "Target size menu in main dialog", 32) Then
            CustomMenuSize = MainForm.GetDefaultMenuSize
        End If

        If Check(AviSynthProfiles, "AviSynth Filter Profiles", 155) Then
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

        If Check(VapourSynthProfiles, "VapourSynth Filter Profiles", 36) Then
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

        If Check(FilterSetupProfiles, "Filter Setup Profiles", 102) Then
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
Forced = force
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
        Return "7680 x auto = 7680 x 0
5760 x auto = 5760 x 0

3840 x auto = 3840 x 0
3200 x auto = 3200 x 0
2880 x auto = 2880 x 0
2560 x auto = 2560 x 0

1920 x auto = 1920 x 0
1600 x auto = 1600 x 0
1440 x auto = 1440 x 0
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
