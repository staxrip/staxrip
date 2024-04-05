
Imports System.Text.RegularExpressions

Imports StaxRip.UI
Imports StaxRip.VideoEncoderCommandLine

<Serializable()>
Public MustInherit Class VideoEncoder
    Inherits Profile
    Implements IComparable(Of VideoEncoder)

    MustOverride Sub Encode()

    MustOverride ReadOnly Property OutputExt As String

    Overridable Property Bitrate As Integer
    Overridable Property Passes As Integer
    Overridable Property QualityMode As Boolean

    Property AutoCompCheckValue As Integer = 50
    Property Muxer As Muxer = New MkvMuxer

    Public MustOverride Sub ShowConfigDialog(Optional path As String = Nothing)

    Sub New()
        CanEditValue = True
    End Sub

    Public Overridable ReadOnly Property IsOvercroppingAllowed As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overridable ReadOnly Property IsUnequalResizingAllowed As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overridable ReadOnly Property IsResizingAllowed As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overridable ReadOnly Property DolbyVisionMetadataPath As String
        Get
            Return Nothing
        End Get
    End Property

    Public Overridable ReadOnly Property Hdr10PlusMetadataPath As String
        Get
            Return Nothing
        End Get
    End Property

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

    Overridable Function GetCommandLine(includePaths As Boolean, includeExecutable As Boolean) As String
    End Function

    Overridable Function GetMenu() As MenuList
    End Function

    Overridable Function CanChunkEncode() As Boolean
    End Function

    Overridable Function GetChunks() As Integer
    End Function

    Overridable Function GetChunkEncodeActions() As List(Of Action)
    End Function

    Overridable Sub ImportCommandLine(commandLine As String)
    End Sub

    Overridable Function BeforeEncoding() As Boolean
        Return True
    End Function

    Overridable Function AfterEncoding() As Boolean
        If Not g.FileExists(OutputPath) Then Throw New ErrorAbortException("Encoder output file is missing", OutputPath)

        Log.WriteLine(MediaInfo.GetSummary(OutputPath))
        Log.Save()

        Return True
    End Function

    Overridable Sub SetMetaData(sourceFile As String)
    End Sub

    Overrides Function CreateEditControl() As Control
        Dim ret As New ToolStripEx With {
            .Renderer = New ToolStripRendererEx(),
            .ShowItemToolTips = False,
            .GripStyle = ToolStripGripStyle.Hidden,
            .Dock = DockStyle.Fill,
            .LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow,
            .ShowControlBorder = True,
            .Font = New Font("Segoe UI", 9 * s.UIScaleFactor)
        }

        Dim pad = ret.Font.Height \ 9

        For Each pair In GetMenu()
            Dim bn As New ToolStripButtonEx
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

    Protected Function GetH265MaxBitrate(level As Single, highTier As Boolean) As Integer
        If level < 1 Then Return 0

        Select Case level
            Case 1
                Return If(highTier, 0, 128)
            Case 2
                Return If(highTier, 0, 1500)
            Case 2.1
                Return If(highTier, 0, 3000)
            Case 3
                Return If(highTier, 0, 6000)
            Case 3.1
                Return If(highTier, 0, 10000)
            Case 4
                Return If(highTier, 30000, 12000)
            Case 4.1
                Return If(highTier, 50000, 20000)
            Case 5
                Return If(highTier, 100000, 25000)
            Case 5.1
                Return If(highTier, 160000, 40000)
            Case 5.2
                Return If(highTier, 240000, 60000)
            Case 6
                Return If(highTier, 240000, 60000)
            Case 6.1
                Return If(highTier, 480000, 120000)
            Case 6.2
                Return If(highTier, 800000, 240000)
            Case Else
                Return 0
        End Select
    End Function

    Sub AutoSetImageSize()
        If p.VideoEncoder.AutoCompCheckValue > 0 AndAlso Calc.GetPercent <> 0 AndAlso p.Script.IsFilterActive("Resize") Then
            Dim oldWidth = p.TargetWidth
            Dim oldHeight = p.TargetHeight

            p.TargetWidth = Calc.FixMod16(CInt((p.SourceHeight - p.CropTop - p.CropBottom) * Calc.GetTargetDAR()))

            Dim cropw = p.SourceWidth - p.CropLeft - p.CropRight

            If p.TargetWidth > cropw Then
                p.TargetWidth = cropw
            End If

            p.TargetHeight = Calc.FixMod16(CInt(p.TargetWidth / Calc.GetTargetDAR()))

            While Calc.GetPercent < p.VideoEncoder.AutoCompCheckValue
                If p.TargetWidth - 16 >= 720 Then
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
    End Function

    Sub OnStateChange()
        g.MainForm.UpdateEncoderStateRelatedControls()
        g.MainForm.SetEncoderControl(p.VideoEncoder.CreateEditControl)
        g.MainForm.lgbEncoder.Text = g.ConvertPath(p.VideoEncoder.Name).Shorten(38)
        g.MainForm.llMuxer.Text = p.VideoEncoder.Muxer.OutputExt.ToUpperInvariant

        If Not QualityMode Then
            If Bitrate = 0 Then
                Bitrate = p.VideoBitrate
            End If

            g.MainForm.tbBitrate.Text = Bitrate.ToString
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
            g.MainForm.llMuxer.Text = Me.Muxer.OutputExt.ToUpperInvariant
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
        g.MainForm.llMuxer.Text = Muxer.OutputExt.ToUpperInvariant
        Dim newPath = p.TargetFile.ChangeExt(Muxer.OutputExt)

        If p.SourceFile <> "" AndAlso newPath.ToLowerInvariant = p.SourceFile.ToLowerInvariant Then
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
        Dim ret As New List(Of VideoEncoder) From {
            New VvencffappEnc(),
            New x265Enc(),
            New x264Enc(),
            New SvtAv1Enc(),
            New AOMEnc(),
            New Rav1e()
        }

        Dim nvEnc = New NVEnc()
        For x = 0 To nvEnc.Params.Codec.Options.Length - 1
            ret.Add(New NVEnc(x))
        Next

        Dim vceEnc = New VCEEnc()
        For x = 0 To vceEnc.Params.Codec.Options.Length - 1
            ret.Add(New VCEEnc(x))
        Next

        Dim qsvEnc = New QSVEnc()
        For x = 0 To qsvEnc.Params.Codec.Options.Length - 1
            ret.Add(New QSVEnc(x))
        Next

        Dim ffmpeg = New ffmpegEnc()
        For x = 0 To ffmpeg.Params.Codec.Options.Length - 1
            ret.Add(New ffmpegEnc(x))
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
        Dim name = InputBox.Show("Please enter a profile name", encoder.Name)

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

    Overrides Function GetCommandLine(includePaths As Boolean, includeExecutable As Boolean) As String
        Return CommandLineParams.GetCommandLine(includePaths, includeExecutable)
    End Function

    Overloads Shared Sub ImportCommandLine(commandLine As String, params As CommandLineParams)
        Try
            If commandLine = "" Then Exit Sub

            For Each i In {"tune", "preset", "profile"}
                Dim match = Regex.Match(commandLine, "(.*)(--" + i + "\s\w+)(.*)")

                If match.Success Then
                    commandLine = match.Groups(2).Value + " " + match.Groups(1).Value + " " + match.Groups(3).Value
                End If
            Next

            Dim a = g.MainForm.ParseCommandLine(commandLine)

            For x = 0 To a.Length - 1
                For Each param In params.Items
                    If param.ImportAction IsNot Nothing AndAlso
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
                                        Dim value = If(a(x + 1).StartsWith("--"), a(x), a(x + 1))

                                        If value.Trim(""""c).ToLowerInvariant = values(xOpt).ToLowerInvariant.Replace(" ", "") Then
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
                                If optionParam.Values IsNot Nothing Then
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

    Overrides Sub ShowConfigDialog(Optional path As String = Nothing)
        Using form As New CommandLineVideoEncoderForm(Me)
            If form.ShowDialog() = DialogResult.OK Then
                OnStateChange()
            End If
        End Using
    End Sub

    Overrides Function GetCommandLine(includePaths As Boolean, includeExecutable As Boolean) As String
        Return CommandLines
    End Function

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
        script.Filters = p.Script.GetFiltersCopy()
        Dim code As String
        Dim framerate = p.Script.GetFramerate()
        Dim totalFrames = p.Script.GetFrameCount()
        Dim range = framerate * p.CompCheckTestblockSeconds
        Dim every = (100 / p.CompCheckPercentage) * range

        If script.Engine = ScriptEngine.AviSynth Then
            code = $"SelectRangeEvery({CInt(every)},{CInt(range)})"
        Else
            code = "fpsnum = clip.fps_num" + BR
            code += "fpsden = clip.fps_den" + BR
            code += $"clip = core.std.SelectEvery(clip = clip, cycle = {CInt(every)}, offsets = range({CInt(range)}))" + BR
            code += "clip = core.std.AssumeFPS(clip = clip, fpsnum = fpsnum, fpsden = fpsden)"
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
                    mkvDemuxer.Demux(sourceFile, Nothing, Nothing, Nothing, p, False, True, True, "Demux Video MKV", True)
            End Select
        End If
    End Sub

    Overrides Function GetMenu() As MenuList
        Dim ret As New MenuList
        ret.Add("Container Configuration", AddressOf OpenMuxerConfigDialog)
        Return ret
    End Function

    Overrides Sub ShowConfigDialog(Optional path As String = Nothing)
    End Sub
End Class
