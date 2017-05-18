Imports System.Text.RegularExpressions
Imports VB6 = Microsoft.VisualBasic
Imports System.Text

<Serializable()>
Public MustInherit Class Demuxer
    MustOverride Sub Run()

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
        Using f As New SimpleSettingsForm(Name)
            f.Height = CInt(f.Height * 0.5)
            Dim ui = f.SimpleUI
            Dim page = ui.CreateFlowPage("main page")

            Dim tb = ui.AddTextBlock(page)
            tb.Label.Text = "Supported Input File Types:"
            tb.Edit.Text = InputExtensions.Join(" ")
            tb.Edit.SaveAction = Sub(value) InputExtensions = value.ToLower.SplitNoEmptyAndWhiteSpace(",", ";", " ")

            Dim ret = f.ShowDialog()
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

    Overrides Sub Run()
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
Class MP4BoxDemuxer
    Inherits Demuxer

    Sub New()
        Name = "MP4Box"
        InputExtensions = {"mp4", "mov"}
    End Sub

    Overrides Sub Run()
        Dim audioStreams As List(Of AudioStream)
        Dim subtitles As List(Of Subtitle)

        Dim demuxAudio = Not (TypeOf p.Audio0 Is NullAudioProfile AndAlso
            TypeOf p.Audio1 Is NullAudioProfile) AndAlso
            MediaInfo.GetAudioCount(p.SourceFile) > 0

        Dim demuxSubtitles = MediaInfo.GetSubtitleCount(p.SourceFile) > 0

        Dim attachments = GetAttachments()

        If Not p.NoDialogs AndAlso Not p.BatchMode AndAlso
            ((demuxAudio AndAlso p.DemuxAudio = DemuxMode.Dialog) OrElse
            (demuxSubtitles AndAlso p.DemuxSubtitles = DemuxMode.Dialog)) Then
            ProcessForm.CloseProcessForm()

            Using f As New StreamDemuxForm(p.SourceFile, attachments)
                If f.ShowDialog() = DialogResult.OK Then
                    audioStreams = f.AudioStreams
                    subtitles = f.Subtitles
                Else
                    Throw New AbortException
                End If
            End Using
        End If

        If demuxAudio AndAlso p.DemuxAudio <> DemuxMode.None Then
            If audioStreams Is Nothing Then audioStreams = MediaInfo.GetAudioStreams(p.SourceFile)

            For Each i In audioStreams
                If i.Enabled Then Audio.DemuxMP4(p.SourceFile, i, Nothing)
            Next
        End If

        If demuxSubtitles AndAlso p.DemuxSubtitles <> DemuxMode.None Then
            If subtitles Is Nothing Then subtitles = MediaInfo.GetSubtitles(p.SourceFile)

            For Each i In subtitles
                If Not i.Enabled Then Continue For

                Dim outpath = p.TempDir + p.SourceFile.Base + " " + i.Filename + i.ExtFull
                If outpath.Length > 259 Then outpath = p.TempDir + p.SourceFile.Base.Shorten(10) + " " + i.Filename.Shorten(20) + i.ExtFull

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

                args += i.ID & " -out " + outpath.Quotes + " " + p.SourceFile.Quotes

                Using proc As New Proc
                    proc.Init("Demux subtitle using MP4Box " + Package.MP4Box.Version, {"Media Export: |", "File Export: |", "ISO File Writing: |", "VobSub Export: |", "SRT Extract: |"})
                    proc.File = Package.MP4Box.Path
                    proc.Arguments = args
                    proc.Process.StartInfo.EnvironmentVariables("TEMP") = p.TempDir
                    proc.Process.StartInfo.EnvironmentVariables("TMP") = p.TempDir
                    proc.Start()
                End Using
            Next
        End If

        If Not attachments.NothingOrEmpty AndAlso attachments(0).Enabled Then
            Using proc As New Proc
                proc.Init("Extract cover using MP4Box " + Package.MP4Box.Version)
                proc.File = Package.MP4Box.Path
                proc.Arguments = "-dump-cover " + p.SourceFile.Quotes + " -out " + p.TempDir + "cover.jpg"
                proc.Process.StartInfo.EnvironmentVariables("TEMP") = p.TempDir
                proc.Process.StartInfo.EnvironmentVariables("TMP") = p.TempDir
                proc.Start()
            End Using
        End If
    End Sub

    Function GetAttachments() As List(Of Attachment)
        Dim cover = MediaInfo.GetGeneral(p.SourceFile, "Cover")
        If cover <> "" Then Return New List(Of Attachment) From {New Attachment With {.Name = "Cover"}}
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

    Overrides Sub Run()
        If p.NoDialogs OrElse p.BatchMode Then Exit Sub

        Using f As New eac3toForm
            f.M2TSFile = p.SourceFile
            f.teTempDir.Text = p.TempDir

            If f.ShowDialog() = DialogResult.OK Then
                Using proc As New Proc
                    proc.TrimChars = {"-"c, " "c}
                    proc.RemoveChars = {CChar(VB6.vbBack)}
                    proc.Init("Demux M2TS using eac3to " + Package.eac3to.Version, "analyze: ", "process: ")
                    proc.File = Package.eac3to.Path
                    proc.Arguments = f.GetArgs(p.SourceFile.Quotes, p.SourceFile.Base)

                    Try
                        proc.Start()
                    Catch ex As Exception
                        ProcessForm.CloseProcessForm()
                        g.ShowException(ex)
                        Throw New AbortException
                    End Try

                    If Not f.cbVideoOutput.Text = "Nothing" Then
                        p.SourceFile = f.OutputFolder + Filepath.GetBase(p.SourceFile) + "." + f.cbVideoOutput.Text.ToLower
                        p.SourceFiles.Clear()
                        p.SourceFiles.Add(p.SourceFile)
                        p.TempDir = f.OutputFolder
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

<Serializable()>
Class mkvDemuxer
    Inherits Demuxer

    Sub New()
        Name = "mkvextract"
        InputExtensions = {"mkv", "webm"}
    End Sub

    Sub DemuxMKVSubtitles(subtitles As List(Of Subtitle))
        If subtitles.Where(Function(subtitle) subtitle.Enabled).Count = 0 Then Exit Sub
        Dim arguments = "tracks " + p.SourceFile.Quotes

        For Each i In subtitles
            If Not i.Enabled Then Continue For

            Dim outpath = p.TempDir + p.SourceFile.Base + " " + i.Filename + i.ExtFull

            If outpath.Length > 259 Then
                outpath = p.TempDir + p.SourceFile.Base.Shorten(10) + " " + i.Filename.Shorten(10) + i.ExtFull
            End If

            arguments += " " & i.StreamOrder & ":""" + outpath + """"
        Next

        arguments += " --ui-language en"

        Using proc As New Proc
            proc.Init("Demux subtitles using mkvextract " + Package.mkvextract.Version, "Progress: ")
            proc.Encoding = Encoding.UTF8
            proc.File = Package.mkvextract.Path
            proc.Arguments = arguments
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using
    End Sub

    Overrides Sub Run()
        Dim audioStreams As List(Of AudioStream)
        Dim subtitles As List(Of Subtitle)
        Dim stdout = ProcessHelp.GetStdOut(Package.mkvmerge.Path, "--identify-verbose --ui-language en " + p.SourceFile.Quotes)

        If stdout.Contains("codec_private_data:") Then stdout = Regex.Replace(stdout, "codec_private_data:\w+", "")

        Dim demuxAudio = Not (TypeOf p.Audio0 Is NullAudioProfile AndAlso
            TypeOf p.Audio1 Is NullAudioProfile) AndAlso
            MediaInfo.GetAudioCount(p.SourceFile) > 0

        Dim demuxSubtitles = MediaInfo.GetSubtitleCount(p.SourceFile) > 0

        Dim attachments = GetAttachments(stdout)

        If Not p.NoDialogs AndAlso Not p.BatchMode AndAlso
            ((demuxAudio AndAlso p.DemuxAudio = DemuxMode.Dialog) OrElse
            (demuxSubtitles AndAlso p.DemuxSubtitles = DemuxMode.Dialog)) Then
            ProcessForm.CloseProcessForm()

            Using f As New StreamDemuxForm(p.SourceFile, attachments)
                If f.ShowDialog() <> DialogResult.OK Then Throw New AbortException
                audioStreams = f.AudioStreams
                subtitles = f.Subtitles
            End Using
        End If

        If demuxAudio AndAlso p.DemuxAudio <> DemuxMode.None Then
            If audioStreams Is Nothing Then audioStreams = MediaInfo.GetAudioStreams(p.SourceFile)
            Audio.DemuxMKV(p.SourceFile, audioStreams, Nothing, True)
        End If

        If demuxSubtitles AndAlso p.DemuxSubtitles <> DemuxMode.None Then
            If subtitles Is Nothing Then subtitles = MediaInfo.GetSubtitles(p.SourceFile)
            DemuxMKVSubtitles(subtitles)
        End If

        If stdout.Contains("Chapters: ") Then
            Using proc As New Proc
                proc.Init("Demux chapters using mkvextract " + Package.mkvmerge.Version, "Progress: ")
                proc.WriteLine(stdout + BR)
                proc.Encoding = Encoding.UTF8
                proc.File = Package.mkvextract.Path
                proc.Arguments = "chapters " + p.SourceFile.Quotes + " --redirect-output " +
                        (p.TempDir + p.SourceFile.Base + "_chapters.xml").Quotes
                proc.Start()
            End Using
        End If

        Dim enabledAttachments = attachments.Where(Function(val) val.Enabled)

        If enabledAttachments.Count > 0 Then
            Using proc As New Proc
                proc.Init("Demux attachments using mkvextract " + Package.mkvmerge.Version, "Progress: ")
                proc.WriteLine(stdout + BR)
                proc.Encoding = Encoding.UTF8
                proc.File = Package.mkvextract.Path
                proc.Arguments = "attachments " + p.SourceFile.Quotes + " " +
                    enabledAttachments.Select(Function(val) val.ID & ":" + GetAttachmentPath(
                    p.TempDir, val.Name).Quotes).Join(" ")
                proc.Start()
            End Using
        End If

        If MediaInfo.GetVideo(p.SourceFile, "FrameRate_Mode") = "VFR" Then
            Dim streamOrder = MediaInfo.GetVideo(p.SourceFile, "StreamOrder").ToInt

            Using proc As New Proc
                proc.Init("Demux timecodes using mkvextract " + Package.mkvextract.Version, "Progress: ")
                proc.Encoding = Encoding.UTF8
                proc.File = Package.mkvextract.Path
                proc.Arguments = "timecodes_v2 " + p.SourceFile.Quotes + " " & streamOrder & ":" + (p.TempDir + p.SourceFile.Base + "_timecodes.txt").Quotes
                proc.Start()
            End Using
        End If
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

<Serializable>
Public Class ffmpegDemuxer
    Inherits Demuxer

    Sub New()
        Name = "ffmpeg"
        InputExtensions = {"avi", "ts", "flv"}
    End Sub

    Public Overrides Sub Run()
        Dim audioStreams As List(Of AudioStream)
        Dim subtitles As List(Of Subtitle)

        Dim demuxAudio = Not (TypeOf p.Audio0 Is NullAudioProfile AndAlso
            TypeOf p.Audio1 Is NullAudioProfile) AndAlso
            MediaInfo.GetAudioCount(p.SourceFile) > 0

        Dim demuxSubtitles = MediaInfo.GetSubtitleCount(p.SourceFile) > 0

        If Not p.NoDialogs AndAlso Not p.BatchMode AndAlso
            ((demuxAudio AndAlso p.DemuxAudio = DemuxMode.Dialog) OrElse
            (demuxSubtitles AndAlso p.DemuxSubtitles = DemuxMode.Dialog)) Then
            ProcessForm.CloseProcessForm()

            Using f As New StreamDemuxForm(p.SourceFile, Nothing)
                If f.ShowDialog() = DialogResult.OK Then
                    audioStreams = f.AudioStreams
                    subtitles = f.Subtitles
                Else
                    Throw New AbortException
                End If
            End Using
        End If

        If demuxAudio AndAlso p.DemuxAudio <> DemuxMode.None Then
            If audioStreams Is Nothing Then audioStreams = MediaInfo.GetAudioStreams(p.SourceFile)

            For Each i In audioStreams
                If i.Enabled Then Audio.Demuxffmpeg(p.SourceFile, i, Nothing)
            Next
        End If
    End Sub

    Sub DemuxSubtitles(subtitles As List(Of Subtitle))
        If subtitles.Where(Function(subtitle) subtitle.Enabled).Count = 0 Then Exit Sub
        Dim arguments = "- i " + p.SourceFile.Quotes

        For Each i In subtitles
            If Not i.Enabled Then Continue For
            Dim outpath = p.TempDir + p.SourceFile.Base + " " + i.Filename + i.ExtFull

            If outpath.Length > 259 Then
                outpath = p.TempDir + p.SourceFile.Base.Shorten(10) + " " + i.Filename.Shorten(10) + i.ExtFull
            End If

            arguments += " " & i.StreamOrder & ":""" + outpath + """"
        Next

        arguments += " --ui-language en"

        Using proc As New Proc
            proc.Init("Demux subtitles using mkvextract " + Package.mkvextract.Version, "Progress: ")
            proc.Encoding = Encoding.UTF8
            proc.File = Package.mkvextract.Path
            proc.Arguments = arguments
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using
    End Sub

    Public Overrides Function ShowConfigDialog() As DialogResult
        Return ShowConfigDialogInputExtensions()
    End Function

    Public Overrides ReadOnly Property HasConfigDialog As Boolean
        Get
            Return True
        End Get
    End Property
End Class