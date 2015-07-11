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
    Property Channels As Integer = 2
    Property Gain As Single
    Property Streams As List(Of AudioStream) = New List(Of AudioStream)

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

        Me.Bitrate = bitrate
        Me.SupportedInput = input
        Me.OutputFileType = fileType
        Me.Channels = channels
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
            End If
        End Set
    End Property

    Sub SetStreamOrLanguage()
        If File = "" Then Exit Sub

        If File <> p.LastOriginalSourceFile Then
            For Each i In Language.Languages
                If File.Contains(i.CultureInfo.EnglishName) Then
                    Language = i
                    Exit Sub
                End If
            Next

            Language = New Language
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

    Private StreamValue As AudioStream

    Property Stream As AudioStream
        Get
            Return StreamValue
        End Get
        Set(value As AudioStream)
            StreamValue = value

            If Not Stream Is Nothing Then
                If Not p.Script.GetFilter("Source").Script.Contains("DirectShowSource") Then
                    Delay = Stream.Delay
                End If

                Language = Stream.Language
                StreamName = Stream.Title
            End If
        End Set
    End Property

    Function IsInputSupported() As Boolean
        Return SupportedInput.Contains(Filepath.GetExt(File))
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

                    If re.IsMatch(base) Then
                        base = re.Replace(base, "")
                    End If
                End If
            Else
                If Not base.Contains("ms") Then
                    base += " " & Delay & "ms"
                End If
            End If
        End If

        Dim targetDir = If(p.TempDir <> "", p.TempDir, Filepath.GetDir(File))

        Return targetDir + base + "_out." + OutputFileType
    End Function

    Function SolveMacros(value As String) As String
        Return SolveMacros(value, True)
    End Function

    Function SolveMacros(value As String, silent As Boolean) As String
        value = value.Replace("%input%", File)
        value = value.Replace("%output%", GetOutputFile)
        value = value.Replace("%bitrate%", Bitrate.ToString)
        value = value.Replace("%channels%", Channels.ToString)
        value = value.Replace("%language_native%", Language.CultureInfo.NativeName)
        value = value.Replace("%language_english%", Language.Name)
        value = value.Replace("%delay%", Delay.ToString)
        Return Macro.Solve(value, silent)
    End Function

    Shared Function GetDefaults() As List(Of AudioProfile)
        Dim ret As New List(Of AudioProfile)

        ret.Add(New GUIAudioProfile(AudioCodec.AAC, 0.35))
        ret.Add(New GUIAudioProfile(AudioCodec.Opus, 1) With {.Bitrate = 80})
        ret.Add(New GUIAudioProfile(AudioCodec.Flac, 0.3))
        ret.Add(New GUIAudioProfile(AudioCodec.Vorbis, 1))
        ret.Add(New GUIAudioProfile(AudioCodec.MP3, 4))
        ret.Add(New GUIAudioProfile(AudioCodec.AC3, 1.0) With {.Channels = 6, .Bitrate = 448})
        ret.Add(GetProfile("Command Line", 48, "-bsn( -abr %bitrate% -aacprofile_hev2 )"))
        ret.Add(New MuxAudioProfile())
        ret.Add(New NullAudioProfile())

        Return ret
    End Function

    Shared Function GetProfile(
        name As String,
        bitrate As Integer,
        params As String,
        Optional channels As Integer = 2) As BatchAudioProfile

        Dim cmdl = """%app:BeSweet%"" -core( -input ""%input%"" -output ""%output%"" )"
        cmdl += " -azid( -c normal"

        If channels = 2 Then
            cmdl += " -L -3db"
        End If

        cmdl += " ) -ota( -d %delay% -g max ) "
        cmdl += params

        Dim fileType = ""

        If params.Contains("-bsn( ") Then
            fileType = "m4a"
        ElseIf params.Contains("-lame( ") Then
            fileType = "mp3"
        ElseIf params.Contains("-ogg( ") Then
            fileType = "ogg"
        End If

        Dim input = If(channels = 6, New String() {"ac3"}, "ac3 mpa mp2 mp3 wav ogg".Split(" "c))

        Return New BatchAudioProfile(name, bitrate, input, fileType, channels, cmdl)
    End Function
End Class

<Serializable()>
Public Class BatchAudioProfile
    Inherits AudioProfile

    Sub New(name As String,
            bitrate As Integer,
            input As String(),
            fileType As String,
            channels As Integer,
            commandLines As String)

        MyBase.New(name, bitrate, input, fileType, channels)
        Me.CommandLines = commandLines
        CanEditValue = True
    End Sub

    Private CommandValue As String

    Overrides Property CommandLines() As String
        Get
            Return CommandValue
        End Get
        Set(Value As String)
            CommandValue = Value
        End Set
    End Property

    Overrides Function Edit() As DialogResult
        Using f As New CommandLineAudioForm(Me)
            f.mbLanguage.Enabled = False
            f.lLanguage.Enabled = False
            f.tbDelay.Enabled = False
            f.lDelay.Enabled = False
            Return f.ShowDialog()
        End Using
    End Function

    Public Overrides Sub Encode()
        If File <> "" Then
            Dim bitrateBefore = p.VideoBitrate
            Dim targetPath = GetOutputFile()

            Dim commands = SolveMacros(CommandLines).Trim

            If commands.Contains("|") OrElse commands.Contains(CrLf) Then
                Dim batchPath = p.TempDir + Filepath.GetBase(File) + "_audio.bat"

                IO.File.WriteAllText(batchPath, commands, Encoding.GetEncoding(850))

                Using proc As New Proc
                    proc.Init("Audio encoding: " + Name, "Maximum Gain Found", "transcoding ...")
                    proc.WriteLine(commands + CrLf2)
                    proc.File = "cmd.exe"
                    proc.Arguments = "/C call """ + batchPath + """"
                    proc.BatchCode = commands

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
            Else
                Using proc As New Proc
                    proc.Init("Audio encoding: " + Name, "Maximum Gain Found", "transcoding ...")
                    proc.CommandLine = commands
                    proc.Start()
                End Using
            End If

            If g.WasFileJustWritten(targetPath) Then
                File = targetPath
                Bitrate = Calc.GetBitrateFromFile(File, p.TargetSeconds)
                p.VideoBitrate = CInt(Calc.GetTotalBitrate - Calc.GetAudioBitrate())

                If Not p.VideoEncoder.QualityMode Then
                    Log.WriteLine("Video Bitrate: " + bitrateBefore.ToString() + " -> " + p.VideoBitrate.ToString)
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
        Using f As New CommandLineAudioForm(Me)
            f.ShowDialog()
        End Using
    End Sub

    Overrides Function HandlesDelay() As Boolean
        Return CommandLines.Contains("%delay%")
    End Function
End Class

<Serializable()>
Public Class NullAudioProfile
    Inherits AudioProfile

    Sub New()
        MyBase.New("No Audio", 0, {}, "ignore", 0)
    End Sub

    Overrides Function HandlesDelay() As Boolean
    End Function

    Overrides Sub EditProject()
        Using f As New SimpleSettingsForm("Null Audio Profile Options")
            f.Size = New Size(500, 300)

            Dim ui = f.SimpleUI

            Dim page = ui.CreateFlowPage("main page")

            Dim nb = ui.AddNumericBlock(page)
            nb.Label.Text = "Reserved Bitrate:"
            nb.NumEdit.Init(0, 1000000, 8)
            nb.NumEdit.Value = CDec(Bitrate)
            nb.NumEdit.SaveAction = Sub(value) Bitrate = CDec(value)

            If f.ShowDialog() = DialogResult.OK Then
                ui.Save()
            End If
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
Public Class MuxAudioProfile
    Inherits AudioProfile

    Sub New()
        MyBase.New("Just Mux", 0, Nothing, "ignore", 2)
        CanEditValue = True
    End Sub

    Public Overrides Property OutputFileType As String
        Get
            If Stream Is Nothing Then
                Return Filepath.GetExt(File)
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

    Public Overrides Sub Encode()
    End Sub

    Private Overloads Function Edit(showProjectSettings As Boolean) As DialogResult
        Using f As New SimpleSettingsForm("Audio Mux Options", "The Audio Mux options allow to add a audio file without reencoding.")
            f.Size = New Size(800, 350)

            Dim ui = f.SimpleUI

            Dim page = ui.CreateFlowPage("main page")
            page.SuspendLayout()

            Dim tbb = ui.AddTextButtonBlock(page)
            tbb.Label.Text = "Stream Name:"
            tbb.Label.Tooltip = "Stream name used by the muxer. The stream name may contain macros."
            tbb.Expand(tbb.Edit)
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
                    mbi.MenuButton.Add("More | " + i.ToString.Substring(0, 1) + " | " + i.ToString, i)
                End If
            Next

            page.ResumeLayout()

            Dim ret = f.ShowDialog()

            If ret = DialogResult.OK Then
                ui.Save()
            End If

            Return ret
        End Using
    End Function
End Class

<Serializable()>
Public Class GUIAudioProfile
    Inherits AudioProfile

    Property Params As New Parameters

    Sub New(codec As AudioCodec, quality As Single)
        MyBase.New(Nothing)

        Me.Channels = Channels
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
                    If Channels = 6 Then
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
                    If Channels = 6 Then
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
                        proc.Init("Audio encoding using BeSweet", "Processed", "transcoding", "Maximum Gain Found : ", "Asserting gain")
                    ElseIf i.Contains("eac3to.exe") Then
                        proc.Init("Audio encoding using eac3to", "process: ", "analyze: ")
                        proc.TrimChars = {"-"c, " "c}
                        proc.RemoveChars = {VB6.ChrW(8)} 'backspace
                    ElseIf i.Contains("ffmpeg.exe") Then
                        proc.Init("Audio encoding using ffmpeg", "size=", "decoding is not implemented", "unsupported frame type", "upload a sample")
                        proc.Encoding = Encoding.UTF8
                    ElseIf i.Contains("qaac64.exe") Then
                        proc.Init("Audio encoding using qaac", ", ETA ")
                    End If

                    proc.CommandLine = i
                    proc.AllowedExitCodes = {0, 1}
                    proc.Start()
                End Using

                If g.WasFileJustWritten(targetPath) Then
                    File = targetPath
                    Bitrate = Calc.GetBitrateFromFile(File, p.TargetSeconds)
                    p.VideoBitrate = CInt(Calc.GetTotalBitrate - Calc.GetAudioBitrate())

                    If Not p.VideoEncoder.QualityMode Then
                        Log.WriteLine("Video Bitrate: " + bitrateBefore.ToString() + " -> " + p.VideoBitrate.ToString)
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

    Function GetEeac3toCommandLine(includePaths As Boolean) As String
        Dim r, id As String

        If IsOneOf(Filepath.GetExtFull(File), ".ts", ".m2ts") AndAlso Not Stream Is Nothing Then
            id = (Stream.StreamOrder + 1) & ": "
        End If

        If includePaths Then
            r = """" + Packs.eac3to.GetPath + """ " + id + """" + File + """ """ + GetOutputFile() + """"
        Else
            r = "eac3to"
        End If

        If Not (Params.Codec = AudioCodec.DTS AndAlso Params.eac3toExtractDtsCore) Then
            Select Case Params.Codec
                Case AudioCodec.AAC
                    r += " -quality=" & Params.Quality.ToString(CultureInfo.InvariantCulture)
                Case AudioCodec.AC3
                    r += " -" & Bitrate

                    If Not {192, 224, 384, 448, 640}.Contains(CInt(Bitrate)) Then
                        Return "Invalid bitrate, choose 192, 224, 384, 448 or 640"
                    End If
                Case AudioCodec.DTS
                    r += " -" & Bitrate
            End Select

            If Params.Normalize Then r += " -normalize"
            If Params.Down16 Then r += " -down16"
            If Params.SamplingRate <> 0 Then r += " -resampleTo" & Params.SamplingRate
            If Params.FrameRateMode = AudioFrameRateMode.Speedup Then r += " -speedup"
            If Params.FrameRateMode = AudioFrameRateMode.Slowdown Then r += " -slowdown"
            If Delay <> 0 Then r += " " + If(Delay > 0, "+", "") & Delay & "ms"

            If Channels = 2 Then
                If Params.eac3toStereoDownmixMode = 0 Then
                    r += " -downStereo"
                Else
                    r += " -downDpl"
                End If
            End If

            If Params.CustomSwitches <> "" Then
                r += " " + Params.CustomSwitches
            End If
        ElseIf Params.eac3toExtractDtsCore Then
            r += " -core"
        End If

        If includePaths Then r += " -progressnumbers"

        Return r
    End Function

    Function GetqaacCommandLine(includePaths As Boolean) As String
        Dim r As String

        includePaths = includePaths And File <> ""

        If includePaths Then
            r = """" + Packs.qaac.GetPath + """ -o """ + GetOutputFile() + """"
        Else
            r = "qaac"
        End If

        Select Case Params.qaacRateMode
            Case 0
                r += " --tvbr " & CInt(Params.Quality)
            Case 1
                r += " --cvbr " & CInt(Bitrate)
            Case 2
                r += " --abr " & CInt(Bitrate)
            Case 3
                r += " --cbr " & CInt(Bitrate)
        End Select

        If Params.qaacHE Then
            r += " --he"
        End If

        If Params.CustomSwitches <> "" Then
            r += " " + Params.CustomSwitches
        End If

        If Delay <> 0 Then
            r += " --delay " + (Delay / 1000).ToString(CultureInfo.InvariantCulture)
        End If

        If Params.Normalize Then
            r += " --normalize"
        End If

        If Params.qaacQuality <> 2 Then
            r += " --quality " & Params.qaacQuality
        End If

        If Params.SamplingRate <> 0 Then
            r += " --rate " & Params.SamplingRate
        End If

        If Params.qaacLowpass <> 0 Then
            r += " --lowpass " & Params.qaacLowpass
        End If

        If Params.qaacNoDither Then
            r += " --no-dither"
        End If

        If Gain <> 0 Then
            r += " --gain " & Gain.ToString(CultureInfo.InvariantCulture)
        End If

        If includePaths Then
            r += " """ + File + """"
        End If

        Return r
    End Function

    Function GetFfmpegCommandLine(includePaths As Boolean) As String
        Dim ret As String

        If includePaths AndAlso File <> "" Then
            ret = """" + Packs.ffmpeg.GetPath + """ -i """ + File + """"
        Else
            ret = "ffmpeg"
        End If

        If Not Stream Is Nothing Then ret += " -map 0:" & Stream.StreamOrder

        Select Case Params.Codec
            Case AudioCodec.MP3
                ret += " -c:a libmp3lame"

                If Params.RateMode = AudioRateMode.VBR Then
                    ret += " -q:a " & CInt(Params.Quality)
                Else
                    ret += " -b:a " & CInt(Bitrate) & "k"
                End If
            Case AudioCodec.AC3
                If Not {192, 224, 384, 448, 640}.Contains(CInt(Bitrate)) Then
                    Return "Invalid bitrate, choose 192, 224, 384, 448 or 640"
                End If

                ret += " -b:a " & CInt(Bitrate) & "k"
            Case AudioCodec.DTS
                ret += " -strict -2 -b:a " & CInt(Bitrate) & "k"
            Case AudioCodec.Flac, AudioCodec.WAV
            Case AudioCodec.Vorbis
                ret += " -c:a libvorbis"

                If Params.RateMode = AudioRateMode.VBR Then
                    ret += " -q:a " & CInt(Params.Quality)
                Else
                    ret += " -b:a " & CInt(Bitrate) & "k"
                End If
            Case AudioCodec.Opus
                ret += " -c:a libopus"

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

        If Params.CustomSwitches <> "" Then
            ret += " " + Params.CustomSwitches
        End If

        If Gain <> 0 Then
            ret += " -af volume=" + Gain.ToString(CultureInfo.InvariantCulture) + "dB"
        End If

        ret += " -ac " & Channels

        If Params.SamplingRate <> 0 Then
            ret += " -ar " & Params.SamplingRate
        End If

        ret += " -y"

        If includePaths AndAlso File <> "" Then
            ret += " """ + GetOutputFile() + """"
        End If

        Return ret
    End Function

    Function GetBeSweetCommandLine(includePaths As Boolean) As String
        Dim r As String

        If includePaths Then
            r = """" + Packs.BeSweet.GetPath + """"
        Else
            r = "BeSweet"
        End If

        If includePaths AndAlso File <> "" Then
            r += " -core( -input """ + File + """ -output """ + GetOutputFile() & """ )"
        End If

        Dim t = ""

        If Not Params.BeSweetAzid.Contains("-s") AndAlso Params.BeSweetDownmixMode <> AudioDownMixMode.Surround Then
            t += " -s " + Params.BeSweetDownmixMode.ToString.ToLower
        End If

        If Not Params.BeSweetAzid.Contains("-c") AndAlso Params.BeSweetDynamicCompression <> "None" Then
            t += " -c " + Params.BeSweetDynamicCompression.ToLower
        End If

        If Not Params.BeSweetAzid.Contains("-L") AndAlso Channels = 2 Then
            t += " -L -3db"
        End If

        If Params.BeSweetAzid <> "" Then
            t += " " + Params.BeSweetAzid
        End If

        If t <> "" Then
            r += " -azid(" + t + " )"
        End If

        Dim ota = If(Delay <> 0, " -d " & Delay, "")

        If Params.Normalize AndAlso OK(Params.BeSweetGainAndNormalization) Then
            ota += " " + Params.BeSweetGainAndNormalization
        End If

        If Params.FrameRateMode = AudioFrameRateMode.Speedup Then
            ota += " -r 23976 25000"
        End If

        If Params.FrameRateMode = AudioFrameRateMode.Slowdown Then
            ota += " -r 25000 23976"
        End If

        If ota <> "" Then
            r += " -ota(" & ota & " )"
        End If

        If Params.SamplingRate <> 0 Then
            r += " -ssrc( --rate " & Params.SamplingRate & " )"
        End If

        If Params.BeSweetCustom <> "" Then
            r += " " + Params.BeSweetCustom
        End If

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
                        r += " -bsn( -vbr " & Params.Quality.ToString(CultureInfo.InvariantCulture) & profile & ch & " )"
                    Case AudioRateMode.ABR
                        r += " -bsn( -abr " & CInt(Bitrate) & profile & ch & " )"
                    Case AudioRateMode.CBR
                        r += " -bsn( -cbr " & CInt(Bitrate) & profile & ch & " )"
                End Select
            Case AudioCodec.MP3
                Select Case Params.RateMode
                    Case AudioRateMode.VBR
                        r += " -lame( -v --vbr-new -V " & CInt(Params.Quality) & " -b 32 -h )"
                    Case AudioRateMode.ABR
                        r += " -lame( --abr " & CInt(Bitrate) & " -h )"
                    Case AudioRateMode.CBR
                        r += " -lame( -b " & CInt(Bitrate) & " -h )"
                End Select
        End Select

        Return r
    End Function

    Public ReadOnly Property CustomName() As String
        Get
            Return MyBase.Name
        End Get
    End Property

    Public Overrides Property Name As String
        Get
            If MyBase.Name = "" Then
                Dim ch = If(Channels = 6, " 5.1", If(Channels = 2, " 2.0", " Mono"))
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
            End If

            Return MyBase.Name
        End Get
        Set(value As String)
            MyBase.Name = value
        End Set
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
                    Case AudioCodec.AAC, AudioCodec.MP3
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
                Return GetEeac3toCommandLine(includePaths)
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
                    Return FileTypes.Audio.Concat(FileTypes.AudioVideo).ToArray
            End Select
        End Get
        Set(value As String())
        End Set
    End Property

    <Serializable()>
    Public Class Parameters
        Implements ISerializable

        Property AacProfile As AudioAacProfile
        Property BeSweetAzid As String = ""
        Property BeSweetCustom As String = ""
        Property BeSweetDownmixMode As AudioDownMixMode = AudioDownMixMode.Surround
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

        Sub New()
        End Sub

        <DebuggerNonUserCode()>
        Sub New(info As SerializationInfo, context As StreamingContext)
            For Each i In Me.GetType.GetProperties()
                Try
                    i.SetValue(Me, info.GetValue(i.Name, i.PropertyType), Nothing)
                Catch
                End Try
            Next
        End Sub

        Sub GetObjectData(info As SerializationInfo, context As StreamingContext) Implements ISerializable.GetObjectData
            For Each i In Me.GetType.GetProperties()
                info.AddValue(i.Name, i.GetValue(Me, Nothing))
            Next
        End Sub
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