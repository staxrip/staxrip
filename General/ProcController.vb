Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Threading.Tasks

Imports Microsoft.Win32
Imports StaxRip.UI

Public Class ProcController
    Property Proc As Proc
    Property LogTextBox As New TextBox
    Property ProgressBar As New LabelProgressBar
    Property ProcForm As ProcForm
    Property CheckBox As New CheckBoxEx

    Private LogAction As Action = New Action(AddressOf LogHandler)
    Private StatusAction As Action(Of String) = New Action(Of String)(AddressOf StatusHandler)

    Shared Property Procs As New List(Of ProcController)
    Shared Property Aborted As Boolean

    Shared Property BlockActivation As Boolean

    Sub New(proc As Proc)
        Me.Proc = proc
        Me.ProcForm = g.ProcForm

        CheckBox.Appearance = Appearance.Button
        CheckBox.AutoSize = True
        CheckBox.Text = " " + proc.Title + " "
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
        AddHandler proc.Process.OutputDataReceived, AddressOf DataReceived
        AddHandler proc.Process.ErrorDataReceived, AddressOf DataReceived
    End Sub

    Sub DataReceived(sender As Object, e As DataReceivedEventArgs)
        If e.Data = "" Then Exit Sub
        Dim ret = Proc.ProcessData(e.Data)
        If ret.Data = "" Then Exit Sub

        If ret.Skip Then
            ProcForm.BeginInvoke(StatusAction, {ret.Data})
        Else
            Proc.Log.WriteLine(ret.Data)
            ProcForm.BeginInvoke(LogAction, Nothing)
        End If
    End Sub

    Private Sub LogHandler()
        Dim log = Proc.Log.ToString
        LogTextBox.Text = log
    End Sub

    Private Sub StatusHandler(value As String)
        ProgressBar.Text = value
        SetProgress(value)
    End Sub

    Shared LastProgress As Double

    Sub SetProgress(value As String)
        If Proc.IsSilent Then Exit Sub

        If value.Contains("%") Then
            value = value.Left("%")

            If value.Contains("[") Then value = value.Right("[")
            If value.Contains(" ") Then value = value.RightLast(" ")

            If value.IsDouble Then
                Dim val = value.ToDouble

                If LastProgress <> val Then
                    ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                    ProcForm.Taskbar?.SetValue(val, 100)
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
                    ProcForm.Taskbar?.SetValue(val, 100)
                    ProcForm.NotifyIcon.Text = val & "%"
                    ProgressBar.Value = val
                    LastProgress = val
                End If

                Exit Sub
            End If
        ElseIf value.IsInt Then
            Dim val = value.ToInt

            If LastProgress <> val Then
                ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                ProcForm.Taskbar?.SetValue(val, 100)
                ProcForm.NotifyIcon.Text = val & "%"
                ProgressBar.Value = val
                ProgressBar.Text = val & "%"
                LastProgress = val
            End If

            Exit Sub
        ElseIf Proc.Frames > 0 AndAlso value.Contains("frame=") AndAlso value.Contains("fps=") Then
            Dim frameString = value.Left("fps=").Right("frame=")

            If frameString.IsInt Then
                Dim frame = frameString.ToInt

                If frame < Proc.Frames Then
                    Dim progressValue = CSng(frame / Proc.Frames * 100)

                    If LastProgress <> progressValue Then
                        ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                        ProcForm.Taskbar?.SetValue(progressValue, 100)
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
                    ProcForm.Taskbar?.SetValue(percent, 100)
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

    Private Sub Click(sender As Object, e As EventArgs)
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
        BlockActivation = False
        Registry.CurrentUser.Write("Software\" + Application.ProductName, "ShutdownMode", 0)

        For Each i In Procs.ToArray
            i.Proc.KillAndThrow()
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
                    If {"conhost", "vspipe"}.Contains(process.ProcessName) Then Continue For
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
            RemoveHandler Proc.Process.OutputDataReceived, AddressOf DataReceived
            RemoveHandler Proc.Process.ErrorDataReceived, AddressOf DataReceived

            ProcForm.flpNav.Controls.Remove(CheckBox)
            ProcForm.pnLogHost.Controls.Remove(LogTextBox)
            ProcForm.pnStatusHost.Controls.Remove(ProgressBar)

            CheckBox.Dispose()
            LogTextBox.Dispose()
            ProgressBar.Dispose()

            For Each i In Procs
                i.Deactivate()
            Next

            If Procs.Count > 0 Then Procs(0).Activate()
        End SyncLock

        If Not Proc.Succeeded Then Abort()

        Task.Run(Sub()
                     Thread.Sleep(500)

                     SyncLock Procs
                         If Procs.Count = 0 AndAlso Not g.IsProcessing Then
                             g.WriteDebugLog("ProcController.Cleanup about to call Finished")
                             Finished()
                             g.WriteDebugLog("ProcController.Cleanup after Finished")
                         End If
                     End SyncLock

                     g.WriteDebugLog("ProcController.Cleanup " + Proc.Header + "; procs count: " & Procs.Count & ";isprocessing: " & g.IsProcessing)
                 End Sub)
    End Sub

    Shared Sub Finished()
        If Not g.ProcForm Is Nothing Then
            If g.ProcForm.tlpMain.InvokeRequired Then
                g.WriteDebugLog("ProcController.Finished ProcForm InvokeRequired")
                g.ProcForm.BeginInvoke(Sub() g.ProcForm.HideForm())
            Else
                g.WriteDebugLog("ProcController.Finished ProcForm no InvokeRequired")
                g.ProcForm.HideForm()
            End If
        End If

        If g.MainForm.Disposing OrElse g.MainForm.IsDisposed Then Exit Sub

        Dim mainSub = Sub()
                          g.WriteDebugLog("ProcController.Finished MainForm start")
                          BlockActivation = False
                          g.MainForm.Show()
                          g.MainForm.Refresh()
                          Aborted = False
                          g.WriteDebugLog("ProcController.Finished MainForm end")
                      End Sub

        If g.MainForm.tlpMain.InvokeRequired Then
            g.WriteDebugLog("ProcController.Finished MainForm InvokeRequired")
            g.MainForm.BeginInvoke(mainSub)
        Else
            g.WriteDebugLog("ProcController.Finished MainForm no InvokeRequired")
            mainSub.Invoke
        End If
    End Sub

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
        If Aborted Then Throw New AbortException
        If g.MainForm.Visible Then g.MainForm.Hide()

        SyncLock Procs
            If g.ProcForm Is Nothing Then
                Task.Run(Sub()
                             g.ProcForm = New ProcForm
                             Application.Run(g.ProcForm)
                         End Sub)

                While Not ProcForm.WasHandleCreated
                    Thread.Sleep(50)
                End While
            End If
        End SyncLock

        g.ProcForm.Invoke(Sub()
                              If Not g.ProcForm.WindowState = FormWindowState.Minimized Then
                                  g.ProcForm.Show()
                                  g.ProcForm.WindowState = FormWindowState.Normal

                                  If Not BlockActivation Then
                                      g.ProcForm.Activate()
                                      BlockActivation = True
                                  End If
                              End If

                              AddProc(proc)
                              g.ProcForm.UpdateControls()
                          End Sub)

        g.WriteDebugLog("ProcController Start " + proc.Header)
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