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
        .HelpURL = "http://www.ffmpeg.org/ffmpeg-all.html",
        .Description = "Versatile audio video converter."})

    Shared Property Haali As Package = Add(New HaaliSplitter)

    Shared Property MediaInfo As Package = Add(New Package With {
        .Name = "MediaInfo",
        .Filename = "MediaInfo.dll",
        .WebURL = "http://mediaarea.net/en/MediaInfo",
        .Description = "MediaInfo is used by StaxRip to read infos from media files."})

    Shared Property MP4Box As Package = Add(New Package With {
        .Name = "MP4Box",
        .Filename = "MP4Box.exe",
        .WebURL = "http://gpac.wp.mines-telecom.fr/",
        .HelpURL = "http://gpac.wp.mines-telecom.fr/mp4box/mp4box-documentation",
        .Description = "MP4Box is a MP4 muxing and demuxing command line app."})

    Shared Property AviSynth As Package = Add(New AviSynthPlusPackage)
    Shared Property NicAudio As Package = Add(New NicAudioPackage)
    Shared Property qaac As Package = Add(New qaacPackage)
    Shared Property UnDot As Package = Add(New UnDotPackage)

    Shared Property xvid_encraw As Package = Add(New PluginPackage With {
        .Name = "xvid_encraw",
        .Filename = "xvid_encraw.exe",
        .Description = "XviD command line encoder",
        .HelpFile = "help.txt",
        .WebURL = "http://www.xvid.com"})

    Shared Property Decomb As Package = Add(New PluginPackage With {
        .Name = "Decomb",
        .Filename = "Decomb.dll",
        .WebURL = "http://rationalqm.us/decomb/decombnew.html",
        .HelpFile = "DecombReferenceManual.html",
        .Description = "This package of plugin functions for Avisynth provides the means for removing combing artifacts from telecined progressive streams, interlaced streams, and mixtures thereof. Functions can be combined to implement inverse telecine (IVTC) for both NTSC and PAL streams.",
        .AvsFilterNames = {"Telecide", "FieldDeinterlace", "Decimate", "IsCombed"}})

    Shared Property temporalsoften As Package = Add(New PluginPackage With {
        .Name = "temporalsoften",
        .Filename = "temporalsoften.dll",
        .VSFilterNames = {"TemporalSoften"}})

    Shared Property fdkaac As Package = Add(New Package With {
        .Name = "fdkaac",
        .Filename = "fdkaac.exe",
        .HelpFile = "help.txt",
        .Description = "Command line AAC encoder based on libfdk-aac.",
        .URL = "http://github.com/nu774/fdkaac",
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

    Shared Property MTN As Package = Add(New Package With {
        .Name = "MTNWindows",
        .Filename = "mtn.exe",
        .DirPath = "MTN",
        .Description = "movie thumbnailer saves thumbnails (screenshots) of movie or video files to jpeg files. StaxRip uses a custom built version with HEVC support added in and also includes the latest FFMPEG.",
        .StartActionValue = Sub()
                                Using fd As New OpenFileDialog
                                    fd.Title = "Select files"
                                    fd.Multiselect = False '' Can Only Do Single.
                                    If fd.ShowDialog = DialogResult.OK Then
                                        g.DefaultCommands.ExecuteCommandLine(Package.Items("MTNWindows").Path.Escape + " " + fd.FileName, True, True, False)
                                    End If
                                End Using
                            End Sub,
        .WebURL = "http://moviethumbnail.sourceforge.net/",
        .HelpURL = "http://moviethumbnail.sourceforge.net/usage.en.html"})

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
        .Name = "mpv",
        .Filename = "mpv.exe",
        .LaunchName = "mpv.exe",
        .URL = "https://mpv.io/",
        .Description = "mpv is a fork of mplayer2 and MPlayer. It shares some features with the former projects while introducing many more."})

    Shared Property Update As Package = Add(New Package With {
        .Name = "Update",
        .Filename = "Update.ps1",
        .LaunchName = "Update.ps1",
        .DirPath = "Updates",
        .StartActionValue = Sub()
                                g.DefaultCommands.ExecutePowerShellScript("cd %app_dir:Update%" + Package.Items("Update").Path.Escape + " ")
                            End Sub,
        .Description = "The PowerShell Script that Updates StaxRip."})

    Shared Property vsedit As Package = Add(New Package With {
        .Name = "vsedit",
        .Filename = "vsedit.exe",
        .URL = "https://vapoursynth.com/",
        .Description = "The New Editor for VapourSynth"})

    Shared Property modPlus As Package = Add(New PluginPackage With {
        .Name = "modPlus",
        .Filename = "modPlus.dll",
        .URL = "http://www.avisynth.nl/users/vcmohan/modPlus/modPlus.html",
        .Description = "This plugin has 9 functions, which modify values of color components to attenuate noise, blur or equalize input.",
        .AvsFilterNames = {"GBlur", "MBlur", "Median", "minvar", "Morph", "SaltPepper", "SegAmp", "TweakHist", "Veed"}})

    Shared Property checkmate As Package = Add(New PluginPackage With {
        .Name = "checkmate",
        .Filename = "checkmate.dll",
        .WebURL = "http://github.com/tp7/checkmate",
        .Description = "Spatial and temporal dot crawl reducer. Checkmate is most effective in static or low motion scenes. When using in high motion scenes (or areas) be careful, it's known to cause artifacts with its default values.",
        .AvsFilterNames = {"checkmate"}})

    Shared Property MedianBlur2 As Package = Add(New PluginPackage With {
        .Name = "MedianBlur2",
        .Filename = "MedianBlur2.dll",
        .URL = "http://avisynth.nl/index.php/MedianBlur2",
        .Description = "Implementation of constant time median filter for AviSynth.",
        .AvsFilterNames = {"MedianBlur", "MedianBlurTemporal"}})

    Shared Property AutoAdjust As Package = Add(New PluginPackage With {
        .Name = "AutoAdjust",
        .Filename = "AutoAdjust.dll",
        .URL = "http://forum.doom9.org/showthread.php?t=167573",
        .Description = "AutoAdjust is an automatic adjustement filter. It calculates statistics of clip, stabilizes them temporally and uses them to adjust luminance gain & color balance.",
        .AvsFilterNames = {"AutoAdjust"}})

    Shared Property SmoothAdjust As Package = Add(New PluginPackage With {
        .Name = "SmoothAdjust",
        .Filename = "SmoothAdjust.dll",
        .URL = "http://forum.doom9.org/showthread.php?t=154971",
        .Description = "SmoothAdjust is a set of 5 plugins to make YUV adjustements.",
        .AvsFilterNames = {"SmoothTweak", "SmoothCurve", "SmoothCustom", "SmoothTools"}})

    Shared Property EEDI3 As Package = Add(New PluginPackage With {
        .Name = "EEDI3",
        .Filename = "EEDI3.dll",
        .URL = "http://avisynth.nl/index.php/EEDI3",
        .Description = "EEDI3 (Enhanced Edge Directed Interpolation) resizes an image by 2x in the vertical direction by copying the existing image to 2*y(n) and interpolating the missing field.",
        .AvsFilterNames = {"EEDI3"},
        .AvsFiltersFunc = Function() {New VideoFilter("Field", "EEDI3", "EEDI3()")}})

    Shared Property EEDI2 As Package = Add(New PluginPackage With {
        .Name = "EEDI2",
        .Filename = "EEDI2.dll",
        .URL = "http://avisynth.nl/index.php/EEDI2",
        .Description = "EEDI2 (Enhanced Edge Directed Interpolation) resizes an image by 2x in the vertical direction by copying the existing image to 2*y(n) and interpolating the missing field.",
        .AvsFilterNames = {"EEDI2"}})

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
        .HelpURL = "http://f3kdb.readthedocs.io/en/latest/#",
        .Description = "Simple debanding filter that can be quite effective for some anime sources.",
        .VSFilterNames = {"f3kdb.Deband"},
        .AvsFilterNames = {"f3kdb"}})

    Shared Property vinverse As Package = Add(New PluginPackage With {
        .Name = "vinverse",
        .Filename = "vinverse.dll",
        .WebURL = "http://avisynth.nl/index.php/Vinverse",
        .Description = "A modern rewrite of a simple but effective plugin to remove residual combing originally based on an AviSynth script by Didée and then written as a plugin by tritical.",
        .AvsFilterNames = {"vinverse", "vinverse2"},
        .AvsFiltersFunc = Function() {
            New VideoFilter("Restoration", "RCR | Vinverse", "vinverse()"),
            New VideoFilter("Restoration", "RCR | Vinverse2", "vinverse2()")}})

    Shared Property scenechange As Package = Add(New PluginPackage With {
        .Name = "scenechange",
        .Filename = "scenechange.dll",
        .VSFilterNames = {"scenechange"}})

    Shared Property avs2pipemod As Package = Add(New Package With {
        .Name = "avs2pipemod",
        .Filename = "avs2pipemod64.exe",
        .WebURL = "http://github.com/chikuzen/avs2pipemod",
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
        .Filename = "x264-10b.exe",
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
        .WebURL = "https://mkvtoolnix.download/",
        .HelpURL = "https://mkvtoolnix.download/docs.html"})

    Shared Property mkvinfo As Package = Add(New Package With {
        .Name = "mkvinfo",
        .Filename = "mkvinfo.exe",
        .DirPath = "MKVToolNix",
        .WebURL = "https://mkvtoolnix.download/",
        .HelpURL = "https://mkvtoolnix.download/docs.html",
        .StartActionValue = Sub()
                                g.DefaultCommands.ExecuteCommandLine(Package.Items("mkvinfo").Path.Escape + " " + "%target_files%" + BR + "pause", False, False, True)
                            End Sub,
        .Description = "MKV muxing tool."})

    Shared Property mkvextract As Package = Add(New Package With {
        .Name = "mkvextract",
        .Filename = "mkvextract.exe",
        .DirPath = "MKVToolNix",
        .WebURL = "https://mkvtoolnix.download/",
        .HelpURL = "https://mkvtoolnix.download/docs.html",
        .Description = "MKV demuxing tool."})

    Shared Property NVEnc As Package = Add(New Package With {
        .Name = "NVEnc",
        .Filename = "NVEncC64.exe",
        .WebURL = "http://github.com/rigaya/NVEnc",
        .DownloadURL = "http://onedrive.live.com/?cid=6bdd4375ac8933c6&id=6BDD4375AC8933C6!2293",
        .Description = "NVIDIA hardware video encoder.",
        .HelpFile = "help.txt"})

    Shared Property QSVEnc As Package = Add(New Package With {
        .Name = "QSVEnc",
        .Filename = "QSVEncC64.exe",
        .Description = "Intel hardware video encoder.",
        .HelpFile = "help.txt",
        .DownloadURL = "http://onedrive.live.com/?cid=6bdd4375ac8933c6&id=6BDD4375AC8933C6!482",
        .WebURL = "http://github.com/rigaya/QSVEnc"})

    Shared Property VCEEnc As Package = Add(New Package With {
        .Name = "VCEEnc",
        .Filename = "VCEEncC64.exe",
        .Description = "AMD hardware video encoder.",
        .HelpFile = "help.txt",
        .DownloadURL = "http://onedrive.live.com/?id=6BDD4375AC8933C6!516&cid=6BDD4375AC8933C6",
        .WebURL = "http://github.com/rigaya/VCEEnc"})

    Shared Property DGDecodeNV As Package = Add(New PluginPackage With {
        .Name = "DGDecodeNV",
        .Filename = "DGDecodeNV.dll",
        .WebURL = "http://rationalqm.us/dgdecnv/dgdecnv.html",
        .Description = Strings.DGDecNV,
        .DirPath = "DGIndexNV",
        .HelpFile = "DGDecodeNVManual.html",
        .IsRequiredFunc = Function() p.Script.Filters(0).Script.Contains("DGSource("),
        .HintDirFunc = Function() DGIndexNV.GetStoredPath.Dir,
        .AvsFilterNames = {"DGSource"},
        .AvsFiltersFunc = Function() {New VideoFilter("Source", "DGSource", "DGSource(""%source_file%"")")},
        .VSFiltersFunc = Function() {New VideoFilter("Source", "DGSource", "clip = core.avs.DGSource(r""%source_file%"")")}})

    Shared Property DGDecodeIM As Package = Add(New PluginPackage With {
        .Name = "DGDecodeIM",
        .Filename = "DGDecodeIM.dll",
        .WebURL = "http://rationalqm.us/mine.html",
        .Description = Strings.DGDecIM,
        .DirPath = "DGIndexIM",
        .HelpFile = "Notes.txt",
        .HintDirFunc = Function() DGIndexIM.GetStoredPath.Dir,
        .IsRequiredFunc = Function() p.Script.Filters(0).Script.Contains("DGSourceIM("),
        .AvsFilterNames = {"DGSourceIM"},
        .AvsFiltersFunc = Function() {New VideoFilter("Source", "DGSourceIM", "DGSourceIM(""%source_file%"")")}})

    Shared Property FFT3DFilter As Package = Add(New PluginPackage With {
        .Name = "FFT3DFilter",
        .Filename = "fft3dfilter.dll",
        .URL = "http://github.com/pinterf/fft3dfilter",
        .Description = "FFT3DFilter uses Fast Fourier Transform method for image processing in frequency domain.",
        .AvsFilterNames = {"FFT3DFilter"},
        .AvsFiltersFunc = Function() {New VideoFilter("Noise", "FFT3DFilter | FFT3DFilter", "FFT3DFilter()")}})

    Shared Property ffms2 As Package = Add(New PluginPackage With {
        .Name = "ffms2",
        .Filename = "ffms2.dll",
        .WebURL = "http://github.com/FFMS/ffms2",
        .Description = "AviSynth+ and VapourSynth source filter supporting various input formats.",
        .HelpURLFunc = Function(engine) If(engine = ScriptEngine.AviSynth, "http://github.com/FFMS/ffms2/blob/master/doc/ffms2-avisynth.md", "http://github.com/FFMS/ffms2/blob/master/doc/ffms2-vapoursynth.md"),
        .AvsFilterNames = {"FFVideoSource", "FFAudioSource"},
        .AvsFiltersFunc = Function() {New VideoFilter("Source", "FFVideoSource", $"FFVideoSource(""%source_file%"", colorspace = ""YV12"", \{BR}              cachefile = ""%source_temp_file%.ffindex"")")},
        .VSFilterNames = {"ffms2"},
        .VSFiltersFunc = Function() {New VideoFilter("Source", "ffms2", "clip = core.ffms2.Source(r""%source_file%"", cachefile = r""%source_temp_file%.ffindex"")")}})

    Shared Property VSFilterMod As Package = Add(New PluginPackage With {
        .Name = "VSFilterMod",
        .Filename = "VSFilterMod.dll",
        .Description = "AviSynth subtitle plugin with support for vobsub srt and ass.",
        .WebURL = "http://github.com/HomeOfVapourSynthEvolution/VSFilterMod",
        .AvsFilterNames = {"VobSub", "TextSubMod"},
        .VSFilterNames = {"vsfm.VobSub", "vsfm.TextSubMod"}})

    Shared Property SangNom2 As Package = Add(New PluginPackage With {
        .Name = "SangNom2",
        .Filename = "SangNom2.dll",
        .WebURL = "http://avisynth.nl/index.php/SangNom2",
        .Description = "SangNom2 is a reimplementation of MarcFD's old SangNom filter. Originally it's a single field deinterlacer using edge-directed interpolation but nowadays it's mainly used in anti-aliasing scripts. The output is not completely but mostly identical to the original SangNom.",
        .AvsFilterNames = {"SangNom2"}})
    Shared Function Add(pack As Package) As Package
        Items(pack.ID) = pack
        Return pack
    End Function

    Shared Sub New()
        Add(New Package With {
            .Name = "Visual C++ 2012",
            .Filename = "msvcp110.dll",
            .Description = "Visual C++ 2012 Redistributable is required by some tools used by StaxRip.",
            .DownloadURL = "http://www.microsoft.com/en-US/download/details.aspx?id=30679",
            .FixedDir = Folder.System,
            .IgnoreVersion = True,
            .IsRequiredFunc = Function() Items("SangNom2 avs").IsRequired,
            .TreePath = "Runtimes"})

        Add(New Package With {
            .Name = "Visual C++ 2013",
            .Filename = "msvcp120.dll",
            .Description = "Visual C++ 2013 Redistributable is required by some tools used by StaxRip.",
            .DownloadURL = "http://www.microsoft.com/en-US/download/details.aspx?id=40784",
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
            .IsRequiredFunc = Function() SangNom2.IsRequired OrElse VSFilterMod.IsRequired,
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
            .WebURL = "http://forum.doom9.org/showthread.php?t=174797"})


        'Add(New Package With {
        '    .Name = "MKVHDR",
        '    .Filename = "mkvmerge.exe",
        '    .DirPath = "MKVToolNix",
        '    .Description = "MKVToolNix is a set of tools to create, alter and inspect Matroska files",
        '    .StartActionValue = Sub()
        '                            g.DefaultCommands.ExecuteCommandLine(Package.Items("MKVHDR").Path.Escape + " " + "-o %target_Injected_file%" + " --colour-matrix 0:9 --colour-range 0:1 --colour-transfer-characteristics 0:16 --colour-primaries 0:9 --max-content-light 0:1000 --max-frame-light 0:300 --max-luminance 0:1000 --min-luminance 0:0.01 --chromaticity-coordinates 0:0.68,0.32,0.265,0.690,0.15,0.06 --white-colour-coordinates 0:0.3127,0.3290 " + p.TargetFile + BR + "pause", False, False, True)
        '                        End Sub,
        '    .WebURL = "https://mkvtoolnix.download/doc/mkvinfo.html"})

        Add(New PluginPackage With {
            .Name = "KNLMeansCL",
            .Filename = "KNLMeansCL.dll",
            .WebURL = "http://github.com/Khanattila/KNLMeansCL",
            .Description = "KNLMeansCL is an optimized pixelwise OpenCL implementation of the Non-local means denoising algorithm. Every pixel is restored by the weighted average of all pixels in its search window. The level of averaging is determined by the filtering parameter h.",
            .VSFilterNames = {"knlm.KNLMeansCL"},
            .AvsFilterNames = {"KNLMeansCL"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Noise", "NLMeans | KNLMeansCL | Spatio-Temporal Light", "KNLMeansCL(D = 1, A = 1, h = 2, device_type=""auto"")"),
                New VideoFilter("Noise", "NLMeans | KNLMeansCL | Spatio-Temporal Medium", "KNLMeansCL(D = 1, A = 1, h = 4, device_type=""auto"")"),
                New VideoFilter("Noise", "NLMeans | KNLMeansCL | Spatio-Temporal Strong", "KNLMeansCL(D = 1, A = 1, h = 8, device_type=""auto"")")},
            .VSFiltersFunc = Function() {
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Light", "clip = core.knlm.KNLMeansCL(clip, d = 1, a = 1, h = 2, device_type=""auto"")"),
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Medium", "clip = core.knlm.KNLMeansCL(clip, d = 1, a = 1, h = 4, device_type=""auto"")"),
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Strong", "clip = core.knlm.KNLMeansCL(clip, d = 1, a = 1, h = 8, device_type=""auto"")")}})

        Add(New PluginPackage With {
            .Name = "mvtools2",
            .Filename = "mvtools2.dll",
            .WebURL = "http://github.com/pinterf/mvtools",
            .HelpURL = "http://avisynth.org.ru/mvtools/mvtools2.html",
            .Description = "MVTools is collection of functions for estimation and compensation of objects motion in video clips. Motion compensation may be used for strong temporal denoising, advanced framerate conversions, image restoration and other tasks.",
            .AvsFilterNames = {"MSuper", "MAnalyse", "MCompensate", "MMask", "MDeGrain1", "MDeGrain2", "MDegrain3"}})

        Add(New PluginPackage With {
            .Name = "MPEG2DecPlus",
            .Filename = "MPEG2DecPlus.dll",
            .WebURL = "http://github.com/chikuzen/MPEG2DecPlus",
            .Description = "Source filter to open D2V index files created with DGIndex or D2VWitch.",
            .AvsFilterNames = {"MPEG2Source"},
            .AvsFiltersFunc = Function() {New VideoFilter("Source", "MPEG2Source", "MPEG2Source(""%source_file%"")")}})

        Add(New PluginPackage With {
            .Name = "DSS2mod",
            .Filename = "avss.dll",
            .WebURL = "http://code.google.com/p/xvid4psp/downloads/detail?name=DSS2%20mod%20%2B%20LAVFilters.7z&can=2&q=",
            .Description = "Direct Show source filter",
            .AvsFilterNames = {"DSS2"},
            .AvsFiltersFunc = Function() {New VideoFilter("Source", "DSS2", "DSS2(""%source_file%"")")}})

        Add(New PluginPackage With {
            .Name = "Deblock",
            .Filename = "Deblock.dll",
            .Description = "Deblocking plugin using the deblocking filter of h264.",
            .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/DeBlock",
            .DirPath = "Plugins\AVS\Deblock",
            .AvsFilterNames = {"Deblock"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Restoration", "Deblock | Deblock", "Deblock(quant=25, aOffset = 0, bOffset = 0, planes=""yuv"")")}})

        Add(New PluginPackage With {
            .Name = "TNLMeans",
            .Filename = "TNLMeans.dll",
            .WebURL = "http://avisynth.nl/index.php/TNLMeans",
            .Description = "TNLMeans is an implementation of the NL-means denoising algorithm. Aside from the original method, TNLMeans also supports extension into 3D, a faster, block based approach, and a multiscale version.",
            .HelpFile = "readme.txt",
            .AvsFilterNames = {"TNLMeans"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Noise", "NLMeans | TNLMeans", "TNLMeans(Ax=4, Ay=4, Az=0, Sx=2, Sy=2, Bx=1, By=1, ms=false, rm=4, a=1.0, h=1.8, sse=true)")}})

        Add(New PluginPackage With {
            .Name = "VagueDenoiser",
            .Filename = "VagueDenoiser.dll",
            .Description = "This is a Wavelet based Denoiser. Basically, it transforms each frame from the video input into the wavelet domain, using various wavelet filters. Then it applies some filtering to the obtained coefficients.",
            .HelpFile = "vaguedenoiser.html",
            .WebURL = "http://avisynth.nl/index.php/VagueDenoiser",
            .AvsFilterNames = {"VagueDenoiser"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Noise", "VagueDenoiser", "VagueDenoiser(threshold=0.8, method=1, nsteps=6, chromaT=0.8)")}})

        Add(New PluginPackage With {
            .Name = "DGTonemap",
            .Filename = "DGTonemap.dll",
            .URL = "http://rationalqm.us/mine.html",
            .HelpFile = "Readme.txt",
            .Description = "DGTonemap provides filters for HDR Tonemapping Reinhard and Hable.",
            .AvsFilterNames = {"DGReinhard", "DGHable"}})

        Add(New PluginPackage With {
            .Name = "AnimeIVTC",
            .Filename = "AnimeIVTC.avsi",
            .URL = "http://avisynth.nl/index.php/AnimeIVTC",
            .AvsFilterNames = {"AnimeIVTC"}})

        Add(New PluginPackage With {
            .Name = "DePan",
            .Filename = "DePan.dll",
            .DirPath = "Plugins\AVS\MVTools2",
             .HelpFile = "Readme_depans.txt",
            .WebURL = "http://avisynth.nl/index.php/Depan",
            .AvsFilterNames = {"DePan", "DePanInterleave", "DePanStabilize", "DePanScenes"}})

        Add(New PluginPackage With {
            .Name = "DePanEstimate",
            .DirPath = "Plugins\AVS\MVTools2",
            .Filename = "DePanEstimate.dll",
             .HelpFile = "Readme_depans.txt",
            .WebURL = "http://avisynth.nl/index.php/Depan",
            .AvsFilterNames = {"DePanEstimate"}})

        Add(New PluginPackage With {
            .Name = "Shader AVSI",
            .DirPath = "Plugins\AVS\Shader",
            .Filename = "Shader.avsi",
             .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/Shader",
            .AvsFilterNames = {"SuperRes", "SuperResXBR", "SuperXBR", "ResizeShader", "SuperResPass", "SuperXbrMulti", "ResizeShader", "ResizeInternal", "CreateParamXY", "CreateParamDivXY", "CreateParamFloat4"}})

        Add(New PluginPackage With {
            .Name = "Shader DLL",
            .DirPath = "Plugins\AVS\Shader",
            .Filename = "Shader.dll",
             .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/Shader",
            .AvsFilterNames = {"Shader", "ShaderExecute", "ConvertToShader", "ConvertFromShader"}})

        Add(New PluginPackage With {
            .Name = "JincResize",
            .Filename = "JincResize.dll",
            .Description = "Jinc (EWA Lanczos) resampling plugin for AviSynth 2.6/AviSynth+.",
            .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/Jinc",
            .AvsFilterNames = {"Jinc36Resize", "Jinc64Resize", "Jinc144Resize", "Jinc256Resize"}})

        Add(New PluginPackage With {
            .Name = "HQDN3D",
            .Filename = "Hqdn3d.dll",
             .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/Hqdn3d",
            .AvsFilterNames = {"HQDN3D"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Noise", "HQDN3D", "HQDN3D(ls = 4.0, cs=3.0, lt=6.0, ct=4.5, restart=7)")}})

        Add(New PluginPackage With {
            .Name = "HQDeringmod",
            .Filename = "HQDeringmod.avsi",
            .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/HQDering_mod",
            .Description = "Applies deringing by using a smart smoother near edges (where ringing occurs) only.",
            .AvsFilterNames = {"HQDeringmod"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Restoration", "Dehalo | HQDeringmod", "HQDeringmod()")}})

        Add(New PluginPackage With {
            .Name = "InterFrame2",
            .Filename = "InterFrame2.avsi",
            .HelpFile = "InterFrame2.html",
            .Description = "A frame interpolation script that makes accurate estimations about the content of frames",
            .DirPath = "Plugins\AVS\InterFrame2",
            .WebURL = "http://avisynth.nl/index.php/InterFrame",
            .AvsFilterNames = {"InterFrame", "InterFrameProcess"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("FrameRate", "InterFrame", "InterFrame(Preset=""Medium"", Tuning=""$select:msg:Select the Tuning Preset;Animation;Film;Smooth;Weak$"", NewNum=$enter_text:Enter the NewNum Value.$, NewDen=$enter_text:Enter the NewDen Value$, Cores=$enter_text:Enter the Number of Cores You want to use$, OverrideAlgo=$select:msg:Which Algorithm Do you want to Use?;Strong Predictions|2;Intelligent|13;Smoothest|23$, GPU=$select:msg:Enable GPU Feature?;True;False$)")}})

        Add(New PluginPackage With {
            .Name = "SVPFlow 1",
            .DirPath = "Plugins\AVS\SVPFlow",
            .HelpFile = "Readme.txt",
            .Description = "Motion vectors search plugin  is a deeply refactored and modified version of MVTools2 Avisynth plugin",
            .Filename = "svpflow1.dll",
            .WebURL = "http://avisynth.nl/index.php/SVPFlow",
            .AvsFilterNames = {"analyse_params", "super_params", "SVSuper", "SVAnalyse"}})

        Add(New PluginPackage With {
            .Name = "SVPFlow 2",
            .DirPath = "Plugins\AVS\SVPFlow",
            .HelpFile = "Readme.txt",
            .Description = "Motion vectors search plugin is a deeply refactored and modified version of MVTools2 Avisynth plugin",
            .Filename = "svpflow2.dll",
            .WebURL = "http://avisynth.nl/index.php/SVPFlow",
            .AvsFilterNames = {"smoothfps_params", "SVConvert", "SVSmoothFps"}})

        Add(New PluginPackage With {
            .Name = "MipSmooth",
            .Filename = "MipSmooth.dll",
            .Description = "a reinvention of SmoothHiQ and Convolution3D. MipSmooth was made to enable smoothing of larger pixel areas than 3x3(x3), to remove blocks and smoothing out low-frequency noise.",
            .HelpFile = "MipSmooth.html",
            .WebURL = "http://avisynth.org.ru/docs/english/externalfilters/mipsmooth.htm",
            .AvsFilterNames = {"MipSmooth"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Restoration", "Deblock | MipSmooth", "MipSmooth(downsizer=""lanczos"", upsizer=""bilinear"", scalefactor=1.5, method = ""strong"")")}})

        Add(New PluginPackage With {
            .Name = "TMM2",
            .Filename = "TMM2.dll",
            .Description = "TMM builds a motion-mask for TDeint, which TDeint uses via its 'emask' parameter.",
            .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/TMM",
            .AvsFilterNames = {"TMM2"}})

        Add(New PluginPackage With {
            .Name = "FineDehalo",
            .Filename = "FineDehalo.avsi",
            .Description = "Halo removal script that uses DeHalo_alpha with a few masks and optional contra-sharpening to try remove halos without removing important details (like line edges). It also includes FineDehalo2, this function tries to remove 2nd order halos. See script for extensive information. ",
            .WebURL = "http://avisynth.nl/index.php/FineDehalo",
            .HelpFile = "Readme.txt",
            .AvsFilterNames = {"FineDehalo"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Restoration", "Dehalo | FineDehalo", "FineDehalo(rx=2.0, ry=2.0, thmi=80, thma=128, thlimi=50, thlima=100, darkstr=1.0, brightstr=1.0, showmask=0, contra=0.0, excl=true)")}})

        Add(New PluginPackage With {
            .Name = "CNR2",
            .Filename = "CNR2.dll",
            .HelpFile = "CNR2.html",
            .Description = "A fast chroma denoiser. Very effective against stationary rainbows and huge analogic chroma activity. Useful to filter VHS/TV caps.",
            .WebURL = "http://avisynth.nl/index.php/CNR2",
            .AvsFilterNames = {"cnr2"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Restoration", "CNR2", "Cnr2(mode=""oxx"", scdthr=10.0, ln=35, lm=192, un=47, um=255, vn=47, vm=255, log=false, sceneChroma=false)")}})

        Add(New PluginPackage With {
            .Name = "LUtils",
            .Filename = "LUtils.avsi",
            .Description = "A collection of helper and wrapper functions meant to help script authors in handling common operations ",
            .WebURL = "https://github.com/AviSynth/avs-scripts",
            .AvsFilterNames = {"LuStackedNto16", "LuPlanarToStacked", "LuRGB48YV12ToRGB48Y", "LuIsFunction", "LuSeparateColumns", "LuMergePlanes", "LuIsHD", "LuConvCSP", "Lu8To16", "Lu16To8", "LuIsEq", "LuSubstrAtIdx", "LuSubstrCnt", "LuReplaceStr", "LUIsDefined", "LuMerge", "LuLut", "LuLimitDif", "LuBlankClip", "LuIsSameRes"}})

        Add(New PluginPackage With {
            .Name = "Average",
            .Filename = "Average.dll",
            .Description = "A simple plugin that calculates a weighted frame-by-frame average from multiple clips. This is a modern rewrite of the old Average plugin but a bit faster, additional colorspace support, and some additional sanity checks.",
            .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/Average",
            .AvsFilterNames = {"Average"}})

        Add(New PluginPackage With {
            .Name = "AvsResize",
            .Filename = "avsresize.dll",
            .HelpFile = "Readme.txt",
            .WebURL = "http://forum.doom9.org/showthread.php?t=173986",
            .AvsFilterNames = {"z_ConvertFormat", "z_PointResize", "z_BilinearResize", "z_BicubicResize", "z_LanczosResize", "z_Lanczos4Resize", "z_BlackmanResize", "z_Spline16Resize", "z_Spline36Resize", "z_Spline64Resize", "z_GaussResize", "z_SincResize"}})

        Add(New PluginPackage With {
            .Name = "Deblock_QED",
            .Filename = "Deblock_QED.avsi",
            .Description = "Designed to provide 8x8 deblocking sensitive to the amount of blocking in the source, compared to other deblockers which apply a uniform deblocking across every frame. ",
            .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/Deblock_QED",
            .AvsFilterNames = {"Deblock_QED"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Restoration", "Deblock | Deblock_QED", "Deblock_QED(quant1=24, quant2=26, aOff1=1, aOff2=1, bOff1=2, bOff2=2, uv=3)")}})

        Add(New PluginPackage With {
            .Name = "nnedi3 AVSI",
            .Filename = "nnedi3.avsi",
            .HelpFile = "Readme.txt",
            .Description = "nnedi3 is an AviSynth 2.5 plugin, but supports all new planar colorspaces when used with AviSynth 2.6",
            .DirPath = "Plugins\AVS\NNEDI3",
            .WebURL = "http://avisynth.nl/index.php/nnedi3",
            .AvsFilterNames = {"nnedi3_rpow2", "nnedi3", "nnedi3x", "nnedi3x_resize_helper", "nnedi3x_rpow2_helper", "nnedi3x_rpow2", "nnedi3_resize16"}})

        Add(New PluginPackage With {
            .Name = "edi_rpow2 AVSI",
            .Filename = "edi_rpow2.avsi",
            .HelpFile = "Readme.txt",
            .Description = "An improved rpow2 function for nnedi3, nnedi3ocl, eedi3, and eedi2.",
            .DirPath = "Plugins\AVS\NNEDI3",
            .WebURL = "http://avisynth.nl/index.php/nnedi3",
            .AvsFilterNames = {"nnedi3_rpow2", "nnedi3", "nnedi3x", "nnedi3x_resize_helper", "nnedi3x_rpow2_helper", "nnedi3x_rpow2", "eedi3", "eedi2", "nnedi3_resize16"}})

        Add(New PluginPackage With {
            .Name = "SmoothD2",
            .DirPath = "Plugins\AVS\SmoothD2",
            .Filename = "SmoothD2.dll",
            .HelpFile = "Readme.txt",
            .Description = "Deblocking filter. Rewrite of SmoothD. Faster, better detail preservation, optional chroma deblocking.",
            .WebURL = "http://avisynth.nl/index.php/SmoothD2",
            .AvsFilterNames = {"SmoothD2"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Restoration", "Deblock | SmoothD2", "SmoothD2(quant=3, num_shift=3, Matrix=3, Qtype=1, ZW=1, ZWce=1, ZWlmDark=0, ZWlmBright=255, ncpu=4)")}})

        Add(New PluginPackage With {
            .Name = "SmoothD2c",
            .DirPath = "Plugins/AVS/SmoothD2",
            .HelpFile = "Readme.txt",
            .Description = "Deblocking filter. Rewrite of SmoothD. Faster, better detail preservation, optional chroma deblocking.",
            .Filename = "SmoothD2c.avs",
            .WebURL = "http://avisynth.nl/index.php/SmoothD2",
            .AvsFilterNames = {"SmoothD2c", "SmoothD2", "Ylevels", "YlevelsS", "colorWarpToLuma", "buildZmask", "m4", "m8", "m16", "m32"}})

        Add(New PluginPackage With {
            .Name = "xNLMeans",
            .Filename = "xNLMeans.dll",
            .WebURL = "http://avisynth.nl/index.php/xNLMeans",
            .Description = "XNLMeans is an AviSynth plugin implementation of the Non Local Means denoising algorithm",
            .HelpFile = "Readme.txt",
            .AvsFilterNames = {"xNLMeans"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Noise", "NLMeans | xNLMeans", "xnlmeans(a=4,h=2.2,vcomp=0.5,s=1)")}})

        Add(New PluginPackage With {
            .Name = "XAA",
            .Filename = "xaa.avsi",
            .WebURL = "http://avisynth.nl/index.php/xaa",
            .Description = "A highly versatile anti-aliasing function.",
            .HelpFile = "Readme.txt",
            .AvsFilterNames = {"XAA"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Line", "Anti-Aliasing | XAA", "xaa()")}})

        Add(New PluginPackage With {
            .Name = "FFT3DGPU",
            .Filename = "FFT3dGPU.dll",
            .Description = "Similar algorithm to FFT3DFilter, but uses graphics hardware for increased speed.",
            .HelpFile = "Readme.txt",
            .AvsFilterNames = {"FFT3DGPU"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Noise", "FFT3DFilter | FFT3DGPU", "FFT3DGPU(sigma=1.5, bt=5, bw=32, bh=32, ow=16, oh=16, sharpen=0.4, NVPerf=$Select:msg:Do you want to use Nvidia Feature;True;False$)")}})

        Add(New PluginPackage With {
            .Name = "DehaloAlpha",
            .Filename = "Dehalo_alpha.avsi",
            .Description = "Reduce halo artifacts that can occur when sharpening.",
            .HelpFile = "Readme.txt",
            .AvsFilterNames = {"DeHalo_alpha_mt", "m4"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Restoration", "Dehalo | DehaloAlpha", "DeHalo_alpha_mt(rx=2.0, ry=2.0, darkstr=1.0, brightstr=1.0, lowsens=50, highsens=50, ss=1.5)")}})

        Add(New PluginPackage With {
            .Name = "MAA2Mod",
            .DirPath = "Plugins\AVS\MAA2",
            .Filename = "maa2mod.avsi",
            .Description = "Updated version of the MAA2+ antialising script from AnimeIVTC. MAA2 uses tp7's SangNom2, which provide a nice speedup for SangNom-based antialiasing. Mod version also includes support for EEDI3 along with a few other new functions.",
            .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/MAA2",
            .AvsFilterNames = {"MAA2", "Sangnom2AA", "maa2ee"},
            .AvsFiltersFunc = Function() {
            New VideoFilter("Line", "MAA2 | MMA2 with nnedi3", "MAA2(mask=1, chroma=false, ss=2.0, aa=48, aac=40, threads=4, show=0, ext_aa=nnedi3_resize16())"),
            New VideoFilter("Line", "MAA2 | MMA2", "MAA2(mask=1, chroma=false, ss=2.0, aa=48, aac=40, threads=4, show=0)")}})

        Add(New PluginPackage With {
            .Name = "DAA3Mod",
            .Filename = "daa3mod.avsi",
            .Description = "Motion-Compensated Anti-aliasing with contra-sharpening, can deal with ifade too, created because when applied daa3 to fixed scenes, it could damage some details and other issues.",
            .WebURL = "http://avisynth.nl/index.php/daa3",
            .AvsFilterNames = {"daa3mod", "mcdaa3"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Line", "DAA3 | MCDAA3", "mcdaa3()"),
                New VideoFilter("Line", "DAA3 | DAA3", "daa3mod()")}})

        Add(New PluginPackage With {
            .Name = "eedi3_resize",
            .Filename = "eedi3_resize.avsi",
            .DirPath = "Plugins\AVS\EEDI3",
            .Description = "eedi3 based resizing script that allows to resize to arbitrary resolutions while maintaining the correct image center and chroma location.",
            .HelpFile = "EEDI3 - Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/eedi3",
            .AvsFilterNames = {"eedi3_resize", "eedi3_resize_splinesclip", "eedi3_resize_GetCSP", "GetCSP_Y8_YV411"}})

        Add(New PluginPackage With {
            .Name = "DeNoise Histogram",
            .DirPath = "Plugins\AVS\DeNoiseMD",
            .Filename = "DiffCol.avsi",
            .Description = "Histogram for both DenoiseMD and DenoiseMF",
            .WebURL = "http://avisynth.nl",
            .AvsFilterNames = {"DiffCol"}})

        Add(New PluginPackage With {
            .Name = "DeNoiseMD",
            .Filename = "DeNoiseMD.avsi",
            .Description = "A fast and accurate denoiser for a Full HD video from a H.264 camera. ",
            .WebURL = "http://avisynth.nl",
            .AvsFilterNames = {"DeNoiseMD1", "BlendClips", "Range8to16", "DenoiseMD2"},
            .AvsFiltersFunc = Function() {
             New VideoFilter("Noise", "DeNoise | Denoise MD | DeNoiseMD 16Bit", "DeNoiseMD1(sigma=4, overlap=2, thcomp=80, str=0.8)" + BR + "Functions" + BR + "Dither_convey_yuv4xxp16_on_yvxx()"),
             New VideoFilter("Noise", "DeNoise | Denoise MD | DeNoiseMD Histogram", "a = last" + BR + "DenoiseMD1(sigma = 4, overlap = 2, thcomp = 80, str = 0.8)" + BR + "b = last.DitherPost(mode = -1)" + BR + "DiffCol(a, b, col = 1, colnum = 4, hista = False, histb = False)"),
             New VideoFilter("Noise", "DeNoise | Denoise MD | DeNoiseMD 8Bit", "DeNoiseMD1(sigma=4, overlap=2, thcomp=80, str=0.8)" + BR + "#Functions" + BR + "DitherPost(mode=7, ampo=1, ampn=0)")}})

        Add(New PluginPackage With {
            .Name = "DeNoiseMF",
            .Filename = "DeNoiseMF.avsi",
            .Description = "A fast and accurate denoiser for a Full HD video from a H.264 camera. ",
            .WebURL = "http://avisynth.nl",
            .AvsFilterNames = {"DeNoiseMF1", "DenoiseMF2"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Noise", "DeNoise | Denoise MF | DeNoiseMF 16Bit", "DenoiseMF2(s1=2.0, s2=2.5, s3=3.0, s4=2.0, overlap=4, thcomp=80, str=0.8, gpu=$select:msg:Use GPU Enabled Feature?;True;False$)" + BR + "#Functions" + BR + "#16Bit Functions" + BR + "Dither_convey_yuv4xxp16_on_yvxx()"),
                New VideoFilter("Noise", "DeNoise | Denoise MF | DeNoiseMF Histogram", "a = last" + BR + "DenoiseMF1(s1=2.0, s2=2.5, s3=3.0, s4=2.0, overlap=4, thcomp=80, str=0.8)" + BR + "b = last.DitherPost(mode = -1)" + BR + "DiffCol(a, b, col = 1, colnum = 4, hista = False, histb = False)"),
                New VideoFilter("Noise", "DeNoise | Denoise MF | DeNoiseMF 8Bit", "DenoiseMF2(s1=2.0, s2=2.5, s3=3.0, s4=2.0, overlap=4, thcomp=80, str=0.8, gpu=$select:msg:Use GPU Enabled Feature?;True;False$)" + BR + "#Functions" + BR + "DitherPost(mode=7, ampo=1, ampn=0)")}})

        Add(New PluginPackage With {
            .Name = "DFTTest",
            .Filename = "dfttest.dll",
            .Description = "2D/3D frequency domain denoiser using Discrete Fourier transform",
            .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/DFTTest",
            .AvsFilterNames = {"dfttest"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Noise", "DFTTest | DFTTest 8Bit | Moderate DeNoise", "dfttest(sigma=16, tbsize=5)"),
                New VideoFilter("Noise", "DFTTest | DFTTest 8Bit | Light DeNoise", "dfttest(sigma=6, tbsize=1)"),
                New VideoFilter("Noise", "DFTTest | DFTTest 8Bit | Strong DeNoise", "dfttest(sigma=64, tbsize=1)"),
                New VideoFilter("Noise", "DFTTest | DFTTest 16Bit | Moderate DeNoise", "dfttest(sigma=16, tbsize=5, lsb_in=true, lsb=true)"),
                New VideoFilter("Noise", "DFTTest | DFTTest 16Bit | Light DeNoise", "dfttest(sigma=6, tbsize=1, lsb_in=true, lsb=true)"),
                New VideoFilter("Noise", "DFTTest | DFTTest 16Bit | Strong DeNoise", "dfttest(sigma=64, tbsize=1, lsb_in=true, lsb=true)"),
                New VideoFilter("Noise", "DFTTest | DFTTest 16Bit | Moderate DeNoise with Dither", "dfttest(sigma=16, tbsize=1, lsb_in=true, lsb=true, dither=1)")}})

        Add(New PluginPackage With {
            .Name = "MT Expand Multi",
            .DirPath = "Plugins\AVS\Dither",
            .Description = "Calls mt_expand or mt_inpand multiple times in order to grow or shrink the mask from the desired width and height.",
            .Filename = "mt_xxpand_multi.avsi",
            .WebURL = "http://avisynth.nl/index.php/Dither",
            .AvsFilterNames = {"mt_expand_multi", "mt_inpand_multi"}})

        Add(New PluginPackage With {
            .Name = "TEMmod",
            .Description = "TEMmod creates an edge mask using gradient vector magnitude. ",
            .Filename = "TEMmod.dll",
            .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/TEMmod",
            .AvsFilterNames = {"TEMmod"}})

        Add(New PluginPackage With {
            .Name = "AVSTP",
            .DirPath = "Plugins\AVS\AVSTP",
            .Description = "AVSTP is a programming library for Avisynth plug-in developers. It helps supporting native multi-threading in plug-ins. It works by sharing a thread pool between multiple plug-ins, so the number of threads stays low whatever the number of instantiated plug-ins. This helps saving resources, especially when working in an Avisynth MT environment. This documentation is mostly targeted to plug-ins developpers, but contains installation instructions for Avisynth users too.",
            .HelpFile = "avstp.html",
            .Filename = "avstp.dll",
            .WebURL = "http://avisynth.nl/index.php/AVSTP",
            .AvsFilterNames = {"Dither_y_gamma_to_linear", "Dither_y_linear_to_gamma", "Dither_convert_8_to_16", "Dither1Pre", "Dither1Pre", "Dither_repair16", "Dither_convert_yuv_to_rgb", "Dither_convert_rgb_to_yuv", "Dither_resize16", "DitherPost", "Dither_crop16", "DitherBuildMask", "SmoothGrad", "GradFun3", "Dither_box_filter16", "Dither_bilateral16", "Dither_limit_dif16", "Dither_resize16nr", "Dither_srgb_display", "Dither_convey_yuv4xxp16_on_yvxx", "Dither_convey_rgb48_on_yv12", "Dither_removegrain16", "Dither_median16", "Dither_get_msb", "Dither_get_lsb", "Dither_gen_null_lsb", "Dither_addborders16", "Dither_lut8", "Dither_lutxy8", "Dither_lutxyz8", "Dither_lut16", "Dither_add16", "Dither_sub16", "Dither_max_dif16", "Dither_min_dif16", "Dither_merge16", "Dither_merge16_8", "Dither_sigmoid_direct", "Dither_sigmoid_inverse", "Dither_add_grain16", "Dither_Luma_Rebuild"}})

        Add(New PluginPackage With {
            .Name = "Dither AVSI",
            .Description = "This package offers a set of tools to manipulate high-bitdepth (16 bits per plane) video clips. The most proeminent features are color banding artifact removal, dithering to 8 bits, colorspace conversions and resizing.",
            .HelpFile = "dither.html",
            .DirPath = "Plugins\AVS\Dither",
            .Filename = "dither.avsi",
            .WebURL = "http://avisynth.nl/index.php/Dither",
            .AvsFilterNames = {"Dither_y_gamma_to_linear", "Dither_y_linear_to_gamma", "Dither_convert_8_to_16", "Dither1Pre", "Dither1Pre", "Dither_repair16", "Dither_convert_yuv_to_rgb", "Dither_convert_rgb_to_yuv", "Dither_resize16", "DitherPost", "Dither_crop16", "DitherBuildMask", "SmoothGrad", "GradFun3", "Dither_box_filter16", "Dither_bilateral16", "Dither_limit_dif16", "Dither_resize16nr", "Dither_srgb_display", "Dither_convey_yuv4xxp16_on_yvxx", "Dither_convey_rgb48_on_yv12", "Dither_removegrain16", "Dither_median16", "Dither_get_msb", "Dither_get_lsb", "Dither_gen_null_lsb", "Dither_addborders16", "Dither_lut8", "Dither_lutxy8", "Dither_lutxyz8", "Dither_lut16", "Dither_add16", "Dither_sub16", "Dither_max_dif16", "Dither_min_dif16", "Dither_merge16", "Dither_merge16_8", "Dither_sigmoid_direct", "Dither_sigmoid_inverse", "Dither_add_grain16", "Dither_Luma_Rebuild"}})

        Add(New PluginPackage With {
            .Name = "Dither DLL",
            .DirPath = "Plugins\AVS\Dither",
            .Description = "This package offers a set of tools to manipulate high-bitdepth (16 bits per plane) video clips. The most proeminent features are color banding artifact removal, dithering to 8 bits, colorspace conversions and resizing.",
            .HelpFile = "dither.html",
            .Filename = "dither.dll",
            .WebURL = "http://avisynth.nl/index.php/Dither",
            .AvsFilterNames = {"Dither_y_gamma_to_linear", "Dither_y_linear_to_gamma", "Dither_convert_8_to_16", "Dither1Pre", "Dither1Pre", "Dither_repair16", "Dither_convert_yuv_to_rgb", "Dither_convert_rgb_to_yuv", "Dither_resize16", "DitherPost", "Dither_crop16", "DitherBuildMask", "SmoothGrad", "GradFun3", "Dither_box_filter16", "Dither_bilateral16", "Dither_limit_dif16", "Dither_resize16nr", "Dither_srgb_display", "Dither_convey_yuv4xxp16_on_yvxx", "Dither_convey_rgb48_on_yv12", "Dither_removegrain16", "Dither_median16", "Dither_get_msb", "Dither_get_lsb", "Dither_gen_null_lsb", "Dither_addborders16", "Dither_lut8", "Dither_lutxy8", "Dither_lutxyz8", "Dither_lut16", "Dither_add16", "Dither_sub16", "Dither_max_dif16", "Dither_min_dif16", "Dither_merge16", "Dither_merge16_8", "Dither_sigmoid_direct", "Dither_sigmoid_inverse", "Dither_add_grain16", "Dither_Luma_Rebuild"}})

        ''   Add(New PluginPackage With {
        '' .Name = "HDRColor",
        ''.Filename = "HDRColor.dll",
        ''.Description = "HDRColor is a high quality color correction filter for AviSynth.",
        ''.WebURL = "http://www.videoartifact.com/hdr/",
        ''.IsRequired = False,
        ''.HelpFile = "HDRColor.txt",
        ''.AvsFilterNames = {"cube", "Repair Y", "DynRangeY", "DynRange", "Color"},
        ''.AvsFiltersFunc = Function() {
        ''      New VideoFilter("Color", "HDRColor | Cube", "Cube(file=""$browse_file$"", from=$select:msg:Select Input Color Space;601;709;2020$, to=$select:msg:Select Output Color Space;601;709;2020$, rg=1.0, gg=1.0, bg=1.0, $select:msg:Select the Range you Wish to Use;TV Range|a=16, b=235, ao=16, bo=235;PC Range|a=0, b=255, ao=0, bo=255$, sat=1.0, sato=1.0, bitdepth=$select:msg:Select BitDepth;8;16;32;88$)"),
        ''    New VideoFilter("Color", "HDRColor | Repair Y", "RepairY($select:msg:Select Input Range;TV Range|a=16,b=235;PC Range|a=0,b=255$, black=0.0, hl=0.5, hlg=1.0, gain=1.0, $select:msg:Select Output Range;TV Range|ao=16,bo=235;PC Range|ao=0,bo=255$, g=2.2, mode=1, bitdepth=$select:msg:Select BitDepth;8;16;32;88$)"),
        ''  New VideoFilter("Color", "HDRColor | DynRangeY", "DynRangeY($select:msg:Select Y-Channel Range for Start Frame;TV Range|a1=16,b1=235;PCRange|a1=0,b1=255$, $select:msg:Select Y-Channel Range for End Frame;TV Range|a2=16,b2=235;PCRange|a2=0,b2=255$, g1=1.0, g2=1.0, $select:msg:Select The Output Range;TV Range|ao=16,bo=235;PC Range|ao=0,bo=255$, mode=1, bitdepth=$select:msg:Select BitDepth;8;16;32;88$)"),
        ''New VideoFilter("Color", "HDRColor | Color", "Color(sat=1.0, g=1.0, saty=1.0, satb=1.0, satg=1.0, satr=1.0, gy=1.0, gb=1.0, gg=1.0, gr=1.0, dyb=0.0, dgr=0.0, mode=1, bitdepth=$select:msg:Select BitDepth;8;16;32;88$)"),
        '' New VideoFilter("Color", "HDRColor | DynColor", "DynColor(sat1=1.0, sat2=1.0, g1=1.0, g2=1.0, saty1=1.0, saty2=1.0, satb1=1.0, satb2=1.0, satg1=1.0, satg2=1.0, satr1=1.0, satr2=1.0, gy1=1.0, gy2=1.0, gb1=1.0, gb2=1.0, gg1=1.0, gg2=1.0, gr1=1.0, gr2=1.0, dyb1=0.0, dyb2=0.0, dgr1=0.0, dgr2=0.0, mode=1, bitdepth=$select:msg:Select BitDepth;8;16;32;88$)")}})

        '                        Add(New PluginPackage With {
        '.Name = "HDRMatrix",
        '.Filename = "HDRMatrix.dll",
        '.Description = "HDRMatrix is a color matrix conversion filter library to work with HDR frames in AviSynth.",
        '.WebURL = "http://www.videoartifact.com/hdr/",
        '.IsRequired = False,
        '.HelpFile = "HDRMatrix.txt",
        '.AvsFilterNames = {"matrix"},
        '.AvsFiltersFunc = Function() {
        '    New VideoFilter("Color", "HDRColor | Matrix", "Matrix(from=$select:msg:Select Input For Color Matrix;601;709$, to=$select:msg:Select Output For Color Matrix;601;709$, rg=1.0, gg=1.0, bg=1.0, a=16, b=235, ao=0, bo=255, bitdepth=$select:msg:Select BitDepth;8;88;16;32$)")}})

        '                        Add(New PluginPackage With {
        '.Name = "BicubicSharp",
        '.Filename = "BicubicSharp.avsi",
        '.Description = "HDRSharp is a sharpening library to work with HDR frames in AviSynth.",
        '.DirPath = "Plugins\AVS\HDRSharp",
        '.WebURL = "http://www.videoartifact.com/hdr/",
        '.IsRequired = False,
        '.HelpFile = "HDRSharp.txt",
        '.AvsFilterNames = {"BicubicSharp", "UnsharpMask8Y", "UnsharpMask16Y", "BicubicSharp8"}})

        '                        Add(New PluginPackage With {
        '.Name = "HDRSharp",
        '.Filename = "HDRSharp.dll",
        '.Description = "HDRSharp is a sharpening library to work with HDR frames in AviSynth.",
        '.WebURL = "http://www.videoartifact.com/hdr/",
        '.DirPath = "Plugins\AVS\HDRSharp",
        '.IsRequired = False,
        '.HelpFile = "HDRSharp.txt",
        '.AvsFilterNames = {"BicubicSharp", "UnsharpMask8Y", "UnsharpMask16Y", "BicubicSharp8"},
        '.AvsFiltersFunc = Function() {
        '    New VideoFilter("Line", "HDRSharp | UnsharpMask8Y", "UnsharpMask8Y(mask, edges=1.0, g=2.2, wg=1.0, bl=0.0, str=1.0, mode=1, bitdepth=$select:msg:Select BitDepth;8;88;16$)" + BR + "#Mask=Yourmasking filter, Masking filter needs to be used with this Function"),
        '    New VideoFilter("Line", "HDRSharp | UnsharpMask8Y", "UnsharpMask8Y(mask, edges=1.0, g=2.2, wg=1.0, bl=0.0, str=1.0, mode=1, bitdepth=$select:msg:Select BitDepth;8;88;16$)" + BR + "#Mask=Yourmasking filter, Masking filter needs to be used with this Function"),
        '    New VideoFilter("Line", "HDRSharp | BicubicSharp", "BicubicSharp(bitdepth=$select:msg:Select BitDepth;8;88;16$)"),
        '    New VideoFilter("Line", "HDRSharp | BicubicSharp8", "BicubicSharp8()")}})

        '                        Add(New PluginPackage With {
        '.Name = "HDRCore",
        '.DirPath = "Plugins\AVS\HDRCore",
        '.Description = "HDRCore library adds 16-bit integer (stacked and linear) and 32-bit floating point pixel types. ",
        '.IsRequired = False,
        '.HelpFile = "HDRCore.txt",
        '.Filename = "HDRCore.dll",
        '.WebURL = "http://www.videoartifact.com/hdr/",
        '.AvsFilterNames = {"BitdepthMsb", "BitdepthMsbLsb", "BitdepthLsb", "Bitdepth", "RangeY", "RangeUV", "ClampHDR"}})

        '                        Add(New PluginPackage With {
        '.Name = "HDRNoise",
        '.DirPath = "Plugins\AVS\HDRNoise",
        '.Description = "HDRNoise is a noise library to work with HDR frames in AviSynth.",
        '.IsRequired = False,
        '.HelpFile = "HDRNoise.txt",
        '.Filename = "HDRNoise.dll",
        '.WebURL = "http://www.videoartifact.com/hdr/",
        '.AvsFilterNames = {"NoiseY", "NoiseYM", "NoiseUV", "NoisedGrayY8", "NoiseMix1Y", "NoiseMix2Y"}})

        '                        Add(New PluginPackage With {
        '.Name = "FilmGrain",
        '.DirPath = "Plugins\AVS\HDRNoise",
        '.Description = "HDRNoise is a noise library to work with HDR frames in AviSynth.",
        '.IsRequired = False,
        '.HelpFile = "HDRNoise.txt",
        '.Filename = "FilmGrain.avsi",
        '.WebURL = "http://www.videoartifact.com/hdr/",
        '.AvsFilterNames = {"FilmGrain1", "FilmGrain2"}})

        '                        Add(New PluginPackage With {
        '.Name = "HDR Sources",
        '.Description = "HDRCore library adds 16-bit integer (stacked and linear) and 32-bit floating point pixel types.",
        '.DirPath = "Plugins\AVS\HDRCore",
        '.IsRequired = False,
        '.Filename = "HDR-sources.avsi",
        '.HelpFile = "HDRCore.txt",
        '.WebURL = "http://www.videoartifact.com/hdr/",
        '.AvsFilterNames = {"VideoDS", "AudioDS", "VideoLS", "AudioLS", "VideoLA", "AudioLA", "AudioSilent"}})

        Add(New PluginPackage With {
            .Name = "DeGrainMedian",
            .Filename = "DeGrainMedian.dll",
            .Description = "DeGrainMedian is a spatio-temporal limited median filter mainly for film grain removal, but may be used for general denoising.",
            .HelpFile = "Degrainmedian.html",
            .WebURL = "http://avisynth.nl/index.php/DeGrainMedian",
            .AvsFilterNames = {"DeGrainMedian"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Noise", "RemoveGrain | DeGrainMedian", "DeGrainMedian(limitY=4, limitUV=6, mode=1, interlaced=false, norow=false)")}})

        Add(New PluginPackage With {
            .Name = "pSharpen",
            .Filename = "pSharpen.avsi",
            .AvsFilterNames = {"pSharpen"},
            .Description = "pSharpen performs two-point sharpening to avoid overshoot.",
            .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/PSharpen",
            .AvsFiltersFunc = Function() {
                New VideoFilter("Line", "Sharpen | pSharpen", "pSharpen(strength=25, threshold=75, ss_x=1.0, ss_y=1.0)")}})

        Add(New PluginPackage With {
          .Name = "GradFun2DBmod",
          .DirPath = "Plugins\AVS\GradFun2DB",
          .Filename = "GradFun2DBmod.avsi",
          .HelpFile = "Readme.txt",
           .WebURL = "http://avisynth.nl/index.php/GradFun2dbmod",
           .Description = "An advanced debanding script based on GradFun2DB.",
          .AvsFilterNames = {"GradFun2DBmod"}})

        Add(New PluginPackage With {
            .Name = "GradFun2DB",
            .DirPath = "Plugins\AVS\GradFun2DB",
            .Filename = "gradfun2db.dll",
            .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/GradFun2db",
            .Description = "A simple and fast debanding filter.",
            .AvsFilterNames = {"gradfun2db"}})

        Add(New PluginPackage With {
            .Name = "AddGrainC",
            .Filename = "AddGrainC.dll",
            .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/AddGrainC",
            .Description = "Generate film-like grain or other effects (like rain) by adding random noise to a video clip.",
            .AvsFilterNames = {"AddGrainC", "AddGrain"}})

        Add(New PluginPackage With {
            .Name = "YFRC",
            .Filename = "YFRC.avsi",
            .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/YFRC",
            .Description = "Yushko Frame Rate Converter - doubles the frame rate with strong artifact detection and scene change detection. YFRC uses masks to reduce artifacts in areas where interpolation failed.",
            .AvsFilterNames = {"YFRC"},
            .AvsFiltersFunc = Function() {
            New VideoFilter("FrameRate", "YRFC", "YFRC(BlockH=16, BlockV=16, OverlayType=0, MaskExpand=1)")}})

        Add(New PluginPackage With {
            .Name = "MCTemporalDenoise",
            .Filename = "MCTemporalDenoise.avsi",
            .HelpFile = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/Abcxyz",
            .Description = "A motion compensated noise removal script with an accompanying post-processing component.",
            .AvsFilterNames = {"MCTemporalDenoise", "MCTemporalDenoisePP"},
            .AvsFiltersFunc = Function() {
            New VideoFilter("Noise", "MCTemporalDenoise | MCTemporalDenoise", "MCTemporalDenoise(settings=""medium"")"),
            New VideoFilter("Noise", "MCTemporalDenoise | MCTemporalDenoisePP", "source=last" + BR + "denoised=FFT3Dfilter()" + BR + "MCTemporalDenoisePP(denoised)")}})


        Add(New PluginPackage With {
            .Name = "L-SMASH-Works",
            .Filename = "LSMASHSource.dll",
            .Description = "AviSynth and VapourSynth source filter based on Libav supporting a wide range of input formats.",
            .WebURL = "http://avisynth.nl/index.php/LSMASHSource",
            .HelpURL = "http://github.com/VFR-maniac/L-SMASH-Works/blob/master/AviSynth/README",
            .AvsFilterNames = {"LSMASHVideoSource", "LSMASHAudioSource", "LWLibavVideoSource", "LWLibavAudioSource"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Source", "LSMASHVideoSource", "LSMASHVideoSource(""%source_file%"", format = ""YUV420P8"")"),
                New VideoFilter("Source", "LWLibavVideoSource", "LWLibavVideoSource(""%source_file%"", format = ""YUV420P8"")")}})

        Add(New PluginPackage With {
            .Name = "vslsmashsource",
            .Filename = "vslsmashsource.dll",
            .Description = "VapourSynth source filter based on Libav supporting a wide range of input formats.",
            .HelpURL = "http://github.com/VFR-maniac/L-SMASH-Works/blob/master/VapourSynth/README",
            .WebURL = "http://avisynth.nl/index.php/LSMASHSource",
            .VSFilterNames = {"lsmas.LibavSMASHSource", "lsmas.LWLibavSource"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Source", "LibavSMASHSource", "clip = core.lsmas.LibavSMASHSource(r""%source_file%"")"),
                New VideoFilter("Source", "LWLibavSource", "clip = core.lsmas.LWLibavSource(r""%source_file%"")")}})

        Add(New PluginPackage With {
            .Name = "Deblock",
            .Filename = "Deblock.dll",
            .DirPath = "Plugins\VS\Deblock",
            .Description = "Deblocking plugin using the deblocking filter of h264.",
            .URL = "http://github.com/HomeOfVapourSynthEvolution/VapourSynth-Deblock/",
            .VSFilterNames = {"deblock.Deblock"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Restoration", "Deblock | Deblock", "clip = core.deblock.Deblock(clip, quant = 25, aoffset = 0, boffset = 0)")}})

        Add(New PluginPackage With {
            .Name = "MSharpen",
            .Filename = "msharpen.dll",
            .WebURL = "http://avisynth.nl/index.php/MSharpen",
            .HelpFile = "Readme.txt",
            .AvsFilterNames = {"MSharpen"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Line", "Sharpen | MSharpen", "MSharpen(threshold = 10, strength = 100, highq = true, mask = false)")}})

        Add(New PluginPackage With {
            .Name = "aWarpSharp2",
            .Filename = "aWarpSharpMT.dll",
            .HelpFile = "aWarpSharp.txt",
            .AvsFilterNames = {"aBlur", "aSobel", "aWarp", "aWarp4", "aWarpSharp", "aWarpSharp2"},
            .Description = "This filter implements the same warp sharpening algorithm as aWarpSharp by Marc FD, but with several bugfixes and optimizations.",
            .WebURL = "http://avisynth.nl/index.php/AWarpSharp2",
            .AvsFiltersFunc = Function() {
                New VideoFilter("Line", "Sharpen | aWarpSharp2", "aWarpSharp2(thresh=128, blur=2, type=0, depth=16, chroma=3)")}})

        Add(New PluginPackage With {
            .Name = "QTGMC",
            .Filename = "QTGMC.avsi",
            .URL = "http://avisynth.nl/index.php/QTGMC",
            .Description = "A very high quality deinterlacer with a range of features for both quality and convenience. These include a simple presets system, extensive noise processing capabilities, support for repair of progressive material, precision source matching, shutter speed simulation, etc. Originally based on TempGaussMC by Didée.",
            .AvsFilterNames = {"QTGMC"},
            .AvsFiltersFunc = Function() {
            New VideoFilter("Field", "QTGMC | QTGMC...", "QTGMC(Preset = ""$select:msg:Select a preset.;Draft;Ultra Fast;Super Fast;Very Fast;Faster;Fast;Medium;Slow;Slower;Very Slow;Placebo$"", InputType=$select:msg:Select Input Type;Interlaced|0;Progressive Type 1|1;Progressive Type 2|2;Progressive Type 3|3$, SourceMatch=3, Sharpness=0.2, TR2=2, EdiThreads=8)"),
            New VideoFilter("Field", "QTGMC | QTGMC With Repair", "QTGMC1 = QTGMC( Preset=""Slower"", InputType=2 )" + BR + "QTGMC2 = QTGMC( Preset=""Slower"", InputType=3, PrevGlobals=""Reuse"")" + BR + "$select:msg:Select the Filter Form you Wish to use. Repair|Repair(QTGMC1, QTGMC2, 1);Repair16|Repair16(QTGMC1, QTGMC2, 1)$")}})

        Add(New PluginPackage With {
            .Name = "SMDegrain",
            .Filename = "SMDegrain.avsi",
            .URL = "http://avisynth.nl/index.php/SMDegrain",
            .Description = "SMDegrain, the Simple MDegrain Mod, is mainly a convenience function for using MVTools.",
            .AvsFilterNames = {"SMDegrain", "MDegrain2"},
            .AvsFiltersFunc = Function() {
            New VideoFilter("Noise", "RemoveGrain | SMDegrain | SMDGrain", "SMDegrain(tr = 2, thSAD = 250, contrasharp = false, refinemotion = true, lsb = false)"),
            New VideoFilter("Noise", "RemoveGrain | SMDegrain | SMDGrain With Motion Vectors", "super_search = Dither_Luma_Rebuild(S0=1.0,c=0.0625).MSuper(rfilter=4)" + BR + "bv2 = super_search.MAnalyse(isb = true,  delta = 2, overlap= 4)" + BR + "bv1 = super_search.MAnalyse(isb = true,  delta = 1, overlap= 4)" + BR + "fv1 = super_search.MAnalyse(isb = false, delta = 1, overlap= 4)" + BR + "fv2 = super_search.MAnalyse(isb = false, delta = 2, overlap= 4)" + BR + "MDegrain2(MSuper(levels = 1), bv1, fv1, bv2, fv2, thSAD = 300, thSADC = 150)"),
            New VideoFilter("Noise", "RemoveGrain | SMDegrain | SMDGrain 16Bit", "sharp=last" + BR + "dfttest(tbsize=1, sigma = 10, lsb = True)" + BR + "SMDegrain(tr=3, thSAD = 300, CClip = sharp, lsb_in = True, lsb_out = True)")}})

        Add(New PluginPackage With {
            .Name = "mClean",
            .Filename = "mClean.avsi",
            .URL = "http://forum.doom9.org/showthread.php?t=174804",
            .Description = "Removes noise whilst retaining as much detail as possible.",
            .AvsFilterNames = {"mClean"},
            .AvsFiltersFunc = Function() {New VideoFilter("Noise", "mClean", "mClean()")}})

        Add(New PluginPackage With {
            .Name = "LSFmod",
            .Filename = "LSFmod.avsi",
            .URL = "http://avisynth.nl/index.php/LSFmod",
            .Description = "A LimitedSharpenFaster mod with a lot of new features and optimizations.",
            .AvsFilterNames = {"LSFmod"},
            .AvsFiltersFunc = Function() {New VideoFilter("Line", "Sharpen | LSFmod", "LSFmod(strength = 100, Smode=5, Smethod=3, kernel=11, preblur=""OFF"", secure=true, Szrp= 16, Spwr= 4, SdmpLo= 4, SdmpHi= 48, Lmode=4, overshoot=1, undershoot=1, Overshoot2=1, Undershoot2=1, soft=-2, soothe=true, keep=20, edgemode=0, edgemaskHQ=true, ss_x= 1.50, ss_y=1.50 , dest_x=%target_width%, dest_y=%target_height%, show=false, screenW=1280, screenH=1024)")}})

        Add(New PluginPackage With {
            .Name = "vsCube",
            .Filename = "vscube.dll",
            .Description = "Deblocking plugin using the deblocking filter of h264.",
            .HelpFile = "Readme.txt",
            .WebURL = "http://rationalqm.us/mine.html",
            .DirPath = "Plugins\AVS\VSCube",
            .AvsFilterNames = {"Cube"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Color", "HDRColor | 3DLutCube", "Cube(""$browse_file$"")")}})

        Add(New PluginPackage With {
            .Name = "RgTools",
            .Filename = "RgTools.dll",
            .URL = "http://github.com/pinterf/RgTools",
            .Description = "RgTools is a modern rewrite of RemoveGrain, Repair, BackwardClense, Clense, ForwardClense and VerticalCleaner all in a single plugin.",
            .AvsFilterNames = {"RemoveGrain", "Clense", "ForwardClense", "BackwardClense", "Repair", "VerticalCleaner"},
            .AvsFiltersFunc = Function() {
               New VideoFilter("Noise", "RemoveGrain | RemoveGrain | RemoveGrain", "RemoveGrain()"),
               New VideoFilter("Noise", "RemoveGrain | RemoveGrain | RemoveGrain With Repair", "Processed = RemoveGrain(Unprocessed, mode=2, modeU=2, modeV=2, planar=false)" + BR + "Repair(Processed, Unprocessed, mode=2, modeU=2, modeV=2, planar=false)" + BR + "#Unprocessed is Clip Source, You must make this adjustment manually")}})

        Add(New PluginPackage With {
            .Name = "JPSDR",
            .Filename = "Plugins_JPSDR.dll",
            .WebURL = "http://forum.doom9.org/showthread.php?t=174248",
            .Description = "Merge of AutoYUY2, NNEDI3 and ResampleMT",
            .AvsFilterNames = {"nnedi3", "nnedi3_rpow2", "AutoYUY2", "PointResizeMT", "BilinearResizeMT", "BicubicResizeMT", "LanczosResizeMT", "Lanczos4ResizeMT", "BlackmanResizeMT", "Spline16ResizeMT", "Spline36ResizeMT", "Spline64ResizeMT", "GaussResizeMT", "SincResizeMT", "DeBilinearResizeMT", "DeBicubicResizeMT", "DeLanczosResizeMT", "DeLanczos4ResizeMT", "DeBlackmanResizeMT", "DeSpline16ResizeMT", "DeSpline36ResizeMT", "DeSpline64ResizeMT", "DeGaussResizeMT", "DeSincResizeMT"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Field", "nnedi3", "nnedi3(field = 1)"),
                New VideoFilter("Resize", "Resize | ResizeMT", "$select:BicubicResizeMT;BilinearResizeMT;BlackmanResizeMT;GaussResizeMT;Lanczos4ResizeMT;LanczosResizeMT;PointResizeMT;SincResizeMT;Spline16ResizeMT;Spline36ResizeMT;Spline64ResizeMT$(%target_width%, %target_height%, prefetch = 4)")}})

        Add(New PluginPackage With {
            .Name = "TIVTC",
            .Filename = "TIVTC.dll",
            .WebURL = "http://github.com/pinterf/TIVTC",
            .HelpURL = "http://avisynth.nl/index.php/TIVTC",
            .Description = "TIVTC is a plugin package containing 7 different filters and 3 conditional functions.",
            .AvsFilterNames = {"TFM", "TDecimate", "MergeHints", "FrameDiff", "FieldDiff", "ShowCombedTIVTC", "RequestLinear"}})

        Add(New PluginPackage With {
            .Name = "masktools2",
            .Filename = "masktools2.dll",
            .URL = "http://github.com/pinterf/masktools",
            .Description = "MaskTools2 contain a set of filters designed to create, manipulate and use masks. Masks, in video processing, are a way to give a relative importance to each pixel. You can, for example, create a mask that selects only the green parts of the video, and then replace those parts with another video.",
            .AvsFilterNames = {"mt_adddiff", "mt_average", "mt_binarize", "mt_circle", "mt_clamp", "mt_convolution", "mt_diamond", "mt_edge", "mt_ellipse", "mt_expand", "mt_hysteresis", "mt_inflate", "mt_inpand", "mt_invert", "mt_logic", "mt_losange", "mt_lut", "mt_lutf", "mt_luts", "mt_lutxy", "mt_makediff", "mt_mappedblur", "mt_merge", "mt_motion", "mt_polish", "mt_rectangle", "mt_square"}})

        Add(New PluginPackage With {
            .Name = "FluxSmooth",
            .Filename = "FluxSmoothSSSE3.dll",
            .AvsFilterNames = {"FluxSmoothT", "FluxSmoothST"},
            .Description = "One of the fundamental properties of noise is that it's random. One of the fundamental properties of motion is that it's not. This is the premise behind FluxSmooth, which examines each pixel and compares it to the corresponding pixel in the previous and last frame. Smoothing occurs if both the previous frame's value and the next frame's value are greater, or if both are less, than the value in the current frame.",
            .WebURL = "http://avisynth.nl/index.php/FluxSmooth"})

        Add(New PluginPackage With {
            .Name = "yadifmod2",
            .Filename = "yadifmod2.dll",
            .Description = "Yet Another Deinterlacing Filter mod  for Avisynth2.6/Avisynth+",
            .HelpURL = "http://github.com/chikuzen/yadifmod2/blob/master/avisynth/readme.md",
            .WebURL = "http://github.com/chikuzen/yadifmod2",
            .AvsFilterNames = {"yadifmod2"},
            .AvsFiltersFunc = Function() {New VideoFilter("Field", "yadifmod2", "yadifmod2()")}})

        Add(New PluginPackage With {
            .Name = "FrameRateConverter DLL",
            .Filename = "FrameRateConverter.dll",
            .DirPath = "Plugins\AVS\FrameRateConverter",
            .Description = "Increases the frame rate with interpolation and fine artifact removal.",
            .URL = "http://github.com/mysteryx93/FrameRateConverter",
            .AvsFilterNames = {"FrameRateConverter"}})

        Add(New PluginPackage With {
            .Name = "FrameRateConverter AVSI",
            .Filename = "FrameRateConverter.avsi",
            .DirPath = "Plugins\AVS\FrameRateConverter",
            .Description = "Increases the frame rate with interpolation and fine artifact removal.",
            .URL = "http://github.com/mysteryx93/FrameRateConverter",
            .AvsFilterNames = {"FrameRateConverter"},
            .AvsFiltersFunc = Function() {New VideoFilter("FrameRate", "FrameRateConverter", "FrameRateConverter()")}})

        Add(New PluginPackage With {
            .Name = "DCTFilter",
            .Filename = "DCTFilter.dll",
            .Description = "A rewrite of DctFilter for Avisynth+.",
            .URL = "http://github.com/chikuzen/DCTFilter",
            .AvsFilterNames = {"DCTFilter", "DCTFilterD", "DCTFilter4", "DCTFilter4D", "DCTFilter8", "DCTFilter8D"}})

        Add(New PluginPackage With {
            .Name = "vcmod",
            .Filename = "vcmod.dll",
            .Description = "vcmod plugin for VapourSynth.",
            .URL = "http://www.avisynth.nl/users/vcmohan/vcmod/vcmod.html",
            .VSFilterNames = {"vcmod.Median", "vcmod.Variance", "vcmod.Amplitude", "vcmod.GBlur", "vcmod.MBlur", "vcmod.Histogram"}})

        Add(New PluginPackage With {
            .Name = "Yadifmod",
            .Filename = "Yadifmod.dll",
            .Description = "Modified version of Fizick's avisynth filter port of yadif from mplayer. This version doesn't internally generate spatial predictions, but takes them from an external clip.",
            .WebURL = "http://github.com/HomeOfVapourSynthEvolution/VapourSynth-Yadifmod",
            .VSFilterNames = {"yadifmod.Yadifmod"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Field", "Yadifmod", "clip = core.yadifmod.Yadifmod(clip, core.nnedi3.nnedi3(clip, field = 0), order = 1, field = -1, mode = 0)")}})

        Add(New PluginPackage With {
            .Name = "GradCurve",
            .Filename = "GradCurve.dll",
            .URL = "http://github.com/xekon/GradCurve",
            .Description = "VapourSynth port of Gradation Curves Virtual Dub Plugin.",
            .VSFilterNames = {"grad.Curve"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Misc", "GradCurve", "clip = core.grad.Curve(clip, fname = r""$browse_file$"", ftype = 1, pmode = 1)")}})

        Add(New PluginPackage With {
            .Name = "nnedi3",
            .Filename = "libnnedi3.dll",
            .WebURL = "http://github.com/dubhater/vapoursynth-nnedi3",
            .Description = "nnedi3 is an intra-field only deinterlacer. It takes in a frame, throws away one field, and then interpolates the missing pixels using only information from the kept field.",
            .VSFilterNames = {"nnedi3.nnedi3", "nnedi3.nnedi3_rpow2"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Field", "nnedi3", "clip = core.nnedi3.nnedi3(clip, field = 1)")}})

        Add(New PluginPackage With {
            .Name = "FFT3DFilter",
            .Filename = "fft3dfilter.dll",
            .WebURL = "http://github.com/VFR-maniac/VapourSynth-FFT3DFilter",
            .Description = "FFT3DFilter uses Fast Fourier Transform method for image processing in frequency domain.",
            .VSFilterNames = {"fft3dfilter.FFT3DFilter"}})

        Add(New PluginPackage With {
            .Name = "mvtools",
            .Filename = "libmvtools.dll",
            .WebURL = "http://github.com/dubhater/vapoursynth-mvtools",
            .Description = "MVTools is a set of filters for motion estimation and compensation.",
            .VSFilterNames = {"mv.Super", "mv.Analyse", "mv.Recalculate", "mv.Compensate", "mv.Degrain1", "mv.Degrain2",
                "mv.Degrain3", "mv.Mask", "mv.Finest", "mv.FlowBlur", "mv.FlowInter", "mv.FlowFPS", "mv.BlockFPS", "mv.SCDetection"}})

        Add(New PluginPackage With {
            .Name = "Sangnommod",
            .Filename = "libsangnom.dll",
            .WebURL = "https://bitbucket.org/James1201/vapoursynth-sangnom/overview",
            .Description = "SangNom is a single field deinterlacer using edge-directed interpolation but nowadays it's mainly used in anti-aliasing scripts.",
            .VSFilterNames = {"sangnom.SangNom"}})

        Add(New PluginPackage With {
            .Name = "adjust",
            .Filename = "adjust.py",
            .Description = "very basic port of the built-in Avisynth filter Tweak.",
            .URL = "http://github.com/dubhater/vapoursynth-adjust",
            .VSFilterNames = {"adjust.Tweak"}})

        Add(New PluginPackage With {
            .Name = "mvsfunc",
            .Filename = "mvsfunc.py",
            .VSFilterNames = {
                "mvsfunc.Depth", "mvsfunc.ToRGB", "mvsfunc.ToYUV", "mvsfunc.BM3D",
                "mvsfunc.PlaneStatistics", "mvsfunc.PlaneCompare", "mvsfunc.ShowAverage",
                "mvsfunc.FilterIf", "mvsfunc.FilterCombed", "mvsfunc.Min", "mvsfunc.Max",
                "mvsfunc.Avg", "mvsfunc.MinFilter", "mvsfunc.MaxFilter", "mvsfunc.LimitFilter",
                "mvsfunc.PointPower", "mvsfunc.SetColorSpace", "mvsfunc.AssumeFrame",
                "mvsfunc.AssumeTFF", "mvsfunc.AssumeBFF", "mvsfunc.AssumeField",
                "mvsfunc.AssumeCombed", "mvsfunc.CheckVersion", "mvsfunc.GetMatrix",
                "mvsfunc.Depth", "mvsfunc.GetPlane", "mvsfunc.PlaneAverage"
            },
            .Description = "mawen1250's VapourSynth functions.",
            .HelpURL = "http://forum.doom9.org/showthread.php?t=172564",
            .WebURL = "http://github.com/HomeOfVapourSynthEvolution/mvsfunc"})

        Add(New PluginPackage With {
            .Name = "havsfunc",
            .Filename = "havsfunc.py",
            .VSFiltersFunc = Function() {
                New VideoFilter("Misc", "LSFmod", "clip = havsfunc.LSFmod(clip, strength = 100)"),
                New VideoFilter("Field", "QTGMC...", $"clip = core.std.SetFieldBased(clip, 2) # 1 = BFF, 2 = TFF{BR}clip = havsfunc.QTGMC(clip, TFF = True, Preset = ""$select:msg:Select a preset.;Draft;Ultra Fast;Super Fast;Very Fast;Faster;Fast;Medium;Slow;Slower;Very Slow;Placebo$"")")},
            .VSFilterNames = {"havsfunc.QTGMC", "havsfunc.ediaa", "havsfunc.daa", "havsfunc.maa",
                                       "havsfunc.SharpAAMCmod", "havsfunc.Deblock_QED", "havsfunc.DeHalo_alpha",
                                       "havsfunc.InterFrame", "havsfunc.FixChromaBleedingMod",
                                        "havsfunc.YAHR", "havsfunc.HQDeringmod", "havsfunc.ivtc_txt60mc",
                                       "havsfunc.Vinverse", "havsfunc.Vinverse2", "havsfunc.logoNR",
                                       "havsfunc.LUTDeCrawl", "havsfunc.LUTDeRainbow", "havsfunc.GSMC",
                                       "havsfunc.SMDegrain", "havsfunc.SmoothLevels", "havsfunc.FastLineDarkenMOD",
                                       "havsfunc.LSFmod", "havsfunc.GrainFactory3", "havsfunc.MCTemporalDenoise"},
            .Description = "Various popular AviSynth scripts ported to VapourSynth.",
            .WebURL = "http://github.com/HomeOfVapourSynthEvolution/havsfunc",
            .HelpURL = "http://forum.doom9.org/showthread.php?t=166582"})

        Add(New PluginPackage With {
            .Name = "d2vsource",
            .Filename = "d2vsource.dll",
            .Description = "Source filter to open D2V index files created with DGIndex or D2VWitch.",
            .URL = "http://github.com/dwbuiten/d2vsource",
            .VSFilterNames = {"d2v.Source"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Source", "d2vsource", "clip = core.d2v.Source(r""%source_file%"")")}})

        Add(New PluginPackage With {
            .Name = "FluxSmooth",
            .Filename = "libfluxsmooth.dll",
            .VSFilterNames = {"SmoothT", "SmoothST"},
            .Description = "FluxSmooth is a filter for smoothing of fluctuations.",
            .WebURL = "http://github.com/dubhater/vapoursynth-fluxsmooth"})

        Add(New PluginPackage With {
            .Name = "msmoosh",
            .Filename = "libmsmoosh.dll",
            .VSFilterNames = {"msmoosh.MSmooth", "msmoosh.MSharpen"},
            .Description = "MSmooth is a spatial smoother that doesn't touch edges." + BR + "MSharpen is a sharpener that tries to sharpen only edges.",
            .WebURL = "http://github.com/dubhater/vapoursynth-msmoosh",
            .VSFiltersFunc = Function() {
                New VideoFilter("Restoration", "Deblock | MSmooth", "clip = core.msmoosh.MSmooth(clip, threshold = 6.0, strength = 3)"),
                New VideoFilter("Line", "Sharpen | MSharpen", "clip = core.msmoosh.MSharpen(clip, threshold = 6.0, strength = 39)")}})

        Add(New PluginPackage With {
            .Name = "SVPFlow 1",
            .DirPath = "Plugins\VS\SVPFlow",
            .HelpFile = "Readme.txt",
            .Description = "Motion vectors search plugin  is a deeply refactored and modified version of MVTools2 Avisynth plugin",
            .Filename = "svpflow1_vs64.dll",
            .WebURL = "https://www.svp-team.com/wiki/Manual:SVPflow",
            .VSFilterNames = {"core.svp1.Super", "core.svp1.Analyse"}})

        Add(New PluginPackage With {
            .Name = "SVPFlow 2",
            .DirPath = "Plugins\VS\SVPFlow",
            .HelpFile = "Readme.txt",
            .Description = "Motion vectors search plugin is a deeply refactored and modified version of MVTools2 Avisynth plugin",
            .Filename = "svpflow2_vs64.dll",
            .WebURL = "https://www.svp-team.com/wiki/Manual:SVPflow",
            .VSFilterNames = {"core.svp2.SmoothFps", "core.svp2.Convert"}})

        Add(New PluginPackage With {
            .Name = "Dither",
            .DirPath = "Plugins\VS\Dither",
            .Description = "VapourSynth port of DitherTools",
            .Filename = "Dither.py",
            .WebURL = "https://github.com/IFeelBloated/VaporSynth-Functions",
            .VSFilterNames = {"Dither.sigmoid_direct", "Dither.sigmoid_inverse", "Dither.linear_and_gamma", "Dither.gamma_to_linear", "Dither.clamp16", "Dither.sbr16", "Dither.Resize16nr", "Dither.get_msb", "Dither.get_lsb"}})

        Add(New PluginPackage With {
            .Name = "ToneMap",
            .Filename = "tonemap.dll",
            .Description = "Tonemap plugin for VapourSynth",
            .WebURL = "https://github.com/ifb/vapoursynth-tonemap",
            .VSFilterNames = {"tonemap.Hable", "tonemap.mobius", "tonemap.Reinhard"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Color", "HDRColor | ToneMapping", "$select:msg:Select Tone Mapping;Hable|clip = core.tonemap.Hable(clip, exposure=2.000, a=0.150, b=0.500, d=0.200, c=0.100, e=0.020, f=0.300, w=11.200);Mobius|clip = core.tonemap.Mobius(clip, exposure=2.000, transition=0.300, peak=1.000);clip = core.tonemap.Reinhard(clip, exposure=2.000, contrast=0.500, peak=1.000)$")}})

        Add(New PluginPackage With {
            .Name = "DeblockPP7",
            .Filename = "DeblockPP7.dll",
            .Description = "VapourSynth port of pp7 from MPlayer.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DeblockPP7",
            .VSFilterNames = {"pp7.DeblockPP7"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Restoration", "Deblock | Deblock PP7", "$select:msg:Select Strength;Medium|clip = core.pp7.DeblockPP7(clip=clip, mode=2);Hard|clip = core.pp7.DeblockPP7(clip=clip);Soft|clip = core.pp7.DeblockPP7(clip=clip, mode=1)$")}})

        Add(New PluginPackage With {
            .Name = "HQDN3D",
            .Filename = "libhqdn3d.dll",
            .Description = "Avisynth port of hqdn3d from avisynth/mplayer.",
            .WebURL = "https://github.com/Hinterwaeldlers/vapoursynth-hqdn3d",
            .VSFilterNames = {"hqdn3d.Hqdn3d"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Noise", "HQDN3D", "clip = core.hqdn3d.Hqdn3d(clip=clip)")}})

        Add(New PluginPackage With {
            .Name = "DFTTest",
            .Filename = "DFTTest.dll",
            .Description = "VapourSynth port of dfttest.",
            .WebURL = "https://github.com/Hinterwaeldlers/vapoursynth-hqdn3d",
            .VSFilterNames = {"dfttest.DFTTest"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Noise", "DFTTest | DFTTest 8Bit", "$select:msg:Select Strength;Moderate DeNoise|clip = core.dfttest.DFTTest(clip, sigma=16, tbsize=5);Light DeNoise|clip = core.dfttest.DFTTest(clip, sigma=6, tbsize=1);Strong DeNoise|clip = core.dfttest.DFTTest(clip, sigma=64, tbsize=1)$"),
                New VideoFilter("Noise", "DFTTest | DFTTest 16Bit", "$select:msg:Select Strength;Moderate DeNoise|clip = core.dfttest.DFTTest(clip, sigma=16, tbsize=5, lsb_in=true, lsb=true);Light DeNoise|clip = core.dfttest.DFTTest(clip, sigma=6, tbsize=1, lsb_in=true, lsb=true);Strong DeNoise|clip = core.dfttest.DFTTest(clip, sigma=64, tbsize=1, lsb_in=true, lsb=true);Moderate DeNoise with Dither|clip = core.dfttest.DFTTest(clip, sigma=16, tbsize=1, lsb_in=true, lsb=true, dither=1)$")}})

        Add(New PluginPackage With {
            .Name = "CTMF",
            .Filename = "CTMF.dll",
            .Description = "Constant Time Median Filtering.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-CTMF",
            .VSFilterNames = {"ctmf.CTMF"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Noise", "CTMF", "clip = core.ctmf.CTMF(clip=clip, memsize=1048576, planes=[0,1,2])")}})

        Add(New PluginPackage With {
            .Name = "VagueDenoiser",
            .Filename = "VagueDenoiser.dll",
            .Description = "VapourSynth port of VagueDenoiser.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-VagueDenoiser",
            .VSFilterNames = {"vd.VagueDenoiser"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Noise", "VagueDenoiser", "clip = core.vd.VagueDenoiser(clip=clip, method=$select:msg:Select Strength;Soft|1;Hard|0)$")}})

        Add(New PluginPackage With {
            .Name = "TTempSmooth",
            .Filename = "TTempSmooth.dll",
            .Description = "VapourSynth port of TTempSmooth.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-TTempSmooth",
            .VSFilterNames = {"ttmpsm.TTempSmooth"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Noise", "TTempSmooth", "clip =  core.ttmpsm.TTempSmooth(clip=clip)")}})

        Add(New PluginPackage With {
            .Name = "DegrainMedian",
            .Filename = "libdegrainmedian.dll",
            .Description = "VapourSynth port of DegrainMedian",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-TTempSmooth",
            .VSFilterNames = {"dgm.DegrainMedian"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Restoration", "DeGrain | DegrainMedian", "clip = core.dgm.DegrainMedian(clip=clip, interlaced=False)")}})

        Add(New PluginPackage With {
            .Name = "psharpen",
            .Filename = "psharpen.py",
            .Description = "VapourSynth port of pSharpen",
            .VSFilterNames = {"psharpen.psharpen"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Line", "Sharpen | pSharpen", "clip = psharpen.psharpen(clip)")}})

        Add(New PluginPackage With {
            .Name = "AWarpSharp2",
            .Filename = "libawarpsharp2.dll",
            .Description = "VapourSynth port of AWarpSharp2",
            .WebURL = "https://github.com/dubhater/vapoursynth-awarpsharp2",
            .VSFilterNames = {"warp.AWarpSharp2"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Line", "Sharpen | aWarpSharpen2", "clip = core.warp.AWarpSharp2(clip=clip, blur=2)")}})

        Add(New PluginPackage With {
            .Name = "fmtconv",
            .Filename = "fmtconv.dll",
            .WebURL = "http://github.com/EleonoreMizo/fmtconv",
            .HelpFile = "fmtconv.html",
            .Description = "Fmtconv is a format-conversion plug-in for the Vapoursynth video processing engine. It does resizing, bitdepth conversion with dithering and colorspace conversion.",
            .VSFilterNames = {"fmtc.bitdepth", "fmtc.convert", "fmtc.matrix", "fmtc.resample", "fmtc.transfer", "fmtc.primaries"}})

        Add(New PluginPackage With {
            .Name = "finesharp",
            .Filename = "finesharp.py",
            .Description = "Port of Didée's FineSharp script to VapourSynth.",
            .WebURL = "http://forum.doom9.org/showthread.php?p=1777860#post1777860",
            .VSFilterNames = {"finesharp.sharpen"},
            .VSFiltersFunc = Function() {
                New VideoFilter("Line", "Sharpen | finesharp", "clip = finesharp.sharpen(clip)")}})

        Add(New PluginPackage With {
            .Name = "FineSharp",
            .Filename = "FineSharp.avsi",
            .HelpFile = "Readme.txt",
            .Description = "Small and fast realtime-sharpening function for 1080p, or after scaling 720p -> 1080p. It's a generic sharpener only for good quality sources!",
            .WebURL = "http://avisynth.nl/index.php/FineSharp",
            .AvsFilterNames = {"FineSharp"},
            .AvsFiltersFunc = Function() {
            New VideoFilter("Line", "Sharpen | FineSharp | Light", "FineSharp(mode=1, sstr=2, cstr=0.8, xstr=0.19, lstr=1.49, pstr=1.272)"),
            New VideoFilter("Line", "Sharpen | FineSharp | Moderate", "FineSharp(mode=2, sstr=2.0,  cstr=1.3, xstr=0.0,  lstr=1.49, pstr=1.472)"),
            New VideoFilter("Line", "Sharpen | FineSharp | Strong", "FineSharp(mode=3, sstr=6.0,  cstr=1.3, xstr=0.0,  lstr=1.49, pstr=1.472)")}})

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

                If Not plugin.AvsFilterNames.NothingOrEmpty AndAlso
                    Not plugin.VSFilterNames.NothingOrEmpty Then

                    Return Name + " avs+vs"
                ElseIf Not plugin.AvsFilterNames.NothingOrEmpty Then
                    Return Name + " avs"
                ElseIf Not plugin.VSFilterNames.NothingOrEmpty Then
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

    Function GetStoredPath() As String
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

        Return ret
    End Function

    Overridable ReadOnly Property Path As String
        Get
            Dim ret = GetStoredPath()
            If File.Exists(ret) Then Return ret

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
                If Not plugin.VSFilterNames Is Nothing AndAlso Not plugin.AvsFilterNames Is Nothing Then
                    ret = Folder.Apps + "Plugins\both\" + Name + "\" + Filename
                    If File.Exists(ret) Then Return ret
                Else
                    If plugin.VSFilterNames Is Nothing Then
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
        AvsFilterNames = {"UnDot"}
    End Sub
End Class

Public Class NicAudioPackage
    Inherits PluginPackage

    Sub New()
        Name = "NicAudio"
        Filename = "NicAudio.dll"
        WebURL = "http://avisynth.org.ru/docs/english/externalfilters/nicaudio.htm"
        Description = "AviSynth audio source filter."
        AvsFilterNames = {"NicAC3Source", "NicDTSSource", "NicMPASource", "RaWavSource"}
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
        SetupFilename = "AviSynthPlus-MT-r2728.exe"
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
        DownloadURL = "http://www.python.org/downloads/windows/"
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
                Registry.CurrentUser.GetString("SOFTWARE\Python\PythonCore\3.6\InstallPath", "ExecutablePath"), ''Location will be Changed for next VS Release
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

    Property AvsFilterNames As String()
    Property VSFilterNames As String()
    Property VSFiltersFunc As Func(Of VideoFilter())
    Property AvsFiltersFunc As Func(Of VideoFilter())

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
            Not package.AvsFilterNames.NothingOrEmpty Then

            Dim fullScriptLower = p.Script.GetFullScript().ToLowerInvariant

            For Each filterName In package.AvsFilterNames
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
            Not package.VSFilterNames.NothingOrEmpty Then

            For Each filter In p.Script.Filters
                If filter.Active Then
                    For Each filterName In package.VSFilterNames
                        If filter.Script.Contains(filterName) Then Return True
                    Next
                End If
            Next
        End If
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
        HintDirFunc = Function() DGDecodeNV.GetStoredPath.Dir
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
        HintDirFunc = Function() DGDecodeIM.GetStoredPath.Dir
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