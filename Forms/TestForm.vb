Public Class TestForm
    Sub New()
        InitializeComponent()

    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)

        Dim l As New List(Of StringBooleanPair)

        For x = 0 To 100000
            Dim pa As New StringBooleanPair(x.ToString, x Mod 2 = 0)
            l.Add(pa)
        Next

        ListViewEx1.CheckBoxes = True
        ListViewEx1.EnableListBoxMode()
        ListViewEx1.ItemCheckProperty = NameOf(StringBooleanPair.Value)
        ListViewEx1.AddItems(l)
        ListViewEx1.SelectFirst()
    End Sub
End Class