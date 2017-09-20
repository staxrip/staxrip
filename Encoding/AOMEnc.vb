Imports System.Text
Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class AOMEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Sub New()
        Name = "AV1"
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
        Encode("Video encoding using aomenc " + Package.AOMEnc.Version, GetArgs(1, p.Script))

        If Params.Mode.Value = 1 Then
            Encode("Video encoding second pass using aomenc " + Package.AOMEnc.Version, GetArgs(2, p.Script))
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(passName As String, commandLine As String)
        p.Script.Synchronize()

        Using proc As New Proc
            proc.Header = passName
            proc.Package = Package.AOMEnc
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
        Dim r As New MenuList
        r.Add("Encoder Options", AddressOf ShowConfigDialog)
        r.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return r
    End Function

    Shared WarningShown As Boolean

    Overrides Sub ShowConfigDialog()
        If Not WarningShown Then
            MsgWarn(
"Please note that AV1 is experimental!",
"The bitstream format is not yet frozen. It's very
likely that files created with the current encoder
become unplayable in the future.")

            WarningShown = True
        End If

        Dim newParams As New AV1Params
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(store)

        Using form As New CommandLineForm(newParams)
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

                Add(Mode)

                Add(RateMode,
                    New OptionParam With {.Path = "Basic", .Switch = "--bit-depth", .Text = "Depth", .Options = {"8", "10", "12"}},
                    New NumParam With {.Path = "Basic", .Switch = "--cq-level", .Text = "CQ Level"},
                    New StringParam With {.Path = "Analysis", .Switch = "--tile-loopfilter", .Text = "Tile Loopfilter"},
                    New OptionParam With {.Path = "Analysis", .Switch = "--tune", .Text = "Tune", .Options = {"Disabled", "PSNR", "SSIM", "Cdef-Dist", "Daala-Dist"}},
                    New NumParam With {.Path = "Analysis", .Switch = "--tile-columns", .Text = "Tile Columns"},
                    New NumParam With {.Path = "Analysis", .Switch = "--tile-rows", .Text = "Tile Rows"},
                    New NumParam With {.Path = "Analysis", .Switch = "--num-tile-groups", .Text = "Num Tile Groups"},
                    New NumParam With {.Path = "Analysis", .Switch = "--mtu-size", .Text = "MTU Size"},
                    New BoolParam With {.Path = "Analysis", .Switch = "--output-partitions", .Text = "Output Partitions"},
                    New BoolParam With {.Path = "Analysis", .Switch = "--disable-tempmv", .Text = "Disable temporal MV prediction", .IntegerValue = True},
                    New OptionParam With {.Path = "Rate Control 1", .Switch = "--aq-mode", .Text = "AQ Mode", .IntegerValue = True, .Options = {"Disabled", "Variance", "Complexity", "Cyclic", "Refresh"}},
                    New OptionParam With {.Path = "Rate Control 1", .Switch = "--deltaq-mode", .Text = "Delta QIndex Mode", .IntegerValue = True, .Options = {"Disabled", "Deltaq", "Deltaq + Deltalf"}},
                    New BoolParam With {.Path = "Rate Control 1", .Switch = "--lossless", .Text = "Lossless", .IntegerValue = True},
                    New BoolParam With {.Path = "Rate Control 1", .Switch = "--enable-qm", .Text = "Enable QM", .IntegerValue = True},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--bias-pct", .Text = "Bias PCT", .Config = {0, 100}},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--max-intra-rate", .Text = "Max Intra Rate"},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--max-inter-rate", .Text = "Max Inter Rate"},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--undershoot-pct", .Text = "Undershoot PCT"},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--overshoot-pct", .Text = "Overshoot PCT"},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--minsection-pct", .Text = "Minsection PCT"},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--maxsection-pct", .Text = "Maxsection PCT"},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--gf-cbr-boost", .Text = "GF CBR Boost"},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--min-q", .Text = "Minimum Quantizer"},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--max-q", .Text = "Maximum Quantizer"},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--qm-min", .Text = "Min QM Flatness", .Init = 8, .Config = {0, 16}},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--qm-max", .Text = "Max QM Flatness", .Init = 16, .Config = {0, 16}},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--buf-sz", .Text = "Buffer Size"},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--buf-initial-sz", .Text = "Buf Initial Size"},
                    New NumParam With {.Path = "Rate Control 2", .Switch = "--buf-optimal-sz", .Text = "Buf Optimal Size"},
                    New NumParam With {.Path = "Slice Decision", .Switch = "--kf-min-dist", .Text = "Min GOP Size"},
                    New NumParam With {.Path = "Slice Decision", .Switch = "--kf-max-dist", .Text = "Max GOP Size"},
                    New NumParam With {.Path = "Slice Decision", .Switch = "--lag-in-frames", .Text = "Lag In Frames"},
                    New BoolParam With {.Path = "Slice Decision", .Switch = "--disable-kf", .Text = "Disable keyframe placement"},
                    New StringParam With {.Path = "Input/Output", .Switch = "--timebase", .Text = "Timebase"},
                    New NumParam With {.Path = "Input/Output", .Switch = "--input-bit-depth", .Text = "Input Bit Depth"},
                    New NumParam With {.Path = "Input/Output", .Switch = "--fps", .Text = "Frame Rate"},
                    New NumParam With {.Path = "Input/Output", .Switch = "--limit", .Text = "Limit"},
                    New NumParam With {.Path = "Input/Output", .Switch = "--skip", .Text = "Skip"},
                    New BoolParam With {.Path = "Input/Output", .Switch = "--yv12", .Text = "YV12"},
                    New BoolParam With {.Path = "Input/Output", .Switch = "--i420", .Text = "I420"},
                    New BoolParam With {.Path = "Input/Output", .Switch = "--i422", .Text = "I422"},
                    New BoolParam With {.Path = "Input/Output", .Switch = "--i444", .Text = "I444"},
                    New BoolParam With {.Path = "Input/Output", .Switch = "--i440", .Text = "I440"},
                    New OptionParam With {.Path = "VUI", .Switch = "--color-space", .Text = "Color Space", .Options = {"Unknown", "BT 2020", "BT 601", "BT 709", "SMPTE 170", "SMPTE 240", "sRGB"}},
                    New OptionParam With {.Path = "VUI", .Switch = "--transfer-function", .Text = "Transfer Function", .Options = {"Unknown", "BT 709", "HLG", "PQ"}},
                    New NumParam With {.Path = "Performance", .Switch = "--threads", .Text = "Threads"},
                    New NumParam With {.Path = "Performance", .Switch = "--cpu-used", .Text = "CPUs Used", .Config = {0, 8}},
                    New BoolParam With {.Path = "Performance", .Switch = "--frame-parallel", .Text = "Frame Parallel", .IntegerValue = True},
                    New OptionParam With {.Path = "Statistic", .Switch = "--test-decode", .Text = "Test Decode", .Options = {"Disabled", "Fatal", "Warn"}},
                    New BoolParam With {.Path = "Statistic", .Switch = "--psnr", .Text = "PSNR"},
                    New BoolParam With {.Path = "Statistic", .Switch = "--debug", .Text = "Debug"},
                    New BoolParam With {.Path = "Statistic", .Switch = "--disable-warnings", .Text = "Disable Warnings"},
                    New BoolParam With {.Path = "Statistic", .Switch = "--disable-warning-prompt", .Text = "Disable Warning Prompt"},
                    New BoolParam With {.Path = "Statistic", .Switch = "--quiet", .Text = "Quiet"},
                    New NumParam With {.Path = "Frame Size", .Switch = "--width", .Text = "Width"},
                    New NumParam With {.Path = "Frame Size", .Switch = "--height", .Text = "Height"},
                    New NumParam With {.Path = "Frame Size", .Switch = "--resize-mode", .Text = "Resize Mode"},
                    New NumParam With {.Path = "Frame Size", .Switch = "--resize-numerator", .Text = "Resize Numerator"},
                    New NumParam With {.Path = "Frame Size", .Switch = "--resize-kf-numerator", .Text = "Resize KF Numerator"},
                    Custom,
                    New StringParam With {.Path = "Misc 1", .Switch = "--error-resilient", .Text = "Error Resilient"},
                    New StringParam With {.Path = "Misc 1", .Switch = "--deadline", .Text = "Deadline"},
                    New StringParam With {.Path = "Misc 1", .Switch = "--q-hist", .Text = "Q-Hist"},
                    New StringParam With {.Path = "Misc 1", .Switch = "--codec", .Text = "Codec"},
                    New StringParam With {.Path = "Misc 1", .Switch = "--rate-hist", .Text = "Rate Hist"},
                    New OptionParam With {.Path = "Misc 1", .Switch = "--stereo-mode", .Text = "Stereo Mode", .Options = {"None", "Mono", "Left-Right", "Bottom-Top", "Top-Bottom", "Right-Left"}},
                    New OptionParam With {.Path = "Misc 1", .Switch = "--tune-content", .Text = "Tune Content", .Options = {"Default", "Screen"}},
                    New OptionParam With {.Path = "Misc 1", .Switch = "--chroma-sample-position", .Text = "Chroma Sample Pos", .Options = {"Unknown", "Vertical", "Colocated"}},
                    New BoolParam With {.Path = "Misc 1", .Switch = "--good", .Text = "Good"},
                    New BoolParam With {.Path = "Misc 1", .Switch = "--verbose", .Text = "Verbose"},
                    New BoolParam With {.Path = "Misc 1", .Switch = "--webm", .Text = "WEBM"},
                    New BoolParam With {.Path = "Misc 1", .Switch = "--ivf", .Text = "IVF"},
                    New BoolParam With {.Path = "Misc 1", .Switch = "--frame-boost", .Text = "Enable frame periodic boost", .IntegerValue = True},
                    New NumParam With {.Path = "Misc 2", .Switch = "--usage", .Text = "Usage"},
                    New NumParam With {.Path = "Misc 2", .Switch = "--profile", .Text = "Profile"},
                    New NumParam With {.Path = "Misc 2", .Switch = "--drop-frame", .Text = "Drop Frame"},
                    New NumParam With {.Path = "Misc 2", .Switch = "--auto-alt-ref", .Text = "Auto Alt Ref"},
                    New NumParam With {.Path = "Misc 2", .Switch = "--sharpness", .Text = "Sharpness", .Config = {0, 7}},
                    New NumParam With {.Path = "Misc 2", .Switch = "--static-thresh", .Text = "Static Thresh"},
                    New NumParam With {.Path = "Misc 2", .Switch = "--arnr-maxframes", .Text = "ARNR Maxframes", .Config = {0, 15}},
                    New NumParam With {.Path = "Misc 2", .Switch = "--arnr-strength", .Text = "ARNR Strength", .Config = {0, 6}},
                    New NumParam With {.Path = "Misc 2", .Switch = "--noise-sensitivity", .Text = "Noise Sensitivity"},
                    New NumParam With {.Path = "Misc 2", .Switch = "--min-gf-interval", .Text = "Min GF Interval"},
                    New NumParam With {.Path = "Misc 2", .Switch = "--max-gf-interval", .Text = "Max GF Interval"})
            End If

            Return ItemsValue
        End Get
    End Property

    Public Overrides Sub ShowHelp(id As String)
        g.ShowCommandLineHelp(Package.AOMEnc, id)
    End Sub

    Shadows Sub Add(ParamArray items As CommandLineParam())
        For Each i In items
            If i.HelpSwitch = "" Then
                Dim switches = i.GetSwitches
                If Not switches.NothingOrEmpty Then i.HelpSwitch = switches(0)
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
                sb.Append(Package.ffmpeg.Path.Escape + " -i " + script.Path.Escape + " -f yuv4mpegpipe -loglevel fatal -hide_banner - | " + Package.AOMEnc.Path.Escape + " -")
            End If
        End If

        Select Case Mode.Value
            Case 0
                sb.Append(" --passes=1")
            Case 1
                sb.Append(" --passes=2 --pass=" & pass)
        End Select

        If Not RateMode.OptionText.EqualsAny("CQ", "Q") Then sb.Append(" --target-bitrate=" & p.VideoBitrate)
        Dim q = From i In Items Where i.GetArgs <> ""
        If q.Count > 0 Then sb.Append(" " + q.Select(Function(item) item.GetArgs).Join(" "))

        If includePaths Then
            If Mode.Value = 1 Then sb.Append(" --fpf=" + (p.TempDir + p.TargetFile.Base + ".txt").Escape)
            sb.Append(" -o " + targetPath.Escape)
        End If

        Return Macro.Expand(sb.ToString.Trim.FixBreak.Replace(BR, " "))
    End Function

    Public Overrides Function GetPackage() As Package
        Return Package.AOMEnc
    End Function
End Class