
Imports System.Text
Imports System.Text.RegularExpressions

Imports Microsoft.Win32

Public Class Package
    Implements IComparable(Of Package)

    Property AllowCustomPath As Boolean = True
    Property Description As String
    Property DownloadURL As String
    Property Exclude As String()
    Property Filename32 As String
    Property Filter As String
    Property Find As Boolean = True
    Property HelpFilename As String
    Property HelpSwitch As String
    Property HelpURL As String
    Property HelpUrlAviSynth As String
    Property HelpUrlVapourSynth As String
    Property HintDirFunc As Func(Of String)
    Property Include As String
    Property IsIncluded As Boolean = True
    Property Keep As String()
    Property Location As String
    Property Locations As String()
    Property Name As String
    Property RequiredFunc As Func(Of Boolean)
    Property SetupAction As Action
    Property Siblings As String()
    Property StatusFunc As Func(Of String)
    Property SupportsAutoUpdate As Boolean = True
    Property TreePath As String
    Property Version As String = ""
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
        .WebURL = "http://rationalqm.us/dgmpgdec/dgmpgdec.html",
        .Location = "Support\DgMpgDec",
        .SupportsAutoUpdate = False,
        .RequiredFunc = Function() CommandLineDemuxer.IsActive("%app:DGIndex%")})

    Shared Property DGDecode As Package = Add(New PluginPackage With {
        .Name = "DGDecode",
        .Filename = "DGDecode.dll",
        .WebURL = "http://rationalqm.us/dgmpgdec/dgmpgdec.html",
        .Description = "Source filter to open D2V index files created with DGIndex or D2V Witch.",
        .Location = "Support\DgMpgDec",
        .SupportsAutoUpdate = False,
        .AvsFilterNames = {"MPEG2Source"},
        .VsFilterNames = {"dgdecode.MPEG2Source"}})

    Shared Property D2VWitch As Package = Add(New Package With {
        .Name = "D2V Witch",
        .Filename = "d2vwitch.exe",
        .Description = "Portable MPEG-2 demuxing and d2v indexing GUI app.",
        .WebURL = "https://github.com/dubhater/D2VWitch",
        .DownloadURL = "https://github.com/dubhater/D2VWitch/releases",
        .Location = "Support\D2V Witch",
        .IsIncluded = False,
        .RequiredFunc = Function() CommandLineDemuxer.IsActive("%app:D2V Witch%"),
        .LaunchAction = Sub()
                            g.AddToPath(Package.d2vsourceVS.Directory)
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
        .Filename = "NicAudio_x64.dll",
        .WebURL = "http://avisynth.org.ru/docs/english/externalfilters/nicaudio.htm",
        .Description = "AviSynth audio source filter plugin.",
        .AvsFilterNames = {"NicAC3Source", "NicDTSSource", "NicMPASource", "NicMPG123Source", "NicLPCMSource", "RaWavSource"}})

    Shared Property UnDot As Package = Add(New PluginPackage With {
        .Name = "UnDot",
        .Filename = "UnDot.dll",
        .WebURL = "http://avisynth.nl/index.php/UnDot",
        .Description = "UnDot is a simple median filter plugin for removing dots, that is stray orphan pixels and mosquito noise.",
        .AvsFilterNames = {"UnDot"}})

    Shared Property ffmpeg As Package = Add(New Package With {
        .Name = "ffmpeg",
        .Filename = "ffmpeg.exe",
        .Locations = {"Encoders\ffmpeg", "FrameServer\AviSynth"},
        .SupportsAutoUpdate = False,
        .AllowCustomPath = False,
        .WebURL = "http://ffmpeg.org",
        .HelpURL = "http://www.ffmpeg.org/documentation.html",
        .DownloadURL = "https://github.com/staxrip/staxrip/wiki/ffmpeg",
        .HelpSwitch = "-h",
        .Description = "Versatile and free audio video convertor console app."})

    Shared Property ffmpeg_non_free As Package = Add(New Package With {
        .Name = "ffmpeg non-free",
        .Filename = "ffmpeg.exe",
        .WebURL = "http://ffmpeg.org",
        .HelpURL = "http://www.ffmpeg.org/documentation.html",
        .HelpSwitch = "-h",
        .IsIncluded = False,
        .VersionAllowAny = True,
        .Find = False,
        .Description = "Versatile audio video convertor console app. " +
                       "Custom build with non-free libraries like fdk-aac.",
        .RequiredFunc = Function() Audio.CommandContains("libfdk_aac")})

    Shared Property OpusDec As Package = Add(New Package With {
        .Name = "OpusDec",
        .Filename = "opusdec.exe",
        .Location = "Audio\Opus",
        .WebURL = "",
        .DownloadURL = "",
        .HelpSwitch = "--help",
        .SupportsAutoUpdate = False,
        .RequiredFunc = Function() False,
        .Description = "Opus is a totally open, royalty-free, highly versatile audio codec. Opus is unmatched for interactive speech and music transmission over the Internet, but is also intended for storage and streaming applications."})

    Shared Property OpusEnc As Package = Add(New Package With {
        .Name = "OpusEnc",
        .Filename = "opusenc.exe",
        .Location = "Audio\Opus",
        .WebURL = "",
        .DownloadURL = "",
        .HelpSwitch = "--help",
        .SupportsAutoUpdate = False,
        .RequiredFunc = Function() Audio.IsEncoderUsed(GuiAudioEncoder.opusenc),
        .Description = "Opus is a totally open, royalty-free, highly versatile audio codec. Opus is unmatched for interactive speech and music transmission over the Internet, but is also intended for storage and streaming applications."})

    Shared Property DEE As Package = Add(New Package With {
        .Name = "DEE",
        .Filename = "dee.exe",
        .Locations = {"Audio\DEE", "Audio\DeeZy", "Audio\DeeZy\DEE", "Audio\DeeZy\Apps\DEE", Folder.Settings + "Tools\DEE"},
        .WebURL = "https://customer.dolby.com/content-creation-and-delivery/dolby-encoding-engine-with-ac-4",
        .DownloadURL = "https://customer.dolby.com/content-creation-and-delivery/dolby-encoding-engine-with-ac-4",
        .HelpSwitch = "--help",
        .IsIncluded = False,
        .VersionAllowAny = True,
        .Find = False,
        .SupportsAutoUpdate = False,
        .RequiredFunc = Function() Audio.IsEncoderUsed(GuiAudioEncoder.deezy),
        .Description = "Dolby Encoding Engine."})

    Shared Property DeeZy As Package = Add(New Package With {
        .Name = "DeeZy",
        .Filename = "deezy.exe",
        .Location = "Audio\DeeZy",
        .WebURL = "https://github.com/jlw4049/DeeZy",
        .DownloadURL = "https://github.com/jlw4049/DeeZy/releases",
        .HelpURL = "https://github.com/jlw4049/DeeZy",
        .HelpSwitch = "--help",
        .SupportsAutoUpdate = False,
        .RequiredFunc = Function() Audio.IsEncoderUsed(GuiAudioEncoder.deezy),
        .Description = "Simple Dolby Encoding Engine CLI Wrapper."})

    Shared Property qaac As Package = Add(New Package With {
        .Name = "qaac",
        .Filename = "qaac64.exe",
        .Filename32 = "qaac.exe",
        .Location = "Audio\qaac",
        .WebURL = "http://github.com/nu774/qaac",
        .DownloadURL = "https://github.com/nu774/qaac/releases",
        .HelpSwitch = "-h",
        .Keep = {"QTfiles64", "libsndfile-1.dll", "libFLAC_dynamic.dll"},
        .RequiredFunc = Function() Audio.IsEncoderUsed(GuiAudioEncoder.qaac),
        .Description = "Console AAC encoder using the non-free Apple AAC encoder."})

    Shared Property AppleApplicationSupport As Package = Add(New Package With {
        .Name = "Apple Application Support",
        .Filename = "CoreAudioToolbox.dll",
        .SupportsAutoUpdate = False,
        .VersionAllowAny = True,
        .IsIncluded = False,
        .DownloadURL = "https://github.com/staxrip/staxrip/wiki/qaac",
        .Locations = {"Audio\qaac", "Audio\qaac\QTfiles64", Folder.Settings + "Tools\Apple Application Support"},
        .RequiredFunc = Function() Audio.IsEncoderUsed(GuiAudioEncoder.qaac),
        .Description = "qaac requires this library, it's not included because of a non-free license."})

    Shared Property fdkaac As Package = Add(New Package With {
        .Name = "fdkaac",
        .Filename = "fdkaac.exe",
        .Location = "Audio\fdkaac",
        .HelpSwitch = "-h",
        .IsIncluded = False,
        .VersionAllowAny = True,
        .Description = "Non-free AAC console encoder using libfdk-aac.",
        .WebURL = "http://github.com/nu774/fdkaac",
        .RequiredFunc = Function() Audio.IsEncoderUsed(GuiAudioEncoder.fdkaac)})

    Shared Property eac3to As Package = Add(New Package With {
        .Name = "eac3to",
        .Filename = "eac3to.exe",
        .Location = "Audio\eac3to",
        .WebURL = "http://forum.doom9.org/showthread.php?t=125966",
        .HelpURL = "http://en.wikibooks.org/wiki/Eac3to/How_to_Use",
        .HelpSwitch = "",
        .Description = "Audio convertor console app."})

    Shared Property NeroAAC As Package = Add(New Package With {
        .Name = "NeroAAC",
        .Filename = "neroAacEnc.exe",
        .HelpSwitch = "-?",
        .IsIncluded = False,
        .VersionAllowAny = True,
        .RequiredFunc = Function() Audio.IsEncoderUsed(GuiAudioEncoder.eac3to) AndAlso Audio.CommandContains("m4a"),
        .Description = "Non-free AAC audio convertor console app."})

    Shared Property MediaInfo As Package = Add(New Package With {
        .Name = "MediaInfo",
        .Filename = "MediaInfo.dll",
        .Location = "Support\MediaInfo.NET",
        .SupportsAutoUpdate = False,
        .WebURL = "http://mediaarea.net/en/MediaInfo",
        .DownloadURL = "https://mediaarea.net/en/MediaInfo/Download/Windows",
        .Description = "Library to retrieve info from media files."})

    Shared Property MediaInfoNET As Package = Add(New Package With {
        .Name = "MediaInfo.NET",
        .Filename = "MediaInfoNET.exe",
        .Location = "Support\MediaInfo.NET",
        .WebURL = "https://github.com/stax76/MediaInfo.NET",
        .DownloadURL = "https://github.com/stax76/MediaInfo.NET/releases",
        .SupportsAutoUpdate = False,
        .Description = "GUI app originally built for StaxRip to show info about media files."})

    Shared Property GetMediaInfo As Package = Add(New Package With {
        .Name = "Get-MediaInfo",
        .Location = "Support\MediaInfo.NET",
        .Filename = "Get-MediaInfo.ps1",
        .SupportsAutoUpdate = False,
        .Description = "Complete PowerShell MediaInfo solution used for the media info folder view.",
        .WebURL = "https://github.com/stax76/Get-MediaInfo",
        .DownloadURL = "https://github.com/stax76/Get-MediaInfo/releases"})

    Shared Property MP4Box As Package = Add(New Package With {
        .Name = "MP4Box",
        .Filename = "MP4Box.exe",
        .Location = "Support\MP4Box",
        .WebURL = "http://gpac.wp.mines-telecom.fr",
        .HelpURL = "http://gpac.wp.mines-telecom.fr/mp4box/mp4box-documentation",
        .DownloadURL = "https://www.mediafire.com/folder/vkt2ckzjvt0qf/StaxRip_Tools",
        .HelpSwitch = "-h",
        .Description = "MP4Box is a MP4 muxing and demuxing console app."})

    Shared Property AviSynth As Package = Add(New Package With {
        .Name = "AviSynth",
        .Filename = "AviSynth.dll",
        .VersionAllowOld = False,
        .AllowCustomPath = False,
        .WebURL = "https://github.com/AviSynth/AviSynthPlus",
        .HelpURL = "http://avisynth.nl",
        .DownloadURL = "https://gitlab.com/uvz/AviSynthPlus-Builds/-/tree/main",
        .Description = "Video processing scripting library.",
        .Exclude = {"_arm64", "_xp", ".exe"},
        .HintDirFunc = Function() Package.AviSynth.GetAviSynthHintDir,
        .RequiredFunc = Function() p.Script.IsAviSynth})

    Shared Property VapourSynth As Package = Add(New Package With {
        .Name = "VapourSynth",
        .Filename = "vapoursynth.dll",
        .Description = "Video processing Python scripting library.",
        .WebURL = "http://www.vapoursynth.com",
        .HelpURL = "http://www.vapoursynth.com/doc",
        .DownloadURL = "https://github.com/vapoursynth/vapoursynth/releases",
        .SupportsAutoUpdate = False,
        .HelpFilename = "doc\index.html",
        .Siblings = {"vspipe"},
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
        .SupportsAutoUpdate = False,
        .Siblings = {"VapourSynth"},
        .RequiredFunc = Function() p.Script.Engine = ScriptEngine.VapourSynth,
        .HintDirFunc = Function() Package.VapourSynth.Path.Dir})

    Shared Property Python As Package = Add(New Package With {
        .Name = "Python",
        .Filename = "python.exe",
        .TreePath = "Runtimes",
        .WebURL = "http://www.python.org",
        .HelpSwitch = "-h",
        .Description = "Scripting language used by VapourSynth.",
        .Exclude = {"\WindowsApps\"},
        .RequiredFunc = Function() p.Script.Engine = ScriptEngine.VapourSynth,
        .HintDirFunc = AddressOf GetPythonHintDir})

    Shared Property chapterEditor As Package = Add(New Package With {
        .Name = "chapterEditor",
        .Location = "Support\chapterEditor",
        .Filename = "chapterEditor.exe",
        .Description = "GUI app to edit chapters and menus for OGG, XML, TTXT, m.AVCHD, m.editions-mkv, Matroska Menu.",
        .Exclude = {"-Linux", "CLI"},
        .WebURL = "https://forum.doom9.org/showthread.php?t=169984",
        .DownloadURL = "https://www.videohelp.com/software/chapterEditor"})

    Shared Property SevenZip As Package = Add(New Package With {
        .Name = "7zip",
        .Location = "Support\7zip",
        .Filename = "7za.exe",
        .Description = "Packing console app.",
        .WebURL = "https://www.7-zip.org",
        .HelpSwitch = "",
        .SupportsAutoUpdate = False,
        .DownloadURL = "https://www.7-zip.org/download.html"})

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

    Shared Property VisualCpp2010 As Package = Add(New Package With {
        .Name = "Visual C++ 2010",
        .Filename = "msvcp100.dll",
        .Description = "Visual C++ 2010 Redistributable.",
        .DownloadURL = "https://www.microsoft.com/en-us/download/details.aspx?id=14632",
        .Locations = {Folder.System, "Support\VC"},
        .VersionAllowAny = True,
        .TreePath = "Runtimes"})

    Shared Property VisualCpp2012 As Package = Add(New Package With {
        .Name = "Visual C++ 2012",
        .Filename = "msvcp110.dll",
        .Description = "Visual C++ 2012 Redistributable.",
        .DownloadURL = "http://www.microsoft.com/en-US/download/details.aspx?id=30679",
        .Locations = {Folder.System, "Support\VC"},
        .VersionAllowAny = True,
        .TreePath = "Runtimes"})

    Shared Property VisualCpp2013 As Package = Add(New Package With {
        .Name = "Visual C++ 2013",
        .Filename = "msvcp120.dll",
        .Description = "Visual C++ 2013 Redistributable.",
        .DownloadURL = "http://www.microsoft.com/en-US/download/details.aspx?id=40784",
        .VersionAllowAny = True,
        .Locations = {Folder.System, "Support\VC"},
        .TreePath = "Runtimes"})

    Shared Property VisualCpp2022 As Package = Add(New Package With {
        .Name = "Visual C++ 2015-2022",
        .Filename = "msvcp140.dll",
        .Description = "Visual C++ 2015-2022 Redistributable.",
        .DownloadURL = "https://support.microsoft.com/en-gb/help/2977003/the-latest-supported-visual-c-downloads",
        .VersionAllowAny = True,
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
        .VsFilterNames = {"TemporalSoften"}})

    Shared Property AVSMeter As Package = Add(New Package With {
        .Name = "AVSMeter",
        .Location = "Support\AVSMeter",
        .Filename = "AVSMeter64.exe",
        .Filename32 = "AVSMeter.exe",
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

    Shared Property SubtitleEdit As Package = Add(New Package With {
        .Name = "Subtitle Edit",
        .Filename = "SubtitleEdit.exe",
        .Location = "Support\SubtitleEdit",
        .Exclude = {"-Setup.zip", "FI.zip", "PL.zip"},
        .WebURL = "http://www.nikse.dk/SubtitleEdit",
        .HelpURL = "http://www.nikse.dk/SubtitleEdit/Help",
        .DownloadURL = "https://github.com/SubtitleEdit/subtitleedit/releases",
        .Description = "Subtitle editor GUI app."})

    Shared Property mpvnet As Package = Add(New Package With {
        .Name = "mpv.net",
        .Filename = "mpvnet.exe",
        .Location = "Support\mpv.net",
        .WebURL = "https://github.com/stax76/mpv.net",
        .DownloadURL = "https://github.com/stax76/mpv.net#download",
        .Description = "The worlds best media player (GUI app)."})

    Shared Property MPC As Package = Add(New Package With {
        .Name = "MPC",
        .Filename = "mpc-be64.exe",
        .Filename32 = "mpc-be.exe",
        .Filter = "|mpc-hc64.exe;mpc-be64.exe|All Files|*.*",
        .IsIncluded = False,
        .VersionAllowAny = True,
        .Required = False,
        .WebURL = "https://sourceforge.net/projects/mpcbe",
        .Description = "DirectShow based media player (GUI app).",
        .Locations = {Registry.LocalMachine.GetString("SOFTWARE\MPC-BE", "ExePath").Dir, Folder.Programs + "MPC-BE x64"}})

    Shared Property modPlus As Package = Add(New PluginPackage With {
        .Name = "modPlus",
        .Filename = "modPlus.dll",
        .WebURL = "http://www.avisynth.nl/users/vcmohan/modPlus/modPlus.html",
        .Description = "Video filter plugin which modify values of color components to attenuate noise, blur or equalize input.",
        .AvsFilterNames = {"GBlur", "MBlur", "Median", "minvar", "Morph", "SaltPepper", "SegAmp", "TweakHist", "Veed"}})

    Shared Property checkmate As Package = Add(New PluginPackage With {
        .Name = "checkmate",
        .Filename = "checkmate.dll",
        .WebURL = "https://github.com/tp7/checkmate",
        .HelpURL = "http://avisynth.nl/index.php/Checkmate",
        .Description = "A spatial and temporal dot crawl reducer. Checkmate is most effective in static or low motion scenes.",
        .AvsFilterNames = {"checkmate"}})

    Shared Property MedianBlur2 As Package = Add(New PluginPackage With {
        .Name = "MedianBlur2",
        .Filename = "MedianBlur2.dll",
        .WebURL = "http://avisynth.nl/index.php/MedianBlur2",
        .DownloadURL = "https://github.com/pinterf/MedianBlur2/releases",
        .Description = "Constant time median video filter plugin.",
        .AvsFilterNames = {"MedianBlur", "MedianBlurTemporal"}})

    Shared Property AutoAdjust As Package = Add(New PluginPackage With {
        .Name = "AutoAdjust",
        .Filename = "AutoAdjust.dll",
        .WebURL = "http://forum.doom9.org/showthread.php?t=167573",
        .Description = "Automatic adjustment video filter plugin that calculates statistics of clip, stabilizes them temporally and uses them to adjust luminance gain & color balance.",
        .AvsFilterNames = {"AutoAdjust"}})

    Shared Property SmoothAdjust As Package = Add(New PluginPackage With {
        .Name = "SmoothAdjust",
        .Filename = "SmoothAdjust.dll",
        .WebURL = "http://forum.doom9.org/showthread.php?t=154971",
        .Description = "SmoothAdjust is a video filter plugin to make YUV adjustments.",
        .AvsFilterNames = {"SmoothTweak", "SmoothCurve", "SmoothCustom", "SmoothTools"}})

    Shared Property EEDI3 As Package = Add(New PluginPackage With {
        .Name = "EEDI3",
        .Filename = "eedi3.dll",
        .WebURL = "http://avisynth.nl/index.php/EEDI3",
        .DownloadURL = "https://github.com/pinterf/EEDI3/releases",
        .Description = "EEDI3 (Enhanced Edge Directed Interpolation) resizes an image by 2x in the vertical direction by copying the existing image to 2*y(n) and interpolating the missing field.",
        .AvsFilterNames = {"eedi3", "eedi3_rpow2"}})

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
        .DownloadURL = "https://gitlab.com/uvz/AviSynthPlus-Plugins-Scripts/-/tree/master/plugins64%2B",
        .Description = "Simple debanding filter that can be quite effective for some anime sources. Included is an independent build.",
        .VsFilterNames = {"f3kdb.Deband"},
        .AvsFilterNames = {"f3kdb"}})

    Shared Property f3kdb_neo As Package = Add(New PluginPackage With {
        .Name = "f3kdb Neo",
        .Filename = "neo-f3kdb.dll",
        .WebURL = "https://github.com/HomeOfAviSynthPlusEvolution/neo_f3kdb",
        .HelpURL = "http://f3kdb.readthedocs.io/en/latest/#",
        .DownloadURL = "https://gitlab.com/uvz/AviSynthPlus-Plugins-Scripts/-/tree/master/plugins64%2B",
        .Description = "Debanding filter forked from flash3kyuu_deband. Included is an independent build.",
        .AvsFilterNames = {"neo_f3kdb"},
        .VsFilterNames = {"neo_f3kdb.Deband"}})

    Shared Property fft3d_neo As Package = Add(New PluginPackage With {
        .Name = "FFT3D Neo",
        .Filename = "neo-fft3d.dll",
        .WebURL = "https://github.com/HomeOfAviSynthPlusEvolution/neo_FFT3D",
        .DownloadURL = "https://github.com/HomeOfAviSynthPlusEvolution/neo_FFT3D/releases",
        .Description = "Neo FFT3D is a 3D Frequency Domain filter - strong denoiser and moderate sharpener.",
        .AvsFilterNames = {"neo_fft3d"},
        .VsFilterNames = {"neo_fft3d.FFT3D"}})

    Shared Property vinverse As Package = Add(New PluginPackage With {
        .Name = "vinverse",
        .Filename = "vinverse.dll",
        .WebURL = "http://avisynth.nl/index.php/Vinverse",
        .Description = "Simple but effective plugin to remove residual combing.",
        .AvsFilterNames = {"vinverse", "vinverse2"}})

    Shared Property scenechange As Package = Add(New PluginPackage With {
        .Name = "scenechange",
        .Filename = "scenechange.dll",
        .VsFilterNames = {"scenechange"}})

    Shared Property avs2pipemod As Package = Add(New Package With {
        .Name = "avs2pipemod",
        .Filename = "avs2pipemod64.exe",
        .Filename32 = "avs2pipemod.exe",
        .Location = "Support\avs2pipemod",
        .WebURL = "http://github.com/chikuzen/avs2pipemod",
        .DownloadURL = "https://github.com/chikuzen/avs2pipemod/releases",
        .HelpSwitch = "stderr",
        .Description = "Console app given an AviSynth script as input, avs2pipemod can send video, audio, or information of various types to stdout for consumption by command line encoders or other tools."})

    Shared Property x264 As Package = Add(New Package With {
        .Name = "x264",
        .Filename = "x264.exe",
        .Locations = {"Encoders\x264", "FrameServer\AviSynth"},
        .Description = "H.264 video encoding console app.",
        .SupportsAutoUpdate = False,
        .AllowCustomPath = False,
        .WebURL = "https://github.com/staxrip/staxrip/wiki/x264",
        .DownloadURL = "https://github.com/staxrip/staxrip/wiki/x264",
        .HelpURL = "http://www.chaneru.com/Roku/HLS/X264_Settings.htm",
        .HelpSwitch = "--fullhelp"})

    Shared Property x265 As Package = Add(New Package With {
        .Name = "x265",
        .Filename = "x265.exe",
        .Locations = {"Encoders\x265", "FrameServer\AviSynth"},
        .WebURL = "https://x265.com",
        .HelpURL = "http://x265.readthedocs.org",
        .DownloadURL = "https://github.com/staxrip/staxrip/wiki/x265",
        .AllowCustomPath = False,
        .SupportsAutoUpdate = False,
        .HelpSwitch = "--fullhelp",
        .Description = "H.265 video encoding console app."})

    Shared Property VVenCFFapp As Package = Add(New Package With {
        .Name = "vvencFFapp",
        .Filename = "vvencFFapp.exe",
        .Location = "Encoders\vvencFFapp",
        .Description = "The Fraunhofer Versatile Video Encoder (VVenC) is a fast and efficient H.266/VVC encoder implementation.",
        .HelpSwitch = "--fullhelp",
        .AllowCustomPath = False,
        .RequiredFunc = Function() TypeOf p.VideoEncoder Is VvencffappEnc,
        .SupportsAutoUpdate = False,
        .WebURL = "https://github.com/fraunhoferhhi/vvenc",
        .DownloadURL = "https://github.com/f11894/VVenC_Build_Actions/releases"})

    Shared Property SvtAv1EncApp As Package = Add(New Package With {
        .Name = "SvtAv1EncApp",
        .Location = "Encoders\SvtAv1EncApp",
        .Filename = "SvtAv1EncApp.exe",
        .WebURL = "https://gitlab.com/AOMediaCodec/SVT-AV1",
        .HelpURL = "https://gitlab.com/AOMediaCodec/SVT-AV1/-/blob/master/Docs/Parameters.md",
        .DownloadURL = "https://www.mediafire.com/folder/vkt2ckzjvt0qf/StaxRip_Tools",
        .RequiredFunc = Function() TypeOf p.VideoEncoder Is SvtAv1Enc,
        .HelpSwitch = "--help",
        .Description = "Intel AV1 encoder."})

    Shared Property Rav1e As Package = Add(New Package With {
        .Name = "rav1e",
        .Filename = "rav1e.exe",
        .Location = "Encoders\Rav1e",
        .Description = "AV1 Video Encoder.",
        .WebURL = "https://github.com/xiph/rav1e",
        .DownloadURL = "https://github.com/xiph/rav1e/releases",
        .RequiredFunc = Function() TypeOf p.VideoEncoder Is Rav1e,
        .HelpSwitch = "--help"})

    Shared Property AOMEnc As Package = Add(New Package With {
        .Name = "AOMEnc",
        .Filename = "aomenc.exe",
        .Location = "Encoders\AOMEnc",
        .Description = "AV1 video encoder console app.",
        .WebURL = "https://aomedia.org",
        .DownloadURL = "https://github.com/staxrip/staxrip/wiki/aomenc",
        .RequiredFunc = Function() TypeOf p.VideoEncoder Is AOMEnc,
        .HelpSwitch = "--help"})

    Shared Property mkvmerge As Package = Add(New Package With {
        .Name = "mkvmerge",
        .Filename = "mkvmerge.exe",
        .Location = "Support\MKVToolNix",
        .WebURL = "https://mkvtoolnix.download",
        .HelpURL = "https://mkvtoolnix.download/docs.html",
        .DownloadURL = "https://www.videohelp.com/software/MKVToolNix",
        .HelpSwitch = "",
        .Siblings = {"mkvextract", "mkvinfo", "MKVToolnix GUI"},
        .Exclude = {"-setup"},
        .Description = "MKV muxing tool."})

    Shared Property mkvextract As Package = Add(New Package With {
        .Name = "mkvextract",
        .Filename = "mkvextract.exe",
        .Location = "Support\MKVToolNix",
        .WebURL = "https://mkvtoolnix.download",
        .HelpURL = "https://mkvtoolnix.download/docs.html",
        .DownloadURL = "https://www.videohelp.com/software/MKVToolNix",
        .HelpSwitch = "",
        .Exclude = {"-setup"},
        .Siblings = {"mkvinfo", "MKVToolnix GUI", "mkvmerge"},
        .Description = "MKV demuxing tool."})

    Shared Property mkvinfo As Package = Add(New Package With {
        .Name = "mkvinfo",
        .Filename = "mkvinfo.exe",
        .Location = "Support\MKVToolNix",
        .WebURL = "https://mkvtoolnix.download",
        .HelpURL = "https://mkvtoolnix.download/docs.html",
        .DownloadURL = "https://www.videohelp.com/software/MKVToolNix",
        .HelpSwitch = "",
        .Exclude = {"-setup"},
        .Siblings = {"mkvextract", "MKVToolnix GUI", "mkvmerge"},
        .Description = "MKV info tool."})

    Shared Property MKVToolnixGUI As Package = Add(New Package With {
        .Name = "MKVToolnix GUI",
        .Filename = "mkvtoolnix-gui.exe",
        .Location = "Support\MKVToolNix",
        .Siblings = {"mkvextract", "mkvinfo", "mkvmerge"},
        .Exclude = {"-setup"},
        .WebURL = "https://mkvtoolnix.download",
        .HelpURL = "https://mkvtoolnix.download/docs.html",
        .DownloadURL = "https://www.videohelp.com/software/MKVToolNix",
        .Description = "MKV muxing/demuxing GUI app."})

    Shared Property AutoCrop As Package = Add(New Package With {
        .Name = "AutoCrop",
        .Filename = "AutoCrop.exe",
        .Location = "Support\AutoCrop",
        .HelpSwitch = "",
        .Description = "AutoCrop console app."})

    Shared Property Err As Package = Add(New Package With {
        .Name = "Err",
        .Filename = "Err.exe",
        .Location = "Support\Err",
        .HelpSwitch = "",
        .Description = "Provides Windows error code info."})

    Shared Property PNGopt As Package = Add(New Package With {
        .Name = "PNGopt",
        .Filename = "apngopt.exe",
        .HelpFilename = "help.txt",
        .Location = "Thumbnails\PNGopt",
        .WebURL = "https://sourceforge.net/projects/apng/files",
        .HelpSwitch = "",
        .Description = "Opt Tools For Creating PNG"})

    Shared Property NVEncC As Package = Add(New Package With {
        .Name = "NVEncC",
        .Filename = "NVEncC64.exe",
        .Filename32 = "NVEncC.exe",
        .Location = "Encoders\NVEncC",
        .HelpSwitch = "-h",
        .WebURL = "http://github.com/rigaya/NVEnc",
        .HelpURL = "https://github.com/rigaya/NVEnc/blob/master/NVEncC_Options.en.md",
        .DownloadURL = "https://github.com/rigaya/NVEnc/releases",
        .Description = "NVIDIA hardware video encoder."})

    Shared Property QSVEncC As Package = Add(New Package With {
        .Name = "QSVEncC",
        .Filename = "QSVEncC64.exe",
        .Filename32 = "QSVEncC.exe",
        .Location = "Encoders\QSVEncC",
        .Description = "Intel hardware video encoder.",
        .WebURL = "http://github.com/rigaya/QSVEnc",
        .DownloadURL = "https://github.com/rigaya/QSVEnc/releases",
        .HelpURL = "https://github.com/rigaya/QSVEnc/blob/master/QSVEncC_Options.en.md",
        .HelpSwitch = "-h"})

    Shared Property VCEEncC As Package = Add(New Package With {
        .Name = "VCEEncC",
        .Filename = "VCEEncC64.exe",
        .Filename32 = "VCEEncC.exe",
        .Location = "Encoders\VCEEncC",
        .Description = "AMD hardware video encoder.",
        .HelpSwitch = "-h",
        .WebURL = "http://github.com/rigaya/VCEEnc",
        .DownloadURL = "https://github.com/rigaya/VCEEnc/releases"})

    Shared Property FFT3DFilter As Package = Add(New PluginPackage With {
        .Name = "FFT3DFilter",
        .Filename = "fft3dfilter.dll",
        .WebURL = "http://github.com/pinterf/fft3dfilter",
        .DownloadURL = "https://github.com/pinterf/fft3dfilter/releases",
        .Description = "FFT3DFilter uses Fast Fourier Transform method for image processing in frequency domain.",
        .AvsFilterNames = {"FFT3DFilter"}})

    Shared Property ffms2 As Package = Add(New PluginPackage With {
        .Name = "ffms2",
        .Filename = "ffms2.dll",
        .WebURL = "http://github.com/FFMS/ffms2",
        .HelpURL = "http://github.com/FFMS/ffms2/blob/master/doc/ffms2-avisynth.md",
        .Description = "AviSynth+ and VapourSynth source filter supporting various input formats.",
        .AvsFilterNames = {"FFVideoSource", "FFAudioSource", "FFMS2"},
        .VsFilterNames = {"ffms2"}})

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
        .WebURL = "https://github.com/kedaitinh12/VSFilterMod",
        .DownloadURL = "https://github.com/kedaitinh12/VSFilterMod/releases",
        .AvsFilterNames = {"VobSub", "TextSubMod"},
        .VsFilterNames = {"vsfm.VobSub", "vsfm.TextSubMod"}})

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
        .VsFilterNames = {"dctf.DCTFilter"}})

    Shared Property DCTFilterVS As Package = Add(New PluginPackage With {
        .Name = "DCTFilter",
        .Filename = "DCTFilter.dll",
        .Location = "Plugins\VS\DCTFilter",
        .Description = "Renewed VapourSynth port of DCTFilter.",
        .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DCTFilter",
        .VsFilterNames = {"dctf.DCTFilter"}})

    Shared Property RemoveGrainVS As Package = Add(New PluginPackage With {
        .Name = "RemoveGrain",
        .Filename = "RemoveGrainVS.dll",
        .Location = "Plugins\VS\RemoveGrainVS",
        .Description = "VapourSynth port of RemoveGrain and Repair plugins from Avisynth.",
        .WebURL = "https://github.com/vapoursynth/vs-removegrain",
        .VsFilterNames = {"rgvs.RemoveGrain", "rgvs.BackwardClense", "rgvs.Clense", "rgvs.ForwardClense", "rgvs.Repair", "rgvs.VerticalCleaner"}})

    Shared Property LibP2P As Package = Add(New PluginPackage With {
        .Name = "LibP2P",
        .Filename = "LibP2P.dll",
        .Location = "Plugins\VS\LibP2P",
        .Description = "Vapoursynth plugin for packing/unpacking of RGB clips.",
        .WebURL = "https://github.com/DJATOM/LibP2P-Vapoursynth",
        .VsFilterNames = {"libp2p.pack", "libp2p.unpack"}})

    Shared Property FFTW As Package = Add(New Package With {
        .Name = "FFTW",
        .Location = "Support\FFTW",
        .Filename = "libfftw3-3.dll",
        .Description = "Library required by various AviSynth and VapourSynth plugins.",
        .WebURL = "http://www.fftw.org"})

    Shared Property havsfunc As Package = Add(New PluginPackage With {
        .Name = "havsfunc",
        .Filename = "havsfunc.py",
        .Location = "Plugins\VS\Scripts",
        .WebURL = "http://github.com/HomeOfVapourSynthEvolution/havsfunc",
        .DownloadURL = "https://github.com/HomeOfVapourSynthEvolution/havsfunc/releases",
        .HelpURL = "http://forum.doom9.org/showthread.php?t=166582",
        .Description = "Various popular AviSynth scripts ported To VapourSynth.",
        .Dependencies = {"DCTFilter.dll", "libfftw3f-3.dll", "RemoveGrainVS.dll"},
        .VsFilterNames = {"havsfunc.aaf", "havsfunc.AverageFrames", "havsfunc.Bob", "havsfunc.ChangeFPS", "havsfunc.Clamp", "havsfunc.ContraSharpening", "havsfunc.daa", "havsfunc.Deblock_QED", "havsfunc.DeHalo_alpha", "havsfunc.DitherLumaRebuild", "havsfunc.EdgeCleaner", "havsfunc.FastLineDarkenMOD", "havsfunc.FineDehalo", "havsfunc.FixChromaBleedingMod", "havsfunc.GrainFactory3", "havsfunc.GrainStabilizeMC", "havsfunc.HQDeringmod", "havsfunc.InterFrame", "havsfunc.ivtc_txt60mc", "havsfunc.KNLMeansCL", "havsfunc.logoNR", "havsfunc.LSFmod", "havsfunc.LUTDeCrawl", "havsfunc.LUTDeRainbow", "havsfunc.MCTemporalDenoise", "havsfunc.MinBlur", "havsfunc.mt_deflate_multi", "havsfunc.mt_expand_multi", "havsfunc.mt_inflate_multi", "havsfunc.mt_inpand_multi", "havsfunc.Overlay", "havsfunc.Padding", "havsfunc.QTGMC", "havsfunc.Resize", "havsfunc.santiag", "havsfunc.sbr", "havsfunc.SCDetect", "havsfunc.SigmoidDirect", "havsfunc.SigmoidInverse", "havsfunc.smartfademod", "havsfunc.SMDegrain", "havsfunc.SmoothLevels", "havsfunc.srestore", "havsfunc.Stab", "havsfunc.STPresso", "havsfunc.TemporalDegrain", "havsfunc.Toon", "havsfunc.Vinverse", "havsfunc.Vinverse2", "havsfunc.Weave", "havsfunc.YAHR"}})

    Shared Property DescaleAVS As Package = Add(New PluginPackage With {
        .Name = "Descale",
        .Filename = "libdescale.dll",
        .Location = "Plugins\AVS\Descale",
        .WebURL = "https://github.com/Irrational-Encoding-Wizardry/descale",
        .DownloadURL = "https://github.com/Irrational-Encoding-Wizardry/descale/releases",
        .Description = "Video/Image filter to undo upscaling.",
        .AvsFilterNames = {"Debilinear", "Debicubic", "Delanczos", "Despline16", "Despline36", "Despline64", "Descale"}})

    Shared Property QTGMC As Package = Add(New PluginPackage With {
        .Name = "QTGMC",
        .Filename = "QTGMC.avsi",
        .WebURL = "http://avisynth.nl/index.php/QTGMC",
        .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.6%20and%20up/QTGMC.avsi",
        .Description = "A very high quality deinterlacer with a range of features for both quality and convenience. These include a simple presets system, extensive noise processing capabilities, support for repair of progressive material, precision source matching, shutter speed simulation, etc. Originally based on TempGaussMC by Didée.",
        .AvsFilterNames = {"QTGMC"}})

    Shared Property SMDegrain As Package = Add(New PluginPackage With {
        .Name = "SMDegrain",
        .Filename = "SMDegrain.avsi",
        .WebURL = "https://raw.githack.com/Dogway/Avisynth-Scripts/master/SMDegrain/SMDegrain.html",
        .DownloadURL = "https://github.com/Dogway/Avisynth-Scripts/blob/master/SMDegrain/SMDegrain.avsi",
        .Description = "SMDegrain is a simple wrapper for MVTools and client functions for general purpose temporal denoising.",
        .Dependencies = {"Average.dll", "dfttest.dll", "ExTools.avsi", "GradePack.avsi", "KNLMeansCL.dll", "LSFplus.avsi", "MasksPack.avsi", "MedianBlur2.dll", "mvtools2.dll", "nnedi3cl.dll", "ResizersPack.avsi", "RgTools.dll", "SharpenersPack.avsi", "vsTTempSmooth.dll"},
        .AvsFilterNames = {"SMDegrain"}})

    Shared Property BBorders As Package = Add(New PluginPackage With {
        .Name = "BBorders",
        .Filename = "bborders.avsi",
        .Location = "Plugins\AVS\Scripts",
        .WebURL = "https://github.com/Asd-g/AviSynthPlus-Scripts/blob/master/bborders.avsi",
        .DownloadURL = "https://github.com/Asd-g/AviSynthPlus-Scripts/blob/master/bborders.avsi",
        .Description = "bbmod is a balanceborders mod that uses division instead subtraction for the filtering.",
        .Dependencies = {"avsresize.dll"},
        .AvsFilterNames = {"BalanceBorders", "bbmod", "bb_process", "BalanceTopBorder", "btb", "chroma_shift_"}})

    Shared Property ExTools As Package = Add(New PluginPackage With {
        .Name = "ExTools",
        .Filename = "ExTools.avsi",
        .Location = "Plugins\AVS\Scripts",
        .WebURL = "https://github.com/Dogway/Avisynth-Scripts/blob/master/ExTools.avsi",
        .DownloadURL = "https://github.com/Dogway/Avisynth-Scripts/blob/master/ExTools.avsi",
        .Description = "Pack of masktools2/removegrain replacement functions with internal Expr() and many extra features.",
        .AvsFilterNames = {"ex_bs"}})

    Shared Property GradePack As Package = Add(New PluginPackage With {
        .Name = "GradePack",
        .Filename = "GradePack.avsi",
        .Location = "Plugins\AVS\Scripts",
        .WebURL = "https://github.com/Dogway/Avisynth-Scripts/blob/master/MasksPack.avsi",
        .DownloadURL = "https://github.com/Dogway/Avisynth-Scripts/blob/master/MasksPack.avsi",
        .Description = "",
        .Dependencies = {"ExTools.avsi", "MasksPack.avsi", "ResizersPack.avsi", "TransformsPack - Main.avsi"},
        .AvsFilterNames = {"ex_vibrance", "ex_contrast", "ex_levels", "ex_autolevels", "ex_blend", "ex_glow", "ex_posterize", "Skin_Qualifier", "HSVxHSV", "GamutWarning", "PseudoColor", "Vignette", "greyscale_rgb", "FindTemp", "GreyWorld", "WhitePoint", "GradientLinear"}})

    Shared Property MasksPack As Package = Add(New PluginPackage With {
        .Name = "MasksPack",
        .Filename = "MasksPack.avsi",
        .Location = "Plugins\AVS\Scripts",
        .WebURL = "",
        .DownloadURL = "https://github.com/Dogway/Avisynth-Scripts/blob/master/MasksPack.avsi",
        .Description = "",
        .Dependencies = {"ExTools.avsi", "ResizersPack.avsi", "GradePack.avsi"},
        .AvsFilterNames = {"BoxMask", "FlatMask", "LumaMask", "CornerMask", "MotionMask", "ex_limitdif", "ex_limitchange", "Soothe"}})

    Shared Property ResizersPack As Package = Add(New PluginPackage With {
        .Name = "ResizersPack",
        .Filename = "ResizersPack.avsi",
        .Location = "Plugins\AVS\Scripts",
        .WebURL = "",
        .DownloadURL = "https://github.com/Dogway/Avisynth-Scripts/blob/master/ResizersPack.avsi",
        .Description = "",
        .Dependencies = {"ExTools.avsi"},
        .AvsFilterNames = {"deep_resize"}})

    Shared Property ScenesPack As Package = Add(New PluginPackage With {
        .Name = "ScenesPack",
        .Filename = "ScenesPack.avsi",
        .Location = "Plugins\AVS\Scripts",
        .WebURL = "",
        .DownloadURL = "https://github.com/Dogway/Avisynth-Scripts/blob/master/ScenesPack.avsi",
        .Description = "SceneStats does a robust Scene Change Detection and calculates scene's statistics.",
        .Dependencies = {"ExTools.avsi", "TransformsPack - Models.avsi", "TransformsPack - Transfers.avsi"},
        .AvsFilterNames = {"ClipStats", "ReadStats", "SceneStats"}})

    Shared Property SharpenersPack As Package = Add(New PluginPackage With {
        .Name = "SharpenersPack",
        .Filename = "SharpenersPack.avsi",
        .Location = "Plugins\AVS\Scripts",
        .WebURL = "",
        .DownloadURL = "https://github.com/Dogway/Avisynth-Scripts/blob/master/MIX%20mods/SharpenersPack.avsi",
        .Description = "Collection of high quality sharpeners for AviSynth+.",
        .Dependencies = {"ExTools.avsi", "ResizersPack.avsi", "RgTools.dll"},
        .AvsFilterNames = {"Adaptive_Sharpen", "blah", "CASm", "CASP", "DelicateSharp", "DetailSharpen", "DGSharpen2", "ex_ContraSharpening", "ex_sharpen22", "ex_SootheSS2", "ex_unsharp", "FineSharpPlus", "halomaskM", "LindaSharp", "MedianSharp", "MedSharp", "MultiSharpen2", "NonlinUSM", "NVSharpen", "pSharpen", "ReCon", "RSharpen", "SeeSaw", "SeeSaw2", "SeeSawMulti", "SharpenComplex2", "SlopeBend", "SSSharp", "SSSharpEX", "SSSharpFaster", "SSW", "TblurNL", "TMed2", "XSharpenPlus"}})

    Shared Property TransformsPack As Package = Add(New PluginPackage With {
        .Name = "TransformsPack",
        .Filename = "TransformsPack - Main.avsi",
        .Location = "Plugins\AVS\Scripts",
        .WebURL = "",
        .DownloadURL = "https://github.com/Dogway/Avisynth-Scripts/blob/master/TransformsPack%20-%20Main.avsi",
        .Description = "",
        .Siblings = {"TransformsPackModels", "TransformsPackTransfers"},
        .Dependencies = {"TransformsPack - Models.avsi", "TransformsPack - Transfers.avsi"},
        .AvsFilterNames = {"Display_Referred", "MatchClip", "ConvertFormat", "color_Fuzzy_Search", "color_propGet", "format_Fuzzy_Search", "bicubic_coeffs", "chroma_placement", "color_coef", "moncurve_coef", "ExtractClip", "MatrixClip", "DotClip", "DotClipA", "MatrixDot", "MatrixInvert", "MatrixTranspose", "Broadcast", "Cross", "Dot"}})

    Shared Property TransformsPackModels As Package = Add(New PluginPackage With {
        .Name = "TransformsPackModels",
        .Filename = "TransformsPack - Models.avsi",
        .Location = "Plugins\AVS\Scripts",
        .WebURL = "",
        .DownloadURL = "https://github.com/Dogway/Avisynth-Scripts/blob/master/TransformsPack%20-%20Models.avsi",
        .Description = "",
        .Siblings = {"TransformsPack avs", "TransformsPackTransfers"},
        .Dependencies = {},
        .AvsFilterNames = {}})

    Shared Property TransformsPackTransfers As Package = Add(New PluginPackage With {
        .Name = "TransformsPackTransfers",
        .Filename = "TransformsPack - Transfers.avsi",
        .Location = "Plugins\AVS\Scripts",
        .WebURL = "",
        .DownloadURL = "https://github.com/Dogway/Avisynth-Scripts/blob/master/TransformsPack%20-%20Transfers.avsi",
        .Description = "",
        .Siblings = {"TransformsPack avs", "TransformsPackModels"},
        .Dependencies = {},
        .AvsFilterNames = {}})

    Shared Property Zs_RF_Shared As Package = Add(New PluginPackage With {
        .Name = "Zs_RF_Shared",
        .Filename = "Zs_RF_Shared.avsi",
        .WebURL = "http://avisynth.nl/index.php/Zs_RF_Shared",
        .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.5%20and%20up/Zs_RF_Shared.avsi",
        .Description = "Shared Functions and utility.",
        .AvsFilterNames = {"sh_StrReplace", "sAverageLumaMask", "m4_sh", "sh_sharpen2", "sh_SootheSS", "slinesm", "t_linemask", "swlinesm", "GreyCenteredToMask_dhh", "Camembert_dhh", "Camembert_dhhMod", "EMask_dhh", "spasses", "spasses2", "sspasses", "sminideen", "DarkPreserve_function", "lightPreserve_function", "sh_Padding", "sTryFTurnRight", "sTryFTurnLeft", "sTryFTurn180", "slimit_dif", "slimit_dif2", "sh_GetCSP", "sh_Y8_YV411", "ContraSharpening", "MinBlur", "sbr", "Dither_YUY2toPlanar16", "Dither_YUY2toPlanar", "Dither_YUY2toInterleaved16", "Dither_YUY2toInterleaved", "Dither_Luma_Rebuild", "SH_KNLMeansCL", "sh_RemoveGrain", "y_gamma_to_linear", "y_linear_to_gamma", "linear_and_gamma", "build_sigmoid_expr", "sigmoid_direct", "sigmoid_inverse", "sh_Vinverse2", "sbrV", "sh_Vinverse2H", "sbrH", "sBlackerPixel", "swhiterPixel", "chroma_rep", "chroma_rep2", "smartfademod", "ediaa", "daa", "maa", "SharpAAMCmod", "VinverseD", "smam", "smam_mask", "Hqdn3d_2", "interlaced60or50", "convert_Fields_scan_order", "yadifmodclipin", "yadifmodclipout", "tmm2_ortmm1", "nonyuy2clipin", "nonyuy2clipout", "sh_LimitChange", "sh_Bob", "sneo_FFT3D", "sneo_dfttest", "svsTTempSmooth", "svsTTempSmoothf", "svsMSharpen", "svsTBilateral", "svsTcanny", "sTcannyMod", "sh_GetUserGlobalIMT", "sh_GetUserGlobalIMTint", "sh_GetUserGlobalIMTbool", "sChromaShift2", "ChrEatWhite", "IsAvsNeo", "IsAvsPlus", "AvsPlusVersionNumber"}})

    Shared Property LSFmod As Package = Add(New PluginPackage With {
        .Name = "LSFmod",
        .Filename = "LSFmod.avsi",
        .WebURL = "http://avisynth.nl/index.php/LSFmod",
        .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/master/avs%202.5%20and%20up/LSFmod.avsi",
        .Description = "A LimitedSharpenFaster mod with a lot of new features and optimizations.",
        .AvsFilterNames = {"LSFmod"}})

    Shared Property LSFplus As Package = Add(New PluginPackage With {
        .Name = "LSFplus",
        .Filename = "LSFplus.avsi",
        .WebURL = "https://github.com/Dogway/Avisynth-Scripts/blob/master/MIX%20mods/LSFplus.avsi",
        .DownloadURL = "https://github.com/Dogway/Avisynth-Scripts/blob/master/MIX%20mods/LSFplus.avsi",
        .Description = "LimitedSharpenFaster+, HBD Performance mod of LSFmod by Dogway.",
        .AvsFilterNames = {"LSFplus"}})

    Shared Property TemporalDegrain2 As Package = Add(New PluginPackage With {
        .Name = "TemporalDegrain2",
        .Filename = "TemporalDegrain2.avsi",
        .WebURL = "http://avisynth.nl/index.php/TemporalDegrain2",
        .Description = "Builds on Temporal Degrain but it is able to clean the noise even further while impoving the sharpness in cases where orignal version had severe drops in visual quality.",
        .AvsFilterNames = {"TemporalDegrain2"}})

    Shared Property NNEDI3CL As Package = Add(New PluginPackage With {
        .Name = "NNEDI3CL",
        .Filename = "nnedi3cl.dll",
        .WebURL = "http://avisynth.nl/index.php/NNEDI3CL",
        .DownloadURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-NNEDI3CL",
        .Description = "nnedi3 is an intra-field only deinterlacer. It takes a frame, throws away one field, and then interpolates the missing pixels using only information from the remaining field. It is also good for enlarging images by powers of two.",
        .AvsFilterNames = {"nnedi3cl"}})

    Shared Property LSmashWorks As Package = Add(New PluginPackage With {
        .Name = "L-SMASH-Works",
        .Filename = "LSMASHSource.dll",
        .Description = "AviSynth and VapourSynth source filter based on Libav supporting a wide range of input formats.",
        .WebURL = "https://github.com/HomeOfAviSynthPlusEvolution/L-SMASH-Works",
        .DownloadURL = "https://github.com/HomeOfAviSynthPlusEvolution/L-SMASH-Works/releases",
        .HelpUrlAviSynth = "https://github.com/HomeOfAviSynthPlusEvolution/L-SMASH-Works/blob/master/AviSynth/README",
        .HelpUrlVapourSynth = "https://github.com/HomeOfAviSynthPlusEvolution/L-SMASH-Works/blob/master/VapourSynth/README",
        .AvsFilterNames = {"LSMASHVideoSource", "LSMASHAudioSource", "LWLibavVideoSource", "LWLibavAudioSource"},
        .VsFilterNames = {"lsmas.LibavSMASHSource", "lsmas.LWLibavSource"}})

    Shared Property BM3D As Package = Add(New PluginPackage With {
        .Name = "BM3D",
        .Filename = "BM3D.dll",
        .VsFilterNames = {"bm3d.RGB2OPP", "bm3d.OPP2RGB", "bm3d.Basic", "bm3d.Final", "bm3d.VBasic", "bm3d.VFinal", "bm3d.VAggregate"},
        .Description = "BM3D denoising filter for VapourSynth.",
        .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-BM3D"})

    Shared Property MCTemporalDenoise As Package = Add(New PluginPackage With {
        .Name = "MCTemporalDenoise",
        .Filename = "MCTemporalDenoise.avsi",
        .Description = "A motion compensated noise removal script with an accompanying post-processing component.",
        .WebURL = "http://avisynth.nl/index.php/MCTemporalDenoise",
        .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.5%20and%20up/MCTemporalDenoise.avsi",
        .Dependencies = {"Zs_RF_Shared.avsi"},
        .AvsFilterNames = {"MCTemporalDenoise", "MCTemporalDenoisePP"}})

    Shared Property xyVSFilter As Package = Add(New PluginPackage With {
        .Name = "xy-VSFilter",
        .Filename = "VSFilter.dll",
        .Description = "AviSynth subtitle plugin.",
        .WebURL = "https://github.com/pinterf/xy-VSFilter",
        .DownloadURL = "https://github.com/pinterf/xy-VSFilter/releases",
        .AvsFilterNames = {"VobSub", "TextSub"}})

    Shared Property DFTTestAVS As Package = Add(New PluginPackage With {
        .Name = "DFTTest",
        .Filename = "dfttest.dll",
        .Description = "2D/3D frequency domain denoiser using Discrete Fourier transform.",
        .HelpFilename = "dfttest - README.txt",
        .WebURL = "https://github.com/pinterf/dfttest",
        .DownloadURL = "https://github.com/pinterf/dfttest/releases",
        .AvsFilterNames = {"dfttest"}})

    Shared Property DescaleVS As Package = Add(New PluginPackage With {
        .Name = "Descale",
        .Filename = "libdescale.dll",
        .Location = "Plugins\VS\Descale",
        .WebURL = "https://github.com/Irrational-Encoding-Wizardry/descale",
        .DownloadURL = "https://github.com/Irrational-Encoding-Wizardry/descale/releases",
        .Description = "Video/Image filter to undo upscaling.",
        .VsFilterNames = {"descale.Debilinear", "descale.Debicubic", "descale.Delanczos", "descale.Despline16", "descale.Despline36", "descale.Despline64", "descale.Descale"}})

    Shared Property InsaneAA As Package = Add(New PluginPackage With {
        .Name = "insaneAA",
        .Filename = "insaneAA.py",
        .Location = "Plugins\VS\insaneAA",
        .WebURL = "https://github.com/Beatrice-Raws/VapourSynth-insaneAA",
        .DownloadURL = "https://github.com/Beatrice-Raws/VapourSynth-insaneAA/releases",
        .Description = "InsaneAA Anti-Aliasing Script (VS port). Original idea by tonik && tophf, edited and ported by DJATOM. Use this script to fix ugly upscaled anime BDs.",
        .Dependencies = {"eedi3.dll", "havsfunc.py", "libdescale.dll", "libnnedi3.dll", "NNEDI3CL.dll", "vsznedi3.dll"},
        .VsFilterNames = {"insaneAA.insaneAA", "insaneAA.revert_upscale", "insaneAA.rescale", "insaneAA.eedi3_instance", "insaneAA.nnedi3_superclip", "insaneAA.validateInput"}})

    Shared Property DeCross As Package = Add(New PluginPackage With {
        .Name = "DeCross",
        .Filename = "libdecross.dll",
        .Location = "Plugins\VS\DeCross",
        .WebURL = "https://github.com/dubhater/vapoursynth-decross",
        .DownloadURL = "https://github.com/dubhater/vapoursynth-decross/releases",
        .Description = "DeCross is a spatio-temporal cross color (rainbow) reduction filter. DeCross must be used right after the source filter, before any field matching or deinterlacing. The luma is returned unchanged. This is a port of the DeCross Avisynth plugin, which is in turn based on the AviUtl plugin CrossColor.",
        .VsFilterNames = {"decross.DeCross"}})

    Shared Property TemporalSoften2 As Package = Add(New PluginPackage With {
        .Name = "TemporalSoften2",
        .Filename = "libtemporalsoften2.dll",
        .Location = "Plugins\VS\TemporalSoften2",
        .WebURL = "https://github.com/dubhater/vapoursynth-temporalsoften2",
        .DownloadURL = "https://github.com/dubhater/vapoursynth-temporalsoften2/releases",
        .Description = "TemporalSoften filter for VapourSynth.",
        .VsFilterNames = {"focus2.TemporalSoften2"}})

    Shared Property MotionMask As Package = Add(New PluginPackage With {
        .Name = "MotionMask",
        .Filename = "libmotionmask.dll",
        .Location = "Plugins\VS\MotionMask",
        .WebURL = "https://github.com/dubhater/vapoursynth-motionmask",
        .DownloadURL = "https://github.com/dubhater/vapoursynth-motionmask/releases",
        .Description = "MotionMask creates a mask of moving pixels. Every output pixel will be set to the absolute difference between the current frame and the previous frame. This is a port of the mt_motion filter from pinterf's updated version of the Avisynth plugin MaskTools.",
        .VsFilterNames = {"motionmask.MotionMask"}})

    Shared Property DFTTest2 As Package = Add(New PluginPackage With {
        .Siblings = {"dfttest2_NVRTC vs", "dfttest2_CUDA vs", "dfttest2_CPU vs"},
        .Name = "dfttest2",
        .Filename = "dfttest2.py",
        .Location = "Plugins\VS\DFTTest2",
        .Description = "DFTTest re-implemetation for VapourSynth (CPU and CUDA).",
        .WebURL = "https://github.com/AmusementClub/vs-dfttest2",
        .DownloadURL = "https://github.com/AmusementClub/vs-dfttest2/releases",
        .Dependencies = {"dfttest2_cpu.dll", "dfttest2_cuda.dll", "dfttest2_nvrtc.dll"},
        .VsFilterNames = {"dfttest2.DFTTest"}})

    Shared Property DFTTest2CPU As Package = Add(New PluginPackage With {
        .Siblings = {"dfttest2 vs", "dfttest2_NVRTC vs", "dfttest2_CUDA vs"},
        .Name = "dfttest2_CPU",
        .Filename = "dfttest2_cpu.dll",
        .Location = "Plugins\VS\DFTTest2",
        .Description = "",
        .WebURL = "https://github.com/AmusementClub/vs-dfttest2",
        .DownloadURL = "https://github.com/AmusementClub/vs-dfttest2/releases",
        .VsFilterNames = {"dfttest2_cpu.RDFT", "dfttest2_cpu.DFTTest"}})

    Shared Property DFTTest2CUDA As Package = Add(New PluginPackage With {
        .Siblings = {"dfttest2 vs", "dfttest2_NVRTC vs", "dfttest2_CPU vs"},
        .Name = "dfttest2_CUDA",
        .Filename = "dfttest2_cuda.dll",
        .Location = "Plugins\VS\DFTTest2",
        .Description = "",
        .WebURL = "https://github.com/AmusementClub/vs-dfttest2",
        .DownloadURL = "https://github.com/AmusementClub/vs-dfttest2/releases",
        .VsFilterNames = {"dfttest2_cuda.RDFT", "dfttest2_cuda.DFTTest"}})

    Shared Property DFTTest2NVRTC As Package = Add(New PluginPackage With {
        .Siblings = {"dfttest2 vs", "dfttest2_CUDA vs", "dfttest2_CPU vs"},
        .Name = "dfttest2_NVRTC",
        .Filename = "dfttest2_nvrtc.dll",
        .Location = "Plugins\VS\DFTTest2",
        .Description = "",
        .WebURL = "https://github.com/AmusementClub/vs-dfttest2",
        .DownloadURL = "https://github.com/AmusementClub/vs-dfttest2/releases",
        .VsFilterNames = {"dfttest2_nvrtc.RDFT", "dfttest2_nvrtc.DFTTest"}})

    Shared Property DFTTestVS As Package = Add(New PluginPackage With {
        .Name = "DFTTest",
        .Filename = "DFTTest.dll",
        .Description = "VapourSynth port of dfttest.",
        .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DFTTest",
        .DownloadURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DFTTest/releases",
        .VsFilterNames = {"dfttest.DFTTest"}})

    Shared Property DFTTestNeo As Package = Add(New PluginPackage With {
        .Name = "DFTTest Neo",
        .Filename = "neo-dfttest.dll",
        .Description = "2D/3D frequency domain denoiser using Discrete Fourier transform.",
        .WebURL = "https://github.com/HomeOfAviSynthPlusEvolution/neo_DFTTest",
        .DownloadURL = "https://github.com/HomeOfAviSynthPlusEvolution/neo_DFTTest/releases",
        .AvsFilterNames = {"neo_dfttest"},
        .VsFilterNames = {"neo_dfttest.DFTTest"}})

    Shared Property muvsfunc As Package = Add(New PluginPackage With {
        .Name = "muvsfunc",
        .Filename = "muvsfunc.py",
        .Location = "Plugins\VS\Scripts",
        .Description = "Muonium's VapourSynth functions.",
        .WebURL = "https://github.com/WolframRhodium/muvsfunc",
        .VsFilterNames = {
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
        .WebURL = "https://github.com/pinterf/FFT3dGPU",
        .DownloadURL = "https://github.com/pinterf/FFT3dGPU/releases",
        .HelpURL = "https://htmlpreview.github.io/?https://github.com/pinterf/FFT3dGPU/blob/master/FFT3dGPU/documentation/fft3dgpu.htm",
        .AvsFilterNames = {"FFT3DGPU"}})

    Shared Property TIVTC As Package = Add(New PluginPackage With {
        .Name = "TIVTC",
        .Filename = "TIVTC.dll",
        .WebURL = "http://github.com/pinterf/TIVTC",
        .DownloadURL = "https://github.com/pinterf/TIVTC/releases",
        .HelpURL = "https://github.com/pinterf/TIVTC/tree/master/Doc_TIVTC",
        .Description = "TIVTC is a plugin package containing 7 different filters and 3 conditional functions.",
        .AvsFilterNames = {"TFM", "TDecimate", "MergeHints", "FrameDiff", "FieldDiff", "ShowCombedTIVTC", "RequestLinear"}})

    Shared Property D2VSourceAVS As Package = Add(New PluginPackage With {
        .Name = "D2VSource",
        .Filename = "D2VSource.dll",
        .WebURL = "https://github.com/Asd-g/MPEG2DecPlus",
        .DownloadURL = "https://github.com/Asd-g/MPEG2DecPlus/releases",
        .Description = "Source filter to open D2V index files created with DGIndex or D2V Witch.",
        .AvsFilterNames = {"D2VSource"}})

    Shared Property d2vsourceVS As Package = Add(New PluginPackage With {
        .Name = "d2vsource",
        .Filename = "d2vsource.dll",
        .Description = "Source filter to open D2V index files created with DGIndex or D2V Witch.",
        .WebURL = "http://github.com/dwbuiten/d2vsource",
        .VsFilterNames = {"d2v.Source"}})

    Shared Sub New()
        Add(New PluginPackage With {
            .Name = "KNLMeansCL",
            .Filename = "KNLMeansCL.dll",
            .WebURL = "https://github.com/pinterf/KNLMeansCL",
            .DownloadURL = "https://github.com/pinterf/KNLMeansCL/releases",
            .HelpURL = "https://github.com/Khanattila/KNLMeansCL/wiki",
            .Description = "KNLMeansCL is an optimized pixelwise OpenCL implementation of the Non-local means denoising algorithm. Every pixel is restored by the weighted average of all pixels in its search window. The level of averaging is determined by the filtering parameter h.",
            .VsFilterNames = {"knlm.KNLMeansCL"},
            .AvsFilterNames = {"KNLMeansCL"}})

        Add(New PluginPackage With {
            .Name = "DSS2mod",
            .Filename = "avss.dll",
            .WebURL = "http://code.google.com/p/xvid4psp/downloads/detail?name=DSS2%20mod%20%2B%20LAVFilters.7z&can=2&q=",
            .Description = "Direct Show source filter.",
            .AvsFilterNames = {"DSS2"}})

        Add(New PluginPackage With {
            .Name = "Deblock",
            .Filename = "Deblock.dll",
            .Description = "Deblocking plugin using the deblocking filter of h264.",
            .HelpFilename = "Readme.txt",
            .HelpURL = "http://avisynth.nl/index.php/DeBlock",
            .WebURL = "https://github.com/299792458m/Avisynth-Deblock",
            .Location = "Plugins\AVS\Deblock",
            .AvsFilterNames = {"Deblock"}})

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
            .AvsFilterNames = {"TNLMeans"}})

        Add(New PluginPackage With {
            .Name = "VagueDenoiser",
            .Filename = "VagueDenoiser.dll",
            .Description = "This is a Wavelet based Denoiser. Basically, it transforms each frame from the video input into the wavelet domain, using various wavelet filters. Then it applies some filtering to the obtained coefficients.",
            .HelpFilename = "vaguedenoiser.html",
            .WebURL = "http://avisynth.nl/index.php/VagueDenoiser",
            .AvsFilterNames = {"VagueDenoiser"}})

        Add(New PluginPackage With {
            .Name = "AnimeIVTC",
            .Filename = "AnimeIVTC.avsi",
            .WebURL = "http://avisynth.nl/index.php/AnimeIVTC",
            .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.5%20and%20up/AnimeIVTC.avsi",
            .AvsFilterNames = {"AnimeIVTC"}})

        Add(New PluginPackage With {
            .Name = "mvtools2",
            .Filename = "mvtools2.dll",
            .Description = "MVTools is collection of functions for estimation and compensation of objects motion in video clips. Motion compensation may be used for strong temporal denoising, advanced framerate conversions, image restoration and other tasks.",
            .WebURL = "http://github.com/pinterf/mvtools",
            .DownloadURL = "https://github.com/pinterf/mvtools/releases",
            .HelpURL = "http://avisynth.nl/index.php/MVTools",
            .AvsFilterNames = {"MSuper", "MAnalyse", "MCompensate", "MMask", "MSCDetection", "MShow", "MDepan", "MFlow", "MFlowInter", "MFlowFps", "MBlockFps", "MFlowBlur", "MDegrain1", "MDegrain2", "MDegrain3", "MDegrain4", "MDegrain5", "MDegrain6", "MDegrainN", "MRecalculate", "MVShow"}})

        Add(New PluginPackage With {
            .Name = "DePan",
            .Filename = "DePan.dll",
            .Location = "Plugins\AVS\MVTools2",
            .WebURL = "https://github.com/pinterf/mvtools",
            .DownloadURL = "https://github.com/pinterf/mvtools/releases",
            .AvsFilterNames = {"DePan", "DePanInterleave", "DePanStabilize", "DePanScenes"}})

        Add(New PluginPackage With {
            .Name = "DePanEstimate",
            .Location = "Plugins\AVS\MVTools2",
            .Filename = "DePanEstimate.dll",
            .WebURL = "https://github.com/pinterf/mvtools",
            .DownloadURL = "https://github.com/pinterf/mvtools/releases",
            .AvsFilterNames = {"DePanEstimate"}})

        Add(New PluginPackage With {
            .Name = "JincResize",
            .Filename = "JincResize.dll",
            .Description = "Jinc (EWA Lanczos) resampling plugin for AviSynth 2.6/AviSynth+.",
            .HelpFilename = "Readme.txt",
            .HelpURL = "https://github.com/Asd-g/AviSynth-JincResize/blob/2.0.1/README.md",
            .WebURL = "http://avisynth.nl/index.php/JincResize",
            .AvsFilterNames = {"Jinc36Resize", "Jinc64Resize", "Jinc144Resize", "Jinc256Resize"}})

        Add(New PluginPackage With {
            .Name = "HQDN3D",
            .Filename = "Hqdn3d.dll",
            .HelpFilename = "Readme.txt",
            .HelpURL = "https://github.com/Asd-g/hqdn3d",
            .WebURL = "https://github.com/Asd-g/hqdn3d",
            .AvsFilterNames = {"HQDN3D"}})

        Add(New PluginPackage With {
            .Name = "HQDeringmod",
            .Filename = "HQDeringmod.avsi",
            .HelpFilename = "Readme.txt",
            .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.5%20and%20up/HQDeringmod.avsi",
            .WebURL = "http://avisynth.nl/index.php/HQDering_mod",
            .Description = "Applies deringing by using a smart smoother near edges (where ringing occurs) only.",
            .AvsFilterNames = {"HQDeringmod"}})

        Add(New PluginPackage With {
            .Name = "InterFrame",
            .Filename = "InterFrame.avsi",
            .HelpFilename = "InterFrame.html",
            .Description = "A frame interpolation script that makes accurate estimations about the content of frames.",
            .Location = "Plugins\AVS\InterFrame2",
            .WebURL = "http://avisynth.nl/index.php/InterFrame",
            .HelpURL = "https://www.spirton.com/uploads/InterFrame/InterFrame2.html",
            .AvsFilterNames = {"InterFrame"}})

        Add(New PluginPackage With {
            .Name = "SVPFlow 1",
            .Location = "Plugins\AVS\SVPFlow",
            .HelpFilename = "Readme.txt",
            .Description = "Motion vectors search plugin  is a deeply refactored and modified version of MVTools2 Avisynth plugin.",
            .Filename = "svpflow1_64.dll",
            .WebURL = "http://avisynth.nl/index.php/SVPFlow",
            .AvsFilterNames = {"analyse_params", "super_params", "SVSuper", "SVAnalyse"}})

        Add(New PluginPackage With {
            .Name = "SVPFlow 2",
            .Location = "Plugins\AVS\SVPFlow",
            .HelpFilename = "Readme.txt",
            .Description = "Motion vectors search plugin is a deeply refactored and modified version of MVTools2 Avisynth plugin.",
            .Filename = "svpflow2_64.dll",
            .WebURL = "http://avisynth.nl/index.php/SVPFlow",
            .AvsFilterNames = {"smoothfps_params", "SVConvert", "SVSmoothFps"}})

        Add(New PluginPackage With {
            .Name = "MipSmooth",
            .Filename = "MipSmooth.dll",
            .Description = "a reinvention of SmoothHiQ and Convolution3D. MipSmooth was made to enable smoothing of larger pixel areas than 3x3(x3), to remove blocks and smoothing out low-frequency noise.",
            .HelpFilename = "MipSmooth.html",
            .WebURL = "http://avisynth.org.ru/docs/english/externalfilters/mipsmooth.htm",
            .AvsFilterNames = {"MipSmooth"}})

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
            .Description = "Halo removal script that uses DeHalo_alpha with a few masks and optional contra-sharpening to try remove halos without removing important details (like line edges). It also includes FineDehalo2, this function tries to remove 2nd order halos. See script for extensive information.",
            .WebURL = "http://avisynth.nl/index.php/FineDehalo",
            .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.5%20and%20up/FineDehalo.avsi",
            .HelpFilename = "Readme.txt",
            .AvsFilterNames = {"FineDehalo"}})

        Add(New PluginPackage With {
            .Name = "CNR2",
            .Filename = "CNR2.dll",
            .HelpFilename = "CNR2.html",
            .Description = "A fast chroma denoiser. Very effective against stationary rainbows and huge analogic chroma activity. Useful to filter VHS/TV caps.",
            .WebURL = "http://avisynth.nl/index.php/Cnr2",
            .AvsFilterNames = {"cnr2"}})

        Add(New PluginPackage With {
            .Name = "Lazy Utilities",
            .Filename = "LUtils.avsi",
            .Description = "A collection of helper and wrapper functions meant to help script authors in handling common operations.",
            .WebURL = "https://github.com/AviSynth/avs-scripts",
            .AvsFilterNames = {"LuStackedNto16", "LuPlanarToStacked", "LuRGB48YV12ToRGB48Y", "LuIsFunction", "LuSeparateColumns", "LuMergePlanes", "LuIsHD", "LuConvCSP", "Lu8To16", "Lu16To8", "LuIsEq", "LuSubstrAtIdx", "LuSubstrCnt", "LuReplaceStr", "LUIsDefined", "LuMerge", "LuLut", "LuLimitDif", "LuBlankClip", "LuIsSameRes"}})

        Add(New PluginPackage With {
            .Name = "MultiSharpen",
            .Filename = "MultiSharpen.avsi",
            .Description = "A small but useful Sharpening Function.",
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
            .Description = "z.lib resizers for AviSynth+.",
            .HelpFilename = "README.md",
            .WebURL = "http://avisynth.nl/index.php/Avsresize",
            .AvsFilterNames = {"z_ConvertFormat", "z_PointResize", "z_BilinearResize", "z_BicubicResize", "z_LanczosResize", "z_Lanczos4Resize", "z_Spline16Resize", "z_Spline36Resize", "z_Spline64Resize"}})

        Add(New PluginPackage With {
            .Name = "DPID",
            .Filename = "DPID.dll",
            .Description = "dpid is an algorithm that preserves visually important details in downscaled images and is especially suited for large downscaling factors.",
            .HelpFilename = "README.md",
            .DownloadURL = "https://github.com/Asd-g/AviSynth-DPID/releases/",
            .WebURL = "https://github.com/Asd-g/AviSynth-DPID",
            .Dependencies = {"avsresize.dll"},
            .AvsFilterNames = {"DPID", "DPIDraw"}})

        Add(New PluginPackage With {
            .Name = "AVS_LibPlacebo",
            .Filename = "avs_libplacebo.dll",
            .Description = "An AviSynth+ plugin interface to libplacebo - a reusable library for Vulcan GPU-accelerated image/video processing primitives and shaders." + BR2 + "This is a port of the VapourSynth plugin vs-placebo.",
            .HelpFilename = "README.md",
            .DownloadURL = "https://github.com/Asd-g/avslibplacebo/releases/",
            .WebURL = "https://github.com/Asd-g/avslibplacebo",
            .AvsFilterNames = {"libplacebo_Deband", "libplacebo_Resample", "libplacebo_Shader", "libplacebo_Tonemap"}})

        Add(New PluginPackage With {
            .Name = "libvs_placebo",
            .Filename = "libvs_placebo.dll",
            .Description = "A VapourSynth plugin interface to libplacebo - a reusable library for Vulcan GPU-accelerated image/video processing primitives and shaders.",
            .DownloadURL = "https://github.com/Lypheo/vs-placebo/releases",
            .WebURL = "https://github.com/Lypheo/vs-placebo",
            .HelpURL = "https://github.com/Lypheo/vs-placebo/blob/master/README.md",
            .VsFilterNames = {"placebo.Deband", "placebo.Resample", "placebo.Shader", "placebo.Tonemap"}})

        Add(New PluginPackage With {
            .Name = "ResizeX",
            .Filename = "ResizeX.avsi",
            .Description = "ResizeX is a wrapper function for AviSynth's internal resizers and Dither_resize16 that corrects for the chroma shift caused by the internal resizers when they're used on horizontally subsampled chroma with MPEG2 placement.",
            .WebURL = "http://avisynth.nl/index.php/External_filters#Resizers",
            .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.6%20and%20up/ResizeX.avsi",
            .AvsFilterNames = {"ResizeX"}})

        Add(New PluginPackage With {
            .Name = "Deblock_QED",
            .Filename = "Deblock_QED.avsi",
            .Description = "Designed to provide 8x8 deblocking sensitive to the amount of blocking in the source, compared to other deblockers which apply an uniform deblocking across every frame.",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/Deblock_QED",
            .AvsFilterNames = {"Deblock_QED"}})

        Add(New PluginPackage With {
            .Name = "nnedi3_resize16 AVSI",
            .Filename = "nnedi3_resize16.avsi",
            .Description = "nnedi3_resize16 is an advanced script for image resizing and colorspace conversion.",
            .Location = "Plugins\AVS\Scripts",
            .WebURL = "http://avisynth.nl/index.php/Nnedi3_resize16",
            .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.6%20and%20up/nnedi3_resize16.avsi",
            .AvsFilterNames = {"nnedi3_resize16"}})

        Add(New PluginPackage With {
            .Name = "nnedi3x AVSI",
            .Filename = "nnedi3x.avsi",
            .Description = "nnedi3x script function is the same as nnedi3ocl but supports RGB24 and YUY2. It also doesn't complain if you feed it with the now removed parameters from the original nnedi3.",
            .Location = "Plugins\AVS\Scripts",
            .WebURL = "http://avisynth.nl/index.php/Nnedi3ocl/nnedi3x",
            .AvsFilterNames = {"nnedi3x"}})

        Add(New PluginPackage With {
            .Name = "edi_rpow2 AVSI",
            .Description = "An improved rpow2 function for nnedi3, nnedi3ocl, eedi3, and eedi2.",
            .Filename = "edi_rpow2.avsi",
            .Location = "Plugins\AVS\Scripts",
            .Dependencies = {"ResizeX.avsi"},
            .WebURL = "https://forum.doom9.org/showthread.php?p=1738351#post1738351",
            .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.6%20and%20up/edi_rpow2.avsi",
            .AvsFilterNames = {"edi_rpow2"}})

        Add(New PluginPackage With {
            .Name = "SmoothD2",
            .Location = "Plugins\AVS\SmoothD2",
            .Filename = "SmoothD2.dll",
            .HelpFilename = "Readme.txt",
            .Description = "Deblocking filter. Rewrite of SmoothD. Faster, better detail preservation, optional chroma deblocking.",
            .WebURL = "http://avisynth.nl/index.php/SmoothD2",
            .AvsFilterNames = {"SmoothD2"}})

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
            .Description = "XNLMeans is an AviSynth plugin implementation of the Non Local Means denoising algorithm.",
            .HelpFilename = "Readme.txt",
            .AvsFilterNames = {"xNLMeans"}})

        Add(New PluginPackage With {
            .Name = "DehaloAlpha",
            .Filename = "Dehalo_alpha.avsi",
            .Description = "Reduce halo artifacts that can occur when sharpening.",
            .HelpFilename = "Readme.txt",
            .AvsFilterNames = {"DeHalo_alpha_mt", "DeHalo_alpha_2BD"}})

        Add(New PluginPackage With {
            .Name = "MAA2Mod",
            .Location = "Plugins\AVS\MAA2",
            .Filename = "maa2mod.avsi",
            .Description = "Updated version of the MAA antialising script from AnimeIVTC. MAA2 uses tp7's SangNom2, which provides a nice speedup for SangNom-based antialiasing, especially when only processing the luma plane. Mod version also includes support for EEDI3 along with a few other new functions.",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/MAA2",
            .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.6%20and%20up/maa2.avsi",
            .AvsFilterNames = {"maa2", "Sangnom2AA", "maa2ee"}})

        Add(New PluginPackage With {
            .Name = "DAA3Mod",
            .Filename = "daa3mod.avsi",
            .Description = "Motion-compensated anti-aliasing with contra-sharpening, can deal with ifade too, created because when applied daa3 to fixed scenes, it could damage some details and other issues.",
            .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.5%20and%20up/daa3MOD.avsi",
            .WebURL = "https://forum.doom9.org/showthread.php?p=1639679#post1639679",
            .AvsFilterNames = {"daa3mod", "mcdaa3"}})

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
            .Description = "Histogram for both DenoiseMD and DenoiseMF.",
            .WebURL = "http://avisynth.nl",
            .AvsFilterNames = {"DiffCol"}})

        Add(New PluginPackage With {
            .Name = "FrameRateConverter DLL",
            .Filename = "FrameRateConverter-x64.dll",
            .Location = "Plugins\AVS\FrameRateConverter",
            .Description = "Increases the frame rate with interpolation and fine artifact removal.",
            .WebURL = "https://github.com/mysteryx93/FrameRateConverter",
            .AvsFilterNames = {"FrameRateConverter"}})

        Add(New PluginPackage With {
            .Name = "FrameRateConverter AVSI",
            .Filename = "FrameRateConverter.avsi",
            .Location = "Plugins\AVS\FrameRateConverter",
            .Description = "Increases the frame rate with interpolation and fine artifact removal.",
            .WebURL = "https://github.com/mysteryx93/FrameRateConverter",
            .AvsFilterNames = {"FrameRateConverter"}})

        Add(New PluginPackage With {
            .Name = "DeNoiseMD",
            .Filename = "DeNoiseMD.avsi",
            .Description = "A fast and accurate denoiser for a Full HD video from a H.264 camera.",
            .WebURL = "http://avisynth.nl",
            .AvsFilterNames = {"DeNoiseMD1", "DenoiseMD2"}})

        Add(New PluginPackage With {
            .Name = "DeNoiseMF",
            .Filename = "DeNoiseMF.avsi",
            .Description = "A fast and accurate denoiser for a Full HD video from a H.264 camera.",
            .WebURL = "http://avisynth.nl",
            .AvsFilterNames = {"DeNoiseMF1", "DenoiseMF2"}})

        Add(New PluginPackage With {
            .Name = "MT Expand Multi",
            .Location = "Plugins\AVS\Dither",
            .Description = "Calls mt_expand or mt_inpand multiple times in order to grow or shrink the mask from the desired width and height.",
            .Filename = "mt_xxpand_multi.avsi",
            .WebURL = "http://avisynth.nl/index.php/Dither",
            .AvsFilterNames = {"mt_expand_multi", "mt_inpand_multi"}})

        Add(New PluginPackage With {
            .Name = "TEMmod",
            .Description = "TEMmod creates an edge mask using gradient vector magnitude.",
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
            .DownloadURL = "https://forum.doom9.org/showpost.php?p=1386559&postcount=3",
            .WebURL = "http://avisynth.nl/index.php/Dither",
            .AvsFilterNames = {"Dither_y_gamma_to_linear", "Dither_y_linear_to_gamma", "Dither_convert_8_to_16", "Dither1Pre", "Dither1Pre", "Dither_repair16", "Dither_convert_yuv_to_rgb", "Dither_convert_rgb_to_yuv", "Dither_resize16", "DitherPost", "Dither_crop16", "DitherBuildMask", "SmoothGrad", "GradFun3", "Dither_box_filter16", "Dither_bilateral16", "Dither_limit_dif16", "Dither_resize16nr", "Dither_srgb_display", "Dither_convey_yuv4xxp16_on_yvxx", "Dither_convey_rgb48_on_yv12", "Dither_removegrain16", "Dither_median16", "Dither_get_msb", "Dither_get_lsb", "Dither_addborders16", "Dither_lut8", "Dither_lutxy8", "Dither_lutxyz8", "Dither_lut16", "Dither_add16", "Dither_sub16", "Dither_max_dif16", "Dither_min_dif16", "Dither_merge16", "Dither_merge16_8", "Dither_sigmoid_direct", "Dither_sigmoid_inverse", "Dither_add_grain16", "Dither_Luma_Rebuild"}})

        Add(New PluginPackage With {
            .Name = "Dither DLL",
            .Location = "Plugins\AVS\Dither",
            .Description = "This package offers a set of tools to manipulate high-bitdepth (16 bits per plane) video clips. The most proeminent features are color banding artifact removal, dithering to 8 bits, colorspace conversions and resizing.",
            .HelpFilename = "dither.html",
            .Filename = "dither.dll",
            .DownloadURL = "https://forum.doom9.org/showpost.php?p=1386559&postcount=3",
            .WebURL = "http://avisynth.nl/index.php/Dither",
            .AvsFilterNames = {"Dither_y_gamma_to_linear", "Dither_y_linear_to_gamma", "Dither_convert_8_to_16", "Dither1Pre", "Dither1Pre", "Dither_repair16", "Dither_convert_yuv_to_rgb", "Dither_convert_rgb_to_yuv", "Dither_resize16", "DitherPost", "Dither_crop16", "DitherBuildMask", "SmoothGrad", "GradFun3", "Dither_box_filter16", "Dither_bilateral16", "Dither_limit_dif16", "Dither_resize16nr", "Dither_srgb_display", "Dither_convey_yuv4xxp16_on_yvxx", "Dither_convey_rgb48_on_yv12", "Dither_removegrain16", "Dither_median16", "Dither_get_msb", "Dither_get_lsb", "Dither_addborders16", "Dither_lut8", "Dither_lutxy8", "Dither_lutxyz8", "Dither_lut16", "Dither_add16", "Dither_sub16", "Dither_max_dif16", "Dither_min_dif16", "Dither_merge16", "Dither_merge16_8", "Dither_sigmoid_direct", "Dither_sigmoid_inverse", "Dither_add_grain16", "Dither_Luma_Rebuild"}})

        Add(New PluginPackage With {
            .Name = "DeGrainMedian",
            .Filename = "DeGrainMedian.dll",
            .Description = "DeGrainMedian is a spatio-temporal limited median filter mainly for film grain removal, but may be used for general denoising.",
            .HelpFilename = "Degrainmedian.html",
            .WebURL = "http://avisynth.nl/index.php/DeGrainMedian",
            .AvsFilterNames = {"DeGrainMedian"}})

        Add(New PluginPackage With {
            .Name = "pSharpen",
            .Filename = "pSharpen.avsi",
            .AvsFilterNames = {"pSharpen"},
            .Description = "pSharpen performs two-point sharpening to avoid overshoot.",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/PSharpen"})

        Add(New PluginPackage With {
            .Name = "GradFun2DBmod",
            .Location = "Plugins\AVS\GradFun2DB",
            .Filename = "GradFun2DBmod.avsi",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/GradFun2DBmod",
            .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.5%20and%20up/GradFun2DBmod.avsi",
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
            .WebURL = "https://github.com/pinterf/AddGrainC",
            .HelpURL = "https://raw.githubusercontent.com/pinterf/AddGrainC/master/Documentation/AddGrainC.txt",
            .DownloadURL = "https://github.com/pinterf/AddGrainC/releases",
            .Description = "Generate film-like grain or other effects (like rain) by adding random noise to a video clip.",
            .AvsFilterNames = {"AddGrainC", "AddGrain"}})

        Add(New PluginPackage With {
            .Name = "YFRC",
            .Filename = "YFRC.avsi",
            .HelpFilename = "Readme.txt",
            .WebURL = "http://avisynth.nl/index.php/YFRC",
            .Description = "Yushko Frame Rate convertor - doubles the frame rate with strong artifact detection and scene change detection. YFRC uses masks to reduce artifacts in areas where interpolation failed.",
            .AvsFilterNames = {"YFRC"}})

        Add(New PluginPackage With {
            .Name = "Deblock",
            .Filename = "Deblock.dll",
            .Location = "Plugins\VS\Deblock",
            .Description = "Deblocking plugin using the deblocking filter of h264.",
            .WebURL = "http://github.com/HomeOfVapourSynthEvolution/VapourSynth-Deblock",
            .VsFilterNames = {"deblock.Deblock"}})

        Add(New PluginPackage With {
            .Name = "MSharpen",
            .Filename = "msharpen.dll",
            .WebURL = "http://avisynth.nl/index.php/MSharpen",
            .HelpFilename = "Readme.txt",
            .AvsFilterNames = {"MSharpen"}})

        Add(New PluginPackage With {
            .Name = "vsMSharpen",
            .Filename = "vsMSharpen.dll",
            .Location = "Plugins\AVS\vsMSharpen",
            .WebURL = "https://github.com/Asd-g/AviSynth-vsMSharpen",
            .DownloadURL = "https://github.com/Asd-g/AviSynth-vsMSharpen/releases",
            .HelpFilename = "Readme.md",
            .AvsFilterNames = {"vsMSharpen"}})

        Add(New PluginPackage With {
            .Name = "mClean",
            .Filename = "mClean.avsi",
            .WebURL = "http://forum.doom9.org/showthread.php?t=174804",
            .Description = "Removes noise whilst retaining as much detail as possible.",
            .AvsFilterNames = {"mClean"}})

        Add(New PluginPackage With {
            .Name = "vsCube",
            .Filename = "vscube.dll",
            .Description = "Deblocking plugin using the deblocking filter of h264.",
            .WebURL = "http://rationalqm.us/mine.html",
            .Location = "Plugins\AVS\VSCube",
            .AvsFilterNames = {"Cube"}})

        Add(New PluginPackage With {
            .Name = "RgTools",
            .Filename = "RgTools.dll",
            .WebURL = "http://github.com/pinterf/RgTools",
            .DownloadURL = "https://github.com/pinterf/RgTools/releases",
            .HelpURL = "http://avisynth.nl/index.php/RgTools",
            .HelpFilename = "README.md",
            .Description = "RgTools is a modern rewrite of RemoveGrain, Repair, BackwardClense, Clense, ForwardClense, VerticalCleaner and TemporalRepair in a single plugin.",
            .AvsFilterNames = {"RemoveGrain", "Repair", "BackwardClense", "Clense", "ForwardClense", "VerticalCleaner", "TemporalRepair"}})

        Add(New PluginPackage With {
            .Name = "JPSDR",
            .Filename = "Plugins_JPSDR.dll",
            .WebURL = "https://github.com/jpsdr/plugins_JPSDR",
            .DownloadURL = "https://github.com/jpsdr/plugins_JPSDR/releases",
            .Description = "Merge of AutoYUY2, NNEDI3, HDRTools, aWarpSharpMT and ResampleMT. Included is the W7 AVX2 variant.",
            .AvsFilterNames = {"aBlur", "aSobel", "AutoYUY2", "aWarp", "aWarp4", "aWarpSharp2", "BicubicResizeMT", "BilinearResizeMT", "BlackmanResizeMT", "ConvertLinearRGBtoYUV", "ConvertLinearRGBtoYUV_BT2446_A_HDRtoSDR", "ConvertRGB_Hable_HDRtoSDR", "ConvertRGB_Mobius_HDRtoSDR", "ConvertRGB_Reinhard_HDRtoSDR", "ConvertRGBtoXYZ", "ConvertXYZ_Hable_HDRtoSDR", "ConvertXYZ_Mobius_HDRtoSDR", "ConvertXYZ_Reinhard_HDRtoSDR", "ConvertXYZ_Scale_HDRtoSDR", "ConvertXYZ_Scale_SDRtoHDR", "ConvertXYZtoRGB", "ConvertXYZtoYUV", "ConvertYUVtoLinearRGB", "ConvertYUVtoXYZ", "ConverXYZ_BT2446_C_HDRtoSDR", "DeBicubicResizeMT", "DeBilinearResizeMT", "DeBlackmanResizeMT", "DeGaussResizeMT", "DeLanczos4ResizeMT", "DeLanczosResizeMT", "DeSincResizeMT", "DeSpline16ResizeMT", "DeSpline36ResizeMT", "DeSpline64ResizeMT", "GaussResizeMT", "Lanczos4ResizeMT", "LanczosResizeMT", "nnedi3", "nnedi3_rpow2", "PointResizeMT", "SincResizeMT", "Spline16ResizeMT", "Spline36ResizeMT", "Spline64ResizeMT"}})

        Add(New PluginPackage With {
            .Name = "TDeint",
            .Filename = "TDeint.dll",
            .Location = "Plugins\AVS\TDeint",
            .WebURL = "https://github.com/pinterf/TIVTC",
            .DownloadURL = "https://github.com/pinterf/TIVTC/releases",
            .Description = "TDeint is a bi-directionally, motion adaptive, sharp deinterlacer.",
            .AvsFilterNames = {"TDeint"}})

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
            .Description = "Yet Another Deinterlacing Filter mod for Avisynth2.6/Avisynth+.",
            .WebURL = "https://github.com/Asd-g/yadifmod2",
            .HelpURL = "https://github.com/Asd-g/yadifmod2/blob/master-1/readme.md",
            .AvsFilterNames = {"yadifmod2"}})

        Add(New PluginPackage With {
            .Name = "LibDedot",
            .Filename = "libdedot.dll",
            .Description = "Temporal rainbow and dotcrawl filter.",
            .WebURL = "https://github.com/dubhater/vapoursynth-dedot",
            .DownloadURL = "https://github.com/dubhater/vapoursynth-dedot/releases",
            .VsFilterNames = {"dedot.Dedot"}})

        Add(New PluginPackage With {
            .Name = "Yadifmod",
            .Filename = "Yadifmod.dll",
            .Description = "Modified version of Fizick's avisynth filter port of yadif from mplayer. This version doesn't internally generate spatial predictions, but takes them from an external clip.",
            .WebURL = "http://github.com/HomeOfVapourSynthEvolution/VapourSynth-Yadifmod",
            .VsFilterNames = {"yadifmod.Yadifmod"}})

        Add(New PluginPackage With {
            .Name = "FixTelecinedFades",
            .Filename = "libftf_em64t_avx_fma.dll",
            .Location = "Plugins\VS\FixTelecinedFades",
            .Description = "InsaneAA Anti-Aliasing Script.",
            .WebURL = "https://github.com/IFeelBloated/Fix-Telecined-Fades",
            .VsFilterNames = {"ftf.FixFades"}})

        Add(New PluginPackage With {
            .Name = "vcm",
            .Filename = "vcm.dll",
            .HelpFilename = "vcm.html",
            .Description = "vcm plugin for VapourSynth.",
            .WebURL = "http://www.avisynth.nl/users/vcmohan/vcm/vcm.html",
            .VsFilterNames = {"vcm.Median", "vcm.Variance", "vcm.Amp", "vcm.GBlur", "vcm.MBlur", "vcm.Hist", "vcm.Fan", "vcm.Mean", "vcm.Neural", "vcm.Veed", "vcm.SaltPepper",
                               "vcm.DeBarrel", "vcm.DeJitter", "vcm.Reform", "vcm.Rotate", "vcm.Fisheye",
                               "vcm.F1Quiver", "vcm.F1QClean", "vcm.F1QLimit", "vcm.F2Quiver", "vcm.F2QLimit", "vcm.F2QBlur", "vcm.F2QBokeh", "vcm.F2QCorr", "vcm.F2QSharp",
                               "vcm.Bokeh", "vcm.ColorBox", "vcm.Grid", "vcm.Jitter", "vcm.Pattern", "vcm.StepFilter", "vcm.Circles"}})

        Add(New PluginPackage With {
            .Name = "vcmod",
            .Filename = "vcmod.dll",
            .Description = "vcmod plugin for VapourSynth.",
            .WebURL = "https://github.com/AmusementClub/vcm",
            .VsFilterNames = {"vcmod.Median", "vcmod.Amplitude", "vcmod.GBlur", "vcmod.MBlur", "vcmod.Histogram", "vcmod.Fan", "vcmod.Variance", "vcmod.Neural", "vcmod.Veed", "vcmod.SaltPepper"}})

        Add(New PluginPackage With {
            .Name = "vcmove",
            .Filename = "vcmove.dll",
            .HelpFilename = "vcmove.html",
            .Description = "vcmove plugin for VapourSynth.",
            .WebURL = "http://www.avisynth.nl/users/vcmohan/vcmove/vcmove.html",
            .VsFilterNames = {"vcmove.Rotate", "vcmove.DeBarrel", "vcmove.Quad2Rect", "vcmove.Rect2Quad"}})

        Add(New PluginPackage With {
            .Name = "vcfreq",
            .Filename = "vcfreq.dll",
            .HelpFilename = "vcfreq.html",
            .Description = "vcvcfreq plugin for VapourSynth.",
            .WebURL = "http://www.avisynth.nl/users/vcmohan/vcfreq/vcfreq.html",
            .VsFilterNames = {"vcfreq.F1Quiver", "vcfreq.F2Quiver", "vcfreq.Blur", "vcfreq.Sharp"}})

        Add(New PluginPackage With {
            .Name = "nnedi3",
            .Filename = "libnnedi3.dll",
            .Location = "Plugins\VS\nnedi3",
            .WebURL = "http://github.com/dubhater/vapoursynth-nnedi3",
            .Description = "nnedi3 is an intra-field only deinterlacer. It takes in a frame, throws away one field, and then interpolates the missing pixels using only information from the kept field.",
            .VsFilterNames = {"nnedi3.nnedi3"}})

        Add(New PluginPackage With {
            .Name = "FFT3DFilter",
            .Filename = "fft3dfilter.dll",
            .WebURL = "http://github.com/VFR-maniac/VapourSynth-FFT3DFilter",
            .Description = "FFT3DFilter uses Fast Fourier Transform method for image processing in frequency domain.",
            .VsFilterNames = {"fft3dfilter.FFT3DFilter"}})

        Add(New PluginPackage With {
            .Name = "MiscFilters",
            .Filename = "MiscFilters.dll",
            .WebURL = "https://github.com/vapoursynth/vs-miscfilters-obsolete",
            .Description = "",
            .VsFilterNames = {"misc.AverageFrames", "misc.Hysteresis", "misc.SCDetect"}})

        Add(New PluginPackage With {
            .Name = "mvtools",
            .Filename = "libmvtools.dll",
            .WebURL = "http://github.com/dubhater/vapoursynth-mvtools",
            .Description = "MVTools is a set of filters for motion estimation and compensation.",
            .VsFilterNames = {"mv.Super", "mv.Analyse", "mv.Recalculate", "mv.Compensate", "mv.Degrain1", "mv.Degrain2",
                "mv.Degrain3", "mv.Mask", "mv.Finest", "mv.Flow", "mv.FlowBlur", "mv.FlowInter", "mv.FlowFPS", "mv.BlockFPS", "mv.SCDetection",
                "mv.DepanAnalyse", "mv.DepanEstimate", "mv.DepanCompensate", "mv.DepanStabilise"}})

        Add(New PluginPackage With {
            .Name = "mvtools-sf",
            .Filename = "libmvtools_sf_em64t.dll",
            .WebURL = "https://github.com/IFeelBloated/vapoursynth-mvtools-sf",
            .Description = "MVTools is a set of filters for motion estimation and compensation.",
            .VsFilterNames = {"mvsf.Super", "mvsf.Analyse", "mvsf.Recalculate", "mvsf.Compensate", "mvsf.Degrain1", "mvsf.Degrain2",
                "mvsf.Degrain3", "mvsf.Mask", "mvsf.Finest", "mvsf.Flow", "mvsf.FlowBlur", "mvsf.FlowInter", "mvsf.FlowFPS", "mvsf.BlockFPS", "mvsf.SCDetection",
                "mvsf.DepanAnalyse", "mvsf.DepanEstimate", "mvsf.DepanCompensate", "mvsf.DepanStabilise"}})

        Add(New PluginPackage With {
            .Name = "TemporalMedian",
            .Filename = "libtemporalmedian.dll",
            .WebURL = "https://github.com/dubhater/vapoursynth-temporalmedian",
            .Description = "TemporalMedian is a temporal denoising filter. It replaces every pixel with the median of its temporal neighbourhood.",
            .VsFilterNames = {"tmedian.TemporalMedian"}})

        Add(New PluginPackage With {
            .Name = "resamplehq",
            .Filename = "resamplehq.py",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "https://github.com/4re/resamplehq",
            .Description = "TemporalMedian is a temporal denoising filter. It replaces every pixel with the median of its temporal neighbourhood.",
            .VsFilterNames = {"resamplehq.resample_hq"}})

        Add(New PluginPackage With {
            .Name = "ASTDR",
            .Filename = "ASTDR.py",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "https://github.com/dubhater/vapoursynth-astdr",
            .Description = "ASTDR is a derainbow function for VapourSynth. ASTDRmc performs motion compensation before calling ASTDR. This is a port of the Avisynth function of the same name, version 1.74.",
            .Dependencies = {"adjust.py", "MiscFilters.dll", "fft3dfilter.dll", "libawarpsharp2.dll", "libfluxsmooth.dll", "libhqdn3d.dll", "libdecross.dll", "libmotionmask.dll", "libtemporalsoften2"},
            .VsFilterNames = {"ASTDR.ASTDR", "ASTDR.ASTDRmc", "ASTDR.BlurForASTDR"}})

        Add(New PluginPackage With {
            .Name = "mcdegrainsharp",
            .Filename = "mcdegrainsharp.py",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "https://gist.github.com/4re/b5399b1801072458fc80#file-mcdegrainsharp-py",
            .Description = "TemporalMedian is a temporal denoising filter. It replaces every pixel with the median of its temporal neighbourhood.",
            .Dependencies = {"TCanny.dll"},
            .VsFilterNames = {"mcdegrainsharp.mcdegrainsharp"}})

        Add(New PluginPackage With {
            .Name = "G41Fun",
            .Filename = "G41Fun.py",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "https://github.com/Vapoursynth-Plugins-Gitify/G41Fun",
            .Description = "The replaced script for hnwvsfunc with re-written functions.",
            .VsFilterNames = {"G41Fun.mClean", "G41Fun.NonlinUSM", "G41Fun.DetailSharpen", "G41Fun.LUSM", "G41Fun.JohnFPS", "G41Fun.TemporalDegrain2",
                "G41Fun.MCDegrainSharp", "G41Fun.FineSharp", "G41Fun.psharpen", "G41Fun.QTGMC", "G41Fun.SMDegrain", "G41Fun.daamod",
                "G41Fun.STPressoHD", "G41Fun.MLDegrain", "G41Fun.Hysteria", "G41Fun.SuperToon", "G41Fun.EdgeDetect", "G41Fun.SpotLess",
                "G41Fun.HQDeringmod", "G41Fun.LSFmod", "G41Fun.SeeSaw", "G41Fun.MaskedDHA"}})

        Add(New PluginPackage With {
            .Name = "fvsfunc",
            .Filename = "fvsfunc.py",
            .Description = "Small collection of VapourSynth functions.",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "https://github.com/Irrational-Encoding-Wizardry/fvsfunc",
            .VsFilterNames = {"fvsfunc.GradFun3mod", "fvsfunc.DescaleM", "fvsfunc.Downscale444", "fvsfunc.JIVTC", "fvsfunc.OverlayInter", "fvsfunc.AutoDeblock", "fvsfunc.ReplaceFrames", "fvsfunc.maa", "fvsfunc.TemporalDegrain", "fvsfunc.DescaleAA", "fvsfunc.InsertSign"}})

        Add(New PluginPackage With {
            .Name = "mvmulti",
            .Filename = "mvmulti.py",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "http://github.com/dubhater/vapoursynth-mvtools",
            .Description = "MVTools is a set of filters for motion estimation and compensation.",
            .VsFilterNames = {"mvmulti.StoreVect", "mvmulti.Analyse", "mvmulti.Recalculate", "mvmulti.Compensate", "mvmulti.Restore", "mvmulti.Flow", "mvmulti.DegrainN"}})

        Add(New PluginPackage With {
            .Name = "Sangnom",
            .Filename = "libsangnom.dll",
            .WebURL = "https://github.com/dubhater/vapoursynth-sangnom",
            .Description = "SangNom is a single field deinterlacer using edge-directed interpolation but nowadays it's mainly used in anti-aliasing scripts.",
            .VsFilterNames = {"sangnom.SangNom"}})

        Add(New PluginPackage With {
            .Name = "CAS",
            .Filename = "CAS.dll",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-CAS",
            .DownloadURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-CAS/releases",
            .Description = "Contrast Adaptive Sharpening.",
            .VsFilterNames = {"cas.CAS"}})

        Add(New PluginPackage With {
            .Name = "nnedi3_rpow2",
            .Filename = "nnedi3_rpow2.py",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "https://github.com/Irrational-Encoding-Wizardry/fvsfunc",
            .Description = "nnedi3_rpow2 ported from Avisynth for VapourSynth.",
            .VsFilterNames = {"nnedi3_rpow2.nnedi3_rpow2"}})

        Add(New PluginPackage With {
            .Name = "znedi3",
            .Filename = "vsznedi3.dll",
            .Location = "Plugins\VS\nnedi3",
            .HelpFilename = "Readme.txt",
            .WebURL = "https://github.com/sekrit-twc/znedi3",
            .Description = "znedi3 is a CPU-optimized version of nnedi.",
            .VsFilterNames = {"znedi3.nnedi3"}})

        Add(New PluginPackage With {
            .Name = "nnedi3cl",
            .Filename = "NNEDI3CL.dll",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-NNEDI3CL",
            .Location = "Plugins\VS\nnedi3",
            .Description = "nnedi3 is an intra-field only deinterlacer. It takes a frame, throws away one field, and then interpolates the missing pixels using only information from the remaining field. It is also good for enlarging images by powers of two.",
            .VsFilterNames = {"nnedi3cl.NNEDI3CL"}})

        Add(New PluginPackage With {
            .Name = "EEDI3m",
            .Filename = "EEDI3m.dll",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-EEDI3",
            .Description = "EEDI3 works by finding the best non-decreasing (non-crossing) warping between two lines by minimizing a cost functional.",
            .VsFilterNames = {"eedi3m.EEDI3"}})

        Add(New PluginPackage With {
            .Name = "EEDI2",
            .Filename = "EEDI2.dll",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-EEDI2",
            .Description = "EEDI2 works by finding the best non-decreasing (non-crossing) warping between two lines by minimizing a cost functional.",
            .VsFilterNames = {"eedi2.EEDI2"}})

        Add(New PluginPackage With {
            .Name = "W3FDIF",
            .Filename = "W3FDIF.dll",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-W3FDIF/releases",
            .Description = "Weston 3 Field Deinterlacing Filter. Ported from FFmpeg's libavfilter.",
            .VsFilterNames = {"w3fdif.W3FDIF"}})

        Add(New PluginPackage With {
            .Name = "MiniDeen",
            .Filename = "neo-minideen.dll",
            .WebURL = "https://github.com/HomeOfAviSynthPlusEvolution/MiniDeen",
            .DownloadURL = "https://github.com/HomeOfAviSynthPlusEvolution/MiniDeen/releases",
            .Description = "MiniDeen is a spatial denoising filter which replaces every pixel with the average of its neighbourhood.",
            .AvsFilterNames = {"MiniDeen"},
            .VsFilterNames = {"neo_minideen.MiniDeen"}})

        Add(New PluginPackage With {
            .Name = "IT",
            .Filename = "vs_it.dll",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-IT",
            .Description = "VapourSynth Plugin - Inverse Telecine (YV12 Only, IT-0051 base, IT_YV12-0103 base).",
            .VsFilterNames = {"it.IT"}})

        Add(New PluginPackage With {
            .Name = "TDeintMod",
            .Filename = "TDeintMod.dll",
            .HelpFilename = "Readme.txt",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-TDeintMod",
            .Description = "TDeintMod is a combination of TDeint and TMM, which are both ported from tritical's AviSynth plugin.",
            .VsFilterNames = {"tdm.TDeintMod"}})

        Add(New PluginPackage With {
            .Name = "adjust",
            .Filename = "adjust.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "very basic port of the built-in Avisynth filter Tweak.",
            .WebURL = "http://github.com/dubhater/vapoursynth-adjust",
            .VsFilterNames = {"adjust.Tweak"}})

        Add(New PluginPackage With {
            .Name = "Oyster",
            .Filename = "Oyster.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "Oyster is an experimental implement of the Blocking Matching concept, designed specifically for compression artifacts removal.",
            .WebURL = "https://github.com/IFeelBloated/Oyster",
            .VsFilterNames = {"Oyster.Basic", "Oyster.Deringing", "Oyster.Destaircase", "Oyster.Deblocking", "Oyster.Super"}})

        Add(New PluginPackage With {
            .Name = "Plum",
            .Filename = "Plum.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "Plum is a sharpening/blind deconvolution suite with certain advanced features like Non-Local error, Block Matching, etc..",
            .WebURL = "https://github.com/IFeelBloated/Plum",
            .VsFilterNames = {"Plum.Super", "Plum.Basic", "Plum.Final"}})

        Add(New PluginPackage With {
            .Name = "Vine",
            .Filename = "Vine.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "Plum is a sharpening/blind deconvolution suite with certain advanced features like Non-Local error, Block Matching, etc..",
            .WebURL = "https://github.com/IFeelBloated/Vine",
            .VsFilterNames = {"Vine.Super", "Vine.Basic", "Vine.Final", "Vine.Dilation", "Vine.Erosion", "Vine.Closing", "Vine.Opening", "Vine.Gradient", "Vine.TopHat", "Vine.Blackhat"}})

        Add(New PluginPackage With {
            .Name = "TCanny",
            .Filename = "TCanny.dll",
            .Description = "Builds an edge map using canny edge detection.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-TCanny",
            .VsFilterNames = {"tcanny.TCanny"}})

        Add(New PluginPackage With {
            .Name = "vsTAAmbk",
            .Filename = "vsTAAmbk.py",
            .Description = "A ported AA-script from Avisynth.",
            .Location = "Plugins\VS\Scripts",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/vsTAAmbk",
            .VsFilterNames = {"taa.TAAmbk", "taa.vsTAAmbk"}})

        Add(New PluginPackage With {
            .Name = "mvsfunc",
            .Filename = "mvsfunc.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "mawen1250's VapourSynth functions.",
            .WebURL = "http://github.com/HomeOfVapourSynthEvolution/mvsfunc",
            .HelpURL = "http://forum.doom9.org/showthread.php?t=172564",
            .VsFilterNames = {
                "mvsfunc.Depth", "mvsfunc.ToRGB", "mvsfunc.ToYUV", "mvsfunc.BM3D", "mvsfunc.VFRSplice",
                "mvsfunc.PlaneStatistics", "mvsfunc.PlaneCompare", "mvsfunc.ShowAverage", "mvsfunc.FilterIf",
                "mvsfunc.FilterCombed", "mvsfunc.Min", "mvsfunc.Max", "mvsfunc.Avg", "mvsfunc.MinFilter",
                "mvsfunc.MaxFilter", "mvsfunc.LimitFilter", "mvsfunc.PointPower", "mvsfunc.CheckMatrix",
                "mvsfunc.postfix2infix", "mvsfunc.SetColorSpace", "mvsfunc.AssumeFrame", "mvsfunc.AssumeTFF",
                "mvsfunc.AssumeBFF", "mvsfunc.AssumeField", "mvsfunc.AssumeCombed", "mvsfunc.CheckVersion",
                "mvsfunc.GetMatrix", "mvsfunc.zDepth", "mvsfunc.GetPlane", "mvsfunc.PlaneAverage",
                "mvsfunc.Preview", "mvsfunc.GrayScale"}})

        Add(New PluginPackage With {
            .Name = "SubText",
            .Filename = "SubText.dll",
            .Description = "Subtitle plugin for VapourSynth based on libass.",
            .WebURL = "https://github.com/vapoursynth/subtext",
            .DownloadURL = "https://github.com/vapoursynth/subtext/releases",
            .VsFilterNames = {"sub.TextFile"}})

        Add(New PluginPackage With {
            .Name = "Bwdif",
            .Filename = "Bwdif.dll",
            .Description = "Motion adaptive deinterlacing based on yadif with the use of w3fdif and cubic interpolation algorithms.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-Bwdif",
            .DownloadURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-Bwdif/releases",
            .VsFilterNames = {"bwdif.Bwdif"}})

        Add(New PluginPackage With {
            .Name = "FluxSmooth",
            .Filename = "libfluxsmooth.dll",
            .VsFilterNames = {"flux.SmoothT", "flux.SmoothST"},
            .Description = "FluxSmooth is a filter for smoothing of fluctuations.",
            .WebURL = "http://github.com/dubhater/vapoursynth-fluxsmooth"})

        Add(New PluginPackage With {
            .Name = "CNR2",
            .Filename = "libcnr2.dll",
            .Location = "Plugins\VS\CNR2",
            .VsFilterNames = {"cnr2.Cnr2"},
            .Description = "Cnr2 is a temporal denoiser designed to denoise only the chroma.",
            .WebURL = "https://github.com/dubhater/vapoursynth-cnr2"})

        Add(New PluginPackage With {
            .Name = "CTMF",
            .Filename = "CTMF.dll",
            .VsFilterNames = {"ctmf.CTMF"},
            .Description = "Constant Time Median Filtering.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-CTMF"})

        Add(New PluginPackage With {
            .Name = "msmoosh",
            .Filename = "libmsmoosh.dll",
            .VsFilterNames = {"msmoosh.MSmooth", "msmoosh.MSharpen"},
            .Description = "MSmooth is a spatial smoother that doesn't touch edges." + BR + "MSharpen is a sharpener that tries to sharpen only edges.",
            .WebURL = "http://github.com/dubhater/vapoursynth-msmoosh"})

        Add(New PluginPackage With {
            .Name = "SVPFlow 1",
            .Location = "Plugins\VS\SVPFlow",
            .Description = "Motion vectors search plugin  is a deeply refactored and modified version of MVTools2 Avisynth plugin.",
            .Filename = "svpflow1_vs64.dll",
            .WebURL = "https://www.svp-team.com/wiki/Manual:SVPflow",
            .VsFilterNames = {"svp1.Super", "svp1.Analyse", "svp1.Convert"}})

        Add(New PluginPackage With {
            .Name = "SVPFlow 2",
            .Location = "Plugins\VS\SVPFlow",
            .Description = "Motion vectors search plugin is a deeply refactored and modified version of MVTools2 Avisynth plugin.",
            .Filename = "svpflow2_vs64.dll",
            .WebURL = "https://www.svp-team.com/wiki/Manual:SVPflow",
            .VsFilterNames = {"svp2.SmoothFps"}})

        Add(New PluginPackage With {
            .Name = "Dither",
            .Location = "Plugins\VS\Scripts",
            .Description = "VapourSynth port of DitherTools.",
            .Filename = "Dither.py",
            .WebURL = "https://github.com/IFeelBloated/VaporSynth-Functions",
            .VsFilterNames = {"Dither.sigmoid_direct", "Dither.sigmoid_inverse", "Dither.linear_to_gamma", "Dither.gamma_to_linear", "Dither.clamp16", "Dither.sbr16", "Dither.Resize16nr", "Dither.get_msb", "Dither.get_lsb"}})

        Add(New PluginPackage With {
            .Name = "DeblockPP7",
            .Filename = "DeblockPP7.dll",
            .Description = "VapourSynth port of pp7 from MPlayer.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DeblockPP7",
            .VsFilterNames = {"pp7.DeblockPP7"}})

        Add(New PluginPackage With {
            .Name = "HQDN3D",
            .Filename = "libhqdn3d.dll",
            .Description = "Avisynth port of hqdn3d from avisynth/mplayer.",
            .WebURL = "https://github.com/Hinterwaeldlers/vapoursynth-hqdn3d",
            .VsFilterNames = {"hqdn3d.Hqdn3d"}})

        Add(New PluginPackage With {
            .Name = "VagueDenoiser",
            .Filename = "VagueDenoiser.dll",
            .Description = "VapourSynth port of VagueDenoiser.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-VagueDenoiser",
            .VsFilterNames = {"vd.VagueDenoiser"}})

        Add(New PluginPackage With {
            .Name = "TimeCube",
            .Filename = "vscube.dll",
            .Description = "Allows Usage of 3DLuts.",
            .WebURL = "https://github.com/sekrit-twc/timecube",
            .VsFilterNames = {"timecube.Cube"}})

        Add(New PluginPackage With {
            .Name = "DegrainMedian",
            .Filename = "libdegrainmedian.dll",
            .Description = "VapourSynth port of DegrainMedian.",
            .WebURL = "https://github.com/dubhater/vapoursynth-degrainmedian",
            .VsFilterNames = {"dgm.DegrainMedian"}})

        Add(New PluginPackage With {
            .Name = "psharpen",
            .Filename = "psharpen.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "VapourSynth port of pSharpen.",
            .VsFilterNames = {"psharpen.psharpen"}})

        Add(New PluginPackage With {
            .Name = "AWarpSharp2",
            .Filename = "libawarpsharp2.dll",
            .Description = "VapourSynth port of AWarpSharp2.",
            .WebURL = "https://github.com/dubhater/vapoursynth-awarpsharp2",
            .VsFilterNames = {"warp.AWarpSharp2"}})

        Add(New PluginPackage With {
            .Name = "fmtconv",
            .Filename = "fmtconv.dll",
            .WebURL = "http://github.com/EleonoreMizo/fmtconv",
            .HelpFilename = "doc\fmtconv.html",
            .Description = "Fmtconv is a format-conversion plug-in for the Vapoursynth video processing engine. It does resizing, bitdepth conversion with dithering and colorspace conversion.",
            .VsFilterNames = {"fmtc.bitdepth", "fmtc.convert", " core.fmtc.matrix", "fmtc.resample", "fmtc.transfer", "fmtc.primaries", " core.fmtc.matrix2020cl", "fmtc.stack16tonative", "nativetostack16"}})

        Add(New PluginPackage With {
            .Name = "finesharp",
            .Filename = "finesharp.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "Port of Didée's FineSharp script to VapourSynth.",
            .WebURL = "http://forum.doom9.org/showthread.php?p=1777860#post1777860",
            .VsFilterNames = {"finesharp.sharpen"}})

        Add(New PluginPackage With {
            .Name = "FineSharp",
            .Filename = "FineSharp.avsi",
            .Description = "Small and relatively fast realtime-sharpening function, for 1080p, or after scaling 720p -> 1080p during playback (to make 720p look more like being 1080p). It's a generic sharpener. Only for good quality sources!",
            .WebURL = "http://avisynth.nl/index.php/FineSharp",
            .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.5%20and%20up/FineSharp.avsi",
            .AvsFilterNames = {"FineSharp"}})

        Add(New PluginPackage With {
            .Name = "CropResize",
            .Filename = "CropResize.avsi",
            .Description = "Advanced crop and resize AviSynth script.",
            .WebURL = "https://forum.videohelp.com/threads/393752-CropResize-Cropping-resizing-script",
            .AvsFilterNames = {"CropResize"}})

        Add(New PluginPackage With {
            .Name = "CAS",
            .Filename = "CAS.dll",
            .Description = "Contrast Adaptive Sharpening. This is a port of the VapourSynth plugin CAS.",
            .WebURL = "https://github.com/Asd-g/AviSynth-CAS",
            .HelpURL = "http://avisynth.nl/index.php/CAS",
            .DownloadURL = "https://github.com/Asd-g/AviSynth-CAS/releases",
            .AvsFilterNames = {"CAS"}})

        Add(New PluginPackage With {
            .Name = "TTempSmooth",
            .Filename = "TTempSmooth.dll",
            .Description = "TTempSmooth is a motion adaptive (it only works on stationary parts of the picture), temporal smoothing filter. TTempSmoothF is a faster (50-75%) version of TTempSmooth that doesn't take the mdiff/mdiffC parameters (it is equivalent to running TTempSmooth with mdiff/mdiffC set equal to or greater then LThresh-1/CThresh-1).",
            .WebURL = "http://avisynth.nl/index.php/TTempSmooth",
            .AvsFilterNames = {"TTempSmooth", "TTempSmoothF"}})

        Add(New PluginPackage With {
            .Name = "TTempSmooth",
            .Filename = "TTempSmooth.dll",
            .Description = "VapourSynth port of TTempSmooth.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-TTempSmooth",
            .VsFilterNames = {"ttmpsm.TTempSmooth"}})

        Add(New PluginPackage With {
            .Name = "vsTTempSmooth",
            .Filename = "vsTTempSmooth.dll",
            .Description = "TTempSmooth is a motion adaptive (it only works on stationary parts of the picture), temporal smoothing filter. This is a port of the VapourSynth plugin TTempSmooth.",
            .WebURL = "https://github.com/Asd-g/AviSynth-vsTTempSmooth",
            .HelpURL = "http://avisynth.nl/index.php/VsTTempSmooth",
            .HelpFilename = "README.md",
            .DownloadURL = "https://github.com/Asd-g/AviSynth-vsTTempSmooth/releases",
            .AvsFilterNames = {"vsTTempSmooth"}})

        Add(New PluginPackage With {
            .Name = "CASm",
            .Filename = "CASm.avsi",
            .Location = "Plugins\AVS\Scripts",
            .Description = "An improved sharpening script based on CAS and aWarpSharp2. Originally composed by Atak_Snajpera for RipBot264, and renamed/modded by JKyle for StaxRip.",
            .WebURL = "https://forum.doom9.org/showthread.php?p=1942236#post1942236",
            .Dependencies = {"Plugins_JPSDR.dll"},
            .AvsFilterNames = {"CASm"}})

        Add(New PluginPackage With {
            .Name = "AddGrain",
            .Filename = "AddGrain.dll",
            .Description = "AddGrain generates film like grain or other effects (like rain) by adding random noise to a video clip. This noise may optionally be horizontally or vertically correlated to cause streaking. VapourSynth port of AddGrainC.",
            .WebURL = "https://github.com/HomeOfVapourSynthEvolution/VapourSynth-AddGrain",
            .VsFilterNames = {"grain.Add"}})

        Add(New PluginPackage With {
            .Name = "Bwdif",
            .Filename = "BWDIF.dll",
            .Description = "Motion adaptive deinterlacing based on yadif with the use of w3fdif and cubic interpolation algorithms. This is a port of the VapourSynth plugin Bwdif, which is again a port of FFmpeg's libavfilter bwdif.",
            .WebURL = "https://github.com/Asd-g/AviSynth-BWDIF",
            .HelpURL = "http://avisynth.nl/index.php/BWDIF",
            .HelpFilename = "README.md",
            .DownloadURL = "https://github.com/Asd-g/AviSynth-BWDIF/releases",
            .AvsFilterNames = {"BWDIF"}})

        Add(New PluginPackage With {
            .Name = "FillBorders",
            .Filename = "libfillborders.dll",
            .Description = "This is a simple filter that fills the borders of a clip, without changing the clip's dimensions.",
            .WebURL = "https://github.com/dubhater/vapoursynth-fillborders",
            .VsFilterNames = {"fb.FillBorders"}})

        Add(New PluginPackage With {
            .Name = "FillBorders",
            .Filename = "FillBorders.dll",
            .Description = "A simple filter that fills the borders of a clip, without changing the clip's dimensions. This is a port of the VapourSynth plugin FillBorders.",
            .WebURL = "https://github.com/Asd-g/AviSynth-FillBorders",
            .HelpURL = "http://avisynth.nl/index.php/FillBorders",
            .HelpFilename = "README.md",
            .DownloadURL = "https://github.com/Asd-g/AviSynth-FillBorders/releases",
            .AvsFilterNames = {"FillBorders", "FillMargins"}})

        Add(New PluginPackage With {
            .Name = "DeRainbow",
            .Filename = "DeRainbow.avsi",
            .Location = "Plugins\AVS\Scripts",
            .Description = "A simple script to reduce rainbows.",
            .WebURL = "http://forum.doom9.org/showthread.php?p=398106#post398106",
            .HelpURL = "http://avisynth.nl/index.php/DeRainbow",
            .AvsFilterNames = {"DeRainbow", "DeRainbowYUY2"}})

        Add(New PluginPackage With {
            .Name = "DotKill",
            .Filename = "DotKill.dll",
            .Description = "A spatio-temporal dotcrawl and rainbow remover for AviSynth/AviSynth+. This is a port of the VapourSynth plugin DotKill.",
            .WebURL = "https://github.com/Asd-g/AviSynth-DotKill",
            .HelpURL = "http://avisynth.nl/index.php/DotKill",
            .HelpFilename = "README.md",
            .DownloadURL = "https://github.com/Asd-g/AviSynth-DotKill/releases",
            .AvsFilterNames = {"DotKillS", "DotKillZ", "DotKillT"}})

        Add(New PluginPackage With {
            .Name = "DotKill",
            .Filename = "DotKill.dll",
            .Description = "A spatio-temporal dotcrawl and rainbow remover for VapourSynth.",
            .WebURL = "https://github.com/myrsloik/DotKill",
            .VsFilterNames = {"dotkill.DotKillS", "dotkill.DotKillZ", "dotkill.DotKillT"}})

        Add(New PluginPackage With {
            .Name = "vsCnr2",
            .Filename = "vsCnr2.dll",
            .Description = "vsCnr2 is a temporal denoiser designed to denoise only the chroma, suited for stationary rainbows or noisy analog captures. This is a port of the VapourSynth plugin Cnr2.",
            .WebURL = "https://github.com/Asd-g/AviSynth-vsCnr2",
            .HelpURL = "http://avisynth.nl/index.php/VsCnr2",
            .HelpFilename = "README.md",
            .DownloadURL = "https://github.com/Asd-g/AviSynth-vsCnr2/releases",
            .AvsFilterNames = {"vsCnr2"}})

        Add(New PluginPackage With {
            .Name = "TimecodeFPS",
            .Filename = "TimecodeFPS.dll",
            .Description = "An AviSynth plugin to convert VFR(variable frame rate) to CFR(constant frame rate) using MKV timecodes. This filter works only with FFmpegSource2 plugin filters, that is, FFMS2 and FFVideoSource source filters.",
            .WebURL = "https://github.com/Asd-g/TimecodeFPS",
            .HelpURL = "http://avisynth.nl/index.php/TimecodeFPS",
            .DownloadURL = "https://github.com/Asd-g/TimecodeFPS/releases",
            .AvsFilterNames = {"TimecodeFPS"}})

        Add(New PluginPackage With {
            .Name = "VfrToCfr",
            .Filename = "vfrtocfr.dll",
            .Description = "This plugin converts variable frame rate clips to constant frame rate by inserting null frames (exact copies of previous frames). This filter works only with FFmpegSource2 plugin filters, that is, FFMS2 and FFVideoSource source filters.",
            .WebURL = "https://github.com/jojje/VfrToCfr-the-other-one",
            .HelpURL = "http://avisynth.nl/index.php/VfrToCfr",
            .DownloadURL = "https://github.com/jojje/VfrToCfr-the-other-one/releases",
            .AvsFilterNames = {"VfrToCfr"}})

        Add(New PluginPackage With {
            .Name = "GRunT",
            .Filename = "grunt.dll",
            .Description = "Gavino's Run-Time for AviSynth.",
            .WebURL = "https://github.com/pinterf/GRunT",
            .HelpURL = "http://avisynth.nl/index.php/GRunT",
            .DownloadURL = "https://github.com/pinterf/GRunT/releases",
            .AvsFilterNames = {"GScriptClip", "GFrameEvaluate", "GConditionalFilter", "GWriteFile", "GWriteFileIf"}})

        Add(New PluginPackage With {
            .Name = "Srestore",
            .Filename = "Srestore.avsi",
            .Description = "An AviSynth field-blending/frame-blending removal script function that uses conditional frame evaluation for the output calculation.",
            .WebURL = "https://forum.doom9.org/showthread.php?p=1944371#post1944371",
            .HelpURL = "http://avisynth.nl/index.php/Srestore",
            .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.5%20and%20up/Srestore.avsi",
            .AvsFilterNames = {"Srestore"}})

        Add(New PluginPackage With {
            .Name = "ReduceFlicker",
            .Filename = "ReduceFlicker.dll",
            .Description = "A AviSynth 2.6/AviSynth+ plugin to reduce temporal oscillations.",
            .WebURL = "https://github.com/Asd-g/ReduceFlicker",
            .HelpURL = "http://avisynth.nl/index.php/ReduceFlicker",
            .HelpFilename = "readme.md",
            .DownloadURL = "https://github.com/Asd-g/ReduceFlicker/releases",
            .AvsFilterNames = {"ReduceFlicker"}})

        Add(New PluginPackage With {
            .Name = "VFRToCFR",
            .Filename = "VFRToCFRVapoursynth.dll",
            .Description = "Converts Variable Frame Rate (VFR) video to a Constant Frame Rate (CFR) video through the use of Matroska Version 2 Timecodes. Ported from the Avisynth plugin written by Nicholai Main.",
            .WebURL = "https://github.com/Irrational-Encoding-Wizardry/Vapoursynth-VFRToCFR",
            .VsFilterNames = {"vfrtocfr.VFRToCFR"}})

        Add(New PluginPackage With {
            .Name = "Bifrost",
            .Filename = "Bifrost.dll",
            .Description = "A temporal derainbowing filter created by Fredrik Mellbin. The original Avisynth plugin (version 1.1) worked on the whole frame or not at all. This version works on blocks, meaning that static parts of the image can be processed even if something moves on screen.",
            .WebURL = "https://github.com/Asd-g/AviSynth-bifrost",
            .HelpURL = "http://avisynth.nl/index.php/Bifrost",
            .HelpFilename = "README.md",
            .DownloadURL = "https://github.com/Asd-g/AviSynth-bifrost/releases",
            .AvsFilterNames = {"Bifrost"}})

        Add(New PluginPackage With {
            .Name = "Bifrost",
            .Filename = "libbifrost.dll",
            .Description = "This is a VapourSynth port of Bifrost, a temporal derainbowing filter created by Fredrik Mellbin. The original Avisynth plugin (version 1.1) worked on the whole frame or not at all. This version works on blocks, meaning that static parts of the image can be processed even if something moves on screen. This VapourSynth plugin will accept any 8 bit YUV formats.",
            .WebURL = "https://github.com/dubhater/vapoursynth-bifrost",
            .VsFilterNames = {"bifrost.Bifrost"}})

        Add(New PluginPackage With {
            .Name = "ChubbyRain2",
            .Filename = "ChubbyRain2.avsi",
            .Location = "Plugins\AVS\Scripts",
            .Description = "A spatio-temporal rainbow reducing script composed by Lothar based on Mug Funky's ChubbyRain. Included is a mod version by Asd-g that accepts progressive sources only.",
            .DownloadURL = "https://github.com/JJKylee/Filter-Scripts/blob/main/AviSynth/ChubbyRain2.avsi",
            .WebURL = "https://forum.doom9.org/showthread.php?p=1945063#post1945063",
            .HelpURL = "http://avisynth.nl/index.php/ChubbyRain2",
            .AvsFilterNames = {"ChubbyRain2"}})

        Add(New PluginPackage With {
            .Name = "ChubbyRain3",
            .Filename = "ChubbyRain3.avsi",
            .Location = "Plugins\AVS\Scripts",
            .Description = "A spatio-temporal rainbow reducing script composed by Lothar based on Mug Funky's ChubbyRain. Included is a mod version by Asd-g that accepts progressive sources only.",
            .DownloadURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.5%20and%20up/ChubbyRain3.avsi",
            .WebURL = "https://github.com/realfinder/AVS-Stuff/blob/Community/avs%202.5%20and%20up/ChubbyRain3.avsi",
            .HelpURL = "http://avisynth.nl/index.php/ChubbyRain2",
            .AvsFilterNames = {"ChubbyRain3"}})

        Add(New PluginPackage With {
            .Name = "DeFlicker",
            .Filename = "Deflicker.dll",
            .Description = "DeFlicker plugin can remove old film intensity flicker by temporal mean luma smoothing. Also it can correct blinding of automatic gain control after flashes. The luma stabilizing not only improves visual impression, but can help to following noise reduction.",
            .WebURL = "https://github.com/pinterf/Deflicker",
            .HelpURL = "http://avisynth.nl/index.php/DeFlicker",
            .DownloadURL = "https://github.com/pinterf/Deflicker/releases",
            .AvsFilterNames = {"Deflicker"}})

        Add(New PluginPackage With {
            .Name = "FillDrops",
            .Filename = "FillDrops.avsi",
            .Location = "Plugins\AVS\Scripts",
            .Description = "An AviSynth filter to detect and replace duplicate frames with a motion-interpolation of the adjacent frames. Included is a mod by Selur.",
            .WebURL = "https://forum.videohelp.com/threads/402416-repair-Videos-with-duplicates-and-dropped-frames#post2624936",
            .DownloadURL = "https://github.com/JJKylee/Filter-Scripts/blob/main/AviSynth/FillDrops.avsi",
            .AvsFilterNames = {"FillDrops"}})

        Add(New PluginPackage With {
            .Name = "filldrops",
            .Filename = "filldrops.py",
            .Location = "Plugins\VS\Scripts",
            .Description = "A filter to detect and replace duplicate frames with a motion-interpolation of the adjacent frames. This is a VapourSynth port of FillDrops by Selur and Myrsloik, modded by JKyle.",
            .WebURL = "https://forum.doom9.org/showthread.php?p=1947291#post1947291",
            .DownloadURL = "https://github.com/JJKylee/Filter-Scripts/blob/main/VapourSynth/filldrops.py",
            .VsFilterNames = {"filldrops.FillDrops"}})

        Add(New PluginPackage With {
            .Name = "DeJump",
            .Filename = "DeJump.avsi",
            .Location = "Plugins\AVS\Scripts",
            .Description = "An AviSynth filter that smooths out jumps and drops preserving the original frame rate. Composed and modded by JKyle based on John Meyer's script.",
            .WebURL = "https://forum.videohelp.com/threads/402416-repair-Videos-with-duplicates-and-dropped-frames#post2625159",
            .DownloadURL = "https://github.com/JJKylee/Filter-Scripts/blob/main/AviSynth/DeJump.avsi",
            .AvsFilterNames = {"DeJump"}})

        Add(New PluginPackage With {
            .Name = "RT_Stats",
            .Filename = "RT_Stats_x64.dll",
            .Description = "StainlessS's compile-time/runtime functions for AviSynth.",
            .WebURL = "https://forum.doom9.org/showthread.php?t=165479",
            .HelpURL = "http://avisynth.nl/index.php/RT_Stats",
            .DownloadURL = "https://www.mediafire.com/folder/hb26mthbjz7z6/StainlessS",
            .AvsFilterNames = {"RT_Stats", "RT_GraphLink", "RT_Subtitle", "RT_FSelOpen", "RT_FSelSaveAs", "RT_FSelFolder", "RT_Debug", "RT_DebugF", "RT_Call", "RT_YankChain", "RT_ForceProcess", "RT_ColorSpaceXMod", "RT_ColorSpaceYMod", "RT_GetProcessName", "RT_GetPid", "RT_GetLastError", "RT_GetLastErrorString", "RT_Version", "RT_VersionString", "RT_VersionDll", "RT_GetSystemEnv", "RT_RandInt", "RT_LocalTimeString", "RT_StrAddStr", "RT_StrReplace", "RT_StrReplaceDeep", "RT_StrReplaceMulti", "RT_QuoteStr", "RT_StrPad", "RT_FindStr", "RT_TxtAddStr", "RT_TxtQueryLines", "RT_TxtGetLine", "RT_TxtFindStr", "RT_String", "RT_TxtSort", "RT_GetWorkingDir", "RT_FilenameSplit", "RT_GetFullPathName", "RT_GetFileExtension", "RT_FileRename", "RT_FileQueryLines", "RT_WriteFile", "RT_TxtWriteFile", "RT_ReadTxtFromFile", "RT_WriteFileList", "RT_FileDelete", "RT_FileDuplicate", "RT_FileFindStr", "RT_GetFileTime", "RT_Hex", "RT_HexValue", "RT_NumberString", "RT_NumberValue", "RT_BitNOT", "RT_BitAND", "RT_BitOR", "RT_BitXOR", "RT_BitLSL", "RT_BitLSR", "RT_BitASL", "RT_BitASR", "RT_BitTST", "RT_BitCLR", "RT_BitSET", "RT_BitCHG", "RT_BitROR", "RT_BitROL", "RT_BitSetCount", "RT_ArrayAlloc", "RT_ArrayGetDim", "RT_ArrayGetType", "RT_ArrayGetElSize", "RT_ArrayGetDim1Max", "RT_ArrayGet", "RT_ArraySet", "RT_ArrayExtend", "RT_ArrayAppend", "RT_ArrayGetAttrib", "RT_ArraySetAttrib", "RT_ArrayGetStrAttrib", "RT_ArraySetStrAttrib", "RT_ArrayTypeName", "RT_ArrayGetID", "RT_ArraySetID", "RT_ArrayCheckID", "RT_DBaseAlloc", "RT_DBaseRecords", "RT_DBaseRecordSize", "RT_DBaseFields", "RT_DBaseFieldType", "RT_DBaseFieldSize", "RT_DBaseRecordsMax", "RT_DBaseGetTypeString", "RT_DBaseGetField", "RT_DBaseSetField", "RT_DBaseSet", "RT_DBaseExtend", "RT_DBaseAppend", "RT_DBaseGetAttrib", "RT_DBaseSetAttrib", "RT_DBaseGetStrAttrib", "RT_DBaseSetStrAttrib", "RT_DBaseTypeName", "RT_DBaseGetID", "RT_DBaseSetID", "RT_DBaseCheckID", "RT_DBaseCmpField", "RT_DBaseRecordSwap", "RT_DBaseQuickSort", "RT_DBaseInsertSort", "RT_DBaseReadCSV", "RT_DBaseWriteCSV", "RT_DBaseFindSeq", "RT_DBaseRgbColorCount", "RT_DBaseDeleteRecord", "RT_Undefined", "RT_VarExist", "RT_FunctionExist", "RT_Ord", "RT_Timer", "RT_TimerHP", "RT_Sleep", "RT_IncrGlobal", "RT_ScriptDir", "RT_ScriptFile", "RT_ScriptName", "RT_PluginDir", "RT_InternalFunctions", "RT_PluginFunctions", "RT_PluginParam", "RT_VarType", "RT_VarIsSame", "RT_FloatAsRGBA", "RT_RGB32AsFloat", "RT_RGB32AsInt", "RT_YPlaneMin", "RT_YPlaneMax", "RT_YPlaneMinMaxDifference", "RT_YPlaneMedian", "RT_AverageLuma", "RT_YPlaneStdev", "RT_YInRange", "RT_YPNorm", "RT_Ystats", "RT_RgbChanMin", "RT_RgbChanMax", "RT_RgbChanMinMaxDifference", "RT_RgbChanMedian", "RT_RgbChanAve", "RT_RgbChanStdev", "RT_RgbChanInRange", "RT_RgbChanPNorm", "RT_RgbChanStats", "RT_RgbInRange", "RT_ChanAve", "RT_AvgLumaDif", "RT_YDifference", "RT_LumaDifference", "RT_LumaCorrelation", "RT_LumaPixelsDifferent", "RT_LumaPixelsDifferentCount", "RT_FrameDifference", "RT_FrameMovement", "RT_QueryColor", "RT_GetSAR", "RT_GetDAR", "RT_SignalDAR", "RT_SignalDAR2", "RT_GetCropDAR", "RT_QueryLumaMinMax", "RT_QueryBorderCrop", "RT_YInRangeLocate", "RT_RgbInRangeLocate", "RT_QwikScanCreate", "RT_QwikScan", "RT_QwikScanEstimateLumaTol"}})

        Add(New PluginPackage With {
            .Name = "ExactDedup",
            .Filename = "ExactDeDup_x64.dll",
            .Description = "ExactDedup is an AviSynth filter intended to remove frames that are exact duplicates of each other, leaving only the first and (optionally) last frames of a run intact, and generates a Matroska v2 timecodes file with timing information for the ensuing stream. Due to the particulars of the AviSynth API, ExactDedup requires two passes to function efficiently. It could theoretically be designed to run with one pass, but this would result in a very long startup delay while the plugin precomputes all of the duplicate frame information and timecodes.",
            .WebURL = "https://forum.doom9.org/showthread.php?t=176111",
            .HelpURL = "http://avisynth.nl/index.php/ExactDedup",
            .DownloadURL = "https://www.mediafire.com/folder/hb26mthbjz7z6/StainlessS",
            .AvsFilterNames = {"ExactDedup"}})

        Add(New PluginPackage With {
            .Name = "Santiag",
            .Filename = "Santiag.avsi",
            .Description = "Simple anti-aliasing with independent horizontal and vertical strength.",
            .WebURL = "https://forum.doom9.org/showthread.php?p=1393006#post1393006",
            .HelpURL = "http://avisynth.nl/index.php/Santiag",
            .AvsFilterNames = {"santiag"}})

        Add(New Package With {
            .Name = "DirectX 9",
            .Filename = "d3d9.dll",
            .Description = "DirectX 9.0c End-User Runtime.",
            .DownloadURL = "https://www.microsoft.com/en-us/download/details.aspx?id=34429",
            .Location = Folder.System,
            .VersionAllowAny = True,
            .TreePath = "Runtimes",
            .RequiredFunc = Function() AviSynthShader.Required OrElse FFT3DGPU.Required})

        g.RunTask(Sub()
                      SyncLock ConfLock
                          LoadConfAll()
                      End SyncLock
                  End Sub)
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
                    Not plugin.VsFilterNames.NothingOrEmpty Then

                    Return Name + " dual"
                ElseIf Not plugin.AvsFilterNames.NothingOrEmpty Then
                    Return Name + " avs"
                ElseIf Not plugin.VsFilterNames.NothingOrEmpty Then
                    Return Name + " vs"
                End If
            End If

            Return Name
        End Get
    End Property

    Private FilenameValue As String

    Property Filename As String
        Get
            If Not Environment.Is64BitProcess AndAlso Filename32 <> "" Then
                Return Filename32
            End If

            Return FilenameValue
        End Get
        Set(value As String)
            FilenameValue = value
        End Set
    End Property

    Private LaunchActionValue As Action

    Overridable Property LaunchAction As Action
        Get
            If LaunchActionValue Is Nothing Then
                If Description.ContainsEx("GUI app") Then
                    LaunchActionValue = Sub() g.ShellExecute(Path)
                ElseIf Not HelpSwitch Is Nothing Then
                    LaunchActionValue = Sub() g.DefaultCommands.ExecutePowerShellCode(
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
            ElseIf Not plugin.VsFilterNames.NothingOrEmpty Then
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

    Sub SetVersion(versionName As String)
        SetVersionInternal(versionName)

        If Not Siblings.NothingOrEmpty Then
            For Each i In Siblings
                Items(i).SetVersionInternal(versionName)
            Next
        End If
    End Sub

    Sub SetVersionInternal(versionName As String)
        Version = versionName
        VersionDate = File.GetLastWriteTimeUtc(Path)
        SaveConf()
    End Sub

    Sub ShowHelp()
        Dim dic As New SortedDictionary(Of String, String)

        If HelpFilename = "" AndAlso Not HelpSwitch Is Nothing Then
            HelpFilename = Name + " Help.txt"

            If CreateHelpfile() = "" Then
                HelpFilename = ""
            End If
        End If

        If HelpFilename <> "" Then
            If Not HelpSwitch Is Nothing AndAlso HelpFilename = Name + " Help.txt" Then
                dic("Console Help") = Directory + HelpFilename
            Else
                dic("Local Help") = Directory + HelpFilename
            End If
        End If

        dic("Online Help") = HelpURL
        dic("AviSynth Help") = HelpUrlAviSynth
        dic("VapourSynth Help") = HelpUrlVapourSynth

        Dim count = dic.Values.Where(Function(val) val <> "").Count

        If count > 1 Then
            Using td As New TaskDialog(Of String)
                td.Title = "Choose an option"

                For Each pair In dic
                    If pair.Value <> "" Then
                        td.AddCommand(pair.Key, pair.Value)
                    End If
                Next

                If td.Show <> "" Then
                    CreateHelpfile()
                    g.ShellExecute(td.SelectedValue)
                End If
            End Using
        Else
            CreateHelpfile()
            g.ShellExecute(HelpFileOrURL)
        End If
    End Sub

    ReadOnly Property HelpFile As String
        Get
            If HelpFilename = "" AndAlso Not HelpSwitch Is Nothing Then
                HelpFilename = Name + " Help.txt"
            End If

            Return Directory + HelpFilename
        End Get
    End Property

    ReadOnly Property URL As String
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

    ReadOnly Property HelpFileOrURL As String
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
        Catch
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
        Dim ret = ""

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
            Return $"App not found, choose:{BR}Tools > Download (Ctrl+D){BR}Tools > Edit Path (Ctrl+P)"
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
        Return Not Path.PathStartsWith(Folder.System) AndAlso AllowCustomPath
    End Function

    Shared ReadOnly Property x264Type As x264Type
        Get
            Dim filePath = x264.Path

            If filePath <> "" Then
                Dim size = New FileInfo(filePath).Length

                If size <> s.Storage.GetInt("x264 size") Then
                    Dim output = ProcessHelp.GetConsoleOutput(filePath, "--version", False)
                    Dim value As Integer

                    If output.Contains("Patman") Then
                        value = x264Type.Patman
                    ElseIf output.Contains("DJATOM") Then
                        value = x264Type.DJATOM
                    Else
                        value = x264Type.Vanilla
                    End If

                    s.Storage.SetInt("x264 size", CInt(size))
                    s.Storage.SetInt("x264 type", value)
                End If

                Return CType(s.Storage.GetInt("x264 type"), x264Type)
            End If
        End Get
    End Property

    Shared ReadOnly Property x265Type As x265Type
        Get
            Dim filePath = x265.Path

            If filePath <> "" Then
                Dim size = New FileInfo(filePath).Length

                If size <> s.Storage.GetInt("x265 size") Then
                    Dim output = ProcessHelp.GetConsoleOutput(filePath, "--version", True)
                    Dim value As Integer

                    If output.Contains("Patman") Then
                        value = x265Type.Patman
                    ElseIf output.Contains("DJATOM") Then
                        value = x265Type.DJATOM
                    ElseIf output.Contains("JPSDR") Then
                        value = x265Type.JPSDR
                    Else
                        value = x265Type.Vanilla
                    End If

                    s.Storage.SetInt("x265 size", CInt(size))
                    s.Storage.SetInt("x265 type", value)
                End If

                Return CType(s.Storage.GetInt("x265 type"), x265Type)
            End If
        End Get
    End Property

    ReadOnly Property Directory As String
        Get
            Return Path.Dir
        End Get
    End Property

    Sub SetStoredPath(value As String)
        s.Storage.SetString(Name + "custom path", value)
    End Sub

    Function GetStoredPath() As String
        Dim ret = ""

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
        If s.AviSynthMode <> FrameServerMode.Portable Then
            Dim dllPath = FrameServerHelp.GetAviSynthInstallPath

            If dllPath.FileExists Then
                Return dllPath.Dir
            End If
        End If

        Return GetPathFromLocation("FrameServer\AviSynth").Dir
    End Function

    Function GetVapourSynthHintDir() As String
        If s.VapourSynthMode <> FrameServerMode.Portable Then
            Dim dllPath = FrameServerHelp.GetVapourSynthInstallPath

            If dllPath <> "" Then
                Return dllPath.Dir
            End If
        End If

        Return GetPathFromLocation("FrameServer\VapourSynth").Dir
    End Function

    Shared Function GetPythonHintDir() As String
        If Not FrameServerHelp.IsVapourSynthPortable Then
            Dim exePath As String

            For Each key In {Registry.CurrentUser, Registry.LocalMachine}
                For Each keyName In key.GetKeyNames("SOFTWARE\Python\PythonCore")
                    exePath = key.GetString($"SOFTWARE\Python\PythonCore\{keyName}\InstallPath", "ExecutablePath")

                    If File.Exists(exePath) Then
                        Return exePath.Dir
                    End If
                Next
            Next

            exePath = FindEverywhere("python.exe", Python.Exclude(0))

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
                If Not plugin.VsFilterNames Is Nothing AndAlso Not plugin.AvsFilterNames Is Nothing Then
                    ret = Folder.Apps + "Plugins\Dual\" + Name + "\" + Filename

                    If File.Exists(ret) Then
                        Return ret
                    End If
                Else
                    If plugin.VsFilterNames Is Nothing Then
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
                If Exclude.NothingOrEmpty Then
                    ret = FindEverywhere(Filename)
                Else
                    ret = FindEverywhere(Filename, Exclude(0))
                End If

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

    Shared Property ConfLock As New Object
    Shared Property WasConfLoaded As Boolean

    ReadOnly Property ConfPath As String
        Get
            Return Folder.Apps + "Conf\" + ID + ".conf"
        End Get
    End Property

    Function GetConf() As String
        Return FileHelp.ReadAllText(ConfPath)
    End Function

    Shared Sub LoadConfAll()
        If Not WasConfLoaded Then
            WasConfLoaded = True

            For Each i In IO.Directory.GetFiles(Folder.Apps + "Conf")
                If Items.ContainsKey(i.Base) Then
                    Items(i.Base).LoadConf()
                Else
                    FileHelp.Delete(i)
                End If
            Next
        End If
    End Sub

    Sub LoadConf()
        For Each line In GetConf.SplitLinesNoEmpty
            If Not line.Contains("=") Then
                Continue For
            End If

            Dim name = line.Left("=").Trim
            Dim value = line.Right("=").Trim

            Select Case name
                Case "Version"
                    Version = value
                Case "Date"
                    SetVersionDate(value)
            End Select
        Next
    End Sub

    Sub SaveConf()
        Dim sb As New StringBuilder
        sb.Append("Version = " + Version + BR +
                  "Date = " + VersionDate.ToInvariantString("yyyy-MM-dd"))
        sb.ToString.WriteFileUTF8BOM(ConfPath)
    End Sub

    Sub SetVersionDate(value As String)
        Try
            Dim tokens = value.Split("-"c)
            VersionDate = New DateTime(CInt(tokens(0)), CInt(tokens(1)), CInt(tokens(2)))
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub
End Class

Public Class PluginPackage
    Inherits Package

    Property AvsFilterNames As String()
    Property VsFilterNames As String()
    Property Dependencies As String()

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

        If p.Script.IsAviSynth AndAlso Not package.AvsFilterNames.NothingOrEmpty Then
            Dim scriptLower = p.Script.GetScript().ToLowerInvariant

            For Each filterName In package.AvsFilterNames
                If scriptLower.Contains(filterName.ToLowerInvariant + "(") Then Return True

                If scriptLower.Contains("import") Then
                    Dim match = Regex.Match(scriptLower, "\bimport\s*\(\s*""\s*(.+\.avsi*)\s*""\s*\)", RegexOptions.IgnoreCase)

                    If match.Success AndAlso File.Exists(match.Groups(1).Value) Then
                        If match.Groups(1).Value.ReadAllText.ToLowerInvariant.Contains(
                                filterName.ToLowerInvariant) Then

                            Return True
                        End If
                    End If
                End If
            Next
        ElseIf p.Script.Engine = ScriptEngine.VapourSynth AndAlso
            Not package.VsFilterNames.NothingOrEmpty Then

            For Each ifilter In p.Script.Filters
                If ifilter.Active Then
                    For Each filterName In package.VsFilterNames
                        If ifilter.Script.Contains(filterName) Then
                            Return True
                        End If
                    Next
                End If
            Next
        End If
    End Function
End Class
