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

File extension of the AviSynth/VapourSynth script so either avs or vpy in quotes.

``%script_file%``

Path of the AviSynth/VapourSynth script.

``%sel_end%``

End position of the first selecion in the preview.

``%sel_start%``

Start position of the first selecion in the preview.

``%settings_dir%``

Path of the settings direcory.

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

``%target_sar%``

Target sample aspect ratio (also known as PAR (pixel aspect ratio)).

``%target_seconds%``

Length in seconds of the target video.

``%target_size%``

Size of the target video in kilo bytes.

``%target_temp_file%``

File located in the temp directory using the same name as the target file.

``%target_width%``

Image width of the target video.

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

``%app:adjust%``

File path to adjust

``%app:aomenc%``

File path to aomenc

``%app:AutoAdjust%``

File path to AutoAdjust

``%app:AviSynth+%``

File path to AviSynth+

``%app:avs2pipemod%``

File path to avs2pipemod

``%app:AVSMeter%``

File path to AVSMeter

``%app:aWarpSharp2%``

File path to aWarpSharp2

``%app:BDSup2Sub++%``

File path to BDSup2Sub++

``%app:checkmate%``

File path to checkmate

``%app:d2vsource%``

File path to d2vsource

``%app:DCTFilter%``

File path to DCTFilter

``%app:Deblock%``

File path to Deblock

``%app:Decomb%``

File path to Decomb

``%app:DeLogo%``

File path to DeLogo

``%app:DGDecodeIM%``

File path to DGDecodeIM

``%app:DGDecodeNV%``

File path to DGDecodeNV

``%app:DGIndex%``

File path to DGIndex

``%app:DGIndexIM%``

File path to DGIndexIM

``%app:DGIndexNV%``

File path to DGIndexNV

``%app:dsmux%``

File path to dsmux

``%app:DSS2mod%``

File path to DSS2mod

``%app:eac3to%``

File path to eac3to

``%app:EEDI2%``

File path to EEDI2

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

``%app:FFTW%``

File path to FFTW

``%app:FineSharp%``

File path to FineSharp

``%app:finesharp%``

File path to finesharp

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

``%app:GradCurve%``

File path to GradCurve

``%app:Haali Splitter%``

File path to Haali Splitter

``%app:havsfunc%``

File path to havsfunc

``%app:Java%``

File path to Java

``%app:JPSDR%``

File path to JPSDR

``%app:KNLMeansCL%``

File path to KNLMeansCL

``%app:LSFmod%``

File path to LSFmod

``%app:L-SMASH-Works%``

File path to L-SMASH-Works

``%app:masktools2%``

File path to masktools2

``%app:mClean%``

File path to mClean

``%app:MediaInfo%``

File path to MediaInfo

``%app:MedianBlur2%``

File path to MedianBlur2

``%app:mkvextract%``

File path to mkvextract

``%app:mkvmerge%``

File path to mkvmerge

``%app:modPlus%``

File path to modPlus

``%app:MP4Box%``

File path to MP4Box

``%app:MPEG2DecPlus%``

File path to MPEG2DecPlus

``%app:mpv.net%``

File path to mpv.net

``%app:MSharpen%``

File path to MSharpen

``%app:msmoosh%``

File path to msmoosh

``%app:mvsfunc%``

File path to mvsfunc

``%app:mvtools%``

File path to mvtools

``%app:mvtools2%``

File path to mvtools2

``%app:NicAudio%``

File path to NicAudio

``%app:nnedi3%``

File path to nnedi3

``%app:NVEnc%``

File path to NVEnc

``%app:ProjectX%``

File path to ProjectX

``%app:Python%``

File path to Python

``%app:qaac%``

File path to qaac

``%app:QSVEnc%``

File path to QSVEnc

``%app:QTGMC%``

File path to QTGMC

``%app:RgTools%``

File path to RgTools

``%app:SangNom2%``

File path to SangNom2

``%app:scenechange%``

File path to scenechange

``%app:SMDegrain%``

File path to SMDegrain

``%app:SmoothAdjust%``

File path to SmoothAdjust

``%app:SubtitleEdit%``

File path to SubtitleEdit

``%app:TComb%``

File path to TComb

``%app:TDeint%``

File path to TDeint

``%app:temporalsoften%``

File path to temporalsoften

``%app:TIVTC%``

File path to TIVTC

``%app:UnDot%``

File path to UnDot

``%app:VapourSynth%``

File path to VapourSynth

``%app:VCEEnc%``

File path to VCEEnc

``%app:vcmod%``

File path to vcmod

``%app:vinverse%``

File path to vinverse

``%app:Visual C++ 2012%``

File path to Visual C++ 2012

``%app:Visual C++ 2013%``

File path to Visual C++ 2013

``%app:Visual C++ 2017%``

File path to Visual C++ 2017

``%app:VSFilterMod%``

File path to VSFilterMod

``%app:vslsmashsource%``

File path to vslsmashsource

``%app:vspipe%``

File path to vspipe

``%app:VSRip%``

File path to VSRip

``%app:x264%``

File path to x264

``%app:x264 10-Bit%``

File path to x264 10-Bit

``%app:x265%``

File path to x265

``%app:xvid_encraw%``

File path to xvid_encraw

``%app:Yadifmod%``

File path to Yadifmod

``%app:yadifmod2%``

File path to yadifmod2

``%app_dir:adjust%``

Folder path to adjust

``%app_dir:aomenc%``

Folder path to aomenc

``%app_dir:AutoAdjust%``

Folder path to AutoAdjust

``%app_dir:AviSynth+%``

Folder path to AviSynth+

``%app_dir:avs2pipemod%``

Folder path to avs2pipemod

``%app_dir:AVSMeter%``

Folder path to AVSMeter

``%app_dir:aWarpSharp2%``

Folder path to aWarpSharp2

``%app_dir:BDSup2Sub++%``

Folder path to BDSup2Sub++

``%app_dir:checkmate%``

Folder path to checkmate

``%app_dir:d2vsource%``

Folder path to d2vsource

``%app_dir:DCTFilter%``

Folder path to DCTFilter

``%app_dir:Deblock%``

Folder path to Deblock

``%app_dir:Decomb%``

Folder path to Decomb

``%app_dir:DeLogo%``

Folder path to DeLogo

``%app_dir:DGDecodeIM%``

Folder path to DGDecodeIM

``%app_dir:DGDecodeNV%``

Folder path to DGDecodeNV

``%app_dir:DGIndex%``

Folder path to DGIndex

``%app_dir:DGIndexIM%``

Folder path to DGIndexIM

``%app_dir:DGIndexNV%``

Folder path to DGIndexNV

``%app_dir:dsmux%``

Folder path to dsmux

``%app_dir:DSS2mod%``

Folder path to DSS2mod

``%app_dir:eac3to%``

Folder path to eac3to

``%app_dir:EEDI2%``

Folder path to EEDI2

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

``%app_dir:FFTW%``

Folder path to FFTW

``%app_dir:FineSharp%``

Folder path to FineSharp

``%app_dir:finesharp%``

Folder path to finesharp

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

``%app_dir:GradCurve%``

Folder path to GradCurve

``%app_dir:Haali Splitter%``

Folder path to Haali Splitter

``%app_dir:havsfunc%``

Folder path to havsfunc

``%app_dir:Java%``

Folder path to Java

``%app_dir:JPSDR%``

Folder path to JPSDR

``%app_dir:KNLMeansCL%``

Folder path to KNLMeansCL

``%app_dir:LSFmod%``

Folder path to LSFmod

``%app_dir:L-SMASH-Works%``

Folder path to L-SMASH-Works

``%app_dir:masktools2%``

Folder path to masktools2

``%app_dir:mClean%``

Folder path to mClean

``%app_dir:MediaInfo%``

Folder path to MediaInfo

``%app_dir:MedianBlur2%``

Folder path to MedianBlur2

``%app_dir:mkvextract%``

Folder path to mkvextract

``%app_dir:mkvmerge%``

Folder path to mkvmerge

``%app_dir:modPlus%``

Folder path to modPlus

``%app_dir:MP4Box%``

Folder path to MP4Box

``%app_dir:MPEG2DecPlus%``

Folder path to MPEG2DecPlus

``%app_dir:mpv.net%``

Folder path to mpv.net

``%app_dir:MSharpen%``

Folder path to MSharpen

``%app_dir:msmoosh%``

Folder path to msmoosh

``%app_dir:mvsfunc%``

Folder path to mvsfunc

``%app_dir:mvtools%``

Folder path to mvtools

``%app_dir:mvtools2%``

Folder path to mvtools2

``%app_dir:NicAudio%``

Folder path to NicAudio

``%app_dir:nnedi3%``

Folder path to nnedi3

``%app_dir:NVEnc%``

Folder path to NVEnc

``%app_dir:ProjectX%``

Folder path to ProjectX

``%app_dir:Python%``

Folder path to Python

``%app_dir:qaac%``

Folder path to qaac

``%app_dir:QSVEnc%``

Folder path to QSVEnc

``%app_dir:QTGMC%``

Folder path to QTGMC

``%app_dir:RgTools%``

Folder path to RgTools

``%app_dir:SangNom2%``

Folder path to SangNom2

``%app_dir:scenechange%``

Folder path to scenechange

``%app_dir:SMDegrain%``

Folder path to SMDegrain

``%app_dir:SmoothAdjust%``

Folder path to SmoothAdjust

``%app_dir:SubtitleEdit%``

Folder path to SubtitleEdit

``%app_dir:TComb%``

Folder path to TComb

``%app_dir:TDeint%``

Folder path to TDeint

``%app_dir:temporalsoften%``

Folder path to temporalsoften

``%app_dir:TIVTC%``

Folder path to TIVTC

``%app_dir:UnDot%``

Folder path to UnDot

``%app_dir:VapourSynth%``

Folder path to VapourSynth

``%app_dir:VCEEnc%``

Folder path to VCEEnc

``%app_dir:vcmod%``

Folder path to vcmod

``%app_dir:vinverse%``

Folder path to vinverse

``%app_dir:Visual C++ 2012%``

Folder path to Visual C++ 2012

``%app_dir:Visual C++ 2013%``

Folder path to Visual C++ 2013

``%app_dir:Visual C++ 2017%``

Folder path to Visual C++ 2017

``%app_dir:VSFilterMod%``

Folder path to VSFilterMod

``%app_dir:vslsmashsource%``

Folder path to vslsmashsource

``%app_dir:vspipe%``

Folder path to vspipe

``%app_dir:VSRip%``

Folder path to VSRip

``%app_dir:x264%``

Folder path to x264

``%app_dir:x264 10-Bit%``

Folder path to x264 10-Bit

``%app_dir:x265%``

Folder path to x265

``%app_dir:xvid_encraw%``

Folder path to xvid_encraw

``%app_dir:Yadifmod%``

Folder path to Yadifmod

``%app_dir:yadifmod2%``

Folder path to yadifmod2

