Imports System.Runtime.CompilerServices
Imports System.Reflection
Imports System.IO
Imports StaxRip.UI
Imports System.Drawing.Drawing2D

Public Module ControlExtensions
    <Extension()>
    Sub AddClickAction(c As Control, action As action)
        AddHandler c.Click, Sub() action()
    End Sub

    <Extension()>
    Function ClientMousePos(c As Control) As Point
        Return c.PointToClient(Control.MousePosition)
    End Function

    <Extension()>
    Sub SetSelectedPath(d As FolderBrowserDialog, path As String)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)

        If Directory.Exists(path) Then
            d.SelectedPath = path
        End If
    End Sub

    <Extension()>
    Sub SetInitDir(d As FileDialog, ParamArray paths As String())
        For Each i In paths
            If Not Directory.Exists(i) Then i = DirPath.GetParent(i)
            If Not Directory.Exists(i) Then i = DirPath.GetParent(i)
            If Not Directory.Exists(i) Then i = DirPath.GetParent(i)
            If Not Directory.Exists(i) Then i = DirPath.GetParent(i)
            If Not Directory.Exists(i) Then i = DirPath.GetParent(i)

            If Directory.Exists(i) Then
                d.InitialDirectory = i
                Exit For
            End If
        Next
    End Sub

    <Extension()>
    Sub SetFilter(d As FileDialog, ParamArray value As String())
        d.Filter = "*." + value.Join(";*.") + "|*." + value.Join(";*.") + "|All Files|*.*"
    End Sub

    <Extension()>
    Function RemoveAndDispose(c As Control, ParamArray controls As Control()) As Point
        For Each i In controls
            c.Controls.Remove(i)
            i.Dispose()
        Next
    End Function

    <Extension()>
    Sub SendMessageCue(tb As TextBox, value As String, hideWhenFocused As Boolean)
        Dim wParam = If(hideWhenFocused, 0, 1)
        Native.SendMessage(tb.Handle, Native.EM_SETCUEBANNER, wParam, value)
    End Sub

    <Extension()>
    Sub SendMessageCue(c As ComboBox, value As String)
        Native.SendMessage(c.Handle, Native.CB_SETCUEBANNER, 1, value)
    End Sub
End Module