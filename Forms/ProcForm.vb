Imports System.Runtime.InteropServices
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

    Private Sub NotifyIcon_MouseClick() Handles NotifyIcon.MouseClick
        ShowForm()
    End Sub

    Private Sub bnJobs_Click() Handles bnJobs.Click
        Using f As New JobsForm()
            f.ShowDialog()
        End Using
    End Sub

    Private Sub bnSuspend_Click(sender As Object, e As EventArgs) Handles bnSuspend.Click
        ProcController.Suspend()
    End Sub

    Private Sub bnResume_Click(sender As Object, e As EventArgs) Handles bnResume.Click
        ProcController.ResumeProcs()
    End Sub

    Private Sub bnAbort_Click(sender As Object, e As EventArgs) Handles bnAbort.Click
        If MsgOK("Abort processing?") Then Abort()
    End Sub

    Sub Abort()
        ProcController.Abort()
    End Sub

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

    Shared Property WasHandleCreated As Boolean

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        WasHandleCreated = True
    End Sub
End Class