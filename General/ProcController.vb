Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Threading.Tasks
Imports Microsoft.Win32
Imports StaxRip.UI

Public Class ProcController
    Property Proc As Proc
    Property LogTextBox As New TextBox
    Property StatusLabel As New Label
    Property ProcForm As ProcForm
    Property CheckBox As New CheckBoxEx

    Private LogAction As Action = New Action(AddressOf LogHandler)
    Private StatusAction As Action(Of String) = New Action(Of String)(AddressOf StatusHandler)

    Shared Property Procs As New List(Of ProcController)
    Shared Property Aborted As Boolean

    Sub New(proc As Proc)
        Me.Proc = proc
        Me.ProcForm = g.ProcForm

        CheckBox.Appearance = Appearance.Button
        CheckBox.AutoSize = True
        CheckBox.Text = " " + proc.Title + " "
        AddHandler CheckBox.Click, AddressOf Click

        StatusLabel.Dock = DockStyle.Fill
        StatusLabel.TextAlign = ContentAlignment.MiddleLeft

        LogTextBox.ScrollBars = ScrollBars.Both
        LogTextBox.Multiline = True
        LogTextBox.Dock = DockStyle.Fill
        LogTextBox.ReadOnly = True
        LogTextBox.WordWrap = True
        LogTextBox.Font = New Font("Consolas", 9 * s.UIScaleFactor)

        ProcForm.pnLogHost.Controls.Add(LogTextBox)
        ProcForm.pnStatusHost.Controls.Add(StatusLabel)
        ProcForm.flpNav.Controls.Add(CheckBox)

        AddHandler proc.ProcDisposed, AddressOf ProcDisposed

        AddHandler proc.Process.OutputDataReceived, AddressOf DataReceived
        AddHandler proc.Process.ErrorDataReceived, AddressOf DataReceived
    End Sub

    Sub DataReceived(sender As Object, e As DataReceivedEventArgs)
        If e.Data = "" Then Exit Sub
        Dim t = Proc.ProcessData(e.Data)
        If t.Item1 = "" Then Exit Sub
        Dim value = t.Item1
        Dim skip = t.Item2

        If skip Then
            ProcForm.BeginInvoke(StatusAction, {value})
        Else
            If Proc.Log.Length < 20000 Then
                Proc.Log.WriteLine(value)
                ProcForm.BeginInvoke(LogAction, Nothing)
            End If
        End If
    End Sub

    Private Sub LogHandler()
        Dim log = Proc.Log.ToString
        LogTextBox.Text = log
        LogTextBox.SelectionStart = log.Length
        LogTextBox.ScrollToCaret()
    End Sub

    Private Sub StatusHandler(value As String)
        StatusLabel.Text = value
        SetTaskbarProgress(value)
    End Sub

    Sub SetTaskbarProgress(value As String)
        If Proc.IsSilent Then Exit Sub

        If value.Contains("%") Then
            value = value.Left("%")

            If value.Contains("[") Then value = value.Right("[")
            If value.Contains(" ") Then value = value.RightLast(" ")

            If value.IsSingle Then
                ProcForm.Taskbar?.SetState(TaskbarStates.Normal)
                ProcForm.Taskbar?.SetValue(value.ToSingle, 100)
                Exit Sub
            End If
        End If

        ProcForm.Taskbar?.SetState(TaskbarStates.NoProgress)
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
        Registry.CurrentUser.Write("Software\" + Application.ProductName, "ShutdownMode", 0)

        For Each i In Procs.ToArray
            i.Proc.KillAndThrow()
        Next

        ShowMainForm()
    End Sub

    Shared Sub ShowMainForm()
        g.MainForm.BeginInvoke(Sub()
                                   g.MainForm.Show()
                                   g.MainForm.Refresh()
                                   g.MainForm.Activate()
                               End Sub)
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
        ProcForm.flpNav.Controls.Remove(CheckBox)

        SyncLock Procs
            Procs.Remove(Me)

            For Each i In Procs
                i.Deactivate()
            Next

            If Procs.Count > 0 Then Procs(0).Activate()
        End SyncLock

        RemoveHandler Proc.ProcDisposed, AddressOf ProcDisposed
        RemoveHandler Proc.Process.OutputDataReceived, AddressOf DataReceived
        RemoveHandler Proc.Process.ErrorDataReceived, AddressOf DataReceived

        ProcForm.pnLogHost.Controls.Remove(LogTextBox)
        ProcForm.pnStatusHost.Controls.Remove(StatusLabel)
        LogTextBox.Dispose()
        StatusLabel.Dispose()
        If Not Proc.Succeeded Then ProcForm.Abort()

        Task.Run(Sub()
                     Thread.Sleep(500)

                     SyncLock Procs
                         If Procs.Count = 0 Then
                             If ProcForm.Visible Then ProcForm.BeginInvoke(Sub()
                                                                               ProcForm.NotifyIcon.Visible = False
                                                                               ProcForm.Hide()
                                                                           End Sub)
                             If Not g.MainForm.Visible Then ShowMainForm()
                         End If
                     End SyncLock
                 End Sub)
    End Sub

    Sub Activate()
        CheckBox.Checked = True
        Proc.IsSilent = False
        LogTextBox.Visible = True
        LogTextBox.BringToFront()
        LogTextBox.Text = Proc.Log.ToString
        StatusLabel.Visible = True
        StatusLabel.BringToFront()
    End Sub

    Sub Deactivate()
        CheckBox.Checked = False
        Proc.IsSilent = True
        LogTextBox.Visible = False
        StatusLabel.Visible = False
    End Sub

    Shared Sub AddProc(proc As Proc)
        Dim pc As New ProcController(proc)

        SyncLock Procs
            Procs.Add(pc)

            If Procs.Count = 1 Then
                pc.Activate()
            Else
                pc.Deactivate()
            End If
        End SyncLock
    End Sub

    Shared CreateProcForm As New Object

    Shared Sub Start(proc As Proc)
        If g.MainForm.Visible Then g.MainForm.Hide()

        SyncLock CreateProcForm
            If g.ProcForm Is Nothing Then
                g.ProcForm = New ProcForm
                Task.Run(Sub() Application.Run(g.ProcForm))
            End If

            While Not ProcForm.WasHandleCreated
                Thread.Sleep(50)
            End While
        End SyncLock

        g.ProcForm.Invoke(Sub()
                              If Not ProcForm.IsMinimized Then g.ProcForm.Show()
                              AddProc(proc)
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