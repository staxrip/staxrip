# Test Build

#### Requirements

.NET 4.7 is required on systems prior Windows 10 Version 1703 (Creators Update)

https://www.microsoft.com/en-us/download/details.aspx?id=55170

#### Download

https://drive.google.com/open?id=0B-gPKiJYuKuITld4dzhuTC1WWWM

https://onedrive.live.com/redir?resid=604D4754F64B0ABC!4140&authkey=!ANUm9V3vTPmEFNI&ithint=folder%2c7z

#### Removed Features

- C# scripting support was removed because it was very heavy requiring 47 nuget packages. You can port existing C# code to PowerShell or load and execute an C# Assembly with PowerShell. Visit the support forum.

#### New Features

- MPEG2DecPlus filter added to open d2v files with AviSynth+
- VCEncC added --check-features --codec --enforce-hrd --filler --fullrange --ltr --pre-analysis --ref --tier --vbaq
- x265 added --me sea --dynamic-rd --scenecut-bias --lookahead-threads --opt-cu-delta-qp --multi-pass-opt-analysis --multi-pass-opt-distortion --multi-pass-opt-rps --aq-motion --ssim-rd --hdr --hdr-opt --refine-level --limit-sao
- added 'Loop Fiter' tab to the x265 options dialog
- icons added to menus and menu editor
- QSVEncC added --profile main10
- colour_primaries added to MediaInfo Folder View
- NVEncC added --lookahead --cbrhq --vbrhq --aq-temporal --no-b-adapt --i-adapt --output-depth --strict-gop --vbr-quality --vpp-gauss --vpp-knn --vpp-pmd --device --preset --direct --adapt-transform --enable-ltr
- added the possibility to use different x265 options in first and second pass
- added setting 'Snap to desktop edges'
- added option to extract timecodes file from MKV (Options > Misc, disabled by default)
- added timecodes file option to muxer dialog, can be TXT or MKV
- added support for hardcoded subtitles using VapourSynth for srt, ass, idx/vobsub, sup/pgs
- added the possiblity to use special GUI macros like $select:val1;val2$ in filter profiles, this is now used in the defaults for AssumeFPS for instance
- added new menu item icons 
- apps dialog will show dependencies for scripts like QTGMC
- flash3kyuu_deband VapourSynth support added
- new macro %eval_ps:<expression>% added to evaluate PowerShell expressions
- various improvements in the batch/cli audio encoder GUI 
- defaults tab added to video filter profile editor and several menu editors
- added option to encode audio with the channels count of the audio source file
- added support for VirtualDub frame server files with vdr extension
- ffmpeg demuxing GUI added, only audio is supported so far, input types can be configured in the settings, default is avi, ts, flv
- added new powershell script for re-muxing
- added option to define a default subtitle name
- added option to enable a default subtitle
- added possibility to define tags for MKV muxer
- support for using AviSynth filters in VapourSynth
- added cover support for MP4 and MKV muxer
- added forced option for audio streams
- icons added to various buttons

#### Fixed Bugs

- format 'E-AC3 EX' was unknown to eac3to demuxer
- fixed x265 command line generation for --limit-tu
- added missing check if Visual C++ 2012 is installed when masktools2, SangNom2 or VCEEncC are used
- nnedi3 wasn't loaded using avs nnedi3_rpow2
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

#### Tweaks

- menu item height increased
- added colorspace = "YV12" default everywhere FFVideoSource is used to open 10Bit sources, profiles have not been reset
- antialiased font rendering added in some places
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
- simplified mkvmerge command line
- menu items had different height under certain circumstances
- the manual filter selection dialog is now based on filter profiles and don't show avs and vs filters together but rather avs or vs depending on avs or vs is enabled
- added DGSource to VapourSynth filter profiles
- added mp4box cover art extraction
- filter setup now also support Automatic as source filter which is also the default now
- every job is always processed in a new staxrip instance
- if the subtitle source is a container like MP4/MKV StaxRip checks the forced and default option
- when staxrip demuxues subtitles it writes _forced to the filename and set the forced flag when it picks up the demuxed subtitles 
- when the source is a container like MP4/MKV the forced and default audio flags are applied
- Cancel in message boxes is now English instead of the system language
- improved DPI scaling for 96 DPI
- subtitle and audio mkv demuxing was merged to happen in one go

#### Updated Tools

- QSVEncC 2.62
- VCEEncC 3.06
- HAvsFunc (vs) 2017-03-06
- MediaInfo 0.7.93
- ffmpeg 3.2.2
- mvtools (vs) 18
- KNLMeansCL 1.0.2
- NVEncC 3.07
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