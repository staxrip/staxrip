
Imports System.Text
Imports StaxRip.VideoEncoderCommandLine

<Serializable()>
Public Class SVTAV1
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "AV1 | SVT-AV1"
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
        Dim encParams As New EncoderParams
        Dim store = ObjectHelp.GetCopy(ParamsStore)
        encParams.Init(store)

        Using form As New CommandLineForm(encParams)
            Dim a = Sub()
                        Dim enc = ObjectHelp.GetCopy(Me)
                        Dim encParamsCopy As New EncoderParams
                        Dim storeCopy = ObjectHelp.GetCopy(store)
                        encParamsCopy.Init(storeCopy)
                        enc.Params = encParamsCopy
                        enc.ParamsStore = storeCopy
                        SaveProfile(enc)
                    End Sub

            form.cms.Add("Save Profile...", a, Keys.Control Or Keys.S, Symbol.Save)

            If form.ShowDialog() = DialogResult.OK Then
                Params = encParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides ReadOnly Property OutputExt() As String
        Get
            Return "ivf"
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()

        Encode("Video encoding", Params.GetCommandLine(True, True, 1), s.ProcessPriority)

        If Params.Passes.Value = 1 Then
            Encode("Video encoding second pass", Params.GetCommandLine(True, True, 2), s.ProcessPriority)
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(passName As String, commandLine As String, priority As ProcessPriorityClass)
        p.Script.Synchronize()

        Using proc As New Proc
            proc.Header = passName
            proc.Package = Package.SVTAV1
            proc.Encoding = Encoding.UTF8
            proc.Priority = priority
            proc.IntegerFrameOutput = True
            proc.FrameCount = p.Script.GetFrameCount
            proc.File = "cmd.exe"
            proc.Arguments = "/S /C """ + commandLine + """"
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

    Public Class EncoderParams
        Inherits CommandLineParams

        Sub New()
            Title = "SVT-AV1 Options"
        End Sub

        Property Custom As New StringParam With {
            .Text = "Custom",
            .Quotes = QuotesMode.Never,
            .AlwaysOn = True}

        Property Mode As New OptionParam With {
            .Switch = "--rc",
            .Switches = {"--tbr"},
            .Text = "Mode",
            .IntegerValue = True,
            .Options = {"0: CQP", "1: VBR", "2: CVBR"}}

        Property Passes As New OptionParam With {
            .HelpSwitch = "--passes",
            .Text = "Passes",
            .Init = 0,
            .Options = {"1 Pass", "2 Passes"},
            .Values = {"1", "2"}}

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)

                    Add("Basic",
                        Custom,
                        Mode,
                        New OptionParam With {.Switch = "--preset", .Text = "Preset", .Init = 8, .IntegerValue = True, .Options = {"0: Very Slow", "1: Slower", "2: Slow", "3: Medium", "4: Fast", "5: Faster", "6: Very Fast", "7: Super Fast", "8: Ultra Fast"}},
                        New OptionParam With {.Switch = "--profile", .Text = "Profile", .IntegerValue = True, .Options = {"0: Main", "1: High", "2: Professional"}},
                        Passes,
                        New OptionParam With {.Switch = "--scm", .Text = "Screen Content Mode", .IntegerValue = True, .Options = {"0: OFF", "1: ON", "2: Content Based Detection"}},
                        New OptionParam With {.Switch = "--irefresh-type", .Text = "Intra Refresh Type", .Options = {"1: CRA (Open GOP)", "2: IDR (Closed GOP)"}, .Values = {"1", "2"}},
                        New NumParam With {.Switch = "--keyint", .Text = "Intra Period", .Init = -1, .Config = {-2, 255, 1}},
                        New NumParam With {.Switch = "--qp", .Text = "QP", .Init = 50, .Config = {0, 63, 1}})
                End If

                Return ItemsValue
            End Get
        End Property

        Public Overrides Sub ShowHelp(options As String())
            ShowConsoleHelp(Package.SVTAV1, options)
        End Sub

        Overrides Function GetCommandLine(
            includePaths As Boolean,
            includeExecutable As Boolean,
            Optional pass As Integer = 1) As String

            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt).Escape
            Dim statsPath = p.VideoEncoder.OutputPath.ChangeExt(".stat").Escape
            Dim ret = ""

            If includePaths AndAlso includeExecutable Then
                ret = Package.ffmpeg.Path.Escape +
                    If(p.Script.Engine = ScriptEngine.VapourSynth, " -f vapoursynth", "") +
                    " -i " + p.Script.Path.Escape +
                    " -f yuv4mpegpipe -strict -1 -loglevel fatal -hide_banner - | " +
                    Package.SVTAV1.Path.Escape
            End If

            If Passes.Value > 0 Then
                ret += " --pass " & pass
            End If

            Dim q = From i In Items Where i.GetArgs <> "" AndAlso Not IsCustom(pass, i.Switch)

            If q.Count > 0 Then
                ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")
            End If

            If Mode.Value <> 0 Then
                ret += " --tbr " & p.VideoBitrate
            End If

            If ret.Contains("%") Then
                ret = Macro.Expand(ret)
            End If

            If includePaths Then
                ret += " -n " & p.Script.GetFrameCount & " -i stdin"
                ret += If(Passes.Value = 1, " --stats " + statsPath, "")
                ret += If(Passes.Value = 0 OrElse (Passes.Value = 1 AndAlso pass = 2), " -b " + targetPath, "")
            End If

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.SVTAV1
        End Function

        Function IsCustom(pass As Integer, switch As String) As Boolean
            If switch = "" Then
                Return False
            End If

            If Custom.Value?.Contains(switch + " ") OrElse Custom.Value?.EndsWith(switch) Then
                Return True
            End If
        End Function

    End Class
End Class
