
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Runtime.InteropServices

Module Module1
    Sub Main()
        Try
            Dim args = Environment.GetCommandLineArgs()

            If args.Length = 1 Then
                Console.WriteLine("AutoCrop <avs/vpy path> <frame count> <VFW>")
                Exit Sub
            End If

            Dim scriptPath = args(1)
            Dim count = CInt(args(2))
            Dim vfw = args(3) = "1"

            Using server = FrameServerFactory.Create(scriptPath, vfw)
                Dim len = server.Info.FrameCount \ (count + 1)
                Dim crops(count - 1) As AutoCrop
                Dim pos = 0

                For i = 0 To count - 1
                    Console.WriteLine($"Progress: {(i / count * 100):f0}%")
                    pos += len

                    Using bmp = BitmapUtil.CreateBitmap(server, pos)
                        crops(i) = AutoCrop.Start(bmp.Clone(New Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format32bppRgb), pos)
                    End Using
                Next

                Dim leftCrops = crops.SelectMany(Function(arg) arg.Left).OrderBy(Function(arg) arg)
                Dim left = leftCrops(leftCrops.Count \ 10)

                Dim topCrops = crops.SelectMany(Function(arg) arg.Top).OrderBy(Function(arg) arg)
                Dim top = topCrops(topCrops.Count \ 10)

                Dim rightCrops = crops.SelectMany(Function(arg) arg.Right).OrderBy(Function(arg) arg)
                Dim right = rightCrops(rightCrops.Count \ 10)

                Dim bottomCrops = crops.SelectMany(Function(arg) arg.Bottom).OrderBy(Function(arg) arg)
                Dim bottom = bottomCrops(bottomCrops.Count \ 10)

                Console.WriteLine($"{left},{top},{right},{bottom}")
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
            Environment.ExitCode = 1
        End Try
    End Sub

    <DllImport("kernel32.dll", CharSet:=CharSet.Unicode)>
    Function LoadLibrary(path As String) As IntPtr
    End Function
End Module

Public Class AutoCrop
    Public Top As Integer()
    Public Bottom As Integer()
    Public Left As Integer()
    Public Right As Integer()

    Shared Function Start(bmp As Bitmap, position As Integer) As AutoCrop
        Dim ret As New AutoCrop
        Dim u = BitmapUtil.Create(bmp)
        Dim max = 20
        Dim xCount = 20
        Dim yCount = 20

        Dim xValues(xCount) As Integer

        For x = 0 To xCount
            xValues(x) = CInt(bmp.Width / (xCount + 1) * x)
        Next

        ret.Top = New Integer(xValues.Length - 1) {}
        ret.Bottom = New Integer(xValues.Length - 1) {}

        For xValue = 0 To xValues.Length - 1
            For y = 0 To u.BitmapData.Height \ 4
                If u.GetMax(xValues(xValue), y) < max Then
                    ret.Top(xValue) = y + 1
                Else
                    Exit For
                End If
            Next

            For y = u.BitmapData.Height - 1 To u.BitmapData.Height - u.BitmapData.Height \ 4 Step -1
                If u.GetMax(xValues(xValue), y) < max Then
                    ret.Bottom(xValue) = u.BitmapData.Height - y
                Else
                    Exit For
                End If
            Next
        Next

        Dim yValues(yCount) As Integer

        For x = 0 To yCount
            yValues(x) = CInt(bmp.Height / (yCount + 1) * x)
        Next

        ret.Left = New Integer(yValues.Length - 1) {}
        ret.Right = New Integer(yValues.Length - 1) {}

        For yValue = 0 To yValues.Length - 1
            For x = 0 To u.BitmapData.Width \ 4
                If u.GetMax(x, yValues(yValue)) < max Then
                    ret.Left(yValue) = x + 1
                Else
                    Exit For
                End If
            Next

            For x = u.BitmapData.Width - 1 To u.BitmapData.Width - u.BitmapData.Width \ 4 Step -1
                If u.GetMax(x, yValues(yValue)) < max Then
                    ret.Right(yValue) = u.BitmapData.Width - x
                Else
                    Exit For
                End If
            Next
        Next

        Return ret
    End Function
End Class

Public Class BitmapUtil
    Property Data As Byte()
    Property BitmapData As BitmapData

    Function GetPixel(x As Integer, y As Integer) As Color
        Dim pos = y * BitmapData.Stride + x * 4
        Return Color.FromArgb(Data(pos), Data(pos + 1), Data(pos + 2))
    End Function

    Function GetMax(x As Integer, y As Integer) As Integer
        Dim col = GetPixel(x, y)
        Dim max = Math.Max(col.R, col.G)
        Return Math.Max(max, col.B)
    End Function

    Shared Function Create(bmp As Bitmap) As BitmapUtil
        Dim util As New BitmapUtil
        Dim rect As New Rectangle(0, 0, bmp.Width, bmp.Height)
        util.BitmapData = bmp.LockBits(rect, Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat)
        Dim ptr = util.BitmapData.Scan0
        Dim bytesCount = Math.Abs(util.BitmapData.Stride) * bmp.Height
        util.Data = New Byte(bytesCount - 1) {}
        Marshal.Copy(ptr, util.Data, 0, bytesCount)
        bmp.UnlockBits(util.BitmapData)
        Return util
    End Function

    Shared Function CreateBitmap(server As IFrameServer, position As Integer) As Bitmap
        Dim pitch As Integer
        Dim data As IntPtr

        If server.GetFrame(position, data, pitch) = 0 Then
            Return New Bitmap(server.Info.Width, server.Info.Height,
                pitch, PixelFormat.Format32bppArgb, data)
        End If
    End Function
End Class

Public Class DirectFrameServer
    Implements IDisposable, IFrameServer

    Property Info As ServerInfo Implements IFrameServer.Info

    Private NativeServer As INativeFrameServer

    Sub New(path As String)
        If path.ToLower.EndsWith(".avs") Then
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
    Public ColorSpace As Integer
End Structure

Public Class FrameServerFactory
    Shared Function Create(scriptPath As String, useVFW As Boolean) As IFrameServer
        If useVFW Then
            Return New VfwFrameServer(scriptPath)
        Else
            Dim dllPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(
                Path.GetDirectoryName(Windows.Forms.Application.StartupPath))), "FrameServer.dll")

            If File.Exists(dllPath) Then
                LoadLibrary(dllPath)
            Else
                LoadLibrary("FrameServer.dll")
            End If

            Return New DirectFrameServer(scriptPath)
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
                data += 40
                pitch = (((Info.Width * 32) + 31) And Not 31) >> 3
                Return 0 'S_OK
            End If
        End If

        Return &H80004005 'E_FAIL
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
        Public rcFrame As RECT
        Public dwEditCount As UInt32
        Public dwFormatChangeCount As UInt32
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=64)>
        Public szName As String
    End Structure

    Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
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
