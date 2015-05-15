Imports Microsoft.Win32

Imports System.Globalization
Imports System.Runtime.Serialization
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Text

Imports StaxRip.UI
Imports StaxRip.CommandLine
Imports System.Collections.Specialized

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

    Sub New(name As String)
        MyBase.New(name)
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
        Dim r As New ToolStripEx

        r.ShowItemToolTips = False
        r.GripStyle = ToolStripGripStyle.Hidden
        r.BackColor = System.Drawing.SystemColors.Window
        r.Dock = DockStyle.Fill
        r.BackColor = System.Drawing.SystemColors.Window
        r.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        r.Padding = New Padding(1, 1, 0, 0)

        For Each i In GetMenu()
            Dim b As New ToolStripButton
            b.Margin = New Padding(0)
            b.Text = i.Key
            b.Padding = New Padding(4)
            Dim happy = i
            AddHandler b.Click, Sub() happy.Value.Invoke()
            b.TextAlign = ContentAlignment.MiddleLeft
            r.Items.Add(b)
        Next

        Return r
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
            p.AvsDoc.IsFilterActive("Resize") Then

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
        Return p.AvsDoc.GetFramerate
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
        g.MainForm.tbTargetFile.Text = Filepath.GetChangeExt(p.TargetFile, Muxer.GetExtension)
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

        If sb.Show = DialogResult.OK Then
            Return sb.SelectedItem
        End If

        Return Nothing
    End Function

    Shared Function Getx264Encoder(
        name As String,
        device As x264DeviceMode,
        Optional mode As x264Mode = x264Mode.SingleCRF) As x264Encoder

        Return Getx264Encoder(name, mode, x264PresetMode.Medium, x264TuneMode.Disabled, device, 22)
    End Function

    Shared Function Getx264Encoder(name As String,
                               mode As x264Mode,
                               preset As x264PresetMode,
                               tuning As x264TuneMode,
                               device As x264DeviceMode,
                               quant As Integer) As x264Encoder

        Dim r As New x264Encoder()
        r.Name = name
        r.Params.Mode.Value = mode
        r.Params.Preset.Value = preset
        r.Params.Tune.Value = tuning
        r.Params.Device.Value = device
        r.Params.Quant.Value = quant
        r.Params.ApplyDeviceSettings()
        r.Params.ApplyDefaults(r.Params)
        r.Params.ApplyDeviceSettings()
        r.SetMuxer()

        Return r
    End Function

    Shared Function GetDefaults() As List(Of VideoEncoder)
        Dim ret As New List(Of VideoEncoder)

        Dim x265 = New x265.x265Encoder
        x265.Params.ApplyPresetDefaultValues()
        x265.Params.ApplyPresetValues()
        ret.Add(x265)

        ret.Add(Getx264Encoder("x264", x264Mode.SingleCRF, x264PresetMode.Medium, x264TuneMode.Disabled, x264DeviceMode.Disabled, 22))

        Dim nv265 As New NvidiaEncoder("NVIDIA H.265")
        nv265.Params.Mode.Value = 2
        nv265.Params.Codec.Value = 1
        ret.Add(nv265)

        Dim nv264 As New NvidiaEncoder("NVIDIA H.264")
        nv264.Params.Mode.Value = 2
        ret.Add(nv264)

        Dim quickSync As New IntelEncoder("Intel H.264")
        ret.Add(quickSync)

        Dim vp9ffmpeg = New ffmpegEncoder()
        vp9ffmpeg.Name = "VP9"
        ret.Add(vp9ffmpeg)

        Dim xvid As New BatchEncoder()
        xvid.OutputFileTypeValue = "avi"
        xvid.Name = "XviD Quality Mode"
        xvid.Muxer = New ffmpegMuxer("AVI")
        xvid.QualityMode = True
        xvid.CommandLines = """%app:xvid_encraw%"" -cq 2 -smoother 0 -max_key_interval 250 -nopacked -vhqmode 4 -qpel -lumimasking -notrellis -max_bframes 1 -bvhq -bquant_ratio 162 -bquant_offset 0 -threads 1 -i ""%avs_file%"" -avi ""%encoder_out_file%"" -par %target_sar%"
        ret.Add(xvid)

        Dim xvid2pass As New BatchEncoder()
        xvid2pass.OutputFileTypeValue = "avi"
        xvid2pass.Name = "XviD 2 pass"
        xvid2pass.Muxer = New ffmpegMuxer("AVI")
        xvid2pass.CommandLines = """%app:xvid_encraw%"" -smoother 0 -max_key_interval 250 -nopacked -vhqmode 4 -qpel -lumimasking -notrellis -max_bframes 1 -bvhq -bquant_ratio 162 -bquant_offset 0 -threads 1 -i ""%avs_file%"" -pass1 ""%temp_file%.stats"" -bitrate %video_bitrate% -par %target_sar% -turbo" + CrLf +
                                 """%app:xvid_encraw%"" -smoother 0 -max_key_interval 250 -nopacked -vhqmode 4 -qpel -lumimasking -notrellis -max_bframes 1 -bvhq -bquant_ratio 162 -bquant_offset 0 -threads 1 -i ""%avs_file%"" -pass2 ""%temp_file%.stats"" -bitrate %video_bitrate% -par %target_sar% -avi ""%encoder_out_file%"""
        ret.Add(xvid2pass)

        ret.Add(New NullEncoder("Just Mux"))

        Dim x264cli As New BatchEncoder()
        x264cli.OutputFileTypeValue = "h264"
        x264cli.Name = "Command Line | x264"
        x264cli.Muxer = New MkvMuxer()
        x264cli.AutoCompCheckValue = 50
        x264cli.CommandLines = """%app:x264%"" --pass 1 --bitrate %video_bitrate% --stats ""%temp_file%.stats"" --output NUL ""%avs_file%""" + CrLf + """%app:x264%"" --pass 2 --bitrate %video_bitrate% --stats ""%temp_file%.stats"" --output ""%encoder_out_file%"" ""%avs_file%"""
        x264cli.CompCheckCommandLines = """%app:x264%"" --crf 18 --output ""%temp_file%_CompCheck%encoder_ext%"" ""%temp_file%_CompCheck.avs"""
        ret.Add(x264cli)

        Dim divx As New BatchEncoder()
        divx.OutputFileTypeValue = "h265"
        divx.Name = "Command Line | DivX H.265"
        divx.Muxer = New MkvMuxer()
        divx.CommandLines = """%app:DivX265%"" -i ""%avs_file%"" -o ""%encoder_out_file%"" -br %video_bitrate% -10"
        ret.Add(divx)

        Dim nvencH265 As New BatchEncoder()
        nvencH265.OutputFileTypeValue = "h265"
        nvencH265.Name = "Command Line | NVIDIA H.265"
        nvencH265.Muxer = New MkvMuxer()
        nvencH265.QualityMode = True
        nvencH265.CommandLines = """%app:ffmpeg%"" -i ""%avs_file%"" -f yuv4mpegpipe -pix_fmt yuv420p - | ""%app:NVEncC%"" --sar %target_sar% --codec h265 --y4m --cqp 36 --input - --output ""%encoder_out_file%"""
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

    Public Class MenuList
        Inherits List(Of KeyValuePair(Of String, Action))

        Overloads Sub Add(text As String, action As action)
            Add(New KeyValuePair(Of String, Action)(text, action))
        End Sub
    End Class
End Class

<Serializable()>
Public Class BatchEncoder
    Inherits VideoEncoder

    Sub New()
        MyBase.New("Command Line")
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

    Overrides Sub Encode()
        Dim commands = Macro.Solve(CommandLines).Trim

        Dim skipStrings As String()

        If commands.Contains("DivX265") Then
            skipStrings = {"encoded @"}
        ElseIf commands.Contains("xvid_encraw") Then
            skipStrings = {"key="}
        ElseIf commands.Contains("x264") Then
            skipStrings = {"frames,"}
        ElseIf commands.Contains("NVEncC") Then
            skipStrings = {"frame= ", "frames: "}
        Else
            skipStrings = {" [ETA ", ", eta ", "frames: ", "frame= "}
        End If

        If commands.Contains("|") OrElse commands.Contains(CrLf) Then
            Dim batchPath = p.TempDir + Filepath.GetBase(p.TargetFile) + "_encode.bat"
            File.WriteAllText(batchPath, commands, Encoding.GetEncoding(850))

            Using proc As New Proc
                proc.Init("Encoding video command line encoder: " + Name)
                proc.SkipStrings = skipStrings
                proc.WriteLine(commands + CrLf2)
                proc.File = "cmd.exe"
                proc.Arguments = "/C call """ + batchPath + """"
                proc.BatchCode = commands

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
                proc.SkipStrings = skipStrings
                proc.CommandLine = commands
                proc.Start()
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

        Dim doc As New AviSynthDocument
        doc.Filters = p.AvsDoc.GetFiltersCopy
        Dim code = "SelectRangeEvery(" + ((100 \ p.CompCheckRange) * 14).ToString + ",14)"
        Log.WriteLine(code + CrLf2)
        doc.Filters.Add(New AviSynthFilter("SelectRangeEvery", "SelectRangeEvery", code, True))
        doc.Path = p.TempDir + p.Name + "_CompCheck.avs"
        doc.Synchronize()

        Dim batchPath = p.TempDir + Filepath.GetBase(p.TargetFile) + "_CompCheck.bat"
        Dim command = Macro.Solve(CompCheckCommandLines)

        File.WriteAllText(batchPath, command, Encoding.GetEncoding(850))
        Log.WriteLine(command + CrLf2)

        Using proc As New Proc
            proc.Init(Nothing)
            proc.File = "cmd.exe"
            proc.Arguments = "/C call """ + batchPath + """"

            Try
                proc.Start()
            Catch ex As Exception
                ProcessForm.CloseProcessForm()
                g.ShowException(ex)
                Throw New AbortException
            End Try
        End Using

        Dim bits = (New FileInfo(p.TempDir + p.Name + "_CompCheck." + OutputfileType).Length) * 8
        p.Compressibility = (bits / doc.GetFrames) / (p.TargetWidth * p.TargetHeight)

        OnAfterCompCheck()

        g.MainForm.Assistant()

        Log.WriteLine(CInt(Calc.GetPercent).ToString() + " %")

        ProcessForm.CloseProcessForm()
    End Sub
End Class

<Serializable()>
Public Class NullEncoder
    Inherits VideoEncoder

    Sub New(name As String)
        MyBase.New(name)
        Muxer = New MkvMuxer()
        QualityMode = True
    End Sub

    Function GetSource() As String
        For Each i In {".h264", ".avc", ".h265", ".hevc", ".mpg", ".avi"}
            If File.Exists(Filepath.GetDirAndBase(p.SourceFile) + "_out" + i) Then
                Return Filepath.GetDirAndBase(p.SourceFile) + "_out" + i
            ElseIf File.Exists(p.TempDir + p.Name + "_out" + i)
                Return p.TempDir + p.Name + "_out" + i
            End If
        Next

        If FileTypes.VideoIndex.Contains(Filepath.GetExtNoDot(p.SourceFile)) Then
            Return p.OriginalSourceFile
        Else
            Return p.SourceFile
        End If
    End Function

    Overrides ReadOnly Property OutputPath As String
        Get
            Dim source = GetSource()

            If Not p.VideoEncoder.Muxer.IsSupported(Filepath.GetExtNoDot(source)) Then
                Select Case Filepath.GetExtNoDot(source)
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
            Return Filepath.GetExtNoDot(OutputPath)
        End Get
    End Property

    Overrides Sub Encode()
        Dim source = GetSource()

        If Not p.VideoEncoder.Muxer.IsSupported(Filepath.GetExtNoDot(source)) Then
            Select Case Filepath.GetExtNoDot(source)
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

    Sub New()
        MyBase.New("ffmpeg")
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
                Case "VP9"
                    Return "webm"
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
        p.AvsDoc.Synchronize()

        Params.RaiseValueChanged(Nothing)

        If Params.Mode.OptionText = "Two Pass" Then
            Encode(Params.GetArgs(1, p.AvsDoc.Path, "NUL", True))
            Encode(Params.GetArgs(2, p.AvsDoc.Path, Filepath.GetDirAndBase(OutputPath) + "." + OutputFileType, True))
        Else
            Encode(Params.GetArgs(1, p.AvsDoc.Path, Filepath.GetDirAndBase(OutputPath) + "." + OutputFileType, True))
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(args As String)
        Using proc As New Proc
            proc.Init("Encoding " + Params.Codec.OptionText + " using ffmpeg", "frame=")
            proc.Encoding = Encoding.UTF8
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

        Sub New()
            Title = "ffmpeg Encoding Options"
        End Sub

        Property Codec As New OptionParam With {
            .Switch = "-c:v",
            .Text = "Codec:",
            .AlwaysOn = True,
            .Options = {"VP9", "Xvid", "ASP", "Theora"},
            .Values = {"libvpx-vp9", "libxvid", "mpeg4", "libtheora"}}

        Property Mode As New OptionParam With {
            .Name = "Mode",
            .Text = "Mode:",
            .Options = {"Quality", "One Pass", "Two Pass"}}

        Property Speed As New OptionParam With {
            .Switch = "-speed",
            .Text = "Speed:",
            .AlwaysOn = True,
            .Options = {"6 - Fastest", "5 - Faster", "4 - Fast", "3 - Medium", "2 - Slow", "1 - Slower", "0 - Slowest"},
            .Values = {"6", "5", "4", "3", "2", "1", "0"},
            .Value = 5}

        Property AQmode As New OptionParam With {
            .Switch = "-aq-mode",
            .Text = "AQ Mode:",
            .Options = {"Disabled", "0", "1", "2", "3"},
            .Values = {"Disabled", "0", "1", "2", "3"}}

        Property Quality As New NumParam With {
            .Name = "Quality",
            .Text = "Quality:",
            .MinMaxStep = {1, 63, 1},
            .Value = 25}

        Property Threads As New NumParam With {
            .Switch = "-threads",
            .Text = "Threads:",
            .Value = 8,
            .DefaultValue = -1}

        Property TileColumns As New NumParam With {
            .Switch = "-tile-columns",
            .Text = "Tile Columns:",
            .Value = 6,
            .DefaultValue = -1}

        Property FrameParallel As New NumParam With {
            .Switch = "-frame-parallel",
            .Text = "Frame Parallel:",
            .Value = 1,
            .DefaultValue = -1}

        Property AutoAltRef As New NumParam With {
            .Switch = "-auto-alt-ref",
            .Text = "Auto Alt Ref:",
            .Value = 1,
            .DefaultValue = -1}

        Property LagInFrames As New NumParam With {
            .Switch = "-lag-in-frames",
            .Text = "Lag In Frames:",
            .Value = 25,
            .DefaultValue = -1}

        Property Custom As New StringParam With {
            .Text = "Custom Switches:"}

        Private ItemsValue As List(Of CommandLineItem)

        Overrides ReadOnly Property Items As List(Of CommandLineItem)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineItem)

                    ItemsValue.AddRange({Codec, Mode, Speed, AQmode, Quality, Threads, TileColumns, FrameParallel, AutoAltRef, LagInFrames, Custom})
                End If

                Return ItemsValue
            End Get
        End Property

        Protected Overrides Sub OnValueChanged(item As CommandLineItem)
            Speed.Visible = Codec.OptionText = "VP9"
            Threads.Visible = Codec.OptionText = "VP9"
            TileColumns.Visible = Codec.OptionText = "VP9"
            FrameParallel.Visible = Codec.OptionText = "VP9"
            AutoAltRef.Visible = Codec.OptionText = "VP9"
            LagInFrames.Visible = Codec.OptionText = "VP9"
            AQmode.Visible = Codec.OptionText = "VP9"

            Quality.Visible = Mode.Value = EncodingMode.Quality
            MyBase.OnValueChanged(item)
        End Sub

        Overloads Overrides Function GetArgs(includePaths As Boolean) As String
            Return GetArgs(1, p.AvsDoc.Path, Filepath.GetDirAndBase(p.VideoEncoder.OutputPath) +
                           "." + p.VideoEncoder.OutputFileType, includePaths)
        End Function

        Overloads Function GetArgs(pass As Integer,
                                   sourcePath As String,
                                   targetPath As String,
                                   Optional includePaths As Boolean = True) As String
            Dim ret As String

            If includePaths Then
                ret += "-i """ + p.AvsDoc.Path + """"
            End If

            Dim q = From i In Items Where i.GetArgs <> ""

            If q.Count > 0 Then
                ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")
            End If

            If Calc.IsARSignalingRequired Then
                ret += " -aspect " + Calc.GetTargetDAR.ToString(CultureInfo.InvariantCulture).Shorten(8)
            End If

            Select Case Mode.Value
                Case EncodingMode.Quality
                    If Codec.OptionText = "VP9" Then
                        ret += " -crf " & Quality.Value & " -b:v 0"
                    Else
                        ret += " -q:v " & Quality.Value
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

            If Custom.Value <> "" Then ret += " " + Custom.Value

            ret += " -y"

            If includePaths Then
                ret += " """ + targetPath + """"
            End If

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

    Sub New(profileName As String)
        MyBase.New(profileName)
    End Sub

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
            f.cms.Items.Add(New ActionMenuItem("Check Hardware", Sub() MsgInfo(ProcessHelp.GetStandardOutput(Packs.NVEncC.GetPath, "--check-hw"))))
            f.cms.Items.Add(New ActionMenuItem("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetStandardOutput(Packs.NVEncC.GetPath, "--check-features"))))
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
        p.AvsDoc.Synchronize()
        Encode(Params.GetArgs(1, p.AvsDoc.Path, Filepath.GetDirAndBase(OutputPath) + "." + OutputFileType, True))
        AfterEncoding()
    End Sub

    Overloads Sub Encode(args As String)
        Using proc As New Proc
            proc.Init("Encoding using NVEncC")
            proc.SkipStrings = {"%]"}
            proc.File = Packs.NVEncC.GetPath
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

        Property Codec As New OptionParam With {
            .Switch = "--codec",
            .Text = "Codec:",
            .Options = {"H264/AVC", "H265/HEVC"},
            .Values = {"h264", "h265"},
            .DefaultValue = -1}

        Property Mode As New OptionParam With {
            .Name = "Mode",
            .Text = "Mode:",
            .Options = {"CBR", "VBR", "CQP"}}

        Property Profile As New OptionParam With {
            .Switch = "--profile",
            .Name = "Profile",
            .Text = "Profile:",
            .Options = {"Baseline", "Main", "High"},
            .Values = {"baseline", "main", "high"},
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
            .MinMaxStep = {0, 51, 1}}

        Property QPP As New NumParam With {
            .Text = "Constant QP P:",
            .Value = 23,
            .MinMaxStep = {0, 51, 1}}

        Property QPB As New NumParam With {
            .Text = "Constant QP B:",
            .Value = 25,
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

        Property Custom As New StringParam With {
            .Text = "Custom Switches:",
            .ArgsFunc = Function() Custom.Value}

        Private ItemsValue As List(Of CommandLineItem)

        Overrides ReadOnly Property Items As List(Of CommandLineItem)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineItem)
                    ItemsValue.AddRange({Codec, Mode, Profile, LevelH264, LevelH265, mvPrecision,
                                         QPI, QPP, QPB, MaxBitrate, GOPLength, BFrames, Ref, Custom})
                End If

                Return ItemsValue
            End Get
        End Property

        Protected Overrides Sub OnValueChanged(item As CommandLineItem)
            Profile.Visible = Codec.ValueText = "h264"
            LevelH264.Visible = Codec.ValueText = "h264"
            LevelH265.Visible = Codec.ValueText = "h265"
            MyBase.OnValueChanged(item)
        End Sub

        Overloads Overrides Function GetArgs(includePaths As Boolean) As String
            Return GetArgs(1, p.AvsDoc.Path, Filepath.GetDirAndBase(p.VideoEncoder.OutputPath) +
                           "." + p.VideoEncoder.OutputFileType, includePaths)
        End Function

        Overloads Function GetArgs(pass As Integer,
                                   sourcePath As String,
                                   targetPath As String,
                                   Optional includePaths As Boolean = True) As String
            Dim ret As String

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

            If p.AutoARSignaling Then
                Dim par = Calc.GetTargetPAR
                If par <> New Point(1, 1) Then ret += " --sar " & par.X & ":" & par.Y
            End If

            If includePaths Then
                ret += " --input """ + sourcePath + """ --output """ + targetPath + """"
            End If

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

    Sub New(profileName As String)
        MyBase.New(profileName)
    End Sub

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

            f.cms.Items.Add(New ActionMenuItem("Check Environment", Sub() g.ShowCode("Check Environment", ProcessHelp.GetStandardOutput(Packs.QSVEncC.GetPath, "--check-environment"))))
            f.cms.Items.Add(New ActionMenuItem("Check Hardware", Sub() MsgInfo(ProcessHelp.GetStandardOutput(Packs.QSVEncC.GetPath, "--check-hw"))))
            f.cms.Items.Add(New ActionMenuItem("Check Features", Sub() g.ShowCode("Check Features", ProcessHelp.GetStandardOutput(Packs.QSVEncC.GetPath, "--check-features"))))
            f.cms.Items.Add(New ActionMenuItem("Check Library", Sub() MsgInfo(ProcessHelp.GetStandardOutput(Packs.QSVEncC.GetPath, "--check-lib"))))

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
        p.AvsDoc.Synchronize()
        Params.RaiseValueChanged(Nothing)
        Encode(Params.GetArgs(1, p.AvsDoc.Path, Filepath.GetDirAndBase(OutputPath) + "." + OutputFileType, True))
        AfterEncoding()
    End Sub

    Overloads Sub Encode(args As String)
        Using proc As New Proc
            proc.Init("Encoding using QSVEncC")
            proc.SkipStrings = {"%]", "frames:"}
            proc.File = Packs.QSVEncC.GetPath
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

        '.Options = {"None", "Normal", "Inverse Telecine", "Inverse Telecine 32", "Inverse Telecine 2332", "Inverse Telecine repeat", "Inverse Telecine 41", "Double Framerate", "Automatic", "Automatic Double Framerate"},
        '.Values = {"none", "normal", "it", "it-manual ""32""", "it-manual ""2332""", "it-manual ""repeat""", "it-manual ""41""", "bob", "auto", "auto-bob"}}

        Property Deinterlace As New OptionParam With {
            .Switch = "--vpp-deinterlace",
            .Text = "Deinterlace:",
            .Options = {"None", "Normal", "Inverse Telecine", "Double Framerate"},
            .Values = {"none", "normal", "it", "bob"}}

        Property BFF As New BoolParam With {
            .NoSwitch = "--bff",
            .Text = "Bottom Field First"}

        Property Quality As New NumParam With {
            .Text = "Quality:",
            .Value = 23,
            .DefaultValue = 23,
            .MinMaxStep = {0, 51, 1}}

        Property QPI As New NumParam With {
            .Text = "QP I:",
            .Value = 24,
            .DefaultValue = 24,
            .MinMaxStep = {0, 51, 1}}

        Property QPP As New NumParam With {
            .Text = "QP P:",
            .Value = 26,
            .DefaultValue = 26,
            .MinMaxStep = {0, 51, 1}}

        Property QPB As New NumParam With {
            .Text = "QP B:",
            .Value = 27,
            .DefaultValue = 27,
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

        Property LookaheadDepth As New NumParam With {
            .Switch = "--la-depth",
            .Text = "Lookahead Depth:",
            .Value = 30,
            .MinMaxStep = {0, 100, 1}}

        Property Ref As New NumParam With {
            .Switch = "--ref",
            .Text = "Ref Frames (0=auto):",
            .MinMaxStep = {0, 16, 1}}

        Property Resize As New BoolParam With {
            .ArgsFunc = Function() If(Resize.Value, "--output-res " & p.TargetWidth & "x" & p.TargetHeight, Nothing),
            .Text = "Resize"}

        Property Scenechange As New BoolParam With {
            .NoSwitch = "--no-scenechange",
            .Text = "Scenechange",
            .Value = True,
            .DefaultValue = True}

        Property MBBRC As New BoolParam With {
            .Switch = "--mbbrc",
            .NoSwitch = "--no-mbbrc",
            .Text = "Per macro block rate control",
            .Value = False,
            .DefaultValue = False}

        Property Custom As New StringParam With {
            .Text = "Custom Switches:",
            .ArgsFunc = Function() Custom.Value}

        Property HardwareDecoding As New BoolParam With {
            .Switch = "--avqsv",
            .Text = "Use hardware decoding (bypasses AviSynth)",
            .Help = "Uses the source file as input for QSVEncC as well as StaxRip's cut/trim and crop values.",
            .Value = False,
            .DefaultValue = False}

        Private ItemsValue As List(Of CommandLineItem)

        Overrides ReadOnly Property Items As List(Of CommandLineItem)
            Get
                If ItemsValue Is Nothing Then
                    ItemsValue = New List(Of CommandLineItem)
                    ItemsValue.AddRange({Mode, QualitySpeed, Deinterlace, BFF, Quality, QPI, QPP, QPB, BFrames, Ref, GOPLength, LookaheadDepth, HardwareDecoding, Resize, Scenechange, MBBRC, Custom})
                End If

                Return ItemsValue
            End Get
        End Property

        Protected Overrides Sub OnValueChanged(item As CommandLineItem)
            Quality.Visible = {"icq", "la-icq", "qvbr-q"}.Contains(Mode.ValueText)
            QPB.Visible = {"cqp", "vqp"}.Contains(Mode.ValueText)
            QPI.Visible = {"cqp", "vqp"}.Contains(Mode.ValueText)
            QPP.Visible = {"cqp", "vqp"}.Contains(Mode.ValueText)
            LookaheadDepth.Visible = {"la", "la-hrd", "la-icq"}.Contains(Mode.ValueText)
            MBBRC.Visible = Not {"cqp", "la", "la-hrd", "la-icq", "vqp", "icq"}.Contains(Mode.ValueText)
            BFF.Visible = Deinterlace.Value > 0

            If p.AvsDoc.IsFilterActive("Resize", "Hardware Encoder") Then
                Resize.Value = True
            End If

            MyBase.OnValueChanged(item)
        End Sub

        Overloads Overrides Function GetArgs(includePaths As Boolean) As String
            Return GetArgs(1, p.AvsDoc.Path, Filepath.GetDirAndBase(p.VideoEncoder.OutputPath) +
                           "." + p.VideoEncoder.OutputFileType, includePaths)
        End Function

        Overloads Function GetArgs(pass As Integer,
                                   sourcePath As String,
                                   targetPath As String,
                                   Optional includePaths As Boolean = True) As String
            Dim ret As String

            Dim q = From i In Items Where i.GetArgs <> ""

            If q.Count > 0 Then
                ret += " " + q.Select(Function(item) item.GetArgs).Join(" ")
            End If

            If Deinterlace.Value > 0 Then If BFF.Value Then ret += " --bff" Else ret += " --tff"

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

            If p.AutoARSignaling Then
                Dim par = Calc.GetTargetPAR
                If par <> New Point(1, 1) Then ret += " --sar " & par.X & ":" & par.Y
            End If

            If p.Ranges.Count > 0 Then
                ret += " --trim " + p.Ranges.Select(Function(range) range.Start & ":" & range.End).Join(",")
            End If

            If CInt(p.CropLeft Or p.CropTop Or p.CropRight Or p.CropBottom) <> 0 Then
                ret += " --crop " & p.CropLeft & "," & p.CropTop & "," & p.CropRight & "," & p.CropBottom
            End If

            If HardwareDecoding.Value Then
                If FileTypes.VideoText.Contains(Filepath.GetExtNoDot(p.SourceFile)) Then
                    sourcePath = p.OriginalSourceFile
                Else
                    sourcePath = p.SourceFile
                End If
            End If

            If includePaths Then ret += " --input-file """ + sourcePath + """ --output-file """ + targetPath + """"

            Return ret.Trim
        End Function

        Public Overrides Function GetPackage() As Package
            Return Packs.QSVEncC
        End Function
    End Class
End Class