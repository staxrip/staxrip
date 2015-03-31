Imports System.ComponentModel
Imports System.Runtime.Serialization.Formatters.Binary

Imports StaxRip.UI

Public Class ProfilesForm
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
        Me.SuspendLayout()
        '
        'bnDown
        '
        Me.bnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnDown.Location = New System.Drawing.Point(579, 262)
        Me.bnDown.Margin = New System.Windows.Forms.Padding(4)
        Me.bnDown.Size = New System.Drawing.Size(34, 34)
        '
        'bnUp
        '
        Me.bnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnUp.Location = New System.Drawing.Point(579, 196)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(4)
        Me.bnUp.Size = New System.Drawing.Size(34, 34)
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemove.Location = New System.Drawing.Point(545, 55)
        Me.bnRemove.Name = "bnRemove"
        Me.bnRemove.Size = New System.Drawing.Size(100, 34)
        Me.bnRemove.TabIndex = 2
        Me.bnRemove.Text = "Remove"
        '
        'bnRight
        '
        Me.bnRight.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRight.Location = New System.Drawing.Point(612, 229)
        Me.bnRight.Margin = New System.Windows.Forms.Padding(4)
        Me.bnRight.Size = New System.Drawing.Size(34, 34)
        '
        'bnLeft
        '
        Me.bnLeft.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnLeft.Location = New System.Drawing.Point(545, 229)
        Me.bnLeft.Margin = New System.Windows.Forms.Padding(4)
        Me.bnLeft.Size = New System.Drawing.Size(34, 34)
        '
        'lb
        '
        Me.lb.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lb.FormattingEnabled = True
        Me.lb.IntegralHeight = False
        Me.lb.ItemHeight = 25
        Me.lb.Location = New System.Drawing.Point(13, 15)
        Me.lb.Margin = New System.Windows.Forms.Padding(4)
        Me.lb.Name = "lb"
        Me.lb.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lb.Size = New System.Drawing.Size(524, 425)
        Me.lb.TabIndex = 0
        '
        'bnLoad
        '
        Me.bnLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnLoad.Location = New System.Drawing.Point(545, 364)
        Me.bnLoad.Size = New System.Drawing.Size(100, 34)
        Me.bnLoad.Text = "Load"
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAdd.Location = New System.Drawing.Point(545, 13)
        Me.bnAdd.Name = "bnAdd"
        Me.bnAdd.Size = New System.Drawing.Size(100, 34)
        Me.bnAdd.TabIndex = 1
        Me.bnAdd.Text = "Add..."
        '
        'bnRename
        '
        Me.bnRename.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRename.Location = New System.Drawing.Point(545, 139)
        Me.bnRename.Name = "bnRename"
        Me.bnRename.Size = New System.Drawing.Size(100, 34)
        Me.bnRename.TabIndex = 5
        Me.bnRename.Text = "Rename"
        Me.bnRename.UseVisualStyleBackColor = True
        '
        'bnEdit
        '
        Me.bnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnEdit.Location = New System.Drawing.Point(545, 97)
        Me.bnEdit.Name = "bnEdit"
        Me.bnEdit.Size = New System.Drawing.Size(100, 34)
        Me.bnEdit.TabIndex = 4
        Me.bnEdit.Text = "Edit..."
        Me.bnEdit.UseVisualStyleBackColor = True
        '
        'bnClone
        '
        Me.bnClone.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnClone.Location = New System.Drawing.Point(545, 322)
        Me.bnClone.Name = "bnClone"
        Me.bnClone.Size = New System.Drawing.Size(100, 34)
        Me.bnClone.TabIndex = 6
        Me.bnClone.Text = "Clone"
        Me.bnClone.UseVisualStyleBackColor = True
        '
        'bnRestore
        '
        Me.bnRestore.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRestore.Location = New System.Drawing.Point(545, 406)
        Me.bnRestore.Name = "bnRestore"
        Me.bnRestore.Size = New System.Drawing.Size(100, 34)
        Me.bnRestore.TabIndex = 15
        Me.bnRestore.Text = "Restore"
        Me.bnRestore.UseVisualStyleBackColor = True
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(545, 448)
        Me.bnCancel.Size = New System.Drawing.Size(100, 34)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(437, 448)
        Me.bnOK.Size = New System.Drawing.Size(100, 34)
        Me.bnOK.Text = "OK"
        '
        'ProfilesForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(658, 495)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.bnOK)
        Me.Controls.Add(Me.bnRestore)
        Me.Controls.Add(Me.bnRight)
        Me.Controls.Add(Me.bnLeft)
        Me.Controls.Add(Me.bnClone)
        Me.Controls.Add(Me.bnEdit)
        Me.Controls.Add(Me.bnRename)
        Me.Controls.Add(Me.bnRemove)
        Me.Controls.Add(Me.bnDown)
        Me.Controls.Add(Me.lb)
        Me.Controls.Add(Me.bnUp)
        Me.Controls.Add(Me.bnAdd)
        Me.Controls.Add(Me.bnLoad)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Margin = New System.Windows.Forms.Padding(5)
        Me.Name = "ProfilesForm"
        Me.Text = "Profiles"
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

        Me.Profiles = Profiles
        Me.LoadProfileMethod = loadAction
        Me.AddProfileMethod = addFunc
        Me.DefaultsFunc = DefaultsFunc

        For Each i As Profile In Profiles
            lb.Items.Add(i.GetCopy)
        Next

        UpdateControls()

        Dim pad = New Padding(CInt(bnLeft.Width / 8))

        bnLeft.Padding = pad
        bnLeft.ZoomImage = My.Resources.ArrowLeft

        bnUp.Padding = pad
        bnUp.ZoomImage = My.Resources.ArrowLeft
        bnUp.ZoomImage.RotateFlip(RotateFlipType.Rotate90FlipNone)

        bnRight.Padding = pad
        bnRight.ZoomImage = My.Resources.ArrowLeft
        bnRight.ZoomImage.RotateFlip(RotateFlipType.Rotate180FlipNone)

        bnDown.Padding = pad
        bnDown.ZoomImage = My.Resources.ArrowLeft
        bnDown.ZoomImage.RotateFlip(RotateFlipType.Rotate270FlipNone)

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

            g.MainForm.SaveSettings()
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
        Dim o = ObjectHelp.GetCopy(Of Profile)(lb.SelectedItem)

        If o.Edit = DialogResult.OK Then
            lb.Items(lb.SelectedIndex) = o
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
        Dim r = InputBox.Show("Enter a name for a sub menu.")

        If OK(r) Then
            lb.SaveSelection()

            For x = 0 To lb.Items.Count - 1
                If lb.GetSelected(x) Then
                    Dim p = DirectCast(lb.Items(x), Profile)
                    p.Name = r + " | " + p.Name
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