Imports System.Text
Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class AOMEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Sub New()
        Name = "AV1"
        AutoCompCheckValue = 50
    End Sub

    <NonSerialized>
    Private ParamsValue As AV1Params

    Property Params As AV1Params
        Get
            If ParamsValue Is Nothing Then
                ParamsValue = New AV1Params
                ParamsValue.Init(ParamsStore)
            End If

            Return ParamsValue
        End Get
        Set(value As AV1Params)
            ParamsValue = value
        End Set
    End Property

    Overrides ReadOnly Property OutputExt As String
        Get
            Return "webm"
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Encode("Encoding video using aomenc " + Package.AOMEnc.Version, GetArgs(1, p.Script), s.ProcessPriority)

        If Params.Mode.Value = AV1RateMode.TwoPass Then
            Encode("Encoding video second pass using aomenc " + Package.AOMEnc.Version, GetArgs(2, p.Script), s.ProcessPriority)
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
            proc.SkipStrings = {"[ETA"}
            proc.WriteLine(batchCode + BR2)
            proc.File = "cmd.exe"
            proc.Arguments = "/C call """ + batchPath.Escape + """"
            proc.Start()
        End Using
    End Sub

    Overloads Function GetArgs(pass As Integer, script As VideoScript, Optional includePaths As Boolean = True) As String
        Return Params.GetArgs(pass, script, OutputPath.DirAndBase + OutputExtFull, includePaths, True)
    End Function

    Overrides Function GetMenu() As MenuList
        Dim r As New MenuList
        r.Add("Encoder Options", AddressOf ShowConfigDialog)
        r.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return r
    End Function

    Overrides Sub ShowConfigDialog()
        Dim newParams As New AV1Params
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(store)

        Using form As New CommandLineForm(newParams)
            form.Text = "Under construction, AV1 isn't finished yet"

            Dim saveProfileAction = Sub()
                                        Dim enc = ObjectHelp.GetCopy(Of AOMEnc)(Me)
                                        Dim params2 As New AV1Params
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            ActionMenuItem.Add(form.cms.Items, "Save Profile...", saveProfileAction).SetImage(Symbol.Save)

            If form.ShowDialog() = DialogResult.OK Then
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides Property QualityMode() As Boolean
        Get
            Return False
        End Get
        Set(Value As Boolean)
        End Set
    End Property

    Public Overrides ReadOnly Property CommandLineParams As CommandLineParams
        Get
            Return Params
        End Get
    End Property
End Class

Public Class AV1Params
    Inherits CommandLineParams

    Sub New()
        Title = "AV1 Options"
        Separator = "="
    End Sub

    Property Mode As New OptionParam With {
        .Name = "Mode",
        .Text = "Mode:",
        .Path = "Basic",
        .Options = {"One Pass", "Two Pass"}}

    Overrides ReadOnly Property Items As List(Of CommandLineParam)
        Get
            If ItemsValue Is Nothing Then
                ItemsValue = New List(Of CommandLineParam)

                Add(Mode)

                Add(New OptionParam With {.Path = "Basic", .Switch = "--bit-depth", .Text = "Depth", .Options = {"8", "10", "12"}, .Help = "Bit depth for codec (8 for version <=1, 10 or 12 for version 2)"},
                    New OptionParam With {.Path = "Rate Control", .Switch = "--lossless", .Text = "Lossless", .IntegerValue = True, .Options = {"false", "true"}, .Help = "Lossless mode"},
                    New OptionParam With {.Path = "Rate Control", .Switch = "--aq-mode", .Text = "AQ Mode", .IntegerValue = True, .Options = {"off", "variance", "complexity", "cyclic", "refresh"}, .Help = "Adaptive quantization mode"},
                    New OptionParam With {.Path = "Rate Control", .Switch = "--deltaq-mode", .Text = "Delta QIndex Mode", .IntegerValue = True, .Options = {"off", "deltaq", "deltaq", "+", "deltalf"}},
                    New NumParam With {.Path = "Rate Control", .Switch = "--bias-pct", .Text = "Bias PCT", .MinMaxStep = {0, 100, 1}, .Help = "CBR/VBR bias (0=CBR, 100=VBR)"},
                    New NumParam With {.Path = "Rate Control", .Switch = "--max-intra-rate", .Text = "Max Intra Rate", .Help = "Max I-frame bitrate (pct)"},
                    New NumParam With {.Path = "Rate Control", .Switch = "--max-inter-rate", .Text = "Max Inter Rate", .Help = "Max P-frame bitrate (pct)"},
                    New NumParam With {.Path = "Rate Control", .Switch = "--undershoot-pct", .Text = "Undershoot PCT", .Help = "Datarate undershoot (min) target (%)"},
                    New NumParam With {.Path = "Rate Control", .Switch = "--overshoot-pct", .Text = "Overshoot PCT", .Help = "Datarate overshoot (max) target (%)"},
                    New NumParam With {.Path = "Rate Control", .Switch = "--minsection-pct", .Text = "Minsection PCT", .Help = "GOP min bitrate (% of target)"},
                    New NumParam With {.Path = "Rate Control", .Switch = "--maxsection-pct", .Text = "Maxsection PCT", .Help = "GOP max bitrate (% of target)"},
                    New NumParam With {.Path = "Rate Control", .Switch = "--gf-cbr-boost", .Text = "GF CBR Boost", .Help = "Boost for Golden Frame in CBR mode (pct)"},
                    New NumParam With {.Path = "Input/Output", .Switch = "--input-bit-depth", .Text = "Input Bit Depth", .Help = "Bit depth of input"},
                    New NumParam With {.Path = "Input/Output", .Switch = "--fps", .Text = "Frame Rate", .Help = "Stream frame rate (rate/scale)"},
                    New BoolParam With {.Path = "Input/Output", .Switch = "--yv12", .Text = "yv12", .Help = "Input file is YV12"},
                    New BoolParam With {.Path = "Input/Output", .Switch = "--i420", .Text = "i420", .Help = "Input file is I420 (default)"},
                    New BoolParam With {.Path = "Input/Output", .Switch = "--i422", .Text = "i422", .Help = "Input file is I422"},
                    New BoolParam With {.Path = "Input/Output", .Switch = "--i444", .Text = "i444", .Help = "Input file is I444"},
                    New BoolParam With {.Path = "Input/Output", .Switch = "--i440", .Text = "i440", .Help = "Input file is I440"},
                    New OptionParam With {.Path = "VUI", .Switch = "--color-space", .Text = "color-space", .Options = {"unknown", "bt601", "bt709", "smpte170", "smpte240", "bt2020", "reserved", "sRGB"}, .Help = "The color space of input content"},
                    New OptionParam With {.Path = "Performance", .Switch = "--tune", .Text = "tune", .Options = {"psnr", "ssim"}, .Help = "Material to favor"},
                    New NumParam With {.Path = "Performance", .Switch = "-t", .Text = "Threads", .Help = "Max number of threads to use"},
                    New NumParam With {.Path = "Performance", .Switch = "--cpu-used", .Text = "cpu-used", .MinMaxStep = {0, 8, 1}, .Help = "CPU Used (0..8)"},
                    New BoolParam With {.Path = "Statistic", .Switch = "--psnr", .Text = "psnr", .Help = "Show PSNR in status line"},
                    New BoolParam With {.Path = "Statistic", .Switch = "--debug", .Text = "Debug", .Help = "Debug mode (makes output deterministic)"},
                    New OptionParam With {.Path = "Image Size", .Switch = "--resize-allowed", .Text = "resize-allowed", .Options = {"true", "false"}, .Help = "Spatial resampling enabled (bool)"},
                    New NumParam With {.Path = "Image Size", .Switch = "--width", .Text = "width", .Help = "Frame width"},
                    New NumParam With {.Path = "Image Size", .Switch = "--height", .Text = "height", .Help = "Frame height"},
                    New NumParam With {.Path = "Image Size", .Switch = "--resize-width", .Text = "resize-width", .Help = "Width of encoded frame"},
                    New NumParam With {.Path = "Image Size", .Switch = "--resize-height", .Text = "resize-height", .Help = "Height of encoded frame"},
                    New NumParam With {.Path = "Image Size", .Switch = "--resize-up", .Text = "resize-up", .Help = "Upscale threshold (buf %)"},
                    New NumParam With {.Path = "Image Size", .Switch = "--resize-down", .Text = "resize-down", .Help = "Downscale threshold (buf %)"},
                    New NumParam With {.Path = "", .Switch = "--limit", .Text = "limit", .Help = "Stop encoding after n input frames"},
                    New NumParam With {.Path = "", .Switch = "--skip", .Text = "skip", .Help = "Skip the first n input frames"},
                    New StringParam With {.Path = "", .Switch = "--deadline", .Text = "deadline", .Help = "Deadline per frame (usec)"},
                    New BoolParam With {.Path = "", .Switch = "--output-partitions", .Text = "output-partitions", .Help = "Makes encoder output partitions. Requires IVF output!"},
                    New StringParam With {.Path = "", .Switch = "--q-hist", .Text = "q-hist", .Help = "Show quantizer histogram (n-buckets)"},
                    New StringParam With {.Path = "", .Switch = "--rate-hist", .Text = "rate-hist", .Help = "Show rate histogram (n-buckets)"},
                    New BoolParam With {.Path = "", .Switch = "--disable-warnings", .Text = "disable-warnings", .Help = "Disable warnings about potentially incorrect encode settings."},
                    New BoolParam With {.Path = "", .Switch = "--disable-warning-prompt", .Text = "disable-warning-prompt", .Help = "Display warnings, but do not prompt user to continue."},
                    New OptionParam With {.Path = "", .Switch = "--test-decode", .Text = "test-decode", .Options = {"off", "fatal", "warn"}, .Help = "Test encode/decode mismatch"},
                    New BoolParam With {.Path = "", .Switch = "--good", .Text = "good", .Help = "Use Good Quality Deadline"},
                    New BoolParam With {.Path = "", .Switch = "--quiet", .Text = "quiet", .Help = "Do not print encode progress"},
                    New BoolParam With {.Path = "", .Switch = "--verbose", .Text = "verbose", .Help = "Show encoder parameters"},
                    New BoolParam With {.Path = "", .Switch = "--webm", .Text = "webm", .Help = "Output WebM (default when WebM IO is enabled)"},
                    New BoolParam With {.Path = "", .Switch = "--ivf", .Text = "ivf", .Help = "Output IVF"},
                    New NumParam With {.Path = "", .Switch = "--usage", .Text = "usage", .Help = "Usage profile number to use"},
                    New NumParam With {.Path = "", .Switch = "--profile", .Text = "profile", .Help = "Bitstream profile number to use"},
                    New OptionParam With {.Path = "", .Switch = "--stereo-mode", .Text = "stereo-mode", .Options = {"mono", "left-right", "bottom-top", "top-bottom", "right-left"}, .Help = "Stereo 3D video format"},
                    New StringParam With {.Path = "", .Switch = "--error-resilient", .Text = "error-resilient", .Help = "error resiliency features"},
                    New BoolParam With {.Path = "", .Switch = "--test-16bit-internal", .Text = "test-16bit-internal", .Help = "Force use of 16 bit internal buffer"},
                    New NumParam With {.Path = "", .Switch = "--lag-in-frames", .Text = "lag-in-frames", .Help = "Max number of frames to lag"},
                    New NumParam With {.Path = "", .Switch = "--drop-frame", .Text = "drop-frame", .Help = "Temporal resampling threshold (buf %)"},
                    New OptionParam With {.Path = "", .Switch = "--end-usage", .Text = "end-usage", .Options = {"vbr", "cbr", "cq", "q"}, .Help = "Rate control mode"},
                    New NumParam With {.Path = "", .Switch = "--min-q", .Text = "min-q", .Help = "Minimum (best) quantizer"},
                    New NumParam With {.Path = "", .Switch = "--max-q", .Text = "max-q", .Help = "Maximum (worst) quantizer"},
                    New NumParam With {.Path = "", .Switch = "--buf-sz", .Text = "buf-sz", .Help = "Client buffer size (ms)"},
                    New NumParam With {.Path = "", .Switch = "--buf-initial-sz", .Text = "buf-initial-sz", .Help = "Client initial buffer size (ms)"},
                    New NumParam With {.Path = "", .Switch = "--buf-optimal-sz", .Text = "buf-optimal-sz", .Help = "Client optimal buffer size (ms)"},
                    New NumParam With {.Path = "", .Switch = "--kf-min-dist", .Text = "kf-min-dist", .Help = "Minimum keyframe interval (frames)"},
                    New NumParam With {.Path = "", .Switch = "--kf-max-dist", .Text = "kf-max-dist", .Help = "Maximum keyframe interval (frames)"},
                    New BoolParam With {.Path = "", .Switch = "--disable-kf", .Text = "disable-kf", .Help = "Disable keyframe placement"},
                    New NumParam With {.Path = "", .Switch = "--auto-alt-ref", .Text = "auto-alt-ref", .Help = "Enable automatic alt reference frames"},
                    New NumParam With {.Path = "", .Switch = "--sharpness", .Text = "sharpness", .MinMaxStep = {0, 7, 1}, .Help = "Loop filter sharpness (0..7)"},
                    New NumParam With {.Path = "", .Switch = "--static-thresh", .Text = "static-thresh", .Help = "Motion detection threshold"},
                    New NumParam With {.Path = "", .Switch = "--tile-columns", .Text = "tile-columns", .Help = "Number of tile columns to use, log2"},
                    New NumParam With {.Path = "", .Switch = "--tile-rows", .Text = "tile-rows", .Help = "Number of tile rows to use, log2 (set to 0 while threads > 1)"},
                    New StringParam With {.Path = "", .Switch = "--tile-loopfilter", .Text = "tile-loopfilter", .Help = "Enable loop filter across tile boundary"},
                    New NumParam With {.Path = "", .Switch = "--arnr-maxframes", .Text = "arnr-maxframes", .MinMaxStep = {0, 15, 1}, .Help = "AltRef max frames (0..15)"},
                    New NumParam With {.Path = "", .Switch = "--arnr-strength", .Text = "arnr-strength", .MinMaxStep = {0, 6, 1}, .Help = "AltRef filter strength (0..6)"},
                    New NumParam With {.Path = "", .Switch = "--cq-level", .Text = "cq-level", .Help = "Constant/Constrained Quality level"},
                    New OptionParam With {.Path = "", .Switch = "--frame-parallel", .Text = "frame-parallel", .IntegerValue = True, .Options = {"false", "true"}, .Help = "Enable frame parallel decodability features"},
                    New OptionParam With {.Path = "", .Switch = "--frame-boost", .Text = "frame-boost", .IntegerValue = True, .Options = {"off", "on"}, .Help = "Enable frame periodic boost"},
                    New NumParam With {.Path = "", .Switch = "--noise-sensitivity", .Text = "noise-sensitivity", .Help = "Noise sensitivity (frames to blur)"},
                    New OptionParam With {.Path = "", .Switch = "--tune-content", .Text = "tune-content", .Options = {"default", "screen"}, .Help = "Tune content type"},
                    New NumParam With {.Path = "", .Switch = "--min-gf-interval", .Text = "min-gf-interval", .Help = "min gf/arf frame interval (default 0, indicating in-built behavior)"},
                    New NumParam With {.Path = "", .Switch = "--max-gf-interval", .Text = "max-gf-interval", .Help = "max gf/arf frame interval (default 0, indicating in-built behavior)"},
                    New NumParam With {.Path = "", .Switch = "--num-tile-groups", .Text = "num-tile-groups", .Help = "Maximum number of tile groups, default is 1"},
                    New NumParam With {.Path = "", .Switch = "--mtu-size", .Text = "mtu-size", .Help = "MTU size for a tile group, default 0 (no MTU targeting), overrides max number of tile groups"},
                    New NumParam With {.Path = "", .Switch = "--disable-tempmv", .Text = "disable-tempmv", .Help = "Disable temporal mv prediction (default is 0)"},
                    New NumParam With {.Path = "", .Switch = "--timebase", .Text = "timebase", .Help = "Output timestamp precision (fractional seconds)"})
            End If

            Return ItemsValue
        End Get
    End Property

    Private TabCount As Integer = 1
    Private ControlCount As Integer

    Shadows Sub Add(ParamArray items As CommandLineParam())
        For Each i In items
            If i.Path = "" Then
                ControlCount += 1

                If ControlCount > 10 Then
                    TabCount += 1
                    ControlCount = 0
                End If

                i.Path = "Misc " & TabCount
            End If

            ItemsValue.Add(i)
        Next
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

        Dim sb As New StringBuilder

        If includePaths AndAlso includeExecutable Then
            If p.Script.Engine = ScriptEngine.VapourSynth Then
                sb.Append(Package.vspipe.Path.Escape + " " + script.Path.Escape + " - --y4m | " + Package.AOMEnc.Path.Escape + " -")
            Else
                sb.Append(Package.ffmpeg.Path.Escape + " -i " + script.Path.Escape + " -f yuv4mpegpipe -loglevel error -hide_banner - | " + Package.AOMEnc.Path.Escape + " -")
            End If
        End If

        Select Case Mode.Value
            Case AV1RateMode.TwoPass
                sb.Append(" --passes=2 --pass=" & pass)
            Case AV1RateMode.OnePass
                sb.Append(" --passes=1")
        End Select

        sb.Append(" --target-bitrate=" & p.VideoBitrate)
        Dim q = From i In Items Where i.GetArgs <> ""
        If q.Count > 0 Then sb.Append(" " + q.Select(Function(item) item.GetArgs).Join(" "))

        If includePaths Then
            If Mode.Value = AV1RateMode.TwoPass Then sb.Append(" --fpf=" + (p.TempDir + p.Name + ".txt").Escape)
            sb.Append(" -o " + targetPath.Escape)
        End If

        Return Macro.Expand(sb.ToString.Trim.FixBreak.Replace(BR, " "))
    End Function

    Public Overrides Function GetPackage() As Package
        Return Package.AOMEnc
    End Function
End Class

Public Enum AV1RateMode
    OnePass
    TwoPass
End Enum