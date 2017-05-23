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
    Friend WithEvents tlpNav As TableLayoutPanel
    Friend WithEvents tlpButtons As TableLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents flpOkCancel As FlowLayoutPanel
    Friend WithEvents tlpTop As TableLayoutPanel
    Friend WithEvents pnListBox As Panel
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
        Me.tlpNav = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpButtons = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpTop = New System.Windows.Forms.TableLayoutPanel()
        Me.pnListBox = New System.Windows.Forms.Panel()
        Me.flpOkCancel = New System.Windows.Forms.FlowLayoutPanel()
        Me.tlpNav.SuspendLayout()
        Me.tlpButtons.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.tlpTop.SuspendLayout()
        Me.pnListBox.SuspendLayout()
        Me.flpOkCancel.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnDown
        '
        Me.bnDown.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnDown.Location = New System.Drawing.Point(80, 160)
        Me.bnDown.Margin = New System.Windows.Forms.Padding(0)
        Me.bnDown.Size = New System.Drawing.Size(80, 80)
        '
        'bnUp
        '
        Me.bnUp.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnUp.Location = New System.Drawing.Point(80, 0)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(0)
        Me.bnUp.Size = New System.Drawing.Size(80, 80)
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnRemove.Location = New System.Drawing.Point(0, 101)
        Me.bnRemove.Margin = New System.Windows.Forms.Padding(0, 6, 0, 0)
        Me.bnRemove.Name = "bnRemove"
        Me.bnRemove.Size = New System.Drawing.Size(240, 80)
        Me.bnRemove.TabIndex = 2
        Me.bnRemove.Text = "     Remove"
        '
        'bnRight
        '
        Me.bnRight.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnRight.Location = New System.Drawing.Point(160, 80)
        Me.bnRight.Margin = New System.Windows.Forms.Padding(0)
        Me.bnRight.Size = New System.Drawing.Size(80, 80)
        '
        'bnLeft
        '
        Me.bnLeft.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnLeft.Location = New System.Drawing.Point(0, 80)
        Me.bnLeft.Margin = New System.Windows.Forms.Padding(0)
        Me.bnLeft.Size = New System.Drawing.Size(80, 80)
        '
        'lb
        '
        Me.lb.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lb.FormattingEnabled = True
        Me.lb.IntegralHeight = False
        Me.lb.ItemHeight = 48
        Me.lb.Location = New System.Drawing.Point(0, 0)
        Me.lb.Margin = New System.Windows.Forms.Padding(15, 15, 15, 0)
        Me.lb.Name = "lb"
        Me.lb.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lb.Size = New System.Drawing.Size(660, 995)
        Me.lb.TabIndex = 0
        '
        'bnLoad
        '
        Me.bnLoad.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnLoad.Location = New System.Drawing.Point(0, 691)
        Me.bnLoad.Margin = New System.Windows.Forms.Padding(0, 6, 0, 0)
        Me.bnLoad.Size = New System.Drawing.Size(240, 80)
        Me.bnLoad.Text = "Load"
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnAdd.Location = New System.Drawing.Point(0, 15)
        Me.bnAdd.Margin = New System.Windows.Forms.Padding(0, 15, 0, 0)
        Me.bnAdd.Name = "bnAdd"
        Me.bnAdd.Size = New System.Drawing.Size(240, 80)
        Me.bnAdd.TabIndex = 1
        Me.bnAdd.Text = "  Add..."
        '
        'bnRename
        '
        Me.bnRename.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnRename.Location = New System.Drawing.Point(0, 273)
        Me.bnRename.Margin = New System.Windows.Forms.Padding(0, 6, 0, 0)
        Me.bnRename.Name = "bnRename"
        Me.bnRename.Size = New System.Drawing.Size(240, 80)
        Me.bnRename.TabIndex = 5
        Me.bnRename.Text = "     Rename"
        Me.bnRename.UseVisualStyleBackColor = True
        '
        'bnEdit
        '
        Me.bnEdit.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnEdit.Location = New System.Drawing.Point(0, 187)
        Me.bnEdit.Margin = New System.Windows.Forms.Padding(0, 6, 0, 0)
        Me.bnEdit.Name = "bnEdit"
        Me.bnEdit.Size = New System.Drawing.Size(240, 80)
        Me.bnEdit.TabIndex = 4
        Me.bnEdit.Text = " Edit..."
        Me.bnEdit.UseVisualStyleBackColor = True
        '
        'bnClone
        '
        Me.bnClone.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnClone.Location = New System.Drawing.Point(0, 605)
        Me.bnClone.Margin = New System.Windows.Forms.Padding(0, 6, 0, 0)
        Me.bnClone.Name = "bnClone"
        Me.bnClone.Size = New System.Drawing.Size(240, 80)
        Me.bnClone.TabIndex = 6
        Me.bnClone.Text = "  Clone"
        Me.bnClone.UseVisualStyleBackColor = True
        '
        'bnRestore
        '
        Me.bnRestore.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnRestore.Location = New System.Drawing.Point(0, 777)
        Me.bnRestore.Margin = New System.Windows.Forms.Padding(0, 6, 0, 0)
        Me.bnRestore.Name = "bnRestore"
        Me.bnRestore.Size = New System.Drawing.Size(240, 80)
        Me.bnRestore.TabIndex = 15
        Me.bnRestore.Text = "Restore"
        Me.bnRestore.UseVisualStyleBackColor = True
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(255, 15)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(15, 15, 0, 15)
        Me.bnCancel.Size = New System.Drawing.Size(240, 80)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(0, 15)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(0, 15, 0, 15)
        Me.bnOK.Size = New System.Drawing.Size(240, 80)
        Me.bnOK.Text = "OK"
        '
        'tlpNav
        '
        Me.tlpNav.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.tlpNav.AutoSize = True
        Me.tlpNav.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpNav.ColumnCount = 3
        Me.tlpNav.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpNav.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpNav.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpNav.Controls.Add(Me.bnLeft, 0, 1)
        Me.tlpNav.Controls.Add(Me.bnRight, 2, 1)
        Me.tlpNav.Controls.Add(Me.bnUp, 1, 0)
        Me.tlpNav.Controls.Add(Me.bnDown, 1, 2)
        Me.tlpNav.Location = New System.Drawing.Point(0, 359)
        Me.tlpNav.Margin = New System.Windows.Forms.Padding(0, 6, 0, 0)
        Me.tlpNav.Name = "tlpNav"
        Me.tlpNav.RowCount = 3
        Me.tlpNav.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpNav.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpNav.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpNav.Size = New System.Drawing.Size(240, 240)
        Me.tlpNav.TabIndex = 21
        '
        'tlpButtons
        '
        Me.tlpButtons.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpButtons.AutoSize = True
        Me.tlpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpButtons.ColumnCount = 1
        Me.tlpButtons.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpButtons.Controls.Add(Me.tlpNav, 0, 4)
        Me.tlpButtons.Controls.Add(Me.bnRemove, 0, 1)
        Me.tlpButtons.Controls.Add(Me.bnRestore, 0, 7)
        Me.tlpButtons.Controls.Add(Me.bnEdit, 0, 2)
        Me.tlpButtons.Controls.Add(Me.bnClone, 0, 5)
        Me.tlpButtons.Controls.Add(Me.bnLoad, 0, 6)
        Me.tlpButtons.Controls.Add(Me.bnRename, 0, 3)
        Me.tlpButtons.Controls.Add(Me.bnAdd, 0, 0)
        Me.tlpButtons.Location = New System.Drawing.Point(690, 0)
        Me.tlpButtons.Margin = New System.Windows.Forms.Padding(0, 0, 15, 0)
        Me.tlpButtons.Name = "tlpButtons"
        Me.tlpButtons.RowCount = 8
        Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpButtons.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpButtons.Size = New System.Drawing.Size(240, 857)
        Me.tlpButtons.TabIndex = 22
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.Controls.Add(Me.tlpTop, 0, 2)
        Me.tlpMain.Controls.Add(Me.flpOkCancel, 0, 3)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 4
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tlpMain.Size = New System.Drawing.Size(945, 1120)
        Me.tlpMain.TabIndex = 23
        '
        'tlpTop
        '
        Me.tlpTop.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpTop.ColumnCount = 2
        Me.tlpTop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpTop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpTop.Controls.Add(Me.pnListBox, 0, 0)
        Me.tlpTop.Controls.Add(Me.tlpButtons, 1, 0)
        Me.tlpTop.Location = New System.Drawing.Point(0, 0)
        Me.tlpTop.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpTop.Name = "tlpTop"
        Me.tlpTop.RowCount = 1
        Me.tlpTop.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpTop.Size = New System.Drawing.Size(945, 1010)
        Me.tlpTop.TabIndex = 24
        '
        'pnListBox
        '
        Me.pnListBox.Controls.Add(Me.lb)
        Me.pnListBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnListBox.Location = New System.Drawing.Point(15, 15)
        Me.pnListBox.Margin = New System.Windows.Forms.Padding(15, 15, 15, 0)
        Me.pnListBox.Name = "pnListBox"
        Me.pnListBox.Size = New System.Drawing.Size(660, 995)
        Me.pnListBox.TabIndex = 24
        '
        'flpOkCancel
        '
        Me.flpOkCancel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.flpOkCancel.AutoSize = True
        Me.flpOkCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpMain.SetColumnSpan(Me.flpOkCancel, 2)
        Me.flpOkCancel.Controls.Add(Me.bnOK)
        Me.flpOkCancel.Controls.Add(Me.bnCancel)
        Me.flpOkCancel.Location = New System.Drawing.Point(435, 1010)
        Me.flpOkCancel.Margin = New System.Windows.Forms.Padding(0, 0, 15, 0)
        Me.flpOkCancel.Name = "flpOkCancel"
        Me.flpOkCancel.Size = New System.Drawing.Size(495, 110)
        Me.flpOkCancel.TabIndex = 23
        '
        'ProfilesForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(945, 1120)
        Me.Controls.Add(Me.tlpMain)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(9)
        Me.Name = "ProfilesForm"
        Me.Text = "Profiles"
        Me.tlpNav.ResumeLayout(False)
        Me.tlpButtons.ResumeLayout(False)
        Me.tlpButtons.PerformLayout()
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.tlpTop.ResumeLayout(False)
        Me.tlpTop.PerformLayout()
        Me.pnListBox.ResumeLayout(False)
        Me.flpOkCancel.ResumeLayout(False)
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

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        If DialogResult = DialogResult.OK Then
            Profiles.Clear()

            For Each i As Profile In lb.Items
                Profiles.Add(i)
            Next

            g.SaveSettings()
        End If

        MyBase.OnFormClosed(e)
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

    Protected Overrides Sub OnHelpRequested(hevent As HelpEventArgs)
        Dim f As New HelpForm()
        f.Doc.WriteStart(Text)
        f.Doc.WriteTips(TipProvider.GetTips)
        f.Show()

        MyBase.OnHelpRequested(hevent)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        Dim h = tlpButtons.Height + flpOkCancel.Height
        ClientSize = New Size(tlpButtons.Height + tlpButtons.Width, h)
    End Sub
End Class