Imports StaxRip.UI

Class EventCommandsEditor
    Inherits DialogBase

#Region " Designer "
    Friend WithEvents bnAdd As ButtonEx
    Friend WithEvents bnRemove As ButtonEx
    Friend WithEvents bnClone As ButtonEx
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents LineControl1 As StaxRip.UI.LineControl
    Friend WithEvents bnDown As ButtonEx
    Friend WithEvents bnUp As ButtonEx
    Friend WithEvents lv As ListViewEx
    Friend WithEvents bnEdit As ButtonEx

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.bnEdit = New StaxRip.UI.ButtonEx()
        Me.bnClone = New StaxRip.UI.ButtonEx()
        Me.bnRemove = New StaxRip.UI.ButtonEx()
        Me.bnAdd = New StaxRip.UI.ButtonEx()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.LineControl1 = New StaxRip.UI.LineControl()
        Me.bnDown = New StaxRip.UI.ButtonEx()
        Me.bnUp = New StaxRip.UI.ButtonEx()
        Me.lv = New StaxRip.UI.ListViewEx()
        Me.SuspendLayout()
        '
        'bnEdit
        '
        Me.bnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnEdit.Location = New System.Drawing.Point(471, 178)
        Me.bnEdit.Size = New System.Drawing.Size(100, 34)
        Me.bnEdit.Text = "Edit..."
        '
        'bnClone
        '
        Me.bnClone.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnClone.Location = New System.Drawing.Point(471, 218)
        Me.bnClone.Size = New System.Drawing.Size(100, 34)
        Me.bnClone.Text = "Clone"
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemove.Location = New System.Drawing.Point(471, 52)
        Me.bnRemove.Size = New System.Drawing.Size(100, 34)
        Me.bnRemove.Text = "Remove"
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAdd.Location = New System.Drawing.Point(471, 12)
        Me.bnAdd.Size = New System.Drawing.Size(100, 34)
        Me.bnAdd.Text = "Add..."
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(471, 476)
        Me.bnCancel.Size = New System.Drawing.Size(100, 34)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(365, 476)
        Me.bnOK.Size = New System.Drawing.Size(100, 34)
        Me.bnOK.Text = "OK"
        '
        'LineControl1
        '
        Me.LineControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineControl1.Location = New System.Drawing.Point(12, 452)
        Me.LineControl1.Margin = New System.Windows.Forms.Padding(4, 2, 5, 2)
        Me.LineControl1.Name = "LineControl1"
        Me.LineControl1.Size = New System.Drawing.Size(559, 17)
        Me.LineControl1.TabIndex = 7
        '
        'bnDown
        '
        Me.bnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnDown.Location = New System.Drawing.Point(471, 132)
        Me.bnDown.Size = New System.Drawing.Size(100, 34)
        Me.bnDown.Text = "Down"
        '
        'bnUp
        '
        Me.bnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnUp.Location = New System.Drawing.Point(471, 92)
        Me.bnUp.Size = New System.Drawing.Size(100, 34)
        Me.bnUp.Text = "Up"
        '
        'lv
        '
        Me.lv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lv.Location = New System.Drawing.Point(12, 12)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(453, 435)
        Me.lv.TabIndex = 21
        '
        'EventCommandsEditor
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(583, 522)
        Me.Controls.Add(Me.lv)
        Me.Controls.Add(Me.bnUp)
        Me.Controls.Add(Me.bnDown)
        Me.Controls.Add(Me.LineControl1)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.bnOK)
        Me.Controls.Add(Me.bnEdit)
        Me.Controls.Add(Me.bnClone)
        Me.Controls.Add(Me.bnRemove)
        Me.Controls.Add(Me.bnAdd)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MinimumSize = New System.Drawing.Size(239, 219)
        Me.Name = "EventCommandsEditor"
        Me.Text = "Event Commands"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Sub New(eventCommands As List(Of EventCommand))
        MyBase.New()
        InitializeComponent()

        lv.UpButton = bnUp
        lv.DownButton = bnDown
        lv.RemoveButton = bnRemove
        lv.SingleSelectionButtons = {bnEdit, bnClone}
        lv.UpdateControls()
        lv.EnableListBoxMode()
        lv.Scrollable = False
        lv.CheckBoxes = True
        lv.MultiSelect = False
        lv.ItemCheckProperty = NameOf(EventCommand.Enabled)
        lv.AddItems(ObjectHelp.GetCopy(eventCommands))
        lv.SelectFirst()
    End Sub

    Private Sub bnAdd_Click() Handles bnAdd.Click
        Dim ec As New EventCommand

        Using f As New EventCommandEditor(ec)
            If f.ShowDialog = DialogResult.OK Then lv.AddItem(ec).Selected = True
        End Using
    End Sub

    Private Sub EventCommandsEditor_HelpRequested() Handles Me.HelpRequested
        Dim f As New HelpForm
        f.Doc.WriteStart(Text)
        f.Doc.WriteP(Strings.EventCommands)
        f.Show()
    End Sub

    Private Sub bnClone_Click() Handles bnClone.Click
        lv.AddItem(ObjectHelp.GetCopy(lv.SelectedItem)).Selected = True
    End Sub

    Private Sub bnEdit_Click(sender As Object, e As EventArgs) Handles bnEdit.Click
        Using f As New EventCommandEditor(lv.SelectedItem(Of EventCommand))
            If f.ShowDialog = DialogResult.OK Then lv.RefreshSelection()
        End Using
    End Sub
End Class