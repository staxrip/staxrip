
Imports Microsoft.Win32
Imports StaxRip.UI

Public Class ProcessingForm
    Inherits FormBase

#Region " Designer "
    Private components As System.ComponentModel.IContainer

    Friend WithEvents bnAbort As System.Windows.Forms.Button
    Friend WithEvents laWhenfinisheddo As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents bnJobs As System.Windows.Forms.Button
    Friend WithEvents flpButtons As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents mbShutdown As MenuButton
    Friend WithEvents pnLogHost As Panel
    Friend WithEvents pnStatusHost As Panel
    Friend WithEvents flpNav As FlowLayoutPanel
    Friend WithEvents bnLog As Button
    Friend WithEvents bnMenu As ButtonEx
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
        Me.laWhenfinisheddo = New System.Windows.Forms.Label()
        Me.NotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.bnJobs = New System.Windows.Forms.Button()
        Me.flpButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.mbShutdown = New StaxRip.UI.MenuButton()
        Me.bnLog = New System.Windows.Forms.Button()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
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
        Me.bnAbort.Location = New System.Drawing.Point(596, 0)
        Me.bnAbort.Margin = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.bnAbort.Name = "bnAbort"
        Me.bnAbort.Size = New System.Drawing.Size(260, 70)
        Me.bnAbort.TabIndex = 2
        Me.bnAbort.Text = "Abort (ESC)"
        '
        'laWhenfinisheddo
        '
        Me.laWhenfinisheddo.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.laWhenfinisheddo.AutoSize = True
        Me.laWhenfinisheddo.Location = New System.Drawing.Point(7, 11)
        Me.laWhenfinisheddo.Margin = New System.Windows.Forms.Padding(7, 0, 7, 0)
        Me.laWhenfinisheddo.Name = "laWhenfinisheddo"
        Me.laWhenfinisheddo.Size = New System.Drawing.Size(307, 48)
        Me.laWhenfinisheddo.TabIndex = 6
        Me.laWhenfinisheddo.Text = "When finished do:"
        '
        'NotifyIcon
        '
        '
        'bnJobs
        '
        Me.bnJobs.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnJobs.Location = New System.Drawing.Point(871, 0)
        Me.bnJobs.Margin = New System.Windows.Forms.Padding(0)
        Me.bnJobs.Name = "bnJobs"
        Me.bnJobs.Size = New System.Drawing.Size(260, 70)
        Me.bnJobs.TabIndex = 9
        Me.bnJobs.Text = "Jobs (F6)"
        Me.bnJobs.UseVisualStyleBackColor = True
        '
        'flpButtons
        '
        Me.flpButtons.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpButtons.AutoSize = True
        Me.flpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpButtons.Controls.Add(Me.laWhenfinisheddo)
        Me.flpButtons.Controls.Add(Me.mbShutdown)
        Me.flpButtons.Controls.Add(Me.bnAbort)
        Me.flpButtons.Controls.Add(Me.bnJobs)
        Me.flpButtons.Controls.Add(Me.bnLog)
        Me.flpButtons.Controls.Add(Me.bnMenu)
        Me.flpButtons.Location = New System.Drawing.Point(206, 729)
        Me.flpButtons.Margin = New System.Windows.Forms.Padding(15)
        Me.flpButtons.Name = "flpButtons"
        Me.flpButtons.Size = New System.Drawing.Size(1521, 70)
        Me.flpButtons.TabIndex = 13
        '
        'mbShutdown
        '
        Me.mbShutdown.Location = New System.Drawing.Point(321, 0)
        Me.mbShutdown.Margin = New System.Windows.Forms.Padding(0)
        Me.mbShutdown.ShowMenuSymbol = True
        Me.mbShutdown.Size = New System.Drawing.Size(260, 70)
        '
        'bnLog
        '
        Me.bnLog.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnLog.Location = New System.Drawing.Point(1146, 0)
        Me.bnLog.Margin = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.bnLog.Name = "bnLog"
        Me.bnLog.Size = New System.Drawing.Size(260, 70)
        Me.bnLog.TabIndex = 13
        Me.bnLog.Text = "Log (F7)"
        Me.bnLog.UseVisualStyleBackColor = True
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnMenu.Location = New System.Drawing.Point(1421, 0)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(0)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(100, 70)
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
        Me.tlpMain.Size = New System.Drawing.Size(1742, 814)
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
        Me.pnLogHost.Size = New System.Drawing.Size(1742, 648)
        Me.pnLogHost.TabIndex = 17
        '
        'pnStatusHost
        '
        Me.pnStatusHost.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnStatusHost.Location = New System.Drawing.Point(0, 654)
        Me.pnStatusHost.Margin = New System.Windows.Forms.Padding(0)
        Me.pnStatusHost.Name = "pnStatusHost"
        Me.pnStatusHost.Size = New System.Drawing.Size(1742, 60)
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
        Me.flpNav.Size = New System.Drawing.Size(1736, 1)
        Me.flpNav.TabIndex = 19
        '
        'ProcessingForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1742, 814)
        Me.Controls.Add(Me.tlpMain)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(9)
        Me.Name = "ProcessingForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "StaxRip - Processing..."
        Me.flpButtons.ResumeLayout(False)
        Me.flpButtons.PerformLayout()
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

    Private TaskbarButtonCreatedMessage As Integer
    Private StopAfterCurrentJobMenuItem As ActionMenuItem
    Private CMS As ContextMenuStripEx

    Property Taskbar As Taskbar

    Sub New()
        InitializeComponent()
        AddHandler Application.ThreadException, AddressOf g.OnUnhandledException
        mbShutdown.Add(System.Enum.GetValues(GetType(ShutdownMode)).Cast(Of Object))
        Icon = g.Icon
        NotifyIcon.Icon = g.Icon
        NotifyIcon.Text = "StaxRip"
        TaskbarButtonCreatedMessage = Native.RegisterWindowMessage("TaskbarButtonCreated")
        ScaleClientSize(45, 28)

        CMS = New ContextMenuStripEx(components)
        CMS.Add("Suspend", AddressOf ProcController.Suspend, "Suspends the current process, might not work with all tools.")
        CMS.Add("Resume", AddressOf ProcController.ResumeProcs, "Resumes a suspended process.")
        CMS.Add("-")
        CMS.Add("Abort", AddressOf Abort, "Aborts all job processing of this StaxRip instance.").KeyDisplayString = "ESC"
        CMS.Add("Skip", AddressOf Skip, "Aborts the current job and continues with the next job.")
        StopAfterCurrentJobMenuItem = CMS.Add("Stop After Current", AddressOf StopAfterCurrentJob, "Stops all job processing after the current job.")
        CMS.Add("-")
        CMS.Add("Jobs", AddressOf JobsForm.ShowForm, "Shows the Jobs dialog.").KeyDisplayString = "F6"
        CMS.Add("Log", AddressOf g.DefaultCommands.ShowLogFile, "Shows the log file.").KeyDisplayString = "F7"
        CMS.Add("-")
        CMS.Add("Help", AddressOf ShowHelp).KeyDisplayString = "F1"

        bnMenu.ContextMenuStrip = CMS
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case &H112 'WM_SYSCOMMAND
                Select Case m.WParam.ToInt32
                    Case Native.SC_CLOSE
                        bnAbort.PerformClick()
                        Exit Sub
                End Select
            Case TaskbarButtonCreatedMessage
                Taskbar = New Taskbar(Handle)
        End Select

        MyBase.WndProc(m)
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)

        If s.MinimizeToTray Then
            NotifyIcon.Visible = True

            If WindowState = FormWindowState.Minimized Then
                Hide()
            End If
        End If
    End Sub

    Protected Overrides Sub OnActivated(e As EventArgs)
        MyBase.OnActivated(e)
        UpdateControls()
    End Sub

    Shared Property WasHandleCreated As Boolean

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        WasHandleCreated = True
    End Sub

    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            If ProcController.BlockActivation Then
                Return True
            End If

            Return MyBase.ShowWithoutActivation
        End Get
    End Property

    Sub StopAfterCurrentJob()
        g.StopAfterCurrentJob = Not g.StopAfterCurrentJob
        StopAfterCurrentJobMenuItem.Checked = g.StopAfterCurrentJob
    End Sub

    Sub Abort()
        If MsgOK("Abort processing?") Then
            ProcController.Abort()
        End If
    End Sub

    Sub Skip()
        If MsgOK("Skip current process?") Then
            ProcController.Skip()
        End If
    End Sub

    Sub ShowForm()
        Show()
        WindowState = FormWindowState.Normal
        Activate()
    End Sub

    Sub HideForm()
        WindowState = FormWindowState.Normal
        NotifyIcon.Visible = False
        Hide()
    End Sub

    Sub UpdateControls()
        laWhenfinisheddo.Enabled = g.IsJobProcessing
        mbShutdown.Enabled = g.IsJobProcessing
        bnJobs.Enabled = g.IsJobProcessing
        StopAfterCurrentJobMenuItem.Enabled = g.IsJobProcessing
        StopAfterCurrentJobMenuItem.Checked = g.StopAfterCurrentJob
        mbShutdown.Value = CType(Registry.CurrentUser.GetInt("Software\" + Application.ProductName, "ShutdownMode"), ShutdownMode)
    End Sub

    Sub cbShutdown_SelectedIndexChanged() Handles mbShutdown.ValueChangedUser
        Registry.CurrentUser.Write("Software\" + Application.ProductName, "ShutdownMode", CInt(mbShutdown.Value))
    End Sub

    Sub NotifyIcon_MouseClick() Handles NotifyIcon.MouseClick
        ShowForm()
    End Sub

    Sub bnAbort_Click(sender As Object, e As EventArgs) Handles bnAbort.Click
        Abort()
    End Sub

    Sub bnJobs_Click(sender As Object, e As EventArgs) Handles bnJobs.Click
        JobsForm.ShowForm()
    End Sub

    Sub bnLog_Click(sender As Object, e As EventArgs) Handles bnLog.Click
        g.DefaultCommands.ShowLogFile()
    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs)
        MyBase.OnKeyUp(e)

        Select Case e.KeyData
            Case Keys.F6
                JobsForm.ShowForm()
            Case Keys.F7
                g.DefaultCommands.ShowLogFile()
            Case Keys.Escape
                Abort()
        End Select
    End Sub

    Sub ProcessingForm_HelpRequested(sender As Object, e As HelpEventArgs) Handles Me.HelpRequested
        ShowHelp()
    End Sub

    Sub ShowHelp()
        Dim form As New HelpForm()
        form.Doc.WriteStart(Text)
        form.Doc.WriteTips(CMS.GetTips)
        form.Show()
    End Sub
End Class
