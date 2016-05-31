Imports System.Text
Imports StaxRip.UI
Imports StaxRip.CommandLine

<Serializable()>
Public MustInherit Class VideoEncoder
    Inherits Profile
    Implements IComparable(Of VideoEncoder)

    MustOverride Sub Encode()

    MustOverride ReadOnly Property OutputFileType As String

    Overridable Property Passes As Integer
    Overridable Property QualityMode As Boolean

    Property AutoCompCheckValue As Integer = 70
    Property Muxer As Muxer = New MkvMuxer

    Public MustOverride Sub ShowConfigDialog()

    Sub New()
        CanEditValue = True
    End Sub

    Private OutputPathValue As String

    Overridable ReadOnly Property OutputPath() As String
        Get
            If TypeOf Muxer Is EncoderMuxerBase OrElse TypeOf Muxer Is NullMuxer Then
                Return p.TargetFile
            Else
                Return p.TempDir + p.Name + "_out." + OutputFileType
            End If
        End Get
    End Property

    Overridable Function GetMenu() As MenuList
    End Function

    Overridable Sub ImportCommandLine(commandLine As String)
        Throw New NotImplementedException("import is not implemented for this encoder")
    End Sub

    Sub AfterEncoding()
        If Not g.WasFileJustWritten(OutputPath) Then
            Throw New ErrorAbortException("Encoder output file is missing", OutputPath)
        Else
            Log.WriteLine(MediaInfo.GetSummary(OutputPath))
        End If
    End Sub

    Overrides Function CreateEditControl() As Control
        Dim ret As New ToolStripEx

        ret.ShowItemToolTips = False
        ret.GripStyle = ToolStripGripStyle.Hidden
        ret.BackColor = System.Drawing.SystemColors.Window
        ret.Dock = DockStyle.Fill
        ret.BackColor = System.Drawing.SystemColors.Window
        ret.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        ret.Padding = New Padding(1, 1, 0, 0)
        ret.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)

        For Each i In GetMenu()
            Dim b As New ToolStripButton
            b.Margin = New Padding(0)
            b.Text = i.Key
            b.Padding = New Padding(4)
            Dim happy = i
            AddHandler b.Click, Sub() happy.Value.Invoke()
            b.TextAlign = ContentAlignment.MiddleLeft
            ret.Items.Add(b)
        Next

        Return ret
    End Function

    Overridable ReadOnly Property IsCompCheckEnabled() As Boolean
        Get
            Return Not QualityMode
        End Get
    End Property

    Protected Sub OnAfterCompCheck()
        If p.CompCheckAction = CompCheckAction.AdjustFileSize Then
            Dim oldSize = g.MainForm.tbTargetSize.Text
            g.MainForm.tbTargetSize.Text = g.GetAutoSize(AutoCompCheckValue).ToString
            Log.WriteLine("Target size: " & oldSize & " MB -> " + g.MainForm.tbTargetSize.Text + " MB")
        ElseIf p.CompCheckAction = CompCheckAction.AdjustImageSize Then
            AutoSetImageSize()
        End If
    End Sub

    Sub AutoSetImageSize()
        If p.VideoEncoder.AutoCompCheckValue > 0 AndAlso Calc.GetPercent <> 0 AndAlso
            p.Script.IsFilterActive("Resize") Then

            Dim oldWidth = p.TargetWidth
            Dim oldHeight = p.TargetHeight

            p.TargetWidth = Calc.FixMod16(CInt((p.SourceHeight - p.CropTop - p.CropBottom) * Calc.GetTargetDAR()))

            Dim cropw = p.SourceWidth - p.CropLeft - p.CropRight

            If p.TargetWidth > cropw Then
                p.TargetWidth = cropw
            End If

            p.TargetHeight = Calc.FixMod16(CInt(p.TargetWidth / Calc.GetTargetDAR()))

            While Calc.GetPercent < p.VideoEncoder.AutoCompCheckValue
                If p.TargetWidth - 16 >= 320 Then
                    p.TargetWidth -= 16
                    p.TargetHeight = Calc.FixMod16(CInt(p.TargetWidth / Calc.GetTargetDAR()))
                Else
                    Exit While
                End If
            End While

            g.MainForm.tbTargetWidth.Text = p.TargetWidth.ToString
            g.MainForm.tbTargetHeight.Text = p.TargetHeight.ToString

            Log.WriteLine("Target image size: " & oldWidth.ToString & "x" & oldHeight.ToString & " -> " & p.TargetWidth.ToString & "x" & p.TargetHeight.ToString)

            If p.AutoSmartCrop Then
                g.MainForm.StartSmartCrop()
            End If
        End If
    End Sub

    Overrides Sub Clean()
        Muxer.Clean()
    End Sub

    Overridable Function GetError() As String
        Return Nothing
    End Function

    Sub OnStateChange()
        g.MainForm.UpdateEncoderStateRelatedControls()
        g.MainForm.SetEncoderControl(p.VideoEncoder.CreateEditControl)
        g.MainForm.lgbEncoder.Text = g.ConvertPath(p.VideoEncoder.Name).Shorten(38)
        g.MainForm.llMuxer.Text = p.VideoEncoder.Muxer.OutputType.ToUpper
        g.MainForm.tbSize_TextChanged()
    End Sub

    Public Enum Modes
        First = 1
        Second = 2
        CompCheck = 4
    End Enum

    Sub OpenMuxerConfigDialog()
        Dim m = ObjectHelp.GetCopy(Of Muxer)(Muxer)

        If m.Edit = DialogResult.OK Then
            Muxer = m
            g.MainForm.llMuxer.Text = Muxer.OutputType.ToUpper
            g.MainForm.Refresh()
            g.MainForm.tbSize_TextChanged()
            g.MainForm.Assistant()
        End If
    End Sub

    Function OpenMuxerProfilesDialog() As DialogResult
        Using f As New ProfilesForm("Muxer Profiles", s.MuxerProfiles,
                                    AddressOf LoadMuxer,
                                    AddressOf GetMuxerProfile,
                                    AddressOf Muxer.GetDefaults)
            Return f.ShowDialog()
        End Using
    End Function

    Sub LoadMuxer(profile As Profile)
        Muxer = DirectCast(ObjectHelp.GetCopy(profile), Muxer)
        Muxer.Init()

        g.MainForm.llMuxer.Text = Muxer.OutputType.ToUpper
        g.MainForm.tbTargetFile.Text = p.TargetFile.ChangeExt(Muxer.GetExtension)
        g.MainForm.RecalcBitrate()
        g.MainForm.Assistant()
    End Sub

    Private Function GetMuxerProfile() As Profile
        Dim sb As New SelectionBox(Of Muxer)

        sb.Title = "New Profile"
        sb.Text = "Please choose a profile."

        sb.AddItem("Current Project", Muxer)

        For Each i In StaxRip.Muxer.GetDefaults()
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK Then Return sb.SelectedItem

        Return Nothing
    End Function

    Shared Function Getx264Encoder(
        name As String,
        device As x264DeviceMode,
        Optional mode As x264Mode = x264Mode.SingleCRF) As x264Encoder

        Return Getx264Encoder(name, mode, x264PresetMode.Medium, x264TuneMode.Disabled, device)
    End Function

    Shared Function Getx264Encoder(name As String,
                                   mode As x264Mode,
                                   preset As x264PresetMode,
                                   tuning As x264TuneMode,
                                   device As x264DeviceMode) As x264Encoder

        Dim r As New x264Encoder()
        r.Name = name
        r.Params.Mode.Value = mode
        r.Params.Preset.Value = preset
        r.Params.Tune.Value = tuning
        r.Params.Device.Value = device
        r.Params.ApplyDeviceSettings()
        r.Params.ApplyDefaults(r.Params)
        r.Params.ApplyDeviceSettings()
        r.SetMuxer()

        Return r
    End Function

    Shared Function GetDefaults() As List(Of VideoEncoder)
        Dim ret As New List(Of VideoEncoder)

        ret.Add(Getx264Encoder("x264", x264Mode.SingleCRF, x264PresetMode.Medium, x264TuneMode.Disabled, x264DeviceMode.Disabled))

        Dim x265crf = New x265Encoder
        x265crf.Params.ApplyPresetDefaultValues()
        x265crf.Params.ApplyPresetValues()
        ret.Add(x265crf)

        ret.Add(New IntelEncoder())

        Dim intel265 As New IntelEncoder()
        intel265.Params.Codec.Value = 1
        ret.Add(intel265)

        Dim nvidia264 As New NVIDIAEncoder()
        nvidia264.Params.Mode.Value = 3
        ret.Add(nvidia264)

        Dim nvidia265 As New NVIDIAEncoder()
        nvidia265.Params.Mode.Value = 3
        nvidia265.Params.Codec.Value = 1
        ret.Add(nvidia265)

        Dim amd264 As New AMDEncoder()
        amd264.Params.Mode.Value = 2
        ret.Add(amd264)

        Dim xvid As New BatchEncoder()
        xvid.OutputFileTypeValue = "avi"
        xvid.Name = "XviD"
        xvid.Muxer = New ffmpegMuxer("AVI")
        xvid.QualityMode = True
        xvid.CommandLines = """%app:xvid_encraw%"" -cq 2 -smoother 0 -max_key_interval 250 -nopacked -vhqmode 4 -qpel -notrellis -max_bframes 1 -bvhq -bquant_ratio 162 -bquant_offset 0 -threads 1 -i ""%script_file%"" -avi ""%encoder_out_file%"" -par %target_sar%"
        ret.Add(xvid)

        ret.Add(New NullEncoder())

        ret.Add(Getx264Encoder("2 pass | x264", x264Mode.TwoPass, x264PresetMode.Medium, x264TuneMode.Disabled, x264DeviceMode.Disabled))

        Dim x265_2pass = New x265Encoder
        x265_2pass.Name = "2 pass | x265"
        x265_2pass.Params.Mode.Value = x265RateMode.TwoPass
        x265_2pass.Params.ApplyPresetDefaultValues()
        x265_2pass.Params.ApplyPresetValues()
        ret.Add(x265_2pass)

        Dim xvid2pass As New BatchEncoder()
        xvid2pass.OutputFileTypeValue = "avi"
        xvid2pass.Name = "2 pass | XviD"
        xvid2pass.Muxer = New ffmpegMuxer("AVI")
        xvid2pass.CommandLines = """%app:xvid_encraw%"" -smoother 0 -max_key_interval 250 -nopacked -vhqmode 4 -qpel -notrellis -max_bframes 1 -bvhq -bquant_ratio 162 -bquant_offset 0 -threads 1 -bitrate %video_bitrate% -par %target_sar% -turbo -pass1 ""%temp_file%.stats"" -i ""%script_file%"" || exit" + BR +
                                 """%app:xvid_encraw%"" -smoother 0 -max_key_interval 250 -nopacked -vhqmode 4 -qpel -notrellis -max_bframes 1 -bvhq -bquant_ratio 162 -bquant_offset 0 -threads 1 -bitrate %video_bitrate% -par %target_sar% -pass2 ""%temp_file%.stats"" -i ""%script_file%"" -avi ""%encoder_out_file%"""
        xvid2pass.CompCheckCommandLines = """%app:xvid_encraw%"" -cq 2 -smoother 0 -max_key_interval 250 -nopacked -vhqmode 4 -qpel -notrellis -max_bframes 1 -bvhq -bquant_ratio 162 -bquant_offset 0 -threads 1 -par %target_sar% -i ""%temp_file%_CompCheck.%script_ext%"" -avi ""%temp_file%_CompCheck.avi"""
        ret.Add(xvid2pass)

        Dim ffmpeg = New ffmpegEncoder()

        For x = 0 To ffmpeg.Params.Codec.Options.Length - 1
            Dim ffmpeg2 = New ffmpegEncoder()
            ffmpeg2.Params.Codec.Value = x
            ret.Add(ffmpeg2)
        Next

        Dim x264cli As New BatchEncoder()
        x264cli.OutputFileTypeValue = "h264"
        x264cli.Name = "Command Line | x264"
        x264cli.Muxer = New MkvMuxer()
        x264cli.AutoCompCheckValue = 50
        x264cli.CommandLines = """%app:x264%"" --pass 1 --bitrate %video_bitrate% --stats ""%temp_file%.stats"" --output NUL ""%script_file%"" || exit" + BR + """%app:x264%"" --pass 2 --bitrate %video_bitrate% --stats ""%temp_file%.stats"" --output ""%encoder_out_file%"" ""%script_file%"""
        x264cli.CompCheckCommandLines = """%app:x264%"" --crf 18 --output ""%temp_file%_CompCheck%encoder_ext%"" ""%temp_file%_CompCheck.%script_ext%"""
        ret.Add(x264cli)

        Dim divx As New BatchEncoder()
        divx.OutputFileTypeValue = "h265"
        divx.Name = "Command Line | DivX H.265"
        divx.Muxer = New MkvMuxer()
        divx.CommandLines = """%app:DivX265%"" -i ""%script_file%"" -o ""%encoder_out_file%"" -br %video_bitrate% -10"
        ret.Add(divx)

        Dim nvencH265 As New BatchEncoder()
        nvencH265.OutputFileTypeValue = "h265"
        nvencH265.Name = "Command Line | NVIDIA H.265"
        nvencH265.Muxer = New MkvMuxer()
        nvencH265.QualityMode = True
        nvencH265.CommandLines = """%app:ffmpeg%"" -i ""%script_file%"" -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | ""%app:NVEncC%"" --sar %target_sar% --codec h265 --y4m --cqp 20 -i - -o ""%encoder_out_file%"""
        ret.Add(nvencH265)

        ret.Add(Getx264Encoder("Devices | DivX Plus", x264DeviceMode.DivXPlus))
        ret.Add(Getx264Encoder("Devices | Blu-ray", x264DeviceMode.BluRay))
        ret.Add(Getx264Encoder("Devices | iPhone", x264DeviceMode.iPhone))
        ret.Add(Getx264Encoder("Devices | PlayStation", x264DeviceMode.PlayStation))
        ret.Add(Getx264Encoder("Devices | Xbox", x264DeviceMode.Xbox))

        Return ret
    End Function

    Function CompareToVideoEncoder(other As VideoEncoder) As Integer Implements System.IComparable(Of VideoEncoder).CompareTo
        Return Name.CompareTo(other.Name)
    End Function

    Overridable Sub RunCompCheck()
    End Sub

    Shared Sub SaveProfile(encoder As VideoEncoder)
        Dim name = InputBox.Show("Please enter a profile name.", "Profile Name", encoder.Name)

        If name <> "" Then
            encoder.Name = name

            For Each i In From prof In s.VideoEncoderProfiles.ToArray
                          Where prof.GetType Is encoder.GetType

                If i.Name = name Then
                    s.VideoEncoderProfiles(s.VideoEncoderProfiles.IndexOf(i)) = encoder
                    Exit Sub
                End If
            Next

            s.VideoEncoderProfiles.Insert(0, encoder)
        End If
    End Sub

    Overrides Function Edit() As DialogResult
        Using f As New ControlHostForm(Name)
            f.AddControl(CreateEditControl, Nothing)
            f.ShowDialog()
        End Using

        Return DialogResult.OK
    End Function

    Class MenuList
        Inherits List(Of KeyValuePair(Of String, Action))

        Overloads Sub Add(text As String, action As Action)
            Add(New KeyValuePair(Of String, Action)(text, action))
        End Sub
    End Class
End Class

<Serializable()>
Public MustInherit Class BasicVideoEncoder
    Inherits VideoEncoder

    MustOverride ReadOnly Property CommandLineParams As CommandLineParams

    Public Overrides Sub ImportCommandLine(commandLine As String)
        If commandLine = "" Then Exit Sub

        Dim a = commandLine.SplitNoEmptyAndWhiteSpace(" ")

        For x = 0 To a.Length - 1
            For Each param In CommandLineParams.Items
                If Not param.ImportAction Is Nothing AndAlso
                    param.GetSwitches.Contains(a(x)) AndAlso a.Length - 1 > x Then

                    param.ImportAction.Invoke(a(x + 1))
                    Exit For
                End If

                If TypeOf param Is BoolParam Then
                    Dim boolParam = DirectCast(param, BoolParam)

                    If boolParam.GetSwitches.Contains(a(x)) Then
                        boolParam.Value = True
                        Exit For
                    End If
                ElseIf TypeOf param Is NumParam Then
                    Dim numParam = DirectCast(param, NumParam)

                    If numParam.GetSwitches.Contains(a(x)) AndAlso
                        a.Length - 1 > x AndAlso a(x + 1).IsSingle Then

                        numParam.Value = a(x + 1).ToSingle
                        Exit For
                    End If
                ElseIf TypeOf param Is OptionParam Then
                    Dim optionParam = DirectCast(param, OptionParam)

                    If optionParam.GetSwitches.Contains(a(x)) AndAlso a.Length - 1 > x Then
                        Dim exitFor As Boolean

                        For xOpt = 0 To optionParam.Options.Length - 1
                            If a(x + 1).Trim(""""c) = optionParam.Options(xOpt) Then
                                optionParam.Value = xOpt
                                exitFor = True
                                Exit For
                            End If
                        Next

                        If exitFor Then Exit For
                    End If
                ElseIf TypeOf param Is StringParam Then
                    Dim stringParam = DirectCast(param, StringParam)

                    If stringParam.GetSwitches.Contains(a(x)) AndAlso a.Length - 1 > x Then
                        stringParam.Value = a(x + 1).Trim(""""c)
                        Exit For
                    End If
                End If
            Next
        Next
    End Sub
End Class

<Serializable()>
Class BatchEncoder
    Inherits VideoEncoder

    Sub New()
        Name = "Command Line"
        Muxer = New MkvMuxer()
    End Sub

    Property CommandLines As String = ""
    Property CompCheckCommandLines As String = ""

    Property OutputFileTypeValue As String

    Overrides ReadOnly Property OutputfileType As String
        Get
            If OutputFileTypeValue = "" Then OutputFileTypeValue = "h264"

            Return OutputFileTypeValue
        End Get
    End Property

    Overrides Sub ShowConfigDialog()
        Using f As New BatchEncoderForm(Me)
            If f.ShowDialog() = DialogResult.OK Then
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides Function GetMenu() As MenuList
        Dim ret As New MenuList

        ret.Add("Codec Configuration", AddressOf ShowConfigDialog)

        If IsCompCheckEnabled Then
            ret.Add("Run Compressibility Check", AddressOf RunCompCheck)
        End If

        ret.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)

        Return ret
    End Function

    Function GetSkipStrings(commands As String) As String()
        If commands.Contains("DivX265") Then
            Return {"encoded @"}
        ElseIf commands.Contains("xvid_encraw") Then
            Return {"key="}
        ElseIf commands.Contains("x264") Then
            Return {"frames,"}
        ElseIf commands.Contains("NVEncC") Then
            Return {"frames: "}
        Else
            Return {" [ETA ", ", eta ", "frames: ", "frame= "}
        End If
    End Function

    Overrides Sub Encode()
        p.Script.Synchronize()
        Dim commands = Macro.Solve(CommandLines).Trim

        If commands.Contains("|") OrElse commands.Contains(BR) Then
            Dim batchPath = p.TempDir + Filepath.GetBase(p.TargetFile) + "_encode.bat"
            File.WriteAllText(batchPath, commands, Encoding.GetEncoding(850))

            Using proc As New Proc
                proc.Init("Encoding video command line encoder: " + Name)
                proc.SkipStrings = GetSkipStrings(commands)
                proc.WriteLine(commands + BR2)
                proc.File = "cmd.exe"
                proc.Arguments = "/C call """ + batchPath + """"

                Try
                    proc.Start()
                Catch ex As AbortException
                    Throw ex
                Catch ex As Exception
                    ProcessForm.CloseProcessForm()
                    g.ShowException(ex)
                    Throw New AbortException
                End Try
            End Using
        Else
            Using proc As New Proc
                proc.Init("Encoding video command line encoder: " + Name)
                proc.SkipStrings = GetSkipStrings(commands)
                proc.CommandLine = commands

                Try
                    proc.Start()
                Catch ex As AbortException
                    Throw ex
                Catch ex As Exception
                    ProcessForm.CloseProcessForm()
                    g.ShowException(ex)
                    Throw New AbortException
                End Try
            End Using
        End If
    End Sub

    Overrides Sub RunCompCheck()
        If CompCheckCommandLines = "" OrElse CompCheckCommandLines.Trim = "" Then
            ShowConfigDialog()
            Exit Sub
        End If

        If Not g.VerifyRequirements Then Exit Sub
        If Not g.IsValidSource Then Exit Sub

        ProcessForm.ShowForm()
        Log.WriteHeader("Compressibility Check")

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

        Log.WriteLine(code + BR2)
        script.Filters.Add(New VideoFilter("aaa", "aaa", code))
        script.Path = p.TempDir + p.Name + "_CompCheck." + script.FileType
        script.Synchronize()

        Dim batchPath = p.TempDir + Filepath.GetBase(p.TargetFile) + "_CompCheck.bat"
        Dim command = Macro.Solve(CompCheckCommandLines)

        File.WriteAllText(batchPath, command, Encoding.GetEncoding(850))
        Log.WriteLine(command + BR2)

        Using proc As New Proc
            proc.Init(Nothing)
            proc.SkipStrings = GetSkipStrings(command)
            proc.File = "cmd.exe"
            proc.Arguments = "/C call """ + batchPath + """"

            Try
                proc.Start()
            Catch ex As AbortException
                ProcessForm.CloseProcessForm()
                Exit Sub
            Catch ex As Exception
                ProcessForm.CloseProcessForm()
                g.ShowException(ex)
                Exit Sub
            End Try
        End Using

        Dim bits = (New FileInfo(p.TempDir + p.Name + "_CompCheck." + OutputfileType).Length) * 8
        p.Compressibility = (bits / script.GetFrames) / (p.TargetWidth * p.TargetHeight)

        OnAfterCompCheck()

        g.MainForm.Assistant()

        Log.WriteLine(CInt(Calc.GetPercent).ToString() + " %")

        ProcessForm.CloseProcessForm()
    End Sub
End Class

<Serializable()>
Class NullEncoder
    Inherits VideoEncoder

    Sub New()
        Name = "Just Mux"
        Muxer = New MkvMuxer()
        QualityMode = True
    End Sub

    Function GetSource() As String
        For Each i In {".h264", ".avc", ".h265", ".hevc", ".mpg", ".avi"}
            If File.Exists(Filepath.GetDirAndBase(p.SourceFile) + "_out" + i) Then
                Return Filepath.GetDirAndBase(p.SourceFile) + "_out" + i
            ElseIf File.Exists(p.TempDir + p.Name + "_out" + i) Then
                Return p.TempDir + p.Name + "_out" + i
            End If
        Next

        If FileTypes.VideoIndex.Contains(Filepath.GetExt(p.SourceFile)) Then
            Return p.LastOriginalSourceFile
        Else
            Return p.SourceFile
        End If
    End Function

    Overrides ReadOnly Property OutputPath As String
        Get
            Dim source = GetSource()

            If Not p.VideoEncoder.Muxer.IsSupported(Filepath.GetExt(source)) Then
                Select Case Filepath.GetExt(source)
                    Case "mkv"
                        Dim streams = MediaInfo.GetVideoStreams(source)
                        If streams.Count = 0 Then Return source
                        Dim stream = streams(0)
                        If stream.Extension = "" Then Throw New Exception("demuxing of video stream format is not implemented")
                        Return p.TempDir + Filepath.GetBase(source) + "_out" + stream.Extension
                End Select
            End If

            Return source
        End Get
    End Property

    Overrides ReadOnly Property OutputFileType As String
        Get
            Return Filepath.GetExt(OutputPath)
        End Get
    End Property

    Overrides Sub Encode()
        Dim source = GetSource()

        If Not p.VideoEncoder.Muxer.IsSupported(Filepath.GetExt(source)) Then
            Select Case Filepath.GetExt(source)
                Case "mkv"
                    Video.DemuxMKV(source)
            End Select
        End If
    End Sub

    Overrides Function GetMenu() As MenuList
        Dim ret As New MenuList
        ret.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return ret
    End Function

    Overrides Sub ShowConfigDialog()
    End Sub
End Class

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

    Overrides ReadOnly Property OutputFileType() As String
        Get
            Return "h264"
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Dim cl = Params.GetCommandLine(True, False)

        If cl.Contains(" | ") Then
            Dim batchPath = p.TempDir + p.TargetFile.Base + "_VCEEncC.bat"
            File.WriteAllText(batchPath, cl, Encoding.GetEncoding(850))

            Using proc As New Proc
                proc.Init("Encoding using VCEEncC " + Package.VCEEncC.Version)
                proc.SkipStrings = {"%]", " frames: "}
                proc.WriteLine(cl + BR2)
                proc.File = "cmd.exe"
                proc.Arguments = "/C call """ + batchPath + """"
                proc.Start()
            End Using
        Else
            Using proc As New Proc
                proc.Init("Encoding using VCEEncC " + Package.VCEEncC.Version)
                proc.SkipStrings = {"%]"}
                proc.File = Package.VCEEncC.Path
                proc.Arguments = cl
                proc.Start()
            End Using
        End If

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

        Property Quality As New OptionParam With {
            .Switch = "--quality",
            .Text = "Quality:",
            .Options = {"fast", "balanced", "slow"},
            .Value = 1,
            .DefaultValue = 1}

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

        Property MaxBitrate As New NumParam With {
            .Switch = "--max-bitrate",
            .Text = "Max Bitrate:",
            .Value = 20000,
            .DefaultValue = 20000,
            .MinMaxStep = {0, 1000000, 1}}

        Property GOPLength As New NumParam With {
            .Switch = "--gop-len",
            .Text = "GOP Length:",
            .MinMaxStep = {0, 10000, 1}}

        Property BFrames As New NumParam With {
            .Switch = "--bframes",
            .Text = "B Frames:",
            .MinMaxStep = {0, 16, 1}}

        Property QPMax As New NumParam With {
            .Switch = "--qp-max",
            .Text = "QP Max:",
            .MinMaxStep = {0, 100, 1},
            .Value = 100,
            .DefaultValue = 100}

        Property QPMin As New NumParam With {
            .Switch = "--qp-min",
            .Text = "QP Min:",
            .MinMaxStep = {0, 100, 1}}

        Property VBVBufsize As New NumParam With {
            .Switch = "--vbv-bufsize",
            .Text = "VBV Bufsize:",
            .MinMaxStep = {0, 1000000, 1},
            .Value = 20000,
            .DefaultValue = 20000}

        Property Custom As New StringParam With {
            .Text = "Custom Switches:",
            .ArgsFunc = Function() Custom.Value}

        Private ItemsValue As List(Of CommandLineParam)

        Overrides ReadOnly Property Items As List(Of CommandLineParam)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineParam)
                    Add("Basic", Decoder, Mode, Quality, QPI, QPP, QPB, GOPLength, BFrames)
                    Add("Advanced", MaxBitrate, VBVBufsize, QPMin, QPMax, Custom)
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
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputFileType)

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

            If sourcePath = "-" Then ret += " --y4m"

            If includePaths Then ret += " -i " + sourcePath.Quotes + " -o " + targetPath.Quotes

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Package.VCEEncC
        End Function
    End Class
End Class