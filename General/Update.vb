
Imports System.ComponentModel
Imports System.Net
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

    Shared Async Sub CheckForUpdate(Optional force As Boolean = False, Optional includeBeta As Boolean = False, Optional x64 As Boolean = True)
        Try
            If Not s.CheckForUpdates AndAlso Not force Then
                Exit Sub
            End If

            If (DateTime.Now - s.CheckForUpdatesLastRequest).TotalHours >= 24 OrElse force Then
                Dim changelogUrl = "https://raw.githubusercontent.com/staxrip/staxrip/master/Changelog.md"
                Dim betaSourcesUrl = "https://staxrip.readthedocs.io/introduction.html"
                Dim dropboxUrl = "https://www.dropbox.com/sh/4ctl2y928xkak4f/AAADEZj_hFpGQaNOdd3yqcAHa?dl=0&lst="
                Dim stableUrl = "https://github.com/staxrip/staxrip/releases"

                Dim currentVersion = Reflection.Assembly.GetEntryAssembly.GetName.Version
                Dim latestVersions = New List(Of (Version As Version, Status As String,
                    SourceSite As String, DownloadUri As String, FileName As String))

                Dim response = Await HttpClient.GetAsync(stableUrl)

                If response.IsSuccessStatusCode Then
                    Dim content = Await response.Content.ReadAsStringAsync()
                    Dim titleMatch = Regex.Match(content, "title=""(\d+\.\d+\.\d+\.\d+)""")
                    Dim onlineVersion = Version.Parse(titleMatch.Groups(1).Value)
                    Dim linkMatch = Regex.Match(content, "(https://[^""]*/([^""/]*StaxRip[^""?]*)[^""]*)\\""")

                    latestVersions.Add((onlineVersion, "Stable", stableUrl,
                        linkMatch.Groups(1).Value, linkMatch.Groups(2).Value))

                ElseIf Not includeBeta Then
                    response.EnsureSuccessStatusCode()
                End If

                If includeBeta Then
                    Try
                        Dim dropboxResponse = Await HttpClient.GetAsync(dropboxUrl)

                        If Not dropboxResponse.IsSuccessStatusCode Then
                            Dim betaSourcesResponse = Await HttpClient.GetAsync(betaSourcesUrl)
                            Dim betaSourcesContent = Await betaSourcesResponse.Content.ReadAsStringAsync()
                            Dim dropboxMatch = Regex.Match(betaSourcesContent, "(https://www\.dropbox\.com[^""]*)""")

                            If dropboxMatch.Success Then
                                dropboxResponse = Await HttpClient.GetAsync(dropboxMatch.Groups(1).Value)
                            End If
                        End If

                        dropboxResponse.EnsureSuccessStatusCode()
                        Dim dropboxContent = Await dropboxResponse.Content.ReadAsStringAsync()
                        Dim betaPattern = If(x64, "(https://[^""]*/([^""/]*StaxRip[^""/?]*x64[^""?]*)[^""]*)\\""",
                                                  "(https://[^""]*/([^""/]*StaxRip[^""/?]*x86[^""?]*)[^""]*)\\""")
                        Dim betaMatches = Regex.Matches(dropboxContent, betaPattern)

                        If betaMatches.Count > 0 Then
                            Dim sortedMatches = New Match(betaMatches.Count - 1) {}
                            betaMatches.CopyTo(sortedMatches, 0)
                            sortedMatches.OrderBy(Function(x) Regex.Replace(x.Groups(2).Value, "\d+", "$&".PadLeft(11, "0"c)))
                            Dim lastMatch = sortedMatches.Last()

                            Dim uri = lastMatch.Groups(1).Value.Replace("?dl=0", "?dl=1")
                            Dim filename = lastMatch.Groups(2).Value
                            Dim versionMatch = Regex.Match(filename, ".*(\d+\.\d+\.\d+\.\d+).*")
                            Dim betaOnlineVersion = Version.Parse(versionMatch.Groups(1).Value)

                            latestVersions.Add((betaOnlineVersion, "Beta", stableUrl, uri, filename))
                        End If
                    Catch
                    End Try
                End If

                If latestVersions.Count > 0 Then
                    Dim latestVersion = latestVersions.OrderBy(Function(x) x.Version).Last()

                    If latestVersion.Version > currentVersion AndAlso (s.CheckForUpdatesDismissed = "" OrElse
                        Version.Parse(s.CheckForUpdatesDismissed) <> latestVersion.Version OrElse force) Then

                        Using td As New TaskDialog(Of String)
                            td.MainInstruction = "A new " + latestVersion.Status + " version was found: " + latestVersion.Version.ToString()

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
                                                td.Content += line + BR
                                                changes += 1
                                            End If
                                        Next
                                    End If
                                End If
                            End If

                            td.AddCommand("Download and save as...", "downloadsaveas")
                            td.AddCommand("Download via browser", "downloadbrowser")
                            td.AddCommand("Open source website", "open")
                            td.AddCommand("Dismiss version " + latestVersion.Version.ToString(), "dismiss")

                            Select Case td.Show
                                Case "downloadsaveas"
                                    Dim saveFileDialog = New SaveFileDialog With {
                                        .AddExtension = True,
                                        .AutoUpgradeEnabled = True,
                                        .CheckFileExists = False,
                                        .DefaultExt = "7z",
                                        .FileName = latestVersion.FileName,
                                        .Filter = "7-zip archive (*.7z)|*.7z",
                                        .OverwritePrompt = True,
                                        .Title = "Save new " + latestVersion.Status + " version as..."
                                    }

                                    If saveFileDialog.ShowDialog() = DialogResult.OK Then
                                        Using client As New WebClient()
                                            AddHandler client.DownloadFileCompleted, AddressOf OnDownloadComplete
                                            client.DownloadFileAsync(New Uri(latestVersion.DownloadUri), saveFileDialog.FileName)

                                            MessageBox.Show("This may take a while." + BR + "You'll be informed when the download finished." + BR2 + "Please do not close this instance till the download is finished!", "Downloading...", MessageBoxButtons.OK)
                                        End Using
                                    End If
                                Case "downloadbrowser"
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
