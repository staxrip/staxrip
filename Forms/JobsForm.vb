Imports System.Threading
Imports System.Threading.Tasks
Imports StaxRip.UI

Friend Class JobsForm
    Inherits DialogBase

#Region " Designer "

    Friend WithEvents bnRemove As ButtonEx
    Friend WithEvents bnStart As StaxRip.UI.ButtonEx
    Friend WithEvents bnDown As StaxRip.UI.ButtonEx
    Friend WithEvents bnUp As StaxRip.UI.ButtonEx
    Friend WithEvents bnLoad As StaxRip.UI.ButtonEx
    Friend WithEvents lv As ListViewEx
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.bnDown = New StaxRip.UI.ButtonEx()
        Me.bnRemove = New StaxRip.UI.ButtonEx()
        Me.bnUp = New StaxRip.UI.ButtonEx()
        Me.bnStart = New StaxRip.UI.ButtonEx()
        Me.bnLoad = New StaxRip.UI.ButtonEx()
        Me.lv = New StaxRip.UI.ListViewEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpMain.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnDown
        '
        Me.bnDown.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnDown.Enabled = False
        Me.bnDown.Location = New System.Drawing.Point(5, 0)
        Me.bnDown.Margin = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.bnDown.Size = New System.Drawing.Size(70, 70)
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnRemove.Location = New System.Drawing.Point(392, 0)
        Me.bnRemove.Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.bnRemove.Size = New System.Drawing.Size(250, 70)
        Me.bnRemove.Text = "Remove"
        '
        'bnUp
        '
        Me.bnUp.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnUp.Enabled = False
        Me.bnUp.Location = New System.Drawing.Point(566, 0)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(0, 0, 5, 0)
        Me.bnUp.Size = New System.Drawing.Size(70, 70)
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
        Me.bnLoad.Location = New System.Drawing.Point(132, 0)
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
        Me.lv.Location = New System.Drawing.Point(15, 15)
        Me.lv.Margin = New System.Windows.Forms.Padding(0, 0, 0, 14)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(1283, 543)
        Me.lv.TabIndex = 7
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.Controls.Add(Me.TableLayoutPanel3, 1, 1)
        Me.tlpMain.Controls.Add(Me.TableLayoutPanel2, 0, 1)
        Me.tlpMain.Controls.Add(Me.lv, 0, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.Padding = New System.Windows.Forms.Padding(15)
        Me.tlpMain.RowCount = 2
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1313, 657)
        Me.tlpMain.TabIndex = 15
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.AutoSize = True
        Me.TableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel3.ColumnCount = 4
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.Controls.Add(Me.bnDown, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.bnLoad, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.bnRemove, 2, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(656, 572)
        Me.TableLayoutPanel3.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(642, 70)
        Me.TableLayoutPanel3.TabIndex = 9
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.AutoSize = True
        Me.TableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.bnStart, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.bnUp, 1, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(15, 572)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(641, 70)
        Me.TableLayoutPanel2.TabIndex = 8
        '
        'JobsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1313, 657)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(0, 2, 0, 2)
        Me.MinimumSize = New System.Drawing.Size(323, 204)
        Me.Name = "JobsForm"
        Me.Text = "Jobs"
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private FileWatcher As New FileSystemWatcher
    Private IsLoading As Boolean

    Sub New()
        MyBase.New()
        InitializeComponent()

        ScaleClientSize(38, 20)
        bnUp.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnDown.Image = ImageHelp.GetSymbolImage(Symbol.Down)

        KeyPreview = True

        lv.UpButton = bnUp
        lv.DownButton = bnDown
        lv.RemoveButton = bnRemove
        lv.SingleSelectionButtons = {bnLoad}
        lv.CheckBoxes = True
        lv.EnableListBoxMode()
        lv.ItemCheckProperty = NameOf(StringBooleanPair.Value)
        lv.AddItems(Job.GetJobs())
        lv.SelectFirst()

        UpdateControls()

        FileWatcher.Path = Folder.Settings
        FileWatcher.NotifyFilter = NotifyFilters.LastWrite Or NotifyFilters.CreationTime
        FileWatcher.Filter = "Jobs.dat"
        AddHandler FileWatcher.Changed, AddressOf Reload
        AddHandler FileWatcher.Created, AddressOf Reload
        AddHandler lv.ItemsChanged, AddressOf HandleItemsChanged
        FileWatcher.EnableRaisingEvents = True
    End Sub

    Sub HandleItemsChanged()
        SaveJobs()
        UpdateControls()
    End Sub

    Sub Reload(sender As Object, e As FileSystemEventArgs)
        Invoke(Sub()
                   If IsDisposed Then Exit Sub
                   IsLoading = True
                   lv.Items.Clear()
                   lv.AddItems(Job.GetJobs())
                   lv.SelectFirst()
                   UpdateControls()
                   IsLoading = False
               End Sub)
    End Sub

    Private Sub UpdateControls()
        Dim activeJobs = From item In lv.Items.OfType(Of ListViewItem)
                         Where DirectCast(item.Tag, StringBooleanPair).Value

        bnStart.Enabled = activeJobs.Count > 0
    End Sub

    Sub SaveJobs(sender As Object, e As EventArgs)
        SaveJobs()
    End Sub

    Sub SaveJobs()
        If IsLoading Then Exit Sub
        Dim jobs As New List(Of StringBooleanPair)

        For Each i As ListViewItem In lv.Items
            jobs.Add(DirectCast(i.Tag, StringBooleanPair))
        Next

        FileWatcher.EnableRaisingEvents = False
        Job.SaveJobs(jobs)
        FileWatcher.EnableRaisingEvents = True
    End Sub

    Private Sub bnStart_Click(sender As Object, e As EventArgs) Handles bnStart.Click
        If Not s.Storage.GetBool("proc form help") Then
            ShowHelp()
            s.Storage.SetBool("proc form help", True)
        End If

        Close()

        If g.IsProcessing Then
            g.StartProcess(Application.ExecutablePath, "-StartJobs")
        Else
            Task.Run(Sub()
                         Thread.Sleep(500)
                         g.MainForm.Invoke(Sub() g.ProcessJobs())
                     End Sub)
        End If
    End Sub

    Private Sub bnLoad_Click(sender As Object, e As EventArgs) Handles bnLoad.Click
        g.MainForm.LoadProject(lv.SelectedItem.ToString)
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        MyBase.Dispose(disposing)
        FileWatcher.Dispose()
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        Select Case e.KeyData
            Case Keys.Control Or Keys.A
                For Each i As ListViewItem In lv.Items
                    i.Selected = True
                Next
            Case Keys.Delete
                lv.RemoveSelection()
        End Select

        MyBase.OnKeyDown(e)
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        MyBase.OnFormClosing(e)
        RemoveHandler FileWatcher.Changed, AddressOf Reload
        RemoveHandler FileWatcher.Created, AddressOf Reload
        RemoveHandler lv.ItemsChanged, AddressOf HandleItemsChanged
    End Sub

    Private Sub JobsForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        ShowHelp()
    End Sub

    Sub ShowHelp()
        MsgInfo("Please note that the job list can be processed by multiple StaxRip instances in parallel." + BR2 +
                "Multiple instances work most efficiently when the files are located on diffeent HDDs.")
    End Sub
End Class