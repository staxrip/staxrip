Imports System.Text

Imports VB6 = Microsoft.VisualBasic
Imports System.Text.RegularExpressions
Imports System.Globalization

Public Class Audio
    Shared Sub Process(ap As AudioProfile)
        If Not File.Exists(ap.File) OrElse TypeOf ap Is NullAudioProfile Then
            Exit Sub
        End If

        If ap.File <> p.SourceFile Then
            Log.WriteHeader("Audio Source File MediaInfo")
            Log.WriteLine(MediaInfo.GetSummary(ap.File))
        End If

        If p.DecodingMode <> DecodingMode.Disabled Then
            Decode(ap)
        End If

        If ap.HasStream Then
            Dim cutting = p.Ranges.Count > 0

            Dim directMux = TypeOf ap Is MuxAudioProfile AndAlso
                p.VideoEncoder.Muxer.IsSupported(ap.Stream.Extension.TrimStart("."c)) AndAlso
                p.VideoEncoder.Muxer.IsSupported(Filepath.GetExtNoDot(ap.File)) AndAlso
                Not cutting

            If (cutting OrElse Not ap.IsInputSupported) AndAlso Not directMux Then
                Select Case Filepath.GetExt(ap.File)
                    Case ".mkv", ".webm"
                        DemuxMKV(ap.File, ap.Stream, ap)
                    Case ".mp4"
                        DemuxMP4(ap.File, ap.Stream, ap)
                    Case Else
                        If p.AvsDoc.GetFilter("Source").Script.ToLower.Contains("directshowsource") AndAlso
                            Not TypeOf ap Is MuxAudioProfile Then

                            DecodeDirectShowSource(ap)
                        ElseIf Not Filepath.GetExt(ap.File) = ".m2ts" Then
                            Demuxffmpeg(ap.File, ap.Stream, ap)
                        End If
                End Select
            End If
        End If

        Cut(ap)

        If Not TypeOf ap Is MuxAudioProfile AndAlso
            Not ap.SupportedInput.Contains(Filepath.GetExtNoDot(ap.File)) Then

            Decode(ap, ap.SupportedInput.Contains("flac"))
        End If

        If TypeOf ap Is GUIAudioProfile Then
            Dim gap = DirectCast(ap, GUIAudioProfile)

            If gap.Params.Normalize Then
                Dim cmdl = ap.CommandLines

                If cmdl <> "" AndAlso cmdl.Contains("ffmpeg.exe") Then
                    SetGain(ap)
                End If
            End If
        End If
    End Sub

    Shared Function GetBaseNameForStream(path As String, stream As AudioStream, Optional shorten As Boolean = False) As String
        Dim ret = If(shorten, Filepath.GetBase(path).Shorten(10), Filepath.GetBase(path)) + " - ID" & (stream.StreamOrder + 1)

        If stream.Delay <> 0 Then ret += " - " & stream.Delay & "ms"
        If stream.Language.TwoLetterCode <> "iv" Then ret += " - " + stream.Language.ToString
        If Not shorten AndAlso path.Length < 130 Then If stream.Title <> "" Then ret += " - " + stream.Title.Shorten(30)

        Return ret
    End Function

    Shared Sub Demuxffmpeg(sourcefile As String, stream As AudioStream, ap As AudioProfile)
        Dim outPath = p.TempDir + GetBaseNameForStream(sourcefile, stream) + stream.Extension
        If outPath.Length > 259 Then outPath = p.TempDir + GetBaseNameForStream(sourcefile, stream, True) + stream.Extension

        Dim streamIndex = stream.StreamOrder
        Dim args = "-i """ + sourcefile + """"

        If MediaInfo.GetAudioCount(sourcefile) > 1 Then args += " -map 0:" & stream.StreamOrder

        args += " -c:a copy -vn -sn -y """ + outPath + """"

        Using proc As New Proc
            proc.Init("Demux audio using ffmpeg", {"Media Export: |", "File Export: |", "ISO File Writing: |"})
            proc.Encoding = Encoding.UTF8
            proc.File = Packs.ffmpeg.GetPath
            proc.Arguments = args
            proc.Start()
        End Using

        If File.Exists(outPath) Then
            If Not ap Is Nothing Then ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub DemuxMP4(sourcefile As String, stream As AudioStream, ap As AudioProfile)
        Dim outPath = p.TempDir + GetBaseNameForStream(sourcefile, stream) + stream.Extension
        If outPath.Length > 259 Then outPath = p.TempDir + GetBaseNameForStream(sourcefile, stream, True) + stream.Extension
        FileHelp.Delete(outPath)
        Dim args As String
        If stream.Format = "AAC" Then args += " -single" Else args += " -raw"
        args += " " & stream.ID & " -out """ + outPath + """ """ + sourcefile + """"

        Using proc As New Proc
            proc.Init("Demux audio using MP4Box", {"Media Export: |", "File Export: |", "ISO File Writing: |"})
            proc.File = Packs.MP4Box.GetPath
            proc.Arguments = args
            proc.Process.StartInfo.EnvironmentVariables("TEMP") = p.TempDir
            proc.Start()
        End Using

        If File.Exists(outPath) Then
            If Not ap Is Nothing Then ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
            Demuxffmpeg(sourcefile, stream, ap)
        End If
    End Sub

    Shared Sub DemuxMKV(sourcefile As String, stream As AudioStream, ap As AudioProfile)
        Dim ext = stream.Extension
        If ext = ".m4a" Then ext = ".aac"
        Dim outPath = p.TempDir + If(sourcefile = p.SourceFile, GetBaseNameForStream(sourcefile, stream), Filepath.GetBase(sourcefile)) + ext
        If outPath.Length > 259 Then outPath = p.TempDir + If(sourcefile = p.SourceFile, GetBaseNameForStream(sourcefile, stream, True), Filepath.GetBase(sourcefile).Shorten(10)) + ext

        Using proc As New Proc
            proc.Init("Demux audio using mkvextract", "Progress: ")
            proc.Encoding = Encoding.UTF8
            proc.File = Packs.Mkvmerge.GetDir + "mkvextract.exe"
            proc.Arguments = "tracks """ + sourcefile + """ " & stream.StreamOrder &
                ":""" + outPath + """ --ui-language en"
            proc.AllowedExitCodes = {0, 1, 2}
            proc.Start()
        End Using

        If File.Exists(outPath) Then
            If Not ap Is Nothing Then ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))

            If Filepath.GetExt(outPath) = ".aac" Then
                Using proc As New Proc
                    proc.Init("Mux AAC to M4A", "|")
                    proc.File = Packs.MP4Box.GetPath

                    Dim sbr = If(outPath.Contains("SBR"), ":sbr", "")

                    Dim m4aPath = Filepath.GetChangeExt(outPath, "m4a")
                    proc.Arguments = "-add """ + outPath + sbr + ":name= "" -new """ + m4aPath + """"
                    proc.Process.StartInfo.EnvironmentVariables("TEMP") = p.TempDir
                    proc.Start()

                    If File.Exists(m4aPath) Then
                        If Not ap Is Nothing Then ap.File = m4aPath
                        FileHelp.Delete(outPath)
                        Log.WriteLine(CrLf + MediaInfo.GetSummary(m4aPath))
                    Else
                        Throw New ErrorAbortException("Error mux AAC to M4A", outPath)
                    End If
                End Using
            End If
        Else
            Log.Write("Error", "no output found")
            Demuxffmpeg(sourcefile, stream, ap)
        End If
    End Sub

    Shared Sub Decode(ap As AudioProfile, Optional useFlac As Boolean = False)
        Dim ext = If(useFlac, ".flac", ".wav")
        If Filepath.GetExt(ap.File) = If(useFlac, ".flac", ".wav") Then Exit Sub

        If Filepath.GetExt(ap.File) = ".avs" Then
            Dim outPath = Filepath.GetDirAndBase(ap.File) + ext
            Dim args = "-i """ + ap.File + """ -y """ + outPath + """"

            Using proc As New Proc
                proc.Init("AVS to FLAC/WAV using ffmpeg", "frame=", "size=", "Multiple")
                proc.Encoding = Encoding.UTF8
                proc.File = Packs.ffmpeg.GetPath
                proc.Arguments = args
                proc.Start()
            End Using

            If g.WasFileJustWritten(outPath) Then
                ap.File = outPath
                Exit Sub
            End If
        End If

        If p.DecodingMode = DecodingMode.ffmpeg Then
            DecodeFfmpeg(ap)
        ElseIf p.DecodingMode = DecodingMode.FFAudioSource Then
            DecodeFFAudioSource(ap)
        ElseIf p.DecodingMode = DecodingMode.eac3to Then
            DecodeEac3to(ap)
        ElseIf p.DecodingMode = DecodingMode.NicAudio Then
            DecodeNicAudio(ap)
        ElseIf p.DecodingMode = DecodingMode.DirectShow Then
            DecodeDirectShowSource(ap)
        End If

        If p.AvsDoc.GetFilter("Source").Script.ToLower.Contains("directshowsource") Then
            DecodeDirectShowSource(ap)
        End If

        DecodeFfmpeg(ap, useFlac)
        DecodeEac3to(ap, useFlac)
        DecodeDirectShowSource(ap, useFlac)
    End Sub

    Shared Sub Cut(ap As AudioProfile)
        If p.Ranges.Count = 0 OrElse Not File.Exists(ap.File) OrElse
            ap.File.Contains("_cut_.") Then

            Exit Sub
        End If

        Select Case p.CuttingMode
            Case CuttingMode.mkvmerge
                CutMkvmerge(ap)
            Case CuttingMode.VirtualDubMod
                If IsOneOf(Filepath.GetExtNoDot(ap.File), FileTypes.VirtualDubModInput) Then
                    CutVirtualDubMod(ap)
                Else
                    CutMkvmerge(ap)
                End If
            Case CuttingMode.NicAudio
                If IsOneOf(Filepath.GetExtNoDot(ap.File), FileTypes.NicAudioInput) AndAlso
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

    Shared Sub DecodeEac3to(ap As AudioProfile, Optional useFlac As Boolean = False)
        If {"wav", "flac"}.Contains(Filepath.GetExtNoDot(ap.File)) Then Exit Sub

        If Not IsOneOf(Filepath.GetExtNoDot(ap.File), FileTypes.eac3toInput) Then
            Exit Sub
        End If

        Dim outPath = p.TempDir + Filepath.GetBase(ap.File) + If(useFlac, ".flac", ".wav")
        Dim args = """" + ap.File + """ """ + outPath + """"

        If ap.Channels = 6 Then
            args += " -down6"
        ElseIf ap.Channels = 2 Then
            args += " -down2"
        End If

        args += " -simple -progressnumbers"

        Using proc As New Proc
            proc.Init("Convert to WAV/FLAC using eac3to")
            proc.File = Packs.eac3to.GetPath
            proc.Arguments = args
            proc.TrimChars = {"-"c, " "c}
            proc.RemoveChars = {VB6.ChrW(8)} 'backspace
            proc.SkipStrings = {"process:", "analyze:"}
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using

        If g.WasFileJustWritten(outPath) Then
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub DecodeFfmpeg(ap As AudioProfile, Optional useFlac As Boolean = False)
        If {"wav", "flac"}.Contains(Filepath.GetExtNoDot(ap.File)) Then Exit Sub
        Dim outPath = p.TempDir + Filepath.GetBase(ap.File) + If(useFlac, ".flac", ".wav")
        Dim args = "-i """ + ap.File + """"

        If Not ap.Stream Is Nothing Then args += " -map 0:" & ap.Stream.StreamOrder

        args += " -y -ac " & ap.Channels
        args += " """ + outPath + """"

        Using proc As New Proc
            proc.Init("Convert from " + Filepath.GetExtNoDot(ap.File).ToUpper + " to " + Filepath.GetExtNoDot(outPath).ToUpper + " using ffmpeg",
                                "frame=", "size=", "Multiple", "decoding is not implemented", "unsupported frame type", "upload a sample")
            proc.Encoding = Encoding.UTF8
            proc.File = Packs.ffmpeg.GetPath
            proc.Arguments = args
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using

        If g.WasFileJustWritten(outPath) Then
            ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub DecodeDirectShowSource(ap As AudioProfile, Optional useFlac As Boolean = False)
        If {"wav", "flac"}.Contains(Filepath.GetExtNoDot(ap.File)) Then Exit Sub
        ap.Delay = 0
        Dim d As New AviSynthDocument
        d.Filters.AddRange(p.AvsDoc.Filters)
        d.Remove("Cutting")
        Dim wavPath = p.TempDir + Filepath.GetBase(ap.File) + "_DecDSS.wav"
        d.Path = p.TempDir + Filepath.GetBase(ap.File) + "_DecDSS.avs"
        d.Filters.Insert(1, New AviSynthFilter("AudioDub(last,DirectShowSource(""" + ap.File + """, video=false))"))
        If ap.Channels = 2 Then d.Filters.Add(New AviSynthFilter(GetDown2Code))
        d.Synchronize()

        Dim args = "-i """ + d.Path + """ -y """ + wavPath + """"

        Using proc As New Proc
            proc.Init("AVS to WAV using ffmpeg", "frame=", "size=", "Multiple")
            proc.WriteLine(Macro.Solve(d.GetScript) + CrLf)
            proc.Encoding = Encoding.UTF8
            proc.File = Packs.ffmpeg.GetPath
            proc.Arguments = args
            proc.Start()
        End Using

        If g.WasFileJustWritten(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub DecodeNicAudio(ap As AudioProfile)
        If Filepath.GetExt(ap.File) = ".wav" Then Exit Sub
        If Not FileTypes.NicAudioInput.Contains(Filepath.GetExtNoDot(ap.File)) Then Exit Sub
        ap.Delay = 0
        Dim d As New AviSynthDocument
        d.Filters.AddRange(p.AvsDoc.Filters)
        d.Remove("Cutting")
        Dim wavPath = p.TempDir + Filepath.GetBase(ap.File) + "_DecodeNicAudio.wav"
        d.Path = p.TempDir + Filepath.GetBase(ap.File) + "_DecodeNicAudio.avs"
        d.Filters.Insert(1, New AviSynthFilter(GetNicAudioCode(ap)))
        If ap.Channels = 2 Then d.Filters.Add(New AviSynthFilter(GetDown2Code))
        d.Synchronize()

        Dim args = "-i """ + d.Path + """ -y """ + wavPath + """"

        Using proc As New Proc
            proc.Init("AVS to WAV using ffmpeg", "frame=", "size=", "Multiple")
            proc.WriteLine(Macro.Solve(d.GetScript) + CrLf)
            proc.Encoding = Encoding.UTF8
            proc.File = Packs.ffmpeg.GetPath
            proc.Arguments = args
            proc.Start()
        End Using

        If g.WasFileJustWritten(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub DecodeFFAudioSource(ap As AudioProfile)
        If Filepath.GetExt(ap.File) = ".wav" Then Exit Sub
        ap.Delay = 0
        Dim cachefile = p.TempDir + Filepath.GetBase(ap.File) + ".ffindex"
        g.MainForm.ffmsindex(ap.File, cachefile)
        Dim d As New AviSynthDocument
        d.Filters.AddRange(p.AvsDoc.Filters)
        d.Remove("Cutting")
        Dim wavPath = p.TempDir + Filepath.GetBase(ap.File) + "_DecodeFFAudioSource.wav"
        d.Path = p.TempDir + Filepath.GetBase(ap.File) + "_DecodeFFAudioSource.avs"
        d.Filters.Insert(1, New AviSynthFilter("AudioDub(last,FFAudioSource(""" + ap.File + """, cachefile = """ + cachefile + """))"))
        If ap.Channels = 2 Then d.Filters.Add(New AviSynthFilter(GetDown2Code))
        d.Synchronize()

        Dim args = "-i """ + d.Path + """ -y """ + wavPath + """"

        Using proc As New Proc
            proc.Init("AVS to WAV using ffmpeg", "frame=", "size=", "Multiple")
            proc.WriteLine(Macro.Solve(d.GetScript) + CrLf)
            proc.Encoding = Encoding.UTF8
            proc.File = Packs.ffmpeg.GetPath
            proc.Arguments = args
            proc.Start()
        End Using

        If g.WasFileJustWritten(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub CutDirectShowSource(ap As AudioProfile)
        If ap.File.Contains("_cut_") Then Exit Sub
        ap.Delay = 0
        Dim d As New AviSynthDocument
        d.Filters.AddRange(p.AvsDoc.Filters)
        Dim wavPath = p.TempDir + Filepath.GetBase(ap.File) + "_cut_ds.wav"
        d.Path = p.TempDir + Filepath.GetBase(ap.File) + "_cut_ds.avs"
        d.Filters.Insert(1, New AviSynthFilter("AudioDub(last,DirectShowSource(""" + ap.File + """, video=false))"))
        If ap.Channels = 2 Then d.Filters.Add(New AviSynthFilter(GetDown2Code))
        d.Synchronize()

        Dim args = "-i """ + d.Path + """ -y """ + wavPath + """"

        Using proc As New Proc
            proc.Init("AVS to WAV using ffmpeg", "frame=", "size=", "Multiple")
            proc.WriteLine(Macro.Solve(d.GetScript) + CrLf)
            proc.Encoding = Encoding.UTF8
            proc.File = Packs.ffmpeg.GetPath
            proc.Arguments = args
            proc.Start()
        End Using

        If g.WasFileJustWritten(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub CutNicAudio(ap As AudioProfile)
        If ap.File.Contains("_cut_") Then Exit Sub
        If Not IsOneOf(Filepath.GetExtNoDot(ap.File), FileTypes.NicAudioInput) Then Exit Sub
        ap.Delay = 0
        Dim d As New AviSynthDocument
        d.Filters.AddRange(p.AvsDoc.Filters)
        Dim wavPath = p.TempDir + Filepath.GetBase(ap.File) + "_cut_na.wav"
        d.Path = p.TempDir + Filepath.GetBase(ap.File) + "_cut_na.avs"
        d.Filters.Insert(1, New AviSynthFilter(GetNicAudioCode(ap)))
        If ap.Channels = 2 Then d.Filters.Add(New AviSynthFilter(GetDown2Code))
        d.Synchronize()

        Dim args = "-i """ + d.Path + """ -y """ + wavPath + """"

        Using proc As New Proc
            proc.Init("AVS to WAV using ffmpeg", "frame=", "size=", "Multiple")
            proc.WriteLine(Macro.Solve(d.GetScript) + CrLf)
            proc.Encoding = Encoding.UTF8
            proc.File = Packs.ffmpeg.GetPath
            proc.Arguments = args
            proc.Start()
        End Using

        If g.WasFileJustWritten(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub CutFFAudioSource(ap As AudioProfile)
        If ap.File.Contains("_cut_") Then Exit Sub
        ap.Delay = 0
        Dim cachefile = p.TempDir + Filepath.GetBase(ap.File) + ".ffindex"
        g.MainForm.ffmsindex(ap.File, cachefile)
        Dim d As New AviSynthDocument
        d.Filters.AddRange(p.AvsDoc.Filters)
        Dim wavPath = p.TempDir + Filepath.GetBase(ap.File) + "_cut_ff.wav"
        d.Path = p.TempDir + Filepath.GetBase(ap.File) + "_cut_ff.avs"
        d.Filters.Insert(1, New AviSynthFilter("AudioDub(last,FFAudioSource(""" + ap.File + """, cachefile = """ + cachefile + """))"))
        If ap.Channels = 2 Then d.Filters.Add(New AviSynthFilter(GetDown2Code))
        d.Synchronize()

        Dim args = "-i """ + d.Path + """ -y """ + wavPath + """"

        Using proc As New Proc
            proc.Init("AVS to WAV using ffmpeg", "frame=", "size=", "Multiple")
            proc.WriteLine(Macro.Solve(d.GetScript) + CrLf)
            proc.Encoding = Encoding.UTF8
            proc.File = Packs.ffmpeg.GetPath
            proc.Arguments = args
            proc.Start()
        End Using

        If g.WasFileJustWritten(wavPath) Then
            ap.File = wavPath
            Log.WriteLine(MediaInfo.GetSummary(wavPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Sub CutMkvmerge(ap As AudioProfile)
        If ap.File.Contains("_cut_") Then Exit Sub

        Dim code = String.Format("BlankClip(length = {0}, fps = {1}, width = 16, height = 16, pixel_type = ""YV12"")" +
                                 CrLf + "KillAudio()",
                                 p.SourceAviSynthDocument.GetFrames,
                                 p.SourceAviSynthDocument.GetFramerate.ToString("f6", CultureInfo.InvariantCulture))

        Dim scriptPath = p.TempDir + Filepath.GetBase(ap.File) + "_cut_mm.avs"
        Dim aviPath = p.TempDir + Filepath.GetBase(ap.File) + "_cut_mm.avi"

        File.WriteAllText(scriptPath, code)

        Dim args = "-i """ + scriptPath + """ -c:v copy -y """ + aviPath + """"

        Using proc As New Proc
            proc.Init("Create avi file with ffmpeg", "frame=", "size=", "Multiple")
            proc.WriteLine("mkvmerge cannot cut audio without video so we create" + CrLf +
                           "a 16x16 pixel avi file using the following script:" + CrLf2 + code + CrLf2)
            proc.Encoding = Encoding.UTF8
            proc.File = Packs.ffmpeg.GetPath
            proc.Arguments = args
            proc.Start()
        End Using

        If Not File.Exists(aviPath) Then
            Throw New ErrorAbortException("Error", "Output file missing")
        Else
            Log.WriteLine(MediaInfo.GetSummary(aviPath))
        End If

        Dim mkvPath = p.TempDir + Filepath.GetBase(ap.File) + "_cut_.mkv"

        args = "-o """ + mkvPath + """ """ + aviPath + """ """ + ap.File + """"
        args += " --split parts-frames:" + p.Ranges.Select(Function(v) v.Start & "-" & v.End).Join(",+")
        args += " --ui-language en"

        Using proc As New Proc
            proc.Init("Cut using mkvmerge", "Progress: ")
            proc.Encoding = Encoding.UTF8
            proc.File = Packs.Mkvmerge.GetPath
            proc.Arguments = args
            proc.AllowedExitCodes = {0, 1, 2}
            proc.Start()
        End Using

        Dim fail As Boolean

        If File.Exists(mkvPath) Then
            Log.WriteLine(MediaInfo.GetSummary(mkvPath))

            Dim streams = MediaInfo.GetAudioStreams(mkvPath)

            If streams.Count > 0 Then
                DemuxMKV(mkvPath, streams(0), ap)
            Else
                fail = True
            End If
        Else
            fail = True
        End If

        If fail AndAlso TypeOf ap Is GUIAudioProfile AndAlso
            Not Filepath.GetExt(ap.File) = ".wav" Then

            Log.Write("Error", "no output found")
            Decode(ap)

            If Filepath.GetExt(ap.File) = ".wav" Then
                Cut(ap)
            End If
        End If
    End Sub

    Shared Sub CutVirtualDubMod(ap As AudioProfile)
        If ap.File.Contains("_cut_") Then Exit Sub

        Dim ext = Filepath.GetExt(ap.File)

        Dim avs As New AviSynthDocument
        avs.Path = p.TempDir + Filepath.GetBase(ap.File) + "_cut_.avs"
        avs.Filters.AddRange(p.AvsDoc.Filters)
        avs.Remove("Cutting")
        avs.Filters.Add(New AviSynthFilter("KillAudio()"))
        avs.Synchronize()

        Dim script As New VirtualDubScript
        script.Open(avs.Path)
        script.StreamSetSource(0, ap.File)
        script.StreamSetMode(0, AudioMode.DirectStreamCopy)
        script.SubsetClear()

        For Each i In p.Ranges
            script.SubsetAddRange(i.Start, i.End - i.Start)
        Next

        Dim outPath = p.TempDir + Filepath.GetBase(ap.File) + "_cut_" + ext

        If ext = ".wav" Then
            script.SaveWAV(outPath)
        Else
            script.StreamDemux(0, outPath)
        End If

        Log.WriteHeader("Audio cutting using VirtualDubMod")
        Log.WriteLine(Macro.Solve(avs.GetScript) + CrLf)
        Log.WriteLine(script.Text)

        script.SaveScript(p.TempDir + Filepath.GetBase(outPath) + "_cut_.vcf")

        Dim pwCut As New Proc
        ProcessForm.ProcInstance = pwCut

        pwCut.File = Packs.VirtualDubMod.GetPath
        pwCut.Arguments = "/s""" + p.TempDir + Filepath.GetBase(outPath) + "_cut_.vcf"" /x"
        pwCut.Wait = True
        pwCut.Priority = s.ProcessPriority

        If Not ProcessForm.IsVisible Then
            pwCut.HideAfterStart = True
        End If

        Dim start = DateTime.Now

        pwCut.Start()

        If g.WasFileJustWritten(outPath) Then
            ap.File = outPath
            Log.WriteStats(start)
            Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            Log.Write("Error", "no output found")
        End If
    End Sub

    Shared Function GetNicAudioCode(ap As AudioProfile) As String
        Select Case Filepath.GetExt(ap.File)
            Case ".ac3"
                Return "AudioDub(last, NicAC3Source(""" + ap.File + """, Channels=" & ap.Channels & ", DRC=1))"
            Case ".dts"
                Return "AudioDub(last, NicDTSSource(""" + ap.File + """, Channels=" & ap.Channels & ", DRC=1))"
            Case ".mpa", ".mp2", ".mp3"
                Return "AudioDub(last, NicMPG123Source(""" + ap.File + """))"
            Case ".wav"
                Return "AudioDub(last, RaWavSource(""" + ap.File + """, Channels=" & ap.Channels & "))"
        End Select
    End Function

    Shared Function GetDown2Code() As String
        Dim a = _
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
        Dim args = "-i """ + ap.File + """"
        If Not ap.Stream Is Nothing Then args += " -map 0:" & ap.Stream.StreamOrder
        args += " -af volumedetect -f null NUL"

        Using proc As New Proc
            proc.Init("Find Gain using ffmpeg", "frame=", "size=", "Multiple", "decoding is not implemented", "unsupported frame type", "upload a sample")
            proc.Encoding = Encoding.UTF8
            proc.File = Packs.ffmpeg.GetPath
            proc.Arguments = args
            proc.Start()

            Dim match = Regex.Match(ProcessForm.CommandLineLog.ToString, "max_volume: -(\d+\.\d+) dB")
            If match.Success Then ap.Gain = match.Groups(1).Value.ToSingle()
        End Using
    End Sub
End Class

Public Enum DecodingMode
    Disabled
    ffmpeg
    NicAudio
    DirectShow
    eac3to
    FFAudioSource
End Enum

Public Enum CuttingMode
    mkvmerge
    VirtualDubMod
    NicAudio
    DirectShow
End Enum