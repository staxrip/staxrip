Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Globalization

Imports vb6 = Microsoft.VisualBasic

<Serializable()>
Public MustInherit Class Muxer
    Inherits Profile

    Property ChapterFile As String

    MustOverride Sub Mux()

    MustOverride ReadOnly Property OutputExt As String

    ReadOnly Property OutputExtFull As String
        Get
            Return "." + OutputExt
        End Get
    End Property

    Overridable ReadOnly Property SupportedInputTypes As String()
        Get
            Return New String() {}
        End Get
    End Property

    Sub New()
    End Sub

    Sub New(name As String)
        MyBase.New(name)
        CanEditValue = True
    End Sub

    Private AdditionalSwitchesValue As String

    Property AdditionalSwitches() As String
        Get
            Return AdditionalSwitchesValue
        End Get
        Set(Value As String)
            If Value = "" Then
                AdditionalSwitchesValue = Nothing
            Else
                AdditionalSwitchesValue = Value
            End If
        End Set
    End Property

    Private SubtitlesValue As List(Of Subtitle)

    Property Subtitles() As List(Of Subtitle)
        Get
            If SubtitlesValue Is Nothing Then
                SubtitlesValue = New List(Of Subtitle)
            End If

            Return SubtitlesValue
        End Get
        Set(value As List(Of Subtitle))
            SubtitlesValue = value
        End Set
    End Property

    Overrides Sub Clean()
        Subtitles = Nothing
        ChapterFile = Nothing
    End Sub

    Overridable Function GetError() As String
        Return Nothing
    End Function

    Overridable Function GetCommandLine() As String
        Return Nothing
    End Function

    Overridable Sub Init()
        If Not File.Exists(p.SourceFile) Then Exit Sub

        Dim files = g.GetFilesInTempDirAndParent
        files.Sort(New StringLogicalComparer)

        For Each file1 In files
            If Filepath.GetExtFull(file1) = ".idx" Then
                Dim v = File.ReadAllText(file1, Encoding.Default)

                If v.Contains(vb6.ChrW(&HA) + vb6.ChrW(&H0) + vb6.ChrW(&HD) + vb6.ChrW(&HA)) Then
                    v = v.FixBreak
                    v = v.Replace(BR + vb6.ChrW(&H0) + BR, BR + "langidx: 0" + BR)
                    File.WriteAllText(file1, v, Encoding.Default)
                End If
            End If

            If FileTypes.SubtitleExludingContainers.Contains(Filepath.GetExt(file1)) AndAlso
                g.IsSourceSameOrSimilar(file1) AndAlso Not file1.Contains("_Preview.") AndAlso
                Not file1.Contains("_Temp.") Then

                If p.ConvertSup2Sub AndAlso Filepath.GetExtFull(file1) = ".sup" Then
                    Continue For
                End If

                If TypeOf Me Is MP4Muxer AndAlso Not {"idx", "srt"}.Contains(Filepath.GetExt(file1)) Then
                    Continue For
                End If

                If file1.Contains("_Forced.") AndAlso Not Filepath.GetBase(file1).Contains(Language.CurrentCulture.Name) Then
                    Continue For
                End If

                For Each iSubtitle In Subtitle.Create(file1)
                    If p.PreferredSubtitles <> "" Then
                        If file1.Contains("_Forced.") Then
                            Static forcedAdded As Boolean

                            If forcedAdded Then
                                Continue For
                            Else
                                iSubtitle.Forced = True
                                forcedAdded = True
                            End If
                        End If

                        Subtitles.Add(iSubtitle)
                    End If
                Next
            End If
        Next

        If p.PreferredSubtitles <> "" AndAlso Subtitles.Count = 0 AndAlso
            p.FirstOriginalSourceFile.Ext.EqualsAny("mkv", "mp4", "m2ts") AndAlso
            MediaInfo.GetSubtitleCount(p.FirstOriginalSourceFile) > 0 AndAlso
            TypeOf Me Is MkvMuxer Then

            Subtitles.AddRange(Subtitle.Create(p.FirstOriginalSourceFile))
        End If

        For Each i In files
            If g.IsSourceSameOrSimilar(i) Then
                If Not TypeOf Me Is WebMMuxer Then
                    If i.ToLower Like "*chapter*txt" Then
                        ChapterFile = i
                    End If

                    If i.ToLower.EndsWith(".xml") AndAlso
                        File.ReadAllText(i).Contains("<Chapters>") Then

                        ChapterFile = i
                    End If
                End If

                If TypeOf Me Is MkvMuxer AndAlso i.Contains("_attachment_") Then
                    AdditionalSwitches += " --attachment-name """ + i.Right("_attachment_") + """ --attach-file """ + i + """"
                End If
            End If
        Next

        If AdditionalSwitches <> "" AndAlso AdditionalSwitches.StartsWith(" ") Then
            AdditionalSwitches = AdditionalSwitches.TrimStart
        End If
    End Sub

    Overridable Function IsSupported(fileType As String) As Boolean
        Return SupportedInputTypes.Contains(fileType)
    End Function

    Shared Function GetDefaults() As List(Of Muxer)
        Dim ret As New List(Of Muxer)

        ret.Add(New MkvMuxer())
        ret.Add(New MP4Muxer("MP4"))
        ret.Add(New ffmpegMuxer("AVI"))
        ret.Add(New WebMMuxer())
        ret.Add(New DivXPluxMuxer())
        ret.Add(New BatchMuxer("Command Line"))
        ret.Add(New NullMuxer("No Muxing"))

        Return ret
    End Function
End Class

<Serializable()>
Class MP4Muxer
    Inherits Muxer

    Sub New(name As String)
        MyBase.New(name)
    End Sub

    Overrides ReadOnly Property OutputExt As String
        Get
            Return "mp4"
        End Get
    End Property

    Overrides Function Edit() As DialogResult
        Using f As New MuxerForm(Me)
            Return f.ShowDialog()
        End Using
    End Function

    Overrides Function GetCommandLine() As String
        Return """" + Package.MP4Box.Path + """ " + GetArgs()
    End Function

    Private Function GetArgs() As String
        Dim args As New StringBuilder

        If MediaInfo.GetFrameRate(p.VideoEncoder.OutputPath, 0) = 0 Then
            args.Append(" -fps " + p.Script.GetFramerate.ToString("f6", CultureInfo.InvariantCulture))
        End If

        Dim temp As String = Nothing
        Dim par = Calc.GetTargetPAR

        If TypeOf p.VideoEncoder Is NullEncoder Then
            temp = ":par=" & par.X & ":" & par.Y
        End If

        args.Append(" -add """ + p.VideoEncoder.OutputPath + "#video" + temp + """")

        AddAudio(p.Audio0, args)
        AddAudio(p.Audio1, args)

        For Each i In p.AudioTracks
            AddAudio(i, args)
        Next

        For Each i In Subtitles
            If i.Enabled AndAlso File.Exists(i.Path) Then
                If i.Path.Ext = "idx" Then
                    If i.Title = "" Then i.Title = " "
                    args.Append(" -add """ + i.Path + "#" & i.IndexIDX + 1 & ":name=" + Macro.Solve(i.Title, True) & """")
                Else
                    If i.Title = "" Then i.Title = " "
                    args.Append(" -add """ + i.Path + ":lang=" + i.Language.ThreeLetterCode + ":name=" + Macro.Solve(i.Title, True) + """")
                End If
            End If
        Next

        If File.Exists(ChapterFile) Then args.Append(" -chap """ + ChapterFile + """")
        If AdditionalSwitches <> "" Then args.Append(" " + Macro.Solve(AdditionalSwitches))

        args.Append(" -new """ + p.TargetFile + """")

        Return args.ToString.Trim
    End Function

    Sub AddAudio(ap As AudioProfile, args As StringBuilder)
        If File.Exists(ap.File) AndAlso IsSupported(ap.File.Ext) AndAlso IsSupported(ap.OutputFileType) Then
            args.Append(" -add """ + ap.File)

            If ap.HasStream AndAlso Filepath.GetExtFull(ap.File) = ".mp4" Then
                args.Append("#trackID=" & ap.Stream.ID)
            Else
                args.Append("#audio")
            End If

            args.Append(":lang=" + ap.Language.ThreeLetterCode)

            If ap.Delay <> 0 AndAlso Not ap.HandlesDelay Then
                args.Append(":delay=" + ap.Delay.ToString)
            End If

            args.Append(":name=" + ap.SolveMacros(ap.StreamName))
            args.Append("""")
        End If
    End Sub

    Overrides Sub Mux()
        Using proc As New Proc
            proc.Init("Muxing using MP4Box " + Package.MP4Box.Version, {"|"})
            proc.File = Package.MP4Box.Path
            proc.Arguments = GetArgs()
            proc.Process.StartInfo.EnvironmentVariables("TEMP") = p.TempDir
            proc.Process.StartInfo.EnvironmentVariables("TMP") = p.TempDir
            proc.Start()
        End Using

        If Not g.WasFileJustWritten(p.TargetFile) Then
            Throw New ErrorAbortException("Error MP4 output file is missing.", GetArgs())
        End If

        Log.WriteLine(MediaInfo.GetSummary(p.TargetFile))
    End Sub

    Overrides Sub Clean()
        Subtitles = Nothing
    End Sub

    Overrides ReadOnly Property SupportedInputTypes() As String()
        Get
            Return {"ts", "m2ts",
                    "mpg", "m2v",
                    "avi", "ac3",
                    "mp4", "m4a", "aac", "mov",
                    "264", "h264", "avc",
                    "265", "h265", "hevc",
                    "mp2", "mpa", "mp3"}
        End Get
    End Property
End Class

<Serializable()>
Class NullMuxer
    Inherits Muxer

    Sub New(name As String)
        MyBase.New(name)
        CanEditValue = False
    End Sub

    Overrides Function IsSupported(type As String) As Boolean
        Return True
    End Function

    Overrides ReadOnly Property OutputExt As String
        Get
            Return p.VideoEncoder.OutputExt
        End Get
    End Property

    Overrides Sub Mux()
    End Sub

    Public Overrides Sub Init()
    End Sub
End Class

<Serializable()>
Class BatchMuxer
    Inherits Muxer

    Property OutputTypeValue As String = "mp4"
    Property CommandLines As String = """%app:MP4Box%"" -nodrop -add ""%encoder_out_file%#video"" -add ""%audio_file1%"" -new ""%target_file%"""

    Sub New(name As String)
        MyBase.New(name)
    End Sub

    Public Overrides ReadOnly Property OutputExt As String
        Get
            Return OutputTypeValue
        End Get
    End Property

    Overrides Function IsSupported(type As String) As Boolean
        Return True
    End Function

    Overrides Sub Mux()
        Log.WriteHeader("Batch Muxing")

        Dim batchPath = p.TempDir + Filepath.GetBase(p.TargetFile) + "_mux.bat"
        Dim batchCode = Proc.WriteBatchFile(batchPath, Macro.Solve(CommandLines))

        Using proc As New Proc
            proc.Init("Encoding video command line encoder: " + Name)
            proc.WriteLine(batchCode + BR2)
            proc.File = "cmd.exe"
            proc.Arguments = "/C call """ + batchPath + """"

            Try
                proc.Start()
            Catch ex As AbortException
                Throw ex
            Catch ex As Exception
                ProcessForm.CloseProcessForm()
                g.ShowException(ex)
                Throw New AbortException
            End Try
        End Using
    End Sub

    Overrides Function Edit() As DialogResult
        Using f As New SimpleSettingsForm("Batch Muxer", "The Batch Muxer dialog allows to configure StaxRip to use a command line or batch code as muxer.")
            f.Height = CInt(f.Height * 0.6)

            Dim ui = f.SimpleUI
            Dim page = ui.CreateFlowPage("main page")

            Dim tb = ui.AddTextBlock(page)
            tb.Label.Text = "Output File Type:"
            tb.Edit.Text = OutputTypeValue
            tb.Edit.SaveAction = Sub(value) OutputTypeValue = value

            Dim l = ui.AddLabel(page, "Batch Script:")
            l.MarginTop = f.Font.Height
            l.Tooltip = "Batch script which may contain macros."

            tb = ui.AddTextBlock(page)
            tb.Label.Visible = False
            tb.Edit.Expandet = True
            tb.Edit.MultilineHeightFactor = 6
            tb.Edit.TextBox.Multiline = True
            tb.Edit.Text = CommandLines
            tb.Edit.UseCommandlineEditor = True
            tb.Edit.SaveAction = Sub(value) CommandLines = value

            Dim ret = f.ShowDialog()
            If ret = DialogResult.OK Then
                ui.Save()

                If p.TargetFile <> "" Then
                    p.TargetFile = p.TargetFile.DirAndBase + "." + OutputTypeValue
                End If
            End If

            Return ret
        End Using
    End Function
End Class

<Serializable()>
Class MkvMuxer
    Inherits Muxer

    Property VideoTrackName As String = ""
    Property VideoTrackLanguage As New Language(CultureInfo.InvariantCulture)
    Property Title As String = ""

    Sub New()
        Name = "MKV"
    End Sub

    Sub New(name As String)
        MyBase.New(name)
    End Sub

    Overrides ReadOnly Property OutputExt As String
        Get
            Return "mkv"
        End Get
    End Property

    Overrides Function Edit() As DialogResult
        Using f As New MuxerForm(Me)
            Return f.ShowDialog()
        End Using
    End Function

    Overrides Sub Mux()
        Using proc As New Proc
            proc.Init("Muxing using mkvmerge " + Package.mkvmerge.Version, "Progress: ")
            proc.Encoding = Encoding.UTF8
            proc.File = Package.mkvmerge.Path
            proc.Arguments = GetArgs()
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using

        If Not g.WasFileJustWritten(p.TargetFile) Then
            Log.Write("Error MKV output file is missing", p.TargetFile)
        End If

        Log.WriteLine(MediaInfo.GetSummary(p.TargetFile))
    End Sub

    Overrides Function GetCommandLine() As String
        Return """" + Package.mkvmerge.Path + """ " + GetArgs()
    End Function

    Private Function GetArgs() As String
        Dim args As New StringBuilder("-o " + p.TargetFile.Quotes)

        Dim vID = -1 '-1 means all

        args.Append(" --noaudio --nosubs --no-chapters --no-attachments --no-track-tags --no-global-tags")

        If VideoTrackLanguage.ThreeLetterCode <> "und" Then
            args.Append(" --language " & vID & ":" + VideoTrackLanguage.ThreeLetterCode)
        End If

        If VideoTrackName <> "" Then
            args.Append(" --track-name """ & vID & ":" + Macro.Solve(VideoTrackName).Replace("""", "'") + """")
        End If

        If MediaInfo.GetFrameRate(p.VideoEncoder.OutputPath, 0) = 0 Then
            args.Append(" --default-duration 0:" + p.Script.GetFramerate.ToString("f6", CultureInfo.InvariantCulture) + "fps")
        End If

        args.Append(" " + p.VideoEncoder.OutputPath.Quotes)

        AddAudioArgs(p.Audio0, args)
        AddAudioArgs(p.Audio1, args)

        For Each i In p.AudioTracks
            AddAudioArgs(i, args)
        Next

        For Each i In Subtitles
            If i.Enabled AndAlso File.Exists(i.Path) Then
                Dim id = i.StreamOrder

                If {"mkv", "mp4", "m2ts", "idx"}.Contains(Filepath.GetExt(i.Path)) Then
                    args.Append(" --no-audio --no-video --no-chapters --no-attachments --no-track-tags --no-global-tags")
                    args.Append(" --subtitle-tracks " & id)
                Else
                    id = 0
                End If

                args.Append(" --forced-track " & id & ":" & If(i.Forced, 1, 0))
                args.Append(" --default-track " & id & ":" & If(i.Default, 1, 0))
                args.Append(" --language " & id & ":" + i.Language.ThreeLetterCode)
                args.Append(" --track-name """ & id & ":" + i.Title?.Trim.Replace("""", "'") + """")
                args.Append(" """ + i.Path + """")
            End If
        Next

        If Not TypeOf Me Is WebMMuxer AndAlso File.Exists(ChapterFile) Then
            args.Append(" --chapters " + ChapterFile.Quotes)
        End If

        If Title <> "" Then args.Append(" --title """ + Macro.Solve(Title).Replace("""", "'") + """")

        If TypeOf p.VideoEncoder Is NullEncoder AndAlso p.Ranges.Count > 0 Then
            args.Append(" --split parts-frames:" + p.Ranges.Select(Function(v) v.Start & "-" & v.End).Join(",+"))
        End If

        args.Append(" --ui-language en")
        If AdditionalSwitches <> "" Then args.Append(" " + Macro.Solve(AdditionalSwitches))

        Return args.ToString
    End Function

    Sub AddAudioArgs(ap As AudioProfile, args As StringBuilder)
        If File.Exists(ap.File) AndAlso IsSupported(ap.File.Ext) AndAlso IsSupported(ap.OutputFileType) Then
            Dim tid = 0
            Dim isCombo As Boolean

            If Not ap.Stream Is Nothing Then
                tid = ap.Stream.StreamOrder
                isCombo = ap.Stream.Name.Contains("THD+AC3")
            Else
                tid = MediaInfo.GetAudio(ap.File, "StreamOrder").ToInt
                isCombo = ap.File.Ext = "thd+ac3"
            End If

            args.Append(" --novideo --nosubs --no-chapters --no-attachments --no-track-tags --no-global-tags --audio-tracks " + If(isCombo, tid & "," & tid + 1, tid.ToString))

            args.Append(" --language " & tid & ":" + ap.Language.ThreeLetterCode)
            If isCombo Then args.Append(" --language " & tid + 1 & ":" + ap.Language.ThreeLetterCode)

            If ap.OutputFileType = "aac" AndAlso ap.File.ToLower.Contains("sbr") Then
                args.Append(" --aac-is-sbr " & tid)
            End If

            args.Append(" --track-name """ & tid & ":" + ap.SolveMacros(ap.StreamName, False) + """")
            If isCombo Then args.Append(" --track-name """ & tid + 1 & ":" + ap.SolveMacros(ap.StreamName, False).Replace("""", "'") + """")

            If ap.Delay <> 0 AndAlso Not ap.HandlesDelay AndAlso Not (ap.HasStream AndAlso ap.Stream.Delay <> 0) Then
                args.Append(" --sync " & tid & ":" + ap.Delay.ToString)
                If isCombo Then args.Append(" --sync " & tid + 1 & ":" + ap.Delay.ToString)
            End If

            args.Append(" --default-track " & tid & ":" & If(ap.Default, 1, 0))
            args.Append(" " + ap.File.Quotes)
        End If
    End Sub

    Overrides ReadOnly Property SupportedInputTypes() As String()
        Get
            Return FileTypes.mkvmergeInput
        End Get
    End Property
End Class

<Serializable()>
Class DivXPluxMuxer
    Inherits MkvMuxer

    Sub New()
        MyBase.New("MKV for DivX Plus")
    End Sub

    Overrides ReadOnly Property SupportedInputTypes() As String()
        Get
            Return {"h264", "mkv", "m4a", "mp4", "aac", "ac3"}
        End Get
    End Property
End Class

<Serializable()>
Class WebMMuxer
    Inherits MkvMuxer

    Sub New()
        MyBase.New("WebM")
    End Sub

    Overrides ReadOnly Property SupportedInputTypes() As String()
        Get
            Return {"mkv", "webm", "mka", "ogg", "opus"}
        End Get
    End Property

    Overrides ReadOnly Property OutputExt As String
        Get
            Return "webm"
        End Get
    End Property
End Class

<Serializable()>
Class ffmpegMuxer
    Inherits Muxer

    Property OutputTypeValue As String = "avi"

    Sub New(name As String)
        MyBase.New(name)
    End Sub

    ReadOnly Property AVITypes As String()
        Get
            Return {"avi", "mp2", "mp3", "ac3", "mpa", "wav"}
        End Get
    End Property

    Public Overrides ReadOnly Property OutputExt As String
        Get
            Return OutputTypeValue
        End Get
    End Property

    Public Overrides ReadOnly Property SupportedInputTypes As String()
        Get
            If OutputExt = "avi" Then Return AVITypes
            Return MyBase.SupportedInputTypes
        End Get
    End Property

    Overrides Function IsSupported(type As String) As Boolean
        If OutputExt = "avi" Then Return AVITypes.Contains(type)
        Return True
    End Function

    Overrides Sub Mux()
        Dim args = "-i """ + p.VideoEncoder.OutputPath + """"

        Dim id As Integer
        Dim mapping = " -map 0:v"

        For Each i In {p.Audio0, p.Audio1}
            If File.Exists(i.File) AndAlso IsSupported(i.OutputFileType) Then
                id += 1
                args += " -i """ + i.File + """"
                mapping += " -map " & id
                If Not i.Stream Is Nothing Then mapping += ":" & i.Stream.StreamOrder
            End If
        Next

        args += mapping
        args += " -c:v copy -c:a copy -y"
        args += " """ + p.TargetFile + """"

        Using proc As New Proc
            proc.Init("Muxing to " + OutputTypeValue + " using ffmpeg " + Package.ffmpeg.Version, "frame=")
            proc.Encoding = Encoding.UTF8
            proc.File = Package.ffmpeg.Path
            proc.Arguments = args
            proc.Start()
        End Using
    End Sub

    Overrides Function Edit() As DialogResult
        Using f As New SimpleSettingsForm(Name)
            f.Height = CInt(f.Height * 0.5)
            Dim ui = f.SimpleUI
            Dim page = ui.CreateFlowPage("main page")

            Dim tb = ui.AddTextBlock(page)
            tb.Label.Text = "Output File Type:"
            tb.Edit.Text = OutputTypeValue
            tb.Edit.SaveAction = Sub(value) OutputTypeValue = value

            Dim ret = f.ShowDialog()

            If ret = DialogResult.OK Then
                ui.Save()
                p.TargetFile = p.TargetFile.DirAndBase + "." + OutputTypeValue
            End If

            Return ret
        End Using
    End Function
End Class