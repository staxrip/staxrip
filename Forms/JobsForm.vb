
Imports System.Threading
Imports System.Threading.Tasks

Imports StaxRip.UI

Friend Class JobsForm
    Inherits DialogBase

#Region " Designer "
    Friend WithEvents bnStart As StaxRip.UI.ButtonEx
    Friend WithEvents bnDown As StaxRip.UI.ButtonEx
    Friend WithEvents bnUp As StaxRip.UI.ButtonEx
    Friend WithEvents bnLoad As StaxRip.UI.ButtonEx
    Friend WithEvents lv As ListViewEx
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents tlpButtonsLeft As TableLayoutPanel
    Friend WithEvents tlpButtonsRight As TableLayoutPanel
    Friend WithEvents bnMenu As ButtonEx
    Friend WithEvents bnRemove As ButtonEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Sub InitializeComponent()
        Me.bnDown = New StaxRip.UI.ButtonEx()
        Me.bnUp = New StaxRip.UI.ButtonEx()
        Me.bnStart = New StaxRip.UI.ButtonEx()
        Me.bnLoad = New StaxRip.UI.ButtonEx()
        Me.lv = New StaxRip.UI.ListViewEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpButtonsRight = New System.Windows.Forms.TableLayoutPanel()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
        Me.bnRemove = New StaxRip.UI.ButtonEx()
        Me.tlpButtonsLeft = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpMain.SuspendLayout()
        Me.tlpButtonsRight.SuspendLayout()
        Me.tlpButtonsLeft.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnDown
        '
        Me.bnDown.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnDown.Enabled = False
        Me.bnDown.Location = New System.Drawing.Point(8, 0)
        Me.bnDown.Margin = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.bnDown.Size = New System.Drawing.Size(100, 70)
        '
        'bnUp
        '
        Me.bnUp.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnUp.Enabled = False
        Me.bnUp.Location = New System.Drawing.Point(726, 0)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(0, 0, 8, 0)
        Me.bnUp.Size = New System.Drawing.Size(100, 70)
        '
        'bnStart
        '
        Me.bnStart.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnStart.Location = New System.Drawing.Point(0, 0)
        Me.bnStart.Margin = New System.Windows.Forms.Padding(0)
        Me.bnStart.Size = New System.Drawing.Size(250, 70)
        Me.bnStart.Text = "Start"
        '
        'bnLoad
        '
        Me.bnLoad.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnLoad.Location = New System.Drawing.Point(584, 0)
        Me.bnLoad.Margin = New System.Windows.Forms.Padding(0)
        Me.bnLoad.Size = New System.Drawing.Size(250, 70)
        Me.bnLoad.Text = "Load"
        '
        'lv
        '
        Me.lv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.lv, 2)
        Me.lv.HideSelection = False
        Me.lv.Location = New System.Drawing.Point(15, 15)
        Me.lv.Margin = New System.Windows.Forms.Padding(0, 0, 0, 14)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(1668, 594)
        Me.lv.TabIndex = 7
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.Controls.Add(Me.tlpButtonsRight, 1, 1)
        Me.tlpMain.Controls.Add(Me.tlpButtonsLeft, 0, 1)
        Me.tlpMain.Controls.Add(Me.lv, 0, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.Padding = New System.Windows.Forms.Padding(15)
        Me.tlpMain.RowCount = 2
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1698, 708)
        Me.tlpMain.TabIndex = 15
        '
        'tlpButtonsRight
        '
        Me.tlpButtonsRight.AutoSize = True
        Me.tlpButtonsRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpButtonsRight.ColumnCount = 5
        Me.tlpButtonsRight.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpButtonsRight.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpButtonsRight.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpButtonsRight.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpButtonsRight.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpButtonsRight.Controls.Add(Me.bnMenu, 2, 0)
        Me.tlpButtonsRight.Controls.Add(Me.bnLoad, 4, 0)
        Me.tlpButtonsRight.Controls.Add(Me.bnRemove, 3, 0)
        Me.tlpButtonsRight.Controls.Add(Me.bnDown, 0, 0)
        Me.tlpButtonsRight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpButtonsRight.Location = New System.Drawing.Point(849, 623)
        Me.tlpButtonsRight.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpButtonsRight.Name = "tlpButtonsRight"
        Me.tlpButtonsRight.RowCount = 1
        Me.tlpButtonsRight.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpButtonsRight.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70.0!))
        Me.tlpButtonsRight.Size = New System.Drawing.Size(834, 70)
        Me.tlpButtonsRight.TabIndex = 9
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnMenu.Location = New System.Drawing.Point(204, 0)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(0)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(100, 70)
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnRemove.Location = New System.Drawing.Point(319, 0)
        Me.bnRemove.Margin = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.bnRemove.Size = New System.Drawing.Size(250, 70)
        Me.bnRemove.Text = "Remove"
        '
        'tlpButtonsLeft
        '
        Me.tlpButtonsLeft.AutoSize = True
        Me.tlpButtonsLeft.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpButtonsLeft.ColumnCount = 2
        Me.tlpButtonsLeft.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpButtonsLeft.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpButtonsLeft.Controls.Add(Me.bnStart, 0, 0)
        Me.tlpButtonsLeft.Controls.Add(Me.bnUp, 1, 0)
        Me.tlpButtonsLeft.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpButtonsLeft.Location = New System.Drawing.Point(15, 623)
        Me.tlpButtonsLeft.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpButtonsLeft.Name = "tlpButtonsLeft"
        Me.tlpButtonsLeft.RowCount = 1
        Me.tlpButtonsLeft.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpButtonsLeft.Size = New System.Drawing.Size(834, 70)
        Me.tlpButtonsLeft.TabIndex = 8
        '
        'JobsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1698, 708)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(0, 2, 0, 2)
        Me.MinimumSize = New System.Drawing.Size(323, 204)
        Me.Name = "JobsForm"
        Me.Text = "Jobs"
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.tlpButtonsRight.ResumeLayout(False)
        Me.tlpButtonsLeft.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private FileWatcher As New FileSystemWatcher
    Private IsLoading As Boolean
    Private Tip As String = "The job list can be processed by multiple StaxRip instances in parallel."
    Private BlockSave As Boolean

    Sub New()
        InitializeComponent()
        RestoreClientSize(40, 20)

        bnUp.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnDown.Image = ImageHelp.GetSymbolImage(Symbol.Down)

        KeyPreview = True

        lv.UpButton = bnUp
        lv.DownButton = bnDown
        lv.RemoveButton = bnRemove
        lv.RightClickOnlyForMenu = True

        lv.SingleSelectionButtons = {bnLoad}
        lv.CheckBoxes = True
        lv.EnableListBoxMode()
        lv.ItemCheckProperty = NameOf(Job.Active)
        lv.AddItems(JobManager.GetJobs())
        lv.SelectFirst()

        Dim cms As New ContextMenuStripEx()
        cms.Form = Me
        bnMenu.ContextMenuStrip = cms
        lv.ContextMenuStrip = cms

        AddHandler Disposed, Sub()
                                 FileWatcher.Dispose()
                                 cms.Dispose()
                             End Sub

        AddHandler lv.ItemRemoved, Sub(item)
                                       Dim fp = DirectCast(item.Tag, Job).Path

                                       If fp.StartsWith(Folder.Settings + "Batch Projects\") Then
                                           FileHelp.Delete(fp)
                                       End If
                                   End Sub

        cms.Add("Select All", Sub() SelectAll(), Keys.Control Or Keys.A, Function() lv.Items.Count > lv.SelectedItems.Count)
        cms.Add("Select None", Sub() SelectNone(), Keys.Shift Or Keys.A, Function() lv.SelectedItems.Count > 0)
        cms.Add("-")
        cms.Add("Check Selection", Sub() CheckSelection(), Keys.None, Function() lv.SelectedItems.Count > lv.CheckedItems.OfType(Of ListViewItem).Where(Function(item) item.Checked).Count)
        cms.Add("Check All", Sub() CheckAll(), Keys.Control Or Keys.Space, Function() lv.Items.Count > lv.CheckedItems.Count)
        cms.Add("-")
        cms.Add("Uncheck Selection", Sub() UncheckSelection(), Keys.None, Function() lv.SelectedItems.OfType(Of ListViewItem).Where(Function(item) item.Checked).Count > 0)
        cms.Add("Uncheck All", Sub() UncheckAll(), Keys.Shift Or Keys.Space, Function() lv.CheckedItems.Count > 0)
        cms.Add("-")
        cms.Add("Move Selection Up", Sub() bnUp.PerformClick(), Keys.Control Or Keys.Up, Function() lv.CanMoveUp).SetImage(Symbol.Up)
        cms.Add("Move Selection Down", Sub() bnDown.PerformClick(), Keys.Control Or Keys.Down, Function() lv.CanMoveDown).SetImage(Symbol.Down)
        cms.Add("-")
        cms.Add("Move Selection To Top", Sub() lv.MoveSelectionTop(), Keys.Control Or Keys.Home, Function() lv.CanMoveUp)
        cms.Add("Move Selection To Bottom", Sub() lv.MoveSelectionBottom(), Keys.Control Or Keys.End, Function() lv.CanMoveDown)
        cms.Add("-")
        cms.Add("Sort Alphabetically", Sub() lv.SortItems(), Keys.Control Or Keys.S, Function() lv.Items.Count > 1).SetImage(Symbol.Sort)
        cms.Add("Remove Selection", Sub() bnRemove.PerformClick(), Keys.Control Or Keys.Delete, Function() lv.SelectedItems.Count > 0).SetImage(Symbol.Remove)
        cms.Add("Load Selection", Sub() bnLoad.PerformClick(), Keys.Control Or Keys.L, Function() lv.SelectedItems.Count = 1)
    End Sub

    Sub UncheckAll()
        BlockSave = True

        For Each item As ListViewItem In lv.Items
            item.Checked = False
        Next

        BlockSave = False
        HandleItemsChanged()
    End Sub

    Sub UncheckSelection()
        BlockSave = True

        For Each item As ListViewItem In lv.SelectedItems
            item.Checked = False
        Next

        BlockSave = False
        HandleItemsChanged()
    End Sub

    Sub CheckAll()
        BlockSave = True

        For Each item As ListViewItem In lv.Items
            item.Checked = True
        Next

        BlockSave = False
        HandleItemsChanged()
    End Sub

    Sub CheckSelection()
        BlockSave = True

        For Each item As ListViewItem In lv.SelectedItems
            item.Checked = True
        Next

        BlockSave = False
        HandleItemsChanged()
    End Sub

    Sub SelectNone()
        BlockSave = True

        For Each item As ListViewItem In lv.Items
            item.Selected = False
        Next

        BlockSave = False
        HandleItemsChanged()
    End Sub

    Sub SelectAll()
        BlockSave = True

        For Each item As ListViewItem In lv.Items
            item.Selected = True
        Next

        BlockSave = False
        HandleItemsChanged()
    End Sub

    Sub HandleItemsChanged()
        If Not BlockSave Then
            SaveJobs()
            UpdateControls()
        End If
    End Sub

    Sub Reload(sender As Object, e As FileSystemEventArgs)
        Invoke(Sub()
                   If Not Disposing AndAlso Not IsDisposed Then
                       IsLoading = True
                       lv.Items.Clear()
                       lv.AddItems(JobManager.GetJobs())
                       lv.SelectFirst()
                       UpdateControls()
                       IsLoading = False
                   End If
               End Sub)
    End Sub

    Sub UpdateControls()
        Dim activeJobs = From item In lv.Items.OfType(Of ListViewItem)
                         Where DirectCast(item.Tag, Job).Active

        bnStart.Enabled = activeJobs.Count > 0
    End Sub

    Sub SaveJobs(sender As Object, e As EventArgs)
        SaveJobs()
    End Sub

    Sub SaveJobs()
        If IsLoading Then
            Exit Sub
        End If

        Dim jobs As New List(Of Job)

        For Each item As ListViewItem In lv.Items
            If Not item Is Nothing Then
                jobs.Add(DirectCast(item.Tag, Job))
            End If
        Next

        FileWatcher.EnableRaisingEvents = False
        JobManager.SaveJobs(jobs)
        FileWatcher.EnableRaisingEvents = True
    End Sub

    Sub bnStart_Click(sender As Object, e As EventArgs) Handles bnStart.Click
        If Not g.VerifyRequirements Then
            Exit Sub
        End If

        Documentation.ShowTip(Tip)
        g.StopAfterCurrentJob = False
        Close()

        If g.IsJobProcessing Then
            g.ShellExecute(Application.ExecutablePath, "-StartJobs")
        Else
            Task.Run(Sub()
                         Thread.Sleep(500)
                         g.MainForm.Invoke(Sub() g.ProcessJobs())
                     End Sub)
        End If
    End Sub

    Sub bnLoad_Click(sender As Object, e As EventArgs) Handles bnLoad.Click
        g.MainForm.LoadProject(lv.SelectedItem.ToString)
        Close()
    End Sub

    Protected Overrides Sub OnFormClosing(args As FormClosingEventArgs)
        MyBase.OnFormClosing(args)

        RemoveHandler FileWatcher.Changed, AddressOf Reload
        RemoveHandler FileWatcher.Created, AddressOf Reload
        RemoveHandler lv.ItemsChanged, AddressOf HandleItemsChanged
    End Sub

    Protected Overrides Sub OnLoad(args As EventArgs)
        MyBase.OnLoad(args)

        FileWatcher.Path = Folder.Settings
        FileWatcher.NotifyFilter = NotifyFilters.LastWrite Or NotifyFilters.CreationTime
        FileWatcher.Filter = "Jobs.dat"
        FileWatcher.EnableRaisingEvents = True

        AddHandler FileWatcher.Changed, AddressOf Reload
        AddHandler FileWatcher.Created, AddressOf Reload
        AddHandler lv.ItemsChanged, AddressOf HandleItemsChanged

        UpdateControls()
    End Sub

    Sub JobsForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        MsgInfo(Tip)
    End Sub

    Sub ShowForm()
        Using form As New JobsForm()
            form.ShowDialog()
        End Using
    End Sub
End Class
