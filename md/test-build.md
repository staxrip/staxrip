# Test Build

#### Download

https://drive.google.com/open?id=0B-gPKiJYuKuITld4dzhuTC1WWWM

https://onedrive.live.com/redir?resid=604D4754F64B0ABC!4140&authkey=!ANUm9V3vTPmEFNI&ithint=folder%2c7z

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
- added possibility to add hardcoded subtitles using VapourSynth

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

#### Updated Tools

- QSVEncC 2.62
- masktools2 2.2.2
- VCEEncC 3.06
- HAvsFunc 2017-03-06
- x264 2762
- MediaInfo 0.7.93
- ffmpeg 3.2.2
- qaac 2.62
- L-SMASH-Works (avs) 929
- vslsmashsource (vs) 929
- KNLMeansCL 1.0.2
- NVEncC 3.07
- RgTools (avs) 0.95
- AviSynth+ 2455
- VapourSynth 37
- x265 2.3+30
- mkvtoolnix 10.0.0
- ffms2 2.2000 test3
- mvtools (vs) 18
- VSFilterMod (avs, vs) 4