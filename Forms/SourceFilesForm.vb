Imports StaxRip.UI

Imports System.Text.RegularExpressions

Imports VB6 = Microsoft.VisualBasic

Public Class SourceFilesForm
    Inherits DialogBase

#Region " Designer "
    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    Friend WithEvents lb As ListBoxEx
    Friend WithEvents bnDown As ButtonEx
    Friend WithEvents bnUp As ButtonEx
    Friend WithEvents bnRemove As ButtonEx
    Friend WithEvents bnAdd As ButtonEx
    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents cbCreateJobs As System.Windows.Forms.CheckBox
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents flpButtons As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents DirTree As MultiFolderTree

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SourceFilesForm))
        Me.lb = New StaxRip.UI.ListBoxEx()
        Me.bnDown = New StaxRip.UI.ButtonEx()
        Me.bnUp = New StaxRip.UI.ButtonEx()
        Me.bnRemove = New StaxRip.UI.ButtonEx()
        Me.bnAdd = New StaxRip.UI.ButtonEx()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.cbCreateJobs = New System.Windows.Forms.CheckBox()
        Me.DirTree = New StaxRip.UI.MultiFolderTree()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.flpButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.flpButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'lb
        '
        Me.lb.AllowDrop = True
        Me.lb.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lb.Enabled = False
        Me.lb.FormattingEnabled = True
        Me.lb.IntegralHeight = False
        Me.lb.ItemHeight = 25
        Me.lb.Location = New System.Drawing.Point(12, 13)
        Me.lb.Name = "lb"
        Me.lb.Size = New System.Drawing.Size(653, 341)
        Me.lb.TabIndex = 2
        Me.lb.Visible = False
        '
        'bnDown
        '
        Me.bnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnDown.Location = New System.Drawing.Point(3, 170)
        Me.bnDown.Size = New System.Drawing.Size(100, 34)
        Me.bnDown.Text = "&Down"
        '
        'bnUp
        '
        Me.bnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnUp.Location = New System.Drawing.Point(3, 130)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(3, 50, 3, 3)
        Me.bnUp.Size = New System.Drawing.Size(100, 34)
        Me.bnUp.Text = "&Up"
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemove.Location = New System.Drawing.Point(3, 43)
        Me.bnRemove.Size = New System.Drawing.Size(100, 34)
        Me.bnRemove.Text = "&Remove"
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAdd.Location = New System.Drawing.Point(3, 3)
        Me.bnAdd.Size = New System.Drawing.Size(100, 34)
        Me.bnAdd.Text = "&Add..."
        Me.TipProvider.SetTipText(Me.bnAdd, "Adds source files with a multi-select enabled file browser. Alternative methods a" & _
        "re Drag & Drop and command line, in all cases files are alphabetically sorted.")
        '
        'cbCreateJobs
        '
        Me.cbCreateJobs.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbCreateJobs.AutoSize = True
        Me.cbCreateJobs.Checked = True
        Me.cbCreateJobs.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbCreateJobs.Location = New System.Drawing.Point(400, 370)
        Me.cbCreateJobs.Name = "cbCreateJobs"
        Me.cbCreateJobs.Size = New System.Drawing.Size(129, 29)
        Me.cbCreateJobs.TabIndex = 3
        Me.cbCreateJobs.Text = "&Create Jobs"
        Me.TipProvider.SetTipText(Me.cbCreateJobs, "Opens files in batch mode and creates jobs. Disabling this option is useful to ba" & _
        "tch perform preparation (demuxing etc.).")
        Me.cbCreateJobs.UseVisualStyleBackColor = True
        Me.cbCreateJobs.Visible = False
        '
        'DirTree
        '
        Me.DirTree.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DirTree.AutoCollaps = True
        Me.DirTree.CheckBoxes = True
        Me.DirTree.Enabled = False
        Me.DirTree.ExpandMode = StaxRip.UI.TreeNodeExpandMode.Normal
        Me.DirTree.Location = New System.Drawing.Point(12, 13)
        Me.DirTree.Name = "DirTree"
        Me.DirTree.Paths = CType(resources.GetObject("DirTree.Paths"), System.Collections.Generic.List(Of String))
        Me.DirTree.Size = New System.Drawing.Size(653, 341)
        Me.DirTree.TabIndex = 1
        Me.DirTree.Visible = False
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(673, 366)
        Me.bnCancel.Size = New System.Drawing.Size(100, 34)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(567, 366)
        Me.bnOK.Size = New System.Drawing.Size(100, 34)
        Me.bnOK.Text = "OK"
        '
        'flpButtons
        '
        Me.flpButtons.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpButtons.Controls.Add(Me.bnAdd)
        Me.flpButtons.Controls.Add(Me.bnRemove)
        Me.flpButtons.Controls.Add(Me.bnUp)
        Me.flpButtons.Controls.Add(Me.bnDown)
        Me.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpButtons.Location = New System.Drawing.Point(671, 9)
        Me.flpButtons.Name = "flpButtons"
        Me.flpButtons.Size = New System.Drawing.Size(108, 317)
        Me.flpButtons.TabIndex = 4
        '
        'SourceFilesForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(785, 412)
        Me.Controls.Add(Me.flpButtons)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.bnOK)
        Me.Controls.Add(Me.cbCreateJobs)
        Me.Controls.Add(Me.DirTree)
        Me.Controls.Add(Me.lb)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Name = "SourceFilesForm"
        Me.Text = "Source Files"
        Me.flpButtons.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Property Mode As SourceInputMode

    Sub New()
        MyBase.New()
        InitializeComponent()
        bnOK.Enabled = False
    End Sub

    Private Sub lbFiles_SelectedIndexChanged() Handles lb.SelectedIndexChanged
        UpdateControls()
    End Sub

    Sub UpdateControls()
        If Mode = SourceInputMode.DirectoryBatch Then
            DirTree.Enabled = True
            DirTree.Visible = True
            DirTree.BringToFront()
            Text = "Directory Batch"
        ElseIf Mode <> SourceInputMode.DirectoryBatch Then
            If Mode = SourceInputMode.Combine Then
                Text = "Merge"
            ElseIf Mode = SourceInputMode.FileBatch Then
                Text = "File Batch"
            End If

            lb.Enabled = True
            lb.Visible = True
            lb.BringToFront()
        End If

        ActiveControl = bnOK
        bnAdd.Enabled = Mode <> SourceInputMode.DirectoryBatch
        bnRemove.Enabled = lb.SelectedIndex > -1 AndAlso Mode <> SourceInputMode.DirectoryBatch
        bnUp.Enabled = lb.CanMoveUp() AndAlso Mode <> SourceInputMode.DirectoryBatch
        bnDown.Enabled = lb.CanMoveDown() AndAlso Mode <> SourceInputMode.DirectoryBatch
        bnOK.Enabled = lb.Items.Count > 0 OrElse DirTree.Paths.Count > 0
        cbCreateJobs.Visible = Mode <> SourceInputMode.Combine
    End Sub

    Private Sub bAdd_Click() Handles bnAdd.Click
        Using d As New OpenFileDialog
            d.Multiselect = True
            d.SetFilter(FileTypes.Video)
            d.SetInitDir(s.LastSourceDir)

            If d.ShowDialog() = DialogResult.OK Then
                s.LastSourceDir = Filepath.GetDir(d.FileName)
                lb.Items.AddRange(d.FileNames.Sort.ToArray)
                UpdateControls()
            End If
        End Using
    End Sub

    Private Sub bRemove_Click() Handles bnRemove.Click
        lb.RemoveSelection()
        UpdateControls()
    End Sub

    Private Sub bUp_Click() Handles bnUp.Click
        lb.MoveSelectionUp()
        UpdateControls()
    End Sub

    Private Sub bDown_Click() Handles bnDown.Click
        lb.MoveSelectionDown()
        UpdateControls()
    End Sub

    Private Sub SourceFilesForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If DialogResult = DialogResult.OK Then
            If g.ShowVideoSourceWarnings(Files) Then e.Cancel = True

            If Mode = SourceInputMode.FileBatch Then
                For Each i In Files
                    For Each i2 In Files
                        Dim a = Filepath.GetDirAndBase(i).ToUpper
                        Dim b = Filepath.GetDirAndBase(i2).ToUpper

                        If a <> b Then
                            If a.StartsWith(b) Then
                                MsgWarn("Files starting with the names of other files can't be used.", b + CrLf2 + a)
                                e.Cancel = True
                                Exit For
                            End If
                        End If
                    Next
                Next
            End If
        End If
    End Sub

    Private Sub SourceFilesForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        Dim f As New HelpForm()
        f.Doc.WriteStart(Text)
        f.Doc.WriteP("The Source Files dialog allows to open source files in different modes.")
        f.Doc.WriteTips(TipProvider.GetTips)
        f.Show()
    End Sub

    ReadOnly Property Files() As List(Of String)
        Get
            Dim ret As New List(Of String)

            For Each i As String In lb.Items
                ret.Add(i)
            Next

            Return ret
        End Get
    End Property

    Private Sub DirTree_AfterCheck(sender As Object, e As TreeViewEventArgs) Handles DirTree.AfterCheck
        UpdateControls()
    End Sub

    Private Sub DirTree_BeforeCheck(sender As Object, e As TreeViewCancelEventArgs) Handles DirTree.BeforeCheck
        Dim n As FolderTreeNode = DirectCast(e.Node, FolderTreeNode)
        Dim found As Boolean

        Try 'dir access could be denied
            For Each i In Directory.GetFiles(n.Path)
                If FileTypes.Video.Contains(Filepath.GetExt(i)) Then
                    found = True
                End If
            Next
        Catch
        End Try

        If Not found Then
            e.Cancel = True
            MsgInfo("'" + n.Path + "' does not contain any known source files.")
        End If
    End Sub

    Private Sub SourceFilesForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyData = (Keys.Alt Or Keys.M) Then
            cbCreateJobs.Focus()
        End If
    End Sub

    Private Sub lb_DragDrop(sender As Object, e As DragEventArgs) Handles lb.DragDrop
        Drop(e, Nothing)
    End Sub

    Private Sub Drop(e As DragEventArgs, mode As SourceInputMode?)
        Dim a = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not a Is Nothing AndAlso a.Length > 0 Then
            If mode.HasValue Then
                Me.Mode = mode.Value
            End If

            Array.Sort(a)
            lb.Items.AddRange(a)
            UpdateControls()
        End If
    End Sub

    Private Sub bCombine_DragEnter(sender As Object, e As DragEventArgs) Handles lb.DragEnter
        e.Effect = DragDropEffects.Copy
    End Sub

    Private Sub SourceFilesForm_Shown() Handles Me.Shown
        Refresh()
        DirTree.Init()
    End Sub
End Class