
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports StaxRip.UI

Public Class DirectFrameServer
    Implements IDisposable, IFrameServer

    Property Info As ServerInfo Implements IFrameServer.Info

    Private NativeServer As INativeFrameServer

    Sub New(path As String)
        If path.Ext = "avs" Then
            Environment.SetEnvironmentVariable("AviSynthDLL", Package.AviSynth.Path)
            NativeServer = CreateAviSynthServer()
        Else
            NativeServer = CreateVapourSynthServer()
        End If

        NativeServer.OpenFile(path)
        Info = Marshal.PtrToStructure(Of ServerInfo)(NativeServer.GetInfo())
    End Sub

    ReadOnly Property [Error] As String Implements IFrameServer.Error
        Get
            Return Marshal.PtrToStringUni(NativeServer.GetError())
        End Get
    End Property

    ReadOnly Property FrameRate As Double Implements IFrameServer.FrameRate
        Get
            Return Info.FrameRateNum / Info.FrameRateDen
        End Get
    End Property

    Function GetFrame(
        position As Integer,
        ByRef data As IntPtr,
        ByRef pitch As Integer) As Integer Implements IFrameServer.GetFrame

        Return NativeServer.GetFrame(position, data, pitch)
    End Function

    <DllImport("FrameServer.dll")>
    Shared Function CreateAviSynthServer() As INativeFrameServer
    End Function

    <DllImport("FrameServer.dll")>
    Shared Function CreateVapourSynthServer() As INativeFrameServer
    End Function

    Sub Dispose() Implements IDisposable.Dispose
        If Not NativeServer Is Nothing Then
            Marshal.ReleaseComObject(NativeServer)
            NativeServer = Nothing
        End If
    End Sub
End Class

Public Interface IFrameServer
    Inherits IDisposable

    Property Info As ServerInfo
    ReadOnly Property [Error] As String
    ReadOnly Property FrameRate As Double
    Function GetFrame(position As Integer, ByRef data As IntPtr, ByRef pitch As Integer) As Integer
End Interface

<Guid("A933B077-7EC2-42CC-8110-91DE21116C1A")>
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
Public Interface INativeFrameServer
    <PreserveSig> Function OpenFile(file As String) As Integer
    <PreserveSig> Function GetFrame(position As Integer, ByRef data As IntPtr, ByRef pitch As Integer) As Integer
    <PreserveSig> Function GetInfo() As IntPtr
    <PreserveSig> Function GetError() As IntPtr
End Interface

Public Structure ServerInfo
    Public Width As Integer
    Public Height As Integer
    Public FrameRateNum As Integer
    Public FrameRateDen As Integer
    Public FrameCount As Integer
    Public ColorSpace As ColorSpace

    Function GetInfoText(position As Integer) As String
        Dim rate = FrameRateNum / FrameRateDen

        Dim lengthtDate = Date.Today.AddSeconds(FrameCount / rate)
        Dim dateFormat = If(lengthtDate.Hour = 0, "mm:ss.fff", "HH:mm:ss.fff")
        Dim frames = FrameCount.ToString
        Dim len = lengthtDate.ToString(dateFormat)

        If position > -1 Then
            frames = position & " of " & FrameCount
            Dim currentDate = Date.Today.AddSeconds(position / rate)
            len = currentDate.ToString(dateFormat) + " of " + lengthtDate.ToString(dateFormat)
        End If

        Return "Width     : " & Width & BR &
               "Height    : " & Height & BR &
               "Frames    : " + frames + BR +
               "Time      : " + len + BR +
               "Framerate : " + rate.ToInvariantString.Shorten(9) + " (" & FrameRateNum & "/" & FrameRateDen & ")" + BR +
               "Format    : " + ColorSpace.ToString.Replace("_", "")
    End Function
End Structure

Public Enum ColorSpace
    Unknown = 0
    BGR24 = 1342177281
    BGR32 = 1342177282
    RGBP8 = -1879048191
    RGBP10 = -1878720511
    RGBP12 = -1878654975
    RGBP14 = -1878589439
    RGBP16 = -1878982655
    Y8 = -536870912
    Y10 = -536543232
    Y12 = -536477696
    Y14 = -536412160
    Y16 = -536805376
    Y32 = -536739840
    YUV410P8 = -1610612471
    YUV411P8 = -1610611959
    YUV420P8 = -1610612720
    YUV420P8_ = -1610612728
    YUV420P10 = -1610285048
    YUV420P12 = -1610219512
    YUV420P14 = -1610153976
    YUV420P16 = -1610547192
    YUV420PS = -1610481656
    YUV422P8 = -1610611960
    YUV422P10 = -1610284280
    YUV422P12 = -1610218744
    YUV422P14 = -1610153208
    YUV422P16 = -1610546424
    YUV422PS = -1610480888
    YUV444P8 = -1610611957
    YUV444P10 = -1610284277
    YUV444P12 = -1610218741
    YUV444P14 = -1610153205
    YUV444P16 = -1610546421
    YUV444PS = -1610480885
    YUY2 = 1610612740
End Enum

Public Class FrameServerFactory
    Shared Function Create(path As String) As IFrameServer
        g.AddToPath(
            Package.Python.Directory,
            Package.VapourSynth.Directory,
            Package.AviSynth.Directory,
            Package.FFTW.Directory,
            Folder.Startup + "Apps\Support\VC")

        If (path.Ext = "avs" AndAlso s.AviSynthMode = FrameServerMode.VFW) OrElse
           (path.Ext = "vpy" AndAlso s.VapourSynthMode = FrameServerMode.VFW) Then

            Return New VfwFrameServer(path)
        Else
            Return New DirectFrameServer(path)
        End If
    End Function
End Class

Public Class VfwFrameServer
    Implements IDisposable, IFrameServer

    Property Info As ServerInfo Implements IFrameServer.Info

    Private AviFile As IntPtr
    Private FrameObject As IntPtr
    Private AviStream As IntPtr

    Sub New(path As String)
        Try
            Me.Error = ""
            AVIFileInit()

            If AVIFileOpen(AviFile, path, 32, IntPtr.Zero) <> 0 Then
                Throw New Exception("AVIFileOpen failed to execute")
            End If

            If AVIFileGetStream(AviFile, AviStream, mmioStringToFOURCC("vids", 0), 0) <> 0 Then
                Throw New Exception("AVIFileGetStream failed to execute")
            End If

            Dim info2 As ServerInfo
            info2.FrameCount = AVIStreamLength(AviStream)

            If info2.FrameCount = 240 Then
                Dim clipInfo = TryCast(Marshal.GetObjectForIUnknown(AviFile), IAvisynthClipInfo)

                If Not clipInfo Is Nothing Then
                    Dim ptr As IntPtr

                    If clipInfo.GetError(ptr) = 0 Then
                        Me.Error = Marshal.PtrToStringAnsi(ptr)
                    End If

                    Marshal.ReleaseComObject(clipInfo)

                    If Me.Error <> "" Then
                        Throw New Exception(Me.Error)
                    End If
                End If
            End If

            Dim aviInfo As New _AVISTREAMINFO()

            If AVIStreamInfo(AviStream, aviInfo, Marshal.SizeOf(aviInfo)) <> 0 Then
                Throw New Exception("AVIStreamInfo failed to execute")
            End If

            info2.FrameRateDen = CInt(aviInfo.dwScale)
            info2.FrameRateNum = CInt(aviInfo.dwRate)
            info2.Width = aviInfo.rcFrame.Right
            info2.Height = aviInfo.rcFrame.Bottom
            info2.ColorSpace = GetColorSpace(aviInfo.fccHandler)
            Info = info2
        Catch ex As Exception
            Me.Error = ex.Message
            Dispose()
        End Try
    End Sub

    ReadOnly Property [Error] As String Implements IFrameServer.Error

    ReadOnly Property FrameRate As Double Implements IFrameServer.FrameRate
        Get
            Return Info.FrameRateNum / Info.FrameRateDen
        End Get
    End Property

    Function GetFrame(
        position As Integer,
        ByRef data As IntPtr,
        ByRef pitch As Integer) As Integer Implements IFrameServer.GetFrame

        If FrameObject = IntPtr.Zero Then
            FrameObject = AVIStreamGetFrameOpen(AviStream, 1)
        End If

        If FrameObject <> IntPtr.Zero Then
            data = AVIStreamGetFrame(FrameObject, position)

            If data <> IntPtr.Zero Then
                data += 40 'BITMAPINFOHEADER size
                pitch = (((Info.Width * 32) + 31) And Not 31) >> 3
                Return 0 'S_OK
            End If
        End If

        Return &H80004005 'E_FAIL
    End Function

    Function GetColorSpace(fcc As UInt32) As ColorSpace
        Select Case FccToString(fcc)
            Case "Y416"
                Return ColorSpace.YUV444P16
            Case "Y410"
                Return ColorSpace.YUV444P10
            Case "YV24"
                Return ColorSpace.YUV444P8
            Case "P216"
                Return ColorSpace.YUV422P16
            Case "P210", "v210", "V210"
                Return ColorSpace.YUV422P10
            Case "YV16"
                Return ColorSpace.YUV422P8
            Case "P016"
                Return ColorSpace.YUV420P16
            Case "P010"
                Return ColorSpace.YUV420P10
            Case "YV12"
                Return ColorSpace.YUV420P8
            Case "Y41B"
                Return ColorSpace.YUV411P8
            Case "YVU9"
                Return ColorSpace.YUV410P8
            Case "Y800"
                Return ColorSpace.Y8
            Case "YUY2"
                Return ColorSpace.YUY2
            Case "b64a"
                Return ColorSpace.RGBP16
            Case "r210"
                Return ColorSpace.RGBP10
            Case "DIB "
                Return ColorSpace.BGR32
        End Select
    End Function

    Function FccToString(fourcc As UInt32) As String
        Return Convert.ToChar((fourcc And &HFF)) +
               Convert.ToChar((fourcc And &HFF00) >> 8) +
               Convert.ToChar((fourcc And &HFF0000) >> 16) +
               Convert.ToChar((fourcc And &HFF000000) >> 24)
    End Function

    <Guid("E6D6B708-124D-11D4-86F3-DB80AFD98778"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Interface IAvisynthClipInfo
        Function GetError(ByRef msg As IntPtr) As Integer
        Function GetParity(value As Integer) As Byte
        Function IsFieldBased() As Byte
    End Interface

    <DllImport("avifil32.dll")>
    Shared Sub AVIFileInit()
    End Sub

    <DllImport("avifil32.dll", CharSet:=CharSet.Unicode)>
    Shared Function AVIFileOpen(
        ByRef ppfile As IntPtr, szFile As String, uMode As Integer, pclsidHandler As IntPtr) As Integer
    End Function

    <DllImport("avifil32.dll")>
    Shared Function AVIFileGetStream(
        pfile As IntPtr, ByRef ppavi As IntPtr, fccType As UInteger, lParam As Integer) As Integer
    End Function

    <DllImport("avifil32.dll")>
    Shared Function AVIStreamLength(pavi As IntPtr) As Integer
    End Function

    <DllImport("avifil32.dll", CharSet:=CharSet.Unicode)>
    Shared Function AVIStreamInfo(pAVIStream As IntPtr, ByRef psi As _AVISTREAMINFO, lSize As Integer) As Integer
    End Function

    <DllImport("avifil32.dll")>
    Shared Function AVIStreamGetFrameOpen(pAVIStream As IntPtr, lpbiWanted As Integer) As IntPtr
    End Function

    <DllImport("avifil32.dll")>
    Shared Function AVIStreamGetFrame(pGetFrameObj As IntPtr, lPos As Integer) As IntPtr
    End Function

    <DllImport("avifil32.dll")>
    Shared Function AVIStreamGetFrameClose(pGetFrameObj As IntPtr) As Integer
    End Function

    <DllImport("avifil32.dll")>
    Shared Function AVIStreamRelease(aviStream As IntPtr) As Integer
    End Function

    <DllImport("avifil32.dll")>
    Shared Function AVIFileRelease(pfile As IntPtr) As Integer
    End Function

    <DllImport("avifil32.dll")>
    Shared Sub AVIFileExit()
    End Sub

    <DllImport("winmm.dll")>
    Shared Function mmioStringToFOURCC(sz As String, uFlags As Integer) As UInteger
    End Function

    Structure BITMAPINFOHEADER
        Public biSize As UInt32
        Public biWidth As Int32
        Public biHeight As Int32
        Public biPlanes As Int16
        Public biBitCount As Int16
        Public biCompression As UInt32
        Public biSizeImage As UInt32
        Public biXPelsPerMeter As Int32
        Public biYPelsPerMeter As Int32
        Public biClrUsed As UInt32
        Public biClrImportant As UInt32
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Structure _AVISTREAMINFO
        Public fccType As UInt32
        Public fccHandler As UInt32
        Public dwFlags As UInt32
        Public dwCaps As UInt32
        Public wPriority As UInt16
        Public wLanguage As UInt16
        Public dwScale As UInt32
        Public dwRate As UInt32
        Public dwStart As UInt32
        Public dwLength As UInt32
        Public dwInitialFrames As UInt32
        Public dwSuggestedBufferSize As UInt32
        Public dwQuality As UInt32
        Public dwSampleSize As UInt32
        Public rcFrame As Native.RECT
        Public dwEditCount As UInt32
        Public dwFormatChangeCount As UInt32
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=64)>
        Public szName As String
    End Structure

    Private WasDisposed As Boolean

    Sub Dispose() Implements IDisposable.Dispose
        If Not WasDisposed Then
            If FrameObject <> IntPtr.Zero Then
                AVIStreamGetFrameClose(FrameObject)
            End If

            If AviStream <> IntPtr.Zero Then
                AVIStreamRelease(AviStream)
            End If

            If AviFile <> IntPtr.Zero Then
                AVIFileRelease(AviFile)
            End If

            AVIFileExit()
            WasDisposed = True
        End If
    End Sub
End Class

Public Class FrameServerHelp
    Shared Function GetSynthPath() As String
        Return If(IsAviSynth(), Package.AviSynth.Path, Package.VapourSynth.Directory + "VSScript.dll")
    End Function

    Shared Function GetAviSynthInstallPath() As String
        Dim ret = Registry.ClassesRoot.GetString("CLSID\{E6D6B700-124D-11D4-86F3-DB80AFD98778}\InProcServer32", Nothing)

        If ret = "AviSynth.dll" Then
            ret = Folder.System + "AviSynth.dll"
        End If

        Return ret
    End Function

    Shared Function GetVapourSynthInstallPath() As String
        For Each key In {Registry.CurrentUser, Registry.LocalMachine}
            Dim dllPath = key.GetString("Software\VapourSynth", "VapourSynthDLL")

            If File.Exists(dllPath) Then
                Return dllPath
            End If
        Next
    End Function

    Shared Function IsAviSynthPortable() As Boolean
        Return s.AviSynthMode = FrameServerMode.Portable
    End Function

    Shared Function IsVapourSynthPortable() As Boolean
        Return s.VapourSynthMode = FrameServerMode.Portable
    End Function

    Shared Function IsPortable() As Boolean
        If (IsAviSynth() AndAlso IsAviSynthPortable()) OrElse
            (IsVapourSynth() AndAlso IsVapourSynthPortable()) Then

            Return True
        End If
    End Function

    Shared Function IsAviSynthSystemInstalled() As Boolean
        Return (Folder.System + "AviSynth.dll").FileExists
    End Function

    Shared Function IsAviSynth() As Boolean
        Return p.Script.IsAviSynth
    End Function

    Shared Function IsVapourSynth() As Boolean
        Return p.Script.Engine = ScriptEngine.VapourSynth
    End Function

    Shared Function IsVfwUsed() As Boolean
        Return (IsAviSynth() AndAlso s.AviSynthMode = FrameServerMode.VFW) OrElse
            (IsVapourSynth() AndAlso s.VapourSynthMode = FrameServerMode.VFW)
    End Function

    Shared Function IsffmpegUsed() As Boolean
        If "avs".EqualsAny(p.Audio0.File.Ext, p.Audio1.File.Ext) Then
            Return True
        End If

        If p.VideoEncoder.GetCommandLine(True, True).ContainsEx("ffmpeg") Then
            Return True
        End If
    End Function

    Shared Sub MoveFiles(srcDir As String, targetDir As String, fileNames As String())
        For Each name In fileNames
            FileHelp.Move(srcDir + name, targetDir + name)
        Next
    End Sub

    Shared Sub AviSynthToolPath()
        If Not IsAviSynth() Then
            Exit Sub
        End If

        If IsffmpegUsed() Then
            If IsAviSynthSystemInstalled() AndAlso Not IsAviSynthPortable() Then
                Dim targetFolder = Folder.Startup + "Apps\Encoders\ffmpeg\"
                FolderHelp.Create(targetFolder)
                MoveFiles(Package.ffmpeg.Directory, targetFolder, {Package.ffmpeg.Filename, "ffmpeg Help.txt"})
            Else
                MoveFiles(Package.ffmpeg.Directory, Package.AviSynth.Directory, {Package.ffmpeg.Filename, "ffmpeg Help.txt"})
            End If
        End If

        If TypeOf p.VideoEncoder Is x264Enc Then
            If IsAviSynthPortable() Then
                If Not Package.x264.Path.Contains("FrameServer\AviSynth") AndAlso Not CommandLineContainsAvsDllPath() Then
                    MoveFiles(Package.x264.Directory, Package.AviSynth.Directory, {Package.x264.Filename, "x264 Help.txt"})
                End If
            Else
                If Package.x264.Path.Contains("FrameServer\AviSynth") Then
                    Dim targetFolder = Folder.Startup + "Apps\Encoders\x264\"
                    FolderHelp.Create(targetFolder)
                    MoveFiles(Package.x264.Directory, targetFolder, {Package.x264.Filename, "x264 Help.txt"})
                End If
            End If
        End If

        If TypeOf p.VideoEncoder Is x265Enc Then
            If IsAviSynthPortable() Then
                If Not Package.x265.Path.Contains("FrameServer\AviSynth") AndAlso Not CommandLineContainsAvsDllPath() Then
                    MoveFiles(Package.x265.Directory, Package.AviSynth.Directory, {Package.x265.Filename, "x265 Help.txt"})
                End If
            Else
                If Package.x265.Path.Contains("FrameServer\AviSynth") Then
                    Dim targetFolder = Folder.Startup + "Apps\Encoders\x265\"
                    FolderHelp.Create(targetFolder)
                    MoveFiles(Package.x265.Directory, targetFolder, {Package.x265.Filename, "x265 Help.txt"})
                End If
            End If
        End If
    End Sub

    Shared Function CommandLineContainsAvsDllPath() As Boolean
        Return p.VideoEncoder.GetCommandLine(True, True).ContainsEx(Package.AviSynth.Path)
    End Function
End Class

Public Enum FrameServerMode
    <DispName("Use portable directly")> Portable
    <DispName("Use installed directly")> Installed
    <DispName("Use installed via VFW")> VFW
End Enum
