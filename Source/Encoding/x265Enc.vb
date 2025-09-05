﻿
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports StaxRip.UI

Imports StaxRip.VideoEncoderCommandLine

<Serializable()>
Public Class x265Enc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Sub New()
        Name = "x265"
        Params.ApplyPresetDefaultValues()
        Params.ApplyPresetValues()
    End Sub

    <NonSerialized>
    Private ParamsValue As x265Params

    Property Params As x265Params
        Get
            If ParamsValue Is Nothing Then
                ParamsValue = New x265Params
                ParamsValue.Init(ParamsStore)
            End If

            Return ParamsValue
        End Get
        Set(value As x265Params)
            ParamsValue = value
        End Set
    End Property

    Public Overrides ReadOnly Property OverridesTargetFileName As Boolean
        Get
            Return Params.OverrideTargetFileName.Value
        End Get
    End Property

    Public Overrides ReadOnly Property OverridingTargetFileName As String
        Get
            Dim value = Macro.ExpandParamValues(Params.TargetFileName.Value, Params.Items)
            value = value.Replace(Environment.NewLine,"")
            Return value
        End Get
    End Property

    Overrides ReadOnly Property IsDolbyVisionSet As Boolean
        Get
            If Not Params.DolbyVisionProfile.Visible Then Return False
            If Not Params.DolbyVisionRpu.Visible Then Return False
            Return Params.DolbyVisionProfile.Value <> 0 AndAlso Not String.IsNullOrWhiteSpace(Params.DolbyVisionRpu.Value)
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

    Overrides ReadOnly Property Codec As String
        Get
            Return "hevc"
        End Get
    End Property

    Overrides ReadOnly Property OutputExt As String
        Get
            Return "hevc"
        End Get
    End Property

    Overrides Sub Encode()
        Encode("Video encoding", GetArgs(1, 0, 0, Nothing, p.Script), s.ProcessPriority)

        If Params.Mode.Value = x265RateMode.TwoPass Then
            Encode("Video encoding second pass", GetArgs(2, 0, 0, Nothing, p.Script), s.ProcessPriority)
        ElseIf Params.Mode.Value = x265RateMode.ThreePass Then
            'Specific order 1 > 3 > 2 is correct!
            Encode("Video encoding Nth pass", GetArgs(3, 0, 0, Nothing, p.Script), s.ProcessPriority)
            Encode("Video encoding last pass", GetArgs(2, 0, 0, Nothing, p.Script), s.ProcessPriority)
        End If
    End Sub

    Overloads Sub Encode(passName As String, commandLine As String, priority As ProcessPriorityClass)
        If Not CanChunkEncode() Then
            p.Script.Synchronize()
        End If

        Using proc As New Proc
            proc.Package = Package.x265
            proc.Header = passName
            proc.Encoding = Encoding.UTF8
            proc.Priority = priority
            proc.FrameCount = p.Script.GetFrameCount
            proc.SkipString = "%] "

            If commandLine.Contains("|") Then
                proc.File = "cmd.exe"
                proc.Arguments = "/S /C """ + commandLine + """"
            Else
                proc.CommandLine = commandLine
            End If

            proc.Start()
        End Using
    End Sub

    Overrides Function BeforeEncoding() As Boolean
        Dim rpu = Params.DolbyVisionRpu.Value
        If Not String.IsNullOrWhiteSpace(rpu) AndAlso rpu = p.HdrDolbyVisionMetadataFile?.Path AndAlso rpu.FileExists() Then
            Dim offset = If(p.Script.IsFilterActive("Crop"), New Padding(p.CropLeft, p.CropTop, p.CropRight, p.CropBottom), Padding.Empty)
            Dim rpuProfile = p.HdrDolbyVisionMetadataFile.ReadProfileFromRpu()
            Dim profile = Params.DolbyVisionProfile.ValueText
            Dim mode = DoviMode.Mode0
            If profile = "8.1" Then mode = DoviMode.Mode2
            If profile = "8.2" Then mode = DoviMode.Mode2
            If profile = "8.4" Then mode = DoviMode.Mode4

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

        If Not String.IsNullOrEmpty(chromaloc) AndAlso Params.Chromaloc.IsDefaultValue Then
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

                If Params.Level.Value > 0 AndAlso Params.HighTier.Value > 0 Then
                    Dim m = GetH265MaxBitrate(Params.Level.ValueText.ToSingle(), Params.HighTier.Value = 1)
                    If Params.VbvMaxRate.Value = 0 Then cl += $" {Params.VbvMaxRate.Switch} {m}"
                    If Params.VbvBufSize.Value = 0 Then cl += $" {Params.VbvBufSize.Switch} {m}"
                End If

                If Not String.IsNullOrWhiteSpace(p.Hdr10PlusMetadataFile) AndAlso p.Hdr10PlusMetadataFile.FileExists() Then
                    cl += $" --dhdr10-info ""{p.Hdr10PlusMetadataFile}"""
                End If
            End If
        End If

        If Not String.IsNullOrWhiteSpace(p.HdrDolbyVisionMetadataFile?.Path) Then
            Dim rpuProfile = p.HdrDolbyVisionMetadataFile.ReadProfileFromRpu()
            Dim profile = ""
            If rpuProfile = 5 Then profile = "5"
            If rpuProfile = 7 Then profile = "8.1"
            If rpuProfile = 8 Then profile = "8.1"
            If profile = "8.1" AndAlso transfer_characteristics = "HLG" Then
                profile = "8.4"
            End If
            If Params.GetStringParam("--dolby-vision-profile")?.Value = "" AndAlso profile <> "" Then
                cl += $" --dolby-vision-profile {profile}"
            End If

            cl += " --output-depth 10"
            cl += $" --dolby-vision-rpu ""{p.HdrDolbyVisionMetadataFile.Path}"""
        End If

        Dim MaxCLL = MediaInfo.GetVideo(sourceFile, "MaxCLL").Trim.Left(" ").ToInt
        Dim MaxFALL = MediaInfo.GetVideo(sourceFile, "MaxFALL").Trim.Left(" ").ToInt

        If MaxCLL <> 0 OrElse MaxFALL <> 0 Then
            cl += $" --max-cll ""{MaxCLL},{MaxFALL}"""
        End If

        ImportCommandLine(cl)
    End Sub

    Overrides Property Bitrate As Integer
        Get
            Return CInt(Params.Bitrate.Value)
        End Get
        Set(value As Integer)
            Params.Bitrate.Value = value
        End Set
    End Property

    Overrides Function CanChunkEncode() As Boolean
        Return CInt(Params.Chunks.Value) > 1 AndAlso Not IsDolbyVisionSet
    End Function

    Overrides Function GetChunks() As Integer
        Return CInt(Params.Chunks.Value)
    End Function

    Overrides Function GetChunkEncodeActions() As List(Of Action)
        Dim maxFrame = If(Params.Decoder.Value = 0, p.Script.GetFrameCount(), p.SourceFrames)
        Dim chunkCount = CInt(Params.Chunks.Value)
        Dim startFrame = CInt(Params.Seek.Value)
        Dim length = If(CInt(Params.Frames.Value) > 0, CInt(Params.Frames.Value), maxFrame - startFrame)
        Dim endFrame = Math.Min(startFrame + length - 1, maxFrame)
        Dim chunkLength = length \ chunkCount
        Dim ret As New List(Of Action)

        For chunk = 0 To chunkCount - 1
            Dim chunkStart = startFrame + (chunk * chunkLength)
            Dim chunkEnd = If(chunk <> chunkCount - 1, chunkStart + (chunkLength - 1), endFrame)
            Dim chunkName = ""
            Dim passName = ""

            If chunkCount > 1 Then
                chunkName = "_chunk" & (chunk + 1)
                passName = " chunk " & (chunk + 1)
            End If

            If Params.Mode.Value = x265RateMode.TwoPass Then
                ret.Add(Sub()
                            Encode("Video encoding pass 1" + passName, GetArgs(1, chunkStart, chunkEnd, chunkName, p.Script), s.ProcessPriority)
                            Encode("Video encoding pass 2" + passName, GetArgs(2, chunkStart, chunkEnd, chunkName, p.Script), s.ProcessPriority)
                        End Sub)
            ElseIf Params.Mode.Value = x265RateMode.ThreePass Then
                ret.Add(Sub()
                            'Specific order 1 > 3 > 2 is correct!
                            Encode("Video encoding first pass" + passName, GetArgs(1, chunkStart, chunkEnd, chunkName, p.Script), s.ProcessPriority)
                            Encode("Video encoding Nth pass" + passName, GetArgs(3, chunkStart, chunkEnd, chunkName, p.Script), s.ProcessPriority)
                            Encode("Video encoding last pass" + passName, GetArgs(2, chunkStart, chunkEnd, chunkName, p.Script), s.ProcessPriority)
                        End Sub)
            Else
                ret.Add(Sub() Encode("Video encoding" + passName, GetArgs(1, chunkStart, chunkEnd, chunkName, p.Script), s.ProcessPriority))
            End If
        Next

        Return ret
    End Function

    Overrides Sub RunCompCheck()
        If Not g.VerifyRequirements OrElse Not g.IsValidSource Then
            Exit Sub
        End If

        Dim newParams As New x265Params
        Dim newStore = ObjectHelp.GetCopy(ParamsStore)
        newParams.Init(newStore)

        Dim enc As New x265Enc
        enc.Params = newParams
        enc.Params.Mode.Value = x265RateMode.SingleCRF
        enc.Params.Quant.Value = enc.Params.CompCheck.Value

        Dim script As New VideoScript
        script.Engine = p.Script.Engine
        script.Filters = p.Script.GetFiltersCopy
        Dim code As String
        Dim framerate = p.Script.GetFramerate()
        Dim totalFrames = p.Script.GetFrameCount()
        Dim range = framerate * p.CompCheckTestblockSeconds
        Dim every = (100 / p.CompCheckPercentage) * range

        If script.Engine = ScriptEngine.AviSynth Then
            code = $"SelectRangeEvery({CInt(every)},{CInt(range)})"
        Else
            code = "fpsnum = clip.fps_num" + BR
            code += "fpsden = clip.fps_den" + BR
            code += $"clip = core.std.SelectEvery(clip = clip, cycle = {CInt(every)}, offsets = range({CInt(range)}))" + BR
            code += "clip = core.std.AssumeFPS(clip = clip, fpsnum = fpsnum, fpsden = fpsden)"
        End If

        script.Filters.Add(New VideoFilter("aaa", "aaa", code))
        script.Path = Path.Combine(p.TempDir, p.TargetFile.Base + "_CompCheck." + script.FileType)
        script.Synchronize()

        Log.WriteLine(BR + script.GetFullScript + BR)

        Dim commandLine = enc.Params.GetArgs(0, 0, 0, Nothing, script, Path.Combine(p.TempDir, p.TargetFile.Base + "_CompCheck." + OutputExt), True, True)

        Try
            Encode("Compressibility Check", commandLine, ProcessPriorityClass.Normal)
        Catch ex As AbortException
            Exit Sub
        Catch ex As Exception
            g.ShowException(ex)
            Exit Sub
        End Try

        Dim bits = (New FileInfo(Path.Combine(p.TempDir, p.TargetFile.Base + "_CompCheck." + OutputExt)).Length) * 8
        p.Compressibility = (bits / script.GetFrameCount) / (p.TargetWidth * p.TargetHeight)

        OnAfterCompCheck()
        g.MainForm.Assistant()

        Log.WriteLine("Quality: " & CInt(Calc.GetPercent).ToString() + " %")
        Log.WriteLine("Compressibility: " + p.Compressibility.ToString("f3"))
        Log.Save()
    End Sub

    Overloads Function GetArgs(
        pass As Integer,
        startFrame As Integer,
        endFrame As Integer,
        chunkName As String,
        script As VideoScript,
        Optional includePaths As Boolean = True) As String

        Return Params.GetArgs(pass, startFrame, endFrame, chunkName, script, OutputPath.DirAndBase + OutputExtFull, includePaths, True)
    End Function

    Overrides Sub ShowConfigDialog(Optional param As CommandLineParam = Nothing)
        Dim newParams As New x265Params
        Dim store = ObjectHelp.GetCopy(ParamsStore)
        newParams.Init(store)
        newParams.ApplyPresetDefaultValues()
        newParams.ApplyTuneDefaultValues()

        Using form As New CommandLineForm(newParams)
            form.HTMLHelpFunc = Function() $"<p><a href=""{Package.x265.HelpURL}"">x265 Online Help</a></p>" +
               $"<h2>x265 Console Help</h2><pre>{HelpDocument.ConvertChars(Package.x265.CreateHelpfile())}</pre>"

            Dim a = Sub()
                        Dim enc = ObjectHelp.GetCopy(Me)
                        Dim params2 As New x265Params
                        Dim store2 = ObjectHelp.GetCopy(store)
                        params2.Init(store2)
                        enc.Params = params2
                        enc.ParamsStore = store2
                        SaveProfile(enc)
                    End Sub

            form.cms.Add("Save Profile...", a, Keys.Control Or Keys.S, Symbol.Save)

            If Not String.IsNullOrWhiteSpace(param?.Path) Then
                form.SimpleUI.ShowPage(param?.Path)
                form.cbGoTo.Text = param.GetSwitches().FirstOrDefault()
            End If

            If form.ShowDialog() = DialogResult.OK Then
                AutoCompCheckValue = CInt(newParams.CompCheckAimedQuality.Value)
                Params = newParams
                ParamsStore = store
                OnStateChange()
                UpdateTargetFile()
            End If
        End Using
    End Sub

    Overrides Property QualityMode() As Boolean
        Get
            Return Params.Mode.Value = x265RateMode.SingleQuant OrElse
                Params.Mode.Value = x265RateMode.SingleCRF
        End Get
        Set(Value As Boolean)
        End Set
    End Property

    Public Overrides ReadOnly Property CommandLineParams As CommandLineParams
        Get
            Return Params
        End Get
    End Property

    Sub SetMuxer()
        Muxer = New MkvMuxer()
    End Sub

    Overrides Function CreateEditControl() As Control
        Return New x265Control(Me) With {.Dock = DockStyle.Fill}
    End Function

    Shared Function Test() As String
        Dim tester As New ConsolAppTester

        tester.IgnoredSwitches = "crop-rectfast-cbf frame-skip help lavf no-scenecut
            ratetol recon-y4m-exec input input-res lft total-frames version pbration
            no-progress progress -hrd-concat fullhelp hdr-opt analysis-reuse-level
            masking-strength pme pmode"

        tester.UndocumentedSwitches = "numa-pools rdoq cip qblur cplxblur cu-stats
            dhdr10-info opt-qp-pps opt-ref-list-length-pps single-sei hrd-concat 
            dhdr10-opt crop pb-factor ip-factor level log display-window start end
            reader-options progress-readframes qp-delta-ref qp-delta-nonref
            scenecut-window"

        tester.Package = Package.x265
        tester.CodeFile = Path.Combine(Folder.Startup.Parent, "Encoding", "x265Enc.vb")

        Return tester.Test
    End Function
End Class

Public Class x265Params
    Inherits CommandLineParams

    Sub New()
        Title = "x265 Options"
    End Sub

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

    Property Mode As New OptionParam With {
        .Name = "Mode",
        .Text = "Mode",
        .Switches = {""},
        .Options = {"Bitrate", "Quantizer", "Quality", "Two Pass", "Three Pass"},
        .Value = 2}

    Property Quant As New NumParam With {
        .Switches = {"--crf", "--qp", "-q"},
        .Name = "Quant",
        .Text = "Quality",
        .DefaultValue = 28,
        .Value = 20,
        .VisibleFunc = Function() Mode.Value = 1 OrElse Mode.Value = 2,
        .Config = {0, 51, 0.5, 1}}

    Property CRFmax As New NumParam With {
        .Switch = "--crf-max",
        .Text = "Maximum CRF",
        .Init = 51,
        .Config = Quant.Config}

    Property CRFmin As New NumParam With {
        .Switch = "--crf-min",
        .Text = "Minimum CRF",
        .Init = 0,
        .Config = Quant.Config}

    Property Bitrate As New NumParam With {
        .HelpSwitch = "--bitrate",
        .Text = "Bitrate",
        .Init = 5000,
        .VisibleFunc = Function() Mode.Value <> 1 AndAlso Mode.Value <> 2,
        .Config = {0, 1000000, 100}}

    Property Decoder As New OptionParam With {
        .Text = "Decoder",
        .Options = {"AviSynth/VapourSynth", "QSVEnc (Intel)", "ffmpeg (Intel)", "ffmpeg (DXVA2)"},
        .Values = {"script", "qs", "ffqsv", "ffdxva"}}

    Property Preset As New OptionParam With {
        .Switch = "--preset",
        .Switches = {"-p"},
        .Text = "Preset",
        .Options = {"Ultra Fast", "Super Fast", "Very Fast", "Faster", "Fast", "Medium", "Slow", "Slower", "Very Slow", "Placebo"},
        .Init = 5}

    Property Tune As New OptionParam With {
        .Switch = "--tune",
        .Switches = {"-t"},
        .Text = "Tune",
        .Options = {"None", "PSNR", "SSIM", "Grain", "Fast Decode", "Zero Latency", "Animation"}}

    Property Level As New OptionParam With {
        .Switch = "--level-idc",
        .Switches = {"--level"},
        .Text = "Level",
        .Options = {"Automatic", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2", "8.5"}}

    Property HighTier As New OptionParam With {
        .Switch = "--high-tier",
        .NoSwitch = "--no-high-tier",
        .Text = "High Tier",
        .Options = {"Undefined", "Yes", "No"},
        .Values = {"", "--high-tier", "--no-high-tier"}}

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

    Property chunkStart As New NumParam With {
        .Switch = "--chunk-start",
        .Name = "Chunk Start",
        .Text = "Chunk Start"}

    Property chunkEnd As New NumParam With {
        .Switch = "--chunk-end",
        .Name = "Chunk End",
        .Text = "Chunk End"}

    Property BFrames As New NumParam With {
        .Switch = "--bframes",
        .Switches = {"-b"},
        .Text = "B-Frames",
        .Config = {0, 16}}

    Property BFrameBias As New NumParam With {
        .Switch = "--bframe-bias",
        .Text = "B-Frame Bias",
        .Config = {-90, 100}}

    Property BAdapt As New OptionParam With {
        .Switch = "--b-adapt",
        .Text = "B-Adapt",
        .IntegerValue = True,
        .Options = {"None", "Fast", "Full"}}

    Property RCLookahead As New NumParam With {
        .Switch = "--rc-lookahead",
        .Text = "RC Lookahead",
        .Config = {0, 250, 5}}

    Property LookaheadSlices As New NumParam With {
        .Switch = "--lookahead-slices",
        .Text = "Lookahead Slices",
        .Config = {0, 16}}

    Property Scenecut As New NumParam With {
        .Switch = "--scenecut",
        .Text = "Scenecut",
        .Config = {0, 900, 10}}

    Property ScenecutAwareQp As New OptionParam With {
        .HelpSwitch = "--scenecut-aware-qp",
        .Text = "Scenecut Aware Qp",
        .Options = {"0 - Disabled", "1 - Forward masking, applies after the scenecut.", "2 - Backward masking, applies before the scenecut.", "3 - Bi-directional masking, applies before and after the scenecut."},
        .IntegerValue = True,
        .Init = 0,
        .VisibleFunc = Function() Mode.Value = x265RateMode.TwoPass Or Mode.Value = x265RateMode.ThreePass}

    Property Ref As New NumParam With {
        .Switch = "--ref",
        .Text = "Ref Frames",
        .Config = {1, 16}}

    Property Slowpass As New BoolParam With {
        .Switch = "--slow-firstpass",
        .NoSwitch = "--no-slow-firstpass",
        .Init = True,
        .Text = "Slow Firstpass"}

    Property [Me] As New OptionParam With {
        .Switch = "--me",
        .Text = "Motion Search Method",
        .Options = {"Diamond", "Hexagon", "Uneven Multi-Hexegon", "Star", "SEA", "Full"},
        .Values = {"dia", "hex", "umh", "star", "sea", "full"}}

    Property MErange As New NumParam With {
        .Switch = "--merange",
        .Text = "ME Range",
        .Config = {0, Integer.MaxValue},
        .ArgsFunc = Function() If(MErange.Value = 0, "--merange " & CInt(Calc.GetYFromTwoPointForm(480, 16, 2160, 57, p.TargetHeight)), If(MErange.Value <> MErange.DefaultValue, "--merange " & CInt(MErange.Value), Nothing))}

    Property SubME As New OptionParam With {
        .Switch = "--subme",
        .Switches = {"-m"},
        .Text = "Subpel Refinement",
        .IntegerValue = True,
        .Options = {"0 - HPEL 1/4 - QPEL 0/4 - HPEL SATD false",
                    "1 - HPEL 1/4 - QPEL 1/4 - HPEL SATD false",
                    "2 - HPEL 1/4 - QPEL 1/4 - HPEL SATD true",
                    "3 - HPEL 2/4 - QPEL 1/4 - HPEL SATD true",
                    "4 - HPEL 2/4 - QPEL 2/4 - HPEL SATD true",
                    "5 - HPEL 1/8 - QPEL 1/8 - HPEL SATD true",
                    "6 - HPEL 2/8 - QPEL 1/8 - HPEL SATD true",
                    "7 - HPEL 2/8 - QPEL 2/8 - HPEL SATD true"}}

    Property MaxMerge As New NumParam With {
        .Switch = "--max-merge",
        .Text = "Maximum Merge",
        .Config = {1, 5}}

    Property SAOnonDeblock As New BoolParam With {
        .Switch = "--sao-non-deblock",
        .Text = "Specify how to handle depencency between SAO and deblocking filter"}

    Property SAO As New BoolParam With {
        .Switch = "--sao",
        .NoSwitch = "--no-sao",
        .Text = "Sample Adaptive Offset"}

    Property SignHide As New BoolParam With {
        .Switch = "--signhide",
        .NoSwitch = "--no-signhide",
        .Text = "Hide sign bit of one coeff per TU (rdo)"}

    Property CompCheck As New NumParam With {
        .Name = "CompCheckQuant",
        .Text = "Comp. Check",
        .Value = 18,
        .Help = "CRF value used as 100% for the compressibility check.",
        .Config = {1, 50}}

    Property CompCheckAimedQuality As New NumParam With {
        .Name = "CompCheckAimedQuality",
        .Text = "Aimed Quality",
        .Value = 50,
        .Help = "Percent value to adjusts the target file size or image size after the compressibility check accordingly.",
        .Config = {1, 100}}

    Property Chunks As New NumParam With {
        .Text = "Chunks",
        .Init = 1,
        .Config = {1, 128}}

    Property Weightp As New BoolParam With {
        .Switch = "--weightp",
        .Switches = {"-w"},
        .NoSwitch = "--no-weightp",
        .Text = "Enable weighted prediction in P slices"}

    Property Weightb As New BoolParam With {
        .Switch = "--weightb",
        .NoSwitch = "--no-weightb",
        .Text = "Enable weighted prediction in B slices"}

    ReadOnly Property AQmode As OptionParam
        Get
            Select Case Package.x265Type
                Case x265Type.Patman
                    Return AQmodeExtended
                Case Else
                    Return AQmodeNormal
            End Select
        End Get
    End Property

    Property AQmodeNormal As New OptionParam With {
        .Switch = "--aq-mode",
        .Text = "AQ Mode",
        .Expanded = True,
        .IntegerValue = True,
        .Init = 2,
        .AlwaysOn = False,
        .Options = {"0: Disabled", "1: AQ enabled", "2: AQ enabled with auto-variance", "3: AQ enabled with auto-variance with bias to dark scenes", "4: AQ enabled with auto-variance and edge information"},
        .ValueChangedAction = Sub(x) AQmodeExtended.Value = x}

    Property AQmodeExtended As New OptionParam With {
        .Switch = "--aq-mode",
        .Switches = {"--aq-mode", "--auto-aq"},
        .Text = "AQ Mode",
        .Expanded = True,
        .IntegerValue = True,
        .Init = 2,
        .AlwaysOn = False,
        .Options = {"0: Disabled", "1: Uniform AQ", "2: AQ enabled with auto-variance", "3: AQ enabled with auto-variance with bias to dark scenes", "4: AQ enabled with auto-variance and edge information", "5: AQ enabled with auto-variance, edge information and bias to dark scenes", "Auto: Automatic decision for each frame using its scene statistics"},
        .ArgsFunc = Function()
                        Return If(AQmodeExtended.AlwaysOn OrElse Not AQmodeExtended.IsDefaultValue,
                            If(AQmodeExtended.Value < 6, $"--aq-mode {AQmodeExtended.Value}", "--auto-aq"),
                            "")
                    End Function,
        .ImportAction = Sub(param As String, arg As String)
                            If param = "--auto-aq" Then
                                AQmodeExtended.Value = 6
                            Else
                                If arg.IsInt() Then
                                    AQmodeExtended.Value = arg.ToInt()
                                End If
                            End If
                        End Sub}

    Property AQStrength As New NumParam With {
        .Switch = "--aq-strength",
        .Text = "AQ Strength",
        .Config = {0, 3, 0.05, 2}}

    Property AQBiasStrength As New NumParam With {
        .Switch = "--aq-bias-strength",
        .Text = "AQ Bias Strength",
        .Init = 1,
        .Config = {0, 3, 0.05, 2},
        .VisibleFunc = Function() AQmode Is AQmodeExtended AndAlso (AQmode.Value = 3 OrElse AQmode.Value = 5)}

    Property CUtree As New BoolParam With {
        .Switch = "--cutree",
        .NoSwitch = "--no-cutree",
        .Text = "CU Tree",
        .Value = True}

    Property RD As New OptionParam With {
        .Switch = "--rd",
        .Text = "RD",
        .IntegerValue = True,
        .Options = {"0 - SA8D mode and split decisions, intra w/ source pixels",
                    "1 - Recon generated (better intra), RDO merge/skip selection",
                    "2 - RDO splits and merge/skip selection",
                    "3 - RDO mode and split decisions, chroma residual used for sa8d",
                    "4 - Currently same as 3",
                    "5 - Adds RDO prediction decisions",
                    "6 - Currently same as 5"}}

    Property MinCuSize As New OptionParam With {
        .Switch = "--min-cu-size",
        .Text = "Min CU size",
        .Options = {"32", "16", "8"}}

    Property MaxCuSize As New OptionParam With {
        .Switch = "--ctu",
        .Switches = {"-s"},
        .Text = "Max CU size",
        .Options = {"64", "32", "16"}}

    Property qgSize As New OptionParam With {
        .Switch = "--qg-size",
        .Text = "QG Size",
        .Options = {"64", "32", "16", "8"},
        .Init = 1}

    Property qpstep As New NumParam With {
        .Switch = "--qpstep",
        .Text = "QP Step",
        .Init = 4,
        .Config = {1, 70, 1, 0}}

    Property qpmin As New NumParam With {
        .Switch = "--qpmin",
        .Text = "QP Minimum",
        .Init = 0,
        .Config = {0, 68, 1, 0}}

    Property qpmax As New NumParam With {
        .Switch = "--qpmax",
        .Text = "QP Maximum",
        .Init = 69,
        .Config = {1, 69, 1, 0}}

    Property TUintra As New NumParam With {
        .Switch = "--tu-intra-depth",
        .Text = "TU Intra Depth",
        .Config = {1, 4}}

    Property TUinter As New NumParam With {
        .Switch = "--tu-inter-depth",
        .Text = "TU Inter Depth",
        .Config = {1, 4}}

    Property rdoqLevel As New NumParam With {
        .Switch = "--rdoq-level",
        .Switches = {"--rdoq"},
        .Text = "RDOQ Level",
        .Config = {0, 2}}

    Property Rect As New BoolParam With {
        .Switch = "--rect",
        .NoSwitch = "--no-rect",
        .Text = "Enable analysis of rectangular motion partitions Nx2N and 2NxN"}

    Property RcGrain As New BoolParam With {
        .Switch = "--rc-grain",
        .NoSwitch = "--no-rc-grain",
        .Text = "Specialised ratecontrol for film grain content"}

    Property AMP As New BoolParam With {
        .Switch = "--amp",
        .NoSwitch = "--no-amp",
        .Text = "Enable analysis of asymmetric motion partitions"}

    Property EarlySkip As New BoolParam With {
        .Switch = "--early-skip",
        .NoSwitch = "--no-early-skip",
        .Text = "Early Skip"}

    Property FastIntra As New BoolParam With {
        .Switch = "--fast-intra",
        .NoSwitch = "--no-fast-intra",
        .Text = "Fast Intra"}

    Property BIntra As New BoolParam With {
        .Switch = "--b-intra",
        .NoSwitch = "--no-b-intra",
        .Text = "Evaluate intra modes in B slices"}

    Property CUlossless As New BoolParam With {
        .Switch = "--cu-lossless",
        .Text = "CU Lossless"}

    Property TSkip As New BoolParam With {
        .Switch = "--tskip",
        .NoSwitch = "--no-tskip",
        .Text = "Enable evaluation of transform skip coding for 4x4 TU coded blocks"}

    Property TSkipFast As New BoolParam With {
        .Switch = "--tskip-fast",
        .NoSwitch = "--no-tskip-fast",
        .Text = "Only evaluate transform skip for NxN intra predictions (4x4 blocks)"}

    Property PsyRD As New NumParam With {
        .Switch = "--psy-rd",
        .Text = "Psy RD",
        .Config = {0, 5, 0.05, 2}}

    Property PsyRDOQ As New NumParam With {
        .Switch = "--psy-rdoq",
        .Text = "Psy RDOQ",
        .Config = {0, 50, 0.05, 2}}

    Property QPadaptationrange As New NumParam With {
        .Switch = "--qp-adaptation-range",
        .Text = "QP Adaptation Range",
        .Init = 1.0,
        .Config = {1, 6, 0.05, 2}}

    Property PBRatio As New NumParam With {
        .Switch = "--pbratio",
        .Switches = {"--pb-factor"},
        .Text = "PB Ratio",
        .Config = {0, 1000, 0.05, 2}}

    Property IPRatio As New NumParam With {
        .Switch = "--ipratio",
        .Switches = {"--ip-factor"},
        .Text = "IP Ratio",
        .Config = {0, 1000, 0.05, 2}}

    Property QComp As New NumParam With {
        .Switch = "--qcomp",
        .Text = "QComp",
        .Config = {0.5, 1, 0.01, 2}}

    Property WPP As New BoolParam With {
        .Switch = "--wpp",
        .NoSwitch = "--no-wpp",
        .Text = "Wavefront Parallel Processing",
        .Init = True}

    Property minLuma As New NumParam With {
        .Switch = "--min-luma",
        .Text = "Minimum Luma"}

    Property maxLuma As New NumParam With {
        .Switch = "--max-luma",
        .Text = "Maximum Luma"}

    Property OutputDepth As New OptionParam With {
        .Switch = "--output-depth",
        .Switches = {"-D"},
        .Text = "Depth",
        .Options = {"8-Bit", "10-Bit", "12-Bit"},
        .Values = {"8", "10", "12"},
        .Value = 1}

    Property Hash As New OptionParam With {
        .Switch = "--hash",
        .Text = "Hash",
        .Expanded = True,
        .IntegerValue = True,
        .Options = {"None", "MD5", "CRC", "Checksum"}}

    Property TemporalMVP As New BoolParam With {
        .Switch = "--temporal-mvp",
        .NoSwitch = "--no-temporal-mvp",
        .Text = "Enable temporal motion vector predictors in P and B slices",
        .Init = True}

    Property StrongIntraSmoothing As New BoolParam With {
        .Switch = "--strong-intra-smoothing",
        .NoSwitch = "--no-strong-intra-smoothing",
        .Text = "Enable strong intra smoothing for 32x32 intra blocks",
        .Init = True}

    Property RDpenalty As New NumParam With {
        .Switch = "--rdpenalty",
        .Text = "RD Penalty",
        .Config = {0, 2}}

    Property OpenGop As New BoolParam With {
        .Switch = "--open-gop",
        .NoSwitch = "--no-open-gop",
        .Text = "Open GOP",
        .Init = True}

    Property Bpyramid As New BoolParam With {
        .Switch = "--b-pyramid",
        .NoSwitch = "--no-b-pyramid",
        .Text = "B-Pyramid",
        .Init = True}

    Property NRintra As New NumParam With {
        .Switch = "--nr-intra",
        .Text = "Intra Noise Reduct.",
        .Config = {0, 2000, 50}}

    Property NRinter As New NumParam With {
        .Switch = "--nr-inter",
        .Text = "Inter Noise Reduct.",
        .Config = {0, 2000, 50}}

    Property Keyint As New NumParam With {
        .Switch = "--keyint",
        .Switches = {"-I"},
        .Text = "Max GOP Size",
        .Config = {0, 10000, 10},
        .Init = 250}

    Property MinKeyint As New NumParam With {
        .Switch = "--min-keyint",
        .Switches = {"-i"},
        .Text = "Min GOP Size",
        .Config = {0, 10000, 10}}

    Property MaxAuSizeFactor As New NumParam With {
        .Switch = "--max-ausize-factor",
        .Text = "Max AU size",
        .Config = {0.5, 1.0, 0.1, 1},
        .Init = 1}

    Property Range As New OptionParam With {.Switch = "--range", .Text = "Range", .Options = {"Undefined", "Limited", "Full"}}
    Property ColorPrim As New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim", .Options = {"Undefined", "BT 2020", "BT 470 BG", "BT 470 M", "BT 709", "Film", "SMPTE 170 M", "SMPTE 240 M", "SMPTE 428", "SMPTE 431", "SMPTE 432"}}
    Property Transfer As New OptionParam With {.Switch = "--transfer", .Text = "Transfer", .Options = {"Undefined", "ARIB-STD-B67", "BT 1361 E", "BT 2020-10", "BT 2020-12", "BT 470 BG", "BT 470 M", "BT 709", "IEC 61966-2-1", "IEC 61966-2-4", "Linear", "Log 100", "Log 316", "SMPTE 170 M", "SMPTE 2084", "SMPTE 240 M", "SMPTE 428"}}
    Property ColorMatrix As New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix", .Options = {"Undefined", "BT 2020 C", "BT 2020 NC", "BT 470 BG", "BT 709", "Chroma-Derived-C", "Chroma-Derived-NC", "FCC", "GBR", "ICTCP", "SMPTE 170 M", "SMPTE 2085", "SMPTE 240 M", "YCgCo"}}
    Property AtcSei As New NumParam With {.Switch = "--atc-sei", .Text = "Alternative transfer SEI:", .Init = -1, .Config = {-1, 99, 1}}

    Property Chromaloc As New OptionParam With {
        .Switch = "--chromaloc",
        .Text = "Chromaloc",
        .Options = {"Undefined", "0", "1", "2", "3", "4", "5"},
        .Values = {"", "0", "1", "2", "3", "4", "5"},
        .Init = 0}

    Property Hdr10 As New OptionParam With {
        .Switch = "--hdr10",
        .NoSwitch = "--no-hdr10",
        .Text = "HDR10",
        .Options = {"Undefined", "Yes", "No"},
        .Values = {"", "--hdr10", "--no-hdr10"},
        .ValueChangedAction = Sub(x)
                                  If x = 1 Then
                                      Chromaloc.Value = 3
                                  ElseIf x = 2 Then
                                      Chromaloc.Value = 0
                                  End If
                              End Sub}

    Property DhdrInfo As New StringParam With {
        .Switch = "--dhdr10-info",
        .Text = "HDR10 Info File",
        .BrowseFile = True}

    Property DolbyVisionProfile As New OptionParam With {
        .Switch = "--dolby-vision-profile",
        .Text = "Dolby Vision Profile",
        .Options = {"Undefined", "5", "8.1", "8.2", "8.4"},
        .ValueChangedAction = Sub(index As Integer)
                                  Select Case index
                                      Case 1
                                          ColorMatrix.ValueChangedUser(2)
                                          ColorPrim.ValueChangedUser(1)
                                          Transfer.ValueChangedUser(0)
                                          Range.ValueChangedUser(1)
                                          AtcSei.Value = AtcSei.InitialValue
                                      Case 2
                                          ColorMatrix.ValueChangedUser(2)
                                          ColorPrim.ValueChangedUser(1)
                                          Transfer.ValueChangedUser(14)
                                          Range.ValueChangedUser(1)
                                          AtcSei.Value = AtcSei.InitialValue
                                      Case 3
                                          ColorMatrix.ValueChangedUser(4)
                                          ColorPrim.ValueChangedUser(4)
                                          Transfer.ValueChangedUser(7)
                                          Range.ValueChangedUser(1)
                                          AtcSei.Value = AtcSei.InitialValue
                                      Case 4
                                          ColorMatrix.ValueChangedUser(2)
                                          ColorPrim.ValueChangedUser(1)
                                          Transfer.ValueChangedUser(1)
                                          Range.ValueChangedUser(1)
                                          AtcSei.Value = 1
                                      Case Else
                                  End Select
                              End Sub}

    Property DolbyVisionRpu As New StringParam With {
        .Switch = "--dolby-vision-rpu",
        .Text = "Dolby Vision RPU",
        .BrowseFile = True}

    Property FrameThreads As New NumParam With {
        .Switch = "--frame-threads",
        .Switches = {"-F"},
        .Text = "Frame Threads"}

    Property RepeatHeaders As New BoolParam With {
        .Switch = "--repeat-headers",
        .Text = "Repeat Headers"}

    Property Info As New BoolParam With {
        .Switch = "--info",
        .NoSwitch = "--no-info",
        .Text = "Info",
        .Init = True}

    Property HRD As New BoolParam With {
        .Switch = "--hrd",
        .Text = "HRD"}

    Property AUD As New BoolParam With {
        .Switch = "--aud",
        .Text = "AUD"}

    Property LimitModes As New BoolParam With {
        .Switch = "--limit-modes",
        .NoSwitch = "--no-limit-modes",
        .Text = "Limit Modes"}

    Property RdRefine As New BoolParam With {
        .Switch = "--rd-refine",
        .Text = "RD Refine"}

    Property MasterDisplay As New StringParam With {
        .Switch = "--master-display",
        .Text = "Master Display",
        .InitAction = Sub(tb)
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 2.3
                      End Sub}

    Property MaxCLL As New NumParam With {
        .Text = "Maximum CLL",
        .Switch = "--max-cll",
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
        .Config = {0, Integer.MaxValue, 50}}

    Property IntraRefresh As New BoolParam With {
        .Switch = "--intra-refresh",
        .Text = "Intra Refresh"}

    Property RefineCtuDistortion As New BoolParam With {
        .Switch = "--refine-ctu-distortion",
        .Text = "Store/normalize ctu distortion in analysis-save/load.",
        .IntegerValue = True}

    Property Custom As New StringParam With {
        .Text = "Custom",
        .Quotes = QuotesMode.Never,
        .AlwaysOn = True,
        .InitAction = Sub(tb)
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = FontManager.GetCodeFont()
                      End Sub}

    Property CustomFirstPass As New StringParam With {
        .Text = "Custom" + BR + "First Pass",
        .Quotes = QuotesMode.Never,
        .InitAction = Sub(tb)
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = FontManager.GetCodeFont()
                      End Sub}

    Property CustomLastPass As New StringParam With {
        .Text = "Custom" + BR + "Last Pass",
        .Quotes = QuotesMode.Never,
        .InitAction = Sub(tb)
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = FontManager.GetCodeFont()
                      End Sub}

    Property CustomNthPass As New StringParam With {
        .Text = "Custom" + BR + "Nth Pass",
        .Quotes = QuotesMode.Never,
        .InitAction = Sub(tb)
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = FontManager.GetCodeFont()
                      End Sub}

    Property Deblock As New BoolParam With {
        .Switch = "--deblock",
        .NoSwitch = "--no-deblock",
        .Text = "Deblocking",
        .ArgsFunc = Function() As String
                        If Deblock.Value Then
                            If DeblockA.Value = DeblockA.DefaultValue AndAlso
                                DeblockB.Value = DeblockB.DefaultValue AndAlso
                                Deblock.DefaultValue Then
                                Return ""
                            Else
                                Return "--deblock " & DeblockA.Value & ":" & DeblockB.Value
                            End If
                        ElseIf Deblock.DefaultValue Then
                            Return "--no-deblock"
                        Else
                            Return ""
                        End If
                    End Function,
        .ImportAction = Sub(param, arg)
                            If param.Equals(Deblock.NoSwitch, StringComparison.OrdinalIgnoreCase) Then
                                Deblock.Value = False
                            Else
                                Deblock.Value = True
                                Dim a = arg.Split(":"c)
                                If a.Length = 2 Then
                                    DeblockA.Value = a(0).ToInt
                                    DeblockB.Value = a(1).ToInt
                                End If
                            End If
                        End Sub}

    Property DeblockA As New NumParam With {
        .Text = "      Strength",
        .Config = {-6, 6}}

    Property DeblockB As New NumParam With {
        .Text = "      Threshold",
        .Config = {-6, 6}}

    Property MaxTuSize As New OptionParam With {
        .Switch = "--max-tu-size",
        .Text = "Max TU Size",
        .Options = {"32", "16", "8", "4"}}

    Property LimitRefs As New OptionParam With {
        .Switch = "--limit-refs",
        .Text = "Limit References",
        .Options = {"0", "1", "2", "3"},
        .Init = 3}

    Property MultiPassOptAnalysis As New BoolParam() With {
        .Switch = "--multi-pass-opt-analysis",
        .Text = "Multipass analysis refinement along with multipass ratecontrol"}

    Property MultiPassOptDistortion As New BoolParam() With {
        .Switch = "--multi-pass-opt-distortion",
        .Text = "Multipass refinement of qp based on distortion data"}

    Property LimitTU As New NumParam With {
        .Switch = "--limit-tu",
        .Text = "Limit TU",
        .Config = {0, 4}}

    Property VbvBufSize As New NumParam With {
        .Switch = "--vbv-bufsize",
        .Text = "VBV Bufsize",
        .Config = {0, 1000000, 100}}

    Property VbvMaxRate As New NumParam With {
        .Switch = "--vbv-maxrate",
        .Text = "VBV Maxrate",
        .Config = {0, 1000000, 100}}

    Property ConstVBV As New BoolParam With {
        .Switch = "--const-vbv",
        .NoSwitch = "--no-const-vbv",
        .Text = "Enable VBV algorithm to be consistent across runs"}

    Property Seek As New NumParam With {
        .HelpSwitch = "--seek",
        .Text = "Seek"}

    Property Frames As New NumParam With {
        .HelpSwitch = "--frames",
        .Switches = {"-f"},
        .Text = "Frames"}

    Property RSkip As New OptionParam With {
        .Switch = "--rskip",
        .Text = "Recursion Skip",
        .Options = {"0 - Disabled", "1 - RD Level 0-4 Neighbour costs and CU homogenity, RD Level 5-6 Comparison with inter2Nx2N", "2 - RD Level 0-6 CU edge denstiy"},
        .Values = {"0", "1", "2"},
        .Init = 1}

    Property ProgressReadframes As New BoolParam() With {
        .Switch = "--progress-readframes",
        .Text = "Progress Readframes",
        .Help = "Print how much frames retrieved from reader",
        .Weight = 9,
        .VisibleFunc = Function() Package.x265Type = x265Type.DJATOM}

    Property ScreenContentCoding As New OptionParam With {
        .Switch = "--scc",
        .Text = "Screen Content Coding",
        .Expanded = True,
        .IntegerValue = True,
        .Options = {"0 - Disabled", "1 - Intrablockcopy fast search with 1x2 CTUs search range", "2 - Intrablockcopy full search"},
        .Init = 0}

    Property Format As New OptionParam With {
        .Switch = "--format",
        .Text = "Format",
        .IntegerValue = True,
        .Options = {"Normal", "Side-by-Side", "Over-Under"},
        .Init = 0}


    Overrides ReadOnly Property Items As List(Of CommandLineParam)
        Get
            If ItemsValue Is Nothing Then
                ItemsValue = New List(Of CommandLineParam)

                Add("Input",
                    Decoder, PipingToolAVS, PipingToolVS,
                    New OptionParam With {.Switch = "--fps", .Text = "Frame Rate", .Options = {"Automatic", "24000/1001", "24", "25", "30000/1001", "30", "50", "60000/1001", "60"}},
                    New OptionParam With {.Switch = "--input-depth", .Text = "Input Depth", .Options = {"Automatic", "8", "10", "12", "14", "16"}},
                    New OptionParam With {.Switch = "--input-csp", .Text = "Input CSP", .Options = {"Automatic", "I400", "I420", "I422", "I444", "NV12", "NV16"}},
                    DhdrInfo,
                    New BoolParam With {.Switch = "--dhdr10-opt", .Text = "Limit frames for which tone mapping information is inserted as SEI message"},
                    DolbyVisionProfile, DolbyVisionRpu,
                    New StringParam With {.Switch = "--nalu-file", .Text = "Nalu File", .BrowseFile = True},
                    Seek, Frames,
                    New OptionParam With {.Switch = "--interlace", .Text = "Interlace", .Options = {"Progressive", "Top Field First", "Bottom Field First"}, .Values = {"", "tff", "bff"}},
                    New BoolParam With {.Switch = "--field", .NoSwitch = "--no-field", .Text = "Field Coding"},
                    New BoolParam With {.Switch = "--dither", .Text = "Dither (High Quality Downscaling)"},
                    New BoolParam With {.Switch = "--copy-pic", .NoSwitch = "--no-copy-pic", .Init = True, .Text = "Copy Pic"},
                    ProgressReadframes)
                Add("Output",
                    New OptionParam With {.Switch = "--log-level", .Text = "Logging Level", .Init = 5, .Options = {"None", "Error", "Warning", "Info", "Debug", "Full"}, .Values = {"-1", "0", "1", "2", "3", "4"}},
                    New StringParam With {.Switch = "--log-file", .Text = "Log File", .BrowseFile = True},
                    New OptionParam With {.Switch = "--log-file-level", .Text = "Log File Logging Level", .Init = 3, .Options = {"None", "Error", "Warning", "Info", "Debug", "Full"}, .Values = {"-1", "0", "1", "2", "3", "4"}},
                    New StringParam With {.Switch = "--csv", .Text = "CSV", .BrowseFile = True},
                    New OptionParam With {.Switch = "--csv-log-level", .Text = "CSV Logging Level", .IntegerValue = True, .Options = {"Default", "Summary", "Frame"}}
                )
                Add("Basic",
                    Mode, Preset, Tune, OutputDepth,
                    New OptionParam With {.Switch = "--profile", .Switches = {"-P"}, .Text = "Profile", .Name = "ProfileMain8", .VisibleFunc = Function() OutputDepth.Value = 0, .Options = {"Automatic", "Main", "Main - Intra", "Main Still Picture", "Main 444 - 8", "Main 444 - Intra", "Main 444 - Still Picture"}},
                    New OptionParam With {.Switch = "--profile", .Switches = {"-P"}, .Text = "Profile", .Name = "ProfileMain10", .VisibleFunc = Function() OutputDepth.Value = 1, .Options = {"Automatic", "Main 10", "Main 10 - Intra", "Main 422 - 10", "Main 422 - 10 - Intra", "Main 444 - 10", "Main 444 - 10 - Intra"}},
                    New OptionParam With {.Switch = "--profile", .Switches = {"-P"}, .Text = "Profile", .Name = "ProfileMain12", .VisibleFunc = Function() OutputDepth.Value = 2, .Options = {"Automatic", "Main 12", "Main 12 - Intra", "Main 422 - 12", "Main 422 - 12 - Intra", "Main 444 - 12", "Main 444 - 12 - Intra"}},
                    Level, HighTier, Quant, Bitrate,
                    New BoolParam With {.Switch = "--uhd-bd", .Text = "Ultra HD Blu-ray"},
                    New BoolParam With {.Switch = "--allow-non-conformance", .Text = "Allow non conformance"}
                )
                Add("Quad-Tree",
                    MaxCuSize, MinCuSize, MaxTuSize, TUintra, TUinter, LimitTU
                )
                Add("Analysis",
                    RD, PsyRD, rdoqLevel, PsyRDOQ,
                    New NumParam With {.Switch = "--dynamic-rd", .Text = "Dynamic RD", .Config = {0, 4}},
                    New BoolParam With {.Switch = "--ssim-rd", .Text = "SSIM RDO"},
                    RdRefine, EarlySkip, RSkip,
                    New NumParam With {.Switch = "--rskip-edge-threshold", .Text = "RSkip Edge Threshold", .Init = 5, .Config = {0, 100}, .VisibleFunc = Function() RSkip.Value = 2},
                    TSkipFast,
                    New BoolParam With {.Switch = "--splitrd-skip", .Text = "Enable skipping split RD analysis"},
                    NRintra, NRinter,
                    New OptionParam With {.Switch = "--ctu-info", .Text = "CTU Info", .Options = {"0", "1", "2", "4", "6"}}
                    )
                Add("Coding Tool",
                    Weightp, Weightb, CUlossless, SignHide, TSkip
                )
                Add("Temporal / Motion Search",
                    MaxMerge, Ref, LimitRefs, [Me], SubME, MErange,
                    Rect, AMP, LimitModes, TemporalMVP,
                    New BoolParam With {.Switch = "--hme", .NoSwitch = "--no-hme", .Text = "3-level Hierarchical Motion Estimation"},
                    New StringParam With {.Switch = "--hme-search", .Text = "HME Search"},
                    New StringParam With {.Switch = "--hme-range", .Text = "HME Range", .Init = "16,32,48", .Quotes = QuotesMode.Never, .RemoveSpace = True}
                )
                Add("Spatial / Intra",
                    StrongIntraSmoothing,
                    New BoolParam With {.Switch = "--constrained-intra", .NoSwitch = "--no-constrained-intra", .Switches = {"--cip"}, .Text = "Constrained Intra Prediction", .Init = False},
                    BIntra, FastIntra, RDpenalty
                )
                Add("Slice Decision",
                    OpenGop,
                    New BoolParam With {.Switch = "--cra-nal", .Text = "CRA to all frames", .VisibleFunc = Function() Keyint.Value = 1},
                    Keyint, MinKeyint,
                    New NumParam() With {.Switch = "--gop-lookahead", .Text = "Gop Lookahead"},
                    Scenecut,
                    New NumParam() With {.Switch = "--scenecut-bias", .Text = "Scenecut Bias", .Init = 5, .Config = {0, 100, 1, 1}},
                    New BoolParam() With {.Switch = "--hist-scenecut", .NoSwitch = "--no-hist-scenecut", .Text = "Scenecut detection using luma edge and chroma histograms"},
                    New BoolParam() With {.Switch = "--fades", .Text = "Detection and handling of fade-in regions"},
                    ScenecutAwareQp,
                    New NumParam() With {.Switch = "--radl", .Text = "Radl"},
                    IntraRefresh
                )
                Add("Slice Decision 2",
                    RCLookahead, LookaheadSlices,
                    New NumParam() With {.Switch = "--lookahead-threads", .Text = "Lookahead Threads"},
                    BFrames, BFrameBias, BAdapt, Bpyramid,
                    New BoolParam With {.Switch = "--hrd-concat", .Init = False, .Text = "HRD Concat"},
                    New StringParam With {.Switch = "--qpfile", .Text = "QP File", .BrowseFile = True},
                    New OptionParam() With {.Switch = "--force-flush", .Text = "Force Flush", .IntegerValue = True, .Options = {"Flush the encoder only when all the input pictures are over", "Flush all the frames even when the input is not over", "Flush the slicetype decided frames only"}}
                )
                Add("Rate Control",
                    CRFmin, CRFmax,
                    VbvBufSize, VbvMaxRate,
                    New NumParam With {.Switch = "--vbv-init", .Text = "VBV Init", .Config = {0.5, 1.0, 0.1, 1}, .Init = 0.9},
                    New NumParam With {.Switch = "--vbv-end", .Text = "VBV End", .Config = {0, 1.0, 0.1, 1}},
                    New NumParam With {.Switch = "--vbv-end-fr-adj", .Text = "VBV Adjust", .Config = {0, 1, 0.1, 1}},
                    New NumParam With {.Switch = "--min-vbv-fullness", .Text = "Min VBV Fullness", .Init = 50, .Config = {0, 100, 1, 1}},
                    New NumParam With {.Switch = "--max-vbv-fullness", .Text = "Max VBV Fullness", .Init = 80, .Config = {0, 100, 1, 1}},
                    chunkStart, chunkEnd,
                    New BoolParam With {.Switch = "--lossless", .Text = "Lossless"},
                    MultiPassOptAnalysis, MultiPassOptDistortion,
                    New BoolParam With {.Switch = "--vbv-live-multi-pass", .Text = "VBV Live Multi Pass"},
                    New BoolParam With {.Switch = "--analyze-src-pics", .NoSwitch = "--no-analyze-src-pics", .Text = "Analyze SRC Pics"}
                )
                Add("Rate Control 2",
                    Slowpass,
                    New BoolParam With {.Switch = "--strict-cbr", .Text = "Strict CBR"},
                    New StringParam With {.Switch = "--analysis-save", .Text = "Analysis Save", .BrowseFile = True},
                    New StringParam With {.Switch = "--analysis-load", .Text = "Analysis Load", .BrowseFile = True},
                    New StringParam With {.Switch = "--analysis-reuse-file", .Text = "Analysis Reuse File", .BrowseFile = True},
                    New OptionParam With {.Switch = "--analysis-save-reuse-level", .Text = "Save Reuse Level", .IntegerValue = True, .Options = {" 0 - Default", " 1 - Lookahead information", " 2 - Level 1 + intra/inter modes, ref's", " 3 - Level 1 + intra/inter modes, ref's", " 4 - Level 1 + intra/inter modes, ref's", " 5 - Level 2 + rect-amp", " 6 - Level 2 + rect-amp", " 7 - Level 5 + AVC size CU refinement", " 8 - Level 5 + AVC size Full CU analysis-info", " 9 - Level 5 + AVC size Full CU analysis-info", "10 - Level 5 + Full CU analysis-info"}},
                    New OptionParam With {.Switch = "--analysis-load-reuse-level", .Text = "Load Reuse Level", .IntegerValue = True, .Options = {" 0 - Default", " 1 - Lookahead information", " 2 - Level 1 + intra/inter modes, ref's", " 3 - Level 1 + intra/inter modes, ref's", " 4 - Level 1 + intra/inter modes, ref's", " 5 - Level 2 + rect-amp", " 6 - Level 2 + rect-amp", " 7 - Level 5 + AVC size CU refinement", " 8 - Level 5 + AVC size Full CU analysis-info", " 9 - Level 5 + AVC size Full CU analysis-info", "10 - Level 5 + Full CU analysis-info"}},
                    New StringParam With {.Switch = "--refine-analysis-type", .Text = "Refine Analysis Type"},
                    New NumParam With {.Switch = "--scale-factor", .Text = "Scale Factor"},
                    New NumParam With {.Switch = "--refine-intra", .Text = "Refine Intra", .Config = {0, 4}},
                    New NumParam With {.Switch = "--refine-inter", .Text = "Refine Inter", .Config = {0, 3}},
                    New BoolParam With {.Switch = "--dynamic-refine", .Text = "Dynamic Refine"},
                    New OptionParam With {.Switch = "--refine-mv", .Text = "Refine MV", .IntegerValue = True, .Options = {"Disabled", "Level 1: Search around scaled MV", "Level 2: Level 1 + Search around best AMVP cand", "Level 3: Level 2 + Search around the other AMVP cand"}},
                    RefineCtuDistortion
                )
                Add("Rate Control 3",
                    AQmode,
                    New BoolParam With {.Switch = "--hevc-aq", .Text = "Mode for HEVC Adaptive Quantization", .Init = False},
                    AQStrength, AQBiasStrength, QPadaptationrange,
                    New BoolParam With {.Switch = "--aq-motion", .Text = "AQ Motion"},
                    New BoolParam With {.Switch = "--sbrc", .NoSwitch = "--no-sbrc", .Text = "Segment based rate control"},
                    qgSize, CUtree, RcGrain, IPRatio, PBRatio, QComp, qpmin, qpmax, qpstep
                )
                Add("Rate Control 4",
                    ConstVBV,
                    New NumParam With {.Switch = "--cbqpoffs", .Text = "CB QP Offset", .Config = {-12, 12}},
                    New NumParam With {.Switch = "--crqpoffs", .Text = "CR QP Offset", .Config = {-12, 12}},
                    New NumParam With {.Switch = "--qblur", .Text = "Temporally Blur Quants", .Config = {0, 1, 0.05, 2}, .Init = 0.5},
                    New NumParam With {.Switch = "--cplxblur", .Text = "Temporally Blur Complexity", .Config = {0, 100, 0.5, 1}, .Init = 20},
                    New StringParam With {.Switch = "--scaling-list", .Text = "Scaling List"},
                    New StringParam With {.Switch = "--zones", .Text = "Zones"},
                    New StringParam With {.Switch = "--zonefile", .Text = "Zone File", .BrowseFile = True},
                    New StringParam With {.Switch = "--lambda-file", .Text = "Lambda File", .BrowseFile = True},
                    MaxAuSizeFactor
                )
                Add("Loop Filter",
                    Deblock, DeblockA, DeblockB, SAO, SAOnonDeblock,
                    New BoolParam With {.Switch = "--limit-sao", .Text = "Limit Sample Adaptive Offset"},
                    New OptionParam With {.Switch = "--selective-sao", .Text = "Selective SAO", .Init = 0, .IntegerValue = True, .Options = {"0 - Disable SAO for all slices", "1 - Enable SAO only for I-slices", "2 - Enable SAO for I-slices & P-slices", "3 - Enable SAO for all reference slices", "4 - Enable SAO for all slices"}}
                )
                Add("VUI",
                    New StringParam With {.Switch = "--sar", .Text = "Sample Aspect Ratio", .Init = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                    New OptionParam With {.Switch = "--display-window", .Text = "Display Window", .Options = {"Undefined", "Left", "Top", "Right", "Bottom"}},
                    New OptionParam With {.Switch = "--overscan", .Text = "Overscan", .Options = {"Undefined", "Show", "Crop"}},
                    New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat", .Options = {"Undefined", "Component", "PAL", "NTSC", "SECAM", "MAC"}},
                    Range, ColorPrim, Transfer, ColorMatrix, Chromaloc
                )
                Add("VUI 2",
                    MasterDisplay, MaxCLL, MaxFALL,
                    New BoolParam With {.Switch = "--cll", .NoSwitch = "--no-cll", .Text = "Emit content light level info SEI", .Init = True},
                    Hdr10,
                    New BoolParam With {.Switch = "--hdr10-opt", .NoSwitch = "--no-hdr10-opt", .Text = "Block-level luma and chroma QP optimization for HDR10 content"},
                    minLuma, maxLuma
                )
                Add("Bitstream",
                    RepeatHeaders, Info, HRD,
                    New BoolParam With {.Switch = "--idr-recovery-sei", .Init = False, .Text = "Recovery SEI"},
                    New BoolParam With {.Switch = "--single-sei", .Init = False, .Text = "Single SEI"},
                    New BoolParam With {.Switch = "--temporal-layers", .Text = "Temporal Layers"},
                    AUD,
                    New BoolParam With {.Switch = "--eob", .NoSwitch = "--no-eob", .Text = "Emit end of bitstream nal unit"},
                    New BoolParam With {.Switch = "--eos", .NoSwitch = "--no-eos", .Text = "Emit end of sequence nal unit"}
                )
                Add("Bitstream 2",
                    Hash, AtcSei,
                    New NumParam With {.Switch = "--pic-struct", .Text = "Picture structure in SEI:", .Init = -1, .Config = {-1, 12, 1}},
                    New NumParam With {.Switch = "--log2-max-poc-lsb", .Text = "Maximum Picture Order Count", .Init = 8},
                    New BoolParam With {.Switch = "--vui-timing-info", .NoSwitch = "--no-vui-timing-info", .Text = "VUI Timing Info", .Init = True},
                    New BoolParam With {.Switch = "--vui-hrd-info", .NoSwitch = "--no-vui-hrd-info", .Text = "VUI HRD Info", .Init = True},
                    New BoolParam With {.Switch = "--opt-qp-pps", .Init = False, .Text = "Optimize QP in PPS"},
                    New BoolParam With {.Switch = "--opt-ref-list-length-pps", .Init = False, .Text = "Optimize L0 and L1 Ref List Length in PPS"},
                    New BoolParam With {.Switch = "--multi-pass-opt-rps", .Init = False, .Text = "Enable Storing", .Help = "Enable Storing commonly used RPS in SPS in multi pass mode"},
                    New BoolParam With {.Switch = "--opt-cu-delta-qp", .Text = "Optimize CU level QPs", .Help = "Optimize CU level QPs pulling up lower QPs close to meanQP", .Init = False}
                )
                Add("Statistic",
                    New BoolParam With {.Switch = "--ssim", .Text = "SSIM"},
                    New BoolParam With {.Switch = "--psnr", .Text = "PSNR"}
                )
                Add("Reconstructed Video",
                    New StringParam With {.Switch = "--recon", .Switches = {"-r"}, .Text = "Recon File", .BrowseFile = True},
                    New NumParam With {.Switch = "--recon-depth", .Text = "Recon Depth"},
                    New BoolParam With {.Switch = "--lowpass-dct", .Text = "Lowpass DCT"},
                    New BoolParam With {.Switch = "--frame-dup", .Text = "Frame Duplication"},
                    New NumParam With {.Switch = "--dup-threshold", .Text = "PSNR Threshold", .Init = 70, .Config = {1, 99}},
                    New BoolParam With {.Switch = "--alpha", .Text = "Alpha channel support", .Help = "Enable alpha channel support", .Init = False, .IntegerValue = True},
                    Format, ScreenContentCoding,
                    New StringParam With {.Switch = "--abr-ladder", .Text = "ABR Ladder File", .BrowseFile = True}
                )
                Add("Performance",
                    New StringParam With {.Switch = "--asm", .NoSwitch = "--no-asm", .Text = "ASM"},
                    New StringParam With {.Switch = "--pools", .Text = "Pools"},
                    New NumParam With {.Switch = "--slices", .Text = "Slices", .Init = 1},
                    FrameThreads, WPP
                )
                Add("Custom",
                    Custom, CustomFirstPass, CustomLastPass, CustomNthPass
                )
                Add("Other",
                    OverrideTargetFileName, TargetFileName, TargetFileNamePreview,
                    New LineParam(),
                    Chunks,
                    CompCheck, CompCheckAimedQuality
                )

                ItemsValue = ItemsValue.OrderBy(Function(i) i.Weight).ToList
            End If

            Return ItemsValue
        End Get
    End Property

    Overrides Function GetCommandLinePreview() As String
        Dim ret = GetCommandLine(True, True)

        If Mode.Value = x265RateMode.TwoPass Then
            ret += BR2 + GetCommandLine(True, True, 2)
        ElseIf Mode.Value = x265RateMode.ThreePass Then
            'Specific order 1 > 3 > 2 is correct!
            ret += BR2 + GetCommandLine(True, True, 3)
            ret += BR2 + GetCommandLine(True, True, 2)
        End If

        Return ret
    End Function

    Public Overrides Sub ShowHelp(options As String())
        Using td As New TaskDialog(Of String)
            td.Title = String.Join(", ", options)
            td.AddCommand("Online Help")
            td.AddCommand("Console Help")

            Select Case td.Show
                Case "Online Help"
                    Dim ret = GetHelpOption(options)

                    If ret <> "" Then
                        g.ShellExecute("https://x265.readthedocs.io/en/latest/cli.html#cmdoption-" + ret.TrimStart("-"c))
                    End If
                Case "Console Help"
                    ShowConsoleHelp(Package.x265, options)
            End Select
        End Using
    End Sub

    Private BlockValueChanged As Boolean

    Protected Overrides Sub OnValueChanged(item As CommandLineParam)
        If BlockValueChanged Then Exit Sub

        If item Is Preset Then
            BlockValueChanged = True
            ApplyPresetValues()
            ApplyTuneValues()
            BlockValueChanged = False
        End If

        If item Is Tune Then
            BlockValueChanged = True
            ApplyTuneValues()
            BlockValueChanged = False
        End If

        If DeblockA.NumEdit IsNot Nothing Then
            If DeblockA.NumEdit IsNot Nothing Then
                DeblockA.NumEdit.Enabled = Deblock.Value
            End If

            If DeblockB.NumEdit IsNot Nothing Then
                DeblockB.NumEdit.Enabled = Deblock.Value
            End If
        End If

        If item IsNot TargetFileName AndAlso item IsNot TargetFileNamePreview Then TargetFileName.TextChangedAction?.Invoke(TargetFileName.Value)

        MyBase.OnValueChanged(item)
    End Sub

    Overloads Overrides Function GetCommandLine(includePaths As Boolean, includeExecutable As Boolean, Optional pass As Integer = 1) As String
        Return GetArgs(pass, 0, 0, Nothing, p.Script, p.VideoEncoder.OutputPath.DirAndBase + p.VideoEncoder.OutputExtFull, includePaths, includeExecutable)
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

        ApplyPresetDefaultValues()
        ApplyTuneDefaultValues()

        Dim sb As New StringBuilder
        Dim pipeTool = If(p.Script.IsAviSynth, PipingToolAVS, PipingToolVS).ValueText.ToLowerInvariant()
        Dim isSingleChunk = endFrame = 0

        If includePaths AndAlso includeExecutable Then
            Dim isCropped = (p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 AndAlso
                Decoder.ValueText <> "avs" AndAlso p.Script.IsFilterActive("Crop")

            Select Case Decoder.ValueText.ToLowerInvariant()
                Case "script"
                    Dim pipeString = ""

                    'DJATOM/Asuna/Patman have broken ANSI fallback
                    If pipeTool = "automatic" AndAlso (Package.x265Type = x265Type.Vanilla OrElse
                        (p.Script.IsAviSynth AndAlso Not TextEncoding.IsProcessUTF8 AndAlso
                        Not TextEncoding.ArePathsSupportedByASCIIEncoding)) Then

                        pipeTool = If(p.Script.IsAviSynth, "avs2pipemod", "vspipe")
                    End If

                    Select Case pipeTool
                        Case "avs2pipemod"
                            Dim chunk = If(isSingleChunk, "", $" -trim={startFrame},{endFrame}")
                            Dim dll = If(FrameServerHelp.IsPortable, $" -dll={Package.AviSynth.Path.Escape}", "")
                            pipeString = Package.avs2pipemod.Path.Escape + dll + chunk + " -y4mp " + script.Path.Escape + " | "
                            sb.Append(pipeString + Package.x265.Path.Escape)

                            If isSingleChunk Then
                                If Seek.Value > 0 Then
                                    sb.Append($" --seek {Seek.Value}")
                                End If

                                If Frames.Value = 0 Then
                                    sb.Append($" --frames {script.GetFrameCount - Seek.Value}")
                                Else
                                    sb.Append($" --frames {Frames.Value}")
                                End If
                            Else
                                sb.Append($" --frames {endFrame - startFrame + 1}")
                            End If
                        Case "vspipe"
                            Dim chunk = If(isSingleChunk, "", $" --start {startFrame} --end {endFrame}")
                            pipeString = Package.vspipe.Path.Escape + " " + script.Path.Escape + " - --container y4m" + chunk + " | "

                            sb.Append(pipeString + Package.x265.Path.Escape)
                            If isSingleChunk Then
                                If Seek.Value > 0 Then
                                    sb.Append($" --seek {Seek.Value}")
                                End If
                                If Frames.Value = 0 Then
                                    sb.Append($" --frames {script.GetFrameCount - Seek.Value}")
                                Else
                                    sb.Append($" --frames {Frames.Value}")
                                End If
                            Else
                                sb.Append($" --frames {endFrame - startFrame + 1}")
                            End If
                        Case "ffmpeg"
                            pipeString = Package.ffmpeg.Path.Escape + If(p.Script.IsVapourSynth, " -f vapoursynth", "") + " -i " + script.Path.Escape + " -f yuv4mpegpipe -strict -1 -loglevel fatal -hide_banner - | "

                            sb.Append(pipeString + Package.x265.Path.Escape)

                            If isSingleChunk Then
                                If Seek.Value > 0 Then
                                    sb.Append($" --seek {Seek.Value}")
                                End If

                                If Frames.Value = 0 Then
                                    sb.Append($" --frames {script.GetFrameCount - Seek.Value}")
                                Else
                                    sb.Append($" --frames {Frames.Value}")
                                End If
                            Else
                                sb.Append($" --seek {startFrame} --frames {endFrame - startFrame + 1}")
                            End If
                        Case "automatic"
                            sb.Append(pipeString + Package.x265.Path.Escape)
                            Dim type = Package.x265Type

                            If FrameServerHelp.IsPortable AndAlso type <> x265Type.Vanilla Then
                                sb.Append(" --reader-options library=" + FrameServerHelp.GetSynthPath.Escape)
                            End If

                            If isSingleChunk Then
                                If Seek.Value > 0 Then
                                    sb.Append($" --seek {Seek.Value}")
                                End If

                                If Frames.Value > 0 Then
                                    sb.Append($" --frames {Frames.Value}")
                                End If
                            Else
                                sb.Append($" --seek {startFrame} --frames {endFrame - startFrame + 1}")
                            End If
                    End Select
                Case "qs"
                    Dim crop = If(isCropped, " --crop " & p.CropLeft & "," & p.CropTop & "," & p.CropRight & "," & p.CropBottom, "")
                    sb.Append(Package.QSVEncC.Path.Escape + " -o - -c raw" + crop + " -i " + p.SourceFile.Escape + " | " + Package.x265.Path.Escape)
                    If isSingleChunk Then
                        If Seek.Value > 0 Then
                            sb.Append($" --seek {Seek.Value}")
                        End If
                        If Frames.Value = 0 Then
                            sb.Append($" --frames {p.SourceFrames - Seek.Value}")
                        Else
                            sb.Append($" --frames {Frames.Value}")
                        End If
                    Else
                        sb.Append($" --seek {startFrame} --frames {endFrame - startFrame + 1}")
                    End If
                Case "ffqsv"
                    Dim crop = If(isCropped, $" -vf ""crop={p.SourceWidth - p.CropLeft - p.CropRight}:{p.SourceHeight - p.CropTop - p.CropBottom}:{p.CropLeft}:{p.CropTop}""", "")
                    sb.Append(Package.ffmpeg.Path.Escape + " -threads 1 -hwaccel qsv -i " + p.SourceFile.Escape + " -f yuv4mpegpipe -strict -1" + crop + " -loglevel fatal -hide_banner - | " + Package.x265.Path.Escape)
                    If isSingleChunk Then
                        If Seek.Value > 0 Then
                            sb.Append($" --seek {Seek.Value}")
                        End If
                        If Frames.Value = 0 Then
                            sb.Append($" --frames {p.SourceFrames - Seek.Value}")
                        Else
                            sb.Append($" --frames {Frames.Value}")
                        End If
                    Else
                        sb.Append($" --seek {startFrame} --frames {endFrame - startFrame + 1}")
                    End If
                Case "ffdxva"
                    Dim crop = If(isCropped, $" -vf ""crop={p.SourceWidth - p.CropLeft - p.CropRight}:{p.SourceHeight - p.CropTop - p.CropBottom}:{p.CropLeft}:{p.CropTop}""", "")
                    Dim pix_fmt = If(p.SourceVideoBitDepth = 10, "yuv420p10le", "yuv420p")
                    sb.Append(Package.ffmpeg.Path.Escape + " -threads 1 -hwaccel dxva2 -i " + p.SourceFile.Escape +
                              " -f yuv4mpegpipe -pix_fmt " + pix_fmt + " -strict -1" + crop +
                              " -loglevel fatal -hide_banner - | " + Package.x265.Path.Escape)
                    If isSingleChunk Then
                        If Seek.Value > 0 Then
                            sb.Append($" --seek {Seek.Value}")
                        End If
                        If Frames.Value = 0 Then
                            sb.Append($" --frames {p.SourceFrames - Seek.Value}")
                        Else
                            sb.Append($" --frames {Frames.Value}")
                        End If
                    Else
                        sb.Append($" --seek {startFrame} --frames {endFrame - startFrame + 1}")
                    End If
            End Select
        End If

        If Mode.Value = x265RateMode.TwoPass OrElse Mode.Value = x265RateMode.ThreePass Then
            sb.Append(" --pass " & pass)

            If pass = 1 Then
                If CustomFirstPass.Value <> "" Then
                    sb.Append(" " + CustomFirstPass.Value)
                End If
            ElseIf pass = 2 Then
                If ScenecutAwareQp.Value <> ScenecutAwareQp.DefaultValue Then
                    sb.Append(" --scenecut-aware-qp " + ScenecutAwareQp.Value.ToString())
                End If
                If CustomLastPass.Value <> "" Then
                    sb.Append(" " + CustomLastPass.Value)
                End If
            Else
                If CustomNthPass.Value <> "" Then
                    sb.Append(" " + CustomNthPass.Value)
                End If
            End If
        End If

        If includePaths AndAlso (MultiPassOptDistortion.Value OrElse MultiPassOptAnalysis.Value) Then
            sb.Append(" --analysis-reuse-file " + (targetPath.DirAndBase + chunkName + ".analysis").Escape)
        End If

        If Mode.Value = x265RateMode.SingleQuant Then
            If Not IsCustom(pass, "--qp") Then
                sb.Append(" --qp " + CInt(Quant.Value).ToString)
            End If
        ElseIf Mode.Value = x265RateMode.SingleCRF Then
            If Quant.Value <> Quant.DefaultValue AndAlso Not IsCustom(pass, "--crf") Then
                sb.Append(" --crf " + Quant.Value.ToInvariantString)
            End If
        Else
            If Not IsCustom(pass, "--bitrate") Then
                sb.Append(" --bitrate " & If(pass = 1, Bitrate.Value, p.VideoBitrate))
            End If
        End If

        Dim q = From i In Items Where i.GetArgs <> "" AndAlso Not IsCustom(pass, i.Switch)

        If q.Count > 0 Then
            sb.Append(" " + q.Select(Function(item) item.GetArgs).Join(" "))
        End If

        If includePaths Then
            If Decoder.Value <> 0 OrElse pipeTool <> "automatic" Then
                sb.Append(" --y4m")
            End If

            If Mode.Value = x265RateMode.TwoPass OrElse Mode.Value = x265RateMode.ThreePass Then
                sb.Append(" --stats " + (targetPath.DirAndBase + chunkName + ".stats").Escape)
            End If

            If (Mode.Value = x265RateMode.ThreePass AndAlso pass <> 2) OrElse (Mode.Value = x265RateMode.TwoPass AndAlso pass = 1) Then
                sb.Append(" --output NUL")
            Else
                sb.Append(" --output " + (targetPath.DirAndBase + chunkName + targetPath.ExtFull).Escape)
            End If

            sb.Append(" --input " + If(Decoder.Value = 0 AndAlso pipeTool = "automatic", script.Path.Escape, "-"))
        Else
            If Seek.Value > 0 AndAlso Not IsCustom(pass, "--seek") Then
                sb.Append(" --seek " & Seek.Value)
            End If

            If Frames.Value > 0 AndAlso Not IsCustom(pass, "--frames") Then
                sb.Append(" --frames " & Frames.Value)
            End If
        End If

        Return Macro.Expand(sb.ToString.Trim.FixBreak.Replace(BR, " "))
    End Function

    Function IsCustom(pass As Integer, switch As String) As Boolean
        If switch = "" Then
            Return False
        End If

        If Mode.Value = x265RateMode.TwoPass OrElse Mode.Value = x265RateMode.ThreePass Then
            If pass = 1 Then
                If CustomFirstPass.Value?.Contains(switch + " ") OrElse
                    CustomFirstPass.Value?.EndsWith(switch) Then

                    Return True
                End If
            ElseIf pass = 2 Then
                If CustomLastPass.Value?.Contains(switch + " ") OrElse
                    CustomLastPass.Value?.EndsWith(switch) Then

                    Return True
                End If
            Else
                If CustomNthPass.Value?.Contains(switch + " ") OrElse
                    CustomNthPass.Value?.EndsWith(switch) Then

                    Return True
                End If
            End If
        End If

        If Custom.Value?.Contains(switch + " ") OrElse Custom.Value?.EndsWith(switch) Then
            Return True
        End If

        Return False
    End Function

    Sub ApplyPresetValues()
        AQmode.Value = 2
        AQStrength.Value = 1
        AQBiasStrength.Value = 1
        CUtree.Value = True
        Deblock.Value = True
        DeblockA.Value = 0
        DeblockB.Value = 0
        FrameThreads.Value = 0
        IPRatio.Value = 1.4
        LimitModes.Value = False
        LimitRefs.Value = 1
        LimitTU.Value = 0
        LookaheadSlices.Value = 8
        MaxCuSize.Value = 0
        MErange.Value = 57
        MinCuSize.Value = 2
        PBRatio.Value = 1.3
        PsyRD.Value = 2.0
        PsyRDOQ.Value = 0
        QComp.Value = 0.6
        RSkip.Value = 1
        TSkip.Value = False

        Select Case Preset.Value
            Case 0 'ultrafast
                [Me].Value = 0
                AMP.Value = False
                AQmode.Value = 0
                AQStrength.Value = 0
                BAdapt.Value = 0
                BFrames.Value = 3
                BIntra.Value = False
                EarlySkip.Value = True
                FastIntra.Value = True
                LimitRefs.Value = 0
                MaxCuSize.Value = 1
                MaxMerge.Value = 2
                MinCuSize.Value = 1
                RCLookahead.Value = 5
                RD.Value = 2
                rdoqLevel.Value = 0
                Rect.Value = False
                Ref.Value = 1
                SAO.Value = False
                Scenecut.Value = 0
                SignHide.Value = False
                SubME.Value = 0
                TUinter.Value = 1
                TUintra.Value = 1
                Weightb.Value = False
                Weightp.Value = False
            Case 1 'superfast
                [Me].Value = 1
                AMP.Value = False
                AQmode.Value = 0
                AQStrength.Value = 0
                BAdapt.Value = 0
                BFrames.Value = 3
                BIntra.Value = False
                EarlySkip.Value = True
                FastIntra.Value = True
                LimitRefs.Value = 0
                MaxCuSize.Value = 1
                MaxMerge.Value = 2
                RCLookahead.Value = 10
                RD.Value = 2
                rdoqLevel.Value = 0
                Rect.Value = False
                Ref.Value = 1
                SAO.Value = False
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 1
                TUinter.Value = 1
                TUintra.Value = 1
                Weightb.Value = False
                Weightp.Value = False
            Case 2 'veryfast
                [Me].Value = 1
                AMP.Value = False
                BAdapt.Value = 0
                BFrames.Value = 4
                BIntra.Value = False
                EarlySkip.Value = True
                FastIntra.Value = True
                LimitRefs.Value = 3
                MaxMerge.Value = 2
                RCLookahead.Value = 15
                RD.Value = 2
                rdoqLevel.Value = 0
                Rect.Value = False
                Ref.Value = 2
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 1
                TUinter.Value = 1
                TUintra.Value = 1
                Weightb.Value = False
                Weightp.Value = True
            Case 3 'faster
                [Me].Value = 1
                AMP.Value = False
                BAdapt.Value = 0
                BFrames.Value = 4
                BIntra.Value = False
                EarlySkip.Value = True
                FastIntra.Value = True
                LimitRefs.Value = 3
                MaxMerge.Value = 2
                RCLookahead.Value = 15
                RD.Value = 2
                rdoqLevel.Value = 0
                Rect.Value = False
                Ref.Value = 2
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 2
                TUinter.Value = 1
                TUintra.Value = 1
                Weightb.Value = False
                Weightp.Value = True
            Case 4 'fast
                [Me].Value = 1
                AMP.Value = False
                BAdapt.Value = 0
                BFrames.Value = 4
                BIntra.Value = False
                EarlySkip.Value = False
                FastIntra.Value = True
                LimitRefs.Value = 3
                MaxMerge.Value = 2
                RCLookahead.Value = 15
                RD.Value = 2
                rdoqLevel.Value = 0
                Rect.Value = False
                Ref.Value = 3
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 2
                TUinter.Value = 1
                TUintra.Value = 1
                Weightb.Value = False
                Weightp.Value = True
            Case 5 'medium
                [Me].Value = 1
                AMP.Value = False
                BAdapt.Value = 2
                BFrames.Value = 4
                BIntra.Value = True
                EarlySkip.Value = True
                FastIntra.Value = False
                MaxMerge.Value = 3
                RCLookahead.Value = 20
                RD.Value = 3
                rdoqLevel.Value = 0
                Rect.Value = False
                Ref.Value = 3
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 2
                TUinter.Value = 1
                TUintra.Value = 1
                Weightb.Value = False
                Weightp.Value = True
            Case 6 'slow
                [Me].Value = 3
                AMP.Value = False
                BAdapt.Value = 2
                BFrames.Value = 4
                BIntra.Value = False
                EarlySkip.Value = False
                FastIntra.Value = False
                LimitModes.Value = True
                LimitRefs.Value = 3
                LookaheadSlices.Value = 4
                MaxMerge.Value = 3
                PsyRDOQ.Value = 1
                RCLookahead.Value = 25
                RD.Value = 4
                rdoqLevel.Value = 2
                Rect.Value = True
                Ref.Value = 4
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 3
                TUinter.Value = 1
                TUintra.Value = 1
                Weightb.Value = False
                Weightp.Value = True
            Case 7 'slower
                [Me].Value = 3
                AMP.Value = True
                BAdapt.Value = 2
                BFrames.Value = 8
                BIntra.Value = True
                EarlySkip.Value = False
                FastIntra.Value = False
                LimitModes.Value = True
                LimitTU.Value = 4
                LookaheadSlices.Value = 1
                MaxMerge.Value = 4
                PsyRDOQ.Value = 1
                RCLookahead.Value = 40
                RD.Value = 6
                rdoqLevel.Value = 2
                Rect.Value = True
                Ref.Value = 5
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 4
                TUinter.Value = 3
                TUintra.Value = 3
                Weightb.Value = True
                Weightp.Value = True
            Case 8 'veryslow
                [Me].Value = 3
                AMP.Value = True
                BAdapt.Value = 2
                BFrames.Value = 8
                BIntra.Value = True
                EarlySkip.Value = False
                FastIntra.Value = False
                LimitRefs.Value = 0
                LookaheadSlices.Value = 1
                MaxMerge.Value = 5
                PsyRDOQ.Value = 1
                RCLookahead.Value = 40
                RD.Value = 6
                rdoqLevel.Value = 2
                Rect.Value = True
                Ref.Value = 5
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 4
                TUinter.Value = 3
                TUintra.Value = 3
                Weightb.Value = True
                Weightp.Value = True
            Case 9 'placebo
                [Me].Value = 3
                AMP.Value = True
                BAdapt.Value = 2
                BFrames.Value = 8
                BIntra.Value = True
                EarlySkip.Value = False
                FastIntra.Value = False
                LimitRefs.Value = 0
                LookaheadSlices.Value = 1
                MaxMerge.Value = 5
                MErange.Value = 92
                RCLookahead.Value = 60
                RD.Value = 6
                rdoqLevel.Value = 2
                Rect.Value = True
                Ref.Value = 5
                RSkip.Value = 0
                SAO.Value = True
                Scenecut.Value = 40
                SignHide.Value = True
                SubME.Value = 5
                TSkip.Value = True
                TUinter.Value = 4
                TUintra.Value = 4
                Weightb.Value = True
                Weightp.Value = True
        End Select
    End Sub

    Sub ApplyPresetDefaultValues()
        AQmode.DefaultValue = 2
        AQStrength.DefaultValue = 1
        CUtree.DefaultValue = True
        Deblock.DefaultValue = True
        DeblockA.DefaultValue = 0
        DeblockB.DefaultValue = 0
        FrameThreads.DefaultValue = 0
        IPRatio.DefaultValue = 1.4
        LimitModes.DefaultValue = False
        LimitRefs.DefaultValue = 3
        LimitTU.DefaultValue = 0
        LookaheadSlices.DefaultValue = 8
        MaxCuSize.DefaultValue = 0
        MErange.DefaultValue = 57
        MinCuSize.DefaultValue = 2
        PBRatio.DefaultValue = 1.3
        PsyRD.DefaultValue = 2.0
        PsyRDOQ.DefaultValue = 0
        QComp.DefaultValue = 0.6
        RSkip.DefaultValue = 1
        TSkip.DefaultValue = False

        Select Case Preset.Value
            Case 0 'ultrafast
                [Me].DefaultValue = 0
                AMP.DefaultValue = False
                AQmode.DefaultValue = 0
                AQStrength.DefaultValue = 0
                BAdapt.DefaultValue = 0
                BFrames.DefaultValue = 3
                BIntra.DefaultValue = False
                EarlySkip.DefaultValue = True
                FastIntra.DefaultValue = True
                LimitRefs.DefaultValue = 0
                MaxCuSize.DefaultValue = 1
                MaxMerge.DefaultValue = 2
                MinCuSize.DefaultValue = 1
                RCLookahead.DefaultValue = 5
                RD.DefaultValue = 2
                rdoqLevel.DefaultValue = 0
                Rect.DefaultValue = False
                Ref.DefaultValue = 1
                SAO.DefaultValue = False
                Scenecut.DefaultValue = 0
                SignHide.DefaultValue = False
                SubME.DefaultValue = 0
                TUinter.DefaultValue = 1
                TUintra.DefaultValue = 1
                Weightb.DefaultValue = False
                Weightp.DefaultValue = False
            Case 1 'superfast
                [Me].DefaultValue = 1
                AMP.DefaultValue = False
                AQmode.DefaultValue = 0
                AQStrength.DefaultValue = 0
                BAdapt.DefaultValue = 0
                BFrames.DefaultValue = 3
                BIntra.DefaultValue = False
                EarlySkip.DefaultValue = True
                FastIntra.DefaultValue = True
                LimitRefs.DefaultValue = 0
                MaxCuSize.DefaultValue = 1
                MaxMerge.DefaultValue = 2
                RCLookahead.DefaultValue = 10
                RD.DefaultValue = 2
                rdoqLevel.DefaultValue = 0
                Rect.DefaultValue = False
                Ref.DefaultValue = 1
                SAO.DefaultValue = False
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 1
                TUinter.DefaultValue = 1
                TUintra.DefaultValue = 1
                Weightb.DefaultValue = False
                Weightp.DefaultValue = False
            Case 2 'veryfast
                [Me].DefaultValue = 1
                AMP.DefaultValue = False
                BAdapt.DefaultValue = 0
                BFrames.DefaultValue = 4
                BIntra.DefaultValue = False
                EarlySkip.DefaultValue = True
                FastIntra.DefaultValue = True
                MaxMerge.DefaultValue = 2
                RCLookahead.DefaultValue = 15
                RD.DefaultValue = 2
                rdoqLevel.DefaultValue = 0
                Rect.DefaultValue = False
                Ref.DefaultValue = 2
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 1
                TUinter.DefaultValue = 1
                TUintra.DefaultValue = 1
                Weightb.DefaultValue = False
                Weightp.DefaultValue = True
            Case 3 'faster
                [Me].DefaultValue = 1
                AMP.DefaultValue = False
                BAdapt.DefaultValue = 0
                BFrames.DefaultValue = 4
                BIntra.DefaultValue = False
                EarlySkip.DefaultValue = True
                FastIntra.DefaultValue = True
                MaxMerge.DefaultValue = 2
                RCLookahead.DefaultValue = 15
                RD.DefaultValue = 2
                rdoqLevel.DefaultValue = 0
                Rect.DefaultValue = False
                Ref.DefaultValue = 2
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 2
                TUinter.DefaultValue = 1
                TUintra.DefaultValue = 1
                Weightb.DefaultValue = False
                Weightp.DefaultValue = True
            Case 4 'fast
                [Me].DefaultValue = 1
                AMP.DefaultValue = False
                BAdapt.DefaultValue = 0
                BFrames.DefaultValue = 4
                BIntra.DefaultValue = False
                EarlySkip.DefaultValue = False
                FastIntra.DefaultValue = True
                MaxMerge.DefaultValue = 2
                RCLookahead.DefaultValue = 15
                RD.DefaultValue = 2
                rdoqLevel.DefaultValue = 0
                Rect.DefaultValue = False
                Ref.DefaultValue = 3
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 2
                TUinter.DefaultValue = 1
                TUintra.DefaultValue = 1
                Weightb.DefaultValue = False
                Weightp.DefaultValue = True
            Case 5 'medium
                [Me].DefaultValue = 1
                AMP.DefaultValue = False
                BAdapt.DefaultValue = 2
                BFrames.DefaultValue = 4
                BIntra.DefaultValue = True
                EarlySkip.DefaultValue = True
                FastIntra.DefaultValue = False
                LimitRefs.DefaultValue = 1
                MaxMerge.DefaultValue = 3
                RCLookahead.DefaultValue = 20
                RD.DefaultValue = 3
                rdoqLevel.DefaultValue = 0
                Rect.DefaultValue = False
                Ref.DefaultValue = 3
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 2
                TUinter.DefaultValue = 1
                TUintra.DefaultValue = 1
                Weightb.DefaultValue = False
                Weightp.DefaultValue = True
            Case 6 'slow
                [Me].DefaultValue = 3
                AMP.DefaultValue = False
                BAdapt.DefaultValue = 2
                BFrames.DefaultValue = 4
                BIntra.DefaultValue = False
                EarlySkip.DefaultValue = False
                FastIntra.DefaultValue = False
                LimitModes.DefaultValue = True
                LookaheadSlices.DefaultValue = 4
                MaxMerge.DefaultValue = 3
                PsyRDOQ.DefaultValue = 1
                RCLookahead.DefaultValue = 25
                RD.DefaultValue = 4
                rdoqLevel.DefaultValue = 2
                Rect.DefaultValue = True
                Ref.DefaultValue = 4
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 3
                TUinter.DefaultValue = 1
                TUintra.DefaultValue = 1
                Weightb.DefaultValue = False
                Weightp.DefaultValue = True
            Case 7 'slower
                [Me].DefaultValue = 3
                AMP.DefaultValue = True
                BAdapt.DefaultValue = 2
                BFrames.DefaultValue = 8
                BIntra.DefaultValue = True
                EarlySkip.DefaultValue = False
                FastIntra.DefaultValue = False
                LimitModes.DefaultValue = True
                LimitRefs.DefaultValue = 1
                LimitTU.DefaultValue = 4
                LookaheadSlices.DefaultValue = 1
                MaxMerge.DefaultValue = 4
                PsyRDOQ.DefaultValue = 1
                RCLookahead.DefaultValue = 40
                RD.DefaultValue = 6
                rdoqLevel.DefaultValue = 2
                Rect.DefaultValue = True
                Ref.DefaultValue = 5
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 4
                TUinter.DefaultValue = 3
                TUintra.DefaultValue = 3
                Weightb.DefaultValue = True
                Weightp.DefaultValue = True
            Case 8 'veryslow
                [Me].DefaultValue = 3
                AMP.DefaultValue = True
                BAdapt.DefaultValue = 2
                BFrames.DefaultValue = 8
                BIntra.DefaultValue = True
                EarlySkip.DefaultValue = False
                FastIntra.DefaultValue = False
                LimitRefs.DefaultValue = 0
                LookaheadSlices.DefaultValue = 1
                MaxMerge.DefaultValue = 5
                PsyRDOQ.DefaultValue = 1
                RCLookahead.DefaultValue = 40
                RD.DefaultValue = 6
                rdoqLevel.DefaultValue = 2
                Rect.DefaultValue = True
                Ref.DefaultValue = 5
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 4
                TUinter.DefaultValue = 3
                TUintra.DefaultValue = 3
                Weightb.DefaultValue = True
                Weightp.DefaultValue = True
            Case 9 'placebo
                [Me].DefaultValue = 3
                AMP.DefaultValue = True
                BAdapt.DefaultValue = 2
                BFrames.DefaultValue = 8
                BIntra.DefaultValue = True
                EarlySkip.DefaultValue = False
                FastIntra.DefaultValue = False
                LimitRefs.DefaultValue = 0
                LookaheadSlices.DefaultValue = 1
                MaxMerge.DefaultValue = 5
                MErange.DefaultValue = 92
                RCLookahead.DefaultValue = 60
                RD.DefaultValue = 6
                rdoqLevel.DefaultValue = 2
                Rect.DefaultValue = True
                Ref.DefaultValue = 5
                RSkip.DefaultValue = 0
                SAO.DefaultValue = True
                Scenecut.DefaultValue = 40
                SignHide.DefaultValue = True
                SubME.DefaultValue = 5
                TUinter.DefaultValue = 4
                TUintra.DefaultValue = 4
                TSkip.DefaultValue = True
                Weightb.DefaultValue = True
                Weightp.DefaultValue = True
        End Select
    End Sub

    Sub ApplyTuneValues()
        AQBiasStrength.Value = 1
        RcGrain.Value = False
        qpstep.Value = 4
        ConstVBV.Value = False

        Select Case Tune.Value
            Case 1 'psnr
                AQStrength.Value = 0.0
                PsyRD.Value = 0.0
                PsyRDOQ.Value = 0.0
                CUtree.Value = False
            Case 2 'ssim
                AQmode.Value = 2
                PsyRD.Value = 0.0
                PsyRDOQ.Value = 0.0
            Case 3 'grain
                IPRatio.Value = 1.1
                PBRatio.Value = 1
                CUtree.Value = False
                AQmode.Value = 0
                qpstep.Value = 1
                RcGrain.Value = True
                PsyRD.Value = 4
                PsyRDOQ.Value = 10
                RSkip.Value = 0
                SAO.Value = False
                ConstVBV.Value = True
            Case 4 'fastdecode
                Deblock.Value = False
                SAO.Value = False
                Weightp.Value = False
                Weightb.Value = False
                BIntra.Value = False
            Case 5 'zerolatency
                BAdapt.Value = 0
                BFrames.Value = 0
                RCLookahead.Value = 0
                Scenecut.Value = 0
                CUtree.Value = False
                FrameThreads.Value = 1
            Case 6 'Animation
                PsyRD.Value = 0.4
                AQStrength.Value = 0.4
                Deblock.Value = True
                DeblockA.Value = 1
                DeblockB.Value = 1
                BFrames.Value += 2
        End Select
    End Sub

    Sub ApplyTuneDefaultValues()
        RcGrain.DefaultValue = False
        qpstep.DefaultValue = 4
        ConstVBV.DefaultValue = False

        Select Case Tune.Value
            Case 1 'psnr
                AQStrength.DefaultValue = 0.0
                PsyRD.DefaultValue = 0.0
                PsyRDOQ.DefaultValue = 0.0
                CUtree.DefaultValue = False
            Case 2 'ssim
                AQmode.DefaultValue = 2
                PsyRD.DefaultValue = 0.0
                PsyRDOQ.DefaultValue = 0.0
            Case 3 'grain
                IPRatio.DefaultValue = 1.1
                PBRatio.DefaultValue = 1
                CUtree.DefaultValue = False
                AQmode.DefaultValue = 0
                qpstep.DefaultValue = 1
                RcGrain.DefaultValue = True
                PsyRD.DefaultValue = 4
                PsyRDOQ.DefaultValue = 10
                RSkip.DefaultValue = 0
                SAO.DefaultValue = False
                ConstVBV.DefaultValue = True
            Case 4 'fastdecode
                Deblock.DefaultValue = False
                SAO.DefaultValue = False
                Weightp.DefaultValue = False
                Weightb.DefaultValue = False
                BIntra.DefaultValue = False
            Case 5 'zerolatency
                BAdapt.DefaultValue = 0
                BFrames.DefaultValue = 0
                RCLookahead.DefaultValue = 0
                Scenecut.DefaultValue = 0
                CUtree.DefaultValue = False
                FrameThreads.DefaultValue = 1
            Case 6 'Animation
                PsyRD.DefaultValue = 0.4
                AQStrength.DefaultValue = 0.4
                Deblock.DefaultValue = True
                DeblockA.DefaultValue = 1
                DeblockB.DefaultValue = 1
                BFrames.DefaultValue += 2
        End Select
    End Sub

    Public Overrides Function GetPackage() As Package
        Return Package.x265
    End Function
End Class

Public Enum x265RateMode
    SingleBitrate
    SingleQuant
    SingleCRF
    TwoPass
    ThreePass
End Enum

Public Enum x265Type
    Vanilla
    Patman
    DJATOM
    JPSDR
End Enum
