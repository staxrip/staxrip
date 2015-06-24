Imports StaxRip.UI

Public Class DemuxForm
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

    Friend WithEvents CommandLink1 As StaxRip.UI.CommandLink
    Friend WithEvents tbName As System.Windows.Forms.TextBox
    Friend WithEvents tbInput As System.Windows.Forms.TextBox
    Friend WithEvents tbInputFormats As System.Windows.Forms.TextBox
    Friend WithEvents tbVideoOut As System.Windows.Forms.TextBox
    Friend WithEvents tbCommand As System.Windows.Forms.TextBox
    Friend WithEvents tbArguments As System.Windows.Forms.TextBox
    Friend WithEvents tbSourceFilters As System.Windows.Forms.TextBox
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents tlp As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tbDescription As System.Windows.Forms.TextBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents bnBrowse As ButtonEx
    Friend WithEvents bnArguments As ButtonEx
    Friend WithEvents cmsArguments As ContextMenuStrip
    Friend WithEvents MacrosToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TipProvider As TipProvider
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.tbName = New System.Windows.Forms.TextBox()
        Me.tbInput = New System.Windows.Forms.TextBox()
        Me.tbInputFormats = New System.Windows.Forms.TextBox()
        Me.tbVideoOut = New System.Windows.Forms.TextBox()
        Me.tbCommand = New System.Windows.Forms.TextBox()
        Me.tbArguments = New System.Windows.Forms.TextBox()
        Me.tbSourceFilters = New System.Windows.Forms.TextBox()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tlp = New System.Windows.Forms.TableLayoutPanel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.tbDescription = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.bnBrowse = New StaxRip.UI.ButtonEx()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.bnArguments = New StaxRip.UI.ButtonEx()
        Me.cmsArguments = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MacrosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.tlp.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.cmsArguments.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbName
        '
        Me.tbName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbName.Location = New System.Drawing.Point(155, 3)
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(902, 31)
        Me.tbName.TabIndex = 1
        '
        'tbInput
        '
        Me.tbInput.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbInput.Location = New System.Drawing.Point(155, 40)
        Me.tbInput.Name = "tbInput"
        Me.tbInput.Size = New System.Drawing.Size(902, 31)
        Me.tbInput.TabIndex = 13
        '
        'tbInputFormats
        '
        Me.tbInputFormats.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbInputFormats.Location = New System.Drawing.Point(155, 114)
        Me.tbInputFormats.Name = "tbInputFormats"
        Me.tbInputFormats.Size = New System.Drawing.Size(902, 31)
        Me.tbInputFormats.TabIndex = 15
        '
        'tbVideoOut
        '
        Me.tbVideoOut.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbVideoOut.Location = New System.Drawing.Point(155, 77)
        Me.tbVideoOut.Name = "tbVideoOut"
        Me.tbVideoOut.Size = New System.Drawing.Size(902, 31)
        Me.tbVideoOut.TabIndex = 3
        '
        'tbCommand
        '
        Me.tbCommand.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbCommand.Location = New System.Drawing.Point(3, 4)
        Me.tbCommand.Name = "tbCommand"
        Me.tbCommand.Size = New System.Drawing.Size(796, 31)
        Me.tbCommand.TabIndex = 7
        '
        'tbArguments
        '
        Me.tbArguments.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbArguments.Location = New System.Drawing.Point(3, 4)
        Me.tbArguments.Multiline = True
        Me.tbArguments.Name = "tbArguments"
        Me.tbArguments.Size = New System.Drawing.Size(796, 31)
        Me.tbArguments.TabIndex = 9
        '
        'tbSourceFilters
        '
        Me.tbSourceFilters.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSourceFilters.Location = New System.Drawing.Point(155, 151)
        Me.tbSourceFilters.Name = "tbSourceFilters"
        Me.tbSourceFilters.Size = New System.Drawing.Size(902, 31)
        Me.tbSourceFilters.TabIndex = 19
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(968, 493)
        Me.bnCancel.Size = New System.Drawing.Size(100, 36)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(862, 493)
        Me.bnOK.Size = New System.Drawing.Size(100, 36)
        Me.bnOK.Text = "OK"
        '
        'tlp
        '
        Me.tlp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlp.ColumnCount = 2
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlp.Controls.Add(Me.Label6, 0, 7)
        Me.tlp.Controls.Add(Me.tbInput, 1, 1)
        Me.tlp.Controls.Add(Me.Label2, 0, 0)
        Me.tlp.Controls.Add(Me.Label7, 0, 9)
        Me.tlp.Controls.Add(Me.Label10, 0, 11)
        Me.tlp.Controls.Add(Me.tbDescription, 0, 12)
        Me.tlp.Controls.Add(Me.tbName, 1, 0)
        Me.tlp.Controls.Add(Me.Label3, 0, 1)
        Me.tlp.Controls.Add(Me.Label8, 0, 2)
        Me.tlp.Controls.Add(Me.tbVideoOut, 1, 2)
        Me.tlp.Controls.Add(Me.tbSourceFilters, 1, 4)
        Me.tlp.Controls.Add(Me.Label9, 0, 4)
        Me.tlp.Controls.Add(Me.TableLayoutPanel1, 1, 7)
        Me.tlp.Controls.Add(Me.TableLayoutPanel2, 1, 9)
        Me.tlp.Controls.Add(Me.tbInputFormats, 1, 3)
        Me.tlp.Controls.Add(Me.Label1, 0, 3)
        Me.tlp.Location = New System.Drawing.Point(9, 2)
        Me.tlp.Margin = New System.Windows.Forms.Padding(0)
        Me.tlp.Name = "tlp"
        Me.tlp.RowCount = 13
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlp.Size = New System.Drawing.Size(1060, 488)
        Me.tlp.TabIndex = 21
        '
        'Label6
        '
        Me.Label6.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 192)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 25)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Command:"
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(139, 25)
        Me.Label2.TabIndex = 24
        Me.Label2.Text = "Demuxer Name:"
        Me.TipProvider.SetTipText(Me.Label2, "If the demuxer name matches the name of a bundled app then help and description i" &
        "s provided.")
        '
        'Label7
        '
        Me.Label7.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 231)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(104, 25)
        Me.Label7.TabIndex = 29
        Me.Label7.Text = "Arguments:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(3, 263)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(106, 25)
        Me.Label10.TabIndex = 32
        Me.Label10.Text = "Description:"
        '
        'tbDescription
        '
        Me.tbDescription.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlp.SetColumnSpan(Me.tbDescription, 2)
        Me.tbDescription.Location = New System.Drawing.Point(3, 291)
        Me.tbDescription.Multiline = True
        Me.tbDescription.Name = "tbDescription"
        Me.tbDescription.ReadOnly = True
        Me.tbDescription.Size = New System.Drawing.Size(1054, 194)
        Me.tbDescription.TabIndex = 33
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 43)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(139, 25)
        Me.Label3.TabIndex = 25
        Me.Label3.Text = "Input File Types:"
        Me.TipProvider.SetTipText(Me.Label3, "Input file type to be handled by the demuxer. Use comma to separate multiple file" &
        " types.")
        '
        'Label8
        '
        Me.Label8.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(3, 80)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(146, 25)
        Me.Label8.TabIndex = 30
        Me.Label8.Text = "Output File Type:"
        Me.TipProvider.SetTipText(Me.Label8, "Single video file type the demuxer outputs.")
        '
        'Label9
        '
        Me.Label9.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(3, 154)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(121, 25)
        Me.Label9.TabIndex = 31
        Me.Label9.Text = "Source Filters:"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.Controls.Add(Me.tbCommand, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.bnBrowse, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(152, 185)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(908, 39)
        Me.TableLayoutPanel1.TabIndex = 35
        '
        'bnBrowse
        '
        Me.bnBrowse.Location = New System.Drawing.Point(805, 3)
        Me.bnBrowse.Size = New System.Drawing.Size(100, 33)
        Me.bnBrowse.Text = "Browse..."
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.AutoSize = True
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel2.Controls.Add(Me.bnArguments, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.tbArguments, 0, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(152, 224)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(908, 39)
        Me.TableLayoutPanel2.TabIndex = 36
        '
        'bnArguments
        '
        Me.bnArguments.ContextMenuStrip = Me.cmsArguments
        Me.bnArguments.Location = New System.Drawing.Point(805, 3)
        Me.bnArguments.ShowMenuSymbol = True
        Me.bnArguments.Size = New System.Drawing.Size(100, 33)
        Me.bnArguments.Text = "Menu"
        '
        'cmsArguments
        '
        Me.cmsArguments.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.cmsArguments.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MacrosToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.cmsArguments.Name = "cmsArguments"
        Me.cmsArguments.Size = New System.Drawing.Size(143, 64)
        '
        'MacrosToolStripMenuItem
        '
        Me.MacrosToolStripMenuItem.Name = "MacrosToolStripMenuItem"
        Me.MacrosToolStripMenuItem.Size = New System.Drawing.Size(142, 30)
        Me.MacrosToolStripMenuItem.Text = "Macros"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(142, 30)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 117)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(128, 25)
        Me.Label1.TabIndex = 26
        Me.Label1.Text = "Input Formats:"
        Me.TipProvider.SetTipText(Me.Label1, "Formats the demuxer handles. Format as shown by MediaInfo. For simplification Sta" &
        "xRip uses lower case and the aliases vc1 and mpeg2 so common formats are avc, he" &
        "vc, mpeg2 and vc1.")
        '
        'DemuxingForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1078, 539)
        Me.Controls.Add(Me.tlp)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.bnOK)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Name = "DemuxingForm"
        Me.Text = "Demux Configuration"
        Me.tlp.ResumeLayout(False)
        Me.tlp.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.cmsArguments.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Target As CommandLineDemuxer
    Private Temp As CommandLineDemuxer

    Sub New(demuxer As CommandLineDemuxer)
        MyBase.New()
        InitializeComponent()

        Target = demuxer
        Temp = ObjectHelp.GetCopy(Of CommandLineDemuxer)(demuxer)

        tbName.Text = Temp.Name
        tbInput.Text = Temp.InputExtensions.ToArray.Join(", ")
        tbInputFormats.Text = Temp.InputFormats.ToArray.Join(", ")
        tbSourceFilters.Text = Temp.SourceFilter
        tbVideoOut.Text = Temp.OutputExtensions.ToArray.Join(", ")
        tbArguments.Text = Temp.Arguments
        tbCommand.Text = Temp.Command
        tbDescription.Text = Temp.GetHelp

        ActiveControl = bnOK
    End Sub

    Function ConvertFormat(input As String) As String
        If input.Contains("MPEG Video") Then input = input.Replace("MPEG Video", "mpeg2")
        If input.Contains("VC-1") Then input = input.Replace("VC-1", "vc1")
        Return input.ToLower
    End Function

    Private Sub DemuxForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If DialogResult = DialogResult.OK Then
            Target.Name = tbName.Text
            Target.InputExtensions = tbInput.Text.ToLower.SplitNoEmptyAndWhiteSpace(",", ";")
            Target.InputFormats = ConvertFormat(tbInputFormats.Text).SplitNoEmptyAndWhiteSpace(",", ";")
            Target.OutputExtensions = tbVideoOut.Text.ToLower.SplitNoEmptyAndWhiteSpace(",", ";")
            Target.SourceFilter = tbSourceFilters.Text.Trim
            Target.Command = tbCommand.Text
            Target.Arguments = tbArguments.Text
        End If
    End Sub

    Private Sub DemuxingForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        Dim f As New HelpForm()
        f.Doc.WriteStart(Text)
        f.Doc.WriteTips(TipProvider.GetTips)
        f.Show()
    End Sub

    Private Sub tbCommand_DoubleClick(sender As Object, e As EventArgs) Handles tbCommand.DoubleClick
        Using d As New OpenFileDialog
            If d.ShowDialog() = DialogResult.OK Then
                tbCommand.Text = d.FileName
            End If
        End Using
    End Sub

    Private Sub tbArguments_DoubleClick(sender As Object, e As EventArgs) Handles tbArguments.DoubleClick
        MacrosForm.ShowDialogForm()
    End Sub

    Private Sub bnBrowse_Click(sender As Object, e As EventArgs) Handles bnBrowse.Click
        tbCommand_DoubleClick(Nothing, Nothing)
    End Sub

    Private Sub MacrosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MacrosToolStripMenuItem.Click
        MacrosForm.ShowDialogForm()
    End Sub

    Private Sub HelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.Click
        For Each i In Packs.Packages
            If tbName.Text = i.Name Then
                If i.GetHelpPath <> "" Then
                    g.ShellExecute(i.GetHelpPath)
                Else
                    MsgWarn("There is no help available for this app.")
                End If

                Exit Sub
            End If
        Next

        MsgWarn("The demuxer name '" + tbName.Text + "' does not match with the name of one of StaxRip's apps. StaxRip includes the following apps:" + CrLf2 +
                Packs.Packages.Where(Function(package) Not TypeOf package Is PluginPackage).Select(Function(package) package.Name).ToArray.Sort.Join(", "))
    End Sub
End Class