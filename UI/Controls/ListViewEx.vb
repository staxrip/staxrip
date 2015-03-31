Imports System.ComponentModel
Imports System.Runtime.InteropServices

Namespace UI
    Public Class ListViewEx
        Inherits ListView

        <DefaultValue(CStr(Nothing))> Property UpButton As Button
        <DefaultValue(CStr(Nothing))> Property DownButton As Button
        <DefaultValue(CStr(Nothing))> Property RemoveButton As Button
        <DefaultValue(CStr(Nothing))> Property EditButton As Button

        Event Edited(value As Object, pos As Point)
        Event BeforeShowControl(e As BeforeShowControlEventArgs)
        Event ControlsUpdated()

        Private ColumnsDic As New Dictionary(Of Control, List(Of Integer))
        Private ControlsDic As New Dictionary(Of Integer, Control)
        Private CurrentPos As Point
        Private IsInit As Boolean
        Private MouseDownIndex As Integer
        Private InDoubleClickCheckHack As Boolean
        Private Const LVM_HITTEST As Integer = &H1000 + 18
        Private Const NM_DBLCLK As Integer = -3

        <DefaultValue(False)>
        Property Editable() As Boolean

        <DefaultValue(False)>
        Property ShowContextMenuOnLeftClick As Boolean

        <DefaultValue(True)>
        Property DoubleClickDoesCheck As Boolean = True

        Sub New()
            DoubleBuffered = True
        End Sub

        Sub UpdateControls()
            If Not RemoveButton Is Nothing Then RemoveButton.Enabled = SelectedItems.Count > 0
            If Not UpButton Is Nothing Then UpButton.Enabled = CanMoveUp()
            If Not DownButton Is Nothing Then DownButton.Enabled = CanMoveDown()
            If Not EditButton Is Nothing Then EditButton.Enabled = SelectedItems.Count = 1

            RaiseEvent ControlsUpdated()
        End Sub

        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()

            If Not DesignMode Then
                If Not UpButton Is Nothing Then UpButton.AddClickAction(AddressOf MoveSelectionUp)
                If Not DownButton Is Nothing Then DownButton.AddClickAction(AddressOf MoveSelectionDown)
                If Not RemoveButton Is Nothing Then RemoveButton.AddClickAction(AddressOf RemoveSelection)
            End If
        End Sub

        Function CanMoveUp() As Boolean
            Return SelectedIndices.Count > 0 AndAlso SelectedIndices(0) > 0
        End Function

        Function CanMoveDown() As Boolean
            Return SelectedIndices.Count > 0 AndAlso SelectedIndices(SelectedIndices.Count - 1) < Items.Count - 1
        End Function

        Sub MoveSelectionUp()
            If CanMoveUp() Then
                Dim iAbove = SelectedIndices(0) - 1

                If iAbove = -1 Then
                    Exit Sub
                End If

                Dim itemAbove = Items(iAbove)
                Items.RemoveAt(iAbove)
                Dim iLastItem = SelectedIndices(SelectedIndices.Count - 1)
                Items.Insert(iLastItem + 1, itemAbove)
                UpdateControls()
                EnsureVisible(iAbove)
            End If
        End Sub

        Sub MoveSelectionDown()
            If CanMoveDown() Then
                Dim iBelow = SelectedIndices(SelectedIndices.Count - 1) + 1

                If iBelow >= Items.Count Then
                    Exit Sub
                End If

                Dim itemBelow = Items(iBelow)
                Items.RemoveAt(iBelow)
                Dim iAbove = SelectedIndices(0) - 1
                Items.Insert(iAbove + 1, itemBelow)
                UpdateControls()
                EnsureVisible(iBelow)
            End If
        End Sub

        Sub RemoveSelection()
            If SelectedItems.Count > 0 Then
                If Not MultiSelect Then
                    Dim index = SelectedIndices(0)

                    If Items.Count - 1 > index Then
                        Items(index + 1).Selected = True
                    Else
                        If index > 0 Then
                            Items(index - 1).Selected = True
                        End If
                    End If

                    Items.RemoveAt(index)
                Else
                    Dim iFirst = SelectedIndices(0)
                    Dim indices(SelectedIndices.Count - 1) As Integer
                    SelectedIndices.CopyTo(indices, 0)

                    For i = indices.Length - 1 To 0 Step -1
                        Items.RemoveAt(indices(i))
                    Next

                    If Items.Count > 0 Then
                        Dim index = If(iFirst > Items.Count - 1, Items.Count - 1, iFirst)
                        Items(index).Selected = True
                        EnsureVisible(index)
                    End If
                End If

                UpdateControls()
            End If
        End Sub

        Protected Overrides Sub OnSelectedIndexChanged(e As EventArgs)
            UpdateControls()
            MyBase.OnSelectedIndexChanged(e)
        End Sub

        Private Sub HideControls()
            For Each i In ControlsDic.Values
                i.Visible = False
                i.Enabled = False
            Next
        End Sub

        Protected Overrides Sub OnColumnReordered(e As ColumnReorderedEventArgs)
            If Editable Then
                Throw New NotImplementedException("Editing doesn't support collumn reorder.")
            End If

            MyBase.OnColumnReordered(e)
        End Sub

        Protected Overrides Sub OnColumnWidthChanging(e As ColumnWidthChangingEventArgs)
            HideControls()
            MyBase.OnColumnWidthChanging(e)
        End Sub

        Sub SendMessageHideFocus()
            Const UIS_SET = 1, UISF_HIDEFOCUS = &H1, WM_CHANGEUISTATE = &H127
            Native.SendMessage(Handle, WM_CHANGEUISTATE, MAKEWPARAM(UIS_SET, UISF_HIDEFOCUS), 0)
        End Sub

        Private Function MAKEWPARAM(low As Int32, high As Int32) As Int32
            Return (low And &HFFFF) Or (high << 16)
        End Function

        'Unfortunately, the .NET ListView component automatically toggles the checked state of items
        'when you double click on them. I know that this is not the behavior of the underlying Win32 ListView control
        'so it has to be something in the WinForms code. At this point it's worth examing how this all works.
        'In a traditional C/C++ application, the ListView control sends WM_NOTIFY messages to the window that
        'is the parent of the ListView. This is typically a dialog box window. In WinForms, events are exposed directly
        'from the controls themselves.So internally WinForms will take the WM_NOTIFY message and reflect it back to the
        'child control and then the child control handles the message by firing events that you add your event handlers too.
        'This happens for other messages besides WM_NOTIFY - such as WM_COMMAND.

        'A few minutes with a program such as Spy++ will show you the message traffic.When you double click a ListView item
        'the underlying Win32 ListView sends a WM_NOTIFY message to the parent window (typically your Form). The WinForms
        'message handler for the parent window then reroutes the message back to the ListView by sending it a new message
        'WM_REFLECT + WM_NOTIFY. The WinForms ListView message handler then dispatches it. When the WinForms ListView sees
        'a NM_DBLCLK notification it then sends a message (LVM_HITTEST) to the Win32 ListView control asking where the click
        'occurred. If it was on an item, the WinForms ListView code will then toggle the checked state of the item.

        'Since none of this behavior is exposed via the properties of the ListView control we'll have to work around it using
        'less convenient means. The solution I came up with was to set a flag during the NM_DBLCLK notification that we're in
        'the midst of a double click notification and then we intercept the LVM_HITTEST call and return that no item was found.

        Protected Overrides Sub WndProc(ByRef m As Message)
            Select Case m.Msg
                Case Native.WM_REFLECT + Native.WM_NOTIFY
                    If Not DoubleClickDoesCheck AndAlso CheckBoxes Then
                        Dim s = DirectCast(Marshal.PtrToStructure(m.LParam, GetType(Native.NMHDR)), Native.NMHDR)

                        If s.code = NM_DBLCLK Then
                            InDoubleClickCheckHack = True
                        End If
                    End If
                Case LVM_HITTEST
                    If InDoubleClickCheckHack Then
                        InDoubleClickCheckHack = False
                        m.Result = New System.IntPtr(-1)
                        Return
                    End If
                Case Native.WM_VSCROLL, Native.WM_HSCROLL
                    HideControls()
                Case Native.WM_LBUTTONDBLCLK
                    OnDoubleClick(Nothing)
            End Select

            MyBase.WndProc(m)
        End Sub

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)

            If SelectedIndices.Count > 0 Then
                MouseDownIndex = SelectedIndices(0)
            End If
        End Sub

        Event UpdateContextMenu()

        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            If e.Button = MouseButtons.Right Then
                RaiseEvent UpdateContextMenu()
            End If

            If ShowContextMenuOnLeftClick AndAlso e.Button = MouseButtons.Left Then
                RaiseEvent UpdateContextMenu()
                ContextMenuStrip.Show(Me, e.Location)
            End If

            If Not DragActive AndAlso Editable AndAlso
                e.Button = Windows.Forms.MouseButtons.Left AndAlso
                SelectedIndices.Count > 0 AndAlso
                SelectedIndices(0) = MouseDownIndex AndAlso
                Not Control.ModifierKeys = Keys.Control Then

                Dim b = GetBounds(e.Location)

                If b <> Rectangle.Empty Then
                    CurrentPos = GetPos(GetMousePos)

                    For Each i In ControlsDic.Keys
                        If i = CurrentPos.X Then
                            Dim pos = b.Location
                            Dim c = ControlsDic(i)
                            Dim args = New BeforeShowControlEventArgs()
                            args.Position = CurrentPos
                            args.Control = c
                            OnBeforeShowControl(args)

                            If Not args.Cancel Then
                                Dim offset = (c.Height - b.Height) \ 2
                                c.Location = New Point(pos.X, pos.Y - offset)
                                c.Size = b.Size

                                If c.Width < 200 Then
                                    c.Width = 200
                                End If

                                c.Enabled = True
                                IsInit = True
                                c.Text = GetText(CurrentPos)
                                IsInit = False
                                c.Visible = True
                                c.Focus()
                            End If

                            Exit For
                        End If
                    Next
                End If
            End If

            DragActive = False

            MyBase.OnMouseUp(e)
        End Sub

        Protected Overridable Sub OnEdited(value As Object, pos As Point)
            RaiseEvent Edited(value, pos)
        End Sub

        Protected Overridable Sub OnBeforeShowControl(e As BeforeShowControlEventArgs)
            RaiseEvent BeforeShowControl(e)
        End Sub

        Private Function GetBounds(mousePos As Point) As Rectangle
            Dim x, y, w, h, columnLeft, checkLength As Integer

            For Each i As ColumnHeader In Columns
                If i.Index = 0 AndAlso CheckBoxes Then
                    checkLength = 20
                Else
                    checkLength = 0
                End If

                If mousePos.X >= columnLeft + checkLength AndAlso _
                    mousePos.X <= columnLeft + i.Width Then

                    x = columnLeft + checkLength
                    w = i.Width - checkLength
                    Exit For
                End If

                columnLeft += i.Width
            Next

            For Each i As ListViewItem In Items
                Dim bounds As Rectangle = i.GetBounds(ItemBoundsPortion.Entire)
                If mousePos.Y >= bounds.Top AndAlso mousePos.Y <= bounds.Bottom Then
                    y = bounds.Top
                    h = bounds.Height
                End If
            Next

            If w = 0 OrElse h = 0 Then
                Return Rectangle.Empty
            Else
                Return New Rectangle(x, y, w, h)
            End If
        End Function

        Private Function GetPos(mousePos As Point) As Point
            Dim x, y, checkLength, columnLeft As Integer

            For Each i As ColumnHeader In Columns
                If i.Index = 0 AndAlso CheckBoxes Then
                    checkLength = 20
                Else
                    checkLength = 0
                End If

                If mousePos.X >= columnLeft + checkLength AndAlso _
                    mousePos.X <= columnLeft + i.Width Then

                    x = i.Index
                End If

                columnLeft += i.Width
            Next

            For Each i As ListViewItem In Items
                Dim bounds As Rectangle = i.GetBounds(ItemBoundsPortion.Entire)

                If mousePos.Y >= bounds.Top AndAlso mousePos.Y <= bounds.Bottom Then
                    y = i.Index
                End If
            Next

            Return New Point(x, y)
        End Function

        Private Function GetText(pos As Point) As String
            Dim item As ListViewItem = Items(pos.Y)
            Return item.SubItems(pos.X).Text
        End Function

        Protected Sub SetText(pos As Point, value As String)
            Dim item = Items(pos.Y)
            item.SubItems(pos.X).Text = value
        End Sub

        Private Sub ControlLostFocus(sender As Object, e As EventArgs)
            HideControls()
        End Sub

        Private Sub ControlValueChanged(sender As Object, e As EventArgs)
            If Not IsInit Then
                Dim c = DirectCast(sender, Control)
                SetText(CurrentPos, c.Text)

                Dim value As Object

                If TypeOf c Is ComboBox Then
                    Dim cb = DirectCast(c, ComboBox)

                    If cb.SelectedItem Is Nothing Then
                        value = cb.Text
                    Else
                        value = cb.SelectedItem
                    End If
                Else
                    value = c.Text
                End If

                OnEdited(value, CurrentPos)
            End If
        End Sub

        Sub AddControl(c As Control, columns As Integer())
            c.Visible = False
            c.Enabled = False

            AddHandler c.TextChanged, AddressOf ControlValueChanged
            AddHandler c.LostFocus, AddressOf ControlLostFocus

            Controls.Add(c)
            ColumnsDic(c) = New List(Of Integer)(columns)

            For Each i In columns
                ControlsDic(i) = c
            Next
        End Sub

        Sub AddTextBox(ParamArray columns As Integer())
            AddControl(New ListViewTextBox, columns)
        End Sub

        'enter won't fire when form has accept button set
        Private Class ListViewTextBox
            Inherits TextBox

            Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
                If keyData = Keys.Enter Then
                    Visible = False
                    Enabled = False
                    Return True
                End If

                Return MyBase.ProcessCmdKey(msg, keyData)
            End Function
        End Class

        'enter won't fire when form has accept button set
        Private Class ComboBoxEx
            Inherits ComboBox

            Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
                If keyData = Keys.Enter Then
                    Visible = False
                    Enabled = False
                    Return True
                End If

                Return MyBase.ProcessCmdKey(msg, keyData)
            End Function
        End Class

        Function AddComboBox(items As Object(), ParamArray columns As Integer()) As ComboBox
            Dim c As New ComboBoxEx
            c.MaxDropDownItems = 20
            c.Items.AddRange(items)
            AddHandler c.SelectedIndexChanged, AddressOf ControlValueChanged
            AddControl(c, columns)
            Return c
        End Function

        Protected Overrides Sub OnDragEnter(e As DragEventArgs)
            e.Effect = DragDropEffects.Move
            MyBase.OnDragEnter(e)
        End Sub

        Protected Overrides Sub OnItemDrag(e As ItemDragEventArgs)
            If SelectedItems.Count > 1 Then
                Throw New NotImplementedException("multiselect drag is not implemented")
            End If

            DoDragDrop(e.Item, DragDropEffects.Move)
            DragActive = True
            MyBase.OnItemDrag(e)
        End Sub

        Private Function GetMousePos() As Point
            Return PointToClient(Control.MousePosition)
        End Function

        Private LastDragOverPos As Point
        Private LastDrawPos As Integer
        Private DragActive As Boolean

        'OnMouseMove doesn't work while dragging
        Protected Overrides Sub OnDragOver(e As DragEventArgs)
            If Control.MousePosition <> LastDragOverPos Then
                Dim mousePos = GetMousePos()
                Dim bounds = GetBounds(mousePos)

                If Not e.Data.GetDataPresent(GetType(ListViewItem)) Then
                    e.Effect = DragDropEffects.None
                ElseIf Not bounds = Rectangle.Empty Then
                    e.Effect = DragDropEffects.Move
                    Dim y As Integer

                    If Math.Abs(mousePos.Y - bounds.Top) < _
                        Math.Abs(mousePos.Y - bounds.Bottom) Then

                        y = bounds.Top
                    Else
                        y = bounds.Bottom
                    End If

                    If y <> LastDrawPos Then
                        LastDrawPos = y
                        Refresh()

                        Using g As Graphics = CreateGraphics()
                            g.DrawLine(Pens.Black, 0, y, Width, y)
                        End Using
                    End If
                Else
                    LastDrawPos = -1
                    e.Effect = DragDropEffects.None
                    Refresh()
                End If
            End If

            LastDragOverPos = Control.MousePosition
            MyBase.OnDragOver(e)
        End Sub

        Protected Overrides Sub OnDragDrop(e As DragEventArgs)
            Dim item = DirectCast(e.Data.GetData(GetType(ListViewItem)), ListViewItem)
            Dim mousePos = GetMousePos()
            Dim p = GetPos(mousePos)
            Dim r = GetBounds(mousePos)
            Dim index As Integer

            If Math.Abs(mousePos.Y - r.Top) < Math.Abs(mousePos.Y - r.Bottom) Then

                index = p.Y
            Else
                index = p.Y + 1
            End If

            If index > item.Index Then
                index -= 1
            End If

            item.Remove()
            Items.Insert(index, item)
            MyBase.OnDragDrop(e)
        End Sub

        Protected Overrides Sub OnHandleCreated(e As EventArgs)
            Native.SetWindowTheme(Handle, "explorer", Nothing)
            MyBase.OnHandleCreated(e)
        End Sub

        Protected Overrides Sub OnColumnClick(e As ColumnClickEventArgs)
            MyBase.OnColumnClick(e)

            Dim sorter = TryCast(ListViewItemSorter, ColumnSorter)

            If Not sorter Is Nothing Then
                If sorter.LastColumn = e.Column Then
                    If Sorting = SortOrder.Ascending Then
                        Sorting = SortOrder.Descending
                    Else
                        Sorting = SortOrder.Ascending
                    End If
                Else
                    Sorting = SortOrder.Descending
                End If

                sorter.ColumnIndex = e.Column
                Sort()
            End If
        End Sub

        Public Class ColumnSorter
            Implements IComparer

            Property LastColumn As Integer
            Property ColumnIndex As Integer

            Public Function Compare(o1 As Object, o2 As Object) As Integer Implements IComparer.Compare
                Dim item1 = DirectCast(o2, ListViewItem)
                Dim item2 = DirectCast(o1, ListViewItem)

                Dim s1 = DirectCast(item1.SubItems(ColumnIndex).Tag, IComparable)
                Dim s2 = DirectCast(item2.SubItems(ColumnIndex).Tag, IComparable)

                If s1 Is Nothing Then
                    s1 = DirectCast(item1.Tag, IComparable)
                    s2 = DirectCast(item2.Tag, IComparable)
                End If

                Dim r As Integer

                If item1.ListView.Sorting = SortOrder.Ascending Then
                    r = s1.CompareTo(s2)
                Else
                    r = s2.CompareTo(s1)
                End If

                LastColumn = ColumnIndex

                Return r
            End Function
        End Class

        Sub SetColumnWidths()
            BeginUpdate()

            For Each i As ColumnHeader In Columns
                Select Case i.Text
                    Case "Hidden"
                        i.Width = 0
                        Continue For
                    Case ""
                        Columns(0).Width = CInt(Font.Height * 1.25)
                        Continue For
                End Select

                i.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)

                Dim headerWidth = i.Width
                i.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)

                If i.Width < headerWidth Then
                    i.Width = headerWidth
                End If
            Next

            Dim widthAll = Aggregate i In Columns.OfType(Of ColumnHeader)() Into Sum(i.Width)
            Columns(Columns.Count - 1).Width -= widthAll - ClientSize.Width

            EndUpdate()
        End Sub
    End Class
End Namespace

