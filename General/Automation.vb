Option Strict Off

Imports Microsoft.VisualBasic

Class Explorer
    Shared ShellApp As Object = CreateObject("Shell.Application")

    Shared Sub SelectFile(handle As IntPtr, filepath As String)
        For Each i In ShellApp.Windows
            Dim h = If(Environment.Is64BitOperatingSystem, handle.ToInt64, handle.ToInt32)

            If TypeName(i) = "IWebBrowser2" AndAlso i.HWND = h Then
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