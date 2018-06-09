#### Requirements

.NET 4.7 is required on systems prior to Windows 10 Anniversary or users using Windows 8.1 or Windows 7 SP1.

https://www.microsoft.com/en-us/download/details.aspx?id=55170

#### Download

Earlier Test Builds For Version 1.7.x.x:

https://www.dropbox.com/sh/4ctl2y928xkak4f/AAADEZj_hFpGQaNOdd3yqcAHa?dl=0


1.8.0.0

- All Filter are 64Bit enabled.
- Scripts and Code is designed for Avisynth+. Using older versions of Avisynth will unlikely work with everything.
- Restored the Old Icon and removed the RIP Icon.
- Added Support for Max CLL, MAX FALL and Master-Display for Nvidia H.265 Encoder.
- Added Support for Max CLL, MAX FALL and Master-Display for QSVEnc(Intel) H.265 Encoder.
- XviD has been Updated to 1.3.5
- Updated x265 to version 2.8+12(8+10+12 Bit Support)
- Add Support for AVX512 for x265 
- Updated FFMPEG to 4.0+
- Updated NVENCc to 4.07
- QSVEnc(Intel) has been Updated to 3.05
- MKVToolNix has been Updated to version 23.
- Updated MP4Box to 0.7.2
- eac3to has been Updated to 3.34
- AVSMeter has been Updated to 2.7.8
- fdkaac has been compiled for 64Bit
- Mediainfo has been Updated to 18.05
- FrameServer for VirtualDub2 and VirtualDub 64Bit works with Video. *Currently a small issue with Audio not decoding properly* (Files are Included)
- MPVnet has been Replaced with MPV. It uses the extact same code as MPVnet. MPVnet was replaced due to playback issues it was creating.
- Project X has been Removed
- Java has been Removed
- AV1 has been Removed for the Time Being (Encoder is unstable).
- mClean has been Updated to 3.2.
- awarpsharp2 has been Updated to 2.0.1
- FrameRateConventer Has been Updated to 1.2.1
- JPSDR has been Updated to 2.2.0.7
- MaskTools2 Has been Updated to 2.2.14
- MVTools2 has been Updated to 2.7.31
- QTGMC has been Updated to 3.358s
- TIVTC has been Updated to 1.0.11
- Added Color, FrameRate, Line, and Restoration context Menu.
The List of all Included Filters:
- Source: 
	- Automatic
	- AviSource
	- DirectShowSource
	- FFVideoSource 
	- Manual
	- MPEG2Source
	- DGSource
	- DGSourceIM*
	- DGSourceNV*
	- DSS2
	* = Not Freeware.
- Resizers: 
	- Dither_Resize16 
	- ResizeMT
	- Resize
	- Hardware Resize
	- JincResize
	- SuperResXBR
	- SuperRes
	- SuperXBR
	- Resize(Z) - Uses ZLib
- Crop: 
	- Dither_Crop16
	- Crop
	- Hardware Crop
- Line:
	- MCDAA3
	- DAA3Mod
	- MAA2Mod
	- XAA
	- HDRSharp
	- pSharpen
	- MSharpen
	- aWarpSharp2
	- FineSharp 
	- LSFmod (With More Syntax added for better results then Default)
- Misc:
	- MTMode (Multithreading)
	- SplitVertical (For MT) 
- Restoration: 
	- CNR2
	- Deblock	(Includes Dll for Both old and New versions, New version is enabled by Default).
	- Deblock_QED 
	- DehaloAlpha
	- FineDehalo
	- HQDeringmod
	- MipSmooth
	- SmoothD2 
- Color:
	- AutoAdjust
	- Histogram
	- ColorYUV (AutoGain)
	- Levels and the Correct color levels for each BitDepth.
	- Tweak
	- Convert(Format(ColorSpace), To(DataFormat), Bits(BitDepth)
	- Dither Tools
	- DFTTest (For Stack)
	- HDRCore
	- HDRColor*	
	- HDRNoise (Function must be Entered Manually to Use)
	- HDRMatrix
	- HDR Tone Mapping
	* = Not Freeware
- Framerate:
	- AssumeFPS
	- AssumeFPS_Source
	- InterFrame
	- ConvertFPS
	- ChangeFPS
	- SVPFlow (Scales to 59.940 and uses GPU by Default)
	- YFRC
- Field:
	- QTGMC (With additional Syntax for both Interlaced and Progressive Sources. Plus Repair option for bad interlaced sources (Includes Progressive)
	- nnedi3
	- IVTC
	- Assume (AssumeTFF & AssumeBFF)
	- EEDI3
	- FieldDeinterlace
	- yadifmod2
	- Select (SelectEven & SelectOdd)
- Noise:
	- KNLMeansCL (Added Device_type to Syntax)
	- DeNoiseMD + Histogram
	- DenoiseMF + Histogram
	- DFTTest
	- mClean
	- RemoveGrain16 
	- Repair16
	- Repair
	- HDRNoise (Syntax must be entered Manually)
	- RemoveGrain
	- FFT3DGPU 
	- HQDN3D
	- SpatialSoften (YUY2 Only)
	- TemporalSoften 
	- MCTemporalDenoise (Script has been Updated to work properly with GradFun2DB & GradFun2DBMod).
	- MCTemporalDenoisePP (Same as Above)
	- SMDGrain
	- DFTTest
	- TNLMeans 
	- VagueDenoiser 
	- xNLMeans)
Support Filters & Scripts:
	- AddGrainC
	- DeGrainMedian
	- GradFun2DB
	- GradFun2DBMod
	- HDRCore Source
	- AVSTP
	- TEMmod
	- MT Expand Multi
	- SmoothD2c
	- edi_rpow2
	- Average
	- nnedi3 rpow2
	- LUtils
	- TMM2
	- SVPFlow 1
	- SVPFlow 2
	- TTempSmooth
	- Depan
	- DepanEstimate
	- TemporalDegrain
	- AnimeIVTC
	- f3kdb
	- TComb
	- TDent
Altered:
	Tweak: Syntax has been alter for Adjusting saturation and includes both realcalc and dither_strength(Only Avisynth+ use these Functions).
	- RemoveGrain: added a secondary option for for artifact removal.	
	- KNLMeansCL: Added additional syntax for Device_type which is set to Auto. It will try to use GPU first before trying to use the CPU. 
 	- Levels with correct range for each bitdepth. Level function does not autoscale based on the bitdepth.
	- added more modern Framerates to Assumefps, ConvertFPS and ChangeFPS. 144 & 240 rates have been added.
- Removed from Filter context Menu but can still works with the auto loading feature(Syntax must be Entered Manually): 
	- Clense (Part of JPSDR)
	- Checkmate
	- SangNom2
	- TComb
	- Undot
	- MedianBlurTemporal
	- FluxSmoothST
	- FluxSmoothT
	- MedianBlur
	- SangNom2
	- f3kdb
	- Vinverse 2

Filters and Software that Support AVX2: aWarpSharp2, DCTFilter, DFTTest, JPSDR, Masktools2, TMM2, TNLMeans, yadifmod2, x265
	- By default AVX or below are activate for compaiblity. 
- VapourSynth is untouched(No Changes).

1.7.0.6 unstable test build

- new: macro for the source file extension added (%source_ext%)

- fix: target image size calculation was using mod 16 instead of the value defined in the options (https://forum.doom9.org/showthread.php?p=1832611#post1832611)
- fix: x265 command line import and meta data import of --max-cll didn't work

- update: mClean 2.3
- update: mvtools2 2.7.24
- update: VapourSynth 43

#### 1.7.0.5 unstable test build

- fix: flash3kyuu_deband not working with AviSynth

- update: mkvtoolnix 20.0.0
- update: AviSynth 2580
- update: mClean 2.0
- update: TIVTC 1.0.10
- update: x265 2.6+31
- update: eac3to 3.34
- update: NVEnc 3.27

#### 1.7.0.4 unstable test build

- new: VapourSynth plugin vcmod added
- new: H265/HEVC and AAC support to eac3to demuxer added
- new: the main window don't get activated after processing in case the active window is borderless which is the case when a player is in fullscreen mode
- new: option added to the options dialog on the video tab to import VUI metadata like HDR from the source file to the video encoder, this is applied when the source loads like before and since this build additionally everytime a video encoder profile is loaded

- fix: for the rare case of the settings failing to load staxrip will offer the options Retry, Reset, Exit

- change: implemented mkvtoolnix command line interface changes
- change: the upscaling assistant message was removed and the aspect ratio error message was improved
- change: update changed nvenc --transfer flags

- update: eac3to 3.32
- update: mkvtoolnix 17.0.0
- update: ffmpeg 3.3.4

#### 1.7.0.3 unstable test build

- new: the custom menu editor shows the name of the icon

#### 1.7.0.2 unstable test build

- new: ffmpeg re-mux TS to MKV includes all audio tracks (Preprocessing settings need to be reset manually)
- new: improved docs at http://staxrip.readthedocs.io, since the main menu changed much unfortunately it had to be reset
- new: ffmpeg normalization modes loudnorm and dynaudnorm added
- new: added Go To Chapter feature in preview dialog, it will only appear after a manual menu reset

- change: audio option Convert/Decoder moved from project options to audio options
- change: replaced CropRel with Crop so minimum required VapourSynth version is R39

- fix: crash when using close after job processing
- fix: missing VC++ runtime for VSFilterMod wasn't detected

- update: QTGMC 3.357
- update: mpv.net 0.2.2
- update: mkvtoolnix 16.0.0 pre 01582
- update: AVSMeter 2.6.6

#### 1.7.0.1 unstable test build

- fix: crash without error message using the macros %app:<>% and %app_dir:<>% in AviSynth

- fix: crash that happened converting PGS to VobSub but might also be triggered by other features

- new: added avisynth filter MedianBlur2

- update: NVEnc 3.23

#### 1.7.0.0 stable release

- new: changed templates to: 'Manual Workflow', 'Automatic Workflow', 'No indexing and demuxing' and 'Re-mux', default startup template is 'Automatic Workflow'
- new: option added to disable the assistant reminder about the aspect ratio error
- new: AviSynth script FineSharp added
- new: x265 option --frames added

- fix: x265 option --frames added twice to command line
- fix: cosmetic DPI scaling issue in main form on 96 DPI

- update: mvsfunc 8
- update: mClean 1.8
- update: NVEnc 3.22

#### 1.6.1.9 unstable test build

- new: thumbnail generator has a new option to define the timestamp location (left, right, top, bottom) (https://github.com/stax76/staxrip/wiki/Screenshots#thumbnails)
- new: thumbnail generator has a new option to calculate the row count based on a time interval (https://github.com/stax76/staxrip/wiki/Screenshots#thumbnails)
- new: the preview dialog can be closed in fullscreen mode with a click in the top right corner, same is true for mpv.net
- new: setting added to increase the start size of the preview and fixed caculation for ultra wide displays
- new: the preview shows the time position in the title bar

- fix: ensure the preview starts within the screen bounds

- update: mClean 1.7c

#### 1.6.1.8 unstable test build

- new: nvenc options --vpp-edgelevel and --vpp-unsharp
- new: added timestamps, automatic scaling and background color option to the thumbnail generator, the text for the movie specs is now much compacter

- update: NVEnc 3.21
- update: mpv.net 0.2.1

#### 1.6.1.7 unstable test build

- new: AV1 support completed
- new: MediaInfo properties MasteringDisplay_ColorPrimaries, MasteringDisplay_Luminance, MaxCLL and MaxFALL are read from the source file and the encoder switches --master-display, --range limited and --max-cll are set

- update: aomenc 2017-08-30

#### 1.6.1.6 unstable test build

- new: various new resolutions added to the resize menu
- new: transfer_characteristics added to mediainfo folder view
- new: MediaInfo properties colour_primaries, transfer_characteristics and matrix_coefficients are read from the source file and the encoder switches --colorprim, --transfer and --colormatrix are set
- new: NVEnc --vpp-rff and --vpp-afs switches added

- fix: preview did not work with > 8 bit depth avs scripts
- fix: incorrect VUI flags corected and all flags alphabetically sorted

- usability: the preprocessing settings have better names and the dialog width adjusts automatically (https://github.com/stax76/staxrip/wiki/Screenshots#preprocessing)

- change: improved discovery of required VapourSynth LoadPlugin/SourceFileLoader functions

- new: VapourSynth plugin vsfft3dfilter added (https://github.com/VFR-maniac/VapourSynth-FFT3DFilter)

- update: MediaInfo 0.7.99
- update: mClean 1.7b
- update: AVSMeter 2.6.5
- update: NVEnc 3.19

#### 1.6.1.5 unstable test build

- fix: issue in new progress bar implementation causes too few refresh cycles or no refreshes at all

- new: FrameRateConverter AviSynth script/plugin added (https://github.com/mysteryx93/FrameRateConverter)
- new: new shutdown mode /hybrid (https://www.lifewire.com/shutdown-command-2618100)

- change: TS avc/hvc is by default now remuxed to mkv using ffmpeg
- change: faster start of preview dialog and fix for the startup location not being remembered

- update: AVSMeter 2.6.3

#### 1.6.1.4 unstable test build

- fix: StaxRip didn't ask to install FFTW in case it's required by the active script (https://forum.doom9.org/showthread.php?p=1817732#post1817732)

- new: the job processing dialog additionally to showing the progress as text it shows progress now also with a progress bar (https://github.com/stax76/staxrip/wiki/Screenshots#job-processing)
- new: DeLogo VapourSynth plugin (https://forum.gleitz.info/showthread.php?26177-StaxRip-Encoding-Frontend-(Diskussion)&p=459186&viewfull=1#post459186)
- new: Log File Viewer added, it features a nav/tab bar to eailiy navigate to the different sections of the log file (https://github.com/stax76/staxrip/wiki/Screenshots#log-file-viewer)
- new: DCTFilter v0.5.0 AviSynth plugin added (https://github.com/chikuzen/DCTFilter)

- usability: preview starts with zoom for small resolutions
- cosmetic: DTS-HRA wasn't detected and extracted and displayed as DTS-HD

- update: x265 2.5+14 (https://forum.doom9.org/showthread.php?p=1817514#post1817514)
- update: QSVEnc 2.73 (https://forum.doom9.org/showthread.php?p=1817549#post1817549)
- update: Plugins_JPSDR 1.2.1 (https://forum.doom9.org/showthread.php?p=1817738#post1817738)
- update: mClean 1.5d (https://forum.doom9.org/showthread.php?t=174804)

#### 1.6.1.3 unstable test build

- new: GradCurve VapourSynth plugin
- new: smarter discovery of required AviSynth LoadPlugin calls
- new: FFT3DFilter AviSynth plugin
- new: mClean AviSynth+ script

#### 1.6.1.2 unstable test build

- fix: move down item in job list was broke

- update: mpv.net 0.2
- update: Plugins_JPSDR 1.2.0
- update: AVSMeter 2.6.2

#### 1.6.1.1 unstable test build

- new: eac3to demuxing supports the temp files folder defined in the options (https://github.com/stax76/staxrip/issues/277)
- new: mpv replaced with mpv.net (https://github.com/stax76/mpvnet)
- new: added button to job processing dialog to show the log file (https://github.com/stax76/staxrip/issues/274)

- fix: the OneDrive link had a permission issue (https://github.com/stax76/staxrip/issues/276)
- fix: chapters were extracted even if the option was disabled (https://github.com/stax76/staxrip/issues/267#issuecomment-324134970)
- fix: null ref exception due to subtitle play button being enabled even if no subtitle is selected (https://github.com/stax76/staxrip/issues/273)
- fix: issue in x264 command line generation
- fix: fdkaac cbr bitrate not being saved (https://forum.doom9.org/showthread.php?p=1816917#post1816917)

- update mvtools2 2.7.22

#### 1.6.0.9 unstable test build

- new: x265 switch and flags added but the x265 binary is not updated because it isn't yet merged with the stable branch (https://forum.doom9.org/showthread.php?p=1815427#post1815427)
- new: added setting to enable debug logging to settings dialog, the issue with the main dialog disappearing is probably fixed so at the moment the setting is probably useless (https://github.com/stax76/staxrip/issues/244)

- fix: subtitle cutting did not work for multi stream idx files extracted with vsrip (https://github.com/stax76/staxrip/issues/268)

- change: The dialog to add files for batch processing and for merging is now bigger, resizeable and has automatic scrollbars (https://github.com/stax76/staxrip/issues/271)

- internal: replace mkvmerge --identify-verbose with --identify due to --identify-verbose being deprecated

- update: mkvmerge 15.0.0 (https://forum.doom9.org/showthread.php?p=1815485#post1815485)

#### 1.6.0.8 unstable test build

- new: support for the fdkaac encoder, it's a high quality encoder, due to non free license it's not included, Opus via ffmpeg is included and because it's a great encoder it's now the default encoder!

- fix: audio channel count not always detected correctly

- change: better logging info and change that might or might not fix the issue with the missing window, unfortunately I'm not able to reproudce it.

#### 1.6.0.7 unstable test build

- fix: issue with numeric up/down controls causing rounding problem (bug report via email)
- fix: memory leak causing high memory and crashes while batch processing, I was able to add and process 300 files with 250 MB memory used at the end (bug report via github issue tracker)

- new: nnedi3 replaced by JPSDR plugin pack which is a merge of NNEDI3, Resize MT, and AutoYUY2, I'm getting freezes with the resize mt functions though (request via doom9 forum)

- change: x265 options --refine-intra and --refine-inter changed from boolean to integer (doom9 forum anouncement)
- change: more debug infos added to find the issue that the main window goes missing after eac3to demuxing (bug report via email)

- update: AVSMeter 2.6.1 (doom9 forum anouncement)
- update: MediaInfo 0.7.98 (request via doom9 forum)

#### 1.6.0.6 unstable test build

- change: improved handling of very long paths in order to avoid hitting the max 260 character limitation of Windows 
- change: due to reports that the main window disappears a debug.log file is created in the startup folder

#### 1.6.0.5 unstable test build

- fix: null reference crash when trying to add files for batch processing (https://github.com/stax76/staxrip/issues/262)

- new: VP8 codec added to ffmpeg encoder (https://github.com/stax76/staxrip/issues/259)
- new: added feature to move jobs to the top or bottom in the jobs dialog (request per private message)
- new: added new event after video encoding and macro to get the video encoder name (https://github.com/stax76/staxrip/issues/261)

- update: NVEnc 3.17
- update: nnedi3 0.9.4.45

#### 1.6.0.4 unstable test build

- new: AviSynth+ filter EEDI2 added
- new: added aliases for powershell to support both app filename and name so some can use avsmeter or avsmeter64

- fix: powershell error using: Tools > Advanced > PowerShell
- fix: when the path or version was edited in the apps dialog using the toolbar the display wasn't refreshed
- fix: scroll/caret issue in avisynth/vapoursynth editor editing long lines (https://forum.doom9.org/showthread.php?p=1814553#post1814553)
- fix: the play feature to play avisynth/vapoursynth scripts didn't work if no audio file is selected
- fix: show warning if vapoursynth is used with ffmpeg and mpv, both don't support vapoursynth input

- change: the filters menu in the main window is build only if needed instead of everytime it's shown, this eleminates some issues of the previous implementation, it took two hours but since small things matter it's worth the time

- update: ffmpeg 3.3.3

#### 1.6.0.3 unstable test build

- new: the tray icon shows the progress as tooltip, this was removed before because of implementation problems (email request)

- fix: for avs and vpy source files the detection of the video source file referenced in the script is more reliable (https://github.com/stax76/staxrip/issues/248)
- fix: if avs and vpy files use relative paths for the video source file the temp dir is set to the path of the avs file (https://github.com/stax76/staxrip/issues/248)
- fix: dead link in main menu to the test build page (https://github.com/stax76/staxrip/issues/252)
- fix: 2 bugs in the x265 GUI fixed (https://github.com/stax76/staxrip/issues/255)

- change: the warning that files with the same beginning then other files can't be used for file batch was removed (https://forum.doom9.org/showthread.php?p=1813803#post1813803)

- usability: cmd.exe /S /C "command line" is still used for execution but now only the command line is printed to the logfile

#### 1.6.0.2 unstable test build

- fix: incorrect command line generated for VCEEnc
- fix: opening avs as source staxrip shouldn't ask which source filter to use
- fix: executing the command to run a command line showed an error: Failed to interpret output

- change: environment values of the log file header are cached

#### 1.6.0.1 unstable test build

- new: menus and shortcuts added to the job dialogs for improved management of large jobs lists

- fix: improved handling of m4v files 

- update: NVEnc 3.16
- update: mkvtoolnix 14.0.0

#### 1.6.0.0 stable build

- new: x265 option --force-flush added

- fix: job dialog DPI scaling issue on small screens

- update: mpv 0.26.0
- update: x265 2.5+6

#### 1.5.3.4 unstable test build

- fix: the main window stayed in the background instead of being activated to be top most
- fix: on Turkish locale staxrip used "SSIM".ToLower() which won't result in ssim and thus --tune ssim did not work, it's required to use ToLowerInvariant instead of ToLower, there might be more bugs like this

#### 1.5.3.3 unstable test build

- fix: one of the included powershell scripts was broken due to internal improvements

#### 1.5.3.2 unstable test build

- fix: log files are no longer saved in the target folder if the temp folder is deleted, they can be found at: Tools > Folders > Log Files
- fix: when a job failed it's no longer removed from the joblist
- fix: when job processing starts there is only one Window activation at the start and no subsequent window activations, this prevents to interupt other software like players

#### 1.5.3.1 unstable test build

- fix: shutdown/standby controls were not always visible when they should

#### 1.5.2.9 unstable test build

- new: at the bottom of the video encoder option dialogs there is a menu item to import a command line from the clipboard

#### 1.5.2.8 unstable test build

- new: everytime the settings are saved there is also a backup of the settings saved in the settings folder

- fix: in case of high memory usage like a avisynth filter leaking memory, if staxrip detects more then 1500 MB memory are consumed while jobs are processed it restarts
- fix: job list got wiped when a job was aborted
- fix: if an error happened in a job processing was aborted instead of continuing with other jobs

#### 1.5.2.7 unstable test build

- fix: job processing is now done in the current instance like in the past
- fix: turned out staxrip never had a memory issue but the experimental ffms2 build was leaking memory

#### 1.5.2.6 unstable test build

- new: powershell added to main menu at: Tools > Advanced > PowerShell, it sets the temp dir as work dir and aliases for all tools so you can type something like: ffmpeg -h 

- change: besided dtsma and thd now also eac3, thd+ac3 and dtshr are preferred when staxrip searches for audio files
- change: displayed audio track ID numbers now always start with 1
- change: raw audio formats (thd, eac3, aac) that potentionally don't support seeking are now played without video 

#### 1.5.2.5 unstable test build

- new: qaac has a new option to pipe from ffmpeg instead of converting to FLAC/W64/WAV

- update: ffmpeg 3.3.2
- update: x265 2.5+2

#### 1.5.2.3 unstable test build

- new: aimed quality feature of the old x264 GUI added to the x264 and x265 GUI

- fix: unable to recover processing form from tray
- fix: unable to open new files (start new processes) after the process abortion feature was used 

- update: x265 2.4+99

#### 1.5.2.2 unstable test build

- new: AviSynth+ x64 plugins added: modPlus, SmoothAdjust, AutoAdjust

- update: NNEDI3 0.9.4.44
- update: KNLMeansCL 1.1.0

#### 1.5.2.1 unstable test build

- new: various new features and improvements in the apps management dialog
- new: The jobs dialog will show it's help the first time jobs are started explaining staxrip's parallel processing features

- change: tray icon stays visible even when the job processing window is visible
- change: mpc replaced with mpv

- fix: when automatic demuxing of the video stream is enabled or if demuxing of the video stream was enabled in the demuxing dialog the demuxed video stream wasn't opened afterwards but still the original video was opened

#### 1.5.1.9 unstable test build

- new: context help is now implemented for all video encoders, a right-click on any label, menu or checkbox will show the help for the option, for nvenc, qsvenc and vceenc a new help browser was developed with powershell look and feel
- new: UT Video added for ffmpeg video encoder GUI with options for: -pred (None Left Gradient Median) -pix_fmt (YUV420P YUV422P YUV444P RGB24 RGBA)
- new: help improved for Tags in mp4box container options dialog, the help is runtime generated using the output from mp4box -tag-list
- new: ffmpeg muxing formats menu and profiles added for: asf avi flv ismv mkv mov mp4 mpg mxf nut ogg ts webm wmv
- new: audio target format W64 added with 16/24 bit depth option
- new: nvenc options --cuda-schedule, --perf-monitor, --perf-monitor-interval

- fix: cmd.exe is now used directly without batch files, this improves foreign/special character support in particular on Windows 7
- fix: Jobs Processing window popping up while job processing even when staxrip was previously minimized
- fix: tray icons not cleaned up
- fix: in a few dialogs the help dialog was shown four times instead once
- fix: broken scaling on DPI change using a multi-monitor setup, virtual scaling is used in that case now
- fix: crash when cutting empty subtitles

- change: all classes made public for powershell usage

- update: x264 0.150.2851
- update: nvenc 3.14

#### 1.5.1.8 unstable test build

- fix: unable to show processing window from tray

- update: qsvenc 2.71

#### 1.5.1.7 unstable test build

- new: setting added to define the maximum number of parallel processes (Tools > Settings > General)

- fix: parallel process management hopefully much more stable

- update: mvtools2 2.7.21.22
- update: x265 2.4+96

#### 1.5.1.6 unstable test build

- fix: process management didn't work properly yet

#### 1.5.1.5 unstable test build

- new: parallel audio processing 
- new: added setting to define how many projects to keep under: 'Main Menu > Project > Recent', the setting is located at: Main Menu > Tools > Settings > General 
- new: added new thumbnail generation options
- new: added new feature to archive log files in the settings directory, by default the last 50 log files are keept, this number can be customized at: 'Main Menu > Tools > Settings > General', the folder can be opened with the windows file explorer at: Main Menu > Tools > Folders > Log Files
- new: qsvenc options --fade-detect and --repartition-check

- fix: wrong fps display in main form for 50/60 fps
- fix: version detection disabled for all VC++ runtimes because of issues with file projection via hardlinks and WINSXS
- fix: two issues in x264 command line generation

- change: renamed and reseted setting to minimize to tray
- change: nvenc vpp deband support improved
- change: Option to delete temp files moved to: Tools > Settings > System
- change: scaling and layout improved for numeric up down control and custom menu editor
- change: code refactoring of dynamically generated dialogs (settings/options/codecs), this has let to changed data types and a reset of video encoder profiles
- change: the mkv title tag in the container options isn't overwritten by a title tag of the source file when the source is loaded

- update: mkvtoolnix 13.0.0.0
- update: qsvenc 2.70
- update: AviSynth+ 2508
- update: x265 2.4+89

#### 1.5.1.4 unstable test build

- new: the new x264 GUI is complete, the old is gone
- new: nvenc option --vpp-deband added
- new: x265 option --[no-]refine-vbv added
- new: added new menu to the PAR/DAR option in the container dialog
- fix: in some circumstances file paths with parenthesis failed to process (regression in 1.5.1.1)
- fix: right-click help didn't navigate to help URL on string options in the x265 and the new x264 dialog
- fix: audio settings/profile display was wrong generated if the audio source file has multiple audio streams
- fix: audio was converted to FLAC/W64 even if the output already existed and processing should be skipped (refers to a new feature added with 1.5.0.9)
- change: medium crf value was changed from 22 to 20 for x264 and x265
- update: x265 2.4+87
- update: nvenc 3.13

#### 1.5.1.2 unstable test build

- new: many improvements on the new x264 GUI introduced in 1.5.0.7, video profile settings were reset because at the same time I improve the x265 GUI, the video profile settings will be reset again until the GUI is finished in 1-2 weeks, the old GUI will then be removed
- fix: the feature to reuse existing audio output files from previous job runs had a bug that would result in the wrong file muxed
- fix: the automatic name generation for the audio settings wasn't always working correctly
- tweak: the 'Play audio and video' feature in the menu of the audio source files was changed, it still has limitations but shows a message box to tell the user about
- tweak: in case the option to delete the temp files is used the log file is copied to the target folder
- update: NVEnc 3.12
- update: 2.4+75

#### 1.5.1.1 unstable test build

- new: added option to pre-render slow scripts into a lossless AVI file
- new: improvements on the new x264 GUI introduced in 1.5.0.7
- fix: downmix not used with qaac (regression in 1.5.0.9)

#### 1.5.0.9 unstable test build

- new: to run PowerShell scripts on certain events the Event Command feature is no longer needed, it's documented here: https://github.com/stax76/staxrip#powershell-scripting
- new: there are 2 new options what to do in case the video and audio encoder output files alread exists from a previous job run (reuse, overwrite or ask (default)), the 'Copy/Mux' video encoder profile does alse reuse the output file from previous job runs, in case it don't exist it uses the source video
- new: there is a new option to define which intermediate format should be used in case the audio encoder don't support the input format, supported is FLAC (default) and W64 (WAV > 4 GB)
- new: there is a new option to define which app should be used to create the intermediate audio file (default is ffmpeg)
- new: the MediaInfo window has 'Developer Mode' in the context menu to show the property names for programmers
- tweak: help improved for: Main Menu > Help > Command Line: There is a special mode where only the MediaInfo window is shown using -mediainfo "inputfile", this is useful for Windows File Explorer integration with an app like Open++.

#### 1.5.0.8 unstable test build

- new: download button added to apps dialog's toolbar, only certain apps have a download URL defined
- new: DGIndex added, this was removed before because there wasn't a x64 d2v source filter, now staxrip has x64 d2v filters for both avisynth and vapoursynth included
- new: improvements on the new x264 GUI introduced in 1.5.0.7
- fix: opening a project from CLI had asked to save the current project even if no changes were made since it was loaded
- fix: wrong channel count used with dolby atmos
- fix: the automatically generated audio profile caption in the main dialog of the current profile gets updated when the source changes
- tweak: improved scaling on 96 DPI (relates to layout changes from 1.5.0.4)
- tweak: improved audio specs parameter display in the main dialog
- tweak: the MediaInfo window is startet as separate process, the window can be minimized and maximized, the layout improved
- update: RgTools 0.96
- update: NVEnc 3.11

#### 1.5.0.7 unstable test build

- new: Deblock VapourSynth plugin added: https://github.com/HomeOfVapourSynthEvolution/VapourSynth-Deblock/
- new: basic new x264 GUI based on the same framework then all other codec GUIs, this will soon replace the old GUI 
- new: support for subtitle cutting
- new: added setting to minimze processing window to task bar instead of the tray area
- new: the options for --sar in the encoder options have now the same dropdown menu then the PAR menus in the main dialog
- tweak: the option Fixed Bitrate was removed, if the bitrate or filesize is fixed depends now on what was edited last
- update: masktools2 (AviSynth+) 2.2.10
- update: mvtools (VapourSynth) 19

#### 1.5.0.6 unstable test build

- new: various new x265 options
- new: various new aspect ratio features
- update: x265 2.4+61
- update: QSVEnc 2.66

#### 1.5.0.4 unstable test build

- new: Subtitle Edit added to Apps in main menu and to container options dialog
- new: nvenc --weightp
- tweak: in case the source image width/height isn't mod 4 staxrip writes crop to the crop section instead of the source section and adds a comment 'ensure mod 4'
- tweak: main form layout and scaling improved and increased precession of several values
- update: TIVTC 1.0.9
- update: VapourSynth R38
- update: x265 2.4+36
- update: nvenc 3.10

#### 1.5.0.3 unstable test build

- new: all demuxers have support to demux the video stream
- new: chapters from mkv are extracted in both xml and ogg 
- new: chapter extraction from MP4
- new: added x265 switches --ctu-info and --dhdr10-opt
- fix: mkv chapters were used for mp4box which don't support them
- update: AviSynth r2504

#### 1.5.0.2 unstable test build

- update: AviSynth r2502
- fix: Opening a Blu-ray folder while having already a source file opened mixed various things up
- fix: DPI scaling broken under rare conditions 
- fix: incorrect command line generation for AV1 two pass
- new: video stream demuxing option added to ffmpeg demuxer

#### 1.5.0.1 unstable test build

- new: experimental AV1 codec support
- new: the demux app was removed, the built-in demuxing GUIs for mkvextract, mp4box, eac3to and ffmpeg can be used as independent tool found at: Tools > Advanced > Demux
- fix: FLAC was extracted to mka instead of flac
- tweak: staxrip is a bit smarter to find out if all job processing is completed and shutdown/standby can be performed
- update: MP4Box 0.7.2-DEV-rev79 which fixes issues with qsvenc output
- update: MediaInfo 0.7.96
- update: ffmpeg 3.3.1

# 1.5.0.0 (2017-05-28)

#### Requirements

.NET 4.7 is required on systems prior Windows 10 Version 1703 (Creators Update)

https://www.microsoft.com/en-us/download/details.aspx?id=55170

#### Removed Features

- C# scripting support was removed because it was very heavy requiring 47 nuget packages. You can port existing C# code to PowerShell or load and execute an C# Assembly with PowerShell. Visit the support forum.

#### New Features

- MPEG2DecPlus filter added to open d2v files with AviSynth+
- VCEncC added --check-features --codec --enforce-hrd --filler --fullrange --ltr --pre-analysis --ref --tier --vbaq
- x265 added --me sea --dynamic-rd --scenecut-bias --lookahead-threads --opt-cu-delta-qp --multi-pass-opt-analysis --multi-pass-opt-distortion --multi-pass-opt-rps --aq-motion --ssim-rd --hdr --hdr-opt --refine-level --limit-sao
- added 'Loop Fiter' tab to the x265 options dialog
- icons added to menus, buttons and menu editor
- QSVEnc added --profile main10
- colour_primaries added to MediaInfo Folder View
- NVEnc added --lookahead --cbrhq --vbrhq --aq-temporal --no-b-adapt --i-adapt --output-depth --strict-gop --vbr-quality --vpp-gauss --vpp-knn --vpp-pmd --device --preset --direct --adapt-transform --enable-ltr
- added the possibility to use different x265 options in first and second pass
- added setting 'Snap to desktop edges'
- added option to extract timecodes file from MKV (Options > Misc, disabled by default)
- added timecodes file option to muxer dialog, can be TXT or MKV
- added support for hardcoded subtitles using VapourSynth for srt, ass, idx/vobsub, sup/pgs
- added the possiblity to use special GUI macros like $select:val1;val2$ in filter profiles, this is now used in the defaults for AssumeFPS for instance
- apps dialog will show dependencies for scripts like QTGMC
- flash3kyuu_deband VapourSynth support added
- new macro %eval_ps:<expression>% added to evaluate PowerShell expressions
- various improvements in the batch/cli audio encoder GUI 
- defaults tab added to video filter profile editor and several menu editors
- added option to encode audio with the channels count of the audio source file
- added support for VirtualDub frame server files with vdr extension
- ffmpeg demuxing GUI added, only audio is supported so far, input types can be configured in the settings (Tools > Settings > Demuxing > ffmpeg > Edit), default is avi, ts, flv
- added new powershell script for re-muxing
- added option to define a default subtitle name
- added option to enable a default subtitle
- added possibility to define tags for MKV muxer
- support for using AviSynth filters in VapourSynth, LoadPlugin calls are added automatically
- added cover support for MP4 and MKV muxer
- added forced option for audio streams
- every job is always processed in a new staxrip instance and every instance run only one job and starts then a new instance if necessary

#### Fixed Bugs

- format 'E-AC3 EX' was unknown to eac3to demuxer
- fixed x265 command line generation for --limit-tu
- added missing check if Visual C++ 2012 is installed when masktools2, SangNom2 or VCEEnc are used
- nnedi3 plugin wasn't loaded via LoadPlugin using avs function nnedi3_rpow2
- in the scripting/code editor it was often needed to right-click a second time until the context menu showed
- fixed incompatible format like wmv being passed to mkvmerge and mp4box
- KNLMeansCL wasn't loaded for HAvsFunc/SMDegrain 
- fixed x265 context help using right-click due to changed x265 URL
- fixed wrong stream used for audio encoding using eac3to when audio source file is mkv 
- PCM wasn't demuxed from MP4/MOV files
- shutdown was broken on creators update
- changed x264 qpmin default value from 10 to 0
- fixed more then one hardcoded subtitle is added to the script automatically
- snap to desktop edges not working properly
- fixed bug using UNC paths
- copy to clipboard in MediaInfo dialog did not work
- fixed PowerShell scripts not being executed on the GUI thread
- fixed rare crash Win 7 failing to load included Segoe font file
- fixed issues staxrip using wrong stream IDs for ffmpeg and mkvmerge
- the option to demux all subtitles did only demux preferred subtitles
- not all forced subtitles were picked up
- each code text field in the code editor is not limited to 15 lines to prevent the dialog get's higher then the screen and thus the OK and Cancel button disappearing offscreen
- menu item hights were inconstistent with certain DPI settings

#### Tweaks

- menu item height increased
- added colorspace = "YV12" default everywhere FFVideoSource is used to open 10Bit sources, profiles have not been reset
- higher quality antialiased font rendering added in various places
- ensuring form show within the bounds of the working area (screen)
- reduce CPU time while encoding
- the message for low disk space is shown only once before jobs are created
- idx/srt/ass renderer for avs/vs is now HomeOfVapourSynthEvolution/VSFilterMod
- in case there are more then ten audio files or streams a sub menu is used
- audio file detection prefers now dtsma and thd files 
- filter context menus open a lot quicker
- if the name text box in the code editor is empty the main dialog will show the code
- migrated C# HDR script to PowerShell
- updated VapourSynth's built in filters
- up/down menu items for the avs/vs filter list in main dialog
- x265 --limit-tu defaults for slower and veryslow presets updated
- in case the AviSynth script contains AudioDub the avs file can be selected as audio source file in the audio context menu 
- the manual filter selection dialog is now based on filter profiles and don't show avs and vs filters together but rather avs or vs depending on avs or vs is enabled
- added DGSource to VapourSynth filter profiles
- added mp4box cover art extraction
- filter setup now also support Automatic as source filter which is also the default now
- if the subtitle source is a container like MP4/MKV StaxRip checks the forced and default option
- when staxrip demuxues subtitles it writes _forced to the filename and set the forced flag when it picks up the demuxed subtitles 
- when the source is a container like MP4/MKV the forced and default audio flags are applied
- Cancel in message boxes is now English instead of the system language
- improved DPI scaling, in particular for 96 DPI
- subtitle and audio mkv demuxing was merged to happen in one run
- the execute command line feature in the audio dialog don't close CMD.exe after execution

#### Updated Tools

- QSVEnc 2.62
- VCEEnc 3.06
- HAvsFunc (vs) 2017-03-06
- MediaInfo 0.7.93
- ffmpeg 3.2.2
- mvtools (vs) 18
- KNLMeansCL 1.0.2
- NVEnc 3.07
- RgTools (avs) 0.95
- AviSynth+ 2455
- VapourSynth 37
- VSFilterMod (avs, vs) 4
- flash3kyuu_deband (avs, vs) 2.0.20140721
- TIVTC (avs) 1.0.6
- masktools2 2.2.7
- yadifmod (vs) 10
- L-SMASH-Works (avs) 929
- vslsmashsource (vs) 929
- qaac 2.64
- mkvtoolnix 12.0.0
- ffms2 2.2000 test6
- mvtools2 (avs) 2.7.20.22
- x265 2.4+27
- MP4Box 0.7.2-DEV-rev37
- x264 0.150.2833

# v1.4.0.0-stable (2016-12-12)

#### New Features

- LSFmod added for both AviSynth and VapourSynth, to create the LSFmod default filter profile go to: 'Filters > Profiles > Restore Defaults > OK' or alternatively: 'Tools > Advanced > Reset Setting > AviSynth/VapourSynth Filter Profiles > OK'
- added new commands SetTargetFile and LoadSourceFile
- added setting to define the minimum required disk space and added Continue and Abort option to the message box
- VCEEnc (AMD H.264 encoder) switches added
- SMDegrain added, to create the SMDegrain default filter profile go to: 'Filters > Profiles > Restore Defaults > OK' or alternatively: 'Tools > Advanced > Reset Setting > AviSynth Filter Profiles > OK'
- some x265 changes
- nnedi3 added to VapourSynth, to create the nnedi3 default filter profile go to: 'Filters > Profiles > Restore Defaults > OK' or alternatively: 'Tools > Advanced > Reset Setting > VapourSynth Filter Profiles > OK'
- added setting to reverse mouse wheel video seek direction
- context menu to copy added to media info dialog

#### Fixed Bugs

- fixed bug in audio detection when using a network drive as temp folder
- fixed crash in processing dialog
- fixed bug with automatic generation of audio profile names
- fixed video comparison overwriting the log file
- fixed codepage problem of non western europe locales
- replaced ffmpeg with avs2pipemod for piping to x265 due to a character encoding bug with file names happening on non western locale systems
- fixed various DPI scaling issues

#### Tweaks

- improved audio detection using numeric order instead of alphanumeric order 
- license changed to MIT

#### Updated Tools

- LSFmod 1.9
- havsfunc 23
- aWarpSharp2 2016-06-24
- VCEEnc 2.0
- nnedi3 (avs) 0.9.4.31
- VC++ Runtime 2015
- ffms2 2.23
- qaac 2.61
- avs2pipemod 1.1.1
- MediaInfo 0.7.89
- L-Smash-Works (avs/vs) 911
- yadifmod2 (avs) 0.0.4-1
- KNLMeansCL (avs/vs) 0.7.7
- nnedi3 (vs) 10
- x264 2721
- ffmpeg 3.1.5
- mvtools2 (avs) 2.7.1.22
- MP4Box 0.6.2 current snapshot
- finesharp (vs) 2016-08-21
- havsfunc (vs) 24
- mvtools (vs) 17
- AviSynth+ 2294
- VapourSynth 35
- x265 2.1+69
- MKVToolNix 9.6.0
- NVEnc 3.02
- QSVEnc 2.59

# v1.3.7-stable (2016-06-14)

#### New Features

- added new QSVEnc switches
- added support to use network drive as temp folder
- added yadifmod for VapourSynth. Due to popular request I didn't reset the filter profiles, to make the default filter profile for yadifmod available the filter profiles must be manually reset in the filter profile editor.
- added new command SaveJpgByPath to preview dialog which can be used to customize the menu to save a jpg file to a fixed path without the file browser showing, the path can contain macros.
- added taskbar progress support

#### Fixed Bugs

- fixed the check for enough free space on the disk of the target file, instead of three times the source file one time is now enough
- fixed crash StaxRip checking for enough free space in file batch mode and if target directory is network share
- fixed Umlaute don't work on Windows 7
- fixed many avs/vs scripts are created when the target file name is changed by typing
- fixed feature to run jobs in new instance and changed the behaviour of this feature in that when the jobs complete the instance exits without overwriting the settings

#### Updated Tools

- QSVEnc 2.51
- yadifmod (VapourSynth) r9



# 1.3.6

### New Features

- added setting to prevent the StaxRip window to become the active foreground window if certain applications are currently in the foreground
- added new option to disable audio and subtitle demuxing
- added subtitle formats column to MediaInfo Folder View
- added all missing x265 switches
- added hardware decoding methods to x265 'Other' tab, this will bypass AviSynth though
- added new standalone mkvextract GUI (Apps > Demux)
- added new QSVEnc switches for hardcoded subtitles
- added MP4 support to standalone demux app
- added -avsinfo starting AVSMeter without a source opened
- added support for unicode filenames using VapourSynth, AviSynth don't support it
- added mkv cutting support without encoding
- added MSharpen x64 filter for AviSynth+ x64
- added x264 10-Bit support, binary is not included, StaxRip will ask for the location
- added ffmpeg codecs x264, x265 and ProRes
- added codec help menu to ffmpeg options dialog which shows help for the currently selected codec, it displayes the output queried with 'ffmpeg -h encoder=name'
- added option to define preferred audio languages
- added audio and subtitle demuxing modes 'Show Dialog', 'Preferred', 'All', 'None'
- added x265 changes for v1.9 183
- added new option to automatically add hardcoded subtitle
- added option to define JPEG compression quality in 'Preview > Save JPG' and thumbnail generator

### Fixed Bugs

- custom switches were missing in the Intel Quick Sync encoding GUI for QSVEnc
- fixed crash with ass file by replacing VSFilter with VSFilterMod

### Tweaks

- the batch audio profile uses now always batch execution, the PATH variable knows the location of ffmpeg and eac3to, the temp files folder is set as current directory, if input files is empty all files are excepted 
- the search feature of the apps dialog searches now also the supported filters of plugins
- enabled audio demuxing using MP4Box for mov files
- removed mkvinfo.exe which is 18 MB large and not really needed. It's large due to QT toolkit being used
- mkv audio demuxing happens now all streams together instead of every stream separate, it's much faster now.
- changed filter profiles editor to support both tab and 4 spaces for multiline profiles, tabs are converted to 4 spaces which is the standard in Python and Visual Studio
- log file improvements
- filters list view did not accept drag and drop with source files 
- enabled posibility to remux mov to mp4
- the apps dialog writes the customized versions now to the settings folder so it don't matter if the startup folder has no write access
- demuxed subtitles are now detected by ID with proper numeric order
- improved Dolby Atmos handling 
- added check if enough disk space is available before a job runs
- replaced drop down menus in Event Commands editor with menu based drop downs

### Updated Tools

- ffmpeg 2016-04-25
- mkvtoolnix 9.2.0
- MP4Box 0.6.2
- mvsfunc 7
- mvtools2 2.7.0.22
- nnedi3 0.9.4.21
- StaxRip Toolbox Demux 1.1
- yadifmod2 0.0.4
- AviSynth 1858
- NVEnc 2.07
- L-SMASH-Works 879
- QSVEnc 2.50
- qaac 2.59
- x265 1.9 200
- AVSMeter 2.2.8
- MediaInfo 0.7.86
- x264 2694

#### upcoming, not yet uploaded