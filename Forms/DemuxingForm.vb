Imports StaxRip.UI

Public Class DemuxingForm
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
    Friend WithEvents tbInputBlacklist As System.Windows.Forms.TextBox
    Friend WithEvents tbVideoOut As System.Windows.Forms.TextBox
    Friend WithEvents tbCommand As System.Windows.Forms.TextBox
    Friend WithEvents tbArguments As System.Windows.Forms.TextBox
    Friend WithEvents llBrowse As System.Windows.Forms.LinkLabel
    Friend WithEvents llMacros As System.Windows.Forms.LinkLabel
    Friend WithEvents tbSourceFilters As System.Windows.Forms.TextBox
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents tlp As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tbDescription As System.Windows.Forms.TextBox

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.tbName = New System.Windows.Forms.TextBox()
        Me.tbInput = New System.Windows.Forms.TextBox()
        Me.tbInputFormats = New System.Windows.Forms.TextBox()
        Me.tbInputBlacklist = New System.Windows.Forms.TextBox()
        Me.tbVideoOut = New System.Windows.Forms.TextBox()
        Me.tbCommand = New System.Windows.Forms.TextBox()
        Me.tbArguments = New System.Windows.Forms.TextBox()
        Me.llBrowse = New System.Windows.Forms.LinkLabel()
        Me.llMacros = New System.Windows.Forms.LinkLabel()
        Me.tbSourceFilters = New System.Windows.Forms.TextBox()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tlp = New System.Windows.Forms.TableLayoutPanel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.tbDescription = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.tlp.SuspendLayout()
        Me.SuspendLayout()
        '
        'tbName
        '
        Me.tbName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbName.Location = New System.Drawing.Point(3, 28)
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(524, 31)
        Me.tbName.TabIndex = 1
        '
        'tbInput
        '
        Me.tbInput.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbInput.Location = New System.Drawing.Point(533, 28)
        Me.tbInput.Name = "tbInput"
        Me.tbInput.Size = New System.Drawing.Size(524, 31)
        Me.tbInput.TabIndex = 13
        '
        'tbInputFormats
        '
        Me.tbInputFormats.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbInputFormats.Location = New System.Drawing.Point(533, 105)
        Me.tbInputFormats.Name = "tbInputFormats"
        Me.tbInputFormats.Size = New System.Drawing.Size(524, 31)
        Me.tbInputFormats.TabIndex = 15
        '
        'tbInputBlacklist
        '
        Me.tbInputBlacklist.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbInputBlacklist.Location = New System.Drawing.Point(533, 182)
        Me.tbInputBlacklist.Name = "tbInputBlacklist"
        Me.tbInputBlacklist.Size = New System.Drawing.Size(524, 31)
        Me.tbInputBlacklist.TabIndex = 17
        '
        'tbVideoOut
        '
        Me.tbVideoOut.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbVideoOut.Location = New System.Drawing.Point(3, 105)
        Me.tbVideoOut.Name = "tbVideoOut"
        Me.tbVideoOut.Size = New System.Drawing.Size(524, 31)
        Me.tbVideoOut.TabIndex = 3
        '
        'tbCommand
        '
        Me.tbCommand.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlp.SetColumnSpan(Me.tbCommand, 2)
        Me.tbCommand.Location = New System.Drawing.Point(3, 259)
        Me.tbCommand.Name = "tbCommand"
        Me.tbCommand.Size = New System.Drawing.Size(1054, 31)
        Me.tbCommand.TabIndex = 7
        '
        'tbArguments
        '
        Me.tbArguments.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlp.SetColumnSpan(Me.tbArguments, 2)
        Me.tbArguments.Location = New System.Drawing.Point(3, 336)
        Me.tbArguments.Multiline = True
        Me.tbArguments.Name = "tbArguments"
        Me.tbArguments.Size = New System.Drawing.Size(1054, 62)
        Me.tbArguments.TabIndex = 9
        '
        'llBrowse
        '
        Me.llBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llBrowse.AutoSize = True
        Me.llBrowse.Location = New System.Drawing.Point(988, 231)
        Me.llBrowse.Name = "llBrowse"
        Me.llBrowse.Size = New System.Drawing.Size(69, 25)
        Me.llBrowse.TabIndex = 11
        Me.llBrowse.TabStop = True
        Me.llBrowse.Text = "Browse"
        '
        'llMacros
        '
        Me.llMacros.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llMacros.AutoSize = True
        Me.llMacros.Location = New System.Drawing.Point(987, 308)
        Me.llMacros.Name = "llMacros"
        Me.llMacros.Size = New System.Drawing.Size(70, 25)
        Me.llMacros.TabIndex = 20
        Me.llMacros.TabStop = True
        Me.llMacros.Text = "Macros"
        '
        'tbSourceFilters
        '
        Me.tbSourceFilters.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSourceFilters.Location = New System.Drawing.Point(3, 182)
        Me.tbSourceFilters.Name = "tbSourceFilters"
        Me.tbSourceFilters.Size = New System.Drawing.Size(524, 31)
        Me.tbSourceFilters.TabIndex = 19
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(968, 541)
        Me.bnCancel.Size = New System.Drawing.Size(100, 36)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(862, 541)
        Me.bnOK.Size = New System.Drawing.Size(100, 36)
        Me.bnOK.Text = "OK"
        '
        'tlp
        '
        Me.tlp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlp.ColumnCount = 2
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlp.Controls.Add(Me.Label6, 0, 6)
        Me.tlp.Controls.Add(Me.llBrowse, 1, 6)
        Me.tlp.Controls.Add(Me.tbName, 0, 1)
        Me.tlp.Controls.Add(Me.tbArguments, 0, 9)
        Me.tlp.Controls.Add(Me.llMacros, 1, 8)
        Me.tlp.Controls.Add(Me.tbInput, 1, 1)
        Me.tlp.Controls.Add(Me.tbCommand, 0, 7)
        Me.tlp.Controls.Add(Me.tbVideoOut, 0, 3)
        Me.tlp.Controls.Add(Me.tbInputFormats, 1, 3)
        Me.tlp.Controls.Add(Me.tbInputBlacklist, 1, 5)
        Me.tlp.Controls.Add(Me.Label2, 0, 0)
        Me.tlp.Controls.Add(Me.Label3, 1, 0)
        Me.tlp.Controls.Add(Me.Label1, 1, 2)
        Me.tlp.Controls.Add(Me.Label4, 1, 4)
        Me.tlp.Controls.Add(Me.Label7, 0, 8)
        Me.tlp.Controls.Add(Me.Label8, 0, 2)
        Me.tlp.Controls.Add(Me.Label10, 0, 10)
        Me.tlp.Controls.Add(Me.tbDescription, 0, 11)
        Me.tlp.Controls.Add(Me.Label9, 0, 4)
        Me.tlp.Controls.Add(Me.tbSourceFilters, 0, 5)
        Me.tlp.Location = New System.Drawing.Point(9, 2)
        Me.tlp.Margin = New System.Windows.Forms.Padding(0)
        Me.tlp.Name = "tlp"
        Me.tlp.RowCount = 12
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
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlp.Size = New System.Drawing.Size(1060, 536)
        Me.tlp.TabIndex = 21
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 231)
        Me.Label6.Margin = New System.Windows.Forms.Padding(3, 15, 3, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(477, 25)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Full executable path, may contain macros:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(189, 25)
        Me.Label2.TabIndex = 24
        Me.Label2.Text = "Name of the demuxer:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(533, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(376, 25)
        Me.Label3.TabIndex = 25
        Me.Label3.Text = "Video input file types the demuxer will handle:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(533, 77)
        Me.Label1.Margin = New System.Windows.Forms.Padding(3, 15, 3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(453, 25)
        Me.Label1.TabIndex = 26
        Me.Label1.Text = "Video input formats to be handled (MediaInfo: Format):"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(533, 154)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(494, 25)
        Me.Label4.TabIndex = 27
        Me.Label4.Text = "Video input formats NOT to be handled (MediaInfo: Format):"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 308)
        Me.Label7.Margin = New System.Windows.Forms.Padding(3, 15, 3, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(273, 25)
        Me.Label7.TabIndex = 29
        Me.Label7.Text = "Arguments, may contain macros:"
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(3, 77)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(370, 25)
        Me.Label8.TabIndex = 30
        Me.Label8.Text = "Video file types outputted by the application:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(3, 416)
        Me.Label10.Margin = New System.Windows.Forms.Padding(3, 15, 3, 0)
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
        Me.tbDescription.Location = New System.Drawing.Point(3, 444)
        Me.tbDescription.Multiline = True
        Me.tbDescription.Name = "tbDescription"
        Me.tbDescription.Size = New System.Drawing.Size(1054, 89)
        Me.tbDescription.TabIndex = 33
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(3, 154)
        Me.Label9.Margin = New System.Windows.Forms.Padding(3, 15, 3, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(343, 25)
        Me.Label9.TabIndex = 31
        Me.Label9.Text = "Run only if defined source filters are used:"
        '
        'DemuxingForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1078, 587)
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
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Target As CommandLineDemuxer
    Private Temp As CommandLineDemuxer
    Private HelpList As New StringPairList

    Sub New(demuxer As CommandLineDemuxer)
        MyBase.New()
        InitializeComponent()

        Target = demuxer
        Temp = ObjectHelp.GetCopy(Of CommandLineDemuxer)(demuxer)

        tbName.Text = Temp.Name
        tbInput.Text = Temp.InputExtensions.ToArray.Join(", ")
        tbInputFormats.Text = Temp.InputFormats.ToArray.Join(", ")
        tbInputBlacklist.Text = Temp.InputFormatsBlacklist.ToArray.Join(", ")
        tbSourceFilters.Text = Temp.SourceFilters.ToArray.Join(", ")
        tbVideoOut.Text = Temp.OutputExtensions.ToArray.Join(", ")
        tbArguments.Text = Temp.Arguments
        tbCommand.Text = Temp.Command
        tbDescription.Text = Temp.Description

        ActiveControl = bnOK
    End Sub

    Private Sub DemuxForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If DialogResult = DialogResult.OK Then
            Target.Name = tbName.Text
            Target.InputExtensions = tbInput.Text.ToLower.SplitNoEmptyAndWhiteSpace(",", ";")
            Target.InputFormats = tbInputFormats.Text.SplitNoEmptyAndWhiteSpace(",", ";")
            Target.OutputExtensions = tbVideoOut.Text.ToLower.SplitNoEmptyAndWhiteSpace(",", ";")
            Target.InputFormatsBlacklist = tbInputBlacklist.Text.SplitNoEmptyAndWhiteSpace(",", ";")
            Target.SourceFilters = tbSourceFilters.Text.SplitNoEmptyAndWhiteSpace(",", ";")
            Target.Command = tbCommand.Text
            Target.Arguments = tbArguments.Text
            Target.Description = tbDescription.Text
        End If
    End Sub

    Sub AssignHelp(ll As LinkLabel, msg As String)
        ll.AddClickAction(Sub() MsgInfo(msg))
        HelpList.Add(ll.Text, msg)
    End Sub

    Private Sub DemuxingForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        Dim f As New HelpForm()
        f.Doc.WriteStart(Text)
        f.Doc.WriteTips(HelpList)
        f.Show()
    End Sub

    Private Sub tbCommand_DoubleClick(sender As Object, e As EventArgs) Handles tbCommand.DoubleClick
        Using d As New OpenFileDialog
            If d.ShowDialog() = DialogResult.OK Then
                tbCommand.Text = d.FileName
            End If
        End Using
    End Sub

    Private Sub llBrowse_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llBrowse.LinkClicked
        tbCommand_DoubleClick(Nothing, Nothing)
    End Sub

    Private Sub tbArguments_DoubleClick(sender As Object, e As EventArgs) Handles tbArguments.DoubleClick
        MacrosForm.ShowDialogForm()
    End Sub

    Private Sub llMacros_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llMacros.LinkClicked
        MacrosForm.ShowDialogForm()
    End Sub
End Class