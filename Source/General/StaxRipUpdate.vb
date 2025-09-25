
Imports System.ComponentModel
Imports System.Net
Imports System.Net.Http
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.VisualBasic

Public Class StaxRipUpdate
    Shared HttpClient As New HttpClient

    Shared Sub SetFirstRunOnCurrentVersion()
        Dim key = g.DefaultCommands.GetApplicationDetails()
        If s.FirstRunOnVersion.Key <> key Then
            s.FirstRunOnVersion = New KeyValuePair(Of String, Date)(key, Date.Now)
        End If
    End Sub

    Shared Sub ShowUpdateQuestion()
        If Not g.IsDevelopmentPC AndAlso s.CheckForUpdatesQuestion Then
            Using td As New TaskDialog(Of String)()
                td.Title = "Check for updates"
                td.Icon = TaskIcon.Question
                td.Content = "Would you like StaxRip to check for updates periodically?" + BR +
                             "Each time it is checked, only these websites are queried:" + BR +
                             "'github.com' and " + BR +
                             "'githubusercontent.com'"

                td.AddCommand("Yes")
                td.AddCommand("No")
                td.AddCommand("Ask me later")

                Dim answer = td.Show
                s.CheckForUpdatesQuestion = Not answer.EqualsAny("Yes", "No")
                s.CheckForUpdates = answer = "Yes"
            End Using
        End If
    End Sub

    Shared Async Sub CheckForUpdateAsync(Optional force As Boolean = False, Optional x64 As Boolean = True)
        If g.IsSupporterRelease Then Exit Sub
        If Not s.CheckForUpdates AndAlso Not force Then Exit Sub

        SetFirstRunOnCurrentVersion()

        Dim hours = Conversion.Fix((DateTime.Now - s.FirstRunOnVersion.Value).TotalHours)
        Dim diffHoursToCheck = 24
        diffHoursToCheck = If(hours < 96, 12, diffHoursToCheck)
        diffHoursToCheck = If(hours < 72, 9, diffHoursToCheck)
        diffHoursToCheck = If(hours < 48, 6, diffHoursToCheck)
        diffHoursToCheck = If(hours < 24, 3, diffHoursToCheck)

        Dim proceed = False
        proceed = (Date.Now - s.CheckForUpdatesLastRequest).TotalHours >= diffHoursToCheck OrElse proceed

        If Not (proceed OrElse force) Then Exit Sub

        Try
            'Dim changelogUrl = "https://raw.githubusercontent.com/staxrip/staxrip/master/CHANGELOG.md"
            Const url = "https://api.github.com/repos/staxrip/staxrip/releases?per_page=5"

            Dim currentVersion = Assembly.GetEntryAssembly().GetName().Version

            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Release-Checker")
            Dim response = Await HttpClient.GetAsync(url)
            response.EnsureSuccessStatusCode()
            Dim content = Await response.Content.ReadAsStringAsync()

            Dim linkMatches = Regex.Matches(content, "(?<=""browser_download_url"":"")https://github\.com/staxrip/staxrip/releases/download/(?<tag>v\d\.\d+\.\d+(?:\.\d+)?)/StaxRip-v?(?<version>\d+\.\d+\.\d+(?:\.\d+)?)-x64(?<type>-.+?)?\.7z(?="")")
            Dim latestVersions = New List(Of (Version As Version, ReleaseType As String, ReleaseUri As String, DownloadUri As String))

            For Each linkMatch As Match In linkMatches
                Dim downloadUri = linkMatch.Groups(0).Value
                Dim tag = linkMatch.Groups("tag").Value
                Dim type = linkMatch.Groups("type").Value
                Dim releaseType = If(type = "-UPDATE", "tool including update",
                                    If(type = "-EXE", "hotfix/update", "release"))
                Dim onlineVersionString = linkMatch.Groups("version").Value
                Dim onlineVersion = Version.Parse(onlineVersionString)
                Dim releaseUri = $"https://github.com/staxrip/staxrip/releases/tag/{tag}"

                If onlineVersion <= currentVersion OrElse (s.CheckForUpdatesDismissed <> "" AndAlso Version.Parse(s.CheckForUpdatesDismissed) >= onlineVersion) Then Continue For

                latestVersions.Add((onlineVersion, releaseType, releaseUri, downloadUri))
            Next

            If latestVersions.Any() Then
                Dim sortedVersions = latestVersions.OrderByDescending(Function(x) x.Version).ToList()
                Dim latestVersion = sortedVersions.First()

                Using td As New TaskDialog(Of String)
                    td.Title = "A new " + latestVersion.ReleaseType + " was found: v" + latestVersion.Version.ToString()
                    td.Icon = TaskIcon.Shield

                    td.AddCommand("Open release page", "open")
                    td.AddCommand("Dismiss v" & latestVersion.Version.ToString(), "dismiss")
                    td.AddCommand("Cancel", "cancel")

                    Select Case td.Show()
                        Case "open"
                            g.ShellExecute(latestVersion.ReleaseUri)
                        Case "dismiss"
                            s.CheckForUpdatesDismissed = latestVersion.Version.ToString()
                    End Select
                End Using
            ElseIf force Then
                MsgInfo("No update available.")
            End If

            s.CheckForUpdatesLastRequest = DateTime.Now
        Catch
        End Try
    End Sub
End Class
