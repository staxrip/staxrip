
Imports System.ComponentModel
Imports System.Drawing.Design

Namespace UI
    <Serializable()>
    Public Class CustomMenuItem
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
        Property Symbol As Symbol
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
            Add(path, Nothing, Keys.None, Symbol.None, Nothing)
        End Sub

        Sub Add(path As String, symbol As Symbol)
            Add(path, Nothing, Keys.None, symbol, Nothing)
        End Sub

        Sub Add(path As String, methodName As String)
            Add(path, methodName, Keys.None, Symbol.None, Nothing)
        End Sub

        Sub Add(path As String, methodName As String, symbol As Symbol)
            Add(path, methodName, Keys.None, symbol, Nothing)
        End Sub

        Sub Add(path As String, methodName As String, symbol As Symbol, params As Object())
            Add(path, methodName, Keys.None, symbol, params)
        End Sub

        Sub Add(path As String, methodName As String, keyData As Keys)
            Add(path, methodName, keyData, Symbol.None, Nothing)
        End Sub

        Sub Add(path As String, methodName As String, keyData As Keys, symbol As Symbol)
            Add(path, methodName, keyData, symbol, Nothing)
        End Sub

        Sub Add(path As String, methodName As String, keyData As Keys, params As Object())
            Add(path, methodName, keyData, Symbol.None, params)
        End Sub

        Sub Add(path As String, methodName As String, params As Object())
            Add(path, methodName, Keys.None, params)
        End Sub

        Sub Add(path As String,
                methodName As String,
                keyData As Keys,
                symbol As Symbol,
                params As Object())

            Dim pathArray = path.SplitNoEmpty("|")
            Dim l = SubItems

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
                        item.Symbol = symbol

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

        Sub AddToList(item As CustomMenuItem, list As List(Of CustomMenuItem))
            For Each i In item.SubItems
                list.Add(i)
                AddToList(i, list)
            Next
        End Sub

        Function GetClone() As CustomMenuItem
            Return DirectCast(ObjectHelp.GetCopy(Me), CustomMenuItem)
        End Function
    End Class

    Public Class CustomMenu
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

                If Not help Is Nothing Then
                    ret.Add(help)
                End If
            Next

            Return ret
        End Function

        Function Edit() As CustomMenuItem
            Using form As New CustomMenuEditor(Me)
                If form.ShowDialog = DialogResult.OK Then
                    MenuItem = form.GetState
                    BuildMenu()
                End If
            End Using

            Return MenuItem
        End Function

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

        Sub OnCommand(item As CustomMenuItem)
            If item.MethodName <> "" Then
                Dim e As New CustomMenuItemEventArgs(item)
                RaiseEvent Command(e)

                If Not e.Handled Then
                    Process(item)
                End If

                Dim form = ToolStrip.FindForm

                If Not form Is Nothing Then
                    form.Refresh()
                End If
            End If
        End Sub

        Sub Process(item As CustomMenuItem)
            CommandManager.Process(item.MethodName, item.Parameters)
        End Sub

        Sub BuildMenu()
            ToolStrip.Items.ClearAndDisplose
            Items.Clear()
            MenuItems.Clear()
            Application.DoEvents()
            BuildMenu(ToolStrip, MenuItem)
        End Sub

        Sub BuildMenu(menu As Object, item As CustomMenuItem)
            For Each cmi As CustomMenuItem In item.SubItems
                cmi.CustomMenu = Me
                Dim tsi As ToolStripItem

                If cmi.Text = "-" Then
                    tsi = New ToolStripSeparator
                Else
                    Dim mi As New MenuItemEx()
                    MenuItems.Add(mi)
                    tsi = mi
                    mi.CustomMenuItem = cmi

                    Dim keys = KeysHelp.GetKeyString(cmi.KeyData)

                    If keys <> "" Then
                        mi.ShortcutKeyDisplayString = keys
                    End If

                    If mi.ShortcutKeyDisplayString Is Nothing Then
                        mi.ShortcutKeyDisplayString += ""
                    End If

                    If Not mi.ShortcutKeyDisplayString.EndsWith(" ") Then
                        mi.ShortcutKeyDisplayString += g.MenuSpace
                    End If

                    If cmi.Symbol <> Symbol.None Then
                        mi.ImageScaling = ToolStripItemImageScaling.None
                        mi.SetImage(cmi.Symbol)
                    End If

                    AddHandler tsi.Click, AddressOf MenuClick
                End If

                Items.Add(cmi)
                tsi.Text = cmi.Text

                If TypeOf menu Is ToolStripMenuItem Then
                    DirectCast(menu, ToolStripMenuItem).DropDownItems.Add(tsi)
                ElseIf TypeOf menu Is ToolStrip Then
                    DirectCast(menu, ToolStrip).Items.Add(tsi)
                End If

                BuildMenu(tsi, cmi)
            Next
        End Sub
    End Class

    Public Class CustomMenuItemEventArgs
        Inherits EventArgs

        Property Handled As Boolean
        Property Item As CustomMenuItem

        Sub New(item As CustomMenuItem)
            Me.Item = item
        End Sub
    End Class

    Public Class MenuItemEx
        Inherits ToolStripMenuItem

        Shared Property UseTooltips As Boolean

        Sub New()
        End Sub

        Sub New(text As String)
            MyBase.New(text)
        End Sub

        Overrides Function GetPreferredSize(constrainingSize As Size) As Size
            Dim ret = MyBase.GetPreferredSize(constrainingSize)
            ret.Height = CInt(Font.Height * 1.4)
            Return ret
        End Function

        Sub SetImage(symbol As Symbol)
            SetImageAsync(symbol, Me)
        End Sub

        Shared Async Sub SetImageAsync(symbol As Symbol, mi As ToolStripMenuItem)
            If symbol = Symbol.None Then
                mi.Image = Nothing
                Exit Sub
            End If

            Dim img = Await ImageHelp.GetSymbolImageAsync(symbol)

            Try
                If Not mi.IsDisposed Then
                    mi.ImageScaling = ToolStripItemImageScaling.None
                    mi.Image = img
                End If
            Catch
            End Try
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            MyBase.Dispose(disposing)
            CustomMenuItem = Nothing
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

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
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
        <DefaultValue("")>
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

    Public Class ActionMenuItem
        Inherits MenuItemEx

        Private Action As Action

        Property EnabledFunc As Func(Of Boolean)
        Property VisibleFunc As Func(Of Boolean)

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
                If value <> Keys.None Then
                    ShortcutValue = value
                    ShortcutKeyDisplayString = KeysHelp.GetKeyString(value) + g.MenuSpace
                    AddHandler Form.KeyDown, AddressOf KeyDown
                End If
            End Set
        End Property

        WriteOnly Property KeyDisplayString As String
            Set(value As String)
                ShortcutKeyDisplayString = value + g.MenuSpace
            End Set
        End Property

        Sub KeyDown(sender As Object, e As KeyEventArgs)
            If Enabled AndAlso e.KeyData = Shortcut AndAlso
                If(EnabledFunc Is Nothing, True, EnabledFunc.Invoke) AndAlso
                If(VisibleFunc Is Nothing, True, VisibleFunc.Invoke) Then

                PerformClick()
                e.Handled = True
            End If
        End Sub

        Sub Opening(sender As Object, e As CancelEventArgs)
            If Not EnabledFunc Is Nothing Then
                Enabled = EnabledFunc.Invoke
            End If

            If Not VisibleFunc Is Nothing Then
                Visible = VisibleFunc.Invoke
            End If
        End Sub

        Protected Overrides Sub OnClick(e As EventArgs)
            Application.DoEvents()

            If Not Action Is Nothing Then
                Action()
            End If

            MyBase.OnClick(e)
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            MyBase.Dispose(disposing)

            If Not Form Is Nothing Then
                RemoveHandler Form.KeyDown, AddressOf KeyDown
            End If

            Action = Nothing
            EnabledFunc = Nothing
            VisibleFunc = Nothing
            Form = Nothing
        End Sub

        Shared Function Add(Of T)(
            items As ToolStripItemCollection,
            path As String,
            action As Action(Of T),
            value As T,
            Optional help As String = Nothing) As ActionMenuItem

            Return Add(items, path, Sub() action(value), help)
        End Function

        Shared Function Add(items As ToolStripItemCollection, path As String) As ActionMenuItem
            Return Add(items, path, Nothing)
        End Function

        Shared Function Add(
            items As ToolStripItemCollection, path As String, action As Action) As ActionMenuItem

            Return Add(items, path, action, Symbol.None, Nothing)
        End Function

        Shared Function Add(
            items As ToolStripItemCollection,
            path As String,
            action As Action,
            tip As String) As ActionMenuItem

            Return Add(items, path, action, Symbol.None, tip)
        End Function

        Shared Function Add(
            items As ToolStripItemCollection,
            path As String,
            action As Action,
            symbol As Symbol,
            Optional tip As String = Nothing) As ActionMenuItem

            Dim a = path.SplitNoEmpty(" | ")
            Dim l = items

            For x = 0 To a.Length - 1
                Dim found = False

                For Each i In l.OfType(Of ToolStripMenuItem)()
                    If x < a.Length - 1 Then
                        If i.Text = a(x) + g.MenuSpace Then
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
                            Dim item As New ActionMenuItem(a(x) + g.MenuSpace, action, tip)
                            item.SetImage(symbol)
                            l.Add(item)
                            l = item.DropDownItems
                            Return item
                        End If
                    Else
                        Dim item As New ActionMenuItem()
                        item.Text = a(x) + g.MenuSpace
                        l.Add(item)
                        l = item.DropDownItems
                    End If
                End If
            Next
        End Function
    End Class

    Public Class TextCustomMenu
        Shared Function EditMenu(value As String, defaults As String, owner As Form) As String
            Using dialog As New MacroEditorDialog
                dialog.SetMacroDefaults()
                dialog.MacroEditorControl.Value = value
                dialog.MacroEditorControl.rtbDefaults.Text = defaults
                dialog.Text = "Menu Editor"

                If defaults <> "" Then
                    dialog.bnContext.Text = " Restore Defaults... "
                    dialog.bnContext.Visible = True
                    dialog.bnContext.AddClickAction(Sub() If MsgOK("Restore defaults?") Then dialog.MacroEditorControl.Value = defaults)
                End If

                If dialog.ShowDialog(owner) = DialogResult.OK Then
                    value = dialog.MacroEditorControl.Value
                End If
            End Using

            Return value
        End Function

        Shared Function GetMenu(
            definition As String,
            owner As Control,
            components As IContainer,
            action As Action(Of String)) As ContextMenuStripEx

            If owner.ContextMenuStrip Is Nothing Then
                owner.ContextMenuStrip = New ContextMenuStripEx(components)
            End If

            Dim ret = DirectCast(owner.ContextMenuStrip, ContextMenuStripEx)
            ret.Items.ClearAndDisplose

            For Each i In definition.SplitKeepEmpty(BR)
                If i.Contains("=") Then
                    Dim arg = i.Right("=").Trim
                    ActionMenuItem.Add(ret.Items, i.Left("=").Trim, action, arg, Nothing)
                ElseIf i.EndsWith("-") Then
                    ActionMenuItem.Add(ret.Items, i)
                ElseIf i = "" Then
                    ret.Items.Add(New ToolStripSeparator)
                End If
            Next

            Return ret
        End Function
    End Class

    Public Class ContextMenuStripEx
        Inherits ContextMenuStrip

        Private FormValue As Form

        Sub New()
        End Sub

        Sub New(container As IContainer)
            MyBase.New(container)
        End Sub

        Protected Overrides Sub OnHandleCreated(e As EventArgs)
            MyBase.OnHandleCreated(e)
            g.SetRenderer(Me)
            Font = New Font("Segoe UI", 9 * s.UIScaleFactor)
        End Sub

        <DefaultValue(GetType(Form), Nothing)>
        Property Form As Form
            Get
                Return FormValue
            End Get
            Set(value As Form)
                AddHandler value.Disposed, Sub() Dispose()
                FormValue = value
            End Set
        End Property

        Function Add(path As String) As ActionMenuItem
            Return Add(path, Nothing)
        End Function

        Function Add(path As String, action As Action) As ActionMenuItem
            Return Add(path, action, True)
        End Function

        Function Add(path As String, action As Action, key As Keys) As ActionMenuItem
            Return Add(path, action, key, True, Nothing, Nothing)
        End Function

        Function Add(path As String, action As Action, help As String) As ActionMenuItem
            Return Add(path, action, True, help)
        End Function

        Function Add(path As String, action As Action, enabled As Boolean) As ActionMenuItem
            Return Add(path, action, enabled, Nothing)
        End Function

        Function Add(path As String, action As Action, key As Keys, help As String) As ActionMenuItem
            Return Add(path, action, key, True, Nothing, help)
        End Function

        Function Add(path As String, action As Action, key As Keys, enabledFunc As Func(Of Boolean)) As ActionMenuItem
            Return Add(path, action, key, True, enabledFunc)
        End Function

        Function Add(path As String, action As Action, key As Keys, enabledFunc As Func(Of Boolean), help As String) As ActionMenuItem
            Return Add(path, action, key, True, enabledFunc, help)
        End Function

        Function Add(path As String, action As Action, enabled As Boolean, help As String) As ActionMenuItem
            Return Add(path, action, Keys.None, enabled, Nothing, help)
        End Function

        Function Add(path As String, action As Action, enabledFunc As Func(Of Boolean), help As String) As ActionMenuItem
            Return Add(path, action, Keys.None, True, enabledFunc, help)
        End Function

        Function Add(
            path As String,
            action As Action,
            key As Keys,
            enabled As Boolean,
            enabledFunc As Func(Of Boolean),
            Optional help As String = Nothing) As ActionMenuItem

            Dim ret = ActionMenuItem.Add(Items, path, action)

            If ret Is Nothing Then
                Exit Function
            End If

            ret.Form = Form
            ret.Shortcut = key
            ret.Enabled = enabled
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

            For Each mi In GetItems.OfType(Of ActionMenuItem)()
                If mi.ShortcutKeyDisplayString <> "" Then
                    Dim sp As New StringPair

                    If mi.Text.EndsWith("...") Then
                        sp.Name = mi.Text.TrimEnd("."c)
                    Else
                        sp.Name = mi.Text
                    End If

                    sp.Value = mi.ShortcutKeyDisplayString
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
