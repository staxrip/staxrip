Imports System.Text
Imports System.Security.Cryptography
Imports System.Runtime.CompilerServices
Imports System.Globalization

Imports VB6 = Microsoft.VisualBasic

Module StringExtensions
    <Extension()>
    Function ChangeExt(path As String, value As String) As String
        If path = "" Then Return ""
        If value = "" Then Return path
        If Not value.StartsWith(".") Then value = "." + value
        Return Filepath.GetDirAndBase(path) + value.ToLower
    End Function

    <Extension()>
    Function Quotes(instance As String) As String
        If instance = "" Then Return ""
        If instance.Contains(" ") Then Return """" + instance + """"
        Return instance
    End Function

    <Extension()>
    Function Parent(instance As String) As String
        Return DirPath.GetParent(instance)
    End Function

    <Extension()>
    Function Ext(instance As String) As String
        Return Filepath.GetExt(instance)
    End Function

    <Extension()>
    Function ExtFull(instance As String) As String
        Return Filepath.GetExtFull(instance)
    End Function

    <Extension()>
    Function Base(instance As String) As String
        Return Filepath.GetBase(instance)
    End Function

    <Extension()>
    Function ContainsAll(instance As String, all As IEnumerable(Of String)) As Boolean
        If instance <> "" Then Return all.All(Function(arg) instance.Contains(arg))
    End Function

    <Extension()>
    Function EqualsAny(instance As String, ParamArray values As String()) As Boolean
        If instance = "" OrElse values.ContainsNothingOrEmpty Then Return False
        Return values.Contains(instance)
    End Function

    <Extension()>
    Function Append(instance As String, value As String) As String
        If instance Is Nothing AndAlso value Is Nothing Then Return ""
        Return instance + value
    End Function

    <Extension()>
    Function AppendSeparator(instance As String) As String
        If instance?.EndsWith(DirPath.Separator) Then Return instance
        Return instance + DirPath.Separator
    End Function

    <Extension()>
    Function FixBreak(value As String) As String
        value = value.Replace(VB6.ChrW(13) + VB6.ChrW(10), VB6.ChrW(10))
        value = value.Replace(VB6.ChrW(13), VB6.ChrW(10))
        Return value.Replace(VB6.ChrW(10), VB6.ChrW(13) + VB6.ChrW(10))
    End Function

    <Extension()>
    Function ContainsUnicode(value As String) As Boolean
        For Each i In value
            If Convert.ToInt32(i) > 255 Then Return True
        Next
    End Function

    <Extension()>
    Function ToTitleCase(value As String) As String
        'TextInfo.ToTitleCase won't work on all upper strings
        Return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower)
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
    Function ToSingle(value As String, Optional defaultValue As Single = 0) As Single
        If value <> "" Then
            If value.Contains(",") Then value = value.Replace(",", ".")

            Dim ret As Single

            If Single.TryParse(value,
                               NumberStyles.Float Or NumberStyles.AllowThousands,
                               CultureInfo.InvariantCulture,
                               ret) Then
                Return ret
            End If
        End If

        Return defaultValue
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
    Function FormatColumn(value As String, delimiter As String) As String
        If value = "" Then Return ""
        Dim lines = value.SplitKeepEmpty(CrLf)
        Dim leftSides As New List(Of String)

        For Each i In lines
            Dim pos = i.IndexOf(delimiter)

            If pos > 0 Then
                leftSides.Add(i.Substring(0, pos).Trim)
            Else
                leftSides.Add(i)
            End If
        Next

        Dim highest = Aggregate i In leftSides Into Max(i.Length)
        Dim ret As New List(Of String)

        For i = 0 To lines.Length - 1
            Dim line = lines(i)

            If line.Contains(delimiter) Then
                ret.Add(leftSides(i).PadRight(highest) + " " + delimiter + " " + line.Substring(line.IndexOf(delimiter) + 1).Trim)
            Else
                ret.Add(leftSides(i))
            End If
        Next

        Return ret.Join(CrLf)
    End Function

    <Extension()>
    Sub WriteFile(value As String, path As String)
        WriteFile(value, path, Encoding.Default)
    End Sub

    <Extension()>
    Sub WriteFile(value As String, path As String, encoding As Encoding)
        Try
            Using sw As New StreamWriter(File.Create(path), encoding)
                sw.Write(value)
            End Using
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    <Extension()>
    Function Left(value As String, index As Integer) As String
        If value = "" OrElse index < 0 Then Return ""
        If index > value.Length Then Return value
        Return value.Substring(0, index)
    End Function

    <Extension()>
    Function Left(value As String, start As String) As String
        If Not value.Contains(start) Then
            Return ""
        End If

        Return value.Substring(0, value.IndexOf(start))
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
    Function EqualIgnoreCase(a As String, b As String) As Boolean
        If a = "" OrElse b = "" Then Return False
        Return String.Compare(a, b, StringComparison.OrdinalIgnoreCase) = 0
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
    Function SplitKeepEmpty(value As String, ParamArray delimiters As String()) As String()
        Return value.Split(delimiters, StringSplitOptions.None)
    End Function

    <Extension()>
    Function SplitNoEmptyAndWhiteSpace(value As String, ParamArray delimiters As String()) As String()
        If value = "" Then Return {}

        Dim a = SplitNoEmpty(value, delimiters)

        For i = 0 To a.Length - 1
            a(i) = a(i).Trim
        Next

        Dim l = a.ToList

        While l.Contains("")
            l.Remove("")
        End While

        Return l.ToArray
    End Function

    <Extension()>
    Function SplitLinesNoEmpty(value As String) As String()
        Return SplitNoEmpty(value, Environment.NewLine)
    End Function

    <Extension()>
    Function RemoveChars(value As String, chars As String) As String
        Dim ret = value

        For Each i In value
            If chars.IndexOf(i) >= 0 Then
                ret = ret.Replace(i, "")
            End If
        Next

        Return ret
    End Function

    <Extension()>
    Function DeleteRight(value As String, count As Integer) As String
        Return Left(value, value.Length - count)
    End Function

    <Extension()>
    Function ReplaceUnicode(value As String) As String
        If value.Contains(Convert.ToChar(&H2212)) Then
            value = value.Replace(Convert.ToChar(&H2212), "-"c)
        End If

        Return value
    End Function

    <Extension()>
    Function MD5Hash(value As String) As String
        Dim crypt = MD5CryptoServiceProvider.Create()
        Dim hash = crypt.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value))
        Dim sb As New StringBuilder()

        For Each i In hash
            sb.Append(i.ToString("x2"))
        Next

        Return sb.ToString()
    End Function

    <Extension()>
    Sub ToClipboard(value As String)
        If value <> "" Then
            Clipboard.SetText(value)
        Else
            Clipboard.Clear()
        End If
    End Sub
End Module