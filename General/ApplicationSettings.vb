Imports System
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Reflection
Imports System.Resources
Imports System.Globalization
Imports System.Xml.Linq

Imports StaxRip.UI

<Serializable()>
Public Class ApplicationSettings
    Implements ISafeSerialization

    Friend FilterPreferences As StringPairList
    Public AudioProfiles As List(Of AudioProfile)
    Public AviSynthCategories As List(Of AviSynthCategory)
    Public AviSynthProfiles As List(Of TargetAviSynthDocument)
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
    Public ThumbnailColumns As Integer = 3
    Public ThumbnailRows As Integer = 12
    Public ThumbnailWidth As Integer = 260
    Public ToolStripRenderMode As ToolStripRenderMode
    Public Versions As Dictionary(Of String, Integer)
    Public VideoEncoderProfiles As List(Of VideoEncoder)
    Public WindowPositions As WindowPositions
    Public WindowPositionsCenterScreen As String()
    Public WindowPositionsRemembered As String()
    Public WindowStatePreview As FormWindowState

    Public DeleteTempFilesToRecycleBin As Boolean = True

    Private WasUpdatedValue As Boolean

    Property WasUpdated() As Boolean Implements ISafeSerialization.WasUpdated
        Get
            Return WasUpdatedValue
        End Get
        Set(value As Boolean)
            WasUpdatedValue = value
        End Set
    End Property

    ReadOnly Property VersionsProperty() As Dictionary(Of String, Integer) Implements ISafeSerialization.Versions
        Get
            Return Versions
        End Get
    End Property

    Private Function Check(obj As Object, key As String, version As Integer) As Boolean
        Return SafeSerialization.Check(Me, obj, key, version)
    End Function

    Sub Init() Implements ISafeSerialization.Init
        If Versions Is Nothing Then
            Versions = New Dictionary(Of String, Integer)
        End If

        If Check(Storage, "Misc", 2) Then
            Storage = New ObjectStorage
        End If

        If Check(VideoEncoderProfiles, "Video Encoder Profiles", 179) Then
            If VideoEncoderProfiles Is Nothing Then
                VideoEncoderProfiles = VideoEncoder.GetDefaults()
            Else
                Dim l As New List(Of VideoEncoder)

                For Each i In VideoEncoderProfiles
                    If Not i.Name.Contains("Backup") Then
                        i.Name = "Backup | " + i.Name
                        l.Add(i)
                    End If
                Next

                VideoEncoderProfiles = VideoEncoder.GetDefaults()
                VideoEncoderProfiles.AddRange(l)
            End If
        End If

        If Check(AudioProfiles, "Audio Profiles", 112) Then
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

        If Check(Demuxers, "Demuxers", 94) Then Demuxers = Demuxer.GetDefaults()

        If Check(FilterPreferences, "Filter Preference", 21) Then
            FilterPreferences = New StringPairList
            FilterPreferences.Add("264, h264, avc", "LWLibavVideoSource")
            FilterPreferences.Add("265, h265, hevc", "LWLibavVideoSource")
            FilterPreferences.Add("default", "FFVideoSource")
            FilterPreferences.Add("dgi", "DGSource")
            FilterPreferences.Add("dgim", "DGSourceIM")
            FilterPreferences.Add("mp4, m4v", "LSMASHVideoSource")
            FilterPreferences.Add("ts, m2ts", "LWLibavVideoSource")
            FilterPreferences.Add("wmv", "DSS2")

            'FilterPreferences.Add("d2v", "MPEG2Source")
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

        If Check(MuxerProfiles, "Container Profiles", 36) Then
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
            CmdlPresetsMKV = "File Attachment = --attach-file ""$browse_file$""" + CrLf +
                             "Attachment Description = --attachment-description ""$enter_text$""" + CrLf +
                             "Process Priority = --priority $select:lowest;lower;normal;higher;highest$"
        End If

        If Check(CmdlPresetsEac3to, "eac3to custom command line menu presets", 7) Then
            CmdlPresetsEac3to = GetDefaultEac3toMenu()
        End If

        If Check(CmdlPresetsMP4, "MP4 custom command line menu presets", 1) Then
            CmdlPresetsMP4 = "iPod = -ipod"
        End If

        If Check(CmdlPresetsX264, "x264 custom command line menu presets", 6) OrElse Not OK(CmdlPresetsX264) Then
            CmdlPresetsX264 = "SAR | PAL | 4:3 = --sar 12:11" + CrLf +
                              "SAR | PAL | 16:9 = --sar 16:11" + CrLf +
                              "SAR | NTSC | 4:3 = --sar 10:11" + CrLf +
                              "SAR | NTSC | 16:9 = --sar 40:33" + CrLf +
                              "Enter SAR = --sar $enter_text:Please enter the SAR.$" + CrLf +
                              "Stats = --stats ""%temp_file%.stats"""
        End If

        If Check(SourceAspectRatioMenu, "Source aspect ratio menu", 22) Then
            SourceAspectRatioMenu = GetDefaultSourceAspectRatioMenu()
        End If

        If Check(TargetImageSizeMenu, "Target image size menu", 13) Then
            TargetImageSizeMenu = GetDefaultTargetImageSizeMenu()
        End If

        If StringList Is Nothing Then
            StringList = New List(Of String)
        End If

        If RecentFramePositions Is Nothing Then
            RecentFramePositions = New List(Of String)
        End If

        If Check(CustomMenuCrop, "Menu in crop dialog", 11) Then
            CustomMenuCrop = CropForm.GetDefaultMenu
        End If

        If Check(CustomMenuMainForm, "Main menu in main window", 120) Then
            CustomMenuMainForm = MainForm.GetDefaultMainMenu
        End If

        If Check(CustomMenuPreview, "Menu in preview dialog", 46) Then
            CustomMenuPreview = PreviewForm.GetDefaultMenuPreview
        End If

        If Check(CustomMenuSize, "Target size menu in main dialog", 24) Then
            CustomMenuSize = MainForm.GetDefaultMenuSize
        End If

        If Check(AviSynthCategories, "Filter Profiles", 128) Then
            If AviSynthCategories Is Nothing Then
                AviSynthCategories = AviSynthCategory.GetDefaults
            Else
                Dim backup As New AviSynthCategory("Backup")

                For Each c In AviSynthCategories.ToList
                    For Each f In c.Filters.ToList
                        If f.Category <> "Backup" Then
                            f.Path = f.Category + " | " + f.Path
                            f.Category = "Backup"
                            backup.Filters.Add(f)
                        End If

                        c.Filters.Remove(f)
                    Next
                Next

                AviSynthCategories = AviSynthCategory.GetDefaults
                AviSynthCategories.Add(backup)
            End If
        End If

        If Check(AviSynthProfiles, "Filter Setup Profiles", 85) Then
            AviSynthProfiles = AviSynthDocument.GetDefaults
        End If

        If LastSourceDirValue Is Nothing Then
            LastSourceDirValue = ""
        End If
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
            If OK(Value) AndAlso Directory.Exists(Value) Then
                LastSourceDirValue = Value
            End If
        End Set
    End Property

    Shared Function GetDefaultSourceAspectRatioMenu() As String
        Dim r =
                "DAR |  4:3 = 1.333333" + CrLf +
                "DAR | 16:9 = 1.777777" + CrLf +
                "DAR | -" + CrLf +
                "DAR | 16:9 PAL  ITU 1.823361 = 1.823361" + CrLf +
                "DAR |  4:3 PAL  ITU 1.367521 = 1.367521" + CrLf +
                "DAR | 16:9 NTSC ITU 1.822784 = 1.822784" + CrLf +
                "DAR |  4:3 NTSC ITU 1.367088 = 1.367088" + CrLf +
                "PAR |  1:1 = 1:1" + CrLf +
                "PAR | -" + CrLf +
                "PAR | 16:9 PAL  DVD MPEG-4 16:11 = 16:11" + CrLf +
                "PAR |  4:3 PAL  DVD MPEG-4 12:11 = 12:11" + CrLf +
                "PAR | 16:9 NTSC DVD MPEG-4 40:33 = 40:33" + CrLf +
                "PAR |  4:3 NTSC DVD MPEG-4 10:11 = 10:11"

        Return r.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
    End Function

    Shared Function GetDefaultTargetImageSizeMenu() As String
        Return "1920 x auto = 1920 x 0" + CrLf +
               "1280 x auto = 1280 x 0" + CrLf2 +
               "640 x auto = 640 x 0" + CrLf +
               "480 x auto = 480 x 0" + CrLf2 +
               "300 k Pixel = 300000" + CrLf +
               "200 k Pixel = 200000"
    End Function

    Shared Function GetDefaultEac3toMenu() As String
        Return _
"AAC 96 Kbps - 2ch - Normalize - 16bit = -down16 -downStereo -normalize -quality=0.3" + CrLf +
"AAC 132 Kbps - 2ch - Normalize - 16bit = -down16 -downStereo -normalize -quality=0.4" + CrLf +
"AAC 240 Kbps 5.1ch - Normalize - 16bit = -down16 -down6 -normalize -quality=0.3" + CrLf +
"Normalize = -normalize" + CrLf +
"Convert to 16 bit = -down16" + CrLf +
"Extract DTS Core = -core" + CrLf +
"Downmix | Multichannel to stereo = -downStereo" + CrLf +
"Downmix | Multichannel to stereo (DPL II) = -downDpl" + CrLf +
"Downmix | 7 or 8 channels to 6 channels = -down6" + CrLf +
"Downmix | Mix LFE in (stereo downmixing) = -mixlfe" + CrLf +
"AAC Quality | 0.10 = -quality=0.10" + CrLf +
"AAC Quality | 0.15 = -quality=0.15" + CrLf +
"AAC Quality | 0.20 = -quality=0.20" + CrLf +
"AAC Quality | 0.25 = -quality=0.25" + CrLf +
"AAC Quality | 0.30 = -quality=0.30" + CrLf +
"AAC Quality | 0.35 = -quality=0.35" + CrLf +
"AAC Quality | 0.40 = -quality=0.40" + CrLf +
"AAC Quality | 0.45 = -quality=0.45" + CrLf +
"AAC Quality | 0.50 = -quality=0.50" + CrLf +
"AAC Quality | 0.55 = -quality=0.55" + CrLf +
"AAC Quality | 0.60 = -quality=0.60" + CrLf +
"AAC Quality | 0.65 = -quality=0.65" + CrLf +
"AAC Quality | 0.70 = -quality=0.70" + CrLf +
"AAC Quality | 0.75 = -quality=0.75" + CrLf +
"AAC Quality | 0.80 = -quality=0.80" + CrLf +
"AAC Quality | 0.85 = -quality=0.85" + CrLf +
"AAC Quality | 0.90 = -quality=0.90" + CrLf +
"AAC Quality | 0.95 = -quality=0.95" + CrLf +
"AC3 Encoding | 192 = -192" + CrLf +
"AC3 Encoding | 224 = -224" + CrLf +
"AC3 Encoding | 384 = -384" + CrLf +
"AC3 Encoding | 448 = -448" + CrLf +
"AC3 Encoding | 640 = -640" + CrLf +
"DTS Encoding | 768 = -768" + CrLf +
"DTS Encoding | 1536 = -1536" + CrLf +
"Resample | 44100 = -resampleTo44100" + CrLf +
"Resample | 48000 = -resampleTo48000" + CrLf +
"Resample | 88200 = -resampleTo88200" + CrLf +
"Resample | 96000 = -resampleTo96000"
    End Function
End Class