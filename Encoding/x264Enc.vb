
Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class x264Enc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Sub New()
        Name = "x264"
        Params.ApplyValues(True)
        Params.ApplyValues(False)
    End Sub

    <NonSerialized>
    Private ParamsValue As x264Params

    Property Params As x264Params
        Get
            If ParamsValue Is Nothing Then
                ParamsValue = New x264Params
                ParamsValue.Init(ParamsStore)
            End If

            Return ParamsValue
        End Get
        Set(value As x264Params)
            ParamsValue = value
        End Set
    End Property

    Overrides ReadOnly Property OutputExt As String
        Get
            If Params.Muxer.Value = 0 OrElse Params.Muxer.Value = 1 Then
                Return "h264"
            Else
                Return Params.Muxer.ValueText.ToLower
            End If
        End Get
    End Property

    Overrides Function GetFixedBitrate() As Integer
        Return CInt(Params.Bitrate.Value)
    End Function

    Overrides Sub Encode()
        p.Script.Synchronize()
        Encode("Video encoding", GetArgs(1, p.Script), s.ProcessPriority)

        If Params.Mode.Value = x264RateMode.TwoPass Then
            Encode("Video encoding second pass", GetArgs(2, p.Script), s.ProcessPriority)
        ElseIf Params.Mode.Value = x264RateMode.ThreePass Then
            Encode("Video encoding second pass", GetArgs(3, p.Script), s.ProcessPriority)
            Encode("Video encoding third pass", GetArgs(2, p.Script), s.ProcessPriority)
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(passName As String, commandLine As String, priority As ProcessPriorityClass)
        p.Script.Synchronize()

        Using proc As New Proc
            proc.Package = Package.x264
            proc.Header = passName
            proc.Priority = priority
            proc.SkipStrings = {"kb/s, eta", "%]"}

            If commandLine.Contains("|") Then
                proc.File = "cmd.exe"
                proc.Arguments = "/S /C """ + commandLine + """"
            Else
                proc.CommandLine = commandLine
            End If

            proc.Start()
        End Using
    End Sub

    Overrides Sub RunCompCheck()
        If Not g.VerifyRequirements OrElse Not g.IsValidSource Then
            Exit Sub
        End If

        Dim newParams As New x264Params
        Dim newStore = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(newStore)

        Dim enc As New x264Enc
        enc.Params = newParams
        enc.Params.Mode.Value = x264RateMode.Quality
        enc.Params.Quant.Value = enc.Params.CompCheck.Value

        Dim script As New VideoScript
        script.Engine = p.Script.Engine
        script.Filters = p.Script.GetFiltersCopy
        Dim code As String
        Dim every = ((100 \ p.CompCheckRange) * 14).ToString

        If script.Engine = ScriptEngine.AviSynth Then
            code = "SelectRangeEvery(" + every + ",14)"
        Else
            code = "fpsnum = clip.fps_num" + BR + "fpsden = clip.fps_den" + BR +
                "clip = core.std.SelectEvery(clip = clip, cycle = " + every + ", offsets = range(14))" + BR +
                "clip = core.std.AssumeFPS(clip = clip, fpsnum = fpsnum, fpsden = fpsden)"
        End If

        script.Filters.Add(New VideoFilter("aaa", "aaa", code))
        script.Path = (p.TempDir + p.TargetFile.Base + "_CompCheck." + script.FileType).ToShortFilePath
        script.Synchronize()

        Log.WriteLine(BR + script.GetFullScript + BR)

        Dim commandLine = enc.Params.GetArgs(0, script, p.TempDir + p.TargetFile.Base + "_CompCheck." + OutputExt, True, True)

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

    Overloads Function GetArgs(pass As Integer, script As VideoScript, Optional includePaths As Boolean = True) As String
        Return Params.GetArgs(pass, script, OutputPath.DirAndBase + OutputExtFull, includePaths, True)
    End Function

    Overrides Sub ShowConfigDialog()
        Dim newParams As New x264Params
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(store)
        newParams.ApplyValues(True)

        Using form As New CommandLineForm(newParams)
            form.HTMLHelp = "<h2>x264 Help</h2>" +
                "<p>Right-clicking a option shows the local console help for the option, pressing Ctrl or Shift while right-clicking a option shows the online help for the option.</p>" +
                "<p>Setting the Bitrate option to 0 will use the bitrate defined in the project/template in the main dialog.</p>" +
               $"<h2>x264 Online Help</h2><p><a href=""{Package.x264.HelpURL}"">x264 Online Help</a></p>" +
               $"<h2>x264 Console Help</h2><pre>{HelpDocument.ConvertChars(Package.x264.CreateHelpfile())}</pre>"

            Dim saveProfileAction = Sub()
                                        Dim enc = ObjectHelp.GetCopy(Of x264Enc)(Me)
                                        Dim params2 As New x264Params
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            ActionMenuItem.Add(form.cms.Items, "Save Profile...", saveProfileAction).SetImage(Symbol.Save)

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
            Return Params.Mode.Value = x264RateMode.Quantizer OrElse Params.Mode.Value = x264RateMode.Quality
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
        Return New x264Control(Me) With {.Dock = DockStyle.Fill}
    End Function

    Shared Function Test() As String
        Dim tester As New ConsolAppTester

        tester.IgnoredSwitches = "help longhelp fullhelp progress"
        tester.UndocumentedSwitches = "y4m"
        tester.Package = Package.x264
        tester.CodeFile = Folder.Startup.Parent + "Encoding\x264Enc.vb"

        Return tester.Test
    End Function
End Class

Public Class x264Params
    Inherits CommandLineParams

    Sub New()
        Title = "x264 Options"
    End Sub

    Property Mode As New OptionParam With {
        .Name = "Mode",
        .Text = "Mode",
        .Switches = {"--bitrate", "--qp", "--crf", "--pass", "--stats", "-q", "-B", "-p"},
        .Options = {"Bitrate", "Quantizer", "Quality", "Two Pass", "Three Pass"},
        .Value = 2}

    Property Quant As New NumParam With {
        .Switches = {"--crf", "--qp"},
        .Name = "Quant",
        .Text = "Quality",
        .Init = 22,
        .VisibleFunc = Function() Mode.Value = 1 OrElse Mode.Value = 2,
        .Config = {0, 69, 0.5, 1}}

    Property Bitrate As New NumParam With {
        .HelpSwitch = "--bitrate",
        .Text = "Bitrate",
        .VisibleFunc = Function() Mode.Value <> 1 AndAlso Mode.Value <> 2,
        .Config = {0, 1000000, 100}}

    Property Preset As New OptionParam With {
        .Switch = "--preset",
        .Text = "Preset",
        .Options = {"Ultra Fast", "Super Fast", "Very Fast", "Faster", "Fast", "Medium", "Slow", "Slower", "Very Slow", "Placebo"},
        .Init = 5}

    Property Tune As New OptionParam With {
        .Switch = "--tune",
        .Text = "Tune",
        .Options = {"None", "Film", "Animation", "Grain", "Still Image", "PSNR", "SSIM", "Fast Decode", "Zero Latency"}}

    Property CompCheck As New NumParam With {
        .Name = "CompCheckQuant",
        .Text = "Comp. Check",
        .Help = "CRF value used as 100%",
        .Value = 18,
        .Config = {1, 50}}

    Property CompCheckAimedQuality As New NumParam With {
        .Name = "CompCheckAimedQuality",
        .Text = "Aimed Quality",
        .Value = 50,
        .Help = "Percent value to adjusts the target file size or image size after the compressibility check accordingly.",
        .Config = {1, 100}}

    Property Custom As New StringParam With {
        .Text = "Custom",
        .Quotes = QuotesMode.Never,
        .AlwaysOn = True,
        .InitAction = Sub(tb)
                          tb.Edit.Expand = True
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = New Font("Consolas", 10 * s.UIScaleFactor)
                      End Sub}

    Property CustomFirstPass As New StringParam With {
        .Text = "Custom" + BR + "First Pass",
        .Quotes = QuotesMode.Never,
        .InitAction = Sub(tb)
                          tb.Edit.Expand = True
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = New Font("Consolas", 10 * s.UIScaleFactor)
                      End Sub}

    Property CustomSecondPass As New StringParam With {
        .Text = "Custom" + BR + "Second Pass",
        .Quotes = QuotesMode.Never,
        .InitAction = Sub(tb)
                          tb.Edit.Expand = True
                          tb.Edit.TextBox.Multiline = True
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = New Font("Consolas", 10 * s.UIScaleFactor)
                      End Sub}

    Property Deblock As New BoolParam With {
        .Switch = "--deblock",
        .NoSwitch = "--no-deblock",
        .Switches = {"-f"},
        .Text = "Deblocking",
        .ImportAction = Sub(param, arg)
                            Dim a = arg.Split(":"c)
                            DeblockA.Value = a(0).ToInt
                            DeblockB.Value = a(1).ToInt
                        End Sub,
        .ArgsFunc = Function() As String
                        If Deblock.Value Then
                            If DeblockA.Value = DeblockA.DefaultValue AndAlso
                                DeblockB.Value = DeblockB.DefaultValue AndAlso
                                Deblock.DefaultValue Then

                                Return Nothing
                            Else
                                Return "--deblock " & DeblockA.Value & ":" & DeblockB.Value
                            End If
                        ElseIf Deblock.DefaultValue Then
                            Return "--no-deblock"
                        End If
                    End Function}

    Property DeblockA As New NumParam With {
        .Text = "      Strength",
        .Config = {-6, 6}}

    Property DeblockB As New NumParam With {
        .Text = "      Threshold",
        .Config = {-6, 6}}

    Property BFrames As New NumParam With {
        .Switch = "--bframes",
        .Switches = {"-b"},
        .Text = "B-Frames",
        .Config = {0, 16}}

    Property AqMode As New OptionParam With {
        .Switch = "--aq-mode",
        .Text = "AQ Mode",
        .IntegerValue = True,
        .Expand = True,
        .Options = {"Disabled", "Variance AQ", "Auto-variance AQ", "Auto-variance AQ with bias to dark scenes"}}

    Property BAdapt As New OptionParam With {
        .Switch = "--b-adapt",
        .Text = "B-Adapt",
        .IntegerValue = True,
        .Options = {"Disabled", "Fast", "Optimal"}}

    Property Cabac As New BoolParam With {
        .NoSwitch = "--no-cabac",
        .Text = "Cabac"}

    Property Weightp As New OptionParam With {
        .Switch = "--weightp",
        .Text = "Weight P Prediction",
        .Expand = True,
        .Options = {"Disabled", "Weighted refs", "Weighted refs + Duplicates"},
        .IntegerValue = True}

    Property Profile As New OptionParam With {
        .Switch = "--profile",
        .Text = "Profile",
        .Options = {"Automatic", "Baseline", "Main", "High", "High 10", "High 422", "High 444"}}

    Property CQM As New OptionParam With {
        .Switch = "--cqm",
        .Options = {"Flat", "JVT"},
        .Text = "Quant Matrice Preset"}

    Property Mbtree As New BoolParam With {
        .NoSwitch = "--no-mbtree",
        .Text = "MB Tree"}

    Property I4x4 As New BoolParam With {
        .Switches = {"--partitions", "-A"},
        .Label = "Partitions",
        .ArgsFunc = AddressOf GetPartitionsArg,
        .LeftMargin = g.MainForm.FontHeight * 1.5,
        .Text = "i4x4"}

    Property P4x4 As New BoolParam With {
        .Switches = {"--partitions", "-A"},
        .LeftMargin = g.MainForm.FontHeight * 1.5,
        .Text = "p4x4"}

    Property B8x8 As New BoolParam With {
        .Switches = {"--partitions", "-A"},
        .LeftMargin = g.MainForm.FontHeight * 1.5,
        .Text = "b8x8"}

    Property I8x8 As New BoolParam With {
        .Switches = {"--partitions", "-A"},
        .LeftMargin = g.MainForm.FontHeight * 1.5,
        .Text = "i8x8"}

    Property P8x8 As New BoolParam With {
        .Switches = {"--partitions", "-A"},
        .LeftMargin = g.MainForm.FontHeight * 1.5,
        .Text = "p8x8"}

    Property _8x8dct As New BoolParam With {
        .Switch = "--8x8dct",
        .NoSwitch = "--no-8x8dct",
        .Text = "8x8dct",
        .LeftMargin = g.MainForm.FontHeight * 1.5}

    Property RcLookahead As New NumParam With {
        .Switch = "--rc-lookahead",
        .Text = "Lookahead"}

    Property Ref As New NumParam With {
        .Switch = "--ref",
        .Switches = {"-r"},
        .Text = "Ref Frames"}

    Property Scenecut As New NumParam With {
        .Switch = "--scenecut",
        .Text = "Scenecut"}

    Property Subme As New OptionParam With {
        .Switch = "--subme",
        .Switches = {"-m"},
        .Text = "Subpel Refinement",
        .IntegerValue = True,
        .Expand = True,
        .Options = {"Fullpel only (not recommended)", "SAD mode decision, one qpel iteration", "SATD mode decision", "Progressively more qpel", "Progressively more qpel", "Progressively more qpel", "RD mode decision for I/P-frames", "RD mode decision for all frames", "RD refinement for I/P-frames", "RD refinement for all frames", "QP-RD - requires trellis=2, aq-mode>0", "Full RD disable all early terminations"}}

    Property Me_ As New OptionParam With {
        .Switch = "--me",
        .Text = "Motion Search Method",
        .Expand = True,
        .Values = {"dia", "hex", "umh", "esa", "tesa"},
        .Options = {"Diamond Search, Radius 1 (fast)", "Hexagonal Search, Radius 2", "Uneven Multi-Hexagon Search", "Exhaustive Search", "Hadamard Exhaustive Search (slow)"}}

    Property Weightb As New BoolParam With {
        .NoSwitch = "--no-weightb",
        .Text = "Weighted prediction for B-frames"}

    Property Trellis As New OptionParam With {
        .Switch = "--trellis",
        .Switches = {"-t"},
        .Text = "Trellis",
        .Expand = True,
        .IntegerValue = True,
        .Options = {"Disabled", "Enabled only on the final encode of a MB", "Enabled on all mode decisions"}}

    Property Direct As New OptionParam With {
        .Switch = "--direct",
        .Text = "Direct MV Prediction",
        .Options = {"None", "Spatial", "Temporal", "Auto"}}

    Property Merange As New NumParam With {
        .Switch = "--merange",
        .Text = "ME Range"}

    Property Fastpskip As New BoolParam With {
        .NoSwitch = "--no-fast-pskip",
        .Text = "Fast Pskip"}

    Property Psy As New BoolParam With {
        .Switch = "--psy-rd",
        .NoSwitch = "--no-psy",
        .Text = "Psy RD",
        .ArgsFunc = Function() As String
                        If Psy.Value Then
                            If PsyRD.Value <> PsyRD.DefaultValue OrElse
                                PsyTrellis.Value <> PsyTrellis.DefaultValue OrElse
                                Not Psy.DefaultValue Then

                                Return "--psy-rd " & PsyRD.Value.ToInvariantString & ":" & PsyTrellis.Value.ToInvariantString
                            End If
                        Else
                            If Psy.DefaultValue Then
                                Return "--no-psy"
                            End If
                        End If
                    End Function}

    Property PsyRD As New NumParam With {
        .Config = {0, 0, 0.05, 2},
        .Text = "     RD"}

    Property PsyTrellis As New NumParam With {
        .Config = {0, 0, 0.05, 2},
        .Text = "     Trellis"}

    Property AqStrength As New NumParam With {
        .Switch = "--aq-strength",
        .Text = "AQ Strength",
        .Config = {0, 0, 0.1, 1}}

    Property DctDecimate As New BoolParam With {
        .NoSwitch = "--no-dct-decimate",
        .Text = "DCT Decimate"}

    Property DeadzoneInter As New NumParam With {
        .Switch = "--deadzone-inter",
        .Text = "Deadzone Inter",
        .Config = {0, 32}}

    Property DeadzoneIntra As New NumParam With {
        .Switch = "--deadzone-intra",
        .Text = "Deadzone Intra",
        .Config = {0, 32}}

    Property MixedRefs As New BoolParam With {
        .NoSwitch = "--no-mixed-refs",
        .Text = "Mixed References"}

    Property ForceCFR As New BoolParam With {
        .Switch = "--force-cfr",
        .Text = "Force constant framerate timestamp generation"}

    Property Ipratio As New NumParam With {
        .Switch = "--ipratio",
        .Text = "IP Ratio",
        .Config = {0, 0, 0.1, 1}}

    Property Pbratio As New NumParam With {
        .Switch = "--pbratio",
        .Text = "PB Ratio",
        .Config = {0, 0, 0.1, 1}}

    Property Qcomp As New NumParam With {
        .Text = "QComp",
        .Switch = "--qcomp",
        .Config = {0, 0, 0.1, 1}}

    Property Muxer As New OptionParam With {
        .Switch = "--muxer",
        .Text = "Output Format",
        .Options = {"Automatic", "RAW", "MKV", "FLV", "MP4"}}

    Property Demuxer As New OptionParam With {
        .Switch = "--demuxer",
        .Text = "Demuxer",
        .Options = {"Automatic", "RAW", "Y4M", "AVS", "LAVF", "FFMS"}}

    Property SlowFirstpass As New BoolParam With {
        .Switch = "--slow-firstpass",
        .Text = "Slow Firstpass"}

    Property PipingToolAVS As New OptionParam With {
        .Text = "Pipe",
        .Name = "PipingToolAVS",
        .VisibleFunc = Function() p.Script.Engine = ScriptEngine.AviSynth,
        .Options = {"Automatic", "None", "avs2pipemod y4m", "avs2pipemod raw", "ffmpeg y4m", "ffmpeg raw"}}

    Property PipingToolVS As New OptionParam With {
        .Text = "Pipe",
        .Name = "PipingToolVS",
        .VisibleFunc = Function() p.Script.Engine = ScriptEngine.VapourSynth,
        .Options = {"Automatic", "None", "vspipe y4m", "vspipe raw", "ffmpeg y4m", "ffmpeg raw"}}

    Sub ApplyValues(isDefault As Boolean)
        Dim setVal = Sub(param As CommandLineParam, value As Object)
                         If TypeOf param Is BoolParam Then
                             If isDefault Then
                                 DirectCast(param, BoolParam).DefaultValue = CBool(value)
                             Else
                                 DirectCast(param, BoolParam).Value = CBool(value)
                             End If
                         ElseIf TypeOf param Is NumParam Then
                             If isDefault Then
                                 DirectCast(param, NumParam).DefaultValue = CDbl(value)
                             Else
                                 DirectCast(param, NumParam).Value = CDbl(value)
                             End If
                         ElseIf TypeOf param Is OptionParam Then
                             If isDefault Then
                                 DirectCast(param, OptionParam).DefaultValue = CInt(value)
                             Else
                                 DirectCast(param, OptionParam).Value = CInt(value)
                             End If
                         End If
                     End Sub

        setVal(Deblock, True)
        setVal(DeblockA, 0)
        setVal(DeblockB, 0)
        setVal(BFrames, 3)
        setVal(AqMode, 1)
        setVal(BAdapt, 1)
        setVal(Cabac, True)
        setVal(Weightp, 2)
        setVal(Mbtree, True)
        setVal(Me_, 1)
        setVal(MixedRefs, True)
        setVal(I4x4, True)
        setVal(P4x4, False)
        setVal(B8x8, True)
        setVal(I8x8, True)
        setVal(P8x8, True)
        setVal(_8x8dct, True)
        setVal(RcLookahead, 40)
        setVal(Ref, 3)
        setVal(Scenecut, 40)
        setVal(Subme, 7)
        setVal(Trellis, 1)
        setVal(Weightb, True)
        setVal(Direct, 1)
        setVal(Merange, 16)
        setVal(Fastpskip, True)
        setVal(Psy, True)
        setVal(PsyRD, 1)
        setVal(PsyTrellis, 0)
        setVal(AqStrength, 1)
        setVal(DctDecimate, True)
        setVal(DeadzoneInter, 21)
        setVal(DeadzoneIntra, 11)
        setVal(Ipratio, 1.4)
        setVal(Pbratio, 1.3)
        setVal(Qcomp, 0.6)
        setVal(ForceCFR, False)
        setVal(SlowFirstpass, False)

        Select Case Preset.Value
            Case 0 'ultrafast
                setVal(Deblock, False)
                setVal(_8x8dct, False)
                setVal(BFrames, 0)
                setVal(AqMode, 0)
                setVal(BAdapt, 0)
                setVal(Cabac, False)
                setVal(Weightp, 0)
                setVal(Mbtree, False)
                setVal(Me_, 0)
                setVal(MixedRefs, False)
                setVal(RcLookahead, 0)
                setVal(Ref, 1)
                setVal(Scenecut, 0)
                setVal(Subme, 0)
                setVal(Trellis, 0)
                setVal(Weightb, False)
            Case 1 'superfast
                setVal(Weightp, 1)
                setVal(Mbtree, False)
                setVal(Me_, 0)
                setVal(MixedRefs, False)
                setVal(P4x4, False)
                setVal(B8x8, False)
                setVal(P8x8, False)
                setVal(RcLookahead, 0)
                setVal(Ref, 1)
                setVal(Subme, 1)
                setVal(Trellis, 0)
            Case 2 'veryfast
                setVal(Weightp, 1)
                setVal(MixedRefs, False)
                setVal(RcLookahead, 10)
                setVal(Ref, 1)
                setVal(Subme, 2)
                setVal(Trellis, 0)
            Case 3 'faster
                setVal(Weightp, 1)
                setVal(MixedRefs, False)
                setVal(RcLookahead, 20)
                setVal(Ref, 2)
                setVal(Subme, 4)
            Case 4 'fast
                setVal(Weightp, 1)
                setVal(RcLookahead, 30)
                setVal(Ref, 2)
                setVal(Subme, 6)
            Case 5 'medium
            Case 6 'slow
                setVal(RcLookahead, 50)
                setVal(Ref, 5)
                setVal(Subme, 8)
                setVal(Trellis, 2)
                setVal(Direct, 3)
            Case 7 'slower
                setVal(BAdapt, 2)
                setVal(Me_, 2)
                setVal(RcLookahead, 60)
                setVal(Ref, 8)
                setVal(Subme, 9)
                setVal(Trellis, 2)
                setVal(Direct, 3)
            Case 8 'veryslow
                setVal(BFrames, 8)
                setVal(BAdapt, 2)
                setVal(Me_, 2)
                setVal(RcLookahead, 60)
                setVal(RcLookahead, 60)
                setVal(Ref, 16)
                setVal(Subme, 10)
                setVal(Trellis, 2)
                setVal(Direct, 3)
                setVal(Merange, 24)
            Case 9 'placebo
                setVal(BFrames, 16)
                setVal(BAdapt, 2)
                setVal(Me_, 4)
                setVal(RcLookahead, 60)
                setVal(Ref, 16)
                setVal(Subme, 11)
                setVal(Trellis, 2)
                setVal(Direct, 3)
                setVal(Merange, 24)
                setVal(Fastpskip, False)
                setVal(SlowFirstpass, True)
        End Select

        Select Case Tune.Value
            Case 1 'film
                setVal(DeblockA, -1)
                setVal(DeblockB, -1)
                setVal(PsyTrellis, 0.15)
            Case 2 'animation
                setVal(DeblockA, 1)
                setVal(DeblockB, 1)
                Dim val = If(isDefault, BFrames.DefaultValue, BFrames.Value) + 2
                setVal(BFrames, If(val > 16, 16, val))
                setVal(PsyRD, 0.4)
                setVal(AqStrength, 0.6)
            Case 3 'grain
                setVal(DeblockA, -2)
                setVal(DeblockB, -2)
                setVal(PsyTrellis, 0.25)
                setVal(AqStrength, 0.5)
                setVal(DctDecimate, False)
                setVal(DeadzoneInter, 6)
                setVal(DeadzoneIntra, 6)
                setVal(Ipratio, 1.1)
                setVal(Pbratio, 1.1)
                setVal(Qcomp, 0.8)
            Case 4 'stillimage
                setVal(DeblockA, -3)
                setVal(DeblockB, -3)
                setVal(PsyRD, 2)
                setVal(PsyTrellis, 0.7)
                setVal(AqStrength, 1.2)
            Case 5 'psnr
                setVal(AqMode, 0)
                setVal(Psy, False)
            Case 6 'ssim
                setVal(AqMode, 2)
                setVal(Psy, False)
            Case 7 'fastdecode
                setVal(Deblock, False)
                setVal(Cabac, False)
                setVal(Weightp, 0)
                setVal(Weightb, False)
            Case 8 'zerolatency
                setVal(BFrames, 0)
                setVal(Mbtree, False)
                setVal(RcLookahead, 0)
                setVal(ForceCFR, True)
        End Select

        Select Case Profile.Value
            Case 1 'baseline
                setVal(Cabac, False)
                setVal(_8x8dct, False)
                setVal(BFrames, 0)
                setVal(Weightp, 0)
            Case 2 'main
                setVal(_8x8dct, False)
        End Select
    End Sub

    Overrides ReadOnly Property Items As List(Of CommandLineParam)
        Get
            If ItemsValue Is Nothing Then
                ItemsValue = New List(Of CommandLineParam)

                Add("Basic",
                    Mode,
                    Preset,
                    Tune,
                    Profile,
                    New OptionParam With {.Switch = "--level", .Text = "Level", .Options = {"Automatic", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                    New OptionParam With {.Switch = "--output-depth", .Text = "Depth", .Options = {"8-Bit", "10-Bit"}, .Values = {"8", "10"}},
                    Quant,
                    Bitrate)
                Add("Analysis",
                    Trellis,
                    CQM,
                    I4x4,
                    P4x4,
                    B8x8,
                    I8x8,
                    P8x8,
                    _8x8dct,
                    Psy,
                    PsyRD,
                    PsyTrellis,
                    Fastpskip,
                    MixedRefs)
                Add("Analysis 2",
                    DeadzoneInter,
                    DeadzoneIntra,
                    New NumParam With {.Switch = "--mvrange", .Text = "MV Range", .Init = -1},
                    New NumParam With {.Switch = "--mvrange-thread", .Text = "MV Range Thread", .Init = -1},
                    New NumParam With {.Switch = "--nr", .Text = "Noise Reduction"})
                Add("Rate Control",
                    New StringParam With {.Switch = "--zones", .Text = "Zones"},
                    AqMode,
                    AqStrength,
                    Ipratio,
                    Pbratio,
                    Qcomp,
                    New NumParam With {.Switch = "--vbv-maxrate", .Text = "VBV Maxrate"},
                    New NumParam With {.Switch = "--vbv-bufsize", .Text = "VBV Bufsize"},
                    New NumParam With {.Switch = "--vbv-init", .Text = "VBV Init", .Config = {0.5, 1.0, 0.1, 1}, .Init = 0.9},
                    New NumParam With {.Switch = "--crf-max", .Text = "Maximum CRF"},
                    New NumParam With {.Switch = "--qpmin", .Text = "Minimum QP"},
                    New NumParam With {.Switch = "--qpmax", .Text = "Maximum QP", .Init = 69})
                Add("Rate Control 2",
                    New NumParam With {.Switch = "--qpstep", .Text = "QP Step", .Init = 4},
                    New NumParam With {.Switch = "--ratetol", .Text = "Rate Tolerance", .Config = {0, 0, 0.1, 1}, .Init = 1},
                    New NumParam With {.Switch = "--chroma-qp-offset", .Text = "Chroma QP Offset"},
                    New NumParam With {.Switch = "--cplxblur", .Text = "T. Blur Complexity.", .Config = {0, 0, 0.1, 1}, .Init = 20},
                    New NumParam With {.Switch = "--qblur", .Text = "Temp. Blur Quants", .Config = {0, 0, 0.1, 1}, .Init = 0.5},
                    Mbtree,
                    New BoolParam With {.Switch = "--cqm4", .Text = "Set all 4x4 quant matrices"},
                    New BoolParam With {.Switch = "--cqm8", .Text = "Set all 8x8 quant matrices"})
                Add("Rate Control 3",
                    New StringParam With {.Switch = "--qpfile", .Text = "QP File", .BrowseFile = True},
                    New StringParam With {.Switch = "--cqmfile", .Text = "CQM File", .BrowseFile = True},
                    New StringParam With {.Switch = "--cqm4i", .Text = "cqm4i"},
                    New StringParam With {.Switch = "--cqm4p", .Text = "cqm4p"},
                    New StringParam With {.Switch = "--cqm8i", .Text = "cqm8i"},
                    New StringParam With {.Switch = "--cqm8p", .Text = "cqm8p"},
                    New StringParam With {.Switch = "--cqm4iy", .Text = "cqm4iy"},
                    New StringParam With {.Switch = "--cqm4ic", .Text = "cqm4ic"},
                    New StringParam With {.Switch = "--cqm4py", .Text = "cqm4py"},
                    New StringParam With {.Switch = "--cqm4pc", .Text = "cqm4pc"})
                Add("Motion Search",
                    Subme,
                    Me_,
                    Weightp,
                    Direct,
                    Merange,
                    Weightb,
                    New BoolParam With {.NoSwitch = "--no-chroma-me", .Init = True, .Text = "Use chroma in motion estimation"})
                Add("Slice Decision",
                    BAdapt,
                    New OptionParam With {.Switch = "--b-pyramid", .Text = "B-Pyramid", .Init = 2, .Options = {"None", "Strict", "Normal"}},
                    BFrames,
                    New NumParam With {.Switch = "--b-bias", .Text = "B-Bias"},
                    RcLookahead,
                    Ref,
                    Scenecut,
                    New NumParam With {.Switch = "--keyint", .Switches = {"-I"}, .Text = "Max GOP Size", .Init = 250},
                    New NumParam With {.Switch = "--min-keyint", .Switches = {"-i"}, .Text = "Min GOP Size"},
                    New NumParam With {.Switch = "--slices", .Text = "Slices"},
                    New NumParam With {.Switch = "--slices-max", .Text = "Slices Max"},
                    New NumParam With {.Switch = "--slice-max-size", .Text = "Slice Max Size"},
                    New NumParam With {.Switch = "--slice-max-mbs", .Text = "Slice Max MBS"},
                    New NumParam With {.Switch = "--slice-min-mbs", .Text = "Slice Min MBS"})
                Add("Slice Decision 2",
                    DctDecimate,
                    New BoolParam With {.Switch = "--intra-refresh", .Text = "Periodic Intra Refresh instead of IDR frames"},
                    New BoolParam With {.Switch = "--open-gop", .Text = "Open GOP"})
                Add("VUI",
                    New StringParam With {.Switch = "--sar", .Text = "Sample AR", .Init = "auto", .Menu = s.ParMenu, .ArgsFunc = AddressOf GetSAR},
                    New StringParam With {.Switch = "--crop-rect", .Text = "Crop Rectangle"},
                    New OptionParam With {.Switch = "--videoformat", .Text = "Videoformat", .Options = {"Undefined", "Component", "PAL", "NTSC", "SECAM", "MAC"}},
                    New OptionParam With {.Switch = "--colorprim", .Text = "Colorprim", .Options = {"Undefined", "BT 2020", "BT 470 BG", "BT 470 M", "BT 709", "Film", "SMPTE 170 M", "SMPTE 240 M", "SMPTE 428", "SMPTE 431", "SMPTE 432"}},
                    New OptionParam With {.Switch = "--colormatrix", .Text = "Colormatrix", .Options = {"Undefined", "BT 2020 C", "BT 2020 NC", "BT 470 BG", "BT 709", "FCC", "GBR", "SMPTE 170 M", "SMPTE 2085", "SMPTE 240 M", "YCgCo", "Chroma Derived C", "ICtCp"}},
                    New OptionParam With {.Switch = "--transfer", .Text = "Transfer", .Options = {"Undefined", "BT 1361 E", "BT 2020-10", "BT 2020-12", "BT 470 BG", "BT 470 M", "BT 709", "IEC 61966-2-1", "IEC 61966-2-4", "Linear", "Log 100", "Log 316", "SMPTE 170 M", "SMPTE 2084", "SMPTE 240 M", "SMPTE 428"}},
                    New OptionParam With {.Switch = "--alternative-transfer", .Text = "Alternative Transfer", .Options = {"Undefined", "BT 1361 E", "BT 2020-10", "BT 2020-12", "BT 470 BG", "BT 470 M", "BT 709", "IEC 61966-2-1", "IEC 61966-2-4", "Linear", "Log 100", "Log 316", "SMPTE 170 M", "SMPTE 2084", "SMPTE 240 M", "SMPTE 428"}},
                    New OptionParam With {.Switch = "--overscan", .Text = "Overscan", .Options = {"Undefined", "Show", "Crop"}},
                    New OptionParam With {.Switch = "--range", .Text = "Range", .Options = {"Auto", "TV", "PC"}},
                    New OptionParam With {.Switch = "--nal-hrd", .Text = "Signal HRD Info", .Options = {"None", "VBR", "CBR"}},
                    New NumParam With {.Switch = "--chromaloc", .Text = "Chromaloc", .Config = {0, 5}},
                    New BoolParam With {.Switch = "--filler", .Text = "Force hard-CBR and generate filler"},
                    New BoolParam With {.Switch = "--pic-struct", .Text = "Force pic_struct in Picture Timing SEI"})
                Add("Input/Output",
                    New StringParam With {.Switch = "--opencl-clbin", .Text = "OpenCl clbin", .BrowseFile = True},
                    New StringParam With {.Switch = "--dump-yuv", .Text = "Dump YUV", .BrowseFile = True},
                    New StringParam With {.Switch = "--tcfile-in", .Text = "TC File In", .BrowseFile = True},
                    New StringParam With {.Switch = "--tcfile-out", .Text = "TC File Out", .BrowseFile = True},
                    New StringParam With {.Switch = "--index", .Text = "Index File", .BrowseFile = True},
                    New StringParam With {.Switch = "--timebase", .Text = "Timebase"},
                    New StringParam With {.Switch = "--input-fmt", .Text = "Input File Format"},
                    PipingToolAVS,
                    PipingToolVS,
                    Demuxer,
                    New OptionParam With {.Switch = "--input-depth", .Text = "Input Depth", .Options = {"Automatic", "8", "10", "12", "14", "16"}},
                    New OptionParam With {.Switch = "--input-csp", .Text = "Input Csp", .Options = {"Automatic", "I420", "YV12", "NV12", "NV21", "I422", "YV16", "NV16", "YUYV", "UYVY", "I444", "YV24", "BGR", "BGRA", "RGB"}},
                    New OptionParam With {.Switch = "--input-range", .Text = "Input Range", .Options = {"Automatic", "TV", "PC"}},
                    New OptionParam With {.Switch = "--output-csp", .Text = "Output Csp", .Options = {"Automatic", "I420", "I422", "I444", "RGB"}},
                    Muxer,
                    New OptionParam With {.Switch = "--fps", .Text = "Frame Rate", .Options = {"Automatic", "24000/1001", "24", "25", "30000/1001", "30", "50", "60000/1001", "60"}})
                Add("Input/Output 2",
                    New OptionParam With {.Switch = "--log-level", .Text = "Log Level", .Options = {"None", "Error", "Warning", "Info", "Debug"}},
                    New OptionParam With {.Switch = "--pulldown", .Text = "Pulldown", .Options = {"None", "22", "32", "64", "Double", "Triple", "Euro"}},
                    New OptionParam With {.Switch = "--avcintra-class", .Text = "AVC Intra Class", .Options = {"None", "50", "100", "200"}},
                    New OptionParam With {.Switch = "--avcintra-flavor", .Text = "AVC Intra Flavor", .Options = {"Panasonic", "Sony"}},
                    New NumParam With {.Switch = "--threads", .Text = "Threads"},
                    New NumParam With {.Switch = "--lookahead-threads", .Text = "Lookahead Threads"},
                    New NumParam With {.Switch = "--seek", .Text = "Seek"},
                    New NumParam With {.Switch = "--sync-lookahead", .Text = "Sync Lookahead"},
                    New NumParam With {.Switch = "--asm", .Text = "ASM"},
                    New NumParam With {.Switch = "--opencl-device", .Text = "OpenCl Device"},
                    New NumParam With {.Switch = "--sps-id", .Text = "SPS/PPS ID"})
                Add("Input/Output 3",
                    New BoolParam With {.Switch = "--fake-interlaced", .Text = "Fake Interlaced"},
                    New BoolParam With {.Switch = "--stitchable", .Text = "Stitchable"},
                    New BoolParam With {.Switch = "--psnr", .Text = "PSNR"},
                    New BoolParam With {.Switch = "--ssim", .Text = "SSIM"},
                    New BoolParam With {.Switch = "--sliced-threads", .Text = "Low-latency but lower-efficiency threading"},
                    New BoolParam With {.Switch = "--thread-input", .Text = "Run Avisynth in its own thread"},
                    New BoolParam With {.Switch = "--non-deterministic", .Text = "Non Deterministic"},
                    New BoolParam With {.Switch = "--cpu-independent", .Text = "Ensure reproducibility across different CPUs"},
                    New BoolParam With {.Switch = "--no-asm", .Text = "Disable all CPU optimizations"},
                    New BoolParam With {.Switch = "--opencl", .Text = "Enable use of OpenCL"},
                    ForceCFR,
                    New BoolParam With {.Switch = "--bluray-compat", .Text = "Enable compatibility hacks for Blu-ray support"},
                    New BoolParam With {.Switch = "--aud", .Text = "Use access unit delimiters"},
                    New BoolParam With {.Switch = "--quiet", .Text = "Quiet Mode"},
                    New BoolParam With {.Switch = "--verbose", .Switches = {"-v"}, .Text = "Print stats for each frame"},
                    New BoolParam With {.Switch = "--dts-compress", .Text = "Eliminate initial delay with container DTS hack"})
                Add("Other",
                    New StringParam With {.Switch = "--video-filter", .Switches = {"--vf"}, .Text = "Video Filter"},
                    New OptionParam With {.Switches = {"--tff", "--bff"}, .Text = "Interlaced", .Options = {"Progressive ", "Top Field First", "Bottom Field First"}, .Values = {"", "--tff", "--bff"}},
                    New OptionParam With {.Switch = "--frame-packing", .Text = "Frame Packing", .IntegerValue = True, .Options = {"Checkerboard", "Column Alternation", "Row Alternation", "Side By Side", "Top Bottom", "Frame Alternation", "Mono", "Tile Format"}},
                    Deblock,
                    DeblockA,
                    DeblockB,
                    CompCheck,
                    CompCheckAimedQuality,
                    SlowFirstpass,
                    New BoolParam With {.Switch = "--constrained-intra", .Text = "Constrained Intra Prediction"},
                    Cabac)
                Add("Custom",
                    Custom,
                    CustomFirstPass,
                    CustomSecondPass)

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
        If Control.ModifierKeys = Keys.Control OrElse Control.ModifierKeys = Keys.Shift Then
            g.ShellExecute("http://www.chaneru.com/Roku/HLS/X264_Settings.htm#" + id.TrimStart("-"c))
        Else
            g.ShowCommandLineHelp(Package.x264, id)
        End If
    End Sub

    Private BlockValueChanged As Boolean

    Protected Overrides Sub OnValueChanged(item As CommandLineParam)
        If BlockValueChanged Then
            Exit Sub
        End If

        If item Is Preset OrElse item Is Tune OrElse item Is Profile Then
            BlockValueChanged = True
            ApplyValues(False)
            BlockValueChanged = False
        End If

        If Not DeblockA.NumEdit Is Nothing Then
            DeblockA.NumEdit.Enabled = Deblock.Value
            DeblockB.NumEdit.Enabled = Deblock.Value

            PsyRD.NumEdit.Enabled = Psy.Value
            PsyTrellis.NumEdit.Enabled = Psy.Value
        End If

        MyBase.OnValueChanged(item)
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

        ApplyValues(True)

        Dim args As String
        Dim pipeTool = If(p.Script.Engine = ScriptEngine.AviSynth, PipingToolAVS, PipingToolVS).ValueText

        If includePaths AndAlso includeExecutable Then
            Dim pipeCmd = ""

            If pipeTool = "automatic" Then
                If p.Script.Engine = ScriptEngine.AviSynth Then
                    pipeTool = "none"
                Else
                    pipeTool = "vspipe y4m"
                End If
            End If

            Select Case pipeTool
                Case "vspipe y4m"
                    pipeCmd = Package.vspipe.Path.Escape + " " + script.Path.Escape + " - --y4m | "
                Case "vspipe raw"
                    pipeCmd = Package.vspipe.Path.Escape + " " + script.Path.Escape + " - | "
                Case "avs2pipemod y4m"
                    Dim dll As String

                    If FrameServerHelp.IsAviSynthPortableUsed Then
                        dll = " -dll=" + Package.AviSynth.Path.Escape
                    End If

                    pipeCmd = Package.avs2pipemod.Path.Escape + dll + " -y4mp " + script.Path.Escape + " | "
                Case "avs2pipemod raw"
                    Dim dll As String

                    If FrameServerHelp.IsAviSynthPortableUsed Then
                        dll = " -dll=" + Package.AviSynth.Path.Escape
                    End If

                    pipeCmd = Package.avs2pipemod.Path.Escape + dll + " -rawvideo " + script.Path.Escape + " | "
                Case "ffmpeg y4m"
                    pipeCmd = Package.ffmpeg.Path.Escape + " -i " + script.Path.Escape + " -f yuv4mpegpipe -strict -1 -loglevel fatal -hide_banner - | "
                Case "ffmpeg raw"
                    pipeCmd = Package.ffmpeg.Path.Escape + " -i " + script.Path.Escape + " -f rawvideo -strict -1 -loglevel fatal -hide_banner - | "
            End Select

            args += pipeCmd + Package.x264.Path.Escape
        End If

        If Mode.Value = x264RateMode.TwoPass OrElse Mode.Value = x264RateMode.ThreePass Then
            args += " --pass " & pass

            If pass = 1 Then
                If CustomFirstPass.Value <> "" Then
                    args += " " + CustomFirstPass.Value
                End If
            Else
                If CustomSecondPass.Value <> "" Then
                    args += " " + CustomSecondPass.Value
                End If
            End If
        End If

        If Mode.Value = x264RateMode.Quantizer Then
            If Not IsCustom(pass, "--qp") Then
                args += " --qp " + CInt(Quant.Value).ToString
            End If
        ElseIf Mode.Value = x264RateMode.Quality Then
            If Not IsCustom(pass, "--crf") Then
                args += " --crf " + Quant.Value.ToInvariantString
            End If
        Else
            If Not IsCustom(pass, "--bitrate") Then
                If Bitrate.Value <> 0 Then
                    args += " --bitrate " & Bitrate.Value
                Else
                    args += " --bitrate " & p.VideoBitrate
                End If
            End If
        End If

        Dim q = From i In Items Where i.GetArgs <> "" AndAlso Not IsCustom(pass, i.Switch)

        If q.Count > 0 Then
            args += " " + q.Select(Function(item) item.GetArgs).Join(" ")
        End If

        If includePaths Then
            Dim input = If(pipeTool = "none", script.Path.ToShortFilePath.Escape, "-")
            Dim dmx = Demuxer.ValueText

            If dmx = "automatic" Then
                If pipeTool = "none" Then
                    dmx = ""
                ElseIf pipeTool.EndsWith(" y4m") Then
                    dmx = "y4m"
                ElseIf pipeTool.EndsWith(" raw") Then
                    dmx = "raw"
                End If
            End If

            If dmx <> "" Then
                Dim info = script.GetInfo

                args += $" --demuxer {dmx} --frames " & info.FrameCount

                If dmx = "raw" Then
                    args += $" --input-res {info.Width}x{info.Height}"

                    If Not args.Contains("--fps ") Then
                        args += $" --fps {info.FrameRateNum}/{info.FrameRateDen}"
                    End If
                End If
            End If

            If Mode.Value = x264RateMode.TwoPass OrElse Mode.Value = x264RateMode.ThreePass Then
                args += " --stats " + (p.TempDir + p.TargetFile.Base + ".stats").Escape
            End If

            If (Mode.Value = x264RateMode.ThreePass AndAlso (pass = 1 OrElse pass = 3)) OrElse
                Mode.Value = x264RateMode.TwoPass AndAlso pass = 1 Then

                args += " --output NUL " + input
            Else
                args += " --output " + targetPath.ToShortFilePath.Escape + " " + input
            End If
        End If

        Return Macro.Expand(args.Trim.FixBreak.Replace(BR, " "))
    End Function

    Function GetPartitionsArg() As String
        If I4x4.Value = I4x4.DefaultValue AndAlso I8x8.Value = I8x8.DefaultValue AndAlso
            P4x4.Value = P4x4.DefaultValue AndAlso P8x8.Value = P8x8.DefaultValue AndAlso
            B8x8.Value = B8x8.DefaultValue Then

            Return Nothing
        End If

        If I4x4.Value AndAlso I8x8.Value AndAlso P4x4.Value AndAlso P8x8.Value AndAlso B8x8.Value Then
            Return "--partitions all"
        ElseIf Not I4x4.Value AndAlso Not I8x8.Value AndAlso Not P4x4.Value AndAlso Not P8x8.Value AndAlso Not B8x8.Value Then
            Return "--partitions none"
        End If

        Dim partitions As String

        If I4x4.Value Then partitions += "i4x4,"
        If I8x8.Value Then partitions += "i8x8,"
        If P4x4.Value Then partitions += "p4x4,"
        If P8x8.Value Then partitions += "p8x8,"
        If B8x8.Value Then partitions += "b8x8"

        If partitions <> "" Then
            Return "--partitions " + partitions.TrimEnd(","c)
        End If
    End Function

    Function IsCustom(pass As Integer, switch As String) As Boolean
        If switch = "" Then
            Return False
        End If

        If Mode.Value = x264RateMode.TwoPass OrElse Mode.Value = x264RateMode.ThreePass Then
            If pass = 1 Then
                If CustomFirstPass.Value?.Contains(switch + " ") OrElse
                    CustomFirstPass.Value?.EndsWith(switch) Then
                    Return True
                End If
            Else
                If CustomSecondPass.Value?.Contains(switch + " ") OrElse
                    CustomSecondPass.Value?.EndsWith(switch) Then
                    Return True
                End If
            End If
        End If

        If Custom.Value?.Contains(switch + " ") OrElse Custom.Value?.EndsWith(switch) Then
            Return True
        End If
    End Function

    Public Overrides Function GetPackage() As Package
        Return Package.x264
    End Function
End Class

Public Enum x264RateMode
    Bitrate
    Quantizer
    Quality
    <DispName("Two Pass")> TwoPass
    <DispName("Three Pass")> ThreePass
End Enum
