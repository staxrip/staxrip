
Imports StaxRip.UI

Public Class SimpleUI
    Inherits Control

    Public WithEvents Tree As New TreeViewEx

    Public Host As New Panel

    Public Event SaveValues()
    Public Event ValueChanged()

    Public Pages As New List(Of IPage)

    Public ActivePage As IPage
    Public Store As Object

    Property FormSizeScaleFactor As SizeF

    Sub New()
        InitControls()
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
    End Sub

    Sub InitControls()
        Tree.Scrollable = False
        Tree.SelectOnMouseDown = True
        Tree.ShowLines = False
        Tree.HideSelection = False
        Tree.FullRowSelect = True
        Tree.ShowPlusMinus = False
        Tree.AutoCollaps = True
        Tree.ExpandMode = TreeNodeExpandMode.InclusiveChilds

        Controls.Add(Tree)
        Controls.Add(Host)
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)

        If Not DesignMode Then
            AddHandler FindForm.Load,
                Sub()
                    If Tree.Nodes.Count > 0 Then
                        Tree.ItemHeight = CInt(Tree.Height / (Tree.Nodes.Count)) - 2
                    End If

                    If Tree.ItemHeight > CInt(Tree.Font.Height * 1.5) Then
                        Tree.ItemHeight = CInt(Tree.Font.Height * 1.5)
                    End If
                End Sub
        End If
    End Sub

    Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
        Tree.Top = 0
        Tree.Left = 0
        Tree.Width = 0
        Tree.Height = Height

        If Tree.Nodes.Count > 1 Then
            Tree.Width = (Aggregate i In Tree.GetNodes Into Max(i.Bounds.Right)) + FontHeight
        End If

        Host.Top = 0
        Host.Left = Tree.Right + CInt(FontHeight / 3)
        Host.Height = Height
        Host.Width = Width - Tree.Width - CInt(FontHeight / 3)

        MyBase.OnLayout(levent)
    End Sub

    Sub Save()
        RaiseEvent SaveValues()
    End Sub

    Sub RaiseChangeEvent()
        RaiseEvent ValueChanged()
    End Sub

    Sub Tree_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles Tree.AfterSelect
        Dim node = e.Node

        For Each i In Pages
            If Not i.Node Is node Then
                DirectCast(i, Control).Visible = False
            End If
        Next

        For Each i In Pages
            If i.Node Is node Then
                If i.FormSizeScaleFactor <> Size.Empty Then
                    FindForm.ScaleClientSize(i.FormSizeScaleFactor.Width, i.FormSizeScaleFactor.Height)
                ElseIf FormSizeScaleFactor <> Size.Empty Then
                    FindForm.ScaleClientSize(FormSizeScaleFactor.Width, FormSizeScaleFactor.Height)
                End If

                DirectCast(i, Control).Visible = True
                DirectCast(i, Control).BringToFront()
                ActivePage = i
                PerformLayout()

                Exit For
            End If
        Next

        If Pages.Where(Function(arg) arg.Node Is node).Count = 0 Then
            If node.Nodes.Count > 0 Then
                Tree.SelectedNode = node.Nodes(0)
            End If
        End If
    End Sub

    Sub ShowPage(pagePath As String)
        For Each i In Pages
            If i.Path = pagePath Then
                Tree.SelectedNode = i.Node
            End If
        Next
    End Sub

    Sub ShowPage(page As IPage)
        For Each i In Pages
            If page Is i Then
                Tree.SelectedNode = i.Node
            End If
        Next
    End Sub

    Sub SelectLast(id As String)
        Dim last = s.Storage.GetString(id)

        If last <> "" Then
            ShowPage(last)
        ElseIf Pages.Count > 0 Then
            ShowPage(Pages(0))
        End If
    End Sub

    Sub SaveLast(id As String)
        If Not ActivePage Is Nothing Then
            s.Storage.SetString(id, ActivePage.Path)
        End If
    End Sub

    Function GetActiveFlowPage() As FlowPage
        If ActivePage Is Nothing Then
            ActivePage = CreateFlowPage("main page")
        End If

        Return DirectCast(ActivePage, FlowPage)
    End Function

    Function GetFlowPage(path As String) As FlowPage
        If path = "" Then
            path = "unknown"
        End If

        Dim q = From i In Pages Where i.Path = path

        If q.Count > 0 Then
            Return DirectCast(q.First, FlowPage)
        Else
            Return CreateFlowPage(path)
        End If
    End Function

    Function AddBool(Optional parent As FlowLayoutPanelEx = Nothing) As SimpleUICheckBox
        If parent Is Nothing Then
            parent = GetActiveFlowPage()
        End If

        Dim ret As New SimpleUICheckBox(Me)
        AddHandler SaveValues, AddressOf ret.Save
        parent.Controls.Add(ret)
        Return ret
    End Function

    Function AddEdit(parent As FlowLayoutPanelEx) As SimpleUITextEdit
        Dim ret As New SimpleUITextEdit(Me)
        parent.Controls.Add(ret)
        Return ret
    End Function

    Function AddNumeric(parent As FlowLayoutPanelEx) As SimpleUINumEdit
        Dim ret As New SimpleUINumEdit(Me)
        parent.Controls.Add(ret)
        Return ret
    End Function

    Function AddLabel(text As String) As SimpleUILabel
        Return AddLabel(Nothing, text)
    End Function

    Function AddLabel(parent As FlowLayoutPanelEx,
                      text As String,
                      Optional widthInFontHeights As Integer = 0) As SimpleUILabel

        If parent Is Nothing Then
            parent = GetActiveFlowPage()
        End If

        Dim ret As New SimpleUILabel
        ret.Offset = widthInFontHeights
        ret.Text = text
        parent.Controls.Add(ret)

        Return ret
    End Function

    Function AddNum(Optional parent As FlowLayoutPanelEx = Nothing) As NumBlock
        If parent Is Nothing Then
            parent = GetActiveFlowPage()
        End If

        Dim ret As New NumBlock(Me)
        ret.AutoSize = True
        ret.UseParenWidth = True
        AddHandler SaveValues, AddressOf ret.NumEdit.Save
        parent.Controls.Add(ret)
        Return ret
    End Function

    Function AddText(Optional parent As FlowLayoutPanelEx = Nothing) As TextBlock
        If parent Is Nothing Then
            parent = GetActiveFlowPage()
        End If

        Dim ret As New TextBlock(Me)
        ret.AutoSize = True
        ret.UseParenWidth = True
        AddHandler SaveValues, AddressOf ret.Edit.Save
        parent.Controls.Add(ret)
        Return ret
    End Function

    Function AddTextMenu(Optional parent As FlowLayoutPanelEx = Nothing) As TextMenuBlock
        If parent Is Nothing Then
            parent = GetActiveFlowPage()
        End If

        Dim ret As New TextMenuBlock(Me)
        ret.AutoSize = True
        ret.UseParenWidth = True
        AddHandler SaveValues, AddressOf ret.Edit.Save
        parent.Controls.Add(ret)
        Return ret
    End Function

    Function AddColorPicker(Optional parent As FlowLayoutPanelEx = Nothing) As ColorPickerBlock
        If parent Is Nothing Then
            parent = GetActiveFlowPage()
        End If

        Dim ret As New ColorPickerBlock(Me)
        ret.AutoSize = True
        ret.UseParenWidth = True
        AddHandler SaveValues, AddressOf ret.Save
        parent.Controls.Add(ret)
        Return ret
    End Function

    Function AddButton(Optional parent As FlowLayoutPanelEx = Nothing) As ButtonBlock
        If parent Is Nothing Then
            parent = GetActiveFlowPage()
        End If

        Dim ret As New ButtonBlock(Me)
        ret.AutoSize = True
        ret.UseParenWidth = True
        parent.Controls.Add(ret)
        Return ret
    End Function

    Function AddTextButton(Optional parent As FlowLayoutPanelEx = Nothing) As TextButtonBlock
        If parent Is Nothing Then
            parent = GetActiveFlowPage()
        End If

        Dim ret As New TextButtonBlock(Me)
        ret.AutoSize = True
        ret.UseParenWidth = True
        AddHandler SaveValues, AddressOf ret.Edit.Save
        parent.Controls.Add(ret)
        Return ret
    End Function

    Function AddMenu(Of T)(Optional parent As FlowLayoutPanelEx = Nothing) As MenuBlock(Of T)
        If parent Is Nothing Then
            parent = GetActiveFlowPage()
        End If

        Dim ret As New MenuBlock(Of T)(Me)
        ret.AutoSize = True
        ret.UseParenWidth = True
        AddHandler SaveValues, AddressOf ret.Button.Save
        parent.Controls.Add(ret)
        Return ret
    End Function

    Function AddEmptyBlock(parent As FlowLayoutPanelEx) As EmptyBlock
        Dim ret As New EmptyBlock
        ret.AutoSize = True
        ret.UseParenWidth = True
        parent.Controls.Add(ret)
        Return ret
    End Function

    Sub AddLine(parent As FlowLayoutPanelEx, Optional text As String = "")
        Dim line As New SimpleUILineControl
        line.Text = text
        line.Expand = True
        parent.Controls.Add(line)
    End Sub

    Function CreateFlowPage(Optional path As String = "-",
                            Optional autoSuspend As Boolean = False) As FlowPage
        Dim ret = New FlowPage
        ret.AutoSuspend = autoSuspend

        If autoSuspend Then
            ret.SuspendLayout()
        End If

        Pages.Add(ret)
        ret.Path = path
        ret.Dock = DockStyle.Fill
        ret.Node = Tree.AddNode(path)
        Host.Controls.Add(ret)
        ActivePage = ret
        Return ret
    End Function

    Function AddControlPage(ctrl As IPage, path As String) As IPage
        Pages.Add(ctrl)
        ctrl.Path = path
        DirectCast(ctrl, Control).Dock = DockStyle.Fill
        ctrl.Node = Tree.AddNode(path)
        Host.Controls.Add(DirectCast(ctrl, Control))
        Return ctrl
    End Function

    Function CreateDataPage(path As String) As DataPage
        Dim ret = New DataPage
        ret.EditMode = DataGridViewEditMode.EditOnEnter
        ret.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        ret.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        ret.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Pages.Add(ret)
        ret.Path = path
        ret.Dock = DockStyle.Fill
        ret.Node = Tree.AddNode(path)
        Host.Controls.Add(ret)
        ActivePage = ret
        Return ret
    End Function

    Public Class DataPage
        Inherits DataGridViewEx
        Implements IPage

        Property Node As TreeNode Implements IPage.Node
        Property Path As String Implements IPage.Path
        Property TipProvider As TipProvider Implements IPage.TipProvider

        Public Property FormSizeScaleFactor As SizeF Implements IPage.FormSizeScaleFactor

        Sub New()
            TipProvider = New TipProvider(Nothing)
            AddHandler Disposed, Sub() TipProvider.Dispose()
        End Sub
    End Class

    Public Class FlowPage
        Inherits FlowLayoutPanelEx
        Implements IPage

        Property Node As TreeNode Implements IPage.Node
        Property Path As String Implements IPage.Path
        Property TipProvider As TipProvider Implements IPage.TipProvider
        Property AutoSuspend As Boolean

        Public Property FormSizeScaleFactor As SizeF Implements IPage.FormSizeScaleFactor

        Sub New()
            TipProvider = New TipProvider(Nothing)
            AddHandler Disposed, Sub() TipProvider.Dispose()
            FlowDirection = FlowDirection.TopDown
        End Sub

        Protected Overrides Sub OnLayout(e As LayoutEventArgs)
            MyBase.OnLayout(e)
        End Sub

        Protected Overrides Sub OnCreateControl()
            If AutoSuspend Then
                ResumeLayout()
            End If

            MyBase.OnCreateControl()
        End Sub
    End Class

    Public Class SimpleUILineControl
        Inherits LineControl
        Implements SimpleUIControl

        Property Expand As Boolean Implements SimpleUIControl.Expand

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            Height = FontHeight * 2
            MyBase.OnLayout(levent)
        End Sub
    End Class

    Public Class SimpleUICheckBox
        Inherits CheckBoxEx

        Property MarginLeft As Double
        Property SaveAction As Action(Of Boolean)
        Property HelpAction As Action

        Private SimpleUI As SimpleUI

        Sub New(ui As SimpleUI)
            AutoSize = True
            SimpleUI = ui
        End Sub

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)

            If e.Button = MouseButtons.Right AndAlso Not HelpAction Is Nothing Then
                HelpAction.Invoke
            End If
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            Margin = New Padding(CInt(FontHeight / 8)) With {.Left = If(MarginLeft <> 0, CInt(MarginLeft), CInt(FontHeight / 4))}
            MyBase.OnLayout(levent)
        End Sub

        Protected Overrides Sub OnCheckedChanged(e As EventArgs)
            MyBase.OnCheckedChanged(e)
            SimpleUI.RaiseChangeEvent()
        End Sub

        Sub Save()
            SaveAction?.Invoke(Checked)

            If Field <> "" Then
                SimpleUI.Store.GetType.GetField(Field).SetValue(SimpleUI.Store, Checked)
            ElseIf [Property] <> "" Then
                SimpleUI.Store.GetType.GetProperty([Property]).SetValue(SimpleUI.Store, Checked)
            End If
        End Sub

        Private FieldValue As String

        Property Field As String
            Get
                Return FieldValue
            End Get
            Set(value As String)
                Checked = CBool(SimpleUI.Store.GetType.GetField(value).GetValue(SimpleUI.Store))
                FieldValue = value
            End Set
        End Property

        Private PropertyValue As String

        Property [Property] As String
            Get
                Return PropertyValue
            End Get
            Set(value As String)
                Checked = CBool(SimpleUI.Store.GetType.GetProperty(value).GetValue(SimpleUI.Store))
                PropertyValue = value
            End Set
        End Property

        WriteOnly Property Help As String
            Set(value As String)
                Dim parent = Me.Parent

                While Not TypeOf parent Is IPage
                    parent = parent.Parent
                End While

                If value.StartsWith("http") Then
                    value = $"[{value} {value}]"
                End If

                DirectCast(parent, IPage).TipProvider.SetTip(value, Me)
            End Set
        End Property

        Private OffsetValue As Integer

        Property Offset As Integer
            Get
                Return OffsetValue
            End Get
            Set(value As Integer)
                OffsetValue = value
                PerformLayout()
            End Set
        End Property

        Overrides Function GetPreferredSize(proposedSize As Size) As Size
            If Offset > 0 Then
                Dim ret = MyBase.GetPreferredSize(proposedSize)

                If ret.Width < Offset * FontHeight Then
                    ret.Width = Offset * FontHeight
                End If

                Return ret
            Else
                Return MyBase.GetPreferredSize(proposedSize)
            End If
        End Function
    End Class

    Public Class SimpleUINumEdit
        Inherits NumEdit

        Private SimpleUI As SimpleUI

        Property SaveAction As Action(Of Double)

        Sub New(ui As SimpleUI)
            SimpleUI = ui
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            MyBase.OnLayout(levent)
            Height = CInt(Font.Height * 1.3)
            Width = CInt(Font.Height * 4.5)
        End Sub

        Sub Save()
            SaveAction?.Invoke(Value)

            If Field <> "" Then
                Dim field = SimpleUI.Store.GetType.GetField(Me.Field)
                field.SetValue(SimpleUI.Store, Convert.ChangeType(Value, field.FieldType))
            ElseIf [Property] <> "" Then
                Dim prop = SimpleUI.Store.GetType.GetProperty([Property])
                prop.SetValue(SimpleUI.Store, Convert.ChangeType(Value, prop.PropertyType))
            End If
        End Sub

        Private PropertyValue As String

        Property [Property] As String
            Get
                Return PropertyValue
            End Get
            Set(value As String)
                Me.Value = CDbl(SimpleUI.Store.GetType.GetProperty(value).GetValue(SimpleUI.Store))
                PropertyValue = value
            End Set
        End Property

        Private FieldValue As String

        Property Field As String
            Get
                Return FieldValue
            End Get
            Set(value As String)
                Me.Value = CDbl(SimpleUI.Store.GetType.GetField(value).GetValue(SimpleUI.Store))
                FieldValue = value
            End Set
        End Property

        Property Config As Double()
            Get
                Return {Minimum, Maximum, Increment, DecimalPlaces}
            End Get
            Set(value As Double())
                If Not value Is Nothing Then
                    If value(0) = 0 AndAlso value(1) = 0 Then
                        value(0) = Double.MinValue
                        value(1) = Double.MaxValue
                    End If

                    If value.Length > 0 Then Minimum = value(0)
                    If value.Length > 1 Then Maximum = value(1)
                    If value.Length > 2 Then Increment = value(2)
                    If value.Length > 3 Then DecimalPlaces = CInt(value(3))
                End If
            End Set
        End Property

        Protected Overrides Sub OnValueChanged(numEdit As NumEdit)
            MyBase.OnValueChanged(numEdit)
            SimpleUI.RaiseChangeEvent()
        End Sub
    End Class

    Public Class SimpleUIButton
        Inherits ButtonEx
        Implements SimpleUIControl

        Property Expand As Boolean Implements SimpleUIControl.Expand
    End Class

    Public Class SimpleUITextEdit
        Inherits TextEdit
        Implements SimpleUIControl

        Property Expand As Boolean Implements SimpleUIControl.Expand
        Property SaveAction As Action(Of String)
        Property MultilineHeightFactor As Integer = 4
        Property WidthFactor As Integer = 10
        Property SimpleUI As SimpleUI

        Sub New(ui As SimpleUI)
            SimpleUI = ui
            AddHandler TextBox.TextChanged, Sub() SimpleUI.RaiseChangeEvent()
            AddHandler SimpleUI.SaveValues, AddressOf Save
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            If TextBox.Multiline Then
                Height = FontHeight * MultilineHeightFactor
            Else
                If Not Expand Then
                    Width = FontHeight * WidthFactor
                End If

                Height = CInt(FontHeight * 1.45)
            End If

            MyBase.OnLayout(levent)
        End Sub

        Sub Save()
            SaveAction?.Invoke(Text)

            If Field <> "" Then
                SimpleUI.Store.GetType.GetField(Field).SetValue(SimpleUI.Store, Text)
            ElseIf [Property] <> "" Then
                SimpleUI.Store.GetType.GetProperty([Property]).SetValue(SimpleUI.Store, Text)
            End If
        End Sub

        Private FieldValue As String

        Property Field As String
            Get
                Return FieldValue
            End Get
            Set(value As String)
                Text = CStr(SimpleUI.Store.GetType.GetField(value).GetValue(SimpleUI.Store))
                FieldValue = value
            End Set
        End Property

        Private PropertyValue As String

        Property [Property] As String
            Get
                Return PropertyValue
            End Get
            Set(value As String)
                Text = CStr(SimpleUI.Store.GetType.GetProperty(value).GetValue(SimpleUI.Store))
                PropertyValue = value
            End Set
        End Property

        WriteOnly Property UseCommandlineEditor As Boolean
            Set(value As Boolean)
                If value Then
                    AddHandler TextBox.MouseDown, Sub() EditCommandline()
                End If
            End Set
        End Property

        WriteOnly Property UseMacroEditor As Boolean
            Set(value As Boolean)
                If value Then
                    AddHandler TextBox.Click, Sub() EditMacro()
                End If
            End Set
        End Property

        Sub EditCommandline()
            Using form As New MacroEditorDialog
                form.SetBatchDefaults()
                form.MacroEditorControl.Value = Text

                If form.ShowDialog() = DialogResult.OK Then
                    Text = form.MacroEditorControl.Value
                End If
            End Using
        End Sub

        Sub EditMacro()
            Using dialog As New MacroEditorDialog
                dialog.SetMacroDefaults()
                dialog.MacroEditorControl.Value = Text

                If dialog.ShowDialog() = DialogResult.OK Then
                    Text = dialog.MacroEditorControl.Value
                End If
            End Using
        End Sub
    End Class

    Public Class SimpleUIMenuButton(Of T)
        Inherits MenuButton
        Implements SimpleUIControl

        Property Expand As Boolean Implements SimpleUIControl.Expand
        Property SaveAction As Action(Of T)
        Property HelpAction As Action

        Private SimpleUI As SimpleUI

        Sub New(ui As SimpleUI)
            SimpleUI = ui
        End Sub

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)

            If e.Button = MouseButtons.Right AndAlso Not HelpAction Is Nothing Then
                HelpAction.Invoke
            End If
        End Sub

        Protected Overrides Sub OnValueChanged(value As Object)
            MyBase.OnValueChanged(value)
            SimpleUI.RaiseChangeEvent()
        End Sub

        Shadows Property Value As T
            Get
                Return CType(MyBase.Value, T)
            End Get
            Set(value As T)
                MyBase.Value = CType(value, T)
            End Set
        End Property

        Sub Save()
            SaveAction?.Invoke(Value)

            If Field <> "" Then
                Dim field = SimpleUI.Store.GetType.GetField(Me.Field)
                field.SetValue(SimpleUI.Store, Convert.ChangeType(Value, field.FieldType))
            ElseIf [Property] <> "" Then
                Dim prop = SimpleUI.Store.GetType.GetProperty([Property])
                prop.SetValue(SimpleUI.Store, Convert.ChangeType(Value, prop.PropertyType))
            End If
        End Sub

        Private PropertyValue As String

        Property [Property] As String
            Get
                Return PropertyValue
            End Get
            Set(value As String)
                Me.Value = DirectCast(SimpleUI.Store.GetType.GetProperty(value).GetValue(SimpleUI.Store), T)
                PropertyValue = value
            End Set
        End Property

        Private FieldValue As String

        Property Field As String
            Get
                Return FieldValue
            End Get
            Set(value As String)
                Me.Value = DirectCast(SimpleUI.Store.GetType.GetField(value).GetValue(SimpleUI.Store), T)
                FieldValue = value
            End Set
        End Property
    End Class

    Public Class SimpleUILabel
        Inherits LabelEx

        Property MarginTop As Integer
        Property HelpAction As Action

        Private OffsetValue As Double

        Property Offset As Double
            Get
                Return OffsetValue
            End Get
            Set(value As Double)
                OffsetValue = value
                PerformLayout()
            End Set
        End Property

        Sub New()
            TextAlign = ContentAlignment.MiddleLeft
            AutoSize = True
        End Sub

        WriteOnly Property Help As String
            Set(value As String)
                If value.StartsWith("http") Then
                    value = $"[{value} {value}]"
                End If

                Dim parent = Me.Parent

                While Not TypeOf parent Is IPage
                    parent = parent.Parent
                End While

                DirectCast(parent, IPage).TipProvider.SetTip(value, Me)
            End Set
        End Property

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)

            If e.Button = MouseButtons.Right AndAlso Not HelpAction Is Nothing Then
                HelpAction.Invoke
            End If
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            If Margin.Top <> MarginTop Then
                Dim m = Margin
                m.Top = MarginTop
                Margin = m
            End If

            MyBase.OnLayout(levent)
        End Sub

        Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
            If Offset > 0 Then
                Dim ret = MyBase.GetPreferredSize(proposedSize)

                If ret.Width < Offset * FontHeight Then
                    ret.Width = CInt(Offset * FontHeight)
                End If

                Return ret
            Else
                Return MyBase.GetPreferredSize(proposedSize)
            End If
        End Function
    End Class

    Public Class EmptyBlock
        Inherits FlowLayoutPanelEx

        Sub New()
            Font = New Font("Segoe UI", 9.0! * s.UIScaleFactor)
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            For Each ctrl As Control In Controls
                ctrl.Margin = New Padding(FontHeight \ 15)
            Next

            MyBase.OnLayout(levent)
        End Sub
    End Class

    MustInherit Class LabelBlock
        Inherits EmptyBlock

        Property Label As New SimpleUILabel

        Sub New()
            Controls.Add(Label)
        End Sub

        Shadows Property Text As String
            Get
                Return Label.Text
            End Get
            Set(value As String)
                If value.EndsWith(":") Then
                    Label.Text = value
                Else
                    Label.Text = value + ":"
                End If
            End Set
        End Property

        Overridable WriteOnly Property Help As String
            Set(value As String)
                Label.Help = value
            End Set
        End Property

        Public WriteOnly Property HelpAction As Action
            Set(value As Action)
                Label.HelpAction = value
            End Set
        End Property
    End Class

    Public Class NumBlock
        Inherits LabelBlock

        Property NumEdit As SimpleUINumEdit

        Sub New(ui As SimpleUI)
            NumEdit = New SimpleUINumEdit(ui)
            Controls.Add(NumEdit)
        End Sub

        Overrides WriteOnly Property Help As String
            Set(value As String)
                Label.Help = value
                NumEdit.Help = value
            End Set
        End Property

        Property Field As String
            Get
                Return NumEdit.Field
            End Get
            Set(value As String)
                NumEdit.Field = value
            End Set
        End Property

        Property [Property] As String
            Get
                Return NumEdit.Property
            End Get
            Set(value As String)
                NumEdit.Property = value
            End Set
        End Property

        Property Config As Double()
            Get
                Return NumEdit.Config
            End Get
            Set(value As Double())
                NumEdit.Config = value
            End Set
        End Property
    End Class

    Public Class ButtonBlock
        Inherits EmptyBlock

        Property Button As SimpleUIButton

        Sub New(ui As SimpleUI)
            Button = New SimpleUIButton
            Controls.Add(Button)
        End Sub
    End Class

    Public Class TextBlock
        Inherits LabelBlock

        Property Edit As SimpleUITextEdit

        Sub New(ui As SimpleUI)
            Edit = New SimpleUITextEdit(ui)
            Controls.Add(Edit)
        End Sub

        Overridable Property Field As String
            Get
                Return Edit.Field
            End Get
            Set(value As String)
                Edit.Field = value
            End Set
        End Property

        Overridable Property [Property] As String
            Get
                Return Edit.Property
            End Get
            Set(value As String)
                Edit.Property = value
            End Set
        End Property

        Property Expandet As Boolean
            Get
                Return Edit.Expand
            End Get
            Set(value As Boolean)
                Edit.Expand = value
            End Set
        End Property
    End Class

    Public Class TextMenuBlock
        Inherits TextBlock

        Property Button As New ButtonEx

        Sub New(ui As SimpleUI)
            MyBase.New(ui)
            Button.Width = FontHeight * 2
            Button.Height = CInt(FontHeight * 1.5)
            Button.ShowMenuSymbol = True
            Button.ContextMenuStrip = New ContextMenuStripEx
            Controls.Add(Button)
            AddHandler Edit.EnabledChanged, Sub() Button.Enabled = Edit.Enabled
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            Button.ContextMenuStrip.Dispose()
            MyBase.Dispose(disposing)
        End Sub

        Sub MenuClick(value As String)
            value = Macro.Expand(value)
            Dim tup = Macro.ExpandGUI(value)

            If tup.Cancel Then
                Exit Sub
            End If

            Edit.Text = tup.Value
        End Sub

        Sub AddMenu(menu As String)
            TextCustomMenu.GetMenu(menu, Button, Nothing, AddressOf MenuClick)
        End Sub

        Sub AddMenu(menuText As String, menuValue As String)
            AddMenu(menuText, Function() menuValue)
        End Sub

        Sub AddMenu(menuText As String, menuFunc As Func(Of String))
            Dim action = Sub()
                             Dim v = menuFunc.Invoke

                             If v <> "" Then
                                 Edit.Text = v
                             End If
                         End Sub

            AddMenu(menuText, action)
        End Sub

        Sub AddMenu(menuText As String, menuAction As Action)
            ActionMenuItem.Add(Button.ContextMenuStrip.Items, menuText, menuAction)
        End Sub
    End Class

    Public Class ColorPickerBlock
        Inherits TextButtonBlock

        Private _Color As Color
        Private SimpleUI As SimpleUI

        Property Color As Color
            Get
                Return _Color
            End Get
            Set(value As Color)
                _Color = value
                Edit.BackColor = value
                Edit.TextBox.BackColor = value
            End Set
        End Property

        Sub New(ui As SimpleUI)
            MyBase.New(ui)
            SimpleUI = ui
            Edit.TextBox.ReadOnly = True

            Button.ClickAction = Sub()
                                     Using cd As New ColorDialog
                                         cd.Color = Color

                                         If cd.ShowDialog() = DialogResult.OK Then
                                             Color = cd.Color
                                         End If
                                     End Using
                                 End Sub
        End Sub

        Sub Save()
            If Field <> "" Then
                SimpleUI.Store.GetType.GetField(Field).SetValue(SimpleUI.Store, Color)
            ElseIf [Property] <> "" Then
                SimpleUI.Store.GetType.GetProperty([Property]).SetValue(SimpleUI.Store, Color)
            End If
        End Sub

        Private _Field As String

        Overrides Property Field As String
            Get
                Return _Field
            End Get
            Set(value As String)
                Color = DirectCast(SimpleUI.Store.GetType.GetField(value).GetValue(SimpleUI.Store), Color)
                _Field = value
            End Set
        End Property

        Private _Property As String

        Overrides Property [Property] As String
            Get
                Return _Property
            End Get
            Set(value As String)
                Color = DirectCast(SimpleUI.Store.GetType.GetProperty(value).GetValue(SimpleUI.Store), Color)
                _Property = value
            End Set
        End Property
    End Class

    Public Class TextButtonBlock
        Inherits TextBlock

        Property Button As New ButtonEx

        Sub New(ui As SimpleUI)
            MyBase.New(ui)
            Button.Width = FontHeight * 2
            Button.Height = CInt(FontHeight * 1.45)
            Button.AutoSizeMode = AutoSizeMode.GrowOnly
            Button.AutoSize = True
            Button.Text = "..."
            Controls.Add(Button)
            AddHandler Edit.EnabledChanged, Sub() Button.Enabled = Edit.Enabled
        End Sub

        Sub BrowseFile(filterTypes As String())
            BrowseFile(FileTypes.GetFilter(filterTypes))
        End Sub

        Sub BrowseFile(filter As String, Optional initDir As String = Nothing)
            Button.ClickAction = Sub()
                                     Using dia As New OpenFileDialog
                                         dia.Filter = filter

                                         If initDir = "" OrElse Not Directory.Exists(initDir) Then
                                             initDir = p.TempDir
                                         End If

                                         dia.SetInitDir(initDir)

                                         If dia.ShowDialog = DialogResult.OK Then
                                             Edit.Text = dia.FileName
                                         End If
                                     End Using
                                 End Sub
        End Sub

        Sub BrowseFolder()
            Button.ClickAction =
                Sub()
                    Using dialog As New FolderBrowserDialog
                        dialog.SetSelectedPath(s.LastSourceDir)

                        If dialog.ShowDialog = DialogResult.OK Then
                            Edit.Text = dialog.SelectedPath
                        End If
                    End Using
                End Sub
        End Sub
    End Class

    Public Class MenuBlock(Of T)
        Inherits LabelBlock

        Property Button As SimpleUIMenuButton(Of T)

        Sub New(ui As SimpleUI)
            Button = New SimpleUIMenuButton(Of T)(ui)
            Controls.Add(Button)
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            If Not Button Is Nothing Then
                Button.Height = CInt(FontHeight * 1.5)
                Button.Width = FontHeight * 10
            End If

            MyBase.OnLayout(levent)
        End Sub

        Sub Add(items As IEnumerable(Of Object))
            Button.Add(items)
        End Sub

        Sub Add(ParamArray items As Object())
            Button.Add(items)
        End Sub

        Sub Add(path As String, obj As T)
            Button.Add(path, obj)
        End Sub

        Public Shadows WriteOnly Property Help As String
            Set(value As String)
                Dim parent = Me.Parent

                While Not TypeOf parent Is IPage
                    parent = parent.Parent
                End While

                DirectCast(parent, IPage).TipProvider.SetTip(value, Label, Button)
            End Set
        End Property

        Public Shadows WriteOnly Property HelpAction As Action
            Set(value As Action)
                Button.HelpAction = value
                Label.HelpAction = value
            End Set
        End Property

        Property Expandet As Boolean
            Get
                Return Button.Expand
            End Get
            Set(value As Boolean)
                Button.Expand = value
            End Set
        End Property

        Property Field As String
            Get
                Return Button.Field
            End Get
            Set(value As String)
                Button.Field = value
            End Set
        End Property

        Property [Property] As String
            Get
                Return Button.Property
            End Get
            Set(value As String)
                Button.Property = value
            End Set
        End Property
    End Class

    Interface SimpleUIControl
        Property Expand As Boolean
    End Interface
End Class