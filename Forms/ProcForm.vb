Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Threading.Tasks
Imports Microsoft.Win32
Imports StaxRip.UI

Public Class ProcForm
    Inherits FormBase

#Region " Designer "
    Private components As System.ComponentModel.IContainer

    Friend WithEvents bnAbort As System.Windows.Forms.Button
    Friend WithEvents lWhenfinisheddo As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents bnResume As System.Windows.Forms.Button
    Friend WithEvents bnJobs As System.Windows.Forms.Button
    Friend WithEvents bnSuspend As System.Windows.Forms.Button
    Friend WithEvents flpButtons As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents mbShutdown As MenuButton
    Friend WithEvents pnLogHost As Panel
    Friend WithEvents pnStatusHost As Panel
    Friend WithEvents flpNav As FlowLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    '<System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.bnAbort = New System.Windows.Forms.Button()
        Me.lWhenfinisheddo = New System.Windows.Forms.Label()
        Me.NotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.bnJobs = New System.Windows.Forms.Button()
        Me.bnResume = New System.Windows.Forms.Button()
        Me.bnSuspend = New System.Windows.Forms.Button()
        Me.flpButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.mbShutdown = New StaxRip.UI.MenuButton()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.pnLogHost = New System.Windows.Forms.Panel()
        Me.pnStatusHost = New System.Windows.Forms.Panel()
        Me.flpNav = New System.Windows.Forms.FlowLayoutPanel()
        Me.flpButtons.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnAbort
        '
        Me.bnAbort.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnAbort.Location = New System.Drawing.Point(1051, 0)
        Me.bnAbort.Margin = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.bnAbort.Name = "bnAbort"
        Me.bnAbort.Size = New System.Drawing.Size(220, 70)
        Me.bnAbort.TabIndex = 2
        Me.bnAbort.Text = "Abort"
        '
        'lWhenfinisheddo
        '
        Me.lWhenfinisheddo.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lWhenfinisheddo.AutoSize = True
        Me.lWhenfinisheddo.Location = New System.Drawing.Point(7, 11)
        Me.lWhenfinisheddo.Margin = New System.Windows.Forms.Padding(7, 0, 7, 0)
        Me.lWhenfinisheddo.Name = "lWhenfinisheddo"
        Me.lWhenfinisheddo.Size = New System.Drawing.Size(307, 48)
        Me.lWhenfinisheddo.TabIndex = 6
        Me.lWhenfinisheddo.Text = "When finished do:"
        '
        'NotifyIcon
        '
        '
        'bnJobs
        '
        Me.bnJobs.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnJobs.Location = New System.Drawing.Point(1281, 0)
        Me.bnJobs.Margin = New System.Windows.Forms.Padding(0)
        Me.bnJobs.Name = "bnJobs"
        Me.bnJobs.Size = New System.Drawing.Size(220, 70)
        Me.bnJobs.TabIndex = 9
        Me.bnJobs.Text = "Jobs"
        Me.bnJobs.UseVisualStyleBackColor = True
        '
        'bnResume
        '
        Me.bnResume.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnResume.Location = New System.Drawing.Point(821, 0)
        Me.bnResume.Margin = New System.Windows.Forms.Padding(0)
        Me.bnResume.Name = "bnResume"
        Me.bnResume.Size = New System.Drawing.Size(220, 70)
        Me.bnResume.TabIndex = 8
        Me.bnResume.Text = "Resume"
        Me.bnResume.UseVisualStyleBackColor = True
        '
        'bnSuspend
        '
        Me.bnSuspend.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnSuspend.Location = New System.Drawing.Point(591, 0)
        Me.bnSuspend.Margin = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.bnSuspend.Name = "bnSuspend"
        Me.bnSuspend.Size = New System.Drawing.Size(220, 70)
        Me.bnSuspend.TabIndex = 12
        Me.bnSuspend.Text = "Suspend"
        Me.bnSuspend.UseVisualStyleBackColor = True
        '
        'flpButtons
        '
        Me.flpButtons.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpButtons.AutoSize = True
        Me.flpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpButtons.Controls.Add(Me.lWhenfinisheddo)
        Me.flpButtons.Controls.Add(Me.mbShutdown)
        Me.flpButtons.Controls.Add(Me.bnSuspend)
        Me.flpButtons.Controls.Add(Me.bnResume)
        Me.flpButtons.Controls.Add(Me.bnAbort)
        Me.flpButtons.Controls.Add(Me.bnJobs)
        Me.flpButtons.Location = New System.Drawing.Point(274, 1162)
        Me.flpButtons.Margin = New System.Windows.Forms.Padding(10, 0, 10, 10)
        Me.flpButtons.Name = "flpButtons"
        Me.flpButtons.Size = New System.Drawing.Size(1501, 70)
        Me.flpButtons.TabIndex = 13
        '
        'mbShutdown
        '
        Me.mbShutdown.Location = New System.Drawing.Point(321, 0)
        Me.mbShutdown.Margin = New System.Windows.Forms.Padding(0)
        Me.mbShutdown.ShowMenuSymbol = True
        Me.mbShutdown.Size = New System.Drawing.Size(260, 70)
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.flpButtons, 0, 3)
        Me.tlpMain.Controls.Add(Me.pnLogHost, 0, 1)
        Me.tlpMain.Controls.Add(Me.pnStatusHost, 0, 2)
        Me.tlpMain.Controls.Add(Me.flpNav, 0, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 4
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1785, 1242)
        Me.tlpMain.TabIndex = 14
        '
        'pnLogHost
        '
        Me.pnLogHost.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnLogHost.Location = New System.Drawing.Point(0, 6)
        Me.pnLogHost.Margin = New System.Windows.Forms.Padding(0)
        Me.pnLogHost.Name = "pnLogHost"
        Me.pnLogHost.Size = New System.Drawing.Size(1785, 1096)
        Me.pnLogHost.TabIndex = 17
        '
        'pnStatusHost
        '
        Me.pnStatusHost.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnStatusHost.Location = New System.Drawing.Point(0, 1102)
        Me.pnStatusHost.Margin = New System.Windows.Forms.Padding(0)
        Me.pnStatusHost.Name = "pnStatusHost"
        Me.pnStatusHost.Size = New System.Drawing.Size(1785, 60)
        Me.pnStatusHost.TabIndex = 18
        '
        'flpNav
        '
        Me.flpNav.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpNav.AutoSize = True
        Me.flpNav.Location = New System.Drawing.Point(3, 3)
        Me.flpNav.Name = "flpNav"
        Me.flpNav.Size = New System.Drawing.Size(1779, 1)
        Me.flpNav.TabIndex = 19
        '
        'ProcForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1785, 1242)
        Me.Controls.Add(Me.tlpMain)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(9)
        Me.Name = "ProcForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "StaxRip - Job Processing"
        Me.flpButtons.ResumeLayout(False)
        Me.flpButtons.PerformLayout()
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Public Shared ShutdownVisible As Boolean

    Private TaskbarButtonCreatedMessage As Integer

    Property Taskbar As Taskbar
    Property OriginalLeft As Integer

    Private Procs As New List(Of ProcController)

    Sub New()
        InitializeComponent()

        AddHandler Application.ThreadException, AddressOf g.OnUnhandledException

        mbShutdown.Add(System.Enum.GetValues(GetType(ShutdownMode)).Cast(Of Object))
        lWhenfinisheddo.Visible = ShutdownVisible
        mbShutdown.Visible = ShutdownVisible

        Icon = My.Resources.RipIcon
        NotifyIcon.Icon = My.Resources.RipIcon
        NotifyIcon.Text = "StaxRip"

        TaskbarButtonCreatedMessage = Native.RegisterWindowMessage("TaskbarButtonCreated")

        If g.IsMinimizedEncodingInstance Then
            WindowState = FormWindowState.Minimized

            If s.MinimizeToTray Then
                ShowInTaskbar = False
                NotifyIcon.Visible = True
            End If
        End If
    End Sub

    Private Sub cbShutdown_SelectedIndexChanged() Handles mbShutdown.ValueChangedUser
        Registry.CurrentUser.Write("Software\" + Application.ProductName, "ShutdownMode", CInt(mbShutdown.Value))
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

    Private Sub NotifyIcon_MouseClick() Handles NotifyIcon.MouseClick
        ShowForm()
    End Sub

    Private Sub bnJobs_Click() Handles bnJobs.Click
        Using f As New JobsForm()
            f.ShowDialog()
        End Using
    End Sub

    Private Sub bnSuspend_Click(sender As Object, e As EventArgs) Handles bnSuspend.Click
        For Each process In GetProcesses()
            For Each thread As ProcessThread In process.Threads
                Dim handle = OpenThread(ThreadAccess.SUSPEND_RESUME, False, thread.Id)
                SuspendThread(handle)
                CloseHandle(handle)
            Next
        Next
    End Sub

    Private Sub bnResume_Click(sender As Object, e As EventArgs) Handles bnResume.Click
        For Each process In GetProcesses()
            For x = process.Threads.Count - 1 To 0 Step -1
                Dim h = OpenThread(ThreadAccess.SUSPEND_RESUME, False, process.Threads(x).Id)
                ResumeThread(h)
                CloseHandle(h)
            Next
        Next
    End Sub

    Private Sub bnAbort_Click(sender As Object, e As EventArgs) Handles bnAbort.Click
        If MsgOK("Abort processing?") Then Abort()
    End Sub

    Sub Abort()
        BeginInvoke(Sub()
                        Registry.CurrentUser.Write("Software\" + Application.ProductName, "ShutdownMode", 0)

                        For Each i In Procs.ToArray
                            Try
                                If Not i.IsClosing Then i.Proc.KillAndThrow()
                            Catch ex As Exception
                            End Try
                        Next

                        HideForm()
                    End Sub)
    End Sub

    Function GetProcesses() As List(Of Process)
        Dim ret As New List(Of Process)

        For Each procButton In Procs
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

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case &H112 'WM_SYSCOMMAND
                Select Case m.WParam.ToInt32
                    Case Native.SC_MINIMIZE
                        g.IsMinimizedEncodingInstance = True

                        If s.MinimizeToTray Then
                            Hide()
                            NotifyIcon.Visible = True
                            Exit Sub
                        End If
                    Case Native.SC_CLOSE
                        bnAbort.PerformClick()
                        Exit Sub
                End Select
            Case TaskbarButtonCreatedMessage
                Taskbar = New Taskbar(Handle)
        End Select

        MyBase.WndProc(m)
    End Sub

    Private Sub ShowForm()
        g.IsMinimizedEncodingInstance = False
        ShowInTaskbar = True
        If Left < 0 Then Left = OriginalLeft
        Show()
        Activate()
        NotifyIcon.Visible = False
    End Sub

    Protected Overrides Sub OnActivated(e As EventArgs)
        mbShutdown.Value = CType(Registry.CurrentUser.GetInt("Software\" + Application.ProductName, "ShutdownMode"), ShutdownMode)
        MyBase.OnActivated(e)
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        mbShutdown.Value = CType(Registry.CurrentUser.GetInt("Software\" + Application.ProductName, "ShutdownMode"), ShutdownMode)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        If g.IsMinimizedEncodingInstance AndAlso s.MinimizeToTray Then
            OriginalLeft = Left
            Left = -5000
        End If
    End Sub

    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            Return g.IsMinimizedEncodingInstance
        End Get
    End Property

    Sub HideForm()
        BeginInvoke(Sub()
                        Hide()
                        ShowMainForm()
                    End Sub)
    End Sub

    Sub ShowMainForm()
        g.MainForm.BeginInvoke(Sub()
                                   If Not g.IsEncodingInstance Then
                                       g.MainForm.Show()
                                       g.MainForm.Refresh()
                                       g.MainForm.Activate()
                                   End If
                               End Sub)
    End Sub

    Sub AddProc(proc As Proc)
        Dim pc As New ProcController(proc, Me)
        Procs.Add(pc)

        If Procs.Count = 1 Then
            pc.Activate()
        Else
            pc.Deactivate()
        End If
    End Sub

    Shared Sub Start(proc As Proc)
        SyncLock g
            If g.ProcForm Is Nothing Then
                g.ProcForm = New ProcForm
                Task.Run(Sub() Application.Run(g.ProcForm))
            End If

            While Not WasHandleCreated
                Thread.Sleep(50)
            End While
        End SyncLock

        If g.MainForm.Visible Then g.MainForm.Invoke(Sub() g.MainForm.Hide())

        g.ProcForm.Invoke(Sub()
                              g.ProcForm.Show()
                              g.ProcForm.AddProc(proc)
                          End Sub)
    End Sub

    Shared Property WasHandleCreated As Boolean

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        WasHandleCreated = True
    End Sub

    Private Class ProcController
        Property Proc As Proc
        Property LogTextBox As New TextBox
        Property StatusLabel As New Label
        Property ProcForm As ProcForm
        Property IsClosing As Boolean
        Property CheckBox As New CheckBoxEx

        Private UpdateAction As Action(Of String, String) = New Action(Of String, String)(AddressOf UpdateStatus)

        Sub New(proc As Proc, procForm As ProcForm)
            Me.Proc = proc
            Me.ProcForm = procForm

            CheckBox.Appearance = Appearance.Button
            CheckBox.AutoSize = True
            CheckBox.Text = " " + proc.Title + " "
            AddHandler CheckBox.Click, AddressOf Click

            StatusLabel.Dock = DockStyle.Fill
            StatusLabel.TextAlign = ContentAlignment.MiddleLeft

            LogTextBox.Multiline = True
            LogTextBox.Dock = DockStyle.Fill
            LogTextBox.ReadOnly = True
            LogTextBox.WordWrap = True
            LogTextBox.Font = New Font("Consolas", 9 * s.UIScaleFactor)

            procForm.pnLogHost.Controls.Add(LogTextBox)
            procForm.pnStatusHost.Controls.Add(StatusLabel)
            procForm.flpNav.Controls.Add(CheckBox)

            AddHandler proc.DataReceived, AddressOf DataReceived
            AddHandler proc.Finished, AddressOf Finished
        End Sub

        Private Sub Click(sender As Object, e As EventArgs)
            For Each i In ProcForm.Procs
                If Not i.CheckBox Is sender Then i.Deactivate()
            Next

            For Each i In ProcForm.Procs
                If i.CheckBox Is sender Then i.Activate()
            Next
        End Sub

        Sub Finished()
            ProcForm.BeginInvoke(Sub() Cleanup())
        End Sub

        Sub AutoClose()
            Task.Run(Sub()
                         Thread.Sleep(900)
                         ProcForm.Invoke(Sub()
                                             If ProcForm.Procs.Count = 0 AndAlso
                                                 Not ProcForm.IsDisposed Then

                                                 ProcForm.Hide()
                                                 ProcForm.ShowMainForm()
                                             End If
                                         End Sub)
                     End Sub)
        End Sub

        Sub Cleanup()
            IsClosing = True
            ProcForm.flpNav.Controls.Remove(CheckBox)
            ProcForm.Procs.Remove(Me)

            If ProcForm.Procs.Count > 0 Then
                ProcForm.Procs(0).Activate()
            Else
                AutoClose()
            End If

            RemoveHandler Proc.DataReceived, AddressOf DataReceived
            RemoveHandler Proc.Finished, AddressOf Finished
            ProcForm.pnLogHost.Controls.Remove(LogTextBox)
            ProcForm.pnStatusHost.Controls.Remove(StatusLabel)
            LogTextBox.Dispose()
            StatusLabel.Dispose()

            If Not Proc.Succeeded Then ProcForm.Abort()
        End Sub

        Sub DataReceived(value As String, log As String)
            ProcForm.BeginInvoke(UpdateAction, {value, log})
        End Sub

        Private Sub UpdateStatus(value As String, log As String)
            If IsClosing Then Exit Sub
            StatusLabel.Text = value

            If log <> "" Then
                LogTextBox.Text = log
                LogTextBox.SelectionStart = log.Length
                LogTextBox.ScrollToCaret()
            End If

            If value?.Contains("%") Then
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
    End Class
End Class