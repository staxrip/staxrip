Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Globalization

Imports VB6 = Microsoft.VisualBasic

Public Class Audio
    Shared Sub Process(ap As AudioProfile)
        If Not File.Exists(ap.File) OrElse TypeOf ap Is NullAudioProfile Then Exit Sub
        If Not Directory.Exists(p.TempDir) Then p.TempDir = ap.File.Dir

        If ap.File <> p.SourceFile Then
            Log.WriteHeader("Audio Source File MediaInfo")
            Log.WriteLine(MediaInfo.GetSummary(ap.File))
        End If

        If TypeOf ap Is GUIAudioProfile Then
            Dim gap = DirectCast(ap, GUIAudioProfile)

            If gap.Params.Normalize Then
                Dim cmdl = ap.CommandLines
                If cmdl <> "" AndAlso cmdl.Contains("ffmpeg") OrElse cmdl.Contains("fdkaac") Then SetGain(ap)
            End If
        End If

        If p.ForceAudioConvert Then Convert(ap)

        If ap.HasStream Then
            Dim cutting = p.Ranges.Count > 0

            Dim directMux = TypeOf ap Is MuxAudioProfile AndAlso
                p.VideoEncoder.Muxer.IsSupported(ap.Stream.Extension.TrimStart("."c)) AndAlso
                p.VideoEncoder.Muxer.IsSupported(ap.File.Ext) AndAlso
                Not cutting

            If (cutting OrElse Not ap.IsInputSupported) AndAlso Not directMux Then
                Select Case ap.File.ExtFull
                    Case ".mkv", ".webm"
                        mkvDemuxer.Demux(ap.File, {ap.Stream}, Nothing, ap, p, False, False)
                    Case ".mp4"
                        MP4BoxDemuxer.Demux(ap.File, ap.Stream, ap, p)
                    Case Else
                        If p.Script.GetFilter("Source").Script.ToLower.Contains("directshowsource") AndAlso
                            Not TypeOf ap Is MuxAudioProfile Then

                            ConvertDirectShowSource(ap)
                        ElseIf Not Filepath.GetExtFull(ap.File) = ".m2ts" Then
                            ffmpegDemuxer.DemuxAudio(ap.File, ap.Stream, ap, p)
                        End If
                End Select
            End If
        End If

        Cut(ap)

        If Not TypeOf ap Is MuxAudioProfile AndAlso Not ap.IsInputSupported Then Convert(ap)
    End Sub

    Shared Function GetBaseNameForStream(path As String, stream As AudioStream, Optional shorten As Boolean = False) As String
        Dim ret = If(shorten, path.Base.Shorten(10), path.Base) + " ID" & (stream.Index + 1)

        If stream.Delay <> 0 Then ret += " " & stream.Delay & "ms"
        If stream.Language.TwoLetterCode <> "iv" Then ret += " " + stream.Language.ToString

        If Not shorten AndAlso path.Length < 130 AndAlso stream.Title <> "" AndAlso
            Not stream.Title.ContainsUnicode Then

            ret += " " + stream.Title.Shorten(30)
        End If

        If Not Filepath.IsValidFileSystemName(ret) Then ret = Filepath.RemoveIllegalCharsFromName(ret)

        Return ret
    End Function

    Shared Sub Convert(ap As AudioProfile)
        If ap.File.Ext = ap.ConvertExt Then Exit Sub

        If ap.File.Ext = "avs" Then
            Dim outPath = ap.File.DirAndBase + "." + ap.ConvertExt
            Dim args = "-i " + ap.File.Escape + " -y -hide_banner " + outPath.Escape

            Using proc As New Proc
                proc.Header = "AVS to " + outPath.Ext.ToUpper
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

        Select Case p.AudioConvertMode
            Case AudioConvertMode.ffmpeg
                ConvertFfmpeg(ap)
            Case AudioConvertMode.FFAudioSource
                ConvertFFAudioSource(ap)
            Case AudioConvertMode.eac3to
                ConvertEac3to(ap)
            Case AudioConvertMode.NicAudio
                ConvertNicAudio(ap)
            Case AudioConvertMode.DirectShow
                ConvertDirectShowSource(ap)
        End Select

        If p.Script.GetFilter("Source").Script.ToLower.Contains("directshowsource") Then
            ConvertDirectShowSource(ap)
        End If

        ConvertEac3to(ap)
        ConvertFfmpeg(ap)
        ConvertDirectShowSource(ap)
    End Sub

    Shared Sub CutNicAudio(ap As AudioProfile)
        If ap.File.Contains("_cut_") Then Exit Sub
        If Not FileTypes.NicAudioInput.Contains(ap.File.Ext) Then Exit Sub
        If Not Package.AviSynth.VerifyOK(True) Then Throw New AbortException
        ap.Delay = 0
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        Dim wavPath = p.TempDir + ap.File.Base + "_cut_na.wav"
        d.Path = p.TempDir + ap.File.Base + "_cut_na.avs"
        d.Filters.Insert(1, New VideoFilter(GetNicAudioCode(ap)))
        If ap.Channels = 2 Then d.Filters.Add(New VideoFilter(GetDown2Code))
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
        If ap.File.Ext = ap.ConvertExt Then Exit Sub
        If Not FileTypes.NicAudioInput.Contains(ap.File.Ext) Then Exit Sub
        If Not Package.AviSynth.VerifyOK(True) Then Throw New AbortException
        ap.Delay = 0
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        d.RemoveFilter("Cutting")
        Dim outPath = p.TempDir + ap.File.Base + "_DecodeNicAudio." + ap.ConvertExt
        d.Path = p.TempDir + ap.File.Base + "_DecodeNicAudio.avs"
        d.Filters.Insert(1, New VideoFilter(GetNicAudioCode(ap)))
        If ap.Channels = 2 Then d.Filters.Add(New VideoFilter(GetDown2Code))
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
                If FileTypes.NicAudioInput.Contains(ap.File.Ext) AndAlso
                    Not TypeOf ap Is MuxAudioProfile Then

                    CutNicAudio(ap)
                Else
                    CutMkvmerge(ap)
                End If
            Case CuttingMode.DirectShow
                If Not TypeOf ap Is MuxAudioProfile Then
                    CutDirectShowSource(ap)
                Else
                    CutMkvmerge(ap)
                End If
        End Select
    End Sub

    Shared Function GetNicAudioCode(ap As AudioProfile) As String
        Select Case Filepath.GetExtFull(ap.File)
            Case ".ac3"
                Return "AudioDub(last, NicAC3Source(""" + ap.File + """, Channels = " & ap.Channels & "))"
            Case ".mpa", ".mp2", ".mp3"
                Return "AudioDub(last, NicMPASource(""" + ap.File + """))"
            Case ".wav"
                Return "AudioDub(last, RaWavSource(""" + ap.File + """, Channels = " & ap.Channels & "))"
        End Select
    End Function

    Shared Sub ConvertEac3to(ap As AudioProfile)
        If ap.File.Ext = ap.ConvertExt Then Exit Sub
        If Not FileTypes.eac3toInput.Contains(ap.File.Ext) Then Exit Sub
        Dim outPath = p.TempDir + ap.File.Base + "." + ap.ConvertExt
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
                gap.Params.Normalize = False
            End If
        End If

        args += " -simple -progressnumbers"

        Using proc As New Proc
            proc.Header = "Convert from " + ap.File.Ext.ToUpper + " to " + outPath.Ext.ToUpper
            proc.Package = Package.eac3to
            proc.Arguments = args
            proc.TrimChars = {"-"c, " "c}
            proc.RemoveChars = {VB6.ChrW(8)} 'backspace
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

    Shared Sub ConvertFfmpeg(ap As AudioProfile)
        If ap.File.Ext = ap.ConvertExt Then Exit Sub
        Dim outPath = p.TempDir + ap.File.Base + "." + ap.ConvertExt
        If ap.File = outPath Then outPath += "." + ap.ConvertExt
        Dim args = "-i " + ap.File.Escape
        If Not ap.Stream Is Nothing Then args += " -map 0:" & ap.Stream.StreamOrder
        If ap.Gain <> 0 Then args += " -af volume=" + ap.Gain.ToInvariantString + "dB"
        args += " -y -hide_banner -ac " & ap.Channels
        If ap.ConvertExt.EqualsAny("wav", "w64") Then args += " -c:a pcm_s24le"
        args += " " + outPath.Escape

        Using proc As New Proc
            proc.Header = "Convert from " + ap.File.Ext.ToUpper + " to " + outPath.Ext.ToUpper
            proc.SkipStrings = {"frame=", "size="}
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using

        If g.FileExists(outPath) Then
            ap.Gain = 0
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub ConvertDirectShowSource(ap As AudioProfile, Optional useFlac As Boolean = False)
        If ap.File.Ext = ap.ConvertExt Then Exit Sub
        If Not Package.AviSynth.VerifyOK(True) Then Throw New AbortException
        ap.Delay = 0
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        d.RemoveFilter("Cutting")
        Dim outPath = p.TempDir + ap.File.Base + "_convDSS." + ap.ConvertExt
        d.Path = p.TempDir + ap.File.Base + "_DecDSS.avs"
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,DirectShowSource(""" + ap.File + """, video=false))"))
        If ap.Channels = 2 Then d.Filters.Add(New VideoFilter(GetDown2Code))
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
        If ap.File.Ext = ap.ConvertExt Then Exit Sub
        If Not Package.AviSynth.VerifyOK(True) Then Throw New AbortException
        ap.Delay = 0
        Dim cachefile = p.TempDir + ap.File.Base + ".ffindex"
        g.ffmsindex(ap.File, cachefile)
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        d.RemoveFilter("Cutting")
        Dim outPath = p.TempDir + ap.File.Base + "_convFFAudioSource." + ap.ConvertExt
        d.Path = p.TempDir + ap.File.Base + "_DecodeFFAudioSource.avs"
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,FFAudioSource(""" + ap.File + """, cachefile = """ + cachefile + """))"))
        If ap.Channels = 2 Then d.Filters.Add(New VideoFilter(GetDown2Code))
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
        If ap.File.Contains("_cut_") Then Exit Sub
        If Not Package.AviSynth.VerifyOK(True) Then Throw New AbortException
        ap.Delay = 0
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        Dim wavPath = p.TempDir + ap.File.Base + "_cut_ds.wav"
        d.Path = p.TempDir + ap.File.Base + "_cut_ds.avs"
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,DirectShowSource(""" + ap.File + """, video=false))"))
        If ap.Channels = 2 Then d.Filters.Add(New VideoFilter(GetDown2Code))
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
        If ap.File.Contains("_cut_") Then Exit Sub
        If Not Package.AviSynth.VerifyOK(True) Then Throw New AbortException
        ap.Delay = 0
        Dim cachefile = p.TempDir + ap.File.Base + ".ffindex"
        g.ffmsindex(ap.File, cachefile)
        Dim d As New VideoScript
        d.Filters.AddRange(p.Script.Filters)
        Dim wavPath = p.TempDir + ap.File.Base + "_cut_ff.wav"
        d.Path = p.TempDir + ap.File.Base + "_cut_ff.avs"
        d.Filters.Insert(1, New VideoFilter("AudioDub(last,FFAudioSource(""" + ap.File + """, cachefile = """ + cachefile + """))"))
        If ap.Channels = 2 Then d.Filters.Add(New VideoFilter(GetDown2Code))
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
        If ap.File.Contains("_cut_") Then Exit Sub
        If Not Package.AviSynth.VerifyOK(True) Then Throw New AbortException

        Dim aviPath = p.TempDir + ap.File.Base + "_cut_mm.avi"
        Dim args = String.Format("-f lavfi -i color=c=black:s=16x16:d={0}:r={1} -y -hide_banner -c:v copy " + aviPath.Escape, (p.CutFrameCount / p.CutFrameRate).ToString("f9", CultureInfo.InvariantCulture), p.CutFrameRate.ToString("f9", CultureInfo.InvariantCulture))

        Using proc As New Proc
            proc.Header = "Create avi file for audio cutting"
            proc.SkipStrings = {"frame=", "size="}
            proc.WriteLog("mkvmerge cannot cut audio without video so a avi file has to be created" + BR2)
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

        Dim mkvPath = p.TempDir + ap.File.Base + "_cut_.mkv"

        Dim args2 = "-o " + mkvPath.Escape + " " + aviPath.Escape + " " + ap.File.Escape
        args2 += " --split parts-frames:" + p.Ranges.Select(Function(v) v.Start & "-" & v.End).Join(",+")
        args2 += " --ui-language en"

        Using proc As New Proc
            proc.Header = "Cut audio"
            proc.SkipString = "Progress: "
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.mkvmerge
            proc.Arguments = args2
            proc.AllowedExitCodes = {0, 1, 2}
            proc.Start()
        End Using

        Dim fail As Boolean

        If File.Exists(mkvPath) Then
            Log.WriteLine(MediaInfo.GetSummary(mkvPath))
            Dim streams = MediaInfo.GetAudioStreams(mkvPath)

            If streams.Count > 0 Then
                mkvDemuxer.Demux(mkvPath, {streams(0)}, Nothing, ap, p, False, False)
            Else
                fail = True
            End If
        Else
            fail = True
        End If

        If fail AndAlso TypeOf ap Is GUIAudioProfile AndAlso Not ap.File.Ext = "wav" Then
            Log.Write("Error", "no output found")
            Convert(ap)

            If Filepath.GetExtFull(ap.File) = ".wav" Then Cut(ap)
        End If
    End Sub

    Shared Function GetDown2Code() As String
        Dim a =
      <a>
Audiochannels() >= 6 ? Down2(last) : last

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
}
</a>
        Return a.Value.Trim
    End Function

    Shared Function FileExist(fp As String) As Boolean
        Return File.Exists(fp) AndAlso New FileInfo(fp).Length > 500
    End Function

    Shared Sub SetGain(ap As AudioProfile)
        Dim args = "-i " + ap.File.Escape
        If Not ap.Stream Is Nothing AndAlso ap.Streams.Count > 1 Then args += " -map 0:a:" & ap.Stream.Index
        args += " -sn -vn -hide_banner -af volumedetect -f null NUL"

        Using proc As New Proc
            proc.Header = "Find Gain"
            proc.SkipStrings = {"frame=", "size="}
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()

            Dim match = Regex.Match(proc.Log.ToString, "max_volume: -(\d+\.\d+) dB")
            If match.Success Then ap.Gain = match.Groups(1).Value.ToSingle()
        End Using
    End Sub
End Class

Public Enum AudioConvertMode
    ffmpeg
    eac3to
    NicAudio
    DirectShow
    FFAudioSource
End Enum

Public Enum AudioConvertType
    FLAC
    W64
    WAVE
End Enum

Public Enum CuttingMode
    mkvmerge
    DirectShow
    NicAudio
End Enum