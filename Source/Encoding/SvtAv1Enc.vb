Imports System.Globalization
Imports System.Text
Imports StaxRip.UI
Imports StaxRip.VideoEncoderCommandLine

<Serializable()>
Public Class SvtAv1Enc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Sub New()
        Name = "SvtAv1EncApp"
        Params.ApplyPresetDefaultValues()
        Params.ApplyPresetValues()
    End Sub

    <NonSerialized>
    Private ParamsValue As SvtAv1EncParams

    Property Params As SvtAv1EncParams
        Get
            If ParamsValue Is Nothing Then
                ParamsValue = New SvtAv1EncParams
                ParamsValue.Init(ParamsStore)
            End If

            Return ParamsValue
        End Get
        Set(value As SvtAv1EncParams)
            ParamsValue = value
        End Set
    End Property

    Overrides ReadOnly Property IsOvercroppingAllowed As Boolean
        Get
            If Not Params.PsyDolbyVisionRpu.Visible Then Return True
            Return String.IsNullOrWhiteSpace(Params.GetStringParam(Params.PsyDolbyVisionRpu.Switch)?.Value)
        End Get
    End Property

    Overrides ReadOnly Property IsUnequalResizingAllowed As Boolean
        Get
            If Not Params.PsyDolbyVisionRpu.Visible Then Return True
            Return String.IsNullOrWhiteSpace(Params.GetStringParam(Params.PsyDolbyVisionRpu.Switch)?.Value)
        End Get
    End Property

    Overrides ReadOnly Property DolbyVisionMetadataPath As String
        Get
            If Not Params.PsyDolbyVisionRpu.Visible Then Return Nothing
            Return Params.GetStringParam(Params.PsyDolbyVisionRpu.Switch)?.Value
        End Get
    End Property

    Overrides ReadOnly Property Hdr10PlusMetadataPath As String
        Get
            Return Nothing
        End Get
    End Property

    Overrides ReadOnly Property OutputExt As String
        Get
            Return "ivf"
        End Get
    End Property

    Overrides Sub Encode()
        Encode("Video encoding", GetArgs(1, 0, 0, Nothing, p.Script), s.ProcessPriority)

        If Params.Passes > 1 Then
            Encode("Video encoding second pass", GetArgs(2, 0, 0, Nothing, p.Script), s.ProcessPriority)
        End If
        If Params.Passes > 2 Then
            Encode("Video encoding second pass", GetArgs(3, 0, 0, Nothing, p.Script), s.ProcessPriority)
        End If
    End Sub

    Overloads Sub Encode(passName As String, commandLine As String, priority As ProcessPriorityClass)
        If Not CanChunkEncode() Then
            p.Script.Synchronize()
        End If

        Using proc As New Proc
            proc.Package = Package.SvtAv1EncApp
            proc.Header = passName
            proc.Encoding = Encoding.UTF8
            proc.Priority = priority
            proc.FrameCount = p.Script.GetFrameCount
            proc.IntegerFrameOutput = Params.Progress.Value < 2
            proc.SkipStrings = {"Encoding:", "Encoding frame", " Frames @ "}

            If commandLine.Contains("|") Then
                proc.File = "cmd.exe"
                proc.Arguments = "/S /C """ + commandLine + """"
            Else
                proc.CommandLine = commandLine
            End If

            proc.Start()
        End Using
    End Sub

    Overrides Property Bitrate As Integer
        Get
            Return CInt(Params.TargetBitrate.Value)
        End Get
        Set(value As Integer)
            Params.TargetBitrate.Value = value
        End Set
    End Property

    Overrides Function CanChunkEncode() As Boolean
        Return CInt(Params.Chunks.Value) > 1
    End Function

    Overrides Function GetChunks() As Integer
        Return CInt(Params.Chunks.Value)
    End Function

    Overrides Function GetChunkEncodeActions() As List(Of Action)
        Dim maxFrame = If(Params.Decoder.Value = 0, p.Script.GetFrameCount(), p.SourceFrames)
        Dim chunkCount = CInt(Params.Chunks.Value)
        Dim startFrame = CInt(Params.FramesToBeSkipped.Value)
        Dim length = If(CInt(Params.FramesToBeEncoded.Value) > 0, CInt(Params.FramesToBeEncoded.Value), maxFrame - startFrame)
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

            If Params.Passes > 1 Then
                ret.Add(Sub()
                            Encode("Video encoding pass 1" + name.Replace("_chunk", " chunk "), GetArgs(1, chunkStart, chunkEnd, name, p.Script), s.ProcessPriority)
                            If Params.Passes > 1 Then
                                Encode("Video encoding pass 2" + name.Replace("_chunk", " chunk "), GetArgs(2, chunkStart, chunkEnd, name, p.Script), s.ProcessPriority)
                            End If
                            If Params.Passes > 2 Then
                                Encode("Video encoding pass 3" + name.Replace("_chunk", " chunk "), GetArgs(3, chunkStart, chunkEnd, name, p.Script), s.ProcessPriority)
                            End If
                        End Sub)
            Else
                ret.Add(Sub() Encode("Video encoding" + name.Replace("_chunk", " chunk "), GetArgs(1, chunkStart, chunkEnd, name, p.Script), s.ProcessPriority))
            End If
        Next

        Return ret
    End Function

    Overrides Sub RunCompCheck()
        If Not g.VerifyRequirements OrElse Not g.IsValidSource Then
            Exit Sub
        End If

        Dim newParams As New SvtAv1EncParams
        Dim newStore = ObjectHelp.GetCopy(ParamsStore)
        newParams.Init(newStore)

        Dim enc As New SvtAv1Enc
        enc.Params = newParams
        enc.Params.RateControlMode.Value = SvtAv1EncRateMode.Quality
        enc.Params.ConstantRateFactor.Value = enc.Params.CompCheck.Value
        enc.Params.QuantizationParameter.Value = enc.Params.CompCheck.Value

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
        script.Path = p.TempDir + p.TargetFile.Base + "_CompCheck." + script.FileType
        script.Synchronize()

        Log.WriteLine(BR + script.GetFullScript + BR)

        Dim commandLine = enc.Params.GetArgs(0, 0, 0, Nothing, script, p.TempDir + p.TargetFile.Base + "_CompCheck." + OutputExt, True, True)

        Try
            Encode("Compressibility Check", commandLine, ProcessPriorityClass.Normal)
        Catch ex As AbortException
            Exit Sub
        Catch ex As Exception
            g.ShowException(ex)
            Exit Sub
        End Try

        Dim bits = (New FileInfo(p.TempDir + p.TargetFile.Base + "_CompCheck." + OutputExt).Length) * 8
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

    Overrides Function GetMenu() As MenuList
        Return New MenuList From {
            {"Encoder Options", AddressOf ShowConfigDialog},
            {"Container Configuration", AddressOf OpenMuxerConfigDialog}
        }
    End Function

    Overrides Sub ShowConfigDialog(Optional path As String = Nothing)
        Dim newParams As New SvtAv1EncParams
        Dim store = ObjectHelp.GetCopy(ParamsStore)
        newParams.Init(store)
        newParams.ApplyPresetDefaultValues()

        Using form As New CommandLineForm(newParams)
            form.HTMLHelpFunc = Function() $"<p><a href=""{Package.SvtAv1EncApp.HelpURL}"">SvtAv1EncApp Online Help</a></p>" +
               $"<h2>SvtAv1EncApp Console Help</h2><pre>{HelpDocument.ConvertChars(Package.SvtAv1EncApp.CreateHelpfile())}</pre>"

            Dim a = Sub()
                        Dim enc = ObjectHelp.GetCopy(Me)
                        Dim params2 As New SvtAv1EncParams
                        Dim store2 = ObjectHelp.GetCopy(store)
                        params2.Init(store2)
                        enc.Params = params2
                        enc.ParamsStore = store2
                        SaveProfile(enc)
                    End Sub

            form.cms.Add("Save Profile...", a, Keys.Control Or Keys.S, Symbol.Save)

            If Not String.IsNullOrWhiteSpace(path) Then
                form.SimpleUI.ShowPage(path)
            End If

            If form.ShowDialog() = DialogResult.OK Then
                AutoCompCheckValue = CInt(newParams.CompCheckAimedQuality.Value)
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides Property QualityMode() As Boolean
        Get
            Return Params.RateControlMode.Value = SvtAv1EncRateMode.Quality
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

        tester.IgnoredSwitches = ""

        tester.UndocumentedSwitches = ""

        tester.Package = Package.SvtAv1EncApp
        tester.CodeFile = Path.Combine(Folder.Startup.Parent, "Encoding", "SvtAv1Enc.vb")

        Return tester.Test
    End Function
End Class

Public Class SvtAv1EncParams
    Inherits CommandLineParams

    Sub New()
        Title = "SvtAv1EncApp Options"
    End Sub


    Property Decoder As New OptionParam With {
        .Text = "Decoder",
        .Options = {"AviSynth/VapourSynth", "QSVEnc (Intel)", "ffmpeg (Intel)", "ffmpeg (DXVA2)"},
        .Values = {"script", "qs", "ffqsv", "ffdxva"}}

    Property PipingToolAVS As New OptionParam With {
        .Name = "PipingToolAVS",
        .Text = "Pipe",
        .VisibleFunc = Function() p.Script.IsAviSynth AndAlso Decoder.Value = 0,
        .Options = {"Automatic", "avs2pipemod", "ffmpeg"}}

    Property PipingToolVS As New OptionParam With {
        .Name = "PipingToolVS",
        .Text = "Pipe",
        .VisibleFunc = Function() p.Script.IsVapourSynth AndAlso Decoder.Value = 0,
        .Options = {"Automatic", "vspipe", "ffmpeg"}}

    Property CompCheck As New NumParam With {
        .Name = "CompCheckQuant",
        .Text = "Comp. Check",
        .Value = 18,
        .Help = "QP value used as 100% for the compressibility check.",
        .Config = {1, 50}}

    Property CompCheckAimedQuality As New NumParam With {
        .Name = "CompCheckAimedQuality",
        .Text = "Aimed Quality",
        .Value = 50,
        .Help = "Percent value to adjusts the target file size or image size after the compressibility check accordingly.",
        .Config = {1, 100}}

    Property Chunks As New NumParam With {
        .Text = "Chunks",
        .Config = {1, 32},
        .Init = 1}

    '   --------------------------------------------------------
    '   --------------------------------------------------------

    Property Progress As New OptionParam With {
        .Switch = "--progress",
        .Text = "Progress",
        .Options = {"0: No Output", "1: Normal (Default)", "2: Patman86/AOMEnc Style Output"},
        .Values = {"0", "1", "2"},
        .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Vanilla,
        .DefaultValue = 1,
        .Value = 2}

    Property ProgressPsy As New OptionParam With {
        .Switch = "--progress",
        .Text = "Progress",
        .Options = {"0: No Output", "1: Normal (Default)", "2: AOMEnc Style Output", "3: Patman86 Style Output"},
        .Values = {"0", "1", "2", "3"},
        .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Psy,
        .DefaultValue = 1,
        .Value = 3}

    Property Preset As New OptionParam With {
        .Switch = "--preset",
        .Text = "Preset",
        .Options = {"-1: Debug Option", "0: Slowest", "1: Extreme Slow", "2: Ultra Slow", "3: Very Slow", "4: Slower", "5: Slow", "6: Medium", "7: Fast", "8: Faster", "9: Very Fast", "10: Mega Fast", "11: Ultra Fast", "12: Extreme Fast", "13: Fastest"},
        .Values = {"-1", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13"},
        .Init = 11}

    '   --------------------------------------------------------
    '   --------------------------------------------------------

    Property FramesToBeEncoded As New NumParam With {
        .HelpSwitch = "--frames",
        .Text = "Frames To Be Encoded",
        .Config = {0, Integer.MaxValue, 100},
        .Init = 0}

    Property FramesToBeSkipped As New NumParam With {
        .HelpSwitch = "--skip",
        .Text = "Frames To Be Skipped",
        .Config = {0, Integer.MaxValue, 100},
        .Init = 0}

    Property EncoderColorFormat As New OptionParam With {
        .Switch = "--color-format",
        .Text = "Encoder Color Format",
        .Options = {"0: YUV400", "1: YUV420", "2: YUV422", "3: YUV444"},
        .IntegerValue = True,
        .Init = 1}

    Property Profile As New OptionParam With {
        .Switch = "--profile",
        .Text = "Profile",
        .Options = {"0: Main", "1: High", "2: Professional"},
        .IntegerValue = True,
        .Init = 0}

    Property Level As New OptionParam With {
        .Switch = "--level",
        .Text = "Level",
        .Options = {"Autodetect from input", "2.0", "2.1", "2.2", "2.3", "3.0", "3.1", "3.2", "3.3", "4.0", "4.1", "4.2", "4.3", "5.0", "5.1", "5.2", "5.3", "6.0", "6.1", "6.2", "6.3", "7.0", "7.1", "7.2", "7.3"},
        .Values = {"0", "2.0", "2.1", "2.2", "2.3", "3.0", "3.1", "3.2", "3.3", "4.0", "4.1", "4.2", "4.3", "5.0", "5.1", "5.2", "5.3", "6.0", "6.1", "6.2", "6.3", "7.0", "7.1", "7.2", "7.3"},
        .Init = 0}

    Property HighDynamicRangeInput As New OptionParam With {
        .Switch = "--enable-hdr",
        .Text = "Enable HDR",
        .Options = {"0: Off", "1: On"},
        .IntegerValue = True,
        .Init = 0}

    'Property EncoderBitDepth As New OptionParam With {
    '    .Switch = "--input-depth",
    '    .Text = "Encoder Bit Depth",
    '    .Options = {"8-Bit", "10-Bit"},
    '    .Values = {"8", "10"},
    '    .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Vanilla,
    '    .DefaultValue = 0,
    '    .Value = 1}

    'Property PsyEncoderBitDepth As New OptionParam With {
    '    .Switch = "--input-depth",
    '    .Text = "Encoder Bit Depth",
    '    .Options = {"8-Bit", "10-Bit"},
    '    .Values = {"8", "10"},
    '    .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Psy,
    '    .DefaultValue = 1,
    '    .Value = 1}

    Property StatReport As New OptionParam With {
        .Switch = "--enable-stat-report",
        .Text = "Stat Report",
        .Options = {"0: Off", "1: Calculates and outputs PSNR SSIM metrics at the end of encoding"},
        .IntegerValue = True,
        .Init = 0}

    Property Asm As New OptionParam With {
        .Switch = "--asm",
        .Text = "Limit Assembly Instruction Set",
        .Options = {"C", "MMX", "SSE", "SSE2", "SSE3", "SSSE3", "SSE4.1", "SSE4.2", "AVX", "AVX2", "AVX512", "MAX"},
        .Values = {"c", "mmx", "sse", "sse2", "sse3", "ssse3", "sse4_1", "sse4_2", "avx", "avx2", "avx512", "max"},
        .Init = 11}

    Property LogicalProcessors As New NumParam With {
        .HelpSwitch = "--lp",
        .Text = "Logical Cores",
        .Config = {0, Environment.ProcessorCount, 1},
        .Init = 0}

    Property PinnedExecution As New OptionParam With {
        .Switch = "--pin",
        .Text = "Pinned Execution",
        .Options = {"0: Off", "1: On"},
        .IntegerValue = True,
        .Init = 0}

    Property FastDecode As New OptionParam With {
        .Switch = "--fast-decode",
        .Text = "Fast Decode",
        .Options = {"0: Off", "1: On"},
        .IntegerValue = True,
        .Init = 0}

    Property Tune As New OptionParam With {
        .Switch = "--tune",
        .Text = "Tune",
        .IntegerValue = True,
        .Options = {"0: VQ", "1: PSNR (default)", "2: SSIM"},
        .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Vanilla,
        .Init = 1}

    Property TunePsy As New OptionParam With {
        .Switch = "--tune",
        .Text = "Tune",
        .IntegerValue = True,
        .Options = {"0: VQ", "1: PSNR", "2: SSIM (default)", "3: Subjective SSIM"},
        .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Psy,
        .Init = 2}

    '   --------------------------------------------------------
    '   --------------------------------------------------------

    Property RateControlMode As New OptionParam With {
        .HelpSwitch = "--rc",
        .Text = "Rate Control Mode",
        .Options = {"Quality", "Variable Bitrate", "Constant Bitrate"},
        .IntegerValue = True,
        .AlwaysOn = True,
        .Value = 0}

    ReadOnly Property QuantizationParameter As NumParam
        Get
            Select Case Package.SvtAv1EncAppType
                Case SvtAv1EncAppType.Psy
                    Return QuantizationParameterPsy
                Case Else
                    Return QuantizationParameterPsy
                    Return QuantizationParameterVanilla
            End Select
        End Get
    End Property

    Property QuantizationParameterVanilla As New NumParam With {
        .HelpSwitch = "--qp",
        .Text = "Quantization Parameter",
        .VisibleFunc = Function() RateControlMode.Value = SvtAv1EncRateMode.Quality AndAlso AqMode.Value = 0,
        .ValueChangedAction = Sub(x) ConstantRateFactor.Value = CInt(x),
        .Config = {1, 63, 1},
        .Init = 35}

    Property QuantizationParameterPsy As New NumParam With {
        .HelpSwitch = "--qp",
        .Text = "Quantization Parameter",
        .VisibleFunc = Function() RateControlMode.Value = SvtAv1EncRateMode.Quality AndAlso AqMode.Value = 0,
        .ValueChangedAction = Sub(x) ConstantRateFactor.Value = x,
        .Config = {1, 70, 0.25, 2},
        .Init = 35}

    ReadOnly Property ConstantRateFactor As NumParam
        Get
            Select Case Package.SvtAv1EncAppType
                Case SvtAv1EncAppType.Psy
                    Return ConstantRateFactorPsy
                Case Else
                    Return ConstantRateFactorPsy
                    Return ConstantRateFactorVanilla
            End Select
        End Get
    End Property

    Property ConstantRateFactorVanilla As New NumParam With {
        .HelpSwitch = "--crf",
        .Text = "Constant Rate Factor",
        .VisibleFunc = Function() RateControlMode.Value = SvtAv1EncRateMode.Quality AndAlso AqMode.Value <> 0,
        .ValueChangedAction = Sub(x) QuantizationParameter.Value = CInt(x),
        .Config = {1, 63, 1, 0},
        .Init = 35}

    Property ConstantRateFactorPsy As New NumParam With {
        .HelpSwitch = "--crf",
        .Text = "Constant Rate Factor",
        .VisibleFunc = Function() RateControlMode.Value = SvtAv1EncRateMode.Quality AndAlso AqMode.Value <> 0,
        .ValueChangedAction = Sub(x) QuantizationParameter.Value = x,
        .Config = {1, 70, 0.25, 2},
        .Init = 35}

    Property TargetBitrate As New NumParam With {
        .HelpSwitch = "--tbr",
        .Text = "Target Bitrate",
        .VisibleFunc = Function() RateControlMode.Value <> SvtAv1EncRateMode.Quality,
        .Config = {100, 100000, 100},
        .Init = 2000}

    Property MaximumBitrate As New NumParam With {
        .Switch = "--mbr",
        .Text = "Maximum Bitrate",
        .VisibleFunc = Function() ConstantRateFactor.Visible,
        .Config = {0, 100000, 100},
        .Init = 0}

    Property MaxQp As New NumParam With {
        .Switch = "--max-qp",
        .Text = "Maximum Quantizer",
        .VisibleFunc = Function() RateControlMode.Value <> SvtAv1EncRateMode.Quality,
        .Config = {1, 63, 1},
        .Init = 63}

    Property MinQp As New NumParam With {
        .Switch = "--min-qp",
        .Text = "Minimum Quantizer",
        .VisibleFunc = Function() RateControlMode.Value <> SvtAv1EncRateMode.Quality,
        .Config = {1, 62, 1},
        .Init = 1}

    Property AqMode As New OptionParam With {
        .Switch = "--aq-mode",
        .Text = "Adaptive Quantization",
        .IntegerValue = True,
        .Options = {"0: Off", "1: Variance base using AV1 segments", "2: Deltaq pred efficiency"},
        .Init = 2}

    Property RecodeLoop As New OptionParam With {
        .Switch = "--recode-loop",
        .Text = "Recode Loop",
        .IntegerValue = True,
        .Options = {"0: Off", "1: Allow recode for KF and exceeding maximum frame bandwidth", "2: Allow recode only for key frames, alternate reference frames, and Golden frames", "3: Allow recode for all frame types based on bitrate constraints", "4: Preset based decision"},
        .Init = 4}

    Property EnableQm As New BoolParam With {
        .Switch = "--enable-qm",
        .Text = "Enable quantisation matrices",
        .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Vanilla,
        .IntegerValue = True,
        .Init = False}

    Property EnableQmPsy As New BoolParam With {
        .Switch = "--enable-qm",
        .Text = "Enable quantisation matrices",
        .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Psy,
        .IntegerValue = True,
        .Init = True}

    Property QmMin As New NumParam With {
        .Switch = "--qm-min",
        .Text = "Min quant matrix flatness",
        .Config = {0, 15, 1},
        .VisibleFunc = Function() EnableQm.Visible AndAlso EnableQm.Value,
        .Init = 8}

    Property QmMinPsy As New NumParam With {
        .Switch = "--qm-min",
        .Text = "Min quant matrix flatness",
        .Config = {0, 15, 1},
        .VisibleFunc = Function() EnableQmPsy.Visible AndAlso EnableQmPsy.Value,
        .Init = 0}

    Property QmMax As New NumParam With {
        .Switch = "--qm-max",
        .Text = "Max quant matrix flatness",
        .VisibleFunc = Function() EnableQm.Value,
        .Config = {0, 15, 1},
        .Init = 15}

    '   --------------------------------------------------------
    '   --------------------------------------------------------

    ReadOnly Property Passes As Integer
        Get
            If PassesCBR.Visible Then
                Return PassesCBR.Value + 1
            ElseIf PassesVBR.Visible Then
                Return PassesVBR.Value + 1
            End If
        End Get
    End Property

    Property PassesCBR As New OptionParam With {
        .Switches = {"--pass", "--passes", "--stats"},
        .Text = "Passes",
        .Options = {"1-pass encode", "2-pass encode", "3-pass encode"},
        .Values = {"1", "2", "3"},
        .VisibleFunc = Function() RateControlMode.Value = SvtAv1EncRateMode.CBR,
        .Init = 0}

    Property PassesVBR As New OptionParam With {
        .Switches = {"--pass", "--passes", "--stats"},
        .Text = "Passes",
        .Options = {"1-pass encode", "2-pass encode"},
        .Values = {"1", "2"},
        .VisibleFunc = Function() RateControlMode.Value = SvtAv1EncRateMode.VBR,
        .Init = 0}

    '   --------------------------------------------------------
    '   --------------------------------------------------------

    Property KeyInt As New OptionParam With {
        .Switch = "--keyint",
        .Text = "Keyint / GOP Size",
        .Options = {"-2: ~5 seconds", "0: ""infinite""", "1 second", "2 seconds", "3 seconds", "4 seconds", "5 seconds", "6 seconds", "7 seconds", "8 seconds", "9 seconds", "10 seconds"},
        .Values = {"-2", "0", "1s", "2s", "3s", "4s", "5s", "6s", "7s", "8s", "9s", "10s"},
        .VisibleFunc = Function() Not ConstantRateFactor.Visible,
        .ValueChangedAction = Sub(x) KeyIntCrf.Value = x,
        .Init = 0}

    Property KeyIntCrf As New OptionParam With {
        .Switch = "--keyint",
        .Text = "Keyint / GOP Size",
        .Options = {"-2: ~5 seconds", "-1: ""infinite""", "1 second", "2 seconds", "3 seconds", "4 seconds", "5 seconds", "6 seconds", "7 seconds", "8 seconds", "9 seconds", "10 seconds"},
        .Values = {"-2", "-1", "1s", "2s", "3s", "4s", "5s", "6s", "7s", "8s", "9s", "10s"},
        .VisibleFunc = Function() ConstantRateFactor.Visible,
        .ValueChangedAction = Sub(x) KeyInt.Value = x,
        .Init = 0}

    Property IntraRefreshRate As New OptionParam With {
        .Switch = "--irefresh-type",
        .Text = "Intra Refresh Type",
        .Options = {"1: FWD Frame (Open GOP)", "2: Key Frame (Closed GOP)"},
        .Values = {"1", "2"},
        .Init = 1}

    Property SceneChangeDetection As New OptionParam With {
        .Switch = "--scd",
        .Text = "Scene Change Detection Control",
        .Options = {"0: Off", "1: On"},
        .IntegerValue = True,
        .Init = 0}

    Property Lookahead As New NumParam With {
        .Switch = "--lookahead",
        .Text = "Lookahead",
        .Config = {-1, 120, 1},
        .Init = -1}

    Property HierarchicalLevels As New OptionParam With {
        .Switch = "--hierarchical-levels",
        .Text = "Hierarchical Levels",
        .Options = {"2: 3 temporal layers", "3: 4 temporal layers", "4: 5 temporal layers", "5: 6 temporal layers"},
        .Values = {"2", "3", "4", "5"},
        .Init = 2}

    Property PredStructure As New OptionParam With {
        .Switch = "--pred-struct",
        .Text = "Prediction Structure",
        .Options = {"1: Low Delay", "2: Random Access"},
        .Values = {"1", "2"},
        .Init = 1}
    Property EnableDg As New OptionParam With {
        .Switch = "--enable-dg",
        .Text = "Dynamic GoP",
        .Options = {"0: Off", "1: On"},
        .IntegerValue = True,
        .Init = 1}

    Property StartupMgSize As New OptionParam With {
        .Switch = "--startup-mg-size",
        .Text = "Startup Mini-GoP Size",
        .Options = {"0: Off", "2: 3 temporal layers", "3: 4 temporal layers", "4: 5 temporal layers"},
        .Values = {"0", "2", "3", "4"},
        .Init = 0}

    '   --------------------------------------------------------
    '   --------------------------------------------------------

    Property TileRow As New NumParam With {
        .Switch = "--tile-rows",
        .Text = "Tile Rows",
        .Config = {0, 6, 1},
        .Init = 0}

    Property TileCol As New NumParam With {
        .Switch = "--tile-columns",
        .Text = "Tile Columns",
        .Config = {0, 4, 1},
        .Init = 0}

    Property LoopFilterEnable As New NumParam With {
        .Switch = "--enable-dlf",
        .Text = "Deblocking Loop Filter",
        .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Vanilla,
        .Config = {0, 1, 1},
        .Init = 1}

    Property LoopFilterEnablePsy As New NumParam With {
        .Switch = "--enable-dlf",
        .Text = "Deblocking Loop Filter",
        .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Psy,
        .Config = {0, 2, 1},
        .Init = 1}

    Property CDEFLevel As New BoolParam With {
        .Switch = "--enable-cdef",
        .Text = "Constrained Directional Enhancement Filter",
        .IntegerValue = True,
        .Init = True}

    Property EnableRestoration As New BoolParam With {
        .Switch = "--enable-restoration",
        .Text = "Loop Restoration Filter",
        .IntegerValue = True,
        .Init = True}

    Property EnableTPLModel As New BoolParam With {
        .Switch = "--enable-tpl-la",
        .Text = "Temporal Dependency Model",
        .VisibleFunc = Function() RateControlMode.Value = SvtAv1EncRateMode.Quality,
        .IntegerValue = True,
        .Init = True}

    Property Mfmv As New NumParam With {
        .Switch = "--enable-mfmv",
        .Text = "Motion Field Motion Vector",
        .Config = {-1, 1, 1},
        .Init = -1}

    Property EnableTF As New BoolParam With {
        .Switch = "--enable-tf",
        .Text = "ALT-REF Frames",
        .IntegerValue = True,
        .Init = True}

    Property EnableOverlays As New BoolParam With {
        .Switch = "--enable-overlays",
        .Text = "Insertion of Overlayer Pictures",
        .IntegerValue = True,
        .Init = False}

    Property ScreenContentMode As New OptionParam With {
        .Switch = "--scm",
        .Text = "Screen Content Detection Level",
        .IntegerValue = True,
        .Options = {"0: Off", "1: On", "2: Content Adaptive"},
        .Init = 2}

    Property RestrictedMotionVector As New BoolParam With {
        .Switch = "--rmv",
        .Text = "Restrict Motion Vectors",
        .IntegerValue = True,
        .Init = False}

    Property FilmGrain As New NumParam With {
        .Switch = "--film-grain",
        .Text = "Film Grain Level",
        .Config = {0, 50, 1},
        .Init = 0}

    Property FilmGrainDenoise As New OptionParam With {
        .Switch = "--film-grain-denoise",
        .Text = "Film Grain Denoise",
        .IntegerValue = True,
        .Options = {"0: No denoising, film grain data is still in frame header", "1: Level of denoising is set by the film-grain parameter"},
        .VisibleFunc = Function() FilmGrain.Value > 0,
        .Init = 0}

    Property FGSTable As New StringParam With {
        .Switch = "--fgs-table",
        .Text = "FGS Table",
        .BrowseFile = True}

    Property SuperresMode As New OptionParam With {
        .Switch = "--superres-mode",
        .Text = "Super-Resolution Mode",
        .Options = {"0: None, no frame super-resolution allowed", "1: All frames are encoded at the specified scale", "2: All frames are coded at a random scale", "3: Super-resolution scale for a frame is determined based on the q_index", "4: Automatically select the mode for appropriate frames"},
        .Values = {"0", "1", "2", "3", "4"},
        .Init = 0}

    Property SuperresDenom As New OptionParam With {
        .Switch = "--superres-denom",
        .Text = "SuperRes Denominator",
        .Options = {"8: No scaling", "9: 8/9-scaling", "10: 8/10-scaling", "11: 8/11-scaling", "12: 8/12-scaling", "13: 8/13-scaling", "14: 8/14-scaling", "15: 8/15-scaling", "16: Half-scaling"},
        .Values = {"8", "9", "10", "11", "12", "13", "14", "15", "16"},
        .VisibleFunc = Function() SuperresMode.Value = 1,
        .Init = 0}

    Property SuperresKfDenom As New OptionParam With {
        .Switch = "--superres-kf-denom",
        .Text = "SuperRes Denominator for KeyFrames",
        .Options = {"8: No scaling", "9: 8/9-scaling", "10: 8/10-scaling", "11: 8/11-scaling", "12: 8/12-scaling", "13: 8/13-scaling", "14: 8/14-scaling", "15: 8/15-scaling", "16: Half-scaling"},
        .Values = {"8", "9", "10", "11", "12", "13", "14", "15", "16"},
        .VisibleFunc = Function() SuperresMode.Value = 1,
        .Init = 0}

    Property SuperresQthres As New NumParam With {
        .Switch = "--superres-qthres",
        .Text = "SuperRes q-threshold",
        .Config = {0, 63, 1},
        .VisibleFunc = Function() SuperresMode.Value = 3,
        .Init = 43}

    Property SuperresKfQthres As New NumParam With {
        .Switch = "--superres-kf-qthres",
        .Text = "SuperRes q-threshold for KeyFrames",
        .Config = {0, 63, 1},
        .VisibleFunc = Function() SuperresMode.Value = 3,
        .Init = 43}


    Property SframeInterval As New NumParam With {
        .Switch = "--sframe-dist",
        .Text = "S-Frame Interval",
        .Config = {0, Integer.MaxValue, 1},
        .Init = 0}

    Property SframeMode As New OptionParam With {
        .Switch = "--sframe-mode",
        .Text = "S-Frame Insertion Mode",
        .Options = {"1: Considered frame will be made into an S-Frame only if it is an altref frame", "2: Next altref frame will be made into an S-Frame (default)"},
        .Values = {"1", "2"},
        .Init = 1}


    Property ResizeMode As New OptionParam With {
        .Switch = "--resize-mode",
        .Text = "Resize Mode",
        .Options = {"0: None, no frame resize allowed (default)", "1: Fixed mode, all frames are encoded at the specified scale of 8/denom", "2: Random mode, all frames are coded at a random scale", "3: Dynamic mode, scale for a frame is determined based on buffer level and average qp in rate control", "4: Random access mode, scaling is controlled by scale events"},
        .Values = {"0", "1", "2", "3", "4"},
        .Init = 0}

    Property ResizeDenom As New OptionParam With {
        .Switch = "--resize-denom",
        .Text = "Resize Denominator",
        .Expanded = True,
        .Options = {"8: No scaling (default)", "9: 8/9-scaling", "10: 8/10-scaling", "11: 8/11-scaling", "12: 8/12-scaling", "13: 8/13-scaling", "14: 8/14-scaling", "15: 8/15-scaling", "16: Half-scaling"},
        .Values = {"8", "9", "10", "11", "12", "13", "14", "15", "16"},
        .VisibleFunc = Function() ResizeMode.Value = 1,
        .Init = 0}

    Property ResizeKfDenom As New OptionParam With {
        .Switch = "--resize-kf-denom",
        .Text = "Resize Denominator for KeyFrames",
        .Expanded = True,
        .Options = {"8: No scaling (default)", "9: 8/9-scaling", "10: 8/10-scaling", "11: 8/11-scaling", "12: 8/12-scaling", "13: 8/13-scaling", "14: 8/14-scaling", "15: 8/15-scaling", "16: Half-scaling"},
        .Values = {"8", "9", "10", "11", "12", "13", "14", "15", "16"},
        .VisibleFunc = Function() ResizeMode.Value = 1,
        .Init = 0}

    Property ResizeFrameEvents As New StringParam With {
        .Switch = "--frame-resz-events",
        .Text = "Resize Events",
        .VisibleFunc = Function() ResizeMode.Value = 4,
        .Init = ""}

    Property ResizeFrameDenoms As New StringParam With {
        .Switch = "--frame-resz-denoms",
        .Text = "Resize Denominator In Event",
        .VisibleFunc = Function() ResizeMode.Value = 4,
        .Init = ""}

    Property ResizeFrameKfDenoms As New StringParam With {
        .Switch = "--frame-resz-kf-denoms",
        .Text = "Resize Denominator for KeyFrames In Event",
        .VisibleFunc = Function() ResizeMode.Value = 4,
        .Init = ""}

    '   --------------------------------------------------------
    '   --------------------------------------------------------

    Property ColorPrimaries As New OptionParam With {
        .Switch = "--color-primaries",
        .Text = "Color Primaries",
        .Expanded = True,
        .Options = {"Unspecified", "BT.709", "BT.470 System M (historical)", "BT.470 System B, G (historical)", "BT.601", "SMPTE 240", "Generic film (color filters using illuminant C)", "BT.2020, BT.2100", "SMPTE 428 (CIE 1921 XYZ)", "SMPTE RP 431-2", "SMPTE EG 432-1", "EBU Tech. 3213-E"},
        .Values = {"2", "1", "4", "5", "6", "7", "8", "9", "10", "11", "12", "22"},
        .Init = 0}

    Property TransferCharacteristics As New OptionParam With {
        .Switch = "--transfer-characteristics",
        .Text = "Transfer Characteristics",
        .Expanded = True,
        .Options = {"Unspecified", "BT.709", "BT.470 System M (historical)", "BT.470 System B, G (historical)", "BT.601", "SMPTE 240 M", "Linear", "Logarithmic (100 : 1 range)", "Logarithmic (100 * Sqrt(10) : 1 range)", "IEC 61966-2-4", "BT.1361", "sRGB or sYCC", "BT.2020 10-bit systems", "BT.2020 12-bit systems", "SMPTE ST 2084, ITU BT.2100 PQ", "SMPTE ST 428", "BT.2100 HLG, ARIB STD-B67"},
        .Values = {"2", "1", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18"},
        .Init = 0}

    Property MatrixCoefficients As New OptionParam With {
        .Switch = "--matrix-coefficients",
        .Text = "Matrix Coefficients",
        .Expanded = True,
        .Options = {"Unspecified", "Identity matrix", "BT.709", "US FCC 73.628", "BT.470 System B, G (historical)", "BT.601", "SMPTE 240 M", "YCgCo", "BT.2020 non-constant luminance, BT.2100 YCbCr", "BT.2020 constant luminance", "SMPTE ST 2085 YDzDx", "Chromaticity-derived non-constant luminance", "Chromaticity-derived constant luminance", "BT.2100 ICtCp"},
        .Values = {"2", "0", "1", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14"},
        .Init = 0}

    Property ColorRange As New OptionParam With {
        .Switch = "--color-range",
        .Text = "Color Range",
        .Expanded = True,
        .IntegerValue = True,
        .Options = {"0: Studio (default)", "1: Full"},
        .Init = 0}

    Property ChromaSamplePosition As New OptionParam With {
        .Switch = "--chroma-sample-position",
        .Text = "Chroma Sample Position",
        .IntegerValue = True,
        .Options = {"0: Unknown (default)", "1: Vertical/Left - horizontally co-located with luma samples, vertical position in the middle between two luma samples", "2: Colocated/Topleft - co-located with luma samples"},
        .Init = 0}

    Property MasteringDisplay As New StringParam With {
        .Switch = "--mastering-display",
        .Text = "Master Display",
        .InitAction = Sub(tb)
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 2.3
                      End Sub}

    Property ContentLightLevel As New StringParam With {
        .Switch = "--content-light",
        .Text = "Content Light Level",
        .ArgsFunc = Function() If(MaxCLL.Value <> 0 OrElse MaxFALL.Value <> 0, """" & MaxCLL.Value & "," & MaxFALL.Value & """", ""),
        .ImportAction = Sub(param, arg)
                            If arg = "" Then Exit Sub

                            Dim a = arg.Trim(""""c).Split(","c)
                            MaxCLL.Value = a(0).ToInt
                            MaxFALL.Value = a(1).ToInt
                        End Sub,
        .Init = ""}

    Property MaxCLL As New NumParam With {
        .Text = "Maximum CLL",
        .Switch = "--content-light",
        .Config = {0, 65535, 50},
        .ArgsFunc = Function() If(MaxCLL.Value <> 0 OrElse MaxFALL.Value <> 0, "--content-light """ & MaxCLL.Value & "," & MaxFALL.Value & """", ""),
        .ImportAction = Sub(param, arg)
                            If arg = "" Then Exit Sub

                            Dim a = arg.Trim(""""c).Split(","c)
                            MaxCLL.Value = a(0).ToInt
                            MaxFALL.Value = a(1).ToInt
                        End Sub,
        .Init = 0}

    Property MaxFALL As New NumParam With {
        .Switches = {"--content-light"},
        .Text = "Maximum FALL",
        .Config = {0, 65535, 50},
        .Init = 0}

    '   --------------------------------------------------------
    '   --------------------------------------------------------

    Property Custom As New StringParam With {
        .Text = "Custom",
        .Quotes = QuotesMode.Never,
        .AlwaysOn = True,
        .InitAction = Sub(tb)
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = g.GetCodeFont
                      End Sub}

    Property CustomFirstPass As New StringParam With {
        .Text = "Custom" + BR + "First Pass",
        .Quotes = QuotesMode.Never,
        .VisibleFunc = Function() Passes > 1,
        .InitAction = Sub(tb)
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = g.GetCodeFont
                      End Sub}

    Property CustomSecondPass As New StringParam With {
        .Text = "Custom" + BR + "Second Pass",
        .Quotes = QuotesMode.Never,
        .VisibleFunc = Function() Passes > 1,
        .InitAction = Sub(tb)
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = g.GetCodeFont
                      End Sub}

    Property CustomThirdPass As New StringParam With {
        .Text = "Custom" + BR + "Third Pass",
        .Quotes = QuotesMode.Never,
        .VisibleFunc = Function() Passes > 2,
        .InitAction = Sub(tb)
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = g.GetCodeFont
                      End Sub}

    '   --------------------------------------------------------
    '   --------------------------------------------------------

    Property PsyDolbyVisionRpu As New StringParam With {
        .Switch = "--dolby-vision-rpu",
        .Text = "Dolby Vision RPU",
        .BrowseFile = True,
        .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Psy}

    Property PsyVarianceBoostStrength As New OptionParam With {
        .Switch = "--variance-boost-strength",
        .Text = "Variance Boost Strength",
        .Expanded = True,
        .IntegerValue = True,
        .Options = {"0: None", "1: Mild", "2: Gentle (default)", "3: Medium", "4: Aggressive"},
        .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Psy,
        .Init = 2}

    Property PsyVarianceOctile As New OptionParam With {
        .Switch = "--variance-octile",
        .Text = "Variance Octile",
        .Expanded = True,
        .IntegerValue = True,
        .Options = {"0", "1: 1/8th", "2: 2/8th", "3: 3/8th", "4: 4/8th", "5: 5/8th", "6: 6/8th (default)", "7: 7/8th", "8: 8/8th"},
        .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Psy,
        .Init = 6}

    Property PsyEnableAltCurve As New OptionParam With {
        .Switch = "--enable-alt-curve",
        .Text = "Enable Alt Curve",
        .Expanded = True,
        .IntegerValue = True,
        .Options = {"0: Off (default)", "1: On"},
        .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Psy,
        .Init = 0}

    Property PsySharpness As New OptionParam With {
        .Switch = "--sharpness",
        .Text = "Sharpness",
        .Expanded = True,
        .Options = {"-7", "-6", "-5", "-4", "-3", "-2", "-1", "0 (default)", "1", "2", "3", "4", "5", "6", "7"},
        .Values = {"-7", "-6", "-5", "-4", "-3", "-2", "-1", "0", "1", "2", "3", "4", "5", "6", "7"},
        .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Psy,
        .Init = 7}

    Property PsyQpScaleCompressStrength As New OptionParam With {
        .Switch = "--qp-scale-compress-strength",
        .Text = "QP Scale Compress Strength",
        .Expanded = True,
        .IntegerValue = True,
        .Options = {"0", "1 (default)", "2", "3"},
        .VisibleFunc = Function() Package.SvtAv1EncAppType = SvtAv1EncAppType.Psy,
        .Init = 1}

    '   --------------------------------------------------------
    '   --------------------------------------------------------






    Overrides ReadOnly Property Items As List(Of CommandLineParam)
        Get
            If ItemsValue Is Nothing Then
                ItemsValue = New List(Of CommandLineParam)

                Add("Input/Output",
                    Decoder, PipingToolAVS, PipingToolVS,
                    CompCheck, CompCheckAimedQuality,
                    Chunks,
                    Progress, ProgressPsy,
                    FramesToBeEncoded, FramesToBeSkipped,
                    EncoderColorFormat,
                    HighDynamicRangeInput, StatReport,
                    Asm, LogicalProcessors, PinnedExecution
                )
                Add("Basic",
                    Preset, Profile, Level, Tune, TunePsy, FastDecode
                )
                Add("Rate Control",
                    RateControlMode, ConstantRateFactor, QuantizationParameter, TargetBitrate, MaximumBitrate, MaxQp, MinQp,
                    PassesVBR, PassesCBR,
                    AqMode, RecodeLoop,
                    EnableQm, EnableQmPsy, QmMax, QmMin, QmMinPsy
                )
                Add("GOP size/type",
                    KeyInt, KeyIntCrf,
                    IntraRefreshRate, SceneChangeDetection, Lookahead, HierarchicalLevels, PredStructure, EnableDg, StartupMgSize
                )
                Add("AV1 Specific 1",
                    TileRow, TileCol, LoopFilterEnable, LoopFilterEnablePsy,
                    CDEFLevel, EnableRestoration, EnableTPLModel, Mfmv, EnableTF, EnableOverlays, ScreenContentMode, RestrictedMotionVector,
                    FilmGrain, FilmGrainDenoise, FGSTable
                )
                Add("AV1 Specific 2",
                    SuperresMode, SuperresDenom, SuperresKfDenom, SuperresQthres, SuperresKfQthres,
                    SframeInterval, SframeMode,
                    ResizeMode, ResizeDenom, ResizeKfDenom, ResizeFrameEvents, ResizeFrameDenoms, ResizeFrameKfDenoms
                )
                Add("Color Description",
                    ColorPrimaries, TransferCharacteristics, MatrixCoefficients, ColorRange, ChromaSamplePosition, MasteringDisplay, MaxCLL, MaxFALL
                )
                If Package.SvtAv1EncAppType = SvtAv1EncAppType.Psy Then
                    Add("PSY",
                        PsyDolbyVisionRpu, PsyVarianceBoostStrength, PsyVarianceOctile, PsyEnableAltCurve, PsySharpness, PsyQpScaleCompressStrength
                    )
                End If

                Add("Custom", Custom, CustomFirstPass, CustomSecondPass, CustomThirdPass)

                'ItemsValue = ItemsValue.OrderBy(Function(i) i.Weight).ToList
            End If

            Return ItemsValue
        End Get
    End Property

    Overrides Function GetCommandLinePreview() As String
        Dim ret = GetCommandLine(True, True, 1)

        If Passes > 1 Then
            ret += BR2 + GetCommandLine(True, True, 2)
        End If
        If Passes > 2 Then
            ret += BR2 + GetCommandLine(True, True, 3)
        End If

        Return ret
    End Function

    Public Overrides Sub ShowHelp(options As String())
        ShowConsoleHelp(Package.SvtAv1EncApp, options)
    End Sub

    Private BlockValueChanged As Boolean

    Protected Overrides Sub OnValueChanged(item As CommandLineParam)
        If BlockValueChanged Then
            Exit Sub
        End If

        If item Is Preset Then
            BlockValueChanged = True
            ApplyPresetValues()
            BlockValueChanged = False
        End If

        MyBase.OnValueChanged(item)
    End Sub

    Overloads Overrides Function GetCommandLine(
        includePaths As Boolean,
        includeExecutable As Boolean,
        Optional pass As Integer = 1) As String

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

        Dim sb As New StringBuilder
        Dim pipeTool = If(p.Script.IsAviSynth, PipingToolAVS, PipingToolVS).ValueText.ToLowerInvariant()
        Dim isSingleChunk = endFrame = 0
        Dim statsPath = (p.VideoEncoder.OutputPath.DirAndBase + chunkName + "_2pass.log").Escape

        If includePaths AndAlso includeExecutable Then
            Dim isCropped = (p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 AndAlso Decoder.ValueText <> "avs" AndAlso p.Script.IsFilterActive("Crop")
            Dim pipeString = ""

            Select Case Decoder.ValueText.ToLowerInvariant()
                Case "script"
                    'Broken ANSI fallback taken from x265
                    If pipeTool = "automatic" OrElse (p.Script.IsAviSynth AndAlso Not TextEncoding.IsProcessUTF8 AndAlso Not TextEncoding.ArePathsSupportedByASCIIEncoding) Then
                        pipeTool = If(p.Script.IsAviSynth, "avs2pipemod", "vspipe")
                    End If

                    Select Case pipeTool
                        Case "avs2pipemod"
                            Dim chunk = If(isSingleChunk, "", $" -trim={startFrame},{endFrame}")
                            Dim dll = If(FrameServerHelp.IsPortable, $" -dll={Package.AviSynth.Path.Escape}", "")
                            pipeString = Package.avs2pipemod.Path.Escape + dll + chunk + " -y4mp " + script.Path.Escape

                            sb.Append(pipeString & " | " & Package.SvtAv1EncApp.Path.Escape)
                        Case "vspipe"
                            Dim chunk = If(isSingleChunk, "", $" --start {startFrame} --end {endFrame}")
                            pipeString = Package.vspipe.Path.Escape + " " + script.Path.Escape + " - --container y4m" + chunk

                            sb.Append(pipeString & " | " & Package.SvtAv1EncApp.Path.Escape)
                        Case "ffmpeg"
                            pipeString = Package.ffmpeg.Path.Escape + If(p.Script.IsVapourSynth, " -f vapoursynth", "") + " -i " + script.Path.Escape + " -f yuv4mpegpipe -strict -1 -loglevel fatal -hide_banner -"

                            sb.Append(pipeString & " | " & Package.SvtAv1EncApp.Path.Escape)
                    End Select
                Case "qs"
                    Dim crop = If(isCropped, " --crop " & p.CropLeft & "," & p.CropTop & "," & p.CropRight & "," & p.CropBottom, "")
                    pipeString = Package.QSVEncC.Path.Escape + " -o - -c raw" + crop + " -i " + p.SourceFile.Escape

                    sb.Append(pipeString & " | " & Package.SvtAv1EncApp.Path.Escape)
                Case "ffqsv"
                    Dim crop = If(isCropped, $" -vf ""crop={p.SourceWidth - p.CropLeft - p.CropRight}:{p.SourceHeight - p.CropTop - p.CropBottom}:{p.CropLeft}:{p.CropTop}""", "")
                    pipeString = Package.ffmpeg.Path.Escape + " -threads 1 -hwaccel qsv -i " + p.SourceFile.Escape + " -f yuv4mpegpipe -strict -1" + crop + " -loglevel fatal -hide_banner -"

                    sb.Append(pipeString & " | " & Package.SvtAv1EncApp.Path.Escape)
                Case "ffdxva"
                    Dim crop = If(isCropped, $" -vf ""crop={p.SourceWidth - p.CropLeft - p.CropRight}:{p.SourceHeight - p.CropTop - p.CropBottom}:{p.CropLeft}:{p.CropTop}""", "")
                    Dim pix_fmt = If(p.SourceVideoBitDepth = 10, "yuv420p10le", "yuv420p")
                    pipeString = Package.ffmpeg.Path.Escape + " -threads 1 -hwaccel dxva2 -i " + p.SourceFile.Escape + " -f yuv4mpegpipe -pix_fmt " + pix_fmt + " -strict -1" + crop + " -loglevel fatal -hide_banner -"

                    sb.Append(pipeString & " | " & Package.SvtAv1EncApp.Path.Escape)
            End Select
        End If

        If includePaths Then
            Dim input = If(Decoder.Value = 0 AndAlso pipeTool = "automatic", script.Path.Escape, "-")

            sb.Append($" --input {input}")
            sb.Append($" --width {p.TargetWidth} --height {p.TargetHeight}")
            sb.Append($" --output {(targetPath.DirAndBase + chunkName + targetPath.ExtFull).Escape}")
        End If

        If isSingleChunk Then
            If FramesToBeSkipped.Value > 0 AndAlso Not IsCustom(pass, "--skip") Then
                sb.Append($" --skip {FramesToBeSkipped.Value}")
            End If

            If includePaths AndAlso Not IsCustom(pass, "--frames") Then
                Dim n = If(FramesToBeEncoded.Value > 0,
                    Math.Min(p.TargetFrames - FramesToBeSkipped.Value, FramesToBeEncoded.Value),
                    p.TargetFrames - FramesToBeSkipped.Value)

                sb.Append($" --frames {n}")
            End If
        Else
            sb.Append($" --frames {endFrame - startFrame + 1}")
        End If

        If Passes > 1 Then
            sb.Append(" --pass " & pass)
            If includePaths Then
                sb.Append(" --stats " + statsPath)
            End If

            If pass = 1 Then
                If CustomFirstPass.Value <> "" Then
                    sb.Append(" " + CustomFirstPass.Value)
                End If
            ElseIf pass = 2 Then

                If CustomSecondPass.Value <> "" Then
                    sb.Append(" " + CustomSecondPass.Value)
                End If
            Else
                If CustomThirdPass.Value <> "" Then
                    sb.Append(" " + CustomThirdPass.Value)
                End If
            End If
        End If

        'If includePaths AndAlso (MultiPassOptDistortion.Value OrElse MultiPassOptAnalysis.Value) Then
        '    sb.Append(" --analysis-reuse-file " + (targetPath.DirAndBase + chunkName + ".analysis").Escape)
        'End If

        If Not IsCustom(pass, "--rc") Then
            sb.Append($" --rc {RateControlMode.Value}")
        End If

        If RateControlMode.Value = SvtAv1EncRateMode.Quality Then
            If ConstantRateFactor.Visible Then
                If Not IsCustom(pass, "--crf") AndAlso ConstantRateFactor.Value <> ConstantRateFactor.DefaultValue Then
                    sb.Append(" --crf " + ConstantRateFactor.Value.ToString("0.##", CultureInfo.InvariantCulture))
                End If
            ElseIf QuantizationParameter.Visible Then
                If Not IsCustom(pass, "--qp") AndAlso QuantizationParameter.Value <> QuantizationParameter.DefaultValue Then
                    sb.Append(" --qp " + QuantizationParameter.Value.ToString("0.##", CultureInfo.InvariantCulture))
                End If
            End If
        Else
            If Not IsCustom(pass, "--tbr") Then
                sb.Append(" --tbr " & If(pass = 1, TargetBitrate.Value, p.VideoBitrate))
            End If
        End If

        Dim q = From i In Items Where i.GetArgs <> "" AndAlso Not IsCustom(pass, i.Switch)

        If q.Count > 0 Then
            sb.Append(" " + q.Select(Function(item) item.GetArgs).Join(" "))
        End If

        Return Macro.Expand(sb.ToString.Trim.FixBreak.Replace(BR, " "))
    End Function

    Function IsCustom(pass As Integer, switch As String) As Boolean
        If switch = "" Then Return False

        If Passes > 1 Then
            If pass = 1 Then
                If CustomFirstPass.Value?.Contains(switch + " ") OrElse CustomFirstPass.Value?.EndsWith(switch) Then
                    Return True
                End If
            ElseIf pass = 2 Then
                If CustomSecondPass.Value?.Contains(switch + " ") OrElse CustomSecondPass.Value?.EndsWith(switch) Then
                    Return True
                End If
            Else
                If CustomThirdPass.Value?.Contains(switch + " ") OrElse CustomThirdPass.Value?.EndsWith(switch) Then
                    Return True
                End If
            End If
        End If

        Return If(Custom.Value?.Contains(switch + " ") OrElse Custom.Value?.EndsWith(switch), False)
    End Function

    Sub ApplyPresetValues()
    End Sub

    Sub ApplyPresetDefaultValues()
    End Sub

    Public Overrides Function GetPackage() As Package
        Return Package.SvtAv1EncApp
    End Function
End Class

Public Enum SvtAv1EncRateMode
    Quality
    VBR
    CBR
End Enum

Public Enum SvtAv1EncAppType
    Vanilla
    Psy
End Enum