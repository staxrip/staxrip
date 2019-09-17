Imports System.Text
Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class x265Enc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Sub New()
        Name = "x265"
        Params.ApplyPresetDefaultValues()
        Params.ApplyPresetValues()
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

    Overrides ReadOnly Property OutputExt As String
        Get
            Return "hevc"
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Encode("Video encoding", GetArgs(1, p.Script), s.ProcessPriority)

        If Params.Mode.Value = x265RateMode.TwoPass Then
            Encode("Video encoding second pass", GetArgs(2, p.Script), s.ProcessPriority)
        ElseIf Params.Mode.Value = x265RateMode.ThreePass Then
            Encode("Video encoding second pass", GetArgs(3, p.Script), s.ProcessPriority)
            Encode("Video encoding third pass", GetArgs(2, p.Script), s.ProcessPriority)
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(passName As String, commandLine As String, priority As ProcessPriorityClass)
        p.Script.Synchronize()

        Using proc As New Proc
            proc.Package = Package.x265
            proc.Header = passName
            proc.Encoding = Encoding.UTF8
            proc.Priority = priority
            proc.SkipString = "%] "
            proc.File = "cmd.exe"
            proc.Arguments = "/S /C """ + commandLine + """"
            proc.Start()
        End Using
    End Sub

    Overrides Sub RunCompCheck()
        If Not g.VerifyRequirements Then Exit Sub
        If Not g.IsValidSource Then Exit Sub

        Dim newParams As New x265Params
        Dim newStore = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(newStore)

        Dim enc As New x265Enc
        enc.Params = newParams
        enc.Params.Mode.Value = x265RateMode.SingleCRF
        enc.Params.Quant.Value = enc.Params.CompCheck.Value

        Dim script As New VideoScript
        script.Engine = p.Script.Engine
        script.Filters = p.Script.GetFiltersCopy
        Dim code As String
        Dim every = ((100 \ p.CompCheckRange) * 14).ToString

        If script.Engine = ScriptEngine.AviSynth Then
            code = "SelectRangeEvery(" + every + ",14)"
        Else
            code = "fpsnum = clip.fps_num" + BR + "fpsden = clip.fps_den" + BR +
                "clip = core.std.SelectEvery(clip = clip, cycle = " + every + ", offsets = range(14))" + BR +
                "clip = core.std.AssumeFPS(clip = clip, fpsnum = fpsnum, fpsden = fpsden)"
        End If

        script.Filters.Add(New VideoFilter("aaa", "aaa", code))
        script.Path = p.TempDir + p.TargetFile.Base + "_CompCheck." + script.FileType
        script.Synchronize()

        Log.WriteLine(BR + script.GetFullScript + BR)

        Dim commandLine = enc.Params.GetArgs(0, script, p.TempDir + p.TargetFile.Base + "_CompCheck." + OutputExt, True, True)

        Try
            Encode("Compressibility Check", commandLine, ProcessPriorityClass.Normal)
        Catch ex As AbortException
            Exit Sub
        Catch ex As Exception
            g.ShowException(ex)
            Exit Sub
        End Try

        Dim bits = (New FileInfo(p.TempDir + p.TargetFile.Base + "_CompCheck." + OutputExt).Length) * 8
        p.Compressibility = (bits / script.GetFrames) / (p.TargetWidth * p.TargetHeight)

        OnAfterCompCheck()
        g.MainForm.Assistant()

        Log.WriteLine("Quality: " & CInt(Calc.GetPercent).ToString() + " %")
        Log.WriteLine("Compressibility: " + p.Compressibility.ToString("f3"))
        Log.Save()
    End Sub

    Overloads Function GetArgs(pass As Integer, script As VideoScript, Optional includePaths As Boolean = True) As String
        Return Params.GetArgs(pass, script, OutputPath.DirAndBase + OutputExtFull, includePaths, True)
    End Function

    Overrides Sub ShowConfigDialog()
        Dim newParams As New x265Params
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(store)
        newParams.ApplyPresetDefaultValues()
        newParams.ApplyTuneDefaultValues()

        Using f As New CommandLineForm(newParams)
            Dim saveProfileAction = Sub()
                                        Dim enc = ObjectHelp.GetCopy(Of x265Enc)(Me)
                                        Dim params2 As New x265Params
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            ActionMenuItem.Add(f.cms.Items, "Save Profile...", saveProfileAction).SetImage(Symbol.Save)

            If f.ShowDialog() = DialogResult.OK Then
                AutoCompCheckValue = CInt(newParams.CompCheckAimedQuality.Value)
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

    Property Decoder As New OptionParam With {
        .Text = "Decoder",
        .Options = {"AviSynth/VapourSynth", "QSVEnc (Intel)", "ffmpeg (Intel)", "ffmpeg (DXVA2)"},
        .Values = {"script", "qs", "ffqsv", "ffdxva"}}

    Property PipingTool As New OptionParam With {
        .Text = "Piping Tool",
        .Options = {"Automatic", "None", "vspipe", "avs2pipemod", "ffmpeg"},
        .Values = {"auto", "none", "vspipe", "avs2pipemod", "ffmpeg"}}

    Property Quant As New NumParam With {
        .Switches = {"--crf", "--qp"},
        .Name = "Quant",
        .Text = "Quality",
        .Value = 28,
        .Config = {0, 51, 1, 1}}

    Property chunkstart As New NumParam With {
        .Switch = "--chunk-start",
        .Name = "Chunk Start",
        .Text = "Chunk Start",
        .Init = 0}

    Property chunkend As New NumParam With {
        .Switch = "--chunk-end",
        .Name = "Chunk End",
        .Text = "Chunk End",
        .Init = 0}

    Property Preset As New OptionParam With {
        .Switch = "--preset",
        .Text = "Preset",
        .Options = {"Ultra Fast", "Super Fast", "Very Fast", "Faster", "Fast", "Medium", "Slow", "Slower", "Very Slow", "Placebo"},
        .InitValue = 5}

    Property Tune As New OptionParam With {
        .Switch = "--tune",
        .Text = "Tune",
        .Options = {"None", "PSNR", "SSIM", "Grain", "Fast Decode", "Zero Latency", "Animation"}}

    Property Mode As New OptionParam With {
        .Name = "Mode",
        .Text = "Mode",
        .Switches = {"--bitrate", "--qp", "--crf", "--pass"},
        .Options = {"Bitrate", "Quantizer", "Quality", "Two Pass", "Three Pass"},
        .Value = 3}

    Property SSIM As New BoolParam With {
        .Switch = "--ssim",
        .Text = "SSIM"}

    Property PSNR As New BoolParam With {
        .Switch = "--psnr",
        .Text = "PSNR"}

    Property BFrames As New NumParam With {
        .Switch = "--bframes",
        .Text = "B-Frames",
        .Config = {0, 16}}

    Property BFrameBias As New NumParam With {
        .Switch = "--bframe-bias",
        .Text = "B-Frame Bias",
        .Config = {-90, 100}}

    Property BAdapt As New OptionParam With {
        .Switch = "--b-adapt",
        .Text = "B-Adapt",
        .IntegerValue = True,
        .Options = {"None", "Fast", "Full"}}

    Property RCLookahead As New NumParam With {
        .Switch = "--rc-lookahead",
        .Text = "RC Lookahead",
        .Config = {0, 250, 5}}

    Property LookaheadSlices As New NumParam With {
        .Switch = "--lookahead-slices",
        .Text = "Lookahead Slices",
        .Config = {0, 16}}

    Property Scenecut As New NumParam With {
        .Switch = "--scenecut",
        .Text = "Scenecut",
        .Config = {0, 900, 10}}

    Property Ref As New NumParam With {
        .Switch = "--ref",
        .Text = "Ref Frames",
        .Config = {1, 16}}

    Property slowpass As New BoolParam With {
        .Switch = "--slow-firstpass",
        .NoSwitch = "--no-slow-firstpass",
        .Init = True,
        .Text = "Slow Firstpass"}

    Property [Me] As New OptionParam With {
        .Switch = "--me",
        .Text = "Motion Search Method",
        .Options = {"Diamond", "Hexagon", "Uneven Multi-Hexegon", "Star", "SEA", "Full"},
        .Values = {"dia", "hex", "umh", "star", "sea", "full"}}

    Property MErange As New NumParam With {
        .Switch = "--merange",
        .Text = "ME Range",
        .Config = {0, Integer.MaxValue},
        .ArgsFunc = Function() If(MErange.Value = 0, "--merange " & CInt(Calc.GetYFromTwoPointForm(480, 16, 2160, 57, p.TargetHeight)), If(MErange.Value <> MErange.DefaultValue, "--merange " & CInt(MErange.Value), Nothing))}

    Property SubME As New OptionParam With {
        .Switch = "--subme",
        .Text = "Subpel Refinement",
        .IntegerValue = True,
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
        .Text = "Maximum Merge",
        .Config = {1, 5}}

    Property SAOnonDeblock As New BoolParam With {
        .Switch = "--sao-non-deblock",
        .Text = "Specify how to handle depencency between SAO and deblocking filter"}

    Property SAO As New BoolParam With {
        .Switch = "--sao",
        .NoSwitch = "--no-sao",
        .Text = "Sample Adaptive Offset"}

    Property SignHide As New BoolParam With {
        .Switch = "--signhide",
        .NoSwitch = "--no-signhide",
        .Text = "Hide sign bit of one coeff per TU (rdo)"}

    Property CompCheck As New NumParam With {
        .Name = "CompCheckQuant",
        .Text = "Comp. Check",
        .Value = 18,
        .Help = "CRF value used as 100% for the compressibility check.",
        .Config = {1, 50}}

    Property CompCheckAimedQuality As New NumParam With {
        .Name = "CompCheckAimedQuality",
        .Text = "Aimed Quality",
        .Value = 50,
        .Help = "Percent value to adjusts the target file size or image size after the compressibility check accordingly.",
        .Config = {1, 100}}

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
        .Text = "AQ Mode",
        .IntegerValue = True,
        .Options = {"Disabled", "AQ", "AQ Auto-variance", "AQ Auto-variance with bias to dark scenes", "AQ Auto-variance and edge information"}}

    Property AQStrength As New NumParam With {
        .Switch = "--aq-strength",
        .Text = "AQ Strength",
        .Config = {0, 3, 0.05, 2}}

    Property CUtree As New BoolParam With {
        .Switch = "--cutree",
        .NoSwitch = "--no-cutree",
        .Text = "CU Tree",
        .Value = True}

    Property RD As New OptionParam With {
        .Switch = "--rd",
        .Text = "RD",
        .IntegerValue = True,
        .Expand = True,
        .Options = {"0 - SA8D mode and split decisions, intra w/ source pixels",
                    "1 - Recon generated (better intra), RDO merge/skip selection",
                    "2 - RDO splits and merge/skip selection",
                    "3 - RDO mode and split decisions, chroma residual used for sa8d",
                    "4 - Currently same as 3",
                    "5 - Adds RDO prediction decisions",
                    "6 - Currently same as 5"}}

    Property MinCuSize As New OptionParam With {
        .Switch = "--min-cu-size",
        .Text = "Min CU size",
        .Options = {"32", "16", "8"}}

    Property refinectudistortion As New OptionParam With {
        .Switch = "--refine-ctu-distortion",
        .Text = "Min CU size",
        .InitValue = 0,
        .Options = {"0", "1"}}

    Property MaxCuSize As New OptionParam With {
        .Switch = "--ctu",
        .Text = "Max CU size",
        .Options = {"64", "32", "16"}}

    Property qgSize As New OptionParam With {
        .Switch = "--qg-size",
        .Text = "QG Size",
        .Options = {"64", "32", "16", "8"},
        .InitValue = 1}

    Property qpstep As New NumParam With {
        .Switch = "--qpstep",
        .Text = "QP Step",
        .Init = 4}

    Property qpmin As New NumParam With {
        .Switch = "--qpmin",
        .Text = "QP Minimum"}

    Property qpmax As New NumParam With {
        .Switch = "--qpmax",
        .Text = "QP Maximum",
        .Init = 69}

    Property TUintra As New NumParam With {
        .Switch = "--tu-intra-depth",
        .Text = "TU Intra Depth",
        .Config = {1, 4}}

    Property TUinter As New NumParam With {
        .Switch = "--tu-inter-depth",
        .Text = "TU Inter Depth",
        .Config = {1, 4}}

    Property rdoqLevel As New NumParam With {
        .Switch = "--rdoq-level",
        .Switches = {"--rdoq"},
        .Text = "RDOQ Level",
        .Config = {0, 2}}

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

    Property TskipFast As New BoolParam With {
        .Switch = "--tskip-fast",
        .Text = "Only evaluate transform skip for NxN intra predictions (4x4 blocks)"}

    Property PsyRD As New NumParam With {
        .Switch = "--psy-rd",
        .Text = "Psy RD",
        .Config = {0, 5, 0.05, 2}}

    Property PsyRDOQ As New NumParam With {
        .Switch = "--psy-rdoq",
        .Text = "Psy RDOQ",
        .Config = {0, 50, 0.05, 2}}

    Property qpadaptationrange As New NumParam With {
        .Switch = "--qp-adaptation-range",
        .Text = "QP Adaptation Range",
        .Init = 1.0,
        .Config = {1, 6, 0.05, 2}}

    Property CRFmax As New NumParam With {
        .Switch = "--crf-max",
        .Text = "Maximum CRF",
        .Config = {0, 51, 1, 1},
        .Init = 51}

    Property CRFmin As New NumParam With {
        .Switch = "--crf-min",
        .Text = "Minimum CRF",
        .Config = {0, 51, 1, 1}}

    Property PBRatio As New NumParam With {
        .Switch = "--pbratio",
        .Switches = {"--pb-factor"},
        .Text = "PB Ratio",
        .Config = {0, 1000, 0.05, 2}}

    Property IPRatio As New NumParam With {
        .Switch = "--ipratio",
        .Switches = {"--ip-factor"},
        .Text = "IP Ratio",
        .Config = {0, 1000, 0.05, 2}}

    Property QComp As New NumParam With {
        .Switch = "--qcomp",
        .Text = "QComp",
        .Config = {0, 1000, 0.05, 2}}

    Property QBlur As New NumParam With {
        .Switch = "--qblur",
        .Text = "Q Blur",
        .Config = {0, 0, 0.05, 2},
        .Init = 0.5}

    Property Cplxblur As New NumParam With {
        .Switch = "--cplxblur",
        .Text = "Blur Complexity",
        .Config = {0, 0, 0.05, 2},
        .Init = 20}

    Property WPP As New BoolParam With {
        .Switch = "--wpp",
        .NoSwitch = "--no-wpp",
        .Text = "Wavefront Parallel Processing",
        .Init = True}

    Property Pmode As New BoolParam With {
        .Switch = "--pmode",
        .NoSwitch = "--no-pmode",
        .Text = "Parallel Mode Decision"}

    Property PME As New BoolParam With {
        .Switch = "--pme",
        .NoSwitch = "--no-pme",
        .Text = "Parallel Motion Estimation"}

    Property minLuma As New NumParam With {
        .Switch = "--min-luma",
        .Text = "Minimum Luma"}

    Property maxLuma As New NumParam With {
        .Switch = "--max-luma",
        .Text = "Maximum Luma"}

    Property OutputDepth As New OptionParam With {
        .Switch = "--output-depth",
        .Text = "Depth",
        .Options = {"8-Bit", "10-Bit", "12-Bit"},
        .Values = {"8", "10", "12"},
        .Value = 1}

    Property Hash As New OptionParam With {
        .Switch = "--hash",
        .Text = "Hash",
        .IntegerValue = True,
        .Options = {"None", "MD5", "CRC", "Checksum"}}

    Property TemporalMVP As New BoolParam With {
        .Switch = "--temporal-mvp",
        .NoSwitch = "--no-temporal-mvp",
        .Text = "Enable temporal motion vector predictors in P and B slices",
        .Init = True}

    Property StrongIntraSmoothing As New BoolParam With {
        .Switch = "--strong-intra-smoothing",
        .NoSwitch = "--no-strong-intra-smoothing",
        .Text = "Enable strong intra smoothing for 32x32 intra blocks",
        .Init = True}

    Property RDpenalty As New NumParam With {
        .Switch = "--rdpenalty",
        .Text = "RD Penalty",
        .Config = {0, 2}}

    Property OpenGop As New BoolParam With {
        .Switch = "--open-gop",
        .NoSwitch = "--no-open-gop",
        .Text = "Open GOP",
        .Init = True}

    Property Bpyramid As New BoolParam With {
        .Switch = "--b-pyramid",
        .NoSwitch = "--no-b-pyramid",
        .Text = "B-Pyramid",
        .Init = True}

    Property Lossless As New BoolParam With {
        .Switch = "--lossless",
        .Text = "Lossless"}

    Property StrictCBR As New BoolParam With {
        .Switch = "--strict-cbr",
        .Text = "Strict CBR"}

    Property NRintra As New NumParam With {
        .Switch = "--nr-intra",
        .Text = "Intra Noise Reduct.",
        .Config = {0, 2000, 50}}

    Property NRinter As New NumParam With {
        .Switch = "--nr-inter",
        .Text = "Inter Noise Reduct.",
        .Config = {0, 2000, 50}}

    Property Keyint As New NumParam With {
        .Switch = "--keyint",
        .Text = "Max GOP Size",
        .Config = {0, 10000, 10},
        .Init = 250}

    Property MinKeyint As New NumParam With {
        .Switch = "--min-keyint",
        .Text = "Min GOP Size",
        .Config = {0, 10000, 10}}

    Property VBVbufsize As New NumParam With {
        .Switch = "--vbv-bufsize",
        .Text = "VBV Bufsize",
        .Config = {0, 1000000, 100}}

    Property VBVmaxrate As New NumParam With {
        .Switch = "--vbv-maxrate",
        .Text = "VBV Maxrate",
        .Config = {0, 1000000, 100}}

    Property VBVinit As New NumParam With {
        .Switch = "--vbv-init",
        .Text = "VBV Init",
        .Config = {0.5, 1.0, 0.1, 1},
        .Init = 0.9}

    Property VBVend As New NumParam With {
        .Switch = "--vbv-end",
        .Text = "VBV End",
        .Config = {0, 1.0, 0.1, 1},
        .Init = 0.0}

    Property maxausizefactor As New NumParam With {
        .Switch = "--max-ausize-factor",
        .Text = "Max Ausize Factor",
        .Config = {0.5, 1.0, 0.1, 1},
        .Init = 1}

    Property VBVfradj As New NumParam With {
        .Switch = "--vbv-end-fr-adj",
        .Text = "VBV Adjust",
        .Config = {0, 1, 0.1, 1},
        .Init = 0.0}

    Property Chromaloc As New NumParam With {
        .Switch = "--chromaloc",
        .Text = "Chromaloc",
        .Config = {0, 5}}

    Property FrameThreads As New NumParam With {
        .Switch = "--frame-threads",
        .Text = "Frame Threads"}

    Property RepeatHeaders As New BoolParam With {
        .Switch = "--repeat-headers",
        .Text = "Repeat Headers"}

    Property Info As New BoolParam With {
        .Switch = "--info",
        .NoSwitch = "--no-info",
        .Text = "Info",
        .Init = True}

    Property HRD As New BoolParam With {
        .Switch = "--hrd",
        .Text = "HRD"}

    Property AUD As New BoolParam With {
        .Switch = "--aud",
        .Text = "AUD"}

    Property LimitModes As New BoolParam With {
        .Switch = "--limit-modes",
        .Text = "Limit Modes"}

    Property RdRefine As New BoolParam With {
        .Switch = "--rd-refine",
        .Text = "RD Refine"}

    Property MaxCLL As New NumParam With {
        .Text = "Maximum CLL",
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
        .Text = "Maximum FALL"}

    Property IntraRefresh As New BoolParam With {
        .Switch = "--intra-refresh",
        .Text = "Intra Refresh"}

    Property Custom As New StringParam With {
        .Text = "Custom",
        .Quotes = QuotesMode.Never,
        .AlwaysOn = True,
        .InitAction = Sub(tb)
                          tb.Edit.Expand = True
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = New Font("Consolas", 10 * s.UIScaleFactor)
                      End Sub}

    Property CustomFirstPass As New StringParam With {
        .Text = "Custom" + BR + "First Pass",
        .Quotes = QuotesMode.Never,
        .InitAction = Sub(tb)
                          tb.Edit.Expand = True
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = New Font("Consolas", 10 * s.UIScaleFactor)
                      End Sub}

    Property CustomSecondPass As New StringParam With {
        .Text = "Custom" + BR + "Second Pass",
        .Quotes = QuotesMode.Never,
        .InitAction = Sub(tb)
                          tb.Edit.Expand = True
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = New Font("Consolas", 10 * s.UIScaleFactor)
                      End Sub}

    Property Deblock As New BoolParam With {
        .Switch = "--deblock",
        .Text = "Deblocking",
        .ArgsFunc = Function() As String
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
                    End Function,
        .ImportAction = Sub(arg As String)
                            Dim a = arg.Split(":"c)
                            DeblockA.Value = a(0).ToInt
                            DeblockB.Value = a(1).ToInt
                        End Sub}

    Property DeblockA As New NumParam With {
        .Text = "      Strength",
        .Config = {-6, 6}}

    Property DeblockB As New NumParam With {
        .Text = "      Threshold",
        .Config = {-6, 6}}

    Property MaxTuSize As New OptionParam With {
        .Switch = "--max-tu-size",
        .Text = "Max TU Size",
        .Options = {"32", "16", "8", "4"}}

    Property LimitRefs As New OptionParam With {
        .Switch = "--limit-refs",
        .Text = "Limit References",
        .Options = {"0", "1", "2", "3"},
        .InitValue = 3}

    Property RecursionSkip As New BoolParam With {
        .Switch = "--rskip",
        .NoSwitch = "--no-rskip",
        .Text = "Early CU depth recursion skip",
        .Init = True}

    Property csvloglevel As New OptionParam With {
        .Switch = "--csv-log-level",
        .Text = "CSV Log Level",
        .IntegerValue = True,
        .Options = {"Default", "Summary", "Frame"}}

    Property multi_pass_opt_analysis As New BoolParam() With {
        .Switch = "--multi-pass-opt-analysis",
        .Text = "Multipass analysis refinement along with multipass ratecontrol"}

    Property multi_pass_opt_distortion As New BoolParam() With {
        .Switch = "--multi-pass-opt-distortion",
        .Text = "Multipass refinement of qp based on distortion data"}

    Property LimitTU As New NumParam With {
        .Switch = "--limit-tu",
        .Text = "Limit TU",
        .Config = {0, 4}}

    Property constvbv As New BoolParam With {
        .Switch = "--const-vbv",
        .NoSwitch = "--no-const-vbv",
        .Text = "Enable VBV algorithm to be consistent across runs"}

    Property Frames As New NumParam With {
        .Switch = "--frames",
        .Text = "Frames"}

    Overrides ReadOnly Property Items As List(Of CommandLineParam)
        Get
            If ItemsValue Is Nothing Then
                ItemsValue = New List(Of CommandLineParam)

                Add("Basic", Preset, Tune,
                    New OptionParam With {.Switch = "--profile", .Text = "Profile", .Name = "ProfileMain8", .InitValue = 1, .VisibleFunc = Function() OutputDepth.Value = 0, .Options = {"Unrestricted", "Main", "Main - Intra", "Main Still Picture", "Main 444 - 8", "Main 444 - Intra", "Main 444 - Main Still Picture"}},
                    New OptionParam With {.Switch = "--profile", .Text = "Profile", .Name = "ProfileMain10", .InitValue = 1, .VisibleFunc = Function() OutputDepth.Value = 1, .Options = {"Unrestricted", "Main 10", "Main 10 - Intra", "Main 422 - 10", "Main 422 - 10 - Intra", "Main 444 - 10", "Main 444 - 10 - Intra"}},
                    New OptionParam With {.Switch = "--profile", .Text = "Profile", .Name = "ProfileMain12", .InitValue = 1, .VisibleFunc = Function() OutputDepth.Value = 2, .Options = {"Unrestricted", "Main 12", "Main 12 - Intra", "Main 422 - 12", "Main 422 - 12 - Intra", "Main 444 - 12", "Main 444 - 12 - Intra"}},
                    New OptionParam With {.Switch = "--level-idc", .Switches = {"--level"}, .Text = "Level", .Options = {"Unrestricted", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2", "8.5"}},
                    Mode, OutputDepth, Quant)
                Add("Analysis", RD,
                    New StringParam With {.Switch = "--analysis-reuse-file", .Text = "Analysis File", .BrowseFile = True},
                    New StringParam With {.Switch = "--analysis-load", .Text = "Analysis Load", .BrowseFile = True},
                    New StringParam With {.Switch = "--analysis-save", .Text = "Analysis Save"},
                    New OptionParam With {.Switch = "--refine-mv", .Text = "Refine MV", .Expand = True, .IntegerValue = True, .Options = {"Disabled", "Level 1: Search around scaled MV", "Level 2: Level 1 + Search around best AMVP cand", "Level 3: Level 2 + Search around the other AMVP cand"}},
                    refinectudistortion, MinCuSize, MaxCuSize, MaxTuSize, LimitRefs)
                Add("Analysis 2",
                    New NumParam With {.Switch = "--analysis-reuse-level", .Text = "Refine Level", .Config = {1, 10}, .Init = 5},
                    New NumParam With {.Switch = "--scale-factor", .Text = "Scale Factor"},
                    LimitTU,
                    TUintra, TUinter, rdoqLevel,
                    PsyRDOQ,
                    New NumParam With {.Switch = "--dynamic-rd", .Text = "Dynamic RD", .Config = {0, 4}},
                    New NumParam With {.Switch = "--refine-intra", .Text = "Refine Intra", .Config = {0, 4}},
                    New NumParam With {.Switch = "--refine-inter", .Text = "Refine Inter", .Config = {0, 3}},
                    qpadaptationrange)
                Add("Analysis 3", Rect, AMP,
                    New BoolParam With {.Switch = "--tskip", .Text = "Enable evaluation of transform skip coding for 4x4 TU coded blocks"},
                    New BoolParam With {.Switch = "--dynamic-refine", .Text = "Dynamic Refine"},
                    EarlySkip, FastIntra, BIntra, CUlossless, TskipFast, LimitModes, RdRefine,
                    New BoolParam With {.Switch = "--cu-stats", .Text = "CU Stats"},
                    RecursionSkip,
                    New BoolParam With {.Switch = "--ssim-rd", .Text = "SSIM RDO"},
                    New BoolParam With {.Switch = "--splitrd-skip", .Text = "Enable skipping split RD analysis"},
                    New BoolParam With {.Switch = "--hevc-aq", .Text = "Mode for HEVC Adaptive Quantization", .Init = False})
                Add("Rate Control",
                    New StringParam With {.Switch = "--zones", .Text = "Zones"},
                    New StringParam With {.Switch = "--zonefile", .Text = "Zone File", .BrowseFile = True},
                    AQmode, qgSize, AQStrength, QComp,
                    New NumParam With {.Switch = "--cbqpoffs", .Text = "CB QP Offset", .Config = {-12, 12}},
                    New NumParam With {.Switch = "--crqpoffs", .Text = "CR QP Offset", .Config = {-12, 12}},
                    NRintra, NRinter, qpmin, qpmax, qpstep, CRFmin, CRFmax)
                Add("Rate Control 2",
                    VBVbufsize, VBVmaxrate, VBVinit, VBVend, VBVfradj,
                    IPRatio, PBRatio, Cplxblur, QBlur,
                    CUtree, Lossless, StrictCBR, rcGrain,
                    multi_pass_opt_analysis,
                    multi_pass_opt_distortion,
                    New BoolParam() With {.Switch = "--aq-motion", .Text = "AQ Motion"},
                    constvbv)
                Add("Motion Search",
                    New StringParam With {.Switch = "--hme-search", .Text = "HME Search"},
                    SubME, [Me], MErange, MaxMerge, Weightp, Weightb, TemporalMVP,
                    New BoolParam With {.Switch = "--analyze-src-pics", .NoSwitch = "--no-analyze-src-pics", .Text = "Analyze SRC Pics"},
                    New BoolParam With {.Switch = "--hme", .NoSwitch = "--no-hme", .Text = "3-level Hierarchical motion estimation"})
                Add("Slice Decision",
                    New StringParam With {.Switch = "--refine-analysis-type", .Text = "Refine Analysis Type"},
                    New OptionParam() With {.Switch = "--force-flush", .Text = "Force Flush", .Expand = True, .IntegerValue = True, .Options = {"Flush the encoder only when all the input pictures are over", "Flush all the frames even when the input is not over", "Flush the slicetype decided frames only"}},
                    BAdapt,
                    New OptionParam With {.Switch = "--ctu-info", .Text = "CTU Info", .Options = {"0", "1", "2", "4", "6"}},
                    BFrames, BFrameBias,
                    RCLookahead,
                    LookaheadSlices,
                    New NumParam() With {.Switch = "--lookahead-threads", .Text = "Lookahead Threads"},
                    New NumParam() With {.Switch = "--gop-lookahead", .Text = "Gop Lookahead"},
                    Scenecut,
                    New NumParam() With {.Switch = "--scenecut-bias", .Text = "Scenecut Bias", .Init = 5, .Config = {0, 100, 1, 1}},
                    New NumParam() With {.Switch = "--radl", .Text = "Radl", .Init = 0},
                    Ref)
                Add("Slice Decision 2", MinKeyint, Keyint, Bpyramid, OpenGop, IntraRefresh,
                    New BoolParam() With {.Switch = "--fades", .Text = "Enable detection and handling of fade-in regions"})
                Add("Performance",
                    New StringParam With {.Switch = "--pools", .Switches = {"--numa-pools"}, .Text = "Pools"},
                    New NumParam With {.Switch = "--slices", .Text = "Slices", .Init = 1},
                    FrameThreads, WPP, Pmode, PME,
                    New BoolParam With {.Switch = "--asm", .NoSwitch = "--no-asm", .Text = "ASM", .Help = "For AVX512 Vector CPU's, Experiential Feature", .Init = True},
                    New BoolParam With {.Switch = "--asm avx512", .Text = "AVX 512"},
                    slowpass,
                    New BoolParam With {.Switch = "--copy-pic", .NoSwitch = "--no-copy-pic", .Init = True, .Text = "Copy Pic"})
                Add("Statistic",
                    New StringParam With {.Switch = "--csv", .Text = "CSV", .BrowseFile = True},
                    New OptionParam With {.Switch = "--log-level", .Switches = {"--log"}, .Text = "Log Level", .Options = {"None", "Error", "Warning", "Info", "Debug", "Full"}, .InitValue = 3},
                    csvloglevel, SSIM, PSNR)
                Add("VUI",
                    New StringParam With {.Switch = "--master-display", .Text = "Master Display"},
                    New StringParam With {.Switch = "--dhdr10-info", .Text = "DHDR10-Info", .BrowseFile = True},
                    New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim", .Options = {"Undefined", "BT 2020", "BT 470 BG", "BT 470 M", "BT 709", "Film", "SMPTE 170 M", "SMPTE 240 M", "SMPTE 428", "SMPTE 431", "SMPTE 432"}},
                    New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix", .Options = {"Undefined", "BT 2020 C", "BT 2020 NC", "BT 470 BG", "BT 709", "Chroma-Derived-C", "Chroma-Derived-NC", "FCC", "GBR", "ICTCP", "SMPTE 170 M", "SMPTE 2085", "SMPTE 240 M", "YCgCo"}},
                    New OptionParam With {.Switch = "--transfer", .Text = "Transfer", .Options = {"Undefined", "ARIB-STD-B67", "BT 1361 E", "BT 2020-10", "BT 2020-12", "BT 470 BG", "BT 470 M", "BT 709", "IEC 61966-2-1", "IEC 61966-2-4", "Linear", "Log 100", "Log 316", "SMPTE 170 M", "SMPTE 2084", "SMPTE 240 M", "SMPTE 428"}},
                    New OptionParam With {.Switch = "--range", .Text = "Range", .Options = {"Undefined", "Limited", "Full"}},
                    minLuma, maxLuma, MaxCLL, MaxFALL,
                    New BoolParam With {.Switch = "--hdr", .Text = "Force signalling of HDR parameters in SEI packets"},
                    New BoolParam With {.Switch = "--hdr-opt", .Text = "Add luma and chroma offsets for HDR/WCG content"},
                    New BoolParam With {.Switch = "--dhdr10-opt", .Text = "Limit frames for which tone mapping information is inserted as SEI message"},
                    New BoolParam With {.Switch = "--atc-sei", .Text = "Emit the alternative transfer characteristics SEI message"},
                    New BoolParam With {.Switch = "--cll", .NoSwitch = "--no-cll", .Text = "Emit content light level info SEI", .Init = True},
                    New BoolParam With {.Switch = "--pic-struct", .Text = "Set the picture structure and emits it in the picture timing SEI message"})
                Add("VUI 2",
                    New StringParam With {.Switch = "--nalu-file", .Text = "Nalu File", .BrowseFile = True},
                    New StringParam With {.Switch = "--sar", .Text = "Sample Aspect Ratio", .InitValue = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                    New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat", .Options = {"Undefined", "Component", "PAL", "NTSC", "SECAM", "MAC"}},
                    New OptionParam With {.Switch = "--overscan", .Text = "Overscan", .Options = {"Undefined", "Show", "Crop"}},
                    New OptionParam With {.Switch = "--display-window", .Text = "Display Window", .Options = {"Undefined", "Left", "Top", "Right", "Top"}},
                    Chromaloc)
                Add("Bitstream",
                    New OptionParam With {.Switch = "--dolby-vision-profile", .Text = "Dolby Vision Profile", .InitValue = 0, .Options = {"0", "5", "8.1", "8.2"}},
                    New StringParam With {.Switch = "--dolby-vision-rpu", .Text = "Dolby Vision RPU", .BrowseFile = True},
                    New NumParam With {.Switch = "--log2-max-poc-lsb", .Text = "Maximum Picture Order Count", .Init = 8},
                    RepeatHeaders, Info, HRD, AUD,
                    New BoolParam With {.Switch = "--hrd-concat", .Init = False, .Text = "HRD Concat"},
                    New BoolParam With {.Switch = "--vui-timing-info", .Text = "VUI Timing Info", .Init = True},
                    New BoolParam With {.Switch = "--vui-hrd-info", .Text = "VUI HRD Info", .Init = True},
                    New BoolParam With {.Switch = "--idr-recovery-sei", .Init = False, .Text = "Recovery SEI"},
                    New BoolParam With {.Switch = "--single-sei", .Init = False, .Text = "Single SEI"})
                Add("Bitstream 2",
                    Hash,
                    New BoolParam With {.Switch = "--temporal-layers", .Text = "Temporal Layers"},
                    New BoolParam With {.Switch = "--opt-qp-pps", .Init = False, .Text = "Optimize QP in PPS"},
                    New BoolParam With {.Switch = "--opt-ref-list-length-pps", .Init = False, .Text = "Optimize L0 and L1 Ref List Length in PPS"},
                    New BoolParam With {.Switch = "--multi-pass-opt-rps", .Init = False, .Text = "Enable Storing", .Help = "Enable Storing commonly used RPS in SPS in multi pass mode"},
                    New BoolParam With {.Switch = "--opt-cu-delta-qp", .Text = "Optimize CU level QPs", .Help = "Optimize CU level QPs pulling up lower QPs close to meanQP", .Init = False})
                Add("Input/Output",
                    New OptionParam With {.Switch = "--input-depth", .Text = "Input Depth", .Options = {"Automatic", "8", "10", "12", "14", "16"}},
                    New OptionParam With {.Switch = "--input-csp", .Text = "Input CSP", .Options = {"Automatic", "I400", "I420", "I422", "I444", "NV12", "NV16"}},
                    New OptionParam With {.Switch = "--interlace", .Text = "Interlace", .Options = {"Progressive", "Top Field First", "Bottom Field First"}, .Values = {"", "tff", "bff"}},
                    New OptionParam With {.Switch = "--fps", .Text = "Frame Rate", .Options = {"Automatic", "24000/1001", "24", "25", "30000/1001", "30", "50", "60000/1001", "60"}},
                    Frames, chunkstart, chunkend,
                    New NumParam With {.Switch = "--seek", .Text = "Seek"},
                    New BoolParam With {.Switch = "--dither", .Text = "Dither (High Quality Downscaling)"},
                    New BoolParam With {.Switch = "--field", .NoSwitch = "--no-field", .Text = "Field Coding"})
                Add("Loop Filter", Deblock, DeblockA, DeblockB, SAO,
                    New BoolParam With {.Switch = "--limit-sao", .Text = "Limit Sample Adaptive Offset"},
                    SAOnonDeblock)
                Add("Other",
                    New StringParam With {.Switch = "--lambda-file", .Text = "Lambda File", .BrowseFile = True},
                    New StringParam With {.Switch = "--qpfile", .Text = "QP File", .BrowseFile = True},
                    New StringParam With {.Switch = "--recon", .Text = "Recon File", .BrowseFile = True},
                    New StringParam With {.Switch = "--scaling-list", .Text = "Scaling List"},
                    Decoder, PipingTool, PsyRD, CompCheck, CompCheckAimedQuality,
                    New NumParam With {.Switch = "--recon-depth", .Text = "Recon Depth"},
                    RDpenalty, maxausizefactor)
                Add("Other 2",
                    New BoolParam With {.Switch = "--high-tier", .Text = "High Tier", .Name = "HighTierV2", .Init = True},
                    SignHide,
                    New BoolParam With {.Switch = "--allow-non-conformance", .Text = "Allow non conformance"},
                    New BoolParam With {.Switch = "--uhd-bd", .Text = "Ultra HD Blu-ray"},
                    StrongIntraSmoothing,
                    New BoolParam With {.Switch = "--constrained-intra", .NoSwitch = "--no-constrained-intra", .Switches = {"--cip"}, .Text = "Constrained Intra Prediction", .Init = False},
                    New BoolParam With {.Switch = "--lowpass-dct", .Text = "Lowpass DCT"})
                Add("Custom", Custom, CustomFirstPass, CustomSecondPass)

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
        g.ShowCommandLineHelp(Package.x265, id)
    End Sub

    Private BlockValueChanged As Boolean

    Protected Overrides Sub OnValueChanged(item As CommandLineParam)
        If BlockValueChanged Then Exit Sub

        If item Is Preset OrElse item Is Tune Then
            BlockValueChanged = True
            ApplyPresetValues()
            ApplyTuneValues()
            BlockValueChanged = False
        End If

        If Not DeblockA.NumEdit Is Nothing Then
            If Not DeblockA.NumEdit Is Nothing Then DeblockA.NumEdit.Enabled = Deblock.Value
            If Not DeblockB.NumEdit Is Nothing Then DeblockB.NumEdit.Enabled = Deblock.Value
        End If

        MyBase.OnValueChanged(item)
    End Sub

    Overloads Overrides Function GetCommandLine(includePaths As Boolean,
                                                includeExecutable As Boolean,
                                                Optional pass As Integer = 1) As String

        Return GetArgs(1, p.Script, p.VideoEncoder.OutputPath.DirAndBase +
                       p.VideoEncoder.OutputExtFull, includePaths, includeExecutable)
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
            Dim isCropped = CInt(p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 AndAlso
                Decoder.ValueText <> "avs" AndAlso p.Script.IsFilterActive("Crop")

            Select Case Decoder.ValueText
                Case "script"
                    Dim pipeString = ""
                    Dim pipeTool = PipingTool.ValueText

                    If pipeTool = "auto" Then
                        If p.Script.Engine = ScriptEngine.VapourSynth Then
                            pipeTool = "vspipe"
                        Else
                            pipeTool = "avs2pipemod"
                        End If
                    End If

                    Select Case pipeTool
                        Case "vspipe"
                            pipeString = Package.vspipe.Path.Escape + " " + script.Path.Escape + " - --y4m | "
                        Case "avs2pipemod"
                            pipeString = Package.avs2pipemod.Path.Escape + " -y4mp " + script.Path.Escape + " | "
                        Case "ffmpeg"
                            pipeString = Package.ffmpeg.Path.Escape + " -i " + script.Path.Escape + " -f yuv4mpegpipe -strict -1 -loglevel fatal -hide_banner - | "
                    End Select

                    sb.Append(pipeString + Package.x265.Path.Escape)
                Case "qs"
                    Dim crop = If(isCropped, " --crop " & p.CropLeft & "," & p.CropTop & "," & p.CropRight & "," & p.CropBottom, "")
                    sb.Append(Package.QSVEnc.Path.Escape + " -o - -c raw" + crop + " -i " + p.SourceFile.Escape + " | " + Package.x265.Path.Escape)
                Case "ffqsv"
                    Dim crop = If(isCropped, $" -vf ""crop={p.SourceWidth - p.CropLeft - p.CropRight}:{p.SourceHeight - p.CropTop - p.CropBottom}:{p.CropLeft}:{p.CropTop}""", "")
                    sb.Append(Package.ffmpeg.Path.Escape + " -threads 1 -hwaccel qsv -i " + p.SourceFile.Escape + " -f yuv4mpegpipe -strict -1" + crop + " -loglevel fatal -hide_banner - | " + Package.x265.Path.Escape)
                Case "ffdxva"
                    Dim crop = If(isCropped, $" -vf ""crop={p.SourceWidth - p.CropLeft - p.CropRight}:{p.SourceHeight - p.CropTop - p.CropBottom}:{p.CropLeft}:{p.CropTop}""", "")
                    sb.Append(Package.ffmpeg.Path.Escape + " -threads 1 -hwaccel dxva2 -i " + p.SourceFile.Escape + " -f yuv4mpegpipe -strict -1" + crop + " -loglevel fatal -hide_banner - | " + Package.x265.Path.Escape)
            End Select
        End If

        If Mode.Value = x265RateMode.TwoPass OrElse Mode.Value = x265RateMode.ThreePass Then
            sb.Append(" --pass " & pass)

            If pass = 1 Then
                If CustomFirstPass.Value <> "" Then sb.Append(" " + CustomFirstPass.Value)
            Else
                If CustomSecondPass.Value <> "" Then sb.Append(" " + CustomSecondPass.Value)
            End If

            If includePaths AndAlso (multi_pass_opt_distortion.Value OrElse multi_pass_opt_analysis.Value) Then
                sb.Append(" --analysis-reuse-file " + targetPath.ChangeExt("analysis").Escape)
            End If
        End If

        If Mode.Value = x265RateMode.SingleQuant Then
            If Not IsCustom(pass, "--qp") Then sb.Append(" --qp " + CInt(Quant.Value).ToString)
        ElseIf Mode.Value = x265RateMode.SingleCRF Then
            If Quant.Value <> 28 AndAlso Not IsCustom(pass, "--crf") Then
                sb.Append(" --crf " + Quant.Value.ToInvariantString)
            End If
        Else
            If Not IsCustom(pass, "--bitrate") Then sb.Append(" --bitrate " & p.VideoBitrate)
        End If

        Dim q = From i In Items Where i.GetArgs <> "" AndAlso Not IsCustom(pass, i.Switch)
        If q.Count > 0 Then sb.Append(" " + q.Select(Function(item) item.GetArgs).Join(" "))

        If includePaths Then
            If PipingTool.ValueText <> "none" Then
                If Frames.Value = 0 AndAlso Not IsCustom(pass, "--frames") Then
                    sb.Append(" --frames " & script.GetFrames)
                End If

                sb.Append(" --y4m")
            End If

            If Mode.Value = x265RateMode.TwoPass OrElse Mode.Value = x265RateMode.ThreePass Then
                sb.Append(" --stats " + (p.TempDir + p.TargetFile.Base + ".stats").Escape)
            End If

            Dim input = If(PipingTool.ValueText = "none", script.Path.Escape, "-")

            If (Mode.Value = x265RateMode.ThreePass AndAlso pass < 3) OrElse
                Mode.Value = x265RateMode.TwoPass AndAlso pass = 1 Then

                sb.Append(" --output NUL " + input)
            Else
                sb.Append(" --output " + targetPath.Escape + " " + input)
            End If
        End If

        Return Macro.Expand(sb.ToString.Trim.FixBreak.Replace(BR, " "))
    End Function

    Function IsCustom(pass As Integer, switch As String) As Boolean
        If switch = "" Then Return False

        If Mode.Value = x265RateMode.TwoPass OrElse Mode.Value = x265RateMode.ThreePass Then
            If pass = 1 Then
                If CustomFirstPass.Value?.Contains(switch + " ") OrElse
                    CustomFirstPass.Value?.EndsWith(switch) Then Return True
            Else
                If CustomSecondPass.Value?.Contains(switch + " ") OrElse
                    CustomSecondPass.Value?.EndsWith(switch) Then Return True
            End If
        End If

        If Custom.Value?.Contains(switch + " ") OrElse Custom.Value?.EndsWith(switch) Then Return True
    End Function

    Sub ApplyPresetValues()
        AQmode.Value = 2
        AQStrength.Value = 1
        DeblockA.Value = 0
        DeblockB.Value = 0
        FrameThreads.Value = 0
        IPRatio.Value = 1.4
        LimitModes.Value = False
        LimitRefs.Value = 3
        LimitTU.Value = 0
        LookaheadSlices.Value = 8
        MaxCuSize.Value = 0
        MinCuSize.Value = 2
        PBRatio.Value = 1.3
        PsyRD.Value = 2.0
        PsyRDOQ.Value = 1
        QComp.Value = 0.6
        RecursionSkip.Value = True

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
                MinCuSize.Value = 1
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
                LimitTU.Value = 4
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
                LimitTU.Value = 4
                LookaheadSlices.Value = 4
                MaxMerge.Value = 4
                MErange.Value = 57
                RCLookahead.Value = 40
                RD.Value = 6
                rdoqLevel.Value = 2
                Rect.Value = True
                RecursionSkip.Value = False
                Ref.Value = 5
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 4
                TUinter.Value = 3
                TUintra.Value = 3
                Weightb.Value = True
                Weightp.Value = True
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
                RecursionSkip.Value = False
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
        AQmode.DefaultValue = 2
        AQStrength.DefaultValue = 1
        DeblockA.DefaultValue = 0
        DeblockB.DefaultValue = 0
        FrameThreads.DefaultValue = 0
        IPRatio.DefaultValue = 1.4
        LimitModes.DefaultValue = False
        LimitRefs.DefaultValue = 3
        LimitTU.DefaultValue = 0
        LookaheadSlices.DefaultValue = 8
        MaxCuSize.DefaultValue = 0
        MinCuSize.DefaultValue = 2
        PBRatio.DefaultValue = 1.3
        PsyRD.DefaultValue = 2.0
        PsyRDOQ.DefaultValue = 1
        QComp.DefaultValue = 0.6
        RecursionSkip.DefaultValue = True

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
                MinCuSize.DefaultValue = 1
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
                LimitTU.DefaultValue = 4
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
                LimitTU.DefaultValue = 4
                LookaheadSlices.DefaultValue = 4
                MaxMerge.DefaultValue = 4
                MErange.DefaultValue = 57
                RCLookahead.DefaultValue = 40
                RD.DefaultValue = 6
                rdoqLevel.DefaultValue = 2
                Rect.DefaultValue = True
                RecursionSkip.DefaultValue = False
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
                RecursionSkip.DefaultValue = False
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
        rcGrain.Value = False
        qpstep.Value = 4
        constvbv.Value = False

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
                RecursionSkip.Value = False
                PsyRD.Value = 4
                PsyRDOQ.Value = 10
                SAO.Value = False
                constvbv.Value = True
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
            Case 6 '"Animation"
                PsyRD.Value = 0.4
                AQStrength.Value = 0.4
                Deblock.Value = True
                DeblockA.Value = 1
                DeblockB.Value = 1
                BFrames.Value = 6
        End Select
    End Sub

    Sub ApplyTuneDefaultValues()
        rcGrain.DefaultValue = False
        qpstep.DefaultValue = 4
        constvbv.DefaultValue = False

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
                RecursionSkip.DefaultValue = False
                PsyRD.DefaultValue = 4
                PsyRDOQ.DefaultValue = 10
                SAO.DefaultValue = False
                constvbv.DefaultValue = True
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
            Case 6 '"Animation
                PsyRD.DefaultValue = 0.4
                AQStrength.DefaultValue = 0.4
                Deblock.DefaultValue = False
                BFrames.DefaultValue = 6
        End Select
    End Sub

    Public Overrides Function GetPackage() As Package
        Return Package.x265
    End Function
End Class

Public Enum x265RateMode
    SingleBitrate
    SingleQuant
    SingleCRF
    TwoPass
    ThreePass
End Enum