
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Public Class FrameServer
    Implements IDisposable

    Property Info As ServerInfo
    Property Server As IFrameServer
    Property Position As Integer

    Private InitError As String

    Sub New(path As String)
        If path.EndsWith(".avs") Then
            Server = CreateAviSynthServer()
        Else
            Dim dll = CStr(Registry.GetValue("HKEY_LOCAL_MACHINE\Software\VapourSynth", "VapourSynthDLL", Nothing))

            If Not File.Exists(dll) Then
                dll = CStr(Registry.GetValue("HKEY_CURRENT_USER\Software\VapourSynth", "VapourSynthDLL", Nothing))
            End If

            If File.Exists(dll) Then
                Environment.SetEnvironmentVariable("path", dll.Dir + ";" + Environment.GetEnvironmentVariable("path"))
            Else
                InitError = "Failed to find VapourSynth location."
                Exit Sub
            End If

            Server = CreateVapourSynthServer()
        End If

        Server.OpenFile(path)
        Info = Marshal.PtrToStructure(Of ServerInfo)(Server.GetInfo())
    End Sub

    ReadOnly Property [Error] As String
        Get
            If InitError <> "" Then
                Return InitError
            End If

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

    <DllImport("FrameServer")>
    Public Shared Function CreateVapourSynthServer() As IFrameServer
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        If Not Server Is Nothing Then
            Marshal.ReleaseComObject(Server)
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
    Public FrameRateNumerator As Integer
    Public FrameRateDenominator As Integer
    Public FrameCount As Integer
End Structure
