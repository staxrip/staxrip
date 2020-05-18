Tools
=====

Console App
-----------
AutoCrop
~~~~~~~~

AutoCrop console tool.




avs2pipemod
~~~~~~~~~~~

Given an AviSynth script as input, avs2pipemod can send video, audio, or information of various types to stdout for consumption by command line encoders or other tools.

http://github.com/chikuzen/avs2pipemod


AVSMeter
~~~~~~~~

AVSMeter displays AviSynth script clip info, CPU and memory usage and the minimum, maximum and average frames processed per second. It measures how fast Avisynth can serve frames to a client application and comes in handy when testing filters/plugins to evaluate their performance and memory requirements.

http://forum.doom9.org/showthread.php?t=174797


dsmux
~~~~~

dsmux is installed by the Haali Splitter and can be used to mux TS containing AVC into MKV in order to fix av sync problems.

http://haali.su/mkv


eac3to
~~~~~~

Audio convertor console app.

http://forum.doom9.org/showthread.php?t=125966


fdkaac
~~~~~~

Console AAC encoder based on libfdk-aac.

http://github.com/nu774/fdkaac


ffmpeg
~~~~~~

Versatile audio video convertor.

http://ffmpeg.org


mkvextract
~~~~~~~~~~

MKV demuxing tool.

https://mkvtoolnix.download/


mkvinfo
~~~~~~~

MKV info tool.

https://mkvtoolnix.download/


mkvmerge
~~~~~~~~

MKV muxing tool.

https://mkvtoolnix.download/


MP4Box
~~~~~~

MP4Box is a MP4 muxing and demuxing console app.

http://gpac.wp.mines-telecom.fr/


mtn
~~~

movie thumbnailer saves thumbnails (screenshots) of movie or video files to jpeg files. StaxRip uses a custom built version with HEVC support added in and also includes the latest FFMPEG.

https://github.com/Revan654/Movie-Thumbnailer-mtn


NVEnc
~~~~~

NVIDIA hardware video encoder.

http://github.com/rigaya/NVEnc


PNGopt
~~~~~~

Opt Tools For Creating PNG

https://sourceforge.net/projects/apng/files/


Python
~~~~~~

Python is required by VapourSynth.

http://www.python.org


qaac
~~~~

qaac is a console AAC encoder frontend based on the Apple AAC encoder.

http://github.com/nu774/qaac


QSVEnc
~~~~~~

Intel hardware video encoder.

http://github.com/rigaya/QSVEnc


rav1e
~~~~~

A Faster and Safer AV1 Encoder

https://github.com/xiph/rav1e


SVT-AV1
~~~~~~~

Intel AV1 encoder.

https://github.com/OpenVisualCloud/SVT-AV1


VCEEnc
~~~~~~

AMD hardware video encoder.

http://github.com/rigaya/VCEEnc


vspipe
~~~~~~

vspipe is part of VapourSynth and used to pipe VapourSynth scripts to encoding apps.

http://www.vapoursynth.com


x264
~~~~

H.264 video encoding console app.

http://www.videolan.org/developers/x264.html


x265
~~~~

H.265 video encoding console app.

http://x265.org


xvid_encraw
~~~~~~~~~~~

XviD console encoder

https://www.xvid.com


GUI App
-------
BDSup2Sub++
~~~~~~~~~~~

Converts Blu-ray subtitles to other formats like VobSub.

https://github.com/amichaeltm/BDSup2SubPlusPlus


chapterEditor
~~~~~~~~~~~~~

ChapterEditor is a chapter editor and menu editor for OGG, XML, TTXT, m.AVCHD, m.editions-mkv, Matroska Menu.

https://forum.doom9.org/showthread.php?t=169984


DGIndex
~~~~~~~

MPEG-2 demuxing and indexing app.




MPC-BE
~~~~~~

DirectShow based media player.

https://sourceforge.net/projects/mpcbe/


MPC-HC
~~~~~~

DirectShow based media player.

https://mpc-hc.org/


mpv.net
~~~~~~~

The worlds best media player.

https://github.com/stax76/mpv.net


Subtitle Edit
~~~~~~~~~~~~~

Subtitle Edit is a open source subtitle editor.

http://www.nikse.dk/SubtitleEdit


VSRip
~~~~~

VSRip rips VobSub subtitles.

http://sourceforge.net/projects/guliverkli


AviSynth Plugin
---------------
AddGrainC
~~~~~~~~~

Generate film-like grain or other effects (like rain) by adding random noise to a video clip.

Filters: AddGrainC, AddGrain

http://avisynth.nl/index.php/AddGrainC


AutoAdjust
~~~~~~~~~~

AutoAdjust is an automatic adjustement filter. It calculates statistics of clip, stabilizes them temporally and uses them to adjust luminance gain & color balance.

Filters: AutoAdjust

http://forum.doom9.org/showthread.php?t=167573


Average
~~~~~~~

A simple plugin that calculates a weighted frame-by-frame average from multiple clips. This is a modern rewrite of the old Average plugin but a bit faster, additional colorspace support, and some additional sanity checks.

Filters: Average

http://avisynth.nl/index.php/Average


AviSynthShader DLL
~~~~~~~~~~~~~~~~~~



Filters: SuperRes, SuperResXBR, SuperXBR, ResizeShader, SuperResPass, SuperXbrMulti, ResizeShader

https://github.com/mysteryx93/AviSynthShader


AvsResize
~~~~~~~~~



Filters: z_ConvertFormat, z_PointResize, z_BilinearResize, z_BicubicResize, z_LanczosResize, z_Lanczos4Resize, z_BlackmanResize, z_Spline16Resize, z_Spline36Resize, z_Spline64Resize, z_GaussResize, z_SincResize

http://forum.doom9.org/showthread.php?t=173986


AVSTP
~~~~~

AVSTP is a programming library for Avisynth plug-in developers. It helps supporting native multi-threading in plug-ins. It works by sharing a thread pool between multiple plug-ins, so the number of threads stays low whatever the number of instantiated plug-ins. This helps saving resources, especially when working in an Avisynth MT environment. This documentation is mostly targeted to plug-ins developpers, but contains installation instructions for Avisynth users too.

Filters: avstp_set_threads

http://avisynth.nl/index.php/AVSTP


checkmate
~~~~~~~~~

Spatial and temporal dot crawl reducer. Checkmate is most effective in static or low motion scenes. When using in high motion scenes (or areas) be careful, it's known to cause artifacts with its default values.

Filters: checkmate

http://github.com/tp7/checkmate


CNR2
~~~~

A fast chroma denoiser. Very effective against stationary rainbows and huge analogic chroma activity. Useful to filter VHS/TV caps.

Filters: cnr2

http://avisynth.nl/index.php/Cnr2


DCTFilter
~~~~~~~~~

A rewrite of DctFilter for Avisynth+.

Filters: DCTFilter, DCTFilterD, DCTFilter4, DCTFilter4D, DCTFilter8, DCTFilter8D

http://github.com/chikuzen/DCTFilter


Deblock
~~~~~~~

Deblocking plugin using the deblocking filter of h264.

Filters: Deblock

http://avisynth.nl/index.php/DeBlock


Decomb
~~~~~~

This package of plugin functions for Avisynth provides the means for removing combing artifacts from telecined progressive streams, interlaced streams, and mixtures thereof. Functions can be combined to implement inverse telecine (IVTC) for both NTSC and PAL streams.

Filters: Telecide, FieldDeinterlace, Decimate, IsCombed

http://rationalqm.us/decomb/decombnew.html


DeGrainMedian
~~~~~~~~~~~~~

DeGrainMedian is a spatio-temporal limited median filter mainly for film grain removal, but may be used for general denoising.

Filters: DeGrainMedian

http://avisynth.nl/index.php/DeGrainMedian


DePan
~~~~~



Filters: DePan, DePanInterleave, DePanStabilize, DePanScenes

http://avisynth.nl/index.php/DePan


DePanEstimate
~~~~~~~~~~~~~



Filters: DePanEstimate

http://avisynth.nl/index.php/DePan


DFTTest
~~~~~~~

2D/3D frequency domain denoiser using Discrete Fourier transform.

Filters: dfttest

https://github.com/pinterf/dfttest


Dither DLL
~~~~~~~~~~

This package offers a set of tools to manipulate high-bitdepth (16 bits per plane) video clips. The most proeminent features are color banding artifact removal, dithering to 8 bits, colorspace conversions and resizing.

Filters: Dither_y_gamma_to_linear, Dither_y_linear_to_gamma, Dither_convert_8_to_16, Dither1Pre, Dither1Pre, Dither_repair16, Dither_convert_yuv_to_rgb, Dither_convert_rgb_to_yuv, Dither_resize16, DitherPost, Dither_crop16, DitherBuildMask, SmoothGrad, GradFun3, Dither_box_filter16, Dither_bilateral16, Dither_limit_dif16, Dither_resize16nr, Dither_srgb_display, Dither_convey_yuv4xxp16_on_yvxx, Dither_convey_rgb48_on_yv12, Dither_removegrain16, Dither_median16, Dither_get_msb, Dither_get_lsb, Dither_addborders16, Dither_lut8, Dither_lutxy8, Dither_lutxyz8, Dither_lut16, Dither_add16, Dither_sub16, Dither_max_dif16, Dither_min_dif16, Dither_merge16, Dither_merge16_8, Dither_sigmoid_direct, Dither_sigmoid_inverse, Dither_add_grain16, Dither_Luma_Rebuild

http://avisynth.nl/index.php/Dither


DSS2mod
~~~~~~~

Direct Show source filter

Filters: DSS2

http://code.google.com/p/xvid4psp/downloads/detail?name=DSS2%20mod%20%2B%20LAVFilters.7z&can=2&q=


EEDI2
~~~~~

EEDI2 (Enhanced Edge Directed Interpolation) resizes an image by 2x in the vertical direction by copying the existing image to 2*y(n) and interpolating the missing field.

Filters: EEDI2

http://avisynth.nl/index.php/EEDI2


EEDI3
~~~~~

EEDI3 (Enhanced Edge Directed Interpolation) resizes an image by 2x in the vertical direction by copying the existing image to 2*y(n) and interpolating the missing field.

Filters: EEDI3

http://avisynth.nl/index.php/EEDI3


ffms2
~~~~~

AviSynth+ and VapourSynth source filter supporting various input formats.

Filters: FFVideoSource, FFAudioSource

http://github.com/FFMS/ffms2


FFT3DFilter
~~~~~~~~~~~

FFT3DFilter uses Fast Fourier Transform method for image processing in frequency domain.

Filters: FFT3DFilter

http://github.com/pinterf/fft3dfilter


FFT3DGPU
~~~~~~~~

Similar algorithm to FFT3DFilter, but uses graphics hardware for increased speed.

Filters: FFT3DGPU




flash3kyuu_deband
~~~~~~~~~~~~~~~~~

Simple debanding filter that can be quite effective for some anime sources.

Filters: f3kdb

http://forum.doom9.org/showthread.php?t=161411


FluxSmooth
~~~~~~~~~~

One of the fundamental properties of noise is that it's random. One of the fundamental properties of motion is that it's not. This is the premise behind FluxSmooth, which examines each pixel and compares it to the corresponding pixel in the previous and last frame. Smoothing occurs if both the previous frame's value and the next frame's value are greater, or if both are less, than the value in the current frame.

Filters: FluxSmoothT, FluxSmoothST

http://avisynth.nl/index.php/FluxSmooth


FrameRateConverter DLL
~~~~~~~~~~~~~~~~~~~~~~

Increases the frame rate with interpolation and fine artifact removal 

Filters: FrameRateConverter

https://github.com/mysteryx93/FrameRateConverter


GradFun2DB
~~~~~~~~~~

A simple and fast debanding filter.

Filters: gradfun2db

http://avisynth.nl/index.php/GradFun2db


HQDN3D
~~~~~~



Filters: HQDN3D

http://avisynth.nl/index.php/Hqdn3d


JincResize
~~~~~~~~~~

Jinc (EWA Lanczos) resampling plugin for AviSynth 2.6/AviSynth+.

Filters: Jinc36Resize, Jinc64Resize, Jinc144Resize, Jinc256Resize

http://avisynth.nl/index.php/JincResize


JPSDR
~~~~~

Merge of AutoYUY2, NNEDI3, HDRTools, aWarpSharpMT and ResampleMT. Included is the W7 AVX variant.

Filters: aBlur, aSobel, AutoYUY2, aWarp, aWarp4, aWarpSharp2, BicubicResizeMT, BilinearResizeMT, BlackmanResizeMT, ConvertLinearRGBtoYUV, ConvertRGB_Hable_HDRtoSDR, ConvertRGB_Mobius_HDRtoSDR, ConvertRGB_Reinhard_HDRtoSDR, ConvertRGBtoXYZ, ConvertXYZ_Hable_HDRtoSDR, ConvertXYZ_Mobius_HDRtoSDR, ConvertXYZ_Reinhard_HDRtoSDR, ConvertXYZ_Scale_HDRtoSDR, ConvertXYZ_Scale_SDRtoHDR, ConvertXYZtoRGB, ConvertXYZtoYUV, ConvertYUVtoLinearRGB, ConvertYUVtoXYZ, DeBicubicResizeMT, DeBilinearResizeMT, DeBlackmanResizeMT, DeGaussResizeMT, DeLanczos4ResizeMT, DeLanczosResizeMT, DeSincResizeMT, DeSpline16ResizeMT, DeSpline36ResizeMT, DeSpline64ResizeMT, GaussResizeMT, Lanczos4ResizeMT, LanczosResizeMT, nnedi3, PointResizeMT, SincResizeMT, Spline16ResizeMT, Spline36ResizeMT, Spline64ResizeMT

http://forum.doom9.org/showthread.php?t=174248


KNLMeansCL
~~~~~~~~~~

KNLMeansCL is an optimized pixelwise OpenCL implementation of the Non-local means denoising algorithm. Every pixel is restored by the weighted average of all pixels in its search window. The level of averaging is determined by the filtering parameter h.

Filters: KNLMeansCL

http://github.com/Khanattila/KNLMeansCL


L-SMASH-Works
~~~~~~~~~~~~~

AviSynth and VapourSynth source filter based on Libav supporting a wide range of input formats.

Filters: LSMASHVideoSource, LSMASHAudioSource, LWLibavVideoSource, LWLibavAudioSource

https://github.com/HolyWu/L-SMASH-Works


masktools2
~~~~~~~~~~

MaskTools2 contain a set of filters designed to create, manipulate and use masks. Masks, in video processing, are a way to give a relative importance to each pixel. You can, for example, create a mask that selects only the green parts of the video, and then replace those parts with another video.

Filters: mt_adddiff, mt_average, mt_binarize, mt_circle, mt_clamp, mt_convolution, mt_diamond, mt_edge, mt_ellipse, mt_expand, mt_hysteresis, mt_inflate, mt_inpand, mt_invert, mt_logic, mt_losange, mt_lut, mt_lutf, mt_luts, mt_lutxy, mt_makediff, mt_mappedblur, mt_merge, mt_motion, mt_polish, mt_rectangle, mt_square

http://github.com/pinterf/masktools


MedianBlur2
~~~~~~~~~~~

Implementation of constant time median filter for AviSynth.

Filters: MedianBlur, MedianBlurTemporal

http://avisynth.nl/index.php/MedianBlur2


MiniDeen
~~~~~~~~

MiniDeen is a spatial denoising filter. It replaces every pixel with the average of its neighbourhood.

Filters: MiniDeen

https://github.com/HomeOfAviSynthPlusEvolution/MiniDeen


MipSmooth
~~~~~~~~~

a reinvention of SmoothHiQ and Convolution3D. MipSmooth was made to enable smoothing of larger pixel areas than 3x3(x3), to remove blocks and smoothing out low-frequency noise.

Filters: MipSmooth

http://avisynth.org.ru/docs/english/externalfilters/mipsmooth.htm


modPlus
~~~~~~~

This plugin has 9 functions, which modify values of color components to attenuate noise, blur or equalize input.

Filters: GBlur, MBlur, Median, minvar, Morph, SaltPepper, SegAmp, TweakHist, Veed

http://www.avisynth.nl/users/vcmohan/modPlus/modPlus.html


MPEG2DecPlus
~~~~~~~~~~~~

Source filter to open D2V index files created with DGIndex or D2VWitch.

Filters: MPEG2Source

http://github.com/chikuzen/MPEG2DecPlus


MSharpen
~~~~~~~~



Filters: MSharpen

http://avisynth.nl/index.php/MSharpen


mvtools2
~~~~~~~~

MVTools is collection of functions for estimation and compensation of objects motion in video clips. Motion compensation may be used for strong temporal denoising, advanced framerate conversions, image restoration and other tasks.

Filters: MSuper, MAnalyse, MCompensate, MMask, MDeGrain1, MDeGrain2, MDegrain3

http://github.com/pinterf/mvtools


NicAudio
~~~~~~~~

AviSynth audio source filter.

Filters: NicAC3Source, NicDTSSource, NicMPASource, RaWavSource

http://avisynth.org.ru/docs/english/externalfilters/nicaudio.htm


RgTools
~~~~~~~

RgTools is a modern rewrite of RemoveGrain, Repair, BackwardClense, Clense, ForwardClense and VerticalCleaner all in a single plugin.

Filters: RemoveGrain, Clense, ForwardClense, BackwardClense, Repair, VerticalCleaner

http://github.com/pinterf/RgTools


SangNom2
~~~~~~~~

SangNom2 is a reimplementation of MarcFD's old SangNom filter. Originally it's a single field deinterlacer using edge-directed interpolation but nowadays it's mainly used in anti-aliasing scripts. The output is not completely but mostly identical to the original SangNom.

Filters: SangNom2

http://avisynth.nl/index.php/SangNom2


SmoothAdjust
~~~~~~~~~~~~

SmoothAdjust is a set of 5 plugins to make YUV adjustements.

Filters: SmoothTweak, SmoothCurve, SmoothCustom, SmoothTools

http://forum.doom9.org/showthread.php?t=154971


SmoothD2
~~~~~~~~

Deblocking filter. Rewrite of SmoothD. Faster, better detail preservation, optional chroma deblocking.

Filters: SmoothD2

http://avisynth.nl/index.php/SmoothD2


SVPFlow 1
~~~~~~~~~

Motion vectors search plugin  is a deeply refactored and modified version of MVTools2 Avisynth plugin

Filters: analyse_params, super_params, SVSuper, SVAnalyse

http://avisynth.nl/index.php/SVPFlow


SVPFlow 2
~~~~~~~~~

Motion vectors search plugin is a deeply refactored and modified version of MVTools2 Avisynth plugin

Filters: smoothfps_params, SVConvert, SVSmoothFps

http://avisynth.nl/index.php/SVPFlow


TDeint
~~~~~~

TDeint is a bi-directionally, motion adaptive, sharp deinterlacer.

Filters: TDeint

http://avisynth.nl/index.php/TDeint


TEMmod
~~~~~~

TEMmod creates an edge mask using gradient vector magnitude. 

Filters: TEMmod

http://avisynth.nl/index.php/TEMmod


TIVTC
~~~~~

TIVTC is a plugin package containing 7 different filters and 3 conditional functions.

Filters: TFM, TDecimate, MergeHints, FrameDiff, FieldDiff, ShowCombedTIVTC, RequestLinear

http://github.com/pinterf/TIVTC


TMM2
~~~~

TMM builds a motion-mask for TDeint, which TDeint uses via its 'emask' parameter.

Filters: TMM2

http://avisynth.nl/index.php/TMM


TNLMeans
~~~~~~~~

TNLMeans is an implementation of the NL-means denoising algorithm. Aside from the original method, TNLMeans also supports extension into 3D, a faster, block based approach, and a multiscale version.

Filters: TNLMeans

http://avisynth.nl/index.php/TNLMeans


UnDot
~~~~~

UnDot is a simple median filter for removing dots, that is stray orphan pixels and mosquito noise.

Filters: UnDot

http://avisynth.nl/index.php/UnDot


VagueDenoiser
~~~~~~~~~~~~~

This is a Wavelet based Denoiser. Basically, it transforms each frame from the video input into the wavelet domain, using various wavelet filters. Then it applies some filtering to the obtained coefficients.

Filters: VagueDenoiser

http://avisynth.nl/index.php/VagueDenoiser


VapourSource
~~~~~~~~~~~~

VapourSource is a VapourSynth script reader for AviSynth 2.6.

Filters: VSImport, VSEval

http://avisynth.nl/index.php/VapourSource


vinverse
~~~~~~~~

Simple but effective plugin to remove residual combing.

Filters: vinverse, vinverse2

http://avisynth.nl/index.php/Vinverse


vsCube
~~~~~~

Deblocking plugin using the deblocking filter of h264.

Filters: Cube

http://rationalqm.us/mine.html


VSFilterMod
~~~~~~~~~~~

AviSynth and VapourSynth subtitle plugin with support for vobsub srt and ass.

Filters: VobSub, TextSubMod

https://github.com/sorayuki/VSFilterMod


xNLMeans
~~~~~~~~

XNLMeans is an AviSynth plugin implementation of the Non Local Means denoising algorithm

Filters: xNLMeans

http://avisynth.nl/index.php/xNLMeans


yadifmod2
~~~~~~~~~

Yet Another Deinterlacing Filter mod  for Avisynth2.6/Avisynth+

Filters: yadifmod2

http://github.com/chikuzen/yadifmod2


AviSynth Script
---------------
AnimeIVTC
~~~~~~~~~



Filters: AnimeIVTC

http://avisynth.nl/index.php/AnimeIVTC


AviSynthShader AVSI
~~~~~~~~~~~~~~~~~~~



Filters: SuperRes, SuperResXBR, SuperXBR, ResizeShader, SuperResPass, SuperXbrMulti, ResizeShader

https://github.com/mysteryx93/AviSynthShader


CropResize
~~~~~~~~~~

Advanced crop and resize AviSynth script.

Filters: CropResize

https://forum.videohelp.com/threads/393752-CropResize-Cropping-resizing-script


DAA3Mod
~~~~~~~

Motion-Compensated Anti-aliasing with contra-sharpening, can deal with ifade too, created because when applied daa3 to fixed scenes, it could damage some details and other issues.

Filters: daa3mod, mcdaa3

http://avisynth.nl/index.php/daa3


Deblock_QED
~~~~~~~~~~~

Designed to provide 8x8 deblocking sensitive to the amount of blocking in the source, compared to other deblockers which apply a uniform deblocking across every frame. 

Filters: Deblock_QED

http://avisynth.nl/index.php/Deblock_QED


DehaloAlpha
~~~~~~~~~~~

Reduce halo artifacts that can occur when sharpening.

Filters: DeHalo_alpha_mt, DeHalo_alpha_2BD




DeNoise Histogram
~~~~~~~~~~~~~~~~~

Histogram for both DenoiseMD and DenoiseMF

Filters: DiffCol

http://avisynth.nl


DeNoiseMD
~~~~~~~~~

A fast and accurate denoiser for a Full HD video from a H.264 camera. 

Filters: DeNoiseMD1, DenoiseMD2

http://avisynth.nl


DeNoiseMF
~~~~~~~~~

A fast and accurate denoiser for a Full HD video from a H.264 camera. 

Filters: DeNoiseMF1, DenoiseMF2

http://avisynth.nl


Dither AVSI
~~~~~~~~~~~

This package offers a set of tools to manipulate high-bitdepth (16 bits per plane) video clips. The most proeminent features are color banding artifact removal, dithering to 8 bits, colorspace conversions and resizing.

Filters: Dither_y_gamma_to_linear, Dither_y_linear_to_gamma, Dither_convert_8_to_16, Dither1Pre, Dither1Pre, Dither_repair16, Dither_convert_yuv_to_rgb, Dither_convert_rgb_to_yuv, Dither_resize16, DitherPost, Dither_crop16, DitherBuildMask, SmoothGrad, GradFun3, Dither_box_filter16, Dither_bilateral16, Dither_limit_dif16, Dither_resize16nr, Dither_srgb_display, Dither_convey_yuv4xxp16_on_yvxx, Dither_convey_rgb48_on_yv12, Dither_removegrain16, Dither_median16, Dither_get_msb, Dither_get_lsb, Dither_addborders16, Dither_lut8, Dither_lutxy8, Dither_lutxyz8, Dither_lut16, Dither_add16, Dither_sub16, Dither_max_dif16, Dither_min_dif16, Dither_merge16, Dither_merge16_8, Dither_sigmoid_direct, Dither_sigmoid_inverse, Dither_add_grain16, Dither_Luma_Rebuild

http://avisynth.nl/index.php/Dither


edi_rpow2 AVSI
~~~~~~~~~~~~~~

An improved rpow2 function for nnedi3, nnedi3ocl, eedi3, and eedi2.

Filters: nnedi3_rpow2

http://avisynth.nl/index.php/nnedi3


eedi3_resize
~~~~~~~~~~~~

eedi3 based resizing script that allows to resize to arbitrary resolutions while maintaining the correct image center and chroma location.

Filters: eedi3_resize

http://avisynth.nl/index.php/eedi3


FineDehalo
~~~~~~~~~~

Halo removal script that uses DeHalo_alpha with a few masks and optional contra-sharpening to try remove halos without removing important details (like line edges). It also includes FineDehalo2, this function tries to remove 2nd order halos. See script for extensive information. 

Filters: FineDehalo

http://avisynth.nl/index.php/FineDehalo


FineSharp
~~~~~~~~~

Small and fast realtime-sharpening function for 1080p, or after scaling 720p -> 1080p. It's a generic sharpener only for good quality sources!

Filters: FineSharp

http://avisynth.nl/index.php/FineSharp


FrameRateConverter AVSI
~~~~~~~~~~~~~~~~~~~~~~~

Increases the frame rate with interpolation and fine artifact removal 

Filters: FrameRateConverter

https://github.com/mysteryx93/FrameRateConverter


GradFun2DBmod
~~~~~~~~~~~~~

An advanced debanding script based on GradFun2DB.

Filters: GradFun2DBmod

http://avisynth.nl/index.php/GradFun2dbmod


HQDeringmod
~~~~~~~~~~~

Applies deringing by using a smart smoother near edges (where ringing occurs) only.

Filters: HQDeringmod

http://avisynth.nl/index.php/HQDering_mod


InterFrame
~~~~~~~~~~

A frame interpolation script that makes accurate estimations about the content of frames

Filters: InterFrame

http://avisynth.nl/index.php/InterFrame


Lazy Utilities
~~~~~~~~~~~~~~

A collection of helper and wrapper functions meant to help script authors in handling common operations 

Filters: LuStackedNto16, LuPlanarToStacked, LuRGB48YV12ToRGB48Y, LuIsFunction, LuSeparateColumns, LuMergePlanes, LuIsHD, LuConvCSP, Lu8To16, Lu16To8, LuIsEq, LuSubstrAtIdx, LuSubstrCnt, LuReplaceStr, LUIsDefined, LuMerge, LuLut, LuLimitDif, LuBlankClip, LuIsSameRes

https://github.com/AviSynth/avs-scripts


LSFmod
~~~~~~

A LimitedSharpenFaster mod with a lot of new features and optimizations.

Filters: LSFmod

http://avisynth.nl/index.php/LSFmod


MAA2Mod
~~~~~~~

Updated version of the MAA2+ antialising script from AnimeIVTC. MAA2 uses tp7's SangNom2, which provide a nice speedup for SangNom-based antialiasing. Mod version also includes support for EEDI3 along with a few other new functions.

Filters: MAA2

http://avisynth.nl/index.php/MAA2


mClean
~~~~~~

Removes noise whilst retaining as much detail as possible.

Filters: mClean

http://forum.doom9.org/showthread.php?t=174804


MCTemporalDenoise
~~~~~~~~~~~~~~~~~

A motion compensated noise removal script with an accompanying post-processing component.

Filters: MCTemporalDenoise, MCTemporalDenoisePP

http://avisynth.nl/index.php/Abcxyz


MT Expand Multi
~~~~~~~~~~~~~~~

Calls mt_expand or mt_inpand multiple times in order to grow or shrink the mask from the desired width and height.

Filters: mt_expand_multi, mt_inpand_multi

http://avisynth.nl/index.php/Dither


MultiSharpen
~~~~~~~~~~~~

A small but useful Sharpening Function

Filters: MultiSharpen




nnedi3 AVSI
~~~~~~~~~~~

nnedi3 is an AviSynth 2.5 plugin, but supports all new planar colorspaces when used with AviSynth 2.6

Filters: nnedi3_resize16

http://avisynth.nl/index.php/nnedi3


nnedi3x AVSI
~~~~~~~~~~~~

nnedi3x is an AviSynth 2.5 plugin, but supports all new planar colorspaces when used with AviSynth 2.6

Filters: nnedi3x

http://avisynth.nl/index.php/nnedi3


pSharpen
~~~~~~~~

pSharpen performs two-point sharpening to avoid overshoot.

Filters: pSharpen

http://avisynth.nl/index.php/PSharpen


QTGMC
~~~~~

A very high quality deinterlacer with a range of features for both quality and convenience. These include a simple presets system, extensive noise processing capabilities, support for repair of progressive material, precision source matching, shutter speed simulation, etc. Originally based on TempGaussMC by Dide.

Filters: QTGMC

http://avisynth.nl/index.php/QTGMC


ResizeX
~~~~~~~



Filters: ResizeX

http://avisynth.nl


SMDegrain
~~~~~~~~~

SMDegrain, the Simple MDegrain Mod, is mainly a convenience function for using MVTools.

Filters: SMDegrain

http://avisynth.nl/index.php/SMDegrain


SmoothD2c
~~~~~~~~~

Deblocking filter. Rewrite of SmoothD. Faster, better detail preservation, optional chroma deblocking.

Filters: SmoothD2c

http://avisynth.nl/index.php/SmoothD2


YFRC
~~~~

Yushko Frame Rate convertor - doubles the frame rate with strong artifact detection and scene change detection. YFRC uses masks to reduce artifacts in areas where interpolation failed.

Filters: YFRC

http://avisynth.nl/index.php/YFRC


VapourSynth Plugin
------------------
AWarpSharp2
~~~~~~~~~~~

VapourSynth port of AWarpSharp2

Filters: warp.AWarpSharp2

https://github.com/dubhater/vapoursynth-awarpsharp2


BM3D
~~~~

BM3D denoising filter for VapourSynth

Filters: bm3d.RGB2OPP, bm3d.OPP2RGB, bm3d.Basic, bm3d.Final, bm3d.VBasic, bm3d.VFinal, bm3d.VAggregate

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-BM3D


Bwdif
~~~~~

Motion adaptive deinterlacing based on yadif with the use of w3fdif and cubic interpolation algorithms.

Filters: bwdif.Bwdif

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-Bwdif


CNR2
~~~~

Cnr2 is a temporal denoiser designed to denoise only the chroma.

Filters: cnr2.Cnr2

https://github.com/dubhater/vapoursynth-cnr2


CTMF
~~~~

Constant Time Median Filtering.

Filters: ctmf.CTMF

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-CTMF


d2vsource
~~~~~~~~~

Source filter to open D2V index files created with DGIndex or D2VWitch.

Filters: d2v.Source

http://github.com/dwbuiten/d2vsource


DCTFilter
~~~~~~~~~

Renewed VapourSynth port of DCTFilter.

Filters: dctf.DCTFilter

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DCTFilter


DCTFilter-f
~~~~~~~~~~~

Renewed VapourSynth port of DCTFilter.

Filters: dctf.DCTFilter

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DCTFilter


Deblock
~~~~~~~

Deblocking plugin using the deblocking filter of h264.

Filters: deblock.Deblock

http://github.com/HomeOfVapourSynthEvolution/VapourSynth-Deblock/


DeblockPP7
~~~~~~~~~~

VapourSynth port of pp7 from MPlayer.

Filters: pp7.DeblockPP7

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DeblockPP7


DegrainMedian
~~~~~~~~~~~~~

VapourSynth port of DegrainMedian

Filters: dgm.DegrainMedian

https://github.com/dubhater/vapoursynth-degrainmedian


DFTTest
~~~~~~~

VapourSynth port of dfttest.

Filters: dfttest.DFTTest

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-DFTTest


EEDI2
~~~~~

EEDI2 works by finding the best non-decreasing (non-crossing) warping between two lines by minimizing a cost functional.

Filters: eedi2.EEDI2

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-EEDI2


EEDI3m
~~~~~~

EEDI3 works by finding the best non-decreasing (non-crossing) warping between two lines by minimizing a cost functional.

Filters: eedi3m.EEDI3

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-EEDI3


FFT3DFilter
~~~~~~~~~~~

FFT3DFilter uses Fast Fourier Transform method for image processing in frequency domain.

Filters: fft3dfilter.FFT3DFilter

http://github.com/VFR-maniac/VapourSynth-FFT3DFilter


FixTelecinedFades
~~~~~~~~~~~~~~~~~

InsaneAA Anti-Aliasing Script.

Filters: ftf.FixFades

https://github.com/IFeelBloated/Fix-Telecined-Fades


FluxSmooth
~~~~~~~~~~

FluxSmooth is a filter for smoothing of fluctuations.

Filters: flux.SmoothT, flux.SmoothST

http://github.com/dubhater/vapoursynth-fluxsmooth


fmtconv
~~~~~~~

Fmtconv is a format-conversion plug-in for the Vapoursynth video processing engine. It does resizing, bitdepth conversion with dithering and colorspace conversion.

Filters: fmtc.bitdepth, fmtc.convert,  core.fmtc.matrix, fmtc.resample, fmtc.transfer, fmtc.primaries,  core.fmtc.matrix2020cl, fmtc.stack16tonative, nativetostack16

http://github.com/EleonoreMizo/fmtconv


HQDN3D
~~~~~~

Avisynth port of hqdn3d from avisynth/mplayer.

Filters: hqdn3d.Hqdn3d

https://github.com/Hinterwaeldlers/vapoursynth-hqdn3d


IT
~~

VapourSynth Plugin - Inverse Telecine (YV12 Only, IT-0051 base, IT_YV12-0103 base).

Filters: it.IT

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-IT


msmoosh
~~~~~~~

MSmooth is a spatial smoother that doesn't touch edges.
MSharpen is a sharpener that tries to sharpen only edges.

Filters: msmoosh.MSmooth, msmoosh.MSharpen

http://github.com/dubhater/vapoursynth-msmoosh


mvtools
~~~~~~~

MVTools is a set of filters for motion estimation and compensation.

Filters: mv.Super, mv.Analyse, mv.Recalculate, mv.Compensate, mv.Degrain1, mv.Degrain2, mv.Degrain3, mv.Mask, mv.Finest, mv.Flow, mv.FlowBlur, mv.FlowInter, mv.FlowFPS, mv.BlockFPS, mv.SCDetection, mv.DepanAnalyse, mv.DepanEstimate, mv.DepanCompensate, mv.DepanStabilise

http://github.com/dubhater/vapoursynth-mvtools


mvtools-sf
~~~~~~~~~~

MVTools is a set of filters for motion estimation and compensation.

Filters: mvsf.Super, mvsf.Analyse, mvsf.Recalculate, mvsf.Compensate, mvsf.Degrain1, mvsf.Degrain2, mvsf.Degrain3, mvsf.Mask, mvsf.Finest, mvsf.Flow, mvsf.FlowBlur, mvsf.FlowInter, mvsf.FlowFPS, mvsf.BlockFPS, mvsf.SCDetection, mvsf.DepanAnalyse, mvsf.DepanEstimate, mvsf.DepanCompensate, mvsf.DepanStabilise

http://github.com/dubhater/vapoursynth-mvtools


nnedi3
~~~~~~

nnedi3 is an intra-field only deinterlacer. It takes in a frame, throws away one field, and then interpolates the missing pixels using only information from the kept field.

Filters: nnedi3.nnedi3

http://github.com/dubhater/vapoursynth-nnedi3


nnedi3cl
~~~~~~~~

nnedi3 is an intra-field only deinterlacer. It takes a frame, throws away one field, and then interpolates the missing pixels using only information from the remaining field. It is also good for enlarging images by powers of two.

Filters: nnedi3cl.NNEDI3CL

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-NNEDI3CL


Sangnom
~~~~~~~

SangNom is a single field deinterlacer using edge-directed interpolation but nowadays it's mainly used in anti-aliasing scripts.

Filters: sangnom.SangNom

https://bitbucket.org/James1201/vapoursynth-sangnom/overview


scenechange
~~~~~~~~~~~



Filters: scenechange




SVPFlow 1
~~~~~~~~~

Motion vectors search plugin  is a deeply refactored and modified version of MVTools2 Avisynth plugin

Filters: core.svp1.Super, core.svp1.Analyse, core.svp1.Convert

https://www.svp-team.com/wiki/Manual:SVPflow


SVPFlow 2
~~~~~~~~~

Motion vectors search plugin is a deeply refactored and modified version of MVTools2 Avisynth plugin

Filters: core.svp2.SmoothFps

https://www.svp-team.com/wiki/Manual:SVPflow


TCanny
~~~~~~

Builds an edge map using canny edge detection.

Filters: tcanny.TCanny

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-TCanny


TDeintMod
~~~~~~~~~

TDeintMod is a combination of TDeint and TMM, which are both ported from tritical's AviSynth plugin.

Filters: tdm.TDeintMod

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-TDeintMod


TemporalMedian
~~~~~~~~~~~~~~

TemporalMedian is a temporal denoising filter. It replaces every pixel with the median of its temporal neighbourhood.

Filters: tmedian.TemporalMedian

https://github.com/dubhater/vapoursynth-temporalmedian


temporalsoften
~~~~~~~~~~~~~~



Filters: TemporalSoften




TimeCube
~~~~~~~~

Allows Usage of 3DLuts.

Filters: timecube.Cube

https://github.com/sekrit-twc/timecube


TTempSmooth
~~~~~~~~~~~

VapourSynth port of TTempSmooth.

Filters: ttmpsm.TTempSmooth

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-TTempSmooth


VagueDenoiser
~~~~~~~~~~~~~

VapourSynth port of VagueDenoiser.

Filters: vd.VagueDenoiser

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-VagueDenoiser


vcfreq
~~~~~~

vcvcfreq plugin for VapourSynth.

Filters: vcfreq.F1Quiver, vcfreq.F2Quiver, vcfreq.Blur, vcfreq.Sharp

http://www.avisynth.nl/users/vcmohan/vcfreq/vcfreq.html


vcmod
~~~~~

vcmod plugin for VapourSynth.

Filters: vcmod.Median, vcmod.Variance, vcmod.Amplitude, vcmod.GBlur, vcmod.MBlur, vcmod.Histogram, vcmod.Fan, vcmod.Variance, vcmod.Neural, vcmod.Veed, vcmod.SaltPepper

http://www.avisynth.nl/users/vcmohan/vcmod/vcmod.html


vcmove
~~~~~~

vcmove plugin for VapourSynth.

Filters: vcmove.Rotate, vcmove.DeBarrel, vcmove.Quad2Rect, vcmove.Rect2Quad

http://www.avisynth.nl/users/vcmohan/vcmove/vcmove.html


W3FDIF
~~~~~~

Weston 3 Field Deinterlacing Filter. Ported from FFmpeg's libavfilter.

Filters: w3fdif.W3FDIF

https://github.com/HomeOfVapourSynthEvolution/VapourSynth-W3FDIF/releases


Yadifmod
~~~~~~~~

Modified version of Fizick's avisynth filter port of yadif from mplayer. This version doesn't internally generate spatial predictions, but takes them from an external clip.

Filters: yadifmod.Yadifmod

http://github.com/HomeOfVapourSynthEvolution/VapourSynth-Yadifmod


znedi3
~~~~~~

znedi3 is a CPU-optimized version of nnedi.

Filters: znedi3.nnedi3

https://github.com/sekrit-twc/znedi3


VapourSynth Script
------------------
adjust
~~~~~~

very basic port of the built-in Avisynth filter Tweak.

Filters: adjust.Tweak

http://github.com/dubhater/vapoursynth-adjust


Dither
~~~~~~

VapourSynth port of DitherTools

Filters: Dither.sigmoid_direct, Dither.sigmoid_inverse, Dither.linear_to_gamma, Dither.gamma_to_linear, Dither.clamp16, Dither.sbr16, Dither.Resize16nr, Dither.get_msb, Dither.get_lsb

https://github.com/IFeelBloated/VaporSynth-Functions


finesharp
~~~~~~~~~

Port of Didie's FineSharp script to VapourSynth.

Filters: finesharp.sharpen

http://forum.doom9.org/showthread.php?p=1777860#post1777860


fvsfunc
~~~~~~~

Small collection of VapourSynth functions

Filters: fvsfunc.GradFun3mod, fvsfunc.DescaleM, fvsfunc.Downscale444, fvsfunc.JIVTC, fvsfunc.OverlayInter, fvsfunc.AutoDeblock, fvsfunc.ReplaceFrames, fvsfunc.maa, fvsfunc.TemporalDegrain, fvsfunc.DescaleAA, fvsfunc.InsertSign

https://github.com/Irrational-Encoding-Wizardry/fvsfunc


G41Fun
~~~~~~

The replaced script for hnwvsfunc with re-written functions.

Filters: G41Fun.mClean, G41Fun.NonlinUSM, G41Fun.DetailSharpen, G41Fun.LUSM, G41Fun.JohnFPS, G41Fun.TemporalDegrain2, G41Fun.MCDegrainSharp, G41Fun.FineSharp, G41Fun.psharpen, G41Fun.QTGMC, G41Fun.SMDegrain, G41Fun.daamod, G41Fun.STPressoHD, G41Fun.MLDegrain, G41Fun.Hysteria, G41Fun.SuperToon, G41Fun.EdgeDetect, G41Fun.SpotLess, G41Fun.HQDeringmod, G41Fun.LSFmod, G41Fun.SeeSaw, G41Fun.MaskedDHA

https://github.com/Helenerineium/hnwvsfunc


havsfunc
~~~~~~~~

Various popular AviSynth scripts ported To VapourSynth.

Filters: havsfunc.aaf, havsfunc.AverageFrames, havsfunc.Bob, havsfunc.ChangeFPS, havsfunc.Clamp, havsfunc.ContraSharpening, havsfunc.daa, havsfunc.Deblock_QED, havsfunc.DeHalo_alpha, havsfunc.DitherLumaRebuild, havsfunc.EdgeCleaner, havsfunc.FastLineDarkenMOD, havsfunc.FineDehalo, havsfunc.FixChromaBleedingMod, havsfunc.GrainFactory3, havsfunc.GrainStabilizeMC, havsfunc.HQDeringmod, havsfunc.InterFrame, havsfunc.ivtc_txt60mc, havsfunc.KNLMeansCL, havsfunc.logoNR, havsfunc.LSFmod, havsfunc.LUTDeCrawl, havsfunc.LUTDeRainbow, havsfunc.MCTemporalDenoise, havsfunc.MinBlur, havsfunc.mt_deflate_multi, havsfunc.mt_expand_multi, havsfunc.mt_inflate_multi, havsfunc.mt_inpand_multi, havsfunc.Overlay, havsfunc.Padding, havsfunc.QTGMC, havsfunc.Resize, havsfunc.santiag, havsfunc.sbr, havsfunc.SCDetect, havsfunc.SigmoidDirect, havsfunc.SigmoidInverse, havsfunc.smartfademod, havsfunc.SMDegrain, havsfunc.SmoothLevels, havsfunc.srestore, havsfunc.Stab, havsfunc.STPresso, havsfunc.TemporalDegrain, havsfunc.Toon, havsfunc.Vinverse, havsfunc.Vinverse2, havsfunc.Weave, havsfunc.YAHR

http://github.com/HomeOfVapourSynthEvolution/havsfunc


mcdegrainsharp
~~~~~~~~~~~~~~

TemporalMedian is a temporal denoising filter. It replaces every pixel with the median of its temporal neighbourhood.

Filters: mcdegrainsharp.mcdegrainsharp

https://gist.github.com/4re/b5399b1801072458fc80#file-mcdegrainsharp-py


muvsfunc
~~~~~~~~

Muonium's VapourSynth functions.

Filters: muvsfunc.LDMerge, muvsfunc.Compare, muvsfunc.ExInpand, muvsfunc.InDeflate, muvsfunc.MultiRemoveGrain, muvsfunc.GradFun3, muvsfunc.AnimeMask, muvsfunc.PolygonExInpand, muvsfunc.Luma, muvsfunc.ediaa, muvsfunc.nnedi3aa, muvsfunc.maa, muvsfunc.SharpAAMcmod, muvsfunc.TEdge, muvsfunc.Sort, muvsfunc.Soothe_mod, muvsfunc.TemporalSoften, muvsfunc.FixTelecinedFades, muvsfunc.TCannyHelper, muvsfunc.MergeChroma, muvsfunc.firniture, muvsfunc.BoxFilter, muvsfunc.SmoothGrad, muvsfunc.DeFilter, muvsfunc.scale, muvsfunc.ColorBarsHD, muvsfunc.SeeSaw, muvsfunc.abcxyz, muvsfunc.Sharpen, muvsfunc.Blur, muvsfunc.BlindDeHalo3, muvsfunc.dfttestMC, muvsfunc.TurnLeft, muvsfunc.TurnRight, muvsfunc.BalanceBorders, muvsfunc.DisplayHistogram, muvsfunc.GuidedFilter, muvsfunc.GMSD, muvsfunc.SSIM, muvsfunc.SSIM_downsample, muvsfunc.LocalStatisticsMatching, muvsfunc.LocalStatistics, muvsfunc.TextSub16, muvsfunc.TMinBlur, muvsfunc.mdering, muvsfunc.BMAFilter, muvsfunc.LLSURE, muvsfunc.YAHRmod, muvsfunc.RandomInterleave

https://github.com/WolframRhodium/muvsfunc


mvmulti
~~~~~~~

MVTools is a set of filters for motion estimation and compensation.

Filters: mvmulti.StoreVect, mvmulti.Analyse, mvmulti.Recalculate, mvmulti.Compensate, mvmulti.Restore, mvmulti.Flow, mvmulti.DegrainN

http://github.com/dubhater/vapoursynth-mvtools


mvsfunc
~~~~~~~

mawen1250's VapourSynth functions.

Filters: mvsfunc.Depth, mvsfunc.ToRGB, mvsfunc.ToYUV, mvsfunc.BM3D, mvsfunc.VFRSplice, mvsfunc.PlaneStatistics, mvsfunc.PlaneCompare, mvsfunc.ShowAverage, mvsfunc.FilterIf, mvsfunc.FilterCombed, mvsfunc.Min, mvsfunc.Max, mvsfunc.Avg, mvsfunc.MinFilter, mvsfunc.MaxFilter, mvsfunc.LimitFilter, mvsfunc.PointPower, mvsfunc.CheckMatrix, mvsfunc.postfix2infix, mvsfunc.SetColorSpace, mvsfunc.AssumeFrame, mvsfunc.AssumeTFF, mvsfunc.AssumeBFF, mvsfunc.AssumeField, mvsfunc.AssumeCombed, mvsfunc.CheckVersion, mvsfunc.GetMatrix, mvsfunc.zDepth, mvsfunc.GetPlane, mvsfunc.PlaneAverage, mvsfunc.Preview, mvsfunc.GrayScale

http://github.com/HomeOfVapourSynthEvolution/mvsfunc


nnedi3_rpow2
~~~~~~~~~~~~

nnedi3_rpow2 ported from Avisynth for VapourSynth

Filters: nnedi3_rpow2

https://github.com/Irrational-Encoding-Wizardry/fvsfunc


Oyster
~~~~~~

Oyster is an experimental implement of the Blocking Matching concept, designed specifically for compression artifacts removal.

Filters: Oyster.Basic, Oyster.Deringing, Oyster.Destaircase, Oyster.Deblocking, Oyster.Super

https://github.com/IFeelBloated/Oyster


Plum
~~~~

Plum is a sharpening/blind deconvolution suite with certain advanced features like Non-Local error, Block Matching, etc..

Filters: Plum.Super, Plum.Basic, Plum.Final

https://github.com/IFeelBloated/Plum


psharpen
~~~~~~~~

VapourSynth port of pSharpen

Filters: psharpen.psharpen




resamplehq
~~~~~~~~~~

TemporalMedian is a temporal denoising filter. It replaces every pixel with the median of its temporal neighbourhood.

Filters: resamplehq.resamplehq

https://gist.github.com/4re/b5399b1801072458fc80#file-mcdegrainsharp-py


taa
~~~

A ported AA-script from Avisynth.

Filters: taa.TAAmbk, taa.vsTAAmbk

https://github.com/HomeOfVapourSynthEvolution/vsTAAmbk


Vine
~~~~

Plum is a sharpening/blind deconvolution suite with certain advanced features like Non-Local error, Block Matching, etc..

Filters: Vine.Super, Vine.Basic, Vine.Final, Vine.Dilation, Vine.Erosion, Vine.Closing, Vine.Opening, Vine.Gradient, Vine.TopHat, Vine.Blackhat

https://github.com/IFeelBloated/Plum


