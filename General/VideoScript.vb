
Imports System.Text
Imports System.Text.RegularExpressions
Imports StaxRip

<Serializable()>
Public Class VideoScript
    Inherits Profile

    <NonSerialized()> Public [Error] As String
    <NonSerialized()> Public Info As ServerInfo
    <NonSerialized()> Public OriginalInfo As ServerInfo

    Property Filters As New List(Of VideoFilter)

    Overridable Property Engine As ScriptEngine = ScriptEngine.AviSynth
    Overridable Property Path As String = ""

    Shared Event Changed(script As VideoScript)

    Sub RaiseChanged()
        RaiseEvent Changed(Me)
    End Sub

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
                    sb.Append(filter.Script + BR)
                End If
            End If
        Next

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

        If filter?.Script?.ToLower.Contains(search.ToLower) AndAlso filter?.Active Then
            Return True
        End If
    End Function

    Sub ActivateFilter(category As String)
        Dim filter = GetFilter(category)

        If Not filter Is Nothing Then
            filter.Active = True
        End If
    End Sub

    Function IsFilterActive(category As String) As Boolean
        Dim filter = GetFilter(category)
        Return Not filter Is Nothing AndAlso filter.Active
    End Function

    Function IsFilterActive(category As String, name As String) As Boolean
        Dim filter = GetFilter(category)
        Return Not filter Is Nothing AndAlso filter.Active AndAlso filter.Name = name
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

    <NonSerialized()> Public LastCode As String
    <NonSerialized()> Public LastPath As String

    Sub Synchronize(Optional convertToRGB As Boolean = False,
                    Optional comparePath As Boolean = True,
                    Optional flipVertical As Boolean = False)

        If Path = "" Then
            Exit Sub
        End If

        Dim srcFilter = GetFilter("Source")

        If Not srcFilter Is Nothing AndAlso Not srcFilter.Script.Contains("(") Then
            Exit Sub
        End If

        Dim code = Macro.Expand(GetScript())

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

                If flipVertical Then
                    vsCode += BR + "clipname = core.std.FlipVertical(clipname)" + BR
                End If

                vsCode += "
if clipname.format.id == vs.RGB24:
    _matrix_in_s = 'rgb'
else:
    if clipname.height > 576:
        _matrix_in_s = '709'
    else:
        _matrix_in_s = '470bg'

clipname = clipname.resize.Bicubic(matrix_in_s = _matrix_in_s, format = vs.COMPATBGR32)
clipname.set_output()
"
                vsCode = vsCode.Replace("clipname", clipname)
                code = code.Replace(match.Value, vsCode).Trim
            End If
        End If

        If Me.Error <> "" OrElse code <> LastCode OrElse (comparePath AndAlso Path <> LastPath) Then
            If Path.Dir.DirExists Then
                If Engine = ScriptEngine.VapourSynth Then
                    ModifyScript(code, Engine).WriteFile(Path, Encoding.UTF8)
                Else
                    ModifyScript(code, Engine).WriteFileDefault(Path)
                End If

                If Not Package.AviSynth.VerifyOK OrElse
                    Not Package.VapourSynth.VerifyOK OrElse
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
        If p.SourceFile.Ext.EqualsAny("avs", "vpy") Then
            Return script
        End If

        If engine = ScriptEngine.VapourSynth Then
            Return ModifyVSScript(script)
        Else
            Return ModifyAVSScript(script)
        End If
    End Function

    Shared Function GetVsPortableAutoLoadPluginCode() As String
        If FrameServerHelp.IsPortable Then
            Dim ret As String
            Dim dir = Folder.Settings + "Plugins\VapourSynth\"

            If dir.DirExists Then
                For Each file In Directory.GetFiles(dir, "*.dll")
                    ret += "core.std.LoadPlugin(r""" + file + """, altsearchpath=True)" + BR
                Next
            End If

            Return ret
        End If
    End Function

    Shared Function ModifyVSScript(script As String) As String
        Dim code = ""
        ModifyVSScript(script, code)

        If Not script.Contains("import importlib.machinery") AndAlso code.Contains("SourceFileLoader") Then
            code = "import importlib.machinery" + BR + code
        End If

        If Not script.Contains("import vapoursynth") Then
            code =
                "import os, sys" + BR +
                "import vapoursynth as vs" + BR + "core = vs.get_core()" + BR +
                GetVsPortableAutoLoadPluginCode() + BR +
                "sys.path.append(r""" + Folder.Startup + "Apps\Plugins\VS\Scripts"")" + BR + code
        End If

        Dim clip As String

        If code <> "" Then
            clip = code + script
        Else
            clip = script
        End If

        If Not clip.Contains(".set_output(") Then
            If clip.EndsWith(BR) Then
                clip += "clip.set_output()"
            Else
                clip += BR + "clip.set_output()"
            End If
        End If

        Return clip
    End Function

    Shared Function ModifyVSScript(ByRef script As String, ByRef code As String) As String
        For Each plugin In Package.Items.Values.OfType(Of PluginPackage)()
            Dim fp = plugin.Path

            If fp <> "" Then
                If Not plugin.VSFilterNames Is Nothing Then
                    For Each filterName In plugin.VSFilterNames
                        If script.Contains(filterName) Then
                            WriteVSCode(script, code, filterName, plugin)
                        End If
                    Next
                End If

                Dim scriptCode = script + code

                If scriptCode.Contains("import " + plugin.Name) Then
                    WriteVSCode(script, code, Nothing, plugin)
                End If

                If Not plugin.AvsFilterNames Is Nothing Then
                    For Each filterName In plugin.AvsFilterNames
                        If script.Contains(".avs." + filterName) Then
                            WriteVSCode(script, code, filterName, plugin)
                        End If
                    Next
                End If
            End If
        Next
    End Function

    Shared Sub WriteVSCode(
        ByRef script As String,
        ByRef code As String,
        ByRef filterName As String,
        plugin As PluginPackage)

        If plugin.Filename.Ext = "py" Then
            Dim line = plugin.Name + " = importlib.machinery.SourceFileLoader('" +
                plugin.Name + "', r""" + plugin.Path + """).load_module()"

            If Not script.Contains(line) AndAlso Not code.Contains(line) Then
                code = line + BR + code
                Dim scriptCode = plugin.Path.ReadAllText
                ModifyVSScript(scriptCode, code)
            End If
        Else
            If s.LoadVapourSynthPlugins AndAlso Not File.Exists(Folder.Plugins + plugin.Filename) AndAlso
                Not script.Contains(plugin.Filename) AndAlso Not code.Contains(plugin.Filename) Then

                Dim line As String

                If script.Contains(".avs." + filterName) OrElse
                    code.Contains(".avs." + filterName) Then

                    line = "core.avs.LoadPlugin(r""" + plugin.Path + """)" + BR
                Else
                    line = "core.std.LoadPlugin(r""" + plugin.Path + """, altsearchpath=True)" + BR
                End If

                code += line
            End If
        End If
    End Sub

    Shared Function GetAVSLoadCode(script As String, scriptAlready As String) As String
        Dim scriptLower = script.ToLower
        Dim loadCode = ""
        Dim plugins = Package.Items.Values.OfType(Of PluginPackage)()

        For Each plugin In plugins
            Dim fp = plugin.Path

            If fp <> "" Then
                If Not plugin.AvsFilterNames Is Nothing Then
                    For Each filterName In plugin.AvsFilterNames
                        If s.LoadAviSynthPlugins AndAlso
                            Not File.Exists(Folder.Plugins + plugin.Filename) AndAlso
                            scriptLower.Contains(filterName.ToLower) Then

                            If plugin.Filename.Ext = "dll" Then
                                Dim load = "LoadPlugin(""" + fp + """)" + BR

                                If File.Exists(Folder.Plugins + fp.FileName) AndAlso
                                    File.GetLastWriteTimeUtc(Folder.Plugins + fp.FileName) <
                                    File.GetLastWriteTimeUtc(fp) Then

                                    MsgWarn("Conflict with outdated plugin", $"An outdated version of {plugin.Name} is located in your auto load folder. StaxRip includes a newer version.{BR2 + Folder.Plugins + fp.FileName}", True)
                                End If

                                If Not scriptLower.Contains(load.ToLower) AndAlso
                                    Not loadCode.ToLower.Contains(load.ToLower) AndAlso
                                    Not scriptAlready.ToLower.Contains(load.ToLower) Then

                                    loadCode += load
                                End If

                            ElseIf plugin.Filename.Ext = "avsi" Then
                                Dim avsiImport = "Import(""" + fp + """)" + BR

                                If Not scriptLower.Contains(avsiImport.ToLower) AndAlso
                                    Not loadCode.Contains(avsiImport) AndAlso
                                    Not scriptAlready.Contains(avsiImport.ToLower) Then

                                    loadCode += avsiImport
                                End If
                            End If
                        End If
                    Next
                End If
            End If
        Next

        Return loadCode
    End Function

    Shared Function GetAVSLoadCodeFromImports(code As String) As String
        code = code.ToLower
        Dim ret = ""

        For Each line In code.SplitLinesNoEmpty
            If line.Contains("import") Then
                Dim match = Regex.Match(line, "\bimport\s*\(\s*""\s*(.+\.avsi*)\s*""\s*\)", RegexOptions.IgnoreCase)

                If match.Success AndAlso File.Exists(match.Groups(1).Value) Then
                    ret += GetAVSLoadCode(match.Groups(1).Value.ReadAllText, code)
                End If
            End If
        Next

        Return ret
    End Function

    Shared Function ModifyAVSScript(script As String) As String
        Dim newScript As String
        Dim loadCode = GetAVSLoadCode(script, "")
        newScript = loadCode + script
        newScript = GetAVSLoadCodeFromImports(newScript) + newScript

        Dim initCode As String

        If FrameServerHelp.IsPortable Then
            initCode = "AddAutoloadDir(""" + Package.AviSynth.Directory + "plugins"")" + BR +
                       "AddAutoloadDir(""" + Folder.Settings + "Plugins\AviSynth" + """)" + BR
        End If

        Return initCode + newScript
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
        Return CInt(GetFrameCount() / GetFramerate())
    End Function

    Function GetFrameCount() As Integer
        Return GetInfo.FrameCount
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
        script.Filters.Add(New VideoFilter("Noise", "DFTTest", "clip = core.dfttest.DFTTest(clip, sigma=6, tbsize=3, opt=3)", False))
        script.Filters.Add(New VideoFilter("Field", "QTGMC Medium", $"clip = core.std.SetFieldBased(clip, 2) # 1=BFF, 2=TFF{BR}clip = havsfunc.QTGMC(clip, TFF=True, Preset='Medium')", False))
        'script.Filters.Add(New VideoFilter("FrameRate", "SVPFlow", "crop_string = """"" + BR + "resize_string = """"" + BR + "super_params = ""{pel:1,scale:{up:0},gpu:1,full:false,rc:true}""" + BR + "analyse_params = ""{block:{w:16},main:{search:{coarse:{type:4,distance:-6,bad:{sad:2000,range:24}},type:4}},refine:[{thsad:250}]}""" + BR + "smoothfps_params = ""{gpuid:11,linear:true,rate:{num:60000,den:1001,abs:true},algo:23,mask:{area:200},scene:{}}""" + BR + "def interpolate(clip):" + BR + "    input = clip" + BR + "    if crop_string!='':" + BR + "        input = eval(crop_string)" + BR + "    if resize_string!='':" + BR + "        input = eval(resize_string)" + BR + "    super   = core.svp1.Super(input,super_params)" + BR + "    vectors = core.svp1.Analyse(super[""clip""],super[""data""],input,analyse_params)" + BR + "    smooth  = core.svp2.SmoothFps(input,super[""clip""],super[""data""],vectors[""clip""],vectors[""data""],smoothfps_params,src=clip)" + BR + "    smooth  = core.std.AssumeFPS(smooth,fpsnum=smooth.fps_num,fpsden=smooth.fps_den)" + BR + "    return smooth" + BR + "clip =  interpolate(clip)", False))
        'script.Filters.Add(New VideoFilter("Color", "Respec", "clip = core.fmtc.resample(clip, css='444')" + BR + "clip = core.fmtc.matrix(clip, mats='709', matd='709')" + BR + "clip = core.fmtc.resample(clip, css='420')" + BR + "clip = core.fmtc.bitdepth(clip, bits=10, fulls=False, fulld=False)", False))
        script.Filters.Add(New VideoFilter("Resize", "BicubicResize", "clip = core.resize.Bicubic(clip, %target_width%, %target_height%)", False))
        ret.Add(script)

        Return ret
    End Function

    Overrides Function Edit() As DialogResult
        Using f As New CodeEditor(Me)
            f.StartPosition = FormStartPosition.CenterParent

            If f.ShowDialog() = DialogResult.OK Then
                Filters = f.GetFilters

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

            Return p.TempDir + p.TargetFile.Base + "." + FileType
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

            Return (p.TempDir + p.TargetFile.Base + "_source." + FileType).ToShortFilePath
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
        Return p.Script.Filters(0).Script
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

    Sub New(category As String,
            name As String,
            script As String,
            Optional active As Boolean = True)

        Me.Path = name
        Me.Script = script
        Me.Category = category
        Me.Active = active
    End Sub

    ReadOnly Property Name As String
        Get
            If Path.Contains("|") Then Return Path.RightLast("|").Trim
            Return Path
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

    Shared Function GetDefault(category As String, name As String) As VideoFilter
        Return FilterCategory.GetAviSynthDefaults.First(Function(val) val.Name = category).Filters.First(Function(val) val.Name = name)
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

    Shared Sub AddDefaults(engine As ScriptEngine, list As List(Of FilterCategory))
        For Each i In Package.Items.Values.OfType(Of PluginPackage)
            Dim filters As VideoFilter() = Nothing

            If engine = ScriptEngine.AviSynth Then
                If Not i.AvsFiltersFunc Is Nothing Then filters = i.AvsFiltersFunc.Invoke
            Else
                If Not i.VSFiltersFunc Is Nothing Then filters = i.VSFiltersFunc.Invoke
            End If

            If Not filters Is Nothing Then
                For Each iFilter In filters
                    Dim matchingCategory = list.Where(Function(category) category.Name = iFilter.Category).FirstOrDefault

                    If matchingCategory Is Nothing Then
                        Dim newCategory As New FilterCategory(iFilter.Category)
                        newCategory.Filters.Add(iFilter)
                        list.Add(newCategory)
                    Else
                        matchingCategory.Filters.Add(iFilter)
                    End If
                Next
            End If
        Next
    End Sub

    Shared Function GetAviSynthDefaults() As List(Of FilterCategory)
        Dim ret As New List(Of FilterCategory)

        Dim src As New FilterCategory("Source")
        src.Filters.AddRange(
            {New VideoFilter("Source", "Manual", "# shows the filter selection dialog"),
             New VideoFilter("Source", "Automatic", "# can be configured at: Tools > Settings > Source Filters"),
             New VideoFilter("Source", "AviSource", "AviSource(""%source_file%"", audio=false)"),
             New VideoFilter("Source", "DirectShowSource", "DirectShowSource(""%source_file%"", audio=false)")})
        ret.Add(src)

        Dim framerate As New FilterCategory("FrameRate")
        framerate.Filters.Add(New VideoFilter(framerate.Name, "AssumeFPS", "AssumeFPS($select:msg:Select a frame rate;24000/1001|24000, 1001;24;25;30000/1001|30000, 1001;30;50;60000/1001|60000, 1001;60;120;144;240$)"))
        framerate.Filters.Add(New VideoFilter(framerate.Name, "AssumeFPS Source File", "AssumeFPS(%media_info_video:FrameRate%)"))
        framerate.Filters.Add(New VideoFilter(framerate.Name, "ConvertFPS", "ConvertFPS($select:msg:Select a frame rate;24;25;29.970;30;50;59.940;60;120;144;240;$)"))
        framerate.Filters.Add(New VideoFilter(framerate.Name, "SVPFlow", "Threads = 8" + BR + "super_params = ""{pel:2,gpu:1}""" + BR + "analyse_params = """"""{block:{w:16,h:16}, main:{search:{coarse:{distance:-10}}}, refine:[{thsad:200}]}"""""" " + BR + "smoothfps_params = ""{rate:{num:4,den:2},algo:23,cubic:1}""" + BR + "super = SVSuper(super_params)" + BR + "vectors = SVAnalyse(super, analyse_params)" + BR + "SVSmoothFps(super, vectors, smoothfps_params, mt=threads)" + BR + "#Prefetch(threads) must be added at the end of the script and Threads=9 after the source" + BR + "Prefetch(threads)"))
        ret.Add(framerate)

        Dim color As New FilterCategory("Color")
        color.Filters.Add(New VideoFilter(color.Name, "Convert | Format", "z_ConvertFormat(pixel_type=""$enter_text:Enter The Format You Wish To Convert To$"", colorspace_op=""$select:msg:Select Color Matrix Input;RGB;FCC;YCGCO;240m;709;2020ncl$:$select:msg:Select Color Transfer Input;Linear;Log100;Log316;470m;470bg;240m;XVYCC;SRGB;709;2020;st2084$:$select:msg:Select Color Primaries Input;470m;470bg;FILM;709;2020$:l=>$select:msg:Select Color Matrix output;RGB;FCC;YCGCO;240m;709;2020ncl$:$select:msg:Select Color Transfer Output;Linear;Log100;Log316;470m;470bg;240m;XVYCC;SRGB;709;2020;st2084$:$select:msg:Select Color Primaries Output;470m;470bg;FILM;709;2020$:l"", dither_type=""$select:msg:Select Dither Type;None;ordered$"")"))
        color.Filters.Add(New VideoFilter(color.Name, "ColorYUV | AutoAdjust", "AutoAdjust(gamma_limit=1.0, scd_threshold=16, gain_mode=1, auto_gain=$select:msg:Enable Auto Gain?;true;false$, auto_balance=$select:msg:Enable Auto Balance?;true;false$, Input_tv=$select:msg:Is the Input using TV Range?;true;false$, output_tv=$select:msg:Do you want to use TV Range for Output?;true;false$, use_dither=$select:msg:Use Dither?;true;false$, high_quality=$select:msg:Use High Quality Mode?;true;false$, high_bitdepth=$select:msg:Use High Bit Depth Mode?;true;false$, threads_count=$enter_text:How Many Threads do You Wish to use?$)"))
        color.Filters.Add(New VideoFilter(color.Name, "ColorYUV | Levels", "$select:TV to PC|ColorYUV(levels=""TV->PC"");PC to TV|ColorYUV(levels=""PC->TV"")$"))
        color.Filters.Add(New VideoFilter(color.Name, "ColorYUV | Stack", "$select:To Stack|ConvertToStacked();From Stacked|ConvertFromStacked()$"))
        color.Filters.Add(New VideoFilter(color.Name, "Convert | ConvertTo", "ConvertTo$enter_text:Enter The Format You Wish To Convert To$()"))
        color.Filters.Add(New VideoFilter(color.Name, "Convert | ConvertBits", "ConvertBits($select:msg:Select the Bit Depth You want to Convert To;8;10;12;14;16;32$)"))
        color.Filters.Add(New VideoFilter(color.Name, "Convert | ConvertFromDoubleWidth", "ConvertFromDoubleWidth(bits=$select:msg:Select the Bit Depth;8;10;12;14;16;32$)"))
        color.Filters.Add(New VideoFilter(color.Name, "Dither | Gamma / Linear", "$select:Gamma To Linear|Dither_y_gamma_to_linear;Linear To Gamma|Dither_y_linear_to_gamma$(curve=""$select:msg:Select the Color Curve;601;709;2020$"")"))
        color.Filters.Add(New VideoFilter(color.Name, "Dither | Sigmoid", "$select:Sigmoid Direct|Dither_sigmoid_direct();Sigmoid Inverse|Dither_sigmoid_inverse()$"))
        color.Filters.Add(New VideoFilter(color.Name, "Dither | 8Bit to 16Bit", "Dither_convert_8_to_16()"))
        color.Filters.Add(New VideoFilter(color.Name, "Dither | YUV / RGB", "$select:RGB To YUV|Dither_convert_rgb_to_yuv();YUV To RGB|Dither_convert_yuv_to_rgb()$"))
        color.Filters.Add(New VideoFilter(color.Name, "Dither | DFTTest(LSB)", "dfttest(lsb=true)"))
        color.Filters.Add(New VideoFilter(color.Name, "Dither | DitherPost", "DitherPost()"))
        color.Filters.Add(New VideoFilter(color.Name, "ColorYUV | AutoGain", "ColorYUV(autogain=$select:msg:Enable AutoGain?;true;false$, autowhite=$select:msg:Enable AutoWhite?;true;false$)"))
        color.Filters.Add(New VideoFilter(color.Name, "ColorYUV | Tweak", "Tweak(realcalc=true, dither_strength=1.0, sat=0.75, startHue=105, endHue=138 )"))
        color.Filters.Add(New VideoFilter(color.Name, "HDRCore | Tone Mapping", "$select:msg:Select the Map Tone You Wish to Use;DGReinhard|DGReinhard();DGHable|DGHable()$"))
        ret.Add(color)

        Dim line As New FilterCategory("Line")
        line.Filters.Add(New VideoFilter(line.Name, "Anti-Aliasing | SangNom2", "Sangnom2()"))
        line.Filters.Add(New VideoFilter(line.Name, "Sharpen | MultiSharpen", "MultiSharpen(1)"))
        ret.Add(line)

        Dim field As New FilterCategory("Field")
        field.Filters.Add(New VideoFilter(field.Name, "IVTC", "Telecide(guide=1)" + BR + "Decimate()"))
        field.Filters.Add(New VideoFilter(field.Name, "FieldDeinterlace", "FieldDeinterlace()"))
        field.Filters.Add(New VideoFilter(field.Name, "Select", "$select:Even|SelectEven();Odd|SelectOdd()$"))
        field.Filters.Add(New VideoFilter(field.Name, "Assume", "$select:TFF|AssumeTFF();BFF|AssumeTFF()$"))
        ret.Add(field)

        Dim noise As New FilterCategory("Noise")
        noise.Filters.Add(New VideoFilter(noise.Name, "RemoveGrain | RemoveGrain | RemoveGrain16 with Repair16", "Processed = Dither_removegrain16(mode=2, modeU=2, modeV=2)" + BR + "Dither_repair16(Processed, mode=2, modeU=2, modeV=2)"))
        ret.Add(noise)

        Dim misc As New FilterCategory("Misc")
        misc.Filters.Add(New VideoFilter(misc.Name, "MTMode | Prefetch", "Prefetch($enter_text:Enter the Number of Threads to Use$)")) ' Number or Reference to Thread header can Be Used here.
        misc.Filters.Add(New VideoFilter(misc.Name, "MTMode | Set Threads", "threads=$enter_text:Enter The Number of Threads to Use$"))
        misc.Filters.Add(New VideoFilter(misc.Name, "MTMode | SetMTMode Filter", "SetFilterMTMode(""$enter_text:Enter The FilterName$"",$enter_text:Enter Mode You Wish to Use$)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "MTMode | Set Max Memory", "SetMemoryMax($enter_text:Enter the Maximum Memory Avisynth Can use$)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "Histogram", "Histogram(""levels"", bits=$select:msg:Select BitDepth;8;10;12$)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "SplitVertical", "Splitvertical=true"))
        misc.Filters.Add(New VideoFilter(misc.Name, "AddBorders", "AddBorders(0,0,0,0) #left,top,right,bottom"))
        misc.Filters.Add(New VideoFilter(misc.Name, "SelectRangeEvery", "SelectRangeEvery(1500,50)"))

        ret.Add(misc)

        Dim resize As New FilterCategory("Resize")
        resize.Filters.Add(New VideoFilter(resize.Name, "Resize...", "$select:BicubicResize;BilinearResize;BlackmanResize;GaussResize;Lanczos4Resize;LanczosResize;PointResize;SincResize;Jinc36Resize;Jinc64Resize;Jinc144Resize;Jinc256Resize;Spline16Resize;Spline36Resize;Spline64Resize$(%target_width%, %target_height%)"))
        resize.Filters.Add(New VideoFilter(resize.Name, "Advanced | Hardware Encoder", "# hardware encoder resizes"))
        resize.Filters.Add(New VideoFilter(resize.Name, "Advanced | SuperRes", "$select:msg:Select Version;SuperResXBR|SuperResXBR;SuperRes|SuperRes;SuperXBR|SuperXBR$(passes=$select:msg:How Many Passes Do you wish to Perform?;2;3;4;5$, factor=$select:msg:Factor Increase by?;2;4$)"))
        resize.Filters.Add(New VideoFilter(resize.Name, "Advanced | Dither_Resize16", "Dither_resize16(%target_width%, %target_height%)"))
        resize.Filters.Add(New VideoFilter(resize.Name, "Advanced | Dither_Resize16 In Linear Light", "Dither_convert_yuv_to_rgb (matrix=""2020"", output=""rgb48y"", lsb_in=true)" + BR + "Dither_y_gamma_to_linear(tv_range_in=false, tv_range_out=false, curve=""2020"", sigmoid=true)" + BR + "Dither_resize16nr(%target_width%, %target_height%, kernel=""spline36"")" + BR + "Dither_y_linear_to_gamma(tv_range_in=false, tv_range_out=false, curve=""2020"", sigmoid=true)" + BR + "r = SelectEvery (3, 0)" + BR + "g = SelectEvery (3, 1)" + BR + "b = SelectEvery (3, 2)" + BR + "Dither_convert_rgb_to_yuv(r, g, b, matrix=""2020"", lsb=true)"))
        ret.Add(resize)

        Dim crop As New FilterCategory("Crop")
        crop.Filters.Add(New VideoFilter(crop.Name, "Crop", "$select:msg:Select Version;Crop;Dither_Crop16$(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)"))
        crop.Filters.Add(New VideoFilter(crop.Name, "Hardware Encoder", "# hardware encoder crops"))
        ret.Add(crop)

        Dim restoration As New FilterCategory("Restoration")
        restoration.Filters.Add(New VideoFilter(restoration.Name, "RCR | ColorBanding", "$select:GradFun3|GradFun3();f3kdb|f3kdb()$"))
        ret.Add(restoration)

        FilterCategory.AddDefaults(ScriptEngine.AviSynth, ret)

        For Each i In ret
            i.Filters.Sort()
        Next

        Return ret
    End Function

    Shared Function GetVapourSynthDefaults() As List(Of FilterCategory)
        Dim ret As New List(Of FilterCategory)

        Dim src As New FilterCategory("Source")
        src.Filters.AddRange(
            {New VideoFilter("Source", "Manual", "# shows filter selection dialog"),
             New VideoFilter("Source", "Automatic", "# can be configured at: Tools > Settings > Source Filters"),
             New VideoFilter("Source", "AVISource", "clip = core.avisource.AVISource(r""%source_file%"")")})
        ret.Add(src)

        Dim framerate As New FilterCategory("FrameRate")
        framerate.Filters.Add(New VideoFilter(framerate.Name, "AssumeFPS | AssumeFPS Source", "clip = core.std.AssumeFPS(clip, fpsnum=int(%media_info_video:FrameRate% * 1000), fpsden=1000)"))
        framerate.Filters.Add(New VideoFilter(framerate.Name, "AssumeFPS | AssumeFPS", "clip = core.std.AssumeFPS(clip, None, $select:msg:Select a frame rate.;24000/1001|24000, 1001;24|24, 1;25|25, 1;30000/1001|30000, 1001;30|30, 1;50|50, 1;60000/1001|60000, 1001;60|60, 1$)"))
        framerate.Filters.Add(New VideoFilter(framerate.Name, "InterFrame", "clip = havsfunc.InterFrame(clip, Preset='Medium', Tuning='$select:msg:Select the Tuning Preset;Animation;Film;Smooth;Weak$', NewNum=$enter_text:Enter the NewNum Value$, NewDen=$enter_text:Enter the NewDen Value$, OverrideAlgo=$select:msg:Which Algorithm Do you Wish to Use?;Strong Predictions|2;Intelligent|13;Smoothest|23$, GPU=$select:msg:Enable GPU Feature?;True;False$)"))
        framerate.Filters.Add(New VideoFilter(framerate.Name, "SVPFlow | MV", "sup = core.mv.Super(clip, pel=2, hpad=0, vpad=0)" + BR + "bvec = core.mv.Analyse(sup, blksize=16, isb=True, chroma=True, search=3, searchparam=1)" + BR + "fvec = core.mv.Analyse(sup, blksize=16, isb=False, chroma=True, search=3, searchparam=1)" + BR + "$select:msg:Select FPS Filter to Use;FlowFPS|clip = core.mv.FlowFPS(clip, sup, bvec, fvec, mask=2;BlockFPS|clip = core.mv.BlockFPS(clip, sup, bvec, fvec, mode=3, thscd2=12$, num=$enter_text:Enter The Num Value$, den=$enter_text:Enter The Den Value$)"))
        framerate.Filters.Add(New VideoFilter(framerate.Name, "SVPFlow | Core", "crop_string = ''" + BR + "resize_string = ''" + BR + "super_params = '{pel:1,scale:{up:0},gpu:1,full:false,rc:true}'" + BR + "analyse_params = '{block:{w:16},main:{search:{coarse:{type:4,distance:-6,bad:{sad:2000,range:24}},type:4}},refine:[{thsad:250}]}'" + BR + "smoothfps_params = '{gpuid:11,linear:true,rate:{num:60000,den:1001,abs:true},algo:23,mask:{area:200},scene:{}}'" + BR + "def interpolate(clip):" + BR + "    input = clip" + BR + "    if crop_string!='':" + BR + "        input = eval(crop_string)" + BR + "    if resize_string != '':" + BR + "        input = eval(resize_string)" + BR + "    super   = core.svp1.Super(input, super_params)" + BR + "    vectors = core.svp1.Analyse(super['clip'], super['data'], input, analyse_params)" + BR + "    smooth  = core.svp2.SmoothFps(input, super['clip'], super['data'], vectors['clip'], vectors['data'], smoothfps_params, src=clip)" + BR + "    smooth  = core.std.AssumeFPS(smooth, fpsnum=smooth.fps_num, fpsden=smooth.fps_den)" + BR + "    return smooth" + BR + "clip =  interpolate(clip)"))
        ret.Add(framerate)

        Dim color As New FilterCategory("Color")
        color.Filters.Add(New VideoFilter(color.Name, "Dither | Gamma / Linear", "clip = $select:Gamma To Linear|Dither.gamma_to_linear;Linear To Gamma|Dither.linear_to_gamma$(clip, curve='$select:msg:Select the Color Curve;601;709;2020$')"))
        color.Filters.Add(New VideoFilter(color.Name, "Dither | Sigmoid", "$select:Sigmoid Inverse|clip = havsfunc.SigmoidInverse(clip);Sigmoid Direct|clip = havsfunc.SigmoidDirect(clip)$"))
        color.Filters.Add(New VideoFilter(color.Name, "Dither | SmoothGrad", "clip = muvsfunc.GradFun3(src=clip, mode=6, smode=1)"))
        color.Filters.Add(New VideoFilter(color.Name, "Dither | Stack", "$select:Native to Stack16|clip = core.fmtc.nativetostack16(clip);Stack16 to Native|clip = fmtc.stack16tonative(clip)$"))
        color.Filters.Add(New VideoFilter(color.Name, "Dither | To RGB / YUV", "clip = $select:To RGB|mvsfunc.ToRGB;To YUV|mvsfunc.ToYUV$(clip,matrix='$select:msg:Select Matrix;470bg;240;709;2020;2020cl;bt2020c$')"))
        color.Filters.Add(New VideoFilter(color.Name, "ColorSpace | Respec", "clip = core.fmtc.resample (clip, css='444')" + BR + "clip = core.fmtc.matrix (clip, mats='$select:msg:Select Input Colorspace;240;601;709;2020$', matd='$select:msg:Select Output Colorspace;240;601;709;2020$')" + BR + "clip = core.fmtc.resample (clip, css='420')" + BR + "clip = core.fmtc.bitdepth (clip, bits=8, fulls=$select:msg:Select Input Range;Limited|False;Full|True$, fulld=$select:msg:Select Output Range;Limited|False;Full|True$)"))
        color.Filters.Add(New VideoFilter(color.Name, "ColorSpace | Matrix", "clip = core.fmtc.matrix(clip, mat='$select:msg:Select Matrix;Linear;470m;470bg;240m;SRGB;709;2020$')"))
        color.Filters.Add(New VideoFilter(color.Name, "ColorSpace | Primaries", "clip = core.fmtc.primaries(clip, prims='$select:msg:Select Input;470m;470bg;240m;SRGB;709;2020$', primd='$select:msg:Select Output;Linear;470m;470bg;240m;SRGB;709;2020$')"))
        color.Filters.Add(New VideoFilter(color.Name, "ColorSpace | Transfer", "clip = core.fmtc.transfer(clip, transs='$select:msg:Select Input;Linear;470m;470bg;240m;SRGB;709;2020;2084$', transd='$select:msg:Select Output;Linear;470m;470bg;240m;SRGB;709;2020;2084$')"))
        color.Filters.Add(New VideoFilter(color.Name, "ColorSpace | Range", "$select:msg:Select Range;PC to TV|clip = core.fmtc.bitdepth(clip, fulls=True, fulld=False);TV to PC|clip = core.fmtc.bitdepth (clip, fulls=False, fulld=True)$"))
        color.Filters.Add(New VideoFilter(color.Name, "Convert | Tweak", "clip = adjust.Tweak(clip, sat=0.75, hue=115-138)"))
        color.Filters.Add(New VideoFilter(color.Name, "Convert | BitDepth (fmtc)", "clip = core.fmtc.bitdepth(clip, bits=$select:msg:Select BitDepth;8;10;12;16;32$)"))
        color.Filters.Add(New VideoFilter(color.Name, "Convert | Format", "clip = core.avs.z_ConvertFormat(clip, pixel_type='$enter_text:Enter The Format You Wish To Convert To$',colorspace_op='$select:msg:Select Color Matrix Input;RGB;240m;709;2020ncl$:$select:msg:Select Color Transfer Input;Linear;470m;470bg;240m;SRGB;709;2020;st2084$:$select:msg:Select Color Primaries Input;470m;470bg;FILM;709;2020$:l=>$select:msg:Select Color Matrix output;RGB;FCC;YCGCO;240m;709;2020ncl$:$select:msg:Select Color Transfer Output;Linear;Log100;Log316;470m;470bg;240m;XVYCC;SRGB;709;2020;st2084$:$select:msg:Select Color Primaries Output;470m;470bg;FILM;709;2020$:l', dither_type='$select:msg:Select Dither Type;None;ordered$')"))
        color.Filters.Add(New VideoFilter(color.Name, "Convert | Convert To", "clip = core.resize.Bicubic(clip, format=vs.$select:COMPATBGR32;COMPATYUY2;GRAY16;GRAY8;GRAYH;GRAYS;RGB24;RGB27;RGB30;RGB48;RGBH;RGBS;YUV410P8;YUV411P8;YUV420P8;YUV420P9;YUV420P10;YUV420P12;YUV420P14;YUV420P16;YUV422P8;YUV422P9;YUV422P10;YUV422P12;YUV422P14;YUV422P16;YUV440P8;YUV444P8;YUV444P9;YUV444P10;YUV444P12;YUV444P14;YUV444P16;YUV444PH;YUV444PS$)"))
        color.Filters.Add(New VideoFilter(color.Name, "Convert | To 444", "clip = core.fmtc.resample(clip, css='444')"))
        ret.Add(color)

        Dim line As New FilterCategory("Line")
        line.Filters.Add(New VideoFilter(line.Name, "Anti-Aliasing | DAA", "clip = havsfunc.daa(clip)"))
        line.Filters.Add(New VideoFilter(line.Name, "Anti-Aliasing | nnedi3AA", "clip = muvsfunc.nnedi3aa(clip)"))
        line.Filters.Add(New VideoFilter(line.Name, "Anti-Aliasing | ediAA", "clip = muvsfunc.ediaa(clip)"))
        line.Filters.Add(New VideoFilter(line.Name, "Anti-Aliasing | Santiag", "clip = havsfunc.santiag(c=clip, opencl=$select:msg:Use GPU Enabled Feature?;True;False$)"))
        line.Filters.Add(New VideoFilter(line.Name, "Anti-Aliasing | MAA", "clip = muvsfunc.maa(clip)"))
        line.Filters.Add(New VideoFilter(line.Name, "Sharpen | LSFmod", "clip = havsfunc.LSFmod(clip, defaults='slow', strength=100, Smode=5, Smethod=3, kernel=11, preblur='OFF', secure=True, Szrp= 16, Spwr= 4, SdmpLo= 4, SdmpHi= 48, Lmode=4, overshoot=1, undershoot=1, soft=-2, soothe=True, keep=20, edgemode=0, edgemaskHQ=True, ss_x= 1.50, ss_y=1.50)"))
        line.Filters.Add(New VideoFilter(line.Name, "Sharpen | SharpAAMcmod", "clip = muvsfunc.SharpAAMcmod(clip)"))
        ret.Add(line)

        Dim field As New FilterCategory("Field")
        field.Filters.Add(New VideoFilter(field.Name, "IVTC", "clip = core.vivtc.VFM(clip, 1)" + BR + "clip = core.vivtc.VDecimate(clip)"))
        field.Filters.Add(New VideoFilter(field.Name, "W3FDIF", "clip = core.w3fdif.W3FDIF(clip,$select:msg:Select Order Option;BFF|0;TFF|1$,$select:msg:Select Interlacing Filter Coefficients;Simple|0;Complex|1$)"))
        field.Filters.Add(New VideoFilter(field.Name, "Select", "$select:Even|clip = clip[::2];Odd|clip = clip[1::2]$"))
        field.Filters.Add(New VideoFilter(field.Name, "eedi", "$select:eedi2|clip = core.eedi2.EEDI2;eedi3|clip = core.eedi3m.EEDI3$(clip,$select:msg:Select Field;Bottom Field|0;Top Field|1;Alternate Each Frame Bottom Field|2;Alternate Each Frame Top Field|3$)"))
        field.Filters.Add(New VideoFilter(field.Name, "nnedi3", "$select:znedi3|clip = core.znedi3.nnedi3;nnedi3cl|clip = core.nnedi3cl.NNEDI3CL;nnedi3|clip = core.nnedi3.nnedi3$(clip, field=$select:msg:Select Field Option;Same Rate Bottom Field|0;Same Rate Top Field|1;Double Rate Alternates Bottom Field|2;Double Rate Alternates Top Field|3$)"))
        field.Filters.Add(New VideoFilter(field.Name, "Field Base", "$select:Frame Based|clip = core.std.SetFieldBased(clip, 0);Bottom Field First|clip = core.std.SetFieldBased(clip, 1);Top Field First|clip = core.std.SetFieldBased(clip, 2)$"))
        ret.Add(field)

        Dim noise As New FilterCategory("Noise")
        noise.Filters.Add(New VideoFilter(noise.Name, "RemoveGrain | SMDegrain", "clip = havsfunc.SMDegrain(clip, contrasharp=True)"))
        noise.Filters.Add(New VideoFilter(noise.Name, "RemoveGrain | RemoveGrain | RemoveGrain", "clip = core.rgvs.RemoveGrain(clip, 1)"))
        noise.Filters.Add(New VideoFilter(noise.Name, "RemoveGrain | RemoveGrain | RemoveGrain with Repair", "Processed = core.rgvs.RemoveGrain(clip, 1)" + BR + "clip = core.rgvs.Repair(clip,Processed, mode=2)"))
        noise.Filters.Add(New VideoFilter(noise.Name, "mClean", "clip = G41Fun.mClean(clip)"))
        noise.Filters.Add(New VideoFilter(noise.Name, "MCTemporalDenoise", "clip = havsfunc.MCTemporalDenoise(i=clip, settings='$select:msg:Select Strength;Very Low;Low;Medium;High;Very High$')"))
        noise.Filters.Add(New VideoFilter(noise.Name, "BM3D", "clip = mvsfunc.BM3D(clip, sigma=[3,3,3], radius1=0)"))
        ret.Add(noise)

        Dim misc As New FilterCategory("Misc")
        misc.Filters.Add(New VideoFilter(misc.Name, "UnSpec", "clip = core.resize.Point(clip, matrix_in_s='unspec',range_s='limited')" + BR + "clip = core.std.AssumeFPS(clip, fpsnum=int(%media_info_video:FrameRate% * 1000), fpsden=1000)" + BR + "clip = core.std.SetFrameProp(clip=clip, prop='_ColorRange', intval=1)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "Histogram", "clip = muvsfunc.DisplayHistogram(clip)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "Cube", "clip = core.timecube.Cube(clip, cube=r""$browse_file$"")"))
        misc.Filters.Add(New VideoFilter(misc.Name, "Anamorphic to Standard", "clip = core.fmtc.resample (clip, w=1280, h=720, css='444')" + BR + "clip = core.fmtc.matrix (clip, mat='709', col_fam=vs.RGB)" + BR + "clip = core.fmtc.transfer (clip, transs='1886', transd='srgb')" + BR + "clip = core.fmtc.bitdepth (clip, bits=8)"))
        ret.Add(misc)

        Dim resize As New FilterCategory("Resize")
        resize.Filters.Add(New VideoFilter(resize.Name, "Resize...", "clip = $select:Bilinear|core.resize.Bilinear;Bicubic|core.resize.Bicubic;Point|core.resize.Point;Lanczos|core.resize.Lanczos;Spline16|core.resize.Spline16;Spline36|core.resize.Spline36;Spline64|core.resize.Spline64;Dither_Resize16|Dither.Resize16nr;Resample|core.fmtc.resample$(clip, %target_width%, %target_height%)"))
        ret.Add(resize)

        Dim crop As New FilterCategory("Crop")
        crop.Filters.Add(New VideoFilter(crop.Name, "Crop", "clip = core.std.Crop(clip, %crop_left%, %crop_right%, %crop_top%, %crop_bottom%)"))
        ret.Add(crop)

        Dim restoration As New FilterCategory("Restoration")
        restoration.Filters.Add(New VideoFilter(restoration.Name, "RCR | ColorBanding", "$select:GradFun3|clip = muvsfunc.GradFun3(src=clip, mode=6, smode=1);f3kdb|clip = core.f3kdb.Deband(clip)$"))
        restoration.Filters.Add(New VideoFilter(restoration.Name, "DeHalo | FineDehalo", "clip = havsfunc.FineDehalo(clip)"))
        restoration.Filters.Add(New VideoFilter(restoration.Name, "DeHalo | DeHaloAlpha", "clip = havsfunc.DeHalo_alpha(clip)"))
        restoration.Filters.Add(New VideoFilter(restoration.Name, "DeBlock | Deblock_QED", "clip = havsfunc.Deblock_QED(clip)"))
        restoration.Filters.Add(New VideoFilter(restoration.Name, "DeBlock | Auto-Deblock", "clip = fvsfunc.AutoDeblock(clip)"))
        restoration.Filters.Add(New VideoFilter(restoration.Name, "DeHalo | BlindDeHalo3", "clip = muvsfunc.BlindDeHalo3(clip, interlaced=False)"))
        restoration.Filters.Add(New VideoFilter(restoration.Name, "DeHalo | EdgeCleaner", "clip = havsfunc.EdgeCleaner(clip)"))
        restoration.Filters.Add(New VideoFilter(restoration.Name, "DeHalo | YAHR", "$select:YAHR|clip = havsfunc.YAHR(clip);YAHRmod|clip = muvsfunc.YAHRmod(clip)$"))
        restoration.Filters.Add(New VideoFilter(restoration.Name, "DeHalo | MDeRing", "clip = muvsfunc.mdering(clip)"))
        restoration.Filters.Add(New VideoFilter(restoration.Name, "DeHalo | HQDering", "clip = havsfunc.HQDeringmod(clip, nrmode=2, darkthr=3.0)"))
        restoration.Filters.Add(New VideoFilter(restoration.Name, "RCR | Vinverse", "$select:Vinverse|clip = havsfunc.Vinverse(clip);Vinverse2|clip = havsfunc.Vinverse2(clip)$"))
        restoration.Filters.Add(New VideoFilter(restoration.Name, "RCR | CNR2", "clip = core.cnr2.Cnr2(clip)"))
        ret.Add(restoration)

        FilterCategory.AddDefaults(ScriptEngine.VapourSynth, ret)

        For Each i In ret
            i.Filters.Sort()
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

                Dim add = Sub(func As String(),
                              path As String,
                              params As FilterParameter())

                              For Each i In func
                                  Dim ret As New FilterParameters
                                  ret.FunctionName = i
                                  ret.Text = path
                                  DefinitionsValue.Add(ret)
                                  ret.Parameters.AddRange(params)
                              Next
                          End Sub

                Dim add2 = Sub(func As String(),
                               param As String,
                               value As String,
                               path As String)

                               For Each i In func
                                   Dim ret As New FilterParameters
                                   ret.FunctionName = i
                                   ret.Text = path
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
        Dim tempString As String
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
