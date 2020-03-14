
Public Class TextHelpForm
    Property Find As String

    Public Sub New(text As String, find As String)
        InitializeComponent()
        rtb.Text = text
        rtb.BackColor = Color.Black
        rtb.ForeColor = Color.White
        rtb.ReadOnly = True
        Me.Find = find
        ScaleClientSize(45, 30)
    End Sub

    Private Sub TextHelpForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        rtb.Find(Find)
        rtb.ScrollToCaret()
    End Sub
End Class
