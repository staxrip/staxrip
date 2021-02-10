
Imports System.Text

Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class aomenc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Sub New()
        Name = "AV1 | aomenc"
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
        Encode("Video encoding pass 1", GetArgs(1, 0, 0, Nothing, p.Script), s.ProcessPriority)

        If Params.Passes.Value = 1 Then
            Encode("Video encoding pass 2", GetArgs(2, 0, 0, Nothing, p.Script), s.ProcessPriority)
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(passName As String, commandLine As String, priority As ProcessPriorityClass)
        If Not CanChunkEncode() Then
            p.Script.Synchronize(False, True, False, TextEncoding.EncodingOfProcess)
        End If

        Using proc As New Proc
            proc.Package = Package.aomenc
            proc.Encoding = Encoding.UTF8
            proc.Header = passName
            proc.Priority = priority
            proc.FrameCount = p.Script.GetFrameCount
            proc.SkipString = "[ETA"
            proc.File = "cmd.exe"
            proc.Arguments = "/S /C """ + commandLine + """"
            proc.Start()
        End Using
    End Sub

    Overrides Function CanChunkEncode() As Boolean
        Return CInt(Params.Chunks.Value) > 1
    End Function

    Overrides Function GetChunks() As Integer
        Return CInt(Params.Chunks.Value)
    End Function

    Overrides Function GetChunkEncodeActions() As List(Of Action)
        Dim maxFrame = If(Params.Decoder.Value = 0, p.Script.GetFrameCount(), p.SourceFrames)
        Dim chunkCount = CInt(Params.Chunks.Value)
        Dim startFrame = CInt(Params.Skip.Value)
        Dim length = If(CInt(Params.Limit.Value) > 0, CInt(Params.Limit.Value), maxFrame - startFrame)
        Dim endFrame = Math.Min(startFrame + length - 1, maxFrame)
        Dim chunkLength = length \ chunkCount
        Dim ret As New List(Of Action)

        For chunk = 0 To chunkCount - 1
            Dim name = ""
            Dim chunkStart = startFrame + (chunk * chunkLength)
            Dim chunkEnd = If(chunk <> chunkCount - 1, chunkStart + (chunkLength - 1), endFrame)

            If chunk > 0 Then
                name = "_chunk" & (chunk + 1)
            End If

            If Params.Passes.Value = 1 Then
                ret.Add(Sub()
                            Encode("Video encoding pass 1" + name.Replace("_chunk", " chunk "),
                                   GetArgs(1, chunkStart, chunkEnd, name, p.Script), s.ProcessPriority)
                            Encode("Video encoding pass 2" + name.Replace("_chunk", " chunk "),
                                   GetArgs(2, chunkStart, chunkEnd, name, p.Script), s.ProcessPriority)
                        End Sub)
            Else
                ret.Add(Sub() Encode("Video encoding" + name.Replace("_chunk", " chunk "),
                    GetArgs(1, chunkStart, chunkEnd, name, p.Script), s.ProcessPriority))
            End If
        Next

        Return ret
    End Function

    Overloads Function GetArgs(
            pass As Integer,
            startFrame As Integer,
            endFrame As Integer,
            chunkName As String,
            script As VideoScript,
            Optional includePaths As Boolean = True) As String

        Return Params.GetArgs(pass, startFrame, endFrame, chunkName, script, OutputPath.DirAndBase + OutputExtFull, includePaths, True)
    End Function

    Overrides Function GetMenu() As MenuList
        Dim menuList As New MenuList
        menuList.Add("Encoder Options", AddressOf ShowConfigDialog)
        menuList.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return menuList
    End Function

    Overrides Sub ShowConfigDialog()
        Dim newParams As New AV1Params
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(store)

        Using form As New CommandLineForm(newParams)
            Dim saveProfileAction = Sub()
                                        Dim enc = ObjectHelp.GetCopy(Of aomenc)(Me)
                                        Dim params2 As New AV1Params
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            MenuItemEx.Add(form.cms.Items, "Save Profile...", saveProfileAction).SetImage(Symbol.Save)

            If form.ShowDialog() = DialogResult.OK Then
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides Property QualityMode() As Boolean
        Get
            Return Params.RateMode.ValueText.EqualsAny("cq", "q")
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
        Title = "aomenc Options"
        Separator = "="
    End Sub


    Property Chunks As New NumParam With {
        .Text = "Chunks",
        .Init = 1,
        .Config = {1, 16}}

    Property CqLevel As New NumParam With {
        .Path = "Rate Control 1",  '.Path = "AV1 Specific 2",    moved to "Rate Control 1" for better usage
        .HelpSwitch = "--cq-level",
        .Text = "CQ Level",
        .AlwaysOn = True,
        .Init = 24,
        .VisibleFunc = Function() RateMode.Value = 2 OrElse RateMode.Value = 3}

    Property Decoder As New OptionParam With {
        .Text = "Decoder",
        .Options = {"AviSynth/VapourSynth", "QSVEnc (Intel)", "ffmpeg (Intel)", "ffmpeg (DXVA2)"},
        .Values = {"script", "qs", "ffqsv", "ffdxva"}}

    Property EnableRestoration As New OptionParam With {
        .Path = "AV1 Specific 1",
        .Switch = "--enable-restoration",
        .Text = "Restoration",
        .IntegerValue = True,
        .Options = {"Off (default in Realtime mode)", "On (default in Non-realtime mode)"}}

    Property Passes As New OptionParam With {
        .Path = "Basic",
        .Name = "Passes",
        .Text = "Passes",
        .Switches = {"--passes", "--pass"},
        .Options = {"One Pass", "Two Pass"},
        .Init = 1}

    Property PipingToolAVS As New OptionParam With {
        .Text = "Pipe",
        .Name = "PipingToolAVS",
        .VisibleFunc = Function() p.Script.IsAviSynth AndAlso Decoder.Value = 0,
        .Options = {"Automatic", "avs2pipemod", "ffmpeg"}}

    Property PipingToolVS As New OptionParam With {
        .Text = "Pipe",
        .Name = "PipingToolVS",
        .VisibleFunc = Function() p.Script.IsVapourSynth AndAlso Decoder.Value = 0,
        .Options = {"Automatic", "vspipe", "ffmpeg"}}

    Property RateMode As New OptionParam With {
        .Path = "Basic",
        .Switch = "--end-usage",
        .Text = "Rate Mode",
        .Options = {"VBR", "CBR", "CQ (Constrained Quality)", "Q (Constant Quality)"},
        .Values = {"vbr", "cbr", "cq", "q"},
        .AlwaysOn = True,
        .Init = 3}

    Property Skip As New NumParam With {
        .Switch = "--skip",
        .Text = "Skip first n frames"}

    Property Limit As New NumParam With {
        .Switch = "--limit",
        .Text = "Stop after n frames"}

    Property TargetBitrate As New NumParam With {
        .HelpSwitch = "--target-bitrate",
        .Text = "Target Bitrate",
        .VisibleFunc = Function() RateMode.Value <> 2 AndAlso RateMode.Value <> 3}

    Property WebM As New OptionParam With {
        .Path = "Basic",
        .Switch = "--webm",
        .Text = "Output WEBM",
        .IntegerValue = True,
        .Options = {"0 - Disabled", "1 - Enabled (default when WebM IO is enabled)"}}

    Property CustomFirstPass As New StringParam With {
        .Text = "Custom 1st pass",
        .Quotes = QuotesMode.Never,
        .InitAction = Sub(tb)
                          tb.Edit.Expand = True
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = New Font("Consolas", 10 * s.UIScaleFactor)
                      End Sub}

    Property CustomSecondPass As New StringParam With {
        .Text = "Custom 2nd pass",
        .Quotes = QuotesMode.Never,
        .InitAction = Sub(tb)
                          tb.Edit.Expand = True
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = New Font("Consolas", 10 * s.UIScaleFactor)
                      End Sub}



    Overrides ReadOnly Property Items As List(Of CommandLineParam)
        Get
            If ItemsValue Is Nothing Then
                ItemsValue = New List(Of CommandLineParam)

                'New BoolParam With {.Switch = "--quiet", .Text = "Do not print encode progress"},
                'New BoolParam With {.Switch = "--disable-warning-prompt", .Text = "Disable Warning Prompt"},
                Add("Basic",
                    New StringParam With {.Switch = "--cfg", .Text = "Config File", .Quotes = QuotesMode.Auto, .BrowseFile = True},
                    New BoolParam With {.Switch = "--debug", .Text = "Debug"},
                    New StringParam With {.Switch = "--codec", .Text = "Codec"},
                    Passes, Skip, Limit,
                    New BoolParam With {.Switch = "--good", .Text = "Good Quality Deadline"},
                    New BoolParam With {.Switch = "--rt", .Text = "Realtime Quality Deadline"},
                    New BoolParam With {.Switch = "--verbose", .Text = "Show encoder parameters", .Init = True},
                    New OptionParam With {.Switch = "--psnr", .Text = "Show PSNR in status line", .Init = 1, .IntegerValue = True, .Options = {"0 - Disable PSNR status line display", "1 - PSNR calculated using input bit-depth (default)", "2 - PSNR calculated using stream bit-depth"}},
                    New NumParam With {.Switch = "--q-hist", .Text = "Q-Hist (n-buckets)"},
                    New NumParam With {.Switch = "--rate-hist", .Text = "Rate Hist (n-buckets)"},
                    New BoolParam With {.Switch = "--disable-warnings", .Text = "Disable Warnings"},
                    New OptionParam With {.Switch = "--test-decode", .Text = "Test Decode", .Options = {"Off", "Fatal", "Warn"}})

                Add("Input/Output",
                    Decoder, PipingToolAVS, PipingToolVS, Chunks,
                    New OptionParam With {.Switch = "--input-bit-depth", .Text = "Input Bit Depth", .Options = {"Automatic", "8", "10", "12"}},
                    New OptionParam With {.Switch = "--bit-depth", .Text = "Bit Depth", .Options = {"8", "10", "12"}, .Init = 1, .AlwaysOn = True},
                    WebM,
                    New BoolParam With {.Switch = "--ivf", .Text = "Output IVF"},
                    New BoolParam With {.Switch = "--obu", .Text = "Output OBU"})

                'New OptionParam With {.Switch = "--profile", .Text = "Profile", .IntegerValue = True, .Options = {"Main", "High", "Professional"}},
                Add("Encoder Global 1",
                    New BoolParam With {.Switch = "--yv12", .Text = "YV12"},
                    New BoolParam With {.Switch = "--i420", .Text = "I420", .Init = True},
                    New BoolParam With {.Switch = "--i422", .Text = "I422"},
                    New BoolParam With {.Switch = "--i444", .Text = "I444"},
                    New NumParam With {.Switch = "--usage", .Text = "Usage"},
                    New NumParam With {.Switch = "--threads", .Text = "Threads", .Init = 32, .AlwaysOn = True},
                    New NumParam With {.Switch = "--profile", .Text = "Profile"},
                    New NumParam With {.Switch = "--width", .Text = "Width"},
                    New NumParam With {.Switch = "--height", .Text = "Height"})

                Add("Encoder Global 2",
                    New NumParam With {.Switch = "--forced_max_frame_width", .Text = "Force Width"},
                    New NumParam With {.Switch = "--forced_max_frame_height", .Text = "Force Height"},
                    New OptionParam With {.Switch = "--stereo-mode", .Text = "Stereo Mode", .Options = {"Disabled", "Mono", "Left-Right", "Bottom-Top", "Top-Bottom", "Right-Left"}},
                    New StringParam With {.Switch = "--timebase", .Text = "Timebase precision"},
                    New StringParam With {.Switch = "--fps", .Text = "Frame Rate"},
                    New StringParam With {.Switch = "--global-error-resilient", .Text = "Global Error Resilient"},
                    New NumParam With {.Switch = "--lag-in-frames", .Text = "Lag In Frames", .Init = 25},
                    New OptionParam With {.Switch = "--large-scale-tile", .Text = "Large Scale Tile Coding", .IntegerValue = True, .Options = {"Off", "On"}},
                    New BoolParam With {.Switch = "--monochrome", .Text = "Monochrome"},
                    New BoolParam With {.Switch = "--full-still-picture-hdr", .Text = "Full header for still picture"},
                    New BoolParam With {.Switch = "--use-16bit-internal", .Text = "Force 16-bit pipeline"})

                Add("Rate Control 1",
                    New NumParam With {.Switch = "--drop-frame", .Text = "Drop Frame"},
                    New NumParam With {.Switch = "--resize-mode", .Text = "Resize Mode"},
                    New NumParam With {.Switch = "--resize-denominator", .Text = "Resize Denominator"},
                    New NumParam With {.Switch = "--resize-kf-denominator", .Text = "Resize KF Denominator"},
                    New NumParam With {.Switch = "--superres-mode", .Text = "SuperRes"},
                    New NumParam With {.Switch = "--superres-denominator", .Text = "SuperRes Denominator"},
                    New NumParam With {.Switch = "--superres-kf-denominator", .Text = "SuperRes KF Denominator"},
                    New NumParam With {.Switch = "--superres-qthresh", .Text = "SuperRes qThresh"},
                    New NumParam With {.Switch = "--superres-kf-qthresh", .Text = "SuperRes KF qThresh"},
                    RateMode, CqLevel, TargetBitrate,
                    New NumParam With {.Switch = "--min-q", .Text = "Minimum Quantizer"},
                    New NumParam With {.Switch = "--max-q", .Text = "Maximum Quantizer"})

                Add("Rate Control 2",
                    New NumParam With {.Switch = "--undershoot-pct", .Text = "Datarate undershoot (min) target (%)"},
                    New NumParam With {.Switch = "--overshoot-pct", .Text = "Datarate overshoot (max) target (%)"},
                    New NumParam With {.Switch = "--buf-sz", .Text = "Client buffer size"},
                    New NumParam With {.Switch = "--buf-initial-sz", .Text = "Client initial buffer size (ms)"},
                    New NumParam With {.Switch = "--buf-optimal-sz", .Text = "Client optimal buffer size (ms)"},
                    New NumParam With {.Switch = "--bias-pct", .Text = "CBR/VBR bias (0=CBR, 100=VBR)", .Config = {0, 100}},
                    New NumParam With {.Switch = "--minsection-pct", .Text = "GOP min bitrate (% of target)"},
                    New NumParam With {.Switch = "--maxsection-pct", .Text = "GOP max bitrate (% of target)"})

                Add("Keyframe Placement",
                    New NumParam With {.Switch = "--enable-fwd-kf", .Text = "Enable forward reference keyframes"},
                    New NumParam With {.Switch = "--kf-min-dist", .Text = "Min keyframe interval", .Init = 0},
                    New NumParam With {.Switch = "--kf-max-dist", .Text = "Max keyframe interval", .Init = 120},
                    New BoolParam With {.Switch = "--disable-kf", .Text = "Disable keyframe placement"})

                'New OptionParam With {.Switch = "--row-mt", .Text = "Multi-Threading", .IntegerValue = True, .Options = {"On", "Off"}},
                'New BoolParam With {.Switch = "--enable-tpl-model", .Text = "TPL model", .Init = True, .IntegerValue = True},
                Add("AV1 Specific 1",
                    New OptionParam With {.Switch = "--cpu-used", .Text = "CPU Used", .Value = 4, .AlwaysOn = True, .IntegerValue = True, .Options = {"0 - Slowest", "1 - Very Slow", "2 - Slower", "3 - Slow", "4 - Medium", "5 - Fast", "6 - Faster", "7 - Very Fast", "8 - Ultra Fast", "9 - Fastest"}},
                    New NumParam With {.Switch = "--auto-alt-ref", .Text = "Auto Alt Ref", .Init = 1, .AlwaysOn = True},
                    New NumParam With {.Switch = "--sharpness", .Text = "Sharpness", .Init = 0, .Config = {0, 7}},
                    New NumParam With {.Switch = "--static-thresh", .Text = "Static Thresh", .AlwaysOn = True},
                    New BoolParam With {.Switch = "--row-mt", .Text = "Multi-Threading", .Init = True, .IntegerValue = True},
                    New NumParam With {.Switch = "--tile-columns", .Text = "Tile Columns", .Init = 2, .AlwaysOn = True},
                    New NumParam With {.Switch = "--tile-rows", .Text = "Tile Rows", .Init = 1, .AlwaysOn = True},
                    New OptionParam With {.Switch = "--enable-tpl-model", .Text = "TPL model", .Value = 1, .AlwaysOn = True, .IntegerValue = True, .Options = {"0 - Off", "1 - Backward source based"}},
                    New OptionParam With {.Switch = "--enable-keyframe-filtering", .Text = "Keyframe Filtering", .Init = 1, .IntegerValue = True, .Options = {"0 - No filter", "1 - Filter without overlay (default)", "2 - Filter with overlay"}},
                    New NumParam With {.Switch = "--arnr-maxframes", .Text = "ARNR Max Frames", .Config = {0, 15}},
                    New NumParam With {.Switch = "--arnr-strength", .Text = "ARNR Filter Strength", .Config = {0, 6}})

                'CqLevel) moved to "Rate Control 1" for better usage
                'New OptionParam With {.Switch = "--enable-cdef", .Text = "Enable CDEF", .Init = 1, .IntegerValue = True, .Options = {"Off", "On"}})
                Add("AV1 Specific 2",
                    New OptionParam With {.Switch = "--tune", .Text = "Tune", .Options = {"psnr", "ssim", "vmaf_with_preprocessing", "vmaf_without_preprocessing", "vmaf", "vmaf_neg"}},
                    New NumParam With {.Switch = "--max-intra-rate", .Text = "Max Intra Rate"},
                    New NumParam With {.Switch = "--max-inter-rate", .Text = "Max Inter Rate"},
                    New NumParam With {.Switch = "--gf-cbr-boost", .Text = "GF CBR Boost"},
                    New BoolParam With {.Switch = "--lossless", .Text = "Lossless", .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-cdef", .Text = "CDEF", .Init = True, .IntegerValue = True},
                    EnableRestoration,
                    New BoolParam With {.Switch = "--enable-rect-partitions", .Text = "Rectangular partitions", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-ab-partitions", .Text = "AB partitions", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-1to4-partitions", .Text = "14 And 41 partitions", .Init = True, .IntegerValue = True},
                    New OptionParam With {.Switch = "--min-partition-size", .Text = "Min partition size", .Value = 0, .AlwaysOn = False, .IntegerValue = True, .Options = {"0 - Disabled", "4 - 4x4", "8 - 8x8", "16 - 16x16", "32 - 32x32", "64 - 64x64", "128 - 128x128"}, .Values = {"0", "4", "8", "16", "32", "64", "128"}},
                    New OptionParam With {.Switch = "--max-partition-size", .Text = "Max partition size", .Value = 0, .AlwaysOn = False, .IntegerValue = True, .Options = {"0 - Disabled", "4 - 4x4", "8 - 8x8", "16 - 16x16", "32 - 32x32", "64 - 64x64", "128 - 128x128"}, .Values = {"0", "4", "8", "16", "32", "64", "128"}})

                Add("AV1 Specific 3",
                    New BoolParam With {.Switch = "--enable-dual-filter", .Text = "Dual filter", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-chroma-deltaq", .Text = "Chroma delta quant", .Init = False, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-intra-edge-filter", .Text = "Intra edge filtering", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-order-hint", .Text = "Order hint", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-tx64", .Text = "64-pt transform", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-flip-idtx", .Text = "Extended transform type", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-rect-tx", .Text = "Rectangular transform", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-dist-wtd-comp", .Text = "Distance-weighted compound", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-masked-comp", .Text = "Masked (wedge/diff-wtd) compound", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-onesided-comp", .Text = "One sided compound", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-interintra-comp", .Text = "Interintra compound", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-smooth-interintra", .Text = "Smooth interintra mode", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-diff-wtd-comp", .Text = "Difference-weighted compound", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-interinter-wedge", .Text = "Interinter wedge compound", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-interintra-wedge", .Text = "Interintra wedge compound", .Init = True, .IntegerValue = True})

                Add("AV1 Specific 4",
                    New BoolParam With {.Switch = "--enable-global-motion", .Text = "Global motion", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-warped-motion", .Text = "Local warped motion", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-filter-intra", .Text = "Filter intra prediction mode", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-smooth-intra", .Text = "Smooth intra prediction modes", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-paeth-intra", .Text = "Paeth intra prediction mode", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-cfl-intra", .Text = "Chroma from luma intra prediction mode", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--force-video-mode", .Text = "Force video mode", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-obmc", .Text = "OBMC", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-overlay", .Text = "Coding overlay frames", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-palette", .Text = "Palette prediction mode", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-intrabc", .Text = "Intra block copy prediction mode", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-angle-delta", .Text = "Intra angle delta", .Init = True, .IntegerValue = True},
                    New OptionParam With {.Switch = "--disable-trellis-quant", .Text = "Disable Trellis Quant", .IntegerValue = True, .Options = {"0 - False", "1 - True", "2 - True for RD Search", "3 - True for estimare yrd search (default)"}},
                    New BoolParam With {.Switch = "--enable-qm", .Text = "Enable QM", .Init = False, .IntegerValue = True},
                    New NumParam With {.Switch = "--qm-min", .Text = "Min QM Flatness", .Init = 8, .Config = {0, 15}},
                    New NumParam With {.Switch = "--qm-max", .Text = "Max QM Flatness", .Init = 15, .Config = {0, 15}})

                Add("AV1 Specific 5",
                    New BoolParam With {.Switch = "--reduced-tx-type-set", .Text = "Reduced set of transform types"},
                    New BoolParam With {.Switch = "--use-intra-dct-only", .Text = "DCT only for INTRA modes"},
                    New BoolParam With {.Switch = "--use-inter-dct-only", .Text = "DCT only for INTER modes"},
                    New BoolParam With {.Switch = "--use-intra-default-tx-only", .Text = "Default-transform only for INTRA modes"},
                    New BoolParam With {.Switch = "--quant-b-adapt", .Text = "Adaptive quantize_b"},
                    New OptionParam With {.Switch = "--coeff-cost-upd-freq", .Text = "Update freq for coeff costs", .IntegerValue = True, .Init = 2, .AlwaysOn = True, .Options = {"0 - SB", "1 - SB Row per Tile", "2 - Tile", "3 - Off"}},
                    New OptionParam With {.Switch = "--mode-cost-upd-freq", .Text = "Update freq for mode costs", .IntegerValue = True, .Init = 2, .AlwaysOn = True, .Options = {"0 - SB", "1 - SB Row per Tile", "2 - Tile", "3 - Off"}},
                    New OptionParam With {.Switch = "--mv-cost-upd-freq", .Text = "Update freq for mv costs", .IntegerValue = True, .Init = 2, .AlwaysOn = True, .Options = {"0 - SB", "1 - SB Row per Tile", "2 - Tile", "3 - Off"}},
                    New BoolParam With {.Switch = "--frame-parallel", .Text = "Frame Parallel", .Init = False, .IntegerValue = True},
                    New BoolParam With {.Switch = "--error-resilient", .Text = "Error Resilient", .Init = False, .IntegerValue = True},
                    New OptionParam With {.Switch = "--aq-mode", .Text = "AQ Mode", .IntegerValue = True, .Options = {"Disabled", "Variance", "Complexity", "Cyclic Refresh"}},
                    New OptionParam With {.Switch = "--deltaq-mode", .Text = "Delta QIndex Mode", .IntegerValue = True, .Options = {"Disabled", "Deltaq Objective (default)", "Deltaq perceptual (requires enable-tpl-model)"}},
                    New BoolParam With {.Switch = "--delta-lf-mode", .Text = "Delta-lf-Mode", .Init = False, .IntegerValue = True},
                    New BoolParam With {.Switch = "--frame-boost", .Text = "Enable frame periodic boost", .Init = False, .IntegerValue = True},
                    New NumParam With {.Switch = "--noise-sensitivity", .Text = "Noise Sensitivity"},
                    New OptionParam With {.Switch = "--tune-content", .Text = "Tune Content", .Options = {"Default", "Screen"}})

                Add("AV1 Specific 6",
                    New OptionParam With {.Switch = "--cdf-update-mode", .Text = "CDF Update", .IntegerValue = True, .Options = {"No Update", "Update CDF on all frames(default)", "Selectively Update CDF on some frames"}, .Init = 1},
                    New OptionParam With {.Switch = "--color-primaries", .Text = "Color Primaries", .Options = {"unspecified", "BT2020", "BT601", "BT709", "BT470M", "BT470BG", "SMPTE170", "XYZ", "SMPTE240", "SMPTE431", "SMPTE432", "FILM", "EBU3213"}},
                    New OptionParam With {.Switch = "--transfer-characteristics", .Text = "Transfer Characteristics", .Options = {"unspecified", "BT709", "BT470M", "BT470BG", "BT601", "SMPTE240", "LIN", "LOG100", "LOG100SQ 10", "IEC 61966", "BT 1361", "SRGB", "BT2020-10bit", "BT2020-12bit", "SMPTE2084", "HLG", "SMPTE428"}},
                    New OptionParam With {.Switch = "--matrix-coefficients", .Text = "Matrix Coefficients", .Options = {" unspecified", "identity", "BT2020NC", "BT2020CL", "BT601", "FCC73", "BT709", "BT470BG", "SMPTE2085", "YCGCO", "SMPTE240", "ICTCP", "CHROMNCL", "CHROMCL"}},
                    New OptionParam With {.Switch = "--chroma-sample-position", .Text = "Chroma Sample Position", .Options = {"Unknown", "Vertical", "Colocated"}},
                    New NumParam With {.Switch = "--min-gf-interval", .Text = "Min GF Interval"},
                    New NumParam With {.Switch = "--max-gf-interval", .Text = "Max GF Interval"},
                    New NumParam With {.Switch = "--gf-min-pyr-height", .Text = "Min height for GF group pyramid structure", .Init = 0, .Config = {0, 5}},
                    New NumParam With {.Switch = "--gf-max-pyr-height", .Text = "Max height for GF group pyramid structure", .Init = 5, .Config = {0, 5}},
                    New OptionParam With {.Switch = "--sb-size", .Text = "Superblock size", .Options = {"Dynamic", "64", "128"}},
                    New NumParam With {.Switch = "--num-tile-groups", .Text = "Num Tile Groups", .Init = 1},
                    New NumParam With {.Switch = "--mtu-size", .Text = "MTU Size"},
                    New OptionParam With {.Switch = "--timing-info", .Text = "Timing info", .Options = {"Unspecified", "Constant", "Model"}},
                    New OptionParam With {.Switch = "--film-grain-test", .Text = "Film grain test vectors", .IntegerValue = True, .Options = {"None (default)", "test-1", "test-2", "test-3", "test-4", "test-5", "test-6", "test-7", "test-8", "test-9", "test-10", "test-11", "test-12", "test-13", "test-14", "test-15", "test-16"}},
                    New StringParam With {.Switch = "--film-grain-table", .Text = "Film Grain Table", .Quotes = QuotesMode.Auto, .BrowseFile = True},
                    New NumParam With {.Switch = "--denoise-noise-level", .Text = "Denoise Level", .Config = {0, 50}},
                    New NumParam With {.Switch = "--denoise-block-size", .Text = "Denoise Block Size", .Config = {0, 64}, .Init = 32})

                Add("AV1 Specific 7",
                    New NumParam With {.Switch = "--max-reference-frames", .Text = "Max ref frames per frame", .Config = {3, 7}, .Init = 7},
                    New BoolParam With {.Switch = "--reduced-reference-set", .Text = "Rreduced set of refs", .Init = False, .IntegerValue = True},
                    New NumParam With {.Switch = "--enable-ref-frame-mvs", .Text = "Temporal mv prediction", .Init = 1},
                    New StringParam With {.Switch = "--target-seq-level-idx", .Text = "Target sequence level index"},  'Possible values are in the form of "ABxy"(pad leading zeros if less than 4 digits). AB: Operating Point(OP) index; xy: Target level index for the OP. E.g. "0" means target level index 0 for the 0th OP; "1021" means target level index 21 for the 10th OP.
                    New OptionParam With {.Switch = "--set-tier-mask", .Text = "Tier mask", .IntegerValue = True, .Options = {"Main tier (default)", "High tier"}},
                    New NumParam With {.Switch = "--min-cr", .Text = "Minimum compression ratio", .Init = 0},
                    New NumParam With {.Switch = "--vbr-corpus-complexity-lap", .Text = "Average corpus complexity for 1pass VBR", .Config = {0, 10000}, .Init = 0},
                    New NumParam With {.Switch = "--input-chroma-subsampling-x", .Text = "Chroma subsampling x value"},
                    New NumParam With {.Switch = "--input-chroma-subsampling-y", .Text = "Chroma subsampling y value"},
                    New NumParam With {.Switch = "--sframe-dist", .Text = "S-Frame interval"},
                    New NumParam With {.Switch = "--sframe-mode", .Text = "S-Frame insertion mode", .Config = {1, 2}, .Init = 1},
                    New StringParam With {.Switch = "--annexb", .Text = "Save as Annex-B", .Quotes = QuotesMode.Auto})

                Add("Custom",
                    CustomFirstPass, CustomSecondPass)

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

    Overrides Sub ShowHelp(id As String)
        g.ShowCommandLineHelp(Package.aomenc, id)
    End Sub

    Overloads Overrides Function GetCommandLine(
        includePaths As Boolean, includeExecutable As Boolean, Optional pass As Integer = 1) As String

        Return GetArgs(1, 0, 0, Nothing, p.Script, p.VideoEncoder.OutputPath.DirAndBase +
                       p.VideoEncoder.OutputExtFull, includePaths, includeExecutable)
    End Function

    Overloads Function GetArgs(
        pass As Integer,
        startFrame As Integer,
        endFrame As Integer,
        chunkName As String,
        script As VideoScript,
        targetPath As String,
        includePaths As Boolean,
        includeExecutable As Boolean) As String

        Dim sb As New StringBuilder
        Dim pipeTool = If(p.Script.IsAviSynth, PipingToolAVS, PipingToolVS).ValueText.ToLower()
        Dim isSingleChunk = endFrame = 0

        If includePaths AndAlso includeExecutable Then
            Dim isCropped = (p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 AndAlso
                Decoder.ValueText <> "avs" AndAlso p.Script.IsFilterActive("Crop")

            Select Case Decoder.ValueText.ToLower()
                Case "script"
                    Dim pipeString = ""

                    If pipeTool = "automatic" Then
                        If p.Script.IsAviSynth Then
                            pipeTool = "avs2pipemod"
                        Else
                            pipeTool = "vspipe"
                        End If
                    End If

                    Select Case pipeTool
                        Case "avs2pipemod"
                            Dim chunk = If(isSingleChunk, "", $" -trim={startFrame},{endFrame}")
                            Dim dll = If(FrameServerHelp.IsPortable, $" -dll={Package.AviSynth.Path.Escape}", "")
                            pipeString = Package.avs2pipemod.Path.Escape + dll + chunk + " -y4mp " + script.Path.Escape + " | "
                            sb.Append(pipeString + Package.aomenc.Path.Escape)

                            If isSingleChunk Then
                                If Skip.Value > 0 Then
                                    sb.Append($" --skip={Skip.Value}")
                                End If
                                If Limit.Value = 0 Then
                                    sb.Append($" --limit={script.GetFrameCount - Skip.Value}")
                                Else
                                    sb.Append($" --limit={Limit.Value}")
                                End If
                            Else
                                sb.Append($" --limit={endFrame - startFrame + 1}")
                            End If
                        Case "vspipe"
                            Dim chunk = If(isSingleChunk, "", $" --start={startFrame} --end={endFrame}")
                            pipeString = Package.vspipe.Path.Escape + " " + script.Path.Escape + " - --y4m" + chunk + " | "

                            sb.Append(pipeString + Package.aomenc.Path.Escape)
                            If isSingleChunk Then
                                If Skip.Value > 0 Then
                                    sb.Append($" --skip={Skip.Value}")
                                End If
                                If Limit.Value = 0 Then
                                    sb.Append($" --limit={script.GetFrameCount - Skip.Value}")
                                Else
                                    sb.Append($" --limit={Limit.Value}")
                                End If
                            Else
                                sb.Append($" --limit={endFrame - startFrame + 1}")
                            End If
                        Case "ffmpeg"
                            pipeString = Package.ffmpeg.Path.Escape + If(p.Script.IsVapourSynth, " -f vapoursynth", "") + " -i " + script.Path.LongPathPrefix.Escape + " -f yuv4mpegpipe -strict -1 -loglevel fatal -hide_banner - | "

                            sb.Append(pipeString + Package.aomenc.Path.Escape)

                            If isSingleChunk Then
                                If Skip.Value > 0 Then
                                    sb.Append($" --skip={Skip.Value}")
                                End If

                                If Limit.Value = 0 Then
                                    sb.Append($" --limit={script.GetFrameCount - Skip.Value}")
                                Else
                                    sb.Append($" --limit={Limit.Value}")
                                End If
                            Else
                                sb.Append($" --skip={startFrame} --limit={endFrame - startFrame + 1}")
                            End If
                    End Select
                Case "qs"
                    Dim crop = If(isCropped, " --crop " & p.CropLeft & "," & p.CropTop & "," & p.CropRight & "," & p.CropBottom, "")
                    sb.Append(Package.QSVEnc.Path.Escape + " -o - -c raw" + crop + " -i " + p.SourceFile.Escape + " | " + Package.aomenc.Path.Escape)
                    If isSingleChunk Then
                        If Skip.Value > 0 Then
                            sb.Append($" --skip={Skip.Value}")
                        End If
                        If Limit.Value = 0 Then
                            sb.Append($" --limit={p.SourceFrames - Skip.Value}")
                        Else
                            sb.Append($" --limit={Limit.Value}")
                        End If
                    Else
                        sb.Append($" --skip={startFrame} --limit={endFrame - startFrame + 1}")
                    End If
                Case "ffqsv"
                    Dim crop = If(isCropped, $" -vf ""crop={p.SourceWidth - p.CropLeft - p.CropRight}:{p.SourceHeight - p.CropTop - p.CropBottom}:{p.CropLeft}:{p.CropTop}""", "")
                    sb.Append(Package.ffmpeg.Path.Escape + " -threads 1 -hwaccel qsv -i " + p.SourceFile.Escape + " -f yuv4mpegpipe -strict -1" + crop + " -loglevel fatal -hide_banner - | " + Package.aomenc.Path.Escape)
                    If isSingleChunk Then
                        If Skip.Value > 0 Then
                            sb.Append($" --skip={Skip.Value}")
                        End If
                        If Limit.Value = 0 Then
                            sb.Append($" --limit={p.SourceFrames - Skip.Value}")
                        Else
                            sb.Append($" --limit={Limit.Value}")
                        End If
                    Else
                        sb.Append($" --skip={startFrame} --limit={endFrame - startFrame + 1}")
                    End If
                Case "ffdxva"
                    Dim crop = If(isCropped, $" -vf ""crop={p.SourceWidth - p.CropLeft - p.CropRight}:{p.SourceHeight - p.CropTop - p.CropBottom}:{p.CropLeft}:{p.CropTop}""", "")
                    Dim pix_fmt = If(p.SourceVideoBitDepth = 10, "yuv420p10le", "yuv420p")
                    sb.Append(Package.ffmpeg.Path.Escape + " -threads 1 -hwaccel dxva2 -i " + p.SourceFile.Escape +
                              " -f yuv4mpegpipe -pix_fmt " + pix_fmt + " -strict -1" + crop +
                              " -loglevel fatal -hide_banner - | " + Package.aomenc.Path.Escape)
                    If isSingleChunk Then
                        If Skip.Value > 0 Then
                            sb.Append($" --skip={Skip.Value}")
                        End If
                        If Limit.Value = 0 Then
                            sb.Append($" --limit={p.SourceFrames - Skip.Value}")
                        Else
                            sb.Append($" --limit={Limit.Value}")
                        End If
                    Else
                        sb.Append($" --skip={startFrame} --limit={endFrame - startFrame + 1}")
                    End If
            End Select
        End If

        Select Case Passes.Value
            Case 0
                sb.Append(" --passes=1")
                Dim value = CustomFirstPass.Value?.Trim()
                If value <> "" Then
                    sb.Append(" " + value)
                End If
            Case 1
                sb.Append(" --passes=2 --pass=" & pass)
                If pass = 1 Then
                    Dim value = CustomFirstPass.Value?.Trim()
                    If value <> "" Then
                        sb.Append(" " + value)
                    End If
                Else
                    Dim value = CustomSecondPass.Value?.Trim()
                    If value <> "" Then
                        sb.Append(" " + value)
                    End If
                End If
        End Select

        If RateMode.ValueText.EqualsAny("cq", "q") Then
            If Not IsCustom(pass, "--cq-level") Then
                sb.Append(" --cq-level=" & CqLevel.Value)
            End If
        Else
            If Not IsCustom(pass, "--target-bitrate") Then
                If TargetBitrate.Value <> 0 Then
                    sb.Append(" --target-bitrate=" & TargetBitrate.Value)
                Else
                    sb.Append(" --target-bitrate=" & p.VideoBitrate)
                End If
            End If
        End If

        Dim q = From i In Items Where i.GetArgs <> "" AndAlso Not IsCustom(pass, i.Switch)

        If q.Count > 0 Then
            sb.Append(" " + q.Select(Function(item) item.GetArgs).Join(" "))
        End If

        sb.Append(" --disable-warning-prompt")

        If includePaths Then
            'If Decoder.Value <> 0 OrElse pipeTool <> "automatic" Then
            '    sb.Append(" --y4m")
            'End If

            If Passes.Value = 1 Then
                sb.Append(" --fpf=" + (p.TempDir + p.TargetFile.Base + ".txt").Escape)
            End If

            sb.Append(" -o " + (targetPath.DirAndBase + chunkName + targetPath.ExtFull).Escape)
            sb.Append(" -")
        Else
            If Skip.Value > 0 AndAlso Not IsCustom(pass, "--skip") Then
                sb.Append(" --skip=" & Skip.Value)
            End If
            If Limit.Value > 0 AndAlso Not IsCustom(pass, "--limit") Then
                sb.Append(" --limit=" & Limit.Value)
            End If
        End If

        Return Macro.Expand(sb.ToString.Trim.FixBreak.Replace(BR, " "))
    End Function

    Function IsCustom(pass As Integer, switch As String) As Boolean
        If switch = "" Then
            Return False
        End If

        If pass = 1 Then
            If CustomFirstPass.Value?.Contains(switch + " ") OrElse
                CustomFirstPass.Value?.EndsWith(switch) Then

                Return True
            End If
        Else
            If CustomSecondPass.Value?.Contains(switch + " ") OrElse
                CustomSecondPass.Value?.EndsWith(switch) Then

                Return True
            End If
        End If
    End Function


    Public Overrides Function GetPackage() As Package
        Return Package.aomenc
    End Function
End Class
