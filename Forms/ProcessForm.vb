Imports System.Threading
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.ComponentModel

Imports Microsoft.Win32

Imports StaxRip.UI
Imports System.Text.RegularExpressions

Public Class ProcessForm
    Inherits FormBase

#Region " Designer "
    Private components As System.ComponentModel.IContainer

    Friend WithEvents bnAbort As System.Windows.Forms.Button
    Friend WithEvents lWhenfinisheddo As System.Windows.Forms.Label
    Private WithEvents cbShutdown As System.Windows.Forms.ComboBox
    Friend WithEvents NotifyIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents bnResume As System.Windows.Forms.Button
    Friend WithEvents bnJobs As System.Windows.Forms.Button
    Friend WithEvents lStatus As System.Windows.Forms.Label
    Friend WithEvents bnDonations As System.Windows.Forms.Button
    Friend WithEvents bnSuspend As System.Windows.Forms.Button
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
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
        Me.bnAbort = New System.Windows.Forms.Button()
        Me.lWhenfinisheddo = New System.Windows.Forms.Label()
        Me.cbShutdown = New System.Windows.Forms.ComboBox()
        Me.tbLog = New System.Windows.Forms.TextBox()
        Me.NotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.bnJobs = New System.Windows.Forms.Button()
        Me.bnResume = New System.Windows.Forms.Button()
        Me.lStatus = New System.Windows.Forms.Label()
        Me.bnDonations = New System.Windows.Forms.Button()
        Me.bnSuspend = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnAbort
        '
        Me.bnAbort.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnAbort.AutoSize = True
        Me.bnAbort.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.bnAbort.Location = New System.Drawing.Point(475, 3)
        Me.bnAbort.Name = "bnAbort"
        Me.bnAbort.Size = New System.Drawing.Size(68, 35)
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
        'cbShutdown
        '
        Me.cbShutdown.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cbShutdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbShutdown.Location = New System.Drawing.Point(165, 6)
        Me.cbShutdown.Name = "cbShutdown"
        Me.cbShutdown.Size = New System.Drawing.Size(116, 33)
        Me.cbShutdown.TabIndex = 5
        '
        'tbLog
        '
        Me.tbLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbLog.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbLog.Location = New System.Drawing.Point(12, 12)
        Me.tbLog.Multiline = True
        Me.tbLog.Name = "tbLog"
        Me.tbLog.ReadOnly = True
        Me.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbLog.Size = New System.Drawing.Size(954, 547)
        Me.tbLog.TabIndex = 7
        Me.tbLog.WordWrap = False
        '
        'NotifyIcon
        '
        '
        'bnJobs
        '
        Me.bnJobs.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnJobs.AutoSize = True
        Me.bnJobs.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.bnJobs.Location = New System.Drawing.Point(549, 3)
        Me.bnJobs.Name = "bnJobs"
        Me.bnJobs.Size = New System.Drawing.Size(58, 35)
        Me.bnJobs.TabIndex = 9
        Me.bnJobs.Text = "Jobs"
        Me.bnJobs.UseVisualStyleBackColor = True
        '
        'bnResume
        '
        Me.bnResume.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnResume.AutoSize = True
        Me.bnResume.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.bnResume.Location = New System.Drawing.Point(384, 3)
        Me.bnResume.Name = "bnResume"
        Me.bnResume.Size = New System.Drawing.Size(85, 35)
        Me.bnResume.TabIndex = 8
        Me.bnResume.Text = "Resume"
        Me.bnResume.UseVisualStyleBackColor = True
        '
        'lStatus
        '
        Me.lStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lStatus.Location = New System.Drawing.Point(12, 563)
        Me.lStatus.Name = "lStatus"
        Me.lStatus.Size = New System.Drawing.Size(954, 29)
        Me.lStatus.TabIndex = 10
        Me.lStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bnDonations
        '
        Me.bnDonations.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnDonations.AutoSize = True
        Me.bnDonations.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.bnDonations.Location = New System.Drawing.Point(613, 3)
        Me.bnDonations.Name = "bnDonations"
        Me.bnDonations.Size = New System.Drawing.Size(80, 35)
        Me.bnDonations.TabIndex = 11
        Me.bnDonations.Text = "Donate"
        '
        'bnSuspend
        '
        Me.bnSuspend.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnSuspend.AutoSize = True
        Me.bnSuspend.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.bnSuspend.Location = New System.Drawing.Point(287, 3)
        Me.bnSuspend.Name = "bnSuspend"
        Me.bnSuspend.Size = New System.Drawing.Size(91, 35)
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
        Me.FlowLayoutPanel1.Controls.Add(Me.cbShutdown)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnSuspend)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnResume)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnAbort)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnJobs)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnDonations)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(273, 595)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(696, 41)
        Me.FlowLayoutPanel1.TabIndex = 13
        '
        'ProcessForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(978, 645)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Controls.Add(Me.tbLog)
        Me.Controls.Add(Me.lStatus)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
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

    Friend Shared ShutdownVisible As Boolean

    Private Shared RefreshTime As Integer = 100
    Private Shared Instance As ProcessForm
    Private Shared Message As String
    Private Shared IsProcess As Boolean
    Private Shared DataReceivedCounter As Integer
    Private Shared LastStatus As String
    Private Shared LastRefresh As Integer

    Shared Property CommandLineLog As New StringBuilder
    Shared Property ProcInstance As Proc

    Sub New()
        InitializeComponent()

        cbShutdown.Items.AddRange(ListBag(Of ShutdownMode).GetBagsForEnumType())
        lWhenfinisheddo.Visible = ShutdownVisible
        cbShutdown.Visible = ShutdownVisible
        tbLog.WordWrap = True

        bnAbort.Visible = IsProcess
        bnResume.Visible = IsProcess
        bnSuspend.Visible = IsProcess

        AddHandler Log.Update, AddressOf LogUpdate
        NotifyIcon.Icon = My.Resources.MainIcon
        NotifyIcon.Text = "StaxRip"
        Icon = My.Resources.MainIcon
    End Sub

    Shared Sub ClearCommandLineOutput()
        CommandLineLog.Length = 0
    End Sub

    Shared Sub CommandLineDataHandler(sendingProcess As Object,
                               d As DataReceivedEventArgs)

        If Not Instance Is Nothing AndAlso d.Data <> "" Then
            Dim value = d.Data

            Try
                Dim tc = Environment.TickCount

                If Not ProcInstance.RemoveChars Is Nothing Then
                    For Each i In ProcInstance.RemoveChars
                        If value.Contains(i) Then value = value.Replace(i, "")
                    Next
                End If

                If Not ProcInstance.TrimChars Is Nothing Then
                    value = value.Trim(ProcInstance.TrimChars)
                End If

                If value <> "" Then
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

                    If value.Trim <> "" Then
                        If LastRefresh = 0 OrElse tc - LastRefresh > RefreshTime Then
                            UpdateStatusThreadsafe(value)
                            LastRefresh = tc
                        End If

                        If Not skip OrElse (ProcInstance.SkipStrings Is Nothing AndAlso
                                            ProcInstance.SkipPatterns Is Nothing) Then

                            If CommandLineLog.Length < 10000 Then
                                Log.WriteLine(value.Trim)
                                CommandLineLog.AppendLine(value.Trim)
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                Dim text = "Exception reading console: " + d.Data + CrLf2 + ex.Message
                Log.WriteLine(text)
                CommandLineLog.AppendLine(text)
            End Try
        End If
    End Sub

    Shared Sub UpdateStatusThreadsafe(value As String)
        Instance.BeginInvoke(New Action(Of String)(AddressOf UpdateStatus), value)
    End Sub

    Private Shared Sub UpdateStatus(value As String)
        If Not Instance Is Nothing Then
            If Instance.Visible Then
                Instance.lStatus.Text = value
            Else
                LastStatus = value

                If value.Length < 55 Then
                    value = "StaxRip" + CrLf + value
                ElseIf value.Length > 63 Then 'throws exception if length is > 63
                    value = value.Substring(0, 63)
                End If

                Instance.NotifyIcon.Text = value
            End If
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

        If Not Instance Is Nothing Then
            Instance.BeginInvoke(New Action(AddressOf LogUpdate))
        End If
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
        Catch ex As Exception
        End Try
    End Sub

    Shared Sub ActivateForm()
        Dim procid As Integer
        Dim procName = ""
        Native.GetWindowThreadProcessId(Native.GetForegroundWindow(), procid)

        For Each i In Process.GetProcesses
            If i.Id = procid Then
                procName = i.ProcessName.ToLower
                Exit For
            End If
        Next

        If procName.Contains("mpc") OrElse procName.Contains("vlc") OrElse
            procName.Contains("staxplayer") OrElse procName.Contains("mediamonkey") Then

            Exit Sub
        End If

        g.MainForm.Activate()
    End Sub

    Shared Sub CloseProcessForm()
        Try
            If Not Instance Is Nothing Then
                g.MainForm.Show()
                ActivateForm()
                g.MainForm.Refresh()

                If Not Instance.IsDisposed Then
                    Instance.Invoke(New Action(AddressOf CloseFormMethod))
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub bnAbort_Click() Handles bnAbort.Click
        If Msg("Abort processing?", MessageBoxIcon.Question,
            MessageBoxButtons.OKCancel) = DialogResult.OK Then

            Registry.CurrentUser.Write("Software\" + Application.ProductName, "ShutdownMode", 0)
            ProcInstance.KillAndThrow()
        End If
    End Sub

    Private Sub ProcessForm_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        RefreshTime = 500
        cbShutdown.SelectedIndex = Registry.CurrentUser.GetInt("Software\" + Application.ProductName, "ShutdownMode")
    End Sub

    Private Sub TaskForm_FormClosing() Handles Me.FormClosing
        RemoveHandler Log.Update, AddressOf LogUpdate
    End Sub

    Private Sub cbShutdown_SelectedIndexChanged() Handles cbShutdown.SelectedIndexChanged
        Registry.CurrentUser.Write("Software\" + Application.ProductName, "ShutdownMode", CInt(ListBag(Of ShutdownMode).GetValue(cbShutdown)))
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case Native.WM_SYSCOMMAND
                Select Case m.WParam.ToInt32
                    Case Native.SC_MINIMIZE
                        If Not ProcessForm.ProcInstance Is Nothing Then
                            Try
                                NativeWindow.Hide(ProcessForm.ProcInstance.Process.MainWindowHandle)
                            Catch ex As Exception
                            End Try

                            RefreshTime = 5000
                            Hide()
                            NotifyIcon.Visible = True
                            Exit Sub
                        End If
                    Case Native.SC_CLOSE
                        bnAbort.PerformClick()
                        Exit Sub
                End Select
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

    Private Sub ShowMe()
        lStatus.Text = LastStatus
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

    Private Sub NotifyIcon_MouseClick() Handles NotifyIcon.MouseClick
        ShowMe()
    End Sub

    Private Sub bnJobs_Click() Handles bnJobs.Click
        Using f As New JobsForm()
            f.ShowDialog()
        End Using
    End Sub

    Private Sub bnDonations_Click(sender As Object, e As EventArgs) Handles bnDonations.Click
        g.ShellExecute(Strings.DonationsURL)
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
End Class