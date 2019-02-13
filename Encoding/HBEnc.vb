Imports System.Text
Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class HBEnc
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore


    Sub New()
        Name = "HB"
    End Sub

    <NonSerialized>
    Private ParamsValue As HBParams

    Property Params As HBParams
        Get
            If ParamsValue Is Nothing Then
                ParamsValue = New HBParams
                ParamsValue.Init(ParamsStore)
            End If

            Return ParamsValue
        End Get
        Set(value As HBParams)
            ParamsValue = value
        End Set
    End Property

    Overrides ReadOnly Property OutputExt As String
        Get
            Return "mkv"
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Encode("Video encoding", GetArgs(1, p.Script), s.ProcessPriority)


        AfterEncoding()
    End Sub

    Overloads Sub Encode(passName As String, commandLine As String, priority As ProcessPriorityClass)
        p.Script.Synchronize()

        Using proc As New Proc
            proc.Package = Package.HB
            proc.Header = "Video encoding"
            proc.Encoding = Encoding.UTF8
            proc.WorkingDirectory = p.TempDir
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
        Dim r As New MenuList
        r.Add("Encoder Options", AddressOf ShowConfigDialog)
        r.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return r
    End Function

    Overrides Sub ShowConfigDialog()

        Dim newParams As New HBParams
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(store)

        Using form As New CommandLineForm(newParams)
            Dim saveProfileAction = Sub()
                                        Dim enc = ObjectHelp.GetCopy(Of HBEnc)(Me)
                                        Dim params2 As New HBParams
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            ActionMenuItem.Add(form.cms.Items, "Save Profile...", saveProfileAction).SetImage(Symbol.Save)

            If form.ShowDialog() = DialogResult.OK Then
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Public Overrides ReadOnly Property CommandLineParams As CommandLineParams
        Get
            Return Params
        End Get
    End Property
End Class

Public Class HBParams
    Inherits CommandLineParams

    Sub New()
        Title = "HB Options"
    End Sub

    Property Tune As New OptionParam With {
        .Text = "Tune",
        .Help = "Quality tuning (Will enforce partition sizes >= 8x8)",
        .Switch = "--tune",
        .Path = "Basic",
        .Options = {"PSNR", "Psychovisual"},
        .Values = {"psnr", "psychovisual"},
        .InitValue = 0}

    Property Limit As New OptionParam With {
        .Text = "format",
        .Switch = "--format",
        .Options = {"MP4", "MKV"},
        .Values = {"av_mp4", "av_mkv"},
        .Path = "Basic"}

    Property Encoder As New OptionParam With {
        .Text = "Encoder",
        .Switch = "--encoder",
        .Options = {"x264", "x264 10Bit", "QSV H.264", "NVEnc H264", "x265", "x265 10Bit", "x265 12Bit", "NVEnc H265", "MPEG-4", "MPEG-2", "VP8", "VP9", "Theora"},
        .Values = {"x264", "x264_10bit", "qsv_h264", "nvenc_h264", "x265", "x265_10bit", "x265_12bit", "nvenc_h265", "mpeg4", "mpeg2", "VP8", "VP9", "theora"},
        .Path = "Basic"}

    Property Rate As New OptionParam With {
        .Text = "Framerate",
        .Switch = "--rate",
        .Options = {"5", "10", "12", "15", "20", "23.976", "24", "25", "29.970", "30", "48", "50", "59.94", "60", "72", "75", "90", "100", "120"},
        .Values = {"5", "10", "12", "15", "20", "23.976", "24", "25", "29.970", "30", "48", "50", "59.94", "60", "72", "75", "90", "100", "120"},
        .Path = "Basic"}

    Property Quality As New NumParam With {
        .Text = "Quality",
        .Switch = "--quality",
        .Init = 22.0,
        .Path = "Basic"}

    Property Bitrate As New NumParam With {
        .Text = "Bitrate",
        .Switch = "--vb",
        .Init = 1000,
        .Path = "Basic"}

    Property TwoPass As New BoolParam With {
        .Text = "Two Pass",
        .Switch = "--two-pass",
        .NoSwitch = "--no-two-pass",
        .Init = False,
        .Path = "Basic"}

    Property Level As New BoolParam With {
        .Text = "Two Pass",
        .Switch = "--two-pass",
        .NoSwitch = "--no-two-pass",
        .Init = False,
        .Path = "Basic"}

    'New OptionParam With {.Switch = "--level-idc", .Switches = {"--level"}, .Text = "Level", .Options = {"Unrestricted", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2", "8.5"}},

    Property Turbo As New BoolParam With {
        .Text = "Turbo",
        .Switch = "--turbo",
        .NoSwitch = "--no-turbo",
        .Init = False,
        .Path = "Basic"}

    Property Mode As New OptionParam With {
        .Text = "Framerate",
        .Options = {"VFR", "CRF", "PRF"},
        .Switches = {"--vfr", "--cfr", "--pfr"},
        .Path = "Basic"}

    Property QuickSync As New BoolParam With {
        .Text = "Enable QuickSync",
        .Switch = "--enable-qsv-decoding",
        .NoSwitch = "--disable-qsv-decoding",
        .Init = False,
        .Path = "Basic"}

    Property QuickSyncDepth As New NumParam With {
        .Text = "QuickSync Async Depth",
        .Switch = "--qsv-async-depth",
        .Init = 4,
        .Path = "Basic"}



    Overrides ReadOnly Property Items As List(Of CommandLineParam)
        Get
            If ItemsValue Is Nothing Then
                ItemsValue = New List(Of CommandLineParam)

                Add(New OptionParam With {.Switch = "--level-idc", .Switches = {"--level"}, .Text = "Level", .Name = "x265Level", .Options = {"Unrestricted", "1.0", "2.0", "2.1", "3.0", "3.1", "4.0", "4.1", "5.0", "5.1", "5.2", "6.0", "6.1", "6.2"}},
                    New OptionParam With {.Switch = "--level-idc", .Switches = {"--level"}, .Text = "Level", .Name = "x264Level", .Options = {"Unrestricted", "1.0", "2.0", "2.1", "3.0", "3.1", "4.0", "4.1", "5.0", "5.1", "5.2"}})

                For Each item In ItemsValue
                    If item.HelpSwitch <> "" Then Continue For
                    Dim switches = item.GetSwitches
                    If switches.NothingOrEmpty Then Continue For
                    item.HelpSwitch = switches(0)
                Next
            End If

            Return ItemsValue
        End Get
    End Property

    Public Overrides Sub ShowHelp(id As String)
        g.ShowCommandLineHelp(Package.HB, id)
    End Sub

    Shadows Sub Add(ParamArray items As CommandLineParam())
        For Each i In items
            If i.HelpSwitch = "" Then
                Dim switches = i.GetSwitches
                If Not switches.NothingOrEmpty Then i.HelpSwitch = switches(0)
            End If

            ItemsValue.Add(i)
        Next
    End Sub
    Overloads Overrides Function GetCommandLine(includePaths As Boolean,
                                                includeExecutable As Boolean,
                                                Optional pass As Integer = 1) As String

        Return GetArgs(1, p.Script, p.VideoEncoder.OutputPath.DirAndBase +
                       p.VideoEncoder.OutputExtFull, includePaths, includeExecutable)
    End Function

    Overloads Function GetArgs(pass As Integer,
                               script As VideoScript,
                               targetPath As String,
                               includePaths As Boolean,
                               includeExecutable As Boolean) As String


        Dim sb As New StringBuilder

        If includePaths AndAlso includeExecutable Then
            If p.Script.Engine = ScriptEngine.VapourSynth Then
                'sb.Append(Package.vspipe.Path.Escape + " -y -loglevel fatal -hide_banner -i " + script.Path.Escape + " -f yuv4mpegpipe - | " + Package.Rav1e.Path.Escape)
            Else
                ' sb.Append(Package.ffmpeg.Path.Escape + " -y -loglevel fatal -hide_banner -i " + script.Path.Escape + " -f yuv4mpegpipe - | " + Package.Rav1e.Path.Escape)
            End If
        End If

        Dim q = From i In Items Where i.GetArgs <> ""
        If q.Count > 0 Then sb.Append(" " + q.Select(Function(item) item.GetArgs).Join(" "))
        sb.Append(" -o " + targetPath.Escape + " - ")

        Return Macro.Expand(sb.ToString.Trim.FixBreak.Replace(BR, " "))
    End Function
    Public Overrides Function GetPackage() As Package
        Return Package.Rav1e
    End Function
End Class