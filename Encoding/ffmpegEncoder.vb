Imports System.Globalization
Imports System.Text
Imports StaxRip.UI
Imports StaxRip.CommandLine

<Serializable()>
Class ffmpegEncoder
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "ffmpeg | " + Params.Codec.OptionText
        End Get
    End Property

    Sub New()
        Muxer = New ffmpegMuxer("AVI")
    End Sub

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
                                        Dim enc = ObjectHelp.GetCopy(Of ffmpegEncoder)(Me)
                                        Dim params2 As New EncoderParams
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            f.cms.Items.Add(New ActionMenuItem("Save Profile...", saveProfileAction))
            f.cms.Items.Add(New ActionMenuItem("Codec Help", Sub() g.ShowCode(newParams.Codec.OptionText + " Help", ProcessHelp.GetStdOut(Package.ffmpeg.Path, "-hide_banner -h encoder=" + newParams.Codec.ValueText))))

            If f.ShowDialog() = DialogResult.OK Then
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides ReadOnly Property OutputFileType() As String
        Get
            Select Case Params.Codec.OptionText
                Case "Xvid", "ASP"
                    Return "avi"
                Case "ProRes"
                    Return "mov"
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
            Encode(Params.GetCommandLine(True, False, 1))
            Encode(Params.GetCommandLine(True, False, 2))
        Else
            Encode(Params.GetCommandLine(True, False))
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(args As String)
        Using proc As New Proc
            proc.Init("Encoding " + Params.Codec.OptionText + " using ffmpeg " + Package.ffmpeg.Version, "frame=")
            proc.Encoding = Encoding.UTF8
            proc.WorkingDirectory = p.TempDir
            proc.File = Package.ffmpeg.Path
            proc.Arguments = args
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

    Class EncoderParams
        Inherits CommandLineParams

        Sub New()
            Title = "ffmpeg Encoding Options"
        End Sub

        Property Codec As New OptionParam With {
            .Switch = "-c:v",
            .Text = "Codec:",
            .AlwaysOn = True,
            .Options = {"x264", "x265", "VP9", "Xvid", "ASP", "Theora", "ProRes", "H.264 Intel", "H.265 Intel", "H.264 NVIDIA"},
            .Values = {"libx264", "libx265", "libvpx-vp9", "libxvid", "mpeg4", "libtheora", "prores", "h264_qsv", "hevc_qsv", "nvenc_h264"}}

        Property Quality As New NumParam With {
            .Name = "Quality",
            .Text = "Quality:",
            .VisibleFunc = Function() Mode.Value = EncodingMode.Quality AndAlso Not Codec.OptionText.EqualsAny("ProRes"),
            .MinMaxStep = {1, 63, 1},
            .Value = 25}

        Property Mode As New OptionParam With {
            .Name = "Mode",
            .Text = "Mode:",
            .VisibleFunc = Function() Not Codec.OptionText.EqualsAny("ProRes"),
            .Options = {"Quality", "One Pass", "Two Pass"}}

        Property Speed As New OptionParam With {
            .Switch = "-speed",
            .Text = "Speed:",
            .AlwaysOn = True,
            .VisibleFunc = Function() Codec.OptionText = "VP9",
            .Options = {"6 - Fastest", "5 - Faster", "4 - Fast", "3 - Medium", "2 - Slow", "1 - Slower", "0 - Slowest"},
            .Values = {"6", "5", "4", "3", "2", "1", "0"},
            .Value = 5}

        Property AQmode As New OptionParam With {
            .Switch = "-aq-mode",
            .Text = "AQ Mode:",
            .VisibleFunc = Function() Codec.OptionText = "VP9",
            .Options = {"Disabled", "0", "1", "2", "3"},
            .Values = {"Disabled", "0", "1", "2", "3"}}

        Property EncodingThreads As New NumParam With {
            .Switch = "-threads",
            .Text = "Encoding Threads:",
            .VisibleFunc = Function() Codec.OptionText = "VP9",
            .Value = 8,
            .DefaultValue = -1}

        Property TileColumns As New NumParam With {
            .Switch = "-tile-columns",
            .Text = "Tile Columns:",
            .VisibleFunc = Function() Codec.OptionText = "VP9",
            .Value = 6,
            .DefaultValue = -1}

        Property FrameParallel As New NumParam With {
            .Switch = "-frame-parallel",
            .Text = "Frame Parallel:",
            .VisibleFunc = Function() Codec.OptionText = "VP9",
            .Value = 1,
            .DefaultValue = -1}

        Property AutoAltRef As New NumParam With {
            .Switch = "-auto-alt-ref",
            .Text = "Auto Alt Ref:",
            .VisibleFunc = Function() Codec.OptionText = "VP9",
            .Value = 1,
            .DefaultValue = -1}

        Property LagInFrames As New NumParam With {
            .Switch = "-lag-in-frames",
            .Text = "Lag In Frames:",
            .VisibleFunc = Function() Codec.OptionText = "VP9",
            .Value = 25,
            .DefaultValue = -1}

        Property DecodingThreads As New NumParam With {
            .Text = "Decoding Threads:",
            .MinMaxStep = {0, 64, 1}}

        Property Decoder As New OptionParam With {
            .Text = "Decoder:",
            .Options = {"AviSynth/VapourSynth", "Intel", "DXVA2"},
            .Values = {"avs", "qsv", "dxva2"}}

        Property Custom As New StringParam With {
            .Text = "Custom Switches:"}

        Property ProResProfile As New OptionParam With {
            .Switch = "-profile:v",
            .Text = "Profile:",
            .VisibleFunc = Function() Codec.OptionText = "ProRes",
            .InitValue = 3,
            .IntegerValue = True,
            .Options = {"Proxy", "LT", "Normal", "HQ"}}

        Private ItemsValue As List(Of CommandLineParam)

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)

                    ItemsValue.AddRange({Decoder, Codec, Mode,
                                        New OptionParam With {.Name = "x264/x265 preset", .Text = "Preset:", .Switch = "-preset", .InitValue = 5, .Options = {"ultrafast", "superfast", "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow", "placebo"}, .VisibleFunc = Function() Codec.OptionText.EqualsAny("x264", "x265")},
                                        New OptionParam With {.Name = "x264/x265 tune", .Text = "Tune:", .Switch = "-tune", .Options = {"none", "film", "animation", "grain", "stillimage", "psnr", "ssim", "fastdecode", "zerolatency"}, .VisibleFunc = Function() Codec.OptionText.EqualsAny("x264", "x265")},
                                        ProResProfile, Speed, AQmode,
                                        Quality, DecodingThreads, EncodingThreads, TileColumns,
                                        FrameParallel, AutoAltRef, LagInFrames,
                                        New BoolParam With {.Name = "h264_qsv Lookahead", .Text = "Lookahead", .Switch = "-look_ahead 1", .NoSwitch = "-look_ahead 0", .Value = False, .DefaultValue = True, .VisibleFunc = Function() Codec.ValueText = "h264_qsv"},
                                        New BoolParam With {.Name = "h264_qsv VCM", .Text = "VCM", .Switch = "-vcm 1", .NoSwitch = "-vcm 0", .Value = False, .DefaultValue = True, .VisibleFunc = Function() Codec.ValueText = "h264_qsv"},
                                        Custom})
                End If

                Return ItemsValue
            End Get
        End Property

        Overloads Overrides Function GetCommandLine(includePaths As Boolean,
                                                    includeExecutable As Boolean,
                                                    Optional pass As Integer = 1) As String
            Dim sourcePath = p.Script.Path
            Dim ret As String

            If includePaths AndAlso includeExecutable Then ret = Package.ffmpeg.Path.Quotes

            Select Case Decoder.ValueText
                Case "qsv"
                    sourcePath = p.LastOriginalSourceFile
                    ret += " -hwaccel qsv"
                Case "dxva2"
                    sourcePath = p.LastOriginalSourceFile
                    ret += " -hwaccel dxva2"
            End Select

            If DecodingThreads.Value <> DecodingThreads.DefaultValue Then
                ret += " -threads " + DecodingThreads.Value.ToString
            End If

            If includePaths Then ret += " -i " + sourcePath.Quotes
            Dim items = From i In Me.Items Where i.GetArgs <> ""
            If items.Count > 0 Then ret += " " + items.Select(Function(item) item.GetArgs).Join(" ")

            If Calc.IsARSignalingRequired Then
                ret += " -aspect " + Calc.GetTargetDAR.ToString(CultureInfo.InvariantCulture).Shorten(8)
            End If

            Select Case Mode.Value
                Case EncodingMode.Quality
                    If Codec.OptionText = "VP9" Then
                        ret += " -crf " & Quality.Value & " -b:v 0"
                    ElseIf Codec.OptionText.EqualsAny("x264", "x265") Then
                        ret += " -crf " & Quality.Value
                    Else
                        ret += " -q:v " & Quality.Value
                    End If
                Case EncodingMode.TwoPass
                    ret += " -pass " & pass
                    ret += $" -b:v {p.VideoBitrate}k"
                    If pass = 1 Then ret += " -f rawvideo"
                Case EncodingMode.OnePass
                    ret += $" -b:v {p.VideoBitrate}k"
            End Select

            If Codec.OptionText = "Xvid" Then ret += " -tag:v xvid"

            Dim targetPath As String

            If Mode.OptionText = "Two Pass" AndAlso pass = 1 Then
                targetPath = "NUL"
            Else
                targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputFileType).Quotes
            End If

            ret += " -an -y -hide_banner"
            If includePaths Then ret += " " + targetPath
            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.ffmpeg
        End Function
    End Class

    Enum EncodingMode
        Quality
        OnePass
        TwoPass
    End Enum
End Class