
Imports StaxRip.UI

Public Class TestForm
    Sub New()
        InitializeComponent()
    End Sub

    Shared Sub ShowForm()
        Using form As New TestForm
            form.ShowDialog()
        End Using
    End Sub
End Class
