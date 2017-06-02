Class TestForm
    Sub New()
        InitializeComponent()
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)

        Dim l As New List(Of String)

        For x = 0 To 1000
            l.Add(x.ToString)
        Next

        ListBoxEx1.Items.AddRange(l.ToArray)
    End Sub
End Class