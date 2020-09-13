
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Threading.Tasks

Imports Microsoft.Win32
Imports StaxRip.UI

Public Class ProcController
    Property Proc As Proc
    Property LogTextBox As New TextBox
    Property ProgressBar As New LabelProgressBar
    Property ProcForm As ProcessingForm
    Property CheckBox As New CheckBoxEx

    Private LogAction As Action = New Action(AddressOf LogHandler)
    Private StatusAction As Action(Of String) = New Action(Of String)(AddressOf StatusHandler)

    Shared Property Procs As New List(Of ProcController)
    Shared Property Aborted As Boolean
    Shared Property LastActivation As Long

    Shared Property BlockActivation As Boolean

    Sub New(proc As Proc)
        Me.Proc = proc
        ProcForm = g.ProcForm

        Dim pad = g.ProcForm.FontHeight \ 6
        CheckBox.Margin = New Padding(pad, pad, 0, pad)
        CheckBox.Appearance = Appearance.Button
        CheckBox.TextAlign = ContentAlignment.MiddleCenter
        CheckBox.Font = New Font("Consolas", 9 * s.UIScaleFactor)
        CheckBox.Text = " " + proc.Title + " "
        Dim sz = TextRenderer.MeasureText(CheckBox.Text, CheckBox.Font)
        CheckBox.Width = sz.Width + CheckBox.Font.Height
        CheckBox.Height = CInt(CheckBox.Font.Height * 1.5)
        AddHandler CheckBox.Click, AddressOf Click

        ProgressBar.Dock = DockStyle.Fill
        ProgressBar.Font = New Font("Consolas", 9 * s.UIScaleFactor)

        LogTextBox.ScrollBars = ScrollBars.Both
        LogTextBox.Multiline = True
        LogTextBox.Dock = DockStyle.Fill
        LogTextBox.ReadOnly = True
        LogTextBox.WordWrap = True
        LogTextBox.Font = New Font("Consolas", 9 * s.UIScaleFactor)

        ProcForm.pnLogHost.Controls.Add(LogTextBox)
        ProcForm.pnStatusHost.Controls.Add(ProgressBar)
        ProcForm.flpNav.Controls.Add(CheckBox)

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
        LogTextBox.Text = Proc.Log.ToString
    End Sub

    Sub StatusHandler(value As String)
        ProgressBar.Text = value
        SetProgress(value)
    End Sub

    Shared LastProgress As Double

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
                If Not i.CheckBox Is sender Then i.Deactivate()
            Next

            For Each i In Procs
                If i.CheckBox Is sender Then i.Activate()
            Next
        End SyncLock
    End Sub

    Sub ProcDisposed()
        ProcForm.BeginInvoke(Sub() Cleanup())
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

            ProcForm.flpNav.Controls.Remove(CheckBox)
            ProcForm.pnLogHost.Controls.Remove(LogTextBox)
            ProcForm.pnStatusHost.Controls.Remove(ProgressBar)

            CheckBox.Dispose()
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
        CheckBox.Checked = True
        Proc.IsSilent = False
        LogTextBox.Visible = True
        LogTextBox.BringToFront()
        LogTextBox.Text = Proc.Log.ToString
        ProgressBar.Visible = True
        ProgressBar.BringToFront()
    End Sub

    Sub Deactivate()
        CheckBox.Checked = False
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

                                  'If Not BlockActivation Then
                                  g.ProcForm.Activate()
                                      '    BlockActivation = True
                                      'End If
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
