Imports System.Text

<Serializable()>
Public Class AviSynthDocument
    Inherits Profile

    <NonSerialized()> Private Framerate As Double
    <NonSerialized()> Private Frames As Integer
    <NonSerialized()> Private Size As Size
    <NonSerialized()> Private ErrorMessage As String

    <NonSerialized()> Public LastSync As String

    Property Filters As New List(Of AviSynthFilter)

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

    Overridable Function GetScript() As String
        Return GetScript(Nothing)
    End Function

    Overridable Function GetScript(skipCategory As String) As String
        Dim sb As New StringBuilder()

        If p.AvsCodeAtTop <> "" Then sb.AppendLine(p.AvsCodeAtTop)
      
        For Each i As AviSynthFilter In Filters
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

    Sub InsertAfter(category As String, af As AviSynthFilter)
        Dim f = GetFilter(category)
        Filters.Insert(Filters.IndexOf(f) + 1, af)
    End Sub

    Function IsFilterActive(category As String) As Boolean
        Dim filter = GetFilter(category)
        Return Not filter Is Nothing AndAlso filter.Active
    End Function

    Function IsFilterActive(category As String, name As String) As Boolean
        Dim filter = GetFilter(category)
        Return Not filter Is Nothing AndAlso filter.Active AndAlso filter.Name = name
    End Function

    Function GetFiltersCopy() As List(Of AviSynthFilter)
        Dim ret = New List(Of AviSynthFilter)

        For Each i In Filters
            ret.Add(i.GetCopy)
        Next

        Return ret
    End Function

    Function GetFilter(category As String) As AviSynthFilter
        For Each i In Filters
            If i.Category = category Then Return i
        Next
    End Function

    Sub Synchronize()
        If Path <> "" Then
            Dim script = Macro.Solve(GetScript())

            Dim current = Path + script

            If Frames = 240 OrElse current <> LastSync Then
                If Directory.Exists(Filepath.GetDir(Path)) Then
                    script = SetPlugins(script)
                    script.WriteFile(Path)

                    If g.MainForm.Visible Then
                        g.MainForm.avsIndexing()
                        ProcessForm.CloseProcessForm()
                    Else
                        g.MainForm.avsIndexing()
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

    Shared Function SetPlugins(script As String) As String
        Dim scriptLower = script.ToLower

        Dim code = ""
        Dim plugins = Packs.Packages.Values.OfType(Of AviSynthPluginPackage)()

        For Each i In plugins
            Dim fp = i.GetPath

            If fp <> "" Then
                For Each i2 In i.FilterNames
                    If scriptLower.Contains(i2.ToLower + "(") Then
                        If i.Filename.Contains(".avsi") Then
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
        Next

        If code <> "" Then
            Return code + script
        Else
            Return script
        End If
    End Function

    Function ContainsIgnoreCase(value As String) As Boolean
        For Each i In Filters
            If i.Active AndAlso i.Script.ToUpper.Contains(value.ToUpper) Then
                Return True
            End If
        Next
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

        If fr = 0 Then fr = p.SourceFramerate
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

    Shared Function GetDefaults() As List(Of TargetAviSynthDocument)
        Dim ret As New List(Of TargetAviSynthDocument)
        Dim doc As New TargetAviSynthDocument("Default Filter Setup")

        doc.Filters.Add(New AviSynthFilter("Source", "Automatic", "", True))
        doc.Filters.Add(New AviSynthFilter("Crop", "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)", False))
        doc.Filters.Add(New AviSynthFilter("Misc", "TDeint", "TDeint()", False))
        doc.Filters.Add(New AviSynthFilter("Misc", "RemoveGrain", "RemoveGrain()", False))
        doc.Filters.Add(New AviSynthFilter("Resize", "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)", False))

        ret.Add(doc)

        Return ret
    End Function

    Overrides Function Edit() As DialogResult
        Using f As New AviSynthEditor(Me)
            f.StartPosition = FormStartPosition.CenterParent

            If f.ShowDialog() = DialogResult.OK Then
                Filters = f.GetFilters
                Return DialogResult.OK
            End If
        End Using

        Return DialogResult.Cancel
    End Function

    Private Function GetDocument() As AviSynthDocument
        Return Me
    End Function
End Class

<Serializable()>
Public Class TargetAviSynthDocument
    Inherits AviSynthDocument

    Sub New(name As String)
        MyBase.New(name)
        CanEditValue = True
    End Sub

    Overrides Property Path() As String
        Get
            If p.SourceFile = "" OrElse p.Name = "" Then
                Return ""
            End If

            Return p.TempDir + p.Name + ".avs"
        End Get
        Set(value As String)
        End Set
    End Property
End Class

<Serializable()>
Public Class SourceAviSynthDocument
    Inherits AviSynthDocument

    Sub New()
        Me.New(Nothing)
    End Sub

    Sub New(name As String)
        MyBase.New(name)
    End Sub

    Overrides Property Path() As String
        Get
            If p.SourceFile = "" Then
                Return ""
            End If

            Return p.TempDir + p.Name + "_Source.avs"
        End Get
        Set(value As String)
        End Set
    End Property

    Overrides Function GetScript() As String
        For Each i In p.AvsDoc.Filters
            If i.Category = "Source" Then
                Return i.Script + CrLf
            End If
        Next
    End Function
End Class

<Serializable()>
Public Class AviSynthFilter

    Property Active As Boolean
    Property Category As String
    Property Path As String
    Property Script As String

    Sub New()
        Me.new("???", "???", "???", True)
    End Sub

    Sub New(code As String)
        Me.new("???", "???", code, True)
    End Sub

    Sub New(category As String,
            name As String,
            script As String,
            active As Boolean)

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

    Function GetCopy() As AviSynthFilter
        Return New AviSynthFilter(Category, Path, Script, Active)
    End Function

    Overrides Function ToString() As String
        Return Path
    End Function
End Class

<Serializable()>
Public Class AviSynthCategory
    Sub New(name As String)
        Me.Name = name
    End Sub

    Property Name As String

    Private FitersValue As New List(Of AviSynthFilter)

    ReadOnly Property Filters() As List(Of AviSynthFilter)
        Get
            If FitersValue Is Nothing Then
                FitersValue = New List(Of AviSynthFilter)
            End If

            Return FitersValue
        End Get
    End Property

    Overrides Function ToString() As String
        Return Name
    End Function

    Shared Function GetDefaults() As List(Of AviSynthCategory)
        Dim ret As New List(Of AviSynthCategory)

        Dim src As New AviSynthCategory("Source")
        src.Filters.AddRange(
            {New AviSynthFilter("Source", "Automatic", "", True),
             New AviSynthFilter("Source", "AviSource", "AviSource(""%source_file%"", audio = false)", True),
             New AviSynthFilter("Source", "DirectShowSource", "DirectShowSource(""%source_file%"", audio = false)", True),
             New AviSynthFilter("Source", "DSS2", "DSS2(""%source_file%"")", True),
             New AviSynthFilter("Source", "FFVideoSource", "FFVideoSource(""%source_file%"", cachefile = ""%temp_file%.ffindex"")", True),
             New AviSynthFilter("Source", "LSMASHVideoSource", "LSMASHVideoSource(""%source_file%"")", True),
             New AviSynthFilter("Source", "LWLibavVideoSource", "LWLibavVideoSource(""%source_file%"")", True),
             New AviSynthFilter("Source", "DGSource", "DGSource(""%source_file%"", deinterlace = 0, resize_w = 0, resize_h = 0)", True),
             New AviSynthFilter("Source", "DGSourceIM", "DGSourceIM(""%source_file%"")", True),
             New AviSynthFilter("Source", "MT | FFVideoSource", "SetFilterMTMode(""DEFAULT_MT_MODE"", 2)" + CrLf + "SetFilterMTMode(""FFVideoSource"", 3)" + CrLf + "FFVideoSource(""%source_file%"", cachefile = ""%temp_file%.ffindex"")", True),
             New AviSynthFilter("Source", "MT | LSMASHVideoSource", "SetFilterMTMode(""DEFAULT_MT_MODE"", 2)" + CrLf + "SetFilterMTMode(""LSMASHVideoSource"", 3)" + CrLf + "LSMASHVideoSource(""%source_file%"")", True),
             New AviSynthFilter("Source", "MT | LWLibavVideoSource", "SetFilterMTMode(""DEFAULT_MT_MODE"", 2)" + CrLf + "SetFilterMTMode(""LWLibavVideoSource"", 3)" + CrLf + "LWLibavVideoSource(""%source_file%"")", True),
             New AviSynthFilter("Source", "MT | DGSource", "SetFilterMTMode(""DEFAULT_MT_MODE"", 2)" + CrLf + "SetFilterMTMode(""DGSource"", 3)" + CrLf + "DGSource(""%source_file%"", deinterlace = 0, resize_w = 0, resize_h = 0)", True),
             New AviSynthFilter("Source", "MT | DGSourceIM", "SetFilterMTMode(""DEFAULT_MT_MODE"", 2)" + CrLf + "SetFilterMTMode(""DGSourceIM"", 3)" + CrLf + "DGSourceIM(""%source_file%"")", True)})
        ret.Add(src)

        Dim misc As New AviSynthCategory("Misc")

        misc.Filters.Add(New AviSynthFilter(misc.Name, "checkmate", "checkmate()", True))
        misc.Filters.Add(New AviSynthFilter(misc.Name, "Clense", "Clense()", True))
        misc.Filters.Add(New AviSynthFilter(misc.Name, "f3kdb", "f3kdb()", True))
        misc.Filters.Add(New AviSynthFilter(misc.Name, "nnedi3", "nnedi3()", True))
        misc.Filters.Add(New AviSynthFilter(misc.Name, "nnedi3", "nnedi3()", True))
        misc.Filters.Add(New AviSynthFilter(misc.Name, "Prefetch(4) ", "Prefetch(4) ", True))
        misc.Filters.Add(New AviSynthFilter(misc.Name, "QTGMC | QTGMC Medium", "QTGMC(Preset=""Medium"")", True))
        misc.Filters.Add(New AviSynthFilter(misc.Name, "QTGMC | QTGMC Slow", "QTGMC(Preset=""Slow"")", True))
        misc.Filters.Add(New AviSynthFilter(misc.Name, "RemoveGrain", "RemoveGrain()", True))
        misc.Filters.Add(New AviSynthFilter(misc.Name, "SangNom2", "SangNom2()", True))
        misc.Filters.Add(New AviSynthFilter(misc.Name, "SelectEven", "SelectEven()", True))
        misc.Filters.Add(New AviSynthFilter(misc.Name, "TDeint", "TDeint()", True))
        misc.Filters.Add(New AviSynthFilter(misc.Name, "UnDot", "UnDot()", True))

        ret.Add(misc)

        Dim resize As New AviSynthCategory("Resize")
        resize.Filters.Add(New AviSynthFilter(resize.Name, "BilinearResize", "BilinearResize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New AviSynthFilter(resize.Name, "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)", True))
        resize.Filters.Add(New AviSynthFilter(resize.Name, "LanczosResize", "LanczosResize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New AviSynthFilter(resize.Name, "Lanczos4Resize", "Lanczos4Resize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New AviSynthFilter(resize.Name, "BlackmanResize", "BlackmanResize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New AviSynthFilter(resize.Name, "GaussResize", "GaussResize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New AviSynthFilter(resize.Name, "SincResize", "SincResize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New AviSynthFilter(resize.Name, "PointResize", "PointResize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New AviSynthFilter(resize.Name, "Hardware Encoder", "# hardware encoder resizes", True))
        resize.Filters.Add(New AviSynthFilter(resize.Name, "Spline | Spline16Resize", "Spline16Resize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New AviSynthFilter(resize.Name, "Spline | Spline36Resize", "Spline36Resize(%target_width%, %target_height%)", True))
        resize.Filters.Add(New AviSynthFilter(resize.Name, "Spline | Spline64Resize", "Spline64Resize(%target_width%, %target_height%)", True))
        ret.Add(resize)

        Dim crop As New AviSynthCategory("Crop")
        crop.Filters.Add(New AviSynthFilter(crop.Name, "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)", True))
        crop.Filters.Add(New AviSynthFilter(crop.Name, "Hardware Encoder", "# hardware encoder crops", True))
        ret.Add(crop)

        Return ret
    End Function
End Class