Imports System.Globalization
Imports System.Runtime.Serialization
Imports System.Text

Imports StaxRip.UI
Imports StaxRip.CommandLine

Namespace x265
    <Serializable()>
    Class x265Encoder
        Inherits VideoEncoder

        Property ParamsStore As New PrimitiveStore

        Sub New()
            MyBase.New("x265")
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
            p.AvsDoc.Synchronize()
            Encode("x265", GetArgs(1))

            If Params.Mode.Value = RateMode.TwoPass OrElse
                Params.Mode.Value = RateMode.ThreePass Then

                Encode("x265 Second Pass", GetArgs(2))
            End If

            If Params.Mode.Value = RateMode.ThreePass Then
                Encode("x265 Third Pass", GetArgs(3))
            End If

            AfterEncoding()
        End Sub

        Overloads Sub Encode(passName As String, args As String)
            Using proc As New Proc
                proc.Init(passName, " frames, ")
                proc.File = Packs.avs4x26x.GetPath
                proc.Arguments = args
                proc.Start()
            End Using
        End Sub

        Overrides Sub RunCompCheck()
            Dim newParams As New x265Params
            Dim newStore = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
            newParams.Init(newStore)

            Dim avsPath = p.TempDir + p.Name + "_CompCheck.avs"
            Dim enc As New x265Encoder
            enc.Params = newParams
            enc.Params.Mode.Value = RateMode.SingleCRF
            enc.Params.Quant.Value = enc.Params.CompCheckQuant.Value

            Dim args = enc.Params.GetArgs(0, avsPath, p.TempDir + p.Name + "_CompCheck." + OutputFileType)
            RunCompCheck(Packs.avs4x26x.GetPath, args, " kb/s, eta ")
        End Sub

        Overloads Function GetArgs(pass As Integer, Optional includePaths As Boolean = True) As String
            Return Params.GetArgs(pass, p.AvsDoc.Path, Filepath.GetDirAndBase(OutputPath) +
                           "." + OutputFileType, includePaths)
        End Function

        Protected Overloads Sub RunCompCheck(executable As String,
                                             arguments As String,
                                             ParamArray logValuesToSkip As String())

            If Not Paths.VerifyRequirements Then Exit Sub
            If Not g.IsValidSource Then Exit Sub

            Dim avsPath = p.TempDir + p.Name + "_CompCheck.avs"

            Dim script = Macro.Solve(p.AvsDoc.GetScript.Trim) + CrLf + "SelectRangeEvery(" +
                ((100 \ p.CompCheckRange) * 14).ToString + ",14)"

            AviSynthDocument.SetPlugins(script)
            script.WriteFile(avsPath)

            Using proc As New Proc
                proc.Init("Compressibility Check", logValuesToSkip)
                proc.File = executable
                proc.Arguments = arguments
                proc.WriteLine(script + CrLf2)

                Try
                    proc.Start()
                Catch ex As AbortException
                    Exit Sub
                Finally
                    ProcessForm.CloseProcessForm()
                End Try
            End Using

            Dim bits = (New FileInfo(p.TempDir + p.Name + "_CompCheck." + OutputFileType).Length) * 8

            Using avi As New AVIFile(avsPath)
                p.Compressibility = (bits / avi.FrameCount) / (p.TargetWidth * p.TargetHeight)
            End Using

            OnAfterCompCheck()
            g.MainForm.Assistant()

            Log.WriteLine("Quality: " & CInt(Calc.GetPercent).ToString() + " %")
            Log.WriteLine("Compressibility: " + p.Compressibility.ToString("f3"))
            Log.Save()

            ProcessForm.CloseProcessForm()
        End Sub

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
                Return Params.Mode.Value = RateMode.SingleQuant OrElse
                    Params.Mode.Value = RateMode.SingleCRF
            End Get
            Set(Value As Boolean)
            End Set
        End Property

        Sub SetMuxer()
            Muxer = New MkvMuxer()
        End Sub

        Overrides Function CreateEditControl() As Control
            Return New x265Control(Me) With {.Dock = DockStyle.Fill}
        End Function
    End Class

    Class x265Params
        Inherits CommandLineParams

        Const NrHelp As String = "Noise reduction - an adaptive deadzone applied after DCT (subtracting from DCT coefficients), before quantization. It does no pixel-level filtering, doesn’t cross DCT block boundaries, has no overlap, The higher the strength value parameter, the more aggressively it will reduce noise." + CrLf2 + "Enabling noise reduction will make outputs diverge between different numbers of frame threads. Outputs will be deterministic but the outputs of -F2 will no longer match the outputs of -F3, etc." + CrLf2 + "Values: any value in range of 0 to 2000. 0 = disabled."

        Sub New()
            Title = "x265 Options"
        End Sub

        Property Quant As New NumParam With {
            .Name = "Quant",
            .Text = "Quality:",
            .Value = 22,
            .MinMaxStepDec = {1D, 100D, 1D, 1D}}

        Property Preset As New OptionParam With {
            .Switch = "--preset",
            .Text = "Preset:",
            .Help = "x265 has a number of predefined --preset options that make trade-offs between encode speed (encoded frames per second) and compression efficiency (quality per bit in the bitstream). The default preset is medium, it does a reasonably good job of finding the best possible quality without spending enormous CPU cycles looking for the absolute most efficient way to achieve that quality. As you go higher than medium, the encoder takes shortcuts to improve performance at the expense of quality and compression efficiency. As you go lower than medium, the encoder tries harder and harder to achieve the best quailty per bit compression ratio.",
            .Options = {"Ultra Fast", "Super Fast", "Very Fast", "Faster", "Fast", "Medium", "Slow", "Slower", "Very Slow", "Placebo"},
            .Values = {"ultrafast", "superfast", "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow", "placebo"},
            .Value = 5,
            .DefaultValue = 5}

        Property Tune As New OptionParam With {
            .Switch = "--tune",
            .Text = "Tune:",
            .Options = {"None", "PSNR", "SSIM", "Grain", "Fast Decode", "Zero Latency", "CBR"},
            .Values = {"none", "psnr", "ssim", "grain", "fastdecode", "zerolatency", "cbr"}}

        Property Mode As New OptionParam With {
            .Name = "Mode",
            .Text = "Mode:",
            .Options = {"Bitrate", "Quantizer", "Quality", "Two Pass", "Three Pass"},
            .Value = 2}

        Property SSIM As New BoolParam With {
            .Switch = "--ssim",
            .Text = "SSIM",
            .Help = "Calculate and report Structural Similarity values. It is recommended to use --tune ssim if you are measuring ssim, else the results should not be used for comparison purposes."}

        Property PSNR As New BoolParam With {
            .Switch = "--psnr",
            .Text = "PSNR",
            .Help = "Calculate and report Peak Signal to Noise Ratio. It is recommended to use --tune psnr if you are measuring PSNR, else the results should not be used for comparison purposes."}

        Property BFrames As New NumParam With {
            .Switch = "--bframes",
            .Text = "B Frames:",
            .Help = "Maximum number of consecutive b-frames. Use --bframes 0 to force all P/I low-latency encodes. This parameter has a quadratic effect on the amount of memory allocated and the amount of work performed by the full trellis version of --b-adapt lookahead.",
            .MinMaxStep = {0, 16, 1}}

        Property BFrameBias As New NumParam With {
            .Switch = "--bframe-bias",
            .Text = "B Frame Bias:",
            .Help = "Bias towards B frames in slicetype decision. The higher the bias the more likely x265 is to use B frames. Can be any value between -90 and 100 and is clipped to that range.",
            .MinMaxStep = {-90, 100, 1}}

        Property BAdapt As New OptionParam With {
            .Switch = "--b-adapt",
            .Text = "B Adapt:",
            .Options = {"None", "Fast", "Full"},
            .Help = "Adaptive B frame scheduling."}

        Property RCLookahead As New NumParam With {
            .Switch = "--rc-lookahead",
            .Text = "RC Lookahead:",
            .Help = "Number of frames for slice-type decision lookahead (a key determining factor for encoder latency). The longer the lookahead buffer the more accurate scenecut decisions will be, and the more effective cuTree will be at improving adaptive quant. Having a lookahead larger than the max keyframe interval is not helpful.",
            .MinMaxStep = {0, 250, 5}}

        Property LookaheadSlices As New NumParam With {
            .Switch = "--lookahead-slices",
            .Text = "Lookahead Slices:",
            .Help = "Use multiple worker threads to measure the estimated cost of each frame within the lookahead. When --b-adapt is 2, most frame cost estimates will be performed in batch mode, many cost estimates at the same time, and lookahead-slices is ignored for batched estimates. The effect on performance can be quite small. The higher this parameter, the less accurate the frame costs will be (since context is lost across slice boundaries) which will result in less accurate B-frame and scene-cut decisions." + CrLf2 + "The encoder may internally lower the number of slices to ensure each slice codes at least 10 16x16 rows of lowres blocks. If slices are used in lookahead, they are logged in the list of tools as lslices." + CrLf2 + "Values: 0 - disabled (default). 1 is the same as 0. Max 16",
            .MinMaxStep = {0, 16, 1}}

        Property Scenecut As New NumParam With {
            .Switch = "--scenecut",
            .Text = "Scenecut:",
            .Help = "How aggressively I-frames need to be inserted. The higher the threshold value, the more aggressive the I-frame placement. --scenecut 0 or --no-scenecut disables adaptive I frame placement.",
            .MinMaxStep = {0, 900, 10}}

        Property Ref As New NumParam With {
            .Switch = "--ref",
            .Text = "References:",
            .Help = "Max number of L0 references to be allowed. This number has a linear multiplier effect on the amount of work performed in motion search, but will generally have a beneficial affect on compression and distortion.",
            .MinMaxStep = {0, 16, 1}}

        Property [Me] As New OptionParam With {
            .Switch = "--me",
            .Text = "Motion Search Method:",
            .Options = {"Diamond", "Hexagon", "Uneven Multi-Hexegon", "Star", "Full"},
            .Values = {"dia", "hex", "umh", "star", "full"},
            .Help = "Motion search method. Generally, the higher the number the harder the ME method will try to find an optimal match. Diamond search is the simplest. Hexagon search is a little better. Uneven Multi-Hexegon is an adaption of the search method used by x264 for slower presets. Star is a three step search adapted from the HM encoder: a star-pattern search followed by an optional radix scan followed by an optional star-search refinement. Full is an exhaustive search; an order of magnitude slower than all other searches but not much better than umh or star."}

        Property MErange As New NumParam With {
            .Switch = "--merange",
            .Text = "ME Range (0=auto):",
            .Help = "Motion search range. The default is derived from the default CTU size (64) minus the luma interpolation half-length (4) minus maximum subpel distance (2) minus one extra pixel just in case the hex search method is used. If the search range were any larger than this, another CTU row of latency would be required for reference frames. Range of values: an integer from 0 to 32768.",
            .MinMaxStep = {0, 32768, 1},
            .ArgsFunc = Function() If(MErange.Value = 0, "--merange " & CInt(Calc.GetYFromTwoPointForm(480, 16, 2160, 57, p.TargetHeight)), If(MErange.Value <> MErange.defaultvalue, "--merange " & CInt(MErange.Value), ""))}

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
            .Expand = True,
            .Help = "Amount of subpel refinement to perform. The higher the number the more subpel iterations and steps are performed. At –subme values larger than 2, chroma residual cost is included in all subpel refinement steps and chroma residual is included in all motion estimation decisions (selecting the best reference picture in each list, and chosing between merge, uni-directional motion and bi-directional motion). The ‘slow’ preset is the first preset to enable the use of chroma residual."}

        Property MaxMerge As New NumParam With {
            .Switch = "--max-merge",
            .Text = "Max Merge:",
            .Help = "Maximum number of neighbor (spatial and temporal) candidate blocks that the encoder may consider for merging motion predictions. If a merge candidate results in no residual, it is immediately selected as a 'skip'. Otherwise the merge candidates are tested as part of motion estimation when searching for the least cost inter option. The max candidate number is encoded in the SPS and determines the bit cost of signaling merge CUs.",
            .MinMaxStep = {1, 5, 1}}

        Property SAOnonDeblock As New BoolParam With {
            .Switch = "--sao-non-deblock",
            .Text = "Specify how to handle depencency between SAO and deblocking filter",
            .Help = "Specify how to handle depencency between SAO and deblocking filter. When enabled, non-deblocked pixels are used for SAO analysis. When disabled, SAO analysis skips the right/bottom boundary areas."}

        Property SAO As New BoolParam With {
            .Switch = "--sao",
            .NoSwitch = "--no-sao",
            .Text = "Sample Adaptive Offset loop filter",
            .Help = "Toggle Sample Adaptive Offset loop filter. Dfault enabled."}

        Property SignHide As New BoolParam With {
            .Switch = "--signhide",
            .NoSwitch = "--no-signhide",
            .Text = "Hide sign bit of one coeff per TU (rdo)",
            .Help = "Hide sign bit of one coeff per TU (rdo). The last sign is implied. This requires analyzing all the coefficients to determine if a sign must be toggled, and then to determine which one can be toggled with the least amount of distortion."}

        Property CompCheckQuant As New NumParam With {
            .Name = "CompCheckQuant",
            .Text = "Comp. Check Quant:",
            .Value = 18,
            .MinMaxStep = {1, 50, 1}}

        Property Weightp As New BoolParam With {
            .Switch = "--weightp",
            .NoSwitch = "--no-weightp",
            .Text = "Enable weighted prediction in P slices",
            .Help = "Enable weighted prediction in P slices. This enables weighting analysis in the lookahead, which influences slice decisions, and enables weighting analysis in the main encoder which allows P reference samples to have a weight function applied to them prior to using them for motion compensation. In video which has lighting changes, it can give a large improvement in compression efficiency."}

        Property Weightb As New BoolParam With {
            .Switch = "--weightb",
            .NoSwitch = "--no-weightb",
            .Text = "Enable weighted prediction in B slices",
            .Help = "Enable weighted prediction in B slices."}

        Property AQmode As New OptionParam With {
            .Switch = "--aq-mode",
            .Text = "AQ Mode:",
            .Options = {"Disabled", "Enabled", "Auto-Variance"},
            .Help = "Adaptive Quantization operating mode. Raise or lower per-block quantization based on complexity analysis of the source image. The more complex the block, the more quantization is used. This offsets the tendency of the encoder to spend too many bits on complex areas and not enough in flat areas."}

        Property Videoformat As New OptionParam With {
            .Switch = "--videoformat",
            .Text = "Videoformat:",
            .Options = {"undefined", "component", "pal", "ntsc", "secam", "mac"},
            .Values = {"", "component", "pal", "ntsc", "secam", "mac"},
            .Help = "Specify the source format of the original analog video prior to digitizing and encoding."}

        Property AQStrength As New NumParam With {
            .Switch = "--aq-strength",
            .Text = "AQ Strength:",
            .Help = "Adjust the strength of the adaptive quantization offsets. Setting --aq-strength to 0 disables AQ.",
            .MinMaxStepDec = {0D, 3D, 0.05D, 2D},
            .Value = 1,
            .DefaultValue = 1}

        Property CUtree As New BoolParam With {
            .Switch = "--cutree",
            .NoSwitch = "--no-cutree",
            .Text = "CU Tree",
            .Help = "Enable the use of lookahead’s lowres motion vector fields to determine the amount of reuse of each block to tune adaptive quantization factors. CU blocks which are heavily reused as motion reference for later frames are given a lower QP (more bits) while CU blocks which are quickly changed and are not referenced are given less bits. This tends to improve detail in the backgrounds of video with less detail in areas of high motion.",
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
            .Expand = True,
            .Help = "Level of RDO in mode decision. The higher the value, the more exhaustive the analysis and the more rate distortion optimization is used. The lower the value the faster the encode, the higher the value the smaller the bitstream (in general)."}

        Property MinCuSize As New OptionParam With {
            .Switch = "--min-cu-size",
            .Text = "Minimum CU size:",
            .Options = {"64", "32", "16", "8"},
            .Values = {"64", "32", "16", "8"},
            .Help = "Minimum CU size (width and height). By using 16 or 32 the encoder will not analyze the cost of CUs below that minimum threshold, saving considerable amounts of compute with a predictable increase in bitrate. This setting has a large effect on performance on the faster presets."}

        Property MaxCuSize As New OptionParam With {
            .Switch = "--ctu",
            .Text = "Maximum CU size:",
            .Options = {"64", "32", "16"},
            .Values = {"64", "32", "16"},
            .Help = "Maximum CU size (width and height). The larger the maximum CU size, the more efficiently x265 can encode flat areas of the picture, giving large reductions in bitrate. However this comes at a loss of parallelism with fewer rows of CUs that can be encoded in parallel, and less frame parallelism as well. Because of this the faster presets use a CU size of 32."}

        Property TUintra As New NumParam With {
            .Switch = "--tu-intra-depth",
            .Text = "TU Intra Depth:",
            .Help = "The transform unit (residual) quad-tree begins with the same depth as the coding unit quad-tree, but the encoder may decide to further split the transform unit tree if it improves compression efficiency. This setting limits the number of extra recursion depth which can be attempted for intra coded units." + CrLf2 + "Default: 1, which means the residual quad-tree is always at the same depth as the coded unit quad-tree." + CrLf2 + "Note that when the CU intra prediction is NxN (only possible with 8x8 CUs), a TU split is implied, and thus the residual quad-tree begins at 4x4 and cannot split any futhrer.",
            .MinMaxStep = {1, 4, 1}}

        Property TUinter As New NumParam With {
            .Switch = "--tu-inter-depth",
            .Text = "TU Inter Depth:",
            .Help = "The transform unit (residual) quad-tree begins with the same depth as the coding unit quad-tree, but the encoder may decide to further split the transform unit tree if it improves compression efficiency. This setting limits the number of extra recursion depth which can be attempted for inter coded units." + CrLf2 + "Default: 1. which means the residual quad-tree is always at the same depth as the coded unit quad-tree unless the CU was coded with rectangular or AMP partitions, in which case a TU split is implied and thus the residual quad-tree begins one layer below the CU quad-tree.",
            .MinMaxStep = {1, 4, 1}}

        Property rdoqLevel As New NumParam With {
            .Switch = "--rdoq-level",
            .Text = "RDOQ Level:",
            .Help = "Specify the amount of rate-distortion analysis to use within quantization:" + CrLf2 + "At level 0 rate-distortion cost is not considered in quant" + CrLf2 + "At level 1 rate-distortion cost is used to find optimal rounding values for each level (and allows psy-rdoq to be effective). It trades-off the signaling cost of the coefficient vs its post-inverse quant distortion from the pre-quant coefficient. When --psy-rdoq is enabled, this formula is biased in favor of more energy in the residual (larger coefficient absolute levels)" + CrLf2 + "At level 2 rate-distortion cost is used to make decimate decisions on each 4x4 coding group, including the cost of signaling the group within the group bitmap. If the total distortion of not signaling the entire coding group is less than the rate cost, the block is decimated. Next, it applies rate-distortion cost analysis to the last non-zero coefficient, which can result in many (or all) of the coding groups being decimated. Psy-rdoq is less effective at preserving energy when RDOQ is at level 2, since it only has influence over the level distortion costs.",
            .MinMaxStep = {0, 2, 1}}

        Property Rect As New BoolParam With {
            .Switch = "--rect",
            .NoSwitch = "--no-rect",
            .Text = "Enable analysis of rectangular motion partitions Nx2N and 2NxN",
            .Help = "Enable analysis of rectangular motion partitions Nx2N and 2NxN (50/50 splits, two directions)."}

        Property AMP As New BoolParam With {
            .Switch = "--amp",
            .NoSwitch = "--no-amp",
            .Text = "Enable analysis of asymmetric motion partitions",
            .Help = "Enable analysis of asymmetric motion partitions (75/25 splits, four directions). At RD levels 0 through 4, AMP partitions are only considered at CU sizes 32x32 and below. At RD levels 5 and 6, it will only consider AMP partitions as merge candidates (no motion search) at 64x64, and as merge or inter candidates below 64x64. The AMP partitions which are searched are derived from the current best inter partition. If Nx2N (vertical rectangular) is the best current prediction, then left and right asymmetrical splits will be evaluated. If 2NxN (horizontal rectangular) is the best current prediction, then top and bottom asymmetrical splits will be evaluated, If 2Nx2N is the best prediction, and the block is not a merge/skip, then all four AMP partitions are evaluated. This setting has no effect if rectangular partitions are disabled."}

        Property EarlySkip As New BoolParam With {
            .Switch = "--early-skip",
            .NoSwitch = "--no-early-skip",
            .Text = "Early Skip",
            .Help = "Measure full CU size (2Nx2N) merge candidates first; if no residual is found the analysis is short circuited."}

        Property FastIntra As New BoolParam With {
            .Switch = "--fast-intra",
            .NoSwitch = "--no-fast-intra",
            .Text = "Fast Intra",
            .Help = "Perform an initial scan of every fifth intra angular mode, then check modes +/- 2 distance from the best mode, then +/- 1 distance from the best mode, effectively performing a gradient descent. When enabled 10 modes in total are checked. When disabled all 33 angular modes are checked. Only applicable for --rd levels 4 and below (medium preset and faster)."}

        Property BIntra As New BoolParam With {
            .Switch = "--b-intra",
            .NoSwitch = "--no-b-intra",
            .Text = "Evaluate intra modes in B slices",
            .Help = "Enables the evaluation of intra modes in B slices."}

        Property CUlossless As New BoolParam With {
            .Switch = "--cu-lossless",
            .Text = "CU Lossless",
            .Help = "For each CU, evaluate lossless (transform and quant bypass) encode of the best non-lossless mode option as a potential rate distortion optimization. If the global option --lossless has been specified, all CUs will be encoded as lossless unconditionally regardless of whether this option was enabled. Only effective at RD levels 3 and above, which perform RDO mode decisions."}

        Property Tskip As New BoolParam With {
            .Switch = "--tskip",
            .Text = "Enable evaluation of transform skip coding for 4x4 TU coded blocks",
            .Help = "Enable evaluation of transform skip (bypass DCT but still use quantization) coding for 4x4 TU coded blocks. Only effective at RD levels 3 and above, which perform RDO mode decisions."}

        Property TskipFast As New BoolParam With {
            .Switch = "--tskip-fast",
            .Text = "Only evaluate transform skip for NxN intra predictions (4x4 blocks)",
            .Help = "Only evaluate transform skip for NxN intra predictions (4x4 blocks). Only applicable if transform skip is enabled. For chroma, only evaluate if luma used tskip. Inter block tskip analysis is unmodified."}

        Property PsyRD As New NumParam With {
            .Switch = "--psy-rd",
            .Text = "Psy RD:",
            .Help = "Influence rate distortion optimizated mode decision to preserve the energy of the source image in the encoded image at the expense of compression efficiency. It only has effect on presets which use RDO-based mode decisions (--rd 3 and above). 1.0 is a typical value.",
            .MinMaxStepDec = {0D, 2D, 0.05D, 2D},
            .Value = 0.3,
            .DefaultValue = 0.3}

        Property PsyRDOQ As New NumParam With {
            .Switch = "--psy-rdoq",
            .Text = "Psy RDOQ:",
            .Help = "Influence rate distortion optimized quantization by favoring higher energy in the reconstructed image. This generally improves perceived visual quality at the cost of lower quality metric scores. It only has effect when --rdoq-level is 1 or 2. High values can be beneficial in preserving high-frequency detail like film grain.",
            .MinMaxStepDec = {0D, 50D, 0.05D, 2D},
            .Value = 1,
            .DefaultValue = 1}

        Property CRFmax As New NumParam With {
            .Switch = "--crf-max",
            .Text = "Maximum CRF:",
            .MinMaxStepDec = {0D, 51D, 1D, 1D},
            .Value = 51,
            .DefaultValue = 51,
            .Help = "Specify an upper limit to the rate factor which may be assigned to any given frame (ensuring a max QP). This is dangerous when CRF is used in combination with VBV as it may result in buffer underruns."}

        Property CRFmin As New NumParam With {
            .Switch = "--crf-min",
            .Text = "Minimum CRF:",
            .Help = "Specify an lower limit to the rate factor which may be assigned to any given frame (ensuring a min compression factor).",
            .MinMaxStepDec = {0D, 51D, 1D, 1D}}

        Property PBRatio As New NumParam With {
            .Switch = "--pbratio",
            .Text = "PB Ratio:",
            .Help = "QP ratio factor between P and B slices. This ratio is used in all of the rate control modes. Some --tune options may change the default value. It is not typically manually specified.",
            .MinMaxStepDec = {0D, 1000D, 0.05D, 2D},
            .Value = 1.3}

        Property IPRatio As New NumParam With {
            .Switch = "--ipratio",
            .Text = "IP Ratio:",
            .Help = "QP ratio factor between I and P slices. This ratio is used in all of the rate control modes. Some --tune options may change the default value. It is not typically manually specified.",
            .MinMaxStepDec = {0D, 1000D, 0.05D, 2D},
            .Value = 1.4}

        Property QComp As New NumParam With {
            .Switch = "--qcomp",
            .Text = "qComp:",
            .Help = "qComp sets the quantizer curve compression factor. It weights the frame quantizer based on the complexity of residual (measured by lookahead). Increasing to 1 will effectively generate CQP.",
            .MinMaxStepDec = {0D, 1000D, 0.05D, 2D},
            .Value = 0.6}

        Property QBlur As New NumParam With {
            .Switch = "--qblur",
            .Text = "Q Blur:",
            .Help = "Temporally blur quants.",
            .MinMaxStepDec = {Integer.MinValue, Integer.MaxValue, 0.05D, 2D},
            .Value = 0.5,
            .DefaultValue = 0.5}

        Property Cplxblur As New NumParam With {
            .Switch = "--cplxblur",
            .Text = "Blur Complexity:",
            .MinMaxStepDec = {Integer.MinValue, Integer.MaxValue, 0.05D, 2D},
            .Value = 20,
            .DefaultValue = 20}

        Property LogLevel As New OptionParam With {
            .Switch = "--log-level",
            .Text = "Log Level:",
            .Options = {"None", "Error", "Warning", "Info", "Frame", "Debug", "Full"},
            .Values = {"none", "error", "warning", "info", "frame", "debug", "full"},
            .Help = "Logging level. Debug level enables per-frame QP, metric, and bitrate logging. If a CSV file is being generated, frame level makes the log be per-frame rather than per-encode. Full level enables hash and weight logging. -1 disables all logging, except certain fatal errors, and can be specified by the string 'none'.",
            .Value = 3,
            .DefaultValue = 3}

        Property Colorprim As New OptionParam With {
            .Switch = "--colorprim",
            .Text = "Colorprim:",
            .Options = {"undefined", "bt709", "bt470m", "bt470bg", "smpte170m", "smpte240m", "film", "bt2020"},
            .Values = {"", "bt709", "bt470m", "bt470bg", "smpte170m", "smpte240m", "film", "bt2020"},
            .Help = "Specify color primitive to use when converting to RGB."}

        Property Transfer As New OptionParam With {
            .Switch = "--transfer",
            .Text = "Transfer:",
            .Options = {"undefined", "bt709", "bt470m", "bt470bg", "smpte170m", "smpte240m", "linear", "log100", "log316", "iec61966-2-4", "bt1361e", "iec61966-2-1", "bt2020-10", "bt2020-12", "smpte-st-2084", "smpte-st-428"},
            .Values = {"", "bt709", "bt470m", "bt470bg", "smpte170m", "smpte240m", "linear", "log100", "log316", "iec61966-2-4", "bt1361e", "iec61966-2-1", "bt2020-10", "bt2020-12", "smpte-st-2084", "smpte-st-428"},
            .Help = "Specify transfer characteristics."}

        Property Colormatrix As New OptionParam With {
            .Switch = "--colormatrix",
            .Text = "Colormatrix:",
            .Options = {"undefined", "GBR", "bt709", "fcc", "bt470bg", "smpte170m", "smpte240m", "YCgCo", "bt2020nc", "bt2020c"},
            .Values = {"", "GBR", "bt709", "fcc", "bt470bg", "smpte170m", "smpte240m", "YCgCo", "bt2020nc", "bt2020c"},
            .Help = "Specify color matrix setting i.e set the matrix coefficients used in deriving the luma and chroma."}

        Property CuStats As New BoolParam With {
            .Switch = "--cu-stats",
            .Text = "Record statistics on how each CU was coded",
            .Help = "Records statistics on how each CU was coded (split depths and other mode decisions) and reports those statistics at the end of the encode."}

        Property Pools As New StringParam With {
            .Switch = "--pools",
            .Text = "Pools:",
            .Help = "Comma seperated list of threads per NUMA node. If ""none"", then no worker pools are created and only frame parallelism is possible. If NULL or """" (default) x265 will use all available threads on each NUMA node:" + CrLf + "" + CrLf + """+"" is a special value indicating all cores detected on the node" + CrLf + """*"" is a special value indicating all cores detected on the node and all remaining nodes" + CrLf + """-"" is a special value indicating no cores on the node, same as ""0""" + CrLf + "" + CrLf + "example strings for a 4-node system:" + CrLf + "" + CrLf + """"" - default, unspecified, all numa nodes are used for thread pools" + CrLf + """*"" - same as default" + CrLf + """none"" - no thread pools are created, only frame parallelism possible" + CrLf + """-"" - same as ""none""" + CrLf + """10"" - allocate one pool, using up to 10 cores on node 0" + CrLf + """-,+"" - allocate one pool, using all cores on node 1" + CrLf + """+,-,+"" - allocate two pools, using all cores on nodes 0 and 2" + CrLf + """+,-,+,-"" - allocate two pools, using all cores on nodes 0 and 2" + CrLf + """-,*"" - allocate three pools, using all cores on nodes 1, 2 and 3" + CrLf + """8,8,8,8"" - allocate four pools with up to 8 threads in each pool" + CrLf2 + "The total number of threads will be determined by the number of threads assigned to all nodes. The worker threads will each be given affinity for their node, they will not be allowed to migrate between nodes, but they will be allowed to move between CPU cores within their node." + CrLf + "" + CrLf + "If the three pool features: --wpp --pmode and --pme are all disabled, then --pools is ignored and no thread pools are created." + CrLf + "" + CrLf + "If ""none"" is specified, then all three of the thread pool features are implicitly disabled." + CrLf + "" + CrLf + "Multiple thread pools will be allocated for any NUMA node with more than 64 logical CPU cores. But any given thread pool will always use at most one NUMA node." + CrLf + "" + CrLf + "Frame encoders are distributed between the available thread pools, and the encoder will never generate more thread pools than --frame-threads. The pools are used for WPP and for distributed analysis and motion search." + CrLf + "" + CrLf + "Default """", one thread is allocated per detected hardware thread (logical CPU cores) and one thread pool per NUMA node.",
            .UseQuotes = True}

        Property Qstep As New NumParam With {
            .Switch = "--qstep",
            .Text = "Q Step:",
            .Help = "The maximum single adjustment in QP allowed to rate control.",
            .MinMaxStep = {0, Integer.MaxValue, 1},
            .Value = 4,
            .DefaultValue = 4}

        Property WPP As New BoolParam With {
            .Switch = "--wpp",
            .noSwitch = "--no-wpp",
            .Text = "Wavefront Parallel Processing",
            .Help = "Enable Wavefront Parallel Processing. The encoder may begin encoding a row as soon as the row above it is at least two CTUs ahead in the encode process. This gives a 3-5x gain in parallelism for about 1% overhead in compression efficiency. This feature is implicitly disabled when no thread pool is present.",
            .Value = True,
            .DefaultValue = True}

        Property Pmode As New BoolParam With {
            .Switch = "--pmode",
            .noSwitch = "--no-pmode",
            .Text = "Parallel Mode Decision",
            .Help = "Parallel mode decision, or distributed mode analysis. When enabled the encoder will distribute the analysis work of each CU (merge, inter, intra) across multiple worker threads. Only recommended if x265 is not already saturating the CPU cores. In RD levels 3 and 4 it will be most effective if –rect is enabled. At RD levels 5 and 6 there is generally always enough work to distribute to warrant the overhead, assuming your CPUs are not already saturated. –pmode will increase utilization without reducing compression efficiency. In fact, since the modes are all measured in parallel it makes certain early-outs impractical and thus you usually get slightly better compression when it is enabled (at the expense of not skipping improbable modes). This bypassing of early-outs can cause pmode to slow down encodes, especially at faster presets. This feature is implicitly disabled when no thread pool is present."}

        Property PME As New BoolParam With {
            .Switch = "--pme",
            .noSwitch = "--no-pme",
            .Text = "Parallel Motion Estimation",
            .Help = "Parallel motion estimation. When enabled the encoder will distribute motion estimation across multiple worker threads when more than two references require motion searches for a given CU. Only recommended if x265 is not already saturating CPU cores. --pmode is much more effective than this option, since the amount of work it distributes is substantially higher. With –pme it is not unusual for the overhead of distributing the work to outweigh the parallelism benefits. This feature is implicitly disabled when no thread pool is present. –pme will increase utilization on many core systems with no effect on the output bitstream."}

        Property Dither As New BoolParam With {
            .Switch = "--dither",
            .Text = "Enable high quality downscaling",
            .Help = "Enable high quality downscaling. Dithering is based on the diffusion of errors from one row of pixels to the next row of pixels in a picture. Only applicable when the input bit depth is larger than 8bits and internal bit depth is 8bits."}

        Property InterlaceMode As New OptionParam With {
            .Switch = "--interlaceMode",
            .Text = "Interlace Mode:",
            .Options = {"Progressive", "Top field first", "Bottom field first"},
            .Values = {"", "tff", "bff"},
            .Help = "HEVC encodes interlaced content as fields. Fields must be provided to the encoder in the correct temporal order. The source dimensions must be field dimensions and the FPS must be in units of fields per second. The decoder must re-combine the fields in their correct orientation for display."}

        Property Profile As New OptionParam With {
            .Switch = "--profile",
            .Text = "Profile:",
            .Options = {"main", "main10", "mainstillpicture", "main422-8", "main422-10", "main444-8", "main444-10"},
            .Values = {"main", "main10", "mainstillpicture", "main422-8", "main422-10", "main444-8", "main444-10"},
            .Help = "Enforce the requirements of the specified profile, ensuring the output stream will be decodable by a decoder which supports that profile. May abort the encode if the specified profile is impossible to be supported by the compile options chosen for the encoder (a high bit depth encoder will be unable to output bitstreams compliant with Main or Mainstillpicture). API users must use x265_param_apply_profile() after configuring their param structure. Any changes made to the param structure after this call might make the encode non-compliant."}

        Property Level As New OptionParam With {
            .Switch = "--level-idc",
            .Text = "Level:",
            .Options = {"Unrestricted", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"},
            .Values = {"", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"},
            .Help = "Minimum decoder requirement level. Unrestricted implies auto-detection by the encoder. If specified, the encoder will attempt to bring the encode specifications within that specified level. If the encoder is unable to reach the level it issues a warning and aborts the encode. If the requested requirement level is higher than the actual level, the actual requirement level is signaled. Beware, specifying a decoder level will force the encoder to enable VBV for constant rate factor encodes, which may introduce non-determinism. The value is specified as a float or as an integer with the level times 10, for example level 5.1 is specified as '5.1' or '51', and level 5.0 is specified as '5.0' or '50'."}

        Property Hash As New OptionParam With {
            .Switch = "--hash",
            .Text = "Hash:",
            .Options = {"None", "MD5", "CRC", "Checksum"},
            .Values = {"", "MD5", "CRC", "Checksum"},
            .Help = "Emit decoded picture hash SEI, so the decoder may validate the reconstructed pictures and detect data loss. Also useful as a debug feature to validate the encoder state."}

        Property HighTier As New BoolParam With {
            .Switch = "--high-tier",
            .Text = "High Tier",
            .Help = "If --level-idc has been specified, the option adds the intention to support the High tier of that level. If your specified level does not support a High tier, a warning is issued and this modifier flag is ignored."}

        Property TemporalMVP As New BoolParam With {
            .Switch = "--temporal-mvp",
            .NoSwitch = "--no-temporal-mvp",
            .Text = "Enable temporal motion vector predictors in P and B slices",
            .Help = "Enable temporal motion vector predictors in P and B slices. This enables the use of the motion vector from the collocated block in the previous frame to be used as a predictor.",
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
            .Help = "Constrained intra prediction. When generating intra predictions for blocks in inter slices, only intra-coded reference pixels are used. Inter-coded reference pixels are replaced with intra-coded neighbor pixels or default values. The general idea is to block the propagation of reference errors that may have resulted from lossy signals.",
            .Value = True,
            .DefaultValue = True}

        Property RDpenalty As New NumParam With {
            .Switch = "--rdpenalty",
            .Text = "RD Penalty (0=disabled):",
            .Help = "When set to 1, transform units of size 32x32 are given a 4x bit cost penalty compared to smaller transform units, in intra coded CUs in P or B slices. When set to 2, transform units of size 32x32 are not even attempted, unless otherwise required by the maximum recursion depth. For this option to be effective with 32x32 intra CUs, --tu-intra-depth must be at least 2. For it to be effective with 64x64 intra CUs, --tu-intra-depth must be at least 3. Note that in HEVC an intra transform unit (a block of the residual quad-tree) is also a prediction unit, meaning that the intra prediction signal is generated for each TU block, the residual subtracted and then coded. The coding unit simply provides the prediction modes that will be used when predicting all of the transform units within the CU. This means that when you prevent 32x32 intra transform units, you are preventing 32x32 intra predictions.",
            .MinMaxStep = {0, 2, 1}}

        Property OpenGop As New BoolParam With {
            .Switch = "--open-gop",
            .NoSwitch = "--no-open-gop",
            .Text = "Open GOP",
            .Help = "Enable open GOP, allow I-slices to be non-IDR.",
            .Value = True,
            .DefaultValue = True}

        Property Bpyramid As New BoolParam With {
            .Switch = "--b-pyramid",
            .NoSwitch = "--no-b-pyramid",
            .Text = "B Pyramid",
            .Help = "Use B-frames as references, when possible.",
            .Value = True,
            .DefaultValue = True}

        Property Lossless As New BoolParam With {
            .Switch = "--lossless",
            .Text = "Lossless",
            .Help = "Enables true lossless coding by bypassing scaling, transform, quantization and in-loop filter processes. This is used for ultra-high bitrates with zero loss of quality. Reconstructed output pictures are bit-exact to the input pictures. Lossless encodes implicitly have no rate control, all rate control options are ignored. Slower presets will generally achieve better compression efficiency (and generate smaller bitstreams)."}

        Property SlowFirstpass As New BoolParam With {
            .Switch = "--slow-firstpass",
            .Text = "Slow Firstpass",
            .Help = "Enable a slow and more detailed first pass encode in multi-pass rate control mode. Speed of the first pass encode is slightly lesser and quality midly improved when compared to the default settings in a multi-pass encode. When turbo first pass is not disabled, these options are set on the first pass to improve performance: --fast-intra, --no-rect, --no-amp, --early-skip, --ref = 1, --max-merge = 1, --me = DIA, --subme = MIN(2, --subme), --rd = MIN(2, --rd)",
            .Value = False,
            .DefaultValue = False}

        Property StrictCBR As New BoolParam With {
            .Switch = "--strict-cbr",
            .Text = "Strict CBR",
            .Help = "Enables stricter conditions to control bitrate deviance from the target bitrate in CBR mode. Bitrate adherence is prioritised over quality. Rate tolerance is reduced to 50%. This option is for use-cases which require the final average bitrate to be within very strict limits of the target - preventing overshoots completely, and achieve bitrates within 5% of target bitrate, especially in short segment encodes. Typically, the encoder stays conservative, waiting until there is enough feedback in terms of encoded frames to control QP. strict-cbr allows the encoder to be more aggressive in hitting the target bitrate even for short segment videos. Experimental."}

        Property CBQPoffs As New NumParam With {
            .Switch = "--cbqpoffs",
            .Text = "Cb QP Offset:",
            .Help = "Offset of Cb chroma QP from the luma QP selected by rate control. This is a general way to spend more or less bits on the chroma channel.",
            .MinMaxStep = {-12, 12, 1}}

        Property CRQPoffs As New NumParam With {
            .Switch = "--crqpoffs",
            .Text = "CR QP Offset:",
            .Help = "Offset of Cr chroma QP from the luma QP selected by rate control. This is a general way to spend more or less bits on the chroma channel.",
            .MinMaxStep = {-12, 12, 1}}

        Property NRintra As New NumParam With {
            .Switch = "--nr-intra",
            .Text = "Intra Noise Reduction:",
            .Help = NrHelp,
            .MinMaxStep = {0, 2000, 50}}

        Property NRinter As New NumParam With {
            .Switch = "--nr-inter",
            .Text = "Inter Noise Reduction:",
            .Help = NrHelp,
            .MinMaxStep = {0, 2000, 50}}

        Property Keyint As New NumParam With {
            .Switch = "--keyint",
            .Text = "Maximum GOP Size:",
            .Help = "Max intra period in frames. A special case of infinite-gop (single keyframe at the beginning of the stream) can be triggered with argument -1. Use 1 to force all-intra.",
            .MinMaxStep = {0, 10000, 10},
            .Value = 250,
            .DefaultValue = 250}

        Property MinKeyint As New NumParam With {
            .Switch = "--min-keyint",
            .Text = "Minimum GOP Size:",
            .Help = "Minimum GOP size. Scenecuts closer together than this are coded as I or P, not IDR. Minimum keyint is clamped to be at least half of --keyint. If you wish to force regular keyframe intervals and disable adaptive I frame placement, you must use --no-scenecut. Range of values: >=0 (0: auto)",
            .MinMaxStep = {0, 10000, 10}}

        Property VBVbufsize As New NumParam With {
            .Switch = "--vbv-bufsize",
            .Text = "VBV Bufsize:",
            .LabelMargin = New Padding With {.Right = 6, .Left = 6},
            .Help = "Specify the size of the VBV buffer (kbits). Enables VBV in ABR mode. In CRF mode, --vbv-maxrate must also be specified.",
            .MinMaxStep = {0, 1000000, 100}}

        Property VBVmaxrate As New NumParam With {
            .Switch = "--vbv-maxrate",
            .Text = "VBV Maxrate:",
            .LabelMargin = New Padding With {.Right = 7},
            .Help = "Maximum local bitrate (kbits/sec). Will be used only if vbv-bufsize is also non-zero. Both vbv-bufsize and vbv-maxrate are required to enable VBV in CRF mode.",
            .MinMaxStep = {0, 1000000, 100}}

        Property VBVinit As New NumParam With {
            .Switch = "--vbv-init",
            .Text = "VBV Init:",
            .Help = "Initial buffer occupancy. The portion of the decode buffer which must be full before the decoder will begin decoding. Determines absolute maximum frame size.",
            .MinMaxStepDec = {0D, 1D, 0.05D, 2D},
            .Value = 0.9,
            .DefaultValue = 0.9}

        Property Chromaloc As New NumParam With {
            .Switch = "--chromaloc",
            .Text = "Chromaloc:",
            .Help = "Specify chroma sample location for 4:2:0 inputs. Consult the HEVC specification for a description of these values.",
            .MinMaxStep = {0, 5, 1}}

        Property FrameThreads As New NumParam With {
            .Switch = "--frame-threads",
            .Text = "Frame Threads (0=auto):",
            .Help = "Number of concurrently encoded frames. Using a single frame thread gives a slight improvement in compression, since the entire reference frames are always available for motion compensation, but it has severe performance implications. Default is an autodetected count based on the number of CPU cores and whether WPP is enabled or not. Over-allocation of frame threads will not improve performance, it will generally just increase memory use. Values: any value between 8 and 16."}

        Property RepeatHeaders As New BoolParam With {
            .Switch = "--repeat-headers",
            .Text = "Repeat Headers",
            .Help = "If enabled, x265 will emit VPS, SPS, and PPS headers with every keyframe. This is intended for use when you do not have a container to keep the stream headers for you and you want keyframes to be random access points."}

        Property Info As New BoolParam With {
            .Switch = "--info",
            .NoSwitch = "--no-info",
            .Text = "Info",
            .Help = "Emit an informational SEI with the stream headers which describes the encoder version, build info, and encode parameters. This is very helpful for debugging purposes but encoding version numbers and build info could make your bitstreams diverge and interfere with regression testing.",
            .Value = True,
            .DefaultValue = True}

        Property HRD As New BoolParam With {
            .Switch = "--hrd",
            .Text = "HRD",
            .Help = "If enable the signalling of HRD parameters to the decoder. The HRD parameters are carried by the Buffering Period SEI messages and Picture Timing SEI messages providing timing information to the decoder."}

        Property AUD As New BoolParam With {
            .Switch = "--aud",
            .Text = "AUD",
            .Help = "Emit an access unit delimiter NAL at the start of each slice access unit. If --repeat-headers is not enabled (indicating the user will be writing headers manually at the start of the stream) the very first AUD will be skipped since it cannot be placed at the start of the access unit, where it belongs."}

        Property Custom As New StringParam With {
            .Text = "Custom Switches:"}

        Property Deblock As New BoolParam With {
            .Switch = "--deblock",
            .Text = "Deblocking",
            .Help = "Deblocking loop filter",
            .ArgsFunc = AddressOf GetDeblockArgs}

        Property DeblockA As New NumParam With {
            .Text = "      Strength:",
            .MinMaxStep = {-6, 6, 1}}

        Property DeblockB As New NumParam With {
            .Text = "      Threshold:",
            .MinMaxStep = {-6, 6, 1}}

        Property MaxTuSize As New OptionParam With {
            .Switch = "--max-tu-size",
            .Text = "Max TU Size",
            .Help = "Maximum TU size (width and height). The residual can be more efficiently compressed by the DCT transform when the max TU size is larger, but at the expense of more computation. Transform unit quad-tree begins at the same depth of the coded tree unit, but if the maximum TU size is smaller than the CU size then transform QT begins at the depth of the max-tu-size.",
            .Options = {"32", "16", "8", "4"},
            .Values = {"32", "16", "8", "4"}}

        Private ItemsValue As List(Of CommandLineItem)

        Overrides ReadOnly Property Items As List(Of CommandLineItem)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineItem)

                    Add("Basic", Quant, Preset, Tune, Profile, Level, Mode)
                    Add("Analysis 1", RD, MinCuSize, MaxCuSize, MaxTuSize, TUintra, TUinter, rdoqLevel)
                    Add("Analysis 2", Rect, AMP, EarlySkip, FastIntra, BIntra, CUlossless, Tskip, TskipFast)
                    Add("Rate Control 1", AQmode, AQStrength, IPRatio, PBRatio, QComp, CBQPoffs, Qstep, QBlur, Cplxblur, CUtree, Lossless, StrictCBR)
                    Add("Rate Control 2", NRintra, NRinter, CRFmin, CRFmax, VBVbufsize, VBVmaxrate, VBVinit)
                    Add("Motion Search", SubME, [Me], MErange, MaxMerge, Weightp, Weightb, TemporalMVP)
                    Add("Slice Decision", BAdapt, BFrames, BFrameBias, RCLookahead, LookaheadSlices, Scenecut, Ref, MinKeyint, Keyint, Bpyramid, OpenGop)
                    Add("Spatial/Intra", StrongIntraSmoothing, ConstrainedIntra, RDpenalty)
                    Add("Performance", Pools, FrameThreads, WPP, Pmode, PME)
                    Add("Statistic", LogLevel, SSIM, PSNR, CuStats)
                    Add("VUI", Videoformat, Colorprim, Colormatrix, Transfer)
                    Add("Bitstream", Hash, RepeatHeaders, Info, HRD, AUD)
                    Add("Other", InterlaceMode, Deblock, DeblockA, DeblockB, PsyRD, PsyRDOQ, CompCheckQuant, SAO, HighTier, SAOnonDeblock, Dither, SlowFirstpass, SignHide, Custom)
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

            If item Is Preset Then
                BlockValueChanged = True
                ApplyPresetValues()
                BlockValueChanged = False
            End If

            If item Is Tune Then
                BlockValueChanged = True
                ApplyTuneValues()
                BlockValueChanged = False
            End If

            DeblockA.NumEdit.Enabled = Deblock.Value
            DeblockB.NumEdit.Enabled = Deblock.Value

            MyBase.OnValueChanged(item)
        End Sub

        Overloads Overrides Function GetArgs(includePaths As Boolean) As String
            Return GetArgs(1, p.AvsDoc.Path, Filepath.GetDirAndBase(p.VideoEncoder.OutputPath) +
                           "." + p.VideoEncoder.OutputFileType, includePaths)
        End Function

        Overloads Function GetArgs(pass As Integer,
                                   sourcePath As String,
                                   targetPath As String,
                                   Optional includePaths As Boolean = True) As String

            ApplyPresetDefaultValues()
            ApplyTuneDefaultValues()

            Dim sb As New StringBuilder

            If includePaths Then
                sb.Append(" --x26x-binary """ + Packs.x265.GetPath + """")
            End If

            If Mode.Value = RateMode.TwoPass OrElse Mode.Value = RateMode.ThreePass Then
                sb.Append(" --pass " & pass)
            End If

            If Mode.Value = RateMode.SingleQuant Then
                sb.Append(" --qp " + CInt(Quant.Value).ToString)
            ElseIf Mode.Value = RateMode.SingleCRF Then
                If Quant.Value <> 28 Then
                    sb.Append(" --crf " + Quant.Value.ToString(CultureInfo.InvariantCulture))
                End If
            Else
                sb.Append(" --bitrate " & p.VideoBitrate)
            End If

            Dim q = From i In Items Where i.GetArgs <> ""

            If q.Count > 0 Then
                sb.Append(" " + q.Select(Function(item) item.GetArgs).Join(" "))
            End If

            If sourcePath <> "" AndAlso includePaths Then
                sb.Append(" --input-res " & p.TargetWidth & "x" & p.TargetHeight)
                sb.Append(" --fps " + p.AvsDoc.GetFramerate.ToString("f6", CultureInfo.InvariantCulture))

                If Calc.IsARSignalingRequired Then
                    Dim par = Calc.GetTargetPAR
                    sb.Append(" --sar " & par.X & ":" & par.Y)
                End If

                If Mode.Value = RateMode.TwoPass OrElse Mode.Value = RateMode.ThreePass Then
                    sb.Append(" --stats """ + p.TempDir + p.Name + ".stats""")
                End If

                If (Mode.Value = RateMode.ThreePass AndAlso pass < 3) OrElse
                    Mode.Value = RateMode.TwoPass AndAlso pass = 1 Then

                    sb.Append(" --output NUL """ + sourcePath + """")
                Else
                    sb.Append(" --output """ + targetPath + """ """ + sourcePath + """")
                End If
            End If

            Return Macro.Solve(sb.ToString.Trim)
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
            MinCuSize.Value = 3
            MaxCuSize.Value = 0
            AQmode.Value = 1

            Select Case Preset.Value
                Case 0 'ultrafast
                    [Me].Value = 0
                    AMP.Value = False
                    AQmode.Value = 0
                    BAdapt.Value = 0
                    BFrames.Value = 3
                    BIntra.Value = False
                    MaxCuSize.Value = 1
                    MinCuSize.Value = 2
                    CUtree.Value = False
                    Deblock.Value = True
                    EarlySkip.Value = True
                    FastIntra.Value = True
                    MaxMerge.Value = 2
                    MErange.Value = 25
                    RCLookahead.Value = 5
                    RD.Value = 2
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
                    rdoqLevel.Value = 0
                Case 1 'superfast
                    [Me].Value = 1
                    AMP.Value = False
                    AQmode.Value = 0
                    BAdapt.Value = 0
                    BFrames.Value = 3
                    BIntra.Value = False
                    MaxCuSize.Value = 1
                    CUtree.Value = False
                    Deblock.Value = True
                    EarlySkip.Value = True
                    FastIntra.Value = True
                    MaxMerge.Value = 2
                    MErange.Value = 44
                    RCLookahead.Value = 10
                    RD.Value = 2
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
                    rdoqLevel.Value = 0
                Case 2 'veryfast
                    [Me].Value = 1
                    AMP.Value = False
                    BAdapt.Value = 0
                    BFrames.Value = 4
                    BIntra.Value = False
                    MaxCuSize.Value = 1
                    CUtree.Value = False
                    Deblock.Value = True
                    EarlySkip.Value = True
                    FastIntra.Value = True
                    MaxMerge.Value = 2
                    MErange.Value = 57
                    RCLookahead.Value = 15
                    RD.Value = 2
                    Rect.Value = False
                    Ref.Value = 1
                    SAO.Value = True
                    Scenecut.Value = 40
                    SignHide.Value = True
                    SubME.Value = 1
                    TUinter.Value = 1
                    TUintra.Value = 1
                    Weightb.Value = False
                    Weightp.Value = True
                    rdoqLevel.Value = 0
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
                    Rect.Value = False
                    Ref.Value = 1
                    SAO.Value = True
                    Scenecut.Value = 40
                    SignHide.Value = True
                    SubME.Value = 2
                    TUinter.Value = 1
                    TUintra.Value = 1
                    Weightb.Value = False
                    Weightp.Value = True
                    rdoqLevel.Value = 0
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
                    rdoqLevel.Value = 0
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
                    rdoqLevel.Value = 0
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
                    MaxMerge.Value = 3
                    MErange.Value = 57
                    RCLookahead.Value = 25
                    RD.Value = 4
                    Rect.Value = True
                    Ref.Value = 3
                    SAO.Value = True
                    Scenecut.Value = 40
                    SignHide.Value = True
                    SubME.Value = 3
                    TUinter.Value = 1
                    TUintra.Value = 1
                    Weightb.Value = False
                    Weightp.Value = True
                    rdoqLevel.Value = 2
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
                    MaxMerge.Value = 3
                    MErange.Value = 57
                    RCLookahead.Value = 30
                    RD.Value = 6
                    Rect.Value = True
                    Ref.Value = 3
                    SAO.Value = True
                    Scenecut.Value = 40
                    SignHide.Value = True
                    SubME.Value = 3
                    TUinter.Value = 2
                    TUintra.Value = 2
                    Weightb.Value = True
                    Weightp.Value = True
                    rdoqLevel.Value = 2
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
                    MaxMerge.Value = 5
                    MErange.Value = 92
                    RCLookahead.Value = 60
                    RD.Value = 6
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
                    rdoqLevel.Value = 2
            End Select
        End Sub

        Sub ApplyPresetDefaultValues()
            MinCuSize.DefaultValue = 3
            MaxCuSize.DefaultValue = 0
            AQmode.DefaultValue = 1

            Select Case Preset.Value
                Case 0 'ultrafast
                    [Me].DefaultValue = 0
                    AMP.DefaultValue = False
                    AQmode.DefaultValue = 0
                    BAdapt.DefaultValue = 0
                    BFrames.DefaultValue = 3
                    BIntra.DefaultValue = False
                    MaxCuSize.DefaultValue = 1
                    MinCuSize.DefaultValue = 2
                    CUtree.DefaultValue = False
                    Deblock.DefaultValue = True
                    EarlySkip.DefaultValue = True
                    FastIntra.DefaultValue = True
                    MaxMerge.DefaultValue = 2
                    MErange.DefaultValue = 25
                    RCLookahead.DefaultValue = 5
                    RD.DefaultValue = 2
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
                    rdoqLevel.DefaultValue = 0
                Case 1 'superfast
                    [Me].DefaultValue = 1
                    AMP.DefaultValue = False
                    AQmode.DefaultValue = 0
                    BAdapt.DefaultValue = 0
                    BFrames.DefaultValue = 3
                    BIntra.DefaultValue = False
                    MaxCuSize.DefaultValue = 1
                    CUtree.DefaultValue = False
                    Deblock.DefaultValue = True
                    EarlySkip.DefaultValue = True
                    FastIntra.DefaultValue = True
                    MaxMerge.DefaultValue = 2
                    MErange.DefaultValue = 44
                    RCLookahead.DefaultValue = 10
                    RD.DefaultValue = 2
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
                    rdoqLevel.DefaultValue = 0
                Case 2 'veryfast
                    [Me].DefaultValue = 1
                    AMP.DefaultValue = False
                    BAdapt.DefaultValue = 0
                    BFrames.DefaultValue = 4
                    BIntra.DefaultValue = False
                    MaxCuSize.DefaultValue = 1
                    CUtree.DefaultValue = False
                    Deblock.DefaultValue = True
                    EarlySkip.DefaultValue = True
                    FastIntra.DefaultValue = True
                    MaxMerge.DefaultValue = 2
                    MErange.DefaultValue = 57
                    RCLookahead.DefaultValue = 15
                    RD.DefaultValue = 2
                    Rect.DefaultValue = False
                    Ref.DefaultValue = 1
                    SAO.DefaultValue = True
                    Scenecut.DefaultValue = 40
                    SignHide.DefaultValue = True
                    SubME.DefaultValue = 1
                    TUinter.DefaultValue = 1
                    TUintra.DefaultValue = 1
                    Weightb.DefaultValue = False
                    Weightp.DefaultValue = True
                    rdoqLevel.DefaultValue = 0
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
                    Rect.DefaultValue = False
                    Ref.DefaultValue = 1
                    SAO.DefaultValue = True
                    Scenecut.DefaultValue = 40
                    SignHide.DefaultValue = True
                    SubME.DefaultValue = 2
                    TUinter.DefaultValue = 1
                    TUintra.DefaultValue = 1
                    Weightb.DefaultValue = False
                    Weightp.DefaultValue = True
                    rdoqLevel.DefaultValue = 0
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
                    rdoqLevel.DefaultValue = 0
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
                    rdoqLevel.DefaultValue = 0
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
                    MaxMerge.DefaultValue = 3
                    MErange.DefaultValue = 57
                    RCLookahead.DefaultValue = 25
                    RD.DefaultValue = 4
                    Rect.DefaultValue = True
                    Ref.DefaultValue = 3
                    SAO.DefaultValue = True
                    Scenecut.DefaultValue = 40
                    SignHide.DefaultValue = True
                    SubME.DefaultValue = 3
                    TUinter.DefaultValue = 1
                    TUintra.DefaultValue = 1
                    Weightb.DefaultValue = False
                    Weightp.DefaultValue = True
                    rdoqLevel.DefaultValue = 2
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
                    MaxMerge.DefaultValue = 3
                    MErange.DefaultValue = 57
                    RCLookahead.DefaultValue = 30
                    RD.DefaultValue = 6
                    Rect.DefaultValue = True
                    Ref.DefaultValue = 3
                    SAO.DefaultValue = True
                    Scenecut.DefaultValue = 40
                    SignHide.DefaultValue = True
                    SubME.DefaultValue = 3
                    TUinter.DefaultValue = 2
                    TUintra.DefaultValue = 2
                    Weightb.DefaultValue = True
                    Weightp.DefaultValue = True
                    rdoqLevel.DefaultValue = 2
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
                    MaxMerge.DefaultValue = 4
                    MErange.DefaultValue = 57
                    RCLookahead.DefaultValue = 40
                    RD.DefaultValue = 6
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
                    rdoqLevel.DefaultValue = 2
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
                    MaxMerge.DefaultValue = 5
                    MErange.DefaultValue = 92
                    RCLookahead.DefaultValue = 60
                    RD.DefaultValue = 6
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
                    rdoqLevel.DefaultValue = 2
            End Select
        End Sub

        Sub ApplyTuneValues()
            PsyRD.Value = 0.3
            PsyRDOQ.Value = 1
            AQStrength.Value = 1
            PBRatio.Value = 1.3
            IPRatio.Value = 1.4
            QComp.Value = 0.6
            DeblockA.Value = 0
            DeblockB.Value = 0

            Select Case Tune.Value
                Case 3 'grain
                    PsyRD.Value = 0.5
                    PsyRDOQ.Value = 30
                    AQStrength.Value = 0.3
                    PBRatio.Value = 1.1
                    IPRatio.Value = 1.1
                    QComp.Value = 0.8
                    DeblockA.Value = -2
                    DeblockB.Value = -2
            End Select
        End Sub

        Sub ApplyTuneDefaultValues()
            PsyRD.DefaultValue = 0.3
            PsyRDOQ.DefaultValue = 1
            AQStrength.DefaultValue = 1
            PBRatio.DefaultValue = 1.3
            IPRatio.DefaultValue = 1.4
            QComp.DefaultValue = 0.6
            DeblockA.DefaultValue = 0
            DeblockB.DefaultValue = 0

            Select Case Tune.Value
                Case 3 'grain
                    PsyRD.DefaultValue = 0.5
                    PsyRDOQ.DefaultValue = 30
                    AQStrength.DefaultValue = 0.3
                    PBRatio.DefaultValue = 1.1
                    IPRatio.DefaultValue = 1.1
                    QComp.DefaultValue = 0.8
                    DeblockA.DefaultValue = -2
                    DeblockB.DefaultValue = -2
            End Select
        End Sub

        Public Overrides Function GetPackage() As Package
            Return Packs.x265
        End Function
    End Class

    Public Enum RateMode
        SingleBitrate
        SingleQuant
        SingleCRF
        TwoPass
        ThreePass
    End Enum
End Namespace