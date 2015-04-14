Imports System.ComponentModel

Imports StaxRip.UI
Imports Microsoft.Win32

Public Class AviSynthListView
    Inherits ListViewEx

    Private BlockItemCheck As Boolean
    WithEvents Menu As New ContextMenuStrip

    Event ScriptChanged()

    Sub New()
        AllowDrop = True
        Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        CheckBoxes = True
        View = View.Details
        HideSelection = False
        FullRowSelect = True
        MultiSelect = False
        HeaderStyle = ColumnHeaderStyle.None
        Columns.Add("")
        Columns.Add("Type")
        Columns.Add("Name")
        ContextMenuStrip = Menu

        SendMessageHideFocus()
    End Sub

    <Browsable(False),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property ProfileFunc As Func(Of AviSynthDocument)

    Sub Load()
        BlockItemCheck = True
        Items.Clear()

        BeginUpdate()

        For Each i In ProfileFunc.Invoke.Filters
            Dim item As New ListViewItem
            item.Tag = i
            item.Checked = i.Active
            item.SubItems.Add(i.Category)
            item.SubItems.Add(i.Name)
            item.SubItems.Add(i.Script)
            Items.Add(item)
        Next

        AutoResizeColumns(True)

        EndUpdate()

        BlockItemCheck = False
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        AutoResizeColumns(True)
    End Sub

    Protected Overrides Sub OnLayout(e As LayoutEventArgs)
        MyBase.OnLayout(e)
        AutoResizeColumns(True)
    End Sub

    Sub UpdateMenu()
        Menu.Items.Clear()

        If SelectedItems.Count > 0 Then
            Dim selectedFilter = DirectCast(SelectedItems(0).Tag, AviSynthFilter)

            For Each i In s.AviSynthCategories
                If i.Name = selectedFilter.Category Then
                    For Each i2 In i.Filters
                        Dim tip = i2.Script

                        If Not tip.Contains("%app:") Then
                            tip = Macro.Solve(tip)
                        End If

                        ActionMenuItem.Add(Menu.Items,
                            i2.Path, AddressOf ReplaceClick, i2.GetCopy, tip)
                    Next
                End If
            Next

            If Menu.Items.Count > 0 Then Menu.Items.Add(New ToolStripSeparator)

            Dim replace As New MenuItemEx("Replace")
            Dim insert As New MenuItemEx("Insert")

            Menu.Items.Add(replace)
            Menu.Items.Add(insert)

            For Each i In s.AviSynthCategories
                For Each i2 In i.Filters
                    Dim tip = i2.Script

                    If Not tip.Contains("%app:") Then
                        tip = Macro.Solve(tip)
                    End If

                    ActionMenuItem.Add(replace.DropDownItems,
                        i.Name + " | " + i2.Path, AddressOf ReplaceClick, i2.GetCopy, tip)

                    ActionMenuItem.Add(insert.DropDownItems,
                        i.Name + " | " + i2.Path, AddressOf InsertClick, i2.GetCopy, tip)
                Next
            Next
        End If

        If SelectedItems.Count = 0 Then
            Dim add As New MenuItemEx("Add")
            Menu.Items.Add(add)

            For Each i In s.AviSynthCategories
                For Each i2 In i.Filters
                    Dim tip = i2.Script

                    If Not tip.Contains("%app:") Then
                        tip = Macro.Solve(tip)
                    End If

                    ActionMenuItem.Add(add.DropDownItems,
                        i.Name + " | " + i2.Path, AddressOf AddClick, i2.GetCopy, tip)
                Next
            Next
        Else
            Menu.Items.Add(New ToolStripSeparator)
            Menu.Items.Add(New ActionMenuItem("Remove", AddressOf RemoveClick, "Removes the selected filter."))
        End If

        If Not Editable Then
            Menu.Items.Add(New ActionMenuItem("Edit...", AddressOf EditClick, "Dialog to edit filters."))
        End If

        If TypeOf FindForm() Is MainForm Then
            Menu.Items.Add(New ActionMenuItem("Play", AddressOf g.DefaultCommands.PlayScript, "Plays the script with the AVI player.", p.SourceFile <> ""))
        End If

        Menu.Items.Add(New ActionMenuItem("Profiles...", AddressOf g.MainForm.OpenAviSynthFilterProfilesDialog, "Dialog to edit profiles."))

        If TypeOf FindForm() Is MainForm Then
            Dim setup As New MenuItemEx("Filter Setup")
            Menu.Items.Add(setup)

            g.PopulateProfileMenu(setup.DropDownItems, s.AviSynthProfiles, AddressOf g.MainForm.OpenAviSynthProfilesDialog, AddressOf g.MainForm.LoadAviSynthProfile)
        End If

        Menu.Items.Add(New ToolStripSeparator)

        ActionMenuItem.Add(Menu.Items, "Help | AviSynth", Sub() ShowAviSynthHelp())
        ActionMenuItem.Add(Menu.Items, "Help | Scintilla", Sub() g.ShellExecute("http://www.aquilinestudios.org/avsfilters/index.html"))
        ActionMenuItem.Add(Menu.Items, "Help | A && E", Sub() g.ShellExecute("http://www.animemusicvideos.org/guides/avtech3/amvapp-avisynth.html"))
    End Sub

    Sub ShowAviSynthHelp()
        Dim installDir = Registry.LocalMachine.GetString("SOFTWARE\AviSynth", Nothing)
        g.ShellExecute(installDir + "\Docs\English\index.htm")
    End Sub

    Private Sub ReplaceClick(f As AviSynthFilter)
        Dim index = SelectedItems(0).Index
        ProfileFunc.Invoke.Filters(index) = f
        Load()
        Items(index).Selected = True
        RaiseChangedAsync()
    End Sub

    Private Sub InsertClick(f As AviSynthFilter)
        Dim index = SelectedItems(0).Index
        ProfileFunc.Invoke.Filters.Insert(index, f)
        Load()
        Items(index).Selected = True
        RaiseChangedAsync()
    End Sub

    Private Sub AddClick(f As AviSynthFilter)
        ProfileFunc.Invoke.Filters.Add(f)
        Load()
        Items(Items.Count - 1).Selected = True
        RaiseChangedAsync()
    End Sub

    Sub RaiseScriptChanged()
        RaiseEvent ScriptChanged()
    End Sub

    Sub EditClick()
        EditClick(AddressOf g.MainForm.GetTargetAviSynthDocument)
    End Sub

    Shared Sub EditClick(docDelegate As Func(Of AviSynthDocument))
        Using f As New ControlHostForm("Filters")
            f.FormBorderStyle = FormBorderStyle.Sizable
            f.Width = 900
            f.Height = 500

            Dim c As New AviSynthListView
            c.SmallImageList = New ImageList() With {.ImageSize = New Size(18, 18)}
            c.Editable = True
            c.Dock = DockStyle.Fill
            c.ProfileFunc = docDelegate
            c.Columns(1).Width += 20
            c.Columns.Add("Script")
            c.HeaderStyle = ColumnHeaderStyle.Nonclickable
            c.DoubleClickDoesCheck = False
            Dim categories = From a In s.AviSynthCategories Select a.Name
            Dim cb = c.AddComboBox(categories.ToArray, 1)
            cb.DropDownStyle = ComboBoxStyle.DropDownList
            cb.Sorted = True
            c.AddTextBox(2)
            c.AddTextBox(3)
            c.Load()
            f.AddControl(c, AddressOf ShowHelp)
            f.ShowDialog()
            g.MainForm.AviSynthListView.Load()
            g.MainForm.AviSynthListView.RaiseScriptChanged()
        End Using
    End Sub

    Protected Overrides Sub OnBeforeShowControl(e As BeforeShowControlEventArgs)
        MyBase.OnBeforeShowControl(e)

        If Not e.Cancel AndAlso e.Position.X = 1 Then
            Dim cb = DirectCast(e.Control, ComboBox)
            cb.Items.Clear()

            Dim categories As New List(Of String)

            For Each i In s.AviSynthCategories
                categories.Add(i.Name)
            Next

            cb.Items.AddRange(categories.ToArray)
        End If
    End Sub

    Protected Overrides Sub OnEdited(value As Object, pos As Point)
        Dim f = DirectCast(Items(pos.Y).Tag, AviSynthFilter)

        Select Case pos.X
            Case 1
                f.Category = value.ToString
            Case 2
                f.Path = value.ToString
            Case 3
                f.Script = value.ToString
        End Select

        MyBase.OnEdited(value, pos)
    End Sub

    Private Sub RemoveClick()
        If SelectedItems.Count > 0 Then
            Dim index = SelectedItems(0).Index
            ProfileFunc.Invoke.Filters.RemoveAt(SelectedItems(0).Index)
            Load()
            RaiseChangedAsync()
        End If
    End Sub

    Sub UpdateDocument()
        ProfileFunc.Invoke.Filters.Clear()

        For Each i As ListViewItem In Items
            ProfileFunc.Invoke.Filters.Add(DirectCast(i.Tag, AviSynthFilter))
        Next

        RaiseEvent ScriptChanged()
    End Sub

    Protected Overrides Sub OnDragDrop(e As DragEventArgs)
        BlockItemCheck = True
        MyBase.OnDragDrop(e)
        BlockItemCheck = False
        UpdateDocument()
    End Sub

    Protected Overrides Sub OnItemCheck(e As ItemCheckEventArgs)
        MyBase.OnItemCheck(e)

        If Not BlockItemCheck AndAlso Focused Then
            Dim filter = DirectCast(Items(e.Index).Tag, AviSynthFilter)

            If e.NewValue = CheckState.Checked AndAlso filter.Category = "Resize" Then
                Dim f = FindForm()

                If Not f Is Nothing AndAlso TypeOf f Is MainForm Then
                    g.MainForm.SetTargetImageSize(p.TargetWidth, 0)
                End If
            End If

            filter.Active = e.NewValue = CheckState.Checked
            RaiseChangedAsync()
        End If
    End Sub

    Sub RaiseChangedAsync()
        Dim async = Sub()
                        Application.DoEvents()
                        RaiseEvent ScriptChanged()
                    End Sub

        g.MainForm.BeginInvoke(async)
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        If e.Button = MouseButtons.Right Then
            UpdateMenu()
        End If

        MyBase.OnMouseUp(e)

        If Control.ModifierKeys = Keys.Control AndAlso SelectedItems.Count > 0 Then
            EditScript()
        End If
    End Sub

    Shared Sub ShowHelp()
        Dim f As New HelpForm()
        f.Doc.WriteStart("Filters")
        f.Doc.WriteP("The filters dialog allows to edit filters. Help about filters in general can be found at [http://www.avisynth.org www.avisynth.org].")
        f.Doc.WriteH2("Editing Features")

        f.Doc.WriteList(
            "Left-click a selected filter to edit the category, name or script",
            "Left-click while pressing Ctrl opens a script editor enabling multi line editing",
            "Right-click on a filter shows a context menu with profile related features",
            "The filter order can be changed using Drag & Drop",
            "The check boxes are used to apply or disable filters")

        f.Doc.WriteP("The mini filter editor in the main dialog has a similar but limited feature set. It supports the context menu, Drag & Drop to reorder, the script editor shows with a simple double-click.")
        f.Doc.WriteTable("Macros", Strings.MacrosHelp, Macro.GetTips())
        f.Show()
    End Sub

    Sub EditScript()
        If SelectedItems.Count > 0 Then
            Dim filter = DirectCast(SelectedItems(0).Tag, AviSynthFilter)

            Using f As New AviSynthScriptEditor(filter.Script)
                For Each i In Packs.Packages.Values.OfType(Of AviSynthPluginPackage)()
                    For Each i2 In i.FilterNames
                        If filter.Script.Contains(i2) Then
                            Dim path = i.GetHelpPath()

                            If path <> "" Then
                                f.MacroEditorControl.llHelp.Visible = True
                                f.MacroEditorControl.llHelp.Text = " " + i.Name + " Help "
                                AddHandler f.MacroEditorControl.llHelp.Click, Sub() g.ShellExecute(path)
                            End If
                        End If
                    Next
                Next

                If f.bnContext.Text = "" Then
                    Dim installDir = Registry.LocalMachine.GetString("SOFTWARE\AviSynth", Nothing)

                    f.bnContext.Text = filter.Script.Left("(")

                    If f.bnContext.Text.EndsWith("Resize") Then
                        f.bnContext.Text = "Resize"
                    End If

                    If f.bnContext.Text.StartsWith("ConvertTo") Then
                        f.bnContext.Text = "Convert"
                    End If

                    Dim cmdl = installDir + "\Docs\English\corefilters\" + f.bnContext.Text + ".htm"

                    If Not File.Exists(cmdl) Then
                        f.bnContext.Text = "AviSynth"
                        cmdl = installDir + "\Docs\English\index.htm"
                    End If

                    If Not File.Exists(cmdl) Then
                        f.bnContext.Text = "AviSynth"
                        cmdl = "http://www.avisynth.org"
                    End If

                    f.bnContext.Text = " " + f.bnContext.Text + " Help "
                    f.bnContext.Visible = True
                    f.bnContext.AddClickAction(Sub() g.ShellExecute(cmdl))
                End If

                If f.ShowDialog(FindForm) = DialogResult.OK AndAlso filter.Script <> f.MacroEditorControl.Value Then
                    filter.Script = f.MacroEditorControl.Value

                    If Columns.Count > 3 Then
                        SetText(New Point(3, SelectedItems(0).Index), filter.Script)
                    End If

                    RaiseEvent ScriptChanged()
                End If
            End Using
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        If e.KeyData = Keys.Delete Then
            RemoveClick()
        End If

        MyBase.OnKeyDown(e)
    End Sub
End Class