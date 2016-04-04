Imports System.Runtime.Serialization
Imports System.ComponentModel
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Xml.Linq
Imports System.Text
Imports System.Security.Permissions
Imports System.Management
Imports System.Security.Principal

Imports StaxRip.UI
Imports VB6 = Microsoft.VisualBasic

Class CommonDirs
    Shared ReadOnly Property Startup() As String
        Get
            Return DirPath.AppendSeparator(Application.StartupPath)
        End Get
    End Property

    Shared ReadOnly Property CurrentDirectory() As String
        Get
            Return DirPath.AppendSeparator(Environment.CurrentDirectory)
        End Get
    End Property

    Shared ReadOnly Property Temp() As String
        Get
            Return DirPath.AppendSeparator(Path.GetTempPath)
        End Get
    End Property

    Shared ReadOnly Property System() As String
        Get
            Return DirPath.AppendSeparator(Environment.SystemDirectory)
        End Get
    End Property

    Shared ReadOnly Property Programs() As String
        Get
            Return DirPath.AppendSeparator(GetFolderPath(Environment.SpecialFolder.ProgramFiles))
        End Get
    End Property

    Shared ReadOnly Property Home() As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.UserProfile).AppendSeparator
        End Get
    End Property

    Shared ReadOnly Property CommonAppData() As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.CommonApplicationData).AppendSeparator
        End Get
    End Property

    Shared ReadOnly Property UserAppDataLocal() As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.LocalApplicationData).AppendSeparator
        End Get
    End Property

    Shared ReadOnly Property UserAppDataRoaming() As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.ApplicationData).AppendSeparator
        End Get
    End Property

    Shared ReadOnly Property Windows() As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.Windows).AppendSeparator
        End Get
    End Property

    <DllImport("shfolder.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function SHGetFolderPath(hwndOwner As IntPtr, nFolder As Integer, hToken As IntPtr, dwFlags As Integer, lpszPath As StringBuilder) As Integer
    End Function

    Shared Function GetFolderPath(folder As Environment.SpecialFolder) As String
        Dim sb As New StringBuilder(260)
        SHGetFolderPath(IntPtr.Zero, CInt(folder), IntPtr.Zero, 0, sb)
        Dim ret = DirPath.AppendSeparator(sb.ToString) '.NET fails on 'D:'
        Call New FileIOPermission(FileIOPermissionAccess.PathDiscovery, ret).Demand()
        Return ret
    End Function
End Class

Class PathBase
    Shared ReadOnly Property Separator() As Char
        Get
            Return Path.DirectorySeparatorChar
        End Get
    End Property

    Shared Function AppendSeparator(path As String) As String
        If path = "" Then Return ""
        If path.EndsWith(Separator) Then Return path
        Return path + Separator
    End Function

    Shared Function IsSameBase(a As String, b As String) As Boolean
        Return Filepath.GetBase(a).EqualIgnoreCase(Filepath.GetBase(b))
    End Function

    Shared Function IsSameDir(a As String, b As String) As Boolean
        Return Filepath.GetDir(a).EqualIgnoreCase(Filepath.GetDir(b))
    End Function

    Shared Function IsValidFileSystemName(name As String) As Boolean
        If name = "" Then Return False
        Dim chars = """*/:<>?\|".ToCharArray

        For Each i In name.ToCharArray
            If chars.Contains(i) Then Return False
            If Convert.ToInt32(i) < 32 Then Return False
        Next

        Return True
    End Function

    Shared Function RemoveIllegalCharsFromName(name As String) As String
        If name = "" Then Return ""

        Dim chars = """*/:<>?\|".ToCharArray

        For Each i In name.ToCharArray
            If chars.Contains(i) Then
                name = name.Replace(i, "_")
            End If
        Next

        For x = 1 To 31
            If name.Contains(Convert.ToChar(x)) Then
                name = name.Replace(Convert.ToChar(x), "_"c)
            End If
        Next

        Return name
    End Function
End Class

Class DirPath
    Inherits PathBase

    Shared Function GetRoot(path As String) As String
        Return Regex.Match(path, "^.:\\").Value
    End Function

    Shared Function TrimTrailingSeparator(path As String) As String
        If path = "" Then Return ""

        If path.EndsWith(Separator) AndAlso Not path.EndsWith(":\") Then
            Return path.TrimEnd(Separator)
        End If

        Return path
    End Function

    Shared Function FixSeperator(path As String) As String
        If path.Contains("\") AndAlso Separator <> "\" Then
            path = path.Replace("\", Separator)
        End If

        If path.Contains("/") AndAlso Separator <> "/" Then
            path = path.Replace("/", Separator)
        End If

        Return path
    End Function

    Shared Function GetParent(path As String) As String
        If path = "" Then Return ""
        Dim temp = TrimTrailingSeparator(path)
        If temp.Contains(Separator) Then path = temp.LeftLast(Separator) + Separator
        Return path
    End Function

    Shared Function GetName(path As String) As String
        If path = "" Then Return ""
        path = TrimTrailingSeparator(path)
        Return path.RightLast(Separator)
    End Function

    Shared Function IsInSysDir(path As String) As Boolean
        If path = "" Then Return False
        If Not path.EndsWith("\") Then path += "\"
        Return path.ToUpper.Contains(CommonDirs.Programs.ToUpper)
    End Function

    Shared Function IsFixedDrive(path As String) As Boolean
        Try
            If path <> "" Then Return New DriveInfo(path).DriveType = DriveType.Fixed
        Catch ex As Exception
        End Try
    End Function
End Class

Class Filepath
    Inherits PathBase

    Private Value As String

    Sub New(path As String)
        Value = path
    End Sub

    Shared Function GetDir(path As String) As String
        If path = "" Then Return ""
        If path.Contains("\") Then path = path.LeftLast("\") + "\"
        Return path
    End Function

    Shared Function GetDirAndBase(path As String) As String
        Return GetDir(path) + GetBase(path)
    End Function

    Shared Function GetName(path As String) As String
        If Not path Is Nothing Then
            Dim index = path.LastIndexOf(IO.Path.DirectorySeparatorChar)

            If index > -1 Then
                Return path.Substring(index + 1)
            End If
        End If

        Return path
    End Function

    Shared Function GetExtFull(filepath As String) As String
        Return GetExt(filepath, True)
    End Function

    Shared Function GetExt(filepath As String) As String
        Return GetExt(filepath, False)
    End Function

    Shared Function GetExt(filepath As String, dot As Boolean) As String
        If filepath = "" Then Return ""
        Dim chars = filepath.ToCharArray

        For x = filepath.Length - 1 To 0 Step -1
            If chars(x) = Separator Then Return ""
            If chars(x) = "."c Then Return filepath.Substring(x + If(dot, 0, 1)).ToLower
        Next

        Return ""
    End Function

    Shared Function GetDirNoSep(path As String) As String
        path = GetDir(path)
        If path.EndsWith(Separator) Then path = TrimSep(path)
        Return path
    End Function

    Shared Function GetBase(path As String) As String
        If path = "" Then Return ""
        Dim ret = path
        If ret.Contains(Separator) Then ret = ret.RightLast(Separator)
        If ret.Contains(".") Then ret = ret.LeftLast(".")
        Return ret
    End Function

    Shared Function TrimSep(path As String) As String
        If path = "" Then Return ""

        If path.EndsWith(Separator) AndAlso Not path.EndsWith(":" + Separator) Then
            Return path.TrimEnd(Separator)
        End If

        Return path
    End Function

    Shared Function GetDirNameOnly(path As String) As String
        Return Filepath.GetDirNoSep(path).RightLast("\")
    End Function

    Shared Function GetShortPath(dir As String,
                                 base1 As String,
                                 base2 As String,
                                 ext As String) As String

        Dim ret = dir + base1 + base2 + "." + ext
        If ret.Length > 260 Then ret = dir + base1.Shorten(10) + base2 + "." + ext
        If ret.Length > 260 Then ret = dir + base1.Shorten(10) + base2.Shorten(10) + "." + ext

        Return ret
    End Function
End Class

Class SafeSerialization
    Shared Sub Serialize(o As Object, path As String)
        Dim list As New List(Of Object)

        For Each i In o.GetType.GetFields(BindingFlags.Public Or
                                          BindingFlags.NonPublic Or
                                          BindingFlags.Instance)

            If Not i.IsNotSerialized Then
                Dim mc As New FieldContainer
                mc.Name = i.Name

                Dim value = i.GetValue(o)

                If Not value Is Nothing Then
                    If IsSimpleType(i.FieldType) Then
                        mc.Value = value
                    Else
                        mc.Value = GetObjectData(value)
                    End If

                    list.Add(mc)
                End If
            End If
        Next

        Dim bf As New BinaryFormatter

        Try
            Using fs As New FileStream(path, FileMode.Create)
                bf.Serialize(fs, list)
            End Using
        Catch
        End Try
    End Sub

    Shared Function Deserialize(instance As Object,
                                path As String,
                                binder As SerializationBinder) As Object

        Dim safeInstance = DirectCast(instance, ISafeSerialization)

        If File.Exists(path) Then
            Dim list As List(Of Object) = Nothing
            Dim bf As New BinaryFormatter

            Using fs As New FileStream(path, FileMode.Open)
                Try
                    list = DirectCast(bf.Deserialize(fs), List(Of Object))
                Catch
                End Try
            End Using

            If Not list Is Nothing Then
                For Each i As FieldContainer In list
                    For Each iFieldInfo In instance.GetType.GetFields(BindingFlags.Public Or
                                                                      BindingFlags.NonPublic Or
                                                                      BindingFlags.Instance)
                        If Not iFieldInfo.IsNotSerialized Then
                            If i.Name = iFieldInfo.Name Then
                                Try
                                    If i.Value.GetType Is GetType(Byte()) Then
                                        iFieldInfo.SetValue(instance, GetObjectInstance(DirectCast(i.Value, Byte()), binder))
                                    Else
                                        iFieldInfo.SetValue(instance, i.Value)
                                    End If
                                Catch ex As Exception
                                    safeInstance.WasUpdated = True
                                End Try
                            End If
                        End If
                    Next
                Next
            End If
        End If

        safeInstance.Init()

        If safeInstance.WasUpdated Then
            safeInstance.WasUpdated = False
            Serialize(instance, path)
        End If

        Return instance
    End Function

    Private Shared Function IsSimpleType(t As Type) As Boolean
        Return t.IsPrimitive OrElse
            t Is GetType(String) OrElse
            t Is GetType(SettingBag(Of String)) OrElse
            t Is GetType(SettingBag(Of Boolean)) OrElse
            t Is GetType(SettingBag(Of Integer)) OrElse
            t Is GetType(SettingBag(Of Double)) OrElse
            t Is GetType(SettingBag(Of Single))
    End Function

    <DebuggerNonUserCode()>
    Private Shared Function GetObjectInstance(ba As Byte(), binder As SerializationBinder) As Object
        Using ms As New MemoryStream(ba)
            Dim bf As New BinaryFormatter

            If Not binder Is Nothing Then
                bf.Binder = binder
            End If

            Return bf.Deserialize(ms)
        End Using
    End Function

    Private Shared Function GetObjectData(o As Object) As Byte()
        Using ms As New MemoryStream
            Dim bf As New BinaryFormatter
            bf.Serialize(ms, o)
            Return ms.ToArray()
        End Using
    End Function

    <Serializable()>
    Class FieldContainer
        Public Value As Object
        Public Name As String
    End Class

    Shared Function Check(iface As ISafeSerialization,
                          obj As Object,
                          key As String,
                          version As Integer) As Boolean

        If obj Is Nothing OrElse
            Not iface.Versions.ContainsKey(key) OrElse
            iface.Versions(key) <> version Then

            iface.Versions(key) = version
            iface.WasUpdated = True
            Return True
        End If
    End Function
End Class

Public Interface ISafeSerialization
    Property WasUpdated() As Boolean
    ReadOnly Property Versions() As Dictionary(Of String, Integer)
    Sub Init()
End Interface

Class Sorter(Of T)
    Private List As New List(Of SortObject(Of T))

    Sub Add(sortValue As IComparable, o As T)
        List.Add(New SortObject(Of T)(sortValue, o))
    End Sub

    Function GetSortedList() As List(Of T)
        Dim r As New List(Of T)
        List.Sort()

        For Each i In List
            r.Add(i.Value)
        Next

        Return r
    End Function

    Private Class SortObject(Of T2)
        Implements IComparable

        Public SortValue As IComparable
        Public Value As T2

        Sub New(sortValue As IComparable, o As T2)
            Me.SortValue = sortValue
            Me.Value = o
        End Sub

        Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Return SortValue.CompareTo(CType(obj, SortObject(Of T2)).SortValue)
        End Function
    End Class
End Class

Class HelpDocument
    Private Path As String
    Private Title As String
    Private IsClosed As Boolean

    Property Writer As XmlTextWriter

    Sub New(path As String)
        Me.Path = path
    End Sub

    Sub WriteStart(title As String)
        WriteStart(title, True)
    End Sub

    Sub WriteStart(title As String, showTitle As Boolean)
        Dim script As XElement =
<script type="text/javascript">
</script>

        Dim style As XElement =
<style type="text/css">
body {
    background-image: url('StaxRipBackground.gif');
    background-repeat: repeat-x;
    font-family: Arial,Helvetica,sans-serif;
}

h1 {
    font-size: 150%;
    color: #333333;
    margin-bottom: -4pt;
}

h2 {
    font-size: 120%;
    color: #666666;
    margin-bottom: -8pt;
}

h3 {
    font-size: 100%;
    color: #333333;
    margin-bottom: -8pt;
}

a {
    color: #666666;
}

td {
    width: 50%;
    vertical-align: top;
}

table {
    table-layout: fixed;
}

</style>

        Me.Title = title
        Writer = New XmlTextWriter(Path, Encoding.UTF8)
        Writer.Formatting = Formatting.Indented
        Writer.WriteStartElement("html")
        Writer.WriteStartElement("head")
        Writer.WriteElementString("title", title)
        Writer.WriteRaw(CrLf + style.ToString + CrLf)
        Writer.WriteRaw(CrLf + script.ToString + CrLf)
        Writer.WriteEndElement() 'head
        Writer.WriteStartElement("body")
        WriteElement("p", "<img src=""" + HelpForm.MainImagePath + """ style=""margin-bottom:-8pt;margin-left:-4pt"">")

        If showTitle Then Writer.WriteElementString("h1", title)
        Dim dummy = HelpForm.BackgroundImagePath
    End Sub

    Sub WriteP(rawText As String)
        WriteElement("p", rawText)
    End Sub

    Sub WriteH2(rawText As String)
        WriteElement("h2", rawText)
    End Sub

    Sub WriteH3(rawText As String)
        WriteElement("h3", rawText)
    End Sub

    Sub WriteP(title As String, rawText As String)
        WriteElement("h2", title)
        WriteElement("p", rawText)
    End Sub

    Sub WriteElement(elementName As String, rawText As String)
        If rawText = "" Then Exit Sub

        Writer.WriteStartElement(elementName)

        If MustConvert(rawText) Then
            Writer.WriteRaw(ConvertMarkup(rawText, False))
        Else
            Writer.WriteRaw(rawText)
        End If

        Writer.WriteEndElement()
    End Sub

    Shared Function ConvertChars(value As String) As String
        value = value.FixBreak

        If value.Contains("<") Then value = value.Replace("<", "&lt;")
        If value.Contains(">") Then value = value.Replace(">", "&gt;")
        If value.Contains(CrLf) Then value = value.Replace(CrLf, "<br>")

        Return value
    End Function

    Shared Function ConvertMarkup(value As String, stripOnly As Boolean) As String
        If stripOnly Then
            If value.Contains("[") Then
                Dim re As New Regex("\[(.+?) (.+?)\]")

                If re.IsMatch(value) Then
                    value = re.Replace(value, "$2")
                End If
            End If

            If value.Contains("'''") Then
                Dim re As New Regex("'''(.+?)'''")

                If re.IsMatch(value) Then
                    value = re.Replace(value, "$1")
                End If
            End If
        Else
            If value.Contains("[") Then
                Dim re As New Regex("\[(.+?) (.+?)\]")
                Dim m = re.Match(value)

                If m.Success Then
                    value = re.Replace(value, "<a href=""$1"">$2</a>")
                End If
            End If

            If value.Contains("'''") Then
                Dim re As New Regex("'''(.+?)'''")
                Dim m = re.Match(value)

                If m.Success Then
                    value = re.Replace(value, "<b>$1</b>")
                End If
            End If
        End If

        Return value
    End Function

    Shared Function MustConvert(value As String) As Boolean
        If value <> "" AndAlso (value.Contains("[") OrElse value.Contains("'''")) Then
            Return True
        End If
    End Function

    Sub WriteTips(ParamArray tips As StringPairList())
        If tips.IsAnythingNothingOrEmpty Then Exit Sub

        Dim l As New StringPairList

        For Each i In tips
            l.AddRange(i)
        Next

        l.Sort()

        For Each i In l
            WriteH3(HelpDocument.ConvertChars(i.Name))
            WriteP(HelpDocument.ConvertChars(i.Value))
        Next
    End Sub

    Sub WriteList(ParamArray values As String())
        Writer.WriteStartElement("ul")

        For Each i As String In values
            Writer.WriteStartElement("li")
            Writer.WriteRaw(ConvertMarkup(i, False))
            Writer.WriteEndElement()
        Next

        Writer.WriteEndElement()
    End Sub

    Sub WriteTable(list As IEnumerable(Of StringPair))
        WriteTable(Nothing, Nothing, New StringPairList(list), True)
    End Sub

    Sub WriteTable(list As StringPairList)
        WriteTable(Nothing, Nothing, list, True)
    End Sub

    Sub WriteTable(title As String, list As StringPairList)
        WriteTable(title, Nothing, list, True)
    End Sub

    Sub WriteTable(title As String, list As StringPairList, sort As Boolean)
        WriteTable(title, Nothing, list, sort)
    End Sub

    Sub WriteTable(title As String, text As String, list As StringPairList)
        WriteTable(title, text, list, True)
    End Sub

    Private Sub WriteTable(title As String, text As String, list As StringPairList, sort As Boolean)
        If sort Then list.Sort()

        If Not title Is Nothing Then
            Writer.WriteElementString("h2", title)
        End If

        If text Is Nothing Then
            Writer.WriteElementString("p", "")
        Else
            WriteP(text)
        End If

        Writer.WriteStartElement("table")
        Writer.WriteAttributeString("border", "1")
        Writer.WriteAttributeString("cellspacing", "0")
        Writer.WriteAttributeString("cellpadding", "3")
        Writer.WriteAttributeString("bordercolordark", "white")
        Writer.WriteAttributeString("bordercolorlight", "black")

        For Each i As StringPair In list
            Writer.WriteStartElement("tr")
            Writer.WriteStartElement("td")
            WriteElement("p", HelpDocument.ConvertChars(i.Name))
            Writer.WriteEndElement() 'td
            Writer.WriteStartElement("td")

            If i.Value Is Nothing Then
                WriteElement("p", "&nbsp;")
            Else
                WriteElement("p", HelpDocument.ConvertChars(i.Value))
            End If

            Writer.WriteEndElement() 'td

            Writer.WriteEndElement() 'tr
        Next

        Writer.WriteEndElement() 'table
    End Sub

    Sub WriteDocument()
        If Not IsClosed Then
            IsClosed = True

            Writer.WriteRaw("<p>&nbsp;</p>" + CrLf)
            Writer.WriteRaw("<h5 align=""center"">Copyright &copy; " & DateTime.Now.Year & " by stax76. All rights reserved.</h5><br>")
            Writer.WriteEndElement() 'body
            Writer.WriteEndElement() 'html
            Writer.Close()
        End If
    End Sub

    Sub WriteDocument(browser As WebBrowser)
        WriteDocument()
        browser.Navigate(Path)
    End Sub
End Class

<Serializable()>
Class SettingBag(Of T)
    Sub New()
    End Sub

    Sub New(value As T)
        Me.Value = value
    End Sub

    Overridable Property Value As T
End Class

Class FieldSettingBag(Of T)
    Inherits SettingBag(Of T)

    Private Obj As Object
    Private Name As String

    Sub New(obj As Object, fieldName As String)
        Me.Obj = obj
        Me.Name = fieldName
    End Sub

    Overrides Property Value() As T
        Get
            Return DirectCast(Obj.GetType.GetField(Name).GetValue(Obj), T)
        End Get
        Set(value As T)
            Obj.GetType.GetField(Name).SetValue(Obj, value)
        End Set
    End Property
End Class

Class ReflectionSettingBag(Of T)
    Inherits SettingBag(Of T)

    Private Obj As Object
    Private Name As String

    Sub New(obj As Object, name As String)
        Me.Obj = obj
        Me.Name = name
    End Sub

    Overrides Property Value() As T
        Get
            Dim f = Obj.GetType.GetField(Name, BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)

            If Not f Is Nothing Then
                Return DirectCast(f.GetValue(Obj), T)
            Else
                Return DirectCast(Obj.GetType.GetProperty(Name).GetValue(Obj, Nothing), T)
            End If
        End Get
        Set(value As T)
            Dim f = Obj.GetType.GetField(Name)

            If Not f Is Nothing Then
                f.SetValue(Obj, value)
            Else
                Obj.GetType.GetProperty(Name).SetValue(Obj, value, Nothing)
            End If
        End Set
    End Property
End Class

<Serializable>
Class StringPair
    Implements IComparable(Of StringPair)

    Property Name As String
    Property Value As String

    Sub New()
    End Sub

    Sub New(name As String, text As String)
        Me.Name = name
        Me.Value = text
    End Sub

    Function CompareTo(other As StringPair) As Integer Implements System.IComparable(Of StringPair).CompareTo
        Return Name.CompareTo(other.Name)
    End Function
End Class

Class Misc
    Public Shared IsAdmin As Boolean = New WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)

    Shared Sub PlayAudioFile(path As String, volume As Integer)
        Try
            Static r As New Reflector("WMPlayer.OCX.7")
            Dim s = r.Invoke("settings", BindingFlags.GetProperty)
            s.Invoke("volume", BindingFlags.SetProperty, volume)
            s.Invoke("setMode", "loop", False)
            r.Invoke("URL", BindingFlags.SetProperty, path)
            r.Invoke("controls", BindingFlags.GetProperty).Invoke("play")
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    Shared Function Eval(value As String) As String
        Static dt As New DataTable()
        Static dr As DataRow = dt.Rows.Add()
        Static dc As DataColumn = dt.Columns.Add()

        dc.Expression = value
        Dim r = dr(0).ToString

        If r.Contains(",") Then
            r = r.Replace(",", ".")
        End If

        Return r
    End Function

    Private Shared Function EvalMatch(match As Match) As String
        If match.Groups(1).Value.Contains("http://") Then
            Return "[url=" + match.Groups(1).Value + "]" + match.Groups(2).Value + "[/url]"
        Else
            Return match.Groups(2).Value
        End If
    End Function

    Shared Sub SendPaste(value As String)
        Dim tmp = Clipboard.GetText()
        value.ToClipboard()
        SendKeys.SendWait("^v")
        tmp.ToClipboard()
    End Sub

    Shared Function Validate(value As String, pattern As String) As Boolean
        If Not Regex.IsMatch(value, pattern) Then
            MsgWarn("""" + value + """ is no valid input.")
            Return False
        End If

        Return True
    End Function
End Class

Class ErrorAbortException
    Inherits ApplicationException

    Property Title As String

    Sub New(title As String, message As String)
        MyBase.New(message)
        Me.Title = title
        Log.WriteHeader(title)
        Log.WriteLine(message)
    End Sub
End Class

Class AbortException
    Inherits ApplicationException
End Class

Class CLIArg
    Sub New(value As String)
        Me.Value = value
    End Sub

    Property Value As String

    Shared Function GetArgs(a As String()) As List(Of CLIArg)
        Dim ret As New List(Of CLIArg)
        Dim args As New List(Of String)(a)
        args.RemoveAt(0)

        For Each i As String In args
            ret.Add(New CLIArg(i))
        Next

        Return ret
    End Function

    Function IsMatch(ParamArray values As String()) As Boolean
        For Each i As String In values
            i = i.ToUpper
            Dim val As String = Value.ToUpper

            If "-" + i = val OrElse "/" + i = val OrElse
                val.ToUpper.StartsWith("-" + i + ":") OrElse
                val.ToUpper.StartsWith("/" + i + ":") Then

                Return True
            End If
        Next
    End Function

    Function GetInt() As Integer
        Return CInt(Value.Right(":"))
    End Function

    Function GetString() As String
        Return Value.Right(":").Trim(""""c)
    End Function

    Function IsFile() As Boolean
        Return File.Exists(Value)
    End Function
End Class

<Serializable()>
Class StringPairList
    Inherits List(Of StringPair)

    Sub New()
    End Sub

    Sub New(list As IEnumerable(Of StringPair))
        AddRange(list)
    End Sub

    Overloads Sub Add(name As String, text As String)
        Add(New StringPair(name, text))
    End Sub
End Class

<Serializable()>
Class CommandParameters
    Sub New(methodName As String, ParamArray params As Object())
        Me.MethodName = methodName
        Parameters = New List(Of Object)(params)
    End Sub

    Property MethodName As String
    Property Parameters As List(Of Object)
End Class

Class Command
    Implements IComparable(Of Command)

    Property Attribute As CommandAttribute
    Property MethodInfo As MethodInfo

    Function FixParameters(params As List(Of Object)) As List(Of Object)
        Dim copiedParams As New List(Of Object)(params)
        Dim checkedParams As New List(Of Object)(GetDefaultParameters)

        For i = 0 To checkedParams.Count - 1
            If copiedParams.Count > 0 Then
                If checkedParams(i).GetType Is copiedParams(0).GetType Then
                    checkedParams(i) = copiedParams(0)
                    copiedParams.RemoveAt(0)
                End If
            End If
        Next

        Return checkedParams
    End Function

    Function GetDefaultParameters() As List(Of Object)
        Dim l As New List(Of Object)

        For Each iParameterInfo In MethodInfo.GetParameters
            If Not iParameterInfo.ParameterType.IsValueType AndAlso
                Not iParameterInfo.ParameterType Is GetType(String) Then

                Throw New Exception("Methods must have string or value type params.")
            End If

            Dim a = DirectCast(iParameterInfo.GetCustomAttributes(GetType(DefaultValueAttribute), False), DefaultValueAttribute())

            If a.Length > 0 Then
                l.Add(a(0).Value)
            Else
                If iParameterInfo.ParameterType Is GetType(String) Then
                    l.Add("")
                ElseIf iParameterInfo.ParameterType.IsValueType Then
                    l.Add(Activator.CreateInstance(iParameterInfo.ParameterType))
                End If
            End If
        Next

        Return l
    End Function

    Overrides Function ToString() As String
        If Not Attribute Is Nothing Then
            Return Attribute.Name
        End If

        Return ""
    End Function

    Property [Object] As Object

    Function CompareTo(other As Command) As Integer Implements System.IComparable(Of Command).CompareTo
        Return Attribute.Name.CompareTo(other.Attribute.Name)
    End Function

    Shared Sub PopulateCommandMenu(items As ToolStripItemCollection, commands As List(Of Command), clickSub As Action(Of Command))
        commands.Sort()

        For Each i In commands
            ActionMenuItem.Add(items, i.Attribute.Name, clickSub, i, i.Attribute.Description)
        Next
    End Sub

    Function GetParameterHelp(parameters As List(Of Object)) As String
        If parameters.Count > 0 Then
            Dim paramList As New List(Of String)
            Dim params As ParameterInfo() = MethodInfo.GetParameters

            For iParams As Integer = 0 To params.Length - 1
                Dim paramInfo As ParameterInfo = params(iParams)
                Dim attributs As Object() = paramInfo.GetCustomAttributes(GetType(DispNameAttribute), False)

                If attributs.Length > 0 Then
                    paramList.Add(DirectCast(attributs(0), DispNameAttribute).DisplayName + " = " + FixParameters(parameters)(iParams).ToString)
                End If
            Next

            Return String.Join(", ", paramList.ToArray)
        End If

        Return Nothing
    End Function
End Class

<AttributeUsage(AttributeTargets.Method)>
Class CommandAttribute
    Inherits Attribute

    Sub New(name As String, description As String)
        Me.Name = name
        Me.Description = description
    End Sub

    Property Name As String
    Property Description As String
    Property Switch As String
End Class

Class CommandManager
    Property Commands As New Dictionary(Of String, Command)

    Function HasCommand(name As String) As Boolean
        Return Not name Is Nothing AndAlso Commands.ContainsKey(name)
    End Function

    Function GetCommand(name As String) As Command
        If HasCommand(name) Then Return Commands(name)
    End Function

    Sub AddCommandsFromObject(obj As Object)
        For Each i As MethodInfo In obj.GetType.GetMethods(BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public)
            Dim attributes As CommandAttribute() = DirectCast(i.GetCustomAttributes(GetType(CommandAttribute), False), CommandAttribute())

            If attributes.Length > 0 Then
                Dim c As New Command

                c.MethodInfo = i
                c.Attribute = attributes(0)
                c.Object = obj

                AddCommand(c)
            End If
        Next
    End Sub

    Sub AddCommand(command As Command)
        If Commands.ContainsKey(command.MethodInfo.Name) Then
            MsgWarn("Command '" + command.MethodInfo.Name + "' already exists.")
        Else
            Commands(command.MethodInfo.Name) = command
        End If
    End Sub

    Sub Process(cp As CommandParameters)
        If Not cp Is Nothing Then
            Process(cp.MethodName, cp.Parameters)
        End If
    End Sub

    Sub Process(name As String, params As List(Of Object))
        If HasCommand(name) Then Process(GetCommand(name), params)
    End Sub

    Sub Process(command As Command, params As List(Of Object))
        Try
            command.MethodInfo.Invoke(command.Object, command.FixParameters(params).ToArray)
        Catch ex As TargetParameterCountException
            MsgError("Parameter mismatch, for the command :" + command.Attribute.Name)
        End Try
    End Sub

    Function ProcessCommandLineArgument(value As String) As Boolean
        For Each i As Command In Commands.Values
            Dim s = i.Attribute.Name.ToUpper.Replace(" ", "")

            Dim switches As New List(Of String)
            switches.Add(s)

            If i.Attribute.Switch <> "" Then
                switches.AddRange(i.Attribute.Switch.ToUpper.SplitNoEmpty("|").Select(Function(arg) arg.Replace("-", "|")))
            End If

            For Each iSwitch As String In switches
                If value.ToUpper = "-" + iSwitch OrElse value.ToUpper = "/" + iSwitch Then
                    Process(i.MethodInfo.Name, New List(Of Object))
                    Return True
                Else
                    Dim test = value.Replace("/", "|").Replace("-", "|").ToUpper

                    If test.StartsWith("|" + iSwitch + ":") Then
                        Dim mc = Regex.Matches(value.Right(":"), """(?<a>.+?)""|(?<a>[^,]+)")
                        Dim args As New List(Of Object)

                        For Each iMatch As Match In mc
                            args.Add(iMatch.Groups("a").Value)
                        Next

                        Dim params = i.MethodInfo.GetParameters

                        For x = 0 To params.Length - 1
                            If args.Count > x Then
                                args(x) = TypeDescriptor.GetConverter(params(x).ParameterType).ConvertFrom(args(x))
                            End If
                        Next

                        Process(i.MethodInfo.Name, args)
                        Return True
                    End If
                End If
            Next
        Next
    End Function

    Function GetTips() As StringPairList
        Dim l As New StringPairList

        For Each i As Command In Commands.Values
            If Not i.Attribute.Description Is Nothing Then
                l.Add(New StringPair(i.Attribute.Name, i.Attribute.Description))
            End If
        Next

        Return l
    End Function
End Class

Friend Module MainModule
    Friend Const CrLf As String = VB6.vbCrLf
    Friend Const CrLf2 As String = VB6.vbCrLf + VB6.vbCrLf

    Function OK(value As String) As Boolean
        Return value <> ""
    End Function

    Sub MsgInfo(text As String, Optional content As String = Nothing)
        Msg(text, content, MsgIcon.Info, MessageBoxButtons.OK)
    End Sub

    Sub MsgError(text As String, Optional content As String = Nothing)
        If text = "" Then text = content
        If text = "" Then Exit Sub

        Using td As New TaskDialog(Of String)
            td.AllowCancel = False

            If content Is Nothing Then
                If text.Length < 80 Then
                    td.MainInstruction = text
                Else
                    td.Content = text
                End If
            Else
                td.MainInstruction = text
                td.Content = content
            End If

            td.MainIcon = TaskDialogIcon.Error
            td.Footer = Strings.TaskDialogFooter
            td.Show()
        End Using
    End Sub

    Sub MsgWarn(text As String, Optional content As String = Nothing)
        Msg(text, content, MsgIcon.Warning, MessageBoxButtons.OK)
    End Sub

    Function MsgOK(text As String) As Boolean
        Return Msg(text, Nothing, MsgIcon.Question, MessageBoxButtons.OKCancel) = DialogResult.OK
    End Function

    Function MsgQuestion(text As String,
                         Optional buttons As MessageBoxButtons = MessageBoxButtons.OKCancel) As DialogResult
        Return Msg(text, Nothing, MsgIcon.Question, buttons)
    End Function

    Function MsgQuestion(heading As String,
                         content As String,
                         Optional buttons As MessageBoxButtons = MessageBoxButtons.OKCancel) As DialogResult
        Return Msg(heading, content, MsgIcon.Question, buttons)
    End Function

    Function Msg(text As String,
                 icon As MsgIcon,
                 buttons As MessageBoxButtons) As DialogResult

        Return Msg(text, Nothing, icon, buttons)
    End Function

    Function Msg(mainInstruction As String,
                 content As String,
                 icon As MsgIcon,
                 buttons As MessageBoxButtons,
                 Optional defaultButton As DialogResult = DialogResult.None) As DialogResult

        If mainInstruction Is Nothing Then mainInstruction = ""

        Using td As New TaskDialog(Of DialogResult)
            td.AllowCancel = False
            td.DefaultButton = defaultButton

            If content Is Nothing Then
                If mainInstruction.Length < 80 Then
                    td.MainInstruction = mainInstruction
                Else
                    td.Content = mainInstruction
                End If
            Else
                td.MainInstruction = mainInstruction
                td.Content = content
            End If

            Select Case icon
                Case MsgIcon.Error
                    td.MainIcon = TaskDialogIcon.Error
                Case MsgIcon.Warning
                    td.MainIcon = TaskDialogIcon.Warn
                Case MsgIcon.Info
                    td.MainIcon = TaskDialogIcon.Info
            End Select

            Select Case buttons
                Case MessageBoxButtons.OK
                    td.CommonButtons = TaskDialogButtons.Ok
                Case MessageBoxButtons.OKCancel
                    td.CommonButtons = TaskDialogButtons.OkCancel
                Case MessageBoxButtons.YesNo
                    td.CommonButtons = TaskDialogButtons.YesNo
                Case MessageBoxButtons.YesNoCancel
                    td.CommonButtons = TaskDialogButtons.YesNoCancel
                Case MessageBoxButtons.RetryCancel
                    td.CommonButtons = TaskDialogButtons.RetryCancel
                Case MessageBoxButtons.AbortRetryIgnore
                    Throw New NotSupportedException
            End Select

            Return td.Show()
        End Using
    End Function
End Module

Class Reflector
    Public Type As Type
    Private BasicFlags As BindingFlags = BindingFlags.Static Or BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic

    Sub New(obj As Object)
        Me.Value = obj
    End Sub

    Sub New(obj As Object, type As Type)
        Me.Value = obj
        Me.Type = type
    End Sub

    Sub New(progID As String)
        Me.New(Activator.CreateInstance(Type.GetTypeFromProgID(progID)))
    End Sub

    Private ValueValue As Object

    Property Value() As Object
        Get
            Return ValueValue
        End Get
        Set(Value As Object)
            ValueValue = Value

            If Not Value Is Nothing Then
                Type = Value.GetType
            End If
        End Set
    End Property

    Function ToBool() As Boolean
        Return DirectCast(Value, Boolean)
    End Function

    Function ToInt() As Integer
        Return DirectCast(Value, Integer)
    End Function

    Overrides Function ToString() As String
        Return DirectCast(Value, String)
    End Function

    Function Cast(Of T)() As T
        Return DirectCast(Value, T)
    End Function

    Function Invoke(name As String) As Reflector
        'returns overloads as single MemberInfo
        For Each i In Type.GetMember(name, BasicFlags)
            Select Case i.MemberType
                Case MemberTypes.Property
                    Return Invoke(name, BasicFlags Or BindingFlags.GetProperty, New Object() {})
                Case MemberTypes.Field
                    Return Invoke(name, BasicFlags Or BindingFlags.GetField, New Object() {})
                Case MemberTypes.Method
                    Return Invoke(name, BasicFlags Or BindingFlags.InvokeMethod, New Object() {})
            End Select
        Next

        Return Invoke(name, BindingFlags.InvokeMethod) 'COM
    End Function

    Function Invoke(name As String, ParamArray args As Object()) As Reflector
        'returns overloads as single MemberInfo
        For Each i In Type.GetMember(name, BasicFlags)
            Select Case i.MemberType
                Case MemberTypes.Property
                    Return Invoke(name, BasicFlags Or BindingFlags.SetProperty, args)
                Case MemberTypes.Field
                    Return Invoke(name, BasicFlags Or BindingFlags.SetField, args)
                Case MemberTypes.Method
                    Return Invoke(name, BasicFlags Or BindingFlags.InvokeMethod, args)
                Case Else
                    Throw New NotImplementedException
            End Select
        Next

        Return Invoke(name, BindingFlags.InvokeMethod, args) 'COM 
    End Function

    Function Invoke(name As String, flags As BindingFlags, ParamArray args As Object()) As Reflector
        Return New Reflector(Type.InvokeMember(name, flags, Nothing, Value, args))
    End Function
End Class

Class Shutdown
    Shared Sub Commit(mode As ShutdownMode)
        Select Case mode
            Case ShutdownMode.Standby
                SetSuspendState(False, False, False)
            Case ShutdownMode.Hibernate
                SetSuspendState(True, False, False)
            Case ShutdownMode.Shutdown
                'Const Shutdown = 1
                Const ForcedShutdown = 5
                'Const PowerOff = 8
                'Const ForcedPowerOff = 12
                Dim mc As New ManagementClass("Win32_OperatingSystem")
                mc.Scope.Options.EnablePrivileges = True
                Dim params = mc.GetMethodParameters("Win32Shutdown")
                params("Flags") = ForcedShutdown.ToString
                DirectCast(mc.GetInstances()(0), ManagementObject).InvokeMethod("Win32Shutdown", params, Nothing)
        End Select
    End Sub

    Declare Function SetSuspendState Lib "powrprof.dll" (hibernate As Boolean, forceCritical As Boolean, disableWakeEvent As Boolean) As Boolean
End Class

Public Enum ShutdownMode
    [Nothing]
    Close
    Standby
    Hibernate
    Shutdown
End Enum

Class NativeWindow
    Private Shared WindowHandles As New ArrayList
    Private Shared ChildHandles As New ArrayList

    Shared Sub Show(title As String)
        Show(title, Native.SW_RESTORE)
    End Sub

    Private Shared Sub Show(title As String, nCmdShow As Integer)
        For Each handle In GetHandles()
            If GetTitle(handle).Contains(title) Then
                Native.ShowWindow(handle, nCmdShow)
            End If
        Next
    End Sub

    Shared Sub Hide(handle As IntPtr)
        Native.ShowWindow(handle, Native.SW_HIDE)
    End Sub

    Shared Sub Hide(title As String)
        For Each handle As IntPtr In GetHandles()
            If GetTitle(handle).Contains(title) Then
                Native.ShowWindow(handle, Native.SW_HIDE)
            End If
        Next
    End Sub

    Shared Function GetHandles() As IntPtr()
        Native.EnumWindows(AddressOf EnumWindowsProc, Nothing)
        Dim winHandles(WindowHandles.Count - 1) As IntPtr
        WindowHandles.CopyTo(winHandles)
        WindowHandles.Clear()
        Return winHandles
    End Function

    Private Shared Function EnumWindowsProc(handle As IntPtr, parameter As Integer) As Boolean
        WindowHandles.Add(handle)
        Return True
    End Function

    Shared Function GetTitle(handle As IntPtr) As String
        Dim sb As New StringBuilder(255)
        Native.GetWindowText(handle, sb, sb.Capacity)
        Return sb.ToString
    End Function

    Private Shared Function GetChildHandles(handle As IntPtr) As IntPtr()
        Native.EnumChildWindows(handle, AddressOf HandleChildCallback, Nothing)
        Dim cHandles(ChildHandles.Count - 1) As IntPtr
        ChildHandles.CopyTo(cHandles)
        ChildHandles.Clear()
        Return cHandles
    End Function

    Private Shared Function HandleChildCallback(handle As IntPtr, parameter As Integer) As Boolean
        ChildHandles.Add(handle)
        Return True
    End Function

    Shared Function GetChildText(handle As IntPtr) As String
        Dim ip As IntPtr = Marshal.AllocHGlobal(1000)
        Native.SendMessage(handle, Native.WM_GETTEXT, New IntPtr(1000), ip)
        Dim ret = Marshal.PtrToStringAuto(ip)
        Marshal.FreeHGlobal(ip)
        Return ret
    End Function

    Shared Function GetHandle(title As String) As IntPtr
        For Each handle As IntPtr In GetHandles()
            If GetTitle(handle).Contains(title) Then
                Return handle
            End If
        Next
    End Function

    Shared Function Exists(title As String, text As String) As Boolean
        Dim currentTitle, currentChildTitle As String

        For Each handle In GetHandles()
            currentTitle = GetTitle(handle)

            If currentTitle <> "" AndAlso currentTitle.Contains(title) Then
                For Each childhandle In GetChildHandles(handle)
                    currentChildTitle = GetChildText(childhandle)

                    If currentChildTitle <> "" AndAlso currentChildTitle.Contains(text) Then
                        Return True
                    End If
                Next
            End If
        Next
    End Function

    Shared Sub Close(title As String)
        Dim handle As IntPtr
        Dim currentTitle As String

        For Each handle In GetHandles()
            currentTitle = GetTitle(handle)

            If currentTitle <> "" AndAlso currentTitle.Contains(title) Then
                Native.SendMessage(handle, Native.WM_CLOSE, IntPtr.Zero, IntPtr.Zero)
            End If
        Next
    End Sub
End Class

Public Enum ToolStripRenderMode
    <DispName("System Window Color")> SystemAuto
    <DispName("System Default Color")> SystemDefault
    <DispName("Win 7 Window Color")> Win7Auto
    <DispName("Win 7 Default Color")> Win7Default
    <DispName("Win 8 Window Color")> Win8Auto
    <DispName("Win 8 Default Color")> Win8Default
    <DispName(".NET Professional")> Professional
End Enum

Class PowerRequest
    Private Shared CurrentPowerRequest As IntPtr

    Shared Sub SuppressStandby()
        If OSVersion.Current < OSVersion.Windows7 Then Exit Sub

        'Clear current power request if there is any.
        If CurrentPowerRequest <> IntPtr.Zero Then
            PowerClearRequest(CurrentPowerRequest, PowerRequestType.PowerRequestSystemRequired)
            CurrentPowerRequest = IntPtr.Zero
        End If

        'Create new power request.
        Dim pContext As POWER_REQUEST_CONTEXT
        pContext.Flags = &H1 'POWER_REQUEST_CONTEXT_SIMPLE_STRING
        pContext.Version = 0 'POWER_REQUEST_CONTEXT_VERSION
        'This is the reason for standby suppression. It is shown when the command "powercfg -requests" is executed.
        pContext.SimpleReasonString = "Standby suppressed by StaxRip.exe"

        CurrentPowerRequest = PowerCreateRequest(pContext)

        If CurrentPowerRequest = IntPtr.Zero Then
            Dim err = Marshal.GetLastWin32Error()

            If err <> 0 Then
                Throw New Win32Exception(err)
            End If
        End If

        Dim success = PowerSetRequest(CurrentPowerRequest, PowerRequestType.PowerRequestSystemRequired)

        If Not success Then
            'Failed to set power request.
            CurrentPowerRequest = IntPtr.Zero
            Dim err = Marshal.GetLastWin32Error()

            If err <> 0 Then
                Throw New Win32Exception(err)
            End If
        End If
    End Sub

    Shared Sub EnableStandby()
        If OSVersion.Current < OSVersion.Windows7 Then Exit Sub

        'only try to clear power request if any power request is set.
        If CurrentPowerRequest <> IntPtr.Zero Then
            Dim success = PowerClearRequest(CurrentPowerRequest, PowerRequestType.PowerRequestSystemRequired)

            If Not success Then
                'Failed to clear power request.
                CurrentPowerRequest = IntPtr.Zero
                Dim err = Marshal.GetLastWin32Error()

                If err <> 0 Then
                    Throw New Win32Exception(err)
                End If
            Else
                CurrentPowerRequest = IntPtr.Zero
            End If
        End If
    End Sub

    Enum PowerRequestType
        PowerRequestDisplayRequired = 0
        PowerRequestSystemRequired
        PowerRequestAwayModeRequired
        PowerRequestExecutionRequired
    End Enum

    <DllImport("kernel32.dll", SetLastError:=True)>
    Shared Function PowerCreateRequest(ByRef Context As POWER_REQUEST_CONTEXT) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Shared Function PowerSetRequest(PowerRequestHandle As IntPtr, RequestType As PowerRequestType) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Shared Function PowerClearRequest(PowerRequestHandle As IntPtr, RequestType As PowerRequestType) As Boolean
    End Function

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Structure POWER_REQUEST_CONTEXT
        Public Version As UInt32
        Public Flags As UInt32
        <MarshalAs(UnmanagedType.LPWStr)>
        Public SimpleReasonString As String
    End Structure
End Class