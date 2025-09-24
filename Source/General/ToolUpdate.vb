﻿
Imports System.Net.Http
Imports System.Text.RegularExpressions

Imports Microsoft.VisualBasic
Imports StaxRip.UI

Public Class ToolUpdate
    Property Package As Package
    Property DownloadFile As String
    Property ExtractDir As String
    Property TargetDir As String
    Property UseCurl As Boolean

    Private HttpClient As New HttpClient
    Private UpdateUI As IUpdateUI

    Sub New(pack As Package, updateUI As IUpdateUI)
        Package = pack
        TargetDir = pack.Directory
        Me.UpdateUI = updateUI
    End Sub

    Async Sub Update()
        Dim content = Await HttpClient.GetStringAsync(Package.DownloadURL)
        Dim matches = Regex.Matches(content, "href=(""|')[^ ]+\.(7z|zip|exe)(""|')")

        For Each match As Match In matches
            Dim url = match.Value

            If Ignore(url) Then Continue For
            If Package.Include <> "" AndAlso Not url.Contains(Package.Include) Then Continue For

            url = url.Substring(6, url.Length - 7)

            If Not url.StartsWith("http") Then
                Dim match2 = Regex.Match(Package.DownloadURL, "https?://[^/]+")
                url = match2.Value + If(url.StartsWith("/"), "", "/") + url
            End If

            DownloadFile = IO.Path.Combine(Folder.Desktop, IO.Path.GetFileName(url))
            Download(url)
            Exit For
        Next
    End Sub

    Sub Download(url As String)
        'TaskDialog trims URLs
        If MessageBox.Show("Download the file shown below?" + BR2 + url,
            "StaxRip", MessageBoxButtons.OKCancel,
            MessageBoxIcon.Question) = DialogResult.OK Then

            Using form As New DownloadForm(url, DownloadFile)
                If form.ShowDialog() = DialogResult.OK AndAlso DownloadFile.FileExists Then
                    If DownloadFile.FileExists Then
                        Extract()
                    Else
                        MsgError("Downloaded file is missing.")
                    End If
                Else
                    FileHelp.Delete(DownloadFile)
                    MsgInfo("Download was canceled or failed.")
                End If
            End Using
        End If
    End Sub

    Sub Extract()
        If DownloadFile.Ext <> "7z" AndAlso DownloadFile.Ext <> "zip" Then
            Exit Sub
        End If

        ExtractDir = Path.Combine(DownloadFile.Dir, DownloadFile.Base)

        Using pr As New Process
            pr.StartInfo.FileName = Package.SevenZip.Path
            pr.StartInfo.Arguments = "x -y " + DownloadFile.Escape + " -o""" + ExtractDir + """"
            pr.StartInfo.UseShellExecute = False
            pr.Start()
            pr.WaitForExit()

            If pr.ExitCode <> 0 Then
                UpdatePackageDialog()
                MsgError("Extraction failed with error exit code " & pr.ExitCode)
                Exit Sub
            End If
        End Using

        If Not File.Exists(Path.Combine(ExtractDir, Package.Filename)) Then
            Dim subDirs As New List(Of String)

            For Each subDir In Directory.GetDirectories(ExtractDir, "*", SearchOption.AllDirectories)
                If (Path.Combine(subDir, Package.Filename)).FileExists AndAlso Not Ignore(subDir) Then
                    subDirs.Add(subDir)
                End If
            Next

            If subDirs.Count > 1 Then
                UpdatePackageDialog()

                Using td As New TaskDialog(Of String)
                    td.Title = "Choose subfolder to extract."

                    For Each subDir In subDirs
                        Dim name = subDir.Replace(ExtractDir, "").TrimEnd(Path.DirectorySeparatorChar)
                        td.AddCommand(name, subDir)
                    Next

                    If td.Show.DirExists Then
                        ExtractDir = td.SelectedValue
                    End If
                End Using
            ElseIf subDirs.Count = 1 Then
                ExtractDir = subDirs(0)
            End If
        End If

        If Not (Path.Combine(ExtractDir, Package.Filename)).FileExists Then
            UpdatePackageDialog()
            MsgError("File missing after extraction.")
            Exit Sub
        End If

        DeleteOldFiles()
    End Sub

    Sub DeleteOldFiles()
        Dim entries = Directory.GetFileSystemEntries(TargetDir)
        entries = entries.Where(Function(item) Not item.FileName.EqualsAny(Package.Keep)).ToArray
        Dim names = entries.Select(Function(item) item.FileName)
        Dim list = String.Join(BR, names)
        UpdatePackageDialog()

        If MsgQuestion("Delete current files?",
            "Delete current files in:" + BR2 + TargetDir + BR2 + list) = DialogResult.OK Then

            For Each file In Directory.GetFiles(TargetDir)
                If file.FileName.EqualsAny(Package.Keep) Then
                    Continue For
                End If

                FileHelp.Delete(file, FileIO.RecycleOption.SendToRecycleBin)
            Next

            For Each folder In Directory.GetDirectories(TargetDir)
                If folder.FileName.EqualsAny(Package.Keep) Then
                    Continue For
                End If

                FolderHelp.Delete(folder, FileIO.RecycleOption.SendToRecycleBin)
            Next
        Else
            UpdatePackageDialog()
            MsgInfo("Update was canceled.")
            Exit Sub
        End If

        CopyFiles()
    End Sub

    Sub CopyFiles()
        Dim entries = Directory.GetFileSystemEntries(ExtractDir)
        Dim names = entries.Select(Function(item) item.FileName)
        Dim list = String.Join(BR, names)
        UpdatePackageDialog()

        If MsgQuestion("Copy new files?",
            "Copy new files from:" + BR2 + ExtractDir + BR2 + "to:" + BR2 +
            TargetDir + BR2 + list) = DialogResult.OK Then

            For Each file In Directory.GetFiles(ExtractDir)
                FileHelp.Copy(file, Path.Combine(TargetDir, file.FileName))
            Next

            For Each folder In Directory.GetDirectories(ExtractDir)
                FolderHelp.Copy(folder, Path.Combine(TargetDir, folder.FileName))
            Next
        Else
            UpdatePackageDialog()
            MsgInfo("Update was canceled.")
            Exit Sub
        End If

        FolderHelp.Delete(ExtractDir, FileIO.RecycleOption.SendToRecycleBin)
        EditVersion()
    End Sub

    Sub EditVersion()
        Dim msg = "What's the name of the new version?" + BR2 + DownloadFile.FileName

        UpdatePackageDialog()
        Dim input = InputBox.Show(msg, DownloadFile.Base)

        If input <> "" Then
            Package.SetVersion(input.Replace(";", "_").Trim)
            UpdatePackageDialog()
            g.DefaultCommands.Test()
        End If
    End Sub

    Function Ignore(value As String) As Boolean
        If value.ContainsAny(Package.Exclude) Then
            Return True
        End If

        Dim x86 = {"_win32", "\x86", "-x86", "32-bit", "-win32"}

        If Environment.Is64BitProcess AndAlso value.ToLowerInvariant.ContainsAny(x86) Then
            Return True
        End If
    End Function

    Sub UpdatePackageDialog()
        UpdateUI.UpdateUI()
    End Sub
End Class
