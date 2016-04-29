Imports System.Globalization
Imports System.Text

<Serializable()>
Public Class Project
    Implements ISafeSerialization

    Private Storage As ObjectStorage

    Public AdjustHeight As Boolean = True
    Public Audio0 As AudioProfile
    Public Audio1 As AudioProfile
    Public AudioTracks As List(Of AudioProfile)
    Public AutoARSignaling As Boolean = True
    Public AutoCompCheck As Boolean
    Public AutoCorrectCropValues As Boolean = True
    Public AutoResizeImage As Integer
    Public AutoSmartCrop As Boolean
    Public AutoSmartOvercrop As Double
    Public BatchMode As Boolean
    Public BitDepth As Integer
    Public ChromaSubsampling As String
    Public CodeAtTop As String = ""
    Public Codec As String
    Public CodecProfile As String
    Public ColorSpace As String
    Public CompCheckAction As CompCheckAction = CompCheckAction.AdjustImageSize
    Public CompCheckRange As Integer = 5
    Public Compressibility As Double
    Public ConvertSup2Sub As Boolean
    Public CustomDAR As String = ""
    Public CustomPAR As String = ""
    Public CutFrameCount As Integer
    Public CutFrameRate As Double
    Public CuttingMode As CuttingMode
    Public DecodingMode As DecodingMode
    Public DefaultTargetFolder As String = ""
    Public DefaultTargetName As String = ""
    Public DeleteTempFilesDir As Boolean
    Public DemuxAudio As DemuxMode
    Public DemuxSubtitles As DemuxMode
    Public FirstOriginalSourceFile As String
    Public FixedBitrate As Integer
    Public ForcedOutputMod As Integer = 16
    Public ITU As Boolean = True
    Public LastOriginalSourceFile As String
    Public Log As StringBuilder
    Public MaxAspectRatioError As Integer = 2
    Public Name As String
    Public NoDialogs As Boolean
    Public PreferredAudio As String
    Public PreferredSubtitles As String
    Public Ranges As List(Of Range)
    Public RemindOversize As Boolean = True
    Public RemindToCrop As Boolean = False
    Public RemindToCut As Boolean = False
    Public RemindToDoCompCheck As Boolean = False
    Public RemindToSetFilters As Boolean = False
    Public ResizeSliderMaxWidth As Integer
    Public SaveThumbnails As Boolean
    Public ScanOrder As String
    Public ScanType As String
    Public Script As TargetVideoScript
    Public ShowDialogsCLI As Boolean
    Public SkippedAssistantTips As List(Of String)
    Public SourceAnamorphic As Boolean
    Public SourceBitrate As Integer
    Public SourceFile As String
    Public SourceFiles As List(Of String)
    Public SourceFrameRate As Double
    Public SourceFrames As Integer
    Public SourceHeight As Integer = 576
    Public SourcePAR As Point = New Point(1, 1)
    Public SourceScript As SourceVideoScript
    Public SourceSeconds As Integer
    Public SourceSize As Long
    Public SourceWidth As Integer = 720
    Public TargetFile As String
    Public TargetFrameRate As Double
    Public TargetHeight As Integer = 576
    Public TargetSeconds As Integer = 5400
    Public TargetSize As Integer = 700
    Public TargetWidth As Integer = 720
    Public TempDir As String
    Public TemplateName As String = ""
    Public TrimCode As String = ""
    Public UseScriptAsAudioSource As Boolean
    Public Versions As Dictionary(Of String, Integer)
    Public VideoBitrate As Integer = 1000
    Public VideoEncoder As VideoEncoder

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
        If TempDir Is Nothing Then TempDir = ""
        If Log Is Nothing Then Log = New StringBuilder
        If Storage Is Nothing Then Storage = New ObjectStorage
        If Ranges Is Nothing Then Ranges = New List(Of Range)
        If TargetFile Is Nothing Then TargetFile = ""
        If Name Is Nothing Then Name = ""
        If SourceFile Is Nothing Then SourceFile = ""

        If Check(PreferredSubtitles, "Automatically Included Subtitles", 2) Then
            If Language.CurrentCulture.TwoLetterCode = "en" Then
                PreferredSubtitles = "eng und"
            Else
                PreferredSubtitles = Language.CurrentCulture.ThreeLetterCode + ", eng, und"
            End If
        End If

        If Check(PreferredAudio, "Preferred Audio Languages", 1) Then
            If Language.CurrentCulture.TwoLetterCode = "en" Then
                PreferredAudio = "eng und"
            Else
                PreferredAudio = Language.CurrentCulture.ThreeLetterCode + ", eng, und"
            End If
        End If

        If SourceScript Is Nothing Then SourceScript = New SourceVideoScript
        If SkippedAssistantTips Is Nothing Then SkippedAssistantTips = New List(Of String)
        If SourceFiles Is Nothing Then SourceFiles = New List(Of String)
        If AudioTracks Is Nothing Then AudioTracks = New List(Of AudioProfile)

        If Check(VideoEncoder, "Video Encoder", 69) Then
            VideoEncoder = VideoEncoder.Getx264Encoder("x264", x264DeviceMode.Disabled)
        End If

        If Check(Audio0, "Audio Track 1", 35) Then
            Audio0 = New GUIAudioProfile(AudioCodec.AAC, 0.35)
            Audio0.Language = New Language(CultureInfo.CurrentCulture.TwoLetterISOLanguageName, True)
        End If

        If Check(Audio1, "Audio Track 2", 35) Then
            Audio1 = New GUIAudioProfile(AudioCodec.AAC, 0.35)
            Audio1.Language = New Language("en", True)
        End If

        If Check(Script, "Filter Setup", 50) Then
            Script = StaxRip.VideoScript.GetDefaults()(0)
            Script.SetFilter("Source", "Manual", "# shows filter selection dialog")
        End If
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

    Function GetAudioTracks() As List(Of AudioProfile)
        Dim ret As New List(Of AudioProfile)

        ret.Add(p.Audio0)
        ret.Add(p.Audio1)
        ret.AddRange(p.AudioTracks)

        Return ret
    End Function
End Class