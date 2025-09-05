﻿
Imports System.Text
Imports System.Web
Imports StaxRip.UI

Imports StaxRip.VideoEncoderCommandLine

<Serializable()>
Public Class ffmpegEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "ffmpeg | " + Params.Codec.OptionText
        End Get
    End Property

    Public Sub New()
        Muxer = New ffmpegMuxer("AVI")
    End Sub

    Public Sub New(codecIndex As Integer)
        Me.New()
        Params.Codec.Value = If(codecIndex > 0 AndAlso codecIndex < Params.Codec.Values.Length, codecIndex, 0)
    End Sub

    Overloads Shared ReadOnly Property Package As Package
        Get
            Return Package.ffmpeg
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

    Overrides Sub ShowConfigDialog(Optional param As CommandLineParam = Nothing)
        Dim newParams = New EncoderParams
        Dim store = ObjectHelp.GetCopy(ParamsStore)
        newParams.Init(store)

        Using form As New CommandLineForm(newParams)
            Dim a1 = Sub()
                         Dim enc = ObjectHelp.GetCopy(Me)
                         Dim params2 As New EncoderParams
                         Dim store2 = ObjectHelp.GetCopy(store)
                         params2.Init(store2)
                         enc.Params = params2
                         enc.ParamsStore = store2
                         SaveProfile(enc)
                     End Sub

            form.cms.Add("Save Profile...", a1, Keys.Control Or Keys.S, Symbol.Save)

            Dim a2 = Sub()
                         Dim codecText = newParams.Codec.OptionText
                         Dim consoleHelp = ProcessHelp.GetConsoleOutput(Package.ffmpeg.Path, "-hide_banner -h encoder=" + newParams.Codec.ValueText)
                         Dim helpDic As New Dictionary(Of String, String) From {
                            {"x264", "https://trac.ffmpeg.org/wiki/Encode/H.264"},
                            {"x265", "https://trac.ffmpeg.org/wiki/Encode/H.265"},
                            {"XviD", "https://trac.ffmpeg.org/wiki/Encode/MPEG-4"},
                            {"VP9", "https://trac.ffmpeg.org/wiki/Encode/VP9"},
                            {"FFV1", "https://trac.ffmpeg.org/wiki/Encode/FFV1"},
                            {"Intel H.264", "https://trac.ffmpeg.org/wiki/Hardware/QuickSync"},
                            {"Intel H.265", "https://trac.ffmpeg.org/wiki/Hardware/QuickSync"},
                            {"AV1", "https://trac.ffmpeg.org/wiki/Encode/AV1"}}

                         Dim help = $"<h2>ffmpeg Online Help</h2>" +
                                     "<p><a href=""{Package.ffmpeg.HelpURL}"">ffmpeg Online Help</a></p>"

                         If helpDic.ContainsKey(codecText) Then
                             help += $"<h2>ffmpeg {codecText} Online Help</h2>" +
                                     $"<p><a href=""{helpDic(codecText)}"">ffmpeg {codecText} Online Help</a></p>"
                         End If

                         help += $"<h2>ffmpeg {codecText} Console Help</h2>" +
                                 $"<pre>{HelpDocument.ConvertChars(consoleHelp) + BR}</pre>"

                         form.HTMLHelpFunc = Function() help
                     End Sub

            AddHandler form.BeforeHelp, a2

            If Not String.IsNullOrWhiteSpace(param?.Path) Then
                form.SimpleUI.ShowPage(param?.Path)
                form.cbGoTo.Text = param.GetSwitches().FirstOrDefault()
            End If

            If form.ShowDialog() = DialogResult.OK Then
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides ReadOnly Property Codec As String
        Get
            Return Params.Codec.ValueText
        End Get
    End Property

    Overrides ReadOnly Property OutputExt As String
        Get
            Select Case Params.Codec.OptionText
                Case "Xvid", "MPEG-4", "FFV1"
                    Return "avi"
                Case "ProRes", "R210", "V210"
                    Return "mov"
                Case "Theora"
                    Return "ogg"
                Case "VP | VP8", "VP | VP9"
                    Return "webm"
                Case "x264", "AMD | AMD AMF H.264", "Intel | Intel H.264", "Nvidia | Nvidia H.264"
                    Return "h264"
                Case "x265", "AMD | AMD AMF H.265", "Intel | Intel H.265", "Nvidia | Nvidia H.265"
                    Return "h265"
                Case "AOM-AV1", "AMD | AMD AMF AV1", "Intel | Intel AV1", "Nvidia | Nvidia AV1"
                    Return "ivf"
                Case Else
                    Return "mkv"
            End Select
        End Get
    End Property

    Overrides ReadOnly Property IsCompCheckEnabled() As Boolean
        Get
            Return False
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Params.RaiseValueChanged(Nothing)

        If Params.Mode.OptionText = "Two Pass" Then
            Encode(Params.GetCommandLine(True, True, 1), s.ProcessPriority)
            Encode(Params.GetCommandLine(True, True, 2), s.ProcessPriority)
        Else
            Encode(Params.GetCommandLine(True, True), s.ProcessPriority)
        End If
    End Sub

    Overloads Sub Encode(commandLine As String, priority As ProcessPriorityClass)
        Using proc As New Proc
            proc.Header = "Video encoding " + Params.Codec.OptionText
            proc.SkipStrings = {"frame=", "size="}
            proc.FrameCount = p.Script.GetFrameCount
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg

            If commandLine.Contains("|") Then
                proc.File = "cmd.exe"
                proc.Arguments = "/S /C """ + commandLine + """"
            Else
                proc.CommandLine = commandLine
            End If

            proc.Start()
        End Using
    End Sub

    Overrides Function GetMenu() As MenuList
        Dim ret As New MenuList
        ret.Add("Encoder Options", AddressOf ShowConfigDialog)
        ret.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return ret
    End Function

    Overrides Property QualityMode() As Boolean
        Get
            Return Params.Mode.Value = EncodingMode.Quality
        End Get
        Set(Value As Boolean)
        End Set
    End Property

    Public Overrides ReadOnly Property CommandLineParams As CommandLineParams
        Get
            Return Params
        End Get
    End Property

    Public Class EncoderParams
        Inherits CommandLineParams

        Sub New()
            Title = "ffmpeg Options"
        End Sub

        Public Overrides ReadOnly Property Package As Package
            Get
                Return ffmpegEnc.Package
            End Get
        End Property


        Property Codec As New OptionParam With {
            .Switch = "-c:v",
            .Text = "Codec",
            .AlwaysOn = True,
            .Options = {"x264", "x265", "AOM-AV1", "XviD", "MPEG-4", "Theora", "ProRes",
                        "R210", "V210", "UT Video", "GoPro CineForm HD", "FFV1",
                        "AMD | AMD AMF H.264", "AMD | AMD AMF H.265", "AMD | AMD AMF AV1",
                        "Intel | Intel H.264", "Intel | Intel H.265", "Intel | Intel AV1",
                        "Nvidia | Nvidia H.264", "Nvidia | Nvidia H.265", "Nvidia | Nvidia AV1",
                        "VP | VP8", "VP | VP9"},
            .Values = {"libx264", "libx265", "libaom-av1", "libxvid", "mpeg4", "libtheora", "prores",
                       "r210", "v210", "utvideo", "cfhd", "ffv1",
                       "h264_amf", "hevc_amf", "av1_amf",
                       "h264_qsv", "hevc_qsv", "av1_qsv",
                       "h264_nvenc", "hevc_nvenc", "av1_nvenc",
                       "libvpx", "libvpx-vp9"}}

        Property Mode As New OptionParam With {
            .Name = "Mode",
            .Text = "Mode",
            .VisibleFunc = Function() Not Codec.ValueText.EqualsAny("prores", "utvideo", "cfhd", "ffv1"),
            .Options = {"Quality", "One Pass", "Two Pass"}}

        Property Decoder As New OptionParam With {
            .Text = "Decoder",
            .Options = {"AviSynth/VapourSynth", "Software", "Intel", "DXVA2", "Nvidia"},
            .Values = {"-", "sw", "qsv", "dxva2", "cuda"}}

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

        Property h264_nvenc_rc As New OptionParam With {
            .Name = "h264_nvenc rc",
            .Switch = "-rc",
            .Text = "Rate Control",
            .Options = {"Preset", "Constqp", "VBR", "CBR", "VBR_MinQP", "LL_2Pass_Quality", "LL_2Pass_Size", "VBR_2Pass"},
            .VisibleFunc = Function() Codec.ValueText = "h264_nvenc"}

        Property Custom As New StringParam With {
            .Text = "Custom",
            .Quotes = QuotesMode.Never,
            .AlwaysOn = True}

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)

                    Add("Main",
                        Decoder, PipingToolAVS, PipingToolVS, Codec, Mode,
                        New OptionParam With {.Name = "x264/x265 preset", .Text = "Preset", .Switch = "-preset", .Init = 5, .Options = {"Ultrafast", "Superfast", "Veryfast", "Faster", "Fast", "Medium", "Slow", "Slower", "Veryslow", "Placebo"}, .VisibleFunc = Function() Codec.OptionText.EqualsAny("x264", "x265")},
                        New OptionParam With {.Name = "x264/x265 tune", .Text = "Tune", .Switch = "-tune", .Options = {"None", "Film", "Animation", "Grain", "Stillimage", "Psnr", "Ssim", "Fastdecode", "Zerolatency"}, .VisibleFunc = Function() Codec.OptionText.EqualsAny("x264", "x265")},
                        New OptionParam With {.Switch = "-profile:v", .Text = "Profile", .VisibleFunc = Function() Codec.OptionText = "ProRes", .Init = 2, .IntegerValue = True, .Options = {"Proxy", "LT", "Standard", "HQ"}},
                        New OptionParam With {.Switch = "-speed", .Text = "Speed", .AlwaysOn = True, .VisibleFunc = Function() Codec.OptionText.EqualsAny("VP8", "VP9"), .Options = {"6 - Fastest", "5 - Faster", "4 - Fast", "3 - Medium", "2 - Slow", "1 - Slower", "0 - Slowest"}, .Values = {"6", "5", "4", "3", "2", "1", "0"}, .Value = 5},
                        New OptionParam With {.Switch = "-cpu-used", .Text = "CPU Used", .Init = 1, .VisibleFunc = Function() Codec.OptionText = "AV1", .IntegerValue = True, .Options = {"0 - Slowest", "1 - Very Slow", "2 - Slower", "3 - Slow", "4 - Medium", "5 - Fast", "6 - Faster", "7 - Very Fast", "8 - Fastest"}},
                        New OptionParam With {.Switch = "-aq-mode", .Text = "AQ Mode", .VisibleFunc = Function() Codec.OptionText = "VP9", .Options = {"Disabled", "0", "1", "2", "3"}, .Values = {"Disabled", "0", "1", "2", "3"}},
                        New OptionParam With {.Name = "h264_nvenc profile", .Switch = "-profile", .Text = "Profile", .Options = {"Baseline", "Main", "High", "High444p"}, .Init = 1, .VisibleFunc = Function() Codec.ValueText = "h264_nvenc"},
                        New OptionParam With {.Name = "h264_nvenc preset", .Switch = "-preset", .Text = "Preset", .Options = {"Default", "Slow", "Medium", "Fast", "HP", "HQ", "BD", "LL", "LLHQ", "LLHP", "Lossless", "Losslesshp"}, .Init = 2, .VisibleFunc = Function() Codec.ValueText = "h264_nvenc"},
                        New OptionParam With {.Name = "h264_nvenc level", .Switch = "-level", .Text = "Level", .Options = {"Auto", "1", "1.0", "1b", "1.0b", "1.1", "1.2", "1.3", "2", "2.0", "2.1", "2.2", "3", "3.0", "3.1", "3.2", "4", "4.0", "4.1", "4.2", "5", "5.0", "5.1"}, .VisibleFunc = Function() Codec.ValueText = "h264_nvenc"},
                        h264_nvenc_rc,
                        New OptionParam With {.Name = "utVideoPred", .Switch = "-pred", .Text = "Prediction", .Init = 3, .Options = {"None", "Left", "Gradient", "Median"}, .VisibleFunc = Function() Codec.ValueText = "utvideo"},
                        New OptionParam With {.Name = "utVideoPixFmt", .Switch = "-pix_fmt", .Text = "Pixel Format", .Options = {"YUV420P", "YUV422P", "YUV444P", "RGB24", "RGBA"}, .VisibleFunc = Function() Codec.ValueText = "utvideo"},
                        New NumParam With {.Name = "Quality", .Text = "Quality", .Init = -1, .VisibleFunc = Function() Mode.Value = EncodingMode.Quality AndAlso Not Codec.ValueText.EqualsAny("prores", "utvideo", "cfhd", "ffv1"), .ArgsFunc = AddressOf GetQualityArgs, .Config = {-1, 63, 0.5, 1}},
                        New NumParam With {.Switch = "-threads", .Text = "Threads", .Config = {0, 64}},
                        New NumParam With {.Switch = "-tile-columns", .Text = "Tile Columns", .VisibleFunc = Function() Codec.OptionText = "VP9", .Value = 6, .DefaultValue = -1},
                        New NumParam With {.Switch = "-frame-parallel", .Text = "Frame Parallel", .VisibleFunc = Function() Codec.OptionText = "VP9", .Value = 1, .DefaultValue = -1},
                        New NumParam With {.Switch = "-auto-alt-ref", .Text = "Auto Alt Ref", .VisibleFunc = Function() Codec.OptionText = "VP9", .Value = 1, .DefaultValue = -1},
                        New NumParam With {.Switch = "-lag-in-frames", .Text = "Lag In Frames", .VisibleFunc = Function() Codec.OptionText = "VP9", .Value = 25, .DefaultValue = -1},
                        New BoolParam With {.Name = "h264_qsv Lookahead", .Text = "Lookahead", .Switch = "-look_ahead 1", .NoSwitch = "-look_ahead 0", .Value = False, .DefaultValue = True, .VisibleFunc = Function() Codec.ValueText = "h264_qsv"},
                        New BoolParam With {.Name = "h264_qsv VCM", .Text = "VCM", .Switch = "-vcm 1", .NoSwitch = "-vcm 0", .Value = False, .DefaultValue = True, .VisibleFunc = Function() Codec.ValueText = "h264_qsv"},
                        Custom)
                End If

                Return ItemsValue
            End Get
        End Property

        Overloads Overrides Function GetCommandLine(
            includePaths As Boolean,
            includeExecutable As Boolean,
            Optional pass As Integer = 1) As String

            Dim pipeTool = If(p.Script.IsAviSynth, PipingToolAVS, PipingToolVS).ValueText.ToLowerInvariant()
            Dim sb As New StringBuilder
            Dim sourcePath = p.Script.Path

            If includePaths AndAlso includeExecutable Then
                Select Case Decoder.ValueText.ToLowerInvariant()
                    Case "-"
                        Dim pipeString = ""

                        If pipeTool = "automatic" Then
                            pipeTool = If(p.Script.IsAviSynth, "avs2pipemod", "vspipe")
                        End If

                        Select Case pipeTool
                            Case "avs2pipemod"
                                sb.Append(Package.avs2pipemod.Path.Escape())
                                sb.Append(If(FrameServerHelp.IsPortable, $" -dll={Package.AviSynth.Path.Escape}", ""))
                                sb.Append(" -y4mp ")
                                'sb.Append($" --frames {endFrame - startFrame + 1}")
                                sb.Append(sourcePath.Escape())
                                sb.Append(" | ")
                                sb.Append(Package.ffmpeg.Path.Escape())
                                sb.Append(" -i -")
                            Case "vspipe"
                                sb.Append(Package.vspipe.Path.Escape())
                                sb.Append(" ")
                                sb.Append(sourcePath.Escape())
                                sb.Append(" - --container y4m")
                                'sb.Append($" --start {startFrame} --end {endFrame}")
                                sb.Append(" | ")
                                sb.Append(Package.ffmpeg.Path.Escape())
                                sb.Append(" -i -")
                            Case "ffmpeg"
                                sb.Append(Package.ffmpeg.Path.Escape())
                                sb.Append(If(sourcePath.Ext = "vpy", " -f vapoursynth", ""))
                                sb.Append(" -i ")
                                sb.Append(sourcePath.Escape())
                                sb.Append(" -f yuv4mpegpipe -strict -1 -loglevel fatal -hide_banner - | ")
                                sb.Append(Package.ffmpeg.Path.Escape())
                                sb.Append(" -i -")
                        End Select
                    Case "sw"
                        sourcePath = p.LastOriginalSourceFile
                        sb.Append(Package.ffmpeg.Path.Escape())
                        sb.Append(" -i ")
                        sb.Append(sourcePath.Escape())
                    Case "qsv"
                        sourcePath = p.LastOriginalSourceFile
                        sb.Append(Package.ffmpeg.Path.Escape())
                        sb.Append(" -hwaccel qsv")
                        sb.Append(" -i ")
                        sb.Append(sourcePath.Escape())
                    Case "dxva2"
                        sourcePath = p.LastOriginalSourceFile
                        sb.Append(Package.ffmpeg.Path.Escape())
                        sb.Append(" -hwaccel dxva2")
                        sb.Append(" -i ")
                        sb.Append(sourcePath.Escape())
                    Case "cuda"
                        sourcePath = p.LastOriginalSourceFile
                        sb.Append(Package.ffmpeg.Path.Escape())
                        sb.Append(" -hwaccel cuda -hwaccel_output_format cuda")
                        sb.Append(" -i ")
                        sb.Append(sourcePath.Escape())
                End Select
            End If

            Dim items = From i In Me.Items Where i.GetArgs <> "" AndAlso Not IsCustom(i.Switch)

            If items.Count > 0 Then
                sb.Append(" " + items.Select(Function(item) item.GetArgs).Join(" "))
            End If
            If Calc.IsARSignalingRequired Then
                sb.Append(" -aspect " + Calc.GetTargetDAR.ToInvariantString.Shorten(8))
            End If
            Select Case Mode.Value
                Case EncodingMode.TwoPass
                    sb.Append(" -pass " & pass)
                    sb.Append($" -b:v {p.VideoBitrate}k")
                    If pass = 1 Then
                        sb.Append(" -f rawvideo")
                    End If
                    If includePaths Then
                        sb.Append(" -passlogfile " + (Path.Combine(p.TempDir, p.TargetFile.Base + "_2pass")).Escape)
                    End If
                Case EncodingMode.OnePass
                    sb.Append($" -b:v {p.VideoBitrate}k")
            End Select
            Select Case Codec.OptionText
                Case "XviD"
                    sb.Append(" -tag:v xvid")
            End Select
            Dim targetPath As String
            If Mode.OptionText = "Two Pass" AndAlso pass = 1 Then
                targetPath = "NUL"
            Else
                targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt).Escape
            End If

            If includePaths Then
                sb.Append(" -an -y -hide_banner " + targetPath)
            End If

            Dim ret = sb.ToString.Trim

            If ret.Contains("%") Then
                ret = Macro.Expand(ret)
            End If

            Return ret
        End Function

        Function IsCustom(switch As String) As Boolean
            If switch = "" Then
                Return False
            End If

            If Custom.Value?.Contains(switch + " ") OrElse Custom.Value?.EndsWith(switch) Then
                Return True
            End If
            Return False
        End Function

        Function GetQualityArgs() As String
            If Mode.Value = EncodingMode.Quality Then
                Dim param = GetNumParamByName("Quality")

                If param.Value <> param.DefaultValue Then
                    If Codec.OptionText.EqualsAny("VP8", "VP9") Then
                        Return "-crf " + param.Value.ToInvariantString & " -b:v 0"
                    ElseIf Codec.OptionText.EqualsAny("x264", "x265", "AV1") Then
                        Return "-crf " + param.Value.ToInvariantString
                    ElseIf Codec.ValueText.EqualsAny("h264_nvenc") Then
                        If h264_nvenc_rc.OptionText = "Constqp" Then
                            Return "-qp " + param.Value.ToInvariantString
                        Else
                            Return "-cq " + param.Value.ToInvariantString
                        End If
                    ElseIf Codec.ValueText.EqualsAny("hevc_nvenc") Then
                        Return "-cq " + param.Value.ToInvariantString
                    Else
                        Return "-q:v " + param.Value.ToInvariantString
                    End If
                End If
            End If
            Return ""
        End Function
    End Class

    Enum EncodingMode
        Quality
        OnePass
        TwoPass
    End Enum
End Class
