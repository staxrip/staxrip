Imports StaxRip.UI

Imports System.Text.RegularExpressions
Imports System.Runtime.Serialization

Imports Microsoft.Win32
Imports VB6 = Microsoft.VisualBasic
Imports System.Text

<Serializable()>
Public MustInherit Class Demuxer
    MustOverride Sub Run()

    Property DemuxSubtitles As Boolean = True
    Property DemuxAudio As Boolean = True
    Property DemuxChapters As Boolean = True
    Property DemuxAttachments As Boolean = True

    Overridable Property Active As Boolean = True
    Overridable Property InputExtensions As String() = {}
    Overridable Property InputFormats As String() = {}
    Overridable Property InputFormatsBlacklist As String() = {}
    Overridable Property Name As String = ""
    Overridable Property OutputExtensions As String() = {}
    Overridable Property SourceFilters As String() = {}

    Overridable ReadOnly Property HasConfigDialog() As Boolean
        Get
            Return False
        End Get
    End Property

    Overridable Function ShowConfigDialog() As DialogResult
        Return DialogResult.OK
    End Function

    Overrides Function ToString() As String
        Return Name + " (" + InputExtensions.Join(", ") + " -> " + OutputExtensions.Join(", ") + ")"
    End Function

    Shared Function GetDefaults() As List(Of Demuxer)
        Dim ret As New List(Of Demuxer)

        Dim dsmux As New CommandLineDemuxer
        dsmux.Name = "dsmux"
        dsmux.InputExtensions = "ts".Split(" "c)
        dsmux.OutputExtensions = {"mkv"}
        dsmux.InputFormats = {"AVC"}
        dsmux.Command = "%app:dsmux%"
        dsmux.Arguments = """%working_dir%%source_name%.mkv"" ""%source_file%"""
        dsmux.Active = g.IsCOMObjectRegistered(GUIDS.HaaliMuxer)
        dsmux.Description = Strings.dsmux
        ret.Add(dsmux)

        ret.Add(New mkvDemuxer)
        ret.Add(New MP4BoxDemuxer)
        ret.Add(New eac3toDemuxer)

        Dim dgnv As New CommandLineDemuxer
        dgnv.Name = "DGIndexNV"
        dgnv.InputExtensions = "h264 mkv mp4 mpg ts m2ts".Split(" "c)
        dgnv.OutputExtensions = {"dgi"}
        dgnv.InputFormats = {"AVC", "VC-1", "MPEG Video"}
        dgnv.Command = "%app:DGIndexNV%"
        dgnv.Arguments = "-i %source_files_comma% -o ""%working_dir%%source_name%.dgi"" -a -e"
        dgnv.SourceFilters = {"DGSource"}
        dgnv.Active = False
        dgnv.Description = Strings.DGDecNV
        ret.Add(dgnv)

        Dim dgi As New CommandLineDemuxer
        dgi.Name = "DGIndex"
        dgi.InputExtensions = "mpg vob ts pva m2v".Split(" "c)
        dgi.OutputExtensions = {"d2v"}
        dgi.InputFormats = {"MPEG Video"}
        dgi.Command = "%app:DGIndex%"
        dgi.Arguments = "-i %source_files% -ia 2 -fo 0 -yr 1 -tn 1 -om 2 -drc 2 -dsd 0 -dsa 0 -o ""%working_dir%%source_name%"" -hide -exit"
        dgi.SourceFilters = {"MPEG2Source"}
        dgnv.Description = Strings.DGMPGDec
        ret.Add(dgi)

        Return ret
    End Function
End Class

<Serializable()>
Public Class CommandLineDemuxer
    Inherits Demuxer

    Property Command As String = ""
    Property Arguments As String = ""
    Property Description As String = ""

    Overrides ReadOnly Property HasConfigDialog() As Boolean
        Get
            Return True
        End Get
    End Property

    Overrides Function ShowConfigDialog() As DialogResult
        Using f As New DemuxingForm(Me)
            Return f.ShowDialog
        End Using
    End Function

    Overrides Sub Run()
        Using proc As New Proc
            Select Case Name
                Case "dsmux"
                    proc.SkipStrings = {"Muxing..."}
                Case "DGIndex"
                    proc.SkipPatterns = {"^\d+$"}
            End Select

            proc.Init(Name)
            proc.File = Macro.Solve(Command)
            proc.Arguments = Macro.Solve(Arguments)
            proc.Start()

            Dim fp = p.TempDir + Filepath.GetBase(p.SourceFile) + ".log"

            If Not File.Exists(fp) Then
                fp = Filepath.GetDirAndBase(p.SourceFile) + ".log"
            End If

            If File.Exists(fp) AndAlso File.ReadAllText(fp).StartsWith("Stream Type: ") Then
                If Name.Contains("DGIndexNV") Then
                    FileHelp.Move(fp, p.TempDir + Filepath.GetBase(p.SourceFile) + "_DGIndexNV.log")
                ElseIf Name.Contains("DGIndex") Then
                    FileHelp.Move(fp, p.TempDir + Filepath.GetBase(p.SourceFile) + "_DGIndex.log")
                End If
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
Public Class MP4BoxDemuxer
    Inherits Demuxer

    Sub New()
        Name = "MP4Box"
        InputExtensions = {"mp4"}
    End Sub

    Overrides Sub Run()
        If DemuxAudio Then
            For Each i In MediaInfo.GetAudioStreams(p.SourceFile)
                Audio.DemuxMP4(p.SourceFile, i, Nothing)
            Next
        End If

        If Not DemuxSubtitles Then Exit Sub

        Dim subs = MediaInfo.GetSubtitles(p.SourceFile)

        If subs.Count = 0 Then Exit Sub

        For Each i In subs
            Dim outpath = p.TempDir + Filepath.GetBase(p.SourceFile) + " - ID" & i.ID
            outpath += " - " + i.Language.Name
            If i.Title <> "" AndAlso i.Title <> " " Then outpath += " - " + i.Title
            outpath += i.Extension

            If outpath.Length > 220 Then outpath = outpath.Replace(Filepath.GetBase(p.SourceFile), "")

            FileHelp.Delete(outpath)

            Using proc As New Proc
                proc.Init("Demux subtitle using MP4Box", {"Media Export: |", "File Export: |", "ISO File Writing: |", "VobSub Export: |"})
                proc.File = Packs.MP4Box.GetPath
                proc.Arguments = "-raw " & i.ID & " -out """ + outpath + """ """ + p.SourceFile + """"
                proc.Process.StartInfo.EnvironmentVariables("TEMP") = p.TempDir
                proc.Start()
            End Using
        Next
    End Sub

    Public Overrides Function ShowConfigDialog() As DialogResult
        Using f As New SimpleSettingsForm("MP4 Demuxing Options")
            f.Size = New Size(500, 250)

            Dim ui = f.SimpleUI

            Dim page = ui.CreateFlowPage("main page")

            Dim cb = ui.AddCheckBox(page)
            cb.Text = "Demux audio"
            cb.Checked = DemuxAudio
            cb.SaveAction = Sub(value) DemuxAudio = value

            cb = ui.AddCheckBox(page)
            cb.Text = "Demux subtitles"
            cb.Checked = DemuxSubtitles
            cb.SaveAction = Sub(value) DemuxSubtitles = value

            Dim ret = f.ShowDialog()

            If ret = DialogResult.OK Then
                ui.Save()
            End If

            Return ret
        End Using
    End Function

    Public Overrides ReadOnly Property HasConfigDialog As Boolean
        Get
            Return True
        End Get
    End Property
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
        Using f As New eac3toForm
            f.M2TSFile = p.SourceFile
            f.tbTempDir.Text = p.TempDir

            If f.ShowDialog() = DialogResult.OK Then
                Using proc As New Proc
                    proc.TrimChars = {"-"c, " "c}
                    proc.RemoveChars = {CChar(VB6.vbBack)}
                    proc.Init("Demux M2TS using eac3to", "analyze: ", "process: ")
                    proc.File = Packs.eac3to.GetPath
                    proc.Arguments = f.GetArgs("""" + p.SourceFile + """", Filepath.GetBase(p.SourceFile))

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
End Class

<Serializable()>
Public Class mkvDemuxer
    Inherits Demuxer

    Sub New()
        Name = "mkvextract"
        InputExtensions = {"mkv", "webm"}
    End Sub

    Sub DemuxMKVSubtitles()
        If Not DemuxSubtitles Then Exit Sub

        Dim subs = MediaInfo.GetSubtitles(p.SourceFile)

        If subs.Count = 0 Then Exit Sub

        Dim args = "tracks """ + p.SourceFile + """"

        For Each i In subs
            Dim outpath = p.TempDir + Filepath.GetBase(p.SourceFile) + " - " + i.Filename + i.Extension
            If outpath.Length > 220 Then outpath = outpath.Replace(Filepath.GetBase(p.SourceFile), "")
            args += " " & i.StreamOrder & ":""" + outpath + """"
        Next

        args += " --ui-language en"

        Using proc As New Proc
            proc.Init("Demux subtitles using mkvextract", "Progress: ")
            proc.Encoding = Encoding.UTF8
            proc.File = Packs.Mkvmerge.GetDir + "mkvextract.exe"
            proc.Arguments = args
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using
    End Sub

    Overrides Sub Run()
        If DemuxAudio Then
            For Each i In MediaInfo.GetAudioStreams(p.SourceFile)
                Audio.DemuxMKV(p.SourceFile, i, Nothing)
            Next
        End If

        If DemuxSubtitles Then
            DemuxMKVSubtitles()
        End If

        If DemuxChapters Then
            Dim output = ProcessHelp.GetStandardOutput(
                Packs.Mkvmerge.GetDir + "mkvinfo.exe", "--ui-language en """ + p.SourceFile + """")

            If output.Contains("|+ Chapters") Then
                Using proc As New Proc
                    proc.Init("Demux chapters using mkvextract", "Progress: ")
                    proc.Encoding = Encoding.UTF8
                    proc.File = Packs.Mkvmerge.GetDir + "mkvextract.exe"
                    proc.Arguments = "chapters """ + p.SourceFile + """ --redirect-output """ +
                        p.TempDir + Filepath.GetBase(p.SourceFile) + "_Chapters.xml"""
                    proc.Start()
                End Using
            End If
        End If

        If DemuxAttachments Then
            Dim output = ProcessHelp.GetStandardOutput(Packs.Mkvmerge.GetPath, "--identify-verbose --ui-language en """ + p.SourceFile + """")

            Dim params As String

            For Each i In output.SplitLinesNoEmpty
                If i.StartsWith("Attachment ID ") Then
                    Dim m = Regex.Match(i, "Attachment ID (\d+):.+, file name '(.+)'")

                    If m.Success Then
                        params += " " + m.Groups(1).Value + ":""" + p.TempDir + Filepath.GetBase(p.SourceFile) + "_attachment_" + m.Groups(2).Value + """"
                    End If
                End If
            Next

            If params <> "" Then
                params += " --ui-language en"

                Using proc As New Proc
                    proc.Init("Demux attachments using mkvextract", "Progress: ")
                    proc.WriteLine(output)
                    proc.Encoding = Encoding.UTF8
                    proc.File = Packs.Mkvmerge.GetDir + "mkvextract.exe"
                    proc.Arguments = "attachments """ + p.SourceFile + """" + params
                    proc.Start()
                End Using
            End If
        End If
    End Sub

    Public Overrides Function ShowConfigDialog() As DialogResult
        Using f As New SimpleSettingsForm("MKV Demuxing Options")
            f.Size = New Size(500, 300)

            Dim ui = f.SimpleUI

            Dim page = ui.CreateFlowPage("main page")

            Dim cb = ui.AddCheckBox(page)
            cb.Text = "Demux audio"
            cb.Tooltip = "Due to the new stream selection feature audio demuxing is no longer necessary."
            cb.Checked = DemuxAudio
            cb.SaveAction = Sub(value) DemuxAudio = value

            cb = ui.AddCheckBox(page)
            cb.Text = "Demux subtitles"
            cb.Tooltip = "MKV and MP4 files can be used as subtitle source files so demuxing isn't necessary, one limitation is the preview don't work."
            cb.Checked = DemuxSubtitles
            cb.SaveAction = Sub(value) DemuxSubtitles = value

            cb = ui.AddCheckBox(page)
            cb.Text = "Demux chapters"
            cb.Tooltip = "Chapters still require demuxing."
            cb.Checked = DemuxChapters
            cb.SaveAction = Sub(value) DemuxChapters = value

            cb = ui.AddCheckBox(page)
            cb.Text = "Demux attachments"
            cb.Tooltip = "Attachments still require demuxing."
            cb.Checked = DemuxAttachments
            cb.SaveAction = Sub(value) DemuxAttachments = value

            Dim ret = f.ShowDialog()

            If ret = DialogResult.OK Then
                ui.Save()
            End If

            Return ret
        End Using
    End Function

    Public Overrides ReadOnly Property HasConfigDialog As Boolean
        Get
            Return True
        End Get
    End Property
End Class