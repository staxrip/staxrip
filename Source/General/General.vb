﻿
Imports System.Runtime.Serialization
Imports System.ComponentModel
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Text
Imports System.Security.Permissions

Imports StaxRip.UI
Imports VB6 = Microsoft.VisualBasic
Imports Microsoft.Win32
Imports ManagedCuda

Public Class Folder
    Shared ReadOnly Property Desktop As String
        Get
            Return Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        End Get
    End Property

    Shared ReadOnly Property WindowsFonts As String
        Get
            Return Environment.GetFolderPath(Environment.SpecialFolder.Fonts)
        End Get
    End Property

    Shared StartupValue As String

    Shared ReadOnly Property Startup As String
        Get
            If StartupValue Is Nothing Then
                StartupValue = Application.StartupPath
            End If

            Return StartupValue
        End Get
    End Property

    Shared ReadOnly Property Temp As String
        Get
            Return Path.GetTempPath
        End Get
    End Property

    Shared ReadOnly Property System As String
        Get
            Return Environment.SystemDirectory
        End Get
    End Property

    Shared ReadOnly Property Programs As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.ProgramFiles)
        End Get
    End Property

    Shared ReadOnly Property Home As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.UserProfile)
        End Get
    End Property

    Shared ReadOnly Property AppDataCommon As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
        End Get
    End Property

    Shared ReadOnly Property AppDataLocal As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        End Get
    End Property

    Shared ReadOnly Property AppDataRoaming As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.ApplicationData)
        End Get
    End Property

    Shared ReadOnly Property Windows As String
        Get
            Return GetFolderPath(Environment.SpecialFolder.Windows)
        End Get
    End Property

    Shared ReadOnly Property Apps As String
        Get
            Return Path.Combine(Folder.Startup, "Apps")
        End Get
    End Property

    Shared ReadOnly Property Fonts As String
        Get
            Return Path.Combine(Folder.Startup, "Fonts")
        End Get
    End Property

    Shared ReadOnly Property UserFonts As String
        Get
            Return Path.Combine(Folder.Settings, "Fonts")
        End Get
    End Property

    Shared ReadOnly Property Icons As String
        Get
            Return Path.Combine(Folder.Startup, "Icons")
        End Get
    End Property

    Shared ReadOnly Property Plugins As String
        Get
            If FrameServerHelp.IsPortable Then
                If p.Script.IsAviSynth Then
                    Return Path.Combine(Folder.Settings, "Plugins", "AviSynth")
                Else
                    Return Path.Combine(Folder.Settings, "Plugins", "VapourSynth")
                End If
            Else
                If p.Script.IsAviSynth Then
                    Return Registry.LocalMachine.GetString("SOFTWARE\AviSynth", "plugindir+")
                Else
                    Return Path.Combine(Folder.AppDataRoaming, "VapourSynth", "plugins64")
                End If
            End If
        End Get
    End Property

    Shared ReadOnly Property Scripts As String
        Get
            Return Path.Combine(Settings, "Scripts")
        End Get
    End Property

    Private Shared SettingsValue As String

    Shared ReadOnly Property Settings As String
        Get
            If SettingsValue Is Nothing Then
                For Each location In Registry.CurrentUser.GetValueNames("Software\StaxRip\SettingsLocation")
                    If Not Directory.Exists(location) Then
                        Registry.CurrentUser.DeleteValue("Software\StaxRip\SettingsLocation", location)
                    End If
                Next

                SettingsValue = Registry.CurrentUser.GetString("Software\StaxRip\SettingsLocation", Startup)

                If Not Directory.Exists(SettingsValue) Then
                    Dim td As New TaskDialog(Of String) With {
                        .Title = "Settings Directory",
                        .Content = "Select the location of the settings directory:"
                    }

                    td.AddCommand(Path.Combine(Startup, "Settings"), "This is the recommended path as long as you have write permissions. Settings are more bound to their StaxRip version and won't be automatically or accidentally transferred to the next version.", Path.Combine(Startup, "Settings"))
                    td.AddCommand(Path.Combine(AppDataRoaming, "StaxRip"), "A good choice if you don't have write permissions in the StaxRip folder or if you want to store the settings in a more common place along with other app settings.", Path.Combine(AppDataRoaming, "StaxRip"))
                    td.AddCommand(Path.Combine(AppDataCommon, "StaxRip"), "This is another option for common settings and also the fallback directory, in case you don't select another directory.", Path.Combine(AppDataCommon, "StaxRip"))
                    td.AddCommand("Browse for custom directory", "You prefer another directory? Feel free to select a directory of your choice. Make sure necessary write permissions are granted.", "Custom")

                    Dim dir = td.Show
                    td.Dispose()

                    If dir = "Custom" Then
                        Using dialog As New FolderBrowserDialog
                            dialog.SelectedPath = Startup
                            dir = If(dialog.ShowDialog = DialogResult.OK, dialog.SelectedPath, Path.Combine(AppDataCommon, "StaxRip"))
                        End Using
                    ElseIf dir = "" Then
                        Process.GetCurrentProcess().Kill()
                    End If

                    If Not dir.DirExists Then
                        Try
                            Directory.CreateDirectory(dir)
                        Catch
                            dir = Path.Combine(AppDataCommon, "StaxRip")

                            If Not dir.DirExists Then
                                Directory.CreateDirectory(dir)
                            End If
                        End Try
                    End If

                    For Each categoryName As String In [Enum].GetNames(GetType(FontCategory)).Skip(1)
                        Directory.CreateDirectory(Path.Combine(dir, "Fonts", categoryName))
                    Next

                    Dim scriptDir = Path.Combine(dir, "Scripts")
                    If Not scriptDir.DirExists Then
                        Directory.CreateDirectory(scriptDir)
                        Const code = "[MainModule]::MsgInfo('Hello World')"
                        code.WriteFileUTF8BOM(Path.Combine(scriptDir, "Hello World.ps1"))
                        Directory.CreateDirectory(Path.Combine(scriptDir, "Auto Load"))
                    End If

                    FolderHelp.Create(Path.Combine(dir, "Plugins", "AviSynth"))
                    FolderHelp.Create(Path.Combine(dir, "Plugins", "VapourSynth"))
                    FolderHelp.Create(Path.Combine(dir, "Plugins", "Dual"))

                    Registry.CurrentUser.Write("Software\StaxRip\SettingsLocation", Folder.Startup, dir)
                    SettingsValue = dir
                End If
            End If

            Return SettingsValue
        End Get
    End Property

    Shared ReadOnly Property Template As String
        Get
            Dim ret = Path.Combine(Settings, "Templates")
            Dim fresh As Boolean

            If Not Directory.Exists(ret) Then
                Directory.CreateDirectory(ret)
                fresh = True
            End If

            Dim version = 44

            If fresh OrElse Not s.Storage.GetInt("template update") = version Then
                s.Storage.SetInt("template update", version)

                Dim files = Directory.GetFiles(ret, "*.srip")

                If files.Length > 0 Then
                    FolderHelp.Delete(Path.Combine(ret, "Backup"))
                    Dim backupDir = Directory.CreateDirectory(Path.Combine(ret, "Backup"))

                    For Each i In files
                        FileHelp.Move(i, Path.Combine(backupDir.FullName, i.FileName))
                    Next
                End If

                Dim auto As New Project
                auto.Init()
                auto.Script = VideoScript.GetDefaults()(1)
                auto.Script.Filters(0) = VideoFilter.GetDefault("Source", "Automatic", ScriptEngine.VapourSynth)
                auto.DemuxAudio = DemuxMode.Preferred
                auto.SubtitleMode = SubtitleMode.Preferred
                SafeSerialization.Serialize(auto, Path.Combine(ret, "Automatic Workflow.srip"))

                Dim manual As New Project
                manual.Init()
                manual.Script = VideoScript.GetDefaults()(1)
                manual.Script.Filters(0) = VideoFilter.GetDefault("Source", "Manual")
                manual.DemuxAudio = DemuxMode.Dialog
                manual.SubtitleMode = SubtitleMode.Dialog
                SafeSerialization.Serialize(manual, Path.Combine(ret, "Manual Workflow.srip"))

                Dim remux As New Project
                remux.Init()
                remux.Script = VideoScript.GetDefaults()(0)
                remux.Script.Filters(0) = New VideoFilter("Source", "DSS2/L-Smash", $"srcFile = ""%source_file%""{BR}ext = LCase(RightStr(srcFile, 3)){BR}(ext == ""mp4"") ? LSMASHVideoSource(srcFile, format = ""YUV420P8"") : DSS2(srcFile)")
                remux.DemuxAudio = DemuxMode.None
                remux.SubtitleMode = SubtitleMode.Disabled
                remux.VideoEncoder = New NullEncoder
                For index = 0 To remux.AudioTracksAvailable - 1
                    remux.AudioTracks.Add(New AudioTrack(index) With {
                                                .AudioProfile = New MuxAudioProfile(),
                                                .EditLabel = New AudioEditButtonLabel(index),
                                                .LanguageLabel = New AudioLanguageLabel(index),
                                                .NameLabel = New AudioNameButtonLabel(index),
                                                .TextEdit = New AudioTextEdit(index)
                                          })
                Next
                SafeSerialization.Serialize(remux, Path.Combine(ret, "Re-mux.srip"))
            End If

            Return ret
        End Get
    End Property

    <DllImport("shfolder.dll", CharSet:=CharSet.Unicode)>
    Shared Function SHGetFolderPath(
        hwndOwner As IntPtr,
        nFolder As Integer,
        hToken As IntPtr,
        dwFlags As Integer,
        lpszPath As StringBuilder) As Integer
    End Function

    Shared Function GetFolderPath(folder As Environment.SpecialFolder) As String
        Dim sb As New StringBuilder(500)
        SHGetFolderPath(IntPtr.Zero, CInt(folder), IntPtr.Zero, 0, sb)
        Dim ret = sb.ToString.FixDir
        Call New FileIOPermission(FileIOPermissionAccess.PathDiscovery, ret).Demand()
        Return ret
    End Function
End Class

Public Class SafeSerialization
    Shared Sub Serialize(o As Object, path As String)
        Dim list As New List(Of Object)

        For Each i In o.GetType.GetFields(
            BindingFlags.Public Or
            BindingFlags.NonPublic Or
            BindingFlags.Instance)

            If Not i.IsNotSerialized Then
                Dim mc As New FieldContainer With {
                    .Name = i.Name
                }

                Dim value = i.GetValue(o)

                If value IsNot Nothing Then
                    mc.Value = If(IsSimpleType(i.FieldType), value, GetObjectData(value))
                    list.Add(mc)
                End If
            End If
        Next

        Dim bf As New BinaryFormatter

        Try
            path = path.TrimQuotes().LongPathPrefix()
            Using fs As New FileStream(path, FileMode.Create)
                bf.Serialize(fs, list)
            End Using
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    Shared Function Deserialize(Of T)(instance As T, path As String) As T
        Dim safeInstance = DirectCast(instance, ISafeSerialization)

        If File.Exists(path) Then
            Dim list As List(Of Object)
            Dim bf As New BinaryFormatter

            Using fs As New FileStream(path, FileMode.Open)
                list = DirectCast(bf.Deserialize(fs), List(Of Object))
            End Using

            For Each i As FieldContainer In list
                For Each iFieldInfo In instance.GetType.GetFields(
                    BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)

                    If Not iFieldInfo.IsNotSerialized Then
                        If i.Name = iFieldInfo.Name Then
                            Try
                                If i.Value.GetType Is GetType(Byte()) Then
                                    iFieldInfo.SetValue(instance, GetObjectInstance(DirectCast(i.Value, Byte())))
                                Else
                                    If iFieldInfo.Name <> "_WasUpdated" Then
                                        iFieldInfo.SetValue(instance, i.Value)
                                    End If
                                End If
                            Catch ex As Exception
                                safeInstance.WasUpdated = True
                            End Try
                        End If
                    End If
                Next
            Next
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
            t Is GetType(DateTime) OrElse
            t Is GetType(Color) OrElse
            t Is GetType(SettingBag(Of String)) OrElse
            t Is GetType(SettingBag(Of Boolean)) OrElse
            t Is GetType(SettingBag(Of Integer)) OrElse
            t Is GetType(SettingBag(Of Double)) OrElse
            t Is GetType(SettingBag(Of Single))
    End Function

    <DebuggerNonUserCode()>
    Private Shared Function GetObjectInstance(ba As Byte()) As Object
        Using ms As New MemoryStream(ba)
            Dim bf As New BinaryFormatter
            'Static binder As New LegacySerializationBinder
            'bf.Binder = binder
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
    Public Class FieldContainer
        Public Value As Object
        Public Name As String
    End Class

    Shared Function Check(iface As ISafeSerialization, obj As Object, key As String, version As Integer) As Boolean
        If obj Is Nothing OrElse Not iface.Versions.ContainsKey(key) OrElse iface.Versions(key) <> version Then
            iface.Versions(key) = version
            iface.WasUpdated = True
            Return True
        End If

        Return False
    End Function

    'legacy
    Private Class LegacySerializationBinder
        Inherits SerializationBinder

        Overrides Function BindToType(assemblyName As String, typeName As String) As Type
            'If typeName.Contains("CLIEncoder") Then
            '    typeName = typeName.Replace("CLIEncoder", "CmdlEncoder")
            'End If

            Return Type.GetType(typeName)
        End Function
    End Class
End Class

Public Interface ISafeSerialization
    Property WasUpdated() As Boolean
    ReadOnly Property Versions() As Dictionary(Of String, Integer)
    Sub Init()
End Interface

Public Class HelpDocument
    Private ReadOnly Path As String
    Private IsClosed As Boolean

    Property Writer As XmlTextWriter

    Sub New(path As String)
        Me.Path = path
    End Sub

    Sub WriteStart(title As String)
        WriteStart(title, True)
    End Sub

    Sub WriteStart(title As String, showTitle As Boolean)
        Dim script = "<script type=""text/javascript""></script>"

        Dim style = "<style type=""text/css"">
@import url(https://fonts.googleapis.com/css?family=Lato:700,900);

body {
    font-family: Tahoma, Geneva, sans-serif;
    font-size: 90%;
    color:#DDDDDD;
    background-color:#323232;
}

h1 {
    font-size: 150%;
    margin-bottom: -4pt;
}

h2 {
    font-size: 120%;
    margin-bottom: -8pt;
}

h3 {
    font-size: 100%;
    margin-bottom: -8pt;
}

a {
    color: #3C8CC8;
}

td {
    width: 50%;
    vertical-align: top;
}

table {
    table-layout: fixed;
}
</style>"

        Writer = New XmlTextWriter(Path, Encoding.UTF8) With {
            .Formatting = Formatting.Indented
        }
        Writer.WriteRaw("<!doctype html>")
        Writer.WriteStartElement("html")
        Writer.WriteStartElement("head")
        Writer.WriteElementString("title", title)
        Writer.WriteRaw(BR + style.ToString + BR)
        Writer.WriteRaw(BR + script.ToString + BR)
        Writer.WriteEndElement() 'head
        Writer.WriteStartElement("body")

        If showTitle Then
            Writer.WriteElementString("h1", title)
        End If
    End Sub

    Sub WriteParagraph(text As String, Optional convert As Boolean = False)
        If convert Then
            text = ConvertChars(text)
        End If

        WriteElement("p", text)
    End Sub

    Sub Write(title As String, text As String, Optional convert As Boolean = False)
        If convert Then
            text = ConvertChars(text)
        End If

        WriteElement("h2", title)
        WriteElement("p", text)
    End Sub

    Sub WriteH2(text As String)
        WriteElement("h2", text)
    End Sub

    Sub WriteH3(text As String)
        WriteElement("h3", text)
    End Sub

    Sub WriteElement(elementName As String, rawText As String)
        If rawText = "" Then
            Exit Sub
        End If

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

        If value.Contains("<") Then
            value = value.Replace("<", "&lt;")
        End If

        If value.Contains(">") Then
            value = value.Replace(">", "&gt;")
        End If

        If value.Contains(BR) Then
            value = value.Replace(BR, "<br>")
        End If

        Return value
    End Function

    'TODO: convert media wiki link to markdown link
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

    'TODO: convert media wiki link to markdown link
    Shared Function MustConvert(value As String) As Boolean
        If value <> "" AndAlso (value.Contains("[") OrElse value.Contains("'''")) Then
            Return True
        End If
        Return False
    End Function

    Sub WriteTips(ParamArray tips As StringPairList())
        If tips.NothingOrEmpty Then
            Exit Sub
        End If

        Dim list As New StringPairList

        For Each i In tips
            list.AddRange(i)
        Next

        list.Sort()

        For Each i In list
            WriteH3(ConvertChars(i.Name))
            WriteParagraph(ConvertChars(i.Value))
        Next
    End Sub

    Sub WriteList(ParamArray values As String())
        Writer.WriteStartElement("ul")

        For Each i In values
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
        WriteTable(title, text, list, False)
    End Sub

    Sub WriteTable(title As String, text As String, list As StringPairList, sort As Boolean)
        If sort Then
            list.Sort()
        End If

        If title IsNot Nothing Then
            Writer.WriteElementString("h2", title)
        End If

        If text Is Nothing Then
            Writer.WriteElementString("p", "")
        Else
            WriteParagraph(text, True)
        End If

        Writer.WriteStartElement("table")
        Writer.WriteAttributeString("border", "1")
        Writer.WriteAttributeString("cellspacing", "0")
        Writer.WriteAttributeString("cellpadding", "3")
        Writer.WriteAttributeString("bordercolordark", "white")
        Writer.WriteAttributeString("bordercolorlight", "black")

        Writer.WriteStartElement("col")
        Writer.WriteAttributeString("style", "width: 40%")
        Writer.WriteEndElement()

        Writer.WriteStartElement("col")
        Writer.WriteAttributeString("style", "width: 60%")
        Writer.WriteEndElement()

        For Each pair In list
            Writer.WriteStartElement("tr")
            Writer.WriteStartElement("td")
            WriteElement("p", ConvertChars(pair.Name))
            Writer.WriteEndElement() 'td
            Writer.WriteStartElement("td")

            If pair.Value Is Nothing Then
                WriteElement("p", "&nbsp;")
            Else
                WriteElement("p", ConvertChars(pair.Value))
            End If

            Writer.WriteEndElement() 'td

            Writer.WriteEndElement() 'tr
        Next

        Writer.WriteEndElement() 'table
    End Sub

    Sub WriteDocument()
        If Not IsClosed Then
            IsClosed = True

            Writer.WriteRaw("<p>&nbsp;</p>" + BR)
            Writer.WriteRaw("<h5 align=""center"">Copyright (C) 2002-" & Date.Now.Year & " StaxRip Authors</h5><br>")
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
Public Class SettingBag(Of T)
    Sub New()
    End Sub

    Sub New(value As T)
        Me.Value = value
    End Sub

    Overridable Property Value As T
End Class

Public Class FieldSettingBag(Of T)
    Inherits SettingBag(Of T)

    Private ReadOnly Obj As Object
    Private ReadOnly Name As String

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

Public Class ReflectionSettingBag(Of T)
    Inherits SettingBag(Of T)

    Private ReadOnly Obj As Object
    Private ReadOnly Name As String

    Sub New(obj As Object, name As String)
        Me.Obj = obj
        Me.Name = name
    End Sub

    Overrides Property Value() As T
        Get
            Dim field = Obj.GetType.GetField(Name, BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)

            If field IsNot Nothing Then
                Return DirectCast(field.GetValue(Obj), T)
            Else
                Return DirectCast(Obj.GetType.GetProperty(Name).GetValue(Obj, Nothing), T)
            End If
        End Get
        Set(value As T)
            Dim f = Obj.GetType.GetField(Name)

            If f IsNot Nothing Then
                f.SetValue(Obj, value)
            Else
                Obj.GetType.GetProperty(Name).SetValue(Obj, value, Nothing)
            End If
        End Set
    End Property
End Class

<Serializable>
Public Class StringPair
    Implements IComparable(Of StringPair)

    Property Name As String
    Property Value As String

    Sub New()
    End Sub

    Sub New(name As String, value As String)
        Me.Name = name
        Me.Value = value
    End Sub

    Function CompareTo(other As StringPair) As Integer Implements IComparable(Of StringPair).CompareTo
        If Name IsNot Nothing Then
            Return Name.CompareTo(other.Name)
        End If
    End Function
End Class

Public Class ErrorAbortException
    Inherits ApplicationException

    Property Title As String

    Sub New(title As String, message As String, Optional proj As Project = Nothing)
        MyBase.New(message)

        If proj Is Nothing Then
            proj = p
        End If

        Me.Title = title
        proj.Log.WriteHeader(title)
        proj.Log.WriteLine(message)
    End Sub
End Class

Public Class AbortException
    Inherits ApplicationException
End Class

Public Class SkipException
    Inherits ApplicationException
End Class

Public Class CliArg
    Sub New(value As String)
        Me.Value = value
    End Sub

    Property Value As String

    Shared Function GetArgs(args As String()) As List(Of CliArg)
        Dim ret As New List(Of CliArg)
        Dim args2 As New List(Of String)(args)
        args2.RemoveAt(0)

        For Each arg As String In args2
            ret.Add(New CliArg(arg))
        Next

        Return ret
    End Function

    Function IsMatch(ParamArray values As String()) As Boolean
        For Each i As String In values
            i = i.ToUpperInvariant
            Dim val As String = Value.ToUpperInvariant

            If "-" + i = val OrElse "/" + i = val OrElse
                val.ToUpperInvariant.StartsWith("-" + i + ":") OrElse
                val.ToUpperInvariant.StartsWith("/" + i + ":") Then

                Return True
            End If
        Next
        Return False
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
Public Class StringPairList
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
Public Class CommandParameters
    Sub New(methodName As String, ParamArray params As Object())
        Me.MethodName = methodName
        Parameters = New List(Of Object)(params)
    End Sub

    Property MethodName As String
    Property Parameters As List(Of Object)
End Class

Public Class Command
    Implements IComparable(Of Command)

    Property Attribute As CommandAttribute
    Property MethodInfo As MethodInfo

    ReadOnly Property Path As String
        Get
            Return MethodInfo?.Name
        End Get
    End Property

    ReadOnly Property Description As String
        Get
            Return Attribute?.Description
        End Get
    End Property

    Function FixParameters(params As List(Of Object)) As List(Of Object)
        Dim copiedParams As New List(Of Object)(params)
        Dim checkedParams As New List(Of Object)(GetDefaultParameters)

        For i = 0 To checkedParams.Count - 1
            If copiedParams.Count > 0 Then
                If Not copiedParams(0) Is Nothing AndAlso
                    checkedParams(i).GetType Is copiedParams(0).GetType Then

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
                Not iParameterInfo.ParameterType Is GetType(String) AndAlso
                Not iParameterInfo.ParameterType.IsArray Then

                Throw New Exception("Methods must have string, value type and array params.")
            End If

            Dim a = DirectCast(iParameterInfo.GetCustomAttributes(GetType(DefaultValueAttribute), False), DefaultValueAttribute())

            If a.Length > 0 Then
                l.Add(a(0).Value)
            Else
                If iParameterInfo.ParameterType Is GetType(String) Then
                    l.Add("")
                ElseIf iParameterInfo.ParameterType.IsValueType Then
                    l.Add(Activator.CreateInstance(iParameterInfo.ParameterType))
                ElseIf iParameterInfo.ParameterType.IsArray Then
                    l.Add(Array.CreateInstance(iParameterInfo.ParameterType.GetElementType(), 0))
                End If
            End If
        Next

        Return l
    End Function

    Overrides Function ToString() As String
        Return MethodInfo.Name
    End Function

    Property [Object] As Object

    Function CompareTo(other As Command) As Integer Implements System.IComparable(Of Command).CompareTo
        Return MethodInfo.Name.CompareTo(other.MethodInfo.Name)
    End Function

    Shared Sub PopulateCommandMenu(
        items As ToolStripItemCollection,
        commands As List(Of Command),
        clickSub As Action(Of Command))

        commands.Sort()

        For Each i In commands
            Dim path = i.Path

            If path.StartsWith("Run") Then path = "Run | " + path
            If path.StartsWith("Save") Then path = "Save | " + path
            If path.StartsWith("Show") Then path = "Show | " + path
            If path.StartsWith("Set") Then path = "Set | " + path
            If path.StartsWith("Start") Then path = "Start | " + path
            If path.StartsWith("Execute") Then path = "Execute | " + path
            If path.StartsWith("Add") Then path = "Add | " + path

            MenuItemEx.Add(items, path, clickSub, i, i.Attribute.Description)
        Next
    End Sub

    Function GetParameterHelp(parameters As List(Of Object)) As String
        If parameters.Count > 0 Then
            Dim paramList As New List(Of String)
            Dim params = MethodInfo.GetParameters

            For iParams = 0 To params.Length - 1
                Dim paramInfo As ParameterInfo = params(iParams)
                Dim attributs = paramInfo.GetCustomAttributes(GetType(DispNameAttribute), False)

                If attributs.Length > 0 Then
                    paramList.Add("Parameter " + DirectCast(attributs(0), DispNameAttribute).DisplayName + ": " + FixParameters(parameters)(iParams).ToString)
                End If
            Next

            Return paramList.Join(BR)
        End If
        Return ""
    End Function
End Class

<AttributeUsage(AttributeTargets.Method)>
Public Class CommandAttribute
    Inherits Attribute

    Sub New(description As String)
        Me.Description = description
    End Sub

    Property Description As String
End Class

Public Class CommandManager
    Property Commands As New Dictionary(Of String, Command)

    Function HasCommand(name As String) As Boolean
        If name = "" Then
            Return False
        End If

        If Commands.ContainsKey(name) Then
            Return True
        End If

        For Each i In Commands.Keys
            If i.IsEqualIgnoreCase(name) Then
                Return True
            End If
        Next
        Return False
    End Function

    Function GetCommand(name As String) As Command
        If HasCommand(name) AndAlso Commands.ContainsKey(name) Then
            Return Commands(name)
        End If

        For Each i In Commands.Keys
            If i.IsEqualIgnoreCase(name) Then
                Return Commands(i)
            End If
        Next
        Return Nothing
    End Function

    Sub AddCommandsFromObject(obj As Object)
        For Each i In obj.GetType.GetMethods(BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public)
            Dim attributes = DirectCast(i.GetCustomAttributes(GetType(CommandAttribute), False), CommandAttribute())

            If attributes.Length > 0 Then
                Dim cmd As New Command With {
                    .MethodInfo = i,
                    .Attribute = attributes(0),
                    .Object = obj
                }
                AddCommand(cmd)
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
        If cp IsNot Nothing Then
            Process(cp.MethodName, cp.Parameters)
        End If
    End Sub

    Sub Process(name As String, params As List(Of Object))
        If HasCommand(name) Then
            ProcessPlusFixParams(GetCommand(name), params)
        End If
    End Sub

    Sub Process(name As String, ParamArray params As Object())
        If HasCommand(name) Then
            ProcessPlusFixParams(GetCommand(name), params.ToList)
        End If
    End Sub

    Sub Process(command As Command, params As List(Of Object))
        Try
            command.MethodInfo.Invoke(command.Object, params.ToArray)
        Catch ex As TargetParameterCountException
            MsgError("Parameter mismatch, for the command :" + command.MethodInfo.Name)
        Catch ex As Exception
            If TypeOf ex.InnerException IsNot AbortException Then
                g.ShowException(ex)
            End If
        End Try
    End Sub

    Sub ProcessPlusFixParams(command As Command, params As List(Of Object))
        Process(command, command.FixParameters(params))
    End Sub

    Function ProcessCommandLineArgument(value As String) As Boolean
        For Each i As Command In Commands.Values
            Dim switch = i.MethodInfo.Name.Replace(" ", "")
            switch = switch.ToUpperInvariant
            Dim preparedValue = value.ToUpperInvariant.Unescape()

            If preparedValue = "-" + switch Then
                Process(i.MethodInfo.Name, New List(Of Object))
                Return True
            Else
                If preparedValue.StartsWith("-" + switch + ":") Then
                    Dim strippedLine = value.Unescape().Right(":")
                    Dim args As New List(Of Object)(g.MainForm.ParseCommandLine(strippedLine, ","c))
                    Dim params = i.MethodInfo.GetParameters

                    For x = 0 To params.Length - 1
                        If args.Count > x Then
                            Dim pt = params(x).ParameterType
                            Dim stringArg = TryCast(args(x), String)

                            If pt.IsArray AndAlso stringArg IsNot Nothing Then
                                Dim splits = g.MainForm.ParseCommandLine(stringArg.TrimQuotes(), ";"c)
                                Dim et = pt.GetElementType()
                                Dim tc = TypeDescriptor.GetConverter(et)

                                If tc IsNot Nothing AndAlso tc.CanConvertFrom(GetType(String)) Then
                                    Dim a = Array.CreateInstance(et, splits.Length)
                                    For y = 0 To splits.Length - 1
                                        a.SetValue(tc.ConvertFromString(splits(y).TrimQuotes().Trim()), y)
                                    Next

                                    args(x) = a
                                Else
                                    Throw New InvalidOperationException($"Cannot convert from String to {et}.")
                                End If
                            ElseIf stringArg IsNot Nothing AndAlso pt Is GetType(String) Then
                                args(x) = TypeDescriptor.GetConverter(GetType(String)).ConvertFromString(stringArg.TrimQuotes())
                            Else
                                args(x) = TypeDescriptor.GetConverter(pt).ConvertFrom(args(x))
                            End If
                        End If
                    Next

                    Process(i.MethodInfo.Name, args)
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    Function GetTips() As StringPairList
        Dim l As New StringPairList

        For Each i As Command In Commands.Values
            If Not i.Attribute.Description Is Nothing Then
                l.Add(New StringPair(i.MethodInfo.Name, i.Attribute.Description))
            End If
        Next

        Return l
    End Function
End Class

Public Module MainModule
    Public Const BR As String = VB6.vbCrLf
    Public Const BR2 As String = VB6.vbCrLf + VB6.vbCrLf
    Public Const BR3 As String = VB6.vbCrLf + VB6.vbCrLf + VB6.vbCrLf
    Public Log As LogBuilder

    Sub MsgInfo(title As Object, Optional content As Object = Nothing)
        Dim title1 = title?.ToString
        Dim content1 = content?.ToString
        Msg(title1, content1, TaskIcon.Info, TaskButton.OK)
    End Sub

    Sub MsgError(title As String, Optional content As String = Nothing, Optional timeout As Integer = 0)
        MsgError(title, content, IntPtr.Zero, timeout)
    End Sub

    Sub MsgError(title As String, content As String, handle As IntPtr, Optional timeout As Integer = 0)
        If timeout < 0 Then Exit Sub
        If title = "" Then title = content
        If title = "" Then Exit Sub

        Using td As New TaskDialog(Of String)
            td.Title = title
            td.Content = content
            td.Owner = handle
            td.Icon = TaskIcon.Error
            td.ShowCopyButton = True
            td.Timeout = timeout
            td.AddButton("OK")
            td.Show()
        End Using
    End Sub

    Private ShownMessages As String

    Sub MsgWarn(text As String, Optional content As String = Nothing, Optional onlyOnce As Boolean = False)
        If onlyOnce AndAlso ShownMessages?.Contains(text + content) Then
            Exit Sub
        End If

        Msg(text, content, TaskIcon.Warning, TaskButton.OK)

        If onlyOnce Then
            ShownMessages += text + content
        End If
    End Sub

    Function MsgOK(title As String) As Boolean
        Return Msg(title, Nothing, TaskIcon.Question, TaskButton.OkCancel) = DialogResult.OK
    End Function

    Function MsgQuestion(
        title As String,
        Optional buttons As TaskButton = TaskButton.OkCancel) As DialogResult

        Return Msg(title, Nothing, TaskIcon.Question, buttons)
    End Function

    Function MsgQuestion(title As String,
                         content As String,
                         Optional buttons As TaskButton = TaskButton.OkCancel) As DialogResult
        Return Msg(title, content, TaskIcon.Question, buttons)
    End Function

    Function Msg(title As String, content As String, icon As TaskIcon, buttons As TaskButton) As DialogResult
        Using td As New TaskDialog(Of DialogResult)
            td.Icon = icon
            td.Title = title
            td.Content = content
            td.Buttons = buttons
            Return td.Show()
        End Using
    End Function
End Module

Public Class Reflector
    Public Type As Type
    Private ReadOnly BasicFlags As BindingFlags = BindingFlags.Static Or BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic

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

            If Value IsNot Nothing Then
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

Public Class Shutdown
    Shared Sub Commit(mode As ShutdownMode)
        Dim psi = New ProcessStartInfo("shutdown.exe") With {
            .CreateNoWindow = True,
            .UseShellExecute = False
        }

        Select Case mode
            Case ShutdownMode.Standby
                Application.SetSuspendState(PowerState.Suspend, s.ShutdownForce, False)
            Case ShutdownMode.Hibernate
                Application.SetSuspendState(PowerState.Hibernate, s.ShutdownForce, False)
            Case ShutdownMode.Hybrid
                psi.Arguments = If(s.ShutdownForce, "/f ", "")
                psi.Arguments += "/hybrid /t " & s.ShutdownTimeout
                Process.Start(psi)?.Dispose()
            Case ShutdownMode.Shutdown
                psi.Arguments = If(s.ShutdownForce, "/f ", "")
                psi.Arguments += "/s /t " & s.ShutdownTimeout
                Process.Start(psi)?.Dispose()
        End Select
    End Sub
End Class

Public Enum ShutdownMode
    [Nothing]
    Close
    Standby
    Hibernate
    Hybrid
    Shutdown
End Enum

Public Class PowerRequest

    Shared Sub SuppressStandby()
        Dim previousState = SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS Or EXECUTION_STATE.ES_SYSTEM_REQUIRED)

        If previousState = EXECUTION_STATE.ES_ERROR Then
            Throw New Win32Exception("expected last EXECUTION_STATE but got 0 indicating an error" + previousState.ToString)
        End If
    End Sub

    Shared Sub EnableStandby()
        Dim previousState = SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS)

        If previousState = EXECUTION_STATE.ES_ERROR Then
            Throw New Win32Exception("expected last EXECUTION_STATE but got 0 indicating an error" + previousState.ToString)
        End If
    End Sub

    <Flags>
    Enum EXECUTION_STATE : uint
        ES_ERROR = 0 ' indicating an error
        ES_SYSTEM_REQUIRED = &H1
        ES_DISPLAY_REQUIRED = &H2
        ES_AWAYMODE_REQUIRED = &H40
        ES_CONTINUOUS = &H80000000
        ' Legacy flag, should Not be used.
        ' ES_USER_PRESENT = &H00000004
    End Enum

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Shared Function SetThreadExecutionState(esFlags As EXECUTION_STATE) As EXECUTION_STATE
    End Function

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Structure POWER_REQUEST_CONTEXT
        Public Version As UInt32
        Public Flags As UInt32
        <MarshalAs(UnmanagedType.LPWStr)>
        Public SimpleReasonString As String
    End Structure
End Class

Public Class CRC32
    Shared ReadOnly Table As UInteger() = New UInteger(255) {}

    Shared Sub New()
        Dim poly = &HEDB88320UI
        Dim temp As UInteger

        For i = 0 To Table.Length - 1
            temp = CUInt(i)

            For j = 8 To 1 Step -1
                If (temp And 1) = 1 Then
                    temp = (temp >> 1) Xor poly
                Else
                    temp >>= 1
                End If
            Next

            Table(i) = temp
        Next
    End Sub

    Shared Function GetChecksum(bytes As Byte()) As UInteger
        Dim crc = &HFFFFFFFFUI

        For i = 0 To bytes.Length - 1
            Dim index = CByte((crc And &HFF) Xor bytes(i))
            crc = (crc >> 8) Xor Table(index)
        Next

        Return Not crc
    End Function

    Shared Function GetChecksum(str As String) As UInteger
        If str <> "" Then
            Return GetChecksum(Encoding.Unicode.GetBytes(str))
        End If
    End Function
End Class

Public Class Cuda
    Private Shared _result As Integer = -1

    Public Shared ReadOnly Property Count As Integer
        Get
            Try
                If _result < 0 Then
                    _result = CudaContext.GetDeviceCount()
                End If
            Catch ex As Exception
                _result = 0
            End Try
            Return _result
        End Get
    End Property

    Public Shared ReadOnly Property IsSupported As Boolean
        Get
            Return Count > 0
        End Get
    End Property
End Class

Public Class Vulkan
    Private Shared _pApiVersion As UInteger
    Private Shared _result As VkResult = Nothing

    <DllImport("vulkan-1.dll")>
    Public Shared Function vkEnumerateInstanceVersion(ByRef pApiVersion As UInteger) As VkResult
    End Function

    Public Shared ReadOnly Property IsSupported As Boolean
        Get
            Try
                If _result = Nothing Then
                    _result = vkEnumerateInstanceVersion(_pApiVersion)
                End If
            Catch ex As Exception
                _result = VkResult.ErrorInitializationFailed
            End Try
            Return _result = VkResult.Success
        End Get
    End Property

    Public Shared ReadOnly Property Version As String
        Get
            Return If(IsSupported, $"{_pApiVersion >> 22}.{(_pApiVersion >> 12) And &H3FF}.{_pApiVersion And &HFFF}", "")
        End Get
    End Property
End Class
