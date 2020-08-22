
Imports System.ComponentModel
Imports System.Globalization
Imports System.Runtime.CompilerServices

<Serializable()>
Public Class Project
    Implements ISafeSerialization, INotifyPropertyChanged

    <NonSerialized>
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private Storage As ObjectStorage

    Public AdjustHeight As Boolean = True
    Public Audio0 As AudioProfile
    Public Audio1 As AudioProfile
    Public AudioTracks As List(Of AudioProfile)
    Public AutoCompCheck As Boolean
    Public AutoCorrectCropValues As Boolean = True
    Public AutoResizeImage As Integer
    Public AutoSmartCrop As Boolean
    Public AutoSmartOvercrop As Double
    Public BatchMode As Boolean
    Public BitrateIsFixed As Boolean
    Public CodeAtTop As String = ""
    Public CompCheckAction As CompCheckAction = CompCheckAction.AdjustImageSize
    Public CompCheckRange As Integer = 5
    Public Compressibility As Double
    Public ConvertSup2Sub As Boolean
    Public CustomSourceDAR As String = ""
    Public CustomSourcePAR As String = ""
    Public CustomTargetDAR As String = ""
    Public CustomTargetPAR As String = ""
    Public CutFrameCount As Integer
    Public CutFrameRate As Double
    Public CuttingMode As CuttingMode
    Public DefaultSubtitle As DefaultSubtitleMode
    Public DefaultTargetFolder As String = ""
    Public DefaultTargetName As String = ""
    Public DemuxAudio As DemuxMode = DemuxMode.All
    Public DemuxSubtitles As DemuxMode = DemuxMode.All
    Public ExtractTimestamps As Boolean
    Public FileExistAudio As FileExistMode
    Public FileExistVideo As FileExistMode
    Public FirstOriginalSourceFile As String
    Public ForcedOutputMod As Integer = 8
    Public HarcodedSubtitle As Boolean
    Public ImportVUIMetadata As Boolean = True
    Public ITU As Boolean = True
    Public LastOriginalSourceFile As String
    Public Log As New LogBuilder
    Public MaxAspectRatioError As Double = 2
    Public MKVHDR As Boolean
    Public MTN As Boolean
    Public NoDialogs As Boolean
    Public PreferredAudio As String
    Public PreferredSubtitles As String
    Public Ranges As List(Of Range)
    Public RemindArError As Boolean = True
    Public RemindOversize As Boolean = True
    Public RemindToCrop As Boolean = False
    Public RemindToCut As Boolean = False
    Public RemindToDoCompCheck As Boolean = False
    Public RemindToSetFilters As Boolean = False
    Public ResizeSliderMaxWidth As Integer
    Public SaveThumbnails As Boolean
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
    Public SourceFrameRate As Double
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
    Public SourceWidth As Integer = 1920
    Public SubtitleName As String = ""
    Public TargetFrameRate As Double
    Public TargetHeight As Integer = 1080
    Public TargetSeconds As Integer = 5400
    Public TargetSize As Integer = 5000
    Public TargetWidth As Integer = 1920
    Public TempDir As String
    Public TemplateName As String = ""
    Public TrimCode As String = ""
    Public UseScriptAsAudioSource As Boolean
    Public Versions As Dictionary(Of String, Integer)
    Public VideoBitrate As Integer = 5000
    Public VideoEncoder As VideoEncoder

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
        If Ranges Is Nothing Then Ranges = New List(Of Range)
        If SourceFile Is Nothing Then SourceFile = ""
        If TargetFile Is Nothing Then TargetFile = ""

        If Check(PreferredSubtitles, "Automatically Included Subtitles", 2) Then
            If Language.CurrentCulture.TwoLetterCode = "en" Then
                PreferredSubtitles = "eng und"
            Else
                PreferredSubtitles = Language.CurrentCulture.ThreeLetterCode + " eng und"
            End If
        End If

        If Check(PreferredAudio, "Preferred Audio Languages", 1) Then
            If Language.CurrentCulture.TwoLetterCode = "en" Then
                PreferredAudio = "eng und"
            Else
                PreferredAudio = Language.CurrentCulture.ThreeLetterCode + " eng und"
            End If
        End If

        If SourceScript Is Nothing Then SourceScript = New SourceVideoScript
        If SkippedAssistantTips Is Nothing Then SkippedAssistantTips = New List(Of String)
        If SourceFiles Is Nothing Then SourceFiles = New List(Of String)
        If AudioTracks Is Nothing Then AudioTracks = New List(Of AudioProfile)

        If Check(VideoEncoder, "Video Encoder", 75) Then VideoEncoder = New x265Enc

        If Check(Audio0, "Audio Track 1", 36) Then
            Audio0 = New GUIAudioProfile(AudioCodec.Opus, 1) With {.Bitrate = 250}
            Audio0.Language = New Language(CultureInfo.CurrentCulture.TwoLetterISOLanguageName, True)
        End If

        If Check(Audio1, "Audio Track 2", 36) Then
            Audio1 = New GUIAudioProfile(AudioCodec.Opus, 1) With {.Bitrate = 250}
            Audio1.Language = New Language("en", True)
        End If

        If Check(Script, "Filter Setup", 50) Then Script = StaxRip.VideoScript.GetDefaults()(0)

        Migrate()
    End Sub

    Sub Migrate()
        Audio0.Migrate()
        Audio1.Migrate()

        For Each i In AudioTracks
            i.Migrate()
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
        Set(ByVal value As String)
            If value <> TargetFileValue Then
                If value <> "" AndAlso Not value.FileName.IsValidFileName Then
                    MsgWarn("Filename contains invalid characters.")
                    Exit Property
                End If

                If Text.Encoding.Default.CodePage <> 65001 AndAlso
                    Not value.IsANSICompatible AndAlso Script.Engine = ScriptEngine.AviSynth Then

                    MsgWarn(Strings.NoUnicode)
                    Exit Property
                End If

                TargetFileValue = value
                NotifyPropertyChanged()
            End If
        End Set
    End Property

    Sub AddHardcodedSubtitleFilter(path As String, showErrorMsg As Boolean)
        If p.Script.Engine = ScriptEngine.AviSynth Then
            Dim filterName As String

            Select Case path.Ext
                Case "idx"
                    filterName = "VobSub"
                Case "srt", "ass"
                    filterName = "TextSubMod"
                Case Else
                    If showErrorMsg Then MsgError("Only idx, srt and ass file types are supported.")
                    Exit Sub
            End Select

            Dim filter As New VideoFilter
            filter.Category = "Subtitle"
            filter.Path = path.FileName
            filter.Active = True
            filter.Script = filterName + "(""" + path + """)"
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
                    Dim sb As New SelectionBox(Of String)
                    sb.Title = "ASS Subtitle Renderer"
                    sb.Text = "Please select a renderer."
                    sb.AddItem("core.vsfm.TextSubMod")
                    sb.AddItem("core.sub.TextFile")

                    If sb.Show = DialogResult.OK Then
                        filterName = sb.SelectedValue
                    Else
                        Exit Sub
                    End If
                Case "sup"
                    filterName = "core.sub.ImageFile"
                Case Else
                    If showErrorMsg Then MsgError("Only idx, srt, ass and sup file types are supported.")
                    Exit Sub
            End Select

            Dim filter As New VideoFilter
            filter.Category = "Subtitle"
            filter.Path = path.FileName
            filter.Active = True
            filter.Script = "clip = " + filterName + "(clip, file = r""" + path + """)"
            Dim insertCat = If(p.Script.IsFilterActive("Crop"), "Crop", "Source")
            Script.InsertAfter(insertCat, filter)
        End If
    End Sub
End Class