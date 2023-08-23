Imports System.Text
Imports StaxRip.VideoEncoderCommandLine

<Serializable()>
Public Class VvencffappEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Sub New()
        Name = "vvencFFapp"
        Params.ApplyPresetDefaultValues()
        Params.ApplyPresetValues()
    End Sub

    <NonSerialized>
    Private ParamsValue As VvencffappParams

    Property Params As VvencffappParams
        Get
            If ParamsValue Is Nothing Then
                ParamsValue = New VvencffappParams
                ParamsValue.Init(ParamsStore)
            End If

            Return ParamsValue
        End Get
        Set(value As VvencffappParams)
            ParamsValue = value
        End Set
    End Property

    Overrides ReadOnly Property OutputExt As String
        Get
            Return "vvc"
        End Get
    End Property

    Overrides Sub Encode()
        Encode("Video encoding", GetArgs(1, 0, 0, Nothing, p.Script), s.ProcessPriority)

        If Params.Mode.Value = VvencffappRateMode.TwoPass Then
            Encode("Video encoding second pass", GetArgs(2, 0, 0, Nothing, p.Script), s.ProcessPriority)
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(passName As String, commandLine As String, priority As ProcessPriorityClass)
        If Not CanChunkEncode() Then
            p.Script.Synchronize()
        End If

        Using proc As New Proc
            proc.Package = Package.VVenCFFapp
            proc.Header = passName
            proc.Encoding = Encoding.UTF8
            proc.Priority = priority
            proc.FrameCount = p.Script.GetFrameCount
            proc.SkipString = "TId: "

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

            If Params.Mode.Value = VvencffappRateMode.TwoPass Then
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

        Dim newParams As New VvencffappParams
        Dim newStore = ObjectHelp.GetCopy(ParamsStore)
        newParams.Init(newStore)

        Dim enc As New VvencffappEnc
        enc.Params = newParams
        enc.Params.Mode.Value = VvencffappRateMode.SingleQuant
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

    Overrides Sub ShowConfigDialog()
        Dim newParams As New VvencffappParams
        Dim store = ObjectHelp.GetCopy(ParamsStore)
        newParams.Init(store)
        newParams.ApplyPresetDefaultValues()

        Using form As New CommandLineForm(newParams)
            form.HTMLHelpFunc = Function() $"<p><a href=""{Package.VVenCFFapp.HelpURL}"">vvencFFapp Online Help</a></p>" +
               $"<h2>vvencFFapp Console Help</h2><pre>{HelpDocument.ConvertChars(Package.VVenCFFapp.CreateHelpfile())}</pre>"

            Dim a = Sub()
                        Dim enc = ObjectHelp.GetCopy(Me)
                        Dim params2 As New VvencffappParams
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
            Return Params.Mode.Value = VvencffappRateMode.SingleQuant
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
        Muxer = New MP4Muxer()
    End Sub

    Overrides Function CreateEditControl() As Control
        Return New VvencffappControl(Me) With {.Dock = DockStyle.Fill}
    End Function

    Shared Function Test() As String
        Dim tester As New ConsolAppTester

        tester.IgnoredSwitches = ""

        tester.UndocumentedSwitches = ""

        tester.Package = Package.VVenCFFapp
        tester.CodeFile = Path.Combine(Folder.Startup.Parent, "Encoding", "VvencffappEnc.vb")

        Return tester.Test
    End Function
End Class

Public Class VvencffappParams
    Inherits CommandLineParams

    Sub New()
        Title = "vvencFFapp Options"
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

    Property Mode As New OptionParam With {
        .Switches = {"--TargetBitrate", "--QP", "--Pass", "--NumPasses", "--RCStatsFile"},
        .Name = "Mode",
        .Text = "Mode",
        .Options = {"Bitrate", "Quantizer", "Two Pass"},
        .Value = 1}

    Property Quant As New NumParam With {
        .Switches = {"--QP", "-q"},
        .Name = "Quant",
        .Text = "Quantizer",
        .DefaultValue = 32,
        .Value = 32,
        .VisibleFunc = Function() Mode.Value = VvencffappRateMode.SingleQuant,
        .Config = {0, 63, 1}}

    Property TargetBitrate As New NumParam With {
        .HelpSwitch = "--TargetBitrate",
        .Text = "Target Bitrate",
        .Init = 5000,
        .VisibleFunc = Function() Mode.Value <> VvencffappRateMode.SingleQuant,
        .Config = {0, 1000000, 100}}

    Property Preset As New OptionParam With {
        .Switch = "--preset",
        .Text = "Preset",
        .Options = {"Faster", "Fast", "Medium", "Slow", "Slower"},
        .Init = 2}

    Property FrameSkip As New NumParam With {
        .HelpSwitch = "--FrameSkip",
        .Text = "Frames To Be Skipped"}

    Property FramesToBeEncoded As New NumParam With {
        .HelpSwitch = "--FramesToBeEncoded",
        .Text = "Frames To Be Encoded"}




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
        .VisibleFunc = Function() Mode.Value = VvencffappRateMode.TwoPass,
        .InitAction = Sub(tb)
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = g.GetCodeFont
                      End Sub}

    Property CustomSecondPass As New StringParam With {
        .Text = "Custom" + BR + "Second Pass",
        .Quotes = QuotesMode.Never,
        .VisibleFunc = Function() Mode.Value = VvencffappRateMode.TwoPass,
        .InitAction = Sub(tb)
                          tb.Edit.MultilineHeightFactor = 6
                          tb.Edit.TextBox.Font = g.GetCodeFont
                      End Sub}



    Overrides ReadOnly Property Items As List(Of CommandLineParam)
        Get
            If ItemsValue Is Nothing Then
                ItemsValue = New List(Of CommandLineParam)

                Add("Basic", Decoder, PipingToolAVS, PipingToolVS, Mode, TargetBitrate, Quant, Preset,
                    New OptionParam() With {.Switch = "--Profile", .Name = "Profile", .Text = "Profile", .Options = {"Automatic", "main_10", "main_10_still_picture"}},
                    New OptionParam() With {.Switch = "--Level", .Name = "Level", .Text = "Level", .Options = {"Automatic", "1.0", "2.0", "2.1", "3.0", "3.1", "4.0", "4.1", "5.0", "5.1", "5.2", "6.0", "6.1", "6.2", "6.3", "15.5"}},
                    New OptionParam() With {.Switch = "--Tier", .Name = "Tier", .Text = "Tier", .Options = {"Main", "High"}},
                    New OptionParam() With {.Switch = "--Verbosity", .Name = "Verbosity", .Text = "Verbosity", .Options = {"0: Silent", "1: Error", "2: Warning", "3: Info", "4: Notice", "5: Verbose (Default)", "6: Debug"}, .Init = 5, .IntegerValue = True},
                    New OptionParam() With {.Switch = "--SIMD", .Name = "SIMD", .Text = "SIMD", .Options = {"Automatic", "SCALAR", "SSE41", "SSE42", "AVX", "AVX2", "AVX512"}},
                    CompCheck, CompCheckAimedQuality,
                    Chunks
                )
                Add("I/O",
                    New OptionParam() With {.Switch = "--OutputBitDepth", .Name = "OutputBitDepth", .Text = "Output Bit-Depth", .Options = {"Automatic", "8", "10", "12"}},
                    FrameSkip, FramesToBeEncoded
                )
                Add("Threading",
                    New NumParam() With {.Switch = "--Threads", .Name = "Threads", .Text = "Threads", .Config = {0, 128, 1, 0}},
                    New NumParam() With {.Switch = "--MaxParallelFrames", .Name = "MaxParallelFrames", .Text = "Max Parallel Frames", .Config = {-1, 128, 1, 0}, .Init = -1}
                )
                'Add("Slice Decision", 

                ')
                Add("Rate Control",
                    New NumParam() With {.Switch = "--LookAhead", .Name = "LookAhead", .Text = "LookAhead", .Config = {-1, 1, 1, 0}, .Init = -1}
                )
                'Add("Quantization", 

                ')
                Add("Custom", Custom, CustomFirstPass, CustomSecondPass)

                'ItemsValue = ItemsValue.OrderBy(Function(i) i.Weight).ToList
            End If

            Return ItemsValue
        End Get
    End Property

    Overrides Function GetCommandLinePreview() As String
        Dim ret = GetCommandLine(True, True, 1)

        If Mode.Value = VvencffappRateMode.TwoPass Then
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
                        g.ShellExecute("https://github.com/fraunhoferhhi/vvenc/wiki/Usage#how-to-use-the-full-featured-expert-mode-encoder")
                    End If
                Case "Console Help"
                    ShowConsoleHelp(Package.VVenCFFapp, options)
            End Select
        End Using
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

                            sb.Append(pipeString & " | " & Package.VVenCFFapp.Path.Escape)
                        Case "vspipe"
                            Dim chunk = If(isSingleChunk, "", $" --start {startFrame} --end {endFrame}")
                            pipeString = Package.vspipe.Path.Escape + " " + script.Path.Escape + " - -c y4m" + chunk

                            sb.Append(pipeString & " | " & Package.VVenCFFapp.Path.Escape)
                        Case "ffmpeg"
                            pipeString = Package.ffmpeg.Path.Escape + If(p.Script.IsVapourSynth, " -f vapoursynth", "") + " -i " + script.Path.Escape + " -f yuv4mpegpipe -strict -1 -loglevel fatal -hide_banner -"

                            sb.Append(pipeString & " | " & Package.VVenCFFapp.Path.Escape)
                    End Select
                Case "qs"
                    Dim crop = If(isCropped, " --crop " & p.CropLeft & "," & p.CropTop & "," & p.CropRight & "," & p.CropBottom, "")
                    pipeString = Package.QSVEncC.Path.Escape + " -o - -c raw" + crop + " -i " + p.SourceFile.Escape

                    sb.Append(pipeString & " | " & Package.VVenCFFapp.Path.Escape)
                Case "ffqsv"
                    Dim crop = If(isCropped, $" -vf ""crop={p.SourceWidth - p.CropLeft - p.CropRight}:{p.SourceHeight - p.CropTop - p.CropBottom}:{p.CropLeft}:{p.CropTop}""", "")
                    pipeString = Package.ffmpeg.Path.Escape + " -threads 1 -hwaccel qsv -i " + p.SourceFile.Escape + " -f yuv4mpegpipe -strict -1" + crop + " -loglevel fatal -hide_banner -"

                    sb.Append(pipeString & " | " & Package.VVenCFFapp.Path.Escape)
                Case "ffdxva"
                    Dim crop = If(isCropped, $" -vf ""crop={p.SourceWidth - p.CropLeft - p.CropRight}:{p.SourceHeight - p.CropTop - p.CropBottom}:{p.CropLeft}:{p.CropTop}""", "")
                    Dim pix_fmt = If(p.SourceVideoBitDepth = 10, "yuv420p10le", "yuv420p")
                    pipeString = Package.ffmpeg.Path.Escape + " -threads 1 -hwaccel dxva2 -i " + p.SourceFile.Escape + " -f yuv4mpegpipe -pix_fmt " + pix_fmt + " -strict -1" + crop + " -loglevel fatal -hide_banner -"

                    sb.Append(pipeString & " | " & Package.VVenCFFapp.Path.Escape)
            End Select
        End If

        If includePaths Then
            Dim input = If(Decoder.Value = 0 AndAlso pipeTool = "automatic", script.Path.Escape, " --InputFile -")

            If (Mode.Value = VvencffappRateMode.TwoPass AndAlso pass = 1) Then
                sb.Append(input)
                sb.Append(" --BitstreamFile NUL")
            Else
                sb.Append(input)
                sb.Append(" --BitstreamFile " + (targetPath.DirAndBase + chunkName + targetPath.ExtFull).Escape)
            End If

            If Decoder.Value <> 0 OrElse pipeTool <> "automatic" Then
                sb.Append(" --y4m")
            End If

            If Mode.Value = VvencffappRateMode.TwoPass Then
                sb.Append(" --RCStatsFile " + (targetPath.DirAndBase + chunkName + ".stats").Escape)
            End If
        End If

        If isSingleChunk Then
            If FrameSkip.Value > 0 AndAlso Not IsCustom(pass, "--FrameSkip") Then
                sb.Append($" --FrameSkip {FrameSkip.Value}")
            End If

            If FramesToBeEncoded.Value > 0 AndAlso Not IsCustom(pass, "--FramesToBeEncoded") Then
                sb.Append($" --FramesToBeEncoded {Math.Min(script.GetFrameCount - FrameSkip.Value, FramesToBeEncoded.Value)}")
            End If
            'Else
            '    If includePaths Then
            '        sb.Append($" --FrameSkip {startFrame} --FramesToBeEncoded {endFrame - startFrame + 1}")
            '    End If
        End If

        If includePaths Then
            sb.Append($" --Size {p.TargetWidth}x{p.TargetHeight}")
        End If

        If Mode.Value = VvencffappRateMode.TwoPass Then
            sb.Append(" --NumPasses 2")
            sb.Append(" --Pass " & pass)

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

        If Mode.Value = VvencffappRateMode.SingleQuant Then
            If Not IsCustom(pass, "--QP") AndAlso Quant.Value <> Quant.DefaultValue Then
                sb.Append(" --QP " + CInt(Quant.Value).ToString)
            End If
        Else
            If Not IsCustom(pass, "--TargetBitrate") Then
                sb.Append(" --TargetBitrate " & If(pass = 1, TargetBitrate.Value, p.VideoBitrate) & "k")
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

        If Mode.Value = VvencffappRateMode.TwoPass Then
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
        Return Package.VVenCFFapp
    End Function
End Class

Public Enum VvencffappRateMode
    SingleBitrate
    SingleQuant
    TwoPass
End Enum
