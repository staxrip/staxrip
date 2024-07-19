
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic

Module Module1
    Sub Main()
        Try
            Dim args = Environment.GetCommandLineArgs()

            If args.Length = 1 Then
                Console.WriteLine("AutoCrop <avs/vpy path> <number|interval> <frame count|seconds> <thresholdbegin> <thresholdend> <luminance threshold> <VFW>")
                Exit Sub
            End If

            Dim scriptPath = args(1)
            Dim selectionMode = CInt(args(2))
            Dim selectionValue = CInt(args(3))
            Dim thresholdBegin = CInt(args(4))
            Dim thresholdEnd = CInt(args(5))
            Dim lumThreshold = CInt(args(6)) / 10000F
            Dim vfw = args(7) = "1"

            Using server = FrameServerFactory.Create(scriptPath, vfw)
                Dim info = server.Info
                Dim frameCount = info.FrameCount
                Dim frameRate = info.FrameRate
                Dim startFrame = thresholdBegin
                Dim endFrame = frameCount - 1 - thresholdEnd
                Dim consideredFrames = endFrame - startFrame
                Dim minFrames = 5
                Dim interval = 0
                If selectionMode = 1 Then
                    interval = consideredFrames \ selectionValue
                ElseIf selectionMode = 2 Then
                    interval = selectionValue
                ElseIf selectionMode = 3 Then
                    interval = CInt(Conversion.Fix(selectionValue * frameRate))
                End If
                interval = Math.Min(interval, consideredFrames \ minFrames)
                Dim analyzeCount = (consideredFrames \ interval) + 1
                Dim analyzeFrames(analyzeCount - 1) As Integer
                Dim crops(analyzeCount - 1) As AutoCrop
                Dim offset = (consideredFrames - ((analyzeCount - 1) * interval)) \ 2
                startFrame += offset

                For i = 0 To analyzeCount - 1
                    analyzeFrames(i) = Math.Min(startFrame + i * interval, endFrame)
                Next

                For i = 0 To analyzeCount - 1
                    Console.WriteLine($"Progress: {(i / analyzeCount * 100):f0}%")

                    Dim frame = analyzeFrames(i)
                    Using bmp = BitmapUtil.CreateBitmap(server, frame)
                        crops(i) = AutoCrop.Start(bmp.Clone(New Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format32bppRgb), frame, lumThreshold)
                    End Using

                    If crops(i).Left.Min = 0 AndAlso crops(i).Top.Min = 0 AndAlso crops(i).Right.Min = 0 AndAlso crops(i).Bottom.Min = 0 Then
                        Console.WriteLine($"Progress: 100%")
                        Exit For
                    End If
                Next

                Dim left = crops.Where(Function(x) x IsNot Nothing).SelectMany(Function(arg) arg.Left).Min()
                Dim top = crops.Where(Function(x) x IsNot Nothing).SelectMany(Function(arg) arg.Top).Min()
                Dim right = crops.Where(Function(x) x IsNot Nothing).SelectMany(Function(arg) arg.Right).Min()
                Dim bottom = crops.Where(Function(x) x IsNot Nothing).SelectMany(Function(arg) arg.Bottom).Min()

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

Module Extensions
    <Extension()>
    Function ToColorHSL(color As Color) As ColorHSL
        Return color
    End Function
End Module

Public Class AutoCrop
    Public Top As Integer()
    Public Bottom As Integer()
    Public Left As Integer()
    Public Right As Integer()

    Shared Function Start(bmp As Bitmap, position As Integer, threshold As Single) As AutoCrop
        Dim ret As New AutoCrop
        Dim u = BitmapUtil.Create(bmp)
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
                If u.GetMaxLuminance(xValues(xValue), y) < threshold Then
                    ret.Top(xValue) = y + 1
                Else
                    Exit For
                End If
            Next

            For y = u.BitmapData.Height - 1 To u.BitmapData.Height - u.BitmapData.Height \ 4 Step -1
                If u.GetMaxLuminance(xValues(xValue), y) < threshold Then
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
                If u.GetMaxLuminance(x, yValues(yValue)) < threshold Then
                    ret.Left(yValue) = x + 1
                Else
                    Exit For
                End If
            Next

            For x = u.BitmapData.Width - 1 To u.BitmapData.Width - u.BitmapData.Width \ 4 Step -1
                If u.GetMaxLuminance(x, yValues(yValue)) < threshold Then
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

    Function GetMaxLuminance(x As Integer, y As Integer) As Single
        Return GetPixel(x, y).ToColorHSL().L
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

    ReadOnly Property FrameRate As Decimal
        Get
            Return If(FrameRateDen <> 0, Decimal.Divide(FrameRateNum, FrameRateDen), 0)
        End Get
    End Property
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

<Serializable>
Public Structure ColorHSL
    Private ReadOnly _h As Integer
    Private ReadOnly _s As Single
    Private ReadOnly _l As Single
    Private ReadOnly _a As Single


    Public ReadOnly Property H As Integer
        Get
            Return _h
        End Get
    End Property

    Public ReadOnly Property S As Single
        Get
            Return _s
        End Get
    End Property

    Public ReadOnly Property L As Single
        Get
            Return _l
        End Get
    End Property

    Public ReadOnly Property A As Single
        Get
            Return _a
        End Get
    End Property

    Public Sub New(h As Integer, s As Single, l As Single, Optional a As Single = 1.0F)
        Dim newHue = h Mod 360
        _h = Mathf.Clamp(If(newHue < 0, newHue + 360, newHue), 0, 359)
        _s = Mathf.Clamp01(s)
        _l = Mathf.Clamp01(l)
        _a = Mathf.Clamp01(a)
    End Sub

    Public Shared Widening Operator CType(colorHSL As ColorHSL) As Color
        Dim hue As Integer = Mathf.Clamp(colorHSL.H Mod 360, 0, 359)
        Dim saturation As Single = Mathf.Clamp01(colorHSL.S)
        Dim lightness As Single = Mathf.Clamp01(colorHSL.L)
        Dim alpha As Single = Mathf.Clamp01(colorHSL.A)
        Dim c As Single = (1.0F - Math.Abs(2.0F * lightness - 1.0F)) * saturation
        Dim x As Single = c * (1.0F - Math.Abs((hue / 60.0F) Mod 2.0F - 1.0F))
        Dim m As Single = lightness - c / 2.0F
        Dim r As Single = c
        Dim g As Single = 0
        Dim b As Single = x

        If hue < 300 Then
            r = x
            g = 0
            b = c
        End If

        If hue < 240 Then
            r = 0
            g = x
            b = c
        End If

        If hue < 180 Then
            r = 0
            g = c
            b = x
        End If

        If hue < 120 Then
            r = x
            g = c
            b = 0
        End If

        If hue < 60 Then
            r = c
            g = x
            b = 0
        End If
        Return Color.FromArgb(CType(alpha * 255.0F, Byte), CType(Mathf.Clamp01(r + m) * 255.0F, Byte), CType(Mathf.Clamp01(g + m) * 255.0F, Byte), CType(Mathf.Clamp01(b + m) * 255.0F, Byte))
    End Operator

    Public Shared Widening Operator CType(color As Color) As ColorHSL
        Dim r As Single = color.R / 255.0F
        Dim g As Single = color.G / 255.0F
        Dim b As Single = color.B / 255.0F
        Dim max As Single = Mathf.Max(r, g, b)
        Dim min As Single = Mathf.Min(r, g, b)
        Dim delta As Single = max - min
        Dim alpha As Single = color.A / 255.0F
        Dim lightness As Single = (min + max) / 2
        Dim saturation As Single = 0F
        saturation = If(delta <> 0F, (delta / (1 - Math.Abs(2 * lightness - 1))), saturation)
        Dim hue As Single = 0F
        If delta <> 0F Then
            hue = If(max = r, 60.0F * (((g - b) / delta) Mod 6.0F), hue)
            hue = If(max = g, 60.0F * (((b - r) / delta) + 2.0F), hue)
            hue = If(max = b, 60.0F * (((r - g) / delta) + 4.0F), hue)
        End If
        Return New ColorHSL(Mathf.RoundToInt(hue), saturation, lightness, alpha)
    End Operator

    Public Shared Operator =(colorA As ColorHSL, colorB As ColorHSL) As Boolean
        Return colorA.Equals(colorB)
    End Operator

    Public Shared Operator <>(colorA As ColorHSL, colorB As ColorHSL) As Boolean
        Return Not colorA.Equals(colorB)
    End Operator

    Public Function AddHue(offset As Integer) As ColorHSL
        Return New ColorHSL(H + offset, S, L, A)
    End Function

    Public Function AddSaturation(offset As Single) As ColorHSL
        Return New ColorHSL(H, S + offset, L, A)
    End Function

    Public Function AddLuminance(offset As Single) As ColorHSL
        Return New ColorHSL(H, S, L + offset, A)
    End Function

    Public Function AddAlpha(offset As Single) As ColorHSL
        Return New ColorHSL(H, S, L, A + offset)
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim objColor = CType(obj, ColorHSL)
        Return H = objColor.H AndAlso S = objColor.S AndAlso L = objColor.L AndAlso A = objColor.A
    End Function

    Public Function SetHue(value As Integer) As ColorHSL
        Return New ColorHSL(value, S, L, A)
    End Function

    Public Function SetSaturation(value As Single) As ColorHSL
        Return New ColorHSL(H, value, L, A)
    End Function

    Public Function SetLuminance(value As Single) As ColorHSL
        Return New ColorHSL(H, S, value, A)
    End Function

    Public Function SetAlpha(value As Single) As ColorHSL
        Return New ColorHSL(H, S, L, value)
    End Function

    Public Function ToColor() As Color
        Return CType(Me, Color)
    End Function

    Public Function ToHTML() As String
        Return ColorTranslator.ToHtml(Me)
    End Function

    Public Overrides Function ToString() As String
        Return String.Format($"HSLA({H:0}, {S:0.00}, {L:0.00}, {A:0.00})")
    End Function
End Structure

Public Class Mathf
    Public Shared Function Clamp(value As Integer, min As Integer, max As Integer) As Integer
        If value < min Then
            value = min
        ElseIf value > max Then
            value = max
        End If
        Return value
    End Function

    Public Shared Function Clamp(value As Single, min As Single, max As Single) As Single
        If value < min Then
            value = min
        ElseIf value > max Then
            value = max
        End If
        Return value
    End Function

    Public Shared Function Clamp01(value As Single) As Single
        If value < 0F Then
            value = 0F
        ElseIf value > 1.0F Then
            value = 1.0F
        End If
        Return value
    End Function

    Public Shared Function Min(a As Single, b As Single) As Single
        Return If((a >= b), b, a)
    End Function

    Public Shared Function Min(ParamArray values As Single()) As Single
        Dim num As Integer = values.Length
        Dim result As Single
        If num = 0 Then
            result = 0F
        Else
            Dim num2 As Single = values(0)
            For i As Integer = 1 To num - 1
                If values(i) < num2 Then
                    num2 = values(i)
                End If
            Next
            result = num2
        End If
        Return result
    End Function

    Public Shared Function Min(a As Integer, b As Integer) As Integer
        Return If((a >= b), b, a)
    End Function

    Public Shared Function Min(ParamArray values As Integer()) As Integer
        Dim num As Integer = values.Length
        Dim result As Integer
        If num = 0 Then
            result = 0
        Else
            Dim num2 As Integer = values(0)
            For i As Integer = 1 To num - 1
                If values(i) < num2 Then
                    num2 = values(i)
                End If
            Next
            result = num2
        End If
        Return result
    End Function

    Public Shared Function Max(a As Single, b As Single) As Single
        Return If((a <= b), b, a)
    End Function

    Public Shared Function Max(ParamArray values As Single()) As Single
        Dim num As Integer = values.Length
        Dim result As Single
        If num = 0 Then
            result = 0F
        Else
            Dim num2 As Single = values(0)
            For i As Integer = 1 To num - 1
                If values(i) > num2 Then
                    num2 = values(i)
                End If
            Next
            result = num2
        End If
        Return result
    End Function

    Public Shared Function Max(a As Integer, b As Integer) As Integer
        Return If((a <= b), b, a)
    End Function

    Public Shared Function Max(ParamArray values As Integer()) As Integer
        Dim num As Integer = values.Length
        Dim result As Integer
        If num = 0 Then
            result = 0
        Else
            Dim num2 As Integer = values(0)
            For i As Integer = 1 To num - 1
                If values(i) > num2 Then
                    num2 = values(i)
                End If
            Next
            result = num2
        End If
        Return result
    End Function

    Public Shared Function RoundToInt(value As Single) As Integer
        Return CInt(Math.Round(value))
    End Function
End Class
