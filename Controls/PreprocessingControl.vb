
Imports StaxRip.UI

Public Class PreprocessingControl
    Inherits UserControl
    Implements IPage

#Region " Designer "
    <DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If

        MyBase.Dispose(disposing)
    End Sub

    Friend WithEvents lv As StaxRip.UI.ListViewEx
    Friend WithEvents bnDown As ButtonEx
    Friend WithEvents bnEdit As ButtonEx
    Friend WithEvents bnRemove As ButtonEx
    Friend WithEvents bnUp As ButtonEx
    Friend WithEvents bnAdd As ButtonEx
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents bnRestore As ButtonEx
    Friend WithEvents flpButtons As FlowLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.flpButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnAdd = New StaxRip.UI.ButtonEx()
        Me.bnRemove = New StaxRip.UI.ButtonEx()
        Me.bnEdit = New StaxRip.UI.ButtonEx()
        Me.bnUp = New StaxRip.UI.ButtonEx()
        Me.bnDown = New StaxRip.UI.ButtonEx()
        Me.bnRestore = New StaxRip.UI.ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.lv = New StaxRip.UI.ListViewEx()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.flpButtons.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'flpButtons
        '
        Me.flpButtons.AutoSize = True
        Me.flpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpButtons.Controls.Add(Me.bnAdd)
        Me.flpButtons.Controls.Add(Me.bnRemove)
        Me.flpButtons.Controls.Add(Me.bnEdit)
        Me.flpButtons.Controls.Add(Me.bnUp)
        Me.flpButtons.Controls.Add(Me.bnDown)
        Me.flpButtons.Controls.Add(Me.bnRestore)
        Me.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpButtons.Location = New System.Drawing.Point(227, 0)
        Me.flpButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flpButtons.Name = "flpButtons"
        Me.flpButtons.Size = New System.Drawing.Size(260, 540)
        Me.flpButtons.TabIndex = 6
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAdd.Location = New System.Drawing.Point(0, 0)
        Me.bnAdd.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnAdd.Size = New System.Drawing.Size(260, 80)
        Me.bnAdd.Text = " Add..."
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemove.Location = New System.Drawing.Point(0, 90)
        Me.bnRemove.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnRemove.Size = New System.Drawing.Size(260, 80)
        Me.bnRemove.Text = "   Remove"
        '
        'bnEdit
        '
        Me.bnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnEdit.Location = New System.Drawing.Point(0, 180)
        Me.bnEdit.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnEdit.Size = New System.Drawing.Size(260, 80)
        Me.bnEdit.Text = " Edit..."
        '
        'bnUp
        '
        Me.bnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnUp.Location = New System.Drawing.Point(0, 270)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnUp.Size = New System.Drawing.Size(260, 80)
        Me.bnUp.Text = "Up"
        '
        'bnDown
        '
        Me.bnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnDown.Location = New System.Drawing.Point(0, 360)
        Me.bnDown.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnDown.Size = New System.Drawing.Size(260, 80)
        Me.bnDown.Text = "Down"
        '
        'bnRestore
        '
        Me.bnRestore.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRestore.Location = New System.Drawing.Point(0, 450)
        Me.bnRestore.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnRestore.Size = New System.Drawing.Size(260, 80)
        Me.bnRestore.Text = "Restore..."
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.Controls.Add(Me.flpButtons, 1, 0)
        Me.tlpMain.Controls.Add(Me.lv, 0, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 1
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(487, 626)
        Me.tlpMain.TabIndex = 7
        '
        'lv
        '
        Me.lv.AllowDrop = True
        Me.lv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lv.CheckBoxes = True
        Me.lv.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lv.DownButton = Me.bnDown
        Me.lv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lv.Location = New System.Drawing.Point(0, 0)
        Me.lv.Margin = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.lv.MultiSelect = False
        Me.lv.Name = "lv"
        Me.lv.RemoveButton = Me.bnRemove
        Me.lv.Size = New System.Drawing.Size(217, 626)
        Me.lv.TabIndex = 0
        Me.lv.UpButton = Me.bnUp
        Me.lv.UseCompatibleStateImageBehavior = False
        Me.lv.View = System.Windows.Forms.View.Details
        '
        'DemuxingControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.tlpMain)
        Me.Margin = New System.Windows.Forms.Padding(6)
        Me.Name = "DemuxingControl"
        Me.Size = New System.Drawing.Size(487, 626)
        Me.flpButtons.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Property Node As TreeNode Implements IPage.Node
    Property Path As String Implements IPage.Path
    Property TipProvider As TipProvider Implements IPage.TipProvider

    Public Property FormSizeScaleFactor As SizeF Implements IPage.FormSizeScaleFactor

    Sub New()
        MyBase.New()
        InitializeComponent()

        TipProvider = New TipProvider(Nothing)
        AddHandler Disposed, Sub() TipProvider.Dispose()

        For Each i In ObjectHelp.GetCopy(Of List(Of Demuxer))(s.Demuxers)
            AddItem(i)
        Next

        bnAdd.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)
        bnUp.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnDown.Image = ImageHelp.GetSymbolImage(Symbol.Down)
        bnEdit.Image = ImageHelp.GetSymbolImage(Symbol.Repair)

        For Each bn In {bnAdd, bnRemove, bnUp, bnDown, bnEdit}
            bn.TextImageRelation = TextImageRelation.Overlay
            bn.ImageAlign = ContentAlignment.MiddleLeft
            Dim pad = bn.Padding
            pad.Left = Control.DefaultFont.Height \ 10
            pad.Right = pad.Left
            bn.Padding = pad
        Next

        TipProvider.TipsFunc = AddressOf GetTips

        lv.SingleSelectionButtons = {bnEdit}
        lv.UpdateControls()
    End Sub

    Function GetTips() As StringPairList
        Dim ret As New StringPairList

        ret.Add("Preprocessing", "The preprocessing menu defines a set of apps that are executed to demux, index or re-mux source files. The applications are executed one by one starting from the top so the next application can use the output from the previous application as input. It's possible to add custom apps by defining command lines.")

        For Each i As ListViewItem In lv.Items
            If TypeOf i.Tag Is CommandLineDemuxer Then
                Dim muxer = DirectCast(i.Tag, CommandLineDemuxer)
                Dim help = muxer.GetHelp

                If help <> "" Then
                    ret.Add(muxer.Name, help)
                End If
            End If
        Next

        Return ret
    End Function

    Function AddItem(value As Demuxer) As ListViewItem
        Dim ret = lv.Items.Add(value.ToString)
        ret.Checked = value.Active
        ret.Tag = value
        Return ret
    End Function

    Protected Overrides Sub OnHandleDestroyed(e As EventArgs)
        If FindForm.DialogResult = DialogResult.OK Then
            s.Demuxers.Clear()

            For Each i As ListViewItem In lv.Items
                Dim demuxer = DirectCast(i.Tag, Demuxer)
                demuxer.Active = i.Checked
                s.Demuxers.Add(demuxer)
            Next
        End If

        MyBase.OnHandleDestroyed(e)
    End Sub

    Sub lv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lv.SelectedIndexChanged
        bnEdit.Enabled = lv.SelectedItems.Count = 1 AndAlso
            DirectCast(lv.SelectedItems(0).Tag, Demuxer).HasConfigDialog
    End Sub

    Sub lv_Layout(sender As Object, e As LayoutEventArgs) Handles lv.Layout
        lv.Columns(0).Width = lv.Width - 5
    End Sub

    Sub bnAdd_Click(sender As Object, e As EventArgs) Handles bnAdd.Click
        Dim sb As New SelectionBox(Of Demuxer)
        sb.Title = "New Demuxer"
        sb.Text = "Please select a Demuxer."

        Dim cli As New CommandLineDemuxer
        cli.Name = "Command Line Demuxer"
        sb.AddItem(cli)

        For Each i In Demuxer.GetDefaults()
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK AndAlso sb.SelectedValue.ShowConfigDialog = DialogResult.OK Then
            AddItem(sb.SelectedValue).Selected = True
        End If
    End Sub

    Sub bnEdit_Click(sender As Object, e As EventArgs) Handles bnEdit.Click
        Dim listItem = lv.SelectedItems(0)
        Dim demuxer = DirectCast(listItem.Tag, Demuxer)
        demuxer.ShowConfigDialog()
        listItem.Text = demuxer.ToString
    End Sub

    Sub bnRestore_Click(sender As Object, e As EventArgs) Handles bnRestore.Click
        If MsgQuestion("Restore defaults?") = DialogResult.OK Then
            lv.Items.Clear()

            For Each dmx In Demuxer.GetDefaults()
                AddItem(dmx)
            Next

            lv.UpdateControls()
        End If
    End Sub
End Class
