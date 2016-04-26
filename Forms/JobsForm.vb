Imports StaxRip.UI

Imports System.Threading
Imports System.Runtime.Serialization.Formatters.Binary

Friend Class JobsForm
    Inherits DialogBase

#Region " Designer "

    Friend WithEvents bnRemove As ButtonEx
    Private WithEvents bnClose As StaxRip.UI.ButtonEx
    Friend WithEvents bnStart As StaxRip.UI.ButtonEx
    Friend WithEvents bnNew As StaxRip.UI.ButtonEx
    Friend WithEvents bnDown As StaxRip.UI.ButtonEx
    Friend WithEvents bnUp As StaxRip.UI.ButtonEx
    Friend WithEvents bnLoad As StaxRip.UI.ButtonEx
    Friend WithEvents lv As ListViewEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.bnDown = New StaxRip.UI.ButtonEx()
        Me.bnRemove = New StaxRip.UI.ButtonEx()
        Me.bnUp = New StaxRip.UI.ButtonEx()
        Me.bnClose = New StaxRip.UI.ButtonEx()
        Me.bnStart = New StaxRip.UI.ButtonEx()
        Me.bnNew = New StaxRip.UI.ButtonEx()
        Me.bnLoad = New StaxRip.UI.ButtonEx()
        Me.lv = New StaxRip.UI.ListViewEx()
        Me.SuspendLayout()
        '
        'bnDown
        '
        Me.bnDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnDown.Enabled = False
        Me.bnDown.Location = New System.Drawing.Point(478, 413)
        Me.bnDown.Margin = New System.Windows.Forms.Padding(4)
        Me.bnDown.Size = New System.Drawing.Size(36, 36)
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemove.Location = New System.Drawing.Point(711, 413)
        Me.bnRemove.Size = New System.Drawing.Size(100, 36)
        Me.bnRemove.Text = "Remove"
        '
        'bnUp
        '
        Me.bnUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnUp.Enabled = False
        Me.bnUp.Location = New System.Drawing.Point(434, 413)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(4)
        Me.bnUp.Size = New System.Drawing.Size(36, 36)
        '
        'bnClose
        '
        Me.bnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnClose.Location = New System.Drawing.Point(817, 413)
        Me.bnClose.Size = New System.Drawing.Size(100, 36)
        Me.bnClose.Text = "Close"
        '
        'bnStart
        '
        Me.bnStart.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnStart.Location = New System.Drawing.Point(12, 413)
        Me.bnStart.Size = New System.Drawing.Size(100, 36)
        Me.bnStart.Text = "Start"
        '
        'bnNew
        '
        Me.bnNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnNew.Location = New System.Drawing.Point(118, 413)
        Me.bnNew.Size = New System.Drawing.Size(228, 36)
        Me.bnNew.Text = "Start in new instance"
        '
        'bnLoad
        '
        Me.bnLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnLoad.Location = New System.Drawing.Point(605, 413)
        Me.bnLoad.Size = New System.Drawing.Size(100, 36)
        Me.bnLoad.Text = "Load"
        '
        'lv
        '
        Me.lv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lv.ItemCheckProperty = Nothing
        Me.lv.Location = New System.Drawing.Point(12, 12)
        Me.lv.MultiSelectionButtons = Nothing
        Me.lv.Name = "lv"
        Me.lv.SingleSelectionButtons = Nothing
        Me.lv.Size = New System.Drawing.Size(905, 395)
        Me.lv.TabIndex = 7
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'JobsForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnClose
        Me.ClientSize = New System.Drawing.Size(929, 461)
        Me.Controls.Add(Me.lv)
        Me.Controls.Add(Me.bnLoad)
        Me.Controls.Add(Me.bnDown)
        Me.Controls.Add(Me.bnUp)
        Me.Controls.Add(Me.bnNew)
        Me.Controls.Add(Me.bnStart)
        Me.Controls.Add(Me.bnClose)
        Me.Controls.Add(Me.bnRemove)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Margin = New System.Windows.Forms.Padding(1)
        Me.MinimumSize = New System.Drawing.Size(365, 259)
        Me.Name = "JobsForm"
        Me.Text = "Jobs"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private FileWatcher As New FileSystemWatcher

    Sub New()
        MyBase.New()
        InitializeComponent()

        Dim pad = New Padding(CInt(bnUp.Width / 8))

        bnUp.Padding = pad
        bnUp.ZoomImage = My.Resources.ArrowLeft
        bnUp.ZoomImage.RotateFlip(RotateFlipType.Rotate90FlipNone)

        bnDown.Padding = pad
        bnDown.ZoomImage = My.Resources.ArrowLeft
        bnDown.ZoomImage.RotateFlip(RotateFlipType.Rotate270FlipNone)

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

        FileWatcher.Path = Paths.SettingsDir
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

        bnNew.Enabled = activeJobs.Count > 0
        bnStart.Enabled = bnNew.Enabled AndAlso Not ProcessForm.IsActive
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
                Using stream As New FileStream(Paths.SettingsDir + "Jobs.dat",
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
        Dim jobsPath = Paths.SettingsDir + "Jobs.dat"
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
        If g.MainForm.IsSaveCanceled Then Exit Sub
        DialogResult = DialogResult.OK
    End Sub

    Private Sub bnNew_Click(sender As Object, e As EventArgs) Handles bnNew.Click
        If Not ProcessForm.IsActive Then If g.MainForm.IsSaveCanceled Then Exit Sub
        g.ShellExecute(Application.ExecutablePath, "-Perform/RunJobs")
    End Sub

    Private Sub bnLoad_Click(sender As Object, e As EventArgs) Handles bnLoad.Click
        g.MainForm.LoadProject(lv.SelectedItem.ToString)
    End Sub

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