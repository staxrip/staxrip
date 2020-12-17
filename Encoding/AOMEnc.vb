
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
        Encode("Video encoding using aomenc " + Package.aomenc.Version, GetArgs(1, p.Script))

        If Params.Passes.Value = 1 Then
            Encode("Video encoding second pass using aomenc " + Package.aomenc.Version, GetArgs(2, p.Script))
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(passName As String, commandLine As String)
        p.Script.Synchronize()

        Using proc As New Proc
            proc.Header = passName
            proc.Package = Package.aomenc
            proc.SkipString = "[ETA"
            proc.File = "cmd.exe"
            proc.Arguments = "/S /C """ + commandLine + """"
            proc.Start()
        End Using
    End Sub

    Overloads Function GetArgs(pass As Integer, script As VideoScript, Optional includePaths As Boolean = True) As String
        Return Params.GetArgs(pass, script, OutputPath.DirAndBase + OutputExtFull, includePaths, True)
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
            Return Params.RateMode.OptionText.EqualsAny("CQ", "Q")
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
        .Switches = {"--passes", "--pass", "--target-bitrate"},
        .Options = {"One Pass", "Two Pass"}}

    Property RateMode As New OptionParam With {
        .Path = "Basic",
        .Switch = "--end-usage",
        .Text = "Rate Mode",
        .Options = {"VBR", "CBR", "CQ", "Q"}}

    Property TargetBitrate As New NumParam With {
        .Switch = "--target-bitrate",
        .Text = "Target Bitrate"}

    Property WebM As New OptionParam With {
        .Path = "Basic",
        .Switch = "--webm",
        .Text = "Output WEBM",
        .Options = {"0 - Disabled", "1 - Enabled (default when WebM IO is enabled)"}}

    Property Custom As New StringParam With {
        .Path = "Custom",
        .Text = "Custom",
        .AlwaysOn = True}


    Overrides ReadOnly Property Items As List(Of CommandLineParam)
        Get
            If ItemsValue Is Nothing Then
                ItemsValue = New List(Of CommandLineParam)
                AddParameters()
            End If

            Return ItemsValue
        End Get
    End Property

    Sub AddParameters()
        AddTab("Basic")
        '#############

        Add(New StringParam With {.Switch = "--cfg", .Text = "Config File", .Quotes = QuotesMode.Auto, .BrowseFile = True})
        Add(New BoolParam With {.Switch = "--debug", .Text = "Debug"})
        Add(New StringParam With {.Switch = "--codec", .Text = "Codec"})
        Add(Passes)
        Add(New NumParam With {.Switch = "--skip", .Text = "Skip first n frames"})
        Add(New NumParam With {.Switch = "--limit", .Text = "Stop after n frames"})
        Add(New BoolParam With {.Switch = "--good", .Text = "Good Quality Deadline"})
        Add(New BoolParam With {.Switch = "--rt", .Text = "Realtime Quality Deadline"})
        'Add(New BoolParam With {.Switch = "--quiet", .Text = "Do not print encode progress"})
        Add(New BoolParam With {.Switch = "--verbose", .Text = "Show encoder parameters"})
        Add(New OptionParam With {.Switch = "--psnr", .Text = "Show PSNR in status line", .DefaultValue = 1, .IntegerValue = True, .Options = {"0 - Disable PSNR status line display", "1 - PSNR calculated using input bit-depth (default)", "2 - PSNR calculated using stream bit-depth"}})
        Add(WebM)
        Add(New BoolParam With {.Switch = "--ivf", .Text = "Output IVF"})
        Add(New BoolParam With {.Switch = "--obu", .Text = "Output OBU"})
        Add(New NumParam With {.Switch = "--q-hist", .Text = "Q-Hist (n-buckets)"})
        Add(New NumParam With {.Switch = "--rate-hist", .Text = "Rate Hist (n-buckets)"})
        Add(New BoolParam With {.Switch = "--disable-warnings", .Text = "Disable Warnings"})
        'Add(New BoolParam With {.Switch = "--disable-warning-prompt", .Text = "Disable Warning Prompt"})
        Add(New OptionParam With {.Switch = "--test-decode", .Text = "Test Decode", .Options = {"Off", "Fatal", "Warn"}})




        AddTab("Encoder Global 1")
        '########################

        Add(New BoolParam With {.Switch = "--yv12", .Text = "YV12"})
        Add(New BoolParam With {.Switch = "--i420", .Text = "I420", .Init = True})
        Add(New BoolParam With {.Switch = "--i422", .Text = "I422"})
        Add(New BoolParam With {.Switch = "--i444", .Text = "I444"})

        Add(New NumParam With {.Switch = "--usage", .Text = "Usage"})
        Add(New NumParam With {.Switch = "--threads", .Text = "Threads"})
        'Add(New OptionParam With {.Switch = "--profile", .Text = "Profile", .IntegerValue = True, .Options = {"Main", "High", "Professional"}})
        Add(New NumParam With {.Switch = "--profile", .Text = "Profile"})
        Add(New NumParam With {.Switch = "--width", .Text = "Width"})
        Add(New NumParam With {.Switch = "--height", .Text = "Height"})



        AddTab("Encoder Global 2")
        '########################

        Add(New NumParam With {.Switch = "--forced_max_frame_width", .Text = "Force Width"})
        Add(New NumParam With {.Switch = "--forced_max_frame_height", .Text = "Force Height"})
        Add(New OptionParam With {.Switch = "--stereo-mode", .Text = "Stereo Mode", .Options = {"Disabled", "Mono", "Left-Right", "Bottom-Top", "Top-Bottom", "Right-Left"}})
        Add(New StringParam With {.Switch = "--timebase", .Text = "Timebase precision"})
        Add(New StringParam With {.Switch = "--fps", .Text = "Frame Rate"})

        Add(New StringParam With {.Switch = "--global-error-resilient", .Text = "Global Error Resilient"})
        Add(New OptionParam With {.Switch = "--bit-depth", .Text = "Bit Depth", .Options = {"8", "10", "12"}, .Init = 1, .AlwaysOn = True})
        Add(New NumParam With {.Switch = "--lag-in-frames", .Text = "Lag In Frames"})
        Add(New OptionParam With {.Switch = "--large-scale-tile", .Text = "Large Scale Tile Coding", .IntegerValue = True, .Options = {"Off", "On"}})
        Add(New BoolParam With {.Switch = "--monochrome", .Text = "Monochrome"})
        Add(New BoolParam With {.Switch = "--full-still-picture-hdr", .Text = "Full header for still picture"})
        Add(New BoolParam With {.Switch = "--use-16bit-internal", .Text = "Force 16-bit pipeline"})



        AddTab("Rate Control 1")
        '######################

        Add(New NumParam With {.Switch = "--drop-frame", .Text = "Drop Frame"})
        Add(New NumParam With {.Switch = "--resize-mode", .Text = "Resize Mode"})
        Add(New NumParam With {.Switch = "--resize-denominator", .Text = "Resize Denominator"})
        Add(New NumParam With {.Switch = "--resize-kf-denominator", .Text = "Resize KF Denominator"})

        Add(New NumParam With {.Switch = "--superres-mode", .Text = "SuperRes"})
        Add(New NumParam With {.Switch = "--superres-denominator", .Text = "SuperRes Denominator"})
        Add(New NumParam With {.Switch = "--superres-kf-denominator", .Text = "SuperRes KF Denominator"})
        Add(New NumParam With {.Switch = "--superres-qthresh", .Text = "SuperRes qThresh"})
        Add(New NumParam With {.Switch = "--superres-kf-qthresh", .Text = "SuperRes KF qThresh"})
        Add(RateMode)
        Add(TargetBitrate)
        Add(New NumParam With {.Switch = "--min-q", .Text = "Minimum Quantizer"})
        Add(New NumParam With {.Switch = "--max-q", .Text = "Maximum Quantizer"})



        AddTab("Rate Control 2")
        '######################

        Add(New NumParam With {.Switch = "--undershoot-pct", .Text = "Undershoot PCT"})
        Add(New NumParam With {.Switch = "--overshoot-pct", .Text = "Overshoot PCT"})
        Add(New NumParam With {.Switch = "--buf-sz", .Text = "Buffer Size"})
        Add(New NumParam With {.Switch = "--buf-initial-sz", .Text = "Buf Initial Size"})
        Add(New NumParam With {.Switch = "--buf-optimal-sz", .Text = "Buf Optimal Size"})
        Add(New NumParam With {.Switch = "--bias-pct", .Text = "Bias PCT", .Config = {0, 100}})
        Add(New NumParam With {.Switch = "--minsection-pct", .Text = "Minsection PCT"})
        Add(New NumParam With {.Switch = "--maxsection-pct", .Text = "Maxsection PCT"})



        AddTab("Keyframe Placement")
        '##########################

        Add(New NumParam With {.Switch = "--enable-fwd-kf", .Text = "Enable forward reference keyframes"})
        Add(New NumParam With {.Switch = "--kf-min-dist", .Text = "Min keyframe interval"})
        Add(New NumParam With {.Switch = "--kf-max-dist", .Text = "Max keyframe interval"})
        Add(New BoolParam With {.Switch = "--disable-kf", .Text = "Disable keyframe placement"})



        AddTab("AV1 Specific 1")
        '######################

        Add(New OptionParam With {.Switch = "--cpu-used", .Text = "CPU Used", .Value = 8, .AlwaysOn = True, .IntegerValue = True, .Options = {"0 - Slowest", "1 - Very Slow", "2 - Slower", "3 - Slow", "4 - Medium", "5 - Fast", "6 - Faster", "7 - Very Fast", "8 - Ultra Fast", "9 - Fastest"}})
        Add(New NumParam With {.Switch = "--auto-alt-ref", .Text = "Auto Alt Ref"})
        Add(New NumParam With {.Switch = "--sharpness", .Text = "Sharpness", .Init = 0, .Config = {0, 7}})
        Add(New NumParam With {.Switch = "--static-thresh", .Text = "Static Thresh"})
        'Add(New OptionParam With {.Switch = "--row-mt", .Text = "Multi-Threading", .IntegerValue = True, .Options = {"On", "Off"}})
        Add(New BoolParam With {.Switch = "--row-mt", .Text = "Multi-Threading", .Init = True, .IntegerValue = True})
        Add(New NumParam With {.Switch = "--tile-columns", .Text = "Tile Columns"})
        Add(New NumParam With {.Switch = "--tile-rows", .Text = "Tile Rows"})
        'Add(New BoolParam With {.Switch = "--enable-tpl-model", .Text = "TPL model", .Init = True, .IntegerValue = True})
        Add(New OptionParam With {.Switch = "--enable-tpl-model", .Text = "TPL model", .Value = 1, .AlwaysOn = True, .IntegerValue = True, .Options = {"0 - Off", "1 - Backward source based"}})
        Add(New OptionParam With {.Switch = "--enable-keyframe-filtering", .Text = "Keyframe Filtering", .Value = 1, .AlwaysOn = True, .IntegerValue = True, .Options = {"0 - No filter", "1 - Filter without overlay (default)", "2 - Filter with overlay"}})
        Add(New NumParam With {.Switch = "--arnr-maxframes", .Text = "ARNR Max Frames", .Config = {0, 15}})
        Add(New NumParam With {.Switch = "--arnr-strength", .Text = "ARNR Filter Strength", .Config = {0, 6}})



        AddTab("AV1 Specific 2")
        '######################

        Add(New OptionParam With {.Switch = "--tune", .Text = "Tune", .Options = {"psnr", "ssim", "vmaf_with_preprocessing", "vmaf_without_preprocessing", "vmaf", "vmaf_neg"}})
        Add(New NumParam With {.Switch = "--cq-level", .Text = "CQ Level"})
        Add(New NumParam With {.Switch = "--max-intra-rate", .Text = "Max Intra Rate"})
        Add(New NumParam With {.Switch = "--max-inter-rate", .Text = "Max Inter Rate"})
        Add(New NumParam With {.Switch = "--gf-cbr-boost", .Text = "GF CBR Boost"})
        Add(New BoolParam With {.Switch = "--lossless", .Text = "Lossless", .IntegerValue = True})
        'Add(New OptionParam With {.Switch = "--enable-cdef", .Text = "Enable CDEF", .Init = 1, .IntegerValue = True, .Options = {"Off", "On"}})
        Add(New BoolParam With {.Switch = "--enable-cdef", .Text = "CDEF", .Init = True, .IntegerValue = True})
        Add(EnableRestoration)
        Add(New BoolParam With {.Switch = "--enable-rect-partitions", .Text = "Rectangular partitions", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-ab-partitions", .Text = "AB partitions", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-1to4-partitions", .Text = "14 And 41 partitions", .Init = True, .IntegerValue = True})
        Add(New OptionParam With {.Switch = "--min-partition-size", .Text = "Min partition size", .Value = 0, .AlwaysOn = False, .IntegerValue = True, .Options = {"0 - Disabled", "4 - 4x4", "8 - 8x8", "16 - 16x16", "32 - 32x32", "64 - 64x64", "128 - 128x128"}, .Values = {"0", "4", "8", "16", "32", "64", "128"}})
        Add(New OptionParam With {.Switch = "--max-partition-size", .Text = "Max partition size", .Value = 0, .AlwaysOn = False, .IntegerValue = True, .Options = {"0 - Disabled", "4 - 4x4", "8 - 8x8", "16 - 16x16", "32 - 32x32", "64 - 64x64", "128 - 128x128"}, .Values = {"0", "4", "8", "16", "32", "64", "128"}})



        AddTab("AV1 Specific 3")
        '######################

        Add(New BoolParam With {.Switch = "--enable-dual-filter", .Text = "Dual filter", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-chroma-deltaq", .Text = "Chroma delta quant", .Init = False, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-intra-edge-filter", .Text = "Intra edge filtering", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-order-hint", .Text = "Order hint", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-tx64", .Text = "64-pt transform", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-flip-idtx", .Text = "Extended transform type", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-rect-tx", .Text = "Rectangular transform", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-dist-wtd-comp", .Text = "Distance-weighted compound", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-masked-comp", .Text = "Masked (wedge/diff-wtd) compound", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-onesided-comp", .Text = "One sided compound", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-interintra-comp", .Text = "Interintra compound", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-smooth-interintra", .Text = "Smooth interintra mode", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-diff-wtd-comp", .Text = "Difference-weighted compound", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-interinter-wedge", .Text = "Interinter wedge compound", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-interintra-wedge", .Text = "Interintra wedge compound", .Init = True, .IntegerValue = True})



        AddTab("AV1 Specific 4")
        '######################

        Add(New BoolParam With {.Switch = "--enable-global-motion", .Text = "Global motion", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-warped-motion", .Text = "Local warped motion", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-filter-intra", .Text = "Filter intra prediction mode", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-smooth-intra", .Text = "Smooth intra prediction modes", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-paeth-intra", .Text = "Paeth intra prediction mode", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-cfl-intra", .Text = "Chroma from luma intra prediction mode", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--force-video-mode", .Text = "Force video mode", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-obmc", .Text = "OBMC", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-overlay", .Text = "Coding overlay frames", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-palette", .Text = "Palette prediction mode", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-intrabc", .Text = "Intra block copy prediction mode", .Init = True, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-angle-delta", .Text = "Intra angle delta", .Init = True, .IntegerValue = True})
        Add(New OptionParam With {.Switch = "--disable-trellis-quant", .Text = "Disable Trellis Quant", .IntegerValue = True, .Options = {"0 - False", "1 - True", "2 - True for RD Search", "3 - True for estimare yrd search (default)"}})
        Add(New BoolParam With {.Switch = "--enable-qm", .Text = "Enable QM", .Init = False, .IntegerValue = True})
        Add(New NumParam With {.Switch = "--qm-min", .Text = "Min QM Flatness", .Init = 8, .Config = {0, 15}})
        Add(New NumParam With {.Switch = "--qm-max", .Text = "Max QM Flatness", .Init = 15, .Config = {0, 15}})



        AddTab("AV1 Specific 5")
        '######################

        Add(New BoolParam With {.Switch = "--reduced-tx-type-set", .Text = "Reduced set of transform types"})
        Add(New BoolParam With {.Switch = "--use-intra-dct-only", .Text = "DCT only for INTRA modes"})
        Add(New BoolParam With {.Switch = "--use-inter-dct-only", .Text = "DCT only for INTER modes"})
        Add(New BoolParam With {.Switch = "--use-intra-default-tx-only", .Text = "Default-transform only for INTRA modes"})
        Add(New BoolParam With {.Switch = "--quant-b-adapt", .Text = "Adaptive quantize_b"})
        Add(New OptionParam With {.Switch = "--coeff-cost-upd-freq", .Text = "Update freq for coeff costs", .IntegerValue = True, .Options = {"0 - SB", "1 - SB Row per Tile", "2 - Tile", "3 - Off"}})
        Add(New OptionParam With {.Switch = "--mode-cost-upd-freq", .Text = "Update freq for mode costs", .IntegerValue = True, .Options = {"0 - SB", "1 - SB Row per Tile", "2 - Tile", "3 - Off"}})
        Add(New OptionParam With {.Switch = "--mv-cost-upd-freq", .Text = "Update freq for mv costs", .IntegerValue = True, .Options = {"0 - SB", "1 - SB Row per Tile", "2 - Tile", "3 - Off"}})
        Add(New BoolParam With {.Switch = "--frame-parallel", .Text = "Frame Parallel", .Init = False, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--error-resilient", .Text = "Error Resilient", .Init = False, .IntegerValue = True})
        Add(New OptionParam With {.Switch = "--aq-mode", .Text = "AQ Mode", .IntegerValue = True, .Options = {"Disabled", "Variance", "Complexity", "Cyclic Refresh"}})
        Add(New OptionParam With {.Switch = "--deltaq-mode", .Text = "Delta QIndex Mode", .IntegerValue = True, .Options = {"Disabled", "Deltaq Objective (default)", "Deltaq perceptual (requires enable-tpl-model)"}})
        Add(New BoolParam With {.Switch = "--delta-lf-mode", .Text = "Delta-lf-Mode", .Init = False, .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--frame-boost", .Text = "Enable frame periodic boost", .Init = False, .IntegerValue = True})
        Add(New NumParam With {.Switch = "--noise-sensitivity", .Text = "Noise Sensitivity"})
        Add(New OptionParam With {.Switch = "--tune-content", .Text = "Tune Content", .Options = {"Default", "Screen"}})



        AddTab("AV1 Specific 6")
        '######################

        Add(New OptionParam With {.Switch = "--cdf-update-mode", .Text = "CDF Update", .IntegerValue = True, .Options = {"No Update", "Update CDF on all frames(default)", "Selectively Update CDF on some frames"}, .Init = 1})
        Add(New OptionParam With {.Switch = "--color-primaries", .Text = "Color Primaries", .Options = {"unspecified", "BT2020", "BT601", "BT709", "BT470M", "BT470BG", "SMPTE170", "XYZ", "SMPTE240", "SMPTE431", "SMPTE432", "FILM", "EBU3213"}})
        Add(New OptionParam With {.Switch = "--transfer-characteristics", .Text = "Transfer Characteristics", .Options = {"unspecified", "BT709", "BT470M", "BT470BG", "BT601", "SMPTE240", "LIN", "LOG100", "LOG100SQ 10", "IEC 61966", "BT 1361", "SRGB", "BT2020-10bit", "BT2020-12bit", "SMPTE2084", "HLG", "SMPTE428"}})
        Add(New OptionParam With {.Switch = "--matrix-coefficients", .Text = "Matrix Coefficients", .Options = {" unspecified", "identity", "BT2020NC", "BT2020CL", "BT601", "FCC73", "BT709", "BT470BG", "SMPTE2085", "YCGCO", "SMPTE240", "ICTCP", "CHROMNCL", "CHROMCL"}})
        Add(New OptionParam With {.Switch = "--chroma-sample-position", .Text = "Chroma Sample Position", .Options = {"Unknown", "Vertical", "Colocated"}})
        Add(New NumParam With {.Switch = "--min-gf-interval", .Text = "Min GF Interval"})
        Add(New NumParam With {.Switch = "--max-gf-interval", .Text = "Max GF Interval"})
        Add(New NumParam With {.Switch = "--gf-min-pyr-height", .Text = "Min height for GF group pyramid structure", .Init = 0, .Config = {0, 5}})
        Add(New NumParam With {.Switch = "--gf-max-pyr-height", .Text = "Max height for GF group pyramid structure", .Init = 5, .Config = {0, 5}})
        Add(New OptionParam With {.Switch = "--sb-size", .Text = "Superblock size", .Options = {"Dynamic", "64", "128"}})
        Add(New NumParam With {.Switch = "--num-tile-groups", .Text = "Num Tile Groups", .Init = 1})
        Add(New NumParam With {.Switch = "--mtu-size", .Text = "MTU Size"})
        Add(New OptionParam With {.Switch = "--timing-info", .Text = "Timing info", .Options = {"Unspecified", "Constant", "Model"}})
        Add(New OptionParam With {.Switch = "--film-grain-test", .Text = "Film grain test vectors", .IntegerValue = True, .Options = {"None (default)", "test-1", "test-2", "test-3", "test-4", "test-5", "test-6", "test-7", "test-8", "test-9", "test-10", "test-11", "test-12", "test-13", "test-14", "test-15", "test-16"}})
        Add(New StringParam With {.Switch = "--film-grain-table", .Text = "Film Grain Table", .Quotes = QuotesMode.Auto, .BrowseFile = True})
        Add(New NumParam With {.Switch = "--denoise-noise-level", .Text = "Denoise Level", .Config = {0, 50}})
        Add(New NumParam With {.Switch = "--denoise-block-size", .Text = "Denoise Block Size", .Config = {0, 64}, .Init = 32})



        AddTab("AV1 Specific 7")
        '######################

        Add(New NumParam With {.Switch = "--max-reference-frames", .Text = "Max ref frames per frame", .Config = {3, 7}, .Init = 7})
        Add(New BoolParam With {.Switch = "--reduced-reference-set", .Text = "Rreduced set of refs", .Init = False, .IntegerValue = True})
        Add(New NumParam With {.Switch = "--enable-ref-frame-mvs", .Text = "Temporal mv prediction", .Init = 1})

        'Possible values are in the form of "ABxy"(pad leading zeros if less than 4 digits). AB: Operating Point(OP) index; xy: Target level index for the OP. E.g. "0" means target level index 0 for the 0th OP; "1021" means target level index 21 for the 10th OP.
        Add(New StringParam With {.Switch = "--target-seq-level-idx", .Text = "Target sequence level index"})

        Add(New OptionParam With {.Switch = "--set-tier-mask", .Text = "Tier mask", .IntegerValue = True, .Options = {"Main tier (default)", "High tier"}})
        Add(New NumParam With {.Switch = "--min-cr", .Text = "Minimum compression ratio", .Init = 0})
        Add(New NumParam With {.Switch = "--vbr-corpus-complexity-lap", .Text = "Average corpus complexity for 1pass VBR", .Config = {0, 10000}, .Init = 0})
        Add(New OptionParam With {.Switch = "--input-bit-depth", .Text = "Input Bit Depth", .Options = {"8", "10", "12"}, .AlwaysOn = True})
        Add(New NumParam With {.Switch = "--input-chroma-subsamplingx", .Text = "Chroma subsampling x value"})
        Add(New NumParam With {.Switch = "--input-chroma-subsamplingy", .Text = "Chroma subsampling y value"})
        Add(New NumParam With {.Switch = "--sframe-dist", .Text = "S-Frame interval"})
        Add(New NumParam With {.Switch = "--sframe-mode", .Text = "S-Frame insertion mode", .Config = {1, 2}, .Init = 1})
        Add(New StringParam With {.Switch = "--annexb", .Text = "Save as Annex-B", .Quotes = QuotesMode.Auto})



        AddTab("Custom")
        '######################

        Add(Custom)

    End Sub

    Overrides Sub ShowHelp(id As String)
        g.ShowCommandLineHelp(Package.aomenc, id)
    End Sub

    Private LastTabName As String

    Sub AddTab(name As String)
        LastTabName = name
    End Sub

    Shadows Sub Add(item As CommandLineParam)
        If item.HelpSwitch = "" Then
            Dim switches = item.GetSwitches

            If Not switches.NothingOrEmpty Then
                item.HelpSwitch = switches(0)
            End If
        End If

        item.Path = LastTabName
        ItemsValue.Add(item)
    End Sub

    Overloads Overrides Function GetCommandLine(
        includePaths As Boolean, includeExecutable As Boolean, Optional pass As Integer = 1) As String

        Return GetArgs(1, p.Script, p.VideoEncoder.OutputPath.DirAndBase +
                       p.VideoEncoder.OutputExtFull, includePaths, includeExecutable)
    End Function

    Overloads Function GetArgs(
        pass As Integer,
        script As VideoScript,
        targetPath As String,
        includePaths As Boolean,
        includeExecutable As Boolean) As String

        Dim sb As New StringBuilder

        If includePaths AndAlso includeExecutable Then
            If p.Script.Engine = ScriptEngine.VapourSynth Then
                sb.Append(Package.vspipe.Path.Escape + " " + script.Path.Escape + " - --y4m | " + Package.aomenc.Path.Escape + " -")
            Else
                sb.Append(Package.ffmpeg.Path.Escape + " -i " + script.Path.Escape + " -f yuv4mpegpipe -loglevel fatal -hide_banner - | " + Package.aomenc.Path.Escape + " -")
            End If
        End If

        sb.Append(" --disable-warning-prompt")

        Select Case Passes.Value
            Case 0
                sb.Append(" --passes=1")
            Case 1
                sb.Append(" --passes=2 --pass=" & pass)
        End Select

        If Not RateMode.OptionText.EqualsAny("CQ", "Q") Then
            sb.Append(" --target-bitrate=" & p.VideoBitrate)
        End If

        Dim q = From i In Items Where i.GetArgs <> ""

        If q.Count > 0 Then
            sb.Append(" " + q.Select(Function(item) item.GetArgs).Join(" "))
        End If

        If includePaths Then
            If Passes.Value = 1 Then
                sb.Append(" --fpf=" + (p.TempDir + p.TargetFile.Base + ".txt").Escape)
            End If

            sb.Append(" -o " + targetPath.Escape)
        End If

        Return Macro.Expand(sb.ToString.Trim.FixBreak.Replace(BR, " "))
    End Function

    Public Overrides Function GetPackage() As Package
        Return Package.AOMEnc
    End Function
End Class
