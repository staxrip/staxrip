Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class VCEEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "AMD | " + Params.Codec.OptionText
        End Get
    End Property

    <NonSerialized>
    Private ParamsValue As EncoderParams

    Property Params As EncoderParams
        Get
            If ParamsValue Is Nothing Then
                ParamsValue = New EncoderParams
                ParamsValue.Init(ParamsStore)
            End If

            Return ParamsValue
        End Get
        Set(value As EncoderParams)
            ParamsValue = value
        End Set
    End Property

    Overrides Sub ShowConfigDialog()
        Dim params1 As New EncoderParams
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        params1.Init(store)

        Using f As New CommandLineForm(params1)
            Dim saveProfileAction = Sub()
                                        Dim enc = ObjectHelp.GetCopy(Of VCEEnc)(Me)
                                        Dim params2 As New EncoderParams
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            f.cms.Items.Add(New ActionMenuItem("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetStdOut(Package.VCEEnc.Path, "--check-features"))))
            f.cms.Items.Add(New ActionMenuItem("Check VCE Support", Sub() MsgInfo(ProcessHelp.GetStdOut(Package.VCEEnc.Path, "--check-hw"))))
            ActionMenuItem.Add(f.cms.Items, "Save Profile...", saveProfileAction, Symbol.Save)

            If f.ShowDialog() = DialogResult.OK Then
                Params = params1
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides ReadOnly Property OutputExt() As String
        Get
            Return Params.Codec.ValueText
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()

        Using proc As New Proc
            proc.Header = "Video encoding"
            proc.Package = Package.VCEEnc
            proc.SkipStrings = {"%]", " frames: "}
            proc.File = "cmd.exe"
            proc.Arguments = "/S /C """ + Params.GetCommandLine(True, True) + """"
            proc.Start()
        End Using

        AfterEncoding()
    End Sub

    Overrides Function GetMenu() As MenuList
        Dim r As New MenuList
        r.Add("Encoder Options", AddressOf ShowConfigDialog)
        r.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return r
    End Function

    Overrides Property QualityMode() As Boolean
        Get
            Return Params.Mode.Value = 0
        End Get
        Set(Value As Boolean)
        End Set
    End Property

    Public Overrides ReadOnly Property CommandLineParams As CommandLineParams
        Get
            Return Params
        End Get
    End Property

    Public Class EncoderParams
        Inherits CommandLineParams

        Sub New()
            Title = "VCEEnc Options"
        End Sub

        Property Mode As New OptionParam With {
            .Name = "Mode",
            .Text = "Mode",
            .Options = {"CQP - Constant QP", "CBR - Constant Bitrate", "VBR - Variable Bitrate"}}

        Property Codec As New OptionParam With {
            .Switch = "--codec",
            .Text = "Codec",
            .Options = {"AMD H.264", "AMD H.265"},
            .Values = {"h264", "hevc"}}

        Property Decoder As New OptionParam With {
            .Text = "Decoder",
            .Options = {"AviSynth/VapourSynth", "VCEEnc (VCE)", "QSVEnc (Intel)", "ffmpeg (Intel)", "ffmpeg (DXVA2)"},
            .Values = {"avs", "vce", "qs", "ffqsv", "ffdxva"}}

        Property QPI As New NumParam With {
            .Text = "QP I",
            .Value = 22,
            .VisibleFunc = Function() Mode.Value = 0,
            .Config = {0, 51}}

        Property QPP As New NumParam With {
            .Text = "QP P",
            .Value = 24,
            .VisibleFunc = Function() Mode.Value = 0,
            .Config = {0, 51}}

        Property QPB As New NumParam With {
            .Text = "QP B",
            .Value = 27,
            .VisibleFunc = Function() Mode.Value = 0,
            .Config = {0, 51}}

        Property Custom As New StringParam With {
            .Text = "Custom",
            .AlwaysOn = True}

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)
                    Add("Basic", Decoder, Mode, Codec,
                        New OptionParam With {.Switch = "--quality", .Text = "Preset", .Options = {"Fast", "Balanced", "Slow"}, .InitValue = 1},
                        New OptionParam With {.Switch = "--profile", .Name = "profile264", .VisibleFunc = Function() Codec.ValueText = "h264", .Text = "Profile", .Options = {"Automatic", "Baseline", "Main", "High"}},
                        New OptionParam With {.Switch = "--profile", .Name = "profile265", .VisibleFunc = Function() Codec.ValueText = "hevc", .Text = "Profile", .Options = {"Main"}},
                        New OptionParam With {.Switch = "--level", .Name = "LevelH264", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "h264", .Options = {"Unrestricted", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                        New OptionParam With {.Switch = "--level", .Name = "LevelH265", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "hevc", .Options = {"Unrestricted", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}},
                        QPI, QPP, QPB)
                    Add("Slice Decision",
                        New NumParam With {.Switch = "--slices", .Text = "Slices", .Init = 1},
                        New NumParam With {.Switch = "--bframes", .Text = "B-Frames", .Config = {0, 16}},
                        New NumParam With {.Switch = "--ref", .Text = "Ref Frames", .Init = 2, .Config = {0, 16}},
                        New NumParam With {.Switch = "--gop-len", .Text = "GOP Length", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--ltr", .Text = "LTR Frames", .Config = {0, Integer.MaxValue, 1}},
                        New BoolParam With {.Switch = "--b-pyramid", .Text = "B-Pyramid"})
                    Add("Rate Control",
                        New NumParam With {.Switch = "--max-bitrate", .Text = "Max Bitrate", .Init = 20000, .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--vbv-bufsize", .Text = "VBV Bufsize", .Config = {0, 1000000}, .Init = 20000},
                        New NumParam With {.Switch = "--qp-min", .Text = "QP Min", .Config = {0, 100}},
                        New NumParam With {.Switch = "--qp-max", .Text = "QP Max", .Config = {0, 100}, .Init = 100},
                        New NumParam With {.Switch = "--b-deltaqp", .Text = "Non-ref Bframe QP Offset"},
                        New NumParam With {.Switch = "--bref-deltaqp", .Text = "Ref Bframe QP Offset"},
                        New BoolParam With {.Switch = "--vbaq", .Text = "VBAQ"})
                    Add("VUI",
                        New StringParam With {.Switch = "--sar", .Text = "Sample Aspect Ratio", .InitValue = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                        New BoolParam With {.Switch = "--enforce-hrd", .Text = "Enforce HRD compatibility"})
                    Add("Other",
                        New StringParam With {.Switch = "--chapter", .Text = "Chapters", .Quotes = True, .BrowseFile = True},
                        New StringParam With {.Switch = "--log", .Text = "Log File", .Quotes = True, .BrowseFile = True},
                        Custom,
                        New OptionParam With {.Switch = "--tier", .Text = "Tier", .Options = {"Main", "High"}, .VisibleFunc = Function() Codec.ValueText = "hevc"},
                        New OptionParam With {.Switch = "--log-level", .Text = "Log Level", .Options = {"Info", "Debug", "Warn", "Error"}},
                        New OptionParam With {.Switch = "--motion-est", .Text = "Motion Estimation", .Options = {"Q-pel", "Full-pel", "Half-pel"}},
                        New OptionParam With {.Switch = "--pre-analysis", .Name = "pre-analysis-h264", .Text = "Pre Analysis", .Options = {"None", "Full", "Half", "Quarter"}, .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New OptionParam With {.Switch = "--pre-analysis", .Name = "pre-analysis-h265", .Text = "Pre Analysis", .Options = {"None", "Auto"}, .VisibleFunc = Function() Codec.ValueText = "hevc"},
                        New OptionParam With {.Switches = {"--tff", "--bff"}, .Text = "Interlaced", .Options = {"Progressive ", "Top Field First", "Bottom Field First"}, .Values = {"", "--tff", "--bff"}},
                        New BoolParam With {.Switch = "--chapter-copy", .Text = "Copy Chapters"},
                        New BoolParam With {.Switch = "--filler", .Text = "Use filler data"},
                        New BoolParam With {.Switch = "--fullrange", .Text = "Set yuv to fullrange", .VisibleFunc = Function() Codec.ValueText = "h264"})

                    For Each item In ItemsValue
                        If item.HelpSwitch <> "" Then Continue For
                        Dim switches = item.GetSwitches
                        If switches.NothingOrEmpty Then Continue For
                        item.HelpSwitch = switches(0)
                    Next
                End If

                Return ItemsValue
            End Get
        End Property

        Public Overrides Sub ShowHelp(id As String)
            g.ShowCommandLineHelp(Package.VCEEnc, id)
        End Sub

        Overrides Function GetCommandLine(includePaths As Boolean,
                                          includeExecutable As Boolean,
                                          Optional pass As Integer = 1) As String
            Dim ret As String
            Dim sourcePath As String
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)
            If includePaths AndAlso includeExecutable Then ret = Package.VCEEnc.Path.Escape

            Select Case Decoder.ValueText
                Case "avs"
                    sourcePath = p.Script.Path
                Case "qs"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.QSVEnc.Path.Escape, "QSVEncC64") + " -o - -c raw" + " -i " + If(includePaths, p.SourceFile.Escape, "path") + " | " + If(includePaths, Package.VCEEnc.Path.Escape, "VCEEncC64")
                Case "ffdxva"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.SourceFile.Escape, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel fatal - | " + If(includePaths, Package.VCEEnc.Path.Escape, "VCEEncC64")
                Case "ffqsv"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.SourceFile.Escape, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel fatal - | " + If(includePaths, Package.VCEEnc.Path.Escape, "VCEEncC64")
                Case "vce"
                    sourcePath = p.LastOriginalSourceFile
                    ret += " --avhw"
            End Select

            Dim q = From i In Items Where i.GetArgs <> ""
            If q.Count > 0 Then ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")

            Select Case Mode.Value
                Case 0
                    ret += " --cqp " & QPI.Value & ":" & QPP.Value & ":" & QPB.Value
                Case 1
                    ret += " --cbr " & p.VideoBitrate
                Case 2
                    ret += " --vbr " & p.VideoBitrate
            End Select

            If CInt(p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 AndAlso
                (p.Script.IsFilterActive("Crop", "Hardware Encoder") OrElse
                (Decoder.ValueText <> "avs" AndAlso p.Script.IsFilterActive("Crop"))) Then

                ret += " --crop " & p.CropLeft & "," & p.CropTop & "," & p.CropRight & "," & p.CropBottom
            End If

            If p.Script.IsFilterActive("Resize", "Hardware Encoder") OrElse
                (Decoder.ValueText <> "avs" AndAlso p.Script.IsFilterActive("Resize")) Then

                ret += " --output-res " & p.TargetWidth & "x" & p.TargetHeight
            End If

            If sourcePath = "-" Then ret += " --y4m"
            If includePaths Then ret += " -i " + sourcePath.Escape + " -o " + targetPath.Escape

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.VCEEnc
        End Function
    End Class
End Class