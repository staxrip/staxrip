
Imports System.Runtime.InteropServices

Public Class TestForm
    Private Server As FrameServer
    Private Renderer As VideoRenderer2

    Sub New()
        InitializeComponent()
        Server = New FrameServer("D:\Samples\Jill_temp\Jill_new_preview.avs")
        'Server = New FrameServer("D:\Samples\Jill_temp\Jill_new_preview.vpy")
        TrackBar1.Maximum = Server.Info.FrameCount - 1
    End Sub

    Shared Sub ShowForm()
        Using form As New TestForm
            form.ShowDialog()
        End Using
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Server.Position = TrackBar1.Value
        Renderer.Draw()
    End Sub

    Private Sub TestForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Renderer = New VideoRenderer2(Panel1, Server)
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        Renderer.Draw()
    End Sub

    Private Sub TestForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Server.Dispose()
    End Sub
End Class
