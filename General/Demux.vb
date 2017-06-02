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
    Overridable Property SourceFilter As String = ""

    Overridable ReadOnly Property HasConfigDialog() As Boolean
        Get
            Return False
        End Get
    End Property

    Overridable Function ShowConfigDialog() As DialogResult
    End Function

    Overrides Function ToString() As String
        Return Name + " (" + InputExtensions.Join(", ") + " -> " + OutputExtensions.Join(", ") + ")"
    End Function

    Overridable Function GetHelp() As String
        For Each i In Package.Items.Values
            If Name = i.Name Then Return i.Description
        Next
    End Function

    Public Function ShowConfigDialogInputExtensions() As DialogResult
        Using form As New SimpleSettingsForm(Name)
            form.ScaleClientSize(23, 10)
            Dim ui = form.SimpleUI
            Dim page = ui.CreateFlowPage("main page")

            Dim tb = ui.AddTextBlock(page)
            tb.Label.Text = "Supported Input File Types:"
            tb.Edit.Text = InputExtensions.Join(" ")
            tb.Edit.SaveAction = Sub(value) InputExtensions = value.ToLower.SplitNoEmptyAndWhiteSpace(",", ";", " ")

            Dim ret = form.ShowDialog()
            If ret = DialogResult.OK Then ui.Save()

            Return ret
        End Using
    End Function

    Shared Function GetDefaults() As List(Of Demuxer)
        Dim ret As New List(Of Demuxer)

        Dim prx As New CommandLineDemuxer
        prx.Name = "ProjectX"
        prx.InputExtensions = {"vob", "mpg", "ts"}
        prx.OutputExtensions = {"m2v"}
        prx.InputFormats = {"mpeg2"}
        prx.Command = "%app:Java%"
        prx.Arguments = "-jar ""%app:ProjectX%"" %source_files% -out ""%working_dir%"""
        ret.Add(prx)

        Dim dsmux As New CommandLineDemuxer
        dsmux.Name = "dsmux"
        dsmux.InputExtensions = {"ts"}
        dsmux.OutputExtensions = {"mkv"}
        dsmux.Command = "%app:dsmux%"
        dsmux.Arguments = """%temp_file%.mkv"" ""%source_file%"""
        dsmux.Active = False
        ret.Add(dsmux)

        ret.Add(New mkvDemuxer)
        ret.Add(New MP4BoxDemuxer)
        ret.Add(New eac3toDemuxer)

        Dim dgnvNoDemux As New CommandLineDemuxer
        dgnvNoDemux.Name = "DGIndexNV"
        dgnvNoDemux.InputExtensions = {"264", "h264", "avc", "mkv", "mp4"}
        dgnvNoDemux.OutputExtensions = {"dgi"}
        dgnvNoDemux.InputFormats = {"avc", "vc1", "mpeg2"}
        dgnvNoDemux.Command = "%app:DGIndexNV%"
        dgnvNoDemux.Arguments = "-i %source_files_comma% -o ""%source_temp_file%.dgi"" -h"
        dgnvNoDemux.SourceFilter = "DGSource"
        dgnvNoDemux.Active = False
        ret.Add(dgnvNoDemux)

        Dim dgnvDemux As New CommandLineDemuxer
        dgnvDemux.Name = "DGIndexNV"
        dgnvDemux.InputExtensions = {"mpg", "vob", "ts", "m2ts", "mts", "m2t"}
        dgnvDemux.OutputExtensions = {"dgi"}
        dgnvDemux.InputFormats = {"avc", "vc1", "mpeg2"}
        dgnvDemux.Command = "%app:DGIndexNV%"
        dgnvDemux.Arguments = "-i %source_files_comma% -o ""%source_temp_file%.dgi"" -a -h"
        dgnvDemux.SourceFilter = "DGSource"
        dgnvDemux.Active = False
        ret.Add(dgnvDemux)

        Dim dgimNoDemux As New CommandLineDemuxer
        dgimNoDemux.Name = "DGIndexIM"
        dgimNoDemux.InputExtensions = {"264", "h264", "avc", "mkv", "mp4"}
        dgimNoDemux.OutputExtensions = {"dgim"}
        dgimNoDemux.InputFormats = {"avc", "vc1", "mpeg2"}
        dgimNoDemux.Command = "%app:DGIndexIM%"
        dgimNoDemux.Arguments = "-i %source_files_comma% -o ""%source_temp_file%.dgim"" -h"
        dgimNoDemux.SourceFilter = "DGSourceIM"
        dgimNoDemux.Active = False
        ret.Add(dgimNoDemux)

        Dim dgimDemux As New CommandLineDemuxer
        dgimDemux.Name = "DGIndexIM"
        dgimDemux.InputExtensions = {"mpg", "vob", "ts", "m2ts", "mts", "m2t"}
        dgimDemux.OutputExtensions = {"dgim"}
        dgimDemux.InputFormats = {"avc", "vc1", "mpeg2"}
        dgimDemux.Command = "%app:DGIndexIM%"
        dgimDemux.Arguments = "-i %source_files_comma% -o ""%source_temp_file%.dgim"" -a -h"
        dgimDemux.SourceFilter = "DGSourceIM"
        dgimDemux.Active = False
        ret.Add(dgimDemux)

        ret.Add(New ffmpegDemuxer)

        Return ret
    End Function
End Class

<Serializable()>
Class CommandLineDemuxer
    Inherits Demuxer

    Property Command As String = ""
    Property Arguments As String = ""

    Overrides ReadOnly Property HasConfigDialog() As Boolean
        Get
            Return True
        End Get
    End Property

    Overrides Function ShowConfigDialog() As DialogResult
        Using f As New DemuxForm(Me)
            Return f.ShowDialog
        End Using
    End Function

    Overrides Sub Run(proj As Project)
        Using proc As New Proc
            If Command?.Contains("DGIndexNV") Then
                If Not Package.DGIndexNV.VerifyOK(True) Then Throw New AbortException
                proc.SkipPatterns = {"^\d+$"}
            ElseIf Command?.Contains("DGIndexIM") Then
                If Not Package.DGIndexIM.VerifyOK(True) Then Throw New AbortException
                proc.SkipPatterns = {"^\d+$"}
            ElseIf Command?.Contains("dsmux") Then
                If Not Package.Haali.VerifyOK(True) Then Throw New AbortException
                proc.SkipStrings = {"Muxing..."}
            ElseIf Command?.Contains("Java") Then
                If Not Package.Java.VerifyOK(True) Then Throw New AbortException
                proc.SkipPatterns = {"^\d+ %$"}
            End If

            proc.Init(Name)
            proc.File = Macro.Expand(Command)
            proc.Arguments = Macro.Expand(Arguments)
            proc.Start()

            If Command?.Contains("DGIndex") Then
                FileHelp.Move(Filepath.GetDirAndBase(p.SourceFile) + ".log", p.TempDir + p.SourceFile.Base + "_dg.log")
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
        Name = "eac3to"
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
                    proc.TrimChars = {"-"c, " "c}
                    proc.RemoveChars = {CChar(VB6.vbBack)}
                    proc.Init("Demux M2TS using eac3to " + Package.eac3to.Version, "analyze: ", "process: ")
                    proc.File = Package.eac3to.Path
                    proc.Arguments = form.GetArgs(proj.SourceFile.Quotes, proj.SourceFile.Base)

                    Try
                        proc.Start()
                    Catch ex As Exception
                        ProcessForm.CloseProcessForm()
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

    Public Overrides Function ShowConfigDialog() As DialogResult
        Return ShowConfigDialogInputExtensions()
    End Function
End Class

<Serializable>
Public Class ffmpegDemuxer
    Inherits Demuxer

    Sub New()
        Name = "ffmpeg"
        InputExtensions = {"avi", "ts", "flv"}
    End Sub

    Public Overrides Sub Run(proj As Project)
        Dim audioStreams As List(Of AudioStream)
        Dim subtitles As List(Of Subtitle)

        Dim demuxAudio = Not (TypeOf proj.Audio0 Is NullAudioProfile AndAlso
            TypeOf proj.Audio1 Is NullAudioProfile) AndAlso
            MediaInfo.GetAudioCount(proj.SourceFile) > 0

        Dim demuxSubtitles = MediaInfo.GetSubtitleCount(proj.SourceFile) > 0

        If Not proj.NoDialogs AndAlso Not proj.BatchMode AndAlso
            ((demuxAudio AndAlso proj.DemuxAudio = DemuxMode.Dialog) OrElse
            (demuxSubtitles AndAlso proj.DemuxSubtitles = DemuxMode.Dialog)) Then
            ProcessForm.CloseProcessForm()

            Using form As New StreamDemuxForm(proj.SourceFile, Nothing)
                If form.ShowDialog() = DialogResult.OK Then
                    audioStreams = form.AudioStreams
                    subtitles = form.Subtitles
                Else
                    Throw New AbortException
                End If
            End Using
        End If

        If demuxAudio AndAlso proj.DemuxAudio <> DemuxMode.None Then
            If audioStreams Is Nothing Then audioStreams = MediaInfo.GetAudioStreams(proj.SourceFile)

            For Each i In audioStreams
                If i.Enabled Then ffmpegDemuxer.DemuxAudio(proj.SourceFile, i, Nothing, proj)
            Next
        End If

        'If demuxSubtitles AndAlso proj.DemuxSubtitles <> DemuxMode.None Then
        '    If subtitles Is Nothing Then subtitles = MediaInfo.GetSubtitles(proj.SourceFile)
        '    Me.DemuxSubtitles(subtitles, proj)
        'End If

        Log.Save(proj)
    End Sub

    Public Overrides Function ShowConfigDialog() As DialogResult
        Return ShowConfigDialogInputExtensions()
    End Function

    Public Overrides ReadOnly Property HasConfigDialog As Boolean
        Get
            Return True
        End Get
    End Property

    Shared Sub DemuxAudio(sourcefile As String, stream As AudioStream, ap As AudioProfile, proj As Project)
        Dim outPath = proj.TempDir + Audio.GetBaseNameForStream(sourcefile, stream) + stream.Extension
        If outPath.Length > 259 Then outPath = proj.TempDir + Audio.GetBaseNameForStream(sourcefile, stream, True) + stream.Extension
        Dim streamIndex = stream.StreamOrder
        Dim args = "-i """ + sourcefile + """"
        If MediaInfo.GetAudioCount(sourcefile) > 1 Then args += " -map 0:a:" & stream.Index
        args += " -vn -sn -y -hide_banner"
        If outPath.Ext = "wav" Then args += " -c:a pcm_s16le" Else args += " -c:a copy"
        args += " " + outPath.Quotes

        Using proc As New Proc
            proc.Project = proj
            proc.Init("Demux audio using ffmpeg " + Package.ffmpeg.Version, {"Media Export: |", "File Export: |", "ISO File Writing: |"})
            proc.Encoding = Encoding.UTF8
            proc.File = Package.ffmpeg.Path
            proc.Arguments = args
            proc.Start()
        End Using

        If File.Exists(outPath) Then
            If Not ap Is Nothing Then ap.File = outPath
            Log.WriteLine(MediaInfo.GetSummary(outPath), proj)
        Else
            Log.Write("Error", "no output found", proj)
        End If
    End Sub

    Sub DemuxSubtitles(subtitles As List(Of Subtitle), proj As Project)
        If subtitles.Where(Function(subtitle) subtitle.Enabled).Count = 0 Then Exit Sub

        For Each subtitle In subtitles
            If Not subtitle.Enabled Then Continue For

            Dim args = "-i " + proj.SourceFile.Quotes
            Dim outpath = proj.TempDir + proj.SourceFile.Base + " " + subtitle.Filename + subtitle.ExtFull

            If outpath.Length > 259 Then
                outpath = proj.TempDir + proj.SourceFile.Base.Shorten(10) + " " + subtitle.Filename.Shorten(10) + subtitle.ExtFull
            End If

            If MediaInfo.GetSubtitleCount(proj.SourceFile) > 1 Then args += " -map 0:s:" & subtitle.Index
            args += " -c:s copy -vn -an -y -hide_banner " + outpath.Quotes

            Using proc As New Proc
                proc.Project = proj
                proc.Init("Demux subtitles using ffmpeg " + Package.ffmpeg.Version, {"Media Export: |", "File Export: |", "ISO File Writing: |"})
                proc.Encoding = Encoding.UTF8
                proc.File = Package.ffmpeg.Path
                proc.Arguments = args
                proc.Start()
            End Using
        Next
    End Sub
End Class

<Serializable()>
Class MP4BoxDemuxer
    Inherits Demuxer

    Sub New()
        Name = "MP4Box"
        InputExtensions = {"mp4", "mov"}
    End Sub

    Overrides Sub Run(proj As Project)
        Dim audioStreams As List(Of AudioStream)
        Dim subtitles As List(Of Subtitle)

        Dim demuxAudio = Not (TypeOf proj.Audio0 Is NullAudioProfile AndAlso
            TypeOf proj.Audio1 Is NullAudioProfile) AndAlso
            MediaInfo.GetAudioCount(proj.SourceFile) > 0

        Dim demuxSubtitles = MediaInfo.GetSubtitleCount(proj.SourceFile) > 0
        Dim attachments = GetAttachments(proj.SourceFile)

        If Not proj.NoDialogs AndAlso Not proj.BatchMode AndAlso
            ((demuxAudio AndAlso proj.DemuxAudio = DemuxMode.Dialog) OrElse
            (demuxSubtitles AndAlso proj.DemuxSubtitles = DemuxMode.Dialog)) Then
            ProcessForm.CloseProcessForm()

            Using f As New StreamDemuxForm(proj.SourceFile, attachments)
                If f.ShowDialog() = DialogResult.OK Then
                    audioStreams = f.AudioStreams
                    subtitles = f.Subtitles
                Else
                    Throw New AbortException
                End If
            End Using
        End If

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

                args += i.ID & " -out " + outpath.Quotes + " " + proj.SourceFile.Quotes

                Using proc As New Proc
                    proc.Project = proj
                    proc.Init("Demux subtitle using MP4Box " + Package.MP4Box.Version, {"Media Export: |", "File Export: |", "ISO File Writing: |", "VobSub Export: |", "SRT Extract: |"})
                    proc.File = Package.MP4Box.Path
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
                proc.Init("Extract cover using MP4Box " + Package.MP4Box.Version)
                proc.File = Package.MP4Box.Path
                proc.Arguments = "-dump-cover " + proj.SourceFile.Quotes + " -out " + (proj.TempDir + "cover.jpg").Quotes
                proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
                proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
                proc.Start()
            End Using
        End If

        Log.Save(proj)
    End Sub

    Function GetAttachments(sourceFilePath As String) As List(Of Attachment)
        Dim cover = MediaInfo.GetGeneral(sourceFilePath, "Cover")
        If cover <> "" Then Return New List(Of Attachment) From {New Attachment With {.Name = "Cover"}}
    End Function

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
        args += " " & stream.ID & " -out " + outPath.Quotes + " " + sourcefile.Quotes

        Using proc As New Proc
            proc.Project = proj
            proc.Init("Demux audio using MP4Box " + Package.MP4Box.Version, {"Media Export: |", "File Export: |", "ISO File Writing: |"})
            proc.File = Package.MP4Box.Path
            proc.Arguments = args
            proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
            proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
            proc.Start()
        End Using

        If File.Exists(outPath) Then
            If MediaInfo.GetAudio(outPath, "Format") = "" Then
                Log.Write("Error", "No format detected by MediaInfo.", proj)
                ffmpegDemuxer.DemuxAudio(sourcefile, stream, ap, proj)
            Else
                If Not ap Is Nothing Then ap.File = outPath
                Log.WriteLine(MediaInfo.GetSummary(outPath), proj)
            End If
        Else
            Log.Write("Error", "no output found", proj)
            ffmpegDemuxer.DemuxAudio(sourcefile, stream, ap, proj)
        End If
    End Sub
End Class

<Serializable()>
Class mkvDemuxer
    Inherits Demuxer

    Sub New()
        Name = "mkvextract"
        InputExtensions = {"mkv", "webm"}
    End Sub

    Overrides Sub Run(proj As Project)
        Dim audioStreams As List(Of AudioStream)
        Dim subtitles As List(Of Subtitle)
        Dim stdout = ProcessHelp.GetStdOut(Package.mkvmerge.Path, "--identify-verbose --ui-language en " + proj.SourceFile.Quotes)

        If stdout.Contains("codec_private_data:") Then stdout = Regex.Replace(stdout, "codec_private_data:\w+", "")

        Dim demuxAudio = (Not (TypeOf proj.Audio0 Is NullAudioProfile AndAlso
            TypeOf proj.Audio1 Is NullAudioProfile)) AndAlso
            MediaInfo.GetAudioCount(proj.SourceFile) > 0

        Dim demuxSubtitles = MediaInfo.GetSubtitleCount(proj.SourceFile) > 0
        Dim attachments = GetAttachments(stdout)

        If Not proj.NoDialogs AndAlso Not proj.BatchMode AndAlso
            ((demuxAudio AndAlso proj.DemuxAudio = DemuxMode.Dialog) OrElse
            (demuxSubtitles AndAlso proj.DemuxSubtitles = DemuxMode.Dialog)) Then
            ProcessForm.CloseProcessForm()

            Using form As New StreamDemuxForm(proj.SourceFile, attachments)
                If form.ShowDialog() <> DialogResult.OK Then Throw New AbortException
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

        Demux(proj.SourceFile, audioStreams, subtitles, Nothing, proj, True)

        If stdout.Contains("Chapters: ") Then
            Using proc As New Proc
                proc.Project = proj
                proc.Init("Demux chapters using mkvextract " + Package.mkvmerge.Version, "Progress: ")
                proc.WriteLine(stdout + BR)
                proc.Encoding = Encoding.UTF8
                proc.File = Package.mkvextract.Path
                proc.Arguments = "chapters " + proj.SourceFile.Quotes + " --redirect-output " +
                        (proj.TempDir + proj.SourceFile.Base + "_chapters.xml").Quotes
                proc.Start()
            End Using
        End If

        Dim enabledAttachments = attachments.Where(Function(val) val.Enabled)

        If enabledAttachments.Count > 0 Then
            Using proc As New Proc
                proc.Project = proj
                proc.Init("Demux attachments using mkvextract " + Package.mkvmerge.Version, "Progress: ")
                proc.WriteLine(stdout + BR)
                proc.Encoding = Encoding.UTF8
                proc.File = Package.mkvextract.Path
                proc.Arguments = "attachments " + proj.SourceFile.Quotes + " " +
                    enabledAttachments.Select(Function(val) val.ID & ":" + GetAttachmentPath(
                    proj.TempDir, val.Name).Quotes).Join(" ")
                proc.Start()
            End Using
        End If

        If MediaInfo.GetVideo(proj.SourceFile, "FrameRate_Mode") = "VFR" Then
            Dim streamOrder = MediaInfo.GetVideo(proj.SourceFile, "StreamOrder").ToInt

            Using proc As New Proc
                proc.Project = proj
                proc.Init("Demux timecodes using mkvextract " + Package.mkvextract.Version, "Progress: ")
                proc.Encoding = Encoding.UTF8
                proc.File = Package.mkvextract.Path
                proc.Arguments = "timecodes_v2 " + proj.SourceFile.Quotes + " " & streamOrder & ":" + (proj.TempDir + proj.SourceFile.Base + "_timecodes.txt").Quotes
                proc.Start()
            End Using
        End If

        Log.Save(proj)
    End Sub

    Shared Sub Demux(sourcefile As String,
                     audioStreams As IEnumerable(Of AudioStream),
                     subtitles As IEnumerable(Of Subtitle),
                     ap As AudioProfile,
                     proj As Project,
                     Optional onlyEnabled As Boolean = False)

        If audioStreams Is Nothing Then audioStreams = New List(Of AudioStream)
        If subtitles Is Nothing Then subtitles = New List(Of Subtitle)

        If onlyEnabled Then audioStreams = audioStreams.Where(Function(arg) arg.Enabled)
        subtitles = subtitles.Where(Function(subtitle) subtitle.Enabled)

        If audioStreams.Count = 0 AndAlso subtitles.Count = 0 Then Exit Sub

        Dim args = "tracks " + sourcefile.Quotes

        For Each subtitle In subtitles
            If Not subtitle.Enabled Then Continue For
            Dim forced = If(subtitle.Forced, "_forced", "")
            Dim outpath = proj.TempDir + sourcefile.Base + " " + subtitle.Filename + forced + subtitle.ExtFull
            If outpath.Length > 259 Then outpath = proj.TempDir + sourcefile.Base.Shorten(10) + " " + subtitle.Filename.Shorten(10) + forced + subtitle.ExtFull
            args += " " & subtitle.StreamOrder & ":" + outpath.Quotes
        Next

        Dim outPaths As New Dictionary(Of String, AudioStream)

        For Each stream In audioStreams
            Dim ext = stream.Extension
            If ext = ".m4a" Then ext = ".aac"
            Dim outPath = proj.TempDir + Audio.GetBaseNameForStream(sourcefile, stream) + ext
            If outPath.Length > 259 Then outPath = proj.TempDir + Audio.GetBaseNameForStream(sourcefile, stream, True) + ext
            outPaths.Add(outPath, stream)
            args += " " & stream.StreamOrder & ":" + outPath.Quotes
        Next

        Using proc As New Proc
            proc.Project = proj
            proc.Init("Demux mkv using mkvextract " + Package.mkvextract.Version, "Progress: ")
            proc.Encoding = Encoding.UTF8
            proc.File = Package.mkvextract.Path
            proc.Arguments = args + " --ui-language en"
            proc.AllowedExitCodes = {0, 1, 2}
            proc.Start()
        End Using

        For Each outPath In outPaths.Keys
            If File.Exists(outPath) Then
                If Not ap Is Nothing Then ap.File = outPath
                Log.WriteLine(MediaInfo.GetSummary(outPath) + BR, proj)

                If outPath.Ext = "aac" Then
                    Using proc As New Proc
                        proc.Project = proj
                        proc.Init("Mux AAC to M4A using MP4Box " + Package.MP4Box.Version, "|")
                        proc.File = Package.MP4Box.Path
                        Dim sbr = If(outPath.Contains("SBR"), ":sbr", "")
                        Dim m4aPath = outPath.ChangeExt("m4a")
                        proc.Arguments = "-add """ + outPath + sbr + ":name= "" -new " + m4aPath.Quotes
                        proc.Process.StartInfo.EnvironmentVariables("TEMP") = proj.TempDir
                        proc.Process.StartInfo.EnvironmentVariables("TMP") = proj.TempDir
                        proc.Start()

                        If File.Exists(m4aPath) Then
                            If Not ap Is Nothing Then ap.File = m4aPath
                            FileHelp.Delete(outPath)
                            Log.WriteLine(BR + MediaInfo.GetSummary(m4aPath), proj)
                        Else
                            Throw New ErrorAbortException("Error mux AAC to M4A", outPath)
                        End If
                    End Using
                End If
            Else
                Log.Write("Error", "no output found", proj)
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
End Class