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
    Friend WithEvents flpButtons As FlowLayoutPanel
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
        Me.flpButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.flpButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnEdit
        '
        Me.bnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnEdit.Location = New System.Drawing.Point(0, 320)
        Me.bnEdit.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnEdit.Size = New System.Drawing.Size(250, 70)
        Me.bnEdit.Text = "Edit..."
        '
        'bnClone
        '
        Me.bnClone.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnClone.Location = New System.Drawing.Point(0, 400)
        Me.bnClone.Margin = New System.Windows.Forms.Padding(0)
        Me.bnClone.Size = New System.Drawing.Size(250, 70)
        Me.bnClone.Text = "Clone"
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemove.Location = New System.Drawing.Point(0, 80)
        Me.bnRemove.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnRemove.Size = New System.Drawing.Size(250, 70)
        Me.bnRemove.Text = "  Remove"
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAdd.Location = New System.Drawing.Point(0, 0)
        Me.bnAdd.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnAdd.Size = New System.Drawing.Size(250, 70)
        Me.bnAdd.Text = "Add..."
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(934, 813)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(5)
        Me.bnCancel.Size = New System.Drawing.Size(250, 70)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(674, 813)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(5)
        Me.bnOK.Size = New System.Drawing.Size(250, 70)
        Me.bnOK.Text = "OK"
        '
        'LineControl1
        '
        Me.LineControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineControl1.Location = New System.Drawing.Point(22, 777)
        Me.LineControl1.Margin = New System.Windows.Forms.Padding(7, 3, 9, 3)
        Me.LineControl1.Name = "LineControl1"
        Me.LineControl1.Size = New System.Drawing.Size(1154, 29)
        Me.LineControl1.TabIndex = 7
        '
        'bnDown
        '
        Me.bnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnDown.Location = New System.Drawing.Point(0, 240)
        Me.bnDown.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnDown.Size = New System.Drawing.Size(250, 70)
        Me.bnDown.Text = "Down"
        '
        'bnUp
        '
        Me.bnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnUp.Location = New System.Drawing.Point(0, 160)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnUp.Size = New System.Drawing.Size(250, 70)
        Me.bnUp.Text = "Up"
        '
        'lv
        '
        Me.lv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lv.Location = New System.Drawing.Point(19, 19)
        Me.lv.Margin = New System.Windows.Forms.Padding(10)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(890, 750)
        Me.lv.TabIndex = 21
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'flpButtons
        '
        Me.flpButtons.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpButtons.AutoSize = True
        Me.flpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpButtons.Controls.Add(Me.bnAdd)
        Me.flpButtons.Controls.Add(Me.bnRemove)
        Me.flpButtons.Controls.Add(Me.bnUp)
        Me.flpButtons.Controls.Add(Me.bnDown)
        Me.flpButtons.Controls.Add(Me.bnEdit)
        Me.flpButtons.Controls.Add(Me.bnClone)
        Me.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpButtons.Location = New System.Drawing.Point(929, 19)
        Me.flpButtons.Margin = New System.Windows.Forms.Padding(10)
        Me.flpButtons.Name = "flpButtons"
        Me.flpButtons.Size = New System.Drawing.Size(250, 470)
        Me.flpButtons.TabIndex = 30
        '
        'EventCommandsEditor
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1198, 897)
        Me.Controls.Add(Me.flpButtons)
        Me.Controls.Add(Me.lv)
        Me.Controls.Add(Me.LineControl1)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.bnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(7)
        Me.MinimumSize = New System.Drawing.Size(405, 302)
        Me.Name = "EventCommandsEditor"
        Me.Text = "Event Commands"
        Me.flpButtons.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

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

        bnUp.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnDown.Image = ImageHelp.GetSymbolImage(Symbol.Down)
        bnAdd.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)
        bnClone.Image = ImageHelp.GetSymbolImage(Symbol.TwoPage)
        bnEdit.Image = ImageHelp.GetSymbolImage(Symbol.Repair)

        For Each bn In {bnAdd, bnRemove, bnClone, bnEdit, bnUp, bnDown}
            bn.TextImageRelation = TextImageRelation.Overlay
            bn.ImageAlign = ContentAlignment.MiddleLeft
            Dim pad = bn.Padding
            pad.Left = Control.DefaultFont.Height \ 10
            pad.Right = pad.Left
            bn.Padding = pad
        Next
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