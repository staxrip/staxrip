Imports System.ComponentModel

Imports StaxRip.UI

Class AviSynthListView
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
    Property ProfileFunc As Func(Of VideoScript)

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
        Dim filterProfiles As List(Of FilterCategory)

        If p.Script.Engine = ScriptingEngine.AviSynth Then
            filterProfiles = s.AviSynthProfiles
        Else
            filterProfiles = s.VapourSynthProfiles
        End If

        Menu.Items.Clear()

        If SelectedItems.Count > 0 Then
            Dim selectedFilter = DirectCast(SelectedItems(0).Tag, VideoFilter)

            For Each i In filterProfiles
                If i.Name = selectedFilter.Category Then
                    For Each i2 In i.Filters
                        Dim tip = i2.Script
                        If Not tip.Contains("%app:") Then tip = Macro.Solve(tip)
                        ActionMenuItem.Add(Menu.Items, i2.Category + " | " + i2.Path, AddressOf ReplaceClick, i2.GetCopy, tip)
                    Next
                End If
            Next

            If Menu.Items.Count > 0 Then Menu.Items.Add(New ToolStripSeparator)

            Dim replace As New MenuItemEx("Replace")
            Dim insert As New MenuItemEx("Insert")

            Menu.Items.Add(replace)
            Menu.Items.Add(insert)

            For Each i In filterProfiles
                For Each i2 In i.Filters
                    Dim tip = i2.Script
                    If Not tip.Contains("%app:") Then tip = Macro.Solve(tip)
                    ActionMenuItem.Add(replace.DropDownItems, i.Name + " | " + i2.Path, AddressOf ReplaceClick, i2.GetCopy, tip)
                    ActionMenuItem.Add(insert.DropDownItems, i.Name + " | " + i2.Path, AddressOf InsertClick, i2.GetCopy, tip)
                Next
            Next
        End If

        Dim add As New MenuItemEx("Add")
        Menu.Items.Add(add)

        For Each i In filterProfiles
            For Each i2 In i.Filters
                Dim tip = i2.Script
                If Not tip.Contains("%app:") Then tip = Macro.Solve(tip)
                ActionMenuItem.Add(add.DropDownItems, i.Name + " | " + i2.Path, AddressOf AddClick, i2.GetCopy, tip)
            Next
        Next

        If SelectedItems.Count > 0 Then
            Menu.Items.Add(New ToolStripSeparator)
            Menu.Items.Add(New ActionMenuItem("Remove", AddressOf RemoveClick, "Removes the selected filter."))
        End If

        Menu.Items.Add(New ActionMenuItem("Edit...", AddressOf ShowEditor, "Dialog to edit filters."))
        Menu.Items.Add(New ActionMenuItem("Play", Sub() g.PlayScript(p.Script), "Plays the script with the AVI player.", p.SourceFile <> ""))
        Menu.Items.Add(New ActionMenuItem("Profiles...", AddressOf g.MainForm.OpenFilterProfilesDialog, "Dialog to edit profiles."))
        Menu.Items.Add(New ActionMenuItem("Code Preview...", AddressOf CodePreview, "Script code preview."))

        Dim setup As New MenuItemEx("Filter Setup")
        Menu.Items.Add(setup)
        g.PopulateProfileMenu(setup.DropDownItems, s.FilterSetupProfiles, AddressOf g.MainForm.OpenAviSynthProfilesDialog, AddressOf g.MainForm.LoadScriptProfile)
    End Sub

    Sub ShowEditor()
        If ProfileFunc().Invoke.Edit = DialogResult.OK Then
            Load()
            RaiseScriptChanged()
        End If
    End Sub

    Sub ReplaceClick(f As VideoFilter)
        Dim index = SelectedItems(0).Index
        ProfileFunc.Invoke.Filters(index) = f
        Load()
        Items(index).Selected = True
        RaiseChangedAsync()
    End Sub

    Private Sub InsertClick(f As VideoFilter)
        Dim index = SelectedItems(0).Index
        ProfileFunc.Invoke.Filters.Insert(index, f)
        Load()
        Items(index).Selected = True
        RaiseChangedAsync()
    End Sub

    Private Sub AddClick(f As VideoFilter)
        ProfileFunc.Invoke.Filters.Add(f)
        Load()
        Items(Items.Count - 1).Selected = True
        RaiseChangedAsync()
    End Sub

    Sub RaiseScriptChanged()
        RaiseEvent ScriptChanged()
    End Sub

    Private Sub RemoveClick()
        If Items.Count > 1 Then
            Dim index = SelectedItems(0).Index
            ProfileFunc.Invoke.Filters.RemoveAt(SelectedItems(0).Index)
            Load()
            RaiseChangedAsync()
        End If
    End Sub

    Sub UpdateDocument()
        ProfileFunc.Invoke.Filters.Clear()

        For Each i As ListViewItem In Items
            ProfileFunc.Invoke.Filters.Add(DirectCast(i.Tag, VideoFilter))
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
            Dim filter = DirectCast(Items(e.Index).Tag, VideoFilter)

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

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        If e.KeyData = Keys.Delete Then RemoveClick()
        MyBase.OnKeyDown(e)
    End Sub

    Sub CodePreview()
        Using f As New StringEditorForm
            f.tb.ReadOnly = True
            f.cbWrap.Checked = False
            f.cbWrap.Visible = False
            f.tb.Text = p.Script.GetFullScript
            f.tb.SelectionStart = f.tb.Text.Length
            f.tb.SelectionLength = 0
            f.Text = "Script Preview"
            f.Width = 800
            f.Height = 500
            f.bOK.Visible = False
            f.bCancel.Text = "Close"
            f.ShowDialog()
        End Using
    End Sub
End Class