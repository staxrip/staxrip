Imports System.Globalization
Imports System.Reflection
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text
Imports System.ComponentModel

Imports Microsoft.VisualBasic.FileIO
Imports System.Management
Imports System.Text.RegularExpressions

Public Class ObjectHelp
    'parse recursive serializable fields and lists 
    Shared Function GetCompareString(obj As Object) As String
        Dim sb As New StringBuilder
        ParseCompareString(obj, Nothing, sb)
        Return sb.ToString
    End Function

    Shared Sub ParseCompareString(obj As Object, declaringObj As Object, sb As StringBuilder)
        If TypeOf obj Is ICollection Then
            For Each i In DirectCast(obj, ICollection)
                If IsGoodType(i) Then
                    If IsToString(i) Then
                        sb.Append(i.ToString)
                    Else
                        ParseCompareString(i, obj, sb)
                    End If
                End If
            Next
        Else
            If IsGoodType(obj) Then
                Dim t = obj.GetType

                While Not t Is Nothing
                    For Each i As FieldInfo In t.GetFields(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.DeclaredOnly)
                        If Not i.IsNotSerialized Then
                            Dim o = i.GetValue(obj)

                            If IsGoodType(o) Then
                                If IsToString(o) Then
                                    sb.Append(i.Name + "=" + o.ToString + BR)
                                Else
                                    If Not o Is declaringObj Then
                                        ParseCompareString(o, obj, sb)
                                    End If
                                End If
                            End If
                        End If
                    Next

                    t = t.BaseType
                End While
            End If
        End If
    End Sub

    Private Shared Function IsGoodType(o As Object) As Boolean
        If o Is Nothing Then Return False
        If TypeOf o Is Pointer Then Return False
        If TypeOf o Is PropertyChangedEventHandler Then Return False
        If Not o.GetType.IsSerializable Then Return False
        Return True
    End Function

    Private Shared Function IsToString(o As Object) As Boolean
        If Not o Is Nothing Then
            If o.GetType.IsPrimitive Then Return True
            If TypeOf o Is String Then Return True
            'some fields change here
            If TypeOf o Is CultureInfo Then Return True
            'some fields change here
            If TypeOf o Is StringBuilder Then Return True
        End If
    End Function

    <DebuggerHidden()>
    Shared Function GetCopy(Of T)(o As T) As T
        Using ms As New MemoryStream
            Dim bf As New BinaryFormatter
            bf.Serialize(ms, o)
            ms.Position = 0
            Return DirectCast(bf.Deserialize(ms), T)
        End Using
    End Function
End Class

Public Class DirectoryHelp
    Shared Sub Delete(filepath As String,
                      Optional recycleOption As RecycleOption = RecycleOption.DeletePermanently)

        If Directory.Exists(filepath) Then
            Try
                FileSystem.DeleteDirectory(filepath, UIOption.OnlyErrorDialogs, recycleOption)
            Catch ex As Exception
            End Try
        End If
    End Sub

    Shared Sub Copy(source As String, target As String, Optional opt As UIOption = UIOption.OnlyErrorDialogs)
        Try
            FileSystem.CopyDirectory(source, target, True)
        Catch
            FileSystem.CopyDirectory(source, target, opt, UICancelOption.DoNothing)
        End Try
    End Sub

    Shared Sub Move(source As String, target As String)
        Try
            FileSystem.MoveDirectory(source, target, True)
        Catch ex As Exception
            FileSystem.MoveDirectory(source, target, UIOption.OnlyErrorDialogs, UICancelOption.DoNothing)
        End Try
    End Sub

    Shared Function Compare(dir1 As String, dir2 As String) As Boolean
        Return dir1.FixDir.ToUpper = dir2.FixDir.ToUpper
    End Function
End Class

Public Class ConsoleHelp
    Private Shared DosCodePageValue As Integer

    Shared ReadOnly Property DosCodePage As Integer
        Get
            If DosCodePageValue = 0 Then DosCodePageValue = Regex.Match(ProcessHelp.GetStdOut("cmd.exe", "/C CHCP"), "\d+").Value.ToInt
            Return DosCodePageValue
        End Get
    End Property
End Class

Public Class FileHelp
    Shared Sub Move(src As String, dest As String)
        If File.Exists(src) Then
            If File.Exists(dest) Then Delete(dest)
            FileSystem.MoveFile(src, dest, UIOption.OnlyErrorDialogs, UICancelOption.DoNothing)
        End If
    End Sub

    Shared Sub Copy(src As String, dest As String, Optional opt As UIOption = UIOption.OnlyErrorDialogs)
        If Not File.Exists(src) Then Exit Sub
        If File.Exists(dest) Then Delete(dest)
        FileSystem.CopyFile(src, dest, opt, UICancelOption.DoNothing)
    End Sub

    Shared Sub Delete(path As String, Optional recycleOption As RecycleOption = RecycleOption.DeletePermanently)
        If File.Exists(path) Then FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, recycleOption, UICancelOption.DoNothing)
    End Sub
End Class

Public Class ProcessHelp
    Shared Function GetStdOut(file As String, arguments As String) As String
        Dim ret = ""
        Dim proc As New Process
        proc.StartInfo.UseShellExecute = False
        proc.StartInfo.CreateNoWindow = True
        proc.StartInfo.RedirectStandardOutput = True
        proc.StartInfo.FileName = file
        proc.StartInfo.Arguments = arguments
        proc.Start()
        ret = proc.StandardOutput.ReadToEnd()
        proc.WaitForExit()
        Return ret
    End Function

    Shared Function GetErrOut(file As String, arguments As String) As String
        Dim ret = ""
        Dim proc As New Process
        proc.StartInfo.UseShellExecute = False
        proc.StartInfo.CreateNoWindow = True
        proc.StartInfo.RedirectStandardError = True
        proc.StartInfo.FileName = file
        proc.StartInfo.Arguments = arguments
        proc.Start()
        ret = proc.StandardError.ReadToEnd()
        proc.WaitForExit()
        Return ret
    End Function

    Private Sub KillProcessAndChildren(pid As Integer)
        Dim searcher As New ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" & pid)
        Dim moc As ManagementObjectCollection = searcher.[Get]()
        For Each mo As ManagementObject In moc
            KillProcessAndChildren(Convert.ToInt32(mo("ProcessID")))
        Next
        Try
            Dim proc As Process = Process.GetProcessById(pid)
            proc.Kill()
            ' process already exited 
        Catch generatedExceptionName As ArgumentException
        End Try
    End Sub

    Shared Function GetChilds(inputProcess As Process) As List(Of Process)
        Dim searcher As New ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" & inputProcess.Id)
        Dim ret As New List(Of Process)

        For Each i As ManagementObject In searcher.Get()
            ret.Add(Process.GetProcessById(CInt(i("ProcessID"))))
        Next

        Return ret
    End Function
End Class

Public Class CommandLineHelp
    Public Shared Function ConvertText(val As String) As String
        If val = "" Then Return ""
        Return Macro.Expand(val).Replace("""", "'").Trim
    End Function
End Class