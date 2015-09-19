Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions

Class MediaInfo
    Implements IDisposable

    Private Handle As IntPtr

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

    <DllImport("MediaInfo.dll", CharSet:=CharSet.Unicode)>
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

    Private Shared Loaded As Boolean

    Sub New(path As String)
        If Not Loaded Then
            Loaded = True
            Native.LoadLibrary(Packs.MediaInfo.GetPath)
        End If

        Handle = MediaInfo_New()
        MediaInfo_Open(Handle, path)
    End Sub

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

    Private VideoStreamsValue As List(Of VideoStream)

    ReadOnly Property VideoStreams() As List(Of VideoStream)
        Get
            If VideoStreamsValue Is Nothing Then
                VideoStreamsValue = New List(Of VideoStream)
                Dim count = MediaInfo_Count_Get(Handle, MediaInfoStreamKind.Video, -1)

                If count > 0 Then
                    For i = 0 To count - 1
                        Dim at As New VideoStream

                        Dim streamOrder = GetVideo(i, "StreamOrder")
                        If Not streamOrder.IsInt Then streamOrder = (i + 1).ToString
                        at.StreamOrder = streamOrder.ToInt

                        at.Format = GetVideo(i, "Format")

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

            If count > 0 Then
                For i = 0 To count - 1
                    Dim at As New AudioStream

                    Dim streamOrder = GetAudio(i, "StreamOrder")
                    If Not streamOrder.IsInt Then streamOrder = (i + 1).ToString
                    at.StreamOrder = streamOrder.ToInt

                    Dim id = GetAudio(i, "ID")
                    If Not id.IsInt Then id = (i + 2).ToString
                    at.ID = id.ToInt

                    at.SamplingRate = GetAudio(i, "SamplingRate").ToInt
                    at.BitDepth = GetAudio(i, "BitDepth").ToInt
                    at.CodecString = GetAudio(i, "Codec/String")
                    at.Codec = GetAudio(i, "Codec")
                    at.Format = GetAudio(i, "Format")
                    at.FormatProfile = GetAudio(i, "Format_Profile")
                    at.Title = GetAudio(i, "Title").Trim

                    If at.Title.Contains("IsoMedia") OrElse at.Title.Contains("GPAC") OrElse at.Title.Contains("PID ") OrElse
                            {"Surround 7.1", "Surround 5.1", "Stereo", "3/2+1", "2/0"}.Contains(at.Title) Then

                        at.Title = ""
                    End If

                    If Not Filepath.IsValidFileSystemName(at.Title) Then
                        at.Title = Filepath.RemoveIllegalCharsFromName(at.Title)
                    End If

                    Dim lm = GetAudio(i, "Language_More")

                    If lm <> "" Then
                        If at.Title = "" Then
                            at.Title = lm
                        Else
                            at.Title += " - " + lm
                        End If
                    End If

                    Dim bitrate = GetAudio(i, "BitRate")

                    If bitrate.IsInt Then
                        at.Bitrate = CInt(bitrate.ToInt / 1000)
                    Else
                        Dim match = Regex.Match(bitrate, "(.+)/(.+)")

                        If match.Success Then
                            If match.Groups(1).Value.IsInt Then
                                at.Bitrate = CInt(match.Groups(1).Value.ToInt / 1000)
                            End If

                            If match.Groups(2).Value.IsInt Then
                                at.BitrateCore = CInt(match.Groups(2).Value.ToInt / 1000)
                            End If
                        End If
                    End If

                    at.Delay = GetAudio(i, "Video_Delay").ToInt
                    If at.Delay = 0 Then at.Delay = GetAudio(i, "Source_Delay").ToInt

                    Dim channels = GetAudio(i, "Channel(s)")
                    at.Channels = channels.ToInt
                    If at.Channels = 0 Then at.Channels = GetAudio(i, "Channel(s)_Original").ToInt

                    If at.Channels = 0 Then
                        Dim match = Regex.Match(channels, "(\d+) */ *(\d+)")

                        If match.Success Then
                            at.Channels = match.Groups(1).Value.ToInt
                            at.ChannelsCore = match.Groups(2).Value.ToInt
                        Else
                            at.Channels = 2
                        End If
                    End If

                    at.Language = New Language(GetAudio(i, "Language/String2"))
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

            If count > 0 Then
                For i = 0 To count - 1
                    Dim s As New Subtitle(New Language(GetInfo(MediaInfoStreamKind.Text, i, "Language")))

                    s.StreamOrder = GetInfo(MediaInfoStreamKind.Text, i, "StreamOrder").ToInt
                    s.ID = GetInfo(MediaInfoStreamKind.Text, i, "ID").ToInt
                    s.Title = GetInfo(MediaInfoStreamKind.Text, i, "Title").Trim
                    s.CodecString = GetInfo(MediaInfoStreamKind.Text, i, "Codec/String")
                    s.Format = GetInfo(MediaInfoStreamKind.Text, i, "Format")

                    ret.Add(s)
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
        Return Marshal.PtrToStringUni(MediaInfo_Inform(mi.Handle, 0)).FormatColumn(":")
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

    Function GetInfo(streamKind As MediaInfoStreamKind, streamNumber As Integer, parameter As String) As String
        Return Marshal.PtrToStringUni(MediaInfo_Get(Handle, streamKind, streamNumber, parameter, MediaInfoInfoKind.Text, MediaInfoInfoKind.Name))
    End Function

    Shared Function GetInfo(path As String, streamKind As MediaInfoStreamKind, parameter As String) As String
        If path = "" Then Return ""
        Return GetMediaInfo(path).GetInfo(streamKind, parameter)
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

    Shared Function GetGeneral(path As String, parameter As String) As String
        Return GetInfo(path, MediaInfoStreamKind.General, parameter)
    End Function

    Function GetFrameRate() As Double
        Dim ret = GetInfo(MediaInfoStreamKind.Video, "FrameRate")

        If ret = "" Then ret = GetInfo(MediaInfoStreamKind.Video, "FrameRate_Original")
        If ret = "" Then ret = GetInfo(MediaInfoStreamKind.Video, "FrameRate_Nominal")

        If ret.IsDouble Then Return ret.ToDouble Else Return 25
    End Function

    Shared Function GetFrameRate(path As String) As Double
        Return GetMediaInfo(path).GetFrameRate
    End Function

    Shared Function GetAudioCodecs(path As String) As String
        Dim ret = GetGeneral(path, "Audio_Codec_List")

        If ret.Contains("MPEG-1 Audio layer 2") Then ret = ret.Replace("MPEG-1 Audio layer 2", "MP2")
        If ret.Contains("MPEG-1 Audio layer 3") Then ret = ret.Replace("MPEG-1 Audio layer 3", "MP3")
        If ret.Contains("MPEG-2 Audio layer 3") Then ret = ret.Replace("MPEG-2 Audio layer 3", "MP3")

        Return ret
    End Function

    Shared Function GetVideoCodec(path As String) As String
        Dim ret = MediaInfo.GetVideo(path, "Codec/String")

        Select Case ret
            Case "MPEG-4 Visual"
                ret = "MPEG-4V"
            Case "MPEG-1 Video"
                ret = "MPEG-1"
            Case "V_MPEGH/ISO/HEVC"
                ret = "HEVC"
            Case "MPEG-2 Video"
                ret = "MPEG-2"
            Case "V_VP8"
                ret = "VP8"
            Case "V_VP9"
                ret = "VP9"
            Case "0x00000000"
                ret = MediaInfo.GetVideo(path, "Format")
        End Select

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
End Class

Public Enum MediaInfoStreamKind
    General
    Video
    Audio
    Text
    Chapters
    Image
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