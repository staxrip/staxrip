
Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class VCEEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "AMD | " + Params.Codec.OptionText.Replace("AMD ", "")
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

        Using f As New CommandLineForm(params1)
            Dim saveProfileAction = Sub()
                                        Dim enc = ObjectHelp.GetCopy(Of VCEEnc)(Me)
                                        Dim params2 As New EncoderParams
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            f.cms.Items.Add(New ActionMenuItem("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetConsoleOutput(Package.VCEEnc.Path, "--check-features"))))
            f.cms.Items.Add(New ActionMenuItem("Check VCE Support", Sub() MsgInfo(ProcessHelp.GetConsoleOutput(Package.VCEEnc.Path, "--check-hw"))))
            ActionMenuItem.Add(f.cms.Items, "Save Profile...", saveProfileAction, Symbol.Save)

            If f.ShowDialog() = DialogResult.OK Then
                Params = params1
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

        Using proc As New Proc
            proc.Header = "Video encoding"
            proc.Package = Package.VCEEnc
            proc.SkipStrings = {"%]", " frames: "}
            proc.File = "cmd.exe"
            proc.Arguments = "/S /C """ + Params.GetCommandLine(True, True) + """"
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
            Return Params.Mode.Value = 0
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

        tester.IgnoredSwitches = "audio-bitrate audio-codec video-streamid video-track vpy vpy-mt
            audio-filter avsw device input-analyze caption2ass audio-file sub-copy version audio-copy
            audio-ignore-decode-error audio-ignore-notrack-error audio-resampler raw help input-file
            audio-samplerate audio-source audio-stream avs avvce-analyze output-file seek skip-frame
            check-avversion check-codecs check-decoders check-encoders check-filters check-protocols
            check-formats dar format fps input-res log-framelist mux-option"

        tester.Package = Package.VCEEnc
        tester.CodeFile = Folder.Startup.Parent + "Encoding\vceenc.vb"

        Return tester.Test
    End Function

    Public Class EncoderParams
        Inherits CommandLineParams

        Sub New()
            Title = "VCEEnc Options"
        End Sub

        Property Mode As New OptionParam With {
            .Name = "Mode",
            .Text = "Mode",
            .Options = {"CQP - Constant QP", "CBR - Constant Bitrate", "VBR - Variable Bitrate"}}

        Property Codec As New OptionParam With {
            .Switch = "--codec",
            .Text = "Codec",
            .Options = {"AMD H.264", "AMD H.265"},
            .Values = {"h264", "hevc"}}

        Property Decoder As New OptionParam With {
            .Text = "Decoder",
            .Options = {"AviSynth/VapourSynth", "VCEEnc (VCE)", "QSVEnc (Intel)", "ffmpeg (Intel)", "ffmpeg (DXVA2)"},
            .Values = {"avs", "vce", "qs", "ffqsv", "ffdxva"}}

        Property QPI As New NumParam With {
            .Text = "QP I",
            .Value = 22,
            .VisibleFunc = Function() Mode.Value = 0,
            .Config = {0, 51}}

        Property QPP As New NumParam With {
            .Text = "QP P",
            .Value = 24,
            .VisibleFunc = Function() Mode.Value = 0,
            .Config = {0, 51}}

        Property QPB As New NumParam With {
            .Text = "QP B",
            .Value = 27,
            .VisibleFunc = Function() Mode.Value = 0,
            .Config = {0, 51}}

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)
                    Add("Basic", Decoder, Mode, Codec,
                        New OptionParam With {.Switch = "--quality", .Text = "Preset", .Options = {"Fast", "Balanced", "Slow"}, .Init = 1},
                        New OptionParam With {.Switch = "--profile", .Name = "profile264", .VisibleFunc = Function() Codec.ValueText = "h264", .Text = "Profile", .Options = {"Automatic", "Baseline", "Main", "High"}},
                        New OptionParam With {.Switch = "--profile", .Name = "profile265", .VisibleFunc = Function() Codec.ValueText = "hevc", .Text = "Profile", .Options = {"Main"}},
                        New OptionParam With {.Switch = "--level", .Name = "LevelH264", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "h264", .Options = {"Unrestricted", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                        New OptionParam With {.Switch = "--level", .Name = "LevelH265", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "hevc", .Options = {"Unrestricted", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}},
                        QPI, QPP, QPB)
                    Add("Slice Decision",
                        New NumParam With {.Switch = "--slices", .Text = "Slices", .Init = 1},
                        New NumParam With {.Switch = "--bframes", .Text = "B-Frames", .Config = {0, 16}},
                        New NumParam With {.Switch = "--ref", .Text = "Ref Frames", .Init = 2, .Config = {0, 16}},
                        New NumParam With {.Switch = "--gop-len", .Text = "GOP Length", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--ltr", .Text = "LTR Frames", .Config = {0, Integer.MaxValue, 1}},
                        New BoolParam With {.Switch = "--b-pyramid", .Text = "B-Pyramid"})
                    Add("Rate Control",
                        New NumParam With {.Switch = "--max-bitrate", .Text = "Max Bitrate", .Init = 20000, .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--vbv-bufsize", .Text = "VBV Bufsize", .Config = {0, 1000000}, .Init = 20000},
                        New NumParam With {.Switch = "--qp-min", .Text = "QP Min", .Config = {0, 100}},
                        New NumParam With {.Switch = "--qp-max", .Text = "QP Max", .Config = {0, 100}, .Init = 100},
                        New NumParam With {.Switch = "--b-deltaqp", .Text = "Non-ref Bframe QP Offset"},
                        New NumParam With {.Switch = "--bref-deltaqp", .Text = "Ref Bframe QP Offset"})
                    Add("VUI",
                        New StringParam With {.Switch = "--sar", .Text = "Sample Aspect Ratio", .Init = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                        New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat", .Options = {"Undefined", "NTSC", "Component", "PAL", "SECAM", "MAC"}},
                        New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix", .Options = {"Undefined", "BT 2020 C", "BT 2020 NC", "BT 470 BG", "BT 709", "FCC", "GBR", "SMPTE 170 M", "SMPTE 240 M", "YCgCo"}},
                        New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim", .Options = {"Undefined", "BT 2020", "BT 470 BG", "BT 470 M", "BT 709", "Film", "SMPTE 170 M", "SMPTE 240 M"}},
                        New OptionParam With {.Switch = "--transfer", .Text = "Transfer", .Options = {"Undefined", "ARIB-SRD-B67", "Auto", "BT 1361 E", "BT 2020-10", "BT 2020-12", "BT 470 BG", "BT 470 M", "BT 709", "IEC 61966-2-1", "IEC 61966-2-4", "Linear", "Log 100", "Log 316", "SMPTE 170 M", "SMPTE 240 M", "SMPTE 2084", "SMPTE 428"}},
                        New BoolParam With {.Switch = "--enforce-hrd", .Text = "Enforce HRD compatibility"})
                    Add("Other",
                        New StringParam With {.Text = "Custom", .Quotes = QuotesMode.Never, .AlwaysOn = True},
                        New StringParam With {.Switch = "--chapter", .Text = "Chapters", .BrowseFile = True},
                        New StringParam With {.Switch = "--log", .Text = "Log File", .BrowseFile = True},
                        New OptionParam With {.Switch = "--tier", .Text = "Tier", .Options = {"Main", "High"}, .VisibleFunc = Function() Codec.ValueText = "hevc"},
                        New OptionParam With {.Switch = "--log-level", .Text = "Log Level", .Options = {"Info", "Debug", "Warn", "Error"}},
                        New OptionParam With {.Switch = "--motion-est", .Text = "Motion Estimation", .Options = {"Q-pel", "Full-pel", "Half-pel"}},
                        New BoolParam With {.Switch = "--chapter-copy", .Text = "Copy Chapters"},
                        New BoolParam With {.Switch = "--filler", .Text = "Use filler data"})

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
            g.ShowCommandLineHelp(Package.VCEEnc, id)
        End Sub

        Overrides Function GetCommandLine(includePaths As Boolean,
                                          includeExecutable As Boolean,
                                          Optional pass As Integer = 1) As String
            Dim ret As String
            Dim sourcePath As String
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)

            If includePaths AndAlso includeExecutable Then
                ret = Package.VCEEnc.Path.Escape
            End If

            Select Case Decoder.ValueText
                Case "avs"
                    sourcePath = p.Script.Path
                Case "qs"
                    sourcePath = "-"

                    If includePaths Then
                        ret = If(includePaths, Package.QSVEnc.Path.Escape, "QSVEncC64") + " -o - -c raw" + " -i " + If(includePaths, p.SourceFile.Escape, "path") + " | " + If(includePaths, Package.VCEEnc.Path.Escape, "VCEEncC64")
                    End If
                Case "ffdxva"
                    sourcePath = "-"

                    If includePaths Then
                        Dim pix_fmt = If(p.SourceVideoBitDepth = 10, "yuv420p10le", "yuv420p")
                        ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") +
                            " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.SourceFile.Escape, "path") +
                            " -f yuv4mpegpipe -pix_fmt " + pix_fmt + " -strict -1 -loglevel fatal - | " +
                            If(includePaths, Package.VCEEnc.Path.Escape, "VCEEncC64")
                    End If
                Case "ffqsv"
                    sourcePath = "-"

                    If includePaths Then
                        ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.SourceFile.Escape, "path") + " -f yuv4mpegpipe -strict -1 -pix_fmt yuv420p -loglevel fatal - | " + If(includePaths, Package.VCEEnc.Path.Escape, "VCEEncC64")
                    End If
                Case "vce"
                    sourcePath = p.LastOriginalSourceFile
                    ret += " --avhw"
            End Select

            Dim q = From i In Items Where i.GetArgs <> ""

            If q.Count > 0 Then
                ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")
            End If

            Select Case Mode.Value
                Case 0
                    ret += " --cqp " & QPI.Value & ":" & QPP.Value & ":" & QPB.Value
                Case 1
                    ret += " --cbr " & p.VideoBitrate
                Case 2
                    ret += " --vbr " & p.VideoBitrate
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

            If sourcePath = "-" Then
                ret += " --y4m"
            End If

            If includePaths Then
                ret += " -i " + sourcePath.Escape + " -o " + targetPath.Escape
            End If

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.VCEEnc
        End Function
    End Class
End Class