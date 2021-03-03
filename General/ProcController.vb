
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks

Imports Microsoft.Win32

Imports StaxRip.UI

Public Class ProcController
    Private LogAction As Action = New Action(AddressOf LogHandler)
    Private StatusAction As Action(Of String) = New Action(Of String)(AddressOf ProgressHandler)
    Private ReadOnly CustomProgressInfoSeparator As String = ", "
    Private UseFirstExpression As Boolean = True
    Private FailCounter As Integer = 0

    Property Proc As Proc
    Property LogTextBox As New RichTextBoxEx
    Property ProgressBar As New LabelProgressBar
    Property ProcForm As ProcessingForm
    Property Button As New ButtonEx

    Shared Property Procs As New List(Of ProcController)
    Shared Property Aborted As Boolean
    Shared Property LastActivation As Long
    Shared Property BlockActivation As Boolean


    Sub New(proc As Proc)
        Me.Proc = proc
        ProcForm = g.ProcForm

        Dim pad = g.ProcForm.FontHeight \ 6
        Button.Margin = New Padding(pad, pad, 0, pad)
        Button.Font = New Font("Consolas", 9 * s.UIScaleFactor)
        Button.Text = " " + proc.Title + " "
        Dim sz = TextRenderer.MeasureText(Button.Text, Button.Font)
        Dim fh = Button.Font.Height
        Button.Width = sz.Width + fh
        Button.Height = CInt(fh * 1.5)
        AddHandler Button.Click, AddressOf Click

        ProgressBar.Dock = DockStyle.Fill
        ProgressBar.Font = New Font("Consolas", 9 * s.UIScaleFactor)

        LogTextBox.ScrollBars = RichTextBoxScrollBars.Both
        LogTextBox.Multiline = True
        LogTextBox.Dock = DockStyle.Fill
        LogTextBox.ReadOnly = True
        LogTextBox.WordWrap = True
        LogTextBox.Font = New Font("Consolas", 9 * s.UIScaleFactor)
        AddHandler LogTextBox.AfterThemeApplied, AddressOf HighlightLog

        ProcForm.pnLogHost.Controls.Add(LogTextBox)
        ProcForm.pnStatusHost.Controls.Add(ProgressBar)
        ProcForm.flpNav.Controls.Add(Button)

        AddHandler proc.ProcDisposed, AddressOf ProcDisposed
        AddHandler proc.OutputDataReceived, AddressOf DataReceived
        AddHandler proc.ErrorDataReceived, AddressOf DataReceived
    End Sub

    Sub DataReceived(value As String)
        If value = "" Then
            Exit Sub
        End If

        Dim ret = Proc.ProcessData(value)

        If ret.Data = "" Then
            Exit Sub
        End If

        If ret.Skip Then
            If Proc.IntegerFrameOutput AndAlso Proc.FrameCount > 0 AndAlso ret.Data.IsInt Then
                ret.Data = "Progress: " + (ret.Data.ToInt / Proc.FrameCount * 100).ToString("0.00") + "%"
            End If

            If Proc.IntegerPercentOutput AndAlso ret.Data.IsInt Then
                ret.Data = "Progress: " + ret.Data + "%"
            End If

            ProcForm.BeginInvoke(StatusAction, {ret.Data})
        Else
            If ret.Data.Trim <> "" Then
                Proc.Log.WriteLine(ret.Data)
            End If

            ProcForm.BeginInvoke(LogAction, Nothing)
        End If
    End Sub

    Sub LogHandler()
        'LogTextBox.BlockPaint = True
        LogTextBox.Text = Proc.Log.ToString
        HighlightLog(ThemeManager.CurrentTheme)
        'LogTextBox.BlockPaint = False
    End Sub

    Public Sub HighlightLog(theme As Theme)
        If Proc.IsSilent Then
            Exit Sub
        End If

        If Not s.OutputHighlighting Then
            LogTextBox.ClearAllFormatting()
            Return
        End If

        If theme Is Nothing Then theme = ThemeManager.CurrentTheme

        LogTextBox.BlockPaint = True
        Try
            Dim oh = theme.ProcessingForm.OutputHighlighting
            Dim matches As MatchCollection
            Dim help As Integer
            Dim IsX264 = Proc.Package Is Package.x264
            Dim IsX265 = Proc.Package Is Package.x265

            If IsX265 Then
                help = 1
                matches = Regex.Matches(LogTextBox.Text, "(?<=\n)x265\s\[info\]:\s(.+\s|tools):\s.+", RegexOptions.IgnoreCase)
                For Each m As Match In matches
                    If help Mod 2 = 0 Then
                        LogTextBox.SelectionFormat(m.Index, m.Length, oh.AlternateBackColor, oh.AlternateForeColor, oh.AlternateFontStyles)
                    End If
                    help += 1
                Next
            End If

            If IsX265 OrElse IsX264 Then
                matches = Regex.Matches(LogTextBox.Text, "(?<=\n)(x264|x265)(?=\s\[)", RegexOptions.IgnoreCase)
                For Each m As Match In matches
                    LogTextBox.SelectionFormat(m.Index, m.Length, oh.SourceBackColor.SetHue(205), oh.SourceForeColor)
                Next
            End If

            matches = Regex.Matches(LogTextBox.Text, "(?<=\n)(avs\+|avs\s|vpy\s)(?=\s\[)", RegexOptions.IgnoreCase)
            For Each m As Match In matches
                LogTextBox.SelectionFormat(m.Index, m.Length, oh.SourceBackColor.SetHue(300), oh.SourceForeColor)
            Next

            matches = Regex.Matches(LogTextBox.Text, "(?<=\n)(raw\s)(?=\s\[)", RegexOptions.IgnoreCase)
            For Each m As Match In matches
                LogTextBox.SelectionFormat(m.Index, m.Length, oh.SourceBackColor.SetHue(175), oh.SourceForeColor)
            Next

            matches = Regex.Matches(LogTextBox.Text, "(?<=\n.*\s\[)(warning)\]:\s(.+)", RegexOptions.IgnoreCase)
            For Each m As Match In matches
                LogTextBox.SelectionFormat(m.Groups(1).Index, m.Groups(1).Length, oh.WarningLabelBackColor, oh.WarningLabelForeColor, oh.WarningLabelFontStyles)
                LogTextBox.SelectionFormat(m.Groups(2).Index, m.Groups(2).Length, oh.WarningTextBackColor, oh.WarningTextForeColor, oh.WarningTextFontStyles)
            Next

            matches = Regex.Matches(LogTextBox.Text, "(?<=\n.*\s\[)info(?=\]:\s)", RegexOptions.IgnoreCase)
            For Each m As Match In matches
                LogTextBox.SelectionFormat(m.Index, m.Length, oh.InfoLabelBackColor, oh.InfoLabelForeColor)
            Next

            matches = Regex.Matches(LogTextBox.Text, "(?<=\s|^)(--\w[^\s=]*)([\s=]((?!--)[^""\s]+|""[^""\n]*"")?)?", RegexOptions.IgnoreCase)
            For Each m As Match In matches
                LogTextBox.SelectionFormat(m.Groups(1).Index, m.Groups(1).Length, oh.ParameterBackColor, oh.ParameterForeColor, oh.ParameterFontStyles)
                If m.Groups.Count > 3 Then
                    LogTextBox.SelectionFormat(m.Groups(3).Index, m.Groups(3).Length, oh.ParameterValueBackColor, oh.ParameterValueForeColor, oh.ParameterValueFontStyles)
                End If
            Next

            matches = Regex.Matches(LogTextBox.Text, "([A-Z]:\\[\w\\]+\.[eE][xX][eE])|(""[A-Z]:\\[\w\s\\]+\.[eE][xX][eE]"")")
            For Each m As Match In matches
                LogTextBox.SelectionFormat(m.Index, m.Length, oh.ExeFileBackColor, oh.ExeFileForeColor, oh.ExeFileFontStyles)
            Next

            matches = Regex.Matches(LogTextBox.Text, "(""[A-Z]:\\[^\a\b\e\f\n\r\t\v""]+\.(json)"")|((?<!"")[A-Z]:\\[\S\\]+\.(json)(?!""))", RegexOptions.IgnoreCase)
            For Each m As Match In matches
                LogTextBox.SelectionFormat(m.Index, m.Length, oh.MetadataFileBackColor, oh.MetadataFileForeColor, oh.MetadataFileFontStyles)
            Next

            matches = Regex.Matches(LogTextBox.Text, "(""[A-Z]:\\[^\a\b\e\f\n\r\t\v""]+\.(avc|h264|h265|hevc|mkv|mp4)"")|((?<!"")[A-Z]:\\[\S\\]+\.(avc|h264|h265|hevc|mkv|mp4)(?!""))", RegexOptions.IgnoreCase)
            For Each m As Match In matches
                LogTextBox.SelectionFormat(m.Index, m.Length, oh.MediaFileBackColor, oh.MediaFileForeColor, oh.MediaFileFontStyles)
            Next

            matches = Regex.Matches(LogTextBox.Text, "(""[A-Z]:\\[^\a\b\e\f\n\r\t\v""]+\.(avs|dll|vpy)"")|((?<!"")[A-Z]:\\[\S\\]+\.(avs|dll|vpy)(?!""))", RegexOptions.IgnoreCase)
            For Each m As Match In matches
                LogTextBox.SelectionFormat(m.Index, m.Length, oh.ScriptFileBackColor, oh.ScriptFileForeColor, oh.ScriptFileFontStyles)
            Next

            matches = Regex.Matches(LogTextBox.Text, "(?<=\s)frames\s(\d+)\s-\s(\d+)\sof\s(\d+)", RegexOptions.IgnoreCase)
            For Each m As Match In matches
                LogTextBox.Select(m.Index, m.Length)
                If m.Groups(1).Value.ToInt() > 0 OrElse m.Groups(2).Value.ToInt() + 1 < m.Groups(3).Value.ToInt() Then
                    LogTextBox.SelectionFormat(m.Index, m.Length, oh.FramesCuttedBackColor, oh.FramesCuttedForeColor, oh.FramesCuttedFontStyles)
                    For i = 1 To m.Groups.Count - 1
                        LogTextBox.SelectionFormat(m.Groups(i).Index, m.Groups(i).Length, oh.FramesCuttedNumberBackColor, oh.FramesCuttedNumberForeColor, oh.FramesCuttedNumberFontStyles)
                    Next
                Else
                    LogTextBox.SelectionFormat(m.Index, m.Length, oh.FramesBackColor, oh.FramesForeColor, oh.FramesFontStyles)
                End If
            Next

            matches = Regex.Matches(LogTextBox.Text, "(?<=\]:\s)(avisynth|vapoursynth).*\d+.*", RegexOptions.IgnoreCase)
            For Each m As Match In matches
                LogTextBox.SelectionFormat(m.Index, m.Length, oh.FrameServerBackColor, oh.FrameServerForeColor, oh.FrameServerFontStyles)
            Next

            If IsX265 OrElse IsX264 Then
                matches = Regex.Matches(LogTextBox.Text, "(?<=\]:\s).*encoder(?:\D*?([^\d\s]*\d+\S*\d[^\d\s]*))(?:.*(djatom|patman|asuna))?.*(?=\n)", RegexOptions.IgnoreCase)
                For Each m As Match In matches
                    LogTextBox.SelectionFormat(m.Index, m.Length, oh.EncoderBackColor, oh.EncoderForeColor, oh.EncoderFontStyles)
                    LogTextBox.SelectionFormat(m.Groups(1).Index, m.Groups(1).Length, oh.EncoderBackColor, oh.EncoderForeColor.AddSaturation(0.1).AddLuminance(0.175), oh.EncoderFontStyles.Union({FontStyle.Bold}).ToArray)
                    If m.Groups.Count > 2 Then
                        LogTextBox.SelectionFormat(m.Groups(2).Index, m.Groups(2).Length, oh.EncoderBackColor, oh.EncoderForeColor.AddSaturation(0.1).AddLuminance(0.175), oh.EncoderFontStyles.Union({FontStyle.Bold}).ToArray)
                    End If
                Next
            End If

        Catch ex As Exception
        Finally
            LogTextBox.Select(0, 0)
            LogTextBox.BlockPaint = False
        End Try
    End Sub

    Sub ProgressHandler(value As String)
        SetProgressText(value)
        SetProgress(value)
    End Sub

    Shared LastProgress As Double

    Sub SetProgressText(value As String)
        If Proc.IsSilent Then
            Exit Sub
        End If

        value = value.Trim()

        If s.ProgressOutputCustomize AndAlso FailCounter < 9 Then
            Dim pattern As String
            Dim match As Match

            If Proc.Package Is Package.x264 Then
                If UseFirstExpression Then
                    'Mod by Patman
                    pattern = "\[(\d+)(\.?\d*)%\]\s+(\d+)/(\d+)\sframes\s@\s(\d+)(\.?\d*)\sfps\s\|\s(\d+)(\.?\d*)\s([a-z]{2}/s)\s\|\s(\d+:\d+:\d+)\s\[-(\d+:\d+:\d+)\]\s\|\s(\d+)(\.\d+)?\s([a-z]{1,2})\s\[(\d+)(\.\d+)?\s([a-z]{1,2})\]"
                    match = Regex.Match(value, pattern, RegexOptions.IgnoreCase)

                    If match.Success Then
                        value = $"[{match.Groups(1).Value,2}{match.Groups(2).Value}%] {match.Groups(3).Value.PadLeft(match.Groups(4).Value.Length)}/{match.Groups(4).Value} frames @ {match.Groups(5).Value}{match.Groups(6).Value} fps{CustomProgressInfoSeparator}{match.Groups(7).Value,4} {match.Groups(9).Value}{CustomProgressInfoSeparator}{match.Groups(12).Value}{match.Groups(13).Value} {match.Groups(14).Value} ({match.Groups(15).Value} {match.Groups(17).Value}){CustomProgressInfoSeparator}{match.Groups(10).Value} (-{match.Groups(11).Value})"
                    Else
                        UseFirstExpression = Not UseFirstExpression
                        FailCounter += 1
                    End If
                Else
                    'Mod by DJATOM since x264 161, using header
                    pattern = "\[\s*((\d+)\.?(\d*))%\]\s+((\d+)/(\d+))\s+((\d+)\.?(\d*))\s+((\d+)\.?(\d*))\s+(\d+:\d+:\d+)\s+(\d+:\d+:\d+)\s+((\d+)\.(\d+))\s([a-z]{1,2})\s+((\d+)\.(\d+))\s([a-z]{1,2})"
                    match = Regex.Match(value, pattern, RegexOptions.IgnoreCase)

                    If match.Success Then
                        value = $"[{match.Groups(2).Value,2}.{match.Groups(3).Value}%] {match.Groups(5).Value.PadLeft(match.Groups(6).Value.Length)}/{match.Groups(6).Value} frames @ {match.Groups(8).Value}.{match.Groups(9).Value} fps{CustomProgressInfoSeparator}{match.Groups(11).Value,4} kb/s{CustomProgressInfoSeparator}{match.Groups(16).Value} {match.Groups(18).Value} ({match.Groups(20).Value}.{match.Groups(21).Value} {match.Groups(22).Value}){CustomProgressInfoSeparator}{match.Groups(13).Value} (-{match.Groups(14).Value})"
                    Else
                        UseFirstExpression = Not UseFirstExpression
                        FailCounter += 1
                    End If
                End If
            ElseIf Proc.Package Is Package.x265 Then
                If UseFirstExpression Then
                    'Mod by Patman since x265 3.5-RC1
                    pattern = "\[(\d+)(\.?\d*)%\]\s+(\d+)/(\d+)\sframes\s@\s(\d+)(\.?\d*)\sfps\s\|\s(\d+)(\.?\d*)\s([a-z]{2}/s)\s\|\s(\d+:\d+:\d+)\s\[-(\d+:\d+:\d+)\]\s\|\s(\d+)(\.\d+)?\s([a-z]{1,2})\s\[(\d+)(\.\d+)?\s([a-z]{1,2})\]"
                    match = Regex.Match(value, pattern, RegexOptions.IgnoreCase)

                    If match.Success Then
                        value = $"[{match.Groups(1).Value,2}{match.Groups(2).Value}%] {match.Groups(3).Value.PadLeft(match.Groups(4).Value.Length)}/{match.Groups(4).Value} frames @ {match.Groups(5).Value}{match.Groups(6).Value} fps{CustomProgressInfoSeparator}{match.Groups(7).Value,4} {match.Groups(9).Value}{CustomProgressInfoSeparator}{match.Groups(12).Value}{match.Groups(13).Value} {match.Groups(14).Value} ({match.Groups(15).Value} {match.Groups(17).Value}){CustomProgressInfoSeparator}{match.Groups(10).Value} (-{match.Groups(11).Value})"
                    Else
                        UseFirstExpression = Not UseFirstExpression
                        FailCounter += 1
                    End If
                Else
                    'Mod by DJATOM since x265 3.4+65, including progress-frames
                    pattern = "\[((\d+)\.?(\d*))%\]\s+((\d+)(\(\d+\))?/(\d+)(\sframes)),\s((\d+)\.?(\d*)(\sfps)),\s((\d+)\.?(\d*)\s([a-z]{2}/s)),\selapsed:\s(\d+:\d+:\d+),\seta:\s(\d+:\d+:\d+),\ssize:\s(\d+)\.(\d+)\s([a-z]{1,2}),\sest\.\ssize:\s(\d+)\.(\d+)\s([a-z]{1,2})"
                    match = Regex.Match(value, pattern, RegexOptions.IgnoreCase)

                    If match.Success Then
                        value = $"[{match.Groups(2).Value,2}.{match.Groups(3).Value}%] {match.Groups(5).Value.PadLeft(match.Groups(7).Value.Length)}{match.Groups(6).Value}/{match.Groups(7).Value} frames @ {match.Groups(10).Value}.{match.Groups(11).Value} fps{CustomProgressInfoSeparator}{match.Groups(14).Value,4} {match.Groups(16).Value}{CustomProgressInfoSeparator}{match.Groups(19).Value} {match.Groups(21).Value} ({match.Groups(22).Value} {match.Groups(24).Value}){CustomProgressInfoSeparator}{match.Groups(17).Value} (-{match.Groups(18).Value})"
                    Else
                        UseFirstExpression = Not UseFirstExpression
                        FailCounter += 1
                    End If
                End If
            End If
        End If
        ProgressBar.Text = value
    End Sub

    Sub SetProgress(value As String)
        If Proc.IsSilent Then
            Exit Sub
        End If

        If value.Contains("%") Then
            value = value.Left("%")

            If value.Contains("[") Then
                value = value.Right("[")
            End If

            If value.Contains(" ") Then
                value = value.RightLast(" ")
            End If

            If value.IsDouble Then
                Dim val = value.ToDouble

                If LastProgress <> val Then
                    ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                    ProcForm.Taskbar?.SetValue(Math.Max(val, 1), 100)
                    ProcForm.NotifyIcon.Text = val & "%"
                    ProgressBar.Value = val
                    LastProgress = val
                End If

                Exit Sub
            End If
        ElseIf Proc.Duration <> TimeSpan.Zero AndAlso value.Contains(" time=") Then
            Dim tokens = value.Right(" time=").Left(" ").Split(":"c)

            If tokens.Length = 3 Then
                Dim ts As New TimeSpan(tokens(0).ToInt, tokens(1).ToInt, tokens(2).ToInt)
                Dim val = 100 / Proc.Duration.TotalSeconds * ts.TotalSeconds

                If LastProgress <> val Then
                    ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                    ProcForm.Taskbar?.SetValue(Math.Max(val, 1), 100)
                    ProcForm.NotifyIcon.Text = val & "%"
                    ProgressBar.Value = val
                    LastProgress = val
                End If

                Exit Sub
            End If
        ElseIf Proc.FrameCount > 0 AndAlso value.Contains("frame=") AndAlso value.Contains("fps=") Then
            Dim frameString = value.Left("fps=").Right("frame=")

            If frameString.IsInt Then
                Dim frame = frameString.ToInt

                If frame < Proc.FrameCount Then
                    Dim progressValue = CSng(frame / Proc.FrameCount * 100)

                    If LastProgress <> progressValue Then
                        ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                        ProcForm.Taskbar?.SetValue(Math.Max(progressValue, 1), 100)
                        ProcForm.NotifyIcon.Text = progressValue & "%"
                        ProgressBar.Value = progressValue
                        LastProgress = progressValue
                    End If

                    Exit Sub
                End If
            End If
        ElseIf value.Contains("/100)") Then
            Dim percentString = value.Right("(").Left("/")

            If percentString.IsInt Then
                Dim percent = percentString.ToInt

                If LastProgress <> percent Then
                    ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                    ProcForm.Taskbar?.SetValue(Math.Max(percent, 1), 100)
                    ProcForm.NotifyIcon.Text = percent & "%"
                    ProgressBar.Value = percent
                    LastProgress = percent
                End If

                Exit Sub
            End If
        ElseIf Proc.Package Is Package.Rav1e AndAlso Proc.FrameCount > 0 AndAlso
            value.StartsWith("encoded ") AndAlso value.Contains(" frames, ") Then

            Dim left = value.Left(" frames, ")
            Dim str = left.Right("encoded ")

            If str.IsInt Then
                Dim frame = str.ToInt

                If frame < Proc.FrameCount Then
                    Dim progressValue = CSng(frame / Proc.FrameCount * 100)

                    If LastProgress <> progressValue Then
                        ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                        ProcForm.Taskbar?.SetValue(Math.Max(progressValue, 1), 100)
                        ProcForm.NotifyIcon.Text = progressValue & "%"
                        ProgressBar.Value = progressValue
                        LastProgress = progressValue
                    End If

                    Exit Sub
                End If
            End If
        ElseIf Proc.Package Is Package.aomenc AndAlso Proc.FrameCount > 0 AndAlso value.Contains(" frame ") Then
            Dim right = value.Right(" frame ")
            Dim left = right.Left("/").Trim

            If left.IsInt Then
                Dim frame = left.ToInt

                If frame < Proc.FrameCount Then
                    Dim progressValue = CSng(frame / Proc.FrameCount * 100)

                    If LastProgress <> progressValue Then
                        ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                        ProcForm.Taskbar?.SetValue(Math.Max(progressValue, 1), 100)
                        ProcForm.NotifyIcon.Text = progressValue & "%"
                        ProgressBar.Value = progressValue
                        LastProgress = progressValue
                    End If

                    Exit Sub
                End If
            End If
        End If

        If LastProgress <> 0 Then
            ProcForm.NotifyIcon.Text = "StaxRip"
            ProcForm.Taskbar?.SetState(TaskbarStates.NoProgress)
            LastProgress = 0
        End If
    End Sub

    Sub Click(sender As Object, e As EventArgs)
        SyncLock Procs
            For Each i In Procs
                If i.Button Is sender Then
                    i.Activate()
                Else
                    i.Deactivate()
                End If
            Next
        End SyncLock
    End Sub

    Sub ProcDisposed()
        ProcForm.BeginInvoke(Sub() Cleanup())
    End Sub

    Shared Sub SetOutputHighlighting(value As Boolean, theme As Theme)
        If value <> s.OutputHighlighting Then
            s.OutputHighlighting = value

            For Each pc In Procs
                pc.HighlightLog(theme)
            Next
        End If
    End Sub

    Shared Sub Abort()
        Aborted = True
        Registry.CurrentUser.Write("Software\" + Application.ProductName, "ShutdownMode", 0)

        For Each i In Procs.ToArray
            i.Proc.Abort = True
            i.Proc.Kill()
        Next
    End Sub

    Shared Sub Skip()
        For Each i In Procs.ToArray
            i.Proc.Skip = True
            i.Proc.Kill()
        Next
    End Sub

    Shared Sub Suspend()
        For Each process In GetProcesses()
            For Each thread As ProcessThread In process.Threads
                Dim handle = OpenThread(ThreadAccess.SUSPEND_RESUME, False, thread.Id)
                SuspendThread(handle)
                CloseHandle(handle)
            Next
        Next
    End Sub

    Shared Sub ResumeProcs()
        For Each process In GetProcesses()
            For x = process.Threads.Count - 1 To 0 Step -1
                Dim h = OpenThread(ThreadAccess.SUSPEND_RESUME, False, process.Threads(x).Id)
                ResumeThread(h)
                CloseHandle(h)
            Next
        Next
    End Sub

    Shared Function GetProcesses() As List(Of Process)
        Dim ret As New List(Of Process)

        For Each procButton In Procs.ToArray
            If procButton.Proc.Process.ProcessName = "cmd" Then
                For Each process In ProcessHelp.GetChilds(procButton.Proc.Process)
                    If {"conhost", "vspipe"}.Contains(process.ProcessName) Then
                        Continue For
                    End If

                    ret.Add(process)
                Next
            Else
                ret.Add(procButton.Proc.Process)
            End If
        Next

        Return ret
    End Function

    Sub Cleanup()
        SyncLock Procs
            Procs.Remove(Me)

            RemoveHandler Proc.ProcDisposed, AddressOf ProcDisposed
            RemoveHandler Proc.OutputDataReceived, AddressOf DataReceived
            RemoveHandler Proc.ErrorDataReceived, AddressOf DataReceived

            ProcForm.flpNav.Controls.Remove(Button)
            ProcForm.pnLogHost.Controls.Remove(LogTextBox)
            ProcForm.pnStatusHost.Controls.Remove(ProgressBar)

            Button.Dispose()
            LogTextBox.Dispose()
            ProgressBar.Dispose()

            For Each i In Procs
                i.Deactivate()
            Next

            If Procs.Count > 0 Then
                Procs(0).Activate()
            End If
        End SyncLock

        If Not Proc.Succeeded And Not Proc.Skip Then
            Abort()
        End If

        Task.Run(Sub()
                     Thread.Sleep(500)

                     SyncLock Procs
                         If Procs.Count = 0 AndAlso Not g.IsJobProcessing Then
                             Finished()
                         End If
                     End SyncLock
                 End Sub)
    End Sub

    Shared Sub Finished()
        If Not g.ProcForm Is Nothing Then
            If g.ProcForm.tlpMain.InvokeRequired Then
                g.ProcForm.BeginInvoke(Sub() g.ProcForm.HideForm())
            Else
                g.ProcForm.HideForm()
            End If
        End If

        If g.MainForm.Disposing OrElse g.MainForm.IsDisposed Then
            Exit Sub
        End If

        Dim mainSub = Sub()
                          If Not Aborted Then
                              BlockActivation = True
                          End If

                          g.MainForm.Show()
                          g.MainForm.Refresh()
                          Aborted = False
                      End Sub

        If g.MainForm.tlpMain.InvokeRequired Then
            g.MainForm.BeginInvoke(mainSub)
        Else
            mainSub.Invoke
        End If
    End Sub

    Shared Function IsLastActivationLessThan(sec As Integer) As Boolean
        Return (LastActivation + sec * 1000) < Environment.TickCount
    End Function

    Sub Activate()
        Button.BackColor = ThemeManager.CurrentTheme.ProcessingForm.ProcessButtonBackSelectedColor
        Button.ForeColor = ThemeManager.CurrentTheme.ProcessingForm.ProcessButtonForeSelectedColor
        Proc.IsSilent = False
        LogTextBox.Visible = True
        LogTextBox.BringToFront()
        LogHandler()
        ProgressBar.Visible = True
        ProgressBar.BringToFront()
    End Sub

    Sub Deactivate()
        Button.BackColor = ThemeManager.CurrentTheme.ProcessingForm.ProcessButtonBackColor
        Button.ForeColor = ThemeManager.CurrentTheme.ProcessingForm.ProcessButtonForeColor
        Proc.IsSilent = True
        LogTextBox.Visible = False
        ProgressBar.Visible = False
    End Sub

    Shared Sub AddProc(proc As Proc)
        SyncLock Procs
            Dim pc As New ProcController(proc)
            Procs.Add(pc)

            If Procs.Count = 1 Then
                pc.Activate()
            Else
                pc.Deactivate()
            End If
        End SyncLock
    End Sub

    Shared Sub Start(proc As Proc)
        If Aborted Then
            Throw New AbortException
        End If

        If g.MainForm.Visible Then
            g.MainForm.Hide()
        End If

        SyncLock Procs
            If g.ProcForm Is Nothing Then
                Dim thread = New Thread(Sub()
                                            g.ProcForm = New ProcessingForm
                                            Application.Run(g.ProcForm)
                                        End Sub)

                thread.SetApartmentState(ApartmentState.STA)
                thread.Start()

                While Not ProcessingForm.WasHandleCreated
                    Thread.Sleep(50)
                End While
            End If
        End SyncLock

        g.ProcForm.Invoke(Sub()
                              If Not g.ProcForm.WindowState = FormWindowState.Minimized Then
                                  g.ProcForm.Show()
                                  g.ProcForm.WindowState = FormWindowState.Normal
                                  g.ProcForm.Activate()
                              End If

                              AddProc(proc)
                              g.ProcForm.UpdateControls()
                          End Sub)

    End Sub

    <DllImport("kernel32.dll")>
    Shared Function SuspendThread(hThread As IntPtr) As UInt32
    End Function

    <DllImport("kernel32.dll")>
    Shared Function OpenThread(dwDesiredAccess As ThreadAccess, bInheritHandle As Boolean, dwThreadId As Integer) As IntPtr
    End Function

    <DllImport("kernel32.dll")>
    Shared Function ResumeThread(hThread As IntPtr) As UInt32
    End Function

    <DllImport("kernel32.dll")>
    Shared Function CloseHandle(hObject As IntPtr) As Boolean
    End Function

    <Flags()>
    Public Enum ThreadAccess As Integer
        TERMINATE = &H1
        SUSPEND_RESUME = &H2
        GET_CONTEXT = &H8
        SET_CONTEXT = &H10
        SET_INFORMATION = &H20
        QUERY_INFORMATION = &H40
        SET_THREAD_TOKEN = &H80
        IMPERSONATE = &H100
        DIRECT_IMPERSONATION = &H200
    End Enum
End Class
