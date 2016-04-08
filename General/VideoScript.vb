Imports System.Text
Imports StaxRip

<Serializable()>
Public Class VideoScript
    Inherits Profile

    <NonSerialized()> Private Framerate As Double
    <NonSerialized()> Private Frames As Integer
    <NonSerialized()> Private Size As Size
    <NonSerialized()> Private ErrorMessage As String

    <NonSerialized()> Public LastSync As String

    Property Filters As New List(Of VideoFilter)

    Overridable Property Engine As ScriptingEngine = ScriptingEngine.AviSynth
    Overridable Property Path As String = ""

    Shared Event Changed(script As VideoScript)

    Sub RaiseChanged()
        RaiseEvent Changed(Me)
    End Sub

    Overridable ReadOnly Property FileType As String
        Get
            If Engine = ScriptingEngine.VapourSynth Then Return "vpy"
            Return "avs"
        End Get
    End Property

    Overridable Function GetScript() As String
        Return GetScript(Nothing)
    End Function

    Overridable Function GetScript(skipCategory As String) As String
        Dim sb As New StringBuilder()

        If p.CodeAtTop <> "" Then sb.AppendLine(p.CodeAtTop)

        For Each i As VideoFilter In Filters
            If i.Active Then
                If skipCategory Is Nothing OrElse i.Category <> skipCategory Then
                    sb.Append(i.Script + CrLf)
                End If
            End If
        Next

        Return sb.ToString
    End Function

    Function GetFullScript() As String
        Return Macro.Solve(ModifyScript(GetScript, Engine)).Trim
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
        If category = "" OrElse search = "" Then Exit Function
        Dim filter = GetFilter(category)
        If filter?.Script?.ToLower?.Contains(search.ToLower) Then Return True
    End Function

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

    Function GetFilter(category As String) As VideoFilter
        For Each i In Filters
            If i.Category = category Then Return i
        Next
    End Function

    Sub Synchronize(Optional convertToRGB As Boolean = False)
        If Path <> "" Then
            Dim script = Macro.Solve(GetScript())

            If convertToRGB Then
                If Engine = ScriptingEngine.AviSynth Then
                    If p.SourceHeight > 576 Then
                        script += CrLf + "ConvertToRGB(matrix=""Rec709"")"
                    Else
                        script += CrLf + "ConvertToRGB(matrix=""Rec601"")"
                    End If
                Else
                    If p.SourceHeight > 576 Then
                        If script.Contains(".set_output()") Then
                            script = script.Replace(".set_output()", ".resize.Bicubic(matrix_in_s = '709', format = vs.COMPATBGR32).set_output()")
                        Else
                            script += CrLf + "clip = clip.resize.Bicubic(matrix_in_s = '709', format = vs.COMPATBGR32)"
                        End If
                    Else
                        If script.Contains(".set_output()") Then
                            script = script.Replace(".set_output()", ".resize.Bicubic(matrix_in_s = '470bg', format = vs.COMPATBGR32).set_output()")
                        Else
                            script += CrLf + "clip = clip.resize.Bicubic(matrix_in_s = '470bg', format = vs.COMPATBGR32)"
                        End If
                    End If
                End If
            End If

            Dim current = Path + script

            If Frames = 240 OrElse current <> LastSync Then
                If Directory.Exists(Filepath.GetDir(Path)) Then
                    script = ModifyScript(script, Engine)

                    If Engine = ScriptingEngine.VapourSynth Then
                        script.WriteFile(Path, Encoding.UTF8)
                    Else
                        script.WriteFile(Path)
                    End If

                    If p.SourceFile <> "" Then
                        If g.MainForm.Visible Then
                            g.MainForm.Indexing()
                            ProcessForm.CloseProcessForm()
                        Else
                            g.MainForm.Indexing()
                        End If
                    End If

                    If Not Packs.AviSynth.VerifyOK OrElse Not Packs.VapourSynth.VerifyOK Then
                        Throw New AbortException
                    End If

                    Using avi As New AVIFile(Path)
                        Framerate = avi.FrameRate
                        Frames = avi.FrameCount
                        Size = avi.FrameSize
                        ErrorMessage = avi.ErrorMessage

                        If Double.IsNaN(Framerate) Then
                            Throw New ErrorAbortException("AviSynth/VapourSynth Error",
                                                          "AviSynth/VapourSynth script returned invalid framerate.")
                        End If
                    End Using

                    LastSync = current
                End If
            End If
        End If
    End Sub

    Shared Function ModifyScript(script As String, engine As ScriptingEngine) As String
        Dim scriptLower = script.ToLower

        Dim code = ""

        If engine = ScriptingEngine.VapourSynth AndAlso Not scriptLower.Contains("import vapoursynth") Then
            code = "import vapoursynth as vs" + CrLf + "core = vs.get_core()" + CrLf
        End If

        Dim plugins = Packs.Packages.OfType(Of PluginPackage)()

        For Each i In plugins
            Dim fp = i.GetPath

            If fp <> "" Then
                If engine = ScriptingEngine.VapourSynth AndAlso code.Contains("core = vs.") Then
                    If Not i.VapourSynthFilterNames Is Nothing Then
                        For Each iFilterName In i.VapourSynthFilterNames
                            If scriptLower.Contains(iFilterName.ToLower) Then
                                PluginPackage.WriteVSCode(script, code, i)
                            End If
                        Next
                    End If
                Else
                    If Not i.AviSynthFilterNames Is Nothing Then
                        For Each i2 In i.AviSynthFilterNames
                            If scriptLower.Contains(i2.ToLower + "(") Then
                                If i.Filename.Ext = "avsi" Then
                                    Dim load = "Import(""" + fp + """)" + CrLf

                                    If Not scriptLower.Contains(load.ToLower) AndAlso Not code.Contains(load) Then
                                        code += load
                                    End If

                                    If Not i.Dependencies.ContainsNothingOrEmpty Then
                                        For Each i3 In i.Dependencies
                                            For Each i4 In plugins.Where(Function(arg) Not arg.AviSynthFilterNames.ContainsNothingOrEmpty)
                                                If i3 = i4.Name Then
                                                    load = "LoadPlugin(""" + i4.GetPath + """)" + CrLf

                                                    If Not scriptLower.Contains(load.ToLower) AndAlso Not code.Contains(load) Then
                                                        code += load
                                                    End If
                                                End If
                                            Next
                                        Next
                                    End If
                                Else
                                    Dim load = "LoadPlugin(""" + fp + """)" + CrLf

                                    If Not scriptLower.Contains(load.ToLower) AndAlso Not code.Contains(load) Then
                                        code += load
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        Next

        Dim clip As String

        If code <> "" Then
            clip = code + script
        Else
            clip = script
        End If

        If engine = ScriptingEngine.VapourSynth AndAlso Not clip.Contains(".set_output()") Then
            If clip.EndsWith(CrLf) Then
                clip += "clip.set_output()"
            Else
                clip += CrLf + "clip.set_output()"
            End If
        End If

        Return clip
    End Function

    Function GetFramerate() As Double
        Synchronize()
        Return Framerate
    End Function

    Function GetErrorMessage() As String
        Synchronize()
        Return ErrorMessage
    End Function

    Function GetSeconds() As Integer
        Dim fr = GetFramerate()

        If fr = 0 Then fr = p.SourceFrameRate
        If fr = 0 Then fr = 25

        Return CInt(GetFrames() / fr)
    End Function

    Function GetFrames() As Integer
        Synchronize()
        Return Frames
    End Function

    Function GetSize() As Size
        Synchronize()
        Return Size
    End Function

    Shared Function GetDefaults() As List(Of TargetVideoScript)
        Dim ret As New List(Of TargetVideoScript)

        Dim script As New TargetVideoScript("AviSynth")
        script.Engine = ScriptingEngine.AviSynth
        script.Filters.Add(New VideoFilter("Source", "FFVideoSource", "FFVideoSource(""%source_file%"", cachefile = ""%temp_file%.ffindex"")"))
        script.Filters.Add(New VideoFilter("Crop", "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)", False))
        script.Filters.Add(New VideoFilter("Field", "TDeint", "TDeint()", False))
        script.Filters.Add(New VideoFilter("Noise", "RemoveGrain", "RemoveGrain()", False))
        script.Filters.Add(New VideoFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)", False))
        ret.Add(script)

        script = New TargetVideoScript("VapourSynth")
        script.Engine = ScriptingEngine.VapourSynth
        script.Filters.Add(New VideoFilter("Source", "ffms2", "clip = core.ffms2.Source(r""%source_file%"", cachefile = r""%temp_file%.ffindex"")"))
        script.Filters.Add(New VideoFilter("Crop", "CropRel", "clip = core.std.CropRel(clip, %crop_left%, %crop_right%, %crop_top%, %crop_bottom%)", False))
        script.Filters.Add(New VideoFilter("Field", "QTGMC Medium", "clip = havsfunc.QTGMC(clip, TFF = True, Preset = 'Medium')", False))
        script.Filters.Add(New VideoFilter("Noise", "SMDegrain", "clip = havsfunc.SMDegrain(clip, contrasharp = True)", False))
        script.Filters.Add(New VideoFilter("Resize", "Bicubic", "clip = core.resize.Bicubic(clip, %target_width%, %target_height%)", False))
        ret.Add(script)

        Return ret
    End Function

    Overrides Function Edit() As DialogResult
        Using f As New ScriptingEditor(Me)
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

    Private Function GetDocument() As VideoScript
        Return Me
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
            If p.SourceFile = "" OrElse p.Name = "" Then Return ""

            Return p.TempDir + p.Name + "." + FileType
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
            If p.SourceFile = "" Then Return ""
            Return p.TempDir + p.Name + "_Source." + p.Script.FileType
        End Get
        Set(value As String)
        End Set
    End Property

    Public Overrides Property Engine As ScriptingEngine
        Get
            Return p.Script.Engine
        End Get
        Set(value As ScriptingEngine)
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
End Class

<Serializable()>
Class FilterCategory
    Sub New(name As String)
        Me.Name = name
    End Sub

    Property Name As String

    Private FitersValue As New List(Of VideoFilter)

    ReadOnly Property Filters() As List(Of VideoFilter)
        Get
            If FitersValue Is Nothing Then
                FitersValue = New List(Of VideoFilter)
            End If

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

    Shared Sub AddDefaults(engine As ScriptingEngine, list As List(Of FilterCategory))
        For Each i In Packs.Packages.OfType(Of PluginPackage)
            Dim filters As VideoFilter() = Nothing

            If engine = ScriptingEngine.AviSynth Then
                If Not i.AviSynthFiltersFunc Is Nothing Then filters = i.AviSynthFiltersFunc.Invoke
            Else
                If Not i.VapourSynthFiltersFunc Is Nothing Then filters = i.VapourSynthFiltersFunc.Invoke
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
             New VideoFilter("Source", "Automatic", "# can be configured at main menu > Tools > Settings > Source Filters"),
             New VideoFilter("Source", "AviSource", "AviSource(""%source_file%"", Audio = False)"),
             New VideoFilter("Source", "DirectShowSource", "DirectShowSource(""%source_file%"", audio = False)"),
             New VideoFilter("Source", "DSS2", "DSS2(""%source_file%"")"),
             New VideoFilter("Source", "FFVideoSource", "FFVideoSource(""%source_file%"", cachefile = ""%temp_file%.ffindex"")"),
             New VideoFilter("Source", "LSMASHVideoSource", "LSMASHVideoSource(""%source_file%"", format = ""YUV420P8"")"),
             New VideoFilter("Source", "LWLibavVideoSource", "LWLibavVideoSource(""%source_file%"", format = ""YUV420P8"")")})
        ret.Add(src)

        Dim misc As New FilterCategory("Misc")
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS MediaInfo", "AssumeFPS(%media_info_video:FrameRate%)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS(24000, 1001)", "AssumeFPS(24000, 1001)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS(30000, 1001)", "AssumeFPS(30000, 1001)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS(60000, 1001)", "AssumeFPS(60000, 1001)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS(24)", "AssumeFPS(24)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS(25)", "AssumeFPS(25)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS(50)", "AssumeFPS(50)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "Prefetch(4)", "Prefetch(4)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "checkmate", "checkmate()"))
        misc.Filters.Add(New VideoFilter(misc.Name, "Clense", "Clense()"))
        misc.Filters.Add(New VideoFilter(misc.Name, "f3kdb", "f3kdb()"))
        misc.Filters.Add(New VideoFilter(misc.Name, "UnDot", "UnDot()"))
        misc.Filters.Add(New VideoFilter(misc.Name, "Tweak", "Tweak(hue = 0, sat = 1, bright = 0, cont = 1, coring = true)"))
        ret.Add(misc)

        Dim field As New FilterCategory("Field")
        field.Filters.Add(New VideoFilter(field.Name, "IVTC", "Telecide(guide = 1).Decimate()"))
        field.Filters.Add(New VideoFilter(field.Name, "TDeint", "TDeint()"))
        field.Filters.Add(New VideoFilter(field.Name, "FieldDeinterlace", "FieldDeinterlace()"))
        field.Filters.Add(New VideoFilter(field.Name, "SangNom2", "SangNom2()"))
        field.Filters.Add(New VideoFilter(field.Name, "vinverse2", "vinverse2()"))
        field.Filters.Add(New VideoFilter(field.Name, "SelectEven", "SelectEven()"))
        field.Filters.Add(New VideoFilter(field.Name, "SelectOdd", "SelectOdd()"))
        ret.Add(field)

        Dim resize As New FilterCategory("Resize")
        resize.Filters.Add(New VideoFilter(resize.Name, "BilinearResize", "BilinearResize(%target_width%, %target_height%)"))
        resize.Filters.Add(New VideoFilter(resize.Name, "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)"))
        resize.Filters.Add(New VideoFilter(resize.Name, "LanczosResize", "LanczosResize(%target_width%, %target_height%)"))
        resize.Filters.Add(New VideoFilter(resize.Name, "Lanczos4Resize", "Lanczos4Resize(%target_width%, %target_height%)"))
        resize.Filters.Add(New VideoFilter(resize.Name, "BlackmanResize", "BlackmanResize(%target_width%, %target_height%)"))
        resize.Filters.Add(New VideoFilter(resize.Name, "GaussResize", "GaussResize(%target_width%, %target_height%)"))
        resize.Filters.Add(New VideoFilter(resize.Name, "SincResize", "SincResize(%target_width%, %target_height%)"))
        resize.Filters.Add(New VideoFilter(resize.Name, "PointResize", "PointResize(%target_width%, %target_height%)"))
        resize.Filters.Add(New VideoFilter(resize.Name, "Hardware Encoder", "# hardware encoder resizes"))
        resize.Filters.Add(New VideoFilter(resize.Name, "Spline | Spline16Resize", "Spline16Resize(%target_width%, %target_height%)"))
        resize.Filters.Add(New VideoFilter(resize.Name, "Spline | Spline36Resize", "Spline36Resize(%target_width%, %target_height%)"))
        resize.Filters.Add(New VideoFilter(resize.Name, "Spline | Spline64Resize", "Spline64Resize(%target_width%, %target_height%)"))
        ret.Add(resize)

        Dim crop As New FilterCategory("Crop")
        crop.Filters.Add(New VideoFilter(crop.Name, "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)"))
        crop.Filters.Add(New VideoFilter(crop.Name, "Hardware Encoder", "# hardware encoder crops"))
        ret.Add(crop)

        FilterCategory.AddDefaults(ScriptingEngine.AviSynth, ret)

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
             New VideoFilter("Source", "Automatic", "# can be configured at main menu > Tools > Settings > Source Filters"),
             New VideoFilter("Source", "AVISource", "clip = core.avisource.AVISource(r""%source_file%"")"),
             New VideoFilter("Source", "ffms2", "clip = core.ffms2.Source(r""%source_file%"", cachefile = r""%temp_file%.ffindex"")"),
             New VideoFilter("Source", "LibavSMASHSource", "clip = core.lsmas.LibavSMASHSource(r""%source_file%"")"),
             New VideoFilter("Source", "LWLibavSource", "clip = core.lsmas.LWLibavSource(r""%source_file%"")")})
        ret.Add(src)

        Dim crop As New FilterCategory("Crop")
        crop.Filters.AddRange(
            {New VideoFilter("Crop", "CropRel", "clip = core.std.CropRel(clip, %crop_left%, %crop_right%, %crop_top%, %crop_bottom%)", False)})
        ret.Add(crop)

        Dim resize As New FilterCategory("Resize")
        resize.Filters.AddRange(
            {New VideoFilter("Resize", "Bilinear", "clip = core.resize.Bilinear(clip, %target_width%, %target_height%)"),
             New VideoFilter("Resize", "Bicubic", "clip = core.resize.Bicubic(clip, %target_width%, %target_height%)"),
             New VideoFilter("Resize", "Point", "clip = core.resize.Point(clip, %target_width%, %target_height%)"),
             New VideoFilter("Resize", "Gauss", "clip = core.resize.Gauss(clip, %target_width%, %target_height%)"),
             New VideoFilter("Resize", "Sinc", "clip = core.resize.Sinc(clip, %target_width%, %target_height%)"),
             New VideoFilter("Resize", "Lanczos", "clip = core.resize.Lanczos(clip, %target_width%, %target_height%)"),
             New VideoFilter("Resize", "Spline", "clip = core.resize.Spline(clip, %target_width%, %target_height%)")})
        ret.Add(resize)

        Dim field As New FilterCategory("Field")
        field.Filters.Add(New VideoFilter(field.Name, "IVTC", "clip = core.vivtc.VFM(clip, 1)" + CrLf + "clip = core.vivtc.VDecimate(clip)"))
        field.Filters.Add(New VideoFilter(field.Name, "Vinverse", "clip = core.vinverse.Vinverse(clip)"))
        field.Filters.Add(New VideoFilter(field.Name, "Select Even", "clip = clip[::2]"))
        field.Filters.Add(New VideoFilter(field.Name, "Select Odd", "clip = clip[1::2]"))
        field.Filters.Add(New VideoFilter(field.Name, "Set Frame Based", "clip = core.std.SetFieldBased(clip, 0)"))
        field.Filters.Add(New VideoFilter(field.Name, "Set Bottom Field First", "clip = core.std.SetFieldBased(clip, 1)"))
        field.Filters.Add(New VideoFilter(field.Name, "Set Top Field First", "clip = core.std.SetFieldBased(clip, 2)"))

        ret.Add(field)

        Dim noise As New FilterCategory("Noise")
        noise.Filters.Add(New VideoFilter(noise.Name, "SMDegrain", "clip = havsfunc.SMDegrain(clip, contrasharp = True)"))
        noise.Filters.Add(New VideoFilter(noise.Name, "RemoveGrain", "clip = core.rgvs.RemoveGrain(clip, 1)"))
        ret.Add(noise)

        Dim misc As New FilterCategory("Misc")
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS MediaInfo", "clip = core.std.AssumeFPS(clip, fpsnum = int(%media_info_video:FrameRate% * 1000), fpsden = 1000)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS 24000/1001", "clip = core.std.AssumeFPS(clip, fpsnum = 24000, fpsden = 1001)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS 30000/1001", "clip = core.std.AssumeFPS(clip, fpsnum = 30000, fpsden = 1001)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS 60000/1001", "clip = core.std.AssumeFPS(clip, fpsnum = 60000, fpsden = 1001)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS 24", "clip = core.std.AssumeFPS(clip, fpsnum = 24, fpsden = 1)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS 25", "clip = core.std.AssumeFPS(clip, fpsnum = 25, fpsden = 1)"))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS 50", "clip = core.std.AssumeFPS(clip, fpsnum = 50, fpsden = 1)"))
        ret.Add(misc)

        FilterCategory.AddDefaults(ScriptingEngine.VapourSynth, ret)

        For Each i In ret
            i.Filters.Sort()
        Next

        Return ret
    End Function
End Class

Class FilterParameter
    Property Name As String
    Property Value As String

    Sub New(name As String, value As String)
        Me.Name = name
        Me.Value = value
    End Sub
End Class

Class FilterParameters
    Property FunctionName As String
    Property Text As String

    Property Parameters As New List(Of FilterParameter)

    Shared DefinitionsValue As List(Of FilterParameters)

    Shared ReadOnly Property Definitions As List(Of FilterParameters)
        Get
            If DefinitionsValue Is Nothing Then
                DefinitionsValue = New List(Of FilterParameters)

                Dim add = Function(func As String, text As String) As FilterParameters
                              Dim ret As New FilterParameters
                              ret.FunctionName = func
                              ret.Text = text
                              DefinitionsValue.Add(ret)
                              Return ret
                          End Function

                Dim add2 = Function(func As String,
                                    param As String,
                                    value As String,
                                    text As String) As FilterParameters

                               Dim ret As New FilterParameters
                               ret.FunctionName = func
                               ret.Text = text
                               DefinitionsValue.Add(ret)
                               ret.Parameters.Add(New FilterParameter(param, value))
                               Return ret
                           End Function

                Dim item As FilterParameters

                item = add("DGSource", "Hardware Resizing")
                item.Parameters.Add(New FilterParameter("resize_w", "%target_width%"))
                item.Parameters.Add(New FilterParameter("resize_h", "%target_height%"))

                item = add("DGSource", "Hardware Cropping")
                item.Parameters.Add(New FilterParameter("crop_l", "%crop_left%"))
                item.Parameters.Add(New FilterParameter("crop_t", "%crop_top%"))
                item.Parameters.Add(New FilterParameter("crop_r", "%crop_right%"))
                item.Parameters.Add(New FilterParameter("crop_b", "%crop_bottom%"))

                add2("FFVideoSource", "rffmode", "0", "rffmode | rffmode = 0 (ignore all flags (default))")
                add2("FFVideoSource", "rffmode", "1", "rffmode | rffmode = 1 (honor all pulldown flags)")
                add2("FFVideoSource", "rffmode", "2", "rffmode | rffmode = 2 (force film)")

                add2("ffms2.Source", "rffmode", "0", "rffmode | rffmode = 0 (ignore all flags (default))")
                add2("ffms2.Source", "rffmode", "1", "rffmode | rffmode = 1 (honor all pulldown flags)")
                add2("ffms2.Source", "rffmode", "2", "rffmode | rffmode = 2 (force film)")

                add2("DGSource", "deinterlace", "0", "deinterlace | deinterlace = 0 (no deinterlacing)")
                add2("DGSource", "deinterlace", "1", "deinterlace | deinterlace = 1 (single rate deinterlacing)")
                add2("DGSource", "deinterlace", "2", "deinterlace | deinterlace = 2 (double rate deinterlacing)")

                add2("havsfunc.QTGMC", "TFF", "True", "TFF | TFF = True (top field first)")
                add2("havsfunc.QTGMC", "TFF", "False", "TFF | TFF = False (bottom field first)")

                add2("QTGMC", "Preset", """Draft""", "Preset | Preset = ""Draft""")
                add2("QTGMC", "Preset", """Ultra Fast""", "Preset | Preset = ""Ultra Fast""")
                add2("QTGMC", "Preset", """Super Fast""", "Preset | Preset = ""Super Fast""")
                add2("QTGMC", "Preset", """Very Fast""", "Preset | Preset = ""Very Fast""")
                add2("QTGMC", "Preset", """Faster""", "Preset | Preset = ""Faster""")
                add2("QTGMC", "Preset", """Fast""", "Preset | Preset = ""Fast""")
                add2("QTGMC", "Preset", """Medium""", "Preset | Preset = ""Medium""")
                add2("QTGMC", "Preset", """Slow""", "Preset | Preset = ""Slow""")
                add2("QTGMC", "Preset", """Slower""", "Preset | Preset = ""Slower""")
                add2("QTGMC", "Preset", """Very Slow""", "Preset | Preset = ""Very Slow""")
                add2("QTGMC", "Preset", """Placebo""", "Preset | Preset = ""Placebo""")
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

Public Enum ScriptingEngine
    AviSynth
    VapourSynth
End Enum