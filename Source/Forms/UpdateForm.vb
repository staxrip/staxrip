Imports System.Net

Public Class UpdateForm
    Public WithEvents Progress As New WebClient
    Private Sub Update_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Update()
        Catch ex As Exception
            MsgInfo(ex.Message)
            Close()
        End Try
    End Sub

    Private Async Sub Update()
        Dim NextVersion = "2.0.0.1"
        Dim Version = My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build & "." & My.Application.Info.Version.Revision

        If Version < NextVersion Then
            Try
                Await Progress.DownloadFileTaskAsync(New Uri("https://github.com/Revan654/staxrip/releases/download/2.0.0.0/Staxrip.2.0.0.1.x64.rar"), Path.Combine(Folder.Startup, "StaxRip.rar"))
                MsgInfo("Download Complete")
                g.OpenDirAndSelectFile(Path.Combine(Folder.Startup, "StaxRip.rar"), g.MainForm.Handle)
                Close()
            Catch ex As WebException

            Finally
                MsgInfo("Already Running the Latest Version!", Nothing)
                File.Delete(Path.Combine(Folder.Startup, "StaxRip.rar"))
                Close()
            End Try

        Else
            MsgInfo("Already Running the Latest Version", Nothing)
            Close()
        End If
    End Sub

    Private Sub ScrappyProgressBar_Report(sender As Object, e As DownloadProgressChangedEventArgs) Handles Progress.DownloadProgressChanged
        Try
            ScrappyProgressBar.Value = e.ProgressPercentage
        Catch ex As Exception
        End Try
    End Sub
End Class