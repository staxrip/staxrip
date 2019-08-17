Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class NVEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "Nvidia | " + Params.Codec.OptionText
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
        Dim newParams As New EncoderParams
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(store)

        Using form As New CommandLineForm(newParams)
            Dim saveProfileAction = Sub()
                                        Dim enc = ObjectHelp.GetCopy(Of NVEnc)(Me)
                                        Dim params2 As New EncoderParams
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            form.cms.Items.Add(New ActionMenuItem("Check Hardware", Sub() MsgInfo(ProcessHelp.GetStdOut(Package.NVEnc.Path, "--check-hw"))))
            form.cms.Items.Add(New ActionMenuItem("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetStdOut(Package.NVEnc.Path, "--check-features"))))
            form.cms.Items.Add(New ActionMenuItem("Check Environment", Sub() g.ShowCode("Check Environment", ProcessHelp.GetStdOut(Package.NVEnc.Path, "--check-environment"))))
            ActionMenuItem.Add(form.cms.Items, "Save Profile...", saveProfileAction).SetImage(Symbol.Save)

            If form.ShowDialog() = DialogResult.OK Then
                Params = newParams
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
        If OutputExt = "h265" Then
            Dim codecs = ProcessHelp.GetStdOut(Package.NVEnc.Path, "--check-hw").Right("Codec(s)")
            If Not codecs?.ToLower.Contains("hevc") Then Throw New ErrorAbortException("NVEnc Error", "H.265/HEVC isn't supported by the graphics card.")
        End If

        p.Script.Synchronize()

        Using proc As New Proc
            proc.Header = "Video encoding"
            proc.Package = Package.NVEnc
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
            Title = "NVEnc Options"
        End Sub

        Property Decoder As New OptionParam With {
            .Text = "Decoder",
            .Options = {"AviSynth/VapourSynth",
                        "NVEnc Native",
                        "NVEnc Cuda",
                        "QSVEnc (Intel)",
                        "ffmpeg (Intel)",
                        "ffmpeg (DXVA2)"},
            .Values = {"avs", "nvnative", "nvcuda", "qs", "ffqsv", "ffdxva"}}

        Property Mode As New OptionParam With {
            .Text = "Mode",
            .Expand = True,
            .Switches = {"--cqp", "--cbr", "--cbrhq", "--vbr", "--vbrhq"},
            .Options = {"CQP - Constant QP", "CBR - Constant Bitrate", "CBR HQ - Constant Bitrate HQ", "VBR - Variable Bitrate", "VBR HQ - Variable Bitrate HQ"},
            .VisibleFunc = Function() Not Lossless.Value,
            .ArgsFunc = Function() As String
                            Select Case Mode.Value
                                Case 0
                                    Return "--cqp " & QPI.Value & ":" & QPP.Value & ":" & QPB.Value
                                Case 1
                                    Return "--cbr " & p.VideoBitrate
                                Case 2
                                    Return "--cbrhq " & p.VideoBitrate
                                Case 3
                                    Return "--vbr " & p.VideoBitrate
                                Case 4
                                    Return "--vbrhq " & p.VideoBitrate
                            End Select
                        End Function}

        Property Codec As New OptionParam With {
            .Switch = "--codec",
            .Text = "Codec",
            .Options = {"Nvidia H.264", "Nvidia H.265"},
            .Values = {"h264", "h265"}}

        Property Profile As New OptionParam With {
            .Switch = "--profile",
            .Text = "Profile",
            .Name = "ProfileH264",
            .VisibleFunc = Function() Codec.ValueText = "h264",
            .Options = {"Baseline", "Main", "High", "High 444"},
            .InitValue = 2}

        Property ProfileH265 As New OptionParam With {
            .Switch = "--profile",
            .Text = "Profile",
            .Name = "ProfileH265",
            .VisibleFunc = Function() Codec.ValueText = "h265",
            .Options = {"Main", "Main 10", "Main 444"},
            .InitValue = 0}

        Property QPI As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP I",
            .Init = 18,
            .VisibleFunc = Function() Mode.Value = 0,
            .Config = {0, 51}}

        Property QPP As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP P",
            .Init = 20,
            .VisibleFunc = Function() Mode.Value = 0,
            .Config = {0, 51}}

        Property QPB As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP B",
            .Init = 22,
            .VisibleFunc = Function() Mode.Value = 0,
            .Config = {0, 51}}

        Property Lossless As New BoolParam With {
            .Switch = "--lossless",
            .Text = "Lossless",
            .VisibleFunc = Function() Codec.ValueText = "h264" AndAlso Profile.Visible}

        Property MaxCLL As New NumParam With {
        .Text = "Maximum CLL",
        .VisibleFunc = Function() Codec.ValueText = "h265",
        .Switch = "--max-cll",
        .Config = {0, Integer.MaxValue, 50},
        .ImportAction = Sub(arg As String)
                            If arg = "" Then Exit Sub
                            Dim a = arg.Trim(""""c).Split(","c)
                            MaxCLL.Value = a(0).ToInt
                            MaxFALL.Value = a(1).ToInt
                        End Sub,
        .ArgsFunc = Function() If(MaxCLL.Value <> 0 OrElse MaxFALL.Value <> 0, "--max-cll """ & MaxCLL.Value & "," & MaxFALL.Value & """", "")}

        Property MaxFALL As New NumParam With {
        .Config = {0, Integer.MaxValue, 50},
        .ArgsFunc = Function() "",
        .VisibleFunc = Function() Codec.ValueText = "h265",
        .Text = "Maximum FALL"}

        Property KNN As New BoolParam With {
            .Switch = "--vpp-knn",
            .Text = "Denoise using K-nearest neighbor",
            .ArgsFunc = AddressOf GetKnnArgs}

        Property Pad As New BoolParam With {
            .Switch = "--vpp-pad",
            .Text = "Padding",
            .ArgsFunc = AddressOf GetPaddingArgs}

        Property PadLeft As New NumParam With {
            .Text = "      Left",
            .Init = 0}

        Property PadTop As New NumParam With {
            .Text = "      Top",
            .Init = 0}

        Property PadRight As New NumParam With {
            .Text = "      Right",
            .Init = 0}

        Property PadBottom As New NumParam With {
            .Text = "      Bottom",
            .Init = 0}

        Property Tweak As New BoolParam With {
            .Switch = "--vpp-tweak",
            .Text = "Tweaking",
            .ArgsFunc = AddressOf GetTweakArgs}

        Property KnnRadius As New NumParam With {
            .Text = "      Radius",
            .Init = 3}

        Property vppcontrast As New NumParam With {
            .Text = "      Contrast",
            .Init = 1.0,
            .Config = {-2.0, 2.0, 0.1, 1}}

        Property vppgamma As New NumParam With {
            .Text = "      Gamma",
            .Init = 1.0,
            .Config = {0.1, 10.0, 0.1, 1}}

        Property vppsaturation As New NumParam With {
            .Text = "      Saturation",
            .Init = 1.0,
            .Config = {0.0, 3.0, 0.1, 1}}

        Property vpphue As New NumParam With {
            .Text = "      Hue",
            .Init = 0.0,
            .Config = {-180.0, 180.0, 0.1, 1}}

        Property vppbrightness As New NumParam With {
            .Text = "      Brightness",
            .Init = 0.0,
            .Config = {-1.0, 1.0, 0.1, 1}}

        Property KnnStrength As New NumParam With {
            .Text = "      Strength",
            .Init = 0.08,
            .Config = {0, 1, 0.02, 2}}

        Property Chromaloc As New NumParam With {
        .Switch = "--chromaloc",
        .Text = "Chromaloc",
        .Config = {0, 5}}

        Property KnnLerp As New NumParam With {
            .Text = "      Lerp",
            .Init = 0.2,
            .Config = {0, Integer.MaxValue, 0.1, 1}}

        Property KnnThLerp As New NumParam With {
            .Text = "      TH Lerp",
            .Init = 0.8,
            .Config = {0, 1, 0.1, 1}}

        Property PMD As New BoolParam With {
            .Switch = "--vpp-pmd",
            .Text = "Denoise using PMD",
            .ArgsFunc = AddressOf GetPmdArgs}

        Property PmdApplyCount As New NumParam With {
            .Text = "      Apply Count",
            .Init = 2}

        Property PmdStrength As New NumParam With {
            .Text = "      Strength",
            .Name = "PmdStrength",
            .Init = 100.0,
            .Config = {0, 100, 1, 1}}

        Property PmdThreshold As New NumParam With {
            .Text = "      Threshold",
            .Init = 100.0,
            .Config = {0, 255, 1, 1}}

        Property Interlace As New OptionParam With {
            .Text = "Interlace",
            .VisibleFunc = Function() Codec.ValueText = "h264",
            .Options = {"Top Field First", "Bottom Field First"},
            .Values = {"--interlace tff", "--interlace bff"}}

        Property Deband As New BoolParam With {.Text = "Deband", .Switches = {"--vpp-deband"}, .ArgsFunc = AddressOf GetDebandArgs}

        Property Deband_range As New NumParam With {.Text = "range", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 127}}
        Property Deband_sample As New NumParam With {.Text = "sample", .HelpSwitch = "--vpp-deband", .Init = 1, .Config = {0, 2}}
        Property Deband_thre As New NumParam With {.Text = "thre", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property Deband_thre_y As New NumParam With {.Text = "     thre_y", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property Deband_thre_cb As New NumParam With {.Text = "     thre_cb", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property Deband_thre_cr As New NumParam With {.Text = "     thre_cr", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property Deband_dither As New NumParam With {.Text = "dither", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property Deband_dither_y As New NumParam With {.Text = "     dither_y", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property Deband_dither_c As New NumParam With {.Text = "     dither_c", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property Deband_seed As New NumParam With {.Text = "seed", .HelpSwitch = "--vpp-deband", .Init = 1234}

        Property Deband_blurfirst As New BoolParam With {.Text = "blurfirst", .HelpSwitch = "--vpp-deband"}
        Property Deband_rand_each_frame As New BoolParam With {.Text = "rand_each_frame", .HelpSwitch = "--vpp-deband"}

        Property AFS As New BoolParam With {.Text = "Auto field shift deinterlacer", .Switches = {"--vpp-afs"}, .ArgsFunc = AddressOf GetAFS}

        Property AFSPreset As New OptionParam With {.Text = "preset", .HelpSwitch = "--vpp-afs", .Options = {"Default", "Triple", "Double", "Anime", "Cinema", "Min_afterimg", "24fps", "24fps_sd", "30fps"}}
        Property AFSINI As New StringParam With {.Text = "ini", .HelpSwitch = "--vpp-afs", .BrowseFile = True}

        Property AFSLeft As New NumParam With {.Text = "left", .HelpSwitch = "--vpp-afs", .Init = 32}
        Property AFSRight As New NumParam With {.Text = "right", .HelpSwitch = "--vpp-afs", .Init = 32}
        Property AFSTop As New NumParam With {.Text = "top", .HelpSwitch = "--vpp-afs", .Init = 16}
        Property AFSBottom As New NumParam With {.Text = "bottom", .HelpSwitch = "--vpp-afs", .Init = 16}

        Property AFSmethod_switch As New NumParam With {.Text = "method_switch", .HelpSwitch = "--vpp-afs", .Config = {0, 256}}
        Property AFScoeff_shift As New NumParam With {.Text = "coeff_shift", .HelpSwitch = "--vpp-afs", .Init = 192, .Config = {0, 256}}
        Property AFSthre_shift As New NumParam With {.Text = "thre_shift", .HelpSwitch = "--vpp-afs", .Init = 128, .Config = {0, 1024}}
        Property AFSthre_deint As New NumParam With {.Text = "thre_deint", .HelpSwitch = "--vpp-afs", .Init = 48, .Config = {0, 1024}}
        Property AFSthre_motion_y As New NumParam With {.Text = "thre_motion_y", .HelpSwitch = "--vpp-afs", .Init = 112, .Config = {0, 1024}}
        Property AFSthre_motion_c As New NumParam With {.Text = "thre_motion_c", .HelpSwitch = "--vpp-afs", .Init = 224, .Config = {0, 1024}}
        Property AFSlevel As New NumParam With {.Text = "level", .HelpSwitch = "--vpp-afs", .Init = 3, .Config = {0, 4}}

        Property AFSshift As New BoolParam With {.Text = "shift", .Init = True, .HelpSwitch = "--vpp-afs"}
        Property AFSdrop As New BoolParam With {.Text = "drop", .HelpSwitch = "--vpp-afs"}
        Property AFSsmooth As New BoolParam With {.Text = "smooth", .HelpSwitch = "--vpp-afs"}
        Property AFS24fps As New BoolParam With {.Text = "24fps", .HelpSwitch = "--vpp-afs"}
        Property AFStune As New BoolParam With {.Text = "tune", .HelpSwitch = "--vpp-afs"}
        Property AFSrff As New BoolParam With {.Text = "rff", .HelpSwitch = "--vpp-afs"}
        Property AFStimecode As New BoolParam With {.Text = "timecode", .HelpSwitch = "--vpp-afs"}
        Property AFSlog As New BoolParam With {.Text = "log", .HelpSwitch = "--vpp-afs"}

        Property VppEdgelevel As New BoolParam With {.Text = "Edgelevel filter to enhance edge", .Switches = {"--vpp-edgelevel"}, .ArgsFunc = AddressOf GetEdge}
        Property VppEdgelevelStrength As New NumParam With {.Text = "     strength", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}
        Property VppEdgelevelThreshold As New NumParam With {.Text = "     threshold", .HelpSwitch = "--vpp-edgelevel", .Init = 20, .Config = {0, 255, 1, 1}}
        Property VppEdgelevelBlack As New NumParam With {.Text = "     black", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}
        Property VppEdgelevelWhite As New NumParam With {.Text = "     white", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}

        Property VppUnsharp As New BoolParam With {.Text = "Enable unsharp filter", .Switches = {"--vpp-unsharp"}, .ArgsFunc = AddressOf GetUnsharp}
        Property VppUnsharpRadius As New NumParam With {.Text = "     radius", .HelpSwitch = "--vpp-unsharp", .Init = 3, .Config = {1, 9}}
        Property VppUnsharpWeight As New NumParam With {.Text = "     weight", .HelpSwitch = "--vpp-unsharp", .Init = 0.5, .Config = {0, 10, 0.5, 1}}
        Property VppUnsharpThreshold As New NumParam With {.Text = "     threshold", .HelpSwitch = "--vpp-unsharp", .Init = 10, .Config = {0, 255, 1, 1}}

        Property VppNnedi As New BoolParam With {.Text = "nnedi deinterlacer", .Switches = {"--vpp-nnedi"}, .ArgsFunc = AddressOf GetNnedi}
        Property VppNnediField As New OptionParam With {.Text = "     field", .HelpSwitch = "--vpp-nnedi", .Options = {"auto", "top", "bottom"}}
        Property VppNnediNns As New OptionParam With {.Text = "     nns", .HelpSwitch = "--vpp-nnedi", .InitValue = 1, .Options = {"16", "32", "64", "128", "256"}}
        Property VppNnediNszie As New OptionParam With {.Text = "     nszie", .HelpSwitch = "--vpp-nnedi", .InitValue = 6, .Options = {"8x6", "16x6", "32x6", "48x6", "8x4", "16x4", "32x4"}}
        Property VppNnediQuality As New OptionParam With {.Text = "     quality", .HelpSwitch = "--vpp-nnedi", .Options = {"fast", "slow"}}
        Property VppNnediPrescreen As New OptionParam With {.Text = "     prescreen", .HelpSwitch = "--vpp-nnedi", .InitValue = 4, .Options = {"none", "original", "new", "original_block", "new_block"}}
        Property VppNnediErrortype As New OptionParam With {.Text = "     errortype", .HelpSwitch = "--vpp-nnedi", .Options = {"abs", "square"}}
        Property VppNnediPrec As New OptionParam With {.Text = "     prec", .HelpSwitch = "--vpp-nnedi", .Options = {"auto", "fp16", "fp32"}}
        Property VppNnediWeightfile As New StringParam With {.Text = "     weightfile", .HelpSwitch = "--vpp-nnedi", .BrowseFile = True}

        Property VppSelectEvery As New BoolParam With {.Text = "Select Every", .Switches = {"--vpp-select-every"}, .ArgsFunc = AddressOf GetSelectEvery}
        Property VppSelectEveryValue As New NumParam With {.Text = "     value", .HelpSwitch = "--vpp-select-every", .Init = 2}
        Property VppSelectEveryOffsets As New StringParam With {.Text = "     offsets", .HelpSwitch = "--vpp-select-every", .Expand = False}

        Property Custom As New StringParam With {.Text = "Custom", .Quotes = QuotesMode.Never, .AlwaysOn = True}

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)
                    Add("Basic", Mode, Decoder, Codec,
                        New OptionParam With {.Switch = "--preset", .Text = "Preset", .Value = 1, .Options = {"Default", "Quality", "Performance"}},
                        Profile, ProfileH265,
                        New OptionParam With {.Switch = "--tier", .Text = "Tier", .VisibleFunc = Function() Codec.ValueText = "h265", .Options = {"Main", "High"}, .Values = {"main", "high"}},
                        New OptionParam With {.Name = "LevelH264", .Switch = "--level", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "h264", .Options = {"Unrestricted", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                        New OptionParam With {.Name = "LevelH265", .Switch = "--level", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "h265", .Options = {"Unrestricted", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}},
                        New OptionParam With {.Switch = "--output-depth", .Text = "Depth", .Options = {"8-Bit", "10-Bit"}, .Values = {"8", "10"}},
                        QPI, QPP, QPB)
                    Add("VPP",
                        New StringParam With {.Switch = "--vpp-subburn", .Text = "Subburn"},
                        New OptionParam With {.Switch = "--vpp-resize", .Text = "Resize", .Options = {"Disabled", "Default", "Bilinear", "Cubic", "Cubic_B05C03", "Cubic_bSpline", "Cubic_Catmull", "Lanczos", "NN", "NPP_Linear", "Spline 36", "Super"}},
                        New OptionParam With {.Switch = "--vpp-deinterlace", .Text = "Deinterlace", .VisibleFunc = Function() Decoder.ValueText.EqualsAny("nvnative", "nvcuda"), .Options = {"None", "Adaptive", "Bob"}},
                        New OptionParam With {.Switch = "--vpp-gauss", .Text = "Gauss", .Options = {"Disabled", "3", "5", "7"}},
                        New BoolParam With {.Switch = "--vpp-rff", .Text = "Enable repeat field flag", .VisibleFunc = Function() Decoder.ValueText.EqualsAny("nvnative", "nvcuda")},
                        VppEdgelevel,
                        VppEdgelevelStrength,
                        VppEdgelevelThreshold,
                        VppEdgelevelBlack,
                        VppEdgelevelWhite,
                        VppUnsharp,
                        VppUnsharpRadius,
                        VppUnsharpWeight,
                        VppUnsharpThreshold,
                        VppSelectEvery,
                        VppSelectEveryValue,
                        VppSelectEveryOffsets)
                    Add("VPP 2 | Denoise",
                        KNN, KnnRadius, KnnStrength, KnnLerp, KnnThLerp,
                        PMD, PmdApplyCount, PmdStrength, PmdThreshold)
                    Add("VPP 2 | Deband",
                        Deband,
                        Deband_range,
                        Deband_sample,
                        Deband_thre,
                        Deband_thre_y,
                        Deband_thre_cb,
                        Deband_thre_cr,
                        Deband_dither,
                        Deband_dither_y,
                        Deband_dither_c,
                        Deband_seed,
                        Deband_blurfirst,
                        Deband_rand_each_frame)
                    Add("VPP 2 | Deinterlace",
                        VppNnedi,
                        VppNnediField,
                        VppNnediNns,
                        VppNnediNszie,
                        VppNnediQuality,
                        VppNnediPrescreen,
                        VppNnediErrortype,
                        VppNnediPrec,
                        VppNnediWeightfile,
                        New OptionParam With {.Switch = "--vpp-yadif", .Text = "Yadif", .Options = {"disabled", "auto", "tff", "bff", "bob", "bob_tff", "bob_bff"}, .Values = {"", "", "mode=tff", "mode=bff", "mode=bob", "mode=bob_tff", "mode=bob_bff"}})
                    Add("VPP 2 | AFS 1",
                        AFS,
                        AFSINI,
                        AFSPreset,
                        AFSLeft,
                        AFSRight,
                        AFSTop,
                        AFSBottom,
                        AFSmethod_switch,
                        AFScoeff_shift,
                        AFSthre_shift,
                        AFSthre_deint,
                        AFSthre_motion_y,
                        AFSthre_motion_c,
                        AFSlevel)
                    Add("VPP 2 | AFS 2",
                        AFSshift,
                        AFSdrop,
                        AFSsmooth,
                        AFS24fps,
                        AFStune,
                        AFSrff,
                        AFStimecode,
                        AFSlog)
                    Add("VPP 2 | Tweak",
                        Tweak,
                        vppbrightness,
                        vppcontrast,
                        vppsaturation,
                        vppgamma,
                        vpphue,
                        Pad,
                        PadLeft,
                        PadTop,
                        PadRight,
                        PadBottom)
                    Add("Analysis",
                        New OptionParam With {.Switch = "--adapt-transform", .Text = "Adaptive Transform", .Options = {"Automatic", "Enabled", "Disabled"}, .Values = {"", "--adapt-transform", "--no-adapt-transform"}, .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New NumParam With {.Switch = "--cu-min", .Text = "Minimum CU Size", .Config = {0, 32, 16}},
                        New NumParam With {.Switch = "--cu-max", .Text = "Maximum CU Size", .Config = {0, 64, 16}},
                        New BoolParam With {.Switch = "--weightp", .Text = "Enable weighted prediction in P slices"})
                    Add("Slice Decision",
                        New OptionParam With {.Switch = "--direct", .Text = "B-Direct Mode", .Options = {"Automatic", "None", "Spatial", "Temporal"}, .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New NumParam With {.Switch = "--bframes", .Text = "B-Frames", .Init = 3, .Config = {0, 16}},
                        New NumParam With {.Switch = "--ref", .Text = "Ref Frames", .Init = 3, .Config = {0, 16}},
                        New NumParam With {.Switch = "--gop-len", .Text = "GOP Length", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--lookahead", .Text = "Lookahead", .Config = {0, 32}},
                        New NumParam With {.Switch = "--slices", .Text = "Slices", .Config = {0, Integer.MaxValue, 1}},
                        New BoolParam With {.Switch = "--strict-gop", .Text = "Strict GOP"},
                        New BoolParam With {.NoSwitch = "--no-b-adapt", .Text = "B-Adapt", .Init = True},
                        New BoolParam With {.NoSwitch = "--no-i-adapt", .Text = "I-Adapt", .Init = True},
                        New BoolParam With {.Switch = "--nonrefp", .Text = "Enable adapt. non-reference P frame insertion"})
                    Add("Rate Control",
                        New StringParam With {.Switch = "--dynamic-rc", .Text = "Dynamic RC"},
                        New NumParam With {.Switch = "--qp-init", .Text = "Initial QP", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--max-bitrate", .Text = "Max Bitrate", .Init = 17500, .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--vbv-bufsize", .Text = "VBV Bufsize", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--aq-strength", .Text = "AQ Strength", .Config = {0, 15}, .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New NumParam With {.Switch = "--vbr-quality", .Text = "VBR Quality", .Config = {0, 51, 1, 1}},
                        New BoolParam With {.Switch = "--aq", .Text = "Adaptive Quantization"},
                        New BoolParam With {.Switch = "--aq-temporal", .Text = "AQ Temporal"},
                        Lossless)
                    Add("Performance",
                        New StringParam With {.Switch = "--perf-monitor", .Text = "Perf. Monitor"},
                        New OptionParam With {.Switch = "--cuda-schedule", .Text = "Cuda Schedule", .Expand = True, .InitValue = 3, .Options = {"Let cuda driver to decide", "CPU will spin when waiting GPU tasks", "CPU will yield when waiting GPU tasks", "CPU will sleep when waiting GPU tasks"}, .Values = {"auto", "spin", "yield", "sync"}},
                        New OptionParam With {.Switch = "--output-buf", .Text = "Output Buffer", .Options = {"8", "16", "32", "64", "128"}},
                        New OptionParam With {.Switch = "--output-thread", .Text = "Output Thread", .Options = {"Automatic", "Disabled", "One Thread"}, .Values = {"-1", "0", "1"}},
                        New NumParam With {.Switch = "--perf-monitor-interval", .Init = 500, .Config = {50, Integer.MaxValue}, .Text = "Perf. Mon. Interval"},
                        New BoolParam With {.Switch = "--max-procfps", .Text = "Limit performance to lower resource usage"})
                    Add("VUI",
                        New StringParam With {.Switch = "--master-display", .Text = "Master Display", .VisibleFunc = Function() Codec.ValueText = "h265"},
                        New StringParam With {.Switch = "--sar", .Text = "Sample Aspect Ratio", .InitValue = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                        New StringParam With {.Switch = "--dhdr10-info", .Text = "Dynamic HDR10 Info", .BrowseFile = True},
                        New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat", .Options = {"Undefined", "NTSC", "Component", "PAL", "SECAM", "MAC"}},
                        New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix", .Options = {"Undefined", "BT 2020 C", "BT 2020 NC", "BT 470 BG", "BT 709", "FCC", "GBR", "SMPTE 170 M", "SMPTE 240 M", "YCgCo"}},
                        New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim", .Options = {"Undefined", "BT 2020", "BT 470 BG", "BT 470 M", "BT 709", "Film", "SMPTE 170 M", "SMPTE 240 M"}},
                        New OptionParam With {.Switch = "--transfer", .Text = "Transfer", .Options = {"Undefined", "ARIB-SRD-B67", "Auto", "BT 1361 E", "BT 2020-10", "BT 2020-12", "BT 470 BG", "BT 470 M", "BT 709", "IEC 61966-2-1", "IEC 61966-2-4", "Linear", "Log 100", "Log 316", "SMPTE 170 M", "SMPTE 240 M", "SMPTE 2084", "SMPTE 428"}},
                        MaxCLL, MaxFALL,
                        Chromaloc,
                        New BoolParam With {.Switch = "--pic-struct", .Text = "Set the picture structure and emits it in the picture timing SEI message"},
                        New BoolParam With {.Switch = "--fullrange", .Text = "Full Range", .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New BoolParam With {.Switch = "--aud", .Text = "AUD"})
                    Add("Other",
                        New StringParam With {.Switch = "--data-copy", .Text = "Data Copy"},
                        New OptionParam With {.Switch = "--mv-precision", .Text = "MV Precision", .Options = {"Automatic", "Q-pel", "Half-pel", "Full-pel"}},
                        New OptionParam With {.Switches = {"--cabac", "--cavlc"}, .Text = "Cabac/Cavlc", .Options = {"Disabled", "Cabac", "Cavlc"}, .Values = {"", "--cabac", "--cavlc"}},
                        Interlace,
                        New OptionParam With {.Switch = "--log-level", .Text = "Log Level", .Options = {"Info", "Debug", "Warn", "Error"}},
                        New NumParam With {.Switch = "--device", .Text = "Device", .Config = {0, 4}},
                        New BoolParam With {.Switch = "--deblock", .NoSwitch = "--no-deblock", .Text = "Deblock", .Init = True},
                        New BoolParam With {.Switch = "--bluray", .Text = "Blu-ray"},
                        Custom)

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
            g.ShowCommandLineHelp(Package.NVEnc, id)
        End Sub

        Protected Overrides Sub OnValueChanged(item As CommandLineParam)
            If Not QPI.NumEdit Is Nothing Then
                VppNnediField.MenuButton.Enabled = VppNnedi.Value
                VppNnediNns.MenuButton.Enabled = VppNnedi.Value
                VppNnediNszie.MenuButton.Enabled = VppNnedi.Value
                VppNnediQuality.MenuButton.Enabled = VppNnedi.Value
                VppNnediPrescreen.MenuButton.Enabled = VppNnedi.Value
                VppNnediErrortype.MenuButton.Enabled = VppNnedi.Value
                VppNnediPrec.MenuButton.Enabled = VppNnedi.Value
                VppNnediWeightfile.TextEdit.Enabled = VppNnedi.Value

                VppEdgelevelStrength.NumEdit.Enabled = VppEdgelevel.Value
                VppEdgelevelThreshold.NumEdit.Enabled = VppEdgelevel.Value
                VppEdgelevelBlack.NumEdit.Enabled = VppEdgelevel.Value
                VppEdgelevelWhite.NumEdit.Enabled = VppEdgelevel.Value

                VppUnsharpRadius.NumEdit.Enabled = VppUnsharp.Value
                VppUnsharpWeight.NumEdit.Enabled = VppUnsharp.Value
                VppUnsharpThreshold.NumEdit.Enabled = VppUnsharp.Value

                VppSelectEveryValue.NumEdit.Enabled = VppSelectEvery.Value
                VppSelectEveryOffsets.TextEdit.Enabled = VppSelectEvery.Value

                KnnRadius.NumEdit.Enabled = KNN.Value
                KnnStrength.NumEdit.Enabled = KNN.Value
                KnnLerp.NumEdit.Enabled = KNN.Value
                KnnThLerp.NumEdit.Enabled = KNN.Value

                PadLeft.NumEdit.Enabled = Pad.Value
                PadTop.NumEdit.Enabled = Pad.Value
                PadRight.NumEdit.Enabled = Pad.Value
                PadBottom.NumEdit.Enabled = Pad.Value

                vppcontrast.NumEdit.Enabled = Tweak.Value
                vppgamma.NumEdit.Enabled = Tweak.Value
                vppsaturation.NumEdit.Enabled = Tweak.Value
                vpphue.NumEdit.Enabled = Tweak.Value
                vppbrightness.NumEdit.Enabled = Tweak.Value

                PmdApplyCount.NumEdit.Enabled = PMD.Value
                PmdStrength.NumEdit.Enabled = PMD.Value
                PmdThreshold.NumEdit.Enabled = PMD.Value

                For Each i In {Deband_range, Deband_sample, Deband_thre, Deband_thre_y, Deband_thre_cb,
                    Deband_thre_cr, Deband_dither, Deband_dither_y, Deband_dither_c, Deband_seed}

                    i.NumEdit.Enabled = Deband.Value
                Next

                Deband_rand_each_frame.CheckBox.Enabled = Deband.Value
                Deband_blurfirst.CheckBox.Enabled = Deband.Value

                AFS.CheckBox.Enabled = Interlace.Value > 0

                AFSPreset.MenuButton.Enabled = AFS.Value
                AFSINI.TextEdit.Enabled = AFS.Value

                For Each i In {AFSLeft, AFSRight, AFSTop, AFSBottom, AFSmethod_switch, AFScoeff_shift,
                               AFSthre_shift, AFSthre_deint, AFSthre_motion_y, AFSthre_motion_c, AFSlevel}

                    i.NumEdit.Enabled = AFS.Value
                Next

                For Each i In {AFSshift, AFSdrop, AFSsmooth, AFS24fps, AFStune, AFSrff, AFStimecode, AFSlog}
                    i.CheckBox.Enabled = AFS.Value
                Next
            End If

            MyBase.OnValueChanged(item)
        End Sub

        Function GetPmdArgs() As String
            If PMD.Value Then
                Dim ret = ""

                If PmdApplyCount.Value <> PmdApplyCount.DefaultValue Then ret += ",apply_count=" & PmdApplyCount.Value
                If PmdStrength.Value <> PmdStrength.DefaultValue Then ret += ",strength=" & PmdStrength.Value
                If PmdThreshold.Value <> PmdThreshold.DefaultValue Then ret += ",threshold=" & PmdThreshold.Value

                If ret <> "" Then
                    Return "--vpp-pmd " + ret.TrimStart(","c)
                Else
                    Return "--vpp-pmd"
                End If
            End If
        End Function

        Function GetTweakArgs() As String
            If Tweak.Value Then
                Dim ret = ""

                If vppbrightness.Value <> vppbrightness.DefaultValue Then ret += "brightness=" & vppbrightness.Value
                If vppcontrast.Value <> vppcontrast.DefaultValue Then ret += ",contrast=" & vppcontrast.Value
                If vppsaturation.Value <> vppsaturation.DefaultValue Then ret += ",saturation=" & vppsaturation.Value
                If vppgamma.Value <> vppgamma.DefaultValue Then ret += ",gamma=" & vppgamma.Value
                If vpphue.Value <> vpphue.DefaultValue Then ret += ",hue=" & vpphue.Value

                If ret <> "" Then
                    Return "--vpp-tweak " + ret.TrimStart(","c)
                Else
                    Return "--vpp-tweak"
                End If
            End If
        End Function

        Function GetPaddingArgs() As String
            If Pad.Value Then
                Dim ret = ""

                If PadLeft.Value <> PadLeft.DefaultValue Then ret += "" & PadLeft.Value
                If PadTop.Value <> PadTop.DefaultValue Then ret += "," & PadTop.Value
                If PadRight.Value <> PadRight.DefaultValue Then ret += "," & PadRight.Value
                If PadBottom.Value <> PadBottom.DefaultValue Then ret += "," & PadBottom.Value

                If ret <> "" Then
                    Return "--vpp-pad " + ret.TrimStart(","c)
                Else
                    Return "--vpp-pad "
                End If
            End If
        End Function

        Function GetKnnArgs() As String
            If KNN.Value Then
                Dim ret = ""

                If KnnRadius.Value <> KnnRadius.DefaultValue Then ret += ",radius=" & KnnRadius.Value
                If KnnStrength.Value <> KnnStrength.DefaultValue Then ret += ",strength=" & KnnStrength.Value.ToInvariantString
                If KnnLerp.Value <> KnnLerp.DefaultValue Then ret += ",lerp=" & KnnLerp.Value.ToInvariantString
                If KnnThLerp.Value <> KnnThLerp.DefaultValue Then ret += ",th_lerp=" & KnnThLerp.Value.ToInvariantString

                If ret <> "" Then
                    Return "--vpp-knn " + ret.TrimStart(","c)
                Else
                    Return "--vpp-knn"
                End If
            End If
        End Function

        Function GetDebandArgs() As String
            Dim ret = ""

            For Each i In {Deband_range, Deband_sample, Deband_thre, Deband_thre_y, Deband_thre_cb,
                Deband_thre_cr, Deband_dither, Deband_dither_y, Deband_dither_c, Deband_seed}

                If i.Value <> i.DefaultValue Then ret += "," + i.Text.Trim + "=" & i.Value
            Next

            If Deband_blurfirst.Value Then ret += "," + "blurfirst"
            If Deband_rand_each_frame.Value Then ret += "," + "rand_each_frame"
            If Deband.Value Then Return ("--vpp-deband " + ret.TrimStart(","c)).TrimEnd
        End Function

        Function GetUnsharp() As String
            Dim ret = ""

            If VppUnsharpRadius.Value <> VppUnsharpRadius.DefaultValue Then ret += "radius=" & VppUnsharpRadius.Value
            If VppUnsharpWeight.Value <> VppUnsharpWeight.DefaultValue Then ret += ",weight=" & VppUnsharpWeight.Value
            If VppUnsharpThreshold.Value <> VppUnsharpThreshold.DefaultValue Then ret += ",threshold=" & VppUnsharpThreshold.Value

            If VppUnsharp.Value Then Return ("--vpp-unsharp " + ret.TrimStart(","c)).TrimEnd
        End Function

        Function GetEdge() As String
            Dim ret = ""

            If VppEdgelevelStrength.Value <> VppEdgelevelStrength.DefaultValue Then ret += "strength=" & VppEdgelevelStrength.Value
            If VppEdgelevelThreshold.Value <> VppEdgelevelThreshold.DefaultValue Then ret += ",threshold=" & VppEdgelevelThreshold.Value
            If VppEdgelevelBlack.Value <> VppEdgelevelBlack.DefaultValue Then ret += ",black=" & VppEdgelevelBlack.Value
            If VppEdgelevelWhite.Value <> VppEdgelevelWhite.DefaultValue Then ret += ",white=" & VppEdgelevelWhite.Value

            If VppEdgelevel.Value Then Return ("--vpp-edgelevel " + ret.TrimStart(","c)).TrimEnd
        End Function

        Function GetSelectEvery() As String
            Dim ret = ""

            ret += VppSelectEveryValue.Value.ToString
            ret += "," + VppSelectEveryOffsets.Value.SplitNoEmptyAndWhiteSpace(" ", ",", ";").Select(Function(item) "offset=" + item).Join(",")

            If VppSelectEvery.Value Then Return ("--vpp-select-every " + ret.TrimStart(","c)).TrimEnd(","c)
        End Function

        Function GetNnedi() As String
            Dim ret = ""
            If VppNnediField.Value <> VppNnediField.DefaultValue Then ret += "field=" + VppNnediField.ValueText
            If VppNnediNns.Value <> VppNnediNns.DefaultValue Then ret += ",nns=" + VppNnediNns.ValueText
            If VppNnediNszie.Value <> VppNnediNszie.DefaultValue Then ret += ",nszie=" + VppNnediNszie.ValueText
            If VppNnediQuality.Value <> VppNnediQuality.DefaultValue Then ret += ",quality=" + VppNnediQuality.ValueText
            If VppNnediPrescreen.Value <> VppNnediPrescreen.DefaultValue Then ret += ",prescreen=" + VppNnediPrescreen.ValueText
            If VppNnediErrortype.Value <> VppNnediErrortype.DefaultValue Then ret += ",errortype=" + VppNnediErrortype.ValueText
            If VppNnediPrec.Value <> VppNnediPrec.DefaultValue Then ret += ",prec=" + VppNnediPrec.ValueText
            If VppNnediWeightfile.Value <> "" Then ret += ",weightfile=" + VppNnediWeightfile.Value.Escape

            If VppNnedi.Value Then Return ("--vpp-nnedi " + ret.TrimStart(","c)).TrimEnd
        End Function

        Function GetAFS() As String
            Dim ret = ""

            If AFSPreset.Value <> AFSPreset.DefaultValue Then ret += "preset=" + AFSPreset.ValueText
            If AFSINI.Value <> "" Then ret += ",ini=" + AFSINI.Value.Escape

            For Each i In {AFSLeft, AFSRight, AFSTop, AFSBottom, AFSmethod_switch, AFScoeff_shift,
                               AFSthre_shift, AFSthre_deint, AFSthre_motion_y, AFSthre_motion_c, AFSlevel}

                If i.Value <> i.DefaultValue Then ret += "," + i.Text + "=" & i.Value
            Next

            For Each i In {AFSshift, AFSdrop, AFSsmooth, AFS24fps, AFStune, AFSrff, AFStimecode, AFSlog}
                If i.Value <> i.DefaultValue Then ret += "," + i.Text + "=" + If(i.Value, "on", "off")
            Next

            If AFS.Value Then Return ("--vpp-afs " + ret.TrimStart(","c)).TrimEnd
        End Function

        Overrides Function GetCommandLine(includePaths As Boolean,
                                          includeExe As Boolean,
                                          Optional pass As Integer = 1) As String
            Dim ret As String
            Dim sourcePath As String
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)

            If includePaths AndAlso includeExe Then ret = Package.NVEnc.Path.Escape

            Select Case Decoder.ValueText
                Case "avs"
                    sourcePath = p.Script.Path
                Case "nvnative"
                    sourcePath = p.LastOriginalSourceFile
                Case "nvcuda"
                    sourcePath = p.LastOriginalSourceFile
                    ret += " --avhw cuda"
                Case "qs"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.QSVEnc.Path.Escape, "QSVEncC64") + " -o - -c raw" + " -i " + If(includePaths, p.SourceFile.Escape, "path") + " | " + If(includePaths, Package.NVEnc.Path.Escape, "NVEncC64")
                Case "ffdxva"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.SourceFile.Escape, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel fatal -hide_banner - | " + If(includePaths, Package.NVEnc.Path.Escape, "NVEncC64")
                Case "ffqsv"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.SourceFile.Escape, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel fatal -hide_banner - | " + If(includePaths, Package.NVEnc.Path.Escape, "NVEncC64")
            End Select

            Dim q = From i In Items Where i.GetArgs <> ""
            If q.Count > 0 Then ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")

            If (p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 AndAlso
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
            If includePaths Then ret += " -i " + sourcePath.Escape + " -o " + targetPath.Escape

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.NVEnc
        End Function
    End Class
End Class