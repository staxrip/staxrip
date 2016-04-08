Imports StaxRip.UI

Class DemuxingControl
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

    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.bnRestore = New StaxRip.UI.ButtonEx()
        Me.bnAdd = New StaxRip.UI.ButtonEx()
        Me.bnDown = New StaxRip.UI.ButtonEx()
        Me.bnEdit = New StaxRip.UI.ButtonEx()
        Me.bnRemove = New StaxRip.UI.ButtonEx()
        Me.bnUp = New StaxRip.UI.ButtonEx()
        Me.lv = New StaxRip.UI.ListViewEx()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SuspendLayout()
        '
        'buRestore
        '
        Me.bnRestore.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRestore.Location = New System.Drawing.Point(306, 199)
        Me.bnRestore.Size = New System.Drawing.Size(100, 34)
        Me.bnRestore.Text = "Restore..."
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAdd.Location = New System.Drawing.Point(306, -1)
        Me.bnAdd.Size = New System.Drawing.Size(100, 34)
        Me.bnAdd.Text = "Add..."
        '
        'bnDown
        '
        Me.bnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnDown.Location = New System.Drawing.Point(306, 159)
        Me.bnDown.Size = New System.Drawing.Size(100, 34)
        Me.bnDown.Text = "Down"
        '
        'bnEdit
        '
        Me.bnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnEdit.Location = New System.Drawing.Point(306, 79)
        Me.bnEdit.Size = New System.Drawing.Size(100, 34)
        Me.bnEdit.Text = "Edit..."
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemove.Location = New System.Drawing.Point(306, 39)
        Me.bnRemove.Size = New System.Drawing.Size(100, 34)
        Me.bnRemove.Text = "Remove"
        '
        'buUp
        '
        Me.bnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnUp.Location = New System.Drawing.Point(306, 119)
        Me.bnUp.Size = New System.Drawing.Size(100, 34)
        Me.bnUp.Text = "Up"
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
        Me.lv.MultiSelect = False
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(300, 303)
        Me.lv.TabIndex = 0
        Me.lv.UpButton = Me.bnUp
        Me.lv.UseCompatibleStateImageBehavior = False
        Me.lv.View = System.Windows.Forms.View.Details
        '
        'DemuxingControl
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Controls.Add(Me.bnRestore)
        Me.Controls.Add(Me.bnAdd)
        Me.Controls.Add(Me.bnDown)
        Me.Controls.Add(Me.bnEdit)
        Me.Controls.Add(Me.bnRemove)
        Me.Controls.Add(Me.bnUp)
        Me.Controls.Add(Me.lv)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "DemuxingControl"
        Me.Size = New System.Drawing.Size(405, 303)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Property Node As TreeNode Implements IPage.Node
    Property Path As String Implements IPage.Path
    Property TipProvider As TipProvider Implements IPage.TipProvider

    Sub New()
        MyBase.New()
        InitializeComponent()

        TipProvider = New TipProvider(Nothing)
        AddHandler Disposed, Sub() TipProvider.Dispose()

        For Each i In ObjectHelp.GetCopy(Of List(Of Demuxer))(s.Demuxers)
            AddItem(i)
        Next

        TipProvider.TipsFunc = AddressOf GetTips
        lv.UpdateControls()
    End Sub

    Function GetTips() As StringPairList
        Dim ret As New StringPairList

        ret.Add("Demuxing", "The demux menu defines a set of applications that are executed to demux and index source file(s). The applications are executed one by one starting from the top so the next application can use the output from the previous application as input. It's possible to add custom demuxers by defining command lines.")

        For Each i As ListViewItem In lv.Items
            If TypeOf i.Tag Is CommandLineDemuxer Then
                Dim muxer = DirectCast(i.Tag, CommandLineDemuxer)
                Dim help = muxer.GetHelp
                If help <> "" Then ret.Add(muxer.Name, help)
            End If
        Next

        Return ret
    End Function

    Function AddItem(value As Demuxer) As ListViewItem
        Dim r = lv.Items.Add(value.ToString)
        r.Checked = value.Active
        r.Tag = value
        Return r
    End Function

    Private Sub bnAdd_Click() Handles bnAdd.Click
        Dim sb As New SelectionBox(Of Demuxer)
        sb.Title = "New Demuxer"
        sb.Text = "Please select a Demuxer."

        Dim cli As New CommandLineDemuxer
        cli.Name = "Command Line Demuxer"
        sb.AddItem(cli)

        For Each i In Demuxer.GetDefaults()
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK AndAlso sb.SelectedItem.ShowConfigDialog = DialogResult.OK Then
            AddItem(sb.SelectedItem).Selected = True
        End If
    End Sub

    Private Sub lv_Layout(sender As Object, e As LayoutEventArgs) Handles lv.Layout
        lv.Columns(0).Width = lv.Width - 5
    End Sub

    Private Sub bnEdit_Click() Handles bnEdit.Click
        Dim listItem = lv.SelectedItems(0)
        Dim demuxer = DirectCast(listItem.Tag, Demuxer)
        demuxer.ShowConfigDialog()
        listItem.Text = demuxer.ToString
    End Sub

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

    Private Sub bnRestore_Click() Handles bnRestore.Click
        If MsgQuestion("Restore defaults?", MessageBoxButtons.OKCancel) = DialogResult.OK Then
            lv.Items.Clear()

            For Each i In Demuxer.GetDefaults()
                AddItem(i)
            Next
        End If
    End Sub

    Private Sub lv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lv.SelectedIndexChanged
        bnEdit.Enabled = lv.SelectedItems.Count = 1 AndAlso
            DirectCast(lv.SelectedItems(0).Tag, Demuxer).HasConfigDialog
    End Sub
End Class