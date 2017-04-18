Imports System.Globalization
Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class NVIDIAEncoder
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
                                        Dim enc = ObjectHelp.GetCopy(Of NVIDIAEncoder)(Me)
                                        Dim params2 As New EncoderParams
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            f.cms.Items.Add(New ActionMenuItem("Check Hardware", Sub() MsgInfo(ProcessHelp.GetStdOut(Package.NVEncC.Path, "--check-hw"))))
            f.cms.Items.Add(New ActionMenuItem("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetStdOut(Package.NVEncC.Path, "--check-features"))))
            f.cms.Items.Add(New ActionMenuItem("Check Environment", Sub() g.ShowCode("Check Environment", ProcessHelp.GetErrorOut(Package.NVEncC.Path, "--check-environment"))))
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
        p.Script.Synchronize()
        Dim batchPath = p.TempDir + p.TargetFile.Base + "_NVEncC.bat"
        Dim batchCode = Proc.WriteBatchFile(batchPath, Params.GetCommandLine(True, True))

        Using proc As New Proc
            proc.Init("Encoding using NVEncC " + Package.NVEncC.Version)
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
            Return Params.Mode.OptionText = "CQP"
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
            .Switches = {"--cqp", "--cbr", "--cbrhq", "--vbr", "--vbrhq"},
            .Options = {"CQP", "CBR", "CBRHQ", "VBR", "VBRHQ"},
            .VisibleFunc = Function() Not Lossless.Value,
            .ArgsFunc = Function() As String
                            Select Case Mode.OptionText
                                Case "CBR"
                                    Return "--cbr " & p.VideoBitrate
                                Case "CBRHQ"
                                    Return "--cbrhq " & p.VideoBitrate
                                Case "VBR"
                                    Return "--vbr " & p.VideoBitrate
                                Case "VBRHQ"
                                    Return "--vbrhq " & p.VideoBitrate
                                Case "CQP"
                                    Return "--cqp " & QPI.Value & ":" & QPP.Value & ":" & QPB.Value
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
            .Options = {"baseline", "main", "high", "high444"},
            .InitValue = 2}

        Property QPI As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "Constant QP I:",
            .InitValue = 20,
            .VisibleFunc = Function() "CQP" = Mode.OptionText,
            .MinMaxStep = {0, 51, 1}}

        Property QPP As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "Constant QP P:",
            .InitValue = 23,
            .VisibleFunc = Function() "CQP" = Mode.OptionText,
            .MinMaxStep = {0, 51, 1}}

        Property QPB As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "Constant QP B:",
            .InitValue = 25,
            .VisibleFunc = Function() "CQP" = Mode.OptionText,
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

        Private ItemsValue As List(Of CommandLineParam)

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)
                    Add("Basic", Decoder, Mode, Codec,
                        New OptionParam With {.Switch = "--preset", .Text = "Preset:", .Options = {"default", "quality", "performance"}},
                        New OptionParam With {.Switch = "--output-depth", .Text = "Depth:", .Options = {"8", "10"}},
                        QPI, QPP, QPB)
                    Add("Analysis",
                        New OptionParam With {.Switch = "--adapt-transform", .Text = "Adaptive Transform:", .Options = {"Automatic", "Enabled", "Disabled"}, .Values = {"", "--adapt-transform", "--no-adapt-transform"}, .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New NumParam With {.Switch = "--cu-min", .Text = "Min CU Size:", .MinMaxStep = {0, 32, 16}},
                        New NumParam With {.Switch = "--cu-max", .Text = "Max CU Size:", .MinMaxStep = {0, 64, 16}})
                    Add("Slice Decision",
                        New OptionParam With {.Switch = "--direct", .Text = "B-Direct Mode:", .Options = {"automatic", "none", "spatial", "temporal"}, .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New NumParam With {.Switch = "--bframes", .Text = "B-Frames:", .InitValue = 3, .MinMaxStep = {0, 16, 1}},
                        New NumParam With {.Switch = "--ref", .Text = "Ref Frames:", .InitValue = 3, .MinMaxStep = {0, 16, 1}},
                        New NumParam With {.Switch = "--gop-len", .Text = "GOP Length:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--lookahead", .Text = "Lookahead:", .MinMaxStep = {0, 32, 1}, .InitValue = 16},
                        New BoolParam With {.Switch = "--strict-gop", .Text = "Strict GOP"},
                        New BoolParam With {.NoSwitch = "--no-b-adapt", .Text = "B-Adapt", .InitValue = True},
                        New BoolParam With {.NoSwitch = "--no-i-adapt", .Text = "I-Adapt", .InitValue = True},
                        New BoolParam With {.Switch = "--enable-ltr", .Text = "LTR (Long Term Reference pictures)", .VisibleFunc = Function() Codec.ValueText = "h265"})
                    Add("Rate Control",
                        New NumParam With {.Switch = "--qp-init", .Text = "Initial QP:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-max", .Text = "Max QP:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-min", .Text = "Min QP:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--max-bitrate", .Text = "Max Bitrate:", .InitValue = 17500, .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--vbv-bufsize", .Text = "VBV Bufsize:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--aq-strength", .Text = "AQ Strength:", .MinMaxStep = {0, 15, 1}, .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New NumParam With {.Switch = "--vbr-quality", .Text = "VBR Quality:", .MinMaxStep = {0, 51, 1}},
                        New BoolParam With {.Switch = "--aq", .Text = "Adaptive Quantization"},
                        New BoolParam With {.Switch = "--aq-temporal", .Text = "AQ Temporal"},
                        Lossless)
                    Add("Profile",
                        New OptionParam With {.Name = "LevelH264", .Switch = "--level", .Text = "Level:", .VisibleFunc = Function() Codec.ValueText = "h264", .Options = {"Unrestricted", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                        New OptionParam With {.Name = "LevelH265", .Switch = "--level", .Text = "Level:", .VisibleFunc = Function() Codec.ValueText = "h265", .Options = {"Unrestricted", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}},
                        Profile,
                        New BoolParam With {.Switch = "--bluray", .Text = "Blu-ray"})
                    Add("Performance",
                        New OptionParam With {.Switch = "--output-buf", .Text = "Output Buffer:", .Options = {"8", "16", "32", "64", "128"}},
                        New OptionParam With {.Switch = "--output-thread", .Text = "Output Thread:", .Options = {"Automatic", "Disabled", "One Thread"}, .Values = {"-1", "0", "1"}},
                        New BoolParam With {.Switch = "--max-procfps", .Text = "Limit performance to lower resource usage"})
                    Add("VUI",
                        New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat:", .Options = {"undef", "ntsc", "component", "pal", "secam", "mac"}},
                        New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix:", .Options = {"undef", "auto", "bt709", "smpte170m", "bt470bg", "smpte240m", "YCgCo", "fcc", "GBR", "bt2020nc", "bt2020c"}},
                        New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim:", .Options = {"undef", "auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "film", "bt2020"}},
                        New OptionParam With {.Switch = "--transfer", .Text = "Transfer:", .Options = {"undef", "auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "linear", "log100", "log316", "iec61966-2-4", "bt1361e", "iec61966-2-1", "bt2020-10", "bt2020-12", "smpte-st-2084", "smpte-st-428", "arib-srd-b67"}},
                        New BoolParam With {.Switch = "--fullrange", .Text = "Full Range", .VisibleFunc = Function() Codec.ValueText = "h264"})
                    Add("VPP",
                        New OptionParam With {.Switch = "--vpp-resize", .Text = "Resize:", .Options = {"Disabled", "default", "bilinear", "cubic", "cubic_b05c03", "cubic_bspline", "cubic_catmull", "lanczos", "nn", "npp_linear", "spline36", "super"}},
                        New OptionParam With {.Switch = "--vpp-deinterlace", .Text = "Deinterlace:", .VisibleFunc = Function() Decoder.ValueText = "nvnative" OrElse Decoder.ValueText = "nvcuda", .Options = {"none", "adaptive", "bob"}},
                        New OptionParam With {.Switch = "--vpp-gauss", .Text = "Gauss:", .Options = {"Disabled", "3", "5", "7"}},
                        KNN, KnnRadius, KnnStrength, KnnLerp, KnnThLerp,
                        PMD, PmdApplyCount, PmdStrength, PmdThreshold)
                    Add("Other",
                        New OptionParam With {.Switch = "--mv-precision", .Text = "MV Precision:", .Options = {"Q-pel", "half-pel", "full-pel"}, .Values = {"Q-pel", "half-pel", "full-pel"}},
                        New OptionParam With {.Switches = {"--cabac", "--cavlc"}, .Text = "Cabac/Cavlc:", .Options = {"Disabled", "Cabac", "Cavlc"}, .Values = {"", "--cabac", "--cavlc"}},
                        New OptionParam With {.Switch = "--interlaced", .Switches = {"--tff", "--bff"}, .Text = "Interlaced:", .VisibleFunc = Function() Codec.ValueText = "h264", .Options = {"Progressive ", "Top Field First", "Bottom Field First"}, .Values = {"", "--tff", "--bff"}},
                        New OptionParam With {.Switch = "--log-level", .Text = "Log Level:", .Options = {"info", "debug", "warn", "error"}},
                        New NumParam With {.Switch = "--device", .Text = "Device:", .MinMaxStep = {0, 4, 1}},
                        New BoolParam With {.Switch = "--deblock", .NoSwitch = "--no-deblock", .Text = "Deblock", .InitValue = True},
                        New StringParam With {.Text = "Custom:"})
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
                    ret += ",strength=" & KnnStrength.Value.ToString(CultureInfo.InvariantCulture)
                End If

                If KnnLerp.Value <> KnnLerp.DefaultValue Then
                    ret += ",lerp=" & KnnLerp.Value.ToString(CultureInfo.InvariantCulture)
                End If

                If KnnThLerp.Value <> KnnThLerp.DefaultValue Then
                    ret += ",th_lerp=" & KnnThLerp.Value.ToString(CultureInfo.InvariantCulture)
                End If

                If ret <> "" Then
                    Return "--vpp-knn " + ret.TrimStart(","c)
                Else
                    Return "--vpp-knn"
                End If
            End If
        End Function

        Private Sub Add(path As String, ParamArray items As CommandLineParam())
            For Each i In items
                i.Path = path
                ItemsValue.Add(i)
            Next
        End Sub

        Overrides Function GetCommandLine(includePaths As Boolean,
                                          includeExe As Boolean,
                                          Optional pass As Integer = 1) As String
            Dim ret As String
            Dim sourcePath As String
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)

            If includePaths AndAlso includeExe Then ret = Package.NVEncC.Path.Quotes

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
                    If includePaths Then ret = If(includePaths, Package.QSVEncC.Path.Quotes, "QSVEncC") + " -o - -c raw" + " -i " + If(includePaths, p.SourceFile.Quotes, "path") + " | " + If(includePaths, Package.NVEncC.Path.Quotes, "NVEncC")
                Case "ffdxva"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.ffmpeg.Path.Quotes, "ffmpeg") + " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.SourceFile.Quotes, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | " + If(includePaths, Package.NVEncC.Path.Quotes, "NVEncC")
                Case "ffqsv"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.ffmpeg.Path.Quotes, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.SourceFile.Quotes, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | " + If(includePaths, Package.NVEncC.Path.Quotes, "NVEncC")
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
            ElseIf p.AutoARSignaling AndAlso p.SourceFile <> "" Then
                Dim par = Calc.GetTargetPAR
                If par <> New Point(1, 1) Then ret += " --sar " & par.X & ":" & par.Y
            End If

            If Decoder.ValueText <> "avs" Then
                If p.Ranges.Count > 0 Then
                    ret += " --trim " + p.Ranges.Select(Function(range) range.Start & ":" & range.End).Join(",")
                End If
            End If

            If sourcePath = "-" Then ret += " --y4m"
            If includePaths Then ret += " -i " + sourcePath.Quotes + " -o " + targetPath.Quotes

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.NVEncC
        End Function
    End Class
End Class