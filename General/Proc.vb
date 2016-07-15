Imports System.Threading
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.ComponentModel
Imports System.Runtime.InteropServices

Class Proc
    Implements IDisposable

    Property TrowException As Boolean
    Property ReTrowException As Boolean
    Property Process As New Process
    Property Wait As Boolean
    Property Priority As ProcessPriorityClass = ProcessPriorityClass.Normal
    Property HideAfterStart As Boolean
    Property AllowedExitCodes As Integer() = {0}
    Property BeginOutputReadLine As Boolean
    Property SkipStrings As String()
    Property SkipPatterns As String()
    Property TrimChars As Char()
    Property RemoveChars As Char()
    Property ExitCode As Integer
    Property NoLog As Boolean

    Private ReadOutput As Boolean
    Private ReadError As Boolean
    Private Header As String

    Sub Init(header As String, ParamArray skipStrings As String())
        ProcessForm.ProcInstance = Me
        Me.Header = header
        AddHandler Process.ErrorDataReceived, AddressOf ProcessForm.CommandLineDataHandler
        AddHandler Process.OutputDataReceived, AddressOf ProcessForm.CommandLineDataHandler
        Process.StartInfo.CreateNoWindow = True
        Process.StartInfo.UseShellExecute = False
        Process.StartInfo.RedirectStandardError = True
        Process.StartInfo.RedirectStandardOutput = True
        ReadOutput = True
        ReadError = True
        Wait = True
        Priority = s.ProcessPriority
        If Not NoLog Then Log.WriteHeader(header)
        If skipStrings.Length > 0 Then Me.SkipStrings = skipStrings
        ProcessForm.ShowForm()
    End Sub

    Property WorkingDirectory() As String
        Get
            Return Process.StartInfo.WorkingDirectory
        End Get
        Set(Value As String)
            If Directory.Exists(Value) Then Process.StartInfo.WorkingDirectory = Value
        End Set
    End Property

    Shared ReadOnly Property BatchHeader As String
        Get 'unicode don't work on Windows 7
            If p.SourceFile.ContainsUnicode Then
                Return "@echo off" + BR + "CHCP 65001" + BR
            Else
                Return "@echo off" + BR
            End If
        End Get
    End Property

    Private Shared ConsoleCPValue As Integer

    Shared ReadOnly Property ConsoleCP As Integer
        Get
            If ConsoleCPValue = 0 Then ConsoleCPValue = Regex.Match(ProcessHelp.GetStdOut("cmd.exe", "/C CHCP"), "\d+").Value.ToInt
            Return ConsoleCPValue
        End Get
    End Property

    Shared ReadOnly Property BatchEncoding As Encoding
        Get 'unicode don't work on Windows 7
            If p.SourceFile.ContainsUnicode Then
                Return New UTF8Encoding(False)
            Else
                Return Encoding.GetEncoding(ConsoleCP)
            End If
        End Get
    End Property

    Shared ReadOnly Property ProcessEncoding As Encoding
        Get 'unicode don't work on Windows 7
            If p.SourceFile.ContainsUnicode Then
                Return Encoding.UTF8
            Else
                Return Encoding.Default
            End If
        End Get
    End Property

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
            Return File.Quotes + " " + Arguments
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
        If Not NoLog Then Log.WriteLine(value)
    End Sub

    Sub KillAndThrow()
        TrowException = True

        If Process.ProcessName = "cmd" Then
            For Each i In ProcessHelp.GetChilds(Process)
                If {"conhost", "vspipe"}.Contains(i.ProcessName) Then Continue For

                If MsgOK("Confirm to kill " + i.ProcessName + ".exe") Then
                    If Not i.HasExited Then i.Kill()
                End If
            Next
        Else
            If Not Process.HasExited Then Process.Kill()
        End If
    End Sub

    Sub Start()
        Try
            If (ReadError OrElse ReadOutput) AndAlso Not NoLog Then
                Log.WriteLine(CommandLine + BR2)
                Log.Save()
            End If

            Dim h = Native.GetForegroundWindow()
            Process.Start()

            If ReadError Then Process.BeginErrorReadLine()
            If ReadOutput Then Process.BeginOutputReadLine()

            'WindowStyle.Hidden yield to window handle zero
            If HideAfterStart Then
                Dim counter As Integer

                While counter < 99 AndAlso Not Process.HasExited
                    Process.Refresh()
                    Dim handle = Process.MainWindowHandle

                    If handle <> IntPtr.Zero Then
                        NativeWindow.Hide(handle)
                        Exit While
                    End If

                    Thread.Sleep(50)
                    counter += 1
                End While
            End If

            Native.SetForegroundWindow(h)
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

                If TrowException Then Throw New AbortException

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

                    errorMessage += BR2 + ProcessForm.CommandLineLog.ToString() + BR
                    ProcessForm.ClearCommandLineOutput()

                    Throw New ErrorAbortException("Error " + Header, errorMessage)
                End If
            End If
        Catch e As ErrorAbortException
            Throw e
        End Try

        If TrowException Then Throw New AbortException
    End Sub

    Shared Sub StartComandLine(cmdl As String)
        Using pw As New Proc
            pw.CommandLine = cmdl
            pw.Start()
        End Using
    End Sub

    Private DisposedValue As Boolean = False

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not DisposedValue Then If disposing Then Cleanup()
        DisposedValue = True
    End Sub

    Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Sub Cleanup()
        Dim writeLog As Boolean

        If ReadError Then
            ReadError = False
            writeLog = True
            RemoveHandler Process.ErrorDataReceived, AddressOf ProcessForm.CommandLineDataHandler
            ProcessForm.ClearCommandLineOutput()
        End If

        If ReadOutput Then
            ReadOutput = False
            writeLog = True
            RemoveHandler Process.OutputDataReceived, AddressOf ProcessForm.CommandLineDataHandler
            ProcessForm.ClearCommandLineOutput()
        End If

        If writeLog AndAlso Not NoLog Then
            Log.WriteStats()
            Log.Save()
        End If

        If Not Process Is Nothing Then Process.Dispose()
    End Sub
End Class