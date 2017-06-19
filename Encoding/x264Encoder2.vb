Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class x264Encoder2
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Sub New()
        Name = "x264 (under construction)"
        AutoCompCheckValue = 50
        Params.ApplyValues(True)
        Params.ApplyValues(False)
    End Sub

    <NonSerialized>
    Private ParamsValue As x264Params2

    Property Params As x264Params2
        Get
            If ParamsValue Is Nothing Then
                ParamsValue = New x264Params2
                ParamsValue.Init(ParamsStore)
            End If

            Return ParamsValue
        End Get
        Set(value As x264Params2)
            ParamsValue = value
        End Set
    End Property

    Overrides ReadOnly Property OutputExt As String
        Get
            Return "h264"
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Encode("Encoding video using x264 " + Package.x264.Version, GetArgs(1, p.Script), s.ProcessPriority)

        If Params.Mode.Value = x264RateMode.TwoPass Then
            Encode("Encoding video second pass using x264 " + Package.x264.Version, GetArgs(2, p.Script), s.ProcessPriority)
        ElseIf Params.Mode.Value = x264RateMode.ThreePass Then
            Encode("Encoding video second pass using x264 " + Package.x264.Version, GetArgs(3, p.Script), s.ProcessPriority)
            Encode("Encoding video third pass using x264 " + Package.x264.Version, GetArgs(2, p.Script), s.ProcessPriority)
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(passName As String,
                         batchCode As String,
                         priority As ProcessPriorityClass)

        p.Script.Synchronize()
        Dim batchPath = p.TempDir + p.TargetFile.Base + "_encode.bat"
        batchCode = Proc.WriteBatchFile(batchPath, batchCode)

        Using proc As New Proc
            proc.Init(passName)
            proc.Priority = priority
            proc.SkipStrings = {"kb/s, eta", "%]"}
            proc.WriteLine(batchCode + BR2)
            proc.File = "cmd.exe"
            proc.Arguments = "/C call " + batchPath.Quotes
            proc.Start()
        End Using
    End Sub

    Overrides Sub RunCompCheck()
        If Not g.VerifyRequirements Then Exit Sub
        If Not g.IsValidSource Then Exit Sub

        Dim newParams As New x264Params2
        Dim newStore = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(newStore)

        Dim enc As New x264Encoder2
        enc.Params = newParams
        enc.Params.Mode.Value = x264RateMode.Quality
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
        script.Path = p.TempDir + p.Name + "_CompCheck." + script.FileType
        script.Synchronize()

        Log.WriteLine(BR + script.GetFullScript + BR)

        Dim commandLine = enc.Params.GetArgs(0, script, p.TempDir + p.Name + "_CompCheck." + OutputExt, True, True)

        Try
            Encode("Compressibility Check", commandLine, ProcessPriorityClass.Normal)
        Catch ex As AbortException
            ProcessForm.CloseProcessForm()
            Exit Sub
        Catch ex As Exception
            ProcessForm.CloseProcessForm()
            g.ShowException(ex)
            Exit Sub
        End Try

        Dim bits = (New FileInfo(p.TempDir + p.Name + "_CompCheck." + OutputExt).Length) * 8
        p.Compressibility = (bits / script.GetFrames) / (p.TargetWidth * p.TargetHeight)

        OnAfterCompCheck()
        g.MainForm.Assistant()

        Log.WriteLine("Quality: " & CInt(Calc.GetPercent).ToString() + " %")
        Log.WriteLine("Compressibility: " + p.Compressibility.ToString("f3"))
        Log.Save()

        ProcessForm.CloseProcessForm()
    End Sub

    Overloads Function GetArgs(pass As Integer, script As VideoScript, Optional includePaths As Boolean = True) As String
        Return Params.GetArgs(pass, script, OutputPath.DirAndBase + OutputExtFull, includePaths, True)
    End Function

    Overrides Sub ShowConfigDialog()
        Dim newParams As New x264Params2
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(store)
        newParams.ApplyValues(True)

        Using f As New CommandLineForm(newParams)
            Dim saveProfileAction = Sub()
                                        Dim enc = ObjectHelp.GetCopy(Of x264Encoder2)(Me)
                                        Dim params2 As New x264Params2
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            ActionMenuItem.Add(f.cms.Items, "Save Profile...", saveProfileAction).SetImage(Symbol.Save)

            If f.ShowDialog() = DialogResult.OK Then
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides Property QualityMode() As Boolean
        Get
            Return Params.Mode.Value = x264RateMode.Quantizer OrElse Params.Mode.Value = x264RateMode.Quality
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
        Return New x264Control2(Me) With {.Dock = DockStyle.Fill}
    End Function
End Class

Public Class x264Params2
    Inherits CommandLineParams

    Sub New()
        Title = "x264 Options"
    End Sub

    Property Quant As New NumParam With {
        .Switch = "--crf",
        .Switches = {"--qp"},
        .Name = "Quant",
        .Text = "Quality:",
        .Path = "Basic",
        .Help = "--crf <float> Quality-based VBR (0-51) [23.0]" + BR + "--qp <integer> Force constant QP (0-69, 0=lossless)",
        .ArgsFunc = Function() Nothing,
        .Value = 20,
        .DefaultValue = -1,
        .MinMaxStepDec = {0D, 69D, 1D, 1D}}

    Property Preset As New OptionParam With {
        .Switch = "--preset",
        .Text = "Preset:",
        .Path = "Basic",
        .Options = {"Ultra Fast", "Super Fast", "Very Fast", "Faster", "Fast", "Medium", "Slow", "Slower", "Very Slow", "Placebo"},
        .Convert = True,
        .InitValue = 5}

    Property Tune As New OptionParam With {
        .Switch = "--tune",
        .Text = "Tune:",
        .Path = "Basic",
        .Options = {"None", "Film", "Animation", "Grain", "Still Image", "PSNR", "SSIM", "Fast Decode", "Zero Latency"},
        .Convert = True}

    Property Mode As New OptionParam With {
        .Name = "Mode",
        .Text = "Mode:",
        .Path = "Basic",
        .Switches = {"--bitrate", "--qp", "--crf", "--pass"},
        .Options = {"Bitrate", "Quantizer", "Quality", "Two Pass", "Three Pass"},
        .Value = 2}

    Property CompCheck As New NumParam With {
        .Name = "CompCheckQuant",
        .Text = "Comp. Check:",
        .Path = "Other",
        .Value = 18,
        .MinMaxStep = {1, 50, 1}}

    Property Custom As New StringParam With {
        .Text = "Custom:",
        .Path = "Custom",
        .InitAction = Sub(tb)
                          tb.Edit.Expandet = True
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = New Font("Consolas", 10 * s.UIScaleFactor)
                      End Sub}

    Property CustomFirstPass As New StringParam With {
        .Text = "Custom" + BR + "First Pass:",
        .Path = "Custom",
        .ArgsFunc = Function() Nothing,
        .InitAction = Sub(tb)
                          tb.Edit.Expandet = True
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = New Font("Consolas", 10 * s.UIScaleFactor)
                      End Sub}

    Property CustomSecondPass As New StringParam With {
        .Text = "Custom" + BR + "Second Pass:",
        .Path = "Custom",
        .ArgsFunc = Function() Nothing,
        .InitAction = Sub(tb)
                          tb.Edit.Expandet = True
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = New Font("Consolas", 10 * s.UIScaleFactor)
                      End Sub}

    Property Deblock As New BoolParam With {
        .Switch = "--deblock",
        .Text = "Deblocking",
        .Path = "Other",
        .Help = "Loop filter parameters [0:0].",
        .ArgsFunc = AddressOf GetDeblockArgs}

    Property DeblockA As New NumParam With {
        .Text = "      Strength:",
        .Path = "Other",
        .MinMaxStep = {-6, 6, 1}}

    Property DeblockB As New NumParam With {
        .Text = "      Threshold:",
        .Path = "Other",
        .MinMaxStep = {-6, 6, 1}}

    Property _8x8dct As New BoolParam With {
        .Switch = "--8x8dct",
        .NoSwitch = "--no-8x8dct",
        .Text = "8x8dct",
        .Path = "Analysis"}

    Property BFrames As New NumParam With {
        .Switch = "--bframes",
        .Text = "B-Frames:",
        .Path = "Slice Decision",
        .MinMaxStep = {0, 16, 1},
        .Help = "Number of B-frames between I And P [3]."}

    Property AqMode As New OptionParam With {
        .Switch = "--aq-mode",
        .Text = "AQ Mode:",
        .Path = "Rate Control",
        .IntegerValue = True,
        .Expand = True,
        .Options = {"Disabled", "Variance AQ", "Auto-variance AQ", "Auto-variance AQ with bias to dark scenes"}}

    Property BAdapt As New OptionParam With {
        .Switch = "--b-adapt",
        .Text = "B-Adapt:",
        .Path = "Slice Decision",
        .IntegerValue = True,
        .Options = {"Disabled", "Fast", "Optimal"},
        .Help = "Adaptive B-frame decision method [1]"}

    Property Cabac As New BoolParam With {
        .Path = "Other",
        .NoSwitch = "--no-cabac",
        .Text = "Cabac"}

    Property weightp As New OptionParam With {
        .Path = "Analysis",
        .Switch = "--weightp",
        .Text = "Weight P Prediction",
        .Expand = True,
        .Options = {"Disabled", "Weighted refs", "Weighted refs + Duplicates"},
        .IntegerValue = True}

    Property profile As New OptionParam With {
        .Path = "Basic",
        .Switch = "--profile",
        .Text = "Profile:",
        .Help = "Force the limits of an H.264 profile.",
        .Convert = True,
        .Options = {"Unrestricted", "Baseline", "Main", "High", "High10", "High422", "High444"}}

    Property CQM As New OptionParam With {
        .Path = "Analysis",
        .Switch = "--cqm",
        .Options = {"flat", "jvt"},
        .Text = "Quant Matrice Preset"}

    Sub ApplyValues(isDefault As Boolean)
        Dim setVal = Sub(param As CommandLineParam, value As Object)
                         If TypeOf param Is BoolParam Then
                             If isDefault Then
                                 DirectCast(param, BoolParam).DefaultValue = CBool(value)
                             Else
                                 DirectCast(param, BoolParam).Value = CBool(value)
                             End If
                         ElseIf TypeOf param Is NumParam Then
                             If isDefault Then
                                 DirectCast(param, NumParam).DefaultValue = CSng(value)
                             Else
                                 DirectCast(param, NumParam).Value = CSng(value)
                             End If
                         ElseIf TypeOf param Is OptionParam Then
                             If isDefault Then
                                 DirectCast(param, OptionParam).DefaultValue = CInt(value)
                             Else
                                 DirectCast(param, OptionParam).Value = CInt(value)
                             End If
                         End If
                     End Sub

        setVal(Deblock, True)
        setVal(DeblockA, 0)
        setVal(DeblockB, 0)
        setVal(_8x8dct, True)
        setVal(BFrames, 3)
        setVal(AqMode, 1)
        setVal(BAdapt, 1)
        setVal(Cabac, True)
        setVal(weightp, 2)

        Select Case Preset.Value
            Case 0 'ultrafast
                setVal(Deblock, False)
                setVal(_8x8dct, False)
                setVal(BFrames, 0)
                setVal(AqMode, 0)
                setVal(BAdapt, 0)
                setVal(Cabac, False)
                setVal(weightp, 0)
            Case 1 'superfast
                setVal(weightp, 1)
            Case 2 'veryfast
                setVal(weightp, 1)
            Case 3 'faster
                setVal(weightp, 1)
            Case 4 'fast
                setVal(weightp, 1)
            Case 5 'medium

            Case 6 'slow

            Case 7 'slower
                setVal(BAdapt, 2)
            Case 8 'veryslow
                setVal(BFrames, 8)
                setVal(BAdapt, 2)
            Case 9 'placebo
                setVal(BFrames, 16)
                setVal(BAdapt, 2)
        End Select

        Select Case Tune.Value
            Case 1 'film
                setVal(DeblockA, -1)
                setVal(DeblockB, -1)
            Case 2 'animation
                setVal(DeblockA, 1)
                setVal(DeblockB, 1)
                Dim val = BFrames.Value
                val += 2
                setVal(BFrames, If(val > 16, 16, val))
            Case 3 'grain
                setVal(DeblockA, -2)
                setVal(DeblockB, -2)
            Case 4 'stillimage
                setVal(DeblockA, -3)
                setVal(DeblockB, -3)
            Case 5 'psnr
                setVal(AqMode, 0)
            Case 6 'ssim
                setVal(AqMode, 2)
            Case 7 'fastdecode
                setVal(Deblock, False)
                setVal(Cabac, False)
                setVal(weightp, 0)
            Case 8 'zerolatency
                setVal(BFrames, 0)
        End Select

        Select Case profile.Value
            Case 1 'baseline
                setVal(Cabac, False)
                setVal(_8x8dct, False)
                setVal(BFrames, 0)
                setVal(weightp, 0)
        End Select
    End Sub

    Private ItemsValue As List(Of CommandLineParam)

    Overrides ReadOnly Property Items As List(Of CommandLineParam)
        Get
            If ItemsValue Is Nothing Then
                ItemsValue = New List(Of CommandLineParam) From {
                    Quant,
                    Preset,
                    Tune,
                    profile,
                    Mode,
                    weightp,
                    CQM,
                    _8x8dct,
                    AqMode,
                    BAdapt,
                    BFrames,
                    New StringParam With {.Path = "VUI", .Switch = "--sar", .Text = "Sample Aspect Ratio:", .InitValue = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                    Deblock,
                    DeblockA,
                    DeblockB,
                    CompCheck,
                    Cabac,
                    Custom,
                    CustomFirstPass,
                    CustomSecondPass}



                'Add("Basic", Quant, Preset, Tune, Profile, OutputDepth,
                'Add("Custom", Custom, CustomFirstPass, CustomSecondPass)

                'New OptionParam With {.Path = "Basic", .Switch = "--preset", .Text = "Preset", .Help = "Use a preset to select encoding settings", .InitValue = 5, Options = {"ultrafast", "superfast", "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow", "placebo"}},
                'New OptionParam With {.Path = "Basic", .Switch = "--tune", .Text = "Tune", .Help = "Tune the settings for a particular type of source or situation.", Options = {"disabled", film", "animation", "grain", "stillimage", "psnr", "ssim", "fastdecode", "zerolatency"}},
                'New BoolParam   With {.Path = "Basic", .Switch = "--slow-firstpass", .Text = "Slow Firstpass"},
                'New NumParam    With {.Path = "Frame-type", .Switch = "--keyint", .Text = "keyint", Help = "keyint", Help = "Maximum GOP size [250]"},
                'New NumParam    With {.Path = "Frame-type", .Switch = "--min-keyint", .Text = "min-keyint", Help = "Minimum GOP size [auto]"},
                'New NumParam    With {.Path = "Frame-type", .Switch = "--scenecut", .Text = "scenecut", Help = "How aggressively to insert extra I-frames [40]"},
                'New BoolParam   With {.Path = "Frame-type", .Switch = "--intra-refresh", .Text = "Intra Refresh", .Help = "Use Periodic Intra Refresh instead of IDR frames"},
                'New NumParam    With {.Path = "", .Switch = "--b-bias", .Text = "b-bias", Help = "Influences how often B-frames are used [0]"},
                'New StringParam With {.Path = "", .Switch = "--b-pyramid", .Text = "b-pyramid", .Options = {"none", "strict", "normal"}, .Help = "Keep some B-frames as references [normal]"},
                'New BoolParam   With {.Path = "", .Switch = "--open-gop", .Text = "open-gop", .Help = "Use recovery points to close GOPs"},
                'New NumParam    With {.Path = "", .Switch = "--ref", .Text = "ref", Help = "Number of reference frames [3]"},
                'New BoolParam   With {.Path = "", .Switch = "--no-deblock", .Text = "no-deblock", .Help = "Disable loop filter"},
                'New NumParam    With {.Path = "", .Switch = "--slices", .Text = "slices", Help = "Number of slices per frame"},
                'New NumParam    With {.Path = "", .Switch = "--slices-max", .Text = "slices-max", Help = "Absolute maximum slices per frame"},
                'New NumParam    With {.Path = "", .Switch = "--slice-max-size", .Text = "slice-max-size", Help = "Limit the size of each slice in bytes"},
                'New NumParam    With {.Path = "", .Switch = "--slice-max-mbs", .Text = "slice-max-mbs", Help = "Limit the size of each slice in macroblocks (max)"},
                'New NumParam    With {.Path = "", .Switch = "--slice-min-mbs", .Text = "slice-min-mbs", Help = "Limit the size of each slice in macroblocks (min)"},
                'New BoolParam   With {.Path = "", .Switch = "--tff", .Text = "tff", .Help = "Enable interlaced mode (top field first)"},
                'New BoolParam   With {.Path = "", .Switch = "--bff", .Text = "bff", .Help = "Enable interlaced mode (bottom field first)"},
                'New BoolParam   With {.Path = "", .Switch = "--constrained-intra", .Text = "constrained-intra", .Help = "Enable constrained intra prediction."},
                'New StringParam With {.Path = "", .Switch = "--pulldown", .Text = "pulldown", .Help = "Use soft pulldown to change frame rate"},

                '{"none", "22", "32", "64", "double", "triple", "euro"}

                'New BoolParam   With {.Path = "", .Switch = "--fake-interlaced", .Text = "fake-interlaced", .Help = "Flag stream as interlaced but encode progressive."},
                'New NumParam    With {.Path = "", .Switch = "--frame-packing", .Text = "frame-packing", .IntegerValue = True, Help = "For stereoscopic videos define frame arrangement"},

                '{"checkerboard", "column alternation", "row alternation", "side by side", "top bottom", "frame alternation", "mono", "tile format"}

                'Ratecontrol:

                'New NumParam    With {.Path = "", .Switch = "--qp", .Text = "qp", Help = "Force constant QP (0-69, 0=lossless)"},
                'New NumParam    With {.Path = "", .Switch = "--bitrate", .Text = "bitrate", Help = "Set bitrate (kbit/s)"},
                'New NumParam    With {.Path = "", .Switch = "--crf", .Text = "crf", Help = "Quality-based VBR (0-51) [23.0]"},
                'New NumParam    With {.Path = "", .Switch = "--rc-lookahead", .Text = "rc-lookahead", Help = "Number of frames for frametype lookahead [40]"},
                'New NumParam    With {.Path = "", .Switch = "--vbv-maxrate", .Text = "vbv-maxrate", Help = "Max local bitrate (kbit/s) [0]"},
                'New NumParam    With {.Path = "", .Switch = "--vbv-bufsize", .Text = "vbv-bufsize", Help = "Set size of the VBV buffer (kbit) [0]"},
                'New NumParam    With {.Path = "", .Switch = "--vbv-init", .Text = "vbv-init", Help = "Initial VBV buffer occupancy [0.9]"},
                'New NumParam    With {.Path = "", .Switch = "--crf-max", .Text = "crf-max", Help = "With CRF+VBV, limit RF to this value"},
                'New NumParam    With {.Path = "", .Switch = "--qpmin", .Text = "qpmin", Help = "Set min QP [0]"},
                'New NumParam    With {.Path = "", .Switch = "--qpmax", .Text = "qpmax", Help = "Set max QP [69]"},
                'New NumParam    With {.Path = "", .Switch = "--qpstep", .Text = "qpstep", Help = "Set max QP step [4]"},
                'New NumParam    With {.Path = "", .Switch = "--ratetol", .Text = "ratetol", Help = "Tolerance of ABR ratecontrol And VBV [1.0]"},
                'New NumParam    With {.Path = "", .Switch = "--ipratio", .Text = "ipratio", Help = "QP factor between I And P [1.40]"},
                'New NumParam    With {.Path = "", .Switch = "--pbratio", .Text = "pbratio", Help = "QP factor between P And B [1.30]"},
                'New NumParam    With {.Path = "", .Switch = "--chroma-qp-offset", .Text = "chroma-qp-offset", Help = "QP difference between chroma And luma [0]"},
                'New NumParam    With {.Path = "", .Switch = "--aq-strength", .Text = "aq-strength", Help = "Reduces blocking And blurring in flat And textured areas. [1.0]"},
                'New StringParam With {.Path = "", .Switch = "--stats", .Text = "stats", .Help = "Filename for 2 pass stats ["x264_2pass.log"]"},
                'New BoolParam   With {.Path = "", .Switch = "--no-mbtree", .Text = "no-mbtree", .Help = "Disable mb-tree ratecontrol."},
                'New NumParam    With {.Path = "", .Switch = "--qcomp", .Text = "qcomp", Help = "QP curve compression [0.60]"},
                'New NumParam    With {.Path = "", .Switch = "--cplxblur", .Text = "cplxblur", Help = "Reduce fluctuations in QP (before curve compression) [20.0]"},
                'New NumParam    With {.Path = "", .Switch = "--qblur", .Text = "qblur", Help = "Reduce fluctuations in QP (after curve compression) [0.5]"},
                'New StringParam With {.Path = "", .Switch = "--qpfile", .Text = "qpfile", .Help = "Force frametypes And QPs for some Or all frames"},

                'Analysis:

                'New StringParam With {.Path = "", .Switch = "--direct", .Text = "direct", .Help = "Direct MV prediction mode ["spatial"]"},

                '{"none", "spatial", "temporal", "auto"}

                'New BoolParam   With {.Path = "", .Switch = "--no-weightb", .Text = "no-weightb", .Help = "Disable weighted prediction for B-frames"},

                '{"Disabled", "Weighted refs", "Weighted refs + Duplicates"}

                'New StringParam With {.Path = "", .Switch = "--me", .Text = "me", .Help = "Integer pixel motion estimation method ["hex"]"},

                '{"dia", "hex", "umh", "esa", "tesa"}

                'New NumParam    With {.Path = "", .Switch = "--merange", .Text = "merange", Help = "Maximum motion vector search range [16]"},
                'New NumParam    With {.Path = "", .Switch = "--mvrange", .Text = "mvrange", Help = "Maximum motion vector length [-1 (auto)]"},
                'New NumParam    With {.Path = "", .Switch = "--mvrange-thread", .Text = "mvrange-thread", Help = "Minimum buffer between threads [-1 (auto)]"},
                'New NumParam    With {.Path = "", .Switch = "--subme", .Text = "subme", .IntegerValue = True, Help = "Subpixel motion estimation And mode decision [7]"},

                '{"fullpel only (Not recommended)", "SAD mode decision, one qpel iteration", "SATD mode decision", "Progressively more qpel", "Progressively more qpel", "Progressively more qpel", "RD mode decision for I/P-frames", "RD mode decision for all frames", "RD refinement for I/P-frames", "RD refinement for all frames", "QP-RD - requires trellis=2, aq-mode>0", "Full RD disable all early terminations"}

                'New <float:float>Param With {.Path = "", .Switch = "--psy-rd", .Text = "Psy RD", .Help = "Strength of psychovisual optimization ["1.0:0.0"]"

                '{"RD (requires subme>=6)", "Trellis (requires trellis)"}

                'New BoolParam   With {.Path = "", .Switch = "--no-psy", .Text = "no-psy", .Help = "Disable all visual optimizations that worsen"},
                '                              both PSNR and SSIM.
                'New BoolParam   With {.Path = "", .Switch = "--no-mixed-refs", .Text = "no-mixed-refs", .Help = "Don't decide references on a per partition basis"},
                'New BoolParam   With {.Path = "", .Switch = "--no-chroma-me", .Text = "no-chroma-me", .Help = "Ignore chroma in motion estimation"},
                'New BoolParam   With {.Path = "", .Switch = "--no-8x8dct", .Text = "no-8x8dct", .Help = "Disable adaptive spatial transform size"},
                'New NumParam    With {.Path = "", .Switch = "--trellis", .Text = "trellis", .IntegerValue = True, Help = "Trellis RD quantization. [1]"},

                '{"Disabled", "Final MB", "Always"}

                'New BoolParam   With {.Path = "", .Switch = "--no-fast-pskip", .Text = "no-fast-pskip", .Help = "Disables early SKIP detection on P-frames"},
                'New BoolParam   With {.Path = "", .Switch = "--no-dct-decimate", .Text = "no-dct-decimate", .Help = "Disables coefficient thresholding on P-frames"},
                'New NumParam    With {.Path = "", .Switch = "--nr", .Text = "nr", Help = "Noise reduction [0]"},
                'New NumParam    With {.Path = "", .Switch = "--deadzone-inter", .Text = "deadzone-inter", .MinMaxStep = {0, 32 , 1}, Help = "Set the size of the inter luma quantization deadzone [21]"},
                'New NumParam    With {.Path = "", .Switch = "--deadzone-intra", .Text = "deadzone-intra", .MinMaxStep = {0, 32 , 1}, Help = "Set the size of the intra luma quantization deadzone [11]"},

                '{"jvt", "flat"}

                'New StringParam With {.Path = "", .Switch = "--cqmfile", .Text = "cqmfile", .Help = "Read custom quant matrices from a JM-compatible file"},
                'New <list>Param With {.Path = "", .Switch = "--cqm4", .Text = "Set all 4x4 quant matrices"
                'New <list>Param With {.Path = "", .Switch = "--cqm8", .Text = "Set all 8x8 quant matrices"
                'New <list>Param With {.Path = "", .Switch = "--cqm4i, --cqm4p, --cqm8i, --cqm8p", .Text = "Set both luma and chroma quant matrices"
                'New <list>Param With {.Path = "", .Switch = "--cqm4iy, --cqm4ic, --cqm4py, --cqm4pc", .Text = "Set individual quant matrices"

                'Video Usability Info (Annex E)
                'The VUI settings are Not used by the encoder but are merely suggestions to
                'the playback equipment. See doc/vui.txt for details. Use at your own risk.

                'New StringParam With {.Path = "", .Switch = "--overscan", .Text = "overscan", .Help = "Specify crop overscan setting ["undef"]"},

                '{"undef", "show", "crop"}

                'New StringParam With {.Path = "", .Switch = "--videoformat", .Text = "videoformat", .Help = "Specify video format ["undef"]"},

                '{"undef", "component", "pal", "ntsc", "secam", "mac"}

                'New StringParam With {.Path = "", .Switch = "--range", .Text = "range", .Help = "Specify color range ["auto"]"},

                '{"auto", "tv", "pc"}

                'New StringParam With {.Path = "", .Switch = "--colorprim", .Text = "colorprim", .Help = "Specify color primaries ["undef"]"},

                '{"undef", "bt709", "bt470m", "bt470bg", "smpte170m", "smpte240m", "film", "bt2020", "smpte428", "smpte431", "smpte432"}

                'New StringParam With {.Path = "", .Switch = "--transfer", .Text = "transfer", .Help = "Specify transfer characteristics ["undef"]"},

                '{"undef", "bt709", "bt470m", "bt470bg", "smpte170m", "smpte240m", "linear", "log100", "log316", "iec61966-2-4", "bt1361e", "iec61966-2-1", "bt2020-10", "bt2020-12", "smpte2084", "smpte428"}

                'New StringParam With {.Path = "", .Switch = "--colormatrix", .Text = "colormatrix", .Help = "Specify color matrix setting ["???"]"},

                '{"undef", "bt709", "fcc", "bt470bg", "smpte170m", "smpte240m", "GBR", "YCgCo", "bt2020nc", "bt2020c", "smpte2085"}

                'New NumParam    With {.Path = """, ".Switch = "--chromaloc", .Text = "chromaloc", Help = "Specify chroma sample location (0 to 5) [0]"},"
                'New StringParam With {.Path = "", .Switch = "--nal-hrd", .Text = "nal-hrd", .Help = "Signal HRD information (requires vbv-bufsize)"},

                '{"none", "vbr", "cbr"}

                'New BoolParam   With {.Path = "", .Switch = "--filler", .Text = "filler", .Help = "Force hard-CBR And generate filler (implied by"},
                'New BoolParam   With {.Path = "", .Switch = "--pic-struct", .Text = "pic-struct", .Help = "Force pic_struct in Picture Timing SEI"},
                'New StringParam With {.Path = "", .Switch = "--crop-rect", .Text = "crop-rect", .Help = "Add 'left,top,right,bottom' to the bitstream-level cropping rectangle"},

                '[Input/Output]

                'New StringParam With {.Path = "", .Switch = "--output", .Text = "output", .Help = "Specify output file"},
                'New StringParam With {.Path = "", .Switch = "--input-fmt", .Text = "input-fmt", .Help = "Specify input file format (requires lavf support)"},
                'New StringParam With {.Path = "", .Switch = "--input-csp", .Text = "input-csp", .Help = "Specify input colorspace format for raw input"},

                '{"i420", "yv12", "nv12", "nv21", "i422", "yv16", "nv16", "yuyv", "uyvy", "i444", "yv24", "bgr", "bgra", "rgb"}

                'New StringParam With {.Path = "", .Switch = "--output-csp", .Text = "output-csp", .Help = "Specify output colorspace ["i420"]"},

                '{"i420", "i422", "i444", "rgb"}

                'New NumParam    With {.Path = "", .Switch = "--input-depth", .Text = "input-depth", Help = "Specify input bit depth for raw input"},
                'New StringParam With {.Path = "", .Switch = "--input-range", .Text = "input-range", .Help = "Specify input color range ["auto"]"},

                '{"auto", "tv", "pc"}

                'New <intxint>Param With {.Path = "", .Switch = "--input-res", .Text = "Specify input resolution (width x height)"
                'New StringParam With {.Path = "", .Switch = "--index", .Text = "index", .Help = "Filename for input index file"},
                'New <float|rational>Param With {.Path = "", .Switch = "--fps", .Text = "Specify framerate"
                'New NumParam    With {.Path = "", .Switch = "--seek", .Text = "seek", Help = "First frame to encode"},
                'New NumParam    With {.Path = "", .Switch = "--frames", .Text = "frames", Help = "Maximum number of frames to encode"},
                'New StringParam With {.Path = "", .Switch = "--level", .Text = "level", .Help = "Specify level (as defined by Annex A)"},
                'New BoolParam   With {.Path = "", .Switch = "--bluray-compat", .Text = "bluray-compat", .Help = "Enable compatibility hacks for Blu-ray support"},
                'New NumParam    With {.Path = "", .Switch = "--avcintra-class", .Text = "avcintra-class", Help = "Use compatibility hacks for AVC-Intra class"},

                '{"50", "100", "200"}

                'New BoolParam   With {.Path = "", .Switch = "--stitchable", .Text = "stitchable", .Help = "Don't optimize headers based on video content"},
                'New BoolParam   With {.Path = "", .Switch = "--verbose", .Text = "verbose", .Help = "Print stats for each frame"},
                'New BoolParam   With {.Path = "", .Switch = "--no-progress", .Text = "no-progress", .Help = "Don't show the progress indicator while encoding"},
                'New BoolParam   With {.Path = "", .Switch = "--quiet", .Text = "quiet", .Help = "Quiet Mode"},
                'New StringParam With {.Path = "", .Switch = "--log-level", .Text = "log-level", .Help = "Specify the maximum level of logging ["info"]"},

                '{"none", "error", "warning", "info", "debug"}

                'New BoolParam   With {.Path = "", .Switch = "--psnr", .Text = "psnr", .Help = "Enable PSNR computation"},
                'New BoolParam   With {.Path = "", .Switch = "--ssim", .Text = "ssim", .Help = "Enable SSIM computation"},
                'New NumParam    With {.Path = "", .Switch = "--threads", .Text = "threads", Help = "Force a specific number of threads"},
                'New NumParam    With {.Path = "", .Switch = "--lookahead-threads", .Text = "lookahead-threads", Help = "Force a specific number of lookahead threads"},
                'New BoolParam   With {.Path = "", .Switch = "--sliced-threads", .Text = "sliced-threads", .Help = "Low-latency but lower-efficiency threading"},
                'New BoolParam   With {.Path = "", .Switch = "--thread-input", .Text = "thread-input", .Help = "Run Avisynth in its own thread"},
                'New NumParam    With {.Path = "", .Switch = "--sync-lookahead", .Text = "sync-lookahead", Help = "Number of buffer frames for threaded lookahead"},
                'New BoolParam   With {.Path = "", .Switch = "--non-deterministic", .Text = "non-deterministic", .Help = "Slightly improve quality of SMP, at the cost of repeatability"},
                'New BoolParam   With {.Path = "", .Switch = "--cpu-independent", .Text = "cpu-independent", .Help = "Ensure exact reproducibility across different cpus, as opposed to letting them select different algorithms"},
                'New NumParam    With {.Path = "", .Switch = "--asm", .Text = "asm", Help = "Override CPU detection"},
                'New BoolParam   With {.Path = "", .Switch = "--no-asm", .Text = "no-asm", .Help = "Disable all CPU optimizations"},
                'New BoolParam   With {.Path = "", .Switch = "--opencl", .Text = "opencl", .Help = "Enable use of OpenCL"},
                'New StringParam With {.Path = "", .Switch = "--opencl-clbin", .Text = "opencl-clbin", .Help = "Specify path of compiled OpenCL kernel cache"},
                'New NumParam    With {.Path = "", .Switch = "--opencl-device", .Text = "opencl-device", Help = "Specify OpenCL device ordinal"},
                'New StringParam With {.Path = "", .Switch = "--dump-yuv", .Text = "dump-yuv", .Help = "Save reconstructed frames"},
                'New NumParam    With {.Path = "", .Switch = "--sps-id", .Text = "sps-id", Help = "Set SPS and PPS id numbers [0]"},
                'New BoolParam   With {.Path = "", .Switch = "--aud", .Text = "aud", .Help = "Use access unit delimiters"},
                'New BoolParam   With {.Path = "", .Switch = "--force-cfr", .Text = "force-cfr", .Help = "Force constant framerate timestamp generation"},
                'New StringParam With {.Path = "", .Switch = "--tcfile-in", .Text = "tcfile-in", .Help = "Force timestamp generation with timecode file"},
                'New StringParam With {.Path = "", .Switch = "--tcfile-out", .Text = "tcfile-out", .Help = "Output timecode v2 file from input timestamps"},
                'New <int/int>Param With {.Path = "", .Switch = "--timebase", .Text = "Specify timebase numerator and denominator"
                '                 <integer>    Specify timebase numerator For input timecode file
                '                              Or specify timebase denominator for other input
                'New BoolParam   With {.Path = "", .Switch = "--dts-compress", .Text = "dts-compress", .Help = "Eliminate initial delay with container DTS hack"},

                'New <alphabeta> Param With {.Path = "", .Switch = "--deblock", .Text = "Loop filter parameters [0:0]"

                'New StringParam With {.Path = "", .Switch = "--partitions", .Text = "partitions", .Help = "Partitions to consider ["p8x8,b8x8,i8x8,i4x4"]"},
                '                                  - p8x8, p4x4, b8x8, i8x8, i4x4
                '                                  - none, all
                '                                  (p4x4 requires p8x8. i8x8 requires --8x8dct.)

            End If

            Return ItemsValue
        End Get
    End Property

    Private BlockValueChanged As Boolean

    Protected Overrides Sub OnValueChanged(item As CommandLineParam)
        If BlockValueChanged Then Exit Sub

        If item Is Preset OrElse item Is Tune OrElse item Is Profile Then
            BlockValueChanged = True
            ApplyValues(False)
            BlockValueChanged = False
        End If

        DeblockA.NumEdit.Enabled = Deblock.Value
        DeblockB.NumEdit.Enabled = Deblock.Value

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
        ApplyValues(True)

        Dim args As String
        Dim sourcePath As String

        If includePaths AndAlso includeExecutable Then
            If p.Script.Engine = ScriptEngine.VapourSynth Then
                args += Package.vspipe.Path.Quotes + " " + script.Path.Quotes + " - --y4m | " + Package.x264.Path.Quotes
                sourcePath = "-"
            Else
                args += Package.x264.Path.Quotes
                sourcePath = script.Path
            End If
        End If

        If Mode.Value = x264RateMode.TwoPass OrElse Mode.Value = x264RateMode.ThreePass Then
            args += " --pass " & pass

            If pass = 1 Then
                If CustomFirstPass.Value <> "" Then args += " " + CustomFirstPass.Value
            Else
                If CustomSecondPass.Value <> "" Then args += " " + CustomSecondPass.Value
            End If
        End If

        If Mode.Value = x264RateMode.Quantizer Then
            If Not IsCustom(pass, "--qp") Then args += " --qp " + CInt(Quant.Value).ToString
        ElseIf Mode.Value = x264RateMode.Quality Then
            If Not IsCustom(pass, "--crf") Then args += " --crf " + Quant.Value.ToInvariantString
        Else
            If Not IsCustom(pass, "--bitrate") Then args += " --bitrate " & p.VideoBitrate
        End If

        Dim q = From i In Items Where i.GetArgs <> "" AndAlso Not IsCustom(pass, i.Switch)
        If q.Count > 0 Then args += " " + q.Select(Function(item) item.GetArgs).Join(" ")

        If includePaths Then
            If sourcePath = "-" Then args += " --demuxer y4m --frames " & script.GetFrames

            If Mode.Value = x264RateMode.TwoPass OrElse Mode.Value = x264RateMode.ThreePass Then
                args += " --stats " + (p.TempDir + p.Name + ".stats").Quotes
            End If

            If (Mode.Value = x264RateMode.ThreePass AndAlso
                (pass = 1 OrElse pass = 3)) OrElse
                Mode.Value = x264RateMode.TwoPass AndAlso pass = 1 Then

                args += " --output NUL " + sourcePath.Quotes
            Else
                args += " --output " + targetPath.Quotes + " " + sourcePath.Quotes
            End If
        End If

        Return Macro.Expand(args.Trim.FixBreak.Replace(BR, " "))
    End Function

    Function IsCustom(pass As Integer, switch As String) As Boolean
        If switch = "" Then Return False

        If Mode.Value = x264RateMode.TwoPass OrElse Mode.Value = x264RateMode.ThreePass Then
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

    Public Overrides Function GetPackage() As Package
        Return Package.x264
    End Function
End Class

Public Enum x264RateMode
    Bitrate
    Quantizer
    Quality
    <DispName("Two Pass")> TwoPass
    <DispName("Three Pass")> ThreePass
End Enum