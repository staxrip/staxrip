Imports System.Globalization
Imports System.Text
Imports System.Text.RegularExpressions

Imports StaxRip.VideoEncoderCommandLine

Public NotInheritable Class SupportReportOptions
    Property IncludeSanitizedCommands As Boolean
End Class

' Keep this report allowlist-based. Do not append raw logs, paths, media titles, scripts, profile names,
' or arbitrary user-entered values. A new field needs a privacy review before it is added here.
Public NotInheritable Class SupportReportBuilder
    Const ReportSchemaVersion = 1

    Private Sub New()
    End Sub

    Shared Function Build(proj As Project, options As SupportReportOptions) As String
        If proj Is Nothing Then Throw New ArgumentNullException(NameOf(proj))
        If options Is Nothing Then options = New SupportReportOptions()

        Dim sb As New StringBuilder()

        sb.AppendLine("## StaxRip support report")
        sb.AppendLine()
        sb.AppendLine("> Generated from an allowlist of diagnostic fields. Review and edit this report before sharing it.")
        sb.AppendLine("> File names, paths, media titles, template names, scripts, custom text, and the raw log are not included.")
        sb.AppendLine()

        AppendApplication(sb)
        AppendWorkflow(sb, proj)
        AppendSourceVideo(sb, proj)
        AppendAudio(sb, proj)
        AppendTools(sb, proj)
        AppendFailureSummary(sb, proj)
        AppendCommands(sb, proj, options.IncludeSanitizedCommands)
        AppendReporterPrompts(sb)
        AppendPrivacySummary(sb, options.IncludeSanitizedCommands)

        Return sb.ToString().TrimEnd() + Environment.NewLine
    End Function

    Private Shared Sub AppendApplication(sb As StringBuilder)
        AppendHeading(sb, "Application")
        AppendItem(sb, "Report schema", ReportSchemaVersion.ToString(CultureInfo.InvariantCulture))
        AppendItem(sb, "StaxRip", g.DefaultCommands.GetApplicationDetails())

        If s?.Version IsNot Nothing Then
            AppendItem(sb, "Settings version", FormatVersion(s.Version))
        End If

        AppendItem(sb, "Windows", OSVersion.VersionString)
        AppendItem(sb, "Process architecture", If(Environment.Is64BitProcess, "x64", "x86"))
        AppendItem(sb, "UI culture", CultureInfo.CurrentCulture.Name)
    End Sub

    Private Shared Sub AppendWorkflow(sb As StringBuilder, proj As Project)
        AppendHeading(sb, "Configured workflow")
        AppendItem(sb, "Template", If(String.IsNullOrWhiteSpace(proj.TemplateName), "Not named", "Configured (name omitted)"))
        AppendItem(sb, "Video encoding", If(proj.SkipVideoEncoding, "Skipped", "Enabled"))
        AppendItem(sb, "Audio encoding", If(proj.SkipAudioEncoding, "Skipped", "Enabled"))
        AppendItem(sb, "Audio source", If(proj.UseScriptAsAudioSource, "Frame-server script", "Selected audio stream or file"))
        AppendItem(sb, "Frame server", GetFrameServer(proj))
        AppendItem(sb, "Video encoder", GetTypeName(proj.VideoEncoder))

        If proj.VideoEncoder?.Muxer IsNot Nothing Then
            AppendItem(sb, "Muxer", GetTypeName(proj.VideoEncoder.Muxer))
        End If

        AppendItem(sb, "Target container", GetFileType(proj.TargetFile))

        If proj.Ranges IsNot Nothing AndAlso proj.Ranges.Count > 0 Then
            AppendItem(sb, "Cutting", $"{proj.CuttingMode}; {proj.Ranges.Count} range(s)")
        Else
            AppendItem(sb, "Cutting", "Not configured")
        End If

        If proj.Script IsNot Nothing Then
            Dim activeFilters = If(proj.Script.Filters Is Nothing, 0, proj.Script.Filters.Where(Function(filter) filter.Active).Count())
            AppendItem(sb, "Active video filters", activeFilters.ToString(CultureInfo.InvariantCulture))
        End If

        AppendItem(sb, "Project custom script text", PresentOrAbsent(Not String.IsNullOrWhiteSpace(proj.CodeAtTop) OrElse Not String.IsNullOrWhiteSpace(proj.CodeAtBottom)) + " (content omitted)")
    End Sub

    Private Shared Sub AppendSourceVideo(sb As StringBuilder, proj As Project)
        AppendHeading(sb, "Source video")

        If String.IsNullOrWhiteSpace(proj.SourceFile) Then
            sb.AppendLine("No source is loaded.")
            sb.AppendLine()
            Return
        End If

        AppendItem(sb, "Source files", If(proj.SourceFiles?.Count > 0, proj.SourceFiles.Count, 1).ToString(CultureInfo.InvariantCulture))
        AppendItem(sb, "Container", GetFileType(proj.SourceFile))
        AppendOptionalItem(sb, "Format", proj.SourceVideoFormat)
        AppendOptionalItem(sb, "Format profile", proj.SourceVideoFormatProfile)
        AppendOptionalItem(sb, "HDR format", proj.SourceVideoHdrFormat)

        If proj.SourceWidth > 0 AndAlso proj.SourceHeight > 0 Then
            AppendItem(sb, "Dimensions", $"{proj.SourceWidth} x {proj.SourceHeight}")
        End If

        If proj.SourceFrameRate > 0 Then
            AppendItem(sb, "Frame rate", proj.SourceFrameRate.ToString(CultureInfo.InvariantCulture) + OptionalSuffix(proj.SourceFrameRateMode))
        End If

        If proj.SourceFrames > 0 Then AppendItem(sb, "Frames", proj.SourceFrames.ToString(CultureInfo.InvariantCulture))
        If proj.SourceSeconds > 0 Then AppendItem(sb, "Duration", TimeSpan.FromSeconds(proj.SourceSeconds).ToString("c", CultureInfo.InvariantCulture))
        If proj.SourceBitrate > 0 Then AppendItem(sb, "Bitrate", proj.SourceBitrate.ToString(CultureInfo.InvariantCulture) + " kb/s")
        If proj.SourceVideoBitDepth > 0 Then AppendItem(sb, "Bit depth", proj.SourceVideoBitDepth.ToString(CultureInfo.InvariantCulture))

        AppendOptionalItem(sb, "Chroma subsampling", proj.SourceChromaSubsampling)
        AppendOptionalItem(sb, "Color space", proj.SourceColorSpace)
        AppendOptionalItem(sb, "Scan type", proj.SourceScanType)
        AppendOptionalItem(sb, "Scan order", proj.SourceScanOrder)

        If proj.TargetWidth > 0 AndAlso proj.TargetHeight > 0 Then
            AppendItem(sb, "Configured output dimensions", $"{proj.TargetWidth} x {proj.TargetHeight}")
        End If

        If proj.TargetFrameRate > 0 Then
            AppendItem(sb, "Configured output frame rate", proj.TargetFrameRate.ToString(CultureInfo.InvariantCulture))
        End If
    End Sub

    Private Shared Sub AppendAudio(sb As StringBuilder, proj As Project)
        AppendHeading(sb, "Audio")

        Dim tracks = If(proj.AudioTracks, New List(Of AudioTrack)()).
            Where(Function(track) track?.AudioProfile IsNot Nothing AndAlso Not TypeOf track.AudioProfile Is NullAudioProfile AndAlso track.IsRelevant).
            Take(Math.Max(proj.AudioTracksAvailable, 0)).
            ToList()

        If tracks.Count = 0 Then
            sb.AppendLine("No active audio output track is configured.")
            sb.AppendLine()
            Return
        End If

        For index = 0 To tracks.Count - 1
            Dim ap = tracks(index).AudioProfile
            sb.AppendLine($"#### Audio output {index + 1}")
            sb.AppendLine()
            AppendItem(sb, "Input type", GetFileType(ap.File))
            AppendItem(sb, "Input selection", If(ap.Stream Is Nothing, "Individual audio file or unresolved stream", "Stream selected from a container"))
            AppendItem(sb, "Profile type", GetTypeName(ap))
            AppendItem(sb, "Decoder", ap.Decoder.ToString())
            AppendItem(sb, "Decoding mode", ap.DecodingMode.ToString())
            AppendItem(sb, "Output codec", ap.AudioCodec.ToString())
            AppendItem(sb, "Output type", ap.OutputFileType)
            If ap.Bitrate > 0 Then AppendItem(sb, "Configured bitrate", ap.Bitrate.ToString(CultureInfo.InvariantCulture) + " kb/s")
            If ap.Delay <> 0 Then AppendItem(sb, "Delay", ap.Delay.ToString(CultureInfo.InvariantCulture) + " ms")
            If ap.Gain <> 0 Then AppendItem(sb, "Gain", ap.Gain.ToString(CultureInfo.InvariantCulture) + " dB")

            Dim guiProfile = TryCast(ap, GUIAudioProfile)
            If guiProfile IsNot Nothing Then
                AppendItem(sb, "Encoder", guiProfile.GetEncoder().ToString())
                AppendItem(sb, "Processing path", $"{ap.Decoder} -> {ap.DecodingMode} -> {guiProfile.GetEncoder()} ({ap.AudioCodec})")
                AppendItem(sb, "Channel mode", guiProfile.Params.ChannelsMode.ToString())
                If guiProfile.Params.ChannelsMode <> ChannelsMode.Original AndAlso ap.Channels > 0 Then
                    AppendItem(sb, "Configured channels", ap.Channels.ToString(CultureInfo.InvariantCulture))
                End If
                AppendItem(sb, "Normalize", YesNo(guiProfile.Params.Normalize))
                If guiProfile.Params.SamplingRate > 0 Then AppendItem(sb, "Configured sample rate", guiProfile.Params.SamplingRate.ToString(CultureInfo.InvariantCulture) + " Hz")
                If guiProfile.Params.Codec = AudioCodec.FLAC Then
                    AppendItem(sb, "FLAC bit depth", If(guiProfile.Params.ffmpegFlacBitDepth > 0, CInt(guiProfile.Params.ffmpegFlacBitDepth).ToString(CultureInfo.InvariantCulture), "Same as source"))
                End If
                If guiProfile.Params.ProbeSize <> 5 Then AppendItem(sb, "Probe size", guiProfile.Params.ProbeSize.ToString(CultureInfo.InvariantCulture) + " MB")
                If guiProfile.Params.AnalyzeDuration <> 5 Then AppendItem(sb, "Analyze duration", guiProfile.Params.AnalyzeDuration.ToString(CultureInfo.InvariantCulture) + "M")
                AppendItem(sb, "Custom switches", PresentOrAbsent(Not String.IsNullOrWhiteSpace(guiProfile.Params.CustomSwitches)) + " (content omitted)")
            Else
                If ap.Channels > 0 Then AppendItem(sb, "Configured channels", ap.Channels.ToString(CultureInfo.InvariantCulture))
                AppendItem(sb, "Processing path", $"{ap.Decoder} -> {ap.DecodingMode} -> {ap.AudioCodec}")
            End If

            AppendAudioStream(sb, ap.Stream)
            sb.AppendLine()
        Next
    End Sub

    Private Shared Sub AppendAudioStream(sb As StringBuilder, stream As AudioStream)
        If stream Is Nothing Then
            AppendItem(sb, "Selected input stream", "Metadata not available")
            Return
        End If

        AppendItem(sb, "Selected input stream", (stream.Index + 1).ToString(CultureInfo.InvariantCulture))
        If stream.ID > 0 Then AppendItem(sb, "Stream ID", stream.ID.ToString(CultureInfo.InvariantCulture))
        If stream.StreamOrder >= 0 Then AppendItem(sb, "Stream order", stream.StreamOrder.ToString(CultureInfo.InvariantCulture))
        AppendOptionalItem(sb, "Input format", stream.FormatString)
        AppendOptionalItem(sb, "Input profile", stream.FormatProfile)
        If stream.Channels > 0 Then AppendItem(sb, "Input channels", stream.Channels.ToString(CultureInfo.InvariantCulture))
        If stream.SamplingRate > 0 Then AppendItem(sb, "Input sample rate", stream.SamplingRate.ToString(CultureInfo.InvariantCulture) + " Hz")
        If stream.BitDepth > 0 Then AppendItem(sb, "Input bit depth", stream.BitDepth.ToString(CultureInfo.InvariantCulture))
        If stream.Bitrate > 0 Then AppendItem(sb, "Input bitrate", stream.Bitrate.ToString(CultureInfo.InvariantCulture) + " b/s")
        If stream.Delay <> 0 Then AppendItem(sb, "Input delay", stream.Delay.ToString(CultureInfo.InvariantCulture) + " ms")
        If stream.Language IsNot Nothing Then AppendItem(sb, "Language", stream.Language.ThreeLetterCode)
        AppendItem(sb, "Default stream", YesNo(stream.Default))
        AppendItem(sb, "Forced stream", YesNo(stream.Forced))
    End Sub

    Private Shared Sub AppendTools(sb As StringBuilder, proj As Project)
        AppendHeading(sb, "Relevant tools")

        Dim tools As New SortedDictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        AddTool(tools, Package.ffmpeg)
        AddTool(tools, Package.MediaInfo)

        If proj.Script?.IsAviSynth Then AddTool(tools, Package.AviSynth)
        If proj.Script?.IsVapourSynth Then AddTool(tools, Package.VapourSynth)

        Dim basicVideoEncoder = TryCast(proj.VideoEncoder, BasicVideoEncoder)
        If basicVideoEncoder IsNot Nothing Then AddTool(tools, basicVideoEncoder.CommandLineParams.Package)

        For Each track In If(proj.AudioTracks, New List(Of AudioTrack)())
            If track?.AudioProfile Is Nothing OrElse TypeOf track.AudioProfile Is NullAudioProfile OrElse Not track.IsRelevant Then Continue For
            AddAudioTools(tools, track.AudioProfile)
        Next

        Dim logText = proj.Log?.ToString()
        If Not String.IsNullOrWhiteSpace(logText) Then
            For Each pack As Package In Package.Items.Values
                If pack Is Nothing OrElse String.IsNullOrWhiteSpace(pack.Name) OrElse String.IsNullOrWhiteSpace(pack.Version) Then Continue For
                If logText.IndexOf(pack.Name + " " + pack.Version, StringComparison.OrdinalIgnoreCase) >= 0 Then AddTool(tools, pack)
            Next
        End If

        AppendItem(sb, "Coverage", "Configured core and encoders, plus versioned packages named in the current log")
        For Each tool In tools
            AppendItem(sb, tool.Key, If(String.IsNullOrWhiteSpace(tool.Value), "Version not recorded", tool.Value))
        Next
    End Sub

    Private Shared Sub AddAudioTools(tools As IDictionary(Of String, String), profile As AudioProfile)
        Select Case profile.Decoder
            Case AudioDecoderMode.Automatic, AudioDecoderMode.ffmpeg
                AddTool(tools, Package.ffmpeg)
            Case AudioDecoderMode.FFAudioSource
                AddTool(tools, Package.ffms2)
            Case AudioDecoderMode.eac3to
                AddTool(tools, Package.eac3to)
            Case AudioDecoderMode.NicAudio
                AddTool(tools, Package.NicAudio)
        End Select

        Dim guiProfile = TryCast(profile, GUIAudioProfile)
        If guiProfile Is Nothing Then Return

        Select Case guiProfile.GetEncoder()
            Case GuiAudioEncoder.deezy
                AddTool(tools, Package.DeeZy)
            Case GuiAudioEncoder.eac3to
                AddTool(tools, Package.eac3to)
            Case GuiAudioEncoder.fdkaac
                AddTool(tools, Package.fdkaac)
            Case GuiAudioEncoder.opusenc
                AddTool(tools, Package.OpusEnc)
            Case GuiAudioEncoder.qaac
                AddTool(tools, Package.qaac)
            Case Else
                AddTool(tools, If(guiProfile.Params.Codec = AudioCodec.AAC AndAlso guiProfile.Params.ffmpegLibFdkAAC, Package.ffmpeg_non_free, Package.ffmpeg))
        End Select
    End Sub

    Private Shared Sub AppendFailureSummary(sb As StringBuilder, proj As Project)
        AppendHeading(sb, "Failure summary")

        Dim logText = proj.Log?.ToString()
        AppendItem(sb, "Current log", If(String.IsNullOrWhiteSpace(logText), "No log content", "Available (raw content omitted)"))

        If Not String.IsNullOrWhiteSpace(logText) Then
            Dim exitCodes = Regex.Matches(logText, "returned exit code:\s*(-?\d+)\s*\(0x([0-9A-F]+)\)", RegexOptions.IgnoreCase).
                OfType(Of Match)().
                Select(Function(match) $"{match.Groups(1).Value} (0x{match.Groups(2).Value.ToUpperInvariant()})").
                Distinct().
                ToList()

            AppendItem(sb, "Recorded failing exit codes", If(exitCodes.Count = 0, "None found", String.Join(", ", exitCodes)))
        End If
    End Sub

    Private Shared Sub AppendCommands(sb As StringBuilder, proj As Project, includeCommands As Boolean)
        AppendHeading(sb, "Command summary")

        If Not includeCommands Then
            sb.AppendLine("Sanitized commands were not included. Enable the command option in the preview only if they are needed, then review them before sharing.")
            sb.AppendLine()
            Return
        End If

        sb.AppendLine("Standard paths and every detected text-valued or custom argument are replaced. This is a best-effort summary, so review it before sharing.")
        sb.AppendLine()

        Dim videoCommand = GetSanitizedVideoCommand(proj)
        AppendCommand(sb, "Video", videoCommand)

        Dim audioIndex = 0
        For Each track In If(proj.AudioTracks, New List(Of AudioTrack)())
            If track?.AudioProfile Is Nothing OrElse TypeOf track.AudioProfile Is NullAudioProfile OrElse Not track.IsRelevant Then Continue For
            audioIndex += 1
            AppendCommand(sb, $"Audio {audioIndex}", GetSanitizedAudioCommand(proj, track.AudioProfile))
        Next

        sb.AppendLine("Muxer commands are omitted because muxer profiles can contain free-form command templates.")
        sb.AppendLine()
    End Sub

    Private Shared Function GetSanitizedVideoCommand(proj As Project) As String
        Dim encoder = TryCast(proj.VideoEncoder, BasicVideoEncoder)
        If encoder Is Nothing Then Return "Unavailable for this encoder type"

        Try
            Dim sensitiveValues As New List(Of String) From {
                proj.SourceFile,
                proj.TargetFile,
                proj.TempDir,
                proj.TemplateName,
                proj.CodeAtTop,
                proj.CodeAtBottom,
                proj.VideoEncoder.Name
            }

            For Each parameter In encoder.CommandLineParams.Items.OfType(Of StringParam)()
                sensitiveValues.Add(parameter.Value)
            Next

            Return SanitizeCommandForReport(encoder.GetCommandLine(False, True), sensitiveValues)
        Catch
            Return "Unavailable in the current project state"
        End Try
    End Function

    Private Shared Function GetSanitizedAudioCommand(proj As Project, profile As AudioProfile) As String
        Dim guiProfile = TryCast(profile, GUIAudioProfile)
        If guiProfile Is Nothing Then Return "Omitted for a free-form audio profile"

        Try
            Dim sensitiveValues As New List(Of String) From {
                proj.SourceFile,
                proj.TargetFile,
                proj.TempDir,
                proj.TemplateName,
                profile.File,
                profile.Name,
                profile.StreamName,
                guiProfile.Params.CustomSwitches
            }

            Return SanitizeCommandForReport(guiProfile.GetCommandLine(False), sensitiveValues)
        Catch
            Return "Unavailable in the current project state"
        End Try
    End Function

    Friend Shared Function SanitizeCommandForReport(command As String, sensitiveValues As IEnumerable(Of String)) As String
        If String.IsNullOrWhiteSpace(command) Then Return "Not available"

        ' Commands are optional because external syntax is extensible. Remove every known free-form value first,
        ' then apply generic path and credential guards. The preview still requires manual review.
        Dim ret = command.Replace(Convert.ToChar(13), " "c).Replace(Convert.ToChar(10), " "c).Replace(Convert.ToChar(9), " "c)

        If sensitiveValues IsNot Nothing Then
            For Each value In sensitiveValues.Where(Function(item) Not String.IsNullOrWhiteSpace(item)).OrderByDescending(Function(item) item.Length)
                ret = Regex.Replace(ret, Regex.Escape(value), "<REDACTED>", RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant)
            Next
        End If

        Dim invariantIgnoreCase = RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant
        ret = Regex.Replace(ret, "\b(?:https?|ftp)://\S+", "<URL>", invariantIgnoreCase)
        ret = Regex.Replace(ret, "(?<![\w.-])[\w.+-]+@[\w.-]+\.[A-Z]{2,}(?![\w.-])", "<EMAIL>", invariantIgnoreCase)
        ret = Regex.Replace(ret, "(--?(?:api[-_]?key|authorization|cookie|key|pass(?:wd|word)|secret|token)\s*[=:]?\s*)(?:(""[^""]*"")|\S+)", "$1<REDACTED>", invariantIgnoreCase)
        ret = Regex.Replace(ret, "\b(Bearer|Basic)\s+[A-Z0-9._~+/=-]+", "$1 <REDACTED>", invariantIgnoreCase)
        ret = Regex.Replace(ret, """(?:[A-Z]:\\|\\\\)[^""]*""", "<PATH>", invariantIgnoreCase)
        ret = Regex.Replace(ret, "(?<!\S)(?:[A-Z]:\\|\\\\).*?(?=\s+(?:--?[A-Z0-9]|/[A-Z?]|\|)|$)", "<PATH>", invariantIgnoreCase)
        ret = Regex.Replace(ret, "\s+", " ").Trim()
        ret = ret.Replace("```", "'''")

        If ret.Length > 4000 Then ret = ret.Substring(0, 4000) + " <TRUNCATED>"
        Return ret
    End Function

    Private Shared Sub AppendReporterPrompts(sb As StringBuilder)
        AppendHeading(sb, "Reporter notes")
        sb.AppendLine("- Observed behavior:")
        sb.AppendLine("- Expected behavior:")
        sb.AppendLine("- Corruption or failure symptom:")
        sb.AppendLine("- Reproduction frequency: Always / Intermittent / Once")
        sb.AppendLine("- First failing StaxRip version:")
        sb.AppendLine("- Last known working StaxRip version:")
        sb.AppendLine("- Reproduces with a clean template: Yes / No / Not tested")
        sb.AppendLine("- Reproduces with another source: Yes / No / Not tested")
        sb.AppendLine("- Reproduces when the equivalent command is run outside StaxRip: Yes / No / Not tested")
        sb.AppendLine("- Earliest affected timestamp or processing stage:")
        sb.AppendLine("- Output validation performed (player, decoder check, MediaInfo, or comparison):")
        sb.AppendLine()
    End Sub

    Private Shared Sub AppendPrivacySummary(sb As StringBuilder, commandsIncluded As Boolean)
        AppendHeading(sb, "Privacy review")
        AppendItem(sb, "Raw log", "Not included")
        AppendItem(sb, "Paths and file names", "Not included")
        AppendItem(sb, "Media and stream titles", "Not included")
        AppendItem(sb, "Template and profile names", "Not included")
        AppendItem(sb, "Scripts and custom text", "Not included")
        AppendItem(sb, "Sanitized command summaries", If(commandsIncluded, "Included; manual review required", "Not included"))
    End Sub

    Private Shared Sub AppendCommand(sb As StringBuilder, heading As String, command As String)
        sb.AppendLine($"#### {heading}")
        sb.AppendLine()
        sb.AppendLine("```text")
        sb.AppendLine(command)
        sb.AppendLine("```")
        sb.AppendLine()
    End Sub

    Private Shared Sub AppendHeading(sb As StringBuilder, heading As String)
        If sb.Length > 0 AndAlso Not sb.ToString().EndsWith(Environment.NewLine + Environment.NewLine, StringComparison.Ordinal) Then
            sb.AppendLine()
        End If

        sb.AppendLine($"### {heading}")
        sb.AppendLine()
    End Sub

    Private Shared Sub AppendItem(sb As StringBuilder, name As String, value As String)
        sb.AppendLine($"- {name}: {SafeTechnicalValue(value)}")
    End Sub

    Private Shared Sub AppendOptionalItem(sb As StringBuilder, name As String, value As String)
        If Not String.IsNullOrWhiteSpace(value) Then AppendItem(sb, name, value)
    End Sub

    Private Shared Function SafeTechnicalValue(value As String) As String
        If String.IsNullOrWhiteSpace(value) Then Return "Not available"

        If Regex.IsMatch(value, "(?:[A-Z]:\\|\\\\|/Users/|/home/|\b(?:https?|ftp)://)", RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant) Then
            Return "Omitted"
        End If

        Dim ret = Regex.Replace(value, "\s+", " ").Trim()
        ret = Regex.Replace(ret, "[^\p{L}\p{Nd} .,_+\-/:;=()\[\]#]", "?")
        If ret.Length > 160 Then ret = ret.Substring(0, 160) + "..."
        Return ret
    End Function

    Private Shared Function GetFrameServer(proj As Project) As String
        If proj.Script Is Nothing Then Return "Not configured"
        If proj.SkipVideoEncoding AndAlso Not proj.UseScriptAsAudioSource Then Return "Not used by the configured encode"
        Return proj.Script.Engine.ToString()
    End Function

    Private Shared Function GetFileType(path As String) As String
        If String.IsNullOrWhiteSpace(path) Then Return "Not available"

        Try
            Dim extension = IO.Path.GetExtension(path).TrimStart("."c)
            Return If(extension = "", "No extension", extension.ToLowerInvariant())
        Catch
            Return "Not available"
        End Try
    End Function

    Private Shared Function GetTypeName(value As Object) As String
        Return If(value Is Nothing, "Not configured", value.GetType().Name)
    End Function

    Private Shared Function FormatVersion(version As Version) As String
        Return $"{version.Major}.{version.Minor}.{version.Build}"
    End Function

    Private Shared Function OptionalSuffix(value As String) As String
        Return If(String.IsNullOrWhiteSpace(value), "", " (" + value + ")")
    End Function

    Private Shared Function YesNo(value As Boolean) As String
        Return If(value, "Yes", "No")
    End Function

    Private Shared Function PresentOrAbsent(value As Boolean) As String
        Return If(value, "Present", "Not present")
    End Function

    Private Shared Sub AddTool(tools As IDictionary(Of String, String), package As Package)
        If package Is Nothing OrElse String.IsNullOrWhiteSpace(package.Name) Then Return
        tools(package.Name) = package.Version
    End Sub
End Class
