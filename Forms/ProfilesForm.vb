Imports StaxRip.UI

Class ProfilesForm
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
    Friend WithEvents bnLoad As ButtonEx
    Friend WithEvents bnDown As ButtonEx
    Friend WithEvents bnUp As ButtonEx
    Friend WithEvents bnRemove As System.Windows.Forms.Button
    Friend WithEvents bnAdd As System.Windows.Forms.Button
    Friend WithEvents bnEdit As System.Windows.Forms.Button
    Friend WithEvents bnClone As System.Windows.Forms.Button
    Friend WithEvents bnRight As ButtonEx
    Friend WithEvents bnLeft As ButtonEx
    Friend WithEvents bnRestore As System.Windows.Forms.Button
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents bnRename As System.Windows.Forms.Button
    '<System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.bnDown = New StaxRip.UI.ButtonEx()
        Me.bnUp = New StaxRip.UI.ButtonEx()
        Me.bnRemove = New System.Windows.Forms.Button()
        Me.bnRight = New StaxRip.UI.ButtonEx()
        Me.bnLeft = New StaxRip.UI.ButtonEx()
        Me.lb = New StaxRip.UI.ListBoxEx()
        Me.bnLoad = New StaxRip.UI.ButtonEx()
        Me.bnAdd = New System.Windows.Forms.Button()
        Me.bnRename = New System.Windows.Forms.Button()
        Me.bnEdit = New System.Windows.Forms.Button()
        Me.bnClone = New System.Windows.Forms.Button()
        Me.bnRestore = New System.Windows.Forms.Button()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnDown
        '
        Me.bnDown.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnDown.Location = New System.Drawing.Point(70, 140)
        Me.bnDown.Margin = New System.Windows.Forms.Padding(0)
        Me.bnDown.Size = New System.Drawing.Size(70, 70)
        '
        'bnUp
        '
        Me.bnUp.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnUp.Location = New System.Drawing.Point(70, 0)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(0)
        Me.bnUp.Size = New System.Drawing.Size(70, 70)
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnRemove.Location = New System.Drawing.Point(0, 90)
        Me.bnRemove.Margin = New System.Windows.Forms.Padding(0, 10, 11, 0)
        Me.bnRemove.Name = "bnRemove"
        Me.bnRemove.Size = New System.Drawing.Size(250, 70)
        Me.bnRemove.TabIndex = 2
        Me.bnRemove.Text = "  Remove"
        '
        'bnRight
        '
        Me.bnRight.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnRight.Location = New System.Drawing.Point(140, 70)
        Me.bnRight.Margin = New System.Windows.Forms.Padding(0)
        Me.bnRight.Size = New System.Drawing.Size(70, 70)
        '
        'bnLeft
        '
        Me.bnLeft.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnLeft.Location = New System.Drawing.Point(0, 70)
        Me.bnLeft.Margin = New System.Windows.Forms.Padding(0)
        Me.bnLeft.Size = New System.Drawing.Size(70, 70)
        '
        'lb
        '
        Me.lb.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lb.FormattingEnabled = True
        Me.lb.IntegralHeight = False
        Me.lb.ItemHeight = 48
        Me.lb.Location = New System.Drawing.Point(11, 10)
        Me.lb.Margin = New System.Windows.Forms.Padding(11, 10, 11, 0)
        Me.lb.Name = "lb"
        Me.lb.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lb.Size = New System.Drawing.Size(877, 782)
        Me.lb.TabIndex = 0
        '
        'bnLoad
        '
        Me.bnLoad.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnLoad.Location = New System.Drawing.Point(0, 630)
        Me.bnLoad.Margin = New System.Windows.Forms.Padding(0, 10, 11, 0)
        Me.bnLoad.Size = New System.Drawing.Size(250, 70)
        Me.bnLoad.Text = "Load"
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnAdd.Location = New System.Drawing.Point(0, 10)
        Me.bnAdd.Margin = New System.Windows.Forms.Padding(0, 10, 11, 0)
        Me.bnAdd.Name = "bnAdd"
        Me.bnAdd.Size = New System.Drawing.Size(250, 70)
        Me.bnAdd.TabIndex = 1
        Me.bnAdd.Text = "Add..."
        '
        'bnRename
        '
        Me.bnRename.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnRename.Location = New System.Drawing.Point(0, 250)
        Me.bnRename.Margin = New System.Windows.Forms.Padding(0, 10, 11, 10)
        Me.bnRename.Name = "bnRename"
        Me.bnRename.Size = New System.Drawing.Size(250, 70)
        Me.bnRename.TabIndex = 5
        Me.bnRename.Text = "  Rename"
        Me.bnRename.UseVisualStyleBackColor = True
        '
        'bnEdit
        '
        Me.bnEdit.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnEdit.Location = New System.Drawing.Point(0, 170)
        Me.bnEdit.Margin = New System.Windows.Forms.Padding(0, 10, 11, 0)
        Me.bnEdit.Name = "bnEdit"
        Me.bnEdit.Size = New System.Drawing.Size(250, 70)
        Me.bnEdit.TabIndex = 4
        Me.bnEdit.Text = "Edit..."
        Me.bnEdit.UseVisualStyleBackColor = True
        '
        'bnClone
        '
        Me.bnClone.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnClone.Location = New System.Drawing.Point(0, 550)
        Me.bnClone.Margin = New System.Windows.Forms.Padding(0, 10, 11, 0)
        Me.bnClone.Name = "bnClone"
        Me.bnClone.Size = New System.Drawing.Size(250, 70)
        Me.bnClone.TabIndex = 6
        Me.bnClone.Text = "Clone"
        Me.bnClone.UseVisualStyleBackColor = True
        '
        'bnRestore
        '
        Me.bnRestore.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnRestore.Location = New System.Drawing.Point(0, 710)
        Me.bnRestore.Margin = New System.Windows.Forms.Padding(0, 10, 11, 0)
        Me.bnRestore.Name = "bnRestore"
        Me.bnRestore.Size = New System.Drawing.Size(250, 70)
        Me.bnRestore.TabIndex = 15
        Me.bnRestore.Text = "Restore"
        Me.bnRestore.UseVisualStyleBackColor = True
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(272, 10)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(11, 10, 11, 10)
        Me.bnCancel.Size = New System.Drawing.Size(250, 70)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(11, 10)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(11, 10, 0, 10)
        Me.bnOK.Size = New System.Drawing.Size(250, 70)
        Me.bnOK.Text = "OK"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.bnLeft, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.bnRight, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.bnUp, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.bnDown, 1, 2)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(20, 330)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0, 0, 11, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(210, 210)
        Me.TableLayoutPanel1.TabIndex = 21
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.AutoSize = True
        Me.TableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel1, 0, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.bnRemove, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.bnRestore, 0, 7)
        Me.TableLayoutPanel2.Controls.Add(Me.bnEdit, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.bnClone, 0, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.bnLoad, 0, 6)
        Me.TableLayoutPanel2.Controls.Add(Me.bnRename, 0, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.bnAdd, 0, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(899, 0)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 8
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(261, 780)
        Me.TableLayoutPanel2.TabIndex = 22
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 3
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.Controls.Add(Me.lb, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.TableLayoutPanel2, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel1, 0, 1)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel3.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 2
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(1160, 882)
        Me.TableLayoutPanel3.TabIndex = 23
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel3.SetColumnSpan(Me.FlowLayoutPanel1, 2)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnOK)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnCancel)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(627, 792)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(533, 90)
        Me.FlowLayoutPanel1.TabIndex = 23
        '
        'ProfilesForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1160, 882)
        Me.Controls.Add(Me.TableLayoutPanel3)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(9)
        Me.Name = "ProfilesForm"
        Me.Text = "Profiles"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Profiles As IList
    Private AddProfileMethod As Func(Of Profile)
    Private LoadProfileMethod As Action(Of Profile)
    Private Counter As Integer
    Private TextValues As New Dictionary(Of Button, String)
    Private DefaultsFunc As Func(Of IList)

    Sub New(title As String,
            profiles As IList,
            loadAction As Action(Of Profile),
            addFunc As Func(Of Profile),
            defaultsFunc As Func(Of IList))

        MyBase.New()
        InitializeComponent()

        Text = title

        Me.Profiles = profiles
        Me.LoadProfileMethod = loadAction
        Me.AddProfileMethod = addFunc
        Me.DefaultsFunc = defaultsFunc

        For Each i As Profile In profiles
            lb.Items.Add(i.GetCopy)
        Next

        UpdateControls()

        bnLeft.Image = ImageHelp.GetSymbolImage(Symbol.Back)
        bnUp.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnRight.Image = ImageHelp.GetSymbolImage(Symbol.Forward)
        bnDown.Image = ImageHelp.GetSymbolImage(Symbol.Down)

        bnAdd.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)
        bnRename.Image = ImageHelp.GetSymbolImage(Symbol.Rename)
        bnClone.Image = ImageHelp.GetSymbolImage(Symbol.TwoPage)
        bnEdit.Image = ImageHelp.GetSymbolImage(Symbol.Repair)

        For Each bn In {bnAdd, bnRemove, bnRename, bnClone, bnEdit}
            bn.TextImageRelation = TextImageRelation.Overlay
            bn.ImageAlign = ContentAlignment.MiddleLeft
            Dim pad = bn.Padding
            pad.Left = Control.DefaultFont.Height \ 10
            pad.Right = pad.Left
            bn.Padding = pad
        Next

        TipProvider.SetTip("Adds all default profiles.", bnRestore)
        TipProvider.SetTip("Moves items up, multiselect is available.", "Up", bnUp)
        TipProvider.SetTip("Moves items down, multiselect is available.", "Down", bnDown)
        TipProvider.SetTip("Moves selected profiles to it's parent menu, multiselect is available.", "Left", bnLeft)
        TipProvider.SetTip("Moves profiles into a sub menu, multiselect is available.", "Right", bnRight)
    End Sub

    Private Sub UpdateControls()
        Dim p = DirectCast(lb.SelectedItem, Profile)
        Dim count = lb.SelectedItems.Count
        bnClone.Enabled = count = 1
        bnEdit.Enabled = count = 1 AndAlso p.CanEdit
        bnLoad.Enabled = count = 1 AndAlso Not LoadProfileMethod Is Nothing
        bnRemove.Enabled = count > 0
        bnRename.Enabled = count = 1
        bnUp.Enabled = lb.CanMoveUp()
        bnDown.Enabled = lb.CanMoveDown()
        bnLeft.Enabled = count > 0 AndAlso lb.Text.Contains(" | ")
        bnRight.Enabled = count > 0
        bnRestore.Enabled = Not DefaultsFunc Is Nothing
    End Sub

    Private Sub lb_KeyDown(sender As Object, e As KeyEventArgs) Handles lb.KeyDown
        Select Case e.KeyData
            Case Keys.Delete
                If bnRemove.Enabled Then bnRemove.PerformClick()
            Case Keys.F2
                If bnRename.Enabled Then bnRename.PerformClick()
            Case Keys.Control Or Keys.Up
                If bnUp.Enabled Then
                    e.Handled = True
                    bnUp.PerformClick()
                End If
            Case Keys.Control Or Keys.Down
                If bnDown.Enabled Then
                    e.Handled = True
                    bnDown.PerformClick()
                End If
        End Select
    End Sub

    Private Sub lb_SelectedIndexChanged() Handles lb.SelectedIndexChanged
        UpdateControls()
    End Sub

    Private Sub bLoad_Click() Handles bnLoad.Click
        LoadProfileMethod(DirectCast(lb.SelectedItem, Profile))
        TextValues(bnLoad) = bnLoad.Text
        bnLoad.ShowBold()
    End Sub

    Private Sub lb_DoubleClick() Handles lb.DoubleClick
        If Not lb.SelectedItem Is Nothing AndAlso DirectCast(lb.SelectedItem, Profile).CanEdit Then
            bnEdit.PerformClick()
        End If
    End Sub

    Private Sub bNew_Click() Handles bnAdd.Click
        Dim p = AddProfileMethod()

        If Not p Is Nothing Then
            p = DirectCast(ObjectHelp.GetCopy(p), Profile)
            p.Name = InputBox.Show("Enter the name of the new profile.", "Name", p.Name)

            If Not p.Name Is Nothing Then
                Dim remove As Profile = Nothing

                For Each i As Profile In lb.Items
                    If i.Name = p.Name Then
                        If MsgOK("There is already a profile with this name, overwrite?") Then
                            remove = i
                        Else
                            Exit Sub
                        End If
                    End If
                Next

                If Not remove Is Nothing Then
                    lb.Items.Remove(remove)
                End If

                p.Clean()
                lb.Items.Insert(0, p)
                lb.ClearSelected()
                lb.SelectedItem = p
                UpdateControls()
            End If
        End If
    End Sub

    Private Sub bRemove_Click() Handles bnRemove.Click
        lb.RemoveSelection()
        UpdateControls()
    End Sub

    Private Sub ProfilesForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If DialogResult = DialogResult.OK Then
            Profiles.Clear()

            For Each i As Profile In lb.Items
                Profiles.Add(i)
            Next

            g.SaveSettings()
        End If
    End Sub

    Private Sub bRename_Click() Handles bnRename.Click
        Dim p = DirectCast(lb.SelectedItem, Profile)
        Dim ret = InputBox.Show("Please enter a name.", "Rename Profile", p.Name)

        If Not ret Is Nothing Then
            p.Name = ret
            lb.UpdateSelection()
            UpdateControls()
        End If
    End Sub

    Private Sub bEdit_Click() Handles bnEdit.Click
        Dim profile = DirectCast(ObjectHelp.GetCopy(lb.SelectedItem), Profile)

        If profile.Edit = DialogResult.OK Then
            lb.Items(lb.SelectedIndex) = profile
            UpdateControls()
        End If
    End Sub

    Private Sub bClone_Click() Handles bnClone.Click
        lb.Items.Insert(lb.SelectedIndex, ObjectHelp.GetCopy(lb.SelectedItem))
    End Sub

    Private Sub bUp_Click(sender As Object, e As EventArgs) Handles bnUp.Click
        lb.MoveSelectionUp()
        UpdateControls()
    End Sub

    Private Sub bDown_Click(sender As Object, e As EventArgs) Handles bnDown.Click
        lb.MoveSelectionDown()
        UpdateControls()
    End Sub

    Private Sub bLeft_Click(sender As Object, e As EventArgs) Handles bnLeft.Click
        lb.SaveSelection()

        For x = 0 To lb.Items.Count - 1
            If lb.GetSelected(x) Then
                Dim p = DirectCast(lb.Items(x), Profile)

                If p.Name.Contains(" | ") Then
                    p.Name = p.Name.Right(" | ")
                    lb.Items(x) = lb.Items(x)
                End If
            End If
        Next

        lb.RestoreSelection()
    End Sub

    Private Sub bRight_Click(sender As Object, e As EventArgs) Handles bnRight.Click
        Dim inputName = InputBox.Show("Enter a name for a sub menu.")

        If inputName <> "" Then
            lb.SaveSelection()

            For x = 0 To lb.Items.Count - 1
                If lb.GetSelected(x) Then
                    Dim p = DirectCast(lb.Items(x), Profile)
                    p.Name = inputName + " | " + p.Name
                    lb.Items(x) = lb.Items(x)
                End If
            Next

            lb.RestoreSelection()
        End If
    End Sub

    Private Sub bRestore_Click(sender As Object, e As EventArgs) Handles bnRestore.Click
        lb.Items.Clear()
        lb.Items.AddRange(DefaultsFunc().OfType(Of Profile).ToArray)
    End Sub

    Private Sub ProfilesForm_HelpRequested() Handles Me.HelpRequested
        Dim f As New HelpForm()
        f.Doc.WriteStart(Text)
        f.Doc.WriteTips(TipProvider.GetTips)
        f.Show()
    End Sub
End Class