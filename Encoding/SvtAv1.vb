Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class SvtAv1
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "More | SVT-AV1"
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
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        encParams.Init(store)

        Using form As New CommandLineForm(encParams)
            Dim saveProfileAction = Sub()
                                        Dim enc = ObjectHelp.GetCopy(Of SvtAv1)(Me)
                                        Dim encParamsCopy As New EncoderParams
                                        Dim storeCopy = DirectCast(ObjectHelp.GetCopy(store), PrimitiveStore)
                                        encParamsCopy.Init(storeCopy)
                                        enc.Params = encParamsCopy
                                        enc.ParamsStore = storeCopy
                                        SaveProfile(enc)
                                    End Sub

            ActionMenuItem.Add(form.cms.Items, "Save Profile...", saveProfileAction, Symbol.Save)

            If form.ShowDialog() = DialogResult.OK Then
                Params = encParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides ReadOnly Property OutputExt() As String
        Get
            'Return Params.Codec.ValueText
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()

        Using proc As New Proc
            proc.Header = "Video encoding"
            proc.Package = Package.SvtAv1
            'proc.SkipStrings = {"%]", " frames: "}
            proc.File = "cmd.exe"
            proc.Arguments = "/S /C """ + Params.GetCommandLine(True, True) + """"
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
            'Return Params.Mode.Value = 0
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
            Title = "SvtAv1 Options"
        End Sub

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)
                    Add("Basic")
                    Add("Advanced")
                    Add("Rate Control")

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

        Public Overrides Sub ShowHelp(switch As String)
            g.ShowCommandLineHelp(Package.SvtAv1, switch)
        End Sub

        Overrides Function GetCommandLine(includePaths As Boolean,
                                          includeExecutable As Boolean,
                                          Optional pass As Integer = 1) As String
            Dim ret As String
            Dim sourcePath As String
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)
            If includePaths AndAlso includeExecutable Then ret = Package.SvtAv1.Path.Escape

            'Select Case Decoder.ValueText
            '    Case "avs"
            '        sourcePath = p.Script.Path
            '    Case "qs"
            '        sourcePath = "-"
            '        If includePaths Then ret = If(includePaths, Package.QSVEnc.Path.Escape, "QSVEncC64") + " -o - -c raw" + " -i " + If(includePaths, p.SourceFile.Escape, "path") + " | " + If(includePaths, Package.SvtAv1.Path.Escape, "SvtAv1C64")
            '    Case "ffdxva"
            '        sourcePath = "-"
            '        If includePaths Then ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.SourceFile.Escape, "path") + " -f yuv4mpegpipe -strict -1 -pix_fmt yuv420p -loglevel fatal - | " + If(includePaths, Package.SvtAv1.Path.Escape, "SvtAv1C64")
            '    Case "ffqsv"
            '        sourcePath = "-"
            '        If includePaths Then ret = If(includePaths, Package.ffmpeg.Path.Escape, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.SourceFile.Escape, "path") + " -f yuv4mpegpipe -strict -1 -pix_fmt yuv420p -loglevel fatal - | " + If(includePaths, Package.SvtAv1.Path.Escape, "SvtAv1C64")
            '    Case "vce"
            '        sourcePath = p.LastOriginalSourceFile
            '        ret += " --avhw"
            'End Select

            Dim q = From i In Items Where i.GetArgs <> ""
            If q.Count > 0 Then ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")

            'Select Case Mode.Value
            '    Case 0
            '        ret += " --cqp " & QPI.Value & ":" & QPP.Value & ":" & QPB.Value
            '    Case 1
            '        ret += " --cbr " & p.VideoBitrate
            '    Case 2
            '        ret += " --vbr " & p.VideoBitrate
            'End Select

            'If CInt(p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 AndAlso
            '    (p.Script.IsFilterActive("Crop", "Hardware Encoder") OrElse
            '    (Decoder.ValueText <> "avs" AndAlso p.Script.IsFilterActive("Crop"))) Then

            '    ret += " --crop " & p.CropLeft & "," & p.CropTop & "," & p.CropRight & "," & p.CropBottom
            'End If

            'If p.Script.IsFilterActive("Resize", "Hardware Encoder") OrElse
            '    (Decoder.ValueText <> "avs" AndAlso p.Script.IsFilterActive("Resize")) Then

            '    ret += " --output-res " & p.TargetWidth & "x" & p.TargetHeight
            'End If

            If sourcePath = "-" Then ret += " --y4m"
            If includePaths Then ret += " -i " + sourcePath.Escape + " -o " + targetPath.Escape

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.SvtAv1
        End Function
    End Class
End Class