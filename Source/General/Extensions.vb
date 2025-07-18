
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Globalization
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Threading.Tasks

Imports Microsoft.Win32
Imports StaxRip.UI
Imports VB6 = Microsoft.VisualBasic

Module StringExtensions
    Private Separator As Char = Path.DirectorySeparatorChar

    <Extension>
    Function TrimTrailingSeparator(instance As String) As String
        If instance = "" Then Return ""

        If instance.EndsWith(Separator) AndAlso Not instance.Length <= 3 Then
            Return instance.TrimEnd(Separator)
        End If

        Return instance
    End Function

    <Extension>
    Function TrimQuotes(instance As String) As String
        If instance = "" Then Return ""
        If instance.Length < 2 Then Return instance
        If Not instance.StartsWith("""") OrElse Not instance.EndsWith("""") Then Return instance

        Return instance.Substring(1, instance.Length - 2)
    End Function

    <Extension()>
    Function Parent(path As String) As String
        If path = "" Then Return ""

        Return IO.Path.GetDirectoryName(path.TrimQuotes())
    End Function

    <Extension>
    Function IndentLines(instance As String, value As String) As String
        If instance = "" Then Return ""

        instance = value + instance
        instance = instance.Replace(BR, BR + value)
        Return instance
    End Function

    <Extension>
    Function StartsWithEx(instance As String, value As String) As Boolean
        Return instance <> "" AndAlso value <> "" AndAlso instance.StartsWith(value)
    End Function

    <Extension>
    Function EndsWithEx(instance As String, value As String) As Boolean
        Return instance <> "" AndAlso value <> "" AndAlso instance.EndsWith(value)
    End Function

    <Extension>
    Function ContainsEx(instance As String, value As String) As Boolean
        Return instance <> "" AndAlso value <> "" AndAlso instance.Contains(value)
    End Function

    <Extension>
    Function Matches(instance As String, pattern As String) As Boolean
        Return instance <> "" AndAlso pattern <> "" AndAlso Regex.IsMatch(instance, pattern)
    End Function

    <Extension>
    Function ToLowerEx(instance As String) As String
        Return If(instance = "", "", instance.ToLowerInvariant)
    End Function

    <Extension>
    Function TrimEx(instance As String) As String
        Return If(instance = "", "", instance.Trim)
    End Function

    <Extension>
    Function PathStartsWith(instance As String, value As String) As Boolean
        If instance <> "" AndAlso value <> "" Then
            Return instance.ToLowerInvariant.StartsWith(value.ToLowerInvariant)
        End If
    End Function

    <Extension>
    Function PathEquals(instance As String, value As String) As Boolean
        If instance <> "" AndAlso value <> "" Then
            Return instance.ToLowerInvariant = value.ToLowerInvariant
        End If
    End Function

    <Extension>
    Sub ThrowIfContainsNewLine(instance As String)
        If instance?.Contains(BR) Then
            Throw New Exception("String contains a line break char: " + instance)
        End If
    End Sub

    <Extension>
    Function Multiply(instance As String, multiplier As Integer) As String
        If multiplier < 1 Then
            multiplier = 1
        End If

        Dim sb As New StringBuilder(multiplier * instance.Length)

        For i = 0 To multiplier - 1
            sb.Append(instance)
        Next

        Return sb.ToString()
    End Function

    <Extension>
    Function IsValidFileName(instance As String) As Boolean
        If instance = "" Then Return False

        Dim chars = Path.GetInvalidFileNameChars()

        For Each i In instance
            If chars.Contains(i) Then
                Return False
            End If

            If Convert.ToInt32(i) < 32 Then
                Return False
            End If
        Next

        Return True
    End Function

    <Extension>
    Function IsValidPath(instance As String) As Boolean
        If instance = "" Then Return False

        Dim chars = Path.GetInvalidPathChars()

        For Each i In instance
            If chars.Contains(i) Then
                Return False
            End If

            If Convert.ToInt32(i) < 32 Then
                Return False
            End If
        Next

        Return True
    End Function

    <Extension>
    Function IsASCIIEncodingCompatible(instance As String) As Boolean
        If instance = "" Then
            Return True
        End If

        For Each i In instance
            If Convert.ToInt32(i) > 127 Then
                Return False
            End If
        Next

        Return True
    End Function

    <Extension>
    Function IsProcessEncodingCompatible(instance As String) As Boolean
        If instance = "" Then
            Return True
        End If

        If IsASCIIEncodingCompatible(instance) Then
            Return True
        End If

        Dim bytes = Encoding.Convert(Encoding.Unicode, TextEncoding.EncodingOfProcess, Encoding.Unicode.GetBytes(instance))
        Return instance = Encoding.Unicode.GetString(Encoding.Convert(TextEncoding.EncodingOfProcess, Encoding.Unicode, bytes))
    End Function

    <Extension()>
    Function FileName(instance As String) As String
        If instance = "" Then Return ""

        Return Path.GetFileName(instance.TrimQuotes())
    End Function

    <Extension()>
    Function ChangeExt(instance As String, value As String) As String
        If instance = "" Then
            Return ""
        End If

        If value = "" Then
            Return instance
        End If

        If Not value.StartsWith(".") Then
            value = "." + value
        End If

        Return instance.DirAndBase + value.ToLowerInvariant
    End Function

    <Extension()>
    Function Escape(instance As String) As String
        If instance = "" Then Return ""
        If instance(0) = """" AndAlso instance(instance.Length - 1) = """" Then Return instance

        For Each i In " ;=~*$%()&"
            If instance.Contains(i) Then
                Return """" + instance + """"
            End If
        Next

        Return instance
    End Function

    <Extension()>
    Function Unescape(instance As String) As String
        If instance.Length < 2 Then Return instance
        If instance(0) = """" AndAlso instance(instance.Length - 1) = """" Then Return instance.Substring(1, instance.Length - 2)
        Return instance
    End Function

    <Extension()>
    Function ExistingParent(instance As String) As String
        Dim ret = instance.Parent

        ' what sp
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        Return ret
    End Function

    <Extension()>
    Function FileExists(instance As String) As Boolean
        If String.IsNullOrWhiteSpace(instance) Then Return False
        Return File.Exists(instance.TrimQuotes())
    End Function

    <Extension()>
    Function FileSize(instance As String) As Long
        If String.IsNullOrWhiteSpace(instance) Then Return 0L
        If Not instance.FileExists() Then Return 0L

        Return New FileInfo(instance.TrimQuotes()).Length
    End Function

    <Extension()>
    Function DirSize(instance As String) As Long
        If String.IsNullOrWhiteSpace(instance) Then Return 0L
        If Not instance.DirExists() Then Return 0L

        Dim ret = 0L
        Dim di = New DirectoryInfo(instance.TrimQuotes())
        Dim files = di.GetFiles()

        For Each f In files
            ret += f.Length
        Next

        For Each d In di.GetDirectories()
            ret += d.FullName.DirSize()
        Next

        Return ret
    End Function

    <Extension()>
    Function DirExists(instance As String) As Boolean
        If instance <> "" Then
            Return Directory.Exists(instance.TrimQuotes())
        End If
    End Function

    <Extension()>
    Function Ext(instance As String) As String
        Return GetExt(instance, False)
    End Function

    <Extension()>
    Function ExtFull(instance As String) As String
        Return GetExt(instance, True)
    End Function

    Function GetExt(filepath As String, includeDot As Boolean) As String
        If filepath = "" Then Return ""

        Dim ext = Path.GetExtension(filepath.TrimQuotes()).ToLowerInvariant()
        If Not includeDot Then
            ext = ext.TrimStart("."c)
        End If
        Return ext
    End Function

    <Extension()>
    Function Base(instance As String) As String
        If instance = "" Then Return ""

        Dim ret = instance.TrimQuotes()
        Dim index = ret.LastIndexOf(Path.DirectorySeparatorChar)

        If Not ret.Contains(".") Then Return If(index < 0, ret, ret.Substring(index + 1))

        Return Path.GetFileNameWithoutExtension(ret)
    End Function

    <Extension()>
    Function Dir(instance As String) As String
        If instance = "" Then Return ""

        Return Path.GetDirectoryName(instance.TrimQuotes())
    End Function

    <Extension()>
    Function LongPathPrefix(instance As String) As String
        If instance = "" Then Return ""

        Dim prefix = "\\?\"

        Return If(instance.Length > GlobalClass.MAX_PATH AndAlso Not instance.StartsWith(prefix), prefix + instance, instance)
    End Function

    <Extension()>
    Function ToShortFilePath(instance As String) As String
        If instance = "" Then
            Return ""
        End If

        If instance.StartsWith("\\?\") Then
            instance = instance.Substring(4)
        End If

        If instance.Length <= GlobalClass.MAX_PATH Then
            Return instance
        End If

        Dim sb As New StringBuilder(GlobalClass.MAX_PATH)
        Native.GetShortPathName(instance.Dir, sb, sb.Capacity)
        Dim ret = Path.Combine(sb.ToString, instance.FileName)

        If ret.Length <= GlobalClass.MAX_PATH Then
            Return ret
        End If

        Native.GetShortPathName(instance, sb, sb.Capacity)
        Return sb.ToString
    End Function

    <Extension()>
    Function ToShortFolderPath(instance As String) As String
        If instance = "" Then
            Return ""
        End If

        If instance.StartsWith("\\?\") Then
            instance = instance.Substring(4)
        End If

        If instance.Length <= GlobalClass.MAX_PATH Then
            Return instance
        End If

        Dim sb As New StringBuilder(GlobalClass.MAX_PATH)
        Native.GetShortPathName(instance, sb, sb.Capacity)
        Return sb.ToString
    End Function

    <Extension()>
    Function DirName(instance As String) As String
        If instance = "" Then Return ""

        If File.Exists(instance) Then
            Return Path.GetFileName(Path.GetDirectoryName(instance))
        ElseIf Directory.Exists(instance) Then
            Return Path.GetFileName(instance)
        ElseIf Path.GetFileName(instance).Contains(".") Then
            Return Path.GetFileName(Path.GetDirectoryName(instance))
        End If

        Return Path.GetFileName(Path.GetDirectoryName(instance))
    End Function

    <Extension()>
    Function DirAndBase(p As String) As String
        Return Path.Combine(p.Dir, p.Base)
    End Function

    <Extension()>
    Function ContainsAll(instance As String, ParamArray all As String()) As Boolean
        If instance <> "" Then
            Return all.All(Function(arg) instance.Contains(arg))
        End If
    End Function

    <Extension()>
    Function ContainsAny(instance As String, ParamArray any As String()) As Boolean
        If instance <> "" AndAlso Not any.NothingOrEmpty Then
            Return any.Any(Function(arg) instance.Contains(arg))
        End If
    End Function

    <Extension()>
    Function EndsWithAny(instance As String, ParamArray any As String()) As Boolean
        If instance <> "" AndAlso Not any.NothingOrEmpty Then
            Return any.Any(Function(arg) instance.EndsWith(arg))
        End If
    End Function

    <Extension()>
    Function StartsWithAny(instance As String, ParamArray any As String()) As Boolean
        If instance <> "" AndAlso Not any.NothingOrEmpty Then
            Return any.Any(Function(arg) instance.StartsWith(arg))
        End If
    End Function

    <Extension()>
    Function EqualsAny(instance As String, ParamArray values As String()) As Boolean
        If instance = "" OrElse values.NothingOrEmpty Then Return False

        Return values.Contains(instance)
    End Function

    <Extension()>
    Function FixDir(instance As String) As String
        If instance = "" Then Return ""
        If instance.Last() = Path.DirectorySeparatorChar AndAlso instance.SplitNoEmpty(Path.DirectorySeparatorChar).Length > 1 Then Return instance.Substring(0, instance.Length - 1)

        Return instance
    End Function

    <Extension()>
    Function FixBreak(value As String) As String
        value = value.Replace(VB6.ChrW(13) + VB6.ChrW(10), VB6.ChrW(10))
        value = value.Replace(VB6.ChrW(13), VB6.ChrW(10))
        Return value.Replace(VB6.ChrW(10), VB6.ChrW(13) + VB6.ChrW(10))
    End Function

    <Extension()>
    Function ToTitleCase(value As String) As String
        'TextInfo.ToTitleCase won't work on all upper strings
        Return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLowerInvariant)
    End Function

    <Extension()>
    Function IsInt(value As String) As Boolean
        Return Integer.TryParse(value, Nothing)
    End Function

    <Extension()>
    Function ToInt(value As String, Optional defaultValue As Integer = 0) As Integer
        Return If(Not Integer.TryParse(value, Nothing), defaultValue, CInt(value))
    End Function

    <Extension()>
    Function IsLong(value As String) As Boolean
        Return Long.TryParse(value, Nothing)
    End Function

    <Extension()>
    Function ToLong(value As String, Optional defaultValue As Long = 0L) As Long
        Dim parsed = 0L
        Return If(Long.TryParse(value, parsed), parsed, defaultValue)
    End Function

    <Extension()>
    Function IsSingle(value As String) As Boolean
        If value <> "" Then
            If value.Contains(",") Then value = value.Replace(",", ".")

            Return Single.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, Nothing)
        End If
    End Function

    <Extension()>
    Function ToSingle(value As String, Optional defaultValue As Single = 0) As Single
        If value <> "" Then
            If value.Contains(",") Then value = value.Replace(",", ".")

            Dim ret As Single

            If Single.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, ret) Then
                Return ret
            End If
        End If

        Return defaultValue
    End Function

    <Extension()>
    Function IsDate(value As String) As Boolean
        If value <> "" Then
            Dim ret As Date
            Return Date.TryParse(value, ret)
        End If
        Return False
    End Function

    <Extension()>
    Function IsDouble(value As String) As Boolean
        If value <> "" Then
            If value.Contains(",") Then
                value = value.Replace(",", ".")
            End If

            Return Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, Nothing)
        End If
    End Function

    <Extension()>
    Function ToDouble(value As String, Optional defaultValue As Double = 0) As Double
        If value <> "" Then
            If value.Contains(",") Then
                value = value.Replace(",", ".")
            End If

            Dim ret As Double

            If Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, ret) Then
                Return ret
            End If
        End If

        Return defaultValue
    End Function

    <Extension()>
    Function ToColor(str As String, Optional defaultColor As Color = Nothing) As Color
        Try
            Return ColorTranslator.FromHtml(str)
        Catch ex As Exception
            Return defaultColor
        End Try
    End Function

    <Extension()>
    Function FormatColumn(value As String, delimiter As String) As String
        If value = "" Then Return ""
        Dim lines = value.SplitKeepEmpty(BR)
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

        Return ret.Join(BR)
    End Function

    <Extension()>
    Function ReadAllText(instance As String) As String
        If Not instance.FileExists Then Return ""

        Return File.ReadAllText(instance)
    End Function

    <Extension()>
    Function ReadAllTextDefault(instance As String) As String
        If Not instance.FileExists Then Return ""

        Return File.ReadAllText(instance, Encoding.Default)
    End Function

    <Extension()>
    Sub WriteFileSystemEncoding(instance As String, path As String)
        If TextEncoding.EncodingOfSystem.CodePage = TextEncoding.UTF8CodePage Then
            WriteFileUTF8(instance, path)
        Else
            WriteFile(instance, path, TextEncoding.EncodingOfSystem)
        End If
    End Sub

    <Extension()>
    Sub WriteFileProcessEncoding(instance As String, path As String)
        WriteFile(instance, path, TextEncoding.EncodingOfProcess)
    End Sub

    <Extension()>
    Sub WriteFileUTF8(instance As String, path As String)
        WriteFile(instance, path, New UTF8Encoding(False))
    End Sub

    <Extension()>
    Sub WriteFileUTF8BOM(instance As String, path As String)
        WriteFile(instance, path, New UTF8Encoding(True))
    End Sub

    <Extension()>
    Sub WriteFile(instance As String, path As String, encoding As Encoding)
        Dim counter = 0

        While True
            Try
                File.WriteAllText(path, instance, encoding)
                Exit While
            Catch ex As Exception
                Thread.Sleep(150)
                counter += 1

                If counter > 9 Then
                    g.ShowException(ex)
                    Exit While
                End If
            End Try
        End While
    End Sub

    <Extension()>
    Function Left(value As String, index As Integer) As String
        If value = "" OrElse index < 0 Then
            Return ""
        End If

        If index > value.Length Then
            Return value
        End If

        Return value.Substring(0, index)
    End Function

    <Extension()>
    Function Left(value As String, start As String) As String
        If value = "" OrElse start = "" Then
            Return ""
        End If

        If Not value.Contains(start) Then
            Return ""
        End If

        Return value.Substring(0, value.IndexOf(start))
    End Function

    <Extension()>
    Function LeftLast(value As String, start As String) As String
        If Not value.Contains(start) Then
            Return ""
        End If

        Return value.Substring(0, value.LastIndexOf(start))
    End Function

    <Extension()>
    Function Right(value As String, start As String) As String
        If value = "" OrElse start = "" Then
            Return ""
        End If

        If Not value.Contains(start) Then
            Return ""
        End If

        Return value.Substring(value.IndexOf(start) + start.Length)
    End Function

    <Extension()>
    Function RightLast(value As String, start As String) As String
        If value = "" OrElse start = "" Then
            Return ""
        End If

        If Not value.Contains(start) Then
            Return ""
        End If

        Return value.Substring(value.LastIndexOf(start) + start.Length)
    End Function

    <Extension()>
    Function IsEqualIgnoreCase(value1 As String, value2 As String) As Boolean
        Return String.Compare(value1, value2, StringComparison.OrdinalIgnoreCase) = 0
    End Function

    <Extension()>
    Function Shorten(value As String, maxLength As Integer) As String
        Return If(value = "" OrElse maxLength < 1 OrElse value.Length <= maxLength, value, value.Substring(0, maxLength))
    End Function

    <Extension()>
    Function IsValidFileSystemName(instance As String) As Boolean
        If instance = "" Then
            Return False
        End If

        Dim chars = """*/:<>?\|^".ToCharArray

        For Each i In instance.ToCharArray
            If chars.Contains(i) Then
                Return False
            End If

            If Convert.ToInt32(i) < 32 Then
                Return False
            End If
        Next

        Return True
    End Function

    <Extension()>
    Function ReplaceInvalidFileSystemName(instance As String, Optional replaceWith As Char = "_"c) As String
        If instance = "" Then Return ""
        Dim ret = instance

        Dim chars = Path.GetInvalidFileNameChars()

        For Each c In chars
            ret = ret.Replace(c, replaceWith)
        Next

        Return ret
    End Function

    <Extension()>
    Function IsSameBase(instance As String, b As String) As Boolean
        Return instance.Base.IsEqualIgnoreCase(b.Base)
    End Function

    <Extension()>
    Function EscapeIllegalFileSysChars(value As String) As String
        If value = "" Then Return ""

        For Each i In value
            If Not IsValidFileSystemName(i) Then
                value = value.Replace(i, "__" + Uri.EscapeDataString(i).TrimStart("%"c) + "__")
            End If
        Next

        Return value
    End Function

    <Extension()>
    Function UnescapeIllegalFileSysChars(value As String) As String
        If value = "" Then
            Return ""
        End If

        For Each match As Match In Regex.Matches(value, "__(\w\w)__")
            value = value.Replace(match.Value, Uri.UnescapeDataString("%" + match.Groups(1).Value))
        Next

        Return value
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
        If value = "" Then
            Return {}
        End If

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

        For Each ch In value
            If chars.Contains(ch) Then
                ret = ret.Replace(ch, "")
            End If
        Next

        Return ret
    End Function

    <Extension()>
    Function DeleteRight(value As String, count As Integer) As String
        Return Left(value, value.Length - count)
    End Function

    <Extension()>
    Function ReplaceAll(instance As String, values As String(), replace As String) As String
        If instance = "" Then Return ""
        If values Is Nothing OrElse Not values.Any() Then Return instance

        For Each value In values
            instance = instance.Replace(value, replace)
        Next

        Return instance
    End Function

    <Extension()>
    Function ReplaceAll(instance As String, values As IEnumerable(Of String), replace As String) As String
        If instance = "" Then Return ""
        If values Is Nothing OrElse Not values.Any() Then Return instance

        For Each value In values
            instance = instance.Replace(value, replace)
        Next

        Return instance
    End Function

    <Extension()>
    Function ReplaceRecursive(value As String, find As String, replace As String) As String
        If value = "" Then
            Return ""
        End If

        While value.Contains(find)
            value = value.Replace(find, replace)
        End While

        Return value
    End Function

    <Extension()>
    Function ReplaceTabsWithSpaces(value As String, Optional tabLength As Integer = 8) As String
        If String.IsNullOrWhiteSpace(value) Then Return ""
        If Not value.Contains(VB6.vbTab) Then Return value

        Dim splitted = value.Split({VB6.vbTab}, StringSplitOptions.None)
        Dim ret = New StringBuilder()

        For Each split As String In splitted
            If String.IsNullOrEmpty(split) Then
                ret.Append(New String(" "c, tabLength))
            Else
                ret.Append(split & New String(" "c, tabLength - (split.RightLast(VB6.vbCrLf).Length Mod tabLength)))
            End If
        Next

        Return ret.ToString()
    End Function

    <Extension()>
    Function Reverse(value As String) As String
        Dim chars = value.ToCharArray
        Array.Reverse(chars)
        Return New String(chars)
    End Function

    <Extension()>
    Function MD5Hash(instance As String) As String
        Using m = MD5.Create()
            Dim inputBuffer = Encoding.UTF8.GetBytes(instance)
            Dim hashBuffer = m.ComputeHash(inputBuffer)
            Return BitConverter.ToString(m.ComputeHash(inputBuffer))
        End Using
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

Module MiscExtensions
    <Extension()>
    Sub CenterScreen(instance As Form)
        If instance IsNot Nothing Then
            instance.StartPosition = FormStartPosition.Manual
            Dim wa = Screen.FromControl(instance).WorkingArea
            instance.Left = (wa.Width - instance.Width) \ 2
            instance.Top = (wa.Height - instance.Height) \ 2
        End If
    End Sub

    <Extension()>
    Function ToInvariantString(value As Double, format As String) As String
        Return value.ToString(format, CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Function ToInvariantString(value As Single, format As String) As String
        Return value.ToString(format, CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Function ToInvariantString(instance As IConvertible) As String
        Return If(instance Is Nothing, "", instance.ToString(CultureInfo.InvariantCulture))
    End Function

    <Extension()>
    Function ToInvariantString(instance As Date, format As String) As String
        Return instance.ToString(format, CultureInfo.InvariantCulture)
    End Function

    <Extension()>
    Function ContainsEx(Of T)(instance As IEnumerable(Of T), value As T) As Boolean
        If instance IsNot Nothing Then
            Return instance.Contains(value)
        End If
    End Function

    <Extension()>
    Function ContainsAny(Of T)(instance As IEnumerable(Of T), ParamArray values As T()) As Boolean
        Return instance.Where(Function(arg) values.Contains(arg)).Count > 0
    End Function

    <Extension()>
    Function ContainsAny(Of T)(instance As IEnumerable(Of T), values As IEnumerable(Of T)) As Boolean
        Return instance.Where(Function(arg) values.Contains(arg)).Count > 0
    End Function

    <Extension()>
    Function Sort(Of T)(instance As IEnumerable(Of T)) As IEnumerable(Of T)
        Dim ret = instance.ToArray
        Array.Sort(Of T)(ret)
        Return ret
    End Function

    <Extension()>
    Function Join(instance As IEnumerable(Of String),
                  delimiter As String,
                  Optional removeEmpty As Boolean = False) As String

        If instance Is Nothing Then
            Return Nothing
        End If

        Dim containsEmpty As Boolean

        For Each item In instance
            If item = "" Then
                containsEmpty = True
                Exit For
            End If
        Next

        If containsEmpty AndAlso removeEmpty Then
            instance = instance.Where(Function(arg) arg <> "")
        End If

        Return String.Join(delimiter, instance)
    End Function

    <Extension()>
    Function GetAttribute(Of T)(mi As MemberInfo) As T
        Dim attributes = mi.GetCustomAttributes(True)

        If Not attributes.NothingOrEmpty Then
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
    Function ToOnOffString(instance As Boolean) As String
        Return If(instance, "On", "Off")
    End Function

    <Extension()>
    Function NeutralCulture(ci As CultureInfo) As CultureInfo
        Return If(ci.IsNeutralCulture, ci, ci.Parent)
    End Function

    <Extension()>
    Function NothingOrEmpty(strings As IEnumerable(Of String)) As Boolean
        If strings?.Any() Then
            For Each str In strings
                If str <> "" Then Return False
            Next
        End If
        Return True
    End Function

    <Extension()>
    Function NothingOrEmpty(objects As IEnumerable(Of Object)) As Boolean
        If objects?.Any() Then
            For Each obj In objects
                If obj IsNot Nothing Then
                    Return False
                End If
            Next
        End If

        Return True
    End Function

    <Extension()>
    Function ToColorHSL(color As Color) As ColorHSL
        Return color
    End Function

    <Extension()>
    Function ToSeparatedString(list As IEnumerable(Of x264Control.QualityItem)) As String
        If list Is Nothing OrElse Not list.Any() Then Return ""

        Return String.Join("_", s.X264QualityDefinitions.OrderBy(Function(x) x.Value).Select(Function(x) $"{x.Value:0.#}{If(String.IsNullOrWhiteSpace(x.Text), "", $"""{x.Text.Trim()}""")}"))
    End Function

    <Extension()>
    Function ToSeparatedString(list As IEnumerable(Of x265Control.QualityItem)) As String
        If list Is Nothing OrElse Not list.Any() Then Return ""

        Return String.Join("_", s.X265QualityDefinitions.OrderBy(Function(x) x.Value).Select(Function(x) $"{x.Value:0.#}{If(String.IsNullOrWhiteSpace(x.Text), "", $"""{x.Text.Trim()}""")}"))
    End Function

    <Extension()>
    Function ToSeparatedString(list As IEnumerable(Of SvtAv1EncAppControl.QualityItem)) As String
        If list Is Nothing OrElse Not list.Any() Then Return ""

        Return String.Join("_", s.SvtAv1EncAppQualityDefinitions.OrderBy(Function(x) x.Value).Select(Function(x) $"{x.Value:0.#}{If(String.IsNullOrWhiteSpace(x.Text), "", $"""{x.Text.Trim()}""")}"))
    End Function

    <Extension()>
    Function ToSeparatedString(list As IEnumerable(Of VvencffappControl.QualityItem)) As String
        If list Is Nothing OrElse Not list.Any() Then Return ""

        Return String.Join("_", s.VvencffappQualityDefinitions.OrderBy(Function(x) x.Value).Select(Function(x) $"{x.Value:0.#}{If(String.IsNullOrWhiteSpace(x.Text), "", $"""{x.Text.Trim()}""")}"))
    End Function

    <Extension()>
    Function ToX264QualityItems(input As String) As IEnumerable(Of x264Control.QualityItem)
        Dim result = New List(Of x264Control.QualityItem)

        If String.IsNullOrWhiteSpace(input) Then Return result

        Dim pattern = "(\d{1,2}([\.,]\d{1,3})?)(""([^""]*)"")?"
        Dim matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase)

        If matches.Count = 0 Then Return result

        Dim qualityConfig = If(TypeOf p.VideoEncoder Is x264Enc, DirectCast(p.VideoEncoder, x264Enc).Params.Quant.Config, {0, 69, 0, 1})

        For Each match As Match In matches
            If Not match.Success Then
                Continue For
            End If

            Dim value = 0.0
            Dim text = ""

            If Double.TryParse(Regex.Replace(match.Groups(1).Value, "\.|,", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator), value) Then
                Dim powed = Math.Pow(10, qualityConfig(3))
                value = CInt(Math.Floor(value * powed)) / powed

                If value >= qualityConfig(0) AndAlso value <= qualityConfig(1) AndAlso Not result.Where(Function(x) x.Value = value).Any() Then
                    text = If(match.Groups.Count > 4, match.Groups(4).Value.Trim(), text)
                    result.Add(New x264Control.QualityItem(value, text, ""))
                End If
            End If
        Next
        Return result
    End Function

    <Extension()>
    Function ToX265QualityItems(input As String) As IEnumerable(Of x265Control.QualityItem)
        Dim result = New List(Of x265Control.QualityItem)

        If String.IsNullOrWhiteSpace(input) Then Return result

        Dim pattern = "(\d{1,2}([\.,]\d{1,3})?)(""([^""]*)"")?"
        Dim matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase)

        If matches.Count = 0 Then Return result

        Dim qualityConfig = If(TypeOf p.VideoEncoder Is x265Enc, DirectCast(p.VideoEncoder, x265Enc).Params.Quant.Config, {0, 51, 0, 1})

        For Each match As Match In matches
            If Not match.Success Then
                Continue For
            End If

            Dim value = 0.0
            Dim text = ""

            If Double.TryParse(Regex.Replace(match.Groups(1).Value, "\.|,", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator), value) Then
                Dim powed = Math.Pow(10, qualityConfig(3))
                value = CInt(Math.Floor(value * powed)) / powed

                If value >= qualityConfig(0) AndAlso value <= qualityConfig(1) AndAlso Not result.Where(Function(x) x.Value = value).Any() Then
                    text = If(match.Groups.Count > 4, match.Groups(4).Value.Trim(), text)
                    result.Add(New x265Control.QualityItem(value, text, ""))
                End If
            End If
        Next

        Return result
    End Function

    <Extension()>
    Function ToSvtAv1EncAppQualityItems(input As String) As IEnumerable(Of SvtAv1EncAppControl.QualityItem)
        Dim result = New List(Of SvtAv1EncAppControl.QualityItem)

        If String.IsNullOrWhiteSpace(input) Then Return result

        Dim pattern = "(\d{1,2}([\.,]\d{1,3})?)(""([^""]*)"")?"
        Dim matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase)

        If matches.Count = 0 Then Return result

        Dim qualityConfig = If(TypeOf p.VideoEncoder Is SvtAv1Enc, DirectCast(p.VideoEncoder, SvtAv1Enc).Params.QuantizationParameter.Config, {0, 63, 0, 1})

        For Each match As Match In matches
            If Not match.Success Then
                Continue For
            End If

            Dim value = 0.0
            Dim text = ""

            If Double.TryParse(Regex.Replace(match.Groups(1).Value, "\.|,", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator), value) Then
                Dim powed = Math.Pow(10, qualityConfig(3))
                value = CInt(Math.Floor(value * powed)) / powed

                If value >= qualityConfig(0) AndAlso value <= qualityConfig(1) AndAlso Not result.Where(Function(x) x.Value = value).Any() Then
                    text = If(match.Groups.Count > 4, match.Groups(4).Value.Trim(), text)
                    result.Add(New SvtAv1EncAppControl.QualityItem(value, text, ""))
                End If
            End If
        Next

        Return result
    End Function

    <Extension()>
    Function ToVvencffappQualityItems(input As String) As IEnumerable(Of VvencffappControl.QualityItem)
        Dim result = New List(Of VvencffappControl.QualityItem)

        If String.IsNullOrWhiteSpace(input) Then Return result

        Dim pattern = "(\d{1,2}([\.,]\d{1,3})?)(""([^""]*)"")?"
        Dim matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase)

        If matches.Count = 0 Then Return result

        Dim qualityConfig = If(TypeOf p.VideoEncoder Is VvencffappEnc, DirectCast(p.VideoEncoder, VvencffappEnc).Params.Quant.Config, {0, 63, 0, 1})

        For Each match As Match In matches
            If Not match.Success Then
                Continue For
            End If

            Dim value = 0.0
            Dim text = ""

            If Double.TryParse(Regex.Replace(match.Groups(1).Value, "\.|,", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator), value) Then
                Dim powed = Math.Pow(10, qualityConfig(3))
                value = CInt(Math.Floor(value * powed)) / powed

                If value >= qualityConfig(0) AndAlso value <= qualityConfig(1) AndAlso Not result.Where(Function(x) x.Value = value).Any() Then
                    text = If(match.Groups.Count > 4, match.Groups(4).Value.Trim(), text)
                    result.Add(New VvencffappControl.QualityItem(value, text, ""))
                End If
            End If
        Next

        Return result
    End Function
End Module

Module RegistryKeyExtensions
    Function GetValue(Of T)(root As RegistryKey, path As String, name As String) As T
        Using key = root.OpenSubKey(path)
            If key IsNot Nothing Then
                Dim value = key.GetValue(name)

                If value IsNot Nothing Then
                    Try
                        Return CType(value, T)
                    Catch
                    End Try
                End If
            End If
        End Using
    End Function

    <Extension()>
    Function GetString(root As RegistryKey, path As String, name As String) As String
        Return GetValue(Of String)(root, path, name)
    End Function

    <Extension()>
    Function GetInt(root As RegistryKey, path As String, name As String) As Integer
        Return GetValue(Of Integer)(root, path, name)
    End Function

    <Extension()>
    Function GetLong(root As RegistryKey, path As String, name As String) As Long
        Dim value = GetValue(Of Long)(root, path, name)
        Return BitConverter.ToUInt32(BitConverter.GetBytes(value), 0)
    End Function

    <Extension()>
    Function GetBoolean(root As RegistryKey, path As String, name As String) As Boolean
        Return GetValue(Of Boolean)(root, path, name)
    End Function

    <Extension()>
    Function GetKeyNames(root As RegistryKey, path As String) As String()
        Using subKey = root.OpenSubKey(path)
            If subKey IsNot Nothing Then
                Return subKey.GetSubKeyNames
            End If
        End Using

        Return {}
    End Function

    <Extension()>
    Function GetValueNames(root As RegistryKey, path As String) As String()
        Using subKey = root.OpenSubKey(path)
            If subKey IsNot Nothing Then
                Return subKey.GetValueNames
            End If
        End Using

        Return {}
    End Function

    <Extension()>
    Sub Write(root As RegistryKey, path As String, name As String, value As Object)
        Dim subKey = If(root.OpenSubKey(path, True), root.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree))

        subKey.SetValue(name, value)
        subKey.Close()
    End Sub

    <Extension()>
    Sub DeleteValue(root As RegistryKey, path As String, name As String)
        Using key = root.OpenSubKey(path, True)
            key?.DeleteValue(name, False)
        End Using
    End Sub
End Module

Module ControlExtensions
    <Extension()>
    Sub ScaleClientSize(instance As Control, width As Single, height As Single)
        instance.ClientSize = New Size(CInt(instance.Font.Height * width / s.UIScaleFactor), CInt(instance.Font.Height * height / s.UIScaleFactor))
    End Sub

    <Extension()>
    Sub ScaleSize(instance As Control, width As Single, height As Single)
        instance.Size = New Size(CInt(instance.Font.Height * width), CInt(instance.Font.Height * height))
    End Sub

    <Extension()>
    Sub ReplaceFontFamily(instance As Control, newFamily As FontFamily)
        instance.Font = New Font(newFamily, instance.Font.Size, instance.Font.Style, instance.Font.Unit, instance.Font.GdiCharSet)
    End Sub

    <Extension()>
    Sub SetFontStyle(instance As Control, style As FontStyle)
        instance.Font = New Font(instance.Font, style)
    End Sub

    <Extension()>
    Sub SetFontSize(instance As Control, fontSize As Single)
        instance.Font = New Font(instance.Font.FontFamily, fontSize * s.UIScaleFactor, instance.Font.Style, instance.Font.Unit, instance.Font.GdiCharSet)
    End Sub

    <Extension()>
    Function ClientMousePos(instance As Control) As Point
        Return instance.PointToClient(Control.MousePosition)
    End Function

    <Extension()>
    Function GetMaxTextSpace(instance As Control, ParamArray values As String()) As String
        Dim ret = ""

        For x = 4 To 2 Step -1
            ret = values.Where(Function(val) val <> "").Join("".PadRight(x))
            Dim testWidth = TextRenderer.MeasureText(ret, instance.Font).Width

            If testWidth < instance.Width Then
                Return ret
            End If
        Next

        Return ret
    End Function

    <Extension()>
    Function SetSymbolAsText(instance As ButtonEx, symbol As Symbol) As ButtonEx
        Dim awesomePath As String = Path.Combine(Folder.Fonts, "Icons", "FontAwesome.ttf")
        Dim segoePath As String = Path.Combine(Folder.Fonts, "Icons", "Segoe-MDL2-Assets.ttf")

        Dim fontFilesExist As Boolean = File.Exists(awesomePath) AndAlso File.Exists(segoePath)
        If Not fontFilesExist Then Return Nothing

        Dim fontCollection As New PrivateFontCollection()
        Dim family As FontFamily = Nothing
        Dim legacy = OSVersion.VersionInfo.dwMajorVersion < 10

        fontCollection.AddFontFile(awesomePath)
        If legacy Then fontCollection.AddFontFile(segoePath)

        If symbol > 61400 Then
            If fontCollection.Families.Count > 0 Then family = fontCollection.Families(0)
        Else
            If legacy Then
                If fontCollection.Families.Count > 1 Then family = fontCollection.Families(1)
            Else
                family = New FontFamily("Segoe MDL2 Assets")
            End If
        End If

        If family IsNot Nothing Then
            instance.Font = New Font(family, instance.Font.Size)
            instance.Text = Convert.ToChar(symbol).ToString()
        End If

        Return instance
    End Function

    <Extension()>
    Function GetAllControls(instance As Control) As IEnumerable(Of Control)
        Dim controls = instance.Controls.Cast(Of Control)
        Return controls.SelectMany(Function(x) x.GetAllControls()).Concat(controls)
    End Function

    <Extension()>
    Function GetAllControls(Of T)(instance As Control) As IEnumerable(Of T)
        Return instance.GetAllControls.OfType(Of T)
    End Function

    <Extension()>
    Sub ClearAllFormatting(instance As RichTextBox)
        instance.Text = instance.Text.ToString()
    End Sub

    <Extension()>
    Sub SelectionFormat(instance As RichTextBox, index As Integer, length As Integer, backColor As ColorHSL, foreColor As ColorHSL, ParamArray fontStyles() As FontStyle)
        If instance Is Nothing Then Return

        instance.SuspendLayout()
        instance.Select(index, length)
        instance.SelectionBackColor = If(backColor.A = 0, instance.SelectionBackColor, backColor.ToColor())
        instance.SelectionColor = foreColor

        If fontStyles IsNot Nothing AndAlso fontStyles.Length > 0 Then
            instance.SelectionFont = New Font(instance.Font, fontStyles.Aggregate(instance.Font.Style, Function(a, n) a Or n))
        End If
        instance.ResumeLayout()
    End Sub
End Module

Module UIExtensions
    <Extension()>
    Sub ClearAndDisplose(instance As ToolStripItemCollection)
        For Each i In instance.OfType(Of IDisposable).ToArray
            i.Dispose()
        Next

        instance.Clear()
    End Sub

    <Extension()>
    Function ResizeToSmallIconSize(img As Image) As Image
        If img IsNot Nothing AndAlso img.Size <> SystemInformation.SmallIconSize Then
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
    Sub SetSelectedPath(d As FolderBrowserDialog, path As String)
        If Not Directory.Exists(path) Then
            path = path.ExistingParent
        End If

        If Directory.Exists(path) Then
            d.SelectedPath = path
        End If
    End Sub

    <Extension()>
    Sub SetInitDir(dialog As FileDialog, ParamArray paths As String())
        For Each path In paths
            If Not Directory.Exists(path) Then
                path = path.ExistingParent
            End If

            If Directory.Exists(path) Then
                dialog.InitialDirectory = path
                Exit For
            End If
        Next
    End Sub

    <Extension()>
    Sub SetFilter(dialog As FileDialog, values As IEnumerable(Of String))
        dialog.Filter = FileTypes.GetFilter(values)
    End Sub

    <Extension()>
    Sub SendMessageCue(te As TextEdit, value As String, hideWhenFocused As Boolean)
        SendMessageCue(te.TextBox, value, hideWhenFocused)
    End Sub

    <Extension()>
    Sub SendMessageCue(tb As TextBox, value As String, hideWhenFocused As Boolean)
        Dim wParam = If(hideWhenFocused, 0, 1)
        Native.SendMessage(tb.Handle, Native.EM_SETCUEBANNER, wParam, value)
    End Sub

    <Extension()>
    Sub SendMessageCue(c As ComboBox, value As String)
        Native.SendMessage(c.Handle, Native.CB_SETCUEBANNER, 1, value)
    End Sub

    Function GetPropertyValue(obj As String, propertyName As String) As Object
        obj.GetType.GetProperty(propertyName).GetValue(obj)
    End Function

    <Extension()>
    Sub RemoveSelection(dgv As DataGridView)
        For Each i As DataGridViewRow In dgv.SelectedRows
            dgv.Rows.Remove(i)
        Next

        If dgv.SelectedRows.Count = 0 AndAlso dgv.RowCount > 0 Then
            dgv.Rows(dgv.RowCount - 1).Selected = True
        End If
    End Sub

    <Extension()>
    Function CanMoveUp(dgv As DataGridView) As Boolean
        Return dgv.SelectedRows.Count > 0 AndAlso dgv.SelectedRows(0).Index > 0
    End Function

    <Extension()>
    Function CanMoveDown(dgv As DataGridView) As Boolean
        Return dgv.SelectedRows.Count > 0 AndAlso dgv.SelectedRows(0).Index < dgv.RowCount - 1
    End Function

    <Extension()>
    Sub MoveSelectionUp(dgv As DataGridView)
        If CanMoveUp(dgv) Then
            Dim bs = DirectCast(dgv.DataSource, BindingSource)
            Dim pos = bs.Position
            bs.RaiseListChangedEvents = False
            Dim current = bs.Current
            bs.Remove(current)
            pos -= 1
            bs.Insert(pos, current)
            bs.Position = pos
            bs.RaiseListChangedEvents = True
            bs.ResetBindings(False)
        End If
    End Sub

    <Extension()>
    Sub MoveSelectionDown(dgv As DataGridView)
        If CanMoveDown(dgv) Then
            Dim bs = DirectCast(dgv.DataSource, BindingSource)
            Dim pos = bs.Position
            bs.RaiseListChangedEvents = False
            Dim current = bs.Current
            bs.Remove(current)
            pos += 1
            bs.Insert(pos, current)
            bs.Position = pos
            bs.RaiseListChangedEvents = True
            bs.ResetBindings(False)
        End If
    End Sub
End Module

Module SymbolExtensions
    <Extension()>
    Function ToChar(symbol As Symbol) As Char
        Return Convert.ToChar(symbol)
    End Function
End Module

Module FontExtensions
    <Extension()>
    Function IsMonospace(fontFamily As FontFamily) As Boolean
        Using bmp As New Bitmap(1, 1)
            Using g As Graphics = Graphics.FromImage(bmp)
                Using f = New Font(fontFamily.Name, 20)
                    Dim w1 = g.MeasureString("ii", f).Width
                    Dim w2 = g.MeasureString("WW", f).Width
                    Return w1 = w2
                End Using
            End Using
        End Using
    End Function

    <Extension()>
    Function IsMonospace(font As Font) As Boolean
        Using bmp As New Bitmap(1, 1)
            Using g As Graphics = Graphics.FromImage(bmp)
                Using f = New Font(font.Name, 20)
                    Dim w1 = g.MeasureString("ii", f).Width
                    Dim w2 = g.MeasureString("WW", f).Width
                    Return w1 = w2
                End Using
            End Using
        End Using
    End Function
End Module