Imports StaxRip.UI

Public Class SelectionBox(Of T)
    Property Items As New List(Of Object)
    Property SelectedItem As T
    Property Text As String
    Property Title As String

    Sub AddItem(text As String, item As T)
        Items.Add(New ListBag(Of T)(text, item))
    End Sub

    Sub AddItem(item As T)
        Items.Add(item)
    End Sub

    Function Show() As DialogResult
        Using Form As New SelectionBoxForm
            If Items.Count > 0 Then
                Form.cbItems.Items.AddRange(Items.ToArray)

                If Not SelectedItem Is Nothing Then
                    Form.cbItems.SelectedItem = SelectedItem
                End If

                If Form.cbItems.SelectedIndex = -1 Then
                    Form.cbItems.SelectedIndex = 0
                End If
            End If

            Form.Text = Title

            If Not OK(Form.Text) Then
                Form.Text = Application.ProductName
            End If

            Form.lText.Text = Text

            Dim ret As DialogResult = Form.ShowDialog

            If TypeOf Form.cbItems.SelectedItem Is ListBag(Of T) Then
                SelectedItem = ListBag(Of T).GetValue(Form.cbItems)
            Else
                SelectedItem = DirectCast(Form.cbItems.SelectedItem, T)
            End If

            Return ret
        End Using
    End Function
End Class