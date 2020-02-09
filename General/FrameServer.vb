
Imports System.Runtime.InteropServices

Public Class FrameServer
    Implements IDisposable

    Property Info As ServerInfo
    Property Server As IFrameServer
    Property Position As Integer

    Sub New(path As String)
        Server = CreateAviSynthServer()
        Server.OpenFile(path)
        Info = Marshal.PtrToStructure(Of ServerInfo)(Server.GetInfo())
    End Sub

    ReadOnly Property [Error] As String
        Get
            Return Marshal.PtrToStringUni(Server.GetError())
        End Get
    End Property

    ReadOnly Property FrameRate As Double
        Get
            Return Info.FrameRateNumerator / Info.FrameRateDenominator
        End Get
    End Property

    <DllImport("FrameServer")>
    Public Shared Function CreateAviSynthServer() As IFrameServer
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        Marshal.ReleaseComObject(Server)
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
    Public FrameRateNumerator As Integer
    Public FrameRateDenominator As Integer
    Public FrameCount As Integer
End Structure
