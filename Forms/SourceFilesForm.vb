
Imports StaxRip.UI

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
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents pnLB As Panel
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lb = New StaxRip.UI.ListBoxEx()
        Me.bnDown = New StaxRip.UI.ButtonEx()
        Me.bnRemove = New StaxRip.UI.ButtonEx()
        Me.bnUp = New StaxRip.UI.ButtonEx()
        Me.bnAdd = New StaxRip.UI.ButtonEx()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.pnLB = New System.Windows.Forms.Panel()
        Me.tlpMain.SuspendLayout()
        Me.pnLB.SuspendLayout()
        Me.SuspendLayout()
        '
        'lb
        '
        Me.lb.AllowDrop = True
        Me.lb.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lb.DownButton = Me.bnDown
        Me.lb.FormattingEnabled = True
        Me.lb.HorizontalScrollbar = True
        Me.lb.IntegralHeight = False
        Me.lb.ItemHeight = 48
        Me.lb.Location = New System.Drawing.Point(0, 0)
        Me.lb.Name = "lb"
        Me.lb.RemoveButton = Me.bnRemove
        Me.lb.Size = New System.Drawing.Size(717, 600)
        Me.lb.TabIndex = 2
        Me.lb.UpButton = Me.bnUp
        '
        'bnDown
        '
        Me.bnDown.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.bnDown.Location = New System.Drawing.Point(749, 370)
        Me.bnDown.Margin = New System.Windows.Forms.Padding(8)
        Me.bnDown.Size = New System.Drawing.Size(250, 80)
        Me.bnDown.Text = "    &Down"
        '
        'bnRemove
        '
        Me.bnRemove.Location = New System.Drawing.Point(749, 112)
        Me.bnRemove.Margin = New System.Windows.Forms.Padding(8)
        Me.bnRemove.Size = New System.Drawing.Size(250, 80)
        Me.bnRemove.Text = "   &Remove"
        '
        'bnUp
        '
        Me.bnUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnUp.Location = New System.Drawing.Point(749, 274)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(8)
        Me.bnUp.Size = New System.Drawing.Size(250, 80)
        Me.bnUp.Text = "&Up"
        '
        'bnAdd
        '
        Me.bnAdd.Location = New System.Drawing.Point(749, 16)
        Me.bnAdd.Margin = New System.Windows.Forms.Padding(8)
        Me.bnAdd.Size = New System.Drawing.Size(250, 80)
        Me.bnAdd.Text = "&Add..."
        '
        'bnCancel
        '
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(749, 632)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(8)
        Me.bnCancel.Size = New System.Drawing.Size(250, 80)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(483, 632)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(8)
        Me.bnOK.Size = New System.Drawing.Size(250, 80)
        Me.bnOK.Text = "OK"
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 3
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.Controls.Add(Me.bnRemove, 2, 1)
        Me.tlpMain.Controls.Add(Me.bnAdd, 2, 0)
        Me.tlpMain.Controls.Add(Me.bnCancel, 2, 7)
        Me.tlpMain.Controls.Add(Me.bnOK, 1, 7)
        Me.tlpMain.Controls.Add(Me.bnUp, 2, 3)
        Me.tlpMain.Controls.Add(Me.bnDown, 2, 4)
        Me.tlpMain.Controls.Add(Me.pnLB, 0, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.Padding = New System.Windows.Forms.Padding(8)
        Me.tlpMain.RowCount = 8
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1015, 729)
        Me.tlpMain.TabIndex = 7
        '
        'pnLB
        '
        Me.pnLB.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.pnLB, 2)
        Me.pnLB.Controls.Add(Me.lb)
        Me.pnLB.Location = New System.Drawing.Point(16, 16)
        Me.pnLB.Margin = New System.Windows.Forms.Padding(8)
        Me.pnLB.Name = "pnLB"
        Me.tlpMain.SetRowSpan(Me.pnLB, 7)
        Me.pnLB.Size = New System.Drawing.Size(717, 600)
        Me.pnLB.TabIndex = 9
        '
        'SourceFilesForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1015, 729)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.HelpButton = False
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(13, 14, 13, 14)
        Me.Name = "SourceFilesForm"
        Me.Text = "Source Files"
        Me.tlpMain.ResumeLayout(False)
        Me.pnLB.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Property IsMerge As Boolean

    Sub New()
        MyBase.New()
        InitializeComponent()

        ScaleClientSize(36, 22)
        MinimumSize = New Size(Width \ 2, CInt(Height * 0.6))

        bnUp.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnDown.Image = ImageHelp.GetSymbolImage(Symbol.Down)
        bnAdd.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)

        For Each bn In {bnAdd, bnRemove, bnUp, bnDown}
            bn.TextImageRelation = TextImageRelation.Overlay
            bn.ImageAlign = ContentAlignment.MiddleLeft
            Dim pad = bn.Padding
            pad.Left = Control.DefaultFont.Height \ 10
            pad.Right = pad.Left
            bn.Padding = pad
        Next

        ActiveControl = bnOK
    End Sub

    Sub ShowOpenFileDialog()
        Using dialog As New OpenFileDialog
            dialog.Multiselect = True
            dialog.SetFilter(FileTypes.Video)
            dialog.SetInitDir(s.LastSourceDir)

            If dialog.ShowDialog() = DialogResult.OK Then
                s.LastSourceDir = dialog.FileName.Dir
                lb.Items.AddRange(dialog.FileNames.Sort.ToArray)
                lb.SelectedIndex = lb.Items.Count - 1
            End If
        End Using
    End Sub

    Sub bnAdd_Click() Handles bnAdd.Click
        If IsMerge Then
            ShowOpenFileDialog()
            Exit Sub
        End If

        Using td As New TaskDialog(Of String)
            td.AddCommand("Add files", "files")
            td.AddCommand("Add folder", "folder")

            Select Case td.Show
                Case "files"
                    ShowOpenFileDialog()
                Case "folder"
                    Using dialog As New FolderBrowserDialog
                        If dialog.ShowDialog = DialogResult.OK Then
                            Dim subfolders = Directory.GetDirectories(dialog.SelectedPath)
                            Dim opt = SearchOption.TopDirectoryOnly

                            If Directory.GetDirectories(dialog.SelectedPath).Count > 0 Then
                                If MsgQuestion("Include sub folders?", TaskDialogButtons.YesNo) = DialogResult.Yes Then
                                    opt = SearchOption.AllDirectories
                                End If
                            End If

                            lb.Items.AddRange(Directory.GetFiles(dialog.SelectedPath, "*.*", opt).Where(Function(val) FileTypes.Video.Contains(val.Ext)).ToArray)
                            lb.SelectedIndex = lb.Items.Count - 1
                        End If
                    End Using
            End Select
        End Using
    End Sub

    Protected Overrides Sub OnFormClosing(args As FormClosingEventArgs)
        MyBase.OnFormClosing(args)

        If DialogResult = DialogResult.OK Then
            Dim files = GetFiles()

            If Not g.VerifySource(GetFiles) Then
                args.Cancel = True
            End If
        End If
    End Sub

    Function GetFiles() As IEnumerable(Of String)
        Return lb.Items.OfType(Of String)
    End Function

    Sub lb_DragDrop(sender As Object, e As DragEventArgs) Handles lb.DragDrop
        Dim items = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not items.NothingOrEmpty Then
            Array.Sort(items)
            lb.Items.AddRange(items.Where(Function(val) File.Exists(val)).ToArray)
        End If
    End Sub

    Sub lb_DragEnter(sender As Object, e As DragEventArgs) Handles lb.DragEnter
        e.Effect = DragDropEffects.Copy
    End Sub
End Class
