Imports System.Text.RegularExpressions
Imports VB6 = Microsoft.VisualBasic
Imports System.Text

<Serializable()>
Public MustInherit Class Demuxer
    MustOverride Sub Run(proj As Project)

    Overridable Property Active As Boolean = True
    Overridable Property InputExtensions As String() = {}
    Overridable Property InputFormats As String() = {}
    Overridable Property Name As String = ""
    Overridable Property OutputExtensions As String() = {}
    Overridable Property SourceFilters As String() = {}

    Property VideoDemuxing As Boolean
    Property VideoDemuxed As Boolean
    Property ChaptersDemuxing As Boolean = True

    Overridable ReadOnly Property HasConfigDialog() As Boolean
        Get
            Return False
        End Get
    End Property

    Overridable Function ShowConfigDialog() As DialogResult
        Using form As New SimpleSettingsForm(Name)
            form.ScaleClientSize(23, 10)
            Dim ui = form.SimpleUI
            Dim page = ui.CreateFlowPage("main page")

            Dim tb = ui.AddText(page)
            tb.Label.Text = "Supported Input File Types:"
            tb.Edit.Text = InputExtensions.Join(" ")
            tb.Edit.SaveAction = Sub(value) InputExtensions = value.ToLower.SplitNoEmptyAndWhiteSpace(",", ";", " ")

            Dim cb = ui.AddBool(page)
            cb.Text = "Video Demuxing"
            cb.Checked = VideoDemuxing
            cb.SaveAction = Sub(val) VideoDemuxing = val

            cb = ui.AddBool(page)
            cb.Text = "Chapters Demuxing"
            cb.Checked = ChaptersDemuxing
            cb.SaveAction = Sub(val) ChaptersDemuxing = val

            Dim ret = form.ShowDialog()
            If ret = DialogResult.OK Then ui.Save()

            Return ret
        End Using
    End Function

    Overrides Function ToString() As String
        Dim input = InputExtensions.Join(", ")
        If input.Length > 25 Then input = input.Shorten(25) + "..."
        Return Name + " (" + input + " -> " + OutputExtensions.Join(", ") + ")"
    End Function

    Overridable Function GetHelp() As String
        For Each i In Package.Items.Values
            If Name = i.Name Then Return i.Description
        Next
    End Function

    Shared Function GetDefaults() As List(Of Demuxer)
        Dim ret As New List(Of Demuxer)

        Dim dsmux As New CommandLineDemuxer
        dsmux.Name = "dsmux: Re-mux TS to MKV"
        dsmux.InputExtensions = {"ts"}
        dsmux.OutputExtensions = {"mkv"}
        dsmux.Command = "%app:dsmux%"
        dsmux.Arguments = """%temp_file%.mkv"" ""%source_file%"""
        dsmux.Active = False
        ret.Add(dsmux)

        ret.Add(New ffmpegDemuxer)

        Dim tsToMkv As New CommandLineDemuxer
        tsToMkv.Name = "ffmpeg: Re-mux TS to MKV"
        tsToMkv.InputExtensions = {"ts"}
        tsToMkv.OutputExtensions = {"mkv"}
        tsToMkv.InputFormats = {"hevc", "avc"}
        tsToMkv.Command = "%app:ffmpeg%"
        tsToMkv.Arguments = "-i ""%source_file%"" -c copy -map 0 -ignore_unknown -sn -y -hide_banner ""%temp_file%.mkv"""
        ret.Add(tsToMkv)

        ret.Add(New mkvDemuxer)
        ret.Add(New MP4BoxDemuxer)
        ret.Add(New eac3toDemuxer)

        Dim dgIndex As New CommandLineDemuxer
        dgIndex.Name = "DGIndex: Demux & Index MPEG-2"
        dgIndex.InputExtensions = {"mpg", "vob", "m2ts", "mts", "m2t"}
        dgIndex.OutputExtensions = {"d2v"}
        dgIndex.InputFormats = {"mpeg2"}
        dgIndex.Command = "%app:DGIndex%"
        dgIndex.Arguments = "-i %source_files% -ia 2 -fo 0 -yr 1 -tn 1 -om 2 -drc 2 -dsd 0 -dsa 0 -o ""%temp_file%"" -hide -exit"
        dgIndex.SourceFilters = {"MPEG2Source", "d2v.Source"}
        ret.Add(dgIndex)

        Dim dgnvNoDemux As New CommandLineDemuxer
        dgnvNoDemux.Name = "DGIndexNV: Index, No Demux"
        dgnvNoDemux.InputExtensions = {"mkv", "mp4", "h264", "h265", "avc", "hevc", "hvc", "264", "265"}
        dgnvNoDemux.OutputExtensions = {"dgi"}
        dgnvNoDemux.InputFormats = {"hevc", "avc", "vc1", "mpeg2"}
        dgnvNoDemux.Command = "%app:DGIndexNV%"
        dgnvNoDemux.Arguments = "-i %source_files_comma% -o ""%source_temp_file%.dgi"" -h"
        dgnvNoDemux.SourceFilters = {"DGSource"}
        dgnvNoDemux.Active = False
        ret.Add(dgnvNoDemux)

        Dim dgnvDemux As New CommandLineDemuxer
        dgnvDemux.Name = "DGIndexNV: Demux & Index"
        dgnvDemux.InputExtensions = {"mpg", "vob", "ts", "m2ts", "mts", "m2t"}
        dgnvDemux.OutputExtensions = {"dgi"}
        dgnvDemux.InputFormats = {"hevc", "avc", "vc1", "mpeg2"}
        dgnvDemux.Command = "%app:DGIndexNV%"
        dgnvDemux.Arguments = "-i %source_files_comma% -o ""%source_temp_file%.dgi"" -a -h"
        dgnvDemux.SourceFilters = {"DGSource"}
        dgnvDemux.Active = False
        ret.Add(dgnvDemux)

        Dim dgimNoDemux As New CommandLineDemuxer
        dgimNoDemux.Name = "DGIndexIM: Index, No Demux"
        dgimNoDemux.InputExtensions = {"264", "h264", "avc", "mkv", "mp4"}
        dgimNoDemux.OutputExtensions = {"dgim"}
        dgimNoDemux.InputFormats = {"hevc", "avc", "vc1", "mpeg2"}
        dgimNoDemux.Command = "%app:DGIndexIM%"
        dgimNoDemux.Arguments = "-i %source_files_comma% -o ""%source_temp_file%.dgim"" -h"
        dgimNoDemux.SourceFilters = {"DGSourceIM"}
        dgimNoDemux.Active = False
        ret.Add(dgimNoDemux)

        Dim dgimDemux As New CommandLineDemuxer
        dgimDemux.Name = "DGIndexIM: Demux & Index"
        dgimDemux.InputExtensions = {"mpg", "vob", "ts", "m2ts", "mts", "m2t"}
        dgimDemux.OutputExtensions = {"dgim"}
        dgimDemux.InputFormats = {"hevc", "avc", "vc1", "mpeg2"}
        dgimDemux.Command = "%app:DGIndexIM%"
        dgimDemux.Arguments = "-i %source_files_comma% -o ""%source_temp_file%.dgim"" -a -h"
        dgimDemux.SourceFilters = {"DGSourceIM"}
        dgimDemux.Active = False
        ret.Add(dgimDemux)

        Return ret
    End Function
End Class

<Serializable()>
Public Class CommandLineDemuxer
    Inherits Demuxer

    Property Command As String = ""
    Property Arguments As String = ""

    Overrides ReadOnly Property HasConfigDialog() As Boolean
        Get
            Return True
        End Get
    End Property

    Overrides Function ShowConfigDialog() As DialogResult
        Using f As New CommandLineDemuxForm(Me)
            Return f.ShowDialog
        End Using
    End Function

    Overrides Sub Run(proj As Project)
        Using proc As New Proc
            If Command?.Contains("DGIndexNV") Then
                If Not Package.DGIndexNV.VerifyOK(True) Then Throw New AbortException
                proc.Package = Package.DGIndexNV
                proc.SkipPatterns = {"^\d+$"}
            ElseIf Command?.Contains("DGIndexIM") Then
                If Not Package.DGIndexIM.VerifyOK(True) Then Throw New AbortException
                proc.Package = Package.DGIndexIM
            ElseIf Command?.Contains("ffmpeg") Then
                proc.Package = Package.ffmpeg
                proc.SkipStrings = {"frame=", "size="}
            ElseIf Command?.Contains("DGIndex") Then
                If Not Package.DGIndex.VerifyOK(True) Then Throw New AbortException
                proc.Package = Package.DGIndex
                proc.SkipPatterns = {"^\d+$"}
            ElseIf Command?.Contains("dsmux") Then
                If Not Package.Haali.VerifyOK(True) Then Throw New AbortException
                proc.SkipString = "Muxing..."
                ''  ElseIf Command?.Contains("Java") Then
                '' If Not Package.Java.VerifyOK(True) Then Throw New AbortException
                '' proc.SkipPatterns = {"^\d+ %$"}
            End If

            proc.Header = Name
            proc.File = Macro.Expand(Command)
            proc.Arguments = Macro.Expand(Arguments)
            proc.Start()

            If Command?.Contains("DGIndex") Then
                FileHelp.Move(p.SourceFile.DirAndBase + ".log", p.TempDir + p.SourceFile.Base + "_dg.log")
                FileHelp.Move(p.TempDir + p.SourceFile.Base + ".demuxed.m2v", p.TempDir + p.SourceFile.Base + ".m2v")
            End If
        End Using
    End Sub

    Shared Function IsActive(value As String) As Boolean
        For Each i In s.Demuxers.OfType(Of CommandLineDemuxer)()
            If i.Active AndAlso (i.Name.Contains(value) OrElse i.Command.Contains(value)) Then
                Return True
            End If
        Next
    End Function
End Class

<Serializable()>
Public Class eac3toDemuxer
    Inherits Demuxer

    Sub New()
        Name = "eac3to: Demux"
        InputExtensions = {"m2ts"}
        OutputExtensions = {"h264", "mkv", "m2v"}
    End Sub

    Overrides Sub Run(proj As Project)
        If proj.NoDialogs OrElse proj.BatchMode Then Exit Sub

        Using form As New eac3toForm(proj)
            form.M2TSFile = proj.SourceFile
            form.teTempDir.Text = proj.TempDir

            If form.ShowDialog() = DialogResult.OK Then
                Using proc As New Proc
                    proc.Project = proj
                    proc.SkipStrings = {"analyze: ", "process: "}
                    proc.TrimChars = {"-"c, " "c}
                    proc.RemoveChars = {CChar(VB6.vbBack)}
                    proc.Header = "Demux M2TS"
                    proc.Package = Package.eac3to
                    proc.Arguments = form.GetArgs(proj.SourceFile.Escape, proj.SourceFile.Base)

                    Try
                        proc.Start()
                    Catch ex As Exception
                        g.ShowException(ex)
                        Throw New AbortException
                    End Try

                    If Not form.cbVideoOutput.Text = "Nothing" Then
                        proj.SourceFile = form.OutputFolder + proj.SourceFile.Base + "." + form.cbVideoOutput.Text.ToLower
                        proj.SourceFiles.Clear()
                        proj.SourceFiles.Add(proj.SourceFile)
                        proj.TempDir = form.OutputFolder
                    End If
                End Using
            Else
                Throw New AbortException
            End If
        End Using
    End Sub

    Public Overrides ReadOnly Property HasConfigDialog As Boolean
        Get
            Return True
        End Get
    End Property
End Class

<Serializable>
Public Class ffmpegDemuxer
    Inherits Demuxer

    Sub New()
        Name = "ffmpeg: Demux"
        InputExtensions = {"avi", "flv"}
    End Sub

    Public Overrides Sub Run(proj As Project)
        Dim audioStreams As List(Of AudioStream)
        Dim subtitles As List(Of Subtitle)
        Dim videoDemuxing As Boolean

        Dim audioDemuxing = Not (TypeOf proj.Audio0 Is NullAudioProfile AndAlso
            TypeOf proj.Audio1 Is NullAudioProfile) AndAlso
            MediaInfo.GetAudioCount(proj.SourceFile) > 0

        Dim subtitlesDemuxing = MediaInfo.GetSubtitleCount(proj.SourceFile) > 0

        If Not proj.NoDialogs AndAlso Not proj.BatchMode AndAlso
            ((audioDemuxing AndAlso proj.DemuxAudio = DemuxMode.Dialog) OrElse
            (subtitlesDemuxing AndAlso proj.DemuxSubtitles = DemuxMode.Dialog)) OrElse
            Not proj Is p Then

            Using form As New StreamDemuxForm(Me, proj.SourceFile, Nothing)
                form.cbDemuxChapters.Visible = False

                If form.ShowDialog() = DialogResult.OK Then
                    videoDemuxing = form.cbDemuxVideo.Checked
                    audioStreams = form.AudioStreams
                    subtitles = form.Subtitles
                Else
                    Throw New AbortException
                End If
            End Using
        End If

        If videoDemuxing Then DemuxVideo(proj)

        If audioDemuxing AndAlso proj.DemuxAudio <> DemuxMode.None Then
            If audioStreams Is Nothing Then audioStreams = MediaInfo.GetAudioStreams(proj.SourceFile)

            For Each i In audioStreams
                If i.Enabled Then DemuxAudio(proj.SourceFile, i, Nothing, proj)
            Next
        End If

        Log.Save(proj)
    End Sub

    Shared Sub DemuxVideo(proj As Project)
        Dim streams = MediaInfo.GetVideoStreams(proj.SourceFile)
        If streams.Count = 0 Then Exit Sub
        Dim outPath = proj.TempDir + proj.SourceFile.Base + streams(0).ExtFull
        If outPath = proj.SourceFile Then Exit Sub
        Dim args = "-i " + proj.SourceFile.Escape
        args += " -c:v copy -an -sn -y -hide_banner"
        args += " " + outPath.Escape

        Using proc As New Proc
            proc.Project = proj
            proc.Header = "Demux video"
            proc.SkipStrings = {"frame=", "size="}
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If File.Exists(outPath) Then
            proj.Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            proj.Log.Write("Error", "no video output found")
        End If
    End Sub

    Shared Sub DemuxAudio(sourcefile As String, stream As AudioStream, ap As AudioProfile, proj As Project)
        Dim outPath = proj.TempDir + Audio.GetBaseNameForStream(sourcefile, stream) + stream.Extension
        If outPath.Length > 259 Then outPath = proj.TempDir + Audio.GetBaseNameForStream(sourcefile, stream, True) + stream.Extension
        Dim streamIndex = stream.StreamOrder
        Dim args = "-i " + sourcefile.Escape
        If MediaInfo.GetAudioCount(sourcefile) > 1 Then args += " -map 0:a:" & stream.Index
        args += " -vn -sn -y -hide_banner"
        If outPath.Ext = "wav" Then args += " -c:a pcm_s16le" Else args += " -c:a copy"
        args += " " + outPath.Escape

        Using proc As New Proc
            proc.Project = proj
            proc.Header = "Demux audio"
            proc.SkipStrings = {"frame=", "size="}
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using

        If File.Exists(outPath) Then
            If Not ap Is Nothing Then ap.File = outPath
            proj.Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            proj.Log.Write("Error", "no audio output found")
        End If
    End Sub

    Sub DemuxSubtitles(subtitles As List(Of Subtitle), proj As Project)
        If subtitles.Where(Function(subtitle) subtitle.Enabled).Count = 0 Then Exit Sub

        For Each subtitle In subtitles
            If Not subtitle.Enabled Then Continue For

            Dim args = "-i " + proj.SourceFile.Escape
            Dim outpath = proj.TempDir + proj.SourceFile.Base + " " + subtitle.Filename + subtitle.ExtFull

            If outpath.Length > 259 Then
                outpath = proj.TempDir + proj.SourceFile.Base.Shorten(10) + " " + subtitle.Filename.Shorten(10) + subtitle.ExtFull
            End If

            If MediaInfo.GetSubtitleCount(proj.SourceFile) > 1 Then args += " -map 0:s:" & subtitle.Index
            args += " -c:s copy -vn -an -y -hide_banner " + outpath.Escape

            Using proc As New Proc
                proc.Project = proj
                proc.Header = "Demux subtitles"
                proc.SkipStrings = {"frame=", "size="}
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.ffmpeg
                proc.Arguments = args
                proc.Start()
            End Using
        Next
    End Sub

    Public Overrides Property OutputExtensions As String()
        Get
            Return FileTypes.VideoDemuxOutput
        End Get
        Set(value As String())
        End Set
    End Property

    Public Overrides ReadOnly Property HasConfigDialog As Boolean
        Get
            Return True
        End Get
    End Property
End Class

<Serializable()>
Public Class MP4BoxDemuxer
    Inherits Demuxer

    Sub New()
        Name = "MP4Box: Demux"
        InputExtensions = {"mp4", "m4v", "mov"}
    End Sub

    Overrides Sub Run(proj As Project)
        Dim audioStreams As List(Of AudioStream)
        Dim subtitles As List(Of Subtitle)

        Dim demuxAudio = Not (TypeOf proj.Audio0 Is NullAudioProfile AndAlso
            TypeOf proj.Audio1 Is NullAudioProfile) AndAlso
            MediaInfo.GetAudioCount(proj.SourceFile) > 0

        Dim demuxSubtitles = MediaInfo.GetSubtitleCount(proj.SourceFile) > 0
        Dim attachments = GetAttachments(proj.SourceFile)
        Dim demuxChapters = ChaptersDemuxing

        If Not proj.NoDialogs AndAlso Not proj.BatchMode AndAlso
            ((demuxAudio AndAlso proj.DemuxAudio = DemuxMode.Dialog) OrElse
            (demuxSubtitles AndAlso proj.DemuxSubtitles = DemuxMode.Dialog)) OrElse
            Not proj Is p Then

            Using form As New StreamDemuxForm(Me, proj.SourceFile, attachments)
                If form.ShowDialog() = DialogResult.OK Then
                    VideoDemuxed = form.cbDemuxVideo.Checked
                    demuxChapters = form.cbDemuxChapters.Checked
                    audioStreams = form.AudioStreams
                    subtitles = form.Subtitles
                Else
                    Throw New AbortException
                End If
            End Using
        End If

        If VideoDemuxed Then DemuxVideo(proj)

        If demuxAudio AndAlso proj.DemuxAudio <> DemuxMode.None Then
            If audioStreams Is Nothing Then audioStreams = MediaInfo.GetAudioStreams(proj.SourceFile)

            For Each i In audioStreams
                If i.Enabled Then MP4BoxDemuxer.Demux(proj.SourceFile, i, Nothing, proj)
            Next
        End If

        If demuxSubtitles AndAlso proj.DemuxSubtitles <> DemuxMode.None Then
            If subtitles Is Nothing Then subtitles = MediaInfo.GetSubtitles(proj.SourceFile)

            For Each i In subtitles
                If Not i.Enabled Then Continue For

                Dim outpath = proj.TempDir + proj.SourceFile.Base + " " + i.Filename + i.ExtFull
                If outpath.Length > 259 Then outpath = proj.TempDir + proj.SourceFile.Base.Shorten(10) + " " + i.Filename.Shorten(20) + i.ExtFull

                FileHelp.Delete(outpath)
                Dim args As String

                Select Case i.ExtFull
                    Case ""
                        Continue For
                    Case ".srt"
                        args = "-srt "
                    Case Else
                        args = "-raw "
                End Select

                args += i.ID & " -out " + outpath.Escape + " " + proj.SourceFile.Escape

                Using proc As New Proc
                    proc.Project = proj
                    proc.Header = "Demux subtitle"
                    proc.SkipString = "|"
                    proc.Package = Package.MP4Box
                    proc.Arguments = args
                    proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
                    proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
                    proc.Start()
                End Using
            Next
        End If

        If Not attachments.NothingOrEmpty AndAlso attachments(0).Enabled Then
            Using proc As New Proc
                proc.Project = proj
                proc.SkipString = "|"
                proc.Header = "Extract cover"
                proc.Package = Package.MP4Box
                proc.Arguments = "-dump-cover " + proj.SourceFile.Escape + " -out " + (proj.TempDir + "cover.jpg").Escape
                proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
                proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
                proc.Start()
            End Using
        End If

        If demuxChapters AndAlso MediaInfo.GetMenu(proj.SourceFile, "Chapters_Pos_End").ToInt -
            MediaInfo.GetMenu(proj.SourceFile, "Chapters_Pos_Begin").ToInt > 0 Then

            Using proc As New Proc
                proc.Project = proj
                proc.Header = "Extract chapters"
                proc.SkipString = "|"
                proc.Package = Package.MP4Box
                proc.Arguments = "-dump-chap-ogg " + proj.SourceFile.Escape + " -out " + (proj.TempDir + proj.SourceFile.Base + "_chapters.txt").Escape
                proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
                proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
                proc.Start()
            End Using
        End If

        Log.Save(proj)
    End Sub

    Public Overrides Property OutputExtensions As String()
        Get
            If VideoDemuxing OrElse VideoDemuxed Then Return {"avi", "mpg", "h264", "h265"}
            Return {}
        End Get
        Set(value As String())
        End Set
    End Property

    Shared Sub DemuxVideo(proj As Project)
        Dim streams = MediaInfo.GetVideoStreams(proj.SourceFile)
        If streams.Count = 0 Then Exit Sub
        Dim outpath = proj.TempDir + proj.SourceFile.Base + streams(0).ExtFull
        If outpath = proj.SourceFile Then Exit Sub
        Dim args = If(streams(0).Ext = "avi", "-avi ", "-raw ")
        args += streams(0).ID & " -out " + outpath.Escape + " " + proj.SourceFile.Escape

        FileHelp.Delete(outpath)

        Using proc As New Proc
            proc.Project = proj
            proc.Header = "Demux video"
            proc.SkipString = "|"
            proc.Package = Package.MP4Box
            proc.Arguments = args
            proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
            proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
            proc.Start()
        End Using

        If File.Exists(outpath) Then
            proj.Log.WriteLine(MediaInfo.GetSummary(outpath))
        Else
            proj.Log.Write("Error", "no video output found")
            ffmpegDemuxer.DemuxVideo(proj)
        End If
    End Sub

    Shared Sub Demux(sourcefile As String, stream As AudioStream, ap As AudioProfile, proj As Project)
        If MediaInfo.GetAudio(sourcefile, "Format") = "PCM" Then
            ffmpegDemuxer.DemuxAudio(sourcefile, stream, ap, proj)
            Exit Sub
        End If

        Dim outPath = proj.TempDir + Audio.GetBaseNameForStream(sourcefile, stream) + stream.Extension
        If outPath.Length > 259 Then outPath = proj.TempDir + Audio.GetBaseNameForStream(sourcefile, stream, True) + stream.Extension
        FileHelp.Delete(outPath)
        Dim args As String
        If stream.Format = "AAC" Then args += "-single" Else args += "-raw"
        args += " " & stream.ID & " -out " + outPath.Escape + " " + sourcefile.Escape

        Using proc As New Proc
            proc.Project = proj
            proc.Header = "Demux audio"
            proc.SkipString = "|"
            proc.Package = Package.MP4Box
            proc.Arguments = args
            proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
            proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
            proc.Start()
        End Using

        If File.Exists(outPath) Then
            If MediaInfo.GetAudio(outPath, "Format") = "" Then
                proj.Log.Write("Error", "No format detected by MediaInfo.")
                ffmpegDemuxer.DemuxAudio(sourcefile, stream, ap, proj)
            Else
                If Not ap Is Nothing Then ap.File = outPath
                proj.Log.WriteLine(MediaInfo.GetSummary(outPath))
            End If
        Else
            proj.Log.Write("Error", "no output found")
            ffmpegDemuxer.DemuxAudio(sourcefile, stream, ap, proj)
        End If
    End Sub

    Function GetAttachments(sourceFilePath As String) As List(Of Attachment)
        Dim cover = MediaInfo.GetGeneral(sourceFilePath, "Cover")
        If cover <> "" Then Return New List(Of Attachment) From {New Attachment With {.Name = "Cover"}}
    End Function

    Public Overrides ReadOnly Property HasConfigDialog As Boolean
        Get
            Return True
        End Get
    End Property
End Class

<Serializable()>
Public Class mkvDemuxer
    Inherits Demuxer

    Sub New()
        Name = "mkvextract: Demux"
        InputExtensions = {"mkv", "webm"}
    End Sub

    Overrides Sub Run(proj As Project)
        Dim audioStreams As List(Of AudioStream)
        Dim subtitles As List(Of Subtitle)
        Dim stdout = ProcessHelp.GetStdOut(Package.mkvmerge.Path, "--identify --ui-language en " + proj.SourceFile.Escape)

        Dim demuxAudio = (Not (TypeOf proj.Audio0 Is NullAudioProfile AndAlso
            TypeOf proj.Audio1 Is NullAudioProfile)) AndAlso
            MediaInfo.GetAudioCount(proj.SourceFile) > 0

        Dim demuxSubtitles = MediaInfo.GetSubtitleCount(proj.SourceFile) > 0
        Dim attachments = GetAttachments(stdout)
        Dim demuxChapters = ChaptersDemuxing

        If Not proj.NoDialogs AndAlso Not proj.BatchMode AndAlso
            ((demuxAudio AndAlso proj.DemuxAudio = DemuxMode.Dialog) OrElse
            (demuxSubtitles AndAlso proj.DemuxSubtitles = DemuxMode.Dialog)) OrElse
            Not proj Is p Then

            Using form As New StreamDemuxForm(Me, proj.SourceFile, attachments)
                If form.ShowDialog() <> DialogResult.OK Then Throw New AbortException
                VideoDemuxed = form.cbDemuxVideo.Checked
                demuxChapters = form.cbDemuxChapters.Checked
                audioStreams = form.AudioStreams
                subtitles = form.Subtitles
            End Using
        End If

        If demuxAudio AndAlso proj.DemuxAudio <> DemuxMode.None Then
            If audioStreams Is Nothing Then audioStreams = MediaInfo.GetAudioStreams(proj.SourceFile)
        End If

        If demuxSubtitles AndAlso proj.DemuxSubtitles <> DemuxMode.None Then
            If subtitles Is Nothing Then subtitles = MediaInfo.GetSubtitles(proj.SourceFile)
        End If

        Demux(proj.SourceFile, audioStreams, subtitles, Nothing, proj, True, VideoDemuxed)

        If demuxChapters AndAlso stdout.Contains("Chapters: ") Then
            Using proc As New Proc
                proc.Project = proj
                proc.Header = "Demux xml chapters"
                proc.SkipString = "Progress: "
                proc.WriteLog(stdout + BR)
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.mkvextract
                proc.Arguments = proj.SourceFile.Escape + " chapters " + (proj.TempDir + proj.SourceFile.Base + "_chapters.xml").Escape
                proc.AllowedExitCodes = {0, 1, 2}
                proc.Start()
            End Using

            Using proc As New Proc
                proc.Project = proj
                proc.Header = "Demux ogg chapters"
                proc.SkipString = "Progress: "
                proc.WriteLog(stdout + BR)
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.mkvextract
                proc.Arguments = proj.SourceFile.Escape + " chapters " + (proj.TempDir + proj.SourceFile.Base + "_chapters.txt").Escape + " --simple"
                proc.AllowedExitCodes = {0, 1, 2}
                proc.Start()
            End Using
        End If

        Dim enabledAttachments = attachments.Where(Function(val) val.Enabled)

        If enabledAttachments.Count > 0 Then
            Using proc As New Proc
                proc.Project = proj
                proc.Header = "Demux attachments"
                proc.SkipString = "Progress: "
                proc.WriteLog(stdout + BR)
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.mkvextract
                proc.Arguments = proj.SourceFile.Escape + " attachments " +
                    enabledAttachments.Select(Function(val) val.ID & ":" + GetAttachmentPath(
                    proj.TempDir, val.Name).Escape).Join(" ")
                proc.AllowedExitCodes = {0, 1, 2}
                proc.Start()
            End Using
        End If

        If MediaInfo.GetVideo(proj.SourceFile, "FrameRate_Mode") = "VFR" Then
            Dim streamOrder = MediaInfo.GetVideo(proj.SourceFile, "StreamOrder").ToInt

            Using proc As New Proc
                proc.Project = proj
                proc.Header = "Demux timestamps"
                proc.SkipString = "Progress: "
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.mkvextract
                proc.Arguments = "timestamps_v2 " + proj.SourceFile.Escape + " " & streamOrder & ":" + (proj.TempDir + proj.SourceFile.Base + "_timestamps.txt").Escape
                proc.AllowedExitCodes = {0, 1, 2}
                proc.Start()
            End Using
        End If

        Log.Save(proj)
    End Sub

    Public Overrides Property OutputExtensions As String()
        Get
            If VideoDemuxing OrElse VideoDemuxed Then Return {"avi", "mpg", "h264", "h265"}
            Return {}
        End Get
        Set(value As String())
        End Set
    End Property

    Shared Sub Demux(sourcefile As String,
                     audioStreams As IEnumerable(Of AudioStream),
                     subtitles As IEnumerable(Of Subtitle),
                     ap As AudioProfile,
                     proj As Project,
                     onlyEnabled As Boolean,
                     videoDemuxing As Boolean)

        If audioStreams Is Nothing Then audioStreams = New List(Of AudioStream)
        If subtitles Is Nothing Then subtitles = New List(Of Subtitle)

        If onlyEnabled Then audioStreams = audioStreams.Where(Function(arg) arg.Enabled)
        subtitles = subtitles.Where(Function(subtitle) subtitle.Enabled)

        If audioStreams.Count = 0 AndAlso subtitles.Count = 0 AndAlso Not videoDemuxing Then Exit Sub

        Dim args = sourcefile.Escape + " tracks"

        If videoDemuxing Then
            Dim stdout = ProcessHelp.GetStdOut(Package.mkvmerge.Path, "--identify " + sourcefile.Escape)
            Dim id = Regex.Match(stdout, "Track ID (\d+): video").Groups(1).Value.ToInt
            Dim outpath = proj.TempDir + sourcefile.Base + MediaInfo.GetVideoStreams(sourcefile)(0).ExtFull
            If outpath <> sourcefile Then args += " " & id & ":" + outpath.Escape
        End If

        For Each subtitle In subtitles
            If Not subtitle.Enabled Then Continue For
            Dim forced = If(subtitle.Forced, "_forced", "")
            Dim outpath = proj.TempDir + sourcefile.Base + " " + subtitle.Filename + forced + subtitle.ExtFull
            If outpath.Length > 259 Then outpath = proj.TempDir + sourcefile.Base.Shorten(10) + " " + subtitle.Filename.Shorten(10) + forced + subtitle.ExtFull
            args += " " & subtitle.StreamOrder & ":" + outpath.Escape
        Next

        Dim outPaths As New Dictionary(Of String, AudioStream)

        For Each stream In audioStreams
            Dim ext = stream.Extension
            If ext = ".m4a" Then ext = ".aac"
            Dim outPath = proj.TempDir + Audio.GetBaseNameForStream(sourcefile, stream) + ext
            If outPath.Length > 259 Then outPath = proj.TempDir + Audio.GetBaseNameForStream(sourcefile, stream, True) + ext
            outPaths.Add(outPath, stream)
            args += " " & stream.StreamOrder & ":" + outPath.Escape
        Next

        Using proc As New Proc
            proc.Project = proj
            proc.Header = "Demux MKV"
            proc.SkipString = "Progress: "
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.mkvextract
            proc.Arguments = args + " --ui-language en"
            proc.AllowedExitCodes = {0, 1, 2}
            proc.Start()
        End Using

        For Each outPath In outPaths.Keys
            If File.Exists(outPath) Then
                If Not ap Is Nothing Then ap.File = outPath
                proj.Log.WriteLine(MediaInfo.GetSummary(outPath) + BR)

                If outPath.Ext = "aac" Then
                    Dim m4aPath As String

                    Using proc As New Proc
                        proc.Project = proj
                        proc.Header = "Mux AAC to M4A"
                        proc.SkipString = "|"
                        proc.Package = Package.MP4Box
                        Dim sbr = If(outPath.Contains("SBR"), ":sbr", "")
                        m4aPath = outPath.ChangeExt("m4a")
                        proc.Arguments = "-add """ + outPath + sbr + ":name= "" -new " + m4aPath.Escape
                        proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
                        proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
                        proc.Start()
                    End Using

                    If File.Exists(m4aPath) Then
                        If Not ap Is Nothing Then ap.File = m4aPath
                        FileHelp.Delete(outPath)
                        proj.Log.WriteLine(BR + MediaInfo.GetSummary(m4aPath))
                    Else
                        Throw New ErrorAbortException("Error mux AAC to M4A", outPath)
                    End If
                End If
            Else
                proj.Log.Write("Error", "no output found")
                ffmpegDemuxer.DemuxAudio(sourcefile, outPaths(outPath), ap, proj)
            End If
        Next
    End Sub

    Shared Function GetAttachmentPath(dir As String, name As String) As String
        Dim prefix = If(name.Base.EqualsAny("cover", "small_cover", "cover_land", "small_cover_land"), "", "_attachment_")
        Dim ret = dir + prefix + name.Base + name.ExtFull
        If ret.Length > 260 Then ret = dir + prefix + name.Base.Shorten(10) + name.ExtFull
        Return ret
    End Function

    Function GetAttachments(stdout As String) As List(Of Attachment)
        Dim ret = New List(Of Attachment)

        For Each i In stdout.SplitLinesNoEmpty
            If i.StartsWith("Attachment ID ") Then
                Dim match = Regex.Match(i, "Attachment ID (\d+):.+, file name '(.+)'")
                If match.Success Then ret.Add(New Attachment With {
                                              .ID = match.Groups(1).Value.ToInt,
                                              .Name = match.Groups(2).Value})
            End If
        Next

        Return ret
    End Function

    Public Overrides ReadOnly Property HasConfigDialog As Boolean
        Get
            Return True
        End Get
    End Property
End Class