Imports StaxRip.UI

Class SelectionBox(Of T)
    Property Text As String
    Property Title As String
    Property Items As New List(Of ListBag(Of T))
    Property SelectedBag As ListBag(Of T)

    Property SelectedItem As T
        Get
            Return SelectedBag.Value
        End Get
        Set(value As T)
            For Each i In Items
                If i.Value.Equals(value) Then SelectedBag = i
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
        Using Form As New SelectionBoxForm
            If Items.Count > 0 Then
                Form.mb.Add(Items)
                Form.mb.Value = SelectedBag
            End If

            For Each i In Items
                Dim textWidth = TextRenderer.MeasureText(i.ToString, Form.mb.Font).Width
                If Form.mb.Width < textWidth Then Form.Width += textWidth - Form.mb.Width
            Next

            Form.Text = Title
            If Form.Text = "" Then Form.Text = Application.ProductName
            Form.lText.Text = Text
            Dim ret = Form.ShowDialog
            SelectedBag = DirectCast(Form.mb.Value, ListBag(Of T))

            Return ret
        End Using
    End Function
End Class