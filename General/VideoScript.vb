Imports System.Text

<Serializable()>
Class VideoScript
    Inherits Profile

    <NonSerialized()> Private Framerate As Double
    <NonSerialized()> Private Frames As Integer
    <NonSerialized()> Private Size As Size
    <NonSerialized()> Private ErrorMessage As String

    <NonSerialized()> Public LastSync As String

    Property Filters As New List(Of VideoFilter)
    Property Engine As ScriptingEngine = ScriptingEngine.AviSynth

    Sub New()
        Me.New(Nothing)
    End Sub

    Sub New(name As String)
        MyBase.New(name)
    End Sub

    Private PathValue As String = ""

    Overridable Property Path() As String
        Get
            Return PathValue
        End Get
        Set(Value As String)
            PathValue = Value
        End Set
    End Property

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

    Sub Remove(category As String, Optional name As String = Nothing)
        For Each i In Filters.ToArray
            If i.Category = category AndAlso (name Is Nothing OrElse i.Path = name) Then
                Filters.Remove(i)
            End If
        Next
    End Sub

    Sub SetFilter(category As String, name As String, script As String)
        For Each i In Filters
            If i.Category = category Then
                i.Path = name
                i.Script = script
                i.Active = True
                Exit For
            End If
        Next
    End Sub

    Sub InsertAfter(category As String, af As VideoFilter)
        Dim f = GetFilter(category)
        Filters.Insert(Filters.IndexOf(f) + 1, af)
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
                    If script.Contains(".set_output()") Then
                        script = script.Replace(".set_output()", ".resize.Bicubic(format=vs.COMPATBGR32).set_output()")
                    Else
                        script += CrLf + "clip = clip.resize.Bicubic(format=vs.COMPATBGR32)"
                    End If
                End If
            End If

            Dim current = Path + script

            If Frames = 240 OrElse current <> LastSync Then
                If Directory.Exists(Filepath.GetDir(Path)) Then
                    script = ModifyScript(script)
                    script.WriteFile(Path)

                    If g.MainForm.Visible Then
                        g.MainForm.Indexing()
                        ProcessForm.CloseProcessForm()
                    Else
                        g.MainForm.Indexing()
                    End If

                    Using avi As New AVIFile(Path)
                        Framerate = avi.FrameRate
                        Frames = avi.FrameCount
                        Size = avi.FrameSize
                        ErrorMessage = avi.ErrorMessage
                    End Using

                    LastSync = current
                End If
            End If
        End If
    End Sub

    Shared Function ModifyScript(script As String) As String
        Dim scriptLower = script.ToLower

        Dim code = ""

        Dim vs = scriptLower.Contains("clip = core.")

        If vs AndAlso Not scriptLower.Contains("import vapoursynth") Then
            code = "import vapoursynth as vs" + CrLf + "core = vs.get_core()" + CrLf
        End If

        Dim plugins = Packs.Packages.OfType(Of PluginPackage)()

        For Each i In plugins
            Dim fp = i.GetPath

            If fp <> "" Then
                If vs Then
                    If Not i.VapourSynthFilterNames Is Nothing Then
                        For Each iFilterName In i.VapourSynthFilterNames
                            If scriptLower.Contains(iFilterName.ToLower) Then
                                If i.Filename.Ext = "py" Then
                                    code += "import importlib.machinery" + CrLf + Filepath.GetBase(i.Filename) +
                                        " = importlib.machinery.SourceFileLoader('" + Filepath.GetBase(i.Filename) +
                                        "', r'" + i.GetPath + "').load_module()" + CrLf

                                    If OK(i.Dependencies) Then
                                        For Each i3 In i.Dependencies
                                            For Each i4 In plugins
                                                If i3 = i4.Name AndAlso Not i4.VapourSynthFilterNames Is Nothing Then
                                                    Dim load = "core.std.LoadPlugin(r'" + i4.GetPath + "')" + CrLf

                                                    If Not scriptLower.Contains(load.ToLower) AndAlso Not code.Contains(load) Then
                                                        code += load
                                                    End If
                                                End If
                                            Next
                                        Next
                                    End If
                                Else
                                    code += "core.std.LoadPlugin(r'" + fp + "')" + CrLf
                                End If
                            End If
                        Next
                    End If
                Else
                    If Not i.AviSynthFilterNames Is Nothing Then
                        For Each i2 In i.AviSynthFilterNames
                            If scriptLower.Contains(i2.ToLower + "(") Then
                                If i.Filename.Ext = ".avsi" Then
                                    Dim load = "Import(""" + fp + """)" + CrLf

                                    If Not scriptLower.Contains(load.ToLower) AndAlso Not code.Contains(load) Then
                                        code += load
                                    End If

                                    If OK(i.Dependencies) Then
                                        For Each i3 In i.Dependencies
                                            For Each i4 In plugins
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

        If vs AndAlso Not clip.Contains(".set_output()") Then
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
        script.Filters.Add(New VideoFilter("Source", "FFVideoSource", "FFVideoSource(""%source_file%"", cachefile = ""%temp_file%.ffindex"")", True))
        script.Filters.Add(New VideoFilter("Crop", "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)", False))
        script.Filters.Add(New VideoFilter("Field", "TDeint", "TDeint()", False))
        script.Filters.Add(New VideoFilter("Misc", "RemoveGrain", "RemoveGrain()", False))
        script.Filters.Add(New VideoFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)", False))
        ret.Add(script)

        script = New TargetVideoScript("VapourSynth")
        script.Engine = ScriptingEngine.VapourSynth
        script.Filters.Add(New VideoFilter("Source", "ffms2", "clip = core.ffms2.Source(source = r'%source_file%', cachefile = r'%temp_file%.ffindex')", True))
        script.Filters.Add(New VideoFilter("Crop", "CropAbs", "cropwidth = clip.width - %crop_left% - %crop_right%" + CrLf + "cropheight = clip.height - %crop_top% - %crop_bottom%" + CrLf + "clip = core.std.CropAbs(clip, cropwidth, cropheight, %crop_left%, %crop_top%)", False))
        script.Filters.Add(New VideoFilter("Field", "QTGMC Medium", "clip = havsfunc.QTGMC(Input = clip, TFF = True, Preset = 'Medium')", False))
        script.Filters.Add(New VideoFilter("Noise", "SMDegrain", "clip = havsfunc.SMDegrain(input = clip, contrasharp = True)", False))
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
Class TargetVideoScript
    Inherits VideoScript

    Sub New(name As String)
        MyBase.New(name)
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
Class SourceVideoScript
    Inherits VideoScript

    Sub New()
        Me.New(Nothing)
    End Sub

    Sub New(name As String)
        MyBase.New(name)
    End Sub

    Overrides Property Path() As String
        Get
            If p.SourceFile = "" Then Return ""
            Return p.TempDir + p.Name + "_Source." + p.Script.FileType
        End Get
        Set(value As String)
        End Set
    End Property

    Overrides Function GetScript() As String
        Return p.Script.Filters(0).Script
    End Function
End Class

<Serializable()>
Public Class VideoFilter

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
            Optional active As Boolean = False)

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
            If FitersValue Is Nothing Then
                FitersValue = New List(Of VideoFilter)
            End If

            Return FitersValue
        End Get
    End Property

    Overrides Function ToString() As String
        Return Name
    End Function

    Shared Function GetAviSynthDefaults() As List(Of FilterCategory)
        Dim ret As New List(Of FilterCategory)

        Dim src As New FilterCategory("Source")
        src.Filters.AddRange(
            {New VideoFilter("Source", "Automatic", "", True),
             New VideoFilter("Source", "AviSource", "AviSource(""%source_file%"", Audio = False)", True),
             New VideoFilter("Source", "DirectShowSource", "DirectShowSource(""%source_file%"", audio = False)", True),
             New VideoFilter("Source", "DSS2", "DSS2(""%source_file%"")", True),
             New VideoFilter("Source", "FFVideoSource", "FFVideoSource(""%source_file%"", cachefile = ""%temp_file%.ffindex"")", True),
             New VideoFilter("Source", "LSMASHVideoSource", "LSMASHVideoSource(""%source_file%"")", True),
             New VideoFilter("Source", "LWLibavVideoSource", "LWLibavVideoSource(""%source_file%"")", True),
             New VideoFilter("Source", "DGSource", "DGSource(""%source_file%"")", True),
             New VideoFilter("Source", "DGSourceIM", "DGSourceIM(""%source_file%"")", True)})
        ret.Add(src)

        Dim misc As New FilterCategory("Misc")
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS(24000, 1001)", "AssumeFPS(24000, 1001)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS(30000, 1001)", "AssumeFPS(30000, 1001)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS(60000, 1001)", "AssumeFPS(60000, 1001)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS(24)", "AssumeFPS(24)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS(25)", "AssumeFPS(25)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS(50)", "AssumeFPS(50)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "SelectEven", "SelectEven()", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "SelectOdd", "SelectOdd()", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "Prefetch(4) ", "Prefetch(4) ", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "checkmate", "checkmate()", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "Clense", "Clense()", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "f3kdb", "f3kdb()", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "RemoveGrain", "RemoveGrain()", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "UnDot", "UnDot()", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "KNLMeansCL | Spatial Light", "KNLMeansCL(D = 0, A = 2, h = 2)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "KNLMeansCL | Spatial Medium", "KNLMeansCL(D = 0, A = 4, h = 4)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "KNLMeansCL | Spatial Strong", "KNLMeansCL(D = 0, A = 6, h = 6)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "KNLMeansCL | Temporal Light", "KNLMeansCL(D = 1, A = 0, h = 3)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "KNLMeansCL | Temporal Medium", "KNLMeansCL(D = 1, A = 0, h = 6)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "KNLMeansCL | Temporal Strong", "KNLMeansCL(D = 1, A = 0, h = 9)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "KNLMeansCL | Spatio-Temporal Light", "KNLMeansCL(D = 1, A = 1, h = 2)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "KNLMeansCL | Spatio-Temporal Medium", "KNLMeansCL(D = 1, A = 1, h = 4)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "KNLMeansCL | Spatio-Temporal Strong", "KNLMeansCL(D = 1, A = 1, h = 8)", True))
        ret.Add(misc)

        Dim field As New FilterCategory("Field")
        field.Filters.Add(New VideoFilter(field.Name, "IVTC", "Telecide(guide=1).Decimate()", True))
        field.Filters.Add(New VideoFilter(field.Name, "TDeint", "TDeint()", True))
        field.Filters.Add(New VideoFilter(field.Name, "FieldDeinterlace", "FieldDeinterlace()", True))
        field.Filters.Add(New VideoFilter(field.Name, "SangNom2", "SangNom2()", True))
        field.Filters.Add(New VideoFilter(field.Name, "nnedi3", "nnedi3()", True))
        field.Filters.Add(New VideoFilter(field.Name, "vinverse2", "vinverse2()", True))
        ret.Add(field)

        Dim resize As New FilterCategory("Resize")
        resize.Filters.Add(New VideoFilter(resize.Name, "BilinearResize", "BilinearResize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New VideoFilter(resize.Name, "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)", True))
        resize.Filters.Add(New VideoFilter(resize.Name, "LanczosResize", "LanczosResize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New VideoFilter(resize.Name, "Lanczos4Resize", "Lanczos4Resize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New VideoFilter(resize.Name, "BlackmanResize", "BlackmanResize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New VideoFilter(resize.Name, "GaussResize", "GaussResize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New VideoFilter(resize.Name, "SincResize", "SincResize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New VideoFilter(resize.Name, "PointResize", "PointResize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New VideoFilter(resize.Name, "Hardware Encoder", "# hardware encoder resizes", True))
        resize.Filters.Add(New VideoFilter(resize.Name, "Spline | Spline16Resize", "Spline16Resize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New VideoFilter(resize.Name, "Spline | Spline36Resize", "Spline36Resize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New VideoFilter(resize.Name, "Spline | Spline64Resize", "Spline64Resize(%target_width%, %target_height%)", True))
        ret.Add(resize)

        Dim crop As New FilterCategory("Crop")
        crop.Filters.Add(New VideoFilter(crop.Name, "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)", True))
        crop.Filters.Add(New VideoFilter(crop.Name, "Hardware Encoder", "# hardware encoder crops", True))
        ret.Add(crop)

        Return ret
    End Function

    Shared Function GetVapourSynthDefaults() As List(Of FilterCategory)
        Dim ret As New List(Of FilterCategory)

        Dim src As New FilterCategory("Source")
        src.Filters.AddRange(
            {New VideoFilter("Source", "Automatic", "", True),
             New VideoFilter("Source", "ffms2", "clip = core.ffms2.Source(source = r'%source_file%', cachefile = r'%temp_file%.ffindex')", True),
             New VideoFilter("Source", "LibavSMASHSource", "clip = core.lsmas.LibavSMASHSource(source = r'%source_file%')", True),
             New VideoFilter("Source", "LWLibavSource", "clip = core.lsmas.LWLibavSource(source = r'%source_file%')", True)})
        ret.Add(src)

        Dim crop As New FilterCategory("Crop")
        crop.Filters.AddRange(
            {New VideoFilter("Crop", "CropAbs", "cropwidth = clip.width - %crop_left% - %crop_right%" + CrLf + "cropheight = clip.height - %crop_top% - %crop_bottom%" + CrLf + "clip = core.std.CropAbs(clip, cropwidth, cropheight, %crop_left%, %crop_top%)", False)})
        ret.Add(crop)

        Dim resize As New FilterCategory("Resize")
        resize.Filters.AddRange(
            {New VideoFilter("Resize", "Bilinear", "clip = core.resize.Bilinear(clip, %target_width%, %target_height%)", True),
             New VideoFilter("Resize", "Bicubic", "clip = core.resize.Bicubic(clip, %target_width%, %target_height%)", True),
             New VideoFilter("Resize", "Point", "clip = core.resize.Point(clip, %target_width%, %target_height%)", True),
             New VideoFilter("Resize", "Gauss", "clip = core.resize.Gauss(clip, %target_width%, %target_height%)", True),
             New VideoFilter("Resize", "Sinc", "clip = core.resize.Sinc(clip, %target_width%, %target_height%)", True),
             New VideoFilter("Resize", "Lanczos", "clip = core.resize.Lanczos(clip, %target_width%, %target_height%)", True),
             New VideoFilter("Resize", "Spline", "clip = core.resize.Spline(clip, %target_width%, %target_height%)", True)})
        ret.Add(resize)

        Dim field As New FilterCategory("Field")
        field.Filters.Add(New VideoFilter(field.Name, "QTGMC | QTGMC Fast", "clip = havsfunc.QTGMC(Input = clip, TFF = True, Preset = 'Fast')", True))
        field.Filters.Add(New VideoFilter(field.Name, "QTGMC | QTGMC Medium", "clip = havsfunc.QTGMC(Input = clip, TFF = True, Preset = 'Medium')", True))
        field.Filters.Add(New VideoFilter(field.Name, "QTGMC | QTGMC Slow", "clip = havsfunc.QTGMC(Input = clip, TFF = True, Preset = 'Slow')", True))
        field.Filters.Add(New VideoFilter(field.Name, "nnedi3", "clip = core.nnedi3.nnedi3(clip = clip, field = 1)", True))
        field.Filters.Add(New VideoFilter(field.Name, "IVTC", "clip = core.vivtc.VFM(clip, 1)" + CrLf + "clip = core.vivtc.VDecimate(clip)", True))
        field.Filters.Add(New VideoFilter(field.Name, "Vinverse", "clip = core.vinverse.Vinverse(clip)", True))
        ret.Add(field)

        Dim noise As New FilterCategory("Noise")
        noise.Filters.Add(New VideoFilter(noise.Name, "SMDegrain", "clip = havsfunc.SMDegrain(input = clip, contrasharp = True)", True))
        noise.Filters.Add(New VideoFilter(noise.Name, "RemoveGrain", "clip = core.rgvs.RemoveGrain(clip, 1)", True))
        noise.Filters.Add(New VideoFilter(noise.Name, "KNLMeansCL | Spatial Light", "clip = core.knlm.KNLMeansCL(clip = clip, d = 0, a = 2, h = 2)", True))
        noise.Filters.Add(New VideoFilter(noise.Name, "KNLMeansCL | Spatial Medium", "clip = core.knlm.KNLMeansCL(clip = clip, d = 0, a = 4, h = 4)", True))
        noise.Filters.Add(New VideoFilter(noise.Name, "KNLMeansCL | Spatial Strong", "clip = core.knlm.KNLMeansCL(clip = clip, d = 0, a = 6, h = 6)", True))
        noise.Filters.Add(New VideoFilter(noise.Name, "KNLMeansCL | Temporal Light", "clip = core.knlm.KNLMeansCL(clip = clip, d = 1, a = 0, h = 3)", True))
        noise.Filters.Add(New VideoFilter(noise.Name, "KNLMeansCL | Temporal Medium", "clip = core.knlm.KNLMeansCL(clip = clip, d = 1, a = 0, h = 6)", True))
        noise.Filters.Add(New VideoFilter(noise.Name, "KNLMeansCL | Temporal Strong", "clip = core.knlm.KNLMeansCL(clip = clip, d = 1, a = 0, h = 9)", True))
        noise.Filters.Add(New VideoFilter(noise.Name, "KNLMeansCL | Spatio-Temporal Light", "clip = core.knlm.KNLMeansCL(clip = clip, d = 1, a = 1, h = 2)", True))
        noise.Filters.Add(New VideoFilter(noise.Name, "KNLMeansCL | Spatio-Temporal Medium", "clip = core.knlm.KNLMeansCL(clip = clip, d = 1, a = 1, h = 4)", True))
        noise.Filters.Add(New VideoFilter(noise.Name, "KNLMeansCL | Spatio-Temporal Strong", "clip = core.knlm.KNLMeansCL(clip = clip, d = 1, a = 1, h = 8)", True))
        ret.Add(noise)

        Dim misc As New FilterCategory("Misc")
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS 24000/1001", "clip = core.std.AssumeFPS(clip = clip, fpsnum = 24000, fpsden = 1001)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS 30000/1001", "clip = core.std.AssumeFPS(clip = clip, fpsnum = 30000, fpsden = 1001)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS 60000/1001", "clip = core.std.AssumeFPS(clip = clip, fpsnum = 60000, fpsden = 1001)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS 24", "clip = core.std.AssumeFPS(clip = clip, fpsnum = 24, fpsden = 1)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS 25", "clip = core.std.AssumeFPS(clip = clip, fpsnum = 25, fpsden = 1)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "AssumeFPS 50", "clip = core.std.AssumeFPS(clip = clip, fpsnum = 50, fpsden = 1)", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "Select Even", "clip = clip[::2]", True))
        misc.Filters.Add(New VideoFilter(misc.Name, "Select Odd", "clip = clip[1::2]", True))
        ret.Add(misc)

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

                Dim add = Function(functionName As String, text As String) As FilterParameters
                              Dim ret As New FilterParameters
                              ret.FunctionName = functionName
                              ret.Text = text
                              DefinitionsValue.Add(ret)
                              Return ret
                          End Function

                Dim item As FilterParameters

                item = add("FFVideoSource", "rffmode = 0 (ignore all flags (default))")
                item.Parameters.Add(New FilterParameter("rffmode", "0"))

                item = add("FFVideoSource", "rffmode = 1 (honor all pulldown flags)")
                item.Parameters.Add(New FilterParameter("rffmode", "1"))

                item = add("FFVideoSource", "rffmode = 2 (force film)")
                item.Parameters.Add(New FilterParameter("rffmode", "2"))

                item = add("DGSource", "deinterlace = 0 (no deinterlacing)")
                item.Parameters.Add(New FilterParameter("deinterlace", "0"))

                item = add("DGSource", "deinterlace = 1 (single rate deinterlacing)")
                item.Parameters.Add(New FilterParameter("deinterlace", "1"))

                item = add("DGSource", "deinterlace = 2 (double rate deinterlacing)")
                item.Parameters.Add(New FilterParameter("deinterlace", "2"))

                item = add("DGSource", "Hardware Resizing")
                item.Parameters.Add(New FilterParameter("resize_w", "%target_width%"))
                item.Parameters.Add(New FilterParameter("resize_h", "%target_height%"))

                item = add("DGSource", "Hardware Cropping")
                item.Parameters.Add(New FilterParameter("crop_l", "%crop_left%"))
                item.Parameters.Add(New FilterParameter("crop_t", "%crop_top%"))
                item.Parameters.Add(New FilterParameter("crop_r", "%crop_right%"))
                item.Parameters.Add(New FilterParameter("crop_b", "%crop_bottom%"))
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

Enum ScriptingEngine
    AviSynth
    VapourSynth
End Enum