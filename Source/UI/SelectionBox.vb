
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
        Using td As New TaskDialog(Of DialogResult)
            If Items.Count > 0 Then
                td.MenuButton.AddRange(Items)
                td.MenuButton.Value = SelectedBag
            End If

            td.Text = If(Title = "", Application.ProductName, Title)
            td.Title = Text
            td.Buttons = TaskButton.OkCancel

            If td.Show() = DialogResult.OK Then
                SelectedBag = DirectCast(td.MenuButton.Value, ListBag(Of T))
            End If

            Return td.SelectedValue
        End Using
    End Function
End Class
