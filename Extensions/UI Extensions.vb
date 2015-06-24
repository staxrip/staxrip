Imports System.Runtime.CompilerServices
Imports System.Reflection
Imports System.IO
Imports StaxRip.UI
Imports System.Drawing.Drawing2D

Public Module ControlExtensions
    <Extension()>
    Sub AddClickAction(c As Control, action As Action)
        AddHandler c.Click, Sub() action()
    End Sub

    <Extension()>
    Function ClientMousePos(c As Control) As Point
        Return c.PointToClient(Control.MousePosition)
    End Function

    <Extension()>
    Sub SetSelectedPath(d As FolderBrowserDialog, path As String)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)

        If Directory.Exists(path) Then
            d.SelectedPath = path
        End If
    End Sub

    <Extension()>
    Sub SetInitDir(d As FileDialog, ParamArray paths As String())
        For Each i In paths
            If Not Directory.Exists(i) Then i = DirPath.GetParent(i)
            If Not Directory.Exists(i) Then i = DirPath.GetParent(i)
            If Not Directory.Exists(i) Then i = DirPath.GetParent(i)
            If Not Directory.Exists(i) Then i = DirPath.GetParent(i)
            If Not Directory.Exists(i) Then i = DirPath.GetParent(i)

            If Directory.Exists(i) Then
                d.InitialDirectory = i
                Exit For
            End If
        Next
    End Sub

    <Extension()>
    Sub SetFilter(d As FileDialog, ParamArray value As String())
        d.Filter = "*." + value.Join(";*.") + "|*." + value.Join(";*.") + "|All Files|*.*"
    End Sub

    <Extension()>
    Sub SendMessageCue(tb As TextBox, value As String, hideWhenFocused As Boolean)
        Dim wParam = If(hideWhenFocused, 0, 1)
        Native.SendMessage(tb.Handle, Native.EM_SETCUEBANNER, wParam, value)
    End Sub

    <Extension()>
    Sub SendMessageCue(c As ComboBox, value As String)
        Native.SendMessage(c.Handle, Native.CB_SETCUEBANNER, 1, value)
    End Sub

    <Extension()>
    Sub AutoResizeColumnsExtended(Of T)(dgv As DataGridView)
        For Each i As DataGridViewColumn In dgv.Columns
            If i.ValueType Is GetType(Boolean) Then
                i.Width = TextRenderer.MeasureText(i.HeaderText + "_", dgv.Font).Width
            Else
                Dim bindingSource = TryCast(dgv.DataSource, BindingSource)

                If Not bindingSource Is Nothing Then
                    Dim items = TryCast(bindingSource.List, IEnumerable(Of T))
                    Dim max As Integer

                    If items.Count > 0 Then
                        max = items.Select(Function(item) TextRenderer.MeasureText(MiscExtensions.ToStringEx(item.GetType.GetProperty(i.DataPropertyName).GetValue(item)), dgv.Font).Width).Max
                    End If

                    Dim width = Math.Max(max, TextRenderer.MeasureText(i.HeaderText, dgv.Font).Width)

                    If i.CellType Is GetType(DataGridViewComboBoxCell) Then
                        width += dgv.Font.Height
                    Else
                        width += dgv.Font.Height \ 3
                    End If

                    i.Width = width
                End If
            End If
        Next
    End Sub

    Function GetPropertyValue(obj As String, propertyName As String) As Object
        obj.GetType.GetProperty(propertyName).GetValue(obj)
    End Function

    <Extension()>
    Sub RemoveSelection(dgv As DataGridView)
        If Not dgv.CurrentCell Is Nothing Then dgv.CurrentCell.OwningRow.Selected = True

        For Each i As DataGridViewRow In dgv.SelectedRows
            dgv.Rows.Remove(i)
        Next
    End Sub

    <Extension()>
    Sub MoveSelectionUp(dgv As DataGridView)
        If dgv.CurrentCell Is Nothing Then Exit Sub
        dgv.CurrentRow.Selected = True
        Dim items = DirectCast(dgv.DataSource, BindingSource).List
        Dim selectedIndices = dgv.SelectedRows.Cast(Of DataGridViewRow).Select(Function(row) row.Index).Sort
        Dim indexAbove = selectedIndices(0) - 1
        If indexAbove = -1 Then Exit Sub
        Dim itemAbove = items(indexAbove)
        items.RemoveAt(indexAbove)
        Dim indexLastItem = selectedIndices(selectedIndices.Count - 1)

        If indexLastItem = items.Count Then
            items.Add(itemAbove)
        Else
            items.Insert(indexLastItem + 1, itemAbove)
        End If
    End Sub

    <Extension()>
    Sub MoveSelectionDown(dgv As DataGridView)
        If dgv.CurrentCell Is Nothing Then Exit Sub
        dgv.CurrentRow.Selected = True
        Dim items = DirectCast(dgv.DataSource, BindingSource).List
        Dim selectedIndices = dgv.SelectedRows.Cast(Of DataGridViewRow).Select(Function(row) row.Index).Sort
        Dim indexBelow = selectedIndices(selectedIndices.Count - 1) + 1
        If indexBelow >= items.Count Then Exit Sub
        Dim itemBelow = items(indexBelow)
        items.RemoveAt(indexBelow)
        Dim indexAbove = selectedIndices(0) - 1
        items.Insert(indexAbove + 1, itemBelow)
    End Sub
End Module