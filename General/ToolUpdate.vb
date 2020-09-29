
Imports System.Net.Http
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks

Public Class ToolUpdate
    Property DownloadPage As String
    Property HttpClient As New HttpClient

    Async Function GetDownloadURL() As Task(Of String)
        Dim page = Await HttpClient.GetStringAsync(DownloadPage)
        Dim match = Regex.Match("", "href="".*(\.7z|\.zip|exe)""")
        Return page
    End Function
End Class
