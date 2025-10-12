
Imports System
Imports System.Drawing.Imaging
Imports System.Text
Imports System.Text.RegularExpressions
Imports StaxRip.UI
Imports StaxRip.VideoEncoderCommandLine

<Serializable()>
Public Class NVEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "NVEncC (Nvidia) | " + Params.Codec.OptionText.Replace("Nvidia ", "")
        End Get
    End Property

    Overloads Shared ReadOnly Property Package As Package
        Get
            Return Package.NVEncC
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

    Public Overrides Property OverridesTargetFileName As Boolean
        Get
            Return Params.OverrideTargetFileName.Value
        End Get
        Set(value As Boolean)
            If Params.OverrideTargetFileName.Value <> value Then
                Params.OverrideTargetFileName.Value = value
                Params.RaiseValueChanged(Params.OverrideTargetFileName)
            End If
        End Set
    End Property

    Public Overrides ReadOnly Property OverridingTargetFileName As String
        Get
            Dim value = Macro.ExpandParamValues(Params.TargetFileName.Value, Params.Items)
            value = value.Replace(Environment.NewLine, "")
            Return value
        End Get
    End Property

    Overrides ReadOnly Property IsDolbyVisionSet As Boolean
        Get
            If Not Params.DolbyVisionRpu.Visible Then Return False
            If Params.DolbyVisionProfileH265.Visible AndAlso Params.DolbyVisionProfileH265.Value <> 0 Then Return Not String.IsNullOrWhiteSpace(Params.DolbyVisionRpu.Value)
            If Params.DolbyVisionProfileAV1.Visible AndAlso Params.DolbyVisionProfileAV1.Value <> 0 Then Return Not String.IsNullOrWhiteSpace(Params.DolbyVisionRpu.Value)
            Return False
        End Get
    End Property

    Overrides ReadOnly Property IsOvercroppingAllowed As Boolean
        Get
            If Not Params.DolbyVisionRpu.Visible Then Return True
            Return String.IsNullOrWhiteSpace(Params.DolbyVisionRpu.Value)
        End Get
    End Property

    Overrides ReadOnly Property IsUnequalResizingAllowed As Boolean
        Get
            If Not Params.DolbyVisionRpu.Visible Then Return True
            Return String.IsNullOrWhiteSpace(Params.DolbyVisionRpu.Value)
        End Get
    End Property

    Overrides ReadOnly Property DolbyVisionMetadataPath As String
        Get
            If Not Params.DolbyVisionRpu.Visible Then Return Nothing
            Return Params.DolbyVisionRpu.Value
        End Get
    End Property

    Overrides ReadOnly Property Hdr10PlusMetadataPath As String
        Get
            If Not Params.DhdrInfo.Visible Then Return Nothing
            Return Params.DhdrInfo.Value
        End Get
    End Property


    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(codecIndex As Integer)
        MyBase.New()
        Params.Codec.Value = If(codecIndex > 0 AndAlso codecIndex < Params.Codec.Values.Length, codecIndex, 0)
    End Sub

    Overrides Sub ShowConfigDialog(Optional param As CommandLineParam = Nothing)
        Dim newParams As New EncoderParams
        Dim store = ObjectHelp.GetCopy(ParamsStore)
        newParams.Init(store)

        Using form As New CommandLineForm(newParams)
            form.HTMLHelpFunc = Function() $"<p><a href=""{Package.NVEncC.HelpURL}"">NVEncC Online Help</a></p>" +
                $"<h2>NVEncC Console Help</h2><pre>{HelpDocument.ConvertChars(Package.NVEncC.CreateHelpfile())}</pre>"

            Dim a = Sub()
                        Dim enc = ObjectHelp.GetCopy(Me)
                        Dim params2 As New EncoderParams
                        Dim store2 = ObjectHelp.GetCopy(store)
                        params2.Init(store2)
                        enc.Params = params2
                        enc.ParamsStore = store2
                        SaveProfile(enc)
                    End Sub

            form.cms.Add("Check Hardware", Sub() g.ShowCode("Check Hardware", ProcessHelp.GetConsoleOutput(Package.NVEncC.Path, "--check-hw")))
            form.cms.Add("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetConsoleOutput(Package.NVEncC.Path, "--check-features")), Keys.Control Or Keys.F)
            form.cms.Add("Check Environment", Sub() g.ShowCode("Check Environment", ProcessHelp.GetConsoleOutput(Package.NVEncC.Path, "--check-environment")))
            form.cms.Add("-")
            form.cms.Add("Save Profile...", a, Keys.Control Or Keys.S, Symbol.Save)

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
            If Params.Codec.ValueText = "av1" Then
                Return Muxer.OutputExt
            End If

            Return Params.Codec.ValueText
        End Get
    End Property

    Overrides Property Bitrate As Integer
        Get
            Return CInt(Params.Bitrate.Value)
        End Get
        Set(value As Integer)
            Params.Bitrate.Value = value
        End Set
    End Property

    Overrides Sub Encode()
        If OutputExt = "h265" Then
            Dim codecs = ProcessHelp.GetConsoleOutput(Package.NVEncC.Path, "--check-hw").Right("Codec(s)")

            If Not codecs.ToLowerInvariant.Contains("hevc") Then
                Throw New ErrorAbortException("NVEncC Error", "H.265/HEVC isn't supported by the graphics card.")
            End If
        End If

        p.Script.Synchronize()

        Using proc As New Proc
            proc.Header = "Video encoding"
            proc.Package = Package.NVEncC
            proc.SkipStrings = {"%]", " frames: "}
            proc.File = "cmd.exe"
            proc.Arguments = "/S /C """ + Params.GetCommandLine(True, True) + """"
            proc.Start()
        End Using
    End Sub

    Overrides Function BeforeEncoding() As Boolean
        Dim rpu = Params.DolbyVisionRpu.Value
        If Not String.IsNullOrWhiteSpace(rpu) AndAlso rpu = p.HdrDolbyVisionMetadataFile?.Path AndAlso rpu.FileExists() Then
            Dim offset = If(p.Script.IsFilterActive("Crop"), New Padding(p.CropLeft, p.CropTop, p.CropRight, p.CropBottom), Padding.Empty)
            Dim rpuProfile = p.HdrDolbyVisionMetadataFile.ReadProfileFromRpu()

            Dim profile = "#"
            If Params.DolbyVisionProfileH265.Visible Then profile = Params.DolbyVisionProfileH265.ValueText
            If Params.DolbyVisionProfileAV1.Visible Then profile = Params.DolbyVisionProfileAV1.ValueText
            If profile = "#" Then Return False

            Dim mode = DoviMode.Mode0
            If profile = "8.1" Then mode = DoviMode.Mode2
            If profile = "8.2" Then mode = DoviMode.Mode2
            If profile = "8.4" Then mode = DoviMode.Mode4
            If profile = "10.1" Then mode = DoviMode.Mode2
            If profile = "10.2" Then mode = DoviMode.Mode2
            If profile = "10.4" Then mode = DoviMode.Mode4

            p.HdrDolbyVisionMetadataFile.WriteEditorConfigFile(offset, mode, True)
            Dim newPath = p.HdrDolbyVisionMetadataFile.WriteModifiedRpu(True)

            If p.HdrDolbyVisionMetadataFile.HasToBeTrimmed Then
                newPath = p.HdrDolbyVisionMetadataFile.TrimRpu()
            End If

            If Not String.IsNullOrWhiteSpace(newPath) Then
                Params.DolbyVisionRpu.Value = newPath
            Else
                Return False
            End If
        End If
        Return True
    End Function

    Overrides Sub SetMetaData(sourceFile As String)
        If Not p.ImportVUIMetadata Then Exit Sub

        Dim cl = ""
        Dim colour_primaries = MediaInfo.GetVideo(sourceFile, "colour_primaries")

        Select Case colour_primaries
            Case "BT.2020"
                If colour_primaries.Contains("BT.2020") Then
                    cl += " --colorprim bt2020"
                End If
            Case "BT.709"
                If colour_primaries.Contains("BT.709") Then
                    cl += " --colorprim bt709"
                End If
        End Select

        Dim transfer_characteristics = MediaInfo.GetVideo(sourceFile, "transfer_characteristics")

        Select Case transfer_characteristics
            Case "PQ", "SMPTE ST 2084"
                If transfer_characteristics.Contains("SMPTE ST 2084") Or transfer_characteristics.Contains("PQ") Then
                    cl += " --transfer smpte2084"
                End If
            Case "BT.709"
                If transfer_characteristics.Contains("BT.709") Then
                    cl += " --transfer bt709"
                End If
            Case "HLG"
                cl += " --transfer arib-std-b67"
        End Select

        Dim matrix_coefficients = MediaInfo.GetVideo(sourceFile, "matrix_coefficients")

        Select Case matrix_coefficients
            Case "BT.2020 non-constant"
                If matrix_coefficients.Contains("BT.2020 non-constant") Then
                    cl += " --colormatrix bt2020nc"
                End If
            Case "BT.709"
                cl += " --colormatrix bt709"
        End Select

        Dim color_range = MediaInfo.GetVideo(sourceFile, "colour_range")

        Select Case color_range
            Case "Limited"
                cl += " --range limited"
            Case "Full"
                cl += " --range full"
        End Select

        Dim ChromaSubsampling_Position = MediaInfo.GetVideo(sourceFile, "ChromaSubsampling_Position")
        Dim chromaloc = New String(ChromaSubsampling_Position.Where(Function(c) c.IsDigit()).ToArray())

        If Not String.IsNullOrEmpty(chromaloc) AndAlso chromaloc <> "0" Then
            cl += $" --chromaloc {chromaloc}"
        End If

        Dim MasteringDisplay_ColorPrimaries = MediaInfo.GetVideo(sourceFile, "MasteringDisplay_ColorPrimaries")
        Dim MasteringDisplay_Luminance = MediaInfo.GetVideo(sourceFile, "MasteringDisplay_Luminance")

        If MasteringDisplay_ColorPrimaries <> "" AndAlso MasteringDisplay_Luminance <> "" Then
            Dim luminanceMatch = Regex.Match(MasteringDisplay_Luminance, "min: ([\d\.]+) cd/m2, max: ([\d\.]+) cd/m2")

            If luminanceMatch.Success Then
                Dim luminanceMin = luminanceMatch.Groups(1).Value.ToDouble * 10000
                Dim luminanceMax = luminanceMatch.Groups(2).Value.ToDouble * 10000

                If MasteringDisplay_ColorPrimaries.Contains("Display P3") Then
                    cl += " --output-depth 10"
                    cl += $" --master-display ""G(13250,34500)B(7500,3000)R(34000,16000)WP(15635,16450)L({luminanceMax},{luminanceMin})"""
                    cl += " --hdr10"
                    cl += " --repeat-headers"
                    cl += " --range limited"
                    cl += " --hrd"
                    cl += " --aud"
                End If

                If MasteringDisplay_ColorPrimaries.Contains("DCI P3") Then
                    cl += " --output-depth 10"
                    cl += $" --master-display ""G(13250,34500)B(7500,3000)R(34000,16000)WP(15700,17550)L({luminanceMax},{luminanceMin})"""
                    cl += " --hdr10"
                    cl += " --repeat-headers"
                    cl += " --range limited"
                    cl += " --hrd"
                    cl += " --aud"
                End If

                If MasteringDisplay_ColorPrimaries.Contains("BT.2020") Then
                    cl += " --output-depth 10"
                    cl += $" --master-display ""G(8500,39850)B(6550,2300)R(35400,14600)WP(15635,16450)L({luminanceMax},{luminanceMin})"""
                    cl += " --hdr10"
                    cl += " --repeat-headers"
                    cl += " --range limited"
                    cl += " --hrd"
                    cl += " --aud"
                End If


                If Not String.IsNullOrWhiteSpace(p.Hdr10PlusMetadataFile) AndAlso p.Hdr10PlusMetadataFile.FileExists() Then
                    cl += $" --dhdr10-info ""{p.Hdr10PlusMetadataFile}"""
                End If
            End If
        End If

        If Not String.IsNullOrWhiteSpace(p.HdrDolbyVisionMetadataFile?.Path) Then
            Dim isAV1 = If(Params.DolbyVisionProfileAV1.Visible, True, False)
            Dim profileParam = If(isAV1, Params.DolbyVisionProfileAV1, Params.DolbyVisionProfileH265)
            Dim rpuProfile = p.HdrDolbyVisionMetadataFile.ReadProfileFromRpu()
            Dim profile = ""
            If rpuProfile = 5 Then profile = "5"
            If rpuProfile = 7 Then profile = "8.1"
            If rpuProfile = 8 Then profile = "8.1"
            If profile = "8.1" AndAlso transfer_characteristics = "HLG" Then profile = "8.4"
            If isAV1 AndAlso profile = "5" Then profile = "10.0"
            If isAV1 AndAlso profile = "8.1" Then profile = "10.1"
            If isAV1 AndAlso profile = "8.2" Then profile = "10.2"
            If isAV1 AndAlso profile = "8.4" Then profile = "10.4"

            If profileParam.Value = 0 AndAlso profile <> "" Then
                cl += $" --dolby-vision-profile {profile}"
            End If

            cl += " --output-depth 10"
            cl += $" --dolby-vision-rpu ""{p.HdrDolbyVisionMetadataFile.Path}"""
        End If

        Dim MaxCLL = MediaInfo.GetVideo(sourceFile, "MaxCLL").ToInt()
        Dim MaxFALL = MediaInfo.GetVideo(sourceFile, "MaxFALL").ToInt()

        If MaxCLL <> 0 OrElse MaxFALL <> 0 Then
            cl += $" --max-cll ""{MaxCLL},{MaxFALL}"""
        End If

        ImportCommandLine(cl)
    End Sub

    Overrides Function GetMenu() As MenuList
        Dim ret As New MenuList From {
            {"Encoder Options", AddressOf ShowConfigDialog},
            {"Container Configuration", AddressOf OpenMuxerConfigDialog}
        }
        Return ret
    End Function

    Overrides Property QualityMode() As Boolean
        Get
            Return Params.Mode.Value = 0 OrElse Params.Mode.Value = 1
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
        Dim tester As New ConsolAppTester With {
            .IgnoredSwitches = "help version check-device input-analyze input-format output-format
            video-streamid video-track vpp-delogo vpp-delogo-cb vpp-delogo-cr vpp-delogo-depth output
            vpp-delogo-pos vpp-delogo-select vpp-delogo-y check-avversion check-codecs log
            check-encoders check-decoders check-formats check-protocols log-framelist fps audio-delay
            check-filters input raw avs vpy vpy-mt key-on-chapter video-tag audio-ignore-decode-error
            avcuvid-analyze audio-source audio-file seek format audio-copy audio-ignore-notrack-error
            audio-copy audio-codec vpp-perf-monitor avi audio-profile check-profiles avsync mux-option
            audio-bitrate audio-ignore audio-ignore audio-samplerate audio-resampler audio-stream dar
            audio-stream audio-stream audio-stream audio-filter chapter-copy chapter sub-copy input-res
            audio-disposition audio-metadata option-list sub-disposition sub-metadata process-codepage
            metadata attachment-copy chapter-no-trim video-metadata input-csp sub-source",
            .UndocumentedSwitches = "",
            .Package = Package.NVEncC,
            .CodeFile = Path.Combine(Folder.Startup.Parent, "Encoding", "nvenc.vb")
        }

        Return tester.Test
    End Function

    Public Class EncoderParams
        Inherits CommandLineParams

        Sub New()
            Title = "NVEncC Options"
        End Sub

        Public Overrides ReadOnly Property Package As Package
            Get
                Return NVEnc.Package
            End Get
        End Property

        Property OverrideTargetFileName As New BoolParam() With {
            .Text = "Override Target File Name",
            .Init = False}

        Property TargetFileName As New StringParam With {
            .Text = "Target File Name",
            .Quotes = QuotesMode.Never,
            .TextChangedAction = Sub(text) TargetFileNamePreview.Value = Macro.ExpandParamValues(text, Items),
            .Init = "%source_name%_new",
            .InitAction = Sub(tb)
                              tb.Edit.MultilineHeightFactor = 6
                              tb.Edit.TextBox.Font = FontManager.GetCodeFont()
                          End Sub}

        Property TargetFileNamePreview As New StringParam With {
            .Text = "Preview",
            .Quotes = QuotesMode.Never,
            .InitAction = Sub(tb)
                              tb.Edit.MultilineHeightFactor = 3
                              tb.Edit.TextBox.Font = FontManager.GetCodeFont()
                              tb.Edit.TextBox.ReadOnly = True
                              BlockValueChanged = True
                              .Value = Macro.ExpandParamValues(TargetFileName.Value, Items)
                              BlockValueChanged = False
                          End Sub}

        Property Decoder As New OptionParam With {
            .Text = "Decoder",
            .Options = {"AviSynth/VapourSynth",
                        "NVEnc Hardware",
                        "NVEnc Software",
                        "QSVEnc (Intel)",
                        "ffmpeg (Intel)",
                        "ffmpeg (DXVA2)"},
            .Values = {"avs", "avhw", "avsw", "qs", "ffqsv", "ffdxva"}}

        Property Mode As New OptionParam With {
            .Text = "Mode",
            .Switches = {"--qvbr", "--cqp", "--cbr", "--vbr"},
            .Options = {"QVBR: Constant Quality Mode",
                         "CQP: Constant QP",
                         "CBR: Constant Bitrate",
                         "VBR: Variable Bitrate"},
            .VisibleFunc = Function() Not Lossless.Value,
            .ArgsFunc = AddressOf GetModeArgs,
            .ImportAction = Sub(param, arg)
                                If Mode.Switches.Contains(param) Then
                                    Mode.Value = Array.IndexOf(Mode.Switches.ToArray, param)
                                End If
                            End Sub}

        Property Codec As New OptionParam With {
            .Switch = "--codec",
            .HelpSwitch = "-c",
            .Text = "Codec",
            .Options = {"H.264", "H.265", "AV1"},
            .Values = {"h264", "h265", "av1"},
            .ImportAction = Sub(param, arg) Codec.Value = If(arg.EqualsAny("h264", "avc"), 0, If(arg.EqualsAny("h265", "hevc"), 1, 2))}

        Property ProfileH264 As New OptionParam With {
            .Switch = "--profile",
            .Text = "Profile",
            .Name = "ProfileH264",
            .VisibleFunc = Function() Codec.ValueText = "h264",
            .Options = {"Auto", "Baseline", "Main", "High", "High 444"},
            .Init = 2}

        Property ProfileH265 As New OptionParam With {
            .Switch = "--profile",
            .Text = "Profile",
            .Name = "ProfileH265",
            .VisibleFunc = Function() Codec.ValueText = "h265",
            .Options = {"Auto", "Main", "Main 10", "Main 444"}}

        Property ProfileAV1 As New OptionParam With {
            .Switch = "--profile",
            .Text = "Profile",
            .Name = "ProfileAV1",
            .VisibleFunc = Function() Codec.ValueText = "av1",
            .Options = {"Auto", "Main", "High"}}

        Property OutputDepth As New OptionParam With {
            .Switch = "--output-depth",
            .Text = "Output Depth",
            .Options = {"8-Bit", "10-Bit"},
            .Values = {"8", "10"},
            .Init = 0}

        Property Bitrate As New NumParam With {
            .Switches = {"--cbr", "--vbr"},
            .Text = "Bitrate",
            .Init = 5000,
            .VisibleFunc = Function() Mode.Value > 1,
            .Config = {0, 1000000, 100}}

        Property VbrQuality As New NumParam With {
            .Switch = "--vbr-quality",
            .Text = "VBR Quality",
            .Config = {0, 51, 0.5, 1},
            .Init = 0,
            .VisibleFunc = Function() Mode.Value = 3}

        Property QPAdvanced As New BoolParam With {
            .Text = "Show advanced QP settings",
            .VisibleFunc = Function() Mode.Value = 1}

        Property QVBR As New NumParam With {
            .Switches = {"--qvbr"},
            .Text = "VBR Quality",
            .Init = 28,
            .VisibleFunc = Function() Mode.Value = 0 AndAlso Not (QPAdvanced.Value AndAlso QPAdvanced.Visible),
            .Config = {0, 51, 0.5, 1}}

        Property QP As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP",
            .Init = 18,
            .VisibleFunc = Function() Mode.Value = 1 AndAlso Codec.Value <> 2 AndAlso Not (QPAdvanced.Value AndAlso QPAdvanced.Visible),
            .Config = {0, 63}}

        Property QPAV1 As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP",
            .Init = 50,
            .VisibleFunc = Function() Mode.Value = 1 AndAlso Codec.Value = 2 AndAlso Not (QPAdvanced.Value AndAlso QPAdvanced.Visible),
            .Config = {0, 255}}

        Property QPI As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP I",
            .Init = 18,
            .VisibleFunc = Function() Codec.Value <> 2 AndAlso QPAdvanced.Value AndAlso QPAdvanced.Visible,
            .Config = {0, 63}}

        Property QPIAV1 As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP I",
            .Init = 50,
            .VisibleFunc = Function() Codec.Value = 2 AndAlso QPAdvanced.Value AndAlso QPAdvanced.Visible,
            .Config = {0, 255}}

        Property QPP As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP P",
            .Init = 20,
            .VisibleFunc = Function() Codec.Value <> 2 AndAlso QPAdvanced.Value AndAlso QPAdvanced.Visible,
            .Config = {0, 63}}

        Property QPPAV1 As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP P",
            .Init = 70,
            .VisibleFunc = Function() Codec.Value = 2 AndAlso QPAdvanced.Value AndAlso QPAdvanced.Visible,
            .Config = {0, 255}}

        Property QPB As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP B",
            .Init = 24,
            .VisibleFunc = Function() Codec.Value <> 2 AndAlso QPAdvanced.Value AndAlso QPAdvanced.Visible,
            .Config = {0, 63}}

        Property QPBAV1 As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP B",
            .Init = 100,
            .VisibleFunc = Function() Codec.Value = 2 AndAlso QPAdvanced.Value AndAlso QPAdvanced.Visible,
            .Config = {0, 255}}

        Property QPInitAdvanced As New BoolParam With {.Text = "Show advanced Initial QP settings", .Init = False}
        Property QPInit8 As New NumParam With {.Switch = "--qp-init", .Text = "Initial QP", .Init = 20, .Config = {0, 51, 1}, .VisibleFunc = Function() Not QPInitAdvanced.Value AndAlso Codec.Value <> 2 AndAlso OutputDepth.Value = 0, .ImportAction = AddressOf ImportQpInitArgs}
        Property QPInit8I As New NumParam With {.Switch = "--qp-init", .Text = "Initial QP I", .DefaultValue = -1, .Value = 20, .Config = {0, 51, 1}, .VisibleFunc = Function() QPInitAdvanced.Value AndAlso Codec.Value <> 2 AndAlso OutputDepth.Value = 0, .ArgsFunc = AddressOf GetQpInitArgs, .ImportAction = AddressOf ImportQpInitArgs}
        Property QPInit8P As New NumParam With {.HelpSwitch = "--qp-init", .Text = "Initial QP P", .DefaultValue = -1, .Value = 24, .Config = {0, 51, 1}, .VisibleFunc = QPInit8I.VisibleFunc}
        Property QPInit8B As New NumParam With {.HelpSwitch = "--qp-init", .Text = "Initial QP B", .DefaultValue = -1, .Value = 28, .Config = {0, 51, 1}, .VisibleFunc = QPInit8I.VisibleFunc}
        Property QPInit10 As New NumParam With {.Switch = "--qp-init", .Text = "Initial QP", .Init = 20, .Config = {0, 63, 1}, .VisibleFunc = Function() Not QPInitAdvanced.Value AndAlso Codec.Value <> 2 AndAlso OutputDepth.Value = 1, .ImportAction = AddressOf ImportQpInitArgs}
        Property QPInit10I As New NumParam With {.Switch = "--qp-init", .Text = "Initial QP I", .DefaultValue = -1, .Value = 20, .Config = {0, 63, 1}, .VisibleFunc = Function() QPInitAdvanced.Value AndAlso Codec.Value <> 2 AndAlso OutputDepth.Value = 1, .ArgsFunc = AddressOf GetQpInitArgs, .ImportAction = AddressOf ImportQpInitArgs}
        Property QPInit10P As New NumParam With {.HelpSwitch = "--qp-init", .Text = "Initial QP P", .DefaultValue = -1, .Value = 24, .Config = {0, 63, 1}, .VisibleFunc = QPInit10I.VisibleFunc}
        Property QPInit10B As New NumParam With {.HelpSwitch = "--qp-init", .Text = "Initial QP B", .DefaultValue = -1, .Value = 28, .Config = {0, 63, 1}, .VisibleFunc = QPInit10I.VisibleFunc}
        Property QPInitAV1 As New NumParam With {.Switch = "--qp-init", .Text = "Initial QP", .Init = 20, .Config = {0, 255, 1}, .VisibleFunc = Function() Not QPInitAdvanced.Value AndAlso Codec.Value = 2, .ImportAction = AddressOf ImportQpInitArgs}
        Property QPInitAV1I As New NumParam With {.Switch = "--qp-init", .Text = "Initial QP I", .DefaultValue = -1, .Value = 20, .Config = {0, 255, 1}, .VisibleFunc = Function() QPInitAdvanced.Value AndAlso Codec.Value = 2, .ArgsFunc = AddressOf GetQpInitArgs, .ImportAction = AddressOf ImportQpInitArgs}
        Property QPInitAV1P As New NumParam With {.HelpSwitch = "--qp-init", .Text = "Initial QP P", .DefaultValue = -1, .Value = 24, .Config = {0, 255, 1}, .VisibleFunc = QPInitAV1I.VisibleFunc}
        Property QPInitAV1B As New NumParam With {.HelpSwitch = "--qp-init", .Text = "Initial QP B", .DefaultValue = -1, .Value = 28, .Config = {0, 255, 1}, .VisibleFunc = QPInitAV1I.VisibleFunc}
        Property QPMaxAdvanced As New BoolParam With {.Text = "Show advanced Maximum QP settings", .Init = False}
        Property QPMax8 As New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP", .Init = 51, .Config = {0, 51, 1}, .VisibleFunc = Function() Not QPMaxAdvanced.Value AndAlso Codec.Value <> 2 AndAlso OutputDepth.Value = 0, .ImportAction = AddressOf ImportQpMaxArgs}
        Property QPMax8I As New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP I", .DefaultValue = -1, .Value = 51, .Config = {0, 51, 1}, .VisibleFunc = Function() QPMaxAdvanced.Value AndAlso Codec.Value <> 2 AndAlso OutputDepth.Value = 0, .ArgsFunc = AddressOf GetQpMaxArgs, .ImportAction = AddressOf ImportQpMaxArgs}
        Property QPMax8P As New NumParam With {.HelpSwitch = "--qp-max", .Text = "Maximum QP P", .DefaultValue = -1, .Value = 51, .Config = {0, 51, 1}, .VisibleFunc = QPMax8I.VisibleFunc}
        Property QPMax8B As New NumParam With {.HelpSwitch = "--qp-max", .Text = "Maximum QP B", .DefaultValue = -1, .Value = 51, .Config = {0, 51, 1}, .VisibleFunc = QPMax8I.VisibleFunc}
        Property QPMax10 As New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP", .Init = 63, .Config = {0, 63, 1}, .VisibleFunc = Function() Not QPMaxAdvanced.Value AndAlso Codec.Value <> 2 AndAlso OutputDepth.Value = 1, .ImportAction = AddressOf ImportQpMaxArgs}
        Property QPMax10I As New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP I", .DefaultValue = -1, .Value = 63, .Config = {0, 63, 1}, .VisibleFunc = Function() QPMaxAdvanced.Value AndAlso Codec.Value <> 2 AndAlso OutputDepth.Value = 1, .ArgsFunc = AddressOf GetQpMaxArgs, .ImportAction = AddressOf ImportQpMaxArgs}
        Property QPMax10P As New NumParam With {.HelpSwitch = "--qp-max", .Text = "Maximum QP P", .DefaultValue = -1, .Value = 63, .Config = {0, 63, 1}, .VisibleFunc = QPMax10I.VisibleFunc}
        Property QPMax10B As New NumParam With {.HelpSwitch = "--qp-max", .Text = "Maximum QP B", .DefaultValue = -1, .Value = 63, .Config = {0, 63, 1}, .VisibleFunc = QPMax10I.VisibleFunc}
        Property QPMaxAV1 As New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP", .Init = 255, .Config = {0, 255, 1}, .VisibleFunc = Function() Not QPMaxAdvanced.Value AndAlso Codec.Value = 2, .ImportAction = AddressOf ImportQpMaxArgs}
        Property QPMaxAV1I As New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP I", .DefaultValue = -1, .Value = 255, .Config = {0, 255, 1}, .VisibleFunc = Function() QPMaxAdvanced.Value AndAlso Codec.Value = 2, .ArgsFunc = AddressOf GetQpMaxArgs, .ImportAction = AddressOf ImportQpMaxArgs}
        Property QPMaxAV1P As New NumParam With {.HelpSwitch = "--qp-max", .Text = "Maximum QP P", .DefaultValue = -1, .Value = 255, .Config = {0, 255, 1}, .VisibleFunc = QPMaxAV1I.VisibleFunc}
        Property QPMaxAV1B As New NumParam With {.HelpSwitch = "--qp-max", .Text = "Maximum QP B", .DefaultValue = -1, .Value = 255, .Config = {0, 255, 1}, .VisibleFunc = QPMaxAV1I.VisibleFunc}
        Property QPMinAdvanced As New BoolParam With {.Text = "Show advanced Minimum QP settings", .Init = False}
        Property QPMin8 As New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP", .Init = 0, .Config = {0, 51, 1}, .VisibleFunc = Function() Not QPMinAdvanced.Value AndAlso Codec.Value <> 2 AndAlso OutputDepth.Value = 0, .ImportAction = AddressOf ImportQpMinArgs}
        Property QPMin8I As New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP I", .DefaultValue = -1, .Value = 0, .Config = {0, 51, 1}, .VisibleFunc = Function() QPMinAdvanced.Value AndAlso Codec.Value <> 2 AndAlso OutputDepth.Value = 0, .ArgsFunc = AddressOf GetQpMinArgs, .ImportAction = AddressOf ImportQpMinArgs}
        Property QPMin8P As New NumParam With {.HelpSwitch = "--qp-min", .Text = "Minimum QP P", .DefaultValue = -1, .Value = 0, .Config = {0, 51, 1}, .VisibleFunc = QPMin8I.VisibleFunc}
        Property QPMin8B As New NumParam With {.HelpSwitch = "--qp-min", .Text = "Minimum QP B", .DefaultValue = -1, .Value = 0, .Config = {0, 51, 1}, .VisibleFunc = QPMin8I.VisibleFunc}
        Property QPMin10 As New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP", .Init = 0, .Config = {0, 63, 1}, .VisibleFunc = Function() Not QPMinAdvanced.Value AndAlso Codec.Value <> 2 AndAlso OutputDepth.Value = 1, .ImportAction = AddressOf ImportQpMinArgs}
        Property QPMin10I As New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP I", .DefaultValue = -1, .Value = 0, .Config = {0, 63, 1}, .VisibleFunc = Function() QPMinAdvanced.Value AndAlso Codec.Value <> 2 AndAlso OutputDepth.Value = 1, .ArgsFunc = AddressOf GetQpMinArgs, .ImportAction = AddressOf ImportQpMinArgs}
        Property QPMin10P As New NumParam With {.HelpSwitch = "--qp-min", .Text = "Minimum QP P", .DefaultValue = -1, .Value = 0, .Config = {0, 63, 1}, .VisibleFunc = QPMin10I.VisibleFunc}
        Property QPMin10B As New NumParam With {.HelpSwitch = "--qp-min", .Text = "Minimum QP B", .DefaultValue = -1, .Value = 0, .Config = {0, 63, 1}, .VisibleFunc = QPMin10I.VisibleFunc}
        Property QPMinAV1 As New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP", .Init = 0, .Config = {0, 255, 1}, .VisibleFunc = Function() Not QPMinAdvanced.Value AndAlso Codec.Value = 2, .ImportAction = AddressOf ImportQpMinArgs}
        Property QPMinAV1I As New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP I", .DefaultValue = -1, .Value = 0, .Config = {0, 255, 1}, .VisibleFunc = Function() QPMinAdvanced.Value AndAlso Codec.Value = 2, .ArgsFunc = AddressOf GetQpMinArgs, .ImportAction = AddressOf ImportQpMinArgs}
        Property QPMinAV1P As New NumParam With {.HelpSwitch = "--qp-min", .Text = "Minimum QP P", .DefaultValue = -1, .Value = 0, .Config = {0, 255, 1}, .VisibleFunc = QPMinAV1I.VisibleFunc}
        Property QPMinAV1B As New NumParam With {.HelpSwitch = "--qp-min", .Text = "Minimum QP B", .DefaultValue = -1, .Value = 0, .Config = {0, 255, 1}, .VisibleFunc = QPMinAV1I.VisibleFunc}


        Property VbvBufSize As New NumParam With {
            .Switch = "--vbv-bufsize",
            .Text = "VBV Bufsize",
            .Config = {0, 1000000, 100}}

        Property MaxBitrate As New NumParam With {
            .Switch = "--max-bitrate",
            .Text = "Max Bitrate",
            .Config = {0, 1000000, 100}}

        Property AQ As New BoolParam With {
            .Switch = "--aq",
            .Text = "Adaptive Quantization (Spatial)"}

        Property Lossless As New BoolParam With {
            .Switch = "--lossless",
            .Text = "Lossless",
            .VisibleFunc = Function() Codec.ValueText = "h264" OrElse Codec.ValueText = "h265"}

        Property BFrames As New NumParam With {
            .Switch = "--bframes",
            .HelpSwitch = "-b",
            .Text = "B-Frames",
            .Config = {0, 16},
            .Init = 3}

        Property DhdrInfo As New StringParam With {
            .Switch = "--dhdr10-info",
            .Text = "HDR10 Info File",
            .BrowseFile = True,
            .VisibleFunc = Function() Codec.ValueText = "h265" OrElse Codec.ValueText = "av1"}

        Property DolbyVisionProfileH265 As New OptionParam With {
            .Switch = "--dolby-vision-profile",
            .Text = "Dolby Vision Profile",
            .Options = {"Undefined", "5", "8.1", "8.2", "8.4"},
            .ValueChangedAction = Sub(index As Integer)
                                      Select Case index
                                          Case 1
                                              ColorMatrix.ValueChangedUser(1)
                                              ColorPrim.ValueChangedUser(1)
                                              Transfer.ValueChangedUser(0)
                                              ColorRange.ValueChangedUser(2)
                                              AtcSei.ValueChangedUser(AtcSei.InitialValue)
                                          Case 2
                                              ColorMatrix.ValueChangedUser(1)
                                              ColorPrim.ValueChangedUser(1)
                                              Transfer.ValueChangedUser(16)
                                              ColorRange.ValueChangedUser(2)
                                              AtcSei.ValueChangedUser(AtcSei.InitialValue)
                                          Case 3
                                              ColorMatrix.ValueChangedUser(4)
                                              ColorPrim.ValueChangedUser(4)
                                              Transfer.ValueChangedUser(8)
                                              ColorRange.ValueChangedUser(2)
                                              AtcSei.ValueChangedUser(AtcSei.InitialValue)
                                          Case 4
                                              ColorMatrix.ValueChangedUser(1)
                                              ColorPrim.ValueChangedUser(1)
                                              Transfer.ValueChangedUser(1)
                                              ColorRange.ValueChangedUser(2)
                                              AtcSei.ValueChangedUser(19)
                                          Case Else
                                      End Select
                                  End Sub,
            .VisibleFunc = Function() Codec.ValueText = "h265"}

        Property DolbyVisionProfileAV1 As New OptionParam With {
            .Switch = "--dolby-vision-profile",
            .Text = "Dolby Vision Profile",
            .Options = {"Undefined", "10.0", "10.1", "10.2", "10.4"},
            .ValueChangedAction = Sub(index As Integer)
                                      Select Case index
                                          Case 1
                                              ColorMatrix.ValueChangedUser(1)
                                              ColorPrim.ValueChangedUser(1)
                                              Transfer.ValueChangedUser(0)
                                              ColorRange.ValueChangedUser(2)
                                              AtcSei.ValueChangedUser(AtcSei.InitialValue)
                                          Case 2
                                              ColorMatrix.ValueChangedUser(1)
                                              ColorPrim.ValueChangedUser(1)
                                              Transfer.ValueChangedUser(16)
                                              ColorRange.ValueChangedUser(2)
                                              AtcSei.ValueChangedUser(AtcSei.InitialValue)
                                          Case 3
                                              ColorMatrix.ValueChangedUser(4)
                                              ColorPrim.ValueChangedUser(4)
                                              Transfer.ValueChangedUser(8)
                                              ColorRange.ValueChangedUser(2)
                                              AtcSei.ValueChangedUser(AtcSei.InitialValue)
                                          Case 4
                                              ColorMatrix.ValueChangedUser(1)
                                              ColorPrim.ValueChangedUser(1)
                                              Transfer.ValueChangedUser(1)
                                              ColorRange.ValueChangedUser(2)
                                              AtcSei.ValueChangedUser(19)
                                          Case Else
                                      End Select
                                  End Sub,
            .VisibleFunc = Function() Codec.ValueText = "av1"}

        Property DolbyVisionRpu As New StringParam With {
            .Switch = "--dolby-vision-rpu",
            .Text = "Dolby Vision RPU",
            .BrowseFile = True,
            .VisibleFunc = Function() Codec.ValueText = "h265" OrElse Codec.ValueText = "av1"}

        Property ColorMatrix As New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix", .Options = {"Undefined", "BT 2020 C", "BT 2020 NC", "BT 470 BG", "BT 709", "FCC", "GBR", "SMPTE 170 M", "SMPTE 240 M", "YCgCo"}}
        Property ColorPrim As New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim", .Options = {"Undefined", "BT 2020", "BT 470 BG", "BT 470 M", "BT 709", "Film", "SMPTE 170 M", "SMPTE 240 M"}}
        Property Transfer As New OptionParam With {.Switch = "--transfer", .Text = "Transfer", .Options = {"Undefined", "ARIB-STD-B67", "Auto", "BT 1361 E", "BT 2020-10", "BT 2020-12", "BT 470 BG", "BT 470 M", "BT 709", "IEC 61966-2-1", "IEC 61966-2-4", "Linear", "Log 100", "Log 316", "SMPTE 170 M", "SMPTE 240 M", "SMPTE 2084", "SMPTE 428"}}
        Property ColorRange As New OptionParam With {.Switch = "--colorrange", .Text = "Colorrange", .Options = {"Undefined", "Auto", "Limited", "TV", "Full", "PC"}}
        Property AtcSei As New OptionParam With {.Switch = "--atc-sei", .Text = "ATC SEI", .Init = 1, .Options = {"Undef", "Unknown", "Auto", "Auto_Res", "BT 709", "SMPTE 170 M", "BT 470 M", "BT 470 BG", "SMPTE 240 M", "Linear", "Log 100", "Log 316", "IEC 61966-2-4", "BT 1361 E", "IEC 61966-2-1", "BT 2020-10", "BT 2020-12", "SMPTE 2084", "SMPTE 428", "ARIB-STD-B67"}, .VisibleFunc = Function() Codec.ValueText = "h265"}

        Property MaxCLL As New NumParam With {
            .Switch = "--max-cll",
            .Text = "Maximum CLL",
            .VisibleFunc = Function() Codec.ValueText = "h265" OrElse Codec.ValueText = "av1",
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
            .VisibleFunc = Function() Codec.ValueText = "h265" OrElse Codec.ValueText = "av1"}

        Property Interlace As New OptionParam With {
            .Text = "Interlace",
            .Switch = "--interlace",
            .Options = {"Disabled", "Top Field First", "Bottom Field First", "Auto"},
            .Values = {"", "tff", "bff", "auto"}}

        Property Custom As New StringParam With {
            .Text = "Custom",
            .Quotes = QuotesMode.Never,
            .AlwaysOn = True}


        Property Vmaf As New BoolParam With {.Switch = "--vmaf", .Text = "VMAF", .ArgsFunc = Function() If(Vmaf.Value AndAlso VmafString.Value = "", "--vmaf", "")}
        Property VmafString As New StringParam With {.Switch = "--vmaf", .Text = "VMAF-Parameters", .VisibleFunc = Function() Vmaf.Value}


        Property Tweak As New BoolParam With {.Switch = "--vpp-tweak", .Text = "Tweaking", .ArgsFunc = AddressOf GetTweakArgs}
        Property TweakContrast As New NumParam With {.Text = "      Contrast", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {-2.0, 2.0, 0.01, 2}}
        Property TweakGamma As New NumParam With {.Text = "      Gamma", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {0.1, 10.0, 0.1, 2}}
        Property TweakSaturation As New NumParam With {.Text = "      Saturation", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {0.0, 3.0, 0.01, 2}}
        Property TweakHue As New NumParam With {.Text = "      Hue", .HelpSwitch = "--vpp-tweak", .Config = {-180.0, 180.0, 0.1, 2}}
        Property TweakBrightness As New NumParam With {.Text = "      Brightness", .HelpSwitch = "--vpp-tweak", .Config = {-1.0, 1.0, 0.01, 2}}

        Property Nlmeans As New BoolParam With {.Text = "Nlmeans", .Switch = "--vpp-nlmeans", .ArgsFunc = AddressOf GetNlmeansArgs}
        Property NlmeansSigma As New NumParam With {.Text = "     Sigma", .HelpSwitch = "--vpp-nlmeans", .Init = 0.005, .Config = {0, 10, 0.001, 3}}
        Property NlmeansH As New NumParam With {.Text = "     H", .HelpSwitch = "--vpp-nlmeans", .Init = 0.05, .Config = {0, 10, 0.01, 2}}
        Property NlmeansPatch As New NumParam With {.Text = "     Patch", .HelpSwitch = "--vpp-nlmeans", .Init = 5, .Config = {3, 200, 1, 0}}
        Property NlmeansSearch As New NumParam With {.Text = "     Search", .HelpSwitch = "--vpp-nlmeans", .Init = 11, .Config = {3, 200, 1, 0}}
        Property NlmeansFp16 As New OptionParam With {.Text = "     Fp16", .HelpSwitch = "--vpp-nlmeans", .Init = 1, .Options = {"None: Do not use fp16 and use fp32. High precision but slow.", "Blockdiff: Use fp16 in block diff calculation. Balanced. (Default)", "All: Additionally use fp16 in weight calculation. Fast but low precision."}, .Values = {"none", "blockdiff", "all"}}

        Property Pmd As New BoolParam With {.Switch = "--vpp-pmd", .Text = "Denoise using PMD", .ArgsFunc = AddressOf GetPmdArgs}
        Property PmdApplyCount As New NumParam With {.Text = "      Apply Count", .Init = 2}
        Property PmdStrength As New NumParam With {.Text = "      Strength", .Name = "PmdStrength", .Init = 100.0, .Config = {0, 100, 1, 1}}
        Property PmdThreshold As New NumParam With {.Text = "      Threshold", .Init = 100.0, .Config = {0, 255, 1, 1}}

        Property Fft3d As New BoolParam With {.Text = "FFT3D", .Switch = "--vpp-fft3d", .ArgsFunc = AddressOf GetFft3dArgs}
        Property Fft3dSigma As New NumParam With {.Text = "      Sigma", .HelpSwitch = "--vpp-fft3d", .Init = 1, .Config = {0, 100, 0.5, 1}}
        Property Fft3dAmount As New NumParam With {.Text = "      Amount", .HelpSwitch = "--vpp-fft3d", .Init = 1, .Config = {0, 1, 0.01, 2}}
        Property Fft3dBlockSize As New OptionParam With {.Text = "      Block Size", .HelpSwitch = "--vpp-fft3d", .Expanded = True, .Init = 2, .Options = {"8", "16", "32 (default)", "64"}, .Values = {"8", "16", "32", "64"}}
        Property Fft3dOverlap As New NumParam With {.Text = "      Overlap", .HelpSwitch = "--vpp-fft3d", .Init = 0.5, .Config = {0.2, 0.8, 0.01, 2}}
        Property Fft3dMethod As New OptionParam With {.Text = "      Method", .HelpSwitch = "--vpp-fft3d", .Expanded = True, .Init = 0, .Options = {"Wiener Method (default)", "Hard Thresholding"}, .Values = {"0", "1"}}
        Property Fft3dTemporal As New OptionParam With {.Text = "      Temporal", .HelpSwitch = "--vpp-fft3d", .Init = 1, .Options = {"Spatial Filtering Only", "Temporal Filtering (default)"}, .Values = {"0", "1"}}
        Property Fft3dPrec As New OptionParam With {.Text = "      Prec", .HelpSwitch = "--vpp-fft3d", .Init = 0, .Options = {"Use fp16 if possible (faster, default)", "Always use fp32"}, .Values = {"auto", "fp32"}}

        Property Knn As New BoolParam With {.Switch = "--vpp-knn", .Text = "Denoise using K-nearest neighbor", .ArgsFunc = AddressOf GetKnnArgs}
        Property KnnRadius As New NumParam With {.Text = "      Radius", .Init = 3}
        Property KnnStrength As New NumParam With {.Text = "      Strength", .Init = 0.08, .Config = {0, 1, 0.02, 2}}
        Property KnnLerp As New NumParam With {.Text = "      Lerp", .Init = 0.2, .Config = {0, Integer.MaxValue, 0.1, 1}}
        Property KnnThLerp As New NumParam With {.Text = "      TH Lerp", .Init = 0.8, .Config = {0, 1, 0.1, 1}}

        Property Convolution As New BoolParam With {.Switch = "--vpp-convolution3d", .Text = "3D noise reduction", .ArgsFunc = AddressOf GetConvolution3dArgs}
        Property ConvolutionMatrix As New OptionParam With {.Text = "      Matrix", .Init = 0, .Options = {"Original", "Standard", "Simple"}, .Values = {"original", "standard", "simple"}}
        Property ConvolutionFast As New OptionParam With {.Text = "      Fast", .Init = 0, .Options = {"False", "True"}, .Values = {"false", "true"}}
        Property ConvolutionYthresh As New NumParam With {.Text = "      Spatial luma threshold", .Init = 3, .Config = {0, 255, 1, 1}}
        Property ConvolutionCthresh As New NumParam With {.Text = "      Spatial chroma threshold", .Init = 4, .Config = {0, 255, 1, 1}}
        Property ConvolutionTYthresh As New NumParam With {.Text = "      Temporal luma threshold", .Init = 3, .Config = {0, 255, 1, 1}}
        Property ConvolutionTCthresh As New NumParam With {.Text = "      Temporal chroma threshold", .Init = 4, .Config = {0, 255, 1, 1}}

        Property Pad As New BoolParam With {.Switch = "--vpp-pad", .Text = "Padding", .ArgsFunc = AddressOf GetPaddingArgs}
        Property PadLeft As New NumParam With {.Text = "      Left"}
        Property PadTop As New NumParam With {.Text = "      Top"}
        Property PadRight As New NumParam With {.Text = "      Right"}
        Property PadBottom As New NumParam With {.Text = "      Bottom"}

        Property Deband As New BoolParam With {.Text = "Deband", .Switch = "--vpp-deband", .ArgsFunc = AddressOf GetDebandArgs}
        Property DebandRange As New NumParam With {.Text = "     Range", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 127}}
        Property DebandSample As New NumParam With {.Text = "     Sample", .HelpSwitch = "--vpp-deband", .Init = 1, .Config = {0, 2}}
        Property DebandThre As New NumParam With {.Text = "     Threshold", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandThreY As New NumParam With {.Text = "          Y", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandThreCB As New NumParam With {.Text = "          CB", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandThreCR As New NumParam With {.Text = "          CR", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandDither As New NumParam With {.Text = "     Dither", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandDitherY As New NumParam With {.Text = "          Y", .HelpSwitch = "--vpp-deband", .Name = "vpp-deband_dither_y", .Init = 15, .Config = {0, 31}}
        Property DebandDitherC As New NumParam With {.Text = "          C", .HelpSwitch = "--vpp-deband", .Init = 15, .Config = {0, 31}}
        Property DebandSeed As New NumParam With {.Text = "     Seed", .HelpSwitch = "--vpp-deband", .Init = 1234}
        Property DebandBlurfirst As New BoolParam With {.Text = "Blurfirst", .HelpSwitch = "--vpp-deband", .LeftMargin = g.MainForm.FontHeight * 1.3}
        Property DebandRandEachFrame As New BoolParam With {.Text = "Rand Each Frame", .HelpSwitch = "--vpp-deband", .LeftMargin = g.MainForm.FontHeight * 1.3}

        Property AfsPreset As New OptionParam With {.Text = "Preset", .HelpSwitch = "--vpp-afs", .Options = {"Default", "Triple", "Double", "Anime", "Cinema", "Min_afterimg", "24fps", "24fps_sd", "30fps"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsINI As New StringParam With {.Text = "INI", .HelpSwitch = "--vpp-afs", .BrowseFile = True, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsLeft As New NumParam With {.Text = "Left", .HelpSwitch = "--vpp-afs", .Init = 32, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsRight As New NumParam With {.Text = "Right", .HelpSwitch = "--vpp-afs", .Init = 32, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsTop As New NumParam With {.Text = "Top", .HelpSwitch = "--vpp-afs", .Init = 16, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsBottom As New NumParam With {.Text = "Bottom", .HelpSwitch = "--vpp-afs", .Init = 16, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsMethodSwitch As New NumParam With {.Text = "Method Switch", .HelpSwitch = "--vpp-afs", .Config = {0, 256}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsCoeffShift As New NumParam With {.Text = "Coeff Shift", .HelpSwitch = "--vpp-afs", .Init = 192, .Config = {0, 256}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsThreShift As New NumParam With {.Text = "Threshold Shift", .HelpSwitch = "--vpp-afs", .Init = 128, .Config = {0, 1024}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsThreDeint As New NumParam With {.Text = "Threshold Deint", .HelpSwitch = "--vpp-afs", .Init = 48, .Config = {0, 1024}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsThreMotionY As New NumParam With {.Text = "Threshold Motion Y", .HelpSwitch = "--vpp-afs", .Init = 112, .Config = {0, 1024}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsThreMotionC As New NumParam With {.Text = "Threshold Motion C", .HelpSwitch = "--vpp-afs", .Init = 224, .Config = {0, 1024}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsLevel As New NumParam With {.Text = "Level", .HelpSwitch = "--vpp-afs", .Init = 3, .Config = {0, 4}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsShift As New BoolParam With {.Text = "Shift", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsDrop As New BoolParam With {.Text = "Drop", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsSmooth As New BoolParam With {.Text = "Smooth", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property Afs24fps As New BoolParam With {.Text = "24 FPS", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsTune As New BoolParam With {.Text = "Tune", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsRFF As New BoolParam With {.Text = "RFF", .HelpSwitch = "--vpp-afs", .Init = True, .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsTimecode As New BoolParam With {.Text = "Timecode", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property AfsLog As New BoolParam With {.Text = "Log", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 2}

        Property Edgelevel As New BoolParam With {.Text = "Edgelevel filter to enhance edge", .Switches = {"--vpp-edgelevel"}, .ArgsFunc = AddressOf GetEdge}
        Property EdgelevelStrength As New NumParam With {.Text = "     Strength", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}
        Property EdgelevelThreshold As New NumParam With {.Text = "     Threshold", .HelpSwitch = "--vpp-edgelevel", .Init = 20, .Config = {0, 255, 1, 1}}
        Property EdgelevelBlack As New NumParam With {.Text = "     Black", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}
        Property EdgelevelWhite As New NumParam With {.Text = "     White", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}

        Property Unsharp As New BoolParam With {.Text = "Unsharp Filter", .Switches = {"--vpp-unsharp"}, .ArgsFunc = AddressOf GetUnsharp}
        Property UnsharpRadius As New NumParam With {.Text = "     Radius", .HelpSwitch = "--vpp-unsharp", .Init = 3, .Config = {1, 9}}
        Property UnsharpWeight As New NumParam With {.Text = "     Weight", .HelpSwitch = "--vpp-unsharp", .Init = 0.5, .Config = {0, 10, 0.5, 1}}
        Property UnsharpThreshold As New NumParam With {.Text = "     Threshold", .HelpSwitch = "--vpp-unsharp", .Init = 10, .Config = {0, 255, 1, 1}}

        Property Warpsharp As New BoolParam With {.Text = "Warpsharp filter", .Switches = {"--vpp-warpsharp"}, .ArgsFunc = AddressOf GetWarpsharpArgs}
        Property WarpsharpThreshold As New NumParam With {.Text = "     Threshold", .HelpSwitch = "--vpp-warpsharp", .Init = 128, .Config = {0, 255, 1, 1}}
        Property WarpsharpBlur As New NumParam With {.Text = "     Blur", .HelpSwitch = "--vpp-warpsharp", .Init = 2, .Config = {0, 30, 1, 0}}
        Property WarpsharpType As New NumParam With {.Text = "     Type", .HelpSwitch = "--vpp-warpsharp", .Init = 0, .Config = {0, 1, 1, 0}}
        Property WarpsharpDepth As New NumParam With {.Text = "     Depth", .HelpSwitch = "--vpp-warpsharp", .Init = 16, .Config = {-128, 128, 1, 1}}
        Property WarpsharpChroma As New NumParam With {.Text = "     Chroma", .HelpSwitch = "--vpp-warpsharp", .Init = 0, .Config = {0, 1, 1, 0}}

        Property DecombFull As New OptionParam With {.Text = "Full", .HelpSwitch = "--vpp-decomb", .Init = 1, .Options = {"Off", "On (Default)"}, .Values = {"off", "on"}, .VisibleFunc = Function() Deinterlacer.Value = 3}
        Property DecombThreshold As New NumParam With {.Text = "Threshold", .HelpSwitch = "--vpp-decomb", .Init = 20, .Config = {0, 255, 1, 0}, .VisibleFunc = Function() Deinterlacer.Value = 3}
        Property DecombDThreshold As New NumParam With {.Text = "DThreshold", .HelpSwitch = "--vpp-decomb", .Init = 7, .Config = {0, 255, 1, 0}, .VisibleFunc = Function() Deinterlacer.Value = 3}
        Property DecombBlend As New OptionParam With {.Text = "Blend", .HelpSwitch = "--vpp-decomb", .Init = 0, .Options = {"Off (Default)", "On"}, .Values = {"off", "on"}, .VisibleFunc = Function() Deinterlacer.Value = 3}

        Property NnediField As New OptionParam With {.Text = "Field", .HelpSwitch = "--vpp-nnedi", .Options = {"auto", "top", "bottom", "bob", "bob_tff", "bob_bff"}, .VisibleFunc = Function() Deinterlacer.Value = 4}
        Property NnediNns As New OptionParam With {.Text = "NNS", .HelpSwitch = "--vpp-nnedi", .Init = 1, .Options = {"16", "32", "64", "128", "256"}, .VisibleFunc = Function() Deinterlacer.Value = 4}
        Property NnediNsize As New OptionParam With {.Text = "N Size", .HelpSwitch = "--vpp-nnedi", .Init = 6, .Options = {"8x6", "16x6", "32x6", "48x6", "8x4", "16x4", "32x4"}, .VisibleFunc = Function() Deinterlacer.Value = 4}
        Property NnediQuality As New OptionParam With {.Text = "Quality", .HelpSwitch = "--vpp-nnedi", .Options = {"fast", "slow"}, .VisibleFunc = Function() Deinterlacer.Value = 4}
        Property NnediPrescreen As New OptionParam With {.Text = "Pre Screen", .HelpSwitch = "--vpp-nnedi", .Init = 4, .Options = {"none", "original", "new", "original_block", "new_block"}, .VisibleFunc = Function() Deinterlacer.Value = 4}
        Property NnediErrortype As New OptionParam With {.Text = "Error Type", .HelpSwitch = "--vpp-nnedi", .Options = {"abs", "square"}, .VisibleFunc = Function() Deinterlacer.Value = 4}
        Property NnediPrec As New OptionParam With {.Text = "Prec", .HelpSwitch = "--vpp-nnedi", .Options = {"auto", "fp16", "fp32"}, .VisibleFunc = Function() Deinterlacer.Value = 4}
        Property NnediWeightfile As New StringParam With {.Text = "Weight File", .HelpSwitch = "--vpp-nnedi", .BrowseFile = True, .VisibleFunc = Function() Deinterlacer.Value = 4}

        Property SelectEvery As New BoolParam With {.Text = "Select Every", .Switches = {"--vpp-select-every"}, .ArgsFunc = AddressOf GetSelectEvery}
        Property SelectEveryValue As New NumParam With {.Text = "      Value", .HelpSwitch = "--vpp-select-every", .Init = 2}
        Property SelectEveryOffsets As New StringParam With {.Text = "      Offsets", .HelpSwitch = "--vpp-select-every", .Expand = False}

        Property Resize As New BoolParam With {.Text = "Resize", .Switch = "--vpp-resize", .ArgsFunc = AddressOf GetResizeArgs}
        Property ResizeAlgo As New OptionParam With {.Text = "      Algo", .HelpSwitch = "--vpp-resize", .Init = 0, .IntegerValue = False, .Options = {"Auto", "bilinear - linear interpolation", "bicubic - bicubic interpolation", "spline16 - 4x4 spline curve interpolation", "spline36 - 6x6 spline curve interpolation", "spline64 - 8x8 spline curve interpolation", "lanczos2 - 4x4 Lanczos resampling", "lanczos3 - 6x6 Lanczos resampling", "lanczos4 - 8x8 Lanczos resampling", "nn - nearest neighbor", "npp_linear - linear interpolation by NPP library", "cubic - 4x4 cubic interpolation", "super - So called 'super sampling' by NPP library (downscale only)", "lanczos - Lanczos interpolation", "nvvfx-superres - Super Resolution based on nvvfx library (upscale only)", "ngx-vsr	- NVIDIA VSR (Video Super Resolution)", "libplacebo - Libplacebo"}, .Values = {"auto", "bilinear", "bicubic", "spline16", "spline36", "spline64", "lanczos2", "lanczos3", "lanczos4", "nn", "npp_linear", "cubic", "super", "lanczos", "nvvfx-superres", "ngx-vsr", "libplacebo"}}
        Property ResizeSuperresMode As New OptionParam With {.Text = "      Superres-Mode", .HelpSwitch = "--vpp-resize", .Init = 1, .IntegerValue = True, .Options = {"0 - Upscaling only", "1 - Upscaling with enhancements (default)"}, .VisibleFunc = Function() ResizeAlgo.Value = 14}
        Property ResizeSuperresStrength As New NumParam With {.Text = "      Superres-Strength", .HelpSwitch = "--vpp-resize", .Init = 0.4, .Config = {0, 1, 0.05, 2}, .VisibleFunc = Function() ResizeAlgo.Value = 14}
        Property ResizeVsrQuality As New NumParam With {.Text = "      VSR-Quality", .HelpSwitch = "--vpp-resize", .Init = 1, .Config = {1, 4, 1, 0}, .VisibleFunc = Function() ResizeAlgo.Value = 15}
        Property ResizeLibplaceboFilter As New OptionParam With {.Text = "      Libplacebo-Filter", .HelpSwitch = "--vpp-resize", .Init = 0, .IntegerValue = True, .Options = {"spline16 - 4x4 spline curve interpolation", "spline36 - 6x6 spline curve interpolation", "spline64 - 8x8 spline curve interpolation", "nearest - nearest neighbor", "bilinear - linear interpolation", "gaussian - Gaussian filter", "sinc - Sinc filter", "lanczos - Lanczos resampling", "ginseng - Ginseng filter", "ewa-jinc - EWA Jinc resampling", "ewa-lanczos - EWA Lanczos resampling", "ewa-lanczossharp - EWA Lanczos sharp resampling", "ewa-lanczos4sharpest - EWA Lanczos 4 sharpest resampling", "ewa-ginseng - EWA Ginseng resampling", "ewa-hann - EWA Hann filter", "ewa-hanning - EWA Hanning filter", "bicubic - Bicubic interpolation", "triangle - Triangle filter", "hermite - Hermite filter", "catmull-rom - Catmull-Rom spline interpolation", "mitchell - Mitchell-Netravali filter", "mitchell-clamp - Mitchell-Netravali filter with clamping", "robidoux - Robidoux filter", "robidouxsharp - Robidoux sharp filter", "ewa-robidoux - EWA Robidoux filter", "ewa-robidouxsharp - EWA Robidoux sharp filter"}, .Values = {"spline16", "spline36", "spline64", "nearest", "bilinear", "gaussian", "sinc", "lanczos", "ginseng", "ewa-jinc", "ewa-lanczos", "ewa-lanczossharp", "ewa-lanczos4sharpest", "ewa-ginseng", "ewa-hann", "ewa-hanning", "bicubic", "triangle", "hermite", "catmull-rom", "mitchell", "mitchell-clamp", "robidoux", "robidouxsharp", "ewa-robidoux", "ewa-robidouxsharp"}, .VisibleFunc = Function() ResizeAlgo.Value = 16}
        Property ResizeLibplaceboPlRadius As New NumParam With {.Text = "      Libplacebo Pl-Radius", .HelpSwitch = "--vpp-resize", .Init = -0.1, .Config = {-0.1, 16, 0.1, 1}, .VisibleFunc = ResizeLibplaceboFilter.VisibleFunc}
        Property ResizeLibplaceboPlClamp As New NumParam With {.Text = "      Libplacebo Pl-Clamp", .HelpSwitch = "--vpp-resize", .Init = 0, .Config = {0, 1, 0.05, 2}, .VisibleFunc = ResizeLibplaceboFilter.VisibleFunc}
        Property ResizeLibplaceboPlTaper As New NumParam With {.Text = "      Libplacebo Pl-Taper", .HelpSwitch = "--vpp-resize", .Init = 0, .Config = {0, 1, 0.05, 2}, .VisibleFunc = ResizeLibplaceboFilter.VisibleFunc}
        Property ResizeLibplaceboPlBlur As New NumParam With {.Text = "      Libplacebo Pl-Blur", .HelpSwitch = "--vpp-resize", .Init = 0, .Config = {0, 100, 1, 1}, .VisibleFunc = ResizeLibplaceboFilter.VisibleFunc}
        Property ResizeLibplaceboPlAntiring As New NumParam With {.Text = "      Libplacebo Pl-Antiring", .HelpSwitch = "--vpp-resize", .Init = 0, .Config = {0, 1, 0.05, 2}, .VisibleFunc = ResizeLibplaceboFilter.VisibleFunc}

        Property NvvfxDenoise As New BoolParam With {.Text = "Webcam denoise filter", .Switch = "--vpp-nvvfx-denoise", .ArgsFunc = AddressOf GetNvvfxDenoiseArgs}
        Property NvvfxDenoiseStrength As New OptionParam With {.Text = "      Strength", .HelpSwitch = "--vpp-nvvfx-denoise", .Options = {"0 - Weaker effect, higher emphasis on texture preservation", "1 - Stronger effect, higher emphasis on noise removal"}}

        Property NvvfxArtifactReduction As New BoolParam With {.Text = "Artifact reduction filter", .Switch = "--vpp-nvvfx-artifact-reduction", .ArgsFunc = AddressOf GetNvvfxArtifactReductionArgs}
        Property NvvfxArtifactReductionMode As New OptionParam With {.Text = "      Mode", .HelpSwitch = "--vpp-nvvfx-artifact-reduction", .Options = {"0 - Removes lesser artifacts, preserves low gradient information better, suited for higher bitrate videos (default)", "1 - Results stronger effect, suitable for lower bitrate videos"}}

        Property TransformFlipX As New BoolParam With {.Switch = "--vpp-transform", .Text = "Flip X", .Label = "Transform", .LeftMargin = g.MainForm.FontHeight * 1.5, .ArgsFunc = AddressOf GetTransform}
        Property TransformFlipY As New BoolParam With {.Text = "Flip Y", .LeftMargin = g.MainForm.FontHeight * 1.5, .HelpSwitch = "--vpp-transform"}
        Property TransformTranspose As New BoolParam With {.Text = "Transpose", .LeftMargin = g.MainForm.FontHeight * 1.5, .HelpSwitch = "--vpp-transform"}

        Property Smooth As New BoolParam With {.Text = "Smooth", .Switch = "--vpp-smooth", .ArgsFunc = AddressOf GetSmoothArgs}
        Property SmoothQuality As New NumParam With {.Text = "      Quality", .HelpSwitch = "--vpp-smooth", .Init = 3, .Config = {1, 6}}
        Property SmoothQP As New NumParam With {.Text = "      QP", .HelpSwitch = "--vpp-smooth", .Config = {0, 63, 1, 1}}
        Property SmoothPrec As New OptionParam With {.Text = "      Precision", .HelpSwitch = "--vpp-smooth", .Options = {"Auto", "FP16", "FP32"}}

        Property DenoiseDct As New BoolParam With {.Text = "Denoise DCT", .Switch = "--vpp-denoise-dct", .ArgsFunc = AddressOf GetDenoiseDctArgs}
        Property DenoiseDctStep As New OptionParam With {.Text = "      Step", .HelpSwitch = "--vpp-denoise-dct", .Init = 1, .Options = {"1 (high quality, slow)", "2 (default)", "4", "8 (fast)"}, .Values = {"1", "2", "4", "8"}}
        Property DenoiseDctSigma As New NumParam With {.Text = "      Sigma", .HelpSwitch = "--vpp-denoise-dct", .Init = 4, .Config = {0, 100, 0.1, 1}}
        Property DenoiseDctBlockSize As New OptionParam With {.Text = "      Block Size", .HelpSwitch = "--vpp-denoise-dct", .Options = {"8 (default)", "16 (slow)"}, .Values = {"8", "16"}}

        Property Colorspace As New BoolParam With {.Text = "Colorspace", .Switch = "--vpp-colorspace", .ArgsFunc = AddressOf GetColorspaceArgs}
        Property ColorspaceMatrixFrom As New OptionParam With {.Text = New String(" "c, 6) + "Matrix From", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"Undefined", "auto", "bt709", "smpte170m", "bt470bg", "smpte240m", "YCgCo", "fcc", "GBR", "bt2020nc", "bt2020c"}}
        Property ColorspaceMatrixTo As New OptionParam With {.Text = New String(" "c, 12) + "Matrix To", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"auto", "bt709", "smpte170m", "bt470bg", "smpte240m", "YCgCo", "fcc", "GBR", "bt2020nc", "bt2020c"}, .VisibleFunc = Function() ColorspaceMatrixFrom.Value > 0}
        Property ColorspaceColorprimFrom As New OptionParam With {.Text = New String(" "c, 6) + "Colorprim From", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"Undefined", "auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "film", "bt2020"}}
        Property ColorspaceColorprimTo As New OptionParam With {.Text = New String(" "c, 12) + "Colorprim To", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "film", "bt2020"}, .VisibleFunc = Function() ColorspaceColorprimFrom.Value > 0}
        Property ColorspaceTransferFrom As New OptionParam With {.Text = New String(" "c, 6) + "Transfer From", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"Undefined", "auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "linear", "log100", "log316", "iec61966-2-4", "iec61966-2-1", "bt2020-10", "bt2020-12", "smpte2084", "arib-std-b67"}}
        Property ColorspaceTransferTo As New OptionParam With {.Text = New String(" "c, 12) + "Transfer To", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "linear", "log100", "log316", "iec61966-2-4", "iec61966-2-1", "bt2020-10", "bt2020-12", "smpte2084", "arib-std-b67"}, .VisibleFunc = Function() ColorspaceTransferFrom.Value > 0}
        Property ColorspaceRangeFrom As New OptionParam With {.Text = New String(" "c, 6) + "Range From", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"Undefined", "auto", "limited", "full"}}
        Property ColorspaceRangeTo As New OptionParam With {.Text = New String(" "c, 12) + "Range To", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"auto", "limited", "full"}, .VisibleFunc = Function() ColorspaceRangeFrom.Value > 0}
        Property ColorspaceLut3d As New StringParam With {.Text = New String(" "c, 6) + "Lut3D", .HelpSwitch = "--vpp-colorspace", .Init = "", .BrowseFile = True}
        Property ColorspaceLut3dinterp As New OptionParam With {.Text = New String(" "c, 12) + "Interpolation", .HelpSwitch = "--vpp-colorspace", .Init = 1, .Options = {"nearest", "trilinear", "tetrahedral", "pyramid", "prism"}, .VisibleFunc = Function() ColorspaceLut3d.Value.Trim().Length > 0}

        Property ColorspaceHdr2sdr As New OptionParam With {.Text = New String(" "c, 0) + "HDR10 to SDR using this tonemapping:", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"none", "hable", "mobius", "reinhard", "bt2390"}}
        Property ColorspaceHdr2sdrSourcepeak As New NumParam With {.Text = New String(" "c, 6) + "Source Peak", .HelpSwitch = "--vpp-colorspace", .Init = 1000, .Config = {0, 10000, 1, 1}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value > 0}
        Property ColorspaceHdr2sdrLdrnits As New NumParam With {.Text = New String(" "c, 6) + "Target brightness", .HelpSwitch = "--vpp-colorspace", .Init = 100.0, .Config = {0, 1000, 1, 1}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value > 0}
        Property ColorspaceHdr2sdrDesatbase As New NumParam With {.Text = New String(" "c, 6) + "Offset for desaturation curve", .HelpSwitch = "--vpp-colorspace", .Init = 0.18, .Config = {0, 10, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value > 0}
        Property ColorspaceHdr2sdrDesatstrength As New NumParam With {.Text = New String(" "c, 6) + "Strength of desaturation curve", .HelpSwitch = "--vpp-colorspace", .Init = 0.75, .Config = {0, 10, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value > 0}
        Property ColorspaceHdr2sdrDesatexp As New NumParam With {.Text = New String(" "c, 6) + "Exponent of the desaturation curve", .HelpSwitch = "--vpp-colorspace", .Init = 1.5, .Config = {0, 100, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value > 0}
        Property ColorspaceHdr2sdrHableA As New NumParam With {.Text = New String(" "c, 6) + "a", .HelpSwitch = "--vpp-colorspace", .Init = 0.22, .Config = {0, 1, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 1}
        Property ColorspaceHdr2sdrHableB As New NumParam With {.Text = New String(" "c, 6) + "b", .HelpSwitch = "--vpp-colorspace", .Init = 0.3, .Config = {0, 1, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 1}
        Property ColorspaceHdr2sdrHableC As New NumParam With {.Text = New String(" "c, 6) + "c", .HelpSwitch = "--vpp-colorspace", .Init = 0.1, .Config = {0, 1, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 1}
        Property ColorspaceHdr2sdrHableD As New NumParam With {.Text = New String(" "c, 6) + "d", .HelpSwitch = "--vpp-colorspace", .Init = 0.2, .Config = {0, 1, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 1}
        Property ColorspaceHdr2sdrHableE As New NumParam With {.Text = New String(" "c, 6) + "e", .HelpSwitch = "--vpp-colorspace", .Init = 0.01, .Config = {0, 1, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 1}
        Property ColorspaceHdr2sdrHableF As New NumParam With {.Text = New String(" "c, 6) + "f", .HelpSwitch = "--vpp-colorspace", .Init = 0.3, .Config = {0, 1, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 1}
        Property ColorspaceHdr2sdrMobiusTransition As New NumParam With {.Text = New String(" "c, 6) + "Transition", .HelpSwitch = "--vpp-colorspace", .Init = 0.3, .Config = {0, 10, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 2}
        Property ColorspaceHdr2sdrMobiusPeak As New NumParam With {.Text = New String(" "c, 6) + "Peak", .HelpSwitch = "--vpp-colorspace", .Init = 1.0, .Config = {0, 100, 0.05, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 2, .Name = "MobiusPeak"}
        Property ColorspaceHdr2sdrReinhardContrast As New NumParam With {.Text = New String(" "c, 6) + "Contrast", .HelpSwitch = "--vpp-colorspace", .Init = 0.5, .Config = {0, 1, 0.01, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 3}
        Property ColorspaceHdr2sdrReinhardPeak As New NumParam With {.Text = New String(" "c, 6) + "Peak", .HelpSwitch = "--vpp-colorspace", .Init = 1.0, .Config = {0, 100, 0.05, 2}, .VisibleFunc = Function() ColorspaceHdr2sdr.Value = 3, .Name = "ReinhardPeak"}

        Property NgxTruehdr As New BoolParam With {.Text = New String(" "c, 0) + "AI enhanced SDR to HDR conversion using RTX Video SDK:", .Switch = "--vpp-ngx-truehdr", .ArgsFunc = AddressOf GetNgxTruehdrArgs, .ImportAction = AddressOf ImportNgxTruehdrArgs}
        Property NgxTruehdrContrast As New NumParam With {.Text = New String(" "c, 6) + "Contrast", .HelpSwitch = "--vpp-ngx-truehdr", .Init = 125.0, .Config = {0, 200, 1, 0}}
        Property NgxTruehdrSaturation As New NumParam With {.Text = New String(" "c, 6) + "Saturation", .HelpSwitch = "--vpp-ngx-truehdr", .Init = 75.0, .Config = {0, 200, 1, 0}}
        Property NgxTruehdrMiddleGray As New NumParam With {.Text = New String(" "c, 6) + "MiddleGray", .HelpSwitch = "--vpp-ngx-truehdr", .Init = 44.0, .Config = {10, 100, 1, 0}}
        Property NgxTruehdrMaxLuminance As New NumParam With {.Text = New String(" "c, 6) + "Max Luminance", .HelpSwitch = "--vpp-ngx-truehdr", .Init = 1000.0, .Config = {400, 2000, 1, 0}}

        Property LibPlaceboDeband As New BoolParam With {.Text = "LibPlacebo-Deband", .Switch = "--vpp-libplacebo-deband", .ArgsFunc = AddressOf GetLibPlaceboDebandArgs, .ImportAction = AddressOf ImportLibPlaceboDebandArgs}
        Property LibPlaceboDebandIterations As New NumParam With {.Text = "     Iterations", .HelpSwitch = "--vpp-libplacebo-deband", .Init = 1, .Config = {0, 255}}
        Property LibPlaceboDebandThreshold As New NumParam With {.Text = "     Threshold", .HelpSwitch = "--vpp-libplacebo-deband", .Init = 4, .Config = {0, 255, 0.5, 1}}
        Property LibPlaceboDebandRadius As New NumParam With {.Text = "     Radius", .HelpSwitch = "--vpp-libplacebo-deband", .Init = 16, .Config = {0, 255, 0.5, 1}}
        Property LibPlaceboDebandGrainY As New NumParam With {.Text = "     Grain Y", .HelpSwitch = "--vpp-libplacebo-deband", .Init = 6, .Config = {0, 255, 0.5, 1}, .ValueChangedAction = Sub(value)
                                                                                                                                                                                                LibPlaceboDebandGrainC.DefaultValue = value
                                                                                                                                                                                                LibPlaceboDebandGrainC.ValueChanged()
                                                                                                                                                                                            End Sub}
        Property LibPlaceboDebandGrainC As New NumParam With {.Text = "     Grain C", .HelpSwitch = "--vpp-libplacebo-deband", .Init = 6, .Config = {0, 255, 0.5, 1}}
        Property LibPlaceboDebandDither As New OptionParam With {.Text = "     Dither", .HelpSwitch = "--vpp-libplacebo-deband", .Init = 1, .Options = {"None", "Blue Noise (default)", "Ordered LUT", "Ordered Fixed", "White Noise"}, .Values = {"none", "blue_noise", "ordered_lut", "ordered_fixed", "white_noise"}, .VisibleFunc = Function() OutputDepth.Value = 0}
        Property LibPlaceboDebandLutSize As New OptionParam With {.Text = "     LUT Size", .HelpSwitch = "--vpp-libplacebo-deband", .Init = 5, .Options = {"2", "4", "8", "16", "32", "64 (default)", "128", "256"}, .Values = {"2", "4", "8", "16", "32", "64", "128", "256"}}

        Property LibPlaceboShader As New BoolParam With {.Text = "LibPlacebo-Shader", .Switch = "--vpp-libplacebo-shader", .ArgsFunc = AddressOf GetLibPlaceboShaderArgs, .ImportAction = AddressOf ImportLibPlaceboShaderArgs}
        Property LibPlaceboShaderShader As New StringParam With {.Text = "     Shader", .HelpSwitch = "--vpp-libplacebo-shader", .Init = "", .BrowseFile = True}
        Property LibPlaceboShaderResWidth As New NumParam With {.Text = "     Resolution Width", .HelpSwitch = "--vpp-libplacebo-shader", .Init = 0, .Config = {0, 7680, 1, 0}}
        Property LibPlaceboShaderResHeight As New NumParam With {.Text = "     Resolution Height", .HelpSwitch = "--vpp-libplacebo-shader", .Init = 0, .Config = {0, 4320, 1, 0}}
        Property LibPlaceboShaderColorsystem As New OptionParam With {.Text = "     Colorsystem", .HelpSwitch = "--vpp-libplacebo-shader", .Init = 0, .Options = {"Unknown (default)", "BT601", "BT709", "SMPTE240M", "BT2020NC", "BT2020C", "BT2100PQ", "BT2100HLG", "DolbyVision", "Ycgco", "RGB", "XYZ"}, .Values = {"unknown", "bt601", "bt709", "smpte240m", "bt2020nc", "bt2020c", "bt2100pq", "bt2100hlg", "dolbyvision", "ycgco", "rgb", "xyz"}}
        Property LibPlaceboShaderTransfer As New OptionParam With {.Text = "     Transfer", .HelpSwitch = "--vpp-libplacebo-shader", .Init = 0, .Options = {"Unknown (default)", "SRGB", "BT1886", "Linear", "Gamma18", "Gamma20", "Gamma22", "Gamma24", "Gamma26", "Gamma28", "ProPhoto", "ST428", "PQ", "HLG", "Vlog", "Slog1", "Slog2"}, .Values = {"unknown", "srgb", "bt1886", "linear", "gamma18", "gamma20", "gamma22", "gamma24", "gamma26", "gamma28", "prophoto", "st428", "pq", "hlg", "vlog", "slog1", "slog2"}}
        Property LibPlaceboShaderResampler As New OptionParam With {.Text = "     Resampler", .HelpSwitch = "--vpp-libplacebo-shader", .Init = 10, .Options = {"Spline16", "Spline36", "Spline64", "Nearest", "Bilinear", "Gaussian", "Sinc", "Lanczos", "Ginseng", "Ewa-Jinc", "Ewa-Lanczos (default)", "Ewa-Lanczossharp", "Ewa-Lanczos4sharpest", "Ewa-Ginseng", "Ewa-Hann", "Ewa-Hanning", "Bicubic", "Triangle", "Hermite", "Catmull-Rom", "Mitchell", "Mitchell-Clamp", "Robidoux", "Robidouxsharp", "Ewa-Robidoux", "Ewa-Robidouxsharp"}, .Values = {"libplacebo-spline16", "libplacebo-spline36", "libplacebo-spline64", "libplacebo-nearest", "libplacebo-bilinear", "libplacebo-gaussian", "libplacebo-sinc", "libplacebo-lanczos", "libplacebo-ginseng", "libplacebo-ewa-jinc", "libplacebo-ewa-lanczos", "libplacebo-ewa-lanczossharp", "libplacebo-ewa-lanczos4sharpest", "libplacebo-ewa-ginseng", "libplacebo-ewa-hann", "libplacebo-ewa-hanning", "libplacebo-bicubic", "libplacebo-triangle", "libplacebo-hermite", "libplacebo-catmull-rom", "libplacebo-mitchell", "libplacebo-mitchell-clamp", "libplacebo-robidoux", "libplacebo-robidouxsharp", "libplacebo-ewa-robidoux", "libplacebo-ewa-robidouxsharp"}}
        Property LibPlaceboShaderRadius As New OptionParam With {.Text = "     Radius Mode", .HelpSwitch = "--vpp-libplacebo-shader", .Init = 0, .Options = {"Auto (default)", "Manual"}, .Values = {"auto", "manual"}}
        Property LibPlaceboShaderRadiusValue As New NumParam With {.Text = "     Radius", .HelpSwitch = "--vpp-libplacebo-shader", .Init = 0, .Config = {0, 16, 0.1, 2}, .VisibleFunc = Function() Not LibPlaceboShaderRadius.IsDefaultValue}
        Property LibPlaceboShaderClamp As New NumParam With {.Text = "     Clamp", .HelpSwitch = "--vpp-libplacebo-shader", .Init = 0, .Config = {0, 1, 0.01, 2}}
        Property LibPlaceboShaderTaper As New NumParam With {.Text = "     Taper", .HelpSwitch = "--vpp-libplacebo-shader", .Init = 0, .Config = {0, 1, 0.01, 2}}
        Property LibPlaceboShaderBlur As New NumParam With {.Text = "     Blur", .HelpSwitch = "--vpp-libplacebo-shader", .Init = 0, .Config = {0, 100, 0.5, 1}}
        Property LibPlaceboShaderAntiring As New NumParam With {.Text = "     Anti Ring", .HelpSwitch = "--vpp-libplacebo-shader", .Init = 0, .Config = {0, 1, 0.01, 2}}
        Property LibPlaceboShaderLinear As New OptionParam With {.Text = "     Linear", .HelpSwitch = "--vpp-libplacebo-shader", .Init = 0, .Options = {"Undefined (default)", "No", "Yes"}, .Values = {"", "false", "true"}}

        Property LibPlaceboTonemapping As New BoolParam With {.Text = "LibPlacebo-Tonemapping", .Switch = "--vpp-libplacebo-tonemapping", .ValueChangedAction = Sub(x) LibPlaceboTonemapping2.Value = x, .ArgsFunc = AddressOf GetLibPlaceboTonemappingArgs, .ImportAction = AddressOf ImportLibPlaceboTonemappingArgs}
        Property LibPlaceboTonemapping2 As New BoolParam With {.Text = "LibPlacebo-Tonemapping", .HelpSwitch = "--vpp-libplacebo-tonemapping", .ValueChangedAction = Sub(x) LibPlaceboTonemapping.Value = x}
        Property LibPlaceboTonemappingSrcCsp As New OptionParam With {.Text = "     Source Colorspace", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0, .Options = {"Auto (default)", "SDR", "HDR10", "HLG", "Dolby Vision", "RGB"}, .Values = {"auto", "sdr", "hdr10", "hlg", "dovi", "rgb"}}
        Property LibPlaceboTonemappingDstCsp As New OptionParam With {.Text = "     Destination Colorspace", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0, .Options = {"Auto (default)", "SDR", "HDR10", "HLG", "Dolby Vision", "RGB"}, .Values = {"auto", "sdr", "hdr10", "hlg", "dovi", "rgb"}}
        Property LibPlaceboTonemappingDstPlTransfer As New OptionParam With {.Text = "     Output Transfer Function", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0, .Options = {"Unknown (default)", "SRGB", "BT1886", "Linear", "Gamma18", "Gamma20", "Gamma22", "Gamma24", "Gamma26", "Gamma28", "ProPhoto", "ST428", "PQ", "HLG", "Vlog", "Slog1", "Slog2"}, .Values = {"unknown", "srgb", "bt1886", "linear", "gamma18", "gamma20", "gamma22", "gamma24", "gamma26", "gamma28", "prophoto", "st428", "pq", "hlg", "vlog", "slog1", "slog2"}}
        Property LibPlaceboTonemappingDstPlColorprim As New OptionParam With {.Text = "     Output Color Primaries", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0, .Options = {"Unknown (default)", "BT601_525", "BT601_625", "BT709", "BT470m", "EBU_3213", "BT2020", "Apple", "Adobe", "ProPhoto", "Cie 1931", "Dci P3", "Display P3", "V Gamut", "S Gamut", "Film C", "Aces AP0", "Aces AP1"}, .Values = {"unknown", "bt601_525", "bt601_625", "bt709", "bt470m", "ebu_3213", "bt2020", "apple", "adobe", "prophoto", "cie_1931", "dci_p3", "display_p3", "v_gamut", "s_gamut", "film_c", "aces_ap0", "aces_ap1"}}
        Property LibPlaceboTonemappingSrcMax As New NumParam With {.Text = "     Source Max Luminance", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0, .Config = {0, 8000, 1, 1}}
        Property LibPlaceboTonemappingSrcMin As New NumParam With {.Text = "     Source Min Luminance", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0, .Config = {0, 8000, 1, 1}}
        Property LibPlaceboTonemappingDstMax As New NumParam With {.Text = "     Destination Max Luminance", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0, .Config = {0, 8000, 1, 1}}
        Property LibPlaceboTonemappingDstMin As New NumParam With {.Text = "     Destination Min Luminance", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0, .Config = {0, 8000, 1, 1}}
        Property LibPlaceboTonemappingDynPeakDetect As New OptionParam With {.Text = "     Dynamic Peak Detection", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 1, .Options = {"No", "Yes (default)"}, .Values = {"false", "true"}}
        Property LibPlaceboTonemappingSmoothPeriod As New NumParam With {.Text = "     Smooth Period", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 20, .Config = {0, 10000, 0.5, 1}}
        Property LibPlaceboTonemappingSceneThresholdLow As New NumParam With {.Text = "     Scene Threshold Low", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 1, .Config = {0, 100, 0.5, 1}}
        Property LibPlaceboTonemappingSceneThresholdHigh As New NumParam With {.Text = "     Scene Threshold High", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 3, .Config = {0, 100, 0.5, 1}}
        Property LibPlaceboTonemappingPercentile As New NumParam With {.Text = "     Percentile", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 99.995, .Config = {0, 100, 0.001, 3}}
        Property LibPlaceboTonemappingBlackCutoff As New NumParam With {.Text = "     Black Cutoff", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 1, .Config = {0, 100, 0.5, 1}}
        Property LibPlaceboTonemappingGamutMapping As New OptionParam With {.Text = "     Gamut Mapping", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 1, .Options = {"Clip", "Perceptual (default)", "Soft Clip", "Relative", "Saturation", "Absolute", "Desaturate", "Darken", "Highlight", "Linear"}, .Values = {"clip", "perceptual", "softclip", "relative", "saturation", "absolute", "desaturate", "darken", "highlight", "linear"}}
        Property LibPlaceboTonemappingFunction As New OptionParam With {.Text = "     Function", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 3, .Options = {"Clip", "ST2094-40", "ST2094-10", "BT2390 (default)", "BT2446a", "Spline", "Reinhard", "Mobius", "Hable", "Gamma", "Linear", "Linear Light"}, .Values = {"clip", "st2094-40", "st2094-10", "bt2390", "bt2446a", "spline", "reinhard", "mobius", "hable", "gamma", "linear", "linearlight"}}
        Property LibPlaceboTonemappingFunctionKneeAdaption As New NumParam With {.Text = "     Knee Adaption Speed", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0.4, .Config = {0, 1, 0.05, 2}, .VisibleFunc = Function() {"st2094-40", "st2094-10", "spline"}.Contains(LibPlaceboTonemappingFunction.ValueText)}
        Property LibPlaceboTonemappingFunctionKneeMax As New NumParam With {.Text = "     Knee Max Point", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0.8, .Config = {0.5, 1, 0.05, 2}, .VisibleFunc = LibPlaceboTonemappingFunctionKneeAdaption.VisibleFunc}
        Property LibPlaceboTonemappingFunctionKneeMin As New NumParam With {.Text = "     Knee Min Point", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0.1, .Config = {0, 0.5, 0.05, 2}, .VisibleFunc = LibPlaceboTonemappingFunctionKneeAdaption.VisibleFunc}
        Property LibPlaceboTonemappingFunctionKneeDefault As New NumParam With {.Text = "     Knee Default Point", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0.4, .Config = {0, 1, 0.05, 2}, .VisibleFunc = LibPlaceboTonemappingFunctionKneeAdaption.VisibleFunc}
        Property LibPlaceboTonemappingFunctionKneeOffset As New NumParam With {.Text = "     Knee Offset", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 1, .Config = {0.5, 2, 0.05, 2}, .VisibleFunc = Function() {"bt2390"}.Contains(LibPlaceboTonemappingFunction.ValueText)}
        Property LibPlaceboTonemappingFunctionSlopeTuning As New NumParam With {.Text = "     Slope Tuning", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 1.5, .Config = {0, 10, 0.25, 2}, .VisibleFunc = Function() {"spline"}.Contains(LibPlaceboTonemappingFunction.ValueText)}
        Property LibPlaceboTonemappingFunctionSlopeOffset As New NumParam With {.Text = "     Slope Offset", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0.2, .Config = {0, 1, 0.05, 2}, .VisibleFunc = LibPlaceboTonemappingFunctionSlopeTuning.VisibleFunc}
        Property LibPlaceboTonemappingFunctionSplineContrast As New NumParam With {.Text = "     Spline Contrast", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0.5, .Config = {0, 1.5, 0.05, 2}, .VisibleFunc = LibPlaceboTonemappingFunctionSlopeTuning.VisibleFunc}
        Property LibPlaceboTonemappingFunctionReinhardContrast As New NumParam With {.Text = "     Reinhard Contrast", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0.5, .Config = {0, 1, 0.05, 2}, .VisibleFunc = Function() {"reinhard"}.Contains(LibPlaceboTonemappingFunction.ValueText)}
        Property LibPlaceboTonemappingFunctionLinearKnee As New NumParam With {.Text = "     Linear Knee Point", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0.3, .Config = {0, 1, 0.05, 2}, .VisibleFunc = Function() {"mobius", "gamma"}.Contains(LibPlaceboTonemappingFunction.ValueText)}
        Property LibPlaceboTonemappingFunctionExposure As New NumParam With {.Text = "     Exposure", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 1.0, .Config = {0, 10, 0.05, 2}, .VisibleFunc = Function() {"linear", "linearlight"}.Contains(LibPlaceboTonemappingFunction.ValueText)}
        Property LibPlaceboTonemappingFunctionMetadata As New OptionParam With {.Text = "     Metadata", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0, .Options = {"Any (default)", "None", "HDR10", "HDR10+", "CIE Y"}, .Values = {"any", "none", "hdr10", "hdr10plus", "cie_y"}}
        Property LibPlaceboTonemappingFunctionContrastRecovery As New NumParam With {.Text = "     Contrast Recovery", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0.3, .Config = {0, 1, 0.05, 2}}
        Property LibPlaceboTonemappingFunctionContrastSmoothness As New NumParam With {.Text = "     Contrast Smoothness", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 3.5, .Config = {0, 100, 0.25, 2}}
        Property LibPlaceboTonemappingFunctionUseDolbyVision As New OptionParam With {.Text = "     Use Dolby Vision", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0, .Options = {"Auto (default)", "No", "Yes"}, .Values = {"auto", "false", "true"}}
        Property LibPlaceboTonemappingFunctionInverseToneMapping As New OptionParam With {.Text = "     Inverse Tone Mapping", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0, .Options = {"No (default)", "Yes"}, .Values = {"false", "true"}}
        Property LibPlaceboTonemappingFunctionShowClipping As New OptionParam With {.Text = "     Show Clipping", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0, .Options = {"No (default)", "Yes"}, .Values = {"false", "true"}}
        Property LibPlaceboTonemappingFunctionVisualizeLut As New OptionParam With {.Text = "     Visualize LUT", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0, .Options = {"No (default)", "Yes"}, .Values = {"false", "true"}}
        Property LibPlaceboTonemappingFunctionLutType As New OptionParam With {.Text = "     LUT Type", .HelpSwitch = "--vpp-libplacebo-tonemapping", .Init = 0, .Options = {"Undefined (default)", "Native", "Normalized", "Conversion"}, .Values = {"", "native", "normalized", "conversion"}}
        Property LibPlaceboTonemappingLut As New StringParam With {.Text = "     LUT File Path", .HelpSwitch = "--vpp-libplacebo-tonemapping-lut", .BrowseFile = True}



        Property Deinterlacer As New OptionParam With {.Text = "Deinterlacing Method", .HelpSwitch = "", .Init = 0, .Options = {"None", "Hardware (HW Decoder must be set to work!)", "AFS (Activate Auto Field Shift)", "Decomb", "Nnedi", "Yadif"}, .ArgsFunc = AddressOf GetDeinterlacerArgs}

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)

                    Add("Basic",
                        Mode, Codec,
                        New OptionParam With {.Switch = "--preset", .HelpSwitch = "-u", .Text = "Preset", .Init = 6, .Options = {"Default", "Quality", "Performance", "P1 (Performance)", "P2", "P3", "P4 (Default)", "P5", "P6", "P7 (Quality)"}, .Values = {"default", "quality", "performance", "p1", "p2", "p3", "p4", "p5", "p6", "p7"}},
                        OutputDepth,
                        New OptionParam With {.Switch = "--output-csp", .Text = "Colorspace", .Options = {"YUV420 (Default)", "YUV422", "YUV444", "RGB", "YUVA420"}, .Values = {"yuv420", "yuv422", "yuv444", "rgb", "yuva420"}},
                        ProfileH264, ProfileH265, ProfileAV1,
                        New OptionParam With {.Name = "TierH265", .Switch = "--tier", .Text = "Tier", .VisibleFunc = Function() Codec.ValueText = "h265", .Options = {"Main", "High"}},
                        New OptionParam With {.Name = "LevelH264", .Switch = "--level", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "h264", .Options = {"Auto", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                        New OptionParam With {.Name = "LevelH265", .Switch = "--level", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "h265", .Options = {"Auto", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}},
                        New OptionParam With {.Name = "LevelAV1", .Switch = "--level", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "av1", .Options = {"Auto", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "5.3", "6", "6.1"}},
                        Bitrate, VbrQuality, QVBR, QPAdvanced, QP, QPAV1, QPI, QPIAV1, QPP, QPPAV1, QPB, QPBAV1)
                    Add("Rate Control",
                        New StringParam With {.Switch = "--dynamic-rc", .Text = "Dynamic RC"},
                        New OptionParam With {.Switch = "--multipass", .Text = "Multipass", .Options = {"None", "2Pass-Quarter", "2Pass-Full"}, .VisibleFunc = Function() Mode.Value = 0 OrElse Mode.Value > 1},
                        QPInitAdvanced, QPInit8, QPInit8I, QPInit8P, QPInit8B, QPInit10, QPInit10I, QPInit10P, QPInit10B, QPInitAV1, QPInitAV1I, QPInitAV1P, QPInitAV1B,
                        QPMaxAdvanced, QPMax8, QPMax8I, QPMax8P, QPMax8B, QPMax10, QPMax10I, QPMax10P, QPMax10B, QPMaxAV1, QPMaxAV1I, QPMaxAV1P, QPMaxAV1B,
                        QPMinAdvanced, QPMin8, QPMin8I, QPMin8P, QPMin8B, QPMin10, QPMin10I, QPMin10P, QPMin10B, QPMinAV1, QPMinAV1I, QPMinAV1P, QPMinAV1B,
                        New NumParam With {.Switch = "--chroma-qp-offset", .Text = "QP offset for chroma", .Config = {0, Integer.MinValue, 1}, .VisibleFunc = Function() Codec.ValueText = "h264" OrElse Codec.ValueText = "h265"},
                        VbvBufSize, MaxBitrate,
                        AQ,
                        New NumParam With {.Switch = "--aq-strength", .Text = "AQ Strength", .Config = {0, 15}, .VisibleFunc = Function() AQ.Value},
                        New BoolParam With {.Switch = "--aq-temporal", .Text = "Adaptive Quantization (Temporal)"},
                        Lossless)
                    Add("Slice Decision",
                        New OptionParam With {.Switch = "--direct", .Text = "B-Direct Mode", .Options = {"Automatic", "None", "Spatial", "Temporal"}, .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New OptionParam With {.Switch = "--bref-mode", .Text = "B-Frame Ref. Mode", .Options = {"Auto", "Disabled", "Each", "Middle"}},
                        BFrames,
                        New NumParam With {.Switch = "--ref", .Text = "Ref Frames", .Init = 3, .Config = {0, 16}},
                        New OptionParam With {.Switch = "--refs-forward", .Text = "Refs Forward", .Options = {"0 - Auto", "1 - Last", "2 - Last2", "3 - Last3", "4 - Golden"}, .Values = {"0", "1", "2", "3", "4"}, .VisibleFunc = Function() Codec.ValueText = "av1"},
                        New OptionParam With {.Switch = "--refs-backward", .Text = "Refs Backward", .Options = {"0 - Auto", "1 - Backward", "2 - Altref2", "3 - Altref"}, .Values = {"0", "1", "2", "3"}, .VisibleFunc = Function() Codec.ValueText = "av1"},
                        New NumParam With {.Switch = "--lookahead", .Text = "Lookahead", .Config = {0, 32}},
                        New NumParam With {.Switch = "--lookahead-level", .Text = "Lookahead Level", .Config = {0, 3}, .Init = 0},
                        New NumParam With {.Switch = "--gop-len", .Text = "GOP Length", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--slices", .Text = "Slices", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--multiref-l0", .Text = "Multi Ref L0", .Config = {0, 7}, .VisibleFunc = Function() Codec.ValueText = "h264" OrElse Codec.ValueText = "h265"},
                        New NumParam With {.Switch = "--multiref-l1", .Text = "Multi Ref L1", .Config = {0, 7}, .VisibleFunc = Function() Codec.ValueText = "h264" OrElse Codec.ValueText = "h265"},
                        New BoolParam With {.Switch = "--strict-gop", .Text = "Strict GOP"},
                        New BoolParam With {.NoSwitch = "--no-b-adapt", .Text = "B-Adapt", .Init = True},
                        New BoolParam With {.NoSwitch = "--no-i-adapt", .Text = "I-Adapt", .Init = True},
                        New BoolParam With {.Switch = "--nonrefp", .Text = "Enable adapt. non-reference P frame insertion"})
                    Add("Analysis",
                        New OptionParam With {.Switch = "--adapt-transform", .Text = "Adaptive Transform", .Options = {"Automatic", "Enabled", "Disabled"}, .Values = {"", "--adapt-transform", "--no-adapt-transform"}, .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New NumParam With {.Switch = "--cu-min", .Text = "Minimum CU Size", .Config = {0, 32, 16}, .VisibleFunc = Function() Codec.ValueText = "h265"},
                        New NumParam With {.Switch = "--cu-max", .Text = "Maximum CU Size", .Config = {0, 64, 16}, .VisibleFunc = Function() Codec.ValueText = "h265"},
                        New OptionParam With {.Switch = "--tf-level", .Text = "Temporal Filtering", .Options = {"0 (default)", "4"}, .Values = {"0", "4"}, .VisibleFunc = Function() BFrames.Value >= 4},
                        New OptionParam With {.Switch = "--part-size-min", .Text = "Part Size Min", .Options = {"0 (auto)", "4", "8", "16", "32", "64"}, .Values = {"0", "4", "8", "16", "32", "64"}, .VisibleFunc = Function() Codec.ValueText = "av1"},
                        New OptionParam With {.Switch = "--part-size-max", .Text = "Part Size Max", .Options = {"0 (auto)", "4", "8", "16", "32", "64"}, .Values = {"0", "4", "8", "16", "32", "64"}, .VisibleFunc = Function() Codec.ValueText = "av1"},
                        New OptionParam With {.Switch = "--tile-columns", .Text = "Tile Columns", .Options = {"0 (auto)", "1", "2", "4", "8", "16", "32", "64"}, .Values = {"0", "1", "2", "4", "8", "16", "32", "64"}, .VisibleFunc = Function() Codec.ValueText = "av1"},
                        New OptionParam With {.Switch = "--tile-rows", .Text = "Tile Rows", .Options = {"0 (auto)", "1", "2", "4", "8", "16", "32", "64"}, .Values = {"0", "1", "2", "4", "8", "16", "32", "64"}, .VisibleFunc = Function() Codec.ValueText = "av1"},
                        New NumParam With {.Switch = "--temporal-layers", .Text = "Temporal Layers", .Config = {0, Integer.MaxValue, 1}},
                        New BoolParam With {.Switch = "--weightp", .Text = "Enable weighted prediction in P slices"})
                    Add("VPP | Misc",
                        New StringParam With {.Switch = "--vpp-curves", .Text = "Curves"},
                        New StringParam With {.Switch = "--vpp-overlay", .Text = "Overlay"},
                        New StringParam With {.Switch = "--vpp-subburn", .Text = "Subburn"},
                        New OptionParam With {.Switch = "--vpp-rotate", .Text = "Rotate", .Options = {"Disabled", "90", "180", "270"}},
                        New BoolParam With {.Switch = "--vpp-rff", .Text = "Enable repeat field flag", .VisibleFunc = Function() Decoder.ValueText.EqualsAny("avhw", "avsw")},
                        SelectEvery, SelectEveryValue, SelectEveryOffsets)
                    Add("VPP | Misc 2",
                        Tweak,
                        TweakBrightness,
                        TweakContrast,
                        TweakSaturation,
                        TweakGamma,
                        TweakHue,
                        Pad,
                        PadLeft,
                        PadTop,
                        PadRight,
                        PadBottom,
                        Smooth,
                        SmoothQuality,
                        SmoothQP,
                        SmoothPrec)
                    Add("VPP | Misc 3",
                        TransformFlipX,
                        TransformFlipY,
                        TransformTranspose,
                        New StringParam With {.Switch = "--vpp-decimate", .Text = "Decimate"},
                        New StringParam With {.Switch = "--vpp-mpdecimate", .Text = "MP Decimate"})
                    Add("VPP | Colorspace",
                        Colorspace,
                        ColorspaceMatrixFrom,
                        ColorspaceMatrixTo,
                        ColorspaceColorprimFrom,
                        ColorspaceColorprimTo,
                        ColorspaceTransferFrom,
                        ColorspaceTransferTo,
                        ColorspaceRangeFrom,
                        ColorspaceRangeTo,
                        ColorspaceLut3d,
                        ColorspaceLut3dinterp)
                    Add("VPP | Colorspace | HDR2SDR",
                        ColorspaceHdr2sdr,
                        ColorspaceHdr2sdrSourcepeak,
                        ColorspaceHdr2sdrLdrnits,
                        ColorspaceHdr2sdrDesatbase,
                        ColorspaceHdr2sdrDesatstrength,
                        ColorspaceHdr2sdrDesatexp,
                        ColorspaceHdr2sdrHableA,
                        ColorspaceHdr2sdrHableB,
                        ColorspaceHdr2sdrHableC,
                        ColorspaceHdr2sdrHableD,
                        ColorspaceHdr2sdrHableE,
                        ColorspaceHdr2sdrHableF,
                        ColorspaceHdr2sdrMobiusTransition,
                        ColorspaceHdr2sdrMobiusPeak,
                        ColorspaceHdr2sdrReinhardContrast,
                        ColorspaceHdr2sdrReinhardPeak)
                    Add("VPP | Ngx-TrueHDR",
                        NgxTruehdr, NgxTruehdrContrast, NgxTruehdrSaturation, NgxTruehdrMiddleGray, NgxTruehdrMaxLuminance
                        )
                    Add("VPP | Deband",
                        Deband, DebandRange, DebandSample, DebandThre, DebandThreY, DebandThreCB, DebandThreCR, DebandDither, DebandDitherY, DebandDitherC, DebandSeed, DebandBlurfirst, DebandRandEachFrame)
                    Add("VPP | Deinterlace",
                        Deinterlacer,
                        New OptionParam With {.Switch = "--vpp-deinterlace", .Text = "Deinterlace Mode", .VisibleFunc = Function() Deinterlacer.Value = 1 AndAlso Decoder.ValueText.EqualsAny("avhw"), .AlwaysOn = True, .Options = {"Normal", "Adaptive", "Bob"}},
                        New OptionParam With {.Switch = "--vpp-yadif", .Text = "Yadif Mode", .VisibleFunc = Function() Deinterlacer.Value = 5, .AlwaysOn = True, .Options = {"Auto", "TFF", "BFF", "Bob", "Bob TFF", "Bob BFF"}, .Values = {"", "mode=tff", "mode=bff", "mode=bob", "mode=bob_tff", "mode=bob_bff"}},
                        AfsINI, AfsPreset, AfsLeft, AfsRight, AfsTop, AfsBottom, AfsMethodSwitch, AfsCoeffShift, AfsThreShift, AfsThreDeint, AfsThreMotionY, AfsThreMotionC, AfsLevel,
                        DecombFull, DecombThreshold, DecombDThreshold, DecombBlend,
                        NnediField, NnediNns, NnediNsize, NnediQuality, NnediPrescreen, NnediErrortype, NnediPrec, NnediWeightfile)
                    Add("VPP | Deinterlace | AFS 2",
                        AfsShift, AfsDrop, AfsSmooth, Afs24fps, AfsTune, AfsRFF, AfsTimecode, AfsLog)
                    Add("VPP | Denoise",
                        Fft3d, Fft3dSigma, Fft3dAmount, Fft3dBlockSize, Fft3dOverlap, Fft3dMethod, Fft3dTemporal, Fft3dPrec,
                        Knn, KnnRadius, KnnStrength, KnnLerp, KnnThLerp)
                    Add("VPP | Denoise 2",
                        Convolution, ConvolutionMatrix, ConvolutionFast, ConvolutionYthresh, ConvolutionCthresh, ConvolutionTYthresh, ConvolutionTCthresh,
                        Pmd, PmdApplyCount, PmdStrength, PmdThreshold)
                    Add("VPP | Denoise 3",
                        New OptionParam With {.Switch = "--vpp-gauss", .Text = "Gauss", .Options = {"Disabled", "3", "5", "7"}},
                        DenoiseDct, DenoiseDctStep, DenoiseDctSigma, DenoiseDctBlockSize,
                        NvvfxDenoise, NvvfxDenoiseStrength,
                        NvvfxArtifactReduction, NvvfxArtifactReductionMode,
                        Nlmeans, NlmeansSigma, NlmeansH, NlmeansPatch, NlmeansSearch, NlmeansFp16)
                    Add("VPP | LibPlacebo | Deband",
                        LibPlaceboDeband, LibPlaceboDebandIterations, LibPlaceboDebandThreshold, LibPlaceboDebandRadius, LibPlaceboDebandGrainY, LibPlaceboDebandGrainC, LibPlaceboDebandDither, LibPlaceboDebandLutSize)
                    Add("VPP | LibPlacebo | Shader",
                        LibPlaceboShader, LibPlaceboShaderShader, LibPlaceboShaderResWidth, LibPlaceboShaderResHeight, LibPlaceboShaderColorsystem, LibPlaceboShaderTransfer, LibPlaceboShaderResampler, LibPlaceboShaderRadius, LibPlaceboShaderRadiusValue, LibPlaceboShaderClamp, LibPlaceboShaderTaper, LibPlaceboShaderBlur, LibPlaceboShaderAntiring, LibPlaceboShaderLinear)
                    Add("VPP | LibPlacebo | Tonemapping",
                        LibPlaceboTonemapping,
                        LibPlaceboTonemappingSrcCsp, LibPlaceboTonemappingDstCsp, LibPlaceboTonemappingDstPlTransfer, LibPlaceboTonemappingDstPlColorprim,
                        LibPlaceboTonemappingSrcMax, LibPlaceboTonemappingSrcMin, LibPlaceboTonemappingDstMax, LibPlaceboTonemappingDstMin,
                        LibPlaceboTonemappingDynPeakDetect, LibPlaceboTonemappingSmoothPeriod, LibPlaceboTonemappingSceneThresholdLow, LibPlaceboTonemappingSceneThresholdHigh, LibPlaceboTonemappingPercentile, LibPlaceboTonemappingBlackCutoff, LibPlaceboTonemappingGamutMapping)
                    Add("VPP | LibPlacebo | Tonemapping 2",
                        LibPlaceboTonemapping2,
                        LibPlaceboTonemappingFunction, LibPlaceboTonemappingFunctionKneeAdaption,
                        LibPlaceboTonemappingFunctionKneeMax, LibPlaceboTonemappingFunctionKneeMin, LibPlaceboTonemappingFunctionKneeDefault, LibPlaceboTonemappingFunctionKneeOffset,
                        LibPlaceboTonemappingFunctionSlopeTuning, LibPlaceboTonemappingFunctionSlopeOffset, LibPlaceboTonemappingFunctionSplineContrast, LibPlaceboTonemappingFunctionReinhardContrast,
                        LibPlaceboTonemappingFunctionLinearKnee, LibPlaceboTonemappingFunctionExposure, LibPlaceboTonemappingFunctionMetadata,
                        LibPlaceboTonemappingFunctionContrastRecovery, LibPlaceboTonemappingFunctionContrastSmoothness,
                        LibPlaceboTonemappingFunctionUseDolbyVision, LibPlaceboTonemappingFunctionInverseToneMapping, LibPlaceboTonemappingFunctionShowClipping,
                        LibPlaceboTonemappingFunctionVisualizeLut, LibPlaceboTonemappingFunctionLutType, LibPlaceboTonemappingLut)
                    Add("VPP | Resize",
                        Resize, ResizeAlgo, ResizeSuperresMode, ResizeSuperresStrength, ResizeVsrQuality,
                        ResizeLibplaceboFilter, ResizeLibplaceboPlRadius, ResizeLibplaceboPlClamp, ResizeLibplaceboPlTaper, ResizeLibplaceboPlBlur, ResizeLibplaceboPlAntiring)
                    Add("VPP | Sharpness",
                        Edgelevel, EdgelevelStrength, EdgelevelThreshold, EdgelevelBlack, EdgelevelWhite,
                        Unsharp, UnsharpRadius, UnsharpWeight, UnsharpThreshold,
                        Warpsharp, WarpsharpThreshold, WarpsharpBlur, WarpsharpType, WarpsharpDepth, WarpsharpChroma)
                    Add("VUI",
                        New StringParam With {.Switch = "--master-display", .Text = "Master Display", .VisibleFunc = Function() Codec.ValueText = "h265" OrElse Codec.ValueText = "av1"},
                        New StringParam With {.Switch = "--sar", .Text = "Sample Aspect Ratio", .Init = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                        DhdrInfo, DolbyVisionProfileH265, DolbyVisionProfileAV1, DolbyVisionRpu,
                        New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat", .Options = {"Undefined", "NTSC", "Component", "PAL", "SECAM", "MAC"}},
                        ColorMatrix, ColorPrim, Transfer, ColorRange, AtcSei,
                        New OptionParam With {.Switch = "--split-enc", .Text = "Split Enc", .Options = {"Split frame forced mode disabled, split frame auto mode enabled.", "Split frame forced mode enabled w/ no. of strips auto-selected by driver.", "Forced 2-strip split frame encoding (if NVENC <= 1 1-strip encode).", "Forced 3-strip split frame encoding (if NVENC > 2, NVENC no# of strips otherwise).", "Forced 4-strip split frame encoding (if NVENC > 3, NVENC no# of strips otherwise).", "Both split frame auto mode and forced mode are disabled."}, .Values = {"auto", "auto_forced", "forced_2", "forced_3", "forced_4", "disable"}})
                    Add("VUI 2",
                        MaxCLL, MaxFALL,
                        New NumParam With {.Switch = "--chromaloc", .Text = "Chromaloc", .Config = {0, 5}},
                        New BoolParam With {.Switch = "--pic-struct", .Text = "Set the picture structure and emits it in the picture timing SEI message", .VisibleFunc = Function() Codec.ValueText = "h264" OrElse Codec.ValueText = "h265"},
                        New BoolParam With {.Switch = "--aud", .Text = "Insert Access Unit Delimiter NAL", .VisibleFunc = Function() Codec.ValueText = "h264" OrElse Codec.ValueText = "h265"},
                        New BoolParam With {.Switch = "--repeat-headers", .Text = "Output VPS, SPS and PPS for every IDR frame"})
                    Add("Performance",
                        New StringParam With {.Switch = "--thread-affinity", .Text = "Thread Affinity"},
                        New StringParam With {.Switch = "--perf-monitor", .Text = "Perf. Monitor"},
                        New OptionParam With {.Switch = "--cuda-schedule", .Text = "Cuda Schedule", .Init = 3, .Options = {"Let cuda driver to decide", "CPU will spin when waiting GPU tasks", "CPU will yield when waiting GPU tasks", "CPU will sleep when waiting GPU tasks"}, .Values = {"auto", "spin", "yield", "sync"}},
                        New OptionParam With {.Switch = "--output-buf", .Text = "Output Buffer", .Options = {"8", "16", "32", "64", "128"}},
                        New OptionParam With {.Switch = "--output-thread", .Text = "Output Thread", .Options = {"Automatic", "Disabled", "One Thread"}, .Values = {"-1", "0", "1"}},
                        New NumParam With {.Switch = "--perf-monitor-interval", .Init = 500, .Config = {50, Integer.MaxValue}, .Text = "Perf. Mon. Interval"},
                        New BoolParam With {.Switch = "--max-procfps", .Text = "Limit performance to lower resource usage"},
                        New BoolParam With {.Switch = "--lowlatency", .Text = "Low Latency"},
                        New BoolParam With {.Switch = "--task-perf-monitor", .Text = "Task Perf Monitor"})
                    Add("Statistic",
                        New StringParam With {.Switch = "--log", .Text = "Log File", .BrowseFile = True},
                        New OptionParam With {.Switch = "--log-level", .Text = "Log Level", .Options = {"Info", "Debug", "Warn", "Error", "Quiet"}},
                        New OptionParam With {.Switch = "--disable-nvml", .Text = "NVML GPU Monitoring", .Options = {"Enabled NVML (default)", "Disable NVML when system has one CUDA devices", "Always disable NVML"}, .IntegerValue = True},
                        New BoolParam With {.Switch = "--ssim", .Text = "SSIM"},
                        New BoolParam With {.Switch = "--psnr", .Text = "PSNR"},
                        Vmaf, VmafString)
                    Add("Input/Output",
                        New StringParam With {.Switch = "--input-option", .Text = "Input Option", .VisibleFunc = Function() Decoder.ValueText.EqualsAny("avhw", "avsw")},
                        Decoder, Interlace,
                        New NumParam With {.Switch = "--input-analyze", .Text = "Input Analyze", .Init = 5, .Config = {1, 600, 0.1, 1}})
                    Add("Misc",
                        New StringParam With {.Switch = "--keyfile", .Text = "Keyframes File", .BrowseFile = True},
                        New StringParam With {.Switch = "--timecode", .Text = "Timecode File", .BrowseFile = True},
                        New StringParam With {.Switch = "--data-copy", .Text = "Data Copy"},
                        New OptionParam With {.Switch = "--mv-precision", .Text = "MV Precision", .Options = {"Automatic", "Q-pel", "Half-pel", "Full-pel"}},
                        New OptionParam With {.Switches = {"--cabac", "--cavlc"}, .Text = "Cabac/Cavlc", .Options = {"Disabled", "Cabac", "Cavlc"}, .Values = {"", "--cabac", "--cavlc"}, .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New NumParam With {.Switch = "--device", .HelpSwitch = "-d", .Text = "Device", .Init = -1, .Config = {-1, 16}},
                        New BoolParam With {.Switch = "--deblock", .NoSwitch = "--no-deblock", .Text = "Deblock", .Init = True, .VisibleFunc = Function() Codec.ValueText = "h264"},
                        New BoolParam With {.Switch = "--bluray", .Text = "Blu-ray", .VisibleFunc = Function() Codec.ValueText = "h264"}
                    )
                    Add("Other",
                        OverrideTargetFileName, TargetFileName, TargetFileNamePreview,
                        New LineParam(),
                        Custom
                    )
                End If

                Return ItemsValue
            End Get
        End Property

        Public Overrides Sub ShowHelp(options As String())
            ShowConsoleHelp(Package.NVEncC, options)
        End Sub

        Private BlockValueChanged As Boolean

        Protected Overrides Sub OnValueChanged(item As CommandLineParam)
            If BlockValueChanged Then Exit Sub

            If Decoder.MenuButton IsNot Nothing AndAlso (item Is Decoder OrElse item Is Nothing) Then
                Dim isIntelPresent = OS.VideoControllers.Where(Function(val) val.Contains("Intel")).Count > 0

                For x = 0 To Decoder.Options.Length - 1
                    If Decoder.Options(x).Contains("Intel") Then
                        Decoder.ShowOption(x, isIntelPresent)
                    End If
                Next
            End If

            For i = 0 To Interlace.Values.Length - 1
                If Interlace.Values(i).ToLowerInvariant().Contains("auto") Then
                    Dim enabled = Decoder.ValueText.EqualsAny("avhw", "avsw")
                    Interlace.ShowOption(i, enabled)
                    If Not enabled AndAlso Interlace.Value = i Then Interlace.Value = 0
                End If
            Next

            If QPI.NumEdit IsNot Nothing Then
                AfsPreset.MenuButton.Enabled = Deinterlacer.Value = 2
                AfsINI.TextEdit.Enabled = Deinterlacer.Value = 2

                For Each i In {AfsLeft, AfsRight, AfsTop, AfsBottom, AfsMethodSwitch, AfsCoeffShift,
                               AfsThreShift, AfsThreDeint, AfsThreMotionY, AfsThreMotionC, AfsLevel}

                    i.NumEdit.Enabled = Deinterlacer.Value = 2
                Next

                For Each i In {AfsShift, AfsDrop, AfsSmooth, Afs24fps, AfsTune, AfsRFF, AfsTimecode, AfsLog}
                    i.CheckBox.Enabled = Deinterlacer.Value = 2
                Next

                DecombFull.MenuButton.Enabled = Deinterlacer.Value = 3
                DecombThreshold.NumEdit.Enabled = Deinterlacer.Value = 3
                DecombDThreshold.NumEdit.Enabled = Deinterlacer.Value = 3
                DecombBlend.MenuButton.Enabled = Deinterlacer.Value = 3

                NnediField.MenuButton.Enabled = Deinterlacer.Value = 4
                NnediNns.MenuButton.Enabled = Deinterlacer.Value = 4
                NnediNsize.MenuButton.Enabled = Deinterlacer.Value = 4
                NnediQuality.MenuButton.Enabled = Deinterlacer.Value = 4
                NnediPrescreen.MenuButton.Enabled = Deinterlacer.Value = 4
                NnediErrortype.MenuButton.Enabled = Deinterlacer.Value = 4
                NnediPrec.MenuButton.Enabled = Deinterlacer.Value = 4
                NnediWeightfile.TextEdit.Enabled = Deinterlacer.Value = 4

                ColorspaceMatrixFrom.MenuButton.Enabled = Colorspace.Value
                ColorspaceMatrixTo.MenuButton.Enabled = Colorspace.Value
                ColorspaceColorprimFrom.MenuButton.Enabled = Colorspace.Value
                ColorspaceColorprimTo.MenuButton.Enabled = Colorspace.Value
                ColorspaceTransferFrom.MenuButton.Enabled = Colorspace.Value
                ColorspaceTransferTo.MenuButton.Enabled = Colorspace.Value
                ColorspaceRangeFrom.MenuButton.Enabled = Colorspace.Value
                ColorspaceRangeTo.MenuButton.Enabled = Colorspace.Value
                ColorspaceLut3d.TextEdit.Enabled = Colorspace.Value
                ColorspaceLut3dinterp.MenuButton.Enabled = Colorspace.Value
                ColorspaceHdr2sdr.MenuButton.Enabled = Colorspace.Value
                ColorspaceHdr2sdrSourcepeak.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrLdrnits.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrDesatbase.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrDesatstrength.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrDesatexp.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrHableA.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrHableB.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrHableC.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrHableD.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrHableE.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrHableF.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrMobiusTransition.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrMobiusPeak.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrReinhardContrast.NumEdit.Enabled = Colorspace.Value
                ColorspaceHdr2sdrReinhardPeak.NumEdit.Enabled = Colorspace.Value

                NgxTruehdrContrast.NumEdit.Enabled = NgxTruehdr.Value
                NgxTruehdrSaturation.NumEdit.Enabled = NgxTruehdr.Value
                NgxTruehdrMiddleGray.NumEdit.Enabled = NgxTruehdr.Value
                NgxTruehdrMaxLuminance.NumEdit.Enabled = NgxTruehdr.Value

                LibPlaceboDebandIterations.NumEdit.Enabled = LibPlaceboDeband.Value
                LibPlaceboDebandThreshold.NumEdit.Enabled = LibPlaceboDeband.Value
                LibPlaceboDebandRadius.NumEdit.Enabled = LibPlaceboDeband.Value
                LibPlaceboDebandGrainY.NumEdit.Enabled = LibPlaceboDeband.Value
                LibPlaceboDebandGrainC.NumEdit.Enabled = LibPlaceboDeband.Value
                LibPlaceboDebandDither.MenuButton.Enabled = LibPlaceboDeband.Value
                LibPlaceboDebandLutSize.MenuButton.Enabled = LibPlaceboDeband.Value

                LibPlaceboShaderShader.TextEdit.Enabled = LibPlaceboShader.Value
                LibPlaceboShaderResWidth.NumEdit.Enabled = LibPlaceboShader.Value
                LibPlaceboShaderResHeight.NumEdit.Enabled = LibPlaceboShader.Value
                LibPlaceboShaderColorsystem.MenuButton.Enabled = LibPlaceboShader.Value
                LibPlaceboShaderTransfer.MenuButton.Enabled = LibPlaceboShader.Value
                LibPlaceboShaderResampler.MenuButton.Enabled = LibPlaceboShader.Value
                LibPlaceboShaderRadius.MenuButton.Enabled = LibPlaceboShader.Value
                LibPlaceboShaderRadiusValue.NumEdit.Enabled = LibPlaceboShader.Value
                LibPlaceboShaderClamp.NumEdit.Enabled = LibPlaceboShader.Value
                LibPlaceboShaderTaper.NumEdit.Enabled = LibPlaceboShader.Value
                LibPlaceboShaderBlur.NumEdit.Enabled = LibPlaceboShader.Value
                LibPlaceboShaderAntiring.NumEdit.Enabled = LibPlaceboShader.Value
                LibPlaceboShaderLinear.MenuButton.Enabled = LibPlaceboShader.Value

                LibPlaceboTonemappingSrcCsp.MenuButton.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingDstCsp.MenuButton.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingDstPlTransfer.MenuButton.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingDstPlColorprim.MenuButton.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingSrcMax.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingSrcMin.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingDstMax.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingDstMin.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingDynPeakDetect.MenuButton.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingSmoothPeriod.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingSceneThresholdLow.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingSceneThresholdHigh.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingPercentile.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingBlackCutoff.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingGamutMapping.MenuButton.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunction.MenuButton.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionKneeAdaption.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionKneeMax.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionKneeMin.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionKneeDefault.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionKneeOffset.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionSlopeTuning.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionSlopeOffset.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionSplineContrast.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionReinhardContrast.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionLinearKnee.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionExposure.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionMetadata.MenuButton.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionContrastRecovery.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionContrastSmoothness.NumEdit.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionUseDolbyVision.MenuButton.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionInverseToneMapping.MenuButton.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionShowClipping.MenuButton.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionVisualizeLut.MenuButton.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingFunctionLutType.MenuButton.Enabled = LibPlaceboTonemapping.Value
                LibPlaceboTonemappingLut.TextEdit.Enabled = LibPlaceboTonemapping.Value

                EdgelevelStrength.NumEdit.Enabled = Edgelevel.Value
                EdgelevelThreshold.NumEdit.Enabled = Edgelevel.Value
                EdgelevelBlack.NumEdit.Enabled = Edgelevel.Value
                EdgelevelWhite.NumEdit.Enabled = Edgelevel.Value

                UnsharpRadius.NumEdit.Enabled = Unsharp.Value
                UnsharpWeight.NumEdit.Enabled = Unsharp.Value
                UnsharpThreshold.NumEdit.Enabled = Unsharp.Value

                WarpsharpBlur.NumEdit.Enabled = Warpsharp.Value
                WarpsharpChroma.NumEdit.Enabled = Warpsharp.Value
                WarpsharpDepth.NumEdit.Enabled = Warpsharp.Value
                WarpsharpThreshold.NumEdit.Enabled = Warpsharp.Value
                WarpsharpType.NumEdit.Enabled = Warpsharp.Value

                SelectEveryValue.NumEdit.Enabled = SelectEvery.Value
                SelectEveryOffsets.TextEdit.Enabled = SelectEvery.Value

                ResizeAlgo.MenuButton.Enabled = Resize.Value
                ResizeSuperresMode.MenuButton.Enabled = Resize.Value
                ResizeSuperresStrength.NumEdit.Enabled = Resize.Value
                ResizeVsrQuality.NumEdit.Enabled = Resize.Value
                ResizeLibplaceboFilter.MenuButton.Enabled = Resize.Value
                ResizeLibplaceboPlRadius.NumEdit.Enabled = Resize.Value
                ResizeLibplaceboPlClamp.NumEdit.Enabled = Resize.Value
                ResizeLibplaceboPlTaper.NumEdit.Enabled = Resize.Value
                ResizeLibplaceboPlBlur.NumEdit.Enabled = Resize.Value
                ResizeLibplaceboPlAntiring.NumEdit.Enabled = Resize.Value

                Fft3dSigma.NumEdit.Enabled = Fft3d.Value
                Fft3dAmount.NumEdit.Enabled = Fft3d.Value
                Fft3dBlockSize.MenuButton.Enabled = Fft3d.Value
                Fft3dOverlap.NumEdit.Enabled = Fft3d.Value
                Fft3dMethod.MenuButton.Enabled = Fft3d.Value
                Fft3dTemporal.MenuButton.Enabled = Fft3d.Value
                Fft3dPrec.MenuButton.Enabled = Fft3d.Value

                KnnRadius.NumEdit.Enabled = Knn.Value
                KnnStrength.NumEdit.Enabled = Knn.Value
                KnnLerp.NumEdit.Enabled = Knn.Value
                KnnThLerp.NumEdit.Enabled = Knn.Value

                ConvolutionMatrix.MenuButton.Enabled = Convolution.Value
                ConvolutionFast.MenuButton.Enabled = Convolution.Value
                ConvolutionYthresh.NumEdit.Enabled = Convolution.Value
                ConvolutionCthresh.NumEdit.Enabled = Convolution.Value
                ConvolutionTYthresh.NumEdit.Enabled = Convolution.Value
                ConvolutionTCthresh.NumEdit.Enabled = Convolution.Value

                PadLeft.NumEdit.Enabled = Pad.Value
                PadTop.NumEdit.Enabled = Pad.Value
                PadRight.NumEdit.Enabled = Pad.Value
                PadBottom.NumEdit.Enabled = Pad.Value

                SmoothQuality.NumEdit.Enabled = Smooth.Value
                SmoothQP.NumEdit.Enabled = Smooth.Value
                SmoothPrec.MenuButton.Enabled = Smooth.Value

                DenoiseDctStep.MenuButton.Enabled = DenoiseDct.Value
                DenoiseDctSigma.NumEdit.Enabled = DenoiseDct.Value
                DenoiseDctBlockSize.MenuButton.Enabled = DenoiseDct.Value

                TweakContrast.NumEdit.Enabled = Tweak.Value
                TweakGamma.NumEdit.Enabled = Tweak.Value
                TweakSaturation.NumEdit.Enabled = Tweak.Value
                TweakHue.NumEdit.Enabled = Tweak.Value
                TweakBrightness.NumEdit.Enabled = Tweak.Value

                NlmeansSigma.NumEdit.Enabled = Nlmeans.Value
                NlmeansH.NumEdit.Enabled = Nlmeans.Value
                NlmeansPatch.NumEdit.Enabled = Nlmeans.Value
                NlmeansSearch.NumEdit.Enabled = Nlmeans.Value
                NlmeansFp16.MenuButton.Enabled = Nlmeans.Value

                PmdApplyCount.NumEdit.Enabled = Pmd.Value
                PmdStrength.NumEdit.Enabled = Pmd.Value
                PmdThreshold.NumEdit.Enabled = Pmd.Value

                For Each i In {DebandRange, DebandSample, DebandThre, DebandThreY, DebandThreCB,
                    DebandThreCR, DebandDither, DebandDitherY, DebandDitherC, DebandSeed}

                    i.NumEdit.Enabled = Deband.Value
                Next

                DebandRandEachFrame.CheckBox.Enabled = Deband.Value
                DebandBlurfirst.CheckBox.Enabled = Deband.Value
            End If

            If item IsNot TargetFileName AndAlso item IsNot TargetFileNamePreview Then TargetFileName.TextChangedAction?.Invoke(TargetFileName.Value)

            MyBase.OnValueChanged(item)
        End Sub

        Function GetSmoothArgs() As String
            If Smooth.Value Then
                Dim ret = ""
                If SmoothQuality.Value <> SmoothQuality.DefaultValue Then ret += ",quality=" & SmoothQuality.Value
                If SmoothQP.Value <> SmoothQP.DefaultValue Then ret += ",qp=" & SmoothQP.Value.ToInvariantString
                If SmoothPrec.Value <> SmoothPrec.DefaultValue Then ret += ",prec=" & SmoothPrec.ValueText
                Return "--vpp-smooth " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetDenoiseDctArgs() As String
            If DenoiseDct.Value Then
                Dim ret = ""
                If DenoiseDctStep.Value <> DenoiseDctStep.DefaultValue Then ret += ",step=" & DenoiseDctStep.ValueText
                If DenoiseDctSigma.Value <> DenoiseDctSigma.DefaultValue Then ret += ",sigma=" & DenoiseDctSigma.Value.ToInvariantString
                If DenoiseDctBlockSize.Value <> DenoiseDctBlockSize.DefaultValue Then ret += ",block_size=" & DenoiseDctBlockSize.ValueText
                Return "--vpp-denoise-dct " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetNlmeansArgs() As String
            If Nlmeans.Value Then
                Dim ret = ""
                If NlmeansSigma.Value <> NlmeansSigma.DefaultValue Then ret += ",sigma=" & NlmeansSigma.Value.ToInvariantString
                If NlmeansH.Value <> NlmeansH.DefaultValue Then ret += ",h=" & NlmeansH.Value.ToInvariantString
                If NlmeansPatch.Value <> NlmeansPatch.DefaultValue Then ret += ",patch=" & NlmeansPatch.Value.ToInvariantString
                If NlmeansSearch.Value <> NlmeansSearch.DefaultValue Then ret += ",search=" & NlmeansSearch.Value.ToInvariantString
                If NlmeansFp16.Value <> NlmeansFp16.DefaultValue Then ret += ",fp16=" & NlmeansFp16.ValueText
                Return "--vpp-nlmeans " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetColorspaceArgs() As String
            If Colorspace.Value Then
                Dim ret = ""
                If ColorspaceMatrixFrom.Value <> ColorspaceMatrixFrom.DefaultValue Then ret += $",matrix={ColorspaceMatrixFrom.ValueText}:{ColorspaceMatrixTo.ValueText}"
                If ColorspaceColorprimFrom.Value <> ColorspaceColorprimFrom.DefaultValue Then ret += $",colorprim={ColorspaceColorprimFrom.ValueText}:{ColorspaceColorprimTo.ValueText}"
                If ColorspaceTransferFrom.Value <> ColorspaceTransferFrom.DefaultValue Then ret += $",transfer={ColorspaceTransferFrom.ValueText}:{ColorspaceTransferTo.ValueText}"
                If ColorspaceRangeFrom.Value <> ColorspaceRangeFrom.DefaultValue Then ret += $",range={ColorspaceRangeFrom.ValueText}:{ColorspaceRangeTo.ValueText}"
                If ColorspaceLut3d.Value <> ColorspaceLut3d.DefaultValue Then ret += $",lut3d={ColorspaceLut3d.Value},lut3d_interp={ColorspaceLut3dinterp.ValueText}"
                If ColorspaceHdr2sdr.Value <> ColorspaceHdr2sdr.DefaultValue Then
                    ret += $",hdr2sdr={ColorspaceHdr2sdr.ValueText}"
                    If ColorspaceHdr2sdrSourcepeak.Value <> ColorspaceHdr2sdrSourcepeak.DefaultValue Then ret += $",source_peak={ColorspaceHdr2sdrSourcepeak.Value.ToInvariantString("0.0")}"
                    If ColorspaceHdr2sdrLdrnits.Value <> ColorspaceHdr2sdrLdrnits.DefaultValue Then ret += $",ldr_nits={ColorspaceHdr2sdrLdrnits.Value.ToInvariantString("0.0")}"
                    If ColorspaceHdr2sdrDesatbase.Value <> ColorspaceHdr2sdrDesatbase.DefaultValue Then ret += $",desat_base={ColorspaceHdr2sdrDesatbase.Value.ToInvariantString("0.00")}"
                    If ColorspaceHdr2sdrDesatstrength.Value <> ColorspaceHdr2sdrDesatstrength.DefaultValue Then ret += $",desat_strength={ColorspaceHdr2sdrDesatstrength.Value.ToInvariantString("0.00")}"
                    If ColorspaceHdr2sdrDesatexp.Value <> ColorspaceHdr2sdrDesatexp.DefaultValue Then ret += $",desat_exp={ColorspaceHdr2sdrDesatexp.Value.ToInvariantString("0.00")}"
                    If ColorspaceHdr2sdr.Value = 1 Then
                        If ColorspaceHdr2sdrHableA.Value <> ColorspaceHdr2sdrHableA.DefaultValue Then ret += $",a={ColorspaceHdr2sdrHableA.Value.ToInvariantString("0.00")}"
                        If ColorspaceHdr2sdrHableB.Value <> ColorspaceHdr2sdrHableB.DefaultValue Then ret += $",b={ColorspaceHdr2sdrHableB.Value.ToInvariantString("0.00")}"
                        If ColorspaceHdr2sdrHableC.Value <> ColorspaceHdr2sdrHableC.DefaultValue Then ret += $",c={ColorspaceHdr2sdrHableC.Value.ToInvariantString("0.00")}"
                        If ColorspaceHdr2sdrHableD.Value <> ColorspaceHdr2sdrHableD.DefaultValue Then ret += $",d={ColorspaceHdr2sdrHableD.Value.ToInvariantString("0.00")}"
                        If ColorspaceHdr2sdrHableE.Value <> ColorspaceHdr2sdrHableE.DefaultValue Then ret += $",e={ColorspaceHdr2sdrHableE.Value.ToInvariantString("0.00")}"
                        If ColorspaceHdr2sdrHableF.Value <> ColorspaceHdr2sdrHableF.DefaultValue Then ret += $",f={ColorspaceHdr2sdrHableF.Value.ToInvariantString("0.00")}"
                    End If
                    If ColorspaceHdr2sdr.Value = 2 Then
                        If ColorspaceHdr2sdrMobiusTransition.Value <> ColorspaceHdr2sdrMobiusTransition.DefaultValue Then ret += $",transition={ColorspaceHdr2sdrMobiusTransition.Value.ToInvariantString("0.00")}"
                        If ColorspaceHdr2sdrMobiusPeak.Value <> ColorspaceHdr2sdrMobiusPeak.DefaultValue Then ret += $",peak={ColorspaceHdr2sdrMobiusPeak.Value.ToInvariantString("0.00")}"
                    End If
                    If ColorspaceHdr2sdr.Value = 3 Then
                        If ColorspaceHdr2sdrReinhardContrast.Value <> ColorspaceHdr2sdrReinhardContrast.DefaultValue Then ret += $",contrast={ColorspaceHdr2sdrReinhardContrast.Value.ToInvariantString("0.00")}"
                        If ColorspaceHdr2sdrReinhardPeak.Value <> ColorspaceHdr2sdrReinhardPeak.DefaultValue Then ret += $",peak={ColorspaceHdr2sdrReinhardPeak.Value.ToInvariantString("0.00")}"
                    End If
                End If
                If ret <> "" Then Return "--vpp-colorspace " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetNgxTruehdrArgs() As String
            If NgxTruehdr.Value Then
                Dim ret = ""
                If NgxTruehdrContrast.Value <> NgxTruehdrContrast.DefaultValue Then ret += ",contrast=" & NgxTruehdrContrast.Value.ToInvariantString()
                If NgxTruehdrSaturation.Value <> NgxTruehdrSaturation.DefaultValue Then ret += ",saturation=" & NgxTruehdrSaturation.Value.ToInvariantString()
                If NgxTruehdrMiddleGray.Value <> NgxTruehdrMiddleGray.DefaultValue Then ret += ",middlegray=" & NgxTruehdrMiddleGray.Value.ToInvariantString()
                If NgxTruehdrMaxLuminance.Value <> NgxTruehdrMaxLuminance.DefaultValue Then ret += ",maxluminance=" & NgxTruehdrMaxLuminance.Value.ToInvariantString()
                Return "--vpp-ngx-truehdr " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function ImportNgxTruehdrArgs(param As String, arg As String) As String
            Dim contrastMatch = Regex.Match(arg, "(?:^|,)contrast=(\d+)?")
            Dim saturationMatch = Regex.Match(arg, "(?:^|,)saturation=(\d+)?")
            Dim middlegrayMatch = Regex.Match(arg, "(?:^|,)middlegray=(\d+)?")
            Dim maxluminanceMatch = Regex.Match(arg, "(?:^|,)maxluminance=(\d+)?")

            NgxTruehdr.Value = True
            NgxTruehdrContrast.Value = NgxTruehdrContrast.DefaultValue
            NgxTruehdrSaturation.Value = NgxTruehdrSaturation.DefaultValue
            NgxTruehdrMiddleGray.Value = NgxTruehdrMiddleGray.DefaultValue
            NgxTruehdrMaxLuminance.Value = NgxTruehdrMaxLuminance.DefaultValue

            If contrastMatch.Success Then NgxTruehdrContrast.Value = contrastMatch.Groups(1).Value.ToDouble(NgxTruehdrContrast.DefaultValue)
            If saturationMatch.Success Then NgxTruehdrSaturation.Value = saturationMatch.Groups(1).Value.ToDouble(NgxTruehdrSaturation.DefaultValue)
            If middlegrayMatch.Success Then NgxTruehdrMiddleGray.Value = middlegrayMatch.Groups(1).Value.ToDouble(NgxTruehdrMiddleGray.DefaultValue)
            If maxluminanceMatch.Success Then NgxTruehdrMaxLuminance.Value = maxluminanceMatch.Groups(1).Value.ToDouble(NgxTruehdrMaxLuminance.DefaultValue)
        End Function

        Function GetLibPlaceboDebandArgs() As String
            If LibPlaceboDeband.Value Then
                Dim ret = ""
                If LibPlaceboDebandIterations.Value <> LibPlaceboDebandIterations.DefaultValue Then ret += ",iterations=" & LibPlaceboDebandIterations.Value.ToInvariantString()
                If LibPlaceboDebandThreshold.Value <> LibPlaceboDebandThreshold.DefaultValue Then ret += ",threshold=" & LibPlaceboDebandThreshold.Value.ToInvariantString()
                If LibPlaceboDebandRadius.Value <> LibPlaceboDebandRadius.DefaultValue Then ret += ",radius=" & LibPlaceboDebandRadius.Value.ToInvariantString()
                If LibPlaceboDebandGrainY.Value <> LibPlaceboDebandGrainY.DefaultValue Then ret += ",grain_y=" & LibPlaceboDebandGrainY.Value.ToInvariantString()
                If LibPlaceboDebandGrainC.Value <> LibPlaceboDebandGrainC.DefaultValue Then ret += ",grain_c=" & LibPlaceboDebandGrainC.Value.ToInvariantString()
                If LibPlaceboDebandDither.Value <> LibPlaceboDebandDither.DefaultValue Then ret += ",dither=" & LibPlaceboDebandDither.ValueText.ToInvariantString()
                If LibPlaceboDebandLutSize.Value <> LibPlaceboDebandLutSize.DefaultValue Then ret += ",lut_size=" & LibPlaceboDebandLutSize.ValueText.ToInvariantString()
                Return "--vpp-libplacebo-deband " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function ImportLibPlaceboDebandArgs(param As String, arg As String) As String
            Dim match1 = Regex.Match(arg, "(?:^|,)iterations=(\d+(\.\d+)?)?")
            Dim match2 = Regex.Match(arg, "(?:^|,)threshold=(\d+(\.\d+)?)?")
            Dim match3 = Regex.Match(arg, "(?:^|,)radius=(\d+(\.\d+)?)?")
            Dim match4 = Regex.Match(arg, "(?:^|,)grain_y=(\d+(\.\d+)?)?")
            Dim match5 = Regex.Match(arg, "(?:^|,)grain_c=(\d+(\.\d+)?)?")
            Dim match6 = Regex.Match(arg, "(?:^|,)dither=(\d+(\.\d+)?)?")
            Dim match7 = Regex.Match(arg, "(?:^|,)lut_size=(\d+(\.\d+)?)?")

            LibPlaceboDeband.Value = True
            LibPlaceboDebandIterations.Value = LibPlaceboDebandIterations.DefaultValue
            LibPlaceboDebandThreshold.Value = LibPlaceboDebandThreshold.DefaultValue
            LibPlaceboDebandRadius.Value = LibPlaceboDebandRadius.DefaultValue
            LibPlaceboDebandGrainY.Value = LibPlaceboDebandGrainY.DefaultValue
            LibPlaceboDebandGrainC.Value = LibPlaceboDebandGrainC.DefaultValue
            LibPlaceboDebandDither.Value = LibPlaceboDebandDither.DefaultValue
            LibPlaceboDebandLutSize.Value = LibPlaceboDebandLutSize.DefaultValue

            If match1.Success Then LibPlaceboDebandIterations.Value = match1.Groups(1).Value.ToDouble(LibPlaceboDebandIterations.DefaultValue)
            If match2.Success Then LibPlaceboDebandThreshold.Value = match2.Groups(1).Value.ToDouble(LibPlaceboDebandThreshold.DefaultValue)
            If match3.Success Then LibPlaceboDebandRadius.Value = match3.Groups(1).Value.ToDouble(LibPlaceboDebandRadius.DefaultValue)
            If match4.Success Then LibPlaceboDebandGrainY.Value = match4.Groups(1).Value.ToDouble(LibPlaceboDebandGrainY.DefaultValue)
            If match5.Success Then LibPlaceboDebandGrainC.Value = match5.Groups(1).Value.ToDouble(LibPlaceboDebandGrainC.DefaultValue)
            If match6.Success AndAlso Array.IndexOf(LibPlaceboDebandDither.Values, match6.Groups(1).Value.ToLowerInvariant()) > -1 Then LibPlaceboDebandDither.Value = Array.IndexOf(LibPlaceboDebandDither.Values, match6.Groups(1).Value.ToLowerInvariant())
            If match7.Success AndAlso Array.IndexOf(LibPlaceboDebandLutSize.Values, match7.Groups(1).Value.ToLowerInvariant()) > -1 Then LibPlaceboDebandLutSize.Value = Array.IndexOf(LibPlaceboDebandLutSize.Values, match7.Groups(1).Value.ToLowerInvariant())
        End Function

        Function GetLibPlaceboShaderArgs() As String
            If LibPlaceboShader.Value Then
                Dim sb = New StringBuilder()
                If Not String.IsNullOrWhiteSpace(LibPlaceboShaderShader.Value) Then sb.Append($",shader={LibPlaceboShaderShader.Value}")
                If Not LibPlaceboShaderResWidth.IsDefaultValue OrElse Not LibPlaceboShaderResHeight.IsDefaultValue Then sb.Append($",res={LibPlaceboShaderResWidth.Value.ToInvariantString()}x{LibPlaceboShaderResHeight.Value.ToInvariantString()}")
                If Not LibPlaceboShaderColorsystem.IsDefaultValue Then sb.Append($",colorsystem={LibPlaceboShaderColorsystem.ValueText.ToInvariantString()}")
                If Not LibPlaceboShaderTransfer.IsDefaultValue Then sb.Append($",transfer={LibPlaceboShaderTransfer.ValueText.ToInvariantString()}")
                If Not LibPlaceboShaderResampler.IsDefaultValue Then sb.Append($",resampler={LibPlaceboShaderResampler.ValueText.ToInvariantString()}")
                If Not LibPlaceboShaderRadius.IsDefaultValue Then sb.Append($",radius={LibPlaceboShaderRadiusValue.Value.ToInvariantString()}")
                If Not LibPlaceboShaderClamp.IsDefaultValue Then sb.Append($",clamp={LibPlaceboShaderClamp.Value.ToInvariantString()}")
                If Not LibPlaceboShaderTaper.IsDefaultValue Then sb.Append($",taper={LibPlaceboShaderTaper.Value.ToInvariantString()}")
                If Not LibPlaceboShaderBlur.IsDefaultValue Then sb.Append($",blur={LibPlaceboShaderBlur.Value.ToInvariantString()}")
                If Not LibPlaceboShaderAntiring.IsDefaultValue Then sb.Append($",antiring={LibPlaceboShaderAntiring.Value.ToInvariantString()}")
                If Not LibPlaceboShaderLinear.IsDefaultValue Then sb.Append($",linear={LibPlaceboShaderLinear.ValueText.ToInvariantString()}")
                Return $"{LibPlaceboShader.Switch} {sb.ToString().TrimStart(","c)}"
            End If
            Return ""
        End Function

        Function ImportLibPlaceboShaderArgs(param As String, arg As String) As String
            LibPlaceboShader.Value = True

            Dim setDouble = Sub(numParam As NumParam, match As Match)
                                If numParam Is Nothing Then Return
                                numParam.SetDefaultValue()
                                If match Is Nothing Then Return
                                If Not match.Success Then Return
                                numParam.Value = match.Groups(1).Value.ToDouble(numParam.DefaultValue)
                            End Sub

            Dim setOption = Sub(optionParam As OptionParam, match As Match)
                                If optionParam Is Nothing Then Return
                                optionParam.SetDefaultValue()
                                If match Is Nothing Then Return
                                If Not match.Success Then Return
                                Dim index = Array.IndexOf(optionParam.Values, match.Groups(1).Value.ToLowerInvariant())
                                If index > -1 Then optionParam.Value = index
                            End Sub

            Dim setString = Sub(stringParam As StringParam, match As Match)
                                If stringParam Is Nothing Then Return
                                stringParam.SetDefaultValue()
                                If match Is Nothing Then Return
                                If Not match.Success Then Return
                                stringParam.Value = match.Groups(1).Value
                            End Sub

            setString(LibPlaceboShaderShader, Regex.Match(arg, "(?:^|,)shader=([^,]*)"))
            setOption(LibPlaceboShaderColorsystem, Regex.Match(arg, "(?:^|,)colorsystem=([^,]*)"))
            setOption(LibPlaceboShaderTransfer, Regex.Match(arg, "(?:^|,)transfer=([^,]*)"))
            setOption(LibPlaceboShaderResampler, Regex.Match(arg, "(?:^|,)resampler=([^,]*)"))
            setDouble(LibPlaceboShaderClamp, Regex.Match(arg, "(?:^|,)clamp=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboShaderTaper, Regex.Match(arg, "(?:^|,)taper=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboShaderBlur, Regex.Match(arg, "(?:^|,)blur=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboShaderAntiring, Regex.Match(arg, "(?:^|,)antiring=(\d+(\.\d+)?)?"))
            setOption(LibPlaceboShaderLinear, Regex.Match(arg, "(?:^|,)linear=([^,]*)"))

            Dim m = Regex.Match(arg, "(?:^|,)res=(\d+)x(\d+)")
            LibPlaceboShaderResWidth.SetDefaultValue()
            LibPlaceboShaderResHeight.SetDefaultValue()
            If m.Success Then
                LibPlaceboShaderResWidth.Value = m.Groups(1).Value.ToDouble(LibPlaceboShaderResWidth.DefaultValue)
                LibPlaceboShaderResHeight.Value = m.Groups(2).Value.ToDouble(LibPlaceboShaderResHeight.DefaultValue)
            End If

            m = Regex.Match(arg, "(?:^|,)radius=([^,]*)")
            If m.Success Then
                LibPlaceboShaderRadius.Value = 1
                setDouble(LibPlaceboShaderRadiusValue, m)
            End If
        End Function

        Function GetLibPlaceboTonemappingArgs() As String
            If LibPlaceboTonemapping.Value Then
                Dim sb = New StringBuilder()
                If Not LibPlaceboTonemappingSrcCsp.IsDefaultValue Then sb.Append($",src_csp={LibPlaceboTonemappingSrcCsp.ValueText.ToInvariantString()}")
                If Not LibPlaceboTonemappingDstCsp.IsDefaultValue Then sb.Append($",dst_csp={LibPlaceboTonemappingDstCsp.ValueText.ToInvariantString()}")
                If Not (LibPlaceboTonemappingDstPlTransfer.IsDefaultValue OrElse LibPlaceboTonemappingDstPlColorprim.IsDefaultValue) Then sb.Append($",dst_pl_transfer={LibPlaceboTonemappingDstPlTransfer.ValueText.ToInvariantString()},dst_pl_colorprim={LibPlaceboTonemappingDstPlColorprim.ValueText.ToInvariantString()}")
                If Not LibPlaceboTonemappingSrcMax.IsDefaultValue Then sb.Append($",src_max={LibPlaceboTonemappingSrcMax.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingSrcMin.IsDefaultValue Then sb.Append($",src_min={LibPlaceboTonemappingSrcMin.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingDstMax.IsDefaultValue Then sb.Append($",dst_max={LibPlaceboTonemappingDstMax.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingDstMin.IsDefaultValue Then sb.Append($",dst_min={LibPlaceboTonemappingDstMin.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingDynPeakDetect.IsDefaultValue Then sb.Append($",dynamic_peak_detection={LibPlaceboTonemappingDynPeakDetect.ValueText.ToInvariantString()}")
                If Not LibPlaceboTonemappingSmoothPeriod.IsDefaultValue Then sb.Append($",smooth_period={LibPlaceboTonemappingSmoothPeriod.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingSceneThresholdLow.IsDefaultValue Then sb.Append($",scene_threshold_low={LibPlaceboTonemappingSceneThresholdLow.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingSceneThresholdHigh.IsDefaultValue Then sb.Append($",scene_threshold_high={LibPlaceboTonemappingSceneThresholdHigh.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingPercentile.IsDefaultValue Then sb.Append($",percentile={LibPlaceboTonemappingPercentile.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingBlackCutoff.IsDefaultValue Then sb.Append($",black_cutoff={LibPlaceboTonemappingBlackCutoff.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingGamutMapping.IsDefaultValue Then sb.Append($",gamut_mapping={LibPlaceboTonemappingGamutMapping.ValueText.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunction.IsDefaultValue Then sb.Append($",tonemapping_function={LibPlaceboTonemappingFunction.ValueText.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionKneeAdaption.IsDefaultValue AndAlso LibPlaceboTonemappingFunctionKneeAdaption.Visible Then sb.Append($",knee_adaptation={LibPlaceboTonemappingFunctionKneeAdaption.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionKneeMax.IsDefaultValue AndAlso LibPlaceboTonemappingFunctionKneeMax.Visible Then sb.Append($",knee_max={LibPlaceboTonemappingFunctionKneeMax.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionKneeMin.IsDefaultValue AndAlso LibPlaceboTonemappingFunctionKneeMin.Visible Then sb.Append($",knee_min={LibPlaceboTonemappingFunctionKneeMin.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionKneeDefault.IsDefaultValue AndAlso LibPlaceboTonemappingFunctionKneeDefault.Visible Then sb.Append($",knee_default={LibPlaceboTonemappingFunctionKneeDefault.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionKneeOffset.IsDefaultValue AndAlso LibPlaceboTonemappingFunctionKneeOffset.Visible Then sb.Append($",knee_offset={LibPlaceboTonemappingFunctionKneeOffset.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionSlopeTuning.IsDefaultValue AndAlso LibPlaceboTonemappingFunctionSlopeTuning.Visible Then sb.Append($",slope_tuning={LibPlaceboTonemappingFunctionSlopeTuning.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionSlopeOffset.IsDefaultValue AndAlso LibPlaceboTonemappingFunctionSlopeOffset.Visible Then sb.Append($",slope_offset={LibPlaceboTonemappingFunctionSlopeOffset.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionSplineContrast.IsDefaultValue AndAlso LibPlaceboTonemappingFunctionSplineContrast.Visible Then sb.Append($",spline_contrast={LibPlaceboTonemappingFunctionSplineContrast.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionReinhardContrast.IsDefaultValue AndAlso LibPlaceboTonemappingFunctionReinhardContrast.Visible Then sb.Append($",reinhard_contrast={LibPlaceboTonemappingFunctionReinhardContrast.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionLinearKnee.IsDefaultValue AndAlso LibPlaceboTonemappingFunctionLinearKnee.Visible Then sb.Append($",linear_knee={LibPlaceboTonemappingFunctionLinearKnee.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionExposure.IsDefaultValue AndAlso LibPlaceboTonemappingFunctionExposure.Visible Then sb.Append($",exposure={LibPlaceboTonemappingFunctionExposure.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionMetadata.IsDefaultValue Then sb.Append($",metadata={LibPlaceboTonemappingFunctionMetadata.ValueText.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionContrastRecovery.IsDefaultValue Then sb.Append($",contrast_recovery={LibPlaceboTonemappingFunctionContrastRecovery.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionContrastSmoothness.IsDefaultValue Then sb.Append($",contrast_smoothness={LibPlaceboTonemappingFunctionContrastSmoothness.Value.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionUseDolbyVision.IsDefaultValue Then sb.Append($",use_dovi={LibPlaceboTonemappingFunctionUseDolbyVision.ValueText.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionInverseToneMapping.IsDefaultValue Then sb.Append($",inverse_tone_mapping={LibPlaceboTonemappingFunctionInverseToneMapping.ValueText.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionShowClipping.IsDefaultValue Then sb.Append($",show_clipping={LibPlaceboTonemappingFunctionShowClipping.ValueText.ToInvariantString()}")
                If Not LibPlaceboTonemappingFunctionVisualizeLut.IsDefaultValue Then sb.Append($",visualize_lut={LibPlaceboTonemappingFunctionVisualizeLut.ValueText.ToInvariantString()}")
                If Not (LibPlaceboTonemappingFunctionLutType.IsDefaultValue OrElse String.IsNullOrWhiteSpace(LibPlaceboTonemappingLut.Value)) Then sb.Append($",lut_type={LibPlaceboTonemappingFunctionLutType.ValueText.ToInvariantString()}")
                If Not String.IsNullOrWhiteSpace(LibPlaceboTonemappingLut.Value) Then sb.Append($" {LibPlaceboTonemappingLut.HelpSwitch} {LibPlaceboTonemappingLut.Value.Escape()}")
                Return $"{LibPlaceboTonemapping.Switch} {sb.ToString().TrimStart(","c)}"
            End If
            Return ""
        End Function

        Function ImportLibPlaceboTonemappingArgs(param As String, arg As String) As String
            LibPlaceboTonemapping.Value = True

            Dim setDouble = Sub(numParam As NumParam, match As Match)
                                If numParam Is Nothing Then Return
                                If match Is Nothing Then Return
                                If Not match.Success Then Return
                                numParam.SetDefaultValue()
                                numParam.Value = match.Groups(1).Value.ToDouble(numParam.DefaultValue)
                            End Sub

            Dim setOption = Sub(optionParam As OptionParam, match As Match)
                                If optionParam Is Nothing Then Return
                                If match Is Nothing Then Return
                                If Not match.Success Then Return
                                optionParam.SetDefaultValue()
                                Dim index = Array.IndexOf(optionParam.Values, match.Groups(1).Value.ToLowerInvariant())
                                If index > -1 Then optionParam.Value = index
                            End Sub

            setOption(LibPlaceboTonemappingSrcCsp, Regex.Match(arg, "(?:^|,)src_csp=([^,]*)"))
            setOption(LibPlaceboTonemappingDstCsp, Regex.Match(arg, "(?:^|,)dst_csp=([^,]*)"))
            setOption(LibPlaceboTonemappingDstPlTransfer, Regex.Match(arg, "(?:^|,)dst_pl_transfer=([^,]*)"))
            setOption(LibPlaceboTonemappingDstPlColorprim, Regex.Match(arg, "(?:^|,)dst_pl_colorprim=([^,]*)"))
            setDouble(LibPlaceboTonemappingSrcMax, Regex.Match(arg, "(?:^|,)src_max=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingSrcMin, Regex.Match(arg, "(?:^|,)src_min=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingDstMax, Regex.Match(arg, "(?:^|,)dst_max=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingDstMin, Regex.Match(arg, "(?:^|,)dst_min=(\d+(\.\d+)?)?"))
            setOption(LibPlaceboTonemappingDynPeakDetect, Regex.Match(arg, "(?:^|,)dynamic_peak_detection=([^,]*)"))
            setDouble(LibPlaceboTonemappingSmoothPeriod, Regex.Match(arg, "(?:^|,)smooth_period=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingSceneThresholdLow, Regex.Match(arg, "(?:^|,)scene_threshold_low=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingSceneThresholdHigh, Regex.Match(arg, "(?:^|,)scene_threshold_high=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingPercentile, Regex.Match(arg, "(?:^|,)percentile=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingBlackCutoff, Regex.Match(arg, "(?:^|,)black_cutoff=(\d+(\.\d+)?)?"))
            setOption(LibPlaceboTonemappingGamutMapping, Regex.Match(arg, "(?:^|,)gamut_mapping=([^,]*)"))
            setOption(LibPlaceboTonemappingFunction, Regex.Match(arg, "(?:^|,)tonemapping_function=([^,]*)"))
            setDouble(LibPlaceboTonemappingFunctionKneeAdaption, Regex.Match(arg, "(?:^|,)knee_adaptation=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingFunctionKneeMax, Regex.Match(arg, "(?:^|,)knee_max=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingFunctionKneeMin, Regex.Match(arg, "(?:^|,)knee_min=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingFunctionKneeDefault, Regex.Match(arg, "(?:^|,)knee_default=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingFunctionKneeOffset, Regex.Match(arg, "(?:^|,)knee_offset=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingFunctionSlopeTuning, Regex.Match(arg, "(?:^|,)slope_tuning=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingFunctionSlopeOffset, Regex.Match(arg, "(?:^|,)slope_offset=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingFunctionSplineContrast, Regex.Match(arg, "(?:^|,)spline_contrast=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingFunctionReinhardContrast, Regex.Match(arg, "(?:^|,)reinhard_contrast=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingFunctionLinearKnee, Regex.Match(arg, "(?:^|,)linear_knee=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingFunctionExposure, Regex.Match(arg, "(?:^|,)exposure=(\d+(\.\d+)?)?"))
            setOption(LibPlaceboTonemappingFunctionMetadata, Regex.Match(arg, "(?:^|,)metadata=([^,]*)"))
            setDouble(LibPlaceboTonemappingFunctionContrastRecovery, Regex.Match(arg, "(?:^|,)contrast_recovery=(\d+(\.\d+)?)?"))
            setDouble(LibPlaceboTonemappingFunctionContrastSmoothness, Regex.Match(arg, "(?:^|,)contrast_smoothness=(\d+(\.\d+)?)?"))
            setOption(LibPlaceboTonemappingFunctionUseDolbyVision, Regex.Match(arg, "(?:^|,)use_dovi=([^,]*)"))
            setOption(LibPlaceboTonemappingFunctionInverseToneMapping, Regex.Match(arg, "(?:^|,)inverse_tone_mapping=([^,]*)"))
            setOption(LibPlaceboTonemappingFunctionShowClipping, Regex.Match(arg, "(?:^|,)show_clipping=([^,]*)"))
            setOption(LibPlaceboTonemappingFunctionVisualizeLut, Regex.Match(arg, "(?:^|,)visualize_lut=([^,]*)"))
            setOption(LibPlaceboTonemappingFunctionLutType, Regex.Match(arg, "(?:^|,)lut_type=([^,]*)"))
        End Function

        Function GetPmdArgs() As String
            If Pmd.Value Then
                Dim ret = ""
                If PmdApplyCount.Value <> PmdApplyCount.DefaultValue Then ret += ",apply_count=" & PmdApplyCount.Value
                If PmdStrength.Value <> PmdStrength.DefaultValue Then ret += ",strength=" & PmdStrength.Value
                If PmdThreshold.Value <> PmdThreshold.DefaultValue Then ret += ",threshold=" & PmdThreshold.Value
                Return "--vpp-pmd " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetTweakArgs() As String
            If Tweak.Value Then
                Dim ret = ""
                If TweakBrightness.Value <> TweakBrightness.DefaultValue Then ret += ",brightness=" & TweakBrightness.Value.ToInvariantString
                If TweakContrast.Value <> TweakContrast.DefaultValue Then ret += ",contrast=" & TweakContrast.Value.ToInvariantString
                If TweakSaturation.Value <> TweakSaturation.DefaultValue Then ret += ",saturation=" & TweakSaturation.Value.ToInvariantString
                If TweakGamma.Value <> TweakGamma.DefaultValue Then ret += ",gamma=" & TweakGamma.Value.ToInvariantString
                If TweakHue.Value <> TweakHue.DefaultValue Then ret += ",hue=" & TweakHue.Value.ToInvariantString
                Return "--vpp-tweak " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetPaddingArgs() As String
            Return If(Pad.Value, $"--vpp-pad {PadLeft.Value},{PadTop.Value},{PadRight.Value},{PadBottom.Value}", "")
        End Function

        Function GetFft3dArgs() As String
            If Fft3d.Value Then
                Dim ret = ""
                If Fft3dSigma.Value <> Fft3dSigma.DefaultValue Then ret += ",sigma=" & Fft3dSigma.Value.ToInvariantString
                If Fft3dAmount.Value <> Fft3dAmount.DefaultValue Then ret += ",amount=" & Fft3dAmount.Value.ToInvariantString
                If Fft3dBlockSize.Value <> Fft3dBlockSize.DefaultValue Then ret += ",block_size=" & Fft3dBlockSize.ValueText
                If Fft3dOverlap.Value <> Fft3dOverlap.DefaultValue Then ret += ",overlap=" & Fft3dOverlap.Value.ToInvariantString
                If Fft3dMethod.Value <> Fft3dMethod.DefaultValue Then ret += ",method=" & Fft3dMethod.ValueText
                If Fft3dTemporal.Value <> Fft3dTemporal.DefaultValue Then ret += ",temporal=" & Fft3dTemporal.ValueText
                If Fft3dPrec.Value <> Fft3dPrec.DefaultValue Then ret += ",prec=" & Fft3dPrec.ValueText
                Return "--vpp-fft3d " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetKnnArgs() As String
            If Knn.Value Then
                Dim ret = ""
                If KnnRadius.Value <> KnnRadius.DefaultValue Then ret += ",radius=" & KnnRadius.Value
                If KnnStrength.Value <> KnnStrength.DefaultValue Then ret += ",strength=" & KnnStrength.Value.ToInvariantString
                If KnnLerp.Value <> KnnLerp.DefaultValue Then ret += ",lerp=" & KnnLerp.Value.ToInvariantString
                If KnnThLerp.Value <> KnnThLerp.DefaultValue Then ret += ",th_lerp=" & KnnThLerp.Value.ToInvariantString
                Return "--vpp-knn " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetConvolution3dArgs() As String
            If Convolution.Value Then
                Dim ret = ""
                If ConvolutionMatrix.Value <> ConvolutionMatrix.DefaultValue Then ret += ",matrix=" & ConvolutionMatrix.ValueText
                If ConvolutionFast.Value <> ConvolutionFast.DefaultValue Then ret += ",fast=" & ConvolutionFast.ValueText
                If ConvolutionYthresh.Value <> ConvolutionYthresh.DefaultValue Then ret += ",ythresh=" & ConvolutionYthresh.Value.ToInvariantString
                If ConvolutionCthresh.Value <> ConvolutionCthresh.DefaultValue Then ret += ",cthresh=" & ConvolutionCthresh.Value.ToInvariantString
                If ConvolutionTYthresh.Value <> ConvolutionTYthresh.DefaultValue Then ret += ",t_ythresh=" & ConvolutionTYthresh.Value.ToInvariantString
                If ConvolutionTCthresh.Value <> ConvolutionTCthresh.DefaultValue Then ret += ",t_cthresh=" & ConvolutionTCthresh.Value.ToInvariantString
                Return "--vpp-convolution3d " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetDebandArgs() As String
            If Deband.Value Then
                Dim ret = ""
                If DebandRange.Value <> DebandRange.DefaultValue Then ret += ",range=" & DebandRange.Value
                If DebandSample.Value <> DebandSample.DefaultValue Then ret += ",sample=" & DebandSample.Value
                If DebandThre.Value <> DebandThre.DefaultValue Then ret += ",thre=" & DebandThre.Value
                If DebandThreY.Value <> DebandThreY.DefaultValue Then ret += ",thre_y=" & DebandThreY.Value
                If DebandThreCB.Value <> DebandThreCB.DefaultValue Then ret += ",thre_cb=" & DebandThreCB.Value
                If DebandThreCR.Value <> DebandThreCR.DefaultValue Then ret += ",thre_cr=" & DebandThreCR.Value
                If DebandDither.Value <> DebandDither.DefaultValue Then ret += ",dither=" & DebandDither.Value
                If DebandDitherY.Value <> DebandDitherY.DefaultValue Then ret += ",dither_y=" & DebandDitherY.Value
                If DebandDitherC.Value <> DebandDitherC.DefaultValue Then ret += ",dither_c=" & DebandDitherC.Value
                If DebandSeed.Value <> DebandSeed.DefaultValue Then ret += ",seed=" & DebandSeed.Value
                If DebandBlurfirst.Value Then ret += ",blurfirst"
                If DebandRandEachFrame.Value Then ret += ",rand_each_frame"
                Return "--vpp-deband " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetEdge() As String
            If Edgelevel.Value Then
                Dim ret = ""
                If EdgelevelStrength.Value <> EdgelevelStrength.DefaultValue Then ret += ",strength=" & EdgelevelStrength.Value.ToInvariantString
                If EdgelevelThreshold.Value <> EdgelevelThreshold.DefaultValue Then ret += ",threshold=" & EdgelevelThreshold.Value.ToInvariantString
                If EdgelevelBlack.Value <> EdgelevelBlack.DefaultValue Then ret += ",black=" & EdgelevelBlack.Value.ToInvariantString
                If EdgelevelWhite.Value <> EdgelevelWhite.DefaultValue Then ret += ",white=" & EdgelevelWhite.Value.ToInvariantString
                Return "--vpp-edgelevel " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetUnsharp() As String
            If Unsharp.Value Then
                Dim ret = ""
                If UnsharpRadius.Value <> UnsharpRadius.DefaultValue Then ret += ",radius=" & UnsharpRadius.Value.ToInvariantString
                If UnsharpWeight.Value <> UnsharpWeight.DefaultValue Then ret += ",weight=" & UnsharpWeight.Value.ToInvariantString
                If UnsharpThreshold.Value <> UnsharpThreshold.DefaultValue Then ret += ",threshold=" & UnsharpThreshold.Value.ToInvariantString
                Return "--vpp-unsharp " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetWarpsharpArgs() As String
            If Warpsharp.Value Then
                Dim ret = ""
                If WarpsharpThreshold.Value <> WarpsharpThreshold.DefaultValue Then ret += ",threshold=" & WarpsharpThreshold.Value.ToInvariantString
                If WarpsharpBlur.Value <> WarpsharpBlur.DefaultValue Then ret += ",blur=" & WarpsharpBlur.Value.ToInvariantString
                If WarpsharpType.Value <> WarpsharpType.DefaultValue Then ret += ",type=" & WarpsharpType.Value.ToInvariantString
                If WarpsharpDepth.Value <> WarpsharpDepth.DefaultValue Then ret += ",depth=" & WarpsharpDepth.Value.ToInvariantString
                If WarpsharpChroma.Value <> WarpsharpChroma.DefaultValue Then ret += ",chroma=" & WarpsharpChroma.Value.ToInvariantString
                Return "--vpp-warpsharp " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetTransform() As String
            Dim ret = ""
            If TransformFlipX.Value Then ret += ",flip_x=true"
            If TransformFlipY.Value Then ret += ",flip_y=true"
            If TransformTranspose.Value Then ret += ",transpose=true"
            If ret <> "" Then Return ("--vpp-transform " + ret.TrimStart(","c))
            Return ""
        End Function

        Function GetSelectEvery() As String
            If SelectEvery.Value Then
                Dim ret = ""
                ret += SelectEveryValue.Value.ToString
                ret += "," + SelectEveryOffsets.Value.SplitNoEmptyAndWhiteSpace(" ", ",", ";").Select(Function(item) "offset=" + item).Join(",")
                If ret <> "" Then Return "--vpp-select-every " + ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetResizeArgs() As String
            If Resize.Value Then
                Dim ret = ""
                If ResizeAlgo.Value <> ResizeAlgo.DefaultValue Then ret += ",algo=" & ResizeAlgo.ValueText.ToInvariantString
                If ResizeSuperresMode.Visible AndAlso ResizeSuperresMode.Value <> ResizeSuperresMode.DefaultValue Then ret += ",superres-mode=" & ResizeSuperresMode.Value.ToInvariantString
                If ResizeSuperresStrength.Visible AndAlso ResizeSuperresStrength.Value <> ResizeSuperresStrength.DefaultValue Then ret += ",superres-strength=" & ResizeSuperresStrength.Value.ToInvariantString
                If ResizeVsrQuality.Visible AndAlso ResizeVsrQuality.Value <> ResizeVsrQuality.DefaultValue Then ret += ",vsr-quality=" & ResizeVsrQuality.Value.ToInvariantString
                If ResizeLibplaceboFilter.Visible Then
                    ret += "-" & ResizeLibplaceboFilter.ValueText.ToInvariantString
                    If ResizeLibplaceboPlRadius.Value >= 0D Then ret += ",pl-radius=" & ResizeLibplaceboPlRadius.Value.ToInvariantString
                    If ResizeLibplaceboPlClamp.Value <> ResizeLibplaceboPlClamp.DefaultValue Then ret += ",pl-clamp=" & ResizeLibplaceboPlClamp.Value.ToInvariantString
                    If ResizeLibplaceboPlTaper.Value <> ResizeLibplaceboPlTaper.DefaultValue Then ret += ",pl-taper=" & ResizeLibplaceboPlTaper.Value.ToInvariantString
                    If ResizeLibplaceboPlBlur.Value <> ResizeLibplaceboPlBlur.DefaultValue Then ret += ",pl-blur=" & ResizeLibplaceboPlBlur.Value.ToInvariantString
                    If ResizeLibplaceboPlAntiring.Value <> ResizeLibplaceboPlAntiring.DefaultValue Then ret += ",pl-antiring=" & ResizeLibplaceboPlAntiring.Value.ToInvariantString
                End If
                Return Resize.Switch & " " & ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetNvvfxDenoiseArgs() As String
            If NvvfxDenoise.Value Then
                Dim ret = ""
                ret += ",strength=" & NvvfxDenoiseStrength.Value.ToInvariantString
                Return NvvfxDenoise.Switch & " " & ret.TrimStart(","c)
            End If
            Return ""
        End Function

        Function GetNvvfxArtifactReductionArgs() As String
            If NvvfxArtifactReduction.Value Then
                Dim ret = ""
                ret += ",mode=" & NvvfxArtifactReductionMode.Value.ToInvariantString
                Return NvvfxArtifactReduction.Switch & " " & ret.TrimStart(","c)
            End If
            Return ""
        End Function


        Function GetDeinterlacerArgs() As String
            Dim ret = ""
            Select Case Deinterlacer.Value
                Case 2
                    If AfsPreset.Value <> AfsPreset.DefaultValue Then ret += ",preset=" + AfsPreset.ValueText
                    If AfsINI.Value <> "" Then ret += ",ini=" + AfsINI.Value.Escape
                    If AfsLeft.Value <> AfsLeft.DefaultValue Then ret += ",left=" & AfsLeft.Value
                    If AfsRight.Value <> AfsRight.DefaultValue Then ret += ",right=" & AfsRight.Value
                    If AfsTop.Value <> AfsTop.DefaultValue Then ret += ",top=" & AfsTop.Value
                    If AfsBottom.Value <> AfsBottom.DefaultValue Then ret += ",bottom=" & AfsBottom.Value
                    If AfsMethodSwitch.Value <> AfsMethodSwitch.DefaultValue Then ret += ",method_switch=" & AfsMethodSwitch.Value
                    If AfsCoeffShift.Value <> AfsCoeffShift.DefaultValue Then ret += ",coeff_shift=" & AfsCoeffShift.Value
                    If AfsThreShift.Value <> AfsThreShift.DefaultValue Then ret += ",thre_shift=" & AfsThreShift.Value
                    If AfsThreDeint.Value <> AfsThreDeint.DefaultValue Then ret += ",thre_deint=" & AfsThreDeint.Value
                    If AfsThreMotionY.Value <> AfsThreMotionY.DefaultValue Then ret += ",thre_motion_y=" & AfsThreMotionY.Value
                    If AfsThreMotionC.Value <> AfsThreMotionC.DefaultValue Then ret += ",thre_motion_c=" & AfsThreMotionC.Value
                    If AfsLevel.Value <> AfsLevel.DefaultValue Then ret += ",level=" & AfsLevel.Value
                    If AfsShift.Value <> AfsShift.DefaultValue Then ret += ",shift=" + If(AfsShift.Value, "on", "off")
                    If AfsDrop.Value <> AfsDrop.DefaultValue Then ret += ",drop=" + If(AfsDrop.Value, "on", "off")
                    If AfsSmooth.Value <> AfsSmooth.DefaultValue Then ret += ",smooth=" + If(AfsSmooth.Value, "on", "off")
                    If Afs24fps.Value <> Afs24fps.DefaultValue Then ret += ",24fps=" + If(Afs24fps.Value, "on", "off")
                    If AfsTune.Value <> AfsTune.DefaultValue Then ret += ",tune=" + If(AfsTune.Value, "on", "off")
                    If AfsRFF.Value <> AfsRFF.DefaultValue Then ret += ",rff=" + If(AfsRFF.Value, "on", "off")
                    If AfsTimecode.Value <> AfsTimecode.DefaultValue Then ret += ",timecode=" + If(AfsTimecode.Value, "on", "off")
                    If AfsLog.Value <> AfsLog.DefaultValue Then ret += ",log=" + If(AfsLog.Value, "on", "off")
                    Return "--vpp-afs " + ret.TrimStart(","c)
                Case 3
                    If DecombFull.Value <> DecombFull.DefaultValue Then ret += ",full=" & DecombFull.Value.ToInvariantString
                    If DecombThreshold.Value <> DecombThreshold.DefaultValue Then ret += ",threshold=" & DecombThreshold.Value.ToInvariantString
                    If DecombDThreshold.Value <> DecombDThreshold.DefaultValue Then ret += ",dthreshold=" & DecombDThreshold.Value.ToInvariantString
                    If DecombBlend.Value <> DecombBlend.DefaultValue Then ret += ",blend=" & DecombBlend.Value.ToInvariantString
                    Return "--vpp-decomb " + ret.TrimStart(","c)
                Case 4
                    If NnediField.Value <> NnediField.DefaultValue Then ret += ",field=" + NnediField.ValueText
                    If NnediNns.Value <> NnediNns.DefaultValue Then ret += ",nns=" + NnediNns.ValueText
                    If NnediNsize.Value <> NnediNsize.DefaultValue Then ret += ",nsize=" + NnediNsize.ValueText
                    If NnediQuality.Value <> NnediQuality.DefaultValue Then ret += ",quality=" + NnediQuality.ValueText
                    If NnediPrescreen.Value <> NnediPrescreen.DefaultValue Then ret += ",prescreen=" + NnediPrescreen.ValueText
                    If NnediErrortype.Value <> NnediErrortype.DefaultValue Then ret += ",errortype=" + NnediErrortype.ValueText
                    If NnediPrec.Value <> NnediPrec.DefaultValue Then ret += ",prec=" + NnediPrec.ValueText
                    If NnediWeightfile.Value <> "" Then ret += ",weightfile=" + NnediWeightfile.Value.Escape
                    Return "--vpp-nnedi " + ret.TrimStart(","c)
                Case Else
                    Return ret
            End Select
        End Function

        Function GetModeArgs() As String
            Select Case Mode.Value
                Case 0
                    Return "--qvbr " & QVBR.Value.ToInvariantString()
                Case 1
                    If Codec.Value = 2 Then Return If(QPAdvanced.Value, $"--cqp {QPIAV1.Value.ToInvariantString()}:{QPPAV1.Value.ToInvariantString()}:{QPBAV1.Value.ToInvariantString()}", $" --cqp {QPAV1.Value.ToInvariantString()}")
                    Return If(QPAdvanced.Value, $"--cqp {QPI.Value.ToInvariantString()}:{QPP.Value.ToInvariantString()}:{QPB.Value.ToInvariantString()}", $" --cqp {QP.Value.ToInvariantString()}")
                Case 2
                    Return "--cbr " & Bitrate.Value.ToInvariantString()
                Case 3
                    Return "--vbr " & Bitrate.Value.ToInvariantString()
            End Select
            Return ""
        End Function

        Function GetQpInitArgs() As String
            If QPInit8I.Visible Then Return $"{QPInit8.HelpSwitch} {QPInit8I.Value}:{QPInit8P.Value}:{QPInit8B.Value}"
            If QPInit10I.Visible Then Return $"{QPInit10.HelpSwitch} {QPInit10I.Value}:{QPInit10P.Value}:{QPInit10B.Value}"
            If QPInitAV1I.Visible Then Return $"{QPInitAV1.HelpSwitch} {QPInitAV1I.Value}:{QPInitAV1P.Value}:{QPInitAV1B.Value}"
        End Function

        Sub ImportQpInitArgs(param As String, arg As String)
            Dim match = Regex.Match(arg, "(\d+)(?::(\d+):(\d+))?")

            If match.Success Then
                Dim isAdvanced = match.Groups(2).Success AndAlso match.Groups(3).Success

                If isAdvanced Then
                    QPInitAdvanced.Value = True
                    QPInit8I.Value = match.Groups(1).Value.ToInt()
                    QPInit10I.Value = match.Groups(1).Value.ToInt()
                    QPInitAV1I.Value = match.Groups(1).Value.ToInt()
                    QPInit8P.Value = match.Groups(2).Value.ToInt()
                    QPInit10P.Value = match.Groups(2).Value.ToInt()
                    QPInitAV1P.Value = match.Groups(2).Value.ToInt()
                    QPInit8B.Value = match.Groups(3).Value.ToInt()
                    QPInit10B.Value = match.Groups(3).Value.ToInt()
                    QPInitAV1B.Value = match.Groups(3).Value.ToInt()
                Else
                    QPInitAdvanced.Value = False
                    QPInit8.Value = match.Groups(1).Value.ToInt()
                    QPInit10.Value = match.Groups(1).Value.ToInt()
                    QPInitAV1.Value = match.Groups(1).Value.ToInt()
                End If
            End If
        End Sub

        Function GetQpMaxArgs() As String
            If QPMax8I.Visible AndAlso Not {QPMax8I.Value, QPMax8P.Value, QPMax8B.Value}.Min() = QPMax8I.Config(1) Then Return $"{QPMax8.HelpSwitch} {QPMax8I.Value}:{QPMax8P.Value}:{QPMax8B.Value}"
            If QPMax10I.Visible AndAlso Not {QPMax10I.Value, QPMax10P.Value, QPMax10B.Value}.Min() = QPMax10I.Config(1) Then Return $"{QPMax10.HelpSwitch} {QPMax10I.Value}:{QPMax10P.Value}:{QPMax10B.Value}"
            If QPMaxAV1I.Visible AndAlso Not {QPMaxAV1I.Value, QPMaxAV1P.Value, QPMaxAV1B.Value}.Min() = QPMaxAV1I.Config(1) Then Return $"{QPMaxAV1.HelpSwitch} {QPMaxAV1I.Value}:{QPMaxAV1P.Value}:{QPMaxAV1B.Value}"
        End Function

        Sub ImportQpMaxArgs(param As String, arg As String)
            Dim match = Regex.Match(arg, "(\d+)(?::(\d+):(\d+))?")

            If match.Success Then
                Dim isAdvanced = match.Groups(2).Success AndAlso match.Groups(3).Success

                If isAdvanced Then
                    QPMaxAdvanced.Value = True
                    QPMax8I.Value = match.Groups(1).Value.ToInt()
                    QPMax10I.Value = match.Groups(1).Value.ToInt()
                    QPMaxAV1I.Value = match.Groups(1).Value.ToInt()
                    QPMax8P.Value = match.Groups(2).Value.ToInt()
                    QPMax10P.Value = match.Groups(2).Value.ToInt()
                    QPMaxAV1P.Value = match.Groups(2).Value.ToInt()
                    QPMax8B.Value = match.Groups(3).Value.ToInt()
                    QPMax10B.Value = match.Groups(3).Value.ToInt()
                    QPMaxAV1B.Value = match.Groups(3).Value.ToInt()
                Else
                    QPMaxAdvanced.Value = False
                    QPMax8.Value = match.Groups(1).Value.ToInt()
                    QPMax10.Value = match.Groups(1).Value.ToInt()
                    QPMaxAV1.Value = match.Groups(1).Value.ToInt()
                End If
            End If
        End Sub

        Function GetQpMinArgs() As String
            If QPMin8I.Visible AndAlso Not {QPMin8I.Value, QPMin8P.Value, QPMin8B.Value}.Max() = QPMin8I.Config(0) Then Return $"{QPMin8.HelpSwitch} {QPMin8I.Value}:{QPMin8P.Value}:{QPMin8B.Value}"
            If QPMin10I.Visible AndAlso Not {QPMin10I.Value, QPMin10P.Value, QPMin10B.Value}.Max() = QPMin10I.Config(0) Then Return $"{QPMin10.HelpSwitch} {QPMin10I.Value}:{QPMin10P.Value}:{QPMin10B.Value}"
            If QPMinAV1I.Visible AndAlso Not {QPMinAV1I.Value, QPMinAV1P.Value, QPMinAV1B.Value}.Max() = QPMinAV1I.Config(0) Then Return $"{QPMinAV1.HelpSwitch} {QPMinAV1I.Value}:{QPMinAV1P.Value}:{QPMinAV1B.Value}"
        End Function

        Sub ImportQpMinArgs(param As String, arg As String)
            Dim match = Regex.Match(arg, "(\d+)(?::(\d+):(\d+))?")

            If match.Success Then
                Dim isAdvanced = match.Groups(2).Success AndAlso match.Groups(3).Success

                If isAdvanced Then
                    QPMinAdvanced.Value = True
                    QPMin8I.Value = match.Groups(1).Value.ToInt()
                    QPMin10I.Value = match.Groups(1).Value.ToInt()
                    QPMinAV1I.Value = match.Groups(1).Value.ToInt()
                    QPMin8P.Value = match.Groups(2).Value.ToInt()
                    QPMin10P.Value = match.Groups(2).Value.ToInt()
                    QPMinAV1P.Value = match.Groups(2).Value.ToInt()
                    QPMin8B.Value = match.Groups(3).Value.ToInt()
                    QPMin10B.Value = match.Groups(3).Value.ToInt()
                    QPMinAV1B.Value = match.Groups(3).Value.ToInt()
                Else
                    QPMinAdvanced.Value = False
                    QPMin8.Value = match.Groups(1).Value.ToInt()
                    QPMin10.Value = match.Groups(1).Value.ToInt()
                    QPMinAV1.Value = match.Groups(1).Value.ToInt()
                End If
            End If
        End Sub

        Overrides Function GetCommandLine(includePaths As Boolean, includeExe As Boolean, Optional pass As Integer = 1) As String
            Dim ret As String = ""
            Dim sourcePath As String = ""
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)

            If includePaths AndAlso includeExe Then
                ret = Package.NVEncC.Path.Escape
            End If

            Select Case Decoder.ValueText
                Case "avs"
                    sourcePath = p.Script.Path

                    If includePaths AndAlso FrameServerHelp.IsPortable Then
                        ret += " --avsdll " + Package.AviSynth.Path.Escape
                    End If
                Case "avhw"
                    sourcePath = p.LastOriginalSourceFile
                    ret += " --avhw"
                Case "avsw"
                    sourcePath = p.LastOriginalSourceFile
                    ret += " --avsw"
                Case "qs"
                    sourcePath = "-"

                    If includePaths Then
                        ret = If(includePaths, Package.QSVEncC.Path.Escape, "QSVEncC64") + " -o - -c raw" + " -i " + If(includePaths, p.SourceFile.Escape, "path") + " | " + If(includePaths, Package.NVEncC.Path.Escape, "NVEncC64")
                    End If
                Case "ffdxva"
                    sourcePath = "-"

                    If includePaths Then
                        Dim pix_fmt = If(p.SourceVideoBitDepth = 10, "yuv420p10le", "yuv420p")
                        ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") +
                            " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.SourceFile.Escape, "path") +
                            " -f yuv4mpegpipe -pix_fmt " + pix_fmt + " -strict -1 -loglevel fatal -hide_banner - | " +
                            If(includePaths, Package.NVEncC.Path.Escape, "NVEncC64")
                    End If
                Case "ffqsv"
                    sourcePath = "-"

                    If includePaths Then
                        ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.SourceFile.Escape, "path") + " -f yuv4mpegpipe -strict -1 -pix_fmt yuv420p -loglevel fatal -hide_banner - | " + If(includePaths, Package.NVEncC.Path.Escape, "NVEncC64")
                    End If
            End Select

            Dim q = From i In Items Where i.GetArgs <> "" AndAlso Not IsCustom(i.Switch)

            If q.Count > 0 Then
                ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")
            End If

            If (p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 AndAlso
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
                    ret += " --trim " + p.Ranges.Select(Function(range) $"{range.Start}:{range.End}").Join(",")
                End If
            End If

            If sourcePath = "-" Then
                ret += " --y4m"
            End If

            If ret.Contains("%") Then
                ret = Macro.Expand(ret)
            End If

            If includePaths Then
                ret += " -i " + sourcePath.Escape + " -o " + targetPath.Escape
            End If

            Return ret.Trim
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
    End Class
End Class
