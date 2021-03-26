
Imports System.ComponentModel
Imports System.Net
Imports System.Net.Http
Imports System.Text.RegularExpressions

Public Class StaxRipUpdate
    Shared HttpClient As New HttpClient

    Shared Sub ShowUpdateQuestion()
        If Not s.CheckForUpdatesQuestion Then
            Using td As New TaskDialog(Of String)()
                td.Title = "Check for updates"
                td.Content = "Would you like StaxRip to check for updates once per day? Each check will only query these sites:" + BR +
                                        "'githubusercontent.com' and " + BR +
                                        "'github.com'"

                td.AddCommand("Yes", "Yes")
                td.AddCommand("No", "No")
                td.AddCommand("Ask me later", "Ask me later")

                Dim answer = td.Show
                s.CheckForUpdatesQuestion = answer <> "Ask me later"
                s.CheckForUpdates = answer = "Yes"
            End Using
        End If
    End Sub

    Shared Async Sub CheckForUpdate(
        Optional force As Boolean = False,
        Optional includeDevBuilds As Boolean = False,
        Optional x64 As Boolean = True)

        Try
            If Not s.CheckForUpdates AndAlso Not force Then
                Exit Sub
            End If

            If (Date.Now - s.CheckForUpdatesLastRequest).TotalHours >= 24 OrElse force Then
                Dim changelogUrl = "https://raw.githubusercontent.com/staxrip/staxrip/master/Changelog.md"
                Dim releaseUrl = "https://github.com/staxrip/staxrip/releases"

                Dim currentVersion = Reflection.Assembly.GetEntryAssembly.GetName.Version
                Dim latestVersions = New List(Of (Version As Version, ReleaseType As String, SourceSite As String, DownloadUri As String, FileName As String))
                Dim response = Await HttpClient.GetAsync(releaseUrl)
                response.EnsureSuccessStatusCode()
                Dim content = Await response.Content.ReadAsStringAsync()
                Dim linkMatches = Regex.Matches(content, "(?<="")/staxrip/staxrip/releases/download/(\d+\.\d+\.\d+(?:\.\d+)?)/(StaxRip-v(\d+\.\d+\.\d+(?:\.\d+)?)[^""]*\.7z)(?="")")

                For Each linkMatch As Match In linkMatches
                    Dim onlineVersion = Version.Parse(linkMatch.Groups(3).Value)

                    If onlineVersion <= currentVersion Then
                        Exit For
                    End If

                    If onlineVersion.Build > 0 Then
                        If includeDevBuilds Then
                            latestVersions.Add((onlineVersion, "DEV version", releaseUrl, "https://github.com" + linkMatch.Groups(0).Value, linkMatch.Groups(2).Value))
                        End If
                    Else
                        latestVersions.Add((onlineVersion, "Release", releaseUrl, "https://github.com" + linkMatch.Groups(0).Value, linkMatch.Groups(2).Value))
                    End If
                Next

                If latestVersions.Count > 0 Then
                    Dim latestVersion = latestVersions.OrderBy(Function(x) x.Version).Last()

                    If latestVersion.Version > currentVersion AndAlso (s.CheckForUpdatesDismissed = "" OrElse
                        Version.Parse(s.CheckForUpdatesDismissed) <> latestVersion.Version OrElse force) Then

                        Using td As New TaskDialog(Of String)
                            td.Title = "A new " + latestVersion.ReleaseType + " was found: v" + latestVersion.Version.ToString()

                            Dim changelogResponse = Await HttpClient.GetAsync(changelogUrl)

                            If changelogResponse.IsSuccessStatusCode Then
                                Dim changelogContent = Await changelogResponse.Content.ReadAsStringAsync()
                                Dim splits = Regex.Split(changelogContent, "\n\n\n")

                                If splits.Any() Then
                                    Dim split = splits.Where(Function(x) x.Contains(latestVersion.Version.ToString()))?.LastOrDefault()

                                    If split <> "" Then
                                        Dim changes = 0

                                        td.Content += "Changes in this version:" + BR

                                        For Each line In Regex.Split(split, "\n")
                                            If changes >= 20 Then
                                                td.Content += "..."
                                                Exit For
                                            ElseIf line.StartsWith("-") Then
                                                line = Regex.Replace(line, "\(/\.\./\.\./issues/\d+\)", "")
                                                td.Content += line + BR
                                                changes += 1
                                            End If
                                        Next
                                    End If
                                End If
                            End If

                            td.AddCommand("Download and save as...", "dl-save-as")
                            td.AddCommand("Download via browser", "dl-browser")
                            td.AddCommand("Open source website", "open")
                            td.AddCommand("Dismiss version " + latestVersion.Version.ToString(), "dismiss")

                            Select Case td.Show
                                Case "dl-save-as"
                                    Dim saveFileDialog = New SaveFileDialog With {
                                        .AddExtension = True,
                                        .AutoUpgradeEnabled = True,
                                        .CheckFileExists = False,
                                        .DefaultExt = "7z",
                                        .FileName = latestVersion.FileName,
                                        .Filter = "7-zip archive (*.7z)|*.7z",
                                        .OverwritePrompt = True,
                                        .Title = "Save new " + latestVersion.ReleaseType + " version as..."
                                    }

                                    If saveFileDialog.ShowDialog() = DialogResult.OK Then
                                        Using client As New WebClient()
                                            AddHandler client.DownloadFileCompleted, AddressOf OnDownloadComplete
                                            client.DownloadFileAsync(New Uri(latestVersion.DownloadUri), saveFileDialog.FileName)

                                            MessageBox.Show("This may take a while." + BR + "You'll be informed when the download finished." + BR2 + "Please do not close this instance till the download is finished!", "Downloading...", MessageBoxButtons.OK)
                                        End Using
                                    End If
                                Case "dl-browser"
                                    g.ShellExecute(latestVersion.DownloadUri)
                                Case "open"
                                    g.ShellExecute(latestVersion.SourceSite)
                                Case "dismiss"
                                    s.CheckForUpdatesDismissed = latestVersion.Version.ToString()
                            End Select
                        End Using
                    ElseIf force Then
                        MsgInfo("No update available.")
                    End If
                ElseIf force Then
                    MsgInfo("No update available.")
                End If

                s.CheckForUpdatesLastRequest = DateTime.Now
            End If
        Catch
        End Try
    End Sub

    Shared Sub OnDownloadComplete(sender As Object, e As AsyncCompletedEventArgs)
        If Not e.Cancelled AndAlso e.Error Is Nothing Then
            MsgInfo("Download successed!")
        Else
            MsgError("Download failed!")
        End If
    End Sub
End Class
