Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class NVEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "NVIDIA " + Params.Codec.OptionText
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

        Using f As New CommandLineForm(newParams)
            Dim saveProfileAction = Sub()
                                        Dim enc = ObjectHelp.GetCopy(Of NVEnc)(Me)
                                        Dim params2 As New EncoderParams
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            f.cms.Items.Add(New ActionMenuItem("Check Hardware", Sub() MsgInfo(ProcessHelp.GetStdOut(Package.NVEnc.Path, "--check-hw"))))
            f.cms.Items.Add(New ActionMenuItem("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetStdOut(Package.NVEnc.Path, "--check-features"))))
            f.cms.Items.Add(New ActionMenuItem("Check Environment", Sub() g.ShowCode("Check Environment", ProcessHelp.GetErrOut(Package.NVEnc.Path, "--check-environment"))))
            ActionMenuItem.Add(f.cms.Items, "Save Profile...", saveProfileAction).SetImage(Symbol.Save)

            If f.ShowDialog() = DialogResult.OK Then
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
            If Not codecs?.ToLower.Contains("hevc") Then Throw New ErrorAbortException("NVEncC Error", "H.265/HEVC isn't supported by the graphics card.")
        End If

        p.Script.Synchronize()
        Dim batchPath = p.TempDir + p.TargetFile.Base + "_NVEncC.bat"
        Dim batchCode = Proc.WriteBatchFile(batchPath, Params.GetCommandLine(True, True))

        Using proc As New Proc
            proc.Init("Encoding using NVEncC " + Package.NVEnc.Version)
            proc.SkipStrings = {"%]", " frames: "}
            proc.WriteLine(batchCode + BR2)
            proc.File = "cmd.exe"
            proc.Arguments = "/C call """ + batchPath + """"
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

    Class EncoderParams
        Inherits CommandLineParams

        Sub New()
            Title = "NVIDIA Encoding Options"
        End Sub

        Property Decoder As New OptionParam With {
            .Text = "Decoder:",
            .Options = {"AviSynth/VapourSynth",
                        "NVEncC (avcuvid native)",
                        "NVEncC (avcuvid cuda)",
                        "QSVEncC (Intel)",
                        "ffmpeg (Intel)",
                        "ffmpeg (DXVA2)"},
            .Values = {"avs", "nvnative", "nvcuda", "qs", "ffqsv", "ffdxva"}}

        Property Mode As New OptionParam With {
            .Text = "Mode:",
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
            .Text = "Codec:",
            .Options = {"H.264", "H.265"},
            .Values = {"h264", "h265"}}

        Property Profile As New OptionParam With {
            .Switch = "--profile",
            .Text = "Profile:",
            .VisibleFunc = Function() Codec.ValueText = "h264",
            .Options = {"Baseline", "Main", "High", "High 444"},
            .Convert = True,
            .InitValue = 2}

        Property QPI As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP I:",
            .InitValue = 20,
            .VisibleFunc = Function() Mode.Value = 0,
            .MinMaxStep = {0, 51, 1}}

        Property QPP As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP P:",
            .InitValue = 23,
            .VisibleFunc = Function() Mode.Value = 0,
            .MinMaxStep = {0, 51, 1}}

        Property QPB As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP B:",
            .InitValue = 25,
            .VisibleFunc = Function() Mode.Value = 0,
            .MinMaxStep = {0, 51, 1}}

        Property Lossless As New BoolParam With {
            .Switch = "--lossless",
            .Text = "Lossless",
            .VisibleFunc = Function() Codec.ValueText = "h264" AndAlso Profile.Visible,
            .Value = False,
            .DefaultValue = False}

        Property KNN As New BoolParam With {
            .Switch = "--vpp-knn",
            .Text = "Denoise using K-nearest neighbor",
            .ArgsFunc = AddressOf GetKnnArgs}

        Property KnnRadius As New NumParam With {
            .Text = "      Radius:",
            .InitValue = 3}

        Property KnnStrength As New NumParam With {
            .Text = "      Strength:",
            .InitValue = 0.08,
            .MinMaxStepDec = {0, 1, 0.02D, 2}}

        Property KnnLerp As New NumParam With {
            .Text = "      Lerp:",
            .InitValue = 0.2,
            .MinMaxStepDec = {0, Integer.MaxValue, 0.1D, 1}}

        Property KnnThLerp As New NumParam With {
            .Text = "      TH Lerp:",
            .InitValue = 0.8,
            .MinMaxStepDec = {0, 1, 0.1D, 1}}

        Property PMD As New BoolParam With {
            .Switch = "--vpp-pmd",
            .Text = "Denoise using PMD",
            .ArgsFunc = AddressOf GetPmdArgs}

        Property PmdApplyCount As New NumParam With {
            .Text = "      Apply Count:",
            .InitValue = 2}

        Property PmdStrength As New NumParam With {
            .Text = "      Strength:",
            .Name = "PmdStrength",
            .InitValue = 100.0,
            .MinMaxStepDec = {0, 100, 1, 1}}

        Property PmdThreshold As New NumParam With {
            .Text = "      Threshold:",
            .InitValue = 100.0,
            .MinMaxStepDec = {0, 255, 1, 1}}

        Property Deband As New BoolParam With {.Text = "Deband", .ArgsFunc = AddressOf GetDebandArgs}

        Property Deband_range As New NumParam With {.Text = "range", .InitValue = 15, .MinMaxStep = {0, 127, 1}, .Help = "Set range (default=15, 0-127)"}
        Property Deband_sample As New NumParam With {.Text = "sample", .InitValue = 1, .MinMaxStep = {0, 2, 1}, .Help = "Set sample (default=1, 0-2)"}
        Property Deband_thre As New NumParam With {.Text = "thre", .InitValue = 15, .MinMaxStep = {0, 31, 1}, .Help = "Set threshold for y, cb & cr"}
        Property Deband_thre_y As New NumParam With {.Text = "thre_y", .InitValue = 15, .MinMaxStep = {0, 31, 1}, .Help = "Set threshold for y (default=15, 0-31)"}
        Property Deband_thre_cb As New NumParam With {.Text = "thre_cb", .InitValue = 15, .MinMaxStep = {0, 31, 1}, .Help = "Set threshold for cb (default=15, 0-31)"}
        Property Deband_thre_cr As New NumParam With {.Text = "thre_cr", .InitValue = 15, .MinMaxStep = {0, 31, 1}, .Help = "Set threshold for cr (default=15, 0-31)"}
        Property Deband_dither As New NumParam With {.Text = "dither", .InitValue = 15, .MinMaxStep = {0, 31, 1}, .Help = "Set strength of dither for y, cb & cr"}
        Property Deband_dither_y As New NumParam With {.Text = "dither_y", .InitValue = 15, .MinMaxStep = {0, 31, 1}, .Help = "Set strength of dither for y (default=15, 0-31)"}
        Property Deband_dither_c As New NumParam With {.Text = "dither_c", .InitValue = 15, .MinMaxStep = {0, 31, 1}, .Help = "Set strength of dither for cb/cr (default=15, 0-31)"}
        Property Deband_seed As New NumParam With {.Text = "seed", .InitValue = 1234, .Help = "Set rand seed (default=1234)"}

        Property Deband_blurfirst As New BoolParam With {.Text = "blurfirst"}
        Property Deband_rand_each_frame As New BoolParam With {.Text = "rand_each_frame"}

        Property Custom As New StringParam With {.Text = "Custom:"}

        Function GetDebandArgs() As String
            Dim ret As String
            If Deband.Value Then ret = "--vpp-deband"
            Dim pairs As New List(Of String)

            For Each i In {Deband_range, Deband_sample, Deband_thre, Deband_thre_y, Deband_thre_cb,
                Deband_thre_cr, Deband_dither, Deband_dither_y, Deband_dither_c, Deband_seed}

                If i.Value <> i.DefaultValue Then pairs.Add(i.Text + "=" & i.Value)
            Next

            If Deband_blurfirst.Value Then pairs.Add("blurfirst")
            If Deband_rand_each_frame.Value Then pairs.Add("rand_each_frame")

            If pairs.Count > 0 Then ret += " " + String.Join(",", pairs)
            Return ret
        End Function

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)
                    Add("Basic", Mode, Decoder, Codec,
                        New OptionParam With {.Switch = "--preset", .Text = "Preset:", .Convert = True, .Value = 1, .Options = {"Default", "Quality", "Performance"}},
                        Profile,
                        New OptionParam With {.Name = "LevelH264", .Switch = "--level", .Text = "Level:", .VisibleFunc = Function() Codec.ValueText = "h264", .Options = {"Unrestricted", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                        New OptionParam With {.Name = "LevelH265", .Switch = "--level", .Text = "Level:", .VisibleFunc = Function() Codec.ValueText = "h265", .Options = {"Unrestricted", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}},
                        New OptionParam With {.Switch = "--output-depth", .Text = "Depth:", .Options = {"8-Bit", "10-Bit"}, .Values = {"8", "10"}},
                        QPI, QPP, QPB)
                    Add("Analysis",
                        New OptionParam With {.Switch = "--adapt-transform", .Text = "Adaptive Transform:", .Options = {"Automatic", "Enabled", "Disabled"}, .Values = {"", "--adapt-transform", "--no-adapt-transform"}, .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New NumParam With {.Switch = "--cu-min", .Text = "Min CU Size:", .MinMaxStep = {0, 32, 16}},
                        New NumParam With {.Switch = "--cu-max", .Text = "Max CU Size:", .MinMaxStep = {0, 64, 16}},
                        New BoolParam With {.Switch = "--weightp", .Text = "Enable weighted prediction in P slices"})
                    Add("Slice Decision",
                        New OptionParam With {.Switch = "--direct", .Text = "B-Direct Mode:", .Options = {"automatic", "none", "spatial", "temporal"}, .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New NumParam With {.Switch = "--bframes", .Text = "B-Frames:", .InitValue = 3, .MinMaxStep = {0, 16, 1}},
                        New NumParam With {.Switch = "--ref", .Text = "Ref Frames:", .InitValue = 3, .MinMaxStep = {0, 16, 1}},
                        New NumParam With {.Switch = "--gop-len", .Text = "GOP Length:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--lookahead", .Text = "Lookahead:", .MinMaxStep = {0, 32, 1}},
                        New BoolParam With {.Switch = "--strict-gop", .Text = "Strict GOP"},
                        New BoolParam With {.NoSwitch = "--no-b-adapt", .Text = "B-Adapt", .InitValue = True},
                        New BoolParam With {.NoSwitch = "--no-i-adapt", .Text = "I-Adapt", .InitValue = True})
                    Add("Rate Control",
                        New NumParam With {.Switch = "--qp-init", .Text = "Initial QP:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-max", .Text = "Max QP:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-min", .Text = "Min QP:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--max-bitrate", .Text = "Max Bitrate:", .InitValue = 17500, .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--vbv-bufsize", .Text = "VBV Bufsize:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--aq-strength", .Text = "AQ Strength:", .MinMaxStep = {0, 15, 1}, .VisibleFunc = Function() Codec.ValueText = "h264", .Help = "AQ strength (weak 1 - 15 strong) FOR H.264 ONLY, Default: auto(= 0)"},
                        New NumParam With {.Switch = "--vbr-quality", .Text = "VBR Quality:", .MinMaxStepDec = {0, 51, 1, 1}, .Help = "Target quality for VBR mode (0.0-51.0, 0 = auto)."},
                        New BoolParam With {.Switch = "--aq", .Text = "Adaptive Quantization"},
                        New BoolParam With {.Switch = "--aq-temporal", .Text = "AQ Temporal"},
                        Lossless)
                    Add("Performance",
                        New OptionParam With {.Switch = "--output-buf", .Text = "Output Buffer:", .Options = {"8", "16", "32", "64", "128"}},
                        New OptionParam With {.Switch = "--output-thread", .Text = "Output Thread:", .Options = {"Automatic", "Disabled", "One Thread"}, .Values = {"-1", "0", "1"}},
                        New BoolParam With {.Switch = "--max-procfps", .Text = "Limit performance to lower resource usage"})
                    Add("VUI",
                        New StringParam With {.Switch = "--sar", .Text = "Sample Aspect Ratio:", .InitValue = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                        New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat:", .Options = {"undef", "ntsc", "component", "pal", "secam", "mac"}},
                        New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix:", .Options = {"undef", "auto", "bt709", "smpte170m", "bt470bg", "smpte240m", "YCgCo", "fcc", "GBR", "bt2020nc", "bt2020c"}},
                        New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim:", .Options = {"undef", "auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "film", "bt2020"}},
                        New OptionParam With {.Switch = "--transfer", .Text = "Transfer:", .Options = {"undef", "auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "linear", "log100", "log316", "iec61966-2-4", "bt1361e", "iec61966-2-1", "bt2020-10", "bt2020-12", "smpte-st-2084", "smpte-st-428", "arib-srd-b67"}},
                        New BoolParam With {.Switch = "--fullrange", .Text = "Full Range", .VisibleFunc = Function() Codec.ValueText = "h264"})
                    Add("VPP",
                        New StringParam With {.Switch = "--vpp-deband", .Text = "Deband Filter", .Help = "[<param1>=<value>][,<param2>=<value>][...]"},
                        New OptionParam With {.Switch = "--vpp-resize", .Text = "Resize:", .Options = {"Disabled", "default", "bilinear", "cubic", "cubic_b05c03", "cubic_bspline", "cubic_catmull", "lanczos", "nn", "npp_linear", "spline36", "super"}},
                        New OptionParam With {.Switch = "--vpp-deinterlace", .Text = "Deinterlace:", .VisibleFunc = Function() Decoder.ValueText = "nvnative" OrElse Decoder.ValueText = "nvcuda", .Options = {"none", "adaptive", "bob"}},
                        New OptionParam With {.Switch = "--vpp-gauss", .Text = "Gauss:", .Options = {"Disabled", "3", "5", "7"}},
                        KNN, KnnRadius, KnnStrength, KnnLerp, KnnThLerp,
                        PMD, PmdApplyCount, PmdStrength, PmdThreshold)
                    Add("VPP | Deband",
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
                    Add("Other",
                        New OptionParam With {.Switch = "--mv-precision", .Text = "MV Precision:", .Options = {"auto", "q-pel", "half-pel", "full-pel"}},
                        New OptionParam With {.Switches = {"--cabac", "--cavlc"}, .Text = "Cabac/Cavlc:", .Options = {"Disabled", "Cabac", "Cavlc"}, .Values = {"", "--cabac", "--cavlc"}},
                        New OptionParam With {.Switch = "--interlaced", .Switches = {"--tff", "--bff"}, .Text = "Interlaced:", .VisibleFunc = Function() Codec.ValueText = "h264", .Options = {"Progressive ", "Top Field First", "Bottom Field First"}, .Values = {"", "--tff", "--bff"}},
                        New OptionParam With {.Switch = "--log-level", .Text = "Log Level:", .Options = {"info", "debug", "warn", "error"}},
                        New NumParam With {.Switch = "--device", .Text = "Device:", .MinMaxStep = {0, 4, 1}},
                        New BoolParam With {.Switch = "--deblock", .NoSwitch = "--no-deblock", .Text = "Deblock", .InitValue = True},
                        New BoolParam With {.Switch = "--bluray", .Text = "Blu-ray"},
                        Custom)
                End If

                Return ItemsValue
            End Get
        End Property

        Protected Overrides Sub OnValueChanged(item As CommandLineParam)
            KnnRadius.NumEdit.Enabled = KNN.Value
            KnnStrength.NumEdit.Enabled = KNN.Value
            KnnLerp.NumEdit.Enabled = KNN.Value
            KnnThLerp.NumEdit.Enabled = KNN.Value

            PmdApplyCount.NumEdit.Enabled = PMD.Value
            PmdStrength.NumEdit.Enabled = PMD.Value
            PmdThreshold.NumEdit.Enabled = PMD.Value

            MyBase.OnValueChanged(item)
        End Sub

        Function GetPmdArgs() As String
            If PMD.Value Then
                Dim ret = ""

                If PmdApplyCount.Value <> PmdApplyCount.DefaultValue Then
                    ret += ",apply_count=" & PmdApplyCount.Value
                End If

                If PmdStrength.Value <> PmdStrength.DefaultValue Then
                    ret += ",strength=" & PmdStrength.Value
                End If

                If PmdThreshold.Value <> PmdThreshold.DefaultValue Then
                    ret += ",threshold=" & PmdThreshold.Value
                End If

                If ret <> "" Then
                    Return "--vpp-pmd " + ret.TrimStart(","c)
                Else
                    Return "--vpp-pmd"
                End If
            End If
        End Function

        Function GetKnnArgs() As String
            If KNN.Value Then
                Dim ret = ""

                If KnnRadius.Value <> KnnRadius.DefaultValue Then
                    ret += ",radius=" & KnnRadius.Value
                End If

                If KnnStrength.Value <> KnnStrength.DefaultValue Then
                    ret += ",strength=" & KnnStrength.Value.ToInvariantString
                End If

                If KnnLerp.Value <> KnnLerp.DefaultValue Then
                    ret += ",lerp=" & KnnLerp.Value.ToInvariantString
                End If

                If KnnThLerp.Value <> KnnThLerp.DefaultValue Then
                    ret += ",th_lerp=" & KnnThLerp.Value.ToInvariantString
                End If

                If ret <> "" Then
                    Return "--vpp-knn " + ret.TrimStart(","c)
                Else
                    Return "--vpp-knn"
                End If
            End If
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
                    ret += " --avcuvid cuda"
                Case "qs"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.QSVEnc.Path.Escape, "QSVEncC") + " -o - -c raw" + " -i " + If(includePaths, p.SourceFile.Escape, "path") + " | " + If(includePaths, Package.NVEnc.Path.Escape, "NVEncC")
                Case "ffdxva"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.SourceFile.Escape, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error -hide_banner - | " + If(includePaths, Package.NVEnc.Path.Escape, "NVEncC")
                Case "ffqsv"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.SourceFile.Escape, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error -hide_banner - | " + If(includePaths, Package.NVEnc.Path.Escape, "NVEncC")
            End Select

            Dim q = From i In Items Where i.GetArgs <> ""
            If q.Count > 0 Then ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")

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
            If includePaths Then ret += " -i " + sourcePath.Escape + " -o " + targetPath.Escape

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.NVEnc
        End Function
    End Class
End Class