Imports System.Runtime.Serialization

Imports StaxRip.UI
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Globalization

<Serializable()>
Public Class x264Encoder
    Inherits VideoEncoder

    Property Params As New x264Params

    Sub New()
        Name = "x264"
        AutoCompCheckValue = 50
    End Sub

    Overrides ReadOnly Property OutputFileType As String
        Get
            Return "h264"
        End Get
    End Property

    Overrides Sub Encode()
        p.Script.Synchronize()
        Encode("Encoding video using x264 " + Package.x264.Version, GetArgs(1), p.Script, s.ProcessPriority)

        If Params.Mode.Value = x264Mode.TwoPass OrElse
            Params.Mode.Value = x264Mode.ThreePass Then

            Encode("Encoding video second pass using x264 " + Package.x264.Version, GetArgs(2), p.Script, s.ProcessPriority)
        End If

        If Params.Mode.Value = x264Mode.ThreePass Then
            Encode("Encoding video third pass using x264 " + Package.x264.Version, GetArgs(3), p.Script, s.ProcessPriority)
        End If

        AfterEncoding()
    End Sub

    Overloads Sub Encode(passName As String,
                         args As String,
                         script As VideoScript,
                         priority As ProcessPriorityClass)

        If p.Script.Engine = ScriptingEngine.VapourSynth Then
            Dim batchPath = p.TempDir + Filepath.GetBase(p.TargetFile) + "_encode.bat"

            Dim batchCode = "@echo off" + CrLf + "CHCP 65001" + CrLf +
                Package.vspipe.GetPath.Quotes + " " + script.Path.Quotes + " - --y4m | " +
                Package.x264.GetPath.Quotes + " " + args

            File.WriteAllText(batchPath, batchCode, New UTF8Encoding(False))

            Using proc As New Proc
                proc.Init(passName)
                proc.Encoding = Encoding.UTF8
                proc.Priority = priority
                proc.SkipStrings = {"kb/s, eta", "%]"}
                proc.WriteLine(batchCode + CrLf2)
                proc.File = "cmd.exe"
                proc.Arguments = "/C call """ + batchPath + """"
                proc.Start()
            End Using
        Else
            Using proc As New Proc
                proc.Init(passName)
                proc.Priority = priority
                proc.SkipStrings = {"kb/s, eta", "%]"}
                proc.File = Package.x264.GetPath
                proc.Arguments = args
                proc.Start()
            End Using
        End If
    End Sub

    Overrides Sub RunCompCheck()
        If Not Paths.VerifyRequirements Then Exit Sub
        If Not g.IsValidSource Then Exit Sub

        Dim enc As New x264Encoder
        enc.Params = ObjectHelp.GetCopy(Of x264Params)(Params)
        enc.Params.Mode.Value = x264Mode.SingleCRF
        enc.Params.Quant.Value = enc.Params.QuantCompCheck.Value

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

        Log.WriteHeader("Compressibility Check Script")
        Log.WriteLine(code + CrLf2)

        script.Filters.Add(New VideoFilter("aaa", "aaa", code))
        script.Path = p.TempDir + p.Name + "_CompCheck." + script.FileType
        script.Synchronize()

        Dim sourcePath = If(p.Script.Engine = ScriptingEngine.VapourSynth, "-", p.TempDir + p.Name + "_CompCheck.avs")
        Dim arguments = enc.GetArgs(0, sourcePath, p.TempDir + p.Name + "_CompCheck." + OutputFileType, script)

        Try
            Encode("Compressibility Check", arguments, script, ProcessPriorityClass.Normal)
        Catch ex As AbortException
            ProcessForm.CloseProcessForm()
            Exit Sub
        Catch ex As Exception
            ProcessForm.CloseProcessForm()
            g.ShowException(ex)
            Exit Sub
        End Try

        Dim bits = (New FileInfo(p.TempDir + p.Name + "_CompCheck." + OutputFileType).Length) * 8
        p.Compressibility = (bits / script.GetFrames) / (p.TargetWidth * p.TargetHeight)
        OnAfterCompCheck()
        g.MainForm.Assistant()

        Log.WriteLine("Quality: " & CInt(Calc.GetPercent).ToString() + " %")
        Log.WriteLine("Compressibility: " + p.Compressibility.ToString("f3"))
        Log.Save()

        ProcessForm.CloseProcessForm()
    End Sub

    Overrides Sub ShowConfigDialog()
        Using f As New x264Form(Me)
            If f.ShowDialog() = DialogResult.OK Then
                g.MainForm.PopulateProfileMenu(DynamicMenuItemID.EncoderProfiles)
                OnStateChange()
            End If
        End Using
    End Sub

    Private Function IsTurboPass(pass As Integer) As Boolean
        If Params.Mode.Value = x264Mode.TwoPass AndAlso pass <> 2 Then
            Return True
        End If

        If Params.Mode.Value = x264Mode.ThreePass AndAlso pass <> 3 Then
            Return True
        End If
    End Function

    Function GetArgs(pass As Integer, Optional includePaths As Boolean = True) As String
        Dim sourcePath = If(p.Script.Engine = ScriptingEngine.VapourSynth, "-", p.Script.Path)
        Return GetArgs(pass, sourcePath, Filepath.GetDirAndBase(OutputPath) + "." + OutputFileType, p.Script, includePaths)
    End Function

    Function GetArgs(pass As Integer,
                     sourcePath As String,
                     targetPath As String,
                     script As VideoScript,
                     Optional includePaths As Boolean = True) As String

        Dim sb As New StringBuilder

        Dim defaults As New x264Params
        defaults.ApplyDefaults(Params)

        If Params.Preset.Value <> x264PresetMode.Medium Then
            sb.Append(" --preset " + CType(Params.Preset.Value, x264PresetMode).ToString.ToLower)
        End If

        If Params.Tune.Value <> x264TuneMode.Disabled Then
            sb.Append(" --tune " + CType(Params.Tune.Value, x264TuneMode).ToString.ToLower)
        End If

        If Params.Profile.Value <> x264ProfileMode.High Then
            sb.Append(" --profile " + CType(Params.Profile.Value, x264ProfileMode).ToString.ToLower)
        End If

        If Params.Mode.Value = x264Mode.TwoPass Then
            sb.Append(" --pass " & pass)

            If pass = 1 AndAlso Params.SlowFirstpass.Value Then
                sb.Append(" --slow-firstpass")
            End If
        ElseIf Params.Mode.Value = x264Mode.ThreePass Then
            sb.Append(" --pass " & pass)

            If (pass = 1 OrElse pass = 2) AndAlso Params.SlowFirstpass.Value Then
                sb.Append(" --slow-firstpass")
            End If
        End If

        If Params.Mode.Value = x264Mode.SingleQuant Then
            sb.Append(" --qp " + CInt(Params.Quant.Value).ToString)
        ElseIf Params.Mode.Value = x264Mode.SingleCRF Then
            If Params.Quant.Value <> 23 Then
                sb.Append(" --crf " + Params.Quant.Value.ToString(CultureInfo.InvariantCulture))
            End If
        Else
            sb.Append(" --bitrate " + p.VideoBitrate.ToString)
        End If

        If Params.Level.Value <> defaults.Level.Value Then
            sb.Append(" --level " + DispNameAttribute.GetValueForEnum(CType(Params.Level.Value, x264LevelMode)))
        End If

        If Params.GOPSizeMax.Value <> defaults.GOPSizeMax.Value Then
            sb.Append(" --keyint " + Params.GOPSizeMax.Value.ToString)
        End If

        If Params.GOPSizeMin.Value <> defaults.GOPSizeMin.Value Then
            sb.Append(" --min-keyint " + Params.GOPSizeMin.Value.ToString)
        End If

        If Params.OpenGopV2.Value <> defaults.OpenGopV2.Value Then
            sb.Append(" --open-gop")
        End If

        If Params.RefFrames.Value <> defaults.RefFrames.Value Then
            sb.Append(" --ref " + Params.RefFrames.Value.ToString)
        End If

        If Not Params.MixedRefs.Value AndAlso Params.MixedRefs.Value <> defaults.MixedRefs.Value Then
            sb.Append(" --no-mixed-refs")
        End If

        If Not Params.FastPSkip.Value AndAlso Params.FastPSkip.Value <> defaults.FastPSkip.Value Then
            sb.Append(" --no-fast-pskip")
        End If

        If Not Params.MbTree.Value AndAlso Params.MbTree.Value <> defaults.MbTree.Value Then
            sb.Append(" --no-mbtree")
        End If

        If Params.RcLookahead.Value <> defaults.RcLookahead.Value Then
            sb.Append(" --rc-lookahead " + Params.RcLookahead.Value.ToString)
        End If

        If Params.NoiseReduction.Value <> defaults.NoiseReduction.Value Then
            sb.Append(" --nr " + Params.NoiseReduction.Value.ToString)
        End If

        If Params.BFrames.Value > 0 Then
            If Params.BFrames.Value <> defaults.BFrames.Value Then
                sb.Append(" --bframes " + Params.BFrames.Value.ToString)
            End If

            If Params.BAdapt.Value <> defaults.BAdapt.Value Then
                sb.Append(" --b-adapt " + Params.BAdapt.Value.ToString)
            End If

            If Params.BFrames.Value > 1 AndAlso Params.BPyramidMode.Value <> defaults.BPyramidMode.Value Then
                sb.Append(" --b-pyramid " + CType(Params.BPyramidMode.Value, x264BPyramidMode).ToString.ToLower)
            End If

            If Params.BFramesBias.Value <> defaults.BFramesBias.Value Then
                sb.Append(" --b-bias " + Params.BFramesBias.Value.ToString)
            End If

            If Not Params.WeightB.Value AndAlso Params.WeightB.Value <> defaults.WeightB.Value Then
                sb.Append(" --no-weightb")
            End If

            If Params.DirectMode.Value <> defaults.DirectMode.Value Then
                sb.Append(" --direct " + CType(Params.DirectMode.Value, x264DirectMode).ToString.ToLower)
            End If
        End If

        If Not Params.Deblock.Value AndAlso Params.Deblock.Value <> defaults.Deblock.Value Then
            sb.Append(" --no-deblock")
        Else
            If Params.DeblockAlpha.Value <> defaults.DeblockAlpha.Value OrElse Params.DeblockBeta.Value <> defaults.DeblockBeta.Value Then
                sb.Append(" --deblock " + Params.DeblockAlpha.Value.ToString + ":" + Params.DeblockBeta.Value.ToString)
            End If
        End If

        If Not Params.CABAC.Value AndAlso Params.CABAC.Value <> defaults.CABAC.Value Then
            sb.Append(" --no-cabac")
        End If

        If Params.SubME.Value <> defaults.SubME.Value Then
            sb.Append(" --subme " + Params.SubME.Value.ToString)
        End If

        If Params.Trellis.Value <> defaults.Trellis.Value Then
            sb.Append(" --trellis " + Params.Trellis.Value.ToString)
        End If

        If Not Params.Psy.Value AndAlso Params.Psy.Value <> defaults.Psy.Value Then
            sb.Append(" --no-psy")
        Else
            If Params.SubME.Value > 5 AndAlso (Params.PsyRD.Value <> defaults.PsyRD.Value OrElse Params.PsyTrellis.Value <> defaults.PsyTrellis.Value) Then
                sb.Append(" --psy-rd " + Params.PsyRD.Value.ToString(CultureInfo.InvariantCulture) + ":" + Params.PsyTrellis.Value.ToString(CultureInfo.InvariantCulture))
            End If
        End If

        Dim part = GetPartition(Params)

        If OK(part) AndAlso part <> GetPartition(defaults) Then
            sb.Append(part)
        End If

        If Not Params.AdaptiveDCT.Value AndAlso Params.AdaptiveDCT.Value <> defaults.AdaptiveDCT.Value Then
            sb.Append(" --no-8x8dct")
        End If

        If Params.IPRatio.Value <> defaults.IPRatio.Value Then
            sb.Append(" --ipratio " + Params.IPRatio.Value.ToString(CultureInfo.InvariantCulture))
        End If

        If Params.PBRatio.Value <> defaults.PBRatio.Value Then
            sb.Append(" --pbratio " + Params.PBRatio.Value.ToString(CultureInfo.InvariantCulture))
        End If

        If Params.QPMin.Value <> defaults.QPMin.Value Then
            sb.Append(" --qpmin " + Params.QPMin.Value.ToString)
        End If

        If Params.VBVBufSize.Value > defaults.VBVBufSize.Value Then
            sb.Append(" --vbv-bufsize " + Params.VBVBufSize.Value.ToString)
        End If

        If Params.VBVMaxRate.Value > defaults.VBVMaxRate.Value Then
            sb.Append(" --vbv-maxrate " + Params.VBVMaxRate.Value.ToString)
        End If

        If Params.VBVInit.Value <> defaults.VBVInit.Value Then
            sb.Append(" --vbv-init " + Params.VBVInit.Value.ToString(CultureInfo.InvariantCulture))
        End If

        If Params.QComp.Value <> defaults.QComp.Value Then
            sb.Append(" --qcomp " + Params.QComp.Value.ToString(CultureInfo.InvariantCulture))
        End If

        If Not Params.ChromaMe.Value Then
            sb.Append(" --no-chroma-me")
        End If

        If Params.MEMethod.Value <> defaults.MEMethod.Value Then
            sb.Append(" --me " + CType(Params.MEMethod.Value, x264MeMethodMode).ToString)
        End If

        If Params.MeRange.Value <> defaults.MeRange.Value Then
            sb.Append(" --merange " + Params.MeRange.Value.ToString)
        End If

        If Params.SceneCut.Value <> defaults.SceneCut.Value Then
            sb.Append(" --scenecut " + Params.SceneCut.Value.ToString)
        End If

        If Params.Threads.Value > defaults.Threads.Value Then
            sb.Append(" --threads " + Params.Threads.Value.ToString)
        End If

        If Params.Aud.Value Then
            sb.Append(" --aud")
        End If

        If Params.Slices.Value > defaults.Slices.Value Then
            sb.Append(" --slices " + Params.Slices.Value.ToString)
        End If

        If Params.NalHrdMode.Value <> defaults.NalHrdMode.Value Then
            sb.Append(" --nal-hrd " + CType(Params.NalHrdMode.Value, x264NalHrdMode).ToString.ToLower)
        End If

        If Params.BlurayCompat.Value Then
            sb.Append(" --bluray-compat")
        End If

        If Params.Overscan.Value <> defaults.Overscan.Value Then
            sb.Append(" --overscan " + CType(Params.Overscan.Value, x264OverscanMode).ToString.ToLower)
        End If

        If Params.Videoformat.Value <> defaults.Videoformat.Value Then
            sb.Append(" --videoformat " + CType(Params.Videoformat.Value, x264VideoformatMode).ToString.ToLower)
        End If

        If Params.Fullrange.Value <> defaults.Fullrange.Value Then
            sb.Append(" --fullrange " + CType(Params.Fullrange.Value, x264FullrangeMode).ToString.ToLower)
        End If

        If Params.Colorprim.Value <> defaults.Colorprim.Value Then
            sb.Append(" --colorprim " + CType(Params.Colorprim.Value, x264ColorprimMode).ToString.ToLower)
        End If

        If Params.Transfer.Value <> defaults.Transfer.Value Then
            sb.Append(" --transfer " + CType(Params.Transfer.Value, x264TransferMode).ToString.ToLower)
        End If

        If Params.Colormatrix.Value <> defaults.Colormatrix.Value Then
            sb.Append(" --colormatrix " + CType(Params.Colormatrix.Value, x264ColormatrixMode).ToString.ToLower)
        End If

        If Params.Chromaloc.Value > defaults.Chromaloc.Value Then
            sb.Append(" --chromaloc " + Params.Chromaloc.Value.ToString)
        End If

        If Params.PicStruct.Value Then
            sb.Append(" --pic-struct")
        End If

        If Params.WeightP.Value <> defaults.WeightP.Value Then
            sb.Append(" --weightp " + Params.WeightP.Value.ToString)
        End If

        If Params.AQMode.Value <> defaults.AQMode.Value Then
            sb.Append(" --aq-mode " + Params.AQMode.Value.ToString)
        End If

        If Params.AQStrengthV2.Value <> defaults.AQStrengthV2.Value Then
            sb.Append(" --aq-strength " + (Params.AQStrengthV2.Value).ToString("f1", CultureInfo.InvariantCulture))
        End If

        If Not Params.Progress.Value Then
            sb.Append(" --no-progress")
        End If

        If Not Params.DctDecimate.Value AndAlso Params.DctDecimate.Value <> defaults.DctDecimate.Value Then
            sb.Append(" --no-dct-decimate")
        End If

        If Params.ThreadInput.Value AndAlso Params.Threads.Value <> defaults.Threads.Value Then
            sb.Append(" --thread-input")
        End If

        If Calc.IsARSignalingRequired Then
            Dim par = Calc.GetTargetPAR
            sb.Append(" --sar " & par.X & ":" & par.Y)
        End If

        If Params.PSNR.Value Then
            sb.Append(" --psnr")
        End If

        If Params.SSIM.Value Then
            sb.Append(" --ssim")
        End If

        If Params.AddAll.Value <> "" Then
            sb.Append(" " + Params.AddAll.Value)
        End If

        Dim ret = sb.ToString

        If IsTurboPass(pass) Then
            Dim switches = Params.TurboRemove.Value.SplitNoEmpty(" ")

            For Each i In switches
                Dim re As New Regex(" *" + i + ".*?(?= --)| *" + i + ".*")

                If re.IsMatch(ret) Then
                    ret = re.Replace(ret, "", RegexOptions.IgnoreCase)
                End If
            Next

            If Params.TurboAdd.Value <> "" Then
                ret += " " + Params.TurboAdd.Value
            End If
        End If

        If sourcePath = "-" Then ret += " --demuxer y4m --frames " & script.GetFrames

        If includePaths Then
            If Params.Mode.Value = x264Mode.TwoPass OrElse Params.Mode.Value = x264Mode.ThreePass Then
                ret += " --stats """ + p.TempDir + p.Name + ".stats"""
            End If

            If (Params.Mode.Value = x264Mode.ThreePass AndAlso
                (pass = 1 OrElse pass = 2)) OrElse
                Params.Mode.Value = x264Mode.TwoPass AndAlso pass = 1 Then

                ret += " --output NUL " + sourcePath.Quotes
            Else
                ret += " --output " + targetPath.Quotes + " " + sourcePath.Quotes
            End If
        End If

        Return Macro.Solve(ret.Trim)
    End Function

    Function GetPartition(params As x264Params) As String
        Dim l As New List(Of String)

        If params.PartitionP8x8.Value Then l.Add("p8x8")
        If params.PartitionB8x8.Value Then l.Add("b8x8")
        If params.PartitionI4x4.Value Then l.Add("i4x4")
        If params.PartitionP4x4.Value Then l.Add("p4x4")
        If params.PartitionI8x8.Value Then l.Add("i8x8")

        If l.Count = 0 Then
            Return " --partitions none"
        ElseIf l.Count = 5 Then
            Return " --partitions all"
        Else
            Return " --partitions " + String.Join(",", l.ToArray)
        End If
    End Function

    Overrides Property QualityMode() As Boolean
        Get
            Return Params.Mode.Value = x264Mode.SingleQuant OrElse Params.Mode.Value = x264Mode.SingleCRF
        End Get
        Set(Value As Boolean)
        End Set
    End Property

    Sub SetMuxer()
        Select Case CType(Params.Device.Value, x264DeviceMode)
            Case x264DeviceMode.DivXPlus
                Muxer = New DivXPluxMuxer()
            Case x264DeviceMode.iPhone
                Muxer = New MP4Muxer("MP4 for iPod/iPhone") With {.AdditionalSwitches = "-ipod"}
            Case x264DeviceMode.PlayStation,
                 x264DeviceMode.Xbox,
                 x264DeviceMode.iPad,
                 x264DeviceMode.iPhone

                Muxer = New MP4Muxer("MP4")
            Case Else
                Muxer = New MkvMuxer()
        End Select
    End Sub

    Overrides Function CreateEditControl() As Control
        Return New x264Control(Me) With {.Dock = DockStyle.Fill}
    End Function
End Class

<Serializable()>
Public Class x264Params
    Implements ISerializable

    Public AdaptiveDCT As New SettingBag(Of Boolean)(True)
    Public AddAll As New SettingBag(Of String)("")
    Public AQMode As New SettingBag(Of Integer)(1)
    Public AQStrengthV2 As New SettingBag(Of Single)(1)
    Public BAdapt As New SettingBag(Of Integer)(x264BAdaptMode.Fast)
    Public BPyramidMode As New SettingBag(Of Integer)(x264BPyramidMode.Normal)
    Public BFrames As New SettingBag(Of Integer)(3)
    Public BFramesBias As New SettingBag(Of Integer)(0)
    Public CABAC As New SettingBag(Of Boolean)(True)
    Public ChromaMe As New SettingBag(Of Boolean)(True)
    Public DctDecimate As New SettingBag(Of Boolean)(True)
    Public DeblockAlpha As New SettingBag(Of Integer)(0)
    Public DeblockBeta As New SettingBag(Of Integer)(0)
    Public DirectMode As New SettingBag(Of Integer)(x264DirectMode.Spatial)
    Public FastPSkip As New SettingBag(Of Boolean)(True)
    Public GOPSizeMax As New SettingBag(Of Integer)(250)
    Public GOPSizeMin As New SettingBag(Of Integer)(25)
    Public IPRatio As New SettingBag(Of Single)(1.4)
    Public Level As New SettingBag(Of Integer)(x264LevelMode.Unrestricted)
    Public Deblock As New SettingBag(Of Boolean)(True)
    Public MEMethod As New SettingBag(Of Integer)(x264MeMethodMode.hex)
    Public MeRange As New SettingBag(Of Integer)(16)
    Public MixedRefs As New SettingBag(Of Boolean)(True)
    Public Mode As New SettingBag(Of Integer)(x264Mode.SingleCRF)
    Public PartitionB8x8 As New SettingBag(Of Boolean)(True)
    Public PartitionI4x4 As New SettingBag(Of Boolean)(True)
    Public PartitionI8x8 As New SettingBag(Of Boolean)(True)
    Public PartitionP4x4 As New SettingBag(Of Boolean)(False)
    Public PartitionP8x8 As New SettingBag(Of Boolean)(True)
    Public PBRatio As New SettingBag(Of Single)(1.3)
    Public Preset As New SettingBag(Of Integer)(x264PresetMode.Medium)
    Public Profile As New SettingBag(Of Integer)(x264ProfileMode.High)
    Public Progress As New SettingBag(Of Boolean)(True)
    Public PSNR As New SettingBag(Of Boolean)(False)
    Public PsyRD As New SettingBag(Of Single)(1.0)
    Public PsyTrellis As New SettingBag(Of Single)(0.0)
    Public QComp As New SettingBag(Of Single)(0.6)
    Public QPMin As New SettingBag(Of Integer)(10)
    Public Quant As New SettingBag(Of Single)(22)
    Public QuantCompCheck As New SettingBag(Of Integer)(18)
    Public RefFrames As New SettingBag(Of Integer)(3)
    Public SceneCut As New SettingBag(Of Integer)(40)
    Public SlowFirstpass As New SettingBag(Of Boolean)(False)
    Public SSIM As New SettingBag(Of Boolean)(False)
    Public SubME As New SettingBag(Of Integer)(x264SubMEMode.d7)
    Public ThreadInput As New SettingBag(Of Boolean)(True)
    Public Threads As New SettingBag(Of Integer)(0)
    Public Trellis As New SettingBag(Of Integer)(x264TrellisMode.FinalMB)
    Public Tune As New SettingBag(Of Integer)(x264TuneMode.Disabled)
    Public TurboAdd As New SettingBag(Of String)("")
    Public TurboRemove As New SettingBag(Of String)("")
    Public VBVBufSize As New SettingBag(Of Integer)(0)
    Public VBVInit As New SettingBag(Of Single)(0.9)
    Public VBVMaxRate As New SettingBag(Of Integer)(0)
    Public MbTree As New SettingBag(Of Boolean)(True)
    Public RcLookahead As New SettingBag(Of Integer)(40)
    Public NoiseReduction As New SettingBag(Of Integer)(0)
    Public Aud As New SettingBag(Of Boolean)(False)
    Public Slices As New SettingBag(Of Integer)(0)
    Public Psy As New SettingBag(Of Boolean)(True)

    Public Device As New SettingBag(Of Integer)(x264DeviceMode.Disabled)
    Public WeightB As New SettingBag(Of Boolean)(True)
    Public WeightP As New SettingBag(Of Integer)(2)
    Public NalHrdMode As New SettingBag(Of Integer)(x264NalHrdMode.None)

    Public Overscan As New SettingBag(Of Integer)(0)
    Public Videoformat As New SettingBag(Of Integer)(0)
    Public Fullrange As New SettingBag(Of Integer)(0)
    Public Colorprim As New SettingBag(Of Integer)(0)
    Public Transfer As New SettingBag(Of Integer)(0)
    Public Colormatrix As New SettingBag(Of Integer)(0)

    Public PicStruct As New SettingBag(Of Boolean)(False)
    Public Chromaloc As New SettingBag(Of Integer)(0)

    Public OpenGopV2 As New SettingBag(Of Boolean)(False)
    Public BlurayCompat As New SettingBag(Of Boolean)(False)

    Sub New()
    End Sub

    <DebuggerNonUserCode()>
    Sub New(info As SerializationInfo, context As StreamingContext)
        For Each i In Me.GetType.GetFields()
            Try
                i.SetValue(Me, info.GetValue(i.Name, i.FieldType))
            Catch
            End Try
        Next
    End Sub

    Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData
        For Each i In Me.GetType.GetFields()
            info.AddValue(i.Name, i.GetValue(Me))
        Next
    End Sub

    Sub ApplyDefaults(profileTunePreset As x264Params)
        Dim defaults As New x264Params

        Preset.Value = profileTunePreset.Preset.Value
        Profile.Value = profileTunePreset.Profile.Value
        Tune.Value = profileTunePreset.Tune.Value

        Select Case profileTunePreset.Preset.Value
            Case x264PresetMode.Ultrafast
                defaults.RefFrames.Value = 1
                defaults.SceneCut.Value = 0
                defaults.Deblock.Value = False
                defaults.CABAC.Value = False
                defaults.BFrames.Value = 0
                defaults.PartitionB8x8.Value = False
                defaults.PartitionI4x4.Value = False
                defaults.PartitionI8x8.Value = False
                defaults.PartitionP4x4.Value = False
                defaults.PartitionP8x8.Value = False
                defaults.AdaptiveDCT.Value = False
                defaults.MEMethod.Value = x264MeMethodMode.dia
                defaults.SubME.Value = x264SubMEMode.d0
                defaults.AQMode.Value = x264AQMode.Disabled
                defaults.MixedRefs.Value = False
                defaults.Trellis.Value = 0
                defaults.BAdapt.Value = 0
                defaults.MbTree.Value = False
                defaults.WeightP.Value = 0
                defaults.RcLookahead.Value = 0
            Case x264PresetMode.Superfast
                defaults.PartitionB8x8.Value = False
                defaults.PartitionI4x4.Value = True
                defaults.PartitionI8x8.Value = True
                defaults.PartitionP4x4.Value = False
                defaults.PartitionP8x8.Value = False
                defaults.MEMethod.Value = x264MeMethodMode.dia
                defaults.SubME.Value = x264SubMEMode.d1
                defaults.RefFrames.Value = 1
                defaults.MixedRefs.Value = False
                defaults.Trellis.Value = 0
                defaults.MbTree.Value = False
                defaults.WeightP.Value = 0
            Case x264PresetMode.Veryfast
                defaults.MbTree.Value = True
                defaults.MixedRefs.Value = False
                defaults.RefFrames.Value = 1
                defaults.SubME.Value = x264SubMEMode.d2
                defaults.Trellis.Value = 0
                defaults.WeightP.Value = 0
            Case x264PresetMode.Faster
                defaults.MixedRefs.Value = False
                defaults.RefFrames.Value = 2
                defaults.SubME.Value = x264SubMEMode.d4
                defaults.WeightP.Value = 1
            Case x264PresetMode.Fast
                defaults.RefFrames.Value = 2
                defaults.SubME.Value = x264SubMEMode.d6
                defaults.RcLookahead.Value = 30
            Case x264PresetMode.Slow
                defaults.MEMethod.Value = x264MeMethodMode.umh
                defaults.SubME.Value = x264SubMEMode.d8
                defaults.RefFrames.Value = 5
                defaults.BAdapt.Value = x264BAdaptMode.Optimal
                defaults.DirectMode.Value = x264DirectMode.Auto
                defaults.RcLookahead.Value = 50
            Case x264PresetMode.Slower
                defaults.MEMethod.Value = x264MeMethodMode.umh
                defaults.SubME.Value = x264SubMEMode.d9
                defaults.RefFrames.Value = 8
                defaults.BAdapt.Value = x264BAdaptMode.Optimal
                defaults.DirectMode.Value = x264DirectMode.Auto
                defaults.PartitionB8x8.Value = True
                defaults.PartitionI4x4.Value = True
                defaults.PartitionI8x8.Value = True
                defaults.PartitionP4x4.Value = True
                defaults.PartitionP8x8.Value = True
                defaults.Trellis.Value = 2
                defaults.RcLookahead.Value = 60
            Case x264PresetMode.Veryslow
                defaults.MEMethod.Value = x264MeMethodMode.umh
                defaults.SubME.Value = x264SubMEMode.d10
                defaults.MeRange.Value = 24
                defaults.RefFrames.Value = 16
                defaults.BAdapt.Value = x264BAdaptMode.Optimal
                defaults.DirectMode.Value = x264DirectMode.Auto
                defaults.PartitionB8x8.Value = True
                defaults.PartitionI4x4.Value = True
                defaults.PartitionI8x8.Value = True
                defaults.PartitionP4x4.Value = True
                defaults.PartitionP8x8.Value = True
                defaults.Trellis.Value = 2
                defaults.BFrames.Value = 8
                defaults.RcLookahead.Value = 60
            Case x264PresetMode.Placebo
                defaults.MEMethod.Value = x264MeMethodMode.tesa
                defaults.SubME.Value = x264SubMEMode.d10
                defaults.MeRange.Value = 24
                defaults.RefFrames.Value = 16
                defaults.BAdapt.Value = x264BAdaptMode.Optimal
                defaults.DirectMode.Value = x264DirectMode.Auto
                defaults.PartitionB8x8.Value = True
                defaults.PartitionI4x4.Value = True
                defaults.PartitionI8x8.Value = True
                defaults.PartitionP4x4.Value = True
                defaults.PartitionP8x8.Value = True
                defaults.FastPSkip.Value = False
                defaults.Trellis.Value = 2
                defaults.BFrames.Value = 16
                defaults.RcLookahead.Value = 60
        End Select

        Select Case profileTunePreset.Tune.Value
            Case x264TuneMode.Film
                defaults.DeblockAlpha.Value = -1
                defaults.DeblockBeta.Value = -1
                defaults.PsyTrellis.Value = 0.15
            Case x264TuneMode.Animation
                defaults.RefFrames.Value = If(defaults.RefFrames.Value > 1, Math.Min(defaults.RefFrames.Value * 2, 16), 1)
                defaults.DeblockAlpha.Value = 1
                defaults.DeblockBeta.Value = 1
                defaults.PsyRD.Value = 0.4
                defaults.AQStrengthV2.Value = 0.6
                defaults.BFrames.Value = Math.Min(defaults.BFrames.Value + 2, 16)
            Case x264TuneMode.Grain
                defaults.DeblockAlpha.Value = -2
                defaults.DeblockBeta.Value = -2
                defaults.PsyTrellis.Value = 0.25
                defaults.DctDecimate.Value = False
                defaults.PBRatio.Value = 1.1
                defaults.IPRatio.Value = 1.1
                defaults.AQStrengthV2.Value = 0.5
                'param->analyse.i_luma_deadzone[0] = 6;
                'param->analyse.i_luma_deadzone[1] = 6;
                defaults.QComp.Value = 0.8
            Case x264TuneMode.PSNR
                defaults.AQMode.Value = x264AQMode.Disabled
                defaults.Psy.Value = False
            Case x264TuneMode.SSIM
                defaults.AQMode.Value = x264AQMode.AutoVariance
                defaults.Psy.Value = False
            Case x264TuneMode.Fastdecode
                defaults.Deblock.Value = False
                defaults.CABAC.Value = False
                defaults.WeightB.Value = False
                defaults.WeightP.Value = 0
                defaults.Trellis.Value = x264TrellisMode.Disabled
            Case x264TuneMode.Stillimage
                defaults.AQStrengthV2.Value = 1.2
                defaults.DeblockAlpha.Value = -3
                defaults.DeblockBeta.Value = -3
                defaults.PsyRD.Value = 2
                defaults.PsyTrellis.Value = 0.7
            Case x264TuneMode.Zerolatency
                defaults.BFrames.Value = 0
                defaults.MbTree.Value = False
                defaults.RcLookahead.Value = 0
        End Select

        Select Case profileTunePreset.Profile.Value
            Case x264ProfileMode.Baseline
                defaults.AdaptiveDCT.Value = False
                defaults.PartitionI8x8.Value = False
                defaults.CABAC.Value = False
                defaults.Trellis.Value = x264TrellisMode.Disabled

                'subme 10 requires trellis
                If defaults.SubME.Value = x264SubMEMode.d10 Then
                    defaults.SubME.Value = x264SubMEMode.d9
                End If

                'param->i_cqm_preset = X264_CQM_FLAT;
                defaults.BFrames.Value = 0
                defaults.WeightP.Value = 0
            Case x264ProfileMode.Main
                defaults.AdaptiveDCT.Value = False
                defaults.PartitionI8x8.Value = False
                'param->i_cqm_preset = X264_CQM_FLAT;
        End Select

        AdaptiveDCT.Value = defaults.AdaptiveDCT.Value
        AQMode.Value = defaults.AQMode.Value
        AQStrengthV2.Value = defaults.AQStrengthV2.Value
        BAdapt.Value = defaults.BAdapt.Value
        BFrames.Value = defaults.BFrames.Value
        WeightB.Value = defaults.WeightB.Value
        CABAC.Value = defaults.CABAC.Value
        DctDecimate.Value = defaults.DctDecimate.Value
        Deblock.Value = defaults.Deblock.Value
        DeblockAlpha.Value = defaults.DeblockAlpha.Value
        DeblockBeta.Value = defaults.DeblockBeta.Value
        DirectMode.Value = defaults.DirectMode.Value
        FastPSkip.Value = defaults.FastPSkip.Value
        IPRatio.Value = defaults.IPRatio.Value
        MbTree.Value = defaults.MbTree.Value
        MEMethod.Value = defaults.MEMethod.Value
        MeRange.Value = defaults.MeRange.Value
        MixedRefs.Value = defaults.MixedRefs.Value
        PartitionB8x8.Value = defaults.PartitionB8x8.Value
        PartitionI4x4.Value = defaults.PartitionI4x4.Value
        PartitionI8x8.Value = defaults.PartitionI8x8.Value
        PartitionP4x4.Value = defaults.PartitionP4x4.Value
        PartitionP8x8.Value = defaults.PartitionP8x8.Value
        PBRatio.Value = defaults.PBRatio.Value
        PsyRD.Value = defaults.PsyRD.Value
        PsyTrellis.Value = defaults.PsyTrellis.Value
        QComp.Value = defaults.QComp.Value
        RcLookahead.Value = defaults.RcLookahead.Value
        RefFrames.Value = defaults.RefFrames.Value
        SceneCut.Value = defaults.SceneCut.Value
        SubME.Value = defaults.SubME.Value
        Trellis.Value = defaults.Trellis.Value
        WeightP.Value = defaults.WeightP.Value
    End Sub

    Sub ApplyDeviceSettings()
        Dim temp As New x264Params
        temp.ApplyDefaults(Me)

        Select Case CType(Device.Value, x264DeviceMode)
            Case x264DeviceMode.iPhone
                temp.Profile.Value = x264ProfileMode.Main

                If temp.RefFrames.Value > 7 Then
                    temp.RefFrames.Value = 7
                End If
            Case x264DeviceMode.iPad
                temp.Profile.Value = x264ProfileMode.High
            Case x264DeviceMode.PlayStation, x264DeviceMode.Xbox
                temp.Profile.Value = x264ProfileMode.High
                temp.Level.Value = x264LevelMode.L4
                temp.VBVBufSize.Value = 25000
                temp.VBVMaxRate.Value = 25000

                If temp.RefFrames.Value > 3 Then
                    temp.RefFrames.Value = 3
                End If

                If temp.BFrames.Value > 3 Then
                    temp.BFrames.Value = 3
                End If

                temp.Aud.Value = True
            Case x264DeviceMode.DivXPlus
                temp.Profile.Value = x264ProfileMode.High
                temp.Level.Value = x264LevelMode.L4
                temp.VBVBufSize.Value = 25000
                temp.VBVMaxRate.Value = 20000

                If temp.BFrames.Value > 3 Then
                    temp.BFrames.Value = 3
                End If

                temp.GOPSizeMax.Value = 4 * If(p.SourceFrameRate = 0, 25, CInt(p.SourceFrameRate))
            Case x264DeviceMode.BluRay
                temp.Level.Value = x264LevelMode.L41
                temp.VBVBufSize.Value = 30000
                temp.VBVMaxRate.Value = 40000
                temp.Slices.Value = 4
                temp.Aud.Value = True
                temp.NalHrdMode.Value = x264NalHrdMode.VBR
                temp.BlurayCompat.Value = True

                If p.TargetHeight = 1080 Then
                    If temp.RefFrames.Value > 4 Then
                        temp.RefFrames.Value = 4
                    End If
                Else
                    If temp.RefFrames.Value > 6 Then
                        temp.RefFrames.Value = 6
                    End If
                End If

                If temp.BFrames.Value > 3 Then
                    temp.BFrames.Value = 3
                End If

                If temp.BPyramidMode.Value = x264BPyramidMode.Normal Then
                    temp.BPyramidMode.Value = x264BPyramidMode.Strict
                End If

                If temp.GOPSizeMax.Value > 48 Then
                    temp.GOPSizeMax.Value = 48
                End If
        End Select

        Profile.Value = temp.Profile.Value
        RefFrames.Value = temp.RefFrames.Value
        Level.Value = temp.Level.Value
        BFrames.Value = temp.BFrames.Value
        VBVMaxRate.Value = temp.VBVMaxRate.Value
        VBVBufSize.Value = temp.VBVBufSize.Value
        GOPSizeMax.Value = temp.GOPSizeMax.Value
        Aud.Value = temp.Aud.Value
        Slices.Value = temp.Slices.Value
        NalHrdMode.Value = temp.NalHrdMode.Value
        BPyramidMode.Value = temp.BPyramidMode.Value
        BlurayCompat.Value = temp.BlurayCompat.Value
    End Sub
End Class

Public Enum x264Mode
    <DispName("Bitrate")>
    SingleBitrate
    <DispName("Quantizer")>
    SingleQuant
    <DispName("Quality")>
    SingleCRF
    <DispName("Two Pass")>
    TwoPass
    <DispName("Three Pass")>
    ThreePass
End Enum

Public Enum x264TrellisMode
    Disabled
    <DispName("Final MB")>
    FinalMB
    Always
End Enum

Public Enum x264SubMEMode
    <DispName("0: Fullpel only")>
    d0
    <DispName("1: SAD mode decision, one qpel iteration")>
    d1
    <DispName("2: SATD mode decision")>
    d2
    <DispName("3: Progressively more qpel")>
    d3
    <DispName("4: Progressively more qpel")>
    d4
    <DispName("5: Progressively more qpel")>
    d5
    <DispName("6: RD mode decision for I/P-frames")>
    d6
    <DispName("7: RD mode decision for all frames")>
    d7
    <DispName("8: RD refinement for I/P-frames")>
    d8
    <DispName("9: RD refinement for all frames")>
    d9
    <DispName("10: QP-RD")>
    d10
    <DispName("11: Full RD")>
    d11
End Enum

Public Enum x264AQMode
    Disabled
    <DispName("Variance AQ (complexity mask)")> Variance
    <DispName("Auto-variance AQ")> AutoVariance
    <DispName("Auto-variance AQ with bias to dark scenes")> AutoVarianceBias
End Enum

Public Enum x264DirectMode
    None
    Spatial
    Temporal
    Auto
End Enum

Public Enum x264MeMethodMode
    <DispName("Diamond")>
    dia
    <DispName("Hexagon")>
    hex
    <DispName("Multi Hex")>
    umh
    <DispName("Exhaustive")>
    esa
    <DispName("SATD Exhaustive")>
    tesa
End Enum

Public Enum x264DeviceMode
    Disabled
    <DispName("DivX Plus")>
    DivXPlus
    <DispName("Blu-ray")>
    BluRay
    iPad
    iPhone
    PlayStation
    Xbox
End Enum

Public Enum x264LevelMode
    <DispName("1")> L1
    <DispName("1.1")> L11
    <DispName("1.2")> L12
    <DispName("1.3")> L13
    <DispName("2")> L2
    <DispName("2.1")> L21
    <DispName("2.2")> L22
    <DispName("3")> L3
    <DispName("3.1")> L31
    <DispName("3.2")> L32
    <DispName("4")> L4
    <DispName("4.1")> L41
    <DispName("4.2")> L42
    <DispName("5")> L5
    <DispName("5.1")> L51
    <DispName("5.2")> L52
    Unrestricted
End Enum

Public Enum x264BAdaptMode
    Disabled
    Fast
    Optimal
End Enum

Public Enum x264ProfileMode
    Baseline
    Main
    High
End Enum

Public Enum x264BPyramidMode
    None
    Strict
    Normal
End Enum

Public Enum x264NalHrdMode
    None = 10
    VBR = 20
    CBR = 30
End Enum

Public Enum x264WeightpMode
    Disabled
    Blind
    Smart
End Enum

Public Enum x264PresetMode
    <DispName("Ultra Fast")> Ultrafast
    <DispName("Super Fast")> Superfast
    <DispName("Very Fast")> Veryfast
    Faster
    Fast
    Medium
    Slow
    Slower
    <DispName("Very Slow")> Veryslow
    Placebo
End Enum

Public Enum x264TuneMode
    Disabled
    Film
    Animation
    Grain
    PSNR
    SSIM
    <DispName("Fast Decode")> Fastdecode
    <DispName("Zero Latency")> Zerolatency
    <DispName("Still Image")> Stillimage
End Enum

Public Enum x264OverscanMode
    <DispName("Undefined")> Undef
    Show
    Crop
End Enum

Public Enum x264VideoformatMode
    <DispName("Undefined")> Undef
    Component
    PAL
    NTSC
    SECAM
    MAC
End Enum

Public Enum x264FullrangeMode
    Off
    [On]
End Enum

Public Enum x264ColorprimMode
    <DispName("Undefined")> Undef
    bt709
    bt470m
    bt470bg
    smpte170m
    smpte240m
    film
End Enum

Public Enum x264TransferMode
    <DispName("Undefined")> Undef
    bt709
    bt470m
    bt470bg
    linear
    log100
    log316
    smpte170m
    smpte240m
End Enum

Public Enum x264ColormatrixMode
    <DispName("Undefined")> Undef
    bt709
    fcc
    bt470bg
    smpte170m
    smpte240m
    GBR
    YCgCo
End Enum

Public Enum x264OpenGopMode
    None
    Normal
    Bluray
End Enum