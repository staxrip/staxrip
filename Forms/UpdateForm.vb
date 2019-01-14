Imports HtmlAgilityPack
Imports System.Text.RegularExpressions
Imports System.Net
Imports System.IO.Compression

Public Class UpdateForm
    Public WithEvents Progress As New WebClient
    Private Sub Update_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Update()
        Catch ex As Exception
            Me.Close()
        End Try
    End Sub
    Private Sub Update()
        Try
            Dim html = "https://github.com/Revan654/staxrip/releases/latest"
            Dim web As HtmlWeb = New HtmlWeb()
            Dim htmlDoc = web.Load(html)
            Try
                Dim node = htmlDoc.DocumentNode.SelectSingleNode("//strong[@class='pl-1 pr-2 flex-auto min-width-0']")
                Dim Export As String = node.OuterHtml
                Dim match = Regex.Match(Export, ">(.*?)<")
                If match.Success Then
                    Dim Tag = match.Groups(1).Value.ToString().Replace("Staxrip.", "").Replace(".x64.rar", "")
                    Dim FilePath = Folder.Startup + "\" + match.Groups(1).Value.ToString()
                    Dim Results = "https://github.com/Revan654/staxrip/releases/download/" + Tag + "/" + match.Groups(1).Value.ToString()
                    Try
                        Dim Version = My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build & "." & My.Application.Info.Version.Revision
                        If Version < Tag Then
                            Progress.DownloadFileAsync(New Uri(Results), FilePath)
                        Else
                            MsgInfo("Already Running the Latest Version", Nothing)
                            Me.Close()
                        End If
                    Catch ex As Exception
                        Me.Close()
                    End Try
                End If
            Catch ex As Exception
                Me.Close()
            End Try
        Catch ex As Exception
            Me.Close()
        End Try
    End Sub

    Private Sub ScrappyProgressBar_Report(sender As Object, e As DownloadProgressChangedEventArgs) Handles Progress.DownloadProgressChanged
        Try
            ScrappyProgressBar.Value = e.ProgressPercentage
        Catch ex As Exception
            Me.Close()
        End Try
    End Sub
End Class