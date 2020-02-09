Option Strict Off

Imports Microsoft.VisualBasic

Public Class ExplorerHelp
    Shared ShellApp As Object = CreateObject("Shell.Application")

    Shared Sub SelectFile(handle As IntPtr, filepath As String)
        For Each i In ShellApp.Windows
            If TypeName(i) = "IWebBrowser2" AndAlso i.HWND = handle.ToInt64 Then
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