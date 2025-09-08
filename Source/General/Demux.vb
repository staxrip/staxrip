﻿
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml

<Serializable()>
Public MustInherit Class Demuxer
    MustOverride Sub Run(proj As Project)

    Overridable Property Active As Boolean = True
    Overridable Property InputExtensions As String() = {}
    Overridable Property InputFormats As String() = {}
    Overridable Property Name As String = ""
    Overridable Property OutputExtensions As String() = {}
    Overridable Property SourceFilters As String() = {}

    Property OverrideExisting As Boolean

    Overridable ReadOnly Property HasConfigDialog() As Boolean
        Get
            Return False
        End Get
    End Property

    Overridable Function ShowConfigDialog() As DialogResult
        Using form As New SimpleSettingsForm(Name)
            form.ScaleClientSize(23, 11)
            Dim ui = form.SimpleUI
            Dim page = ui.CreateFlowPage("main page")

            Dim tb = ui.AddText(page)
            tb.Label.Text = "Supported Input File Types:"
            tb.Edit.Text = InputExtensions.Join(" ")
            tb.Edit.SaveAction = Sub(value) InputExtensions = value.ToLowerInvariant.SplitNoEmptyAndWhiteSpace(",", ";", " ")

            Dim cb = ui.AddBool(page)
            cb.Text = "Recreate already existing files"
            cb.Help = "Demux even when ouput files already exist."
            cb.Checked = OverrideExisting
            cb.SaveAction = Sub(val) OverrideExisting = val

            Dim ret = form.ShowDialog()

            If ret = DialogResult.OK Then
                ui.Save()
            End If

            Return ret
        End Using
    End Function

    Overrides Function ToString() As String
        Dim input = InputExtensions.Join(", ")

        If input.Length > 30 Then
            input = input.Shorten(30) + "..."
        End If

        Return Name + " (" + input + " -> " + OutputExtensions.Join(", ") + ")"
    End Function

    Overridable Function GetHelp() As String
        For Each i In Package.Items.Values
            If Name = i.Name Then
                Return i.Description
            End If
        Next
    End Function

    Shared Function GetDefaults() As List(Of Demuxer)
        Dim ret As New List(Of Demuxer) From {
            New eac3toDemuxer With {.Active = False},
            New CommandLineDemuxer With {
                .Name = "ffmpeg: Re-mux (M2)TS to MKV",
                .Active = False,
                .InputExtensions = {"m2ts", "ts"},
                .OutputExtensions = {"mkv"},
                .InputFormats = {"hevc", "avc"},
                .Command = "%app:ffmpeg%",
                .Arguments = "-y -hide_banner -probesize 10M -i ""%source_file%"" -map 0 -dn -c copy -ignore_unknown ""%temp_file%.mkv"""
            },
            New ffmpegDemuxer,
            New mkvDemuxer,
            New MP4BoxDemuxer,
            New CommandLineDemuxer With {
                .Name = "DGIndex: Demux & Index MPEG-2",
                .InputExtensions = {"mpg", "mpeg", "vob", "m2ts", "m2v", "mts", "m2t"},
                .OutputExtensions = {"d2v"},
                .InputFormats = {"mpeg2"},
                .Command = "%app:DGIndex%",
                .Arguments = "-i %source_files% -ia 2 -fo 0 -yr 1 -tn 1 -om 2 -drc 2 -dsd 0 -dsa 0 -o ""%temp_file%"" -hide -exit",
                .SourceFilters = {"MPEG2Source", "D2VSource", "d2v.Source"}
            },
            New CommandLineDemuxer With {
                .Name = "D2V Witch: Demux & Index MPEG-2",
                .InputExtensions = {"mpg", "mpeg", "vob", "m2ts", "m2v", "mts", "m2t"},
                .OutputExtensions = {"d2v"},
                .InputFormats = {"mpeg2"},
                .Command = "cmd.exe",
                .Arguments = "/S /C """"%app:D2V Witch%"" --audio-ids all --output ""%temp_file%.d2v"" %source_files%""",
                .SourceFilters = {"MPEG2Source", "D2VSource", "d2v.Source"},
                .Active = False
            }
        }

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
        Using form As New CommandLineDemuxForm(Me)
            Return form.ShowDialog
        End Using
    End Function

    Overrides Sub Run(proj As Project)
        Using proc As New Proc
            Dim test = Macro.Expand(Command + Arguments).ToLowerEx.RemoveChars(" ")

            If test.Contains("dgindex") AndAlso Not test.Contains("dgindexnv") Then
                proc.Package = Package.DGIndex
                proc.IntegerPercentOutput = True
            ElseIf test.Contains("d2vwitch") Then
                proc.Package = Package.D2VWitch
                proc.SkipString = "%"
            Else
                proc.SkipStrings = {"frame=", "size="}
            End If

            proc.Header = Name
            proc.File = Macro.Expand(Command)
            proc.Arguments = Macro.Expand(Arguments)
            proc.Start()

            If Command?.Contains("DGIndex") Then
                FileHelp.Move(p.SourceFile.DirAndBase + ".log", Path.Combine(p.TempDir, p.SourceFile.Base + "_dg.log"))
                FileHelp.Move(Path.Combine(p.TempDir, p.SourceFile.Base + ".demuxed.m2v"), Path.Combine(p.TempDir, p.SourceFile.Base + ".m2v"))
            End If
        End Using

        If p.ExtractTimestamps <> TimestampsMode.Never AndAlso proj.SourceFile.ToLowerEx().EndsWithEx(".mkv") Then
            If p.ExtractTimestamps = TimestampsMode.Always OrElse proj.SourceFrameRateMode = "VFR" Then
                Dim streamOrder = MediaInfo.GetVideo(proj.SourceFile, "StreamOrder").ToInt

                Using proc As New Proc
                    proc.Project = proj
                    proc.Header = "Demux timestamps"
                    proc.SkipString = "Progress: "
                    proc.Encoding = Encoding.UTF8
                    proc.Package = Package.mkvextract
                    proc.Arguments = "--ui-language en timestamps_v2 " + proj.SourceFile.Escape + " " & streamOrder & ":" + Path.Combine(proj.TempDir, proj.SourceFile.Base + "_timestamps.txt").Escape
                    proc.AllowedExitCodes = {0, 1, 2}
                    proc.Start()
                End Using
            End If
        End If
    End Sub

    Shared Function IsActive(value As String) As Boolean
        For Each demuxer In s.Demuxers.OfType(Of CommandLineDemuxer)()
            If demuxer.Active AndAlso (demuxer.Name.Contains(value) OrElse demuxer.Command.Contains(value)) Then
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
        If proj.NoDialogs OrElse proj.BatchMode Then
            Exit Sub
        End If

        Using form As New eac3toForm(proj)
            form.M2TSFile = proj.SourceFile
            form.teTempDir.Text = proj.TempDir

            If form.ShowDialog() = DialogResult.OK Then
                Using proc As New Proc
                    proc.Project = proj
                    proc.SkipStrings = {"analyze: ", "process: "}
                    proc.TrimChars = {"-"c, " "c}
                    proc.Header = "Demux M2TS"
                    proc.Package = Package.eac3to
                    Dim outFiles As New List(Of String)
                    proc.Arguments = form.GetArgs(proj.SourceFile.Escape, proj.SourceFile.Base, outFiles)
                    proc.OutputFiles = outFiles

                    Try
                        proc.Start()
                    Catch ex As Exception
                        g.ShowException(ex)
                        Throw New AbortException
                    End Try

                    If proj Is p Then
                        Dim selectedLanguages = form.Streams.Where(Function(i) i.IsAudio AndAlso i.Checked).Select(Function(i) i.Language.ThreeLetterCode)
                        Dim preferredAudios = p.PreferredAudio.ToLowerInvariant.SplitNoEmptyAndWhiteSpace(",", ";", " ").Union(selectedLanguages).Distinct()
                        p.PreferredAudio = preferredAudios.Join(" ")
                    End If

                    If Not form.cbVideoOutput.Text = "Nothing" Then
                        proj.SourceFile = Path.Combine(form.OutputFolder, proj.SourceFile.Base + "." + form.cbVideoOutput.Text.ToLowerInvariant)
                        proj.SourceFiles.Clear()
                        proj.SourceFiles.Add(proj.SourceFile)
                        proj.TempDir = form.OutputFolder.FixDir
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
        InputExtensions = {"avi", "flv", "m2ts", "ts"}
    End Sub

    Public Overrides Sub Run(proj As Project)
        Dim audioStreams As List(Of AudioStream) = Nothing
        Dim subtitles As List(Of Subtitle) = Nothing

        Dim videoDemuxing = proj.DemuxVideo
        Dim audioDemuxing = MediaInfo.GetAudioCount(proj.SourceFile) > 0
        Dim subtitlesDemuxing = MediaInfo.GetSubtitleCount(proj.SourceFile) > 0

        If Not proj.NoDialogs AndAlso Not proj.BatchMode AndAlso
            ((audioDemuxing AndAlso proj.DemuxAudio = DemuxMode.Dialog) OrElse
            (subtitlesDemuxing AndAlso proj.SubtitleMode = SubtitleMode.Dialog)) OrElse
            proj IsNot p Then

            Using form As New StreamDemuxForm(proj.SourceFile, Nothing)
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

        If videoDemuxing Then
            DemuxVideo(proj, OverrideExisting)
        End If

        If audioDemuxing AndAlso proj.DemuxAudio <> DemuxMode.None Then
            If audioStreams Is Nothing Then
                audioStreams = MediaInfo.GetAudioStreams(proj.SourceFile)
            End If

            For Each stream In audioStreams
                If stream.Enabled Then
                    DemuxAudio(proj.SourceFile, stream, Nothing, proj, OverrideExisting)
                End If
            Next
        End If

        If subtitlesDemuxing AndAlso proj.IsSubtitleDemuxingRequired Then
            subtitles = If(subtitles, MediaInfo.GetSubtitles(proj.SourceFile))
            DemuxSubtitles(subtitles, proj, OverrideExisting)
        End If

        Log.Save(proj)
    End Sub

    Shared Sub DemuxVideo(proj As Project, overrideExisting As Boolean)
        Dim streams = MediaInfo.GetVideoStreams(proj.SourceFile)

        If streams.Count = 0 OrElse streams(0).Ext = "" Then
            Exit Sub
        End If

        Dim outPath = Path.Combine(proj.TempDir, proj.SourceFile.Base + streams(0).ExtFull)

        If outPath = proj.SourceFile OrElse (Not overrideExisting AndAlso outPath.FileExists) Then
            Exit Sub
        End If

        Dim args = "-y -hide_banner -probesize 10M -i " + proj.SourceFile.Escape
        args += " -c:v copy -an -sn"
        args += " " + outPath.Escape

        Using proc As New Proc
            proc.Project = proj
            proc.Header = "Demux video"
            proc.SkipStrings = {"frame=", "size="}
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.OutputFiles = {outPath}
            proc.Start()
        End Using

        If File.Exists(outPath) Then
            proj.Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            proj.Log.Write("Error", "no video output found")
        End If
    End Sub

    Shared Sub DemuxAudio(
        sourcefile As String,
        stream As AudioStream,
        ap As AudioProfile,
        proj As Project,
        overrideExisting As Boolean)

        Dim outPath = Path.Combine(proj.TempDir, Audio.GetBaseNameForStream(sourcefile, stream) + stream.ExtFull)

        If Not overrideExisting AndAlso outPath.FileExists Then
            Exit Sub
        End If

        Dim args = "-y -hide_banner -probesize 20M -analyzeduration 20M -i " + sourcefile.Escape

        If MediaInfo.GetAudioCount(sourcefile) > 1 Then
            args += " -map 0:a:" & stream.Index
        End If

        args += " -vn -sn"

        If stream.FormatString.ToLowerEx().StartsWith("dts") Then
            If outPath.Ext.ToLowerEx().StartsWith("dts") Then args += $" -f dts"
            args += " -c:a copy"
        ElseIf stream.FormatString.ToLowerEx() = "pcm" Then
            Dim bd = stream.BitDepth
            If bd = 20 Then bd = 24

            If outPath.Ext.ToLowerEx() = "pcm" Then args += $" -f s{bd}le"
            args += $" -c:a pcm_s{bd}le"
        Else
            args += " -c:a copy"
        End If

        args += " " + outPath.Escape

        Using proc As New Proc
            proc.Project = proj
            proc.Header = "Demux Audio"
            proc.SkipStrings = {"frame=", "size="}
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.OutputFiles = {outPath}
            proc.Start()
        End Using

        If File.Exists(outPath) Then
            If ap IsNot Nothing Then
                ap.File = outPath
            End If

            proj.Log.WriteLine(MediaInfo.GetSummary(outPath))
        Else
            proj.Log.Write("Error", "no audio output found")
        End If
    End Sub

    Sub DemuxSubtitles(subtitles As List(Of Subtitle), proj As Project, overrideExisting As Boolean)
        If subtitles Is Nothing Then Exit Sub
        If proj Is Nothing Then Exit Sub

        For Each subtitle In subtitles
            Dim skip As Boolean = False

            Select Case proj.SubtitleMode
                Case SubtitleMode.All
                    skip = False
                Case SubtitleMode.PreferredNoMux
                    Dim prefSubtitles = proj.PreferredSubtitles.ToLowerInvariant.SplitNoEmptyAndWhiteSpace(",", ";", " ")
                    skip = Not prefSubtitles.ContainsAny("all", subtitle.Language.TwoLetterCode.ToLowerInvariant(), subtitle.Language.ThreeLetterCode.ToLowerInvariant(), subtitle.Language.Name.ToLowerInvariant())
                Case Else
                    skip = Not subtitle.Enabled
            End Select

            If skip Then Continue For

            Dim outpath = Path.Combine(proj.TempDir, subtitle.Filename + subtitle.ExtFull)
            Dim args = "-y -hide_banner -probesize 10M -i " + proj.SourceFile.Escape
            Dim codec = If(subtitle.Ext = "srt", "srt", "copy")
            Dim format = ""

            If subtitle.Ext = "mks" Then
                codec = "dvbsub"
                format = " -f matroska"
            End If

            If Not overrideExisting AndAlso outpath.FileExists Then Exit Sub

            If MediaInfo.GetSubtitleCount(proj.SourceFile) > 1 Then
                args += " -map 0:s:" & subtitle.Index
            End If

            args += $" -c:s {codec}{format} -vn -an {outpath.Escape}"

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

    Private _videoDemuxing As Boolean = True

    Sub New()
        Name = "MP4Box: Demux"
        InputExtensions = {"mp4", "m4v", "mov"}
    End Sub

    Overrides Sub Run(proj As Project)
        Dim audioStreams As List(Of AudioStream) = Nothing
        Dim subtitles As List(Of Subtitle) = Nothing
        Dim attachments = GetAttachments(proj.SourceFile)
        Dim demuxAudio = MediaInfo.GetAudioCount(proj.SourceFile) > 0
        Dim demuxSubtitles = MediaInfo.GetSubtitleCount(proj.SourceFile) > 0
        Dim demuxChapters = proj.DemuxChapters
        Dim demuxAttachments = proj.DemuxAttachments
        _videoDemuxing = proj.DemuxVideo

        If Not proj.NoDialogs AndAlso Not proj.BatchMode AndAlso
            ((demuxAudio AndAlso proj.DemuxAudio = DemuxMode.Dialog) OrElse
            (demuxSubtitles AndAlso proj.SubtitleMode = SubtitleMode.Dialog)) OrElse
            proj IsNot p Then

            Using form As New StreamDemuxForm(proj.SourceFile, attachments)
                If form.ShowDialog() = DialogResult.OK Then
                    _videoDemuxing = form.cbDemuxVideo.Checked
                    demuxChapters = form.cbDemuxChapters.Checked
                    audioStreams = form.AudioStreams
                    subtitles = form.Subtitles
                Else
                    Throw New AbortException
                End If
            End Using
        End If

        If _videoDemuxing Then
            DemuxVideo(proj, OverrideExisting)
        End If

        If demuxAudio AndAlso proj.DemuxAudio <> DemuxMode.None Then
            If audioStreams Is Nothing Then
                audioStreams = MediaInfo.GetAudioStreams(proj.SourceFile)
            End If

            For Each stream In audioStreams
                If stream.Enabled Then
                    MP4BoxDemuxer.DemuxAudio(proj.SourceFile, stream, Nothing, proj, OverrideExisting)
                End If
            Next
        End If

        If demuxSubtitles AndAlso proj.IsSubtitleDemuxingRequired Then
            If subtitles Is Nothing Then
                subtitles = MediaInfo.GetSubtitles(proj.SourceFile)
            End If

            For Each subtitle In subtitles
                Dim skip As Boolean = False

                Select Case proj.SubtitleMode
                    Case SubtitleMode.All
                        skip = False
                    Case SubtitleMode.PreferredNoMux
                        Dim prefSubtitles = proj.PreferredSubtitles.ToLowerInvariant.SplitNoEmptyAndWhiteSpace(",", ";", " ")
                        skip = Not prefSubtitles.ContainsAny("all", subtitle.Language.TwoLetterCode.ToLowerInvariant(), subtitle.Language.ThreeLetterCode.ToLowerInvariant(), subtitle.Language.Name.ToLowerInvariant())
                    Case Else
                        skip = Not subtitle.Enabled
                End Select

                If skip Then Continue For

                Dim outpath = Path.Combine(proj.TempDir, subtitle.Filename + subtitle.ExtFull)

                If Not OverrideExisting AndAlso outpath.FileExists Then
                    Continue For
                End If

                FileHelp.Delete(outpath)
                Dim args As String

                Select Case subtitle.ExtFull
                    Case ""
                        Continue For
                    Case ".srt"
                        args = "-srt "
                    Case Else
                        args = "-raw "
                End Select

                args += subtitle.ID & " -out " + outpath.Escape + " " + proj.SourceFile.Escape

                Using proc As New Proc
                    proc.Project = proj
                    proc.Header = "Demux subtitle"
                    proc.SkipString = "|"
                    proc.Package = Package.MP4Box
                    proc.Arguments = args
                    proc.OutputFiles = {outpath}
                    proc.Start()
                End Using
            Next
        End If

        If demuxAttachments AndAlso Not attachments.NothingOrEmpty AndAlso attachments(0).Enabled Then
            Using proc As New Proc
                proc.Project = proj
                proc.SkipString = "|"
                proc.Header = "Extract cover"
                proc.Package = Package.MP4Box
                proc.Arguments = "-dump-cover " + proj.SourceFile.Escape + " -out " + Path.Combine(proj.TempDir, "cover.jpg").Escape
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
                proc.Arguments = "-dump-chap-ogg " + proj.SourceFile.Escape +
                    " -out " + Path.Combine(proj.TempDir, proj.SourceFile.Base + "_chapters.txt").Escape
                proc.Start()
            End Using
        End If

        Log.Save(proj)
    End Sub

    Public Overrides Property OutputExtensions As String()
        Get
            If p.DemuxVideo OrElse _videoDemuxing Then
                Return {"avi", "mpg", "h264", "h265"}
            End If

            Return {}
        End Get
        Set(value As String())
        End Set
    End Property

    Shared Sub DemuxVideo(proj As Project, overrideExisting As Boolean)
        Dim streams = MediaInfo.GetVideoStreams(proj.SourceFile)

        If streams.Count = 0 OrElse streams(0).Ext = "" Then
            Exit Sub
        End If

        Dim outpath = Path.Combine(proj.TempDir, proj.SourceFile.Base + streams(0).ExtFull)

        If outpath = proj.SourceFile OrElse (Not overrideExisting AndAlso outpath.FileExists) Then
            Exit Sub
        End If

        Dim args = If(streams(0).Ext = "avi", "-avi ", "-raw ")
        args += streams(0).ID & " -out " + outpath.Escape + " " + proj.SourceFile.Escape

        FileHelp.Delete(outpath)

        Using proc As New Proc
            proc.Project = proj
            proc.Header = "Demux video"
            proc.SkipString = "|"
            proc.Package = Package.MP4Box
            proc.Arguments = args
            proc.OutputFiles = {outpath}
            proc.Start()
        End Using

        If File.Exists(outpath) Then
            proj.Log.WriteLine(MediaInfo.GetSummary(outpath))
        Else
            proj.Log.Write("Error", "no video output found")
            ffmpegDemuxer.DemuxVideo(proj, overrideExisting)
        End If
    End Sub

    Shared Sub DemuxAudio(
        sourcefile As String,
        stream As AudioStream,
        ap As AudioProfile,
        proj As Project,
        overrideExisting As Boolean)

        If MediaInfo.GetAudio(sourcefile, "Format") = "PCM" Then
            ffmpegDemuxer.DemuxAudio(sourcefile, stream, ap, proj, overrideExisting)
            Exit Sub
        End If

        Dim outPath = Path.Combine(proj.TempDir, Audio.GetBaseNameForStream(sourcefile, stream) + stream.ExtFull)

        If Not overrideExisting AndAlso outPath.FileExists Then
            Exit Sub
        End If

        FileHelp.Delete(outPath)
        Dim args = ""

        If stream.Format.EqualsAny("AAC", "Opus") Then
            args += "-single"
        Else
            args += "-raw"
        End If

        args += " " & stream.ID & " -out " + outPath.Escape + " " + sourcefile.Escape

        Using proc As New Proc
            proc.Project = proj
            proc.Header = "Demux audio"
            proc.SkipString = "|"
            proc.Package = Package.MP4Box
            proc.Arguments = args
            proc.OutputFiles = {outPath}
            proc.Start()
        End Using

        If File.Exists(outPath) Then
            If MediaInfo.GetAudio(outPath, "Format") = "" Then
                proj.Log.Write("Error", "No format detected by MediaInfo.")
                ffmpegDemuxer.DemuxAudio(sourcefile, stream, ap, proj, overrideExisting)
            Else
                If Not ap Is Nothing Then ap.File = outPath
                proj.Log.WriteLine(MediaInfo.GetSummary(outPath))
            End If
        Else
            proj.Log.Write("Error", "no output found")
            ffmpegDemuxer.DemuxAudio(sourcefile, stream, ap, proj, overrideExisting)
        End If
    End Sub

    Function GetAttachments(sourceFilePath As String) As List(Of Attachment)
        Dim cover = MediaInfo.GetGeneral(sourceFilePath, "Cover")

        If cover <> "" Then
            Return New List(Of Attachment) From {New Attachment With {.Name = "Cover"}}
        End If
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

    Private ReadOnly _videoDemuxing As Boolean = True

    Sub New()
        Name = "mkvextract: Demux"
        InputExtensions = {"mkv", "webm"}
    End Sub

    Overrides Sub Run(proj As Project)
        Dim audioStreams As List(Of AudioStream) = Nothing
        Dim subtitles As List(Of Subtitle) = Nothing
        Dim stdout = ProcessHelp.GetConsoleOutput(Package.mkvmerge.Path, "--identify --ui-language en " + proj.SourceFile.Escape)
        Dim attachments = GetAttachments(stdout)
        Dim demuxAudio = MediaInfo.GetAudioCount(proj.SourceFile) > 0
        Dim demuxSubtitles = MediaInfo.GetSubtitleCount(proj.SourceFile) > 0
        Dim demuxChapters = proj.DemuxChapters
        Dim demuxAttachments = proj.DemuxAttachments
        Dim demuxTags = proj.DemuxTags
        Dim _videoDemuxing = proj.DemuxVideo

        If Not proj.NoDialogs AndAlso Not proj.BatchMode AndAlso ((demuxAudio AndAlso proj.DemuxAudio = DemuxMode.Dialog) OrElse (demuxSubtitles AndAlso proj.SubtitleMode = SubtitleMode.Dialog)) OrElse proj IsNot p Then
            Using form As New StreamDemuxForm(proj.SourceFile, attachments)
                If form.ShowDialog() <> DialogResult.OK Then
                    Throw New AbortException
                End If

                _videoDemuxing = form.cbDemuxVideo.Checked
                demuxChapters = form.cbDemuxChapters.Checked
                audioStreams = form.AudioStreams
                subtitles = form.Subtitles
            End Using
        End If

        If audioStreams Is Nothing AndAlso demuxAudio AndAlso proj.DemuxAudio <> DemuxMode.None Then
            audioStreams = MediaInfo.GetAudioStreams(proj.SourceFile)
        End If

        If subtitles Is Nothing AndAlso demuxSubtitles AndAlso proj.IsSubtitleDemuxingRequired Then
            subtitles = MediaInfo.GetSubtitles(proj.SourceFile)
        End If

        Demux(proj.SourceFile, audioStreams, subtitles, Nothing, proj, True, _videoDemuxing, OverrideExisting, "Demux MKV", True)

        If demuxChapters AndAlso stdout.Contains("Chapters: ") Then
            Using proc As New Proc
                proc.Project = proj
                proc.Header = "Demux xml chapters"
                proc.SkipString = "Progress: "
                proc.WriteLog(stdout + BR)
                proc.Encoding = Encoding.UTF8
                proc.Package = Package.mkvextract
                proc.Arguments = proj.SourceFile.Escape + " --ui-language en chapters " + Path.Combine(proj.TempDir, proj.SourceFile.Base + "_chapters.xml").Escape
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
                proc.Arguments = proj.SourceFile.Escape + " --ui-language en chapters " + Path.Combine(proj.TempDir, proj.SourceFile.Base + "_chapters.txt").Escape + " --simple"
                proc.AllowedExitCodes = {0, 1, 2}
                proc.Start()
            End Using
        End If

        If p.ExtractTimestamps <> TimestampsMode.Never AndAlso proj.SourceFile.ToLowerEx().EndsWithEx(".mkv") Then
            If p.ExtractTimestamps = TimestampsMode.Always OrElse MediaInfo.GetVideo(proj.SourceFile, "FrameRate_Mode") = "VFR" Then
                Dim streamOrder = MediaInfo.GetVideo(proj.SourceFile, "StreamOrder").ToInt

                Using proc As New Proc
                    proc.Project = proj
                    proc.Header = "Demux timestamps"
                    proc.SkipString = "Progress: "
                    proc.Encoding = Encoding.UTF8
                    proc.Package = Package.mkvextract
                    proc.Arguments = "--ui-language en timestamps_v2 " + proj.SourceFile.Escape + " " & streamOrder & ":" + Path.Combine(proj.TempDir, proj.SourceFile.Base + "_timestamps.txt").Escape
                    proc.AllowedExitCodes = {0, 1, 2}
                    proc.Start()
                End Using
            End If
        End If

        If demuxAttachments Then
            Dim enabledAttachments = attachments.Where(Function(val) val.Enabled)

            If enabledAttachments.Count > 0 Then
                Using proc As New Proc
                    proc.Project = proj
                    proc.Header = "Demux attachments"
                    proc.SkipString = "Progress: "
                    proc.WriteLog(stdout + BR)
                    proc.Encoding = Encoding.UTF8
                    proc.Package = Package.mkvextract
                    proc.Arguments = proj.SourceFile.Escape + " --ui-language en attachments " + enabledAttachments.Select(Function(val) val.ID & ":" + GetAttachmentPath(proj, val.Name).Escape).Join(" ")
                    proc.AllowedExitCodes = {0, 1, 2}
                    proc.Start()
                End Using
            End If
        End If

        If demuxTags Then
            Dim tagsIncluded = Regex.IsMatch(stdout, "^Global tags: \d+", RegexOptions.Multiline Or RegexOptions.IgnoreCase)

            If tagsIncluded Then
                Dim tagsString = ProcessHelp.GetConsoleOutput(Package.mkvextract.Path, proj.SourceFile.Escape + " tags")

                If Not String.IsNullOrWhiteSpace(tagsString) Then
                    Try
                        Dim xmlDoc = New XmlDocument()
                        xmlDoc.LoadXml(tagsString)
                        Dim tagsNode = xmlDoc.SelectSingleNode("/Tags")
                        Dim tagNodes = tagsNode.SelectNodes("Tag[Simple/Name[contains(text(), ""BPS"")]]")

                        For Each node As XmlNode In tagNodes
                            tagsNode.RemoveChild(node)
                        Next

                        xmlDoc.Save(Path.Combine(proj.TempDir, proj.SourceFile.Base + "_tags.xml"))
                    Catch ex As Exception
                    End Try
                End If
            End If
        End If

        Log.Save(proj)
    End Sub

    Public Overrides Property OutputExtensions As String()
        Get
            Return If(p.DemuxVideo OrElse _videoDemuxing, {"avi", "mpg", "h264", "h265", "hevc"}, {})
        End Get
        Set(value As String())
        End Set
    End Property

    Shared Sub Demux(
        sourcefile As String,
        audioStreams As IEnumerable(Of AudioStream),
        subtitles As IEnumerable(Of Subtitle),
        ap As AudioProfile,
        proj As Project,
        onlyEnabled As Boolean,
        videoDemuxing As Boolean,
        overrideExisting As Boolean,
        title As String,
        useStreamName As Boolean)

        If audioStreams Is Nothing Then
            audioStreams = New List(Of AudioStream)
        End If

        If onlyEnabled Then
            audioStreams = audioStreams.Where(Function(arg) arg.Enabled)
        End If

        If audioStreams.Count = 0 AndAlso subtitles.NothingOrEmpty AndAlso Not videoDemuxing Then
            Exit Sub
        End If

        Dim args = "--ui-language en " + sourcefile.Escape + " tracks"
        Dim newCount As Integer
        Dim outPaths As New List(Of String)

        If videoDemuxing Then
            Dim stdout = ProcessHelp.GetConsoleOutput(Package.mkvmerge.Path, "--identify " + sourcefile.Escape)
            Dim id = Regex.Match(stdout, "Track ID (\d+): video").Groups(1).Value.ToInt
            Dim videoStreams = MediaInfo.GetVideoStreams(sourcefile)

            If videoStreams.Count > 0 Then
                Dim outPath = Path.Combine(proj.TempDir, sourcefile.Base + videoStreams(0).ExtFull)

                If outPath <> sourcefile Then
                    args += " " & id & ":" + outPath.Escape
                    outPaths.Add(outPath)

                    If Not outPath.FileExists Then
                        newCount += 1
                    End If
                End If
            End If
        End If

        If subtitles IsNot Nothing AndAlso proj.IsSubtitleDemuxingRequired Then
            For Each subtitle In subtitles
                Dim skip As Boolean = False

                Select Case proj.SubtitleMode
                    Case SubtitleMode.All
                        skip = False
                    Case SubtitleMode.PreferredNoMux
                        Dim prefSubtitles = proj.PreferredSubtitles.ToLowerInvariant.SplitNoEmptyAndWhiteSpace(",", ";", " ")
                        skip = Not prefSubtitles.ContainsAny("all", subtitle.Language.TwoLetterCode.ToLowerInvariant(), subtitle.Language.ThreeLetterCode.ToLowerInvariant(), subtitle.Language.Name.ToLowerInvariant())
                    Case Else
                        skip = Not subtitle.Enabled
                End Select

                If skip Then Continue For

                Dim _default = If(subtitle.Default, "_default", "")
                Dim forced = If(subtitle.Forced, "_forced", "")
                Dim commentary = If(subtitle.Commentary, "_commentary", "")
                Dim hearingimpaired = If(subtitle.Hearingimpaired, "_hearingimpaired", "")
                Dim outPath = Path.Combine(proj.TempDir, subtitle.Filename + _default + forced + commentary + hearingimpaired + subtitle.ExtFull)

                If subtitle.Ext = "mks" Then
                    Dim arguments = "--ui-language en --no-audio --no-video --no-global-tags --no-attachments --no-buttons -o " + outPath.Escape + " " + sourcefile.Escape

                    Using proc As New Proc
                        proc.Project = proj
                        proc.Header = title
                        proc.SkipString = "Progress: "
                        proc.Encoding = Encoding.UTF8
                        proc.Package = Package.mkvmerge
                        proc.Arguments = arguments
                        proc.AllowedExitCodes = {0, 1, 2}
                        proc.Start()
                    End Using
                Else
                    args += " " & subtitle.StreamOrder & ":" + outPath.Escape
                    outPaths.Add(outPath)

                    If Not outPath.FileExists Then
                        newCount += 1
                    End If
                End If
            Next
        End If

        Dim audioOutPaths As New Dictionary(Of String, AudioStream)

        For Each stream In audioStreams
            Dim ext = stream.Ext

            If ext = "m4a" Then
                ext = "aac"
            End If

            Dim base = If(useStreamName, Audio.GetBaseNameForStream(sourcefile, stream), sourcefile.Base)
            Dim outPath = Path.Combine(proj.TempDir, base + "." + ext)
            audioOutPaths.Add(outPath, stream)
            outPaths.Add(outPath)
            args += " " & stream.StreamOrder & ":" + outPath.Escape

            If Not outPath.FileExists Then
                newCount += 1
            End If
        Next

        If Not (newCount > 0 OrElse overrideExisting) Then
            Exit Sub
        End If

        Using proc As New Proc
            proc.Project = proj
            proc.Header = title
            proc.SkipString = "Progress: "
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.mkvextract
            proc.Arguments = args
            proc.AllowedExitCodes = {0, 1, 2}
            proc.OutputFiles = outPaths
            proc.Start()
        End Using

        outPaths.Clear()

        For Each outPath In audioOutPaths.Keys
            If File.Exists(outPath) Then
                If ap IsNot Nothing Then
                    ap.File = outPath
                End If

                proj.Log.WriteLine(MediaInfo.GetSummary(outPath) + BR)
            Else
                proj.Log.Write("Error", "no output found")
                ffmpegDemuxer.DemuxAudio(sourcefile, audioOutPaths(outPath), ap, proj, overrideExisting)
            End If
        Next
    End Sub

    Shared Function GetAttachmentPath(proj As Project, name As String) As String
        Dim prefix = If(name.Base.EqualsAny("cover", "small_cover", "cover_land", "small_cover_land"), "", If(proj.SourceFile.StartsWith(proj.TempDir), proj.SourceFile.Base + "_", "") + "attachment_")
        Dim ret = Path.Combine(proj.TempDir, prefix + name.Base + name.ExtFull)
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
