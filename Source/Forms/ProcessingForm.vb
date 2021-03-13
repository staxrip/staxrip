
Imports Microsoft.Win32
Imports StaxRip.UI

Public Class ProcessingForm
    Inherits FormBase

#Region " Designer "
    Private components As System.ComponentModel.IContainer

    Friend WithEvents bnAbort As ButtonEx
    Friend WithEvents laWhenfinisheddo As LabelEx
    Friend WithEvents NotifyIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents bnJobs As ButtonEx
    Friend WithEvents flpButtons As FlowLayoutPanel
    Friend WithEvents mbShutdown As MenuButton
    Friend WithEvents pnLogHost As PanelEx
    Friend WithEvents pnStatusHost As PanelEx
    Friend WithEvents flpNav As FlowLayoutPanel
    Friend WithEvents bnLog As ButtonEx
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
        Me.bnAbort = New ButtonEx()
        Me.laWhenfinisheddo = New LabelEx()
        Me.NotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.bnJobs = New ButtonEx()
        Me.flpButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.mbShutdown = New MenuButton()
        Me.bnLog = New ButtonEx()
        Me.bnMenu = New ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.pnLogHost = New PanelEx()
        Me.pnStatusHost = New PanelEx()
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

    Private Shared ReadOnly _priorities() As KeyValuePair(Of ProcessPriorityClass, String) = {
        New KeyValuePair(Of ProcessPriorityClass, String)(ProcessPriorityClass.High, "High"),
        New KeyValuePair(Of ProcessPriorityClass, String)(ProcessPriorityClass.AboveNormal, "Above Normal"),
        New KeyValuePair(Of ProcessPriorityClass, String)(ProcessPriorityClass.Normal, "Normal"),
        New KeyValuePair(Of ProcessPriorityClass, String)(ProcessPriorityClass.BelowNormal, "Below Normal"),
        New KeyValuePair(Of ProcessPriorityClass, String)(ProcessPriorityClass.Idle, "Idle")
    }

    Private TaskbarButtonCreatedMessage As Integer
    Private StopAfterCurrentJobMenuItem As MenuItemEx
    Private ProgressReformattingMenuItem As MenuItemEx
    Private OutputHighlightingMenuItem As MenuItemEx
    Private CMS As ContextMenuStripEx

    Private Const _priorityMenuName As String = "Priority"
    Private Const _themeMenuName As String = "Temporary Theme"

    Property Taskbar As Taskbar

    Sub New()
        InitializeComponent()
        AddHandler Application.ThreadException, AddressOf g.OnUnhandledException
        mbShutdown.Add(System.Enum.GetValues(GetType(ShutdownMode)).Cast(Of Object))
        Icon = g.Icon
        NotifyIcon.Icon = g.Icon
        NotifyIcon.Text = "StaxRip"
        TaskbarButtonCreatedMessage = Native.RegisterWindowMessage("TaskbarButtonCreated")
        RestoreClientSize(42, 28)

        CMS = New ContextMenuStripEx(components)
        CMS.Form = Me
        CMS.Add("Suspend", AddressOf ProcController.Suspend, "Suspends the current process, might not work with all tools.")
        CMS.Add("Resume", AddressOf ProcController.ResumeProcs, "Resumes a suspended process.")

        For Each priority In _priorities
            CMS.Add($"{_priorityMenuName} | {priority.Value}", Sub() ProcController.SetProcessPriority(priority.Key))
        Next

        CMS.Add("-")
        CMS.Add("Abort", AddressOf Abort, Keys.Escape, "Aborts all job processing of this StaxRip instance.")
        CMS.Add("Skip", AddressOf Skip, "Aborts the current job and continues with the next job.")
        StopAfterCurrentJobMenuItem = CMS.Add("Stop After Current Job", AddressOf StopAfterCurrentJob, "Stops all job processing after the current job.")
        CMS.Add("-")
        OutputHighlightingMenuItem = CMS.Add("Output Highlighting", AddressOf SetOutputHighlighting, Keys.Control Or Keys.H)
        ProgressReformattingMenuItem = CMS.Add("Progress Reformatting", AddressOf SetProgressReformatting, Keys.Control Or Keys.R)

        For Each theme In ThemeManager.Themes
            CMS.Add($"{_themeMenuName} | {theme.Name}", Sub() ThemeManager.SetCurrentTheme(theme.Name))
        Next

        CMS.Add("-")
        CMS.Add("Jobs", AddressOf JobsForm.ShowForm, Keys.F6, "Shows the Jobs dialog.")
        CMS.Add("Log", AddressOf g.DefaultCommands.ShowLogFile, Keys.F7, "Shows the log file.")
        CMS.Add("-")
        CMS.Add("Help", AddressOf ShowHelp).ShortcutKeyDisplayString = "F1"

        CMS.ApplyMarginFix()

        ApplyTheme()

        AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
    End Sub

    Sub OnThemeChanged(theme As Theme)
        ApplyTheme(theme)
    End Sub

    Sub ApplyTheme()
        ApplyTheme(ThemeManager.CurrentTheme)
    End Sub

    Sub ApplyTheme(theme As Theme)
        If DesignHelp.IsDesignMode Then
            Exit Sub
        End If

        BackColor = theme.ProcessingForm.BackColor
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
        ProcController.LastActivation = Environment.TickCount
    End Sub

    Shared Property WasHandleCreated As Boolean

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        WasHandleCreated = True
    End Sub

    Sub StopAfterCurrentJob()
        g.StopAfterCurrentJob = Not g.StopAfterCurrentJob
    End Sub

    Sub SetProgressReformatting()
        s.ProgressReformatting = Not s.ProgressReformatting
    End Sub

    Sub SetOutputHighlighting()
        OutputHighlightingMenuItem.Checked = Not OutputHighlightingMenuItem.Checked
        ProcController.SetOutputHighlighting(OutputHighlightingMenuItem.Checked, ThemeManager.CurrentTheme)
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
        OutputHighlightingMenuItem.Checked = s.OutputHighlighting
        mbShutdown.Value = CType(Registry.CurrentUser.GetInt("Software\" + Application.ProductName, "ShutdownMode"), ShutdownMode)
        ApplyTheme()
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

    Sub bnMenu_Click(sender As Object, e As EventArgs) Handles bnMenu.Click
        StopAfterCurrentJobMenuItem.Enabled = g.IsJobProcessing
        StopAfterCurrentJobMenuItem.Checked = g.StopAfterCurrentJob

        OutputHighlightingMenuItem.Enabled = g.IsJobProcessing

        ProgressReformattingMenuItem.Enabled = g.IsJobProcessing
        ProgressReformattingMenuItem.Checked = s.ProgressReformatting

        Dim priorityText = _priorities.First(Function(x) x.Key = ProcController.GetProcessPriority()).Value
        For Each item In CMS.GetItems().OfType(Of MenuItemEx).Where(Function(i) i.Path.StartsWith(_priorityMenuName + " | "))
            item.Checked = item.Path.EndsWith($" | {priorityText}")
        Next

        For Each item In CMS.GetItems().OfType(Of MenuItemEx).Where(Function(i) i.Path.StartsWith(_themeMenuName + " | "))
            item.Checked = item.Path.EndsWith($" | {ThemeManager.CurrentTheme.Name}")
        Next

        CMS.Show(bnMenu, New Point(0, 0), ToolStripDropDownDirection.AboveRight)
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
