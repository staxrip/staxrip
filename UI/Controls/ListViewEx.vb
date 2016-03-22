Imports System.ComponentModel
Imports System.Runtime.InteropServices

Namespace UI
    Enum AutoCheckMode
        None
        SingleClick
        DoubleClick
    End Enum

    Class ListViewEx
        Inherits ListView

        <DefaultValue(CStr(Nothing))> Property UpButton As Button
        <DefaultValue(CStr(Nothing))> Property DownButton As Button
        <DefaultValue(CStr(Nothing))> Property RemoveButton As Button
        <DefaultValue(CStr(Nothing))> Property EditButton As Button

        Event Edited(value As Object, pos As Point)
        Event BeforeShowControl(e As BeforeShowControlEventArgs)
        Event ControlsUpdated()

        Private ColumnsDic As New Dictionary(Of Control, List(Of Integer))
        Private CurrentPos As Point
        Private IsInit As Boolean

        <DefaultValue(False)>
        Property ShowContextMenuOnLeftClick As Boolean

        <DefaultValue(GetType(AutoCheckMode), "DoubleClick")>
        Property AutoCheckMode As AutoCheckMode = AutoCheckMode.DoubleClick

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
                Dim indexAbove = SelectedIndices(0) - 1
                If indexAbove = -1 Then Exit Sub
                Dim itemAbove = Items(indexAbove)
                Items.RemoveAt(indexAbove)
                Dim iLastItem = SelectedIndices(SelectedIndices.Count - 1)
                Items.Insert(iLastItem + 1, itemAbove)
                UpdateControls()
                EnsureVisible(indexAbove)
            End If
        End Sub

        Sub MoveSelectionDown()
            If CanMoveDown() Then
                Dim indexBelow = SelectedIndices(SelectedIndices.Count - 1) + 1
                If indexBelow >= Items.Count Then Exit Sub
                Dim itemBelow = Items(indexBelow)
                Items.RemoveAt(indexBelow)
                Dim iAbove = SelectedIndices(0) - 1
                Items.Insert(iAbove + 1, itemBelow)
                UpdateControls()
                EnsureVisible(indexBelow)
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

        Sub SendMessageHideFocus()
            Const UIS_SET = 1, UISF_HIDEFOCUS = &H1, WM_CHANGEUISTATE = &H127
            Native.SendMessage(Handle, WM_CHANGEUISTATE, MAKEWPARAM(UIS_SET, UISF_HIDEFOCUS), 0)
        End Sub

        Private Function MAKEWPARAM(low As Int32, high As Int32) As Int32
            Return (low And &HFFFF) Or (high << 16)
        End Function

        Protected Overrides Sub WndProc(ByRef m As Message)
            Select Case m.Msg
                Case Native.WM_LBUTTONDBLCLK
                    If CheckBoxes AndAlso AutoCheckMode <> AutoCheckMode.DoubleClick Then
                        OnDoubleClick(Nothing)
                        Exit Sub
                    End If
                Case Native.WM_LBUTTONDOWN
                    If CheckBoxes AndAlso AutoCheckMode = AutoCheckMode.SingleClick Then
                        Dim pos = ClientMousePos
                        Dim item = GetItemAt(pos.X, pos.Y)

                        If Not item Is Nothing Then
                            Dim itemBounds = item.GetBounds(ItemBoundsPortion.Entire)

                            If pos.X > itemBounds.Left + itemBounds.Height Then
                                item.Checked = Not item.Checked
                            End If
                        End If
                    End If
            End Select

            MyBase.WndProc(m)
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

            DragActive = False

            MyBase.OnMouseUp(e)
        End Sub

        Private Function GetBounds(mousePos As Point) As Rectangle
            Dim x, y, w, h, columnLeft, checkLength As Integer

            For Each i As ColumnHeader In Columns
                If i.Index = 0 AndAlso CheckBoxes Then
                    checkLength = 20
                Else
                    checkLength = 0
                End If

                If mousePos.X >= columnLeft + checkLength AndAlso
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

                If mousePos.X >= columnLeft + checkLength AndAlso
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

                    If Math.Abs(mousePos.Y - bounds.Top) <
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

        Class ColumnSorter
            Implements IComparer

            Property LastColumn As Integer
            Property ColumnIndex As Integer

            Function Compare(o1 As Object, o2 As Object) As Integer Implements IComparer.Compare
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

        Shadows Sub AutoResizeColumns(lastAtListViewWidth As Boolean)
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
                If i.Width < headerWidth Then i.Width = headerWidth
            Next

            If lastAtListViewWidth Then
                Dim widthAll = Aggregate i In Columns.OfType(Of ColumnHeader)() Into Sum(i.Width)
                Columns(Columns.Count - 1).Width -= widthAll - ClientSize.Width
            End If

            EndUpdate()
        End Sub
    End Class
End Namespace

