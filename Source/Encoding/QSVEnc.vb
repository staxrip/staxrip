
Imports Microsoft
Imports Microsoft.VisualBasic.Logging
Imports System.Runtime.InteropServices.ComTypes
Imports StaxRip.VideoEncoderCommandLine

<Serializable()>
Public Class QSVEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "QSVEncC (Intel) | " + Params.Codec.OptionText.Replace("Intel ", "")
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

    Overrides Property Bitrate As Integer
        Get
            Return CInt(Params.Bitrate.Value)
        End Get
        Set(value As Integer)
            Params.Bitrate.Value = value
        End Set
    End Property

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(codecIndex As Integer)
        MyBase.New()
        Params.Codec.Value = If(codecIndex > 0 AndAlso codecIndex < Params.Codec.Values.Length, codecIndex, 0)
    End Sub

    Overrides Sub ShowConfigDialog()
        Dim params1 As New EncoderParams
        Dim store = ObjectHelp.GetCopy(ParamsStore)
        params1.Init(store)

        Using form As New CommandLineForm(params1)
            form.HTMLHelpFunc = Function() $"<p><a href=""{Package.QSVEncC.HelpURL}"">QSVEnc Online Help</a></p>" +
                $"<p><a href=""https://github.com/staxrip/staxrip/wiki/qsvenc-bitrate-modes"">QSVEnc bitrate modes</a></p>" +
                $"<pre>{HelpDocument.ConvertChars(Package.QSVEncC.CreateHelpfile())}</pre>"

            Dim a = Sub()
                        Dim enc = ObjectHelp.GetCopy(Me)
                        Dim params2 As New EncoderParams
                        Dim store2 = ObjectHelp.GetCopy(store)
                        params2.Init(store2)
                        enc.Params = params2
                        enc.ParamsStore = store2
                        SaveProfile(enc)
                    End Sub

            form.cms.Add("Check Hardware", Sub() g.ShowCode("Check Hardware", ProcessHelp.GetConsoleOutput(Package.QSVEncC.Path, "--check-hw")))
            form.cms.Add("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetConsoleOutput(Package.QSVEncC.Path, "--check-features")), Keys.Control Or Keys.F)
            form.cms.Add("Check Environment", Sub() g.ShowCode("Check Environment", ProcessHelp.GetConsoleOutput(Package.QSVEncC.Path, "--check-environment")))
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
            If Params.Codec.ValueText = "mpeg2" Then
                Return "m2v"
            End If

            Return Params.Codec.ValueText
        End Get
    End Property

    Overrides Sub Encode()
        If OutputExt = "hevc" Then
            Dim codecs = ProcessHelp.GetConsoleOutput(Package.QSVEncC.Path, "--check-features").Right("Codec")

            If Not codecs.ToLowerEx.Contains("hevc") Then
                Throw New ErrorAbortException("QSVEnc Error", "H.265/HEVC isn't supported by your Hardware.")
            End If
        End If

        p.Script.Synchronize()
        Params.RaiseValueChanged(Nothing)

        Using proc As New Proc
            proc.Header = "Video encoding"
            proc.Package = Package.QSVEncC
            proc.SkipString = " frames: "
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
            Return Params.Mode.ValueText.EqualsAny("cqp", "icq", "la-icq", "vcm")
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

        tester.IgnoredSwitches = "version check-lib check-device video-streamid video-track input-analyze
            check-avversion check-codecs check-encoders check-decoders check-formats vpp-delogo-add
            check-protocols chapter-no-trim check-filters device output vpp-half-turn input-format
            raw avs vpy-mt audio-source audio-file seek format output-format audio-profile
            audio-copy audio-copy audio-codec audio-bitrate audio-ignore check-profiles vpp-delogo-pos
            audio-ignore audio-samplerate audio-resampler audio-stream audio-stream vpp-delogo-y vpy
            audio-stream audio-stream audio-filter chapter-copy chapter sub-copy vpp-delogo-depth input
            avsync mux-option input-res fps dar avqsv-analyze benchmark vpp-delogo-cb vpp-delogo-cr
            bench-quality log log-framelist audio-thread avi avqsv input-file python qvbr-quality help
            audio-ignore-decode-error audio-ignore-notrack-error nv12 output-file sharpness vpp-delogo
            check-features-html perf-monitor perf-monitor-plot perf-monitor-interval vpp-delogo-select
            audio-delay audio-disposition audio-metadata option-list sub-disposition sub-metadata
            metadata video-metadata video-tag attachment-copy sub-source process-codepage"

        tester.UndocumentedSwitches = "input-thread chromaloc videoformat colormatrix colorprim transfer fullrange"
        tester.Package = Package.QSVEncC
        tester.CodeFile = Path.Combine(Folder.Startup.Parent, "Encoding", "qsvenc.vb")

        Return tester.Test
    End Function

    Public Class EncoderParams
        Inherits CommandLineParams

        Sub New()
            Title = "QSVEncC Options"
        End Sub

        Property Device As New OptionParam With {
            .Switch = "--device",
            .Text = "Device",
            .Options = {"Automatic", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"}}

        Property Decoder As New OptionParam With {
            .Text = "Decoder",
            .Options = {"AviSynth/VapourSynth", "QSVEnc Hardware", "QSVEnc Software", "ffmpeg Intel", "ffmpeg DXVA2"},
            .Values = {"avs", "qshw", "qssw", "ffqsv", "ffdxva"}}

        Property Codec As New OptionParam With {
            .Switch = "--codec",
            .Text = "Codec",
            .Options = {"H.264", "H.265", "MPEG-2", "VP9", "AV1"},
            .Values = {"h264", "hevc", "mpeg2", "vp9", "av1"}}

        Property Mode As New OptionParam With {
            .Switches = {"--avbr", "--cbr", "--cqp", "--icq", "--la", "--la-hrd", "--la-icq", "--qvbr", "--vbr", "--vcm"},
            .Name = "Mode",
            .Text = "Mode",
            .Options = {"AVBR - Average Variable Bitrate", "CBR - Constant Bitrate", "CQP - Constant QP", "ICQ - Intelligent Constant Quality", "LA - VBR Lookahead", "LA-HRD - VBR HRD Lookahead", "LA-ICQ - Intelligent Constant Quality Lookahead", "QVBR - Quality-Defined Variable Bitrate", "VBR - Variable Bitrate", "VCM - Video Conferencing Mode"},
            .Values = {"avbr", "cbr", "cqp", "icq", "la", "la-hrd", "la-icq", "qvbr", "vbr", "vcm"},
            .Init = 3}

        Property Bitrate As New NumParam With {
            .HelpSwitch = "--bitrate",
            .Text = "Bitrate",
            .Init = 5000,
            .VisibleFunc = Function() Mode.Value < 2 OrElse Mode.Value = 4 OrElse Mode.Value = 5 OrElse Mode.Value = 7 OrElse Mode.Value = 8 OrElse Mode.Value = 9,
            .Config = {0, 1000000, 100}}

        Property QvbrQuality As New NumParam With {
            .Switch = "--qvbr-quality",
            .Text = "QVBR Quality",
            .Init = 18,
            .VisibleFunc = Function() Mode.Value = 7,
            .Config = {0, 51, 1}}

        Property Quality As New NumParam With {
            .Text = "Quality",
            .Init = 18,
            .VisibleFunc = Function() {"icq", "la-icq"}.Contains(Mode.ValueText),
            .Config = {0, 63}}

        Property OutputDepth As New OptionParam With {
            .Switch = "--output-depth",
            .Text = "Output Depth",
            .Options = {"8-Bit", "10-Bit"},
            .Values = {"8", "10"},
            .Init = 0}

        Property QPI As New NumParam With {
            .HelpSwitch = "--cqp",
            .Text = "QP I",
            .Init = 18,
            .VisibleFunc = Function() {"cqp"}.Contains(Mode.ValueText),
            .Config = {0, 63}}

        Property QPP As New NumParam With {
            .HelpSwitch = "--cqp",
            .Text = "QP P",
            .Init = 20,
            .VisibleFunc = Function() {"cqp"}.Contains(Mode.ValueText),
            .Config = {0, 63}}

        Property QPB As New NumParam With {
            .HelpSwitch = "--cqp",
            .Text = "QP B",
            .Init = 24,
            .VisibleFunc = Function() {"cqp"}.Contains(Mode.ValueText),
            .Config = {0, 63}}

        Property QPOffsetI As New NumParam With {
            .HelpSwitch = "--qp-offset",
            .Text = "QP Offset I",
            .VisibleFunc = Function() {"cqp"}.Contains(Mode.ValueText),
            .Config = {0, Integer.MaxValue, 1}}

        Property QPOffsetP As New NumParam With {
            .HelpSwitch = "--qp-offset",
            .Text = "QP Offset P",
            .VisibleFunc = Function() {"cqp"}.Contains(Mode.ValueText),
            .Config = {0, Integer.MaxValue, 1}}

        Property QPOffsetB As New NumParam With {
            .HelpSwitch = "--qp-offset",
            .Text = "QP Offset B",
            .VisibleFunc = Function() {"cqp"}.Contains(Mode.ValueText),
            .Config = {0, Integer.MaxValue, 1}}

        Property mctf As New BoolParam With {
            .Switch = "--vpp-mctf",
            .Text = "Enable Mctf",
            .ArgsFunc = AddressOf GetmctfArgs}

        Property mctfval As New NumParam With {
            .Text = "      Mctf",
            .Config = {0, 20}}

        Property Chromaloc As New NumParam With {
            .Switch = "--chromaloc",
            .Text = "Chromaloc",
            .Config = {0, 5}}

        Property VBVbufsize As New NumParam With {
            .Switch = "--vbv-bufsize",
            .Text = "VBV Bufsize",
            .Config = {0, 1000000, 100}}

        Property MaxCLL As New NumParam With {
            .Text = "Maximum CLL",
            .VisibleFunc = Function() Codec.ValueText = "hevc",
            .Switch = "--max-cll",
            .Config = {0, Integer.MaxValue, 50},
            .ArgsFunc = Function() If(MaxCLL.Value <> 0 OrElse MaxFALL.Value <> 0, "--max-cll """ & MaxCLL.Value & "," & MaxFALL.Value & """", ""),
            .ImportAction = Sub(param, arg)
                                If arg = "" Then
                                    Exit Sub
                                End If

                                Dim a = arg.Trim(""""c).Split(","c)
                                MaxCLL.Value = a(0).ToInt
                                MaxFALL.Value = a(1).ToInt
                            End Sub}

        Property MaxFALL As New NumParam With {
            .Switches = {"--max-cll"},
            .Text = "Maximum FALL",
            .Config = {0, Integer.MaxValue, 50},
            .VisibleFunc = Function() Codec.ValueText = "hevc"}

        Property Tune As New OptionParam With {
            .Switch = "--tune",
            .Text = "Tune",
            .Options = {"Default", "PSNR", "SSIM", "MS_SSIM", "VMAF", "Perceptual"}}


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

        Property Tweak As New BoolParam With {.Switch = "--vpp-tweak", .Text = "Tweaking", .ArgsFunc = AddressOf GetTweakArgs}
        Property TweakContrast As New NumParam With {.Text = "      Contrast", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {-2.0, 2.0, 0.1, 1}}
        Property TweakGamma As New NumParam With {.Text = "      Gamma", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {0.1, 10.0, 0.1, 1}}
        Property TweakSaturation As New NumParam With {.Text = "      Saturation", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {0.0, 3.0, 0.1, 1}}
        Property TweakHue As New NumParam With {.Text = "      Hue", .HelpSwitch = "--vpp-tweak", .Config = {-180.0, 180.0, 0.1, 1}}
        Property TweakBrightness As New NumParam With {.Text = "      Brightness", .HelpSwitch = "--vpp-tweak", .Config = {-1.0, 1.0, 0.1, 1}}
        Property TweakSwapuv As New OptionParam With {.Text = "      Swapuv", .HelpSwitch = "--vpp-tweak", .IntegerValue = False, .Init = 0, .Options = {"False", "True"}, .Values = {"false", "true"}}

        Property Pad As New BoolParam With {.Switch = "--vpp-pad", .Text = "Padding", .ArgsFunc = AddressOf GetPaddingArgs}
        Property PadLeft As New NumParam With {.Text = "      Left"}
        Property PadTop As New NumParam With {.Text = "      Top"}
        Property PadRight As New NumParam With {.Text = "      Right"}
        Property PadBottom As New NumParam With {.Text = "      Bottom"}

        Property Smooth As New BoolParam With {.Text = "Smooth", .Switch = "--vpp-smooth", .ArgsFunc = AddressOf GetSmoothArgs}
        Property SmoothQuality As New NumParam With {.Text = "      Quality", .HelpSwitch = "--vpp-smooth", .Init = 3, .Config = {1, 6}}
        Property SmoothQP As New NumParam With {.Text = "      QP", .HelpSwitch = "--vpp-smooth", .Config = {0, 100, 10, 1}}
        Property SmoothPrec As New OptionParam With {.Text = "      Precision", .HelpSwitch = "--vpp-smooth", .Options = {"Auto", "FP16", "FP32"}}

        Property Pmd As New BoolParam With {.Text = "Pmd", .Switch = "--vpp-pmd", .ArgsFunc = AddressOf GetPmdArgs}
        Property PmdApplyCount As New NumParam With {.Text = "      Apply Count", .HelpSwitch = "--vpp-pmd", .Init = 2, .Config = {1, Integer.MaxValue, 1}}
        Property PmdStrength As New NumParam With {.Text = "      Strength", .HelpSwitch = "--vpp-pmd", .Init = 100, .Config = {0, 100, 1, 1}}
        Property PmdThreshold As New NumParam With {.Text = "      Threshold", .HelpSwitch = "--vpp-pmd", .Init = 100, .Config = {0, 255, 1}}

        Property Denoise As New BoolParam With {.Text = "Denoise", .Switch = "--vpp-denoise", .ArgsFunc = AddressOf GetDenoiseArgs}
        Property DenoiseMode As New OptionParam With {.Text = "      Mode", .HelpSwitch = "--vpp-denoise", .Init = 0, .Options = {"Auto (Default)", "Auto BD Rate", "Auto Subjective", "Auto Adjust", "Pre-Processing", "Post-Processing"}, .Values = {"auto", "auto_bdrate", "auto_subjective", "auto_adjust", "pre", "post"}}
        Property DenoiseStrength As New NumParam With {.Text = "      Strength", .HelpSwitch = "--vpp-denoise", .Init = 100, .Config = {0, 100, 1, 0}}

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


        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)

                    Add("Basic",
                        Device, Mode, Codec,
                        New OptionParam With {.Switch = "--quality", .Text = "Preset", .Options = {"Best", "Higher", "High", "Balanced", "Fast", "Faster", "Fastest"}, .Init = 3},
                        New OptionParam With {.Switch = "--profile", .Text = "Profile", .Name = "ProfileH264", .VisibleFunc = Function() Codec.Value = 0, .Options = {"Automatic", "Baseline", "Main", "High"}},
                        New OptionParam With {.Switch = "--profile", .Text = "Profile", .Name = "ProfileH265", .VisibleFunc = Function() Codec.Value = 1, .Options = {"Automatic", "Main", "Main 10"}},
                        New OptionParam With {.Switch = "--profile", .Text = "Profile", .Name = "ProfileMPEG2", .VisibleFunc = Function() Codec.Value = 2, .Options = {"Automatic", "Simple", "Main", "High"}},
                        New OptionParam With {.Switch = "--profile", .Text = "Profile", .Name = "ProfileAV1", .VisibleFunc = Function() Codec.Value = 4, .Options = {"Automatic", "Main", "High", "Pro"}},
                        New OptionParam With {.Switch = "--tier", .Text = "Tier", .VisibleFunc = Function() Codec.ValueText = "h265", .Options = {"Main", "High"}, .Values = {"main", "high"}},
                        New OptionParam With {.Name = "LevelH264", .Switch = "--level", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "h264", .Options = {"Automatic", "1", "1b", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                        New OptionParam With {.Name = "LevelH265", .Switch = "--level", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "hevc", .Options = {"Automatic", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}},
                        New OptionParam With {.Name = "LevelMpeg2", .Switch = "--level", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "mpeg2", .Options = {"Automatic", "low", "main", "high", "high1440"}},
                        New OptionParam With {.Name = "LevelVp9", .Switch = "--level", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "vp9", .Options = {"0", "1", "2", "3"}},
                        New OptionParam With {.Name = "LevelAV1", .Switch = "--level", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "av1", .Options = {"Automatic", "2", "2.1", "2.2", "2.3", "3", "3.1", "3.2", "3.3", "4", "4.1", "4.2", "4.3", "5", "5.1", "5.2", "5.3", "6", "6.1", "6.2", "6.3", "7", "7.1", "7.2", "7.3"}},
                        Tune, OutputDepth, QPI, QPP, QPB, Bitrate, QvbrQuality, Quality)
                    Add("Analysis",
                        New OptionParam With {.Switch = "--trellis", .Text = "Trellis", .Options = {"Automatic", "Off", "I", "IP", "All"}},
                        New OptionParam With {.Switch = "--ctu", .Text = "CTU", .Options = {"16", "32", "64"}, .VisibleFunc = Function() Codec.ValueText = "hevc"},
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
                        New BoolParam With {.Switch = "--i-adapt", .Text = "Adaptive I-Frame Insert"},
                        New BoolParam With {.Switch = "--adapt-ltr", .Text = "Adaptive LTR frames"},
                        New BoolParam With {.Switch = "--extbrc", .Text = "Extended Rate Control"},
                        New BoolParam With {.Switch = "--direct-bias-adjust", .Text = "Direct Bias Adjust"},
                        New BoolParam With {.Switch = "--strict-gop", .Text = "Strict Gop"},
                        New BoolParam With {.Switch = "--open-gop", .Text = "Open Gop"})
                    Add("Rate Control", VBVbufsize,
                        New NumParam With {.Switch = "--max-bitrate", .Text = "Max Bitrate", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP", .Config = {0, Integer.MaxValue, 1}},
                        QPOffsetI, QPOffsetP, QPOffsetB,
                        New NumParam With {.Switch = "--avbr-unitsize", .Text = "AVBR Unitsize", .Init = 90},
                        New BoolParam With {.Switch = "--mbbrc", .Text = "Per macro block rate control"})
                    Add("Motion Search",
                        New OptionParam With {.Switch = "--mv-scaling", .Text = "MV Scaling", .IntegerValue = True, .Options = {"Default", "MV cost to be 0", "MV cost 1/2 of default", "MV cost 1/4 of default", "MV cost 1/8 of default"}},
                        New BoolParam With {.Switch = "--weightb", .Text = "B-Frame Weight Prediction"},
                        New BoolParam With {.Switch = "--weightp", .Text = "P-Frame Weight Prediction"})
                    Add("Performance",
                        New OptionParam With {.Switch = "--output-buf", .Text = "Output Buffer", .Options = {"8", "16", "32", "64", "128"}},
                        New OptionParam With {.Switch = "--hyper-mode", .Text = "Hyper Mode", .Options = {"Off (Default)", "On - Use Hyper Encode", "Adaptive - Use Hyper Encode whenever possible"}, .Values = {"off", "on", "adaptive"}},
                        New NumParam With {.Switch = "--input-buf", .Text = "Input Buffer", .Config = {0, 16}},
                        New NumParam With {.Switch = "--mfx-thread", .Text = "Input Threads MFX"},
                        New NumParam With {.Switch = "--output-thread", .Text = "Output Thread", .Config = {0, 64}},
                        New NumParam With {.Switch = "--async-depth", .Text = "Async Depth", .Config = {0, 64}},
                        New BoolParam With {.Switch = "--min-memory", .Text = "Minimize memory usage"},
                        New BoolParam With {.Switch = "--max-procfps", .Text = "Limit performance to lower resource usage"})
                    Add("VPP | Misc",
                        New StringParam With {.Switch = "--vpp-subburn", .Text = "Subburn"},
                        New OptionParam With {.Switch = "--vpp-resize", .Text = "Resize", .Options = {"auto", "simple", "advanced", "bilinear", "bicubic", "spline16", "spline36", "spline64", "lanczos2", "lanczos3", "lanczos4"}},
                        New OptionParam With {.Switch = "--vpp-resize-mode", .Text = "Resize Mode", .Options = {"auto", "lowpower", "quality"}},
                        New OptionParam With {.Switch = "--vpp-rotate", .Text = "Rotate", .Options = {"Disabled", "90", "180", "270"}},
                        New OptionParam With {.Switch = "--vpp-image-stab", .Text = "Image Stabilizer", .Options = {"Disabled", "Upscale", "Box"}},
                        New OptionParam With {.Switch = "--vpp-mirror", .Text = "Mirror Image", .Options = {"Disabled", "H", "V"}},
                        New OptionParam With {.Switch = "--vpp-deinterlace", .Text = "Deinterlace", .Options = {"None", "Normal", "Inverse Telecine", "Double Framerate"}, .Values = {"none", "normal", "it", "bob"}},
                        New NumParam With {.Switch = "--vpp-detail-enhance", .Text = "Detail Enhance", .Config = {0, 100}},
                        New BoolParam With {.Switch = "--vpp-rff", .Text = "RFF", .Init = True},
                        New BoolParam With {.Switch = "--vpp-perc-pre-enc", .Text = "Perceptual Pre Encode"},
                        mctf,
                        mctfval)
                    Add("VPP | Misc 2",
                        Tweak, TweakBrightness, TweakContrast, TweakSaturation, TweakGamma, TweakHue, TweakSwapuv,
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
                    Add("VPP | Denoise",
                        Pmd, PmdApplyCount, PmdStrength, PmdThreshold,
                        Denoise, DenoiseMode, DenoiseStrength)
                    Add("VPP | Sharpness",
                        Edgelevel, EdgelevelStrength, EdgelevelThreshold, EdgelevelBlack, EdgelevelWhite,
                        Unsharp, UnsharpRadius, UnsharpWeight, UnsharpThreshold,
                        Warpsharp, WarpsharpThreshold, WarpsharpBlur, WarpsharpType, WarpsharpDepth, WarpsharpChroma)
                    Add("VUI",
                        New StringParam With {.Switch = "--master-display", .Text = "Master Display", .VisibleFunc = Function() Codec.ValueText = "hevc"},
                        New StringParam With {.Switch = "--sar", .Text = "Sample Aspect Ratio", .Init = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                        New StringParam With {.Switch = "--dhdr10-info", .Text = "HDR10 Info File", .BrowseFile = True},
                        New StringParam With {.Switch = "--dolby-vision-rpu", .Text = "Dolby Vision RPU", .BrowseFile = True},
                        New OptionParam With {.Switch = "--dolby-vision-profile", .Text = "Dolby Vision Profile", .Options = {"Undefined", "5.0", "8.1", "8.2", "8.4"}},
                        New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat", .Options = {"Undefined", "NTSC", "Component", "PAL", "SECAM", "MAC"}},
                        New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix", .Options = {"Undefined", "Auto", "BT 709", "SMPTE 170 M", "BT 470 BG", "SMPTE 240 M", "YCgCo", "FCC", "GBR", "BT 2020 NC", "BT 2020 C"}},
                        New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim", .Options = {"Undefined", "Auto", "BT 709", "SMPTE 170 M", "BT 470 M", "BT 470 BG", "SMPTE 240 M", "Film", "BT 2020"}},
                        New OptionParam With {.Switch = "--transfer", .Text = "Transfer", .Options = {"Undefined", "Auto", "BT 709", "SMPTE 170 M", "BT 470 M", "BT 470 BG", "SMPTE 240 M", "Linear", "Log 100", "Log 316", "IEC 61966-2-4", "BT 1361 E", "IEC 61966-2-1", "BT 2020-10", "BT 2020-12", "SMPTE 2084", "SMPTE 428", "ARIB-STD-B67"}},
                        New OptionParam With {.Switch = "--atc-sei", .Text = "ATC SEI", .Init = 1, .Options = {"Undef", "Unknown", "Auto", "Auto_Res", "BT 709", "SMPTE 170 M", "BT 470 M", "BT 470 BG", "SMPTE 240 M", "Linear", "Log 100", "Log 316", "IEC 61966-2-4", "BT 1361 E", "IEC 61966-2-1", "BT 2020-10", "BT 2020-12", "SMPTE 2084", "SMPTE 428", "ARIB-STD-B67"}},
                        New OptionParam With {.Switch = "--colorrange", .Text = "Colorrange", .Options = {"Limited", "Full", "Auto"}},
                        MaxCLL, MaxFALL, Chromaloc,
                        New BoolParam With {.Switch = "--pic-struct", .Text = "Set the picture structure and emits it in the picture timing SEI message"},
                        New BoolParam With {.Switch = "--fullrange", .Text = "Fullrange"},
                        New BoolParam With {.Switch = "--aud", .Text = "AUD"})
                    Add("Input/Output",
                        New StringParam With {.Switch = "--input-option", .Text = "Input Option"},
                        Decoder,
                        New OptionParam With {.Switch = "--input-csp", .Text = "Input CSP", .Init = 2, .Options = {"Invalid", "NV12", "YV12", "YUV420P", "YUV422P", "YUV444P", "YUV420P9LE", "YUV420P10LE", "YUV420P12LE", "YUV420P14LE", "YUV420P16LE", "P010", "YUV422P9LE", "YUV422P10LE", "YUV422P12LE", "YUV422P14LE", "YUV422P16LE", "YUV444P9LE", "YUV444P10LE", "YUV444P12LE", "YUV444P14LE", "YUV444P16LE"}},
                        New OptionParam With {.Switch = "--interlace", .Text = "Interlace", .Options = {"Undefined", "TFF", "BFF"}})
                    Add("Other",
                        New StringParam With {.Text = "Custom", .Quotes = QuotesMode.Never, .AlwaysOn = True},
                        New StringParam With {.Switch = "--data-copy", .Text = "Data Copy"},
                        New StringParam With {.Switch = "--thread-affinity", .Text = "Thread Affinity"},
                        New OptionParam With {.Switches = {"--disable-d3d", "--d3d9", "--d3d11", "--d3d"}, .Text = "D3D", .Options = {"Disabled", "D3D9", "D3D11", "D3D9/D3D11"}, .Values = {"--disable-d3d", "--d3d9", "--d3d11", "--d3d"}, .Init = 3},
                        New OptionParam With {.Switch = "--log-level", .Text = "Log Level", .Options = {"Info", "Debug", "Warn", "Error", "Quiet"}},
                        New OptionParam With {.Switch = "--sao", .Text = "SAO", .Options = {"Auto", "None", "Luma", "Chroma", "All"}, .VisibleFunc = Function() Codec.ValueText = "hevc"})
                    Add("Other 2",
                        New NumParam With {.Switch = "--max-framesize", .Text = "Max frame size in bytes", .Config = {0, Integer.MaxValue}, .VisibleFunc = Function() Bitrate.Visible},
                        New NumParam With {.Switch = "--max-framesize-i", .Text = "Max frame size in bytes for I-frames", .Config = {0, Integer.MaxValue}, .VisibleFunc = Function() Bitrate.Visible},
                        New NumParam With {.Switch = "--max-framesize-p", .Text = "Max frame size in bytes for P/B frames", .Config = {0, Integer.MaxValue}, .VisibleFunc = Function() Bitrate.Visible},
                        New NumParam With {.Switch = "--tile-row", .Text = "Number of tile rows", .Init = 2, .DefaultValue = 1, .Config = {0, Integer.MaxValue}, .VisibleFunc = Function() Codec.ValueText = "av1"},
                        New NumParam With {.Switch = "--tile-col", .Text = "Number of tile columns", .Init = 1, .Config = {0, 100}, .VisibleFunc = Function() Codec.ValueText = "av1"},
                        New BoolParam With {.Switch = "--no-deblock", .Text = "No Deblock", .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New BoolParam With {.Switch = "--fallback-rc", .Text = "Enable fallback for unsupported modes", .Init = True},
                        New BoolParam With {.Switch = "--timer-period-tuning", .NoSwitch = "--no-timer-period-tuning", .Text = "Timer Period Tuning", .Init = True},
                        New BoolParam With {.Switch = "--fixed-func", .Text = "Use fixed func instead of GPU EU"},
                        New BoolParam With {.Switch = "--hevc-gpb", .Text = "Use GPB for P-frames", .VisibleFunc = Function() Codec.ValueText = "hevc"},
                        New BoolParam With {.Switch = "--bluray", .Text = "Blu-ray"},
                        New BoolParam With {.Switch = "--tskip", .Text = "Transform Skip", .VisibleFunc = Function() Codec.ValueText = "hevc"},
                        New BoolParam With {.Switch = "--fade-detect", .Text = "Fade Detection"},
                        New BoolParam With {.Switch = "--lowlatency", .Text = "Low Latency"},
                        New BoolParam With {.Switch = "--timecode", .Text = "Output timecode file"})
                End If

                Return ItemsValue
            End Get
        End Property

        Public Overrides Sub ShowHelp(options As String())
            ShowConsoleHelp(Package.QSVEncC, options)
        End Sub

        Protected Overrides Sub OnValueChanged(item As CommandLineParam)
            If QPI.NumEdit IsNot Nothing Then
                mctfval.NumEdit.Enabled = mctf.Value

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
                TweakSwapuv.MenuButton.Enabled = Tweak.Value

                For Each i In {DebandRange, DebandSample, DebandThre, DebandThreY, DebandThreCB, DebandThreCR, DebandDither, DebandDitherY, DebandDitherC, DebandSeed}
                    i.NumEdit.Enabled = Deband.Value
                Next

                DebandRandEachFrame.CheckBox.Enabled = Deband.Value
                DebandBlurfirst.CheckBox.Enabled = Deband.Value
            End If

            MyBase.OnValueChanged(item)
        End Sub

        Overrides Function GetCommandLine(
            includePaths As Boolean,
            includeExecutable As Boolean,
            Optional pass As Integer = 1) As String

            Dim ret As String = ""
            Dim sourcePath = p.Script.Path
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)

            If includePaths AndAlso includeExecutable Then
                ret = Package.QSVEncC.Path.Escape
            End If

            Select Case Decoder.ValueText
                Case "avs"
                    sourcePath = p.Script.Path

                    If includePaths AndAlso FrameServerHelp.IsPortable Then
                        ret += " --avsdll " + Package.AviSynth.Path.Escape
                    End If
                Case "qshw"
                    sourcePath = p.LastOriginalSourceFile
                    ret += " --avhw"
                Case "qssw"
                    sourcePath = p.LastOriginalSourceFile
                    ret += " --avsw"
                Case "ffdxva"
                    sourcePath = "-"

                    If includePaths Then
                        Dim pix_fmt = If(p.SourceVideoBitDepth = 10, "yuv420p10le", "yuv420p")
                        ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel dxva2 -i " +
                            If(includePaths, p.LastOriginalSourceFile.Escape, "path") + " -f yuv4mpegpipe -pix_fmt " +
                            pix_fmt + " -strict -1 -loglevel fatal -hide_banner - | " +
                            If(includePaths, Package.QSVEncC.Path.Escape, "QSVEncC64")
                    End If
                Case "ffqsv"
                    sourcePath = "-"

                    If includePaths Then
                        ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.LastOriginalSourceFile.Escape, "path") + " -f yuv4mpegpipe -strict -1 -pix_fmt yuv420p -loglevel fatal -hide_banner - | " + If(includePaths, Package.QSVEncC.Path.Escape, "QSVEncC64")
                    End If
            End Select

            Select Case Mode.ValueText
                Case "avbr", "cbr", "la", "la-hrd", "qvbr", "vbr", "vcm"
                    ret += " --" + Mode.ValueText + " " & If(pass = 1, Bitrate.Value, p.VideoBitrate)
                Case "icq", "la-icq"
                    ret += " --" + Mode.ValueText + " " & CInt(Quality.Value)
                Case "cqp"
                    ret += " --cqp " & CInt(QPI.Value) & ":" & CInt(QPP.Value) & ":" & CInt(QPB.Value)

                    If QPOffsetI.Value <> QPOffsetI.DefaultValue OrElse
                        QPOffsetP.Value <> QPOffsetP.DefaultValue OrElse
                        QPOffsetB.Value <> QPOffsetB.DefaultValue Then

                        ret += " --qp-offset " & CInt(QPOffsetI.Value) & ":" & CInt(QPOffsetP.Value) & ":" & CInt(QPOffsetB.Value)
                    End If
                Case Else
                    ret += " --" + Mode.ValueText + " " & p.VideoBitrate
            End Select

            Dim q = From i In Items Where i.GetArgs <> ""

            If q.Count > 0 Then
                ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")
            End If

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

        Function GetmctfArgs() As String
            If mctf.Value Then
                Dim ret = ""

                If mctfval.Value <> mctfval.DefaultValue Then
                    ret += "" & mctfval.Value
                End If

                If ret <> "" Then
                    Return "--vpp-mctf " + ret.TrimStart(","c)
                Else
                    Return "--vpp-mctf"
                End If
            End If
            Return ""
        End Function

        Function GetSmoothArgs() As String
            If Smooth.Value Then
                Dim ret = ""
                If SmoothQuality.Value <> SmoothQuality.DefaultValue Then ret += ",quality=" & SmoothQuality.Value
                If SmoothQP.Value <> SmoothQP.DefaultValue Then ret += ",qp=" & SmoothQP.Value.ToInvariantString
                If SmoothPrec.Value <> SmoothPrec.DefaultValue Then ret += ",prec=" & SmoothPrec.ValueText
                Return "--vpp-smooth " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetPmdArgs() As String
            If Pmd.Value Then
                Dim ret = ""
                If PmdApplyCount.Value <> PmdApplyCount.DefaultValue Then ret += ",apply_count=" & PmdApplyCount.Value
                If PmdStrength.Value <> PmdStrength.DefaultValue Then ret += ",strength=" & PmdStrength.Value.ToInvariantString
                If PmdThreshold.Value <> PmdThreshold.DefaultValue Then ret += ",threshold=" & PmdThreshold.Value
                Return "--vpp-pmd " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetDenoiseArgs() As String
            If Denoise.Value Then
                Dim ret = ""
                If DenoiseMode.Value <> DenoiseMode.DefaultValue Then ret += ",mode=" & DenoiseMode.ValueText
                If DenoiseStrength.Value <> DenoiseStrength.DefaultValue Then ret += ",strength=" & DenoiseStrength.Value
                Return "--vpp-denoise " + ret.TrimStart(","c)
            End If
            Return ""
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
            Return ""
        End Function

        Function GetTweakArgs() As String
            If Tweak.Value Then
                Dim ret = ""
                If TweakBrightness.Value <> TweakBrightness.DefaultValue Then ret += ",brightness=" & TweakBrightness.Value.ToInvariantString
                If TweakContrast.Value <> TweakContrast.DefaultValue Then ret += ",contrast=" & TweakContrast.Value.ToInvariantString
                If TweakSaturation.Value <> TweakSaturation.DefaultValue Then ret += ",saturation=" & TweakSaturation.Value.ToInvariantString
                If TweakGamma.Value <> TweakGamma.DefaultValue Then ret += ",gamma=" & TweakGamma.Value.ToInvariantString
                If TweakHue.Value <> TweakHue.DefaultValue Then ret += ",hue=" & TweakHue.Value.ToInvariantString
                If TweakSwapuv.Value <> TweakSwapuv.DefaultValue Then ret += ",swapuv=" & TweakSwapuv.ValueText.ToInvariantString
                Return ("--vpp-tweak " + ret.TrimStart(","c)).Trim()
            End If
            Return ""
        End Function

        Function GetPaddingArgs() As String
            Return If(Pad.Value, $"--vpp-pad {PadLeft.Value},{PadTop.Value},{PadRight.Value},{PadBottom.Value}", "")
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
            Return ""
        End Function

        Function GetTransform() As String
            Dim ret = ""
            If TransformFlipX.Value Then ret += ",flip_x=true"
            If TransformFlipY.Value Then ret += ",flip_y=true"
            If TransformTranspose.Value Then ret += ",transpose=true"
            If ret <> "" Then Return ("--vpp-transform " + ret.TrimStart(","c))
            Return ""
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
            Return ""
        End Function

        Function GetUnsharp() As String
            If Unsharp.Value Then
                Dim ret = ""
                If UnsharpRadius.Value <> UnsharpRadius.DefaultValue Then ret += ",radius=" & UnsharpRadius.Value.ToInvariantString
                If UnsharpWeight.Value <> UnsharpWeight.DefaultValue Then ret += ",weight=" & UnsharpWeight.Value.ToInvariantString
                If UnsharpThreshold.Value <> UnsharpThreshold.DefaultValue Then ret += ",threshold=" & UnsharpThreshold.Value.ToInvariantString
                Return "--vpp-unsharp " + ret.TrimStart(","c)
            End If
            Return ""
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
            Return ""
        End Function


        Public Overrides Function GetPackage() As Package
            Return Package.QSVEncC
        End Function
    End Class
End Class
