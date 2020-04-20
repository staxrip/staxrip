
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Runtime.InteropServices

Module Module1
    Sub Main()
        Dim args = Environment.GetCommandLineArgs()

        If args.Length = 1 Then
            Console.WriteLine("AutoCrop <avs/vpy path> <frame count>")
            Console.WriteLine("Depends on StaxRips FrameServer.dll.")
            Exit Sub
        End If

        Try
            LoadLibrary(Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Windows.Forms.Application.StartupPath))), "FrameServer.dll"))
        Catch
        End Try

        Dim scriptPath = args(1)
        Dim count = CInt(args(2))

        Using server As New FrameServer(scriptPath)
            Dim len = server.Info.FrameCount \ (count + 1)
            Dim crops(count - 1) As AutoCrop
            Dim pos As Integer

            For x = 1 To count
                Console.WriteLine("Progress: " & ((x - 1) / count * 100) & "%")
                pos = len * x

                Using bmp = BitmapUtil.CreateBitmap(server, pos)
                    crops(x - 1) = AutoCrop.Start(bmp.Clone(New Rectangle(0, 0, bmp.Width, bmp.Height), PixelFormat.Format32bppRgb), pos)
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

    Shared Function CreateBitmap(server As FrameServer, position As Integer) As Bitmap
        Dim pitch As Integer
        Dim data As IntPtr

        If server.NativeServer.GetFrame(position, data, pitch) = 0 Then
            Return New Bitmap(server.Info.Width, server.Info.Height,
                pitch, PixelFormat.Format32bppArgb, data)
        End If
    End Function
End Class

Public Class FrameServer
    Implements IDisposable

    Property Info As ServerInfo
    Property NativeServer As IFrameServer

    Sub New(path As String)
        If path.EndsWith(".avs") Then
            NativeServer = CreateAviSynthServer()
        Else
            NativeServer = CreateVapourSynthServer()
        End If

        NativeServer.OpenFile(path)
        Info = Marshal.PtrToStructure(Of ServerInfo)(NativeServer.GetInfo())
    End Sub

    ReadOnly Property [Error] As String
        Get
            Return Marshal.PtrToStringUni(NativeServer.GetError())
        End Get
    End Property

    ReadOnly Property FrameRate As Double
        Get
            Return Info.FrameRateNum / Info.FrameRateDen
        End Get
    End Property

    <DllImport("FrameServer.dll")>
    Shared Function CreateAviSynthServer() As IFrameServer
    End Function

    <DllImport("FrameServer.dll")>
    Shared Function CreateVapourSynthServer() As IFrameServer
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        If Not NativeServer Is Nothing Then
            Marshal.ReleaseComObject(NativeServer)
            NativeServer = Nothing
        End If
    End Sub
End Class

<Guid("A933B077-7EC2-42CC-8110-91DE21116C1A")>
<InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
Public Interface IFrameServer
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
End Structure

Public Enum ColorSpace
    None
End Enum
