
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
End Structure
