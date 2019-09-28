Imports StaxRip.UI

Public Class FiltersListView
    Inherits ListViewEx

    Private BlockItemCheck As Boolean
    WithEvents Menu As New ContextMenuStripEx
    Property IsLoading As Boolean

    Event Changed()

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
        Menu.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)
        ContextMenuStrip = Menu
        SendMessageHideFocus()
        AddHandler VideoScript.Changed, Sub(script As VideoScript)
                                            If Not p.Script Is Nothing AndAlso script Is p.Script Then
                                                OnChanged()
                                            End If
                                        End Sub
    End Sub

    Sub Load()
        If p.Script.Engine = ScriptEngine.AviSynth Then
            g.MainForm.lgbFilters.Text = "AVS Filters"
        Else
            g.MainForm.lgbFilters.Text = "VS Filters"
        End If

        BlockItemCheck = True
        Items.Clear()
        BeginUpdate()

        For Each i In p.Script.Filters
            Dim item As New ListViewItem
            item.Tag = i
            item.Checked = i.Active
            item.SubItems.Add(i.Category)
            If i.Name = "" Then item.SubItems.Add(i.Script) Else item.SubItems.Add(i.Name)
            item.SubItems.Add(i.Script)
            Items.Add(item)
        Next

        AutoResizeColumns(True)
        EndUpdate()
        BlockItemCheck = False
    End Sub

    Sub RebuildMenu()
        Menu.Items.ClearAndDisplose
        Dim filterProfiles = If(p.Script.Engine = ScriptEngine.AviSynth, s.AviSynthProfiles, s.VapourSynthProfiles)
        Dim selectedFunc = Function() SelectedItems.Count > 0
        Menu.Add("active").VisibleFunc = selectedFunc
        Dim sep0 = New ToolStripSeparator
        Menu.Items.Add(sep0)

        Dim replaceMenuItem = Menu.Add("Replace")
        replaceMenuItem.SetImage(Symbol.Switch)
        replaceMenuItem.VisibleFunc = selectedFunc

        For Each i In filterProfiles
            For Each i2 In i.Filters
                ActionMenuItem.Add(replaceMenuItem.DropDownItems, i.Name + " | " + i2.Path, AddressOf ReplaceClick, i2, i2.Script)
            Next
        Next

        Dim insertMenuItem = Menu.Add("Insert")
        insertMenuItem.SetImage(Symbol.LeftArrowKeyTime0)
        insertMenuItem.VisibleFunc = selectedFunc

        For Each i In filterProfiles
            For Each i2 In i.Filters
                ActionMenuItem.Add(insertMenuItem.DropDownItems, i.Name + " | " + i2.Path, AddressOf InsertClick, i2, i2.Script)
            Next
        Next

        Dim add = Menu.Add("Add")
        add.SetImage(Symbol.Add)

        For Each i In filterProfiles
            For Each i2 In i.Filters
                ActionMenuItem.Add(add.DropDownItems, i.Name + " | " + i2.Path, AddressOf AddClick, i2, i2.Script)
            Next
        Next

        Menu.Add("-")
        Dim remove = Menu.Add("Remove", AddressOf RemoveClick, "Removes the selected filter.")
        remove.SetImage(Symbol.Remove)
        remove.EnabledFunc = selectedFunc

        Menu.Add("Edit Code...", AddressOf ShowEditor, "Dialog to edit filters.").SetImage(Symbol.Code)
        Menu.Add("Preview Code...", Sub() g.CodePreview(p.Script.GetFullScript), "Script code preview.")
        Menu.Add("Play", Sub() g.PlayScript(p.Script), Function() p.SourceFile <> "", "Plays the script with the AVI player.").SetImage(Symbol.Play)
        Menu.Add("Profiles...", AddressOf g.MainForm.ShowFilterProfilesDialog, "Dialog to edit profiles.").SetImage(Symbol.FavoriteStar)

        Menu.Add("-")

        Dim moveUpItem = Menu.Add("Move Up", AddressOf MoveUp, "Moves the selected item up.")
        moveUpItem.SetImage(Symbol.Up)
        moveUpItem.EnabledFunc = Function() SelectedItems.Count > 0 AndAlso SelectedItems(0).Index > 0

        Dim moveDownItem = Menu.Add("Move Down", AddressOf MoveDown, "Moves the selected item down.")
        moveDownItem.SetImage(Symbol.Down)
        moveDownItem.EnabledFunc = Function() SelectedItems.Count > 0 AndAlso SelectedItems(0).Index < Items.Count - 1

        Menu.Add("-")
        Dim setup = Menu.Add("Filter Setup")
        setup.SetImage(Symbol.MultiSelect)
        g.PopulateProfileMenu(setup.DropDownItems, s.FilterSetupProfiles, AddressOf g.MainForm.ShowFilterSetupProfilesDialog, AddressOf g.MainForm.LoadFilterSetup)

        AddHandler Menu.Opening, Sub()
                                     Dim active = DirectCast(Menu.Items(0), ActionMenuItem)
                                     active.DropDownItems.ClearAndDisplose
                                     sep0.Visible = SelectedItems.Count > 0
                                     If SelectedItems.Count = 0 Then Exit Sub
                                     Dim selectedFilter = DirectCast(SelectedItems(0).Tag, VideoFilter)
                                     active.Text = selectedFilter.Category

                                     For Each i In filterProfiles
                                         If i.Name = selectedFilter.Category Then
                                             For Each i2 In i.Filters
                                                 ActionMenuItem.Add(active.DropDownItems, i2.Path, AddressOf ReplaceClick, i2.GetCopy, i2.Script)
                                             Next
                                         End If
                                     Next
                                 End Sub
    End Sub

    Sub MoveUp()
        If SelectedItems.Count = 0 Then Exit Sub
        Dim index = SelectedItems(0).Index
        If index = 0 Then Exit Sub
        Dim sel = p.Script.Filters(index)
        p.Script.Filters.Remove(sel)
        p.Script.Filters.Insert(index - 1, sel)
        Load()
        g.MainForm.Assistant()
    End Sub

    Sub MoveDown()
        If SelectedItems.Count = 0 Then Exit Sub
        Dim index = SelectedItems(0).Index
        If index = p.Script.Filters.Count - 1 Then Exit Sub
        Dim sel = p.Script.Filters(index)
        p.Script.Filters.Remove(sel)
        p.Script.Filters.Insert(index + 1, sel)
        Load()
        g.MainForm.Assistant()
    End Sub

    Sub ShowEditor()
        If p.Script.Edit = DialogResult.OK Then OnChanged()
    End Sub

    Sub ReplaceClick(filter As VideoFilter)
        filter = filter.GetCopy
        Dim val = Macro.ExpandGUI(filter.Script)
        If val.Cancel Then Exit Sub

        If val.Value <> filter.Script AndAlso val.Caption <> "" Then
            Dim path = filter.Path.Replace("...", "")

            If val.Caption.EndsWith(path) Then
                filter.Path = val.Caption
            Else
                filter.Path = path + " " + val.Caption
            End If
        End If

        filter.Script = val.Value
        Dim index = SelectedItems(0).Index
        p.Script.SetFilter(index, filter)
        Items(index).Selected = True
    End Sub

    Private Sub InsertClick(filter As VideoFilter)
        filter = filter.GetCopy
        Dim val = Macro.ExpandGUI(filter.Script)
        If val.Cancel Then Exit Sub

        If val.Value <> filter.Script AndAlso val.Caption <> "" Then
            Dim path = filter.Path.Replace("...", "")

            If val.Caption.EndsWith(path) Then
                filter.Path = val.Caption
            Else
                filter.Path = path + " " + val.Caption
            End If
        End If

        filter.Script = val.Value
        Dim index = SelectedItems(0).Index
        p.Script.InsertFilter(index, filter)
        Items(index).Selected = True
    End Sub

    Private Sub AddClick(filter As VideoFilter)
        filter = filter.GetCopy
        Dim val = Macro.ExpandGUI(filter.Script)
        If val.Cancel Then Exit Sub

        If val.Value <> filter.Script AndAlso val.Caption <> "" Then
            Dim path = filter.Path.Replace("...", "")

            If val.Caption.EndsWith(path) Then
                filter.Path = val.Caption
            Else
                filter.Path = path + " " + val.Caption
            End If
        End If

        filter.Script = val.Value
        p.Script.AddFilter(filter)
        Items(Items.Count - 1).Selected = True
    End Sub

    Sub OnChanged()
        If IsLoading Then Exit Sub
        Load()
        RaiseEvent Changed()
    End Sub

    Sub RaiseChangedAsync()
        Dim async = Sub()
                        Application.DoEvents()
                        OnChanged()
                    End Sub

        g.MainForm.BeginInvoke(async)
    End Sub

    Private Sub RemoveClick()
        If Items.Count > 1 Then p.Script.RemoveFilterAt(SelectedItems(0).Index)
    End Sub

    Sub UpdateDocument()
        p.Script.Filters.Clear()

        For Each i As ListViewItem In Items
            p.Script.Filters.Add(DirectCast(i.Tag, VideoFilter))
        Next

        OnChanged()
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        AutoResizeColumns(True)
    End Sub

    Protected Overrides Sub OnLayout(e As LayoutEventArgs)
        MyBase.OnLayout(e)
        AutoResizeColumns(True)
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
End Class