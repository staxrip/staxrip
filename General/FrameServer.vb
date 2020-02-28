
Imports System.Runtime.InteropServices

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

    <DllImport("FrameServer")>
    Public Shared Function CreateAviSynthServer() As IFrameServer
    End Function

    <DllImport("FrameServer")>
    Public Shared Function CreateVapourSynthServer() As IFrameServer
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
    <PreserveSig> Function OpenFile(file As String) As IntPtr
    <PreserveSig> Function GetFrame(position As Integer) As IntPtr
    <PreserveSig> Function GetInfo() As IntPtr
    <PreserveSig> Function GetError() As IntPtr
End Interface

Public Structure ServerInfo
    Public Width As Integer
    Public Height As Integer
    Public FrameRateNum As Integer
    Public FrameRateDen As Integer
    Public FrameCount As Integer

    ' ######### avs2pipemod64.exe -info ########
    '
    ' avisynth_version 2.600 / AviSynth+ 3.4 (r2923, 3.4, x86_64)
    ' script_name      D:\Samples\Jill_temp\Jill_new.avs
    ' 
    ' v:width            1920
    ' v:height           1080
    ' v:image_type       framebased
    ' v:field_order      assumed bottom field first
    ' v:pixel_type       YV12
    ' v:bit_depth        8
    ' v:number of planes 3
    ' v:fps              30000/1001
    ' v:frames           6172
    ' v:duration[sec]    205.939
    '
    ' ###########################################

    Function GetText(position As Integer) As String
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
               "Framerate : " + rate.ToString.Shorten(9) & " (" & FrameRateNum & "/" & FrameRateDen & ")"
    End Function
End Structure
