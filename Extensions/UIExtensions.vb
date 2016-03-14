Imports System.Runtime.CompilerServices
Imports System.Drawing.Drawing2D

Public Module UIExtensions
    <Extension()>
    Function ResizeToSmallIconSize(img As Image) As Image
        If Not img Is Nothing AndAlso img.Size <> SystemInformation.SmallIconSize Then
            Dim s = SystemInformation.SmallIconSize
            Dim r As New Bitmap(s.Width, s.Height)

            Using g = Graphics.FromImage(DirectCast(r, Image))
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.PixelOffsetMode = PixelOffsetMode.HighQuality
                g.DrawImage(img, 0, 0, s.Width, s.Height)
            End Using

            Return r
        End If

        Return img
    End Function

    <Extension()>
    Function ResizeImage(image As Image, ByVal height As Integer) As Image
        Dim percentHeight = height / image.Height
        Dim ret = New Bitmap(CInt(image.Width * percentHeight), CInt(height))

        Using g = Graphics.FromImage(ret)
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            g.DrawImage(image, 0, 0, ret.Width, ret.Height)
        End Using

        Return ret
    End Function

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

        If Directory.Exists(path) Then d.SelectedPath = path
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
        For Each i As DataGridViewRow In dgv.SelectedRows
            dgv.Rows.Remove(i)
        Next

        If dgv.SelectedRows.Count = 0 AndAlso dgv.RowCount > 0 Then
            dgv.Rows(dgv.RowCount - 1).Selected = True
        End If
    End Sub

    <Extension()>
    Function CanMoveUp(dgv As DataGridView) As Boolean
        Return dgv.SelectedRows.Count > 0 AndAlso dgv.SelectedRows(0).Index > 0
    End Function

    <Extension()>
    Function CanMoveDown(dgv As DataGridView) As Boolean
        Return dgv.SelectedRows.Count > 0 AndAlso dgv.SelectedRows(0).Index < dgv.RowCount - 1
    End Function

    <Extension()>
    Sub MoveSelectionUp(dgv As DataGridView)
        If CanMoveUp(dgv) Then
            Dim bs = DirectCast(dgv.DataSource, BindingSource)
            Dim pos = bs.Position
            bs.RaiseListChangedEvents = False
            Dim current = bs.Current
            bs.Remove(current)
            pos -= 1
            bs.Insert(pos, current)
            bs.Position = pos
            bs.RaiseListChangedEvents = True
            bs.ResetBindings(False)
        End If
    End Sub

    <Extension()>
    Sub MoveSelectionDown(dgv As DataGridView)
        If CanMoveDown(dgv) Then
            Dim bs = DirectCast(dgv.DataSource, BindingSource)
            Dim pos = bs.Position
            bs.RaiseListChangedEvents = False
            Dim current = bs.Current
            bs.Remove(current)
            pos += 1
            bs.Insert(pos, current)
            bs.Position = pos
            bs.RaiseListChangedEvents = True
            bs.ResetBindings(False)
        End If
    End Sub
End Module