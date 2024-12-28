
Imports System.Text.RegularExpressions
Imports System.Xml.Serialization
Imports MS.Internal.Text.TextInterface
Imports StaxRip.UI
Imports StaxRip.VideoEncoderCommandLine

<Serializable()>
Public Class VCEEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "VCEEncC (AMD) | " + Params.Codec.OptionText.Replace("AMD ", "")
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

    Overrides Property Bitrate As Integer
        Get
            Return CInt(Params.Bitrate.Value)
        End Get
        Set(value As Integer)
            Params.Bitrate.Value = value
        End Set
    End Property

    'Overrides ReadOnly Property IsOvercroppingAllowed As Boolean
    '    Get
    '        If Not Params.DolbyVisionRpu.Visible Then Return True
    '        Return String.IsNullOrWhiteSpace(Params.GetStringParam(Params.DolbyVisionRpu.Switch)?.Value)
    '    End Get
    'End Property

    'Overrides ReadOnly Property IsUnequalResizingAllowed As Boolean
    '    Get
    '        If Not Params.DolbyVisionRpu.Visible Then Return True
    '        Return String.IsNullOrWhiteSpace(Params.GetStringParam(Params.DolbyVisionRpu.Switch)?.Value)
    '    End Get
    'End Property

    'Overrides ReadOnly Property DolbyVisionMetadataPath As String
    '    Get
    '        If Not Params.DolbyVisionRpu.Visible Then Return Nothing
    '        Return Params.GetStringParam(Params.DolbyVisionRpu.Switch)?.Value
    '    End Get
    'End Property

    'Overrides ReadOnly Property Hdr10PlusMetadataPath As String
    '    Get
    '        If Not Params.DhdrInfo.Visible Then Return Nothing
    '        Return Params.GetStringParam(Params.DhdrInfo.Switch)?.Value
    '    End Get
    'End Property

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(codecIndex As Integer)
        MyBase.New()
        Params.Codec.Value = If(codecIndex > 0 AndAlso codecIndex < Params.Codec.Values.Length, codecIndex, 0)
    End Sub

    Overrides Sub ShowConfigDialog(Optional path As String = Nothing)
        Dim params1 As New EncoderParams
        Dim store = ObjectHelp.GetCopy(ParamsStore)
        params1.Init(store)

        Using form As New CommandLineForm(params1)
            Dim a = Sub()
                        Dim enc = ObjectHelp.GetCopy(Me)
                        Dim params2 As New EncoderParams
                        Dim store2 = ObjectHelp.GetCopy(store)
                        params2.Init(store2)
                        enc.Params = params2
                        enc.ParamsStore = store2
                        SaveProfile(enc)
                    End Sub

            form.cms.Add("Check Hardware", Sub() g.ShowCode("Check Hardware", ProcessHelp.GetConsoleOutput(Package.VCEEncC.Path, "--check-hw")))
            form.cms.Add("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetConsoleOutput(Package.VCEEncC.Path, "--check-features")), Keys.Control Or Keys.F)
            form.cms.Add("-")
            form.cms.Add("Save Profile...", a, Keys.Control Or Keys.S, Symbol.Save)

            If Not String.IsNullOrWhiteSpace(path) Then
                form.SimpleUI.ShowPage(path)
            End If

            If form.ShowDialog() = DialogResult.OK Then
                Params = params1
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

    Overrides ReadOnly Property OutputExt() As String
        Get
            If Params.Codec.ValueText = "av1" Then
                Return Muxer.OutputExt
            End If

            Return Params.Codec.ValueText
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()

        Using proc As New Proc
            proc.Header = "Video encoding"
            proc.Package = Package.VCEEncC
            proc.SkipStrings = {"%]", " frames: "}
            proc.File = "cmd.exe"
            proc.Arguments = "/S /C """ + Params.GetCommandLine(True, True) + """"
            proc.Start()
        End Using
    End Sub

    'Overrides Function BeforeEncoding() As Boolean
    '    Dim rpu = Params.GetStringParam("--dolby-vision-rpu")?.Value
    '    If p.Script.IsFilterActive("Crop") AndAlso Not String.IsNullOrWhiteSpace(rpu) AndAlso rpu = p.HdrDolbyVisionMetadataFile?.Path AndAlso rpu.FileExists() Then
    '        If (p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 Then
    '            p.HdrDolbyVisionMetadataFile.WriteEditorConfigFile(New Padding(p.CropLeft, p.CropTop, p.CropRight, p.CropBottom), True)
    '            Dim newPath = p.HdrDolbyVisionMetadataFile.WriteCroppedRpu(True)
    '            If Not String.IsNullOrWhiteSpace(newPath) Then
    '                Params.DolbyVisionRpu.Value = newPath
    '            Else
    '                Return False
    '            End If
    '        End If
    '    End If
    '    Return True
    'End Function

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
            cl += " --output-depth 10"
            cl += $" --dolby-vision-rpu ""{p.HdrDolbyVisionMetadataFile.Path}"""

            Select Case p.HdrDolbyVisionMode
                Case DoviMode.Untouched, DoviMode.Mode0, DoviMode.Mode1
                    cl += $""
                Case DoviMode.Mode4
                    cl += $" --dolby-vision-profile 8.4"
                Case Else
                    cl += $" --dolby-vision-profile 8.1"
            End Select
        End If

        Dim MaxCLL = MediaInfo.GetVideo(sourceFile, "MaxCLL").Trim.Left(" ").ToInt
        Dim MaxFALL = MediaInfo.GetVideo(sourceFile, "MaxFALL").Trim.Left(" ").ToInt

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
            audio-filter device input-analyze audio-file sub-copy version audio-copy
            audio-ignore-decode-error audio-ignore-notrack-error audio-resampler raw help input-file
            audio-samplerate audio-source audio-stream avs avvce-analyze output-file seek skip-frame
            check-avversion check-codecs check-decoders check-encoders check-filters check-protocols
            check-formats dar format fps input-res log-framelist mux-option
            vpp-delogo vpp-delogo-cb vpp-delogo-cr vpp-delogo-depth output
            vpp-delogo-pos vpp-delogo-select vpp-delogo-y"

        tester.Package = Package.VCEEncC
        tester.CodeFile = Path.Combine(Folder.Startup.Parent, "Encoding", "vceenc.vb")

        Return tester.Test
    End Function

    Public Class EncoderParams
        Inherits CommandLineParams

        Sub New()
            Title = "VCEEncC Options"
        End Sub

        Property Codec As New OptionParam With {
            .Switch = "--codec",
            .Text = "Codec",
            .Options = {"H.264", "H.265", "AV1"},
            .Values = {"h264", "hevc", "av1"}}

        Property Mode As New OptionParam With {
            .Name = "Mode",
            .Text = "Mode",
            .Options = {"CQP - Constant QP", "CBR - Constant Bitrate", "CBRHQ - High Quality Constant Bitrate", "VBR - Variable Bitrate", "VBRHQ - High Quality Variable Bitrate", "QVBR - Quality-Defined Variable Bitrate"}}

        Property OutputDepth As New OptionParam With {
            .Switch = "--output-depth",
            .Text = "Depth",
            .Options = {"8-Bit", "10-Bit"},
            .Values = {"8", "10"}}

        Property Decoder As New OptionParam With {
            .Text = "Decoder",
            .Options = {"AviSynth/VapourSynth", "VCEEnc (Hardware)", "VCEEnc (Software)", "QSVEnc (Intel)", "ffmpeg (Intel)", "ffmpeg (DXVA2)"},
            .Values = {"avs", "avhw", "avsw", "qs", "ffqsv", "ffdxva"}}

        Property Interlace As New OptionParam With {
            .Text = "Interlace",
            .Switch = "--interlace",
            .Options = {"Disabled", "Top Field First", "Bottom Field First", "Auto"},
            .Values = {"", "tff", "bff", "auto"}}

        Property Bitrate As New NumParam With {
            .HelpSwitch = "--bitrate",
            .Text = "Bitrate",
            .Init = 5000,
            .VisibleFunc = Function() Mode.Value > 0,
            .Config = {0, 1000000, 100}}

        Property QvbrQuality As New NumParam With {
            .Switch = "--qvbr-quality",
            .Text = "QVBR Quality",
            .Init = 28,
            .VisibleFunc = Function() Mode.Value = 5,
            .Config = {0, 51, 1}}

        Property QPAdvanced As New BoolParam With {
            .Text = "Show advanced QP settings",
            .VisibleFunc = Function() Mode.Value = 0}

        Property QP As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP",
            .Init = 18,
            .VisibleFunc = Function() Mode.Value = 0 AndAlso Codec.Value <> 2 AndAlso Not (QPAdvanced.Value AndAlso QPAdvanced.Visible),
            .Config = {0, 63}}

        Property QPAV1 As New NumParam With {
            .Switches = {"--cqp"},
            .Text = "QP",
            .Init = 50,
            .VisibleFunc = Function() Mode.Value = 0 AndAlso Codec.Value = 2 AndAlso Not (QPAdvanced.Value AndAlso QPAdvanced.Visible),
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


        Property DolbyVisionProfile As New OptionParam With {
            .Switch = "--dolby-vision-profile",
            .Text = "Dolby Vision Profile",
            .Options = {"Undefined", "5", "8.1", "8.2", "8.4"}}

        Property DolbyVisionRpu As New StringParam With {
            .Switch = "--dolby-vision-rpu",
            .Text = "Dolby Vision RPU",
            .BrowseFile = True}

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


        Property Tiles As New NumParam With {.Switch = "--tiles", .Text = "Tiles", .Init = 0, .Config = {0, 64}, .VisibleFunc = Function() Codec.Value = 2}
        Property CdefMode As New OptionParam With {.Switch = "--cdef-mode", .Text = "CDEF Mode", .Init = 0, .Options = {"Off", "On"}, .Values = {"off", "on"}, .VisibleFunc = Function() Codec.Value = 2}
        Property CdfUpdate As New BoolParam With {.Switch = "--cdf-update", .Text = "CDF Update", .Init = False, .VisibleFunc = Function() Codec.Value = 2}
        Property CdfFrameEndUpdate As New BoolParam With {.Switch = "--cdf-frame-end-update", .Text = "CDF Frame End Update", .Init = False, .VisibleFunc = Function() Codec.Value = 2}
        Property TemporalLayers As New NumParam With {.Switch = "--temporal-layers", .Text = "Temporal Layers", .Value = 0, .Config = {0, 1000}, .VisibleFunc = Function() Codec.Value = 2}
        Property AqMode As New OptionParam With {.Switch = "--aq-mode", .Text = "AQ Mode", .Init = 0, .Options = {"None", "CAQ"}, .Values = {"none", "caq"}, .VisibleFunc = Function() Codec.Value = 2}

        Property Pa As New BoolParam With {.Switch = "--pa", .Text = "Pre-Analysis to enhance quality", .VisibleFunc = Function() Mode.Value > 0 AndAlso OutputDepth.Value = 0, .ArgsFunc = AddressOf GetPaArgs}
        Property PaSc As New OptionParam With {.Text = "      Sensitivity of scene change detection", .HelpSwitch = "--pa", .Init = 2, .Options = {"None", "Low", "Medium", "High"}, .VisibleFunc = Function() Pa.Visible}
        Property PaSs As New OptionParam With {.Text = "      Sensitivity of static scene detection", .HelpSwitch = "--pa", .Init = 3, .Options = {"None", "Low", "Medium", "High"}, .VisibleFunc = Function() Pa.Visible}
        Property PaActivityType As New OptionParam With {.Text = "      Block activity calcualtion mode", .HelpSwitch = "--pa", .Init = 0, .Options = {"Y", "YUV"}, .VisibleFunc = Function() Pa.Visible}
        Property PaCaqStrength As New OptionParam With {.Text = "      Content Adaptive Quantization (CAQ) strength", .HelpSwitch = "--pa", .Init = 1, .Options = {"Low", "Medium", "High"}, .VisibleFunc = Function() Pa.Visible}
        Property PaInitqpsc As New NumParam With {.Text = "      Initial qp after scene change", .HelpSwitch = "--pa", .Init = -1.0, .Config = {-1.0, 100.0, 1, 0}, .VisibleFunc = Function() Pa.Visible}
        Property PaFskipMaxqp As New NumParam With {.Text = "      Threshold to insert skip frame on static scene", .HelpSwitch = "--pa", .Init = 35.0, .Config = {0.0, 100.0, 1, 0}, .VisibleFunc = Function() Pa.Visible}
        Property PaLookahead As New NumParam With {.Text = "      Lookahead buffer size", .HelpSwitch = "--pa", .Init = 0, .Config = {0.0, 100.0, 1, 0}, .VisibleFunc = Function() Pa.Visible}
        Property PaLtr As New OptionParam With {.Text = "      Enable automatic LTR frame management", .HelpSwitch = "--pa", .IntegerValue = False, .Init = 0, .Options = {"False", "True"}, .Values = {"false", "true"}, .VisibleFunc = Function() Pa.Visible}
        Property PaPaq As New OptionParam With {.Text = "      Perceptual AQ mode", .HelpSwitch = "--pa", .IntegerValue = False, .Init = 0, .Options = {"None", "CAQ"}, .Values = {"none", "caq"}, .VisibleFunc = Function() Pa.Visible}
        Property PaTaq As New OptionParam With {.Text = "      Temporal AQ mode", .HelpSwitch = "--pa", .IntegerValue = True, .Init = 0, .Options = {"0", "1", "2"}, .VisibleFunc = Function() Pa.Visible}
        Property PaMotionQuality As New OptionParam With {.Text = "      High motion quality boost mode", .HelpSwitch = "--pa", .IntegerValue = False, .Init = 0, .Options = {"None", "Auto"}, .Values = {"none", "auto"}, .VisibleFunc = Function() Pa.Visible}


        Property Tweak As New BoolParam With {.Switch = "--vpp-tweak", .Text = "Tweaking", .ArgsFunc = AddressOf GetTweakArgs}
        Property TweakContrast As New NumParam With {.Text = "      Contrast", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {-2.0, 2.0, 0.1, 1}}
        Property TweakGamma As New NumParam With {.Text = "      Gamma", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {0.1, 10.0, 0.1, 1}}
        Property TweakSaturation As New NumParam With {.Text = "      Saturation", .HelpSwitch = "--vpp-tweak", .Init = 1.0, .Config = {0.0, 3.0, 0.1, 1}}
        Property TweakHue As New NumParam With {.Text = "      Hue", .HelpSwitch = "--vpp-tweak", .Config = {-180.0, 180.0, 0.1, 1}}
        Property TweakBrightness As New NumParam With {.Text = "      Brightness", .HelpSwitch = "--vpp-tweak", .Config = {-1.0, 1.0, 0.1, 1}}
        Property TweakSwapuv As New OptionParam With {.Text = "      Swapuv", .HelpSwitch = "--vpp-tweak", .IntegerValue = False, .Init = 0, .Options = {"False", "True"}, .Values = {"false", "true"}}

        Property Pad As New BoolParam With {.Switch = "--vpp-pad", .Text = "Padding", .ArgsFunc = AddressOf GetPaddingArgs}
        Property PadLeft As New NumParam With {.Text = "      Left"}
        Property PadTop As New NumParam With {.Text = "      Top"}
        Property PadRight As New NumParam With {.Text = "      Right"}
        Property PadBottom As New NumParam With {.Text = "      Bottom"}

        Property Smooth As New BoolParam With {.Text = "Smooth", .Switch = "--vpp-smooth", .ArgsFunc = AddressOf GetSmoothArgs}
        Property SmoothQuality As New NumParam With {.Text = "      Quality", .HelpSwitch = "--vpp-smooth", .Init = 3, .Config = {1, 6}}
        Property SmoothQP As New NumParam With {.Text = "      QP", .HelpSwitch = "--vpp-smooth", .Config = {0, 100, 10, 1}}
        Property SmoothPrec As New OptionParam With {.Text = "      Precision", .HelpSwitch = "--vpp-smooth", .Options = {"Auto", "FP16", "FP32"}}

        Property TransformFlipX As New BoolParam With {.Switch = "--vpp-transform", .Text = "Flip X", .Label = "Transform", .LeftMargin = g.MainForm.FontHeight * 1.5, .ArgsFunc = AddressOf GetTransform}
        Property TransformFlipY As New BoolParam With {.Text = "Flip Y", .LeftMargin = g.MainForm.FontHeight * 1.5, .HelpSwitch = "--vpp-transform"}
        Property TransformTranspose As New BoolParam With {.Text = "Transpose", .LeftMargin = g.MainForm.FontHeight * 1.5, .HelpSwitch = "--vpp-transform"}

        Property Decomb As New BoolParam With {.Text = "Decomb", .Switch = "--vpp-decomb", .ArgsFunc = AddressOf GetDecombArgs}
        Property DecombFull As New OptionParam With {.Text = "      Full", .HelpSwitch = "--vpp-decomb", .Init = 1, .Options = {"Off", "On (Default)"}, .Values = {"off", "on"}}
        Property DecombThreshold As New NumParam With {.Text = "      Threshold", .HelpSwitch = "--vpp-decomb", .Init = 20, .Config = {0, 255, 1, 0}}
        Property DecombDThreshold As New NumParam With {.Text = "      DThreshold", .HelpSwitch = "--vpp-decomb", .Init = 7, .Config = {0, 255, 1, 0}}
        Property DecombBlend As New OptionParam With {.Text = "      Blend", .HelpSwitch = "--vpp-decomb", .Init = 0, .Options = {"Off (Default)", "On"}, .Values = {"off", "on"}}

        Property DenoiseDct As New BoolParam With {.Text = "Denoise DCT", .Switch = "--vpp-denoise-dct", .ArgsFunc = AddressOf GetDenoiseDctArgs}
        Property DenoiseDctStep As New OptionParam With {.Text = "      Step", .HelpSwitch = "--vpp-denoise-dct", .Init = 1, .Options = {"1 (high quality, slow)", "2 (default)", "4", "8 (fast)"}, .Values = {"1", "2", "4", "8"}}
        Property DenoiseDctSigma As New NumParam With {.Text = "      Sigma", .HelpSwitch = "--vpp-denoise-dct", .Init = 4, .Config = {0, 100, 0.1, 1}}
        Property DenoiseDctBlockSize As New OptionParam With {.Text = "      Block Size", .HelpSwitch = "--vpp-denoise-dct", .Options = {"8 (default)", "16 (slow)"}, .Values = {"8", "16"}}

        Property Fft3d As New BoolParam With {.Text = "FFT3D", .Switch = "--vpp-fft3d", .ArgsFunc = AddressOf GetFft3dArgs}
        Property Fft3dSigma As New NumParam With {.Text = "      Sigma", .HelpSwitch = "--vpp-fft3d", .Init = 1, .Config = {0, 100, 0.5, 1}}
        Property Fft3dAmount As New NumParam With {.Text = "      Amount", .HelpSwitch = "--vpp-fft3d", .Init = 1, .Config = {0, 1, 0.01, 2}}
        Property Fft3dBlockSize As New OptionParam With {.Text = "      Block Size", .HelpSwitch = "--vpp-fft3d", .Expanded = True, .Init = 2, .Options = {"8", "16", "32 (default)", "64"}, .Values = {"8", "16", "32", "64"}}
        Property Fft3dOverlap As New NumParam With {.Text = "      Overlap", .HelpSwitch = "--vpp-fft3d", .Init = 0.5, .Config = {0.2, 0.8, 0.01, 2}}
        Property Fft3dMethod As New OptionParam With {.Text = "      Method", .HelpSwitch = "--vpp-fft3d", .Expanded = True, .Init = 0, .Options = {"Wiener Method (default)", "Hard Thresholding"}, .Values = {"0", "1"}}
        Property Fft3dTemporal As New OptionParam With {.Text = "      Temporal", .HelpSwitch = "--vpp-fft3d", .Init = 1, .Options = {"Spatial Filtering Only", "Temporal Filtering (default)"}, .Values = {"0", "1"}}
        Property Fft3dPrec As New OptionParam With {.Text = "      Prec", .HelpSwitch = "--vpp-fft3d", .Init = 0, .Options = {"Use fp16 if possible (faster, default)", "Always use fp32"}, .Values = {"auto", "fp32"}}

        Property Nlmeans As New BoolParam With {.Text = "Nlmeans", .Switch = "--vpp-nlmeans", .ArgsFunc = AddressOf GetNlmeansArgs}
        Property NlmeansSigma As New NumParam With {.Text = "     Sigma", .HelpSwitch = "--vpp-nlmeans", .Init = 0.005, .Config = {0, 10, 0.001, 3}}
        Property NlmeansH As New NumParam With {.Text = "     H", .HelpSwitch = "--vpp-nlmeans", .Init = 0.05, .Config = {0, 10, 0.01, 2}}
        Property NlmeansPatch As New NumParam With {.Text = "     Patch", .HelpSwitch = "--vpp-nlmeans", .Init = 5, .Config = {3, 200, 1, 0}}
        Property NlmeansSearch As New NumParam With {.Text = "     Search", .HelpSwitch = "--vpp-nlmeans", .Init = 11, .Config = {3, 200, 1, 0}}
        Property NlmeansFp16 As New OptionParam With {.Text = "     Fp16", .HelpSwitch = "--vpp-nlmeans", .Init = 1, .Options = {"None: Do not use fp16 and use fp32. High precision but slow.", "Blockdiff: Use fp16 in block diff calculation. Balanced. (Default)", "All: Additionally use fp16 in weight calculation. Fast but low precision."}, .Values = {"none", "blockdiff", "all"}}

        Property Colorspace As New BoolParam With {.Text = "Colorspace", .Switch = "--vpp-colorspace", .ArgsFunc = AddressOf GetColorspaceArgs}
        Property ColorspaceMatrixFrom As New OptionParam With {.Text = New String(" "c, 6) + "Matrix From", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"Undefined", "auto", "bt709", "smpte170m", "bt470bg", "smpte240m", "YCgCo", "fcc", "GBR", "bt2020nc", "bt2020c"}}
        Property ColorspaceMatrixTo As New OptionParam With {.Text = New String(" "c, 12) + "Matrix To", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"auto", "bt709", "smpte170m", "bt470bg", "smpte240m", "YCgCo", "fcc", "GBR", "bt2020nc", "bt2020c"}, .VisibleFunc = Function() ColorspaceMatrixFrom.Value > 0}
        Property ColorspaceColorprimFrom As New OptionParam With {.Text = New String(" "c, 6) + "Colorprim From", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"Undefined", "auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "film", "bt2020"}}
        Property ColorspaceColorprimTo As New OptionParam With {.Text = New String(" "c, 12) + "Colorprim To", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "film", "bt2020"}, .VisibleFunc = Function() ColorspaceColorprimFrom.Value > 0}
        Property ColorspaceTransferFrom As New OptionParam With {.Text = New String(" "c, 6) + "Transfer From", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"Undefined", "auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "linear", "log100", "log316", "iec61966-2-4", "iec61966-2-1", "bt2020-10", "bt2020-12", "smpte2084", "arib-std-b67"}}
        Property ColorspaceTransferTo As New OptionParam With {.Text = New String(" "c, 12) + "Transfer To", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"auto", "bt709", "smpte170m", "bt470m", "bt470bg", "smpte240m", "linear", "log100", "log316", "iec61966-2-4", "iec61966-2-1", "bt2020-10", "bt2020-12", "smpte2084", "arib-std-b67"}, .VisibleFunc = Function() ColorspaceTransferFrom.Value > 0}
        Property ColorspaceRangeFrom As New OptionParam With {.Text = New String(" "c, 6) + "Range From", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"Undefined", "auto", "limited", "full"}}
        Property ColorspaceRangeTo As New OptionParam With {.Text = New String(" "c, 12) + "Range To", .HelpSwitch = "--vpp-colorspace", .Init = 0, .Options = {"auto", "limited", "full"}, .VisibleFunc = Function() ColorspaceRangeFrom.Value > 0}
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

        Property Deinterlacer As New OptionParam With {.Text = "Deinterlacing Method", .HelpSwitch = "", .Init = 0, .Options = {"None", "AFS (Activate Auto Field Shift)", "Nnedi", "Yadif"}, .ArgsFunc = AddressOf GetDeinterlacerArgs}

        Property AfsPreset As New OptionParam With {.Text = "Preset", .HelpSwitch = "--vpp-afs", .Options = {"Default", "Triple", "Double", "Anime", "Cinema", "Min_afterimg", "24fps", "24fps_sd", "30fps"}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsINI As New StringParam With {.Text = "INI", .HelpSwitch = "--vpp-afs", .BrowseFile = True, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsLeft As New NumParam With {.Text = "Left", .HelpSwitch = "--vpp-afs", .Init = 32, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsRight As New NumParam With {.Text = "Right", .HelpSwitch = "--vpp-afs", .Init = 32, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsTop As New NumParam With {.Text = "Top", .HelpSwitch = "--vpp-afs", .Init = 16, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsBottom As New NumParam With {.Text = "Bottom", .HelpSwitch = "--vpp-afs", .Init = 16, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsMethodSwitch As New NumParam With {.Text = "Method Switch", .HelpSwitch = "--vpp-afs", .Config = {0, 256}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsCoeffShift As New NumParam With {.Text = "Coeff Shift", .HelpSwitch = "--vpp-afs", .Init = 192, .Config = {0, 256}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsThreShift As New NumParam With {.Text = "Threshold Shift", .HelpSwitch = "--vpp-afs", .Init = 128, .Config = {0, 1024}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsThreDeint As New NumParam With {.Text = "Threshold Deint", .HelpSwitch = "--vpp-afs", .Init = 48, .Config = {0, 1024}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsThreMotionY As New NumParam With {.Text = "Threshold Motion Y", .HelpSwitch = "--vpp-afs", .Init = 112, .Config = {0, 1024}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsThreMotionC As New NumParam With {.Text = "Threshold Motion C", .HelpSwitch = "--vpp-afs", .Init = 224, .Config = {0, 1024}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsLevel As New NumParam With {.Text = "Level", .HelpSwitch = "--vpp-afs", .Init = 3, .Config = {0, 4}, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsShift As New BoolParam With {.Text = "Shift", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsDrop As New BoolParam With {.Text = "Drop", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsSmooth As New BoolParam With {.Text = "Smooth", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property Afs24fps As New BoolParam With {.Text = "24 FPS", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsTune As New BoolParam With {.Text = "Tune", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsRFF As New BoolParam With {.Text = "RFF", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsTimecode As New BoolParam With {.Text = "Timecode", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}
        Property AfsLog As New BoolParam With {.Text = "Log", .HelpSwitch = "--vpp-afs", .LeftMargin = g.MainForm.FontHeight * 1.3, .VisibleFunc = Function() Deinterlacer.Value = 1}

        Property NnediField As New OptionParam With {.Text = "Field", .HelpSwitch = "--vpp-nnedi", .Options = {"auto", "top", "bottom"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property NnediNns As New OptionParam With {.Text = "NNS", .HelpSwitch = "--vpp-nnedi", .Init = 1, .Options = {"16", "32", "64", "128", "256"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property NnediNsize As New OptionParam With {.Text = "N Size", .HelpSwitch = "--vpp-nnedi", .Init = 6, .Options = {"8x6", "16x6", "32x6", "48x6", "8x4", "16x4", "32x4"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property NnediQuality As New OptionParam With {.Text = "Quality", .HelpSwitch = "--vpp-nnedi", .Options = {"fast", "slow"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property NnediPrescreen As New OptionParam With {.Text = "Pre Screen", .HelpSwitch = "--vpp-nnedi", .Init = 4, .Options = {"none", "original", "new", "original_block", "new_block"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property NnediErrortype As New OptionParam With {.Text = "Error Type", .HelpSwitch = "--vpp-nnedi", .Options = {"abs", "square"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property NnediPrec As New OptionParam With {.Text = "Prec", .HelpSwitch = "--vpp-nnedi", .Options = {"auto", "fp16", "fp32"}, .VisibleFunc = Function() Deinterlacer.Value = 2}
        Property NnediWeightfile As New StringParam With {.Text = "Weight File", .HelpSwitch = "--vpp-nnedi", .BrowseFile = True, .VisibleFunc = Function() Deinterlacer.Value = 2}

        Property Yadif As New OptionParam With {.Text = "Yadif Mode", .HelpSwitch = "--vpp-yadif", .AlwaysOn = True, .Options = {"Auto", "TFF", "BFF", "Bob", "Bob TFF", "Bob BFF"}, .Values = {"", "tff", "bff", "bob", "bob_tff", "bob_bff"}, .VisibleFunc = Function() Deinterlacer.Value = 3}

        Property Edgelevel As New BoolParam With {.Text = "Edgelevel filter to enhance edge", .Switches = {"--vpp-edgelevel"}, .ArgsFunc = AddressOf GetEdge}
        Property EdgelevelStrength As New NumParam With {.Text = "     Strength", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}
        Property EdgelevelThreshold As New NumParam With {.Text = "     Threshold", .HelpSwitch = "--vpp-edgelevel", .Init = 20, .Config = {0, 255, 1, 1}}
        Property EdgelevelBlack As New NumParam With {.Text = "     Black", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}
        Property EdgelevelWhite As New NumParam With {.Text = "     White", .HelpSwitch = "--vpp-edgelevel", .Config = {0, 31, 1, 1}}

        Property VppResize As New OptionParam With {.Text = "Resize", .Switch = "--vpp-resize", .Options = {"Disabled", "advanced", "bilinear", "spline16", "spline36", "spline64", "lanczos2", "lanczos3", "lanczos4", "amf_bilinear", "amf_bicubic", "amf_fsr"}}
        Property VppScalerSharpness As New NumParam With {.Text = "Scaler Sharpness", .Switch = "--vpp-scaler-sharpness", .Config = {0, 2, 0.1, 2}, .Init = 0.5, .VisibleFunc = Function() VppResize.ValueText = "amf_fsr"}

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

        Property Pmd As New BoolParam With {.Switch = "--vpp-pmd", .Text = "Denoise using PMD", .ArgsFunc = AddressOf GetPmdArgs}
        Property PmdApplyCount As New NumParam With {.Text = "      Apply Count", .Init = 2}
        Property PmdStrength As New NumParam With {.Text = "      Strength", .Name = "PmdStrength", .Init = 100.0, .Config = {0, 100, 1, 1}}
        Property PmdThreshold As New NumParam With {.Text = "      Threshold", .Init = 100.0, .Config = {0, 255, 1, 1}}

        Property Knn As New BoolParam With {.Switch = "--vpp-knn", .Text = "Denoise using K-nearest neighbor", .ArgsFunc = AddressOf GetKnnArgs}
        Property KnnRadius As New NumParam With {.Text = "      Radius", .Init = 3}
        Property KnnStrength As New NumParam With {.Text = "      Strength", .Init = 0.08, .Config = {0, 1, 0.02, 2}}
        Property KnnLerp As New NumParam With {.Text = "      Lerp", .Init = 0.2, .Config = {0, Integer.MaxValue, 0.1, 1}}
        Property KnnThLerp As New NumParam With {.Text = "      TH Lerp", .Init = 0.8, .Config = {0, 1, 0.1, 1}}


        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)

                    Add("Basic", Codec, Mode,
                        OutputDepth,
                        New OptionParam With {.Switch = "--preset", .Text = "Preset", .Options = {"Fast", "Balanced", "Slow"}, .Init = 1, .VisibleFunc = Function() Codec.ValueText <> "av1"},
                        New OptionParam With {.Switch = "--preset", .Text = "Preset", .Options = {"Fast", "Balanced", "Slow", "Slower"}, .Init = 1, .VisibleFunc = Function() Codec.ValueText = "av1"},
                        New OptionParam With {.Switch = "--profile", .Name = "profile264", .VisibleFunc = Function() Codec.ValueText = "h264", .Text = "Profile", .Options = {"Automatic", "Baseline", "Main", "High", "High444"}},
                        New OptionParam With {.Switch = "--profile", .Name = "profile265", .VisibleFunc = Function() Codec.ValueText = "hevc", .Text = "Profile", .Options = {"Automatic", "Main", "Main10", "Main444"}},
                        New OptionParam With {.Switch = "--profile", .Name = "profile265", .VisibleFunc = Function() Codec.ValueText = "av1", .Text = "Profile", .Options = {"Automatic", "Main"}},
                        New OptionParam With {.Switch = "--level", .Name = "LevelH264", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "h264", .Options = {"Unrestricted", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                        New OptionParam With {.Switch = "--level", .Name = "LevelH265", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "hevc", .Options = {"Unrestricted", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}},
                        New OptionParam With {.Switch = "--level", .Name = "LevelAV1", .Text = "Level", .VisibleFunc = Function() Codec.ValueText = "av1", .Options = {"Unrestricted", "2", "2.1", "2.2", "2.3", "3", "3.1", "3.2", "3.3", "4", "4.1", "4.2", "4.3", "5", "5.1", "5.2", "5.3", "6", "6.1", "6.2", "6.3", "7", "7.1", "7.2", "7.3"}},
                        New OptionParam With {.Switch = "--tier", .Text = "Tier", .Options = {"Main", "High"}, .VisibleFunc = Function() Codec.ValueText = "hevc"},
                        QPAdvanced, QP, QPAV1, QPI, QPIAV1, QPP, QPPAV1, QPB, QPBAV1, Bitrate, QvbrQuality)
                    Add("Slice Decision",
                        New NumParam With {.Switch = "--slices", .Text = "Slices", .Init = 1, .VisibleFunc = Function() Codec.ValueText <> "av1"},
                        New NumParam With {.Switch = "--bframes", .Text = "B-Frames", .Config = {0, 16}},
                        New NumParam With {.Switch = "--ref", .Text = "Ref Frames", .Init = 0, .Config = {0, 16}},
                        New NumParam With {.Switch = "--gop-len", .Text = "GOP Length", .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--ltr", .Text = "LTR Frames", .Config = {0, Integer.MaxValue, 1}},
                        New BoolParam With {.Switch = "--adapt-minigop", .Text = "Adapt MiniGop", .VisibleFunc = Function() Codec.ValueText = "h264" OrElse Codec.ValueText = "av1"},
                        New BoolParam With {.Switch = "--b-pyramid", .Text = "B-Pyramid"})
                    Add("Rate Control",
                        New NumParam With {.Switch = "--max-bitrate", .Text = "Max Bitrate", .Init = 0, .Config = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--vbv-bufsize", .Text = "VBV Bufsize", .Init = 0, .Config = {0, 500000, 1}},
                        New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP", .Init = 51, .Config = {0, 51, 1}, .VisibleFunc = Function() Codec.Value <> 2 AndAlso OutputDepth.Value = 0},
                        New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP", .Init = 0, .Config = {0, 51, 1}, .VisibleFunc = Function() Codec.Value <> 2 AndAlso OutputDepth.Value = 0},
                        New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP", .Init = 63, .Config = {0, 63, 1}, .VisibleFunc = Function() Codec.Value <> 2 AndAlso OutputDepth.Value = 1},
                        New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP", .Init = 0, .Config = {0, 63, 1}, .VisibleFunc = Function() Codec.Value <> 2 AndAlso OutputDepth.Value = 1},
                        New NumParam With {.Switch = "--qp-max", .Text = "Maximum QP", .Init = 255, .Config = {0, 255, 1}, .VisibleFunc = Function() Codec.Value = 2},
                        New NumParam With {.Switch = "--qp-min", .Text = "Minimum QP", .Init = 0, .Config = {0, 255, 1}, .VisibleFunc = Function() Codec.Value = 2},
                        New NumParam With {.Switch = "--b-deltaqp", .Text = "Non-ref Bframe QP Offset"},
                        New NumParam With {.Switch = "--bref-deltaqp", .Text = "Ref Bframe QP Offset"})
                    Add("Pre...",
                        New BoolParam With {.Switch = "--pe", .Text = "Pre-Encode assisted rate control"},
                        Pa, PaSc, PaSs, PaActivityType, PaCaqStrength, PaInitqpsc, PaFskipMaxqp, PaLookahead, PaLtr, PaPaq, PaTaq, PaMotionQuality)
                    Add("AV1 Specific",
                        Tiles, TemporalLayers, AqMode, CdefMode, CdfUpdate, CdfFrameEndUpdate)
                    Add("VPP | Misc",
                        New StringParam With {.Switch = "--vpp-subburn", .Text = "Subburn"},
                        New OptionParam With {.Switch = "--vpp-rotate", .Text = "Rotate", .Options = {"Disabled", "90", "180", "270"}},
                        VppResize, VppScalerSharpness)
                    Add("VPP | Misc 2",
                        Tweak, TweakBrightness, TweakContrast, TweakSaturation, TweakGamma, TweakHue, TweakSwapuv,
                        Pad, PadLeft, PadTop, PadRight, PadBottom,
                        Smooth, SmoothQuality, SmoothQP, SmoothPrec)
                    Add("VPP | Misc 3",
                        TransformFlipX, TransformFlipY, TransformTranspose,
                        New StringParam With {.Switch = "--vpp-decimate", .Text = "Decimate"},
                        New StringParam With {.Switch = "--vpp-mpdecimate", .Text = "MP Decimate"},
                        New BoolParam With {.Switch = "--vpp-rff", .Text = "RFF", .Init = True})
                    Add("VPP | Colorspace",
                        Colorspace,
                        ColorspaceMatrixFrom, ColorspaceMatrixTo,
                        ColorspaceColorprimFrom, ColorspaceColorprimTo,
                        ColorspaceTransferFrom, ColorspaceTransferTo,
                        ColorspaceRangeFrom, ColorspaceRangeTo)
                    Add("VPP | Colorspace | HDR2SDR",
                        ColorspaceHdr2sdr,
                        ColorspaceHdr2sdrSourcepeak, ColorspaceHdr2sdrLdrnits, ColorspaceHdr2sdrDesatbase, ColorspaceHdr2sdrDesatstrength, ColorspaceHdr2sdrDesatexp,
                        ColorspaceHdr2sdrHableA, ColorspaceHdr2sdrHableB, ColorspaceHdr2sdrHableC, ColorspaceHdr2sdrHableD, ColorspaceHdr2sdrHableE, ColorspaceHdr2sdrHableF,
                        ColorspaceHdr2sdrMobiusTransition, ColorspaceHdr2sdrMobiusPeak, ColorspaceHdr2sdrReinhardContrast, ColorspaceHdr2sdrReinhardPeak)
                    Add("VPP | Deband",
                        Deband, DebandRange, DebandSample, DebandThre, DebandThreY, DebandThreCB, DebandThreCR,
                        DebandDither, DebandDitherY, DebandDitherC, DebandSeed, DebandBlurfirst, DebandRandEachFrame)
                    Add("VPP | Deinterlace",
                        Deinterlacer,
                        Yadif,
                        NnediField, NnediNns, NnediNsize, NnediQuality, NnediPrescreen, NnediErrortype, NnediPrec, NnediWeightfile)
                    Add("VPP | Deinterlace 2",
                        AfsINI, AfsPreset, AfsLeft, AfsRight, AfsTop, AfsBottom, AfsMethodSwitch, AfsCoeffShift, AfsThreShift, AfsThreDeint, AfsThreMotionY, AfsThreMotionC, AfsLevel,
                        Decomb, DecombFull, DecombThreshold, DecombDThreshold, DecombBlend)
                    Add("VPP | Deinterlace 2 | AFS 2",
                        AfsShift, AfsDrop, AfsSmooth, Afs24fps, AfsTune, AfsRFF, AfsTimecode, AfsLog)
                    Add("VPP | Denoise",
                        Knn, KnnRadius, KnnStrength, KnnLerp, KnnThLerp,
                        Pmd, PmdApplyCount, PmdStrength, PmdThreshold,
                        DenoiseDct, DenoiseDctStep, DenoiseDctSigma, DenoiseDctBlockSize)
                    Add("VPP | Denoise 2",
                        Fft3d, Fft3dSigma, Fft3dAmount, Fft3dBlockSize, Fft3dOverlap, Fft3dMethod, Fft3dTemporal, Fft3dPrec,
                        Nlmeans, NlmeansSigma, NlmeansH, NlmeansPatch, NlmeansSearch, NlmeansFp16)
                    Add("VPP | Sharpness",
                        Edgelevel, EdgelevelStrength, EdgelevelThreshold, EdgelevelBlack, EdgelevelWhite,
                        Unsharp, UnsharpRadius, UnsharpWeight, UnsharpThreshold,
                        Warpsharp, WarpsharpThreshold, WarpsharpBlur, WarpsharpType, WarpsharpDepth, WarpsharpChroma)
                    Add("VUI",
                        New StringParam With {.Switch = "--dhdr10-info", .Text = "HDR10 Info File", .Help = "Path to the JSON file or 'copy' to copy from source", .BrowseFile = True, .VisibleFunc = Function() Codec.Value = 1 OrElse Codec.Value = 2},   'DolbyVisionProfile, DolbyVisionRpu,
                        New StringParam With {.Switch = "--master-display", .Text = "Master Display", .VisibleFunc = Function() Codec.ValueText = "h265" OrElse Codec.ValueText = "av1"},
                        New StringParam With {.Switch = "--sar", .Text = "Sample Aspect Ratio", .Init = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                        New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat", .Options = {"Undefined", "NTSC", "Component", "PAL", "SECAM", "MAC"}},
                        New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix", .Options = {"Undefined", "BT 2020 C", "BT 2020 NC", "BT 470 BG", "BT 709", "FCC", "GBR", "SMPTE 170 M", "SMPTE 240 M", "YCgCo"}},
                        New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim", .Options = {"Undefined", "BT 2020", "BT 470 BG", "BT 470 M", "BT 709", "Film", "SMPTE 170 M", "SMPTE 240 M"}},
                        New OptionParam With {.Switch = "--transfer", .Text = "Transfer", .Options = {"Undefined", "ARIB-STD-B67", "Auto", "BT 1361 E", "BT 2020-10", "BT 2020-12", "BT 470 BG", "BT 470 M", "BT 709", "IEC 61966-2-1", "IEC 61966-2-4", "Linear", "Log 100", "Log 316", "SMPTE 170 M", "SMPTE 240 M", "SMPTE 2084", "SMPTE 428"}},
                        New OptionParam With {.Switch = "--atc-sei", .Text = "ATC SEI", .Init = 1, .Options = {"Undef", "Unknown", "Auto", "Auto_Res", "BT 709", "SMPTE 170 M", "BT 470 M", "BT 470 BG", "SMPTE 240 M", "Linear", "Log 100", "Log 316", "IEC 61966-2-4", "BT 1361 E", "IEC 61966-2-1", "BT 2020-10", "BT 2020-12", "SMPTE 2084", "SMPTE 428", "ARIB-STD-B67"}, .VisibleFunc = Function() Codec.ValueText = "h265"},
                        New OptionParam With {.Switch = "--colorrange", .Text = "Colorrange", .Options = {"Auto", "Limited", "Full"}},
                        MaxCLL, MaxFALL,
                        New NumParam With {.Switch = "--chromaloc", .Text = "Chromaloc", .Config = {0, 5}},
                        New BoolParam With {.Switch = "--enforce-hrd", .Text = "Enforce HRD compatibility"})
                    Add("Statistic",
                        New BoolParam With {.Switch = "--ssim", .Text = "SSIM"},
                        New BoolParam With {.Switch = "--psnr", .Text = "PSNR"})
                    Add("Input/Output",
                        New StringParam With {.Switch = "--input-option", .Text = "Input Option", .VisibleFunc = Function() Decoder.ValueText.EqualsAny("avhw", "avsw")},
                        Decoder, Interlace,
                        New NumParam With {.Switch = "--input-analyze", .Text = "Input Analyze", .Init = 5, .Config = {1, 600, 0.1, 1}})
                    Add("Other",
                        New StringParam With {.Text = "Custom", .Quotes = QuotesMode.Never, .AlwaysOn = True},
                        New StringParam With {.Switch = "--chapter", .Text = "Chapters", .BrowseFile = True},
                        New StringParam With {.Switch = "--log", .Text = "Log File", .BrowseFile = True},
                        New StringParam With {.Switch = "--timecode", .Text = "Timecode File", .BrowseFile = True},
                        New OptionParam With {.Switch = "--log-level", .Text = "Log Level", .Options = {"Info", "Debug", "Warn", "Error"}},
                        New OptionParam With {.Switch = "--motion-est", .Text = "Motion Estimation", .Options = {"Q-pel", "Full-pel", "Half-pel"}},
                        New BoolParam With {.Switch = "--chapter-copy", .Text = "Copy Chapters"},
                        New BoolParam With {.Switch = "--vbaq", .Text = "Adaptive quantization in frame"},
                        New BoolParam With {.Switch = "--no-deblock", .Text = "Disable deblock filter"},
                        New BoolParam With {.Switch = "--filler", .Text = "Use filler data"},
                        New StringParam With {.Switch = "--thread-affinity", .Text = "Thread Affinity"})
                End If

                Return ItemsValue
            End Get
        End Property


        Protected Overrides Sub OnValueChanged(item As CommandLineParam)
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
                ColorspaceMatrixFrom.MenuButton.Enabled = Colorspace.Value
                ColorspaceMatrixTo.MenuButton.Enabled = Colorspace.Value
                ColorspaceColorprimFrom.MenuButton.Enabled = Colorspace.Value
                ColorspaceColorprimTo.MenuButton.Enabled = Colorspace.Value
                ColorspaceTransferFrom.MenuButton.Enabled = Colorspace.Value
                ColorspaceTransferTo.MenuButton.Enabled = Colorspace.Value
                ColorspaceRangeFrom.MenuButton.Enabled = Colorspace.Value
                ColorspaceRangeTo.MenuButton.Enabled = Colorspace.Value
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

                KnnRadius.NumEdit.Enabled = Knn.Value
                KnnStrength.NumEdit.Enabled = Knn.Value
                KnnLerp.NumEdit.Enabled = Knn.Value
                KnnThLerp.NumEdit.Enabled = Knn.Value

                PadLeft.NumEdit.Enabled = Pad.Value
                PadTop.NumEdit.Enabled = Pad.Value
                PadRight.NumEdit.Enabled = Pad.Value
                PadBottom.NumEdit.Enabled = Pad.Value

                SmoothQuality.NumEdit.Enabled = Smooth.Value
                SmoothQP.NumEdit.Enabled = Smooth.Value
                SmoothPrec.MenuButton.Enabled = Smooth.Value

                TweakContrast.NumEdit.Enabled = Tweak.Value
                TweakGamma.NumEdit.Enabled = Tweak.Value
                TweakSaturation.NumEdit.Enabled = Tweak.Value
                TweakHue.NumEdit.Enabled = Tweak.Value
                TweakBrightness.NumEdit.Enabled = Tweak.Value
                TweakSwapuv.MenuButton.Enabled = Tweak.Value

                PaSc.MenuButton.Enabled = Pa.Value
                PaSs.MenuButton.Enabled = Pa.Value
                PaActivityType.MenuButton.Enabled = Pa.Value
                PaCaqStrength.MenuButton.Enabled = Pa.Value
                PaInitqpsc.NumEdit.Enabled = Pa.Value
                PaFskipMaxqp.NumEdit.Enabled = Pa.Value
                PaLookahead.NumEdit.Enabled = Pa.Value
                PaLtr.MenuButton.Enabled = Pa.Value
                PaPaq.MenuButton.Enabled = Pa.Value
                PaTaq.MenuButton.Enabled = Pa.Value
                PaMotionQuality.MenuButton.Enabled = Pa.Value

                PmdApplyCount.NumEdit.Enabled = Pmd.Value
                PmdStrength.NumEdit.Enabled = Pmd.Value
                PmdThreshold.NumEdit.Enabled = Pmd.Value

                For Each i In {DebandRange, DebandSample, DebandThre, DebandThreY, DebandThreCB,
                    DebandThreCR, DebandDither, DebandDitherY, DebandDitherC, DebandSeed}

                    i.NumEdit.Enabled = Deband.Value
                Next

                DebandRandEachFrame.CheckBox.Enabled = Deband.Value
                DebandBlurfirst.CheckBox.Enabled = Deband.Value

                AfsPreset.MenuButton.Enabled = Deinterlacer.Value = 1
                AfsINI.TextEdit.Enabled = Deinterlacer.Value = 1

                For Each i In {AfsLeft, AfsRight, AfsTop, AfsBottom, AfsMethodSwitch, AfsCoeffShift,
                               AfsThreShift, AfsThreDeint, AfsThreMotionY, AfsThreMotionC, AfsLevel}

                    i.NumEdit.Enabled = Deinterlacer.Value = 1
                Next

                For Each i In {AfsShift, AfsDrop, AfsSmooth, Afs24fps, AfsTune, AfsRFF, AfsTimecode, AfsLog}
                    i.CheckBox.Enabled = Deinterlacer.Value = 1
                Next

                NnediField.MenuButton.Enabled = Deinterlacer.Value = 2
                NnediNns.MenuButton.Enabled = Deinterlacer.Value = 2
                NnediNsize.MenuButton.Enabled = Deinterlacer.Value = 2
                NnediQuality.MenuButton.Enabled = Deinterlacer.Value = 2
                NnediPrescreen.MenuButton.Enabled = Deinterlacer.Value = 2
                NnediErrortype.MenuButton.Enabled = Deinterlacer.Value = 2
                NnediPrec.MenuButton.Enabled = Deinterlacer.Value = 2
                NnediWeightfile.TextEdit.Enabled = Deinterlacer.Value = 2

            End If

            MyBase.OnValueChanged(item)
        End Sub



        Public Overrides Sub ShowHelp(options As String())
            ShowConsoleHelp(Package.VCEEncC, options)
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

        Function GetDecombArgs() As String
            If Decomb.Value Then
                Dim ret = ""
                If DecombFull.Value <> DecombFull.DefaultValue Then ret += ",full=" & DecombFull.Value.ToInvariantString
                If DecombThreshold.Value <> DecombThreshold.DefaultValue Then ret += ",threshold=" & DecombThreshold.Value.ToInvariantString
                If DecombDThreshold.Value <> DecombDThreshold.DefaultValue Then ret += ",dthreshold=" & DecombDThreshold.Value.ToInvariantString
                If DecombBlend.Value <> DecombBlend.DefaultValue Then ret += ",blend=" & DecombBlend.Value.ToInvariantString
                Return "--vpp-decomb " + ret.TrimStart(","c)
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

        Function GetPmdArgs() As String
            If Pmd.Value Then
                Dim ret = ""
                If PmdApplyCount.Value <> PmdApplyCount.DefaultValue Then ret += ",apply_count=" & PmdApplyCount.Value
                If PmdStrength.Value <> PmdStrength.DefaultValue Then ret += ",strength=" & PmdStrength.Value
                If PmdThreshold.Value <> PmdThreshold.DefaultValue Then ret += ",threshold=" & PmdThreshold.Value
                Return ("--vpp-pmd " + ret.TrimStart(","c)).Trim()
            End If
            Return ""
        End Function

        Function GetPaArgs() As String
            If Pa.Value Then
                Dim ret = ""
                If PaSc.Value <> PaSc.DefaultValue Then ret += ",sc=" & PaSc.OptionText.ToInvariantString
                If PaSs.Value <> PaSs.DefaultValue Then ret += ",ss=" & PaSs.OptionText.ToInvariantString
                If PaActivityType.Value <> PaActivityType.DefaultValue Then ret += ",activity-type=" & PaActivityType.OptionText.ToInvariantString
                If PaCaqStrength.Value <> PaCaqStrength.DefaultValue Then ret += ",caq-strength=" & PaCaqStrength.OptionText.ToInvariantString
                If PaInitqpsc.Value <> PaInitqpsc.DefaultValue Then ret += ",initqpsc=" & PaInitqpsc.Value.ToInvariantString
                If PaFskipMaxqp.Value <> PaFskipMaxqp.DefaultValue Then ret += ",fskip-maxqp=" & PaFskipMaxqp.Value.ToInvariantString
                If PaLookahead.Value <> PaLookahead.DefaultValue Then ret += ",lookahead=" & PaLookahead.Value.ToInvariantString
                If PaLtr.Value <> PaLtr.DefaultValue Then ret += ",ltr=" & PaLtr.ValueText.ToInvariantString
                If PaPaq.Value <> PaPaq.DefaultValue Then ret += ",paq=" & PaPaq.ValueText.ToInvariantString
                If PaTaq.Value <> PaTaq.DefaultValue Then ret += ",taq=" & PaTaq.Value.ToInvariantString
                If PaMotionQuality.Value <> PaMotionQuality.DefaultValue Then ret += ",motion-quality=" & PaMotionQuality.ValueText.ToInvariantString
                Return ("--pa " + ret.TrimStart(","c)).Trim()
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
                If TweakSwapuv.Value <> TweakSwapuv.DefaultValue Then ret += ",swapuv=" & TweakSwapuv.ValueText.ToInvariantString
                Return ("--vpp-tweak " + ret.TrimStart(","c)).Trim()
            End If
            Return ""
        End Function

        Function GetPaddingArgs() As String
            Return If(Pad.Value, $"--vpp-pad {PadLeft.Value},{PadTop.Value},{PadRight.Value},{PadBottom.Value}", "")
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

        Function GetDeinterlacerArgs() As String
            Dim ret = ""
            Select Case Deinterlacer.Value
                Case 1
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
                    Return ("--vpp-afs " + ret.TrimStart(","c)).Trim()
                Case 2
                    If NnediField.Value <> NnediField.DefaultValue Then ret += ",field=" + NnediField.ValueText
                    If NnediNns.Value <> NnediNns.DefaultValue Then ret += ",nns=" + NnediNns.ValueText
                    If NnediNsize.Value <> NnediNsize.DefaultValue Then ret += ",nsize=" + NnediNsize.ValueText
                    If NnediQuality.Value <> NnediQuality.DefaultValue Then ret += ",quality=" + NnediQuality.ValueText
                    If NnediPrescreen.Value <> NnediPrescreen.DefaultValue Then ret += ",prescreen=" + NnediPrescreen.ValueText
                    If NnediErrortype.Value <> NnediErrortype.DefaultValue Then ret += ",errortype=" + NnediErrortype.ValueText
                    If NnediPrec.Value <> NnediPrec.DefaultValue Then ret += ",prec=" + NnediPrec.ValueText
                    If NnediWeightfile.Value <> "" Then ret += ",weightfile=" + NnediWeightfile.Value.Escape
                    Return ("--vpp-nnedi " + ret.TrimStart(","c)).Trim()
                Case 3
                    If Yadif.Value <> Yadif.DefaultValue Then ret += ",mode=" + Yadif.ValueText
                    Return ("--vpp-yadif " + ret.TrimStart(","c)).Trim()
                Case Else
                    Return ret
            End Select
        End Function


        Overrides Function GetCommandLine(
            includePaths As Boolean,
            includeExecutable As Boolean,
            Optional pass As Integer = 1) As String

            Dim ret As String = ""
            Dim sourcePath As String = ""
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)

            If includePaths AndAlso includeExecutable Then
                ret = Package.VCEEncC.Path.Escape
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
                        ret = If(includePaths, Package.QSVEncC.Path.Escape, "QSVEncC64") + " -o - -c raw" + " -i " + If(includePaths, p.SourceFile.Escape, "path") + " | " + If(includePaths, Package.VCEEncC.Path.Escape, "VCEEncC64")
                    End If
                Case "ffdxva"
                    sourcePath = "-"

                    If includePaths Then
                        Dim pix_fmt = If(p.SourceVideoBitDepth = 10, "yuv420p10le", "yuv420p")
                        ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") +
                            " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.SourceFile.Escape, "path") +
                            " -f yuv4mpegpipe -pix_fmt " + pix_fmt + " -strict -1 -loglevel fatal - | " +
                            If(includePaths, Package.VCEEncC.Path.Escape, "VCEEncC64")
                    End If
                Case "ffqsv"
                    sourcePath = "-"

                    If includePaths Then
                        ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.SourceFile.Escape, "path") + " -f yuv4mpegpipe -strict -1 -pix_fmt yuv420p -loglevel fatal - | " + If(includePaths, Package.VCEEncC.Path.Escape, "VCEEncC64")
                    End If
            End Select

            Dim rate = If(pass = 1, CInt(Bitrate.Value), p.VideoBitrate)

            Select Case Mode.Value
                Case 0
                    ret += If(QPAdvanced.Value, $" --cqp {QPI.Value}:{QPP.Value}:{QPB.Value}", $" --cqp {QP.Value}")
                Case 1
                    ret += " --cbr " & rate
                Case 2
                    ret += " --cbrhq " & rate
                Case 3
                    ret += " --vbr " & rate
                Case 4
                    ret += " --vbrhq " & rate
                Case 5
                    ret += " --qvbr " & rate
                Case Else
                    Throw New NotImplementedException("Mode not supported")
            End Select

            Dim q = From i In Items Where i.GetArgs <> ""

            If q.Count > 0 Then
                ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")
            End If


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

            If ret.Contains("%") Then
                ret = Macro.Expand(ret)
            End If

            If includePaths Then
                ret += " -i " + sourcePath.Escape + " -o " + targetPath.Escape
            End If

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.VCEEncC
        End Function
    End Class
End Class