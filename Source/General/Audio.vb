
Imports System.Text
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.Runtime.Remoting

Public Class Audio
    Shared Sub Process(ap As AudioProfile)
        If Not File.Exists(ap.File) OrElse TypeOf ap Is NullAudioProfile Then
            Exit Sub
        End If

        If Not Directory.Exists(p.TempDir) Then
            p.TempDir = ap.File.Dir
        End If

        If ap.File <> p.SourceFile Then
            Log.Write("Media Info Audio Source " & (ap.GetTrackIndex + 1), MediaInfo.GetSummary(ap.File))
        End If

        Dim normalized = False
        Dim isGUIAP = TypeOf ap Is GUIAudioProfile
        Dim gap = TryCast(ap, GUIAudioProfile)

        If isGUIAP AndAlso gap.ContainsCommand("ffmpeg") Then
            normalized = gap.NormalizeFF()
        End If

        Dim extractCore = isGUIAP AndAlso gap.ExtractCore

        If ap.Decoder <> AudioDecoderMode.Automatic AndAlso Not extractCore Then
            Convert(ap)
        End If

        If ap.HasStream Then
            Dim cutting = p.Ranges.Count > 0

            Dim directMux = TypeOf ap Is MuxAudioProfile AndAlso
                p.VideoEncoder.Muxer.IsSupported(ap.Stream.Ext) AndAlso
                p.VideoEncoder.Muxer.IsSupported(ap.File.Ext) AndAlso Not cutting

            Dim trackIsSupportedButNotContainer = TypeOf ap Is MuxAudioProfile AndAlso
                p.VideoEncoder.Muxer.IsSupported(ap.Stream.Ext) AndAlso
                Not p.VideoEncoder.Muxer.IsSupported(ap.File.Ext)

            If ((cutting OrElse Not ap.IsInputSupported) AndAlso Not directMux) OrElse
                trackIsSupportedButNotContainer Then

                Select Case ap.File.ExtFull
                    Case ".mkv", ".webm"
                        mkvDemuxer.Demux(ap.File, {ap.Stream}, Nothing, ap, p, False, False, True, "Demux MKV Audio " & (ap.GetTrackIndex + 1), True)
                    Case ".mp4"
                        MP4BoxDemuxer.DemuxAudio(ap.File, ap.Stream, ap, p, True)
                    Case Else
                        If p.Script.GetFilter("Source").Script.ToLowerInvariant.Contains("directshowsource") AndAlso TypeOf ap IsNot MuxAudioProfile Then
                            ConvertDirectShowSource(ap)
                        ElseIf Not ap.File.Ext = "m2ts" Then
                            ffmpegDemuxer.DemuxAudio(ap.File, ap.Stream, ap, p, True)
                        End If
                End Select
            End If
        End If

        Cut(ap)

        If TypeOf ap IsNot MuxAudioProfile AndAlso ap.NeedConvert Then
            Convert(ap, True)
        End If
    End Sub

    Shared Function GetBaseNameForStream(filePath As String, stream As AudioStream) As String
        Dim base = If(p.TempDir?.DirExists() AndAlso New DirectoryInfo(p.TempDir).Name.EndsWith("_temp") AndAlso filePath.Base.StartsWithEx(p.SourceFile.Base),
            filePath.Base.Substring(p.SourceFile.Base.Length),
            filePath.Base)

        Dim ret = base + " ID" & (stream.Index + 1)

        If stream.Language.TwoLetterCode <> "iv" Then
            ret += $"_[{stream.Language.Name}] ({stream.Language.EnglishName})"
        End If

        If stream.SBR Then
            ret += " SBR"
        End If

        If stream.Delay <> 0 Then
            ret += " " & stream.Delay & "ms"
        End If

        If stream.Title <> "" Then
            Dim pathLength = (p.TempDir + ret).Length
            Dim leftLength = GlobalClass.MAX_PATH - 10 - pathLength
            If leftLength > 3 Then
                ret += " {" + stream.Title.Shorten(leftLength).EscapeIllegalFileSysChars + "}"
            End If
        End If

        Return ret.Trim
    End Function

    Shared Sub Convert(ap As AudioProfile, Optional force As Boolean = False)
        If Not force AndAlso ap.File.Ext = ap.ConvertExt Then Exit Sub

        If ap.File.Ext = "avs" Then
            Dim outPath = ap.File.DirAndBase + "." + ap.ConvertExt
            Dim args = "-i " + ap.File.Escape + " -y -hide_banner " + outPath.Escape

            Using proc As New Proc
                proc.Header = "AVS to " + outPath.Ext.ToUpperInvariant
                proc.SkipStrings = {"frame=", "size="}
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.ffmpeg
                proc.Arguments = args
                proc.Start()
            End Using

            If g.FileExists(outPath) Then
                ap.File = outPath
                Exit Sub
            End If
        End If

        Dim isGUIAP = TypeOf ap Is GUIAudioProfile
        Dim gap = TryCast(ap, GUIAudioProfile)
        Dim renormalize = isGUIAP AndAlso gap.Params.Normalize AndAlso gap.GetEncoder() <> GuiAudioEncoder.deezy AndAlso
                                (gap.Params.ffmpegNormalizeMode = ffmpegNormalizeMode.dynaudnorm OrElse
                                (gap.Params.ChannelsMode <> ChannelsMode.Original OrElse gap.GetEncoder() = GuiAudioEncoder.opusenc))

        Select Case ap.Decoder
            Case AudioDecoderMode.ffmpeg, AudioDecoderMode.Automatic
                ConvertFF(ap)
            Case AudioDecoderMode.FFAudioSource
                ConvertFFAudioSource(ap)
            Case AudioDecoderMode.eac3to
                ConvertEac3to(ap)
            Case AudioDecoderMode.NicAudio
                ConvertNicAudio(ap)
            Case AudioDecoderMode.DirectShow
                ConvertDirectShowSource(ap)
        End Select

        If p.Script.GetFilter("Source").Script.ToLowerInvariant.Contains("directshowsource") Then
            ConvertDirectShowSource(ap)
        End If

        gap.Params.Normalize = renormalize
        gap.Params.ffmpegNormalizeMode = If(gap.Params.ffmpegNormalizeMode = ffmpegNormalizeMode.dynaudnorm, ffmpegNormalizeMode.volumedetect, gap.Params.ffmpegNormalizeMode)

        ConvertFF(ap, renormalize)
        ConvertEac3to(ap)
        ConvertDirectShowSource(ap)
    End Sub

    Shared Sub CutNicAudio(ap As AudioProfile)
        If ap.File.Contains("_cut_") Then
            Exit Sub
        End If

        If Not FileTypes.NicAudioInput.Contains(ap.File.Ext) Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        ap.Delay = 0
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        Dim wavPath = Path.Combine(p.TempDir, ap.File.Base + "_cut_na.wav")
        d.Path = Path.Combine(p.TempDir, ap.File.Base + "_cut_na.avs")
        d.Filters.Insert(1, New VideoFilter(GetNicAudioCode(ap)))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " + d.Path.Escape + " -y -hide_banner " + wavPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) + BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub ConvertNicAudio(ap As AudioProfile)
        If ap.File.Ext = ap.ConvertExt Then
            Exit Sub
        End If

        If Not FileTypes.NicAudioInput.Contains(ap.File.Ext) Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        ap.Delay = 0
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        d.RemoveFilter("Cutting")
        Dim outPath = Path.Combine(p.TempDir, ap.File.Base + "_DecodeNicAudio." + ap.ConvertExt)
        d.Path = Path.Combine(p.TempDir, ap.File.Base + "_DecodeNicAudio.avs")
        d.Filters.Insert(1, New VideoFilter(GetNicAudioCode(ap)))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " + d.Path.Escape + " -y -hide_banner " + outPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) + BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub Cut(ap As AudioProfile)
        If p.Ranges.Count = 0 OrElse
            Not File.Exists(ap.File) OrElse
            TypeOf p.VideoEncoder Is NullEncoder OrElse
            ap.File.Contains("_cut_.") Then

            Exit Sub
        End If

        Select Case p.CuttingMode
            Case CuttingMode.mkvmerge
                CutMkvmerge(ap)
            Case CuttingMode.NicAudio
                If FileTypes.NicAudioInput.Contains(ap.File.Ext) AndAlso TypeOf ap IsNot MuxAudioProfile Then
                    CutNicAudio(ap)
                Else
                    CutMkvmerge(ap)
                End If
            Case CuttingMode.DirectShow
                If TypeOf ap IsNot MuxAudioProfile Then
                    CutDirectShowSource(ap)
                Else
                    CutMkvmerge(ap)
                End If
        End Select
    End Sub

    Shared Function GetNicAudioCode(ap As AudioProfile) As String
        Select Case ap.File.Ext
            Case "ac3"
                Return "AudioDub(last, NicAC3Source(""" + ap.File + """, Channels = " & ap.Channels & "))"
            Case "mpa", "mp2", "mp3"
                Return "AudioDub(last, NicMPASource(""" + ap.File + """))"
            Case "wav"
                Return "AudioDub(last, RaWavSource(""" + ap.File + """, Channels = " & ap.Channels & "))"
        End Select
    End Function

    Shared Sub ConvertEac3to(ap As AudioProfile)
        If ap.File.Ext = ap.ConvertExt Then
            Exit Sub
        End If

        If Not FileTypes.eac3toInput.Contains(ap.File.Ext) Then
            Exit Sub
        End If

        Dim outPath = Path.Combine(p.TempDir, ap.File.Base + "." + ap.ConvertExt)
        Dim args = ap.File.Escape + " " + outPath.Escape

        If ap.Channels = 6 Then
            args += " -down6"
        ElseIf ap.Channels = 2 Then
            args += " -down2"
        End If

        If TypeOf ap Is GUIAudioProfile Then
            Dim gap = DirectCast(ap, GUIAudioProfile)

            If gap.Params.Normalize Then
                args += " -normalize"
                gap.Params.Normalize = True
            End If
        End If

        args += " -simple -progressnumbers"

        Using proc As New Proc
            proc.Header = "Convert " + ap.File.Ext.ToUpperInvariant + " to " + outPath.Ext.ToUpperInvariant + " " & (ap.GetTrackIndex + 1)
            proc.Package = Package.eac3to
            proc.Arguments = args
            proc.TrimChars = {"-"c, " "c}
            proc.SkipStrings = {"process:", "analyze:"}
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub ConvertFF(ap As AudioProfile, Optional force As Boolean = False)
        If Not force AndAlso ap.File.Ext = ap.ConvertExt Then Exit Sub

        Dim gap = TryCast(ap, GUIAudioProfile)

        gap?.NormalizeFF()

        Dim base = ap.File.Base

        If Not base.Contains(ap.GetTrackID) Then
            base += ap.GetTrackID
        End If

        Dim outPath = Path.Combine(p.TempDir, base + "." + ap.ConvertExt)

        If ap.File = outPath Then
            outPath += "." + ap.ConvertExt
        End If

        Dim cancel = True
        Dim args = "-y -hide_banner -i " + ap.File.Escape

        If ap.Stream IsNot Nothing Then
            args += " -map 0:" & ap.Stream.StreamOrder
        End If

        If ap.Gain <> 0 Then
            args += " -af volume=" + ap.Gain.ToInvariantString + "dB"
            cancel = False
        End If

        If gap?.Params.Normalize Then
            If gap.Params.ffmpegNormalizeMode = ffmpegNormalizeMode.dynaudnorm Then
                args += " " + Audio.GetDynAudNormArgs(gap.Params)
                cancel = False
            ElseIf gap.Params.ffmpegNormalizeMode = ffmpegNormalizeMode.loudnorm Then
                args += " " + Audio.GetLoudNormArgs(gap.Params)
                cancel = False
            End If
        End If

        If gap.Params.CenterOptimizedStereo AndAlso ((gap?.Params.Codec <> AudioCodec.Opus AndAlso gap?.Params.ChannelsMode = ChannelsMode._2) OrElse (gap?.Params.Codec = AudioCodec.Opus AndAlso gap?.Params.OpusencDownmix = OpusDownmix.Stereo)) Then
            args += " -af pan=stereo|c0=c2+0.30*c0+0.30*c4|c1=c2+0.30*c1+0.30*c5"
            gap.Params.ChannelsMode = ChannelsMode.Original
            gap.Params.OpusencDownmix = OpusDownmix.Original
            cancel = False
        ElseIf gap.Params.ChannelsMode <> ChannelsMode.Original Then
            args += " -ac " & CInt(gap.Params.ChannelsMode)
            gap.Params.ChannelsMode = ChannelsMode.Original
            gap.Params.OpusencDownmix = OpusDownmix.Original
        ElseIf gap.Params.Codec = AudioCodec.Opus AndAlso gap.Params.OpusencDownmix <> OpusDownmix.Original Then
            args += " -ac " & CInt(gap.Params.OpusencDownmix)
            gap.Params.ChannelsMode = ChannelsMode.Original
            gap.Params.OpusencDownmix = OpusDownmix.Original
        Else
            If Not force Then
                args += " -ac " & ap.Channels
            End If
            gap.Params.ChannelsMode = ChannelsMode.Original
            gap.Params.OpusencDownmix = OpusDownmix.Original
        End If

        If force AndAlso cancel Then Exit Sub

        If ap.ConvertExt.EqualsAny("wav", "w64") Then
            args += " -c:a pcm_s24le"
        End If

        Dim matches = Regex.Matches(args, " -af ([^ ]+)")
        If matches.Count > 1 Then
            Dim concated = ""
            For i = matches.Count - 1 To 0 Step -1
                Dim match = matches(i)
                concated = $"{match.Groups(1)},{concated}"
                args = args.Remove(match.Index, match.Length)
            Next
            concated = " -af " + concated.Trim(","c)
            args = args.Insert(matches(0).Index, concated)
        End If

        args = Regex.Replace(args, " -af ([^ ]+)", " -af ""$1""")
        args += " " + outPath.Escape

        Using proc As New Proc
            proc.Header = "Convert " + ap.File.Ext.ToUpperInvariant + " to " + outPath.Ext.ToUpperInvariant + " " & (ap.GetTrackIndex + 1)
            proc.SkipStrings = {"frame=", "size="}
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            If outPath.StartsWith("\\?\") Then
                outPath = outPath.Substring(4)
            End If

            ap.Gain = 0
            gap.Params.CenterOptimizedStereo = False
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Function GetLoudNormArgs(params As GUIAudioProfile.Parameters) As String
        Return "-af loudnorm=" +
            "I=" + params.ffmpegLoudnormIntegrated.ToInvariantString +
            ":TP=" + params.ffmpegLoudnormTruePeak.ToInvariantString +
            ":LRA=" + params.ffmpegLoudnormLRA.ToInvariantString +
            ":measured_I=" + params.ffmpegLoudnormIntegratedMeasured.ToInvariantString +
            ":measured_TP=" + params.ffmpegLoudnormTruePeakMeasured.ToInvariantString +
            ":measured_LRA=" + params.ffmpegLoudnormLraMeasured.ToInvariantString +
            ":measured_thresh=" + params.ffmpegLoudnormThresholdMeasured.ToInvariantString +
            ":print_format=summary"
    End Function

    Shared Function GetDynAudNormArgs(params As GUIAudioProfile.Parameters) As String
        Dim ret = ""

        If params.ffmpegDynaudnormF <> 500 Then ret += ":f=" + params.ffmpegDynaudnormF.ToInvariantString
        If params.ffmpegDynaudnormG <> 31 Then ret += ":g=" + params.ffmpegDynaudnormG.ToInvariantString
        If params.ffmpegDynaudnormP <> 0.95 Then ret += ":p=" + params.ffmpegDynaudnormP.ToInvariantString
        If params.ffmpegDynaudnormM <> 10 Then ret += ":m=" + params.ffmpegDynaudnormM.ToInvariantString
        If params.ffmpegDynaudnormR <> 0 Then ret += ":r=" + params.ffmpegDynaudnormR.ToInvariantString
        If params.ffmpegDynaudnormS <> 0 Then ret += ":s=" + params.ffmpegDynaudnormS.ToInvariantString
        If Not params.ffmpegDynaudnormN Then ret += ":n=false"
        If params.ffmpegDynaudnormC Then ret += ":c=true"
        If params.ffmpegDynaudnormB Then ret += ":b=true"

        Return If(ret = "", "-af dynaudnorm", "-af dynaudnorm=" + ret.Trim(":"c))
    End Function

    Shared Sub ConvertDirectShowSource(ap As AudioProfile, Optional useFlac As Boolean = False)
        If ap.File.Ext = ap.ConvertExt Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        ap.Delay = 0
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        d.RemoveFilter("Cutting")
        Dim outPath = Path.Combine(p.TempDir, ap.File.Base + "_convDSS." + ap.ConvertExt)
        d.Path = Path.Combine(p.TempDir, ap.File.Base + "_DecDSS.avs")
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,DirectShowSource(""" + ap.File + """, video=false))"))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " + d.Path.Escape + " -y -hide_banner " + outPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) + BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub ConvertFFAudioSource(ap As AudioProfile)
        If ap.File.Ext = ap.ConvertExt Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        ap.Delay = 0
        Dim cachefile = Path.Combine(p.TempDir, ap.File.Base + ".ffindex")
        g.ffmsindex(ap.File, cachefile)
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        d.RemoveFilter("Cutting")
        Dim outPath = Path.Combine(p.TempDir, ap.File.Base + "_convFFAudioSource." + ap.ConvertExt)
        d.Path = Path.Combine(p.TempDir, ap.File.Base + "_DecodeFFAudioSource.avs")
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,FFAudioSource(""" + ap.File + """, cachefile=""" + cachefile + """))"))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " + d.Path.Escape + " -y -hide_banner " + outPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) + BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub CutDirectShowSource(ap As AudioProfile)
        If ap.File.Contains("_cut_") Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        ap.Delay = 0
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        Dim wavPath = Path.Combine(p.TempDir, ap.File.Base + "_cut_ds.wav")
        d.Path = Path.Combine(p.TempDir, ap.File.Base + "_cut_ds.avs")
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,DirectShowSource(""" + ap.File + """, video=false))"))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " + d.Path.Escape + " -y -hide_banner " + wavPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) + BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub CutFFAudioSource(ap As AudioProfile)
        If ap.File.Contains("_cut_") Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        ap.Delay = 0
        Dim cachefile = Path.Combine(p.TempDir, ap.File.Base + ".ffindex")
        g.ffmsindex(ap.File, cachefile)
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        Dim wavPath = Path.Combine(p.TempDir, ap.File.Base + "_cut_ff.wav")
        d.Path = Path.Combine(p.TempDir, ap.File.Base + "_cut_ff.avs")
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,FFAudioSource(""" + ap.File + """, cachefile=""" + cachefile + """))"))

        If ap.Channels = 2 Then
            d.Filters.Add(New VideoFilter(GetDown2Code))
        End If

        d.Synchronize()

        Dim args = "-i " + d.Path.Escape + " -y -hide_banner " + wavPath.Escape

        Using proc As New Proc
            proc.Header = "AVS to WAV"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog(Macro.Expand(d.GetScript) + BR)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If g.FileExists(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub CutMkvmerge(ap As AudioProfile)
        If ap.File.Contains("_cut_") Then
            Exit Sub
        End If

        If Not Package.AviSynth.VerifyOK(True) Then
            Throw New AbortException
        End If

        Dim base = ap.File.Base

        If Not base.Contains(ap.GetTrackID) Then
            base += ap.GetTrackID
        End If

        Dim aviPath = Path.Combine(p.TempDir, base + "_cut_.avi")
        Dim d = (p.CutFrameCount / p.CutFrameRate).ToString("f9", CultureInfo.InvariantCulture)
        Dim r = p.CutFrameRate.ToString("f9", CultureInfo.InvariantCulture)
        Dim args = $"-f lavfi -i color=c=black:s=16x16:d={d}:r={r} -y -hide_banner -c:v ffv1 -g 1 " + aviPath.Escape

        Using proc As New Proc
            proc.Header = "Create avi file for audio cutting " & (ap.GetTrackIndex + 1)
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog("mkvmerge cannot cut audio without video so an avi file has to be created" + BR2)
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If Not File.Exists(aviPath) Then
            Throw New ErrorAbortException("Error", "Output file missing")
        Else
            Log.WriteLine(MediaInfo.GetSummary(aviPath))
        End If

        Dim mkvPath = Path.Combine(p.TempDir, base + "_cut_.mkv")

        Dim args2 = "-o " + mkvPath.Escape + " " + aviPath.Escape + " " + ap.File.Escape
        args2 += " --split parts-frames:" + p.Ranges.Select(Function(v) v.Start & "-" & (v.End + 1)).Join(",+")
        args2 += " --engage append_and_split_flac --ui-language en"

        Using proc As New Proc
            proc.Header = "Cut Audio " & (ap.GetTrackIndex + 1)
            proc.SkipString = "Progress: "
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.mkvmerge
            proc.Arguments = args2
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using

        Dim fail As Boolean

        If File.Exists(mkvPath) Then
            Log.WriteLine(MediaInfo.GetSummary(mkvPath))
            Dim streams = MediaInfo.GetAudioStreams(mkvPath)

            If streams.Count > 0 Then
                mkvDemuxer.Demux(mkvPath, {streams(0)}, Nothing, ap, p, False, False, True,
                    "Demux cutted MKV " & (ap.GetTrackIndex + 1), False)
            Else
                fail = True
            End If
        Else
            fail = True
        End If

        If fail AndAlso TypeOf ap Is GUIAudioProfile AndAlso Not ap.File.Ext = "wav" Then
            Log.Write("Error", "no output found")
            Convert(ap)

            If ap.File.Ext = "wav" Then
                Cut(ap)
            End If
        End If
    End Sub

    Shared Function GetDown2Code() As String
        Return "Audiochannels() >= 6 ? Down2(last) : last

function Down2(clip a) 
{
	a = ConvertAudioToFloat(a)
	fl = GetChannel(a, 1)
	fr = GetChannel(a, 2)
	c = GetChannel(a, 3)
	lfe = GetChannel(a, 4)
	sl = GetChannel(a, 5)
	sr = GetChannel(a, 6)
	l_sl = MixAudio(fl, sl, 0.2929, 0.2929)
	c_lfe = MixAudio(lfe, c, 0.2071, 0.2071)
	r_sr = MixAudio(fr, sr, 0.2929, 0.2929)
	l = MixAudio(l_sl, c_lfe, 1.0, 1.0)
	r = MixAudio(r_sr, c_lfe, 1.0, 1.0)
	return MergeChannels(l, r)
}"
    End Function

    Shared Function FileExist(fp As String) As Boolean
        Return File.Exists(fp) AndAlso New FileInfo(fp).Length > 500
    End Function

    Shared Function IsEncoderUsed(encoder As GuiAudioEncoder) As Boolean
        If Not p.AudioTracks.NothingOrEmpty() Then
            For Each track In p.AudioTracks
                If IsEncoderUsed(track.AudioProfile, encoder) Then
                    Return True
                End If
            Next
        End If

        If Not p.AudioFiles.NothingOrEmpty Then
            For Each ap In p.AudioFiles
                If IsEncoderUsed(ap, encoder) Then
                    Return True
                End If
            Next
        End If

        Return False
    End Function

    Shared Function IsEncoderUsed(ap As AudioProfile, encoder As GuiAudioEncoder) As Boolean
        If TypeOf ap Is GUIAudioProfile AndAlso ap.File <> "" Then
            Dim gap = DirectCast(ap, GUIAudioProfile)

            If gap.GetEncoder = encoder Then
                Return True
            End If
        End If
    End Function



    Shared Function CommandContains(find As String) As Boolean
        If Not p.AudioTracks.NothingOrEmpty() Then
            For Each track In p.AudioTracks
                If track.AudioProfile.IsUsedAndContainsCommand(find) Then
                    Return True
                End If
            Next
        End If

        If Not p.AudioFiles.NothingOrEmpty Then
            For Each ap In p.AudioFiles
                If ap.IsUsedAndContainsCommand(find) Then
                    Return True
                End If
            Next
        End If

        Return False
    End Function
End Class

Public Enum AudioDecoderMode
    Automatic
    ffmpeg
    eac3to
    NicAudio
    DirectShow
    FFAudioSource
End Enum

Public Enum AudioDecodingMode
    FLAC
    W64
    WAVE
    Pipe
End Enum

Public Enum CuttingMode
    mkvmerge
    DirectShow
    NicAudio
End Enum

Public Enum ffNormalizeMode
    volumedetect
    loudnorm
    dynaudnorm
End Enum

Public Enum ffmpegNormalizeMode
    volumedetect
    loudnorm
    dynaudnorm
End Enum
