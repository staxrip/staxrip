Public Class DataForm
    Property HelpAction As Action

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub DataForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        If Not HelpAction Is Nothing Then HelpAction.Invoke
    End Sub
End Class