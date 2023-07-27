Imports System.Text
Imports StaxRip.VideoEncoderCommandLine

<Serializable()>
Public Class SvtAv1Enc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Sub New()
        Name = "AV1 | SvtAv1EncApp"
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

    Overrides ReadOnly Property OutputExt As String
        Get
            Return "ivf"
        End Get
    End Property

    Overrides Sub Encode()
        Encode("Video encoding", GetArgs(1, 0, 0, Nothing, p.Script), s.ProcessPriority)

        If Params.Passes.Value > 0 Then
            Encode("Video encoding second pass", GetArgs(2, 0, 0, Nothing, p.Script), s.ProcessPriority)
        End If

        AfterEncoding()
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
            proc.SkipStrings = {"Encoding frame", " Frames @ "}

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
        Dim startFrame = CInt(Params.FrameSkip.Value)
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

            If Params.Passes.Value > 0 Then
                ret.Add(Sub()
                            Encode("Video encoding pass 1" + name.Replace("_chunk", " chunk "),
                                   GetArgs(1, chunkStart, chunkEnd, name, p.Script), s.ProcessPriority)
                            Encode("Video encoding pass 2" + name.Replace("_chunk", " chunk "),
                                   GetArgs(2, chunkStart, chunkEnd, name, p.Script), s.ProcessPriority)
                        End Sub)
            Else
                ret.Add(Sub() Encode("Video encoding" + name.Replace("_chunk", " chunk "),
                    GetArgs(1, chunkStart, chunkEnd, name, p.Script), s.ProcessPriority))
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

    Overrides Sub ShowConfigDialog()
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
        .Init = 1,
        .Config = {1, 32}}



    Property Preset As New OptionParam With {
        .Switch = "--preset",
        .Text = "Preset",
        .Options = {"-2: Debug Option 2", "-1: Debug Option 1", "0: Slowest", "1: Extreme Slow", "2: Ultra Slow", "3: Very Slow", "4: Slower", "5: Slow", "6: Medium", "7: Fast", "8: Faster", "9: Very Fast", "10: Mega Fast", "11: Ultra Fast", "12: Extreme Fast", "13: Fastest"},
        .Values = {"-2", "-1", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13"},
        .Init = 12}

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

    Property EnableHdr As New BoolParam With {
        .Switch = "--enable-hdr",
        .Text = "Enable HDR",
        .Init = False
    }



    Property RateControlMode As New OptionParam With {
        .HelpSwitch = "--rc",
        .Name = "RateControlMode",
        .Text = "Rate Control Mode",
        .Options = {"Quality", "Variable Bitrate", "Constant Bitrate"},
        .IntegerValue = True,
        .AlwaysOn = True,
        .Value = 0}

    Property ConstantRateFactor As New NumParam With {
        .HelpSwitch = "--crf",
        .Name = "ConstantRateFactor",
        .Text = "Constant Rate Factor",
        .DefaultValue = 35,
        .Value = 35,
        .VisibleFunc = Function() RateControlMode.Value = SvtAv1EncRateMode.Quality AndAlso AqMode.Value <> 0,
        .ValueChangedAction = Sub(x) QuantizationParameter.Value = CInt(x),
        .Config = {1, 63, 1}}

    Property QuantizationParameter As New NumParam With {
        .HelpSwitch = "--qp",
        .Name = "QuantizationParameter",
        .Text = "Quantization Parameter",
        .DefaultValue = 35,
        .Value = 35,
        .VisibleFunc = Function() RateControlMode.Value = SvtAv1EncRateMode.Quality AndAlso AqMode.Value = 0,
        .ValueChangedAction = Sub(x) ConstantRateFactor.Value = CInt(x),
        .Config = {1, 63, 1}}

    Property MaximumBitrate As New NumParam With {
        .Switch = "--mbr",
        .Text = "Maximum Bitrate",
        .Init = 0,
        .VisibleFunc = Function() RateControlMode.Value = SvtAv1EncRateMode.Quality,
        .Config = {0, 100000, 100}}

    Property TargetBitrate As New NumParam With {
        .HelpSwitch = "--tbr",
        .Text = "Target Bitrate",
        .Init = 7000,
        .VisibleFunc = Function() RateControlMode.Value <> SvtAv1EncRateMode.Quality,
        .Config = {100, 100000, 100}}

    Property MaxQp As New NumParam With {
        .Switch = "--max-qp",
        .Text = "Maximum Quantizer",
        .Init = 63,
        .VisibleFunc = Function() RateControlMode.Value <> SvtAv1EncRateMode.Quality,
        .Config = {1, 63, 1}}

    Property MinQp As New NumParam With {
        .Switch = "--min-qp",
        .Text = "Minimum Quantizer",
        .Init = 1,
        .VisibleFunc = Function() RateControlMode.Value <> SvtAv1EncRateMode.Quality,
        .Config = {1, 63, 1}}

    Property AqMode As New OptionParam With {
        .Switch = "--aq-mode",
        .Text = "Adaptive Quantization",
        .IntegerValue = True,
        .Init = 2,
        .Options = {"0: Off", "1: Variance base using AV1 segments", "2: Deltaq pred efficiency"}}

    Property EnableQm As New BoolParam With {
        .Switch = "--enable-qm",
        .Text = "Enable quantisation matrices",
        .Init = False
    }

    Property QmMax As New NumParam With {
        .Switch = "--qm-max",
        .Text = "Max quant matrix flatness",
        .Init = 15,
        .VisibleFunc = Function() EnableQm.Value,
        .Config = {0, 15, 1}}

    Property QmMin As New NumParam With {
        .Switch = "--qm-min",
        .Text = "Min quant matrix flatness",
        .Init = 8,
        .VisibleFunc = Function() EnableQm.Value,
        .Config = {0, 15, 1}}




    Property Passes As New OptionParam With {
        .Switches = {"--pass", "--passes", "--stats"},
        .Text = "Passes",
        .Init = 0,
        .Options = {"One-pass encode", "Multi-pass encode"},
        .Values = {"1", "2"}}


    Property KeyInt As New OptionParam With {
        .Switch = "--keyint",
        .Text = "GOP size",
        .Options = {"-2: ~5 seconds", "0: ""infinite"""},
        .Values = {"-2", "0"},
        .VisibleFunc = Function() Not ConstantRateFactor.Visible,
        .ValueChangedAction = Sub(x) KeyIntCrf.Value = x
    }

    Property KeyIntCrf As New OptionParam With {
        .Switch = "--keyint",
        .Text = "GOP size",
        .Options = {"-2: ~5 seconds", "-1: ""infinite"""},
        .Values = {"-2", "-1"},
        .VisibleFunc = Function() ConstantRateFactor.Visible,
        .ValueChangedAction = Sub(x) KeyInt.Value = x
    }


    Property FrameSkip As New NumParam With {
        .HelpSwitch = "--skip",
        .Text = "Frames To Be Skipped"}

    Property FramesToBeEncoded As New NumParam With {
        .HelpSwitch = "--n",
        .Text = "Frames To Be Encoded"}


    Property FilmGrain As New NumParam With {
        .Switch = "--film-grain", 
        .Text = "Film Grain Denoising Level", 
        .Init = 0, 
        .Config = {0, 50, 1}
    }

    Property FilmGrainDenoise As New OptionParam With {
        .Switch = "--film-grain-denoise", 
        .Text = "Film Grain Denoise", 
        .Init = 1, 
        .IntegerValue = True,
        .Options = {"0: no denoising, film grain data is still in frame header", "1: level of denoising is set by the film-grain parameter"},
        .VisibleFunc = Function() FilmGrain.Value > 0
    }

    Property Progress As New OptionParam With {
        .Switch = "--progress",
        .Text = "Progress",
        .DefaultValue = 1,
        .Value = 2,
        .Options = {"0: No Output", "1: Normal (Default)", "2: AOMEnc Style Output"},
        .Values = {"0", "1", "2"}
    }

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
        .VisibleFunc = Function() Passes.Value > 0,
        .InitAction = Sub(tb)
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = g.GetCodeFont
                      End Sub}

    Property CustomSecondPass As New StringParam With {
        .Text = "Custom" + BR + "Second Pass",
        .Quotes = QuotesMode.Never,
        .VisibleFunc = Function() Passes.Value > 0,
        .InitAction = Sub(tb)
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = g.GetCodeFont
                      End Sub}



    Overrides ReadOnly Property Items As List(Of CommandLineParam)
        Get
            If ItemsValue Is Nothing Then
                ItemsValue = New List(Of CommandLineParam)

                Add("Basic", Decoder, PipingToolAVS, PipingToolVS,
                    Progress,
                    Preset, Profile, Level, 
                    Passes, EnableHdr,
                    FrameSkip, FramesToBeEncoded,
                    CompCheck, CompCheckAimedQuality,
                    Chunks
                )
                Add("Rate Control",
                    RateControlMode, ConstantRateFactor, QuantizationParameter, TargetBitrate, MaximumBitrate, MaxQp, MinQp, AqMode,
                    EnableQm, QmMax, QmMin
                )
                Add("GOP size/type",
                    KeyInt, KeyIntCrf,
                    New OptionParam With {.Switch = "--irefresh-type", .Text = "Intra Refresh Type", .Options = {"1: FWD Frame (Open GOP)", "2: Key Frame (Closed GOP)"}, .Values = {"1", "2"}},
                    New BoolParam With {.Switch = "--scd", .Text = "Scene Change Detection Control", .Init = False, .IntegerValue = True},
                    New NumParam With {.Switch = "--lookahead", .Text = "Lookahead", .Init = -1, .Config = {-1, 120, 1}}
                )
                Add("AV1 Specific 1",
                    New NumParam With {.Switch = "--tile-rows", .Text = "Tile Rows", .Init = 1, .Config = {0, 6, 1}},
                    New NumParam With {.Switch = "--tile-columns", .Text = "Tile Columns", .Init = 1, .Config = {0, 4, 1}},
                    New BoolParam With {.Switch = "--enable-dlf", .Text = "Deblocking Loop Filter", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-cdef", .Text = "Constrained Directional Enhancement Filter", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-restoration", .Text = "Loop Restoration Filter", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-tpl-la", .Text = "Temporal Dependency Model", .Init = True, .IntegerValue = True, .VisibleFunc = Function() RateControlMode.Value = SvtAv1EncRateMode.Quality},
                    New NumParam With {.Switch = "--enable-mfmv", .Text = "Motion Field Motion Vector", .Init = -1, .Config = {-1, 1, 1}},
                    New BoolParam With {.Switch = "--enable-dg", .Text = "Dynamic GoP", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--fast-decode", .Text = "Fast Decoder Levels", .Init = False, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-tf", .Text = "ALT-REF Frames", .Init = True, .IntegerValue = True},
                    New BoolParam With {.Switch = "--enable-overlays", .Text = "Insertion of Overlayer Pictures", .Init = False, .IntegerValue = True},
                    New OptionParam With {.Switch = "--tune", .Text = "Tune", .Init = 1, .IntegerValue = True, .Options = {"0: VQ", "1: PSNR"}},
                    New OptionParam With {.Switch = "--scm", .Text = "Screen Content Detection Level", .Init = 2, .IntegerValue = True, .Options = {"0: Off", "1: On", "2: Content Adaptive"}},
                    New BoolParam With {.Switch = "--rmv", .Text = "Restrict Motion Vectors", .Init = False, .IntegerValue = True}
                )
                Add("AV1 Specific 2",
                    FilmGrain, FilmGrainDenoise,
                    New OptionParam With {.Switch = "--superres-mode", .Text = "Superres Mode", .Init = 1, .IntegerValue = True, .Options = {"0: Off", "1", "2", "3", "4: Auto-Select"}}
                )
                Add("Color Description",
                    New OptionParam With {.Switch = "--color-primaries", .Text = "Color Primaries", .Options = {"Unspecified", "BT.709", "BT.470 System M (historical)", "BT.470 System B, G (historical)", "BT.601", "SMPTE 240", "Generic film (color filters using illuminant C)", "BT.2020, BT.2100", "SMPTE 428 (CIE 1921 XYZ)", "SMPTE RP 431-2", "SMPTE EG 432-1", "EBU Tech. 3213-E"}, .Values = {"2", "1", "4", "5", "6", "7", "8", "9", "10", "11", "12", "22"}},
                    New OptionParam With {.Switch = "--transfer-characteristics", .Text = "Transfer Characteristics", .Options = {"Unspecified", "BT.709", "BT.470 System M (historical)", "BT.470 System B, G (historical)", "BT.601", "SMPTE 240 M", "Linear", "Logarithmic (100 : 1 range)", "Logarithmic (100 * Sqrt(10) : 1 range)", "IEC 61966-2-4", "BT.1361", "sRGB or sYCC", "BT.2020 10-bit systems", "BT.2020 12-bit systems", "SMPTE ST 2084, ITU BT.2100 PQ", "SMPTE ST 428", "BT.2100 HLG, ARIB STD-B67"}, .Values = {"2", "1", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18"}},
                    New OptionParam With {.Switch = "--matrix-coefficients", .Text = "Matrix Coefficients", .Options = {"Unspecified", "Identity matrix", "BT.709", "US FCC 73.628", "BT.470 System B, G (historical)", "BT.601", "SMPTE 240 M", "YCgCo", "BT.2020 non-constant luminance, BT.2100 YCbCr", "BT.2020 constant luminance", "SMPTE ST 2085 YDzDx", "Chromaticity-derived non-constant luminance", "Chromaticity-derived constant luminance", "BT.2100 ICtCp"}, .Values = {"2", "0", "1", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14"}},
                    New OptionParam With {.Switch = "--color-range", .Text = "Color Range", .IntegerValue = True, .Options = {"0: Studio", "1: Full"}}
                )
                Add("Custom", Custom, CustomFirstPass, CustomSecondPass)

                'ItemsValue = ItemsValue.OrderBy(Function(i) i.Weight).ToList
            End If

            Return ItemsValue
        End Get
    End Property

    Overrides Function GetCommandLinePreview() As String
        Dim ret = GetCommandLine(True, True, 1)

        If Passes.Value > 0 Then
            ret += BR2 + GetCommandLine(True, True, 2)
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
                            pipeString = Package.vspipe.Path.Escape + " " + script.Path.Escape + " - --y4m" + chunk

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
            sb.Append($" --output {targetPath.Escape}")
        End If

        If isSingleChunk Then
            If FrameSkip.Value > 0 AndAlso Not IsCustom(pass, "--skip") Then
                sb.Append($" --skip {FrameSkip.Value}")
            End If

            If FramesToBeEncoded.Value > 0 AndAlso Not IsCustom(pass, "--n") Then
                sb.Append($" --n {Math.Min(script.GetFrameCount - FrameSkip.Value, FramesToBeEncoded.Value)}")
            End If
            'Else
            '    If includePaths Then
            '        sb.Append($" --skip {startFrame} --n {endFrame - startFrame + 1}")
            '    End If
        End If

        If Passes.Value > 0 Then
            sb.Append(" --passes " & Passes.ValueText)
            sb.Append(" --pass " & pass)
            sb.Append(" --stats " + statsPath)

            If pass = 1 Then
                If CustomFirstPass.Value <> "" Then
                    sb.Append(" " + CustomFirstPass.Value)
                End If
            Else
                If CustomSecondPass.Value <> "" Then
                    sb.Append(" " + CustomSecondPass.Value)
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
                    sb.Append(" --crf " + CInt(ConstantRateFactor.Value).ToString)
                End If
            ElseIf QuantizationParameter.Visible Then
                If Not IsCustom(pass, "--qp") AndAlso QuantizationParameter.Value <> QuantizationParameter.DefaultValue Then
                    sb.Append(" --qp " + CInt(QuantizationParameter.Value).ToString)
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
        If switch = "" Then
            Return False
        End If

        If Passes.Value > 0 Then
            If pass = 1 Then
                If CustomFirstPass.Value?.Contains(switch + " ") OrElse CustomFirstPass.Value?.EndsWith(switch) Then
                    Return True
                End If
            Else
                If CustomSecondPass.Value?.Contains(switch + " ") OrElse CustomSecondPass.Value?.EndsWith(switch) Then
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
