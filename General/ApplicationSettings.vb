Imports System.Globalization

Imports StaxRip.UI

<Serializable()>
Public Class ApplicationSettings
    Implements ISafeSerialization

    Public AviSynthFilterPreferences As StringPairList
    Public VapourSynthFilterPreferences As StringPairList
    Public eac3toProfiles As List(Of eac3toProfile)
    Public AudioProfiles As List(Of AudioProfile)
    Public AviSynthProfiles As List(Of FilterCategory)
    Public VapourSynthProfiles As List(Of FilterCategory)
    Public FilterSetupProfiles As List(Of TargetVideoScript)
    Public CmdlPresetsEac3to As String
    Public CmdlPresetsMKV As String
    Public CmdlPresetsMP4 As String
    Public CmdlPresetsX264 As String
    Public CustomMenuCrop As CustomMenuItem
    Public CustomMenuMainForm As CustomMenuItem
    Public CustomMenuPreview As CustomMenuItem
    Public CustomMenuSize As CustomMenuItem
    Public Demuxers As List(Of Demuxer)
    Public EnableTooltips As Boolean = True
    Public EventCommands As List(Of EventCommand)
    Public HidePreviewButtons As Boolean
    Public LastPosition As Integer
    Public LastSourceDirValue As String
    Public MuxerProfiles As List(Of Muxer)
    Public PackagePaths As Dictionary(Of String, String)
    Public PreviewToggleInfos As Boolean
    Public ProcessPriority As ProcessPriorityClass = ProcessPriorityClass.Idle
    Public RecentFramePositions As List(Of String)
    Public RecentOptionsPage As String
    Public RecentProjects As List(Of String)
    Public PreventStandby As Boolean = True
    Public ShowPathsInCommandLine As Boolean
    Public SourceAspectRatioMenu As String
    Public StartupTemplate As String
    Public Storage As ObjectStorage
    Public StringDictionary As Dictionary(Of String, String)
    Public StringList As List(Of String)
    Public TargetImageSizeMenu As String
    Public ToolStripRenderModeEx As ToolStripRenderModeEx
    Public Versions As Dictionary(Of String, Integer)
    Public VideoEncoderProfiles As List(Of VideoEncoder)
    Public WindowPositions As WindowPositions
    Public WindowPositionsCenterScreen As String()
    Public WindowPositionsRemembered As String()
    Public PreviewFormBorderStyle As FormBorderStyle
    Public DeleteTempFilesToRecycleBin As Boolean = True
    Public ShowTemplateSelection As Boolean
    Public UIScaleFactor As Single = 1
    Public PreventActivation As String
    Public MinimumDiskSpace As Integer = 20
    Public ReverseVideoScrollDirection As Boolean

    Property WasUpdated As Boolean Implements ISafeSerialization.WasUpdated

    ReadOnly Property VersionsProperty() As Dictionary(Of String, Integer) Implements ISafeSerialization.Versions
        Get
            Return Versions
        End Get
    End Property

    Private Function Check(obj As Object, key As String, version As Integer) As Boolean
        Return SafeSerialization.Check(Me, obj, key, version)
    End Function

    Sub Init() Implements ISafeSerialization.Init
        If Versions Is Nothing Then Versions = New Dictionary(Of String, Integer)
        If Check(Storage, "Misc", 2) Then Storage = New ObjectStorage

        If Check(VideoEncoderProfiles, "Video Encoder Profiles", 191) Then
            If VideoEncoderProfiles Is Nothing Then
                VideoEncoderProfiles = VideoEncoder.GetDefaults()
            Else
                Dim profiles As New List(Of VideoEncoder)

                For Each i In VideoEncoderProfiles
                    If Not i.Name.Contains("Backup") Then
                        i.Name = "Backup | " + i.Name
                        profiles.Add(i)
                    End If
                Next

                VideoEncoderProfiles = VideoEncoder.GetDefaults()
                VideoEncoderProfiles.AddRange(profiles)
            End If
        End If

        If Check(AudioProfiles, "Audio Profiles", 115) Then
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

        If Check(Demuxers, "Demuxers", 103) Then Demuxers = Demuxer.GetDefaults()

        If Check(AviSynthFilterPreferences, "AviSynth Filter Preferences", 2) Then
            AviSynthFilterPreferences = New StringPairList
            AviSynthFilterPreferences.Add("default", "FFVideoSource")
            AviSynthFilterPreferences.Add("264, h264, avc", "LWLibavVideoSource")
            AviSynthFilterPreferences.Add("265, h265, hevc", "LWLibavVideoSource")
            AviSynthFilterPreferences.Add("d2v", "MPEG2Source")
            AviSynthFilterPreferences.Add("dgi", "DGSource")
            AviSynthFilterPreferences.Add("dgim", "DGSourceIM")
            AviSynthFilterPreferences.Add("mp4, m4v, mov", "LSMASHVideoSource")
            AviSynthFilterPreferences.Add("ts, m2ts, mts, m2t", "LWLibavVideoSource")
            AviSynthFilterPreferences.Add("wmv", "DSS2")
        End If

        If Check(VapourSynthFilterPreferences, "VapourSynth Filter Preference", 3) Then
            VapourSynthFilterPreferences = New StringPairList
            VapourSynthFilterPreferences.Add("default", "ffms2")
            VapourSynthFilterPreferences.Add("264, h264, avc", "LWLibavSource")
            VapourSynthFilterPreferences.Add("265, h265, hevc", "LWLibavSource")
            VapourSynthFilterPreferences.Add("avi, avs", "AVISource")
            VapourSynthFilterPreferences.Add("mp4, m4v, mov", "LibavSMASHSource")
            VapourSynthFilterPreferences.Add("ts, m2ts, mts, m2t", "LWLibavSource")
            VapourSynthFilterPreferences.Add("d2v", "d2vsource")
        End If

        If PreventActivation = "" Then PreventActivation = "mpc vlc media play kodi"

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

        If WindowPositionsCenterScreen Is Nothing Then
            WindowPositionsCenterScreen = {}
        End If

        If Check(StartupTemplate, "Startup Template", 2) Then
            StartupTemplate = "x264"
        End If

        If PackagePaths Is Nothing Then
            PackagePaths = New Dictionary(Of String, String)
        End If

        If RecentProjects Is Nothing Then
            RecentProjects = New List(Of String)
        End If

        If Check(MuxerProfiles, "Container Profiles", 37) Then
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

        If Check(SourceAspectRatioMenu, "Source aspect ratio menu", 24) Then
            SourceAspectRatioMenu = GetDefaultSourceAspectRatioMenu()
        End If

        If Check(TargetImageSizeMenu, "Target image size menu", 13) Then
            TargetImageSizeMenu = GetDefaultTargetImageSizeMenu()
        End If

        If StringList Is Nothing Then StringList = New List(Of String)

        If RecentFramePositions Is Nothing Then RecentFramePositions = New List(Of String)

        If Check(CustomMenuCrop, "Menu in crop dialog", 17) Then
            CustomMenuCrop = CropForm.GetDefaultMenu
        End If

        If Check(CustomMenuMainForm, "Main menu in main window", 161) Then
            CustomMenuMainForm = MainForm.GetDefaultMenu
        End If

        If Check(CustomMenuPreview, "Menu in preview dialog", 52) Then
            CustomMenuPreview = PreviewForm.GetDefaultMenuPreview
        End If

        If Check(CustomMenuSize, "Target size menu in main dialog", 29) Then
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
                    If Not i.Path?.StartsWith("Backup | ") Then i.Path = "Backup | " + i.Path
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

        If LastSourceDirValue Is Nothing Then LastSourceDirValue = ""
    End Sub

    Property LastSourceDir() As String
        Get
            If LastSourceDirValue = "" Then
                Return ""
            End If

            If Not Directory.Exists(LastSourceDirValue) Then
                LastSourceDirValue = DirPath.GetParent(LastSourceDirValue)
            End If

            Return LastSourceDirValue
        End Get
        Set(Value As String)
            If Directory.Exists(Value) Then LastSourceDirValue = Value
        End Set
    End Property

    Shared Function GetDefaultSourceAspectRatioMenu() As String
        Dim r =
                "DAR |  4:3 = 1.333333" + BR +
                "DAR | 16:9 = 1.777777" + BR +
                "DAR | -" + BR +
                "DAR | 16:9 PAL ITU 1.823361 = 1.823361" + BR +
                "DAR |  4:3 PAL ITU 1.367521 = 1.367521" + BR +
                "DAR | 16:9 NTSC ITU 1.822784 = 1.822784" + BR +
                "DAR |  4:3 NTSC ITU 1.367088 = 1.367088" + BR +
                "PAR |  1:1 = 1:1" + BR +
                "PAR | -" + BR +
                "PAR | 16:9 PAL DVD MPEG-4 16:11 = 16:11" + BR +
                "PAR |  4:3 PAL DVD MPEG-4 12:11 = 12:11" + BR +
                "PAR | 16:9 NTSC DVD MPEG-4 40:33 = 40:33" + BR +
                "PAR |  4:3 NTSC DVD MPEG-4 10:11 = 10:11"

        Return r.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
    End Function

    Shared Function GetDefaultTargetImageSizeMenu() As String
        Return "1920 x auto = 1920 x 0" + BR +
               "1280 x auto = 1280 x 0" + BR2 +
               "640 x auto = 640 x 0" + BR +
               "480 x auto = 480 x 0" + BR2 +
               "300 k Pixel = 300000" + BR +
               "200 k Pixel = 200000"
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
End Class