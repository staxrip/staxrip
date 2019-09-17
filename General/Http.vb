Imports System.Net.Http
Imports System.Text.RegularExpressions

Public Class Http
    Shared HttpClient As New HttpClient

    Shared Sub ShowUpdateQuestion()
        If Not s.CheckForUpdatesQuestion Then
            s.CheckForUpdates = MsgQuestion("Would you like StaxRip to check for updates once per day.",
                                            TaskDialogButtons.YesNo) = DialogResult.Yes
            s.CheckForUpdatesQuestion = True
        End If
    End Sub

    Shared Async Sub CheckForUpdates()
        Try
            If Not s.CheckForUpdates Then Exit Sub

            If (DateTime.Now - s.CheckForUpdatesLastRequest).TotalHours > 24 Then
                Dim response = Await HttpClient.GetAsync("https://github.com/staxrip/staxrip/releases")
                response.EnsureSuccessStatusCode()
                Dim content = Await response.Content.ReadAsStringAsync()
                Dim match = Regex.Match(content, "title=""(\d\.\d\.\d\.\d)""")
                If Not match.Success Then Throw New Exception("regex failure")
                Dim onlineVersion = match.Groups(1).Value.Replace(".", "").ToInt
                Dim currentVersion = Application.ProductVersion.Replace(".", "").ToInt
                If currentVersion > 9000 Then Throw New Exception("invalid version scheme")

                If onlineVersion > currentVersion AndAlso onlineVersion <> s.CheckForUpdatesDismissedVersion Then
                    Using td As New TaskDialog(Of String)
                        td.MainInstruction = "A newer version was found online: " + match.Groups(1).Value
                        td.AddCommandLink("Show download page", "dl")
                        td.AddCommandLink("Dismiss " + match.Groups(1).Value, "dismiss")

                        Select Case td.Show
                            Case "dl"
                                Process.Start("https://github.com/staxrip/staxrip/releases")
                            Case "dismiss"
                                s.CheckForUpdatesDismissedVersion = onlineVersion
                        End Select
                    End Using
                End If

                s.CheckForUpdatesLastRequest = DateTime.Now
            End If
        Catch
        End Try
    End Sub
End Class