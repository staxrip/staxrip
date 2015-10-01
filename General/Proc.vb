Imports System.Threading
Imports System.Text.RegularExpressions
Imports System.Text

Public Class Proc
    Implements IDisposable

    Property TrowException As Boolean
    Property ReTrowException As Boolean
    Property Process As New Process
    Property Wait As Boolean
    Property Priority As ProcessPriorityClass = ProcessPriorityClass.Normal
    Property HideAfterStart As Boolean
    Property AllowedExitCodes As Integer() = {0}
    Property BeginOutputReadLine As Boolean
    Property BatchCode As String
    Property SkipStrings As String()
    Property SkipPatterns As String()
    Property TrimChars As Char()
    Property RemoveChars As Char()

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
        Log.WriteHeader(header)
        If skipStrings.Length > 0 Then Me.SkipStrings = skipStrings
        ProcessForm.ShowForm()
    End Sub

    Property Directory() As String
        Get
            Return Process.StartInfo.WorkingDirectory
        End Get
        Set(Value As String)
            If IO.Directory.Exists(Value) Then
                Process.StartInfo.WorkingDirectory = Value
            End If
        End Set
    End Property

    Private ExitCodeValue As Integer

    ReadOnly Property ExitCode() As Integer
        Get
            Return ExitCodeValue
        End Get
    End Property

    Property File() As String
        Get
            Return Process.StartInfo.FileName
        End Get
        Set(Value As String)
            If OK(Value) AndAlso Value.Contains("%") Then
                Process.StartInfo.FileName = Environment.ExpandEnvironmentVariables(Value)
            Else
                Process.StartInfo.FileName = Value
            End If
        End Set
    End Property

    Property CommandLine() As String
        Get
            Return """" + File + """ " + Arguments
        End Get
        Set(Value As String)
            Try
                Dim m = Regex.Match(Value, "((?<file>[^\s""]+)|""(?<file>.+?)"") *(?<args>[^\f\r]*)")
                File = m.Groups("file").Value
                Arguments = m.Groups("args").Value
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
        TrowException = True

        Try
            If BatchCode <> "" Then
                Dim code = BatchCode.ToLower

                For Each i In Process.GetProcesses()
                    Try
                        If code.Contains(i.ProcessName.ToLower + ".exe") Then
                            Dim procName = Process.GetProcessById(i.Id).ProcessName
                            Dim procsByName = Process.GetProcessesByName(procName)
                            Dim procIndexdName As String = Nothing
                            Dim tempIndexdName As String = Nothing

                            For idx = 0 To procsByName.Length - 1
                                tempIndexdName = If(idx = 0, procName, Convert.ToString(procName) & "#" & idx)
                                Dim procId = New PerformanceCounter("Process", "ID Process", tempIndexdName)

                                If CInt(procId.NextValue()) = i.Id Then
                                    procIndexdName = tempIndexdName
                                End If
                            Next

                            Dim parentId = New PerformanceCounter("Process", "Creating Process ID", procIndexdName)
                            Dim ppid = CInt(parentId.NextValue())

                            If ppid = Process.Id AndAlso Not i.HasExited AndAlso
                                Msg("Confirm to kill " + i.ProcessName + ".exe",
                                    MessageBoxIcon.Question,
                                    MessageBoxButtons.OKCancel) = DialogResult.OK Then
                                If Not i.HasExited Then i.Kill()
                            End If
                        End If
                    Catch
                    End Try
                Next
            Else
                Process.Kill()
            End If
        Catch
        End Try
    End Sub

    Sub Start()
        Try
            If ReadError OrElse ReadOutput Then
                Log.WriteLine(CommandLine + CrLf2)
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
            Dim m = ex.Message

            If File <> "" Then
                m += CrLf2 + "File: " + File
            End If

            If Arguments <> "" Then
                m += CrLf2 + "Arguments: " + Arguments
            End If

            MsgError(m)

            If ReTrowException Then Throw New AbortException
        End Try

        Try
            If Priority <> ProcessPriorityClass.Normal AndAlso Not Process.HasExited Then
                Process.PriorityClass = Priority
            End If

            If Wait Then
                Process.WaitForExit()
                ExitCodeValue = Process.ExitCode
                Process.Close()

                If TrowException Then Throw New AbortException

                If AllowedExitCodes.Length > 0 AndAlso Not AllowedExitCodes.Contains(ExitCodeValue) Then
                    Dim errorText = Header + " failed with error code " &
                        ExitCodeValue & CrLf2 + ProcessForm.CommandLineLog.ToString() + CrLf

                    ProcessForm.ClearCommandLineOutput()

                    Throw New ErrorAbortException("Error " + Header, errorText)
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
        If Not DisposedValue Then
            If disposing Then
                Cleanup()
            End If
        End If

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

        If writeLog Then
            Log.WriteStats()
            Log.Save()
        End If

        If Not Process Is Nothing Then
            Process.Dispose()
        End If
    End Sub
End Class