Imports Microsoft.Win32
Imports StaxRip

Class Packs
    Shared Property AviSynth As New AviSynthPlusPackage
    Shared Property AVSMeter As New AVSMeterPackage
    Shared Property BDSup2SubPP As New BDSup2SubPackage
    Shared Property BeSweet As New BeSweetPackage
    Shared Property checkmate As New checkmatePackage
    Shared Property DGDecodeIM As New DGDecodeIMPackage
    Shared Property DGIndexIM As New DGIndexIMPackage
    Shared Property DGIndexNV As New DGIndexNVPackage
    Shared Property DivX265 As New DivX265Package
    Shared Property dsmux As New dsmuxPackage
    Shared Property DSS2mod As New DSS2modPackage
    Shared Property eac3to As New eac3toPackage
    Shared Property ffmpeg As New ffmpegPackage
    Shared Property ffms2 As New ffms2Package
    Shared Property Haali As New HaaliSplitter
    Shared Property Java As New JavaPackage
    Shared Property lsmashWorks As New LSmashWorksAviSynthPackage
    Shared Property MediaInfo As New MediaInfoPackage
    Shared Property Mkvmerge As New MKVToolNixPackage
    Shared Property MP4Box As New MP4BoxPackage
    Shared Property MPC As New MPCPackage
    Shared Property NeroAACEnc As New NeroAACEncPackage
    Shared Property NicAudio As New NicAudioPackage
    Shared Property nnedi3 As New nnedi3Package
    Shared Property NVEncC As New NVEncCPackage
    Shared Property ProjectX As New ProjectXPackage
    Shared Property qaac As New qaacPackage
    Shared Property SangNom2 As New SangNom2Package
    Shared Property TDeint As New TDeintPackage
    Shared Property UnDot As New UnDotPackage
    Shared Property VSRip As New VSRipPackage
    Shared Property x264 As New x264Package
    Shared Property x265 As New x265Package
    Shared Property xvid_encraw As New xvid_encrawPackage
    Shared Property flash3kyuu_deband As New flash3kyuu_debandPackage
    Shared Property Decomb As New DecombPackage
    Shared Property vinverse As New vinversePackage
    Shared Property vspipe As New vspipePackage
    Shared Property VapourSynth As New VapourSynthPackage
    Shared Property Python As New PythonPackage
    Shared Property scenechange As New scenechangePackage
    Shared Property temporalsoften As New temporalsoftenPackage
    Shared Property LSmashWorksVapourSynth As New vslsmashsourcePackage

    Shared Property QSVEncC As New Package With {
        .Name = "QSVEncC",
        .Filename = "QSVEncC64.exe",
        .Description = "Intel hardware accelerated H.264, H.265 and MPEG2 encoder.",
        .HelpFile = "help.txt",
        .WebURL = "https://onedrive.live.com/?cid=6bdd4375ac8933c6&id=6BDD4375AC8933C6!482"}

    Shared Property VCEEncC As New Package With {
        .Name = "VCEEncC",
        .Filename = "VCEEncC64.exe",
        .Description = "AMD GPU accelerated H.264 encoder.",
        .HelpFile = "help.txt",
        .WebURL = "https://www.dropbox.com/sh/c4q2ekr269fd4w9/6zuEnjBI-Q"}

    Shared Property vscpp2013 As New Package With {
        .Name = "Visual C++ 2013",
        .Filename = "msvcp120.dll",
        .Description = "Visual C++ 2013 Redistributable Packages which is required by some tools used by StaxRip.",
        .DownloadURL = "https://www.microsoft.com/en-US/download/details.aspx?id=40784",
        .FixedDir = CommonDirs.System}

    Shared Property vscpp2015 As New Package With {
        .Name = "Visual C++ 2015",
        .Filename = "msvcp140.dll",
        .Description = "Visual C++ 2015 Redistributable Packages which is required by some tools used by StaxRip.",
        .DownloadURL = "http://download.microsoft.com/download/8/c/b/8cb4af84-165e-4b36-978d-e867e07fc707/vc_redist.x64.exe",
        .FixedDir = CommonDirs.System}

    Public Shared DGDecodeNV As New PluginPackage With {
        .Name = "DGDecodeNV",
        .Filename = "DGDecodeNV.dll",
        .WebURL = "http://rationalqm.us/dgdecnv/dgdecnv.html",
        .Description = Strings.DGDecNV,
        .HelpFile = "DGDecodeNVManual.html",
        .AviSynthFilterNames = {"DGSource"},
        .AviSynthFiltersFunc = Function() {New VideoFilter("Source", "DGSource", "DGSource(""%source_file%"")"),
                                           New VideoFilter("Source", "DGSourceIM", "DGSourceIM(""%source_file%"")")},
        .IsRequiredFunc = Function() p.Script.Filters(0).Script.Contains("DGSource(")}

    Public Shared Property Packages As New List(Of Package)

    Shared Sub New()
        Packages.Add(VCEEncC)
        Packages.Add(LSmashWorksVapourSynth)
        Packages.Add(temporalsoften)
        Packages.Add(scenechange)
        Packages.Add(Python)
        Packages.Add(VapourSynth)
        Packages.Add(vspipe)
        Packages.Add(vinverse)
        Packages.Add(Decomb)
        Packages.Add(flash3kyuu_deband)
        Packages.Add(AviSynth)
        Packages.Add(AVSMeter)
        Packages.Add(BDSup2SubPP)
        Packages.Add(BeSweet)
        Packages.Add(checkmate)
        Packages.Add(DGDecodeIM)
        Packages.Add(DGDecodeNV)
        Packages.Add(DGIndexIM)
        Packages.Add(DGIndexNV)
        Packages.Add(DivX265)
        Packages.Add(dsmux)
        Packages.Add(DSS2mod)
        Packages.Add(eac3to)
        Packages.Add(ffmpeg)
        Packages.Add(ffms2)
        Packages.Add(Haali)
        Packages.Add(Java)
        Packages.Add(lsmashWorks)
        Packages.Add(MediaInfo)
        Packages.Add(Mkvmerge)
        Packages.Add(MP4Box)
        Packages.Add(MPC)
        Packages.Add(NeroAACEnc)
        Packages.Add(NicAudio)
        Packages.Add(nnedi3)
        Packages.Add(NVEncC)
        Packages.Add(ProjectX)
        Packages.Add(qaac)
        Packages.Add(QSVEncC)
        Packages.Add(SangNom2)
        Packages.Add(TDeint)
        Packages.Add(UnDot)
        Packages.Add(VSRip)
        Packages.Add(x264)
        Packages.Add(x265)
        Packages.Add(xvid_encraw)
        Packages.Add(vscpp2013)
        Packages.Add(vscpp2015)

        Packages.Add(New PluginPackage With {
            .Name = "adjust",
            .Filename = "adjust.py",
            .VapourSynthFilterNames = {"adjust.Tweak"},
            .Description = "very basic port of the built-in Avisynth filter Tweak.",
            .WebURL = "https://github.com/dubhater/vapoursynth-adjust",
            .HelpURL = "https://github.com/dubhater/vapoursynth-adjust"})

        Packages.Add(New PluginPackage With {
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

        Packages.Add(New PluginPackage With {
            .Name = "havsfunc",
            .Filename = "havsfunc.py",
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Field", "QTGMC | QTGMC Fast", "clip = havsfunc.QTGMC(clip, TFF = True, Preset = ""Fast"")"),
                New VideoFilter("Field", "QTGMC | QTGMC Medium", "clip = havsfunc.QTGMC(clip, TFF = True, Preset = ""Medium"")"),
                New VideoFilter("Field", "QTGMC | QTGMC Slow", "clip = havsfunc.QTGMC(clip, TFF = True, Preset = ""Slow"")")},
            .VapourSynthFilterNames = {"havsfunc.QTGMC", "havsfunc.ediaa", "havsfunc.daa", "havsfunc.maa",
                                      "havsfunc.SharpAAMCmod", "havsfunc.Deblock_QED", "havsfunc.DeHalo_alpha",
                                      "havsfunc.YAHR", "havsfunc.HQDeringmod", "havsfunc.ivtc_txt60mc",
                                      "havsfunc.Vinverse", "havsfunc.Vinverse2", "havsfunc.logoNR",
                                      "havsfunc.LUTDeCrawl", "havsfunc.LUTDeRainbow", "havsfunc.GSMC",
                                      "havsfunc.SMDegrain", "havsfunc.SmoothLevels", "havsfunc.FastLineDarkenMOD",
                                      "havsfunc.LSFmod", "havsfunc.GrainFactory3"},
            .Dependencies = {"fmtconv", "mvtools", "nnedi3", "scenechange", "temporalsoften", "mvsfunc"},
            .Description = "Various popular AviSynth scripts ported to VapourSynth.",
            .HelpURL = "http://forum.doom9.org/showthread.php?t=166582"})

        Packages.Add(New PluginPackage With {
            .Name = "KNLMeansCL",
            .Filename = "KNLMeansCL.dll",
            .WebURL = "http://forum.doom9.org/showthread.php?t=171379",
            .HelpFile = "DOC.txt",
            .Description = "KNLMeansCL is an optimized pixelwise OpenCL implementation of the Non-local means denoising algorithm. Every pixel is restored by the weighted average of all pixels in its search window. The level of averaging is determined by the filtering parameter h.",
            .VapourSynthFilterNames = {"knlm.KNLMeansCL"},
            .AviSynthFilterNames = {"KNLMeansCL"},
            .AviSynthFiltersFunc = Function() {
                New VideoFilter("Noise", "KNLMeansCL | Spatial Light", "KNLMeansCL(D = 0, A = 2, h = 2)"),
                New VideoFilter("Noise", "KNLMeansCL | Spatial Medium", "KNLMeansCL(D = 0, A = 4, h = 4)"),
                New VideoFilter("Noise", "KNLMeansCL | Spatial Strong", "KNLMeansCL(D = 0, A = 6, h = 6)"),
                New VideoFilter("Noise", "KNLMeansCL | Temporal Light", "KNLMeansCL(D = 1, A = 1, h = 3)"),
                New VideoFilter("Noise", "KNLMeansCL | Temporal Medium", "KNLMeansCL(D = 1, A = 1, h = 6)"),
                New VideoFilter("Noise", "KNLMeansCL | Temporal Strong", "KNLMeansCL(D = 1, A = 1, h = 9)"),
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Light", "KNLMeansCL(D = 1, A = 1, h = 2)"),
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Medium", "KNLMeansCL(D = 1, A = 1, h = 4)"),
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Strong", "KNLMeansCL(D = 1, A = 1, h = 8)")},
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Noise", "KNLMeansCL | Spatial Light", "clip = core.knlm.KNLMeansCL(clip, d = 0, a = 2, h = 2)"),
                New VideoFilter("Noise", "KNLMeansCL | Spatial Medium", "clip = core.knlm.KNLMeansCL(clip, d = 0, a = 4, h = 4)"),
                New VideoFilter("Noise", "KNLMeansCL | Spatial Strong", "clip = core.knlm.KNLMeansCL(clip, d = 0, a = 6, h = 6)"),
                New VideoFilter("Noise", "KNLMeansCL | Temporal Light", "clip = core.knlm.KNLMeansCL(clip, d = 1, a = 1, h = 3)"),
                New VideoFilter("Noise", "KNLMeansCL | Temporal Medium", "clip = core.knlm.KNLMeansCL(clip, d = 1, a = 1, h = 6)"),
                New VideoFilter("Noise", "KNLMeansCL | Temporal Strong", "clip = core.knlm.KNLMeansCL(clip, d = 1, a = 1, h = 9)"),
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Light", "clip = core.knlm.KNLMeansCL(clip, d = 1, a = 1, h = 2)"),
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Medium", "clip = core.knlm.KNLMeansCL(clip, d = 1, a = 1, h = 4)"),
                New VideoFilter("Noise", "KNLMeansCL | Spatio-Temporal Strong", "clip = core.knlm.KNLMeansCL(clip, d = 1, a = 1, h = 8)")}})

        Packages.Add(New PluginPackage With {
            .Name = "d2vsource",
            .Filename = "d2vsource.dll",
            .VapourSynthFilterNames = {"d2v.Source"},
            .Description = "D2V parser and decoder for VapourSynth.",
            .WebURL = "https://github.com/dwbuiten/d2vsource",
            .HelpURL = "https://github.com/dwbuiten/d2vsource",
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Source", "d2vsource", "clip = core.d2v.Source(r""%source_file%"")")}})

        Packages.Add(New PluginPackage With {
            .Name = "FluxSmooth",
            .Filename = "libfluxsmooth.dll",
            .VapourSynthFilterNames = {"SmoothT", "SmoothST"},
            .Description = "FluxSmooth is a filter for smoothing of fluctuations.",
            .WebURL = "https://github.com/dubhater/vapoursynth-fluxsmooth",
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Noise", "FluxSmooth SmoothT", "clip = core.flux.SmoothT(clip, temporal_threshold = 7, planes = [0, 1, 2])"),
                New VideoFilter("Noise", "FluxSmooth SmoothST", "clip = core.flux.SmoothST(clip, temporal_threshold = 7, spatial_threshold = 7, planes = [0, 1, 2])")}})

        Packages.Add(New PluginPackage With {
            .Name = "msmoosh",
            .Filename = "libmsmoosh.dll",
            .VapourSynthFilterNames = {"msmoosh.MSmooth", "msmoosh.MSharpen"},
            .Description = "MSmooth is a spatial smoother that doesn't touch edges." + CrLf + "MSharpen is a sharpener that tries to sharpen only edges.",
            .WebURL = "https://github.com/dubhater/vapoursynth-msmoosh",
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Noise", "MSmooth", "clip = core.msmoosh.MSmooth(clip, threshold = 6.0, strength = 3)"),
                New VideoFilter("Misc", "MSharpen", "clip = core.msmoosh.MSharpen(clip, threshold = 6.0, strength = 39)")}})

        Packages.Add(New PluginPackage With {
            .Name = "fmtconv",
            .Filename = "fmtconv.dll",
            .WebURL = "http://github.com/EleonoreMizo/fmtconv",
            .HelpFile = "fmtconv.html",
            .Description = "Fmtconv is a format-conversion plug-in for the Vapoursynth video processing engine. It does resizing, bitdepth conversion with dithering and colorspace conversion.",
            .VapourSynthFilterNames = {"fmtc.bitdepth", "fmtc.convert", "fmtc.matrix", "fmtc.resample", "fmtc.transfer"}})

        Packages.Add(New PluginPackage With {
            .Name = "finesharp",
            .Filename = "finesharp.py",
            .Description = "Port of Didée's FineSharp script to VapourSynth.",
            .WebURL = "http://forum.doom9.org/showthread.php?t=166524",
            .VapourSynthFilterNames = {"finesharp.sharpen"},
            .VapourSynthFiltersFunc = Function() {
                New VideoFilter("Misc", "finesharp", "clip = finesharp.sharpen(clip)")}})

        Packages.Add(New PluginPackage With {
            .Name = "aWarpSharp2",
            .Filename = "aWarpSharp.dll",
            .HelpFile = "aWarpSharp.txt",
            .AviSynthFilterNames = {"aBlur", "aSobel", "aWarp", "aWarp4", "aWarpSharp", "aWarpSharp2"},
            .Description = "This filter implements the same warp sharpening algorithm as aWarpSharp by Marc FD, but with several bugfixes and optimizations.",
            .WebURL = "http://avisynth.nl/index.php/AWarpSharp2",
            .AviSynthFiltersFunc = Function() {
                New VideoFilter("Misc", "aWarpSharp2", "aWarpSharp2(thresh = 128, blur = 2, type = 0, depth = 16, chroma = 4)")}})

        Packages.Add(New PluginPackage With {
            .Name = "VSFilterMod",
            .Filename = "VSFilterMod64.dll",
            .Description = "AviSynth subtitle plugin with support for vobsub srt and ass.",
            .WebURL = "http://avisynth.org.ru/docs/english/externalfilters/vsfilter.htm",
            .AviSynthFilterNames = {"VobSub", "TextSubMod"}})

        Packages.Add(New PluginPackage With {
            .Name = "TComb",
            .Filename = "TComb.dll",
            .HelpFile = "ReadMe.txt",
            .AviSynthFilterNames = {"TComb"},
            .Description = "TComb is a temporal comb filter.",
            .WebURL = "http://avisynth.nl/index.php/TComb",
            .AviSynthFiltersFunc = Function() {
                New VideoFilter("Misc", "TComb", "TComb(mode = 0, fthreshL = 255, othreshL = 255)")}})

        Packages.Add(New PluginPackage With {
            .Name = "QTGMC",
            .Filename = "QTGMC.avsi",
            .WebURL = "http://avisynth.nl/index.php/QTGMC",
            .HelpURL = "http://avisynth.nl/index.php/QTGMC",
            .Description = "A very high quality deinterlacer with a range of features for both quality and convenience. These include a simple presets system, extensive noise processing capabilities, support for repair of progressive material, precision source matching, shutter speed simulation, etc. Originally based on TempGaussMC by Didée.",
            .AviSynthFilterNames = {"QTGMC"},
            .AviSynthFiltersFunc = Function() {
                New VideoFilter("Field", "QTGMC | QTGMC Fast", "QTGMC(Preset = ""Fast"")"),
                New VideoFilter("Field", "QTGMC | QTGMC Medium", "QTGMC(Preset = ""Medium"")"),
                New VideoFilter("Field", "QTGMC | QTGMC Slow", "QTGMC(Preset = ""Slow"")")},
            .Dependencies = {"masktools2", "mvtools2", "nnedi3", "RgTools"}})

        Packages.Add(New PluginPackage With {
            .Name = "RgTools",
            .Filename = "RgTools.dll",
            .WebURL = "http://avisynth.nl/index.php/RgTools",
            .HelpURL = "http://avisynth.nl/index.php/RgTools",
            .Description = "RgTools is a modern rewrite of RemoveGrain, Repair, BackwardClense, Clense, ForwardClense and VerticalCleaner all in a single plugin.",
            .AviSynthFilterNames = {"RemoveGrain", "Clense", "ForwardClense", "BackwardClense", "Repair", "VerticalCleaner"},
            .AviSynthFiltersFunc = Function() {New VideoFilter("Noise", "RemoveGrain", "RemoveGrain()")}})

        Packages.Add(New PluginPackage With {
            .Name = "nnedi3",
            .Filename = "nnedi3.dll",
            .WebURL = "http://forum.doom9.org/showthread.php?t=170083",
            .HelpFile = "Readme.txt",
            .Description = "nnedi3 is an intra-field only deinterlacer. It takes in a frame, throws away one field, and then interpolates the missing pixels using only information from the kept field.",
            .AviSynthFilterNames = {"nnedi3"},
            .AviSynthFiltersFunc = Function() {New VideoFilter("Field", "nnedi3", "nnedi3(field = 1)")}})

        Packages.Add(New PluginPackage With {
            .Name = "mvtools2",
            .Filename = "mvtools2.dll",
            .WebURL = "https://github.com/pinterf/mvtools",
            .HelpURL = "http://avisynth.org.ru/mvtools/mvtools2.html",
            .Description = "MVTools is collection of functions for estimation and compensation of objects motion in video clips. Motion compensation may be used for strong temporal denoising, advanced framerate conversions, image restoration and other tasks.",
            .AviSynthFilterNames = {"MSuper", "MAnalyse", "MCompensate", "MMask", "MDeGrain1", "MDeGrain2", "MDegrain3"}})

        Packages.Add(New PluginPackage With {
            .Name = "mvtools",
            .Filename = "libmvtools.dll",
            .WebURL = "http://github.com/dubhater/vapoursynth-mvtools",
            .Description = "MVTools is a set of filters for motion estimation and compensation.",
            .VapourSynthFilterNames = {"mv.Super", "mv.Analyse", "mv.Recalculate", "mv.Compensate", "mv.Degrain1", "mv.Degrain2",
                "mv.Degrain3", "mv.Mask", "mv.Finest", "mv.FlowBlur", "mv.FlowInter", "mv.FlowFPS", "mv.BlockFPS", "mv.SCDetection"}})

        Packages.Add(New PluginPackage With {
            .Name = "masktools2",
            .Filename = "masktools2.dll",
            .WebURL = "http://avisynth.nl/index.php/MaskTools2",
            .HelpURL = "http://avisynth.nl/index.php/MaskTools2",
            .Description = "MaskTools2 contain a set of filters designed to create, manipulate and use masks. Masks, in video processing, are a way to give a relative importance to each pixel. You can, for example, create a mask that selects only the green parts of the video, and then replace those parts with another video.",
            .AviSynthFilterNames = {"Mt_edge", "Mt_motion"}})

        Packages.Add(New PluginPackage With {
            .Name = "FluxSmooth",
            .Filename = "FluxSmoothSSSE3.dll",
            .AviSynthFilterNames = {"FluxSmoothT", "FluxSmoothST"},
            .Description = "One of the fundamental properties of noise is that it's random. One of the fundamental properties of motion is that it's not. This is the premise behind FluxSmooth, which examines each pixel and compares it to the corresponding pixel in the previous and last frame. Smoothing occurs if both the previous frame's value and the next frame's value are greater, or if both are less, than the value in the current frame.",
            .WebURL = "http://avisynth.nl/index.php/FluxSmooth",
            .AviSynthFiltersFunc = Function() {
                New VideoFilter("Noise", "FluxSmoothT", "FluxSmoothT(temporal_threshold = 8)"),
                New VideoFilter("Noise", "FluxSmoothST", "FluxSmoothST(temporal_threshold = 8, spatial_threshold = 8)")}})

        Packages.Add(New PluginPackage With {
            .Name = "yadifmod2",
            .Filename = "yadifmod2_avx.dll",
            .AviSynthFilterNames = {"yadifmod2"},
            .Description = "Yet Another Deinterlacing Filter mod  for Avisynth2.6/Avisynth+",
            .HelpFile = "readme.md",
            .WebURL = "https://github.com/chikuzen/yadifmod2",
            .AviSynthFiltersFunc = Function() {New VideoFilter("Field", "yadifmod2", "yadifmod2()")}})

        Packages.Sort()

        Dim fp = CommonDirs.Startup + "Apps\Versions.txt"

        If File.Exists(fp) Then
            For Each i In File.ReadAllLines(CommonDirs.Startup + "Apps\Versions.txt")
                For Each i2 In Packages
                    If i Like "*=*;*" Then
                        Dim name = i.Left("=").Trim

                        If name = i2.ID Then
                            i2.Version = i.Right("=").Left(";").Trim
                            Dim a = i.Right("=").Right(";").Trim.Split("-"c)
                            i2.VersionDate = New DateTime(CInt(a(0)), CInt(a(1)), CInt(a(2)))
                        End If
                    End If
                Next
            Next
        End If
    End Sub

    Shared Function GetFilterProfiles(engine As ScriptingEngine) As List(Of VideoFilter)
        Dim ret As New List(Of VideoFilter)

        For Each i In Packages.OfType(Of PluginPackage)
            If engine = ScriptingEngine.AviSynth Then
                ret.AddRange(i.AviSynthFiltersFunc.Invoke)
            Else
                ret.AddRange(i.VapourSynthFiltersFunc.Invoke)
            End If
        Next
    End Function
End Class

Public Class Package
    Implements IComparable(Of Package)

    Property Name As String
    Property FileNotFoundMessage As String
    Property SetupAction As Action
    Property Version As String
    Property Description As String
    Property VersionDate As DateTime
    Property HelpFile As String
    Property HelpDir As String
    Property Filenames As String()
    Property WebURL As String
    Property DownloadURL As String
    Property HelpURL As String
    Property IsRequiredFunc As Func(Of Boolean)
    Property StatusFunc As Func(Of String)

    ReadOnly Property ID As String
        Get
            If TypeOf Me Is PluginPackage Then
                Dim plugin = DirectCast(Me, PluginPackage)

                If Not plugin.AviSynthFilterNames.ContainsNothingOrEmpty AndAlso
                    Not plugin.VapourSynthFilterNames.ContainsNothingOrEmpty Then

                    Return Name + " avs+vs"
                ElseIf Not plugin.AviSynthFilterNames.ContainsNothingOrEmpty Then
                    Return Name + " avs"
                ElseIf Not plugin.VapourSynthFilterNames.ContainsNothingOrEmpty Then
                    Return Name + " vs"
                End If
            End If

            Return Name
        End Get
    End Property

    Overridable Property FixedDir As String

    Private FilenameValue As String

    Overridable Property Filename As String
        Get
            If FilenameValue = "" AndAlso Not Filenames.ContainsNothingOrEmpty Then
                FilenameValue = Filenames(0)
            End If

            Return FilenameValue
        End Get
        Set(value As String)
            FilenameValue = value
        End Set
    End Property

    Protected LaunchName As String

    Overridable ReadOnly Property LaunchAction As Action
        Get
            If LaunchName <> "" Then Return Sub() g.ShellExecute(GetDir() + LaunchName)
        End Get
    End Property

    Overridable ReadOnly Property LaunchTitle As String
        Get
            Return Name
        End Get
    End Property

    Protected IsRequiredValue As Boolean = True

    Overridable ReadOnly Property IsRequired() As Boolean
        Get
            If Not IsRequiredFunc Is Nothing Then Return IsRequiredFunc.Invoke
            Return IsRequiredValue
        End Get
    End Property

    Sub LaunchWithJava()
        Try
            Dim p As New Process
            p.StartInfo.FileName = Packs.Java.GetDir + "javaw.exe"
            p.StartInfo.Arguments = "-jar """ + GetPath() + """"
            p.StartInfo.WorkingDirectory = GetDir()
            p.Start()
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    Function GetHelpPath() As String
        If HelpFile <> "" Then
            Return GetDir() + HelpFile
        ElseIf HelpDir <> "" Then
            Return GetDir() + HelpDir
        ElseIf HelpURL <> "" Then
            Return HelpURL
        ElseIf WebURL <> "" Then
            Return WebURL
        End If
    End Function

    Function VerifyOK(Optional showEvenIfNotRequired As Boolean = False) As Boolean
        If (IsRequired() OrElse showEvenIfNotRequired) AndAlso IsStatusCritical() Then
            Using f As New AppsForm
                f.ShowPackage(Me)
                f.ShowDialog()
                g.MainForm.Refresh()
            End Using

            If IsStatusCritical() Then Return False
        End If

        Return True
    End Function

    Function IsStatusCritical() As Boolean
        Return GetStatusLocation() <> "" OrElse GetStatus() <> ""
    End Function

    Overridable Function GetStatus() As String
        If IsOutdated() Then Return "Unsupported outdated version"
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
            Dim text = "Unknown version, press F12 to edit the version."

            If SetupAction Is Nothing Then
                Return text
            Else
                Return text + " In case of problems download and install the required version."
            End If
        End If
    End Function

    Function GetStatusLocation() As String
        Dim path = GetPath()

        If path = "" Then
            If FileNotFoundMessage <> "" Then
                Return "App Not found, press F11 to locate the App. " + FileNotFoundMessage
            ElseIf Not SetupAction Is Nothing Then
                Return "Please install " + Name + "."
            End If

            Return "App Not found, press F11 to locate the App."
        End If

        If FixedDir <> "" AndAlso path <> "" AndAlso Not path.ToLower.StartsWith(FixedDir.ToLower) Then
            Return "The App has To be located at: " + FixedDir
        End If
    End Function

    Function IsOutdated() As Boolean
        Dim fp = GetPath()
        If fp <> "" Then If (VersionDate - File.GetLastWriteTimeUtc(fp)).TotalDays > 3 Then Return True
    End Function

    Overridable Function IsCorrectVersion() As Boolean
        Dim fp = GetPath()

        If fp <> "" Then
            Dim dt = File.GetLastWriteTimeUtc(fp)
            Return dt.AddDays(-2) < VersionDate AndAlso dt.AddDays(2) > VersionDate
        End If
    End Function

    Function GetDir() As String
        Return Filepath.GetDir(GetPath)
    End Function

    Sub SetPath(path As String)
        s?.Storage?.SetString(Name + "custom path", path)
    End Sub

    Overridable Function GetPath() As String
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

        Dim plugin = TryCast(Me, PluginPackage)

        If Not plugin Is Nothing Then
            If Not plugin.VapourSynthFilterNames Is Nothing AndAlso Not plugin.AviSynthFilterNames Is Nothing Then
                ret = CommonDirs.Startup + "Apps\Plugins\both\" + Name + "\" + Filename
                If File.Exists(ret) Then Return ret
            Else
                If plugin.VapourSynthFilterNames Is Nothing Then
                    ret = CommonDirs.Startup + "Apps\Plugins\avs\" + Name + "\" + Filename
                    If File.Exists(ret) Then Return ret
                Else
                    ret = CommonDirs.Startup + "Apps\Plugins\vs\" + Name + "\" + Filename
                    If File.Exists(ret) Then Return ret
                End If
            End If
        End If

        ret = CommonDirs.Startup + "Apps\" + Name + "\" + Filename
        If File.Exists(ret) Then Return ret
    End Function

    Overrides Function ToString() As String
        Return Name
    End Function

    Function CompareTo(other As Package) As Integer Implements System.IComparable(Of Package).CompareTo
        Return Name.CompareTo(other.Name)
    End Function
End Class

Class UnDotPackage
    Inherits PluginPackage

    Sub New()
        Name = "UnDot"
        Filename = "UnDot.dll"
        WebURL = "http://avisynth.nl/index.php/UnDot"
        Description = "UnDot is a simple median filter for removing dots, that is stray orphan pixels and mosquito noise."
        AviSynthFilterNames = {"UnDot"}
    End Sub
End Class

Class NicAudioPackage
    Inherits PluginPackage

    Sub New()
        Name = "NicAudio"
        Filename = "NicAudio.dll"
        WebURL = "http://avisynth.org.ru/docs/english/externalfilters/nicaudio.htm"
        Description = "AviSynth audio source filter."
        AviSynthFilterNames = {"NicAC3Source", "NicDTSSource", "NicMPASource", "RaWavSource"}
    End Sub
End Class

Class AviSynthPlusPackage
    Inherits Package

    Sub New()
        Name = "AviSynth+"
        Filename = "avisynth.dll"
        WebURL = "http://avisynth.nl/index.php/AviSynth%2B"
        Description = "StaxRip support both AviSynth+ x64 and VapourSynth x64 as scripting based video processing tool."
        FixedDir = CommonDirs.System
        SetupAction = Sub() g.ShellExecute(CommonDirs.Startup + "Apps\AviSynth+_r1847.exe")
    End Sub

    Public Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return p.Script.Engine = ScriptingEngine.AviSynth
        End Get
    End Property

    Public Overrides Function GetStatus() As String
        If Not Directory.Exists(Paths.PluginsDir) Then
            Return "The AviSynth+ plugins directory is missing, run the AviSynth+ setup."
        End If

        Return MyBase.GetStatus()
    End Function
End Class

Class PythonPackage
    Inherits Package

    Sub New()
        Name = "Python"
        Filename = "python.exe"
        WebURL = "http://www.python.org"
        Description = "Python x64 is required by VapourSynth x64. StaxRip x64 supports both AviSynth+ x64 and VapourSynth x64 as scripting based video processing tool."
        DownloadURL = "https://www.python.org/ftp/python/3.5.1/python-3.5.1-amd64-webinstall.exe"
    End Sub

    Public Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return p.Script.Engine = ScriptingEngine.VapourSynth
        End Get
    End Property

    Public Overrides Function GetPath() As String
        Dim ret = MyBase.GetPath
        If File.Exists(ret) Then Return ret

        For Each i In {
            Registry.CurrentUser.GetString("SOFTWARE\Python\PythonCore\3.5\InstallPath", "ExecutablePath"),
            Registry.LocalMachine.GetString("SOFTWARE\Python\PythonCore\3.5\InstallPath", "ExecutablePath"),
            Registry.CurrentUser.GetString("SOFTWARE\Python\PythonCore\3.5\InstallPath", Nothing).AppendSeparator + "python.exe",
            Registry.LocalMachine.GetString("SOFTWARE\Python\PythonCore\3.5\InstallPath", Nothing).AppendSeparator + "python.exe"}

            If File.Exists(i) Then Return i
        Next

        Dim paths = Environment.ExpandEnvironmentVariables("%Path%").SplitNoEmptyAndWhiteSpace(";").ToList
        paths.AddRange(Environment.ExpandEnvironmentVariables("%PATH%").SplitNoEmptyAndWhiteSpace(";"))

        For Each i In paths
            i = i.Trim(" "c, """"c).AppendSeparator

            If File.Exists(i + "python.exe") Then
                SetPath(i + "python.exe")
                Return i + "python.exe"
            End If
        Next
    End Function
End Class

Class VapourSynthPackage
    Inherits Package

    Sub New()
        Name = "VapourSynth"
        Filename = "vapoursynth.dll"
        Description = "StaxRip x64 supports both AviSynth+ x64 and VapourSynth x64 as scripting based video processing tool."
        WebURL = "http://www.vapoursynth.com"
        HelpURL = "http://www.vapoursynth.com/doc"
        DownloadURL = "https://dl.dropboxusercontent.com/u/73468194/VapourSynth-R32-RC2.exe"
    End Sub

    Public Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return p.Script.Engine = ScriptingEngine.VapourSynth
        End Get
    End Property

    Public Overrides Property FixedDir As String
        Get
            Return Registry.LocalMachine.GetString("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\VapourSynth_is1", "Inno Setup: App Path") + "\core64\"
        End Get
        Set(value As String)
        End Set
    End Property
End Class

Class vspipePackage
    Inherits Package

    Sub New()
        Name = "vspipe"
        Filename = "vspipe.exe"
        Description = "vspipe is installed by VapourSynth and used to pipe VapourSynth scripts to encoding apps."
        WebURL = "http://www.vapoursynth.com/doc/vspipe.html"
        DownloadURL = "http://github.com/vapoursynth/vapoursynth/releases"
    End Sub

    Public Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return p.Script.Engine = ScriptingEngine.VapourSynth
        End Get
    End Property

    Public Overrides Property FixedDir As String
        Get
            Return Registry.LocalMachine.GetString("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\VapourSynth_is1", "Inno Setup: App Path") + "\core64\"
        End Get
        Set(value As String)
        End Set
    End Property
End Class

Class PluginPackage
    Inherits Package

    Property AviSynthFilterNames As String()
    Property VapourSynthFilterNames As String()
    Property Dependencies As String()
    Property VapourSynthFiltersFunc As Func(Of VideoFilter())
    Property AviSynthFiltersFunc As Func(Of VideoFilter())

    Function GetDependencies() As List(Of PluginPackage)
        Dim plugins = Packs.Packages.OfType(Of PluginPackage)()
        Dim ret As New List(Of PluginPackage)

        If Not Dependencies Is Nothing Then
            For Each iPlugin In plugins
                For Each i In Dependencies
                    If iPlugin.Name = i Then
                        ret.Add(iPlugin)
                    End If
                Next
            Next
        End If

        Return ret
    End Function

    Shared Sub WriteVSCode(ByRef script As String, ByRef code As String, plugin As PluginPackage)
        For Each i In plugin.GetDependencies
            WriteVSCode(script, code, i)
        Next

        If plugin.Filename.Ext = "py" Then
            If Not script.Contains("import importlib.machinery") AndAlso
                Not code.Contains("import importlib.machinery") Then

                code += "import importlib.machinery" + CrLf
            End If

            Dim line = plugin.Name + " = importlib.machinery.SourceFileLoader('" +
                plugin.Name + "', r""" + plugin.GetPath + """).load_module()" + CrLf

            If Not script.Contains(line) AndAlso Not code.Contains(line) Then code += line
        ElseIf Not plugin.VapourSynthFilterNames Is Nothing Then
            If Not File.Exists(Paths.PluginsDir + plugin.Filename) Then
                Dim line = "core.std.LoadPlugin(r""" + plugin.GetPath + """)" + CrLf

                If Not script.Contains(line) AndAlso Not code.Contains(line) Then
                    code += line
                End If
            End If
        End If
    End Sub
End Class

Class BeSweetPackage
    Inherits Package

    Sub New()
        Name = "BeSweet"
        Filename = "BeSweet.exe"
        WebURL = "http://dspguru.doom9.net"
        Description = "Alternative audio converter, for most formats StaxRip uses now eac3to by default."
        HelpDir = "help"
    End Sub

    Overrides Function GetStatus() As String
        For Each i In {p.Audio0, p.Audio1}
            If TypeOf i Is BatchAudioProfile Then
                Dim profile = DirectCast(i, BatchAudioProfile)

                If profile.CommandLines.Contains("-lame(") Then
                    If Not CheckLib(GetDir() + "lame_enc.dll", New DateTime(2005, 12, 22)) Then
                        Return "There is a problem with the lame library."
                    End If
                End If

                If profile.CommandLines.Contains("-bsn(") Then
                    If Not CheckLib(GetDir() + "bsn.dll", New DateTime(2006, 5, 22)) Then
                        Return "There is a problem with the bsn library."
                    End If
                End If
            End If
        Next
    End Function

    Private Function CheckLib(filename As String, dt As DateTime) As Boolean
        Return File.Exists(filename) AndAlso File.GetLastWriteTime(filename) > dt.AddDays(-2)
    End Function
End Class

Class VSRipPackage
    Inherits Package

    Sub New()
        Name = "VSRip"
        Filename = "VSRip.exe"
        Description = "VSRip rips VobSub subtitles."
        WebURL = "http://sourceforge.net/projects/guliverkli"
        LaunchName = Filename
    End Sub
End Class

Class NeroAACEncPackage
    Inherits Package

    Sub New()
        Name = "Nero AAC Encoder"
        Filename = "neroAacEnc.exe"
        Description = "Free AAC encoder"
        WebURL = "http://www.nero.com/enu/downloads-nerodigital-nero-aac-codec.php"
        HelpFile = "nero readme.txt"
        FixedDir = CommonDirs.Startup + "Apps\BeSweet\"
    End Sub
End Class

Class JavaPackage
    Inherits Package

    Sub New()
        Name = "Java"
        Filename = "Java.exe"
        WebURL = "http://java.com"
        Description = "Java is required by ProjectX. " + Strings.ProjectX
        DownloadURL = "http://java.com/en/download"
        IsRequiredValue = False
    End Sub

    Public Overrides Function GetPath() As String
        Dim ret = MyBase.GetPath()
        If ret <> "" Then Return ret

        ret = "C:\ProgramData\Oracle\Java\javapath\" + Filename
        If File.Exists(ret) Then Return ret

        ret = "C:\Windows\Sysnative\" + Filename
        If File.Exists(ret) Then Return ret

        ret = "C:\Windows\System32\" + Filename
        If File.Exists(ret) Then Return ret
    End Function
End Class

Class ProjectXPackage
    Inherits Package

    Sub New()
        Name = "ProjectX"
        Filename = "ProjectX.jar"
        WebURL = "http://project-x.sourceforge.net"
        Description = Strings.ProjectX
        IsRequiredValue = False
    End Sub

    Overrides ReadOnly Property LaunchAction As Action
        Get
            Return AddressOf LaunchWithJava
        End Get
    End Property

    Public Overrides Function GetStatus() As String
        If Packs.Java.GetPath = "" Then Return "Failed to locate Java, ProjectX requires Java."
        Return MyBase.GetStatus()
    End Function
End Class

Class x264Package
    Inherits Package

    Sub New()
        Name = "x264"
        Filename = "x264.exe"
        WebURL = "http://www.videolan.org/developers/x264.html"
        Description = "H.264 video encoding command line app."
        HelpFile = "Help.txt"
    End Sub
End Class

Class x265Package
    Inherits Package

    Sub New()
        Name = "x265"
        Filename = "x265_ml.exe"
        WebURL = "http://x265.org"
        HelpURL = "http://x265.readthedocs.org"
        Description = "H.265 video encoding command line app."
    End Sub
End Class

Class MP4BoxPackage
    Inherits Package

    Sub New()
        Name = "MP4Box"
        Filename = "MP4Box.exe"
        WebURL = "http://gpac.wp.mines-telecom.fr/"
        HelpURL = "http://gpac.wp.mines-telecom.fr/mp4box/mp4box-documentation"
        Description = "MP4Box is a MP4 muxing and demuxing command line app."
    End Sub
End Class

Class MKVToolNixPackage
    Inherits Package

    Sub New()
        Name = "MKVToolNix"
        Filename = "mkvmerge.exe"
        WebURL = "http://www.bunkus.org/videotools/mkvtoolnix"
        HelpURL = "http://www.bunkus.org/videotools/mkvtoolnix/docs.html"
        Description = "MKVtoolnix contains mkvmerge and mkvextract to mux and demux Matroska (MKV) files."
    End Sub
End Class

Class MediaInfoPackage
    Inherits Package

    Sub New()
        Name = "MediaInfo"
        Filename = "MediaInfo.dll"
        WebURL = "http://mediainfo.sourceforge.net"
        Description = "MediaInfo is used by StaxRip to read infos from media files."
    End Sub
End Class

Class ffmpegPackage
    Inherits Package

    Sub New()
        Name = "ffmpeg"
        Filename = "ffmpeg.exe"
        WebURL = "http://ffmpeg.org"
        HelpURL = "https://www.ffmpeg.org/ffmpeg-all.html"
        Description = "Versatile audio video converter."
    End Sub
End Class

Class eac3toPackage
    Inherits Package

    Sub New()
        Name = "eac3to"
        Filename = "eac3to.exe"
        WebURL = "http://forum.doom9.org/showthread.php?t=125966"
        HelpURL = "http://en.wikibooks.org/wiki/Eac3to/How_to_Use"
        Description = "Audio conversion command line app."
    End Sub
End Class

Class ffms2Package
    Inherits PluginPackage

    Sub New()
        Name = "ffms2"
        Filename = "ffms2.dll"
        WebURL = "https://github.com/FFMS/ffms2"
        Description = "AviSynth+ and VapourSynth source filter supporting various input formats."
        AviSynthFilterNames = {"FFVideoSource", "FFAudioSource"}
        VapourSynthFilterNames = {"ffms2"}
        HelpDir = "doc"
    End Sub
End Class

Class LSmashWorksAviSynthPackage
    Inherits PluginPackage

    Sub New()
        Name = "L-SMASH-Works"
        Filename = "LSMASHSource.dll"
        WebURL = "http://avisynth.nl/index.php/LSMASHSource"
        Description = "AviSynth and VapourSynth source filter based on Libav supporting a wide range of input formats."
        HelpFile = "README.txt"
        AviSynthFilterNames = {"LSMASHVideoSource", "LSMASHAudioSource", "LWLibavVideoSource", "LWLibavAudioSource"}
    End Sub

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return p.Script.GetFilter("Source").Script.Contains("LSMASHVideoSource") OrElse
                p.Script.GetFilter("Source").Script.Contains("LWLibavVideoSource")
        End Get
    End Property
End Class

Class vslsmashsourcePackage
    Inherits PluginPackage

    Sub New()
        Name = "vslsmashsource"
        Filename = "vslsmashsource.dll"
        Description = "VapourSynth source filter based on Libav supporting a wide range of input formats."
        VapourSynthFilterNames = {"lsmas.LibavSMASHSource", "lsmas.LWLibavSource"}
        HelpFile = "README.txt"
        WebURL = "http://avisynth.nl/index.php/LSMASHSource"
    End Sub

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return p.Script.GetFilter("Source").Script.Contains("lsmas.LibavSMASHSource") OrElse
                p.Script.GetFilter("Source").Script.Contains("lsmas.LWLibavSource")
        End Get
    End Property
End Class

Class qaacPackage
    Inherits Package

    Sub New()
        Name = "qaac"
        Filename = "qaac64.exe"
        WebURL = "http://github.com/nu774/qaac"
        Description = "qaac is a command line AAC encoder frontend based on the Apple AAC encoder. qaac requires libflac which StaxRip includes and it requires AppleApplicationSupport64.msi which can be extracted from the x64 iTunes installer using a decompression tool like 7-Zip. The makeportable script found on the qaac website can also be used."
    End Sub

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return TypeOf p.Audio0 Is GUIAudioProfile AndAlso
                DirectCast(p.Audio0, GUIAudioProfile).Params.Encoder = GuiAudioEncoder.qaac OrElse
                TypeOf p.Audio1 Is GUIAudioProfile AndAlso
                DirectCast(p.Audio1, GUIAudioProfile).Params.Encoder = GuiAudioEncoder.qaac
        End Get
    End Property

    Overrides Function GetStatus() As String
        Dim path = CommonDirs.Programs + "Common Files\Apple\Apple Application Support\CoreAudioToolbox.dll"

        If Not File.Exists(path) AndAlso Not File.Exists(GetDir() + "QTfiles64\CoreAudioToolbox.dll") Then
            Return "Failed to locate CoreAudioToolbox, read the description below. Expected paths:" +
                CrLf2 + path + CrLf2 + GetDir() + "QTfiles64\CoreAudioToolbox.dll"
        End If
    End Function
End Class

Class checkmatePackage
    Inherits PluginPackage

    Sub New()
        Name = "checkmate"
        Filename = "checkmate.dll"
        WebURL = "http://github.com/tp7/checkmate"
        Description = "Spatial and temporal dot crawl reducer. Checkmate is most effective in static or low motion scenes. When using in high motion scenes (or areas) be careful, it's known to cause artifacts with its default values."
        AviSynthFilterNames = {"checkmate"}
    End Sub
End Class

Class SangNom2Package
    Inherits PluginPackage

    Sub New()
        Name = "SangNom2"
        Filename = "SangNom2.dll"
        WebURL = "http://avisynth.nl/index.php/SangNom2"
        Description = "SangNom2 is a reimplementation of MarcFD's old SangNom filter. Originally it's a single field deinterlacer using edge-directed interpolation but nowadays it's mainly used in anti-aliasing scripts. The output is not completely but mostly identical to the original SangNom."
        AviSynthFilterNames = {"SangNom2"}
    End Sub
End Class

Class DSS2modPackage
    Inherits PluginPackage

    Sub New()
        Name = "DSS2mod"
        Filename = "DSS2.dll"
        WebURL = "http://code.google.com/p/xvid4psp/downloads/detail?name=DSS2%20mod%20%2B%20LAVFilters.7z&can=2&q="
        Description = "Direct Show source filter"
        AviSynthFilterNames = {"DSS2"}
    End Sub
End Class

Class vinversePackage
    Inherits PluginPackage

    Sub New()
        Name = "vinverse"
        Filename = "vinverse.dll"
        WebURL = "http://avisynth.nl/index.php/Vinverse"
        Description = "A modern rewrite of a simple but effective plugin to remove residual combing originally based on an AviSynth script by Didée and then written as a plugin by tritical."
        AviSynthFilterNames = {"vinverse", "vinverse2"}
    End Sub
End Class

Class DGIndexNVPackage
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

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return CommandLineDemuxer.IsActive("DGIndexNV")
        End Get
    End Property
End Class

Class DGIndexIMPackage
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

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return CommandLineDemuxer.IsActive("DGIndexIM")
        End Get
    End Property
End Class

Class DGDecodeIMPackage
    Inherits PluginPackage

    Sub New()
        Name = "DGDecodeIM"
        Filename = "DGDecodeIM.dll"
        WebURL = "http://rationalqm.us/mine.html"
        Description = Strings.DGDecIM
        HelpFile = "Notes.txt"
        AviSynthFilterNames = {"DGSourceIM"}
    End Sub

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return p.Script.Filters(0).Script.Contains("DGSourceIM(")
        End Get
    End Property
End Class

Class BDSup2SubPackage
    Inherits Package

    Sub New()
        Name = "BDSup2Sub++"
        Filename = "bdsup2sub++.exe"
        LaunchName = Filename
        WebURL = "http://forum.doom9.org/showthread.php?p=1613303"
        Description = "Converts Blu-ray subtitles to other formats like VobSub."
    End Sub
End Class

Class NVEncCPackage
    Inherits Package

    Sub New()
        Name = "NVEncC"
        Filename = "NVEncC64.exe"
        WebURL = "https://onedrive.live.com/?cid=6bdd4375ac8933c6&id=6BDD4375AC8933C6!2293"
        Description = "nvidia GPU accelerated H.264/H.265 encoder."
        HelpFile = "help.txt"
    End Sub
End Class

Class AVSMeterPackage
    Inherits Package

    Sub New()
        Name = "AVSMeter"
        Filename = "AVSMeter64.exe"
        Description = "AVSMeter runs an Avisynth script with virtually no overhead, displays clip info, CPU and memory usage and the minimum, maximum and average frames processed per second. It measures how fast Avisynth can serve frames to a client application like x264 and comes in handy when testing filters/plugins to evaluate their performance and memory requirements."
        HelpFile = "doc\AVSMeter.html"
        WebURL = "http://forum.doom9.org/showthread.php?t=165528"
    End Sub
End Class

Class DivX265Package
    Inherits Package

    Sub New()
        Name = "DivX265"
        Filename = "DivX265.exe"
        Description = "DivX H265 command line encoder"
        HelpFile = "help.txt"
        WebURL = "http://labs.divx.com/term/HEVC"
    End Sub
End Class

Class xvid_encrawPackage
    Inherits Package

    Sub New()
        Name = "xvid_encraw"
        Filename = "xvid_encraw.exe"
        Description = "XviD command line encoder"
        HelpFile = "help.txt"
        WebURL = "https://www.xvid.com"
    End Sub
End Class

Class dsmuxPackage
    Inherits Package

    Sub New()
        Name = "dsmux"
        Filename = "dsmux.x64.exe"
        Description = Strings.dsmux
        WebURL = "http://haali.su/mkv"
        SetupAction = Sub() g.ShellExecute(CommonDirs.Startup + "Apps\MatroskaSplitter.exe")
    End Sub

    Public Overrides Function GetPath() As String
        Dim ret = Registry.ClassesRoot.GetString("CLSID\" + GUIDS.HaaliMuxer.ToString + "\InprocServer32", Nothing)
        ret = Filepath.GetDir(ret) + Filename
        If File.Exists(ret) Then Return ret
    End Function

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return CommandLineDemuxer.IsActive("dsmux")
        End Get
    End Property
End Class

Class HaaliSplitter
    Inherits Package

    Sub New()
        Name = "Haali Splitter"
        Filename = "splitter.ax"
        WebURL = "http://haali.su/mkv"
        SetupAction = Sub() g.ShellExecute(CommonDirs.Startup + "Apps\MatroskaSplitter.exe")
        Description = "Haali Splitter is used by eac3to and dsmux to write MKV files. Haali Splitter and LAV Filters overrite each other, most people prefer LAV Filters, therefore it's recommended to install Haali first and LAV Filters last."
        IsRequiredValue = False
    End Sub

    Public Overrides Function GetPath() As String
        Dim ret = Registry.ClassesRoot.GetString("CLSID\" + GUIDS.HaaliMuxer.ToString + "\InprocServer32", Nothing)
        If File.Exists(ret) Then Return ret
    End Function
End Class

Class MPCPackage
    Inherits Package

    Sub New()
        Name = "MPC Player"
        Filenames = {"mpc-be64.exe", "mpc-hc64.exe"}
        Description = "MPC is a open source media player with built in playback support for all common media formats. MPC-HC or MPC-BE can be used, x64 is absolutely required because StaxRip supports only AviSynth+ x64. StaxRip uses MPC's /dub and /sub CLI switches."
        WebURL = "http://mpc-hc.org"
        HelpURL = "http://forum.doom9.org/showthread.php?p=1719479&goto=newpost"
        IsRequiredValue = False
    End Sub

    Public Overrides ReadOnly Property LaunchAction As Action
        Get
            Return Sub() g.ShellExecute(GetPath, If(p.SourceFile <> "" AndAlso Not p.SourceFile.EqualsAny(FileTypes.VideoIndex), """" + p.SourceFile, """"))
        End Get
    End Property
End Class

Class TDeintPackage
    Inherits PluginPackage

    Sub New()
        Name = "TDeint"
        Filename = "TDeint.dll"
        WebURL = "http://avisynth.nl/index.php/TDeint"
        Description = "TDeint is a bi-directionally, motion adaptive, sharp deinterlacer. It can adaptively choose between using per-field and per-pixel motion adaptivity, and can use cubic interpolation, kernel interpolation (with temporal direction switching), or one of two forms of modified ELA interpolation which help to reduce ""jaggy"" edges in moving areas where interpolation must be used."
        AviSynthFilterNames = {"TDeint"}
    End Sub
End Class

Class nnedi3Package
    Inherits PluginPackage

    Sub New()
        Name = "nnedi3"
        Filename = "libnnedi3.dll"
        WebURL = "http://github.com/dubhater/vapoursynth-nnedi3"
        Description = "nnedi3 is an intra-field only deinterlacer. It takes in a frame, throws away one field, and then interpolates the missing pixels using only information from the kept field."
        VapourSynthFilterNames = {"nnedi3.nnedi3", "nnedi3.nnedi3_rpow2"}
    End Sub
End Class

Class scenechangePackage
    Inherits PluginPackage

    Sub New()
        Name = "scenechange"
        Filename = "scenechange.dll"
        VapourSynthFilterNames = {"scenechange"}
    End Sub
End Class

Class temporalsoftenPackage
    Inherits PluginPackage

    Sub New()
        Name = "temporalsoften"
        Filename = "temporalsoften.dll"
        VapourSynthFilterNames = {"temporalsoften"}
    End Sub
End Class

Class DecombPackage
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

Class flash3kyuu_debandPackage
    Inherits PluginPackage

    Sub New()
        Name = "flash3kyuu_deband"
        Filename = "flash3kyuu_deband.dll"
        WebURL = "http://forum.doom9.org/showthread.php?t=161411"
        HelpFile = "flash3kyuu_deband.txt"
        Description = "Simple debanding filter that can be quite effective for some anime sources."
        AviSynthFilterNames = {"f3kdb"}
    End Sub
End Class