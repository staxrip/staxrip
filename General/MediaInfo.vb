
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions

Public Class MediaInfo
    Implements IDisposable

    Private Handle As IntPtr
    Private Shared Loaded As Boolean

    Sub New(path As String)
        If Not Loaded Then
            Native.LoadLibrary(Package.MediaInfo.Path)
            Loaded = True
        End If

        Handle = MediaInfo_New()
        MediaInfo_Open(Handle, path)
    End Sub

    Private VideoStreamsValue As List(Of VideoStream)

    ReadOnly Property VideoStreams() As List(Of VideoStream)
        Get
            If VideoStreamsValue Is Nothing Then
                VideoStreamsValue = New List(Of VideoStream)
                Dim count = MediaInfo_Count_Get(Handle, MediaInfoStreamKind.Video, -1)

                If count > 0 Then
                    For index = 0 To count - 1
                        Dim at As New VideoStream
                        at.Index = index

                        Dim streamOrder = GetVideo(index, "StreamOrder")
                        If Not streamOrder.IsInt Then streamOrder = (index + 1).ToString
                        at.StreamOrder = streamOrder.ToInt

                        at.Format = GetVideo(index, "Format")
                        at.ID = GetVideo(index, "ID").ToInt

                        VideoStreamsValue.Add(at)
                    Next
                End If
            End If

            Return VideoStreamsValue
        End Get
    End Property

    ReadOnly Property AudioStreams() As List(Of AudioStream)
        Get
            Dim ret As New List(Of AudioStream)
            Dim count = MediaInfo_Count_Get(Handle, MediaInfoStreamKind.Audio, -1)
            Dim offset As Integer

            If count > 0 Then
                For index = 0 To count - 1
                    Dim at As New AudioStream
                    at.Index = index

                    Dim streamOrder = GetAudio(index, "StreamOrder")

                    If Not streamOrder.IsInt Then
                        streamOrder = (index + 1).ToString
                    End If

                    at.StreamOrder = streamOrder.ToInt + offset

                    Dim id = GetAudio(index, "ID")

                    If Not id.IsInt Then
                        id = (index + 2).ToString
                    End If

                    at.ID = id.ToInt + offset

                    at.Lossy = GetAudio(index, "Compression_Mode") = "Lossy"
                    at.SamplingRate = GetAudio(index, "SamplingRate").ToInt
                    at.BitDepth = GetAudio(index, "BitDepth").ToInt
                    at.Format = GetAudio(index, "Format")
                    at.FormatString = GetAudio(index, "Format/String")
                    at.FormatProfile = GetAudio(index, "Format_Profile")
                    at.Title = GetAudio(index, "Title").Trim
                    at.Forced = GetAudio(index, "Forced") = "Yes"
                    at.Default = GetAudio(index, "Default") = "Yes"

                    Dim lm = GetAudio(index, "Language_More")

                    If lm <> "" Then
                        If at.Title = "" Then
                            at.Title = lm
                        Else
                            at.Title += " - " + lm
                        End If
                    End If

                    Dim bitrate = GetAudio(index, "BitRate")

                    If bitrate.IsInt Then
                        at.Bitrate = CInt(bitrate.ToInt / 1000)
                    ElseIf bitrate.Contains("/") Then
                        Dim values = bitrate.Split("/"c)
                        at.Bitrate = CInt(values(0).ToInt / 1000)
                        at.Bitrate2 = CInt(values(1).ToInt / 1000)
                    End If

                    If at.Bitrate = 0 Then at.Bitrate = GetAudio(index, "FromStats_BitRate").ToInt

                    at.Delay = GetAudio(index, "Video_Delay").ToInt

                    If at.Delay = 0 Then
                        at.Delay = GetAudio(index, "Source_Delay").ToInt
                    End If

                    Dim channels = GetAudio(index, "Channel(s)")
                    at.Channels = channels.ToInt

                    If at.Channels = 0 Then
                        at.Channels = GetAudio(index, "Channel(s)_Original").ToInt
                    End If

                    If at.Channels = 0 Then
                        If channels.Contains("/") Then
                            Dim values = channels.Split("/"c)
                            at.Channels = values(0).ToInt
                            at.Channels2 = values(1).ToInt
                        End If
                    End If

                    at.Language = New Language(GetAudio(index, "Language/String2"))

                    Select Case p.DemuxAudio
                        Case DemuxMode.All
                            at.Enabled = True
                        Case DemuxMode.None
                            at.Enabled = False
                        Case DemuxMode.Preferred, DemuxMode.Dialog
                            Dim autoCode = p.PreferredAudio.ToLower.SplitNoEmptyAndWhiteSpace(",", ";", " ")
                            at.Enabled = autoCode.ContainsAny("all", at.Language.TwoLetterCode, at.Language.ThreeLetterCode)
                    End Select

                    ret.Add(at)
                Next
            End If

            Return ret
        End Get
    End Property

    ReadOnly Property Subtitles() As List(Of Subtitle)
        Get
            Dim ret As New List(Of Subtitle)
            Dim count = MediaInfo_Count_Get(Handle, MediaInfoStreamKind.Text, -1)
            Dim offset As Integer

            If count > 0 Then
                For index = 0 To count - 1
                    Dim subtitle As New Subtitle(New Language(GetText(index, "Language")))
                    subtitle.Index = index
                    Dim streamOrder = GetText(index, "StreamOrder")

                    If streamOrder <> "" Then
                        If streamOrder.Contains("-") Then
                            subtitle.StreamOrder = streamOrder.Right("-").ToInt + offset
                        Else
                            subtitle.StreamOrder = streamOrder.ToInt
                        End If
                    End If

                    subtitle.Forced = GetText(index, "Forced") = "Yes"
                    subtitle.Default = GetText(index, "Default") = "Yes"
                    subtitle.ID = GetText(index, "ID").ToInt
                    subtitle.Title = GetText(index, "Title").Trim
                    subtitle.CodecString = GetText(index, "Codec/String")
                    If subtitle.CodecString = "" Then subtitle.CodecString = GetText(index, "Format")
                    subtitle.Format = GetText(index, "Format")
                    subtitle.Size = GetText(index, "StreamSize").ToInt

                    Dim autoCode = p.PreferredSubtitles.ToLower.SplitNoEmptyAndWhiteSpace(",", ";", " ")
                    subtitle.Enabled = autoCode.ContainsAny("all", subtitle.Language.TwoLetterCode, subtitle.Language.ThreeLetterCode) OrElse p.DemuxSubtitles = DemuxMode.All

                    ret.Add(subtitle)
                Next
            End If

            Return ret
        End Get
    End Property

    Shared Function GetAudioStreams(path As String) As List(Of AudioStream)
        Return GetMediaInfo(path).AudioStreams
    End Function

    Shared Function GetVideoStreams(path As String) As List(Of VideoStream)
        Return GetMediaInfo(path).VideoStreams
    End Function

    Shared Function GetSubtitles(path As String) As List(Of Subtitle)
        Return GetMediaInfo(path).Subtitles
    End Function

    Shared Function GetSummary(path As String) As String
        Dim mi = GetMediaInfo(path)
        MediaInfo_Option(mi.Handle, "Complete", "0")
        Dim ret = Marshal.PtrToStringUni(MediaInfo_Inform(mi.Handle, 0))
        If ret.Contains("UniqueID/String") Then ret = Regex.Replace(ret, "UniqueID/String +: .+\n", "")
        If ret.Contains("Unique ID") Then ret = Regex.Replace(ret, "Unique ID +: .+\n", "")
        If ret.Contains("Encoded_Library_Settings") Then ret = Regex.Replace(ret, "Encoded_Library_Settings +: .+\n", "")
        If ret.Contains("Encoding settings") Then ret = Regex.Replace(ret, "Encoding settings +: .+\n", "")
        If ret.Contains("Format settings, ") Then ret = ret.Replace("Format settings, ", "Format, ")
        Return ret.FormatColumn(":").Trim
    End Function

    Shared Function GetCompleteSummary(path As String) As String
        Dim mi = GetMediaInfo(path)
        MediaInfo_Option(mi.Handle, "Complete", "1")
        Return Marshal.PtrToStringUni(MediaInfo_Inform(mi.Handle, 0))
    End Function

    Function GetInfo(streamKind As MediaInfoStreamKind, parameter As String) As String
        Return Marshal.PtrToStringUni(MediaInfo_Get(Handle, streamKind, 0, parameter, MediaInfoInfoKind.Text, MediaInfoInfoKind.Name))
    End Function

    Function GetAudio(streamNumber As Integer, parameter As String) As String
        Return Marshal.PtrToStringUni(MediaInfo_Get(Handle, MediaInfoStreamKind.Audio, streamNumber, parameter, MediaInfoInfoKind.Text, MediaInfoInfoKind.Name))
    End Function

    Function GetText(streamNumber As Integer, parameter As String) As String
        Return Marshal.PtrToStringUni(MediaInfo_Get(Handle, MediaInfoStreamKind.Text, streamNumber, parameter, MediaInfoInfoKind.Text, MediaInfoInfoKind.Name))
    End Function

    Function GetInfo(streamKind As MediaInfoStreamKind, streamNumber As Integer, parameter As String) As String
        Return Marshal.PtrToStringUni(MediaInfo_Get(Handle, streamKind, streamNumber, parameter, MediaInfoInfoKind.Text, MediaInfoInfoKind.Name))
    End Function

    Shared Function GetInfo(path As String, streamKind As MediaInfoStreamKind, parameter As String) As String
        If path = "" Then Return ""
        Return GetMediaInfo(path).GetInfo(streamKind, parameter)
    End Function

    Shared Function GetMenu(path As String, parameter As String) As String
        Return GetInfo(path, MediaInfoStreamKind.Menu, parameter)
    End Function

    Shared Function GetAudio(path As String, parameter As String) As String
        Return GetInfo(path, MediaInfoStreamKind.Audio, parameter)
    End Function

    Function GetVideo(parameter As String) As String
        Return GetInfo(MediaInfoStreamKind.Video, parameter)
    End Function

    Function GetVideo(streamNumber As Integer, parameter As String) As String
        Return GetInfo(MediaInfoStreamKind.Video, streamNumber, parameter)
    End Function

    Shared Function GetVideo(path As String, parameter As String) As String
        Return GetInfo(path, MediaInfoStreamKind.Video, parameter)
    End Function

    Function GetGeneral(parameter As String) As String
        Return GetInfo(MediaInfoStreamKind.General, parameter)
    End Function

    Shared Function GetGeneral(path As String, parameter As String) As String
        Return GetInfo(path, MediaInfoStreamKind.General, parameter)
    End Function

    Shared Function GetVideoFormat(path As String) As String
        Dim ret = GetInfo(path, MediaInfoStreamKind.Video, "Format")

        If ret = "MPEG Video" Then
            ret = "MPEG"
        End If

        Return ret
    End Function

    Function GetFrameRate(Optional defaultValue As Double = 25) As Double
        Dim ret = GetVideo("FrameRate_Num").ToInt / GetVideo("FrameRate_Den").ToInt

        If Calc.IsValidFrameRate(ret) Then
            Return ret
        End If

        ret = GetVideo("FrameRate_Original_Num").ToInt / GetVideo("FrameRate_Original_Den").ToInt

        If Calc.IsValidFrameRate(ret) Then
            Return ret
        End If

        ret = GetVideo("FrameRate").ToDouble

        If Calc.IsValidFrameRate(ret) Then
            Return ret
        End If

        ret = GetVideo("FrameRate_Original").ToDouble

        If Calc.IsValidFrameRate(ret) Then
            Return ret
        End If

        ret = GetVideo("FrameRate_Nominal").ToDouble

        If Calc.IsValidFrameRate(ret) Then
            Return ret
        End If

        Return defaultValue
    End Function

    Shared Function GetFrameRate(path As String, Optional defaultValue As Double = 25) As Double
        Return GetMediaInfo(path).GetFrameRate(defaultValue)
    End Function

    Function GetChannels() As Integer
        Dim channelsString = GetInfo(MediaInfoStreamKind.Audio, "Channel(s)")
        Dim ret = channelsString.ToInt
        If ret = 0 Then ret = GetInfo(MediaInfoStreamKind.Audio, "Channel(s)_Original").ToInt

        If ret = 0 Then
            If channelsString.Contains("/") Then
                Dim values = channelsString.Split("/"c)
                Dim value0 = values(0).ToInt
                Dim value1 = values(1).ToInt
                If value0 >= value1 Then ret = value0 Else ret = value1
            End If
        End If

        If ret = 0 Then ret = 2
        Return ret
    End Function

    Shared Function GetChannels(path As String) As Integer
        Return GetMediaInfo(path).GetChannels
    End Function

    Shared Function GetAudioCodecs(path As String) As String
        Dim ret = GetGeneral(path, "Audio_Codec_List")

        If ret.Contains("MPEG-1 Audio layer 2") Then ret = ret.Replace("MPEG-1 Audio layer 2", "MP2")
        If ret.Contains("MPEG-1 Audio layer 3") Then ret = ret.Replace("MPEG-1 Audio layer 3", "MP3")
        If ret.Contains("MPEG-2 Audio layer 3") Then ret = ret.Replace("MPEG-2 Audio layer 3", "MP3")

        Return ret
    End Function

    Function GetVideoCount() As Integer
        Return MediaInfo_Count_Get(Handle, MediaInfoStreamKind.Video, -1)
    End Function

    Shared Function GetVideoCount(Path As String) As Integer
        Return GetMediaInfo(Path).GetVideoCount
    End Function

    Function GetAudioCount() As Integer
        Return MediaInfo_Count_Get(Handle, MediaInfoStreamKind.Audio, -1)
    End Function

    Shared Function GetAudioCount(path As String) As Integer
        Return GetMediaInfo(path).GetAudioCount
    End Function

    Function GetSubtitleCount() As Integer
        Return MediaInfo_Count_Get(Handle, MediaInfoStreamKind.Text, -1)
    End Function

    Shared Function GetSubtitleCount(path As String) As Integer
        Return GetMediaInfo(path).GetSubtitleCount
    End Function

    Shared Cache As New Dictionary(Of String, MediaInfo)

    Shared Function GetMediaInfo(path As String) As MediaInfo
        If path = "" Then Return Nothing
        Dim key = path & File.GetLastWriteTime(path).Ticks
        If Cache.ContainsKey(key) Then Return Cache(key)
        Dim ret As New MediaInfo(path)
        Cache(key) = ret
        Return ret
    End Function

    Shared Sub ClearCache()
        For Each i In Cache
            i.Value.Dispose()
        Next

        Cache.Clear()
    End Sub

#Region "IDisposable"

    Private Disposed As Boolean

    Sub Dispose() Implements IDisposable.Dispose
        If Not Disposed Then
            Disposed = True
            MediaInfo_Close(Handle)
            MediaInfo_Delete(Handle)
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
    End Sub

#End Region

#Region "native"

    <DllImport("MediaInfo.dll")>
    Private Shared Function MediaInfo_New() As IntPtr
    End Function

    <DllImport("MediaInfo.dll")>
    Private Shared Sub MediaInfo_Delete(Handle As IntPtr)
    End Sub

    <DllImport("MediaInfo.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function MediaInfo_Open(Handle As IntPtr, FileName As String) As Integer
    End Function

    <DllImport("MediaInfo.dll")>
    Private Shared Function MediaInfo_Close(Handle As IntPtr) As Integer
    End Function

    <DllImport("MediaInfo.dll")>
    Private Shared Function MediaInfo_Inform(Handle As IntPtr,
                                             Reserved As Integer) As IntPtr
    End Function

    <DllImport("MediaInfo.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function MediaInfo_Get(Handle As IntPtr,
                                          StreamKind As MediaInfoStreamKind,
                                          StreamNumber As Integer, Parameter As String,
                                          KindOfInfo As MediaInfoInfoKind,
                                          KindOfSearch As MediaInfoInfoKind) As IntPtr
    End Function

    <DllImport("MediaInfo.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function MediaInfo_Option(Handle As IntPtr,
                                             OptionString As String,
                                             Value As String) As IntPtr
    End Function

    <DllImport("MediaInfo.dll")>
    Private Shared Function MediaInfo_State_Get(Handle As IntPtr) As Integer
    End Function

    <DllImport("MediaInfo.dll")>
    Private Shared Function MediaInfo_Count_Get(Handle As IntPtr,
                                                StreamKind As MediaInfoStreamKind,
                                                StreamNumber As Integer) As Integer
    End Function

#End Region

End Class

Public Enum MediaInfoStreamKind
    General
    Video
    Audio
    Text
    Other
    Image
    Menu
    Max
End Enum

Public Enum MediaInfoInfoKind
    Name
    Text
    Measure
    Options
    NameText
    MeasureText
    Info
    HowTo
End Enum