
Imports StaxRip.VideoEncoderCommandLine

<Serializable()>
Public Class VCEEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "AMD | " + Params.Codec.OptionText.Replace("AMD ", "")
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
        Dim store = ObjectHelp.GetCopy(ParamsStore)
        params1.Init(store)

        Using form As New CommandLineForm(params1)
            Dim a = Sub()
                        Dim enc = ObjectHelp.GetCopy(Me)
                        Dim params2 As New EncoderParams
                        Dim store2 = ObjectHelp.GetCopy(store)
                        params2.Init(store2)
                        enc.Params = params2
                        enc.ParamsStore = store2
                        SaveProfile(enc)
                    End Sub

            form.cms.Add("Check Hardware", Sub() g.ShowCode("Check Hardware", ProcessHelp.GetConsoleOutput(Package.VCEEnc.Path, "--check-hw")))
            form.cms.Add("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetConsoleOutput(Package.VCEEnc.Path, "--check-features")), Keys.Control Or Keys.F)
            form.cms.Add("-")
            form.cms.Add("Save Profile...", a, Keys.Control Or Keys.S, Symbol.Save)

            If form.ShowDialog() = DialogResult.OK Then
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
        Dim ret As New MenuList
        ret.Add("Encoder Options", AddressOf ShowConfigDialog)
        ret.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return ret
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

    Shared Function Test() As String
        Dim tester As New ConsolAppTester

        tester.IgnoredSwitches = "audio-bitrate audio-codec video-streamid video-track vpy vpy-mt
            audio-filter avsw device input-analyze caption2ass audio-file sub-copy version audio-copy
            audio-ignore-decode-error audio-ignore-notrack-error audio-resampler raw help input-file
            audio-samplerate audio-source audio-stream avs avvce-analyze output-file seek skip-frame
            check-avversion check-codecs check-decoders check-encoders check-filters check-protocols
            check-formats dar format fps input-res log-framelist mux-option
            vpp-delogo vpp-delogo-cb vpp-delogo-cr vpp-delogo-depth output
            vpp-delogo-pos vpp-delogo-select vpp-delogo-y"

        tester.Package = Package.VCEEnc
        tester.CodeFile = Folder.Startup.Parent + "Encoding\vceenc.vb"

        Return tester.Test
    End Function

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

        Property Pa As New BoolParam With {.Switch = "--pa", .Text = "Pre-Analysis to enhance quality", .ArgsFunc = AddressOf GetPaArgs}
        Property PaSc As New OptionParam With {.Text = "      Sensitivity of scene change detection", .HelpSwitch = "--pa-sc", .Init = 2, .Options = {"None", "Low", "Medium", "High"}}
        Property PaSs As New OptionParam With {.Text = "      Sensitivity of static scene detection", .HelpSwitch = "--pa-ss", .Init = 2, .Options = {"None", "Low", "Medium", "High"}}
        Property PaActivityType As New OptionParam With {.Text = "      Block activity calcualtion mode", .HelpSwitch = "--pa-activity-type", .Init = 0, .Options = {"Y", "YUV"}}
        Property PaCaqStrength As New OptionParam With {.Text = "      Content Adaptive Quantization (CAQ) strength", .HelpSwitch = "--pa-caq-strength", .Init = 1, .Options = {"Low", "Medium", "High"}}
        Property PaInitqpsc As New NumParam With {.Text = "      Initial qp after scene change", .HelpSwitch = "--pa-initqpsc", .Init = -1.0, .Config = {-1.0, 100.0, 1, 0}}
        Property PaFskipMaxqp As New NumParam With {.Text = "      Threshold to insert skip frame on static scene", .HelpSwitch = "--pa-fskip-maxqp", .Init = 35.0, .Config = {0.0, 100.0, 1, 0}}


        Property Tweak As New BoolParam With {.Switch = "--vpp-tweak", .Text = "Tweaking", .ArgsFunc = AddressOf GetTweakArgs}
        Property TweakContrast As New NumParam With {.Text = "      Contrast", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {-2.0, 2.0, 0.1, 1}}
        Property TweakGamma As New NumParam With {.Text = "      Gamma", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {0.1, 10.0, 0.1, 1}}
        Property TweakSaturation As New NumParam With {.Text = "      Saturation", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {0.0, 3.0, 0.1, 1}}
        Property TweakHue As New NumParam With {.Text = "      Hue", .HelpSwitch = "--vpp-tweak", .Config = {-180.0, 180.0, 0.1, 1}}
        Property TweakBrightness As New NumParam With {.Text = "      Brightness", .HelpSwitch = "--vpp-tweak", .Config = {-1.0, 1.0, 0.1, 1}}

        Property Pad As New BoolParam With {.Switch = "--vpp-pad", .Text = "Padding", .ArgsFunc = AddressOf GetPaddingArgs}
        Property PadLeft As New NumParam With {.Text = "      Left"}
        Property PadTop As New NumParam With {.Text = "      Top"}
        Property PadRight As New NumParam With {.Text = "      Right"}
        Property PadBottom As New NumParam With {.Text = "      Bottom"}

        Property Smooth As New BoolParam With {.Text = "Smooth", .Switch = "--vpp-smooth", .ArgsFunc = AddressOf GetSmoothArgs}
        Property SmoothQuality As New NumParam With {.Text = "      Quality", .HelpSwitch = "--vpp-smooth", .Init = 3, .Config = {1, 6}}
        Property SmoothQP As New NumParam With {.Text = "      QP", .HelpSwitch = "--vpp-smooth", .Config = {0, 100, 10, 1}}
        Property SmoothPrec As New OptionParam With {.Text = "      Precision", .HelpSwitch = "--vpp-smooth", .Options = {"Auto", "FP16", "FP32"}}

        Property TransformFlipX As New BoolParam With {.Switch = "--vpp-transform", .Text = "Flip X", .Label = "Transform", .LeftMargin = g.MainForm.FontHeight * 1.5, .ArgsFunc = AddressOf GetTransform}
        Property TransformFlipY As New BoolParam With {.Text = "Flip Y", .LeftMargin = g.MainForm.FontHeight * 1.5, .HelpSwitch = "--vpp-transform"}
        Property TransformTranspose As New BoolParam With {.Text = "Transpose", .LeftMargin = g.MainForm.FontHeight * 1.5, .HelpSwitch = "--vpp-transform"}

        Property Colorspace As New BoolParam With {.Text = "Colorspace", .Switch = "--vpp-colorspace", .ArgsFunc = AddressOf GetColorspaceArgs}
        Property ColorspaceMatrixFrom As New OptionParam With {.Text = New String(" "c, 6) + "Matrix From", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"Undefined", "auto", "bt709", "smpte170m", "bt470bg", "smpte240m", "YCgCo", "fcc", "GBR", "bt2020nc", "bt2020c"}}
        Property ColorspaceMatrixTo As New OptionParam With {.Text = New String(" "c, 12) + "Matrix To", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"auto", "bt709", "smpte170m", "bt470bg", "smpte240m", "YCgCo", "fcc", "GBR", "bt2020nc", "bt2020c"}, .VisibleFunc = Function() ColorspaceMatrixFrom.Value > 0}
        Property ColorspaceColorprimFrom As New OptionParam With {.Text = New String(" "c, 6) + "Colorprim From", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"Undefined", "auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "film", "bt2020"}}
        Property ColorspaceColorprimTo As New OptionParam With {.Text = New String(" "c, 12) + "Colorprim To", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "film", "bt2020"}, .VisibleFunc = Function() ColorspaceColorprimFrom.Value > 0}
        Property ColorspaceTransferFrom As New OptionParam With {.Text = New String(" "c, 6) + "Transfer From", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"Undefined", "auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "linear", "log100", "log316", "iec61966-2-4", "iec61966-2-1", "bt2020-10", "bt2020-12", "smpte2084", "arib-std-b67"}}
        Property ColorspaceTransferTo As New OptionParam With {.Text = New String(" "c, 12) + "Transfer To", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "linear", "log100", "log316", "iec61966-2-4", "iec61966-2-1", "bt2020-10", "bt2020-12", "smpte2084", "arib-std-b67"}, .VisibleFunc = Function() ColorspaceTransferFrom.Value > 0}
        Property ColorspaceRangeFrom As New OptionParam With {.Text = New String(" "c, 6) + "Range From", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"Undefined", "auto", "limited", "full"}}
        Property ColorspaceRangeTo As New OptionParam With {.Text = New String(" "c, 12) + "Range To", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"auto", "limited", "full"}, .VisibleFunc = Function() ColorspaceRangeFrom.Value > 0}
        Property ColorspaceHdr2sdr As New OptionParam With {.Text = New String(" "c, 0) + "HDR10 to SDR using this tonemapping:", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"none", "hable", "mobius", "reinhard", "bt2390"}}
        Property ColorspaceHdr2sdrSourcepeak As New NumParam With {.Text = New String(" "c, 6) + "Source Peak", .HelpSwitch = "--vpp-colorspace", .Init = 1000, .Config = {0, 10000, 1, 1}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value > 0}
        Property ColorspaceHdr2sdrLdrnits As New NumParam With {.Text = New String(" "c, 6) + "Target brightness", .HelpSwitch = "--vpp-colorspace", .Init = 100.0, .Config = {0, 1000, 1, 1}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value > 0}
        Property ColorspaceHdr2sdrDesatbase As New NumParam With {.Text = New String(" "c, 6) + "Offset for desaturation curve", .HelpSwitch = "--vpp-colorspace", .Init = 0.18, .Config = {0, 10, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value > 0}
        Property ColorspaceHdr2sdrDesatstrength As New NumParam With {.Text = New String(" "c, 6) + "Strength of desaturation curve", .HelpSwitch = "--vpp-colorspace", .Init = 0.75, .Config = {0, 10, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value > 0}
        Property ColorspaceHdr2sdrDesatexp As New NumParam With {.Text = New String(" "c, 6) + "Exponent of the desaturation curve", .HelpSwitch = "--vpp-colorspace", .Init = 1.5, .Config = {0, 100, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value > 0}
        Property ColorspaceHdr2sdrHableA As New NumParam With {.Text = New String(" "c, 6) + "a", .HelpSwitch = "--vpp-colorspace", .Init = 0.22, .Config = {0, 1, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 1}
        Property ColorspaceHdr2sdrHableB As New NumParam With {.Text = New String(" "c, 6) + "b", .HelpSwitch = "--vpp-colorspace", .Init = 0.3, .Config = {0, 1, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 1}
        Property ColorspaceHdr2sdrHableC As New NumParam With {.Text = New String(" "c, 6) + "c", .HelpSwitch = "--vpp-colorspace", .Init = 0.1, .Config = {0, 1, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 1}
        Property ColorspaceHdr2sdrHableD As New NumParam With {.Text = New String(" "c, 6) + "d", .HelpSwitch = "--vpp-colorspace", .Init = 0.2, .Config = {0, 1, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 1}
        Property ColorspaceHdr2sdrHableE As New NumParam With {.Text = New String(" "c, 6) + "e", .HelpSwitch = "--vpp-colorspace", .Init = 0.01, .Config = {0, 1, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 1}
        Property ColorspaceHdr2sdrHableF As New NumParam With {.Text = New String(" "c, 6) + "f", .HelpSwitch = "--vpp-colorspace", .Init = 0.3, .Config = {0, 1, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 1}
        Property ColorspaceHdr2sdrMobiusTransition As New NumParam With {.Text = New String(" "c, 6) + "Transition", .HelpSwitch = "--vpp-colorspace", .Init = 0.3, .Config = {0, 10, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 2}
        Property ColorspaceHdr2sdrMobiusPeak As New NumParam With {.Text = New String(" "c, 6) + "Peak", .HelpSwitch = "--vpp-colorspace", .Init = 1.0, .Config = {0, 100, 0.05, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 2, .Name = "MobiusPeak"}
        Property ColorspaceHdr2sdrReinhardContrast As New NumParam With {.Text = New String(" "c, 6) + "Contrast", .HelpSwitch = "--vpp-colorspace", .Init = 0.5, .Config = {0, 1, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 3}
        Property ColorspaceHdr2sdrReinhardPeak As New NumParam With {.Text = New String(" "c, 6) + "Peak", .HelpSwitch = "--vpp-colorspace", .Init = 1.0, .Config = {0, 100, 0.05, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 3, .Name = "ReinhardPeak"}

        Property Deband As New BoolParam With {.Text = "Deband", .Switch = "--vpp-deband", .ArgsFunc = AddressOf GetDebandArgs}
        Property DebandRange As New NumParam With {.Text = "     Range", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 127}}
        Property DebandSample As New NumParam With {.Text = "     Sample", .HelpSwitch = "--vpp-deband", .Init = 1, .Config = {0, 2}}
        Property DebandThre As New NumParam With {.Text = "     Threshold", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandThreY As New NumParam With {.Text = "          Y", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandThreCB As New NumParam With {.Text = "          CB", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandThreCR As New NumParam With {.Text = "          CR", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandDither As New NumParam With {.Text = "     Dither", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandDitherY As New NumParam With {.Text = "          Y", .HelpSwitch = "--vpp-deband", .Name = "vpp-deband_dither_y", .Init = 15, .Config = {0, 31}}
        Property DebandDitherC As New NumParam With {.Text = "          C", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandSeed As New NumParam With {.Text = "     Seed", .HelpSwitch = "--vpp-deband", .Init = 1234}
        Property DebandBlurfirst As New BoolParam With {.Text = "Blurfirst", .HelpSwitch = "--vpp-deband", .LeftMargin = g.MainForm.FontHeight * 1.3}
        Property DebandRandEachFrame As New BoolParam With {.Text = "Rand Each Frame", .HelpSwitch = "--vpp-deband", .LeftMargin = g.MainForm.FontHeight * 1.3}

        Property Deinterlacer As New OptionParam With {.Text = "Deinterlacing Method", .HelpSwitch = "", .Init = 0, .Options = {"None", "AFS (Activate Auto Field Shift)", "Nnedi"}, .ArgsFunc = AddressOf GetDeinterlacerArgs}

        Property AfsPreset As New OptionParam With {.Text = "Preset", .HelpSwitch = "--vpp-afs", .Options = {"Default", "Triple", "Double", "Anime", "Cinema", "Min_afterimg", "24fps", "24fps_sd", "30fps"}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsINI As New StringParam With {.Text = "INI", .HelpSwitch = "--vpp-afs", .BrowseFile = True, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsLeft As New NumParam With {.Text = "Left", .HelpSwitch = "--vpp-afs", .Init = 32, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsRight As New NumParam With {.Text = "Right", .HelpSwitch = "--vpp-afs", .Init = 32, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsTop As New NumParam With {.Text = "Top", .HelpSwitch = "--vpp-afs", .Init = 16, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsBottom As New NumParam With {.Text = "Bottom", .HelpSwitch = "--vpp-afs", .Init = 16, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsMethodSwitch As New NumParam With {.Text = "Method Switch", .HelpSwitch = "--vpp-afs", .Config = {0, 256}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsCoeffShift As New NumParam With {.Text = "Coeff Shift", .HelpSwitch = "--vpp-afs", .Init = 192, .Config = {0, 256}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsThreShift As New NumParam With {.Text = "Threshold Shift", .HelpSwitch = "--vpp-afs", .Init = 128, .Config = {0, 1024}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsThreDeint As New NumParam With {.Text = "Threshold Deint", .HelpSwitch = "--vpp-afs", .Init = 48, .Config = {0, 1024}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsThreMotionY As New NumParam With {.Text = "Threshold Motion Y", .HelpSwitch = "--vpp-afs", .Init = 112, .Config = {0, 1024}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsThreMotionC As New NumParam With {.Text = "Threshold Motion C", .HelpSwitch = "--vpp-afs", .Init = 224, .Config = {0, 1024}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsLevel As New NumParam With {.Text = "Level", .HelpSwitch = "--vpp-afs", .Init = 3, .Config = {0, 4}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsShift As New BoolParam With {.Text = "Shift", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsDrop As New BoolParam With {.Text = "Drop", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsSmooth As New BoolParam With {.Text = "Smooth", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property Afs24fps As New BoolParam With {.Text = "24 FPS", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsTune As New BoolParam With {.Text = "Tune", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsRFF As New BoolParam With {.Text = "RFF", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsTimecode As New BoolParam With {.Text = "Timecode", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsLog As New BoolParam With {.Text = "Log", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}

        Property NnediField As New OptionParam With {.Text = "Field", .HelpSwitch = "--vpp-nnedi", .Options = {"auto", "top", "bottom"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property NnediNns As New OptionParam With {.Text = "NNS", .HelpSwitch = "--vpp-nnedi", .Init = 1, .Options = {"16", "32", "64", "128", "256"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property NnediNsize As New OptionParam With {.Text = "N Size", .HelpSwitch = "--vpp-nnedi", .Init = 6, .Options = {"8x6", "16x6", "32x6", "48x6", "8x4", "16x4", "32x4"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property NnediQuality As New OptionParam With {.Text = "Quality", .HelpSwitch = "--vpp-nnedi", .Options = {"fast", "slow"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property NnediPrescreen As New OptionParam With {.Text = "Pre Screen", .HelpSwitch = "--vpp-nnedi", .Init = 4, .Options = {"none", "original", "new", "original_block", "new_block"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property NnediErrortype As New OptionParam With {.Text = "Error Type", .HelpSwitch = "--vpp-nnedi", .Options = {"abs", "square"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property NnediPrec As New OptionParam With {.Text = "Prec", .HelpSwitch = "--vpp-nnedi", .Options = {"auto", "fp16", "fp32"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property NnediWeightfile As New StringParam With {.Text = "Weight File", .HelpSwitch = "--vpp-nnedi", .BrowseFile = True, .VisibleFunc = Function() Deinterlacer.Value = 2}

        Property Edgelevel As New BoolParam With {.Text = "Edgelevel filter to enhance edge", .Switches = {"--vpp-edgelevel"}, .ArgsFunc = AddressOf GetEdge}
        Property EdgelevelStrength As New NumParam With {.Text = "     Strength", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}
        Property EdgelevelThreshold As New NumParam With {.Text = "     Threshold", .HelpSwitch = "--vpp-edgelevel", .Init = 20, .Config = {0, 255, 1, 1}}
        Property EdgelevelBlack As New NumParam With {.Text = "     Black", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}
        Property EdgelevelWhite As New NumParam With {.Text = "     White", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}

        Property Unsharp As New BoolParam With {.Text = "Unsharp Filter", .Switches = {"--vpp-unsharp"}, .ArgsFunc = AddressOf GetUnsharp}
        Property UnsharpRadius As New NumParam With {.Text = "     Radius", .HelpSwitch = "--vpp-unsharp", .Init = 3, .Config = {1, 9}}
        Property UnsharpWeight As New NumParam With {.Text = "     Weight", .HelpSwitch = "--vpp-unsharp", .Init = 0.5, .Config = {0, 10, 0.5, 1}}
        Property UnsharpThreshold As New NumParam With {.Text = "     Threshold", .HelpSwitch = "--vpp-unsharp", .Init = 10, .Config = {0, 255, 1, 1}}

        Property Warpsharp As New BoolParam With {.Text = "Warpsharp filter", .Switches = {"--vpp-warpsharp"}, .ArgsFunc = AddressOf GetWarpsharpArgs}
        Property WarpsharpThreshold As New NumParam With {.Text = "     Threshold", .HelpSwitch = "--vpp-warpsharp", .Init = 128, .Config = {0, 255, 1, 1}}
        Property WarpsharpBlur As New NumParam With {.Text = "     Blur", .HelpSwitch = "--vpp-warpsharp", .Init = 2, .Config = {0, 30, 1, 0}}
        Property WarpsharpType As New NumParam With {.Text = "     Type", .HelpSwitch = "--vpp-warpsharp", .Init = 0, .Config = {0, 1, 1, 0}}
        Property WarpsharpDepth As New NumParam With {.Text = "     Depth", .HelpSwitch = "--vpp-warpsharp", .Init = 16, .Config = {-128, 128, 1, 1}}
        Property WarpsharpChroma As New NumParam With {.Text = "     Chroma", .HelpSwitch = "--vpp-warpsharp", .Init = 0, .Config = {0, 1, 1, 0}}

        Property Pmd As New BoolParam With {.Switch = "--vpp-pmd", .Text = "Denoise using PMD", .ArgsFunc = AddressOf GetPmdArgs}
        Property PmdApplyCount As New NumParam With {.Text = "      Apply Count", .Init = 2}
        Property PmdStrength As New NumParam With {.Text = "      Strength", .Name = "PmdStrength", .Init = 100.0, .Config = {0, 100, 1, 1}}
        Property PmdThreshold As New NumParam With {.Text = "      Threshold", .Init = 100.0, .Config = {0, 255, 1, 1}}

        Property Knn As New BoolParam With {.Switch = "--vpp-knn", .Text = "Denoise using K-nearest neighbor", .ArgsFunc = AddressOf GetKnnArgs}
        Property KnnRadius As New NumParam With {.Text = "      Radius", .Init = 3}
        Property KnnStrength As New NumParam With {.Text = "      Strength", .Init = 0.08, .Config = {0, 1, 0.02, 2}}
        Property KnnLerp As New NumParam With {.Text = "      Lerp", .Init = 0.2, .Config = {0, Integer.MaxValue, 0.1, 1}}
        Property KnnThLerp As New NumParam With {.Text = "      TH Lerp", .Init = 0.8, .Config = {0, 1, 0.1, 1}}


        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)

                    Add("Basic", Decoder, Mode, Codec,
                        New OptionParam With {.Switch = "--output-depth", .Text = "Depth", .Options = {"8-Bit", "10-Bit"}, .Values = {"8", "10"}},
                        New OptionParam With {.Switch = "--quality", .Text = "Preset", .Options = {"Fast", "Balanced", "Slow"}, .Init = 1},
                        New OptionParam With {.Switch = "--profile", .Name = "profile264", .VisibleFunc = Function() Codec.ValueText = "h264", .Text = "Profile", .Options = {"Automatic", "Baseline", "Main", "High"}},
                        New OptionParam With {.Switch = "--profile", .Name = "profile265", .VisibleFunc = Function() Codec.ValueText = "hevc", .Text = "Profile", .Options = {"Main"}},
                        New OptionParam With {.Switch = "--level", .Name = "LevelH264", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "h264", .Options = {"Unrestricted", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                        New OptionParam With {.Switch = "--level", .Name = "LevelH265", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "hevc", .Options = {"Unrestricted", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}},
                        New OptionParam With {.Switch = "--tier", .Text = "Tier", .Options = {"Main", "High"}, .VisibleFunc = Function() Codec.ValueText = "hevc"},
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
                        New NumParam With {.Switch = "--bref-deltaqp", .Text = "Ref Bframe QP Offset"})
                    Add("Pre...",
                        New BoolParam With {.Switch = "--pe", .Text = "Pre-Encode assisted rate control"},
                        Pa, PaSc, PaSs, PaActivityType, PaCaqStrength, PaInitqpsc, PaFskipMaxqp)
                    Add("VPP | Misc",
                        New StringParam With {.Switch = "--vpp-subburn", .Text = "Subburn"},
                        New OptionParam With {.Switch = "--vpp-resize", .Text = "Resize", .Options = {"Disabled", "Default", "Bilinear", "Cubic", "Cubic_B05C03", "Cubic_bSpline", "Cubic_Catmull", "Lanczos", "NN", "NPP_Linear", "Spline 36", "Super"}},
                        New OptionParam With {.Switch = "--vpp-rotate", .Text = "Rotate", .Options = {"Disabled", "90", "180", "270"}})
                    Add("VPP | Misc 2",
                        Tweak, TweakBrightness, TweakContrast, TweakSaturation, TweakGamma, TweakHue,
                        Pad, PadLeft, PadTop, PadRight, PadBottom,
                        Smooth, SmoothQuality, SmoothQP, SmoothPrec)
                    Add("VPP | Misc 3",
                        TransformFlipX, TransformFlipY, TransformTranspose,
                        New StringParam With {.Switch = "--vpp-decimate", .Text = "Decimate"},
                        New StringParam With {.Switch = "--vpp-mpdecimate", .Text = "MP Decimate"})
                    Add("VPP | Colorspace",
                        Colorspace,
                        ColorspaceMatrixFrom, ColorspaceMatrixTo,
                        ColorspaceColorprimFrom, ColorspaceColorprimTo,
                        ColorspaceTransferFrom, ColorspaceTransferTo,
                        ColorspaceRangeFrom, ColorspaceRangeTo)
                    Add("VPP | Colorspace | HDR2SDR",
                        ColorspaceHdr2sdr,
                        ColorspaceHdr2sdrSourcepeak, ColorspaceHdr2sdrLdrnits, ColorspaceHdr2sdrDesatbase, ColorspaceHdr2sdrDesatstrength, ColorspaceHdr2sdrDesatexp,
                        ColorspaceHdr2sdrHableA, ColorspaceHdr2sdrHableB, ColorspaceHdr2sdrHableC, ColorspaceHdr2sdrHableD, ColorspaceHdr2sdrHableE, ColorspaceHdr2sdrHableF,
                        ColorspaceHdr2sdrMobiusTransition, ColorspaceHdr2sdrMobiusPeak, ColorspaceHdr2sdrReinhardContrast, ColorspaceHdr2sdrReinhardPeak)
                    Add("VPP | Deband",
                        Deband, DebandRange, DebandSample, DebandThre, DebandThreY, DebandThreCB, DebandThreCR,
                        DebandDither, DebandDitherY, DebandDitherC, DebandSeed, DebandBlurfirst, DebandRandEachFrame)
                    Add("VPP | Deinterlace",
                        Deinterlacer,
                        NnediField, NnediNns, NnediNsize, NnediQuality, NnediPrescreen, NnediErrortype, NnediPrec, NnediWeightfile,
                        AfsINI, AfsPreset, AfsLeft, AfsRight, AfsTop, AfsBottom, AfsMethodSwitch, AfsCoeffShift, AfsThreShift, AfsThreDeint, AfsThreMotionY, AfsThreMotionC, AfsLevel)
                    Add("VPP | Deinterlace | AFS 2",
                        AfsShift, AfsDrop, AfsSmooth, Afs24fps, AfsTune, AfsRFF, AfsTimecode, AfsLog)
                    Add("VPP | Denoise",
                        Knn, KnnRadius, KnnStrength, KnnLerp, KnnThLerp,
                        Pmd, PmdApplyCount, PmdStrength, PmdThreshold)
                    Add("VPP | Sharpness",
                        Edgelevel, EdgelevelStrength, EdgelevelThreshold, EdgelevelBlack, EdgelevelWhite,
                        Unsharp, UnsharpRadius, UnsharpWeight, UnsharpThreshold,
                        Warpsharp, WarpsharpThreshold, WarpsharpBlur, WarpsharpType, WarpsharpDepth, WarpsharpChroma)
                    Add("VUI",
                        New StringParam With {.Switch = "--sar", .Text = "Sample Aspect Ratio", .Init = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                        New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat", .Options = {"Undefined", "NTSC", "Component", "PAL", "SECAM", "MAC"}},
                        New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix", .Options = {"Undefined", "BT 2020 C", "BT 2020 NC", "BT 470 BG", "BT 709", "FCC", "GBR", "SMPTE 170 M", "SMPTE 240 M", "YCgCo"}},
                        New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim", .Options = {"Undefined", "BT 2020", "BT 470 BG", "BT 470 M", "BT 709", "Film", "SMPTE 170 M", "SMPTE 240 M"}},
                        New OptionParam With {.Switch = "--transfer", .Text = "Transfer", .Options = {"Undefined", "ARIB-STD-B67", "Auto", "BT 1361 E", "BT 2020-10", "BT 2020-12", "BT 470 BG", "BT 470 M", "BT 709", "IEC 61966-2-1", "IEC 61966-2-4", "Linear", "Log 100", "Log 316", "SMPTE 170 M", "SMPTE 240 M", "SMPTE 2084", "SMPTE 428"}},
                        New BoolParam With {.Switch = "--enforce-hrd", .Text = "Enforce HRD compatibility"})
                    Add("Statistic",
                        New BoolParam With {.Switch = "--ssim", .Text = "SSIM"},
                        New BoolParam With {.Switch = "--psnr", .Text = "PSNR"})
                    Add("Other",
                        New StringParam With {.Text = "Custom", .Quotes = QuotesMode.Never, .AlwaysOn = True},
                        New StringParam With {.Switch = "--chapter", .Text = "Chapters", .BrowseFile = True},
                        New StringParam With {.Switch = "--log", .Text = "Log File", .BrowseFile = True},
                        New StringParam With {.Switch = "--timecode", .Text = "Timecode File", .BrowseFile = True},
                        New OptionParam With {.Switch = "--log-level", .Text = "Log Level", .Options = {"Info", "Debug", "Warn", "Error"}},
                        New OptionParam With {.Switch = "--motion-est", .Text = "Motion Estimation", .Options = {"Q-pel", "Full-pel", "Half-pel"}},
                        New NumParam With {.Switch = "--input-analyze", .Text = "Input Analyze", .Init = 5, .Config = {1, 600, 0.1, 1}},
                        New BoolParam With {.Switch = "--chapter-copy", .Text = "Copy Chapters"},
                        New BoolParam With {.Switch = "--vbaq", .Text = "Adaptive quantization in frame"},
                        New BoolParam With {.Switch = "--filler", .Text = "Use filler data"})
                End If

                Return ItemsValue
            End Get
        End Property


        Protected Overrides Sub OnValueChanged(item As CommandLineParam)
            If Decoder.MenuButton IsNot Nothing AndAlso (item Is Decoder OrElse item Is Nothing) Then
                Dim isIntelPresent = OS.VideoControllers.Where(Function(val) val.Contains("Intel")).Count > 0

                For x = 0 To Decoder.Options.Length - 1
                    If Decoder.Options(x).Contains("Intel") Then
                        Decoder.ShowOption(x, isIntelPresent)
                    End If
                Next
            End If

            If QPI.NumEdit IsNot Nothing Then
                ColorspaceMatrixFrom.MenuButton.Enabled = Colorspace.Value
                ColorspaceMatrixTo.MenuButton.Enabled = Colorspace.Value
                ColorspaceColorprimFrom.MenuButton.Enabled = Colorspace.Value
                ColorspaceColorprimTo.MenuButton.Enabled = Colorspace.Value
                ColorspaceTransferFrom.MenuButton.Enabled = Colorspace.Value
                ColorspaceTransferTo.MenuButton.Enabled = Colorspace.Value
                ColorspaceRangeFrom.MenuButton.Enabled = Colorspace.Value
                ColorspaceRangeTo.MenuButton.Enabled = Colorspace.Value
                ColorspaceHdr2sdr.MenuButton.Enabled = Colorspace.Value
                ColorspaceHdr2sdrSourcepeak.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrLdrnits.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrDesatbase.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrDesatstrength.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrDesatexp.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrHableA.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrHableB.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrHableC.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrHableD.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrHableE.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrHableF.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrMobiusTransition.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrMobiusPeak.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrReinhardContrast.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrReinhardPeak.NumEdit.Enabled = Colorspace.Value

                EdgelevelStrength.NumEdit.Enabled = Edgelevel.Value
                EdgelevelThreshold.NumEdit.Enabled = Edgelevel.Value
                EdgelevelBlack.NumEdit.Enabled = Edgelevel.Value
                EdgelevelWhite.NumEdit.Enabled = Edgelevel.Value

                UnsharpRadius.NumEdit.Enabled = Unsharp.Value
                UnsharpWeight.NumEdit.Enabled = Unsharp.Value
                UnsharpThreshold.NumEdit.Enabled = Unsharp.Value

                WarpsharpBlur.NumEdit.Enabled = Warpsharp.Value
                WarpsharpChroma.NumEdit.Enabled = Warpsharp.Value
                WarpsharpDepth.NumEdit.Enabled = Warpsharp.Value
                WarpsharpThreshold.NumEdit.Enabled = Warpsharp.Value
                WarpsharpType.NumEdit.Enabled = Warpsharp.Value

                KnnRadius.NumEdit.Enabled = Knn.Value
                KnnStrength.NumEdit.Enabled = Knn.Value
                KnnLerp.NumEdit.Enabled = Knn.Value
                KnnThLerp.NumEdit.Enabled = Knn.Value

                PadLeft.NumEdit.Enabled = Pad.Value
                PadTop.NumEdit.Enabled = Pad.Value
                PadRight.NumEdit.Enabled = Pad.Value
                PadBottom.NumEdit.Enabled = Pad.Value

                SmoothQuality.NumEdit.Enabled = Smooth.Value
                SmoothQP.NumEdit.Enabled = Smooth.Value
                SmoothPrec.MenuButton.Enabled = Smooth.Value

                TweakContrast.NumEdit.Enabled = Tweak.Value
                TweakGamma.NumEdit.Enabled = Tweak.Value
                TweakSaturation.NumEdit.Enabled = Tweak.Value
                TweakHue.NumEdit.Enabled = Tweak.Value
                TweakBrightness.NumEdit.Enabled = Tweak.Value

                PaSc.MenuButton.Enabled = Pa.Value
                PaSs.MenuButton.Enabled = Pa.Value
                PaActivityType.MenuButton.Enabled = Pa.Value
                PaCaqStrength.MenuButton.Enabled = Pa.Value
                PaInitqpsc.NumEdit.Enabled = Pa.Value
                PaFskipMaxqp.NumEdit.Enabled = Pa.Value

                PmdApplyCount.NumEdit.Enabled = Pmd.Value
                PmdStrength.NumEdit.Enabled = Pmd.Value
                PmdThreshold.NumEdit.Enabled = Pmd.Value

                For Each i In {DebandRange, DebandSample, DebandThre, DebandThreY, DebandThreCB,
                    DebandThreCR, DebandDither, DebandDitherY, DebandDitherC, DebandSeed}

                    i.NumEdit.Enabled = Deband.Value
                Next

                DebandRandEachFrame.CheckBox.Enabled = Deband.Value
                DebandBlurfirst.CheckBox.Enabled = Deband.Value

                AfsPreset.MenuButton.Enabled = Deinterlacer.Value = 1
                AfsINI.TextEdit.Enabled = Deinterlacer.Value = 1

                For Each i In {AfsLeft, AfsRight, AfsTop, AfsBottom, AfsMethodSwitch, AfsCoeffShift,
                               AfsThreShift, AfsThreDeint, AfsThreMotionY, AfsThreMotionC, AfsLevel}

                    i.NumEdit.Enabled = Deinterlacer.Value = 1
                Next

                For Each i In {AfsShift, AfsDrop, AfsSmooth, Afs24fps, AfsTune, AfsRFF, AfsTimecode, AfsLog}
                    i.CheckBox.Enabled = Deinterlacer.Value = 1
                Next

                NnediField.MenuButton.Enabled = Deinterlacer.Value = 2
                NnediNns.MenuButton.Enabled = Deinterlacer.Value = 2
                NnediNsize.MenuButton.Enabled = Deinterlacer.Value = 2
                NnediQuality.MenuButton.Enabled = Deinterlacer.Value = 2
                NnediPrescreen.MenuButton.Enabled = Deinterlacer.Value = 2
                NnediErrortype.MenuButton.Enabled = Deinterlacer.Value = 2
                NnediPrec.MenuButton.Enabled = Deinterlacer.Value = 2
                NnediWeightfile.TextEdit.Enabled = Deinterlacer.Value = 2

            End If

            MyBase.OnValueChanged(item)
        End Sub



        Public Overrides Sub ShowHelp(options As String())
            ShowConsoleHelp(Package.VCEEnc, options)
        End Sub

        Function GetSmoothArgs() As String
            If Smooth.Value Then
                Dim ret = ""
                If SmoothQuality.Value <> SmoothQuality.DefaultValue Then ret += ",quality=" & SmoothQuality.Value
                If SmoothQP.Value <> SmoothQP.DefaultValue Then ret += ",qp=" & SmoothQP.Value.ToInvariantString
                If SmoothPrec.Value <> SmoothPrec.DefaultValue Then ret += ",prec=" & SmoothPrec.ValueText
                Return "--vpp-smooth " + ret.TrimStart(","c)
            End If
        End Function

        Function GetColorspaceArgs() As String
            If Colorspace.Value Then
                Dim ret = ""
                If ColorspaceMatrixFrom.Value <> ColorspaceMatrixFrom.DefaultValue Then ret += $",matrix={ColorspaceMatrixFrom.ValueText}:{ColorspaceMatrixTo.ValueText}"
                If ColorspaceColorprimFrom.Value <> ColorspaceColorprimFrom.DefaultValue Then ret += $",colorprim={ColorspaceColorprimFrom.ValueText}:{ColorspaceColorprimTo.ValueText}"
                If ColorspaceTransferFrom.Value <> ColorspaceTransferFrom.DefaultValue Then ret += $",transfer={ColorspaceTransferFrom.ValueText}:{ColorspaceTransferTo.ValueText}"
                If ColorspaceRangeFrom.Value <> ColorspaceRangeFrom.DefaultValue Then ret += $",range={ColorspaceRangeFrom.ValueText}:{ColorspaceRangeTo.ValueText}"
                If ColorspaceHdr2sdr.Value <> ColorspaceHdr2sdr.DefaultValue Then
                    ret += $",hdr2sdr={ColorspaceHdr2sdr.ValueText}"
                    If ColorspaceHdr2sdrSourcepeak.Value <> ColorspaceHdr2sdrSourcepeak.DefaultValue Then ret += $",source_peak={ColorspaceHdr2sdrSourcepeak.Value.ToInvariantString("0.0")}"
                    If ColorspaceHdr2sdrLdrnits.Value <> ColorspaceHdr2sdrLdrnits.DefaultValue Then ret += $",ldr_nits={ColorspaceHdr2sdrLdrnits.Value.ToInvariantString("0.0")}"
                    If ColorspaceHdr2sdrDesatbase.Value <> ColorspaceHdr2sdrDesatbase.DefaultValue Then ret += $",desat_base={ColorspaceHdr2sdrDesatbase.Value.ToInvariantString("0.00")}"
                    If ColorspaceHdr2sdrDesatstrength.Value <> ColorspaceHdr2sdrDesatstrength.DefaultValue Then ret += $",desat_strength={ColorspaceHdr2sdrDesatstrength.Value.ToInvariantString("0.00")}"
                    If ColorspaceHdr2sdrDesatexp.Value <> ColorspaceHdr2sdrDesatexp.DefaultValue Then ret += $",desat_exp={ColorspaceHdr2sdrDesatexp.Value.ToInvariantString("0.00")}"
                    If ColorspaceHdr2sdr.Value = 1 Then
                        If ColorspaceHdr2sdrHableA.Value <> ColorspaceHdr2sdrHableA.DefaultValue Then ret += $",a={ColorspaceHdr2sdrHableA.Value.ToInvariantString("0.00")}"
                        If ColorspaceHdr2sdrHableB.Value <> ColorspaceHdr2sdrHableB.DefaultValue Then ret += $",b={ColorspaceHdr2sdrHableB.Value.ToInvariantString("0.00")}"
                        If ColorspaceHdr2sdrHableC.Value <> ColorspaceHdr2sdrHableC.DefaultValue Then ret += $",c={ColorspaceHdr2sdrHableC.Value.ToInvariantString("0.00")}"
                        If ColorspaceHdr2sdrHableD.Value <> ColorspaceHdr2sdrHableD.DefaultValue Then ret += $",d={ColorspaceHdr2sdrHableD.Value.ToInvariantString("0.00")}"
                        If ColorspaceHdr2sdrHableE.Value <> ColorspaceHdr2sdrHableE.DefaultValue Then ret += $",e={ColorspaceHdr2sdrHableE.Value.ToInvariantString("0.00")}"
                        If ColorspaceHdr2sdrHableF.Value <> ColorspaceHdr2sdrHableF.DefaultValue Then ret += $",f={ColorspaceHdr2sdrHableF.Value.ToInvariantString("0.00")}"
                    End If
                    If ColorspaceHdr2sdr.Value = 2 Then
                        If ColorspaceHdr2sdrMobiusTransition.Value <> ColorspaceHdr2sdrMobiusTransition.DefaultValue Then ret += $",transition={ColorspaceHdr2sdrMobiusTransition.Value.ToInvariantString("0.00")}"
                        If ColorspaceHdr2sdrMobiusPeak.Value <> ColorspaceHdr2sdrMobiusPeak.DefaultValue Then ret += $",peak={ColorspaceHdr2sdrMobiusPeak.Value.ToInvariantString("0.00")}"
                    End If
                    If ColorspaceHdr2sdr.Value = 3 Then
                        If ColorspaceHdr2sdrReinhardContrast.Value <> ColorspaceHdr2sdrReinhardContrast.DefaultValue Then ret += $",contrast={ColorspaceHdr2sdrReinhardContrast.Value.ToInvariantString("0.00")}"
                        If ColorspaceHdr2sdrReinhardPeak.Value <> ColorspaceHdr2sdrReinhardPeak.DefaultValue Then ret += $",peak={ColorspaceHdr2sdrReinhardPeak.Value.ToInvariantString("0.00")}"
                    End If
                End If
                If ret <> "" Then Return "--vpp-colorspace " + ret.TrimStart(","c)
            End If
        End Function

        Function GetPmdArgs() As String
            If Pmd.Value Then
                Dim ret = ""
                If PmdApplyCount.Value <> PmdApplyCount.DefaultValue Then ret += ",apply_count=" & PmdApplyCount.Value
                If PmdStrength.Value <> PmdStrength.DefaultValue Then ret += ",strength=" & PmdStrength.Value
                If PmdThreshold.Value <> PmdThreshold.DefaultValue Then ret += ",threshold=" & PmdThreshold.Value
                Return ("--vpp-pmd " + ret.TrimStart(","c)).Trim()
            End If
        End Function

        Function GetPaArgs() As String
            If Pa.Value Then
                Dim ret = ""
                If PaSc.Value <> PaSc.DefaultValue Then ret += " --pa-sc " & PaSc.OptionText.ToInvariantString
                If PaSs.Value <> PaSs.DefaultValue Then ret += " --pa-ss " & PaSs.OptionText.ToInvariantString
                If PaActivityType.Value <> PaActivityType.DefaultValue Then ret += " --pa-activity-type " & PaActivityType.OptionText.ToInvariantString
                If PaCaqStrength.Value <> PaCaqStrength.DefaultValue Then ret += " --pa-caq-strength " & PaCaqStrength.OptionText.ToInvariantString
                If PaInitqpsc.Value <> PaInitqpsc.DefaultValue Then ret += " --pa-initqpsc " & PaInitqpsc.Value.ToInvariantString
                If PaFskipMaxqp.Value <> PaFskipMaxqp.DefaultValue Then ret += " --pa-fskip-maxqp " & PaFskipMaxqp.Value.ToInvariantString
                Return ("--pa " + ret.TrimStart(" "c)).Trim()
            End If
        End Function

        Function GetTweakArgs() As String
            If Tweak.Value Then
                Dim ret = ""
                If TweakBrightness.Value <> TweakBrightness.DefaultValue Then ret += ",brightness=" & TweakBrightness.Value.ToInvariantString
                If TweakContrast.Value <> TweakContrast.DefaultValue Then ret += ",contrast=" & TweakContrast.Value.ToInvariantString
                If TweakSaturation.Value <> TweakSaturation.DefaultValue Then ret += ",saturation=" & TweakSaturation.Value.ToInvariantString
                If TweakGamma.Value <> TweakGamma.DefaultValue Then ret += ",gamma=" & TweakGamma.Value.ToInvariantString
                If TweakHue.Value <> TweakHue.DefaultValue Then ret += ",hue=" & TweakHue.Value.ToInvariantString
                Return ("--vpp-tweak " + ret.TrimStart(","c)).Trim()
            End If
        End Function

        Function GetPaddingArgs() As String
            Return If(Pad.Value, $"--vpp-pad {PadLeft.Value},{PadTop.Value},{PadRight.Value},{PadBottom.Value}", "")
        End Function

        Function GetKnnArgs() As String
            If Knn.Value Then
                Dim ret = ""
                If KnnRadius.Value <> KnnRadius.DefaultValue Then ret += ",radius=" & KnnRadius.Value
                If KnnStrength.Value <> KnnStrength.DefaultValue Then ret += ",strength=" & KnnStrength.Value.ToInvariantString
                If KnnLerp.Value <> KnnLerp.DefaultValue Then ret += ",lerp=" & KnnLerp.Value.ToInvariantString
                If KnnThLerp.Value <> KnnThLerp.DefaultValue Then ret += ",th_lerp=" & KnnThLerp.Value.ToInvariantString
                Return "--vpp-knn " + ret.TrimStart(","c)
            End If
        End Function

        Function GetDebandArgs() As String
            If Deband.Value Then
                Dim ret = ""
                If DebandRange.Value <> DebandRange.DefaultValue Then ret += ",range=" & DebandRange.Value
                If DebandSample.Value <> DebandSample.DefaultValue Then ret += ",sample=" & DebandSample.Value
                If DebandThre.Value <> DebandThre.DefaultValue Then ret += ",thre=" & DebandThre.Value
                If DebandThreY.Value <> DebandThreY.DefaultValue Then ret += ",thre_y=" & DebandThreY.Value
                If DebandThreCB.Value <> DebandThreCB.DefaultValue Then ret += ",thre_cb=" & DebandThreCB.Value
                If DebandThreCR.Value <> DebandThreCR.DefaultValue Then ret += ",thre_cr=" & DebandThreCR.Value
                If DebandDither.Value <> DebandDither.DefaultValue Then ret += ",dither=" & DebandDither.Value
                If DebandDitherY.Value <> DebandDitherY.DefaultValue Then ret += ",dither_y=" & DebandDitherY.Value
                If DebandDitherC.Value <> DebandDitherC.DefaultValue Then ret += ",dither_c=" & DebandDitherC.Value
                If DebandSeed.Value <> DebandSeed.DefaultValue Then ret += ",seed=" & DebandSeed.Value
                If DebandBlurfirst.Value Then ret += ",blurfirst"
                If DebandRandEachFrame.Value Then ret += ",rand_each_frame"
                Return "--vpp-deband " + ret.TrimStart(","c)
            End If
        End Function

        Function GetEdge() As String
            If Edgelevel.Value Then
                Dim ret = ""
                If EdgelevelStrength.Value <> EdgelevelStrength.DefaultValue Then ret += ",strength=" & EdgelevelStrength.Value.ToInvariantString
                If EdgelevelThreshold.Value <> EdgelevelThreshold.DefaultValue Then ret += ",threshold=" & EdgelevelThreshold.Value.ToInvariantString
                If EdgelevelBlack.Value <> EdgelevelBlack.DefaultValue Then ret += ",black=" & EdgelevelBlack.Value.ToInvariantString
                If EdgelevelWhite.Value <> EdgelevelWhite.DefaultValue Then ret += ",white=" & EdgelevelWhite.Value.ToInvariantString
                Return "--vpp-edgelevel " + ret.TrimStart(","c)
            End If
        End Function

        Function GetUnsharp() As String
            If Unsharp.Value Then
                Dim ret = ""
                If UnsharpRadius.Value <> UnsharpRadius.DefaultValue Then ret += ",radius=" & UnsharpRadius.Value.ToInvariantString
                If UnsharpWeight.Value <> UnsharpWeight.DefaultValue Then ret += ",weight=" & UnsharpWeight.Value.ToInvariantString
                If UnsharpThreshold.Value <> UnsharpThreshold.DefaultValue Then ret += ",threshold=" & UnsharpThreshold.Value.ToInvariantString
                Return "--vpp-unsharp " + ret.TrimStart(","c)
            End If
        End Function

        Function GetWarpsharpArgs() As String
            If Warpsharp.Value Then
                Dim ret = ""
                If WarpsharpThreshold.Value <> WarpsharpThreshold.DefaultValue Then ret += ",threshold=" & WarpsharpThreshold.Value.ToInvariantString
                If WarpsharpBlur.Value <> WarpsharpBlur.DefaultValue Then ret += ",blur=" & WarpsharpBlur.Value.ToInvariantString
                If WarpsharpType.Value <> WarpsharpType.DefaultValue Then ret += ",type=" & WarpsharpType.Value.ToInvariantString
                If WarpsharpDepth.Value <> WarpsharpDepth.DefaultValue Then ret += ",depth=" & WarpsharpDepth.Value.ToInvariantString
                If WarpsharpChroma.Value <> WarpsharpChroma.DefaultValue Then ret += ",chroma=" & WarpsharpChroma.Value.ToInvariantString
                Return "--vpp-warpsharp " + ret.TrimStart(","c)
            End If
        End Function

        Function GetTransform() As String
            Dim ret = ""
            If TransformFlipX.Value Then ret += ",flip_x=true"
            If TransformFlipY.Value Then ret += ",flip_y=true"
            If TransformTranspose.Value Then ret += ",transpose=true"
            If ret <> "" Then Return ("--vpp-transform " + ret.TrimStart(","c))
        End Function

        Function GetDeinterlacerArgs() As String
            Dim ret = ""
            Select Case Deinterlacer.Value
                Case 1
                    If AfsPreset.Value <> AfsPreset.DefaultValue Then ret += ",preset=" + AfsPreset.ValueText
                    If AfsINI.Value <> "" Then ret += ",ini=" + AfsINI.Value.Escape
                    If AfsLeft.Value <> AfsLeft.DefaultValue Then ret += ",left=" & AfsLeft.Value
                    If AfsRight.Value <> AfsRight.DefaultValue Then ret += ",right=" & AfsRight.Value
                    If AfsTop.Value <> AfsTop.DefaultValue Then ret += ",top=" & AfsTop.Value
                    If AfsBottom.Value <> AfsBottom.DefaultValue Then ret += ",bottom=" & AfsBottom.Value
                    If AfsMethodSwitch.Value <> AfsMethodSwitch.DefaultValue Then ret += ",method_switch=" & AfsMethodSwitch.Value
                    If AfsCoeffShift.Value <> AfsCoeffShift.DefaultValue Then ret += ",coeff_shift=" & AfsCoeffShift.Value
                    If AfsThreShift.Value <> AfsThreShift.DefaultValue Then ret += ",thre_shift=" & AfsThreShift.Value
                    If AfsThreDeint.Value <> AfsThreDeint.DefaultValue Then ret += ",thre_deint=" & AfsThreDeint.Value
                    If AfsThreMotionY.Value <> AfsThreMotionY.DefaultValue Then ret += ",thre_motion_y=" & AfsThreMotionY.Value
                    If AfsThreMotionC.Value <> AfsThreMotionC.DefaultValue Then ret += ",thre_motion_c=" & AfsThreMotionC.Value
                    If AfsLevel.Value <> AfsLevel.DefaultValue Then ret += ",level=" & AfsLevel.Value
                    If AfsShift.Value <> AfsShift.DefaultValue Then ret += ",shift=" + If(AfsShift.Value, "on", "off")
                    If AfsDrop.Value <> AfsDrop.DefaultValue Then ret += ",drop=" + If(AfsDrop.Value, "on", "off")
                    If AfsSmooth.Value <> AfsSmooth.DefaultValue Then ret += ",smooth=" + If(AfsSmooth.Value, "on", "off")
                    If Afs24fps.Value <> Afs24fps.DefaultValue Then ret += ",24fps=" + If(Afs24fps.Value, "on", "off")
                    If AfsTune.Value <> AfsTune.DefaultValue Then ret += ",tune=" + If(AfsTune.Value, "on", "off")
                    If AfsRFF.Value <> AfsRFF.DefaultValue Then ret += ",rff=" + If(AfsRFF.Value, "on", "off")
                    If AfsTimecode.Value <> AfsTimecode.DefaultValue Then ret += ",timecode=" + If(AfsTimecode.Value, "on", "off")
                    If AfsLog.Value <> AfsLog.DefaultValue Then ret += ",log=" + If(AfsLog.Value, "on", "off")
                    Return "--vpp-afs " + ret.TrimStart(","c)
                Case 2
                    If NnediField.Value <> NnediField.DefaultValue Then ret += ",field=" + NnediField.ValueText
                    If NnediNns.Value <> NnediNns.DefaultValue Then ret += ",nns=" + NnediNns.ValueText
                    If NnediNsize.Value <> NnediNsize.DefaultValue Then ret += ",nsize=" + NnediNsize.ValueText
                    If NnediQuality.Value <> NnediQuality.DefaultValue Then ret += ",quality=" + NnediQuality.ValueText
                    If NnediPrescreen.Value <> NnediPrescreen.DefaultValue Then ret += ",prescreen=" + NnediPrescreen.ValueText
                    If NnediErrortype.Value <> NnediErrortype.DefaultValue Then ret += ",errortype=" + NnediErrortype.ValueText
                    If NnediPrec.Value <> NnediPrec.DefaultValue Then ret += ",prec=" + NnediPrec.ValueText
                    If NnediWeightfile.Value <> "" Then ret += ",weightfile=" + NnediWeightfile.Value.Escape
                    Return "--vpp-nnedi " + ret.TrimStart(","c)
                Case Else
                    Return ret
            End Select
        End Function

        'Function GetModeArgs() As String
        '    Dim rate = If(ConstantQualityMode.Value, 0, Bitrate.Value)

        '    Select Case Mode.Value
        '        Case 0
        '            Return If(QPAdvanced.Value, $"--cqp {QPI.Value}:{QPP.Value}:{QPB.Value}", $" --cqp {QP.Value}")
        '        Case 1
        '            Return "--cbr " & rate
        '        Case 2
        '            Return "--vbr " & rate
        '    End Select
        'End Function



        Overrides Function GetCommandLine(
            includePaths As Boolean,
            includeExecutable As Boolean,
            Optional pass As Integer = 1) As String

            Dim ret As String
            Dim sourcePath As String
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)

            If includePaths AndAlso includeExecutable Then
                ret = Package.VCEEnc.Path.Escape
            End If

            Select Case Decoder.ValueText
                Case "avs"
                    sourcePath = p.Script.Path

                    If includePaths AndAlso FrameServerHelp.IsPortable Then
                        ret += " --avsdll " + Package.AviSynth.Path.Escape
                    End If
                Case "qs"
                    sourcePath = "-"

                    If includePaths Then
                        ret = If(includePaths, Package.QSVEnc.Path.Escape, "QSVEncC64") + " -o - -c raw" + " -i " + If(includePaths, p.SourceFile.Escape, "path") + " | " + If(includePaths, Package.VCEEnc.Path.Escape, "VCEEncC64")
                    End If
                Case "ffdxva"
                    sourcePath = "-"

                    If includePaths Then
                        Dim pix_fmt = If(p.SourceVideoBitDepth = 10, "yuv420p10le", "yuv420p")
                        ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") +
                            " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.SourceFile.Escape, "path") +
                            " -f yuv4mpegpipe -pix_fmt " + pix_fmt + " -strict -1 -loglevel fatal - | " +
                            If(includePaths, Package.VCEEnc.Path.Escape, "VCEEncC64")
                    End If
                Case "ffqsv"
                    sourcePath = "-"

                    If includePaths Then
                        ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.SourceFile.Escape, "path") + " -f yuv4mpegpipe -strict -1 -pix_fmt yuv420p -loglevel fatal - | " + If(includePaths, Package.VCEEnc.Path.Escape, "VCEEncC64")
                    End If
                Case "vce"
                    sourcePath = p.LastOriginalSourceFile
                    ret += " --avhw"
            End Select

            Dim q = From i In Items Where i.GetArgs <> ""

            If q.Count > 0 Then
                ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")
            End If

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

            If sourcePath = "-" Then
                ret += " --y4m"
            End If

            If ret.Contains("%") Then
                ret = Macro.Expand(ret)
            End If

            If includePaths Then
                ret += " -i " + sourcePath.Escape + " -o " + targetPath.Escape
            End If

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.VCEEnc
        End Function
    End Class
End Class