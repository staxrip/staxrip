
Imports System.Text.RegularExpressions

Imports Microsoft.Win32

Public Class Package
    Implements IComparable(Of Package)

    Property Description As String
    Property DownloadURL As String
    Property Filename As String
    Property HelpFilename As String
    Property HelpSwitch As String
    Property HelpURL As String
    Property HelpUrlAviSynth As String
    Property HelpUrlVapourSynth As String
    Property HintDirFunc As Func(Of String)
    Property IgnorePath As String
    Property IsIncluded As Boolean = True
    Property Location As String
    Property Locations As String()
    Property Name As String
    Property RequiredFunc As Func(Of Boolean)
    Property Find As Boolean = True
    Property SetupAction As Action
    Property StatusFunc As Func(Of String)
    Property TreePath As String
    Property Version As String
    Property VersionAllowAny As Boolean
    Property VersionAllowNew As Boolean
    Property VersionAllowOld As Boolean = True
    Property VersionDate As Date
    Property WebURL As String

    Shared Property Items As New SortedDictionary(Of String, Package)

    Shared Property DGIndex As Package = Add(New Package With {
        .Name = "DGIndex",
        .Filename = "DGIndex.exe",
        .Description = "MPEG-2 demuxing and d2v indexing GUI app.",
        .Location = "Support\DGIndex",
        .RequiredFunc = Function() CommandLineDemuxer.IsActive("%app:DGIndex%")})

    Shared Property D2VWitch As Package = Add(New Package With {
        .Name = "D2V Witch",
        .Filename = "d2vwitch.exe",
        .Description = "Portable MPEG-2 demuxing and d2v indexing GUI app.",
        .WebURL = "https://github.com/dubhater/D2VWitch",
        .DownloadURL = "https://github.com/dubhater/D2VWitch/releases",
        .Location = "Support\D2V Witch",
        .RequiredFunc = Function() CommandLineDemuxer.IsActive("%app:D2V Witch%"),
        .LaunchAction = Sub()
                            g.AddToPath(Package.d2vsource.Directory)
                            g.Execute(D2VWitch.Path)
                        End Sub})

    Shared Property Haali As Package = Add(New Package With {
        .Name = "Haali Splitter",
        .Filename = "splitter.ax",
        .WebURL = "http://haali.su/mkv",
        .Description = "Haali Splitter is used by eac3to to write MKV files.",
        .Required = False,
        .IsIncluded = False,
        .Locations = {Registry.ClassesRoot.GetString("CLSID\" + GUIDS.HaaliMuxer.ToString + "\InprocServer32", Nothing).Dir}})

    Shared Property NicAudio As Package = Add(New PluginPackage With {
        .Name = "NicAudio",
        .Filename = "NicAudio.dll",
        .WebURL = "http://avisynth.org.ru/docs/english/externalfilters/nicaudio.htm",
        .Description = "AviSynth audio source filter plugin.",
        .AvsFilterNames = {"NicAC3Source", "NicDTSSource", "NicMPASource", "RaWavSource"}})

    Shared Property qaac As Package = Add(New Package With {
        .Name = "qaac",
        .Filename = "qaac64.exe",
        .Location = "Audio\qaac",
        .WebURL = "http://github.com/nu774/qaac",
        .HelpSwitch = "-h",
        .RequiredFunc = Function() TypeOf p.Audio0 Is GUIAudioProfile AndAlso DirectCast(p.Audio0, GUIAudioProfile).Params.Encoder = GuiAudioEncoder.qaac OrElse TypeOf p.Audio1 Is GUIAudioProfile AndAlso DirectCast(p.Audio1, GUIAudioProfile).Params.Encoder = GuiAudioEncoder.qaac,
        .Description = "Console AAC encoder based on the Apple AAC encoder."})

    Shared Property UnDot As Package = Add(New PluginPackage With {
        .Name = "UnDot",
        .Filename = "UnDot.dll",
        .WebURL = "http://avisynth.nl/index.php/UnDot",
        .Description = "UnDot is a simple median filter plugin for removing dots, that is stray orphan pixels and mosquito noise.",
        .AvsFilterNames = {"UnDot"}})

    Shared Property eac3to As Package = Add(New Package With {
        .Name = "eac3to",
        .Filename = "eac3to.exe",
        .Location = "Audio\eac3to",
        .WebURL = "http://forum.doom9.org/showthread.php?t=125966",
        .HelpURL = "http://en.wikibooks.org/wiki/Eac3to/How_to_Use",
        .HelpSwitch = "",
        .Description = "Audio convertor console app."})

    Shared Property ffmpeg As Package = Add(New Package With {
        .Name = "ffmpeg",
        .Filename = "ffmpeg.exe",
        .Location = "Encoders\ffmpeg",
        .WebURL = "http://ffmpeg.org",
        .HelpURL = "http://www.ffmpeg.org/documentation.html",
        .DownloadURL = "https://www.mediafire.com/folder/vkt2ckzjvt0qf/StaxRip_Tools",
        .HelpSwitch = "-h",
        .Description = "Versatile audio video convertor console app."})

    Shared Property ffmpeg_non_free As Package = Add(New Package With {
        .Name = "ffmpeg non-free",
        .Filename = "ffmpeg.exe",
        .WebURL = "http://ffmpeg.org",
        .HelpURL = "http://www.ffmpeg.org/documentation.html",
        .HelpSwitch = "-h",
        .IsIncluded = False,
        .VersionAllowAny = True,
        .Find = False,
        .Description = "Versatile audio video convertor console app. Custom build with non-free libraries like fdk-aac.",
        .RequiredFunc = Function() p.Audio0.ContainsCommand("libfdk_aac") OrElse p.Audio1.ContainsCommand("libfdk_aac")})

    Shared Property MediaInfo As Package = Add(New Package With {
        .Name = "MediaInfo",
        .Filename = "MediaInfo.dll",
        .Location = "Support\MediaInfo.NET",
        .WebURL = "http://mediaarea.net/en/MediaInfo",
        .DownloadURL = "https://mediaarea.net/en/MediaInfo/Download/Windows",
        .Description = "Library to retrieve info from media files."})

    Shared Property MediaInfoNET As Package = Add(New Package With {
        .Name = "MediaInfo.NET",
        .Filename = "MediaInfoNET.exe",
        .Location = "Support\MediaInfo.NET",
        .WebURL = "https://github.com/stax76/MediaInfo.NET",
        .DownloadURL = "https://github.com/stax76/MediaInfo.NET/releases",
        .Description = "GUI app originally built for StaxRip to show info about media files."})

    Shared Property GetMediaInfo As Package = Add(New Package With {
        .Name = "Get-MediaInfo",
        .Location = "Support\MediaInfo.NET",
        .Filename = "Get-MediaInfo.ps1",
        .Description = "Complete PowerShell MediaInfo solution used for the media info folder view.",
        .WebURL = "https://github.com/stax76/Get-MediaInfo",
        .DownloadURL = "https://github.com/stax76/Get-MediaInfo/releases"})

    Shared Property MP4Box As Package = Add(New Package With {
        .Name = "MP4Box",
        .Filename = "MP4Box.exe",
        .Location = "Support\MP4Box",
        .WebURL = "http://gpac.wp.mines-telecom.fr/",
        .HelpURL = "http://gpac.wp.mines-telecom.fr/mp4box/mp4box-documentation",
        .DownloadURL = "https://www.mediafire.com/folder/vkt2ckzjvt0qf/StaxRip_Tools",
        .HelpSwitch = "-h",
        .Description = "MP4Box is a MP4 muxing and demuxing console app."})

    Shared Property AviSynth As Package = Add(New Package With {
        .Name = "AviSynth",
        .Filename = "AviSynth.dll",
        .VersionAllowOld = False,
        .WebURL = "https://github.com/AviSynth/AviSynthPlus",
        .HelpURL = "http://avisynth.nl",
        .DownloadURL = "https://github.com/AviSynth/AviSynthPlus/releases",
        .Description = "Video processing scripting library.",
        .HintDirFunc = Function() Package.AviSynth.GetAviSynthHintDir,
        .RequiredFunc = Function() p.Script.Engine = ScriptEngine.AviSynth})

    Shared Property VapourSynth As Package = Add(New Package With {
        .Name = "VapourSynth",
        .Filename = "vapoursynth.dll",
        .Description = "Video processing Python scripting library.",
        .WebURL = "http://www.vapoursynth.com",
        .HelpURL = "http://www.vapoursynth.com/doc",
        .DownloadURL = "https://github.com/vapoursynth/vapoursynth/releases",
        .HelpFilename = "doc\index.html",
        .RequiredFunc = Function() p.Script.Engine = ScriptEngine.VapourSynth,
        .HintDirFunc = Function() Package.VapourSynth.GetVapourSynthHintDir})

    Shared Property vspipe As Package = Add(New Package With {
        .Name = "vspipe",
        .Filename = "vspipe.exe",
        .Description = "Console app that pipes VapourSynth scripts to video encoding console apps.",
        .WebURL = "http://www.vapoursynth.com",
        .HelpURL = "http://www.vapoursynth.com/doc/vspipe.html",
        .DownloadURL = "https://github.com/vapoursynth/vapoursynth/releases",
        .HelpSwitch = "stderr",
        .RequiredFunc = Function() p.Script.Engine = ScriptEngine.VapourSynth,
        .HintDirFunc = Function() Package.VapourSynth.GetVapourSynthHintDir})

    Shared Property Python As Package = Add(New Package With {
        .Name = "Python",
        .Filename = "python.exe",
        .TreePath = "Runtimes",
        .WebURL = "http://www.python.org",
        .HelpSwitch = "-h",
        .Description = "Scripting language used by VapourSynth.",
        .IgnorePath = "\WindowsApps\",
        .RequiredFunc = Function() p.Script.Engine = ScriptEngine.VapourSynth,
        .HintDirFunc = AddressOf GetPythonHintDir})

    Shared Property chapterEditor As Package = Add(New Package With {
        .Name = "chapterEditor",
        .Location = "Support\chapterEditor",
        .Filename = "chapterEditor.exe",
        .Description = "GUI app to edit chapters and menus for OGG, XML, TTXT, m.AVCHD, m.editions-mkv, Matroska Menu.",
        .WebURL = "https://forum.doom9.org/showthread.php?t=169984",
        .DownloadURL = "https://www.videohelp.com/software/chapterEditor"})

    Shared Property xvid_encraw As Package = Add(New Package With {
        .Name = "xvid_encraw",
        .Location = "Encoders\xvid_encraw",
        .Filename = "xvid_encraw.exe",
        .Description = "MPEG-4 video encoder console app.",
        .WebURL = "https://www.xvid.com",
        .DownloadURL = "https://www.mediafire.com/folder/vkt2ckzjvt0qf/StaxRip_Tools",
        .RequiredFunc = Function() TypeOf p.VideoEncoder Is BatchEncoder AndAlso DirectCast(p.VideoEncoder, BatchEncoder).CommandLines.Contains("xvid_encraw"),
        .IsIncluded = False,
        .HelpSwitch = "-h"})

    Shared Property VisualCpp2012 As Package = Add(New Package With {
        .Name = "Visual C++ 2012",
        .Filename = "msvcp110.dll",
        .Description = "Visual C++ 2012 Redistributable is required by some tools used by StaxRip.",
        .DownloadURL = "http://www.microsoft.com/en-US/download/details.aspx?id=30679",
        .Locations = {Folder.System, "Support\VC"},
        .VersionAllowAny = True,
        .TreePath = "Runtimes"})

    Shared Property VisualCpp2013 As Package = Add(New Package With {
        .Name = "Visual C++ 2013",
        .Filename = "msvcp120.dll",
        .Description = "Visual C++ 2013 Redistributable is required by some tools used by StaxRip.",
        .DownloadURL = "http://www.microsoft.com/en-US/download/details.aspx?id=40784",
        .VersionAllowAny = True,
        .Locations = {Folder.System, "Support\VC"},
        .TreePath = "Runtimes"})

    Shared Property VisualCpp2019 As Package = Add(New Package With {
        .Name = "Visual C++ 2019",
        .Filename = "msvcp140.dll",
        .Description = "Visual C++ Redistributable is required by many tools used by StaxRip.",
        .DownloadURL = "https://support.microsoft.com/en-gb/help/2977003/the-latest-supported-visual-c-downloads",
        .VersionAllowOld = False,
        .VersionAllowNew = True,
        .Locations = {Folder.System, "Support\VC"},
        .TreePath = "Runtimes"})

    Shared Property Decomb As Package = Add(New PluginPackage With {
        .Name = "Decomb",
        .Filename = "Decomb.dll",
        .WebURL = "http://rationalqm.us/decomb/decombnew.html",
        .HelpFilename = "DecombReferenceManual.html",
        .Description = "Video filter plugin that provides the means for removing combing artifacts from telecined progressive streams, interlaced streams, and mixtures thereof.",
        .AvsFilterNames = {"Telecide", "FieldDeinterlace", "Decimate", "IsCombed"}})

    Shared Property temporalsoften As Package = Add(New PluginPackage With {
        .Name = "temporalsoften",
        .Filename = "temporalsoften.dll",
        .VSFilterNames = {"TemporalSoften"}})

    Shared Property fdkaac As Package = Add(New Package With {
        .Name = "fdkaac",
        .Filename = "fdkaac.exe",
        .Location = "Audio\fdkaac",
        .HelpFilename = "help.txt",
        .HelpSwitch = "-h",
        .Description = "AAC console encoder based on libfdk-aac.",
        .WebURL = "http://github.com/nu774/fdkaac"})

    Shared Property AVSMeter As Package = Add(New Package With {
        .Name = "AVSMeter",
        .Location = "Support\AVSMeter",
        .Filename = "AVSMeter64.exe",
        .Description = "Console app that displays AviSynth script clip info.",
        .HelpFilename = "doc\AVSMeter.html",
        .WebURL = "http://forum.doom9.org/showthread.php?t=174797",
        .DownloadURL = "https://www.mediafire.com/folder/x6f7yqjufdg7c/Groucho%27s_Avisynth_Stuff",
        .HelpSwitch = ""})

    Shared Property BDSup2SubPP As Package = Add(New Package With {
        .Name = "BDSup2Sub++",
        .Filename = "bdsup2sub++.exe",
        .Location = "Subtitles\BDSup2Sub++",
        .WebURL = "https://github.com/amichaeltm/BDSup2SubPlusPlus",
        .DownloadURL = "https://github.com/amichaeltm/BDSup2SubPlusPlus/releases",
        .Description = "GUI app that converts Blu-ray subtitles to other formats like VobSub."})

    Shared Property MTN As Package = Add(New Package With {
        .Name = "mtn",
        .Filename = "mtn.exe",
        .Location = "Thumbnails\MTN",
        .Description = "Movie thumbnailer saves thumbnails (screenshots) of movie or video files to jpeg files. StaxRip uses a custom built version with HEVC support added in and also includes the latest FFMPEG.",
        .WebURL = "https://github.com/Revan654/Movie-Thumbnailer-mtn",
        .HelpURL = "http://moviethumbnail.sourceforge.net/usage.en.html",
        .HelpSwitch = ""})

    Shared Property SubtitleEdit As Package = Add(New Package With {
        .Name = "Subtitle Edit",
        .Filename = "SubtitleEdit.exe",
        .Location = "Support\SubtitleEdit",
        .WebURL = "http://www.nikse.dk/SubtitleEdit",
        .HelpURL = "http://www.nikse.dk/SubtitleEdit/Help",
        .Description = "Subtitle editor GUI app."})

    Shared Property mpvnet As Package = Add(New Package With {
        .Name = "mpv.net",
        .Filename = "mpvnet.exe",
        .Location = "Support\mpv.net",
        .WebURL = "https://github.com/stax76/mpv.net",
        .Description = "The worlds best media player (GUI app)."})

    Shared Property MpcBE As Package = Add(New Package With {
        .Name = "MPC-BE",
        .Filename = "mpc-be64.exe",
        .IsIncluded = False,
        .VersionAllowAny = True,
        .Required = False,
        .WebURL = "https://sourceforge.net/projects/mpcbe/",
        .Description = "DirectShow based media player (GUI app).",
        .Locations = {Registry.LocalMachine.GetString("SOFTWARE\MPC-BE", "ExePath").Dir, Folder.Programs + "MPC-BE x64"}})

    Shared Property MpcHC As Package = Add(New Package With {
        .Name = "MPC-HC",
        .Filename = "mpc-hc64.exe",
        .IsIncluded = False,
        .VersionAllowAny = True,
        .Required = False,
        .WebURL = "https://mpc-hc.org/",
        .Description = "DirectShow based media player (GUI app).",
        .Locations = {Registry.CurrentUser.GetString("Software\MPC-HC\MPC-HC", "ExePath").Dir, Folder.Programs + "MPC-HC"}})

    Shared Property modPlus As Package = Add(New PluginPackage With {
        .Name = "modPlus",
        .Filename = "modPlus.dll",
        .WebURL = "http://www.avisynth.nl/users/vcmohan/modPlus/modPlus.html",
        .Description = "Video filter plugin which modify values of color components to attenuate noise, blur or equalize input.",
        .AvsFilterNames = {"GBlur", "MBlur", "Median", "minvar", "Morph", "SaltPepper", "SegAmp", "TweakHist", "Veed"}})

    Shared Property checkmate As Package = Add(New PluginPackage With {
        .Name = "checkmate",
        .Filename = "checkmate.dll",
        .WebURL = "http://github.com/tp7/checkmate",
        .Description = "Spatial and temporal dot crawl reducer. Checkmate is most effective in static or low motion scenes.",
        .AvsFilterNames = {"checkmate"}})

    Shared Property MedianBlur2 As Package = Add(New PluginPackage With {
        .Name = "MedianBlur2",
        .Filename = "MedianBlur2.dll",
        .WebURL = "http://avisynth.nl/index.php/MedianBlur2",
        .Description = "Constant time median video filter plugin.",
        .AvsFilterNames = {"MedianBlur", "MedianBlurTemporal"}})

    Shared Property AutoAdjust As Package = Add(New PluginPackage With {
        .Name = "AutoAdjust",
        .Filename = "AutoAdjust.dll",
        .WebURL = "http://forum.doom9.org/showthread.php?t=167573",
        .Description = "Automatic adjustement video filter plugin that calculates statistics of clip, stabilizes them temporally and uses them to adjust luminance gain & color balance.",
        .AvsFilterNames = {"AutoAdjust"}})

    Shared Property SmoothAdjust As Package = Add(New PluginPackage With {
        .Name = "SmoothAdjust",
        .Filename = "SmoothAdjust.dll",
        .WebURL = "http://forum.doom9.org/showthread.php?t=154971",
        .Description = "SmoothAdjust is a video filter plugin to make YUV adjustements.",
        .AvsFilterNames = {"SmoothTweak", "SmoothCurve", "SmoothCustom", "SmoothTools"}})

    Shared Property EEDI3 As Package = Add(New PluginPackage With {
        .Name = "EEDI3",
        .Filename = "EEDI3.dll",
        .WebURL = "http://avisynth.nl/index.php/EEDI3",
        .Description = "EEDI3 (Enhanced Edge Directed Interpolation) resizes an image by 2x in the vertical direction by copying the existing image to 2*y(n) and interpolating the missing field.",
        .AvsFilterNames = {"EEDI3"},
        .AvsFiltersFunc = Function() {New VideoFilter("Field", "EEDI3", "EEDI3()")}})

    Shared Property EEDI2 As Package = Add(New PluginPackage With {
        .Name = "EEDI2",
        .Filename = "EEDI2.dll",
        .WebURL = "http://avisynth.nl/index.php/EEDI2",
        .Description = "EEDI2 (Enhanced Edge Directed Interpolation) resizes an image by 2x in the vertical direction by copying the existing image to 2*y(n) and interpolating the missing field.",
        .AvsFilterNames = {"EEDI2"}})

    Shared Property VSRip As Package = Add(New Package With {
        .Name = "VSRip",
        .Filename = "VSRip.exe",
        .Location = "Subtitles\VSRip",
        .Description = "GUI app that rips VobSub subtitles.",
        .WebURL = "http://sourceforge.net/projects/guliverkli"})

    Shared Property flash3kyuu_deband As Package = Add(New PluginPackage With {
        .Name = "flash3kyuu_deband",
        .Filename = "flash3kyuu_deband.dll",
        .WebURL = "http://forum.doom9.org/showthread.php?t=161411",
        .HelpURL = "http://f3kdb.readthedocs.io/en/latest/#",
        .Description = "Simple debanding filter that can be quite effective for some anime sources.",
        .VSFilterNames = {"core.f3kdb.Deband"},
        .AvsFilterNames = {"f3kdb"}})

    Shared Property f3kdb_neo As Package = Add(New PluginPackage With {
        .Name = "f3kdb Neo",
        .Filename = "neo-f3kdb.dll",
        .WebURL = "https://github.com/HomeOfAviSynthPlusEvolution/neo_f3kdb",
        .HelpURL = "http://f3kdb.readthedocs.io/en/latest/#",
        .DownloadURL = "https://github.com/HomeOfAviSynthPlusEvolution/neo_f3kdb/releases",
        .Description = "Debanding filter forked from flash3kyuu_deband.",
        .AvsFilterNames = {"neo_f3kdb"},
        .AvsFiltersFunc = Function() {New VideoFilter("Misc", "f3kdb Neo", $"neo_f3kdb(y=64, cb=64, cr=64, grainy=0, grainc=0)")},
        .VSFilterNames = {"core.neo_f3kdb.Deband"},
        .VSFiltersFunc = Function() {New VideoFilter("Misc", "f3kdb Neo", "clip = core.neo_f3kdb.Deband(clip, y=64, cb=64, cr=64, grainy=0, grainc=0)")}})

    Shared Property vinverse As Package = Add(New PluginPackage With {
        .Name = "vinverse",
        .Filename = "vinverse.dll",
        .WebURL = "http://avisynth.nl/index.php/Vinverse",
        .Description = "Simple but effective plugin to remove residual combing.",
        .AvsFilterNames = {"vinverse", "vinverse2"},
        .AvsFiltersFunc = Function() {New VideoFilter("Restoration", "RCR | Vinverse", "$select:Vinverse|vinverse(sstr=2.7, amnt=255, uv=3, scl=0.25);Vinverse2|vinverse2(sstr=2.7, amnt=255, uv=3, scl=0.25)$")}})

    Shared Property scenechange As Package = Add(New PluginPackage With {
        .Name = "scenechange",
        .Filename = "scenechange.dll",
        .VSFilterNames = {"scenechange"}})

    Shared Property avs2pipemod As Package = Add(New Package With {
        .Name = "avs2pipemod",
        .Filename = "avs2pipemod64.exe",
        .Location = "Support\avs2pipemod",
        .WebURL = "http://github.com/chikuzen/avs2pipemod",
        .DownloadURL = "https://github.com/chikuzen/avs2pipemod/releases",
        .HelpSwitch = "stderr",
        .Description = "Given an AviSynth script as input, avs2pipemod can send video, audio, or information of various types to stdout for consumption by command line encoders or other tools."})

    Shared Property x264 As Package = Add(New Package With {
        .Name = "x264",
        .Filename = "x264.exe",
        .Location = "Encoders\x264",
        .Description = "H.264 video encoding console app. Patman mod supports vpy input and shows the estimated size in the status line.",
        .WebURL = "http://www.videolan.org/developers/x264.html",
        .DownloadURL = "https://www.mediafire.com/folder/vkt2ckzjvt0qf/StaxRip_Tools",
        .HelpURL = "http://www.chaneru.com/Roku/HLS/X264_Settings.htm",
        .HelpFilename = "x264 Help.txt",
        .HelpSwitch = "--fullhelp"})

    Shared Property x265 As Package = Add(New Package With {
        .Name = "x265",
        .Location = "Encoders\x265",
        .Filename = "x265.exe",
        .WebURL = "http://x265.org",
        .HelpURL = "http://x265.readthedocs.org",
        .DownloadURL = "https://www.mediafire.com/folder/vkt2ckzjvt0qf/StaxRip_Tools",
        .HelpSwitch = "--log-level full --fullhelp",
        .HelpFilename = "x265 Help.txt",
        .Description = "H.265 video encoding console app. Patman mod shows the estimated size in the status line."})

    Shared Property SVTAV1 As Package = Add(New Package With {
        .Name = "SVT-AV1",
        .Location = "Encoders\SVT-AV1",
        .Filename = "SvtAv1EncApp.exe",
        .WebURL = "https://github.com/OpenVisualCloud/SVT-AV1",
        .HelpURL = "https://github.com/OpenVisualCloud/SVT-AV1/blob/master/Docs/svt-av1_encoder_user_guide.md",
        .DownloadURL = "https://www.mediafire.com/folder/vkt2ckzjvt0qf/StaxRip_Tools",
        .HelpSwitch = "stderr-help",
        .Description = "Intel AV1 encoder."})

    Shared Property Rav1e As Package = Add(New Package With {
        .Name = "rav1e",
        .Filename = "rav1e.exe",
        .Location = "Encoders\Rav1e",
        .Description = "AV1 Video Encoder.",
        .WebURL = "https://github.com/xiph/rav1e",
        .HelpFilename = "rav1e help.txt",
        .HelpSwitch = "--help"})

    Shared Property aomenc As Package = Add(New Package With {
        .Name = "aomenc",
        .Filename = "aomenc.exe",
        .Location = "Encoders\aomenc",
        .Description = "AV1 video encoder console app.",
        .IsIncluded = False,
        .VersionAllowAny = True,
        .WebURL = "https://aomedia.org",
        .DownloadURL = "https://www.mediafire.com/folder/vkt2ckzjvt0qf/StaxRip_Tools",
        .RequiredFunc = Function() TypeOf p.VideoEncoder Is aomenc,
        .HelpSwitch = "--help"})

    Shared Property mkvmerge As Package = Add(New Package With {
        .Name = "mkvmerge",
        .Filename = "mkvmerge.exe",
        .Location = "Support\MKVToolNix",
        .WebURL = "https://mkvtoolnix.download/",
        .HelpURL = "https://mkvtoolnix.download/docs.html",
        .DownloadURL = "https://www.fosshub.com/MKVToolNix.html",
        .HelpSwitch = "",
        .Description = "MKV muxing tool."})

    Shared Property mkvextract As Package = Add(New Package With {
        .Name = "mkvextract",
        .Filename = "mkvextract.exe",
        .Location = "Support\MKVToolNix",
        .WebURL = "https://mkvtoolnix.download/",
        .HelpURL = "https://mkvtoolnix.download/docs.html",
        .DownloadURL = "https://www.fosshub.com/MKVToolNix.html",
        .HelpSwitch = "",
        .Description = "MKV demuxing tool."})

    Shared Property mkvinfo As Package = Add(New Package With {
        .Name = "mkvinfo",
        .Filename = "mkvinfo.exe",
        .Location = "Support\MKVToolNix",
        .WebURL = "https://mkvtoolnix.download/",
        .HelpURL = "https://mkvtoolnix.download/docs.html",
        .DownloadURL = "https://www.fosshub.com/MKVToolNix.html",
        .HelpSwitch = "",
        .Description = "MKV info tool."})

    Shared Property AutoCrop As Package = Add(New Package With {
        .Name = "AutoCrop",
        .Filename = "AutoCrop.exe",
        .Location = "Support\AutoCrop",
        .HelpSwitch = "",
        .Description = "AutoCrop console app."})

    Shared Property PNGopt As Package = Add(New Package With {
        .Name = "PNGopt",
        .Filename = "apngopt.exe",
        .HelpFilename = "help.txt",
        .Location = "Thumbnails\PNGopt",
        .WebURL = "https://sourceforge.net/projects/apng/files/",
        .HelpSwitch = "",
        .Description = "Opt Tools For Creating PNG"})

    Shared Property NVEnc As Package = Add(New Package With {
        .Name = "NVEnc",
        .Filename = "NVEncC64.exe",
        .Location = "Encoders\NVEnc",
        .HelpSwitch = "-h",
        .WebURL = "http://github.com/rigaya/NVEnc",
        .HelpURL = "https://github.com/rigaya/NVEnc/blob/master/NVEncC_Options.en.md",
        .DownloadURL = "https://github.com/rigaya/NVEnc/releases",
        .Description = "NVIDIA hardware video encoder.",
        .HelpFilename = "NVEnc Help.txt"})

    Shared Property QSVEnc As Package = Add(New Package With {
        .Name = "QSVEnc",
        .Filename = "QSVEncC64.exe",
        .Location = "Encoders\QSVEnc",
        .Description = "Intel hardware video encoder.",
        .HelpFilename = "QSVEnc Help.txt",
        .WebURL = "http://github.com/rigaya/QSVEnc",
        .DownloadURL = "https://github.com/rigaya/QSVEnc/releases",
        .HelpURL = "https://github.com/rigaya/QSVEnc/blob/master/QSVEncC_Options.en.md",
        .HelpSwitch = "-h"})

    Shared Property VCEEnc As Package = Add(New Package With {
        .Name = "VCEEnc",
        .Filename = "VCEEncC64.exe",
        .Location = "Encoders\VCEEnc",
        .Description = "AMD hardware video encoder.",
        .HelpFilename = "VCEEnc Help.txt",
        .HelpSwitch = "-h",
        .WebURL = "http://github.com/rigaya/VCEEnc",
        .DownloadURL = "https://github.com/rigaya/VCEEnc/releases"})

    Shared Property FFT3DFilter As Package = Add(New PluginPackage With {
        .Name = "FFT3DFilter",
        .Filename = "fft3dfilter.dll",
        .WebURL = "http://github.com/pinterf/fft3dfilter",
        .Description = "FFT3DFilter uses Fast Fourier Transform method for image processing in frequency domain.",
        .HelpFilename = "fft3dfilter.html",
        .AvsFilterNames = {"FFT3DFilter"},
        .AvsFiltersFunc = Function() {New VideoFilter("Noise", "FFT3DFilter | FFT3DFilter", "FFT3DFilter()")}})

    Shared Property ffms2 As Package = Add(New PluginPackage With {
        .Name = "ffms2",
        .Filename = "ffms2.dll",
        .WebURL = "http://github.com/FFMS/ffms2",
        .HelpURL = "http://github.com/FFMS/ffms2/blob/master/doc/ffms2-avisynth.md",
        .Description = "AviSynth+ and VapourSynth source filter supporting various input formats.",
        .AvsFilterNames = {"FFVideoSource", "FFAudioSource"},
        .AvsFiltersFunc = Function() {New VideoFilter("Source", "FFVideoSource", $"FFVideoSource(""%source_file%"", cachefile=""%source_temp_file%.ffindex"")" + BR + "#AssumeFPS(25)")},
        .VSFilterNames = {"ffms2"},
        .VSFiltersFunc = Function() {New VideoFilter("Source", "ffms2", "clip = core.ffms2.Source(r""%source_file%"", cachefile=r""%source_temp_file%.ffindex"")" + BR + "#clip = core.std.AssumeFPS(clip, None, 25, 1)")}})

    Shared Property AviSynthShader As Package = Add(New PluginPackage With {
        .Name = "AviSynthShader DLL",
        .Location = "Plugins\AVS\AviSynthShader",
        .Filename = "Shader-x64.dll",
        .WebURL = "https://github.com/mysteryx93/AviSynthShader",
        .AvsFilterNames = {"SuperRes", "SuperResXBR", "SuperXBR", "ResizeShader", "SuperResPass", "SuperXbrMulti", "ResizeShader"}})

    Shared Property AviSynthShaderAVSI As Package = Add(New PluginPackage With {
        .Name = "AviSynthShader AVSI",
        .Location = "Plugins\AVS\AviSynthShader",
        .Filename = "Shader.avsi",
        .WebURL = "https://github.com/mysteryx93/AviSynthShader",
        .AvsFilterNames = {"SuperRes", "SuperResXBR", "SuperXBR", "ResizeShader", "SuperResPass", "SuperXbrMulti", "ResizeShader"}})

    Shared Property VSFilterMod As Package = Add(New PluginPackage With {
        .Name = "VSFilterMod",
        .Filename = "VSFilterMod.dll",
        .Description = "AviSynth and VapourSynth subtitle plugin with support for vobsub srt and ass.",
        .WebURL = "https://github.com/sorayuki/VSFilterMod",
        .DownloadURL = "https://github.com/sorayuki/VSFilterMod/releases",
        .AvsFilterNames = {"VobSub", "TextSubMod"},
        .VSFilterNames = {"vsfm.VobSub", "vsfm.TextSubMod"}})

    Shared Property SangNom2 As Package = Add(New PluginPackage With {
        .Name = "SangNom2",
        .Filename = "SangNom2.dll",
        .WebURL = "http://avisynth.nl/index.php/SangNom2",
        .Description = "SangNom2 is a reimplementation of MarcFD's old SangNom filter. Originally it's a single field deinterlacer using edge-directed interpolation but nowadays it's mainly used in anti-aliasing scripts. The output is not completely but mostly identical to the original SangNom.",
        .AvsFilterNames = {"SangNom2"}})

    Shared Property DCTFilter As Package = Add(New PluginPackage With {
        .Name = "DCTFilter",
        .Filename = "DCTFilter.dll",
        .Location = "Plugins\AVS\DCTFilter",
        .Description = "A rewrite of DctFilter for Avisynth+.",
        .WebURL = "https://github.com/Asd-g/DCTFilter",
        .AvsFilterNames = {"DCTFilter", "DCTFilterD", "DCTFilter4", "DCTFilter4D", "DCTFilter8", "DCTFilter8D"}})

    Shared Property DCTFilterF As Package = Add(New PluginPackage With {
        .Name = "DCTFilter-f",
        .Filename = "DCTFilter.dll",
        .Location = "Plugins\VS\DCTFilter-f",
        .Description = "Renewed VapourSynth port of DCTFilter.",
        .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DCTFilter",
        .VSFilterNames = {"dctf.DCTFilter"}})

    Shared Property DCTFilterVS As Package = Add(New PluginPackage With {
        .Name = "DCTFilter",
        .Filename = "DCTFilter.dll",
        .Location = "Plugins\VS\DCTFilter",
        .Description = "Renewed VapourSynth port of DCTFilter.",
        .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DCTFilter",
        .VSFilterNames = {"dctf.DCTFilter"}})

    Shared Property FFTW As Package = Add(New Package With {
        .Name = "FFTW",
        .Location = "Support\FFTW",
        .Filename = "libfftw3-3.dll",
        .Description = "Library required by various AviSynth and VapourSynth plugins.",
        .WebURL = "http://www.fftw.org"})

    Shared Property havsfunc As Package = Add(New PluginPackage With {
        .Name = "havsfunc",
        .WebURL = "http://github.com/HomeOfVapourSynthEvolution/havsfunc",
        .DownloadURL = "https://github.com/HomeOfVapourSynthEvolution/havsfunc/releases",
        .HelpURL = "http://forum.doom9.org/showthread.php?t=166582",
        .Description = "Various popular AviSynth scripts ported To VapourSynth.",
        .Filename = "havsfunc.py",
        .Location = "Plugins\VS\Scripts",
        .VSFilterNames = {"havsfunc.aaf", "havsfunc.AverageFrames", "havsfunc.Bob", "havsfunc.ChangeFPS", "havsfunc.Clamp", "havsfunc.ContraSharpening", "havsfunc.daa", "havsfunc.Deblock_QED", "havsfunc.DeHalo_alpha", "havsfunc.DitherLumaRebuild", "havsfunc.EdgeCleaner", "havsfunc.FastLineDarkenMOD", "havsfunc.FineDehalo", "havsfunc.FixChromaBleedingMod", "havsfunc.GrainFactory3", "havsfunc.GrainStabilizeMC", "havsfunc.HQDeringmod", "havsfunc.InterFrame", "havsfunc.ivtc_txt60mc", "havsfunc.KNLMeansCL", "havsfunc.logoNR", "havsfunc.LSFmod", "havsfunc.LUTDeCrawl", "havsfunc.LUTDeRainbow", "havsfunc.MCTemporalDenoise", "havsfunc.MinBlur", "havsfunc.mt_deflate_multi", "havsfunc.mt_expand_multi", "havsfunc.mt_inflate_multi", "havsfunc.mt_inpand_multi", "havsfunc.Overlay", "havsfunc.Padding", "havsfunc.QTGMC", "havsfunc.Resize", "havsfunc.santiag", "havsfunc.sbr", "havsfunc.SCDetect", "havsfunc.SigmoidDirect", "havsfunc.SigmoidInverse", "havsfunc.smartfademod", "havsfunc.SMDegrain", "havsfunc.SmoothLevels", "havsfunc.srestore", "havsfunc.Stab", "havsfunc.STPresso", "havsfunc.TemporalDegrain", "havsfunc.Toon", "havsfunc.Vinverse", "havsfunc.Vinverse2", "havsfunc.Weave", "havsfunc.YAHR"},
        .VSFiltersFunc = Function() {
            New VideoFilter("Field", "QTGMC | QTGMC", $"clip = core.std.SetFieldBased(clip, 2) # 1=BFF, 2=TFF{BR}clip = havsfunc.QTGMC(clip, TFF=True, Preset='$select:msg:Select a preset.;Draft;Ultra Fast;Super Fast;Very Fast;Faster;Fast;Medium;Slow;Slower;Very Slow;Placebo$', InputType=$select:msg:Select Input Type;Interlaced|0;Progressive|1;Progressive Repair Details|2;Progressive Full Repair|3$, SourceMatch=3, Sharpness=0.2)"),
            New VideoFilter("Field", "QTGMC | QTGMC with Repair", $"clip = core.std.SetFieldBased(clip, 2) # 1=BFF, 2=TFF{BR}QTGMC1 = havsfunc.QTGMC(clip, TFF=True, Preset='Slower', InputType=2){BR}QTGMC2 = havsfunc.QTGMC(clip, TFF=True, Preset='Slower', InputType=3){BR}clip = core.rgvs.Repair(QTGMC1,QTGMC2, mode=1)")}})

    Shared Property QTGMC As Package = Add(New PluginPackage With {
        .Name = "QTGMC",
        .Filename = "QTGMC.avsi",
        .WebURL = "http://avisynth.nl/index.php/QTGMC",
        .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.6%20and%20up/QTGMC.avsi",
        .Description = "A very high quality deinterlacer with a range of features for both quality and convenience. These include a simple presets system, extensive noise processing capabilities, support for repair of progressive material, precision source matching, shutter speed simulation, etc. Originally based on TempGaussMC by Dide.",
        .AvsFilterNames = {"QTGMC"},
        .AvsFiltersFunc = Function() {
            New VideoFilter("Field", "QTGMC | QTGMC...", "QTGMC(preset=""$select:msg:Select a preset.;Draft;Ultra Fast;Super Fast;Very Fast;Faster;Fast;Medium;Slow;Slower;Very Slow;Placebo$"", InputType=$select:msg:Select Input Type;Interlaced|0;Progressive|1;Progressive Repair Details|2;Progressive Full Repair|3$, sourceMatch=3, sharpness=0.2, tr2=2, ediThreads=8)"),
            New VideoFilter("Field", "QTGMC | QTGMC With Repair", "QTGMC1 = QTGMC(preset=""Slower"", inputType=2)" + BR + "QTGMC2 = QTGMC(preset=""Slower"", inputType=3, prevGlobals=""Reuse"")" + BR + "$select:msg:Select Repair Mode To Use;Repair|Repair(QTGMC1, QTGMC2, 1);Repair16|Repair16(QTGMC1, QTGMC2, 1)$")}})

    Shared Property SMDegrain As Package = Add(New PluginPackage With {
        .Name = "SMDegrain",
        .Filename = "SMDegrain.avsi",
        .WebURL = "http://avisynth.nl/index.php/SMDegrain",
        .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.5%20and%20up/SMDegrain.avsi",
        .Description = "SMDegrain, the Simple MDegrain Mod, is mainly a convenience function for using MVTools.",
        .AvsFilterNames = {"SMDegrain"},
        .AvsFiltersFunc = Function() {
            New VideoFilter("Noise", "SMDegrain | SMDGrain", "SMDegrain(tr=2, thSAD=250, contrasharp=false, refinemotion=true, lsb=false)"),
            New VideoFilter("Noise", "SMDegrain | SMDGrain With Motion Vectors", "super_search = Dither_Luma_Rebuild(S0=1.0, c=0.0625).MSuper(rfilter=4)" + BR + "bv2 = super_search.MAnalyse(isb=true,  delta=2, overlap=4)" + BR + "bv1 = super_search.MAnalyse(isb=true,  delta=1, overlap=4)" + BR + "fv1 = super_search.MAnalyse(isb=false, delta=1, overlap=4)" + BR + "fv2 = super_search.MAnalyse(isb=false, delta=2, overlap=4)" + BR + "MDegrain2(MSuper(levels=1), bv1, fv1, bv2, fv2, thSAD=300, thSADC=150)"),
            New VideoFilter("Noise", "SMDegrain | SMDGrain 16Bit", "sharp=last" + BR + "dfttest(tbsize=1, sigma=10, lsb=True)" + BR + "SMDegrain(tr=3, thSAD=300, CClip=sharp, lsb_in=True, lsb_out=True)")}})

    Shared Property Zs_RF_Shared As Package = Add(New PluginPackage With {
        .Name = "Zs_RF_Shared",
        .Filename = "Zs_RF_Shared.avsi",
        .WebURL = "https://github.com/realfinder/AVS-Stuff",
        .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.5%20and%20up/Zs_RF_Shared.avsi",
        .Description = "Shared Functions and utility.",
        .RequiredFunc = Function() MCTemporalDenoise.Required,
        .AvsFilterNames = {"Dither_Luma_Rebuild", "AvsPlusVersionNumber"}})

    Shared Property LSFmod As Package = Add(New PluginPackage With {
        .Name = "LSFmod",
        .Filename = "LSFmod.avsi",
        .WebURL = "http://avisynth.nl/index.php/LSFmod",
        .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/master/avs%202.5%20and%20up/LSFmod.avsi",
        .Description = "A LimitedSharpenFaster mod with a lot of new features and optimizations.",
        .AvsFilterNames = {"LSFmod"},
        .AvsFiltersFunc = Function() {New VideoFilter("Line", "Sharpen | LSFmod", "LSFmod(defaults=""slow"", strength=100, Smode=5, Smethod=3, kernel=11, preblur=""OFF"", secure=true, Szrp=16, Spwr=4, SdmpLo=4, SdmpHi=48, Lmode=4, overshoot=1, undershoot=1, Overshoot2=1, Undershoot2=1, soft=-2, soothe=true, keep=20, edgemode=0, edgemaskHQ=true, ss_x=1.50, ss_y=1.50, dest_x=%target_width%, dest_y=%target_height%, show=false, screenW=1280, screenH=1024)")}})

    Shared Property TemporalDegrain2 As Package = Add(New PluginPackage With {
        .Name = "TemporalDegrain2",
        .Filename = "TemporalDegrain2.avsi",
        .WebURL = "http://avisynth.nl/index.php/TemporalDegrain2",
        .Description = "Builds on Temporal Degrain but it is able to clean the noise even further while impoving the sharpness in cases where orignal version had severe drops in visual quality.",
        .AvsFilterNames = {"TemporalDegrain2"},
        .AvsFiltersFunc = Function() {
            New VideoFilter("Noise", "TemporalDegrain2", "TemporalDegrain2(degrainTR=2, postFFT=3, postSigma=3)")}})

    Shared Property LSmashWorks As Package = Add(New PluginPackage With {
        .Name = "L-SMASH-Works",
        .Filename = "LSMASHSource.dll",
        .Description = "AviSynth and VapourSynth source filter based on Libav supporting a wide range of input formats.",
        .WebURL = "https://github.com/HolyWu/L-SMASH-Works",
        .DownloadURL = "https://github.com/HolyWu/L-SMASH-Works/releases",
        .HelpUrlAviSynth = "https://github.com/HolyWu/L-SMASH-Works/blob/master/AviSynth/README",
        .HelpUrlVapourSynth = "https://github.com/HolyWu/L-SMASH-Works/blob/master/VapourSynth/README",
        .AvsFilterNames = {"LSMASHVideoSource", "LSMASHAudioSource", "LWLibavVideoSource", "LWLibavAudioSource"},
        .AvsFiltersFunc = Function() {
            New VideoFilter("Source", "LSMASHVideoSource", "LSMASHVideoSource(""%source_file%"")" + BR + "#AssumeFPS(25)"),
            New VideoFilter("Source", "LWLibavVideoSource", "LWLibavVideoSource(""%source_file%"", cachefile=""%source_temp_file%.lwi"")" + BR + "#AssumeFPS(25)")},
        .VSFilterNames = {"lsmas.LibavSMASHSource", "lsmas.LWLibavSource"},
        .VSFiltersFunc = Function() {
            New VideoFilter("Source", "LibavSMASHSource", "clip = core.lsmas.LibavSMASHSource(r""%source_file%"")" + BR + "#clip = core.std.AssumeFPS(clip, None, 25, 1)"),
            New VideoFilter("Source", "LWLibavSource", "clip = core.lsmas.LWLibavSource(r""%source_file%"", cachefile=r""%source_temp_file%.lwi"")" + BR + "#clip = core.std.AssumeFPS(clip, None, 25, 1)")}})

    Shared Property BM3D As Package = Add(New PluginPackage With {
        .Name = "BM3D",
        .Filename = "BM3D.dll",
        .VSFilterNames = {"bm3d.RGB2OPP", "bm3d.OPP2RGB", "bm3d.Basic", "bm3d.Final", "bm3d.VBasic", "bm3d.VFinal", "bm3d.VAggregate"},
        .Description = "BM3D denoising filter for VapourSynth",
        .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-BM3D"})

    Shared Property MCTemporalDenoise As Package = Add(New PluginPackage With {
        .Name = "MCTemporalDenoise",
        .Filename = "MCTemporalDenoise.avsi",
        .WebURL = "http://avisynth.nl/index.php/MCTemporalDenoise",
        .Description = "A motion compensated noise removal script with an accompanying post-processing component.",
        .AvsFilterNames = {"MCTemporalDenoise", "MCTemporalDenoisePP"},
        .AvsFiltersFunc = Function() {
            New VideoFilter("Noise", "MCTemporalDenoise | MCTemporalDenoise", "MCTemporalDenoise(settings=""medium"")"),
            New VideoFilter("Noise", "MCTemporalDenoise | MCTemporalDenoisePP", "source=last" + BR + "denoised=FFT3Dfilter()" + BR + "MCTemporalDenoisePP(denoised)")}})

    Shared Property DFTTestAVS As Package = Add(New PluginPackage With {
        .Name = "DFTTest",
        .Filename = "dfttest.dll",
        .Description = "2D/3D frequency domain denoiser using Discrete Fourier transform.",
        .HelpFilename = "dfttest - README.txt",
        .WebURL = "https://github.com/pinterf/dfttest",
        .DownloadURL = "https://github.com/pinterf/dfttest/releases",
        .AvsFilterNames = {"dfttest"},
        .AvsFiltersFunc = Function() {New VideoFilter("Noise", "DFTTest", "dfttest($select:msg:Select Strength;Light|sigma=6, tbsize=3;Moderate|sigma=16, tbsize=5;Strong Static|sigma=64, tbsize=1;Strong Temporal|sigma=64, tbsize=3$,$select:msg:Reduce Banding?;No Deband| dither=0;Deband| dither=1;Add Noise| dither=2;More Noise| dither=3$)")}})

    Shared Property DFTTestVS As Package = Add(New PluginPackage With {
        .Name = "DFTTest",
        .Filename = "DFTTest.dll",
        .Description = "VapourSynth port of dfttest.",
        .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DFTTest",
        .DownloadURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DFTTest/releases",
        .VSFilterNames = {"dfttest.DFTTest"},
        .VSFiltersFunc = Function() {New VideoFilter("Noise", "DFTTest", "$select:msg:Select Strength;Light|clip = core.dfttest.DFTTest(clip, sigma=6, tbsize=3,opt=3);Moderate|clip = core.dfttest.DFTTest(clip, sigma=16, tbsize=5,opt=3);Strong|clip = core.dfttest.DFTTest(clip, sigma=64, tbsize=1,opt=3)$")}})

    Shared Property DFTTestNeoVS As Package = Add(New PluginPackage With {
        .Name = "DFTTest Neo",
        .Filename = "neo-dfttest.dll",
        .Description = "2D/3D frequency domain denoiser using Discrete Fourier transform.",
        .WebURL = "https://github.com/HomeOfAviSynthPlusEvolution/neo_DFTTest",
        .DownloadURL = "https://github.com/HomeOfAviSynthPlusEvolution/neo_DFTTest/releases",
        .VSFilterNames = {"neo_dfttest.DFTTest"},
        .VSFiltersFunc = Function() {New VideoFilter("Noise", "DFTTest (Neo)", "clip = core.neo_dfttest.DFTTest(clip, ftype=0, sigma=2.0, planes=[0,1,2])")}})

    Shared Property muvsfunc As Package = Add(New PluginPackage With {
        .Name = "muvsfunc",
        .Filename = "muvsfunc.py",
        .Location = "Plugins\VS\Scripts",
        .Description = "Muonium's VapourSynth functions.",
        .WebURL = "https://github.com/WolframRhodium/muvsfunc",
        .VSFilterNames = {
            "muvsfunc.LDMerge", "muvsfunc.Compare", "muvsfunc.ExInpand", "muvsfunc.InDeflate", "muvsfunc.MultiRemoveGrain", "muvsfunc.GradFun3", "muvsfunc.AnimeMask",
            "muvsfunc.PolygonExInpand", "muvsfunc.Luma", "muvsfunc.ediaa", "muvsfunc.nnedi3aa", "muvsfunc.maa", "muvsfunc.SharpAAMcmod", "muvsfunc.TEdge",
            "muvsfunc.Sort", "muvsfunc.Soothe_mod", "muvsfunc.TemporalSoften", "muvsfunc.FixTelecinedFades", "muvsfunc.TCannyHelper", "muvsfunc.MergeChroma", "muvsfunc.firniture",
            "muvsfunc.BoxFilter", "muvsfunc.SmoothGrad", "muvsfunc.DeFilter", "muvsfunc.scale", "muvsfunc.ColorBarsHD", "muvsfunc.SeeSaw", "muvsfunc.abcxyz",
            "muvsfunc.Sharpen", "muvsfunc.Blur", "muvsfunc.BlindDeHalo3", "muvsfunc.dfttestMC", "muvsfunc.TurnLeft", "muvsfunc.TurnRight", "muvsfunc.BalanceBorders",
            "muvsfunc.DisplayHistogram", "muvsfunc.GuidedFilter", "muvsfunc.GMSD", "muvsfunc.SSIM", "muvsfunc.SSIM_downsample", "muvsfunc.LocalStatisticsMatching", "muvsfunc.LocalStatistics",
            "muvsfunc.TextSub16", "muvsfunc.TMinBlur", "muvsfunc.mdering", "muvsfunc.BMAFilter", "muvsfunc.LLSURE", "muvsfunc.YAHRmod", "muvsfunc.RandomInterleave"}})

    Shared Property FFT3DGPU As Package = Add(New PluginPackage With {
        .Name = "FFT3DGPU",
        .Filename = "FFT3dGPU.dll",
        .Description = "Similar algorithm to FFT3DFilter, but uses graphics hardware for increased speed.",
        .HelpFilename = "Readme.txt",
        .AvsFilterNames = {"FFT3DGPU"},
        .AvsFiltersFunc = Function() {New VideoFilter("Noise", "FFT3DFilter | FFT3DGPU", "FFT3DGPU(sigma=1.5, bt=5, bw=32, bh=32, ow=16, oh=16, sharpen=0.4, NVPerf=$select:msg:Enable Nvidia Function;True;False$)")}})

    Shared Property MPEG2DecPlus As Package = Add(New PluginPackage With {
        .Name = "MPEG2DecPlus",
        .Filename = "MPEG2DecPlus64.dll",
        .WebURL = "https://github.com/Asd-g/MPEG2DecPlus",
        .DownloadURL = "https://github.com/Asd-g/MPEG2DecPlus/releases",
        .Description = "Source filter to open D2V index files created with DGIndex or D2V Witch.",
        .AvsFilterNames = {"MPEG2Source"},
        .AvsFiltersFunc = Function() {New VideoFilter("Source", "MPEG2Source", "MPEG2Source(""%source_file%"")")}})

    Shared Property d2vsource As Package = Add(New PluginPackage With {
        .Name = "d2vsource",
        .Filename = "d2vsource.dll",
        .Description = "Source filter to open D2V index files created with DGIndex or D2V Witch.",
        .WebURL = "http://github.com/dwbuiten/d2vsource",
        .VSFilterNames = {"d2v.Source"},
        .VSFiltersFunc = Function() {New VideoFilter("Source", "d2vsource", "clip = core.d2v.Source(r""%source_file%"")")}})

    Shared Sub New()
        Add(New PluginPackage With {
            .Name = "KNLMeansCL",
            .Filename = "KNLMeansCL.dll",
            .WebURL = "https://github.com/pinterf/KNLMeansCL",
            .DownloadURL = "https://github.com/pinterf/KNLMeansCL/releases",
            .HelpURL = "https://github.com/Khanattila/KNLMeansCL/wiki",
            .Description = "KNLMeansCL is an optimized pixelwise OpenCL implementation of the Non-local means denoising algorithm. Every pixel is restored by the weighted average of all pixels in its search window. The level of averaging is determined by the filtering parameter h.",
            .VSFilterNames = {"knlm.KNLMeansCL"},
            .AvsFilterNames = {"KNLMeansCL"},
            .AvsFiltersFunc = Function() {New VideoFilter("Noise", "NLMeans | KNLMeansCL", "KNLMeansCL(D=1, A=1, h=$select:msg:Select Strength;Light|2;Medium|4;Strong|4$, device_type=""auto"")")},
            .VSFiltersFunc = Function() {New VideoFilter("Noise", "KNLMeansCL", "clip = core.knlm.KNLMeansCL(clip, d=1, a=1, h=$select:msg:Select Strength;Light|2;Medium|4;Strong|4$, device_type='auto')")}})

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
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/DeBlock",
            .Location = "Plugins\AVS\Deblock",
            .AvsFilterNames = {"Deblock"},
            .AvsFiltersFunc = Function() {New VideoFilter("Restoration", "DeBlock | DeBock", "Deblock(quant=25, aOffset=0, bOffset=0)")}})

        Add(New PluginPackage With {
            .Name = "VapourSource",
            .Filename = "VapourSource_x64.dll",
            .Description = "VapourSource is a VapourSynth script reader for AviSynth 2.6.",
            .WebURL = "http://avisynth.nl/index.php/VapourSource",
            .AvsFilterNames = {"VSImport", "VSEval"}})

        Add(New PluginPackage With {
            .Name = "TNLMeans",
            .Filename = "TNLMeans.dll",
            .WebURL = "http://avisynth.nl/index.php/TNLMeans",
            .Description = "TNLMeans is an implementation of the NL-means denoising algorithm. Aside from the original method, TNLMeans also supports extension into 3D, a faster, block based approach, and a multiscale version.",
            .HelpFilename = "readme.txt",
            .AvsFilterNames = {"TNLMeans"},
            .AvsFiltersFunc = Function() {New VideoFilter("Noise", "NLMeans | TNLMeans", "TNLMeans(Ax=4, Ay=4, Az=0, Sx=2, Sy=2, Bx=1, By=1, ms=false, rm=4, a=1.0, h=1.8, sse=true)")}})

        Add(New PluginPackage With {
            .Name = "VagueDenoiser",
            .Filename = "VagueDenoiser.dll",
            .Description = "This is a Wavelet based Denoiser. Basically, it transforms each frame from the video input into the wavelet domain, using various wavelet filters. Then it applies some filtering to the obtained coefficients.",
            .HelpFilename = "vaguedenoiser.html",
            .WebURL = "http://avisynth.nl/index.php/VagueDenoiser",
            .AvsFilterNames = {"VagueDenoiser"},
            .AvsFiltersFunc = Function() {New VideoFilter("Noise", "VagueDenoiser", "VagueDenoiser(threshold=0.8, method=1, nsteps=6, chromaT=0.8)")}})

        Add(New PluginPackage With {
            .Name = "AnimeIVTC",
            .Filename = "AnimeIVTC.avsi",
            .WebURL = "http://avisynth.nl/index.php/AnimeIVTC",
            .AvsFilterNames = {"AnimeIVTC"}})

        Add(New PluginPackage With {
            .Name = "mvtools2",
            .Filename = "mvtools2.dll",
            .WebURL = "http://github.com/pinterf/mvtools",
            .DownloadURL = "https://github.com/pinterf/mvtools/releases",
            .HelpFilename = "mvtools2.html",
            .Description = "MVTools is collection of functions for estimation and compensation of objects motion in video clips. Motion compensation may be used for strong temporal denoising, advanced framerate conversions, image restoration and other tasks.",
            .AvsFilterNames = {"MSuper", "MAnalyse", "MCompensate", "MMask", "MDeGrain1", "MDeGrain2", "MDegrain3"}})

        Add(New PluginPackage With {
            .Name = "DePan",
            .Filename = "DePan.dll",
            .Location = "Plugins\AVS\MVTools2",
            .HelpFilename = "DePan.html",
            .WebURL = "https://github.com/pinterf/mvtools",
            .DownloadURL = "https://github.com/pinterf/mvtools/releases",
            .AvsFilterNames = {"DePan", "DePanInterleave", "DePanStabilize", "DePanScenes"}})

        Add(New PluginPackage With {
            .Name = "DePanEstimate",
            .Location = "Plugins\AVS\MVTools2",
            .Filename = "DePanEstimate.dll",
            .HelpFilename = "DePan.html",
            .WebURL = "https://github.com/pinterf/mvtools",
            .DownloadURL = "https://github.com/pinterf/mvtools/releases",
            .AvsFilterNames = {"DePanEstimate"}})

        Add(New PluginPackage With {
            .Name = "JincResize",
            .Filename = "JincResize.dll",
            .Description = "Jinc (EWA Lanczos) resampling plugin for AviSynth 2.6/AviSynth+.",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/JincResize",
            .AvsFilterNames = {"Jinc36Resize", "Jinc64Resize", "Jinc144Resize", "Jinc256Resize"}})

        Add(New PluginPackage With {
            .Name = "HQDN3D",
            .Filename = "Hqdn3d.dll",
             .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/Hqdn3d",
            .AvsFilterNames = {"HQDN3D"},
            .AvsFiltersFunc = Function() {New VideoFilter("Noise", "HQDN3D", "HQDN3D(ls=4.0, cs=3.0, lt=6.0, ct=4.5, restart=7)")}})

        Add(New PluginPackage With {
            .Name = "HQDeringmod",
            .Filename = "HQDeringmod.avsi",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/HQDering_mod",
            .Description = "Applies deringing by using a smart smoother near edges (where ringing occurs) only.",
            .AvsFilterNames = {"HQDeringmod"},
            .AvsFiltersFunc = Function() {New VideoFilter("Restoration", "DeHalo | HQDeringmod", "HQDeringmod()")}})

        Add(New PluginPackage With {
            .Name = "InterFrame",
            .Filename = "InterFrame.avsi",
            .HelpFilename = "InterFrame.html",
            .Description = "A frame interpolation script that makes accurate estimations about the content of frames",
            .Location = "Plugins\AVS\InterFrame2",
            .WebURL = "http://avisynth.nl/index.php/InterFrame",
            .AvsFilterNames = {"InterFrame"},
            .AvsFiltersFunc = Function() {New VideoFilter("FrameRate", "InterFrame", "InterFrame(preset=""Medium"", tuning=""$select:msg:Select the Tuning Preset;Animation;Film;Smooth;Weak$"", newNum=$enter_text:Enter the NewNum Value.$, newDen=$enter_text:Enter the NewDen Value$, cores=$enter_text:Enter the Number of Cores You want to use$, overrideAlgo=$select:msg:Which Algorithm Do you want to Use?;Strong Predictions|2;Intelligent|13;Smoothest|23$, GPU=$select:msg:Enable GPU Feature?;True;False$)")}})

        Add(New PluginPackage With {
            .Name = "SVPFlow 1",
            .Location = "Plugins\AVS\SVPFlow",
            .HelpFilename = "Readme.txt",
            .Description = "Motion vectors search plugin  is a deeply refactored and modified version of MVTools2 Avisynth plugin",
            .Filename = "svpflow1.dll",
            .WebURL = "http://avisynth.nl/index.php/SVPFlow",
            .AvsFilterNames = {"analyse_params", "super_params", "SVSuper", "SVAnalyse"}})

        Add(New PluginPackage With {
            .Name = "SVPFlow 2",
            .Location = "Plugins\AVS\SVPFlow",
            .HelpFilename = "Readme.txt",
            .Description = "Motion vectors search plugin is a deeply refactored and modified version of MVTools2 Avisynth plugin",
            .Filename = "svpflow2.dll",
            .WebURL = "http://avisynth.nl/index.php/SVPFlow",
            .AvsFilterNames = {"smoothfps_params", "SVConvert", "SVSmoothFps"}})

        Add(New PluginPackage With {
            .Name = "MipSmooth",
            .Filename = "MipSmooth.dll",
            .Description = "a reinvention of SmoothHiQ and Convolution3D. MipSmooth was made to enable smoothing of larger pixel areas than 3x3(x3), to remove blocks and smoothing out low-frequency noise.",
            .HelpFilename = "MipSmooth.html",
            .WebURL = "http://avisynth.org.ru/docs/english/externalfilters/mipsmooth.htm",
            .AvsFilterNames = {"MipSmooth"},
            .AvsFiltersFunc = Function() {New VideoFilter("Restoration", "DeBlock | MipSmooth", "MipSmooth(downsizer=""lanczos"", upsizer=""bilinear"", scalefactor=1.5, method=""strong"")")}})

        Add(New PluginPackage With {
            .Name = "TMM2",
            .Filename = "TMM2.dll",
            .Description = "TMM builds a motion-mask for TDeint, which TDeint uses via its 'emask' parameter.",
            .WebURL = "https://github.com/Asd-g/TMM2",
            .DownloadURL = "https://github.com/Asd-g/TMM2/releases",
            .AvsFilterNames = {"TMM2"}})

        Add(New PluginPackage With {
            .Name = "FineDehalo",
            .Filename = "FineDehalo.avsi",
            .Description = "Halo removal script that uses DeHalo_alpha with a few masks and optional contra-sharpening to try remove halos without removing important details (like line edges). It also includes FineDehalo2, this function tries to remove 2nd order halos. See script for extensive information. ",
            .WebURL = "http://avisynth.nl/index.php/FineDehalo",
            .HelpFilename = "Readme.txt",
            .AvsFilterNames = {"FineDehalo"},
            .AvsFiltersFunc = Function() {New VideoFilter("Restoration", "DeHalo | FineDehalo", "FineDehalo(rx=2.0, ry=2.0, thmi=80, thma=128, thlimi=50, thlima=100, darkstr=1.0, brightstr=1.0, showmask=0, contra=0.0, excl=true)")}})

        Add(New PluginPackage With {
            .Name = "CNR2",
            .Filename = "CNR2.dll",
            .HelpFilename = "CNR2.html",
            .Description = "A fast chroma denoiser. Very effective against stationary rainbows and huge analogic chroma activity. Useful to filter VHS/TV caps.",
            .WebURL = "http://avisynth.nl/index.php/Cnr2",
            .AvsFilterNames = {"cnr2"},
            .AvsFiltersFunc = Function() {New VideoFilter("Restoration", "RCR | CNR2", "Cnr2(mode=""oxx"", scdthr=10.0, ln=35, lm=192, un=47, um=255, vn=47, vm=255, log=false, sceneChroma=false)")}})

        Add(New PluginPackage With {
            .Name = "Lazy Utilities",
            .Filename = "LUtils.avsi",
            .Description = "A collection of helper and wrapper functions meant to help script authors in handling common operations ",
            .WebURL = "https://github.com/AviSynth/avs-scripts",
            .AvsFilterNames = {"LuStackedNto16", "LuPlanarToStacked", "LuRGB48YV12ToRGB48Y", "LuIsFunction", "LuSeparateColumns", "LuMergePlanes", "LuIsHD", "LuConvCSP", "Lu8To16", "Lu16To8", "LuIsEq", "LuSubstrAtIdx", "LuSubstrCnt", "LuReplaceStr", "LUIsDefined", "LuMerge", "LuLut", "LuLimitDif", "LuBlankClip", "LuIsSameRes"}})

        Add(New PluginPackage With {
            .Name = "MultiSharpen",
            .Filename = "MultiSharpen.avsi",
            .Description = "A small but useful Sharpening Function",
            .AvsFilterNames = {"MultiSharpen"}})

        Add(New PluginPackage With {
            .Name = "Average",
            .Filename = "Average.dll",
            .Description = "A simple video filter plugin that calculates a weighted frame-by-frame average from multiple clips.",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/Average",
            .AvsFilterNames = {"Average"}})

        Add(New PluginPackage With {
            .Name = "AvsResize",
            .Filename = "avsresize.dll",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://forum.doom9.org/showthread.php?t=173986",
            .AvsFilterNames = {"z_ConvertFormat", "z_PointResize", "z_BilinearResize", "z_BicubicResize", "z_LanczosResize", "z_Lanczos4Resize", "z_BlackmanResize", "z_Spline16Resize", "z_Spline36Resize", "z_Spline64Resize", "z_GaussResize", "z_SincResize"}})

        Add(New PluginPackage With {
            .Name = "ResizeX",
            .Filename = "ResizeX.avsi",
            .WebURL = "http://avisynth.nl",
            .AvsFilterNames = {"ResizeX"}})

        Add(New PluginPackage With {
            .Name = "Deblock_QED",
            .Filename = "Deblock_QED.avsi",
            .Description = "Designed to provide 8x8 deblocking sensitive to the amount of blocking in the source, compared to other deblockers which apply a uniform deblocking across every frame. ",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/Deblock_QED",
            .AvsFilterNames = {"Deblock_QED"},
            .AvsFiltersFunc = Function() {New VideoFilter("Restoration", "DeBlock | DeBlock_QED", "Deblock_QED(quant1=24, quant2=26, aOff1=1, aOff2=1, bOff1=2, bOff2=2, uv=3)")}})

        Add(New PluginPackage With {
            .Name = "nnedi3 AVSI",
            .Filename = "nnedi3_16.avsi",
            .HelpFilename = "Readme.txt",
            .Description = "nnedi3 is an AviSynth 2.5 plugin, but supports all new planar colorspaces when used with AviSynth 2.6",
            .Location = "Plugins\AVS\NNEDI3",
            .WebURL = "http://avisynth.nl/index.php/nnedi3",
            .AvsFilterNames = {"nnedi3_resize16"}})

        Add(New PluginPackage With {
            .Name = "nnedi3x AVSI",
            .Filename = "nnedi3x.avsi",
            .HelpFilename = "Readme.txt",
            .Description = "nnedi3x is an AviSynth 2.5 plugin, but supports all new planar colorspaces when used with AviSynth 2.6",
            .Location = "Plugins\AVS\NNEDI3",
            .WebURL = "http://avisynth.nl/index.php/nnedi3",
            .AvsFilterNames = {"nnedi3x"}})

        Add(New PluginPackage With {
            .Name = "edi_rpow2 AVSI",
            .Filename = "edi_rpow2.avsi",
            .HelpFilename = "Readme.txt",
            .Description = "An improved rpow2 function for nnedi3, nnedi3ocl, eedi3, and eedi2.",
            .Location = "Plugins\AVS\NNEDI3",
            .WebURL = "http://avisynth.nl/index.php/nnedi3",
            .AvsFilterNames = {"nnedi3_rpow2"}})

        Add(New PluginPackage With {
            .Name = "SmoothD2",
            .Location = "Plugins\AVS\SmoothD2",
            .Filename = "SmoothD2.dll",
            .HelpFilename = "Readme.txt",
            .Description = "Deblocking filter. Rewrite of SmoothD. Faster, better detail preservation, optional chroma deblocking.",
            .WebURL = "http://avisynth.nl/index.php/SmoothD2",
            .AvsFilterNames = {"SmoothD2"},
            .AvsFiltersFunc = Function() {New VideoFilter("Restoration", "DeBlock | SmoothD2", "SmoothD2(quant=3, num_shift=3, Matrix=3, Qtype=1, ZW=1, ZWce=1, ZWlmDark=0, ZWlmBright=255, ncpu=4)")}})

        Add(New PluginPackage With {
            .Name = "SmoothD2c",
            .Location = "Plugins/AVS/SmoothD2",
            .HelpFilename = "Readme.txt",
            .Description = "Deblocking filter. Rewrite of SmoothD. Faster, better detail preservation, optional chroma deblocking.",
            .Filename = "SmoothD2c.avs",
            .WebURL = "http://avisynth.nl/index.php/SmoothD2",
            .AvsFilterNames = {"SmoothD2c"}})

        Add(New PluginPackage With {
            .Name = "xNLMeans",
            .Filename = "xNLMeans.dll",
            .WebURL = "http://avisynth.nl/index.php/xNLMeans",
            .Description = "XNLMeans is an AviSynth plugin implementation of the Non Local Means denoising algorithm",
            .HelpFilename = "Readme.txt",
            .AvsFilterNames = {"xNLMeans"},
            .AvsFiltersFunc = Function() {New VideoFilter("Noise", "NLMeans | xNLMeans", "xnlmeans(a=4,h=2.2,vcomp=0.5,s=1)")}})

        Add(New PluginPackage With {
            .Name = "DehaloAlpha",
            .Filename = "Dehalo_alpha.avsi",
            .Description = "Reduce halo artifacts that can occur when sharpening.",
            .HelpFilename = "Readme.txt",
            .AvsFilterNames = {"DeHalo_alpha_mt", "DeHalo_alpha_2BD"},
            .AvsFiltersFunc = Function() {New VideoFilter("Restoration", "DeHalo | DehaloAlpha", "DeHalo_alpha_mt(rx=2.0, ry=2.0, darkstr=1.0, brightstr=1.0, lowsens=50, highsens=50, ss=1.5)")}})

        Add(New PluginPackage With {
            .Name = "MAA2Mod",
            .Location = "Plugins\AVS\MAA2",
            .Filename = "maa2mod.avsi",
            .Description = "Updated version of the MAA2+ antialising script from AnimeIVTC. MAA2 uses tp7's SangNom2, which provide a nice speedup for SangNom-based antialiasing. Mod version also includes support for EEDI3 along with a few other new functions.",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/MAA2",
            .AvsFilterNames = {"MAA2"},
            .AvsFiltersFunc = Function() {New VideoFilter("Line", "Anti-Aliasing | MMA2", "MAA2(mask=1, chroma=false, ss=2.0, aa=48, aac=40, threads=4, show=0)")}})

        Add(New PluginPackage With {
            .Name = "DAA3Mod",
            .Filename = "daa3mod.avsi",
            .Description = "Motion-Compensated Anti-aliasing with contra-sharpening, can deal with ifade too, created because when applied daa3 to fixed scenes, it could damage some details and other issues.",
            .WebURL = "http://avisynth.nl/index.php/daa3",
            .AvsFilterNames = {"daa3mod", "mcdaa3"},
            .AvsFiltersFunc = Function() {New VideoFilter("Line", "Anti-Aliasing | DAA", "daa3mod()")}})

        Add(New PluginPackage With {
            .Name = "eedi3_resize",
            .Filename = "eedi3_resize.avsi",
            .Location = "Plugins\AVS\EEDI3",
            .Description = "eedi3 based resizing script that allows to resize to arbitrary resolutions while maintaining the correct image center and chroma location.",
            .HelpFilename = "EEDI3 - Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/eedi3",
            .AvsFilterNames = {"eedi3_resize"}})

        Add(New PluginPackage With {
            .Name = "DeNoise Histogram",
            .Location = "Plugins\AVS\DeNoiseMD",
            .Filename = "DiffCol.avsi",
            .Description = "Histogram for both DenoiseMD and DenoiseMF",
            .WebURL = "http://avisynth.nl",
            .AvsFilterNames = {"DiffCol"}})

        Add(New PluginPackage With {
            .Name = "FrameRateConverter DLL",
            .Filename = "FrameRateConverter-x64.dll",
            .Location = "Plugins\AVS\FrameRateConverter",
            .Description = "Increases the frame rate with interpolation and fine artifact removal ",
            .WebURL = "https://github.com/mysteryx93/FrameRateConverter",
            .AvsFilterNames = {"FrameRateConverter"}})

        Add(New PluginPackage With {
            .Name = "FrameRateConverter AVSI",
            .Filename = "FrameRateConverter.avsi",
            .Location = "Plugins\AVS\FrameRateConverter",
            .Description = "Increases the frame rate with interpolation and fine artifact removal ",
            .WebURL = "https://github.com/mysteryx93/FrameRateConverter",
            .AvsFilterNames = {"FrameRateConverter"}})

        Add(New PluginPackage With {
            .Name = "DeNoiseMD",
            .Filename = "DeNoiseMD.avsi",
            .Description = "A fast and accurate denoiser for a Full HD video from a H.264 camera. ",
            .WebURL = "http://avisynth.nl",
            .AvsFilterNames = {"DeNoiseMD1", "DenoiseMD2"},
            .AvsFiltersFunc = Function() {New VideoFilter("Noise", "DeNoise | Denoise MD", "DeNoiseMD1(sigma=4, overlap=2, thcomp=80, str=0.8)" + "DitherPost(mode=7, ampo=1, ampn=0)")}})

        Add(New PluginPackage With {
            .Name = "DeNoiseMF",
            .Filename = "DeNoiseMF.avsi",
            .Description = "A fast and accurate denoiser for a Full HD video from a H.264 camera. ",
            .WebURL = "http://avisynth.nl",
            .AvsFilterNames = {"DeNoiseMF1", "DenoiseMF2"},
            .AvsFiltersFunc = Function() {New VideoFilter("Noise", "DeNoise | Denoise MF", "DenoiseMF2(s1=2.0, s2=2.5, s3=3.0, s4=2.0, overlap=4, thcomp=80, str=0.8, gpu=$select:msg:Use GPU Enabled Feature?;True;False$)")}})

        Add(New PluginPackage With {
            .Name = "MT Expand Multi",
            .Location = "Plugins\AVS\Dither",
            .Description = "Calls mt_expand or mt_inpand multiple times in order to grow or shrink the mask from the desired width and height.",
            .Filename = "mt_xxpand_multi.avsi",
            .WebURL = "http://avisynth.nl/index.php/Dither",
            .AvsFilterNames = {"mt_expand_multi", "mt_inpand_multi"}})

        Add(New PluginPackage With {
            .Name = "TEMmod",
            .Description = "TEMmod creates an edge mask using gradient vector magnitude. ",
            .Filename = "TEMmod.dll",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/TEMmod",
            .AvsFilterNames = {"TEMmod"}})

        Add(New PluginPackage With {
            .Name = "AVSTP",
            .Location = "Plugins\AVS\AVSTP",
            .Description = "AVSTP is a programming library for Avisynth plug-in developers. It helps supporting native multi-threading in plug-ins. It works by sharing a thread pool between multiple plug-ins, so the number of threads stays low whatever the number of instantiated plug-ins. This helps saving resources, especially when working in an Avisynth MT environment. This documentation is mostly targeted to plug-ins developpers, but contains installation instructions for Avisynth users too.",
            .HelpFilename = "avstp.html",
            .Filename = "avstp.dll",
            .WebURL = "http://avisynth.nl/index.php/AVSTP",
            .AvsFilterNames = {"avstp_set_threads"}})

        Add(New PluginPackage With {
            .Name = "Dither AVSI",
            .Description = "This package offers a set of tools to manipulate high-bitdepth (16 bits per plane) video clips. The most proeminent features are color banding artifact removal, dithering to 8 bits, colorspace conversions and resizing.",
            .HelpFilename = "dither.html",
            .Location = "Plugins\AVS\Dither",
            .Filename = "dither.avsi",
            .WebURL = "http://avisynth.nl/index.php/Dither",
            .AvsFilterNames = {"Dither_y_gamma_to_linear", "Dither_y_linear_to_gamma", "Dither_convert_8_to_16", "Dither1Pre", "Dither1Pre", "Dither_repair16", "Dither_convert_yuv_to_rgb", "Dither_convert_rgb_to_yuv", "Dither_resize16", "DitherPost", "Dither_crop16", "DitherBuildMask", "SmoothGrad", "GradFun3", "Dither_box_filter16", "Dither_bilateral16", "Dither_limit_dif16", "Dither_resize16nr", "Dither_srgb_display", "Dither_convey_yuv4xxp16_on_yvxx", "Dither_convey_rgb48_on_yv12", "Dither_removegrain16", "Dither_median16", "Dither_get_msb", "Dither_get_lsb", "Dither_addborders16", "Dither_lut8", "Dither_lutxy8", "Dither_lutxyz8", "Dither_lut16", "Dither_add16", "Dither_sub16", "Dither_max_dif16", "Dither_min_dif16", "Dither_merge16", "Dither_merge16_8", "Dither_sigmoid_direct", "Dither_sigmoid_inverse", "Dither_add_grain16", "Dither_Luma_Rebuild"}})

        Add(New PluginPackage With {
            .Name = "Dither DLL",
            .Location = "Plugins\AVS\Dither",
            .Description = "This package offers a set of tools to manipulate high-bitdepth (16 bits per plane) video clips. The most proeminent features are color banding artifact removal, dithering to 8 bits, colorspace conversions and resizing.",
            .HelpFilename = "dither.html",
            .Filename = "dither.dll",
            .WebURL = "http://avisynth.nl/index.php/Dither",
            .AvsFilterNames = {"Dither_y_gamma_to_linear", "Dither_y_linear_to_gamma", "Dither_convert_8_to_16", "Dither1Pre", "Dither1Pre", "Dither_repair16", "Dither_convert_yuv_to_rgb", "Dither_convert_rgb_to_yuv", "Dither_resize16", "DitherPost", "Dither_crop16", "DitherBuildMask", "SmoothGrad", "GradFun3", "Dither_box_filter16", "Dither_bilateral16", "Dither_limit_dif16", "Dither_resize16nr", "Dither_srgb_display", "Dither_convey_yuv4xxp16_on_yvxx", "Dither_convey_rgb48_on_yv12", "Dither_removegrain16", "Dither_median16", "Dither_get_msb", "Dither_get_lsb", "Dither_addborders16", "Dither_lut8", "Dither_lutxy8", "Dither_lutxyz8", "Dither_lut16", "Dither_add16", "Dither_sub16", "Dither_max_dif16", "Dither_min_dif16", "Dither_merge16", "Dither_merge16_8", "Dither_sigmoid_direct", "Dither_sigmoid_inverse", "Dither_add_grain16", "Dither_Luma_Rebuild"}})

        Add(New PluginPackage With {
            .Name = "DeGrainMedian",
            .Filename = "DeGrainMedian.dll",
            .Description = "DeGrainMedian is a spatio-temporal limited median filter mainly for film grain removal, but may be used for general denoising.",
            .HelpFilename = "Degrainmedian.html",
            .WebURL = "http://avisynth.nl/index.php/DeGrainMedian",
            .AvsFilterNames = {"DeGrainMedian"},
            .AvsFiltersFunc = Function() {New VideoFilter("Noise", "DeGrainMedian", "DeGrainMedian(limitY=4, limitUV=6, mode=1, interlaced=false, norow=false)")}})

        Add(New PluginPackage With {
            .Name = "pSharpen",
            .Filename = "pSharpen.avsi",
            .AvsFilterNames = {"pSharpen"},
            .Description = "pSharpen performs two-point sharpening to avoid overshoot.",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/PSharpen",
            .AvsFiltersFunc = Function() {New VideoFilter("Line", "Sharpen | pSharpen", "pSharpen(strength=25, threshold=75, ss_x=1.0, ss_y=1.0)")}})

        Add(New PluginPackage With {
            .Name = "GradFun2DBmod",
            .Location = "Plugins\AVS\GradFun2DB",
            .Filename = "GradFun2DBmod.avsi",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/GradFun2dbmod",
            .Description = "An advanced debanding script based on GradFun2DB.",
            .AvsFilterNames = {"GradFun2DBmod"}})

        Add(New PluginPackage With {
            .Name = "GradFun2DB",
            .Location = "Plugins\AVS\GradFun2DB",
            .Filename = "gradfun2db.dll",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/GradFun2db",
            .Description = "A simple and fast debanding filter.",
            .AvsFilterNames = {"gradfun2db"}})

        Add(New PluginPackage With {
            .Name = "AddGrainC",
            .Filename = "AddGrainC.dll",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/AddGrainC",
            .Description = "Generate film-like grain or other effects (like rain) by adding random noise to a video clip.",
            .AvsFilterNames = {"AddGrainC", "AddGrain"}})

        Add(New PluginPackage With {
            .Name = "YFRC",
            .Filename = "YFRC.avsi",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/YFRC",
            .Description = "Yushko Frame Rate convertor - doubles the frame rate with strong artifact detection and scene change detection. YFRC uses masks to reduce artifacts in areas where interpolation failed.",
            .AvsFilterNames = {"YFRC"},
            .AvsFiltersFunc = Function() {New VideoFilter("FrameRate", "YRFC", "YFRC(BlockH=16, BlockV=16, OverlayType=0, MaskExpand=1)")}})

        Add(New PluginPackage With {
            .Name = "Deblock",
            .Filename = "Deblock.dll",
            .Location = "Plugins\VS\Deblock",
            .Description = "Deblocking plugin using the deblocking filter of h264.",
            .WebURL = "http://github.com/HomeOfVapourSynthEvolution/VapourSynth-Deblock/",
            .VSFilterNames = {"deblock.Deblock"},
            .VSFiltersFunc = Function() {New VideoFilter("Restoration", "DeBlock | DeBlock", "clip = core.deblock.Deblock(clip, quant=25, aoffset=0, boffset=0)")}})

        Add(New PluginPackage With {
            .Name = "MSharpen",
            .Filename = "msharpen.dll",
            .WebURL = "http://avisynth.nl/index.php/MSharpen",
            .HelpFilename = "Readme.txt",
            .AvsFilterNames = {"MSharpen"},
            .AvsFiltersFunc = Function() {New VideoFilter("Line", "Sharpen | MSharpen", "MSharpen(threshold=10, strength=100, highq=true, mask=false)")}})

        Add(New PluginPackage With {
            .Name = "mClean",
            .Filename = "mClean.avsi",
            .WebURL = "http://forum.doom9.org/showthread.php?t=174804",
            .Description = "Removes noise whilst retaining as much detail as possible.",
            .AvsFilterNames = {"mClean"},
            .AvsFiltersFunc = Function() {New VideoFilter("Noise", "mClean", "mClean()")}})

        Add(New PluginPackage With {
            .Name = "vsCube",
            .Filename = "vscube.dll",
            .Description = "Deblocking plugin using the deblocking filter of h264.",
            .WebURL = "http://rationalqm.us/mine.html",
            .Location = "Plugins\AVS\VSCube",
            .AvsFilterNames = {"Cube"},
            .AvsFiltersFunc = Function() {New VideoFilter("Color", "HDRCore | Cube", "Cube(""$browse_file$"")")}})

        Add(New PluginPackage With {
            .Name = "RgTools",
            .Filename = "RgTools.dll",
            .WebURL = "http://github.com/pinterf/RgTools",
            .HelpURL = "https://github.com/pinterf/RgTools/blob/master/RgTools/documentation/RgTools.txt",
            .Description = "RgTools is a modern rewrite of RemoveGrain, Repair, BackwardClense, Clense, ForwardClense and VerticalCleaner all in a single plugin.",
            .AvsFilterNames = {"RemoveGrain", "Clense", "ForwardClense", "BackwardClense", "Repair", "VerticalCleaner"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Noise", "RemoveGrain | RemoveGrain", "RemoveGrain(mode=2, modeU=2, modeV=2, planar=false)"),
                New VideoFilter("Noise", "RemoveGrain | RemoveGrain With Repair", "Processed = RemoveGrain(mode=2, modeU=2, modeV=2, planar=false)" + BR + "Repair(Processed, mode=2, modeU=2, modeV=2, planar=false)")}})

        Add(New PluginPackage With {
            .Name = "JPSDR",
            .Filename = "Plugins_JPSDR.dll",
            .WebURL = "https://github.com/jpsdr/plugins_JPSDR",
            .DownloadURL = "https://github.com/jpsdr/plugins_JPSDR/releases",
            .Description = "Merge of AutoYUY2, NNEDI3, HDRTools, aWarpSharpMT and ResampleMT. Included is the W7 AVX variant.",
            .AvsFilterNames = {"aBlur", "aSobel", "AutoYUY2", "aWarp", "aWarp4", "aWarpSharp2", "BicubicResizeMT", "BilinearResizeMT", "BlackmanResizeMT", "ConvertLinearRGBtoYUV", "ConvertRGB_Hable_HDRtoSDR", "ConvertRGB_Mobius_HDRtoSDR", "ConvertRGB_Reinhard_HDRtoSDR", "ConvertRGBtoXYZ", "ConvertXYZ_Hable_HDRtoSDR", "ConvertXYZ_Mobius_HDRtoSDR", "ConvertXYZ_Reinhard_HDRtoSDR", "ConvertXYZ_Scale_HDRtoSDR", "ConvertXYZ_Scale_SDRtoHDR", "ConvertXYZtoRGB", "ConvertXYZtoYUV", "ConvertYUVtoLinearRGB", "ConvertYUVtoXYZ", "DeBicubicResizeMT", "DeBilinearResizeMT", "DeBlackmanResizeMT", "DeGaussResizeMT", "DeLanczos4ResizeMT", "DeLanczosResizeMT", "DeSincResizeMT", "DeSpline16ResizeMT", "DeSpline36ResizeMT", "DeSpline64ResizeMT", "GaussResizeMT", "Lanczos4ResizeMT", "LanczosResizeMT", "nnedi3", "PointResizeMT", "SincResizeMT", "Spline16ResizeMT", "Spline36ResizeMT", "Spline64ResizeMT"},
            .AvsFiltersFunc = Function() {
                New VideoFilter("Field", "nnedi3", "nnedi3(field=1)"),
                New VideoFilter("Resize", "Advanced | ResizeMT", "$select:BicubicResizeMT;BilinearResizeMT;BlackmanResizeMT;GaussResizeMT;Lanczos4ResizeMT;LanczosResizeMT;PointResizeMT;SincResizeMT;Spline16ResizeMT;Spline36ResizeMT;Spline64ResizeMT$(%target_width%, %target_height%, prefetch=4)"),
                New VideoFilter("Line", "Sharpen | aWarpSharp2", "aWarpSharp2(thresh=128, blur=2, type=0, depth=16, chroma=3)")}})

        Add(New PluginPackage With {
            .Name = "TDeint",
            .Filename = "TDeint.dll",
            .Location = "Plugins\AVS\TDeint",
            .WebURL = "https://github.com/pinterf/TIVTC",
            .DownloadURL = "https://github.com/pinterf/TIVTC/releases",
            .Description = "TDeint is a bi-directionally, motion adaptive, sharp deinterlacer.",
            .AvsFilterNames = {"TDeint"},
            .AvsFiltersFunc = Function() {New VideoFilter("Field", "TDeint", "TDeint()")}})

        Add(New PluginPackage With {
            .Name = "TIVTC",
            .Filename = "TIVTC.dll",
            .WebURL = "http://github.com/pinterf/TIVTC",
            .DownloadURL = "https://github.com/pinterf/TIVTC/releases",
            .HelpURL = "https://github.com/pinterf/TIVTC/tree/master/Doc_TIVTC",
            .Description = "TIVTC is a plugin package containing 7 different filters and 3 conditional functions.",
            .AvsFilterNames = {"TFM", "TDecimate", "MergeHints", "FrameDiff", "FieldDiff", "ShowCombedTIVTC", "RequestLinear"}})

        Add(New PluginPackage With {
            .Name = "masktools2",
            .Filename = "masktools2.dll",
            .WebURL = "http://github.com/pinterf/masktools",
            .DownloadURL = "https://github.com/pinterf/masktools/releases",
            .Description = "MaskTools2 contain a set of filters designed to create, manipulate and use masks. Masks, in video processing, are a way to give a relative importance to each pixel. You can, for example, create a mask that selects only the green parts of the video, and then replace those parts with another video.",
            .AvsFilterNames = {"mt_adddiff", "mt_average", "mt_binarize", "mt_circle", "mt_clamp", "mt_convolution", "mt_diamond", "mt_edge", "mt_ellipse", "mt_expand", "mt_hysteresis", "mt_inflate", "mt_inpand", "mt_invert", "mt_logic", "mt_losange", "mt_lut", "mt_lutf", "mt_luts", "mt_lutxy", "mt_makediff", "mt_mappedblur", "mt_merge", "mt_motion", "mt_polish", "mt_rectangle", "mt_square"}})

        Add(New PluginPackage With {
            .Name = "FluxSmooth",
            .Filename = "FluxSmooth.dll",
            .AvsFilterNames = {"FluxSmoothT", "FluxSmoothST"},
            .Description = "One of the fundamental properties of noise is that it's random. One of the fundamental properties of motion is that it's not. This is the premise behind FluxSmooth, which examines each pixel and compares it to the corresponding pixel in the previous and last frame. Smoothing occurs if both the previous frame's value and the next frame's value are greater, or if both are less, than the value in the current frame.",
            .WebURL = "http://avisynth.nl/index.php/FluxSmooth"})

        Add(New PluginPackage With {
            .Name = "yadifmod2",
            .Filename = "yadifmod2.dll",
            .Description = "Yet Another Deinterlacing Filter mod",
            .WebURL = "https://github.com/Asd-g/yadifmod2",
            .HelpUrlAviSynth = "https://github.com/Asd-g/yadifmod2/blob/master-1/avisynth/readme.md",
            .HelpUrlVapourSynth = "https://github.com/Asd-g/yadifmod2/blob/master-1/vapoursynth/readme.md",
            .AvsFilterNames = {"yadifmod2"},
            .AvsFiltersFunc = Function() {New VideoFilter("Field", "yadifmod2", "yadifmod2()")}})

        Add(New PluginPackage With {
            .Name = "Yadifmod",
            .Filename = "Yadifmod.dll",
            .Description = "Modified version of Fizick's avisynth filter port of yadif from mplayer. This version doesn't internally generate spatial predictions, but takes them from an external clip.",
            .WebURL = "http://github.com/HomeOfVapourSynthEvolution/VapourSynth-Yadifmod",
            .VSFilterNames = {"yadifmod.Yadifmod"},
            .VSFiltersFunc = Function() {New VideoFilter("Field", "Yadifmod", "clip = core.yadifmod.Yadifmod(clip, core.nnedi3.nnedi3(clip, field=0), order=1, field=-1, mode=0)")}})

        Add(New PluginPackage With {
            .Name = "FixTelecinedFades",
            .Filename = "libftf_em64t_avx_fma.dll",
            .Location = "Plugins\VS\FixTelecinedFades",
            .Description = "InsaneAA Anti-Aliasing Script.",
            .WebURL = "https://github.com/IFeelBloated/Fix-Telecined-Fades",
            .VSFilterNames = {"ftf.FixFades"},
            .VSFiltersFunc = Function() {New VideoFilter("Restoration", "RCR | Fix Telecined Fades", "clip = core.fmtc.bitdepth (clip, bits=32)" + BR + "clip = core.ftf.FixFades(clip)" + BR + "clip = core.fmtc.bitdepth (clip, bits=8)")}})

        Add(New PluginPackage With {
            .Name = "vcmod",
            .Filename = "vcmod.dll",
            .HelpFilename = "vcmod.html",
            .Description = "vcmod plugin for VapourSynth.",
            .WebURL = "http://www.avisynth.nl/users/vcmohan/vcmod/vcmod.html",
            .VSFilterNames = {"vcmod.Median", "vcmod.Variance", "vcmod.Amplitude", "vcmod.GBlur", "vcmod.MBlur", "vcmod.Histogram", "vcmod.Fan", "vcmod.Variance", "vcmod.Neural", "vcmod.Veed", "vcmod.SaltPepper"}})

        Add(New PluginPackage With {
            .Name = "vcmove",
            .Filename = "vcmove.dll",
            .HelpFilename = "vcmove.html",
            .Description = "vcmove plugin for VapourSynth.",
            .WebURL = "http://www.avisynth.nl/users/vcmohan/vcmove/vcmove.html",
            .VSFilterNames = {"vcmove.Rotate", "vcmove.DeBarrel", "vcmove.Quad2Rect", "vcmove.Rect2Quad"}})

        Add(New PluginPackage With {
            .Name = "vcfreq",
            .Filename = "vcfreq.dll",
            .HelpFilename = "vcfreq.html",
            .Description = "vcvcfreq plugin for VapourSynth.",
            .WebURL = "http://www.avisynth.nl/users/vcmohan/vcfreq/vcfreq.html",
            .VSFilterNames = {"vcfreq.F1Quiver", "vcfreq.F2Quiver", "vcfreq.Blur", "vcfreq.Sharp"}})

        Add(New PluginPackage With {
            .Name = "nnedi3",
            .Filename = "libnnedi3.dll",
            .Location = "Plugins\VS\nnedi3",
            .WebURL = "http://github.com/dubhater/vapoursynth-nnedi3",
            .Description = "nnedi3 is an intra-field only deinterlacer. It takes in a frame, throws away one field, and then interpolates the missing pixels using only information from the kept field.",
            .VSFilterNames = {"nnedi3.nnedi3"}})

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
                "mv.Degrain3", "mv.Mask", "mv.Finest", "mv.Flow", "mv.FlowBlur", "mv.FlowInter", "mv.FlowFPS", "mv.BlockFPS", "mv.SCDetection",
                "mv.DepanAnalyse", "mv.DepanEstimate", "mv.DepanCompensate", "mv.DepanStabilise"}})

        Add(New PluginPackage With {
            .Name = "mvtools-sf",
            .Filename = "libmvtools_sf_em64t.dll",
            .WebURL = "http://github.com/dubhater/vapoursynth-mvtools",
            .Description = "MVTools is a set of filters for motion estimation and compensation.",
            .VSFilterNames = {"mvsf.Super", "mvsf.Analyse", "mvsf.Recalculate", "mvsf.Compensate", "mvsf.Degrain1", "mvsf.Degrain2",
                "mvsf.Degrain3", "mvsf.Mask", "mvsf.Finest", "mvsf.Flow", "mvsf.FlowBlur", "mvsf.FlowInter", "mvsf.FlowFPS", "mvsf.BlockFPS", "mvsf.SCDetection",
                "mvsf.DepanAnalyse", "mvsf.DepanEstimate", "mvsf.DepanCompensate", "mvsf.DepanStabilise"}})

        Add(New PluginPackage With {
            .Name = "TemporalMedian",
            .Filename = "libtemporalmedian.dll",
            .WebURL = "https://github.com/dubhater/vapoursynth-temporalmedian",
            .Description = "TemporalMedian is a temporal denoising filter. It replaces every pixel with the median of its temporal neighbourhood.",
            .VSFilterNames = {"tmedian.TemporalMedian"}})

        Add(New PluginPackage With {
            .Name = "resamplehq",
            .Filename = "resamplehq.py",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "https://gist.github.com/4re/b5399b1801072458fc80#file-mcdegrainsharp-py",
            .Description = "TemporalMedian is a temporal denoising filter. It replaces every pixel with the median of its temporal neighbourhood.",
            .VSFilterNames = {"resamplehq.resamplehq"},
            .VSFiltersFunc = Function() {New VideoFilter("Resize", "ReSampleHQ", "clip = resamplehq.resamplehq(clip, %target_width%, %target_height%, kernel='$select:Point;Rect;Linear;Cubic;Lanczos;Blackman;Blackmanminlobe;Spline16;Spline36;Spline64;Gauss;Sinc$')")}})

        Add(New PluginPackage With {
            .Name = "mcdegrainsharp",
            .Filename = "mcdegrainsharp.py",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "https://gist.github.com/4re/b5399b1801072458fc80#file-mcdegrainsharp-py",
            .Description = "TemporalMedian is a temporal denoising filter. It replaces every pixel with the median of its temporal neighbourhood.",
            .VSFilterNames = {"mcdegrainsharp.mcdegrainsharp"},
            .VSFiltersFunc = Function() {New VideoFilter("Line", "Sharpen | McDegrainSharp", "clip = mcdegrainsharp.mcdegrainsharp(clip, plane=4)")}})

        Add(New PluginPackage With {
            .Name = "G41Fun",
            .Filename = "G41Fun.py",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "https://github.com/Helenerineium/hnwvsfunc",
            .Description = "The replaced script for hnwvsfunc with re-written functions.",
            .VSFilterNames = {"G41Fun.mClean", "G41Fun.NonlinUSM", "G41Fun.DetailSharpen", "G41Fun.LUSM", "G41Fun.JohnFPS", "G41Fun.TemporalDegrain2",
                "G41Fun.MCDegrainSharp", "G41Fun.FineSharp", "G41Fun.psharpen", "G41Fun.QTGMC", "G41Fun.SMDegrain", "G41Fun.daamod",
                "G41Fun.STPressoHD", "G41Fun.MLDegrain", "G41Fun.Hysteria", "G41Fun.SuperToon", "G41Fun.EdgeDetect", "G41Fun.SpotLess",
                "G41Fun.HQDeringmod", "G41Fun.LSFmod", "G41Fun.SeeSaw", "G41Fun.MaskedDHA"}})

        Add(New PluginPackage With {
            .Name = "fvsfunc",
            .Filename = "fvsfunc.py",
            .Description = "Small collection of VapourSynth functions",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "https://github.com/Irrational-Encoding-Wizardry/fvsfunc",
            .VSFilterNames = {"fvsfunc.GradFun3mod", "fvsfunc.DescaleM", "fvsfunc.Downscale444", "fvsfunc.JIVTC", "fvsfunc.OverlayInter", "fvsfunc.AutoDeblock", "fvsfunc.ReplaceFrames", "fvsfunc.maa", "fvsfunc.TemporalDegrain", "fvsfunc.DescaleAA", "fvsfunc.InsertSign"}})

        Add(New PluginPackage With {
            .Name = "nnedi3_rpow2",
            .Filename = "nnedi3_rpow2.py",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "https://github.com/Irrational-Encoding-Wizardry/fvsfunc",
            .Description = "nnedi3_rpow2 ported from Avisynth for VapourSynth",
            .VSFilterNames = {"nnedi3_rpow2"}})

        Add(New PluginPackage With {
            .Name = "mvmulti",
            .Filename = "mvmulti.py",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "http://github.com/dubhater/vapoursynth-mvtools",
            .Description = "MVTools is a set of filters for motion estimation and compensation.",
            .VSFilterNames = {"mvmulti.StoreVect", "mvmulti.Analyse", "mvmulti.Recalculate", "mvmulti.Compensate", "mvmulti.Restore", "mvmulti.Flow", "mvmulti.DegrainN"}})

        Add(New PluginPackage With {
            .Name = "Sangnom",
            .Filename = "libsangnom.dll",
            .WebURL = "https://bitbucket.org/James1201/vapoursynth-sangnom/overview",
            .Description = "SangNom is a single field deinterlacer using edge-directed interpolation but nowadays it's mainly used in anti-aliasing scripts.",
            .VSFilterNames = {"sangnom.SangNom"},
            .VSFiltersFunc = Function() {New VideoFilter("Line", "Anti-Aliasing | Sangnom", "clip = core.sangnom.SangNom(clip)")}})

        Add(New PluginPackage With {
            .Name = "znedi3",
            .Filename = "vsznedi3.dll",
            .Location = "Plugins\VS\nnedi3",
            .HelpFilename = "Readme.txt",
            .WebURL = "https://github.com/sekrit-twc/znedi3",
            .Description = "znedi3 is a CPU-optimized version of nnedi.",
            .VSFilterNames = {"znedi3.nnedi3"}})

        Add(New PluginPackage With {
            .Name = "nnedi3cl",
            .Filename = "NNEDI3CL.dll",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-NNEDI3CL",
            .Location = "Plugins\VS\nnedi3",
            .Description = "nnedi3 is an intra-field only deinterlacer. It takes a frame, throws away one field, and then interpolates the missing pixels using only information from the remaining field. It is also good for enlarging images by powers of two.",
            .VSFilterNames = {"nnedi3cl.NNEDI3CL"}})

        Add(New PluginPackage With {
            .Name = "EEDI3m",
            .Filename = "EEDI3m.dll",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-EEDI3",
            .Description = "EEDI3 works by finding the best non-decreasing (non-crossing) warping between two lines by minimizing a cost functional.",
            .VSFilterNames = {"eedi3m.EEDI3"}})

        Add(New PluginPackage With {
            .Name = "EEDI2",
            .Filename = "EEDI2.dll",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-EEDI2",
            .Description = "EEDI2 works by finding the best non-decreasing (non-crossing) warping between two lines by minimizing a cost functional.",
            .VSFilterNames = {"eedi2.EEDI2"}})

        Add(New PluginPackage With {
            .Name = "W3FDIF",
            .Filename = "W3FDIF.dll",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-W3FDIF/releases",
            .Description = "Weston 3 Field Deinterlacing Filter. Ported from FFmpeg's libavfilter.",
            .VSFilterNames = {"w3fdif.W3FDIF"}})

        Add(New PluginPackage With {
            .Name = "MiniDeen",
            .Filename = "neo-minideen.dll",
            .WebURL = "https://github.com/HomeOfAviSynthPlusEvolution/MiniDeen",
            .DownloadURL = "https://github.com/HomeOfAviSynthPlusEvolution/MiniDeen/releases",
            .Description = "MiniDeen is a spatial denoising filter which replaces every pixel with the average of its neighbourhood.",
            .AvsFilterNames = {"MiniDeen"},
            .AvsFiltersFunc = Function() {New VideoFilter("Noise", "MiniDeen", "MiniDeen(radius=1, thrY=10, thrUV=12, Y=3, U=3, V=3)")},
            .VSFilterNames = {"neo_minideen.MiniDeen"},
            .VSFiltersFunc = Function() {New VideoFilter("Noise", "MiniDeen", "clip = core.neo_minideen.MiniDeen(clip, radius=[1,1,1], threshold=[10,12,12], planes=[0,1,2])")}})

        Add(New PluginPackage With {
            .Name = "IT",
            .Filename = "vs_it.dll",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-IT",
            .Description = "VapourSynth Plugin - Inverse Telecine (YV12 Only, IT-0051 base, IT_YV12-0103 base).",
            .VSFilterNames = {"it.IT"}})

        Add(New PluginPackage With {
            .Name = "TDeintMod",
            .Filename = "TDeintMod.dll",
            .HelpFilename = "Readme.txt",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-TDeintMod",
            .Description = "TDeintMod is a combination of TDeint and TMM, which are both ported from tritical's AviSynth plugin.",
            .VSFilterNames = {"tdm.TDeintMod"}})

        Add(New PluginPackage With {
            .Name = "adjust",
            .Filename = "adjust.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "very basic port of the built-in Avisynth filter Tweak.",
            .WebURL = "http://github.com/dubhater/vapoursynth-adjust",
            .VSFilterNames = {"adjust.Tweak"}})

        Add(New PluginPackage With {
            .Name = "Oyster",
            .Filename = "Oyster.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "Oyster is an experimental implement of the Blocking Matching concept, designed specifically for compression artifacts removal.",
            .WebURL = "https://github.com/IFeelBloated/Oyster",
            .VSFilterNames = {"Oyster.Basic", "Oyster.Deringing", "Oyster.Destaircase", "Oyster.Deblocking", "Oyster.Super"}})

        Add(New PluginPackage With {
            .Name = "Plum",
            .Filename = "Plum.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "Plum is a sharpening/blind deconvolution suite with certain advanced features like Non-Local error, Block Matching, etc..",
            .WebURL = "https://github.com/IFeelBloated/Plum",
            .VSFilterNames = {"Plum.Super", "Plum.Basic", "Plum.Final"}})

        Add(New PluginPackage With {
            .Name = "Vine",
            .Filename = "Vine.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "Plum is a sharpening/blind deconvolution suite with certain advanced features like Non-Local error, Block Matching, etc..",
            .WebURL = "https://github.com/IFeelBloated/Plum",
            .VSFilterNames = {"Vine.Super", "Vine.Basic", "Vine.Final", "Vine.Dilation", "Vine.Erosion", "Vine.Closing", "Vine.Opening", "Vine.Gradient", "Vine.TopHat", "Vine.Blackhat"}})

        Add(New PluginPackage With {
            .Name = "TCanny",
            .Filename = "TCanny.dll",
            .Description = "Builds an edge map using canny edge detection.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-TCanny",
            .VSFilterNames = {"tcanny.TCanny"}})

        Add(New PluginPackage With {
            .Name = "taa",
            .Filename = "vsTAAmbk.py",
            .Description = "A ported AA-script from Avisynth.",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/vsTAAmbk",
            .VSFilterNames = {"taa.TAAmbk", "taa.vsTAAmbk"},
            .VSFiltersFunc = Function() {New VideoFilter("Line", "Anti-Aliasing | TAAmbk", "clip = taa.TAAmbk(clip, preaa=-1, aatype=4, mtype=1, mthr=24, sharp=-1,aarepair=-20, postaa=False, stabilize=1)")}})

        Add(New PluginPackage With {
            .Name = "mvsfunc",
            .Filename = "mvsfunc.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "mawen1250's VapourSynth functions.",
            .WebURL = "http://github.com/HomeOfVapourSynthEvolution/mvsfunc",
            .HelpURL = "http://forum.doom9.org/showthread.php?t=172564",
            .VSFilterNames = {
                "mvsfunc.Depth", "mvsfunc.ToRGB", "mvsfunc.ToYUV", "mvsfunc.BM3D", "mvsfunc.VFRSplice",
                "mvsfunc.PlaneStatistics", "mvsfunc.PlaneCompare", "mvsfunc.ShowAverage", "mvsfunc.FilterIf",
                "mvsfunc.FilterCombed", "mvsfunc.Min", "mvsfunc.Max", "mvsfunc.Avg", "mvsfunc.MinFilter",
                "mvsfunc.MaxFilter", "mvsfunc.LimitFilter", "mvsfunc.PointPower", "mvsfunc.CheckMatrix",
                "mvsfunc.postfix2infix", "mvsfunc.SetColorSpace", "mvsfunc.AssumeFrame", "mvsfunc.AssumeTFF",
                "mvsfunc.AssumeBFF", "mvsfunc.AssumeField", "mvsfunc.AssumeCombed", "mvsfunc.CheckVersion",
                "mvsfunc.GetMatrix", "mvsfunc.zDepth", "mvsfunc.GetPlane", "mvsfunc.PlaneAverage",
                "mvsfunc.Preview", "mvsfunc.GrayScale"}})

        Add(New PluginPackage With {
            .Name = "Bwdif",
            .Filename = "Bwdif.dll",
            .Description = "Motion adaptive deinterlacing based on yadif with the use of w3fdif and cubic interpolation algorithms.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-Bwdif",
            .DownloadURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-Bwdif/releases",
            .VSFilterNames = {"bwdif.Bwdif"},
            .VSFiltersFunc = Function() {New VideoFilter("Field", "Bwdif", "clip = core.bwdif.Bwdif(clip, field=0)")}})

        Add(New PluginPackage With {
            .Name = "FluxSmooth",
            .Filename = "libfluxsmooth.dll",
            .VSFilterNames = {"flux.SmoothT", "flux.SmoothST"},
            .Description = "FluxSmooth is a filter for smoothing of fluctuations.",
            .WebURL = "http://github.com/dubhater/vapoursynth-fluxsmooth"})

        Add(New PluginPackage With {
            .Name = "CNR2",
            .Filename = "libcnr2.dll",
            .Location = "Plugins\VS\CNR2",
            .VSFilterNames = {"cnr2.Cnr2"},
            .Description = "Cnr2 is a temporal denoiser designed to denoise only the chroma.",
            .WebURL = "https://github.com/dubhater/vapoursynth-cnr2"})

        Add(New PluginPackage With {
            .Name = "CTMF",
            .Filename = "CTMF.dll",
            .VSFilterNames = {"ctmf.CTMF"},
            .Description = "Constant Time Median Filtering.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-CTMF"})

        Add(New PluginPackage With {
            .Name = "msmoosh",
            .Filename = "libmsmoosh.dll",
            .VSFilterNames = {"msmoosh.MSmooth", "msmoosh.MSharpen"},
            .Description = "MSmooth is a spatial smoother that doesn't touch edges." + BR + "MSharpen is a sharpener that tries to sharpen only edges.",
            .WebURL = "http://github.com/dubhater/vapoursynth-msmoosh",
            .VSFiltersFunc = Function() {
                New VideoFilter("Restoration", "DeBlock | MSmooth", "clip = core.msmoosh.MSmooth(clip, threshold=3.0, strength=1)"),
                New VideoFilter("Line", "Sharpen | MSharpen", "clip = core.msmoosh.MSharpen(clip, threshold=6.0, strength=39)")}})

        Add(New PluginPackage With {
            .Name = "SVPFlow 1",
            .Location = "Plugins\VS\SVPFlow",
            .Description = "Motion vectors search plugin  is a deeply refactored and modified version of MVTools2 Avisynth plugin",
            .Filename = "svpflow1_vs64.dll",
            .WebURL = "https://www.svp-team.com/wiki/Manual:SVPflow",
            .VSFilterNames = {"core.svp1.Super", "core.svp1.Analyse", "core.svp1.Convert"}})

        Add(New PluginPackage With {
            .Name = "SVPFlow 2",
            .Location = "Plugins\VS\SVPFlow",
            .Description = "Motion vectors search plugin is a deeply refactored and modified version of MVTools2 Avisynth plugin",
            .Filename = "svpflow2_vs64.dll",
            .WebURL = "https://www.svp-team.com/wiki/Manual:SVPflow",
            .VSFilterNames = {"core.svp2.SmoothFps"}})

        Add(New PluginPackage With {
            .Name = "Dither",
            .Location = "Plugins\VS\Scripts",
            .Description = "VapourSynth port of DitherTools",
            .Filename = "Dither.py",
            .WebURL = "https://github.com/IFeelBloated/VaporSynth-Functions",
            .VSFilterNames = {"Dither.sigmoid_direct", "Dither.sigmoid_inverse", "Dither.linear_to_gamma", "Dither.gamma_to_linear", "Dither.clamp16", "Dither.sbr16", "Dither.Resize16nr", "Dither.get_msb", "Dither.get_lsb"}})

        Add(New PluginPackage With {
            .Name = "DeblockPP7",
            .Filename = "DeblockPP7.dll",
            .Description = "VapourSynth port of pp7 from MPlayer.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DeblockPP7",
            .VSFilterNames = {"pp7.DeblockPP7"},
            .VSFiltersFunc = Function() {New VideoFilter("Restoration", "DeBlock | DeBlock PP7", "$select:msg:Select Strength;Medium|clip = core.pp7.DeblockPP7(clip=clip, mode=2);Hard|clip = core.pp7.DeblockPP7(clip=clip);Soft|clip = core.pp7.DeblockPP7(clip=clip, mode=1)$")}})

        Add(New PluginPackage With {
            .Name = "HQDN3D",
            .Filename = "libhqdn3d.dll",
            .Description = "Avisynth port of hqdn3d from avisynth/mplayer.",
            .WebURL = "https://github.com/Hinterwaeldlers/vapoursynth-hqdn3d",
            .VSFilterNames = {"hqdn3d.Hqdn3d"},
            .VSFiltersFunc = Function() {New VideoFilter("Noise", "HQDN3D", "clip = core.hqdn3d.Hqdn3d(clip=clip)")}})

        Add(New PluginPackage With {
            .Name = "VagueDenoiser",
            .Filename = "VagueDenoiser.dll",
            .Description = "VapourSynth port of VagueDenoiser.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-VagueDenoiser",
            .VSFilterNames = {"vd.VagueDenoiser"},
            .VSFiltersFunc = Function() {New VideoFilter("Noise", "VagueDenoiser", "clip = core.vd.VagueDenoiser(clip=clip, method=$select:msg:Select Strength;Soft|1;Hard|0$)")}})

        Add(New PluginPackage With {
            .Name = "TTempSmooth",
            .Filename = "TTempSmooth.dll",
            .Description = "VapourSynth port of TTempSmooth.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-TTempSmooth",
            .VSFilterNames = {"ttmpsm.TTempSmooth"},
            .VSFiltersFunc = Function() {New VideoFilter("Noise", "TTempSmooth", "clip =  core.ttmpsm.TTempSmooth(clip)")}})

        Add(New PluginPackage With {
            .Name = "TimeCube",
            .Filename = "vscube.dll",
            .Description = "Allows Usage of 3DLuts.",
            .WebURL = "https://github.com/sekrit-twc/timecube",
            .VSFilterNames = {"timecube.Cube"}})

        Add(New PluginPackage With {
            .Name = "DegrainMedian",
            .Filename = "libdegrainmedian.dll",
            .Description = "VapourSynth port of DegrainMedian",
            .WebURL = "https://github.com/dubhater/vapoursynth-degrainmedian",
            .VSFilterNames = {"dgm.DegrainMedian"},
            .VSFiltersFunc = Function() {New VideoFilter("Noise", "DegrainMedian", "clip = core.dgm.DegrainMedian(clip=clip, interlaced=False)")}})

        Add(New PluginPackage With {
            .Name = "psharpen",
            .Filename = "psharpen.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "VapourSynth port of pSharpen",
            .VSFilterNames = {"psharpen.psharpen"},
            .VSFiltersFunc = Function() {New VideoFilter("Line", "Sharpen | pSharpen", "clip = psharpen.psharpen(clip)")}})

        Add(New PluginPackage With {
            .Name = "AWarpSharp2",
            .Filename = "libawarpsharp2.dll",
            .Description = "VapourSynth port of AWarpSharp2",
            .WebURL = "https://github.com/dubhater/vapoursynth-awarpsharp2",
            .VSFilterNames = {"warp.AWarpSharp2"},
            .VSFiltersFunc = Function() {New VideoFilter("Line", "Sharpen | aWarpSharpen2", "clip = core.warp.AWarpSharp2(clip=clip, blur=2)")}})

        Add(New PluginPackage With {
            .Name = "fmtconv",
            .Filename = "fmtconv.dll",
            .WebURL = "http://github.com/EleonoreMizo/fmtconv",
            .HelpFilename = "doc\fmtconv.html",
            .Description = "Fmtconv is a format-conversion plug-in for the Vapoursynth video processing engine. It does resizing, bitdepth conversion with dithering and colorspace conversion.",
            .VSFilterNames = {"fmtc.bitdepth", "fmtc.convert", " core.fmtc.matrix", "fmtc.resample", "fmtc.transfer", "fmtc.primaries", " core.fmtc.matrix2020cl", "fmtc.stack16tonative", "nativetostack16"}})

        Add(New PluginPackage With {
            .Name = "finesharp",
            .Filename = "finesharp.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "Port of Didie's FineSharp script to VapourSynth.",
            .WebURL = "http://forum.doom9.org/showthread.php?p=1777860#post1777860",
            .VSFilterNames = {"finesharp.sharpen"},
            .VSFiltersFunc = Function() {New VideoFilter("Line", "Sharpen | FineSharp", "$select:msg:Select Strength;Light|clip = finesharp.sharpen(clip, mode=1, sstr=2, cstr=0.8, xstr=0.19, lstr=1.49, pstr=1.272);Moderate|clip = finesharp.sharpen(clip, mode=2, sstr=2.0,  cstr=1.3, xstr=0.0,  lstr=1.49, pstr=1.472);Strong|clip = finesharp.sharpen(clip, mode=3, sstr=6.0,  cstr=1.3, xstr=0.0,  lstr=1.49, pstr=1.472)$")}})

        Add(New PluginPackage With {
            .Name = "FineSharp",
            .Filename = "FineSharp.avsi",
            .Description = "Small and fast realtime-sharpening function for 1080p, or after scaling 720p -> 1080p. It's a generic sharpener only for good quality sources!",
            .WebURL = "http://avisynth.nl/index.php/FineSharp",
            .AvsFilterNames = {"FineSharp"},
            .AvsFiltersFunc = Function() {New VideoFilter("Line", "Sharpen | FineSharp", "$select:msg:Select Strength;Light|FineSharp(mode=1, sstr=2, cstr=0.8, xstr=0.19, lstr=1.49, pstr=1.272);Moderate|FineSharp(mode=2, sstr=2.0,  cstr=1.3, xstr=0.0,  lstr=1.49, pstr=1.472);Strong|FineSharp(mode=3, sstr=6.0,  cstr=1.3, xstr=0.0,  lstr=1.49, pstr=1.472)$")}})

        Add(New PluginPackage With {
            .Name = "CropResize",
            .Filename = "CropResize.avsi",
            .Description = "Advanced crop and resize AviSynth script.",
            .WebURL = "https://forum.videohelp.com/threads/393752-CropResize-Cropping-resizing-script",
            .AvsFilterNames = {"CropResize"},
            .AvsFiltersFunc = Function() {New VideoFilter("Resize", "Advanced | CropResize", $"CropResize(%target_width%, %target_height%, \{BR}    %crop_left%, %crop_top%, -%crop_right%, -%crop_bottom%, \{BR}    InDAR=%source_dar%, OutDAR=%target_dar%, Info=true)")}})

        Add(New Package With {
            .Name = "DirectX 9",
            .Filename = "d3d9.dll",
            .Description = "DirectX 9.0c End-User Runtime.",
            .DownloadURL = "https://www.microsoft.com/en-us/download/details.aspx?id=34429",
            .Location = Folder.System,
            .VersionAllowAny = True,
            .TreePath = "Runtimes",
            .RequiredFunc = Function() AviSynthShader.Required OrElse FFT3DGPU.Required})

        Try
            Dim versionFile = Folder.Apps + "Versions.txt"

            For Each line In File.ReadAllLines(versionFile)
                For Each pack In Items.Values
                    If line Like "*=*;*" Then
                        Dim name = line.Left("=").Trim

                        If name = pack.ID Then
                            pack.Version = line.Right("=").Right(";").Trim
                            Dim dateArray = line.Right("=").Left(";").Trim.Split("-"c)
                            pack.VersionDate = New DateTime(CInt(dateArray(0)), CInt(dateArray(1)), CInt(dateArray(2)))
                        End If
                    End If
                Next
            Next
        Catch ex As Exception
            MsgError("You are running a StaxRip version that don't has any apps included!" + BR2 +
                     "Please download and use a StaxRip version that has all required apps included." + BR2 +
                     "The Apps are expected to be located in a directory called 'Apps' in the startup folder besides the StaxRip executable.")
        End Try
    End Sub

    Shared Function Add(pack As Package) As Package
        Items(pack.ID) = pack
        Return pack
    End Function

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

    Private LaunchActionValue As Action

    Overridable Property LaunchAction As Action
        Get
            If LaunchActionValue Is Nothing Then
                If Description.ContainsEx("GUI app") Then
                    LaunchActionValue = Sub() g.ShellExecute(Path)
                ElseIf Not HelpSwitch Is Nothing Then
                    LaunchActionValue = Sub() g.DefaultCommands.ExecutePowerShellScript(
                        $"& '{Path}' {If(HelpSwitch.Contains("stderr"), HelpSwitch.Replace("stderr", ""), HelpSwitch)}", True)
                ElseIf Filename.Ext.EqualsAny("avsi", "py") Then
                    LaunchActionValue = Sub() g.ShellExecute(g.GetTextEditorPath, Path.Escape)
                End If
            End If

            Return LaunchActionValue
        End Get
        Set(value As Action)
            LaunchActionValue = value
        End Set
    End Property

    Private RequiredValue As Boolean = True

    Overridable Property Required() As Boolean
        Get
            If Not RequiredFunc Is Nothing Then
                Return RequiredFunc.Invoke
            End If

            Return RequiredValue
        End Get
        Set(value As Boolean)
            RequiredValue = value
        End Set
    End Property

    Function GetTypeName() As String
        If Not HelpSwitch Is Nothing Then
            Return "Console App"
        ElseIf Description.ContainsEx("GUI app") Then
            Return "GUI App"
        ElseIf TypeOf Me Is PluginPackage Then
            Dim plugin = DirectCast(Me, PluginPackage)

            If Not plugin.AvsFilterNames.NothingOrEmpty Then
                If plugin.Filename.Ext = "dll" Then
                    Return "AviSynth Plugin"
                ElseIf plugin.Filename.Ext.EqualsAny("avs", "avsi") Then
                    Return "AviSynth Script"
                End If
            ElseIf Not plugin.VSFilterNames.NothingOrEmpty Then
                If plugin.Filename.Ext = "dll" Then
                    Return "VapourSynth Plugin"
                ElseIf plugin.Filename.Ext = "py" Then
                    Return "VapourSynth Script"
                End If
            End If
        ElseIf Filename.Ext = "dll" Then
            Return "Library"
        Else
            Return "Misc"
        End If
    End Function

    Sub ShowHelp()
        Dim dic As New SortedDictionary(Of String, String)

        If HelpFilename = "" AndAlso Not HelpSwitch Is Nothing Then
            HelpFilename = Name + " Help.txt"

            If CreateHelpfile() = "" Then
                HelpFilename = ""
            End If
        End If

        If HelpFilename <> "" Then
            dic("Local") = Directory + HelpFilename
        End If

        dic("Online") = HelpURL
        dic("AviSynth") = HelpUrlAviSynth
        dic("VapourSynth") = HelpUrlVapourSynth

        Dim count = dic.Values.Where(Function(val) val <> "").Count

        If count > 1 Then
            Using dialog As New TaskDialog(Of String)
                dialog.MainInstruction = "Choose option"

                For Each pair In dic
                    If pair.Value <> "" Then
                        dialog.AddCommand(pair.Key, pair.Value)
                    End If
                Next

                If dialog.Show <> "" Then
                    CreateHelpfile()
                    g.ShellExecute(dialog.SelectedValue)
                End If
            End Using
        Else
            CreateHelpfile()

            If HelpFileOrURL <> "" AndAlso (HelpFileOrURL.Contains("http") OrElse
                File.Exists(HelpFileOrURL)) Then

                g.ShellExecute(HelpFileOrURL)
            Else
                MsgInfo("No help resource available.")
            End If
        End If
    End Sub

    Public ReadOnly Property HelpFile As String
        Get
            Return Directory + HelpFilename
        End Get
    End Property

    Public ReadOnly Property URL As String
        Get
            If WebURL <> "" Then
                Return WebURL
            ElseIf HelpURL <> "" Then
                Return HelpURL
            ElseIf HelpUrlAviSynth <> "" Then
                Return HelpUrlAviSynth
            ElseIf HelpUrlVapourSynth <> "" Then
                Return HelpUrlVapourSynth
            ElseIf DownloadURL <> "" Then
                Return DownloadURL
            End If
        End Get
    End Property

    Public ReadOnly Property HelpFileOrURL As String
        Get
            If HelpFilename <> "" Then
                Return HelpFile
            ElseIf HelpURL <> "" Then
                Return HelpURL
            ElseIf HelpUrlAviSynth <> "" Then
                Return HelpUrlAviSynth
            ElseIf HelpUrlVapourSynth <> "" Then
                Return HelpUrlVapourSynth
            ElseIf WebURL <> "" Then
                Return WebURL
            ElseIf DownloadURL <> "" Then
                Return DownloadURL
            End If
        End Get
    End Property

    Function CreateHelpfile() As String
        If File.Exists(HelpFile) Then
            Return HelpFile.ReadAllText
        End If

        Try
            If Not HelpSwitch Is Nothing Then
                Dim stderr = HelpSwitch.Contains("stderr")
                Dim switch = If(stderr, HelpSwitch.Replace("stderr", ""), HelpSwitch)

                File.WriteAllText(HelpFile, BR + ProcessHelp.GetConsoleOutput(
                    Path, switch, stderr).Trim + BR)

                Return HelpFile.ReadAllText
            End If
        Catch ex As Exception
        End Try

        Return ""
    End Function

    Function VerifyOK(Optional showEvenIfNotRequired As Boolean = False) As Boolean
        If (Required() OrElse showEvenIfNotRequired) AndAlso (Required AndAlso GetStatus() <> "") Then
            Using form As New AppsForm
                form.ShowPackage(Me)
                form.ShowDialog()

                If GetStatus() <> "" Then
                    Throw New AbortException()
                End If
            End Using

            If Required AndAlso GetStatus() <> "" Then
                Return False
            End If
        End If

        Return True
    End Function

    Overridable Function GetStatus() As String
        If GetStatusLocation() <> "" Then
            Return GetStatusLocation()
        End If

        If Not s.AllowToolsWithWrongVersion Then
            If GetStatusVersion() <> "" Then
                Return GetStatusVersion()
            End If
        End If

        If Not StatusFunc Is Nothing Then
            Return StatusFunc.Invoke
        End If
    End Function

    Function GetStatusVersion() As String
        Dim ret As String

        If Not IsVersionValid() Then
            If IsVersionOld() Then
                If Not VersionAllowOld Then
                    ret = $"The currently used version of {Name} is not compatible (too old)."
                Else
                    ret = $"An old {Name} version was found, click on Version (F12) and enter the name of this version or install a newer version."
                End If
            End If

            If IsVersionNew() Then
                ret = $"A new {Name} version was found, new versions are usually compatible, click on Version (F12) and enter the name of this version."
            End If
        End If

        Return ret
    End Function

    Function GetStatusDisplay() As String
        If GetStatus() <> "" Then
            Return GetStatus()
        End If

        Return "OK"
    End Function

    Function GetStatusLocation() As String
        If Path = "" Then
            Return "App not found, use the Path menu to locate the App."
        End If
    End Function

    Function IsVersionOld() As Boolean
        Dim filepath = Path

        If filepath <> "" Then
            If (VersionDate - File.GetLastWriteTimeUtc(filepath)).TotalDays > 3 Then
                Return True
            End If
        End If
    End Function

    Function IsVersionNew() As Boolean
        Dim filepath = Path

        If filepath <> "" Then
            If (VersionDate - File.GetLastWriteTimeUtc(filepath)).TotalDays < -3 Then
                Return True
            End If
        End If
    End Function

    Function IsVersionCorrect() As Boolean
        Return Not IsVersionOld() AndAlso Not IsVersionNew()
    End Function

    Overridable Function IsVersionValid() As Boolean
        If VersionAllowAny Then
            Return True
        End If

        If IsVersionNew() AndAlso VersionAllowNew Then
            Return True
        End If

        Dim filepath = Path

        If filepath <> "" Then
            Dim dt = File.GetLastWriteTimeUtc(filepath)
            Return dt.AddDays(-2) < VersionDate AndAlso dt.AddDays(2) > VersionDate
        End If
    End Function

    Function IsCustomPathAllowed() As Boolean
        Return Not Path.StartsWithEx(Folder.System) AndAlso Not Path.ContainsEx("FrameServer")
    End Function

    ReadOnly Property Directory As String
        Get
            Return Path.Dir
        End Get
    End Property

    Sub SetStoredPath(value As String)
        s.Storage.SetString(Name + "custom path", value)
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

    Function GetAviSynthHintDir() As String
        If Not s.AviSynthMode = FrameServerMode.Portable AndAlso
            File.Exists(Folder.System + Filename) Then

            Return Folder.System
        End If

        Return GetPathFromLocation("FrameServer\AviSynth").Dir
    End Function

    Function GetVapourSynthHintDir() As String
        If Not s.VapourSynthMode = FrameServerMode.Portable Then
            For Each key In {Registry.CurrentUser, Registry.LocalMachine}
                Dim dllPath = key.GetString("Software\VapourSynth", "VapourSynthDLL")

                If File.Exists(dllPath) Then
                    Return dllPath.Dir
                End If
            Next
        End If

        Return GetPathFromLocation("FrameServer\VapourSynth").Dir
    End Function

    Shared Function GetPythonHintDir() As String
        If Not FrameServerHelp.IsVapourSynthPortableUsed Then
            Dim exePath As String

            For Each key In {Registry.CurrentUser, Registry.LocalMachine}
                For Each keyName In key.GetKeyNames("SOFTWARE\Python\PythonCore")
                    exePath = key.GetString($"SOFTWARE\Python\PythonCore\{keyName}\InstallPath", "ExecutablePath")

                    If File.Exists(exePath) Then
                        Return exePath.Dir
                    End If
                Next
            Next

            exePath = FindEverywhere("python.exe", Python.IgnorePath)

            If exePath <> "" Then
                Return exePath.Dir
            End If
        End If

        Return Folder.Apps + "FrameServer\VapourSynth\"
    End Function

    Overridable ReadOnly Property Path As String
        Get
            Dim ret = GetStoredPath()

            If File.Exists(ret) Then
                Return ret
            End If

            If Location <> "" Then
                ret = GetPathFromLocation(Location)

                If ret <> "" Then
                    Return ret
                End If
            End If

            If Not Locations.NothingOrEmpty Then
                ret = GetPathFromLocation(Locations)

                If ret <> "" Then
                    Return ret
                End If
            End If

            If Not HintDirFunc Is Nothing Then
                ret = HintDirFunc.Invoke + Filename

                If File.Exists(ret) Then
                    Return ret
                End If
            End If

            Dim plugin = TryCast(Me, PluginPackage)

            If Not plugin Is Nothing Then
                If Not plugin.VSFilterNames Is Nothing AndAlso Not plugin.AvsFilterNames Is Nothing Then
                    ret = Folder.Apps + "Plugins\Dual\" + Name + "\" + Filename

                    If File.Exists(ret) Then
                        Return ret
                    End If
                Else
                    If plugin.VSFilterNames Is Nothing Then
                        ret = Folder.Apps + "Plugins\AVS\" + Name + "\" + Filename

                        If File.Exists(ret) Then
                            Return ret
                        End If
                    Else
                        ret = Folder.Apps + "Plugins\VS\" + Name + "\" + Filename

                        If File.Exists(ret) Then
                            Return ret
                        End If
                    End If
                End If
            End If

            If Find Then
                ret = FindEverywhere(Filename, IgnorePath)

                If ret <> "" Then
                    Return ret
                End If
            End If
        End Get
    End Property

    Function GetPathFromLocation(dir As String) As String
        If dir = "" Then
            Return Nothing
        End If

        If Not dir.Contains(":\") AndAlso Not dir.StartsWith("\\") Then
            dir = Folder.Apps + dir
        End If

        If File.Exists(dir.FixDir + Filename) Then
            Return dir.FixDir + Filename
        End If

        If g.Is32Bit AndAlso Filename.Contains("64.exe") Then
            Dim fp = dir.FixDir + Filename.Replace("64.exe", ".exe")

            If File.Exists(fp) Then
                Return fp
            End If
        End If
    End Function

    Function GetPathFromLocation(dirs As String()) As String
        For Each hintDir In dirs
            Dim ret = GetPathFromLocation(hintDir)

            If ret <> "" Then
                Return ret
            End If
        Next
    End Function

    Shared Function IsNotEmptyOrIgnored(filePath As String, ignorePath As String) As Boolean
        Return filePath <> "" AndAlso (ignorePath = Nothing OrElse Not filePath.Contains(ignorePath))
    End Function

    Shared Function FindEverywhere(fileName As String, Optional ignorePath As String = Nothing) As String
        Dim ret As String

        If fileName.Ext = "exe" Then
            ret = FindInMuiCacheKey(fileName)

            If IsNotEmptyOrIgnored(ret, ignorePath) Then
                Return ret
            End If

            ret = FindInAppKey(fileName)

            If IsNotEmptyOrIgnored(ret, ignorePath) Then
                Return ret
            End If
        End If

        ret = FindInPathEnvVar(fileName)

        If IsNotEmptyOrIgnored(ret, ignorePath) Then
            Return ret
        End If
    End Function

    Shared Function FindEverywhere(fileNames As String()) As String
        For Each fn In fileNames
            Dim ret = FindEverywhere(fn)

            If ret <> "" Then
                Return ret
            End If
        Next
    End Function

    Shared Function FindInMuiCacheKey(ParamArray fileNames As String()) As String
        For Each exeName In fileNames
            Using key = Registry.ClassesRoot.OpenSubKey("Local Settings\Software\Microsoft\Windows\Shell\MuiCache")
                If Not key Is Nothing Then
                    For Each valueName In key.GetValueNames
                        If valueName.Contains(exeName) Then
                            Dim ret = valueName.Left(exeName) + exeName

                            If File.Exists(ret) Then
                                Return ret
                            End If
                        End If
                    Next
                End If
            End Using

            Using key = Registry.CurrentUser.OpenSubKey("Software\Classes\Local Settings\Software\Microsoft\Windows\Shell\MuiCache")
                If Not key Is Nothing Then
                    For Each valueName In key.GetValueNames
                        If valueName.Contains(exeName) Then
                            Dim ret = valueName.Left(exeName) + exeName

                            If File.Exists(ret) Then
                                Return ret
                            End If
                        End If
                    Next
                End If
            End Using
        Next
    End Function

    Shared Function FindInPathEnvVar(filename As String) As String
        Dim paths = Environment.GetEnvironmentVariable("path").SplitNoEmpty(";")

        For Each folder In paths
            Dim filepath = folder.FixDir + filename

            If File.Exists(filepath) AndAlso Not New FileInfo(filepath).Length = 0 Then
                Return filepath
            End If
        Next
    End Function

    Shared Function FindInAppKey(filename As String) As String
        Dim value = Registry.ClassesRoot.GetString($"Applications\{filename}\shell\open\command", Nothing)
        value = value.Left(".exe").TrimStart(""""c)

        If File.Exists(value) Then
            Return value
        End If

        value = Registry.LocalMachine.GetString($"SOFTWARE\Classes\Applications\{filename}\shell\open\command", Nothing)
        value = value.Left(".exe").TrimStart(""""c)

        If File.Exists(value) Then
            Return value
        End If
    End Function

    Overrides Function ToString() As String
        Return Name
    End Function

    Function CompareTo(other As Package) As Integer Implements System.IComparable(Of Package).CompareTo
        Return Name.CompareTo(other.Name)
    End Function
End Class

Public Class PluginPackage
    Inherits Package

    Property AvsFilterNames As String()
    Property VSFilterNames As String()
    Property VSFiltersFunc As Func(Of VideoFilter())
    Property AvsFiltersFunc As Func(Of VideoFilter())

    Public Overrides Property Required As Boolean
        Get
            Return IsPluginPackageRequired(Me)
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Shared Function IsPluginPackageRequired(package As PluginPackage) As Boolean
        If p Is Nothing Then
            Return False
        End If

        If p.Script.Engine = ScriptEngine.AviSynth AndAlso
            Not package.AvsFilterNames.NothingOrEmpty Then

            Dim scriptLower = p.Script.GetScript().ToLowerInvariant

            For Each filterName In package.AvsFilterNames
                If scriptLower.Contains(filterName.ToLowerInvariant + "(") Then Return True

                If scriptLower.Contains("import") Then
                    Dim match = Regex.Match(scriptLower, "\bimport\s*\(\s*""\s*(.+\.avsi*)\s*""\s*\)",
                                            RegexOptions.IgnoreCase)

                    If match.Success AndAlso File.Exists(match.Groups(1).Value) Then
                        If match.Groups(1).Value.ReadAllText.ToLowerInvariant.Contains(
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