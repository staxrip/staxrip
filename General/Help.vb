Imports Microsoft.Win32
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms
Imports System.Security.Cryptography
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Xml

Imports Microsoft.VisualBasic.FileIO
Imports StaxRip.UI

Public Class ObjectHelp
    'parse recursive serializable fields and lists 
    Shared Function GetCompareString(obj As Object) As String
        Dim sb As New StringBuilder
        ParseCompareString(obj, Nothing, sb)
        Return sb.ToString
    End Function

    Shared Sub ParseCompareString(obj As Object, declaringObj As Object, sb As StringBuilder)
        If TypeOf obj Is ICollection Then
            For Each i As Object In DirectCast(obj, ICollection)
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
                                    sb.Append(i.Name + "=" + o.ToString + CrLf)
                                    'sb.Append(i.Name + "=" + o.ToString + " [" + t.Name + "]" + CrLf)
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
        If o Is Nothing Then
            Return False
        End If

        If TypeOf o Is Pointer Then
            Return False
        End If

        If Not o.GetType.IsSerializable Then
            Return False
        End If

        Return True
    End Function

    Private Shared Function IsToString(o As Object) As Boolean
        If Not o Is Nothing Then
            If o.GetType.IsPrimitive Then
                Return True
            End If

            If TypeOf o Is String Then
                Return True
            End If

            If TypeOf o Is CultureInfo Then 'some fields change here
                Return True
            End If

            If TypeOf o Is StringBuilder Then 'some fields change here
                Return True
            End If
        End If
    End Function

    <DebuggerHidden()>
    Shared Function GetCopy(o As Object) As Object
        Using ms As New MemoryStream
            Dim bf As New BinaryFormatter
            bf.Serialize(ms, o)
            ms.Position = 0
            Return bf.Deserialize(ms)
        End Using
    End Function

    <DebuggerHidden()>
    Shared Function GetCopy(Of T)(o As Object) As T
        Using ms As New MemoryStream
            Dim bf As New BinaryFormatter
            bf.Serialize(ms, o)
            ms.Position = 0
            Return DirectCast(bf.Deserialize(ms), T)
        End Using
    End Function

    Shared Sub Serialize(o As Object, path As String)
        Dim fs As FileStream = New FileStream(path, FileMode.OpenOrCreate)
        Dim bf As BinaryFormatter = New BinaryFormatter
        bf.Serialize(fs, o)
        fs.Close()
    End Sub

    Shared Function Deserialize(path As String) As Object
        Dim fs As FileStream = New FileStream(path, FileMode.Open)
        Dim bf As BinaryFormatter = New BinaryFormatter
        Dim o As Object = bf.Deserialize(fs)
        fs.Close()
        Return o
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

    Shared Sub Copy(source As String, target As String)
        Try
            FileSystem.CopyDirectory(source, target, True)
        Catch
            FileSystem.CopyDirectory(source, target, UIOption.OnlyErrorDialogs, UICancelOption.DoNothing)
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
        Return DirPath.AppendSeparator(dir1).ToUpper = DirPath.AppendSeparator(dir2).ToUpper
    End Function

    Shared Function FindFiles(startDir As String,
                              filename As String,
                              Optional filePaths As List(Of String) = Nothing) As List(Of String)

        If filePaths Is Nothing Then
            filePaths = New List(Of String)
        End If

        If Directory.Exists(startDir) Then
            Dim di As New DirectoryInfo(startDir)

            For Each i As FileSystemInfo In di.GetFileSystemInfos()
                If TypeOf i Is FileInfo Then
                    If Filepath.GetName(i.FullName).EqualIgnoreCase(filename) Then
                        filePaths.Add(i.FullName)
                    End If
                Else
                    FindFiles(i.FullName, filename, filePaths)
                End If
            Next
        End If

        Return filePaths
    End Function

    <DebuggerNonUserCode()>
    Shared Sub FileCallback(startDir As String, callback As Action(Of String), cancelArgs As CancelEventArgs)
        Try
            Dim di As New DirectoryInfo(startDir)

            For Each i As FileSystemInfo In di.GetFileSystemInfos()
                If cancelArgs.Cancel Then
                    Exit Sub
                End If

                If TypeOf i Is FileInfo Then
                    callback(i.FullName)
                Else
                    FileCallback(i.FullName, callback, cancelArgs)
                End If
            Next
        Catch
        End Try
    End Sub
End Class

Public Class FileHelp
    Shared Sub Move(src As String, dest As String)
        If File.Exists(dest) Then
            Delete(dest)
        End If

        Try
            FileSystem.MoveFile(src, dest, True)
        Catch
            FileSystem.MoveFile(src, dest, UIOption.OnlyErrorDialogs, UICancelOption.DoNothing)
        End Try
    End Sub

    Shared Sub Copy(src As String, dest As String)
        If Not File.Exists(src) Then
            Exit Sub
        End If

        If File.Exists(dest) Then
            Delete(dest)
        End If

        Try
            FileSystem.CopyFile(src, dest, True)
        Catch
            FileSystem.CopyFile(src, dest, UIOption.OnlyErrorDialogs, UICancelOption.DoNothing)
        End Try
    End Sub

    Shared Sub Delete(path As String,
                      Optional recycleOption As RecycleOption = RecycleOption.DeletePermanently)

        If File.Exists(path) Then
            FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, recycleOption, UICancelOption.DoNothing)
        End If
    End Sub
End Class

Public Class ProcessHelp
    Shared Function GetStandardOutput(file As String, arguments As String) As String
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

    Shared Function GetErrorOutput(file As String, arguments As String) As String
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
End Class