﻿
Imports StaxRip.UI

Public Class ProfilesForm
    Inherits DialogBase

#Region " Designer "
    Private components As System.ComponentModel.IContainer

    Friend WithEvents lbMain As ListBoxEx
    Friend WithEvents bnLoad As ButtonEx
    Friend WithEvents bnDown As ButtonEx
    Friend WithEvents bnUp As ButtonEx
    Friend WithEvents bnRemove As ButtonEx
    Friend WithEvents bnAdd As ButtonEx
    Friend WithEvents bnEdit As ButtonEx
    Friend WithEvents bnClone As ButtonEx
    Friend WithEvents bnRight As ButtonEx
    Friend WithEvents bnLeft As ButtonEx
    Friend WithEvents bnRestore As ButtonEx
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents tlpArrows As TableLayoutPanel
    Friend WithEvents pnListBox As Panel
    Friend WithEvents flpRight As FlowLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents bnRename As ButtonEx
    '<System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.bnDown = New ButtonEx()
        Me.bnUp = New ButtonEx()
        Me.bnRemove = New ButtonEx()
        Me.bnRight = New ButtonEx()
        Me.bnLeft = New ButtonEx()
        Me.lbMain = New ListBoxEx()
        Me.bnLoad = New ButtonEx()
        Me.bnAdd = New ButtonEx()
        Me.bnRename = New ButtonEx()
        Me.bnEdit = New ButtonEx()
        Me.bnClone = New ButtonEx()
        Me.bnRestore = New ButtonEx()
        Me.bnCancel = New ButtonEx()
        Me.bnOK = New ButtonEx()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.tlpArrows = New System.Windows.Forms.TableLayoutPanel()
        Me.pnListBox = New System.Windows.Forms.Panel()
        Me.flpRight = New System.Windows.Forms.FlowLayoutPanel()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpArrows.SuspendLayout()
        Me.pnListBox.SuspendLayout()
        Me.flpRight.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnDown
        '
        Me.bnDown.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnDown.Location = New System.Drawing.Point(0, 85)
        Me.bnDown.Margin = New System.Windows.Forms.Padding(0)
        Me.bnDown.Size = New System.Drawing.Size(112, 70)
        '
        'bnUp
        '
        Me.bnUp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnUp.Location = New System.Drawing.Point(0, 0)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(0)
        Me.bnUp.Size = New System.Drawing.Size(112, 70)
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnRemove.Location = New System.Drawing.Point(0, 95)
        Me.bnRemove.Margin = New System.Windows.Forms.Padding(0, 0, 0, 15)
        Me.bnRemove.Name = "bnRemove"
        Me.bnRemove.Size = New System.Drawing.Size(240, 80)
        Me.bnRemove.TabIndex = 2
        Me.bnRemove.Text = "     Remove"
        '
        'bnRight
        '
        Me.bnRight.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRight.Location = New System.Drawing.Point(127, 85)
        Me.bnRight.Margin = New System.Windows.Forms.Padding(0)
        Me.bnRight.Size = New System.Drawing.Size(113, 70)
        '
        'bnLeft
        '
        Me.bnLeft.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnLeft.Location = New System.Drawing.Point(127, 0)
        Me.bnLeft.Margin = New System.Windows.Forms.Padding(0)
        Me.bnLeft.Size = New System.Drawing.Size(113, 70)
        '
        'lbMain
        '
        Me.lbMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lbMain.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.lbMain.FormattingEnabled = True
        Me.lbMain.IntegralHeight = False
        Me.lbMain.ItemHeight = 67
        Me.lbMain.Location = New System.Drawing.Point(0, 0)
        Me.lbMain.Margin = New System.Windows.Forms.Padding(15, 15, 15, 0)
        Me.lbMain.Name = "lbMain"
        Me.lbMain.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lbMain.Size = New System.Drawing.Size(825, 820)
        Me.lbMain.TabIndex = 0
        '
        'bnLoad
        '
        Me.bnLoad.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnLoad.Location = New System.Drawing.Point(0, 645)
        Me.bnLoad.Margin = New System.Windows.Forms.Padding(0, 15, 0, 0)
        Me.bnLoad.Size = New System.Drawing.Size(240, 80)
        Me.bnLoad.Text = "Load"
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnAdd.Location = New System.Drawing.Point(0, 0)
        Me.bnAdd.Margin = New System.Windows.Forms.Padding(0, 0, 0, 15)
        Me.bnAdd.Name = "bnAdd"
        Me.bnAdd.Size = New System.Drawing.Size(240, 80)
        Me.bnAdd.TabIndex = 1
        Me.bnAdd.Text = "  Add..."
        '
        'bnRename
        '
        Me.bnRename.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnRename.Location = New System.Drawing.Point(0, 285)
        Me.bnRename.Margin = New System.Windows.Forms.Padding(0, 0, 0, 15)
        Me.bnRename.Name = "bnRename"
        Me.bnRename.Size = New System.Drawing.Size(240, 80)
        Me.bnRename.TabIndex = 5
        Me.bnRename.Text = "     Rename"
        '
        'bnEdit
        '
        Me.bnEdit.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnEdit.Location = New System.Drawing.Point(0, 190)
        Me.bnEdit.Margin = New System.Windows.Forms.Padding(0, 0, 0, 15)
        Me.bnEdit.Name = "bnEdit"
        Me.bnEdit.Size = New System.Drawing.Size(240, 80)
        Me.bnEdit.TabIndex = 4
        Me.bnEdit.Text = " Edit..."
        '
        'bnClone
        '
        Me.bnClone.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnClone.Location = New System.Drawing.Point(0, 550)
        Me.bnClone.Margin = New System.Windows.Forms.Padding(0, 15, 0, 0)
        Me.bnClone.Name = "bnClone"
        Me.bnClone.Size = New System.Drawing.Size(240, 80)
        Me.bnClone.TabIndex = 6
        Me.bnClone.Text = "  Clone"
        '
        'bnRestore
        '
        Me.bnRestore.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnRestore.Location = New System.Drawing.Point(0, 740)
        Me.bnRestore.Margin = New System.Windows.Forms.Padding(0, 15, 0, 0)
        Me.bnRestore.Name = "bnRestore"
        Me.bnRestore.Size = New System.Drawing.Size(240, 80)
        Me.bnRestore.TabIndex = 15
        Me.bnRestore.Text = "Restore"
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(855, 850)
        Me.bnCancel.Size = New System.Drawing.Size(240, 80)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(600, 850)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(600, 15, 0, 15)
        Me.bnOK.Size = New System.Drawing.Size(240, 80)
        Me.bnOK.Text = "OK"
        '
        'tlpArrows
        '
        Me.tlpArrows.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.tlpArrows.ColumnCount = 3
        Me.tlpArrows.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpArrows.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15.0!))
        Me.tlpArrows.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpArrows.Controls.Add(Me.bnUp, 0, 0)
        Me.tlpArrows.Controls.Add(Me.bnLeft, 2, 0)
        Me.tlpArrows.Controls.Add(Me.bnRight, 2, 2)
        Me.tlpArrows.Controls.Add(Me.bnDown, 0, 2)
        Me.tlpArrows.Location = New System.Drawing.Point(0, 380)
        Me.tlpArrows.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpArrows.Name = "tlpArrows"
        Me.tlpArrows.RowCount = 3
        Me.tlpArrows.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpArrows.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15.0!))
        Me.tlpArrows.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpArrows.Size = New System.Drawing.Size(240, 155)
        Me.tlpArrows.TabIndex = 21
        '
        'pnListBox
        '
        Me.pnListBox.AutoSize = True
        Me.pnListBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnListBox.Controls.Add(Me.lbMain)
        Me.pnListBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnListBox.Location = New System.Drawing.Point(15, 15)
        Me.pnListBox.Margin = New System.Windows.Forms.Padding(15, 15, 0, 0)
        Me.pnListBox.Name = "pnListBox"
        Me.pnListBox.Size = New System.Drawing.Size(825, 820)
        Me.pnListBox.TabIndex = 24
        '
        'flpRight
        '
        Me.flpRight.AutoSize = True
        Me.flpRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpRight.Controls.Add(Me.bnAdd)
        Me.flpRight.Controls.Add(Me.bnRemove)
        Me.flpRight.Controls.Add(Me.bnEdit)
        Me.flpRight.Controls.Add(Me.bnRename)
        Me.flpRight.Controls.Add(Me.tlpArrows)
        Me.flpRight.Controls.Add(Me.bnClone)
        Me.flpRight.Controls.Add(Me.bnLoad)
        Me.flpRight.Controls.Add(Me.bnRestore)
        Me.flpRight.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpRight.Location = New System.Drawing.Point(855, 15)
        Me.flpRight.Margin = New System.Windows.Forms.Padding(15, 15, 15, 0)
        Me.flpRight.Name = "flpRight"
        Me.flpRight.Size = New System.Drawing.Size(240, 820)
        Me.flpRight.TabIndex = 24
        '
        'tlpMain
        '
        Me.tlpMain.AutoSize = True
        Me.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpMain.ColumnCount = 3
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.pnListBox, 0, 0)
        Me.tlpMain.Controls.Add(Me.flpRight, 1, 0)
        Me.tlpMain.Controls.Add(Me.bnCancel, 1, 1)
        Me.tlpMain.Controls.Add(Me.bnOK, 0, 1)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Size = New System.Drawing.Size(1177, 1012)
        Me.tlpMain.TabIndex = 25
        '
        'ProfilesForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1177, 1012)
        Me.Controls.Add(Me.tlpMain)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(9)
        Me.Name = "ProfilesForm"
        Me.Text = $"Profiles - {g.DefaultCommands.GetApplicationDetails()}"
        Me.tlpArrows.ResumeLayout(False)
        Me.pnListBox.ResumeLayout(False)
        Me.flpRight.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Profiles As IList
    Private AddProfileMethod As Func(Of Profile)
    Private LoadProfileMethod As Action(Of Profile)
    Private TextValues As New Dictionary(Of Button, String)
    Private DefaultsFunc As Func(Of IList)

    Sub New(title As String,
            profiles As IList,
            loadAction As Action(Of Profile),
            addFunc As Func(Of Profile),
            defaultsFunc As Func(Of IList))

        MyBase.New()
        InitializeComponent()
        Text = $"{title} - {g.DefaultCommands.GetApplicationDetails()}"

        Me.Profiles = profiles
        Me.DefaultsFunc = defaultsFunc
        LoadProfileMethod = loadAction
        AddProfileMethod = addFunc

        For Each i As Profile In profiles
            lbMain.Items.Add(i.GetCopy)
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

        ApplyTheme()

        AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        RemoveHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
        components?.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    Sub OnThemeChanged(theme As Theme)
        ApplyTheme(theme)
    End Sub

    Sub ApplyTheme()
        ApplyTheme(ThemeManager.CurrentTheme)
    End Sub

    Sub ApplyTheme(theme As Theme)
        If DesignHelp.IsDesignMode Then
            Exit Sub
        End If

        BackColor = theme.General.BackColor
    End Sub

    Sub UpdateControls()
        Dim pro = DirectCast(lbMain.SelectedItem, Profile)
        Dim count = lbMain.SelectedItems.Count
        bnClone.Enabled = count = 1
        bnEdit.Enabled = count = 1 AndAlso pro.CanEdit
        bnLoad.Enabled = count = 1 AndAlso Not LoadProfileMethod Is Nothing
        bnRemove.Enabled = count > 0
        bnRename.Enabled = count = 1
        bnUp.Enabled = lbMain.CanMoveUp()
        bnDown.Enabled = lbMain.CanMoveDown()
        bnLeft.Enabled = count > 0 AndAlso lbMain.Text.Contains(" | ")
        bnRight.Enabled = count > 0
        bnRestore.Enabled = Not DefaultsFunc Is Nothing
    End Sub

    Sub lbMain_KeyDown(sender As Object, e As KeyEventArgs) Handles lbMain.KeyDown
        Select Case e.KeyData
            Case Keys.Delete
                If bnRemove.Enabled Then
                    bnRemove.PerformClick()
                End If
            Case Keys.F2
                If bnRename.Enabled Then
                    bnRename.PerformClick()
                End If
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

    Sub lbMain_SelectedIndexChanged() Handles lbMain.SelectedIndexChanged
        UpdateControls()
    End Sub

    Sub bnLoad_Click() Handles bnLoad.Click
        LoadProfileMethod(DirectCast(lbMain.SelectedItem, Profile))
        TextValues(bnLoad) = bnLoad.Text
        bnLoad.ShowBold()
    End Sub

    Sub lbMain_DoubleClick() Handles lbMain.DoubleClick
        If Not lbMain.SelectedItem Is Nothing AndAlso DirectCast(lbMain.SelectedItem, Profile).CanEdit Then
            bnEdit.PerformClick()
        End If
    End Sub

    Sub bnAdd_Click() Handles bnAdd.Click
        Dim pm = AddProfileMethod()

        If Not pm Is Nothing Then
            pm = ObjectHelp.GetCopy(pm)
            pm.Name = InputBox.Show("Enter the name of the new profile", pm.Name)

            If Not pm.Name Is Nothing Then
                Dim remove As Profile = Nothing

                For Each i As Profile In lbMain.Items
                    If i.Name = pm.Name Then
                        If MsgOK("There is already a profile with this name, overwrite?") Then
                            remove = i
                        Else
                            Exit Sub
                        End If
                    End If
                Next

                If Not remove Is Nothing Then
                    lbMain.Items.Remove(remove)
                End If

                pm.Clean()
                lbMain.Items.Insert(0, pm)
                lbMain.ClearSelected()
                lbMain.SelectedItem = pm
                UpdateControls()
            End If
        End If
    End Sub

    Sub bnRemove_Click() Handles bnRemove.Click
        lbMain.RemoveSelection()
        UpdateControls()
    End Sub

    Sub bnRename_Click() Handles bnRename.Click
        Dim profile = DirectCast(lbMain.SelectedItem, Profile)
        Dim ret = InputBox.Show("Please enter a name to rename the profile", profile.Name)

        If Not ret Is Nothing Then
            profile.Name = ret
            lbMain.UpdateSelection()
            UpdateControls()
        End If
    End Sub

    Sub bnEdit_Click() Handles bnEdit.Click
        Dim profile = DirectCast(ObjectHelp.GetCopy(lbMain.SelectedItem), Profile)

        If profile.Edit = DialogResult.OK Then
            lbMain.Items(lbMain.SelectedIndex) = profile
            UpdateControls()
        End If
    End Sub

    Sub bnClone_Click() Handles bnClone.Click
        lbMain.Items.Insert(lbMain.SelectedIndex, ObjectHelp.GetCopy(lbMain.SelectedItem))
    End Sub

    Sub bnUp_Click(sender As Object, e As EventArgs) Handles bnUp.Click
        lbMain.MoveSelectionUp()
        UpdateControls()
    End Sub

    Sub bnDown_Click(sender As Object, e As EventArgs) Handles bnDown.Click
        lbMain.MoveSelectionDown()
        UpdateControls()
    End Sub

    Sub bnLeft_Click(sender As Object, e As EventArgs) Handles bnLeft.Click
        lbMain.SaveSelection()

        For x = 0 To lbMain.Items.Count - 1
            If lbMain.GetSelected(x) Then
                Dim p = DirectCast(lbMain.Items(x), Profile)

                If p.Name.Contains(" | ") Then
                    p.Name = p.Name.Right(" | ")
                    lbMain.Items(x) = lbMain.Items(x)
                End If
            End If
        Next

        lbMain.RestoreSelection()
    End Sub

    Sub bnRightRight_Click(sender As Object, e As EventArgs) Handles bnRight.Click
        Dim inputName = InputBox.Show("Enter a name for a sub menu")

        If inputName <> "" Then
            lbMain.SaveSelection()

            For x = 0 To lbMain.Items.Count - 1
                If lbMain.GetSelected(x) Then
                    Dim p = DirectCast(lbMain.Items(x), Profile)
                    p.Name = inputName + " | " + p.Name
                    lbMain.Items(x) = lbMain.Items(x)
                End If
            Next

            lbMain.RestoreSelection()
        End If
    End Sub

    Sub bnRestore_Click(sender As Object, e As EventArgs) Handles bnRestore.Click
        Using td = New TaskDialog(Of String)()
            td.Title = "How to restore?"
            td.AddCommand("Clean Restore", "Clean")
            td.AddCommand("Overwrite current profiles with default profiles", "Current profiles will be overwritten by the defaults, profiles with different name remain.", "Overwrite")
            td.AddButton("Cancel", "Cancel")
            td.Icon = TaskIcon.Question

            Select Case td.Show
                Case "Clean"
                    lbMain.Items.Clear()
                    lbMain.Items.AddRange(DefaultsFunc().OfType(Of Profile).ToArray())
                Case "Overwrite"
                    Dim uniqueProfiles = lbMain.Items.OfType(Of Profile).Where(Function(x) Not DefaultsFunc().OfType(Of Profile).Any(Function(y) x.Name = y.Name)).ToArray()
                    lbMain.Items.Clear()
                    lbMain.Items.AddRange(uniqueProfiles)
                    lbMain.Items.AddRange(DefaultsFunc().OfType(Of Profile).ToArray())
            End Select
        End Using

        UpdateControls()
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)

        If DialogResult = DialogResult.OK Then
            Profiles.Clear()

            For Each i As Profile In lbMain.Items
                Profiles.Add(i)
            Next

            g.SaveSettings()
        End If
    End Sub

    Sub ProfilesForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        Dim form As New HelpForm()
        form.Doc.WriteStart(Text)
        form.Doc.WriteTips(TipProvider.GetTips)
        form.Show()
    End Sub

    Sub bnOK_Click(sender As Object, e As EventArgs) Handles bnOK.Click
        DialogResult = DialogResult.OK
    End Sub
End Class