Option Strict Off

Imports Microsoft.VisualBasic

Public Class Explorer
    Shared ShellApp As Object = CreateObject("Shell.Application")

    Shared Sub SelectFile(handle As IntPtr, filepath As String)
        For Each i In ShellApp.Windows
            If TypeName(i) = "IWebBrowser2" AndAlso i.HWND = handle.ToInt32 Then
                'two shorter methods tried to get correct FolderItem but didn't work
                For Each i2 In i.Document.Folder.Items()
                    If i2.Path = filepath Then
                        i.Document.SelectItem(i2, 1 + 4 + 8)
                        Exit Sub
                    End If
                Next
            End If
        Next
    End Sub
End Class