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
    Public Class CustomMenuItem
        Sub New()
        End Sub

        Sub New(text As String)
            Me.Text = text
        End Sub

        <NonSerialized()>
        Private CustomMenuValue As CustomMenu

        Property CustomMenu() As CustomMenu
            Get
                Return CustomMenuValue
            End Get
            Set(Value As CustomMenu)
                CustomMenuValue = Value
            End Set
        End Property

        Private TextValue As String

        Overridable Property Text() As String
            Get
                Return TextValue
            End Get
            Set(Value As String)
                TextValue = Value
            End Set
        End Property

        Private SubItemsValue As New List(Of CustomMenuItem)

        Property SubItems() As List(Of CustomMenuItem)
            Get
                Return SubItemsValue
            End Get
            Set(Value As List(Of CustomMenuItem))
                SubItemsValue = Value
            End Set
        End Property

        Private KeyDataValue As Keys

        Property KeyData() As Keys
            Get
                Return KeyDataValue
            End Get
            Set(Value As Keys)
                KeyDataValue = Value
            End Set
        End Property

        Private MethodNameValue As String

        Property MethodName() As String
            Get
                Return MethodNameValue
            End Get
            Set(Value As String)
                MethodNameValue = Value
            End Set
        End Property

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
        Private ParentValue As CustomMenuItem

        Property Parent() As CustomMenuItem
            Get
                Return ParentValue
            End Get
            Set(Value As CustomMenuItem)
                ParentValue = Value
            End Set
        End Property

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

    Public Class CustomMenu
        Private Items As New List(Of CustomMenuItem)

        Event Command(e As CustomMenuItemEventArgs)

        Sub New( _
            defaultMenu As Func(Of CustomMenuItem), _
            menuItem As CustomMenuItem, _
            commandManager As CommandManager, _
            toolStrip As ToolStrip)

            Me.CommandManager = commandManager
            Me.DefaultMenu = defaultMenu
            Me.MenuItem = menuItem
            Me.ToolStrip = toolStrip
        End Sub

        Private MenuValue As Menu

        Property Menu() As Menu
            Get
                Return MenuValue
            End Get
            Set(Value As Menu)
                MenuValue = Value
            End Set
        End Property

        Private MenuStripValue As MenuStrip

        Property MenuStrip() As MenuStrip
            Get
                Return MenuStripValue
            End Get
            Set(Value As MenuStrip)
                MenuStripValue = Value
            End Set
        End Property

        Private ToolStripValue As ToolStrip

        Property ToolStrip() As ToolStrip
            Get
                Return ToolStripValue
            End Get
            Set(Value As ToolStrip)
                ToolStripValue = Value
            End Set
        End Property

        Private MenuItemsValue As New List(Of MenuItemEx)

        Property MenuItems() As List(Of MenuItemEx)
            Get
                Return MenuItemsValue
            End Get
            Set(Value As List(Of MenuItemEx))
                MenuItemsValue = Value
            End Set
        End Property

        Private DefaultMenuValue As Func(Of CustomMenuItem)

        Property DefaultMenu() As Func(Of CustomMenuItem)
            Get
                Return DefaultMenuValue
            End Get
            Set(Value As Func(Of CustomMenuItem))
                DefaultMenuValue = Value
            End Set
        End Property

        Private MenuItemValue As CustomMenuItem

        Property MenuItem() As CustomMenuItem
            Get
                Return MenuItemValue
            End Get
            Set(Value As CustomMenuItem)
                MenuItemValue = Value
            End Set
        End Property

        Private CommandManagerValue As CommandManager

        Property CommandManager() As CommandManager
            Get
                Return CommandManagerValue
            End Get
            Set(Value As CommandManager)
                CommandManagerValue = Value
            End Set
        End Property

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
                If Not i.Tooltip Is Nothing Then
                    ret.Add(i.Tooltip)
                End If
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

                If Not e.Handled Then
                    Process(item)
                End If

                Dim f As Form = ToolStrip.FindForm

                If Not f Is Nothing Then
                    f.Refresh()
                End If
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

    Public Class CustomMenuItemEventArgs
        Inherits EventArgs

        Sub New(item As CustomMenuItem)
            Me.Item = item
        End Sub

        Private HandledValue As Boolean

        Property Handled() As Boolean
            Get
                Return HandledValue
            End Get
            Set(Value As Boolean)
                HandledValue = Value
            End Set
        End Property

        Private ItemValue As CustomMenuItem

        Property Item() As CustomMenuItem
            Get
                Return ItemValue
            End Get
            Set(Value As CustomMenuItem)
                ItemValue = Value
            End Set
        End Property
    End Class

    Public Class MenuItemEx
        Inherits ToolStripMenuItem

        Private TipValue As StringPair

        Shared Property UseTooltips As Boolean

        Sub New()
        End Sub

        Sub New(text As String)
            MyBase.New(text)
        End Sub

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        ReadOnly Property Tooltip() As StringPair
            Get
                If TipValue Is Nothing AndAlso
                    Not CustomMenuItem Is Nothing AndAlso
                    Not CustomMenuItem.CustomMenu Is Nothing Then

                    If CustomMenuItem.CustomMenu.CommandManager.HasCommand(CustomMenuItem.MethodName) Then
                        Dim command = CustomMenuItem.CustomMenu.CommandManager.GetCommand(CustomMenuItem.MethodName)

                        If Not command.Attribute.Description Is Nothing Then
                            Dim sp As New StringPair

                            If Text.EndsWith("...") Then
                                sp.Name = Text.TrimEnd("."c)
                            Else
                                sp.Name = Text
                            End If

                            sp.Value = command.Attribute.Description
                            Dim paramHelp = command.GetParameterHelp(CustomMenuItem.Parameters)

                            If Not paramHelp Is Nothing Then
                                sp.Value += " (" + paramHelp + ")"
                            End If

                            TipValue = sp
                        End If
                    End If
                End If

                Return TipValue
            End Get
        End Property

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
                        If c.MethodInfo.Name = "ExecuteCmdl" Then
                            HelpText = CustomMenuItem.Parameters(0).ToString.Trim(""""c)
                        Else
                            HelpText = c.Attribute.Description
                        End If
                    End If
                End If
            End Set
        End Property

        Private Function ShouldSerializeHelpText() As Boolean
            Return OK(HelpTextValue)
        End Function

        Private HelpTextValue As String

        <Editor(GetType(StringEditor), GetType(UITypeEditor))>
        Property HelpText() As String
            Get
                Return HelpTextValue
            End Get
            Set(Value As String)
                HelpTextValue = Value

                If UseTooltips Then
                    If OK(HelpTextValue) Then
                        If OK(HelpTextValue) AndAlso HelpTextValue.Length < 80 Then
                            ToolTipText = HelpTextValue.TrimEnd("."c)
                        Else
                            ToolTipText = "Right-click for help"
                        End If
                    End If
                End If
            End Set
        End Property

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)

            If e.Button = MouseButtons.Right AndAlso OK(HelpText) Then
                CloseAll(Me)
                g.ShowHelp(Text, HelpText)
            End If
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

        Sub New(text As String, a As Action)
            Me.New(text, a, Nothing)
        End Sub

        Sub New(text As String,
                action As Action,
                Optional tooltip As String = Nothing,
                Optional enabled As Boolean = True)

            Me.Text = text
            Me.Action = action
            Me.HelpText = tooltip
            Me.Enabled = enabled
        End Sub

        Shared Function Add(items As ToolStripItemCollection,
                            path As String,
                            action As Action,
                            Optional tip As String = Nothing) As ToolStripMenuItem

            Return ActionMenuItem(Of Action).Add(items, path, AddressOf OnAction, action, tip)
        End Function

        Shared Sub OnAction(a As Action)
            Application.DoEvents()
            a()
        End Sub

        Protected Overrides Sub OnClick(e As EventArgs)
            MyBase.OnClick(e)
            Application.DoEvents()
            Action()
        End Sub
    End Class

    Public Class ActionMenuItem(Of T)
        Inherits MenuItemEx

        Private Action As Action(Of T)
        Property Value As T

        Sub New()
        End Sub

        Sub New(text As String, action As Action(Of T), value As T)
            Me.New(text, action, value, Nothing)
        End Sub

        Sub New(text As String, action As Action(Of T), value As T, tip As String)
            Me.New(text, action, value, tip, Keys.None)
        End Sub

        Sub New(text As String,
                action As Action(Of T),
                argument As T,
                toolTip As String,
                key As Keys)

            Me.Text = text
            Me.Action = action
            Me.Value = argument
            Me.HelpText = toolTip

            If key <> Keys.None Then
                Me.ShortcutKeyDisplayString = KeysHelp.GetKeyString(key)
            End If
        End Sub

        Protected Overrides Sub OnClick(e As EventArgs)
            MyBase.OnClick(e)
            Application.DoEvents()

            If Not Action Is Nothing Then
                Action(Value)
            End If
        End Sub

        Shared Function Add(items As ToolStripItemCollection,
                            path As String,
                            action As Action(Of T),
                            value As T,
                            Optional toolTip As String = Nothing) As ToolStripMenuItem

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
                            Dim item As New ActionMenuItem(Of T)(a(x), action, value, toolTip, Keys.None)
                            l.Add(item)
                            l = item.DropDownItems
                            Return item
                        End If
                    Else
                        Dim item As New ActionMenuItem(Of T)
                        item.Text = a(x)
                        l.Add(item)
                        l = item.DropDownItems
                    End If
                End If
            Next
        End Function
    End Class

    Public Class TextCustomMenu
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
                    ActionMenuItem(Of String).Add(r.Items, i.Left("="), action, arg, Nothing)
                ElseIf i.EndsWith(" | -") Then
                    ActionMenuItem(Of String).Add(r.Items, i, Nothing, Nothing, Nothing)
                ElseIf i = "" Then
                    r.Items.Add(New ToolStripSeparator)
                End If
            Next

            Return r
        End Function
    End Class
End Namespace