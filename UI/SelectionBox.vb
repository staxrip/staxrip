
Imports StaxRip.UI

Public Class SelectionBox(Of T)
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
                If i.Value.Equals(value) Then
                    SelectedBag = i
                End If
            Next
        End Set
    End Property

    Property SelectedText As String
        Get
            Return SelectedBag.Text
        End Get
        Set(value As String)
            For Each i In Items
                If i.Text = value Then
                    SelectedBag = i
                End If
            Next
        End Set
    End Property

    Sub AddItem(text As String, item As T)
        Items.Add(New ListBag(Of T)(text, item))

        If SelectedBag Is Nothing Then
            SelectedBag = Items.Last
        End If
    End Sub

    Sub AddItem(item As T)
        AddItem(item.ToString, item)
    End Sub

    Function Show() As DialogResult
        Using form As New SelectionBoxForm
            If Items.Count > 0 Then
                form.mb.Add(Items)
                form.mb.Value = SelectedBag
            End If

            For Each i In Items
                Dim textWidth = TextRenderer.MeasureText(i.ToString, form.mb.Font).Width + form.FontHeight * 3

                If form.mb.Width < textWidth Then
                    form.Width += textWidth - form.mb.Width
                End If
            Next

            form.Text = Title

            If form.Text = "" Then
                form.Text = Application.ProductName
            End If

            form.laText.Text = Text
            Dim ret = form.ShowDialog
            SelectedBag = DirectCast(form.mb.Value, ListBag(Of T))

            Return ret
        End Using
    End Function
End Class
