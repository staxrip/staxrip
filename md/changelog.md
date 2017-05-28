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
- QSVEncC added --profile main10
- colour_primaries added to MediaInfo Folder View
- NVEncC added --lookahead --cbrhq --vbrhq --aq-temporal --no-b-adapt --i-adapt --output-depth --strict-gop --vbr-quality --vpp-gauss --vpp-knn --vpp-pmd --device --preset --direct --adapt-transform --enable-ltr
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
- added missing check if Visual C++ 2012 is installed when masktools2, SangNom2 or VCEEncC are used
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

# v1.4.0.0-stable (2016-12-12)

#### New Features

- LSFmod added for both AviSynth and VapourSynth, to create the LSFmod default filter profile go to: 'Filters > Profiles > Restore Defaults > OK' or alternatively: 'Tools > Advanced > Reset Setting > AviSynth/VapourSynth Filter Profiles > OK'
- added new commands SetTargetFile and LoadSourceFile
- added setting to define the minimum required disk space and added Continue and Abort option to the message box
- VCEEncC (AMD H.264 encoder) switches added
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
- VCEEncC 2.0
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
- NVEncC 3.02
- QSVEncC 2.59

# v1.3.7-stable (2016-06-14)

#### New Features

- added new QSVEncC switches
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

- QSVEncC 2.51
- yadifmod (VapourSynth) r9



# 1.3.6

### New Features

- added setting to prevent the StaxRip window to become the active foreground window if certain applications are currently in the foreground
- added new option to disable audio and subtitle demuxing
- added subtitle formats column to MediaInfo Folder View
- added all missing x265 switches
- added hardware decoding methods to x265 'Other' tab, this will bypass AviSynth though
- added new standalone mkvextract GUI (Apps > Demux)
- added new QSVEncC switches for hardcoded subtitles
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

- custom switches were missing in the Intel Quick Sync encoding GUI for QSVEncC
- fixed crash with ass file by replacing VSFilter with VSFilterMod

### Tweaks

- the batch audio profile uses now always batch execution, the PATH variable knows the location of ffmpeg, eac3to and BeSweet, the temp files directory is set as current directory, if input files is empty all files are excepted 
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
- NVEncC 2.07
- L-SMASH-Works 879
- QSVEncC 2.50
- qaac 2.59
- x265 1.9 200
- AVSMeter 2.2.8
- MediaInfo 0.7.86
- x264 2694