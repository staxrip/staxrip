
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading

Public Class Proc
    Implements IDisposable

    Property Abort As Boolean
    Property AllowedExitCodes As Integer() = {0}
    Property BeginOutputReadLine As Boolean
    Property Duration As TimeSpan
    Property ErrorReader As AsyncStreamReader
    Property ExitCode As Integer
    Property FrameCount As Integer
    Property Header As String
    Property IntegerFrameOutput As Boolean
    Property IntegerPercentOutput As Boolean
    Property IsSilent As Boolean
    Property Log As New LogBuilder
    Property OutputFiles As IEnumerable(Of String)
    Property OutputReader As AsyncStreamReader
    Property Package As Package
    Property Priority As ProcessPriorityClass = ProcessPriorityClass.Normal
    Property Process As New Process
    Property Skip As Boolean
    Property SkipString As String
    Property SkipStrings As String()
    Property Succeeded As Boolean
    Property TrimChars As Char()
    Property Wait As Boolean

    Private LogItems As List(Of String)

    Event ProcDisposed()
    Event OutputDataReceived(value As String)
    Event ErrorDataReceived(value As String)

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
            If ProjectValue Is Nothing Then
                ProjectValue = p
            End If

            Return ProjectValue
        End Get
        Set(value As Project)
            ProjectValue = If(value, p)
        End Set
    End Property

    Property WorkingDirectory() As String
        Get
            Return Process.StartInfo.WorkingDirectory
        End Get
        Set(Value As String)
            If Directory.Exists(Value) Then
                Process.StartInfo.WorkingDirectory = Value
            End If
        End Set
    End Property

    ReadOnly Property Title As String
        Get
            If Package IsNot Nothing Then
                Return Package.Name
            End If

            Dim header = ""

            If Me.Header <> "" Then
                header = Me.Header.ToLowerInvariant
            End If

            Dim ret = ""

            For Each i In Package.Items.Values
                If header?.Contains(i.Name.ToLowerInvariant) OrElse Arguments?.Contains(i.Filename) Then
                    ret += " | " + i.Name
                End If
            Next

            If ret = "" Then
                ret = File.Base
            End If

            Return ret.TrimStart(" |".ToCharArray)
        End Get
    End Property

    Shared Function GetSkipStrings(commands As String) As String()
        commands = commands.ToLowerInvariant

        If commands.Contains("xvid_encraw") Then
            Return {"key=", "frames("}
        ElseIf commands.Contains("ffmpeg") Then
            Return {"frame=", "size=", "Press [q] to stop"}
        ElseIf commands.Contains("eac3to") Then
            Return {"process: ", "analyze: "}
        ElseIf commands.Contains("qaac") Then
            Return {", ETA ", "x)", "Optimizing..."}
        ElseIf commands.Contains("fdkaac") Then
            Return {"%]", "x)"}
        ElseIf commands.Contains("nvenc") Then
            Return {"frames: "}
        ElseIf commands.Contains("x264") OrElse commands.Contains("x265") Then
            Return {"%]"}
        Else
            Return {" [ETA ", ", eta ", "frames: ", "Maximum Gain Found",
                "transcoding ...", "process: ", "analyze: "}
        End If
    End Function

    Property File() As String
        Get
            Return Process.StartInfo.FileName
        End Get
        Set(Value As String)
            Process.StartInfo.FileName = If(Value?.Contains("%"), Environment.ExpandEnvironmentVariables(Value), Value)
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
        If LogItems Is Nothing Then
            LogItems = New List(Of String)
        End If

        LogItems.Add(value)
    End Sub

    Sub Kill()
        Try
            If Not Process.HasExited Then
                If Process.ProcessName = "cmd" Then
                    For Each i In ProcessHelp.GetChilds(Process)
                        If {"conhost", Package.avs2pipemod.Filename.Base(), Package.vspipe.Filename.Base()}.Contains(i.ProcessName) Then
                            Continue For
                        End If

                        If Not i.HasExited Then
                            i.Kill()
                        End If
                    Next
                Else
                    Process.Kill()
                End If
            End If
        Catch
        End Try

        If OutputFiles Is Nothing Then
            Exit Sub
        End If

        g.RunTask(Sub()
                      For Each i In OutputFiles
                          Dim counter = 0

                          While i.FileExists AndAlso counter < 9
                              Try
                                  IO.File.Delete(i)
                              Catch
                              End Try

                              Thread.Sleep(100)
                              counter += 1
                          End While
                      Next
                  End Sub)
    End Sub

    Sub OutputReadNotifyUser(value As String)
        RaiseEvent OutputDataReceived(value)
    End Sub

    Sub ErrorReadNotifyUser(value As String)
        RaiseEvent ErrorDataReceived(value)
    End Sub

    Sub Start()
        If ProcController.Aborted Then
            Throw New AbortException
        End If

        Try
            If Header <> "" Then
                Log.WriteHeader(Header)

                If Package IsNot Nothing AndAlso Not s.AllowToolsWithWrongVersion Then
                    Log.WriteLine(Package.Name + " " + Package.Version + BR2)
                End If
            End If

            If Process.StartInfo.FileName = "" AndAlso Package IsNot Nothing Then
                Process.StartInfo.FileName = Package.Path
            Else
            End If

            If ReadOutput Then
                If File = "cmd.exe" AndAlso Arguments.StartsWithEx("/S /C """) AndAlso Arguments.EndsWithEx("""") Then
                    Log.WriteLine(Arguments.Substring(7, Arguments.Length - 8) + BR2)
                Else
                    Log.WriteLine(CommandLine + BR2)
                End If

                ProcController.Start(Me)
            End If

            If LogItems IsNot Nothing Then
                For Each line In LogItems
                    Log.WriteLine(line)
                Next
            End If

            SetEnvironmentVariables(Process)
            Process.Start()

            If ReadOutput Then
                OutputReader = New AsyncStreamReader(
                    Process.StandardOutput.BaseStream,
                    AddressOf OutputReadNotifyUser,
                    Process.StandardOutput.CurrentEncoding)

                ErrorReader = New AsyncStreamReader(
                    Process.StandardError.BaseStream,
                    AddressOf ErrorReadNotifyUser,
                    Process.StandardError.CurrentEncoding)

                OutputReader.BeginReadLine()
                ErrorReader.BeginReadLine()
            End If
        Catch ex As AbortException
            Throw ex
        Catch ex As SkipException
            Throw ex
        Catch ex As Exception
            Dim msg = ex.Message

            If File <> "" Then
                msg += BR2 + "File: " + File
            End If

            If Arguments <> "" Then
                msg += BR2 + "Arguments: " + Arguments
            End If

            MsgError(msg)
        End Try

        Try
            If Priority <> ProcessPriorityClass.Normal AndAlso Not Process.HasExited Then
                Process.PriorityClass = Priority
            End If

            If Wait Then
                Process.WaitForExit()
                OutputReader?.WaitUtilEOF()
                ErrorReader?.WaitUtilEOF()

                ExitCode = Process.ExitCode

                If Abort Then
                    Throw New AbortException
                End If

                If Skip Then
                    Throw New SkipException
                End If

                If ExitCode <> 0 AndAlso Not AllowedExitCodes.ContainsEx(ExitCode) Then
                    Dim l = Log.ToString().Replace(Header, "")
                    l = Regex.Replace(l, "^-+\s+-+\s*$", "", RegexOptions.Multiline)
                    l = Regex.Replace(l, "^\s*", "", RegexOptions.Singleline)
                    l = Regex.Replace(l, "\s*$", "", RegexOptions.Singleline)
                    l = l.Trim()
                    Log.Clear()

                    Dim ec = $"{Header} returned exit code: {ExitCode} (0x{ExitCode:X})"
                    Dim sb = New StringBuilder()
                    sb.AppendLine(l)
                    sb.AppendLine()
                    sb.AppendLine()
                    sb.AppendLine(ec)
                    sb.AppendLine(New String("~"c, ec.Length))
                    sb.AppendLine()

                    If s.ErrorMessageExtendedByErr Then
                        Dim errOutput = ProcessHelp.GetConsoleOutput(Package.Err.Path, "/ntstatus.h /winerror.h " & ExitCode)
                        sb.AppendLine("It's unclear what this exit code means, in case it's")
                        sb.AppendLine("a Windows system error then it possibly means:")
                        sb.AppendLine()
                        sb.AppendLine(errOutput)
                        sb.AppendLine()
                    End If

                    Throw New ErrorAbortException("Error " + Header, sb.ToString(), Project)
                End If

                Succeeded = True
            End If
        Catch e As ErrorAbortException
            Throw e
        End Try

        If Abort Then Throw New AbortException
        If Skip Then Throw New SkipException
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

                Process?.Dispose()
                OutputReader?.Dispose()
                ErrorReader?.Dispose()

                RaiseEvent ProcDisposed()
            End If
        End If

        DisposedValue = True
    End Sub

    Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Sub AddToPath(folder As String)
        Dim dic = Process.StartInfo.EnvironmentVariables
        dic("path") = folder + ";" + dic("path")
    End Sub

    Shared Sub SetEnvironmentVariables(process As Process)
        If process.StartInfo.UseShellExecute Then
            Exit Sub
        End If

        Dim dic = process.StartInfo.EnvironmentVariables
        dic("AviSynthDLL") = Package.AviSynth.Path

        Dim keys = dic.Keys.OfType(Of String).Select(Function(key) key.ToLowerInvariant)

        For Each mac In Macro.GetMacros(True, False, False, True, False)
            Dim name = mac.Name.Trim("%"c)

            If Not keys.Contains(name) Then
                dic(name) = Macro.Expand(mac.Name)
            End If
        Next

        Dim path = dic("path")

        For Each pack In Package.Items.Values
            If pack.Path.Ext = "exe" AndAlso pack.HelpSwitch IsNot Nothing AndAlso
                pack.Path.FileExists AndAlso Not path.Contains(pack.Directory + ";") Then

                path = pack.Directory + ";" + path
            End If
        Next

        Dim cppDir = IO.Path.Combine(Folder.Startup, "Apps", "Support", "VC")

        If Not path.Contains(cppDir + ";") Then
            path = cppDir + ";" + path
        End If

        dic("path") = path
    End Sub

    Function ProcessData(value As String) As (Data As String, Skip As Boolean)
        If value = "" Then
            Return ("", False)
        End If

        value = Regex.Replace(value, "\x1B\[[0-9;]*[mK]", "")

        If TrimChars IsNot Nothing Then
            value = value.Trim(TrimChars)
        End If

        If Not String.IsNullOrWhiteSpace(SkipString) AndAlso value.Contains(SkipString) Then
            Return (value, True)
        End If

        If SkipStrings IsNot Nothing Then
            For Each i In SkipStrings
                If value.Contains(i) Then
                    Return (value, True)
                End If
            Next
        End If

        If IntegerFrameOutput AndAlso value.Trim.IsInt Then
            Return (value.Trim, True)
        End If

        If IntegerPercentOutput AndAlso value.IsInt Then
            Return (value, True)
        End If

        Return (value, False)
    End Function
End Class
