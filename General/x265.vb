Imports System.Globalization
Imports System.Text

Imports StaxRip.CommandLine

<Serializable()>
Public Class x265Encoder
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Sub New()
        Name = "x265"
        AutoCompCheckValue = 50
    End Sub

    <NonSerialized>
    Private ParamsValue As x265Params

    Property Params As x265Params
        Get
            If ParamsValue Is Nothing Then
                ParamsValue = New x265Params

                ParamsValue.Init(ParamsStore)
            End If

            Return ParamsValue
        End Get
        Set(value As x265Params)
            ParamsValue = value
        End Set
    End Property

    Overrides ReadOnly Property OutputFileType As String
        Get
            Return "hevc"
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Encode("Encoding video using x265 " + Packs.x265.Version, GetArgs(1, p.Script), p.Script, s.ProcessPriority)

        If Params.Mode.Value = x265RateMode.TwoPass OrElse
            Params.Mode.Value = x265RateMode.ThreePass Then

            Encode("Encoding video second pass using x265 " + Packs.x265.Version, GetArgs(2, p.Script), p.Script, s.ProcessPriority)
        End If

        If Params.Mode.Value = x265RateMode.ThreePass Then
            Encode("Encoding video third pass using x265 " + Packs.x265.Version, GetArgs(3, p.Script), p.Script, s.ProcessPriority)
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(passName As String,
                         args As String,
                         script As VideoScript,
                         priority As ProcessPriorityClass)

        Dim cli As String

        If p.Script.Engine = ScriptingEngine.VapourSynth Then
            cli = """" + Packs.vspipe.GetPath + """ """ + script.Path + """ - --y4m | """ + Packs.x265.GetPath + """ " + args
        Else
            cli = """" + Packs.ffmpeg.GetPath + """ -i """ + script.Path + """ -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | """ + Packs.x265.GetPath + """ " + args
        End If

        Dim batchPath = p.TempDir + Filepath.GetBase(p.TargetFile) + "_encode.bat"
        File.WriteAllText(batchPath, cli, Encoding.GetEncoding(850))

        Using proc As New Proc
            proc.Init(passName)
            proc.Priority = priority
            proc.SkipStrings = {"%] "}
            proc.WriteLine(cli + CrLf2)
            proc.File = "cmd.exe"
            proc.Arguments = "/C call """ + batchPath + """"
            proc.Start()
        End Using
    End Sub

    Overrides Sub RunCompCheck()
        If Not Paths.VerifyRequirements Then Exit Sub
        If Not g.IsValidSource Then Exit Sub

        Dim newParams As New x265Params
        Dim newStore = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(newStore)

        Dim enc As New x265Encoder
        enc.Params = newParams
        enc.Params.Mode.Value = x265RateMode.SingleCRF
        enc.Params.Quant.Value = enc.Params.CompCheckQuant.Value

        Dim script As New VideoScript
        script.Engine = p.Script.Engine
        script.Filters = p.Script.GetFiltersCopy
        Dim code As String
        Dim every = ((100 \ p.CompCheckRange) * 14).ToString

        If script.Engine = ScriptingEngine.AviSynth Then
            code = "SelectRangeEvery(" + every + ",14)"
        Else
            code = "fpsnum = clip.fps_num" + CrLf + "fpsden = clip.fps_den" + CrLf +
            "clip = core.std.SelectEvery(clip = clip, cycle = " + every + ", offsets = range(14))" + CrLf +
            "clip = core.std.AssumeFPS(clip = clip, fpsnum = fpsnum, fpsden = fpsden)"
        End If

        Log.WriteLine(code + CrLf2)
        script.Filters.Add(New VideoFilter("aaa", "aaa", code))
        script.Path = p.TempDir + p.Name + "_CompCheck." + script.FileType
        script.Synchronize()

        Dim arguments = enc.Params.GetArgs(0, script, p.TempDir + p.Name + "_CompCheck." + OutputFileType, True, False)

        Try
            Encode("Compressibility Check", arguments, script, ProcessPriorityClass.Normal)
        Catch ex As AbortException
            ProcessForm.CloseProcessForm()
            Exit Sub
        Catch ex As Exception
            ProcessForm.CloseProcessForm()
            g.ShowException(ex)
            Exit Sub
        End Try

        Dim bits = (New FileInfo(p.TempDir + p.Name + "_CompCheck." + OutputFileType).Length) * 8
        p.Compressibility = (bits / script.GetFrames) / (p.TargetWidth * p.TargetHeight)

        OnAfterCompCheck()
        g.MainForm.Assistant()

        Log.WriteLine("Quality: " & CInt(Calc.GetPercent).ToString() + " %")
        Log.WriteLine("Compressibility: " + p.Compressibility.ToString("f3"))
        Log.Save()

        ProcessForm.CloseProcessForm()
    End Sub

    Overloads Function GetArgs(pass As Integer, script As VideoScript, Optional includePaths As Boolean = True) As String
        Return Params.GetArgs(pass, script, Filepath.GetDirAndBase(OutputPath) +
                       "." + OutputFileType, includePaths, False)
    End Function

    Overrides Sub ShowConfigDialog()
        Dim newParams As New x265Params
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(store)
        newParams.ApplyPresetDefaultValues()
        newParams.ApplyTuneDefaultValues()

        Using f As New CommandLineForm(newParams)
            If f.ShowDialog() = DialogResult.OK Then
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides Property QualityMode() As Boolean
        Get
            Return Params.Mode.Value = x265RateMode.SingleQuant OrElse
                Params.Mode.Value = x265RateMode.SingleCRF
        End Get
        Set(Value As Boolean)
        End Set
    End Property

    Public Overrides ReadOnly Property CommandLineParams As CommandLineParams
        Get
            Return Params
        End Get
    End Property

    Sub SetMuxer()
        Muxer = New MkvMuxer()
    End Sub

    Overrides Function CreateEditControl() As Control
        Return New x265Control(Me) With {.Dock = DockStyle.Fill}
    End Function
End Class

Public Class x265Params
    Inherits CommandLineParams

    Sub New()
        Title = "x265 Options"
    End Sub

    Property Quant As New NumParam With {
        .Switch = "--crf",
        .Name = "Quant",
        .Text = "Quality:",
        .ArgsFunc = Function() Nothing,
        .Value = 22,
        .DefaultValue = 28,
        .MinMaxStepDec = {0D, 51D, 1D, 1D}}

    Property Preset As New OptionParam With {
        .Switch = "--preset",
        .Text = "Preset:",
        .Options = {"Ultra Fast", "Super Fast", "Very Fast", "Faster", "Fast", "Medium", "Slow", "Slower", "Very Slow", "Placebo"},
        .Values = {"ultrafast", "superfast", "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow", "placebo"},
        .Value = 5,
        .DefaultValue = 5}

    Property Tune As New OptionParam With {
        .Switch = "--tune",
        .Text = "Tune:",
        .Options = {"None", "PSNR", "SSIM", "Grain", "Fast Decode", "Zero Latency"},
        .Values = {"none", "psnr", "ssim", "grain", "fastdecode", "zerolatency"}}

    Property Mode As New OptionParam With {
        .Name = "Mode",
        .Text = "Mode:",
        .Options = {"Bitrate", "Quantizer", "Quality", "Two Pass", "Three Pass"},
        .Value = 2}

    Property SSIM As New BoolParam With {
        .Switch = "--ssim",
        .Text = "SSIM"}

    Property PSNR As New BoolParam With {
        .Switch = "--psnr",
        .Text = "PSNR"}

    Property BFrames As New NumParam With {
        .Switch = "--bframes",
        .Text = "B Frames:",
        .MinMaxStep = {0, 16, 1}}

    Property BFrameBias As New NumParam With {
        .Switch = "--bframe-bias",
        .Text = "B Frame Bias:",
        .MinMaxStep = {-90, 100, 1}}

    Property BAdapt As New OptionParam With {
        .Switch = "--b-adapt",
        .Text = "B Adapt:",
        .Options = {"None", "Fast", "Full"}}

    Property RCLookahead As New NumParam With {
        .Switch = "--rc-lookahead",
        .Text = "RC Lookahead:",
        .MinMaxStep = {0, 250, 5}}

    Property LookaheadSlices As New NumParam With {
        .Switch = "--lookahead-slices",
        .Text = "Lookahead Slices:",
        .MinMaxStep = {0, 16, 1}}

    Property Scenecut As New NumParam With {
        .Switch = "--scenecut",
        .Text = "Scenecut:",
        .MinMaxStep = {0, 900, 10}}

    Property Ref As New NumParam With {
        .Switch = "--ref",
        .Text = "References:",
        .MinMaxStep = {1, 16, 1}}

    Property [Me] As New OptionParam With {
        .Switch = "--me",
        .Text = "Motion Search Method:",
        .Options = {"Diamond", "Hexagon", "Uneven Multi-Hexegon", "Star", "Full"},
        .Values = {"dia", "hex", "umh", "star", "full"}}

    Property MErange As New NumParam With {
        .Switch = "--merange",
        .Text = "ME Range (0=auto):",
        .MinMaxStep = {0, 32768, 1},
        .ArgsFunc = Function() If(MErange.Value = 0, "--merange " & CInt(Calc.GetYFromTwoPointForm(480, 16, 2160, 57, p.TargetHeight)), If(MErange.Value <> MErange.DefaultValue, "--merange " & CInt(MErange.Value), Nothing))}

    Property SubME As New OptionParam With {
        .Switch = "--subme",
        .Text = "Subpel Refinement:",
        .Options = {"0 - HPEL 1/4 - QPEL 0/4 - HPEL SATD false",
                    "1 - HPEL 1/4 - QPEL 1/4 - HPEL SATD false",
                    "2 - HPEL 1/4 - QPEL 1/4 - HPEL SATD true",
                    "3 - HPEL 2/4 - QPEL 1/4 - HPEL SATD true",
                    "4 - HPEL 2/4 - QPEL 2/4 - HPEL SATD true",
                    "5 - HPEL 1/8 - QPEL 1/8 - HPEL SATD true",
                    "6 - HPEL 2/8 - QPEL 1/8 - HPEL SATD true",
                    "7 - HPEL 2/8 - QPEL 2/8 - HPEL SATD true"},
        .Expand = True}

    Property MaxMerge As New NumParam With {
        .Switch = "--max-merge",
        .Text = "Max Merge:",
        .MinMaxStep = {1, 5, 1}}

    Property SAOnonDeblock As New BoolParam With {
        .Switch = "--sao-non-deblock",
        .Text = "Specify how to handle depencency between SAO and deblocking filter"}

    Property SAO As New BoolParam With {
        .Switch = "--sao",
        .NoSwitch = "--no-sao",
        .Text = "Sample Adaptive Offset loop filter"}

    Property SignHide As New BoolParam With {
        .Switch = "--signhide",
        .NoSwitch = "--no-signhide",
        .Text = "Hide sign bit of one coeff per TU (rdo)"}

    Property CompCheckQuant As New NumParam With {
        .Name = "CompCheckQuant",
        .Text = "Comp. Check Quant:",
        .Value = 18,
        .MinMaxStep = {1, 50, 1}}

    Property Weightp As New BoolParam With {
        .Switch = "--weightp",
        .NoSwitch = "--no-weightp",
        .Text = "Enable weighted prediction in P slices"}

    Property Weightb As New BoolParam With {
        .Switch = "--weightb",
        .NoSwitch = "--no-weightb",
        .Text = "Enable weighted prediction in B slices"}

    Property AQmode As New OptionParam With {
        .Switch = "--aq-mode",
        .Text = "AQ Mode:",
        .Options = {"Disabled", "Enabled", "Auto-Variance", "Auto-Variance and bias to dark scenes"}}

    Property Videoformat As New OptionParam With {
        .Switch = "--videoformat",
        .Text = "Videoformat:",
        .Options = {"undefined", "component", "pal", "ntsc", "secam", "mac"},
        .Values = {"", "component", "pal", "ntsc", "secam", "mac"}}

    Property AQStrength As New NumParam With {
        .Switch = "--aq-strength",
        .Text = "AQ Strength:",
        .MinMaxStepDec = {0D, 3D, 0.05D, 2D}}

    Property CUtree As New BoolParam With {
        .Switch = "--cutree",
        .NoSwitch = "--no-cutree",
        .Text = "CU Tree",
        .Value = True}

    Property RD As New OptionParam With {
        .Switch = "--rd",
        .Text = "RD:",
        .Options = {"0 - SA8D mode and split decisions, intra w/ source pixels",
                    "1 - Recon generated (better intra), RDO merge/skip selection",
                    "2 - RDO splits and merge/skip selection",
                    "3 - RDO mode and split decisions, chroma residual used for sa8d",
                    "4 - Currently same as 3",
                    "5 - Adds RDO prediction decisions",
                    "6 - Currently same as 5"},
        .Expand = True}

    Property MinCuSize As New OptionParam With {
        .Switch = "--min-cu-size",
        .Text = "Minimum CU size:",
        .Options = {"64", "32", "16", "8"},
        .Values = {"64", "32", "16", "8"}}

    Property MaxCuSize As New OptionParam With {
        .Switch = "--ctu",
        .Text = "Maximum CU size:",
        .Options = {"64", "32", "16"},
        .Values = {"64", "32", "16"}}

    Property qgSize As New OptionParam With {
        .Switch = "--qg-size",
        .Text = "QG Size:",
        .ValueIsName = True,
        .Options = {"64", "32", "16"},
        .Value = 1,
        .DefaultValue = 1}

    Property qpstep As New NumParam With {
        .Switch = "--qpstep",
        .Text = "QP Step:",
        .Value = 4,
        .DefaultValue = 4}

    Property TUintra As New NumParam With {
        .Switch = "--tu-intra-depth",
        .Text = "TU Intra Depth:",
        .MinMaxStep = {1, 4, 1}}

    Property TUinter As New NumParam With {
        .Switch = "--tu-inter-depth",
        .Text = "TU Inter Depth:",
        .MinMaxStep = {1, 4, 1}}

    Property rdoqLevel As New NumParam With {
        .Switch = "--rdoq-level",
        .Text = "RDOQ Level:",
        .MinMaxStep = {0, 2, 1}}

    Property Rect As New BoolParam With {
        .Switch = "--rect",
        .NoSwitch = "--no-rect",
        .Text = "Enable analysis of rectangular motion partitions Nx2N and 2NxN"}

    Property rcGrain As New BoolParam With {
        .Switch = "--rc-grain",
        .NoSwitch = "--no-rc-grain",
        .Text = "Specialised ratecontrol for film grain content"}

    Property AMP As New BoolParam With {
        .Switch = "--amp",
        .NoSwitch = "--no-amp",
        .Text = "Enable analysis of asymmetric motion partitions"}

    Property EarlySkip As New BoolParam With {
        .Switch = "--early-skip",
        .NoSwitch = "--no-early-skip",
        .Text = "Early Skip"}

    Property FastIntra As New BoolParam With {
        .Switch = "--fast-intra",
        .NoSwitch = "--no-fast-intra",
        .Text = "Fast Intra"}

    Property BIntra As New BoolParam With {
        .Switch = "--b-intra",
        .NoSwitch = "--no-b-intra",
        .Text = "Evaluate intra modes in B slices"}

    Property CUlossless As New BoolParam With {
        .Switch = "--cu-lossless",
        .Text = "CU Lossless"}

    Property Tskip As New BoolParam With {
        .Switch = "--tskip",
        .Text = "Enable evaluation of transform skip coding for 4x4 TU coded blocks"}

    Property TskipFast As New BoolParam With {
        .Switch = "--tskip-fast",
        .Text = "Only evaluate transform skip for NxN intra predictions (4x4 blocks)"}

    Property PsyRD As New NumParam With {
        .Switch = "--psy-rd",
        .Text = "Psy RD:",
        .MinMaxStepDec = {0D, 5D, 0.05D, 2D}}

    Property PsyRDOQ As New NumParam With {
        .Switch = "--psy-rdoq",
        .Text = "Psy RDOQ:",
        .MinMaxStepDec = {0D, 50D, 0.05D, 2D}}

    Property CRFmax As New NumParam With {
        .Switch = "--crf-max",
        .Text = "Maximum CRF:",
        .MinMaxStepDec = {0D, 51D, 1D, 1D},
        .Value = 51,
        .DefaultValue = 51}

    Property CRFmin As New NumParam With {
        .Switch = "--crf-min",
        .Text = "Minimum CRF:",
        .MinMaxStepDec = {0D, 51D, 1D, 1D}}

    Property PBRatio As New NumParam With {
        .Switch = "--pbratio",
        .Text = "PB Ratio:",
        .MinMaxStepDec = {0D, 1000D, 0.05D, 2D}}

    Property IPRatio As New NumParam With {
        .Switch = "--ipratio",
        .Text = "IP Ratio:",
        .MinMaxStepDec = {0D, 1000D, 0.05D, 2D}}

    Property QComp As New NumParam With {
        .Switch = "--qcomp",
        .Text = "qComp:",
        .MinMaxStepDec = {0D, 1000D, 0.05D, 2D}}

    Property QBlur As New NumParam With {
        .Switch = "--qblur",
        .Text = "Q Blur:",
        .MinMaxStepDec = {Decimal.MinValue, Decimal.MaxValue, 0.05D, 2D},
        .Value = 0.5,
        .DefaultValue = 0.5}

    Property Cplxblur As New NumParam With {
        .Switch = "--cplxblur",
        .Text = "Blur Complexity:",
        .MinMaxStepDec = {Decimal.MinValue, Decimal.MaxValue, 0.05D, 2D},
        .Value = 20,
        .DefaultValue = 20}

    Property LogLevel As New OptionParam With {
        .Switch = "--log-level",
        .Text = "Log Level:",
        .ValueIsName = True,
        .Options = {"none", "error", "warning", "info", "debug", "full"},
        .Value = 3,
        .DefaultValue = 3}

    Property Colorprim As New OptionParam With {
        .Switch = "--colorprim",
        .Text = "Colorprim:",
        .ValueIsName = True,
        .Options = {"undefined", "bt709", "bt470m", "bt470bg", "smpte170m", "smpte240m", "film", "bt2020"}}

    Property Transfer As New OptionParam With {
        .Switch = "--transfer",
        .Text = "Transfer:",
        .ValueIsName = True,
        .Options = {"undefined", "bt709", "bt470m", "bt470bg", "smpte170m", "smpte240m", "linear", "log100", "log316", "iec61966-2-4", "bt1361e", "iec61966-2-1", "bt2020-10", "bt2020-12", "smpte-st-2084", "smpte-st-428", "arib-std-b67"}}

    Property Colormatrix As New OptionParam With {
        .Switch = "--colormatrix",
        .Text = "Colormatrix:",
        .ValueIsName = True,
        .Options = {"undefined", "GBR", "bt709", "fcc", "bt470bg", "smpte170m", "smpte240m", "YCgCo", "bt2020nc", "bt2020c"}}

    Property Pools As New StringParam With {
        .Switch = "--pools",
        .Text = "Pools:",
        .UseQuotes = True}

    Property WPP As New BoolParam With {
        .Switch = "--wpp",
        .NoSwitch = "--no-wpp",
        .Text = "Wavefront Parallel Processing",
        .Value = True,
        .DefaultValue = True}

    Property Pmode As New BoolParam With {
        .Switch = "--pmode",
        .NoSwitch = "--no-pmode",
        .Text = "Parallel Mode Decision"}

    Property PME As New BoolParam With {
        .Switch = "--pme",
        .NoSwitch = "--no-pme",
        .Text = "Parallel Motion Estimation"}

    Property Dither As New BoolParam With {
        .Switch = "--dither",
        .Text = "Enable high quality downscaling"}

    Property minLuma As New NumParam With {
        .Switch = "--min-luma",
        .Text = "Minimum Luma:"}

    Property maxLuma As New NumParam With {
        .Switch = "--max-luma",
        .Text = "Maximum Luma:"}

    Property InterlaceMode As New OptionParam With {
        .Switch = "--interlace",
        .Text = "Interlace Mode:",
        .Options = {"Progressive", "Top field first", "Bottom field first"},
        .Values = {"", "tff", "bff"}}

    Property Profile As New OptionParam With {
        .Switch = "--profile",
        .Text = "Profile:",
        .ValueIsName = True,
        .Options = {"Unrestricted", "main", "main-intra", "mainstillpicture", "main422-8", "main444-intra", "main444-stillpicture", "main444-8", "main10", "main10-intra", "main422-10", "main422-10-intra", "main444-10", "main444-10-intra"}}

    Property OutputDepth As New OptionParam With {
        .Switch = "--output-depth",
        .Text = "Depth:",
        .ValueIsName = True,
        .Options = {"8", "10", "12"}}

    Property Level As New OptionParam With {
        .Switch = "--level-idc",
        .Text = "Level:",
        .ValueIsName = True,
        .Options = {"Unrestricted", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2", "8.5"}}

    Property Hash As New OptionParam With {
        .Switch = "--hash",
        .Text = "Hash:",
        .Options = {"None", "MD5", "CRC", "Checksum"},
        .Values = {"", "MD5", "CRC", "Checksum"}}

    Property HighTier As New BoolParam With {
        .Switch = "--high-tier",
        .Text = "High Tier"}

    Property TemporalMVP As New BoolParam With {
        .Switch = "--temporal-mvp",
        .NoSwitch = "--no-temporal-mvp",
        .Text = "Enable temporal motion vector predictors in P and B slices",
        .Value = True,
        .DefaultValue = True}

    Property StrongIntraSmoothing As New BoolParam With {
        .Switch = "--strong-intra-smoothing",
        .NoSwitch = "--no-strong-intra-smoothing",
        .Text = "Enable strong intra smoothing for 32x32 intra blocks",
        .Value = True,
        .DefaultValue = True}

    Property ConstrainedIntra As New BoolParam With {
        .Switch = "--constrained-intra",
        .NoSwitch = "--no-constrained-intra",
        .Text = "Constrained Intra Prediction",
        .Value = True,
        .DefaultValue = True}

    Property RDpenalty As New NumParam With {
        .Switch = "--rdpenalty",
        .Text = "RD Penalty (0=disabled):",
        .MinMaxStep = {0, 2, 1}}

    Property OpenGop As New BoolParam With {
        .Switch = "--open-gop",
        .NoSwitch = "--no-open-gop",
        .Text = "Open GOP",
        .Value = True,
        .DefaultValue = True}

    Property Bpyramid As New BoolParam With {
        .Switch = "--b-pyramid",
        .NoSwitch = "--no-b-pyramid",
        .Text = "B Pyramid",
        .Value = True,
        .DefaultValue = True}

    Property Lossless As New BoolParam With {
        .Switch = "--lossless",
        .Text = "Lossless"}

    Property SlowFirstpass As New BoolParam With {
        .Switch = "--slow-firstpass",
        .Text = "Slow Firstpass",
        .Value = False,
        .DefaultValue = False}

    Property StrictCBR As New BoolParam With {
        .Switch = "--strict-cbr",
        .Text = "Strict CBR"}

    Property CBQPoffs As New NumParam With {
        .Switch = "--cbqpoffs",
        .Text = "Cb QP Offset:",
        .MinMaxStep = {-12, 12, 1}}

    Property CRQPoffs As New NumParam With {
        .Switch = "--crqpoffs",
        .Text = "CR QP Offset:",
        .MinMaxStep = {-12, 12, 1}}

    Property NRintra As New NumParam With {
        .Switch = "--nr-intra",
        .Text = "Intra Noise Reduction:",
        .MinMaxStep = {0, 2000, 50}}

    Property NRinter As New NumParam With {
        .Switch = "--nr-inter",
        .Text = "Inter Noise Reduction:",
        .MinMaxStep = {0, 2000, 50}}

    Property Keyint As New NumParam With {
        .Switch = "--keyint",
        .Text = "Maximum GOP Size:",
        .MinMaxStep = {0, 10000, 10},
        .Value = 250,
        .DefaultValue = 250}

    Property MinKeyint As New NumParam With {
        .Switch = "--min-keyint",
        .Text = "Minimum GOP Size:",
        .MinMaxStep = {0, 10000, 10}}

    Property VBVbufsize As New NumParam With {
        .Switch = "--vbv-bufsize",
        .Text = "VBV Bufsize:",
        .LabelMargin = New Padding With {.Right = 6, .Left = 6},
        .MinMaxStep = {0, 1000000, 100}}

    Property VBVmaxrate As New NumParam With {
        .Switch = "--vbv-maxrate",
        .Text = "VBV Maxrate:",
        .LabelMargin = New Padding With {.Right = 7},
        .MinMaxStep = {0, 1000000, 100}}

    Property VBVinit As New NumParam With {
        .Switch = "--vbv-init",
        .Text = "VBV Init:",
        .MinMaxStepDec = {0D, 1D, 0.05D, 2D},
        .Value = 0.9,
        .DefaultValue = 0.9}

    Property Chromaloc As New NumParam With {
        .Switch = "--chromaloc",
        .Text = "Chromaloc:",
        .MinMaxStep = {0, 5, 1}}

    Property FrameThreads As New NumParam With {
        .Switch = "--frame-threads",
        .Text = "Frame Threads (0=auto):"}

    Property RepeatHeaders As New BoolParam With {
        .Switch = "--repeat-headers",
        .Text = "Repeat Headers"}

    Property Info As New BoolParam With {
        .Switch = "--info",
        .NoSwitch = "--no-info",
        .Text = "Info",
        .Value = True,
        .DefaultValue = True}

    Property HRD As New BoolParam With {
        .Switch = "--hrd",
        .Text = "HRD"}

    Property AUD As New BoolParam With {
        .Switch = "--aud",
        .Text = "AUD"}

    Property AllowNonConformance As New BoolParam With {
        .Switch = "--allow-non-conformance",
        .Text = "Allow non conformance"}

    Property LimitModes As New BoolParam With {
        .Switch = "--limit-modes",
        .Text = "Limit Modes"}

    Property RdRefine As New BoolParam With {
        .Switch = "--rd-refine",
        .Text = "RD Refine"}

    Property MaxCLL As New NumParam With {
        .Text = "Maximum CLL:",
        .Switch = "--max-cll",
        .MinMaxStep = {0, Integer.MaxValue, 50},
        .ArgsFunc = Function() If(MaxCLL.Value <> 0 OrElse MaxFALL.Value <> 0, "--max-cll """ & MaxCLL.Value & "," & MaxFALL.Value & """", "")}

    Property MaxFALL As New NumParam With {
        .MinMaxStep = {0, Integer.MaxValue, 50},
        .ArgsFunc = Function() "",
        .Text = "Maximum FALL:"}

    Property IntraRefresh As New BoolParam With {
        .Switch = "--intra-refresh",
        .Text = "Intra Refresh"}

    Property Custom As New StringParam With {
        .Text = "Custom:",
        .InitAction = Sub(tb)
                          tb.Label.Visible = False
                          tb.Expand(tb.Edit)
                          tb.Edit.Height = CInt(tb.Edit.Font.Height * 15)
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.TextBox.Font = New Font("Consolas", 10)
                      End Sub}

    Property Deblock As New BoolParam With {
        .Switch = "--deblock",
        .Text = "Deblocking",
        .ArgsFunc = AddressOf GetDeblockArgs}

    Property DeblockA As New NumParam With {
        .Text = "      Strength:",
        .MinMaxStep = {-6, 6, 1}}

    Property DeblockB As New NumParam With {
        .Text = "      Threshold:",
        .MinMaxStep = {-6, 6, 1}}

    Property MaxTuSize As New OptionParam With {
        .Switch = "--max-tu-size",
        .Text = "Max TU Size:",
        .Options = {"32", "16", "8", "4"},
        .Values = {"32", "16", "8", "4"}}

    Property LimitRefs As New OptionParam With {
        .Switch = "--limit-refs",
        .Text = "Limit References:",
        .Options = {"0", "1", "2", "3"},
        .Value = 3,
        .DefaultValue = 3}

    Property CSV As New BoolParam With {
        .Switch = "--csv",
        .Text = "Write encoding results to a comma separated value log file",
        .ArgsFunc = Function() If(CSV.Value, "--csv """ + Filepath.GetDirAndBase(p.TargetFile) + ".csv""", Nothing)}

    Property csvloglevel As New OptionParam With {
        .Switch = "--csv-log-level",
        .Text = "CSV Log Level:",
        .Options = {"Default", "Summary", "Frame"}}

    Private ItemsValue As List(Of CommandLineItem)

    Overrides ReadOnly Property Items As List(Of CommandLineItem)
        Get
            If ItemsValue Is Nothing Then
                ItemsValue = New List(Of CommandLineItem)

                Add("Basic", Quant, Preset, Tune, Profile, OutputDepth, Level, Mode)
                Add("Analysis 1", RD, MinCuSize, MaxCuSize, MaxTuSize, LimitRefs, TUintra, TUinter, rdoqLevel)
                Add("Analysis 2", Rect, AMP, EarlySkip, FastIntra, BIntra, CUlossless, Tskip, TskipFast, LimitModes, RdRefine)
                Add("Rate Control 1", AQmode, qgSize, AQStrength, QComp, CBQPoffs, QBlur, Cplxblur, CUtree, Lossless, StrictCBR, rcGrain)
                Add("Rate Control 2", NRintra, NRinter, CRFmin, CRFmax, VBVbufsize, VBVmaxrate, VBVinit, qpstep, IPRatio, PBRatio)
                Add("Motion Search", SubME, [Me], MErange, MaxMerge, Weightp, Weightb, TemporalMVP)
                Add("Slice Decision", BAdapt, BFrames, BFrameBias, RCLookahead, LookaheadSlices, Scenecut, Ref, MinKeyint, Keyint, Bpyramid, OpenGop, IntraRefresh)
                Add("Spatial/Intra", StrongIntraSmoothing, ConstrainedIntra, RDpenalty)
                Add("Performance", Pools, FrameThreads, WPP, Pmode, PME)
                Add("Statistic", LogLevel, csvloglevel, CSV, SSIM, PSNR)
                Add("VUI", Videoformat, Colorprim, Colormatrix, Transfer, minLuma, maxLuma, MaxCLL, MaxFALL)
                Add("Bitstream", Hash, RepeatHeaders, Info, HRD, AUD)
                Add("Other 1", InterlaceMode, Deblock, DeblockA, DeblockB, PsyRD, PsyRDOQ, CompCheckQuant)
                Add("Other 2", SAO, HighTier, SAOnonDeblock, Dither, SlowFirstpass, SignHide, AllowNonConformance)
                Add("Custom", Custom)

                For Each i In ItemsValue
                    If i.Switch <> "" Then
                        i.URL = "http://x265.readthedocs.org/en/latest/cli.html#cmdoption" + i.Switch
                    End If
                Next
            End If

            Return ItemsValue
        End Get
    End Property

    Private AddedList As New List(Of String)

    Private Sub Add(path As String, ParamArray items As CommandLineItem())
        For Each i In items
            i.Path = path
            ItemsValue.Add(i)

            If i.GetKey = "" OrElse AddedList.Contains(i.GetKey) Then
                Throw New Exception
            End If
        Next
    End Sub

    Private BlockValueChanged As Boolean

    Protected Overrides Sub OnValueChanged(item As CommandLineItem)
        If BlockValueChanged Then
            Exit Sub
        End If

        If item Is Preset OrElse item Is Tune Then
            BlockValueChanged = True
            ApplyPresetValues()
            ApplyTuneValues()
            BlockValueChanged = False
        End If

        DeblockA.NumEdit.Enabled = Deblock.Value
        DeblockB.NumEdit.Enabled = Deblock.Value

        MyBase.OnValueChanged(item)
    End Sub

    Overloads Overrides Function GetCommandLine(includePaths As Boolean,
                                                includeExecutable As Boolean,
                                                Optional pass As Integer = 0) As String

        Return GetArgs(1, p.Script, Filepath.GetDirAndBase(p.VideoEncoder.OutputPath) +
                       "." + p.VideoEncoder.OutputFileType, includePaths, includeExecutable)
    End Function

    Overloads Function GetArgs(pass As Integer,
                               script As VideoScript,
                               targetPath As String,
                               includePaths As Boolean,
                               includeExecutable As Boolean) As String

        ApplyPresetDefaultValues()
        ApplyTuneDefaultValues()

        Dim sb As New StringBuilder

        If includePaths AndAlso includeExecutable Then
            If p.Script.Engine = ScriptingEngine.VapourSynth Then
                sb.Append(Packs.vspipe.GetPath.Quotes + " " + script.Path.Quotes + " - --y4m | " + Packs.x265.GetPath.Quotes)
            Else
                sb.Append(Packs.ffmpeg.GetPath.Quotes + " -i " + script.Path.Quotes + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | " + Packs.x265.GetPath.Quotes)
            End If
        End If

        If Mode.Value = x265RateMode.TwoPass OrElse Mode.Value = x265RateMode.ThreePass Then
            sb.Append(" --pass " & pass)
        End If

        If Mode.Value = x265RateMode.SingleQuant Then
            If Not IsCustom("--qp") Then sb.Append(" --qp " + CInt(Quant.Value).ToString)
        ElseIf Mode.Value = x265RateMode.SingleCRF Then
            If Quant.Value <> 28 AndAlso Not IsCustom("--crf") Then
                sb.Append(" --crf " + Quant.Value.ToString(CultureInfo.InvariantCulture))
            End If
        Else
            If Not IsCustom("--bitrate") Then sb.Append(" --bitrate " & p.VideoBitrate)
        End If

        Dim q = From i In Items Where i.GetArgs <> "" AndAlso Not IsCustom(i.Switch)

        If q.Count > 0 Then
            sb.Append(" " + q.Select(Function(item) item.GetArgs).Join(" "))
        End If

        If includePaths Then
            sb.Append(" --frames " & script.GetFrames)
            sb.Append(" --y4m")

            If Calc.IsARSignalingRequired AndAlso Not IsCustom("--sar") Then
                Dim par = Calc.GetTargetPAR
                sb.Append(" --sar " & par.X & ":" & par.Y)
            End If

            If Mode.Value = x265RateMode.TwoPass OrElse Mode.Value = x265RateMode.ThreePass Then
                sb.Append(" --stats """ + p.TempDir + p.Name + ".stats""")
            End If

            If (Mode.Value = x265RateMode.ThreePass AndAlso pass < 3) OrElse
                Mode.Value = x265RateMode.TwoPass AndAlso pass = 1 Then

                sb.Append(" --output NUL -")
            Else
                sb.Append(" --output """ + targetPath + """ - ")
            End If
        End If

        Return Macro.Solve(sb.ToString.Trim.FixBreak.Replace(CrLf, " "))
    End Function

    Function IsCustom(switch As String) As Boolean
        If switch = "" OrElse Custom.Value = "" Then Return False
        If Custom.Value.Contains(switch + " ") Then Return True
        If Custom.Value.EndsWith(switch) Then Return True
    End Function

    Function GetDeblockArgs() As String
        If Deblock.Value Then
            If DeblockA.Value = DeblockA.DefaultValue AndAlso
                DeblockB.Value = DeblockB.DefaultValue AndAlso
                Deblock.DefaultValue Then

                Return ""
            Else
                Return "--deblock " & DeblockA.Value & ":" & DeblockB.Value
            End If
        ElseIf Deblock.DefaultValue Then
            Return "--no-deblock"
        End If
    End Function

    Sub ApplyPresetValues()
        AQmode.Value = 1
        AQStrength.Value = 1
        DeblockA.Value = 0
        DeblockB.Value = 0
        FrameThreads.Value = 0
        IPRatio.Value = 1.4
        MaxCuSize.Value = 0
        MinCuSize.Value = 3
        PBRatio.Value = 1.3
        PsyRD.Value = 2.0
        PsyRDOQ.Value = 1
        QComp.Value = 0.6
        LookaheadSlices.Value = 8

        Select Case Preset.Value
            Case 0 'ultrafast
                [Me].Value = 0
                AMP.Value = False
                AQmode.Value = 0
                AQStrength.Value = 0
                BAdapt.Value = 0
                BFrames.Value = 3
                BIntra.Value = False
                CUtree.Value = False
                Deblock.Value = True
                EarlySkip.Value = True
                FastIntra.Value = True
                LimitRefs.Value = 0
                MaxCuSize.Value = 1
                MaxMerge.Value = 2
                MErange.Value = 25
                MinCuSize.Value = 2
                RCLookahead.Value = 5
                RD.Value = 2
                rdoqLevel.Value = 0
                Rect.Value = False
                Ref.Value = 1
                SAO.Value = False
                Scenecut.Value = 0
                SignHide.Value = False
                SubME.Value = 0
                TUinter.Value = 1
                TUintra.Value = 1
                Weightb.Value = False
                Weightp.Value = False
            Case 1 'superfast
                [Me].Value = 1
                AMP.Value = False
                AQmode.Value = 0
                AQStrength.Value = 0
                BAdapt.Value = 0
                BFrames.Value = 3
                BIntra.Value = False
                CUtree.Value = False
                Deblock.Value = True
                EarlySkip.Value = True
                FastIntra.Value = True
                LimitRefs.Value = 0
                MaxCuSize.Value = 1
                MaxMerge.Value = 2
                MErange.Value = 44
                RCLookahead.Value = 10
                RD.Value = 2
                rdoqLevel.Value = 0
                Rect.Value = False
                Ref.Value = 1
                SAO.Value = False
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 1
                TUinter.Value = 1
                TUintra.Value = 1
                Weightb.Value = False
                Weightp.Value = False
            Case 2 'veryfast
                [Me].Value = 1
                AMP.Value = False
                BAdapt.Value = 0
                BFrames.Value = 4
                BIntra.Value = False
                CUtree.Value = False
                Deblock.Value = True
                EarlySkip.Value = True
                FastIntra.Value = True
                MaxCuSize.Value = 1
                MaxMerge.Value = 2
                MErange.Value = 57
                RCLookahead.Value = 15
                RD.Value = 2
                rdoqLevel.Value = 0
                Rect.Value = False
                Ref.Value = 2
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 1
                TUinter.Value = 1
                TUintra.Value = 1
                Weightb.Value = False
                Weightp.Value = True
            Case 3 'faster
                [Me].Value = 1
                AMP.Value = False
                BAdapt.Value = 0
                BFrames.Value = 4
                BIntra.Value = False
                CUtree.Value = False
                Deblock.Value = True
                EarlySkip.Value = True
                FastIntra.Value = True
                MaxMerge.Value = 2
                MErange.Value = 57
                RCLookahead.Value = 15
                RD.Value = 2
                rdoqLevel.Value = 0
                Rect.Value = False
                Ref.Value = 2
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 2
                TUinter.Value = 1
                TUintra.Value = 1
                Weightb.Value = False
                Weightp.Value = True
            Case 4 'fast
                [Me].Value = 1
                AMP.Value = False
                BAdapt.Value = 2
                BFrames.Value = 4
                BIntra.Value = False
                CUtree.Value = True
                Deblock.Value = True
                EarlySkip.Value = False
                FastIntra.Value = True
                MaxMerge.Value = 2
                MErange.Value = 57
                RCLookahead.Value = 15
                RD.Value = 2
                rdoqLevel.Value = 0
                Rect.Value = False
                Ref.Value = 3
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 2
                TUinter.Value = 1
                TUintra.Value = 1
                Weightb.Value = False
                Weightp.Value = True
            Case 5 'medium
                [Me].Value = 1
                AMP.Value = False
                BAdapt.Value = 2
                BFrames.Value = 4
                BIntra.Value = False
                CUtree.Value = True
                Deblock.Value = True
                EarlySkip.Value = False
                FastIntra.Value = False
                MaxMerge.Value = 2
                MErange.Value = 57
                RCLookahead.Value = 20
                RD.Value = 3
                rdoqLevel.Value = 0
                Rect.Value = False
                Ref.Value = 3
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 2
                TUinter.Value = 1
                TUintra.Value = 1
                Weightb.Value = False
                Weightp.Value = True
            Case 6 'slow
                [Me].Value = 3
                AMP.Value = False
                BAdapt.Value = 2
                BFrames.Value = 4
                BIntra.Value = False
                CUtree.Value = True
                Deblock.Value = True
                EarlySkip.Value = False
                FastIntra.Value = False
                LimitModes.Value = True
                LookaheadSlices.Value = 4
                MaxMerge.Value = 3
                MErange.Value = 57
                RCLookahead.Value = 25
                RD.Value = 4
                rdoqLevel.Value = 2
                Rect.Value = True
                Ref.Value = 4
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 3
                TUinter.Value = 1
                TUintra.Value = 1
                Weightb.Value = False
                Weightp.Value = True
            Case 7 'slower
                [Me].Value = 3
                AMP.Value = True
                BAdapt.Value = 2
                BFrames.Value = 8
                BIntra.Value = True
                CUtree.Value = True
                Deblock.Value = True
                EarlySkip.Value = False
                FastIntra.Value = False
                LimitModes.Value = True
                LimitRefs.Value = 2
                LookaheadSlices.Value = 4
                MaxMerge.Value = 3
                MErange.Value = 57
                RCLookahead.Value = 30
                RD.Value = 6
                rdoqLevel.Value = 2
                Rect.Value = True
                Ref.Value = 4
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 3
                TUinter.Value = 2
                TUintra.Value = 2
                Weightb.Value = True
                Weightp.Value = True
            Case 8 'veryslow
                [Me].Value = 3
                AMP.Value = True
                BAdapt.Value = 2
                BFrames.Value = 8
                BIntra.Value = True
                CUtree.Value = True
                Deblock.Value = True
                EarlySkip.Value = False
                FastIntra.Value = False
                LimitModes.Value = True
                MaxMerge.Value = 4
                MErange.Value = 57
                RCLookahead.Value = 40
                RD.Value = 6
                Rect.Value = True
                Ref.Value = 5
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 4
                TUinter.Value = 3
                TUintra.Value = 3
                Weightb.Value = True
                Weightp.Value = True
                rdoqLevel.Value = 2
                LookaheadSlices.Value = 4
            Case 9 'placebo
                [Me].Value = 3
                AMP.Value = True
                BAdapt.Value = 2
                BFrames.Value = 8
                BIntra.Value = True
                CUtree.Value = True
                Deblock.Value = True
                EarlySkip.Value = False
                FastIntra.Value = False
                LimitRefs.Value = 0
                LookaheadSlices.Value = 0
                MaxMerge.Value = 5
                MErange.Value = 92
                RCLookahead.Value = 60
                RD.Value = 6
                rdoqLevel.Value = 2
                Rect.Value = True
                Ref.Value = 5
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 5
                TUinter.Value = 4
                TUintra.Value = 4
                Weightb.Value = True
                Weightp.Value = True
        End Select
    End Sub

    Sub ApplyPresetDefaultValues()
        AQmode.DefaultValue = 1
        AQStrength.DefaultValue = 1
        DeblockA.DefaultValue = 0
        DeblockB.DefaultValue = 0
        FrameThreads.DefaultValue = 0
        IPRatio.DefaultValue = 1.4
        MaxCuSize.DefaultValue = 0
        MinCuSize.DefaultValue = 3
        PBRatio.DefaultValue = 1.3
        PsyRD.DefaultValue = 2.0
        PsyRDOQ.DefaultValue = 1
        QComp.DefaultValue = 0.6
        LookaheadSlices.DefaultValue = 8

        Select Case Preset.Value
            Case 0 'ultrafast
                [Me].DefaultValue = 0
                AMP.DefaultValue = False
                AQmode.DefaultValue = 0
                AQStrength.DefaultValue = 0
                BAdapt.DefaultValue = 0
                BFrames.DefaultValue = 3
                BIntra.DefaultValue = False
                CUtree.DefaultValue = False
                Deblock.DefaultValue = True
                EarlySkip.DefaultValue = True
                FastIntra.DefaultValue = True
                LimitRefs.DefaultValue = 0
                MaxCuSize.DefaultValue = 1
                MaxMerge.DefaultValue = 2
                MErange.DefaultValue = 25
                MinCuSize.DefaultValue = 2
                RCLookahead.DefaultValue = 5
                RD.DefaultValue = 2
                rdoqLevel.DefaultValue = 0
                Rect.DefaultValue = False
                Ref.DefaultValue = 1
                SAO.DefaultValue = False
                Scenecut.DefaultValue = 0
                SignHide.DefaultValue = False
                SubME.DefaultValue = 0
                TUinter.DefaultValue = 1
                TUintra.DefaultValue = 1
                Weightb.DefaultValue = False
                Weightp.DefaultValue = False
            Case 1 'superfast
                [Me].DefaultValue = 1
                AMP.DefaultValue = False
                AQmode.DefaultValue = 0
                AQStrength.DefaultValue = 0
                BAdapt.DefaultValue = 0
                BFrames.DefaultValue = 3
                BIntra.DefaultValue = False
                CUtree.DefaultValue = False
                Deblock.DefaultValue = True
                EarlySkip.DefaultValue = True
                FastIntra.DefaultValue = True
                LimitRefs.DefaultValue = 0
                MaxCuSize.DefaultValue = 1
                MaxMerge.DefaultValue = 2
                MErange.DefaultValue = 44
                RCLookahead.DefaultValue = 10
                RD.DefaultValue = 2
                rdoqLevel.DefaultValue = 0
                Rect.DefaultValue = False
                Ref.DefaultValue = 1
                SAO.DefaultValue = False
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 1
                TUinter.DefaultValue = 1
                TUintra.DefaultValue = 1
                Weightb.DefaultValue = False
                Weightp.DefaultValue = False
            Case 2 'veryfast
                [Me].DefaultValue = 1
                AMP.DefaultValue = False
                BAdapt.DefaultValue = 0
                BFrames.DefaultValue = 4
                BIntra.DefaultValue = False
                CUtree.DefaultValue = False
                Deblock.DefaultValue = True
                EarlySkip.DefaultValue = True
                FastIntra.DefaultValue = True
                MaxCuSize.DefaultValue = 1
                MaxMerge.DefaultValue = 2
                MErange.DefaultValue = 57
                RCLookahead.DefaultValue = 15
                RD.DefaultValue = 2
                rdoqLevel.DefaultValue = 0
                Rect.DefaultValue = False
                Ref.DefaultValue = 2
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 1
                TUinter.DefaultValue = 1
                TUintra.DefaultValue = 1
                Weightb.DefaultValue = False
                Weightp.DefaultValue = True
            Case 3 'faster
                [Me].DefaultValue = 1
                AMP.DefaultValue = False
                BAdapt.DefaultValue = 0
                BFrames.DefaultValue = 4
                BIntra.DefaultValue = False
                CUtree.DefaultValue = False
                Deblock.DefaultValue = True
                EarlySkip.DefaultValue = True
                FastIntra.DefaultValue = True
                MaxMerge.DefaultValue = 2
                MErange.DefaultValue = 57
                RCLookahead.DefaultValue = 15
                RD.DefaultValue = 2
                rdoqLevel.DefaultValue = 0
                Rect.DefaultValue = False
                Ref.DefaultValue = 2
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 2
                TUinter.DefaultValue = 1
                TUintra.DefaultValue = 1
                Weightb.DefaultValue = False
                Weightp.DefaultValue = True
            Case 4 'fast
                [Me].DefaultValue = 1
                AMP.DefaultValue = False
                BAdapt.DefaultValue = 2
                BFrames.DefaultValue = 4
                BIntra.DefaultValue = False
                CUtree.DefaultValue = True
                Deblock.DefaultValue = True
                EarlySkip.DefaultValue = False
                FastIntra.DefaultValue = True
                MaxMerge.DefaultValue = 2
                MErange.DefaultValue = 57
                RCLookahead.DefaultValue = 15
                RD.DefaultValue = 2
                rdoqLevel.DefaultValue = 0
                Rect.DefaultValue = False
                Ref.DefaultValue = 3
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 2
                TUinter.DefaultValue = 1
                TUintra.DefaultValue = 1
                Weightb.DefaultValue = False
                Weightp.DefaultValue = True
            Case 5 'medium
                [Me].DefaultValue = 1
                AMP.DefaultValue = False
                BAdapt.DefaultValue = 2
                BFrames.DefaultValue = 4
                BIntra.DefaultValue = False
                CUtree.DefaultValue = True
                Deblock.DefaultValue = True
                EarlySkip.DefaultValue = False
                FastIntra.DefaultValue = False
                MaxMerge.DefaultValue = 2
                MErange.DefaultValue = 57
                RCLookahead.DefaultValue = 20
                RD.DefaultValue = 3
                rdoqLevel.DefaultValue = 0
                Rect.DefaultValue = False
                Ref.DefaultValue = 3
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 2
                TUinter.DefaultValue = 1
                TUintra.DefaultValue = 1
                Weightb.DefaultValue = False
                Weightp.DefaultValue = True
            Case 6 'slow
                [Me].DefaultValue = 3
                AMP.DefaultValue = False
                BAdapt.DefaultValue = 2
                BFrames.DefaultValue = 4
                BIntra.DefaultValue = False
                CUtree.DefaultValue = True
                Deblock.DefaultValue = True
                EarlySkip.DefaultValue = False
                FastIntra.DefaultValue = False
                LimitModes.DefaultValue = True
                LookaheadSlices.DefaultValue = 4
                MaxMerge.DefaultValue = 3
                MErange.DefaultValue = 57
                RCLookahead.DefaultValue = 25
                RD.DefaultValue = 4
                rdoqLevel.DefaultValue = 2
                Rect.DefaultValue = True
                Ref.DefaultValue = 4
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 3
                TUinter.DefaultValue = 1
                TUintra.DefaultValue = 1
                Weightb.DefaultValue = False
                Weightp.DefaultValue = True
            Case 7 'slower
                [Me].DefaultValue = 3
                AMP.DefaultValue = True
                BAdapt.DefaultValue = 2
                BFrames.DefaultValue = 8
                BIntra.DefaultValue = True
                CUtree.DefaultValue = True
                Deblock.DefaultValue = True
                EarlySkip.DefaultValue = False
                FastIntra.DefaultValue = False
                LimitModes.DefaultValue = True
                LimitRefs.DefaultValue = 2
                LookaheadSlices.DefaultValue = 4
                MaxMerge.DefaultValue = 3
                MErange.DefaultValue = 57
                RCLookahead.DefaultValue = 30
                RD.DefaultValue = 6
                rdoqLevel.DefaultValue = 2
                Rect.DefaultValue = True
                Ref.DefaultValue = 4
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 3
                TUinter.DefaultValue = 2
                TUintra.DefaultValue = 2
                Weightb.DefaultValue = True
                Weightp.DefaultValue = True
            Case 8 'veryslow
                [Me].DefaultValue = 3
                AMP.DefaultValue = True
                BAdapt.DefaultValue = 2
                BFrames.DefaultValue = 8
                BIntra.DefaultValue = True
                CUtree.DefaultValue = True
                Deblock.DefaultValue = True
                EarlySkip.DefaultValue = False
                FastIntra.DefaultValue = False
                LimitModes.DefaultValue = True
                LookaheadSlices.DefaultValue = 4
                MaxMerge.DefaultValue = 4
                MErange.DefaultValue = 57
                RCLookahead.DefaultValue = 40
                RD.DefaultValue = 6
                rdoqLevel.DefaultValue = 2
                Rect.DefaultValue = True
                Ref.DefaultValue = 5
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 4
                TUinter.DefaultValue = 3
                TUintra.DefaultValue = 3
                Weightb.DefaultValue = True
                Weightp.DefaultValue = True
            Case 9 'placebo
                [Me].DefaultValue = 3
                AMP.DefaultValue = True
                BAdapt.DefaultValue = 2
                BFrames.DefaultValue = 8
                BIntra.DefaultValue = True
                CUtree.DefaultValue = True
                Deblock.DefaultValue = True
                EarlySkip.DefaultValue = False
                FastIntra.DefaultValue = False
                LimitRefs.DefaultValue = 0
                LookaheadSlices.DefaultValue = 0
                MaxMerge.DefaultValue = 5
                MErange.DefaultValue = 92
                RCLookahead.DefaultValue = 60
                RD.DefaultValue = 6
                rdoqLevel.DefaultValue = 2
                Rect.DefaultValue = True
                Ref.DefaultValue = 5
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 5
                TUinter.DefaultValue = 4
                TUintra.DefaultValue = 4
                Weightb.DefaultValue = True
                Weightp.DefaultValue = True
        End Select
    End Sub

    Sub ApplyTuneValues()
        Select Case Tune.Value
            Case 1 '"psnr"
                AQStrength.Value = 0.0
                PsyRD.Value = 0.0
                PsyRDOQ.Value = 0.0
            Case 2 '"ssim"
                AQmode.Value = 2
                PsyRD.Value = 0.0
                PsyRDOQ.Value = 0.0
            Case 3 'grain
                IPRatio.Value = 1.1
                PBRatio.Value = 1
                CUtree.Value = False
                AQmode.Value = 0
                qpstep.Value = 1
                rcGrain.Value = True
            Case 4 '"fastdecode"
                Deblock.Value = False
                SAO.Value = False
                Weightp.Value = False
                Weightb.Value = False
                BIntra.Value = False
            Case 5 '"zerolatency"
                BAdapt.Value = 0
                BFrames.Value = 0
                RCLookahead.Value = 0
                Scenecut.Value = 0
                CUtree.Value = False
                FrameThreads.Value = 1
        End Select
    End Sub

    Sub ApplyTuneDefaultValues()
        Select Case Tune.Value
            Case 1 '"psnr"
                AQStrength.DefaultValue = 0.0
                PsyRD.DefaultValue = 0.0
                PsyRDOQ.DefaultValue = 0.0
            Case 2 '"ssim"
                AQmode.DefaultValue = 2
                PsyRD.DefaultValue = 0.0
                PsyRDOQ.DefaultValue = 0.0
            Case 3 'grain
                IPRatio.DefaultValue = 1.1
                PBRatio.DefaultValue = 1
                CUtree.DefaultValue = False
                AQmode.DefaultValue = 0
                qpstep.DefaultValue = 1
                rcGrain.DefaultValue = True
            Case 4 '"fastdecode"
                Deblock.DefaultValue = False
                SAO.DefaultValue = False
                Weightp.DefaultValue = False
                Weightb.DefaultValue = False
                BIntra.DefaultValue = False
            Case 5 '"zerolatency"
                BAdapt.DefaultValue = 0
                BFrames.DefaultValue = 0
                RCLookahead.DefaultValue = 0
                Scenecut.DefaultValue = 0
                CUtree.DefaultValue = False
                FrameThreads.DefaultValue = 1
        End Select
    End Sub

    Public Overrides Function GetPackage() As Package
        Return Packs.x265
    End Function
End Class

Public Enum x265RateMode
    SingleBitrate
    SingleQuant
    SingleCRF
    TwoPass
    ThreePass
End Enum