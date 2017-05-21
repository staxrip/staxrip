Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports Microsoft.Win32
Imports StaxRip.UI

Class ProcessForm
    Inherits FormBase

#Region " Designer "
    Private components As System.ComponentModel.IContainer

    Friend WithEvents bnAbort As System.Windows.Forms.Button
    Friend WithEvents lWhenfinisheddo As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents bnResume As System.Windows.Forms.Button
    Friend WithEvents bnJobs As System.Windows.Forms.Button
    Friend WithEvents lStatus As System.Windows.Forms.Label
    Friend WithEvents bnSuspend As System.Windows.Forms.Button
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents mbShutdown As MenuButton
    Friend WithEvents tbLog As System.Windows.Forms.TextBox

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ProcessForm))
        Me.bnAbort = New System.Windows.Forms.Button()
        Me.lWhenfinisheddo = New System.Windows.Forms.Label()
        Me.tbLog = New System.Windows.Forms.TextBox()
        Me.NotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.bnJobs = New System.Windows.Forms.Button()
        Me.bnResume = New System.Windows.Forms.Button()
        Me.lStatus = New System.Windows.Forms.Label()
        Me.bnSuspend = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.mbShutdown = New StaxRip.UI.MenuButton()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnAbort
        '
        Me.bnAbort.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnAbort.Location = New System.Drawing.Point(519, 3)
        Me.bnAbort.Margin = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.bnAbort.Name = "bnAbort"
        Me.bnAbort.Size = New System.Drawing.Size(115, 35)
        Me.bnAbort.TabIndex = 2
        Me.bnAbort.Text = "Abort"
        '
        'lWhenfinisheddo
        '
        Me.lWhenfinisheddo.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lWhenfinisheddo.AutoSize = True
        Me.lWhenfinisheddo.Location = New System.Drawing.Point(3, 8)
        Me.lWhenfinisheddo.Name = "lWhenfinisheddo"
        Me.lWhenfinisheddo.Size = New System.Drawing.Size(156, 25)
        Me.lWhenfinisheddo.TabIndex = 6
        Me.lWhenfinisheddo.Text = "When finished do:"
        '
        'tbLog
        '
        Me.tbLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbLog.Location = New System.Drawing.Point(12, 12)
        Me.tbLog.Multiline = True
        Me.tbLog.Name = "tbLog"
        Me.tbLog.ReadOnly = True
        Me.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLog.Size = New System.Drawing.Size(884, 537)
        Me.tbLog.TabIndex = 7
        Me.tbLog.WordWrap = False
        '
        'NotifyIcon
        '
        '
        'bnJobs
        '
        Me.bnJobs.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnJobs.Location = New System.Drawing.Point(637, 3)
        Me.bnJobs.Name = "bnJobs"
        Me.bnJobs.Size = New System.Drawing.Size(115, 35)
        Me.bnJobs.TabIndex = 9
        Me.bnJobs.Text = "Jobs"
        Me.bnJobs.UseVisualStyleBackColor = True
        '
        'bnResume
        '
        Me.bnResume.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnResume.Location = New System.Drawing.Point(401, 3)
        Me.bnResume.Name = "bnResume"
        Me.bnResume.Size = New System.Drawing.Size(115, 35)
        Me.bnResume.TabIndex = 8
        Me.bnResume.Text = "Resume"
        Me.bnResume.UseVisualStyleBackColor = True
        '
        'lStatus
        '
        Me.lStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lStatus.Location = New System.Drawing.Point(12, 553)
        Me.lStatus.Name = "lStatus"
        Me.lStatus.Size = New System.Drawing.Size(884, 29)
        Me.lStatus.TabIndex = 10
        Me.lStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bnSuspend
        '
        Me.bnSuspend.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnSuspend.Location = New System.Drawing.Point(283, 3)
        Me.bnSuspend.Margin = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.bnSuspend.Name = "bnSuspend"
        Me.bnSuspend.Size = New System.Drawing.Size(115, 35)
        Me.bnSuspend.TabIndex = 12
        Me.bnSuspend.Text = "Suspend"
        Me.bnSuspend.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Controls.Add(Me.lWhenfinisheddo)
        Me.FlowLayoutPanel1.Controls.Add(Me.mbShutdown)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnSuspend)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnResume)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnAbort)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnJobs)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(144, 585)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(755, 41)
        Me.FlowLayoutPanel1.TabIndex = 13
        '
        'mbShutdown
        '
        Me.mbShutdown.Location = New System.Drawing.Point(165, 3)
        Me.mbShutdown.ShowMenuSymbol = True
        Me.mbShutdown.Size = New System.Drawing.Size(115, 35)
        '
        'ProcessForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(908, 635)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Controls.Add(Me.tbLog)
        Me.Controls.Add(Me.lStatus)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "ProcessForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "StaxRip x64 - Log"
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region

    Public Shared ShutdownVisible As Boolean
    Public Shared Instance As ProcessForm

    Shared Message As String
    Shared IsProcess As Boolean
    Shared DataReceivedCounter As Integer
    Shared LastRefresh As Integer

    Shared Property CommandLineLog As New StringBuilder
    Shared Property ProcInstance As Proc

    Private TaskbarButtonCreatedMessage As Integer

    Property Taskbar As Taskbar
    Property OriginalLeft As Integer

    Sub New()
        AddHandler Application.ThreadException, AddressOf g.OnUnhandledException

        InitializeComponent()

        mbShutdown.Add(System.Enum.GetValues(GetType(ShutdownMode)).Cast(Of Object))
        lWhenfinisheddo.Visible = ShutdownVisible
        mbShutdown.Visible = ShutdownVisible
        tbLog.WordWrap = True

        bnAbort.Visible = IsProcess
        bnResume.Visible = IsProcess
        bnSuspend.Visible = IsProcess

        tbLog.Font = New Font("Consolas", 9 * s.UIScaleFactor)

        AddHandler Log.Update, AddressOf LogUpdate
        Icon = My.Resources.RipIcon
        NotifyIcon.Icon = My.Resources.RipIcon
        NotifyIcon.Text = "StaxRip"

        TaskbarButtonCreatedMessage = Native.RegisterWindowMessage("TaskbarButtonCreated")

        If g.IsMinimizedEncodingInstance Then
            ShowInTaskbar = False
            NotifyIcon.Visible = True
        End If
    End Sub

    Shared Sub ClearCommandLineOutput()
        CommandLineLog.Length = 0
    End Sub

    Shared Sub CommandLineDataHandler(sendingProcess As Object, d As DataReceivedEventArgs)
        If Not Instance Is Nothing AndAlso d.Data <> "" Then
            Dim value = d.Data

            If Not ProcInstance.RemoveChars Is Nothing Then
                For Each i In ProcInstance.RemoveChars
                    If value.Contains(i) Then value = value.Replace(i, "")
                Next
            End If

            If Not ProcInstance.TrimChars Is Nothing Then value = value.Trim(ProcInstance.TrimChars)
            Dim skip As Boolean

            If Not ProcInstance.SkipStrings Is Nothing Then
                For Each i In ProcInstance.SkipStrings
                    If value.Contains(i) Then skip = True
                Next
            End If

            If Not ProcInstance.SkipPatterns Is Nothing Then
                For Each i In ProcInstance.SkipPatterns
                    If Regex.IsMatch(value, i) Then skip = True
                Next
            End If

            UpdateStatusThreadsafe(value)

            If Not skip AndAlso CommandLineLog.Length < 10000 AndAlso value.Trim <> "" Then
                Log.WriteLine(value.Trim)
                CommandLineLog.AppendLine(value.Trim)
            End If
        End If
    End Sub

    Private Shared UpdateAction As Action(Of String) = New Action(Of String)(AddressOf UpdateStatus)

    Shared Sub UpdateStatusThreadsafe(value As String)
        Instance.BeginInvoke(UpdateAction, {value})
    End Sub

    Private Shared Sub UpdateStatus(value As String)
        If IsVisible AndAlso value <> "" Then
            Instance.lStatus.Text = value

            If value.Contains("%") Then
                value = value.Left("%")

                If value.Contains("[") Then value = value.Right("[")
                If value.Contains(" ") Then value = value.RightLast(" ")

                If value.IsSingle Then
                    Instance.Taskbar?.SetState(TaskbarStates.Normal)
                    Instance.Taskbar?.SetValue(value.ToSingle, 100)
                    Exit Sub
                End If
            End If

            Instance.Taskbar?.SetState(TaskbarStates.NoProgress)
        End If
    End Sub

    Shared ReadOnly Property IsActive() As Boolean
        Get
            Return Not Instance Is Nothing
        End Get
    End Property

    Shared ReadOnly Property IsVisible() As Boolean
        Get
            Return IsActive AndAlso Instance.Visible
        End Get
    End Property

    Sub LogUpdate(text As String)
        Message = text
        Instance?.BeginInvoke(New Action(AddressOf LogUpdate))
    End Sub

    Private Sub LogUpdate()
        lStatus.Text = ""
        tbLog.Text = Message
        tbLog.SelectionStart = tbLog.TextLength
        tbLog.ScrollToCaret()
    End Sub

    Shared Sub ShowForm()
        ShowForm(True)
    End Sub

    Shared Sub ShowForm(isProcess As Boolean)
        StaxRip.ProcessForm.IsProcess = isProcess

        If Instance Is Nothing Then
            g.MainForm.Hide()
            Dim t = New Thread(AddressOf ShowFormInternal)
            t.SetApartmentState(ApartmentState.STA)
            t.Start()

            While Instance Is Nothing OrElse Not Instance.IsHandleCreated
                Thread.Sleep(50)
            End While
        Else
            Instance.Invoke(New Action(AddressOf SetMessage))
        End If
    End Sub

    Private Shared Sub ShowFormInternal()
        Dim f As New ProcessForm
        Instance = f
        f.tbLog.Text = Message
        Application.Run(f)
    End Sub

    Private Shared Sub SetMessage()
        Instance.tbLog.Text = Message
    End Sub

    Private Shared Sub CloseFormMethod()
        Try
            Instance.Close()
            Instance.Dispose()
            Instance = Nothing
        Catch
        End Try
    End Sub

    Shared Sub CloseProcessForm()
        Try
            If Not Instance Is Nothing Then
                g.MainForm.Show()
                g.MainForm.Refresh()
                If Not Instance.IsDisposed Then Instance.Invoke(New Action(AddressOf CloseFormMethod))
            End If
        Catch
        End Try
    End Sub

    Private Sub bnAbort_Click() Handles bnAbort.Click
        If MsgOK("Abort processing?") Then
            Registry.CurrentUser.Write("Software\" + Application.ProductName, "ShutdownMode", 0)
            ProcInstance.KillAndThrow()
        End If
    End Sub

    Private Sub TaskForm_FormClosing() Handles Me.FormClosing
        RemoveHandler Log.Update, AddressOf LogUpdate
    End Sub

    Private Sub cbShutdown_SelectedIndexChanged() Handles mbShutdown.ValueChangedUser
        Registry.CurrentUser.Write("Software\" + Application.ProductName, "ShutdownMode", CInt(mbShutdown.Value))
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case &H112 'WM_SYSCOMMAND
                Select Case m.WParam.ToInt32
                    Case Native.SC_MINIMIZE
                        g.IsMinimizedEncodingInstance = True
                        Hide()
                        NotifyIcon.Visible = True
                        Exit Sub
                    Case Native.SC_CLOSE
                        bnAbort.PerformClick()
                        Exit Sub
                End Select
            Case TaskbarButtonCreatedMessage
                Taskbar = New Taskbar(Handle)
        End Select

        MyBase.WndProc(m)
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
        ShowMe()
    End Sub

    Private Sub bnJobs_Click() Handles bnJobs.Click
        Using f As New JobsForm()
            f.ShowDialog()
        End Using
    End Sub

    Private Sub bnSuspend_Click(sender As Object, e As EventArgs) Handles bnSuspend.Click
        For Each i As ProcessThread In GetProcess.Threads
            Dim h = OpenThread(ThreadAccess.SUSPEND_RESUME, False, i.Id)
            SuspendThread(h)
            CloseHandle(h)
        Next
    End Sub

    Private Sub bnResume_Click(sender As Object, e As EventArgs) Handles bnResume.Click
        Dim proc = GetProcess()

        For x = proc.Threads.Count - 1 To 0 Step -1
            Dim h = OpenThread(ThreadAccess.SUSPEND_RESUME, False, proc.Threads(x).Id)
            ResumeThread(h)
            CloseHandle(h)
        Next
    End Sub

    Function GetProcess() As Process
        If ProcInstance.Process.ProcessName = "cmd" Then
            For Each i In ProcessHelp.GetChilds(ProcInstance.Process)
                If {"conhost", "vspipe"}.Contains(i.ProcessName) Then Continue For
                Return i
            Next
        Else
            Return ProcInstance.Process
        End If
    End Function

    Private Sub ShowMe()
        lStatus.Text = ""
        g.IsMinimizedEncodingInstance = False
        ShowInTaskbar = True
        If Left < 0 Then Left = OriginalLeft
        Show()
        Activate()

        Try
            Dim proc = ProcessForm.ProcInstance.Process
            Native.ShowWindow(proc.MainWindowHandle, Native.SW_RESTORE)
            Native.SetForegroundWindow(proc.MainWindowHandle)
        Catch
        End Try

        NotifyIcon.Visible = False
    End Sub

    Protected Overrides Sub OnActivated(e As EventArgs)
        mbShutdown.Value = CType(Registry.CurrentUser.GetInt("Software\" + Application.ProductName, "ShutdownMode"), ShutdownMode)
        MyBase.OnActivated(e)
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        mbShutdown.Value = CType(Registry.CurrentUser.GetInt("Software\" + Application.ProductName, "ShutdownMode"), ShutdownMode)
        If g.IsMinimizedEncodingInstance Then Hide()
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        If g.IsMinimizedEncodingInstance Then
            OriginalLeft = Left
            Left = -5000
        End If
    End Sub

    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            Return g.IsMinimizedEncodingInstance
        End Get
    End Property
End Class