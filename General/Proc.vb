Imports System.Text.RegularExpressions
Imports System.Text
Imports System.ComponentModel
Imports System.Runtime.InteropServices

Public Class Proc
    Implements IDisposable

    Property Abort As Boolean
    Property IsSilent As Boolean
    Property Process As New Process
    Property Wait As Boolean
    Property Priority As ProcessPriorityClass = ProcessPriorityClass.Normal
    Property AllowedExitCodes As Integer() = {0}
    Property BeginOutputReadLine As Boolean
    Property SkipString As String
    Property SkipStrings As String()
    Property SkipPatterns As String()
    Property TrimChars As Char()
    Property RemoveChars As Char()
    Property ExitCode As Integer
    Property Frames As Integer
    Property Duration As TimeSpan
    Property Log As New LogBuilder
    Property Succeeded As Boolean
    Property Header As String
    Property Package As Package

    Private LogItems As List(Of String)

    Event ProcDisposed()

    Sub New(Optional readOutput As Boolean = True)
        Me.ReadOutput = readOutput
    End Sub

    Private ReadOutputValue As Boolean

    Property ReadOutput As Boolean
        Get
            Return ReadOutputValue
        End Get
        Set(value As Boolean)
            ReadOutputValue = value

            If value Then
                Process.StartInfo.CreateNoWindow = True
                Process.StartInfo.UseShellExecute = False
                Process.StartInfo.RedirectStandardError = True
                Process.StartInfo.RedirectStandardOutput = True
                Priority = s.ProcessPriority
                Wait = True
            End If
        End Set
    End Property

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
            If Not Package Is Nothing Then Return Package.Name
            Dim header = ""
            If Me.Header <> "" Then header = Me.Header.ToLower
            Dim ret = ""

            For Each i In Package.Items.Values
                If header?.Contains(i.Name.ToLower) OrElse Arguments?.Contains(i.Filename) Then ret += " | " + i.Name
            Next

            If ret = "" Then ret = File.Base
            Return ret.TrimStart(" |".ToCharArray)
        End Get
    End Property

    Shared Sub ExecuteBatch(batchCode As String,
                            header As String,
                            suffix As String,
                            skipStrings As String())

        If batchCode.Contains(BR) Then
            Dim batchPath = p.TempDir + p.TargetFile.Base + suffix + ".bat"
            batchCode = WriteBatchFile(batchPath, batchCode)

            Using proc As New Proc
                proc.Header = header
                proc.SkipStrings = skipStrings
                proc.WriteLog(batchCode + BR2)
                proc.File = "cmd.exe"
                proc.Arguments = "/C call """ + batchPath + """"

                Try
                    proc.Start()
                Catch ex As AbortException
                    Throw ex
                Catch ex As Exception
                    g.ShowException(ex)
                    Throw New AbortException
                End Try
            End Using
        Else
            Using proc As New Proc
                proc.Header = header
                proc.SkipStrings = skipStrings
                proc.File = "cmd.exe"
                proc.Arguments = "/S /C """ + batchCode + """"

                Try
                    proc.Start()
                Catch ex As AbortException
                    Throw ex
                Catch ex As Exception
                    g.ShowException(ex)
                    Throw New AbortException
                End Try
            End Using
        End If
    End Sub

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

    Sub WriteLog(value As String)
        If LogItems Is Nothing Then LogItems = New List(Of String)
        LogItems.Add(value)
    End Sub

    Sub KillAndThrow()
        Try
            Abort = True

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
        If ProcController.Aborted Then Throw New AbortException

        Try
            If Header <> "" Then
                If Not Package Is Nothing Then Header += " using " + Package.Name + " " + Package.Version
                Log.WriteHeader(Header)
            End If

            If Process.StartInfo.FileName = "" Then Process.StartInfo.FileName = Package.Path

            If ReadOutput Then
                ProcController.Start(Me)

                If File = "cmd.exe" AndAlso Arguments?.StartsWith("/S /C """) AndAlso Arguments?.EndsWith("""") Then
                    Log.WriteLine(Arguments.Substring(7, Arguments.Length - 8) + BR2)
                Else
                    Log.WriteLine(CommandLine + BR2)
                End If
            End If

            If Not LogItems Is Nothing Then
                For Each line In LogItems
                    Log.WriteLine(line)
                Next
            End If

            Process.Start()

            If ReadOutput Then
                Process.BeginOutputReadLine()
                Process.BeginErrorReadLine()
            End If
        Catch ex As AbortException
            Throw ex
        Catch ex As Exception
            Dim msg = ex.Message
            If File <> "" Then msg += BR2 + "File: " + File
            If Arguments <> "" Then msg += BR2 + "Arguments: " + Arguments
            MsgError(msg)
        End Try

        Try
            If Priority <> ProcessPriorityClass.Normal AndAlso Not Process.HasExited Then
                Process.PriorityClass = Priority
            End If

            If Wait Then
                Process.WaitForExit()
                ExitCode = Process.ExitCode

                If Abort Then
                    Throw New AbortException
                End If

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

        If Abort Then Throw New AbortException
    End Sub

    Private DisposedValue As Boolean = False

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not DisposedValue Then
            If disposing Then
                If ReadOutput Then
                    Log.WriteStats()

                    If Project.Log.EndsWith(BR2) Then
                        Project.Log.Append(Log.ToString?.TrimStart)
                    Else
                        Project.Log.Append(Log.ToString)
                    End If

                    Project.Log.Save(Project)
                End If

                If Not Process Is Nothing Then Process.Dispose()
                RaiseEvent ProcDisposed()
            End If
        End If

        DisposedValue = True
    End Sub

    Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Function ProcessData(value As String) As (Data As String, Skip As Boolean)
        If value = "" Then Return ("", False)

        If Not RemoveChars Is Nothing Then
            For Each i In RemoveChars
                If value.Contains(i) Then value = value.Replace(i, "")
            Next
        End If

        If Not TrimChars Is Nothing Then value = value.Trim(TrimChars)

        If SkipString <> "" Then
            If value.Contains(SkipString) Then Return (value, True)
        ElseIf Not SkipStrings Is Nothing Then
            For Each i In SkipStrings
                If value.Contains(i) Then Return (value, True)
            Next
        End If

        If Not SkipPatterns Is Nothing Then
            For Each i In SkipPatterns
                If Regex.IsMatch(value, i) Then Return (value, True)
            Next
        End If

        Return (value.Trim, False)
    End Function
End Class