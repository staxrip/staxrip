
Imports System
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions

Imports StaxRip

Imports VB = Microsoft.VisualBasic

<Serializable()>
Public Class VideoScript
    Inherits Profile

    <NonSerialized()> Public [Error] As String
    <NonSerialized()> Public Info As ServerInfo
    <NonSerialized()> Public OriginalInfo As ServerInfo
    <NonSerialized()> Public LastCode As String
    <NonSerialized()> Public LastPath As String


    Property Filters As New List(Of VideoFilter)

    Overridable Property Engine As ScriptEngine = ScriptEngine.AviSynth
    Overridable Property Path As String = ""

    Shared Event Changed(script As VideoScript)

    Sub RaiseChanged()
        RaiseEvent Changed(Me)
    End Sub

    ReadOnly Property IsAviSynth As Boolean
        Get
            Return Engine = ScriptEngine.AviSynth
        End Get
    End Property

    ReadOnly Property IsVapourSynth As Boolean
        Get
            Return Engine = ScriptEngine.VapourSynth
        End Get
    End Property

    Overridable ReadOnly Property FileType As String
        Get
            Return If(Engine = ScriptEngine.VapourSynth, "vpy", "avs")
        End Get
    End Property

    Overridable Function GetScript() As String
        Return GetScript(Nothing)
    End Function

    Overridable Function GetScript(skipCategory As String) As String
        Dim sb As New StringBuilder()

        If p.CodeAtTop <> "" Then
            sb.AppendLine(p.CodeAtTop)
        End If

        For Each filter As VideoFilter In Filters
            If filter.Active Then
                If skipCategory Is Nothing OrElse filter.Category <> skipCategory Then
                    sb.AppendLine(filter.Script)
                End If
            End If
        Next

        If p.CodeAtBottom <> "" Then
            sb.AppendLine(p.CodeAtBottom)
        End If

        Return sb.ToString
    End Function

    Function GetFullScript() As String
        Return Macro.Expand(ModifyScript(GetScript, Engine)).Trim
    End Function

    Sub RemoveFilter(category As String, Optional name As String = Nothing)
        For Each i In Filters.ToArray
            If i.Category = category AndAlso (name = "" OrElse i.Path = name) Then
                Filters.Remove(i)
                RaiseChanged()
            End If
        Next
    End Sub

    Sub RemoveFilterAt(index As Integer)
        If Filters.Count > 0 AndAlso index < Filters.Count Then
            Filters.RemoveAt(index)
            RaiseChanged()
        End If
    End Sub

    Sub RemoveFilter(filter As VideoFilter)
        If Filters.Contains(filter) Then
            Filters.Remove(filter)
            RaiseChanged()
        End If
    End Sub

    Sub InsertFilter(index As Integer, filter As VideoFilter)
        Filters.Insert(index, filter)
        RaiseChanged()
    End Sub

    Sub AddFilter(filter As VideoFilter)
        Filters.Add(filter)
        RaiseChanged()
    End Sub

    Sub SetFilter(index As Integer, filter As VideoFilter)
        Filters(index) = filter
        RaiseChanged()
    End Sub

    Sub SetFilter(category As String, name As String, script As String)
        For Each i In Filters
            If i.Category = category Then
                i.Path = name
                i.Script = script
                i.Active = True
                RaiseChanged()
                Exit Sub
            End If
        Next

        If Filters.Count > 0 Then
            Filters.Insert(1, New VideoFilter(category, name, script))
            RaiseChanged()
        End If
    End Sub

    Sub InsertAfter(category As String, af As VideoFilter)
        Dim f = GetFilter(category)
        Filters.Insert(Filters.IndexOf(f) + 1, af)
        RaiseChanged()
    End Sub

    Function Contains(category As String, search As String) As Boolean
        If category = "" OrElse search = "" Then Return False
        Dim filter = GetFilter(category)

        If filter?.Script?.ToLowerInvariant.Contains(search.ToLowerInvariant) AndAlso filter?.Active Then
            Return True
        End If
    End Function

    Sub ActivateFilter(category As String)
        Dim filter = GetFilter(category)

        If filter IsNot Nothing Then
            filter.Active = True
        End If
    End Sub

    Function IsFilterActive(category As String) As Boolean
        Dim filter = GetFilter(category)
        Return filter IsNot Nothing AndAlso filter.Active
    End Function

    Function IsFilterActive(category As String, name As String) As Boolean
        Dim filter = GetFilter(category)
        Return filter IsNot Nothing AndAlso filter.Active AndAlso filter.Name = name
    End Function

    Function GetFiltersCopy() As List(Of VideoFilter)
        Dim ret = New List(Of VideoFilter)

        For Each i In Filters
            ret.Add(i.GetCopy)
        Next

        Return ret
    End Function

    Function GetNewScript() As VideoScript
        Dim returnValue As New VideoScript
        returnValue.Engine = Engine
        returnValue.Filters = GetFiltersCopy()
        Return returnValue
    End Function

    Function GetFilter(category As String) As VideoFilter
        For Each filter In Filters
            If filter.Category = category Then
                Return filter
            End If
        Next
    End Function

    Sub Synchronize(
        Optional convertToRGB As Boolean = False,
        Optional comparePath As Boolean = True,
        Optional flipVertical As Boolean = False)

        If Path = "" Then Exit Sub

        Dim srcFilter = GetFilter("Source")
        If srcFilter IsNot Nothing AndAlso Not srcFilter.Script.Contains("(") Then Exit Sub

        Dim code = GetScript()

        If convertToRGB Then
            If Engine = ScriptEngine.AviSynth Then
                If p.SourceHeight > 576 Then
                    code += BR + "ConvertBits(8)" + BR + "ConvertToRGB32(matrix=""Rec709"")"
                Else
                    code += BR + "ConvertBits(8)" + BR + "ConvertToRGB32(matrix=""Rec601"")"
                End If

                If flipVertical Then
                    code += BR + "FlipVertical()"
                End If
            Else
                code = code.Trim

                If Not code.Contains(".set_output(") Then
                    code += BR + "clip.set_output()"
                End If

                Dim match = Regex.Match(code, "(\w+)\.set_output\(\)")
                Dim clipname = match.Groups(1).Value

                Dim vsCode = ""

                vsCode += "
m_rgb = 0
m_709 = 1
m_unspec = 2
m_470bg = 5
m_2020ncl = 9

t_709 = 1
t_470bg = 5
t_st2084 = 16

p_709 = 1
p_470bg = 5
p_2020 = 9

props = clip.get_frame(0).props

if '_Matrix' in props and props['_Matrix'] != m_unspec and props['_Matrix'] < 15:
    matrix = props['_Matrix']
else:
    if clipname.format.id == vs.RGB24:
        matrix = m_rgb
    else:
        if %source_height% > 576:
            matrix = m_709
        else:
            matrix = m_470bg

if '_Transfer' in props and props['_Transfer'] > 0 and props['_Transfer'] < 19:
    transfer = props['_Transfer']
else:
    if matrix == m_470bg:
        transfer = t_470bg
    elif matrix == m_2020ncl:
        transfer = t_st2084
    else:
        transfer = t_709

if '_Primaries' in props and props['_Primaries'] > 0 and props['_Primaries'] < 23:
    primaries = props['_Primaries']
else:
    if matrix == m_470bg:
        primaries = p_470bg
    elif matrix == m_2020ncl:
        primaries = p_2020
    else:
        primaries = p_709

clipname = clipname.resize.Bicubic(matrix_in = matrix, transfer_in = transfer, primaries_in = primaries, format = vs.RGB24)"
                If Not vsCode.ContainsEx(Package.LibP2P.Filename) Then vsCode += BR + "core.std.LoadPlugin(r""" + Package.LibP2P.Path + """, altsearchpath=True)"
                vsCode += "
clipname = clipname.libp2p.Pack()
clipname.set_output()" + BR
                vsCode = vsCode.Replace("clipname", clipname)
                code = code.Replace(match.Value, vsCode).Trim
            End If
        End If

        code = Macro.Expand(code)

        If Me.Error <> "" OrElse code <> LastCode OrElse (comparePath AndAlso Path <> LastPath) Then
            If Path.Dir.DirExists Then
                If Engine = ScriptEngine.VapourSynth Then
                    ModifyScript(code, Engine).WriteFileUTF8(Path)
                Else
                    ModifyScript(code, Engine).WriteFile(Path, TextEncoding.EncodingOfProcess)
                End If

                If Not Package.AviSynth.VerifyOK OrElse Not Package.VapourSynth.VerifyOK OrElse
                    Not Package.vspipe.VerifyOK Then

                    Throw New AbortException
                End If

                g.MainForm.Indexing()

                Using server = FrameServerFactory.Create(Path)
                    Info = server.Info

                    If Not convertToRGB Then
                        OriginalInfo = Info
                    End If

                    Me.Error = server.Error
                End Using

                LastCode = code
                LastPath = Path
            End If
        End If
    End Sub

    Shared Function ModifyScript(script As String, engine As ScriptEngine) As String
        Return If(p.SourceFile.Ext.EqualsAny("avs", "vpy"),
            script,
            If(engine = ScriptEngine.VapourSynth, ModifyVSScript(script), ModifyAVSScript(script)))
    End Function

    Shared Function GetVsPortableAutoLoadPluginCode() As String
        Dim ret = New StringBuilder

        If FrameServerHelp.IsPortable Then
            Dim folders = {IO.Path.Combine(Folder.Settings, "Plugins", "VapourSynth"),
                           IO.Path.Combine(Folder.Settings, "Plugins", "Dual")}

            For Each folder In folders
                If folder.DirExists Then
                    For Each file In Directory.GetFiles(folder, "*.dll")
                        ret.AppendLine("core.std.LoadPlugin(r""" + file + """, altsearchpath=True)")
                    Next
                End If
            Next
        End If

        Return ret.ToString()
    End Function

    Shared Function ModifyVSScript(script As String) As String
        Dim clip = New StringBuilder
        Dim code = ""

        ModifyVSScript(script, code)

        If Not script.Contains("import vapoursynth") Then
            clip.AppendLine("import os, sys")
            clip.AppendLine("import vapoursynth as vs")
            clip.AppendLine("core = vs.core")
            clip.AppendLine(GetVsPortableAutoLoadPluginCode())
            clip.AppendLine("sys.path.append(r""" + IO.Path.Combine(Folder.Startup, "Apps", "Plugins", "VS", "Scripts") + """)")
        End If

        If Not script.Contains("import importlib.machinery") AndAlso code.Contains("SourceFileLoader") Then
            clip.AppendLine("import importlib.machinery")
        End If

        clip.AppendLine(code)
        clip.AppendLine(script)

        If Not clip.ToString().Contains(".set_output(") Then
            clip.Append("clip.set_output()")
        End If

        Return clip.ToString()
    End Function

    Shared Function ModifyVSScript(ByRef script As String, ByRef code As String) As String
        Dim functions = FindFunctionsVS(script)
        For Each plugin In Package.Items.Values.OfType(Of PluginPackage)()
            Dim fp = plugin.Path
            If fp <> "" Then
                If plugin.VsFilterNames IsNot Nothing Then
                    For Each filterName In plugin.VsFilterNames
                        If functions.Contains(filterName) Then
                            WriteVSCode(script, code, filterName, plugin)
                        End If
                    Next
                End If

                If script.ToString().Contains("import " & plugin.Name) OrElse code.ToString().Contains("import " & plugin.Name) Then
                    WriteVSCode(script, code, Nothing, plugin)
                End If

                If plugin.AvsFilterNames IsNot Nothing Then
                    For Each filterName In plugin.AvsFilterNames
                        If script.Contains(".avs." & filterName) Then
                            WriteVSCode(script, code, filterName, plugin)
                        End If
                    Next
                End If
            End If
        Next
    End Function

    Shared Sub WriteVSCode(ByRef script As String, ByRef code As String, ByRef filterName As String, plugin As PluginPackage)
        If plugin.Filename.Ext = "py" Then
            Dim line = plugin.Name + " = importlib.machinery.SourceFileLoader('" + plugin.Name + "', r""" + plugin.Path + """).load_module()"

            If code.Contains(line) Then
                If Not script.ContainsEx(filterName) Then
                    code = code.Replace(line + BR, "")
                    code = line + BR + code
                End If
            Else
                If Not script.Contains(line) Then
                    code = line + BR + code
                    Dim scriptCode = plugin.Path.ReadAllText
                    ModifyVSScript(scriptCode, code)
                End If
            End If
        Else
            If s.LoadVapourSynthPlugins AndAlso Not IsVsPluginInAutoLoadFolder(plugin.Filename) AndAlso Not script.Contains(plugin.Filename) AndAlso Not code.Contains(plugin.Filename) Then
                Dim line = If(script.Contains(".avs." + filterName) OrElse code.Contains(".avs." + filterName),
                    "core.avs.LoadPlugin(r""" + plugin.Path + """)",
                    "core.std.LoadPlugin(r""" + plugin.Path + """, altsearchpath=True)")

                If Not plugin.RequirementsFulfilled Then
                    code += "# !! The following line is commented out by StaxRip, because the system does not fulfill all requirements to execute it !!" & BR & "# "
                End If

                code += line + BR
            End If
        End If
    End Sub

    Shared Function IsVsPluginInAutoLoadFolder(filename As String) As Boolean
        If FrameServerHelp.IsPortable Then
            Dim folders = {
                IO.Path.Combine(Package.VapourSynth.Directory, "vapoursynth64", "plugins"),
                IO.Path.Combine(Package.VapourSynth.Directory, "vapoursynth64", "coreplugins"),
                IO.Path.Combine(Folder.Settings, "Plugins", "VapourSynth"),
                IO.Path.Combine(Folder.Settings, "Plugins", "Dual")}

            For Each folder In folders
                If File.Exists(IO.Path.Combine(folder, filename)) Then
                    Return True
                End If
            Next
        Else
            Return File.Exists(IO.Path.Combine(Folder.Plugins, filename))
        End If
    End Function

    Shared Function IsAvsPluginInAutoLoadFolder(filename As String) As Boolean
        If FrameServerHelp.IsPortable Then
            Dim folders = {
                IO.Path.Combine(Package.AviSynth.Directory, "plugins"),
                IO.Path.Combine(Folder.Settings, "Plugins", "AviSynth"),
                IO.Path.Combine(Folder.Settings, "Plugins", "Dual")}

            For Each folder In folders
                If File.Exists(IO.Path.Combine(folder, filename)) Then
                    Return True
                End If
            Next
        Else
            Return (Folder.Plugins + filename).FileExists
        End If
    End Function

    Shared Function GetAvsLoadCode(script As String, scriptAlready As String) As String
        Dim scriptLower = script.ToLowerInvariant
        Dim scriptAlreadyLower = scriptAlready.ToLowerInvariant
        Dim loadCode = ""
        Dim plugins = Package.Items.Values.OfType(Of PluginPackage)()
        Dim functions = FindFunctionsAVS(scriptLower)

        For Each plugin In plugins
            Dim fp = plugin.Path

            If fp <> "" Then
                If plugin.AvsFilterNames IsNot Nothing Then
                    For Each filterName In plugin.AvsFilterNames
                        If s.LoadAviSynthPlugins AndAlso functions.Contains(filterName.ToLowerInvariant) Then
                            If plugin.Filename.Ext = "dll" Then
                                Dim load = "LoadPlugin(""" + fp + """)" + BR

                                If Not scriptLower.Contains(load.ToLowerInvariant) AndAlso
                                    Not loadCode.ToLowerInvariant.Contains(load.ToLowerInvariant) AndAlso
                                    Not scriptAlreadyLower.Contains(load.ToLowerInvariant) AndAlso
                                    Not IsAvsPluginInAutoLoadFolder(plugin.Filename) Then

                                    loadCode += load
                                End If

                                If Not plugin.Dependencies.NothingOrEmpty Then
                                    For Each iDependency In plugin.Dependencies
                                        Dim fp2 = Package.Items.Values.OfType(Of PluginPackage).Where(Function(pack) pack.Filename.ToLowerEx() = iDependency.ToLowerEx()).FirstOrDefault()?.Path

                                        If fp2.Ext = "dll" Then
                                            load = "LoadPlugin(""" + fp2 + """)" + BR

                                            If Not scriptLower.Contains(load.ToLowerInvariant) AndAlso
                                                Not loadCode.ToLowerInvariant.Contains(load.ToLowerInvariant) AndAlso
                                                Not scriptAlreadyLower.Contains(load.ToLowerInvariant) Then

                                                loadCode += load
                                            End If
                                        ElseIf fp2.Ext = "avsi" Then
                                            Dim avsiImport = "Import(""" + fp2 + """)" + BR

                                            If Not scriptLower.Contains(avsiImport.ToLowerInvariant) AndAlso
                                                Not loadCode.ToLowerInvariant.Contains(avsiImport.ToLowerInvariant) AndAlso
                                                Not scriptAlreadyLower.Contains(avsiImport.ToLowerInvariant) Then

                                                loadCode += avsiImport
                                            End If
                                        End If
                                    Next
                                End If
                            ElseIf plugin.Filename.Ext = "avsi" Then
                                Dim avsiImport = "Import(""" + fp + """)" + BR

                                If Not scriptLower.Contains(avsiImport.ToLowerInvariant) AndAlso
                                    Not loadCode.ToLowerInvariant.Contains(avsiImport.ToLowerInvariant) AndAlso
                                    Not scriptAlreadyLower.Contains(avsiImport.ToLowerInvariant) AndAlso
                                    Not IsAvsPluginInAutoLoadFolder(plugin.Filename) Then

                                    loadCode += avsiImport
                                End If

                                If Not plugin.Dependencies.NothingOrEmpty Then
                                    For Each iDependency In plugin.Dependencies
                                        Dim fp2 = Package.Items.Values.OfType(Of PluginPackage).Where(Function(pack) pack.Filename.ToLowerEx() = iDependency.ToLowerEx()).FirstOrDefault()?.Path

                                        If fp2.Ext = "dll" Then
                                            Dim load = "LoadPlugin(""" + fp2 + """)" + BR

                                            If Not scriptLower.Contains(load.ToLowerInvariant) AndAlso
                                                Not loadCode.ToLowerInvariant.Contains(load.ToLowerInvariant) AndAlso
                                                Not scriptAlreadyLower.Contains(load.ToLowerInvariant) Then

                                                loadCode += load
                                            End If
                                        ElseIf fp2.Ext = "avsi" Then
                                            avsiImport = "Import(""" + fp2 + """)" + BR

                                            If Not scriptLower.Contains(avsiImport.ToLowerInvariant) AndAlso
                                                Not loadCode.ToLowerInvariant.Contains(avsiImport.ToLowerInvariant) AndAlso
                                                Not scriptAlreadyLower.Contains(avsiImport.ToLowerInvariant) Then

                                                loadCode += avsiImport
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    Next
                End If
            End If
        Next

        Return loadCode
    End Function

    Shared ReadOnly AllowedFunctionChars As HashSet(Of Char) = Enumerable.Range(48, 10).Concat(Enumerable.Range(65, 26).Concat(Enumerable.Range(97, 26))).Select(Function(i) Convert.ToChar(i)).Concat({"_"c}).ToHashSet()

    ' this functions finds too many function because all text is treated as script
    ' so functions in comments or strings that match the syntax will be interpreted as functions
    Shared Function FindFunctionsVS(script As String) As HashSet(Of String)
        Dim functionTokens = New HashSet(Of String)

        Dim scriptIndex = script.LastIndexOf("."c, script.Length - 1)

        While scriptIndex > 0
            Dim wordStart = scriptIndex - 1
            Dim wordEnd = scriptIndex + 1

            If script.Chars(wordEnd).IsDigit() Or Not AllowedFunctionChars.Contains(script.Chars(wordEnd)) Then
                scriptIndex = script.LastIndexOf("."c, scriptIndex - 1)
                Continue While
            End If

            While wordEnd < script.Length AndAlso AllowedFunctionChars.Contains(script.Chars(wordEnd))
                wordEnd += 1
            End While
            While wordStart >= 0 AndAlso AllowedFunctionChars.Contains(script.Chars(wordStart))
                wordStart -= 1
            End While

            scriptIndex = script.LastIndexOf("."c, scriptIndex - 1)

            If IsFunctionCommented(script, wordStart) Then
                Continue While
            End If

            Dim func = script.Substring(wordStart + 1, wordEnd - 1 - wordStart).Trim()
            functionTokens.Add(func)
        End While

        Return functionTokens
    End Function

    Shared Function IsFunctionCommented(script As String, index As Integer) As Boolean
        While index >= 0 AndAlso Not Environment.NewLine.Contains(script.Chars(index))
            If script.Chars(index) = "#"c Then
                Return True
            End If
            index -= 1
        End While
        Return False
    End Function

    ' this functions finds too many function because all text is treated as script
    ' so functions in comments or strings that match the syntax will be interpreted as functions
    Shared Function FindFunctionsAVS(script As String) As HashSet(Of String)
        Dim functionTokens = New HashSet(Of String)
        Dim scriptIndex = script.LastIndexOf("("c, script.Length - 1)

        While scriptIndex > 0
            scriptIndex -= 1
            Dim wordEnd = scriptIndex
            Dim wordStart = scriptIndex

            While wordStart >= 0 AndAlso script.Chars(wordStart) = " "c
                wordStart -= 1
            End While

            While wordStart >= 0 AndAlso AllowedFunctionChars.Contains(script.Chars(wordStart))
                wordStart -= 1
            End While

            scriptIndex = script.LastIndexOf("("c, scriptIndex - 1)

            If IsFunctionCommented(script, wordStart) Then
                Continue While
            End If

            Dim func = script.Substring(wordStart + 1, wordEnd - wordStart).Trim()
            functionTokens.Add(func)
        End While

        Return functionTokens
    End Function

    Shared Function GetAVSLoadCodeFromImports(code As String) As String
        code = code.ToLowerInvariant
        Dim ret = ""

        For Each line In code.SplitLinesNoEmpty
            If line.Contains("import") Then
                Dim match = Regex.Match(line, "\bimport\s*\(\s*""\s*(.+\.avsi*)\s*""\s*\)", RegexOptions.IgnoreCase)

                If match.Success AndAlso File.Exists(match.Groups(1).Value) Then
                    ret += GetAvsLoadCode(match.Groups(1).Value.ReadAllText, code)
                End If
            End If
        Next

        Return ret.SplitLinesNoEmpty.Distinct.Join(BR)
    End Function

    Shared Function ModifyAVSScript(script As String) As String
        Dim ret = New StringBuilder()
        Dim sb = New StringBuilder()
        Dim newScript As String
        newScript = GetAvsLoadCode(script, "") & script
        newScript = GetAVSLoadCodeFromImports(newScript) & BR & newScript

        Using sr As New StringReader(newScript)
            Dim line As String
            Do
                line = sr.ReadLine()
                If line IsNot Nothing Then
                    If line.StartsWithEx("AddAutoloadDir(") Then
                        sb.Insert(0, line)
                    Else
                        sb.AppendLine(line)
                    End If
                Else
                    Exit Do
                End If
            Loop
        End Using

        If FrameServerHelp.IsPortable Then
            ret.AppendLine($"AddAutoloadDir(""{IO.Path.Combine(Package.AviSynth.Directory, "plugins")}{IO.Path.DirectorySeparatorChar}"")")

            Dim folders = {IO.Path.Combine(Folder.Settings, "Plugins", "AviSynth"),
                           IO.Path.Combine(Folder.Settings, "Plugins", "Dual")}

            For Each folder In folders
                If FolderHelp.HasFiles(folder) Then
                    ret.AppendLine($"AddAutoloadDir(""{folder}"")")
                End If
            Next
        End If

        ret.Append(sb)

        Return ret.ToString()
    End Function

    Function GetFramerate() As Double
        Dim info = GetInfo()
        Dim ret = info.FrameRateNum / info.FrameRateDen
        Return If(Calc.IsValidFrameRate(ret), ret, 25)
    End Function

    Function GetCachedFrameRate() As Double
        Dim ret = Info.FrameRateNum / Info.FrameRateDen
        Return If(Calc.IsValidFrameRate(ret), ret, 25)
    End Function

    Function GetInfo() As ServerInfo
        Synchronize(False, False)
        Return Info
    End Function

    Function GetError() As String
        Synchronize(False, False)
        Return Me.Error
    End Function

    Function GetSeconds() As Integer
        Dim info = GetInfo()
        Return CInt(info.FrameCount / info.FrameRate)
    End Function

    Function GetFrameCount() As Integer
        Return GetInfo().FrameCount
    End Function

    Shared Function GetDefaults() As List(Of TargetVideoScript)
        Dim ret As New List(Of TargetVideoScript)

        Dim script = New TargetVideoScript("AviSynth")
        script.Engine = ScriptEngine.AviSynth
        script.Filters.Add(New VideoFilter("Source", "Automatic", "# can be configured at: Tools > Settings > Source Filters"))
        script.Filters.Add(New VideoFilter("Crop", "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)", False))
        script.Filters.Add(New VideoFilter("Field", "QTGMC Medium", "QTGMC(preset=""Medium"")", False))
        script.Filters.Add(New VideoFilter("Noise", "DFTTest", "DFTTest(sigma=6, tbsize=1)", False))
        script.Filters.Add(New VideoFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%)", False))
        ret.Add(script)

        script = New TargetVideoScript("VapourSynth")
        script.Engine = ScriptEngine.VapourSynth
        script.Filters.Add(New VideoFilter("Source", "Automatic", "# can be configured at: Tools > Settings > Source Filters"))
        script.Filters.Add(New VideoFilter("Crop", "Crop", "clip = core.std.Crop(clip, %crop_left%, %crop_right%, %crop_top%, %crop_bottom%)", False))
        script.Filters.Add(New VideoFilter("Field", "QTGMC Medium", $"clip = core.std.SetFieldBased(clip, 2) # 1=BFF, 2=TFF{BR}clip = havsfunc.QTGMC(clip, TFF=True, Preset='Medium')", False))
        script.Filters.Add(New VideoFilter("Noise", "DFTTest", "clip = core.dfttest.DFTTest(clip, sigma=6, tbsize=3, opt=3)", False))
        script.Filters.Add(New VideoFilter("Resize", "BicubicResize", "clip = core.resize.Bicubic(clip, %target_width%, %target_height%)", False))
        ret.Add(script)

        Return ret
    End Function

    Overrides Function Edit() As DialogResult
        Using form As New CodeEditor(Me)
            form.StartPosition = FormStartPosition.CenterParent

            If form.ShowDialog() = DialogResult.OK Then
                Filters = form.GetFilters

                If Filters.Count = 0 OrElse Filters(0).Category <> "Source" Then
                    MsgError("The first filter must be a source filter.")
                    Filters = GetDefaults(0).Filters
                End If

                Return DialogResult.OK
            End If
        End Using

        Return DialogResult.Cancel
    End Function
End Class

<Serializable()>
Public Class TargetVideoScript
    Inherits VideoScript

    Sub New(name As String)
        Me.Name = name
        CanEditValue = True
    End Sub

    Overrides Property Path() As String
        Get
            If p.SourceFile = "" OrElse p.TargetFile = "" Then
                Return ""
            End If

            Return IO.Path.Combine(p.TempDir, p.TargetFile.Base + "." + FileType)
        End Get
        Set(value As String)
        End Set
    End Property
End Class

<Serializable()>
Public Class SourceVideoScript
    Inherits VideoScript

    Overrides Property Path() As String
        Get
            If p.SourceFile = "" Then
                Return ""
            End If

            Return IO.Path.Combine(p.TempDir, p.TargetFile.Base + "_source." + FileType)
        End Get
        Set(value As String)
        End Set
    End Property

    Public Overrides Property Engine As ScriptEngine
        Get
            Return p.Script.Engine
        End Get
        Set(value As ScriptEngine)
        End Set
    End Property

    Overrides Function GetScript() As String
        Dim rotationFilter = p.Script.Filters.Where(Function(x) x.Active = True AndAlso x.Category = "Rotation")?.FirstOrDefault()
        Dim ret = p.Script.Filters(0).Script
        ret += If(rotationFilter Is Nothing, "", BR + rotationFilter.Script)
        Return ret
    End Function
End Class

<Serializable()>
Public Class VideoFilter
    Implements IComparable(Of VideoFilter)

    Property Active As Boolean
    Property Category As String
    Property Path As String
    Property Script As String

    Sub New()
        Me.New("???", "???", "???", True)
    End Sub

    Sub New(code As String)
        Me.New("???", "???", code, True)
    End Sub

    Sub New(category As String, name As String, script As String, Optional active As Boolean = True)
        Me.Path = name
        Me.Script = script
        Me.Category = category
        Me.Active = active
    End Sub

    ReadOnly Property Name As String
        Get
            Return If(Path.Contains("|"), Path.RightLast("|").Trim, Path)
        End Get
    End Property

    Function GetCopy() As VideoFilter
        Return New VideoFilter(Category, Path, Script, Active)
    End Function

    Overrides Function ToString() As String
        Return Path
    End Function

    Function CompareTo(other As VideoFilter) As Integer Implements System.IComparable(Of VideoFilter).CompareTo
        Return Path.CompareTo(other.Path)
    End Function

    Shared Function GetDefault(category As String, name As String, Optional scriptEngine As ScriptEngine = ScriptEngine.AviSynth) As VideoFilter
        Dim defaults = If(scriptEngine = ScriptEngine.AviSynth, FilterCategory.GetAviSynthDefaults(), FilterCategory.GetVapourSynthDefaults())

        Return defaults.Where(Function(i) i.Name = category).FirstOrDefault()?.Filters.Where(Function(i) i.Name = name).FirstOrDefault()
    End Function
End Class

<Serializable()>
Public Class FilterCategory
    Sub New(name As String)
        Me.Name = name
    End Sub

    Property Name As String

    Private FitersValue As New List(Of VideoFilter)

    ReadOnly Property Filters() As List(Of VideoFilter)
        Get
            If FitersValue Is Nothing Then FitersValue = New List(Of VideoFilter)
            Return FitersValue
        End Get
    End Property

    Overrides Function ToString() As String
        Return Name
    End Function

    Shared Sub AddFilter(filter As VideoFilter, list As List(Of FilterCategory))
        Dim matchingCategory = list.Where(Function(category) category.Name = filter.Category).FirstOrDefault

        If matchingCategory Is Nothing Then
            Dim newCategory As New FilterCategory(filter.Category)
            newCategory.Filters.Add(filter)
            list.Add(newCategory)
        Else
            matchingCategory.Filters.Add(filter)
        End If
    End Sub

    Shared Function GetAviSynthDefaults() As List(Of FilterCategory)
        Using stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("StaxRip.AviSynthFilterProfileDefaults.txt")
            Using sr As New StreamReader(stream)
                Return ParseFilterProfilesIniContent(sr.ReadToEnd)
            End Using
        End Using
    End Function

    Shared Function GetVapourSynthDefaults() As List(Of FilterCategory)
        Using stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("StaxRip.VapourSynthFilterProfileDefaults.txt")
            Using sr As New StreamReader(stream)
                Return ParseFilterProfilesIniContent(sr.ReadToEnd)
            End Using
        End Using
    End Function

    Shared Function ParseFilterProfilesIniContent(content As String) As List(Of FilterCategory)
        Dim ret As New List(Of FilterCategory)
        Dim cat As FilterCategory = Nothing
        Dim filter As VideoFilter = Nothing

        For Each line In content.SplitLinesNoEmpty
            Dim multiline = line.StartsWith("    ") OrElse line.StartsWith(VB.vbTab)

            If line.StartsWith("[") AndAlso line.EndsWith("]") Then
                cat = New FilterCategory(line.Substring(1, line.Length - 2).Trim)
                ret.Add(cat)
            End If

            If multiline Then
                If filter IsNot Nothing Then
                    If filter.Script = "" Then
                        If line.StartsWith(VB.vbTab) Then
                            filter.Script += line.Substring(1)
                        End If

                        If line.StartsWith("    ") Then
                            filter.Script += line.Substring(4)
                        End If
                    Else
                        If line.StartsWith(VB.vbTab) Then
                            filter.Script += BR + line.Substring(1)
                        End If

                        If line.StartsWith("    ") Then
                            filter.Script += BR + line.Substring(4)
                        End If
                    End If
                End If
            Else
                Dim filterName = line.Left("=").Trim

                If filterName <> "" AndAlso cat IsNot Nothing Then
                    filter = New VideoFilter(cat.Name, filterName, line.Right("=").Trim)
                    cat.Filters.Add(filter)
                End If
            End If
        Next

        Return ret
    End Function
End Class

Public Class FilterParameter
    Property Name As String
    Property Value As String

    Sub New(name As String, value As String)
        Me.Name = name
        Me.Value = value
    End Sub
End Class

Public Class FilterParameters
    Property FunctionName As String
    Property Text As String

    Property Parameters As New List(Of FilterParameter)

    Shared DefinitionsValue As List(Of FilterParameters)

    Shared ReadOnly Property Definitions As List(Of FilterParameters)
        Get
            If DefinitionsValue Is Nothing Then
                DefinitionsValue = New List(Of FilterParameters)

                Dim add = Sub(func As String(), path As String, params As FilterParameter())
                              For Each i In func
                                  Dim ret As New FilterParameters With {
                                      .FunctionName = i,
                                      .Text = path
                                  }
                                  DefinitionsValue.Add(ret)
                                  ret.Parameters.AddRange(params)
                              Next
                          End Sub

                Dim add2 = Sub(func As String(), param As String, value As String, path As String)
                               For Each i In func
                                   Dim ret As New FilterParameters With {
                                       .FunctionName = i,
                                       .Text = path
                                   }
                                   DefinitionsValue.Add(ret)
                                   ret.Parameters.Add(New FilterParameter(param, value))
                               Next
                           End Sub

                add({"FFVideoSource",
                     "LWLibavVideoSource",
                     "LSMASHVideoSource",
                     "ffms2.Source",
                     "LibavSMASHSource",
                     "LWLibavSource"}, "fpsnum, fpsden | 30000, 1001", {
                    New FilterParameter("fpsnum", "30000"),
                    New FilterParameter("fpsden", "1001")})

                add({"FFVideoSource",
                     "LWLibavVideoSource",
                     "LSMASHVideoSource",
                     "ffms2.Source",
                     "LibavSMASHSource",
                     "LWLibavSource"}, "fpsnum, fpsden | 60000, 1001", {
                    New FilterParameter("fpsnum", "60000"),
                    New FilterParameter("fpsden", "1001")})

                add({"FFVideoSource",
                     "LWLibavVideoSource",
                     "LSMASHVideoSource",
                     "ffms2.Source",
                     "LibavSMASHSource",
                     "LWLibavSource"}, "fpsnum, fpsden | 24, 1", {
                    New FilterParameter("fpsnum", "24"),
                    New FilterParameter("fpsden", "1")})

                add({"FFVideoSource",
                     "LWLibavVideoSource",
                     "LSMASHVideoSource",
                     "ffms2.Source",
                     "LibavSMASHSource",
                     "LWLibavSource"}, "fpsnum, fpsden | 25, 1", {
                    New FilterParameter("fpsnum", "25"),
                    New FilterParameter("fpsden", "1")})

                add({"FFVideoSource",
                     "LWLibavVideoSource",
                     "LSMASHVideoSource",
                     "ffms2.Source",
                     "LibavSMASHSource",
                     "LWLibavSource"}, "fpsnum, fpsden | 30, 1", {
                    New FilterParameter("fpsnum", "30"),
                    New FilterParameter("fpsden", "1")})

                add({"FFVideoSource",
                     "LWLibavVideoSource",
                     "LSMASHVideoSource",
                     "ffms2.Source",
                     "LibavSMASHSource",
                     "LWLibavSource"}, "fpsnum, fpsden | 50, 1", {
                    New FilterParameter("fpsnum", "50"),
                    New FilterParameter("fpsden", "1")})

                add({"FFVideoSource",
                     "LWLibavVideoSource",
                     "LSMASHVideoSource",
                     "ffms2.Source",
                     "LibavSMASHSource",
                     "LWLibavSource"}, "fpsnum, fpsden | 60, 1", {
                    New FilterParameter("fpsnum", "60"),
                    New FilterParameter("fpsden", "1")})

                add2({"ffms2.Source", "FFVideoSource"}, "rffmode", "0", "rffmode | 0 (ignore all flags (default))")
                add2({"ffms2.Source", "FFVideoSource"}, "rffmode", "1", "rffmode | 1 (honor all pulldown flags)")
                add2({"ffms2.Source", "FFVideoSource"}, "rffmode", "2", "rffmode | 2 (force film)")

                add2({"FFVideoSource"}, "colorspace", """YUV420P8""", "colorspace | YUV420P8")
                add2({"FFVideoSource"}, "colorspace", """YUV422P8""", "colorspace | YUV422P8")
                add2({"FFVideoSource"}, "colorspace", """YUV444P8""", "colorspace | YUV444P8")
                add2({"FFVideoSource"}, "colorspace", """YUV410P8""", "colorspace | YUV410P8")
                add2({"FFVideoSource"}, "colorspace", """YUV411P8""", "colorspace | YUV411P8")
                add2({"FFVideoSource"}, "colorspace", """YUV420P9""", "colorspace | YUV420P9")
                add2({"FFVideoSource"}, "colorspace", """YUV422P9""", "colorspace | YUV422P9")
                add2({"FFVideoSource"}, "colorspace", """YUV444P9""", "colorspace | YUV444P9")
                add2({"FFVideoSource"}, "colorspace", """YUV420P10""", "colorspace | YUV420P10")
                add2({"FFVideoSource"}, "colorspace", """YUV422P10""", "colorspace | YUV422P10")
                add2({"FFVideoSource"}, "colorspace", """YUV444P10""", "colorspace | YUV444P10")
                add2({"FFVideoSource"}, "colorspace", """YUV420P12""", "colorspace | YUV420P12")
                add2({"FFVideoSource"}, "colorspace", """YUV422P12""", "colorspace | YUV422P12")
                add2({"FFVideoSource"}, "colorspace", """YUV444P12""", "colorspace | YUV444P12")
                add2({"FFVideoSource"}, "colorspace", """YUV420P14""", "colorspace | YUV420P14")
                add2({"FFVideoSource"}, "colorspace", """YUV422P14""", "colorspace | YUV422P14")
                add2({"FFVideoSource"}, "colorspace", """YUV444P14""", "colorspace | YUV444P14")
                add2({"FFVideoSource"}, "colorspace", """YUV420P16""", "colorspace | YUV420P16")
                add2({"FFVideoSource"}, "colorspace", """YUV422P16""", "colorspace | YUV422P16")
                add2({"FFVideoSource"}, "colorspace", """YUV444P16""", "colorspace | YUV444P16")
                add2({"FFVideoSource"}, "colorspace", """Y8""", "colorspace | Y8")
                add2({"FFVideoSource"}, "colorspace", """YUY2""", "colorspace | YUY2")
                add2({"FFVideoSource"}, "colorspace", """RGB24""", "colorspace | RGB24")
                add2({"FFVideoSource"}, "colorspace", """RGB32""", "colorspace | RGB32")

                add2({"havsfunc.QTGMC"}, "TFF", "True", "TFF | True (top field first)")
                add2({"havsfunc.QTGMC"}, "TFF", "False", "TFF | False (bottom field first)")

                add2({"QTGMC"}, "Preset", """Draft""", "Preset | Draft")
                add2({"QTGMC"}, "Preset", """Ultra Fast""", "Preset | Ultra Fast")
                add2({"QTGMC"}, "Preset", """Super Fast""", "Preset | Super Fast")
                add2({"QTGMC"}, "Preset", """Very Fast""", "Preset | Very Fast")
                add2({"QTGMC"}, "Preset", """Faster""", "Preset | Faster")
                add2({"QTGMC"}, "Preset", """Fast""", "Preset | Fast")
                add2({"QTGMC"}, "Preset", """Medium""", "Preset | Medium")
                add2({"QTGMC"}, "Preset", """Slow""", "Preset | Slow")
                add2({"QTGMC"}, "Preset", """Slower""", "Preset | Slower")
                add2({"QTGMC"}, "Preset", """Very Slow""", "Preset | Very Slow")
                add2({"QTGMC"}, "Preset", """Placebo""", "Preset | Placebo")

                add2({"LSMASHVideoSource",
                      "LWLibavVideoSource",
                      "LibavSMASHSource",
                      "LWLibavSource"}, "prefer_hw", "0", "prefer_hw | Software Decoder")

                add2({"LSMASHVideoSource",
                      "LWLibavVideoSource",
                      "LibavSMASHSource",
                      "LWLibavSource"}, "prefer_hw", "1", "prefer_hw | NVIDIA CUVID")

                add2({"LSMASHVideoSource",
                      "LWLibavVideoSource",
                      "LibavSMASHSource",
                      "LWLibavSource"}, "prefer_hw", "2", "prefer_hw | Intel Quick Sync")

                add2({"LSMASHVideoSource",
                      "LWLibavVideoSource",
                      "LibavSMASHSource",
                      "LWLibavSource"}, "prefer_hw", "3", "prefer_hw | HW Automatic")

                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV420P8""", "format | YUV420P8")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV422P8""", "format | YUV422P8")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV444P8""", "format | YUV444P8")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV410P8""", "format | YUV410P8")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV411P8""", "format | YUV411P8")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV420P9""", "format | YUV420P9")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV422P9""", "format | YUV422P9")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV444P9""", "format | YUV444P9")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV420P10""", "format | YUV420P10")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV422P10""", "format | YUV422P10")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV444P10""", "format | YUV444P10")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV420P12""", "format | YUV420P12")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV422P12""", "format | YUV422P12")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV444P12""", "format | YUV444P12")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV420P14""", "format | YUV420P14")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV422P14""", "format | YUV422P14")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV444P14""", "format | YUV444P14")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV420P16""", "format | YUV420P16")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV422P16""", "format | YUV422P16")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUV444P16""", "format | YUV444P16")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """Y8""", "format | Y8")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """YUY2""", "format | YUY2")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """RGB24""", "format | RGB24")
                add2({"LSMASHVideoSource", "LWLibavVideoSource", "LibavSMASHSource", "LWLibavSource"}, "format", """RGB32""", "format | RGB32")
            End If

            Return DefinitionsValue
        End Get
    End Property

    Shared Function SplitCSV(input As String) As String()
        Dim chars = input.ToCharArray()
        Dim values As New List(Of String)()
        Dim tempString As String = ""
        Dim isString As Boolean
        Dim characterCount As Integer
        Dim level As Integer

        For Each i In chars
            characterCount += 1

            If i = """"c Then
                If isString Then
                    isString = False
                Else
                    isString = True
                End If
            End If

            If Not isString Then
                If i = "("c Then level += 1
                If i = ")"c Then level -= 1
            End If

            If i <> ","c Then
                tempString = tempString & i
            ElseIf i = ","c AndAlso (isString OrElse level > 0) Then
                tempString = tempString & i
            Else
                values.Add(tempString.Trim)
                tempString = ""
            End If

            If characterCount = chars.Length Then
                values.Add(tempString.Trim)
                tempString = ""
            End If
        Next

        Return values.ToArray()
    End Function
End Class

Public Enum ScriptEngine
    AviSynth
    VapourSynth
End Enum
