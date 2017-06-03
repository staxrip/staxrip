Imports StaxRip.UI

Class DemuxForm
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
    Friend WithEvents tlpMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tbDescription As System.Windows.Forms.TextBox
    Friend WithEvents bnBrowse As ButtonEx
    Friend WithEvents bnArguments As ButtonEx
    Friend WithEvents cmsArguments As ContextMenuStripEx
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
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.bnArguments = New StaxRip.UI.ButtonEx()
        Me.cmsArguments = New StaxRip.UI.ContextMenuStripEx(Me.components)
        Me.MacrosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.bnBrowse = New StaxRip.UI.ButtonEx()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.tbDescription = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.tlpMain.SuspendLayout()
        Me.cmsArguments.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbName
        '
        Me.tbName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.tbName, 3)
        Me.tbName.Location = New System.Drawing.Point(295, 12)
        Me.tbName.Margin = New System.Windows.Forms.Padding(0)
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(1439, 55)
        Me.tbName.TabIndex = 1
        '
        'tbInput
        '
        Me.tbInput.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.tbInput, 3)
        Me.tbInput.Location = New System.Drawing.Point(295, 92)
        Me.tbInput.Margin = New System.Windows.Forms.Padding(0)
        Me.tbInput.Name = "tbInput"
        Me.tbInput.Size = New System.Drawing.Size(1439, 55)
        Me.tbInput.TabIndex = 13
        '
        'tbInputFormats
        '
        Me.tbInputFormats.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.tbInputFormats, 3)
        Me.tbInputFormats.Location = New System.Drawing.Point(295, 252)
        Me.tbInputFormats.Margin = New System.Windows.Forms.Padding(0)
        Me.tbInputFormats.Name = "tbInputFormats"
        Me.tbInputFormats.Size = New System.Drawing.Size(1439, 55)
        Me.tbInputFormats.TabIndex = 15
        '
        'tbVideoOut
        '
        Me.tbVideoOut.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.tbVideoOut, 3)
        Me.tbVideoOut.Location = New System.Drawing.Point(295, 172)
        Me.tbVideoOut.Margin = New System.Windows.Forms.Padding(0)
        Me.tbVideoOut.Name = "tbVideoOut"
        Me.tbVideoOut.Size = New System.Drawing.Size(1439, 55)
        Me.tbVideoOut.TabIndex = 3
        '
        'tbCommand
        '
        Me.tbCommand.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.tbCommand, 2)
        Me.tbCommand.Location = New System.Drawing.Point(295, 412)
        Me.tbCommand.Margin = New System.Windows.Forms.Padding(0)
        Me.tbCommand.Name = "tbCommand"
        Me.tbCommand.Size = New System.Drawing.Size(1174, 55)
        Me.tbCommand.TabIndex = 7
        '
        'tbArguments
        '
        Me.tbArguments.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.tbArguments, 2)
        Me.tbArguments.Location = New System.Drawing.Point(295, 492)
        Me.tbArguments.Margin = New System.Windows.Forms.Padding(0)
        Me.tbArguments.Name = "tbArguments"
        Me.tbArguments.Size = New System.Drawing.Size(1174, 55)
        Me.tbArguments.TabIndex = 9
        '
        'tbSourceFilters
        '
        Me.tbSourceFilters.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.tbSourceFilters, 3)
        Me.tbSourceFilters.Location = New System.Drawing.Point(295, 332)
        Me.tbSourceFilters.Margin = New System.Windows.Forms.Padding(0)
        Me.tbSourceFilters.Name = "tbSourceFilters"
        Me.tbSourceFilters.Size = New System.Drawing.Size(1439, 55)
        Me.tbSourceFilters.TabIndex = 19
        '
        'bnCancel
        '
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(1484, 847)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(15, 15, 0, 0)
        Me.bnCancel.Size = New System.Drawing.Size(250, 70)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(1219, 847)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(0, 15, 0, 0)
        Me.bnOK.Size = New System.Drawing.Size(250, 70)
        Me.bnOK.Text = "OK"
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 4
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.Controls.Add(Me.bnArguments, 3, 6)
        Me.tlpMain.Controls.Add(Me.bnBrowse, 3, 5)
        Me.tlpMain.Controls.Add(Me.tbArguments, 1, 6)
        Me.tlpMain.Controls.Add(Me.tbCommand, 1, 5)
        Me.tlpMain.Controls.Add(Me.Label6, 0, 5)
        Me.tlpMain.Controls.Add(Me.tbInput, 1, 1)
        Me.tlpMain.Controls.Add(Me.bnOK, 2, 9)
        Me.tlpMain.Controls.Add(Me.bnCancel, 3, 9)
        Me.tlpMain.Controls.Add(Me.Label2, 0, 0)
        Me.tlpMain.Controls.Add(Me.Label10, 0, 7)
        Me.tlpMain.Controls.Add(Me.tbDescription, 0, 8)
        Me.tlpMain.Controls.Add(Me.tbName, 1, 0)
        Me.tlpMain.Controls.Add(Me.Label3, 0, 1)
        Me.tlpMain.Controls.Add(Me.Label8, 0, 2)
        Me.tlpMain.Controls.Add(Me.tbVideoOut, 1, 2)
        Me.tlpMain.Controls.Add(Me.tbSourceFilters, 1, 4)
        Me.tlpMain.Controls.Add(Me.Label9, 0, 4)
        Me.tlpMain.Controls.Add(Me.tbInputFormats, 1, 3)
        Me.tlpMain.Controls.Add(Me.Label1, 0, 3)
        Me.tlpMain.Controls.Add(Me.Label7, 0, 6)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(15, 15)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 10
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpMain.Size = New System.Drawing.Size(1734, 917)
        Me.tlpMain.TabIndex = 21
        '
        'bnArguments
        '
        Me.bnArguments.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnArguments.ContextMenuStrip = Me.cmsArguments
        Me.bnArguments.Location = New System.Drawing.Point(1484, 485)
        Me.bnArguments.Margin = New System.Windows.Forms.Padding(15, 0, 0, 0)
        Me.bnArguments.ShowMenuSymbol = True
        Me.bnArguments.Size = New System.Drawing.Size(250, 70)
        Me.bnArguments.Text = "Menu"
        '
        'cmsArguments
        '
        Me.cmsArguments.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.cmsArguments.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MacrosToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.cmsArguments.Name = "cmsArguments"
        Me.cmsArguments.Size = New System.Drawing.Size(217, 108)
        '
        'MacrosToolStripMenuItem
        '
        Me.MacrosToolStripMenuItem.Name = "MacrosToolStripMenuItem"
        Me.MacrosToolStripMenuItem.Size = New System.Drawing.Size(216, 52)
        Me.MacrosToolStripMenuItem.Text = "Macros"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(216, 52)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'bnBrowse
        '
        Me.bnBrowse.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnBrowse.Location = New System.Drawing.Point(1484, 405)
        Me.bnBrowse.Margin = New System.Windows.Forms.Padding(15, 0, 0, 0)
        Me.bnBrowse.Size = New System.Drawing.Size(250, 70)
        Me.bnBrowse.Text = "Browse..."
        '
        'Label6
        '
        Me.Label6.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 416)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(192, 48)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Command:"
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(277, 48)
        Me.Label2.TabIndex = 24
        Me.Label2.Text = "Demuxer Name:"
        Me.TipProvider.SetTipText(Me.Label2, "If the demuxer name matches the name of a bundled app then help and description i" &
        "s provided.")
        '
        'Label10
        '
        Me.Label10.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(3, 576)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(209, 48)
        Me.Label10.TabIndex = 32
        Me.Label10.Text = "Description:"
        '
        'tbDescription
        '
        Me.tbDescription.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.tbDescription, 4)
        Me.tbDescription.Location = New System.Drawing.Point(0, 640)
        Me.tbDescription.Margin = New System.Windows.Forms.Padding(0)
        Me.tbDescription.Multiline = True
        Me.tbDescription.Name = "tbDescription"
        Me.tbDescription.ReadOnly = True
        Me.tbDescription.Size = New System.Drawing.Size(1734, 192)
        Me.tbDescription.TabIndex = 33
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(275, 48)
        Me.Label3.TabIndex = 25
        Me.Label3.Text = "Input File Types:"
        Me.TipProvider.SetTipText(Me.Label3, "Input file type to be handled by the demuxer. Use comma to separate multiple file" &
        " types.")
        '
        'Label8
        '
        Me.Label8.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(3, 176)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(289, 48)
        Me.Label8.TabIndex = 30
        Me.Label8.Text = "Output File Type:"
        Me.TipProvider.SetTipText(Me.Label8, "Single video file type the demuxer outputs.")
        '
        'Label9
        '
        Me.Label9.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(3, 336)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(242, 48)
        Me.Label9.TabIndex = 31
        Me.Label9.Text = "Source Filters:"
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 256)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(249, 48)
        Me.Label1.TabIndex = 26
        Me.Label1.Text = "Input Formats:"
        Me.TipProvider.SetTipText(Me.Label1, "Formats the demuxer handles. Format as shown by MediaInfo. For simplification Sta" &
        "xRip uses lower case and the aliases vc1 and mpeg2 so common formats are avc, he" &
        "vc, mpeg2 and vc1.")
        '
        'Label7
        '
        Me.Label7.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 496)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(202, 48)
        Me.Label7.TabIndex = 29
        Me.Label7.Text = "Arguments:"
        '
        'DemuxForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1764, 947)
        Me.Controls.Add(Me.tlpMain)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(11, 10, 11, 10)
        Me.Name = "DemuxForm"
        Me.Padding = New System.Windows.Forms.Padding(15)
        Me.Text = "Demux Configuration"
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.cmsArguments.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Target As CommandLineDemuxer
    Private Temp As CommandLineDemuxer

    Sub New(demuxer As CommandLineDemuxer)
        MyBase.New()
        InitializeComponent()
        ScaleClientSize(40, 22)

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

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        If DialogResult = DialogResult.OK Then
            Target.Name = tbName.Text
            Target.InputExtensions = tbInput.Text.ToLower.SplitNoEmptyAndWhiteSpace(",", ";")
            Target.InputFormats = ConvertFormat(tbInputFormats.Text).SplitNoEmptyAndWhiteSpace(",", ";")
            Target.OutputExtensions = tbVideoOut.Text.ToLower.SplitNoEmptyAndWhiteSpace(",", ";")
            Target.SourceFilter = tbSourceFilters.Text.Trim
            Target.Command = tbCommand.Text
            Target.Arguments = tbArguments.Text
        End If

        MyBase.OnFormClosed(e)
    End Sub

    Protected Overrides Sub OnHelpRequested(hevent As HelpEventArgs)
        Dim f As New HelpForm()
        f.Doc.WriteStart(Text)
        f.Doc.WriteTips(TipProvider.GetTips)
        f.Show()

        MyBase.OnHelpRequested(hevent)
    End Sub

    Private Sub tbCommand_DoubleClick(sender As Object, e As EventArgs) Handles tbCommand.DoubleClick
        Using d As New OpenFileDialog
            If d.ShowDialog() = DialogResult.OK Then tbCommand.Text = d.FileName
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
        For Each i In Package.Items.Values
            If tbName.Text = i.Name Then
                If i.GetHelpPath <> "" Then
                    g.ShellExecute(i.GetHelpPath)
                Else
                    MsgWarn("There is no help available for this app.")
                End If

                Exit Sub
            End If
        Next

        MsgWarn("The demuxer name '" + tbName.Text + "' does not match with the name of one of StaxRip's apps. StaxRip includes the following apps:" + BR2 +
                Package.Items.Values.Where(Function(package) Not TypeOf package Is PluginPackage).Select(Function(package) package.Name).ToArray.Sort.Join(", "))
    End Sub
End Class