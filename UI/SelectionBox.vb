Imports StaxRip.UI

Class SelectionBox(Of T)
    Property Text As String
    Property Title As String
    Property Items As New List(Of ListBag(Of T))
    Property SelectedBag As ListBag(Of T)

    Property SelectedValue As T
        Get
            Return SelectedBag.Value
        End Get
        Set(value As T)
            For Each i In Items
                If i.Value.Equals(value) Then SelectedBag = i
            Next
        End Set
    End Property

    Property SelectedText As String
        Get
            Return SelectedBag.Text
        End Get
        Set(value As String)
            For Each i In Items
                If i.Text = value Then SelectedBag = i
            Next
        End Set
    End Property

    Sub AddItem(text As String, item As T)
        Items.Add(New ListBag(Of T)(text, item))
        If SelectedBag Is Nothing Then SelectedBag = Items.Last
    End Sub

    Sub AddItem(item As T)
        AddItem(item.ToString, item)
    End Sub

    Function Show() As DialogResult
        Using sb As New SelectionBoxForm
            If Items.Count > 0 Then
                sb.mb.Add(Items)
                sb.mb.Value = SelectedBag
            End If

            For Each i In Items
                Dim textWidth = TextRenderer.MeasureText(i.ToString, sb.mb.Font).Width
                If sb.mb.Width < textWidth Then sb.Width += textWidth - sb.mb.Width
            Next

            sb.Text = Title
            If sb.Text = "" Then sb.Text = Application.ProductName
            sb.lText.Text = Text
            Dim ret = sb.ShowDialog
            SelectedBag = DirectCast(sb.mb.Value, ListBag(Of T))

            Return ret
        End Using
    End Function
End Class