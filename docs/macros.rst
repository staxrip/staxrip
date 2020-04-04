Macros
======

``$browse_file$``

Filepath returned from a file browser.

``$enter_text$``

Text entered in a input box.

``$enter_text:prompt$``

Text entered in a input box.

``$select:param1;param2;param...$``

String selected from dropdown, to show a optional message the first parameter has to start with msg: and to give the items optional captions use caption|value.

Example: $select:msg:hello;cap1|val1;cap2|val2$

``%app:name%``

Returns the path of a aplication.

``%app_dir:name%``

Returns the directory of a aplication.

``%audio_bitrate%``

Overall audio bitrate.

``%audio_file1%``

File path of the first audio file.

``%audio_file2%``

File path of the second audio file.

``%compressibility%``

Compressibility value.

``%crop_bottom%``

Bottom crop value.

``%crop_height%``

Crop height.

``%crop_left%``

Left crop value.

``%crop_right%``

Right crop value.

``%crop_top%``

Top crop value.

``%crop_width%``

Crop width.

``%delay%``

Audio delay of the first audio track.

``%delay2%``

Audio delay of the second audio track.

``%dpi%``

DPI value of the main dialog.

``%encoder_ext%``

File extension of the format the encoder of the active project outputs.

``%encoder_out_file%``

Output file of the video encoder.

``%eval:expression%``

Evaluates a math expression which may contain default macros.

``%eval_ps:expression%``

Evaluates a PowerShell expression which may contain default macros.

``%filter:name%``

Returns the script code of a filter of the active project that matches the specified name.

``%media_info_audio:property%``

Returns a MediaInfo audio property for the video source file.

``%media_info_video:property%``

Returns a MediaInfo video property for the source file.

``%muxer_ext%``

Output extension of the active muxer.

``%player%``

Path of the media player.

``%plugin_dir%``

AviSynth/VapourSynth plugin directory.

``%pos_frame%``

Current preview position in frames.

``%pos_ms%``

Current preview position in milliseconds.

``%processing%``

Returns 'True' if a job is currently processing otherwise 'False'.

``%programs_dir%``

Programs system directory.

``%script_dir%``

Users PowerShell scripts directory.

``%script_ext%``

File extension of the AviSynth/VapourSynth script so either avs or vpy.

``%script_file%``

Path of the AviSynth/VapourSynth script.

``%sel_end%``

End position of the first selecion in the preview.

``%sel_start%``

Start position of the first selecion in the preview.

``%settings_dir%``

Path of the settings direcory.

``%source_dar%``

Source display aspect ratio.

``%source_dir%``

Directory of the source file.

``%source_dir_name%``

Name of the source file directory.

``%source_dir_parent%``

Parent directory of the source file directory.

``%source_ext%``

File extension of the source file.

``%source_file%``

File path of the source video.

``%source_files%``

Source files in quotes separated by a blank.

``%source_files_comma%``

Source files in quotes separated by comma.

``%source_framerate%``

Frame rate returned by the source filter AviSynth section.

``%source_frames%``

Length in frames of the source video.

``%source_height%``

Image height of the source video.

``%source_name%``

The name of the source file without file extension.

``%source_par_x%``

Source pixel/sample aspect ratio.

``%source_par_y%``

Source pixel/sample aspect ratio.

``%source_seconds%``

Length in seconds of the source video.

``%source_temp_file%``

File located in the temp directory using the same name as the source file.

``%source_width%``

Image width of the source video.

``%startup_dir%``

Directory of the application.

``%system_dir%``

System directory.

``%target_dar%``

Target display aspect ratio.

``%target_dir%``

Directory of the target file.

``%target_file%``

File path of the target file.

``%target_framerate%``

Frame rate of the target video.

``%target_frames%``

Length in frames of the target video.

``%target_height%``

Image height of the target video.

``%target_name%``

Name of the target file without file extension.

``%target_par_x%``

Target pixel/sample aspect ratio.

``%target_par_y%``

Target pixel/sample aspect ratio.

``%target_seconds%``

Length in seconds of the target video.

``%target_size%``

Size of the target video in kilo bytes.

``%target_temp_file%``

File located in the temp directory using the same name as the target file.

``%target_width%``

Image width of the target video.

``%temp_dir%``

Directory of the source file or the temp directory if enabled.

``%temp_file%``

File located in the temp directory using the same name as the source file.

``%template_name%``

Name of the template the active project is based on.

``%text_editor%``

Path of the application currently associated with TXT files.

``%version%``

StaxRip version.

``%video_bitrate%``

Video bitrate in Kbps

``%video_encoder%``

Depending on which video encoder is active returns x264, x265, nvenc, qsvenc, vceenc, aomenc, ffmpeg or xvid_encraw.

``%working_dir%``

Directory of the source file or the temp directory if enabled.

``%app:AddGrainC%``

File path to AddGrainC

``%app:adjust%``

File path to adjust

``%app:AnimeIVTC%``

File path to AnimeIVTC

``%app:AutoAdjust%``

File path to AutoAdjust

``%app:Average%``

File path to Average

``%app:AviSynth%``

File path to AviSynth

``%app:AviSynthShader AVSI%``

File path to AviSynthShader AVSI

``%app:AviSynthShader DLL%``

File path to AviSynthShader DLL

``%app:avs2pipemod%``

File path to avs2pipemod

``%app:AVSMeter%``

File path to AVSMeter

``%app:AvsResize%``

File path to AvsResize

``%app:AVSTP%``

File path to AVSTP

``%app:AWarpSharp2%``

File path to AWarpSharp2

``%app:BDSup2Sub++%``

File path to BDSup2Sub++

``%app:BM3D%``

File path to BM3D

``%app:Bwdif%``

File path to Bwdif

``%app:chapterEditor%``

File path to chapterEditor

``%app:checkmate%``

File path to checkmate

``%app:CNR2%``

File path to CNR2

``%app:CNR2%``

File path to CNR2

``%app:CropResize%``

File path to CropResize

``%app:CTMF%``

File path to CTMF

``%app:d2vsource%``

File path to d2vsource

``%app:DAA3Mod%``

File path to DAA3Mod

``%app:DCTFilter%``

File path to DCTFilter

``%app:DCTFilter%``

File path to DCTFilter

``%app:DCTFilter-f%``

File path to DCTFilter-f

``%app:Deblock%``

File path to Deblock

``%app:Deblock%``

File path to Deblock

``%app:Deblock_QED%``

File path to Deblock_QED

``%app:DeblockPP7%``

File path to DeblockPP7

``%app:Decomb%``

File path to Decomb

``%app:DeGrainMedian%``

File path to DeGrainMedian

``%app:DegrainMedian%``

File path to DegrainMedian

``%app:DehaloAlpha%``

File path to DehaloAlpha

``%app:DeNoise Histogram%``

File path to DeNoise Histogram

``%app:DeNoiseMD%``

File path to DeNoiseMD

``%app:DeNoiseMF%``

File path to DeNoiseMF

``%app:DePan%``

File path to DePan

``%app:DePanEstimate%``

File path to DePanEstimate

``%app:DFTTest%``

File path to DFTTest

``%app:DFTTest%``

File path to DFTTest

``%app:DGDecodeNV%``

File path to DGDecodeNV

``%app:DGHDRtoSDR%``

File path to DGHDRtoSDR

``%app:DGIndex%``

File path to DGIndex

``%app:DGIndexNV%``

File path to DGIndexNV

``%app:DGTonemap%``

File path to DGTonemap

``%app:DirectX 9%``

File path to DirectX 9

``%app:Dither AVSI%``

File path to Dither AVSI

``%app:Dither DLL%``

File path to Dither DLL

``%app:Dither%``

File path to Dither

``%app:dsmux%``

File path to dsmux

``%app:DSS2mod%``

File path to DSS2mod

``%app:eac3to%``

File path to eac3to

``%app:edi_rpow2 AVSI%``

File path to edi_rpow2 AVSI

``%app:EEDI2%``

File path to EEDI2

``%app:EEDI2%``

File path to EEDI2

``%app:EEDI3%``

File path to EEDI3

``%app:eedi3_resize%``

File path to eedi3_resize

``%app:EEDI3m%``

File path to EEDI3m

``%app:fdkaac%``

File path to fdkaac

``%app:ffmpeg%``

File path to ffmpeg

``%app:ffms2%``

File path to ffms2

``%app:FFT3DFilter%``

File path to FFT3DFilter

``%app:FFT3DFilter%``

File path to FFT3DFilter

``%app:FFT3DGPU%``

File path to FFT3DGPU

``%app:FFTW%``

File path to FFTW

``%app:FineDehalo%``

File path to FineDehalo

``%app:FineSharp%``

File path to FineSharp

``%app:finesharp%``

File path to finesharp

``%app:FixTelecinedFades%``

File path to FixTelecinedFades

``%app:flash3kyuu_deband%``

File path to flash3kyuu_deband

``%app:FluxSmooth%``

File path to FluxSmooth

``%app:FluxSmooth%``

File path to FluxSmooth

``%app:fmtconv%``

File path to fmtconv

``%app:FrameRateConverter AVSI%``

File path to FrameRateConverter AVSI

``%app:FrameRateConverter DLL%``

File path to FrameRateConverter DLL

``%app:fvsfunc%``

File path to fvsfunc

``%app:G41Fun%``

File path to G41Fun

``%app:Get-MediaInfo%``

File path to Get-MediaInfo

``%app:GradFun2DB%``

File path to GradFun2DB

``%app:GradFun2DBmod%``

File path to GradFun2DBmod

``%app:Haali Splitter%``

File path to Haali Splitter

``%app:havsfunc%``

File path to havsfunc

``%app:HQDeringmod%``

File path to HQDeringmod

``%app:HQDN3D%``

File path to HQDN3D

``%app:HQDN3D%``

File path to HQDN3D

``%app:InterFrame%``

File path to InterFrame

``%app:IT%``

File path to IT

``%app:JincResize%``

File path to JincResize

``%app:JPSDR%``

File path to JPSDR

``%app:KNLMeansCL%``

File path to KNLMeansCL

``%app:Lazy Utilities%``

File path to Lazy Utilities

``%app:LSFmod%``

File path to LSFmod

``%app:L-SMASH-Works%``

File path to L-SMASH-Works

``%app:MAA2Mod%``

File path to MAA2Mod

``%app:masktools2%``

File path to masktools2

``%app:mcdegrainsharp%``

File path to mcdegrainsharp

``%app:mClean%``

File path to mClean

``%app:MCTemporalDenoise%``

File path to MCTemporalDenoise

``%app:MediaInfo%``

File path to MediaInfo

``%app:MedianBlur2%``

File path to MedianBlur2

``%app:MiniDeen%``

File path to MiniDeen

``%app:MipSmooth%``

File path to MipSmooth

``%app:mkvextract%``

File path to mkvextract

``%app:mkvinfo%``

File path to mkvinfo

``%app:mkvmerge%``

File path to mkvmerge

``%app:modPlus%``

File path to modPlus

``%app:MP4Box%``

File path to MP4Box

``%app:MPC-BE%``

File path to MPC-BE

``%app:MPC-HC%``

File path to MPC-HC

``%app:MPEG2DecPlus%``

File path to MPEG2DecPlus

``%app:mpv.net%``

File path to mpv.net

``%app:MSharpen%``

File path to MSharpen

``%app:msmoosh%``

File path to msmoosh

``%app:MT Expand Multi%``

File path to MT Expand Multi

``%app:mtn%``

File path to mtn

``%app:MultiSharpen%``

File path to MultiSharpen

``%app:muvsfunc%``

File path to muvsfunc

``%app:mvmulti%``

File path to mvmulti

``%app:mvsfunc%``

File path to mvsfunc

``%app:mvtools%``

File path to mvtools

``%app:mvtools2%``

File path to mvtools2

``%app:mvtools-sf%``

File path to mvtools-sf

``%app:NicAudio%``

File path to NicAudio

``%app:nnedi3 AVSI%``

File path to nnedi3 AVSI

``%app:nnedi3%``

File path to nnedi3

``%app:nnedi3_rpow2%``

File path to nnedi3_rpow2

``%app:nnedi3cl%``

File path to nnedi3cl

``%app:nnedi3x AVSI%``

File path to nnedi3x AVSI

``%app:NVEnc%``

File path to NVEnc

``%app:Oyster%``

File path to Oyster

``%app:Plum%``

File path to Plum

``%app:PNGopt%``

File path to PNGopt

``%app:pSharpen%``

File path to pSharpen

``%app:psharpen%``

File path to psharpen

``%app:Python%``

File path to Python

``%app:qaac%``

File path to qaac

``%app:QSVEnc%``

File path to QSVEnc

``%app:QTGMC%``

File path to QTGMC

``%app:rav1e%``

File path to rav1e

``%app:resamplehq%``

File path to resamplehq

``%app:ResizeX%``

File path to ResizeX

``%app:RgTools%``

File path to RgTools

``%app:Sangnom%``

File path to Sangnom

``%app:SangNom2%``

File path to SangNom2

``%app:scenechange%``

File path to scenechange

``%app:SMDegrain%``

File path to SMDegrain

``%app:SmoothAdjust%``

File path to SmoothAdjust

``%app:SmoothD2%``

File path to SmoothD2

``%app:SmoothD2c%``

File path to SmoothD2c

``%app:Subtitle Edit%``

File path to Subtitle Edit

``%app:SVPFlow 1%``

File path to SVPFlow 1

``%app:SVPFlow 1%``

File path to SVPFlow 1

``%app:SVPFlow 2%``

File path to SVPFlow 2

``%app:SVPFlow 2%``

File path to SVPFlow 2

``%app:SVT-AV1%``

File path to SVT-AV1

``%app:taa%``

File path to taa

``%app:TCanny%``

File path to TCanny

``%app:TDeint%``

File path to TDeint

``%app:TDeintMod%``

File path to TDeintMod

``%app:TEMmod%``

File path to TEMmod

``%app:TemporalMedian%``

File path to TemporalMedian

``%app:temporalsoften%``

File path to temporalsoften

``%app:TimeCube%``

File path to TimeCube

``%app:TIVTC%``

File path to TIVTC

``%app:TMM2%``

File path to TMM2

``%app:TNLMeans%``

File path to TNLMeans

``%app:TTempSmooth%``

File path to TTempSmooth

``%app:UnDot%``

File path to UnDot

``%app:VagueDenoiser%``

File path to VagueDenoiser

``%app:VagueDenoiser%``

File path to VagueDenoiser

``%app:VapourSource%``

File path to VapourSource

``%app:VapourSynth%``

File path to VapourSynth

``%app:VCEEnc%``

File path to VCEEnc

``%app:vcfreq%``

File path to vcfreq

``%app:vcmod%``

File path to vcmod

``%app:vcmove%``

File path to vcmove

``%app:Vine%``

File path to Vine

``%app:vinverse%``

File path to vinverse

``%app:Visual C++ 2012%``

File path to Visual C++ 2012

``%app:Visual C++ 2013%``

File path to Visual C++ 2013

``%app:Visual C++ 2019%``

File path to Visual C++ 2019

``%app:vsCube%``

File path to vsCube

``%app:VSFilterMod%``

File path to VSFilterMod

``%app:vspipe%``

File path to vspipe

``%app:VSRip%``

File path to VSRip

``%app:W3FDIF%``

File path to W3FDIF

``%app:x264%``

File path to x264

``%app:x265%``

File path to x265

``%app:xNLMeans%``

File path to xNLMeans

``%app:xvid_encraw%``

File path to xvid_encraw

``%app:Yadifmod%``

File path to Yadifmod

``%app:yadifmod2%``

File path to yadifmod2

``%app:YFRC%``

File path to YFRC

``%app:znedi3%``

File path to znedi3

``%app_dir:AddGrainC%``

Folder path to AddGrainC

``%app_dir:adjust%``

Folder path to adjust

``%app_dir:AnimeIVTC%``

Folder path to AnimeIVTC

``%app_dir:AutoAdjust%``

Folder path to AutoAdjust

``%app_dir:Average%``

Folder path to Average

``%app_dir:AviSynth%``

Folder path to AviSynth

``%app_dir:AviSynthShader AVSI%``

Folder path to AviSynthShader AVSI

``%app_dir:AviSynthShader DLL%``

Folder path to AviSynthShader DLL

``%app_dir:avs2pipemod%``

Folder path to avs2pipemod

``%app_dir:AVSMeter%``

Folder path to AVSMeter

``%app_dir:AvsResize%``

Folder path to AvsResize

``%app_dir:AVSTP%``

Folder path to AVSTP

``%app_dir:AWarpSharp2%``

Folder path to AWarpSharp2

``%app_dir:BDSup2Sub++%``

Folder path to BDSup2Sub++

``%app_dir:BM3D%``

Folder path to BM3D

``%app_dir:Bwdif%``

Folder path to Bwdif

``%app_dir:chapterEditor%``

Folder path to chapterEditor

``%app_dir:checkmate%``

Folder path to checkmate

``%app_dir:CNR2%``

Folder path to CNR2

``%app_dir:CNR2%``

Folder path to CNR2

``%app_dir:CropResize%``

Folder path to CropResize

``%app_dir:CTMF%``

Folder path to CTMF

``%app_dir:d2vsource%``

Folder path to d2vsource

``%app_dir:DAA3Mod%``

Folder path to DAA3Mod

``%app_dir:DCTFilter%``

Folder path to DCTFilter

``%app_dir:DCTFilter%``

Folder path to DCTFilter

``%app_dir:DCTFilter-f%``

Folder path to DCTFilter-f

``%app_dir:Deblock%``

Folder path to Deblock

``%app_dir:Deblock%``

Folder path to Deblock

``%app_dir:Deblock_QED%``

Folder path to Deblock_QED

``%app_dir:DeblockPP7%``

Folder path to DeblockPP7

``%app_dir:Decomb%``

Folder path to Decomb

``%app_dir:DeGrainMedian%``

Folder path to DeGrainMedian

``%app_dir:DegrainMedian%``

Folder path to DegrainMedian

``%app_dir:DehaloAlpha%``

Folder path to DehaloAlpha

``%app_dir:DeNoise Histogram%``

Folder path to DeNoise Histogram

``%app_dir:DeNoiseMD%``

Folder path to DeNoiseMD

``%app_dir:DeNoiseMF%``

Folder path to DeNoiseMF

``%app_dir:DePan%``

Folder path to DePan

``%app_dir:DePanEstimate%``

Folder path to DePanEstimate

``%app_dir:DFTTest%``

Folder path to DFTTest

``%app_dir:DFTTest%``

Folder path to DFTTest

``%app_dir:DGDecodeNV%``

Folder path to DGDecodeNV

``%app_dir:DGHDRtoSDR%``

Folder path to DGHDRtoSDR

``%app_dir:DGIndex%``

Folder path to DGIndex

``%app_dir:DGIndexNV%``

Folder path to DGIndexNV

``%app_dir:DGTonemap%``

Folder path to DGTonemap

``%app_dir:DirectX 9%``

Folder path to DirectX 9

``%app_dir:Dither AVSI%``

Folder path to Dither AVSI

``%app_dir:Dither DLL%``

Folder path to Dither DLL

``%app_dir:Dither%``

Folder path to Dither

``%app_dir:dsmux%``

Folder path to dsmux

``%app_dir:DSS2mod%``

Folder path to DSS2mod

``%app_dir:eac3to%``

Folder path to eac3to

``%app_dir:edi_rpow2 AVSI%``

Folder path to edi_rpow2 AVSI

``%app_dir:EEDI2%``

Folder path to EEDI2

``%app_dir:EEDI2%``

Folder path to EEDI2

``%app_dir:EEDI3%``

Folder path to EEDI3

``%app_dir:eedi3_resize%``

Folder path to eedi3_resize

``%app_dir:EEDI3m%``

Folder path to EEDI3m

``%app_dir:fdkaac%``

Folder path to fdkaac

``%app_dir:ffmpeg%``

Folder path to ffmpeg

``%app_dir:ffms2%``

Folder path to ffms2

``%app_dir:FFT3DFilter%``

Folder path to FFT3DFilter

``%app_dir:FFT3DFilter%``

Folder path to FFT3DFilter

``%app_dir:FFT3DGPU%``

Folder path to FFT3DGPU

``%app_dir:FFTW%``

Folder path to FFTW

``%app_dir:FineDehalo%``

Folder path to FineDehalo

``%app_dir:FineSharp%``

Folder path to FineSharp

``%app_dir:finesharp%``

Folder path to finesharp

``%app_dir:FixTelecinedFades%``

Folder path to FixTelecinedFades

``%app_dir:flash3kyuu_deband%``

Folder path to flash3kyuu_deband

``%app_dir:FluxSmooth%``

Folder path to FluxSmooth

``%app_dir:FluxSmooth%``

Folder path to FluxSmooth

``%app_dir:fmtconv%``

Folder path to fmtconv

``%app_dir:FrameRateConverter AVSI%``

Folder path to FrameRateConverter AVSI

``%app_dir:FrameRateConverter DLL%``

Folder path to FrameRateConverter DLL

``%app_dir:fvsfunc%``

Folder path to fvsfunc

``%app_dir:G41Fun%``

Folder path to G41Fun

``%app_dir:Get-MediaInfo%``

Folder path to Get-MediaInfo

``%app_dir:GradFun2DB%``

Folder path to GradFun2DB

``%app_dir:GradFun2DBmod%``

Folder path to GradFun2DBmod

``%app_dir:Haali Splitter%``

Folder path to Haali Splitter

``%app_dir:havsfunc%``

Folder path to havsfunc

``%app_dir:HQDeringmod%``

Folder path to HQDeringmod

``%app_dir:HQDN3D%``

Folder path to HQDN3D

``%app_dir:HQDN3D%``

Folder path to HQDN3D

``%app_dir:InterFrame%``

Folder path to InterFrame

``%app_dir:IT%``

Folder path to IT

``%app_dir:JincResize%``

Folder path to JincResize

``%app_dir:JPSDR%``

Folder path to JPSDR

``%app_dir:KNLMeansCL%``

Folder path to KNLMeansCL

``%app_dir:Lazy Utilities%``

Folder path to Lazy Utilities

``%app_dir:LSFmod%``

Folder path to LSFmod

``%app_dir:L-SMASH-Works%``

Folder path to L-SMASH-Works

``%app_dir:MAA2Mod%``

Folder path to MAA2Mod

``%app_dir:masktools2%``

Folder path to masktools2

``%app_dir:mcdegrainsharp%``

Folder path to mcdegrainsharp

``%app_dir:mClean%``

Folder path to mClean

``%app_dir:MCTemporalDenoise%``

Folder path to MCTemporalDenoise

``%app_dir:MediaInfo%``

Folder path to MediaInfo

``%app_dir:MedianBlur2%``

Folder path to MedianBlur2

``%app_dir:MiniDeen%``

Folder path to MiniDeen

``%app_dir:MipSmooth%``

Folder path to MipSmooth

``%app_dir:mkvextract%``

Folder path to mkvextract

``%app_dir:mkvinfo%``

Folder path to mkvinfo

``%app_dir:mkvmerge%``

Folder path to mkvmerge

``%app_dir:modPlus%``

Folder path to modPlus

``%app_dir:MP4Box%``

Folder path to MP4Box

``%app_dir:MPC-BE%``

Folder path to MPC-BE

``%app_dir:MPC-HC%``

Folder path to MPC-HC

``%app_dir:MPEG2DecPlus%``

Folder path to MPEG2DecPlus

``%app_dir:mpv.net%``

Folder path to mpv.net

``%app_dir:MSharpen%``

Folder path to MSharpen

``%app_dir:msmoosh%``

Folder path to msmoosh

``%app_dir:MT Expand Multi%``

Folder path to MT Expand Multi

``%app_dir:mtn%``

Folder path to mtn

``%app_dir:MultiSharpen%``

Folder path to MultiSharpen

``%app_dir:muvsfunc%``

Folder path to muvsfunc

``%app_dir:mvmulti%``

Folder path to mvmulti

``%app_dir:mvsfunc%``

Folder path to mvsfunc

``%app_dir:mvtools%``

Folder path to mvtools

``%app_dir:mvtools2%``

Folder path to mvtools2

``%app_dir:mvtools-sf%``

Folder path to mvtools-sf

``%app_dir:NicAudio%``

Folder path to NicAudio

``%app_dir:nnedi3 AVSI%``

Folder path to nnedi3 AVSI

``%app_dir:nnedi3%``

Folder path to nnedi3

``%app_dir:nnedi3_rpow2%``

Folder path to nnedi3_rpow2

``%app_dir:nnedi3cl%``

Folder path to nnedi3cl

``%app_dir:nnedi3x AVSI%``

Folder path to nnedi3x AVSI

``%app_dir:NVEnc%``

Folder path to NVEnc

``%app_dir:Oyster%``

Folder path to Oyster

``%app_dir:Plum%``

Folder path to Plum

``%app_dir:PNGopt%``

Folder path to PNGopt

``%app_dir:pSharpen%``

Folder path to pSharpen

``%app_dir:psharpen%``

Folder path to psharpen

``%app_dir:Python%``

Folder path to Python

``%app_dir:qaac%``

Folder path to qaac

``%app_dir:QSVEnc%``

Folder path to QSVEnc

``%app_dir:QTGMC%``

Folder path to QTGMC

``%app_dir:rav1e%``

Folder path to rav1e

``%app_dir:resamplehq%``

Folder path to resamplehq

``%app_dir:ResizeX%``

Folder path to ResizeX

``%app_dir:RgTools%``

Folder path to RgTools

``%app_dir:Sangnom%``

Folder path to Sangnom

``%app_dir:SangNom2%``

Folder path to SangNom2

``%app_dir:scenechange%``

Folder path to scenechange

``%app_dir:SMDegrain%``

Folder path to SMDegrain

``%app_dir:SmoothAdjust%``

Folder path to SmoothAdjust

``%app_dir:SmoothD2%``

Folder path to SmoothD2

``%app_dir:SmoothD2c%``

Folder path to SmoothD2c

``%app_dir:Subtitle Edit%``

Folder path to Subtitle Edit

``%app_dir:SVPFlow 1%``

Folder path to SVPFlow 1

``%app_dir:SVPFlow 1%``

Folder path to SVPFlow 1

``%app_dir:SVPFlow 2%``

Folder path to SVPFlow 2

``%app_dir:SVPFlow 2%``

Folder path to SVPFlow 2

``%app_dir:SVT-AV1%``

Folder path to SVT-AV1

``%app_dir:taa%``

Folder path to taa

``%app_dir:TCanny%``

Folder path to TCanny

``%app_dir:TDeint%``

Folder path to TDeint

``%app_dir:TDeintMod%``

Folder path to TDeintMod

``%app_dir:TEMmod%``

Folder path to TEMmod

``%app_dir:TemporalMedian%``

Folder path to TemporalMedian

``%app_dir:temporalsoften%``

Folder path to temporalsoften

``%app_dir:TimeCube%``

Folder path to TimeCube

``%app_dir:TIVTC%``

Folder path to TIVTC

``%app_dir:TMM2%``

Folder path to TMM2

``%app_dir:TNLMeans%``

Folder path to TNLMeans

``%app_dir:TTempSmooth%``

Folder path to TTempSmooth

``%app_dir:UnDot%``

Folder path to UnDot

``%app_dir:VagueDenoiser%``

Folder path to VagueDenoiser

``%app_dir:VagueDenoiser%``

Folder path to VagueDenoiser

``%app_dir:VapourSource%``

Folder path to VapourSource

``%app_dir:VapourSynth%``

Folder path to VapourSynth

``%app_dir:VCEEnc%``

Folder path to VCEEnc

``%app_dir:vcfreq%``

Folder path to vcfreq

``%app_dir:vcmod%``

Folder path to vcmod

``%app_dir:vcmove%``

Folder path to vcmove

``%app_dir:Vine%``

Folder path to Vine

``%app_dir:vinverse%``

Folder path to vinverse

``%app_dir:Visual C++ 2012%``

Folder path to Visual C++ 2012

``%app_dir:Visual C++ 2013%``

Folder path to Visual C++ 2013

``%app_dir:Visual C++ 2019%``

Folder path to Visual C++ 2019

``%app_dir:vsCube%``

Folder path to vsCube

``%app_dir:VSFilterMod%``

Folder path to VSFilterMod

``%app_dir:vspipe%``

Folder path to vspipe

``%app_dir:VSRip%``

Folder path to VSRip

``%app_dir:W3FDIF%``

Folder path to W3FDIF

``%app_dir:x264%``

Folder path to x264

``%app_dir:x265%``

Folder path to x265

``%app_dir:xNLMeans%``

Folder path to xNLMeans

``%app_dir:xvid_encraw%``

Folder path to xvid_encraw

``%app_dir:Yadifmod%``

Folder path to Yadifmod

``%app_dir:yadifmod2%``

Folder path to yadifmod2

``%app_dir:YFRC%``

Folder path to YFRC

``%app_dir:znedi3%``

Folder path to znedi3

