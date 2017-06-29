Imports System.Text.RegularExpressions
Imports System.Text
Imports System.ComponentModel
Imports System.Runtime.InteropServices

Public Class Proc
    Implements IDisposable

    Property IsSilent As Boolean
    Property TrowAbortException As Boolean
    Property ReTrowException As Boolean
    Property Process As New Process
    Property Wait As Boolean
    Property Priority As ProcessPriorityClass = ProcessPriorityClass.Normal
    Property AllowedExitCodes As Integer() = {0}
    Property BeginOutputReadLine As Boolean
    Property SkipStrings As String()
    Property SkipPatterns As String()
    Property TrimChars As Char()
    Property RemoveChars As Char()
    Property ExitCode As Integer
    Property Log As New LogBuilder
    Property Succeeded As Boolean

    Private ReadOutput As Boolean
    Private Header As String

    Event DataReceived(value As String, log As String)
    Event Finished()

    Sub Init(header As String, ParamArray skipStrings As String())
        Me.Header = header
        AddHandler Process.ErrorDataReceived, AddressOf OnDataReceived
        AddHandler Process.OutputDataReceived, AddressOf OnDataReceived
        Process.StartInfo.CreateNoWindow = True
        Process.StartInfo.UseShellExecute = False
        Process.StartInfo.RedirectStandardError = True
        Process.StartInfo.RedirectStandardOutput = True
        ReadOutput = True
        Wait = True
        Priority = s.ProcessPriority
        Log.WriteHeader(header)
        If skipStrings.Length > 0 Then Me.SkipStrings = skipStrings
    End Sub

    Private ProjectValue As Project

    Property Project As Project
        Get
            If ProjectValue Is Nothing Then ProjectValue = p
            Return ProjectValue
        End Get
        Set(value As Project)
            If value Is Nothing Then
                ProjectValue = p
            Else
                ProjectValue = value
            End If
        End Set
    End Property

    Property WorkingDirectory() As String
        Get
            Return Process.StartInfo.WorkingDirectory
        End Get
        Set(Value As String)
            If Directory.Exists(Value) Then Process.StartInfo.WorkingDirectory = Value
        End Set
    End Property

    ReadOnly Property Title As String
        Get
            Dim header = Me.Header.ToLower

            For Each i In Package.Items.Values
                If header.Contains(i.Name.ToLower) Then Return i.Name
            Next

            Return File.Base
        End Get
    End Property

    Shared Function WriteBatchFile(path As String, content As String) As String
        If OSVersion.Current = OSVersion.Windows7 Then
            For Each i In content
                If Convert.ToInt32(i) > 137 Then
                    Throw New ErrorAbortException("Unsupported Windows Version",
                                                  "Executing batch files with character '" & i &
                                                  "' requires minimum Windows 8.")
                End If
            Next
        End If

        If content.IsDosCompatible Then
            content = "@echo off" + BR + content
            IO.File.WriteAllText(path, content, Encoding.GetEncoding(ConsoleHelp.DosCodePage))
        ElseIf content.IsANSICompatible Then
            content = "@echo off" + BR + "CHCP " & Encoding.Default.CodePage & BR + content
            IO.File.WriteAllText(path, content, Encoding.Default)
        Else
            content = "@echo off" + BR + "CHCP 65001" + BR + content
            IO.File.WriteAllText(path, content, New UTF8Encoding(False))
        End If

        Return content
    End Function

    Property File() As String
        Get
            Return Process.StartInfo.FileName
        End Get
        Set(Value As String)
            If Value?.Contains("%") Then
                Process.StartInfo.FileName = Environment.ExpandEnvironmentVariables(Value)
            Else
                Process.StartInfo.FileName = Value
            End If
        End Set
    End Property

    Property CommandLine() As String
        Get
            Return File.Escape + " " + Arguments
        End Get
        Set(Value As String)
            Try
                Dim match = Regex.Match(Value, "((?<file>[^\s""]+)|""(?<file>.+?)"") *(?<args>[^\f\r]*)")
                File = match.Groups("file").Value
                Arguments = match.Groups("args").Value
            Catch
                Throw New Exception("Failed to parse command line: " + Value)
            End Try
        End Set
    End Property

    Property Arguments() As String
        Get
            Return Process.StartInfo.Arguments
        End Get
        Set(Value As String)
            Process.StartInfo.Arguments = Value

            If Process.StartInfo.Arguments.Contains("\""") Then
                Process.StartInfo.Arguments = Process.StartInfo.Arguments.Replace("\""", "\\""")
            End If

            If Process.StartInfo.Arguments.Contains("%") Then
                Process.StartInfo.Arguments = Environment.ExpandEnvironmentVariables(Process.StartInfo.Arguments)
            End If
        End Set
    End Property

    WriteOnly Property Encoding As Encoding
        Set(value As Encoding)
            Process.StartInfo.StandardErrorEncoding = value
            Process.StartInfo.StandardOutputEncoding = value
        End Set
    End Property

    Sub WriteLine(value As String)
        Log.WriteLine(value)
    End Sub

    Sub KillAndThrow()
        TrowAbortException = True

        Try
            If Not Process.HasExited Then
                If Process.ProcessName = "cmd" Then
                    For Each i In ProcessHelp.GetChilds(Process)
                        If {"conhost", "vspipe", "avs2pipemod64"}.Contains(i.ProcessName) Then Continue For
                        If Not i.HasExited Then i.Kill()
                    Next
                Else
                    Process.Kill()
                End If
            End If
        Catch
        End Try
    End Sub

    Sub Start()
        Try
            If ReadOutput Then
                ProcForm.Start(Me)
                Log.WriteLine(CommandLine + BR2)
            End If

            Process.Start()

            If ReadOutput Then
                Process.BeginOutputReadLine()
                Process.BeginErrorReadLine()
            End If
        Catch ex As Exception
            Dim msg = ex.Message
            If File <> "" Then msg += BR2 + "File: " + File
            If Arguments <> "" Then msg += BR2 + "Arguments: " + Arguments
            MsgError(msg)
            If ReTrowException Then Throw New AbortException
        End Try

        Try
            If Priority <> ProcessPriorityClass.Normal AndAlso Not Process.HasExited Then
                Process.PriorityClass = Priority
            End If

            If Wait Then
                Process.WaitForExit()
                ExitCode = Process.ExitCode
                Process.Close()

                If TrowAbortException Then Throw New AbortException

                If AllowedExitCodes.Length > 0 AndAlso Not AllowedExitCodes.Contains(ExitCode) Then
                    Dim ntdllHandle = Native.LoadLibrary("NTDLL.DLL")
                    Dim systemErrorMessage As String

                    Dim retval = Native.FormatMessage(Native.FORMAT_MESSAGE_ALLOCATE_BUFFER Or
                                                      Native.FORMAT_MESSAGE_FROM_SYSTEM Or
                                                      Native.FORMAT_MESSAGE_FROM_HMODULE,
                                                      ntdllHandle, ExitCode, 0, systemErrorMessage,
                                                      0, IntPtr.Zero)
                    Native.FreeLibrary(ntdllHandle)

                    Const ERROR_MR_MID_NOT_FOUND = 317

                    If retval = 0 AndAlso Marshal.GetLastWin32Error <> ERROR_MR_MID_NOT_FOUND Then
                        Throw New Win32Exception(Marshal.GetLastWin32Error)
                    End If

                    Dim errorMessage = Header + " failed with exit code: " & ExitCode & " (" + "0x" + ExitCode.ToString("X") + ")"

                    If systemErrorMessage <> "" Then
                        errorMessage += BR2 + "The exit code might be a system error code: " + systemErrorMessage.Trim
                    End If

                    systemErrorMessage = Nothing

                    retval = Native.FormatMessage(Native.FORMAT_MESSAGE_ALLOCATE_BUFFER Or
                                                  Native.FORMAT_MESSAGE_FROM_SYSTEM,
                                                  IntPtr.Zero, ExitCode, 0, systemErrorMessage,
                                                  0, IntPtr.Zero)

                    If retval = 0 AndAlso Marshal.GetLastWin32Error <> ERROR_MR_MID_NOT_FOUND Then
                        Throw New Win32Exception(Marshal.GetLastWin32Error)
                    End If

                    If systemErrorMessage <> "" Then
                        errorMessage += BR2 + "The exit code might be a system error code: " + systemErrorMessage.Trim
                    End If

                    errorMessage += BR2 + Log.ToString() + BR

                    Throw New ErrorAbortException("Error " + Header, errorMessage, Project)
                End If

                Succeeded = True
            End If
        Catch e As ErrorAbortException
            Throw e
        End Try

        If TrowAbortException Then Throw New AbortException
    End Sub

    Shared Sub StartComandLine(cmdl As String)
        Using pw As New Proc
            pw.CommandLine = cmdl
            pw.Start()
        End Using
    End Sub

    Private DisposedValue As Boolean = False

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not DisposedValue Then
            If disposing Then
                If ReadOutput Then
                    RemoveHandler Process.OutputDataReceived, AddressOf OnDataReceived
                    RemoveHandler Process.ErrorDataReceived, AddressOf OnDataReceived

                    Log.WriteStats()
                    Project.Log.Append(Log.ToString)
                    Project.Log.Save(Project)
                    RaiseEvent Finished()
                End If

                If Not Process Is Nothing Then Process.Dispose()
            End If
        End If

        DisposedValue = True
    End Sub

    Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Sub OnDataReceived(sendingProcess As Object, d As DataReceivedEventArgs)
        Dim value = d.Data
        If value = "" Then Exit Sub

        If Not RemoveChars Is Nothing Then
            For Each i In RemoveChars
                If value.Contains(i) Then value = value.Replace(i, "")
            Next
        End If

        If Not TrimChars Is Nothing Then value = value.Trim(TrimChars)
        Dim skip As Boolean

        If Not SkipStrings Is Nothing Then
            For Each i In SkipStrings
                If value.Contains(i) Then
                    skip = True
                    Exit For
                End If
            Next
        End If

        If Not SkipPatterns Is Nothing Then
            For Each i In SkipPatterns
                If Regex.IsMatch(value, i) Then
                    skip = True
                    Exit For
                End If
            Next
        End If

        If value <> "" Then
            If Not skip Then Log.WriteLine(value.Trim)
            If Not IsSilent Then RaiseEvent DataReceived(value, If(skip, Nothing, Log.ToString))
        End If
    End Sub

    Sub Refresh()
        RaiseEvent DataReceived(Nothing, Log.ToString)
    End Sub
End Class