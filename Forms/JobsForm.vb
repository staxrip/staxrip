Imports StaxRip.UI

Imports System.Threading
Imports System.Runtime.Serialization.Formatters.Binary

Friend Class JobsForm
    Inherits DialogBase

#Region " Designer "

    Friend WithEvents bnRemove As ButtonEx
    Friend WithEvents bnStart As StaxRip.UI.ButtonEx
    Friend WithEvents bnDown As StaxRip.UI.ButtonEx
    Friend WithEvents bnUp As StaxRip.UI.ButtonEx
    Friend WithEvents bnLoad As StaxRip.UI.ButtonEx
    Friend WithEvents lv As ListViewEx
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel1.SuspendLayout()
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
        Me.bnRemove.Location = New System.Drawing.Point(358, 0)
        Me.bnRemove.Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.bnRemove.Size = New System.Drawing.Size(200, 70)
        Me.bnRemove.Text = "Remove"
        '
        'bnUp
        '
        Me.bnUp.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnUp.Enabled = False
        Me.bnUp.Location = New System.Drawing.Point(482, 0)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(0, 0, 5, 0)
        Me.bnUp.Size = New System.Drawing.Size(70, 70)
        '
        'bnStart
        '
        Me.bnStart.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnStart.Location = New System.Drawing.Point(0, 0)
        Me.bnStart.Margin = New System.Windows.Forms.Padding(0)
        Me.bnStart.Size = New System.Drawing.Size(200, 70)
        Me.bnStart.Text = "Start"
        '
        'bnLoad
        '
        Me.bnLoad.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnLoad.Location = New System.Drawing.Point(148, 0)
        Me.bnLoad.Margin = New System.Windows.Forms.Padding(0)
        Me.bnLoad.Size = New System.Drawing.Size(200, 70)
        Me.bnLoad.Text = "Load"
        '
        'lv
        '
        Me.lv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.lv, 2)
        Me.lv.Location = New System.Drawing.Point(0, 0)
        Me.lv.Margin = New System.Windows.Forms.Padding(0, 0, 0, 14)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(1115, 264)
        Me.lv.TabIndex = 7
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel3, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lv, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(20, 19)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1115, 348)
        Me.TableLayoutPanel1.TabIndex = 15
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
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(557, 278)
        Me.TableLayoutPanel3.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(558, 70)
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
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 278)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(557, 70)
        Me.TableLayoutPanel2.TabIndex = 8
        '
        'JobsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1155, 386)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(0, 2, 0, 2)
        Me.MinimumSize = New System.Drawing.Size(323, 204)
        Me.Name = "JobsForm"
        Me.Text = "Jobs"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private FileWatcher As New FileSystemWatcher

    Sub New()
        MyBase.New()
        InitializeComponent()

        ClientSize = New Size(FontHeight * 35, FontHeight * 20)

        Dim pad = New Padding(CInt(bnUp.Width / 8))

        bnUp.Padding = pad
        bnUp.ZoomImage = ImageHelp.GetSymbolImage(Symbol.Up)

        bnDown.Padding = pad
        bnDown.ZoomImage = ImageHelp.GetSymbolImage(Symbol.Down)

        KeyPreview = True

        lv.UpButton = bnUp
        lv.DownButton = bnDown
        lv.RemoveButton = bnRemove
        lv.SingleSelectionButtons = {bnLoad}
        lv.CheckBoxes = True
        lv.EnableListBoxMode()
        lv.ItemCheckProperty = NameOf(StringBooleanPair.Value)
        lv.AddItems(GetJobs())
        lv.SelectFirst()
        UpdateControls()

        FileWatcher.Path = Folder.Settings
        FileWatcher.NotifyFilter = NotifyFilters.LastWrite Or NotifyFilters.CreationTime
        FileWatcher.Filter = "Jobs.dat"
        AddHandler FileWatcher.Changed, AddressOf Reload
        AddHandler FileWatcher.Created, AddressOf Reload
        FileWatcher.EnableRaisingEvents = True

        AddHandler lv.ItemsChanged, Sub()
                                        SaveJobs()
                                        UpdateControls()
                                    End Sub
    End Sub

    Private IsLoading As Boolean

    Sub Reload(sender As Object, e As FileSystemEventArgs)
        If IsHandleCreated Then Invoke(Sub()
                                           IsLoading = True
                                           lv.Items.Clear()
                                           lv.AddItems(GetJobs())
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

    Private Sub JobsForm_HelpRequested() Handles Me.HelpRequested
        Dim f As New HelpForm
        f.Doc.WriteStart(Text)
        f.Doc.WriteP("The Jobs dialog allows to encode a batch of projects.")
        f.Show()
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
        SaveJobs(jobs)
        FileWatcher.EnableRaisingEvents = True
    End Sub

    Shared Sub SaveJobs(jobs As List(Of StringBooleanPair))
        Dim formatter As New BinaryFormatter
        Dim counter As Integer

        While True
            Try
                Using stream As New FileStream(Folder.Settings + "Jobs.dat",
                                               FileMode.Create,
                                               FileAccess.ReadWrite,
                                               FileShare.None)

                    formatter.Serialize(stream, jobs)
                End Using

                Exit While
            Catch ex As Exception
                Thread.Sleep(500)
                counter += 1
                If counter > 9 Then Throw ex
            End Try
        End While
    End Sub

    Shared Function GetJobs() As List(Of StringBooleanPair)
        Dim formatter As New BinaryFormatter
        Dim jobsPath = Folder.Settings + "Jobs.dat"
        Dim counter As Integer

        If File.Exists(jobsPath) Then
            While True
                Try
                    Using stream As New FileStream(jobsPath,
                                                   FileMode.Open,
                                                   FileAccess.ReadWrite,
                                                   FileShare.None)

                        Return DirectCast(formatter.Deserialize(stream), List(Of StringBooleanPair))
                    End Using

                    Exit While
                Catch ex As Exception
                    Thread.Sleep(500)
                    counter += 1

                    If counter > 9 Then
                        g.ShowException(ex, "Failed to load job file:" + BR2 + jobsPath)
                        FileHelp.Delete(jobsPath)
                        Exit While
                    End If
                End Try
            End While
        End If

        Return New List(Of StringBooleanPair)
    End Function

    Shared Sub RemoveJob(jobPath As String)
        Dim jobs = GetJobs()

        For Each i In jobs.ToArray
            If i.Key = jobPath Then
                jobs.Remove(i)
                SaveJobs(jobs)
            End If
        Next
    End Sub

    Shared Sub ActivateJob(jobPath As String, Optional isActive As Boolean = True)
        Dim jobs = GetJobs()

        For Each i In jobs
            If i.Key = jobPath Then i.Value = isActive
        Next

        SaveJobs(jobs)
    End Sub

    Shared Sub AddJob(jobPath As String, Optional isActive As Boolean = True)
        Dim jobs = GetJobs()

        For Each i In jobs.ToArray
            If i.Key = jobPath Then jobs.Remove(i)
        Next

        jobs.Add(New StringBooleanPair(jobPath, isActive))
        SaveJobs(jobs)
    End Sub

    Private Sub bnStart_Click(sender As Object, e As EventArgs) Handles bnStart.Click
        g.DefaultCommands.StartJobs()
    End Sub

    Private Sub bnLoad_Click(sender As Object, e As EventArgs) Handles bnLoad.Click
        g.MainForm.LoadProject(lv.SelectedItem.ToString)
    End Sub

    Shared ReadOnly Property ActiveJobs As IEnumerable(Of StringBooleanPair)
        Get
            Return From i In JobsForm.GetJobs() Where i.Value = True
        End Get
    End Property

    Protected Overrides Sub Dispose(disposing As Boolean)
        MyBase.Dispose(disposing)
        FileWatcher.Dispose()
    End Sub

    Private Sub JobsForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyData
            Case Keys.Control Or Keys.A
                For Each i As ListViewItem In lv.Items
                    i.Selected = True
                Next
            Case Keys.Delete
                lv.RemoveSelection()
        End Select
    End Sub
End Class