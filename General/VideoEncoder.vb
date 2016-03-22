Imports System.Globalization
Imports System.Text

Imports StaxRip.UI
Imports StaxRip.CommandLine

<Serializable()>
MustInherit Class VideoEncoder
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
            Dim oldSize = g.MainForm.tbSize.Text
            g.MainForm.tbSize.Text = g.GetAutoSize(AutoCompCheckValue).ToString
            Log.WriteLine("Target size: " & oldSize & " MB -> " + g.MainForm.tbSize.Text + " MB")
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
                g.MainForm.PerformSmartCrop()
            End If
        End If
    End Sub

    Overrides Sub Clean()
        Muxer.Clean()
    End Sub

    Overridable Function GetFrameRate() As Double
        Return p.Script.GetFramerate
    End Function

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

        Dim x265crf = New x265.x265Encoder
        x265crf.Params.ApplyPresetDefaultValues()
        x265crf.Params.ApplyPresetValues()
        ret.Add(x265crf)

        Dim intel264 As New IntelEncoder()
        intel264.Params.BFrames.Value = 3
        ret.Add(intel264)

        Dim intel265 As New IntelEncoder()
        intel265.Params.Codec.Value = 1
        intel265.Params.BFrames.Value = 2
        ret.Add(intel265)

        Dim nvidia264 As New NvidiaEncoder()
        nvidia264.Params.Mode.Value = 3
        ret.Add(nvidia264)

        Dim nvidia265 As New NvidiaEncoder()
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

        Dim x265_2pass = New x265.x265Encoder
        x265_2pass.Name = "2 pass | x265"
        x265_2pass.Params.Mode.Value = x265.RateMode.TwoPass
        x265_2pass.Params.ApplyPresetDefaultValues()
        x265_2pass.Params.ApplyPresetValues()
        ret.Add(x265_2pass)

        Dim xvid2pass As New BatchEncoder()
        xvid2pass.OutputFileTypeValue = "avi"
        xvid2pass.Name = "2 pass | XviD"
        xvid2pass.Muxer = New ffmpegMuxer("AVI")
        xvid2pass.CommandLines = """%app:xvid_encraw%"" -smoother 0 -max_key_interval 250 -nopacked -vhqmode 4 -qpel -notrellis -max_bframes 1 -bvhq -bquant_ratio 162 -bquant_offset 0 -threads 1 -bitrate %video_bitrate% -par %target_sar% -turbo -pass1 ""%temp_file%.stats"" -i ""%script_file%"" || exit" + CrLf +
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
        x264cli.CommandLines = """%app:x264%"" --pass 1 --bitrate %video_bitrate% --stats ""%temp_file%.stats"" --output NUL ""%script_file%"" || exit" + CrLf + """%app:x264%"" --pass 2 --bitrate %video_bitrate% --stats ""%temp_file%.stats"" --output ""%encoder_out_file%"" ""%script_file%"""
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
        Dim commands = Macro.Solve(CommandLines).Trim

        If commands.Contains("|") OrElse commands.Contains(CrLf) Then
            Dim batchPath = p.TempDir + Filepath.GetBase(p.TargetFile) + "_encode.bat"
            File.WriteAllText(batchPath, commands, Encoding.GetEncoding(850))

            Using proc As New Proc
                proc.Init("Encoding video command line encoder: " + Name)
                proc.SkipStrings = GetSkipStrings(commands)
                proc.WriteLine(commands + CrLf2)
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

        If Not Paths.VerifyRequirements Then Exit Sub
        If Not g.IsValidSource Then Exit Sub

        ProcessForm.ShowForm()
        Log.WriteHeader("Compressibility Check")

        Dim script As New VideoScript
        script.Engine = p.Script.Engine
        script.Filters = p.Script.GetFiltersCopy
        Dim code As String
        Dim every = ((100 \ p.CompCheckRange) * 14).ToString

        If script.Engine = ScriptingEngine.AviSynth Then
            code = "SelectRangeEvery(" + every + ",14)"
        Else
            code = "fpsnum = clip.fps_num" + CrLf + "fpsden = clip.fps_den" + CrLf +
                "clip = core.std.SelectEvery(clip = clip, cycle = " + every + ", offsets = range(14))" + CrLf +
                "clip = core.std.AssumeFPS(clip = clip, fpsnum = fpsnum, fpsden = fpsden)"
        End If

        Log.WriteLine(code + CrLf2)
        script.Filters.Add(New VideoFilter("aaa", "aaa", code))
        script.Path = p.TempDir + p.Name + "_CompCheck." + script.FileType
        script.Synchronize()

        Dim batchPath = p.TempDir + Filepath.GetBase(p.TargetFile) + "_CompCheck.bat"
        Dim command = Macro.Solve(CompCheckCommandLines)

        File.WriteAllText(batchPath, command, Encoding.GetEncoding(850))
        Log.WriteLine(command + CrLf2)

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
Class ffmpegEncoder
    Inherits VideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "ffmpeg | " + Params.Codec.OptionText
        End Get
    End Property

    Sub New()
        Muxer = New ffmpegMuxer("AVI")
    End Sub

    <NonSerialized>
    Private ParamsValue As ffmpegParams

    Property Params As ffmpegParams
        Get
            If ParamsValue Is Nothing Then
                ParamsValue = New ffmpegParams

                ParamsValue.Init(ParamsStore)
            End If

            Return ParamsValue
        End Get
        Set(value As ffmpegParams)
            ParamsValue = value
        End Set
    End Property

    Overrides Sub ShowConfigDialog()
        Dim newParams As New ffmpegParams
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(store)

        Using f As New CommandLineForm(newParams)
            If f.ShowDialog() = DialogResult.OK Then
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides ReadOnly Property OutputFileType() As String
        Get
            Select Case Params.Codec.OptionText
                Case "Xvid", "ASP"
                    Return "avi"
                Case Else
                    Return "mkv"
            End Select
        End Get
    End Property

    Overrides ReadOnly Property IsCompCheckEnabled() As Boolean
        Get
            Return False
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Params.RaiseValueChanged(Nothing)

        If Params.Mode.OptionText = "Two Pass" Then
            Encode(Params.GetCommandLine(True, False, 1))
            Encode(Params.GetCommandLine(True, False, 2))
        Else
            Encode(Params.GetCommandLine(True, False))
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(args As String)
        Using proc As New Proc
            proc.Init("Encoding " + Params.Codec.OptionText + " using ffmpeg", "frame=")
            proc.Encoding = Encoding.UTF8
            proc.WorkingDirectory = p.TempDir
            proc.File = Packs.ffmpeg.GetPath
            proc.Arguments = args
            proc.Start()
        End Using
    End Sub

    Overrides Function GetMenu() As MenuList
        Dim r As New MenuList

        r.Add("Encoder Options", AddressOf ShowConfigDialog)
        r.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)

        Return r
    End Function

    Overrides Property QualityMode() As Boolean
        Get
            Return Params.Mode.Value = EncodingMode.Quality
        End Get
        Set(Value As Boolean)
        End Set
    End Property

    Class ffmpegParams
        Inherits CommandLineParams

        'h264_qsv encoder AVOptions:
        '  -async_depth       <int>        E..V.... Maximum processing parallelism (from 0 to INT_MAX) (default 4)
        '  -avbr_accuracy     <int>        E..V.... Accuracy of the AVBR ratecontrol (from 0 to INT_MAX) (default 0)
        '  -avbr_convergence  <int>        E..V.... Convergence of the AVBR ratecontrol (from 0 to INT_MAX) (default 0)
        '  -preset            <int>        E..V.... (from 1 to 7) (default medium)
        '     veryfast                     E..V....
        '     faster                       E..V....
        '     fast                         E..V....
        '     medium                       E..V....
        '     slow                         E..V....
        '     slower                       E..V....
        '     veryslow                     E..V....
        '  -vcm               <int>        E..V.... Use the video conferencing mode ratecontrol (from 0 to 1) (default 0)
        '  -rdo               <int>        E..V.... Enable rate distortion optimization (from -1 to 1) (default -1)
        '  -max_frame_size    <int>        E..V.... Maximum encoded frame size in bytes (from -1 to 65535) (default -1)
        '  -max_slice_size    <int>        E..V.... Maximum encoded slice size in bytes (from -1 to 65535) (default -1)
        '  -bitrate_limit     <int>        E..V.... Toggle bitrate limitations (from -1 to 1) (default -1)
        '  -mbbrc             <int>        E..V.... MB level bitrate control (from -1 to 1) (default -1)
        '  -extbrc            <int>        E..V.... Extended bitrate control (from -1 to 1) (default -1)
        '  -adaptive_i        <int>        E..V.... Adaptive I-frame placement (from -1 to 1) (default -1)
        '  -adaptive_b        <int>        E..V.... Adaptive B-frame placement (from -1 to 1) (default -1)
        '  -b_strategy        <int>        E..V.... Strategy to choose between I/P/B-frames (from -1 to 1) (default -1)
        '  -cavlc             <int>        E..V.... Enable CAVLC (from 0 to 1) (default 0)
        '  -idr_interval      <int>        E..V.... Distance (in I-frames) between IDR frames (from 0 to INT_MAX) (default 0)
        '  -pic_timing_sei    <int>        E..V.... Insert picture timing SEI with pic_struct_syntax element (from 0 to 1) (default 1)
        '  -single_sei_nal_unit <int>        E..V.... Put all the SEI messages into one NALU (from -1 to 1) (default -1)
        '  -max_dec_frame_buffering <int>        E..V.... Maximum number of frames buffered in the DPB (from 0 to 65535) (default 0)
        '  -look_ahead        <int>        E..V.... Use VBR algorithm with look ahead (from 0 to 1) (default 1)
        '  -look_ahead_depth  <int>        E..V.... Depth of look ahead in number frames (from 0 to 100) (default 0)
        '  -look_ahead_downsampling <int>        E..V.... (from 0 to 2) (default unknown)
        '     unknown                      E..V....
        '     off                          E..V....
        '     2x                           E..V....
        '  -int_ref_type      <int>        E..V.... Intra refresh type (from -1 to 65535) (default -1)
        '     none                         E..V....
        '     vertical                     E..V....
        '  -int_ref_cycle_size <int>        E..V.... Number of frames in the intra refresh cycle (from -1 to 65535) (default -1)
        '  -int_ref_qp_delta  <int>        E..V.... QP difference for the refresh MBs (from -32768 to 32767) (default -32768)
        '  -recovery_point_sei <int>        E..V.... Insert recovery point SEI messages (from -1 to 1) (default -1)
        '  -trellis           <flags>      E..V.... Trellis quantization (default 0)
        '     off                          E..V....
        '     I                            E..V....
        '     P                            E..V....
        '     B                            E..V....
        '  -profile           <int>        E..V.... (from 0 to INT_MAX) (default unknown)
        '     unknown                      E..V....
        '     baseline                     E..V....
        '     main                         E..V....
        '     high                         E..V....
        '  -a53cc             <int>        E..V.... Use A53 Closed Captions (if available) (from 0 to 1) (default 0)

        'hevc_qsv encoder AVOptions:
        '  -async_depth       <int>        E..V.... Maximum processing parallelism (from 0 to INT_MAX) (default 4)
        '  -avbr_accuracy     <int>        E..V.... Accuracy of the AVBR ratecontrol (from 0 to INT_MAX) (default 0)
        '  -avbr_convergence  <int>        E..V.... Convergence of the AVBR ratecontrol (from 0 to INT_MAX) (default 0)
        '  -preset            <int>        E..V.... (from 1 to 7) (default medium)
        '     veryfast                     E..V....
        '     faster                       E..V....
        '     fast                         E..V....
        '     medium                       E..V....
        '     slow                         E..V....
        '     slower                       E..V....
        '     veryslow                     E..V....
        '  -vcm               <int>        E..V.... Use the video conferencing mode ratecontrol (from 0 to 1) (default 0)
        '  -rdo               <int>        E..V.... Enable rate distortion optimization (from -1 to 1) (default -1)
        '  -max_frame_size    <int>        E..V.... Maximum encoded frame size in bytes (from -1 to 65535) (default -1)
        '  -max_slice_size    <int>        E..V.... Maximum encoded slice size in bytes (from -1 to 65535) (default -1)
        '  -bitrate_limit     <int>        E..V.... Toggle bitrate limitations (from -1 to 1) (default -1)
        '  -mbbrc             <int>        E..V.... MB level bitrate control (from -1 to 1) (default -1)
        '  -extbrc            <int>        E..V.... Extended bitrate control (from -1 to 1) (default -1)
        '  -adaptive_i        <int>        E..V.... Adaptive I-frame placement (from -1 to 1) (default -1)
        '  -adaptive_b        <int>        E..V.... Adaptive B-frame placement (from -1 to 1) (default -1)
        '  -b_strategy        <int>        E..V.... Strategy to choose between I/P/B-frames (from -1 to 1) (default -1)
        '  -cavlc             <int>        E..V.... Enable CAVLC (from 0 to 1) (default 0)
        '  -load_plugin       <int>        E..V.... A user plugin to load in an internal session (from 0 to 2) (default hevc_sw)
        '     none                         E..V....
        '     hevc_sw                      E..V....
        '     hevc_hw                      E..V....
        '  -load_plugins      <string>     E..V.... A :-separate list of hexadecimal plugin UIDs to load in an internal session (default '')
        '  -profile           <int>        E..V.... (from 0 to INT_MAX) (default unknown)
        '     unknown                      E..V....
        '     main                         E..V....
        '     main10                       E..V....
        '     mainsp                       E..V....

        Sub New()
            Title = "ffmpeg Encoding Options"
        End Sub

        Property Codec As New OptionParam With {
            .Switch = "-c:v",
            .Text = "Codec:",
            .AlwaysOn = True,
            .Options = {"VP9", "Xvid", "ASP", "Theora", "H.264 Intel", "H.265 Intel", "H.264 NVIDIA"},
            .Values = {"libvpx-vp9", "libxvid", "mpeg4", "libtheora", "h264_qsv", "hevc_qsv", "nvenc_h264"}}

        Property Mode As New OptionParam With {
            .Name = "Mode",
            .Text = "Mode:",
            .Options = {"Quality", "One Pass", "Two Pass"}}

        Property Speed As New OptionParam With {
            .Switch = "-speed",
            .Text = "Speed:",
            .AlwaysOn = True,
            .VisibleFunc = Function() Codec.OptionText = "VP9",
            .Options = {"6 - Fastest", "5 - Faster", "4 - Fast", "3 - Medium", "2 - Slow", "1 - Slower", "0 - Slowest"},
            .Values = {"6", "5", "4", "3", "2", "1", "0"},
            .Value = 5}

        Property AQmode As New OptionParam With {
            .Switch = "-aq-mode",
            .Text = "AQ Mode:",
            .VisibleFunc = Function() Codec.OptionText = "VP9",
            .Options = {"Disabled", "0", "1", "2", "3"},
            .Values = {"Disabled", "0", "1", "2", "3"}}

        Property Quality As New NumParam With {
            .Name = "Quality",
            .Text = "Quality:",
            .VisibleFunc = Function() Mode.Value = EncodingMode.Quality,
            .MinMaxStep = {1, 63, 1},
            .Value = 25}

        Property EncodingThreads As New NumParam With {
            .Switch = "-threads",
            .Text = "Encoding Threads:",
            .VisibleFunc = Function() Codec.OptionText = "VP9",
            .Value = 8,
            .DefaultValue = -1}

        Property TileColumns As New NumParam With {
            .Switch = "-tile-columns",
            .Text = "Tile Columns:",
            .VisibleFunc = Function() Codec.OptionText = "VP9",
            .Value = 6,
            .DefaultValue = -1}

        Property FrameParallel As New NumParam With {
            .Switch = "-frame-parallel",
            .Text = "Frame Parallel:",
            .VisibleFunc = Function() Codec.OptionText = "VP9",
            .Value = 1,
            .DefaultValue = -1}

        Property AutoAltRef As New NumParam With {
            .Switch = "-auto-alt-ref",
            .Text = "Auto Alt Ref:",
            .VisibleFunc = Function() Codec.OptionText = "VP9",
            .Value = 1,
            .DefaultValue = -1}

        Property LagInFrames As New NumParam With {
            .Switch = "-lag-in-frames",
            .Text = "Lag In Frames:",
            .VisibleFunc = Function() Codec.OptionText = "VP9",
            .Value = 25,
            .DefaultValue = -1}

        Property DecodingThreads As New NumParam With {
            .Text = "Decoding Threads:",
            .MinMaxStep = {0, 64, 1},
            .Value = 1,
            .DefaultValue = 0}

        Property Decoder As New OptionParam With {
            .Text = "Decoder:",
            .Options = {"AviSynth/VapourSynth", "Intel", "DXVA2"},
            .Values = {"avs", "qsv", "dxva2"}}

        Property Custom As New StringParam With {
            .Text = "Custom Switches:"}

        Private ItemsValue As List(Of CommandLineItem)

        Overrides ReadOnly Property Items As List(Of CommandLineItem)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineItem)

                    ItemsValue.AddRange({Decoder, Codec, Mode, Speed, AQmode, Quality,
                                        DecodingThreads, EncodingThreads, TileColumns,
                                        FrameParallel, AutoAltRef, LagInFrames, Custom})
                End If

                Return ItemsValue
            End Get
        End Property

        Protected Overrides Sub OnValueChanged(item As CommandLineItem)
            MyBase.OnValueChanged(item)
        End Sub

        Overloads Overrides Function GetCommandLine(includePaths As Boolean,
                                                    includeExecutable As Boolean,
                                                    Optional pass As Integer = 0) As String
            Dim sourcePath = p.Script.Path
            Dim targetPath As String
            Dim ret As String

            If Mode.OptionText = "Two Pass" AndAlso pass = 1 Then
                targetPath = "NUL"
            Else
                targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputFileType)
            End If

            If includePaths AndAlso includeExecutable Then ret = Packs.ffmpeg.GetPath.Quotes

            Select Case Decoder.ValueText
                Case "qsv"
                    sourcePath = p.LastOriginalSourceFile
                    ret += " -hwaccel qsv"
                Case "dxva2"
                    sourcePath = p.LastOriginalSourceFile
                    ret += " -hwaccel dxva2"
            End Select

            If DecodingThreads.Value <> DecodingThreads.DefaultValue Then
                ret += " -threads " + DecodingThreads.Value.ToString
            End If

            If includePaths Then ret += " -i " + sourcePath.Quotes

            Dim items = From i In Me.Items Where i.GetArgs <> ""

            If items.Count > 0 Then ret += " " + items.Select(Function(item) item.GetArgs).Join(" ")

            If Calc.IsARSignalingRequired Then
                ret += " -aspect " + Calc.GetTargetDAR.ToString(CultureInfo.InvariantCulture).Shorten(8)
            End If

            Select Case Mode.Value
                Case EncodingMode.Quality
                    If Codec.OptionText = "VP9" Then
                        ret += " -crf " & Quality.Value & " -b:v 0"
                    Else
                        If Codec.OptionText = "h264_qsv" Then
                            ret += " -q:v " & Quality.Value
                        End If
                    End If
                Case EncodingMode.TwoPass
                    ret += " -pass " & pass
                    ret += " -b:v " & p.VideoBitrate & "k"

                    If Codec.OptionText = "Xvid" OrElse Codec.OptionText = "ASP" Then
                        ret += " -f avi"
                    ElseIf Codec.OptionText = "VP9" Then
                        ret += " -f webm"
                    Else
                        ret += " -f mkv"
                    End If
                Case EncodingMode.OnePass
                    ret += " -b:v " & p.VideoBitrate & "k"
            End Select

            Select Case Codec.OptionText
                Case "Xvid"
                    ret += " -tag:v xvid"
            End Select

            ret += " -an -y -hide_banner"

            If includePaths Then ret += " " + targetPath.Quotes

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Packs.ffmpeg
        End Function
    End Class

    Enum EncodingMode
        Quality
        OnePass
        TwoPass
    End Enum
End Class

<Serializable()>
Class NvidiaEncoder
    Inherits VideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "NVIDIA " + Params.Codec.OptionText
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
        Dim newParams As New EncoderParams
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(store)

        Using f As New CommandLineForm(newParams)
            f.cms.Items.Add(New ActionMenuItem("Check Hardware", Sub() MsgInfo(ProcessHelp.GetStdOut(Packs.NVEncC.GetPath, "--check-hw"))))
            f.cms.Items.Add(New ActionMenuItem("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetStdOut(Packs.NVEncC.GetPath, "--check-features"))))
            f.cms.Items.Add(New ActionMenuItem("Check Environment", Sub() g.ShowCode("Check Environment", ProcessHelp.GetErrorOutput(Packs.NVEncC.GetPath, "--check-environment"))))

            If f.ShowDialog() = DialogResult.OK Then
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides ReadOnly Property OutputFileType() As String
        Get
            Return Params.Codec.ValueText
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Dim cl = Params.GetCommandLine(True, False)

        If cl.Contains(" | ") Then
            Dim batchPath = p.TempDir + p.TargetFile.Base + "_NVEncC.bat"
            File.WriteAllText(batchPath, cl, Encoding.GetEncoding(850))

            Using proc As New Proc
                proc.Init("Encoding using NVEncC")
                proc.SkipStrings = {"%]", " frames: "}
                proc.WriteLine(cl + CrLf2)
                proc.File = "cmd.exe"
                proc.Arguments = "/C call """ + batchPath + """"
                proc.Start()
            End Using
        Else
            Using proc As New Proc
                proc.Init("Encoding using NVEncC")
                proc.SkipStrings = {"%]"}
                proc.File = Packs.NVEncC.GetPath
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

    Class EncoderParams
        Inherits CommandLineParams

        Sub New()
            Title = "NVIDIA Encoding Options"
        End Sub

        Property Decoder As New OptionParam With {
            .Text = "Decoder:",
            .Options = {"AviSynth/VapourSynth", "NVEncC (NVIDIA CUVID)", "QSVEncC (Intel)", "ffmpeg (Intel)", "ffmpeg (DXVA2)"},
            .Values = {"avs", "nv", "qs", "ffqsv", "ffdxva"}}

        Property Mode As New OptionParam With {
            .Text = "Mode:",
            .Options = {"CBR", "VBR", "VBR2", "CQP"}}

        Property Codec As New OptionParam With {
            .Switch = "--codec",
            .Text = "Codec:",
            .Options = {"H.264", "H.265"},
            .Values = {"h264", "h265"}}

        Property Profile As New OptionParam With {
            .Switch = "--profile",
            .Text = "Profile:",
            .ValueIsName = True,
            .Options = {"baseline", "main", "high", "high444"},
            .Value = 2,
            .DefaultValue = 2}

        Property LevelH264 As New OptionParam With {
            .Name = "LevelH264",
            .Switch = "--level",
            .Text = "Level:",
            .Options = {"Unrestricted", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"},
            .Values = {"", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}}

        Property LevelH265 As New OptionParam With {
            .Name = "LevelH265",
            .Switch = "--level",
            .Text = "Level:",
            .Options = {"Unrestricted", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"},
            .Values = {"", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}}

        Property mvPrecision As New OptionParam With {
            .Switch = "--mv-precision",
            .Text = "MV Precision:",
            .Options = {"Q-pel", "half-pel", "full-pel"},
            .Values = {"Q-pel", "half-pel", "full-pel"}}

        Property QPI As New NumParam With {
            .Text = "Constant QP I:",
            .Value = 20,
            .VisibleFunc = Function() "CQP" = Mode.OptionText,
            .MinMaxStep = {0, 51, 1}}

        Property QPP As New NumParam With {
            .Text = "Constant QP P:",
            .Value = 23,
            .VisibleFunc = Function() "CQP" = Mode.OptionText,
            .MinMaxStep = {0, 51, 1}}

        Property QPB As New NumParam With {
            .Text = "Constant QP B:",
            .Value = 25,
            .VisibleFunc = Function() "CQP" = Mode.OptionText,
            .MinMaxStep = {0, 51, 1}}

        Property MaxBitrate As New NumParam With {
            .Switch = "--max-bitrate",
            .Text = "Maximum Bitrate:",
            .Value = 17500,
            .DefaultValue = 17500,
            .MinMaxStep = {0, 1000000, 1}}

        Property GOPLength As New NumParam With {
            .Switch = "--gop-len",
            .Text = "GOP Length (0=auto):",
            .MinMaxStep = {0, 10000, 1}}

        Property BFrames As New NumParam With {
            .Switch = "--bframes",
            .Text = "B Frames:",
            .Value = 3,
            .DefaultValue = 3,
            .MinMaxStep = {0, 16, 1}}

        Property Ref As New NumParam With {
            .Switch = "--ref",
            .Text = "Reference Frames:",
            .Value = 3,
            .DefaultValue = 3,
            .MinMaxStep = {0, 16, 1}}

        Property Lossless As New BoolParam With {
            .Switch = "--lossless",
            .Text = "Lossless",
            .VisibleFunc = Function() Codec.ValueText = "h264",
            .Value = False,
            .DefaultValue = False}

        Property FullRange As New BoolParam With {
            .Switch = "--fullrange",
            .Text = "Full Range",
            .VisibleFunc = Function() Codec.ValueText = "h264",
            .Value = False,
            .DefaultValue = False}

        Property AQ As New BoolParam With {
            .Switch = "--aq",
            .Text = "Adaptive Quantization",
            .Value = False,
            .DefaultValue = False}

        Property Custom As New StringParam With {
            .Text = "Custom Switches:",
            .ArgsFunc = Function() Custom.Value}

        Private ItemsValue As List(Of CommandLineItem)

        Overrides ReadOnly Property Items As List(Of CommandLineItem)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineItem)
                    Add("Basic", Decoder, Mode, Codec, Profile, LevelH264, LevelH265,
                        QPI, QPP, QPB, GOPLength, BFrames, Ref)

                    Add("Advanced", mvPrecision, MaxBitrate, AQ, Lossless, FullRange, Custom)
                End If

                Return ItemsValue
            End Get
        End Property

        Private AddedList As New List(Of String)

        Private Sub Add(path As String, ParamArray items As CommandLineItem())
            For Each i In items
                i.Path = path
                ItemsValue.Add(i)

                If i.GetKey = "" OrElse AddedList.Contains(i.GetKey) Then
                    Throw New Exception
                End If
            Next
        End Sub

        Protected Overrides Sub OnValueChanged(item As CommandLineItem)
            Profile.Visible = Codec.ValueText = "h264"
            Lossless.Visible = Profile.Visible
            LevelH264.Visible = Codec.ValueText = "h264"
            LevelH265.Visible = Codec.ValueText = "h265"
            MyBase.OnValueChanged(item)
        End Sub

        Overrides Function GetCommandLine(includePaths As Boolean,
                                          includeExecutable As Boolean,
                                          Optional pass As Integer = 0) As String
            Dim ret As String
            Dim sourcePath As String
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputFileType)

            If includePaths AndAlso includeExecutable Then
                ret = Packs.NVEncC.GetPath.Quotes
            End If

            Select Case Decoder.ValueText
                Case "avs"
                    sourcePath = p.Script.Path
                Case "nv"
                    sourcePath = p.LastOriginalSourceFile
                Case "qs"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Packs.QSVEncC.GetPath.Quotes, "QSVEncC") + " -o - -c raw" + " -i " + If(includePaths, p.SourceFile.Quotes, "path") + " | " + If(includePaths, Packs.NVEncC.GetPath.Quotes, "NVEncC")
                Case "ffdxva"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Packs.ffmpeg.GetPath.Quotes, "ffmpeg") + " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.SourceFile.Quotes, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | " + If(includePaths, Packs.NVEncC.GetPath.Quotes, "NVEncC")
                Case "ffqsv"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Packs.ffmpeg.GetPath.Quotes, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.SourceFile.Quotes, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | " + If(includePaths, Packs.NVEncC.GetPath.Quotes, "NVEncC")
            End Select

            Dim q = From i In Items Where i.GetArgs <> ""

            If q.Count > 0 Then
                ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")
            End If

            If Not Lossless.Value Then
                Select Case Mode.OptionText
                    Case "CBR"
                        ret += " --cbr " & p.VideoBitrate
                    Case "VBR"
                        ret += " --vbr " & p.VideoBitrate
                    Case "VBR2"
                        ret += " --vbr2 " & p.VideoBitrate
                    Case "CQP"
                        ret += " --cqp " & QPI.Value & ":" & QPP.Value & ":" & QPB.Value
                End Select
            End If

            If CInt(p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 AndAlso
                p.Script.IsFilterActive("Crop") Then

                ret += " --crop " & p.CropLeft & "," & p.CropTop & "," & p.CropRight & "," & p.CropBottom
            End If

            If p.Script.IsFilterActive("Resize", "Hardware Encoder") OrElse
                (Decoder.ValueText <> "avs" AndAlso p.Script.IsFilterActive("Resize")) Then

                ret += " --output-res " & p.TargetWidth & "x" & p.TargetHeight
            ElseIf p.AutoARSignaling Then
                Dim par = Calc.GetTargetPAR
                If par <> New Point(1, 1) Then ret += " --sar " & par.X & ":" & par.Y
            End If

            If sourcePath = "-" Then ret += " --y4m"

            If includePaths Then ret += " -i " + sourcePath.Quotes + " -o " + targetPath.Quotes

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Packs.NVEncC
        End Function
    End Class
End Class

<Serializable()>
Class IntelEncoder
    Inherits VideoEncoder

    Property ParamsStore As New PrimitiveStore

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Return "Intel " + Params.Codec.OptionText
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

    Public Overrides Function GetFrameRate() As Double
        If Params.Deinterlace.OptionText = "Double Framerate" Then
            Return MyBase.GetFrameRate() * 2
        End If

        Return MyBase.GetFrameRate()
    End Function

    Overrides Sub ShowConfigDialog()
        Dim newParams As New EncoderParams
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(store)

        Using f As New CommandLineForm(newParams)
            f.HTMLHelp = Strings.Intel

            f.cms.Items.Add(New ActionMenuItem("Check Environment", Sub() g.ShowCode("Check Environment", ProcessHelp.GetStdOut(Packs.QSVEncC.GetPath, "--check-environment"))))
            f.cms.Items.Add(New ActionMenuItem("Check Hardware", Sub() MsgInfo(ProcessHelp.GetStdOut(Packs.QSVEncC.GetPath, "--check-hw"))))
            f.cms.Items.Add(New ActionMenuItem("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetStdOut(Packs.QSVEncC.GetPath, "--check-features"))))
            f.cms.Items.Add(New ActionMenuItem("Check Library", Sub() MsgInfo(ProcessHelp.GetStdOut(Packs.QSVEncC.GetPath, "--check-lib"))))

            If f.ShowDialog() = DialogResult.OK Then
                Params = newParams
                ParamsStore = store
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides ReadOnly Property OutputFileType() As String
        Get
            If Params.Codec.ValueText = "mpeg2" Then Return "m2v"
            Return Params.Codec.ValueText
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Params.RaiseValueChanged(Nothing)
        Dim cl = Params.GetCommandLine(True, False)

        If cl.Contains(" | ") Then
            Dim batchPath = p.TempDir + p.TargetFile.Base + "_QSVEncC.bat"
            File.WriteAllText(batchPath, cl, Encoding.GetEncoding(850))

            Using proc As New Proc
                proc.Init("Encoding using QSVEncC")
                proc.SkipStrings = {" frames: "}
                proc.WriteLine(cl + CrLf2)
                proc.File = "cmd.exe"
                proc.Arguments = "/C call """ + batchPath + """"
                proc.Start()
            End Using
        Else
            Using proc As New Proc
                proc.Init("Encoding using QSVEncC")
                proc.SkipStrings = {" frames: "}
                proc.File = Packs.QSVEncC.GetPath
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
            Return {"cqp", "vqp", "icq", "la-icq", "qvbr-q"}.Contains(Params.Mode.ValueText)
        End Get
        Set(Value As Boolean)
        End Set
    End Property

    Class EncoderParams
        Inherits CommandLineParams

        Shared Modes As New List(Of StringPair) From {
            New StringPair("avbr", "AVBR - Average Variable Bitrate"),
            New StringPair("cbr", "CBR - Constant Bitrate"),
            New StringPair("cqp", "CQP - Constant QP"),
            New StringPair("icq", "ICQ - Intelligent Constant Quality"),
            New StringPair("la", "LA - VBR Lookahead"),
            New StringPair("la-hrd", "LA-HRD - VBR HRD Lookahead"),
            New StringPair("la-icq", "LA-ICQ - Intelligent Constant Quality Lookahead"),
            New StringPair("qvbr", "QVBR - Quality Variable Bitrate using bitrate"),
            New StringPair("qvbr-q", "QVBR-Q - Quality Variable Bitrate using quality"),
            New StringPair("vbr", "VBR - Variable Bitrate"),
            New StringPair("vcm", "VCM - Video Conferencing Mode"),
            New StringPair("vqp", "VQP - Variable QP")}

        Sub New()
            Title = "Intel Encoding Options"
        End Sub

        Property Decoder As New OptionParam With {
            .Text = "Decoder:",
            .Options = {"AviSynth/VapourSynth", "QSVEncC (Intel)", "ffmpeg (Intel)", "ffmpeg (DXVA2)"},
            .Values = {"avs", "qs", "ffqsv", "ffdxva"}}

        Property Codec As New OptionParam With {
            .Switch = "--codec",
            .Text = "Codec:",
            .Options = {"H.264", "H.265", "MPEG-2"},
            .Values = {"h264", "hevc", "mpeg2"}}

        Property Mode As New OptionParam With {
            .Name = "Mode",
            .Text = "Mode:",
            .Expand = True,
            .Options = Modes.Select(Function(a) a.Value).ToArray,
            .Values = Modes.Select(Function(a) a.Name).ToArray,
            .Value = 2,
            .DefaultValue = 2}

        Property QualitySpeed As New OptionParam With {
            .Switch = "--quality",
            .Text = "Quality/Speed:",
            .Options = {"best", "higher", "high", "balanced", "fast", "faster", "fastest"},
            .Values = {"best", "higher", "high", "balanced", "fast", "faster", "fastest"},
            .Value = 3,
            .DefaultValue = 3}

        Property Deinterlace As New OptionParam With {
            .Switch = "--vpp-deinterlace",
            .Text = "Deinterlace:",
            .Options = {"None", "Normal", "Inverse Telecine", "Double Framerate"},
            .Values = {"none", "normal", "it", "bob"}}

        Property TFF As New BoolParam With {
            .Switch = "--tff",
            .Text = "Top Field First"}

        Property BFF As New BoolParam With {
            .Switch = "--bff",
            .Text = "Bottom Field First"}

        Property Quality As New NumParam With {
            .Text = "Quality:",
            .Value = 23,
            .DefaultValue = 23,
            .VisibleFunc = Function() {"icq", "la-icq", "qvbr-q"}.Contains(Mode.ValueText),
            .MinMaxStep = {0, 51, 1}}

        Property QPI As New NumParam With {
            .Text = "QP I:",
            .Value = 24,
            .DefaultValue = 24,
            .VisibleFunc = Function() {"cqp", "vqp"}.Contains(Mode.ValueText),
            .MinMaxStep = {0, 51, 1}}

        Property QPP As New NumParam With {
            .Text = "QP P:",
            .Value = 26,
            .DefaultValue = 26,
            .VisibleFunc = Function() {"cqp", "vqp"}.Contains(Mode.ValueText),
            .MinMaxStep = {0, 51, 1}}

        Property QPB As New NumParam With {
            .Text = "QP B:",
            .Value = 27,
            .DefaultValue = 27,
            .VisibleFunc = Function() {"cqp", "vqp"}.Contains(Mode.ValueText),
            .MinMaxStep = {0, 51, 1}}

        Property MaxBitrate As New NumParam With {
            .Switch = "--max-bitrate",
            .Text = "Maximum Bitrate:",
            .Value = 17500,
            .DefaultValue = 17500,
            .MinMaxStep = {0, 1000000, 1}}

        Property GOPLength As New NumParam With {
            .Switch = "--gop-len",
            .Text = "GOP Length (0=auto):",
            .MinMaxStep = {0, 10000, 1}}

        Property BFrames As New NumParam With {
            .Switch = "--bframes",
            .Text = "B Frames:",
            .MinMaxStep = {0, 16, 1}}

        Property LookaheadDepth As New NumParam With {
            .Switch = "--la-depth",
            .Text = "Lookahead Depth:",
            .Value = 30,
            .VisibleFunc = Function() {"la", "la-hrd", "la-icq"}.Contains(Mode.ValueText),
            .MinMaxStep = {0, 100, 1}}

        Property Ref As New NumParam With {
            .Switch = "--ref",
            .Text = "Ref Frames (0=auto):",
            .MinMaxStep = {0, 16, 1}}

        Property Scenechange As New BoolParam With {
            .Switch = "--scenechange",
            .NoSwitch = "--no-scenechange",
            .VisibleFunc = Function() Decoder.ValueText = "avs",
            .Text = "Scenechange"}

        Property Fallback As New BoolParam With {
            .Switch = "--fallback-rc",
            .Text = "Enable fallback for unsupported modes",
            .Value = True}

        Property MBBRC As New BoolParam With {
            .Switch = "--mbbrc",
            .NoSwitch = "--no-mbbrc",
            .Text = "Per macro block rate control",
            .VisibleFunc = Function() Not {"cqp", "la", "la-hrd", "la-icq", "vqp", "icq"}.Contains(Mode.ValueText),
            .Value = False,
            .DefaultValue = False}

        Property Custom As New StringParam With {
            .Text = "Custom Switches:",
            .ArgsFunc = Function() Custom.Value}

        Property LevelH264 As New OptionParam With {
            .Switch = "--level",
            .Text = "Level:",
            .ValueIsName = True,
            .VisibleFunc = Function() Codec.Value = 0,
            .Options = {"Automatic", "1", "1b", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3", "3.1", "3.2", "4", "4.1", "4.2", "5", "5.1", "5.2"}}

        Property LevelHEVC As New OptionParam With {
            .Name = "LevelHEVC",
            .Switch = "--level",
            .Text = "Level:",
            .ValueIsName = True,
            .VisibleFunc = Function() Codec.Value = 1,
            .Options = {"Automatic", "1", "2", "2.1", "3", "3.1", "4", "4.1", "5", "5.1", "5.2", "6", "6.1", "6.2"}}

        Property LevelMPEG2 As New OptionParam With {
            .Name = "LevelMPEG2",
            .Switch = "--level",
            .Text = "Level:",
            .ValueIsName = True,
            .VisibleFunc = Function() Codec.Value = 2,
            .Options = {"Automatic", "low", "main", "high", "High1440"}}

        Property ProfileH264 As New OptionParam With {
            .Switch = "--profile",
            .Text = "Profile:",
            .ValueIsName = True,
            .VisibleFunc = Function() Codec.Value = 0,
            .Options = {"Automatic", "Baseline", "Main", "High"}}

        Property ProfileHEVC As New OptionParam With {
            .Name = "ProfileHEVC",
            .Switch = "--profile",
            .Text = "Profile:",
            .ValueIsName = True,
            .VisibleFunc = Function() Codec.Value = 1,
            .Options = {"Main"}}

        Property ProfileMPEG2 As New OptionParam With {
            .Name = "ProfileMPEG2",
            .Switch = "--profile",
            .Text = "Profile:",
            .ValueIsName = True,
            .VisibleFunc = Function() Codec.Value = 2,
            .Options = {"Automatic", "Simple", "Main", "High"}}

        Property Rotate As New OptionParam With {
            .Switch = "--vpp-rotate",
            .Text = "Rotate:",
            .ValueIsName = True,
            .Options = {"0", "90", "180", "270"}}

        Property OutputBuf As New OptionParam With {
            .Switch = "--output-buf",
            .Text = "Output Buffer:",
            .Options = {"8", "16", "32", "64", "128"},
            .ValueIsName = True}

        Private ItemsValue As List(Of CommandLineItem)

        Overrides ReadOnly Property Items As List(Of CommandLineItem)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineItem)

                    Add("Basic", Decoder, Codec, QualitySpeed, Mode, Quality, QPI, QPP, QPB)
                    Add("Advanced", ProfileH264, ProfileHEVC, ProfileMPEG2, LevelHEVC, LevelH264, LevelMPEG2, Rotate, OutputBuf, BFrames, Ref, GOPLength, LookaheadDepth, Scenechange, Fallback, MBBRC, Custom)
                    Add("Deinterlace", Deinterlace, TFF, BFF)
                End If

                Return ItemsValue
            End Get
        End Property

        Private AddedList As New List(Of String)

        Private Sub Add(path As String, ParamArray items As CommandLineItem())
            For Each i In items
                i.Path = path
                ItemsValue.Add(i)

                If i.GetKey = "" OrElse AddedList.Contains(i.GetKey) Then
                    Throw New Exception
                End If
            Next
        End Sub

        Function GetMode(name As String) As Integer
            For x = 0 To Modes.Count - 1
                If Modes(x).Name = name Then Return x
            Next
        End Function

        Protected Overrides Sub OnValueChanged(item As CommandLineItem)
            If item Is Deinterlace Then
                If Deinterlace.ValueText = "normal" OrElse Deinterlace.ValueText = "bob" Then
                    If Not TFF.Value AndAlso Not BFF.Value Then TFF.Value = True
                Else
                    TFF.Value = False
                    BFF.Value = False
                End If
            End If

            If item Is Codec OrElse item Is Nothing Then
                If Codec.ValueText = "hevc" Then
                    BFrames.DefaultValue = 2
                Else
                    BFrames.DefaultValue = 3
                End If
            End If

            If item Is Codec Then
                Mode.Value = 2

                If Codec.ValueText = "hevc" Then
                    BFrames.Value = 2
                Else
                    BFrames.Value = 3
                End If
            End If

            If item Is Codec OrElse item Is Nothing Then
                For Each i In Modes
                    Select Case Codec.ValueText
                        Case "h264"
                            Mode.ShowOption(GetMode(i.Name), True)
                        Case "hevc"
                            Mode.ShowOption(GetMode(i.Name), i.Name.EqualsAny("cbr", "vbr", "cqp", "vqp", "icq", "vcm"))
                        Case "mpeg2"
                            Mode.ShowOption(GetMode(i.Name), i.Name.EqualsAny("cbr", "vbr", "avbr", "cqp", "vqp"))
                    End Select
                Next
            End If

            MyBase.OnValueChanged(item)
        End Sub

        Overrides Function GetCommandLine(includePaths As Boolean,
                                          includeExecutable As Boolean,
                                          Optional pass As Integer = 0) As String
            Dim ret As String
            Dim sourcePath = p.Script.Path
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputFileType)

            If includePaths AndAlso includeExecutable Then ret = Packs.QSVEncC.GetPath.Quotes

            Select Case Decoder.ValueText
                Case "avs"
                    sourcePath = p.Script.Path
                Case "qs"
                    sourcePath = p.LastOriginalSourceFile
                Case "ffdxva"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Packs.ffmpeg.GetPath.Quotes, "ffmpeg") + " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.LastOriginalSourceFile.Quotes, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | " + If(includePaths, Packs.QSVEncC.GetPath.Quotes, "QSVEncC")
                Case "ffqsv"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Packs.ffmpeg.GetPath.Quotes, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.LastOriginalSourceFile.Quotes, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | " + If(includePaths, Packs.QSVEncC.GetPath.Quotes, "QSVEncC")

            End Select

            Dim q = From i In Items Where i.GetArgs <> ""

            If q.Count > 0 Then
                ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")
            End If

            Select Case Mode.ValueText
                Case "icq", "la-icq"
                    ret += " --" + Mode.ValueText + " " & CInt(Quality.Value)
                Case "qvbr-q"
                    ret += " --qvbr-q " & CInt(Quality.Value) & " --qvbr " & p.VideoBitrate
                Case "cqp", "vqp"
                    ret += " --" + Mode.ValueText + " " & CInt(QPI.Value) & ":" & CInt(QPP.Value) & ":" & CInt(QPB.Value)
                Case Else
                    ret += " --" + Mode.ValueText + " " & p.VideoBitrate
            End Select

            If p.Script.IsFilterActive("Resize", "Hardware Encoder") OrElse
                (Decoder.ValueText <> "avs" AndAlso p.Script.IsFilterActive("Resize")) Then

                ret += " --output-res " & p.TargetWidth & "x" & p.TargetHeight
            ElseIf p.AutoARSignaling Then
                Dim par = Calc.GetTargetPAR
                If par <> New Point(1, 1) Then ret += " --sar " & par.X & ":" & par.Y
            End If

            If CInt(p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 AndAlso
                (p.Script.IsFilterActive("Crop", "Hardware Encoder") OrElse
                (Decoder.ValueText <> "avs" AndAlso p.Script.IsFilterActive("Crop"))) Then

                ret += " --crop " & p.CropLeft & "," & p.CropTop & "," & p.CropRight & "," & p.CropBottom
            End If

            If Decoder.ValueText <> "avs" Then
                If p.Ranges.Count > 0 Then
                    ret += " --trim " + p.Ranges.Select(Function(range) range.Start & ":" & range.End).Join(",")
                End If
            End If

            If sourcePath = "-" Then ret += " --y4m"

            If includePaths Then ret += " -i " + sourcePath.Quotes + " -o " + targetPath.Quotes

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Packs.QSVEncC
        End Function
    End Class
End Class

<Serializable()>
Class AMDEncoder
    Inherits VideoEncoder

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
        Dim newParams As New EncoderParams
        Dim store = DirectCast(ObjectHelp.GetCopy(ParamsStore), PrimitiveStore)
        newParams.Init(store)

        Using f As New CommandLineForm(newParams)
            f.cms.Items.Add(New ActionMenuItem("Check VCE Support", Sub() MsgInfo(ProcessHelp.GetStdOut(Packs.NVEncC.GetPath, "--check-vce"))))

            If f.ShowDialog() = DialogResult.OK Then
                Params = newParams
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
                proc.Init("Encoding using VCEEncC")
                proc.SkipStrings = {"%]", " frames: "}
                proc.WriteLine(cl + CrLf2)
                proc.File = "cmd.exe"
                proc.Arguments = "/C call """ + batchPath + """"
                proc.Start()
            End Using
        Else
            Using proc As New Proc
                proc.Init("Encoding using VCEEncC")
                proc.SkipStrings = {"%]"}
                proc.File = Packs.VCEEncC.GetPath
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
            .ValueIsName = True,
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
            .Text = "Maximum Bitrate:",
            .Value = 20000,
            .DefaultValue = 20000,
            .MinMaxStep = {0, 1000000, 1}}

        Property GOPLength As New NumParam With {
            .Switch = "--gop-len",
            .Text = "GOP Length (0=auto):",
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

        Private ItemsValue As List(Of CommandLineItem)

        Overrides ReadOnly Property Items As List(Of CommandLineItem)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineItem)
                    Add("Basic", Decoder, Mode, Quality, QPI, QPP, QPB, GOPLength, BFrames)
                    Add("Advanced", MaxBitrate, VBVBufsize, QPMin, QPMax, Custom)
                End If

                Return ItemsValue
            End Get
        End Property

        Private AddedList As New List(Of String)

        Private Sub Add(path As String, ParamArray items As CommandLineItem())
            For Each i In items
                i.Path = path
                ItemsValue.Add(i)

                If i.GetKey = "" OrElse AddedList.Contains(i.GetKey) Then
                    Throw New Exception
                End If
            Next
        End Sub

        Protected Overrides Sub OnValueChanged(item As CommandLineItem)
            MyBase.OnValueChanged(item)
        End Sub

        Overrides Function GetCommandLine(includePaths As Boolean,
                                          includeExecutable As Boolean,
                                          Optional pass As Integer = 0) As String
            Dim ret As String
            Dim sourcePath As String
            Dim targetPath = p.VideoEncoder.OutputPath.ChangeExt(p.VideoEncoder.OutputFileType)

            If includePaths AndAlso includeExecutable Then
                ret = Packs.VCEEncC.GetPath.Quotes
            End If

            Select Case Decoder.ValueText
                Case "avs"
                    sourcePath = p.Script.Path
                Case "qs"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Packs.QSVEncC.GetPath.Quotes, "QSVEncC") + " -o - -c raw" + " -i " + If(includePaths, p.SourceFile.Quotes, "path") + " | " + If(includePaths, Packs.VCEEncC.GetPath.Quotes, "VCEEncC")
                Case "ffdxva"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Packs.ffmpeg.GetPath.Quotes, "ffmpeg") + " -threads 1 -hwaccel dxva2 -i " + If(includePaths, p.SourceFile.Quotes, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | " + If(includePaths, Packs.VCEEncC.GetPath.Quotes, "VCEEncC")
                Case "ffqsv"
                    sourcePath = "-"
                    If includePaths Then ret = If(includePaths, Packs.ffmpeg.GetPath.Quotes, "ffmpeg") + " -threads 1 -hwaccel qsv -i " + If(includePaths, p.SourceFile.Quotes, "path") + " -f yuv4mpegpipe -pix_fmt yuv420p -loglevel error - | " + If(includePaths, Packs.VCEEncC.GetPath.Quotes, "VCEEncC")
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
            Return Packs.VCEEncC
        End Function
    End Class
End Class