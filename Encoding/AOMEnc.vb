
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

        If Params.Mode.Value = 1 Then
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

    Property Mode As New OptionParam With {
        .Name = "Mode",
        .Text = "Mode",
        .Path = "Basic",
        .Switches = {"--passes", "--pass", "--target-bitrate"},
        .Options = {"One Pass", "Two Pass"}}

    Property RateMode As New OptionParam With {
        .Path = "Basic",
        .Switch = "--end-usage",
        .Text = "Rate Mode",
        .Options = {"VBR", "CBR", "CQ", "Q"}}

    Property Custom As New StringParam With {
        .Text = "Custom",
        .Path = "Misc 1",
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

        Add(Mode)
        Add(RateMode)
        Add(New OptionParam With {.Switch = "--profile", .Text = "Profile", .IntegerValue = True, .Options = {"Main", "High", "Professional"}})
        Add(New OptionParam With {.Switch = "--bit-depth", .Text = "Depth", .Options = {"8", "10", "12"}})
        Add(New OptionParam With {.Switch = "--cpu-used", .Text = "CPU Used", .Value = 8, .AlwaysOn = True, .IntegerValue = True, .Options = {"0 - Slowest", "1 - Very Slow", "2 - Slower", "3 - Slow", "4 - Medium", "5 - Fast", "6 - Faster", "7 - Very Fast", "8 - Fastest"}})

        Add(New NumParam With {.Switch = "--cq-level", .Text = "CQ Level"})

        AddTab("Analysis")
        '################

        Add(New StringParam With {.Switch = "--cfg", .Text = "Config File", .Quotes = QuotesMode.Auto, .BrowseFile = True})

        Add(New OptionParam With {.Switch = "--tune", .Text = "Tune", .Options = {"psnr", "ssim", "vmaf_with_preprocessing", "vmaf_without_preprocessing", "vmaf"}})
        Add(New OptionParam With {.Switch = "--large-scale-tile", .Text = "Large Scale Tile Groups", .IntegerValue = True, .Options = {"Off", "On"}})

        Add(New NumParam With {.Switch = "--tile-columns", .Text = "Tile Columns"})
        Add(New NumParam With {.Switch = "--tile-rows", .Text = "Tile Rows"})
        Add(New NumParam With {.Switch = "--num-tile-groups", .Text = "Num Tile Groups"})
        Add(New NumParam With {.Switch = "--mtu-size", .Text = "MTU Size"})

        AddTab("Rate Control 1")
        '######################

        Add(New OptionParam With {.Switch = "--aq-mode", .Text = "AQ Mode", .IntegerValue = True, .Options = {"Disabled", "Variance", "Complexity", "Cyclic", "Refresh"}})
        Add(New OptionParam With {.Switch = "--deltaq-mode", .Text = "Delta QIndex Mode", .IntegerValue = True, .Options = {"Disabled", "Deltaq", "Deltaq + Deltalf"}})

        Add(New NumParam With {.Switch = "--superres-mode", .Text = "SuperRes"})
        Add(New NumParam With {.Switch = "--superres-denominator", .Text = "SuperRes Denominator"})
        Add(New NumParam With {.Switch = "--superres-kf-denominator", .Text = "SuperRes KF Denominator"})
        Add(New NumParam With {.Switch = "--superres-qthresh", .Text = "SuperRes qThresh"})
        Add(New NumParam With {.Switch = "--superres-kf-qthresh", .Text = "SuperRes KF qThresh"})

        Add(New BoolParam With {.Switch = "--lossless", .Text = "Lossless", .IntegerValue = True})
        Add(New BoolParam With {.Switch = "--enable-qm", .Text = "Enable QM", .IntegerValue = True})

        AddTab("Rate Control 2")
        '######################

        Add(New NumParam With {.Switch = "--bias-pct", .Text = "Bias PCT", .Config = {0, 100}})
        Add(New NumParam With {.Switch = "--max-intra-rate", .Text = "Max Intra Rate"})
        Add(New NumParam With {.Switch = "--max-inter-rate", .Text = "Max Inter Rate"})
        Add(New NumParam With {.Switch = "--undershoot-pct", .Text = "Undershoot PCT"})
        Add(New NumParam With {.Switch = "--overshoot-pct", .Text = "Overshoot PCT"})
        Add(New NumParam With {.Switch = "--minsection-pct", .Text = "Minsection PCT"})
        Add(New NumParam With {.Switch = "--maxsection-pct", .Text = "Maxsection PCT"})
        Add(New NumParam With {.Switch = "--gf-cbr-boost", .Text = "GF CBR Boost"})
        Add(New NumParam With {.Switch = "--min-q", .Text = "Minimum Quantizer"})
        Add(New NumParam With {.Switch = "--max-q", .Text = "Maximum Quantizer"})
        Add(New NumParam With {.Switch = "--qm-min", .Text = "Min QM Flatness", .Init = 8, .Config = {0, 16}})
        Add(New NumParam With {.Switch = "--qm-max", .Text = "Max QM Flatness", .Init = 16, .Config = {0, 16}})
        Add(New NumParam With {.Switch = "--buf-sz", .Text = "Buffer Size"})
        Add(New NumParam With {.Switch = "--buf-initial-sz", .Text = "Buf Initial Size"})
        Add(New NumParam With {.Switch = "--buf-optimal-sz", .Text = "Buf Optimal Size"})

        AddTab("Slice Decision")
        '######################

        Add(New NumParam With {.Switch = "--kf-min-dist", .Text = "Min GOP Size"})
        Add(New NumParam With {.Switch = "--kf-max-dist", .Text = "Max GOP Size"})
        Add(New NumParam With {.Switch = "--lag-in-frames", .Text = "Lag In Frames"})

        Add(New BoolParam With {.Switch = "--disable-kf", .Text = "Disable keyframe placement"})

        AddTab("Input/Output")
        '####################

        Add(New StringParam With {.Switch = "--timebase", .Text = "Timebase"})
        Add(New StringParam With {.Switch = "--annexb", .Text = "Save as Annex-B", .Quotes = QuotesMode.Auto})

        Add(New NumParam With {.Switch = "--input-bit-depth", .Text = "Input Bit Depth"})
        Add(New NumParam With {.Switch = "--fps", .Text = "Frame Rate"})
        Add(New NumParam With {.Switch = "--limit", .Text = "Limit"})
        Add(New NumParam With {.Switch = "--skip", .Text = "Skip"})

        Add(New BoolParam With {.Switch = "--yv12", .Text = "YV12"})
        Add(New BoolParam With {.Switch = "--i420", .Text = "I420"})
        Add(New BoolParam With {.Switch = "--i422", .Text = "I422"})
        Add(New BoolParam With {.Switch = "--i444", .Text = "I444"})

        AddTab("VUI")
        '###########

        Add(New OptionParam With {.Switch = "--color-primaries", .Text = "Primaries", .Options = {" unspecified", "BT 2020", "BT 601", "BT 709", "BT 470 M", "BT 470 BG", "SMPTE 170", "XYZ", "SMPTE 240", "SMPTE 431", "SMPTE 432", "FILM", "EBU 3213"}})
        Add(New OptionParam With {.Switch = "--transfer-characteristics", .Text = "Transfer", .Options = {"unspecified", "BT 709", "BT 470 M", "BT 470 BG", "BT 601", "SMPTE 240", "LIN", "LOG 100", "LOG 100SQ 10", "IEC 61966", "BT 1361", "SRGB", "BT2020-10bit", "BT2020-12bit", "SMPTE 2084", "HLG", "SMPTE 428"}})
        Add(New OptionParam With {.Switch = "--matrix-coefficients", .Text = "Matrix", .Options = {" unspecified", "identity", "BT 2020 NC", "BT 2020 CL", "BT 601", "FCC 73", "BT 709", "BT 470 BG", "SMPTE 2085", "YCGCO", "SMPTE 240", "ICTCP", "CHROM NCL", "CHROM CL"}})

        AddTab("Performance")
        '###################

        Add(New OptionParam With {.Switch = "--row-mt", .Text = "Multi-Threading", .IntegerValue = True, .Options = {"On", "Off"}})
        Add(New NumParam With {.Switch = "--threads", .Text = "Threads"})
        Add(New BoolParam With {.Switch = "--frame-parallel", .Text = "Frame Parallel", .IntegerValue = True})

        AddTab("Statistic")
        '#################

        Add(New OptionParam With {.Switch = "--test-decode", .Text = "Test Decode", .Options = {"Disabled", "Fatal", "Warn"}})

        Add(New BoolParam With {.Switch = "--psnr", .Text = "PSNR"})
        Add(New BoolParam With {.Switch = "--debug", .Text = "Debug"})
        Add(New BoolParam With {.Switch = "--disable-warnings", .Text = "Disable Warnings"})
        Add(New BoolParam With {.Switch = "--disable-warning-prompt", .Text = "Disable Warning Prompt"})
        Add(New BoolParam With {.Switch = "--quiet", .Text = "Quiet"})

        AddTab("Frame Size")
        '##################

        Add(New NumParam With {.Switch = "--width", .Text = "Width"})
        Add(New NumParam With {.Switch = "--height", .Text = "Height"})
        Add(New NumParam With {.Switch = "--forced_max_frame_width", .Text = "Force Width"})
        Add(New NumParam With {.Switch = "--forced_max_frame_height", .Text = "Force Height"})
        Add(New NumParam With {.Switch = "--resize-mode", .Text = "Resize Mode"})
        Add(New NumParam With {.Switch = "--resize-denominator", .Text = "Resize Denominator"})
        Add(New NumParam With {.Switch = "--resize-kf-denominator", .Text = "Resize KF Denominator"})

        AddTab("Filters")
        '###############

        Add(New StringParam With {.Switch = "--film-grain-table", .Text = "Film Grain Table", .Quotes = QuotesMode.Auto, .BrowseFile = True})
        Add(New OptionParam With {.Switch = "--enable-restoration", .Text = "Restoration", .IntegerValue = True, .Options = {"Off", "On"}})
        Add(New NumParam With {.Switch = "--sharpness", .Text = "Sharpness", .Config = {0, 7}})
        Add(New NumParam With {.Switch = "--noise-sensitivity", .Text = "Noise Sensitivity"})
        Add(New NumParam With {.Switch = "--denoise-noise-level", .Text = "Denoise Level", .Config = {0, 50}})
        Add(New NumParam With {.Switch = "--denoise-block-size", .Text = "Denoise Block Size", .Config = {0, 32}})

        AddTab("Misc 1")
        '##############

        Add(Custom)
        Add(New StringParam With {.Switch = "--error-resilient", .Text = "Error Resilient"})
        Add(New StringParam With {.Switch = "--global-error-resilient", .Text = "Global Error Resilient"})
        Add(New StringParam With {.Switch = "--q-hist", .Text = "Q-Hist"})
        Add(New StringParam With {.Switch = "--codec", .Text = "Codec"})
        Add(New StringParam With {.Switch = "--rate-hist", .Text = "Rate Hist"})

        Add(New OptionParam With {.Switch = "--stereo-mode", .Text = "Stereo Mode", .Options = {"None", "Mono", "Left-Right", "Bottom-Top", "Top-Bottom", "Right-Left"}})
        Add(New OptionParam With {.Switch = "--tune-content", .Text = "Tune Content", .Options = {"Default", "Screen"}})
        Add(New OptionParam With {.Switch = "--chroma-sample-position", .Text = "Chroma Sample Pos", .Options = {"Unknown", "Vertical", "Colocated"}})
        Add(New OptionParam With {.Switch = "--enable-cdef", .Text = "Enable CDEF", .Init = 1, .IntegerValue = True, .Options = {"Off", "On"}})
        Add(New OptionParam With {.Switch = "--cdf-update-mode", .Text = "CDF Update", .IntegerValue = True, .Options = {"No Update", "Update All", "Selectively Update"}, .Init = 1})
        Add(New OptionParam With {.Switch = "--disable-trellis-quant", .Text = "Disable Trellis Quant", .IntegerValue = True, .Options = {"Off", "On"}})
        Add(New OptionParam With {.Switch = "--sb-size", .Text = "Chroma Sample Pos", .Options = {"Dynamic", "64", "128"}})
        Add(New OptionParam With {.Switch = "--timing-info", .Text = "Chroma Sample Pos", .Options = {"Unspecified", "Constant", "Model"}})

        AddTab("Misc 2")
        '##############

        Add(New NumParam With {.Switch = "--usage", .Text = "Usage"})
        Add(New NumParam With {.Switch = "--drop-frame", .Text = "Drop Frame"})
        Add(New NumParam With {.Switch = "--auto-alt-ref", .Text = "Auto Alt Ref"})
        Add(New NumParam With {.Switch = "--static-thresh", .Text = "Static Thresh"})
        Add(New NumParam With {.Switch = "--arnr-maxframes", .Text = "ARNR Maxframes", .Config = {0, 15}})
        Add(New NumParam With {.Switch = "--arnr-strength", .Text = "ARNR Strength", .Config = {0, 6}})
        Add(New NumParam With {.Switch = "--enable-ref-frame-mvs", .Text = "Ref Frame mvs", .Init = 1})
        Add(New NumParam With {.Switch = "--min-gf-interval", .Text = "Min GF Interval"})
        Add(New NumParam With {.Switch = "--max-gf-interval", .Text = "Max GF Interval"})

        Add(New BoolParam With {.Switch = "--good", .Text = "Good"})
        Add(New BoolParam With {.Switch = "--verbose", .Text = "Verbose"})
        Add(New BoolParam With {.Switch = "--webm", .Text = "WEBM"})
        Add(New BoolParam With {.Switch = "--ivf", .Text = "IVF"})
        Add(New BoolParam With {.Switch = "--obu", .Text = "OBU"})
        Add(New BoolParam With {.Switch = "--frame-boost", .Text = "Enable frame periodic boost", .IntegerValue = True})
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

        Select Case Mode.Value
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
            If Mode.Value = 1 Then
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
