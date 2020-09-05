
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Xml.Linq
Imports System.ComponentModel
Imports System.Runtime.Serialization

Imports VB6 = Microsoft.VisualBasic

<Serializable()>
Public MustInherit Class Muxer
    Inherits Profile

    Property CoverFile As String = ""
    Property ChapterFile As String = ""
    Property TagFile As String = ""
    Property TimestampsFile As String = ""
    Property VideoTrackName As String = ""

    MustOverride Sub Mux()

    MustOverride ReadOnly Property OutputExt As String

    Sub New()
    End Sub

    Sub New(name As String)
        MyBase.New(name)
        CanEditValue = True
    End Sub

    Private TagsValue As BindingList(Of StringPair)

    Property Tags As BindingList(Of StringPair)
        Get
            If TagsValue Is Nothing Then
                TagsValue = New BindingList(Of StringPair)
            End If
            Return TagsValue
        End Get
        Set(value As BindingList(Of StringPair))
            TagsValue = value
        End Set
    End Property

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

    Private AdditionalSwitchesValue As String

    Property AdditionalSwitches As String
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

    Private AttachmentsValue As List(Of String)

    Property Attachments As List(Of String)
        Get
            If AttachmentsValue Is Nothing Then
                AttachmentsValue = New List(Of String)
            End If

            AttachmentsValue.Sort()
            Return AttachmentsValue
        End Get
        Set(value As List(Of String))
            AttachmentsValue = value
        End Set
    End Property

    <OnDeserialized>
    Sub OnDeserialized(context As StreamingContext)
        If TagFile Is Nothing Then
            TagFile = ""
        End If
    End Sub

    Overrides Sub Clean()
        Subtitles = Nothing
        ChapterFile = Nothing
        TagFile = Nothing
        TimestampsFile = Nothing
        Tags = Nothing
        Attachments = Nothing
    End Sub

    Protected Sub ExpandMacros()
        For Each i In Subtitles
            If i.Title?.Contains("%") Then
                If i.Title.Contains("%language_native%") Then
                    i.Title = i.Title.Replace("%language_native%", i.Language.CultureInfo.NativeName)
                End If

                If i.Title.Contains("%language_english%") Then
                    i.Title = i.Title.Replace("%language_english%", i.Language.Name)
                End If

                If i.Title.Contains("%") Then
                    i.Title = Macro.Expand(i.Title)
                End If
            End If
        Next
    End Sub

    Overridable Function GetError() As String
        Return Nothing
    End Function

    Overridable Function GetCommandLine() As String
        Return Nothing
    End Function

    Overridable Sub Init()
        If Not File.Exists(p.SourceFile) Then
            Exit Sub
        End If

        Dim files = g.GetFilesInTempDirAndParent
        files.Sort(New StringLogicalComparer)

        If File.Exists(p.FirstOriginalSourceFile.DirAndBase + ".nfo") Then
            Try
                Dim fieldNames = "title originaltitle showtitle studio genre director season episode premiered aired outline plot tagline".Split(" "c)
                Dim root = XElement.Parse((p.FirstOriginalSourceFile.DirAndBase + ".nfo").ReadAllText)
                Dim elements = root.Elements.Where(Function(x) fieldNames.Contains(x.Name.ToString()) AndAlso x.Value <> "")

                For Each i In elements
                    Tags.Add(New StringPair(i.Name.ToString, i.Value))
                Next
            Catch
            End Try
        End If

        For Each fp In files
            If fp.Ext = "idx" Then
                Dim v = fp.ReadAllTextDefault

                If v.Contains(VB6.ChrW(&HA) + VB6.ChrW(&H0) + VB6.ChrW(&HD) + VB6.ChrW(&HA)) Then
                    v = v.FixBreak
                    v = v.Replace(BR + VB6.ChrW(&H0) + BR, BR + "langidx: 0" + BR)
                    File.WriteAllText(fp, v, Encoding.Default)
                End If
            End If

            If FileTypes.SubtitleExludingContainers.Contains(fp.Ext) AndAlso
                g.IsSourceSameOrSimilar(fp) AndAlso Not fp.Contains("_view.") AndAlso
                Not fp.Contains("_Temp.") Then

                If p.ConvertSup2Sub AndAlso fp.Ext = "sup" Then
                    Continue For
                End If

                If TypeOf Me Is MP4Muxer AndAlso Not {"idx", "srt"}.Contains(fp.Ext) Then
                    Continue For
                End If

                For Each iSubtitle In Subtitle.Create(fp)
                    If p.PreferredSubtitles <> "" Then
                        If fp.Contains("_forced") Then iSubtitle.Forced = True
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
                    Dim lower = i.ToLower

                    If lower.Contains("chapters") Then
                        If TypeOf Me Is MP4Muxer Then
                            If i.Ext = "txt" Then
                                ChapterFile = i
                            End If
                        Else
                            If i.Ext.EqualsAny("txt", "xml") Then
                                ChapterFile = i
                            End If
                        End If
                    End If
                End If
            End If

            If TypeOf Me Is MkvMuxer AndAlso i.Contains("_attachment_") Then
                Attachments.Add(i)
            End If
        Next

        For Each iDir In {p.TempDir, p.TempDir.Parent}
            For Each iExt In {"jpg", "png"}
                Dim fp = iDir + "cover." + iExt
                If File.Exists(fp) Then CoverFile = fp
            Next
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

        ret.AddRange({New MkvMuxer(), New MP4Muxer(), New WebMMuxer()})
        ret.AddRange(ffmpegMuxer.SupportedFormats.Select(Function(val) New ffmpegMuxer("ffmpeg | " + val) With {.OutputFormat = val}))
        ret.AddRange({New BatchMuxer("Command Line"), New NullMuxer("No Muxing")})

        Return ret
    End Function
End Class

<Serializable()>
Public Class MP4Muxer
    Inherits Muxer

    Property PAR As String = ""

    Sub New()
        MyClass.New("MP4 (mp4box)")
    End Sub

    Sub New(name As String)
        MyBase.New(name)
    End Sub

    Overrides ReadOnly Property OutputExt As String
        Get
            Return "mp4"
        End Get
    End Property

    Overrides Function Edit() As DialogResult
        Using form As New MuxerForm(Me)
            Return form.ShowDialog()
        End Using
    End Function

    Overrides Function GetCommandLine() As String
        Return Package.MP4Box.Path.Escape + " " + GetArgs()
    End Function

    Function GetArgs() As String
        Dim args As New StringBuilder

        If MediaInfo.GetFrameRate(p.VideoEncoder.OutputPath, 0) = 0 Then
            args.Append(" -fps " + p.Script.GetFramerate.ToString("f6", CultureInfo.InvariantCulture))
        End If

        Dim videoParams = ""

        If PAR <> "" Then
            Dim par = Me.PAR.TrimEx

            If par.EqualsAny("auto", "0") Then
                Dim par2 = Calc.GetTargetPAR
                par = par2.X & "/" & par2.Y
            End If

            Dim val = Calc.ParseCustomAR(par, 0, 0)

            If val.X <> 0 Then
                videoParams = ":par=" & val.X & ":" & val.Y
            End If
        ElseIf Calc.IsARSignalingRequired Then
            If TypeOf p.VideoEncoder Is NullEncoder Then
                Dim par = Calc.GetTargetPAR
                videoParams = ":par=" & par.X & ":" & par.Y
            End If
        End If

        videoParams += ":name="

        If VideoTrackName <> "" Then
            videoParams += Macro.Expand(VideoTrackName)
        End If

        args.Append(" -add " + (p.VideoEncoder.OutputPath.ToShortFilePath + "#video" + videoParams).Escape)

        For x = 2 To 99
            Dim fp = p.VideoEncoder.OutputPath.DirAndBase + "_chunk" & x & p.VideoEncoder.OutputExtFull

            If fp.FileExists Then
                args.Append(" -cat " + fp.ToShortFilePath.Escape)
            Else
                Exit For
            End If
        Next

        AddAudio(p.Audio0, args)
        AddAudio(p.Audio1, args)

        For Each track In p.AudioTracks
            AddAudio(track, args)
        Next

        ExpandMacros()

        For Each st In Subtitles
            If st.Enabled AndAlso File.Exists(st.Path) Then
                If st.Path.Ext = "idx" Then
                    args.Append(" -add """ + st.Path.ToShortFilePath + "#" & st.IndexIDX + 1 &
                                ":name=" + Macro.Expand(st.Title) & """")
                Else
                    args.Append(" -add """ + st.Path.ToShortFilePath + ":lang=" + st.Language.ThreeLetterCode +
                                ":name=" + Macro.Expand(st.Title) + """")
                End If
            End If
        Next

        If File.Exists(ChapterFile) Then
            args.Append(" -chap " + ChapterFile.ToShortFilePath.Escape)
        End If

        If AdditionalSwitches <> "" Then
            args.Append(" " + Macro.Expand(AdditionalSwitches))
        End If

        Dim tagList As New List(Of String)

        If CoverFile <> "" AndAlso File.Exists(CoverFile) Then
            tagList.Add("cover=" + CoverFile.ToShortFilePath.Escape)
        End If

        If Tags.Count > 0 Then
            tagList.AddRange(Tags.Select(Function(val) val.Name + "=" + val.Value))
        End If

        If tagList.Count > 0 Then
            args.Append(" -itags " + Macro.Expand(String.Join(":", tagList)))
        End If

        args.Append(" -new " + p.TargetFile.ToShortFilePath.Escape)

        Return args.ToString.Trim
    End Function

    Sub AddAudio(ap As AudioProfile, args As StringBuilder)
        If File.Exists(ap.File) AndAlso IsSupported(ap.File.Ext) AndAlso IsSupported(ap.OutputFileType) Then
            args.Append(" -add """ + ap.File.ToShortFilePath)

            If ap.HasStream AndAlso ap.File.Ext = "mp4" Then
                args.Append("#trackID=" & ap.Stream.ID)
            Else
                args.Append("#audio")
            End If

            args.Append(":lang=" + ap.Language.ThreeLetterCode)

            If ap.Delay <> 0 AndAlso Not ap.HandlesDelay Then
                args.Append(":delay=" + ap.Delay.ToString)
            End If

            args.Append(":name=" + ap.ExpandMacros(ap.StreamName))
            args.Append("""")
        End If
    End Sub

    Overrides Sub Mux()
        Using proc As New Proc
            proc.Header = "Muxing"
            proc.SkipString = "|"
            proc.Package = Package.MP4Box
            proc.Arguments = GetArgs()
            proc.Process.StartInfo.EnvironmentVariables("TEMP") = p.TempDir
            proc.Process.StartInfo.EnvironmentVariables("TMP") = p.TempDir
            proc.Start()
        End Using

        If Not g.FileExists(p.TargetFile) Then
            Throw New ErrorAbortException("MP4 output file is missing.", GetArgs())
        End If

        Log.WriteLine(MediaInfo.GetSummary(p.TargetFile))
    End Sub

    Overrides Sub Clean()
        Subtitles = Nothing
    End Sub

    Overrides ReadOnly Property SupportedInputTypes() As String()
        Get
            Return {"ts", "m2ts", "ivf",
                    "mpg", "m2v",
                    "avi", "ac3", "opus",
                    "mp4", "m4a", "aac", "mov",
                    "264", "h264", "avc",
                    "265", "h265", "hevc", "hvc",
                    "mp2", "mpa", "mp3"}
        End Get
    End Property
End Class

<Serializable()>
Public Class NullMuxer
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
Public Class BatchMuxer
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

        Dim batchPath = p.TempDir + p.TargetFile.Base + "_mux.bat"
        Dim batchCode = Proc.WriteBatchFile(batchPath, Macro.Expand(CommandLines))

        Using proc As New Proc
            proc.Header = "Video encoding command line encoder: " + Name
            proc.WriteLog(batchCode + BR2)
            proc.File = "cmd.exe"
            proc.Arguments = "/C call """ + batchPath + """"

            Try
                proc.Start()
            Catch ex As AbortException
                Throw ex
            Catch ex As Exception
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

            Dim tb = ui.AddText(page)
            tb.Label.Text = "Output File Type:"
            tb.Edit.Text = OutputTypeValue
            tb.Edit.SaveAction = Sub(value) OutputTypeValue = value

            Dim l = ui.AddLabel(page, "Batch Script:")
            l.MarginTop = f.Font.Height
            l.Help = "Batch script which may contain macros."

            tb = ui.AddText(page)
            tb.Label.Visible = False
            tb.Edit.Expand = True
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
Public Class MkvMuxer
    Inherits Muxer

    Property VideoTrackLanguage As New Language(CultureInfo.InvariantCulture)
    Property Title As String = ""
    Property DAR As String = ""

    Sub New()
        Name = "MKV (mkvmerge)"
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
        Using form As New MuxerForm(Me)
            Return form.ShowDialog()
        End Using
    End Function

    Public Overrides Sub Init()
        MyBase.Init()

        For Each iDir In {p.TempDir, p.TempDir.Parent}
            For Each iBase In {"small_cover", "cover_land", "small_cover_land"}
                For Each iExt In {".jpg", ".png"}
                    Dim fp = iDir + iBase + iExt

                    If File.Exists(fp) Then
                        AdditionalSwitches += " --attach-file " + fp.Escape
                    End If
                Next
            Next
        Next

        AdditionalSwitches = AdditionalSwitches?.Trim
    End Sub

    Overrides Sub Mux()
        WriteTagfile()

        Using proc As New Proc
            proc.Header = "Muxing"
            proc.SkipStrings = {"Progress: ", "+-> Pre-parsing"}
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.mkvmerge
            proc.Arguments = GetArgs()
            proc.AllowedExitCodes = {0, 1}
            proc.Start()
        End Using

        If Not g.FileExists(p.TargetFile) Then
            Log.Write("Error MKV output file is missing", p.TargetFile)
        End If

        Log.WriteLine(MediaInfo.GetSummary(p.TargetFile))
    End Sub

    Sub WriteTagfile()
        If Tags.Count = 0 Then Exit Sub

        Dim xml = <Tags>
                      <%= From tag In Tags Select
                        <Tag>
                            <Simple>
                                <Name><%= Macro.Expand(tag.Name) %></Name>
                                <String><%= Macro.Expand(tag.Value) %></String>
                            </Simple>
                        </Tag>
                      %>
                  </Tags>

        Dim filepath = p.TempDir + p.TargetFile.Base + "_tags.xml"
        xml.Save(filepath)
        TagFile = filepath
    End Sub

    Overrides Function GetCommandLine() As String
        Return Package.mkvmerge.Path.Escape + " " + GetArgs()
    End Function

    Function Convert(val As String) As String
        Return CommandLineHelp.ConvertText(val)
    End Function

    Function GetArgs() As String
        Dim args = "-o " + p.TargetFile.LongPathPrefix.Escape

        Dim stdout = ProcessHelp.GetConsoleOutput(Package.mkvmerge.Path, "--identify " +
            p.VideoEncoder.OutputPath.LongPathPrefix.Escape)

        Dim id = Regex.Match(stdout, "Track ID (\d+): video").Groups(1).Value.ToInt

        If Not FileTypes.VideoOnly.Contains(p.VideoEncoder.OutputPath.Ext) Then
            args += " --no-audio --no-subs --no-chapters --no-attachments --no-track-tags --no-global-tags"
        End If

        If VideoTrackLanguage.ThreeLetterCode <> "und" Then
            args += " --language " & id & ":" + VideoTrackLanguage.ThreeLetterCode
        End If

        If VideoTrackName <> "" Then
            args += " --track-name """ & id & ":" + Convert(VideoTrackName) + """"
        End If

        Dim dar = Me.DAR.TrimEx

        If dar.EqualsAny("auto", "0") Then
            dar = Calc.GetTargetDAR.ToInvariantString.Shorten(11)
        End If

        If dar <> "" Then
            args += " --aspect-ratio " & id & ":" + dar.TrimEx.Replace(",", ".").Replace(":", "/")
        ElseIf Calc.IsARSignalingRequired AndAlso TypeOf p.VideoEncoder Is NullEncoder Then
            args += " --aspect-ratio " & id & ":" + Calc.GetTargetDAR.ToInvariantString.Shorten(11)
        End If

        If MediaInfo.GetFrameRate(p.VideoEncoder.OutputPath, 0) = 0 Then
            args += " --default-duration 0:" + p.Script.GetFramerate.ToString("f6", CultureInfo.InvariantCulture) + "fps"
        End If

        If TimestampsFile <> "" Then
            If TimestampsFile.Ext = "txt" Then
                args += " --timestamps 0:" + TimestampsFile.Escape
            ElseIf TimestampsFile.Ext = "mkv" Then
                args += " --timestamps " + MediaInfo.GetVideo(TimestampsFile, "StreamOrder") + ":" + TimestampsFile.Escape
            End If
        End If

        args += " " + p.VideoEncoder.OutputPath.LongPathPrefix.Escape

        For x = 2 To 99
            Dim fp = p.VideoEncoder.OutputPath.DirAndBase + "_chunk" & x & p.VideoEncoder.OutputExtFull

            If fp.FileExists Then
                args += " + " + fp.LongPathPrefix.Escape
            Else
                Exit For
            End If
        Next

        AddAudioArgs(p.Audio0, args)
        AddAudioArgs(p.Audio1, args)

        For Each i In p.AudioTracks
            AddAudioArgs(i, args)
        Next

        ExpandMacros()

        For Each subtitle In Subtitles
            If subtitle.Enabled AndAlso File.Exists(subtitle.Path) Then
                id = If(FileTypes.SubtitleSingle.Contains(subtitle.Path.Ext), 0, subtitle.StreamOrder)

                If Not FileTypes.SubtitleExludingContainers.Contains(subtitle.Path.Ext) Then
                    args += " --no-audio --no-video --no-chapters --no-attachments --no-track-tags --no-global-tags"
                End If

                If Not FileTypes.SubtitleSingle.Contains(subtitle.Path.Ext) Then
                    args += " --subtitle-tracks " & id
                End If

                Dim isContainer = FileTypes.VideoAudio.Contains(subtitle.Path.Ext)

                args += " --forced-track " & id & ":" & If(subtitle.Forced, 1, 0)
                args += " --default-track " & id & ":" & If(subtitle.Default, 1, 0)
                args += " --language " & id & ":" + subtitle.Language.ThreeLetterCode

                If isContainer OrElse subtitle.Title <> "" Then
                    args += " --track-name """ & id & ":" + Convert(subtitle.Title) + """"
                End If

                args += " " + subtitle.Path.LongPathPrefix.Escape
            End If
        Next

        If Not TypeOf Me Is WebMMuxer AndAlso File.Exists(ChapterFile) Then
            If p.Ranges.Count > 0 AndAlso ChapterFile.Ext = "xml" Then
                Dim xDoc = XDocument.Load(ChapterFile)
                Dim lstTimeRanges As New List(Of (StartTime As TimeSpan, EndTime As TimeSpan, Offset As TimeSpan))
                Dim offsetRecord As TimeSpan
                Dim lstValidChapterAtoms As New List(Of XElement)

                For Each r In p.Ranges
                    lstTimeRanges.Add((
                        New TimeSpan(CLng(10000000L * r.Start / p.CutFrameRate)),
                        New TimeSpan(CLng(10000000L * r.End / p.CutFrameRate)),
                        offsetRecord
                    ))

                    offsetRecord += lstTimeRanges.Last.EndTime - lstTimeRanges.Last.StartTime

                    For Each elAtom In xDoc.Descendants("ChapterAtom")
                        Dim elChapterTimeStart = elAtom.Element("ChapterTimeStart")

                        If Not lstValidChapterAtoms.Contains(elAtom) Then
                            Dim tsStart = TimeSpan.Parse(Left(elAtom.Element("ChapterTimeStart").Value, 16))

                            If tsStart >= lstTimeRanges.Last.StartTime And tsStart <= lstTimeRanges.Last.EndTime Then
                                elAtom.Element("ChapterTimeStart").Value = (lstTimeRanges.Last.Offset + (tsStart - lstTimeRanges.Last.StartTime)).ToString()
                                lstValidChapterAtoms.Add(elAtom)
                            End If
                        End If
                    Next
                Next

                xDoc.Descendants("ChapterAtom").Where(Function(Atom) Not lstValidChapterAtoms.Contains(Atom)).Remove()

                If xDoc.Descendants("ChapterAtom").Count > 0 Then
                    Dim CutChapterFile = Path.Combine(Path.GetDirectoryName(ChapterFile), "cut_cpt" + ".xml")
                    xDoc.Save(CutChapterFile)
                    args += " --chapters " + CutChapterFile.Escape
                End If
            Else
                args += " --chapters " + ChapterFile.Escape
            End If
        End If

        If CoverFile <> "" AndAlso File.Exists(CoverFile) Then
            args += " --attach-file " + CoverFile.Escape
        End If

        If Title <> "" Then
            args += " --title """ + Convert(Title) + """"
        End If

        If TypeOf p.VideoEncoder Is NullEncoder AndAlso p.Ranges.Count > 0 Then
            args += " --split parts-frames:" + p.Ranges.Select(Function(v) v.Start & "-" & v.End).Join(",+")
        End If

        If File.Exists(TagFile) Then
            args += " --global-tags " + TagFile.Escape
        End If

        args += " --ui-language en"

        If AdditionalSwitches <> "" Then
            args += " " + Macro.Expand(AdditionalSwitches)
        End If

        For Each i In Attachments
            Dim name = Path.GetFileName(i)

            If i.Contains("_attachment_") Then
                name = i.Right("_attachment_")
            End If

            args += $" --attachment-name {name.Escape} --attach-file {i.LongPathPrefix.Escape}"
        Next

        Return args
    End Function

    Sub AddAudioArgs(ap As AudioProfile, ByRef args As String)
        If File.Exists(ap.File) AndAlso IsSupported(ap.File.Ext) AndAlso IsSupported(ap.OutputFileType) Then
            Dim tid = 0
            Dim isCombo As Boolean

            If Not ap.Stream Is Nothing Then
                tid = ap.Stream.StreamOrder
                isCombo = ap.Stream.Name.Contains("THD+AC3")

                Dim stdout = ProcessHelp.GetConsoleOutput(Package.mkvmerge.Path, "--identify " + ap.File.Escape)
                Dim values = Regex.Matches(stdout, "Track ID (\d+): audio").OfType(Of Match).Select(Function(match) match.Groups(1).Value.ToInt)

                If values.Count = ap.Streams.Count Then
                    tid = values(ap.Stream.Index)
                End If
            Else
                tid = MediaInfo.GetAudio(ap.File, "StreamOrder").ToInt
                isCombo = ap.File.Ext = "thd+ac3"
            End If

            Dim isAudioType = FileTypes.Audio.Contains(ap.File.Ext)

            If Not isAudioType Then
                args += " --no-video --no-subs --no-chapters --no-attachments --no-track-tags --no-global-tags"
            ElseIf ap.File.Ext = "m4a" Then
                args += " --no-chapters" 'eac3to writes chapters to m4a
            End If

            args += " --audio-tracks " + If(isCombo, tid & "," & tid + 1, tid.ToString)
            args += " --language " & tid & ":" + ap.Language.ThreeLetterCode

            If isCombo Then
                args += " --language " & tid + 1 & ":" + ap.Language.ThreeLetterCode
            End If

            If ap.OutputFileType = "aac" AndAlso ap.File.ToLower.Contains("sbr") Then
                args += " --aac-is-sbr " & tid
            End If

            If TypeOf ap Is MuxAudioProfile AndAlso ap.DisplayName.ContainsAny("DTSMA", "DTSX", "DTSHRA") Then
                Dim map = DirectCast(ap, MuxAudioProfile)

                If map.ExtractDTSCore Then
                    args += " --reduce-to-core " & tid
                End If
            End If

            If Not (isAudioType AndAlso ap.StreamName = "") Then
                args += " --track-name """ & tid & ":" + Convert(ap.ExpandMacros(ap.StreamName, False)) + """"
            End If

            If isCombo Then
                args += " --track-name """ & tid + 1 & ":" + Convert(ap.ExpandMacros(ap.StreamName, False)) + """"
            End If

            If ap.Delay <> 0 AndAlso Not ap.HandlesDelay AndAlso Not (ap.HasStream AndAlso ap.Stream.Delay <> 0) Then
                args += " --sync " & tid & ":" + ap.Delay.ToString

                If isCombo Then
                    args += " --sync " & tid + 1 & ":" + ap.Delay.ToString
                End If
            End If

            args += " --default-track " & tid & ":" & If(ap.Default, 1, 0)
            args += " --forced-track " & tid & ":" & If(ap.Forced, 1, 0)
            args += " " + ap.File.LongPathPrefix.Escape
        End If
    End Sub

    Overrides ReadOnly Property SupportedInputTypes() As String()
        Get
            Return {"avi", "wav", "ivf",
                    "mp4", "m4v", "m4a", "aac",
                    "flv", "mov",
                    "264", "h264", "avc",
                    "265", "h265", "hevc", "hvc",
                    "ac3", "ec3", "eac3", "thd+ac3", "thd",
                    "mkv", "mka", "webm",
                    "mp2", "mpa", "mp3",
                    "ogg", "ogm",
                    "dts", "dtsma", "dtshr", "dtshd",
                    "mpg", "m2v", "mpv", "vob", "mpeg",
                    "ts", "m2ts",
                    "opus", "flac"}
        End Get
    End Property
End Class

<Serializable()>
Public Class WebMMuxer
    Inherits MkvMuxer

    Sub New()
        MyBase.New("WebM (mkvmerge)")
    End Sub

    Overrides ReadOnly Property SupportedInputTypes() As String()
        Get
            Return {"mkv", "webm", "mka", "ogg", "opus", "ivf"}
        End Get
    End Property

    Overrides ReadOnly Property OutputExt As String
        Get
            Return "webm"
        End Get
    End Property
End Class

<Serializable()>
Public Class ffmpegMuxer
    Inherits Muxer

    Property OutputFormat As String = "AVI"

    Sub New(name As String)
        MyBase.New(name)
    End Sub

    ReadOnly Property AVITypes As String()
        Get
            Return {"avi", "mp2", "mp3", "ac3", "mpa", "wav"}
        End Get
    End Property

    Shared ReadOnly Property SupportedFormats As String()
        Get
            Return {"ASF", "AVI", "FLV", "ISMV", "IVF", "MKV", "MOV",
                    "MP4", "MPG", "MXF", "NUT", "OGG", "TS", "WEBM", "WMV"}
        End Get
    End Property

    Public Overrides ReadOnly Property OutputExt As String
        Get
            Return OutputFormat.ToLower
        End Get
    End Property

    Public Overrides ReadOnly Property SupportedInputTypes As String()
        Get
            If OutputExt = "avi" Then
                Return AVITypes
            End If

            Return MyBase.SupportedInputTypes
        End Get
    End Property

    Overrides Function IsSupported(type As String) As Boolean
        If OutputExt = "avi" Then
            Return AVITypes.Contains(type)
        End If

        Return True
    End Function

    Overrides Sub Mux()
        Dim args = "-i "

        If File.Exists(p.VideoEncoder.OutputPath) Then
            args += p.VideoEncoder.OutputPath.Escape
        Else
            args += p.LastOriginalSourceFile.Escape
        End If

        Dim id As Integer
        Dim mapping = " -map 0:v"

        For Each track In {p.Audio0, p.Audio1}
            If Not TypeOf track Is NullAudioProfile AndAlso File.Exists(track.File) AndAlso
                IsSupported(track.OutputFileType) Then

                id += 1
                args += " -i " + track.File.Escape
                mapping += " -map " & id

                If Not track.Stream Is Nothing Then
                    mapping += ":" & track.Stream.StreamOrder
                End If
            End If
        Next

        args += mapping + " -c:v copy -c:a copy -y -hide_banner -strict -2 " + p.TargetFile.Escape

        Using proc As New Proc
            proc.Header = "Muxing to " + OutputFormat
            proc.SkipStrings = {"frame=", "size="}
            proc.Encoding = Encoding.UTF8
            proc.Package = Package.ffmpeg
            proc.Arguments = args
            proc.Start()
        End Using
    End Sub

    Overrides Function Edit() As DialogResult
        Using form As New SimpleSettingsForm("ffmpeg Container Options")
            form.ScaleClientSize(25, 10)
            Dim ui = form.SimpleUI
            ui.Store = Me
            ui.CreateFlowPage()

            Dim m = ui.AddMenu(Of String)
            m.Text = "Output Format:"
            m.Property = NameOf(OutputFormat)
            m.Add(SupportedFormats)

            Dim ret = form.ShowDialog()

            If ret = DialogResult.OK Then
                ui.Save()

                If p.SourceFile <> "" Then
                    p.TargetFile = p.TargetFile.DirAndBase + "." + OutputExt
                End If
            End If

            Return ret
        End Using
    End Function
End Class
