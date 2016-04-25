Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Security.Permissions
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Win32
Imports VB6 = Microsoft.VisualBasic

Class Settings
    Private Shared Key As String = "SOFTWARE\VideoToolbox\Demux"

    Shared Property MkvToolnixDir As String
        Get
            Return ReadString(NameOf(MkvToolnixDir))
        End Get
        Set(value As String)
            Write(NameOf(MkvToolnixDir), value)
        End Set
    End Property

    Shared Property MP4BoxPath As String
        Get
            Return ReadString(NameOf(MP4BoxPath))
        End Get
        Set(value As String)
            Write(NameOf(MP4BoxPath), value)
        End Set
    End Property

    Shared Sub Write(name As String, value As Object)
        Registry.CurrentUser.Write(Key, name, value)
    End Sub

    Shared Function ReadString(name As String) As String
        Return Registry.CurrentUser.GetString(Key, name)
    End Function
End Class

<Serializable()>
Public Class Subtitle
    Property Title As String = ""
    Property Path As String
    Property CodecString As String
    Property Format As String
    Property ID As Integer
    Property StreamOrder As Integer
    Property IndexIDX As Integer
    Property Language As Language
    Property [Default] As Boolean
    Property Enabled As Boolean
    Property Size As Long

    Sub New()
        Language = New Language
    End Sub

    Sub New(lang As Language)
        Language = lang
    End Sub

    ReadOnly Property Filename As String
        Get
            Dim ret = "ID" & (StreamOrder + 1)
            ret += " " + Language.Name

            If Title <> "" AndAlso Title <> " " AndAlso Not Title.ContainsUnicode Then
                ret += " " + Title.Shorten(30)
            End If

            If Not Filepath.IsValidFileSystemName(ret) Then ret = Filepath.RemoveIllegal(ret)
            Return ret
        End Get
    End Property

    ReadOnly Property Extension As String
        Get
            Select Case CodecString
                Case "VobSub"
                    Return ".idx"
                Case "S_HDMV/PGS", "PGS"
                    Return ".sup"
                Case "S_TEXT/ASS", "ASS"
                    Return ".ass"
                Case "S_TEXT/UTF8", "UTF-8"
                    Return ".srt"
                Case "S_TEXT/SSA", "SSA"
                    Return ".ssa"
                Case "S_TEXT/USF", "USF"
                    Return ".usf"
                Case "Timed"
                    Return ".srt"
            End Select
        End Get
    End Property

    ReadOnly Property TypeName As String
        Get
            Dim ret = Extension
            If ret = "" Then ret = Path.Ext
            Return ret.TrimStart("."c).ToUpper.Replace("SUP", "PGS").Replace("IDX", "VobSub")
        End Get
    End Property
End Class

Public Enum AutoCheckMode
    None
    SingleClick
    DoubleClick
End Enum

Public Class ListViewEx
    Inherits ListView

    Property ItemCheckProperty As String

    <DllImport("user32.dll", SetLastError:=True)>
    Public Shared Function GetWindowLong(hWnd As IntPtr, nIndex As Integer) As Integer
    End Function

    <DefaultValue(GetType(AutoCheckMode), "DoubleClick")>
    Property AutoCheckMode As AutoCheckMode = AutoCheckMode.DoubleClick

    Function VScrollVisibel() As Boolean
        Const GWL_STYLE = -16
        Const WS_VSCROLL = &H200000
        Dim wndStyle = GetWindowLong(Handle, GWL_STYLE)
        Return (wndStyle And WS_VSCROLL) <> 0
    End Function

    Sub EnableListBoxMode()
        View = View.Details
        FullRowSelect = True
        Columns.Add("")
        HeaderStyle = ColumnHeaderStyle.None
        AddHandler Layout, Sub() Columns(0).Width = Width - 4 - If(VScrollVisibel(), SystemInformation.VerticalScrollBarWidth, 0)
        AddHandler HandleCreated, Sub() Columns(0).Width = Width - 4 - If(VScrollVisibel(), SystemInformation.VerticalScrollBarWidth, 0)
    End Sub

    Sub SendMessageHideFocus()
        Const UIS_SET = 1, UISF_HIDEFOCUS = &H1, WM_CHANGEUISTATE = &H127
        Native.SendMessage(Handle, WM_CHANGEUISTATE, MAKEWPARAM(UIS_SET, UISF_HIDEFOCUS), 0)
    End Sub

    Private Function MAKEWPARAM(low As Int32, high As Int32) As Int32
        Return (low And &HFFFF) Or (high << 16)
    End Function

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case &H203 'WM_LBUTTONDBLCLK
                If CheckBoxes AndAlso AutoCheckMode <> AutoCheckMode.DoubleClick Then
                    OnDoubleClick(Nothing)
                    Exit Sub
                End If
            Case &H201 'WM_LBUTTONDOWN
                If CheckBoxes AndAlso AutoCheckMode = AutoCheckMode.SingleClick Then
                    Dim pos = ClientMousePos
                    Dim item = GetItemAt(pos.X, pos.Y)

                    If Not item Is Nothing Then
                        Dim itemBounds = item.GetBounds(ItemBoundsPortion.Entire)

                        If pos.X > itemBounds.Left + itemBounds.Height Then
                            item.Checked = Not item.Checked
                        End If
                    End If
                End If
        End Select

        MyBase.WndProc(m)
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        Native.SetWindowTheme(Handle, "explorer", Nothing)

        If ItemCheckProperty <> "" Then
            AddHandler ItemCheck, Sub(sender As Object, e2 As ItemCheckEventArgs)
                                      Items(e2.Index).Tag.GetType.GetProperty(ItemCheckProperty).SetValue(Items(e2.Index).Tag, e2.NewValue = CheckState.Checked)
                                  End Sub
        End If
    End Sub
End Class

<Serializable()>
Public Class Language
    Implements IComparable(Of Language)

    <NonSerialized>
    Public IsCommon As Boolean

    Sub New()
        Me.New("")
    End Sub

    Sub New(ci As CultureInfo, Optional isCommon As Boolean = False)
        Me.IsCommon = isCommon
        CultureInfoValue = ci
    End Sub

    Sub New(twoLetterCode As String, Optional isCommon As Boolean = False)
        Try
            Me.IsCommon = isCommon

            Select Case twoLetterCode
                Case "iw"
                    twoLetterCode = "he"
                Case "jp"
                    twoLetterCode = "ja"
            End Select

            CultureInfoValue = New CultureInfo(twoLetterCode)
        Catch ex As Exception
            CultureInfoValue = CultureInfo.InvariantCulture
        End Try
    End Sub

    Private CultureInfoValue As CultureInfo

    ReadOnly Property CultureInfo() As CultureInfo
        Get
            Return CultureInfoValue
        End Get
    End Property

    ReadOnly Property TwoLetter() As String
        Get
            Return CultureInfo.TwoLetterISOLanguageName
        End Get
    End Property

    ReadOnly Property Name() As String
        Get
            If CultureInfo.TwoLetterISOLanguageName = "iv" Then
                Return "Undetermined"
            Else
                Return CultureInfo.EnglishName
            End If
        End Get
    End Property

    Private Shared LanguagesValue As List(Of Language)

    Shared ReadOnly Property Languages() As List(Of Language)
        Get
            If LanguagesValue Is Nothing Then
                Dim l As New List(Of Language)

                l.Add(New Language("en", True))
                l.Add(New Language("es", True))
                l.Add(New Language("de", True))
                l.Add(New Language("fr", True))
                l.Add(New Language("it", True))
                l.Add(New Language("ru", True))
                l.Add(New Language("zh", True))
                l.Add(New Language("hi", True))
                l.Add(New Language("ja", True))
                l.Add(New Language("pt", True))
                l.Add(New Language("ar", True))
                l.Add(New Language("bn", True))
                l.Add(New Language("pa", True))
                l.Add(New Language("ms", True))
                l.Add(New Language("ko", True))

                l.Add(New Language(CultureInfo.InvariantCulture, True))

                Dim current = l.Where(Function(a) a.TwoLetter = CultureInfo.CurrentCulture.TwoLetterISOLanguageName).FirstOrDefault

                If current Is Nothing Then l.Add(Language.Current)

                l.Sort()

                Dim l2 As New List(Of Language)

                For Each i In CultureInfo.GetCultures(CultureTypes.NeutralCultures)
                    l2.Add(New Language(i))
                Next

                l2.Sort()
                l.AddRange(l2)
                LanguagesValue = l
            End If

            Return LanguagesValue
        End Get
    End Property

    Shared ReadOnly Property Current As Language
        Get
            Return New Language(CultureInfo.CurrentCulture, True)
        End Get
    End Property

    Overrides Function ToString() As String
        Return Name
    End Function

    Function CompareTo(other As Language) As Integer Implements System.IComparable(Of Language).CompareTo
        Return Name.CompareTo(other.Name)
    End Function

    Overrides Function Equals(o As Object) As Boolean
        If TypeOf o Is Language Then
            Return CultureInfo.Equals(DirectCast(o, Language).CultureInfo)
        End If
    End Function
End Class

<Serializable>
Public Class AudioStream
    Property BitDepth As Integer
    Property Bitrate As Integer
    Property BitrateCore As Integer
    Property Channels As Integer
    Property ChannelsCore As Integer
    Property Codec As String
    Property CodecString As String
    Property Delay As Integer
    Property Format As String
    Property FormatProfile As String 'was only field to show DTS MA
    Property ID As Integer
    Property Language As Language
    Property SamplingRate As Integer
    Property StreamOrder As Integer
    Property Title As String
    Property Enabled As Boolean = True

    ReadOnly Property Name As String
        Get
            Dim sb As New StringBuilder()
            sb.Append("ID" & (StreamOrder + 1))

            If CodecString <> "" Then
                Select Case CodecString
                    Case "MPEG-1 Audio layer 2"
                        sb.Append(" MP2")
                    Case "MPEG-1 Audio layer 3"
                        sb.Append(" MP3")
                    Case "TrueHD / AC3"
                        sb.Append(" THD+AC3")
                    Case "AC3+"
                        sb.Append(" E-AC3")
                    Case Else
                        If Codec = "TrueHD / AC3" Then
                            sb.Append(" THD+AC3")
                        ElseIf FormatProfile = "MA / Core" Then
                            sb.Append(" DTS MA/Core")
                        ElseIf FormatProfile = "HRA / Core" Then
                            sb.Append(" DTS HRA/Core")
                        Else
                            sb.Append(" " + CodecString)
                        End If
                End Select
            End If

            If ChannelsCore > 0 Then
                sb.Append(" " & Channels & "/" & ChannelsCore & "ch")
            Else
                sb.Append(" " & Channels & "ch")
            End If

            If BitDepth > 0 Then sb.Append(" " & BitDepth & "Bit")
            If SamplingRate > 0 Then sb.Append(" " & SamplingRate & "Hz")

            If BitrateCore > 0 Then
                sb.Append(" " & If(Bitrate = 0, "?", Bitrate.ToString) & "/" & BitrateCore & "Kbps")
            ElseIf Bitrate > 0 Then
                sb.Append(" " & Bitrate & "Kbps")
            End If

            If Delay <> 0 Then sb.Append(" " & Delay & "ms")
            If Language.TwoLetter <> "iv" Then sb.Append(" " + Language.Name)
            If Title <> "" AndAlso Title <> " " Then sb.Append(" " + Title)

            Return sb.ToString
        End Get
    End Property

    ReadOnly Property Extension() As String
        Get
            Select Case CodecString
                Case "AAC LC", "AAC LC-SBR", "AAC LC-SBR-PS"
                    Return ".aac"
                Case "AC3"
                    Return ".ac3"
                Case "DTS"
                    Return ".dts"
                Case "DTS-HD"
                    If FormatProfile = "MA / Core" Then
                        Return ".dtsma"
                    ElseIf FormatProfile = "HRA / Core" Then
                        Return ".dtshr"
                    Else
                        Return ".dtshd"
                    End If
                Case "PCM", "ADPCM"
                    Return ".wav"
                Case "MPEG-1 Audio layer 2"
                    Return ".mp2"
                Case "MPEG-1 Audio layer 3"
                    Return ".mp3"
                Case "TrueHD / AC3"
                    Return ".thd"
                Case "Flac"
                    Return ".flac"
                Case "Vorbis"
                    Return ".ogg"
                Case "Opus"
                    Return ".opus"
                Case "TrueHD"
                    Return ".thd"
                Case "AC3+"
                    Return ".eac3"
                Case Else
                    Return ".mka"
            End Select
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class

Public Class MediaInfo
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
    Private Shared Function MediaInfo_Get(Handle As IntPtr,
                                          StreamKind As MediaInfoStreamKind,
                                          StreamNumber As Integer, Parameter As String,
                                          KindOfInfo As MediaInfoInfoKind,
                                          KindOfSearch As MediaInfoInfoKind) As IntPtr
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

            Dim mipath = "MediaInfo.dll"

            If Not File.Exists(mipath) Then
                Dim names = Registry.CurrentUser.GetValueNames("SOFTWARE\StaxRip\SettingsLocation")

                If Not names.NothingOrEmpty AndAlso Directory.Exists(names(0)) Then
                    mipath = names(0).AppendSeparator + "Apps\MediaInfo\MediaInfo.dll"
                End If
            End If

            Native.LoadLibrary(mipath)
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
            Dim offset As Integer

            If count > 0 Then
                For i = 0 To count - 1
                    Dim at As New AudioStream

                    Dim streamOrder = GetAudio(i, "StreamOrder")
                    If Not streamOrder.IsInt Then streamOrder = (i + 1).ToString
                    at.StreamOrder = streamOrder.ToInt + offset

                    Dim id = GetAudio(i, "ID")
                    If Not id.IsInt Then id = (i + 2).ToString
                    at.ID = id.ToInt + offset

                    at.Codec = GetAudio(i, "Codec")
                    If at.Codec = "TrueHD / AC3" Then offset += 1

                    at.SamplingRate = GetAudio(i, "SamplingRate").ToInt
                    at.BitDepth = GetAudio(i, "BitDepth").ToInt
                    at.CodecString = GetAudio(i, "Codec/String")
                    at.Format = GetAudio(i, "Format")
                    at.FormatProfile = GetAudio(i, "Format_Profile")
                    at.Title = GetAudio(i, "Title").Trim

                    If at.Title.Contains("IsoMedia") OrElse at.Title.Contains("GPAC") OrElse at.Title.Contains("PID ") OrElse
                            at.Title.EqualsAny("Surround 7.1", "Surround 5.1", "Stereo", "3/2+1", "2/0") Then

                        at.Title = ""
                    End If

                    If Not Filepath.IsValidFileSystemName(at.Title) Then
                        at.Title = Filepath.RemoveIllegal(at.Title)
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
            Dim offset As Integer

            If count > 0 Then
                For Each i In AudioStreams
                    If i.Codec = "TrueHD / AC3" Then offset += 1
                Next

                For i = 0 To count - 1
                    Dim s As New Subtitle(New Language(GetInfo(MediaInfoStreamKind.Text, i, "Language")))
                    Dim streamOrder = GetInfo(MediaInfoStreamKind.Text, i, "StreamOrder")

                    If streamOrder <> "" Then
                        If streamOrder.Contains("-") Then
                            s.StreamOrder = streamOrder.Right("-").ToInt + offset
                        Else
                            s.StreamOrder = streamOrder.ToInt
                        End If
                    End If

                    s.ID = GetInfo(MediaInfoStreamKind.Text, i, "ID").ToInt
                    s.Title = GetInfo(MediaInfoStreamKind.Text, i, "Title").Trim
                    s.CodecString = GetInfo(MediaInfoStreamKind.Text, i, "Codec/String")
                    s.Format = GetInfo(MediaInfoStreamKind.Text, i, "Format")
                    s.Size = GetInfo(MediaInfoStreamKind.Text, i, "StreamSize").ToInt

                    ret.Add(s)
                Next
            End If

            Return ret
        End Get
    End Property

    Shared Function GetAudioStreams(path As String) As List(Of AudioStream)
        Return GetMediaInfo(path).AudioStreams
    End Function

    Shared Function GetSubtitles(path As String) As List(Of Subtitle)
        Return GetMediaInfo(path).Subtitles
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

Public Class VideoStream
    Property Format As String
    Property StreamOrder As Integer

    ReadOnly Property Extension() As String
        Get
            Select Case Format
                Case "MPEG Video"
                    Return ".mpg"
                Case "AVC"
                    Return ".h264"
                Case "MPEG-4 Visual"
                    Return ".avi"
                Case "HEVC"
                    Return "h265"
            End Select
        End Get
    End Property
End Class

Public Class Native

#Region "Function"

#Region "user32.dll"

    <DllImport("user32.dll")>
    Shared Function SetProcessDPIAware() As Boolean
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function SendMessage(hWnd As IntPtr,
                                Msg As Int32,
                                wParam As Integer,
                                lParam As Integer) As IntPtr
    End Function

#End Region

#Region "kernel32.dll"

    <DllImport("kernel32.dll", CharSet:=CharSet.Unicode)>
    Shared Function LoadLibrary(path As String) As IntPtr
    End Function

#End Region

    <DllImport("uxtheme.dll", CharSet:=CharSet.Unicode)>
    Shared Function SetWindowTheme(hWnd As IntPtr,
                                   pszSubAppName As String,
                                   pszSubIdList As String) As Integer
    End Function

#End Region

End Class

Class DirPath
    Inherits PathBase

    Shared Function TrimTrailingSeparator(path As String) As String
        If path = "" Then Return ""

        If path.EndsWith(Separator) AndAlso Not path.EndsWith(":\") Then
            Return path.TrimEnd(Separator)
        End If

        Return path
    End Function

    Shared Function GetParent(path As String) As String
        If path = "" Then Return ""
        Dim temp = TrimTrailingSeparator(path)
        If temp.Contains(Separator) Then path = temp.LeftLast(Separator) + Separator
        Return path
    End Function
End Class

Public Class PathBase
    Shared ReadOnly Property Separator() As Char
        Get
            Return Path.DirectorySeparatorChar
        End Get
    End Property

    Shared Function AppendSeparator(path As String) As String
        If path = "" Then Return ""
        If path.EndsWith(Separator) Then Return path
        Return path + Separator
    End Function

    Shared Function IsValidFileSystemName(name As String) As Boolean
        If name = "" Then Return False
        Dim chars = """*/:<>?\|".ToCharArray

        For Each i In name.ToCharArray
            If chars.Contains(i) Then Return False
            If Convert.ToInt32(i) < 32 Then Return False
        Next

        Return True
    End Function

    Shared Function RemoveIllegal(name As String) As String
        If name = "" Then Return ""
        Dim chars = """*/:<>?\|".ToCharArray

        For Each i In name.ToCharArray
            If chars.Contains(i) Then name = name.Replace(i, "_")
        Next

        For x = 1 To 31
            If name.Contains(Convert.ToChar(x)) Then name = name.Replace(Convert.ToChar(x), "_"c)
        Next

        Return name
    End Function
End Class

Public Class Filepath
    Inherits PathBase

    Private Value As String

    Sub New(path As String)
        Value = path
    End Sub

    Shared Function GetDir(path As String) As String
        If path = "" Then Return ""
        If path.Contains("\") Then path = path.LeftLast("\") + "\"
        Return path
    End Function

    Shared Function GetDirAndBase(path As String) As String
        Return GetDir(path) + GetBase(path)
    End Function

    Shared Function GetExtFull(filepath As String) As String
        Return GetExt(filepath, True)
    End Function

    Shared Function GetExt(filepath As String) As String
        Return GetExt(filepath, False)
    End Function

    Shared Function GetExt(filepath As String, dot As Boolean) As String
        If filepath = "" Then Return ""
        Dim chars = filepath.ToCharArray

        For x = filepath.Length - 1 To 0 Step -1
            If chars(x) = Separator Then Return ""
            If chars(x) = "."c Then Return filepath.Substring(x + If(dot, 0, 1)).ToLower
        Next

        Return ""
    End Function

    Shared Function GetBase(path As String) As String
        If path = "" Then Return ""
        Dim ret = path
        If ret.Contains(Separator) Then ret = ret.RightLast(Separator)
        If ret.Contains(".") Then ret = ret.LeftLast(".")
        Return ret
    End Function
End Class