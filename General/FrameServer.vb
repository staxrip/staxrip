
Imports System.Runtime.InteropServices

Public Class FrameServer
    Implements IDisposable

    Property Info As ServerInfo
    Property NativeServer As IFrameServer

    Shared WasInitialized As Boolean

    Sub New(path As String)
        If Not WasInitialized Then
            Package.Python.AddToPath()
            Package.AviSynth.AddToPath()
            Package.VapourSynth.AddToPath()
            WasInitialized = True
        End If

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

    Function GetInfoText(position As Integer) As String
        Dim rate = FrameRateNum / FrameRateDen
        Dim currentDate = Date.Today.AddSeconds(position / rate)
        Dim lengthtDate = Date.Today.AddSeconds(FrameCount / rate)
        Dim dateFormat = If(lengthtDate.Hour = 0, "mm:ss.fff", "HH:mm:ss.fff")
        Dim frames = FrameCount.ToString
        Dim len = lengthtDate.ToString(dateFormat)

        If position > -1 Then
            frames = position & " of " & FrameCount
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
