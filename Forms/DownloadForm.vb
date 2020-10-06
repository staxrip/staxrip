
Imports System.ComponentModel
Imports System.Net

Public Class DownloadForm
    Private WebClient As New WebClient

    Property Path As String

    Sub New(url As String, path As String)
        InitializeComponent()
        Me.Path = path
        AddHandler WebClient.DownloadProgressChanged, AddressOf ProgressChanged
        AddHandler WebClient.DownloadFileCompleted, AddressOf Completed
        Dim filename = url.FileName
        WebClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36")
        WebClient.Headers.Add("Referer", url)
        WebClient.DownloadFileAsync(New Uri(url), path)
    End Sub

    Sub ProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)
        Try
            Dim mb = e.TotalBytesToReceive / 1024 / 1024
            Dim received = e.BytesReceived / 1024 / 1024
            Text = "Download - " + received.ToString("0.#") + " MB of " + mb.ToString("0.#") + " MB"
            ProgressBar.Maximum = CInt(e.TotalBytesToReceive / 1024)
            ProgressBar.Value = CInt(e.BytesReceived / 1024)
        Catch
        End Try
    End Sub

    Sub Completed(sender As Object, e As AsyncCompletedEventArgs)
        If Not e.Cancelled Then
            DialogResult = DialogResult.OK
        End If

        Close()
    End Sub

    Sub bnCancel_Click(sender As Object, e As EventArgs) Handles bnCancel.Click
        WebClient.CancelAsync()
        Close()
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)
        WebClient.Dispose()
    End Sub
End Class
