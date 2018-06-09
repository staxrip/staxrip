Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class QSVEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "Intel " + Params.Codec.OptionText
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
            f.HTMLHelp = Strings.Intel

            Dim saveProfileAction = Sub()
                                        Dim enc = ObjectHelp.GetCopy(Of QSVEnc)(Me)
                                        Dim params2 As New EncoderParams
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            ActionMenuItem.Add(f.cms.Items, "Save Profile...", saveProfileAction).SetImage(Symbol.Save)
            f.cms.Items.Add(New ActionMenuItem("Check Environment", Sub() g.ShowCode("Check Environment", ProcessHelp.GetStdOut(Package.QSVEnc.Path, "--check-environment"))))
            f.cms.Items.Add(New ActionMenuItem("Check Hardware", Sub() MsgInfo(ProcessHelp.GetStdOut(Package.QSVEnc.Path, "--check-hw"))))
            f.cms.Items.Add(New ActionMenuItem("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetStdOut(Package.QSVEnc.Path, "--check-features"))))
            f.cms.Items.Add(New ActionMenuItem("Check Library", Sub() MsgInfo(ProcessHelp.GetStdOut(Package.QSVEnc.Path, "--check-lib"))))

            If f.ShowDialog() = DialogResult.OK Then
                Params = params1
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides ReadOnly Property OutputExt() As String
        Get
            If Params.Codec.ValueText = "mpeg2" Then Return "m2v"
            Return Params.Codec.ValueText
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Params.RaiseValueChanged(Nothing)

        Using proc As New Proc
            proc.Header = "Video encoding"
            proc.Package = Package.QSVEnc
            proc.SkipString = " frames: "
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
            Return Params.Mode.ValueText.EqualsAny("cqp", "vqp", "icq", "la-icq", "qvbr-q")
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

        Shared Modes As New List(Of StringPair) From {
            New StringPair("avbr", "AVBR - Average Variable Bitrate"),
            New StringPair("cbr", "CBR - Constant Bitrate"),
            New StringPair("cqp", "CQP - Constant QP"),
            New StringPair("icq", "ICQ - Intelligent Constant Quality"),
            New StringPair("la", "LA - VBR Lookahead"),
            New StringPair("la-hrd", "LA-HRD - VBR HRD Lookahead"),
            New StringPair("la-icq", "LA-ICQ - Intelligent Constant Quality Lookahead"),
            New StringPair("qvbr", "QVBR - Quality Variable Bitrate using bitrate"),
            New StringPair("qvbr-q", "QVBR-Q - Quality Variable Bitrate using quality"),
            New StringPair("vbr", "VBR - Variable Bitrate"),
            New StringPair("vcm", "VCM - Video Conferencing Mode"),
            New StringPair("vqp", "VQP - Variable QP")}

        Sub New()
            Title = "QSVEnc Options"
        End Sub

        Property Decoder As New OptionParam With {
            .Text = "Decoder",
            .Options = {"AviSynth/VapourSynth", "QSVEnc Intel", "QSVEnc Software", "ffmpeg Intel", "ffmpeg DXVA2"},
            .Values = {"avs", "qs", "qsw", "ffqsv", "ffdxva"}}

        Property Codec As New OptionParam With {
            .Switch = "--codec",
            .Text = "Codec",
            .Options = {"H.264", "H.265", "MPEG-2"},
            .Values = {"h264", "hevc", "mpeg2"}}

        Property Mode As New OptionParam With {
            .Switches = {"--avbr", "--cbr", "--vbr", "--qvbr-q", "--cqp", "--vqp", "--icq", "--la-icq", "--vcm", "--la", "--la-hrd", "--qvbr"},
            .Name = "Mode",
            .Text = "Mode",
            .Expand = True,
            .Options = Modes.Select(Function(a) a.Value).ToArray,
            .Values = Modes.Select(Function(a) a.Name).ToArray,
            .InitValue = 2}

        Property Deinterlace As New OptionParam With {
            .Switch = "--vpp-deinterlace",
            .Text = "Deinterlace",
            .Options = {"None", "Normal", "Inverse Telecine", "Double Framerate"},
            .Values = {"none", "normal", "it", "bob"}}

        Property Quality As New NumParam With {
            .Text = "Quality",
            .Init = 23,
            .VisibleFunc = Function() {"icq", "la-icq", "qvbr-q"}.Contains(Mode.ValueText),
            .Config = {0, 51}}

        Property QPI As New NumParam With {
            .Switches = {"--cqp", "--vqp"},
            .Text = "QP I",
            .Init = 24,
            .VisibleFunc = Function() {"cqp", "vqp"}.Contains(Mode.ValueText),
            .Config = {0, 51}}

        Property QPP As New NumParam With {
            .Switches = {"--cqp", "--vqp"},
            .Text = "QP P",
            .Init = 26,
            .VisibleFunc = Function() {"cqp", "vqp"}.Contains(Mode.ValueText),
            .Config = {0, 51}}

        Property QPB As New NumParam With {
            .Switches = {"--cqp", "--vqp"},
            .Text = "QP B",
            .Init = 27,
            .VisibleFunc = Function() {"cqp", "vqp"}.Contains(Mode.ValueText),
            .Config = {0, 51}}

        Property MaxCLL As New NumParam With {
        .Text = "Maximum CLL",
        .Switch = "--max-cll",
        .Config = {0, Integer.MaxValue, 50},
        .VisibleFunc = Function() Codec.ValueText = "h265",
        .ImportAction = Sub(arg As String)
                            Dim a = arg.Split(","c)
                            MaxCLL.Value = a(0).ToInt
                            MaxFALL.Value = a(1).ToInt
                        End Sub,
        .ArgsFunc = Function() If(MaxCLL.Value <> 0 OrElse MaxFALL.Value <> 0, "--max-cll """ & MaxCLL.Value & "," & MaxFALL.Value & """", "")}

        Property MaxFALL As New NumParam With {
        .Config = {0, Integer.MaxValue, 50},
        .VisibleFunc = Function() Codec.ValueText = "h265",
        .ArgsFunc = Function() "",
        .Text = "Maximum FALL"}

        Property TFF As New BoolParam With {
            .Switch = "--tff",
            .Text = "Top Field First"}

        Property BFF As New BoolParam With {
            .Switch = "--bff",
            .Text = "Bottom Field First"}

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)

                    Add("Basic", Mode, Decoder, Codec,
                        New OptionParam With {.Switch = "--quality", .Text = "Preset", .Options = {"Best", "Higher", "High", "Balanced", "Fast", "Faster", "Fastest"}, .InitValue = 3},
                        New OptionParam With {.Switch = "--profile", .Text = "Profile", .Name = "ProfileH264", .VisibleFunc = Function() Codec.Value = 0, .Options = {"Automatic", "Baseline", "Main", "High"}},
                        New OptionParam With {.Switch = "--profile", .Text = "Profile", .Name = "ProfileH265", .VisibleFunc = Function() Codec.Value = 1, .Options = {"Automatic", "Main", "Main 10"}},
                        New OptionParam With {.Switch = "--profile", .Text = "Profile", .Name = "ProfileMPEG2", .VisibleFunc = Function() Codec.Value = 2, .Options = {"Automatic", "Simple", "Main", "High"}},
                        New OptionParam With {.Switch = "--level", .Name = "LevelHEVC", .Text = "Level", .VisibleFunc = Function() Codec.Value = 1, .Options = {"Automatic", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}},
                        New OptionParam With {.Switch = "--level", .Text = "Level", .VisibleFunc = Function() Codec.Value = 0, .Options = {"Automatic", "1", "1b", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                        New OptionParam With {.Switch = "--level", .Name = "LevelMPEG2", .Text = "Level", .VisibleFunc = Function() Codec.Value = 2, .Options = {"Automatic", "low", "main", "high", "High1440"}},
                        Quality, QPI, QPP, QPB)
                    Add("Analysis",
                        New OptionParam With {.Switch = "--trellis", .Text = "Trellis", .Options = {"Automatic", "Off", "I", "IP", "All"}},
                        New BoolParam With {.Switch = "--repartition-check", .Text = "Repartition Check"})
                    Add("Slice Decision",
                        New OptionParam With {.Switch = "--la-quality", .Text = "LA Quality", .Options = {"Automatic", "Fast", "Medium", "Slow"}},
                        New NumParam With {.Switch = "--slices", .Text = "Slices", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--la-depth", .VisibleFunc = Function() Mode.ValueText.EqualsAny("la", "la-hrd", "la-icq"), .Text = "Lookahead Depth", .Value = 30, .Config = {0, 100}},
                        New NumParam With {.Switch = "--la-window-size", .Text = "LA Window Size"},
                        New NumParam With {.Switch = "--bframes", .Text = "B-Frames", .Init = 3, .Config = {0, 16}},
                        New NumParam With {.Switch = "--ref", .Text = "Ref Frames", .Config = {0, 16}},
                        New NumParam With {.Switch = "--gop-len", .Text = "GOP Length", .Config = {0, Integer.MaxValue, 1}},
                        New BoolParam With {.Switch = "--b-pyramid", .Text = "B-Pyramid"},
                        New BoolParam With {.Switch = "--b-adapt", .Text = "Adaptive B-Frame Insert"},
                        New BoolParam With {.Switch = "--direct-bias-adjust", .Text = "Direct Bias Adjust"},
                        New BoolParam With {.Switch = "--scenechange", .Text = "Scenechange"},
                        New BoolParam With {.Switch = "--strict-gop", .Text = "Strict Gop"},
                        New BoolParam With {.Switch = "--open-gop", .Text = "Open Gop"})
                    Add("Rate Control",
                        New NumParam With {.Switch = "--max-bitrate", .Text = "Max Bitrate", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-offset", .Text = "QP Offset", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--avbr-unitsize", .Text = "AVBR Unitsize", .Init = 90},
                        New BoolParam With {.Switch = "--mbbrc", .Text = "Per macro block rate control"},
                        New BoolParam With {.Switch = "--extbrc", .Text = "Extended Rate Control"})
                    Add("Motion Search",
                        New OptionParam With {.Switch = "--mv-scaling", .Text = "MV Scaling", .IntegerValue = True, .Options = {"Default", "MV cost to be 0", "MV cost 1/2 of default", "MV cost 1/4 of default", "MV cost 1/8 of default"}},
                        New BoolParam With {.Switch = "--weightb", .Text = "B-Frame Weight Prediction"},
                        New BoolParam With {.Switch = "--weightp", .Text = "P-Frame Weight Prediction"})
                    Add("Performance",
                        New OptionParam With {.Switch = "--output-buf", .Text = "Output Buffer", .Options = {"8", "16", "32", "64", "128"}},
                        New NumParam With {.Switch = "--input-buf", .Text = "Input Buffer", .Config = {0, 16}},
                        New NumParam With {.Switch = "--input-thread", .Text = "Input Thread", .Config = {0, 64}},
                        New NumParam With {.Switch = "--mfx-thread", .Text = "Input Threads MFX"},
                        New NumParam With {.Switch = "--output-thread", .Text = "Output Thread", .Config = {0, 64}},
                        New NumParam With {.Switch = "--async-depth", .Text = "Async Depth", .Config = {0, 64}},
                        New BoolParam With {.Switch = "--min-memory", .Text = "Minimize memory usage"},
                        New BoolParam With {.Switch = "--max-procfps", .Text = "Limit performance to lower resource usage"})
                    Add("Bitstream",
                        New BoolParam With {.Switch = "--aud", .Text = "Insert aud nal unit"},
                        New BoolParam With {.Switch = "--pic-struct", .Text = "Insert pic-timing SEI with pic_struct"})
                    Add("VPP",
                        New StringParam With {.Switch = "--vpp-sub", .Text = "Subtitle File", .Quotes = True, .BrowseFile = True},
                        New StringParam With {.Switch = "--vpp-sub-charset", .Text = "Subtitle Charset", .Quotes = True},
                        New OptionParam With {.Switch = "--vpp-sub-shaping", .Text = "Subtitle Shaping", .Options = {"Simple", "Complex"}},
                        New OptionParam With {.Switch = "--vpp-rotate", .Text = "Rotate", .Options = {"0", "90", "180", "270"}},
                        New OptionParam With {.Switch = "--vpp-image-stab", .Text = "Image Stabilizer", .Options = {"Disabled", "Upscale", "Box"}},
                        New OptionParam With {.Switch = "--vpp-mirror", .Text = "Mirror Image", .Options = {"Disabled", "H", "V"}},
                        New OptionParam With {.Switch = "--vpp-scaling", .Text = "Scaling Quality", .Options = {"Automatic", "Simple", "Fine"}},
                        New NumParam With {.Switch = "--vpp-denoise", .Text = "Denoise", .Config = {0, 100}},
                        New NumParam With {.Switch = "--vpp-detail-enhance", .Text = "Detail Enhance", .Config = {0, 100}})
                    Add("VUI",
                        New StringParam With {.Switch = "--master-display", .Text = "Master Display", .Quotes = True, .VisibleFunc = Function() Codec.ValueText = "h265"},
                        New StringParam With {.Switch = "--sar", .Text = "Sample Aspect Ratio", .InitValue = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                        New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat", .Options = {"Undefined", "NTSC", "Component", "PAL", "SECAM", "MAC"}},
                        New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix", .Options = {"Undefined", "BT 470 BG", "BT 709", "FCC", "GBR", "SMPTE 170 M", "SMPTE 240 M", "YCgCo"}},
                        New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim", .Options = {"Undefined", "BT 709", "SMPTE 170 M", "BT 470 M", "BT 470 BG", "SMPTE 240 M", "Film", "BT 2020"}},
                        New OptionParam With {.Switch = "--transfer", .Text = "Transfer", .Options = {"Undefined", "BT 709", "SMPTE 170 M", "BT 470 M", "BT 470 BG", "SMPTE 240 M", "Linear", "Log 100", "Log 316", "IEC 61966-2-4", "BT 1361 E", "IEC 61966-2-1", "BT 2020-10", "BT 2020-12", "SMPTE 2084", "SMPTE 428", "ARIB-SRD-B67"}},
                        MaxCLL, MaxFALL,
                        New BoolParam With {.Switch = "--fullrange", .Text = "Fullrange"})
                    Add("Deinterlace", Deinterlace, TFF, BFF)
                    Add("Other",
                        New StringParam With {.Text = "Custom", .AlwaysOn = True},
                        New OptionParam With {.Switches = {"--disable-d3d", "--d3d9", "--d3d11", "--d3d"}, .Text = "D3D", .Options = {"Disabled", "D3D9", "D3D11", "D3D9/D3D11"}, .Values = {"--disable-d3d", "--d3d9", "--d3d11", "--d3d"}, .InitValue = 3},
                        New OptionParam With {.Switch = "--log-level", .Text = "Log Level", .Options = {"Info", "Debug", "Warn", "Error"}},
                        New BoolParam With {.Switch = "--no-deblock", .Text = "No Deblock"},
                        New BoolParam With {.Switch = "--fallback-rc", .Text = "Enable fallback for unsupported modes", .Value = True},
                        New BoolParam With {.Switch = "--timer-period-tuning", .NoSwitch = "--no-timer-period-tuning", .Text = "Timer Period Tuning", .Init = True},
                        New BoolParam With {.Switch = "--i-adapt", .Text = "Adaptive I-Frame Insert"},
                        New BoolParam With {.Switch = "--fixed-func", .Text = "Use fixed func instead of GPU EU"},
                        New BoolParam With {.Switch = "--bluray", .Text = "Blu-ray"},
                        New BoolParam With {.Switch = "--fade-detect", .Text = "Fade Detection"})

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
            g.ShowCommandLineHelp(Package.QSVEnc, id)
        End Sub

        Function GetMode(name As String) As Integer
            For x = 0 To Modes.Count - 1
                If Modes(x).Name = name Then Return x
            Next
        End Function

        Protected Overrides Sub OnValueChanged(item As CommandLineParam)
            If item Is Deinterlace Then
                If Deinterlace.ValueText = "normal" OrElse Deinterlace.ValueText = "bob" Then
                    If Not TFF.Value AndAlso Not BFF.Value Then TFF.Value = True
                Else
                    TFF.Value = False
                    BFF.Value = False
                End If
            End If

            If Not Mode.MenuButton Is Nothing Then
                If item Is Codec OrElse item Is Nothing Then
                    For Each i In Modes
                        Select Case Codec.ValueText
                            Case "h264"
                                Mode.ShowOption(GetMode(i.Name), True)
                            Case "hevc"
                                Mode.ShowOption(GetMode(i.Name), i.Name.EqualsAny("cbr", "vbr", "cqp", "vqp", "icq", "vcm"))
                            Case "mpeg2"
                                Mode.ShowOption(GetMode(i.Name), i.Name.EqualsAny("cbr", "vbr", "avbr", "cqp", "vqp"))
                        End Select
                    Next
                End If
            End If

            MyBase.OnValueChanged(item)
        End Sub

        Overrides Function GetCommandLine(includePaths As Boolean,
                                          includeExecutable As Boolean,
                                          Optional pass As Integer = 1) As String
            Dim ret As String
            Dim sourcePath = p.Script.Path
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)

            If includePaths AndAlso includeExecutable Then ret = Package.QSVEnc.Path.Escape

            Select Case Decoder.ValueText
                Case "avs"
                    sourcePath = p.Script.Path
                Case "qs", "qsw"
                    sourcePath = p.LastOriginalSourceFile
                Case "ffdxva"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.LastOriginalSourceFile.Escape, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel fatal -hide_banner - | " + If(includePaths, Package.QSVEnc.Path.Escape, "QSVEncC64")
                Case "ffqsv"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.LastOriginalSourceFile.Escape, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel fatal -hide_banner - | " + If(includePaths, Package.QSVEnc.Path.Escape, "QSVEncC64")
            End Select

            Dim q = From i In Items Where i.GetArgs <> ""
            If q.Count > 0 Then ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")

            Select Case Mode.ValueText
                Case "icq", "la-icq"
                    ret += " --" + Mode.ValueText + " " & CInt(Quality.Value)
                Case "qvbr-q"
                    ret += " --qvbr-q " & CInt(Quality.Value) & " --qvbr " & p.VideoBitrate
                Case "cqp", "vqp"
                    ret += " --" + Mode.ValueText + " " & CInt(QPI.Value) & ":" & CInt(QPP.Value) & ":" & CInt(QPB.Value)
                Case Else
                    ret += " --" + Mode.ValueText + " " & p.VideoBitrate
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

            If Decoder.ValueText <> "avs" Then
                If p.Ranges.Count > 0 Then
                    ret += " --trim " + p.Ranges.Select(Function(range) range.Start & ":" & range.End).Join(",")
                End If
            End If

            If sourcePath = "-" Then ret += " --y4m"
            If Decoder.ValueText = "qsw" Then ret += " --avsw"
            If includePaths Then ret += " -i " + sourcePath.Escape + " -o " + targetPath.Escape

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.QSVEnc
        End Function
    End Class
End Class