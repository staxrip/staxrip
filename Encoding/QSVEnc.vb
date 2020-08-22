
Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class QSVEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "Intel | " + Params.Codec.OptionText.Replace("Intel ", "")
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
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        params1.Init(store)

        Using form As New CommandLineForm(params1)
            form.HTMLHelp = $"<p><a href=""{Package.QSVEnc.HelpURL}"">QSVEnc online help</a></p>" +
                $"<p><a href=""https://github.com/staxrip/staxrip/wiki/qsvenc-bitrate-modes"">QSVEnc bitrate modes</a></p>" +
                $"<pre>{HelpDocument.ConvertChars(Package.QSVEnc.CreateHelpfile())}</pre>"

            Dim saveProfileAction = Sub()
                                        Dim enc = ObjectHelp.GetCopy(Of QSVEnc)(Me)
                                        Dim params2 As New EncoderParams
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            ActionMenuItem.Add(form.cms.Items, "Save Profile...", saveProfileAction).SetImage(Symbol.Save)
            form.cms.Items.Add(New ActionMenuItem("Check Environment", Sub() g.ShowCode("Check Environment", ProcessHelp.GetConsoleOutput(Package.QSVEnc.Path, "--check-environment"))))
            form.cms.Items.Add(New ActionMenuItem("Check Hardware", Sub() MsgInfo(ProcessHelp.GetConsoleOutput(Package.QSVEnc.Path, "--check-hw"))))
            form.cms.Items.Add(New ActionMenuItem("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetConsoleOutput(Package.QSVEnc.Path, "--check-features"))))

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
            Dim codecs = ProcessHelp.GetConsoleOutput(Package.QSVEnc.Path, "--check-features").Right("Codec")

            If Not codecs?.ToLower.Contains("hevc") Then
                Throw New ErrorAbortException("QSVEnc Error", "H.265/HEVC isn't supported by your Hardware.")
            End If
        End If

        p.Script.Synchronize()
        Params.RaiseValueChanged(Nothing)

        Using proc As New Proc
            proc.Header = "Video encoding"
            proc.Package = Package.QSVEnc
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
            Return Params.Mode.ValueText.EqualsAny("cqp", "vqp", "icq", "la-icq", "qvbr-q")
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
            raw avs vpy-mt audio-source audio-file seek format caption2ass output-format audio-profile
            audio-copy audio-copy audio-codec audio-bitrate audio-ignore check-profiles vpp-delogo-pos
            audio-ignore audio-samplerate audio-resampler audio-stream audio-stream vpp-delogo-y vpy
            audio-stream audio-stream audio-filter chapter-copy chapter sub-copy vpp-delogo-depth input
            avsync mux-option input-res fps dar avqsv-analyze benchmark vpp-delogo-cb vpp-delogo-cr
            bench-quality log log-framelist audio-thread avi avqsv input-file python qvbr-quality help
            audio-ignore-decode-error audio-ignore-notrack-error nv12 output-file sharpness vpp-delogo
            check-features-html perf-monitor perf-monitor-plot perf-monitor-interval vpp-delogo-select
            audio-delay audio-disposition audio-metadata option-list sub-disposition sub-metadata
            metadata video-metadata video-tag attachment-copy"

        tester.UndocumentedSwitches = "input-thread chromaloc videoformat colormatrix colorprim transfer fullrange"
        tester.Package = Package.QSVEnc
        tester.CodeFile = Folder.Startup.Parent + "Encoding\qsvenc.vb"

        Return tester.Test
    End Function

    Public Class EncoderParams
        Inherits CommandLineParams

        Sub New()
            Title = "QSVEnc Options"
        End Sub

        Property Decoder As New OptionParam With {
            .Text = "Decoder",
            .Options = {"AviSynth/VapourSynth", "QSVEnc Hardware", "QSVEnc Software", "ffmpeg Intel", "ffmpeg DXVA2"},
            .Values = {"avs", "qshw", "qssw", "ffqsv", "ffdxva"}}

        Property Codec As New OptionParam With {
            .Switch = "--codec",
            .Text = "Codec",
            .Options = {"Intel H.264", "Intel H.265", "Intel MPEG-2"},
            .Values = {"h264", "hevc", "mpeg2"}}

        Property Mode As New OptionParam With {
            .Switches = {"--avbr", "--cbr", "--vbr", "--qvbr-q", "--cqp", "--icq", "--la-icq", "--vcm", "--la", "--la-hrd", "--qvbr"},
            .Name = "Mode",
            .Text = "Mode",
            .Expand = True,
            .Options = {"AVBR - Average Variable Bitrate", "CBR - Constant Bitrate", "CQP - Constant QP", "ICQ - Intelligent Constant Quality", "LA - VBR Lookahead", "LA-HRD - VBR HRD Lookahead", "LA-ICQ - Intelligent Constant Quality Lookahead", "QVBR - Quality Variable Bitrate using bitrate", "QVBR-Q - Quality Variable Bitrate using quality", "VBR - Variable Bitrate", "VCM - Video Conferencing Mode"},
            .Values = {"avbr", "cbr", "cqp", "icq", "la", "la-hrd", "la-icq", "qvbr", "qvbr-q", "vbr", "vcm"},
            .Init = 2}

        Property Quality As New NumParam With {
            .Text = "Quality",
            .Init = 23,
            .VisibleFunc = Function() {"icq", "la-icq", "qvbr-q"}.Contains(Mode.ValueText),
            .Config = {0, 51}}

        Property QPI As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP I",
            .Init = 24,
            .VisibleFunc = Function() {"cqp"}.Contains(Mode.ValueText),
            .Config = {0, 51}}

        Property QPP As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP P",
            .Init = 26,
            .VisibleFunc = Function() {"cqp"}.Contains(Mode.ValueText),
            .Config = {0, 51}}

        Property QPB As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP B",
            .Init = 27,
            .VisibleFunc = Function() {"cqp"}.Contains(Mode.ValueText),
            .Config = {0, 51}}

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

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)

                    Add("Basic", Mode, Decoder, Codec,
                        New OptionParam With {.Switch = "--quality", .Text = "Preset", .Options = {"Best", "Higher", "High", "Balanced", "Fast", "Faster", "Fastest"}, .Init = 3},
                        New OptionParam With {.Switch = "--profile", .Text = "Profile", .Name = "ProfileH264", .VisibleFunc = Function() Codec.Value = 0, .Options = {"Automatic", "Baseline", "Main", "High"}},
                        New OptionParam With {.Switch = "--profile", .Text = "Profile", .Name = "ProfileH265", .VisibleFunc = Function() Codec.Value = 1, .Options = {"Automatic", "Main", "Main 10"}},
                        New OptionParam With {.Switch = "--profile", .Text = "Profile", .Name = "ProfileMPEG2", .VisibleFunc = Function() Codec.Value = 2, .Options = {"Automatic", "Simple", "Main", "High"}},
                        New OptionParam With {.Switch = "--tier", .Text = "Tier", .VisibleFunc = Function() Codec.ValueText = "h265", .Options = {"Main", "High"}, .Values = {"main", "high"}},
                        New OptionParam With {.Switch = "--level", .Name = "LevelHEVC", .Text = "Level", .VisibleFunc = Function() Codec.Value = 1, .Options = {"Automatic", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}},
                        New OptionParam With {.Switch = "--level", .Text = "Level", .VisibleFunc = Function() Codec.Value = 0, .Options = {"Automatic", "1", "1b", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                        New OptionParam With {.Switch = "--level", .Name = "LevelMPEG2", .Text = "Level", .VisibleFunc = Function() Codec.Value = 2, .Options = {"Automatic", "low", "main", "high", "High1440"}},
                        Quality, QPI, QPP, QPB)
                    Add("Analysis",
                        New OptionParam With {.Switch = "--trellis", .Text = "Trellis", .Options = {"Automatic", "Off", "I", "IP", "All"}},
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
                        New BoolParam With {.Switch = "--adapt-ltr", .Text = "Adaptive LTR frames"},
                        New BoolParam With {.Switch = "--direct-bias-adjust", .Text = "Direct Bias Adjust"},
                        New BoolParam With {.Switch = "--strict-gop", .Text = "Strict Gop"},
                        New BoolParam With {.Switch = "--open-gop", .Text = "Open Gop"})
                    Add("Rate Control", VBVbufsize,
                        New NumParam With {.Switch = "--max-bitrate", .Text = "Max Bitrate", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--qp-offset", .Text = "QP Offset", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--avbr-unitsize", .Text = "AVBR Unitsize", .Init = 90},
                        New BoolParam With {.Switch = "--mbbrc", .Text = "Per macro block rate control"})
                    Add("Motion Search",
                        New OptionParam With {.Switch = "--mv-scaling", .Text = "MV Scaling", .IntegerValue = True, .Options = {"Default", "MV cost to be 0", "MV cost 1/2 of default", "MV cost 1/4 of default", "MV cost 1/8 of default"}},
                        New BoolParam With {.Switch = "--weightb", .Text = "B-Frame Weight Prediction"},
                        New BoolParam With {.Switch = "--weightp", .Text = "P-Frame Weight Prediction"})
                    Add("Performance",
                        New OptionParam With {.Switch = "--output-buf", .Text = "Output Buffer", .Options = {"8", "16", "32", "64", "128"}},
                        New NumParam With {.Switch = "--input-buf", .Text = "Input Buffer", .Config = {0, 16}},
                        New NumParam With {.Switch = "--mfx-thread", .Text = "Input Threads MFX"},
                        New NumParam With {.Switch = "--output-thread", .Text = "Output Thread", .Config = {0, 64}},
                        New NumParam With {.Switch = "--async-depth", .Text = "Async Depth", .Config = {0, 64}},
                        New BoolParam With {.Switch = "--min-memory", .Text = "Minimize memory usage"},
                        New BoolParam With {.Switch = "--max-procfps", .Text = "Limit performance to lower resource usage"})
                    Add("VPP",
                        New StringParam With {.Switch = "--vpp-colorspace", .Text = "Colorspace"},
                        New OptionParam With {.Switch = "--vpp-rotate", .Text = "Rotate", .Options = {"0", "90", "180", "270"}},
                        New OptionParam With {.Switch = "--vpp-image-stab", .Text = "Image Stabilizer", .Options = {"Disabled", "Upscale", "Box"}},
                        New OptionParam With {.Switch = "--vpp-mirror", .Text = "Mirror Image", .Options = {"Disabled", "H", "V"}},
                        New OptionParam With {.Switch = "--vpp-resize", .Text = "Scaling Quality", .Options = {"Auto", "Simple", "Fine"}},
                        New NumParam With {.Switch = "--vpp-denoise", .Text = "Denoise", .Config = {0, 100}},
                        New NumParam With {.Switch = "--vpp-detail-enhance", .Text = "Detail Enhance", .Config = {0, 100}},
                        mctf,
                        mctfval)
                    Add("VUI",
                        New StringParam With {.Switch = "--master-display", .Text = "Master Display", .VisibleFunc = Function() Codec.ValueText = "hevc"},
                        New StringParam With {.Switch = "--sar", .Text = "Sample Aspect Ratio", .Init = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                        New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat", .Options = {"Undefined", "NTSC", "Component", "PAL", "SECAM", "MAC"}},
                        New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix", .Options = {"Undefined", "BT 2020 NC", "BT 2020 C", "BT 470 BG", "BT 709", "FCC", "GBR", "SMPTE 170 M", "SMPTE 240 M", "YCgCo"}},
                        New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim", .Options = {"Undefined", "BT 709", "SMPTE 170 M", "BT 470 M", "BT 470 BG", "SMPTE 240 M", "Film", "BT 2020"}},
                        New OptionParam With {.Switch = "--colorrange", .Text = "Colorrange", .Options = {"Undefined", "Limited", "TV", "Full", "PC", "Auto"}},
                        New OptionParam With {.Switch = "--transfer", .Text = "Transfer", .Options = {"Undefined", "BT 709", "SMPTE 170 M", "BT 470 M", "BT 470 BG", "SMPTE 240 M", "Linear", "Log 100", "Log 316", "IEC 61966-2-4", "BT 1361 E", "IEC 61966-2-1", "BT 2020-10", "BT 2020-12", "SMPTE 2084", "SMPTE 428", "ARIB-SRD-B67"}},
                        MaxCLL, MaxFALL, Chromaloc,
                        New BoolParam With {.Switch = "--pic-struct", .Text = "Set the picture structure and emits it in the picture timing SEI message"},
                        New BoolParam With {.Switch = "--fullrange", .Text = "Fullrange"},
                        New BoolParam With {.Switch = "--aud", .Text = "AUD"})
                    Add("Other",
                        New StringParam With {.Text = "Custom", .Quotes = QuotesMode.Never, .AlwaysOn = True},
                        New StringParam With {.Switch = "--sub-source", .Text = "Subtitle File", .BrowseFile = True, .BrowseFileFilter = FileTypes.GetFilter(FileTypes.SubtitleExludingContainers)},
                        New StringParam With {.Switch = "--data-copy", .Text = "Data Copy"},
                        New StringParam With {.Switch = "--input-option", .Text = "Input Option"},
                        New OptionParam With {.Switch = "--interlace", .Text = "Interlace", .Options = {"Undefined", "TFF", "BFF"}},
                        New OptionParam With {.Switch = "--vpp-deinterlace", .Text = "Deinterlace", .Options = {"None", "Normal", "Inverse Telecine", "Double Framerate"}, .Values = {"none", "normal", "it", "bob"}},
                        New OptionParam With {.Switches = {"--disable-d3d", "--d3d9", "--d3d11", "--d3d"}, .Text = "D3D", .Options = {"Disabled", "D3D9", "D3D11", "D3D9/D3D11"}, .Values = {"--disable-d3d", "--d3d9", "--d3d11", "--d3d"}, .Init = 3},
                        New OptionParam With {.Switch = "--log-level", .Text = "Log Level", .Options = {"Info", "Debug", "Warn", "Error"}},
                        New OptionParam With {.Switch = "--sao", .Text = "SAO", .Options = {"Auto", "None", "Luma", "Chroma", "All"}, .VisibleFunc = Function() Codec.ValueText = "hevc"},
                        New OptionParam With {.Switch = "--ctu", .Text = "CTU", .Options = {"16", "32", "64"}, .VisibleFunc = Function() Codec.ValueText = "hevc"},
                        New BoolParam With {.Switch = "--no-deblock", .Text = "No Deblock"},
                        New BoolParam With {.Switch = "--fallback-rc", .Text = "Enable fallback for unsupported modes", .Value = True},
                        New BoolParam With {.Switch = "--timer-period-tuning", .NoSwitch = "--no-timer-period-tuning", .Text = "Timer Period Tuning", .Init = True},
                        New BoolParam With {.Switch = "--i-adapt", .Text = "Adaptive I-Frame Insert"},
                        New BoolParam With {.Switch = "--fixed-func", .Text = "Use fixed func instead of GPU EU"},
                        New BoolParam With {.Switch = "--bluray", .Text = "Blu-ray"},
                        New BoolParam With {.Switch = "--tskip", .Text = "T-Skip", .VisibleFunc = Function() Codec.ValueText = "hevc"},
                        New BoolParam With {.Switch = "--fade-detect", .Text = "Fade Detection"},
                        New BoolParam With {.Switch = "--lowlatency", .Text = "Low Latency"})

                    For Each item In ItemsValue
                        If item.HelpSwitch <> "" Then
                            Continue For
                        End If

                        Dim switches = item.GetSwitches

                        If switches.NothingOrEmpty Then
                            Continue For
                        End If

                        item.HelpSwitch = switches(0)
                    Next
                End If

                Return ItemsValue
            End Get
        End Property

        Public Overrides Sub ShowHelp(id As String)
            g.ShowCommandLineHelp(Package.QSVEnc, id)
        End Sub

        Protected Overrides Sub OnValueChanged(item As CommandLineParam)
            If Not Mode.MenuButton Is Nothing AndAlso (item Is Codec OrElse item Is Nothing) Then
                For x = 0 To Mode.Values.Length - 1
                    Select Case Codec.ValueText
                        Case "h264"
                            Mode.ShowOption(x, True)
                        Case "hevc"
                            Mode.ShowOption(x, Mode.Values(x).EqualsAny("cbr", "vbr", "cqp", "icq", "vcm"))
                        Case "mpeg2"
                            Mode.ShowOption(x, Mode.Values(x).EqualsAny("cbr", "vbr", "avbr", "cqp"))
                    End Select
                Next
            End If

            If Not QPI.NumEdit Is Nothing Then
                mctfval.NumEdit.Enabled = mctf.Value
            End If

            MyBase.OnValueChanged(item)
        End Sub

        Overrides Function GetCommandLine(
            includePaths As Boolean,
            includeExecutable As Boolean,
            Optional pass As Integer = 1) As String

            Dim ret As String
            Dim sourcePath = p.Script.Path
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)

            If includePaths AndAlso includeExecutable Then
                ret = Package.QSVEnc.Path.Escape
            End If

            Select Case Decoder.ValueText
                Case "avs"
                    sourcePath = p.Script.Path

                    If includePaths AndAlso FrameServerHelp.IsAviSynthPortableUsed Then
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
                            If(includePaths, Package.QSVEnc.Path.Escape, "QSVEncC64")
                    End If
                Case "ffqsv"
                    sourcePath = "-"

                    If includePaths Then
                        ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.LastOriginalSourceFile.Escape, "path") + " -f yuv4mpegpipe -strict -1 -pix_fmt yuv420p -loglevel fatal -hide_banner - | " + If(includePaths, Package.QSVEnc.Path.Escape, "QSVEncC64")
                    End If
            End Select

            Dim q = From i In Items Where i.GetArgs <> ""

            If q.Count > 0 Then
                ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")
            End If

            Select Case Mode.ValueText
                Case "icq", "la-icq"
                    ret += " --" + Mode.ValueText + " " & CInt(Quality.Value)
                Case "qvbr-q"
                    ret += " --qvbr-q " & CInt(Quality.Value) & " --qvbr " & p.VideoBitrate
                Case "cqp"
                    ret += " --" + Mode.ValueText + " " & CInt(QPI.Value) & ":" & CInt(QPP.Value) & ":" & CInt(QPB.Value)
                Case Else
                    ret += " --" + Mode.ValueText + " " & p.VideoBitrate
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

            If Decoder.ValueText <> "avs" Then
                If p.Ranges.Count > 0 Then
                    ret += " --trim " + p.Ranges.Select(Function(range) range.Start & ":" & range.End).Join(",")
                End If
            End If

            If sourcePath = "-" Then
                ret += " --y4m"
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
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.QSVEnc
        End Function
    End Class
End Class
