Imports System.Text
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

            f.cms.Items.Add(New ActionMenuItem("Save Profile...", saveProfileAction))
            f.cms.Items.Add(New ActionMenuItem("Check Hardware", Sub() MsgInfo(ProcessHelp.GetStdOut(Packs.NVEncC.GetPath, "--check-hw"))))
            f.cms.Items.Add(New ActionMenuItem("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetStdOut(Packs.NVEncC.GetPath, "--check-features"))))
            f.cms.Items.Add(New ActionMenuItem("Check Environment", Sub() g.ShowCode("Check Environment", ProcessHelp.GetErrorOutput(Packs.NVEncC.GetPath, "--check-environment"))))

            If f.ShowDialog() = DialogResult.OK Then
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides ReadOnly Property OutputFileType() As String
        Get
            Return Params.Codec.ValueText
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Dim cl = Params.GetCommandLine(True, False)

        If cl.Contains(" | ") Then
            Dim batchPath = p.TempDir + p.TargetFile.Base + "_NVEncC.bat"
            File.WriteAllText(batchPath, cl, Encoding.GetEncoding(850))

            Using proc As New Proc
                proc.Init("Encoding using NVEncC " + Packs.NVEncC.Version)
                proc.SkipStrings = {"%]", " frames: "}
                proc.WriteLine(cl + CrLf2)
                proc.File = "cmd.exe"
                proc.Arguments = "/C call """ + batchPath + """"
                proc.Start()
            End Using
        Else
            Using proc As New Proc
                proc.Init("Encoding using NVEncC " + Packs.NVEncC.Version)
                proc.SkipStrings = {"%]"}
                proc.File = Packs.NVEncC.GetPath
                proc.Arguments = cl
                proc.Start()
            End Using
        End If

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
            .Options = {"AviSynth/VapourSynth", "NVEncC (NVIDIA CUVID)", "QSVEncC (Intel)", "ffmpeg (Intel)", "ffmpeg (DXVA2)"},
            .Values = {"avs", "nv", "qs", "ffqsv", "ffdxva"}}

        Property Mode As New OptionParam With {
            .Text = "Mode:",
            .Switches = {"--cbr", "--vbr", "--vbr2", "--cqp"},
            .Options = {"CBR", "VBR", "VBR2", "CQP"},
            .VisibleFunc = Function() Not Lossless.Value,
            .ArgsFunc = Function() As String
                            Select Case Mode.OptionText
                                Case "CBR"
                                    Return " --cbr " & p.VideoBitrate
                                Case "VBR"
                                    Return " --vbr " & p.VideoBitrate
                                Case "VBR2"
                                    Return " --vbr2 " & p.VideoBitrate
                                Case "CQP"
                                    Return " --cqp " & QPI.Value & ":" & QPP.Value & ":" & QPB.Value
                            End Select
                        End Function}

        Property Codec As New OptionParam With {
            .Switch = "--codec",
            .Text = "Codec:",
            .Options = {"H.264", "H.265"},
            .Values = {"h264", "h265"}}

        Property LevelH264 As New OptionParam With {
            .Name = "LevelH264",
            .Switch = "--level",
            .Text = "Level:",
            .VisibleFunc = Function() Codec.ValueText = "h264",
            .Options = {"Unrestricted", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"},
            .Values = {"", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}}

        Property LevelH265 As New OptionParam With {
            .Name = "LevelH265",
            .Switch = "--level",
            .Text = "Level:",
            .VisibleFunc = Function() Codec.ValueText = "h265",
            .Options = {"Unrestricted", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"},
            .Values = {"", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}}

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

        Property BFrames As New NumParam With {
            .Switch = "--bframes",
            .Text = "B Frames:",
            .Value = 3,
            .DefaultValue = 3,
            .MinMaxStep = {0, 16, 1}}

        Property Ref As New NumParam With {
            .Switch = "--ref",
            .Text = "References:",
            .Value = 3,
            .DefaultValue = 3,
            .MinMaxStep = {0, 16, 1}}

        Property Lossless As New BoolParam With {
            .Switch = "--lossless",
            .Text = "Lossless",
            .VisibleFunc = Function() Codec.ValueText = "h264" AndAlso Profile.Visible,
            .Value = False,
            .DefaultValue = False}

        Property Custom As New StringParam With {
            .Text = "Custom:",
            .ArgsFunc = Function() Custom.Value}

        Private ItemsValue As List(Of CommandLineParam)

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)
                    Add("Basic", Decoder, Mode, Codec, QPI, QPP, QPB)
                    Add("Rate Control",
                        New NumParam With {.Switch = "--qp-init", .Text = "Initial QP:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--max-bitrate", .Text = "Maximum Bitrate:", .InitValue = 17500, .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--vbv-bufsize", .Text = "VBV Bufsize:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New BoolParam With {.Switch = "--aq", .Text = "Adaptive Quantization", .Value = False, .DefaultValue = False},
                        Lossless)
                    Add("Profile", LevelH264, LevelH265, Profile,
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
                    Add("Other",
                        New OptionParam With {.Switch = "--vpp-deinterlace", .Text = "Deinterlace:", .VisibleFunc = Function() Decoder.ValueText = "nv", .Options = {"none", "adaptive", "bob"}},
                        New OptionParam With {.Switch = "--interlaced", .Switches = {"--tff", "--bff"}, .Text = "Interlaced:", .VisibleFunc = Function() Codec.ValueText = "h264", .Options = {"Progressive ", "Top Field First", "Bottom Field First"}, .Values = {"", "--tff", "--bff"}},
                        New OptionParam With {.Switch = "--mv-precision", .Text = "MV Precision:", .Options = {"Q-pel", "half-pel", "full-pel"}, .Values = {"Q-pel", "half-pel", "full-pel"}},
                        New OptionParam With {.Switch = "--log-level", .Text = "Log Level:", .Options = {"info", "debug", "warn", "error"}},
                        BFrames, Ref,
                        New NumParam With {.Switch = "--gop-len", .Text = "GOP Length:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--cu-max", .Text = "Max CU Size:", .MinMaxStep = {0, 64, 16}},
                        New NumParam With {.Switch = "--cu-min", .Text = "Min CU Size:", .MinMaxStep = {0, 32, 16}},
                        New BoolParam With {.Switch = "--deblock", .Text = "Deblock"},
                        New BoolParam With {.Switch = "--cabac", .Text = "Cabac"},
                        New BoolParam With {.Switch = "--cavlc", .Text = "Cavlc"},
                        Custom)
                End If

                Return ItemsValue
            End Get
        End Property

        Private Sub Add(path As String, ParamArray items As CommandLineParam())
            For Each i In items
                i.Path = path
                ItemsValue.Add(i)
            Next
        End Sub

        Overrides Function GetCommandLine(includePaths As Boolean,
                                          includeExecutable As Boolean,
                                          Optional pass As Integer = 0) As String
            Dim ret As String
            Dim sourcePath As String
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputFileType)

            If includePaths AndAlso includeExecutable Then
                ret = Packs.NVEncC.GetPath.Quotes
            End If

            Select Case Decoder.ValueText
                Case "avs"
                    sourcePath = p.Script.Path
                Case "nv"
                    sourcePath = p.LastOriginalSourceFile
                Case "qs"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Packs.QSVEncC.GetPath.Quotes, "QSVEncC") + " -o - -c raw" + " -i " + If(includePaths, p.SourceFile.Quotes, "path") + " | " + If(includePaths, Packs.NVEncC.GetPath.Quotes, "NVEncC")
                Case "ffdxva"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Packs.ffmpeg.GetPath.Quotes, "ffmpeg") + " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.SourceFile.Quotes, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | " + If(includePaths, Packs.NVEncC.GetPath.Quotes, "NVEncC")
                Case "ffqsv"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Packs.ffmpeg.GetPath.Quotes, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.SourceFile.Quotes, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | " + If(includePaths, Packs.NVEncC.GetPath.Quotes, "NVEncC")
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
            ElseIf p.AutoARSignaling Then
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
            Return Packs.NVEncC
        End Function
    End Class
End Class