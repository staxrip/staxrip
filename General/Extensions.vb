
Imports System.Drawing.Drawing2D
Imports System.Globalization
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions

Imports Microsoft.Win32
Imports VB6 = Microsoft.VisualBasic

Module StringExtensions
    Private Separator As Char = Path.DirectorySeparatorChar

    <Extension>
    Function TrimTrailingSeparator(instance As String) As String
        If instance = "" Then
            Return ""
        End If

        If instance.EndsWith(Separator) AndAlso Not instance.Length <= 3 Then
            Return instance.TrimEnd(Separator)
        End If

        Return instance
    End Function

    <Extension()>
    Function Parent(path As String) As String
        If path = "" Then
            Return ""
        End If

        Dim temp = TrimTrailingSeparator(path)

        If temp.Contains(Separator) Then
            path = temp.LeftLast(Separator) + Separator
        End If

        Return path
    End Function

    <Extension>
    Function IndentLines(instance As String, value As String) As String
        If instance = "" Then
            Return ""
        End If

        instance = value + instance
        instance = instance.Replace(BR, BR + value)
        Return instance
    End Function

    <Extension>
    Function StartsWithEx(instance As String, value As String) As Boolean
        If instance <> "" AndAlso value <> "" Then
            Return instance.StartsWith(value)
        End If
    End Function

    <Extension>
    Function EndsWithEx(instance As String, value As String) As Boolean
        If instance <> "" AndAlso value <> "" Then
            Return instance.EndsWith(value)
        End If
    End Function

    <Extension>
    Function ContainsEx(instance As String, value As String) As Boolean
        If instance <> "" AndAlso value <> "" Then
            Return instance.Contains(value)
        End If
    End Function

    <Extension>
    Function ToLowerEx(instance As String) As String
        If instance = "" Then
            Return ""
        End If

        Return instance.ToLower
    End Function

    <Extension>
    Function TrimEx(instance As String) As String
        If instance = "" Then
            Return ""
        End If

        Return instance.Trim
    End Function

    <Extension>
    Function PathStartsWith(instance As String, value As String) As Boolean
        If instance <> "" AndAlso value <> "" Then
            Return instance.ToLower.StartsWith(value.ToLower)
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
        If instance = "" Then
            Return False
        End If

        Dim chars = """*/:<>?\|"

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
    Function IsDosCompatible(instance As String) As Boolean
        If instance = "" Then
            Return True
        End If

        Dim bytes = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding(ConsoleHelp.DosCodePage), Encoding.Unicode.GetBytes(instance))
        Return instance = Encoding.Unicode.GetString(Encoding.Convert(Encoding.GetEncoding(ConsoleHelp.DosCodePage), Encoding.Unicode, bytes))
    End Function

    <Extension>
    Function IsANSICompatible(instance As String) As Boolean
        If instance = "" Then
            Return True
        End If

        Dim bytes = Encoding.Convert(Encoding.Unicode, Encoding.Default, Encoding.Unicode.GetBytes(instance))
        Return instance = Encoding.Unicode.GetString(Encoding.Convert(Encoding.Default, Encoding.Unicode, bytes))
    End Function

    <Extension()>
    Function FileName(instance As String) As String
        If instance = "" Then
            Return ""
        End If

        Dim index = instance.LastIndexOf(Path.DirectorySeparatorChar)

        If index > -1 Then
            Return instance.Substring(index + 1)
        End If

        Return instance
    End Function

    <Extension()>
    Function Upper(instance As String) As String
        If instance = "" Then Return ""
        Return instance.ToUpperInvariant
    End Function

    <Extension()>
    Function Lower(instance As String) As String
        If instance = "" Then Return ""
        Return instance.ToLowerInvariant
    End Function

    <Extension()>
    Function ChangeExt(instance As String, value As String) As String
        If instance = "" Then Return ""
        If value = "" Then Return instance
        If Not value.StartsWith(".") Then value = "." + value
        Return instance.DirAndBase + value.ToLower
    End Function

    <Extension()>
    Function Escape(instance As String) As String
        If instance = "" Then
            Return ""
        End If

        For Each i In " ;=~*$%()&"
            If instance.Contains(i) Then
                Return """" + instance + """"
            End If
        Next

        Return instance
    End Function

    <Extension()>
    Function ExistingParent(instance As String) As String
        Dim ret = instance.Parent
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        If Not Directory.Exists(ret) Then ret = ret.Parent Else Return ret
        Return ret
    End Function

    <Extension()>
    Function FileExists(instance As String) As Boolean
        Return File.Exists(instance)
    End Function

    <Extension()>
    Function DirExists(instance As String) As Boolean
        Return Directory.Exists(instance)
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
        If filepath = "" Then
            Return ""
        End If

        Dim chars = filepath.ToCharArray()

        For x = filepath.Length - 1 To 0 Step -1
            If chars(x) = Separator Then
                Return ""
            End If

            If chars(x) = "."c Then
                Return filepath.Substring(x + If(includeDot, 0, 1)).ToLower()
            End If
        Next

        Return ""
    End Function

    <Extension()>
    Function Base(instance As String) As String
        If instance = "" Then
            Return ""
        End If

        Dim ret = instance

        If ret.Contains(Separator) Then
            ret = ret.RightLast(Separator)
        End If

        If ret.Contains(".") Then
            ret = ret.LeftLast(".")
        End If

        Return ret
    End Function

    <Extension()>
    Function Dir(instance As String) As String
        If instance = "" Then
            Return ""
        End If

        If instance.Contains("\") Then
            instance = instance.LeftLast("\") + "\"
        End If

        Return instance
    End Function

    <Extension()>
    Function LongPathPrefix(instance As String) As String
        If instance = "" Then
            Return ""
        End If

        Dim MAX_PATH = 260
        Dim prefix = "\\?\"

        If instance.Length > MAX_PATH AndAlso Not instance.StartsWith(prefix) Then
            Return prefix + instance
        End If

        Return instance
    End Function

    <Extension()>
    Function ToShortPath(instance As String) As String
        If instance = "" Then
            Return ""
        End If

        Dim MAX_PATH = 260

        If instance.Length <= MAX_PATH Then
            Return instance
        End If

        Dim sb As New StringBuilder(500)
        Native.GetShortPathName(instance, sb, sb.Capacity)
        Return sb.ToString
    End Function

    <Extension()>
    Function ToShortFilePath(instance As String) As String
        If instance = "" Then
            Return ""
        End If

        Dim MAX_PATH = 260

        If instance.Length <= MAX_PATH Then
            Return instance
        End If

        Dim sb As New StringBuilder(500)
        Native.GetShortPathName(instance.Dir, sb, sb.Capacity)
        Dim ret = sb.ToString + instance.FileName

        If ret.Length <= MAX_PATH Then
            Return ret
        End If

        Native.GetShortPathName(instance, sb, sb.Capacity)
        Return sb.ToString
    End Function

    <Extension()>
    Function DirName(instance As String) As String
        If instance = "" Then
            Return ""
        End If

        instance = TrimTrailingSeparator(instance)
        Return instance.RightLast(Separator)
    End Function

    <Extension()>
    Function DirAndBase(path As String) As String
        Return path.Dir + path.Base
    End Function

    <Extension()>
    Function ContainsAll(instance As String, ParamArray all As String()) As Boolean
        If instance <> "" Then
            Return all.All(Function(arg) instance.Contains(arg))
        End If
    End Function

    <Extension()>
    Function ContainsAny(instance As String, ParamArray any As String()) As Boolean
        If instance <> "" Then
            Return any.Any(Function(arg) instance.Contains(arg))
        End If
    End Function

    <Extension()>
    Function EqualsAny(instance As String, ParamArray values As String()) As Boolean
        If instance = "" OrElse values.NothingOrEmpty Then
            Return False
        End If

        Return values.Contains(instance)
    End Function

    <Extension()>
    Function FixDir(instance As String) As String
        If instance = "" Then
            Return ""
        End If

        While instance.EndsWith(Separator + Separator)
            instance = instance.Substring(0, instance.Length - 1)
        End While

        If instance.EndsWith(Separator) Then
            Return instance
        End If

        Return instance + Separator
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
        Return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower)
    End Function

    <Extension()>
    Function IsInt(value As String) As Boolean
        Return Integer.TryParse(value, Nothing)
    End Function

    <Extension()>
    Function ToInt(value As String, Optional defaultValue As Integer = 0) As Integer
        If Not Integer.TryParse(value, Nothing) Then
            Return defaultValue
        End If

        Return CInt(value)
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
    Function IsDouble(value As String) As Boolean
        If value <> "" Then
            If value.Contains(",") Then
                value = value.Replace(",", ".")
            End If

            Return Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, Nothing)
        End If
    End Function

    <Extension()>
    Function ToDouble(value As String, Optional defaultValue As Single = 0) As Double
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
        If Not File.Exists(instance) Then
            Return ""
        End If

        Return File.ReadAllText(instance)
    End Function

    <Extension()>
    Function ReadAllTextDefault(instance As String) As String
        If Not File.Exists(instance) Then
            Return ""
        End If

        Return File.ReadAllText(instance, Encoding.Default)
    End Function

    <Extension()>
    Sub WriteFileDefault(instance As String, path As String)
        WriteFile(instance, path, Encoding.Default)
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
        Try
            File.WriteAllText(path, instance, encoding)
        Catch ex As Exception
            g.ShowException(ex)
        End Try
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
        If value = "" OrElse value.Length <= maxLength Then
            Return value
        End If

        Return value.Substring(0, maxLength)
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

Module NumericExtensions
    <Extension()>
    Function ToInvariantString(value As Double, format As String) As String
        Return value.ToString(format, CultureInfo.InvariantCulture)
    End Function
End Module

Module MiscExtensions
    <Extension()>
    Function ToInvariantString(instance As IConvertible) As String
        If instance Is Nothing Then
            Return ""
        End If

        Return instance.ToString(CultureInfo.InvariantCulture)
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
    Function NeutralCulture(ci As CultureInfo) As CultureInfo
        If ci.IsNeutralCulture Then
            Return ci
        Else
            Return ci.Parent
        End If
    End Function

    <Extension()>
    Function NothingOrEmpty(strings As IEnumerable(Of String)) As Boolean
        If strings Is Nothing OrElse strings.Count = 0 Then
            Return True
        End If

        For Each i In strings
            If i = "" Then Return True
        Next
    End Function

    <Extension()>
    Function NothingOrEmpty(objects As IEnumerable(Of Object)) As Boolean
        If objects Is Nothing OrElse objects.Count = 0 Then Return True

        For Each i In objects
            If i Is Nothing Then Return True
        Next
    End Function
End Module

Module RegistryKeyExtensions
    Function GetValue(Of T)(root As RegistryKey, path As String, name As String) As T
        Using key = root.OpenSubKey(path)
            If Not key Is Nothing Then
                Dim value = key.GetValue(name)

                If Not value Is Nothing Then
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
    Function GetBoolean(root As RegistryKey, path As String, name As String) As Boolean
        Return GetValue(Of Boolean)(root, path, name)
    End Function

    <Extension()>
    Function GetKeyNames(root As RegistryKey, path As String) As String()
        Using subKey = root.OpenSubKey(path)
            If Not subKey Is Nothing Then
                Return subKey.GetSubKeyNames
            End If
        End Using

        Return {}
    End Function

    <Extension()>
    Function GetValueNames(root As RegistryKey, path As String) As String()
        Using subKey = root.OpenSubKey(path)
            If Not subKey Is Nothing Then
                Return subKey.GetValueNames
            End If
        End Using

        Return {}
    End Function

    <Extension()>
    Sub Write(root As RegistryKey, path As String, name As String, value As Object)
        Dim subKey = root.OpenSubKey(path, True)

        If subKey Is Nothing Then
            subKey = root.CreateSubKey(path, RegistryKeyPermissionCheck.ReadWriteSubTree)
        End If

        subKey.SetValue(name, value)
        subKey.Close()
    End Sub

    <Extension()>
    Sub DeleteValue(root As RegistryKey, path As String, name As String)
        Using key = root.OpenSubKey(path, True)
            If Not key Is Nothing Then
                key.DeleteValue(name, False)
            End If
        End Using
    End Sub
End Module

Module ControlExtension
    <Extension()>
    Sub ScaleClientSize(instance As Control, width As Single, height As Single)
        instance.ClientSize = New Size(CInt(instance.Font.Height * width), CInt(instance.Font.Height * height))
    End Sub

    <Extension()>
    Sub SetFontStyle(instance As Control, style As FontStyle)
        instance.Font = New Font(instance.Font.FontFamily, instance.Font.Size, style)
    End Sub

    <Extension()>
    Sub SetFontSize(instance As Control, fontSize As Single)
        instance.Font = New Font(instance.Font.FontFamily, fontSize)
    End Sub

    <Extension()>
    Sub AddClickAction(instance As Control, action As Action)
        AddHandler instance.Click, Sub() action()
    End Sub

    <Extension()>
    Function ClientMousePos(instance As Control) As Point
        Return instance.PointToClient(Control.MousePosition)
    End Function

    <Extension()>
    Function GetMaxTextSpace(instance As Control, ParamArray values As String()) As String
        Dim ret As String

        For x = 4 To 2 Step -1
            ret = values.Join("".PadRight(x))
            Dim testWidth = TextRenderer.MeasureText(ret, instance.Font).Width

            If testWidth < instance.Width Then
                Return ret
            End If
        Next

        Return ret
    End Function
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
