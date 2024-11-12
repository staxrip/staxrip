
Imports System.ComponentModel
Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports StaxRip.UI

<Serializable()>
Public Class Project
    Implements ISafeSerialization, INotifyPropertyChanged

    <NonSerialized>
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Storage As ObjectStorage

    Public AbortOnFrameMismatch As Boolean = True
    Public AddAttachmentsToMuxer As Boolean = True
    Public AddTagsToMuxer As Boolean = True
    Public AdjustHeight As Boolean = True
    Public AudioFiles As New List(Of AudioProfile)
    Public AudioTracks As New List(Of AudioTrack)
    Public AudioTracksAvailable As Integer = 4
    Public AutoCompCheck As Boolean = False
    Public AutoCorrectCropValues As Boolean = True
    Public AutoCropDolbyVisionThresholdBegin As Integer = 0
    Public AutoCropDolbyVisionThresholdEnd As Integer = 0
    Public AutoCropDolbyVisionMode As AutoCropDolbyVisionMode = AutoCropDolbyVisionMode.Automatic
    Public AutoCropMode As AutoCropMode = AutoCropMode.Always
    Public AutoCropFrameRangeMode As AutoCropFrameRangeMode = AutoCropFrameRangeMode.Automatic
    Public AutoCropFrameRangeThresholdBegin As Integer = 0
    Public AutoCropFrameRangeThresholdEnd As Integer = 0
    Public AutoCropFrameSelectionMode As AutoCropFrameSelectionMode = AutoCropFrameSelectionMode.TimeInterval
    Public AutoCropFixedFramesFrameSelection As Integer = 200
    Public AutoCropFrameIntervalFrameSelection As Integer = 400
    Public AutoCropTimeIntervalFrameSelection As Integer = 15
    Public AutoCropLuminanceThreshold As Single = 10.0
    Public AutoResizeImage As Integer
    Public AutoRotation As Boolean = True
    Public AutoSmartCrop As Boolean
    Public AutoSmartOvercrop As Double
    Public BatchMode As Boolean
    Public BitrateIsFixed As Boolean = True
    Public CodeAtTop As String = ""
    Public CompCheckAction As CompCheckAction = CompCheckAction.AdjustImageSize
    Public CompCheckPercentage As Double = 5.0
    Public CompCheckTestblockSeconds As Double = 2.0
    Public Compressibility As Double
    Public ConvertChromaSubsampling As Boolean = True
    Public ConvertSup2Sub As Boolean
    Public ConvertTo10Bit As Boolean = False
    Public CropWithTonemapping As Boolean = Vulkan.IsSupported
    Public CropWithHighContrast As Boolean = False
    Public CustomSourceDAR As String = ""
    Public CustomSourcePAR As String = ""
    Public CustomTargetDAR As String = ""
    Public CustomTargetPAR As String = ""
    Public CutFrameCount As Integer
    Public CutFrameRate As Double
    Public CuttingMode As CuttingMode
    Public D2VAutoForceFilmThreshold As Single = 95.0F
    Public DefaultSubtitle As DefaultSubtitleMode = DefaultSubtitleMode.Default
    Public DefaultTargetFolder As String = ""
    Public DefaultTargetName As String = ""
    Public DeleteTempFilesMode As DeleteMode = DeleteMode.Disabled
    Public DeleteTempFilesSelection As DeleteSelection = DeleteSelection.Everything
    Public DeleteTempFilesSelectionMode As SelectionMode = SelectionMode.Include
    Public DeleteTempFilesCustomSelection As String() = FileTypes.Video.Distinct().Sort().ToArray()
    Public DeleteTempFilesSelectiveSelection As DeleteSelectiveSelection = DeleteSelectiveSelection.Videos
    Public DeleteTempFilesOnFrameMismatchNegative As Integer = 0
    Public DeleteTempFilesOnFrameMismatchPositive As Integer = 1
    Public DemuxAttachments As Boolean = True
    Public DemuxAudio As DemuxMode = DemuxMode.All
    Public DemuxChapters As Boolean = True
    Public DemuxTags As Boolean = False
    Public DemuxVideo As Boolean = False
    Public ExtractHdrmetadata As HdrmetadataMode = HdrmetadataMode.All
    Public ExtractTimestamps As TimestampsMode = TimestampsMode.VfrOnly
    Public ExtractForcedSubSubtitles As Boolean = True
    Public FileExistAudio As FileExistMode
    Public FileExistVideo As FileExistMode
    Public FirstOriginalSourceFile As String
    Public ForcedOutputMod As Integer = 2
    Public ForcedOutputModDirection As ForceOutputModDirection = ForceOutputModDirection.Increase
    Public ForcedOutputModIgnorable As Boolean = False
    Public ForcedOutputModOnlyIfCropped As Boolean = False
    Public HardcodedSubtitle As Boolean
    Public Hdr10PlusMetadataFile As String
    Public HdrDolbyVisionMetadataFile As DolbyVisionMetadataFile
    Public HdrDolbyVisionMode As DoviMode = DoviMode.Mode2
    Public ImportVUIMetadata As Boolean = True
    Public ITU As Boolean
    Public LastOriginalSourceFile As String
    Public Log As New LogBuilder
    Public MaxAspectRatioError As Double = 2
    Public NoDialogs As Boolean
    Public NoTempDir As Boolean
    Public PreferredAudio As String
    Public PreferredSubtitles As String
    Public Ranges As List(Of Range)
    Public RangesBasedOnFPS As Double = 0
    Public RemindOversize As Boolean = True
    Public RemindToCrop As Boolean = False
    Public RemindToCut As Boolean = False
    Public RemindToDoCompCheck As Boolean = False
    Public RemindToSetFilters As Boolean = False
    Public Script As TargetVideoScript
    Public SkipAudioEncoding As Boolean
    Public SkippedAssistantTips As List(Of String)
    Public SkipVideoEncoding As Boolean
    Public SourceAnamorphic As Boolean
    Public SourceBitrate As Integer
    Public SourceChromaSubsampling As String
    Public SourceColorSpace As String
    Public SourceFile As String
    Public SourceFiles As List(Of String)
    Public SourceFrameRate As Decimal
    Public SourceFrameRateMode As String
    Public SourceFrames As Integer
    Public SourceHeight As Integer = 1080
    Public SourcePAR As Point = New Point(1, 1)
    Public SourceScanOrder As String
    Public SourceScanType As String
    Public SourceScript As SourceVideoScript
    Public SourceSeconds As Integer
    Public SourceSize As Long
    Public SourceVideoBitDepth As Integer
    Public SourceVideoFormat As String
    Public SourceVideoFormatProfile As String
    Public SourceVideoHdrFormat As String = ""
    Public SourceVideoSize As Long
    Public SourceWidth As Integer = 1920
    Public SubtitleMode As SubtitleMode
    Public SubtitleName As String = ""
    Public TargetFrames As Integer
    Public TargetFrameRate As Double
    Public TargetHeight As Integer = 1080
    Public TargetSeconds As Integer = 5400
    Public TargetSize As Integer = 5000
    Public TargetWidth As Integer = 1920
    Public TempDir As String
    Public TemplateName As String = ""
    Public Thumbnailer As Boolean = False
    Public ThumbnailerSettings As ObjectStorage
    Public TrimCode As String = ""
    Public UseScriptAsAudioSource As Boolean
    Public Versions As Dictionary(Of String, Integer)
    Public VideoBitrate As Integer = 5000
    Public VideoEncoder As VideoEncoder
    Public WarnArError As Boolean = True
    Public WarnIdenticalAudio As Boolean = True
    Public WarnNoAudio As Boolean = False

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
        If Versions Is Nothing Then Versions = New Dictionary(Of String, Integer)
        If TempDir Is Nothing Then TempDir = ""
        If Log Is Nothing Then Log = New LogBuilder
        If Storage Is Nothing Then Storage = New ObjectStorage
        If ThumbnailerSettings Is Nothing Then ThumbnailerSettings = New ObjectStorage()
        If Ranges Is Nothing Then Ranges = New List(Of Range)
        If SourceFile Is Nothing Then SourceFile = ""
        If TargetFile Is Nothing Then TargetFile = ""

        If Check(PreferredSubtitles, "Automatically Included Subtitles", 2) Then
            PreferredSubtitles = If(Language.CurrentCulture.TwoLetterCode = "en", "eng und", Language.CurrentCulture.ThreeLetterCode + " eng und")
        End If

        If Check(PreferredAudio, "Preferred Audio Languages", 1) Then
            PreferredAudio = If(Language.CurrentCulture.TwoLetterCode = "en", "eng und", Language.CurrentCulture.ThreeLetterCode + " eng und")
        End If

        If SourceScript Is Nothing Then SourceScript = New SourceVideoScript
        If SkippedAssistantTips Is Nothing Then SkippedAssistantTips = New List(Of String)
        If SourceFiles Is Nothing Then SourceFiles = New List(Of String)
        If AudioFiles Is Nothing Then AudioFiles = New List(Of AudioProfile)
        If AudioTracks Is Nothing Then AudioTracks = New List(Of AudioTrack)

        If Check(VideoEncoder, "Video Encoder", 80) Then
            VideoEncoder = New x265Enc
        End If

        'If Check(AudioTracks, "Audio Tracks", 1) Then
        '    AudioTracks.Add(New AudioTrack() With {
        '                        .AudioProfile = New MuxAudioProfile With {
        '                                            .Language = New Language(CultureInfo.CurrentCulture.TwoLetterISOLanguageName, True)
        '                                        },
        '                        .EditLabel = New AudioEditButtonLabel(0),
        '                        .NameLabel = New AudioNameButtonLabel(0),
        '                        .TextEdit = New AudioTextEdit(0)
        '                    })
        '    AudioTracks.Add(New AudioTrack() With {
        '                        .AudioProfile = New MuxAudioProfile With {
        '                                            .Language = New Language("en", True)
        '                                        },
        '                        .EditLabel = New AudioEditButtonLabel(1),
        '                        .NameLabel = New AudioNameButtonLabel(1),
        '                        .TextEdit = New AudioTextEdit(1)
        '                    })
        'End If

        If Check(Script, "Filter Setup", 50) Then Script = VideoScript.GetDefaults()(0)

        Migrate()
    End Sub

    Sub Migrate()
        For Each track In AudioTracks
            track.AudioProfile.Migrate()
        Next

        For Each ap In AudioFiles
            ap.Migrate()
        Next
    End Sub

    Private CropRightValue As Integer

    Property CropRight As Integer
        Get
            Return CropRightValue
        End Get
        Set(value As Integer)
            CropRightValue = If(value >= 0, value, 0)
        End Set
    End Property

    Private CropLeftValue As Integer

    Property CropLeft As Integer
        Get
            Return CropLeftValue
        End Get
        Set(value As Integer)
            CropLeftValue = If(value >= 0, value, 0)
        End Set
    End Property

    Private CropTopValue As Integer

    Property CropTop As Integer
        Get
            Return CropTopValue
        End Get
        Set(value As Integer)
            CropTopValue = If(value >= 0, value, 0)
        End Set
    End Property

    Private CropBottomValue As Integer

    Property CropBottom As Integer
        Get
            Return CropBottomValue
        End Get
        Set(value As Integer)
            CropBottomValue = If(value >= 0, value, 0)
        End Set
    End Property

    Sub NotifyPropertyChanged(
        <CallerMemberName()> Optional ByVal propertyName As String = Nothing)

        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Private TargetFileValue As String

    Public Property TargetFile As String
        Get
            Return TargetFileValue
        End Get
        Set(value As String)
            If value <> TargetFileValue Then
                If value <> "" AndAlso (value.ContainsAny(Path.GetInvalidPathChars()) OrElse Not value.FileName.IsValidFileName()) Then
                    MsgWarn("Filename contains invalid characters.")
                    Exit Property
                End If

                TargetFileValue = value
                NotifyPropertyChanged()
            End If
        End Set
    End Property

    Sub AddHardcodedSubtitleFilter(path As String, showErrorMsg As Boolean)
        If p.Script.IsAviSynth Then
            Dim filterName As String

            Select Case path.Ext
                Case "idx"
                    filterName = "VobSub"
                Case "srt", "ass"
                    filterName = "TextSub"
                Case Else
                    If showErrorMsg Then
                        MsgError("Only idx, srt and ass file types are supported.")
                    End If

                    Exit Sub
            End Select

            Dim filter As New VideoFilter With {
                .Category = "Subtitle",
                .Path = path.FileName,
                .Active = True,
                .Script = filterName + "(""" + path + """)"
            }
            Dim insertCat = If(p.Script.IsFilterActive("Crop"), "Crop", "Source")
            Script.InsertAfter(insertCat, filter)
        Else
            Dim filterName As String

            Select Case path.Ext
                Case "idx"
                    filterName = "core.vsfm.VobSub"
                Case "srt"
                    filterName = "core.vsfm.TextSubMod"
                Case "ass"
                    Using td As New TaskDialog(Of String)
                        td.Title = "Please select an ASS renderer"
                        td.AddCommand("core.vsfm.TextSubMod")
                        td.AddCommand("core.sub.TextFile")

                        If td.Show <> "" Then
                            filterName = td.SelectedValue
                        Else
                            Exit Sub
                        End If
                    End Using
                Case "sup"
                    filterName = "core.sub.ImageFile"
                Case Else
                    If showErrorMsg Then MsgError("Only idx, srt, ass and sup file types are supported.")
                    Exit Sub
            End Select

            Dim filter As New VideoFilter With {
                .Category = "Subtitle",
                .Path = path.FileName,
                .Active = True,
                .Script = "clip = " + filterName + "(clip, file = r""" + path + """)"
            }
            Dim insertCat = If(p.Script.IsFilterActive("Crop"), "Crop", "Source")
            Script.InsertAfter(insertCat, filter)
        End If
    End Sub

    ReadOnly Property IsSubtitleDemuxingRequired As Boolean
        Get
            Return SubtitleMode = SubtitleMode.All OrElse SubtitleMode = SubtitleMode.Dialog OrElse SubtitleMode = SubtitleMode.Preferred OrElse SubtitleMode = SubtitleMode.PreferredNoMux
        End Get
    End Property
End Class