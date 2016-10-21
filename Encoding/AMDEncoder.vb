Imports System.Text
Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Class AMDEncoder
    Inherits BasicVideoEncoder

    Sub New()
        Name = "AMD H.264"
    End Sub

    Property ParamsStore As New PrimitiveStore

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
        Dim params1 As New EncoderParams
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        params1.Init(store)

        Using f As New CommandLineForm(params1)
            Dim saveProfileAction = Sub()
                                        Dim enc = ObjectHelp.GetCopy(Of AMDEncoder)(Me)
                                        Dim params2 As New EncoderParams
                                        Dim store2 = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        params2.Init(store2)
                                        enc.Params = params2
                                        enc.ParamsStore = store2
                                        SaveProfile(enc)
                                    End Sub

            f.cms.Items.Add(New ActionMenuItem("Save Profile...", saveProfileAction))
            f.cms.Items.Add(New ActionMenuItem("Check VCE Support", Sub() MsgInfo(ProcessHelp.GetStdOut(Package.NVEncC.Path, "--check-vce"))))

            If f.ShowDialog() = DialogResult.OK Then
                Params = params1
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides ReadOnly Property OutputExt() As String
        Get
            Return "h264"
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Dim batchPath = p.TempDir + p.TargetFile.Base + "_VCEEncC.bat"
        Dim batchCode = Proc.WriteBatchFile(batchPath, Params.GetCommandLine(True, False))

        Using proc As New Proc
            proc.Init("Encoding using VCEEncC " + Package.VCEEncC.Version)
            proc.SkipStrings = {"%]", " frames: "}
            proc.WriteLine(batchCode + BR2)
            proc.File = "cmd.exe"
            proc.Arguments = "/C call """ + batchPath + """"
            proc.Start()
        End Using

        AfterEncoding()
    End Sub

    Overrides Function GetMenu() As MenuList
        Dim r As New MenuList
        r.Add("Encoder Options", AddressOf ShowConfigDialog)
        r.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return r
    End Function

    Overrides Property QualityMode() As Boolean
        Get
            Return Params.Mode.OptionText = "CQP"
        End Get
        Set(Value As Boolean)
        End Set
    End Property

    Public Overrides ReadOnly Property CommandLineParams As CommandLineParams
        Get
            Return Params
        End Get
    End Property

    Class EncoderParams
        Inherits CommandLineParams

        Sub New()
            Title = "AMD Encoding Options"
        End Sub

        Property Mode As New OptionParam With {
            .Name = "Mode",
            .Text = "Mode:",
            .Options = {"CBR", "VBR", "CQP"}}

        Property Decoder As New OptionParam With {
            .Text = "Decoder:",
            .Options = {"AviSynth/VapourSynth", "QSVEncC (Intel)", "ffmpeg (Intel)", "ffmpeg (DXVA2)"},
            .Values = {"avs", "qs", "ffqsv", "ffdxva"}}

        Property QPI As New NumParam With {
            .Text = "Constant QP I:",
            .Value = 22,
            .VisibleFunc = Function() "CQP" = Mode.OptionText,
            .MinMaxStep = {0, 51, 1}}

        Property QPP As New NumParam With {
            .Text = "Constant QP P:",
            .Value = 24,
            .VisibleFunc = Function() "CQP" = Mode.OptionText,
            .MinMaxStep = {0, 51, 1}}

        Property QPB As New NumParam With {
            .Text = "Constant QP B:",
            .Value = 27,
            .VisibleFunc = Function() "CQP" = Mode.OptionText,
            .MinMaxStep = {0, 51, 1}}

        Property Custom As New StringParam With {
            .Text = "Custom Switches:",
            .ArgsFunc = Function() Custom.Value}

        Private ItemsValue As List(Of CommandLineParam)

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)
                    Add("Basic", Decoder, Mode,
                        New OptionParam With {.Switch = "--quality", .Text = "Quality:", .Options = {"fast", "balanced", "slow"}, .InitValue = 1},
                        New OptionParam With {.Switch = "--profile", .Text = "Profile:", .Options = {"Baseline", "Main", "High"}},
                        New OptionParam With {.Switch = "--level", .Text = "Level:", .Options = {"Automatic", "1", "1b", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}},
                        QPI, QPP, QPB)
                    Add("Slice Decision",
                        New NumParam With {.Switch = "--slices", .Text = "Slices:", .InitValue = 1},
                        New NumParam With {.Switch = "--bframes", .Text = "B-Frames:", .MinMaxStep = {0, 16, 1}},
                        New NumParam With {.Switch = "--gop-len", .Text = "GOP Length:", .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New BoolParam With {.Switch = "--b-pyramid", .Text = "B-Pyramid"})
                    Add("Rate Control",
                        New NumParam With {.Switch = "--max-bitrate", .Text = "Max Bitrate:", .InitValue = 20000, .MinMaxStep = {0, Integer.MaxValue, 1}},
                        New NumParam With {.Switch = "--vbv-bufsize", .Text = "VBV Bufsize:", .MinMaxStep = {0, 1000000, 1}, .InitValue = 20000},
                        New NumParam With {.Switch = "--qp-min", .Text = "QP Min:", .MinMaxStep = {0, 100, 1}},
                        New NumParam With {.Switch = "--qp-max", .Text = "QP Max:", .MinMaxStep = {0, 100, 1}, .InitValue = 100},
                        New NumParam With {.Switch = "--b-deltaqp", .Text = "Non-ref bframe QP offset:"},
                        New NumParam With {.Switch = "--bref-deltaqp", .Text = "Ref bframe QP offset:"})
                    Add("Other",
                        New StringParam With {.Switch = "--chapter", .Text = "Chapters:", .Quotes = True, .BrowseFileFilter = "*.*|*.*"},
                        New StringParam With {.Switch = "--log", .Text = "Log File:", .Quotes = True, .BrowseFileFilter = "*.*|*.*"},
                        New OptionParam With {.Switch = "--log-level", .Text = "Log Level:", .Options = {"info", "debug", "warn", "error"}},
                        New OptionParam With {.Switch = "--motion-est", .Text = "Motion Estimation:", .Options = {"q-pel", "full-pel", "half-pel"}},
                        New OptionParam With {.Switches = {"--tff", "--bff"}, .Text = "Interlaced:", .Options = {"Progressive ", "Top Field First", "Bottom Field First"}, .Values = {"", "--tff", "--bff"}},
                        New BoolParam With {.Switch = "--chapter-copy", .Text = "Copy Chapters"},
                        Custom)
                End If

                Return ItemsValue
            End Get
        End Property

        Private Sub Add(path As String, ParamArray items As CommandLineParam())
            For Each i In items
                i.Path = path
                ItemsValue.Add(i)
            Next
        End Sub

        Overrides Function GetCommandLine(includePaths As Boolean,
                                          includeExecutable As Boolean,
                                          Optional pass As Integer = 1) As String
            Dim ret As String
            Dim sourcePath As String
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)

            If includePaths AndAlso includeExecutable Then
                ret = Package.VCEEncC.Path.Quotes
            End If

            Select Case Decoder.ValueText
                Case "avs"
                    sourcePath = p.Script.Path
                Case "qs"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.QSVEncC.Path.Quotes, "QSVEncC") + " -o - -c raw" + " -i " + If(includePaths, p.SourceFile.Quotes, "path") + " | " + If(includePaths, Package.VCEEncC.Path.Quotes, "VCEEncC")
                Case "ffdxva"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.ffmpeg.Path.Quotes, "ffmpeg") + " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.SourceFile.Quotes, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | " + If(includePaths, Package.VCEEncC.Path.Quotes, "VCEEncC")
                Case "ffqsv"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Package.ffmpeg.Path.Quotes, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.SourceFile.Quotes, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | " + If(includePaths, Package.VCEEncC.Path.Quotes, "VCEEncC")
            End Select

            Dim q = From i In Items Where i.GetArgs <> ""

            If q.Count > 0 Then
                ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")
            End If

            Select Case Mode.OptionText
                Case "CBR"
                    ret += " --cbr " & p.VideoBitrate
                Case "VBR"
                    ret += " --vbr " & p.VideoBitrate
                Case "CQP"
                    ret += " --cqp " & QPI.Value & ":" & QPP.Value & ":" & QPB.Value
            End Select

            If CInt(p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 AndAlso
                (p.Script.IsFilterActive("Crop", "Hardware Encoder") OrElse
                (Decoder.ValueText <> "avs" AndAlso p.Script.IsFilterActive("Crop"))) Then

                ret += " --crop " & p.CropLeft & "," & p.CropTop & "," & p.CropRight & "," & p.CropBottom
            End If

            If p.Script.IsFilterActive("Resize", "Hardware Encoder") OrElse
                (Decoder.ValueText <> "avs" AndAlso p.Script.IsFilterActive("Resize")) Then

                ret += " --output-res " & p.TargetWidth & "x" & p.TargetHeight
            ElseIf p.AutoARSignaling AndAlso p.SourceFile <> "" Then
                Dim par = Calc.GetTargetPAR
                If par <> New Point(1, 1) Then ret += " --sar " & par.X & ":" & par.Y
            End If

            If sourcePath = "-" Then ret += " --y4m"

            If includePaths Then ret += " -i " + sourcePath.Quotes + " -o " + targetPath.Quotes

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.VCEEncC
        End Function
    End Class
End Class