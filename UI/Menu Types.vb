Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Reflection
Imports System.Text
Imports System.Drawing.Drawing2D

Imports StaxRip.UI

Namespace UI
    <Serializable()>
    Class CustomMenuItem
        Sub New()
        End Sub

        Sub New(text As String)
            Me.Text = text
        End Sub

        <NonSerialized()>
        Public CustomMenu As CustomMenu

        Overridable Property Text As String

        Property SubItems As New List(Of CustomMenuItem)
        Property KeyData As Keys
        Property MethodName As String

        Private ParametersValue As List(Of Object)

        Property Parameters() As List(Of Object)
            Get
                If ParametersValue Is Nothing Then
                    ParametersValue = New List(Of Object)
                End If

                Return ParametersValue
            End Get
            Set(Value As List(Of Object))
                ParametersValue = Value
            End Set
        End Property

        Sub Add(path As String)
            Add(path, Nothing, Keys.None, Nothing)
        End Sub

        Sub Add(path As String, methodName As String)
            Add(path, methodName, Keys.None, Nothing)
        End Sub

        Sub Add(path As String, methodName As String, ParamArray params As Object())
            Add(path, methodName, Keys.None, params)
        End Sub

        Sub Add(path As String, methodName As String, keyData As Keys, ParamArray params As Object())
            Add(path.SplitNoEmpty("|"), methodName, keyData, params)
        End Sub

        Private Sub Add(pathArray As String(), methodName As String, keyData As Keys, ParamArray params As Object())
            Dim l As List(Of CustomMenuItem) = SubItems

            For i = 0 To pathArray.Length - 1
                Dim found As Boolean = False

                For Each iItem In l
                    If i < pathArray.Length - 1 Then
                        If iItem.Text = pathArray(i) Then
                            found = True
                            l = iItem.SubItems
                        End If
                    End If
                Next

                If Not found Then
                    Dim item As New CustomMenuItem(pathArray(i))
                    l.Add(item)
                    l = item.SubItems

                    If i = pathArray.Length - 1 Then
                        item.MethodName = methodName
                        item.KeyData = keyData

                        If Not params Is Nothing Then
                            item.Parameters.AddRange(params)
                        End If
                    End If
                End If
            Next
        End Sub

        <NonSerialized()>
        Public Parent As CustomMenuItem

        Shared Sub SetParents(item As CustomMenuItem)
            For Each i In item.SubItems
                i.Parent = item
                SetParents(i)
            Next
        End Sub

        Sub Remove()
            Parent.SubItems.Remove(Me)
        End Sub

        Function GetAllItems() As List(Of CustomMenuItem)
            Dim l As New List(Of CustomMenuItem)
            AddToList(Me, l)
            Return l
        End Function

        Private Sub AddToList(item As CustomMenuItem, list As List(Of CustomMenuItem))
            For Each i In item.SubItems
                list.Add(i)
                AddToList(i, list)
            Next
        End Sub

        Function GetClone() As CustomMenuItem
            Return DirectCast(ObjectHelp.GetCopy(Me), CustomMenuItem)
        End Function
    End Class

    Class CustomMenu
        Private Items As New List(Of CustomMenuItem)

        Property Menu As Menu
        Property MenuStrip As MenuStrip
        Property ToolStrip As ToolStrip
        Property MenuItems As New List(Of MenuItemEx)
        Property DefaultMenu As Func(Of CustomMenuItem)
        Property MenuItem As CustomMenuItem
        Property CommandManager As CommandManager

        Event Command(e As CustomMenuItemEventArgs)

        Sub New(defaultMenu As Func(Of CustomMenuItem),
                menuItem As CustomMenuItem,
                commandManager As CommandManager,
                toolStrip As ToolStrip)

            Me.CommandManager = commandManager
            Me.DefaultMenu = defaultMenu
            Me.MenuItem = menuItem
            Me.ToolStrip = toolStrip
        End Sub

        Sub AddKeyDownHandler(control As Control)
            AddHandler control.KeyDown, AddressOf OnKeyDown
        End Sub

        Function GetKeys() As StringPairList
            Dim ret As New StringPairList

            For Each i As MenuItemEx In MenuItems
                If i.ShortcutKeyDisplayString <> "" Then
                    Dim sp As New StringPair

                    If i.Text.EndsWith("...") Then
                        sp.Name = i.Text.TrimEnd("."c)
                    Else
                        sp.Name = i.Text
                    End If

                    sp.Value = i.ShortcutKeyDisplayString
                    ret.Add(sp)
                End If
            Next

            Return ret
        End Function

        Function GetTips() As StringPairList
            Dim ret As New StringPairList

            For Each i As MenuItemEx In MenuItems
                Dim help = i.GetHelp
                If Not help Is Nothing Then ret.Add(help)
            Next

            Return ret
        End Function

        Function Edit() As CustomMenuItem
            Using f As New CustomMenuEditor(Me)
                If f.ShowDialog = DialogResult.OK Then
                    MenuItem = f.GetState
                    BuildMenu()
                End If
            End Using

            Return MenuItem
        End Function

        Sub Check(methodName As String, checked As Boolean)
            For Each i In Me.MenuItems
                If Not i.CustomMenuItem Is Nothing Then
                    If i.CustomMenuItem.MethodName = methodName Then
                        i.Checked = checked
                    End If
                End If
            Next
        End Sub

        Sub OnKeyDown(sender As Object, e As KeyEventArgs)
            For Each i As CustomMenuItem In Items
                If i.KeyData = e.KeyData Then
                    OnCommand(i)
                    Exit For
                End If
            Next
        End Sub

        Sub MenuClick(sender As Object, e As EventArgs)
            If TypeOf sender Is MenuItemEx Then
                OnCommand(DirectCast(sender, MenuItemEx).CustomMenuItem)
            End If
        End Sub

        Private Sub OnCommand(item As CustomMenuItem)
            If item.MethodName <> "" Then
                Dim e As New CustomMenuItemEventArgs(item)
                RaiseEvent Command(e)
                If Not e.Handled Then Process(item)
                Dim f As Form = ToolStrip.FindForm
                If Not f Is Nothing Then f.Refresh()
            End If
        End Sub

        Sub Process(item As CustomMenuItem)
            CommandManager.Process(item.MethodName, item.Parameters)
        End Sub

        Sub BuildMenu()
            ToolStrip.Items.Clear()
            Items.Clear()
            MenuItems.Clear()
            BuildMenu(ToolStrip, MenuItem)
        End Sub

        Private Sub BuildMenu(menu As Object, item As CustomMenuItem)
            For Each i As CustomMenuItem In item.SubItems
                i.CustomMenu = Me
                Dim mi As ToolStripItem

                If i.Text = "-" Then
                    mi = New ToolStripSeparator
                Else
                    Dim emi As New MenuItemEx()
                    MenuItems.Add(emi)
                    mi = emi
                    emi.CustomMenuItem = i

                    Dim keys = KeysHelp.GetKeyString(i.KeyData)

                    If keys <> "" Then
                        emi.ShortcutKeyDisplayString = keys
                    End If

                    AddHandler mi.Click, AddressOf MenuClick
                End If

                Items.Add(i)
                mi.Text = i.Text

                If TypeOf menu Is ToolStripMenuItem Then
                    DirectCast(menu, ToolStripMenuItem).DropDownItems.Add(mi)
                ElseIf TypeOf menu Is ToolStrip Then
                    DirectCast(menu, ToolStrip).Items.Add(mi)
                End If

                BuildMenu(mi, i)
            Next
        End Sub
    End Class

    Class CustomMenuItemEventArgs
        Inherits EventArgs

        Property Handled As Boolean
        Property Item As CustomMenuItem

        Sub New(item As CustomMenuItem)
            Me.Item = item
        End Sub
    End Class

    Class MenuItemEx
        Inherits ToolStripMenuItem

        Shared Property UseTooltips As Boolean

        Sub New()
        End Sub

        Sub New(text As String)
            MyBase.New(text)
        End Sub

        Function GetHelp() As StringPair
            If Not CustomMenuItem Is Nothing AndAlso Not CustomMenuItem.CustomMenu Is Nothing AndAlso
                CustomMenuItem.CustomMenu.CommandManager.HasCommand(CustomMenuItem.MethodName) Then

                Dim command = CustomMenuItem.CustomMenu.CommandManager.GetCommand(CustomMenuItem.MethodName)

                If command.Attribute.Description <> "" Then
                    Dim ret As New StringPair

                    If Text.EndsWith("...") Then
                        ret.Name = Text.TrimEnd("."c)
                    Else
                        ret.Name = Text
                    End If

                    ret.Value = command.Attribute.Description
                    Dim paramHelp = command.GetParameterHelp(CustomMenuItem.Parameters)
                    If paramHelp <> "" Then ret.Value += " (" + paramHelp + ")"

                    Return ret
                End If
            End If
        End Function

        Private CustomMenuItemValue As CustomMenuItem

        <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property CustomMenuItem() As CustomMenuItem
            Get
                Return CustomMenuItemValue
            End Get
            Set(Value As CustomMenuItem)
                CustomMenuItemValue = Value

                If Not Value Is Nothing AndAlso
                    Not Value.CustomMenu Is Nothing AndAlso
                    Value.CustomMenu.CommandManager.HasCommand(CustomMenuItem.MethodName) Then

                    Dim c = CustomMenuItem.CustomMenu.CommandManager.GetCommand(CustomMenuItem.MethodName)

                    If c.MethodInfo.Name <> "DynamicMenuItem" Then
                        If c.MethodInfo.Name = "ExecuteCommandLine" Then
                            Help = CustomMenuItem.Parameters(0).ToString.Trim(""""c)
                        Else
                            Help = c.Attribute.Description
                        End If
                    End If
                End If
            End Set
        End Property

        Private Function ShouldSerializeHelpText() As Boolean
            Return HelpValue <> ""
        End Function

        Private HelpValue As String

        <Editor(GetType(StringEditor), GetType(UITypeEditor))>
        Property Help() As String
            Get
                Return HelpValue
            End Get
            Set(Value As String)
                HelpValue = Value

                If UseTooltips Then
                    If HelpValue <> "" Then
                        If HelpValue.Length < 80 Then
                            ToolTipText = HelpValue.TrimEnd("."c)
                        Else
                            ToolTipText = "Right-click for help"
                        End If
                    End If
                End If
            End Set
        End Property

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            If e.Button = MouseButtons.Right AndAlso Help <> "" Then
                CloseAll(Me)
                g.ShowHelp(Text, Help)
            End If

            MyBase.OnMouseDown(e)
        End Sub

        Sub CloseAll(item As Object)
            If TypeOf item Is ToolStripItem Then
                Dim d = DirectCast(item, ToolStripItem)
                CloseAll(d.Owner)
            End If

            If TypeOf item Is ToolStripDropDown Then
                Dim d = DirectCast(item, ToolStripDropDown)
                d.Close()
                CloseAll(d.OwnerItem)
            End If
        End Sub

        Protected Overrides Sub OnClick(e As EventArgs)
            Application.DoEvents()
            MyBase.OnClick(e)
        End Sub
    End Class

    Class ActionMenuItem
        Inherits MenuItemEx

        Private Action As Action

        Property EnabledFunc As Func(Of Boolean)
        Property Form As Form

        Sub New()
        End Sub

        Sub New(text As String, a As Action)
            Me.New(text, a, Nothing)
        End Sub

        Sub New(text As String,
                action As Action,
                Optional tooltip As String = Nothing,
                Optional enabled As Boolean = True)

            Me.Text = text
            Me.Action = action
            Me.Help = tooltip
            Me.Enabled = enabled
        End Sub

        Private ShortcutValue As Keys

        Property Shortcut As Keys
            Get
                Return ShortcutValue
            End Get
            Set(value As Keys)
                ShortcutValue = value
                ShortcutKeyDisplayString = KeysHelp.GetKeyString(value)
                AddHandler Form.KeyDown, AddressOf KeyDown
            End Set
        End Property

        Sub KeyDown(sender As Object, e As KeyEventArgs)
            If Enabled AndAlso e.KeyData = Shortcut AndAlso
                If(EnabledFunc Is Nothing, True, EnabledFunc.Invoke) Then

                PerformClick()
                e.Handled = True
            End If
        End Sub

        Sub Opening(sender As Object, e As CancelEventArgs)
            If Not EnabledFunc Is Nothing Then Enabled = EnabledFunc.Invoke
        End Sub

        Protected Overrides Sub OnClick(e As EventArgs)
            Application.DoEvents()
            If Not Action Is Nothing Then Action()
            MyBase.OnClick(e)
        End Sub

        Shared Function Add(Of T)(items As ToolStripItemCollection,
                                  path As String,
                                  action As Action(Of T),
                                  value As T,
                                  Optional help As String = Nothing) As ActionMenuItem

            Return Add(items, path, Sub() action(value), help)
        End Function

        Shared Function Add(items As ToolStripItemCollection,
                            path As String,
                            action As Action,
                            Optional tip As String = Nothing) As ActionMenuItem

            Dim a = path.SplitNoEmpty(" | ")
            Dim l = items

            For x = 0 To a.Length - 1
                Dim found = False

                For Each i In l.OfType(Of ToolStripMenuItem)()
                    If x < a.Length - 1 Then
                        If i.Text = a(x) Then
                            found = True
                            l = i.DropDownItems
                        End If
                    End If
                Next

                If Not found Then
                    If x = a.Length - 1 Then
                        If a(x) = "-" Then
                            l.Add(New ToolStripSeparator)
                        Else
                            Dim item As New ActionMenuItem(a(x), action, tip)
                            l.Add(item)
                            l = item.DropDownItems
                            Return item
                        End If
                    Else
                        Dim item As New ActionMenuItem()
                        item.Text = a(x)
                        l.Add(item)
                        l = item.DropDownItems
                    End If
                End If
            Next
        End Function
    End Class

    Class TextCustomMenu
        Shared Function EditMenu(value As String, owner As Form) As String
            Return EditMenu(value, Nothing, Nothing, owner)
        End Function

        Shared Function EditMenu(value As String,
                                 helpName As String,
                                 defaults As String,
                                 owner As Form) As String

            Using f As New MacroEditor
                f.SetMacroDefaults()
                f.MacroEditorControl.Value = value
                f.Text = "Menu Editor"
                Dim t = f

                Dim resetAction = Sub()
                                      If MsgOK("Restore defaults?") Then
                                          t.MacroEditorControl.Value = defaults
                                      End If
                                  End Sub

                If helpName <> "" Then
                    f.bnContext.Text = " Restore Defaults... "
                    f.bnContext.Visible = True
                    f.bnContext.AddClickAction(resetAction)
                End If

                If f.ShowDialog(owner) = DialogResult.OK Then
                    value = f.MacroEditorControl.Value
                End If
            End Using

            Return value
        End Function

        Shared Function GetMenu(definition As String,
                                owner As Control,
                                components As IContainer,
                                action As Action(Of String)) As ContextMenuStrip

            If owner.ContextMenuStrip Is Nothing Then
                owner.ContextMenuStrip = New ContextMenuStrip(components)
            End If

            Dim r = owner.ContextMenuStrip
            r.Items.Clear()

            For Each i In definition.SplitKeepEmpty(CrLf)
                If i.Contains("=") Then
                    Dim arg = i.Right("=").Trim
                    ActionMenuItem.Add(r.Items, i.Left("="), action, arg, Nothing)
                ElseIf i.EndsWith(" | -") Then
                    ActionMenuItem.Add(r.Items, i, Nothing, Nothing)
                ElseIf i = "" Then
                    r.Items.Add(New ToolStripSeparator)
                End If
            Next

            Return r
        End Function
    End Class

    Class ContextMenuStripEx
        Inherits ContextMenuStrip

        Private FormValue As Form

        Property Form As Form
            Get
                Return FormValue
            End Get
            Set(value As Form)
                AddHandler value.Disposed, Sub() Dispose()
                FormValue = value
            End Set
        End Property

        Sub Add(path As String)
            ActionMenuItem.Add(Items, path, Nothing)
        End Sub

        Function Add(path As String,
                     action As Action,
                     Optional help As String = Nothing) As ActionMenuItem

            Dim ret = ActionMenuItem.Add(Items, path, action)

            ret.Form = Form
            ret.Help = help

            AddHandler Opening, AddressOf ret.Opening

            Return ret
        End Function

        Function Add(path As String, action As action, shortcut As Keys,
                     enabledFunc As Func(Of Boolean),
                     Optional help As String = Nothing) As ActionMenuItem

            Dim ret = ActionMenuItem.Add(Items, path, action)

            ret.Form = Form
            ret.Shortcut = shortcut
            ret.EnabledFunc = enabledFunc
            ret.Help = help

            AddHandler Opening, AddressOf ret.Opening

            Return ret
        End Function

        Function GetTips() As StringPairList
            Dim ret As New StringPairList

            For Each i In GetItems.OfType(Of ActionMenuItem)()
                If i.Help <> "" Then
                    Dim pair As New StringPair

                    If i.Text.EndsWith("...") Then
                        pair.Name = i.Text.TrimEnd("."c)
                    Else
                        pair.Name = i.Text
                    End If

                    pair.Value = i.Help
                    ret.Add(pair)
                End If
            Next

            Return ret
        End Function

        Function GetKeys() As StringPairList
            Dim ret As New StringPairList

            For Each i In GetItems.OfType(Of ActionMenuItem)()
                If i.ShortcutKeyDisplayString <> "" Then
                    Dim sp As New StringPair

                    If i.Text.EndsWith("...") Then
                        sp.Name = i.Text.TrimEnd("."c)
                    Else
                        sp.Name = i.Text
                    End If

                    sp.Value = i.ShortcutKeyDisplayString
                    ret.Add(sp)
                End If
            Next

            Return ret
        End Function

        Function GetItems() As List(Of ToolStripItem)
            Dim ret As New List(Of ToolStripItem)
            AddItemsRecursive(Items, ret)
            Return ret
        End Function

        Shared Sub AddItemsRecursive(searchList As ToolStripItemCollection, returnList As List(Of ToolStripItem))
            For Each i As ToolStripItem In searchList
                returnList.Add(i)

                If TypeOf i Is ToolStripDropDownItem Then
                    AddItemsRecursive(DirectCast(i, ToolStripDropDownItem).DropDownItems, returnList)
                End If
            Next
        End Sub
    End Class
End Namespace