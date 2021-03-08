
Public Class DataForm
    Property HelpAction As Action

    Sub New()
        InitializeComponent()
    End Sub

    Sub DataForm_HelpRequested(sender As Object, args As HelpEventArgs) Handles Me.HelpRequested
        HelpAction?.Invoke
    End Sub
End Class
