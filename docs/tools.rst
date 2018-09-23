Supported Tools
===============

Tools
-----

AddGrainC
~~~~~~~~~~~

Generate film-like grain or other effects (like rain) by adding random noise to a video clip. This noise may optionally be horizontally or vertically correlated to cause streaking. 

Used Version: 1.7.1 

Adjust
~~~~~~~~~~~

very basic port of the built-in Avisynth filter Tweak.

Used Version: 2015-03-22

AnimeIVTC
~~~~~~~~~~~

Used Version: 2.20

AviSynth+
~~~~~~~~~

StaxRip support both AviSynth+ x64 and VapourSynth x64 as scripting based video processing tool.

Used Version: 2580

http://avisynth.nl/index.php/AviSynth%2B


avs2pipemod
~~~~~~~~~~~

Given an AviSynth script as input, avs2pipemod can send video, audio, or information of various types to stdout for consumption by command line encoders or other tools.

Used Version: 1.1.1

https://github.com/chikuzen/avs2pipemod


AVSMeter
~~~~~~~~

AVSMeter runs an Avisynth script with virtually no overhead, displays clip info, CPU and memory usage and the minimum, maximum and average frames processed per second. It measures how fast Avisynth can serve frames to a client application like x264 and comes in handy when testing filters/plugins to evaluate their performance and memory requirements.

Used Version: 2.6.6

https://forum.doom9.org/showthread.php?t=174797


BDSup2Sub++
~~~~~~~~~~~

Converts Blu-ray subtitles to other formats like VobSub.

Used Version: 1.0.2

http://forum.doom9.org/showthread.php?p=1613303


DGIndex
~~~~~~~

MPEG-2 demuxing and indexing app.

Used Version: 1.5.8

http://rationalqm.us/dgmpgdec/dgmpgdec.html


DGIndexIM
~~~~~~~~~

DGDecIM is a shareware AviSynth source filter using Intel powered hardware acceleration. DGIndexIM can be enabled and configured at Tools/Setting/Demux. Which file types DGIndexIM handles can be configured. DGIndexIM can demux audio with proper av sync.

Used Version: b50

http://rationalqm.us/mine.html


DGIndexNV
~~~~~~~~~

DGDecNV is a shareware AviSynth source filter using NVIDIA hardware acceleration. DGIndexNV can be configured at Tools > Setting > Demux. DGDecNV is not included so must be downloaded manually.

Used Version: 2053 2017-08-28

http://rationalqm.us/dgdecnv/dgdecnv.html


dsmux
~~~~~

dsmux is installed by the Haali Splitter and is used to mux TS containing AVC into MKV in order to fix av sync problems, dsmux handles av sync much better then many other TS tools. dsmux can be enabled or disabled in the settings on the preprocessing tab, if no audio is present or DGDecNV/DGDecIM is used, dsmux is not necessary and skipped automatically. LAV Filters and Haali Splitter overrite each other, most people prefer LAV Filters, therefore it's recommended to install Haali first and LAV Filters last.

Used Version: 2013-04-14

http://haali.su/mkv


eac3to
~~~~~~

Audio conversion command line app.

Used Version: 3.34

http://forum.doom9.org/showthread.php?t=125966


fdkaac
~~~~~~

Command line AAC encoder based on libfdk-aac.

Used Version: 0.6.3

https://github.com/nu774/fdkaac


ffmpeg
~~~~~~

Versatile audio video converter.

Used Version: 3.3.4

http://ffmpeg.org


FFTW
~~~~

Library required by the FFT3DFilter AviSynth plugin.

Used Version: 3.3.6

http://www.fftw.org/


Haali Splitter
~~~~~~~~~~~~~~

Haali Splitter is used by eac3to and dsmux to write MKV files. Haali Splitter and LAV Filters overrite each other, most people prefer LAV Filters, therefore it's recommended to install Haali first and LAV Filters last.

Used Version: 2013-04-14

http://haali.su/mkv


MediaInfo
~~~~~~~~~

MediaInfo is used by StaxRip to read infos from media files.

Used Version: 0.7.99

https://mediaarea.net/en/MediaInfo


mkvextract
~~~~~~~~~~

MKV demuxing tool.

Used Version: 26.0.0

http://www.bunkus.org/videotools/mkvtoolnix


mkvmerge
~~~~~~~~

MKV muxing tool.

Used Version: 26.0.0

http://www.bunkus.org/videotools/mkvtoolnix


MP4Box
~~~~~~

MP4Box is a MP4 muxing and demuxing command line app.

Used Version: 0.7.2-DEV-rev709

http://gpac.wp.mines-telecom.fr/


mpv
~~~~~~~

libmpv based media player.

Used Version: 2018-09-12

https://mpv.io


NVEnc
~~~~~

NVIDIA hardware video encoder.

Used Version: 3.27

https://github.com/rigaya/NVEnc


Python
~~~~~~

Python x64 is required by VapourSynth x64. StaxRip x64 supports both AviSynth+ x64 and VapourSynth x64 as scripting based video processing tool. Use anaconda if you don't have admin access.

Used Version: 3.6.6

http://www.python.org
https://www.anaconda.com/download/


qaac
~~~~

qaac is a command line AAC encoder frontend based on the Apple AAC encoder. qaac requires libflac which StaxRip includes and it requires AppleApplicationSupport64.msi which can be extracted from the x64 iTunes installer using a decompression tool like 7-Zip. The makeportable script found on the qaac website can also be used.

Used Version: 2.64

http://github.com/nu774/qaac


QSVEnc
~~~~~~

Intel hardware video encoder.

Used Version: 2.73

https://github.com/rigaya/QSVEnc


SubtitleEdit
~~~~~~~~~~~~

Subtitle Edit is a open source subtitle editor.

Used Version: 

http://www.nikse.dk/SubtitleEdit


VapourSynth
~~~~~~~~~~~

StaxRip x64 supports both AviSynth+ x64 and VapourSynth x64 as scripting based video processing tool.

Used Version: R44

http://www.vapoursynth.com


VCEEnc
~~~~~~

AMD hardware video encoder.

Used Version: 3.06

https://github.com/rigaya/VCEEnc


Visual C++ 2012
~~~~~~~~~~~~~~~

Visual C++ 2012 Redistributable is required by some tools used by StaxRip.

Used Version: 




Visual C++ 2013
~~~~~~~~~~~~~~~

Visual C++ 2013 Redistributable is required by some tools used by StaxRip.

Used Version: 




Visual C++ 2017
~~~~~~~~~~~~~~~

Visual C++ 2017 Redistributable is required by some tools used by StaxRip.

Used Version: 




vspipe
~~~~~~

vspipe is installed by VapourSynth and used to pipe VapourSynth scripts to encoding apps.

Used Version: R43

http://www.vapoursynth.com/doc/vspipe.html


VSRip
~~~~~

VSRip rips VobSub subtitles.

Used Version: 1.0.0.7

http://sourceforge.net/projects/guliverkli


x264
~~~~

H.264 video encoding command line app.

Used Version: 0.150.2851 8-Bit

http://www.videolan.org/developers/x264.html


x264 10-Bit
~~~~~~~~~~~

H.264 video encoding command line app.

Used Version: 0.150.2851 10-Bit

http://www.videolan.org/developers/x264.html


x265
~~~~

H.265 video encoding command line app.

Used Version: 2.8+66

http://x265.org


AviSynth Plugins
----------------
AutoAdjust
~~~~~~~~~~

AutoAdjust is an automatic adjustement filter. It calculates statistics of clip, stabilizes them temporally and uses them to adjust luminance gain & color balance.

Filters: AutoAdjust

Used Version: 2.60

https://forum.doom9.org/showthread.php?t=167573


aWarpSharp2
~~~~~~~~~~~

This filter implements the same warp sharpening algorithm as aWarpSharp by Marc FD, but with several bugfixes and optimizations.

Filters: aBlur, aSobel, aWarp, aWarp4, aWarpSharp, aWarpSharp2

Used Version: 2016-06-24

http://avisynth.nl/index.php/AWarpSharp2


checkmate
~~~~~~~~~

Spatial and temporal dot crawl reducer. Checkmate is most effective in static or low motion scenes. When using in high motion scenes (or areas) be careful, it's known to cause artifacts with its default values.

Filters: checkmate

Used Version: 0.9

http://github.com/tp7/checkmate


DCTFilter
~~~~~~~~~

A rewrite of DctFilter for Avisynth+.

Filters: DCTFilter, DCTFilterD, DCTFilter4, DCTFilter4D, DCTFilter8, DCTFilter8D

Used Version: 0.5.0

https://github.com/chikuzen/DCTFilter


Decomb
~~~~~~

This package of plugin functions for Avisynth provides the means for removing combing artifacts from telecined progressive streams, interlaced streams, and mixtures thereof. Functions can be combined to implement inverse telecine (IVTC) for both NTSC and PAL streams.

Filters: Telecide, FieldDeinterlace, Decimate, IsCombed

Used Version: 5.2.4

http://rationalqm.us/decomb/decombnew.html


DGDecodeIM
~~~~~~~~~~

DGDecIM is a shareware AviSynth source filter using Intel powered hardware acceleration. DGIndexIM can be enabled and configured at Tools/Setting/Demux. Which file types DGIndexIM handles can be configured. DGIndexIM can demux audio with proper av sync.

Filters: DGSourceIM

Used Version: b50

http://rationalqm.us/mine.html


DGDecodeNV
~~~~~~~~~~

DGDecNV is a shareware AviSynth source filter using NVIDIA hardware acceleration. DGIndexNV can be configured at Tools > Setting > Demux. DGDecNV is not included so must be downloaded manually.

Filters: DGSource

Used Version: 2053 2017-08-28

http://rationalqm.us/dgdecnv/dgdecnv.html


DSS2mod
~~~~~~~

Direct Show source filter

Filters: DSS2

Used Version: 2014-11-13

http://code.google.com/p/xvid4psp/downloads/detail?name=DSS2%20mod%20%2B%20LAVFilters.7z&can=2&q=


EEDI2
~~~~~

EEDI2 (Enhanced Edge Directed Interpolation) resizes an image by 2x in the vertical direction by copying the existing image to 2*y(n) and interpolating the missing field.

Filters: EEDI2

Used Version: 0.9.2

http://avisynth.nl/index.php/EEDI2


ffms2
~~~~~

AviSynth+ and VapourSynth source filter supporting various input formats.

Filters: FFVideoSource, FFAudioSource

Used Version: 2.23.1

https://github.com/FFMS/ffms2


FFT3DFilter
~~~~~~~~~~~

FFT3DFilter uses Fast Fourier Transform method for image processing in frequency domain.

Filters: FFT3DFilter

Used Version: 2.4.7

https://github.com/pinterf/fft3dfilter


FineSharp
~~~~~~~~~

Small and fast realtime-sharpening function for 1080p, or after scaling 720p -> 1080p. It's a generic sharpener only for good quality sources!

Filters: FineSharp

Used Version: 2012-04-12

https://forum.doom9.org/showthread.php?p=1569035


flash3kyuu_deband
~~~~~~~~~~~~~~~~~

Simple debanding filter that can be quite effective for some anime sources.

Filters: f3kdb

Used Version: 2015-05-02

http://forum.doom9.org/showthread.php?t=161411


FluxSmooth
~~~~~~~~~~

One of the fundamental properties of noise is that it's random. One of the fundamental properties of motion is that it's not. This is the premise behind FluxSmooth, which examines each pixel and compares it to the corresponding pixel in the previous and last frame. Smoothing occurs if both the previous frame's value and the next frame's value are greater, or if both are less, than the value in the current frame.

Filters: FluxSmoothT, FluxSmoothST

Used Version: 2010-12-01

http://avisynth.nl/index.php/FluxSmooth


FrameRateConverter AVSI
~~~~~~~~~~~~~~~~~~~~~~~

Increases the frame rate with interpolation and fine artifact removal.

Filters: FrameRateConverter

Used Version: 1.2.1

https://github.com/mysteryx93/FrameRateConverter


FrameRateConverter DLL
~~~~~~~~~~~~~~~~~~~~~~

Increases the frame rate with interpolation and fine artifact removal.

Filters: FrameRateConverter

Used Version: 1.2

https://github.com/mysteryx93/FrameRateConverter


JPSDR
~~~~~

Merge of AutoYUY2, NNEDI3 and ResampleMT

Filters: nnedi3, nnedi3_rpow2, AutoYUY2, PointResizeMT, BilinearResizeMT, BicubicResizeMT, LanczosResizeMT, Lanczos4ResizeMT, BlackmanResizeMT, Spline16ResizeMT, Spline36ResizeMT, Spline64ResizeMT, GaussResizeMT, SincResizeMT

Used Version: 2.2.0.0

https://forum.doom9.org/showthread.php?t=174248


KNLMeansCL
~~~~~~~~~~

KNLMeansCL is an optimized pixelwise OpenCL implementation of the Non-local means denoising algorithm. Every pixel is restored by the weighted average of all pixels in its search window. The level of averaging is determined by the filtering parameter h.

Filters: KNLMeansCL

Used Version: 1.1.1

https://github.com/Khanattila/KNLMeansCL


LSFmod
~~~~~~

A LimitedSharpenFaster mod with a lot of new features and optimizations.

Filters: LSFmod

Used Version: 1.9

http://avisynth.nl/index.php/LSFmod


L-SMASH-Works
~~~~~~~~~~~~~

AviSynth and VapourSynth source filter based on Libav supporting a wide range of input formats.

Filters: LSMASHVideoSource, LSMASHAudioSource, LWLibavVideoSource, LWLibavAudioSource

Used Version: 929

http://avisynth.nl/index.php/LSMASHSource


masktools2
~~~~~~~~~~

MaskTools2 contain a set of filters designed to create, manipulate and use masks. Masks, in video processing, are a way to give a relative importance to each pixel. You can, for example, create a mask that selects only the green parts of the video, and then replace those parts with another video.

Filters: mt_adddiff, mt_average, mt_binarize, mt_circle, mt_clamp, mt_convolution, mt_diamond, mt_edge, mt_ellipse, mt_expand, mt_hysteresis, mt_inflate, mt_inpand, mt_invert, mt_logic, mt_losange, mt_lut, mt_lutf, mt_luts, mt_lutxy, mt_makediff, mt_mappedblur, mt_merge, mt_motion, mt_polish, mt_rectangle, mt_square

Used Version: 2.2.14

https://github.com/pinterf/masktools


mClean
~~~~~~

Removes noise whilst retaining as much detail as possible.

Filters: mClean

Used Version: 3.2

https://forum.doom9.org/showthread.php?t=174804


MedianBlur2
~~~~~~~~~~~

Implementation of constant time median filter for AviSynth.

Filters: MedianBlur, MedianBlurTemporal

Used Version: 0.94

http://avisynth.nl/index.php/MedianBlur2


modPlus
~~~~~~~

This plugin has 9 functions, which modify values of color components to attenuate noise, blur or equalize input.

Filters: GBlur, MBlur, Median, minvar, Morph, SaltPepper, SegAmp, TweakHist, Veed

Used Version: 2017-10-17

http://www.avisynth.nl/users/vcmohan/modPlus/modPlus.html


MPEG2DecPlus
~~~~~~~~~~~~

Source filter to open D2V index files created with DGIndex or D2VWitch.

Filters: MPEG2Source

Used Version: 1.5.8.0

https://github.com/chikuzen/MPEG2DecPlus


MSharpen
~~~~~~~~



Filters: MSharpen

Used Version: 0.9

https://github.com/tp7/msharpen


mvtools2
~~~~~~~~

MVTools is collection of functions for estimation and compensation of objects motion in video clips. Motion compensation may be used for strong temporal denoising, advanced framerate conversions, image restoration and other tasks.

Filters: MSuper, MAnalyse, MCompensate, MMask, MDeGrain1, MDeGrain2, MDegrain3

Used Version: 2.7.31.0

https://github.com/pinterf/mvtools


NicAudio
~~~~~~~~

AviSynth audio source filter.

Filters: NicAC3Source, NicDTSSource, NicMPASource, RaWavSource

Used Version: 1.1

http://avisynth.org.ru/docs/english/externalfilters/nicaudio.htm


QTGMC
~~~~~

A very high quality deinterlacer with a range of features for both quality and convenience. These include a simple presets system, extensive noise processing capabilities, support for repair of progressive material, precision source matching, shutter speed simulation, etc. Originally based on TempGaussMC by Didée.

Filters: QTGMC

Used Version: 3.361s

http://avisynth.nl/index.php/QTGMC


RgTools
~~~~~~~

RgTools is a modern rewrite of RemoveGrain, Repair, BackwardClense, Clense, ForwardClense and VerticalCleaner all in a single plugin.

Filters: RemoveGrain, Clense, ForwardClense, BackwardClense, Repair, VerticalCleaner

Used Version: 0.96

https://github.com/pinterf/RgTools


SangNom2
~~~~~~~~

SangNom2 is a reimplementation of MarcFD's old SangNom filter. Originally it's a single field deinterlacer using edge-directed interpolation but nowadays it's mainly used in anti-aliasing scripts. The output is not completely but mostly identical to the original SangNom.

Filters: SangNom2

Used Version: 0.35

http://avisynth.nl/index.php/SangNom2


SMDegrain
~~~~~~~~~

SMDegrain, the Simple MDegrain Mod, is mainly a convenience function for using MVTools.

Filters: SMDegrain

Used Version: 3.1.2.97s

http://avisynth.nl/index.php/SMDegrain


SmoothAdjust
~~~~~~~~~~~~

SmoothAdjust is a set of 5 plugins to make YUV adjustements.

Filters: SmoothTweak, SmoothCurve, SmoothCustom, SmoothTools

Used Version: 3.20

https://forum.doom9.org/showthread.php?t=154971


TComb
~~~~~

TComb is a temporal comb filter.

Filters: TComb

Used Version: 2015-07-26

http://avisynth.nl/index.php/TComb


TDeint
~~~~~~

TDeint is a bi-directionally, motion adaptive, sharp deinterlacer. It can adaptively choose between using per-field and per-pixel motion adaptivity, and can use cubic interpolation, kernel interpolation (with temporal direction switching), or one of two forms of modified ELA interpolation which help to reduce "jaggy" edges in moving areas where interpolation must be used.

Filters: TDeint

Used Version: 1.1

http://avisynth.nl/index.php/TDeint


TIVTC
~~~~~

TIVTC is a plugin package containing 7 different filters and 3 conditional functions.

Filters: TFM, TDecimate, MergeHints, FrameDiff, FieldDiff, ShowCombedTIVTC, RequestLinear

Used Version: 1.0.11

https://github.com/pinterf/TIVTC


UnDot
~~~~~

UnDot is a simple median filter for removing dots, that is stray orphan pixels and mosquito noise.

Filters: UnDot

Used Version: 0.0.1.1

http://avisynth.nl/index.php/UnDot


vinverse
~~~~~~~~

A modern rewrite of a simple but effective plugin to remove residual combing originally based on an AviSynth script by Didée and then written as a plugin by tritical.

Filters: vinverse, vinverse2

Used Version: 2013-11-30

http://avisynth.nl/index.php/Vinverse


VSFilterMod
~~~~~~~~~~~

AviSynth subtitle plugin with support for vobsub srt and ass.

Filters: VobSub, TextSubMod

Used Version: 4

https://github.com/HomeOfVapourSynthEvolution/VSFilterMod


yadifmod2
~~~~~~~~~

Yet Another Deinterlacing Filter mod  for Avisynth2.6/Avisynth+

Filters: yadifmod2

Used Version: 0.0.4-1

https://github.com/chikuzen/yadifmod2


VapourSynth Plugins
-------------------
adjust
~~~~~~

very basic port of the built-in Avisynth filter Tweak.

Filters: adjust.Tweak

Used Version: 2015-03-22

https://github.com/dubhater/vapoursynth-adjust


d2vsource
~~~~~~~~~

Source filter to open D2V index files created with DGIndex or D2VWitch.

Filters: d2v.Source

Used Version: 1.0

https://github.com/dwbuiten/d2vsource


Deblock
~~~~~~~

Deblocking plugin using the deblocking filter of h264.

Filters: deblock.Deblock

Used Version: 6

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-Deblock/


DeLogo
~~~~~~

DeLogo Plugin Ported for VapourSynth.

Filters: delogo.AddLogo, delogo.EraseLogo

Used Version: 0.4

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DeLogo


ffms2
~~~~~

AviSynth+ and VapourSynth source filter supporting various input formats.

Filters: ffms2

Used Version: 2.23.1

https://github.com/FFMS/ffms2


FFT3DFilter
~~~~~~~~~~~

FFT3DFilter uses Fast Fourier Transform method for image processing in frequency domain.

Filters: fft3dfilter.FFT3DFilter

Used Version: 2015

https://github.com/VFR-maniac/VapourSynth-FFT3DFilter


finesharp
~~~~~~~~~

Port of Didée's FineSharp script to VapourSynth.

Filters: finesharp.sharpen

Used Version: 2016-08-21

http://forum.doom9.org/showthread.php?p=1777860#post1777860


flash3kyuu_deband
~~~~~~~~~~~~~~~~~

Simple debanding filter that can be quite effective for some anime sources.

Filters: f3kdb.Deband

Used Version: 2015-05-02

http://forum.doom9.org/showthread.php?t=161411


FluxSmooth
~~~~~~~~~~

FluxSmooth is a filter for smoothing of fluctuations.

Filters: SmoothT, SmoothST

Used Version: 1.0

https://github.com/dubhater/vapoursynth-fluxsmooth


fmtconv
~~~~~~~

Fmtconv is a format-conversion plug-in for the Vapoursynth video processing engine. It does resizing, bitdepth conversion with dithering and colorspace conversion.

Filters: fmtc.bitdepth, fmtc.convert, fmtc.matrix, fmtc.resample, fmtc.transfer

Used Version: 20

http://github.com/EleonoreMizo/fmtconv


GradCurve
~~~~~~~~~

VapourSynth port of Gradation Curves Virtual Dub Plugin.

Filters: grad.Curve

Used Version: 2.0

https://github.com/xekon/GradCurve


havsfunc
~~~~~~~~

Various popular AviSynth scripts ported to VapourSynth.

Filters: havsfunc.QTGMC, havsfunc.ediaa, havsfunc.daa, havsfunc.maa, havsfunc.SharpAAMCmod, havsfunc.Deblock_QED, havsfunc.DeHalo_alpha, havsfunc.YAHR, havsfunc.HQDeringmod, havsfunc.ivtc_txt60mc, havsfunc.Vinverse, havsfunc.Vinverse2, havsfunc.logoNR, havsfunc.LUTDeCrawl, havsfunc.LUTDeRainbow, havsfunc.GSMC, havsfunc.SMDegrain, havsfunc.SmoothLevels, havsfunc.FastLineDarkenMOD, havsfunc.LSFmod, havsfunc.GrainFactory3

Used Version: 2017-03-06

https://github.com/HomeOfVapourSynthEvolution/havsfunc


KNLMeansCL
~~~~~~~~~~

KNLMeansCL is an optimized pixelwise OpenCL implementation of the Non-local means denoising algorithm. Every pixel is restored by the weighted average of all pixels in its search window. The level of averaging is determined by the filtering parameter h.

Filters: knlm.KNLMeansCL

Used Version: 1.1.0

https://github.com/Khanattila/KNLMeansCL


msmoosh
~~~~~~~

MSmooth is a spatial smoother that doesn't touch edges.
MSharpen is a sharpener that tries to sharpen only edges.

Filters: msmoosh.MSmooth, msmoosh.MSharpen

Used Version: 1.1

https://github.com/dubhater/vapoursynth-msmoosh


mvsfunc
~~~~~~~

mawen1250's VapourSynth functions.

Filters: mvsfunc.Depth, mvsfunc.ToRGB, mvsfunc.ToYUV, mvsfunc.BM3D, mvsfunc.PlaneStatistics, mvsfunc.PlaneCompare, mvsfunc.ShowAverage, mvsfunc.FilterIf, mvsfunc.FilterCombed, mvsfunc.Min, mvsfunc.Max, mvsfunc.Avg, mvsfunc.MinFilter, mvsfunc.MaxFilter, mvsfunc.LimitFilter, mvsfunc.PointPower, mvsfunc.SetColorSpace, mvsfunc.AssumeFrame, mvsfunc.AssumeTFF, mvsfunc.AssumeBFF, mvsfunc.AssumeField, mvsfunc.AssumeCombed, mvsfunc.CheckVersion, mvsfunc.GetMatrix, mvsfunc.zDepth, mvsfunc.GetPlane, mvsfunc.PlaneAverage

Used Version: 8

https://github.com/HomeOfVapourSynthEvolution/mvsfunc


mvtools
~~~~~~~

MVTools is a set of filters for motion estimation and compensation.

Filters: mv.Super, mv.Analyse, mv.Recalculate, mv.Compensate, mv.Degrain1, mv.Degrain2, mv.Degrain3, mv.Mask, mv.Finest, mv.FlowBlur, mv.FlowInter, mv.FlowFPS, mv.BlockFPS, mv.SCDetection

Used Version: 19

http://github.com/dubhater/vapoursynth-mvtools


nnedi3
~~~~~~

nnedi3 is an intra-field only deinterlacer. It takes in a frame, throws away one field, and then interpolates the missing pixels using only information from the kept field.

Filters: nnedi3.nnedi3, nnedi3.nnedi3_rpow2

Used Version: 10

http://github.com/dubhater/vapoursynth-nnedi3


scenechange
~~~~~~~~~~~



Filters: scenechange

Used Version: 2014-09-25




temporalsoften
~~~~~~~~~~~~~~



Filters: TemporalSoften

Used Version: 2014-09-25




vcmod
~~~~~

vcmod plugin for VapourSynth.

Filters: vcmod.Median, vcmod.Variance, vcmod.Amplitude, vcmod.GBlur, vcmod.MBlur, vcmod.Histogram

Used Version: r24

http://www.avisynth.nl/users/vcmohan/vcmod/vcmod.html


VSFilterMod
~~~~~~~~~~~

AviSynth subtitle plugin with support for vobsub srt and ass.

Filters: vsfm.VobSub, vsfm.TextSubMod

Used Version: 4

https://github.com/HomeOfVapourSynthEvolution/VSFilterMod


vslsmashsource
~~~~~~~~~~~~~~

VapourSynth source filter based on Libav supporting a wide range of input formats.

Filters: lsmas.LibavSMASHSource, lsmas.LWLibavSource

Used Version: 929

http://avisynth.nl/index.php/LSMASHSource


Yadifmod
~~~~~~~~

Modified version of Fizick's avisynth filter port of yadif from mplayer. This version doesn't internally generate spatial predictions, but takes them from an external clip.

Filters: yadifmod.Yadifmod

Used Version: 10

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-Yadifmod


