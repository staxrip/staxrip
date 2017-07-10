Public Class TextHelpForm
    Property Find As String

    Public Sub New(text As String, find As String)
        InitializeComponent()
        rtb.Text = text
        rtb.BackColor = Color.FromArgb(255, 1, 36, 86)
        rtb.ForeColor = Color.FromArgb(255, 238, 237, 240)
        rtb.ReadOnly = True
        Me.Find = find
        ScaleClientSize(40, 30)
    End Sub

    Private Sub TextHelpForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        rtb.Find(Find)
        rtb.ScrollToCaret()
    End Sub
End Class