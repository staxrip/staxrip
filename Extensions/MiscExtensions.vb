Imports System.Runtime.CompilerServices
Imports System.Reflection
Imports System.IO
Imports System.Drawing.Drawing2D

Imports Microsoft.Win32
Imports System.Globalization

Public Module MiscExtensions
    <Extension()>
    Function Sort(Of T)(value As IEnumerable(Of T)) As IEnumerable(Of T)
        Dim a As T() = value.ToArray
        Array.Sort(Of T)(a)
        Return a
    End Function

    <Extension()>
    Function Join(value As IEnumerable(Of String), delimiter As String) As String
        If value Is Nothing Then Return Nothing

        Return String.Join(delimiter, value.ToArray)
    End Function

    <Extension()>
    Function GetAttribute(Of T)(mi As MemberInfo) As T
        Dim attributes = mi.GetCustomAttributes(True)

        If OK(attributes) Then
            If attributes.Length = 1 Then
                If TypeOf attributes(0) Is T Then
                    Return DirectCast(attributes(0), T)
                End If
            Else
                For Each i In attributes
                    If TypeOf i Is T Then
                        Return DirectCast(i, T)
                    End If
                Next
            End If
        End If
    End Function

    <Extension()>
    Function IsDigit(c As Char) As Boolean
        Return Char.IsDigit(c)
    End Function

    <Extension()>
    Function EnsureRange(value As Integer,
                         min As Integer,
                         max As Integer) As Integer

        If value < min Then
            value = min
        ElseIf value > max Then
            value = max
        End If

        Return value
    End Function

    <Extension()>
    Function ResizeToSmallIconSize(img As Image) As Image
        If Not img Is Nothing AndAlso img.Size <> SystemInformation.SmallIconSize Then
            Dim s = SystemInformation.SmallIconSize
            Dim r As New Bitmap(s.Width, s.Height)

            Using g = Graphics.FromImage(DirectCast(r, Image))
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.PixelOffsetMode = PixelOffsetMode.HighQuality
                g.DrawImage(img, 0, 0, s.Width, s.Height)
            End Using

            Return r
        End If

        Return img
    End Function

    <Extension()>
    Function ResizeImage(image As Image, ByVal height As Integer) As Image
        Dim percentHeight = height / image.Height
        Dim ret = New Bitmap(CInt(image.Width * percentHeight), CInt(height))

        Using g = Graphics.FromImage(ret)
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            g.DrawImage(image, 0, 0, ret.Width, ret.Height)
        End Using

        Return ret
    End Function

    <Extension()>
    Function ToStringEx(obj As Object) As String
        If obj Is Nothing Then Return "" Else Return obj.ToString
    End Function

    <Extension()>
    Function NeutralCulture(ci As CultureInfo) As CultureInfo
        If ci.IsNeutralCulture Then Return ci Else Return ci.Parent
    End Function
End Module

Public Module RegistryKeyExtensions
    '<Extension()>
    'Sub Delete(rootKey As RegistryKey, key As String)
    '    Dim k = rootKey.OpenSubKey(key)

    '    If Not k Is Nothing Then
    '        Dim names = k.GetSubKeyNames
    '        k.Close()

    '        For Each i In names
    '            Delete(rootKey, key + "\" + i)
    '        Next

    '        rootKey.DeleteSubKey(key, True)
    '    End If
    'End Sub

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
    Function GetInt(rootKey As RegistryKey, subKey As String, name As String) As Integer
        Return GetValue(Of Integer)(rootKey, subKey, name)
    End Function

    <Extension()>
    Function GetBoolean(rootKey As RegistryKey, subKey As String, name As String) As Boolean
        Return GetValue(Of Boolean)(rootKey, subKey, name)
    End Function

    <Extension()>
    Function GetValueNames(rootKey As RegistryKey, subKeyName As String) As IEnumerable(Of String)
        Using k = rootKey.OpenSubKey(subKeyName)
            If Not k Is Nothing Then
                Return k.GetValueNames
            End If
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

    <Extension()>
    Sub DeleteValue(rootKey As RegistryKey, key As String, valueName As String)
        Using k = rootKey.OpenSubKey(key, True)
            If Not k Is Nothing Then
                k.DeleteValue(valueName, False)
            End If
        End Using
    End Sub
End Module