Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Text
Imports System.Runtime.Serialization
Imports System.Reflection
Imports System.Threading

Imports StaxRip.UI

Imports Microsoft.Win32

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

    <NonSerialized()>
    Private IsYV12Value As Boolean

    ReadOnly Property IsYV12 As Boolean
        Get
            Synchronize()
            Return IsYV12Value
        End Get
    End Property

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

        If p.AvsCodeAtTop <> "" Then
            sb.AppendLine(p.AvsCodeAtTop)
        End If

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

    Sub ActivateFilter(category As String)
        Dim f = GetFilter(category)
        If Not f Is Nothing Then f.Active = True
    End Sub

    Function IsFilterActive(category As String) As Boolean
        Dim f = GetFilter(category)
        Return Not f Is Nothing AndAlso f.Active
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
            If i.Category = category Then
                Return i
            End If
        Next
    End Function

    Sub Synchronize()
        If Path <> "" Then
            Dim script = Macro.Solve(GetScript())

            Dim current = Path + script

            If Frames = 240 OrElse current <> LastSync Then
                If Directory.Exists(Filepath.GetDir(Path)) Then
                    SetPlugins(script)
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
                        IsYV12Value = avi.IsYV12
                        Size = avi.FrameSize
                        ErrorMessage = avi.ErrorMessage
                    End Using

                    LastSync = current
                End If
            End If
        End If
    End Sub

    Shared Sub SetPlugins(ByRef script As String)
        Dim lscript = script.ToLower

        Dim plugins = ""

        For Each i In Packs.Packages.Values.OfType(Of AviSynthPluginPackage)()
            Dim fp = i.GetPath

            If fp <> "" Then
                For Each i2 In i.FilterNames
                    If lscript.Contains(i2.ToLower + "(") Then
                        Dim v = If(i.IsCPlugin, "LoadC", "Load") + "Plugin(""" + fp + """)" + CrLf

                        If Not lscript.Contains(v.ToLower) AndAlso Not plugins.Contains(v) Then
                            plugins += v
                        End If
                    End If
                Next
            End If
        Next

        If plugins <> "" Then
            script = plugins + script
        End If
    End Sub

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
        doc.Filters.Add(New AviSynthFilter("Field", "Yadif", "Yadif()", False))
        doc.Filters.Add(New AviSynthFilter("Crop", "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)", False))
        doc.Filters.Add(New AviSynthFilter("Noise", "FluxSmooth medium", "FluxSmoothT(4)", False))
        doc.Filters.Add(New AviSynthFilter("Resize", "BicubicResize neutral", "BicubicResize(%target_width%, %target_height%, 0, 0.5)", False))

        ret.Add(doc)

        Return ret
    End Function

    Overrides Function Edit() As DialogResult
        AviSynthListView.EditClick(AddressOf GetDocument)
        Return DialogResult.OK
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
    'Implements ISerializable

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

        'source

        Dim src As New AviSynthCategory("Source")

        src.Filters.AddRange(
            {New AviSynthFilter("Source", "Automatic", "", True),
             New AviSynthFilter("Source", "AviSource", "AviSource(""%source_file%"", audio = false)", True),
             New AviSynthFilter("Source", "MPEG2Source", "MPEG2Source(""%source_file%"")", True),
             New AviSynthFilter("Source", "DirectShowSource", "DirectShowSource(""%source_file%"", audio = false, convertfps = true, fps = %original_framerate%)", True),
             New AviSynthFilter("Source", "FFVideoSource", "FFVideoSource(""%source_file%"", cachefile = ""%working_dir%%source_name%.ffindex"")" + CrLf + "AssumeFPS(%original_framerate%)", True),
             New AviSynthFilter("Source", "LSMASHVideoSource", "LSMASHVideoSource(""%source_file%"")", True),
             New AviSynthFilter("Source", "LWLibavVideoSource", "LWLibavVideoSource(""%source_file%"")" + CrLf + "AssumeFPS(%original_framerate%)", True),
             New AviSynthFilter("Source", "DGSource", "DGSource(""%source_file%"", deinterlace = 0, resize_w = 0, resize_h = 0)", True)})

        ret.Add(src)

        Dim field As New AviSynthCategory("Field")

        field.Filters.Add(New AviSynthFilter(field.Name, "Yadif", "Yadif()", True))
        field.Filters.Add(New AviSynthFilter(field.Name, "IVTC", "Telecide(guide = 1).Decimate()", True))
        field.Filters.Add(New AviSynthFilter(field.Name, "FieldDeinterlace", "FieldDeinterlace()", True))
        field.Filters.Add(New AviSynthFilter(field.Name, "TomsMoComp", "TomsMoComp(-1, 5, 1)", True))
        field.Filters.Add(New AviSynthFilter(field.Name, "SelectEven", "SelectEven()", True))
        field.Filters.Add(New AviSynthFilter(field.Name, "SeparateFields", "SeparateFields()", True))

        ret.Add(field)

        'noise

        Dim noise As New AviSynthCategory("Noise")

        With noise.Filters
            .Add(New AviSynthFilter(noise.Name, "FluxSmooth Low", "FluxSmoothT(2)", True))
            .Add(New AviSynthFilter(noise.Name, "FluxSmooth Medium", "FluxSmoothT(4)", True))
            .Add(New AviSynthFilter(noise.Name, "FluxSmooth Heavy", "FluxSmoothT(8)", True))
            .Add(New AviSynthFilter(noise.Name, "Deen", "Deen()", True))
            .Add(New AviSynthFilter(noise.Name, "UnDot", "UnDot()", True))
        End With

        ret.Add(noise)

        'resize

        Dim resize As New AviSynthCategory("Resize")

        With resize.Filters
            .Add(New AviSynthFilter(resize.Name, "BilinearResize", "BilinearResize(%target_width%, %target_height%)", True))
            .Add(New AviSynthFilter(resize.Name, "BicubicResize", "BicubicResize(%target_width%, %target_height%, 0, 0.5)", True))
            .Add(New AviSynthFilter(resize.Name, "LanczosResize", "LanczosResize(%target_width%, %target_height%)", True))
            .Add(New AviSynthFilter(resize.Name, "Lanczos4Resize", "Lanczos4Resize(%target_width%, %target_height%)", True))
            .Add(New AviSynthFilter(resize.Name, "BlackmanResize", "BlackmanResize(%target_width%, %target_height%)", True))
            .Add(New AviSynthFilter(resize.Name, "GaussResize", "GaussResize(%target_width%, %target_height%)", True))
            .Add(New AviSynthFilter(resize.Name, "SincResize", "SincResize(%target_width%, %target_height%)", True))
            .Add(New AviSynthFilter(resize.Name, "PointResize", "PointResize(%target_width%, %target_height%)", True))
            .Add(New AviSynthFilter(resize.Name, "Empty", "# calculations/macros", True))
            .Add(New AviSynthFilter(resize.Name, "Spline | Spline16Resize", "Spline16Resize(%target_width%, %target_height%)", True))
            .Add(New AviSynthFilter(resize.Name, "Spline | Spline36Resize", "Spline36Resize(%target_width%, %target_height%)", True))
            .Add(New AviSynthFilter(resize.Name, "Spline | Spline64Resize", "Spline64Resize(%target_width%, %target_height%)", True))
        End With

        ret.Add(resize)

        'crop

        Dim crop As New AviSynthCategory("Crop")
        crop.Filters.Add(New AviSynthFilter(crop.Name, "Crop", "Crop(%crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%)", True))
        ret.Add(crop)

        Return ret
    End Function
End Class