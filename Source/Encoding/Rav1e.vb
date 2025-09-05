﻿
Imports System.Text
Imports StaxRip.UI

Imports StaxRip.VideoEncoderCommandLine

<Serializable()>
Public Class Rav1e
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Sub New()
        Name = "Rav1e"
    End Sub

    Overloads Shared ReadOnly Property Package As Package
        Get
            Return Package.Rav1e
        End Get
    End Property

    <NonSerialized>
    Private ParamsValue As Rav1eParams

    Property Params As Rav1eParams
        Get
            If ParamsValue Is Nothing Then
                ParamsValue = New Rav1eParams
                ParamsValue.Init(ParamsStore)
            End If

            Return ParamsValue
        End Get
        Set(value As Rav1eParams)
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

    Overrides ReadOnly Property Codec As String
        Get
            Return "av1"
        End Get
    End Property

    Overrides ReadOnly Property OutputExt As String
        Get
            Return "ivf"
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Encode("Video encoding", GetArgs(1, p.Script), s.ProcessPriority)

        If Params.Passes.Visible AndAlso Params.Passes.Value = 1 Then
            Encode("Video encoding", GetArgs(2, p.Script), s.ProcessPriority)
        End If
    End Sub

    Overloads Sub Encode(passName As String, commandLine As String, priority As ProcessPriorityClass)
        p.Script.Synchronize()

        Using proc As New Proc
            proc.Package = Package.Rav1e
            proc.Header = "Video encoding"
            proc.FrameCount = p.Script.GetFrameCount
            proc.Encoding = Encoding.UTF8
            proc.Priority = priority
            proc.SkipString = "encoded "
            proc.File = "cmd.exe"
            proc.Arguments = "/S /C """ + commandLine + """"
            proc.Start()
        End Using
    End Sub

    Overloads Function GetArgs(pass As Integer, script As VideoScript, Optional includePaths As Boolean = True) As String
        Return Params.GetArgs(pass, script, OutputPath.DirAndBase + OutputExtFull, includePaths, True)
    End Function

    Overrides Function GetMenu() As MenuList
        Dim ret As New MenuList
        ret.Add("Encoder Options", AddressOf ShowConfigDialog)
        ret.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return ret
    End Function

    Overrides Sub ShowConfigDialog(Optional param As CommandLineParam = Nothing)
        Dim newParams As New Rav1eParams
        Dim store = ObjectHelp.GetCopy(ParamsStore)
        newParams.Init(store)

        Using form As New CommandLineForm(newParams)
            Dim a = Sub()
                        Dim enc = ObjectHelp.GetCopy(Me)
                        Dim params2 As New Rav1eParams
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
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides Property QualityMode() As Boolean
        Get
            Return Params.Mode.OptionText.EqualsAny("Quality")
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

        tester.IgnoredSwitches = "fullhelp output help psnr version verbose"
        tester.UndocumentedSwitches = "y4m help version verbose"
        tester.Package = Package.Rav1e
        tester.CodeFile = Path.Combine(Folder.Startup.Parent, "Encoding", "Rav1e.vb")

        Return tester.Test
    End Function
End Class

Public Class Rav1eParams
    Inherits CommandLineParams

    Sub New()
        Title = "Rav1e Options"
    End Sub

    Public Overrides ReadOnly Property Package As Package
        Get
            Return Rav1e.Package
        End Get
    End Property


    Property Tune As New OptionParam With {
        .Text = "Tune",
        .Help = "Quality tuning (Will enforce partition sizes >= 8x8)",
        .Switch = "--tune",
        .Path = "Basic",
        .Options = {"PSNR", "Psychovisual"},
        .Values = {"psnr", "psychovisual"}}

    Property Limit As New NumParam With {
        .Text = "Limit",
        .Help = "Maximum number of frames to encode",
        .Switch = "--limit",
        .Path = "Basic"}

    Property Mode As New OptionParam With {
        .Text = "Mode",
        .Path = "Basic",
        .AlwaysOn = True,
        .Options = {"Speed", "Bitrate"},
        .Values = {"--speed", "--bitrate"}}

    Property Bitrate As New NumParam With {
        .Text = "Bitrate",
        .Path = "Basic",
        .Config = {0, 9999},
        .ArgsFunc = Function() "" & Bitrate.Value,
        .VisibleFunc = Function() Mode.Value = 1}

    Property Passes As New OptionParam With {
        .Text = "Passes",
        .Path = "Basic",
        .Switches = {"--first-pass", "--second-pass"},
        .Options = {"One Pass", "Two Passes"},
        .VisibleFunc = Function() Mode.Value = 1}

    Property Range As New OptionParam With {
        .Text = "Range",
        .Path = "VUI",
        .Switch = "--range",
        .Options = {"Unspecified", "Limited", "Full"}}

    Property Prime As New OptionParam With {
        .Text = "Primaries",
        .Path = "VUI",
        .Switch = "--primaries",
        .Init = 1,
        .Options = {"BT709", "Unspecified", "BT470M", "BT470BG", "ST170M", "ST240M", "Film", "BT2020", "ST428", "P3DCI", "P3Display", "Tech3213"}}

    Property Matrix As New OptionParam With {
        .Text = "Matrix",
        .Path = "VUI",
        .Switch = "--matrix",
        .Init = 2,
        .Options = {"Identity", "BT709", "Unspecified", "BT470M", "BT470BG", "ST170M", "ST240M", "YCgCo", "BT2020NonConstantLuminance", "BT2020ConstantLuminance", "ST2085", "ChromaticityDerivedNonConstantLuminance", "ChromaticityDerivedConstantLuminance", "ICtCp"}}

    Property Transfer As New OptionParam With {
        .Text = "Transfer",
        .Path = "VUI",
        .Switch = "--transfer",
        .Init = 1,
        .Options = {"BT1886", "Unspecified", "BT470M", "BT470BG", "ST170M", "ST240M", "Linear", "Logarithmic100", "Logarithmic316", "XVYCC", "BT1361E", "SRGB", "BT2020Ten", "BT2020Twelve", "PerceptualQuantizer", "ST428", "HybridLogGamma"}}

    Property Speed As New NumParam With {
        .Text = "Speed",
        .HintText = "0 = Slowest, 10 = Fastest",
        .Switch = "--speed",
        .Config = {0, 10},
        .Init = 6,
        .VisibleFunc = Function() Mode.Value = 0,
        .ArgsFunc = Function() Speed.Value.ToString,
        .Path = "Basic"}

    Property Quantizer As New NumParam With {
        .Text = "Quantizer",
        .Switch = "--quantizer",
        .Config = {0, 255, 1},
        .Path = "Basic",
        .Init = 100}

    Property Keyint As New NumParam With {
        .Text = "Keyframe Interval",
        .Switch = "--keyint",
        .Path = "Basic",
        .Config = {0, 300},
        .Init = 240}

    Property MinKeyint As New NumParam With {
        .Text = "Min Keyframe",
        .Switch = "--min-keyint",
        .Path = "Basic",
        .Config = {0, 300},
        .Init = 12}

    Property Light As New NumParam With {
        .Text = "Content Light",
        .Switch = "--content_light",
        .Path = "VUI",
        .Config = {0, Integer.MaxValue, 50},
        .ArgsFunc = Function() If(Light.Value <> 0 OrElse MaxFALL.Value <> 0, "--content_light """ & Light.Value & "," & MaxFALL.Value & """", ""),
        .ImportAction = Sub(param, arg)
                            If arg = "" Then
                                Exit Sub
                            End If

                            Dim a = arg.Trim(""""c).Split(","c)
                            Light.Value = a(0).ToInt
                            MaxFALL.Value = a(1).ToInt
                        End Sub}

    Property MaxFALL As New NumParam With {
        .Switches = {"--content_light"},
        .Text = "Maximum FALL",
        .Path = "VUI",
        .Config = {0, Integer.MaxValue, 50}}

    Property Threads As New NumParam With {
        .Text = "Threads",
        .Switch = "--threads",
        .Path = "Basic",
        .Config = {0, 20}}

    Property Custom As New StringParam With {
        .Text = "Custom",
        .Path = "Misc",
        .Quotes = QuotesMode.Never,
        .AlwaysOn = True}

    Overrides ReadOnly Property Items As List(Of CommandLineParam)
        Get
            If ItemsValue Is Nothing Then
                ItemsValue = New List(Of CommandLineParam)

                Add("Main",
                    Tune, Mode, Passes, Speed, Bitrate, Quantizer,
                    Keyint, MinKeyint, Threads, Limit, 
                    New BoolParam With {.Switch = "--low-latency", .Text = "Low Latency", .Path = "Basic"}
                )
                Add("VUI",
                    New StringParam With {.Switch = "--mastering-display", .Path = "VUI", .Text = "Master Display"},
                    Light, MaxFALL, Prime, Matrix, Transfer, Range
                )
                Add("Custom",
                    Custom
                )
            End If

            Return ItemsValue
        End Get
    End Property

    Public Overrides Sub ShowHelp(options As String())
        ShowConsoleHelp(Package.Rav1e, options)
    End Sub

    Overloads Overrides Function GetCommandLine(
        includePaths As Boolean,
        includeExecutable As Boolean,
        Optional pass As Integer = 1) As String

        Return GetArgs(1, p.Script, p.VideoEncoder.OutputPath.DirAndBase +
                       p.VideoEncoder.OutputExtFull, includePaths, includeExecutable)
    End Function

    Overloads Function GetArgs(
        pass As Integer,
        script As VideoScript,
        targetPath As String,
        includePaths As Boolean,
        includeExecutable As Boolean) As String

        Dim sb As New StringBuilder

        If includePaths AndAlso includeExecutable Then
            sb.Append(Package.ffmpeg.Path.Escape + $" -loglevel fatal -hide_banner{If(script.Path.Ext = "vpy", " -f vapoursynth", "")} -i " + script.Path.Escape + " -f yuv4mpegpipe -strict -1 - | " + Package.Rav1e.Path.Escape)
        End If

        If includePaths AndAlso Passes.Visible AndAlso Passes.Value = 1 Then
            Dim statsFile = (targetPath.DirAndBase + ".stats").Escape

            If pass = 1 Then
                sb.Append(" --first-pass " & statsFile)
            Else
                sb.Append(" --second-pass " & statsFile)
            End If
        End If

        Dim q = From i In Items Where i.GetArgs <> ""

        If q.Count > 0 Then
            sb.Append(" " + q.Select(Function(item) item.GetArgs).Join(" "))
        End If

        sb.Append(" -y -o " + targetPath.Escape + " - ")

        Return Macro.Expand(sb.ToString.Trim.FixBreak.Replace(BR, " "))
    End Function
End Class

Public Enum Rav1eRateMode
    Speed
    OnePass
    TwoPass
End Enum