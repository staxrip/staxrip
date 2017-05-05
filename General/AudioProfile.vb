Imports StaxRip.UI

Imports System.Globalization

Imports VB6 = Microsoft.VisualBasic
Imports System.Runtime.Serialization
Imports System.Text.RegularExpressions
Imports System.Text

<Serializable()>
Public MustInherit Class AudioProfile
    Inherits Profile

    Property Language As New Language
    Property Delay As Integer
    Property StreamName As String = ""
    Property Gain As Single
    Property Streams As List(Of AudioStream) = New List(Of AudioStream)
    Property [Default] As Boolean

    Overridable Property Channels As Integer = 2
    Overridable Property OutputFileType As String = "unknown"
    Overridable Property Bitrate As Double
    Overridable Property SupportedInput As String()

    Overridable Property CommandLines As String

    Sub New(name As String)
        MyBase.New(name)
    End Sub

    Sub New(name As String,
            bitrate As Integer,
            input As String(),
            fileType As String,
            channels As Integer)

        MyBase.New(name)

        Me.Channels = channels
        Me.Bitrate = bitrate
        SupportedInput = input
        OutputFileType = fileType
    End Sub

    Private FileValue As String = ""

    Property File As String
        Get
            Return FileValue
        End Get
        Set(value As String)
            If FileValue <> value Then
                FileValue = value
                Stream = Nothing
                OnFileChanged()
            End If
        End Set
    End Property

    Private StreamValue As AudioStream

    Property Stream As AudioStream
        Get
            Return StreamValue
        End Get
        Set(value As AudioStream)
            If Not value Is StreamValue Then
                StreamValue = value

                If Not Stream Is Nothing Then
                    If Not p.Script.GetFilter("Source").Script.Contains("DirectShowSource") Then
                        Delay = Stream.Delay
                    End If

                    Language = Stream.Language
                    StreamName = Stream.Title
                End If

                OnStreamChanged()
            End If
        End Set
    End Property

    Property DisplayName As String
        Get
            Dim ret = ""

            If Stream Is Nothing Then
                Dim streams = MediaInfo.GetAudioStreams(File)

                If streams.Count > 0 Then
                    ret = GetAudioText(streams(0), File)
                Else
                    ret = File.FileName
                End If
            Else
                ret = Stream.Name + " (" + Filepath.GetExt(File) + ")"
            End If

            Return ret
        End Get
        Set(value As String)
        End Set
    End Property

    Overridable Sub OnFileChanged()
    End Sub

    Overridable Sub OnStreamChanged()
    End Sub

    Function GetAudioText(stream As AudioStream, path As String) As String
        For Each i In Language.Languages
            If path.Contains(i.CultureInfo.EnglishName) Then
                stream.Language = i
                Exit For
            End If
        Next

        Dim matchDelay = Regex.Match(path, " (-?\d+)ms")
        If matchDelay.Success Then stream.Delay = matchDelay.Groups(1).Value.ToInt

        Dim matchID = Regex.Match(path, " ID(\d+)")
        Dim name As String

        If matchID.Success Then
            stream.StreamOrder = matchID.Groups(1).Value.ToInt - 1
            name = stream.Name
        Else
            name = stream.Name.Substring(4)
        End If

        If Filepath.GetBase(File) = Filepath.GetBase(p.SourceFile) Then
            Return name + " (" + Filepath.GetExt(File) + ")"
        Else
            Return name + " (" + Filepath.GetName(File) + ")"
        End If
    End Function

    Sub SetStreamOrLanguage()
        If File = "" Then Exit Sub

        If File <> p.LastOriginalSourceFile Then
            For Each i In Language.Languages
                If File.Contains(i.CultureInfo.EnglishName) Then
                    Language = i
                    Exit Sub
                End If
            Next
        Else
            For Each i In Streams
                If i.Language.Equals(Language) Then
                    Stream = i
                    Exit For
                End If
            Next

            If Stream Is Nothing AndAlso Streams.Count > 0 Then
                Stream = Streams(0)
            End If
        End If
    End Sub

    Function IsInputSupported() As Boolean
        Return SupportedInput.NothingOrEmpty OrElse SupportedInput.Contains(File.Ext)
    End Function

    Function IsMuxProfile() As Boolean
        Return TypeOf Me Is MuxAudioProfile
    End Function

    ReadOnly Property HasStream As Boolean
        Get
            Return Stream IsNot Nothing
        End Get
    End Property

    Overridable Sub Encode()
    End Sub

    Overridable Sub EditProject()
    End Sub

    Overridable Function HandlesDelay() As Boolean
    End Function

    Function GetOutputFile() As String
        Dim base = Filepath.GetBase(File)

        If Delay <> 0 Then
            If HandlesDelay() Then
                If base.Contains("ms") Then
                    Dim re As New Regex(" (-?\d+)ms")
                    If re.IsMatch(base) Then base = re.Replace(base, "")
                End If
            Else
                If Not base.Contains("ms") Then base += " " & Delay & "ms"
            End If
        End If

        Dim targetDir = If(p.TempDir <> "", p.TempDir, Filepath.GetDir(File))
        Dim track As String

        If Me Is p.Audio0 Then track = "1"
        If Me Is p.Audio1 Then track = "2"

        Return targetDir + base + "_out" + track + "." + OutputFileType
    End Function

    Function SolveMacros(value As String) As String
        Return SolveMacros(value, True)
    End Function

    Function SolveMacros(value As String, silent As Boolean) As String
        If value = "" Then Return ""
        If value.Contains("""%input%""") Then value = value.Replace("""%input%""", File.Quotes)
        If value.Contains("%input%") Then value = value.Replace("%input%", File.Quotes)
        If value.Contains("""%output%""") Then value = value.Replace("""%output%""", GetOutputFile.Quotes)
        If value.Contains("%output%") Then value = value.Replace("%output%", GetOutputFile.Quotes)
        If value.Contains("%bitrate%") Then value = value.Replace("%bitrate%", Bitrate.ToString)
        If value.Contains("%channels%") Then value = value.Replace("%channels%", Channels.ToString)
        If value.Contains("%language_native%") Then value = value.Replace("%language_native%", Language.CultureInfo.NativeName)
        If value.Contains("%language_english%") Then value = value.Replace("%language_english%", Language.Name)
        If value.Contains("%delay%") Then value = value.Replace("%delay%", Delay.ToString)
        Return Macro.Expand(value)
    End Function

    Shared Function GetDefaults() As List(Of AudioProfile)
        Dim ret As New List(Of AudioProfile)
        ret.Add(New GUIAudioProfile(AudioCodec.AAC, 0.35))
        ret.Add(New GUIAudioProfile(AudioCodec.Opus, 1) With {.Bitrate = 80})
        ret.Add(New GUIAudioProfile(AudioCodec.Flac, 0.3))
        ret.Add(New GUIAudioProfile(AudioCodec.Vorbis, 1))
        ret.Add(New GUIAudioProfile(AudioCodec.MP3, 4))
        ret.Add(New GUIAudioProfile(AudioCodec.AC3, 1.0) With {.Channels = 6, .Bitrate = 448})
        ret.Add(New BatchAudioProfile(320, {}, "mp3", 2, "ffmpeg -i %input% -b:a %bitrate%k -hide_banner -y %output%"))
        ret.Add(New MuxAudioProfile())
        ret.Add(New NullAudioProfile())
        Return ret
    End Function
End Class

<Serializable()>
Public Class BatchAudioProfile
    Inherits AudioProfile

    Sub New(bitrate As Integer,
            input As String(),
            fileType As String,
            channels As Integer,
            batchCode As String)

        MyBase.New(Nothing, bitrate, input, fileType, channels)
        Me.CommandLines = batchCode
        CanEditValue = True
    End Sub

    Overrides Function Edit() As DialogResult
        Using f As New BatchAudioEncoderForm(Me)
            f.mbLanguage.Enabled = False
            f.lLanguage.Enabled = False
            f.tbDelay.Enabled = False
            f.lDelay.Enabled = False
            Return f.ShowDialog()
        End Using
    End Function

    Public Overrides ReadOnly Property DefaultName As String
        Get
            Dim ch As String

            Select Case Channels
                Case 8
                    ch += "7.1"
                Case 7
                    ch += "6.1"
                Case 6
                    ch += "5.1"
                Case 2
                    ch += "2.0"
                Case 1
                    ch += "Mono"
                Case Else
                    ch += Channels & ".0"
            End Select

            Return "Custom Batch Code | " + OutputFileType.Upper + " " & ch & " " & Bitrate & " Kbps"
        End Get
    End Property

    Function GetCode() As String
        Dim cl = SolveMacros(CommandLines).Trim

        Return {
            Package.ffmpeg,
            Package.eac3to,
            Package.BeSweet,
            Package.qaac}.
            Where(Function(pack) cl.ToLower.Contains(pack.Name.ToLower)).
            Select(Function(pack) "set PATH=%PATH%;" + pack.GetDir).
            Join(BR) + BR2 + "cd /D " + p.TempDir.Quotes + BR2 + cl
    End Function

    Public Overrides Sub Encode()
        If File <> "" Then
            Dim bitrateBefore = p.VideoBitrate
            Dim targetPath = GetOutputFile()

            Dim batchCode = GetCode()
            Dim batchPath = p.TempDir + File.Base + "_audio.bat"
            batchCode = Proc.WriteBatchFile(batchPath, batchCode)

            Using proc As New Proc
                proc.Init("Audio encoding: " + Name)
                proc.SkipStrings = {"Maximum Gain Found", "transcoding ...", "size=", "process: ", "analyze: "}
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

            If g.WasFileJustWritten(targetPath) Then
                File = targetPath
                Bitrate = Calc.GetBitrateFromFile(File, p.TargetSeconds)
                p.VideoBitrate = CInt(Calc.GetVideoBitrate)

                If Not p.VideoEncoder.QualityMode Then
                    Log.WriteLine("Video Bitrate: " + bitrateBefore.ToString() + " -> " & p.VideoBitrate & BR)
                End If

                Log.WriteLine(MediaInfo.GetSummary(File))
            Else
                Log.Write("Error", "no output found")

                If Not Filepath.GetExtFull(File) = ".wav" Then
                    Audio.Decode(Me)

                    If Filepath.GetExtFull(File) = ".wav" Then
                        Encode()
                    End If
                End If
            End If
        End If
    End Sub

    Overrides Sub EditProject()
        Using f As New BatchAudioEncoderForm(Me)
            f.ShowDialog()
        End Using
    End Sub

    Overrides Function HandlesDelay() As Boolean
        Return CommandLines.Contains("%delay%")
    End Function
End Class

<Serializable()>
Class NullAudioProfile
    Inherits AudioProfile

    Sub New()
        MyBase.New("No Audio", 0, {}, "ignore", 0)
    End Sub

    Overrides Function HandlesDelay() As Boolean
    End Function

    Overrides Sub EditProject()
        Using f As New SimpleSettingsForm("Null Audio Profile Options")
            f.Width = CInt(f.Width * 0.6)
            f.Height = CInt(f.Height * 0.4)

            Dim ui = f.SimpleUI
            Dim page = ui.CreateFlowPage("main page")

            Dim nb = ui.AddNumericBlock(page)
            nb.Label.Text = "Reserved Bitrate:"
            nb.NumEdit.Init(0, 1000000, 8)
            nb.NumEdit.Value = CDec(Bitrate)
            nb.NumEdit.SaveAction = Sub(value) Bitrate = CDec(value)

            If f.ShowDialog() = DialogResult.OK Then ui.Save()
        End Using
    End Sub

    Public Overrides Property OutputFileType As String
        Get
            Return "ignore"
        End Get
        Set(value As String)
        End Set
    End Property

    Public Overrides Sub Encode()
    End Sub
End Class

<Serializable()>
Class MuxAudioProfile
    Inherits AudioProfile

    Sub New()
        MyBase.New("Just Mux", 0, Nothing, "ignore", 2)
        CanEditValue = True
    End Sub

    Public Overrides Property OutputFileType As String
        Get
            If Stream Is Nothing Then
                Return File.Ext
            Else
                Return Stream.Extension.TrimStart("."c)
            End If
        End Get
        Set(value As String)
        End Set
    End Property

    Overrides Property SupportedInput As String()
        Get
            Return {}
        End Get
        Set(value As String())
        End Set
    End Property

    Overrides Function Edit() As DialogResult
        Return Edit(False)
    End Function

    Overrides Sub EditProject()
        Edit(True)
    End Sub

    Overrides Sub Encode()
    End Sub

    Overrides Sub OnFileChanged()
        MyBase.OnFileChanged()
        SetBitrate()
    End Sub

    Overrides Sub OnStreamChanged()
        MyBase.OnStreamChanged()
        SetBitrate()
    End Sub

    Sub SetBitrate()
        If Stream Is Nothing Then
            Bitrate = Calc.GetBitrateFromFile(File, p.SourceSeconds)
        Else
            Bitrate = Stream.Bitrate + Stream.BitrateCore
        End If
    End Sub

    Private Overloads Function Edit(showProjectSettings As Boolean) As DialogResult
        Using f As New SimpleSettingsForm("Audio Mux Options", "The Audio Mux options allow to add a audio file without reencoding.")
            f.Height = CInt(f.Height * 0.6)
            Dim ui = f.SimpleUI
            Dim page = ui.CreateFlowPage("main page")
            page.SuspendLayout()

            Dim tbb = ui.AddTextButtonBlock(page)
            tbb.Label.Text = "Stream Name:"
            tbb.Label.Tooltip = "Stream name used by the muxer. The stream name may contain macros."
            tbb.Edit.Expandet = True
            tbb.Edit.Text = StreamName
            tbb.Edit.SaveAction = Sub(value) StreamName = value
            tbb.Button.Text = "Macro String Editor..."
            tbb.Button.ClickAction = AddressOf tbb.Edit.EditMacro

            Dim nb = ui.AddNumericBlock(page)
            nb.Label.Text = "Delay:"
            nb.Label.Tooltip = "Delay used by the muxer."
            nb.NumEdit.Init(Integer.MinValue, Integer.MaxValue, 1)
            nb.NumEdit.Value = Delay
            nb.NumEdit.SaveAction = Sub(value) Delay = CInt(value)

            Dim mbi = ui.AddMenuButtonBlock(Of Language)(page)
            mbi.Label.Text = "Language:"
            mbi.Label.Tooltip = "Language of the audio track."
            mbi.MenuButton.Value = Language
            mbi.MenuButton.SaveAction = Sub(value) Language = value

            For Each i In Language.Languages
                If i.IsCommon Then
                    mbi.MenuButton.Add(i.ToString, i)
                Else
                    mbi.MenuButton.Add("More | " + i.ToString.Substring(0, 1).ToUpper + " | " + i.ToString, i)
                End If
            Next

            Dim cb = ui.AddCheckBox(page)
            cb.Text = "Default Stream"
            cb.Tooltip = "Make this stream default in MKV container."
            cb.Checked = [Default]
            cb.SaveAction = Sub(value) [Default] = value

            page.ResumeLayout()

            Dim ret = f.ShowDialog()
            If ret = DialogResult.OK Then ui.Save()

            Return ret
        End Using
    End Function
End Class

<Serializable()>
Class GUIAudioProfile
    Inherits AudioProfile

    Property Params As New Parameters

    Sub New(codec As AudioCodec, quality As Single)
        MyBase.New(Nothing)

        Params.Codec = codec
        Params.Quality = quality

        Select Case codec
            Case AudioCodec.DTS, AudioCodec.Flac, AudioCodec.WAV, AudioCodec.AC3
                Params.RateMode = AudioRateMode.CBR
            Case Else
                Params.RateMode = AudioRateMode.VBR
        End Select

        Bitrate = GetBitrate()
    End Sub

    Public Overrides Property Channels As Integer
        Get
            Select Case Params.ChannelsMode
                Case ChannelsMode.Original
                    If Not Stream Is Nothing Then
                        Return Stream.Channels
                    ElseIf File <> "" AndAlso IO.File.Exists(File) Then
                        Return MediaInfo.GetChannels(File)
                    End If
                Case ChannelsMode._1
                    Return 1
                Case ChannelsMode._6
                    Return 6
                Case ChannelsMode._7
                    Return 7
                Case ChannelsMode._8
                    Return 8
            End Select

            Return 2
        End Get
        Set(value As Integer)
        End Set
    End Property

    Private SourceBitDepthValue As Integer

    ReadOnly Property SourceBitDepth As Integer 'can be 0
        Get
            If SourceBitDepthValue = 0 Then
                If Stream Is Nothing Then
                    If File <> "" AndAlso IO.File.Exists(File) Then
                        SourceBitDepthValue = MediaInfo.GetAudio(File, "BitDepth").ToInt
                    End If
                Else
                    SourceBitDepthValue = Stream.BitDepth
                End If
            End If

            Return SourceBitDepthValue
        End Get
    End Property

    ReadOnly Property OutputBitDepth As Integer 'can be 0
        Get
            If Params.Down16 AndAlso SourceBitDepth > 16 Then
                Return 16
            Else
                Return SourceBitDepthValue
            End If
        End Get
    End Property

    Private SourceSamplingRateValue As Integer

    ReadOnly Property SourceSamplingRate As Integer 'can be 0
        Get
            If SourceSamplingRateValue = 0 Then
                If Stream Is Nothing Then
                    If File <> "" AndAlso IO.File.Exists(File) Then
                        SourceSamplingRateValue = MediaInfo.GetAudio(File, "SamplingRate").ToInt
                    End If
                Else
                    SourceSamplingRateValue = Stream.SamplingRate
                End If
            End If

            Return SourceSamplingRateValue
        End Get
    End Property

    ReadOnly Property TargetSamplingRate As Integer 'can be 0
        Get
            If Params.SamplingRate <> 0 Then
                Return Params.SamplingRate
            Else
                Return SourceSamplingRate
            End If
        End Get
    End Property

    Function GetBitrate() As Integer
        If Params.RateMode = AudioRateMode.VBR Then
            Select Case Params.Codec
                Case AudioCodec.AAC
                    If Channels >= 6 Then
                        Select Case Params.Encoder
                            Case GuiAudioEncoder.qaac
                                Return Calc.GetYFromTwoPointForm(0, 50, 127, 500, Params.Quality)
                            Case Else
                                Return Calc.GetYFromTwoPointForm(0.1, 25, 1, 1000, Params.Quality)
                        End Select
                    Else
                        Select Case Params.Encoder
                            Case GuiAudioEncoder.qaac
                                Return Calc.GetYFromTwoPointForm(0, 25, 127, 200, Params.Quality)
                            Case Else
                                Return Calc.GetYFromTwoPointForm(0.1, 25, 1, 350, Params.Quality)
                        End Select
                    End If
                Case AudioCodec.MP3
                    Return Calc.GetYFromTwoPointForm(9, 65, 0, 245, Params.Quality)
                Case AudioCodec.Vorbis
                    If Channels >= 6 Then
                        Return Calc.GetYFromTwoPointForm(0, 120, 10, 1440, Params.Quality)
                    Else
                        Return Calc.GetYFromTwoPointForm(0, 64, 10, 500, Params.Quality)
                    End If
                Case AudioCodec.Opus
                    Return CInt(Bitrate)
            End Select
        Else
            Dim bitDepth = If(OutputBitDepth > 0, OutputBitDepth, If(Params.Down16, 16, 24))

            Select Case Params.Codec
                Case AudioCodec.Flac
                    Return CInt(((If(TargetSamplingRate = 0, 48000, TargetSamplingRate) * bitDepth * Channels) / 1000) * 0.55)
                Case AudioCodec.WAV
                    Return CInt((If(TargetSamplingRate = 0, 48000, TargetSamplingRate) * bitDepth * Channels) / 1000)
            End Select
        End If

        Return CInt(Bitrate)
    End Function

    Public Overrides Sub Encode()
        If File <> "" Then
            Dim bitrateBefore = p.VideoBitrate
            Dim targetPath = GetOutputFile()

            For Each i In SolveMacros(CommandLines, False).SplitLinesNoEmpty
                Dim start = DateTime.Now

                Using proc As New Proc
                    If i.Contains("BeSweet.exe") Then
                        proc.Init("Audio encoding using BeSweet " + Package.BeSweet.Version, "Processed", "transcoding", "Maximum Gain Found : ", "Asserting gain")
                    ElseIf i.Contains("eac3to.exe") Then
                        proc.Init("Audio encoding using eac3to " + Package.eac3to.Version, "process: ", "analyze: ")
                        proc.TrimChars = {"-"c, " "c}
                        proc.RemoveChars = {VB6.ChrW(8)} 'backspace
                    ElseIf i.Contains("ffmpeg.exe") Then
                        proc.Init("Audio encoding using ffmpeg " + Package.ffmpeg.Version, "size=", "decoding is not implemented", "unsupported frame type", "upload a sample")
                        proc.Encoding = Encoding.UTF8
                    ElseIf i.Contains("qaac64.exe") Then
                        proc.Init("Audio encoding using qaac " + Package.qaac.Version, ", ETA ")
                    End If

                    proc.CommandLine = i
                    proc.AllowedExitCodes = {0, 1}
                    proc.Start()
                End Using

                If g.WasFileJustWritten(targetPath) Then
                    File = targetPath
                    Bitrate = Calc.GetBitrateFromFile(File, p.TargetSeconds)
                    p.VideoBitrate = CInt(Calc.GetVideoBitrate)

                    If Not p.VideoEncoder.QualityMode Then
                        Log.WriteLine("Video Bitrate: " + bitrateBefore.ToString() + " -> " & p.VideoBitrate & BR)
                    End If

                    Log.WriteLine(MediaInfo.GetSummary(File))
                Else
                    Log.Write("Error", "no output found")

                    If Not Filepath.GetExtFull(File) = ".wav" Then
                        Audio.Decode(Me)

                        If Filepath.GetExtFull(File) = ".wav" Then
                            Encode()
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Overrides Function Edit() As DialogResult
        Using f As New AudioForm()
            f.LoadProfile(Me)
            f.mbLanguage.Enabled = False
            f.numDelay.Enabled = False
            f.numGain.Enabled = False
            Return f.ShowDialog()
        End Using
    End Function

    Overrides Sub EditProject()
        Using f As New AudioForm()
            f.LoadProfile(Me)
            f.ShowDialog()
        End Using
    End Sub

    Public Overrides Property OutputFileType As String
        Get
            Select Case Params.Codec
                Case AudioCodec.AAC
                    Return "m4a"
                Case AudioCodec.Vorbis
                    Return "ogg"
                Case Else
                    Return Params.Codec.ToString.ToLower
            End Select
        End Get
        Set(value As String)
        End Set
    End Property

    Function GetEac3toCommandLine(includePaths As Boolean) As String
        Dim ret, id As String

        If File.Ext.EqualsAny("ts", "m2ts", "mkv") AndAlso Not Stream Is Nothing Then
            id = (Stream.StreamOrder + 1) & ": "
        End If

        If includePaths Then
            ret = Package.eac3to.Path.Quotes + " " + id + File.Quotes + " " + GetOutputFile.Quotes
        Else
            ret = "eac3to"
        End If

        If Not (Params.Codec = AudioCodec.DTS AndAlso Params.eac3toExtractDtsCore) Then
            Select Case Params.Codec
                Case AudioCodec.AAC
                    ret += " -quality=" & Params.Quality.ToInvariantString
                Case AudioCodec.AC3
                    ret += " -" & Bitrate

                    If Not {192, 224, 384, 448, 640}.Contains(CInt(Bitrate)) Then
                        Return "Invalid bitrate, choose 192, 224, 384, 448 or 640"
                    End If
                Case AudioCodec.DTS
                    ret += " -" & Bitrate
            End Select

            If Params.Normalize Then ret += " -normalize"
            If Params.Down16 Then ret += " -down16"
            If Params.SamplingRate <> 0 Then ret += " -resampleTo" & Params.SamplingRate
            If Params.FrameRateMode = AudioFrameRateMode.Speedup Then ret += " -speedup"
            If Params.FrameRateMode = AudioFrameRateMode.Slowdown Then ret += " -slowdown"
            If Delay <> 0 Then ret += " " + If(Delay > 0, "+", "") & Delay & "ms"
            If Gain < 0 Then ret += " " & CInt(Gain) & "dB"
            If Gain > 0 Then ret += " +" & CInt(Gain) & "dB"

            Select Case Channels
                Case 6
                    If Params.ChannelsMode <> ChannelsMode.Original Then ret += " -down6"
                Case 2
                    If Params.eac3toStereoDownmixMode = 0 Then
                        If Params.ChannelsMode <> ChannelsMode.Original Then ret += " -downStereo"
                    Else
                        ret += " -downDpl"
                    End If
            End Select

            If Params.CustomSwitches <> "" Then ret += " " + Params.CustomSwitches
        ElseIf Params.eac3toExtractDtsCore Then
            ret += " -core"
        End If

        If includePaths Then ret += " -progressnumbers"

        Return ret
    End Function

    Function GetqaacCommandLine(includePaths As Boolean) As String
        Dim ret As String
        includePaths = includePaths And File <> ""

        If includePaths Then
            ret = Package.qaac.Path.Quotes + " -o " + GetOutputFile.Quotes
        Else
            ret = "qaac"
        End If

        Select Case Params.qaacRateMode
            Case 0
                ret += " --tvbr " & CInt(Params.Quality)
            Case 1
                ret += " --cvbr " & CInt(Bitrate)
            Case 2
                ret += " --abr " & CInt(Bitrate)
            Case 3
                ret += " --cbr " & CInt(Bitrate)
        End Select

        If Params.qaacHE Then ret += " --he"
        If Delay <> 0 Then ret += " --delay " + (Delay / 1000).ToInvariantString
        If Params.Normalize Then ret += " --normalize"
        If Params.qaacQuality <> 2 Then ret += " --quality " & Params.qaacQuality
        If Params.SamplingRate <> 0 Then ret += " --rate " & Params.SamplingRate
        If Params.qaacLowpass <> 0 Then ret += " --lowpass " & Params.qaacLowpass
        If Params.qaacNoDither Then ret += " --no-dither"
        If Gain <> 0 Then ret += " --gain " & Gain.ToInvariantString
        If Params.CustomSwitches <> "" Then ret += " " + Params.CustomSwitches
        If includePaths Then ret += " " + File.Quotes

        Return ret
    End Function

    Function GetFfmpegCommandLine(includePaths As Boolean) As String
        Dim ret As String

        If includePaths AndAlso File <> "" Then
            ret = Package.ffmpeg.Path.Quotes + " -i " + File.Quotes
        Else
            ret = "ffmpeg"
        End If

        If Not Stream Is Nothing Then ret += " -map 0:" & Stream.StreamOrder

        Select Case Params.Codec
            Case AudioCodec.MP3
                If Not Params.CustomSwitches.Contains("-c:a ") Then ret += " -c:a libmp3lame"

                Select Case Params.RateMode
                    Case AudioRateMode.ABR
                        ret += " -b:a " & CInt(Bitrate) & "k -abr 1"
                    Case AudioRateMode.CBR
                        ret += " -b:a " & CInt(Bitrate) & "k"
                    Case AudioRateMode.VBR
                        ret += " -q:a " & CInt(Params.Quality)
                End Select
            Case AudioCodec.AC3
                If Not {192, 224, 384, 448, 640}.Contains(CInt(Bitrate)) Then
                    Return "Invalid bitrate, choose 192, 224, 384, 448 or 640"
                End If

                ret += " -b:a " & CInt(Bitrate) & "k"
            Case AudioCodec.DTS
                ret += " -strict -2 -b:a " & CInt(Bitrate) & "k"
            Case AudioCodec.Flac, AudioCodec.WAV
            Case AudioCodec.Vorbis
                If Not Params.CustomSwitches.Contains("-c:a ") Then ret += " -c:a libvorbis"

                If Params.RateMode = AudioRateMode.VBR Then
                    ret += " -q:a " & CInt(Params.Quality)
                Else
                    ret += " -b:a " & CInt(Bitrate) & "k"
                End If
            Case AudioCodec.Opus
                If Not Params.CustomSwitches.Contains("-c:a ") Then ret += " -c:a libopus"

                If Params.RateMode = AudioRateMode.VBR Then
                    ret += " -vbr on"
                Else
                    ret += " -vbr off"
                End If

                ret += " -b:a " & CInt(Bitrate) & "k"
            Case AudioCodec.AAC
                If Params.RateMode = AudioRateMode.VBR Then
                    ret += " -q:a " & Calc.GetYFromTwoPointForm(0.1, 1, 1, 10, Params.Quality)
                Else
                    ret += " -b:a " & CInt(Bitrate) & "k"
                End If
        End Select

        If Gain <> 0 Then ret += " -af volume=" + Gain.ToInvariantString + "dB"
        If Params.ChannelsMode <> ChannelsMode.Original Then ret += " -ac " & Channels
        If Params.SamplingRate <> 0 Then ret += " -ar " & Params.SamplingRate
        ret += " -y -hide_banner"
        If Params.CustomSwitches <> "" Then ret += " " + Params.CustomSwitches
        If includePaths AndAlso File <> "" Then ret += " " + GetOutputFile.Quotes

        Return ret
    End Function

    Function GetBeSweetCommandLine(includePaths As Boolean) As String
        Dim ret As String

        If includePaths Then
            ret = Package.BeSweet.Path.Quotes
        Else
            ret = "BeSweet"
        End If

        If includePaths AndAlso File <> "" Then
            ret += " -core( -input " + File.Quotes + " -output " + GetOutputFile.Quotes & " )"
        End If

        Dim t = ""

        If Not Params.BeSweetAzid.Contains("-c") AndAlso Params.BeSweetDynamicCompression <> "None" Then
            t += " -c " + Params.BeSweetDynamicCompression.ToLower
        End If

        If Not Params.BeSweetAzid.Contains("-L") AndAlso Channels = 2 Then
            t += " -L -3db"
        End If

        If Params.BeSweetAzid <> "" Then t += " " + Params.BeSweetAzid
        If t <> "" Then ret += " -azid(" + t + " )"

        Dim ota = If(Delay <> 0, " -d " & Delay, "")

        If Params.Normalize AndAlso Params.BeSweetGainAndNormalization <> "" Then
            ota += " " + Params.BeSweetGainAndNormalization
        End If

        If Params.FrameRateMode = AudioFrameRateMode.Speedup Then
            ota += " -r 23976 25000"
        End If

        If Params.FrameRateMode = AudioFrameRateMode.Slowdown Then
            ota += " -r 25000 23976"
        End If

        If ota <> "" Then ret += " -ota(" & ota & " )"

        If Params.SamplingRate <> 0 Then
            ret += " -ssrc( --rate " & Params.SamplingRate & " )"
        End If

        If Params.CustomSwitches <> "" Then ret += " " + Params.CustomSwitches

        Select Case Params.Codec
            Case AudioCodec.AAC
                Dim ch = If(Channels = 6, " -6chnew", "")
                Dim profile As String = Nothing

                Select Case Params.AacProfile
                    Case AudioAacProfile.LC
                        profile = " -aacprofile_lc"
                    Case AudioAacProfile.SBR
                        profile = " -aacprofile_he"
                    Case AudioAacProfile.SBRPS
                        profile = " -aacprofile_hev2"
                End Select

                Select Case Params.RateMode
                    Case AudioRateMode.VBR
                        ret += " -bsn( -vbr " & Params.Quality.ToInvariantString & profile & ch & " )"
                    Case AudioRateMode.ABR
                        ret += " -bsn( -abr " & CInt(Bitrate) & profile & ch & " )"
                    Case AudioRateMode.CBR
                        ret += " -bsn( -cbr " & CInt(Bitrate) & profile & ch & " )"
                End Select
            Case AudioCodec.MP3
                Select Case Params.RateMode
                    Case AudioRateMode.VBR
                        ret += " -lame( -v --vbr-new -V " & CInt(Params.Quality) & " -b 32 -h )"
                    Case AudioRateMode.ABR
                        ret += " -lame( --abr " & CInt(Bitrate) & " -h )"
                    Case AudioRateMode.CBR
                        ret += " -lame( -b " & CInt(Bitrate) & " -h )"
                End Select
            Case AudioCodec.AC3
                ret += " -bsn( -exe aften.exe -b " & CInt(Bitrate) & If(Channels = 6, " -6chnew", "") + " )"
        End Select

        Return ret
    End Function

    Public Overrides ReadOnly Property DefaultName As String
        Get
            If Params Is Nothing Then Exit Property
            Dim ch As String

            Select Case Params.ChannelsMode
                Case ChannelsMode._8
                    ch += " 7.1"
                Case ChannelsMode._7
                    ch += " 6.1"
                Case ChannelsMode._6
                    ch += " 5.1"
                Case ChannelsMode._2
                    ch += " 2.0"
                Case ChannelsMode._1
                    ch += " Mono"
            End Select

            Dim circa = If(Params.RateMode = AudioRateMode.VBR OrElse Params.Codec = AudioCodec.Flac, "~", "")
            Dim rate = If(Params.RateMode = AudioRateMode.VBR, " " & Params.RateMode.ToString, "")
            Dim co = Params.Codec.ToString

            Select Case Params.Codec
                Case AudioCodec.AAC
                    If Params.AacProfile <> AudioAacProfile.Automatic Then
                        co += "-" + Params.AacProfile.ToString
                    End If
            End Select

            Return co + rate & ch & " " & circa & Bitrate & " Kbps"
        End Get
    End Property

    Overrides Property CommandLines() As String
        Get
            Return GetCommandLine(True)
        End Get
        Set(Value As String)
        End Set
    End Property

    Overrides ReadOnly Property CanEdit() As Boolean
        Get
            Return True
        End Get
    End Property

    Overrides Function HandlesDelay() As Boolean
        Return GetEncoder() <> GuiAudioEncoder.ffmpeg
    End Function

    Function GetEncoder() As GuiAudioEncoder
        Select Case Params.Encoder
            Case GuiAudioEncoder.BeSweet
                Select Case Params.Codec
                    Case AudioCodec.AAC, AudioCodec.MP3, AudioCodec.AC3
                        Return GuiAudioEncoder.BeSweet
                End Select
            Case GuiAudioEncoder.Eac3to
                Select Case Params.Codec
                    Case AudioCodec.AAC, AudioCodec.AC3, AudioCodec.Flac, AudioCodec.WAV, AudioCodec.DTS
                        Return GuiAudioEncoder.Eac3to
                End Select
            Case GuiAudioEncoder.ffmpeg
                Return GuiAudioEncoder.ffmpeg
            Case GuiAudioEncoder.qaac
                If Params.Codec = AudioCodec.AAC Then
                    Return GuiAudioEncoder.qaac
                End If
        End Select

        Select Case Params.Codec
            Case AudioCodec.AC3, AudioCodec.AAC, AudioCodec.Flac, AudioCodec.WAV
                Return GuiAudioEncoder.Eac3to
        End Select

        Return GuiAudioEncoder.ffmpeg
    End Function

    Function GetCommandLine(includePaths As Boolean) As String
        Select Case GetEncoder()
            Case GuiAudioEncoder.BeSweet
                Return GetBeSweetCommandLine(includePaths)
            Case GuiAudioEncoder.Eac3to
                Return GetEac3toCommandLine(includePaths)
            Case GuiAudioEncoder.qaac
                Return GetqaacCommandLine(includePaths)
            Case Else
                Return GetFfmpegCommandLine(includePaths)
        End Select
    End Function

    Overrides Property SupportedInput As String()
        Get
            Select Case GetEncoder()
                Case GuiAudioEncoder.BeSweet
                    Return FileTypes.BeSweetInput
                Case GuiAudioEncoder.Eac3to
                    Return FileTypes.eac3toInput
                Case GuiAudioEncoder.qaac
                    Return FileTypes.qaacInput
                Case Else
                    Return FileTypes.Audio.Concat(FileTypes.VideoAudio).ToArray
            End Select
        End Get
        Set(value As String())
        End Set
    End Property

    <Serializable()>
    Class Parameters
        Property AacProfile As AudioAacProfile
        Property BeSweetAzid As String = ""
        Property BeSweetDynamicCompression As String = "Normal"
        Property BeSweetGainAndNormalization As String = "-norm 0.97"
        Property Codec As AudioCodec
        Property CustomSwitches As String = ""
        Property Down16 As Boolean
        Property eac3toExtractDtsCore As Boolean
        Property eac3toStereoDownmixMode As Integer
        Property Encoder As GuiAudioEncoder
        Property FrameRateMode As AudioFrameRateMode
        Property Normalize As Boolean = True
        Property qaacHE As Boolean
        Property qaacLowpass As Integer
        Property qaacNoDither As Boolean
        Property qaacQuality As Integer = 2
        Property qaacRateMode As Integer
        Property Quality As Single = 0.3
        Property RateMode As AudioRateMode
        Property SamplingRate As Integer
        Property ChannelsMode As ChannelsMode
    End Class
End Class

Public Enum AudioCodec
    AAC
    AC3
    DTS
    Flac
    MP3
    Opus
    Vorbis
    WAV
End Enum

Public Enum AudioRateMode
    CBR
    ABR
    VBR
End Enum

Public Enum AudioAacProfile
    Automatic
    LC
    SBR
    <DispName("SBR+PS")> SBRPS = 300
End Enum

Public Enum GuiAudioEncoder
    Automatic
    BeSweet
    Eac3to
    ffmpeg
    qaac
End Enum

Public Enum AudioFrameRateMode
    Keep
    <DispName("Apply PAL speedup")> Speedup
    <DispName("Reverse PAL speedup")> Slowdown
End Enum

Public Enum AudioDownMixMode
    <DispName("Simple")> Stereo
    <DispName("Dolby Surround")> Surround
    <DispName("Dolby Surround 2")> Surround2
End Enum

Public Enum ChannelsMode
    Original
    <DispName("1 (Mono)")> _1
    <DispName("2 (Stereo)")> _2
    <DispName("5.1")> _6
    <DispName("6.1")> _7
    <DispName("7.1")> _8
End Enum