
Imports StaxRip.CommandLine
Imports StaxRip.UI

<Serializable()>
Public Class SVTAV1
    Inherits BasicVideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "SVT-AV1"
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
                                        Dim enc = ObjectHelp.GetCopy(Of SVTAV1)(Me)
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
            Return "ivf"
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()

        Using proc As New Proc
            proc.Header = "Video encoding"
            proc.Package = Package.SVTAV1
            proc.RemoveChars = {Convert.ToChar(8)} 'back space
            proc.File = "cmd.exe"
            proc.Arguments = "/S /C """ + Params.GetCommandLine(True, True) + """"
            proc.Start()
        End Using

        AfterEncoding()
    End Sub

    Overrides Function GetMenu() As MenuList
        Dim ret As New MenuList
        ret.Add("Encoder Options", AddressOf ShowConfigDialog)
        ret.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return ret
    End Function

    Overrides Property QualityMode() As Boolean
        Get
            Return True
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

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)
                    Add("Basic",
                        New OptionParam With {.Switch = "--rc", .Text = "Mode", .Options = {"CQP", "VBR", "CVBR"}},
                        New OptionParam With {.Switch = "-enc-mode", .Text = "Preset", .Init = 8, .IntegerValue = True, .Options = {"Very Slow", "Slower", "Slow", "Medium", "Fast", "Faster", "Very Fast", "Super Fast", "Ultra Fast"}},
                        New NumParam With {.Switch = "-q", .Text = "QP", .Config = {0, 63, 1}, .Init = 50})

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

        Public Overrides Sub ShowHelp(switch As String)
            g.ShowCommandLineHelp(Package.SVTAV1, switch)
        End Sub

        Overrides Function GetCommandLine(
            includePaths As Boolean,
            includeExecutable As Boolean,
            Optional pass As Integer = 1) As String

            Dim ret = ""
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputExt)

            If includePaths AndAlso includeExecutable Then
                ret = Package.ffmpeg.Path.Escape +
                    If(p.Script.Engine = ScriptEngine.VapourSynth, " -f vapoursynth", "") +
                    " -i " + p.Script.Path.Escape +
                    " -f yuv4mpegpipe -strict -1 -loglevel fatal -hide_banner - | " +
                    Package.SVTAV1.Path.Escape
            End If

            Dim q = From i In Items Where i.GetArgs <> ""

            If q.Count > 0 Then
                ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")
            End If

            If includePaths Then
                ret += " -n " & p.Script.GetFrameCount & " -i stdin -b " + targetPath.Escape
            End If

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.SVTAV1
        End Function
    End Class
End Class