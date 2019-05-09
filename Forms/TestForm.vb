Public Class TestForm
    Sub New()
        InitializeComponent()
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
    End Sub

    Shared Sub ShowForm()
        Using form As New TestForm
            form.ShowDialog()
        End Using
    End Sub
End Class