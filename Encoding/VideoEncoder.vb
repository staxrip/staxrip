
Imports System.Text.RegularExpressions

Imports StaxRip.UI
Imports StaxRip.CommandLine

<Serializable()>
Public MustInherit Class VideoEncoder
    Inherits Profile
    Implements IComparable(Of VideoEncoder)

    MustOverride Sub Encode()

    MustOverride ReadOnly Property OutputExt As String

    Overridable Property Passes As Integer
    Overridable Property QualityMode As Boolean

    Property AutoCompCheckValue As Integer = 50
    Property Muxer As Muxer = New MkvMuxer

    Public MustOverride Sub ShowConfigDialog()

    Sub New()
        CanEditValue = True
    End Sub

    ReadOnly Property OutputExtFull As String
        Get
            Return "." + OutputExt
        End Get
    End Property

    Overridable ReadOnly Property OutputPath() As String
        Get
            If TypeOf Muxer Is NullMuxer Then
                Return p.TargetFile
            Else
                Return p.TempDir + p.TargetFile.Base + "_out." + OutputExt
            End If
        End Get
    End Property

    Overridable Function GetMenu() As MenuList
    End Function

    Overridable Function CanChunkEncode() As Boolean
    End Function

    Overridable Function GetFixedBitrate() As Integer
    End Function

    Overridable Function GetChunkEncodeActions() As List(Of Action)
    End Function

    Overridable Sub ImportCommandLine(commandLine As String)
    End Sub

    Sub SetMetaData(sourceFile As String)
        If Not p.ImportVUIMetadata Then
            Exit Sub
        End If

        Dim cl As String
        Dim colour_primaries = MediaInfo.GetVideo(sourceFile, "colour_primaries")

        Select Case colour_primaries
            Case "BT.2020"
                If colour_primaries.Contains("BT.2020") Then
                    cl += " --colorprim bt2020"
                End If
            Case "BT.709"
                If colour_primaries.Contains("BT.709") Then
                    cl += " --colorprim bt709"
                End If
        End Select

        Dim transfer_characteristics = MediaInfo.GetVideo(sourceFile, "transfer_characteristics")

        Select Case transfer_characteristics
            Case "PQ", "SMPTE ST 2084"
                If transfer_characteristics.Contains("SMPTE ST 2084") Or transfer_characteristics.Contains("PQ") Then
                    cl += " --transfer smpte2084"
                End If
            Case "BT.709"
                If transfer_characteristics.Contains("BT.709") Then
                    cl += " --transfer bt709"
                End If
            Case "HLG"
                cl += " --transfer arib-std-b67"
        End Select

        Dim matrix_coefficients = MediaInfo.GetVideo(sourceFile, "matrix_coefficients")

        Select Case matrix_coefficients
            Case "BT.2020 non-constant"
                If matrix_coefficients.Contains("BT.2020 non-constant") Then
                    cl += " --colormatrix bt2020nc"
                End If
            Case "BT.709"
                cl += " --colormatrix bt709"
        End Select

        Dim color_range = MediaInfo.GetVideo(sourceFile, "colour_range")

        Select Case color_range
            Case "Limited"
                cl += " --range limited"
            Case "Full"
                cl += " --range full"
        End Select

        Dim MasteringDisplay_ColorPrimaries = MediaInfo.GetVideo(sourceFile, "MasteringDisplay_ColorPrimaries")
        Dim MasteringDisplay_Luminance = MediaInfo.GetVideo(sourceFile, "MasteringDisplay_Luminance")

        If MasteringDisplay_ColorPrimaries <> "" AndAlso MasteringDisplay_Luminance <> "" Then
            Dim luminanceMatch = Regex.Match(MasteringDisplay_Luminance, "min: ([\d\.]+) cd/m2, max: ([\d\.]+) cd/m2")

            If luminanceMatch.Success Then
                Dim luminanceMin = luminanceMatch.Groups(1).Value.ToDouble * 10000
                Dim luminanceMax = luminanceMatch.Groups(2).Value.ToDouble * 10000

                If MasteringDisplay_ColorPrimaries.Contains("Display P3") Then
                    cl += " --output-depth 10"
                    cl += $" --master-display ""G(13250,34500)B(7500,3000)R(34000,16000)WP(15635,16450)L({luminanceMax},{luminanceMin})"""
                    cl += " --hdr"
                    cl += " --repeat-headers"
                    cl += " --range limited"
                    cl += " --hrd"
                    cl += " --aud"
                End If

                If MasteringDisplay_ColorPrimaries.Contains("DCI P3") Then
                    cl += " --output-depth 10"
                    cl += $" --master-display ""G(13250,34500)B(7500,3000)R(34000,16000)WP(15700,17550)L({luminanceMax},{luminanceMin})"""
                    cl += " --hdr"
                    cl += " --repeat-headers"
                    cl += " --range limited"
                    cl += " --hrd"
                    cl += " --aud"
                End If

                If MasteringDisplay_ColorPrimaries.Contains("BT.2020") Then
                    cl += " --output-depth 10"
                    cl += $" --master-display ""G(8500,39850)B(6550,2300)R(35400,14600)WP(15635,16450)L({luminanceMax},{luminanceMin})"""
                    cl += " --hdr"
                    cl += " --repeat-headers"
                    cl += " --range limited"
                    cl += " --hrd"
                    cl += " --aud"
                End If
            End If
        End If

        Dim MaxCLL = MediaInfo.GetVideo(sourceFile, "MaxCLL").Trim.Left(" ").ToInt
        Dim MaxFALL = MediaInfo.GetVideo(sourceFile, "MaxFALL").Trim.Left(" ").ToInt

        If MaxCLL <> 0 OrElse MaxFALL <> 0 Then
            cl += $" --max-cll ""{MaxCLL},{MaxFALL}"""
        End If

        ImportCommandLine(cl)
    End Sub

    Sub AfterEncoding()
        If Not g.FileExists(OutputPath) Then
            Throw New ErrorAbortException("Encoder output file is missing", OutputPath)
        Else
            Log.WriteLine(MediaInfo.GetSummary(OutputPath))
        End If
    End Sub

    Overrides Function CreateEditControl() As Control
        Dim ret As New ToolStripEx

        ret.Renderer = New ToolStripRendererEx(ToolStripRenderModeEx.SystemDefault)
        ret.ShowItemToolTips = False
        ret.GripStyle = ToolStripGripStyle.Hidden
        ret.BackColor = SystemColors.Window
        ret.Dock = DockStyle.Fill
        ret.BackColor = SystemColors.Window
        ret.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow
        ret.ShowControlBorder = True
        ret.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)

        Dim pad = ret.Font.Height \ 9

        For Each pair In GetMenu()
            Dim bn As New ToolStripButton
            bn.Margin = New Padding(2, 2, 0, 0)
            bn.Text = pair.Key
            bn.Padding = New Padding(pad)
            Dim tmp = pair
            AddHandler bn.Click, Sub() tmp.Value.Invoke()
            bn.TextAlign = ContentAlignment.MiddleLeft
            ret.Items.Add(bn)
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
        g.MainForm.llMuxer.Text = p.VideoEncoder.Muxer.OutputExt.ToUpper

        If GetFixedBitrate() <> 0 Then
            p.BitrateIsFixed = True
            g.MainForm.tbBitrate.Text = GetFixedBitrate().ToString
        End If

        g.MainForm.UpdateSizeOrBitrate()
    End Sub

    Public Enum Modes
        First = 1
        Second = 2
        CompCheck = 4
    End Enum

    Sub OpenMuxerConfigDialog()
        Dim muxer = ObjectHelp.GetCopy(Of Muxer)(Me.Muxer)

        If muxer.Edit = DialogResult.OK Then
            Me.Muxer = muxer
            g.MainForm.llMuxer.Text = Me.Muxer.OutputExt.ToUpper
            g.MainForm.Refresh()
            g.MainForm.UpdateSizeOrBitrate()
            g.MainForm.Assistant()
        End If
    End Sub

    Function OpenMuxerProfilesDialog() As DialogResult
        Using form As New ProfilesForm("Muxer Profiles", s.MuxerProfiles,
            AddressOf LoadMuxer, AddressOf GetMuxerProfile, AddressOf Muxer.GetDefaults)

            Return form.ShowDialog()
        End Using
    End Function

    Sub LoadMuxer(profile As Profile)
        Muxer = DirectCast(ObjectHelp.GetCopy(profile), Muxer)
        Muxer.Init()
        g.MainForm.llMuxer.Text = Muxer.OutputExt.ToUpper
        Dim newPath = p.TargetFile.ChangeExt(Muxer.OutputExt)

        If p.SourceFile <> "" AndAlso newPath.ToLower = p.SourceFile.ToLower Then
            newPath = newPath.Dir + newPath.Base + "_new" + newPath.ExtFull
        End If

        g.MainForm.tbTargetFile.Text = newPath
        g.MainForm.RecalcBitrate()
        g.MainForm.Assistant()
    End Sub

    Function GetMuxerProfile() As Profile
        Dim sb As New SelectionBox(Of Muxer)

        sb.Title = "New Profile"
        sb.Text = "Please select a profile."

        sb.AddItem("Current Project", Muxer)

        For Each i In StaxRip.Muxer.GetDefaults()
            sb.AddItem(i)
        Next

        If sb.Show = DialogResult.OK Then
            Return sb.SelectedValue
        End If

        Return Nothing
    End Function

    Shared Function GetDefaults() As List(Of VideoEncoder)
        Dim ret As New List(Of VideoEncoder)

        ret.Add(New x264Enc)
        ret.Add(New x265Enc)

        ret.Add(New aomenc)
        ret.Add(New Rav1e)
        ret.Add(New SVTAV1)

        Dim nvidia264 As New NVEnc()
        ret.Add(nvidia264)

        Dim nvidia265 As New NVEnc()
        nvidia265.Params.Codec.Value = 1
        ret.Add(nvidia265)

        ret.Add(New QSVEnc())

        Dim intel265 As New QSVEnc()
        intel265.Params.Codec.Value = 1
        ret.Add(intel265)

        ret.Add(New VCEEnc())

        Dim amd265 As New VCEEnc()
        amd265.Params.Codec.Value = 1
        ret.Add(amd265)

        Dim ffmpeg = New ffmpegEnc()

        For x = 0 To ffmpeg.Params.Codec.Options.Length - 1
            Dim ffmpeg2 = New ffmpegEnc()
            ffmpeg2.Params.Codec.Value = x
            ret.Add(ffmpeg2)
        Next

        Dim cmdl As New BatchEncoder()
        cmdl.OutputFileTypeValue = "avi"
        cmdl.Name = "Command Line"
        cmdl.Muxer = New ffmpegMuxer("AVI")
        cmdl.QualityMode = True
        cmdl.CommandLines = """%app:xvid_encraw%"" -cq 2 -smoother 0 -max_key_interval 250 -nopacked -vhqmode 4 -qpel -notrellis -max_bframes 1 -bvhq -bquant_ratio 162 -bquant_offset 0 -threads 1 -i ""%script_file%"" -avi ""%encoder_out_file%"" -par %target_par_x%:%target_par_y%"
        ret.Add(cmdl)

        ret.Add(New NullEncoder())

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
        ShowConfigDialog()
        Return DialogResult.OK
    End Function

    Public Class MenuList
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
        ImportCommandLine(commandLine, CommandLineParams)
    End Sub

    Overloads Shared Sub ImportCommandLine(commandLine As String, params As CommandLineParams)
        Try
            If commandLine = "" Then
                Exit Sub
            End If

            For Each i In {"tune", "preset", "profile"}
                Dim match = Regex.Match(commandLine, "(.*)(--" + i + "\s\w+)(.*)")

                If match.Success Then
                    commandLine = match.Groups(2).Value + " " + match.Groups(1).Value + " " + match.Groups(3).Value
                End If
            Next

            Dim a = commandLine.SplitNoEmptyAndWhiteSpace(" ")

            For x = 0 To a.Length - 1
                For Each param In params.Items
                    If Not param.ImportAction Is Nothing AndAlso
                        param.GetSwitches.Contains(a(x)) AndAlso a.Length - 1 > x Then

                        param.ImportAction.Invoke(a(x), a(x + 1))
                        params.RaiseValueChanged(param)
                        Exit For
                    End If

                    If TypeOf param Is BoolParam Then
                        Dim boolParam = DirectCast(param, BoolParam)

                        If boolParam.GetSwitches.Contains(a(x)) Then
                            boolParam.Value = True
                            params.RaiseValueChanged(param)
                            Exit For
                        End If
                    ElseIf TypeOf param Is NumParam Then
                        Dim numParam = DirectCast(param, NumParam)

                        If numParam.GetSwitches.Contains(a(x)) AndAlso
                            a.Length - 1 > x AndAlso a(x + 1).IsDouble Then

                            numParam.Value = a(x + 1).ToDouble
                            params.RaiseValueChanged(param)
                            Exit For
                        End If
                    ElseIf TypeOf param Is OptionParam Then
                        Dim optionParam = DirectCast(param, OptionParam)

                        If optionParam.GetSwitches.Contains(a(x)) Then
                            Dim exitFor As Boolean

                            If a.Length - 1 > x Then
                                If optionParam.IntegerValue Then
                                    For xOpt = 0 To optionParam.Options.Length - 1
                                        If a(x + 1) = xOpt.ToString Then
                                            optionParam.Value = xOpt
                                            params.RaiseValueChanged(param)
                                            exitFor = True
                                            Exit For
                                        End If
                                    Next
                                Else
                                    For xOpt = 0 To optionParam.Options.Length - 1
                                        Dim values = If(optionParam.Values.NothingOrEmpty, optionParam.Options, optionParam.Values)

                                        If a(x + 1).Trim(""""c).ToLower = values(xOpt).ToLower.Replace(" ", "") Then
                                            optionParam.Value = xOpt
                                            params.RaiseValueChanged(param)
                                            exitFor = True
                                            Exit For
                                        End If
                                    Next
                                End If

                                If exitFor Then
                                    Exit For
                                End If
                            ElseIf a.Length - 1 = x Then
                                If Not optionParam.Values Is Nothing Then
                                    For xOpt = 0 To optionParam.Values.Length - 1
                                        If a(x) = optionParam.Values(xOpt) AndAlso optionParam.Values(xOpt).StartsWith("--") Then
                                            optionParam.Value = xOpt
                                            params.RaiseValueChanged(param)
                                            exitFor = True
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    ElseIf TypeOf param Is StringParam Then
                        Dim stringParam = DirectCast(param, StringParam)

                        If stringParam.GetSwitches.Contains(a(x)) AndAlso a.Length - 1 > x Then
                            stringParam.Value = a(x + 1).Trim(""""c)
                            params.RaiseValueChanged(param)
                            Exit For
                        End If
                    End If
                Next
            Next
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub
End Class

<Serializable()>
Public Class BatchEncoder
    Inherits VideoEncoder

    Sub New()
        Name = "Command Line"
        Muxer = New MkvMuxer()
    End Sub

    Property CommandLines As String = ""
    Property CompCheckCommandLines As String = ""

    Property OutputFileTypeValue As String

    Overrides ReadOnly Property OutputExt As String
        Get
            Return OutputFileTypeValue
        End Get
    End Property

    Overrides Sub ShowConfigDialog()
        Using form As New CommandLineVideoEncoderForm(Me)
            If form.ShowDialog() = DialogResult.OK Then
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
        p.Script.Synchronize()

        For Each line In Macro.Expand(CommandLines).SplitLinesNoEmpty
            Using proc As New Proc
                proc.Header = "Video encoding command line encoder: " + Name
                proc.SkipStrings = Proc.GetSkipStrings(CommandLines)
                proc.File = "cmd.exe"
                proc.Arguments = "/S /C """ + line + """"

                Try
                    proc.Start()
                Catch ex As AbortException
                    Throw ex
                Catch ex As Exception
                    g.ShowException(ex)
                    Throw New AbortException
                End Try
            End Using
        Next
    End Sub

    Overrides Sub RunCompCheck()
        If CompCheckCommandLines = "" OrElse CompCheckCommandLines.Trim = "" Then
            ShowConfigDialog()
            Exit Sub
        End If

        If Not g.VerifyRequirements OrElse Not g.IsValidSource Then
            Exit Sub
        End If

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

        script.Filters.Add(New VideoFilter("aaa", "aaa", code))
        script.Path = (p.TempDir + p.TargetFile.Base + "_CompCheck." + script.FileType).ToShortFilePath
        script.Synchronize()

        Dim line = Macro.Expand(CompCheckCommandLines)

        Using proc As New Proc
            proc.Header = "Compressibility Check"
            proc.WriteLog(code + BR2)
            proc.SkipStrings = Proc.GetSkipStrings(line)
            proc.File = "cmd.exe"
            proc.Arguments = "/S /C """ + line + """"

            Try
                proc.Start()
            Catch ex As AbortException
                Exit Sub
            Catch ex As Exception
                g.ShowException(ex)
                Exit Sub
            End Try
        End Using

        Dim bits = (New FileInfo(p.TempDir + p.TargetFile.Base + "_CompCheck." + OutputExt).Length) * 8
        p.Compressibility = (bits / script.GetFrameCount) / (p.TargetWidth * p.TargetHeight)

        OnAfterCompCheck()

        g.MainForm.Assistant()

        Log.WriteLine(CInt(Calc.GetPercent).ToString() + " %")
        Log.Save()
    End Sub
End Class

<Serializable()>
Public Class NullEncoder
    Inherits VideoEncoder

    Sub New()
        Name = "Copy/Mux"
        Muxer = New MkvMuxer()
        QualityMode = True
    End Sub

    Function GetSourceFile() As String
        For Each i In {".h264", ".avc", ".h265", ".hevc", ".mpg", ".avi"}
            If File.Exists(p.SourceFile.DirAndBase + "_out" + i) Then
                Return p.SourceFile.DirAndBase + "_out" + i
            ElseIf File.Exists(p.TempDir + p.TargetFile.Base + "_out" + i) Then
                Return p.TempDir + p.TargetFile.Base + "_out" + i
            End If
        Next

        If FileTypes.VideoText.Contains(p.SourceFile.Ext) Then
            Return p.LastOriginalSourceFile
        Else
            Return p.SourceFile
        End If
    End Function

    Overrides ReadOnly Property OutputPath As String
        Get
            Dim sourceFile = GetSourceFile()

            If Not p.VideoEncoder.Muxer.IsSupported(sourceFile.Ext) Then
                Select Case sourceFile.Ext
                    Case "mkv"
                        Dim streams = MediaInfo.GetVideoStreams(sourceFile)

                        If streams.Count = 0 Then
                            Return sourceFile
                        End If

                        Return p.TempDir + sourceFile.Base + streams(0).ExtFull
                End Select
            End If

            Return sourceFile
        End Get
    End Property

    Overrides ReadOnly Property OutputExt As String
        Get
            Return OutputPath.Ext
        End Get
    End Property

    Overrides Sub Encode()
        Dim sourceFile = GetSourceFile()

        If Not p.VideoEncoder.Muxer.IsSupported(sourceFile.Ext) Then
            Select Case sourceFile.Ext
                Case "mkv"
                    mkvDemuxer.Demux(sourceFile, Nothing, Nothing, Nothing, p, False, True)
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
