
Imports System.Net.Http
Imports System.Text.RegularExpressions

Public Class Update
    Shared HttpClient As New HttpClient

    Shared Sub ShowUpdateQuestion()
        If Not s.CheckForUpdatesQuestion Then
            s.CheckForUpdates = MsgQuestion("Would you like StaxRip to check for updates once per day?",
                TaskDialogButtons.YesNo) = DialogResult.Yes

            s.CheckForUpdatesQuestion = True
        End If
    End Sub

    Shared Async Sub CheckForUpdate(Optional force As Boolean = False)
        Try
            If Not s.CheckForUpdates AndAlso Not force Then
                Exit Sub
            End If

            If (DateTime.Now - s.CheckForUpdatesLastRequest).TotalHours > 24 OrElse force Then
                Dim response = Await HttpClient.GetAsync("https://github.com/staxrip/staxrip/releases")
                response.EnsureSuccessStatusCode()
                Dim content = Await response.Content.ReadAsStringAsync()
                Dim match = Regex.Match(content, "title=""(\d+\.\d+\.\d+\.\d+)""")
                Dim onlineVersion = Version.Parse(match.Groups(1).Value)
                Dim currentVersion = Reflection.Assembly.GetEntryAssembly.GetName.Version

                If onlineVersion > currentVersion AndAlso (s.CheckForUpdatesDismissed = "" OrElse
                    Version.Parse(s.CheckForUpdatesDismissed) <> onlineVersion OrElse force) Then

                    Using td As New TaskDialog(Of String)
                        td.MainInstruction = "A newer version was found online: " + match.Groups(1).Value
                        td.AddCommand("Show download page", "download")
                        td.AddCommand("Dismiss " + match.Groups(1).Value, "dismiss")

                        Select Case td.Show
                            Case "download"
                                g.ShellExecute("https://github.com/staxrip/staxrip/releases")
                            Case "dismiss"
                                s.CheckForUpdatesDismissed = onlineVersion.ToString
                        End Select
                    End Using
                ElseIf force Then
                    MsgInfo("No update available.")
                End If

                s.CheckForUpdatesLastRequest = DateTime.Now
            End If
        Catch
        End Try
    End Sub
End Class
