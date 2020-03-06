
Public Class TestForm
    Sub New()
        InitializeComponent()
    End Sub

    Shared Sub ShowForm()
        Using form As New TestForm
            form.ShowDialog()
        End Using
    End Sub

    Private Sub Button1_Paint(sender As Object, e As PaintEventArgs) Handles Button1.Paint
        Dim g = e.Graphics
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        Dim sf As New StringFormat
        sf.Alignment = StringAlignment.Center
        sf.LineAlignment = StringAlignment.Center
        Dim r = Button1.ClientSize
        Dim rf As New RectangleF(0, 0, r.Width, r.Height)
        g.DrawString("❯]", New Font("Segoe UI Symbol", 14), Brushes.Black, rf, sf)
    End Sub
End Class
