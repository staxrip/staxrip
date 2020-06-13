
Imports StaxRip.UI

Public Class EventCommandsEditor
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
    Friend WithEvents tlpTop As TableLayoutPanel
    Friend WithEvents flpBottom As FlowLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
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
        Me.tlpTop = New System.Windows.Forms.TableLayoutPanel()
        Me.flpBottom = New System.Windows.Forms.FlowLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.flpButtons.SuspendLayout()
        Me.tlpTop.SuspendLayout()
        Me.flpBottom.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnEdit
        '
        Me.bnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnEdit.Location = New System.Drawing.Point(0, 360)
        Me.bnEdit.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnEdit.Size = New System.Drawing.Size(250, 80)
        Me.bnEdit.Text = " Edit..."
        '
        'bnClone
        '
        Me.bnClone.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnClone.Location = New System.Drawing.Point(0, 450)
        Me.bnClone.Margin = New System.Windows.Forms.Padding(0)
        Me.bnClone.Size = New System.Drawing.Size(250, 80)
        Me.bnClone.Text = " Clone"
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemove.Location = New System.Drawing.Point(0, 90)
        Me.bnRemove.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnRemove.Size = New System.Drawing.Size(250, 80)
        Me.bnRemove.Text = "    Remove"
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAdd.Location = New System.Drawing.Point(0, 0)
        Me.bnAdd.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnAdd.Size = New System.Drawing.Size(250, 80)
        Me.bnAdd.Text = " Add..."
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(270, 10)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(10)
        Me.bnCancel.Size = New System.Drawing.Size(250, 80)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(10, 10)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(10, 10, 0, 10)
        Me.bnOK.Size = New System.Drawing.Size(250, 80)
        Me.bnOK.Text = "OK"
        '
        'LineControl1
        '
        Me.LineControl1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineControl1.Location = New System.Drawing.Point(10, 588)
        Me.LineControl1.Margin = New System.Windows.Forms.Padding(10, 3, 10, 3)
        Me.LineControl1.Name = "LineControl1"
        Me.LineControl1.Size = New System.Drawing.Size(785, 10)
        Me.LineControl1.TabIndex = 7
        '
        'bnDown
        '
        Me.bnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnDown.Location = New System.Drawing.Point(0, 270)
        Me.bnDown.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnDown.Size = New System.Drawing.Size(250, 80)
        Me.bnDown.Text = "  Down"
        '
        'bnUp
        '
        Me.bnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnUp.Location = New System.Drawing.Point(0, 180)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.bnUp.Size = New System.Drawing.Size(250, 80)
        Me.bnUp.Text = "Up"
        '
        'lv
        '
        Me.lv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lv.Location = New System.Drawing.Point(10, 10)
        Me.lv.Margin = New System.Windows.Forms.Padding(10, 10, 10, 6)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(519, 563)
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
        Me.flpButtons.Location = New System.Drawing.Point(539, 10)
        Me.flpButtons.Margin = New System.Windows.Forms.Padding(0, 10, 10, 10)
        Me.flpButtons.Name = "flpButtons"
        Me.flpButtons.Size = New System.Drawing.Size(250, 530)
        Me.flpButtons.TabIndex = 30
        '
        'tlpTop
        '
        Me.tlpTop.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpTop.ColumnCount = 2
        Me.tlpTop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpTop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpTop.Controls.Add(Me.flpButtons, 1, 0)
        Me.tlpTop.Controls.Add(Me.lv, 0, 0)
        Me.tlpTop.Location = New System.Drawing.Point(3, 3)
        Me.tlpTop.Name = "tlpTop"
        Me.tlpTop.RowCount = 1
        Me.tlpTop.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpTop.Size = New System.Drawing.Size(799, 579)
        Me.tlpTop.TabIndex = 33
        '
        'flpBottom
        '
        Me.flpBottom.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.flpBottom.AutoSize = True
        Me.flpBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpBottom.Controls.Add(Me.bnOK)
        Me.flpBottom.Controls.Add(Me.bnCancel)
        Me.flpBottom.Location = New System.Drawing.Point(275, 601)
        Me.flpBottom.Margin = New System.Windows.Forms.Padding(0, 0, 0, 6)
        Me.flpBottom.Name = "flpBottom"
        Me.flpBottom.Size = New System.Drawing.Size(530, 100)
        Me.flpBottom.TabIndex = 34
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.flpBottom, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.tlpTop, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.LineControl1, 0, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 3
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(805, 707)
        Me.TableLayoutPanel2.TabIndex = 35
        '
        'EventCommandsEditor
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(805, 707)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(7)
        Me.MinimumSize = New System.Drawing.Size(405, 302)
        Me.Name = "EventCommandsEditor"
        Me.Text = "Event Command"
        Me.flpButtons.ResumeLayout(False)
        Me.tlpTop.ResumeLayout(False)
        Me.tlpTop.PerformLayout()
        Me.flpBottom.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Sub New(eventCommands As List(Of EventCommand))
        MyBase.New()
        InitializeComponent()

        ScaleClientSize(25, 20)

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

    Sub bnAdd_Click() Handles bnAdd.Click
        Dim ec As New EventCommand

        Using editor As New EventCommandEditor(ec)
            If editor.ShowDialog = DialogResult.OK Then
                lv.AddItem(ec).Selected = True
            End If
        End Using
    End Sub

    Sub EventCommandsEditor_HelpRequested() Handles Me.HelpRequested
        g.ShowPage("commands")
    End Sub

    Sub bnClone_Click() Handles bnClone.Click
        lv.AddItem(ObjectHelp.GetCopy(lv.SelectedItem)).Selected = True
    End Sub

    Sub bnEdit_Click(sender As Object, e As EventArgs) Handles bnEdit.Click
        Using form As New EventCommandEditor(lv.SelectedItem(Of EventCommand))
            If form.ShowDialog = DialogResult.OK Then
                lv.RefreshSelection()
            End If
        End Using
    End Sub
End Class
