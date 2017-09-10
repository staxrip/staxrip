Imports System.Text.RegularExpressions
Imports Microsoft.Win32
Imports StaxRip

Public Class Package
    Implements IComparable(Of Package)

    Property Description As String
    Property DirPath As String
    Property DownloadURL As String
    Property FileNotFoundMessage As String
    Property HelpFile As String
    Property HelpURL As String
    Property HelpURLFunc As Func(Of ScriptEngine, String)
    Property HintDirFunc As Func(Of String)
    Property IgnoreVersion As Boolean
    Property IsRequiredFunc As Func(Of Boolean)
    Property LaunchName As String
    Property Name As String
    Property SetupFilename As String
    Property StatusFunc As Func(Of String)
    Property TreePath As String
    Property Version As String
    Property VersionDate As DateTime
    Property WebURL As String
    Property Filename As String

    Overridable Property FixedDir As String

    Property URL As String
        Get
            Return WebURL
        End Get
        Set(value As String)
            WebURL = value
            HelpURL = value
        End Set
    End Property

    Private SetupActionValue As Action

    Property SetupAction As Action
        Get
            If SetupActionValue Is Nothing AndAlso File.Exists(Folder.Apps + SetupFilename) Then
                SetupActionValue = Sub() g.StartProcess(Folder.Apps + SetupFilename)
            End If

            Return SetupActionValue
        End Get
        Set(value As Action)
            SetupActionValue = value
        End Set
    End Property

    Shared Property Items As New SortedDictionary(Of String, Package)

    Shared Property Python As Package = Add(New PythonPackage)

    Shared Property checkmate As Package = Add(New checkmatePackage)
    Shared Property DGIndexIM As Package = Add(New DGIndexIMPackage)
    Shared Property DGIndexNV As Package = Add(New DGIndexNVPackage)
    Shared Property dsmux As Package = Add(New dsmuxPackage)

    Shared Property eac3to As Package = Add(New Package With {
        .Name = "eac3to",
        .Filename = "eac3to.exe",
        .WebURL = "http://forum.doom9.org/showthread.php?t=125966",
        .HelpURL = "http://en.wikibooks.org/wiki/Eac3to/How_to_Use",
        .Description = "Audio conversion command line app."})

    Shared Property ffmpeg As Package = Add(New Package With {
        .Name = "ffmpeg",
        .Filename = "ffmpeg.exe",
        .WebURL = "http://ffmpeg.org",
        .HelpURL = "https://www.ffmpeg.org/ffmpeg-all.html",
        .Description = "Versatile audio video converter."})

    Shared Property Haali As Package = Add(New HaaliSplitter)
    Shared Property Java As Package = Add(New JavaPackage)

    Shared Property MediaInfo As Package = Add(New Package With {
        .Name = "MediaInfo",
        .Filename = "MediaInfo.dll",
        .WebURL = "http://mediainfo.sourceforge.net",
        .Description = "MediaInfo is used by StaxRip to read infos from media files."})

    Shared Property MP4Box As Package = Add(New Package With {
        .Name = "MP4Box",
        .Filename = "MP4Box.exe",
        .WebURL = "http://gpac.wp.mines-telecom.fr/",
        .HelpURL = "http://gpac.wp.mines-telecom.fr/mp4box/mp4box-documentation",
        .Description = "MP4Box is a MP4 muxing and demuxing command line app."})

    Shared Property AviSynth As Package = Add(New AviSynthPlusPackage)
    Shared Property NicAudio As Package = Add(New NicAudioPackage)
    Shared Property ProjectX As Package = Add(New ProjectXPackage)
    Shared Property qaac As Package = Add(New qaacPackage)
    Shared Property UnDot As Package = Add(New UnDotPackage)
    Shared Property xvid_encraw As Package = Add(New xvid_encrawPackage)
    Shared Property Decomb As Package = Add(New DecombPackage)
    Shared Property temporalsoften As Package = Add(New temporalsoftenPackage)

    Shared Property fdkaac As Package = Add(New Package With {
        .Name = "fdkaac",
        .Filename = "fdkaac.exe",
        .HelpFile = "help.txt",
        .Description = "Command line AAC encoder based on libfdk-aac.",
        .URL = "https://github.com/nu774/fdkaac",
        .IsRequiredFunc = Function() TypeOf p.Audio0 Is GUIAudioProfile AndAlso DirectCast(p.Audio0, GUIAudioProfile).Params.Encoder = GuiAudioEncoder.fdkaac OrElse TypeOf p.Audio1 Is GUIAudioProfile AndAlso DirectCast(p.Audio1, GUIAudioProfile).Params.Encoder = GuiAudioEncoder.fdkaac})

    Shared Property vspipe As Package = Add(New Package With {
        .Name = "vspipe",
        .Filename = "vspipe.exe",
        .Description = "vspipe is installed by VapourSynth and used to pipe VapourSynth scripts to encoding apps.",
        .WebURL = "http://www.vapoursynth.com/doc/vspipe.html",
        .DownloadURL = "http://github.com/vapoursynth/vapoursynth/releases",
        .IsRequiredFunc = Function() p.Script.Engine = ScriptEngine.VapourSynth,
        .HintDirFunc = Function() Registry.LocalMachine.GetString("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\VapourSynth_is1", "Inno Setup: App Path") + "\core64\"})

    Shared Property VapourSynth As Package = Add(New Package With {
        .Name = "VapourSynth",
        .Filename = "vapoursynth.dll",
        .Description = "StaxRip x64 supports both AviSynth+ x64 and VapourSynth x64 as scripting based video processing tool.",
        .WebURL = "http://www.vapoursynth.com",
        .HelpURL = "http://www.vapoursynth.com/doc",
        .DownloadURL = "http://github.com/vapoursynth/vapoursynth/releases",
        .IsRequiredFunc = Function() p.Script.Engine = ScriptEngine.VapourSynth,
        .HintDirFunc = Function() Registry.LocalMachine.GetString("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\VapourSynth_is1", "Inno Setup: App Path") + "\core64\"})

    Shared Property DGIndex As Package = Add(New Package With {
        .Name = "DGIndex",
        .Filename = "DGIndex.exe",
        .LaunchName = "DGIndex.exe",
        .HelpFile = "DGIndexManual.html",
        .Description = "MPEG-2 demuxing and indexing app.",
        .WebURL = "http://rationalqm.us/dgmpgdec/dgmpgdec.html"})

    Shared Property BDSup2SubPP As Package = Add(New Package With {
        .Name = "BDSup2Sub++",
        .Filename = "bdsup2sub++.exe",
        .LaunchName = "bdsup2sub++.exe",
        .WebURL = "http://forum.doom9.org/showthread.php?p=1613303",
        .Description = "Converts Blu-ray subtitles to other formats like VobSub."})

    Shared Property SubtitleEdit As Package = Add(New Package With {
        .Name = "SubtitleEdit",
        .Filename = "SubtitleEdit.exe",
        .LaunchName = "SubtitleEdit.exe",
        .HintDirFunc = Function() "C:\Program Files\Subtitle Edit\",
        .WebURL = "http://www.nikse.dk/SubtitleEdit",
        .HelpURL = "http://www.nikse.dk/SubtitleEdit/Help",
        .IsRequired = False,
        .IgnoreVersion = True,
        .Description = "Subtitle Edit is a open source subtitle editor."})

    Shared Property mpvnet As Package = Add(New Package With {
        .Name = "mpv.net",
        .Filename = "mpvnet.exe",
        .LaunchName = "mpvnet.exe",
        .URL = "https://github.com/stax76/mpvnet",
        .Description = "libmpv based media player."})

    Shared Property AOMEnc As Package = Add(New Package With {
        .Name = "aomenc",
        .Filename = "aomenc.exe",
        .WebURL = "http://aomedia.org",
        .IsRequiredFunc = Function() TypeOf p.VideoEncoder Is AOMEnc,
        .Description = "AOMedia Video 1 (AV1) is an open, royalty-free video coding format designed for video transmissions over the Internet."})

    Shared Property TDeint As Package = Add(New PluginPackage With {
        .Name = "TDeint",
        .Filename = "TDeint.dll",
        .WebURL = "http://avisynth.nl/index.php/TDeint",
        .Description = "TDeint is a bi-directionally, motion adaptive, sharp deinterlacer. It can adaptively choose between using per-field and per-pixel motion adaptivity, and can use cubic interpolation, kernel interpolation (with temporal direction switching), or one of two forms of modified ELA interpolation which help to reduce ""jaggy"" edges in moving areas where interpolation must be used.",
        .AviSynthFilterNames = {"TDeint"}})

    Shared Property modPlus As Package = Add(New PluginPackage With {
        .Name = "modPlus",
        .Filename = "modPlus.dll",
        .URL = "http://www.avisynth.nl/users/vcmohan/modPlus/modPlus.html",
        .Description = "This plugin has 9 functions, which modify values of color components to attenuate noise, blur or equalize input.",
        .AviSynthFilterNames = {"GBlur", "MBlur", "Median", "minvar", "Morph", "SaltPepper", "SegAmp", "TweakHist", "Veed"}})

    Shared Property AutoAdjust As Package = Add(New PluginPackage With {
        .Name = "AutoAdjust",
        .Filename = "AutoAdjust.dll",
        .URL = "https://forum.doom9.org/showthread.php?t=167573",
        .Description = "AutoAdjust is an automatic adjustement filter. It calculates statistics of clip, stabilizes them temporally and uses them to adjust luminance gain & color balance.",
        .AviSynthFilterNames = {"AutoAdjust"}})

    Shared Property SmoothAdjust As Package = Add(New PluginPackage With {
        .Name = "SmoothAdjust",
        .Filename = "SmoothAdjust.dll",
        .URL = "https://forum.doom9.org/showthread.php?t=154971",
        .Description = "SmoothAdjust is a set of 5 plugins to make YUV adjustements.",
        .AviSynthFilterNames = {"SmoothTweak", "SmoothCurve", "SmoothCustom", "SmoothTools"}})

    Shared Property EEDI2 As Package = Add(New PluginPackage With {
        .Name = "EEDI2",
        .Filename = "EEDI2.dll",
        .URL = "http://avisynth.nl/index.php/EEDI2",
        .Description = "EEDI2 (Enhanced Edge Directed Interpolation) resizes an image by 2x in the vertical direction by copying the existing image to 2*y(n) and interpolating the missing field.",
        .AviSynthFilterNames = {"EEDI2"},
        .AviSynthFiltersFunc = Function() {New VideoFilter("Field", "EEDI2", "EEDI2()")}})

    Shared Property VSRip As Package = Add(New Package With {
        .Name = "VSRip",
        .Filename = "VSRip.exe",
        .Description = "VSRip rips VobSub subtitles.",
        .WebURL = "http://sourceforge.net/projects/guliverkli",
        .LaunchName = "VSRip.exe"})

    Shared Property flash3kyuu_deband As Package = Add(New PluginPackage With {
        .Name = "flash3kyuu_deband",
        .Filename = "flash3kyuu_deband.dll",
        .WebURL = "http://forum.doom9.org/showthread.php?t=161411",
        .HelpURL = "https://f3kdb.readthedocs.io/en/latest/#",
        .Description = "Simple debanding filter that can be quite effective for some anime sources.",
        .AviSynthFilterNames = {"f3kdb"},
        .AviSynthFiltersFunc = Function() {New VideoFilter("Misc", "f3kdb", "f3kdb()")},
        .VapourSynthFilterNames = {"f3kdb.Deband"},
        .VapourSynthFiltersFunc = Function() {New VideoFilter("Misc", "f3kdb...", "clip = core.f3kdb.Deband(clip, preset = ""$select:msg:Select a preset.;depth;low;medium;high;veryhigh;nograin;luma;chroma$"")")}})

    Shared Property vinverse As Package = Add(New PluginPackage With {
        .Name = "vinverse",
        .Filename = "vinverse.dll",
        .WebURL = "http://avisynth.nl/index.php/Vinverse",
        .Description = "A modern rewrite of a simple but effective plugin to remove residual combing originally based on an AviSynth script by Didée and then written as a plugin by tritical.",
        .AviSynthFilterNames = {"vinverse", "vinverse2"}})

    Shared Property scenechange As Package = Add(New PluginPackage With {
        .Name = "scenechange",
        .Filename = "scenechange.dll",
        .VapourSynthFilterNames = {"scenechange"}})

    Shared Property avs2pipemod As Package = Add(New Package With {
        .Name = "avs2pipemod",
        .Filename = "avs2pipemod64.exe",
        .WebURL = "https://github.com/chikuzen/avs2pipemod",
        .Description = "Given an AviSynth script as input, avs2pipemod can send video, audio, or information of various types to stdout for consumption by command line encoders or other tools."})

    Shared ReadOnly Property x264 As Package
        Get
            If IsX264_10Required() Then Return x264_10 Else Return x264_8
        End Get
    End Property

    Shared Property x264_8 As Package = Add(New Package With {
        .Name = "x264",
        .Filename = "x264.exe",
        .Description = "H.264 video encoding command line app.",
        .WebURL = "http://www.videolan.org/developers/x264.html",
        .HelpURL = "http://www.chaneru.com/Roku/HLS/X264_Settings.htm"})

    Shared Property x264_10 As Package = Add(New Package With {
        .Name = "x264 10-Bit",
        .Filename = "x264-10bit.exe",
        .DirPath = "x264",
        .HelpURL = "http://www.chaneru.com/Roku/HLS/X264_Settings.htm",
        .WebURL = "http://www.videolan.org/developers/x264.html",
        .Description = "H.264 video encoding command line app."})

    Shared Function IsX264_10Required() As Boolean
        Return TypeOf p.VideoEncoder Is x264Enc AndAlso DirectCast(p.VideoEncoder, x264Enc).Params.Depth.Value = 1
    End Function

    Shared Property x265 As Package = Add(New Package With {
        .Name = "x265",
        .Filename = "x265.exe",
        .WebURL = "http://x265.org",
        .HelpURL = "http://x265.readthedocs.org",
        .Description = "H.265 video encoding command line app."})

    Shared Property mkvmerge As Package = Add(New Package With {
        .Name = "mkvmerge",
        .Filename = "mkvmerge.exe",
        .DirPath = "MKVToolNix",
        .WebURL = "http://www.bunkus.org/videotools/mkvtoolnix",
        .HelpURL = "http://www.bunkus.org/videotools/mkvtoolnix/docs.html",
        .Description = "MKV muxing tool."})

    Shared Property mkvextract As Package = Add(New Package With {
        .Name = "mkvextract",
        .Filename = "mkvextract.exe",
        .DirPath = "MKVToolNix",
        .WebURL = "http://www.bunkus.org/videotools/mkvtoolnix",
        .HelpURL = "http://www.bunkus.org/videotools/mkvtoolnix/docs.html",
        .Description = "MKV demuxing tool."})

    Shared Property NVEnc As Package = Add(New Package With {
        .Name = "NVEnc",
        .Filename = "NVEncC64.exe",
        .WebURL = "https://github.com/rigaya/NVEnc",
        .DownloadURL = "https://onedrive.live.com/?cid=6bdd4375ac8933c6&id=6BDD4375AC8933C6!2293",
        .Description = "NVIDIA hardware video encoder.",
        .HelpFile = "help.txt"})

    Shared Property QSVEnc As Package = Add(New Package With {
        .Name = "QSVEnc",
        .Filename = "QSVEncC64.exe",
        .Description = "Intel hardware video encoder.",
        .HelpFile = "help.txt",
        .DownloadURL = "https://onedrive.live.com/?cid=6bdd4375ac8933c6&id=6BDD4375AC8933C6!482",
        .WebURL = "https://github.com/rigaya/QSVEnc"})

    Shared Property VCEEnc As Package = Add(New Package With {
        .Name = "VCEEnc",
        .Filename = "VCEEncC64.exe",
        .Description = "AMD hardware video encoder.",
        .HelpFile = "help.txt",
        .DownloadURL = "https://onedrive.live.com/?id=6BDD4375AC8933C6!516&cid=6BDD4375AC8933C6",
        .WebURL = "https://github.com/rigaya/VCEEnc"})

    Shared Property DGDecodeNV As Package = Add(New PluginPackage With {
        .Name = "DGDecodeNV",
        .Filename = "DGDecodeNV.dll",
        .WebURL = "http://rationalqm.us/dgdecnv/dgdecnv.html",
        .Description = Strings.DGDecNV,
        .HelpFile = "DGDecodeNVManual.html",
        .IsRequiredFunc = Function() p.Script.Filters(0).Script.Contains("DGSource("),
        .AviSynthFilterNames = {"DGSource"},
        .AviSynthFiltersFunc = Function() {New VideoFilter("Source", "DGSource", "DGSource(""%source_file%"")")},
        .VapourSynthFiltersFunc = Function() {New VideoFilter("Source", "DGSource", "clip = core.avs.DGSource(r""%source_file%"")")}})

    Shared Property FFT3DFilter As Package = Add(New PluginPackage With {
        .Name = "FFT3DFilter",
        .Filename = "fft3dfilter.dll",
        .URL = "https://github.com/pinterf/fft3dfilter",
        .Description = "FFT3DFilter uses Fast Fourier Transform method for image processing in frequency domain.",
        .AviSynthFilterNames = {"FFT3DFilter"},
        .AviSynthFiltersFunc = Function() {New VideoFilter("Noise", "FFT3DFilter", "FFT3DFilter()")}})

    Shared Property DGDecodeIM As Package = Add(New PluginPackage With {
        .Name = "DGDecodeIM",
        .Filename = "DGDecodeIM.dll",
        .WebURL = "http://rationalqm.us/mine.html",
        .Description = Strings.DGDecIM,
        .HelpFile = "Notes.txt",
        .IsRequiredFunc = Function() p.Script.Filters(0).Script.Contains("DGSourceIM("),
        .AviSynthFilterNames = {"DGSourceIM"},
        .AviSynthFiltersFunc = Function() {New VideoFilter("Source", "DGSourceIM", "DGSourceIM(""%source_file%"")")}})

    Shared Property ffms2 As Package = Add(New PluginPackage With {
        .Name = "ffms2",
        .Filename = "ffms2.dll",
        .WebURL = "https://github.com/FFMS/ffms2",
        .Description = "AviSynth+ and VapourSynth source filter supporting various input formats.",
        .HelpURLFunc = Function(engine) If(engine = ScriptEngine.AviSynth, "https://github.com/FFMS/ffms2/blob/master/doc/ffms2-avisynth.md", "https://github.com/FFMS/ffms2/blob/master/doc/ffms2-vapoursynth.md"),
        .AviSynthFilterNames = {"FFVideoSource", "FFAudioSource"},
        .AviSynthFiltersFunc = Function() {New VideoFilter("Source", "FFVideoSource", "FFVideoSource(""%source_file%"", colorspace = ""YV12"", \" + BR + "              cachefile = ""%source_temp_file%.ffindex"")")},
        .VapourSynthFilterNames = {"ffms2"},
        .VapourSynthFiltersFunc = Function() {New VideoFilter("Source", "ffms2", "clip = core.ffms2.Source(r""%source_file%"", cachefile = r""%source_temp_file%.ffindex"")")}})

    Shared Function Add(pack As Package) As Package
        Items(pack.ID) = pack
        Return pack
    End Function

    Shared Sub New()
        Add(New Package With {
            .Name = "Visual C++ 2012",
            .Filename = "msvcp110.dll",
            .Description = "Visual C++ 2012 Redistributable is required by some tools used by StaxRip.",
            .DownloadURL = "https://www.microsoft.com/en-US/download/details.aspx?id=30679",
            .FixedDir = Folder.System,
            .IgnoreVersion = True,
            .IsRequiredFunc = Function() Items("SangNom2 avs").IsRequired,
            .TreePath = "Runtimes"})

        Add(New Package With {
            .Name = "Visual C++ 2013",
            .Filename = "msvcp120.dll",
            .Description = "Visual C++ 2013 Redistributable is required by some tools used by StaxRip.",
            .DownloadURL = "https://www.microsoft.com/en-US/download/details.aspx?id=40784",
            .IgnoreVersion = True,
            .FixedDir = Folder.System,
            .TreePath = "Runtimes"})

        Add(New Package With {
            .Name = "Visual C++ 2017",
            .Filename = "msvcp140.dll",
            .Description = "Visual C++ 2017 Redistributable is required by some tools used by StaxRip.",
            .DownloadURL = "http://download.microsoft.com/download/8/9/d/89d195e1-1901-4036-9a75-fbe46443fc5a/vc_redist.x64.exe",
            .FixedDir = Folder.System,
            .IgnoreVersion = True,
            .TreePath = "Runtimes"})

        Add(New Package With {
            .Name = "FFTW",
            .Filename = "libfftw3-3.dll",
            .Description = "Library required by the FFT3DFilter AviSynth plugin.",
            .URL = "http://www.fftw.org/",
            .FixedDir = Folder.System,
            .IsRequiredFunc = Function() Package.FFT3DFilter.IsRequired,
            .SetupAction = Sub()
                               Using pr As New Process
                                   pr.StartInfo.FileName = "xcopy.exe"
                                   pr.StartInfo.Arguments = $"""{Folder.Apps + "FFTW\"}*ff*"" ""{Folder.System}"" /Y"
                                   pr.StartInfo.Verb = "runas"
                                   pr.Start()
                                   pr.WaitForExit()
                                   If pr.ExitCode <> 0 Then MsgError("FFTW returned an error.")
                               End Using
                           End Sub})

        Add(New Package With {
            .Name = "AVSMeter",
            .Filename = "AVSMeter64.exe",
            .Description = "AVSMeter runs an Avisynth script with virtually no overhead, displays clip info, CPU and memory usage and the minimum, maximum and average frames processed per second. It measures how fast Avisynth can serve frames to a client application like x264 and comes in handy when testing filters/plugins to evaluate their performance and memory requirements.",
            .StartActionValue = Sub()
                                    If p.SourceFile = "" Then
                                        g.DefaultCommands.ExecuteCommandLine(Package.Items("AVSMeter").Path.Escape + " -avsinfo" + BR + "pause", False, False, True)
                                    Else
                                        g.DefaultCommands.ExecuteCommandLine(Package.Items("AVSMeter").Path.Escape + " " + p.Script.Path.Escape + BR + "pause", False, False, True)
                                    End If
                                End Sub,
            .HelpFile = "doc\AVSMeter.html",
            .WebURL = "https://forum.doom9.org/showthread.php?t=174797"})

        Add(New PluginPackage With {
            .Name = "KNLMeansCL",
            .Filename = "KNLMeansCL.dll",
            .WebURL = "https://github.com/Khanattila/KNLMeansCL",
            .Description = "KNLMeansCL is an optimized pixelwise OpenCL implementation of the Non-local means denoising algorithm. Every pixel is restored by the weighted average of all pixels in its search window. The level of averaging is determined by the filtering parameter h.",
            .VapourSynthFilterNames = {"knlm.KNLMeansCL"},
            .AviSynthFilterNames = {"KNLMeansCL"},
            .AviSynthFiltersFunc = Function() {
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Light", "KNLMeansCL(D = 1, A = 1, h = 2)"),
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Medium", "KNLMeansCL(D = 1, A = 1, h = 4)"),
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Strong", "KNLMeansCL(D = 1, A = 1, h = 8)")},
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Light", "clip = core.knlm.KNLMeansCL(clip, d = 1, a = 1, h = 2)"),
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Medium", "clip = core.knlm.KNLMeansCL(clip, d = 1, a = 1, h = 4)"),
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Strong", "clip = core.knlm.KNLMeansCL(clip, d = 1, a = 1, h = 8)")}})

        Add(New PluginPackage With {
            .Name = "SangNom2",
            .Filename = "SangNom2.dll",
            .WebURL = "http://avisynth.nl/index.php/SangNom2",
            .Description = "SangNom2 is a reimplementation of MarcFD's old SangNom filter. Originally it's a single field deinterlacer using edge-directed interpolation but nowadays it's mainly used in anti-aliasing scripts. The output is not completely but mostly identical to the original SangNom.",
            .AviSynthFilterNames = {"SangNom2"}})

        Add(New PluginPackage With {
            .Name = "mvtools2",
            .Filename = "mvtools2.dll",
            .WebURL = "https://github.com/pinterf/mvtools",
            .HelpURL = "http://avisynth.org.ru/mvtools/mvtools2.html",
            .Description = "MVTools is collection of functions for estimation and compensation of objects motion in video clips. Motion compensation may be used for strong temporal denoising, advanced framerate conversions, image restoration and other tasks.",
            .AviSynthFilterNames = {"MSuper", "MAnalyse", "MCompensate", "MMask", "MDeGrain1", "MDeGrain2", "MDegrain3"}})

        Add(New PluginPackage With {
            .Name = "MPEG2DecPlus",
            .Filename = "MPEG2DecPlus64.dll",
            .WebURL = "https://github.com/chikuzen/MPEG2DecPlus",
            .Description = "Source filter to open D2V index files created with DGIndex or D2VWitch.",
            .AviSynthFilterNames = {"MPEG2Source"},
            .AviSynthFiltersFunc = Function() {New VideoFilter("Source", "MPEG2Source", "MPEG2Source(""%source_file%"")")}})

        Add(New PluginPackage With {
            .Name = "DSS2mod",
            .Filename = "DSS2.dll",
            .WebURL = "http://code.google.com/p/xvid4psp/downloads/detail?name=DSS2%20mod%20%2B%20LAVFilters.7z&can=2&q=",
            .Description = "Direct Show source filter",
            .AviSynthFilterNames = {"DSS2"},
            .AviSynthFiltersFunc = Function() {New VideoFilter("Source", "DSS2", "DSS2(""%source_file%"")")}})

        Add(New PluginPackage With {
            .Name = "L-SMASH-Works",
            .Filename = "LSMASHSource.dll",
            .Description = "AviSynth and VapourSynth source filter based on Libav supporting a wide range of input formats.",
            .WebURL = "http://avisynth.nl/index.php/LSMASHSource",
            .HelpURL = "https://github.com/VFR-maniac/L-SMASH-Works/blob/master/AviSynth/README",
            .AviSynthFilterNames = {"LSMASHVideoSource", "LSMASHAudioSource", "LWLibavVideoSource", "LWLibavAudioSource"},
            .AviSynthFiltersFunc = Function() {
                New VideoFilter("Source", "LSMASHVideoSource", "LSMASHVideoSource(""%source_file%"", format = ""YUV420P8"")"),
                New VideoFilter("Source", "LWLibavVideoSource", "LWLibavVideoSource(""%source_file%"", format = ""YUV420P8"")")}})

        Add(New PluginPackage With {
            .Name = "vslsmashsource",
            .Filename = "vslsmashsource.dll",
            .Description = "VapourSynth source filter based on Libav supporting a wide range of input formats.",
            .HelpURL = "https://github.com/VFR-maniac/L-SMASH-Works/blob/master/VapourSynth/README",
            .WebURL = "http://avisynth.nl/index.php/LSMASHSource",
            .VapourSynthFilterNames = {"lsmas.LibavSMASHSource", "lsmas.LWLibavSource"},
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Source", "LibavSMASHSource", "clip = core.lsmas.LibavSMASHSource(r""%source_file%"")"),
                New VideoFilter("Source", "LWLibavSource", "clip = core.lsmas.LWLibavSource(r""%source_file%"")")}})

        Add(New PluginPackage With {
            .Name = "Deblock",
            .Filename = "Deblock.dll",
            .Description = "Deblocking plugin using the deblocking filter of h264.",
            .URL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-Deblock/",
            .VapourSynthFilterNames = {"deblock.Deblock"},
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Misc", "Deblock", "clip = core.deblock.Deblock(clip, quant = 25, aoffset = 0, boffset = 0)")}})

        Add(New PluginPackage With {
            .Name = "MSharpen",
            .Filename = "msharpen.dll",
            .WebURL = "https://github.com/tp7/msharpen",
            .HelpURL = "http://forum.doom9.org/showthread.php?t=169832",
            .AviSynthFilterNames = {"MSharpen"},
            .AviSynthFiltersFunc = Function() {
                New VideoFilter("Misc", "MSharpen", "MSharpen(threshold = 10, strength = 100, highq = true, mask = false)")}})

        Add(New PluginPackage With {
            .Name = "aWarpSharp2",
            .Filename = "aWarpSharp.dll",
            .HelpFile = "aWarpSharp.txt",
            .AviSynthFilterNames = {"aBlur", "aSobel", "aWarp", "aWarp4", "aWarpSharp", "aWarpSharp2"},
            .Description = "This filter implements the same warp sharpening algorithm as aWarpSharp by Marc FD, but with several bugfixes and optimizations.",
            .WebURL = "http://avisynth.nl/index.php/AWarpSharp2",
            .AviSynthFiltersFunc = Function() {
                New VideoFilter("Misc", "aWarpSharp2", "aWarpSharp2(thresh = 128, blur = 2, type = 0, depth = 16, chroma = 4)")}})

        Add(New PluginPackage With {
            .Name = "VSFilterMod",
            .Filename = "VSFilterMod.dll",
            .Description = "AviSynth subtitle plugin with support for vobsub srt and ass.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VSFilterMod",
            .AviSynthFilterNames = {"VobSub", "TextSubMod"},
            .VapourSynthFilterNames = {"vsfm.VobSub", "vsfm.TextSubMod"}})

        Add(New PluginPackage With {
            .Name = "TComb",
            .Filename = "TComb.dll",
            .HelpFile = "ReadMe.txt",
            .AviSynthFilterNames = {"TComb"},
            .Description = "TComb is a temporal comb filter.",
            .WebURL = "http://avisynth.nl/index.php/TComb",
            .AviSynthFiltersFunc = Function() {New VideoFilter("Misc", "TComb", "TComb(mode = 0, fthreshL = 255, othreshL = 255)")}})

        Add(New PluginPackage With {
            .Name = "QTGMC",
            .Filename = "QTGMC.avsi",
            .URL = "http://avisynth.nl/index.php/QTGMC",
            .Description = "A very high quality deinterlacer with a range of features for both quality and convenience. These include a simple presets system, extensive noise processing capabilities, support for repair of progressive material, precision source matching, shutter speed simulation, etc. Originally based on TempGaussMC by Didée.",
            .AviSynthFilterNames = {"QTGMC"},
            .AviSynthFiltersFunc = Function() {New VideoFilter("Field", "QTGMC...", "QTGMC(Preset = ""$select:msg:Select a preset.;Draft;Ultra Fast;Super Fast;Very Fast;Faster;Fast;Medium;Slow;Slower;Very Slow;Placebo$"")")}})

        Add(New PluginPackage With {
            .Name = "SMDegrain",
            .Filename = "SMDegrain.avsi",
            .URL = "http://avisynth.nl/index.php/SMDegrain",
            .Description = "SMDegrain, the Simple MDegrain Mod, is mainly a convenience function for using MVTools.",
            .AviSynthFilterNames = {"SMDegrain"},
            .AviSynthFiltersFunc = Function() {New VideoFilter("Noise", "SMDegrain", "SMDegrain(tr = 2, thSAD = 250, contrasharp = false, refinemotion = true, lsb = false)")}})

        Add(New PluginPackage With {
            .Name = "mClean",
            .Filename = "mClean.avsi",
            .URL = "https://forum.doom9.org/showthread.php?t=174804",
            .Description = "Removes noise whilst retaining as much detail as possible.",
            .AviSynthFilterNames = {"mClean"},
            .AviSynthFiltersFunc = Function() {New VideoFilter("Noise", "mClean", "mClean()")}})

        Add(New PluginPackage With {
            .Name = "LSFmod",
            .Filename = "LSFmod.avsi",
            .URL = "http://avisynth.nl/index.php/LSFmod",
            .Description = "A LimitedSharpenFaster mod with a lot of new features and optimizations.",
            .AviSynthFilterNames = {"LSFmod"},
            .AviSynthFiltersFunc = Function() {New VideoFilter("Misc", "LSFmod", "LSFmod(strength = 100)")},
            .Dependencies = {"RgTools", "masktools2"}})

        Add(New PluginPackage With {
            .Name = "RgTools",
            .Filename = "RgTools.dll",
            .URL = "https://github.com/pinterf/RgTools",
            .Description = "RgTools is a modern rewrite of RemoveGrain, Repair, BackwardClense, Clense, ForwardClense and VerticalCleaner all in a single plugin.",
            .AviSynthFilterNames = {"RemoveGrain", "Clense", "ForwardClense", "BackwardClense", "Repair", "VerticalCleaner"},
            .AviSynthFiltersFunc = Function() {New VideoFilter("Noise", "RemoveGrain", "RemoveGrain()")}})

        Add(New PluginPackage With {
            .Name = "JPSDR",
            .Filename = "Plugins_JPSDR.dll",
            .WebURL = "https://forum.doom9.org/showthread.php?t=174248",
            .Description = "Merge of AutoYUY2, NNEDI3 and ResampleMT",
            .AviSynthFilterNames = {"nnedi3", "nnedi3_rpow2", "AutoYUY2", "PointResizeMT", "BilinearResizeMT", "BicubicResizeMT", "LanczosResizeMT", "Lanczos4ResizeMT", "BlackmanResizeMT", "Spline16ResizeMT", "Spline36ResizeMT", "Spline64ResizeMT", "GaussResizeMT", "SincResizeMT"},
            .AviSynthFiltersFunc = Function() {
                New VideoFilter("Field", "nnedi3", "nnedi3(field = 1)"),
                New VideoFilter("Resize", "Resize MT...", "$select:BicubicResizeMT;BilinearResizeMT;BlackmanResizeMT;GaussResizeMT;Lanczos4ResizeMT;LanczosResizeMT;PointResizeMT;SincResizeMT;Spline16ResizeMT;Spline36ResizeMT;Spline64ResizeMT$(%target_width%, %target_height%, prefetch = 4)")}})

        Add(New PluginPackage With {
            .Name = "TIVTC",
            .Filename = "TIVTC.dll",
            .WebURL = "https://github.com/pinterf/TIVTC",
            .HelpURL = "http://avisynth.nl/index.php/TIVTC",
            .Description = "TIVTC is a plugin package containing 7 different filters and 3 conditional functions.",
            .AviSynthFilterNames = {"TFM", "TDecimate", "MergeHints", "FrameDiff", "FieldDiff", "ShowCombedTIVTC", "RequestLinear"}})

        Add(New PluginPackage With {
            .Name = "masktools2",
            .Filename = "masktools2.dll",
            .URL = "https://github.com/pinterf/masktools",
            .Description = "MaskTools2 contain a set of filters designed to create, manipulate and use masks. Masks, in video processing, are a way to give a relative importance to each pixel. You can, for example, create a mask that selects only the green parts of the video, and then replace those parts with another video.",
            .AviSynthFilterNames = {"mt_adddiff", "mt_average", "mt_binarize", "mt_circle", "mt_clamp", "mt_convolution", "mt_diamond", "mt_edge", "mt_ellipse", "mt_expand", "mt_hysteresis", "mt_inflate", "mt_inpand", "mt_invert", "mt_logic", "mt_losange", "mt_lut", "mt_lutf", "mt_luts", "mt_lutxy", "mt_makediff", "mt_mappedblur", "mt_merge", "mt_motion", "mt_polish", "mt_rectangle", "mt_square"}})

        Add(New PluginPackage With {
            .Name = "FluxSmooth",
            .Filename = "FluxSmoothSSSE3.dll",
            .AviSynthFilterNames = {"FluxSmoothT", "FluxSmoothST"},
            .Description = "One of the fundamental properties of noise is that it's random. One of the fundamental properties of motion is that it's not. This is the premise behind FluxSmooth, which examines each pixel and compares it to the corresponding pixel in the previous and last frame. Smoothing occurs if both the previous frame's value and the next frame's value are greater, or if both are less, than the value in the current frame.",
            .WebURL = "http://avisynth.nl/index.php/FluxSmooth",
            .AviSynthFiltersFunc = Function() {
                New VideoFilter("Noise", "FluxSmoothT", "FluxSmoothT(temporal_threshold = 8)"),
                New VideoFilter("Noise", "FluxSmoothST", "FluxSmoothST(temporal_threshold = 8, spatial_threshold = 8)")}})

        Add(New PluginPackage With {
            .Name = "yadifmod2",
            .Filename = "yadifmod2.dll",
            .Description = "Yet Another Deinterlacing Filter mod  for Avisynth2.6/Avisynth+",
            .HelpURL = "https://github.com/chikuzen/yadifmod2/blob/master/avisynth/readme.md",
            .WebURL = "https://github.com/chikuzen/yadifmod2",
            .AviSynthFilterNames = {"yadifmod2"},
            .AviSynthFiltersFunc = Function() {New VideoFilter("Field", "yadifmod2", "yadifmod2()")}})

        Add(New PluginPackage With {
            .Name = "FrameRateConverter DLL",
            .Filename = "FrameRateConverter-x64.dll",
            .DirPath = "Plugins\AVS\FrameRateConverter",
            .Description = "Increases the frame rate with interpolation and fine artifact removal.",
            .URL = "https://github.com/mysteryx93/FrameRateConverter",
            .AviSynthFilterNames = {"FrameRateConverter"}})

        Add(New PluginPackage With {
            .Name = "FrameRateConverter AVSI",
            .Filename = "FrameRateConverter.avsi",
            .DirPath = "Plugins\AVS\FrameRateConverter",
            .Description = "Increases the frame rate with interpolation and fine artifact removal.",
            .URL = "https://github.com/mysteryx93/FrameRateConverter",
            .AviSynthFilterNames = {"FrameRateConverter"},
            .AviSynthFiltersFunc = Function() {New VideoFilter("Misc", "FrameRateConverter", "FrameRateConverter()")}})

        Add(New PluginPackage With {
            .Name = "DCTFilter",
            .Filename = "DCTFilter.dll",
            .Description = "A rewrite of DctFilter for Avisynth+.",
            .URL = "https://github.com/chikuzen/DCTFilter",
            .AviSynthFilterNames = {"DCTFilter", "DCTFilterD", "DCTFilter4", "DCTFilter4D", "DCTFilter8", "DCTFilter8D"}})

        Add(New PluginPackage With {
            .Name = "Yadifmod",
            .Filename = "Yadifmod.dll",
            .Description = "Modified version of Fizick's avisynth filter port of yadif from mplayer. This version doesn't internally generate spatial predictions, but takes them from an external clip.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-Yadifmod",
            .VapourSynthFilterNames = {"yadifmod.Yadifmod"},
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Field", "Yadifmod", "clip = core.yadifmod.Yadifmod(clip, core.nnedi3.nnedi3(clip, field = 0), order = 1, field = -1, mode = 0)")}})

        Add(New PluginPackage With {
            .Name = "GradCurve",
            .Filename = "GradCurve.dll",
            .URL = "https://github.com/xekon/GradCurve",
            .Description = "VapourSynth port of Gradation Curves Virtual Dub Plugin.",
            .VapourSynthFilterNames = {"grad.Curve"},
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Misc", "GradCurve", "clip = core.grad.Curve(clip, fname = ""path"", ftype = 1, pmode = 1)")}})

        Add(New PluginPackage With {
            .Name = "nnedi3",
            .Filename = "libnnedi3.dll",
            .WebURL = "http://github.com/dubhater/vapoursynth-nnedi3",
            .Description = "nnedi3 is an intra-field only deinterlacer. It takes in a frame, throws away one field, and then interpolates the missing pixels using only information from the kept field.",
            .VapourSynthFilterNames = {"nnedi3.nnedi3", "nnedi3.nnedi3_rpow2"},
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Field", "nnedi3", "clip = core.nnedi3.nnedi3(clip, field = 1)")}})

        Add(New PluginPackage With {
            .Name = "mvtools",
            .Filename = "libmvtools.dll",
            .WebURL = "http://github.com/dubhater/vapoursynth-mvtools",
            .Description = "MVTools is a set of filters for motion estimation and compensation.",
            .VapourSynthFilterNames = {"mv.Super", "mv.Analyse", "mv.Recalculate", "mv.Compensate", "mv.Degrain1", "mv.Degrain2",
                "mv.Degrain3", "mv.Mask", "mv.Finest", "mv.FlowBlur", "mv.FlowInter", "mv.FlowFPS", "mv.BlockFPS", "mv.SCDetection"}})

        Add(New PluginPackage With {
            .Name = "adjust",
            .Filename = "adjust.py",
            .VapourSynthFilterNames = {"adjust.Tweak"},
            .Description = "very basic port of the built-in Avisynth filter Tweak.",
            .URL = "https://github.com/dubhater/vapoursynth-adjust"})

        Add(New PluginPackage With {
            .Name = "mvsfunc",
            .Filename = "mvsfunc.py",
            .VapourSynthFilterNames = {"mvsfunc.Depth", "mvsfunc.ToRGB", "mvsfunc.ToYUV", "mvsfunc.BM3D",
                                       "mvsfunc.PlaneStatistics", "mvsfunc.PlaneCompare", "mvsfunc.ShowAverage",
                                       "mvsfunc.FilterIf", "mvsfunc.FilterCombed", "mvsfunc.Min", "mvsfunc.Max",
                                       "mvsfunc.Avg", "mvsfunc.MinFilter", "mvsfunc.MaxFilter", "mvsfunc.LimitFilter",
                                       "mvsfunc.PointPower", "mvsfunc.SetColorSpace", "mvsfunc.AssumeFrame",
                                       "mvsfunc.AssumeTFF", "mvsfunc.AssumeBFF", "mvsfunc.AssumeField",
                                       "mvsfunc.AssumeCombed", "mvsfunc.CheckVersion", "mvsfunc.GetMatrix",
                                       "mvsfunc.zDepth", "mvsfunc.GetPlane", "mvsfunc.PlaneAverage"},
            .Dependencies = {"fmtconv", "adjust"},
            .Description = "mawen1250's VapourSynth functions.",
            .HelpURL = "http://forum.doom9.org/showthread.php?t=172564",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/mvsfunc"})

        Add(New PluginPackage With {
            .Name = "havsfunc",
            .Filename = "havsfunc.py",
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Misc", "LSFmod", "clip = havsfunc.LSFmod(clip, strength = 100)"),
                New VideoFilter("Field", "QTGMC...", $"clip = core.std.SetFieldBased(clip, 2) # 1 = BFF, 2 = TFF{BR}clip = havsfunc.QTGMC(clip, TFF = True, Preset = ""$select:msg:Select a preset.;Draft;Ultra Fast;Super Fast;Very Fast;Faster;Fast;Medium;Slow;Slower;Very Slow;Placebo$"")")},
            .VapourSynthFilterNames = {"havsfunc.QTGMC", "havsfunc.ediaa", "havsfunc.daa", "havsfunc.maa",
                                       "havsfunc.SharpAAMCmod", "havsfunc.Deblock_QED", "havsfunc.DeHalo_alpha",
                                       "havsfunc.YAHR", "havsfunc.HQDeringmod", "havsfunc.ivtc_txt60mc",
                                       "havsfunc.Vinverse", "havsfunc.Vinverse2", "havsfunc.logoNR",
                                       "havsfunc.LUTDeCrawl", "havsfunc.LUTDeRainbow", "havsfunc.GSMC",
                                       "havsfunc.SMDegrain", "havsfunc.SmoothLevels", "havsfunc.FastLineDarkenMOD",
                                       "havsfunc.LSFmod", "havsfunc.GrainFactory3"},
            .Dependencies = {"fmtconv", "mvtools", "nnedi3", "scenechange", "temporalsoften", "mvsfunc", "KNLMeansCL"},
            .Description = "Various popular AviSynth scripts ported to VapourSynth.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/havsfunc",
            .HelpURL = "http://forum.doom9.org/showthread.php?t=166582"})

        Add(New PluginPackage With {
            .Name = "d2vsource",
            .Filename = "d2vsource.dll",
            .Description = "Source filter to open D2V index files created with DGIndex or D2VWitch.",
            .URL = "https://github.com/dwbuiten/d2vsource",
            .VapourSynthFilterNames = {"d2v.Source"},
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Source", "d2vsource", "clip = core.d2v.Source(r""%source_file%"")")}})

        Add(New PluginPackage With {
            .Name = "DeLogo",
            .Filename = "libdelogo.dll",
            .Description = "DeLogo Plugin Ported for VapourSynth.",
            .URL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DeLogo",
            .VapourSynthFilterNames = {"delogo.AddLogo", "delogo.EraseLogo"},
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Misc", "Delogo AddLogo", "clip = core.delogo.AddLogo(clip, logofile = r""$browse_file$"")"),
                New VideoFilter("Misc", "Delogo EraseLogo", "clip = core.delogo.EraseLogo(clip, logofile = r""$browse_file$"")")}})

        Add(New PluginPackage With {
            .Name = "FluxSmooth",
            .Filename = "libfluxsmooth.dll",
            .VapourSynthFilterNames = {"SmoothT", "SmoothST"},
            .Description = "FluxSmooth is a filter for smoothing of fluctuations.",
            .WebURL = "https://github.com/dubhater/vapoursynth-fluxsmooth",
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Noise", "FluxSmooth SmoothT", "clip = core.flux.SmoothT(clip, temporal_threshold = 7, planes = [0, 1, 2])"),
                New VideoFilter("Noise", "FluxSmooth SmoothST", "clip = core.flux.SmoothST(clip, temporal_threshold = 7, spatial_threshold = 7, planes = [0, 1, 2])")}})

        Add(New PluginPackage With {
            .Name = "msmoosh",
            .Filename = "libmsmoosh.dll",
            .VapourSynthFilterNames = {"msmoosh.MSmooth", "msmoosh.MSharpen"},
            .Description = "MSmooth is a spatial smoother that doesn't touch edges." + BR + "MSharpen is a sharpener that tries to sharpen only edges.",
            .WebURL = "https://github.com/dubhater/vapoursynth-msmoosh",
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Noise", "MSmooth", "clip = core.msmoosh.MSmooth(clip, threshold = 6.0, strength = 3)"),
                New VideoFilter("Misc", "MSharpen", "clip = core.msmoosh.MSharpen(clip, threshold = 6.0, strength = 39)")}})

        Add(New PluginPackage With {
            .Name = "fmtconv",
            .Filename = "fmtconv.dll",
            .WebURL = "http://github.com/EleonoreMizo/fmtconv",
            .HelpFile = "fmtconv.html",
            .Description = "Fmtconv is a format-conversion plug-in for the Vapoursynth video processing engine. It does resizing, bitdepth conversion with dithering and colorspace conversion.",
            .VapourSynthFilterNames = {"fmtc.bitdepth", "fmtc.convert", "fmtc.matrix", "fmtc.resample", "fmtc.transfer"}})

        Add(New PluginPackage With {
            .Name = "finesharp",
            .Filename = "finesharp.py",
            .Description = "Port of Didée's FineSharp script to VapourSynth.",
            .WebURL = "http://forum.doom9.org/showthread.php?p=1777860#post1777860",
            .VapourSynthFilterNames = {"finesharp.sharpen"},
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Misc", "finesharp", "clip = finesharp.sharpen(clip)")}})

        Dim fp = Folder.Settings + "Versions.txt"

        Try
            If Not File.Exists(fp) OrElse Not File.ReadAllText(fp).Contains(Application.ProductVersion + BR2) Then
                FileHelp.Delete(fp)
                fp = Folder.Apps + "Versions.txt"
            End If

            For Each line In File.ReadAllLines(fp)
                For Each pack In Items.Values
                    If line Like "*=*;*" Then
                        Dim name = line.Left("=").Trim

                        If name = pack.ID Then
                            pack.Version = line.Right("=").Right(";").Trim
                            Dim a = line.Right("=").Left(";").Trim.Split("-"c)
                            pack.VersionDate = New DateTime(CInt(a(0)), CInt(a(1)), CInt(a(2)))
                        End If
                    End If
                Next
            Next
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    ReadOnly Property ID As String
        Get
            If TypeOf Me Is PluginPackage Then
                Dim plugin = DirectCast(Me, PluginPackage)

                If Not plugin.AviSynthFilterNames.NothingOrEmpty AndAlso
                    Not plugin.VapourSynthFilterNames.NothingOrEmpty Then

                    Return Name + " avs+vs"
                ElseIf Not plugin.AviSynthFilterNames.NothingOrEmpty Then
                    Return Name + " avs"
                ElseIf Not plugin.VapourSynthFilterNames.NothingOrEmpty Then
                    Return Name + " vs"
                End If
            End If

            Return Name
        End Get
    End Property

    Private StartActionValue As Action

    Overridable ReadOnly Property StartAction As Action
        Get
            If LaunchName <> "" Then Return Sub() g.StartProcess(GetDir() + LaunchName)
            If Not StartActionValue Is Nothing Then Return StartActionValue
        End Get
    End Property

    Overridable ReadOnly Property LaunchTitle As String
        Get
            Return Name
        End Get
    End Property

    Private IsRequiredValue As Boolean = True

    Overridable Property IsRequired() As Boolean
        Get
            If Not IsRequiredFunc Is Nothing Then Return IsRequiredFunc.Invoke
            Return IsRequiredValue
        End Get
        Set(value As Boolean)
            IsRequiredValue = value
        End Set
    End Property

    Sub StartWithJava()
        Try
            Dim p As New Process
            p.StartInfo.FileName = Package.Java.GetDir + "javaw.exe"
            p.StartInfo.Arguments = "-jar """ + Path + """"
            p.StartInfo.WorkingDirectory = GetDir()
            p.Start()
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    Function GetHelpPath(Optional engine As ScriptEngine = ScriptEngine.AviSynth) As String
        If HelpFile <> "" Then
            Return GetDir() + HelpFile
        ElseIf HelpURL <> "" Then
            Return HelpURL
        ElseIf Not HelpURLFunc Is Nothing Then
            Return HelpURLFunc.Invoke(engine)
        ElseIf WebURL <> "" Then
            Return WebURL
        End If
    End Function

    Function VerifyOK(Optional showEvenIfNotRequired As Boolean = False) As Boolean
        If (IsRequired() OrElse showEvenIfNotRequired) AndAlso IsStatusCritical() Then
            Using f As New AppsForm
                f.ShowPackage(Me)
                f.ShowDialog()
            End Using

            If IsStatusCritical() Then Return False
        End If

        Return True
    End Function

    Function IsStatusCritical() As Boolean
        Return GetStatusLocation() <> "" OrElse GetStatus() <> ""
    End Function

    Overridable Function GetStatus() As String
        If IsOutdated() Then Return "Unsupported outdated version, continue with F12 if you must."
        If Not StatusFunc Is Nothing Then Return StatusFunc.Invoke
    End Function

    Function GetStatusDisplay() As String
        Dim ret = GetStatusLocation()
        If ret <> "" Then Return ret
        ret = GetStatus()
        If ret <> "" Then Return ret
        ret = GetStatusVersion()
        If ret <> "" Then Return ret
        Return "OK"
    End Function

    Function GetStatusVersion() As String
        If Not IsCorrectVersion() Then
            Dim text = If(IgnoreVersion, "OK", "Unknown version, press F12 to edit the version.")

            If SetupFilename = "" Then
                Return text
            Else
                Return text + " In case of problems download and install the required version."
            End If
        End If
    End Function

    Function GetAppNotFoundMessage() As String
        If FixedDir <> "" Then
            Return "App not found at '" + FixedDir.TrimEnd("\"c) + "'"
        Else
            Return "App not found, press F11 to locate the App."
        End If
    End Function

    Function GetStatusLocation() As String
        Dim pathVar = Path

        If pathVar = "" Then
            If FileNotFoundMessage <> "" Then
                Return GetAppNotFoundMessage() + " " + FileNotFoundMessage
            ElseIf SetupFilename <> "" Then
                Return "Please install " + Name + "."
            End If

            Return GetAppNotFoundMessage()
        End If

        If FixedDir <> "" AndAlso pathVar <> "" AndAlso Not pathVar.ToLower.StartsWith(FixedDir.ToLower) Then
            Return "The App has To be located at: " + FixedDir
        End If
    End Function

    Function IsOutdated() As Boolean
        Dim fp = Path

        If fp <> "" AndAlso Not IgnoreVersion Then
            If (VersionDate - File.GetLastWriteTimeUtc(fp)).TotalDays > 3 Then Return True
        End If
    End Function

    Overridable Function IsCorrectVersion() As Boolean
        Dim fp = Path

        If fp <> "" Then
            Dim dt = File.GetLastWriteTimeUtc(fp)
            Return dt.AddDays(-2) < VersionDate AndAlso dt.AddDays(2) > VersionDate
        End If
    End Function

    Function GetDir() As String
        Return Path.Dir
    End Function

    Sub SetPath(pathParam As String)
        s?.Storage?.SetString(Name + "custom path", pathParam)
    End Sub

    Overridable ReadOnly Property Path As String
        Get
            Dim ret As String

            If Not s Is Nothing AndAlso Not s.Storage Is Nothing Then
                ret = s.Storage.GetString(Name + "custom path")

                If ret <> "" Then
                    If File.Exists(ret) Then
                        Return ret
                    Else
                        s.Storage.SetString(Name + "custom path", Nothing)
                    End If
                End If
            End If

            If FixedDir <> "" Then
                If File.Exists(FixedDir + Filename) Then Return FixedDir + Filename
                Return Nothing
            End If

            If DirPath <> "" AndAlso File.Exists(Folder.Apps + DirPath + "\" + Filename) Then
                Return Folder.Apps + DirPath + "\" + Filename
            End If

            If Not HintDirFunc Is Nothing Then
                If File.Exists(HintDirFunc.Invoke + Filename) Then Return HintDirFunc.Invoke + Filename
            End If

            Dim plugin = TryCast(Me, PluginPackage)

            If Not plugin Is Nothing Then
                If Not plugin.VapourSynthFilterNames Is Nothing AndAlso Not plugin.AviSynthFilterNames Is Nothing Then
                    ret = Folder.Apps + "Plugins\both\" + Name + "\" + Filename
                    If File.Exists(ret) Then Return ret
                Else
                    If plugin.VapourSynthFilterNames Is Nothing Then
                        ret = Folder.Apps + "Plugins\avs\" + Name + "\" + Filename
                        If File.Exists(ret) Then Return ret
                    Else
                        ret = Folder.Apps + "Plugins\vs\" + Name + "\" + Filename
                        If File.Exists(ret) Then Return ret
                    End If
                End If
            End If

            ret = Folder.Apps + Name + "\" + Filename
            If File.Exists(ret) Then Return ret
        End Get
    End Property

    Overrides Function ToString() As String
        Return Name
    End Function

    Function CompareTo(other As Package) As Integer Implements System.IComparable(Of Package).CompareTo
        Return Name.CompareTo(other.Name)
    End Function
End Class

Public Class UnDotPackage
    Inherits PluginPackage

    Sub New()
        Name = "UnDot"
        Filename = "UnDot.dll"
        WebURL = "http://avisynth.nl/index.php/UnDot"
        Description = "UnDot is a simple median filter for removing dots, that is stray orphan pixels and mosquito noise."
        AviSynthFilterNames = {"UnDot"}
    End Sub
End Class

Public Class NicAudioPackage
    Inherits PluginPackage

    Sub New()
        Name = "NicAudio"
        Filename = "NicAudio.dll"
        WebURL = "http://avisynth.org.ru/docs/english/externalfilters/nicaudio.htm"
        Description = "AviSynth audio source filter."
        AviSynthFilterNames = {"NicAC3Source", "NicDTSSource", "NicMPASource", "RaWavSource"}
    End Sub
End Class

Public Class AviSynthPlusPackage
    Inherits Package

    Sub New()
        Name = "AviSynth+"
        Filename = "avisynth.dll"
        WebURL = "http://avisynth.nl/index.php/AviSynth%2B"
        Description = "StaxRip support both AviSynth+ x64 and VapourSynth x64 as scripting based video processing tool."
        FixedDir = Folder.System
        SetupFilename = "AviSynth+r2508.exe"
    End Sub

    Public Overrides Property IsRequired As Boolean
        Get
            Return p.Script.Engine = ScriptEngine.AviSynth
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public Overrides Function GetStatus() As String
        If Not Directory.Exists(Folder.Plugins) Then
            Return "The AviSynth+ plugins directory is missing, run the AviSynth+ setup."
        End If

        Return MyBase.GetStatus()
    End Function
End Class

Public Class PythonPackage
    Inherits Package

    Sub New()
        Name = "Python"
        Filename = "python.exe"
        TreePath = "Runtimes"
        WebURL = "http://www.python.org"
        Description = "Python x64 is required by VapourSynth x64. StaxRip x64 supports both AviSynth+ x64 and VapourSynth x64 as scripting based video processing tool."
        DownloadURL = "https://www.python.org/downloads/windows/"
    End Sub

    Public Overrides Property IsRequired As Boolean
        Get
            Return p.Script.Engine = ScriptEngine.VapourSynth
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public Overrides ReadOnly Property Path As String
        Get
            Dim ret = MyBase.Path
            If File.Exists(ret) Then Return ret

            For Each i In {
                Registry.CurrentUser.GetString("SOFTWARE\Python\PythonCore\3.6\InstallPath", "ExecutablePath"),
                Registry.LocalMachine.GetString("SOFTWARE\Python\PythonCore\3.6\InstallPath", "ExecutablePath"),
                Registry.CurrentUser.GetString("SOFTWARE\Python\PythonCore\3.6\InstallPath", Nothing).FixDir + "python.exe",
                Registry.LocalMachine.GetString("SOFTWARE\Python\PythonCore\3.6\InstallPath", Nothing).FixDir + "python.exe"}

                If File.Exists(i) Then Return i
            Next

            Dim paths = Environment.ExpandEnvironmentVariables("%Path%").SplitNoEmptyAndWhiteSpace(";").ToList
            paths.AddRange(Environment.ExpandEnvironmentVariables("%PATH%").SplitNoEmptyAndWhiteSpace(";"))

            For Each i In paths
                i = i.Trim(" "c, """"c).FixDir

                If File.Exists(i + "python.exe") Then
                    SetPath(i + "python.exe")
                    Return i + "python.exe"
                End If
            Next
        End Get
    End Property
End Class

Public Class PluginPackage
    Inherits Package

    Property AviSynthFilterNames As String()
    Property VapourSynthFilterNames As String()
    Property Dependencies As String()
    Property VapourSynthFiltersFunc As Func(Of VideoFilter())
    Property AviSynthFiltersFunc As Func(Of VideoFilter())

    Public Overrides Property IsRequired As Boolean
        Get
            Return IsPluginPackageRequired(Me)
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Shared Function IsPluginPackageRequired(package As PluginPackage) As Boolean
        If p Is Nothing Then Return False

        If p.Script.Engine = ScriptEngine.AviSynth AndAlso
            Not package.AviSynthFilterNames.NothingOrEmpty Then

            Dim fullScriptLower = p.Script.GetFullScript().ToLowerInvariant

            For Each filterName In package.AviSynthFilterNames
                If fullScriptLower.Contains(filterName.ToLowerInvariant) Then Return True

                If fullScriptLower.Contains("import") Then
                    Dim match = Regex.Match(fullScriptLower, "\bimport\s*\(\s*""\s*(.+\.avsi*)\s*""\s*\)",
                                            RegexOptions.IgnoreCase)

                    If match.Success AndAlso File.Exists(match.Groups(1).Value) Then
                        If File.ReadAllText(match.Groups(1).Value).ToLowerInvariant.Contains(
                                filterName.ToLowerInvariant) Then

                            Return True
                        End If
                    End If
                End If
            Next
        ElseIf p.Script.Engine = ScriptEngine.VapourSynth AndAlso
            Not package.VapourSynthFilterNames.NothingOrEmpty Then

            For Each filter In p.Script.Filters
                If filter.Active Then
                    For Each filterName In package.VapourSynthFilterNames
                        If filter.Script.ToLower.Contains(filterName.ToLower + "(") Then
                            Return True
                        End If
                    Next
                End If
            Next
        End If

        For Each package2 In Items.Values.OfType(Of PluginPackage)
            If package2 Is package Then Continue For

            If Not package2.Dependencies.NothingOrEmpty Then
                For Each dependency In package2.Dependencies
                    If dependency = package.Name Then
                        If IsPluginPackageRequired(package2) Then Return True
                    End If
                Next
            End If
        Next
    End Function

    Function GetDependencies() As List(Of PluginPackage)
        Dim plugins = Package.Items.Values.OfType(Of PluginPackage)()
        Dim ret As New List(Of PluginPackage)

        If Not Dependencies Is Nothing Then
            For Each plugin In plugins
                For Each i In Dependencies
                    If plugin.Name = i Then ret.Add(plugin)
                Next
            Next
        End If

        Return ret
    End Function

    Shared Sub WriteVSCode(ByRef scriptLower As String,
                           ByRef code As String,
                           ByRef filterName As String,
                           plugin As PluginPackage)

        If Not plugin.VapourSynthFilterNames.NothingOrEmpty Then
            For Each i In plugin.GetDependencies
                If Not i.VapourSynthFilterNames.NothingOrEmpty Then
                    WriteVSCode(scriptLower, code, filterName, i)
                End If
            Next
        End If

        If plugin.Filename.Ext = "py" Then
            If Not scriptLower.Contains("import importlib.machinery") AndAlso
                Not code.Contains("import importlib.machinery") Then

                code += "import importlib.machinery" + BR
            End If

            Dim line = plugin.Name + " = importlib.machinery.SourceFileLoader('" +
                plugin.Name + "', r""" + plugin.Path + """).load_module()" + BR

            If Not scriptLower.Contains(line.ToLower) AndAlso Not code.ToLower.Contains(line.ToLower) Then code += line
        Else
            If Not File.Exists(Folder.Plugins + plugin.Filename) AndAlso
                Not scriptLower.Contains(plugin.Filename.ToLower) AndAlso Not code.ToLower.Contains(plugin.Filename.ToLower) Then

                Dim line As String

                If scriptLower.Contains(".avs." + filterName.ToLower) OrElse
                    code.ToLower.Contains(".avs." + filterName.ToLower) Then

                    line = "core.avs.LoadPlugin(r""" + plugin.Path + """)" + BR
                Else
                    line = "core.std.LoadPlugin(r""" + plugin.Path + """)" + BR
                End If

                code += line
            End If
        End If
    End Sub
End Class

Public Class JavaPackage
    Inherits Package

    Sub New()
        Name = "Java"
        Filename = "Java.exe"
        WebURL = "http://java.com"
        TreePath = "Runtimes"
        Description = "Java is required by ProjectX. " + Strings.ProjectX
        DownloadURL = "http://java.com/en/download"
        IsRequired = False
        IgnoreVersion = True
    End Sub

    Public Overrides ReadOnly Property Path As String
        Get
            Dim ret = MyBase.Path
            If ret <> "" Then Return ret

            ret = "C:\ProgramData\Oracle\Java\javapath\" + Filename
            If File.Exists(ret) Then Return ret

            ret = "C:\Windows\Sysnative\" + Filename
            If File.Exists(ret) Then Return ret

            ret = "C:\Windows\System32\" + Filename
            If File.Exists(ret) Then Return ret
        End Get
    End Property
End Class

Public Class ProjectXPackage
    Inherits Package

    Sub New()
        Name = "ProjectX"
        Filename = "ProjectX.jar"
        WebURL = "http://project-x.sourceforge.net"
        Description = Strings.ProjectX
        IsRequired = False
    End Sub

    Overrides ReadOnly Property StartAction As Action
        Get
            Return AddressOf StartWithJava
        End Get
    End Property

    Public Overrides Function GetStatus() As String
        If Package.Java.Path = "" Then Return "Failed to locate Java, ProjectX requires Java."
        Return MyBase.GetStatus()
    End Function
End Class

Public Class qaacPackage
    Inherits Package

    Sub New()
        Name = "qaac"
        Filename = "qaac64.exe"
        WebURL = "http://github.com/nu774/qaac"
        Description = "qaac is a command line AAC encoder frontend based on the Apple AAC encoder. qaac requires libflac which StaxRip includes and it requires AppleApplicationSupport64.msi which can be extracted from the x64 iTunes installer using a decompression tool like 7-Zip. The makeportable script found on the qaac website can also be used."
    End Sub

    Overrides Property IsRequired As Boolean
        Get
            Return TypeOf p.Audio0 Is GUIAudioProfile AndAlso
                DirectCast(p.Audio0, GUIAudioProfile).Params.Encoder = GuiAudioEncoder.qaac OrElse
                TypeOf p.Audio1 Is GUIAudioProfile AndAlso
                DirectCast(p.Audio1, GUIAudioProfile).Params.Encoder = GuiAudioEncoder.qaac
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Overrides Function GetStatus() As String
        Dim pathVar = Folder.Programs + "Common Files\Apple\Apple Application Support\CoreAudioToolbox.dll"

        If Not File.Exists(pathVar) AndAlso Not File.Exists(GetDir() + "QTfiles64\CoreAudioToolbox.dll") Then
            Return "Failed to locate CoreAudioToolbox, read the description below. Expected paths:" +
                BR2 + pathVar + BR2 + GetDir() + "QTfiles64\CoreAudioToolbox.dll"
        End If
    End Function
End Class

Public Class DGIndexNVPackage
    Inherits Package

    Sub New()
        Name = "DGIndexNV"
        Filename = "DGIndexNV.exe"
        WebURL = "http://rationalqm.us/dgdecnv/dgdecnv.html"
        Description = Strings.DGDecNV
        HelpFile = "DGIndexNVManual.html"
        LaunchName = Filename
        FileNotFoundMessage = "DGIndexNV can be disabled under Tools/Settings/Demux."
    End Sub

    Overrides Function GetStatus() As String
        If Not File.Exists(GetDir() + "License.txt") Then
            Return "DGDecNV is shareware requiring a license file but the file is missing."
        End If
    End Function

    Overrides Property IsRequired As Boolean
        Get
            Return CommandLineDemuxer.IsActive("DGIndexNV")
        End Get
        Set(value As Boolean)
        End Set
    End Property
End Class

Public Class DGIndexIMPackage
    Inherits Package

    Sub New()
        Name = "DGIndexIM"
        Filename = "DGIndexIM.exe"
        WebURL = "http://rationalqm.us/mine.html"
        Description = Strings.DGDecIM
        HelpFile = "Notes.txt"
        FileNotFoundMessage = "DGIndexIM can be disabled under Tools/Settings/Demux."
    End Sub

    Overrides Function GetStatus() As String
        If Not File.Exists(GetDir() + "License.txt") Then
            Return "DGIndexIM is shareware requiring a license file but the file is missing."
        End If
    End Function

    Overrides Property IsRequired As Boolean
        Get
            Return CommandLineDemuxer.IsActive("DGIndexIM")
        End Get
        Set(value As Boolean)
        End Set
    End Property
End Class

Public Class dsmuxPackage
    Inherits Package

    Sub New()
        Name = "dsmux"
        Filename = "dsmux.x64.exe"
        Description = Strings.dsmux
        WebURL = "http://haali.su/mkv"
        SetupFilename = "MatroskaSplitter.exe"
    End Sub

    Public Overrides ReadOnly Property Path As String
        Get
            Dim ret = Registry.ClassesRoot.GetString("CLSID\" + GUIDS.HaaliMuxer.ToString + "\InprocServer32", Nothing)
            ret = FilePath.GetDir(ret) + Filename
            If File.Exists(ret) Then Return ret
        End Get
    End Property

    Overrides Property IsRequired As Boolean
        Get
            Return CommandLineDemuxer.IsActive("dsmux")
        End Get
        Set(value As Boolean)
        End Set
    End Property
End Class

Public Class HaaliSplitter
    Inherits Package

    Sub New()
        Name = "Haali Splitter"
        Filename = "splitter.ax"
        WebURL = "http://haali.su/mkv"
        SetupFilename = "MatroskaSplitter.exe"
        Description = "Haali Splitter is used by eac3to and dsmux to write MKV files. Haali Splitter and LAV Filters overrite each other, most people prefer LAV Filters, therefore it's recommended to install Haali first and LAV Filters last."
        IsRequired = False
    End Sub

    Public Overrides ReadOnly Property Path As String
        Get
            Dim ret = Registry.ClassesRoot.GetString("CLSID\" + GUIDS.HaaliMuxer.ToString + "\InprocServer32", Nothing)
            If File.Exists(ret) Then Return ret
        End Get
    End Property
End Class

Public Class temporalsoftenPackage
    Inherits PluginPackage

    Sub New()
        Name = "temporalsoften"
        Filename = "temporalsoften.dll"
        VapourSynthFilterNames = {"temporalsoften"}
    End Sub
End Class

Public Class DecombPackage
    Inherits PluginPackage

    Sub New()
        Name = "Decomb"
        Filename = "Decomb.dll"
        WebURL = "http://rationalqm.us/decomb/decombnew.html"
        HelpFile = "DecombReferenceManual.html"
        Description = "This package of plugin functions for Avisynth provides the means for removing combing artifacts from telecined progressive streams, interlaced streams, and mixtures thereof. Functions can be combined to implement inverse telecine (IVTC) for both NTSC and PAL streams."
        AviSynthFilterNames = {"Telecide", "FieldDeinterlace", "Decimate", "IsCombed"}
    End Sub
End Class

Public Class xvid_encrawPackage
    Inherits Package

    Sub New()
        Name = "xvid_encraw"
        Filename = "xvid_encraw.exe"
        Description = "XviD command line encoder"
        HelpFile = "help.txt"
        WebURL = "https://www.xvid.com"
    End Sub
End Class

Public Class checkmatePackage
    Inherits PluginPackage

    Sub New()
        Name = "checkmate"
        Filename = "checkmate.dll"
        WebURL = "http://github.com/tp7/checkmate"
        Description = "Spatial and temporal dot crawl reducer. Checkmate is most effective in static or low motion scenes. When using in high motion scenes (or areas) be careful, it's known to cause artifacts with its default values."
        AviSynthFilterNames = {"checkmate"}
    End Sub
End Class