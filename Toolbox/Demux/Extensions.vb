Imports System.Runtime.CompilerServices
Imports Microsoft.Win32

Public Module StringExtensions
    <Extension()>
    Function Dir(instance As String) As String
        Return Filepath.GetDir(instance)
    End Function

    <Extension()>
    Function RemoveIllegal(instance As String) As String
        Return Filepath.RemoveIllegal(instance)
    End Function

    <Extension()>
    Function FileName(instance As String) As String
        If instance = "" Then Return ""
        Dim index = instance.LastIndexOf("\")
        If index > -1 Then Return instance.Substring(index + 1)
        Return instance
    End Function

    <Extension()>
    Function Quotes(instance As String) As String
        If instance = "" Then Return ""
        If instance.Contains(" ") Then Return """" + instance + """"
        Return instance
    End Function

    <Extension()>
    Function Ext(instance As String) As String
        Return Filepath.GetExt(instance)
    End Function

    <Extension()>
    Function Base(instance As String) As String
        Return Filepath.GetBase(instance)
    End Function

    <Extension()>
    Function EqualsAny(instance As String, ParamArray values As String()) As Boolean
        If instance = "" OrElse values.NothingOrEmpty Then Return False
        Return values.Contains(instance)
    End Function

    <Extension()>
    Function AppendSeparator(instance As String) As String
        If instance?.EndsWith(DirPath.Separator) Then Return instance
        Return instance + DirPath.Separator
    End Function

    <Extension()>
    Function ContainsUnicode(value As String) As Boolean
        For Each i In value
            If Convert.ToInt32(i) > 255 Then Return True
        Next
    End Function

    <Extension()>
    Function IsInt(value As String) As Boolean
        Return Integer.TryParse(value, Nothing)
    End Function

    <Extension()>
    Function ToInt(value As String, Optional defaultValue As Integer = 0) As Integer
        If Not Integer.TryParse(value, Nothing) Then Return defaultValue
        Return CInt(value)
    End Function

    <Extension()>
    Function IsSingle(value As String) As Boolean
        If value <> "" Then
            If value.Contains(",") Then value = value.Replace(",", ".")

            Return Single.TryParse(value,
                                   NumberStyles.Float Or NumberStyles.AllowThousands,
                                   CultureInfo.InvariantCulture,
                                   Nothing)
        End If
    End Function

    <Extension()>
    Function IsDouble(value As String) As Boolean
        If value <> "" Then
            If value.Contains(",") Then value = value.Replace(",", ".")

            Return Double.TryParse(value,
                                   NumberStyles.Float Or NumberStyles.AllowThousands,
                                   CultureInfo.InvariantCulture,
                                   Nothing)
        End If
    End Function

    <Extension()>
    Function ToDouble(value As String, Optional defaultValue As Single = 0) As Double
        If value <> "" Then
            If value.Contains(",") Then value = value.Replace(",", ".")

            Dim ret As Double

            If Double.TryParse(value,
                               NumberStyles.Float Or NumberStyles.AllowThousands,
                               CultureInfo.InvariantCulture,
                               ret) Then
                Return ret
            End If
        End If

        Return defaultValue
    End Function

    <Extension()>
    Function LeftLast(value As String, start As String) As String
        If Not value.Contains(start) Then Return ""
        Return value.Substring(0, value.LastIndexOf(start))
    End Function

    <Extension()>
    Function Right(value As String, start As String) As String
        If Not value.Contains(start) Then
            Return ""
        End If

        Return value.Substring(value.IndexOf(start) + start.Length)
    End Function

    <Extension()>
    Function RightLast(value As String, start As String) As String
        If Not value.Contains(start) Then Return ""
        Return value.Substring(value.LastIndexOf(start) + start.Length)
    End Function

    <Extension()>
    Function Shorten(value As String, maxLength As Integer) As String
        If value = "" OrElse value.Length <= maxLength Then
            Return value
        End If

        Return value.Substring(0, maxLength)
    End Function

    <Extension()>
    Function SplitNoEmpty(value As String, ParamArray delimiters As String()) As String()
        Return value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
    End Function

    <Extension()>
    Function SplitLinesNoEmpty(value As String) As String()
        Return SplitNoEmpty(value, Environment.NewLine)
    End Function
End Module

Public Module MiscExtensions
    <Extension()>
    Function NeutralCulture(ci As CultureInfo) As CultureInfo
        If ci.IsNeutralCulture Then Return ci Else Return ci.Parent
    End Function

    <Extension()>
    Function NothingOrEmpty(strings As IEnumerable(Of String)) As Boolean
        If strings Is Nothing OrElse strings.Count = 0 Then Return True

        For Each i In strings
            If i = "" Then Return True
        Next
    End Function
End Module

Public Module RegistryKeyExtensions
    Private Function GetValue(Of T)(rootKey As RegistryKey, key As String, name As String) As T
        Using k = rootKey.OpenSubKey(key)
            If Not k Is Nothing Then
                Dim r = k.GetValue(name)

                If Not r Is Nothing Then
                    Try
                        Return CType(r, T)
                    Catch ex As Exception
                    End Try
                End If
            End If
        End Using
    End Function

    <Extension()>
    Function GetString(rootKey As RegistryKey, subKey As String, name As String) As String
        Return GetValue(Of String)(rootKey, subKey, name)
    End Function

    <Extension()>
    Function GetValueNames(rootKey As RegistryKey, subKeyName As String) As IEnumerable(Of String)
        Using k = rootKey.OpenSubKey(subKeyName)
            If Not k Is Nothing Then Return k.GetValueNames
        End Using

        Return {}
    End Function

    <Extension()>
    Sub GetSubKeys(rootKey As RegistryKey, keys As List(Of RegistryKey))
        If Not rootKey Is Nothing Then
            keys.Add(rootKey)

            For Each i In rootKey.GetSubKeyNames
                GetSubKeys(rootKey.OpenSubKey(i), keys)
            Next
        End If
    End Sub

    <Extension()>
    Sub Write(rootKey As RegistryKey, subKey As String, valueName As String, valueValue As Object)
        Dim k = rootKey.OpenSubKey(subKey, True)

        If k Is Nothing Then
            k = rootKey.CreateSubKey(subKey, RegistryKeyPermissionCheck.ReadWriteSubTree)
        End If

        k.SetValue(valueName, valueValue)
        k.Close()
    End Sub
End Module

Public Module ControlExtension
    <Extension()>
    Function ClientMousePos(instance As Control) As Point
        Return instance.PointToClient(Control.MousePosition)
    End Function
End Module

Public Module UIExtensions
    <Extension()>
    Sub SetSelectedPath(d As FolderBrowserDialog, path As String)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)
        If Not Directory.Exists(path) Then path = DirPath.GetParent(path)

        If Directory.Exists(path) Then d.SelectedPath = path
    End Sub
End Module